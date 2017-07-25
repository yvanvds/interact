namespace Interact
{
  public enum SensorType
  {
    AcceleroMeter,
    MagnetoMeter,
    Gyroscope,
    Compass,
    Light,
    Pressure,
    Proximity,
    LinearAcceleration,
    Rotation,
    GameRotation,
    Humidity,
    AmbientTemperature,
    SignificantMotion,
    StepDetector,
    StepCounter,
    HeartRate,
    Pose,
    Stationary,
    Motion,
    HeartBeat,
    Tilt,
  }

  public enum SensorValueType
  {
    Single,
    Dual,
    Vector,
    Trigger,
  }

  public enum SensorDelay
  {
    Fastest = 0,
    Game = 20,
    Ui = 60,
    Default = 200,
  }
}