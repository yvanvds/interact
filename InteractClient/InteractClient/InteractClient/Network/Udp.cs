using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InteractClient.Network
{
    public static class Udp
    {
        private static bool autoConnect = true;

        private static UdpSocketReceiver udpReciever = new UdpSocketReceiver();

        private static string ID = string.Empty;

        public static async void Start()
        {
            udpReciever.MessageReceived += async (sender, args) =>
            {
                parseUdpMessage(args.RemoteAddress, args.ByteData);
            };

            try
            {
                await udpReciever.StartListeningAsync(Global.UdpPort);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        private static void parseUdpMessage(String ipAddress, byte[] data)
        {
            var reader = new BinaryReader(new MemoryStream(data), Encoding.UTF8);
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                switch ((Global.NetworkMessage)reader.ReadByte())
                {
                    case Global.NetworkMessage.Acknowledge:
                        {
                            // the server acknowledges us
                            string name = reader.ReadString();
                            string token = reader.ReadString();

                            Global.RunOnGui(() =>
                            {
                                Servers.Add(name, ipAddress);

                                // tokens can be used for auto connecting
                                if (autoConnect && token != "")
                                {
                                    if (token.Equals(Global.Settings.Token))
                                    {

                                        // instant connect if client and server have the same token
                                        var server = Servers.Get(ipAddress);
                                        if (server != null)
                                        {
                                            Sender.Connect(server, 11234);
                                            autoConnect = false; // only autoConnect the first time
                                        }
                                    }
                                }
                            });
                            
                            break;
                        }
                    case Global.NetworkMessage.EndOfMessage:
                        {
                            return;
                        }
                    default:
                        {
                            Debug.WriteLine("unknown message recieved");
                            break;
                        }
                }
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }
    }
}
