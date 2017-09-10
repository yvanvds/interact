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
    public Dictionary<string, Client> List;

    public ClientList()
    {
      screenList = new TrulyObservableCollection<Client>();
      List = new Dictionary<string, Client>();
    }

    public void Add(string ID, Client newClient)
    {
      // if this client is already known, just update IP and last seen
      if (List.ContainsKey(ID))
      {
        App.Current.Dispatcher.Invoke((Action)delegate
        {
          List[ID].IpAddress = newClient.IpAddress;
          List[ID].LastSeen = 0;
        });

        return;
      }

      // if we know this IP and the connectionID is different,
      // update the connection key
      foreach (var client in List)
      {
        if(client.Value.IpAddress.Equals(newClient.IpAddress)) {
          // notify client that the old connection is invalid
          Global.Sender.CloseConnection(client.Key);

          App.Current.Dispatcher.Invoke((Action)delegate
          {
            List.Remove(client.Key);
            screenList.Remove(client.Value);
            List.Add(ID, newClient);
            screenList.Add(newClient);
          });
          return;
        }
      }

      // else add to list and screenlist
      App.Current.Dispatcher.Invoke((Action)delegate
      {
        List.Add(ID, newClient);
        screenList.Add(newClient);
      });

    }

    public void ConfirmPresence(string ID, String IpAddress)
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

    public void Remove(string ID)
    {
      if (List.ContainsKey(ID))
      {
        App.Current.Dispatcher.Invoke((Action)delegate
        {
          screenList.Remove(List[ID]);
          List.Remove(ID);
        });
      }
    }

    public Client Get(string ID)
    {
      if (List.ContainsKey(ID)) return List[ID];
      return null;
    }

    public void Update()
    {
      // update last seen and remove old clients

      List<string> KeysToDelete = new List<string>();

      foreach (string ID in List.Keys)
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

      foreach (string ID in KeysToDelete)
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
