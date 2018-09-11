using System;

namespace ClientScriptCompiler
{
	public class Compiler : MarshalByRefObject
	{
		CSharpCodeProvider provider;
		CompilerParameters parameters;
		Assembly assembly = null;
		ServerInterface.IScript handle = null;

		public Compiler()
		{
			provider = new CSharpCodeProvider();
			parameters = new CompilerParameters();

			parameters.GenerateInMemory = true;
			parameters.GenerateExecutable = false;

			List<string> assemblies = new List<string>();
			assemblies.Add("System.dll");
			assemblies.Add("netstandard.dll");
			assemblies.Add("ServerInterface.dll");

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

		public string Run(ServerInterface.IServer server)
		{
			try
			{
				Type type = assembly.GetType("Scripts.Main");
				var obj = Activator.CreateInstance(type);
				handle = (obj as ServerInterface.IScript);
				handle.Init(server);
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
