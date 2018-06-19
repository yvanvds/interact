using InteractServer.Models;
using InteractServer.Project;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace InteractServer.Project
{
  /// <summary>
  /// Interaction logic for ProjectExplorerPage.xaml
  /// </summary>
  public partial class ExplorerPage : Page
  {
    // for remembering expanded nodes
    private List<string> expandedNodes;

    public ExplorerPage()
    {
      InitializeComponent();
      expandedNodes = new List<string>();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
      Refresh();
    }

    public void Refresh()
    {
      Explorer.ItemsSource = null;
      if (Global.ProjectManager.Current == null)
      {
        ProjectName.Content = "No Project Loaded";
        return;
      }

      ProjectName.Content = Global.ProjectManager.Current.Name;

      Explorer.ItemsSource = Global.ProjectManager.Current.Folders;
      foreach(var item in Explorer.ItemsSource as List<IDiskResourceFolder>)
      {
        Global.Log.AddEntry(item.FolderName);
      }
    }

    private void Explorer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (sender is TreeView)
      {
        TreeView tv = sender as TreeView;
        if (tv.SelectedItem != null)
        {
          IContentModel model = tv.SelectedItem as IContentModel;
          if(model != null) Global.ViewManager.AddAndShow(model);
        }
      }

    }

    private void AddResource_Click(object sender, RoutedEventArgs e)
    {
      IDiskResourceFolder folder = ((MenuItem)sender).DataContext as IDiskResourceFolder;
      if (folder == null) return;

      if (folder is Image.Container)
      {
        OpenFileDialog dlg = new OpenFileDialog();
        dlg.CheckFileExists = true;
        dlg.Filter = "Images Files|*.jpg;*.jpeg;*.png;";
        dlg.Multiselect = true;

        if (dlg.ShowDialog() == true)
        {
          foreach (string name in dlg.FileNames)
          {
            Global.ProjectManager.Current.Images.Add(name);
          }
          Refresh();
        }
      }
      else if (folder is SoundFile.Container)
      {
        OpenFileDialog dlg = new OpenFileDialog();
        dlg.CheckFileExists = true;
        dlg.Filter = "Sound Files|*.mp3;*.ogg;*.wav;";
        dlg.Multiselect = true;

        if (dlg.ShowDialog() == true)
        {
          foreach (string name in dlg.FileNames)
          {
            Global.ProjectManager.Current.SoundFiles.Add(name);
          }
          Refresh();
        }
      }
      else if (folder is Screen.Container)
      {
        Global.ViewManager.StartNewView(ContentType.Screen);
        Refresh();
      }
      else if (folder is ServerScript.Container)
      {
        Global.ViewManager.StartNewView(ContentType.ServerScript);
        Refresh();
      }
      else if (folder is Patcher.Container)
      {
        Global.ViewManager.StartNewView(ContentType.Patcher);
        Refresh();
      }
    }

    private void RenameResource_Click(object sender, RoutedEventArgs e)
    {
      IDiskResourceFolder folder = null;

      IDiskResource resource = ((MenuItem)sender).DataContext as IDiskResource;

      switch(resource.Type)
      {
        case ContentType.ServerScript:
          folder = Global.ProjectManager.Current.ServerScripts;
          break;
        case ContentType.Screen:
          folder = Global.ProjectManager.Current.Screens;
          break;
        case ContentType.SoundFile:
          folder = Global.ProjectManager.Current.SoundFiles;
          break;
        case ContentType.Image:
          folder = Global.ProjectManager.Current.Images;
          break;
        case ContentType.Patcher:
          folder = Global.ProjectManager.Current.Patchers;
          break;
      }

      if (folder != null)
      {
        string oldName = resource.Name;
        Dialogs.RenameResource dlg = new Dialogs.RenameResource(folder, resource);
        dlg.ShowDialog();
        if (resource is IContentModel)
        {
          Global.ViewManager.RefreshName(resource as IContentModel);

          if(resource is ServerScript.Item)
          {
            Global.IntelliServerScripts.RenameScript(oldName, resource.Name);
          }
        } 
        Refresh();
      }
      
    }

    private void ReloadResource_Click(object sender, RoutedEventArgs e)
    {
      IDiskResource resource = ((MenuItem)sender).DataContext as IDiskResource;
      IContentModel model = ((MenuItem)sender).DataContext as IContentModel;

      Global.ViewManager.Close(model);

      switch (resource.Type)
      {
        case ContentType.ServerScript:
          //Global.ProjectManager.Current.ServerScripts.Reload(model as ServerScript);
          break;
        case ContentType.Screen:
          //Global.ProjectManager.Current.Screens.Reload(resource as Screen);
          break;
        case ContentType.SoundFile:
          // Global.ProjectManager.Current.SoundFiles.Reload(model as SoundFile);
          break;
        case ContentType.Image:
          //Global.ProjectManager.Current.Images.Reload(resource as Models.Image);
          break;
        case ContentType.Patcher:
          //Global.ProjectManager.Current.Patchers.Reload(model as Patcher);
          break;
      }

      Refresh();
    }

    private void ReplaceResource_Click(object sender, RoutedEventArgs e)
    {
      IDiskResourceFolder folder = ((MenuItem)sender).DataContext as IDiskResourceFolder;

      /*if (resource is Models.Screen)
      {
        Global.Log.AddEntry("Screens only exist within the project and cannot be replaced");
      }
      else if (resource is Models.Image)
      {
        OpenFileDialog dlg = new OpenFileDialog();
        dlg.CheckFileExists = true;
        dlg.Filter = "Images Files|*.jpg;*.jpeg;*.png;";
        dlg.Multiselect = false;

        if (dlg.ShowDialog() == true)
        {
          (resource as Models.Image).Replace(dlg.FileName);
          Global.ProjectManager.Current.Images.Save(resource as Models.Image);
          Refresh();
        }
      }
      else if (resource is Models.SoundFile)
      {
        OpenFileDialog dlg = new OpenFileDialog()
        {
          CheckFileExists = true,
          Filter = "Sound Files|*.mp3;*.ogg;*.wav;",
          Multiselect = false
        };
        if (dlg.ShowDialog() == true)
        {
          (resource as Models.SoundFile).Replace(dlg.FileName);
          Global.ProjectManager.Current.SoundFiles.Save(resource as Models.SoundFile);
          Refresh();
        }
      }*/
    }

    private void RemoveResource_Click(object sender, RoutedEventArgs e)
    {
      IDiskResource resource = ((MenuItem)sender).DataContext as IDiskResource;
      IContentModel model = ((MenuItem)sender).DataContext as IContentModel;

      Global.ViewManager.Close(model);

      switch (resource.Type)
      {
        case ContentType.ServerScript:
          Global.ProjectManager.Current.ServerScripts.Remove(model as ServerScript.Item);
          break;
        case ContentType.Screen:
          Global.ProjectManager.Current.Screens.Remove(model as Screen.Item);
          break;
        case ContentType.SoundFile:
          Global.ProjectManager.Current.SoundFiles.Remove(model as SoundFile.Item);
          break;
        case ContentType.Image:
          Global.ProjectManager.Current.Images.Remove(resource as Image.Item);
          break;
        case ContentType.Patcher:
          Global.ProjectManager.Current.Patchers.Remove(model as Patcher.Item);
          break;
      }

      Refresh();
    }

    private void PlayButton_Click(object sender, RoutedEventArgs e)
    {
      Guid ID = Guid.Parse((sender as Button).Tag.ToString());
      SoundFile.Item file = Global.ProjectManager.Current.SoundFiles.Get(ID);

      if (file == null) return;

      if(file.IsPlaying())
      {
        file.Stop();
      } else
      {
        file.Play();
      }
    }
  }
}
