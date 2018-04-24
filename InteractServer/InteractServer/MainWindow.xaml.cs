using InteractServer.Models;
using InteractServer.Pages;
using MahApps.Metro.Controls;
using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : MetroWindow
  {

    private Timer networkTimer = new Timer();
    LayoutAnchorable logPane, errorLogPane;
    LayoutAnchorable clientPane;
    LayoutAnchorable projectExplorerPane;
    LayoutDocument projectConfigPane;
    LayoutAnchorable propertiesPane;

    DispatcherTimer UpdateYSE = new DispatcherTimer();

    public MainWindow()
    {
      InitializeComponent();

      Global.Init();
      Global.AppWindow = this;

      Global.Multicast.Start();
      //Global.Network.Start();
      Global.Sender.Start();

      networkTimer.Elapsed += new ElapsedEventHandler(OnNetworkTimerEvent);
      networkTimer.Interval = 10000;
      networkTimer.Enabled = true;

      AddLogPane();
      AddClientPane();
      AddPropertiesPane();
      AddProjectExplorerPane();
      AddProjectConfigPane();

      // set focus panes
      clientPane.IsActive = true;

      // start audio engine
      Global.Yse.System.Init();
      UpdateYSE.Interval = new System.TimeSpan(0, 0, 0, 0, 50);
      UpdateYSE.Tick += new System.EventHandler(UpdateAudio);
      UpdateYSE.Start();

      // open project?
      if(Properties.Settings.Default.OpenProjectOnStart)
      {
        if(Properties.Settings.Default.LastOpenProject.Length > 0)
        {
          Global.ProjectManager.OpenProject(Properties.Settings.Default.LastOpenProject);
        }
      }
    }

    private void UpdateAudio(object sender, EventArgs e)
    {
      Global.Yse.System.Update();
    }

    private static void OnNetworkTimerEvent(object source, ElapsedEventArgs e)
    {
      Global.Clients.Update();
      Global.Sender.RequestAcknowledge();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      UpdateYSE.Stop();
      Global.Yse.System.Close();
      CloseProject();
      Global.Multicast.Stop();
      Global.Sender.CloseConnection(); 
      networkTimer.Enabled = false;
    }


    //////////////////////////////////////
    // Project Ribbon Click Methods
    //////////////////////////////////////

    private void CloseProject()
    {
      if (Global.ProjectManager != null && Global.ProjectManager.Current != null && Global.ProjectManager.Current.Running)
      {
        Global.Sender.StopRunningProject();
        JintEngine.Runner.Stop();
        Global.ProjectManager.Current.Stop();
      }

      if (Global.ScreenManager.NeedsSaving() || Global.ProjectManager.Current != null && Global.ProjectManager.Current.Tainted())
      {
        var result = Messages.SaveCurrentProject();
        if (result.Equals(MessageBoxResult.Cancel))
        {
          return;
        }
        else if (result.Equals(MessageBoxResult.Yes))
        {
          SaveProject();
        }
      }

      // remove all open documents
      Global.ScreenManager.Clear();
      Global.ServerScriptManager.Clear();
    }

    private void SaveProject()
    {
      Global.ProjectManager.Current.Save();
      Global.ScreenManager.SaveAll();
      Global.ServerScriptManager.SaveAll();
    }


    //////////////////////////////////////
    // Screen Ribbon Click Methods
    //////////////////////////////////////

    private void ButtonCreateScreen_Click(object sender, RoutedEventArgs e)
    {
      if (Global.ProjectManager.Current == null)
      {
        Messages.NoOpenProject();
        return;
      }

      Global.ScreenManager.StartNewScreen();
    }

    private async void ButtonStartScreen_Click(object sender, RoutedEventArgs e)
    {
      if (Global.ProjectManager.Current == null)
      {
        Messages.NoOpenProject();
        return;
      }


      Global.ProjectManager.Current.Save();
      Global.ScreenManager.SaveAll();

      Global.Log.Clear();
      Global.ErrorLog.Clear();

      BasePage page = FocusDocument();
      if(page != null)
      {
        if(!page.ServerSide)
        {
          Screen screen = page.ScreenView.Screen;
          if (screen != null)
          {
            //ButtonStartScreen.IsEnabled = false;
            //ButtonStopScreen.IsEnabled = true;
            await Task.Run(() => Global.ProjectManager.Current.TestScreen(screen));
            return;
          }
        }
      }
      Messages.NoScreenSelected();
    }

    private void ButtonStopScreen_Click(object sender, RoutedEventArgs e)
    {
      if (Global.ProjectManager.Current == null)
      {
        Messages.NoOpenProject();
        return;
      }

      //ButtonStartScreen.IsEnabled = true;
      //ButtonStopScreen.IsEnabled = false;

      Global.Sender.StopCurrentScreen();
    }

    //////////////////////////////////////
    // View Ribbon Click Methods
    //////////////////////////////////////
    private void ButtonClientsView_Click(object sender, RoutedEventArgs e)
    {
      clientPane.IsVisible = !clientPane.IsVisible;
    }

    private void ButtonLogView_Click(object sender, RoutedEventArgs e)
    {
      logPane.IsVisible = !logPane.IsVisible;
    }

    private void ButtonExplorerView_Click(object sender, RoutedEventArgs e)
    {
      projectExplorerPane.IsVisible = !projectExplorerPane.IsVisible;
    }

    private void AddLogPane()
    {
      Frame frame = new Frame();
      frame.Content = Global.Log;
      logPane = new LayoutAnchorable();
      logPane.Title = "Event Log";
      logPane.Content = frame;
      dockBottom.Children.Add(logPane);

      Frame errorFrame = new Frame();
      errorFrame.Content = Global.ErrorLog;
      errorLogPane = new LayoutAnchorable();
      errorLogPane.Title = "Error Log";
      errorLogPane.Content = errorFrame;
      dockBottom.Children.Add(errorLogPane);
    }

    private void AddClientPane()
    {
      Frame frame = new Frame();
      frame.Content = Global.ClientPage;
      clientPane = new LayoutAnchorable();
      clientPane.Title = "Clients";
      clientPane.Content = frame;
      dockRight.Children.Add(clientPane);
    }

    private void AddPropertiesPane()
    {
      Frame frame = new Frame();
      frame.Content = Global.PropertiesPage;
      propertiesPane = new LayoutAnchorable();
      propertiesPane.Title = "Properties";
      propertiesPane.Content = frame;
      propertiesPane.IsActive = true;
      dockRight.Children.Add(propertiesPane);

    }

    private void AddProjectExplorerPane()
    {
      Frame frame = new Frame();
      frame.Content = Global.ProjectExplorerPage;
      projectExplorerPane = new LayoutAnchorable();
      projectExplorerPane.Title = "Project Explorer";
      projectExplorerPane.Content = frame;
      dockLeft.Children.Add(projectExplorerPane);
    }

    private void AddProjectConfigPane()
    {
      Frame frame = new Frame();
      frame.Content = Global.ProjectConfigPage;
      projectConfigPane = new LayoutDocument();
      projectConfigPane.Title = "Project Configuration";
      projectConfigPane.Content = frame;

    }

    public void AddDocument(LayoutDocument document)
    {
      int index = GetDocumentIndex(document);
      if (index == -1)
      {
        dockMain.Children.Add(document);
      }
      document.IsActive = true;
    }

    public void CloseDocument(LayoutDocument document)
    {
      //dockMain.Children.Remove(document);
      document.CanClose = false;
      document.CanFloat = false;
      dockMain.RemoveChild(document);
    }

    public BasePage FocusDocument()
    {
      if (dockMain.SelectedContentIndex >= 0 && dockMain.SelectedContentIndex < dockMain.Children.Count)
      {
        Frame f = dockMain.SelectedContent.Content as Frame;
        if (f.Content is BasePage)
        {
          return (f.Content as BasePage);
        }
      }

      return null;
    }


    public int GetDocumentIndex(LayoutDocument document) // returns -1 if the document is not found
    {
      for (int i = 0; i < dockMain.ChildrenCount; i++)
      {
        if (dockMain.Children[i] == document)
        {
          return i;
        }
      }

      return -1;
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
      
    }

#region Commands

    private void NewProject_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void NewProject_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
      CloseProject();
      Global.Log.Clear();
      Global.ErrorLog.Clear();
      Global.ProjectManager.StartNewProject();
    }

    private void OpenProject_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void OpenProject_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
      CloseProject();
      Global.Log.Clear();
      Global.ErrorLog.Clear();
      Global.ProjectManager.OpenProject();
    }

    private void SaveProject_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = Global.ProjectManager != null && Global.ProjectManager.Current != null;
    }

    private void SaveProject_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
      SaveProject();
    }

    private void Exit_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void Exit_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }

    private void AppOptions_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void AppOptions_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
      Windows.ServerOptionsWindow window = new Windows.ServerOptionsWindow();
      window.ShowDialog();
    }

    private void ProjectOptions_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = Global.ProjectManager != null && Global.ProjectManager.Current != null;
    }

    private void ProjectOptions_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
      dockMain.Children.Add(projectConfigPane);
      projectConfigPane.IsActive = true;
      Global.ProjectConfigPage.LinkProject(Global.ProjectManager.Current);
    }

    private void StartProject_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
    {
      if(Global.ProjectManager == null)
      {
        e.CanExecute = false;
        return;
      }

      if (Global.ProjectManager.Current == null)
      {
        e.CanExecute = false;
        return;
      }

      Screen screen = Global.ProjectManager.Current.Screens.Get(Global.ProjectManager.Current.Config.StartupScreen);
      if (screen == null)
      {
        e.CanExecute = false;
        return;
      }

      if(Global.ProjectManager.Current.Running)
      {
        e.CanExecute = false;
        return;
      }

      e.CanExecute = true;
    }

    private async void StartProject_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
      Global.ProjectManager.Current.Save();
      Global.ScreenManager.SaveAll();
      Global.ServerScriptManager.SaveAll();

      Global.Log.Clear();
      Global.ErrorLog.Clear();

      JintEngine.Runner.Start();

      await Task.Run(() => Global.ProjectManager.Current.Run());
    }

    private void StopProject_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
    {
      if(Global.ProjectManager == null || Global.ProjectManager.Current == null)
      {
        e.CanExecute = false;
        return;
      }

      e.CanExecute = Global.ProjectManager.Current.Running;
    }

    private void StopProject_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
      Global.Sender.StopRunningProject();
      JintEngine.Runner.Stop();
      Global.ProjectManager.Current.Stop();
    }

    #endregion

  }
}
