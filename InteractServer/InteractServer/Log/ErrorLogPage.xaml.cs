using InteractServer.Controls;
using InteractServer.Log;
using InteractServer.Models;
using InteractServer.Project;
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
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.Log
{
  /// <summary>
  /// Interaction logic for ErrorLogPage.xaml
  /// </summary>
  public partial class ErrorLogPage : Page
  {
    private bool autoScroll = true;

    public ObservableCollection<ErrorLogEntry> LogEntries { get; set; }


    public ErrorLogPage()
    {
      InitializeComponent();
      DataContext = LogEntries = new ObservableCollection<ErrorLogEntry>();
    }

    public void AddEntry(int index, int line, String message, IDiskResource resource = null)
    {
      App.Current.Dispatcher.Invoke((Action)delegate
      {
        LogEntries.Add(new ErrorLogEntry()
        {
          Index = index,
          Line = line,
          Message = message,
          Resource = resource,
        });
      });
    }

    public void Clear()
    {
      App.Current.Dispatcher.Invoke((Action)delegate
      {
        LogEntries.Clear();
      });
      
    }

    private void ScrollViewer_Changed(object sender, ScrollChangedEventArgs e)
    {
      // User scroll event : set or unset autoscroll mode
      if (e.ExtentHeightChange == 0)
      {   // Content unchanged : user scroll event
        if ((e.Source as ScrollViewer).VerticalOffset == (e.Source as ScrollViewer).ScrollableHeight)
        {   // Scroll bar is in bottom
            // Set autoscroll mode
          autoScroll = true;
        }
        else
        {   // Scroll bar isn't in bottom
            // Unset autoscroll mode
          autoScroll = false;
        }
      }

      // Content scroll event : autoscroll eventually
      if (autoScroll && e.ExtentHeightChange != 0)
      {   // Content changed and autoscroll mode set
          // Autoscroll
        (e.Source as ScrollViewer).ScrollToVerticalOffset((e.Source as ScrollViewer).ExtentHeight);
      }
    }

    private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
      ErrorLogEntry entry = (sender as TextBlock).DataContext as ErrorLogEntry;
      
      if(entry.Resource is IContentModel)
      {
        Global.ViewManager.AddAndShow(entry.Resource as IContentModel);
        CodeEditor editor = Global.ViewManager.GetCodeEditor(entry.Resource as IContentModel);

        if (editor != null)
        {
          editor.HighlightLine(entry.Line, entry.Index);
        }
      }

    }
  }
}

