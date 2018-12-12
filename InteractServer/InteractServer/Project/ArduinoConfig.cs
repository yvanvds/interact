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
	public class ArduinoConfig : IResource
	{
		public Controls.ArduinoControl ArduinoGUI = null;

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

		public string DisplayName => System.IO.Path.GetFileNameWithoutExtension(Name);
		public string Location => Path.Combine(folderPath, Name);

		private ContentType type = ContentType.Invalid;
		public ContentType Type => type;

		private string id = string.Empty;
		public string ID => id;

		private string icon = "";
		public string Icon => icon;

		private int version = 0;
		public int Version => version;

		private LayoutDocument document = null;
		public LayoutDocument Document => document;

		private string content = string.Empty;
		private string folderPath;

		private OscTree.Object OscObject;

		public ArduinoConfig(string name, string folderPath)
		{
			ArduinoGUI = new Controls.ArduinoControl();

			Name = name;
			this.id = shortid.ShortId.Generate(false, false);
			this.folderPath = folderPath;

			type = ContentType.ClientArduino;
			content = ArduinoGUI.ToJSON();

			OscObject = new OscTree.Object(new OscTree.Address(Name, id), typeof(object));
			Osc.Tree.Client.Add(OscObject);
			ArduinoGUI.SetOscParent(OscObject);

			setupDocument();
		}

		public ArduinoConfig(JObject obj, string folderPath)
		{
			ArduinoGUI = new Controls.ArduinoControl();

			LoadFromJson(obj);
			this.folderPath = folderPath;

			OscObject = new OscTree.Object(new OscTree.Address(Name, id), typeof(object));
			Osc.Tree.Client.Add(OscObject);
			ArduinoGUI.SetOscParent(OscObject);

			try
			{
				content = File.ReadAllText(Path.Combine(folderPath, Name));
				ArduinoGUI.LoadJSON(content);
			} catch(Exception)
			{

			}

			type = ContentType.ClientArduino;
			setupDocument();
		}

		private void setupDocument()
		{
			Frame frame = new Frame();
			frame.Content = ArduinoGUI;

			document = new LayoutDocument();
			document.Title = Name;
			document.Content = frame;
		}

		public void DeleteOnDisk()
		{
			File.Delete(Path.Combine(folderPath, Name));
		}

		public bool LoadFromJson(JObject obj)
		{
			if (obj.ContainsKey("Name")) Name = (string)obj["Name"];
			else return false;
			if (obj.ContainsKey("ID")) id = (string)obj["ID"];
			else return false;
			if (obj.ContainsKey("Version")) version = (int)obj["Version"];

			needsSaving = false;
			return true;
		}

		public void MoveTo(string path)
		{
			throw new NotImplementedException();
		}

		public void OnShow()
		{
			ArduinoGUI.OnShow();
		}

		public bool SaveContent()
		{
			try
			{
				content = ArduinoGUI.ToJSON();
				File.WriteAllText(Path.Combine(folderPath, Name), content);
				return true;
			}
			catch (Exception e)
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
			if (ArduinoGUI.NeedsSaving())
				needsSaving = true;
			return needsSaving;
		}

		public void UpdateRouteNames()
		{
			ArduinoGUI.UpdateRouteNames();
		}
	}
}
