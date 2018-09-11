using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using Newtonsoft.Json.Linq;
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.Project
{
	class SensorConfig : IResource
	{

		public string Name
		{
			get => SensorGUI.Name;
			set
			{
				string content = value;
				content = Regex.Replace(content, @"[^a-zA-Z0-9 -]", "");
				content = Utils.String.UppercaseWords(content);
				content = Regex.Replace(content, @"\s+", "");
				SensorGUI.Name = content;
				needsSaving = true;
			}
		}

		private ContentType type = ContentType.Invalid;
		public ContentType Type => type;

		private string id = string.Empty;
		public string ID => id;

		private string icon = @"/InteractServer;component/Resources/Icons/sensors_16.png";
		public string Icon => icon;

		private int version = 0;
		public int Version => version;

		private LayoutDocument document = null;
		public LayoutDocument Document => document;

		public Controls.SensorControl SensorGUI = null;

		private string content = string.Empty;

		private string folderPath;

		public SensorConfig(string name, string folderPath)
		{
			SensorGUI = new Controls.SensorControl();

			Name = name;
			this.id = shortid.ShortId.Generate(false, false);
			this.folderPath = folderPath;

			type = ContentType.ClientSensors;
			content = SensorGUI.ToJSON();

			setupDocument();
		}

		public SensorConfig(JObject obj, string folderPath)
		{
			SensorGUI = new Controls.SensorControl();

			LoadFromJson(obj);
			this.folderPath = folderPath;

			try
			{
				content = File.ReadAllText(System.IO.Path.Combine(folderPath, ID + "_sensorConf.json"));
				SensorGUI.LoadJSON(content);
			} catch(Exception)
			{
				
			}

			type = ContentType.ClientSensors;

			setupDocument();
		}

		private void setupDocument()
		{
			Frame frame = new Frame();
			frame.Content = SensorGUI;

			document = new LayoutDocument();
			document.Title = Name;
			document.Content = frame;
		}

		public void DeleteOnDisk()
		{
			File.Delete(System.IO.Path.Combine(folderPath, ID + "_sensorConf.json"));
		}

		public bool LoadFromJson(JObject obj)
		{
			if (obj.ContainsKey("Name")) Name = (string)obj["Name"];
			else return false;
			if (obj.ContainsKey("ID")) id = (string)obj["ID"];
			else return false;
			if (obj.ContainsKey("Type"))
			{
				switch ((string)obj["Type"])
				{
					case "ClientGui":
						type = ContentType.ClientGui;
						break;
					case "ServerGui":
						type = ContentType.ServerGui;
						break;
				}
			}
			if (obj.ContainsKey("Version")) version = (int)obj["Version"];

			needsSaving = false;
			return true;
		}

		public void MoveTo(string path)
		{
			throw new NotImplementedException();
		}

		public bool SaveContent()
		{ 
			try
			{
				content = SensorGUI.ToJSON();
				File.WriteAllText(Path.Combine(folderPath, ID + "_sensorConf.json"), content);
				return true;
			} catch(Exception e)
			{
				Dialogs.Error.Show("Error on " + e.TargetSite, e.Message);
				return false;
			}
		}

		public JObject SaveToJson()
		{
			if (needsSaving) version++;
			JObject obj = new JObject();
			obj["Name"] = Name;
			obj["ID"] = ID;
			obj["Type"] = Type.ToString();
			obj["Version"] = Version;
			needsSaving = false;
			return obj;
		}

		public string SerializeForClient()
		{
			JObject obj = new JObject();
			obj["Name"] = Name;
			obj["ID"] = ID;
			obj["Type"] = Type.ToString();
			obj["Version"] = Version;
			obj["Content"] = content;
			return obj.ToString();
		}

		private bool needsSaving = false;
		public bool NeedsSaving()
		{
			if (SensorGUI.NeedsSaving())
				needsSaving = true;
			return needsSaving;
		}
	}
}
