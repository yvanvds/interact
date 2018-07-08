using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.Settings;
using Interact.Network;

namespace InteractClient.Implementation.Network
{
  public class Clients : Interact.Network.Clients
  {
    private Dictionary<Guid, Client> List = new Dictionary<Guid, Client>();

    public override int Count => List.Count;

    public override Interact.Network.Client this[int key] => List.ElementAt(key).Value;

		public Clients()
		{
			List.Add(Global.deviceID, new Client("127.0.0.1", Global.deviceID, CrossSettings.Current.Get<String>("UserName"), true));
		}

    public override Interact.Network.Client GetLocal()
    {
			return List[Global.deviceID];
    }

    public override void Invoke(string MethodName, params object[] arguments)
    {
      foreach(Client client in List.Values)
      {
        client.Invoke(MethodName, arguments);
      }
    }

    public override void StartScreen(string screenName)
    {
      foreach(Client client in List.Values)
      {
        client.StartScreen(screenName);
      }
    }

		public bool Contains(Guid clientID)
		{
			return List.ContainsKey(clientID);
		}

    public void Add(Guid clientID, string IpAddress, string UserName, bool local)
    {
      List.Add(clientID, new Client(IpAddress, clientID, UserName, local));
    }

		public void Remove(Guid clientID)
		{
			List.Remove(clientID);
		}

    public void Clear()
    {
      List.Clear();
    }
  }
}
