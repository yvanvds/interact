using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptInterface.Resolume
{
	public class PlaybackController
	{
		ISender parent;

		public PlaybackController(ISender parent)
		{
			this.parent = parent;
		}

		public void Bpm(float value)
		{
			parent.Send("/playbackcontroller/bpm", value);
		}

		public void HalfTime()
		{
			parent.Send("/playbackcontroller/timingdividetwo", 1);
		}

		public void DoubleTime()
		{
			parent.Send("/playbackcontroller/timingmulttwo", 1);
		}

		public void Tap()
		{
			parent.Send("/playbackcontroller/tap", 1);
		}

		public void ReSync()
		{
			parent.Send("/playbackcontroller/resync", 1);
		}

		public void Paused(bool value)
		{
			parent.Send("/playbackcontroller/paused", value ? 1 : 0);
		}
	}
}
