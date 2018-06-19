using InteractServer.Models;
using InteractServer.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Log
{
  public class ErrorLogEntry : PropertyChangedBase
  {
    public int Index { get; set; }
    public int Line { get; set; }
    public string Message { get; set; }

    public IDiskResource Resource;
  }

}
