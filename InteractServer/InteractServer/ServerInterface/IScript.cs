using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptInterface
{
	public interface IScript
	{
		void Init(IServer server);
		void OnOsc(string endPoint, object[] args);
	}
}
