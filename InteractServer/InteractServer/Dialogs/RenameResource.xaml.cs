using InteractServer.Models;
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
  /// <summary>
  /// Interaction logic for RenameResource.xaml
  /// </summary>
  public partial class RenameResource : Window
  {
    private ProjectResourceGroup group;
    private ProjectResource resource;
    private bool replaceEverywhere = false;
    private string oldName;
    private string newName;

    public RenameResource(ProjectResourceGroup group, ProjectResource resource)
    {
      InitializeComponent();
      BRename.IsEnabled = false;
      this.group = group;
      this.resource = resource;
      TBResourceName.Text = resource.Name;
      oldName = resource.Name;
    }

    private void TBResourceName_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (oldName == null) return; // happens during Initializecomponent()
      bool valid = true;
      newName = TBResourceName.Text;
      if (oldName.Equals(newName))
      {
        valid = false;
      }
      else if (newName.Length < 3)
      {
        valid = false;
      }
      else
      {
        foreach (ProjectResource resource in group.Resources)
        {
          if (resource.Name.Equals(newName))
          {
            valid = false;
            break;
          }
        }
      }

      if (!valid)
      {
        BRename.IsEnabled = false;
      }
      else
      {
        BRename.IsEnabled = true;
      }
    }

    private void CBRename_Click(object sender, RoutedEventArgs e)
    {
      replaceEverywhere = CBRename.IsChecked.Value;
    }

    private void BRename_Click(object sender, RoutedEventArgs e)
    {
      resource.Name = newName;

      if (group is Screens)
      {
        ((Screens)group).Save(resource as Screen);
      }
      else if (group is Images)
      {
        ((Images)group).Save(resource as Models.Image);
      }
      else if (group is ServerScripts)
      {
        ((ServerScripts)group).Save(resource as ServerScript);
      }


      Close();
    }
  }
}
