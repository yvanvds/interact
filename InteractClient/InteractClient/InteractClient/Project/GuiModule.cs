using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Project
{
	public class GuiModule : BaseModule
	{
		private OSCGui_Forms.OscGuiView view;

		public override void LoadContent()
		{
			view.LoadJSON(Content);
		}

		public GuiModule()
		{
			view = new OSCGui_Forms.OscGuiView();
			Global.OscLocal.Add(view.OscTree);
		}

		public override void Activate()
		{
			Global.SetClientGui(view);
		}
	}
}
