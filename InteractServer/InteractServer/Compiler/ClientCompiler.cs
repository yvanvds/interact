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

		ScriptCompiler.Compiler compiler = null;
		ClientScriptObject client = null;
		OscForwarder oscForwarder = null;
		LogForwarder LogForwarder = new LogForwarder();

		public ClientCompiler()
		{
			compiler = new ScriptCompiler.Compiler(null, null);
		}

		public void Compile(string[] files)
		{
			try
			{
				bool result = compiler.CreateAssembly(files);
				if (result == false)
				{
					Log.Log.Handle.AddEntry("Client Scripts are not loaded");
				}
			}
			catch (Exception e)
			{
				Log.Log.Handle.AddEntry(e.Message);
			}
		}

		public void Run()
		{
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
			try
			{
				string result = compiler.InvokeOsc(endpoint, args);
				if (result != string.Empty)
				{
					Log.Log.Handle.AddEntry("Client Script Error: " + result);
				}
			}
			catch (System.Runtime.Remoting.RemotingException e)
			{
				Log.Log.Handle.AddEntry("Client Script InvokeOsc error: " + e.Message);
			}
		}

		public void StopAssembly()
		{
			compiler.Stop();
			if (oscForwarder != null)
			{
				oscForwarder.Clear();
			}
		}

		~ClientCompiler()
		{
			StopAssembly();
		}

		public void OnLoad(object sender, EventArgs e)
		{
			Log.Log.Handle.AddEntry("Client Scripts Loaded");
		}

		public void OnUnload(object sender, EventArgs e)
		{
			Log.Log.Handle.AddEntry("Client Scripts Unloaded");
		}
	}
}
