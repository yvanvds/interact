using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace InteractClient.Project
{
	public class ArduinoModule : BaseModule
	{
		private List<Arduino.ArduinoConfig> pins = new List<Arduino.ArduinoConfig>();
		public List<Arduino.ArduinoConfig> Pins { get => pins; }

		public string GroupID = string.Empty;

		public ArduinoModule()
		{

		}

		public override void LoadContent()
		{
			pins.Clear();
			JObject obj = JObject.Parse(Content);
			if (obj.ContainsKey("SelectedGroup"))
			{
				GroupID = (string)obj["SelectedGroup"];
				if(obj.ContainsKey("Pins"))
				{
					JArray arr = obj["Pins"] as JArray;
					if(arr != null)
					{
						foreach (JObject pin in arr)
						{
							Pins.Add(new Arduino.ArduinoConfig(pin));
						}
					}
					
				}
				
			}
		}

		public override void Activate()
		{
			if (Device.RuntimePlatform != "UWP") return;
			if (!Global.Arduino.IsImplemented()) return;

			Global.Arduino.Start(this);
		}

		public void Deactivate()
		{
			if (Device.RuntimePlatform != "UWP") return;
			Global.Arduino.Stop();
		}
	}
}
