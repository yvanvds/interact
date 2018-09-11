using System;

namespace ScriptInterface
{
	public interface IServer
	{
		IOsc Osc { get; }
		ILog Log { get; }
	}
}
