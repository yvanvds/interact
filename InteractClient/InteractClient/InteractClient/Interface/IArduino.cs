using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Interact.Device.Arduino;

namespace InteractClient.Interface
{
  public delegate void ArduinoReadyEventHandler();
  public delegate void ArduinoFailedEventHandler(string message);
  public delegate void ArduinoDigitalPinEventHandler(byte pin, PinState state);
  public delegate void ArduinoAnalogPinEventHandler(string pin, ushort value);

  public interface IArduino
  {
    Task<ObservableCollection<string>> GetDeviceList(string interfaceType, CancellationToken token);
    Task RenewDeviceList(string interfaceType);
    void ListPinConfiguration();

    void Connect(string interfaceType, string deviceName);
    void Connect(string hostname, ushort port);
    void Begin(uint baudRate);

    void SetDigitalPinMode(byte pin, PinMode mode);
    PinMode GetDigitalPinMode(byte pin);
    void SetDigitalPinState(byte pin, PinState state);
    PinState GetDigitalPinState(byte pin);

    void SetAnalogPinMode(byte pin, PinMode mode);
    PinMode GetAnalogPinMode(byte pin);

    event ArduinoReadyEventHandler DeviceReady;
    event ArduinoFailedEventHandler DeviceConnectionFailed;
    event ArduinoDigitalPinEventHandler DigitalPinSignal;
    event ArduinoAnalogPinEventHandler AnalogPinSignal;

    void Reset();
  }
}
