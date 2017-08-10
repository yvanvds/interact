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
  public class ServerList
  {

    public string Name { get; set; }
    public string Address { get; set; }

    public static List<ServerList> Servers { get => servers; }
    private static List<ServerList> servers = new List<ServerList>();

    public static ServerList AddOrUpdate(string name, string address)
    {
      foreach (ServerList server in ServerList.Servers)
      {
        if (server.Address == address)
        {
          server.Name = name;
          return server;
        }
      }

      ServerList.Servers.Add(new ServerList
      {
        Name = name,
        Address = address,
      });
      // return the last server
      return Servers.Last();
    }
  }
}
