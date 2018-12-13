using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Network
{
    public static class Servers
    {
        private static ObservableCollection<Server> servers = new ObservableCollection<Server>();
        public static ObservableCollection<Server> List { get => servers; }


        public static void Add(string name, string address)
        {
            foreach (var server in Servers.List)
            {
                if (server.Address == address)
                {
                    server.Name = name;
                    return;
                }
            }

            Servers.List.Add(new Server
            {
                Name = name,
                Address = address,
            });
        }

        public static Server Get(string address)
        {
            foreach (var server in Servers.List)
            {
                if (server.Address == address) return server;
            }
            return null;
        }

        public static void Clear()
        {
            Servers.List.Clear();
        }

        public static void Refresh()
        {
            Clear();
            Multicast.Get().RequestServerList();
        }
    }
}
