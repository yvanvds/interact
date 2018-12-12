using System;

namespace Scripts
{

	public class Main : Base
	{

		public override void OnCreate()
		{
			// All scripts will be recompiled and run instantly whenever
			// you save your project. This method will be called after
			// each succesful compilation.

			Log.AddEntry("Server script OnCreate() called");

			// Pass your Osc Endpoints to the server here. (Or put them in a separate
			// class and call it here.)

			Osc.AddEndpoint("ToLog", (args) => {
				if (args.Length > 0)
				{
					float x = Convert.ToSingle(args[0]);
					x /= 100;
					Log.AddEntry("From Server Script: " + x.ToString());
				}
			});
		}

		public override void OnProjectStart()
		{
			// This method will be called whenever you press the play/run button
		}

		public override void OnProjectStop()
		{
			// This method will be called whenever you press the stop button
		}
	}
}
