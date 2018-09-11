using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptInterface;

namespace InteractServer.Compiler
{
	class ClientScriptObject : MarshalByRefObject, ScriptInterface.IClient
	{
		private IOsc osc;
		public IOsc Osc => osc;

		private ILog log;
		public ILog Log => log;

		public ClientScriptObject(IOsc osc, ILog log)
		{
			this.osc = osc;
			this.log = log;
		}
	}
}
