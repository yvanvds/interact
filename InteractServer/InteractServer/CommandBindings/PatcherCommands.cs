using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InteractServer.CommandBindings
{
  public static class PatcherCommands
  {
    public static readonly RoutedUICommand AddObject = new RoutedUICommand
    (
      "Add Object",
      "AddObject",
      typeof(PatcherCommands),
      new InputGestureCollection()
      {
        new KeyGesture(Key.A, ModifierKeys.Control)
        
      }
    );

    public static readonly RoutedUICommand Perform = new RoutedUICommand
    (
      "Perform",
      "Perform",
      typeof(PatcherCommands),
      new InputGestureCollection()
      {
        new KeyGesture(Key.E, ModifierKeys.Control)
      }
    );
  }
}
