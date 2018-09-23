using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace InteractServer.Controls
{
	public partial class SoundGrid : UserControl
	{
		string SoundPath;

		private bool needsSaving = false;
		public bool NeedsSaving => needsSaving;

		private DispatcherTimer GuiTimer = null;

		private OscTree.Tree tree = null;

		public SoundGrid(string name, string ID, string soundPath)
		{
			InitializeComponent();
			Name = name;
			SoundPath = soundPath;
			System.IO.Directory.CreateDirectory(soundPath);

			tree = new OscTree.Tree(new OscTree.Address(name, ID));
			Osc.Tree.ServerSounds.Add(tree);

			GuiTimer = new DispatcherTimer();
			GuiTimer.Tick += GuiTimer_Tick;
			GuiTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
			GuiTimer.Start();
		}

		private void GuiTimer_Tick(object sender, EventArgs e)
		{
			foreach (SoundControl control in Panel.Children)
			{
				control.UpdateGui();
			}
		}

		private void AddSoundButton_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.Filter = "Audio file (ogg, wav)|*.ogg;*.wav";
			if(Properties.Settings.Default.LastAudioFolder.Length > 0)
			{
				dialog.InitialDirectory = Properties.Settings.Default.LastAudioFolder;
			}

			if(dialog.ShowDialog() == true)
			{
				AddFile(dialog.FileName);
				Properties.Settings.Default.LastAudioFolder = System.IO.Path.GetDirectoryName(dialog.FileName);
				Properties.Settings.Default.Save();
				needsSaving = true;
			}
		}

		private void AddFile(string path)
		{
			string OriginalFileName = path;
			string ID = shortid.ShortId.Generate(false, false);
			string Name = System.IO.Path.GetFileNameWithoutExtension(path);
			string ProjectFileName = ID + System.IO.Path.GetExtension(path);

			var target = System.IO.Path.Combine(SoundPath, ProjectFileName);
			System.IO.File.Copy(path, target, true);

			var sound = new SoundControl(SoundControl.CreateJObject(OriginalFileName, ProjectFileName, ID, Name), tree, SoundPath, this);

			Panel.Children.Add(sound);
		}

		public void AddCopy(string OriginalFileName, string ProjectFileName, string Name)
		{
			string ID = shortid.ShortId.Generate(false, false);

			var sound = new SoundControl(SoundControl.CreateJObject(OriginalFileName, ProjectFileName, ID, Name), tree, SoundPath, this);
			Panel.Children.Add(sound);
		}

		public JObject Save()
		{
			var obj = new JObject();
			foreach(SoundControl sound in Panel.Children)
			{
				obj[sound.ID] = sound.Save();
			}
			return obj;
		}

		public void Load(JObject content)
		{
			foreach(var obj in content.Values())
			{
				Panel.Children.Add(new SoundControl(obj as JObject, tree, SoundPath, this));
			}
		}

		public IYse.ISound GetByName(string name)
		{
			foreach(var soundcontrol in Panel.Children)
			{
				var sc = soundcontrol as SoundControl;
				if(sc.SoundName.Equals(name)) {
					return sc.Sound;
				}
			}
			return null;
		}

		public IYse.ISound GetByID(string ID)
		{
			foreach (var soundcontrol in Panel.Children)
			{
				var sc = soundcontrol as SoundControl;
				if (sc.ID.Equals(ID))
				{
					return sc.Sound;
				}
			}
			return null;
		}
	}
}
