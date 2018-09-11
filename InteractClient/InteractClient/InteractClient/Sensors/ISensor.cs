using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Sensors
{
	public class SensorValueChangedEventArgs : EventArgs
	{
		public SensorType SensorType { get; set; }
		public object Value { get; set; }
		public SensorValueType ValueType { get; set; }
	}

	public delegate void SensorValueChangedEventHandler(object sender, SensorValueChangedEventArgs e);

	public interface ISensor
	{
		event SensorValueChangedEventHandler SensorValueChanged;

		void Start(SensorType sensorType, SensorDelay interval = SensorDelay.Default);
		void Stop(SensorType sensorType);

		bool IsActive(SensorType sensorType);
	}
}
