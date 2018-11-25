using ScriptInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Compiler
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
