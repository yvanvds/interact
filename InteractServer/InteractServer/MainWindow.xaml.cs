using InteractServer.Models;
using InteractServer.Network;
using InteractServer.Pages;
using InteractServer.Views;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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

namespace InteractServer
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    private Timer networkTimer = new Timer();
    LayoutAnchorable logPane, errorLogPane;
    LayoutAnchorable clientPane;
    LayoutAnchorable projectExplorerPane;
    LayoutDocument projectConfigPane;
    LayoutAnchorable propertiesPane;

    public MainWindow()
    {
      InitializeComponent();

      Global.Init();
      Global.AppWindow = this;

      Global.Multicast.Join();
      //Global.Network.Start();
      Global.NetworkService.Start();

      networkTimer.Elapsed += new ElapsedEventHandler(OnNetworkTimerEvent);
      networkTimer.Interval = 10000;
      networkTimer.Enabled = true;

      AddLogPane();
      AddClientPane();
      AddPropertiesPane();
      AddProjectExplorerPane();
      AddProjectConfigPane();
    }

    private static void OnNetworkTimerEvent(object source, ElapsedEventArgs e)
    {
      Global.Clients.Update();
      Global.NetworkService.RequestAcknowledge();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      CloseProject();
      Global.Multicast.Disconnect();
      networkTimer.Enabled = false;
    }

    //////////////////////////////////////
    // Project Ribbon Click Methods
    //////////////////////////////////////
    private void ButtonNewProject_Click(object sender, RoutedEventArgs e)
    {
      CloseProject();
      Global.Log.Clear();
      Global.ErrorLog.Clear();
      Global.ProjectManager.StartNewProject();
    }

    private void ButtonLoadProject_Click(object sender, RoutedEventArgs e)
    {
      CloseProject();
      Global.Log.Clear();
      Global.ErrorLog.Clear();

      Global.ProjectManager.OpenProject();
    }

    private void CloseProject()
    {

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
    }

    private void SaveProject()
    {
      Global.ProjectManager.Current.Save();
      Global.ScreenManager.SaveAll();
      Global.ServerScriptManager.SaveAll();
    }

    private void ButtonSaveProject_Click(object sender, RoutedEventArgs e)
    {
      SaveProject();
    }

    private void ButtonConfigProject_Click(object sender, RoutedEventArgs e)
    {
      if (Global.ProjectManager.Current == null)
      {
        Messages.NoOpenProject();
        return;
      }

      dockMain.Children.Add(projectConfigPane);
      projectConfigPane.IsActive = true;
      Global.ProjectConfigPage.LinkProject(Global.ProjectManager.Current);
    }

    private async void ButtonStartProject_Click(object sender, RoutedEventArgs e)
    {
      if (Global.ProjectManager.Current == null)
      {
        Messages.NoOpenProject();
        return;
      }

      Screen screen = Global.ProjectManager.Current.Screens.Get(Global.ProjectManager.Current.Config.StartupScreen);
      if (screen == null)
      {
        Messages.NoStartupScreenSet();
        return;
      }

      Global.ProjectManager.Current.Save();
      Global.ScreenManager.SaveAll();
      Global.ServerScriptManager.SaveAll();

      Global.Log.Clear();
      Global.ErrorLog.Clear();

      JintEngine.Runner.Start();

      await Task.Run(() => Global.ProjectManager.Current.Run());
      ButtonStartProject.IsEnabled = false;
      ButtonStopProject.IsEnabled = true;
      
    }

    private void ButtonStopProject_Click(object sender, RoutedEventArgs e)
    {
      if (Global.ProjectManager.Current == null)
      {
        Messages.NoOpenProject();
        return;
      }

      ButtonStartProject.IsEnabled = true;
      ButtonStopProject.IsEnabled = false;

      Global.NetworkService.StopCurrentScreen();
      JintEngine.Runner.Stop();
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
            ButtonStartScreen.IsEnabled = false;
            ButtonStopScreen.IsEnabled = true;
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

      ButtonStartScreen.IsEnabled = true;
      ButtonStopScreen.IsEnabled = false;

      Global.NetworkService.StopCurrentScreen();
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
      dockMain.Children.Remove(document);
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

    private void Ribbon_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }


  }
}
