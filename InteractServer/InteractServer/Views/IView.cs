using InteractServer.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.Views
{
  

  public interface IView
  {
    Guid ID { get; }
    LayoutDocument Document { get; }

    void Save();

    bool Tainted { get; }

    ContentType Type { get; }

    CodeEditor Editor { get; }
  }
}
