using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.IO
{
	public interface IFolder
	{
		string Path { get; }
		string Name { get; }

		bool Exists();
		bool FolderExists(string name);
		Task<bool> FolderCreate(string name);
		Task FolderDelete(string name);
		Task<IFolder> GetFolder(string name, bool CreateIfNotExists = true);
		Task<List<IFolder>> GetFolders();

		bool FileExists(string name);
		Task FileDelete(string name);
		Task FileWrite(string name, string content);
		Task FileWrite(string name, byte[] content);
		Task<string> FileRead(string name);
	}
}
