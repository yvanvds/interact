using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InteractServer
{
  public static class Messages
  {
    public static void NoOpenProject()
    {
      MessageBox.Show("You need to open a project first.", "Can't do this");
    }

    public static void NoScreenSelected()
    {
      MessageBox.Show("Please select a screen to test.", "No Screen Selected");
    }

    public static void NoStartupScreenSet()
    {
      MessageBox.Show("Please ensure the project has a valid startup screen.", "No Startup Screen Set");
    }

    public static void DatabaseCorrupted()
    {
      MessageBox.Show("There is something very wrong with the database!", "Unable to continue");
    }

    public static MessageBoxResult RequestScreenSave(string name)
    {
      return MessageBox.Show("Save changes to " + name + "?", "Screen Is not Saved", MessageBoxButton.YesNoCancel);
    }

    public static MessageBoxResult RequestScriptSave(string name)
    {
      return MessageBox.Show("Save changes to " + name + "?", "Script Is not Saved", MessageBoxButton.YesNoCancel);
    }

    public static MessageBoxResult SaveCurrentProject()
    {
      return MessageBox.Show("Save the current project?", "Project contains changes", MessageBoxButton.YesNoCancel);
    }
  }
}
