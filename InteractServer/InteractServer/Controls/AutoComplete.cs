using AutocompleteMenuNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InteractServer.Controls
{
  public class AutoComplete
  {
    public List<AutocompleteItem> items = new List<AutocompleteItem>();

    public void Build()
    {
      foreach (var item in keywords)
      {
        items.Add(new AutocompleteItem(item) { ImageIndex = 0 });
      }

      foreach (var item in snippets)
      {
        items.Add(new SnippetAutocompleteItem(item) { ImageIndex = 1 });
      }

      items.Add(new InsertSpaceSnippet(@"^(\w+)([=<>!:]+)(\w+)$"));
    }

    string[] keywords = { "break", "case", "catch", "class", "const", "continue", "debugger",
      "default", "delete", "do", "else", "export", "extends", "finally", "for", "function", "if",
      "import", "in", "instanceof", "new", "return", "super", "switch", "this", "throw", "try",
      "typeof", "var", "void", "while", "with", "yield", "enum", "implements", "interface",
      "let", "package", "private", "protected", "public", "static", "await", "abstract",
      "boolean", "byte", "char", "double", "final", "float", "goto", "int", "long", "native",
      "short", "synchronized", "throws", "transient", "volatile", "null", "true", "false" };

    string[] snippets = { "if(^)\n{\n}", "if(^)\n{\n}\nelse\n{\n}", "for(^;;)\n{\n}", "while(^)\n{\n}", "do${\n^}while();", "switch(^)\n{\n\tcase : break;\n}" };
  }

  class InsertSpaceSnippet : AutocompleteItem
  {
    string pattern;

    public InsertSpaceSnippet(string pattern)
        : base("")
    {
      this.pattern = pattern;
    }

    public InsertSpaceSnippet()
        : this(@"^(\d+)([a-zA-Z_]+)(\d*)$")
    {
    }

    public override CompareResult Compare(string fragmentText)
    {
      if (Regex.IsMatch(fragmentText, pattern))
      {
        Text = InsertSpaces(fragmentText);
        if (Text != fragmentText)
          return CompareResult.Visible;
      }
      return CompareResult.Hidden;
    }

    public string InsertSpaces(string fragment)
    {
      var m = Regex.Match(fragment, pattern);
      if (m.Groups[1].Value == "" && m.Groups[3].Value == "")
        return fragment;
      return (m.Groups[1].Value + " " + m.Groups[2].Value + " " + m.Groups[3].Value).Trim();
    }

    public override string ToolTipTitle
    {
      get
      {
        return Text;
      }
    }
  }
}