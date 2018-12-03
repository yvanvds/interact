using Newtonsoft.Json.Linq;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptCompiler
{
	public class ParseError
	{
		private int line;
		public int Line => line;

		private int column;
		public int Column => column;

		private string errorNumber = string.Empty;
		public string ErrorNumber;

		private string errorText = string.Empty;
		public string ErrorText => errorText;

		private string fileName = string.Empty;
		public string FileName => fileName;

		private bool isWarning;
		public bool IsWarning => isWarning;

		public ParseError(CompilerError error)
		{
			line = error.Line;
			column = error.Column;
			errorNumber = error.ErrorNumber;
			errorText = error.ErrorText;
			fileName = error.FileName;
			isWarning = error.IsWarning;
		}

		public JObject AsJson()
		{
			var result = new JObject();
			result["line"] = line;
			result["column"] = column;
			result["errorNumber"] = errorNumber;
			result["errorText"] = errorText;
			result["fileName"] = fileName;
			result["warning"] = isWarning;
			return result;
		}
	}
}
