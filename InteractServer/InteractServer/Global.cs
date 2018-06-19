using InteractServer.Models;
using InteractServer.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer
{
  public enum ContentType
  {
    Invalid,
    Screen,
    ServerScript,
    Patcher,
    Image,
    SoundFile,
  }

  public static class Global
  {
    public static Log.LogPage Log;
    public static Log.ErrorLogPage ErrorLog;
    public static ClientList Clients;
    public static Network.Multicast Multicast;
    public static Network.SignalSender Sender;
    public static Project.ProjectManager ProjectManager;
    public static Managers.ViewManager ViewManager;
    public static Managers.AudioManager AudioManager;

    public static Intellisense.Scripts IntelliServerScripts;
    public static Intellisense.Scripts IntelliClientScripts;
    public static Intellisense.ServerObjects ServerObjects;
    public static Intellisense.ClientObjects ClientObjects;

    public static ClientPage ClientPage;
    public static PropertiesPage PropertiesPage;
    public static Project.ExplorerPage ProjectExplorerPage;
    public static Project.ConfigPage ProjectConfigPage;
    public static String ProjectPath;

    public static MainWindow AppWindow;

    public static YSE.YseInterface Yse;

    public static void Init()
    {
      Multicast = new Network.Multicast();
      Sender = new Network.SignalSender();
      Clients = new ClientList();
      ProjectManager = new Project.ProjectManager();
      ViewManager = new Managers.ViewManager();

      Log = new Log.LogPage();
      ErrorLog = new Log.ErrorLogPage();

      IntelliServerScripts = new Intellisense.Scripts();
      IntelliClientScripts = new Intellisense.Scripts();
      ServerObjects = new Intellisense.ServerObjects();
      ClientObjects = new Intellisense.ClientObjects();

      ClientPage = new ClientPage();
      PropertiesPage = new PropertiesPage();
      ProjectExplorerPage = new Project.ExplorerPage();
      ProjectConfigPage = new Project.ConfigPage();

      Yse = new YSE.YseInterface(Log.AddEntry);
      AudioManager = new Managers.AudioManager();
    }
  }
}
