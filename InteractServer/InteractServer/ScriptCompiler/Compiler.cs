using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ScriptCompiler
{
	public class Compiler
	{
		PluginHost host = null;
		Scripts.Base handle = null;
		Sponsor<Scripts.IScript> objectFromAssembly = null;

		public string Errors()
		{
			return host.Errors();
		}

		public Compiler(EventHandler onLoad, EventHandler onUnload)
		{
			host = new PluginHost();
			host.PluginsLoaded += onLoad;
			host.PluginsUnloaded += onUnload;
		}

		public bool CreateAssembly(string[] files)
		{
			return host.CreateAssembly(files);
		}

		public bool HasScriptInterface()
		{
			try
			{
				return host.GetImplementation<Scripts.IScript>() != null;
			}catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}
			
		}

		public string Run(Scripts.ICommunicator communicator)
		{
			try
			{
				objectFromAssembly = host.GetImplementation<Scripts.IScript>();
				if (objectFromAssembly != null)
				{
					handle = objectFromAssembly.Instance as Scripts.Base;
					if (handle == null)
					{
						return "Unable to create script object";
					}
				}
				else
				{
					return "Unable to find Script interface";
				}

				if(!host.InjectCommunicator(communicator))
				{
					return "Unable to inject Communicator Object";
				}

				return OnCreate();
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		

		public void Stop()
		{
			if (objectFromAssembly != null)
			{
				objectFromAssembly.Dispose();
			}
		}

		public string OnCreate()
		{
			try
			{
				handle?.OnCreate();
			}
			catch(Exception e)
			{
				return e.Message;
			}
			return string.Empty;
		}

		public string OnProjectStart()
		{
			try
			{
				handle?.OnProjectStart();
			}
			catch (Exception e)
			{
				return e.Message;
			}
			return string.Empty;
		}

		public string OnProjectStop()
		{
			try
			{
				handle?.OnProjectStop();
			}
			catch (Exception e)
			{
				return e.Message;
			}
			return string.Empty;
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

		public object OscValueOverride(string methodName, object[] args)
		{
			/*Type type = assembly.GetType("Scripts.Overrides");
			var method = type.GetMethod(methodName);
			if (method != null)
			{
				try
				{
					return method.Invoke(null, args);
				}
				catch (Exception)
				{
					// 
				}
			}
			return args;*/
			return null;
		}
	}
}
