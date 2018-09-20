using InteractServer.Clients;
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
		private List<Group> list = new List<Group>();
		public IReadOnlyCollection<Group> List => list.AsReadOnly();

		private bool needsSaving = false;

		public ClientGroups()
		{
			list.Add(new Group("Guests"));
		}

		public ClientGroups(JObject data)
		{
			list.Add(new Group("Guests"));
			try
			{
				foreach (var group in data)
				{
					list.Add(new Group(group.Value as JObject));
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

			foreach(var group in list)
			{
				if(group.name != "Guests")
				{
					result[group.ID] = group.Save();
				}
			}

			needsSaving = false;
			return result;
		}

		public void AddGroup(Group group)
		{
			list.Add(group);
			needsSaving = true;
		}

		public bool NeedsSaving()
		{
			if (needsSaving) return true;
			foreach(var group in list)
			{
				if (group.NeedsSaving()) return true;
			}
			return false;
		}

		public int Count()
		{
			return list.Count;
		}

		public Group GetGroup(int index)
		{
			return list[index];
		}

		public void AddClient(Clients.Client client)
		{
			foreach(var group in list)
			{
				if(group.HasClient(client))
				{
					group.UpdateClientHandle(client);
					return;
				}
			}
			// add to guests if we're at this point
			list[0].Add(new GroupMember(client));
		}

		public void ClientHasGone(Clients.Client client)
		{
			foreach(var group in list)
			{
				if(group.HasClient(client))
				{
					if(group.name == "Guests")
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
			foreach(var item in list)
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

		public Group GetGroup(Client client) {
			foreach(var group in list) {
				if(group.HasClient(client))
				{
					return group;
				}
			}
			return null;
		}
	}
}
