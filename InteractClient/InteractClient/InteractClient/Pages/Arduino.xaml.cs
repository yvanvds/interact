using Acr.Settings;
using InteractClient.Arduino;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InteractClient.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Arduino : ContentPage
	{
		CancellationTokenSource cancelTokenSource;
		IArduino arduino;
		bool timerShouldRun = false;

		public Arduino ()
		{
			InitializeComponent ();
			arduino = DependencyService.Get<IArduino>();
			ConnectionMethodPicker.SelectedIndexChanged += ConnectionPickerSelectionChanged;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			Reset();

			if (DevicePicker.ItemsSource == null)
			{
				ConnectMessage.Text = "Select an item to connect to.";
				Task task = RefreshDeviceList();
				task.ContinueWith(result => {

					Device.BeginInvokeOnMainThread(() =>
					{
						string savedInterface = CrossSettings.Current.Get<string>("ArduinoInterface");

						if (savedInterface != null && savedInterface.Length != 0)
						{
							for (int i = 0; i < ConnectionMethodPicker.Items.Count; i++)
							{
								if (ConnectionMethodPicker.Items[i].Equals(savedInterface))
								{
									ConnectionMethodPicker.SelectedIndex = i;
									break;
								}
							}

							if (savedInterface.Equals("USB") || savedInterface.Equals("Bluetooth") || savedInterface.Equals("DfRobot"))
							{
								string savedDevice = CrossSettings.Current.Get<string>("ArduinoDevice");
								for (int i = 0; i < DevicePicker.Items.Count; i++)
								{
									if (DevicePicker.Items[i].Equals(savedDevice))
									{
										DevicePicker.SelectedIndex = i;
										break;
									}
								}

								uint savedBaudRate = CrossSettings.Current.Get<uint>("ArduinoBaudRate");
								for (int i = 0; i < BaudRatePicker.Items.Count; i++)
								{
									uint br = Convert.ToUInt32(BaudRatePicker.Items[i]);
									if (br == savedBaudRate)
									{
										BaudRatePicker.SelectedIndex = i;
										break;
									}
								}

							}

							if (savedInterface.Equals("Network"))
							{
								string savedHost = CrossSettings.Current.Get<string>("ArduinoHost");
								NetworkHostNameEntry.Text = savedHost;
								string savedPort = Convert.ToString(CrossSettings.Current.Get<ushort>("ArduinoPort"));
								NetworkPortEntry.Text = savedPort;

								uint savedBaudRate = CrossSettings.Current.Get<uint>("ArduinoBaudRate");
								for (int i = 0; i < BaudRatePicker.Items.Count; i++)
								{
									uint br = Convert.ToUInt32(BaudRatePicker.Items[i]);
									if (br == savedBaudRate)
									{
										BaudRatePicker.SelectedIndex = i;
										break;
									}
								}
							}
						}
					});



				});

			}
		}

		private async Task RefreshDeviceList()
		{

			if (ConnectionMethodPicker.SelectedItem == null)
			{
				ConnectMessage.Text = "Select a connection method to continue.";
				return;
			}

			DevicePicker.ItemsSource = null;

			ObservableCollection<string> result = null;

			switch (ConnectionMethodPicker.SelectedItem as string)
			{
				default:
				case "Bluetooth":
					DevicePicker.IsVisible = true;
					DevicesText.IsVisible = true;
					NetworkHostNameEntry.IsEnabled = false;
					NetworkPortEntry.IsEnabled = false;
					BaudRatePicker.IsEnabled = true;
					NetworkHostNameEntry.Text = "";
					NetworkPortEntry.Text = "";

					cancelTokenSource = new CancellationTokenSource();
					cancelTokenSource.Token.Register(() => OnConnectionCancelled());

					result = await arduino.GetDeviceList("Bluetooth", cancelTokenSource.Token);
					if (result.Count > 0)
					{
						DevicePicker.ItemsSource = result;
					}
					else
					{
						ConnectMessage.Text = "No items found.";
						DevicePicker.IsVisible = false;
					}
					break;

				case "USB":
					DevicePicker.IsVisible = true;
					DevicesText.IsVisible = true;
					NetworkHostNameEntry.IsEnabled = false;
					NetworkPortEntry.IsEnabled = false;
					BaudRatePicker.IsEnabled = true;
					NetworkHostNameEntry.Text = "";
					NetworkPortEntry.Text = "";

					cancelTokenSource = new CancellationTokenSource();
					cancelTokenSource.Token.Register(() => OnConnectionCancelled());

					result = await arduino.GetDeviceList("USB", cancelTokenSource.Token);
					if (result.Count > 0)
					{
						DevicePicker.ItemsSource = result;
					}
					else
					{
						ConnectMessage.Text = "No items found.";
						DevicePicker.IsVisible = false;
					}

					break;

				case "DfRobot":
					DevicePicker.IsVisible = true;
					DevicesText.IsVisible = true;
					NetworkHostNameEntry.IsEnabled = false;
					NetworkPortEntry.IsEnabled = false;
					BaudRatePicker.IsEnabled = true;
					NetworkHostNameEntry.Text = "";
					NetworkPortEntry.Text = "";

					cancelTokenSource = new CancellationTokenSource();
					cancelTokenSource.Token.Register(() => OnConnectionCancelled());

					result = await arduino.GetDeviceList("DfRobot", cancelTokenSource.Token);
					if (result.Count > 0)
					{
						DevicePicker.ItemsSource = result;
					}
					else
					{
						ConnectMessage.Text = "No items found.";
						DevicePicker.IsVisible = false;
					}

					break;

				case "Network":
					DevicePicker.IsVisible = false;
					DevicesText.IsVisible = false;
					NetworkHostNameEntry.IsEnabled = true;
					NetworkPortEntry.IsEnabled = true;
					BaudRatePicker.IsEnabled = false;
					ConnectMessage.Text = "Enter a host and port to connect.";
					break;
			}
		}

		private async void ConnectionPickerSelectionChanged(object sender, EventArgs e)
		{
			await RefreshDeviceList();
		}

		private async Task RefreshButton_Clicked(object sender, EventArgs e)
		{
			await RefreshDeviceList();
		}

		private void CancelButton_Clicked(object sender, EventArgs e)
		{
			OnConnectionCancelled();
		}

		private void ConnectButton_Clicked(object sender, EventArgs e)
		{
			SetUIEnabled(false);

			string device = "";
			if (DevicePicker.SelectedItem != null)
			{
				device = DevicePicker.SelectedItem as string;
			}
			else if ((ConnectionMethodPicker.SelectedItem as string) != "Network")
			{
				ConnectMessage.Text = "You must select an item to proceed.";
				SetUIEnabled(true);
				return;
			}

			uint baudRate = Convert.ToUInt32((BaudRatePicker.SelectedItem as string));
			arduino.DeviceReady += OnDeviceReady;
			arduino.DeviceConnectionFailed += OnConnectionFailed;

			switch (ConnectionMethodPicker.SelectedItem as string)
			{
				default:
				case "Bluetooth":
					arduino.Connect("Bluetooth", device, baudRate);
					break;

				case "USB":
					arduino.Connect("USB", device, baudRate);
					break;

				case "DfRobot":
					arduino.Connect("DfRobot", device, baudRate);
					break;

				case "Network":
					string host = NetworkHostNameEntry.Text;
					string port = NetworkPortEntry.Text;
					ushort portnum = 0;

					if (Uri.CheckHostName(host) == UriHostNameType.Unknown)
					{
						ConnectMessage.Text = "You have entered an invalid host or IP.";
						return;
					}

					if (!ushort.TryParse(port, out portnum))
					{
						ConnectMessage.Text = "You have entered an invalid port number.";
						return;
					}

					arduino.Connect(host, portnum, baudRate);
					break;
			}

			timerShouldRun = true;
			Device.StartTimer(TimeSpan.FromSeconds(30), () =>
			{
				if (timerShouldRun) Connection_TimeOut();
				return false;
			});
		}

		private void OnConnectionFailed(string message)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				timerShouldRun = false;
				ConnectMessage.Text = "Connection attempt failed: " + message;
				Reset();
			});
		}

		private void OnDeviceReady()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				timerShouldRun = false;
				ConnectMessage.Text = "Successfully connected!";
			});

			// Store this connection
			if (ConnectionMethodPicker.SelectedItem != null)
			{
				CrossSettings.Current.Set<string>("ArduinoInterface", ConnectionMethodPicker.SelectedItem as string);
			}
			if (DevicePicker.SelectedItem != null)
			{
				CrossSettings.Current.Set<string>("ArduinoDevice", DevicePicker.SelectedItem as string);
			}
			if (BaudRatePicker.SelectedItem != null)
			{
				CrossSettings.Current.Set<uint>("ArduinoBaudRate", Convert.ToUInt32((BaudRatePicker.SelectedItem as string)));
			}

			if (CrossSettings.Current.Get<string>("ArduinoInterface").Equals("Network"))
			{
				string host = NetworkHostNameEntry.Text;
				string port = NetworkPortEntry.Text;
				ushort portnum = 0;
				if (ushort.TryParse(port, out portnum))
				{
					CrossSettings.Current.Set<string>("ArduinoHost", host);
					CrossSettings.Current.Set<ushort>("ArduinoPort", portnum);
				}
			}
		}

		private void Connection_TimeOut()
		{
			if (timerShouldRun)
				Device.BeginInvokeOnMainThread(() =>
				{
					timerShouldRun = false;
					ConnectMessage.Text = "Connection attempt timed out.";
					Reset();
				});
		}

		private void OnConnectionCancelled()
		{
			timerShouldRun = false;
			ConnectMessage.Text = "Connection attempt cancelled.";
			Reset();
		}

		private void SetUIEnabled(bool enabled)
		{
			RefreshButton.IsEnabled = enabled;
			ConnectButton.IsEnabled = enabled;
			CancelButton.IsEnabled = !enabled;
		}

		private void Reset()
		{
			if (arduino != null)
			{
				arduino.Disconnect();
			}

			if (cancelTokenSource != null)
			{
				cancelTokenSource.Dispose();
			}

			cancelTokenSource = null;

			SetUIEnabled(true);
		}


		private void BackButton_Clicked(object sender, EventArgs e)
		{
			OnConnectionCancelled();
			Navigation.PopAsync();
		}

		private async Task ClearButton_Clicked(object sender, EventArgs e)
		{
			CrossSettings.Current.Set<string>("ArduinoInterface", "");
			CrossSettings.Current.Set<string>("ArduinoDevice", "");
			CrossSettings.Current.Set<uint>("ArduinoBaudRate", 0);
			CrossSettings.Current.Set<string>("ArduinoHost", "");
			CrossSettings.Current.Set<ushort>("ArduinoPort", 0);

			await RefreshDeviceList();
		}
	}
}