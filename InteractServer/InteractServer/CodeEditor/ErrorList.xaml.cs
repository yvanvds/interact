using InteractServer.Project;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	public class ParseError
	{
		private int line;
		public int Line => line;

		private int column;
		public int Column => column;

		private string errorNumber = string.Empty;
		public string ErrorNumber => errorNumber;

		private string errorText = string.Empty;
		public string ErrorText => errorText;

		private string fileName = string.Empty;
		private string readableFilename = string.Empty;
		public string FileName => fileName;
		public string ReadableFilename => readableFilename;

		private bool isWarning;
		public bool IsWarning => isWarning;

		private string icon;
		public string Icon => icon;

		public ParseError(JObject obj)
		{
			line = (int)obj["line"];
			column = (int)obj["column"];
			errorNumber = (string)obj["errorNumber"];
			errorText = (string)obj["errorText"];
			fileName = (string)obj["fileName"];
			isWarning = (bool)obj["warning"];

			if(isWarning)
			{
				icon = @"/InteractServer;component/Resources/Icons/StatusWarning_16x.png";
			} else
			{
				icon = @"/InteractServer;component/Resources/Icons/StatusCriticalError_16x.png";
			}

			var name = System.IO.Path.GetFileNameWithoutExtension(fileName);
			readableFilename = Project.Project.Current?.ResourceName(name);
		}
	}

	/// <summary>
	/// Interaction logic for ErrorList.xaml
	/// </summary>
	public partial class ErrorList : Page
	{

		public static ErrorList Handle = null;
		private ObservableCollection<ParseError> Errors = new ObservableCollection<ParseError>();

		public ErrorList()
		{
			InitializeComponent();
			Handle = this;
			errorListView.ItemsSource = Errors;
		}

		public void Populate(string errors)
		{
			Errors.Clear();
			JArray arr = JArray.Parse(errors);
			foreach(JObject a in arr)
			{
				Errors.Add(new ParseError(a));
				Log.Log.Handle.AddEntry(Errors.Last().ErrorText);
			}
			
			if(Errors.Count > 0)
			{
				MainWindow.Handle.FocusOnErrorList();
			}
		}

		private void ErrorListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ListBox listBox = (ListBox)sender;
			ParseError error = listBox.SelectedItem as ParseError;

			if(error != null)
			{
				IResource resource = Project.Project.Current?.ServerModules.GetByName(error.ReadableFilename);
				if (resource == null)
				{
					resource = Project.Project.Current?.ClientModules.GetByName(error.ReadableFilename);
				}
				if (resource != null)
				{
					MainWindow.Handle.AddDocument(resource.Document);
					if(resource.Type == ContentType.ServerScript || resource.Type == ContentType.ClientScript)
					{
						var script = resource as Project.Script;
						script.View.SetFocus(error.Line, error.Column);
					}
				} 
			}
		}
	}
}
