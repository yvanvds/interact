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

		public void SendByID(string address, object[] values)
		{
			obj.Send(new OscTree.Route(address, OscTree.Route.RouteType.ID), values);
		}

		public void SendByID(string address, object value)
		{
			SendByID(address, new object[] { value });
		}

		public void SendByName(string address, object[] values)
		{
			obj.Send(new OscTree.Route(address, OscTree.Route.RouteType.NAME), values);
		}

		public void SendByName(string address, object value)
		{
			SendByName(address, new object[] { value });
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
	}
}
