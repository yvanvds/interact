using InteractServer.Views;
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

namespace InteractServer.Pages
{
  /// <summary>
  /// Interaction logic for PatcherPage.xaml
  /// </summary>
  public partial class PatcherPage : Page
  {
    IYse.ISound sound;
    IYse.IPatcher patcher;
    PatcherView view;

    public PatcherPage(PatcherView view)
    {
      InitializeComponent();
      this.view = view;

      sound = Global.Yse.NewSound();
      patcher = Global.Yse.NewPatcher();
      patcher.Create(1);

      Yap.Handle = new Utils.YseHandler(patcher);
      Yap.Focusable = true;
      Yap.OnChange += Editor_ContentChanged;
      Yap.Focus();
      Yap.Init();

      sound.Create(patcher);
      //sound.Play();

      Load();
    }

    public void Load()
    {
      Yap.Clear();
      if (view.Patcher.ContentObj.Content == null) return;
      if (view.Patcher.ContentObj.Content.Length > 0)
      {
        Yap.Load(view.Patcher.ContentObj.Content);
      }
    }

    public void Save()
    {
      view.Patcher.ContentObj.Content = Yap.Save();
    }

    public void DiscardChanges()
    {
      Yap.Clear();
      Yap.Load(view.Patcher.ContentObj.Content);
    }

    public void Close()
    {
      Yap.Clear();
      sound.Stop();
      //patcher.Dispose();
      //sound.Dispose();
    }

    private void Editor_ContentChanged(object sender, EventArgs e)
    {
      view.Taint();
    }

    private void AddObjectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void AddObjectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      Yap.AddObject(true);
    }

    private void PerformCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void PerformCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      Yap.Deselect();
      YapView.Interface.PerformanceMode = !YapView.Interface.PerformanceMode;
    }

    private void ToggleSwitch_Click(object sender, RoutedEventArgs e)
    {
      if((bool)AudioToggle.IsChecked)
      {
        sound.Play();
      } else
      {
        sound.Stop();
      }
    }

    private void PerformToggle_Click(object sender, RoutedEventArgs e)
    {

    }
  }
}
