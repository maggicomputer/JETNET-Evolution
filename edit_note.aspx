<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="edit_note.aspx.vb" Inherits="crmWebClient.edit_note"
  EnableViewState="true" ValidateRequest="false" %>

<%@ Register Src="controls/ActionItems.ascx" TagName="ActionItems" TagPrefix="uc1" %>
<%@ Register Src="controls/Notes.ascx" TagName="Notes" TagPrefix="uc2" %>
<%@ Register Src="controls/Opportunities.ascx" TagName="Opportunities" TagPrefix="uc3" %>
<%@ Register Src="controls/Email.ascx" TagName="Email" TagPrefix="uc4" %>
<%@ Register Src="controls/Documents.ascx" TagName="Documents" TagPrefix="uc5" %>
<%@ Register Src="controls/Wanted.ascx" TagName="Wanted" TagPrefix="uc6" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <title>
    <asp:Literal runat="server" ID="titleh">Marketplace Manager</asp:Literal></title>
  <link href="common/redesign.css" rel="stylesheet" type="text/css" />
  <link rel="stylesheet" type="text/css" href="common/anylinkmenu.css" />
  <!--Created Stylesheet-->
  <link href="/EvoStyles/stylesheets/additional_styles.css" rel="stylesheet" type="text/css" />
  <!--Grid/Layout Styles-->
  <link href="/EvoStyles/stylesheets/layout/base_html_elements.css" rel="stylesheet"
    type="text/css" />
  <link href="/EvoStyles/stylesheets/layout/skeleton_grid.css" rel="stylesheet" type="text/css" />

  <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
<link href="EvoStyles/stylesheets/tableThemes.css" type="text/css" rel="stylesheet" />
</head>
<body>
  <div class="container aircraftContainer">
    <div class="row remove_margin">
      <div class="valueSpec viewValueExport Simplistic aircraftSpec">
        <div class="sixteen columns">
          <form id="form1" runat="server">
          <!-- The following loads a blank page that refreshes 60 seconds before the session timeout to keep session from expiring -->
          <iframe id="ifrmBlank" frameborder="0" width="0" height="0" runat="server" src="sessionKeepAlive.aspx">
          </iframe>
          <div>
            <h2 class="mainHeading padded_left">
              <asp:Literal runat="server" ID="heading">Add a Note</asp:Literal></h2>
          </div>
          <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
          </cc1:ToolkitScriptManager>
          <uc3:Opportunities ID="Opportunities1" runat="server" Visible="false" />
          <uc1:ActionItems ID="ActionItems1" runat="server" Visible="false" />
          <uc2:Notes ID="Notes1" runat="server" Visible="false" />
          <uc5:Documents ID="Documents1" runat="server" Visible="false" />
          <uc4:Email ID="Email1" runat="server" Visible="false" />
          <uc6:Wanted ID="Wanted1" runat="server" Visible="false" />
          </form>
        </div>
      </div>
    </div>
  </div>

  <script type="text/javascript">
    window.onload = function() {
      ResizeWindowInfo();
    }
    function ResizeWindowInfo() {
      var myWidth = 0, myHeight = 0;
      var innerW = 0, innerH = 0;
      var resizeWidth = 0, resizeHeight = 0;
      if (typeof (window.outerWidth) == 'number') {
        //Non-IE
        myWidth = window.outerWidth;
        myHeight = window.outerHeight;
        innerW = window.innerWidth;
        innerH = window.innerHeight;
      } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
        //IE 6+ in 'standards compliant mode'
        myWidth = document.documentElement.clientWidth;
        myHeight = document.documentElement.clientHeight;
      } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
        //IE 4 compatible
        myWidth = document.body.clientWidth;
        myHeight = document.body.clientHeight;
      }

      if ((innerH > 0) && (innerW > 0)) {
        resizeWidth = (myWidth - innerW) + 60;

        if (myHeight < 600) {
          resizeHeight = (myHeight - innerH) + 60
        }
        window.resizeBy(resizeWidth, resizeHeight)
      }
    }
  </script>

</body>
<asp:chart id="ANALYTICS_HISTORY" visible="False" runat="server" imagestoragemode="UseImageLocation"
  imagetype="Jpeg">
        <Series>
            <asp:Series Name="Series1" ChartArea="ChartArea1">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
            </asp:ChartArea>
        </ChartAreas>
    </asp:chart>
<asp:updatepanel id="bottom_tab_update_panel" runat="server" childrenastriggers="true"
  visible="false">
  <ContentTemplate>
    <div id="chart_div_value_history" runat="server" ></div> 
    </ContentTemplate>
 </asp:updatepanel>
</html>
