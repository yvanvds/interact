using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Compiler
{
	class OscForwarder : MarshalByRefObject, ScriptInterface.IOsc
	{
		OscTree.Object obj;
		bool serverside = false;

		public OscForwarder(string name, bool serverside)
		{
			this.serverside = serverside;
			obj = new OscTree.Object(new OscTree.Address(name, name), typeof(object));
			if(serverside)
			{
				Osc.Tree.Server.Add(obj);
			}
			else
			{
				Osc.Tree.Client.Add(obj);
			}
		}

		public void Dispose()
		{
			obj.DetachFromParent();
		}

		~OscForwarder()
		{
			Dispose();
		}

		public void AddEndpoint(string name)
		{
			if(serverside)
			{
				obj.Endpoints.Add(new OscTree.Endpoint(name, (args) =>
				{
					Project.Project.Current.ServerCompiler.InvokeOsc(name, args);
				}));
			} else
			{
				obj.Endpoints.Add(new OscTree.Endpoint(name, (args) =>
				{
					// don't do anything on client
				}));
			}
		}

		public void SendByID(string address, object[] values, bool OnGuiThread = false)
		{
			try
			{
				if (OnGuiThread)
				{
					App.Current?.Dispatcher.Invoke((Action)delegate
					{
						obj.Send(new OscTree.Route(address, OscTree.Route.RouteType.ID), values);
					});
				}
				else
				{
					obj.Send(new OscTree.Route(address, OscTree.Route.RouteType.ID), values);
				}

			}
			catch (Exception e)
			{
				Log.Log.Handle.AddEntry(e.Message);
			}
		}

		public void SendByID(string address, object value, bool OnGuiThread = false)
		{
			SendByID(address, new object[] { value }, OnGuiThread);
		}

		public void SendByName(string address, object[] values, bool OnGuiThread = false)
		{
			try
			{
				if(OnGuiThread)
				{
					App.Current?.Dispatcher.Invoke((Action)delegate
					{
						obj.Send(new OscTree.Route(address, OscTree.Route.RouteType.NAME), values);
					});
				} else
				{
					obj.Send(new OscTree.Route(address, OscTree.Route.RouteType.NAME), values);
				}
				
			}
			catch(Exception e)
			{
				Log.Log.Handle.AddEntry(e.Message);
			}
		}

		public void SendByName(string address, object value, bool OnGuiThread = false)
		{
			SendByName(address, new object[] { value }, OnGuiThread);
		}

		

		public void Clear()
		{
			obj.Endpoints.List.Clear();
		}

		

		public override object InitializeLifetimeService()
		{
			return null;
		}

		public void ToResolume(string address, object[] values)
		{
			Network.Resolume.Handle.Send(address, values);
		}

		public bool SendToClient(string ID, string address, object[] values)
		{
			Clients.Client target = Clients.ClientList.Handle.Get(ID);
			if(target != null)
			{
				target.Send.ToClient(address, values);
				return true;
			}
			return false;
		}

		public bool SendToClient(string ID, string address, object value)
		{
			return SendToClient(ID, address, new object[] { value });
		}
	}
}
