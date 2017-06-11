using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Models
{
    public class SoundFiles : ProjectResourceGroup
    {
        private SQLiteConnection connection;

        public SoundFiles(SQLiteConnection connection)
        {
            Name = "SoundFiles";
            Icon = @"/InteractServer;component/Resources/Images/page_sound.gif";
            this.connection = connection;
            connection.CreateTable<SoundFile>();

            refresh();
        }

        private void refresh()
        {
            Resources = new System.Collections.ObjectModel.ObservableCollection<ProjectResource>(connection.Table<SoundFile>().AsEnumerable());
        }

        public bool SourcePathExists(string path)
        {
            foreach (SoundFile sf in Resources)
            {
                if (sf.SourcePath.Equals(path)) return true;
            }
            return false;
        }

        public bool NameExists(string name)
        {
            foreach (SoundFile sf in Resources)
            {
                if (sf.Name.Equals(name)) return true;
            }
            return false;
        }

        public string GetAvailableName(string name)
        {
            if (!NameExists(name)) return name;

            int i = 0;
            string newName;
            do
            {
                i++;
                newName = name + "_" + i.ToString();

            } while (NameExists(newName));

            return newName;
        }

        public bool Add(String FileName)
        {
            if (SourcePathExists(FileName))
            {
                Global.Log.AddEntry("The soundfile " + FileName + " is already part of this project.");
                return false;
            }

            connection.Insert(new SoundFile()
            {
                Version = 0,
                SourcePath = FileName,
                Name = GetAvailableName(Path.GetFileNameWithoutExtension(FileName)),
                Content = File.ReadAllBytes(FileName),
                FileType = SoundFile.GetFileType(FileName),
            });
            Global.Log.AddEntry("Soundfile " + FileName + " added to project.");
            refresh();
            return true;
        }

        public void Save(SoundFile sf)
        {
            sf.Version++;
            connection.Update(sf);
        }

        public void Remove(SoundFile sf)
        {
            connection.Delete(sf);
            refresh();
        }

        public void SendVersionsToClient(Guid ProjectID, string clientID)
        {
            foreach (SoundFile sf in Resources)
            {
                Global.Clients.Get(clientID).QueueMethod(() =>
                {
                    Global.NetworkService.SendSoundFileVersion(clientID, ProjectID, sf.ID, sf.Version);
                });
            }
        }

        public SoundFile Get(int sfID)
        {
            foreach (SoundFile sf in Resources)
            {
                if (sf.ID == sfID)
                {
                    return sf;
                }
            }
            return null;
        }

        public SoundFile Get(string name)
        {
            foreach (SoundFile sf in Resources)
            {
                if (sf.Name.Equals(name))
                {
                    return sf;
                }
            }
            return null;
        }
    }
}
