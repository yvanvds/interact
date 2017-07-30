﻿using InteractClient.Interface;
using InteractClient.UWP.Implementation;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        connection.ConnectionEstablished -= OnDeviceReady;
        connection.ConnectionFailed -= OnConnectionFailed;
        connection.end();
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
        Network.Service.Get().WriteLog("Arduino: Pin " + (int)pin + " is disabled.");
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
        Network.Service.Get().WriteLog("Arduino: Pin " + (int)pin + " is disabled.");
        return Convert(PinState.LOW);
      }

      if(arduino.getPinMode(pin) != PinMode.OUTPUT)
      {
        Network.Service.Get().WriteLog("Arduino: Pin " + (int)pin + " is not in output mode.");
        return Convert(PinState.LOW);
      }

      return Convert(arduino.digitalRead(pin));
    }

    public void SetDigitalPinState(byte pin, Interact.Device.Arduino.PinState state)
    {
      if (disabledPins.Contains(pin))
      {
        Network.Service.Get().WriteLog("Arduino: Pin " + (int)pin + " is disabled.");
        return;
      }

      if (arduino.getPinMode(pin) != PinMode.OUTPUT)
      {
        Network.Service.Get().WriteLog("Arduino: Pin " + (int)pin + " is not in output mode.");
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
        InteractClient.Network.Service.Get().WriteLog("Arduino Error: No Hardware Profile found.");
        return;
      }

      analogOffset = hardware.AnalogOffset;
      disabledPins = hardware.DisabledPins;
      analogPins = hardware.AnalogPins;
      digitalPins = hardware.DigitalPins;
      pwmPins = hardware.PwmPins;
      i2cPins = hardware.I2cPins;

      Network.Service.Get().WriteLog("Arduino Configuration: ");
      Network.Service.Get().WriteLog("  Digital  Pins: " + digitalPins.Count);
      Network.Service.Get().WriteLog("  Analog   Pins: " + analogPins.Count);
      Network.Service.Get().WriteLog("  Disabled Pins: " + disabledPins.Count);
      Network.Service.Get().WriteLog("  PWM      Pins: " + pwmPins.Count);
      Network.Service.Get().WriteLog("  i2c      Pins: " + i2cPins.Count);
      Network.Service.Get().WriteLog("  Analog Pin Offset: " + analogOffset);

      //ListPinConfiguration();
    }

    public void ListPinConfiguration()
    {
      foreach(var pin in digitalPins)
      {
        Network.Service.Get().WriteLog("Digital: " + pin);
      }

      foreach (var pin in analogPins)
      {
        Network.Service.Get().WriteLog("Analog: " + pin);
      }

      foreach (var pin in pwmPins)
      {
        Network.Service.Get().WriteLog("PWM: " + pin);
      }

      foreach (var pin in i2cPins)
      {
        Network.Service.Get().WriteLog("i2c: " + pin);
      }

      foreach (var pin in disabledPins)
      {
        Network.Service.Get().WriteLog("Disabled: " + pin);
      }
    }

    private void OnDigitalPinUpdated(byte pin, PinState state)
    {
      DigitalPinSignal(pin, Convert(state));
    }

    private void OnAnalogPinUpdated(string pin, ushort value)
    {
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
  }
}