using ActiproSoftware.Windows.Controls.SyntaxEditor;
using Newtonsoft.Json.Linq;
using OscGuiControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.Project
{
	class Script : IResource, OscGuiControl.IPropertyInterface
	{
		public CodeEditor.CodeEditor View = null;
		private LayoutDocument document = null;
		public LayoutDocument Document => document;

		#region PropertyInterface
		static private PropertyCollection properties = null;
		public PropertyCollection Properties => properties;

		static Script()
		{
			properties = new PropertyCollection();
			properties.Add("Name");
			properties.Add("ID");
			properties.Add("Version");
			properties.Add("DoUpdate", "Active", "Update Method");
			properties.Add("UpdateFrequency", "Frequency", "Update Method");
		}
		#endregion PropertyInterface

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

		private string id = string.Empty;
		public string ID => id;

		private ContentType type = ContentType.Invalid;
		public ContentType Type => type;

		private string icon = @"/InteractServer;component/Resources/Icons/Code_16x.png";
		public string Icon => icon;

		private int version = 0;
		public int Version => version;

		private string folderPath;
		public string FolderPath => folderPath;
		private bool serverSide;

		private bool doUpdate = false;
		public bool DoUpdate { get => doUpdate; set => doUpdate = value; }

		private int updateFrequency = 50;
		public int UpdateFrequency { get => updateFrequency; set => updateFrequency = value; }

		private string content = string.Empty;

		public Script(string name, bool serverSide, string folderPath)
		{
			setupView(name);

			this.id = shortid.ShortId.Generate(true);
			this.folderPath = folderPath;
			this.serverSide = serverSide;

			if (serverSide)
			{
				type = ContentType.ServerScript;
			}
			else
			{
				type = ContentType.ClientScript;
			}

			content = "";

			setupDocument();
		}

		public Script(JObject obj, bool serverSide, string folderPath)
		{
			string name = (string)obj["Name"];
			setupView(name);

			LoadFromJson(obj);
			this.folderPath = folderPath;
			this.serverSide = serverSide;

			try
			{
				content = File.ReadAllText(System.IO.Path.Combine(folderPath, ID));
			}
			catch (Exception e)
			{
				Dialogs.Error.Show("Script Error", e.Message);
			}

			setupDocument();
		}

		private void setupView(string name)
		{
			View = new CodeEditor.CodeEditor(name);
		}

		private void setupDocument()
		{
			View.Text = content;
			Frame frame = new Frame();
			frame.Content = View;

			document = new LayoutDocument();
			document.Title = Name;
			document.Content = frame;
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
					case "ClientScript":
						type = ContentType.ClientScript;
						break;
					case "ServerScript":
						type = ContentType.ServerScript;
						break;
				}
			}
			if (obj.ContainsKey("Version")) version = (int)obj["Version"];

			if (obj.ContainsKey("DoUpdate")) DoUpdate = (bool)obj["DoUpdate"];
			if (obj.ContainsKey("UpdateFrequency")) UpdateFrequency = (int)obj["UpdateFrequency"];

			needsSaving = false;
			return true;
		}

		public void MoveTo(string path)
		{
			throw new NotImplementedException();
		}

		public JObject SaveToJson()
		{
			if (needsSaving) version++;
			JObject obj = new JObject();
			obj["Name"] = Name;
			obj["ID"] = ID;
			obj["Type"] = Type.ToString();
			obj["Version"] = Version;
			obj["DoUpdate"] = DoUpdate;
			obj["UpdateFrequency"] = UpdateFrequency;
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
			obj["DoUpdate"] = DoUpdate;
			obj["UpdateFrequency"] = UpdateFrequency;
			return obj.ToString();
		}

		public bool SaveContent()
		{
			try
			{
				content = View.Text;
				View.Save(Path.Combine(folderPath, ID));
				View.NeedsSaving = false;
				return true;
			} catch(Exception e)
			{
				Dialogs.Error.Show("Error on " + e.TargetSite, e.Message);
				return false;
			}
		}

		public string GetCurrentContent()
		{
			return View.Text;
		}

		public void DeleteOnDisk()
		{
			File.Delete(Path.Combine(folderPath, ID));
		}

		private bool needsSaving = false;
		public bool NeedsSaving()
		{
			if(View.NeedsSaving)
			{
				needsSaving = true;
			}
			return needsSaving;
		}

		public void OnShow()
		{

		}
	}


}
