using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project.Screen
{
  public class Container : IDiskResourceFolder
  {
    private Collection<IDiskResource> diskResources;

    // implement IdiskResourceFolder
    public string FolderName => "Screens";
    public string Icon => @"/InteractServer;component/Resources/Icons/Phone_16x.png";
    public bool IsExpanded { get; set; } = false;
    public int Count => resources.Count;
    public string GuiCount => " [" + Count.ToString() + "]";

    ObservableCollection<IDiskResource> resources = new ObservableCollection<IDiskResource>();
    public ObservableCollection<IDiskResource> Resources => resources;

    public Container(Collection<IDiskResource> diskResources)
    {
      this.diskResources = diskResources;

      Refresh();
    }

    public void Refresh()
    {
      // clean up deleted resources
      for (int i = resources.Count - 1; i >= 0; i--)
      {
        Guid id = resources[i].ID;
        bool found = false;
        foreach (var diskres in diskResources)
        {
          if (id.Equals(diskres.ID))
          {
            found = true;
            break;
          }
        }
        if (!found)
        {
          resources.RemoveAt(i);
        }
      }

      // add new resources
      foreach (var diskres in diskResources)
      {
        if (diskres.Type != ContentType.Screen) continue;

        Guid id = diskres.ID;
        bool found = false;
        foreach (var res in resources)
        {
          if (id.Equals(res.ID))
          {
            found = true;
            break;
          }
        }
        if (!found)
        {
          resources.Add(new Item(diskres));
        }
      }
    }

    public bool NameExists(string name)
    {
      foreach (var res in resources)
      {
        if (res.Name.Equals(name)) return true;
      }
      return false;
    }

    
    public IContentModel Create(String Name, String ScreenType)
    {
      string path = Global.ProjectPath;
      path = System.IO.Path.Combine(path, "Screens", Name + ".js");
      System.IO.File.WriteAllText(path, "");

      DiskResource resource = new DiskResource();
      resource.ID = Guid.NewGuid();
      resource.Path = "Screens/" + Name + ".js";
      resource.Type = ContentType.Screen;
      resource.Data = ScreenType;

      diskResources.Add(resource);
      Global.ProjectManager.SaveCurrentProject();

      resources.Add(new Item(resource));
      return resources.Last() as Item;
    }
    
    public void Save(Item screen)
    {
      string path = Global.ProjectPath;
      path = System.IO.Path.Combine(path, "Screens", screen.Name + ".js");
      System.IO.File.WriteAllText(path, screen.Content.ToString());
      screen.Version++;
      Global.IntelliClientScripts.AddScript(screen.Name, screen.Content.ToString(), Intellisense.Scriptcontent.SCRIPT_TYPE.CLIENT);
      
    }

    public void Remove(Item screen)
    {
      string path = System.IO.Path.Combine(Global.ProjectPath, screen.Path);
      System.IO.File.Delete(path);
      Global.ProjectManager.Current.RemoveFromProject(screen.ID);
      Global.IntelliClientScripts.RemoveScript(screen.Name);
      Refresh();
    }

    public void SendVersionsToClient(Guid ProjectID, Guid clientID)
    {
			var client = Global.Clients.Get(clientID);
      foreach (Item screen in Resources)
      {
        client.QueueMethod(() =>
        {
          client.Send.ScreenVersion(ProjectID, screen.ID, screen.Version);
        });
      }
    }

    public Item Get(Guid ScreenID)
    {
      foreach (Item screen in Resources)
      {
        if (screen.ID == ScreenID)
        {
          return screen;
        }
      }
      return null;
    }

    public Item Get(string name)
    {
      foreach (Item screen in Resources)
      {
        if (screen.Name.Equals(name))
        {
          return screen;
        }
      }
      return null;
    }
  }
}
