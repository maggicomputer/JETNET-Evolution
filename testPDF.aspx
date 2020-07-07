<%@ Page Title="" Language="vb" AutoEventWireup="false"  MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" CodeBehind="testPDF.aspx.vb" Inherits="crmWebClient.testPDF" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="">
    <ProgressTemplate>
      <div id="divLoading" runat="server" style="text-align: center; font-weight: bold; background-color: #eeeeee; filter: alpha(opacity=90);
        opacity: 0.9; width: 395px; height: 295px; text-align: center; padding: 75px; position: absolute; border: 1px solid #003957;
        z-index: 10; margin-left: 225px;">
        <span>Please wait ... </span>
        <br />
        <br />
        <img src="/images/loading.gif" alt="Loading..." /><br />
      </div>
    </ProgressTemplate>
  </asp:UpdateProgress>
  <div style="text-align: left;">
    <asp:UpdatePanel ID="testPDF" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
      <ContentTemplate>
        <asp:FileUpload ID="FileUpload1" runat="server" Width="100" />
        <asp:Button ID="Button1" runat="server" Text="Process HTML to PDF" />
         <br />
        <asp:TextBox ID="TextBox1" runat="server" Rows="5" Columns="120" TextMode="MultiLine"></asp:TextBox>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">
</asp:Content>