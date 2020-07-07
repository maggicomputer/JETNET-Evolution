<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Yacht_View_Template.aspx.vb"
    Inherits="crmWebClient.Yacht_View_Template" MasterPageFile="~/EvoStyles/YachtTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/YachtTheme.Master" %>

<%@ Register src="controls/Yacht_View_Master.ascx" tagname="YachtViewMaster" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
  <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE&sensor=false"></script>
  <script language="javascript" type="text/javascript" src="https://www.google.com/jsapi?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE"></script>
 
   
  <script language="javascript" type="text/javascript">

    function openNotesWindowJS(reportWindowPath, sReportFrom) {

      //alert(" show report : " + bShowReport + " report path : " + reportWindowPath + " report number : " + sReportID);

      var rightNow = new Date();
      var reportWindowName = "NotesReport" + sReportFrom + "Window";
      reportWindowName += rightNow.getTime();

      var reportWindowOptions = "scrollbars=yes,menubar=yes,height=800,width=1050,resizable=yes,toolbar=no,location=no,status=no";

      if (reportWindowPath != "") {
        var Place = window.open(reportWindowPath, reportWindowName, reportWindowOptions);
      }

      return true;
    }

    function openSmallWindowJS(address, windowname) {
      var rightNow = new Date();
      windowname += rightNow.getTime();
      var Place = window.open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1050,resizable=yes,toolbar=no,location=no,status=no");
      return true;
    }

  </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:YachtViewMaster ID="YachtViewMaster1" runat="server" />

</asp:Content>

