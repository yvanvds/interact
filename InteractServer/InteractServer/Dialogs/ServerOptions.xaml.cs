using IYse;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InteractServer.Dialogs
{
	/// <summary>
	/// Interaction logic for ServerOptions.xaml
	/// </summary>
	public partial class ServerOptions : MetroWindow
	{
		List<IDevice> devices = new List<IDevice>();
		List<string> hosts = new List<string>();
		List<string> devicesOnHost = new List<string>();
		List<string> outputs = new List<string>();

		string selectedHostName = string.Empty;
		string selectedDeviceName = string.Empty;
		string selectedChannelConf = string.Empty;

		IDevice selectedDevice = null;

		public ServerOptions()
		{
			InitializeComponent();
			ServerNameText.Text = Properties.Settings.Default.ServerName;
			NetworkTokenText.Text = Properties.Settings.Default.NetworkToken;
			OpenProjectSwitch.IsChecked = Properties.Settings.Default.OpenProjectOnStart;

			if(Properties.Settings.Default.AudioHostName != String.Empty)
			{
				selectedHostName = Properties.Settings.Default.AudioHostName;
			} else
			{
				selectedHostName = Yse.Yse.Handle.Interface.System.DefaultHost;
			}
			
			if(Properties.Settings.Default.AudioDeviceName != String.Empty)
			{
				selectedDeviceName = Properties.Settings.Default.AudioDeviceName;
			} else
			{
				selectedDeviceName = Yse.Yse.Handle.Interface.System.DefaultDevice;
			}

			if(Properties.Settings.Default.AudioChannelConf != string.Empty)
			{
				selectedChannelConf = Properties.Settings.Default.AudioChannelConf;
			} else
			{
				selectedChannelConf = "Stereo";
			}
			

			for (uint i = 0; i < Yse.Yse.Handle.Interface.System.NumDevices; i++)
			{
				IDevice device = Yse.Yse.Handle.Interface.System.GetDevice(i);
				if(device.OutputChannels.Count > 0)
				{
					devices.Add(device);
				}
			}

			foreach(var device in devices)
			{
				if(!hosts.Contains(device.TypeName))
				{
					hosts.Add(device.TypeName);
				}
			}

			ComboBoxHost.ItemsSource = hosts;
			ComboBoxHost.SelectedIndex = hosts.IndexOf(selectedHostName);
		}

		private void SetOutputs(IDevice output)
		{
			outputs.Clear();
			if (output == null) return;

			int channels = output.OutputChannels.Count;
			if(channels > 0)
			{
				outputs.Add("Mono");
			}
			if(channels > 1)
			{
				outputs.Add("Stereo");
			}
			if(channels > 3)
			{
				outputs.Add("Quad Surround");
			}
			if(channels > 4)
			{
				outputs.Add("5.1 Surround (Rear)");
				outputs.Add("5.1 Surround (Side)");
			}
			if(channels > 5)
			{
				outputs.Add("6.1 Surround");
			}
			if(channels > 6)
			{
				outputs.Add("7.1 Surround");
			}
		}

		private void ButtonOK_Click(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default.ServerName = ServerNameText.Text;
			Properties.Settings.Default.NetworkToken = NetworkTokenText.Text;
			Properties.Settings.Default.OpenProjectOnStart = OpenProjectSwitch.IsChecked.Value;

			bool audioConfChanged = false;
			if(selectedHostName != Properties.Settings.Default.AudioHostName)
			{
				Properties.Settings.Default.AudioHostName = selectedHostName;
				audioConfChanged = true;
			}
			if(selectedDeviceName != Properties.Settings.Default.AudioDeviceName)
			{
				Properties.Settings.Default.AudioDeviceName = selectedDeviceName;
				audioConfChanged = true;
			}
			if(selectedChannelConf != Properties.Settings.Default.AudioChannelConf)
			{
				Properties.Settings.Default.AudioChannelConf = selectedChannelConf;
				audioConfChanged = true;
			}

			if(audioConfChanged)
			{
				foreach(var device in devices)
				{
					if(device.TypeName.Equals(selectedHostName) && device.Name.Equals(selectedDeviceName) && device.OutputChannels.Count > 0)
					{
						Yse.Yse.Handle.OpenDevice(device, selectedChannelConf);
						break;
					}
				}
			}


			Properties.Settings.Default.Save();
			Close();
		}

		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void ComboBoxHost_SelectionChanged(object sender, RoutedEventArgs e)
		{
			selectedHostName = hosts.ElementAt(ComboBoxHost.SelectedIndex);

			devicesOnHost.Clear();
			selectedDevice = null;
			foreach (var device in devices)
			{
				if (device.TypeName.Equals(selectedHostName) && device.OutputChannels.Count > 0)
				{
					devicesOnHost.Add(device.Name);
					if (device.Name.Equals(selectedDeviceName))
					{
						selectedDevice = device;
					}
				}
			}

			selectedDeviceName = Yse.Yse.Handle.Interface.System.DefaultDevice;
			ComboBoxDevice.ItemsSource = devicesOnHost;
			ComboBoxDevice.SelectedIndex = devicesOnHost.IndexOf(selectedDeviceName);

		}

		private void ComboBoxDevice_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(ComboBoxDevice.SelectedIndex >= 0 && ComboBoxDevice.SelectedIndex < devicesOnHost.Count)
			{
				selectedDeviceName = devicesOnHost.ElementAt(ComboBoxDevice.SelectedIndex);
			} else
			{
				selectedDeviceName = string.Empty;
			}
			

			selectedDevice = null;
			foreach (var device in devices)
			{
				if (device.TypeName.Equals(selectedHostName) && device.OutputChannels.Count > 0)
				{
					if (device.Name.Equals(selectedDeviceName))
					{
						selectedDevice = device;
					}
				}
			}

			SetOutputs(selectedDevice);
			ComboBoxOutput.ItemsSource = outputs;
			int index = outputs.IndexOf(selectedChannelConf);
			if(index != -1)
			{
				ComboBoxOutput.SelectedIndex = index;
			}
			else if(outputs.Count < 2)
			{
				ComboBoxOutput.SelectedIndex = 0;
			} else
			{
				ComboBoxOutput.SelectedIndex = 1;
			}
			if(ComboBoxOutput.SelectedIndex >= 0)
			{
                if(ComboBoxOutput.SelectedIndex < outputs.Count)
                {
                    selectedChannelConf = outputs[ComboBoxOutput.SelectedIndex];
                } else
                {
                    selectedChannelConf = string.Empty;
                }
				
			} else
			{
				selectedChannelConf = string.Empty;
			}
			
		}

		private void ComboBoxOutput_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectedChannelConf = outputs[ComboBoxOutput.SelectedIndex];
		}
	}
}
