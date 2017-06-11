using InteractServer.Models;
using InteractServer.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.Views
{
    public class ScreenView
    {
        public ScreenView(Screen screen)
        {
            Screen = screen;
            Document = new LayoutDocument();
            Document.Content = new Frame() { Content = GeneratePageForScreen(screen) };
            Document.Title = screen.Name;
            Document.Closing += Document_Closing; 
        }

        private void Document_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Screen.Tainted)
            {
                var result = Messages.RequestScreenSave(Screen.Name);

                if (result.Equals(MessageBoxResult.Cancel)) {
                    e.Cancel = true;
                } else if(result.Equals(MessageBoxResult.Yes)) {
                    Save();
                    e.Cancel = false;
                } else if(result.Equals(MessageBoxResult.No))
                {
                    DiscardChanges();
                    e.Cancel = false;
                }
            }
        }

        public void DetachfromParent()
        {
            Document.Parent.RemoveChild(Document);
        }


        public void Save()
        {
            if(Screen.Tainted)
            {
                Frame f = Document.Content as Frame;
                BasePage sp = f.Content as BasePage;
                sp.Save();
                Global.ProjectManager.Current.Screens.Save(Screen);
                Screen.Tainted = false;
                Document.Title = Screen.Name;
            }
        }

        public void DiscardChanges()
        {
            if (Screen.Tainted)
            {
                Frame f = Document.Content as Frame;
                BasePage sp = f.Content as BasePage;
                sp.DiscardChanges();
                Screen.Tainted = false;
                Document.Title = Screen.Name;
            }
        }

        public void Taint()
        {
            if(!Screen.Tainted)
            {
                Screen.Tainted = true;
                Document.Title = Screen.Name + " *";
            }
        }

        public bool Tainted()
        {
            return Screen.Tainted;
        }

        public int ID => Screen.ID;

        public Screen Screen { get => screen; set => screen = value; }
        public LayoutDocument Document { get => document; set => document = value; }

        private BasePage GeneratePageForScreen(Screen screen)
        {
            if (screen.Type.Equals(ScreenType.Script)) return new ScriptPage(this);
            return null;
        }

        private Screen screen;
        private LayoutDocument document;


    }
}
