using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InteractServer.Project
{
  public class Project
  {

    ProjectFile file;

    public string FileName { get; set; }
    private int Version { get; set; } = 0;

    private bool running = false;
    public bool Running { get => running; }

    // objects for project configuration
    public Config.ConfigValues Config;
    public Config.UI.Button ConfigButton;
    public Config.UI.Page ConfigPage;
    public Config.UI.Text ConfigText;
    public Config.UI.Title ConfigTitle;

    // serverscripts belonging to this project
    public ServerScript.Container ServerScripts;

    // screens belonging to this project
    public Screen.Container Screens;

    // images belonging to this project
    public Image.Container Images;

    // audio belonging to this project
    public SoundFile.Container SoundFiles;

    // patchers belonging to this project
    public Patcher.Container Patchers;

    // resourceGroups for the project explorer view
    public List<IDiskResourceFolder> Folders = new List<IDiskResourceFolder>();

    public string Name { get => Config.Name; }

    private bool validProject = false;
    public bool Valid => validProject;

    public Project(string FileName)
    {
      this.FileName = FileName;

      // load file contents
      string content;
      try
      {
        content = File.ReadAllText(FileName);
      } catch
      {
        MessageBox.Show("Project File " + FileName + " not found", "Error");
        return;
      }
      Global.ProjectPath = Path.GetDirectoryName(FileName);

      try
      {
        file = JsonConvert.DeserializeObject<ProjectFile>(content,
          new JsonSerializerSettings
          {
            TypeNameHandling = TypeNameHandling.Auto,
            SerializationBinder = new DiskResourceSerializationBinder()
          }
        );
      } catch
      {
        MessageBox.Show("This is not a valid project", "Error");
        return;
      }

      // ensure directories are in place
      
      if(!Directory.Exists(System.IO.Path.Combine(Global.ProjectPath, "Screens")))
      {
        Directory.CreateDirectory(System.IO.Path.Combine(Global.ProjectPath, "Screens"));
      }
      if(!Directory.Exists(System.IO.Path.Combine(Global.ProjectPath, "ServerScripts")))
      {
        Directory.CreateDirectory(System.IO.Path.Combine(Global.ProjectPath, "ServerScripts"));
      }
      if (!Directory.Exists(System.IO.Path.Combine(Global.ProjectPath, "Images")))
      {
        Directory.CreateDirectory(System.IO.Path.Combine(Global.ProjectPath, "Images"));
      }
      if(!Directory.Exists(System.IO.Path.Combine(Global.ProjectPath, "SoundFiles")))
      {
        Directory.CreateDirectory(System.IO.Path.Combine(Global.ProjectPath, "SoundFiles"));
      }
      if(!Directory.Exists(System.IO.Path.Combine(Global.ProjectPath, "Patchers")))
      {
        Directory.CreateDirectory(System.IO.Path.Combine(Global.ProjectPath, "Patchers"));
      }

      // prepare config values
      Config = new Config.ConfigValues(file.configValues);
      ConfigButton = new Config.UI.Button(file.configButton);
      ConfigPage = new Config.UI.Page(file.configPage);
      ConfigText = new Config.UI.Text(file.configText);
      ConfigTitle = new Config.UI.Title(file.configTitle);


      // prepare project containers
      ServerScripts = new ServerScript.Container(file.DiskResources);
      Screens = new Screen.Container(file.DiskResources);
      Images = new Image.Container(file.DiskResources);
      SoundFiles = new SoundFile.Container(file.DiskResources);
      Patchers = new Patcher.Container(file.DiskResources);

      // add all content to list for treeview
      Folders.Add(ServerScripts);
      Folders.Add(Screens);
      Folders.Add(Images);
      Folders.Add(SoundFiles);
      Folders.Add(Patchers);

      // update the last opened project
      Properties.Settings.Default.LastOpenProject = FileName;
      Properties.Settings.Default.Save();

      validProject = true;
    }

    public Guid ProjectID()
    {
      return Config.ID;
    }

    public void Save()
    {
      string jsonData = JsonConvert.SerializeObject(file, Formatting.Indented,
        new JsonSerializerSettings
        {
          TypeNameHandling = TypeNameHandling.Auto,
          SerializationBinder = new DiskResourceSerializationBinder()
        }
      );
      File.WriteAllText(FileName, jsonData);
      Config.Tainted = false;
      ConfigButton.Tainted = false;
      ConfigPage.Tainted = false;
      ConfigTitle.Tainted = false;
      ConfigText.Tainted = false;
      Version++;
    }

    public void RemoveFromProject(Guid resourceID)
    {
      foreach(IDiskResource resource in file.DiskResources)
      {
        if(resource.ID == resourceID)
        {
          file.DiskResources.Remove(resource);
          Save();
          return;
        }
      }
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

    public void Run()
    {
      running = true;

      MakeCurrentOnClients();
      SendScreenVersionsToClients();
      SendImageVersionsToClients();
      SendSoundFileVersionsToClients();
      SendPatcherVersionsToClients();
      SendClientListToClients();

      Screen.Item screen = Screens.Get(Global.ProjectManager.Current.Config.StartupScreen);
      screen.RunOnSelectedClients();
    }

    public void Stop()
    {
      running = false;
      // the actual project stop happens in jint
    }

    public void TestScreen(Screen.Item screen)
    {
      running = true;

      // First send current project to clients
      MakeCurrentOnClients();

      // update resources
      SendScreenVersionsToClients();
      SendImageVersionsToClients();
      SendSoundFileVersionsToClients();
      SendPatcherVersionsToClients();
      SendClientListToClients();

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
        Global.Sender.SetCurrentProject(ID, ProjectID(), Version);
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

      Global.Sender.SendProjectConfig(clientID, ProjectID(), values);
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

    public void SendPatcherVersionsToClients()
    {
      foreach (string key in Global.Clients.List.Keys)
      {
        SendPatcherVersionsToClient(key);
      }
    }

    public void SendPatcherVersionsToClient(string ID)
    {
      Patchers.SendVersionsToClient(ProjectID(), ID);
    }

    public void SendClientListToClients()
    {
      foreach(string key in Global.Clients.List.Keys)
      {
        SendClientListToClient(key);
      }
    }

    public void SendClientListToClient(string ID)
    {
      foreach(var client in Global.Clients.List)
      {
        Global.Clients.Get(ID).QueueMethod(() =>
        {
          Global.Sender.SendClientInfo(ID, client.Value.IpAddress, client.Key, client.Value.UserName);
        });
      }
    }

    public void InitIntellisense()
    {
      if (ServerScripts == null) return;

      foreach (ServerScript.Item script in ServerScripts.Resources)
      {
        Global.IntelliServerScripts.AddScript(script.Name, script.Content, Intellisense.Scriptcontent.SCRIPT_TYPE.SERVER);
      }

      foreach(Screen.Item screen in Screens.Resources)
      {
        if (screen.ScreenType.Equals("Script")) Global.IntelliClientScripts.AddScript(screen.Name, screen.Content, Intellisense.Scriptcontent.SCRIPT_TYPE.CLIENT);
        else if (screen.ScreenType.Equals("UtilityScript")) Global.IntelliClientScripts.AddScript(screen.Name, screen.Content, Intellisense.Scriptcontent.SCRIPT_TYPE.CLIENT_UTILITY);
               
      }
    }
  }
}
