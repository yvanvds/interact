using InteractServer.Dialogs;
using InteractServer.Groups;
using InteractServer.Sensors;
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
	/// Interaction logic for SensorControl.xaml
	/// </summary>
	public partial class SensorControl : UserControl
	{
		public Dictionary<string, Sensor> Sensors = new Dictionary<string, Sensor>();
		private string selectedGroup = string.Empty;
		private bool needsSaving = false;

		public SensorControl()
		{
			InitializeComponent();

			AddSensor("Accelerometer");
			AddSensor("AmbientTemperature");
			AddSensor("Compass");
			AddSensor("GameRotation");
			AddSensor("Gyroscope");
			AddSensor("HeartBeat");
			AddSensor("HeartRate");
			AddSensor("Humidity");
			AddSensor("Light");
			AddSensor("LinearAcceleration");
			AddSensor("Magnetometer");
			AddSensor("Motion");
			AddSensor("Pose");
			AddSensor("Pressure");
			AddSensor("Proximity");
			AddSensor("Rotation");
			AddSensor("SignificantMotion");
			AddSensor("Stationary");
			AddSensor("StepCounter");
			AddSensor("StepDetector");
			AddSensor("Tilt");

			SensorList.ItemsSource = Sensors.Values;
		}

		private void AddSensor(string name)
		{
			Sensors.Add(name, new Sensor(name));
		}

		public string ToJSON()
		{
			needsSaving = false;
			JObject obj = new JObject();
			obj["SelectedGroup"] = selectedGroup;
			foreach(var sensor in Sensors)
			{
				obj[sensor.Key] = sensor.Value.ToJSON();
			}
			return obj.ToString();
		}

		public void LoadJSON(string content)
		{
			JObject obj = JObject.Parse(content);
			if(obj.ContainsKey("SelectedGroup"))
			{
				selectedGroup = (string)obj["SelectedGroup"];
			}

			foreach (var sensor in Sensors)
			{
				LoadJSONSensor(obj, sensor.Key);
			}
			UpdateRouteNames();
		}

		public void UpdateRouteNames()
		{
			var routes = new OscTree.Routes();
			foreach (var sensor in Sensors)
			{
				if (sensor.Value.Route != null)
				{
					routes.Add(sensor.Value.Route);
				}
			}
			routes.UpdateScreenNames(Osc.Tree.Root);
		}

		private void LoadJSONSensor(JObject obj, string name)
		{
			if (obj.ContainsKey(name))
			{
				Sensors[name].ReadJSON(obj[name] as JObject);
			}
		}


		public bool NeedsSaving()
		{
			if (needsSaving) return true;
			foreach(var sensor in Sensors.Values)
			{
				if (sensor.NeedsSaving) return true;
			}
			return false;
		}

		public void OnShow()
		{
			cb.ItemsSource = Project.Project.Current?.Groups.List;
			foreach(var item in Project.Project.Current?.Groups.List)
			{
				if(item.ID == selectedGroup)
				{
					cb.SelectedItem = item;
					break;
				}
			}

		}

		private void RouteButton_Click(object sender, RoutedEventArgs e)
		{
			Sensor sensor = (e.Source as Button).DataContext as Sensor;
			var dialog = new RouteSelector(null, Osc.Tree.Root);
			dialog.ShowDialog();

			if(dialog.DialogResult == true) {
				sensor.Route = dialog.CurrentRoute;
				needsSaving = true;
			}
		}

		private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(e.AddedItems.Count > 0)
			{
				selectedGroup = (e.AddedItems[0] as Group).ID;
			}
			else
			{
				selectedGroup = string.Empty;
			}
			needsSaving = true;
		}
	}
}
