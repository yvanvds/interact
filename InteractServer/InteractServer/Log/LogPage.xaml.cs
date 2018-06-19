using InteractServer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace InteractServer.Log
{
  /// <summary>
  /// Interaction logic for LogPage.xaml
  /// </summary>
  public partial class LogPage : Page
  {
    private bool autoScroll = true;

    public ObservableCollection<LogEntry> LogEntries { get; set; }


    public LogPage()
    {
      InitializeComponent();
      DataContext = LogEntries = new ObservableCollection<LogEntry>();
    }

    public void AddEntry(String message)
    {
      App.Current?.Dispatcher.Invoke((Action)delegate
      {
        LogEntries.Add(new LogEntry()
        {
          DateTime = DateTime.Now,
          Message = message,
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
  }





}
