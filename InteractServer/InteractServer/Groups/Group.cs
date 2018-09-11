using InteractServer.Clients;
using InteractServer.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Groups
{
	public class Group
	{
		public string Name { get; }
		public string ID { get; }

		public TrulyObservableCollection<GroupMember> Members = new TrulyObservableCollection<GroupMember>();
		private bool needsSaving = false;

		public Group(string name)
		{
			Name = name;
			ID = shortid.ShortId.Generate(false, false);
		}

		public Group(JObject data)
		{
			try
			{
				Name = (string)data["Name"];
				ID = (string)data["ID"];

				JObject jMember = (JObject)data["Members"];

				foreach(var member in jMember)
				{
					Members.Add(new GroupMember((string)member.Value, member.Key));
				}
			} catch(Exception e)
			{
				Log.Log.Handle.AddEntry("Group data is invalid: " + e.Message);
			}
		}

		public JObject Save()
		{
			var result = new JObject();

			result["Name"] = Name;
			result["ID"] = ID;

			var jMember = new JObject();
			foreach(var member in Members)
			{
				jMember[member.ID] = member.Name;
			}
			result["Members"] = jMember;

			needsSaving = false;
			return result;
		}

		public bool HasClient(Clients.Client client)
		{
			foreach(var member in Members)
			{
				if (client.ID == member.ID) return true;
			}
			return false;
		}

		public void UpdateClientHandle(Clients.Client client)
		{
			foreach(var member in Members)
			{
				if(client.ID == member.ID)
				{
					member.Handle = client;
					member.Name = client.Name;
				}
			}
		}

		public void Add(GroupMember member)
		{
			if (Members.Contains(member)) return;
			Members.Add(member);
			needsSaving = true;
		}

		public void Remove(GroupMember member)
		{
			if (!Members.Contains(member)) return;
			Members.Remove(member);
			needsSaving = true;
		}

		public void Remove(Client client)
		{
			foreach(var member in Members)
			{
				if(member.ID == client.ID)
				{
					Remove(member);
					return;
				}
			}
		}

		public void ClearHandle(Client client)
		{
			foreach(var member in Members)
			{
				if(member.ID == client.ID)
				{
					member.Handle = null;
				}
				return;
			}
		}

		public bool NeedsSaving()
		{
			return needsSaving;
		}
	}
}
