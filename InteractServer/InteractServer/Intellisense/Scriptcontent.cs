using AutocompleteMenuNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InteractServer.Intellisense
{
  public class Scriptcontent
  {
    private enum PARSE_STATE
    {
      NONE,
      FUNC,
      REG_OBJ,
      PARSE_OBJ,
    }
    PARSE_STATE state = PARSE_STATE.NONE;
  
    public enum SCRIPT_TYPE
    {
      NONE,
      SERVER,
      CLIENT,
      CLIENT_UTILITY,
    }
    public SCRIPT_TYPE Type { get; set; }

    public string Name { get; set; }

    public List<AutocompleteItem> Functions = new List<AutocompleteItem>();

    class ScriptObject
    {
      public int[] Position;
      public string ObjName;
      public string ObjType;
    }

    List<ScriptObject> Objects = new List<ScriptObject>();

    public void Parse(string content)
    {
      Functions.Clear();
      Objects.Clear();

      if (content == null) return;
      var parts = Regex.Split(content, @"([.,;\[\]\(\)\s])");

      List<int> currentPos = new List<int>();
      currentPos.Add(0);
      int level = 0;

      string name = "";
      string previousPart = "";
      ScriptObject ObjectToParse = null;

      state = PARSE_STATE.NONE;

      foreach (string p in parts)
      {
        if(p.Equals("{"))
        {
          level++;
          if(currentPos.Count -1 < level)
          {
            currentPos.Add(0);
          } else
          {
            currentPos[level]++;
          }
        } else if(p.Equals("}"))
        {
          level--;
        }

        else if (state == PARSE_STATE.NONE)
        {
          if (p.Equals("function"))
          {
            name = "";
            state = PARSE_STATE.FUNC;
          }
          else if (p.Equals("var"))
          {
            state = PARSE_STATE.REG_OBJ;
          }
          else if (p.Equals("="))
          {
            foreach(ScriptObject obj in Objects)
            {
              if(obj.ObjName.Equals(previousPart) && isReachable(obj.Position, currentPos.GetRange(0, level + 1).ToArray())) {
                ObjectToParse = obj;
                state = PARSE_STATE.PARSE_OBJ;
                break;
              }
            }
          }
        }
        else
        {
          switch (state)
          {
            case PARSE_STATE.FUNC:
              {
                if (!p.Equals(" "))
                {
                  name += p;
                }
                if (p.Equals(")"))
                {
                  Functions.Add(new AutocompleteItem(name));
                  state = PARSE_STATE.NONE;
                }
                break;
              }
            case PARSE_STATE.REG_OBJ:
              {
                if (p != " ")
                {
                  Objects.Add(new ScriptObject()
                  {
                    ObjName = p,
                    Position = currentPos.GetRange(0, level+1).ToArray()
                  });
                  state = PARSE_STATE.NONE;
                }
                break;
              }
            case PARSE_STATE.PARSE_OBJ:
              {
                if (!p.Equals(" ") && !p.Equals("new"))
                {
                  if(ObjectToParse != null) ObjectToParse.ObjType = p;
                  ObjectToParse = null;
                  state = PARSE_STATE.NONE;
                }
                break;
              }
          }
        }

        if (!p.Equals(" ")) previousPart = p;
      }
    }

    public void AddFunctions(List<AutocompleteItem> list)
    {
      list.AddRange(Functions);
    }

    public void AddObjects(List<AutocompleteItem> list, int[] braceLevel)
    {
      foreach(ScriptObject obj in Objects)
      {
        if(isReachable(obj.Position, braceLevel))
        {
          list.Add(new AutocompleteItem(obj.ObjName));
        }
      }
    }

    public string GetObjectType(string objName, int[] braceLevel)
    {
      foreach(ScriptObject obj in Objects)
      {
        if(obj.ObjName.Equals(objName))
        {
          if (isReachable(obj.Position, braceLevel)) return obj.ObjType;
        }
      }
      return null;
    }

    private bool isReachable(int[] objectPos, int[] currentPos)
    {
      for(int i = 0; i < currentPos.Length; i++)
      {
        if(i < objectPos.Length)
        {
          if(currentPos[i] != objectPos[i])
          {
            return false;
          }
        }
      }
      if (objectPos.Length > currentPos.Length) return false;
      return true;
    }
  }
}
