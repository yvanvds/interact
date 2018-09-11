using GongSolutions.Wpf.DragDrop;
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

namespace InteractServer.Controls
{
	/// <summary>
	/// Interaction logic for GroupMemberList.xaml
	/// </summary>
	public partial class GroupMemberList : UserControl, IDropTarget
	{
		Groups.Group group;

		public GroupMemberList()
		{
			InitializeComponent();
			this.DataContext = this;
		}

		public void SetGroup(Groups.Group group)
		{
			this.group = group;
			List.ItemsSource = group.Members;
		}

		void IDropTarget.DragOver(IDropInfo dropInfo)
		{
			if(dropInfo.Data is Groups.GroupMember)
			{
				dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
				dropInfo.Effects = DragDropEffects.Copy;
			}
		}

		void IDropTarget.Drop(IDropInfo dropInfo)
		{
			if (dropInfo.Data is Groups.GroupMember)
			{
				var member = dropInfo.Data as Groups.GroupMember;
				Project.Project.Current.Groups.MoveMemberToGroup(member, group);
			}
		}
	}
}
