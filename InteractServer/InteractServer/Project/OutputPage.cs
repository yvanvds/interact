﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using tainicom.WpfPropertyGrid;
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.Project
{
    [BrowsableProperty(BrowsableCategoryAttribute.All, false)]
    [BrowsableProperty("Name", true)]
    [BrowsableProperty("ID", true)]
    [BrowsableProperty("Version", true)]
    public class OutputPage : IResource
    {
        public Controls.OutputList View = null;

        public string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                needsSaving = true;
            }
        }

        public string DisplayName => System.IO.Path.GetFileNameWithoutExtension(Name);
        public string Location => Path.Combine(folderPath, Name);

        private LayoutDocument document = null;
        public LayoutDocument Document => document;

        private string folderPath;
        private bool serverSide;

        private string content = string.Empty;

        private ContentType type = ContentType.Invalid;
        public ContentType Type => type;

        private string id = string.Empty;
        public string ID => id;

        private string icon = "";
        public string Icon => icon;

        private int version = 0;
        public int Version => version;

        public OutputPage(string name, bool serverSide, string folderPath)
        {
            this.id = shortid.ShortId.Generate(true);
            this.folderPath = folderPath;
            this.serverSide = serverSide;
            this.Name = name;

            createView();

            if (serverSide)
            {
                type = ContentType.ServerOutput;
            }
            else
            {
                type = ContentType.ClientOutput;
            }

            setupDocument();
        }

        public OutputPage(JObject obj, bool serverSide, string folderPath)
        {
            this.folderPath = folderPath;
            this.serverSide = serverSide;
            LoadFromJson(obj);
            this.name = (string)obj["Name"];
            createView();

            try
            {
                content = File.ReadAllText(System.IO.Path.Combine(this.folderPath, Name));
                View.Load(JObject.Parse(content));
            }
            catch (Exception e)
            {
                Dialogs.Error.Show("OutputList Error", e.Message);
            }

            if (serverSide)
            {
                type = ContentType.ServerOutput;
            }
            else
            {
                type = ContentType.ClientOutput;
            }

            setupDocument();
        }

        private void createView()
        {
            View = new Controls.OutputList(DisplayName, ID);
        }

        private void setupDocument()
        {
            Frame frame = new Frame();
            frame.Content = View;

            document = new LayoutDocument();
            document.Content = frame;
            document.Title = Name;
        }

        public void DeleteOnDisk()
        {
            File.Delete(Path.Combine(folderPath, Name));
        }

        public bool LoadFromJson(JObject obj)
        {

            if (obj.ContainsKey("ID")) id = (string)obj["ID"];
            else return false;

            if (obj.ContainsKey("Type"))
            {
                switch ((string)obj["Type"])
                {
                    case "ClientOutput":
                        type = ContentType.ClientOutput;
                        break;
                    case "ServerOutput":
                        type = ContentType.ServerOutput;
                        break;
                }
            }
            if (obj.ContainsKey("Version")) version = (int)obj["Version"];

            needsSaving = false;
            return true;
        }

        public void MoveTo(string path)
        {
            throw new NotImplementedException();
        }

        private bool needsSaving = false;
        public bool NeedsSaving()
        {
            if (View.NeedsSaving) needsSaving = true;
            return needsSaving;
        }

        public bool SaveContent()
        {
            try
            {
                content = View.Save().ToString();
                File.WriteAllText(System.IO.Path.Combine(folderPath, Name), content);
                return true;
            }
            catch (Exception e)
            {
                Dialogs.Error.Show("Error on " + e.TargetSite, e.Message);
                return false;
            }
        }

        public JObject SaveToJson()
        {
            if (needsSaving) version++;
            JObject obj = new JObject();
            obj["Name"] = Name;
            obj["ID"] = ID;
            obj["Type"] = Type.ToString();
            obj["Version"] = Version;
            needsSaving = false;
            return obj;
        }

        public string SerializeForClient()
        {
            JObject obj = new JObject();
            obj["Name"] = Name;
            obj["ID"] = ID;
            obj["Type"] = Type.ToString();
            obj["Version"] = Version;
            obj["Content"] = content;
            return obj.ToString();
        }

        public void OnShow()
        {

        }
    }
}
