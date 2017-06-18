﻿using InteractServer.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InteractServer.Dialogs
{
  /// <summary>
  /// Interaction logic for NewScreenDialog.xaml
  /// </summary>
  public partial class NewScreenDialog : Window
  {
    private String ModelName;

    private ObservableCollection<string> cbList;

    public NewScreenDialog()
    {
      InitializeComponent();
      BContinue.IsEnabled = false;
      LFileExists.Visibility = Visibility.Hidden;
      ModelName = "";

      cbList = new ObservableCollection<string>();
      cbList.Add("Scripted Screen");
      cbList.Add("Utility Script");
      cbList.Add("Info Screen");
      CBType.ItemsSource = cbList;
      CBType.SelectedIndex = 0;
    }

    private void BContinue_Click(object sender, RoutedEventArgs e)
    {
      if (!ValidModel()) return;

      String type = ScreenType.Invalid;

      switch(CBType.SelectedIndex)
      {
        case 0: { type = ScreenType.Script; break; }
        case 1: { type = ScreenType.UtilityScript; break; }
        case 2: { type = ScreenType.Info; break; }
      }

      Global.ScreenManager.CreateNewScreen(ModelName, type);
      Close();
    }

    private void TBScreenName_TextChanged(object sender, TextChangedEventArgs e)
    {
      String content = ModelName = TBScreenName.Text;
      content = Regex.Replace(content, @"[^a-zA-Z0-9 -]", "");
      content = Utils.StringUtils.UppercaseWords(content);
      content = Regex.Replace(content, @"\s+", "");

      BContinue.IsEnabled = ValidModel();
    }

    private bool ValidModel()
    {
      if (ModelName.Length == 0) return false;

      if (Global.ProjectManager.Current.Screens.NameExists(ModelName))
      {
        LFileExists.Visibility = Visibility.Visible;
        return false;
      }
      LFileExists.Visibility = Visibility.Hidden;
      return true;
    }
  }
}
