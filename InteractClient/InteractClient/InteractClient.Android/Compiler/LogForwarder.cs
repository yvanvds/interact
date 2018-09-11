using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Compiler
{
	public class LogForwarder : MarshalByRefObject, ScriptInterface.ILog
	{
		public void AddEntry(string message)
		{
			Network.Sender.WriteLog(message);
		}
	}
}
