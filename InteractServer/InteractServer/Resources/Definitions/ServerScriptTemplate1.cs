using System;
using ScriptInterface;

namespace Scripts
{

	public class Main : Script
	{

		// Handle to reach the main script object from within other classes
		public static Main Handle;

		public Main(IServer server) : base(server)
		{
			Handle = this;
			Server.Log.AddEntry("Server Scripts are Running");

			// Pass your Osc Endpoints to the server here. They must be handled 
			// in the method 'OnOsc' below.
			Server.Osc.AddEndpoint("ToLog");
		}


		// Osc Handler. Do not delete.
		public override void OnOsc(string endpoint, object[] args)
		{
			switch (endpoint)
			{
				// Handle the Osc Endpoint 'ToLog'
				case "ToLog":
					if (args.Length > 0)
					{
						float x = Convert.ToSingle(args[0]);
						x /= 100;
						Server.Log.AddEntry("From Server Script: " + x.ToString());

						// Send the result to another destination
						// Server.Osc.Send("/Root/Server/TestGui/Slider1/Value", x);
					}
					break;
			}
		}
	}

}