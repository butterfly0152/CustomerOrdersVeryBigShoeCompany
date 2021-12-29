using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingLibrary
{
 public static class General
 {
  public static string FormatInput(string input)
  {
   return input.Replace("'", " ").Replace("\"", " ").Replace("\r\n", "").Replace("\n", " ");
  }
 }
}
