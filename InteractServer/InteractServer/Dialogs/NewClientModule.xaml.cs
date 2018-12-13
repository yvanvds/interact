using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
			Type = GetContentType();
			DialogResult = true;
			Close();
		}

		private ContentType GetContentType()
		{
			switch (CBType.SelectedIndex)
			{
				case 0:
					return ContentType.ClientGui;
				case 1:
					return ContentType.ClientScript;
				case 2:
					return ContentType.ClientPatcher;
				case 3:
					return ContentType.ClientSensors;
				case 4:
					return ContentType.ClientArduino;
				default:
					return ContentType.Invalid;
			}
		}

		private void SetContentType(ContentType type)
		{
			Type = type;
			switch (Type)
			{
				case ContentType.ClientGui:
					CBType.SelectedIndex = 0;
					break;
				case ContentType.ClientScript:
					CBType.SelectedIndex = 1;
					break;
				case ContentType.ClientPatcher:
					CBType.SelectedIndex = 2;
					break;
				case ContentType.ClientSensors:
					CBType.SelectedIndex = 3;
					break;
				case ContentType.ClientArduino:
					CBType.SelectedIndex = 4;
					break;
			}
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

			if (Project.Project.Current.ClientModules.FileExists(ModuleName, GetContentType()))
			{
				LFileExists.Visibility = Visibility.Visible;
				return false;
			}
			LFileExists.Visibility = Visibility.Hidden;
			return true;
		}

		public void ShowDialog(ContentType type)
		{
			SetContentType(type);
			ShowDialog();
		}

        private void TBModuleName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return && BContinue.IsEnabled)
            {
                if (!ValidModel()) return;
                Type = GetContentType();
                DialogResult = true;
                Close();
            }
        }
    }
}
