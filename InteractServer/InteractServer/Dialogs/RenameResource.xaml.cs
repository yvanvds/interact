using InteractServer.Project;
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
  /// <summary>
  /// Interaction logic for RenameResource.xaml
  /// </summary>
  public partial class RenameResource : MetroWindow
  {
    private IDiskResourceFolder folder;
    private IDiskResource resource;
    private bool replaceEverywhere = false;
    private string oldName;
    private string newName;

    public RenameResource(IDiskResourceFolder folder, IDiskResource resource)
    {
      InitializeComponent();
      BRename.IsEnabled = false;
      this.folder = folder;
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
        foreach (IDiskResource resource in folder.Resources)
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


    private void BRename_Click(object sender, RoutedEventArgs e)
    {
      resource.MoveTo(newName);
      Close();
    }
  }
}
