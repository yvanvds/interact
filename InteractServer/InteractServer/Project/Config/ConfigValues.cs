using InteractServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project.Config
{
  public class ConfigValues
  {
    public ConfigValues(Shared.Project.ConfigValues backend)
    {
      this.backend = backend;
    }

    [DisplayName("Project Name")]
    public string Name
    {
      get => backend.Name;
      set
      {
        backend.Name = value;
        backend.Tainted = true;
      }
    }

    [DisplayName("Startup Screen")]
    [Xceed.Wpf.Toolkit.PropertyGrid.Attributes.ItemsSource(typeof(ScreensItemSource))]
    public string StartupScreen
    {
      get => backend.StartupScreen;
      set
      {
        backend.StartupScreen = value;
        backend.Tainted = true;
      }
    }

    public class ScreensItemSource : Xceed.Wpf.Toolkit.PropertyGrid.Attributes.IItemsSource
    {
      public Xceed.Wpf.Toolkit.PropertyGrid.Attributes.ItemCollection GetValues()
      {
        Xceed.Wpf.Toolkit.PropertyGrid.Attributes.ItemCollection strings = new Xceed.Wpf.Toolkit.PropertyGrid.Attributes.ItemCollection();
        foreach (Screen.Item screen in Global.ProjectManager.Current.Screens.Resources)
        {
          strings.Add(screen.Name);
        }
        return strings;
      }
    }

    [Browsable(false)]
    public Guid ID
    {
      get => backend.ID;
      set => backend.ID = value;
    }

    [Browsable(false)]
    private Shared.Project.ConfigValues backend;

    public String Serialize()
    {
      return backend.Serialize();
    }

    public void DeSerialize(String data)
    {
      backend.DeSerialize(data);
    }

    [Browsable(false)]
    public bool Tainted
    {
      get
      {
        return backend.Tainted;
      }
      set
      {
        backend.Tainted = value;
      }
    }
  }
}
