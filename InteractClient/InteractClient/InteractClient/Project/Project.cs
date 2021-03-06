﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InteractClient.Project
{
	public class Project
	{
		public string Name { get; set; }
		public string ID { get; set; }
		public int Version { get; set; }
		public string StartupScreen { get; set; }
		public string GroupID { get; set; }
		private SensorModule currentSensorModule = null;
		private ArduinoModule currentArduinoModule = null;

		public Dictionary<string, BaseModule> ClientModules = new Dictionary<string, BaseModule>();

		public bool IsLocal = false;
		public Cache Cache;

		private bool running = false;
		public bool Running { get => running; }

		public Project(string id, int version)
		{
			this.ID = id;
			this.Version = version;
			this.Cache = new Cache(id);
		}

		public async Task UpdateProject(int version)
		{
			await Cache.UpdateProject();
			Name = Cache.Name;

			await Cache.RefreshClientModules(ClientModules);
			Version = version;

			var info = Global.Settings.GetProjectInfo(ID);
			if (info == null)
			{
				info = new Info
				{
					Name = Name,
					Origin = Network.Sender.ServerName,
					FirstScreen = StartupScreen,
					DownloadDate = DateTime.Now,
					LastUseDate = DateTime.Now
				};
			}
			else
			{
				info.LastUseDate = DateTime.Now;
				info.Name = Name;
				info.Origin = Network.Sender.ServerName;
				info.FirstScreen = StartupScreen;
			}

			Global.Settings.SetProjectInfo(ID.ToString(), info);
		}

		public BaseModule GetClientModule(string ID)
		{
			if (ClientModules.ContainsKey(ID))
			{
				return ClientModules[ID];
			}
			return null;
		}

		public BaseModule GetClientModuleByName(string name)
		{
			foreach(var module in ClientModules.Values)
			{
				if(module.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
				{
					return module;
				}
			}
			return null;
		}

		public void Start()
		{
			List<string> scripts = new List<string>();
			foreach(var module in ClientModules.Values)
			{
				if(module is PatcherModule)
				{
					(module as PatcherModule).EnableAudio();
				}

				else if (module is ScriptModule)
				{
					scripts.Add((module as ScriptModule).Content);
				}
			}

			if(scripts.Count > 0)
			{
				Network.Sender.WriteLog("Running Scripts...");
				Global.Compiler.Run();
			}

			StopSensors();
			StopArduino();

			foreach (var module in ClientModules.Values)
			{
				if(module is SensorModule)
				{
                    if (Device.RuntimePlatform != Device.UWP)
                    {
                        var mod = module as SensorModule;
                        if (mod.GroupID.Equals(GroupID))
                        {
                            currentSensorModule = mod;
                            currentSensorModule.Activate();
                        }
                    }
                        
				} else if (module is ArduinoModule)
				{
					var mod = module as ArduinoModule;
					if(mod.GroupID.Equals(GroupID))
					{
						currentArduinoModule = mod;
						currentArduinoModule.Activate();
					}
				}
			}

			running = true;
		}

		public void Stop()
		{
			running = false;

			StopSensors();
			StopArduino();

			foreach (var module in ClientModules.Values)
			{
				if (module is PatcherModule)
				{
					(module as PatcherModule).DisableAudio();
				}
			}
			Global.Compiler.StopAssembly();
		}

		private void StopSensors()
		{
			if (currentSensorModule != null)
			{
				currentSensorModule.Deactivate();
				currentSensorModule = null;
			}
			Global.Sensors?.StopAll();
		}

		private void StopArduino()
		{
			if(currentArduinoModule != null)
			{
				currentArduinoModule.Deactivate();
				currentArduinoModule = null;
			}
			
		}
	}
}
