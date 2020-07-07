<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Yacht_View_Master.ascx.vb"
  Inherits="crmWebClient.Yacht_View_Master" %>
<%@ Register Src="yachtCatBrandModel.ascx" TagName="viewCBMDropDowns" TagPrefix="yacht" %>
<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting"
  TagPrefix="asp" %>

<script type="text/javascript">

  google.load('visualization', '1', { packages: ['corechart'] });

  var bIsBaseView = false;
  var bIsViewView = false;
  var bShowInactiveCountriesView = false;

  var bIsBaseBase = false;
  var bIsViewBase = false;
  var bShowInactiveCountriesBase = false;

  var bIsBaseCompany = false;
  var bIsViewCompany = false;
  var bShowInactiveCountriesCompany = false;
  
</script>

<table class="centerTable" id="mainTableID" width='100%' border="0" cellpadding="0"
  cellspacing="0">
  <tr>
    <td align="center" style="text-align: center; padding-left: 0px;">
      <div id="divLoading" runat="server" class="loadingScreenPage">
        <span>Please wait while the page is loading... </span>
        <br />
        <br />
        <img src="/images/loading.gif" alt="Loading..." /><br />
      </div>
      <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="bottom_tab_update_panel"
        runat="server" DisplayAfter="500">
        <ProgressTemplate>
          <div id="divTabLoading" runat="server" class="loadingScreenTab" style="width: 650px;
            height: 550px;" align="center">
            <span>Please wait while the Tab is loading... </span>
            <br />
            <br />
            <img src="/images/loading.gif" alt="Loading..." /><br />
          </div>
        </ProgressTemplate>
      </asp:UpdateProgress>
      <asp:Panel runat="server" ID="login_warning_panel" CssClass="loadingScreenPage" Visible="false">
        <asp:Label runat="server" ID="login_warning_text"></asp:Label>
      </asp:Panel>
      <asp:Panel ID="loaded_visibility1" runat="server" CssClass="display_none" Width="100%"
        HorizontalAlign="Center" ChildrenAsTriggers="True">
        <table cellpadding="1" cellspacing="0" align="center" width="100%">
          <tr>
            <td valign="top" align="center">
              <asp:Panel ID="parent_toggle2" runat="server">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                  <tr>
                    <td align="left" valign="top" class="dark_header">
                      <table width="100%" cellpadding="3" cellspacing="0">
                        <tr>
                          <td align="left" valign="middle" width="20%">
                            <a class="underline" onclick="javascript:load('help.aspx?t=1&s=1','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');">
                              <img src="../images/info_white.png" class="float_left" border="0" alt="Show View Help"
                                title="Show View Help" style="padding-bottom: 2px;" />
                            </a>
                            <asp:Panel ID="Control_Panel1" runat="server">
                              <asp:Image ID="ControlImage1" runat="server" ImageUrl="../images/search_expand.jpg"   />
                            </asp:Panel>
                          </td>
                          <td align="left" valign="bottom" style="padding-bottom: 10px;">
                            <asp:Label ID="breadcrumbs1" runat="server" CssClass="float_left criteria_text"></asp:Label>
                          </td>
                          <td align="right" valign="top">
                            <asp:Label runat="server" ID="sortByText" Visible="false">Sort By:</asp:Label>
                          </td>
                          <td align="left" valign="bottom" width="85">
                            <div class="action_dropdown_container">
                              <asp:BulletedList ID="sort_dropdown" runat="server" CssClass="ul_top sort_dropdown_width"
                                Visible="false">
                                <asp:ListItem># of Yachts</asp:ListItem>
                              </asp:BulletedList>
                              <asp:BulletedList ID="sort_submenu_dropdown" runat="server" CssClass="ul_bottom sort_dropdown"
                                OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                                <asp:ListItem># of Yachts</asp:ListItem>
                                <asp:ListItem>Company</asp:ListItem>
                              </asp:BulletedList>  
                                <asp:BulletedList ID="central_agent_select_start" runat="server" CssClass="ul_top" Visible="false">
                                <asp:ListItem>Show All Central Agents</asp:ListItem>
                              </asp:BulletedList>
                                 <asp:BulletedList ID="central_agent_select" runat="server" CssClass="ul_bottom ac_action_dropdown"  DisplayMode="HyperLink">       
                              </asp:BulletedList> 
                          </td>
                          <td align="left" valign="bottom" width="66">
                            <div class="action_dropdown_container">
                              <asp:BulletedList ID="sort_by_dropdown" runat="server" CssClass="ul_top" Visible="false">
                                <asp:ListItem>DESC</asp:ListItem>
                              </asp:BulletedList>
                              <asp:BulletedList ID="sort_by_dropdown_submenu" runat="server" CssClass="ul_bottom   ul_sort_by"
                                OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                                <asp:ListItem>ASC</asp:ListItem>
                                <asp:ListItem>DESC</asp:ListItem>
                              </asp:BulletedList> 
                            </div>
                          </td>
                          <td align="right" valign="bottom" width="63">
                            <div class="action_dropdown_container">
                              <asp:BulletedList ID="lower_actions_dropdown" runat="server" CssClass="ul_top" Visible="false">
                                <asp:ListItem>Actions</asp:ListItem>
                              </asp:BulletedList>
                              <asp:BulletedList ID="lower_actions_submenu_dropdown" runat="server" CssClass="ul_bottom ac_action_dropdown"
                                DisplayMode="HyperLink" OnClick="submenu_dropdown_Click">
                                <asp:ListItem Value="javascript:alert('client side reaction - server side is submenu_dropdown_Click');">Test Item 1</asp:ListItem> 
                              </asp:BulletedList>
                            </div>
                          </td>
                          <td align="left" valign="bottom" style="padding-bottom: 10px;" width="90">
                            <asp:Label ID="buttons1" runat="server" CssClass="float_right criteria_text"></asp:Label>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                </table>
                <cc1:CollapsiblePanelExtender ID="PanelCollapseEx1" runat="server" TargetControlID="Collapse_Panel1"
                  Collapsed="true" ExpandControlID="Control_Panel1" ImageControlID="ControlImage1"
                  ExpandedImage="../images/search_collapse.jpg" CollapsedImage="../images/search_expand.jpg"
                  CollapseControlID="Control_Panel1" Enabled="True" CollapsedText="New Search" ExpandedText="Hide Search">
                </cc1:CollapsiblePanelExtender>
                <div id="atAGlanceCriteriaDivID">
                  <asp:Panel ID="Collapse_Panel1" runat="server" Height="0px" Width="100%" CssClass="collapse">
                    <asp:Table ID="Table1" runat="server" Width="100%" CellPadding="3" CellSpacing="0">
                      <asp:TableRow>
                        <asp:TableCell ID="cellTypeMakeModelView" HorizontalAlign="left" VerticalAlign="top"
                          Width="50%" ColumnSpan="3">
                          <asp:Table ID="Table2" Width="100%" CellPadding="0" CellSpacing="0" runat="server">
                            <asp:TableRow>
                              <asp:TableCell HorizontalAlign="left" VerticalAlign="middle">
                                <asp:Panel ID="Panel3" runat="server" Width="60%">
                                  <yacht:viewCBMDropDowns ID="viewCBMDropDowns" runat="server" />
                                </asp:Panel>

                                <script language="javascript" type="text/javascript">
                                  refreshYachtCategoryBrandModel("", "");
                                </script>

                              </asp:TableCell>
                            </asp:TableRow>
                          </asp:Table>
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow>
                        <asp:TableCell ID="cellNotesSearchPnl" HorizontalAlign="left" VerticalAlign="top"
                          ColumnSpan="4">
                          <asp:Table ID="notesSearch_table" runat="server" Width="100%" CellPadding="3" CellSpacing="0">
                            <asp:TableRow ID="regular_search">
                              <asp:TableCell HorizontalAlign="Left" VerticalAlign="middle" Width="40%">
                                <asp:Label ID="Label10" runat="server" Text="Search For:"></asp:Label>&nbsp;
                                <asp:TextBox ID="notesSearch_for_txt" runat="server" Width="80%"></asp:TextBox>
                              </asp:TableCell>
                              <asp:TableCell HorizontalAlign="Left" VerticalAlign="middle" Width="40%">
                                <asp:Label ID="Label11" runat="server" Text="User:"></asp:Label>&nbsp;
                                <asp:DropDownList ID="notesSearch_who" runat="server" Width="100%">
                                </asp:DropDownList>
                              </asp:TableCell>
                              <asp:TableCell HorizontalAlign="Left" VerticalAlign="middle">
                                <asp:Label ID="order_lbl" runat="server" Text="Order By:"></asp:Label>&nbsp;
                                <asp:DropDownList ID="notesSearch_order_by" runat="server">
                                  <asp:ListItem Value="date">Entry Date</asp:ListItem>
                                  <asp:ListItem Value="note">Note Text</asp:ListItem>
                                </asp:DropDownList>
                              </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="action_sort">
                              <asp:TableCell HorizontalAlign="Left" VerticalAlign="top" ColumnSpan="2" Height="40"
                                BackColor="#dbe7fa" BorderColor="#b2c0d6" BorderStyle="Solid" BorderWidth="1px">
                                Yacht Search:<br />
                                <asp:Table runat="server" ID="notesSearch_yt_details_table" Width="100%" CellPadding="1"
                                  CellSpacing="0">
                                  <asp:TableRow ID="TableRow3">
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="middle" Width="25%">
                                      Search Field:&nbsp;
                                      <asp:DropDownList ID="notesSearch_yt_search_field" runat="server" Enabled="true"
                                        Visible="true">
                                        <asp:ListItem Text="" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Name/Callsign" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Name" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Callsign" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Yacht ID" Value="8"></asp:ListItem>
                                      </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="middle" Width="25%">
                                      Select How:&nbsp;
                                      <asp:DropDownList ID="notesSearch_yt_search_field_operator" runat="server">
                                        <asp:ListItem Value="1">Begins With</asp:ListItem>
                                        <asp:ListItem Value="2">Anywhere</asp:ListItem>
                                        <asp:ListItem Value="4">Equals</asp:ListItem>
                                      </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="middle">
                                      Search For:&nbsp;
                                      <asp:TextBox ID="notesSearch_yt_search_field_text" runat="server" Width="70%"></asp:TextBox>
                                    </asp:TableCell>
                                  </asp:TableRow>
                                </asp:Table>
                              </asp:TableCell>
                              <asp:TableCell HorizontalAlign="Left" VerticalAlign="middle" ID="action_sort_col_two"
                                Wrap="false">
                                <asp:Label ID="Label3" runat="server" Text="Start/End:"></asp:Label>&nbsp;
                                <asp:TextBox ID="notesSearch_date" runat="server" Visible="true" Width="140px"></asp:TextBox>&nbsp;
                                <img src="../images/final.jpg" id="cal_image4" alt="&ldquo;mm/dd/yyyy&rdquo;, for Between Use &ldquo;mm/dd/yyyy:mm/dd/yyyy&rdquo;"
                                  title="&ldquo;mm/dd/yyyy&rdquo;, for Between Use &ldquo;mm/dd/yyyy:mm/dd/yyyy&rdquo;" />&nbsp;
                              </asp:TableCell>
                            </asp:TableRow>
                          </asp:Table>
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow>
                        <asp:TableCell HorizontalAlign="right" VerticalAlign="top" ColumnSpan="4">
                          <asp:Button runat="server" ID="atGlanceGo" Text="Search" ToolTip='Click to Apply Critera'
                            UseSubmitBehavior="false" />&nbsp;
                          <asp:Button runat="server" ID="atGlanceClear" Text="Clear Selections" ToolTip="Click to Clear Critera"
                            UseSubmitBehavior="false" />
                        </asp:TableCell>
                      </asp:TableRow>
                    </asp:Table>
                  </asp:Panel>
                </div> 
              </asp:Panel>
            </td>
          </tr>
          <tr>
            <td valign="top" align="left">
              <asp:UpdatePanel ID="bottom_tab_update_panel" runat="server" ChildrenAsTriggers="True">
                <ContentTemplate>
                  <asp:Panel ID="yacht_view16" runat="server" Width="100%" Visible="false">
                    <cc1:TabContainer ID="top_left_container" runat="server" CssClass="dark-theme float_left padding"
                      Visible="true" Width="32%" Height="130">
                      <cc1:TabPanel ID="top_left_panel" runat="server">
                        <ContentTemplate>
                          <asp:Label ID="top_left_label" runat="server"></asp:Label>
                        </ContentTemplate>
                      </cc1:TabPanel>
                    </cc1:TabContainer>
                    <cc1:TabContainer ID="top_center_container" runat="server" CssClass="dark-theme float_left padding"
                      Visible="true" Width="32%" Height="130">
                      <cc1:TabPanel ID="top_center_panel" runat="server">
                        <ContentTemplate>
                          <asp:Label ID="top_center_label" runat="server"></asp:Label>
                        </ContentTemplate>
                      </cc1:TabPanel>
                    </cc1:TabContainer>
                    <cc1:TabContainer ID="top_right_container" runat="server" CssClass="dark-theme float_right padding"
                      Visible="true" Width="32%" Height="130">
                      <cc1:TabPanel ID="top_right_panel" runat="server">
                        <ContentTemplate>
                          <asp:Label ID="top_right_label" runat="server"></asp:Label>
                        </ContentTemplate>
                      </cc1:TabPanel>
                    </cc1:TabContainer>
                    <div class="clear">
                      &nbsp;</div>
                    <cc1:TabContainer ID="bottom_left_container" runat="server" CssClass="dark-theme float_left padding"
                      Visible="true" Width="32%" Height="400">
                      <cc1:TabPanel ID="bottom_left_panel" runat="server">
                        <ContentTemplate>
                          <asp:Label ID="bottom_left_label" runat="server"></asp:Label>
                        </ContentTemplate>
                      </cc1:TabPanel>
                    </cc1:TabContainer>
                    <cc1:TabContainer ID="bottom_center_container" runat="server" CssClass="dark-theme float_left padding"
                      Visible="true" Width="32%" Height="400">
                      <cc1:TabPanel ID="bottom_center_panel" runat="server">
                        <ContentTemplate>
                          <asp:Label ID="bottom_center_label" runat="server"></asp:Label>
                        </ContentTemplate>
                      </cc1:TabPanel>
                    </cc1:TabContainer>
                    <cc1:TabContainer ID="bottom_right_container" runat="server" CssClass="dark-theme float_right padding"
                      Visible="true" Width="32%" Height="400">
                      <cc1:TabPanel ID="bottom_right_panel" runat="server">
                        <ContentTemplate>
                          <asp:Label ID="bottom_right_label" runat="server"></asp:Label>
                        </ContentTemplate>
                      </cc1:TabPanel>
                    </cc1:TabContainer>
                  </asp:Panel>
                  <asp:Panel ID="yacht_view17" runat="server" Width="100%" Visible="false">
                    <cc1:TabContainer ID="navalArchitectsTab" runat="server" CssClass="dark-theme float_left padding"
                      Visible="true" Width="32%" Height="400">
                      <cc1:TabPanel ID="navalArchitectsTabPanel" runat="server">
                        <ContentTemplate>
                          <asp:Label ID="navalArchitectsLabel" runat="server"></asp:Label>
                        </ContentTemplate>
                      </cc1:TabPanel>
                    </cc1:TabContainer>
                    <cc1:TabContainer ID="interiorDesignersTab" runat="server" CssClass="dark-theme float_left padding"
                      Visible="true" Width="32%" Height="400">
                      <cc1:TabPanel ID="interiorDesignersTabPanel" runat="server">
                        <ContentTemplate>
                          <asp:Label ID="interiorDesignersLabel" runat="server"></asp:Label>
                        </ContentTemplate>
                      </cc1:TabPanel>
                    </cc1:TabContainer>
                    <cc1:TabContainer ID="exteriorDesignersTab" runat="server" CssClass="dark-theme float_right padding"
                      Visible="true" Width="32%" Height="400">
                      <cc1:TabPanel ID="exteriorDesignersTabPanel" runat="server">
                        <ContentTemplate>
                          <asp:Label ID="exteriorDesignersLabel" runat="server"></asp:Label>
                        </ContentTemplate>
                      </cc1:TabPanel>
                    </cc1:TabContainer>
                  </asp:Panel>
                  <asp:Panel ID="yacht_view18" runat="server" Width="100%" Visible="false">
                    <table cellpadding="2" cellspacing="0" width='100%'>
                      <tr>
                        <td width='50%'>
                          <cc1:TabContainer ID="MFR_LIST" runat="server" CssClass="dark-theme float_left padding"
                            Visible="true" Width="99%" Height="650">
                            <cc1:TabPanel ID="MFR_LIST_PANEL" runat="server">
                              <ContentTemplate>
                                <asp:Label ID="MFR_LIST_LABEL" runat="server"></asp:Label>
                              </ContentTemplate>
                            </cc1:TabPanel>
                          </cc1:TabContainer>
                          <cc1:TabContainer ID="LATEST_NEWS_MFR" runat="server" CssClass="dark-theme float_left padding"
                            Visible="false" Width="99%" Height="350">
                            <cc1:TabPanel ID="LATEST_NEWS_MFR_PANEL" runat="server">
                              <ContentTemplate>
                                <asp:Label ID="LATEST_NEWS_MFR_LABEL" runat="server"></asp:Label>
                              </ContentTemplate>
                            </cc1:TabPanel>
                          </cc1:TabContainer>
                        </td>
                        <td width='50%'>
                          <table cellpadding="2" cellspacing="0" width='100%'>
                            <tr>
                              <td>
                                <cc1:TabContainer ID="YEAR_CHART" runat="server" CssClass="dark-theme float_left padding"
                                  Visible="true" Width="99%" Height="260">
                                  <cc1:TabPanel ID="YEAR_CHART_PANEL" runat="server">
                                    <ContentTemplate>
                                      <asp:Label ID="YEAR_CHART_LABEL" runat="server"></asp:Label>
                                    </ContentTemplate>
                                  </cc1:TabPanel>
                                </cc1:TabContainer>
                              </td>
                            </tr>
                            <tr>
                              <td>
                                <cc1:TabContainer ID="YEAR_LIST" runat="server" CssClass="dark-theme float_left padding"
                                  Visible="true" Width="99%" Height="350">
                                  <cc1:TabPanel ID="YEAR_LIST_PANEL" runat="server">
                                    <ContentTemplate>
                                      <asp:Label ID="YEAR_LIST_LABEL" runat="server"></asp:Label>
                                    </ContentTemplate>
                                  </cc1:TabPanel>
                                </cc1:TabContainer>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                    </table>
                    <asp:Label ID="mfr_comp_id" runat="server" Visible="false" Text="0"></asp:Label>
                  </asp:Panel>
                  <asp:Panel ID="yacht_view21" runat="server" Width="100%" Visible="false">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
                      <ContentTemplate>
                        <cc1:TabContainer ID="view_21_tabcontain" runat="server" Width="100%" ActiveTabIndex="0"
                          AutoPostBack="true" BorderStyle="None" Style="margin-left: auto; margin-right: auto;"
                          CssClass="dark-theme" Font-Size="X-Small">
                          <cc1:TabPanel ID="view_21_tab0" runat="server">
                            <ContentTemplate>
                              <asp:Label ID="view_21_0" runat="server"></asp:Label>
                            </ContentTemplate>
                          </cc1:TabPanel>
                          <cc1:TabPanel ID="view_21_tab1" runat="server" Visible="false">
                            <ContentTemplate>
                              <table cellpadding='3' cellspacing='0' border='0'>
                                <tr valign='top'>
                                  <td width='50%'>
                                    <asp:Label ID="view_21_1" runat="server"></asp:Label>
                                    <asp:Label ID="view_21_hide_left" runat="server" Visible="false">
                                  </td>
                                  <td width='50%'>
                                    <table cellpadding='2' cellspacing='0' border='0' width='100%'>
                                      <asp:Label runat="server" ID="graph_title_label" Text=""></asp:Label>
                                      <tr valign='top'>
                                        <td align='center'>
                                          <div id="chart_div_tab1_all" style="border-top: 0">
                                          </div>
                                          <asp:Label runat="server" ID="div_for_yacht_size"  Visible="false" ></asp:Label>
                                        </td> 
                                        <asp:Label runat="server" ID="end_row_new_row" Text="</tr><tr>" Visible="false"></asp:Label> 
                                        <asp:Label runat="server" ID="graph_title_label2" Text=""></asp:Label>
                                        <td align='center'>
                                          <div id="chart_div_tab1_all_2" style="border-top: 0">
                                          </div>
                                        </td>
                                      </tr>
                                      <tr>
                                        <td colspan='2'>
                                          <cc1:TabContainer ID="make_model_Tab_container" runat="server" Width="100%" ActiveTabIndex="0"
                                            BorderStyle="None" Style="margin-left: auto; margin-right: auto;" CssClass="dark-theme"
                                            Font-Size="X-Small">
                                            <cc1:TabPanel ID="tplist1" runat="server" Visible="true">
                                              <ContentTemplate>
                                                <asp:Label ID="view_21_1_list" runat="server"></asp:Label>
                                              </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel ID="tplist2" runat="server" Visible="true">
                                              <ContentTemplate>
                                                <asp:Label ID="view_21_1_list2" runat="server"></asp:Label>
                                              </ContentTemplate>
                                            </cc1:TabPanel>
                                          </cc1:TabContainer>
                                        </td>
                                      </tr>
                                    </table>
                                    </asp:Label>
                                  </td>
                                </tr>
                              </table>
                            </ContentTemplate>
                          </cc1:TabPanel>
                          <cc1:TabPanel ID="view_21_tab2" runat="server" Visible="false">
                            <ContentTemplate>
                              <asp:Label ID="view_21_2" runat="server"></asp:Label>
                            </ContentTemplate>
                          </cc1:TabPanel>
                          <cc1:TabPanel ID="view_21_tab3" runat="server" Visible="false">
                            <ContentTemplate>
                              <asp:Label ID="view_21_3" runat="server"></asp:Label>
                            </ContentTemplate>
                          </cc1:TabPanel>
                          <cc1:TabPanel ID="view_21_tab4" runat="server" Visible="false">
                            <ContentTemplate>
                              <asp:Label ID="view_21_4" runat="server"></asp:Label>
                            </ContentTemplate>
                          </cc1:TabPanel>
                          <cc1:TabPanel ID="view_21_tab5" runat="server" Visible="false">
                            <ContentTemplate>
                              <asp:Label ID="view_21_5" runat="server"></asp:Label>
                            </ContentTemplate>
                          </cc1:TabPanel>
                          <cc1:TabPanel ID="view_21_tab6" runat="server" Visible="false">
                            <ContentTemplate>
                              <asp:Label ID="view_21_6" runat="server"></asp:Label>
                            </ContentTemplate>
                          </cc1:TabPanel>
                          <cc1:TabPanel ID="view_21_tab7" runat="server" Visible="false">
                            <ContentTemplate>
                              <asp:Label ID="view_21_7" runat="server"></asp:Label>
                            </ContentTemplate>
                          </cc1:TabPanel>
                          <cc1:TabPanel ID="view_21_tab8" runat="server" Visible="false">
                            <ContentTemplate>
                              <asp:Label ID="view_21_8" runat="server"></asp:Label>
                            </ContentTemplate>
                          </cc1:TabPanel>
                          <cc1:TabPanel ID="view_21_tab100" runat="server" Visible="false">
                            <ContentTemplate>
                              <table cellpadding='3' cellspacing='0' border='0'>
                                <tr valign='top'>
                                  <td width='50%'>
                                    <asp:Label ID="view_21_tab100_label" runat="server"></asp:Label>
                                    <br />
                                    <asp:Label ID="view_21_100_list" runat="server"></asp:Label>
                                  </td>
                                  <td width='50%'>
                                    <table cellpadding='2' cellspacing='0' border='0' width='100%'>
                                      <tr valign='top'>
                                        <td width='50%' align='center'>
                                          <b>Yacht Category Size Owned</b><br />
                                          <div id="chart_div_tab100_all" style="border-top: 0">
                                          </div>
                                        </td>
                                      </tr>
                                      <tr>
                                        <td colspan='2' align='center'>
                                          <b>Yacht Brands Owned</b><br />
                                          <div id="chart_div_tab100_all_2" style="border-top: 0">
                                          </div>
                                        </td>
                                      </tr>
                                    </table>
                                  </td>
                                </tr>
                              </table>
                            </ContentTemplate>
                          </cc1:TabPanel>
                        </cc1:TabContainer>
                      </ContentTemplate>
                    </asp:UpdatePanel>
                  </asp:Panel>
                  <asp:Panel ID="yacht_view23" runat="server" Width="100%" Visible="false">
                    <table width='100%' cellpadding='0' cellspacing='0'>
                      <tr valign='top'>
                        <td align='left' width='50%'>
                          <table width='100%' cellpadding='0' cellspacing='0'>
                            <tr>
                              <td align='left'>
                                <cc1:TabContainer ID="view_23_left" runat="server" CssClass="dark-theme float_left padding"
                                  Visible="true" Width="99%">
                                  <cc1:TabPanel ID="view_23_tab1" runat="server">
                                    <ContentTemplate>
                                      <asp:Label ID="view_23_central" runat="server" Visible="False" Text="0"></asp:Label>
                                    </ContentTemplate>
                                  </cc1:TabPanel>
                                </cc1:TabContainer>
                              </td>
                            </tr>
                            <tr>
                              <td align='left'>
                               <cc1:TabContainer ID="view_23_left_bottom" runat="server" CssClass="dark-theme float_left padding"
                                  Visible="false" Width="99%">
                                  <cc1:TabPanel ID="view_23_tab_bottom_left" runat="server">
                                    <ContentTemplate>
                                      <asp:Label ID="view_23_central_bottom" runat="server" Text="0"></asp:Label>
                                    </ContentTemplate>
                                  </cc1:TabPanel>
                                </cc1:TabContainer>
                              </td>
                            </tr>
                          </table>
                        </td>
                        <td width='50%'>
                          <table width='100%'>
                            <tr>
                              <td align='left'>
                                <cc1:TabContainer ID="view_23_right" runat="server" CssClass="dark-theme float_left padding"
                                  Visible="true" Width="99%">
                                  <cc1:TabPanel ID="view_23_tab2" runat="server">
                                    <ContentTemplate>
                                      <asp:Label ID="view_23_central2" runat="server" Visible="False" Text="0"></asp:Label>
                                    </ContentTemplate>
                                  </cc1:TabPanel>
                                </cc1:TabContainer>
                                <asp:Label runat="server" ID="central_title_label1" Text=""></asp:Label>
                                <div id="div_central_top_all" style="border-top: 0">
                                </div>
                              </td>
                            </tr>
                            <tr>
                              <td align='left'>
                                <cc1:TabContainer ID="view_23_right_bottom" runat="server" CssClass="dark-theme float_left padding"
                                  Visible="false" Width="99%">
                                  <cc1:TabPanel ID="view_23_tab3" runat="server">
                                    <ContentTemplate>
                                      <asp:Label ID="view_23_central3" runat="server" Text="0"></asp:Label>
                                    </ContentTemplate>
                                  </cc1:TabPanel>
                                </cc1:TabContainer>
                                <asp:Label runat="server" ID="central_title_label2" Text=""></asp:Label>
                                <div id="div_central_bottom_all" style="border-top: 0">
                                </div>
                              </td>
                            </tr>
                            <tr>
                              <td>
                                <asp:Label runat="server" ID="central_title_label3" Text=""></asp:Label>
                                <div id="div_central_third_all" style="border-top: 0">
                                </div>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                    </table>
                  </asp:Panel>
                  <asp:Panel ID="yacht_view25" runat="server" Width="100%" Visible="false">
                    <cc1:TabContainer ID="yacht_viewTabContainer" runat="server" CssClass="dark-theme float_left padding"
                      Visible="true" Width="99%">
                      <cc1:TabPanel ID="yacht_viewTabPanel" runat="server">
                        <HeaderTemplate>
                          <asp:Label ID="yacht_viewHeaderLabel" runat="server"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                          <asp:Label ID="yacht_viewContentLabel" runat="server"></asp:Label>
                        </ContentTemplate>
                      </cc1:TabPanel>
                    </cc1:TabContainer>
                  </asp:Panel>
                </ContentTemplate>
              </asp:UpdatePanel>
            </td>
          </tr>
        </table>
      </asp:Panel>
    </td>
  </tr>
</table>
<asp:Panel ID="large_graph_panel" runat="server" Visible="false" Width='100%'>
  <asp:UpdatePanel ID="graph_update_panel" Visible="false" runat="server">
    <ContentTemplate>
      <cc1:TabContainer ID="tabcontainer_graph" runat="server" Width="100%" BorderStyle="None"
        Style="margin-left: auto; margin-right: auto;" CssClass="dark-theme">
        <cc1:TabPanel ID="graph_panel" runat="server">
          <ContentTemplate>
            <br />
            <br />
            <div id="large_graph_div">
            </div>
            <br />
            <asp:CheckBox ID="show_asking" runat="server" Text="Show Asking Price" />
            <asp:CheckBox ID="show_take" runat="server" Text="Show Take Price" />
            <asp:CheckBox ID="show_estimated" runat="server" Text="Show Estimated Value" />
            <asp:CheckBox ID="show_my" runat="server" Text="Show MY AC" />
            <asp:Button ID="change_large_graph" runat="server" Text="Update Graph" OnClientClick="change_large_graph_clicked()" />
          </ContentTemplate>
        </cc1:TabPanel>
      </cc1:TabContainer>
    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Panel>
<asp:Chart ID="YEAR_MRF_CHART" runat="server" Visible="false">
  <series>
    <asp:Series Name="Series1" ChartArea="ChartArea1">
    </asp:Series>
  </series>
  <chartareas>
    <asp:ChartArea Name="ChartArea1">
    </asp:ChartArea>
  </chartareas>
</asp:Chart>
<asp:Label runat="server" Visible="false" ID="dummy_label"></asp:Label>
