using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptInterface
{
	public interface IOsc
	{
		void AddEndpoint(string name);
		void Send(string address, object[] values);
		void Send(string address, object value);
	}
}
