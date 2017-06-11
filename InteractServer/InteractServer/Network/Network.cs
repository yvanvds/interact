//using InteractServer.Controls;
using InteractServer.Models;
using InteractServer.Pages;
using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Sockets.Plugin.Abstractions;

namespace InteractServer.Network
{
    public class Network
    {
        private UdpSocketReceiver udpReciever = new UdpSocketReceiver();
        private UdpSocketClient udpSender = new UdpSocketClient();

        public Network()
        {
            udpReciever.MessageReceived += async (sender, args) =>
            {
                await parseUdpMessage(args.RemoteAddress, args.RemotePort, args.ByteData);
            };
        }

        public async void Start()
        {
            await udpReciever.StartListeningAsync(Constants.UdpPort);
        }

        /*private async Task parseTcpMessage(ITcpSocketClient client)
        {
            var reader = new BinaryReader(client.GetStream(), Encoding.UTF8);
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                switch((NetworkMessage)reader.ReadByte())
                {
                    case NetworkMessage.RequestConnection:
                        {
                            // Add new client
                            Client newClient = new Client((TcpSocketClient)client);
                            newClient.IpAddress = client.RemoteAddress; 
                            newClient.ID = Guid.NewGuid();
                            await Task.Run(() => Global.Clients.Add(newClient));

                            // send ID to client
                            var writer = GetWriter();
                            writer.Write((Byte)NetworkMessage.SetID);
                            writer.Write(newClient.ID.ToByteArray());
                            newClient.Send(writer);

                            // Log action
                            await Task.Run(() => Global.Log.AddEntry("Client added with IP: " + newClient.IpAddress));

                            break;
                        }

                    case NetworkMessage.IamOnline:
                        {
                            Guid ID = new Guid(reader.ReadBytes(16));
                            Global.Clients.ConfirmPresence(ID, client.RemoteAddress);
                            break;
                        }
                    case NetworkMessage.SetUserName:
                        {
                            Guid ID = new Guid(reader.ReadBytes(16));
                            String name = reader.ReadString();
                            Global.Clients.SetUserName(ID, name);
                            break;
                        }
                    case NetworkMessage.Disconnect:
                        {
                            Guid ID = new Guid(reader.ReadBytes(16));
                            Global.Clients.Remove(ID);
                            break;
                        }
                    case NetworkMessage.JavaException:
                        {
                            Guid ID = new Guid(reader.ReadBytes(16));
                            String error = reader.ReadString();
                            Global.Log.AddEntry(error);
                            break;
                        }
                    case NetworkMessage.EndOfMessage:
                        {
                            return;
                        }
                    default:
                        {
                            Global.Log.AddEntry("Invalid message received from " + client.RemoteAddress);
                            break;
                        }
                }
            }
        }*/

        private async Task parseUdpMessage(String IpAddress, String Port, byte[] data)
        {
            var reader = new BinaryReader(new MemoryStream(data), Encoding.UTF8);
            while(reader.BaseStream.Position < reader.BaseStream.Length)
            {
                switch((NetworkMessage)reader.ReadByte())
                {
                    
                    
                }
            }
        }

        public async void SendUdp(Client client, BinaryWriter writer)
        {
            await SendUdp(client.IpAddress, writer);
        }

        public async Task SendUdp(string IpAddress, BinaryWriter writer)
        {
            writer.Write((Byte)NetworkMessage.EndOfMessage);
            writer.Flush();
            MemoryStream stream = writer.BaseStream as MemoryStream;
            await udpSender.SendToAsync(stream.GetBuffer(), IpAddress, Constants.UdpPort);
        }

        public async Task SendUdp(Client client, MemoryStream stream)
        {
            await udpSender.SendToAsync(stream.GetBuffer(), client.IpAddress, Constants.UdpPort);
        }

        public BinaryWriter GetWriter()
        {
            var stream = new MemoryStream();
            return new BinaryWriter(stream, Encoding.UTF8);
        }

        
    }
}
