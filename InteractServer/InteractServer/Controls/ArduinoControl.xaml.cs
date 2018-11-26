using InteractServer.Dialogs;
using InteractServer.Groups;
using InteractServer.Sensors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// Interaction logic for ArduinoControl.xaml
	/// </summary>
	public partial class ArduinoControl : UserControl
	{
		public ObservableCollection<ArduinoPin> Pins = new ObservableCollection<ArduinoPin>();
		private string selectedGroup = string.Empty;
		private bool needsSaving = false;

		private OscTree.Object oscParent = null;

		public ArduinoControl()
		{
			InitializeComponent();

			PinList.DataContext = this;
			PinList.ItemsSource = Pins;

		}

		public string ToJSON()
		{
			needsSaving = false;
			JObject obj = new JObject();
			obj["SelectedGroup"] = selectedGroup;
			var arr = new JArray();
			foreach (var pin in Pins)
			{
				arr.Add(pin.ToJSON());
			}
			obj["Pins"] = arr;
			return obj.ToString();
		}

		public void LoadJSON(string content)
		{
			JObject obj = JObject.Parse(content);
			if(obj.ContainsKey("SelectedGroup"))
			{
				selectedGroup = (string)obj["SelectedGroup"];
			}
			if(obj.ContainsKey("Pins"))
			{
				var arr = obj["Pins"] as JArray;
				foreach(var pin in arr)
				{
					var newpin = new ArduinoPin(oscParent);
					newpin.ReadJSON(pin as JObject);
					Pins.Add(newpin);
				}
			}

			UpdateRouteNames();
		}

		public void UpdateRouteNames()
		{
			var routes = new OscTree.Routes();
			foreach(var pin in Pins)
			{
				if(pin.Route != null)
				{
					routes.Add(pin.Route);
				}
			}
			routes.UpdateScreenNames(Osc.Tree.Root);
		}

		public void SetOscParent(OscTree.Object parent)
		{
			oscParent = parent;
		}

		public bool NeedsSaving()
		{
			if (needsSaving) return true;
			foreach (var pin in Pins)
			{
				if (pin.NeedsSaving) return true;
			}
			return false;
		}

		public void OnShow()
		{
			cb.ItemsSource = Project.Project.Current?.Groups.List;
			foreach (var item in Project.Project.Current?.Groups.List)
			{
				if (item.ID == selectedGroup)
				{
					cb.SelectedItem = item;
					break;
				}
			}
		}

		private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				selectedGroup = (e.AddedItems[0] as Group).ID;
			}
			else
			{
				selectedGroup = string.Empty;
			}
			needsSaving = true;
		}

		private void AddPin_Click(object sender, RoutedEventArgs e)
		{
			Pins.Add(new ArduinoPin(oscParent));
			Pins.Last().ID = shortid.ShortId.Generate(true, false);
		}

		private void Route_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			ArduinoPin pin = (e.Source as Label).DataContext as ArduinoPin;
			var dialog = new RouteSelector(null, Osc.Tree.Root, true);
			if (pin.Route != null)
			{
				dialog.SetRoute(pin.Route);
			}
			dialog.ShowDialog();

			if(dialog.DialogResult == true)
			{
				pin.Route = dialog.CurrentRoute;
				needsSaving = true;
			}
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			ArduinoPin pin = (e.Source as Button).DataContext as ArduinoPin;
			if(pin.IsDigitalOut)
			{
				oscParent.Endpoints.List.Remove(pin.Name);
			}
			Pins.Remove(pin);
		}
	}
}
