using System;

namespace ScriptInterface
{
	public interface IServer
	{
		IOsc Osc { get; }
		ILog Log { get; }
	}

	public class FakeServer : IServer
	{
		public IOsc Osc => throw new NotImplementedException();

		public ILog Log => throw new NotImplementedException();
	}
}
