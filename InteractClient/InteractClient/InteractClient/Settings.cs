using Acr.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient
{
	public class Settings
	{
		public Settings()
		{

		}

		public string ID
		{
			set => CrossSettings.Current.Set<string>("ID", value);
			get => CrossSettings.Current.Get<string>("ID");
		}

		public string Token
		{
			set => CrossSettings.Current.Set<string>("Token", value);
			get => CrossSettings.Current.Get<string>("Token");
		}

		public string DeviceID
		{
			set => CrossSettings.Current.Set<string>("DeviceID", value);
			get => CrossSettings.Current.Get<string>("DeviceID");
		}

		public string ArduinoInterface
		{
			set => CrossSettings.Current.Set<string>("ArduinoInterface", value);
			get => CrossSettings.Current.Get<string>("ArduinoInterface");
		}

		public string ArduinoDevice
		{
			set => CrossSettings.Current.Set<string>("ArduinoDevice", value);
			get => CrossSettings.Current.Get<string>("ArduinoDevice");
		}

		public uint ArduinoBaudRate
		{
			set => CrossSettings.Current.Set<uint>("ArduinoBaudRate", value);
			get => CrossSettings.Current.Get<uint>("ArduinoBaudRate");
		}

		public string ArduinoHost
		{
			set => CrossSettings.Current.Set<string>("ArduinoHost", value);
			get => CrossSettings.Current.Get<string>("ArduinoHost");
		}

		public ushort ArduinoPort
		{
			set => CrossSettings.Current.Get<ushort>("ArduinoPort", value);
			get => CrossSettings.Current.Get<ushort>("ArduinoPort");
		}

		public void SetProjectInfo(string name, Project.Info info)
		{
			CrossSettings.Current.Set<Project.Info>(name, info);
		}

		public Project.Info GetProjectInfo(string name)
		{
			return CrossSettings.Current.Get<Project.Info>(name);
		}

		public void RemoveProjectInfo(string name)
		{
			CrossSettings.Current.Remove(name);
		}
	}
}
