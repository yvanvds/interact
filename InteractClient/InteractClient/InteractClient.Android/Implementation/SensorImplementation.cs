using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using InteractClient.Interface;
using Android.Hardware;
using Xamarin.Forms;
using InteractClient.Droid.Implementation;

[assembly: Dependency(typeof(SensorImplementation))]
namespace InteractClient.Droid.Implementation
{
  public class SensorTriggerEventListener : TriggerEventListener
  {
    public Action<TriggerEvent> Callback;

    public override void OnTrigger(TriggerEvent e)
    {
      Callback(e);
    }
  }

  public class SensorImplementation : Java.Lang.Object, ISensorEventListener, ISensor
  {
    private SensorManager sensorManager;
    private Sensor sensorAccelerometer;
    private Sensor sensorGyroscope;
    private Sensor sensorMagnetometer;
    private Sensor sensorCompass;
    private Sensor sensorLight;
    private Sensor sensorPressure;
    private Sensor sensorProximity;
    private Sensor sensorLinearAcceleration;
    private Sensor sensorRotation;
    private Sensor sensorGameRotation;
    private Sensor sensorHumidity;
    private Sensor sensorAmbientTemperature;
    private Sensor sensorSignificantMotion;
    private Sensor sensorStepDetector;
    private Sensor sensorStepCounter;
    private Sensor sensorHeartRate;
    private Sensor sensorPose;
    private Sensor sensorStationary;
    private Sensor sensorMotion;
    private Sensor sensorHeartBeat;

    private IDictionary<Interact.SensorType, bool> sensorStatus;
    public SensorTriggerEventListener TriggerEventListener;
    public event SensorValueChangedEventHandler SensorValueChanged;

    // gyroscope calculations
    //private static float NS2S = 1.0f / 1000000000.0f;
    //private float[] gyroDeltaRotationVector = new float[4];
    //private float gyroTimestamp = 0;

    // tilt calculations
    private float[] tiltGravity;
    private float[] tiltMagnetic;

    public SensorImplementation() : base()
    {
      sensorManager = (SensorManager)Android.App.Application.Context.GetSystemService(Context.SensorService);

      sensorAccelerometer = sensorManager.GetDefaultSensor(SensorType.Accelerometer);
      sensorGyroscope = sensorManager.GetDefaultSensor(SensorType.Gyroscope);
      sensorMagnetometer = sensorManager.GetDefaultSensor(SensorType.MagneticField);
      sensorCompass = sensorManager.GetDefaultSensor(SensorType.Orientation);
      sensorLight = sensorManager.GetDefaultSensor(SensorType.Light);
      sensorPressure = sensorManager.GetDefaultSensor(SensorType.Pressure);
      sensorProximity = sensorManager.GetDefaultSensor(SensorType.Proximity);
      sensorLinearAcceleration = sensorManager.GetDefaultSensor(SensorType.LinearAcceleration);
      sensorRotation = sensorManager.GetDefaultSensor(SensorType.RotationVector);
      sensorGameRotation = sensorManager.GetDefaultSensor(SensorType.GameRotationVector);
      sensorHumidity = sensorManager.GetDefaultSensor(SensorType.RelativeHumidity);
      sensorAmbientTemperature = sensorManager.GetDefaultSensor(SensorType.AmbientTemperature);
      sensorSignificantMotion = sensorManager.GetDefaultSensor(SensorType.SignificantMotion);
      sensorStepDetector = sensorManager.GetDefaultSensor(SensorType.StepDetector);
      sensorStepCounter = sensorManager.GetDefaultSensor(SensorType.StepCounter);
      sensorHeartRate = sensorManager.GetDefaultSensor(SensorType.HeartRate);
      sensorPose = sensorManager.GetDefaultSensor(SensorType.Pose6dof);
      sensorStationary = sensorManager.GetDefaultSensor(SensorType.StationaryDetect);
      sensorMotion = sensorManager.GetDefaultSensor(SensorType.MotionDetect);
      sensorHeartBeat = sensorManager.GetDefaultSensor(SensorType.HeartBeat);

      sensorStatus = new Dictionary<Interact.SensorType, bool>
      {
        {Interact.SensorType.AcceleroMeter, false },
        {Interact.SensorType.Gyroscope, false },
        {Interact.SensorType.MagnetoMeter, false },
        {Interact.SensorType.Compass, false },
        {Interact.SensorType.Light,false },
        {Interact.SensorType.Pressure, false },
        {Interact.SensorType.Proximity, false },
        {Interact.SensorType.LinearAcceleration, false },
        {Interact.SensorType.Rotation, false },
        {Interact.SensorType.GameRotation, false },
        {Interact.SensorType.Humidity, false },
        {Interact.SensorType.AmbientTemperature, false },
        {Interact.SensorType.SignificantMotion, false },
        {Interact.SensorType.StepDetector, false },
        {Interact.SensorType.StepCounter, false },
        {Interact.SensorType.HeartRate, false },
        {Interact.SensorType.Pose, false },
        {Interact.SensorType.Stationary, false },
        {Interact.SensorType.Motion, false },
        {Interact.SensorType.HeartBeat, false },
        {Interact.SensorType.Tilt, false },
      };

      TriggerEventListener = new SensorTriggerEventListener();
      TriggerEventListener.Callback = OnSensorTrigger;
    }

    
    

    public bool IsActive(Interact.SensorType sensorType)
    {
      return sensorStatus[sensorType];
    }

    public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
    {
      
    }

    public void OnSensorTrigger(TriggerEvent e) {
      if (SensorValueChanged == null) return;

      switch(e.Sensor.Type)
      {
        case SensorType.SignificantMotion:
          {
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {
              ValueType = Interact.SensorValueType.Trigger,
              SensorType = Interact.SensorType.SignificantMotion,
              Value = new Interact.Utility.SensorValue() { Value = 1 },
            });
            if(IsActive(Interact.SensorType.SignificantMotion))
            {
              Start(Interact.SensorType.SignificantMotion);
            }
            break;
          }
      }

      
    }

    public void OnSensorChanged(SensorEvent e)
    {
      if (SensorValueChanged == null) return;

      switch(e.Sensor.Type)
      {
        case SensorType.Accelerometer:
          {
            if(sensorStatus[Interact.SensorType.AcceleroMeter])
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {
              ValueType = Interact.SensorValueType.Vector, 
              SensorType = Interact.SensorType.AcceleroMeter,
              Value = new Interact.Utility.SensorVector() { X = e.Values[0], Y = e.Values[1], Z = e.Values[2] },
            });
            tiltGravity = e.Values.ToArray();
            break;
          }

        case SensorType.Gyroscope:
          {
            /*
            if (gyroTimestamp != 0)
            {
              float dT = (e.Timestamp - gyroTimestamp) * NS2S;

              // axis of the rotation sample, not normalized yet
              float axisX = e.Values[0];
              float axisY = e.Values[1];
              float axisZ = e.Values[2];

              // calculate the angular speed of the sample
              float omegaMagnitude = (float)Math.Sqrt(axisX * axisX + axisY * axisY + axisZ * axisZ);

              // normalize the rotation vector if it's big enough to get the axis
              // (that is, EPSILON should represent your maximum allowable margin of error)
              if(omegaMagnitude > 0.5)
              {
                axisX /= omegaMagnitude;
                axisY /= omegaMagnitude;
                axisZ /= omegaMagnitude;
              }

              // Integrate around this axis with the angular speed by the timestep 
              // in order to get a delta rotation from this sample over the timestep
              // We will convert this axis-angle representation of the delta rotation
              // into a quaternion before turning it into the rotation matrix.

              float thetaOverTwo = omegaMagnitude * dT / 2.0f;
              float sinThetaOverTwo = (float)Math.Sin(thetaOverTwo);
              float cosThetaOverTwo = (float)Math.Cos(thetaOverTwo);
              gyroDeltaRotationVector[0] = sinThetaOverTwo * axisX;
              gyroDeltaRotationVector[1] = sinThetaOverTwo * axisY;
              gyroDeltaRotationVector[2] = sinThetaOverTwo * axisZ;
              gyroDeltaRotationVector[3] = cosThetaOverTwo;
            }

            gyroTimestamp = e.Timestamp;
            float[] deltaRotationMatrix = new float[9];
            SensorManager.GetRotationMatrixFromVector(deltaRotationMatrix, gyroDeltaRotationVector);
            */

            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {
              ValueType = Interact.SensorValueType.Vector,
              SensorType = Interact.SensorType.Gyroscope,
              Value = new Interact.Utility.SensorVector() { X = e.Values[0], Y = e.Values[1], Z = e.Values[2] },
            });
            break;
          }

        case SensorType.MagneticField:
          {
            if(sensorStatus[Interact.SensorType.MagnetoMeter])
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {
              ValueType = Interact.SensorValueType.Vector,
              SensorType = Interact.SensorType.MagnetoMeter,
              Value = new Interact.Utility.SensorVector() { X = e.Values[0], Y = e.Values[1], Z = e.Values[2] },
            });
            tiltMagnetic = e.Values.ToArray();
            break;
          }

        case SensorType.Orientation:
          {
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {
              ValueType = Interact.SensorValueType.Single,
              SensorType = Interact.SensorType.Compass,
              Value = new Interact.Utility.SensorValue() { Value = e.Values[0] },
            });
            break;
          }

        case SensorType.Light:
          {
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {
              ValueType = Interact.SensorValueType.Single,
              SensorType = Interact.SensorType.Light,
              Value = new Interact.Utility.SensorValue() { Value = e.Values[0] },
            });
            break;
          }

        case SensorType.Pressure:
          {
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {
              ValueType = Interact.SensorValueType.Single,
              SensorType = Interact.SensorType.Pressure,
              Value = new Interact.Utility.SensorValue() { Value = e.Values[0] },
            });
            break;
          }
        case SensorType.Proximity:
          {
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {
              ValueType = Interact.SensorValueType.Single,
              SensorType = Interact.SensorType.Proximity,
              Value = new Interact.Utility.SensorValue() { Value = e.Values[0] },
            });
            break;
          }
        case SensorType.LinearAcceleration:
          {
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {
              ValueType = Interact.SensorValueType.Vector,
              SensorType = Interact.SensorType.LinearAcceleration,
              Value = new Interact.Utility.SensorVector() { X = e.Values[0], Y = e.Values[1], Z = e.Values[2] },
            });
            break;
          }
        case SensorType.RotationVector:
          {
            float[] rotationMatrix = new float[16];
            SensorManager.GetRotationMatrixFromVector(rotationMatrix, e.Values.ToArray());

            // remap coordinate system
            float[] remappedRotationMatrix = new float[16];
            SensorManager.RemapCoordinateSystem(rotationMatrix,
              Android.Hardware.Axis.X, Android.Hardware.Axis.Z,
              remappedRotationMatrix);

            // convert to orientations
            float[] orientations = new float[3];
            SensorManager.GetOrientation(remappedRotationMatrix, orientations);

            // convert to degrees
            for(int i = 0; i < 3; i++)
            {
              orientations[i] = (float)(orientations[i] * (180.0f / Math.PI));
            }

            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {
              ValueType = Interact.SensorValueType.Vector,
              SensorType = Interact.SensorType.Rotation,
              Value = new Interact.Utility.SensorVector() { X = orientations[0], Y = orientations[1], Z = orientations[2] },
            });
            break;
          }
        case SensorType.GameRotationVector:
          {
            float[] rotationMatrix = new float[16];
            SensorManager.GetRotationMatrixFromVector(rotationMatrix, e.Values.ToArray());

            // remap coordinate system
            float[] remappedRotationMatrix = new float[16];
            SensorManager.RemapCoordinateSystem(rotationMatrix,
              Android.Hardware.Axis.X, Android.Hardware.Axis.Z,
              remappedRotationMatrix);

            // convert to orientations
            float[] orientations = new float[3];
            SensorManager.GetOrientation(remappedRotationMatrix, orientations);

            // convert to degrees
            for (int i = 0; i < 3; i++)
            {
              orientations[i] = (float)(orientations[i] * (180.0f / Math.PI));
            }

            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {
              ValueType = Interact.SensorValueType.Vector,
              SensorType = Interact.SensorType.GameRotation,
              Value = new Interact.Utility.SensorVector() { X = orientations[0], Y = orientations[1], Z = orientations[2] },
            });
            break;
          }
        case SensorType.RelativeHumidity:
          {
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {
              ValueType = Interact.SensorValueType.Single,
              SensorType = Interact.SensorType.Humidity,
              Value = new Interact.Utility.SensorValue() { Value = e.Values[0] },
            });
            break;
          }
        case SensorType.AmbientTemperature:
          {
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {
              ValueType = Interact.SensorValueType.Single,
              SensorType = Interact.SensorType.AmbientTemperature,
              Value = new Interact.Utility.SensorValue() { Value = e.Values[0] },
            });
            break;
          }
        case SensorType.StepDetector:
          {
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {
              ValueType = Interact.SensorValueType.Trigger,
              SensorType = Interact.SensorType.StepDetector,
              Value = new Interact.Utility.SensorValue() { Value = 1 },
            });
            break;
          }

        case SensorType.StepCounter:
          {
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {

            });
            break;
          }
        case SensorType.HeartRate:
          {
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {

            });
            break;
          }
        case SensorType.Pose6dof:
          {
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {

            });
            break;
          }
        case SensorType.StationaryDetect:
          {
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {

            });
            break;
          }
        case SensorType.MotionDetect:
          {
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {

            });
            break;
          }
        case SensorType.HeartBeat:
          {
            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {

            });
            break;
          }
      }

      if(sensorStatus[Interact.SensorType.Tilt] && e.Sensor.Type == SensorType.MagneticField)
      {
        if(tiltGravity != null && tiltMagnetic != null)
        {
          float[] R = new float[9];
          if(SensorManager.GetRotationMatrix(R, null, tiltGravity, tiltMagnetic))
          {
            float[] orientation = new float[9];
            SensorManager.GetOrientation(R, orientation);
            float roll = orientation[2];
            int rollDeg = (int)Math.Round(roll * (180.0f / Math.PI));
            int degrees = rollDeg;
            // tilted back towards user more than -90 degrees
            if (degrees < -90) degrees = -90;
            // tilted forward past 0 degrees
            else if (degrees > 0) degrees = 0;
            // normalize into a positive value
            degrees *= -1;
            // invert from 90-0 to 0-90
            degrees = 90 - degrees;
            // convert to scale of 0-100
            float power = degrees / 90f * 100f;

            float pitch = orientation[1];
            int pitchDeg = -(int)Math.Round(pitch * (180f / Math.PI));


            SensorValueChanged(this, new SensorValueChangedEventArgs()
            {
              ValueType = Interact.SensorValueType.Vector,
              SensorType = Interact.SensorType.Tilt,
              Value = new Interact.Utility.SensorVector() { X = rollDeg, Y = pitchDeg, Z = power },
            });
          }
          
        }
      }
    }

    public void Start(Interact.SensorType sensorType, Interact.SensorDelay interval = Interact.SensorDelay.Default)
    {
      Android.Hardware.SensorDelay delay = Android.Hardware.SensorDelay.Normal;
      switch(interval)
      {
        case Interact.SensorDelay.Fastest: delay = Android.Hardware.SensorDelay.Fastest; break;
        case Interact.SensorDelay.Game: delay = Android.Hardware.SensorDelay.Game; break;
        case Interact.SensorDelay.Ui: delay = Android.Hardware.SensorDelay.Ui; break;
      }

      switch(sensorType)
      {
        case Interact.SensorType.AcceleroMeter:
          if (sensorAccelerometer != null)
            sensorManager.RegisterListener(this, sensorAccelerometer, delay);
          else Network.Service.Get().WriteLog("Sensor: AcceleroMeter not available.");
          break;

        case Interact.SensorType.Gyroscope:
          if (sensorGyroscope != null)
            sensorManager.RegisterListener(this, sensorGyroscope, delay);
          else Network.Service.Get().WriteLog("Sensor: Gyroscope not available.");
          break;

        case Interact.SensorType.MagnetoMeter:
          if (sensorMagnetometer != null)
            sensorManager.RegisterListener(this, sensorMagnetometer, delay);
          else Network.Service.Get().WriteLog("Sensor: Magnetometer not available.");
          break;

        case Interact.SensorType.Compass:
          if (sensorCompass != null)
            sensorManager.RegisterListener(this, sensorCompass, delay);
          else Network.Service.Get().WriteLog("Sensor: Compass not available.");
          break;

        case Interact.SensorType.Light:
          if(sensorLight != null)
            sensorManager.RegisterListener(this, sensorLight, delay);
          else Network.Service.Get().WriteLog("Sensor: Light not available.");
          break;

        case Interact.SensorType.Pressure:
          if (sensorPressure != null)
            sensorManager.RegisterListener(this, sensorPressure, delay);
          else Network.Service.Get().WriteLog("Sensor: Pressure not available.");
          break;

        case Interact.SensorType.Proximity:
          if (sensorProximity != null)
            sensorManager.RegisterListener(this, sensorProximity, delay);
          else Network.Service.Get().WriteLog("Sensor: Proximity not available.");
          break;

        case Interact.SensorType.LinearAcceleration:
          if (sensorLinearAcceleration != null)
            sensorManager.RegisterListener(this, sensorLinearAcceleration, delay);
          else Network.Service.Get().WriteLog("Sensor: Linear Acceleration not available.");
          break;

        case Interact.SensorType.Rotation:
          if (sensorRotation != null)
            sensorManager.RegisterListener(this, sensorRotation, delay);
          else Network.Service.Get().WriteLog("Sensor: Rotation not available.");
          break;

        case Interact.SensorType.GameRotation:
          if (sensorGameRotation != null)
            sensorManager.RegisterListener(this, sensorGameRotation, delay);
          else Network.Service.Get().WriteLog("Sensor: Game Rotation not available.");
          break;

        case Interact.SensorType.Humidity:
          if (sensorHumidity != null)
            sensorManager.RegisterListener(this, sensorHumidity, delay);
          else Network.Service.Get().WriteLog("Sensor: Humidity not available.");
          break;

        case Interact.SensorType.AmbientTemperature:
          if (sensorAmbientTemperature != null)
            sensorManager.RegisterListener(this, sensorAmbientTemperature, delay);
          else Network.Service.Get().WriteLog("Sensor: Ambient Temperature not available.");
          break;

        case Interact.SensorType.SignificantMotion:
          if (sensorSignificantMotion != null)
            sensorManager.RequestTriggerSensor(TriggerEventListener, sensorSignificantMotion);
          else Network.Service.Get().WriteLog("Sensor: Significant Motion not available.");
          break;

        case Interact.SensorType.StepDetector:
          if (sensorStepDetector != null)
            sensorManager.RequestTriggerSensor(TriggerEventListener, sensorStepDetector);
          else Network.Service.Get().WriteLog("Sensor: Step Detector not available.");
          break;

        case Interact.SensorType.StepCounter:
          if (sensorStepCounter != null)
            sensorManager.RegisterListener(this, sensorStepCounter, delay);
          else Network.Service.Get().WriteLog("Sensor: Step Counter not available.");
          break;

        case Interact.SensorType.HeartRate:
          if (sensorHeartRate != null)
            sensorManager.RegisterListener(this, sensorHeartRate, delay);
          else Network.Service.Get().WriteLog("Sensor: Heart Rate not available.");
          break;

        case Interact.SensorType.Pose:
          if (sensorPose != null)
            sensorManager.RegisterListener(this, sensorPose, delay);
          else Network.Service.Get().WriteLog("Sensor: Pose not available.");
          break;

        case Interact.SensorType.Stationary:
          if (sensorStationary != null)
            sensorManager.RegisterListener(this, sensorStationary, delay);
          else Network.Service.Get().WriteLog("Sensor: Stationary not available.");
          break;

        case Interact.SensorType.Motion:
          if (sensorMotion != null)
            sensorManager.RegisterListener(this, sensorMotion, delay);
          else Network.Service.Get().WriteLog("Sensor: Motion not available.");
          break;

        case Interact.SensorType.HeartBeat:
          if (sensorHeartBeat != null)
            sensorManager.RegisterListener(this, sensorHeartBeat, delay);
          else Network.Service.Get().WriteLog("Sensor: Heart Beat not available.");
          break;
        case Interact.SensorType.Tilt:
          if(sensorMagnetometer != null && sensorAccelerometer != null)
          {
            sensorManager.RegisterListener(this, sensorMagnetometer, delay);
            sensorManager.RegisterListener(this, sensorAccelerometer, delay);
          } else Network.Service.Get().WriteLog("Sensor: Tilt not available.");
          break;
      }
      sensorStatus[sensorType] = true;
    }

    public void Stop(Interact.SensorType sensorType)
    {
      switch(sensorType)
      {
        case Interact.SensorType.AcceleroMeter:
          if (sensorAccelerometer != null && sensorStatus[Interact.SensorType.Tilt] == false)
            sensorManager.UnregisterListener(this, sensorAccelerometer);
          break;
        case Interact.SensorType.Gyroscope:
          if(sensorGyroscope != null)
            sensorManager.UnregisterListener(this, sensorGyroscope);
          break;
        case Interact.SensorType.MagnetoMeter:
          if (sensorMagnetometer != null && sensorStatus[Interact.SensorType.Tilt] == false)
            sensorManager.UnregisterListener(this, sensorMagnetometer);
          break;
        case Interact.SensorType.Compass:
          if(sensorCompass != null)
            sensorManager.UnregisterListener(this, sensorCompass);
          break;
        case Interact.SensorType.Light:
          if (sensorLight != null)
            sensorManager.UnregisterListener(this, sensorLight);
          break;
        case Interact.SensorType.Pressure:
          if (sensorPressure != null)
            sensorManager.UnregisterListener(this, sensorPressure);
          break;
        case Interact.SensorType.Proximity:
          if (sensorProximity != null)
            sensorManager.UnregisterListener(this, sensorProximity);
          break;
        case Interact.SensorType.LinearAcceleration:
          if (sensorLinearAcceleration != null)
            sensorManager.UnregisterListener(this, sensorLinearAcceleration);
          break;
        case Interact.SensorType.Rotation:
          if (sensorRotation != null)
            sensorManager.UnregisterListener(this, sensorRotation);
          break;
        case Interact.SensorType.GameRotation:
          if (sensorGameRotation != null)
            sensorManager.UnregisterListener(this, sensorGameRotation);
          break;
        case Interact.SensorType.Humidity:
          if (sensorHumidity != null)
            sensorManager.UnregisterListener(this, sensorHumidity);
          break;
        case Interact.SensorType.AmbientTemperature:
          if (sensorAmbientTemperature != null)
            sensorManager.UnregisterListener(this, sensorAmbientTemperature);
          break;
        case Interact.SensorType.SignificantMotion:
          if (sensorSignificantMotion != null)
            sensorManager.CancelTriggerSensor(TriggerEventListener, sensorSignificantMotion);
          break;
        case Interact.SensorType.StepDetector:
          if (sensorStepDetector != null)
            sensorManager.CancelTriggerSensor(TriggerEventListener, sensorStepDetector);
          break;
        case Interact.SensorType.StepCounter:
          if (sensorStepCounter != null)
            sensorManager.UnregisterListener(this, sensorStepCounter);
          break;
        case Interact.SensorType.HeartRate:
          if (sensorHeartRate != null)
            sensorManager.UnregisterListener(this, sensorHeartRate);
          break;
        case Interact.SensorType.Pose:
          if (sensorPose != null)
            sensorManager.UnregisterListener(this, sensorPose);
          break;
        case Interact.SensorType.Stationary:
          if (sensorStationary != null)
            sensorManager.UnregisterListener(this, sensorStationary);
          break;
        case Interact.SensorType.Motion:
          if (sensorMotion != null)
            sensorManager.UnregisterListener(this, sensorMotion);
          break;
        case Interact.SensorType.HeartBeat:
          if (sensorHeartBeat != null)
            sensorManager.UnregisterListener(this, sensorHeartBeat);
          break;
        case Interact.SensorType.Tilt:
          if (sensorAccelerometer != null && sensorStatus[Interact.SensorType.AcceleroMeter] == false)
            sensorManager.UnregisterListener(this, sensorAccelerometer);
          if (sensorMagnetometer != null && sensorStatus[Interact.SensorType.MagnetoMeter] == false)
            sensorManager.UnregisterListener(this, sensorMagnetometer);
          break;
      }
      sensorStatus[sensorType] = false;
    }
  }
}