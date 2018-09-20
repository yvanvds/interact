using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Compiler
{
	public class LogForwarder : MarshalByRefObject, ScriptInterface.ILog
	{
		public void AddEntry(string message)
		{
			Log.Log.Handle.AddEntry(message);
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
