using InteractClient.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace InteractClient.UWP.FileSystem
{
	public class FileSystem : IFileSystem
	{
		Folder localStorage = null;
		public IFolder LocalStorage => localStorage;

		public FileSystem()
		{
			localStorage = new Folder(ApplicationData.Current.LocalFolder);
		}
	}
}
