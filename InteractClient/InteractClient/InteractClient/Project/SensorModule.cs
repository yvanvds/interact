using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Project
{
	public class SensorModule : BaseModule
	{
		private Dictionary<Sensors.SensorType, Sensors.SensorConfig> sensors = new Dictionary<Sensors.SensorType, Sensors.SensorConfig>();

		public string GroupID = string.Empty;

		public SensorModule()
		{

		}

		public override void LoadContent()
		{
			sensors.Clear();
			JObject obj = JObject.Parse(Content);
			if(obj.ContainsKey("SelectedGroup"))
			{
				GroupID = (string)obj["SelectedGroup"];
			}

			if(obj.ContainsKey("Accelerometer"))
			{
				sensors.Add(Sensors.SensorType.AcceleroMeter, new Sensors.SensorConfig(obj["Accelerometer"] as JObject));
			}

			if (obj.ContainsKey("AmbientTemperature"))
			{
				sensors.Add(Sensors.SensorType.AmbientTemperature, new Sensors.SensorConfig(obj["AmbientTemperature"] as JObject));
			}

			if (obj.ContainsKey("Compass"))
			{
				sensors.Add(Sensors.SensorType.Compass, new Sensors.SensorConfig(obj["Compass"] as JObject));
			}

			if (obj.ContainsKey("GameRotation"))
			{
				sensors.Add(Sensors.SensorType.GameRotation, new Sensors.SensorConfig(obj["GameRotation"] as JObject));
			}

			if (obj.ContainsKey("Gyroscope"))
			{
				sensors.Add(Sensors.SensorType.Gyroscope, new Sensors.SensorConfig(obj["Gyroscope"] as JObject));
			}

			if (obj.ContainsKey("HeartBeat"))
			{
				sensors.Add(Sensors.SensorType.HeartBeat, new Sensors.SensorConfig(obj["HeartBeat"] as JObject));
			}

			if (obj.ContainsKey("HeartRate"))
			{
				sensors.Add(Sensors.SensorType.HeartRate, new Sensors.SensorConfig(obj["HeartRate"] as JObject));
			}

			if (obj.ContainsKey("Humidity"))
			{
				sensors.Add(Sensors.SensorType.Humidity, new Sensors.SensorConfig(obj["Humidity"] as JObject));
			}

			if (obj.ContainsKey("Light"))
			{
				sensors.Add(Sensors.SensorType.Light, new Sensors.SensorConfig(obj["Light"] as JObject));
			}

			if (obj.ContainsKey("LinearAcceleration"))
			{
				sensors.Add(Sensors.SensorType.LinearAcceleration, new Sensors.SensorConfig(obj["LinearAcceleration"] as JObject));
			}

			if (obj.ContainsKey("Magnetometer"))
			{
				sensors.Add(Sensors.SensorType.MagnetoMeter, new Sensors.SensorConfig(obj["Magnetometer"] as JObject));
			}

			if (obj.ContainsKey("Motion"))
			{
				sensors.Add(Sensors.SensorType.Motion, new Sensors.SensorConfig(obj["Motion"] as JObject));
			}

			if (obj.ContainsKey("Pose"))
			{
				sensors.Add(Sensors.SensorType.Pose, new Sensors.SensorConfig(obj["Pose"] as JObject));
			}

			if (obj.ContainsKey("Pressure"))
			{
				sensors.Add(Sensors.SensorType.Pressure, new Sensors.SensorConfig(obj["Pressure"] as JObject));
			}

			if (obj.ContainsKey("Proximity"))
			{
				sensors.Add(Sensors.SensorType.Proximity, new Sensors.SensorConfig(obj["Proximity"] as JObject));
			}

			if (obj.ContainsKey("Rotation"))
			{
				sensors.Add(Sensors.SensorType.Rotation, new Sensors.SensorConfig(obj["Rotation"] as JObject));
			}

			if (obj.ContainsKey("SignificantMotion"))
			{
				sensors.Add(Sensors.SensorType.SignificantMotion, new Sensors.SensorConfig(obj["SignificantMotion"] as JObject));
			}

			if (obj.ContainsKey("Stationary"))
			{
				sensors.Add(Sensors.SensorType.Stationary, new Sensors.SensorConfig(obj["Stationary"] as JObject));
			}

			if (obj.ContainsKey("StepCounter"))
			{
				sensors.Add(Sensors.SensorType.StepCounter, new Sensors.SensorConfig(obj["StepCounter"] as JObject));
			}

			if (obj.ContainsKey("StepDetector"))
			{
				sensors.Add(Sensors.SensorType.StepDetector, new Sensors.SensorConfig(obj["StepDetector"] as JObject));
			}

			if (obj.ContainsKey("Tilt"))
			{
				sensors.Add(Sensors.SensorType.Tilt, new Sensors.SensorConfig(obj["Tilt"] as JObject));
			}
		}

		public override void Activate()
		{
			
			foreach(var sensor in sensors.Values)
			{
				if(sensor.Active)
				{
					Global.Sensors.Start(sensor.Type);
				}
			}

			Global.Sensors.SensorValueChanged += Sensors_SensorValueChanged;
		}

		public void Deactivate()
		{
			Global.Sensors.SensorValueChanged -= Sensors_SensorValueChanged;
		}

		private void Sensors_SensorValueChanged(object sender, Sensors.SensorValueChangedEventArgs e)
		{
			if (sensors[e.SensorType].Route == null) return;

			if(e.ValueType == Sensors.SensorValueType.Vector)
			{
				float[] f = (float[])e.Value;
				Global.OscRoot.Send(sensors[e.SensorType].Route, new object[] { f[0], f[1], f[2] });
			} else
			{
				Global.OscRoot.Send(sensors[e.SensorType].Route, new object[] { e.Value });
			}
			
		}
	}
}
