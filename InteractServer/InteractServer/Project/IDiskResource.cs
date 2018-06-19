using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project
{
  public interface IDiskResource
  {
    string Name { get; }
    string Path { get; set; }
    ContentType Type { get; set; }
    string Data { get; set; }
    Guid ID { get; set; }
    int Version { get; set; }

    void MoveTo(string path);
  }
}
