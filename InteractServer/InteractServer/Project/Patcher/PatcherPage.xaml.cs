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
using static InteractServer.Managers.AudioManager;

namespace InteractServer.Pages
{
  /// <summary>
  /// Interaction logic for PatcherPage.xaml
  /// </summary>
  public partial class PatcherPage : Page
  {
    PatcherView view;

    public PatcherPage(PatcherView view)
    {
      InitializeComponent();
      this.view = view;

      AudioObject audioObject = Global.AudioManager.GetObject(view.Patcher);

      if(audioObject != null)
      {
        Yap.Handle = new Utils.YseHandler(audioObject.patchAudio);
        Yap.Focusable = true;
        Yap.OnChange += Editor_ContentChanged;
        Yap.Focus();
        Yap.Init();

        Yap.Load();
      }
    }

    public void Save()
    {
      view.Patcher.Content = Yap.Save();
    }

    public void DiscardChanges()
    {
      Yap.Clear();
      Yap.Load();
    }

    public void Clear()
    {
      Yap.Clear();
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
      AudioObject obj = Global.AudioManager.GetObject(view.Patcher);
      if(obj != null)
      {
        if(AudioToggle.IsChecked.HasValue)
        {
          obj.EnableAudio = (bool)AudioToggle.IsChecked;
        }
      }
    }

    private void PerformToggle_Click(object sender, RoutedEventArgs e)
    {

    }

  }
}
