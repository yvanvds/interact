using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project
{
	public class ServerModuleFolder : AbstractFolder, IFolder
	{
		FileGroup GuiGroup;
		FileGroup PatcherGroup;
		FileGroup SoundGroup;
        FileGroup OutputGroup;
		public FileGroup ScriptGroup;

		public ServerModuleFolder(string name, string path, string icon)
			: base(name, path, icon)
		{
			Groups.Add(new FileGroup("Guis", Path.Combine(path, "Gui"), @"/InteractServer;component/Resources/Icons/screen.png", ".json", typeof(Gui), ContentType.ServerGui));
			GuiGroup = Groups.Last();

			Groups.Add(new FileGroup("Patchers", Path.Combine(path, "Patcher"), @"/InteractServer;component/Resources/Icons/Patcher_16x.png", ".yap", typeof(Patcher), ContentType.ServerPatcher));
			PatcherGroup = Groups.Last();

			Groups.Add(new FileGroup("Sounds", Path.Combine(path, "Sound"), @"/InteractServer;component/Resources/Icons/SoundFile_16x.png", ".json", typeof(SoundPage), ContentType.ServerSounds));
			SoundGroup = Groups.Last();

			Groups.Add(new FileGroup("Scripts", Path.Combine(path, "Script"), @"/InteractServer;component/Resources/Icons/Script_16x.png", ".cs", typeof(Script), ContentType.ServerScript));
			ScriptGroup = Groups.Last();

            Groups.Add(new FileGroup("Outputs", Path.Combine(path, "Output"), @"/InteractServer;component/Resources/Icons/route-16.png", ".out", typeof(OutputPage), ContentType.ServerOutput));
            OutputGroup = Groups.Last();

			if (!File.Exists(Path.Combine(path, "Script", "Main.cs")))
			{
				var resource = ScriptGroup.CreateResource("Main", true);
				using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("InteractServer.Resources.Definitions.ServerScriptTemplate1.cs"))
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
			switch(type)
			{
				case ContentType.ServerGui:
					GuiGroup.CreateResource(name, true);
					break;
				case ContentType.ServerPatcher:
					PatcherGroup.CreateResource(name, true);
					break;
				case ContentType.ServerSounds:
				  SoundGroup.CreateResource(name, true);
					break;
                case ContentType.ServerOutput:
                    OutputGroup.CreateResource(name, true);
                    break;
				case ContentType.ServerScript:
					var resource = ScriptGroup.CreateResource(name, true);
#if(WithSyntaxEditor)
					((resource as Script).View as CodeEditor.CodeEditor).SetLanguage(Project.Current.Intellisense.ServerLanguage);
#endif
					string path = "InteractServer.Resources.Definitions.ServerScriptTemplate2.cs";

					using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
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
			if(obj.ContainsKey(Name))
			{
				JObject arr = obj[Name] as JObject;
				foreach (var resource in arr.Values())
				{
					JObject elm = resource as JObject;
					string type  = (string)elm["Type"];
					switch(type)
					{
						case "ServerGui":
							GuiGroup.CreateResource(elm, true);
							break;
						case "ServerScript":
							ScriptGroup.CreateResource(elm, true);
							break;
						case "ServerPatcher":
							PatcherGroup.CreateResource(elm, true);
							break;
						case "ServerSounds":
							SoundGroup.CreateResource(elm, true);
							break;
                        case "ServerOutput":
                            OutputGroup.CreateResource(elm, true);
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

	}
}
