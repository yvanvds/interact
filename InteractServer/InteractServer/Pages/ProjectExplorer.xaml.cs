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

namespace InteractServer.Pages
{
	/// <summary>
	/// Interaction logic for ProjectExplorer.xaml
	/// </summary>
	public partial class ProjectExplorer : Page
	{
		public static ProjectExplorer Handle = null;


		public ProjectExplorer()
		{
			InitializeComponent();
			Handle = this;
		}

		private void RemoveResource_Click(object sender, RoutedEventArgs e)
		{
			var resource = ((MenuItem)sender).DataContext as Project.IResource;
			if (resource != null)
			{
				MainWindow.Handle.CloseDocument(resource.Document);
				Project.Project.Current.RemoveResource(resource);
				Refresh();
			}
		}

		private void RenameResource_Click(object sender, RoutedEventArgs e)
		{

		}

		private void AddResource_Click(object sender, RoutedEventArgs e)
		{
			Project.IFolder folder = ((MenuItem)sender).DataContext as Project.IFolder;
			Project.Project.Current.CreateResourceInFolder(folder);
		}

		private void PlayButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Explorer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if(sender is TreeView)
			{
				TreeView tv = sender as TreeView;
				if(tv.SelectedItem != null)
				{
					Project.IResource resource = tv.SelectedItem as Project.IResource;
					if (resource != null)
					{
						MainWindow.Handle.AddDocument(resource.Document);
						Properties.Handle.SetSelected(resource);
					}
				}
			}
			
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			Refresh();
		}

		public void Refresh()
		{
			Explorer.ItemsSource = null;
			if(Project.Project.Current == null)
			{
				ProjectName.Content = "No Project Loaded";
				return;
			}

			ProjectName.Content = Project.Project.Current.Name;
			Explorer.ItemsSource = Project.Project.Current.Folders;
		}

		private void ProjectName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if(Project.Project.Current != null)
			{
				Properties.Handle.SetSelected(Project.Project.Current);
			}
		}
	}
}
