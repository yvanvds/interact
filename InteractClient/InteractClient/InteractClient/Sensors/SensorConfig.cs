using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Sensors
{
	public class SensorConfig
	{
		private string name = string.Empty;
		public string Name => name;

		public Sensors.SensorType Type;

		private bool active = false;
		public bool Active => active;

		private OscTree.Route route = null;
		public OscTree.Route Route => route;

		public SensorConfig(JObject obj)
		{
			if (obj.ContainsKey("name"))
			{
				name = (string)obj["name"];
			}

			if (obj.ContainsKey("active"))
			{
				active = (bool)obj["active"];
			}

			if (obj.ContainsKey("route"))
			{
				route = new OscTree.Route(obj["route"] as JObject);
			}

			switch(name)
			{
				case "Accelerometer": Type = SensorType.AcceleroMeter; break;
				case "AmbientTemperature": Type = SensorType.AmbientTemperature; break;
				case "Compass": Type = SensorType.Compass; break;
				case "GameRotation": Type = SensorType.GameRotation; break;
				case "Gyroscope": Type = SensorType.Gyroscope; break;
				case "HeartBeat": Type = SensorType.HeartBeat; break;
				case "HeartRate": Type = SensorType.HeartRate; break;
				case "Humidity": Type = SensorType.Humidity; break;
				case "Light": Type = SensorType.Light; break;
				case "LinearAcceleration": Type = SensorType.LinearAcceleration; break;
				case "Magnetometer": Type = SensorType.MagnetoMeter; break;
				case "Motion": Type = SensorType.Motion; break;
				case "Pose": Type = SensorType.Pose; break;
				case "Pressure": Type = SensorType.Pressure; break;
				case "Proximity": Type = SensorType.Proximity; break;
				case "Rotation": Type = SensorType.Rotation; break;
				case "SignificantMotion": Type = SensorType.SignificantMotion; break;
				case "Stationary": Type = SensorType.Stationary; break;
				case "StepCounter": Type = SensorType.StepCounter; break;
				case "StepDetector": Type = SensorType.StepDetector; break;
				case "Tilt": Type = SensorType.Tilt; break;
				default:
					{
						Network.Sender.WriteLog("Unknown sensor type in client configuration: " + name);
						break;
					}
			}
		}
	}
}
