﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Scripts
{
	public static class Log
	{
		public static void AddEntry(string message)
		{
			Base.Com.AddLogEntry(message);
		}
	}
}