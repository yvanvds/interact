using System;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace InteractServer.Models
{
  public class Patchers : ProjectResourceGroup
  {
    private SQLiteConnection connection;

    public Patchers(SQLiteConnection connection)
    {
      Name = "Patchers";
      Icon = @"/InteractServer;component/Resources/Icons/Patcher_16x.png";
      this.connection = connection;
      connection.CreateTable<Patcher>();

      refresh();
    }

    private void refresh()
    {
      Resources = new ObservableCollection<ProjectResource>(connection.Table<Patcher>().AsEnumerable<Patcher>());
    }

    public bool NameExists(string name)
    {
      foreach(Patcher patcher in Resources)
      {
        if (patcher.Name.Equals(name)) return true;
      }
      return false;
    }

    public Patcher CreatePatcher(String Name)
    {
      if (NameExists(Name)) return null;

      connection.Insert(new Patcher()
      {
        Version = 0,
        Name = Name,
        Content = null
      });

      refresh();

      foreach(Patcher patcher in Resources)
      {
        if(patcher.Name.Equals(Name))
        {
          string content = (patcher.ContentObj as PatcherContent.Base).Content;

          return patcher;
        }
      }

      return null;
    }

    public void Save(Patcher patcher)
    {
      patcher.Version++;
      connection.Update(patcher);
      string content = (patcher.ContentObj as PatcherContent.Base).Content;

    }

    public void Remove(Patcher patcher)
    {
      connection.Delete(patcher);
      refresh();
    }

    public void SendVersionsToClient(Guid ProjectID, string clientID)
    {
      foreach(Patcher patcher in Resources)
      {
        Global.Clients.Get(clientID).QueueMethod(() =>
        {
          Global.Sender.SendPatcherVersion(clientID, ProjectID, patcher.ID, patcher.Version);
        });
      }
    }

    public Patcher Get(int PatcherID)
    {
      foreach(Patcher patcher in Resources)
      {
        if(patcher.ID == PatcherID)
        {
          return patcher;
        }
      }
      return null;
    }

    public Patcher Get(string name)
    {
      foreach(Patcher patcher in Resources)
      {
        if (patcher.Name.Equals(name)) return patcher;
      }
      return null;
    }
  }
}
