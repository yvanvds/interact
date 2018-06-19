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
  public partial class NewServerScriptDialog : MetroWindow, IDialog
  {
    private string modelName;
    string IDialog.ModelName => modelName;

    public string Type => "";

    public NewServerScriptDialog()
    {
      InitializeComponent();
      BContinue.IsEnabled = false;
      LFileExists.Visibility = Visibility.Hidden;
      modelName = "";
    }

    private void BContinue_Click(object sender, RoutedEventArgs e)
    {
      if (!ValidModel()) return;
      DialogResult = true;
      Close();
    }

    private void TBScriptName_TextChanged(object sender, TextChangedEventArgs e)
    {
      modelName = TBScriptName.Text.Trim();
      //content = Regex.Replace(content, @"[^a-zA-Z0-9 -]", "");
      //content = Utils.StringUtils.UppercaseWords(content);
      //content = Regex.Replace(content, @"\s+", "");

      BContinue.IsEnabled = ValidModel();
    }

    private bool ValidModel()
    {
      if (modelName.Length == 0) return false;

      if (Global.ProjectManager.Current.ServerScripts.NameExists(modelName))
      {
        LFileExists.Visibility = Visibility.Visible;
        return false;
      }
      LFileExists.Visibility = Visibility.Hidden;
      return true;
    }
  }
}
