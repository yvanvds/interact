using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks; 

namespace InteractServer.Network
{
	public class Receiver
	{
		private Rug.Osc.OscReceiver receiver;
		private Task task;
		private CancellationTokenSource cancellationTokenSource;

		private bool started = false;
		public bool IsStarted { get => started; }

		public Receiver()
		{
			Start();
		}

		public void Start()
		{
			if (receiver == null) receiver = new Rug.Osc.OscReceiver(11234);
			else return;

			receiver.Connect();
			cancellationTokenSource = new CancellationTokenSource();
			task = Task.Run(() => ParseAsync(cancellationTokenSource.Token));
			started = true;
		}

		~Receiver()
		{
			Stop();
		}

		public void Stop()
		{
			if (!started) return;
			cancellationTokenSource.Cancel();

			try
			{
				task.Wait();
			} catch (AggregateException e)
			{
				foreach(var v in e.InnerExceptions)
				{
					Global.Log.AddEntry("OscReceiver: " + e.Message + " " + v.Message);
				}
			}
			finally
			{
				cancellationTokenSource.Dispose();
			}

			receiver?.Close();
			started = false;
		}

		private async Task ParseAsync(CancellationToken token)
		{
			while(true)
			{
				if(receiver.State == Rug.Osc.OscSocketState.Connected)
				{
					try
					{
						Rug.Osc.OscPacket packet;
						while(receiver.TryReceive(out packet))
						{
							Rug.Osc.OscMessage message = Rug.Osc.OscMessage.Parse(packet.ToString());

							if(message.Address.StartsWith("/client"))
							{
								ParseClientMessage(message);
							}
							else if (message.Address.StartsWith("/server"))
							{
								ParseServerMessage(message);
							}
							else if (message.Address.StartsWith("/proxy"))
							{
								ParseProxyMessage(message);
							}
							else
							{
								Global.Log.AddEntry("Unknown Network Message Received: " + message.ToString());
							}
							
						}
					} catch (Exception e)
					{
						Global.Log.AddEntry("Network Receiver Error: " + e.Message);
					}
				}

				if(token.IsCancellationRequested)
				{
					token.ThrowIfCancellationRequested();
				}

				await Task.Delay(1);
			}
		}

		void ParseClientMessage(Rug.Osc.OscMessage message)
		{
			string address = message.Address.Substring(message.Address.IndexOf("/", 1));
			switch (address)
			{
				case "/register":
					{
						Guid guid = Guid.Parse(message.ElementAt(0).ToString());
						string userName = message.ElementAt(0).ToString();
						string ip = message.Origin.Address.ToString();
						Global.Clients.Add(guid, new Models.Client
						(
							userName,
							message.Origin.Address.ToString()
						));
						Global.Log.AddEntry("Client " + userName + " connected at " + ip + ".");
					}
					break;

				case "/ping":
					{
						string ip = message.Origin.Address.ToString();
						var id = ToGuid(message.ElementAt(0));

						App.Current.Dispatcher.Invoke((Action)delegate
						{
							if (Global.Clients.List.ContainsKey(id))
							{
								Global.Clients.Get(id)?.ConfirmPresence(ip);
							}
						});
					}
					break;

				case "/disconnect":
					{
						string ip = message.Origin.Address.ToString();
						var id = ToGuid(message.ElementAt(0));
						Global.Log.AddEntry("Client on " + ip + " disconnected.");
						Global.Clients.Remove(id);
					}
					break;

				case "/get/nextmethod":
					{
						var id = ToGuid(message.ElementAt(0));
						Models.Client client = Global.Clients.Get(id);
						if (client != null) client.GetNextMethod();
					}
					break;

				case "/get/projectconfig":
					{
						var id = ToGuid(message.ElementAt(0));
						Global.Log.AddEntry("Client " + Global.Clients.Get(id).UserName + " asks for project configuration.");
						Global.ProjectManager.Current.SendConfigToClient(id);
					}
					break;

				case "/get/screen":
					{
						var clientID = ToGuid(message.ElementAt(0));
						var screenID = ToGuid(message.ElementAt(1));
						Global.Log.AddEntry("Client " + Global.Clients.Get(clientID).UserName + " asks for screen " + screenID + ".");
						Global.ProjectManager.Current.Screens.Get(screenID).SendToClient(clientID);
					}
					break;

				case "/get/image":
					{
						var clientID = ToGuid(message.ElementAt(0));
						var imageID = ToGuid(message.ElementAt(1));
						Global.Log.AddEntry("Client " + Global.Clients.Get(clientID).UserName + " asks for image " + imageID + ".");
						Global.ProjectManager.Current.Images.Get(imageID).SendToClient(clientID);
					}
					break;

				case "/get/soundfile":
					{
						var clientID = ToGuid(message.ElementAt(0));
						var soundfileID = ToGuid(message.ElementAt(1));
						Global.Log.AddEntry("Client " + Global.Clients.Get(clientID).UserName + " asks for soundfile " + soundfileID + ".");
						Global.ProjectManager.Current.SoundFiles.Get(soundfileID).SendToClient(clientID);
					}
					break;

				case "/get/patcher":
					{
						var clientID = ToGuid(message.ElementAt(0));
						var patcherID = ToGuid(message.ElementAt(1));
						Global.Log.AddEntry("Client " + Global.Clients.Get(clientID).UserName + " asks for patcher " + patcherID + ".");
						Global.ProjectManager.Current.Patchers.Get(patcherID).SendToClient(clientID);
					}
					break;
			}
		}

		void ParseServerMessage(Rug.Osc.OscMessage message)
		{
			string address = message.Address.Substring(message.Address.IndexOf("/", 1));
			switch (address)
			{
				case "/invoke":
					{
						if(JintEngine.Runner.Engine != null)
						{
							object[] array = message.ToArray();
							Array.Resize<object>(ref array, array.Length + 1);
							array[array.Length - 1] = message.Origin.Address.ToString();
						}
					}
					break;
				case "/log":
					{
						Global.Log.AddEntry(message.ToString());
					}
					break;
				case "/errorlog":
					{
						Global.ErrorLog.AddEntry(
							Convert.ToInt32(message.ElementAt(0)),
							Convert.ToInt32(message.ElementAt(1)),
							message.ElementAt(2).ToString(),
							Global.ProjectManager.Current.Screens.Get(Guid.Parse(message.ElementAt(3).ToString()))
							);
					}
					break;
			}
		}

		void ParseProxyMessage(Rug.Osc.OscMessage message)
		{
			string address = message.Address.Substring(message.Address.IndexOf("/", 1));
			switch(address)
			{
				// get client address and pass through
			}
		}

		static Guid ToGuid(object o)
		{
			return new Guid(o as string);
		}
	}
}
