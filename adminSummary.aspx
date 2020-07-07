<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="adminSummary.aspx.vb" Inherits="crmWebClient.adminSummary" MasterPageFile="~/EvoStyles/CustomerAdminTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/CustomerAdminTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
  <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <link rel="Stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.12.1/themes/smoothness/jquery-ui.css" />

  <script type="text/javascript">
    var bDontClose = false;

    function ActiveTabChanged(sender, args) { }

    function openReportWindow(reportWindowPath, sReportID) {

      //alert(" show report : " + bShowReport + " report path : " + reportWindowPath + " report number : " + sReportID);

      var rightNow = new Date();
      var reportWindowName = "AdminReport" + sReportID + "Window";
      reportWindowName += rightNow.getTime();

      var reportWindowOptions = "scrollbars=yes,menubar=yes,height=800,width=1050,resizable=yes,toolbar=no,location=no,status=no";

      if (reportWindowPath != "") {
        var Place = window.open(reportWindowPath, reportWindowName, reportWindowOptions);
      }

      return true;
    }
         
  </script>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="admin_report_panel">
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
    <asp:UpdatePanel ID="admin_report_panel" runat="server" ChildrenAsTriggers="True" UpdateMode="Always">
      <ContentTemplate>
        <asp:Table ID="menuTable" CellPadding="4" CellSpacing="0" Width="100%" CssClass="buttonsTable"
          runat="server">
          <asp:TableRow>
            <asp:TableCell ID="TableCell0" runat="server" HorizontalAlign="left" VerticalAlign="middle"
              Style="padding-right: 4px;">
              <div style="text-align: center;">
                <asp:Label ID="reportErrorLbl" runat="server" Visible="false"></asp:Label>
              </div>
              <asp:Label ID="adminReportsListLbl" runat="server"></asp:Label>
            </asp:TableCell>
          </asp:TableRow>

        </asp:Table>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
  <div id="DivLoadingMessage" style="display: none;">
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

  <script type="text/javascript">

    function ShowLoadingMessage(DivTag, Title, Message) {
      $("#" + DivTag).html(Message);
      $("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 75, resizable: false });
    }

    function CloseLoadingMessage(DivTag) {
      $("#" + DivTag).dialog("close");
    }

  </script>

</asp:Content> 
