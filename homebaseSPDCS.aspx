<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="homebaseSPDCS.aspx.vb" Inherits="crmWebClient.homebaseSPDCS" MasterPageFile="~/EvoStyles/HomebaseTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/HomebaseTheme.Master" %>
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
  <div style="text-align: left; padding-top: 8px;">
    <asp:UpdatePanel ID="faa_flight_panel" runat="server" ChildrenAsTriggers="True"
      UpdateMode="Conditional">
      <ContentTemplate>
        <strong>Sale Price Data Collection Summary</strong>
        <asp:TextBox id="percent_good" size='1' value='25' runat="server"></asp:TextBox>%
        &nbsp;&nbsp;&nbsp;Only Show Past 15 Year MFR
        <asp:checkbox id="show_15_year" runat="server"></asp:checkbox>
         &nbsp;&nbsp;&nbsp;Model Meets %
         <asp:dropdownlist ID="ddl_is_good" runat="server" onclientclick='' ToolTip="Model Meets %">
        <asp:ListItem Value="All">All</asp:ListItem>
        <asp:ListItem Value="Y">Yes</asp:ListItem>
        <asp:ListItem Value="N">No</asp:ListItem> 
      </asp:dropdownlist>
      &nbsp;&nbsp;&nbsp;
       <asp:dropdownlist ID="ddl_model_type" runat="server" onclientclick='' ToolTip="">
        <asp:ListItem Value="All">All</asp:ListItem>
        <asp:ListItem Value="J" selected="true">Jets</asp:ListItem>
        <asp:ListItem Value="T">Turboprop</asp:ListItem> 
        <asp:ListItem Value="P">Piston</asp:ListItem> 
      </asp:dropdownlist>
        <asp:Button id="change_percent" runat="server" text="Change"></asp:Button>
        <asp:Table ID="menuTable" CellPadding="4" CellSpacing="0" Width="100%" CssClass="buttonsTable"
          runat="server">
          <asp:TableRow>
            <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="left" VerticalAlign="middle" Style="padding-right: 4px;">
              <div style="text-align: right; visibility: hidden;">
                Enter Aircraft ID <asp:TextBox ID="reg_no" runat="server" Width="200"></asp:TextBox>&nbsp;&nbsp;
                  <asp:LinkButton ID="runTaskBtn" runat="server" PostBackUrl="~/homebaseSPDCS.aspx?task=run" Text="<strong>Generate Report</strong>"></asp:LinkButton>
            </div>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="left"
              VerticalAlign="middle">
              <asp:Label ID="SPDCSDetailsLbl" runat="server" Text=""></asp:Label>
            </asp:TableCell>
         </asp:TableRow>
        </asp:Table>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</asp:Content>