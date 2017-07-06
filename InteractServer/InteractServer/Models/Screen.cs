using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Shared;

namespace InteractServer.Models
{
  public static class ScreenType
  {
    // don't use an enum here because they will translate to
    // numbers in json on save. This would make the type prone
    // to errors if we would mix up these values later on.
    public static String Invalid = "Invalid";
    public static String Base = "Base";
    public static String Script = "Script";
    public static String UtilityScript = "UtilityScript";
    public static String Info = "Info";
  }

  public class Screen : ProjectResource
  {
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    public int Version { get; set; }

    public string Type { get; set; }

    // content in serialized format
    public byte[] Content
    {
      get {
        //if (ContentObj == null) ContentObj = CreateScreenObj(Type);
        return Encoding.UTF8.GetBytes(ContentObj.Serialize());
      }
      set
      {
        if (value != null)
        {
          ContentObj = UnpackScreenObj(Type, Encoding.UTF8.GetString(value));
        }
        else
        {
          ContentObj = CreateScreenObj(Type);
        }

      }
    }

    // content as used in app
    [JsonIgnore]
    [Ignore]
    public ScreenContent.Base ContentObj { get; set; }

    public Screen()
    {
      Type = ScreenType.Invalid;
    }

    public String Serialize()
    {
      return JsonConvert.SerializeObject(this);
    }

    public void SendToClient(string clientID)
    {
      Global.NetworkService.SendScreen(Global.ProjectManager.Current.ProjectID(), clientID, ID, Serialize());
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
      Global.NetworkService.SendScreen(clients, ID, Serialize());
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
            Global.NetworkService.StartScreen(key, ID);
          });
        }
      }
    }

    public static ScreenContent.Base UnpackScreenObj(string type, string content)
    {
      if (type.Equals(ScreenType.Script)) return new ScreenContent.Script(content);
      if (type.Equals(ScreenType.UtilityScript)) return new ScreenContent.Script(content);
      return null;
    }

    public static ScreenContent.Base CreateScreenObj(string type)
    {
      if (type.Equals(ScreenType.Script)) return new ScreenContent.Script("");
      if (type.Equals(ScreenType.UtilityScript)) return new ScreenContent.Script("");
      return null;
    }
  }
}
