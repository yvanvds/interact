using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project
{
  public interface IDiskResourceFolder
  {
    string FolderName { get; }
    string Icon { get; }
    bool IsExpanded { get; set; }

    int Count { get; }
    string GuiCount { get; }

    ObservableCollection<IDiskResource> Resources { get; }
  }
}
