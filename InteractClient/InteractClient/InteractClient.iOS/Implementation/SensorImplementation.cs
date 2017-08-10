using InteractClient.Interface;
using InteractClient.iOS.Implementation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Interact;

using CoreLocation;
using Foundation;
using CoreMotion;
using UIKit;

[assembly: Dependency(typeof(SensorImplementation))]
namespace InteractClient.iOS.Implementation
{
  public abstract class SensorImplementation : ISensor, IDisposable
  {
    private double ms = 1000.0;
    private CMMotionManager motionManager;
    private CLLocationManager locationManager;

    private IDictionary<Interact.SensorType, bool> sensorStatus;

    public event SensorValueChangedEventHandler SensorValueChanged;

    public SensorImplementation()
    {
      motionManager = new CMMotionManager();
      locationManager = new CLLocationManager();

      if (locationManager != null)
      {
        locationManager.PausesLocationUpdatesAutomatically = false;
        if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
        {
          locationManager.RequestAlwaysAuthorization();
        }
        if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
        {
          locationManager.AllowsBackgroundLocationUpdates = true;
        }

        locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
        locationManager.HeadingFilter = 1;
      }

      sensorStatus = new Dictionary<SensorType, bool>
      {
        {SensorType.AcceleroMeter, false },
        {SensorType.Gyroscope, false },
        {SensorType.MagnetoMeter, false },
        {SensorType.Compass, false },
      };
    }

    public bool IsActive(SensorType sensorType)
    {
      return sensorStatus[sensorType];
    }

    public void Start(SensorType sensorType, SensorDelay interval = SensorDelay.Default)
    {
      switch(sensorType)
      {
        case SensorType.AcceleroMeter: 
          if(motionManager != null && motionManager.AccelerometerAvailable)
          {
            motionManager.AccelerometerUpdateInterval = (double)interval / ms;
            motionManager.StartAccelerometerUpdates(NSOperationQueue.CurrentQueue, OnAccelerometerChanged);
          } else
          {
            Network.Signaler.Get().WriteLog("Sensor: AcceleroMeter not available.");
          }
          break;
        case SensorType.Gyroscope:
          if(motionManager != null && motionManager.GyroAvailable)
          {
            motionManager.GyroUpdateInterval = (double)interval / ms;
            motionManager.StartGyroUpdates(NSOperationQueue.CurrentQueue, OnGyroscopeChanged);
          }
          else
          {
            Network.Signaler.Get().WriteLog("Sensor: Gyroscope not available.");
          }
          break;
        case SensorType.MagnetoMeter:
          if (motionManager != null && motionManager.MagnetometerAvailable)
          {
            motionManager.MagnetometerUpdateInterval = (double)interval / ms;
            motionManager.StartMagnetometerUpdates(NSOperationQueue.CurrentQueue, OnMagnometerChanged);
          }
          else
          {
            Network.Signaler.Get().WriteLog("Sensor: Magnetometer not available.");
          }
          break;
        case SensorType.Compass:
          if (locationManager != null && CLLocationManager.HeadingAvailable)
          {
            locationManager.StartUpdatingHeading();
            locationManager.UpdatedHeading += OnHeadingChanged;
          }
          else
          {
            Network.Signaler.Get().WriteLog("Sensor: Compass not available.");
          }
          break;
      }
      sensorStatus[sensorType] = true;
    }

    private void OnHeadingChanged(object sender, CLHeadingUpdatedEventArgs e)
    {
      if (SensorValueChanged == null)
        return;
      SensorValueChanged(
        this, 
        new SensorValueChangedEventArgs {
          ValueType = SensorValueType.Single,
          SensorType = SensorType.Compass,
          Value = new Interact.Utility.SensorValue {
            Value = e.NewHeading.TrueHeading
          }
        });
    }

    private void OnMagnometerChanged(CMMagnetometerData data, NSError error)
    {
      if (SensorValueChanged == null)
        return;
      SensorValueChanged(this, new SensorValueChangedEventArgs { ValueType = SensorValueType.Vector, SensorType = SensorType.MagnetoMeter, Value = new Interact.Utility.SensorVector() { X = data.MagneticField.X, Y = data.MagneticField.Y, Z = data.MagneticField.Z } });
    }

    private void OnAccelerometerChanged(CMAccelerometerData data, NSError error)
    {
      if (SensorValueChanged == null)
        return;
      SensorValueChanged(this, new SensorValueChangedEventArgs { ValueType = SensorValueType.Vector, SensorType = SensorType.AcceleroMeter, Value = new Interact.Utility.SensorVector() { X = data.Acceleration.X, Y = data.Acceleration.Y, Z = data.Acceleration.Z } });
    }

    private void OnGyroscopeChanged(CMGyroData data, NSError error)
    {
      if (SensorValueChanged == null)
        return;
      SensorValueChanged(this, new SensorValueChangedEventArgs { ValueType = SensorValueType.Vector, SensorType = SensorType.Gyroscope, Value = new Interact.Utility.SensorVector() { X = data.RotationRate.x, Y = data.RotationRate.y, Z = data.RotationRate.z } });
    }

    public void Stop(SensorType sensorType)
    {
      switch (sensorType)
      {
        case SensorType.AcceleroMeter:
          if (motionManager.AccelerometerActive)
            motionManager.StopAccelerometerUpdates();
          break;

        case SensorType.Gyroscope:
          if (motionManager.GyroActive)
            motionManager.StopGyroUpdates();
          break;

        case SensorType.MagnetoMeter:
          if (motionManager.MagnetometerActive)
            motionManager.StopMagnetometerUpdates();
          break;

        case SensorType.Compass:
          if (CLLocationManager.HeadingAvailable)
          {
            locationManager.StopUpdatingHeading();
            locationManager.UpdatedHeading -= OnHeadingChanged;
          }
          break;

      }
      sensorStatus[sensorType] = false;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    ~SensorImplementation()
    {
      Dispose(false);
    }

    private bool disposed = false;

    public virtual void Dispose(bool disposing)
    {
      if (!disposed)
      {
        if (disposing)
        {
          //dispose only
        }
        disposed = true;
      }
    }
  }
}
