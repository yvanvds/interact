using Shared;
using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Network
{
    public class Server
    {

        public string Name { get; set; }
        public string Address { get; set; }

        public  static List<Server> Servers { get => servers; }
        private static List<Server> servers = new List<Server>();

        public static void AddOrUpdate(string name, string address)
        {
            bool found = false;
            foreach (Server server in Server.Servers)
            {
                if (server.Address == address)
                {
                    server.Name = name;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Server.Servers.Add(new Server
                {
                    Name = name,
                    Address = address,
                });
            }
        }
    }
}
