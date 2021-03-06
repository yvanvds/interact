﻿using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
	/// Interaction logic for NewProjectDialog.xaml
	/// </summary>
	public partial class NewProjectDialog : MetroWindow
	{
		private bool ValidFolder;
		private String DiskName, ProjectName, FolderName;

		public NewProjectDialog()
		{
			InitializeComponent();
			BContinue.IsEnabled = false;
			ValidFolder = false;
			DiskName = "";
			ProjectName = "";
			FolderName = "";
		}

		private void BContinue_Click(object sender, RoutedEventArgs e)
		{
			if (!ValidProject()) return;
			Properties.Settings.Default.LastProjectFolder = FolderName;
			Properties.Settings.Default.Save();

			Project.Project.CreateProject(FolderName, DiskName, ProjectName);
			Close();
		}

		private void BPickFolder_Click(object sender, RoutedEventArgs e)
		{
			ValidFolder = false;
			using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				dialog.Description = "Choose a Folder for your Project";
				if (Properties.Settings.Default.LastProjectFolder != string.Empty)
				{
                    if(System.IO.Directory.Exists(Properties.Settings.Default.LastProjectFolder))
                    {
                        dialog.SelectedPath = Properties.Settings.Default.LastProjectFolder;
                    }
				}

				System.Windows.Forms.DialogResult result = dialog.ShowDialog();
				if (result.Equals(System.Windows.Forms.DialogResult.OK))
				{
					FolderName = TBFoldername.Text = dialog.SelectedPath;
					if (TBFoldername.Text.Length > 0)
					{
						ValidFolder = true;
					}
				}
			}
			BContinue.IsEnabled = ValidProject();
		}

		private void TBProjectName_TextChanged(object sender, TextChangedEventArgs e)
		{
			String content = ProjectName = TBProjectName.Text;
			content = Regex.Replace(content, @"[^a-zA-Z0-9 -]", "");
			content = Utils.String.UppercaseWords(content);
			content = Regex.Replace(content, @"\s+", "");

			DiskName = TBDiskName.Text = content;
			BContinue.IsEnabled = ValidProject();
		}

		private void TBDiskName_TextChanged(object sender, TextChangedEventArgs e)
		{
			DiskName = TBDiskName.Text;
			BContinue.IsEnabled = ValidProject();
		}

		private bool ValidProject()
		{
			if (!ValidFolder) return false;
			if (FolderName.Length == 0) return false;
			if (DiskName.Length == 0) return false;
			if (ProjectName.Length == 0) return false;

			if (System.IO.Directory.Exists(FolderName + @"\" + DiskName))
			{
				LFolderExists.Visibility = Visibility.Visible;
				return false;
			}
			LFolderExists.Visibility = Visibility.Hidden;
			return true;
		}
	}
}
