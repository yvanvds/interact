using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;


namespace InteractServer.Dialogs
{
	/// <summary>
	/// Interaction logic for NewClientModule.xaml
	/// </summary>
	public partial class NewClientModule : MetroWindow
	{
		private ObservableCollection<string> cbList;

		public ContentType Type;
		public string ModuleName = string.Empty;

		public NewClientModule()
		{
			InitializeComponent();
			BContinue.IsEnabled = false;
			LFileExists.Visibility = Visibility.Hidden;

			cbList = new ObservableCollection<string>();
			cbList.Add("Gui Screen");
			cbList.Add("Script File");
			cbList.Add("Audio Patcher");
			cbList.Add("Sensor Configuration");
			cbList.Add("Arduino Configuration");
			CBType.ItemsSource = cbList;
			CBType.SelectedIndex = 0;
		}

		private void BContinue_Click(object sender, RoutedEventArgs e)
		{
			if (!ValidModel()) return;
			Type = ContentType.Invalid;

			switch (CBType.SelectedIndex)
			{
				case 0:
					Type = ContentType.ClientGui;
					break;
				case 1:
					Type = ContentType.ClientScript;
					break;
				case 2:
					Type = ContentType.ClientPatcher;
					break;
				case 3:
					Type = ContentType.ClientSensors;
					break;
				case 4:
					Type = ContentType.ClientArduino;
					break;
			}
			DialogResult = true;
			Close();
		}

		private void TBModuleName_TextChanged(object sender, TextChangedEventArgs e)
		{
			ModuleName = TBModuleName.Text.Trim();
			ModuleName = Regex.Replace(ModuleName, @"[^a-zA-Z0-9 -]", "");
			ModuleName = Utils.String.UppercaseWords(ModuleName);
			ModuleName = Regex.Replace(ModuleName, @"\s+", "");

			BContinue.IsEnabled = ValidModel();
		}

		private bool ValidModel()
		{
			if (ModuleName.Length == 0) return false;

			if (Project.Project.Current.ClientModules.FileExists(ModuleName))
			{
				LFileExists.Visibility = Visibility.Visible;
				return false;
			}
			LFileExists.Visibility = Visibility.Hidden;
			return true;
		}
	}
}
