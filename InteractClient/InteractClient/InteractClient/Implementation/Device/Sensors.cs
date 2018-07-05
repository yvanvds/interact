using Interact;
using Interact.Utility;
using InteractClient.Interface;
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Text;


namespace InteractClient.Implementation.Device
{
  public class Sensors : Interact.Device.Sensors
  {
    static Lazy<ISensor> Implementation = new Lazy<ISensor>(() => Create(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    public static ISensor Current
    {
      get
      {
        var result = Implementation.Value;
        if (result == null)
        {
          throw new NotImplementedException();
        }
        return result;
      }
    }

    static ISensor Create()
    {
      return DependencyService.Get<ISensor>();
    }

    public Sensor _Compass;
    public Sensor _AcceleroMeter;
    public Sensor _MagnetoMeter;
    public Sensor _Gyroscope;
    public Sensor _Light;
    public Sensor _Pressure;
    public Sensor _Proximity;
    public Sensor _LinearAcceleration;
    public Sensor _Rotation;
    public Sensor _GameRotation;
    public Sensor _Humidity;
    public Sensor _AmbientTemperature;
    public Sensor _SignificantMotion;
    public Sensor _StepDetector;
    public Sensor _StepCounter;
    public Sensor _HeartRate;
    public Sensor _Pose;
    public Sensor _Stationary;
    public Sensor _Motion;
    public Sensor _HeartBeat;
    public Sensor _Tilt;

    public override Interact.Device.Sensor Compass => _Compass;
    public override Interact.Device.Sensor AcceleroMeter => _AcceleroMeter;
    public override Interact.Device.Sensor MagnetoMeter => _MagnetoMeter;
    public override Interact.Device.Sensor Gyroscope => _Gyroscope;
    public override Interact.Device.Sensor Light => _Light;
    public override Interact.Device.Sensor Pressure => _Pressure;
    public override Interact.Device.Sensor Proximity => _Proximity;
    public override Interact.Device.Sensor LinearAcceleration => _LinearAcceleration;
    public override Interact.Device.Sensor Rotation => _Rotation;
    public override Interact.Device.Sensor GameRotation => _GameRotation;
    public override Interact.Device.Sensor Humidity => _Humidity;
    public override Interact.Device.Sensor AmbientTemperature => _AmbientTemperature;
    public override Interact.Device.Sensor SignificantMotion => _SignificantMotion;
    public override Interact.Device.Sensor StepDetector => _StepDetector;
    public override Interact.Device.Sensor StepCounter => _StepCounter;
    public override Interact.Device.Sensor HeartRate => _HeartRate;
    public override Interact.Device.Sensor Pose => _Pose;
    public override Interact.Device.Sensor Stationary => _Stationary;
    public override Interact.Device.Sensor Motion => _Motion;
    public override Interact.Device.Sensor HeartBeat => _HeartBeat;
    public override Interact.Device.Sensor Tilt => _Tilt;

    public Sensors()
    {
      _Compass = new Sensor(SensorType.Compass);
      _AcceleroMeter = new Sensor(SensorType.AcceleroMeter);
      _MagnetoMeter = new Sensor(SensorType.MagnetoMeter);
      _Gyroscope = new Sensor(SensorType.Gyroscope);
      _Light = new Sensor(SensorType.Light);
      _Pressure = new Sensor(SensorType.Pressure);
      _Proximity = new Sensor(SensorType.Proximity);
      _LinearAcceleration = new Sensor(SensorType.LinearAcceleration);
      _Rotation = new Sensor(SensorType.Rotation);
      _GameRotation = new Sensor(SensorType.GameRotation);
      _Humidity = new Sensor(SensorType.Humidity);
      _AmbientTemperature = new Sensor(SensorType.AmbientTemperature);
      _SignificantMotion = new Sensor(SensorType.SignificantMotion);
      _StepDetector = new Sensor(SensorType.StepDetector);
      _StepCounter = new Sensor(SensorType.StepCounter);
      _HeartRate = new Sensor(SensorType.HeartRate);
      _Pose = new Sensor(SensorType.Pose);
      _Stationary = new Sensor(SensorType.Stationary);
      _Motion = new Sensor(SensorType.Motion);
      _HeartBeat = new Sensor(SensorType.HeartBeat);
      _Tilt = new Sensor(SensorType.Tilt);

      Current.SensorValueChanged += (s, a) =>
      {

        switch (a.SensorType)
        {
          case SensorType.AcceleroMeter:
            AcceleroMeter.Send((SensorVector)a.Value);
            break;
          case SensorType.Compass:
            Compass.Send((SensorValue)a.Value);
            break;
          case SensorType.Gyroscope:
            Gyroscope.Send((SensorVector)a.Value);
            break;
          case SensorType.MagnetoMeter:
            MagnetoMeter.Send((SensorVector)a.Value);
            break;
          case SensorType.Light:
            Light.Send((SensorValue)a.Value);
            break;
          case SensorType.Pressure:
            Pressure.Send((SensorValue)a.Value);
            break;
          case SensorType.Proximity:
            Proximity.Send((SensorValue)a.Value);
            break;
          case SensorType.LinearAcceleration:
            LinearAcceleration.Send((SensorValue)a.Value);
            break;
          case SensorType.Rotation:
            Rotation.Send((SensorValue)a.Value);
            break;
          case SensorType.GameRotation:
            GameRotation.Send((SensorValue)a.Value);
            break;
          case SensorType.Humidity:
            Humidity.Send((SensorValue)a.Value);
            break;
          case SensorType.AmbientTemperature:
            AmbientTemperature.Send((SensorValue)a.Value);
            break;
          case SensorType.SignificantMotion:
            SignificantMotion.Send((SensorValue)a.Value);
            break;
          case SensorType.StepDetector:
            StepDetector.Send((SensorValue)a.Value);
            break;
          case SensorType.StepCounter:
            StepCounter.Send((SensorValue)a.Value);
            break;
          case SensorType.HeartRate:
            HeartRate.Send((SensorValue)a.Value);
            break;
          case SensorType.Pose:
            Pose.Send((SensorValue)a.Value);
            break;
          case SensorType.Stationary:
            Stationary.Send((SensorValue)a.Value);
            break;
          case SensorType.Motion:
            Motion.Send((SensorValue)a.Value);
            break;
          case SensorType.HeartBeat:
            HeartBeat.Send((SensorValue)a.Value);
            break;
          case SensorType.Tilt:
            Tilt.Send((SensorVector)a.Value);
            break;
        }

      };
    }

    public override void Stop()
    {
      Compass.Stop();
      AcceleroMeter.Stop();
      MagnetoMeter.Stop();
      Gyroscope.Stop();
      Light.Stop();
      Pressure.Stop();
      Proximity.Stop();
      LinearAcceleration.Stop();
      Rotation.Stop();
      GameRotation.Stop();
      Humidity.Stop();
      AmbientTemperature.Stop();
      SignificantMotion.Stop();
      StepDetector.Stop();
      StepCounter.Stop();
      HeartRate.Stop();
      Pose.Stop();
      Stationary.Stop();
      Motion.Stop();
      HeartBeat.Stop();
      Tilt.Stop();
    }

  }
}
