using ActiproSoftware.Text;
using ActiproSoftware.Text.Languages.CSharp.Implementation;
using ActiproSoftware.Text.Languages.DotNet;
using ActiproSoftware.Text.Languages.DotNet.Reflection;
using ActiproSoftware.Text.Parsing;
using ActiproSoftware.Text.Parsing.LLParser;
using ActiproSoftware.Windows.Controls.SyntaxEditor;
using InteractServer.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace InteractServer.CodeEditor
{
	/// <summary>
	/// Interaction logic for CodeEditor.xaml
	/// </summary>
	public partial class CodeEditor : UserControl, ICodeEditor
	{
		private int documentNumber;
		private bool hasPendingParseData;
		private int newCodeLine = 0;

		public bool NeedsSaving { get; set; }

		// A project assembly (similar to a Visual Studio project) contains source files and assembly references for reflection
		private IProjectAssembly projectAssembly;

		public string Text
		{
			get => codeEditor.Text;
			set => codeEditor.Text = value;
		}

		public CodeEditor(string name)
		{
			InitializeComponent();

			Name = name;
			codeEditor.Document.TextChanged += Document_TextChanged;
			codeEditor.ZoomLevel = 1.25;
			codeEditor.Document.TabSize = 2;
			codeEditor.IsCurrentLineHighlightingEnabled = true;
		}

		public void SetLanguage(ISyntaxLanguage language)
		{
			codeEditor.Document.Language = language;
		}

		public void Save(string path)
		{
			codeEditor.Document.SaveFile(path, ActiproSoftware.Text.LineTerminator.CarriageReturnNewline);
		}

		private void Document_TextChanged(object sender, ActiproSoftware.Text.TextSnapshotChangedEventArgs e)
		{
			NeedsSaving = true;
		}

		/// <summary>
		/// Occurs when the document's parse data has changed.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The <c>EventArgs</c> that contains data related to this event.</param>
		private void OnCodeEditorDocumentParseDataChanged(object sender, EventArgs e)
		{
			//
			// NOTE: The parse data here is generated in a worker thread... this event handler is called 
			//         back in the UI thread immediately when the worker thread completes... it is best
			//         practice to delay UI updates until the end user stops typing... we will flag that
			//         there is a pending parse data change, which will be handled in the 
			//         UserInterfaceUpdate event
			//

			hasPendingParseData = true;
		}

		/// <summary>
		/// Occurs after a brief delay following any document text, parse data, or view selection update, allowing consumers to update the user interface during an idle period.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> that contains data related to this event.</param>
		private void OnCodeEditorUserInterfaceUpdate(object sender, RoutedEventArgs e)
		{
			// If there is a pending parse data change...
			if (hasPendingParseData)
			{
				// Clear flag
				hasPendingParseData = false;

				ILLParseData parseData = codeEditor.Document.ParseData as ILLParseData;
				if (parseData != null)
				{
					if (codeEditor.Document.CurrentSnapshot.Length < 10000)
					{
						// Show the AST
						//if (parseData.Ast != null)
						//	astOutputEditor.Document.SetText(parseData.Ast.ToTreeString(0));
						//else
							//astOutputEditor.Document.SetText(null);
					}
					//else
						//astOutputEditor.Document.SetText("(Not displaying large AST for performance reasons)");

					// Output errors
					//errorListView.ItemsSource = parseData.Errors;
				}
				else
				{
					// Clear UI
					//astOutputEditor.Document.SetText(null);
					//errorListView.ItemsSource = null;
				}
			}
		}

		/// <summary>
		/// Occurs when the document's view selection has changed.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The <see cref="EditorViewSelectionEventArgs"/> that contains data related to this event.</param>
		private void OnCodeEditorViewSelectionChanged(object sender, EditorViewSelectionEventArgs e)
		{
			// Quit if this event is not for the active view
			if (!e.View.IsActive)
				return;

			// Update line, col, and character display
			linePanel.Text = System.String.Format("Ln {0}", e.CaretPosition.DisplayLine);
			columnPanel.Text = System.String.Format("Col {0}", e.CaretDisplayCharacterColumn);
			characterPanel.Text = System.String.Format("Ch {0}", e.CaretPosition.DisplayCharacter);
		}

		/// <summary>
		/// Occurs when a mouse is double-clicked.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">A <see cref="MouseButtonEventArgs"/> that contains the event data.</param>
		private void OnErrorListViewDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ListBox listBox = (ListBox)sender;
			IParseError error = listBox.SelectedItem as IParseError;
			if (error != null)
			{
				codeEditor.ActiveView.Selection.StartPosition = error.PositionRange.StartPosition;
				codeEditor.Focus();
			}
		}

		public void InsertMethod(string line)
		{
			// find a good line
			int lastPos = Text.LastIndexOf('}');
			lastPos = Text.LastIndexOf('}', lastPos-1);
			lastPos = Text.LastIndexOf('\n', lastPos);

			var start = Text.Substring(0, lastPos);
			var end = Text.Substring(lastPos + 1);

			start += "\t\t" + line + '\n';
			start += "\t\t{\n";
			newCodeLine = start.Split('\n').Length;
			start += "\t\t\t\n";
			start += "\t\t}\n";
			start += "\n";
			start += end;
			Text = start;
			
		}

		public void SetFocusOnNewCode()
		{
			codeEditor.Caret.Position = new TextPosition(newCodeLine, 8);
			codeEditor.ActiveView.Scroller.ScrollToCaret();
			codeEditor.ActiveView.Focus();
		}
	}
}
