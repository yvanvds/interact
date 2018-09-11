using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

			List<string> assemblies = new List<string>();
			assemblies.Add("System.dll");
			assemblies.Add("netstandard.dll");
			assemblies.Add("ScriptInterface.dll");

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

		public string Run(ScriptInterface.IServer server)
		{
			try
			{
				Type type = assembly.GetType("Scripts.Main");
				var obj = Activator.CreateInstance(type, server);
				handle = (obj as ScriptInterface.Script);
				return string.Empty;

			} catch(Exception e)
			{
				return e.Message;
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
			} catch(Exception e)
			{
				return e.Message;
			}
			return string.Empty;
		}

		public object OscValueOverride(string methodName, object[] args)
		{
			Type type = assembly.GetType("Scripts.Overrides");
			var method = type.GetMethod(methodName);
			if(method != null)
			{
				try
				{
					return method.Invoke(null, args);
				} catch(Exception)
				{
					// 
				}
			}
			return args;
		}
	}
}
