﻿using System;

namespace Scripts
{
	public interface ICommunicator
	{
		// OSC
		void AddOscEndpoint(string name);
		void SendOscByID(string address, object[] values, bool OnGuiThread);
		void SendOscByName(string address, object[] values, bool OnGuiThread);

		// Resolume
		void SendToResolume(string address, object[] values);

		// Log
		void AddLogEntry(string message);
	}

}
