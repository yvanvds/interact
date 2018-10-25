using System;
using System.Collections.Generic;
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

namespace InteractServer.CodeEditor
{
	/// <summary>
	/// Interaction logic for FallbackEditor.xaml
	/// </summary>
	public partial class FallbackEditor : UserControl, ICodeEditor
	{
		private int documentNumber;
		private bool hasPendingParseData;
		private int newCodeLine = 0;

		public bool NeedsSaving { get; set; }

		public string Text
		{
			get => codeEditor.Text;
			set => codeEditor.Text = value;
		}

		public FallbackEditor(string name)
		{
			InitializeComponent();

			Name = name;
		}

		private void codeEditor_TextChanged(object sender, TextChangedEventArgs e)
		{
			NeedsSaving = true;
		}

		public void Save(string path)
		{
			try
			{
				System.IO.File.WriteAllText(path, codeEditor.Text);
			} catch(Exception e)
			{
				var window = new Dialogs.Error("Error writing file to disk", e.Message);
				window.ShowDialog();
			}			
		}

		public void InsertMethod(string line) { }
		public void SetFocusOnNewCode() { }
	}
}
