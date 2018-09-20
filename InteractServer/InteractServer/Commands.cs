using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InteractServer
{
	public static class Commands
	{
		public static readonly RoutedUICommand Exit = new RoutedUICommand
	 (
		 "Exit", "Exit", typeof(Commands),
		 new InputGestureCollection()
		 {
				new KeyGesture(Key.F4, ModifierKeys.Alt)
		 }
	 );

		public static readonly RoutedUICommand AppOptions = new RoutedUICommand
		(
			"Server Options", "SOptions", typeof(Commands),
			new InputGestureCollection()
			{
				new KeyGesture(Key.O, ModifierKeys.Alt)
			}
		);

		public static readonly RoutedUICommand ProjectStart = new RoutedUICommand
		(
			"Start Project", "Start", typeof(Commands),
			new InputGestureCollection()
			{
				new KeyGesture(Key.F5)
			}
		);

		public static readonly RoutedUICommand ProjectStop = new RoutedUICommand
		(
			"Stop Project", "Stop", typeof(Commands),
			new InputGestureCollection()
			{
				new KeyGesture(Key.F4)
			}
		);

		public static readonly RoutedUICommand ProjectOptions = new RoutedUICommand
		(
			"Project Options", "POptions", typeof(Commands)
		);

		public static readonly RoutedUICommand AddObject = new RoutedUICommand
		(
			"Add Object",
			"AddObject",
			typeof(Commands),
			new InputGestureCollection()
			{
				new KeyGesture(Key.A, ModifierKeys.Control)

			}
		);

		public static readonly RoutedUICommand Perform = new RoutedUICommand
		(
			"Perform",
			"Perform",
			typeof(Commands),
			new InputGestureCollection()
			{
				new KeyGesture(Key.E, ModifierKeys.Control)
			}
		);
	}
}
