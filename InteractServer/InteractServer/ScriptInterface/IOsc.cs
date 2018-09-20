using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptInterface
{
	public interface IOsc
	{
		void AddEndpoint(string name);

		void SendByID(string address, object[] values);
		void SendByID(string address, object value);

		void SendByName(string address, object[] values);
		void SendByName(string address, object value);

		void ToResolume(string address, object[] values);
	}
}
