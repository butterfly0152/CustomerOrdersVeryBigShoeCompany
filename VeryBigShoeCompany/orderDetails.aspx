<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="orderDetails.aspx.cs" Inherits="VeryBigShoeCompany.orderDetails" %>

<asp:Content ID="orderDetailsContent" ContentPlaceHolderID="pageContent" runat="server">
 <div class="pgOrderDetails">
  <div class="container">
   <asp:Panel ID="pnlNoItems" CssClass="pnlNoItems" runat="server">
    <asp:Label runat="server">No valid data available</asp:Label>
   </asp:Panel>

   <asp:Panel runat="server" ID="pnlContent">
    <!-- print -->
    <div class="row">
     <div class="col-12">
      <a class="cmdUploadFile marginLeft10 floatLeft centeredText blackColor textDecorationNone" href="index.aspx">Back</a>
      <asp:Button runat="server" ID="cmdPrint" Text="Print" CssClass="cmdUploadFile floatRight" OnClick="cmdPrint_Click" />
     </div>
    </div>

    <!-- table -->
    <div class="row marginTop30">
     <div class="col-12">
      <table class="table table-hover table-responsive-md centeredText">
       <thead>
        <tr>
         <th class="table-title">Customer Name</th>
         <th class="table-title">Customer Email</th>
         <th class="table-title">Quantity</th>
         <th class="table-title">Size</th>
         <th class="table-title">Date Required</th>
         <th class="table-title">Notes</th>
        </tr>
       </thead>
       <tbody id="tableBody" runat="server"></tbody>
      </table>
     </div>
    </div>
   </asp:Panel>

  </div>



 </div>
</asp:Content>
