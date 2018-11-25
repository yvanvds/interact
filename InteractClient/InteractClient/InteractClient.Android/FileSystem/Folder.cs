using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace InteractClient.Droid.FileSystem
{
	public class Folder : InteractClient.IO.IFolder
	{
		public string Path => info.FullName;

		public string Name => info.Name;

		private System.IO.DirectoryInfo info;

		public Folder(string path)
		{
			info = new System.IO.DirectoryInfo(path);
		}

		public Folder(System.IO.DirectoryInfo info)
		{
			this.info = info;
		}

		public bool Exists()
		{
			return info.Exists;
		}


		public bool FolderExists(string name)
		{
			var folderpath = System.IO.Path.Combine(Path, name);
			return System.IO.Directory.Exists(folderpath);
		}

		public bool FileExists(string name)
		{
			var filepath = System.IO.Path.Combine(Path, name);
			return System.IO.File.Exists(filepath);
		}

		public async Task FileDelete(string name)
		{
			if (!FileExists(name)) return;
			var filepath = System.IO.Path.Combine(Path, name);
			await Task.Run(() =>
			{
				System.IO.File.Delete(filepath);
			});
		}

		public async Task<bool> FolderCreate(string name)
		{
			if (FolderExists(name)) return true;
			var folderpath = System.IO.Path.Combine(Path, name);

			System.IO.DirectoryInfo info = null;
			await Task.Run(() => info = System.IO.Directory.CreateDirectory(folderpath));
			return info.Exists;
		}

		public async Task FolderDelete(string name)
		{
			if (!FolderExists(name)) return;

			var folderpath = System.IO.Path.Combine(Path, name);
			await Task.Run(() => System.IO.Directory.Delete(folderpath));
		}

		public async Task<IO.IFolder> GetFolder(string name, bool CreateIfNotExists = true)
		{
			if(!FolderExists(name))
			{
				if(CreateIfNotExists)
				{
					var result = await FolderCreate(name);
					if (!result) return null;
				} else
				{
					return null;
				}
			}

			return new Folder(System.IO.Path.Combine(Path, name));
		}

		public async Task<List<IO.IFolder>> GetFolders()
		{
			List<IO.IFolder> result = new List<IO.IFolder>();
			System.IO.DirectoryInfo[] list = null;

			await Task.Run(() =>
				list = info.GetDirectories()
			);

			foreach (var item in list)
			{
				result.Add(new Folder(item));
			}
			return result;
		} 

		public async Task FileWrite(string name, string content)
		{
			var filename = System.IO.Path.Combine(Path, name);
			await FileDelete(name);

			using (var writer = System.IO.File.CreateText(filename))
			{
				await writer.WriteAsync(content);
			}
		} 

		public async Task FileWrite(string name, byte[] content)
		{
			var filename = System.IO.Path.Combine(Path, name);
			await FileDelete(name);

			using (var write = System.IO.File.Create(filename))
			{
				await write.WriteAsync(content, 0, content.Length);
			}
		}

		public async Task<string> FileRead(string name)
		{
			if (!FileExists(name)) return string.Empty;

			var filename = System.IO.Path.Combine(Path, name);
			using (var reader = new System.IO.StreamReader(filename))
			{
				var content = await reader.ReadToEndAsync();
				return content;
			}
		}
	}
}