using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptInterface
{
	public interface IOsc
	{
		void AddEndpoint(string name);

		void SendByID(string address, object[] values, bool OnGuiThread = false);
		void SendByID(string address, object value, bool OnGuiThread = false);

		void SendByName(string address, object[] values, bool OnGuiThread = false);
		void SendByName(string address, object value, bool OnGuiThread = false);

		void ToResolume(string address, object[] values);
	}
}
