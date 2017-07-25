using Interact.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.Device
{
  public abstract class Sensor
  {
    public abstract SensorValue CurrentValue { get; }

    public abstract float ScaleMin { get; set; }
    public abstract float ScaleMax { get; set; }

    public abstract void Route(string IPAddress, int port, string OscAddress);

    public abstract void Send(SensorValue value);

    public abstract void Start(int delay);
    public abstract void Stop();
  }

  public abstract class Sensors
  {
    public abstract Sensor Compass { get; }
    public abstract Sensor AcceleroMeter { get; }
    public abstract Sensor MagnetoMeter { get; }
    public abstract Sensor Gyroscope { get; }
    public abstract Sensor Light { get; }
    public abstract Sensor Pressure { get; }
    public abstract Sensor Proximity { get; }
    public abstract Sensor LinearAcceleration { get; }
    public abstract Sensor Rotation { get; }
    public abstract Sensor GameRotation { get; }
    public abstract Sensor Humidity { get; }
    public abstract Sensor AmbientTemperature { get; }
    public abstract Sensor SignificantMotion { get; }
    public abstract Sensor StepDetector { get; }
    public abstract Sensor StepCounter { get; }
    public abstract Sensor HeartRate { get; }
    public abstract Sensor Pose { get; }
    public abstract Sensor Stationary { get; }
    public abstract Sensor Motion { get; }
    public abstract Sensor HeartBeat { get; }
    public abstract Sensor Tilt { get; }

  public abstract void Stop();

    ~Sensors() { Stop(); }
  }
}
