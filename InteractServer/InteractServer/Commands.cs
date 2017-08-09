using System.Windows.Input;


namespace InteractServer.Commands
{
  public static class CustomCommands
  {
    public static readonly RoutedUICommand Exit = new RoutedUICommand
    (
      "Exit", "Exit", typeof(CustomCommands),
      new InputGestureCollection()
      {
        new KeyGesture(Key.F4, ModifierKeys.Alt)
      }
    );

    public static readonly RoutedUICommand AppOptions = new RoutedUICommand
    (
      "Server Options", "SOptions", typeof(CustomCommands),
      new InputGestureCollection()
      {
        new KeyGesture(Key.O, ModifierKeys.Alt)
      }
    );

    public static readonly RoutedUICommand ProjectOptions = new RoutedUICommand
    (
      "Project Options", "POptions", typeof(CustomCommands),
      new InputGestureCollection()
      {
        new KeyGesture(Key.O, ModifierKeys.Control)
      }
    );

    public static readonly RoutedUICommand ProjectStart = new RoutedUICommand
    (
      "Start Project", "Start", typeof(CustomCommands),
      new InputGestureCollection()
      {
        new KeyGesture(Key.F5)
      }
    );

    public static readonly RoutedUICommand ProjectStop = new RoutedUICommand
    (
      "Stop Project", "Stop", typeof(CustomCommands),
      new InputGestureCollection()
      {
        new KeyGesture(Key.F4)
      }
    );
  }
}
