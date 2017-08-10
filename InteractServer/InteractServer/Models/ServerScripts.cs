﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Models
{
  public class ServerScripts : ProjectResourceGroup
  {
    private SQLiteConnection connection;

    public ServerScripts(SQLiteConnection connection)
    {
      Name = "ServerScripts";

      Icon = @"/InteractServer;component/Resources/Icons/Code_16x.png";
      this.connection = connection;
      connection.CreateTable<ServerScript>();

      refresh();
    }

    private void refresh()
    {
      Resources = new ObservableCollection<ProjectResource>(connection.Table<ServerScript>().AsEnumerable<ServerScript>());
    }

    public bool NameExists(string name)
    {
      foreach (ServerScript script in Resources)
      {
        if (script.Name.Equals(name)) return true;
      }
      return false;
    }

    public ServerScript CreateServerScript(String Name)
    {
      if (NameExists(Name)) return null;

      connection.Insert(new ServerScript()
      {
        Name = Name,
        Content = ""
      });

      refresh();

      foreach(ServerScript serverScript in Resources)
      {
        if (serverScript.Name.Equals(Name))
        {
          Global.IntelliServerScripts.AddScript(serverScript.Name, serverScript.Content, Intellisense.Scriptcontent.SCRIPT_TYPE.SERVER);
          return serverScript;
        }
      }

      return null;
    }

    public void Save(ServerScript serverScript)
    {
      connection.Update(serverScript);
      Global.IntelliServerScripts.AddScript(serverScript.Name, serverScript.Content, Intellisense.Scriptcontent.SCRIPT_TYPE.SERVER);
    }

    public void Remove(ServerScript serverScript)
    {
      connection.Delete(serverScript);
      Global.IntelliServerScripts.RemoveScript(serverScript.Name);
      refresh();
    }

    public ServerScript Get(int scriptID)
    {
      foreach(ServerScript serverScript in Resources)
      {
        if(serverScript.ID == scriptID)
        {
          return serverScript;
        }
      }
      return null;
    }

    public ServerScript Get(string name)
    {
      foreach(ServerScript serverScript in Resources)
      {
        if(serverScript.Name.Equals(name))
        {
          return serverScript;
        }
      }
      return null;
    }
  }
}
