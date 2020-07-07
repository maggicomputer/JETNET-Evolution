<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="home.aspx.vb" Inherits="crmWebClient.home"
  MasterPageFile="~/main_site.Master" EnableViewState="true" StylesheetTheme="Evo" %>

<%@ MasterType VirtualPath="~/main_site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <asp:Label runat="server" ID="evo_scripts">

    <style type="text/css">
      .ui-autocomplete-input {
        width: 99% !important;
      }

      .ui-autocomplete {
        padding: 0;
        list-style: none;
        background-color: #fff;
        width: 218px;
        border: 1px solid #B0BECA;
        max-height: 350px;
        overflow-y: scroll;
      }

        .ui-autocomplete .ui-menu-item a {
          border-top: 1px solid #B0BECA;
          display: block;
          padding: 6px 6px;
          color: #353D44;
          cursor: pointer; /*margin-bottom: -11px;*/
        }

        .ui-autocomplete .ui-menu-item:first-child a {
          border-top: none;
        }

        .ui-autocomplete .ui-menu-item a.ui-state-hover {
          background-color: #D5E5F4;
          color: #161A1C;
        }

      A.underline {
        font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
        text-decoration: underline;
        cursor: pointer;
      }
    </style>

    <script type="text/javascript">

      function openSmallWindowJS(address, windowname) {
        var rightNow = new Date();
        windowname += rightNow.getTime();
        var Place = open(address, windowname, "menubar,scrollbars=1,resizable,width=1150,height=600");
      }

      function drawVisualization() {

        var options = {
          curveType: 'function',
          width: 295, height: 300,
          vAxis: { title: "Clicks", minValue: -1 },
          legend: { position: 'top' }
        };

        var chart = new google.visualization.LineChart(document.getElementById('visualization'));
        chart.draw(data, options);

      }

      function SetUpAutoComplete() {
        $('#<%= ModelDynamic.ClientID %>').selectToAutocomplete();
        $('#<%= searchAircraft.ClientID %>').click(function () {
          PopulateModelID()
          //return false;
        });
        $('#<%= searchCompany.ClientID %>').click(function () {
          PopulateModelID()
          //return false;
        });
      }

      function PopulateModelID() {
        var answerString = $('#<%= ModelDynamic.ClientID %>').serialize();
        //alert(answerString);
        var answerArray = answerString.split("=");
        var ModelIDBox = document.getElementById("<%= ___amod_id.ClientID %>");
        if (ModelIDBox != null) {
          document.getElementById("<%= ___amod_id.ClientID %>").value = answerArray[1];
        }
      }

      /*These Functions are going to take the place of up above. I wanted one nice function that would do this no matter what boxes we were trying to fill, so it could be used on both the yacht or aircraft side.*/
      function SetUpVariableAutoComplete(jqueryModelDropdownID, jquerySearchButtonFirstID, jquerySearchButtonSecondID, jqueryModelTextboxID) {
        $(jqueryModelDropdownID).selectToAutocomplete();
        $(jquerySearchButtonFirstID).click(function () {
          PopulateVariableModelID(jqueryModelDropdownID, jqueryModelTextboxID)
        });
        $(jquerySearchButtonSecondID).click(function () {
          PopulateVariableModelID(jqueryModelDropdownID, jqueryModelTextboxID)
        });

      }

      function PopulateVariableModelID(jqueryModelDropdownID, jqueryModelTextboxID) {
        var answerString = $(jqueryModelDropdownID).serialize();
        var answerArray = answerString.split("=");
        var ModelIDBox = document.getElementById(jqueryModelTextboxID);
        if (ModelIDBox != null) {
          document.getElementById(jqueryModelTextboxID).value = answerArray[1];
        }
      }

      function refreshHome() {
      <%= PostBackStr.ToString %>;
      }

    </script>
  </asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:Panel runat="server" Visible="true" ID="crmPanelVisibility">
    <asp:UpdateProgress ID="UpdateProgress3" AssociatedUpdatePanelID="crm_update_panel"
      runat="server" DisplayAfter="5">
      <ProgressTemplate>
        <div id="Div1" runat="server" class="loadingScreenUpdatePanel">
          <br />
          <br />
          <img src="Images/loading.gif" alt="Loading..." /><br />
        </div>
      </ProgressTemplate>
    </asp:UpdateProgress>
    <table width="100%" cellpadding="5" cellspacing="0">
      <tr>
        <td rowspan="3" valign="top">
          <asp:UpdatePanel runat="server" ID="crm_update_panel" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <ContentTemplate>
              <cc1:TabContainer runat="server" ID="crm_tab" Width="100%" Visible="true" CssClass="dark-theme"
                AutoPostBack="true" OnClientActiveTabChanged="LargeTabActiveTabChanged">
                <cc1:TabPanel ID="crm_action_panel" runat="server" Visible="true">
                  <HeaderTemplate>
                    Action Items
                  </HeaderTemplate>
                  <ContentTemplate>
                    <div class="padding">
                      <asp:Panel runat="server" BackColor="#184D7B" ForeColor="White" Font-Size="14pt"
                        Height="35px" CssClass="no_pad">
                        <asp:Label ID="today_date" runat="server" Text="Most Recently Edited Companies" CssClass="today_date"></asp:Label>
                      </asp:Panel>
                      <asp:Label runat="server" ID="demo_attention_label" Font-Bold="True" ForeColor="Red"
                        Font-Size="Medium" Visible="False"><p align="center">Please note that this is a demonstration account only.</p> <p align="center"> All data under this account may be viewed by other demonstration users and may be erased at any time.</p></asp:Label>
                      <br />
                      <asp:Panel runat="server" BackColor="#C8DAF0" ForeColor="Black" Font-Size="11pt"
                        Height="25px" ID="calendar_panel">
                        <asp:Panel runat="server" ID="timeframepanel" CssClass="float_right margin_4">
                          <asp:DropDownList runat="server" ID="crm_calendar_timeframe" CssClass="float_right"
                            AutoPostBack="True">
                            <asp:ListItem Value="1">Next Day</asp:ListItem>
                            <asp:ListItem Selected="True" Value="7">Next Week</asp:ListItem>
                            <asp:ListItem Value="31">Next Month</asp:ListItem>
                          </asp:DropDownList>
                          <asp:Label ID="crm_calendar_timeframe_label" runat="server" CssClass="padding float_right">Show Actions Through:</asp:Label>
                        </asp:Panel>
                        <asp:Label ID="main_calendar" runat="server" Text="Calendar" CssClass="today_calendar"></asp:Label>
                      </asp:Panel>
                      <asp:Label ID="main_calendar_txt" runat="server"></asp:Label>
                    </div>
                  </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="crm_market_overview_panel" runat="server" Visible="true">
                  <HeaderTemplate>
                    Market Overview
                  </HeaderTemplate>
                  <ContentTemplate>
                    <div class="padding">
                      <asp:Label ID="crm_market_overview" runat="server"></asp:Label>
                      <asp:Button runat="server" ID="toggleSales" Text="Show Pre-Owned Sales Summary" Visible="false" />
                    </div>
                  </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="crm_event_panel" runat="server">
                  <HeaderTemplate>
                    Events (Recent)
                  </HeaderTemplate>
                  <ContentTemplate>
                    <asp:UpdatePanel runat="server" ID="crm_event_update_panel" UpdateMode="Conditional"
                      ChildrenAsTriggers="false">
                      <ContentTemplate>
                        <asp:Panel runat="server" ID="crm_time_panel" CssClass="display_none light_seafoam_green_header_color">
                          <table width="100%" cellpadding="3" cellspacing="0">
                            <tr>
                              <td align="right" valign="middle" width="50">
                                <asp:Label ID="Label2" runat="server" Font-Size="9px">Range:</asp:Label>
                              </td>
                              <td align="left" valign="top">
                                <asp:RadioButtonList ID="crm_event_time" Visible="true" RepeatColumns="4" CellPadding="3"
                                  runat="server" RepeatLayout="Table" AutoPostBack="true" Font-Size="9px">
                                  <asp:ListItem Value="1">One Day</asp:ListItem>
                                  <asp:ListItem Selected="True" Value="7">One Week</asp:ListItem>
                                  <asp:ListItem Value="30">One Month</asp:ListItem>
                                  <asp:ListItem Value="90">Three Months</asp:ListItem>
                                </asp:RadioButtonList>
                              </td>
                            </tr>
                            <tr>
                              <td align="right" valign="middle">
                                <asp:Label ID="Label3" runat="server" Font-Size="9px">Category:</asp:Label>
                              </td>
                              <td align="left" valign="top">
                                <asp:RadioButtonList ID="crm_event_category" Visible="true" RepeatColumns="6" CellPadding="3"
                                  runat="server" RepeatLayout="flow" AutoPostBack="true" Font-Size="9px">
                                  <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                </asp:RadioButtonList>
                              </td>
                            </tr>
                          </table>
                        </asp:Panel>
                        <asp:Label ID="crm_event_listing" runat="server" Text=""></asp:Label>
                      </ContentTemplate>
                    </asp:UpdatePanel>
                  </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="crm_wanteds_panel" runat="server">
                  <HeaderTemplate>
                    Wanteds (Recent)
                  </HeaderTemplate>
                  <ContentTemplate>
                    <asp:UpdatePanel runat="server" ID="crm_wanted_update_panel" UpdateMode="Conditional"
                      ChildrenAsTriggers="false">
                      <ContentTemplate>
                        <asp:Label ID="crm_wanted_label" runat="server" Text=""></asp:Label>
                      </ContentTemplate>
                    </asp:UpdatePanel>
                  </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="crm_user_activity_panel" runat="server">
                  <HeaderTemplate>
                    User Activity
                  </HeaderTemplate>
                  <ContentTemplate>
                    <asp:Label ID="user_activity_label" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="crm_client_db_panel" runat="server">
                  <HeaderTemplate>
                    Client Database
                  </HeaderTemplate>
                  <ContentTemplate>
                    <asp:Label ID="client_database_label" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
            </ContentTemplate>
          </asp:UpdatePanel>
        </td>
        <td valign="top" width="25%">
          <asp:Panel runat="server" ID="home_right_visible" BackColor="White">
            <asp:Label ID="home_companies_txt" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="home_contacts_txt" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="home_aircraft_txt" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="home_notes_txt" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="home_documents_txt" runat="server" Text=""></asp:Label>
            <br />
          </asp:Panel>
        </td>
      </tr>
    </table>
  </asp:Panel>
  <asp:Table ID="evo_display_table" runat="server" CellPadding="2" CellSpacing="0"
    Width="100%" CssClass="evo_display_table">
    <asp:TableHeaderRow>
      <asp:TableCell VerticalAlign="Top">
        <asp:UpdatePanel runat="server" ID="main_home_update_panel" UpdateMode="Conditional"
          ChildrenAsTriggers="false">
          <ContentTemplate>
            <cc1:TabContainer runat="server" ID="main_home_tab_container" Width="100%" ActiveTabIndex="0"
              CssClass="dark-theme" OnClientActiveTabChanged="LargeTabActiveTabChanged" AutoPostBack="true">
              <cc1:TabPanel ID="market_summary_tab" runat="server" HeaderText="Market Overview"
                Visible="false">
                <ContentTemplate>
                  <asp:Label runat="server" ID="market_load">
                    <div id="Div1" runat="server" class="loadingScreenPage home_page_margin">
                      <span>Please wait while the
                        <asp:Label runat="server" ID="market_overview_tab_label">Market Overview</asp:Label>
                        Tab is loading... </span>
                      <br />
                      <br />
                      <img src="Images/loading.gif" alt="Loading..." /><br />
                    </div>
                  </asp:Label>
                  <asp:Label ID="market_listing_label" runat="server" Text=""></asp:Label>
                  <asp:Button runat="server" ID="toggleSalesEvo" Text="Show Pre-Owned Sales Summary"
                    Visible="false" OnClientClick="javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" />
                </ContentTemplate>
              </cc1:TabPanel>
              <cc1:TabPanel ID="market_activity_tab" runat="server" HeaderText="Recent Events"
                Visible="false">
                <ContentTemplate>
                  <asp:Label runat="server" ID="events_load">
                    <div id="Div2" runat="server" class="loadingScreenPage home_page_margin">
                      <span>Please wait while the Events Tab is loading... </span>
                      <br />
                      <br />
                      <img src="Images/loading.gif" alt="Loading..." /><br />
                    </div>
                  </asp:Label>
                  <asp:Panel runat="server" ID="event_time_panel" CssClass="display_none light_seafoam_green_header_color toggleSmallScreen">
                    <table width="100%" cellpadding="0" cellspacing="0">
                      <tr>
                        <td align="right" valign="middle" width="50">
                          <asp:Label runat="server" Font-Size="9px" Font-Bold="true">Range:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                          <asp:RadioButtonList ID="event_time" Visible="true" RepeatColumns="3" CellPadding="3"
                            runat="server" RepeatLayout="Table" AutoPostBack="true" Font-Size="9px">
                            <asp:ListItem Selected="True" Value="7">One Week</asp:ListItem>
                            <asp:ListItem Value="30">One Month</asp:ListItem>
                            <asp:ListItem Value="90">Three Months</asp:ListItem>
                          </asp:RadioButtonList>
                        </td>
                      </tr>
                      <tr>
                        <td align="right" valign="middle">
                          <asp:Label ID="Label1" runat="server" Font-Size="9px" Font-Bold="true">Category:</asp:Label>
                        </td>
                        <td align="left" valign="top">
                          <asp:RadioButtonList ID="event_category" Visible="true" RepeatColumns="6" CellPadding="3"
                            runat="server" RepeatLayout="table" AutoPostBack="true" Font-Size="9px">
                            <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                          </asp:RadioButtonList>
                        </td>
                      </tr>
                    </table>
                  </asp:Panel>
                  <asp:Label ID="event_listing_label" runat="server" Text=""></asp:Label>
                </ContentTemplate>
              </cc1:TabPanel>
              <cc1:TabPanel ID="wanted_tab" runat="server" Visible="false" HeaderText="Recent Wanteds">
                <ContentTemplate>
                  <asp:Label runat="server" ID="wanteds_load">
                    <div id="Div5" runat="server" class="loadingScreenPage home_page_margin">
                      <span>Please wait while the Wanteds Tab is loading... </span>
                      <br />
                      <br />
                      <img src="Images/loading.gif" alt="Loading..." /><br />
                    </div>
                  </asp:Label>
                  <asp:Label ID="wanted_listing_label" runat="server" Text=""></asp:Label>
                  <asp:DataGrid runat="server" ID="wanted_results" AutoGenerateColumns="false" Width="100%"
                    AllowCustomPaging="false" AllowPaging="true" Visible="false">
                    <Columns>
                      <asp:TemplateColumn HeaderText="MAKE">
                        <ItemTemplate>
                          <%#DataBinder.Eval(Container.DataItem, "amod_make_name").ToString%>
                          <%#DataBinder.Eval(Container.DataItem, "amod_model_name").ToString%>
                        </ItemTemplate>
                      </asp:TemplateColumn>
                      <asp:TemplateColumn HeaderText="DATE LISTED">
                        <ItemTemplate>
                          <a href="#" onclick="javascript:load('WantedDetails.aspx?id=<%#DataBinder.Eval(Container.DataItem, "amwant_id").ToString%>','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;">
                            <%#crmWebClient.clsGeneral.clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "amwant_listed_date").ToString)%></a>
                        </ItemTemplate>
                      </asp:TemplateColumn>
                      <asp:TemplateColumn HeaderText="INTERESTED PARTY">
                        <ItemTemplate>
                          <%#crmWebClient.DisplayFunctions.WriteDetailsLink(0, DataBinder.Eval(Container.DataItem, "comp_id"), 0, 0, True, DataBinder.Eval(Container.DataItem, "comp_name").ToString, "", "")%>
                        </ItemTemplate>
                      </asp:TemplateColumn>
                      <asp:TemplateColumn HeaderText="YEAR RANGE">
                        <ItemTemplate>
                          <%#DataBinder.Eval(Container.DataItem, "amwant_start_year").ToString%>-<%#DataBinder.Eval(Container.DataItem, "amwant_end_year").ToString%>
                        </ItemTemplate>
                      </asp:TemplateColumn>
                      <asp:TemplateColumn HeaderText="MAX PRICE">
                        <ItemTemplate>
                          <%#crmWebClient.clsGeneral.clsGeneral.no_zero(DataBinder.Eval(Container.DataItem, "amwant_max_price").ToString, "", True)%>
                        </ItemTemplate>
                      </asp:TemplateColumn>
                      <asp:TemplateColumn HeaderText="MAX AFTT">
                        <ItemTemplate>
                          <%#DataBinder.Eval(Container.DataItem, "amwant_max_aftt").ToString%>
                        </ItemTemplate>
                      </asp:TemplateColumn>
                      <asp:TemplateColumn HeaderText="DAMAGE">
                        <ItemTemplate>
                        </ItemTemplate>
                      </asp:TemplateColumn>
                    </Columns>
                  </asp:DataGrid>
                </ContentTemplate>
              </cc1:TabPanel>
              <cc1:TabPanel ID="MyAnalytics" runat="server" HeaderText="MyAnalytics">
                <ContentTemplate>
                  <asp:Label runat="server" ID="analytics_load">
                    <div id="Div3" runat="server" class="loadingScreenPage home_page_margin">
                      <span>Please wait while the Analytics Tab is loading... </span>
                      <br />
                      <br />
                      <img src="Images/loading.gif" alt="Loading..." /><br />
                    </div>
                  </asp:Label>
                  <asp:Label ID="MyAnalytics_listing_label" runat="server" Text=""></asp:Label>
                </ContentTemplate>
              </cc1:TabPanel>
              <cc1:TabPanel ID="quick_search_tab" runat="server" HeaderText="Quick Search">
                <ContentTemplate>
                  <asp:Panel runat="server" ID="searchControls" DefaultButton="searchAircraft">
                    <table width="100%" cellpadding="3" cellspacing="0" class="data_aircraft_grid override_borders">
                      <tr class="header_row">
                        <td align="left" valign="top" width="50%" id="AircraftQuickSearchHeader">
                          <b>Aircraft</b>
                        </td>
                        <td align="left" valign="top" width="50%" id="CompanyQuickSearchHeader">
                          <b>Company</b>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top" id="AircraftQuickSearchCell">
                          <table width="100%" cellpadding="3" cellspacing="0">
                            <tr>
                              <td align="left" valign="top">Make Model:
                                <img src="../images/magnify_small.png" class="help_cursor" border="0" width="9" alt="Type characters describing the aircraft make and model that you desire and Evolution will provide you with a list of models that match your needs. Note that Quick Search only allows for one model selection at a time."
                                  title="Type characters describing the aircraft make and model that you desire and Evolution will provide you with a list of models that match your needs. Note that Quick Search only allows for one model selection at a time." />
                              </td>
                              <td align="left" valign="top" colspan="3">
                                <asp:TextBox ID="___amod_id" runat="server" CssClass="display_none"></asp:TextBox>
                                <div class="ui-widget">
                                  <asp:DropDownList runat="server" name="ModelDynamic" ID="ModelDynamic" autofocus="autofocus"
                                    autocorrect="off" autocomplete="off">
                                  </asp:DropDownList>
                                </div>
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top">
                                <a href="#" onclick="javascript:load('MasterLists.aspx?helplist=serial','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');">Serial #</a>:
                              </td>
                              <td align="left" valign="top" width="28%">
                                <asp:TextBox ID="___ac_ser_no_from" runat="server" Width="100%"></asp:TextBox>
                              </td>
                              <td align="right" valign="top" width="20%">
                                <a href="#" onclick="javascript:load('MasterLists.aspx?helplist=registration','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');">Reg #</a>:
                              </td>
                              <td align="left" valign="top" width="28%">
                                <asp:TextBox ID="___ac_reg_no" runat="server" Width="100%"></asp:TextBox>
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top">IATA:
                              </td>
                              <td align="left" valign="top">
                                <asp:TextBox ID="___ac_aport_iata_code" runat="server" Width="100%"></asp:TextBox>
                              </td>
                              <td align="right" valign="top">ICAO:
                              </td>
                              <td align="left" valign="top">
                                <asp:TextBox ID="___ac_aport_icao_code" runat="server" Width="100%"></asp:TextBox>
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top">
                                <asp:Label runat="server" ID="market_status_label">Market Status:</asp:Label>
                              </td>
                              <td align="left" valign="top" colspan="3">
                                <asp:DropDownList ID="___market" runat="server" Width="100%">
                                  <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                  <asp:ListItem Value="For Sale">For Sale</asp:ListItem>
                                  <asp:ListItem Value="For Sale/Lease">For Sale/Lease</asp:ListItem>
                                  <asp:ListItem Value="For Sale/Trade">For Sale/Trade</asp:ListItem>
                                  <asp:ListItem Value="For Sale on Exclusive">For Sale on Exclusive</asp:ListItem>
                                  <asp:ListItem Value="For Sale Not on Exclusive">For Sale Not on Exclusive</asp:ListItem>
                                  <asp:ListItem Value="Not For Sale">Not For Sale</asp:ListItem>
                                  <asp:ListItem Value="Lease">Lease</asp:ListItem>
                                </asp:DropDownList>
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top">Details/Avionics
                              </td>
                              <td align="left" valign="top" colspan="3">
                                <asp:TextBox ID="___attributeID" runat="server" CssClass="display_none"></asp:TextBox>
                                <div class="ui-widget">
                                  <asp:TextBox ID="___generic_data_description" runat="server" Width="100%"></asp:TextBox>
                                </div>
                              </td>
                            </tr>
                          </table>
                        </td>
                        <td align="left" valign="top" class="gray_background_color" id="CompanyQuickSearchCell">
                          <table width="100%" cellpadding="3" cellspacing="0">
                            <tr>
                              <td align="left" valign="top" width="130">Company Name:
                              </td>
                              <td align="left" valign="top">
                                <asp:TextBox ID="company_name___comp_name" runat="server" Width="100%"></asp:TextBox>
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top">Contact Name (First/Last):
                              </td>
                              <td align="left" valign="top">
                                <asp:TextBox ID="company_contact_first___contact_first_name" runat="server" Width="45%"
                                  CssClass="float_left"></asp:TextBox>
                                <asp:TextBox ID="company_contact_last___contact_last_name" runat="server" Width="45%"
                                  CssClass="float_right"></asp:TextBox>
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top">Email Address:
                              </td>
                              <td align="left" valign="top">
                                <asp:TextBox ID="company_email_address___comp_email_address" runat="server" Width="100%"></asp:TextBox>
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top">Relationship To Aircraft:
                              </td>
                              <td align="left" valign="top">
                                <asp:DropDownList ID="company_relationship___cref_contact_type" runat="server" Width="100%">
                                </asp:DropDownList>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                    </table>
                  </asp:Panel>
                  <table width="100%" cellpadding="3" cellspacing="0">
                    <tr>
                      <td align="left" valign="top" width="50%" id="AircraftQuickSearchButton">
                        <asp:LinkButton ID="searchAircraft" runat="server" CssClass="gray_button float_right"
                          OnClientClick="document.body.style.cursor='wait';">Search Aircraft</asp:LinkButton>
                      </td>
                      <td align="left" valign="top" width="50%" id="CompanyQuickSearchButton">
                        <asp:LinkButton ID="searchCompany" runat="server" CssClass="gray_button float_right">Search Company</asp:LinkButton>
                      </td>
                    </tr>
                  </table>
                </ContentTemplate>
              </cc1:TabPanel>
              <cc1:TabPanel ID="action_item_tab" runat="server" HeaderText="Action Items">
                <ContentTemplate>
                  <asp:Label runat="server" ID="actions_load">
                    <div id="Div4" runat="server" class="loadingScreenPage home_page_margin">
                      <span>Please wait while the Action Items Tab is loading... </span>
                      <br />
                      <br />
                      <img src="Images/loading.gif" alt="Loading..." /><br />
                    </div>
                  </asp:Label>
                  <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                      <td align="right" valign="middle" width="50">
                        <asp:Label ID="Label6" runat="server" Font-Size="9px" Font-Bold="true">Range:</asp:Label>
                      </td>
                      <td align="left" valign="top">
                        <asp:RadioButtonList ID="action_time" Visible="true" RepeatColumns="3" CellPadding="3"
                          runat="server" RepeatLayout="Table" AutoPostBack="true" Font-Size="9px" CssClass="actionItemRadio">
                          <asp:ListItem Selected="True" Value="5">Next 5 Days</asp:ListItem>
                          <asp:ListItem Value="14">Next 14 Days</asp:ListItem>
                          <asp:ListItem Value="30">Next 30 Days</asp:ListItem>
                        </asp:RadioButtonList>
                      </td>
                    </tr>
                  </table>
                  <asp:Label ID="evo_action_items" runat="server" Text=""></asp:Label>
                </ContentTemplate>
              </cc1:TabPanel>
              <cc1:TabPanel ID="reports_tab" runat="server" HeaderText="Reports" Visible="false">
                <ContentTemplate>
                  <asp:Label runat="server" ID="custom_reports_results">
                  </asp:Label>
                  <asp:Label ID="custom_reports_label" runat="server" Text=""></asp:Label>
                </ContentTemplate>
              </cc1:TabPanel>
              <cc1:TabPanel ID="index_tab" runat="server" HeaderText="Attributes" Visible="false">
                <ContentTemplate>
                  <div id="index_wait_div" runat="server" class="loadingScreenPage home_page_margin">
                    <span>Please wait while the Attributes Tab is loading... </span>
                    <br />
                    <br />
                    <img src="Images/loading.gif" alt="Loading..." /><br />
                  </div>
                  <asp:Label ID="index_tab_label" runat="server" CssClass="display_none"></asp:Label>
                  <asp:Panel runat="server" ID="indexPanel" CssClass="display_none">
                  </asp:Panel>
                </ContentTemplate>
              </cc1:TabPanel>
              <cc1:TabPanel ID="airport_tab" runat="server" HeaderText="MyAirports" Visible="false">
                <ContentTemplate>
                  <asp:Label runat="server" ID="MyAirports_Load" Visible="true">
                    <div id="Div4_MyAirports" runat="server" class="loadingScreenPage home_page_margin">
                      <span>Please wait while the MyAirports Tab is loading... </span>
                      <br />
                      <br />
                      <img src="Images/loading.gif" alt="Loading..." /><br />
                    </div>
                  </asp:Label>
                  <asp:Label ID="my_airports_label" Text="" runat="server" Visible="false"></asp:Label>
                  <asp:DropDownList ID="months_choice" runat="server" Visible="false" AutoPostBack="true">
                    <asp:ListItem Value="MTD">Current Month to Date</asp:ListItem>
                    <asp:ListItem Value="YTD">Current Year to Date</asp:ListItem>
                    <asp:ListItem Value="1" Selected="True">1 Month</asp:ListItem>
                    <asp:ListItem Value="3">3 Months</asp:ListItem>
                    <asp:ListItem Value="6">6 Months</asp:ListItem>
                    <asp:ListItem Value="12">12 Months</asp:ListItem>
                    <asp:ListItem Value="24">24 Months</asp:ListItem>
                  </asp:DropDownList>
                  <div runat="server" id="div_airports_label_table">
                    <div style="text-align: center; width: 100%;" runat="server" id="airportsResults">
                      <asp:Label ID="airportsTable" runat="server" Text=""></asp:Label>
                    </div>
                  </div>
                </ContentTemplate>
              </cc1:TabPanel>
              <cc1:TabPanel ID="my_mpm_tabpanel" runat="server" HeaderText="MyMPM" Visible="false">
                <ContentTemplate>
                  <div id="mympm_div" runat="server" class="loadingScreenPage home_page_margin">
                    <span>Please wait while the MyMPM are loading... </span>
                    <br />
                    <br />
                    <img src="Images/loading.gif" alt="Loading..." /><br />
                  </div>
                  <asp:Label ID="mympm_label" runat="server"><p>&nbsp;</p></asp:Label>
                </ContentTemplate>
              </cc1:TabPanel>
              <cc1:TabPanel ID="folder_events_tab" runat="server" HeaderText="Events" Visible="false">
                <ContentTemplate>
                  <div id="event_folder_div" runat="server" class="loadingScreenPage home_page_margin">
                    <span>Please wait while the Event Folders are loading... </span>
                    <br />
                    <br />
                    <img src="Images/loading.gif" alt="Loading..." /><br />
                  </div>
                  <asp:Label ID="folder_events_tab_text" runat="server" CssClass="padding"><p>&nbsp;</p></asp:Label>
                </ContentTemplate>
              </cc1:TabPanel>
            </cc1:TabContainer>
          </ContentTemplate>
        </asp:UpdatePanel>
      </asp:TableCell>
      <asp:TableCell Width="300px" VerticalAlign="top" CssClass="mobile_display_off_cell">
        <cc1:TabContainer runat="server" ID="small_home_container_tab" Width="100%" ActiveTabIndex="0"
          CssClass="dark-theme" OnClientActiveTabChanged="SmallTabActiveTabChanged">
          <cc1:TabPanel ID="recent_activity_panel" runat="server" HeaderText="Recent Activity">
            <ContentTemplate>
              <asp:Label ID="recent_aircraft_activity_evo" runat="server" Text=""></asp:Label>
              <asp:TreeView ID="aircraft_recent" runat="server" SkinID="project_recent_view" CssClass="vertical_align_top tiny_text aircraft_folder"
                Visible="false" NodeWrap="true">
              </asp:TreeView>
              <asp:TreeView ID="company_recent" runat="server" SkinID="project_recent_view" Visible="false"
                NodeWrap="true" CssClass="vertical_align_top tiny_text aircraft_folder">
              </asp:TreeView>
              <asp:TreeView ID="contact_recent" runat="server" SkinID="project_recent_view" Visible="false"
                NodeWrap="true" CssClass="vertical_align_top tiny_text aircraft_folder">
              </asp:TreeView>
              <br class="div_clear" />
            </ContentTemplate>
          </cc1:TabPanel>
          <cc1:TabPanel ID="projects_panel" runat="server" HeaderText="Folders">
            <ContentTemplate>
              <asp:UpdatePanel runat="server" ID="folder_update_panel" ChildrenAsTriggers="true"
                UpdateMode="Conditional">
                <ContentTemplate>
                  <div class="small_subbar">
                    <a href="#" onclick="javascript:load('/help/helpexamples/340.pdf ','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"
                      class="red_button help_button">
                      <img src="images/info_white.png" border="0" width="13" alt="" /></a>
                    <asp:CheckBox runat="server" ID="show_hidden_folders" Text="Show Hidden Folders?"
                      AutoPostBack="true" OnCheckedChanged="change_hidden_folder" EnableViewState="true"
                      CssClass="tiny_text margin_right white_text float_right" onclick="createCookie('hideHidden', this.checked, 1);" />
                    <asp:CheckBox ID="hide_shared" runat="server" Text="Hide Shared Folders?" AutoPostBack="true"
                      OnCheckedChanged="change_hidden_folder" EnableViewState="true" CssClass="tiny_text margin_right white_text float_right" onclick="createCookie('hideShared', this.checked, 1);" />
                  </div>
                  <asp:Panel runat="server" ID="aircraft_folder_container" CssClass="aircraft_folder">
                    <a href="FolderMaintenance.aspx?t=3" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <a href="FolderMaintenance.aspx?t=3&newStaticFolder=true&fromHome=true" target="new"
                      class="float_right margin_right tiny_text">New<img src="images/newsearch.png" alt="New"
                        border="0" class="padding_left" /></a>
                    <asp:TreeView ID="aircraft_projects" runat="server" SkinID="project_tree_view" Width="250px">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Panel runat="server" ID="history_folder_container" CssClass="aircraft_folder">
                    <a href="FolderMaintenance.aspx?t=8" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <asp:TreeView ID="history_projects" runat="server" SkinID="project_tree_view">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Panel runat="server" ID="company_folder_container" CssClass="aircraft_folder">
                    <a href="FolderMaintenance.aspx?t=1" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <a href="FolderMaintenance.aspx?t=1&newStaticFolder=true&fromHome=true" target="new"
                      class="float_right margin_right tiny_text">New<img src="images/newsearch.png" alt="New"
                        border="0" class="padding_left" /></a>
                    <asp:TreeView ID="company_projects" runat="server" SkinID="project_tree_view">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Panel runat="server" ID="contact_folder_container" CssClass="aircraft_folder">
                    <a href="FolderMaintenance.aspx?t=2" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <asp:TreeView ID="contact_projects" runat="server" SkinID="project_tree_view">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Panel runat="server" ID="event_folder_container" CssClass="aircraft_folder">
                    <a href="FolderMaintenance.aspx?t=5" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <asp:TreeView ID="event_projects" runat="server" SkinID="project_tree_view">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Panel runat="server" ID="wanted_folder_container" CssClass="aircraft_folder">
                    <a href="FolderMaintenance.aspx?t=9" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <asp:TreeView ID="wanted_projects" runat="server" SkinID="project_tree_view">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Panel runat="server" ID="performance_specs_folder_container" CssClass="aircraft_folder">
                    <a href="FolderMaintenance.aspx?t=12" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <asp:TreeView ID="performance_specs_projects" runat="server" SkinID="project_tree_view">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Panel runat="server" ID="operating_costs_folder_container" CssClass="aircraft_folder">
                    <a href="FolderMaintenance.aspx?t=11" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <asp:TreeView ID="operating_costs_projects" runat="server" SkinID="project_tree_view">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Panel runat="server" ID="marketing_summary_folder_container" CssClass="aircraft_folder">
                    <a href="FolderMaintenance.aspx?t=13" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <asp:TreeView ID="marketing_summary_projects" runat="server" SkinID="project_tree_view">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Panel runat="server" ID="airport_folder_container" CssClass="aircraft_folder">
                    <a href="FolderMaintenance.aspx?t=17" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <a href="FolderMaintenance.aspx?t=17&newStaticFolder=true&fromHome=true" target="new"
                      class="float_right margin_right tiny_text">New<img src="images/newsearch.png" alt="New"
                        border="0" class="padding_left" /></a>
                    <asp:TreeView ID="airport_projects" runat="server" SkinID="project_tree_view">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Panel runat="server" ID="values_folder_container" CssClass="aircraft_folder display_none">
                    <a href="FolderMaintenance.aspx?t=16" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <asp:TreeView ID="values_projects" runat="server" SkinID="project_tree_view">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Label ID="no_projects" runat="server" Text="" Visible="false"><br /><p align="center">There are no current projects.</p></asp:Label>
                  <br class="div_clear" />
                </ContentTemplate>
              </asp:UpdatePanel>
            </ContentTemplate>
          </cc1:TabPanel>
          <cc1:TabPanel ID="chat_panel" runat="server" HeaderText="Community Chat">
            <ContentTemplate>
              <div class="padding" id="userSearch" style="display: none;">
                <input type="text" class="text-input" id="filter" value="" size="50" placeholder="Search by Name, Email, or Company to find Members" />
              </div>
              <div id="userHeader" style="display: none;">
                Search Results: Click on the user's name to select. <span id="filter-count" class="float_right emphasis_text"></span>
              </div>
              <asp:Label runat="server" ID="labelListOfUsers" Visible="false"></asp:Label>
              <div id="divCommunityListLbl" style="display: none; text-align: left; vertical-align: middle; padding-left: 8px;">
                <strong>My JETNET Community Online</strong>
              </div>
              <div id="divCommunityList" style="display: none; height: 220px; width: 100%; overflow: auto; vertical-align: top;">
                <p>
                  <table id="tblChatUsers" cellpadding="3" cellspacing="0" border="0" width="100%"
                    style="width: 100%;">
                  </table>
                </p>
              </div>
              <div id="divEnableChat" style="display: none; text-align: left; vertical-align: middle; padding: 3px;">
              </div>
            </ContentTemplate>
          </cc1:TabPanel>
        </cc1:TabContainer>
      </asp:TableCell>
    </asp:TableHeaderRow>
  </asp:Table>
  <asp:Table ID="yacht_display_table" runat="server" Visible="false" CellPadding="2"
    CellSpacing="0" Width="100%" CssClass="evo_display_table">
    <asp:TableHeaderRow>
      <asp:TableCell VerticalAlign="Top">
        <asp:UpdatePanel runat="server" ID="yacht_update_panel" UpdateMode="Conditional"
          ChildrenAsTriggers="false">
          <ContentTemplate>
            <cc1:TabContainer runat="server" ID="yacht_summary_tab" Width="100%" ActiveTabIndex="0"
              CssClass="dark-theme" OnClientActiveTabChanged="LargeTabActiveTabChanged" AutoPostBack="true">
              <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Summary">
                <ContentTemplate>
                  <asp:Label ID="yacht_summary_label" runat="server" Text=""></asp:Label>
                </ContentTemplate>
              </cc1:TabPanel>
              <cc1:TabPanel ID="yacht_quick_search_tab" runat="server" HeaderText="Quick Search">
                <ContentTemplate>
                  <asp:Panel runat="server" ID="yacht_search_control_panel">
                    <table width="100%" cellpadding="3" cellspacing="0" class="data_aircraft_grid override_borders">
                      <tr class="header_row">
                        <td align="left" valign="top" width="50%">
                          <b>Yacht</b>
                        </td>
                        <td align="left" valign="top" width="50%">
                          <b>Company</b>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top">
                          <table width="100%" cellpadding="3" cellspacing="0">
                            <tr class="display_none">
                              <td align="left" valign="top" width="140">Brand/Model:<img src="../images/magnify_small.png" class="padding_left help_cursor"
                                border="0" width="9" alt="Type characters describing the Yacht Brand/Model that you desire and Yacht Spot will provide you with a list of models that match your needs. Note that Quick Search only allows for one model selection at a time."
                                title="Type characters describing the Yacht Brand/Model that you desire and Yacht Spot will provide you with a list of models that match your needs. Note that Quick Search only allows for one model selection at a time." />
                              </td>
                              <td align="left" valign="top" colspan="3">
                                <asp:TextBox ID="___yt_model_id" runat="server" CssClass="display_none"></asp:TextBox>
                                <div class="ui-widget">
                                  <asp:DropDownList runat="server" name="YachtModelDynamic" ID="YachtModelDynamic"
                                    autofocus="autofocus" autocorrect="off" autocomplete="off">
                                  </asp:DropDownList>
                                </div>
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top" width="160">Yacht Name:
                              </td>
                              <td align="left" valign="top" colspan="3">
                                <asp:TextBox ID="___yacht_name_search" runat="server" Width="45%"></asp:TextBox>
                                <asp:CheckBox runat="server" Text="Search Prev. Names" ID="___ypn_previous_name" />
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top" width="160">Hull #:
                              </td>
                              <td align="left" valign="top" width="24%">
                                <asp:TextBox ID="___hull_MFR_from" runat="server" Width="100%"></asp:TextBox>
                              </td>
                              <td align="right" valign="top" width="20%">Call Sign:
                              </td>
                              <td align="left" valign="top" width="30%">
                                <asp:TextBox ID="___yt_call_sign" runat="server" Width="100%"></asp:TextBox>
                              </td>
                            </tr>
                            <tr>
                              <td colspan='5' align='left'>
                                <table cellspacing='0' cellpadding='0' border='0' width='100%'>
                                  <tr>
                                    <td>
                                      <asp:CheckBox runat="server" ID="___for_sale" Text="For Sale?" CssClass="float_left" />&nbsp;
                                    </td>
                                    <td>
                                      <asp:CheckBox runat="server" ID="___for_lease" Text="For Lease?" CssClass="float_left" />&nbsp;
                                    </td>
                                    <td>
                                      <asp:CheckBox runat="server" ID="___for_charter" Text="For Charter?" CssClass="float_left" />&nbsp;
                                    </td>
                                  </tr>
                                </table>
                              </td>
                            </tr>
                            <!--
                            <tr>
                              <td align="left" valign="top">
                                Market Status:
                              </td>
                              <td align="left" valign="top" colspan="3">
                                <asp:DropDownList ID="___yt_market" runat="server" Width="100%">
                                  <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                  <asp:ListItem Value="Available">Available</asp:ListItem>
                                  <asp:ListItem Value="Auction">Auction</asp:ListItem>
                                  <asp:ListItem Value="Deposit">Deposit</asp:ListItem>
                                  <asp:ListItem Value="Lease Pending">Lease Pending</asp:ListItem>
                                  <asp:ListItem Value="Sale Pending">Sale Pending</asp:ListItem>
                                  <asp:ListItem Value="Sealed Bid">Sealed Bid</asp:ListItem>
                                  <asp:ListItem Value="See Notes">See Notes</asp:ListItem>
                                  <asp:ListItem Value="Unconfirmed">Unconfirmed</asp:ListItem>
                                  <asp:ListItem Value="Not For Sale">Not For Sale</asp:ListItem>
                                </asp:DropDownList>
                              </td>
                            </tr>
                            -->
                            <tr>
                              <td align="left" valign="top">Year MFR/Dlv:
                              </td>
                              <td align="left" valign="top" colspan="3">
                                <asp:TextBox ID="___yt_year_mfr" runat="server" Width="60px"></asp:TextBox>/
                                <asp:TextBox ID="___yt_year_dlv" runat="server" Width="60px"></asp:TextBox>
                              </td>
                            </tr>
                            <tr>
                              <td>Length:
                              </td>
                              <td>
                                <asp:DropDownList runat="server" ID="___operator_length" Width="100%">
                                </asp:DropDownList>
                              </td>
                              <td>
                                <asp:TextBox ID="___length_to" runat="server" Width="100%">
                                </asp:TextBox>
                              </td>
                              <td>
                                <asp:CheckBox ID="___us_standard" runat="server" Text="US" Checked='true' />
                                <asp:CheckBox ID="___metric_standard" runat="server" Text="Metric" />
                                <cc1:MutuallyExclusiveCheckBoxExtender ID="mecbe1" runat="server" TargetControlID="___us_standard"
                                  Key="YesNo" />
                                <cc1:MutuallyExclusiveCheckBoxExtender ID="mecbe2" runat="server" TargetControlID="___metric_standard"
                                  Key="YesNo" />
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top">Yacht Type:
                              </td>
                              <td align="left" valign="top" colspan="3">
                                <asp:DropDownList ID="___yt_motor_sailing" runat="server" Width="100%">
                                  <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                  <asp:ListItem Value="M">Motor</asp:ListItem>
                                  <asp:ListItem Value="S">Sailing</asp:ListItem>
                                </asp:DropDownList>
                              </td>
                            </tr>
                          </table>
                        </td>
                        <td align="left" valign="top" class="gray_background_color">
                          <table width="100%" cellpadding="3" cellspacing="0">
                            <tr>
                              <td align="left" valign="top" width="130">Company Name:
                              </td>
                              <td align="left" valign="top">
                                <asp:TextBox ID="company_name___" runat="server" Width="100%"></asp:TextBox>
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top">Contact Name (First/Last):
                              </td>
                              <td align="left" valign="top">
                                <asp:TextBox ID="company_contact_first___" runat="server" Width="45%" CssClass="float_left"></asp:TextBox>
                                <asp:TextBox ID="company_contact_last___" runat="server" Width="45%" CssClass="float_right"></asp:TextBox>
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top">Email Address:
                              </td>
                              <td align="left" valign="top">
                                <asp:TextBox ID="company_email_address___" runat="server" Width="100%"></asp:TextBox>
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top">Relationship To Yacht:
                              </td>
                              <td align="left" valign="top">
                                <asp:DropDownList ID="company_relationship___" runat="server" Width="100%">
                                </asp:DropDownList>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                    </table>
                    <table width="100%" cellpadding="3" cellspacing="0">
                      <tr>
                        <td align="left" valign="top" width="50%">
                          <asp:LinkButton ID="searchYacht" runat="server" CssClass="gray_button float_right"
                            OnClientClick="document.body.style.cursor='wait';">Search Yacht</asp:LinkButton>
                        </td>
                        <td align="left" valign="top" width="50%">
                          <asp:LinkButton ID="searchYachtCompany" runat="server" CssClass="gray_button float_right">Search Company</asp:LinkButton>
                        </td>
                      </tr>
                    </table>
                  </asp:Panel>
                </ContentTemplate>
              </cc1:TabPanel>
              <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="Latest News">
                <ContentTemplate>
                  <asp:Label ID="yacht_latest_news_label" runat="server" Text=""></asp:Label>
                </ContentTemplate>
              </cc1:TabPanel>
              <cc1:TabPanel ID="yacht_index_tab" runat="server" HeaderText="Index" Visible="false">
                <HeaderTemplate>
                  Attributes
                </HeaderTemplate>
                <ContentTemplate>
                  <div id="yacht_index_wait_div" runat="server" class="loadingScreenPage home_page_margin">
                    <span>Please wait while the Attributes Tab is loading... </span>
                    <br />
                    <br />
                    <img src="Images/loading.gif" alt="Loading..." /><br />
                  </div>
                  <asp:Label ID="yacht_index_tab_label" runat="server"></asp:Label>
                  <asp:Panel runat="server" ID="yachtIndexPanel" CssClass="display_none">
                  </asp:Panel>
                </ContentTemplate>
              </cc1:TabPanel>
              <cc1:TabPanel ID="yacht_action_items" runat="server" HeaderText="Action Items" Visible="false">
                <ContentTemplate>
                  <asp:Label runat="server" ID="yacht_action_items_label"><p>There are no current items for display.</p></asp:Label>
                </ContentTemplate>
              </cc1:TabPanel>
            </cc1:TabContainer>
          </ContentTemplate>
        </asp:UpdatePanel>
      </asp:TableCell>
      <asp:TableCell VerticalAlign="Top" Width="300px">
        <cc1:TabContainer runat="server" ID="yacht_small_tab" Width="100%" ActiveTabIndex="0"
          CssClass="dark-theme" OnClientActiveTabChanged="SmallTabActiveTabChanged">
          <cc1:TabPanel ID="TabPanel6" runat="server" HeaderText="Recent Activity">
            <ContentTemplate>
              <asp:Label ID="recent_aircraft_activity_yacht" runat="server" Text=""></asp:Label>
              <asp:TreeView ID="yacht_company_recent" runat="server" SkinID="project_recent_view"
                Visible="false" NodeWrap="true" CssClass="vertical_align_top tiny_text aircraft_folder">
              </asp:TreeView>
              <asp:TreeView ID="yacht_contact_recent" runat="server" SkinID="project_recent_view"
                Visible="false" NodeWrap="true" CssClass="vertical_align_top tiny_text aircraft_folder">
              </asp:TreeView>
              <asp:TreeView ID="yacht_recent" runat="server" SkinID="project_recent_view" Visible="false"
                NodeWrap="true" CssClass="vertical_align_top tiny_text aircraft_folder">
              </asp:TreeView>
              <br class="div_clear" />
            </ContentTemplate>
          </cc1:TabPanel>
          <cc1:TabPanel ID="TabPanel7" runat="server" HeaderText="Folders">
            <ContentTemplate>
              <asp:UpdatePanel runat="server" ID="yacht_folder_update" ChildrenAsTriggers="true"
                UpdateMode="Conditional">
                <ContentTemplate>
                  <div class="small_subbar">
                    <a href="#" onclick="javascript:load('http://www.jetnetevo.com/help/helpexamples/340.pdf ','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"
                      class="red_button help_button">
                      <img src="images/info_white.png" border="0" width="13" /></a>
                    <asp:CheckBox runat="server" ID="yacht_hidden_folders" Text="Show Hidden Folders?"
                      AutoPostBack="true" EnableViewState="true" CssClass="tiny_text margin_right white_text float_right"
                      OnCheckedChanged="change_hidden_folder" />
                  </div>
                  <asp:Panel runat="server" ID="Panel4" CssClass="aircraft_folder">
                    <a href="FolderMaintenance.aspx?t=1" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <asp:TreeView ID="yacht_company_projects" runat="server" SkinID="project_tree_view">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Panel runat="server" ID="Panel2" CssClass="aircraft_folder">
                    <a href="FolderMaintenance.aspx?t=2" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <asp:TreeView ID="yacht_contact_projects" runat="server" SkinID="project_tree_view">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Panel runat="server" ID="Panel7" CssClass="aircraft_folder">
                    <a href="FolderMaintenance.aspx?t=10" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <asp:TreeView ID="yacht_projects" runat="server" SkinID="project_tree_view">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Panel runat="server" ID="Panel1" CssClass="aircraft_folder">
                    <a href="FolderMaintenance.aspx?t=14" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <asp:TreeView ID="yacht_history_projects" runat="server" SkinID="project_tree_view">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Panel runat="server" ID="Panel3" CssClass="aircraft_folder">
                    <a href="FolderMaintenance.aspx?t=15" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit_icon.png" alt="Edit" border="0" class="padding_left" /></a>
                    <asp:TreeView ID="yacht_event_projects" runat="server" SkinID="project_tree_view">
                    </asp:TreeView>
                  </asp:Panel>
                  <asp:Label ID="Label4" runat="server" Text="" Visible="false"><br /><p align="center">There are no current projects.</p></asp:Label>
                  <br class="div_clear" />
                </ContentTemplate>
              </asp:UpdatePanel>
            </ContentTemplate>
          </cc1:TabPanel>
        </cc1:TabContainer>
      </asp:TableCell>
    </asp:TableHeaderRow>
  </asp:Table>
  <asp:TextBox ID="time" runat="server" Style="display: none;">
  </asp:TextBox>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

  <script type="text/javascript">

    function DisplayACDetailsWithAnalytics(ACID) {
      window.open("", "myNewWin", "width=1050,height=900,toolbar=0,scrollbars=1");

      my_form = document.createElement('FORM');
      my_form.name = 'myForm';
      my_form.method = 'POST';
      my_form.action = 'DisplayAircraftDetail.aspx?acid=' + ACID + '&analytics=Y';
      my_form.target = "myNewWin";
      my_tb = document.createElement('INPUT');
      my_tb.type = 'HIDDEN';
      my_tb.name = 'analytics';
      my_tb.value = "true";
      my_form.appendChild(my_tb);
      document.body.appendChild(my_form);
      my_form.submit();
    }


    function focusButton(typeofButton) {
      if (typeofButton == 'company') {
        $('#<%= searchCompany.clientID %>').trigger("focus");
      }
    }
    function SubmitForm(model, lifecycle, for_sale, exclusive, type_code, make) {
      my_form = document.createElement('FORM');
      my_form.name = 'myForm';
      my_form.method = 'POST';
      my_form.action = 'Aircraft_Listing.aspx';

      my_tb = document.createElement('INPUT');
      my_tb.type = 'HIDDEN';
      my_tb.name = 'for_sale';
      my_tb.value = for_sale;
      my_form.appendChild(my_tb);

      my_tb = document.createElement('INPUT');
      my_tb.type = 'HIDDEN';
      my_tb.name = 'complete_search';
      my_tb.value = "Y";
      my_form.appendChild(my_tb);

      my_tb = document.createElement('INPUT');
      my_tb.type = 'HIDDEN';
      my_tb.name = 'model';
      my_tb.value = model;
      my_form.appendChild(my_tb);


      my_tb = document.createElement('INPUT');
      my_tb.type = 'HIDDEN';
      my_tb.name = 'make';
      my_tb.value = make;
      my_form.appendChild(my_tb);

      my_tb = document.createElement('INPUT');
      my_tb.type = 'HIDDEN';
      my_tb.name = 'type_code';
      my_tb.value = type_code;
      my_form.appendChild(my_tb);

      my_tb = document.createElement('INPUT');
      my_tb.type = 'HIDDEN';
      my_tb.name = 'exclusive';
      my_tb.value = exclusive;
      my_form.appendChild(my_tb);

      my_tb = document.createElement('INPUT');
      my_tb.type = 'HIDDEN';
      my_tb.name = 'lifecycle';
      my_tb.value = lifecycle;
      my_form.appendChild(my_tb);
      document.body.appendChild(my_form);


      my_form.submit();
    }


    function RedrawDatatablesOnSys() {
      setTimeout(reRenderThem, 1800);
    }

    function reRenderThem() {
      $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
      $($.fn.dataTable.tables(true)).DataTable().scroller.measure();
      $($.fn.dataTable.tables(true)).DataTable().responsive.recalc()
    }

    function selectAllRows(data, selectedRows, tableName) {

      var IDsToUse = '';
      var count = 0;

      data.each(function (value, index) {
        if (IDsToUse.length == 0) {
          IDsToUse = value[1];
        } else {
          IDsToUse += ', ' + value[1];
        }
        count += 1;
      });

      //$("#" + selectedRows).val(IDsToUse);

    }


    function CreateTheDatatable(divName, tableName, jQueryTablename) {

      var selectedRows = '';

      try {
        if ($.fn.DataTable.isDataTable("#" + jQueryTablename)) {
          $("#" + divName).empty();
        };
      }
      catch (err) {

      }

      if ($("#" + tableName).length) {

        //if ((tableName == "companyDataTable") || (tableName == "airportDataTable")) {
        //  selectedRows = "";

        //} else {
        //  selectedRows = "";

        //}

        //jQuery("#" + tableName).css('display', 'block');

        var clone = jQuery("#" + tableName).clone(true);

        jQuery("#" + tableName).css('display', 'none');
        clone[0].setAttribute('id', jQueryTablename);
        clone.appendTo("#" + divName);

        var table = $("#" + jQueryTablename).DataTable({
          destroy: true,
          language: { "search": "Filter:" },
          fixedHeader: true,
          "initComplete": function (settings, json) {
            setTimeout(function () {
              $("#" + jQueryTablename).DataTable().columns.adjust();
              $("#" + jQueryTablename).DataTable().scroller.measure();

              var dataRows = $("#" + jQueryTablename).DataTable().rows();
              selectAllRows(dataRows.data(), selectedRows, tableName);

            }, 1200)
          },
          scrollCollapse: true,
          stateSave: true,
          paging: false,
          columnDefs: [
            { targets: [1], className: 'display_none' },
            { orderable: false, className: 'select-checkbox', width: '10px', targets: [0] }
          ],
          select: { style: 'multi', selector: 'td:first-child' },
          order: [[3, 'desc']],
          dom: 'Bftrp',
          buttons: [
            { extend: 'csv', exportOptions: { columns: ':visible' } },
            { extend: 'excel', exportOptions: { columns: ':visible' } },
            { extend: 'pdf', orientation: 'landscape', pageSize: 'A2', exportOptions: { columns: ':visible' } },
            //{ extend: 'colvis', text: 'Columns', collectionLayout: 'fixed two-column', postfixButtons: ['colvisRestore'] },

            {
              text: 'Remove Selected Rows', className: 'RemoveRowsValue',
              action: function (e, dt, node, config) {

                dt.rows({ selected: true }).remove().draw(false);
                selectAllRows(dt.rows({ selected: false }).data(), selectedRows, tableName);

              }
            },

            {
              text: 'Keep Selected Rows', className: 'KeepTableRow',
              action: function (e, dt, node, config) {

                dt.draw();
                selectAllRows(dt.rows({ selected: true }).data(), selectedRows, tableName);
                dt.rows({ selected: false }).remove().draw(false);
                dt.rows('.selected').deselect();

              }
            },

            {
              text: 'Reload Table', className: 'RefreshTableValue',
              action: function (e, dt, node, config) {

                //$("#" + selectedRows).val('');
                ChangeTheMouseCursorOnItemParentDocument('cursor_wait');

              }
            }
          ]
        });
      }

      $(".RefreshTableValue").addClass('display_none');
      $(".KeepTableRow").addClass('display_none');

      $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
      $($.fn.dataTable.tables(true)).DataTable().scroller.measure();
    };

  </script>

</asp:Content>
