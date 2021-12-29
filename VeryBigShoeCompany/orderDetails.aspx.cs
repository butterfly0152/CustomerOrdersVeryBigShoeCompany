using PdfSharp.Charting;
using PdfSharp.Pdf;
using ProcessingLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace VeryBigShoeCompany
{
 public partial class orderDetails : System.Web.UI.Page
 {
  CurrentSession _session = null;
  protected void Page_Load(object sender, EventArgs e)
  {
   Session["user"] = CurrentSession.SessionCheck((CurrentSession)Session["user"], out _session);

   if (_session.ListDataImports.Count > 0)
   {
    pnlNoItems.CssClass += " displayNone";
    tableBody.InnerHtml = PopulateTable();
   }
   else
   {
    pnlContent.CssClass += "displayNone";
   }
   if(!Page.IsPostBack)
   {
    //if I found validation errors, I show them here
    if (_session.ListTotalErrors.Count > 0)
    {
     ClientScript.RegisterStartupScript(this.GetType(), "openGeneralModal", "javascript:openGeneralModal('" + string.Join("<br/>", _session.ListTotalErrors) + "');", true);
     _session.ListTotalErrors.Clear();
    }
   }
  }

  private string PopulateTable()
  {
   StringBuilder sb = new StringBuilder();

   foreach (BigShoeDataImportOrder di in _session.ListDataImports)
   {
    CreateTableRow(di, sb);
   }
   return sb.ToString();
  }

  public void CreateTableRow(BigShoeDataImportOrder di, StringBuilder sb)
  {
   sb.Append("<tr>");
   sb.Append($"<td class= 'table-subtitle'>{di.CustomerName}</td>");
   sb.Append($"<td class= 'table-subtitle'>{di.CustomerEmail}</td>");
   sb.Append($"<td class= 'table-subtitle'>{di.Size}</td>");
   sb.Append($"<td class= 'table-subtitle'>{di.Quantity}</td>");
   sb.Append($"<td class= 'table-subtitle'>{di.DateRequired.ToString("yyyy-MM-dd")}</td>");
   sb.Append($"<td class= 'table-subtitle'>{di.Notes}</td>");
   sb.Append("</tr>");
  }

  protected void cmdPrint_Click(object sender, EventArgs e)
  {
   //create pdf document to be printed
   string orderName = GeneratePdf(out string err);
   if (err.Length > 0)
   {
    ClientScript.RegisterStartupScript(this.GetType(), "openGeneralModal", "javascript:openGeneralModal('Error while generating pdf: <br/> " + err + "');", true);
    return;
   }
   //open the created document
   Process.Start(orderName);

   //print the document
   string printingErr = PrintOrders(orderName);
   if (printingErr.Length > 0)
   {
    ClientScript.RegisterStartupScript(this.GetType(), "openGeneralModal", "javascript:openGeneralModal('Error while printing pdf: <br/> " + printingErr + "');", true);
   }
  }

  private string GeneratePdf(out string err)
  {
   err = "";
   string fileName = "";
   string template = CreateHtml();
   PdfDocument pdf = PdfGenerator.GeneratePdf(template, PdfSharp.PageSize.A4);

   string name = $"OrderInfo_{DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss")}.pdf";
   string onlyPath = System.IO.Path.GetTempPath() + @"\OrderImport\";
   string tempFileName = onlyPath + name;

   if (!Directory.Exists(System.IO.Path.GetTempPath() + @"\OrderImport"))
   {
    Directory.CreateDirectory(System.IO.Path.GetTempPath() + @"\OrderImport");
   }
   try
   {
    pdf.Save(tempFileName);
    fileName = tempFileName;
    return fileName;
   }
   catch (Exception ex)
   {
    err = General.FormatInput(ex.Message);
   }
   return fileName;
  }

  private string PrintOrders(string orderName)
  {
   string err = "";
   try
   {
    ProcessStartInfo info = new ProcessStartInfo();
    Process p;

    // Set process setting to be hidden
    info.Verb = "print";
    info.FileName = orderName;

    info.CreateNoWindow = true;
    info.WindowStyle = ProcessWindowStyle.Hidden;

    // Start hidden process
    p = new Process();
    p.StartInfo = info;
    p.Start();

    // Give the process some time
    p.WaitForInputIdle();
    Thread.Sleep(1000);

    // Close it
    if (p.CloseMainWindow() == false)
    {
     p.Close();
    }
   }
   catch(Exception ex)
   {
    err = General.FormatInput(ex.Message);
   }
   return err;
  }
  public string CreateHtml()
  {
   StringBuilder sb = new StringBuilder();

   CreateItemsTableHeader(sb);
   int PageNumber = 1;
   int TotalPages = 1;

   int idx = 1;
   int No = 1;

   foreach (BigShoeDataImportOrder di in _session.ListDataImports)
   {
    bool willjumptonexpage = idx == 24 && _session.ListDataImports.Count + 4 > 24;

    sb.Append("<tr>");
    sb.Append($"<td style = 'font-size: 14px; text-align:center; padding-top:3px; padding-bottom:3px; padding-left:2px; padding-right:2px; border-right:1px solid #e6e6e6; border-top: 1px solid #e6e6e6; border-left: 1px solid #e6e6e6; border-bottom: 1px solid #e6e6e6;'>{No}</td>");

    sb.Append($"<td style = 'font-size: 14px; text-align:center; padding-top:3px; padding-bottom:3px; padding-left:2px; padding-right:2px; border-right:1px solid #e6e6e6; border-top: 1px solid #e6e6e6; border-left: 1px solid #e6e6e6; border-bottom: 1px solid #e6e6e6;'>{di.CustomerName}</td>");

    sb.Append($"<td style = 'font-size: 14px; text-align:center; padding-top:3px; padding-bottom:3px; padding-left:2px; padding-right:2px; border-right:1px solid #e6e6e6; border-top: 1px solid #e6e6e6; border-left: 1px solid #e6e6e6; border-bottom: 1px solid #e6e6e6;'>{di.CustomerEmail}</td>");

    sb.Append($"<td style = 'font-size: 14px; text-align:center; padding-top:3px; padding-bottom:3px; padding-left:2px; padding-right:2px; border-right:1px solid #e6e6e6; border-top: 1px solid #e6e6e6; border-left: 1px solid #e6e6e6; border-bottom:1px solid #e6e6e6;'>{di.Size}</td>");

    sb.Append($"<td style = 'font-size: 14px; text-align:center; padding-top:3px; padding-bottom:3px; padding-left:2px; padding-right:2px; border-right:1px solid #e6e6e6; border-top: 1px solid #e6e6e6; border-left: 1px solid #e6e6e6; border-bottom: 1px solid #e6e6e6;'>{di.Quantity}</td>");

    sb.Append($"<td style = 'font-size: 14px; text-align:center; padding-top:3px; padding-bottom:3px; padding-left:2px; padding-right:2px; border-right:1px solid #e6e6e6; border-top: 1px solid #e6e6e6; border-left: 1px solid #e6e6e6; border-bottom: 1px solid #e6e6e6;'>{di.DateRequired.ToString("yyyy-MM-dd")}</td>");

    sb.Append($"<td style = 'font-size: 14px; text-align:center; padding-top:3px; padding-bottom:3px; padding-left:2px; padding-right:2px; border-right:1px solid #e6e6e6; border-top: 1px solid #e6e6e6; border-left: 1px solid #e6e6e6; border-bottom: 1px solid #e6e6e6;'>{di.Notes}</td>");
    sb.Append("</tr>");

    if (willjumptonexpage)
    {
     sb.Append("</table>");
     idx = 0;
     PageNumber += 1;
     TotalPages += 1;

     CreateItemsTableHeader(sb);
    }
    idx++;
    No++;
   }

   sb = sb.Replace("[$TotalPages$]", TotalPages.ToString());
   return sb.ToString();
  }

  private void CreateItemsTableHeader(StringBuilder sb)
  {
   //company name
   sb.Append("<table align = 'center' style = 'border-collapse: collapse; margin-top:20px; margin-left: auto; margin-right: auto; width:100%;'>");
   sb.Append("<tr>");
   sb.Append("<th style= 'padding-bottom:15px; font-size: 20px;'>Very Big Shoe Company</th>");
   sb.Append("</tr>");

   sb.Append("<tr>");
   sb.Append("<td style= 'font-size:18px;'>Orders info</td>");
   sb.Append("</tr>");
   sb.Append("</table>");
   //the table
   sb.Append("<table align = 'center' style = 'border-collapse: collapse; margin-top:40px; margin-left: auto; margin-right: auto; width:100%;'>");
   sb.Append("<tr>");

   sb.Append("<th style = 'font-weight:normal; font-size:14px; text-align:center; padding-bottom:3px; padding-top:3px; padding-right:10px; padding-left:10px;'>");
   sb.Append("NO.");
   sb.Append("</th>");

   sb.Append("<th style = 'font-weight:normal; font-size:14px; text-align:center; padding-bottom:3px; padding-top:3px; padding-right:10px; padding-left:10px;'>");
   sb.Append("CUSTOMER NAME");
   sb.Append("</th>");

   sb.Append("<th style = 'font-weight:normal; font-size:14px; text-align:center; padding-bottom:3px; padding-top:3px; padding-right:10px; padding-left:10px;'>");
   sb.Append("CUSTOMER EMAIL");
   sb.Append("</th>");

   sb.Append("<th style = 'font-weight:normal; font-size:14px; text-align:center; padding-bottom:3px; padding-top:3px; padding-right:10px; padding-left:10px;'>");
   sb.Append("SIZE");
   sb.Append("</th>");

   sb.Append("<th style = 'font-weight:normal; font-size:14px; text-align:center; padding-bottom:3px; padding-top:3px; padding-right:10px; padding-left:10px;'>");
   sb.Append("QUANTITY");
   sb.Append("</th>");

   sb.Append("<th style = 'font-weight:normal; font-size:14px; text-align:center; padding-bottom:3px; padding-top:3px; padding-right:10px; padding-left:10px;'>");
   sb.Append("DATE REQUIRED");
   sb.Append("</th>");

   sb.Append("<th style = 'font-weight:normal; font-size:14px; text-align:center; padding-bottom:3px; padding-top:3px; padding-right:10px; padding-left:10px;'>");
   sb.Append("NOTES");
   sb.Append("</th>");
   sb.Append("</tr>");
  }
 }
}