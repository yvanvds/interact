using PCLStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Project
{
	public class Manager
	{
		private ObservableCollection<Info> list = new ObservableCollection<Info>();
		public ObservableCollection<Info> List => list;

		public async Task Load()
		{
			// create application folder if it does not exist
			IFolder appCacheFolder = null;
			{
				IFolder rootFolder = FileSystem.Current.LocalStorage;
				await rootFolder.CreateFolderAsync("ProjectCache", CreationCollisionOption.OpenIfExists).ContinueWith(createFolderTask =>
				{
					createFolderTask.Wait();
					appCacheFolder = createFolderTask.Result;
				});
			}

			var folders = await appCacheFolder.GetFoldersAsync();

			foreach (var folder in folders)
			{
				string folderName = folder.Name;
				var info = Acr.Settings.CrossSettings.Current.Get<Info>(folderName);
				if (info == null)
				{
					info = new Info();
					info.Name = "Unknown";
					info.Origin = "Unknown";
				}
				info.Guid = folderName;
				list.Add(info);
			}
		}

		public async Task DeleteFromDisk(Info projectInfo)
		{
			IFolder rootFolder = FileSystem.Current.LocalStorage;
			IFolder appCacheFolder = await rootFolder.GetFolderAsync("ProjectCache");

			var folder = await appCacheFolder.GetFolderAsync(projectInfo.Guid);
			await folder.DeleteAsync();

			if (Global.ProjectList.ContainsKey(projectInfo.Guid))
			{
				Global.ProjectList.Remove(projectInfo.Guid);
			}
			Acr.Settings.CrossSettings.Current.Remove(projectInfo.Guid);
			list.Remove(projectInfo);
		}

		public static async Task SetCurrent(string id, int version)
		{
			if(Global.ProjectList.ContainsKey(id))
			{
				Global.CurrentProject = Global.ProjectList[id];
				Global.CurrentProject.IsLocal = version == -1 ? true : false;

				if(!Global.CurrentProject.IsLocal)
				{
					if(Global.CurrentProject.Version != version)
					{
						Network.Sender.WriteLog("Project->SetCurrent: project " + id + " found. Requested update.");
						await Global.CurrentProject.UpdateProject(version);
						Network.Sender.ProjectUpdateReady(id);
					} else
					{
						Network.Sender.WriteLog("Project->SetCurrent: project " + id + " found and up to date.");
						Network.Sender.ProjectUpdateReady(id);
					}
				}

				// activate patchers


				Global.SetScreenMessage("Project is Ready");
				return;
			}

			// create new project
			try
			{
				Global.CurrentProject = new Project(id, version);
				bool local = version == -1 ? true : false;
				Global.CurrentProject.IsLocal = local;
				await Global.CurrentProject.Cache.Init(local);
				Global.ProjectList.Add(id, Global.CurrentProject);

				await Global.CurrentProject.UpdateProject(version);
				if(!local)
				{
					Network.Sender.ProjectUpdateReady(id);
					Global.SetScreenMessage("Project is Ready");
				}
			} 
			catch(NullReferenceException)
			{
				if(!Global.CurrentProject.IsLocal)
				{
					Network.Sender.WriteLog("Project->SetCurrent: Unable to add project");
				}
				Global.SetScreenMessage("Project failed to load!");
			}
		}
	}
}
