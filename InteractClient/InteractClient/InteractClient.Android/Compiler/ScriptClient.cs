using System;
using System.Collections.Generic;
using System.Text;
using ScriptInterface;

namespace InteractClient.Compiler
{
	public class ScriptClient : MarshalByRefObject, IClient
	{
		private IOsc osc;
		public IOsc Osc => osc;

		private ILog log;
		public ILog Log => log;

		public ScriptClient(IOsc osc, ILog log)
		{
			this.osc = osc;
			this.log = log;
		}
	}
}
