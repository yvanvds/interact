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


	public partial class RouteInspector : MetroWindow
	{

		private OscTree.Routes list;
		public OscTree.Routes List
		{
			get => list;
			set
			{
				list = value;
				ContentList.ItemsSource = list;
				ContentList.InvalidateVisual();
			}
		}
		public OscTree.Object Origin;

		private OscTree.Tree root;

		public RouteInspector(OscTree.Tree root)
		{
			InitializeComponent();
			RemoveButton.IsEnabled = false;
			EditButton.IsEnabled = false;

			this.root = root;
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			var window = new RouteSelector(Origin, root);
			window.ShowDialog();
			if (window.CurrentRoute != null)
			{
				list.Add(window.CurrentRoute);
				ContentList.Items.Refresh();
			}
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			var selected = ContentList.SelectedItem as OscTree.Route;
			list.Remove(selected);
			ContentList.Items.Refresh();
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			var selected = ContentList.SelectedItem as OscTree.Route;
			var window = new RouteSelector(Origin, root);
			window.SetRoute(selected);
			window.ShowDialog();

			if (window.CurrentRoute != null)
			{
				int index = list.IndexOf(selected);
				if (index != -1)
				{
					list.RemoveAt(index);
					list.Insert(index, window.CurrentRoute);
					ContentList.Items.Refresh();
				}

			}
		}

		private void ContentList_Selected(object sender, RoutedEventArgs e)
		{
			if (ContentList.SelectedItem == null)
			{
				RemoveButton.IsEnabled = false;
				EditButton.IsEnabled = false;
			}
			else
			{
				RemoveButton.IsEnabled = true;
				EditButton.IsEnabled = true;
			}
		}
	}
}
