using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static InteractClient.Arduino.Arduino;

namespace InteractClient.Arduino
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

		void Connect(string interfaceType, string deviceName, uint baudRate);
		void Connect(string hostname, ushort port, uint baudRate);
		void Disconnect();

		void EnableAnalogCallback(bool value);
		void EnableDigitalCallback(bool value);

		void SetDigitalPinMode(byte pin, PinMode mode);
		PinMode GetDigitalPinMode(byte pin);
		void SetDigitalPinState(byte pin, PinState state);
		PinState GetDigitalPinState(byte pin);

		void SetAnalogPinMode(byte pin, PinMode mode);
		PinMode GetAnalogPinMode(byte pin);

		int GetStepSize(int pin);
		void SetStepSize(int pin, int size);

		event ArduinoReadyEventHandler DeviceReady;
		event ArduinoFailedEventHandler DeviceConnectionFailed;
		event ArduinoDigitalPinEventHandler DigitalPinSignal;
		event ArduinoAnalogPinEventHandler AnalogPinSignal;
	}
}
