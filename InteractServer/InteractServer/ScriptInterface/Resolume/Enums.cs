using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptInterface.Resolume
{
	public enum AutoPilot
	{
		BackWard,
		Off,
		Forward,
		Random,
	}

	public enum Direction
	{
		Backward,
		Forward,
		Pause,
		Random,
	}

	public enum PlayMode
	{
		Loop,
		BackAndForth,
		Once,
		KeepLastFrame,
	}
}
