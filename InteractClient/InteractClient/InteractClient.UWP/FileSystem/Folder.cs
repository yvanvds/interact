using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace InteractClient.UWP.FileSystem
{
	public class Folder : InteractClient.IO.IFolder
	{
		public string Path => folder.Path;

		public string Name => folder.Name;

		private StorageFolder folder;

		public Folder(StorageFolder folder)
		{
			this.folder = folder;
		}

		public bool Exists()
		{
			return true;
		}


		public bool FolderExists(string name)
		{
			return System.IO.Directory.Exists(System.IO.Path.Combine(folder.Path, name));
		}

		public bool FileExists(string name)
		{
			return System.IO.File.Exists(System.IO.Path.Combine(folder.Path, name));
		}

		public async Task FileDelete(string name)
		{
			if (!FileExists(name)) return;
			var file = await folder.GetFileAsync(name);
			await file.DeleteAsync();
		}

		public async Task<bool> FolderCreate(string name)
		{
			if (FolderExists(name)) return true;
			var result = await folder.CreateFolderAsync(name);
			return (result != null);
		}

		public async Task FolderDelete(string name)
		{
			if (!FolderExists(name)) return;
			var result = await folder.GetFolderAsync(name);
			await result.DeleteAsync();
		}

		public async Task<IO.IFolder> GetFolder(string name, bool CreateIfNotExists = true)
		{
			if (!FolderExists(name))
			{
				if (CreateIfNotExists)
				{
					var result = await FolderCreate(name);
					if (!result) return null;
				}
				else
				{
					return null;
				}
			}

			return new Folder(await folder.GetFolderAsync(name));
		}

		public async Task<List<IO.IFolder>> GetFolders()
		{
			List<IO.IFolder> result = new List<IO.IFolder>();
			var list = await folder.GetFoldersAsync();
			foreach (var item in list)
			{
				result.Add(new Folder(item));
			}
			return result;
		}

		public async Task FileWrite(string name, string content)
		{
			await FileDelete(name);

			var file = await folder.CreateFileAsync(name);
			await FileIO.WriteTextAsync(file, content);
		}

		public async Task FileWrite(string name, byte[] content)
		{
			await FileDelete(name);

			var file = await folder.CreateFileAsync(name);
			await FileIO.WriteBytesAsync(file, content);
		}

		public async Task<string> FileRead(string name)
		{
			if (!FileExists(name)) return string.Empty;

			var file = await folder.GetFileAsync(name);
			return await FileIO.ReadTextAsync(file);
		}
	}
}
