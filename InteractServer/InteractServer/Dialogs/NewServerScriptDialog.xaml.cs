using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InteractServer.Dialogs
{
  /// <summary>
  /// Interaction logic for NewServerScriptDialog.xaml
  /// </summary>
  public partial class NewServerScriptDialog : MetroWindow
  {
    private String ModelName;

    public NewServerScriptDialog()
    {
      InitializeComponent();
      BContinue.IsEnabled = false;
      LFileExists.Visibility = Visibility.Hidden;
      ModelName = "";
    }

    private void BContinue_Click(object sender, RoutedEventArgs e)
    {
      if (!ValidModel()) return;

      Global.ServerScriptManager.CreateNewServerScript(ModelName);
      Close();
    }

    private void TBScriptName_TextChanged(object sender, TextChangedEventArgs e)
    {
      String content = ModelName = TBScriptName.Text;
      content = Regex.Replace(content, @"[^a-zA-Z0-9 -]", "");
      content = Utils.StringUtils.UppercaseWords(content);
      content = Regex.Replace(content, @"\s+", "");

      BContinue.IsEnabled = ValidModel();
    }

    private bool ValidModel()
    {
      if (ModelName.Length == 0) return false;

      if (Global.ProjectManager.Current.ServerScripts.NameExists(ModelName))
      {
        LFileExists.Visibility = Visibility.Visible;
        return false;
      }
      LFileExists.Visibility = Visibility.Hidden;
      return true;
    }
  }
}
