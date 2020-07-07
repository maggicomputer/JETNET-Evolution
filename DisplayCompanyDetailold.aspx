<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DisplayCompanyDetailold.aspx.vb"
    Inherits="crmWebClient.DisplayCompanyDetailold" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE&sensor=false">
    </script>

    <script type="text/javascript" src="https://www.google.com/jsapi?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE">
    </script>

    <script type="text/javascript">

        google.load('visualization', '1', { packages: ['corechart'] });

        var textAlias = "<%= txtAlias.trim %>";

        function openSmallWindowJS(address, windowname) {
            var rightNow = new Date();
            windowname += rightNow.getTime();
            var Place = window.open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1250,resizable=yes,toolbar=no,location=no,status=no");
        }

    </script>
    <style>
        .CLIENTCRMRowCheckBox td {
            background-color: #ffece7 !important;
        }

    </style>
    <link href="EvoStyles/stylesheets/tableThemes.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server" ID="history_background" CssClass="">
    </asp:Panel>
    <div runat="server" id="toggle_vis" class="companyContainer">
        <div class="row valueSpec viewValueExport Simplistic aircraftSpec">
            <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="control_update_panel"
                runat="server" DisplayAfter="5">
                <ProgressTemplate>
                    <div runat="server" class="loadingScreenBox">
                        <span>Please wait while the Tab is loading... </span>
                        <br />
                        <br />
                        <img src="Images/loading.gif" alt="Loading..." /><br />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width='96%' runat="server"
                CssClass="DetailsBrowseTable">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="center" VerticalAlign="top">
                        <div class="backgroundShade">
                            <table width="100%" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:Label runat="server" ID="NextCompSwap" CssClass="float_right" Visible="false">
                       <input id="nextComp" type="button" value="Next Company > " class="gray_button" />
                                        </asp:Label>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:Label ID="regular_toggle_buttons" runat="server">
                                            <table width="100%" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <asp:UpdatePanel ID="control_update_panel" runat="server" ChildrenAsTriggers="true">
                                                            <ContentTemplate>
                                                                <ul id="view_company_insight" runat="server" visible="false" class="cssMenu_subpage"
                                                                    style="padding-left: 4px;">
                                                                    <li><a href="#" class="gray_button noBefore"><strong>Intel</strong></a>
                                                                        <ul>
                                                                            <asp:Label runat="server" ID="li_start0" Visible="false" Text="<li>"></asp:Label>
                                                                            <asp:LinkButton ID="operations_link" runat="server"></asp:LinkButton>
                                                                            <asp:Label runat="server" ID="li_end0" Visible="false" Text="</li>"></asp:Label>
                                                                            <asp:Label runat="server" ID="li_start1" Visible="false" Text="<li>"></asp:Label>
                                                                            <asp:LinkButton ID="ownership_link" runat="server"></asp:LinkButton>
                                                                            <asp:Label runat="server" ID="li_end1" Visible="false" Text="</li>"></asp:Label>
                                                                            <asp:Label runat="server" ID="li_start2" Visible="false" Text="<li>"></asp:Label>
                                                                            <asp:LinkButton ID="manu_link" runat="server"></asp:LinkButton>
                                                                            <asp:Label runat="server" ID="li_end2" Visible="false" Text="</li>"></asp:Label>
                                                                            <asp:Label runat="server" ID="li_start3" Visible="false" Text="<li>"></asp:Label>
                                                                            <asp:LinkButton ID="dealer_link" runat="server"></asp:LinkButton>
                                                                            <asp:Label runat="server" ID="li_end3" Visible="false" Text="</li>"></asp:Label>
                                                                            <asp:Label runat="server" ID="li_start4" Visible="false" Text="<li>"></asp:Label>
                                                                            <asp:LinkButton ID="lease_link" runat="server"></asp:LinkButton>
                                                                            <asp:Label runat="server" ID="li_end4" Visible="false" Text="</li>"></asp:Label>
                                                                            <asp:Label runat="server" ID="li_start5" Visible="false" Text="<li>"></asp:Label>
                                                                            <asp:LinkButton ID="financial_link" runat="server"></asp:LinkButton>
                                                                            <asp:Label runat="server" ID="li_end5" Visible="false" Text="</li>"></asp:Label>
                                                                            <asp:Label runat="server" ID="li_start6" Visible="false" Text="<li>"></asp:Label>
                                                                            <asp:LinkButton ID="portfolio_link" runat="server"></asp:LinkButton>
                                                                            <asp:Label runat="server" ID="li_end6" Visible="false" Text="</li>"></asp:Label>
                                                                        </ul>
                                                                    </li>
                                                                </ul>
                                                                <asp:LinkButton ID="view_company_history" runat="server" Visible="false" OnClick="ViewCompanyHistory"
                                                                    CssClass="gray_button float_left noBefore"><strong>History</strong></asp:LinkButton>
                                                                <asp:LinkButton ID="view_share_relationships" runat="server" CssClass="gray_button float_left"
                                                                    Visible="false" OnClick="ViewCompanyShare"><strong>Share Relationships</strong></asp:LinkButton>
                                                                <asp:LinkButton ID="view_notes" runat="server" Visible="false" CssClass="blue_button float_left"
                                                                    OnClick="ViewCompanyNotes"><strong>Notes/Actions</strong></asp:LinkButton>
                                                                <asp:LinkButton ID="view_company_events" runat="server" Visible="false" OnClick="ViewCompanyEvents"
                                                                    CssClass="gray_button float_left"><strong>Events</strong></asp:LinkButton>
                                                                <asp:LinkButton ID="view_folders" runat="server" Visible="true" OnClick="ViewCompanyFolders"
                                                                    CssClass="gray_button float_left"><strong>Folders</strong></asp:LinkButton>
                                                                <asp:LinkButton ID="map_this_company" runat="server" CssClass="gray_button float_left"
                                                                    OnClick="ViewCompanyMap"><strong>Map</strong></asp:LinkButton>
                                                                <asp:LinkButton ID="new_company_link" runat="server" CssClass="gray_button float_left"
                                                                    Visible="false" OnClientClick="javascript:load('/edit.aspx?action=new&type=company&Listing=1&from=companyDetails','','scrollbars=yes,menubar=no,height=900,width=940,resizable=yes,toolbar=no,location=no,status=no');return false;"><strong>New</strong></asp:LinkButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td align="left" valign="top" runat="server" id="cssExportMenu">
                                                        <ul class="cssMenu_subpage">
                                                            <li><a href="#" class="gray_button"><strong>Export</strong></a>
                                                                <ul>
                                                                    <li>
                                                                        <asp:LinkButton ID="export_company" runat="server">Company to Outlook</asp:LinkButton></li>
                                                                    <asp:Panel runat="server" ID="company_details_report_panel">
                                                                        <li><a href='#' onclick="javascript:load('PDF_Creator.aspx?export_type=Company Details&comp_id=<%=trim(request("compid"))%>&IS_CLIENT=<%=trim(request("source"))%>&r_id=47&use_insight_roll=<%=trim(request("use_insight_roll"))%>&use_insight_op=<%=trim(request("use_insight_op"))%>&use_insight_own=<%=trim(request("use_insight_own"))%>&use_insight_manu=<%=trim(request("use_insight_manu"))%>&use_insight_dealer=<%=trim(request("use_insight_dealer"))%>&use_insight_lease=<%=trim(request("use_insight_lease"))%>&use_insight_finance=<%=trim(request("use_insight_finance"))%>&homebase=<%=trim(request("homebase"))%>','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">Company Details Report</a></li>
                                                                        "
                                                                    </asp:Panel>
                                                                </ul>
                                                            </li>
                                                        </ul>
                                                    </td>
                                                    <td align="left" valign="top" runat="server" id="cssExportMenu2" visible="false">
                                                        <ul class="cssMenu_subpage">
                                                            <li><a href="#" class="gray_button"><strong>Reports</strong></a>
                                                                <ul>
                                                                    <asp:Panel runat="server" ID="Panel1">
                                                                        <li><a href='#' onclick="javascript:load('http://jetnet14/help/listcompanysubscriptioninstalls.asp?SearchCompId=<%=Trim(Request("compid"))%>&rdIncludeAllInstalls=ON&chkLinkCustomerProgram=ON&chkIncludeSubscriptionNotes=ON&chkIncludeContractAmount=ON','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">Installs</a></li>
                                                                        "
                                                                    </asp:Panel>
                                                                </ul>
                                                            </li>
                                                        </ul>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Label>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:Label ID="history_toggle_buttons" runat="server">
                  <a href="#" class="gray_button noBefore">View Current Company</a></asp:Label>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:Label runat="server" ID="company_help_button_label"><a href="#">Help</a></asp:Label>
                                        <a href="#" class="gray_button float_left" onclick="javascript:window.close();"><strong>Close</strong></a>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="3" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="NotesHeader"
                        BackColor="#4d4d4d" ForeColor="White">
                        <asp:Label ID="browseTableTitle" runat="server" Text=""></asp:Label>
                        <asp:Label runat="server" ID="browse_label" Visible="false">Record
              <asp:Label ID="currentRecLabel" runat="server" Text="1"></asp:Label>
                            of
              <asp:Label ID="totalRecLabel" runat="server" Text="1"></asp:Label>
                            found</asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <div class="clearfix">
            </div>

              <div class="six columns remove_margin main">
                    <asp:Button runat="server" ID="refreshPage" Text="Refresh Page" CssClass="display_none" />
                    <asp:Label ID="history_information_label" runat="server"></asp:Label>
                    <asp:Panel runat="server" ID="informationContainer">
                        <asp:Label ID="information_label" runat="server" Text=""></asp:Label>
                        <asp:Label ID="company_address" runat="server" CssClass="display_none"></asp:Label>
                        <asp:Label ID="company_name" runat="server" CssClass="display_none"></asp:Label>
                        <asp:Label ID="rollup_link" runat="server" Visible="false" CssClass="rollupLink"></asp:Label>
                        <asp:Label ID="clear_model" runat="server" Visible="false" CssClass="rollupLink"></asp:Label>
                        <asp:Label ID="faa_data_link" runat="server" Visible="false" CssClass="rollupLink"></asp:Label>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="servicesContainer" Visible="false" CssClass="Box">
                        <div class="subHeader">
                            Service Summary
                        </div>
                        <div id="servicesButtons" style="text-align: right; padding-right: 8px;">
                            <asp:LinkButton ID="inactiveServices" runat="server" Text="Show Inactive" CssClass="float_right padding" PostBackUrl="~/DisplayCompanyDetail.aspx?task=inactive" />
                            <asp:LinkButton ID="activeServices" runat="server" Text="Show Active" CssClass="float_right padding" PostBackUrl="~/DisplayCompanyDetail.aspx?task=active" Visible="false" />
                        </div>
                        <br />
                        <asp:Label runat="server" ID="services_label"></asp:Label>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="Trials_Container" Visible="false" CssClass="Box">
                        <div class="subHeader">
                            Trial Summary
                        </div>
                        <div id="trialsButtons" style="text-align: right; padding-right: 8px;">
                            <asp:LinkButton ID="trials_link_button_all" runat="server" Text="Show Inactive" CssClass="float_right padding" PostBackUrl="~/DisplayCompanyDetail.aspx?task=inactive" />
                            <asp:LinkButton ID="trails_link_button_active" runat="server" Text="Show Active" CssClass="float_right padding" PostBackUrl="~/DisplayCompanyDetail.aspx?task=active" Visible="false" />
                        </div>
                        <br />
                        <asp:Label runat="server" ID="trial_label"></asp:Label>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="subscriptionSummaryContainer" Visible="false" CssClass="Box">
                        <div class="subHeader">
                            Subscription Summary
                        </div>
                        <div id="subscriptionButtons" style="text-align: right; padding-right: 8px;">
                            <asp:LinkButton ID="inactiveSub" runat="server" Text="Show Inactive" CssClass="float_right padding" PostBackUrl="~/DisplayCompanyDetail.aspx?task=inactive" />
                            <asp:LinkButton ID="activeSub" runat="server" Text="Show Active" CssClass="float_right padding" PostBackUrl="~/DisplayCompanyDetail.aspx?task=active" Visible="false" />
                        </div>
                        <br />
                        <asp:Label runat="server" ID="subscription_label"></asp:Label>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="activeUserContainer" Visible="false" CssClass="Box">
                        <div class="subHeader">
                            Active Users
                        </div>
                        <br />
                        <div style='max-height: 670px; overflow: auto;'>
                            <asp:Label runat="server" ID="activeUser_Label"></asp:Label>
                        </div>
                    </asp:Panel>
                    <a name="customerActivities"></a>
                    <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="customerActivitiesUpdate"
                        runat="server" DisplayAfter="150">
                        <ProgressTemplate>
                            <div id="divTabLoading_top" runat="server" class="Div_Loading_Mouse_Cursor_Full_Page"
                                align="center">
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:UpdatePanel ID="customerActivitiesUpdate" runat="server" ChildrenAsTriggers="true"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="customer_activities_panel" Visible="false" CssClass="Box">
                                <div class="subHeader">
                                    Customer Activities
                                </div>
                                <div style='max-height: 470px; overflow: auto;'>
                                    <div id="activitiesButtons" style="text-align: right; padding-right: 8px;">
                                        <asp:LinkButton ID="showAllActivities" runat="server" Text="Show All" CssClass="float_right padding" PostBackUrl="~/DisplayCompanyDetail.aspx?task=showAll#customerActivities" />
                                        <asp:LinkButton ID="showTop50Activities" runat="server" Text="Show Last 50" CssClass="float_right padding" PostBackUrl="~/DisplayCompanyDetail.aspx?task=topFifty#customerActivities" Visible="false" />
                                        <asp:LinkButton ID="activitiesAddNew" runat="server" Text="Add New" CssClass="float_right padding" />
                                    </div>
                                    <br />
                                    <asp:DropDownList runat="server" ID="customerActivitiesFilter" AutoPostBack="true">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                        <asp:ListItem Value="DOCUMENT">Contracts/Documents</asp:ListItem>
                                        <asp:ListItem Value="ACTIVITY">Technical Support</asp:ListItem>
                                        <asp:ListItem Value="MARKETING">Marketing Activities</asp:ListItem>
                                        <asp:ListItem Value="EXECUTION">Executions</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label runat="server" ID="customerActivities_Label"></asp:Label>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UpdateProgress3" AssociatedUpdatePanelID="researchPanelUpdate"
                        runat="server" DisplayAfter="150">
                        <ProgressTemplate>
                            <div id="divTabLoading_research" runat="server" class="Div_Loading_Mouse_Cursor_Full_Page"
                                align="center">
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:UpdatePanel ID="researchPanelUpdate" runat="server" ChildrenAsTriggers="true"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="researchNotesPanel" Visible="false" CssClass="Box">
                                <div class="subHeader">
                                    Research Notes
                                </div>
                                <br />
                                <div style='max-height: 470px; overflow: auto;'>
                                    <asp:DropDownList runat="server" ID="researchNoteDropdown" AutoPostBack="true">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label runat="server" ID="researchNotes"></asp:Label>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Panel runat="server" ID="contract_execution_panel" Visible="false" CssClass="Box">
                        <div class="subHeader">
                            Contract Execution
                        </div>
                        <br />
                        <div style='max-height: 470px; overflow: auto;'>
                            <asp:Label runat="server" ID="contractExecution_Label"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="contract_list_panel" Visible="false" CssClass="Box">
                        <div class="subHeader">
                            Contract List
                        </div>
                        <br />
                        <div style='max-height: 470px; overflow: auto;'>
                            <asp:Label runat="server" ID="contractList_Label"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:UpdatePanel ID="events_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="eventContainer" CssClass="Box" Visible="false">
                                <div class="subHeader">
                                    Events
                <asp:Label ID="newWindow" runat="server" CssClass="float_right"></asp:Label>
                                </div>
                                <br />
                                <asp:DataGrid runat="server" ID="eventDataGrid" AutoGenerateColumns="false" Width="100%"
                                    PageSize="20" AllowPaging="true" Visible="true" CellSpacing="0" CellPadding="0"
                                    CssClass="formatTable blue small" GridLines="None" PagerStyle-Mode="NextPrev" PagerStyle-NextPageText="Next > "
                                    PagerStyle-PrevPageText="< Previous">
                                    <HeaderStyle CssClass="header_row noBorder" />
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="<b class='title'>Aircraft</b>" ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <%#DataBinder.Eval(Container.DataItem, "amod_make_name").ToString%>&nbsp;<%#DataBinder.Eval(Container.DataItem, "amod_model_name").ToString%>Ser #:<%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString, "", "")%><%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_reg_no")), "Reg #: " & DataBinder.Eval(Container.DataItem, "ac_reg_no").ToString, "")%></em>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="<b class='title'>Activity Date/Time</b>" ItemStyle-Wrap="false"
                                            ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="mobile_display_off_cell" HeaderStyle-CssClass="mobile_display_off_cell">
                                            <ItemTemplate>
                                                <%#DataBinder.Eval(Container.DataItem, "priorev_entry_date").ToString%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="<b class='title'>Description</b>" ItemStyle-CssClass="mobile_display_off_cell"
                                            ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="mobile_display_off_cell">
                                            <ItemTemplate>
                                                <%#DataBinder.Eval(Container.DataItem, "priorev_subject").ToString%>
                                                <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "priorev_description")), " [" & DataBinder.Eval(Container.DataItem, "priorev_description").ToString & "]", "")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="<b class='title'>Activity Date/Time</b><br /><b class='title'>Description</b>"
                                            ItemStyle-VerticalAlign="Top" ItemStyle-Wrap="true" ItemStyle-CssClass="mobile_display_on_cell"
                                            HeaderStyle-CssClass="mobile_display_on_cell">
                                            <ItemTemplate>
                                                <%#DataBinder.Eval(Container.DataItem, "priorev_entry_date").ToString%><br />
                                                <br />
                                                <%#DataBinder.Eval(Container.DataItem, "priorev_subject").ToString%>
                                                <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "priorev_description")), " [" & DataBinder.Eval(Container.DataItem, "priorev_description").ToString & "]", "")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid><asp:Label ID="events_label" runat="server" Text="" CssClass="padding"></asp:Label><asp:LinkButton
                                    runat="server" ID="closeEvents" CssClass="float_right padding" OnClick="ViewCompanyEvents"
                                    Visible="false">Close Events</asp:LinkButton>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="history_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="historyContainer" CssClass="Box" Visible="false">
                                <div class="subHeader">
                                    History
                <asp:LinkButton runat="server" ID="closeHistory" CssClass="float_right padding" OnClick="ViewCompanyHistory"
                    Visible="false">Close History</asp:LinkButton>
                                </div>
                                <br />
                                <asp:DataGrid runat="server" ID="historyDataGrid" AutoGenerateColumns="false" Width="100%"
                                    PageSize="10" AllowPaging="true" Visible="true" CellSpacing="3" CellPadding="3"
                                    CssClass="formatTable blue small" GridLines="None" PagerStyle-Mode="NextPrev" PagerStyle-NextPageText="Next > "
                                    PagerStyle-PrevPageText="< Previous">
                                    <HeaderStyle CssClass="header_row" />
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="<b class='title'>Description</b>">
                                            <ItemStyle VerticalAlign="Top" Width="350" />
                                            <ItemTemplate>
                                                <%#crmWebClient.clsGeneral.clsGeneral.TwoPlaceYear(DataBinder.Eval(Container.DataItem, "journ_date"))%>
                      -
                      <%#DataBinder.Eval(Container.DataItem, "amod_make_name").ToString%>&nbsp;<%#DataBinder.Eval(Container.DataItem, "amod_model_name").ToString%>Ser #:
                      <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, DataBinder.Eval(Container.DataItem, "journ_id"), True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString, "", "")%><br />
                                                <%#DataBinder.Eval(Container.DataItem, "journ_subject").ToString%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="<b class='title'>Relationships</b>">
                                            <ItemStyle VerticalAlign="Top" Width="200" />
                                            <ItemTemplate>
                                                <%#DataBinder.Eval(Container.DataItem, "actype_name").ToString%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="<b class='title'>Documents</b>">
                                            <ItemStyle VerticalAlign="Top" Width="200" />
                                            <ItemTemplate>
                                                <%#crmWebClient.clsGeneral.clsGeneral.Configure_Company_History_Documents(DataBinder.Eval(Container.DataItem, "amod_make_name"), DataBinder.Eval(Container.DataItem, "amod_model_name"), DataBinder.Eval(Container.DataItem, "ac_ser_no_full"), DataBinder.Eval(Container.DataItem, "ac_id"), DataBinder.Eval(Container.DataItem, "journ_id"), Master.aclsData_Temp)%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                                <asp:Panel runat="server" ID="yachtHistory" Visible="false">
                                    <asp:DataGrid runat="server" ID="yacht_trans_grid" AutoGenerateColumns="false" Width="100%"
                                        AllowPaging="false" Visible="true" CellSpacing="3" CellPadding="3" CssClass="formatTable blue"
                                        GridLines="None">
                                        <HeaderStyle CssClass="header_row" />
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="Transaction Description" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <%#FormatDateTime(DataBinder.Eval(Container.DataItem, "journ_date").ToString, DateFormat.ShortDate)%>
                        -
                        <%#DataBinder.Eval(Container.DataItem, "jcat_subcategory_name").ToString%>
                        -
                        <%#DataBinder.Eval(Container.DataItem, "journ_subject").ToString%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Yacht" ItemStyle-Wrap="false" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <%#DataBinder.Eval(Container.DataItem, "yt_yacht_name").ToString%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Relationships" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <%#DataBinder.Eval(Container.DataItem, "yct_name").ToString%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                    <asp:Label ID="yacht_trans_label" runat="server" Text=""></asp:Label>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="share_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="shareContainer" Visible="false" CssClass="Box">
                                <asp:Label ID="share_label" runat="server"></asp:Label>
                                <asp:LinkButton runat="server" ID="closeShare" CssClass="float_right padding" OnClick="ViewCompanyShare"
                                    Visible="false">Close Relationships</asp:LinkButton>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Label ID="contacts_label" runat="server" Text=""></asp:Label>
                    <asp:UpdatePanel
                        ID="notes_update_panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="notesPanel">
                                <div class="Box">
                                    <table class="formatTable blue" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <div class="subHeader">
                                                    Notes
                        <asp:Label ID="notes_add_new" runat="server" CssClass="float_right smallLink upperCase display_inline_block" Style="width: 65%"></asp:Label>
                                                </div>
                                                <asp:Label ID="notes_label" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="actionPanel">
                                <div class="Box">
                                    <table class="formatTable blue" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <div class="subHeader">
                                                    ACTION ITEMS
                        <asp:Label ID="action_add_new" runat="server" CssClass="float_right smallLink upperCase"></asp:Label>
                                                </div>
                                                <asp:Label ID="action_label" runat="server" Text=""></asp:Label><asp:LinkButton runat="server"
                                                    ID="closeNotes" CssClass="float_right padding" OnClick="ViewCompanyNotes" Visible="false">Close Notes/Actions</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Label ID="wanteds_label" runat="server" Text=""></asp:Label>
                    <asp:Panel runat="server" ID="yachtContainer" Visible="false">
                        <div class="Box">
                            <div class="subHeader">
                                <asp:Label runat="server" ID="yachtsHeader">Yachts</asp:Label>
                            </div>
                            <br />
                            <a href="#" name='yacht_tab'></a>
                            <asp:DataGrid runat="server" ID="YachtDataGrid" AutoGenerateColumns="False" Width="100%"
                                CellSpacing="0" CellPadding="0" CssClass="formatTable blue" GridLines="None">
                                <HeaderStyle CssClass="header_row" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="<A href='DisplayCompanyDetail.aspx?order_by=ym_brand_name#yacht_tab'>Brand/Model</a>">
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "ym_brand_name").ToString%><%#IIf(DataBinder.Eval(Container.DataItem, "ym_model_name").ToString <> "", "/" & DataBinder.Eval(Container.DataItem, "ym_model_name").ToString, "")  %>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="mobile_display_off_cell" VerticalAlign="Top" />
                                        <ItemStyle CssClass="mobile_display_off_cell" VerticalAlign="Top" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="<A  href='DisplayCompanyDetail.aspx?order_by=yt_yacht_name#yacht_tab'>Name</a>">
                                        <ItemTemplate>
                                            <a href="#" onclick="javascript:load('DisplayYachtDetail.aspx?yid=<%#DataBinder.Eval(Container.DataItem, "yt_id").ToString%>','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">
                                                <%#DataBinder.Eval(Container.DataItem, "yt_yacht_name").ToString%></a>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="mobile_display_off_cell" VerticalAlign="Top" />
                                        <ItemStyle CssClass="mobile_display_off_cell" VerticalAlign="Top" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="<A href='DisplayCompanyDetail.aspx?order_by=ym_brand_name#yacht_tab'>Brand/Model</a><br /><A  href='DisplayCompanyDetail.aspx?order_by=yt_yacht_name#yacht_tab'>Name</a>">
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "ym_brand_name").ToString%><%#IIf(DataBinder.Eval(Container.DataItem, "ym_model_name").ToString <> "", "/" & DataBinder.Eval(Container.DataItem, "ym_model_name").ToString, "")  %><br />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="mobile_display_on_cell" VerticalAlign="Top" />
                                        <ItemStyle CssClass="mobile_display_on_cell" VerticalAlign="Top" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Year">
                                        <ItemTemplate>
                                            <%#IIf(DataBinder.Eval(Container.DataItem, "yt_year_mfr").ToString <> "1900", DataBinder.Eval(Container.DataItem, "yt_year_mfr").ToString, "")%>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="mobile_display_off_cell" VerticalAlign="Top" />
                                        <ItemStyle CssClass="mobile_display_off_cell" VerticalAlign="Top" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Hull #">
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "yt_hull_mfr_nbr").ToString%>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="mobile_display_off_cell" VerticalAlign="Top" />
                                        <ItemStyle CssClass="mobile_display_off_cell" VerticalAlign="Top" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Year<br />Hull #">
                                        <ItemTemplate>
                                            <%#IIf(DataBinder.Eval(Container.DataItem, "yt_year_mfr").ToString <> "1900", DataBinder.Eval(Container.DataItem, "yt_year_mfr").ToString, "")%><br />
                                            <br />
                                            <%#DataBinder.Eval(Container.DataItem, "yt_hull_mfr_nbr").ToString%>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="mobile_display_on_cell" VerticalAlign="Top" />
                                        <ItemStyle CssClass="mobile_display_on_cell" VerticalAlign="Top" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="forsale" CssClass='<%#iif((DataBinder.Eval(Container.DataItem, "yt_forsale_flag").ToString = "Y") or (DataBinder.Eval(Container.DataItem, "yt_for_charter_flag").ToString = "Y") or (DataBinder.Eval(Container.DataItem, "yt_for_lease_flag").ToString = "Y"),"light_green_background padding_text","") %>'><%#IIf((DataBinder.Eval(Container.DataItem, "yt_forsale_flag").ToString = "Y"), DataBinder.Eval(Container.DataItem, "yt_forsale_status").ToString & " " & crmWebClient.clsGeneral.clsGeneral.display_yacht_status(DataBinder.Eval(Container.DataItem, "yt_forsale_flag"), DataBinder.Eval(Container.DataItem, "yt_for_charter_flag"), DataBinder.Eval(Container.DataItem, "yt_for_lease_flag"), DataBinder.Eval(Container.DataItem, "yt_id")) & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "yt_asking_price")), "(Asking " & Trim(crmWebClient.clsGeneral.clsGeneral.no_zero(DataBinder.Eval(Container.DataItem, "yt_asking_price"), "", True)) & ")", ""), IIf((DataBinder.Eval(Container.DataItem, "yt_forsale_flag").ToString = "Y" Or DataBinder.Eval(Container.DataItem, "yt_for_charter_flag").ToString = "Y" Or DataBinder.Eval(Container.DataItem, "yt_for_lease_flag").ToString = "Y"), crmWebClient.clsGeneral.clsGeneral.display_yacht_status(DataBinder.Eval(Container.DataItem, "yt_forsale_flag"), DataBinder.Eval(Container.DataItem, "yt_for_charter_flag"), DataBinder.Eval(Container.DataItem, "yt_for_lease_flag"), DataBinder.Eval(Container.DataItem, "yt_id")), DataBinder.Eval(Container.DataItem, "yt_forsale_status").ToString))%></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle VerticalAlign="Top" />
                                        <ItemStyle VerticalAlign="Top" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Relationship">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="type" CssClass='<%#IIF(instr(DataBinder.Eval(Container.DataItem, "yct_name"),"Central Agent") > 0,"light_purple_background padding_text", "" & IIF(instr(DataBinder.Eval(Container.DataItem, "yct_name"),"Lessee") > 0,"light_orange_background padding_text", "") & "") %>'><%#DataBinder.Eval(Container.DataItem, "yct_name").ToString%></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle VerticalAlign="Top" />
                                        <ItemStyle VerticalAlign="Top" />
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid><asp:Label ID="yacht_label" runat="server"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:UpdatePanel ID="Company_Relationship_Panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional" Visible="false">
                        <ContentTemplate>
                            <div class="Box">
                                <asp:Label runat="server" ID="relationshipHeaderText" CssClass="subHeader"></asp:Label>
                                <asp:Label runat="server" ID="Company_Relationship_Label"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="aircraft_model_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional" Visible="false">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="modelHeader"></asp:Label><asp:Label runat="server"
                                ID="aircraft_model_label"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="aircraft_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="aircraftPanel">
                                <div class="Box">
                                    <asp:Label runat="server" ID="AircraftTextHeader" CssClass="subHeader">AIRCRAFT</asp:Label><asp:DataGrid
                                        runat="server" ID="aircraftDataGrid" AutoGenerateColumns="false" Width="100%" PageSize="50"
                                        AllowPaging="true" Visible="true" CellSpacing="0" CellPadding="0" CssClass="formatTable blue small aircraftTable"
                                        GridLines="None" PagerStyle-Mode="NextPrev" PagerStyle-NextPageText="Next > " PagerStyle-PrevPageText="< Previous">
                                        <HeaderStyle CssClass="header_row" />
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="<b class='title'>Source</b>" ItemStyle-VerticalAlign="Top"
                                                HeaderStyle-VerticalAlign="Top" Visible="false">
                                                <ItemTemplate>
                                                    <%#crmWebClient.clsGeneral.clsgeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "source"))%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="<b class='title'>Year Model</b>" ItemStyle-VerticalAlign="Top"
                                                HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <%#DataBinder.Eval(Container.DataItem, "ac_year").ToString%>&nbsp;<%#DataBinder.Eval(Container.DataItem, "amod_make_name").ToString%>&nbsp;<%#DataBinder.Eval(Container.DataItem, "amod_model_name").ToString%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="<b class='title'>Ser&nbsp;#</b>" ItemStyle-VerticalAlign="Top"
                                                HeaderStyle-VerticalAlign="Top" ItemStyle-CssClass="mobile_display_off_cell" HeaderStyle-CssClass="mobile_display_off_cell">
                                                <ItemTemplate>
                                                    <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full"), "text_underline", "")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="<b class='title'>Reg&nbsp;#</b>" ItemStyle-VerticalAlign="Top"
                                                HeaderStyle-VerticalAlign="Top" ItemStyle-CssClass="mobile_display_off_cell" HeaderStyle-CssClass="mobile_display_off_cell">
                                                <ItemTemplate>
                                                    <%#DataBinder.Eval(Container.DataItem, "ac_reg_nbr").ToString%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="<b class='title'>Year</b><br /><b class='title'>Ser&nbsp;#</b><br /><b class='title'>Reg&nbsp;#</b>"
                                                ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="mobile_display_on_cell" HeaderStyle-CssClass="mobile_display_on_cell">
                                                <ItemTemplate>
                                                    <%#DataBinder.Eval(Container.DataItem, "ac_year").ToString%><br />
                                                    <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full"), "", "")%><br />
                                                    <%#DataBinder.Eval(Container.DataItem, "ac_reg_nbr").ToString%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="<b class='title'>Status</b>" ItemStyle-VerticalAlign="Top"
                                                HeaderStyle-VerticalAlign="Top" ItemStyle-CssClass="maxwidth_200 minwidth_70">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="forsale" CssClass="company_aircraft_list"><%#crmWebClient.clsGeneral.clsGeneral.DisplayStatusListingDateEvoACListing(DataBinder.Eval(Container.DataItem, "ac_forsale_flag"), DataBinder.Eval(Container.DataItem, "ac_status").ToString, DataBinder.Eval(Container.DataItem, "ac_delivery"), DataBinder.Eval(Container.DataItem, "ac_asking_price"), DataBinder.Eval(Container.DataItem, "ac_date_listed"), DataBinder.Eval(Container.DataItem, "ac_asking_wordage"), True, Now())%></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="<b class='title'>Type/Contact</b>" ItemStyle-VerticalAlign="Top"
                                                HeaderStyle-VerticalAlign="Top" ItemStyle-CssClass="mobile_display_off_cell" HeaderStyle-CssClass="mobile_display_off_cell">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="type" CssClass='<%#IIF(instr(DataBinder.Eval(Container.DataItem, "act_name"),"Exclusive Broker") > 0,"light_purple_background padding_text", "" & IIF(instr(DataBinder.Eval(Container.DataItem, "act_name"),"Lessee") > 0,"light_orange_background padding_text", "") & "") %>'><%#showFractionalPercent(DataBinder.Eval(Container.DataItem, "act_name").ToString, DataBinder.Eval(Container.DataItem, "acref_owner_percentage").ToString)%></asp:Label>
                                                    -
                        <%#crmwebclient.contactfunctions.DisplayContactNameTitle(DataBinder.Eval(Container.DataItem, "contact_first_name").ToString,DataBinder.Eval(Container.DataItem, "contact_last_name").ToString,DataBinder.Eval(Container.DataItem, "contact_title").ToString,DataBinder.Eval(Container.DataItem, "contact_id"),DataBinder.Eval(Container.DataItem, "comp_id"), false) %>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="<b class='title'>Type</b><br /><b class='title'>Contact</b>"
                                                HeaderStyle-VerticalAlign="Top" ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="mobile_display_on_cell"
                                                HeaderStyle-CssClass="mobile_display_on_cell">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="typeMob" CssClass='<%#IIF(instr(DataBinder.Eval(Container.DataItem, "act_name"),"Exclusive Broker") > 0,"light_purple_background padding_text", "" & IIF(instr(DataBinder.Eval(Container.DataItem, "act_name"),"Lessee") > 0,"light_orange_background padding_text", "") & "") %>'><%#DataBinder.Eval(Container.DataItem, "act_name").ToString%></asp:Label><br />
                                                    <%#DataBinder.Eval(Container.DataItem, "contact_first_name").ToString & " " & DataBinder.Eval(Container.DataItem, "contact_last_name").ToString%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn Visible="false" HeaderText="<b class='title'>Engine</b>" ItemStyle-VerticalAlign="Top"
                                                HeaderStyle-VerticalAlign="Top" ItemStyle-CssClass="mobile_display_off_cell" HeaderStyle-CssClass="mobile_display_off_cell">
                                                <ItemTemplate>
                                                    <%#DataBinder.Eval(Container.DataItem, "ac_engine_name").ToString%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="cssClass" ItemStyle-CssClass="display_none"></asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid><asp:DataGrid runat="server" ID="aircraftDataGrid_YachtSpot" AutoGenerateColumns="false"
                                        Width="100%" PageSize="50" AllowPaging="true" Visible="true" CellSpacing="3" CellPadding="3"
                                        CssClass="formatTable blue" GridLines="None" PagerStyle-Mode="NextPrev" PagerStyle-NextPageText="Next > "
                                        PagerStyle-PrevPageText="< Previous">
                                        <HeaderStyle CssClass="header_row" />
                                        <AlternatingItemStyle CssClass="alt_row" />
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="<b class='title'>Aircraft</b>" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <a href='#' style='font-weight: 100;' onclick="javascript:window.open('DisplayCompanyDetail.aspx?jetnet_note=Y','','scrollbars=yes,menubar=no,height=150,width=800,resizable=yes,toolbar=no,location=no,status=no');">
                                                        <%#showFractionalPercent(DataBinder.Eval(Container.DataItem, "act_name").ToString, DataBinder.Eval(Container.DataItem, "acref_owner_percentage").ToString)%>
                                                        <%#DataBinder.Eval(instr(DataBinder.Eval(Container.DataItem, "act_name"),"Exclusive Broker") > 0,"light_purple_background padding_text", "" & IIF(instr(DataBinder.Eval(Container.DataItem, "act_name"),"Lessee") > 0,"light_orange_background padding_text", "") & "") %>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                    <asp:Label ID="aircraft_label" runat="server" Text=""></asp:Label><asp:TextBox runat="server"
                                        ID="company_amount_of_ac" Text="0" CssClass="display_none"></asp:TextBox>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Panel runat="server" ID="dataProviderContainer" CssClass="Box" Visible="false">
                        <div class="subHeader">
                            Data Provider Summary
                        </div>
                        <br />
                        <asp:Label runat="server" ID="submitted_label"></asp:Label>
                    </asp:Panel>
               </div>
               <div class="six columns main" >
                    <asp:UpdatePanel ID="folders_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="foldersContainer" CssClass="Box">
                                <div class="subHeader">
                                    Folders
                <asp:LinkButton runat="server" ID="closeFolders" CssClass="float_right padding" OnClick="ViewCompanyFolders"
                    Visible="false">Close Folders</asp:LinkButton>
                                </div>
                                <asp:Label ID="folders_label" runat="server" Text="" CssClass="small_panel_height"></asp:Label>
                                <asp:Label ID="crm_folders_label" runat="server" Text="" CssClass="small_panel_height"></asp:Label>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="map_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="mapContainer" Visible="false">
                                <div class="Box">
                                    <div id="map_canvas" style="width: 100%; height: 250px">
                                    </div>
                                    <asp:LinkButton runat="server" ID="closeMap" CssClass="float_right padding" OnClick="ViewCompanyMap"
                                        Visible="false">Close Map</asp:LinkButton>
                                    <br clear="all" />
                                </div>
                                <br clear="all" />
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Label ID="marketing_label" runat="server" Text=""></asp:Label>
                    <asp:Label ID="about_label" runat="server" Text=""></asp:Label>
                    <asp:Label ID="business_label" runat="server" Text=""></asp:Label>
                    <asp:Label ID="certifications_label" runat="server" Text=""></asp:Label>
                    <asp:Panel runat="server" ID="services_used_panel" Visible="false" CssClass="Box">
                        <div class="subHeader">
                            Services Used<asp:Label ID="add_services" runat="server" CssClass="float_right smallLink upperCase display_inline_block" Style="width: 65%"></asp:Label>
                        </div>
                        <br />
                        <asp:Label runat="server" ID="servicesUsed_Label"></asp:Label>
                    </asp:Panel>
                    <asp:UpdatePanel ID="ProspectUpdate" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="prospectsContainer" CssClass="display_none">
                                <div class="Box">
                                    <table class="formatTable blue" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <div class="subHeader">
                                                    Prospects / Opportunities<asp:Label ID="new_prospects_add" runat="server" CssClass="float_right smallLink"></asp:Label>
                                                </div>
                                                <asp:Label ID="prospects_label" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel runat="server" ID="actionPanel_Admin_Top" Visible="false">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="actionPanel_Admin">
                                <div class="Box">
                                    <table class="formatTable blue" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <div class="subHeader">
                                                    ACTION ITEMS
                        <asp:Label ID="action_add_new_Admin" runat="server" CssClass="float_right smallLink upperCase"></asp:Label>
                                                </div>
                                                <asp:Label ID="action_label_Admin" runat="server" Text=""></asp:Label><asp:LinkButton runat="server"
                                                    ID="closeNotes_Admin" CssClass="float_right padding" OnClick="ViewCompanyNotes" Visible="false">Close Notes/Actions</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="relationships_udpate_panel" runat="server" ChildrenAsTriggers="True"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="relationshipHeader" CssClass="rollupLink subHeader display_none"></asp:Label><asp:Label
                                ID="relationships_label" runat="server" Text=""></asp:Label><div class="overflow_hidden mapPanel">
                                    <div id="chart_div_tab1_all" style="border-top: 0">
                                    </div>
                                </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="news_tab_update_panel" runat="server" ChildrenAsTriggers="True"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="newsContainer">
                                <asp:Label runat="server" ID="newsHeader"></asp:Label><asp:Label ID="news_label"
                                    runat="server" Text=""></asp:Label>
                                <asp:CheckBox ID="all_news" runat="server" Text="Show All News" AutoPostBack="true" />
                                <div class="tab_container_div2 overflow_hidden">
                                    <div id="chart_div_tab2_all" style="border-top: 0">
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="summary_update" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="summary_label" runat="server" Text=""></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                <!--</div>-->
            </div>
        </div>
    </div>
    <div id="toggle_vis_note" runat="server" visible="false">
        <a href="#" class="light_gray_button float_right" onclick="javascript:window.close();">Close</a>
        <asp:Label ID="note_label" runat="server" Text=""></asp:Label>
    </div>
    <span id="TellJetnetChangesContainer">
        <asp:Panel runat="server" ID="TellJetnetAboutChanges" Visible="false" class="sticky_bottom_position">
            <a href="#" id="closeTellJetnetChanges">X</a>
            <img src="images/arrowsCircle.png" width="36" />
            <a id="tellJetnetAboutChangesLink">TELL JETNET ABOUT CHANGES TO THIS COMPANY</a>
        </asp:Panel>
    </span>
    <asp:Panel runat="server" Visible="false" ID="TellJetnetAboutChangesForm">
        <div id="notifyJetnetDialog" style="display: none;">
            <iframe frameborder="0" width="100%" height="400px" id="notifyIframe" runat="server"></iframe>
        </div>
        <asp:Literal runat="server" ID="includeJqueryTheme"></asp:Literal>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

    <script language="javascript" type="text/javascript">

        function fnRefreshPage() {
            $('#<%= refreshPage.ClientID%>').click();
        }


        var map;
        var geocoder;
        $(document).ready(function () {
            resizeImages();
            //Commented out. Added to Empty Masterpage to work on the aircraft page as well as contact
            // if Modernizr detects class "touch"
            //      if ($('html').hasClass('touch')) {
            //        // for each element with class "make-tel-link"
            //        $(".make-tel-link").each(function() {
            //          var jPhoneNumber = $(this).text();
            //          // wrap phone with href="tel:" and then insert phone number
            //          $(this).wrapInner('<a class="jPhoneLink" href=""></a>');
            //          $('.jPhoneLink').attr('href', 'tel:' + jPhoneNumber);
            //        });
            //      }
        });

        function resizeImages() {

            var img = $(".pictureResize"); // Get my img elem
            var pic_real_width, pic_real_height;
            img.on('load', function () {

                var testStr = '#container-' + $(this).attr('id')
                var containerToChange = $(testStr);
                //alert(testStr);
                pic_real_width = this.width;   // Note: $(this).width() will not
                pic_real_height = this.height; // work for in memory images.

                //alert(pic_real_width + ' ' + pic_real_height);
                if (pic_real_width > pic_real_height) {

                    containerToChange.addClass("circular--landscape");
                    $(this).removeClass("pictureResize");
                } else if (pic_real_width < pic_real_height) {
                    containerToChange.addClass("circular--portrait");
                    $(this).removeClass("pictureResize");


                } else {
                    $(this).removeClass("pictureResize");
                    $(this).addClass("circular--square");

                }
            }).each(function () {
                if (this.complete) $(this).load();
            });
        }
    </script>

</asp:Content>
