<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/EmptyHomebaseTheme.Master" CodeBehind="homebaseCompanyProspect.aspx.vb" Inherits="crmWebClient.homebaseCompanyProspect" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyHomebaseTheme.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <link rel="Stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.12.1/themes/smoothness/jquery-ui.css" />
  <link href="common/aircraft_model.css" type="text/css" rel="stylesheet" />


  <script type="text/javascript" src="/common/moment-with-locales.js"></script>

  <style type="text/css">
    .ui-state-default, .ui-widget-content .ui-state-default, .ui-widget-header .ui-state-default {
      border: 1px solid #d3d3d3;
      background: #078fd7 50% 50% repeat-x;
      font-weight: normal;
      color: #555555;
    }

    .container {
      max-width: 1150px;
    }

    .searchPanelContainerDiv .chosen-container {
      position: relative !important;
    }


  </style>

  <script type="text/javascript">

    function openSmallWindowJS(address, windowname) {
      var rightNow = new Date();
      windowname += rightNow.getTime();
      var Place = window.open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1150,resizable=yes,toolbar=no,location=no,status=no");
    }

  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="">
    <ProgressTemplate>
      <div id="divLoading" runat="server" style="text-align: center; font-weight: bold; background-color: #eeeeee; filter: alpha(opacity=90); opacity: 0.9; width: 395px; height: 295px; text-align: center; padding: 75px; position: absolute; border: 1px solid #003957; z-index: 10; margin-left: 225px;">
        <span>Please wait ... </span>
        <br />
        <br />
        <img src="/images/loading.gif" alt="Loading..." /><br />
      </div>
    </ProgressTemplate>
  </asp:UpdateProgress>
  <asp:Panel runat="server" ID="contentClass" CssClass="valueViewPDFExport remove_padding">
    <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width="100%" runat="server"
      class="DetailsBrowseTable">
      <asp:TableRow>
        <asp:TableCell HorizontalAlign="right" VerticalAlign="middle">
              <div class="backgroundShade">
                <a href="#" onclick="javascript:window.close();" class="gray_button float_left"><strong>Close</strong></a>
              </div>
        </asp:TableCell>
      </asp:TableRow>
    </asp:Table>
    <div id="searchPanelContainerDiv" runat="server" width="1050">

    </div>

        <div id="DivLoadingMessage" style="display: none;">
    </div>

  </asp:Panel>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

  <script type="text/javascript">

    function ActiveTabChanged(sender, args) {

      var nextTab = sender.get_activeTab().get_id();

      if (nextTab.indexOf("tab1") > 0) {
        //alert("finder preferences");
        //swapChosenDropdowns();
      }

    }

    function ShowLoadingMessage(DivTag, Title, Message) {
      $("#" + DivTag).html(Message);
      $("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 75, resizable: false });
    }

    function CloseLoadingMessage(DivTag) {
      $("#" + DivTag).dialog("close");
    }

  </script>

</asp:Content>
