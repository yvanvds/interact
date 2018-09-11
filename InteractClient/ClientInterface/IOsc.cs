using System;
using System.Collections.Generic;
using System.Text;

namespace ClientInterface
{
	public interface IOsc
	{
		void AddEndpoint(string name);
		void Send(string address, object[] values);
		void Send(string address, object value);
	}
}
