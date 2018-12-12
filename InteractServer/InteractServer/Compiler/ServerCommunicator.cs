using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InteractServer.Clients;
using InteractServer.Controls;
using InteractServer.Project;
using Scripts;
using Scripts.Sound;

namespace InteractServer.Compiler
{
	public class ServerCommunicator : MarshalByRefObject, ICommunicator
	{
		public void AddOscEndpoint(string name)
		{
			Osc.Tree.ServerScripts.Endpoints.Add(new OscTree.Endpoint(name, (args) =>
			{
				Project.Project.Current?.ServerCompiler.InvokeOsc(name, args);
			}, typeof(object[])));
		}

		public void AddLogEntry(string message)
		{
			Log.Log.Handle.AddEntry(message);
		}


		public override object InitializeLifetimeService()
		{
			return null;
		}

		public void SendOscByID(string address, object[] values, bool OnGuiThread)
		{
			try
			{
				if (OnGuiThread)
				{
					App.Current?.Dispatcher.Invoke((Action)delegate
					{
						Osc.Tree.Root.Send(new OscTree.Route(address, OscTree.Route.RouteType.ID), values);
					});
				}
				else
				{
					Osc.Tree.Root.Send(new OscTree.Route(address, OscTree.Route.RouteType.ID), values);
				}
			}
			catch (Exception e)
			{
				Log.Log.Handle.AddEntry(e.Message);
			}
		}

		public void SendOscByName(string address, object[] values, bool OnGuiThread)
		{
			try
			{
				if (OnGuiThread)
				{
					App.Current?.Dispatcher.Invoke((Action)delegate
					{
						Osc.Tree.Root.Send(new OscTree.Route(address, OscTree.Route.RouteType.NAME), values);
					});
				}
				else
				{
					Osc.Tree.Root.Send(new OscTree.Route(address, OscTree.Route.RouteType.NAME), values);
				}

			}
			catch (Exception e)
			{
				Log.Log.Handle.AddEntry(e.Message);
			}
		}

		public void SendToResolume(string address, object[] values)
		{
			Network.Resolume.Handle.Send(address, values);
		}

		public int ClientCount()
		{
			return ClientList.Handle.List.Count;
		}

		public bool ClientIDExists(string ID)
		{
			return ClientList.Handle.IDExists(ID);
		}

		public bool ClientNameExists(string Name)
		{
			return ClientList.Handle.NameExists(Name);
		}

		public string ClientName(string ID)
		{
			return ClientList.Handle.GetName(ID);
		}

		public string ClientID(string Name)
		{
			return ClientList.Handle.GetID(Name);
		}

		public string ClientIP(string ID)
		{
			return ClientList.Handle.GetIP(ID);
		}
	}
}
