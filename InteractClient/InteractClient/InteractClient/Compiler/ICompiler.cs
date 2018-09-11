using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient
{
	public interface ICompiler
	{
		//void Compile(string[] files);
		void LoadAssembly(byte[] array);
		void Run();
		void InvokeOsc(string endpoint, object[] args);
		void StopAssembly();
	}
}
