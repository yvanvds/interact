using System;
using System.Collections.Generic;
using System.Text;

namespace Scripts
{
	public interface IScript
	{
		void OnCreate();
		void OnProjectStart();
		void OnProjectStop();
	}

	public abstract class ServerBase : MarshalByRefObject, IScript
	{
		private static ICommunicator com;
		public static ICommunicator Com => com;

		public static void InjectCommunicator(ICommunicator com)
		{
			ServerBase.com = com;
		}

		public void OnOsc(string endpoint, object[] args)
		{
			Osc.OnOsc(endpoint, args);
		}

		public abstract void OnCreate();
		public abstract void OnProjectStart();
		public abstract void OnProjectStop();
	}
}
