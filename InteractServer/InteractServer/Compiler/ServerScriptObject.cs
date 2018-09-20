using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptInterface;

namespace InteractServer.Compiler
{
	public class ServerScriptObject : MarshalByRefObject, IServer
	{
		private IOsc osc;
		public IOsc Osc => osc;

		private ILog log;
		public ILog Log => log;

		public ServerScriptObject(IOsc osc, ILog log)
		{
			this.osc = osc;
			this.log = log;
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
