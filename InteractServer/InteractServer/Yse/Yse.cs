using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace InteractServer.Yse
{
	public class Yse
	{
		public static Yse Handle = null;

		public YSE.YseInterface Interface;

		private DispatcherTimer update = new DispatcherTimer();

		public Yse()
		{
			Interface = new YSE.YseInterface(AddLogMessage);
			Interface.System.Init();
			Interface.Log.Level = IYse.ERROR_LEVEL.ERROR;
			Handle = this;

			Log.Log.Handle.AddEntry("Yse Version: " + Interface.System.Version);

			update.Interval = new TimeSpan(0, 0, 0, 0, 50);
			update.Tick += new EventHandler(UpdateFunc);
			update.Start();
		}

		public void UpdateFunc(object sender, EventArgs e)
		{
			Interface.System.Update();
		}

		public void AddLogMessage(string message)
		{
			Log.Log.Handle.AddEntry("Yse: " + message);
		}

		public void Dispose()
		{
			update.Stop();
			Interface.System.Close();
		}
	}
}
