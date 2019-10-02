using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using Newtonsoft.Json.Linq;
using OscGuiControl;
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.Project
{
	class Gui : IResource
	{
		//#region PropertyInterface
		//static private PropertyCollection properties = null;
		//public PropertyCollection Properties => properties;

		//static Gui()
		//{
		//	properties = new PropertyCollection();
		//	properties.Add("Name");
		//	properties.Add("ID");
		//	properties.Add("Version");
		//}

		//#endregion PropertyInterface

		private string name = string.Empty;
		public string Name
		{
			get => name;
			set {
				name = value;
				needsSaving = true;
			}
		}

		public string DisplayName => System.IO.Path.GetFileNameWithoutExtension(Name);
		public string Location => System.IO.Path.Combine(folderPath, Name);

		private string id = string.Empty;
		public string ID => id;

		//public string Path { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		private ContentType type = ContentType.Invalid;
		public ContentType Type => type;

		private string icon = "";
		public string Icon => icon;

		//public string Data { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		private int version = 0;
		public int Version => version;

		private string content = string.Empty;

		public OscGuiControl.OscGUI OscGUI = null;
		private LayoutDocument document = null;
		public LayoutDocument Document => document;

		private string folderPath;
		private bool serverSide;

		public Gui(string name, bool serverSide, string folderPath)
		{
			OscGUI = new OscGuiControl.OscGUI();
			Name = name;
            OscGUI.Name = DisplayName;
			this.id = shortid.ShortId.Generate(false, false);
			this.folderPath = folderPath;
			this.serverSide = serverSide;

			
			OscGUI.SetInspector(Pages.Properties.Handle.GetInspector());

			if (serverSide)
			{
				type = ContentType.ServerGui;
				OscGUI.SetGridSize(8, 8);
				Osc.Tree.Server.Add(OscGUI.OscTree);
			}
			else
			{
				type = ContentType.ClientGui;
				OscGUI.SetGridSize(8, 4);
                OscGUI.OscTree.Endpoints.Add(new OscTree.Endpoint("Activate", (args) => { }, typeof(object)));
                Osc.Tree.Client.Add(OscGUI.OscTree);
			}

			content = OscGUI.ToJSON();

			setupDocument();
		}

		public Gui(JObject obj, bool serverSide, string folderPath)
		{
			OscGUI = new OscGuiControl.OscGUI();
			OscGUI.SetInspector(Pages.Properties.Handle.GetInspector());

			LoadFromJson(obj);
			this.folderPath = folderPath;
			this.serverSide = serverSide;

			try
			{
				content = File.ReadAllText(System.IO.Path.Combine(this.folderPath, Name));
				OscGUI.LoadJSON(content);
                OscGUI.Name = DisplayName;
				OscGUI.EditMode = false;
			} catch(Exception)
			{
				// set size because loading failed
				if (serverSide) OscGUI.SetGridSize(8, 8);
				else OscGUI.SetGridSize(8, 4);
			}

			if (serverSide)
			{
				type = ContentType.ServerGui;
				Osc.Tree.Server.Add(OscGUI.OscTree);
			}
			else
			{
				type = ContentType.ClientGui;
				OscGUI.OscTree.Endpoints.Add(new OscTree.Endpoint("Activate", (args) => { }, typeof(object)));
				Osc.Tree.Client.Add(OscGUI.OscTree);
			}
			setupDocument();
		}

		private void setupDocument()
		{
			Frame frame = new Frame();
			if(serverSide)
			{
				frame.Content = OscGUI;
			} else
			{
				var grid = new Grid();
				var column1 = new ColumnDefinition();
				column1.Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
				grid.ColumnDefinitions.Add(column1);
				var column2 = new ColumnDefinition();
				column2.Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
				grid.ColumnDefinitions.Add(column2);
				frame.Content = grid;
				Grid.SetColumn(OscGUI, 0);
				grid.Children.Add(OscGUI);
			}
			
			document = new LayoutDocument();
			document.Title = Name;
			document.Content = frame;
		}

		public bool SaveContent()
		{
			try
			{
				content = OscGUI.ToJSON();
				File.WriteAllText(System.IO.Path.Combine(folderPath, Name), content);
				return true;
			}
			catch (Exception e)
			{
				Dialogs.Error.Show("Error on " + e.TargetSite, e.Message);
				return false;
			}
		}

		public void DeleteOnDisk()
		{
			File.Delete(System.IO.Path.Combine(folderPath, Name));
		}

		public bool LoadFromJson(JObject obj)
		{
			if (obj.ContainsKey("Name")) Name = (string)obj["Name"];
			else return false;
			if (obj.ContainsKey("ID")) id = (string)obj["ID"];
			else return false;
			if(obj.ContainsKey("Type"))
			{
				switch((string)obj["Type"])
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

		public JObject SaveToJson()
		{
			if(needsSaving)version++;
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
			if (OscGUI.NeedsSaving())
				needsSaving = true;
			return needsSaving;
		}

		public void OnShow()
		{
			
		}
	}
}
