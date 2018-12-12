using System;
using System.Collections.Generic;
using System.Text;

namespace Scripts
{
	public static class Client
	{
		public static int Count { get => Base.Com.ClientCount(); }

		public static bool IDExists(string ID)
		{
			return Base.Com.ClientIDExists(ID);
		}

		public static bool NameExists(string Name)
		{
			return Base.Com.ClientNameExists(Name);
		}

		public static string GetName(string ID)
		{
			return Base.Com.ClientName(ID);
		}

		public static string GetID(string Name)
		{
			return Base.Com.ClientID(Name);
		}

		public static string GetIP(string ID)
		{
			return Base.Com.ClientIP(ID);
		}
	}
}
