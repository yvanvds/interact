using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Sensors
{
	public class ArduinoPin : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private string name;
		public string Name
		{
			get => name;
			set
			{
				name = value;
				OnPropertyChanged("Name");
				needsSaving = true;
			}
		}

		private string id;
		public string ID
		{
			get => id;
			set
			{
				id = value;
				OnPropertyChanged("ID");
				needsSaving = true;
			}
		}

		private byte pin;
		public byte Pin
		{
			get => pin;
			set
			{
				pin = value;
				OnPropertyChanged("Pin");
				needsSaving = true;
			}
		}

		private string mode = "Analog";
		public string Mode
		{
			get => mode;
			set
			{
				mode = value;
				OnPropertyChanged("Mode");
				needsSaving = true;
			}
		}

		private int stepSize = 1;
		public int StepSize
		{
			get => stepSize;
			set
			{
				stepSize = value;
				OnPropertyChanged("StepSize");
				needsSaving = true;
			}
		}

		private int lowValue = 0;
		public int LowValue
		{
			get => lowValue;
			set
			{
				lowValue = value;
				OnPropertyChanged("LowValue");
				needsSaving = true;
			}
		}

		private int highValue = 1024;
		public int HighValue
		{
			get => highValue;
			set
			{
				highValue = value;
				OnPropertyChanged("HighValue");
				needsSaving = true;
			}
		}

		private bool needsSaving = false;
		public bool NeedsSaving => needsSaving;

		public string OscRouteName
		{
			get
			{
				if (route == null)
				{
					return "No Route Set";
				}
				else
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

		public ArduinoPin()
		{
			mode = "Analog In";
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
			obj["Name"] = Name;
			obj["ID"] = ID;
			obj["Pin"] = pin;
			obj["Mode"] = Mode;
			obj["StepSize"] = StepSize;
			obj["LowValue"] = LowValue;
			obj["HighValue"] = HighValue;
			if (route != null)
			{
				obj["Route"] = route.ToJSON();
			}

			return obj;
		}

		public void ReadJSON(JObject obj)
		{
			if (obj.ContainsKey("Name"))
			{
				name = (string)obj["Name"];
			}

			if(obj.ContainsKey("ID"))
			{
				id = (string)obj["ID"];
			}

			if(obj.ContainsKey("Pin"))
			{
				pin = (byte)obj["Pin"];
			}

			if(obj.ContainsKey("Mode"))
			{
				mode = (string)obj["Mode"];
			}

			if(obj.ContainsKey("StepSize"))
			{
				stepSize = (int)obj["StepSize"];
			}

			if(obj.ContainsKey("LowValue"))
			{
				lowValue = (int)obj["LowValue"];
			}

			if(obj.ContainsKey("HighValue"))
			{
				highValue = (int)obj["HighValue"];
			}

			if (obj.ContainsKey("Route"))
			{
				route = new OscTree.Route(obj["Route"] as JObject);
				OnPropertyChanged("OscRouteName");
			}
			needsSaving = false;
		}
	}
}
