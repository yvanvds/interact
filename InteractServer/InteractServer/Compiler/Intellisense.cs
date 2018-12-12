using ActiproSoftware.Text;
using ActiproSoftware.Text.Implementation;
using ActiproSoftware.Text.Languages.CSharp.Implementation;
using ActiproSoftware.Text.Languages.DotNet;
using ActiproSoftware.Text.Languages.DotNet.Reflection;
using ActiproSoftware.Text.Tagging.Implementation;
using ActiproSoftware.Text.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
		public ISyntaxLanguage ClientLanguage;

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

			ServerLanguage.RegisterService(new CodeDocumentTaggerProvider<CodeEditor.Tagger>(typeof(CodeEditor.Tagger)));
			ClientLanguage.RegisterService(new CodeDocumentTaggerProvider<CodeEditor.Tagger>(typeof(CodeEditor.Tagger)));
		}

		private ISyntaxLanguage InitializeLanguage()
		{
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("InteractServer.Resources.Definitions.CSharp.langdef"))
			{
				if(stream != null)
				{
					SyntaxLanguageDefinitionSerializer serializer = new SyntaxLanguageDefinitionSerializer();
					return serializer.LoadFromStream(stream);
				} else
				{
					return SyntaxLanguage.PlainText;
				}
			}
		}

		private void DotNetProjectAssemblyReferenceLoader(object sender, DoWorkEventArgs e)
		{
			ServerAssembly.AssemblyReferences.AddMsCorLib();
			ServerAssembly.AssemblyReferences.Add("System.dll");
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
			ClientAssembly.AssemblyReferences.Add("System.dll");
            ClientAssembly.AssemblyReferences.Add("netstandard.dll");
            ClientAssembly.AssemblyReferences.AddFrom(
				Path.Combine(
					Path.GetDirectoryName(
						System.Reflection.Assembly.GetExecutingAssembly().Location),
					"ScriptInterface.dll"
					));
		}

		public void Dispose()
		{
			ServerAssembly.AssemblyReferences.Clear();
			ClientAssembly.AssemblyReferences.Clear();
		}
	}
}
