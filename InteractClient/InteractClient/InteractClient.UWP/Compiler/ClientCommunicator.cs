using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Compiler
{
	public class ClientCommunicator : MarshalByRefObject, ICommunicator
	{
		public void AddLogEntry(string message)
		{
			Network.Sender.WriteLog(message);
		}

		public void AddOscEndpoint(string name)
		{
			Global.ClientScripts.Endpoints.Add(new OscTree.Endpoint(name, (args) =>
			{
				Global.Compiler?.InvokeOsc(name, args);
			}, typeof(object[])));
		}

        public int ClientCount()
        {
            return 0;
        }

        public bool ClientIDExists(string ID)
        {
            return false;
        }

        public bool ClientNameExists(string Name)
        {
            return false;
        }

        public string ClientName(string ID)
        {
            return string.Empty;
        }

        public string ClientID(string Name)
        {
            return string.Empty;
        }

        public string ClientIP(string ID)
        {
            return string.Empty;
        }

        public void SendOscByID(string address, object[] values, bool OnGuiThread)
		{
			try
			{
				if (OnGuiThread)
				{
					Xamarin.Forms.Device.BeginInvokeOnMainThread(
						() =>
						{
							Global.OscRoot.Send(new OscTree.Route(address, OscTree.Route.RouteType.ID), values);
						});
				}
				else
				{
					Global.OscRoot.Send(new OscTree.Route(address, OscTree.Route.RouteType.ID), values);
				}
			}
			catch (Exception e)
			{
				Network.Sender.WriteLog("OscForwarder - " + e.Message);
			}
		}

		public void SendOscByName(string address, object[] values, bool OnGuiThread)
		{
			try
			{
				if (OnGuiThread)
				{
					Xamarin.Forms.Device.BeginInvokeOnMainThread(
						() =>
						{
							Global.OscRoot.Send(new OscTree.Route(address, OscTree.Route.RouteType.NAME), values);
						});
				}
				else
				{
					Global.OscRoot.Send(new OscTree.Route(address, OscTree.Route.RouteType.NAME), values);
				}
			}
			catch (Exception e)
			{
				Network.Sender.WriteLog("OscForwarder - " + e.Message);
			}
		}

		public void SendToResolume(string address, object[] values)
		{
			// not implemented on client
		}
	}
}
