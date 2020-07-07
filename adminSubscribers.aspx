<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="adminSubscribers.aspx.vb"
  Inherits="crmWebClient.adminSubscribers" MasterPageFile="~/EvoStyles/CustomerAdminTheme.Master"
  StylesheetTheme="Evo" %>

<%@ MasterType VirtualPath="~/EvoStyles/CustomerAdminTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script type="text/javascript">

    function openSmallWindowJS(address, windowname) {

      var rightNow = new Date();
      windowname += rightNow.getTime();
      var Place = open(address, windowname, "menubar,scrollbars=1,resizable,width=1150,height=600");

      return true;
    }

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
  <asp:Label ID="debugText" runat="server" Visible="false"></asp:Label>
  <asp:UpdatePanel runat="server" Visible="true" ID="subscriber_Criteria" ChildrenAsTriggers="true"
    UpdateMode="Conditional" style="padding-top: 8px; text-align: left;">
    <ContentTemplate>
      <cc1:CollapsiblePanelExtender ID="SubscriberPanelEx" runat="server" TargetControlID="Subscriber_Collapse_Panel"
        Collapsed="true" ExpandControlID="Subscriber_Control_Panel" ImageControlID="Subscriber_Image"
        ExpandedImage="../Images/search_collapse.jpg" CollapsedImage="../Images/search_expand.jpg"
        CollapseControlID="Subscriber_Control_Panel" Enabled="True" CollapsedText="New Search"
        ExpandedText="Hide Search">
      </cc1:CollapsiblePanelExtender>
      <table border="0" style="padding-top: 8px; text-align: left; width: 100%;">
        <tr>
          <td style="vertical-align: top; text-align: left;" class="dark_header" width="100%">
            <asp:Table ID="Table3" runat="server" Width="100%" CellPadding="0" CellSpacing="0"
              CssClass="padding_table">
              <asp:TableRow>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="20" ID="Subscriber_help_text">
                        <img src="../images/info_white.png" class="float_left padding_bottom help_cursor" alt="Click for More Information." title="Click for More Information." />
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="90" ID="Subscriber_search_expand_text">
                  <asp:Panel ID="Subscriber_Control_Panel" runat="server" Width="100%">
                    <asp:Image ID="Subscriber_Image" runat="server" ImageUrl="../Images/search_expand.jpg" />
                  </asp:Panel>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" ID="Subscriber_results_text">
                  <asp:Label ID="Subscriber_criteria_results" runat="server" Text=""></asp:Label>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="Subscriber_sort_by_text">
                        Sort By: 
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="70" ID="Subscriber_sort_by_dropdown">
                  <div class="action_dropdown_container">
                    <asp:BulletedList ID="Subscriber_sort_dropdown" runat="server" CssClass="ul_top sort_dropdown_width">
                      <asp:ListItem>Sort By</asp:ListItem>
                    </asp:BulletedList>
                    <asp:BulletedList ID="Subscriber_sort_submenu_dropdown" runat="server" CssClass="ul_bottom sort_dropdown"
                      OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                      <asp:ListItem>Name</asp:ListItem>
                      <asp:ListItem>Address</asp:ListItem>
                      <asp:ListItem>City</asp:ListItem>
                      <asp:ListItem>State</asp:ListItem>
                      <asp:ListItem>Country</asp:ListItem>
                      <asp:ListItem>Subscription</asp:ListItem>
                    </asp:BulletedList>
                  </div>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="65" ID="Subscriber_per_page_text">
                        Per Page:&nbsp;
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="Subscriber_per_page_dropdown_">
                  <div class="action_dropdown_container">
                    <asp:BulletedList ID="Subscriber_per_page_dropdown" runat="server" CssClass="ul_top per_page_width">
                      <asp:ListItem Value="10">10</asp:ListItem>
                    </asp:BulletedList>
                    <asp:BulletedList ID="Subscriber_per_page_submenu_dropdown" runat="server" CssClass="ul_bottom per_page_dropdown"
                      OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                      <asp:ListItem Value="10">10</asp:ListItem>
                      <asp:ListItem Value="20">20</asp:ListItem>
                      <asp:ListItem Value="30">30</asp:ListItem>
                      <asp:ListItem Value="40">40</asp:ListItem>
                      <asp:ListItem Value="50">50</asp:ListItem>
                      <asp:ListItem Value="60">60</asp:ListItem>
                      <asp:ListItem Value="70">70</asp:ListItem>
                      <asp:ListItem Value="80">80</asp:ListItem>
                      <asp:ListItem Value="90">90</asp:ListItem>
                      <asp:ListItem Value="100">100</asp:ListItem>
                      <asp:ListItem Value="200">200</asp:ListItem>
                      <asp:ListItem Value="300">300</asp:ListItem>
                      <asp:ListItem Value="400">400</asp:ListItem>
                      <asp:ListItem Value="500">500</asp:ListItem>
                    </asp:BulletedList>
                  </div>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="45" ID="Subscriber_view_dropdown_">
                  <div class="action_dropdown_container">
                    <asp:BulletedList ID="Subscriber_view_dropdown" runat="server" CssClass="ul_top thumnail_view_bullet">
                      <asp:ListItem></asp:ListItem>
                    </asp:BulletedList>
                    <asp:BulletedList ID="Subscriber_view_submenu_dropdown" runat="server" CssClass="ul_bottom thumbnail"
                      OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                      <asp:ListItem>Subscription</asp:ListItem>
                      <asp:ListItem>Subscriber</asp:ListItem>
                    </asp:BulletedList>
                  </div>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="65" ID="Subscriber_go_to_text">
                        Go To:&nbsp;
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="Subscriber_go_to_dropdown_">
                  <div class="action_dropdown_container">
                    <asp:BulletedList ID="Subscriber_go_to_dropdown" runat="server" CssClass="ul_top per_page_width">
                      <asp:ListItem>1</asp:ListItem>
                    </asp:BulletedList>
                    <asp:BulletedList ID="Subscriber_go_to_submenu_dropdown" runat="server" CssClass="ul_bottom per_page_dropdown"
                      OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                    </asp:BulletedList>
                  </div>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="75" ID="TableCell11">
                  <div class="action_dropdown_container">
                    <asp:BulletedList ID="Subscriber_actions_dropdown" runat="server" CssClass="ul_top">
                      <asp:ListItem>Actions</asp:ListItem>
                    </asp:BulletedList>
                    <asp:BulletedList ID="Subscriber_actions_submenu_dropdown" runat="server" CssClass="ul_bottom ac_action_dropdown"
                      DisplayMode="HyperLink" OnClick="submenu_dropdown_Click">
                      <asp:ListItem Value="javascript:SubMenuDrop(1,0,'SUB');">Custom Export</asp:ListItem>
                      <asp:ListItem Value="javascript:SubMenuDrop(5,0,'SUB');">JETNET Export/Report</asp:ListItem>
                      <asp:ListItem Value="javascript:SubMenuDrop(2,0,'SUB');">Summary</asp:ListItem>
                    </asp:BulletedList>
                  </div>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="70" ID="TableCell10">
                <div class="action_dropdown_container">
                </div>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="180" ID="Subscriber_paging">
                  <asp:Label ID="Label2" runat="server" CssClass="criteria_text criteria_spacer">
                    <asp:ImageButton ID="Subscriber_previous_all" ImageUrl="../images/previous_all.png"
                      runat="server" Visible="false" CommandName="previous_all" />&nbsp;<asp:ImageButton
                        ID="Subscriber_previous" ImageUrl="../images/previous_listing.png" Visible="false"
                        runat="server" CommandName="previous" />&nbsp;<asp:Label ID="Subscriber_record_count"
                          runat="server">Showing 25 - 50</asp:Label>&nbsp;<asp:ImageButton ID="Subscriber_next"
                            ImageUrl="../images/next_listing.png" runat="server" CommandName="next" />&nbsp;<asp:ImageButton
                              ID="Subscriber_next_all" ImageUrl="~/images/next_all.png" runat="server" CommandName="next_all" /></asp:Label>
                </asp:TableCell>
              </asp:TableRow>
            </asp:Table>
          </td>
        </tr>
      </table>
      <asp:Panel ID="Subscriber_Collapse_Panel" runat="server" Height="0px" Width="100%"
        CssClass="collapse">
        <strong style="padding-left: 2px;">Subscriber Company</strong>
        <asp:Table ID="Table4" Width="100%" CellPadding="3" CellSpacing="0" runat="server">
          <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="200">
              Name:
              <asp:TextBox ID="company_name" runat="server" Width="100%" Rows="1" Height="12px"
                TextMode="MultiLine"></asp:TextBox>
              <asp:CheckBox runat="server" ID="company_status_flag" Visible="false" Text="Include Companies no longer active?" />
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
              Agency Type:
              <asp:DropDownList ID="company_agency_type" runat="server" Width="99%">
                <asp:ListItem Value="">All</asp:ListItem>
                <asp:ListItem Value="C">Civilian</asp:ListItem>
                <asp:ListItem Value="G">Government</asp:ListItem>
              </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="170" RowSpan="2">
              <asp:Label runat="server" ID="relationship_text">Relationship to Aircraft:</asp:Label>
              <br class="clear" />
              <asp:CheckBox ID="comp_not_in_selected" Text="Not in Selected Relationship" runat="server" />
              <asp:ListBox ID="company_relationship" runat="server" Width="100%" SelectionMode="Multiple"
                Rows="14">
                <asp:ListItem>All</asp:ListItem>
              </asp:ListBox>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2" BackColor="#CFCFCF">
              <strong>Subscriber Product Code</strong><br />
              <asp:CheckBox ID="comp_product_helicopter_flag" runat="server" Checked="true" Text="Helicopter" />
              <asp:CheckBox ID="comp_product_business_flag" runat="server" Checked="true" Text="Business" />
              <asp:CheckBox ID="comp_product_commercial_flag" runat="server" Checked="true" Text="Commercial" />
              <asp:CheckBox ID="comp_product_yacht_flag" runat="server" Checked="true" Text="Yacht" />&nbsp;&nbsp;&nbsp;
              <asp:CheckBox ID="goto_companySearch" runat="server" Checked="false" Text="Search All Companies" AutoPostBack="true" />

            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
              <table border="0" style="padding: 4px; border-collapse: separate; border-spacing: 6px; text-align: left; width: 100%;">
                <tr>
                  <td style="vertical-align: top; text-align: left;">Address:<br />
                    <asp:TextBox ID="company_address" runat="server" Width="98%"></asp:TextBox>
                  </td>
                  <td style="vertical-align: top; text-align: left;">Business Type:<br />
                    <asp:ListBox ID="company_business" runat="server" Width="100%" Rows="5" SelectionMode="Multiple">
                      <asp:ListItem>All</asp:ListItem>
                    </asp:ListBox>
                  </td>
                </tr>
                <tr>
                  <td style="vertical-align: top; text-align: left;">City:<br />
                    <asp:TextBox ID="comp_city" runat="server" Width="98%" Rows="1" Height="12px" TextMode="MultiLine"></asp:TextBox>
                  </td>
                  <td style="vertical-align: top; text-align: left;">Postal Code:<br />
                    <asp:TextBox ID="comp_zip_code" runat="server" Width="98%" Rows="1" Height="12px"
                      TextMode="MultiLine"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td style="vertical-align: top; text-align: left;">Email Address:<br />
                    <asp:TextBox ID="company_email_address" runat="server" Width="98%" Rows="1" Height="12px"
                      TextMode="MultiLine"></asp:TextBox>
                  </td>
                  <td style="vertical-align: top; text-align: left;">Company ID:<br />
                    <asp:TextBox ID="company_id" runat="server" Width="98%" Rows="1" Height="12px" TextMode="SingleLine"></asp:TextBox>
                  </td>
                </tr>
              </table>
            </asp:TableCell>
            <asp:TableCell ID="subscription_cell" HorizontalAlign="Left" VerticalAlign="Top"
              ColumnSpan="2" CssClass="seperator_top_bottom lighter_blue_search">
              <strong>Subscription</strong>
              <table border="0" style="padding: 4px; border-collapse: separate; border-spacing: 6px; text-align: left; width: 100%;">
                <tr>
                  <td style="vertical-align: top; text-align: left;" colspan="3">Service&nbsp;Code:<br />
                    <asp:ListBox ID="service_code_list" runat="server" Width="475" Rows="4" SelectionMode="Multiple">
                      <asp:ListItem>All</asp:ListItem>
                    </asp:ListBox>
                  </td>
                </tr>
                <tr>
                  <td style="vertical-align: top; text-align: left;">Subscription&nbsp;ID:
                    <br />
                    <asp:TextBox ID="sub_id" runat="server" Width="65" Rows="1" Height="12px"></asp:TextBox>
                  </td>
                  <td style="vertical-align: top; text-align: left;">Services&nbsp;Used:
                    <br />
                    <asp:ListBox ID="service_used" runat="server" Width="115" Rows="4" SelectionMode="Multiple">
                      <asp:ListItem>All</asp:ListItem>
                    </asp:ListBox>
                  </td>
                  <td style="vertical-align: top; text-align: left;">Subscription&nbsp;Level:
                    <br />
                    <asp:DropDownList ID="parent_subscriptions" runat="server" Width="125" Rows="1">
                      <asp:ListItem Value="" Text="All"></asp:ListItem>
                      <asp:ListItem Value="parent" Text="Parent Subscriptions"></asp:ListItem>
                    </asp:DropDownList>
                  </td>
                </tr>
                <tr>
                  <td style="vertical-align: top; text-align: left;">Last&nbsp;Login&nbsp;Date:
                    <br />
                    <asp:TextBox ID="last_login_date" runat="server" Width="105" Rows="1" Height="12px"></asp:TextBox>
                  </td>
                  <td style="vertical-align: top; text-align: left;">End&nbsp;Date:
                    <br />
                    <asp:TextBox ID="sub_end_date" runat="server" Width="105" Rows="1" Height="12px"></asp:TextBox>
                  </td>
                  <td style="vertical-align: top; text-align: left;">
                    <asp:CheckBox ID="chkHistoricalSub" runat="server" Text="Search Historical Subscribers" />
                  </td>
                </tr>
                <tr>
                  <td style="vertical-align: top; text-align: left;" colspan="3">
                    <asp:CheckBox ID="chkAerodexFlag" runat="server" Text="Aerodex" />&nbsp;
                    <asp:CheckBox ID="chkDemoFlag" runat="server" Text="Demo" />&nbsp;
                    <asp:CheckBox ID="chkMarketingFlag" runat="server" Text="Marketing" />&nbsp;
                    <asp:CheckBox ID="chkCRMFlag" runat="server" Text="MPM" />&nbsp;
                    <asp:CheckBox ID="chkSPIFlag" runat="server" Text="Values" />&nbsp;
                  </td>
                </tr>
                <tr>
                  <td style="vertical-align: top; text-align: left;" colspan="3">
                    <asp:CheckBox ID="chkCloudNotesFlag" runat="server" Text="Cloud Notes" />&nbsp;
                    <asp:CheckBox ID="chkNotesPlusFlag" runat="server" Text="Cloud Notes+" />&nbsp;
                    <asp:CheckBox ID="chkMobileFlag" runat="server" Text="Mobile" />&nbsp;
                    <asp:CheckBox ID="chkAdminFlag" runat="server" Text="Admin" />
                  </td>
                </tr>
              </table>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" RowSpan="3" ColumnSpan="3">
              <asp:Panel ID="Panel1" Width="100%" runat="server" CssClass="region_panel">
                <strong>Subscriber Company Location</strong>
                <evo:viewCCSTDropDowns ID="viewCCSTDropDowns" runat="server" />

                <script type="text/javascript">

                  checkRadioButtons(bIsBaseCompany, bIsViewCompany, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);

                </script>

              </asp:Panel>
            </asp:TableCell>
            <asp:TableCell ID="TableCell1" runat="server" CssClass="lighter_gray_search">
              <strong>Subscriber Contact</strong>
              <table border="0" style="padding: 4px; border-collapse: separate; border-spacing: 6px; text-align: left; width: 100%;">
                <tr>
                  <td style="vertical-align: top; text-align: left;" class="lighter_gray_search">First Name:
                    <asp:TextBox ID="company_contact_first" runat="server" Width="97%"></asp:TextBox><br />
                    Last Name:
                    <asp:TextBox ID="company_contact_last" runat="server" Width="97%"></asp:TextBox>
                  </td>
                  <td style="vertical-align: top; text-align: left;" class="lighter_gray_search">Contact Title:
                    <asp:ListBox ID="company_contact_title" runat="server" Width="97%" SelectionMode="Multiple">
                      <asp:ListItem>All</asp:ListItem>
                    </asp:ListBox>
                  </td>
                </tr>
                <tr>
                  <td style="vertical-align: top; text-align: left;" class="lighter_gray_search">Email Address:
                    <asp:TextBox ID="company_contact_email_address" runat="server" Width="97%" Rows="1"
                      Height="12px" TextMode="MultiLine"></asp:TextBox><br />
                  </td>
                  <td style="vertical-align: top; text-align: left;" class="lighter_gray_search">Contact ID:
                    <asp:TextBox ID="company_contact_id" runat="server" Width="97%" Rows="1" Height="12px"
                      TextMode="SingleLine"></asp:TextBox><br />
                  </td>
                </tr>
              </table>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell HorizontalAlign="right" VerticalAlign="Top" ColumnSpan="2">
              <asp:Button ID="Subscriber_search" runat="server" Text="Search" UseSubmitBehavior="false"
                CssClass="button_width button-darker" /><br />
              <asp:Button ID="reset_form" runat="server" Text="Clear Selections" CssClass="button_width font-weight-normal" />
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
      </asp:Panel>
      <asp:Label runat="server" ID="page_type" CssClass="display_none"></asp:Label>
      <div class="DataGridShadowContainer">
        <asp:DataGrid runat="server" ID="Results_Subscription" AutoGenerateColumns="false"
          Width="100%" AllowCustomPaging="false" AllowPaging="true" Visible="false">
          <Columns>
            <asp:TemplateColumn HeaderText="SUB ID">
              <ItemTemplate>
                <asp:Label ID="lbl_sub_id" runat="server" Text='<%#setForcolor(DataBinder.Eval(Container.DataItem, "comp_id"), DataBinder.Eval(Container.DataItem, "sub_id"), DataBinder.Eval(Container.DataItem, "sub_parent_sub_id"))%>'
                  ToolTip='<%#IIf(Not String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "sub_parent_sub_id").ToString.Trim), "Parent Sub ID : " + DataBinder.Eval(Container.DataItem, "sub_parent_sub_id").ToString.Trim, "NO PARENT SUB ID")%>'></asp:Label>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="COMP ID">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "comp_id")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="COMPANY">
              <ItemTemplate>
                <%#crmWebClient.DisplayFunctions.WriteDetailsLink(0, DataBinder.Eval(Container.DataItem, "comp_id"), 0, 0, True, DataBinder.Eval(Container.DataItem, "comp_name").ToString.Trim, "", "")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="CITY">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "comp_city")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="STATE">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "comp_state")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="COUNTRY">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "comp_country")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="SERVICE">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "serv_code")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="HELI">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "sub_helicopters_flag")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="BUS">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "sub_business_aircraft_flag")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="COM">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "sub_commerical_flag")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="YACHT">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "sub_yacht_flag")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="TIER">
              <ItemTemplate>
                <%#IIf(DataBinder.Eval(Container.DataItem, "sub_busair_tier_level").ToString.Trim.ToUpper.Contains("1"), "JETS", IIf(DataBinder.Eval(Container.DataItem, "sub_busair_tier_level").ToString.Trim.ToUpper.Contains("2"), "TURBOS", "ALL"))%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="ADEX">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "sub_aerodex_flag")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="SPI">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "sub_sale_price_flag")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="USERS">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "sub_nbr_of_installs")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="MAX EXPORT">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "sub_max_allowed_custom_export")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="NOTES">
              <ItemTemplate>
                <asp:Label ID="lbl_notes_id" runat="server" Text='<%#IIf(DataBinder.Eval(Container.DataItem, "sub_cloud_notes_flag").ToString.Trim.ToUpper.Contains("Y"), "CLOUD", IIf(DataBinder.Eval(Container.DataItem, "sub_server_side_notes_flag").ToString.Trim.ToUpper.Contains("Y"), "CLOUD+", "N"))%>'
                  ToolTip='<%#IIf(DataBinder.Eval(Container.DataItem, "sub_cloud_notes_flag").ToString.Trim.ToUpper.Contains("Y"), "CLOUD DB : " + DataBinder.Eval(Container.DataItem, "sub_cloud_notes_database"), IIf(DataBinder.Eval(Container.DataItem, "sub_server_side_notes_flag").ToString.Trim.ToUpper.Contains("Y"), "CLOUD+ DB : " + DataBinder.Eval(Container.DataItem, "sub_server_side_dbase_name"), ""))%>'></asp:Label>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="SHARING">
              <ItemTemplate>
                <%#IIf(DataBinder.Eval(Container.DataItem, "sub_share_by_comp_id_flag").ToString.Trim.ToUpper.Contains("Y"), "PARENT COMP", IIf(DataBinder.Eval(Container.DataItem, "sub_share_by_parent_sub_id_flag").ToString.Trim.ToUpper.Contains("Y"), "PRIMARY SUB", IIf(DataBinder.Eval(Container.DataItem, "sub_share_by_parent_sub_id_flag").ToString.Trim.ToUpper.Contains("N") And DataBinder.Eval(Container.DataItem, "sub_share_by_comp_id_flag").ToString.Trim.ToUpper.Contains("N"), "SUBSCRIPTION", "NOT SHARED")))%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="START/END DATE" HeaderStyle-Wrap="true">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "sub_start_date", "{0:d}")%>
                <br />
                <%#DataBinder.Eval(Container.DataItem, "sub_end_date", "{0:d}")%>
              </ItemTemplate>
            </asp:TemplateColumn>
          </Columns>
        </asp:DataGrid>
        <asp:DataGrid runat="server" ID="Results_Subscriber" AutoGenerateColumns="false"
          Width="100%" AllowCustomPaging="false" AllowPaging="true" Visible="false">
          <Columns>
            <asp:TemplateColumn HeaderText="SUBID">
              <ItemTemplate>
                <asp:Label ID="lbl_sub_id" runat="server" Text='<%#setForcolor(DataBinder.Eval(Container.DataItem, "comp_id"), DataBinder.Eval(Container.DataItem, "sub_id"), DataBinder.Eval(Container.DataItem, "sub_parent_sub_id"))%>'
                  ToolTip='<%#IIf(Not String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "sub_parent_sub_id").ToString.Trim), "Parent Sub ID : " + DataBinder.Eval(Container.DataItem, "sub_parent_sub_id").ToString.Trim, "NO PARENT SUB ID")%>'></asp:Label>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="COMPID">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "comp_id")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="COMPANY">
              <ItemTemplate>
                <%#crmWebClient.DisplayFunctions.WriteDetailsLink(0, DataBinder.Eval(Container.DataItem, "comp_id"), 0, 0, True, DataBinder.Eval(Container.DataItem, "comp_name").ToString.Trim, "", "")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="CITY">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "comp_city")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="STATE">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "comp_state")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="COUNTRY">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "comp_country")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="PRIVILEGES">
              <ItemTemplate>
                <asp:Label ID="lbl_admin_flag" runat="server" Text='<%#IIf(DataBinder.Eval(Container.DataItem, "subins_admin_flag").ToString.Trim.ToUpper.Contains("Y"), "<strong>A</strong>", "S")%>'
                  ToolTip='<%#IIf(DataBinder.Eval(Container.DataItem, "subins_admin_flag").ToString.Trim.ToUpper.Contains("Y"), "Administrator", "Standard User")%>'></asp:Label>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="CONTACT">
              <ItemTemplate>
                <%#DisplayContactInfoListing(Container.DataItem("comp_id"), Container.DataItem("contact_id"),Container.DataItem("contact_sirname"), Container.DataItem("contact_first_name"), Container.DataItem("contact_middle_initial"), Container.DataItem("contact_last_name"), IIf(Not IsDBNull(Container.DataItem("contact_title")), Container.DataItem("contact_title"),""), False)%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="MOBILE">
              <ItemTemplate>
                <asp:Label ID="lbl_mobile_flag" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "subins_evo_mobile_flag")%>'
                  ToolTip='<%#IIf(DataBinder.Eval(Container.DataItem, "subins_evo_mobile_flag").ToString.Trim.ToUpper.Contains("Y"), "ACTIVE DATE : " + DataBinder.Eval(Container.DataItem, "subins_mobile_active_date", "{0:d}"), "NOT ENABLED")%>'></asp:Label>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="EMAIL">
              <ItemTemplate>
                <%#DisplayContactErrorListing(DataBinder.Eval(Container.DataItem, "contact_email_address"))%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="PASSWORD">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "sublogin_password")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="LAST LOGIN">
              <ItemTemplate>
                <%#DataBinder.Eval(Container.DataItem, "subins_last_login_date", "{0:d}")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="LAST HOST NAME">
              <ItemTemplate>
                <%#IIf(DataBinder.Eval(Container.DataItem, "LastHostName").ToString.Trim.ToUpper.Contains("JETNETEVO.COM"), "OLD EVO", IIf(DataBinder.Eval(Container.DataItem, "LastHostName").ToString.Trim.ToUpper.Contains("JETNETEVOLUTION.COM"), "NEW EVO", DataBinder.Eval(Container.DataItem, "LastHostName").ToString.Trim.ToUpper))%>
              </ItemTemplate>
            </asp:TemplateColumn>
          </Columns>
        </asp:DataGrid>
        <asp:Label ID="company_attention" runat="server" Text="" CssClass="red_text emphasis_text text_align_center small_to_medium_text"></asp:Label>
      </div>

      <script type="text/javascript">
        //Automatically submit on enter press
        $(function () {
          $('textarea').on('keyup', function (e) {
            if (e.keyCode == 13) {
              $("#<%= Subscriber_search.clientID %>").click();
            }
          });
        });

      </script>

    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

  <script type="text/javascript">

    //alert('bottom script');

  </script>

</asp:Content>

