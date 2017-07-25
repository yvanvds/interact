using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Sockets.Plugin;

namespace InteractClient.Osc
{
  public class OscSender
  {
    public string Host { get; set; }
    public int? Port { get; set; }
    private UdpSocketClient sender;

    public OscSender()
    {
      sender = new UdpSocketClient();
    }

    public OscSender(string host, int port) : this()
    {
      Host = host;
      Port = port;
    }

    public void Init(string host, int port)
    {
      Host = host;
      Port = port;
    }

    public async void Send(OscPacket packet)
    {
      if (Host != null && Port != null)
      {
        await Send(packet, Host, Port.Value);
      }
      else
      {
        throw new InvalidOperationException("No destination was supplied");
      }
    }

    public async Task Send(OscPacket packet, string hostname, int port)
    {
      await sender.SendToAsync(packet.Bytes, packet.Bytes.Length, hostname, port);
    }
  }

}
