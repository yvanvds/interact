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

namespace InteractServer.Controls
{
	/// <summary>
	/// Interaction logic for SoundControl.xaml
	/// </summary>
	public partial class SoundControl : UserControl
	{
		private IYse.ISound sound;

		private string originalFileName;
		private string projectFileName;
		private string id;
		private string soundName;

		public string OriginalFileName => originalFileName;
		public string ProjectFileName => projectFileName;
		public string ID => id;
		public string SoundName => soundName;

		public OscTree.Object osc;

		public bool Loop
		{
			get
			{
				if (sound == null) return false;
				return sound.Loop;
			}
			set
			{
				if (sound == null) return;
				sound.Loop = value;
			}
		}

		public SoundControl(JObject obj, OscTree.Tree oscParent, string soundPath)
		{
			InitializeComponent();
			originalFileName = obj.ContainsKey("OriginalFileName") ? (string)obj["OriginalFileName"] : String.Empty;
			projectFileName = obj.ContainsKey("ProjectFileName") ? (string)obj["ProjectFileName"] : String.Empty;
			id = obj.ContainsKey("ID") ? (string)obj["ID"] : String.Empty;
			soundName = obj.ContainsKey("Name") ? (string)obj["Name"] : string.Empty;

			this.DataContext = this;

			if(projectFileName != string.Empty)
			{
				sound = Yse.Yse.Handle.Interface.NewSound();
				var path = System.IO.Path.Combine(soundPath, projectFileName);
				sound.Create(path);

				PositionSlider.Minimum = 0;
				PositionSlider.Maximum = sound.Length;
			}

			osc = new OscTree.Object(new OscTree.Address(soundName, id), typeof(float));
			oscParent.Add(osc);
			osc.Endpoints.Add(new OscTree.Endpoint("Play", (args) =>
			{
				if (sound == null) return;

				if (args.Count() > 0)
				{
					if (Convert.ToBoolean(args[0]) == true)
					{
						sound.Play();
					}
					else
					{
						sound.Stop();
					}
				} else
				{
					sound.Play();
				}
				
			}, typeof(bool)));

			osc.Endpoints.Add(new OscTree.Endpoint("Stop", (args) =>
			{
				if (sound == null) return;
				sound.Stop();
			}, typeof(bool)));

			osc.Endpoints.Add(new OscTree.Endpoint("Pause", (args) =>
			{
				if (sound == null) return;
				sound.Pause();
			}, typeof(bool)));

			osc.Endpoints.Add(new OscTree.Endpoint("Time", (args) =>
			{
				if (sound == null) return;

				if(args.Count() > 0)
				{
					sound.Time = Convert.ToSingle(args[0]);
				}
			}, typeof(float)));

			osc.Endpoints.Add(new OscTree.Endpoint("Volume", (args) =>
			{
				if (sound == null) return;
				if (args.Count() > 0)
				{
					sound.Volume = Convert.ToSingle(args[0]);
				}
			}, typeof(float)));

			osc.Endpoints.Add(new OscTree.Endpoint("Speed", (args) =>
			{
				if (sound == null) return;
				if (args.Count() > 0)
				{
					sound.Speed = Convert.ToSingle(args[0]);
				}
			}, typeof(float)));

			osc.Endpoints.Add(new OscTree.Endpoint("Loop", (args) =>
			{
				if (sound == null) return;
				if (args.Count() > 0)
				{
					sound.Loop = Convert.ToBoolean(args[0]);
				}
			}, typeof(bool)));
		}

		public JObject Save()
		{
			var obj = new JObject();
			obj["OriginalFileName"] = OriginalFileName;
			obj["ProjectFileName"] = ProjectFileName;
			obj["ID"] = ID;
			obj["Name"] = SoundName;
			return obj;
		}

		public static JObject CreateJObject(string originalFileName, string projectFileName, string id, string soundName)
		{
			var obj = new JObject();
			obj["OriginalFileName"] = originalFileName;
			obj["ProjectFileName"] = projectFileName;
			obj["ID"] = id;
			obj["Name"] = soundName;
			return obj;
		}

		private void ButtonPlay_Click(object sender, RoutedEventArgs e)
		{
			if (sound != null) sound.Play();
		}

		private void ButtonStop_Click(object sender, RoutedEventArgs e)
		{
			if (sound != null) sound.Stop();
		}

		private void ButtonPause_Click(object sender, RoutedEventArgs e)
		{
			if (sound != null) sound.Pause();
		}

		public void UpdateGui()
		{
			if (sound == null) return;
			float length = sound.Length / 1000;
			{

				int minutes = (int)length / 60;
				int seconds = (int)length % 60;
				LabelLength.Content = minutes + ":" + seconds;
			}
			float time = sound.Time / 1000;
			{
				int minutes = (int)time / 60;
				int seconds = (int)time % 60;
				LabelPosition.Content = minutes + ":" + seconds;
			}
			float timeleft = length - time;
			{
				int minutes = (int)timeleft / 60;
				int seconds = (int)timeleft % 60;
				LabelTimeLeft.Content = minutes + ":" + seconds;
			}

			PositionSlider.Maximum = length;
			PositionSlider.Value = time;
		}


	}
}
