using InteractClient.Project;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace InteractClient.Arduino
{
	public class Arduino
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

		delegate void OnOscCallback(int pin, PinState state);
		class Pin
		{
			private ArduinoConfig Config;

			public byte Number => Config.Pin;
			public string Name => Config.Name;

			private float offset = 0;
			public float Offset => offset;
			private float scale = 1;
			public float Scale => scale;

			private bool digital = false;
			public bool Digital => digital;

			public OscTree.Route Route => Config.Route;

			public Pin(ArduinoConfig config, OscTree.Object parent)
			{
				Config = config;
				if (Config.Mode.Equals("Digital Out", StringComparison.CurrentCultureIgnoreCase))
				{
					parent.Endpoints.Add(new OscTree.Endpoint(Config.Name, OnOsc, new Type[] { typeof(bool) }));
					digital = true;
				} else if (Config.Mode.Equals("Digital In", StringComparison.CurrentCultureIgnoreCase))
				{
					digital = true;
				} else if (Config.Mode.Equals("Analog In", StringComparison.CurrentCultureIgnoreCase))
				{
					if(Config.LowValue < Config.HighValue)
					{
						offset = Config.LowValue;
						float range = Config.HighValue - Config.LowValue;
						scale = range / 1024f;
					} else if (Config.HighValue < Config.LowValue)
					{
						offset = Config.LowValue;
						float range = Config.LowValue - Config.HighValue;
						scale = -(range / 1024f);
					}
					digital = false;
				} 
			}

			private OnOscCallback callback = null;
			public void SetOscCallback(OnOscCallback func)
			{
				callback = func;
			}

			public void OnOsc(object[] args)
			{
                PinState state = PinState.HIGH;
                try
                {
                    state = (bool)args[0] == true ? PinState.HIGH : PinState.LOW;
                }
                catch (Exception) { }
				callback?.Invoke(Number, state);
			}
		}

		IArduino device;
		private OscTree.Object OscObject = null;
		private OscTree.Tree OscParent = null;
		private string OscName = string.Empty;

		private List<Pin> Pins = new List<Pin>();
		ArduinoModule config;

		bool started = false;
		public bool Running { get => started; }

		public Arduino(OscTree.Tree parent)
		{
			device = DependencyService.Get<IArduino>();
			OscParent = parent;
		}

		public bool IsImplemented()
		{
			return Device.RuntimePlatform == Device.UWP;
		}

		public async void Start(ArduinoModule config)
		{
			this.config = config;

			if(OscObject != null)
			{
				OscParent.Children.List.Remove(OscName);
			}

			OscObject = new OscTree.Object(new OscTree.Address(config.Name, config.ID), typeof(object));
			OscParent.Add(OscObject);
			OscName = config.ID;

			if (started)
			{
				Reconfigure();
				return;
			}

			string savedInterface = Global.Settings.ArduinoInterface;

			if(savedInterface != null)
			{
				if(savedInterface.Equals("USB") || savedInterface.Equals("Bluetooth") || savedInterface.Equals("DfRobot"))
				{
					string savedDevice = Global.Settings.ArduinoDevice;
					uint savedBaudRate = Global.Settings.ArduinoBaudRate;

					await device.RenewDeviceList(savedInterface);
					device.DeviceReady += OnDeviceReadyEvent;
					device.DeviceConnectionFailed += OnConnectionFailedEvent;
					device.DigitalPinSignal += OnDigitalPinEvent;
					device.AnalogPinSignal += OnAnalogPinEvent;
					device.Connect(savedInterface, savedDevice, savedBaudRate);
					return;
				} 
				else if (savedInterface.Equals("Network"))
				{
					string savedHost = Global.Settings.ArduinoHost;
					ushort savedPort = Global.Settings.ArduinoPort;
					uint savedBaudRate = Global.Settings.ArduinoBaudRate;

					device.DeviceReady += OnDeviceReadyEvent;
					device.DeviceConnectionFailed += OnConnectionFailedEvent;
					device.DigitalPinSignal += OnDigitalPinEvent;
					device.AnalogPinSignal += OnAnalogPinEvent;
					device.Connect(savedHost, savedPort, savedBaudRate);
					return;
				}
			}
			Network.Sender.WriteLog("No Arduino Configured");
		}

		public void Stop()
		{
			if (started)
			{
				if (OscObject != null)
				{
					OscParent.Children.List.Remove(OscName);
				}
				device.DeviceReady -= OnDeviceReadyEvent;
				device.DeviceConnectionFailed -= OnConnectionFailedEvent;
				device.DigitalPinSignal -= OnDigitalPinEvent;
				device.AnalogPinSignal -= OnAnalogPinEvent;
				device.Disconnect();
				started = false;
			}
		}

		private void Reconfigure()
		{
			Pins.Clear();
			OscObject.Endpoints.List.Clear();
			bool analogActive = false;
			bool digitalActive = false;

			foreach (var p in config.Pins)
			{
				var pin = new Pin(p, OscObject);
				
				switch(p.Mode)
				{
					case "Analog In":
						{
							SetAnalogPinMode(p.Pin, PinMode.ANALOG);
							SetStepSize(p.Pin, p.StepSize);
							analogActive = true;
							break;
						}
					case "Digital In":
						{
							SetDigitalPinMode(p.Pin, PinMode.INPUT);
							digitalActive = true;
							break;
						}
					case "Digital Out":
						{
							SetDigitalPinMode(p.Pin, PinMode.OUTPUT);
							SetDigitalPinState(p.Pin, PinState.LOW);
							pin.SetOscCallback(SetDigitalPinState);
							digitalActive = true;
							break;
						}
				}
				
				Pins.Add(pin);
				
			}
			device.EnableAnalogCallback(analogActive);
			device.EnableDigitalCallback(digitalActive);
		}

		private void OnAnalogPinEvent(string pin, ushort value)
		{
			byte id = Convert.ToByte(pin.Substring(1));
			foreach (var current in Pins)
			{
				if (!current.Digital && current.Number == id)
				{
					if(current.Route != null)
					{
						float output = current.Offset + (value * current.Scale);
						
						//Device.BeginInvokeOnMainThread(() =>
						//{
							OscObject.Send(new OscTree.Route(current.Route.GetActualRoute(), OscTree.Route.RouteType.ID), new object[] { output });
						//});
					}
					else {
						//Network.Sender.WriteLog("Arduino Pin " + current.Name + " has no route.");
					}
					break;
				}
			}
		}

		private void OnDigitalPinEvent(byte pin, PinState state)
		{
			foreach(var current in Pins)
			{
				if(current.Digital && current.Number == pin)
				{
					if(current.Route != null)
					{
						//Device.BeginInvokeOnMainThread(() =>
						//{
							OscObject.Send(current.Route, new object[] { state == PinState.LOW ? false : true });
						//});
					}
					
				}
			}
		}

		private void OnConnectionFailedEvent(string message)
		{
			started = false;
			Network.Sender.WriteLog(message);
		}

		private void OnDeviceReadyEvent()
		{
			Reconfigure();
			started = true;
			Network.Sender.WriteLog("Arduino device is ready for use.");
		}

		public PinMode GetDigitalPinMode(int pin)
		{
			return device.GetDigitalPinMode((byte)pin);
		}

		public void SetDigitalPinMode(int pin, PinMode mode)
		{
			device.SetDigitalPinMode((byte)pin, mode);
		}

		public PinState GetDigitalPinState(int pin)
		{
			return device.GetDigitalPinState((byte)pin);
		}

		public void SetDigitalPinState(int pin, PinState state)
		{
			device.SetDigitalPinState((byte)pin, state);
		}

		public PinMode GetAnalogPinMode(int pin)
		{
			return device.GetAnalogPinMode((byte)pin);
		}

		public void SetAnalogPinMode(int pin, PinMode mode)
		{
			device.SetAnalogPinMode((byte)pin, mode);
		}

		// Stepsize sets the minimal deviation from the last sent value before the 
		// output is handled. This only works with analog pins because digital pins only 
		// output 0 and 1.
		public int GetStepSize(int pin)
		{
			return device.GetStepSize(pin);
		}

		public void SetStepSize(int pin, int size)
		{
			device.SetStepSize(pin, size);
		}

	}
}
