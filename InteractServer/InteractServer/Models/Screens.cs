﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Models
{
  public class Screens : ProjectResourceGroup
  {
    private SQLiteConnection connection;

    public Screens(SQLiteConnection connection)
    {
      Name = "Screens"; // the name is shown in the tree view
      Icon = @"/InteractServer;component/Resources/Images/page.gif";
      this.connection = connection;
      connection.CreateTable<Screen>();

      refresh();
    }

    private void refresh()
    {
      Resources = new ObservableCollection<ProjectResource>(connection.Table<Screen>().AsEnumerable<Screen>());
    }

    public bool NameExists(string name)
    {
      foreach (Screen screen in Resources)
      {
        if (screen.Name.Equals(name)) return true;
      }
      return false;
    }

    public Screen CreateScreen(String Name, String Type)
    {
      if (NameExists(Name)) return null;

      connection.Insert(new Screen()
      {
        Version = 0,
        Name = Name,
        Type = Type,
        Content = null
      });

      refresh();

      foreach (Screen screen in Resources)
      {
        if (screen.Name.Equals(Name)) return screen;
      }

      // this probably cannot happen
      return null;
    }

    public void Save(Screen screen)
    {
      screen.Version++;
      connection.Update(screen);
    }

    public void Remove(Screen screen)
    {
      connection.Delete(screen);
      refresh();
    }

    public void SendVersionsToClient(Guid ProjectID, string clientID)
    {
      foreach (Screen screen in Resources)
      {
        Global.Clients.Get(clientID).QueueMethod(() =>
        {
          Global.NetworkService.SendScreenVersion(clientID, ProjectID, screen.ID, screen.Version);
        });
      }
    }

    public Screen Get(int ScreenID)
    {
      foreach (Screen screen in Resources)
      {
        if (screen.ID == ScreenID)
        {
          return screen;
        }
      }
      return null;
    }

    public Screen Get(string name)
    {
      foreach (Screen screen in Resources)
      {
        if (screen.Name.Equals(name))
        {
          return screen;
        }
      }
      return null;
    }
  }
}
