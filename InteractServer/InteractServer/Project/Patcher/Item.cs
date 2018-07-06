using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project.Patcher
{
  public class Item : IContentModel, IDiskResource
  {
    private IDiskResource resource;

    public Guid ID { get => resource.ID; set => resource.ID = value; }
    public string Name => resource.Name;

    [JsonIgnore]
    public string Path { get => resource.Path; set => resource.Path = value; }
    [JsonIgnore]
    public string Data { get => resource.Data; set => resource.Data = value; }
    public int Version { get => resource.Version; set => resource.Version = value; }
    [JsonIgnore]
    public bool Tainted { get; set; }
    [JsonIgnore]
    public ContentType Type
    {
      get => resource.Type; set => resource.Type = value;
    }
    public void MoveTo(string path) { resource.MoveTo(path); }
    public string Content { get; set; }


    public Item(IDiskResource resource)
    {
      this.resource = resource;
      string file = System.IO.Path.Combine(Global.ProjectPath, resource.Path);
      this.Content = System.IO.File.ReadAllText(file);
      Tainted = false;
    }

    public String Serialize()
    {
      return JsonConvert.SerializeObject(this);
    }

    public void SendToClient(Guid clientID)
    {
			Global.Clients.Get(clientID).Send.PatcherSet(Global.ProjectManager.Current.ProjectID(), ID, Serialize());
    }

    public void SendToSelectedClients()
    {
			var data = Serialize();
      foreach (var key in Global.Clients.List.Keys)
      {
        if (Global.Clients.List[key].IsSelected)
        {
          Global.Clients.Get(key).Send.PatcherSet(Global.ProjectManager.Current.ProjectID(), ID, data);
				}
      }
    }

    public static PatcherContent.Base UnpackPatcherObj(string content)
    {
      return new PatcherContent.Base(content);
    }

    public static PatcherContent.Base CreatePatcherObj()
    {
      return new PatcherContent.Base("");
    }
  }
}
