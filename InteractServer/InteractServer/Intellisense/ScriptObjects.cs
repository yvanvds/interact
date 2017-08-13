using AutocompleteMenuNS;
using Jint.Runtime.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InteractServer.Intellisense
{
  public class ScriptObjects
  {
    protected class GlobalObject
    {
      public string name;
      public object obj;
    }

    protected class ScriptType
    {
      public bool canCreate = true;
      public Type type;
    }

    protected Dictionary<string, GlobalObject> Objects = new Dictionary<string, GlobalObject>();
    protected Dictionary<string, ScriptType> Types = new Dictionary<string, ScriptType>();
    protected Dictionary<string, ScriptType> Enums = new Dictionary<string, ScriptType>();

    public ScriptObjects()
    {

    }

    protected void AddGlobalObject(string objName, string className, Type type, object obj = null)
    {
      Objects.Add(objName, new ScriptObjects.GlobalObject()
      {
        name = className,
        obj = obj
      });
      Types.Add(className, new ScriptType() { canCreate = false, type = type });
    }

    protected void AddScriptType(string typeName, Type type)
    {
      Types.Add(typeName, new ScriptType() { type = type });
    }

    protected void AddEnum(string enumName, Type type)
    {
      Enums.Add(enumName, new ScriptType() { type = type });
    }

    public string GetGlobalObjectType(string key)
    {
      if (Objects.ContainsKey(key))
      {
        return Objects[key].name;
      }
      return null;
    }

    public void InsertInto(Jint.Engine engine)
    {
      foreach (KeyValuePair<string, GlobalObject> entry in Objects)
      {
        if (entry.Value.obj != null) engine.SetValue(entry.Key, entry.Value.obj);
      }

      foreach (KeyValuePair<string, ScriptType> entry in Types)
      {
        if (entry.Value.canCreate) engine.SetValue(entry.Key, TypeReference.CreateTypeReference(engine, entry.Value.type));
      }

      foreach (KeyValuePair<string, ScriptType> entry in Enums)
      {
        engine.SetValue(entry.Key, TypeReference.CreateTypeReference(engine, entry.Value.type));
      }
    }

    public void AddTypes(List<AutocompleteItem> list)
    {
      foreach (var entry in Types)
      {
        if (entry.Value.canCreate)
        {
          list.Add(new AutocompleteItem(entry.Key + "()"));
        }

      }
    }

    public bool HasType(string typeName)
    {
      return Types.ContainsKey(typeName);
    }

    public bool HasEnum(string enumName)
    {
      return Enums.ContainsKey(enumName);
    }

    public void GetTypeMethods(List<string> objParts, List<AutocompleteItem> list)
    {
      PropertyInfo[] propInfo = null;
      MethodInfo[] methodInfo = null;

      if (objParts.Count > 1)
      {
        int index = objParts.Count - 1;
        PropertyInfo pInfo = Types[objParts[index]].type.GetProperty(objParts[index - 1]);
        MethodInfo mInfo = Types[objParts[index]].type.GetMethod(objParts[index - 1]);
        if (pInfo == null && mInfo == null) return;

        index--;
        while (index > 0)
        {
          if (pInfo != null)
          {
            mInfo = pInfo.PropertyType.GetMethod(objParts[index - 1]);
            pInfo = pInfo.PropertyType.GetProperty(objParts[index - 1]);
          }
          else if (mInfo != null)
          {
            pInfo = mInfo.ReturnType.GetProperty(objParts[index - 1]);
            mInfo = mInfo.ReturnType.GetMethod(objParts[index - 1]);
          }
          if (pInfo == null && mInfo == null) return;
          index--;
        }
        if (pInfo != null)
        {
          propInfo = pInfo.PropertyType.GetProperties();
          methodInfo = pInfo.PropertyType.GetMethods();
        }
        else if (mInfo != null)
        {
          propInfo = mInfo.ReturnType.GetProperties();
          methodInfo = mInfo.ReturnType.GetMethods();
        }
      }
      else
      {
        propInfo = Types[objParts[0]].type.GetProperties();
        methodInfo = Types[objParts[0]].type.GetMethods();
      }

      if (propInfo != null) foreach (var entry in propInfo)
        {
          list.Add(new MethodAutocompleteItem(entry.Name + " = ")
          {
            MenuText = entry.Name + " (" + entry.PropertyType.Name + ")",
            ImageIndex = 1,
          });
        }

      if (methodInfo != null) foreach (var entry in methodInfo)
        {
          if (entry.IsSpecialName) continue;
          if (!entry.IsPublic) continue;

          string s = entry.Name + "(";
          ParameterInfo[] pInfo = entry.GetParameters();
          foreach (var parm in pInfo)
          {
            s += parm.ParameterType.Name + " " + parm.Name;
            if (parm.Position < pInfo.Length - 1) s += ", ";
          }
          s += ")";
          string complete = entry.Name + "()";

          list.Add(new MethodAutocompleteItem(complete)
          {
            MenuText = entry.ReturnType.Name + " " + s,
            ImageIndex = 0,
          });
        }
    }

    public void GetMethodArgs(List<string> objParts, int currentArg, List<AutocompleteItem> list)
    {
      MethodInfo mInfo = null;

      if (objParts.Count > 1)
      {
        int index = objParts.Count - 1;
        try
        {
          mInfo = Types[objParts[index]].type.GetMethod(objParts[index - 1]);
        } catch(AmbiguousMatchException)
        {
          // TODO: implement methods with overloading!
          mInfo = null;
        }
        
        if (mInfo == null) return;

        index--;
        while (index > 0)
        {
          if (mInfo != null)
          {
            try
            {
              mInfo = mInfo.ReturnType.GetMethod(objParts[index - 1]);
            }
            catch (AmbiguousMatchException)
            {
              // TODO: implement methods with overloading!
              mInfo = null;
            }

          }
          index--;
        }
      }

      if (mInfo != null)
      {
        if (mInfo.IsSpecialName) return;
        if (!mInfo.IsPublic) return;

        list.Add(new ArgumentsHintAutocompleteItem(mInfo.GetParameters(), currentArg)
        {
          ImageIndex = 2,
        });
      }
    }

    public void GetEnumValues(string obj, List<AutocompleteItem> list)
    {
      string[] result = Enums[obj].type.GetEnumNames();
      foreach (string s in result)
      {
        list.Add(new MethodAutocompleteItem(s));
      }
    }
  }
}
