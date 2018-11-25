using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.IO
{
	public static class FileSystem
	{
		private static IFileSystem current = null;
		public static IFileSystem Current { get => current; }

		public static void Init(IFileSystem current)
		{
			FileSystem.current = current;
		}
	}
}
