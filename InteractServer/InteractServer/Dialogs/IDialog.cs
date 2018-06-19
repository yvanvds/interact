using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Dialogs
{
  public interface IDialog
  {
    bool? ShowDialog();

    string ModelName { get; }

    string Type { get; }
  }
}
