using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.CodeEditor
{
	public interface ICodeEditor
	{
		bool NeedsSaving { get; set; }
		string Text { get; set; }
		string Name { get; set; }

		void Save(string path);

		void InsertMethod(string line);
		void SetFocusOnNewCode();
	}
}
