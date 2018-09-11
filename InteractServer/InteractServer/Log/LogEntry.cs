using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InteractServer.Log
{
	public class LogEntry : PropertyChangedBase
	{
		public DateTime DateTime { get; set; }
		public string Message { get; set; }
	}

	public class PropertyChangedBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			Application.Current.Dispatcher.BeginInvoke((Action)(() =>
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}));
		}
	}
}
