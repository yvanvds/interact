using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.JintEngine
{
  public class Clients
  {
    public void InvokeAll(string MethodName, params object[] arguments)
    {
      Global.NetworkService.InvokeMethod(MethodName, arguments);
    }

    public void Invoke(string clientID, string methodName, params object[] arguments)
    {
      Global.NetworkService.InvokeMethod(clientID, methodName, arguments);
    }

    public void InvokeSelected(string[] clients, string methodName, params object[] arguments)
    {
      Global.NetworkService.InvokeMethod(new List<string>(clients), methodName, arguments);
    }

    public void StartScreen(string screenName)
    {
      int ID = Global.ProjectManager.Current.Screens.Get(screenName).ID;
      Global.NetworkService.StartScreen(ID);
    }

    public void StartScreen(string clientID, string screenName)
    {
      int ID = Global.ProjectManager.Current.Screens.Get(screenName).ID;
      Global.NetworkService.StartScreen(clientID, ID);
    }

    public void StartScreen(string[] clients, string screenName)
    {
      int ID = Global.ProjectManager.Current.Screens.Get(screenName).ID;
      Global.NetworkService.StartScreen(new List<string>(clients), ID);
    }
  }
}
