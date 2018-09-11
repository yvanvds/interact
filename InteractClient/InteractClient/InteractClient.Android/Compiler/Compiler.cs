using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace InteractClient.Compiler
{
	public class Compiler: ICompiler
	{
		AppDomain domain = null;
		ProxyDomain proxy = null;
		ScriptClient client = null;
		OscForwarder oscForwarder = null;
		LogForwarder logForwarder = new LogForwarder();

		public void LoadAssembly(byte[] array)
		{
			if (domain != null)
			{
				AppDomain.Unload(domain);
			}

			domain = AppDomain.CreateDomain("Scripts");
			proxy = (ProxyDomain)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(ProxyDomain).FullName);
			proxy.LoadAssembly(array);
		}
		
		public void Run()
		{
			if (domain == null) return;
			if (proxy == null) return;

			if(client == null)
			{
				oscForwarder = new OscForwarder("ClientScripts");
				client = new ScriptClient(oscForwarder, logForwarder);
			} else
			{
				oscForwarder.Clear();
			}

			var result = proxy.Run(client);
			if(result != string.Empty)
			{
				Network.Sender.WriteLog("Script error: " + result);
			}
		}

		public void InvokeOsc(string endpoint, object[] args)
		{
			if (domain == null) return;
			if (proxy == null) return;

			string result = proxy.InvokeOsc(endpoint, args);
			if(result != string.Empty)
			{
				Network.Sender.WriteLog("Script error: " + result);
			}
		}

		public void StopAssembly()
		{
			if(domain != null)
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

		~Compiler()
		{
			StopAssembly();
		}
	}

	class ProxyDomain : MarshalByRefObject
	{
		Assembly assembly;
		ScriptInterface.Script handle = null;

		public void LoadAssembly(byte[] array)
		{
			assembly = null;
			try
			{
				assembly = Assembly.Load(array);
			}
			catch(Exception e)
			{
				throw new InvalidOperationException(e.Message);
			}
		}

		public string Run(ScriptInterface.IClient client)
		{
			try
			{
				Type type = assembly.GetType("Scripts.Main");
				var obj = Activator.CreateInstance(type, client);
				handle = (obj as ScriptInterface.Script);
				return string.Empty;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public string InvokeOsc(string endpoint, object[] args)
		{
			try
			{
				handle.OnOsc(endpoint, args);
			}
			catch (Exception e)
			{
				return e.Message;
			}
			return string.Empty;
		}
	}
}
