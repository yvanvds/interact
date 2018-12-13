using InteractClient.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InteractClient.IO;
using Xamarin.Forms;

namespace InteractClient.Project
{
	public class Cache
	{
		private static IFolder appCacheFolder = null;
		private IFolder projectFolder = null;

		private Dictionary<string, Task<bool>> downloadTasks = new Dictionary<string, Task<bool>>();
		private object locker = new object();

		private string projectID;
		public string Name { get; set; }
		public int Version { get; set; }

		private bool ProjectIsLocal = false;

		private Dictionary<string, int> clientModules = new Dictionary<string, int>();
		public Dictionary<string, int> ClientModules => clientModules;

		public Cache(string projectID)
		{
			this.projectID = projectID;
		}

		public async Task Init(bool projectIsLocal = false)
		{
			this.ProjectIsLocal = projectIsLocal;

			// create application folder if it does not exist
			if (appCacheFolder == null)
			{
				IFolder rootFolder = FileSystem.Current.LocalStorage;
				appCacheFolder = await rootFolder.GetFolder("ProjectCache");
				if(appCacheFolder == null)
				{
					return;
				}
			}

			// create project folder if it does not exist
			projectFolder = await appCacheFolder.GetFolder(projectID.ToString());

			// load last configuration if available
			var path = Path.Combine(projectFolder.Path, projectID.ToString());

			if (projectFolder.FileExists(projectID.ToString()))
			{
				string content = await GetFileContent(projectFolder, projectID.ToString());
				try
				{
					JObject obj = JObject.Parse(content);
					if (obj.ContainsKey("Name")) Name = (string)obj["Name"];
					if (obj.ContainsKey("Version")) Version = (int)obj["Version"];
					if (obj.ContainsKey("ID")) projectID = (string)obj["ID"];
					if (obj.ContainsKey("Client Modules"))
					{
						JObject cMods = obj["Client Modules"] as JObject;
						ClientModules.Clear();
						foreach (var mod in cMods)
						{
							ClientModules.Add(mod.Key, (int)mod.Value);
						}
					}
				} catch (Exception e)
				{
					Sender.WriteLog("ProjectCache error: " + e.Message);
				}
				
			}
		}

		public async Task UpdateProject()
		{
			downloadTasks.Clear();
			string path = await GetContentLocation(projectID, true);
			if(path != string.Empty)
			{
				string content = await GetFileContent(projectFolder, projectID);

				try
				{
					JObject obj = JObject.Parse(content);
					if (obj.ContainsKey("Name")) Name = (string)obj["Name"];
					if (obj.ContainsKey("Version")) Version = (int)obj["Version"];
					if (obj.ContainsKey("Client Modules"))
					{
						JObject cMods = obj["Client Modules"] as JObject;
						await UpdateResource(cMods, ClientModules);
					}
				} catch(Exception e)
				{
					Sender.WriteLog("Cache Parsing Error: " + e.Message);
				}
				
			}
		}

		public async Task UpdateResource(JObject cMods, Dictionary<string,int> resource)
		{
			// add or update
			foreach(var item in cMods)
			{
				if(!resource.ContainsKey(item.Key) || resource[item.Key] != (int)item.Value)
				{
					await GetContentLocation(item.Key, true);
				}
			}
			await GetContentLocation("ClientScript.dll", true);

			// delete old files
			foreach(var item in resource)
			{
				if(!cMods.ContainsKey(item.Key))
				{
					await projectFolder.FileDelete(item.Key);
				}
			}

			resource.Clear();
			foreach(var item in cMods)
			{
				resource.Add(item.Key, (int)item.Value);
			}
		}

		public async Task RefreshClientModules(Dictionary<string, BaseModule> clientModules)
		{
			foreach (var module in this.clientModules)
			{
				string id = module.Key;
				if (!clientModules.ContainsKey(id))
				{
					string content = await GetFileContent(projectFolder, module.Key);
					JObject data = null;
					try
					{
						data = JObject.Parse(content);
					}
					catch(Exception e)
					{
						Network.Sender.WriteLog("Error while loading module " + module.Key + ": " + e.Message);
					}
					if(data != null && data.ContainsKey("Type"))
					{
						var Type = (string)data["Type"];
						switch(Type)
						{
							case "ClientGui":
								{
									var m = new GuiModule();
									m.Deserialize(data);
									Global.RunOnGui(() =>
									{
										m.LoadContent();
									});
									
									clientModules.Add(id, m);
									break;
								}
							case "ClientPatcher":
								{
									var m = new PatcherModule();
									m.Deserialize(data);
									m.LoadContent();
									clientModules.Add(id, m);
									break;
								}
							case "ClientScript":
								{
									var m = new ScriptModule();
									m.Deserialize(data);
									clientModules.Add(id, m);
									break;
								}
							case "ClientSensors":
								{
									var m = new SensorModule();
									m.Deserialize(data);
									m.LoadContent();
									clientModules.Add(id, m);
									break;
								}
							case "ClientArduino":
								{
									var m = new ArduinoModule();
									m.Deserialize(data);
									m.LoadContent();
									clientModules.Add(id, m);
									break;
								}
						}
					}
				}
				else if (clientModules[id].Version != Convert.ToInt32(module.Value))
				{
					string content = await GetFileContent(projectFolder, module.Key);
					if(content != string.Empty)
					{
						clientModules[id].Deserialize(JObject.Parse(content));
						Global.RunOnGui(() =>
						{
							clientModules[id].LoadContent();
						});
					}
					
				}
			}

			var itemsToRemove = clientModules.Where(item => !this.ClientModules.ContainsKey(item.Key.ToString())).ToArray();
			foreach (var item in itemsToRemove)
			{
				if(item.Value is PatcherModule)
				{
					(item.Value as PatcherModule).Dispose();
				}
				clientModules.Remove(item.Key);
			}

            //check for scripts
            if (Device.RuntimePlatform != Device.UWP)
            {
                bool containsScripts = false;
                foreach (var module in clientModules.Values)
                {
                    if (module is ScriptModule) containsScripts = true;
                }
                if (containsScripts)
                {
                    var file = File.ReadAllBytes(Path.Combine(projectFolder.Path, "ClientScript.dll"));
                    Global.Compiler.LoadAssembly(file);
                }
            }
                
		}

		public async Task<string> GetContentLocation(string id, bool reload = false)
		{
			try
			{
				var path = Path.Combine(projectFolder.Path, id);

				if (reload == false)
				{
					if (projectFolder.FileExists(id) && !downloadTasks.ContainsKey(path))
					{
						return path;
					}
				}

				string url = "http://" + Sender.ServerAddress + ":11235/" + id;
				var success = await download(url, path);
				return success ? path : "";
			}
			catch (Exception e)
			{
				Sender.WriteLog("ProjectCache error: " + e.Message);
				return "";
			}
		}

		private Task<bool> download(string url, string path)
		{
			lock (locker)
			{
				Task<bool> task;
				if (downloadTasks.TryGetValue(path, out task))
					return task;

				downloadTasks.Add(path, task = downloadAndSaveFile(url, path));
				return task;
			}
		}

		private async Task<bool> downloadAndSaveFile(string url, string fileName)
		{
			if (ProjectIsLocal) return true;

			try
			{
				var client = new HttpClient();
				var data = await client.GetByteArrayAsync(url);
				var fileNamePaths = fileName.Split('\\');
				var name = fileNamePaths[fileNamePaths.Length - 1];
				await projectFolder.FileWrite(name, data);
				return true;
			}
			catch (Exception e)
			{
				Sender.WriteLog("ProjectCache download error on file " + fileName + " : " + e.Message);
			}
			return false;
		}

		public static async Task<string> GetFileContent(IFolder folder, string filename)
		{
			string content = string.Empty;
			try
			{
				content = await folder.FileRead(filename);
			}
			catch (Exception e)
			{
				Sender.WriteLog("ProjectCache file error: " + e.Message);
			}

			return content;
		}
	}
}
