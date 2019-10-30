using InteractServer.Clients;
using InteractServer.Utils;
using Newtonsoft.Json.Linq;
using OscGuiControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Groups
{
	public class Group
	{

		#region PropertyInterface
		//static private PropertyCollection properties = null;
		//public PropertyCollection Properties => properties;

		//static Group()
		//{
		//	properties = new PropertyCollection();
		//	properties.Add("Name", "Name");
		//	properties.Add("FirstClientGui", "Gui on Startup");
		//}
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
        [DisplayName("Gui on Startup")]
		public string FirstClientGui
		{
			get
			{
				var module = Project.Project.Current.ClientModules.Get(firstClientGui);
				if (module != null) return module.Name;
				return "Not Set";
			}
			set
			{
                if (value == null) return;
                if (value.Equals("Not Set"))
                {
                    firstClientGui = string.Empty;
                    needsSaving = true;
                }
                else
                {
                    var module = Project.Project.Current.ClientModules.GetByName(value);
                    if (module != null)
                    {
                        firstClientGui = module.ID;
                        needsSaving = true;
                    }
                }
			}
		}
		public string FirstClientGuiID => firstClientGui;

		public TrulyObservableCollection<GroupMember> Members = new TrulyObservableCollection<GroupMember>();
		private bool needsSaving = false;

		private OscTree.Tree groupTree;

		public Group(string name)
		{
			this.name = name;
			if(name.Equals("Guests"))
			{
				ID = name;
			} else
			{
				ID = shortid.ShortId.Generate(false, false);
			}
			initOsc();
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

			initOsc();
		}

		~Group()
		{
			RemoveFromOsc();
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

		public void RemoveFromOsc()
		{
			if(groupTree != null)
			{
				Osc.Tree.Root.Remove(groupTree);
			}
		}

		private void initOsc()
		{
			groupTree = new OscTree.Tree(new OscTree.Address(Name, ID));
			groupTree.IgnoreInGui = true;
			groupTree.ErrorHandler += Log.Log.Handle.AddEntry;
			groupTree.ReRoute += ((OscTree.Route route, object[] arguments) =>
			{
				// remove first /
				string newRoute = route.OriginalName.Remove(0,1);
				string[] parts = newRoute.Split('/');
				parts[1] = "LocalClient";
				newRoute = string.Empty;
				foreach(var part in parts)
				{
					newRoute += "/" + part;
				}
			
				foreach(var member in Members)
				{
					try
					{
						member.Handle?.Send.ToClient(newRoute, arguments);
					}
					catch(Exception e)
					{
						Log.Log.Handle.AddEntry(e.Message);
					}
				}
			});
			Osc.Tree.Root.Add(groupTree);
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
