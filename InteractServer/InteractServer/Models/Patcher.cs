using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Models
{
  public class Patcher : ProjectResource
  {
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public int Version { get; set; }
    public byte[] Content
    {
      get
      {
        return Encoding.UTF8.GetBytes(ContentObj.Serialize());
      }
      set
      {
        if (value != null)
        {
          ContentObj = UnpackPatcherObj(Encoding.UTF8.GetString(value));

        }
        else
        {
          ContentObj = CreatePatcherObj();
        }
      }
    }

    [JsonIgnore]
    [Ignore]
    public PatcherContent.Base ContentObj { get; set; }

    public Patcher()
    {

    }

    public String Serialize()
    {
      return JsonConvert.SerializeObject(this);
    }

    public void SendToClient(string clientID)
    {
      Global.Sender.SendPatcher(Global.ProjectManager.Current.ProjectID(), clientID, ID, Serialize());
    }

    public void SendToSelectedClients()
    {
      List<string> clients = new List<string>();
      foreach (string key in Global.Clients.List.Keys)
      {
        if (Global.Clients.List[key].IsSelected)
        {
          clients.Add(key);
        }
      }
      Global.Sender.SendPatcher(clients, ID, Serialize());
    }


    public void RunOnSelectedClients()
    {
      List<string> clients = new List<string>();
      foreach (string key in Global.Clients.List.Keys)
      {
        if (Global.Clients.List[key].IsSelected)
        {
          Global.Clients.Get(key).QueueMethod(() =>
          {
            Global.Sender.StartScreen(key, ID);
          });
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
