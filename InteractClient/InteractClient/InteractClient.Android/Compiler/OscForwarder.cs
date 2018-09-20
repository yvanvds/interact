using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Compiler
{
	public class OscForwarder : MarshalByRefObject, ScriptInterface.IOsc
	{
		OscTree.Object obj;

		public OscForwarder(string name)
		{
			obj = new OscTree.Object(new OscTree.Address(name, name), typeof(object));
			Global.OscLocal.Add(obj);
		}

		public void Clear()
		{
			obj.Endpoints.List.Clear();
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
			obj.Endpoints.Add(new OscTree.Endpoint(name, (args) =>
			{
				Global.Compiler.InvokeOsc(name, args);
			}));
		}

		public void Send(string address, object[] values)
		{
			obj.Send(new OscTree.Route(address, OscTree.Route.RouteType.NAME), values);
		}

		public void Send(string address, object value)
		{
			Send(address, new object[] { value });
		}
	}
}
