using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scripts;

namespace InteractServer.Compiler
{
	public class ClientCommunicator : MarshalByRefObject, ICommunicator
	{
		public void AddLogEntry(string message)
		{

		}

		public void AddOscEndpoint(string name)
		{
			Osc.Tree.ClientScripts.Endpoints.Add(new OscTree.Endpoint(name, (args) =>
			{
				// fake client interface, don't do anything here
			}, typeof(object[])));
		}

		public void SendOscByID(string address, object[] values, bool OnGuiThread)
		{

		}

		public void SendOscByName(string address, object[] values, bool OnGuiThread)
		{

		}

		public void SendToResolume(string address, object[] values)
		{

		}

		public override object InitializeLifetimeService()
		{
			return null;
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
	}
}
