using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project
{
  public interface IContentModel
  {
    Guid ID { get; }
    string Name { get; }
    ContentType Type { get; }

    bool Tainted { get; set; }
  }
}
