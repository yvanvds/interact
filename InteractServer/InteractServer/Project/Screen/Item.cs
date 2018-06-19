using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Shared;

namespace InteractServer.Project.Screen
{
  public static class ScreenTypes
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

  public class Item : IContentModel, IDiskResource
  {
    private IDiskResource resource;

    // implements IContentModel
    public Guid ID { get => resource.ID; set => resource.ID = value; }
    public string Name => resource.Name;
    [JsonIgnore]
    public string Path { get => resource.Path; set => resource.Path = value; }
    [JsonIgnore]
    public string Data { get => resource.Data; set => resource.Data = value; }
    [JsonIgnore]
    public bool Tainted { get; set; }
    [JsonIgnore]
    public ContentType Type { get => resource.Type; set => resource.Type = value; }
    public void MoveTo(string path) { resource.MoveTo(path); }

    public int Version { get => resource.Version; set => resource.Version = value; }

    [JsonProperty("Type")]
    public string ScreenType { get; set; }

    public string Content { get; set; }

    public Item(IDiskResource resource)
    {
      this.resource = resource;
      ScreenType = resource.Data;
      string file = System.IO.Path.Combine(Global.ProjectPath, resource.Path);
      this.Content = System.IO.File.ReadAllText(file);
      Tainted = false;
      
    }

    public String Serialize()
    {
      return JsonConvert.SerializeObject(this);
    }

    public void SendToClient(string clientID)
    {
      Global.Sender.SendScreen(Global.ProjectManager.Current.ProjectID(), clientID, ID, Serialize());
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
      Global.Sender.SendScreen(clients, ID, Serialize());
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

    public static ScreenContent.Base UnpackScreenObj(string screenType, string content)
    {
      if (screenType.Equals(ScreenTypes.Script)) return new ScreenContent.Script(content);
      if (screenType.Equals(ScreenTypes.UtilityScript)) return new ScreenContent.Script(content);
      return null;
    }

    public static ScreenContent.Base CreateScreenObj(string screenType)
    {
      if (screenType.Equals(ScreenTypes.Script)) return new ScreenContent.Script("");
      if (screenType.Equals(ScreenTypes.UtilityScript)) return new ScreenContent.Script("");
      return null;
    }
  }
}
