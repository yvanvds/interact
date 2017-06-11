using InteractServer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InteractServer.Pages
{
  public abstract class BasePage : Page
  {
    public ScreenView ScreenView;
    public ServerScriptView ServerScriptView;
    public bool ServerSide = false;

    public BasePage(ScreenView view)
    {
      ScreenView = view;
      ServerSide = false;
    }

    public BasePage(ServerScriptView view)
    {
      ServerScriptView = view;
      ServerSide = true;
    }

    public abstract void Save();
    public abstract void DiscardChanges();
  }
}
