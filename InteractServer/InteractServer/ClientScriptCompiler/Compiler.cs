using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace ClientScriptCompiler
{
	public class Compiler : MarshalByRefObject
	{
		CSharpCodeProvider provider;
		CompilerParameters parameters;
		Assembly assembly = null;
		ClientInterface.IScript handle = null;

		public Compiler()
		{
			provider = new CSharpCodeProvider();
			parameters = new CompilerParameters();

			parameters.GenerateInMemory = true;
			parameters.GenerateExecutable = false;

			List<string> assemblies = new List<string>();
			assemblies.Add("System.dll");
			assemblies.Add("netstandard.dll");
			assemblies.Add("ClientInterface.dll");

			parameters.ReferencedAssemblies.AddRange(assemblies.ToArray());
		}

		public bool CreateAssembly(string[] files)
		{
			CompilerResults results = provider.CompileAssemblyFromFile(parameters, files);
			if (results.Errors.HasErrors)
			{
				assembly = null;
				StringBuilder sb = new StringBuilder();

				foreach (CompilerError error in results.Errors)
				{
					sb.AppendLine(String.Format("Line {2} # Error ({0}) # {1}", error.ErrorNumber, error.ErrorText, error.Line));
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
