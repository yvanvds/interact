using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.IO
{
	public interface IFileSystem
	{
		IFolder LocalStorage { get; }
	}
}
