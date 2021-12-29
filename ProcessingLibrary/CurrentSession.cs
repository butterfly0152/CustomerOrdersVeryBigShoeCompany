using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingLibrary
{
 public partial class CurrentSession
 {
  public List<BigShoeDataImportOrder> ListDataImports = new List<BigShoeDataImportOrder>();
  public List<string> ListTotalErrors = new List<string>();
  public static CurrentSession SessionCheck(CurrentSession cs, out CurrentSession outcs)
  {
   if (cs == null)
   {
    cs = new CurrentSession();
   }
   outcs = cs;
   return cs;
  }
 
}
}
