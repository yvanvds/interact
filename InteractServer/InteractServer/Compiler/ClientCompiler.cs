using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Compiler
{
	public class ClientCompiler
	{
		public static bool CreateAssemblyOnDisk(string[] code, string output)
		{
			var provider = new CSharpCodeProvider();
			var parameters = new CompilerParameters();

			parameters.GenerateInMemory = false;
			parameters.GenerateExecutable = false;
			parameters.OutputAssembly = output;

			List<string> assemblies = new List<string>();
			assemblies.Add("netstandard.dll");
			assemblies.Add("ScriptInterface.dll");

			parameters.ReferencedAssemblies.AddRange(assemblies.ToArray());

			CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
			if (results.Errors.HasErrors)
			{

				foreach (CompilerError error in results.Errors)
				{
					Log.Log.Handle.AddEntry(String.Format("Client Script Compiler: Line {2} # Error ({0}) # {1}", error.ErrorNumber, error.ErrorText, error.Line));
				}
				return false;
			}
			return true;
		}

		AppDomain domain = null;
		ScriptCompiler.Compiler compiler = null;
		ClientScriptObject client = null;
		OscForwarder oscForwarder = null;
		LogForwarder LogForwarder = new LogForwarder();

		public void Compile(string[] files)
		{
			if (domain != null)
			{
				AppDomain.Unload(domain);
			}

			domain = AppDomain.CreateDomain("ClientScripts");

			compiler = (ScriptCompiler.Compiler)domain.CreateInstanceFromAndUnwrap("ScriptCompiler.dll", "ScriptCompiler.Compiler");

			try
			{
				bool result = compiler.CreateAssembly(files);
				if (result == false)
				{
					AppDomain.Unload(domain);
					domain = null;
					compiler = null;
					Log.Log.Handle.AddEntry("Client Scripts are not loaded");
				}
			}
			catch (Exception e)
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

			if (client == null)
			{
				oscForwarder = new OscForwarder("ClientScripts", false);
				client = new ClientScriptObject(oscForwarder, LogForwarder);
			}
			else
			{
				oscForwarder.Clear();
			}

			var result = compiler.Run(client);
			if (result != string.Empty)
			{
				Log.Log.Handle.AddEntry("Client Script Error: " + result);
			}
		}

		public void InvokeOsc(string endpoint, object[] args)
		{
			string result = compiler.InvokeOsc(endpoint, args);
			if (result != string.Empty)
			{
				Log.Log.Handle.AddEntry("Client Script Error: " + result);
			}
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
			if (oscForwarder != null)
			{
				oscForwarder.Clear();
			}
		}

		~ClientCompiler()
		{
			StopAssembly();
		}
	}
}
