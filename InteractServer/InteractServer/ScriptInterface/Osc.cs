using System;
using System.Collections.Generic;
using System.Text;

namespace Scripts
{
	public static class Osc
	{
		private static Dictionary<string, Action<object[]>> actions = new Dictionary<string, Action<object[]>>();

		public static void AddEndpoint(string name, Action<object[]> action)
		{
			actions.Add(name, action);
			ServerBase.Com.AddOscEndpoint(name);
		}

		public static void OnOsc(string name, object[] args)
		{
			foreach(var action in actions)
			{
				if(action.Key.Equals(name))
				{
					action.Value.Invoke(args);
				}
			}
		}

		public static void SendByID(string address, object[] values, bool OnGuiThread = false)
		{
			ServerBase.Com.SendOscByID(address, values, OnGuiThread);
		}

		public static void SendByID(string address, object value, bool OnGuiThread = false)
		{
			ServerBase.Com.SendOscByID(address, new object[] { value }, OnGuiThread);
		}

		public static void SendByName(string address, object[] values, bool OnGuiThread = false)
		{
			ServerBase.Com.SendOscByName(address, values, OnGuiThread);
		}

		public static void SendByName(string address, object value, bool OnGuiThread = false)
		{
			ServerBase.Com.SendOscByName(address, new object[] { value }, OnGuiThread);
		}
	}
}
