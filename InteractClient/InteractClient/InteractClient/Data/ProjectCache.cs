using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InteractClient.Network;
using Newtonsoft.Json;
using PCLStorage;

namespace InteractClient.Data
{
	public class ProjectCache
	{
		private static IFolder appCacheFolder = null;
		private IFolder projectFolder = null;

		private Dictionary<string, Task<bool>> downloadTasks = new Dictionary<string, Task<bool>>();
		private object locker = new object();

		private Guid projectID;

		private Dictionary<string, string> config = new Dictionary<string, string>();
		public Dictionary<string, string> Config => config;

		private Dictionary<string, string> screens = new Dictionary<string, string>();
		public Dictionary<string, string> Screens => screens;

		private Dictionary<string, string> images = new Dictionary<string, string>();
		public Dictionary<string, string> Images => images;

		private Dictionary<string, string> patchers = new Dictionary<string, string>();
		public Dictionary<string, string> Patchers => patchers;

		private Dictionary<string, string> soundfiles = new Dictionary<string, string>();
		public Dictionary<string, string> SoundFiles => soundfiles;

		public ProjectCache(Guid projectID)
		{
			this.projectID = projectID;
		}

		public async Task Init()
		{
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
				Dictionary<string, Dictionary<string, string>> values = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(content);

				config = values["config"];
				screens = values["screens"];
				images = values["images"];
				patchers = values["patchers"];
				soundfiles = values["soundfiles"];
			}	
		}

		public async Task UpdateProject()
		{
			string path = await GetContentLocation(projectID, true);
			if(path != string.Empty)
			{
				string content = await GetFileContent(path);
				Dictionary<string, Dictionary<string, string>> values = JsonConvert.DeserializeObject< Dictionary<string, Dictionary<string, string>> >(content);

				config = values["config"];

				Dictionary<string, string> newScreenList = values["screens"];
				await UpdateResource(newScreenList, screens);

				Dictionary<string, string> newImagesList = values["images"];
				await UpdateResource(newImagesList, images);

				Dictionary<string, string> newPatchersList = values["patchers"];
				await UpdateResource(newPatchersList, patchers);

				Dictionary<string, string> newSoundfilesList = values["soundfiles"];
				await UpdateResource(newSoundfilesList, soundfiles);
			}
		}

		private async Task UpdateResource(Dictionary<string, string> newDict, Dictionary<string, string> oldDict)
		{
			// add or update
			foreach (var item in newDict)
			{
				if(!oldDict.ContainsKey(item.Key) || oldDict[item.Key] != item.Value)
				{
					await GetContentLocation(new Guid(item.Key), true);
				}
			}

			// delete old files
			foreach(var item in oldDict)
			{
				if(!newDict.ContainsKey(item.Key))
				{
					IFile file = await projectFolder.GetFileAsync(item.Key);
					await file.DeleteAsync();
				}
			}

			oldDict = newDict;
		}

		public async Task RefreshScreens(Dictionary<Guid, Screen> projectScreens)
		{
			foreach(var item in screens)
			{
				Guid id = new Guid(item.Key);
				if (!projectScreens.ContainsKey(id)) {
					Screen s = new Screen();
					string content = await GetFileContent(Path.Combine(projectFolder.Path, item.Key));
					s.Deserialize(content);
					projectScreens.Add(id, s);
				} else if (projectScreens[id].Version != Convert.ToInt32(item.Value))
				{
					string content = await GetFileContent(Path.Combine(projectFolder.Path, item.Key));
					projectScreens[id].Deserialize(content);
				}
			}

			var itemsToRemove = projectScreens.Where(item => !screens.ContainsKey(item.Key.ToString())).ToArray();
			foreach (var item in itemsToRemove)
				projectScreens.Remove(item.Key);
		}

		public async Task RefreshImages(Dictionary<Guid, Image> projectImages)
		{
			foreach (var item in images)
			{
				Guid id = new Guid(item.Key);
				if (!projectImages.ContainsKey(id))
				{
					Image s = new Image();
					string content = await GetFileContent(Path.Combine(projectFolder.Path, item.Key));
					s.Deserialize(content);
					projectImages.Add(id, s);
				}
				else if (projectImages[id].Version != Convert.ToInt32(item.Value))
				{
					string content = await GetFileContent(Path.Combine(projectFolder.Path, item.Key));
					projectImages[id].Deserialize(content);
				}
			}

			var itemsToRemove = projectImages.Where(item => !images.ContainsKey(item.Key.ToString())).ToArray();
			foreach (var item in itemsToRemove)
				projectImages.Remove(item.Key);
		}

		public async Task RefreshPatchers(Dictionary<Guid, Patcher> projectPatchers)
		{
			foreach (var item in patchers)
			{
				Guid id = new Guid(item.Key);
				if (!projectPatchers.ContainsKey(id))
				{
					Patcher s = new Patcher();
					string content = await GetFileContent(Path.Combine(projectFolder.Path, item.Key));
					s.Deserialize(content);
					projectPatchers.Add(id, s);
				}
				else if (projectPatchers[id].Version != Convert.ToInt32(item.Value))
				{
					string content = await GetFileContent(Path.Combine(projectFolder.Path, item.Key));
					projectPatchers[id].Deserialize(content);
				}
			}

			var itemsToRemove = projectPatchers.Where(item => !patchers.ContainsKey(item.Key.ToString())).ToArray();
			foreach (var item in itemsToRemove)
				projectPatchers.Remove(item.Key);
		}

		public async Task RefreshSounds(Dictionary<Guid, SoundFile> projectSounds)
		{
			foreach (var item in soundfiles)
			{
				Guid id = new Guid(item.Key);
				if (!projectSounds.ContainsKey(id))
				{
					SoundFile s = new SoundFile();
					string content = await GetFileContent(Path.Combine(projectFolder.Path, item.Key));
					s.Deserialize(content);
					projectSounds.Add(id, s);
				}
				else if (projectSounds[id].Version != Convert.ToInt32(item.Value))
				{
					string content = await GetFileContent(Path.Combine(projectFolder.Path, item.Key));
					projectSounds[id].Deserialize(content);
				}
			}

			var itemsToRemove = projectSounds.Where(item => !soundfiles.ContainsKey(item.Key.ToString())).ToArray();
			foreach (var item in itemsToRemove)
				projectSounds.Remove(item.Key);
		}

		public async Task<string> GetContentLocation(Guid id, bool reload = false)
		{
			try
			{
				var path = Path.Combine(projectFolder.Path, id.ToString());

				if(reload == false)
				{
					var exists = await projectFolder.CheckExistsAsync(id.ToString());
					if (exists == ExistenceCheckResult.FileExists && !downloadTasks.ContainsKey(path))
					{
						return path;
					}
				}

				string url = "http://" + Sender.Get().ServerAddress + ":11235/" + id.ToString();
				var success = await download(url, path);
				return success ? path : "";
			}
			catch (Exception e)
			{
				Sender.Get().WriteLog("ProjectCache error: " + e.Message);
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
				Sender.Get().WriteLog("ProjectCache download error: " + e.Message);
			}

			if(file != null)
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
			} catch (Exception e)
			{
				Sender.Get().WriteLog("ProjectCache file error: " + e.Message);
			}
			
			return content;
		}
	}
}

