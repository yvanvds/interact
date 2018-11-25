using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Compiler
{
	class OscForwarder : MarshalByRefObject, ScriptInterface.IOsc
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

		public void SendByID(string address, object[] values, bool OnGuiThread = false)
		{
			try
			{
				if (OnGuiThread)
				{
					Xamarin.Forms.Device.BeginInvokeOnMainThread(
						() =>
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
				Network.Sender.WriteLog("OscForwarder - " + e.Message);
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
				if (OnGuiThread)
				{
					Xamarin.Forms.Device.BeginInvokeOnMainThread(
						() =>
						{
							obj.Send(new OscTree.Route(address, OscTree.Route.RouteType.NAME), values);
						});
				}
				else
				{
					obj.Send(new OscTree.Route(address, OscTree.Route.RouteType.NAME), values);
				}
			}
			catch (Exception e)
			{
				Network.Sender.WriteLog("OscForwarder - " + e.Message);
			}

		}

		public void SendByName(string address, object value, bool OnGuiThread = false)
		{
			SendByName(address, new object[] { value }, OnGuiThread);
		}

		public void ToResolume(string address, object[] values)
		{
			// not implemented on client
		}

		public bool SendToClient(string ID, string address, object[] values)
		{
			return false;
		}

		public bool SendToClient(string ID, string address, object value)
		{
			return false;
		}
	}
}
