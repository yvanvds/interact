using MahApps.Metro.Controls;
using Newtonsoft.Json.Linq;
using OscGuiControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.Project
{
	class Patcher : IResource, OscGuiControl.IPropertyInterface
	{
		public YapView.YapView View = null;
		public Grid PageGrid = null;
		public MahApps.Metro.Controls.ToggleSwitch SoundSwitch = null;
		public MahApps.Metro.Controls.ToggleSwitch EditSwitch = null;

		private LayoutDocument document = null;
		public LayoutDocument Document => document;

		public IYse.ISound sound;
		public IYse.IPatcher patcher;
		public Yse.YseHandler handler;

		#region PropertyInterface
		static private PropertyCollection properties = null;
		public PropertyCollection Properties => properties;

		static Patcher()
		{
			properties = new PropertyCollection();
			properties.Add("Name");
			properties.Add("ID");
			properties.Add("Version");
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

		private string icon = @"/InteractServer;component/Resources/Icons/Patcher_16x.png";
		public string Icon => icon;

		private int version = 0;
		public int Version => version;

		private string folderPath;
		private bool serverSide;

		private string content = string.Empty;

		private OscTree.Object oscObject;

		public Patcher(string name, bool serverSide, string folderPath)
		{
			createView();

			Name = name;
			this.id = shortid.ShortId.Generate(true);
			this.folderPath = folderPath;
			this.serverSide = serverSide;

			if (serverSide)
			{
				type = ContentType.ServerPatcher;
			} else
			{
				type = ContentType.ClientPatcher;
			}

			content = patcher.DumpJSON();

			setupOsc();
			setupDocument();

		}

		public Patcher(JObject obj, bool serverSide, string folderPath)
		{
			createView();

			LoadFromJson(obj);
			this.folderPath = folderPath;
			this.serverSide = serverSide;

			try
			{
				content = File.ReadAllText(System.IO.Path.Combine(folderPath, ID));
				patcher.ParseJSON(content);
				View.Load();
			} catch(Exception e)
			{
				Dialogs.Error.Show("Patcher Error", e.Message);
			}

			if(serverSide)
			{
				type = ContentType.ServerPatcher;
			} else
			{
				type = ContentType.ClientPatcher;
			}

			setupOsc();
			setupDocument();
		}

		private void createView()
		{
			sound = Yse.Yse.Handle.Interface.NewSound();
			patcher = Yse.Yse.Handle.Interface.NewPatcher();

			patcher.Create(1);
			
			sound.Create(patcher);
			sound.Play();

			PageGrid = new Grid();
			RowDefinition row1 = new RowDefinition();
			row1.Height = new System.Windows.GridLength(50);
			PageGrid.RowDefinitions.Add(row1);
			RowDefinition row2 = new RowDefinition();
			row2.Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
			PageGrid.RowDefinitions.Add(row2);

			var top = new StackPanel();
			top.Orientation = Orientation.Horizontal;
			Grid.SetRow(top, 0);
			PageGrid.Children.Add(top);

			SoundSwitch = new MahApps.Metro.Controls.ToggleSwitch();
			SoundSwitch.OffLabel = "Sound Off";
			SoundSwitch.OnLabel = "Sound On";
			SoundSwitch.IsCheckedChanged += SoundSwitch_IsCheckedChanged;
			top.Children.Add(SoundSwitch);

			EditSwitch = new MahApps.Metro.Controls.ToggleSwitch();
			EditSwitch.OffLabel = "Edit Mode";
			EditSwitch.OnLabel = "Performance Mode";
			top.Children.Add(EditSwitch);

			View = new YapView.YapView();
			handler = new Yse.YseHandler(patcher);
			View.Handle = handler;
			View.Focusable = true;
			View.Focus();
			View.Init();

			View.Load();
			View.PreviewMouseLeftButtonDown += View_PreviewMouseLeftButtonDown;

			Grid.SetRow(View, 1);
			PageGrid.Children.Add(View);

			Binding PerformanceBinding = new Binding();
			PerformanceBinding.Path = new System.Windows.PropertyPath(typeof(YapView.Interface).GetProperty(nameof(YapView.Interface.PerformanceMode)));
			PerformanceBinding.Mode = BindingMode.TwoWay;
			PerformanceBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			EditSwitch.SetBinding(ToggleSwitch.IsCheckedProperty, PerformanceBinding);


		}

		private void View_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if(View.IsFocused == false)
			{
				View.Focus();
			}
			if(View.IsKeyboardFocused == false)
			{
				Keyboard.Focus(View);
			}
		}

		private void SoundSwitch_IsCheckedChanged(object sender, EventArgs e)
		{
			if((bool)SoundSwitch.IsChecked)
			{
				sound.Play();
				sound.Volume = 1;
			} else
			{
				sound.FadeAndStop(100);
			}
		}

		private void setupOsc()
		{
			oscObject = new OscTree.Object(new OscTree.Address(Name, ID), typeof(float));
			

			patcher.OnOscBang += onOscBang;
			patcher.OnOscInt += onOscInt;
			patcher.OnOscFloat += onOscFloat;
			patcher.OnOscString += onOscString;
			if(type == ContentType.ServerPatcher)
			{
				Osc.Tree.ServerPatchers.Add(oscObject);
			} else
			{
				Osc.Tree.Client.Add(oscObject);
			}
			handler.SetOscObject(oscObject);
		}

		private void setupDocument()
		{
			Frame frame = new Frame();
			frame.Content = PageGrid;

			CommandBinding add = new CommandBinding(Commands.AddObject, AddObjectCommand_Executed, AddObjectCommand_CanExecute);
			CommandBinding perform = new CommandBinding(Commands.Perform, PerformCommand_Executed, PerformCommand_CanExecute);
			frame.CommandBindings.Add(add);
			frame.CommandBindings.Add(perform);

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

			if(obj.ContainsKey("Type"))
			{
				switch((string)obj["Type"])
				{
					case "ClientPatcher":
						type = ContentType.ClientPatcher;
						break;
					case "ServerPatcher":
						type = ContentType.ServerPatcher;
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

		public bool SaveContent()
		{
			try
			{
				content = View.Save();
				File.WriteAllText(System.IO.Path.Combine(folderPath, ID), content);
				return true;
			} catch(Exception e)
			{
				Dialogs.Error.Show("Error on " + e.TargetSite, e.Message);
				return false;
			}
		}

		public void DeleteOnDisk()
		{
			File.Delete(Path.Combine(folderPath, ID));
		}

		private bool needsSaving = false;
		public bool NeedsSaving()
		{
			if((View.Handle as Yse.YseHandler).NeedsSaving)
			{
				needsSaving = true;
			}
			return needsSaving;
		}

		private void AddObjectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void AddObjectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			View.AddObject(true);
		}

		private void PerformCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void PerformCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			View.Deselect();
			YapView.Interface.PerformanceMode = !YapView.Interface.PerformanceMode;
		}

		private void onOscBang(string to)
		{
			InteractServer.Osc.Tree.Root.Deliver(new OscTree.Route(to, OscTree.Route.RouteType.NAME), null);
		}

		private void onOscInt(string to, int value)
		{
			InteractServer.Osc.Tree.Root.Deliver(new OscTree.Route(to, OscTree.Route.RouteType.NAME), new object[] { value });
		}

		private void onOscFloat(string to, float value)
		{
			InteractServer.Osc.Tree.Root.Deliver(new OscTree.Route(to, OscTree.Route.RouteType.NAME), new object[] { value });
		}

		private void onOscString(string to, string value)
		{
			InteractServer.Osc.Tree.Root.Deliver(new OscTree.Route(to, OscTree.Route.RouteType.NAME), new object[] { value });
		}

		public void OnShow()
		{

		}
	}
}
