using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptInterface.Resolume
{
	public class Audio
	{
		ISender parent;

		public Audio(ISender parent)
		{
			this.parent = parent;
		}

		public void Volume(float value)
		{
			parent.Send("/audio/volume/", value);
		}

		public void Pan(float value)
		{
			parent.Send("/audio/pan", value);
		}
	}
}
