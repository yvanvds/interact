using ActiproSoftware.Windows.Themes;
using InteractServer.Dialogs;
using MahApps.Metro.Controls;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 
	

	public partial class MainWindow : MetroWindow
	{
		public static MainWindow Handle = null;

		public MainWindow()
		{
			InitializeComponent();
			Handle = this;

			ThemeManager.CurrentTheme = "MetroDark";
			CodeEditor.SyntaxEditorHelper.UpdateHighlightingStyleRegistryForThemeChange();

			addPages();

			Yse.Yse audio = new Yse.Yse(); // will keep an internal handle at Yse.Yse.Handle;

			if(Properties.Settings.Default.OpenProjectOnStart)
			{
				if(Properties.Settings.Default.LastOpenProject.Length > 0)
				{
					Project.Project.OpenProject(Properties.Settings.Default.LastOpenProject);
					Pages.ProjectExplorer.Handle.Refresh();
				}
			}


		}

		#region Project Management
		private void NewProject_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if(Project.Project.Current != null)
			{
				Project.Project.Current.Close();
			}

			var dialog = new Dialogs.NewProjectDialog();
			dialog.ShowDialog();
			Pages.ProjectExplorer.Handle.Refresh();
		}

		private void NewProject_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void OpenProject_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.Filter = "Interact project file (*.intp)|*.intp";
			if (Properties.Settings.Default.LastProjectFolder.Length > 0)
			{
				dialog.InitialDirectory = Properties.Settings.Default.LastProjectFolder;
			}

			if(dialog.ShowDialog() == true)
			{
				Project.Project.OpenProject(dialog.FileName);
				Properties.Settings.Default.LastProjectFolder = System.IO.Path.GetDirectoryName(dialog.FileName);
				Properties.Settings.Default.Save();
				Pages.ProjectExplorer.Handle.Refresh();
			}
		}

		private void OpenProject_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void SaveProject_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if(Project.Project.Current != null && Project.Project.Current.NeedsSaving())
				Project.Project.Current.Save();
		}

		private void SaveProject_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Project.Project.Current != null;
		}
		#endregion Project Management

		#region Panels
		private LayoutAnchorable projectExplorer;
		private LayoutAnchorable properties;
		private LayoutAnchorable log;
		private LayoutDocument clientExplorer;
		private void addPages()
		{
			{
				Frame frame = new Frame();
				frame.Content = new Pages.ProjectExplorer();
				projectExplorer = new LayoutAnchorable();
				projectExplorer.Title = "Project Explorer";
				projectExplorer.Content = frame;
				dockLeft.Children.Add(projectExplorer);
			}
			{
				Frame frame = new Frame();
				frame.Content = new Pages.Properties();
				properties = new LayoutAnchorable();
				properties.Title = "Properties";
				properties.Content = frame;
				properties.IsActive = true;
				dockRight.Children.Add(properties);
			}
			{
				Frame frame = new Frame();
				frame.Content = new Log.Log();
				log = new LayoutAnchorable();
				log.Title = "Event Log";
				log.Content = frame;
				dockBottom.Children.Add(log);
			}
			{
				Frame frame = new Frame();
				frame.Content = new Pages.ClientExplorer();
				clientExplorer = new LayoutDocument();
				clientExplorer.Title = "Clients";
				clientExplorer.Content = frame;		
			}

		}
		#endregion Panels

		public void AddDocument(LayoutDocument document)
		{
			if(dockMain.Children.Contains(document))
			{
				document.IsActive = true;
			}
			else
			{
				dockMain.InsertChildAt(dockMain.ChildrenCount, document);
				dockMain.SelectedContentIndex = dockMain.ChildrenCount-1;
			}
		}

		public void CloseDocument(LayoutDocument document)
		{
			if(dockMain.Children.Contains(document))
			{
				dockMain.Children.Remove(document);
			}
		}

		public void CloseDocuments()
		{
			dockMain.Children.Clear();
		}

		private void AppOptions_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var dialog = new Dialogs.ServerOptions();
			dialog.ShowDialog();
		}

		private void AppOptions_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Project.Project.Current?.Save();
			Application.Current.Shutdown();
		}

		private void Exit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		#region Project Status
		private void StartProject_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
		{
			if (Project.Project.Current == null)
			{
				e.CanExecute = false;
				return;
			}

			/*Item screen = Global.ProjectManager.Current.Screens.Get(Global.ProjectManager.Current.Config.StartupScreen);
			if (screen == null)
			{
				e.CanExecute = false;
				return;
			}*/

			if (Project.Project.Current.Running)
			{
				e.CanExecute = false;
				return;
			}

			e.CanExecute = true;
		}

		private async void StartProject_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{
			if (Project.Project.Current.NeedsSaving())
			{
				Project.Project.Current.Save();
				if (!Project.Project.Current.CompileClientScript())
				{
					Log.Log.Handle.AddEntry("The Client Script contains errors");
					return;
				}
			}

			Log.Log.Handle.Clear();
			await Task.Run(() => Project.Project.Current.Run());
		}

		private void StopProject_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
		{
			if (Project.Project.Current == null)
			{
				e.CanExecute = false;
				return;
			}

			e.CanExecute = Project.Project.Current.Running;
		}

		private void StopProject_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{
			(App.Current as App).ClientList.ProjectStop();
			Project.Project.Current.Stop();
		}

		#endregion Project Status

		private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{

		}

		#region View Management
		private void ViewLog(object sender, RoutedEventArgs e)
		{
			log.IsVisible = !log.IsVisible;
		}

		private void ViewExplorer(object sender, RoutedEventArgs e)
		{
			projectExplorer.IsVisible = !projectExplorer.IsVisible;
		}

		private void ViewProperties(object sender, RoutedEventArgs e)
		{
			properties.IsVisible = !properties.IsVisible;
		}

		private void ViewClientExplorer(object sender, RoutedEventArgs e)
		{
			if (clientExplorer.Root != null)
			{
				clientExplorer.Close();
			} else
			{
				dockMain.Children.Add(clientExplorer);
				clientExplorer.IsActive = true;
			}
		}

		private void ViewClientOverrideScript(object sender, RoutedEventArgs e)
		{
			if (Project.Project.Current == null) return;

			var window = new DocumentAsDialog("Client Endpoint Code", Project.Project.Current.ClientEndpointWriter.View);
			window.ShowDialog();
			if (window.DialogResult == true)
			{
				Project.Project.Current.ClientEndpointWriter.Save();
				Project.Project.Current.RecompileClientScripts();
			}
		}

		private void ViewServerOverrideScript(object sender, RoutedEventArgs e)
		{
			if (Project.Project.Current == null) return;

			var window = new DocumentAsDialog("Server Endpoint Code", Project.Project.Current.ServerEndpointWriter.View);
			window.ShowDialog();
			if (window.DialogResult == true)
			{
				Project.Project.Current.ServerEndpointWriter.Save();
				Project.Project.Current.RecompileServerScripts();
			}
		}

		#endregion View Management

		private void ProjectOptions_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var dialog = new Dialogs.ProjectOptions();
			dialog.ShowDialog();
		}

		private void ProjectOptions_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if(Project.Project.Current != null)
			{
				e.CanExecute = true;
			}
		}

		private void ProjectClose_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if(Project.Project.Current != null)
			{
				Project.Project.Current.Close();
			}
		}

		private void ProjectClose_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (Project.Project.Current != null)
			{
				e.CanExecute = true;
			}
		}

		private void About_click(object sender, RoutedEventArgs e)
		{
			var window = new Dialogs.About();
			window.ShowDialog();
		}
	}
}
