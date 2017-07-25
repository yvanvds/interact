using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interact.Network;

namespace InteractClient.Implementation.Network
{
  public class Clients : Interact.Network.Clients
  {
    private List<Client> List = new List<Client>();

    public override int Count => List.Count;

    public override Interact.Network.Client this[int key] => List[key];

    public override Interact.Network.Client GetLocal()
    {
      foreach(Client client in List)
      {
        if (client.Local) return client;
      }
      return null;
    }

    public override void Invoke(string MethodName, params object[] arguments)
    {
      foreach(Client client in List)
      {
        client.Invoke(MethodName, arguments);
      }
    }

    public override void StartScreen(string screenName)
    {
      foreach(Client client in List)
      {
        client.StartScreen(screenName);
      }
    }

    public void Add(string IpAddress, string Key, string UserName, bool local)
    {
      List.Add(new Client(IpAddress, Key, UserName, local));
    }

    public void Clear()
    {
      List.Clear();
    }
  }
}
