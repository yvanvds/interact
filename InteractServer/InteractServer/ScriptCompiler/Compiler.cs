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
		ScriptInterface.Script handle = null;
		Sponsor<ScriptInterface.IScript> objectFromAssembly = null;

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
				var server = new ScriptInterface.FakeServer();
				return host.GetImplementation<ScriptInterface.IScript>(new object[] { server }) != null;
			}catch(Exception)
			{
				return false;
			}
			
		}

		public string Run(ScriptInterface.IServer server)
		{
			try
			{
				objectFromAssembly = host.GetImplementation<ScriptInterface.IScript>(new object[] { server });
				if (objectFromAssembly != null)
				{
					handle = objectFromAssembly.Instance as ScriptInterface.Script;
					if (handle == null)
					{
						return "Unable to create script object";
					}
					else
					{
						return string.Empty;
					}
				}
				else
				{
					return "Unable to find Script interface";
				}


			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public string Run(ScriptInterface.IClient client)
		{
			try
			{
					objectFromAssembly = host.GetImplementation<ScriptInterface.IScript>(new object[] { client });
					if (objectFromAssembly != null)
					{
						handle = objectFromAssembly.Instance as ScriptInterface.Script;
						if (handle == null)
						{
							return "Unable to create script object";
						}
						else
						{
							return string.Empty;
						}
					}
					else
					{
						return "Unable to find Script interface";
					}
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
