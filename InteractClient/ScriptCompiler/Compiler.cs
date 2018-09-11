using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ScriptCompiler
{
	public class Compiler : MarshalByRefObject
	{
		CSharpCodeProvider provider;
		CompilerParameters parameters;
		Assembly assembly = null;
		ScriptInterface.Script handle = null;

		public Compiler()
		{
			provider = new CSharpCodeProvider();
			parameters = new CompilerParameters();

			parameters.GenerateInMemory = true;
			parameters.GenerateExecutable = false;
			parameters.IncludeDebugInformation = true;

			List<string> assemblies = new List<string>();
			//assemblies.Add("System.dll");
			//assemblies.Add("netstandard.dll");
			//assemblies.Add("ClientInterface.dll");

			//parameters.ReferencedAssemblies.AddRange(assemblies.ToArray());

		}

		public bool CreateAssembly(string[] files)
		{
			CompilerResults results = provider.CompileAssemblyFromSource(parameters, new string[] { "namespace Main { public class Test { public static void Test() { return 100; } } }" });
			if(results.Errors.HasErrors)
			{
				assembly = null;
				StringBuilder sb = new StringBuilder();

				foreach(CompilerError error in results.Errors)
				{
					sb.AppendLine(String.Format("Line {2} # Error ({0} # {1}", error.ErrorNumber, error.ErrorText, error.Line));
				}
				throw new InvalidOperationException(sb.ToString());
			}
			assembly = results.CompiledAssembly;
			return assembly != null;
		}

		public string Run(ClientInterface.IClient client)
		{
			try
			{
				Type type = assembly.GetType("Scripts.Main");
				var obj = Activator.CreateInstance(type);
				handle = (obj as ClientInterface.IScript);
				handle.Init(client);
				return string.Empty;

			} catch(Exception e)
			{
				return e.Message;
			}
		}

		public string InvokeOsc(string endpoint, object[] args)
		{
			try
			{
				handle.OnOsc(endpoint, args);
			} catch(Exception e)
			{
				return e.Message;
			}
			return string.Empty;
		}
	}
}
