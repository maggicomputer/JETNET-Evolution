<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DisplayModelDetail.aspx.vb"
  Inherits="crmWebClient.DisplayModelDetail" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script language="javascript" type="text/javascript" src="https://www.google.com/jsapi?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE"></script>

  <script type="text/javascript">
    google.load('visualization', '1', { packages: ['corechart'] });
  </script>


  <script language="javascript" type="text/javascript">

    function openSmallWindowJS(address, windowname) {
      var rightNow = new Date();
      windowname += rightNow.getTime();
      var Place = window.open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1050,resizable=yes,toolbar=no,location=no,status=no");
      return true;
    }
       
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div id="DivModelMessage" style="display: none;"></div>
   <script language="javascript" type="text/javascript">
                 
     function ShowModelMessage(DivTag, Title, Message) {
       $("#" + DivTag).html(Message);
       $("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 55, resizable: false });
     }

     function CloseModelMessage(DivTag) {
       $("#" + DivTag).dialog("close");
     }

     //$(window).bind('load', function() {
       ShowModelMessage('DivModelMessage', 'Please Wait ...', 'Loading Model Insight Page ... Please Wait ...');
     //});

     $(document).ready(function() {
       CloseModelMessage("DivModelMessage");
     });
   
  </script>
    
  <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="" DisplayAfter="10">
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
  <div style="text-align: left;">
    <asp:UpdatePanel ID="model_display_panel" runat="server" ChildrenAsTriggers="True"
      UpdateMode="Conditional">
      <ContentTemplate>
        <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width='100%' runat="server"
          class="DetailsBrowseTable">
          <asp:TableRow>
            <asp:TableCell HorizontalAlign="right" VerticalAlign="middle">
              <div class="backgroundShade">
                <a href="#" class="gray_button float_left noBefore" target="_blank" id="know_more"
                  runat="server"><strong>Model Market Summary</strong></a> <a href="#" onclick="javascript:window.close();"
                    class="float_left"><img src="/images/x.svg" alt="Close" /></a></div>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
        <div class="NotesHeader">
        </div>
        <table width='100%' cellpadding='3' cellspacing='0' class='Main_Aircraft_Display_Table'>
          <tr>
            <td align="left" valign="top" width="60%">
             <asp:Label runat="server" Visible="false" ID="model_name_admin" Text="Aircraft Model Name:"></asp:Label> <asp:DropDownList ID="model_id_admin" runat="server" AutoPostBack="true" OnSelectedIndexChanged="model_id_admin_clicked" Visible="false" ></asp:DropDownList> 
              <cc1:TabContainer ID="information" runat="server" Visible="true" CssClass="dark-theme">
                <cc1:TabPanel ID="information_tab" runat="server" Visible="true" HeaderText="MODEL INFORMATION">
                  <ContentTemplate>
                    <asp:Label ID="information_label" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <cc1:TabContainer ID="internalNotes" runat="server" Visible="true" CssClass="dark-theme">
                <cc1:TabPanel ID="internalNotes_tab" runat="server" Visible="true" HeaderText="MODEL INSIGHT, COST FACTORS, & NOTES">
                  <ContentTemplate>
                    <asp:Label ID="internalNotes_label" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <cc1:TabContainer ID="engine" runat="server" Visible="true" CssClass="dark-theme">
                <cc1:TabPanel ID="engine_tab" runat="server" Visible="true" HeaderText="ENGINE">
                  <ContentTemplate>
                    <asp:Label ID="engine_label" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <cc1:TabContainer ID="maintenance" runat="server" Visible="true" CssClass="dark-theme">
                <cc1:TabPanel ID="maintenance_tab" runat="server" Visible="true" HeaderText="MAINTENANCE PROGRAMS">
                  <ContentTemplate>
                    <asp:Label ID="maintenance_label" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
            </td>
            <td align="left" valign="top">
              <asp:Label ID="picture_label" runat="server" Text=""></asp:Label>
              <cc1:TabContainer ID="operational" runat="server" Visible="true" CssClass="dark-theme">
                <cc1:TabPanel ID="operational_tab" runat="server" Visible="true" HeaderText="Operational Trends">
                  <ContentTemplate>
                    <asp:Label ID="operational_label" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <cc1:TabContainer ID="utilization" runat="server" Visible="true" CssClass="dark-theme">
                <cc1:TabPanel ID="utilization_tab" runat="server" Visible="true" HeaderText="MODEL UTILIZATION">
                  <ContentTemplate>
                    <asp:Label ID="utilization_label" runat="server" Text=""></asp:Label> 
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top" colspan="2">
              <cc1:TabContainer ID="features" runat="server" Visible="true" CssClass="dark-theme">
                <cc1:TabPanel ID="features_tab" runat="server" Visible="true" HeaderText="IMPORTANT FEATURE CODES">
                  <ContentTemplate>
                    <asp:Label ID="features_label" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <cc1:TabContainer ID="topics" runat="server" Visible="true" CssClass="dark-theme">
                <cc1:TabPanel ID="topics_tab" runat="server" Visible="true" HeaderText="TOPICS / ATTRIBUTES">
                  <ContentTemplate>
                    <asp:Label ID="topics_label" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <cc1:TabContainer ID="basic" runat="server" Visible="true" CssClass="dark-theme">
                <cc1:TabPanel ID="basic_tab" runat="server" Visible="true" HeaderText="BASIC CONFIGURATION">
                  <ContentTemplate>
                    <asp:Label ID="basic_label" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <cc1:TabContainer ID="costs" runat="server" Visible="true" CssClass="dark-theme">
                <cc1:TabPanel ID="costs_tab" runat="server" Visible="true" HeaderText="COSTS">
                  <ContentTemplate>
                    <asp:Label ID="costs_label" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <cc1:TabContainer ID="resources" runat="server" Visible="false" CssClass="dark-theme">
                <cc1:TabPanel ID="resources_tab" runat="server" Visible="true" HeaderText="RESOURCES">
                  <ContentTemplate>
                    <asp:Label ID="resources_label" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top" colspan="2">
              <cc1:TabContainer ID="sale" runat="server" Visible="true" CssClass="dark-theme">
                <cc1:TabPanel ID="sale_tab" runat="server" Visible="true" HeaderText="SALE PRICES">
                  <ContentTemplate>
                    <asp:Label ID="sale_label" runat="server" Text=""></asp:Label>
                    <br /><br />
                    <table width='100%'><tr><td width='50%'>
                    <asp:Label ID="assett_label" runat="server" Text=""></asp:Label>
                    </td><td width='50%'> 
                    <asp:Label ID="assett_label2" runat="server" Text=""></asp:Label>
                    </td></tr></table>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <cc1:TabContainer ID="dealer" runat="server" Visible="true" CssClass="dark-theme">
                <cc1:TabPanel ID="dealer_tab" runat="server" Visible="true" HeaderText="AIRCRAFT DEALERS">
                  <ContentTemplate>
                    <asp:Label ID="dealer_label" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <cc1:TabContainer ID="userInterest" runat="server" Visible="true" CssClass="dark-theme">
                <cc1:TabPanel ID="userInterest_tab" runat="server" Visible="true" HeaderText="USER INTEREST">
                  <ContentTemplate>
                    <asp:Label ID="userInterest_label" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <cc1:TabContainer ID="maintenanceDetails" runat="server" Visible="true" CssClass="dark-theme">
                <cc1:TabPanel ID="maintenanceDetails_tab" runat="server" Visible="true" HeaderText="MAINTENANCE DETAILS">
                  <ContentTemplate>
                    <asp:Label ID="maintenanceDetails_label" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
            </td>
          </tr>
        </table>
        <asp:Label runat="server" ID="no_model_text"></asp:Label>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div> 

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">
</asp:Content>
