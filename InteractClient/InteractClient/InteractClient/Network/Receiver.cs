using InteractClient.JintEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Network
{
	public class Receiver
	{
		private static Receiver instance = null;

		private Osc.OscReceiver receiver;
		private bool started = false;

		public static Receiver Get()
		{
			if(instance == null)
			{
				instance = new Receiver();
			}
			return instance;
		}

		public void Start()
		{
			if(receiver == null)
			{
				receiver = new Osc.OscReceiver();
			} else
			{
				return;
			}

			receiver.DefaultOnMessageReceived += (sender, args) =>
			{
				string address = args.Message.Address.ToString();

				if(address.StartsWith("/action"))
				{
					ParseAction(args);
				} else if (address.StartsWith("/client"))
				{
					ParseClient(args);
				} else if (address.StartsWith("/project"))
				{
					ParseProject(args);
				} else if (address.StartsWith("/screen"))
				{
					ParseScreen(args);
				} 

			};

			receiver.Start(11234);
			started = true;
		}

		public void Stop()
		{
			if (!started) return;
			receiver.Stop();
			receiver = null;
			started = false;
		}

		~Receiver()
		{
			Stop();
		}

		private void ParseAction(Osc.OSCMessageReceivedArgs args)
		{
			int index = args.Message.Address.ToString().IndexOf("/", 1);
			string address = args.Message.Address.ToString().Substring(index);

			switch (address)
			{
				case "/ping":
					{
						InteractClient.Network.Sender.Get().Ping();
					}
					break;

				case "/invoke":
					{
						System.Diagnostics.Debug.WriteLine("/action/invoke: " + args.Message.Arguments.ToString());
					}
					break;

				case "/connect":
					{
						Global.Connected = true;
						Global.UpdatePage();
					}
					break;

				case "/disconnect":
					{
						Global.Connected = false;
						Global.UpdatePage();
					}
					break;
				default:
					{
						Sender.Get().WriteLog("Client got invalid message: " + args.Message.ToString());
					}
					break;
			}
		}

		private void ParseClient(Osc.OSCMessageReceivedArgs args)
		{
			int index = args.Message.Address.ToString().IndexOf("/", 1);
			string address = args.Message.Address.ToString().Substring(index);
			List<Osc.Values.IOscValue> list = args.Message.Arguments;

			switch (address)
			{
				case "/add":
					{
						Guid clientID = ToGuid(list[0]);
						string ip = ToString(list[1]);
						string name = ToString(list[2]);
						Sender.Get().Clients.Add(clientID, ip, name, false);
					}
					break;
				case "/remove":
					{
						Guid clientID = ToGuid(list[0]);
						Sender.Get().Clients.Remove(clientID);
					}
					break;
				default:
					{
						Sender.Get().WriteLog("Client got invalid message: " + args.Message.ToString());
					}
					break;
			}
		}

		private void ParseProject(Osc.OSCMessageReceivedArgs args)
		{
			int index = args.Message.Address.ToString().IndexOf("/", 1);
			string address = args.Message.Address.ToString().Substring(index);
			List<Osc.Values.IOscValue> list = args.Message.Arguments;

			try
			{


				switch (address)
				{
					case "/stop":
						{
							Engine.Instance.StopRunningProject();
							Engine.Instance.SetScreenMessage("Project Stopped.", false);
						}
						break;

					case "/set":
						{
							Data.Project.SetCurrent(ToGuid(list[0]), ToInt(list[1]));
							Engine.Instance.SetScreenMessage("Loading Project...", true);
						}
						break;

					default:
						{
							Sender.Get().WriteLog("Client got invalid message: " + args.Message.ToString());
						}
						break;
				}
			} catch (Exception e)
			{
				Sender.Get().WriteLog("Client got unparseable message: " + args.Message.ToString());
				Sender.Get().WriteLog("Caused exception: " + e.Message);
			}
		}

		private void ParseScreen(Osc.OSCMessageReceivedArgs args)
		{
			int index = args.Message.Address.ToString().IndexOf("/", 1);
			string address = args.Message.Address.ToString().Substring(index);
			List<Osc.Values.IOscValue> list = args.Message.Arguments;

			switch (address)
			{
				case "/stop":
					{
						Engine.Instance.StopScreen();
						Engine.Instance.SetScreenMessage("Screen Stopped.", false);
					}
					break;

				case "/start":
					{
						Engine.Instance.StartScreen(ToGuid(list[0]));
					}
					break;
				default:
					{
						Sender.Get().WriteLog("Client got invalid message: " + args.Message.ToString());
					}
					break;
			}
		}

		private static Guid ToGuid(Osc.Values.IOscValue value)
		{
			return new Guid((value as Osc.Values.OscString).Contents);
		}

		private static int ToInt(Osc.Values.IOscValue value)
		{
			return (value as Osc.Values.OscInt).Contents;
		}

		private static string ToString(Osc.Values.IOscValue value)
		{
			return (value as Osc.Values.OscString).Contents;
		}
	}
}
