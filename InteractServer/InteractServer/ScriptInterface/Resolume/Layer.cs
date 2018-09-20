using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptInterface.Resolume
{
	public class Layer : ISender
	{
		ISender parent;
		string name;

		Audio audio;
		public Audio Audio => audio;

		Video video;
		public Video Video => video;

		private Dictionary<string, Clip> clip = new Dictionary<string, Clip>();
		public Dictionary<string, Clip> Clip => clip;

		public Layer(ISender parent, string name)
		{
			this.parent = parent;
			this.name = name;

			audio = new Audio(this);
			video = new Video(this);
		}

		public void Send(string route, object[] arguments)
		{
			parent.Send("/" + name + route, arguments);
		}

		public void Send(string route, object argument)
		{
			parent.Send("/" + name + route, argument);
		}

		public void AddClip(string name)
		{
			clip.Add(name, new Clip(this, name));
		}

		public void AddClip(string myName, string resolumeName)
		{
			clip.Add(myName, new Clip(this, resolumeName));
		}

		public void Select()
		{
			Send("/select", 1);
		}

		public void Clear()
		{
			Send("/clear", 1);
		}

		public void ByPass(bool value)
		{
			Send("/bypassed", value ? 1 : 0);
		}

		public void Solo(bool value)
		{
			Send("/solo", value ? 1 : 0);
		}

		public void MoveUp()
		{
			Send("/moveup", 1);
		}

		public void MoveDown()
		{
			Send("/movedown", 1);
		}

		public void Group(int value)
		{
			Send("/group", value);
		}

		public void OpacityAndVolume(float value)
		{
			Send("/opacityandvolume", value);
		}

		public void TransitionTime(float value)
		{
			Send("/transitiontime", value);
		}

		public void AutoPilot(AutoPilot value)
		{
			Send("/autopilot/action", (int)value);
		}
	}
}
