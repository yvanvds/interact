using InteractServer.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Osc
{
	public static class Tree
	{
		public static OscTree.Tree Root = new OscTree.Tree(new OscTree.Address("Root", "Root"));
		public static OscTree.Tree Server = new OscTree.Tree(new OscTree.Address("Server", "Server"));
		public static OscTree.Tree Client = new OscTree.Tree(new OscTree.Address("Client", "Client"));
		public static OscTree.Tree LocalClient = new OscTree.Tree(new OscTree.Address("LocalClient", "LocalClient"));
		public static OscTree.Tree AllClients = new OscTree.Tree(new OscTree.Address("AllClients", "AllClients"));
		public static OscTree.Tree ServerPatchers = new OscTree.Tree(new OscTree.Address("Patchers", "Patchers"));
		public static OscTree.Tree ServerSounds = new OscTree.Tree(new OscTree.Address("Sounds", "Sounds"));
        public static OscTree.Tree ServerRouters = new OscTree.Tree(new OscTree.Address("Outputs", "Outputs"));

        public static OscTree.Object ServerScripts = new OscTree.Object(new OscTree.Address("ServerScripts", "ServerScripts"), typeof(Object));
		public static OscTree.Object ClientScripts = new OscTree.Object(new OscTree.Address("ClientScripts", "ClientScripts"), typeof(Object));
        
		public static void Init()
		{
			Root.Add(Server);
			Root.Add(Client);

			LocalClient.IgnoreInGui = true;
			LocalClient.ReRoute += ((OscTree.Route route, object[] arguments) =>
			{
				// if this route is used on the server, it is for testing. Just route to the normal client tree.
				var newRoute = new OscTree.Route(route.OriginalName, route.Type);
				newRoute.CurrentStep = route.CurrentStep;
				Client.Deliver(newRoute, arguments);
			});
			Root.Add(LocalClient);
			

			AllClients.IgnoreInGui = true;
			AllClients.ReRoute += ((OscTree.Route route, object[] arguments) =>
			{
				string newRoute = route.OriginalName.Remove(0, 1);
				string[] parts = newRoute.Split('/');
				parts[1] = "LocalClient";
				newRoute = string.Empty;
				foreach (var part in parts)
				{
					newRoute += "/" + part;
				}

				foreach (var client in ClientList.Handle.List.Values)
				{
					client.Send.ToClient(newRoute, arguments);
				}
			});
			Root.Add(AllClients);
			

			Server.Add(ServerPatchers);
			Server.Add(ServerSounds);
			Server.Add(ServerScripts);
            Server.Add(ServerRouters);

			Client.Add(ClientScripts);
		}

		public static void AddErrorHandlingToOsc()
		{
			Root.ErrorHandler += Log.Log.Handle.AddEntry;
			Server.ErrorHandler += Log.Log.Handle.AddEntry;
			Client.ErrorHandler += Log.Log.Handle.AddEntry;
			LocalClient.ErrorHandler += Log.Log.Handle.AddEntry;
			AllClients.ErrorHandler += Log.Log.Handle.AddEntry;
		}

		public static void Clear()
		{
			Server.Clear();
			Client.Clear();
			ServerPatchers.Clear();
			ServerSounds.Clear();
            ServerRouters.Clear();
			ServerScripts.Endpoints.Clear();
			ClientScripts.Endpoints.Clear();

			Server.Add(ServerPatchers);
			Server.Add(ServerSounds);
			Server.Add(ServerScripts);
            Server.Add(ServerRouters);

			Client.Add(ClientScripts);
		}
	}
}
