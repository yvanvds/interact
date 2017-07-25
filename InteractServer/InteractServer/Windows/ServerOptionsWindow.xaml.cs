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

namespace InteractServer.Windows
{
  /// <summary>
  /// Interaction logic for ServerOptionsWindow.xaml
  /// </summary>
  public partial class ServerOptionsWindow : Window
  {
    public ServerOptionsWindow()
    {
      InitializeComponent();

      ServerNameText.Text = Properties.Settings.Default.ServerName;
      NetworkTokenText.Text = Properties.Settings.Default.NetworkToken;
    }

    private void ButtonOK_Click(object sender, RoutedEventArgs e)
    {
      Properties.Settings.Default.ServerName = ServerNameText.Text;
      Properties.Settings.Default.NetworkToken = NetworkTokenText.Text;
      Properties.Settings.Default.Save();
      Close();
    }

    private void ButtonCancel_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

  }
}
