using ProcessingLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;

namespace VeryBigShoeCompany.assets.pages
{
 public partial class index : System.Web.UI.Page
 {
  CurrentSession _session = null;
  protected void Page_Load(object sender, EventArgs e)
  {
   Session["user"] = CurrentSession.SessionCheck((CurrentSession)Session["user"], out _session);
  }

  protected void cmdProcessFile_Click(object sender, EventArgs e)
  {
   _session.ListTotalErrors.Clear();
   _session.ListDataImports.Clear();
   string error = "";
   BigShoeDataImport dataImport = XMLDeserializer.DeserializeBigShoeDataImport(formFile.PostedFile.InputStream, ref error);
   if (error.Length > 0)
   {
    //show the error
    ClientScript.RegisterStartupScript(this.GetType(), "openGeneralModal", "javascript:openGeneralModal('Error while processing the file: " + error + "');", true);
    return;
   }

   foreach (BigShoeDataImportOrder order in dataImport.Order)
   {
    if (!Validator.ValidateImportOrder(order, out List<string> errors))
    {
     _session.ListTotalErrors.AddRange(errors);
    }
    else
    {
     //show only the correct data
     _session.ListDataImports.Add(order);
    }
   }
    HttpContext.Current.Response.Redirect("orderDetails.aspx");
  }
 }
}