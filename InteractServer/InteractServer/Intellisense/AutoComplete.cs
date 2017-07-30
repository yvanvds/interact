using AutocompleteMenuNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections;
using ScintillaNET;

namespace InteractServer.Intellisense
{
  public class AutoComplete : IEnumerable<AutocompleteItem>
  {
    private bool ServerSide = false;
    private Scintilla TextArea;

    public string ScriptName { get; set; }

    public AutoComplete(Scintilla TextArea, bool ServerSide)
    {
      this.TextArea = TextArea;
      this.ServerSide = ServerSide;
    }

    public IEnumerable<AutocompleteItem> Build()
    {
      // gather positional info (between which braces is the cursor?)
      int[] braceLevel = BraceLevel('{');

      // build result list
      List<AutocompleteItem> result = new List<AutocompleteItem>();

      int start = TextArea.WordStartPosition(TextArea.CurrentPosition, true);
      char previousChar = (char)TextArea.GetCharAt(start - 1);
      string previousToken = TextArea.GetWordFromPosition(start - 2);

      // don't show anything if the previous character is a semicolon
      if (previousChar == ';') return result;

      // show type or object members
      if (TextArea.GetCharAt(TextArea.CurrentPosition - 1) == '.' || previousChar == '.')
      {
        // find first part of chain
        List<string> objParts = GetObjectParts(start);
        
        // get type
        string Type = ServerSide ? Global.IntelliServerScripts.GetObjectType(objParts.Last(), braceLevel, ScriptName) : Global.IntelliClientScripts.GetObjectType(objParts.Last(), braceLevel, ScriptName);

        // might be an engine-injected object?
        if(Type == null)
        {
          Type = ServerSide ? Global.ServerObjects.GetGlobalObjectType(objParts.Last()) : Global.ClientObjects.GetGlobalObjectType(objParts.Last());
        }

        if (Type != null)
        {
          objParts[objParts.Count - 1] = Type;

          if (ServerSide)
          {
            if (Global.ServerObjects.HasType(Type))
            {
              Global.ServerObjects.GetTypeMethods(objParts, result);
              return result;
            }
          } else
          {
            if (Global.ClientObjects.HasType(Type))
            {
              Global.ClientObjects.GetTypeMethods(objParts, result);
              return result;
            }
          }
        } else
        {
          // might be an enum
          if (ServerSide)
          {
            if (Global.ServerObjects.HasEnum(objParts.Last()))
            {
              Global.ServerObjects.GetEnumValues(objParts.Last(), result);
              return result;
            }
          } else
          {
            if(Global.ClientObjects.HasEnum(objParts.Last()))
            {
              Global.ClientObjects.GetEnumValues(objParts.Last(), result);
              return result;
            }
          }
        }
      }



      // show registered types
      if (previousChar == ' ')
      {
        if(TextArea.GetWordFromPosition(start - 2).Equals("new"))
        {
          if (ServerSide) Global.ServerObjects.AddTypes(result);
          else Global.ClientObjects.AddTypes(result);
          return result;
        }
      }

      // show registered types
      if (TextArea.GetWordFromPosition(TextArea.CurrentPosition - 2).Equals("new"))
      {
        if (ServerSide) Global.ServerObjects.AddTypes(result);
        else Global.ClientObjects.AddTypes(result);
        return result;
      }

      // check if between parentheses
      int parPos = GetParenthesesStartPos();
      if(parPos != 0)
      {
        // count comma's to know which argument this is
        int pos = TextArea.CurrentPosition;
        int currentArg = 0;
        int parentheses = 0;
        while(pos != parPos)
        {
          pos--;
          int c = TextArea.GetCharAt(pos);
          if(c.Equals(')'))
          {
            parentheses++;
          } else if(c.Equals('('))
          {
            parentheses--;
          }
          else if(c.Equals(','))
          {
            currentArg++;
          }
        }

        // find first part of chain
        List<string> objParts = GetObjectParts(parPos);

        // get type
        string Type = ServerSide ? Global.IntelliServerScripts.GetObjectType(objParts.Last(), braceLevel, ScriptName) : Global.IntelliClientScripts.GetObjectType(objParts.Last(), braceLevel, ScriptName);

        // might be an engine-injected object?
        if (Type == null)
        {
          Type = ServerSide ? Global.ServerObjects.GetGlobalObjectType(objParts.Last()) : Global.ClientObjects.GetGlobalObjectType(objParts.Last());
        }

        if (Type != null)
        {
          objParts[objParts.Count - 1] = Type;

          if (ServerSide)
          {
            if (Global.ServerObjects.HasType(Type))
            {
              Global.ServerObjects.GetMethodArgs(objParts, currentArg, result);
              return result;
            }
          }
          else
          {
            if (Global.ClientObjects.HasType(Type))
            {
              Global.ClientObjects.GetMethodArgs(objParts, currentArg, result);
              return result;
            }
          }
        }
      }


      // else show known script function and object names
      if(ServerSide)
      {
        Global.IntelliServerScripts.AddObjects(result, braceLevel, ScriptName);
        Global.IntelliServerScripts.Addfunctions(result, ScriptName);
      } else
      {
        Global.IntelliClientScripts.AddObjects(result, braceLevel, ScriptName);
        Global.IntelliClientScripts.Addfunctions(result, ScriptName);
      }

      return result;
    }

    private int[] BraceLevel(char braceType)
    {
      List<int> currentPos = new List<int>();
      currentPos.Add(0);
      int level = 0;

      int textPos = 0;
      while (textPos < TextArea.CurrentPosition)
      {
        if (TextArea.GetCharAt(textPos).Equals(braceType))
        {

          int endPos = TextArea.BraceMatch(textPos);

          if (endPos == -1)
          {
            // not a valid pair
            break;
          }
          else if (endPos < TextArea.CurrentPosition)
          {
            // if this brace is closed before the current position, we are not interested in its contents
            if (textPos < endPos) textPos = endPos;
            level++;
            if (currentPos.Count - 1 < level)
            {
              currentPos.Add(0);
            }
            else
            {
              currentPos[level]++;
            }
            level--;
          }
          else
          {
            textPos++;
            level++;
            if (currentPos.Count - 1 < level)
            {
              currentPos.Add(0);
            }
            else
            {
              currentPos[level]++;
            }
          }
        }
        else
        {
          textPos++;
        }
      }
      return currentPos.GetRange(0, level + 1).ToArray();
    }

    private List<string> GetObjectParts(int pos)
    {
      // find first part of chain
      int startPos = TextArea.WordStartPosition(pos - 2, true);
      List<string> objParts = new List<string>();
      while (true)
      {
        objParts.Add(TextArea.GetWordFromPosition(startPos));
        if (TextArea.GetCharAt(startPos - 1) == '.')
        {
          startPos = TextArea.WordStartPosition(startPos - 2, true);
        }
        else
        {
          break;
        }
      }
      return objParts;
    }

    public int GetParenthesesStartPos()
    {
      int pos = TextArea.CurrentPosition - 1;
      while(pos > 0)
      {
        if(TextArea.GetCharAt(pos).Equals('('))
        {
          int endPos = TextArea.BraceMatch(pos);
          if(endPos >= TextArea.CurrentPosition)
          {
            // this means we're inside this Parentheses block
            return pos;
          }
        }
        pos--;
      }

      // not within a Parenthese block
      return 0;
    }

    IEnumerator<AutocompleteItem> IEnumerable<AutocompleteItem>.GetEnumerator()
    {
      return Build().GetEnumerator();
    }

    public IEnumerator GetEnumerator()
    {
      return Build().GetEnumerator();
    }

    // keywords with more than 4 chars
    string[] keywords = { "continue", 
      "default", "delete", "export", "extends", "finally", "function", 
      "import", "instanceof", "return", "super", "switch", "throw", 
      "typeof", "while", "yield", "implements", "interface",
      "package", "private", "protected", "public", "static", "await", "abstract",
      "boolean",  "double", "final", "native",
      "synchronized", "throws", "transient", "volatile" };

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