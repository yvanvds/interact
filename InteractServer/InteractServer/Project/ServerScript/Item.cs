using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project.ServerScript
{
  public class Item : IContentModel, IDiskResource
  {
    private IDiskResource resource;

    // implements IContentModel
    public Guid ID { get => resource.ID; set => resource.ID = value; }
    public string Name => resource.Name;
    public string Path { get => resource.Path; set => resource.Path = value; }
    public string Data { get => resource.Data; set => resource.Data = value; }
    public int Version { get => resource.Version; set => resource.Version = value; }
    public bool Tainted { get; set; }
    public ContentType Type { get => resource.Type; set => resource.Type = value; }
    public void MoveTo(string path) { resource.MoveTo(path); }

    public string Content { get; set; }

    public Item(IDiskResource resource) 
    {
      this.resource = resource;
      string file = System.IO.Path.Combine(Global.ProjectPath, resource.Path);
      this.Content = System.IO.File.ReadAllText(file);
      Tainted = false;
    }
  }
}
