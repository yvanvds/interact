using InteractServer.Project;
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
	/// Interaction logic for ClientExplorer.xaml
	/// </summary>
	public partial class ClientExplorer : Page
	{
		public static ClientExplorer Handle = null;
		ClientGroups groups = null;

		public ClientExplorer()
		{
			InitializeComponent();
			Handle = this;
		}

		public void SetGroups(ClientGroups groups)
		{
			this.groups = groups;
			GuestList.SetGroup(groups.GetGroup(0));

			for (int i = 1; i < groups.Count(); i++)
			{
				AddGroup(groups.GetGroup(i));
			}
		}

		private void AllClients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var list = sender as ListView;
			Pages.Properties.Handle?.SetSelected(list.SelectedItem);
			e.Handled = true;
		}

		private void AddGroup(Groups.Group group)
		{
			
			var coldef = new ColumnDefinition();
			coldef.Width = new GridLength(200);
			GroupGrid.ColumnDefinitions.Insert(GroupGrid.ColumnDefinitions.Count - 1, coldef);
			Grid.SetColumn(ButtonAddGroup, GroupGrid.ColumnDefinitions.Count);

			var label = new Label();
			label.Content = group.Name;
			Grid.SetColumn(label, GroupGrid.ColumnDefinitions.Count - 2);
			Grid.SetRow(label, 0);
			GroupGrid.Children.Add(label);

			var list = new Controls.GroupMemberList();
			
			list.SetGroup(group);
			Grid.SetColumn(list, GroupGrid.ColumnDefinitions.Count - 2);
			Grid.SetRow(list, 1);
			GroupGrid.Children.Add(list);
		}

		private void ButtonAddGroup_Click(object sender, RoutedEventArgs e)
		{
			var window = new Dialogs.GetString("Group Name");
			window.ShowDialog();
			if(window.DialogResult == true)
			{
				var name = window.Result;
				var group = new Groups.Group(name);
				groups.AddGroup(group);

				AddGroup(group);
			}
		}

		private void ButtonAddFakes(object sender, RoutedEventArgs e)
		{
			(App.Current as App).ClientList.AddFakeClients();
		}
	}
}
