using System;
using ScriptInterface;

namespace Scripts
{

	public class Main : Script
	{

		public Main(IClient client) : base(client)
		{
			Client.Log.AddEntry("Client Scripts are Running");

			// Pass your Osc Endpoints to the server here. They must be handled 
			// in the method 'OnOsc' below.
			Client.Osc.AddEndpoint("ToLog");
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
						Client.Log.AddEntry("From Client Script: " + x.ToString());

						// Send the result to another destination
						// Client.Osc.Send("/Root/Server/TestGui/Slider1/Value", x);
					}
					break;
			}
		}
	}
}