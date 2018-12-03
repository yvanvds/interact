using Newtonsoft.Json.Linq;
using OscGuiControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project
{
	public class Project : OscGuiControl.IPropertyInterface
	{
		#region PropertyInterface
		static private PropertyCollection properties = null;
		public PropertyCollection Properties => properties;

		static Project()
		{
			properties = new PropertyCollection();
			properties.Add("Name", "Name");
			properties.Add("FirstClientGui", "Gui on Startup");
		}

		#endregion PropertyInterface

		#region Static
		private static Project current = null;
		public static Project Current => current;

		public static void CreateProject(string folderName, string diskName, string projectName)
		{
			
			string projectPath = Path.Combine(folderName, diskName);
			Directory.CreateDirectory(projectPath);

			string filePath = Path.Combine(projectPath, diskName + ".intp");

			JObject projectFile = new JObject();
			projectFile["Name"] = projectName;
			projectFile["ID"] = shortid.ShortId.Generate(true);

			try
			{
				File.WriteAllText(filePath, projectFile.ToString());
				current = new Project(filePath);
				current.Save();
			}
			catch (Exception e)
			{
				Dialogs.Error.Show("Error on " + e.TargetSite, e.Message);
			}
		}

		public static void OpenProject(string path)
		{
			if(Current != null) Current.Close();

			try
			{
				current = new Project(path);
				current.RecompileScripts();
				InteractServer.Properties.Settings.Default.LastOpenProject = path;
				InteractServer.Properties.Settings.Default.Save();

				Network.Resolume.Handle.Connect(current.ResolumeIP, current.ResolumePort);
			}
			catch (Exception e)
			{
				Dialogs.Error.Show("Error on " + e.TargetSite, e.Message);
			}
		}
		#endregion Static

		private string name = string.Empty;
		public string Name
		{
			get => name;
			set
			{
				name = value;
				needsSaving = true;
			}
		}
		private string id;
		public string ID => id;

		private string filePath;
		private string projectPath;

		private bool valid = false;
		public bool Valid => valid;

		private string firstClientGui = string.Empty;
		public string FirstClientGui
		{
			get
			{
				var module = ClientModules.Get(firstClientGui);
				if (module != null) return module.Name;
				return string.Empty;
			}
			set
			{
				var module = ClientModules.GetByName(value);
				if (module != null) firstClientGui = module.ID;
				else firstClientGui = string.Empty;
				needsSaving = true;
			}
		}
		public string FirstClientGuiID => firstClientGui;

		private string resolumeIP = string.Empty;
		public string ResolumeIP
		{
			get => resolumeIP;
			set
			{
				resolumeIP = value;
				needsSaving = true;
			}
		}

		private int resolumePort = 7000;
		public int ResolumePort
		{
			get => resolumePort;
			set
			{
				resolumePort = value;
				needsSaving = true;
			}
		}

		private int version = 0;

		public Compiler.Intellisense Intellisense = new Compiler.Intellisense();
		public Compiler.ServerCompiler ServerCompiler = new Compiler.ServerCompiler();
		public Compiler.ClientCompiler ClientCompiler = new Compiler.ClientCompiler();

		public EndpointWriter ServerEndpointWriter;
		public EndpointWriter ClientEndpointWriter;

		public ClientGroups Groups = new ClientGroups();

		public Project(string filePath)
		{
			this.filePath = filePath;
			projectPath = Path.GetDirectoryName(filePath);
			JObject obj = null;
			try
			{
				string content = File.ReadAllText(filePath);
				obj = JObject.Parse(content);
			}
			catch (Exception e)
			{
				Dialogs.Error.Show("Error on " + e.TargetSite, e.Message);
				return;
			}

			if (obj.ContainsKey("Name")) Name = (string)obj["Name"];
			else
			{
				Dialogs.Error.Show("Invalid Project File", "The project has no name.");
				return; // project will be invalid
			}

			if (obj.ContainsKey("ID")) id = (string)obj["ID"];
			else
			{
				Dialogs.Error.Show("Invalid Project File", "The project has no ID");
				return;
			}

			if (obj.ContainsKey("Version")) version = (int)obj["Version"];
			if (obj.ContainsKey("firstClientGui")) firstClientGui = (string)obj["firstClientGui"];

			if (obj.ContainsKey("ResolumePort")) resolumePort = (int)obj["ResolumePort"];
			if (obj.ContainsKey("ResolumeIP")) resolumeIP = (string)obj["ResolumeIP"];

			if (!LoadFolders(obj))
			{
				return;
			}

			ServerEndpointWriter = new EndpointWriter(Path.Combine(projectPath, "Server"), Intellisense.ServerLanguage, true);
			ClientEndpointWriter = new EndpointWriter(Path.Combine(projectPath, "Client"), Intellisense.ClientLanguage, false);

			Osc.Tree.Root.ValueOverrideHandler = ServerCompiler.OscValueOverride;

			Pages.ClientExplorer.Handle?.SetGroups(Groups);

			valid = true;
		}

		#region Save
		bool needsSaving = false;
		public bool NeedsSaving()
		{
			if (needsSaving) return true;

			foreach (var folder in Folders)
			{
				if (folder.NeedsSaving())
				{
					return true;
				}
			}
			if (Groups.NeedsSaving()) return true;

			return false;
		}

		public bool Save()
		{
			return SaveAs(filePath);
		}

		public bool SaveAs(string filePath)
		{
			version++;
			string content = SerializeProject();
			try
			{
				File.WriteAllText(filePath, content);
			}
			catch (Exception e)
			{
				Dialogs.Error.Show("Error on " + e.TargetSite, e.Message);
				return false;
			}
			needsSaving = false;
			return true;
		}

		public string SerializeProject()
		{
			JObject obj = new JObject();
			obj["Name"] = Name;
			obj["ID"] = ID;
			obj["Version"] = version;
			obj["firstClientGui"] = firstClientGui;
			obj["ResolumeIP"] = resolumeIP;
			obj["ResolumePort"] = resolumePort;

			SaveFolders(obj);
			return obj.ToString();
		}

		public string SerializeForClient()
		{
			JObject obj = new JObject();
			obj["Name"] = Name;
			obj["ID"] = ID;
			obj["Version"] = version;
			obj["firstClientGui"] = firstClientGui;
			obj["ResolumeIP"] = resolumeIP;
			obj["ResolumePort"] = resolumePort;

			ClientModules.SaveForClient(obj);

			return obj.ToString();
		}

		public string SerializeResource(string id)
		{
			var module = ClientModules.Get(id);
			if (module != null) return module.SerializeForClient();
			return string.Empty;
		}

		public bool CompileClientScript()
		{
			return ClientModules.CompileScript();
		}

		#endregion Save

		#region Folders
		public ServerModuleFolder ServerModules;
		public ClientModuleFolder ClientModules;
		public List<IFolder> Folders = new List<IFolder>();

		bool LoadFolders(JObject obj)
		{
			if (File.Exists(Path.Combine(projectPath, "groups.json")))
			{
				var groups = File.ReadAllText(Path.Combine(projectPath, "groups.json"));
				JObject groupObj = JObject.Parse(groups);
				Groups = new ClientGroups(groupObj);
			}


			ServerModules = new ServerModuleFolder("Server Modules", Path.Combine(projectPath, "Server"), @"/InteractServer;component/Resources/Icons/screen.png");
			if (!ServerModules.Load(obj)) return false;
			Folders.Add(ServerModules);
			foreach (var resource in ServerModules.Resources)
			{
				if (resource is Script)
				{
#if (WithSyntaxEditor)
					((resource as Script).View as CodeEditor.CodeEditor).SetLanguage(Intellisense.ServerLanguage);
#endif
				}
			}

			ClientModules = new ClientModuleFolder("Client Modules", Path.Combine(projectPath, "Client"), @"/InteractServer;component/Resources/Icons/Phone_16x.png");
			if (!ClientModules.Load(obj)) return false;
			Folders.Add(ClientModules);
			foreach (var resource in ClientModules.Resources)
			{
				if (resource is Script)
				{
#if (WithSyntaxEditor)
					((resource as Script).View as CodeEditor.CodeEditor).SetLanguage(Intellisense.ClientLanguage);
#endif
				}
			}

			needsSaving = false;
			return true;
		}

		bool SaveFolders(JObject obj)
		{
			bool recompileNeeded = false;
			foreach (var resource in ServerModules.Resources)
			{
				if (resource is Script)
				{
					if (resource.NeedsSaving()) recompileNeeded = true;
				}
			}
			if (!recompileNeeded) foreach (var resource in ClientModules.Resources)
				{
					if (resource is Script)
					{
						if (resource.NeedsSaving()) recompileNeeded = true;
					}
				}
			foreach (var folder in Folders)
			{
				folder.SaveContent();
				if (!folder.SaveToJson(obj)) return false;
			}
			if (Groups.NeedsSaving())
			{
				var jObj = Groups.Save();
				string content = jObj.ToString();
				File.WriteAllText(Path.Combine(projectPath, "groups.json"), content);
			}

			if (recompileNeeded)
			{
				RecompileScripts();
			}
			return true;
		}

		public string ResourceName(string ID)
		{
			string result = ServerModules.GetName(ID);
			if(result.Length == 0)
			{
				result = ClientModules.GetName(ID);
			}
			return result;
		}

		public void RecompileScripts()
		{
			RecompileServerScripts();
			RecompileClientScripts();

			foreach (var module in ClientModules.Resources)
			{
				if (module is SensorConfig)
				{
					(module as SensorConfig).UpdateRouteNames();
				} else if (module is ArduinoConfig)
				{
					(module as ArduinoConfig).UpdateRouteNames();
				}
			}
		}

		public void RecompileServerScripts()
		{
			ServerCompiler.StopAssembly();

			List<string> scripts = new List<string>();
			foreach (var resource in ServerModules.Resources)
			{
				if (resource is Script)
				{
					scripts.Add(Path.Combine((resource as Script).FolderPath, resource.ID));
				}
			}

			scripts.Add(Path.Combine(projectPath, "Server", "EndpointWriter.cs"));

			if (scripts.Count > 0)
			{
				Log.Log.Handle.AddEntry("Recompiling Server Scripts...");
				ServerCompiler.Compile(scripts.ToArray());
				if(ServerCompiler.HasScriptInterface())
				{
					ServerCompiler.Run();
				} else
				{
					Log.Log.Handle.AddEntry("No Server Scripts found");
				}
				
			}
		}

		public void RecompileClientScripts()
		{
			ClientCompiler.StopAssembly();

			List<string> clientScripts = new List<string>();
			foreach (var resource in ClientModules.Resources)
			{
				if (resource is Script)
				{
					clientScripts.Add(Path.Combine((resource as Script).FolderPath, resource.ID));
				}
			}

			clientScripts.Add(Path.Combine(projectPath, "Client", "EndpointWriter.cs"));
			if (clientScripts.Count > 0)
			{
				Log.Log.Handle.AddEntry("Recompiling Client Scripts...");
				ClientCompiler.Compile(clientScripts.ToArray());

				if(ClientCompiler.HasScriptInterface())
				{
					if (ClientCompiler.Run())
					{
						Log.Log.Handle.AddEntry("Client Scripts Reloaded.");
					}
					else
					{
						Log.Log.Handle.AddEntry("Some Client Scripts have errors!");
					}
				} else
				{
					Log.Log.Handle.AddEntry("No client scripts found.");
				}
				
			}
		}

		public void CreateResourceInFolder(IFolder folder)
		{
			if (folder == ServerModules)
			{
				var dialog = new Dialogs.NewServerModule();
				dialog.ShowDialog();
				if (dialog.DialogResult == true)
				{
					ServerModules.CreateResource(dialog.ModuleName, dialog.Type);
				}
			}
			else if (folder == ClientModules)
			{
				var dialog = new Dialogs.NewClientModule();
				dialog.ShowDialog();
				if (dialog.DialogResult == true)
				{
					ClientModules.CreateResource(dialog.ModuleName, dialog.Type);
					if (firstClientGui == string.Empty && dialog.Type == ContentType.ClientGui)
					{
						var mod = ClientModules.GetByName(dialog.ModuleName);
						firstClientGui = mod.ID;
					}
				}
			}

			Save();
			Pages.ProjectExplorer.Handle.Refresh();
		}

		public void Close()
		{
			if (Current != null)
			{
				if (Current.NeedsSaving())
				{
					Current.Save();
				}
			}

			Osc.Tree.Clear();
			MainWindow.Handle.CloseDocuments();
			Pages.Properties.Handle.SetSelected(null);

			Project.current = null;
			Pages.ProjectExplorer.Handle.Refresh();
		}

		public void RemoveResource(IResource resource)
		{
			if (ServerModules.Resources.Contains(resource))
			{
				ServerModules.RemoveResource(resource);
			}
			else
			{
				ClientModules.RemoveResource(resource);
			}
			needsSaving = true;
		}
#endregion Folders

#region Run
		private bool running = false;
		public bool Running { get => running; }

		public void Run()
		{
			running = true;
			MakeCurrentOnClients();
			ServerCompiler.OnProjectStart();
		}

		public void Stop()
		{
			running = false;
			ServerCompiler.OnProjectStop();
		}

#endregion Run

		public void MakeCurrentOnClients()
		{
			foreach (var client in (App.Current as App).ClientList.List.Values)
			{
				client.Send.ProjectSet(this.id, version);
			}
		}

		public void MakeCurrentOnClient(string id)
		{
			var client = (App.Current as App).ClientList.Get(id);
			client?.Send.ProjectSet(this.id, version);
		}

		~Project()
		{
			Intellisense.Dispose();
		}
	}
}
