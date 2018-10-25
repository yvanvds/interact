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
		private bool hasServerScript = false;

		public ServerModuleFolder(string name, string path, string icon)
			: base(name, path, icon)
		{
		}

		public override bool CreateResource(string name, ContentType type)
		{
			switch(type)
			{
				case ContentType.ServerGui:
					resources.Add(new Gui(name, true, this.path));
					break;
				case ContentType.ServerPatcher:
					resources.Add(new Patcher(name, true, this.path));
					break;
				case ContentType.ServerSounds:
					resources.Add(new SoundPage(name, true, this.path));
					break;
				case ContentType.ServerScript:
					resources.Add(new Script(name, true, this.path));
#if(WithSyntaxEditor)
					((resources.Last() as Script).View as CodeEditor.CodeEditor).SetLanguage(Project.Current.Intellisense.ServerLanguage);
#endif
					string path;
					if (!hasServerScript)
					{
						path = "InteractServer.Resources.Definitions.ServerScriptTemplate1.cs";

					} else
					{
						path = "InteractServer.Resources.Definitions.ServerScriptTemplate2.cs";
					}

					using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
					{
						if (stream != null)
						{
							StreamReader reader = new StreamReader(stream);
							(resources.Last() as Script).View.Text = reader.ReadToEnd();
							(resources.Last() as Script).SaveContent();
						}
					}
					hasServerScript = true;

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
							resources.Add(new Gui(elm, true, path));
							break;
						case "ServerScript":
							resources.Add(new Script(elm, true, path));
							hasServerScript = true;
							break;
						case "ServerPatcher":
							resources.Add(new Patcher(elm, true, path));
							break;
						case "ServerSounds":
							resources.Add(new SoundPage(elm, true, path));
							break;
					}
				}
			}
			needsSaving = false;
			return true;
		}

		public override bool SaveToJson(JObject obj)
		{
			if(resources.Count > 0)
			{
				obj[Name] = new JObject();
				foreach(var resource in resources)
				{
					obj[Name][resource.ID] = resource.SaveToJson();
				}
			}
			needsSaving = false;
			return true;
		}
		#endregion JSON

	}
}
