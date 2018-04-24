using InteractServer.Models;
using InteractServer.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer
{
  public static class Global
  {
    public static LogPage Log;
    public static ErrorLogPage ErrorLog;
    public static ClientList Clients;
    public static Network.Multicast Multicast;
    public static Network.SignalSender Sender;
    public static Managers.ProjectManager ProjectManager;
    public static Managers.ScreenManager ScreenManager;
    public static Managers.ServerScriptManager ServerScriptManager;
    public static Managers.PatcherManager PatcherManager;

    public static Intellisense.Scripts IntelliServerScripts;
    public static Intellisense.Scripts IntelliClientScripts;
    public static Intellisense.ServerObjects ServerObjects;
    public static Intellisense.ClientObjects ClientObjects;

    public static ClientPage ClientPage;
    public static PropertiesPage PropertiesPage;
    public static ProjectExplorerPage ProjectExplorerPage;
    public static ProjectConfigPage ProjectConfigPage;


    public static MainWindow AppWindow;

    public static YSE.YseInterface Yse;

    public static void Init()
    {
      Multicast = new Network.Multicast();
      Sender = new Network.SignalSender();
      Clients = new ClientList();
      ProjectManager = new Managers.ProjectManager();
      ScreenManager = new Managers.ScreenManager();
      ServerScriptManager = new Managers.ServerScriptManager();
      PatcherManager = new Managers.PatcherManager();
      Log = new LogPage();
      ErrorLog = new ErrorLogPage();

      IntelliServerScripts = new Intellisense.Scripts();
      IntelliClientScripts = new Intellisense.Scripts();
      ServerObjects = new Intellisense.ServerObjects();
      ClientObjects = new Intellisense.ClientObjects();

      ClientPage = new ClientPage();
      PropertiesPage = new PropertiesPage();
      ProjectExplorerPage = new ProjectExplorerPage();
      ProjectConfigPage = new ProjectConfigPage();

      Yse = new YSE.YseInterface(Log.AddEntry);
    }
  }
}
