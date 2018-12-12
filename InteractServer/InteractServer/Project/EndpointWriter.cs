using ActiproSoftware.Text;
using ActiproSoftware.Text.Languages.CSharp.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.Project
{
	public class EndpointWriter
	{
		private string path;
		private bool serverside;

		public string Content;
		public CodeEditor.ICodeEditor View = null;

		private LayoutDocument document = null;
		public LayoutDocument Document => document;

		public EndpointWriter(string path, ISyntaxLanguage language, bool serverside)
		{
			this.path = path;
			this.serverside = serverside;

			if (!File.Exists(Path.Combine(path, "EndpointWriter.cs")))
			{
				string template;
				if (serverside)
				{
					template = "InteractServer.Resources.Definitions.ServerEndpointTemplate.cs";
				}
				else
				{
					template = "InteractServer.Resources.Definitions.ClientEndpointTemplate.cs";
				}

				using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(template))
				{
					if (stream != null)
					{
						StreamReader reader = new StreamReader(stream);
						Content = reader.ReadToEnd();
					}
				}

				File.WriteAllText(Path.Combine(path, "EndpointWriter.cs"), Content);
			}

			else
			{
				Content = File.ReadAllText(Path.Combine(path, "EndpointWriter.cs"));
			}

#if(WithSyntaxEditor)
			View = new CodeEditor.CodeEditor(serverside ? "ServerEndpointCode" : "ClientEndpointCode");
#else
			View = new CodeEditor.FallbackEditor(serverside ? "ServerEndpointCode" : "ClientEndpointCode");
#endif
			View.Text = Content;
			Frame frame = new Frame();
			frame.Content = View;

			document = new LayoutDocument();
			document.Title = serverside ? "ServerEndpointCode" : "ClientEndpointCode";
			document.Content = frame;

#if(WithSyntaxEditor)
			(View as CodeEditor.CodeEditor).SetLanguage(language);
#endif
		}

		public void PrepareForView()
		{
			(document.Content as Frame).Content = View;
		}

		public bool Save()
		{
			try
			{
				Content = View.Text;
				View.Save(Path.Combine(path, "EndpointWriter.cs"));
				return true;
			} catch(Exception e)
			{
				Dialogs.Error.Show("Error on " + e.TargetSite, e.Message);
				return false;
			}
		}

		public void DiscardChanges()
		{
			View.Text = Content;
		}
	}
}
