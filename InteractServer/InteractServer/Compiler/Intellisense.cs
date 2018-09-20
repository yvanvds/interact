using ActiproSoftware.Text.Languages.CSharp.Implementation;
using ActiproSoftware.Text.Languages.DotNet;
using ActiproSoftware.Text.Languages.DotNet.Reflection;
using ActiproSoftware.Text.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace InteractServer.Compiler
{
	public class Intellisense
	{
		public IProjectAssembly ServerAssembly;
		public IProjectAssembly ClientAssembly;
		public CSharpSyntaxLanguage ServerLanguage;
		public CSharpSyntaxLanguage ClientLanguage;

		public Intellisense()
		{
			ServerAssembly = new CSharpProjectAssembly("Project");
			var assemblyLoader = new BackgroundWorker();
			assemblyLoader.DoWork += DotNetProjectAssemblyReferenceLoader;
			assemblyLoader.RunWorkerAsync();

			ServerLanguage = new CSharpSyntaxLanguage();
			ServerLanguage.RegisterProjectAssembly(ServerAssembly);

			ClientAssembly = new CSharpProjectAssembly("Client");
			var clientAssemblyLoader = new BackgroundWorker();
			clientAssemblyLoader.DoWork += ClientAssemblyReferenceLoader;
			clientAssemblyLoader.RunWorkerAsync();

			ClientLanguage = new CSharpSyntaxLanguage();
			ClientLanguage.RegisterProjectAssembly(ClientAssembly);
		}

		private void DotNetProjectAssemblyReferenceLoader(object sender, DoWorkEventArgs e)
		{
			ServerAssembly.AssemblyReferences.AddMsCorLib();
			ServerAssembly.AssemblyReferences.Add("ServerInterface");
			ServerAssembly.AssemblyReferences.Add("netstandard.dll");
			ServerAssembly.AssemblyReferences.AddFrom(
				Path.Combine(
					Path.GetDirectoryName(
						System.Reflection.Assembly.GetExecutingAssembly().Location), 
					"ScriptInterface.dll"
					));
		}

		private void ClientAssemblyReferenceLoader(object sender, DoWorkEventArgs e)
		{
			ClientAssembly.AssemblyReferences.AddMsCorLib();
			ClientAssembly.AssemblyReferences.Add("ClientInterface");
		}

		public void Dispose()
		{
			ServerAssembly.AssemblyReferences.Clear();
			ClientAssembly.AssemblyReferences.Clear();
		}
	}
}
