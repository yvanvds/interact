
using InteractClient.Interface;
using Xamarin.Forms;
using Acr.Settings;

namespace InteractClient.Implementation.Device
{
  public class Arduino : Interact.Device.Arduino
  {
    IArduino device;

    string OnDeviceReadyHandler;
    string OnFailedConnectionHandler;
    string OnAnalogPinUpdateHandler;
    string OnDigitalPinUpdateHandler;

    private Network.OscSender OscSender = null;
    private string OscAddress;

    bool allowJintOutput = false;
    bool started = false;

    public Arduino()
    {
      device = DependencyService.Get<IArduino>();
    }

    public override bool IsImplemented()
    {
      switch(Xamarin.Forms.Device.RuntimePlatform)
      {
        case Xamarin.Forms.Device.Windows:
        case Xamarin.Forms.Device.WinPhone:
          return true;
        default:
          return false;
      }
    }

    public override async void Start()
    {
      if (started) return;

      string savedInterface = Settings.Current.Get<string>("ArduinoInterface");

      if (savedInterface != null)
      {
        if (savedInterface.Equals("USB") || savedInterface.Equals("Bluetooth") || savedInterface.Equals("DfRobot"))
        {
          string savedDevice = Settings.Current.Get<string>("ArduinoDevice");
          uint savedBaudRate = Settings.Current.Get<uint>("ArduinoBaudRate");

          await device.RenewDeviceList(savedInterface);

          device.DeviceReady += OnDeviceReadyEvent;
          device.DeviceConnectionFailed += OnConnectionFailedEvent;
          device.DigitalPinSignal += OnDigitalPinEvent;
          device.AnalogPinSignal += OnAnalogPinEvent;
          device.Connect(savedInterface, savedDevice, savedBaudRate);
          started = true;
          return;

        }
        else if (savedInterface.Equals("Network"))
        {
          string savedHost = Settings.Current.Get<string>("ArduinoHost");
          ushort savedPort = Settings.Current.Get<ushort>("ArduinoPort");
          uint savedBaudRate = Settings.Current.Get<uint>("ArduinoBaudRate");

          device.DeviceReady += OnDeviceReadyEvent;
          device.DeviceConnectionFailed += OnConnectionFailedEvent;
          device.DigitalPinSignal += OnDigitalPinEvent;
          device.AnalogPinSignal += OnAnalogPinEvent;
          device.Connect(savedHost, savedPort, savedBaudRate);
          started = true;
          return;
        }
      }

      InteractClient.Network.Service.Get().WriteLog("No Arduino Configured");
    }

    public void AllowJintOutput(bool value)
    {
      // This method must be called when switching screens to make sure
      // no method is invoked at that moment
      allowJintOutput = value;
    }

    public void RemoveSignalHandlers()
    {
      OnDeviceReadyHandler = null;
      OnFailedConnectionHandler = null;
      OnAnalogPinUpdateHandler = null;
      OnDigitalPinUpdateHandler = null;
    }

    private void OnAnalogPinEvent(string pin, ushort value)
    {
      OscSender?.Send(OscAddress + "/analog/" + pin, (int)value);
      if (!allowJintOutput) return;

      if (OnAnalogPinUpdateHandler != null)
      {
        JintEngine.Engine.Instance.Invoke(OnAnalogPinUpdateHandler, pin, value);
      }
    }

    private void OnDigitalPinEvent(byte pin, PinState state)
    {
      OscSender?.Send(OscAddress + "/digital/" + pin, (int)state);
      if (!allowJintOutput) return;

      if (OnDigitalPinUpdateHandler != null)
      {
        JintEngine.Engine.Instance.Invoke(OnDigitalPinUpdateHandler, pin, (int)state);
      }
    }

    private void OnConnectionFailedEvent(string message)
    {
      if (!allowJintOutput) return;
      if(OnFailedConnectionHandler != null)
      {
        JintEngine.Engine.Instance.Invoke(OnFailedConnectionHandler, message);
      }
    }

    private void OnDeviceReadyEvent()
    {
      if (!allowJintOutput) return;
      if(OnDeviceReadyHandler != null)
      {
        JintEngine.Engine.Instance.Invoke(OnDeviceReadyHandler);
      }
    }

    public override void Stop()
    {
      AllowJintOutput(false);
      RemoveSignalHandlers();
      device.Disconnect();
      started = false;
    }

    public override PinMode GetAnalogPinMode(int pin)
    {
      return device.GetAnalogPinMode((byte)pin);
    }

    public override void SetAnalogPinMode(int pin, PinMode mode)
    {
      device.SetAnalogPinMode((byte)pin, mode);
    }

    public override PinMode GetDigitalPinMode(int pin)
    {
      return device.GetDigitalPinMode((byte)pin);
    }

    public override void SetDigitalPinMode(int pin, PinMode mode)
    {
      device.SetDigitalPinMode((byte)pin, mode);
    }

    public override PinState GetDigitalPinState(int pin)
    {
      return device.GetDigitalPinState((byte)pin);
    }

    public override void SetdigitalPinState(int pin, PinState state)
    {
      device.SetDigitalPinState((byte)pin, state);
    }

    public override void OnDeviceReady(string functionName)
    {
      OnDeviceReadyHandler = functionName;
    }

    public override void OnConnectionFailed(string functionName)
    {
      OnFailedConnectionHandler = functionName;
    }

    public override void OnAnalogPinUpdate(string functionName)
    {
      device.EnableAnalogCallback(true);
      OnAnalogPinUpdateHandler = functionName;
    }

    public override void OnDigitalPinUpdate(string functionName)
    {
      device.EnableDigitalCallback(true);
      OnDigitalPinUpdateHandler = functionName;
    }

    public override void SendOSC(string destination, int port, string address)
    {
      device.EnableAnalogCallback(true);
      device.EnableDigitalCallback(true);

      OscSender = new Network.OscSender();
      OscSender.Init(destination, port);
      OscAddress = address;
    }

    public override void StopOSC()
    {
      OscSender = null;
    }
  }
}
