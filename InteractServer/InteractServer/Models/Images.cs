using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Models
{
    public class Images : ProjectResourceGroup
    {
        private SQLiteConnection connection;

        public Images(SQLiteConnection connection)
        {
            Name = "Images";
            Icon = @"/InteractServer;component/Resources/Images/image.gif";
            this.connection = connection;
            connection.CreateTable<Image>();

            refresh();
        }

        private void refresh()
        {
            Resources = new System.Collections.ObjectModel.ObservableCollection<ProjectResource>(connection.Table<Image>().AsEnumerable());
        }

        public bool SourcePathExists(string path)
        {
            foreach(Image image in Resources)
            {
                if (image.SourcePath.Equals(path)) return true;
            }
            return false;
        }

        public bool NameExists(string name)
        {
            foreach(Image image in Resources)
            {
                if (image.Name.Equals(name)) return true;
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
            if(SourcePathExists(FileName))
            {
                Global.Log.AddEntry("The image " + FileName + " is already part of this project.");
                return false;
            }

            connection.Insert(new Image()
            {
                Version = 0,
                SourcePath = FileName,
                Name = GetAvailableName(Path.GetFileNameWithoutExtension(FileName)),
                ImageObj = new System.Drawing.Bitmap(FileName),
            });
            Global.Log.AddEntry("Image " + FileName + " added to project.");
            refresh();
            return true;
        }

        public void Save(Image image)
        {
            image.Version++;
            connection.Update(image);
        }

        public void Remove(Image image)
        {
            connection.Delete(image);
            refresh();
        }

        public void SendVersionsToClient(Guid ProjectID, string clientID)
        {
            foreach (Image image in Resources)
            {
                Global.Clients.Get(clientID).QueueMethod(() =>
                {
                    Global.NetworkService.SendImageVersion(clientID, ProjectID, image.ID, image.Version);
                });
            }
        }

        public Image Get(int imageID)
        {
            foreach(Image image in Resources)
            {
                if(image.ID == imageID)
                {
                    return image;
                }
            }
            return null;
        }

        public Image Get(string name)
        {
            foreach(Image image in Resources)
            {
                if(image.Name.Equals(name))
                {
                    return image;
                }
            }
            return null;
        }

    }
}
