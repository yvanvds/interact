using InteractClient.JintEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Network
{
	public class Receiver
	{
		private Osc.OscReceiver receiver;
		private bool started = false;

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
				} else if (address.StartsWith("/patcher"))
				{
					ParsePatcher(args);
				} else if (address.StartsWith("/image"))
				{
					ParseImage(args);
				} else if (address.StartsWith("/soundfile"))
				{
					ParseSoundfile(args);
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

					}
					break;

				case "/invoke":
					{
						System.Diagnostics.Debug.WriteLine("/action/invoke: " + args.Message.Arguments.ToString());
					}
					break;
			}
		}

		private void ParseClient(Osc.OSCMessageReceivedArgs args)
		{
			int index = args.Message.Address.ToString().IndexOf("/", 1);
			string address = args.Message.Address.ToString().Substring(index);

			switch (address)
			{
				case "/add":
					{
						System.Diagnostics.Debug.WriteLine("/client/add: " + args.Message.Arguments.ToString());
					}
					break;
			}
		}

		private void ParseProject(Osc.OSCMessageReceivedArgs args)
		{
			int index = args.Message.Address.ToString().IndexOf("/", 1);
			string address = args.Message.Address.ToString().Substring(index);
			List<Osc.Values.IOscValue> list = args.Message.Arguments;

			switch (address)
			{
				case "/stop":
					{
						Engine.Instance.StopRunningProject();
					}
					break;

				case "/set":
					{
						Data.Project.SetCurrent(ToGuid(list[0]), ToInt(list[1]));
						Engine.Instance.SetScreenMessage("Loading Project...", true);
					}
					break;

				case "/config":
					{
						Guid projectID = ToGuid(list[0]);
						list.RemoveAt(0);
						System.Diagnostics.Debug.WriteLine("/project/config: " + list.ToString());
						//Data.Project.List[projectID]?.SetConfig();
					}
					break;
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

				case "/set":
					{
						Data.Project.List[ToGuid(list[0])]?.UpdateScreen(ToGuid(list[1]), ToString(list[2]));
					}
					break;

				case "/version":
					{
						Guid projectID = ToGuid(list[0]);
						Data.Project.List[projectID]?.SetScreenVersion(projectID, ToGuid(list[1]), ToInt(list[2]));
						Engine.Instance.SetScreenMessage("Loading Screens...", true);
					}
					break;

				case "/start":
					{
						Engine.Instance.StartScreen(ToGuid(list[0]));
					}
					break;
			}
		}

		private void ParsePatcher(Osc.OSCMessageReceivedArgs args)
		{
			int index = args.Message.Address.ToString().IndexOf("/", 1);
			string address = args.Message.Address.ToString().Substring(index);
			List<Osc.Values.IOscValue> list = args.Message.Arguments;

			switch (address)
			{
				case "/version":
					{
						Guid projectID = ToGuid(list[0]);
						Data.Project.List[projectID]?.SetPatcherVersion(projectID, ToGuid(list[1]), ToInt(list[2]));
						Engine.Instance.SetScreenMessage("Loading Patchers...", true);
					}
					break;

				case "/set":
					{
						Data.Project.List[ToGuid(list[0])]?.UpdatePatcher(ToGuid(list[1]), ToString(list[2]));
					}
					break;
			}
		}

		private void ParseImage(Osc.OSCMessageReceivedArgs args)
		{
			int index = args.Message.Address.ToString().IndexOf("/", 1);
			string address = args.Message.Address.ToString().Substring(index);
			List<Osc.Values.IOscValue> list = args.Message.Arguments;

			switch (address)
			{
				case "/version":
					{
						Guid projectID = ToGuid(list[0]);
						Data.Project.List[projectID]?.SetImageVersion(projectID, ToGuid(list[1]), ToInt(list[2]));
						Engine.Instance.SetScreenMessage("Loading images...", true);
					}
					break;

				case "/set":
					{
						Data.Project.List[ToGuid(list[0])]?.UpdateImage(ToGuid(list[1]), ToString(list[2]));
					}
					break;
			}
		}

		private void ParseSoundfile(Osc.OSCMessageReceivedArgs args)
		{
			int index = args.Message.Address.ToString().IndexOf("/", 1);
			string address = args.Message.Address.ToString().Substring(index);
			List<Osc.Values.IOscValue> list = args.Message.Arguments;

			switch (address)
			{
				case "/version":
					{
						Guid projectID = ToGuid(list[0]);
						Data.Project.List[projectID]?.SetSoundFileVersion(projectID, ToGuid(list[1]), ToInt(list[2]));
						Engine.Instance.SetScreenMessage("Loading Sounds...", true);
					}
					break;

				case "/set":
					{
						Data.Project.List[ToGuid(list[0])]?.UpdateSoundFile(ToGuid(list[1]), ToString(list[2]));
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
