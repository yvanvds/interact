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
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace InteractServer.Windows
{
  /// <summary>
  /// Interaction logic for AboutWindow.xaml
  /// </summary>
  public partial class AboutWindow : MetroWindow
  {
    public AboutWindow()
    {
      InitializeComponent();
    }

    private void Interact_Click(object sender, RoutedEventArgs e)
    {
      System.Diagnostics.Process.Start("https://interact.mutecode.com");
    }

    private void Musica_Click(object sender, RoutedEventArgs e)
    {
      System.Diagnostics.Process.Start("http://www.musica.be");
    }
  }
}
