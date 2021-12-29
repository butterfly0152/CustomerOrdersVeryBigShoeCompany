<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="VeryBigShoeCompany.assets.pages.index" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="pageContent" runat="server">
 <div class="indexPage">
  <div class="container">
   <div class="divProcessing">

    <input class="form-control form-control-sm width20P" id="formFile" type="file" runat="server" accept =".xml">
    <asp:Button runat="server" ID="cmdProcessFile" CssClass="cmdUploadFile marginLeft10" OnClick="cmdProcessFile_Click" Text="Upload File" />

   </div>
  </div>
 </div>

</asp:Content>
