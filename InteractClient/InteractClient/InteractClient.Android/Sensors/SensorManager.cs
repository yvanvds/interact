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
using Android.Hardware;
using InteractClient.Sensors;

namespace InteractClient.Droid.Sensors
{
	public class SensorManager : Java.Lang.Object, ISensorEventListener, InteractClient.Sensors.ISensor
	{
		private Android.Hardware.SensorManager sensorManager;
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

		private IDictionary<InteractClient.Sensors.SensorType, bool> sensorStatus;
		public SensorTriggerEventListener TriggerEventListener;
		public event SensorValueChangedEventHandler SensorValueChanged;

		// tilt calculations
		private float[] tiltGravity;
		private float[] tiltMagnetic;

		public SensorManager() : base()
		{
			sensorManager = (Android.Hardware.SensorManager)Android.App.Application.Context.GetSystemService(Context.SensorService);

			sensorAccelerometer = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.Accelerometer);
			sensorGyroscope = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.Gyroscope);
			sensorMagnetometer = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.MagneticField);
			sensorCompass = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.Orientation);
			sensorLight = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.Light);
			sensorPressure = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.Pressure);
			sensorProximity = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.Proximity);
			sensorLinearAcceleration = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.LinearAcceleration);
			sensorRotation = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.RotationVector);
			sensorGameRotation = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.GameRotationVector);
			sensorHumidity = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.RelativeHumidity);
			sensorAmbientTemperature = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.AmbientTemperature);
			sensorSignificantMotion = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.SignificantMotion);
			sensorStepDetector = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.StepDetector);
			sensorStepCounter = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.StepCounter);
			sensorHeartRate = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.HeartRate);
			sensorPose = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.Pose6dof);
			sensorStationary = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.StationaryDetect);
			sensorMotion = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.MotionDetect);
			sensorHeartBeat = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.HeartBeat);


			sensorStatus = new Dictionary<InteractClient.Sensors.SensorType, bool>
			{
				{InteractClient.Sensors.SensorType.AcceleroMeter, false },
				{InteractClient.Sensors.SensorType.Gyroscope, false },
				{InteractClient.Sensors.SensorType.MagnetoMeter, false },
				{InteractClient.Sensors.SensorType.Compass, false },
				{InteractClient.Sensors.SensorType.Light,false },
				{InteractClient.Sensors.SensorType.Pressure, false },
				{InteractClient.Sensors.SensorType.Proximity, false },
				{InteractClient.Sensors.SensorType.LinearAcceleration, false },
				{InteractClient.Sensors.SensorType.Rotation, false },
				{InteractClient.Sensors.SensorType.GameRotation, false },
				{InteractClient.Sensors.SensorType.Humidity, false },
				{InteractClient.Sensors.SensorType.AmbientTemperature, false },
				{InteractClient.Sensors.SensorType.SignificantMotion, false },
				{InteractClient.Sensors.SensorType.StepDetector, false },
				{InteractClient.Sensors.SensorType.StepCounter, false },
				{InteractClient.Sensors.SensorType.HeartRate, false },
				{InteractClient.Sensors.SensorType.Pose, false },
				{InteractClient.Sensors.SensorType.Stationary, false },
				{InteractClient.Sensors.SensorType.Motion, false },
				{InteractClient.Sensors.SensorType.HeartBeat, false },
				{InteractClient.Sensors.SensorType.Tilt, false },
			};

			TriggerEventListener = new SensorTriggerEventListener();
			TriggerEventListener.Callback = OnSensorTrigger;
		}

		public bool IsActive(InteractClient.Sensors.SensorType sensorType)
		{
			return sensorStatus[sensorType];
		}

		public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
		{

		}

		#region Triggers
		public void OnSensorTrigger(TriggerEvent e)
		{
			if (SensorValueChanged == null) return;

			switch (e.Sensor.Type)
			{
				case Android.Hardware.SensorType.SignificantMotion:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Trigger,
							SensorType = InteractClient.Sensors.SensorType.SignificantMotion,
							Value = 1,
						});
						if (IsActive(InteractClient.Sensors.SensorType.SignificantMotion))
						{
							Start(InteractClient.Sensors.SensorType.SignificantMotion);
						}
						break;
					}
				default:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = SensorValueType.Trigger,
							SensorType = InteractClient.Sensors.SensorType.StepDetector,
							Value = e.Values[0],
						});
						break;
					}
			}
		}

		public void OnSensorChanged(SensorEvent e)
		{
			if (SensorValueChanged == null) return;

			switch(e.Sensor.Type)
			{
				case Android.Hardware.SensorType.Accelerometer:
					{
						if (sensorStatus[InteractClient.Sensors.SensorType.AcceleroMeter])
							SensorValueChanged(this, new SensorValueChangedEventArgs()
							{
								ValueType = InteractClient.Sensors.SensorValueType.Vector,
								SensorType = InteractClient.Sensors.SensorType.AcceleroMeter,
								Value = e.Values.ToArray(),
							});
						tiltGravity = e.Values.ToArray();
						break;
					}

				case Android.Hardware.SensorType.Gyroscope:
					{
						if(sensorStatus[InteractClient.Sensors.SensorType.Gyroscope])
						{
							SensorValueChanged(this, new SensorValueChangedEventArgs()
							{
								ValueType = InteractClient.Sensors.SensorValueType.Vector,
								SensorType = InteractClient.Sensors.SensorType.Gyroscope,
								Value = e.Values.ToArray(),
							});
						}
						break;
					}

				case Android.Hardware.SensorType.MagneticField:
					{
						if (sensorStatus[InteractClient.Sensors.SensorType.MagnetoMeter])
							SensorValueChanged(this, new SensorValueChangedEventArgs()
							{
								ValueType = InteractClient.Sensors.SensorValueType.Vector,
								SensorType = InteractClient.Sensors.SensorType.MagnetoMeter,
								Value = e.Values.ToArray(),
							});
						tiltMagnetic = e.Values.ToArray();
						break;
					}

				case Android.Hardware.SensorType.Orientation:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Single,
							SensorType = InteractClient.Sensors.SensorType.Compass,
							Value = e.Values[0],
						});
						break;
					}

				case Android.Hardware.SensorType.Light:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Single,
							SensorType = InteractClient.Sensors.SensorType.Light,
							Value = e.Values[0],
						});
						break;
					}

				case Android.Hardware.SensorType.Pressure:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Single,
							SensorType = InteractClient.Sensors.SensorType.Pressure,
							Value = e.Values[0],
						});
						break;
					}

				case Android.Hardware.SensorType.Proximity:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Single,
							SensorType = InteractClient.Sensors.SensorType.Proximity,
							Value = e.Values[0],
						});
						break;
					}

				case Android.Hardware.SensorType.LinearAcceleration:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Vector,
							SensorType = InteractClient.Sensors.SensorType.LinearAcceleration,
							Value = e.Values.ToArray(),
						});
						break;
					}

				case Android.Hardware.SensorType.RotationVector:
					{
						float[] rotationMatrix = new float[16];
						Android.Hardware.SensorManager.GetRotationMatrixFromVector(rotationMatrix, e.Values.ToArray());

						// remap coordinate system
						float[] remappedRotationMatrix = new float[16];
						Android.Hardware.SensorManager.RemapCoordinateSystem(rotationMatrix,
							Android.Hardware.Axis.X, Android.Hardware.Axis.Z,
							remappedRotationMatrix);

						// convert to orientations
						float[] orientations = new float[3];
						Android.Hardware.SensorManager.GetOrientation(remappedRotationMatrix, orientations);

						// convert to degrees
						for (int i = 0; i < 3; i++)
						{
							orientations[i] = (float)(orientations[i] * (180.0f / Math.PI));
						}

						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Vector,
							SensorType = InteractClient.Sensors.SensorType.Rotation,
							Value = orientations,
						});
						break;
					}

				case Android.Hardware.SensorType.GameRotationVector:
					{
						float[] rotationMatrix = new float[16];
						Android.Hardware.SensorManager.GetRotationMatrixFromVector(rotationMatrix, e.Values.ToArray());

						// remap coordinate system
						float[] remappedRotationMatrix = new float[16];
						Android.Hardware.SensorManager.RemapCoordinateSystem(rotationMatrix,
							Android.Hardware.Axis.X, Android.Hardware.Axis.Z,
							remappedRotationMatrix);

						// convert to orientations
						float[] orientations = new float[3];
						Android.Hardware.SensorManager.GetOrientation(remappedRotationMatrix, orientations);

						// convert to degrees
						for (int i = 0; i < 3; i++)
						{
							orientations[i] = (float)(orientations[i] * (180.0f / Math.PI));
						}

						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Vector,
							SensorType = InteractClient.Sensors.SensorType.GameRotation,
							Value = orientations,
						});
						break;
					}

				case Android.Hardware.SensorType.RelativeHumidity:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Single,
							SensorType = InteractClient.Sensors.SensorType.Humidity,
							Value = e.Values[0],
						});
						break;
					}

				case Android.Hardware.SensorType.AmbientTemperature:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Single,
							SensorType = InteractClient.Sensors.SensorType.AmbientTemperature,
							Value = e.Values[0],
						});
						break;
					}

				case Android.Hardware.SensorType.StepDetector:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Trigger,
							SensorType = InteractClient.Sensors.SensorType.StepDetector,
							Value = e.Values[0],
						});
						break;
					}

				case Android.Hardware.SensorType.StepCounter:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Trigger,
							SensorType = InteractClient.Sensors.SensorType.StepCounter,
							Value = e.Values[0],
						});
						break;
					}
				case Android.Hardware.SensorType.HeartRate:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Trigger,
							SensorType = InteractClient.Sensors.SensorType.HeartRate,
							Value = e.Values[0],
						});
						break;
					}
				case Android.Hardware.SensorType.Pose6dof:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{

						});
						break;
					}
				case Android.Hardware.SensorType.StationaryDetect:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Trigger,
							SensorType = InteractClient.Sensors.SensorType.Stationary,
							Value = e.Values[0],
						});
						break;
					}
				case Android.Hardware.SensorType.MotionDetect:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Trigger,
							SensorType = InteractClient.Sensors.SensorType.Motion,
							Value = e.Values[0],
						});
						break;
					}
				case Android.Hardware.SensorType.HeartBeat:
					{
						SensorValueChanged(this, new SensorValueChangedEventArgs()
						{
							ValueType = InteractClient.Sensors.SensorValueType.Trigger,
							SensorType = InteractClient.Sensors.SensorType.StepDetector,
							Value = e.Values[0],
						});
						break;
					}
			}

			if (sensorStatus[InteractClient.Sensors.SensorType.Tilt] && e.Sensor.Type == Android.Hardware.SensorType.MagneticField)
			{
				if (tiltGravity != null && tiltMagnetic != null)
				{
					float[] R = new float[9];
					if (Android.Hardware.SensorManager.GetRotationMatrix(R, null, tiltGravity, tiltMagnetic))
					{
						float[] orientation = new float[9];
						Android.Hardware.SensorManager.GetOrientation(R, orientation);
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
							ValueType = InteractClient.Sensors.SensorValueType.Vector,
							SensorType = InteractClient.Sensors.SensorType.Tilt,
							Value = new float[] { rollDeg, pitchDeg, power },
						});
					}

				}
			}
		}
		#endregion Triggers

		public void Start(InteractClient.Sensors.SensorType sensorType, InteractClient.Sensors.SensorDelay interval = InteractClient.Sensors.SensorDelay.Default)
		{
			Android.Hardware.SensorDelay delay = Android.Hardware.SensorDelay.Normal;
			switch (interval)
			{
				case InteractClient.Sensors.SensorDelay.Fastest: delay = Android.Hardware.SensorDelay.Fastest; break;
				case InteractClient.Sensors.SensorDelay.Game: delay = Android.Hardware.SensorDelay.Game; break;
				case InteractClient.Sensors.SensorDelay.Ui: delay = Android.Hardware.SensorDelay.Ui; break;
			}

			switch (sensorType)
			{
				case InteractClient.Sensors.SensorType.AcceleroMeter:
					if (sensorAccelerometer != null)
						sensorManager.RegisterListener(this, sensorAccelerometer, delay);
					else Network.Sender.WriteLog("Sensor: AcceleroMeter not available.");
					break;

				case InteractClient.Sensors.SensorType.Gyroscope:
					if (sensorGyroscope != null)
						sensorManager.RegisterListener(this, sensorGyroscope, delay);
					else Network.Sender.WriteLog("Sensor: Gyroscope not available.");
					break;

				case InteractClient.Sensors.SensorType.MagnetoMeter:
					if (sensorMagnetometer != null)
						sensorManager.RegisterListener(this, sensorMagnetometer, delay);
					else Network.Sender.WriteLog("Sensor: Magnetometer not available.");
					break;

				case InteractClient.Sensors.SensorType.Compass:
					if (sensorCompass != null)
						sensorManager.RegisterListener(this, sensorCompass, delay);
					else Network.Sender.WriteLog("Sensor: Compass not available.");
					break;

				case InteractClient.Sensors.SensorType.Light:
					if (sensorLight != null)
						sensorManager.RegisterListener(this, sensorLight, delay);
					else Network.Sender.WriteLog("Sensor: Light not available.");
					break;

				case InteractClient.Sensors.SensorType.Pressure:
					if (sensorPressure != null)
						sensorManager.RegisterListener(this, sensorPressure, delay);
					else Network.Sender.WriteLog("Sensor: Pressure not available.");
					break;

				case InteractClient.Sensors.SensorType.Proximity:
					if (sensorProximity != null)
						sensorManager.RegisterListener(this, sensorProximity, delay);
					else Network.Sender.WriteLog("Sensor: Proximity not available.");
					break;

				case InteractClient.Sensors.SensorType.LinearAcceleration:
					if (sensorLinearAcceleration != null)
						sensorManager.RegisterListener(this, sensorLinearAcceleration, delay);
					else Network.Sender.WriteLog("Sensor: Linear Acceleration not available.");
					break;

				case InteractClient.Sensors.SensorType.Rotation:
					if (sensorRotation != null)
						sensorManager.RegisterListener(this, sensorRotation, delay);
					else Network.Sender.WriteLog("Sensor: Rotation not available.");
					break;

				case InteractClient.Sensors.SensorType.GameRotation:
					if (sensorGameRotation != null)
						sensorManager.RegisterListener(this, sensorGameRotation, delay);
					else Network.Sender.WriteLog("Sensor: Game Rotation not available.");
					break;

				case InteractClient.Sensors.SensorType.Humidity:
					if (sensorHumidity != null)
						sensorManager.RegisterListener(this, sensorHumidity, delay);
					else Network.Sender.WriteLog("Sensor: Humidity not available.");
					break;

				case InteractClient.Sensors.SensorType.AmbientTemperature:
					if (sensorAmbientTemperature != null)
						sensorManager.RegisterListener(this, sensorAmbientTemperature, delay);
					else Network.Sender.WriteLog("Sensor: Ambient Temperature not available.");
					break;

				case InteractClient.Sensors.SensorType.SignificantMotion:
					if (sensorSignificantMotion != null)
						sensorManager.RequestTriggerSensor(TriggerEventListener, sensorSignificantMotion);
					else Network.Sender.WriteLog("Sensor: Significant Motion not available.");
					break;

				case InteractClient.Sensors.SensorType.StepDetector:
					if (sensorStepDetector != null)
						sensorManager.RegisterListener(this, sensorStepDetector, delay);
					else Network.Sender.WriteLog("Sensor: Step Detector not available.");
					break;

				case InteractClient.Sensors.SensorType.StepCounter:
					if (sensorStepCounter != null)
						sensorManager.RegisterListener(this, sensorStepCounter, delay);
					else Network.Sender.WriteLog("Sensor: Step Counter not available.");
					break;

				case InteractClient.Sensors.SensorType.HeartRate:
					if (sensorHeartRate != null)
						sensorManager.RegisterListener(this, sensorHeartRate, delay);
					else Network.Sender.WriteLog("Sensor: Heart Rate not available.");
					break;

				case InteractClient.Sensors.SensorType.Pose:
					if (sensorPose != null)
						sensorManager.RegisterListener(this, sensorPose, delay);
					else Network.Sender.WriteLog("Sensor: Pose not available.");
					break;

				case InteractClient.Sensors.SensorType.Stationary:
					if (sensorStationary != null)
						sensorManager.RegisterListener(this, sensorStationary, delay);
					else Network.Sender.WriteLog("Sensor: Stationary not available.");
					break;

				case InteractClient.Sensors.SensorType.Motion:
					if (sensorMotion != null)
						sensorManager.RegisterListener(this, sensorMotion, delay);
					else Network.Sender.WriteLog("Sensor: Motion not available.");
					break;

				case InteractClient.Sensors.SensorType.HeartBeat:
					if (sensorHeartBeat != null)
						sensorManager.RegisterListener(this, sensorHeartBeat, delay);
					else Network.Sender.WriteLog("Sensor: Heart Beat not available.");
					break;
				case InteractClient.Sensors.SensorType.Tilt:
					if (sensorMagnetometer != null && sensorAccelerometer != null)
					{
						sensorManager.RegisterListener(this, sensorMagnetometer, delay);
						sensorManager.RegisterListener(this, sensorAccelerometer, delay);
					}
					else Network.Sender.WriteLog("Sensor: Tilt not available.");
					break;
			}
			sensorStatus[sensorType] = true;
		}

		public void Stop(InteractClient.Sensors.SensorType sensorType)
		{
			switch (sensorType)
			{
				case InteractClient.Sensors.SensorType.AcceleroMeter:
					if (sensorAccelerometer != null && sensorStatus[InteractClient.Sensors.SensorType.Tilt] == false)
						sensorManager.UnregisterListener(this, sensorAccelerometer);
					break;
				case InteractClient.Sensors.SensorType.Gyroscope:
					if (sensorGyroscope != null)
						sensorManager.UnregisterListener(this, sensorGyroscope);
					break;
				case InteractClient.Sensors.SensorType.MagnetoMeter:
					if (sensorMagnetometer != null && sensorStatus[InteractClient.Sensors.SensorType.Tilt] == false)
						sensorManager.UnregisterListener(this, sensorMagnetometer);
					break;
				case InteractClient.Sensors.SensorType.Compass:
					if (sensorCompass != null)
						sensorManager.UnregisterListener(this, sensorCompass);
					break;
				case InteractClient.Sensors.SensorType.Light:
					if (sensorLight != null)
						sensorManager.UnregisterListener(this, sensorLight);
					break;
				case InteractClient.Sensors.SensorType.Pressure:
					if (sensorPressure != null)
						sensorManager.UnregisterListener(this, sensorPressure);
					break;
				case InteractClient.Sensors.SensorType.Proximity:
					if (sensorProximity != null)
						sensorManager.UnregisterListener(this, sensorProximity);
					break;
				case InteractClient.Sensors.SensorType.LinearAcceleration:
					if (sensorLinearAcceleration != null)
						sensorManager.UnregisterListener(this, sensorLinearAcceleration);
					break;
				case InteractClient.Sensors.SensorType.Rotation:
					if (sensorRotation != null)
						sensorManager.UnregisterListener(this, sensorRotation);
					break;
				case InteractClient.Sensors.SensorType.GameRotation:
					if (sensorGameRotation != null)
						sensorManager.UnregisterListener(this, sensorGameRotation);
					break;
				case InteractClient.Sensors.SensorType.Humidity:
					if (sensorHumidity != null)
						sensorManager.UnregisterListener(this, sensorHumidity);
					break;
				case InteractClient.Sensors.SensorType.AmbientTemperature:
					if (sensorAmbientTemperature != null)
						sensorManager.UnregisterListener(this, sensorAmbientTemperature);
					break;
				case InteractClient.Sensors.SensorType.SignificantMotion:
					if (sensorSignificantMotion != null)
						sensorManager.CancelTriggerSensor(TriggerEventListener, sensorSignificantMotion);
					break;
				case InteractClient.Sensors.SensorType.StepDetector:
					if (sensorStepDetector != null)
						sensorManager.CancelTriggerSensor(TriggerEventListener, sensorStepDetector);
					break;
				case InteractClient.Sensors.SensorType.StepCounter:
					if (sensorStepCounter != null)
						sensorManager.UnregisterListener(this, sensorStepCounter);
					break;
				case InteractClient.Sensors.SensorType.HeartRate:
					if (sensorHeartRate != null)
						sensorManager.UnregisterListener(this, sensorHeartRate);
					break;
				case InteractClient.Sensors.SensorType.Pose:
					if (sensorPose != null)
						sensorManager.UnregisterListener(this, sensorPose);
					break;
				case InteractClient.Sensors.SensorType.Stationary:
					if (sensorStationary != null)
						sensorManager.UnregisterListener(this, sensorStationary);
					break;
				case InteractClient.Sensors.SensorType.Motion:
					if (sensorMotion != null)
						sensorManager.UnregisterListener(this, sensorMotion);
					break;
				case InteractClient.Sensors.SensorType.HeartBeat:
					if (sensorHeartBeat != null)
						sensorManager.UnregisterListener(this, sensorHeartBeat);
					break;
				case InteractClient.Sensors.SensorType.Tilt:
					if (sensorAccelerometer != null && sensorStatus[InteractClient.Sensors.SensorType.AcceleroMeter] == false)
						sensorManager.UnregisterListener(this, sensorAccelerometer);
					if (sensorMagnetometer != null && sensorStatus[InteractClient.Sensors.SensorType.MagnetoMeter] == false)
						sensorManager.UnregisterListener(this, sensorMagnetometer);
					break;
			}
			sensorStatus[sensorType] = false;
		}

		public void StopAll()
		{
			Stop(InteractClient.Sensors.SensorType.AcceleroMeter);
			Stop(InteractClient.Sensors.SensorType.AmbientTemperature);
			Stop(InteractClient.Sensors.SensorType.Compass);
			Stop(InteractClient.Sensors.SensorType.GameRotation);
			Stop(InteractClient.Sensors.SensorType.Gyroscope);
			Stop(InteractClient.Sensors.SensorType.HeartBeat);
			Stop(InteractClient.Sensors.SensorType.HeartRate);
			Stop(InteractClient.Sensors.SensorType.Humidity);
			Stop(InteractClient.Sensors.SensorType.Light);
			Stop(InteractClient.Sensors.SensorType.LinearAcceleration);
			Stop(InteractClient.Sensors.SensorType.MagnetoMeter);
			Stop(InteractClient.Sensors.SensorType.Motion);
			Stop(InteractClient.Sensors.SensorType.Pose);
			Stop(InteractClient.Sensors.SensorType.Pressure);
			Stop(InteractClient.Sensors.SensorType.Proximity);
			Stop(InteractClient.Sensors.SensorType.Rotation);
			Stop(InteractClient.Sensors.SensorType.SignificantMotion);
			Stop(InteractClient.Sensors.SensorType.Stationary);
			Stop(InteractClient.Sensors.SensorType.StepCounter);
			Stop(InteractClient.Sensors.SensorType.StepDetector);
			Stop(InteractClient.Sensors.SensorType.Tilt);
		}
	}
}