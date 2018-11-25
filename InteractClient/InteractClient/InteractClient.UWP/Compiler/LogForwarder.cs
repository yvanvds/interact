using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Compiler
{
	public class LogForwarder : MarshalByRefObject, ScriptInterface.ILog
	{
		public void AddEntry(string message)
		{
			Network.Sender.WriteLog(message);
		}
	}
}
