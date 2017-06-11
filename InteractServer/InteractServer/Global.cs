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
    public static ClientList Clients;
    public static Network.Network Network;
    public static Network.Multicast Multicast;
    public static Network.Service NetworkService;
    public static Managers.ProjectManager ProjectManager;
    public static Managers.ScreenManager ScreenManager;
    public static Managers.ServerScriptManager ServerScriptManager;

    public static ClientPage ClientPage;
    public static PropertiesPage PropertiesPage;
    public static ProjectExplorerPage ProjectExplorerPage;
    public static ProjectConfigPage ProjectConfigPage;

    public static MainWindow AppWindow;

    static Global()
    {
      Multicast = new Network.Multicast();
      Network = new Network.Network();
      NetworkService = new Network.Service();
      Clients = new ClientList();
      ProjectManager = new Managers.ProjectManager();
      ScreenManager = new Managers.ScreenManager();
      ServerScriptManager = new Managers.ServerScriptManager();
      Log = new LogPage();

      ClientPage = new ClientPage();
      PropertiesPage = new PropertiesPage();
      ProjectExplorerPage = new ProjectExplorerPage();
      ProjectConfigPage = new ProjectConfigPage();
    }
  }
}
