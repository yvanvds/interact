using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
	/// Interaction logic for NewServerModule.xaml
	/// </summary>
	public partial class NewServerModule : MetroWindow
	{
		private ObservableCollection<string> cbList;

		public ContentType Type;
		public string ModuleName = string.Empty;

		public NewServerModule()
		{
			InitializeComponent();
			BContinue.IsEnabled = false;
			LFileExists.Visibility = Visibility.Hidden;

			cbList = new ObservableCollection<string>();
			cbList.Add("Gui Screen");
			cbList.Add("Script File");
			cbList.Add("Audio Patcher");
			cbList.Add("Sound Page");
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
					Type = ContentType.ServerGui;
					break;
				case 1:
					Type = ContentType.ServerScript;
					break;
				case 2:
					Type = ContentType.ServerPatcher;
					break;
				case 3:
					Type = ContentType.ServerSounds;
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

			if (Project.Project.Current.ServerModules.FileExists(ModuleName))
			{
				LFileExists.Visibility = Visibility.Visible;
				return false;
			}
			LFileExists.Visibility = Visibility.Hidden;
			return true;
		}
	}
}
