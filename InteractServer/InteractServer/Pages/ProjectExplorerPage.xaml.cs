using InteractServer.Models;
using Microsoft.Win32;
using NAudio.Wave;
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

namespace InteractServer.Pages
{
  /// <summary>
  /// Interaction logic for ProjectExplorerPage.xaml
  /// </summary>
  public partial class ProjectExplorerPage : Page
  {
    //public string SelectedImagePath { get; set; }

    // for remembering expanded nodes
    private List<string> expandedNodes;

    // for auditing sound files
    IWavePlayer player = new WaveOut();

    public ProjectExplorerPage()
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

      ProjectName.Content = Global.ProjectManager.Current.Config.Name;

      Explorer.ItemsSource = Global.ProjectManager.Current.ResourceGroups;
    }

    private void Explorer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (sender is TreeView)
      {
        TreeView tv = sender as TreeView;
        if (tv.SelectedItem != null)
        {
          if (tv.SelectedItem is Screen)
          {
            Global.ScreenManager.AddAndShow(tv.SelectedItem as Screen);
          } else if(tv.SelectedItem is ServerScript)
          {
            Global.ServerScriptManager.AddAndShow(tv.SelectedItem as ServerScript);
          } else if (tv.SelectedItem is Patcher)
          {
            Global.PatcherManager.AddAndShow(tv.SelectedItem as Patcher);
          }
        }
      }

    }

    private void AddResource_Click(object sender, RoutedEventArgs e)
    {
      ProjectResourceGroup group = ((MenuItem)sender).DataContext as ProjectResourceGroup;
      if (group == null) return;

      if (group is Images)
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
      else if (group is SoundFiles)
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
      else if (group is Screens)
      {
        Global.ScreenManager.StartNewScreen();
        Refresh();
      }
      else if (group is ServerScripts)
      {
        Global.ServerScriptManager.StartNewServerScript();
        Refresh();
      }
      else if (group is Patchers)
      {
        Global.PatcherManager.StartNewPatcher();
      }
    }

    private void RenameResource_Click(object sender, RoutedEventArgs e)
    {
      ProjectResourceGroup group = null;

      ProjectResource resource = ((MenuItem)sender).DataContext as ProjectResource;

      if(resource is Models.ServerScript)
      {
        group = Global.ProjectManager.Current.ServerScripts;
      }
      else if (resource is Models.Screen)
      {
        group = Global.ProjectManager.Current.Screens;
      }
      else if (resource is Models.Image)
      {
        group = Global.ProjectManager.Current.Images;
      }
      else if (resource is Models.SoundFile)
      {
        group = Global.ProjectManager.Current.SoundFiles;
      }
      else if (resource is Models.Patcher)
      {
        group = Global.ProjectManager.Current.Patchers;
      }

      if (group != null)
      {
        string oldName = resource.Name;
        Dialogs.RenameResource dlg = new Dialogs.RenameResource(group, resource);
        dlg.ShowDialog();
        if (resource is Models.Screen)
        {
          Screen s = resource as Models.Screen;
          Global.ScreenManager.RefreshName(s);
        } else if(resource is Models.ServerScript)
        {
          ServerScript s = resource as Models.ServerScript;
          Global.ServerScriptManager.RefreshName(s);
          Global.IntelliServerScripts.RenameScript(oldName, s.Name);
        } else if(resource is Models.Patcher)
        {
          Patcher p = resource as Patcher;
          Global.PatcherManager.RefreshName(p);
        }
        Refresh();
      }

    }

    private void ReloadResource_Click(object sender, RoutedEventArgs e)
    {
      ProjectResource resource = ((MenuItem)sender).DataContext as ProjectResource;

      if (resource is Models.Screen)
      {
        Global.Log.AddEntry("Screens only exist within the project and cannot be reloaded");
      }
      else if (resource is Models.Image)
      {
        ((Models.Image)resource).Reload();
        Global.ProjectManager.Current.Images.Save(resource as Models.Image);
      }
      else if (resource is Models.SoundFile)
      {
        ((Models.SoundFile)resource).Reload();
        Global.ProjectManager.Current.SoundFiles.Save(resource as Models.SoundFile);
      }
    }

    private void ReplaceResource_Click(object sender, RoutedEventArgs e)
    {
      ProjectResource resource = ((MenuItem)sender).DataContext as ProjectResource;

      if (resource is Models.Screen)
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
      }
    }

    private void RemoveResource_Click(object sender, RoutedEventArgs e)
    {
      ProjectResource resource = ((MenuItem)sender).DataContext as ProjectResource;

      if(resource is Models.ServerScript)
      {
        Global.ServerScriptManager.Close(resource as ServerScript);
        Global.ProjectManager.Current.ServerScripts.Remove(resource as ServerScript);
        Refresh();
      }
      if (resource is Models.Screen)
      {
        Global.ScreenManager.Close(resource as Screen);
        Global.ProjectManager.Current.Screens.Remove(resource as Screen);
        Refresh();
      }
      else if (resource is Models.Image)
      {
        Global.ProjectManager.Current.Images.Remove(resource as Models.Image);
        Refresh();
      }
      else if (resource is Models.SoundFile)
      {
        Global.ProjectManager.Current.SoundFiles.Remove(resource as Models.SoundFile);
        Refresh();
      }
      else if (resource is Models.Patcher)
      {
        Global.PatcherManager.Close(resource as Patcher);
        Global.ProjectManager.Current.Patchers.Remove(resource as Patcher);
        Refresh();
      }
    }

    private void PlayButton_Click(object sender, RoutedEventArgs e)
    {
      int ID = Convert.ToInt32((sender as Button).Tag);
      SoundFile file = Global.ProjectManager.Current.SoundFiles.Get(ID);

      if (player.PlaybackState != PlaybackState.Stopped)
      {
        player.Stop();
      }

      var stream = file.GetStream();
      if (stream != null)
      {
        player.Init(file.GetStream());
        player.Play();
      }


    }
  }
}
