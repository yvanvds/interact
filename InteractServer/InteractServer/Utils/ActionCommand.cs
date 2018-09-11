using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InteractServer.Utils
{
	public class ActionCommand : ICommand
	{
		private readonly Action action;

		public ActionCommand(Action action)
		{
			this.action = action;
		}

		public void Execute(object parameter)
		{
			action();
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;
	}
}
