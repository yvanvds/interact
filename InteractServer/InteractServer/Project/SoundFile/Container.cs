using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project.SoundFile
{
  public class Container : IDiskResourceFolder
  {
    private Collection<IDiskResource> diskResources;

    // implement IdiskResourceFolder
    public string FolderName => "SoundFiles";
    public string Icon => @"/InteractServer;component/Resources/Icons/SoundFile_16x.png";
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
        if (diskres.Type != ContentType.SoundFile) continue;

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

    public bool SourcePathExists(string path)
    {
      foreach (IDiskResource resource in Resources)
      {
        string respath = System.IO.Path.Combine(Global.ProjectPath, resource.Path);
        if (System.IO.Path.Equals(respath, path)) return true;
      }
      return false;
    }

    public bool NameExists(string name)
    {
      foreach (Item sf in Resources)
      {
        if (sf.Name.Equals(name)) return true;
      }
      return false;
    }

    /*public string GetAvailableName(string name)
    {
      if (!NameExists(name)) return name;

      int i = 0;
      string newName;
      do
      {
        i++;
        newName = name + "_" + i.ToString();

      } while (NameExists(newName));

      return newName;
    }*/

    public bool Add(String FileName)
    {

      if (SourcePathExists(FileName))
      {
        Global.Log.AddEntry("The soundfile " + FileName + " is already part of this project.");
        return false;
      }

      DiskResource resource = new DiskResource();
      resource.ID = Guid.NewGuid();
      resource.Path = "SoundFiles/" + Path.GetFileName(FileName);
      resource.Type = ContentType.SoundFile;

      System.IO.File.Copy(FileName, Path.Combine(Global.ProjectPath, resource.Path));

      diskResources.Add(resource);
      Global.ProjectManager.SaveCurrentProject();

      resources.Add(new Item(resource));
      return true;
    }

    public void Remove(Item sf)
    {
      string path = System.IO.Path.Combine(Global.ProjectPath, sf.Path);
      System.IO.File.Delete(path);
      Global.ProjectManager.Current.RemoveFromProject(sf.ID);
      Refresh();
    }


    public void SendVersionsToClient(Guid ProjectID, string clientID)
    {
      foreach (Item sf in Resources)
      {
        Global.Clients.Get(clientID).QueueMethod(() =>
        {
          Global.Sender.SendSoundFileVersion(clientID, ProjectID, sf.ID, sf.Version);
        });
      }
    }

    public Item Get(Guid sfID)
    {
      foreach (Item sf in Resources)
      {
        if (sf.ID == sfID)
        {
          return sf;
        }
      }
      return null;
    }

    public Item Get(string name)
    {
      foreach (Item sf in Resources)
      {
        if (sf.Name.Equals(name))
        {
          return sf;
        }
      }
      return null;
    }
  }
}
