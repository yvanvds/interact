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
		public ServerOptions()
		{
			InitializeComponent();
			ServerNameText.Text = Properties.Settings.Default.ServerName;
			NetworkTokenText.Text = Properties.Settings.Default.NetworkToken;
			OpenProjectSwitch.IsChecked = Properties.Settings.Default.OpenProjectOnStart;
		}

		private void ButtonOK_Click(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default.ServerName = ServerNameText.Text;
			Properties.Settings.Default.NetworkToken = NetworkTokenText.Text;
			Properties.Settings.Default.OpenProjectOnStart = OpenProjectSwitch.IsChecked.Value;
			Properties.Settings.Default.Save();
			Close();
		}

		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
