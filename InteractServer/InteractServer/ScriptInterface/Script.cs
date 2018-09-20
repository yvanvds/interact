using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptInterface
{
	public abstract class Script
	{
		private IServer server;
		public IServer Server => server;
		private IClient client;
		public IClient Client => client;

		public Script(IServer server)
		{
			this.server = server;
		}

		public Script(IClient client)
		{
			this.client = client;
		}


		public abstract void OnOsc(string endPoint, object[] args);
		public abstract void OnProjectStart();
		public abstract void OnProjectStop();
	}
}
