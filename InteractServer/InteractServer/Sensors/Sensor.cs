using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Sensors
{
	public class Sensor : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private string name;
		public string Name => name;

		private bool active = false;
		public bool Active
		{
			get => active;
			set
			{
				active = value;
				needsSaving = true;
			}
		}

		private bool needsSaving = false;
		public bool NeedsSaving => needsSaving;

		public string OscRouteName
		{
			get
			{
				if(route == null)
				{
					return "No Route Set";
				} else
				{
					return Route.ScreenName;
				}
			}
		}

		private OscTree.Route route = null;
		public OscTree.Route Route
		{
			get => route;
			set
			{
				route = value;
				OnPropertyChanged("OscRouteName");
				needsSaving = true;
			}
		}

		public Sensor(string name)
		{
			this.name = name;
		}

		protected void OnPropertyChanged(string name)
		{
			var handler = PropertyChanged;
			if(handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}

		public JObject ToJSON()
		{
			needsSaving = false;
			var obj = new JObject();
			obj["name"] = Name;
			obj["active"] = Active;
			if(route != null)
			{
				obj["route"] = route.ToJSON();
			}
			
			return obj;
		}

		public void ReadJSON(JObject obj)
		{
			if(obj.ContainsKey("name"))
			{
				name = (string)obj["name"];
			}

			if(obj.ContainsKey("active"))
			{
				active = (bool)obj["active"];
			}

			if(obj.ContainsKey("route"))
			{
				route = new OscTree.Route(obj["route"] as JObject);
				OnPropertyChanged("OscRouteName");
			}
			needsSaving = false;
		}
	}
}
