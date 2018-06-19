using InteractServer.Models;
using InteractServer.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InteractServer.Utils
{
  public class ProjectExplorerTemplateSelector : DataTemplateSelector
  {
    public DataTemplate FolderTemplate { get; set; }
    public DataTemplate ScreenTemplate { get; set; }
    public DataTemplate ServerScriptTemplate { get; set; }
    public DataTemplate SoundFileTemplate { get; set; }
    public DataTemplate ImageTemplate { get; set; }
    public DataTemplate PatcherTemplate { get; set; }


    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      FrameworkElement element = container as FrameworkElement;

      if (element != null && item != null)
      {
        if (item is IDiskResourceFolder)
        {
          return FolderTemplate;
        }
        else if (item is Project.Image.Item)
        {
          return ImageTemplate;
        }
        else if (item is Project.SoundFile.Item)
        {
          return SoundFileTemplate;
        }
        else if (item is Project.Screen.Item)
        {
          return ScreenTemplate;
        }
        else if (item is Project.ServerScript.Item)
        {
          return ServerScriptTemplate;
        }
        else if (item is Project.Patcher.Item)
        {
          return PatcherTemplate;
        }
      }

      return null;
    }
  }
}
