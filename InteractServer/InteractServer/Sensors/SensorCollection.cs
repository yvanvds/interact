using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Sensors
{
	public class SensorCollection
	{
		public List<Sensor> Sensors = new List<Sensor>();

		public SensorCollection()
		{
			Sensors.Add(new Sensor("Compas"));
			Sensors.Add(new Sensor("Step Counter"));
		}
	}
}
