﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptInterface
{
	public interface IScript
	{
		void OnProjectStart();
		void OnProjectStop();
	}

	public abstract class Script : MarshalByRefObject, IScript
	{
		private IServer server;
		public IServer Server => server;
		private IClient client;
		public IClient Client => client;

		public Script(IServer server)
		{
			this.server = server;
			Sound.Sound.Server = server;
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