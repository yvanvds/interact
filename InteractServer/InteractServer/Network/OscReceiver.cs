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
		private Osc.OscReceiver receiver;

		private bool started = false;
		public bool IsStarted { get => started; }

		public OscReceiver()
		{
			Start();
		}

		public void Start()
		{
			if (receiver == null)
			{
				receiver = new Osc.OscReceiver();
				receiver.DefaultOnMessageReceived += (sender, args) =>
				{
					string address = args.Message.Address.ToString();

					if (address.StartsWith("/internal"))
					{
						ParseInternalMessage(args);
					}
					else if (address.StartsWith("/server"))
					{
						//ParseServerMessage(message);
					}
					else if (address.StartsWith("/route", StringComparison.CurrentCultureIgnoreCase))
					{
						ParseRoutedMessage(args);
					}
					else
					{
						Log.Log.Handle?.AddEntry("Unknown Network Message Received: " + args.Message.ToString());
					}
				};
			}

			receiver.Start(11234);
			started = true;
			started = true;
		}

		public void Stop()
		{
			if (!started) return;
			receiver.Stop();
			receiver = null;
			started = false;
		}

		~OscReceiver()
		{
			Stop();
		}


		void ParseRoutedMessage(Osc.OSCMessageReceivedArgs args)
		{
			int index = args.Message.Address.ToString().IndexOf("/", 1);
			string address = args.Message.Address.ToString().Substring(index);
			List<Osc.Values.IOscValue> list = args.Message.Arguments;

			App.Current.Dispatcher.Invoke((Action)delegate
			{
				Osc.Tree.Root.Deliver(new OscTree.Route(address, OscTree.Route.RouteType.ID), ToObjectArray(list));
			});
		}

		void ParseInternalMessage(Osc.OSCMessageReceivedArgs args)
		{
			int index = args.Message.Address.ToString().IndexOf("/", 1);
			string address = args.Message.Address.ToString().Substring(index);
			List<Osc.Values.IOscValue> list = args.Message.Arguments;

			switch (address)
			{
				case "/register":
					{
						string guid = ToString(list[0]);
						string userName = ToString(list[1]);
						if (userName == "null") userName = string.Empty;
						string ip = args.Origin;
						(App.Current as App)?.ClientList.Add(
							guid, new Clients.Client
						(
							userName,
							ip,
							guid
						));
						(App.Current as App)?.ClientList.Get(guid).Send.Connect();
						Log.Log.Handle?.AddEntry("Client " + userName + " connected at " + ip + ".");
						Project.Project.Current?.MakeCurrentOnClient(guid);
					}
					break;

				case "/ping":
					{
						string ip = args.Origin;
						string id = ToString(list[0]);

						App.Current?.Dispatcher.Invoke((Action)delegate
						{
							var client = (App.Current as App)?.ClientList.Get(id);
							client?.ConfirmPresence(ip);
						});
					}
					break;

				case "/log":
					{
						Log.Log.Handle.AddEntry(ToString(list[0]));
					}
					break;

				case "/disconnect":
					{
						string ip = args.Origin;
						string id = ToString(list[0]);
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
						string clientID = ToString(list[0]);
						var projectID = ToString(list[1]);
						if (Project.Project.Current != null)
						{
							if (Project.Project.Current.Running)
							{
								var client = (App.Current as App).ClientList.Get(clientID);
								var group = Project.Project.Current.Groups.GetGroup(client);
								if(group != null)
								{
									client.Send.GroupSet(group.ID, group.FirstClientGuiID);
								}
								client.Send.ProjectStart();
								if(group != null && group.FirstClientGuiID != string.Empty)
								{
									client.Send.ScreenStart(group.FirstClientGuiID);
								} else
								{
									client.Send.ScreenStart(Project.Project.Current.FirstClientGuiID);
								}
								
							}
						}
					}
					break;
					
				default:
					{
						Log.Log.Handle?.AddEntry("Unhandled Client Message: " + args.Message.Address);
					}
					break;
			}
		}


		private static int ToInt(Osc.Values.IOscValue value)
		{
			if(value.TypeTag == 'i')
			{
				return (value as Osc.Values.OscInt).Contents;
			}
			return 0;
		}

		private static string ToString(Osc.Values.IOscValue value)
		{
			if (value.TypeTag == 's')
			{
				return (value as Osc.Values.OscString).Contents;
			}
			return string.Empty;
		}

		private static object[] ToObjectArray(List<Osc.Values.IOscValue> list)
		{
			object[] result = new object[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				switch (list[i].TypeTag)
				{
					case 'f': result[i] = (list[i] as Osc.Values.OscFloat).Contents; break;
					case 'F': result[i] = false; break;
					case 'i': result[i] = (list[i] as Osc.Values.OscInt).Contents; break;
					case 'N': result[i] = null; break;
					case 's': result[i] = (list[i] as Osc.Values.OscString).Contents; break;
					case 'T': result[i] = true; break;
					default: result[i] = 0; break; // should not happen
				}
			}
			return result;
		}
	}
}
