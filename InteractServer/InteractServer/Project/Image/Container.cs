using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project.Image
{
  public class Container : IDiskResourceFolder
  {
    private Collection<IDiskResource> diskResources;

    // implement IdiskResourceFolder
    public string FolderName => "Images";
    public string Icon => @"/InteractServer;component/Resources/Icons/image_16x.png";
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
        if (diskres.Type != ContentType.Image) continue;

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
      foreach (Item image in resources)
      {
        if (image.Name.Equals(name)) return true;
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
        Global.Log.AddEntry("The image " + FileName + " is already part of this project.");
        return false;
      }
      
      DiskResource resource = new DiskResource();
      resource.ID = Guid.NewGuid();
      resource.Path = "Images/" + Path.GetFileName(FileName);
      resource.Type = ContentType.Image;

      System.IO.File.Copy(FileName, Path.Combine(Global.ProjectPath, resource.Path));

      diskResources.Add(resource);
      Global.ProjectManager.SaveCurrentProject();

      resources.Add(new Item(resource));
      return true;
    }
    /*
    public void Save(Image image)
    {
      image.Version++;
      connection.Update(image);
    }*/

    public void Remove(Item image)
    {
      image.Release();
      string path = System.IO.Path.Combine(Global.ProjectPath, image.Path);
      System.IO.File.Delete(path);
      Global.ProjectManager.Current.RemoveFromProject(image.ID);
      Refresh();
    }

    public void Reload(Item image)
    {
      image.Reload();
    }
    
    public void SendVersionsToClient(Guid ProjectID, Guid clientID)
    {
			var client = Global.Clients.Get(clientID);
      foreach (Item image in Resources)
      {
        client.QueueMethod(() =>
        {
          client.Send.ImageVersion(ProjectID, image.ID, image.Version);
        });
      }
    }

    public Item Get(Guid imageID)
    {
      foreach (Item image in Resources)
      {
        if (image.ID == imageID)
        {
          return image;
        }
      }
      return null;
    }

    public Item Get(string name)
    {
      foreach (Item image in Resources)
      {
        if (image.Name.Equals(name))
        {
          return image;
        }
      }
      return null;
    }

  }
}
