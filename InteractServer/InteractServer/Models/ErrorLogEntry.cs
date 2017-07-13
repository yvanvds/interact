using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Models
{
  public class ErrorLogEntry : PropertyChangedBase
  {
    public int Index { get; set; }
    public int Line { get; set; }
    public string Message { get; set; }

    public ProjectResource Resource;
  }

}
