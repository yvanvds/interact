using IYse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace InteractServer.Yse
{
	public class Yse
	{
		public static Yse Handle = null;

		public YSE.YseInterface Interface;

		private DispatcherTimer update = new DispatcherTimer();

		public Yse()
		{
			Interface = new YSE.YseInterface(AddLogMessage);
			Interface.System.Init();
			Interface.Log.Level = IYse.ERROR_LEVEL.ERROR;
			Handle = this;

			Log.Log.Handle.AddEntry("Yse Version: " + Interface.System.Version);

			string hostName = Properties.Settings.Default.AudioHostName;
			string deviceName = Properties.Settings.Default.AudioDeviceName;
			string channelConf = Properties.Settings.Default.AudioChannelConf;

			if(hostName != string.Empty && deviceName != string.Empty && channelConf != string.Empty)
			{
				bool success = false;
				for (uint i = 0; i < Interface.System.NumDevices; i++)
				{
					IDevice device = Interface.System.GetDevice(i);
					if (device.OutputChannels.Count > 0 && device.Name.Equals(deviceName) && device.TypeName.Equals(hostName))
					{
						success = true;
						OpenDevice(device, channelConf);
						break;
					}
				}
				if(!success)
				{
					Dialogs.Error.Show("Invalid Audio Configuration", "The current audio configuration is invalid. Please open Application Options, Audio.");
				}
			}

			update.Interval = new TimeSpan(0, 0, 0, 0, 50);
			update.Tick += new EventHandler(UpdateFunc);
			update.Start();
		}

		public void UpdateFunc(object sender, EventArgs e)
		{
			Interface.System.Update();
		}

		public void AddLogMessage(string message)
		{
			Log.Log.Handle.AddEntry("Yse: " + message);
		}

		public void OpenDevice(IDevice device, string channelconf)
		{
			ChannelType channelType = new ChannelType();
			switch(channelconf)
			{
				case "Mono": channelType = ChannelType.Mono; break;
				case "Stereo": channelType = ChannelType.Stereo; break;
				case "Quad Surround": channelType = ChannelType.Quad; break;
				case "5.1 Surround (Rear)": channelType = ChannelType.Surround51; break;
				case "5.1 Surround (Side)": channelType = ChannelType.Surround51Side; break;
				case "6.1 Surround": channelType = ChannelType.Surround61; break;
				case "7.1 Surround": channelType = ChannelType.Surround71; break;
			}

			Interface.System.CloseCurrentDevice();
			IDeviceSetup setup = Interface.NewDeviceSetup();
			setup.SetOutput(device);
			Interface.System.OpenDevice(setup, channelType);
			Log.Log.Handle.AddEntry("Yse Output set to " + device.TypeName + ": " + device.Name + " with " + channelconf);
		}

		public void Dispose()
		{
			update.Stop();
			Interface.System.Close();
		}
	}
}
