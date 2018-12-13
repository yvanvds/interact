using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Compiler
{
	public class Compiler : ICompiler
	{
		AppDomain domain = null;
		ProxyDomain proxy = null;
		ClientCommunicator communicator = null;

		public void LoadAssembly(byte[] array)
		{
            return;
			if (domain != null)
			{
				AppDomain.Unload(domain);
			}

			domain = AppDomain.CreateDomain("Scripts");
			Assembly a = domain.Load(Assembly.GetExecutingAssembly().FullName);
			proxy = (ProxyDomain)a.CreateInstance(typeof(ProxyDomain).FullName);
			proxy.LoadAssembly(array);
		}

		public void Run()
		{
            return;
			if (domain == null) return;
			if (proxy == null) return;


			if (communicator == null)
			{
				communicator = new ClientCommunicator();
			}
			Global.ClientScripts.Endpoints.Clear();

			try
			{
				var result = proxy.Run(communicator);
				if (result != string.Empty)
				{
					Network.Sender.WriteLog("Client Script error: " + result);
				}
			}
			catch (Exception e)
			{
				Network.Sender.WriteLog("Client Script Error: " + e.Message);
			}
			proxy.OnProjectStart();

		}

		public void InvokeOsc(string endpoint, object[] args)
		{
            return;
			if (domain == null) return;
			if (proxy == null) return;

			try
			{
				string result = proxy.InvokeOsc(endpoint, args);
				if (result != string.Empty)
				{
					Network.Sender.WriteLog("Client Script error: " + result);
				}
			}
			catch (Exception e)
			{
				Network.Sender.WriteLog("Client Script Error: " + e.Message);
			}

		}

		public void StopAssembly()
		{
            return;
			proxy?.OnProjectStop();
			if (domain != null)
			{
				try
				{
					AppDomain.Unload(domain);
				}
				catch (Exception) { }
			}
			domain = null;
			Global.ClientScripts.Endpoints.Clear();
		}

		~Compiler()
		{
			StopAssembly();
		}
	}

	class ProxyDomain : MarshalByRefObject
	{
		Assembly assembly;
		Scripts.Base handle = null;

		public void LoadAssembly(byte[] array)
		{
            return;
			assembly = null;
			try
			{
				assembly = Assembly.Load(array);
			}
			catch (Exception e)
			{
				throw new InvalidOperationException(e.Message);
			}
		}

		public string Run(Scripts.ICommunicator communicator)
		{
            return string.Empty;
			try
			{
				Type type = assembly.GetType("Scripts.Main");
				var obj = Activator.CreateInstance(type);
				handle = (obj as Scripts.Base);
				if (!InjectCommunicator(communicator))
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

		public string OnCreate()
		{
			/*try
			{
				handle?.OnCreate();
			}
			catch (Exception e)
			{
				return e.Message;
			}*/
			return string.Empty;
		}

		public string OnProjectStart()
		{
			/*try
			{
				handle?.OnProjectStart();
			}
			catch (Exception e)
			{
				return e.Message;
			}*/
			return string.Empty;
		}

		public string OnProjectStop()
		{
			/*try
			{
				handle?.OnProjectStop();
			}
			catch (Exception e)
			{
				return e.Message;
			}*/
			return string.Empty;
		}

		private bool InjectCommunicator(Scripts.ICommunicator communicator)
		{
			/*Type t = assembly.GetType("Scripts.Main");
			if (t != null)
			{
				Type b = t.BaseType;
				MethodInfo info = b.GetMethod("InjectCommunicator");
				info.Invoke(null, new object[] { communicator });
				return true;
			}*/
			return false;
		}

		public string InvokeOsc(string endpoint, object[] args)
		{
			/*try
			{
				handle.OnOsc(endpoint, args);
			}
			catch (Exception e)
			{
				return e.Message;
			}*/
			return string.Empty;
		}
	}
}
