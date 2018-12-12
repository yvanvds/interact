using InteractServer.Utils;
using System;
using System.Collections.Generic;


namespace InteractServer.Clients
{
	public class ClientList
	{
		public Dictionary<string, Client> List;
		public TrulyObservableCollection<Client> ScreenList;
		public static ClientList Handle = null;


		public ClientList()
		{
			List = new Dictionary<string, Client>();
			ScreenList = new TrulyObservableCollection<Client>();
			Handle = this;
		}

		List<string> keysToDelete = new List<string>();
		public void Update()
		{
			keysToDelete.Clear();

			App.Current.Dispatcher.Invoke((Action)delegate
			{
				foreach (var client in List)
				{
					client.Value.LastSeen++;
					if(client.Value.LastSeen > 3)
					{
						keysToDelete.Add(client.Key);
						Project.Project.Current.Groups.ClientHasGone(client.Value);
					}
				}

				foreach(var key in keysToDelete)
				{
					if(ScreenList.Contains(List[key]))
					{
						ScreenList.Remove(List[key]);
					}
					List.Remove(key);
				}
			});
			
		}

		public void Add(string id, Client newClient)
		{
			if(List.ContainsKey(id))
			{
				App.Current?.Dispatcher.Invoke((Action)delegate
				{
					List[id].Name = newClient.Name;
					List[id].ConfirmPresence(newClient.IP);
				});
			} else
			{
				App.Current?.Dispatcher.Invoke((Action)delegate
				{
					List.Add(id, newClient);
					ScreenList.Add(newClient);
					Project.Project.Current.Groups.AddClient(newClient);
				});
			}
		}

		public void AddFakeClients()
		{
			{
				var client = new Client("Fake 1", "127.0.0.1", "id1");
				client.IsFake = true;
				Add("id1", client);
			}

			{
				var client = new Client("Fake 2", "127.0.0.1", "id2");
				client.IsFake = true;
				Add("id2", client);
			}

			{
				var client = new Client("Fake 3", "127.0.0.1", "id3");
				client.IsFake = true;
				Add("id3", client);
			}
		}

		public void ConfirmPresence(string id, string ip)
		{
			if(List.ContainsKey(id))
			{
				App.Current.Dispatcher.Invoke((Action)delegate
				{
					List[id].ConfirmPresence(ip);
				});
			}
		}

		public void PingAllClients()
		{
			foreach(var client in List.Values)
			{
				client.Send.Ping();
			}
		}

		public void CloseConnections()
		{
			foreach(var client in List.Values)
			{
				client.Send.Disconnect();
			}
		}

		public void Remove(string id)
		{
			if (App.Current == null) return;
			if(List.ContainsKey(id))
			{
				App.Current.Dispatcher.Invoke((Action)delegate
				{
					Project.Project.Current.Groups.ClientHasGone(List[id]);
					ScreenList.Remove(List[id]);
					List.Remove(id);
				});
			}
		}

		public void ProjectStop()
		{
			foreach(var client in List.Values)
			{
				client.ClearQueue();
				client.Send.ProjectStop();
			}
		}

		public Client Get(string id)
		{
			if (List.ContainsKey(id)) return List[id];
			return null;
		}

		public bool IDExists(string ID)
		{
			return List.ContainsKey(ID);
		}

		public bool NameExists(string Name)
		{
			foreach(var client in List)
			{
				if (client.Value.Name.Equals(Name)) {
					return true;
				}
			}
			return false;
		}

		public string GetName(string ID)
		{
			if (List.ContainsKey(ID)) return List[ID].Name;
			return string.Empty;
		}

		public string GetID(string Name)
		{
			foreach(var client in List)
			{
				if(client.Value.Name.Equals(Name))
				{
					return client.Key;
				}
			}
			return string.Empty;
		}

		public string GetIP(string ID)
		{
			if (List.ContainsKey(ID)) return List[ID].Name;
			return string.Empty;
		}
	}
}
