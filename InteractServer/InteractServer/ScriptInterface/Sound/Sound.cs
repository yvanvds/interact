﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Scripts.Sound
{
	public class Sound
	{
		private string oscPath;
		

		public Sound(string oscPath)
		{
			this.oscPath = oscPath;
		}

		public void Play()
		{
			//Server.Osc.SendByName(oscPath + "/Play", true);
		}

		public void Stop()
		{
			//Server.Osc.SendByName(oscPath + "/Stop", true);
		}

		public void Pause()
		{
			//Server.Osc.SendByName(oscPath + "/Pause", true);
		}

		public void Time(float value)
		{
			//Server.Osc.SendByName(oscPath + "/Time", value);
		}

		public void Volume(float value)
		{
			//Server.Osc.SendByName(oscPath + "/Volume", value);
		}

		public void Speed(float value)
		{
			//Server.Osc.SendByName(oscPath + "/Speed", value);
		}

		public void Loop(bool value)
		{
			//Server.Osc.SendByName(oscPath + "/Loop", value);
		}

		public void Pos(float x, float y, float z)
		{
			//Server.Osc.SendByName(oscPath + "/Pos", new object[] { x, y, z });
		}
	}
}
