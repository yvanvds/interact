using InteractServer.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Models
{
  public class ClientList
  {
    // collection is used to display a client list on screen
    public TrulyObservableCollection<Client> screenList;

    // this is the main list to work with
    public Dictionary<Guid, Client> List;

    public ClientList()
    {
      screenList = new TrulyObservableCollection<Client>();
      List = new Dictionary<Guid, Client>();
    }

    public void Add(Guid ID, Client newClient)
    {
      // if this client is already known, just update IP and last seen
      if (List.ContainsKey(ID))
      {
        App.Current.Dispatcher.Invoke((Action)delegate
        {
					List[ID].UserName = newClient.UserName;
          List[ID].IpAddress = newClient.IpAddress;
          List[ID].LastSeen = 0;
        });

        return;
      }

      // else add to list and screenlist
      App.Current.Dispatcher.Invoke((Action)delegate
      {
        List.Add(ID, newClient);
        screenList.Add(newClient);
      });

    }

    public void ConfirmPresence(Guid ID, String IpAddress)
    {
      if (List.ContainsKey(ID))
      {
        App.Current.Dispatcher.Invoke((Action)delegate
        {
          List[ID].IpAddress = IpAddress;
          List[ID].LastSeen = 0;
        });
      }
    }

		public void Ping()
		{
			foreach(Client client in List.Values)
			{
				client.Send.Ping();
			}
		}

		public void CloseConnection()
		{
			foreach(var client in List.Values)
			{
				client.Send.Disconnect();
			}
		}

		public void ProjectStop()
		{
			foreach(var client in List.Values)
			{
				client.Send.ProjectStop();
			}
		}

		public void ScreenStop()
		{
			foreach(var client in List.Values)
			{
				client.Send.ScreenStop();
			}
		}

    public void Remove(Guid ID)
    {
      if (App.Current == null) return;

      if (List.ContainsKey(ID))
      {
        App.Current.Dispatcher.Invoke((Action)delegate
        {
          screenList.Remove(List[ID]);
          List.Remove(ID);
        });
      }
    }

    public Client Get(Guid ID)
    {
      if (List.ContainsKey(ID)) return List[ID];
      return null;
    }

    public void Update()
    {
      // update last seen and remove old clients

      List<Guid> KeysToDelete = new List<Guid>();

      foreach (Guid ID in List.Keys)
      {
        App.Current.Dispatcher.Invoke((Action)delegate
        {
          List[ID].LastSeen++;
        });

        if (List[ID].LastSeen > 3)
        {
          KeysToDelete.Add(ID);
        }
      }

      foreach (Guid ID in KeysToDelete)
      {
        App.Current.Dispatcher.Invoke((Action)delegate
        {
          screenList.Remove(List[ID]);
          List.Remove(ID);
        });
      }
    }
  }
}
