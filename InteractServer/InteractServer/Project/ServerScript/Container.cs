using InteractServer.Project;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project.ServerScript
{
  public class Container : IDiskResourceFolder
  {
    private Collection<IDiskResource> diskResources;
    
    // implement IdiskResourceFolder
    public string FolderName => "ServerScripts";
    public string Icon => @"/InteractServer;component/Resources/Icons/Code_16x.png";
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
      for(int i = resources.Count -1; i >= 0; i--)
      {
        Guid id = resources[i].ID;
        bool found = false;
        foreach(var diskres in diskResources)
        {
          if(id.Equals(diskres.ID)) {
            found = true;
            break;
          }
        }
        if(!found)
        {
          resources.RemoveAt(i);
        }
      }

      // add new resources
      foreach(var diskres in diskResources)
      {
        if (diskres.Type != ContentType.ServerScript) continue;

        Guid id = diskres.ID;
        bool found = false;
        foreach(var res in resources)
        {
          if(id.Equals(res.ID))
          {
            found = true;
            break;
          }
        }
        if(!found)
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

    public IContentModel Create(string name)
    {
      string path = Global.ProjectPath;
      path = System.IO.Path.Combine(path, "ServerScripts", name + ".js");
      System.IO.File.WriteAllText(path, "");

      DiskResource resource = new DiskResource();
      resource.ID = Guid.NewGuid();
      resource.Path = "ServerScripts/" + name + ".js";
      resource.Type = ContentType.ServerScript;

      diskResources.Add(resource);
      Global.ProjectManager.SaveCurrentProject();

      resources.Add(new Item(resource));
      return resources.Last() as Item;
    }
    
    public void Save(Item serverScript)
    {
      string path = Global.ProjectPath;
      path = System.IO.Path.Combine(path, "ServerScripts", serverScript.Name + ".js");
      System.IO.File.WriteAllText(path, serverScript.Content);
      serverScript.Version++;
      Global.IntelliServerScripts.AddScript(serverScript.Name, serverScript.Content, Intellisense.Scriptcontent.SCRIPT_TYPE.SERVER);
    }
    
    public void Remove(Item serverScript)
    {
      string path = System.IO.Path.Combine(Global.ProjectPath, serverScript.Path);
      System.IO.File.Delete(path);
      Global.ProjectManager.Current.RemoveFromProject(serverScript.ID);
      Global.IntelliServerScripts.RemoveScript(serverScript.Name);
      Refresh();
    }

    /*
    public ServerScript Get(int scriptID)
    {
      foreach(ServerScript serverScript in Resources)
      {
        if(serverScript.ID == scriptID)
        {
          return serverScript;
        }
      }
      return null;
    }

    public ServerScript Get(string name)
    {
      foreach(ServerScript serverScript in Resources)
      {
        if(serverScript.Name.Equals(name))
        {
          return serverScript;
        }
      }
      return null;
    }*/
  }
}
