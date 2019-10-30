using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project
{
	public class ClientModuleFolder : AbstractFolder, IFolder
	{
		public FileGroup GuiGroup;
		public FileGroup PatcherGroup;
		public FileGroup ScriptGroup;
		public FileGroup SensorGroup;
		public FileGroup ArduinoGroup;

		public ClientModuleFolder(string name, string path, string icon)
			: base(name, path, icon)
		{
			Groups.Add(new FileGroup("Guis", Path.Combine(path, "Gui"), @"/InteractServer;component/Resources/Icons/screen.png", ".json", typeof(Gui), ContentType.ClientGui));
			GuiGroup = Groups.Last();

			Groups.Add(new FileGroup("Patchers", Path.Combine(path, "Patcher"), @"/InteractServer;component/Resources/Icons/Patcher_16x.png", ".yap", typeof(Patcher), ContentType.ClientPatcher));
			PatcherGroup = Groups.Last();

			Groups.Add(new FileGroup("Scripts", Path.Combine(path, "Script"), @"/InteractServer;component/Resources/Icons/Script_16x.png", ".cs", typeof(Script), ContentType.ClientScript));
			ScriptGroup = Groups.Last();

			Groups.Add(new FileGroup("Sensors", Path.Combine(path, "Sensor"), @"/InteractServer;component/Resources/Icons/sensors_16.png", ".json", typeof(SensorConfig), ContentType.ClientSensors));
			SensorGroup = Groups.Last();

			Groups.Add(new FileGroup("Arduino", Path.Combine(path, "Arduino"), @"/InteractServer;component/Resources/Icons/arduino_16.png", ".json", typeof(ArduinoConfig), ContentType.ClientArduino));
			ArduinoGroup = Groups.Last();

			if (!File.Exists(Path.Combine(path, "Script", "Main.cs")))
			{
				var resource = ScriptGroup.CreateResource("Main", false);
				using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("InteractServer.Resources.Definitions.ClientScriptTemplate1.cs"))
				{
					if (stream != null)
					{
						StreamReader reader = new StreamReader(stream);
						(resource as Script).View.Text = reader.ReadToEnd();
						(resource as Script).SaveContent();
					}
				}
				needsSaving = true;
			}
		}

		public override bool CreateResource(string name, ContentType type)
		{
			switch (type)
			{
				case ContentType.ClientGui:
					GuiGroup.CreateResource(name, false);
					break;
				case ContentType.ClientPatcher:
					PatcherGroup.CreateResource(name, false);
					break;
				case ContentType.ClientSensors:
					SensorGroup.CreateResource(name, false);
					break;
				case ContentType.ClientArduino:
					ArduinoGroup.CreateResource(name, false);
					break;
				case ContentType.ClientScript:
					var resource = ScriptGroup.CreateResource(name, false);
#if (WithSyntaxEditor)
					((resource as Script).View as CodeEditor.CodeEditor).SetLanguage(Project.Current.Intellisense.ClientLanguage);
#endif
					string content = "InteractServer.Resources.Definitions.ClientScriptTemplate2.cs";

					using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(content))
					{
						if (stream != null)
						{
							StreamReader reader = new StreamReader(stream);
							(resource as Script).View.Text = reader.ReadToEnd();
							(resource as Script).SaveContent();
						}
					}
					break;
			}
			needsSaving = true;
			return true;
		}

		#region JSON
		public override bool Load(JObject obj)
		{
			if (obj.ContainsKey(Name))
			{
				JObject arr = obj[Name] as JObject;
				foreach (var resource in arr.Values())
				{
					JObject elm = resource as JObject;
					string type = (string)elm["Type"];
					switch (type)
					{
						case "ClientGui":
							GuiGroup.CreateResource(elm, false);
							break;
						case "ClientScript":
							ScriptGroup.CreateResource(elm, false);
							break;
						case "ClientPatcher":
							PatcherGroup.CreateResource(elm, false);
							break;
						case "ClientSensors":
							SensorGroup.CreateResource(elm, false);
							break;
						case "ClientArduino":
							ArduinoGroup.CreateResource(elm, false);
							break;
					}
				}
			}
			needsSaving = false;
			return true;
		}

		public override bool SaveToJson(JObject obj)
		{
			obj[Name] = new JObject();
			foreach (var group in groups)
			{
				group.SaveToJson(obj[Name] as JObject);
			}
			needsSaving = false;
			return true;
		}
		#endregion JSON

		public void SaveForClient(JObject obj)
		{
			obj[Name] = new JObject();
			foreach(var group in groups)
			{
				group.SaveForClient(obj[Name] as JObject);
			}
		}

		public bool CompileScript()
		{
			List<string> code = new List<string>();
			foreach(var resource in ScriptGroup.Resources)
			{
				code.Add((resource as Script).GetCurrentContent());		
			}

			var path = Path.Combine(base.path, "ClientScript.dll");
			File.Delete(path);

			if (code.Count > 0)
			{	
				return Compiler.ClientCompiler.CreateAssemblyOnDisk(code.ToArray(), path);
			}
			return true;
		}

		public byte[] GetCompiledScript()
		{
			var path = Path.Combine(base.path, "ClientScript.dll");
			if(!File.Exists(path))
			{
				CompileScript();
			}
			if(File.Exists(path))
			{
				return File.ReadAllBytes(path);
			}
			return null;
		}
		
		public void UpdateRouteNames()
		{
			foreach(var resource in SensorGroup.Resources) {
				(resource as SensorConfig).UpdateRouteNames();
			}
			foreach(var resource in ArduinoGroup.Resources)
			{
				(resource as ArduinoConfig).UpdateRouteNames();
			}
		}
	}
}
