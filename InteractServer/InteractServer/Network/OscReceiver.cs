using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InteractServer.Network
{
	public class OscReceiver
	{
		private Rug.Osc.OscReceiver receiver;
		private Task task;
		private CancellationTokenSource cancellationTokenSource;

		private bool started = false;
		public bool IsStarted { get => started; }

		public OscReceiver()
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

		public void Stop()
		{
			if (!started) return;
			cancellationTokenSource.Cancel();

			try
			{
				task.Wait();
			}
			catch (AggregateException e)
			{
				foreach (var v in e.InnerExceptions)
				{
					Log.Log.Handle?.AddEntry("OscReceiver: " + e.Message + " " + v.Message);
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
			while (true)
			{
				if (receiver.State == Rug.Osc.OscSocketState.Connected)
				{
					Rug.Osc.OscPacket packet = null;
					try
					{
						while (receiver.TryReceive(out packet))
						{
							Rug.Osc.OscMessage message = packet as Rug.Osc.OscMessage;

							if (message.Address.StartsWith("/internal"))
							{
								ParseInternalMessage(message);
							}
							else if (message.Address.StartsWith("/server"))
							{
								//ParseServerMessage(message);
							}
							else if (message.Address.StartsWith("/route"))
							{
								ParseRoutedMessage(message);
							}
							else
							{
								Log.Log.Handle?.AddEntry("Unknown Network Message Received: " + message.ToString());
							}

						}
					}
					catch (Exception e)
					{
						Log.Log.Handle?.AddEntry("Network Receiver Error: " + e.Message);
						if (packet != null)
						{
							Log.Log.Handle?.AddEntry("Offending Package: " + packet.ToString());
						}

					}
				}

				if (token.IsCancellationRequested)
				{
					token.ThrowIfCancellationRequested();
				}

				await Task.Delay(1);
			}
		}

		void ParseRoutedMessage(Rug.Osc.OscMessage message)
		{
			string address = message.Address.Substring(message.Address.IndexOf("/", 1));
			object[] parms = message.ToArray();
			App.Current.Dispatcher.Invoke((Action)delegate
			{
				Osc.Tree.Root.Deliver(new OscTree.Route(address, OscTree.Route.RouteType.ID), parms);
			});
		}

		void ParseInternalMessage(Rug.Osc.OscMessage message)
		{
			string address = message.Address.Substring(message.Address.IndexOf("/", 1));
			switch (address)
			{
				case "/register":
					{
						string guid = message.ElementAt(0).ToString();
						string userName = message.ElementAt(1).ToString();
						if (userName == "null") userName = string.Empty;
						string ip = message.Origin.Address.ToString();
						(App.Current as App)?.ClientList.Add(
							guid, new Clients.Client
						(
							userName,
							message.Origin.Address.ToString(),
							guid
						));
						(App.Current as App)?.ClientList.Get(guid).Send.Connect();
						Log.Log.Handle?.AddEntry("Client " + userName + " connected at " + ip + ".");
						Project.Project.Current?.MakeCurrentOnClient(guid);
					}
					break;

				case "/ping":
					{
						string ip = message.Origin.Address.ToString();
						string id = message.ElementAt(0).ToString();

						App.Current?.Dispatcher.Invoke((Action)delegate
						{
							var client = (App.Current as App)?.ClientList.Get(id);
							client?.ConfirmPresence(ip);
						});
					}
					break;

				case "/log":
					{
						Log.Log.Handle.AddEntry(message.ElementAt(0).ToString());
					}
					break;

				case "/disconnect":
					{
						string ip = message.Origin.Address.ToString();
						string id = message.ElementAt(0).ToString();
						Log.Log.Handle?.AddEntry("Client on " + ip + " disconnected.");
						(App.Current as App)?.ClientList.Remove(id);
					}
					break;
					/*
				case "/get/nextmethod":
					{
						string id = message.ElementAt(0).ToString();
						Models.Client client = Global.Clients.Get(id);
						if (client != null) client.GetNextMethod();
					}
					break;
					*/
				case "/project/ready":
					{
						string clientID = message.ElementAt(0).ToString();
						var projectID = message.ElementAt(1).ToString();
						if (Project.Project.Current != null)
						{
							if (Project.Project.Current.Running)
							{
								var client = (App.Current as App).ClientList.Get(clientID);
								client.Send.ProjectStart();
								client.Send.ScreenStart(Project.Project.Current.FirstClientGuiID);
							}
						}
					}
					break;
					
				default:
					{
						Log.Log.Handle?.AddEntry("Unhandled Client Message: " + message.Address);
					}
					break;
			}
		}

		/*void ParseServerMessage(Rug.Osc.OscMessage message)
		{
			string address = message.Address.Substring(message.Address.IndexOf("/", 1));
			switch (address)
			{
				case "/invoke":
					{
						if (JintEngine.Runner.Engine != null)
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

				default:
					{
						Global.Log.AddEntry("Unhandled Server Message: " + message.Address);
					}
					break;
			}
		}*/
	}
}
