using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Arduino
{
	public class ArduinoConfig
	{
		private string name = string.Empty;
		public string Name => name;

		private OscTree.Route route = null;
		public OscTree.Route Route => route;

		private string id = string.Empty;
		public string ID => id;

		private byte pin = 0;
		public byte Pin => pin;

		private string mode = "Analog In";
		public string Mode => mode;

		private int stepSize = 1;
		public int StepSize => stepSize;

		private int lowValue = 0;
		public int LowValue => lowValue;

		private int highValue = 1024;
		public int HighValue => highValue;

		public ArduinoConfig(JObject obj)
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

			if (obj.ContainsKey("StepSize"))
			{
				stepSize = (int)obj["StepSize"];
			}

			if (obj.ContainsKey("LowValue"))
			{
				lowValue = (int)obj["LowValue"];
			}

			if (obj.ContainsKey("HighValue"))
			{
				highValue = (int)obj["HighValue"];
			}

			if (obj.ContainsKey("Route"))
			{
				route = new OscTree.Route(obj["Route"] as JObject);
			}
		}
	}
}
