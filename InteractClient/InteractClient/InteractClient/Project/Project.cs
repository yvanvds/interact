using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

			var info = Acr.Settings.CrossSettings.Current.Get<Info>(ID);
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

			Acr.Settings.CrossSettings.Current.Set(ID.ToString(), info);
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

			foreach (var module in ClientModules.Values)
			{
				if(module is SensorModule)
				{
					var mod = module as SensorModule;
					if(mod.GroupID.Equals(GroupID))
					{
						currentSensorModule = mod;
						currentSensorModule.Activate();
					}
				}
			}

			running = true;
		}

		public void Stop()
		{
			running = false;

			StopSensors();
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
			Global.Sensors.StopAll();
		}
	}
}
