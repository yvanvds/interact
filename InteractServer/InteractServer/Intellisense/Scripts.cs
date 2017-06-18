using AutocompleteMenuNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Intellisense
{
  public class Scripts
  {
    private List<Scriptcontent> scripts = new List<Scriptcontent>();

    public void AddScript(string name, string content, Scriptcontent.SCRIPT_TYPE type)
    {

      foreach (Scriptcontent s in scripts)
      {
        if(s.Name.Equals(name))
        {
          s.Parse(content);
          return;
        }
      }

      // add new
      Scriptcontent script = new Scriptcontent();
      script.Name = name;
      script.Type = type;
      script.Parse(content);
      scripts.Add(script);
    }

    public void RemoveScript(string name)
    {
      foreach (Scriptcontent s in scripts)
      {
        if(s.Name.Equals(name))
        {
          scripts.Remove(s);
          return;
        }
      }
    }

    public void RenameScript(string oldName, string newName)
    {
      foreach(Scriptcontent s in scripts)
      {
        if(s.Name.Equals(oldName))
        {
          s.Name = newName;
          return;
        }
      }
    }

    public void ClearAll()
    {
      scripts.Clear();
    }

    public void Addfunctions(List<AutocompleteItem> list, string scriptName)
    {
      foreach(Scriptcontent content in scripts)
      {
        // Take only functions from the client script with the same name as 
        // the one who requests it. Other types of scripts (utility and server)
        // are also accessible from other scripts.

        if (content.Type == Scriptcontent.SCRIPT_TYPE.CLIENT)
        {
          if (content.Name.Equals(scriptName)) content.AddFunctions(list);
        } else
        {
          content.AddFunctions(list);
        }
        
      }
    }

    public string GetObjectType(string objName, int[] braceLevel, string scriptName)
    {
      string result = null;
      foreach (Scriptcontent content in scripts)
      {
        switch(content.Type)
        {
          case Scriptcontent.SCRIPT_TYPE.CLIENT:
            {
              if(content.Name.Equals(scriptName))
              {
                result = content.GetObjectType(objName, braceLevel);
                if (result != null) return result;
              }
              break;
            }

          case Scriptcontent.SCRIPT_TYPE.SERVER:
          case Scriptcontent.SCRIPT_TYPE.CLIENT_UTILITY:
            {
              result = content.GetObjectType(objName, scriptName.Equals(content.Name) ? braceLevel : new int[] { 0 });
              if (result != null) return result;
              break;
            }
        }
        
      }
      return result;
    }

    public void AddObjects(List<AutocompleteItem> list, int[] braceLevel, string scriptName)
    {
      foreach(Scriptcontent content in scripts)
      {
        switch(content.Type)
        {
          case Scriptcontent.SCRIPT_TYPE.CLIENT:
            {
              if(content.Name.Equals(scriptName))
              {
                content.AddObjects(list, braceLevel);
              }
              break;
            }
          case Scriptcontent.SCRIPT_TYPE.SERVER:
          case Scriptcontent.SCRIPT_TYPE.CLIENT_UTILITY:
            {
              content.AddObjects(list, scriptName.Equals(content.Name) ? braceLevel : new int[] { 0 });
              break;
            }
        }
        
      }
    }

    public void UpdateScript(string name, string content)
    {
      foreach(Scriptcontent s in scripts)
      {
        if (s.Name.Equals(name)) s.Parse(content);
      }
    }
  }

  
}
