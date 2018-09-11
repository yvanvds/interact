using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Compiler
{
	public class ServerCompiler
	{
		AppDomain domain = null;
		ScriptCompiler.Compiler compiler = null;
		ServerScriptObject server = null;
		OscForwarder oscForwarder = null;
		LogForwarder LogForwarder = new LogForwarder();

		public void Compile(string[] files)
		{
			if(domain != null)
			{
				AppDomain.Unload(domain);
			}

			domain = AppDomain.CreateDomain("ServerScripts");

			compiler = (ScriptCompiler.Compiler)domain.CreateInstanceFromAndUnwrap("ScriptCompiler.dll", "ScriptCompiler.Compiler");

			try
			{
				bool result = compiler.CreateAssembly(files);
				if(result == false)
				{
					AppDomain.Unload(domain);
					domain = null;
					compiler = null;
					Log.Log.Handle.AddEntry("Scripts are not loaded");
				}
			} catch(Exception e)
			{
				AppDomain.Unload(domain);
				domain = null;
				compiler = null;
				Log.Log.Handle.AddEntry(e.Message);
			}
		}

		public void Run()
		{
			if (domain == null) return;
			if (compiler == null) return;

			if(server == null)
			{
				oscForwarder = new OscForwarder("ServerScripts", true);
				server = new ServerScriptObject(oscForwarder, LogForwarder);
			} else
			{
				oscForwarder.Clear();
			}

			var result = compiler.Run(server);
			if(result != string.Empty)
			{
				Log.Log.Handle.AddEntry("Server Script Error: " + result);
			}
		}

		public void InvokeOsc(string endpoint, object[] args)
		{
			string result = compiler.InvokeOsc(endpoint, args);
			if(result != string.Empty)
			{
				Log.Log.Handle.AddEntry("Server Script Error: " + result);
			}
		}

		public object OscValueOverride(string methodName, object[] args)
		{
			return compiler.OscValueOverride(methodName, args);
		}

		public void StopAssembly()
		{
			if (domain != null)
			{
				try
				{
					AppDomain.Unload(domain);
				}
				catch (Exception) { }
			}
			domain = null;
			if(oscForwarder != null)
			{
				oscForwarder.Clear();
			}
		}

		~ServerCompiler()
		{
			StopAssembly();
		}
	}
}
