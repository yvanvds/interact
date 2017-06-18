using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Models
{
  public class Project
  {
    private class ConfigStorage
    {
      [PrimaryKey]
      public string ID { get; set; }
      public byte[] Content { get; set; }
    }

    public string FileName { get; set; }
    private int Version { get; set; } = 0;

    // objects for project configuration
    public ProjectPart.ConfigValues Config = new ProjectPart.ConfigValues();
    public ProjectPart.UI.Button ConfigButton = new ProjectPart.UI.Button();
    public ProjectPart.UI.Page ConfigPage = new ProjectPart.UI.Page();
    public ProjectPart.UI.Text ConfigText = new ProjectPart.UI.Text();
    public ProjectPart.UI.Title ConfigTitle = new ProjectPart.UI.Title();

    // serverscripts belonging to this project
    public ServerScripts ServerScripts;

    // screens belonging to this project
    public Screens Screens;

    // images belonging to this project
    public Images Images;

    // audio belonging to this project
    public SoundFiles SoundFiles;

    // resourceGroups for the project explorer view
    public List<ProjectResourceGroup> ResourceGroups = new List<ProjectResourceGroup>();

    private SQLiteConnection connection;

    public Project(string FileName, string CreateWithName = "")
    {
      this.FileName = FileName;
      connection = new SQLiteConnection(FileName);
      connection.CreateTable<ConfigStorage>();

      if (CreateWithName.Length > 0)
      {
        // this is a new project
        Config.Name = CreateWithName;
        Config.ID = Guid.NewGuid();
        Config.Tainted = true;
        Save();
      }

      // load config values if available
      var configResult = connection.Find<ConfigStorage>("ConfigValues");
      if (configResult != null)
      {
        Config.DeSerialize(Encoding.UTF8.GetString(configResult.Content));
        Config.Tainted = false;
      }

      var buttonResult = connection.Find<ConfigStorage>("ConfigButton");
      if (buttonResult != null)
      {
        ConfigButton.DeSerialize(Encoding.UTF8.GetString(buttonResult.Content));
        ConfigButton.Tainted = false;
      }

      var pageResult = connection.Find<ConfigStorage>("ConfigPage");
      if (pageResult != null)
      {
        ConfigPage.DeSerialize(Encoding.UTF8.GetString(pageResult.Content));
        ConfigPage.Tainted = false;
      }

      var textResult = connection.Find<ConfigStorage>("ConfigText");
      if (textResult != null)
      {
        ConfigText.DeSerialize(Encoding.UTF8.GetString(textResult.Content));
        ConfigText.Tainted = false;
      }

      var titleResult = connection.Find<ConfigStorage>("ConfigTitle");
      if (titleResult != null)
      {
        ConfigTitle.DeSerialize(Encoding.UTF8.GetString(titleResult.Content));
        ConfigTitle.Tainted = false;
      }



      // load content
      ServerScripts = new ServerScripts(connection);
      Screens = new Screens(connection);
      Images = new Images(connection);
      SoundFiles = new SoundFiles(connection);

      // add all content to list for treeview
      ResourceGroups.Add(ServerScripts);
      ResourceGroups.Add(Screens);
      ResourceGroups.Add(Images);
      ResourceGroups.Add(SoundFiles);

    }

    public Guid ProjectID()
    {
      return Config.ID;
    }

    public void Save()
    {
      bool somethingWasSaved = false;

      if (Config.Tainted)
      {
        saveConfigStorage("ConfigValues", Config.Serialize());
        Config.Tainted = false;
        somethingWasSaved = true;
      }
      if (ConfigButton.Tainted)
      {
        saveConfigStorage("ConfigButton", ConfigButton.Serialize());
        ConfigButton.Tainted = false;
        somethingWasSaved = true;
      }
      if (ConfigPage.Tainted)
      {
        saveConfigStorage("ConfigPage", ConfigPage.Serialize());
        ConfigPage.Tainted = false;
        somethingWasSaved = true;
      }
      if (ConfigTitle.Tainted)
      {
        saveConfigStorage("ConfigTitle", ConfigTitle.Serialize());
        ConfigTitle.Tainted = false;
        somethingWasSaved = true;
      }
      if (ConfigText.Tainted)
      {
        saveConfigStorage("ConfigText", ConfigText.Serialize());
        ConfigText.Tainted = false;
        somethingWasSaved = true;
      }

      if (somethingWasSaved) Version++;
    }

    public bool Tainted()
    {
      if (Config.Tainted) return true;
      if (ConfigButton.Tainted) return true;
      if (ConfigPage.Tainted) return true;
      if (ConfigTitle.Tainted) return true;
      if (ConfigText.Tainted) return true;
      return false;
    }

    private void saveConfigStorage(string ID, string Value)
    {
      var result = connection.Find<ConfigStorage>(ID);
      if (result != null)
      {
        result.Content = Encoding.UTF8.GetBytes(Value);
        connection.Update(result);
      }
      else
      {
        connection.Insert(new ConfigStorage()
        {
          ID = ID,
          Content = Encoding.UTF8.GetBytes(Value),
        });
      }
    }

    public void Run()
    {
      MakeCurrentOnClients();
      SendScreenVersionsToClients();
      SendImageVersionsToClients();
      SendSoundFileVersionsToClients();
      Screen screen = Screens.Get(Global.ProjectManager.Current.Config.StartupScreen);
      screen.RunOnSelectedClients();
    }

    public void TestScreen(Screen screen)
    {
      // First send current project to clients
      MakeCurrentOnClients();

      // update screens
      SendScreenVersionsToClients();
      SendImageVersionsToClients();
      SendSoundFileVersionsToClients();

      // and run this task
      screen.RunOnSelectedClients();
    }

    public void MakeCurrentOnClients()
    {
      foreach (string key in Global.Clients.List.Keys)
      {
        MakeCurrentOnClient(key);
      }
    }

    public void MakeCurrentOnClient(string ID)
    {
      Global.Clients.List[ID].QueueMethod(() =>
      {
        Global.NetworkService.SetCurrentProject(ID, ProjectID(), Version);
      });
    }

    public void SendConfigToClient(string clientID)
    {
      Dictionary<string, string> values = new Dictionary<string, string>();
      values.Add("ConfigValues", Config.Serialize());
      values.Add("ConfigButton", ConfigButton.Serialize());
      values.Add("ConfigPage", ConfigPage.Serialize());
      values.Add("ConfigTitle", ConfigTitle.Serialize());
      values.Add("ConfigText", ConfigText.Serialize());

      Global.NetworkService.SendProjectConfig(clientID, ProjectID(), values);
    }

    public void SendScreenVersionsToClients()
    {
      foreach (string key in Global.Clients.List.Keys)
      {
        SendScreenVersionsToClient(key);
      }
    }

    public void SendScreenVersionsToClient(string ID)
    {
      Screens.SendVersionsToClient(ProjectID(), ID);
    }

    public void SendImageVersionsToClients()
    {
      foreach (string key in Global.Clients.List.Keys)
      {
        SendImageVersionsToClient(key);
      }
    }

    public void SendImageVersionsToClient(string ID)
    {
      Images.SendVersionsToClient(ProjectID(), ID);
    }

    public void SendSoundFileVersionsToClients()
    {
      foreach (string key in Global.Clients.List.Keys)
      {
        SendSoundFileVersionsToClient(key);
      }
    }

    public void SendSoundFileVersionsToClient(string ID)
    {
      SoundFiles.SendVersionsToClient(ProjectID(), ID);
    }

    public void InitIntellisense()
    {
      foreach (ServerScript script in ServerScripts.Resources)
      {
        Global.IntelliServerScripts.AddScript(script.Name, script.Content, Intellisense.Scriptcontent.SCRIPT_TYPE.SERVER);
      }

      foreach(Screen screen in Screens.Resources)
      {
        if(screen.ContentObj is ScreenContent.Script)
        {
          string content = (screen.ContentObj as ScreenContent.Script).Content;
          if (screen.Type.Equals("Script")) Global.IntelliClientScripts.AddScript(screen.Name, content, Intellisense.Scriptcontent.SCRIPT_TYPE.CLIENT);
          else if (screen.Type.Equals("UtilityScript")) Global.IntelliClientScripts.AddScript(screen.Name, content, Intellisense.Scriptcontent.SCRIPT_TYPE.CLIENT_UTILITY);
        }
        
      }
    }
  }
}
