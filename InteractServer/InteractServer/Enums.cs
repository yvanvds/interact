using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer
{
	public static class Constants
	{
		public const int UdpPort = 11012;
		public const int TcpPort = 11013;
		public const int MulticastPort = 33344;
		public const String MulticastAddress = "239.192.0.1";
	}

	public enum ContentType
	{
		Invalid,
		ServerGui,
		ServerScript,
		ServerPatcher,
		ServerSounds,
		ClientGui,
		ClientScript,
		ClientPatcher,
		ClientSounds,
		ClientSensors,
		ClientArduino,
	}

	public enum NetworkMessage
	{
		EndOfMessage, // sent by client and server to indicate end of message
		Acknowledge, // sent by server to acknowledge presence
	}
}
