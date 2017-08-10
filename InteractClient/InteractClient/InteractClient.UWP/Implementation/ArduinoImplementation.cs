using InteractClient.Interface;
using InteractClient.UWP.Implementation;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Xamarin.Forms;

[assembly: Dependency(typeof(ArduinoImplementation))]
namespace InteractClient.UWP.Implementation
{
  public class ArduinoImplementation : IArduino
  {
    private IStream connection;
    private RemoteDevice arduino;

    private int analogOffset = 0;
    private IList<byte> disabledPins;
    private IList<byte> analogPins;
    private IList<byte> digitalPins;
    private IList<byte> pwmPins;
    private IList<byte> i2cPins;
    private bool isI2cEnabled = false;

    private bool AnalogCallbackEnabled = false;
    private bool DigitalCallbackEnabled = false;

    private Dictionary<string, DeviceInformation> connections = new Dictionary<string, DeviceInformation>();

    // both are needed to keep track of steps
    private Dictionary<string, int> stepSize = new Dictionary<string, int>();
    private Dictionary<string, int> lastValue = new Dictionary<string, int>();

    public event ArduinoReadyEventHandler DeviceReady;
    public event ArduinoFailedEventHandler DeviceConnectionFailed;
    public event ArduinoDigitalPinEventHandler DigitalPinSignal;
    public event ArduinoAnalogPinEventHandler AnalogPinSignal;

    public async Task<ObservableCollection<string>> GetDeviceList(string interfaceType, CancellationToken token)
    {
      ObservableCollection<string> output = new ObservableCollection<string>();
      connections.Clear();

      Task<DeviceInformationCollection> task = null;
      switch(interfaceType)
      {
        case "Bluetooth":
          task = BluetoothSerial.listAvailableDevicesAsync().AsTask<DeviceInformationCollection>(token);
          break;

        case "USB":
          task = UsbSerial.listAvailableDevicesAsync().AsTask<DeviceInformationCollection>(token);
          break;
        case "DfRobot":
          task = DfRobotBleSerial.listAvailableDevicesAsync().AsTask<DeviceInformationCollection>(token);
          break;
      }

      if (task != null)
      {
        await task.ContinueWith(listTask =>
        {
          var result = listTask.Result;
          foreach(DeviceInformation device in result)
          {
            output.Add(device.Name);
            connections.Add(device.Name, device);
          }
        });
      }

      return output;
    }

    public async Task RenewDeviceList(string interfaceType)
    {
      connections.Clear();

      Task<DeviceInformationCollection> task = null;
      switch (interfaceType)
      {
        case "Bluetooth":
          task = BluetoothSerial.listAvailableDevicesAsync().AsTask<DeviceInformationCollection>();
          break;

        case "USB":
          task = UsbSerial.listAvailableDevicesAsync().AsTask<DeviceInformationCollection>();
          break;
        case "DfRobot":
          task = DfRobotBleSerial.listAvailableDevicesAsync().AsTask<DeviceInformationCollection>();
          break;
      }

      if (task != null)
      {
        await task.ContinueWith(listTask =>
        {
          var result = listTask.Result;
          foreach (DeviceInformation device in result)
          {
            connections.Add(device.Name, device);
          }
        });
      }
    }

    public void Connect(string interfaceType, string deviceName, uint baudRate)
    {
      Disconnect();
      if(!connections.ContainsKey(deviceName)) return;

      switch(interfaceType)
      {
        case "Bluetooth":
          connection = new BluetoothSerial(connections[deviceName]);
          break;

        case "USB":
          connection = new UsbSerial(connections[deviceName]);
          break;

        case "DfRobot":
          connection = new DfRobotBleSerial(connections[deviceName]);
          break;
        
      }

      arduino = new RemoteDevice(connection);
      arduino.DeviceReady += OnDeviceReady;
      arduino.DeviceConnectionFailed += OnConnectionFailed;
      connection.begin(baudRate, SerialConfig.SERIAL_8N1);
    }

    public void Connect(string hostname, ushort port, uint baudRate)
    {
      Disconnect();
      connection = new NetworkSerial(new Windows.Networking.HostName(hostname), port);
      arduino = new RemoteDevice(connection);
      arduino.DeviceReady += OnDeviceReady;
      arduino.DeviceConnectionFailed += OnConnectionFailed;
      connection.begin(baudRate, SerialConfig.SERIAL_8N1);
    }

    public void Disconnect()
    {
      if(connection != null)
      {
        try
        {
          connection.ConnectionEstablished -= OnDeviceReady;
          connection.ConnectionFailed -= OnConnectionFailed;
          connection.end();
        } catch(SEHException)
        {
          
        }
        
      }

      if(arduino != null)
      {
        EnableAnalogCallback(false);
        EnableDigitalCallback(false);
        arduino.Dispose();
      }

      connection = null;
      arduino = null;
    }

    public void EnableAnalogCallback(bool value)
    {
      if(value == true)
      {
        if (!AnalogCallbackEnabled)
        {
          arduino.AnalogPinUpdated += OnAnalogPinUpdated;
          AnalogCallbackEnabled = true;
        }
      } else
      {
        if(AnalogCallbackEnabled)
        {
          arduino.AnalogPinUpdated -= OnAnalogPinUpdated;
          AnalogCallbackEnabled = false;
        }
      }
    }

    public void EnableDigitalCallback(bool value)
    {
      if (value == true)
      {
        if (!DigitalCallbackEnabled)
        {
          arduino.DigitalPinUpdated += OnDigitalPinUpdated;
          DigitalCallbackEnabled = true;
        }
      }
      else
      {
        if (DigitalCallbackEnabled)
        {
          arduino.DigitalPinUpdated -= OnDigitalPinUpdated;
          DigitalCallbackEnabled = false;
        }
      }
    }

    public void SetDigitalPinMode(byte pin, Interact.Device.Arduino.PinMode mode)
    {
      if(disabledPins.Contains(pin))
      {
        Network.Signaler.Get().WriteLog("Arduino: Pin " + (int)pin + " is disabled.");
        return;
      }

      PinMode pm = Convert(mode);
      arduino.pinMode(pin, pm);
    }

    public Interact.Device.Arduino.PinMode GetDigitalPinMode(byte pin)
    {
      return Convert(arduino.getPinMode(pin));
    }

    public Interact.Device.Arduino.PinState GetDigitalPinState(byte pin)
    {
      if (disabledPins.Contains(pin))
      {
        Network.Signaler.Get().WriteLog("Arduino: Pin " + (int)pin + " is disabled.");
        return Convert(PinState.LOW);
      }

      if(arduino.getPinMode(pin) != PinMode.OUTPUT)
      {
        Network.Signaler.Get().WriteLog("Arduino: Pin " + (int)pin + " is not in output mode.");
        return Convert(PinState.LOW);
      }

      return Convert(arduino.digitalRead(pin));
    }

    public void SetDigitalPinState(byte pin, Interact.Device.Arduino.PinState state)
    {
      if (disabledPins.Contains(pin))
      {
        Network.Signaler.Get().WriteLog("Arduino: Pin " + (int)pin + " is disabled.");
        return;
      }

      if (arduino.getPinMode(pin) != PinMode.OUTPUT)
      {
        Network.Signaler.Get().WriteLog("Arduino: Pin " + (int)pin + " is not in output mode.");
        return;
      }

      PinState ps = Convert(state);
      arduino.digitalWrite(pin, ps);
    }

    public void SetAnalogPinMode(byte pin, Interact.Device.Arduino.PinMode mode)
    {
      arduino.pinMode("A" + (pin), Convert(mode));
    }

    public Interact.Device.Arduino.PinMode GetAnalogPinMode(byte pin)
    {
      return Convert(arduino.getPinMode("A" + (pin)));
    }

    public void SetPwmPinMode(byte pin, Interact.Device.Arduino.PinMode mode)
    {
      arduino.pinMode(pin, Convert(mode));
    }

    public Interact.Device.Arduino.PinMode GetPwmPinMode(byte pin)
    {
      return Convert(arduino.getPinMode(pin));
    }

    private void OnConnectionFailed(string message)
    {
      DeviceConnectionFailed(message);
    }

    private void OnDeviceReady()
    {
      DeviceReady();
      RetrieveDeviceConfiguration();

      // reset pins
      foreach (var pin in digitalPins)
      {
        SetDigitalPinMode(pin, Interact.Device.Arduino.PinMode.OUTPUT);
      }

      foreach (var pin in analogPins)
      {
        SetAnalogPinMode(pin, Interact.Device.Arduino.PinMode.OUTPUT);
      }
    }

    private void RetrieveDeviceConfiguration()
    {
      HardwareProfile hardware = arduino.DeviceHardwareProfile;
      if(hardware == null)
      {
        InteractClient.Network.Signaler.Get().WriteLog("Arduino Error: No Hardware Profile found.");
        return;
      }

      analogOffset = hardware.AnalogOffset;
      disabledPins = hardware.DisabledPins;
      analogPins = hardware.AnalogPins;
      digitalPins = hardware.DigitalPins;
      pwmPins = hardware.PwmPins;
      i2cPins = hardware.I2cPins;

      Network.Signaler.Get().WriteLog("Arduino Configuration: ");
      Network.Signaler.Get().WriteLog("  Digital  Pins: " + digitalPins.Count);
      Network.Signaler.Get().WriteLog("  Analog   Pins: " + analogPins.Count);
      Network.Signaler.Get().WriteLog("  Disabled Pins: " + disabledPins.Count);
      Network.Signaler.Get().WriteLog("  PWM      Pins: " + pwmPins.Count);
      Network.Signaler.Get().WriteLog("  i2c      Pins: " + i2cPins.Count);
      Network.Signaler.Get().WriteLog("  Analog Pin Offset: " + analogOffset);

      //ListPinConfiguration();
    }

    public void ListPinConfiguration()
    {
      foreach(var pin in digitalPins)
      {
        Network.Signaler.Get().WriteLog("Digital: " + pin);
      }

      foreach (var pin in analogPins)
      {
        Network.Signaler.Get().WriteLog("Analog: " + pin);
      }

      foreach (var pin in pwmPins)
      {
        Network.Signaler.Get().WriteLog("PWM: " + pin);
      }

      foreach (var pin in i2cPins)
      {
        Network.Signaler.Get().WriteLog("i2c: " + pin);
      }

      foreach (var pin in disabledPins)
      {
        Network.Signaler.Get().WriteLog("Disabled: " + pin);
      }
    }

    private void OnDigitalPinUpdated(byte pin, PinState state)
    {
      DigitalPinSignal(pin, Convert(state));
    }

    private void OnAnalogPinUpdated(string pin, ushort value)
    {
      if(stepSize.ContainsKey(pin))
      {
        if (Math.Abs(value - lastValue[pin]) < stepSize[pin]) return;
        else lastValue[pin] = value;
      }
      AnalogPinSignal(pin, value);
    }

    private PinMode Convert(Interact.Device.Arduino.PinMode mode)
    {
      return (PinMode)mode;
    }

    private PinState Convert(Interact.Device.Arduino.PinState state)
    {
      return (PinState)state;
    }

    private Interact.Device.Arduino.PinMode Convert(PinMode mode)
    {
      return (Interact.Device.Arduino.PinMode)mode;
    }

    private Interact.Device.Arduino.PinState Convert(PinState state)
    {
      return (Interact.Device.Arduino.PinState)state;
    }

    public int GetStepSize(int pin)
    {
      if(stepSize.ContainsKey("A" + pin))
      {
        return stepSize["A" + pin];
      }

      return 0;
    }

    public void SetStepSize(int pin, int size)
    {
      stepSize["A" + pin] = size;
      if(!lastValue.ContainsKey("A" + pin))
      {
        lastValue["A + pin"] = 0;
      }
    }
  }
}
