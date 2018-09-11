using System;

namespace ClientInterface
{
	public interface IScript
	{
		void Init(IClient client);
		void OnOsc(string endPoint, object[] args);
	}
}
