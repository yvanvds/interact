using System;
using System.Collections.Generic;
using System.Text;

namespace Script.Resolume
{
	public class Clip : ISender
	{
		ISender parent;
		string name;

		public Clip(ISender parent, string name)
		{
			this.parent = parent;
			this.name = name;

			
		}

		public void Send(string route, object[] arguments)
		{
			parent.Send("/" + name + route, arguments);
		}

		public void Send(string route, object argument)
		{
			parent.Send("/" + name + route, argument);
		}

		public void Connect()
		{
			Send("/connect", 1f);
		}

		public void Preview()
		{
			Send("/preview", 1);
		}

		public void Position(float value)
		{
			Send("/video/position", value);
		}

		public void Direction(Direction value)
		{
			Send("/video/direction", (int)value);
		}

		public void Playmode(PlayMode value)
		{
			Send("/video/playmode", (int)value);
		}

		public void Speed(float value)
		{
			Send("/video/position/speed", value);
		}
	}
}
