﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Network
{
	public class Receiver
	{
		private static Receiver instance = null;

		private Osc.OscReceiver receiver;
		private bool started = false;

		public static Receiver Get()
		{
			if (instance == null)
			{
				instance = new Receiver();
			}
			return instance;
		}

		public void Start()
		{
			if (receiver == null)
			{
				receiver = new Osc.OscReceiver();
			}
			else
			{
				return;
			}

			receiver.DefaultOnMessageReceived += (sender, args) =>
			{
				string address = args.Message.Address.ToString();

				if (address.StartsWith("/internal"))
				{
					ParseInternal(args);
				} else
				{
					if(address.StartsWith("/root", StringComparison.CurrentCultureIgnoreCase))
					{
						if(Global.CurrentProject.Running)
						{
							//Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
							//{
								object[] obj = ToObjectArray(args.Message.Arguments);
								Global.OscRoot.Deliver(new OscTree.Route(address, OscTree.Route.RouteType.ID), obj);
							//});
						}
					}
				}
			};

			try
			{
				receiver.Start(11234);
				started = true;
			} catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}
			
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

		private async void ParseInternal(Osc.OSCMessageReceivedArgs args)
		{
			int index = args.Message.Address.ToString().IndexOf("/", 1);
			string address = args.Message.Address.ToString().Substring(index);
			List<Osc.Values.IOscValue> list = args.Message.Arguments;

			switch (address)
			{
				case "/ping":
					{
						Sender.Ping();
					}
					break;

				case "/invoke":
					{
						System.Diagnostics.Debug.WriteLine("/internal/invoke: " + args.Message.Arguments.ToString());
					}
					break;

				case "/connect":
					{
						Global.UpdatePage();
					}
					break;

				case "/disconnect":
					{
						Network.Sender.ServerLost();
						Global.UpdatePage();
					}
					break;
				case "/project/set":
					{
						Global.SetScreenMessage("Loading Project...");
						await Project.Manager.SetCurrent(ToString(list[0]), ToInt(list[1]));
					}
					break;
				case "/project/start":
					{
						if(Global.CurrentProject != null)
						{
							Global.CurrentProject.Start();
						}
					}
					break;
				case "/screen/start":
					{
						if(Global.CurrentProject != null)
						{
							var module = Global.CurrentProject.GetClientModule(ToString(list[0]));
							if(module != null)
							{
								module.Activate();
							}
						}
					}
					break;
				case "/project/stop":
					{
						Global.StopClientGui();
					}
					break;
				case "/group/set":
					{
						if(Global.CurrentProject != null)
						{
							Global.CurrentProject.GroupID = ToString(list[0]);
							Global.CurrentProject.StartupScreen = ToString(list[1]);
						}
						break;
					}
				default:
					{
						Sender.WriteLog("Client got invalid message: " + args.Message.ToString());
					}
					break;
			}
		}


		private static int ToInt(Osc.Values.IOscValue value)
		{
			return (value as Osc.Values.OscInt).Contents;
		}

		private static string ToString(Osc.Values.IOscValue value)
		{
			return (value as Osc.Values.OscString).Contents;
		}

		private static object[] ToObjectArray(List<Osc.Values.IOscValue> list)
		{
			object[] result = new object[list.Count];
			for(int i = 0; i < list.Count; i++)
			{
				switch(list[i].TypeTag)
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
