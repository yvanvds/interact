using InteractServer.Dialogs;
using Shared;
using InteractServer.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;
using InteractServer.Models;
using InteractServer.Views;

namespace InteractServer.Managers
{
    public class ScreenManager
    {
        Collection<ScreenView> List;

        public ScreenManager()
        {
            List = new Collection<ScreenView>();
        }

        public void StartNewScreen()
        {
            NewScreenDialog dlg = new NewScreenDialog();
            dlg.ShowDialog();
        }

        public void CreateNewScreen(String ScreenName, String type)
        {
            if(Global.ProjectManager.Current == null)
            {
                Messages.NoOpenProject();
                return;
            }

            // create a model and add it to the current project
            Screen screen = Global.ProjectManager.Current.Screens.CreateScreen(ScreenName, type);

            // show model on screen
            AddAndShow(screen);
        }

        public void AddAndShow(Screen screen)
        {
            foreach(ScreenView view in List)
            {
                if(view.ID == screen.ID)
                {
                    // document is already open, just give it focus
                    Global.AppWindow.AddDocument(view.Document);
                    return;
                }
            }

            // if we get here, a new documentwindow must be added
            ScreenView newView = new ScreenView(screen);
            List.Add(newView);
            Global.AppWindow.AddDocument(newView.Document);
        }

        public void Close(Screen screen)
        {
            foreach(ScreenView view in List)
            {
                if(view.ID == screen.ID)
                {
                    Global.AppWindow.CloseDocument(view.Document);
                    List.Remove(view);
                    return;
                }
            }
        }

        public void RefreshName(Screen screen)
        {
            foreach(ScreenView view in List)
            {
                if(view.ID == screen.ID)
                {
                    view.Document.Title = screen.Name;
                    return;
                }
            }
        }

        public void SaveAll()
        {
            foreach(ScreenView view in List)
            {
                view.Save();
            }         
        }

        public bool NeedsSaving()
        {
            foreach(ScreenView view in List)
            {
                if (view.Tainted()) return true;
            }
            return false;
        }

        public void Clear()
        {
            foreach(ScreenView view in List)
            {
                view.DetachfromParent();
            }
            List.Clear();
        }

    }
}
