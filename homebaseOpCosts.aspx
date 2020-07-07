<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/EmptyHomebaseTheme.Master"
  CodeBehind="homebaseOpCosts.aspx.vb" Inherits="crmWebClient.homebaseOpCosts" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyHomebaseTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script language="javascript" type="text/javascript">
    var bDontClose = false;

    function ActiveTabChanged(sender, args) { }

    function openSmallWindowJS(address, windowname) {

      var rightNow = new Date();
      windowname += rightNow.getTime();
      var Place = open(address, windowname, "menubar,scrollbars=1,resizable,width=900,height=600");

      return true;
    }
  
  </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="">
    <ProgressTemplate>
      <div id="divLoading" runat="server" style="text-align: center; font-weight: bold;
        background-color: #eeeeee; filter: alpha(opacity=90); opacity: 0.9; width: 395px;
        height: 295px; text-align: center; padding: 75px; position: absolute; border: 1px solid #003957;
        z-index: 10; margin-left: 225px;">
        <span>Please wait ... </span>
        <br />
        <br />
        <img src="/images/loading.gif" alt="Loading..." /><br />
      </div>
    </ProgressTemplate>
  </asp:UpdateProgress>
    <asp:UpdatePanel ID="opCosts_panel" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
      <ContentTemplate>
          <asp:Panel ID="container_opCosts_listing" runat="server" HorizontalAlign="Center">
            <asp:Label ID="opCosts_listing_text" runat="server" Text=""></asp:Label>
          </asp:Panel>
      </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
