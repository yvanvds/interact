using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using InteractClient.IO;

namespace InteractClient.Droid.FileSystem
{
	public class FileSystem : IFileSystem
	{
		Folder localStorage = null;
		public IFolder LocalStorage => localStorage;

		public FileSystem()
		{
			localStorage = new Folder(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
		}
	}
}