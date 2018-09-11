using System;
using System.Collections.Generic;
using System.Text;

namespace ClientInterface
{
	public interface IClient
	{
		IOsc Osc { get; }
		ILog Log { get; }
	}
}
