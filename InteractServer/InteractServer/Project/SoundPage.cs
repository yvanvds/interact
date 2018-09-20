using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;
using OscGuiControl;
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.Project
{
	public class SoundPage : IResource, OscGuiControl.IPropertyInterface
	{
		public Controls.SoundGrid View = null;
		

		private string name;
		public string Name
		{
			get => View.Name;
			set
			{
				string content = value;
				content = Regex.Replace(content, @"[^a-zA-Z0-9 -]", "");
				content = Utils.String.UppercaseWords(content);
				content = Regex.Replace(content, @"\s+", "");
				View.Name = content;
				needsSaving = true;
			}
		}

		private LayoutDocument document = null;
		public LayoutDocument Document => document;

		private string folderPath;
		private bool serverSide;

		private string content = string.Empty;

		#region PropertyInterface
		static private PropertyCollection properties = null;
		public PropertyCollection Properties => properties;

		static SoundPage()
		{
			properties = new PropertyCollection();
			properties.Add("Name");
			properties.Add("ID");
			properties.Add("Version");
		}
		#endregion PropertyInterface

		private ContentType type = ContentType.Invalid;
		public ContentType Type => type;

		private string id = string.Empty;
		public string ID => id;

		private string icon = @"/InteractServer;component/Resources/Icons/SoundFile_16x.png";
		public string Icon => icon;

		private int version = 0;
		public int Version => version;

		public SoundPage(string name, bool serverSide, string folderPath)
		{
			this.id = shortid.ShortId.Generate(true);
			this.folderPath = folderPath;
			this.serverSide = serverSide;

			createView(name);

			if(serverSide)
			{
				type = ContentType.ServerSounds;
			} else
			{
				type = ContentType.ClientSounds;
			}

			setupDocument();
		}

		public SoundPage(JObject obj, bool serverSide, string folderPath)
		{
			this.folderPath = folderPath;
			this.serverSide = serverSide;
			LoadFromJson(obj);
			createView((string)obj["Name"]);

			try
			{
				content = File.ReadAllText(System.IO.Path.Combine(folderPath, ID));
				View.Load(JObject.Parse(content));
			}
			catch (Exception e)
			{
				Dialogs.Error.Show("SoundPage Error", e.Message);
			}

			if (serverSide)
			{
				type = ContentType.ServerSounds;
			}
			else
			{
				type = ContentType.ClientSounds;
			}

			setupDocument();
		}

		private void createView(string name)
		{
			View = new Controls.SoundGrid(name, ID, Path.Combine(folderPath, "Sounds"));
		}

		private void setupDocument()
		{
			Frame frame = new Frame();
			frame.Content = View;

			document = new LayoutDocument();
			document.Content = frame;
			document.Title = Name;
		}

		

		public void DeleteOnDisk()
		{
			File.Delete(Path.Combine(folderPath, ID));
		}

		public bool LoadFromJson(JObject obj)
		{

			if (obj.ContainsKey("ID")) id = (string)obj["ID"];
			else return false;

			if (obj.ContainsKey("Type"))
			{
				switch ((string)obj["Type"])
				{
					case "ClientSounds":
						type = ContentType.ClientSounds;
						break;
					case "ServerSounds":
						type = ContentType.ServerSounds;
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

		private bool needsSaving = false;
		public bool NeedsSaving()
		{
			if (View.NeedsSaving) needsSaving = true;
			return needsSaving;
		}

		public bool SaveContent()
		{
			try
			{
				content = View.Save().ToString();
				File.WriteAllText(System.IO.Path.Combine(folderPath, ID), content);
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

		public void OnShow()
		{

		}
	}
}
