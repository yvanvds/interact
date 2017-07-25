using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interact.Network;
using Microsoft.AspNet.SignalR;

namespace InteractServer.Implementation.Network
{
  public class Clients : Interact.Network.Clients
  {
    private List<Client> List = new List<Client>();

    public override int Count => List.Count;

    public override Interact.Network.Client this[int key]
    {
      get => List[key];
    }

    public override void Invoke(string MethodName, params object[] arguments)
    {
      Global.NetworkService.InvokeMethod(MethodName, arguments);
    }

    public override void StartScreen(string screenName)
    {
      int ID = Global.ProjectManager.Current.Screens.Get(screenName).ID;
      Global.NetworkService.StartScreen(ID);
    }

    public override Interact.Network.Client GetLocal()
    {
      Global.Log.AddEntry("Client.GetLocal() should not be called in a server script.");
      return null;
    }

    public Clients()
    {
      foreach(var client in Global.Clients.List)
      {
        List.Add(new Client(
          client.Value.IpAddress, client.Key, client.Value.UserName
        ));
      }
    }
  }
}
