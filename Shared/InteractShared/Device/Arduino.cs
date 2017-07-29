using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.Device
{
  public abstract class Arduino
  {
    public enum PinMode
    {
      INPUT = 0,
      OUTPUT = 1,
      ANALOG = 2,
      PWM = 3,
      SERVO = 4,
      SHIFT = 5,
      I2C = 6,
      ONEWIRE = 7,
      STEPPER = 8,
      ENCODER = 9,
      SERIAL = 10,
      PULLUP = 11,
      IGNORED = 127
    }

    public enum PinState
    {
      LOW = 0,
      HIGH = 1
    }

    public abstract bool IsImplemented();
    public abstract void Start();
    public abstract void Stop();

    public abstract PinMode GetDigitalPinMode(int pin);
    public abstract void SetDigitalPinMode(int pin, PinMode mode);
    public abstract PinState GetDigitalPinState(int pin);
    public abstract void SetdigitalPinState(int pin, PinState state);

    public abstract PinMode GetAnalogPinMode(int pin);
    public abstract void SetAnalogPinMode(int pin, PinMode mode);

    public abstract void OnDeviceReady(string functionName);
    public abstract void OnConnectionFailed(string functionName);

    public abstract void SendOSC(string destination, int port, string address);
    public abstract void StopOSC();

    public abstract void OnAnalogPinUpdate(string functionName);
    public abstract void OnDigitalPinUpdate(string functionName);
  }
}
