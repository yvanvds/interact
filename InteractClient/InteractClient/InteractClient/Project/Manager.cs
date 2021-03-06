﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using InteractClient.IO;

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
				appCacheFolder = await rootFolder.GetFolder("ProjectCache");
			}

			var folders = await appCacheFolder.GetFolders();

			foreach (var folder in folders)
			{
				string folderName = folder.Name;
				var info = Global.Settings.GetProjectInfo(folderName);
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
			IFolder appCacheFolder = await rootFolder.GetFolder("ProjectCache");

			await appCacheFolder.FolderDelete(projectInfo.Guid);

			if (Global.ProjectList.ContainsKey(projectInfo.Guid))
			{
				Global.ProjectList.Remove(projectInfo.Guid);
			}
			Global.Settings.RemoveProjectInfo(projectInfo.Guid);
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
			catch(Exception e)
			{
				Network.Sender.WriteLog("Project->SetCurrent: Unable to add project - " + e.Message);
				Global.SetScreenMessage("Project failed to load: " + e.Message);
			}
		}
	}
}
