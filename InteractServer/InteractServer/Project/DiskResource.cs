using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project
{
  public class DiskResource : IDiskResource
  {
    string name;
    [JsonIgnore]
    public string Name => name;

    string path;
    public string Path {
      get => path;
      set
      {
        path = value;

        name = System.IO.Path.GetFileNameWithoutExtension(path);
      }
    }

    public ContentType Type { get; set; }
    public Guid ID { get; set; }
    public string Data { get; set; }
    public int Version { get; set; }

    public void MoveTo(string name)
    {
      string oldLocation = System.IO.Path.Combine(Global.ProjectPath, Path);
      string extension = System.IO.Path.GetExtension(Path);
      string newLocation = System.IO.Path.Combine(Global.ProjectPath, TypeToFolder(), name + extension);


      System.IO.File.Move(oldLocation, newLocation);
      Path = TypeToFolder() + "/" + name + extension;
      Global.ProjectManager.SaveCurrentProject();
    }

    public string TypeToFolder()
    {
      switch(Type)
      {
        case ContentType.Image: return "Images";
        case ContentType.Patcher: return "Patchers";
        case ContentType.ServerScript: return "ServerScripts";
        case ContentType.Screen: return "Screens";
        case ContentType.SoundFile: return "SoundFiles";
      }
      return "Invalid";
    }
  }
}
