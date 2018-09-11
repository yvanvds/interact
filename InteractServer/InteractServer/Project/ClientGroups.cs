using InteractServer.Groups;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project
{
	public class ClientGroups
	{
		private List<Group> List = new List<Group>();
		private bool needsSaving = false;

		public ClientGroups()
		{
			List.Add(new Group("Guests"));
		}

		public ClientGroups(JObject data)
		{
			List.Add(new Group("Guests"));
			try
			{
				foreach (var group in data)
				{
					List.Add(new Group(group.Value as JObject));
				}
			}
			catch(Exception e)
			{
				Log.Log.Handle.AddEntry("Group List data is invalid: " + e.Message);
			}
		}

		public JObject Save()
		{
			var result = new JObject();

			foreach(var group in List)
			{
				if(group.Name != "Guests")
				{
					result[group.ID] = group.Save();
				}
			}

			needsSaving = false;
			return result;
		}

		public void AddGroup(Group group)
		{
			List.Add(group);
			needsSaving = true;
		}

		public bool NeedsSaving()
		{
			if (needsSaving) return true;
			foreach(var group in List)
			{
				if (group.NeedsSaving()) return true;
			}
			return false;
		}

		public int Count()
		{
			return List.Count;
		}

		public Group GetGroup(int index)
		{
			return List[index];
		}

		public void AddClient(Clients.Client client)
		{
			foreach(var group in List)
			{
				if(group.HasClient(client))
				{
					group.UpdateClientHandle(client);
					return;
				}
			}
			// add to guests if we're at this point
			List[0].Add(new GroupMember(client));
		}

		public void ClientHasGone(Clients.Client client)
		{
			foreach(var group in List)
			{
				if(group.HasClient(client))
				{
					if(group.Name == "Guests")
					{
						group.Remove(client);
					} else
					{
						group.ClearHandle(client);
					}
					return;
				}
			}
		}

		public void MoveMemberToGroup(GroupMember member, Group group)
		{
			foreach(var item in List)
			{
				if(item == group)
				{
					item.Add(member);
				} else
				{
					item.Remove(member);
				}
			}
		}
	}
}
