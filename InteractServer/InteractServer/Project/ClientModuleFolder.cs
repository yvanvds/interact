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
		private bool hasClientScript = false;

		public ClientModuleFolder(string name, string path, string icon)
			: base(name, path, icon)
		{

		}

		public override bool CreateResource(string name, ContentType type)
		{
			switch (type)
			{
				case ContentType.ClientGui:
					resources.Add(new Gui(name, false, path));
					break;
				case ContentType.ClientPatcher:
					resources.Add(new Patcher(name, false, path));
					break;
				case ContentType.ClientSensors:
					resources.Add(new SensorConfig(name, path));
					break;
				case ContentType.ClientScript:
					resources.Add(new Script(name, false, path));
					(resources.Last() as Script).View.SetLanguage(Project.Current.Intellisense.ClientLanguage);
					string resource;
					if (!hasClientScript)
					{
						resource = "InteractServer.Resources.Definitions.ClientScriptTemplate1.cs";
					}
					else
					{
						resource = "InteractServer.Resources.Definitions.ClientScriptTemplate2.cs";
					}

					using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
					{
						if (stream != null)
						{
							StreamReader reader = new StreamReader(stream);
							(resources.Last() as Script).View.Text = reader.ReadToEnd();
							(resources.Last() as Script).SaveContent();
						}
					}
					hasClientScript = true;

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
							resources.Add(new Gui(elm, false, path));
							break;
						case "ClientScript":
							resources.Add(new Script(elm, false, path));
							hasClientScript = true;
							break;
						case "ClientPatcher":
							resources.Add(new Patcher(elm, false, path));
							break;
						case "SensorConfig":
							resources.Add(new SensorConfig(elm, path));
							break;
					}
				}
			}
			needsSaving = false;
			return true;
		}

		public override bool SaveToJson(JObject obj)
		{
			if (resources.Count > 0)
			{
				obj[Name] = new JObject();
				foreach (var resource in resources)
				{
					obj[Name][resource.ID] = resource.SaveToJson();
				}
			}
			needsSaving = false;
			return true;
		}

		public void SaveForClient(JObject obj)
		{
			if (resources.Count > 0)
			{
				obj[Name] = new JObject();
				foreach (var resource in resources)
				{
					obj[Name][resource.ID] = resource.Version;
				}
			}
		}

		public bool CompileScript()
		{
			List<string> code = new List<string>();
			foreach(var resource in resources)
			{
				if(resource is Script)
				{
					code.Add((resource as Script).GetCurrentContent());
				}
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
		#endregion JSON
	}
}
