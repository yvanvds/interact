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
            cbList.Add("Output");
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
					return ContentType.ServerGui;
				case 1:
					return ContentType.ServerScript;
				case 2:
					return ContentType.ServerPatcher;
				case 3:
					return ContentType.ServerSounds;
                case 4:
                    return ContentType.ServerOutput;
				default:
					return ContentType.Invalid;
			}
		}

		private void SetContentType(ContentType type)
		{
			Type = type;
			switch(Type)
			{
				case ContentType.ServerGui:
					CBType.SelectedIndex = 0;
					break;
				case ContentType.ServerScript:
					CBType.SelectedIndex = 1;
					break;
				case ContentType.ServerPatcher:
					CBType.SelectedIndex = 2;
					break;
				case ContentType.ServerSounds:
					CBType.SelectedIndex = 3;
					break;
                case ContentType.ServerOutput:
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

			if (Project.Project.Current.ServerModules.FileExists(ModuleName, GetContentType()))
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

        private void TBModuleName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return && BContinue.IsEnabled)
            {
                if (!ValidModel()) return;
                Type = GetContentType();
                DialogResult = true;
                Close();
            }
        }
    }
}
