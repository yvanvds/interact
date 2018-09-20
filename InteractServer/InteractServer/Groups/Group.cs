using InteractServer.Clients;
using InteractServer.Utils;
using Newtonsoft.Json.Linq;
using OscGuiControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Groups
{
	public class Group : OscGuiControl.IPropertyInterface
	{

		#region PropertyInterface
		static private PropertyCollection properties = null;
		public PropertyCollection Properties => properties;

		static Group()
		{
			properties = new PropertyCollection();
			properties.Add("Name", "Name");
			properties.Add("FirstClientGui", "Gui on Startup");
		}
		#endregion PropertyInterface

		public string name;
		public string Name
		{
			get => name;
			set
			{
				name = value;
				needsSaving = true;
			}
		}
		public string ID { get; }

		private string firstClientGui = string.Empty;
		public string FirstClientGui
		{
			get
			{
				var module = Project.Project.Current.ClientModules.Get(firstClientGui);
				if (module != null) return module.Name;
				return string.Empty;
			}
			set
			{
				var module = Project.Project.Current.ClientModules.GetByName(value);
				if (module != null) firstClientGui = module.ID;
				else firstClientGui = string.Empty;
				needsSaving = true;
			}
		}
		public string FirstClientGuiID => firstClientGui;

		public TrulyObservableCollection<GroupMember> Members = new TrulyObservableCollection<GroupMember>();
		private bool needsSaving = false;

		public Group(string name)
		{
			this.name = name;
			ID = shortid.ShortId.Generate(false, false);
		}

		public Group(JObject data)
		{
			try
			{
				name = (string)data["Name"];
				ID = (string)data["ID"];

				if(data.ContainsKey("ClientGui"))
				{
					firstClientGui = (string)data["ClientGui"];
				}

				JObject jMember = (JObject)data["Members"];

				foreach(var member in jMember)
				{
					Members.Add(new GroupMember((string)member.Value, member.Key));
				}
			} catch(Exception e)
			{
				Log.Log.Handle.AddEntry("Group data is invalid: " + e.Message);
			}
			needsSaving = false;
		}

		public JObject Save()
		{
			var result = new JObject();

			result["Name"] = name;
			result["ID"] = ID;

			var jMember = new JObject();
			foreach(var member in Members)
			{
				jMember[member.ID] = member.Name;
			}
			result["Members"] = jMember;
			result["ClientGui"] = firstClientGui;

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
			member.Handle?.QueueMethod(() =>
			{
				member.Handle?.Send.GroupSet(ID, FirstClientGuiID);
			});
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
					return;
				}
				
			}
		}

		public bool NeedsSaving()
		{
			return needsSaving;
		}
	}
}
