using InteractClient.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
				await rootFolder.CreateFolderAsync("ProjectCache", CreationCollisionOption.OpenIfExists).ContinueWith(createFolderTask =>
				{
					createFolderTask.Wait();
					appCacheFolder = createFolderTask.Result;
				});
			}

			// create project folder if it does not exist
			await appCacheFolder.CreateFolderAsync(projectID.ToString(), CreationCollisionOption.OpenIfExists).ContinueWith(createFolderTask =>
			{
				createFolderTask.Wait();
				projectFolder = createFolderTask.Result;
			});

			// load last configuration if available
			var path = Path.Combine(projectFolder.Path, projectID.ToString());
			var exists = await projectFolder.CheckExistsAsync(projectID.ToString());
			if (exists == ExistenceCheckResult.FileExists)
			{
				string content = await GetFileContent(path);
				JObject obj = JObject.Parse(content);
				if (obj.ContainsKey("Name")) Name = (string)obj["Name"];
				if (obj.ContainsKey("Version")) Version = (int)obj["Version"];
				if (obj.ContainsKey("ID")) projectID = (string)obj["ID"];
				if(obj.ContainsKey("Client Modules"))
				{
					JObject cMods = obj["Client Modules"] as JObject;
					ClientModules.Clear();
					foreach(var mod in cMods)
					{
						ClientModules.Add(mod.Key, (int)mod.Value);
					}
				}
			}
		}

		public async Task UpdateProject()
		{
			downloadTasks.Clear();
			string path = await GetContentLocation(projectID, true);
			if(path != string.Empty)
			{
				string content = await GetFileContent(path);
				JObject obj = JObject.Parse(content);
				if (obj.ContainsKey("Name")) Name = (string)obj["Name"];
				if (obj.ContainsKey("Version")) Version = (int)obj["Version"];
				if(obj.ContainsKey("Client Modules"))
				{
					JObject cMods = obj["Client Modules"] as JObject;
					await UpdateResource(cMods, ClientModules);
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
					IFile file = await projectFolder.GetFileAsync(item.Key);
					await file.DeleteAsync();
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
					string content = await GetFileContent(Path.Combine(projectFolder.Path, module.Key));
					var data = JObject.Parse(content);
					if(data.ContainsKey("Type"))
					{
						var Type = (string)data["Type"];
						switch(Type)
						{
							case "ClientGui":
								{
									var m = new GuiModule();
									m.Deserialize(data);
									m.LoadContent();
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
						}
					}
				}
				else if (clientModules[id].Version != Convert.ToInt32(module.Value))
				{
					string content = await GetFileContent(Path.Combine(projectFolder.Path, module.Key));
					clientModules[id].Deserialize(JObject.Parse(content));
					clientModules[id].LoadContent();
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
			bool containsScripts = false;
			foreach(var module in clientModules.Values)
			{
				if (module is ScriptModule) containsScripts = true;
			}
			if(containsScripts)
			{
				var file = File.ReadAllBytes(Path.Combine(projectFolder.Path, "ClientScript.dll"));
				Global.Compiler.LoadAssembly(file);
			}
		}

		public async Task<string> GetContentLocation(string id, bool reload = false)
		{
			try
			{
				var path = Path.Combine(projectFolder.Path, id);

				if (reload == false)
				{
					var exists = await projectFolder.CheckExistsAsync(id);
					if (exists == ExistenceCheckResult.FileExists && !downloadTasks.ContainsKey(path))
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

			IFile file = null;
			try
			{
				var client = new HttpClient();
				var data = await client.GetByteArrayAsync(url);
				var fileNamePaths = fileName.Split('\\');
				fileName = fileNamePaths[fileNamePaths.Length - 1];
				file = await FileSystem.Current.LocalStorage.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

				using (var fileStream = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
				{
					fileStream.Write(data, 0, data.Length);
				}
				return true;
			}
			catch (Exception e)
			{
				Sender.WriteLog("ProjectCache download error: " + e.Message);
			}

			if (file != null)
			{
				await file.DeleteAsync();
			}
			return false;
		}

		public static async Task<string> GetFileContent(string path)
		{
			string content = string.Empty;
			try
			{
				IFile file = await FileSystem.Current.LocalStorage.GetFileAsync(path);
				using (var fileStream = await file.OpenAsync(PCLStorage.FileAccess.Read))
				{
					using (var reader = new StreamReader(fileStream, Encoding.UTF8))
					{
						content = reader.ReadToEnd();
					}
				}
			}
			catch (Exception e)
			{
				Sender.WriteLog("ProjectCache file error: " + e.Message);
			}

			return content;
		}
	}
}
