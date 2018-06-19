using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace InteractServer.Project.Patcher
{
  public class Container : IDiskResourceFolder
  {
    private Collection<IDiskResource> diskResources;

    // implement IdiskResourceFolder
    public string FolderName => "Patchers";
    public string Icon => @"/InteractServer;component/Resources/Icons/Patcher_16x.png";
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
        if (diskres.Type != ContentType.Patcher) continue;

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
      foreach(Item patcher in resources)
      {
        if (patcher.Name.Equals(name)) return true;
      }
      return false;
    }

   public IContentModel Create(String Name)
    {
      string path = Global.ProjectPath;
      path = System.IO.Path.Combine(path, "Patchers", Name + ".yap");
      System.IO.File.WriteAllText(path, "");

      DiskResource resource = new DiskResource();
      resource.ID = Guid.NewGuid();
      resource.Path = "Patchers/" + Name + ".yap";
      resource.Type = ContentType.Patcher;

      diskResources.Add(resource);
      Global.ProjectManager.SaveCurrentProject();

      resources.Add(new Item(resource));
      return resources.Last() as Item;
    }

    public void Save(Item patcher)
    {
      string path = Global.ProjectPath;
      path = System.IO.Path.Combine(path, "Patchers", patcher.Name + ".yap");
      patcher.Version++;
      System.IO.File.WriteAllText(path, patcher.Content.ToString());
    }

    public void Remove(Item patcher)
    {
      string path = System.IO.Path.Combine(Global.ProjectPath, patcher.Path);
      System.IO.File.Delete(path);
      Global.ProjectManager.Current.RemoveFromProject(patcher.ID);
      Refresh();
    }

    public void SendVersionsToClient(Guid ProjectID, string clientID)
    {
      foreach(Item patcher in Resources)
      {
        Global.Clients.Get(clientID).QueueMethod(() =>
        {
          Global.Sender.SendPatcherVersion(clientID, ProjectID, patcher.ID, patcher.Version);
        });
      }
    }

    public Item Get(Guid PatcherID)
    {
      foreach(Item patcher in Resources)
      {
        if(patcher.ID == PatcherID)
        {
          return patcher;
        }
      }
      return null;
    }

    public Item Get(string name)
    {
      foreach(Item patcher in Resources)
      {
        if (patcher.Name.Equals(name)) return patcher;
      }
      return null;
    }
  }
}
