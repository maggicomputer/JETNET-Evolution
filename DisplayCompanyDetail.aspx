<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DisplayCompanyDetail.aspx.vb"
    Inherits="crmWebClient.DisplayCompanyDetail" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE&sensor=false">
    </script>


    <script type="text/javascript">

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



        * {
            box-sizing: border-box;
        }
    </style>

    <style>
        .companyContainer .rollupLink {
            display: block;
            width: 100%;
            padding-bottom: 9px;
            margin-left: 7px;
            padding-top: 7px;
            padding-left: 0px;
        }

        .noBorderWhite {
            border-bottom: 1px solid #fff !important;
        }

        .data_aircraft_grid tr:last-child td {
            border-bottom: 0px solid #fff !important;
        }

        .companyContainer .Box .Box {
            padding: 0px;
            border: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart', 'table'] });
    </script>
    <asp:Panel runat="server" ID="history_background" CssClass="">
    </asp:Panel>
    <div runat="server" id="toggle_vis" class="companyContainer">
        <div class="row valueSpec viewValueExport Simplistic aircraftSpec">
            <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="" DisplayAfter="500" class="loadingScreenBox">
                <ProgressTemplate>
                    <span></span>
                    <div class="loader">Loading...</div>
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
                                        <asp:Label ID="regular_toggle_buttons" runat="server">
                                            <asp:UpdatePanel ID="control_update_panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="dropdownSettings-sub">
                                                        <asp:LinkButton ID="LinkButton1" runat="server"><img src="images/menu.svg" alt="Menu" /></asp:LinkButton>
                                                        <div class="dropdown-content-sub" style="right: 90px;">
                                                            <div class="row">
                                                                <div class="six columns" id="view_company_insight" runat="server" visible="false">
                                                                    <a href="#" runat="server" id="intelDrop"><strong>Intel</strong></a>
                                                                    <ul>
                                                                        <asp:Literal runat="server" ID="li_start0" Visible="false" Text="<li>"></asp:Literal>
                                                                        <asp:Literal ID="operations_link" runat="server"></asp:Literal>
                                                                        <asp:Literal runat="server" ID="li_end0" Visible="false" Text="</li>"></asp:Literal>
                                                                        <asp:Literal runat="server" ID="li_start1" Visible="false" Text="<li>"></asp:Literal>
                                                                        <asp:Literal ID="ownership_link" runat="server"></asp:Literal>
                                                                        <asp:Literal runat="server" ID="li_end1" Visible="false" Text="</li>"></asp:Literal>
                                                                        <asp:Literal runat="server" ID="li_start2" Visible="false" Text="<li>"></asp:Literal>
                                                                        <asp:Literal ID="manu_link" runat="server"></asp:Literal>
                                                                        <asp:Literal runat="server" ID="li_end2" Visible="false" Text="</li>"></asp:Literal>
                                                                        <asp:Literal runat="server" ID="li_start3" Visible="false" Text="<li>"></asp:Literal>
                                                                        <asp:Literal ID="dealer_link" runat="server"></asp:Literal>
                                                                        <asp:Literal runat="server" ID="li_end3" Visible="false" Text="</li>"></asp:Literal>
                                                                        <asp:Literal runat="server" ID="li_start4" Visible="false" Text="<li>"></asp:Literal>
                                                                        <asp:Literal ID="lease_link" runat="server"></asp:Literal>
                                                                        <asp:Literal runat="server" ID="li_end4" Visible="false" Text="</li>"></asp:Literal>
                                                                        <asp:Literal runat="server" ID="li_start5" Visible="false" Text="<li>"></asp:Literal>
                                                                        <asp:Literal ID="financial_link" runat="server"></asp:Literal>
                                                                        <asp:Literal runat="server" ID="li_end5" Visible="false" Text="</li>"></asp:Literal>
                                                                        <asp:Literal runat="server" ID="li_start6" Visible="false" Text="<li>"></asp:Literal>
                                                                        <asp:Literal ID="portfolio_link" runat="server"></asp:Literal>
                                                                        <asp:Literal runat="server" ID="li_end6" Visible="false" Text="</li>"></asp:Literal>
                                                                    </ul>
                                                                </div>
                                                                <div class="six columns">
                                                                    <a href="#"><strong>VIEW</strong></a>
                                                                    <ul>

                                                                        <li>
                                                                            <asp:LinkButton ID="view_company_history" runat="server" Visible="false" OnClick="ViewCompanyHistory">History</asp:LinkButton></li>
                                                                        <li>
                                                                            <asp:LinkButton ID="view_share_relationships" runat="server" Visible="false" OnClick="ViewCompanyShare">Share Relationships</asp:LinkButton></li>
                                                                        <li>
                                                                            <asp:LinkButton ID="view_notes" runat="server" Visible="false" OnClick="ViewCompanyNotes">Notes/Actions</asp:LinkButton></li>
                                                                        <li>
                                                                            <asp:LinkButton ID="view_company_events" runat="server" Visible="false" OnClick="ViewCompanyEvents">Events</asp:LinkButton></li>
                                                                        <li>
                                                                            <asp:LinkButton ID="view_folders" runat="server" Visible="true" OnClick="ViewCompanyFolders">Folders</asp:LinkButton></li>
                                                                        <li>
                                                                            <asp:Label ID="history_toggle_buttons" runat="server"><a href="#">View Current Company</a></asp:Label></li>
                                                                        <li class="display_none">
                                                                            <asp:Literal runat="server" ID="toggleLink"></asp:Literal></li>
                                                                        <li>
                                                                            <asp:LinkButton ID="map_this_company" runat="server" OnClick="ViewCompanyMap">Map</asp:LinkButton></li>
                                                                        <li>
                                                                            <asp:LinkButton ID="Data_More_Link" Visible="false" runat="server">Data Provider Summary</asp:LinkButton></li>
                                                                        <li>
                                                                            <asp:LinkButton ID="Research_More_Link" Visible="false" runat="server">Research Notes</asp:LinkButton></li>
                                                                        <li>
                                                                             <asp:Literal ID="Company_Flight_Link" runat="server">Flight Data</asp:Literal></li>

                                                                    </ul>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="dropdownSettings-sub" runat="server" id="AddMenuItem" visible="false">
                                                        <asp:LinkButton runat="server"><img src="images/edit.svg" alt="Edit" /></asp:LinkButton>
                                                        <div class="dropdown-content-sub" style="right: 50px;">
                                                            <div class="row">
                                                                <div class="twelve columns">
                                                                    <ul>

                                                                        <li runat="server" id="edit_company_link" visible="false">Edit Company</li>
                                                                        <asp:Literal runat="server" ID="viewOther" Visible="false"></asp:Literal>
                                                                        <li runat="server" id="new_company_link" visible="false"><a href="#" onclick="javascript:window.open('/edit.aspx?action=new&amp;type=company&amp;Listing=1&amp;from=companyDetails');">New Company</a></li>

                                                                        <li runat="server" id="Add_Note_Top" visible="false"></li>
                                                                        <li runat="server" id="Add_Action_Top" visible="false"></li>
                                                                        <li runat="server" id="Add_Prospect_Top" visible="false"></li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Label>
                                        <div class="dropdownSettings-sub" id="cssExportMenu" runat="server">
                                            <a href="#">
                                                <img src="images/download.svg" alt="Help" /></a>
                                            <div class="dropdown-content-sub">
                                                <a href="#"><strong>EXPORT/REPORTS</strong></a>
                                                <ul>
                                                    <li>
                                                        <asp:LinkButton ID="export_company" runat="server">Company to Outlook</asp:LinkButton></li>
                                                    <asp:Panel runat="server" ID="company_details_report_panel">
                                                        <li><a href='#' onclick="javascript:load('PDF_Creator.aspx?export_type=Company Details&comp_id=<%=trim(request("compid"))%>&IS_CLIENT=<%=trim(request("source"))%>&r_id=47&use_insight_roll=<%=trim(request("use_insight_roll"))%>&use_insight_op=<%=trim(request("use_insight_op"))%>&use_insight_own=<%=trim(request("use_insight_own"))%>&use_insight_manu=<%=trim(request("use_insight_manu"))%>&use_insight_dealer=<%=trim(request("use_insight_dealer"))%>&use_insight_lease=<%=trim(request("use_insight_lease"))%>&use_insight_finance=<%=trim(request("use_insight_finance"))%>&homebase=<%=trim(request("homebase"))%>','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">Company Details Report</a></li>
                                                    </asp:Panel>
                                                    <li runat="server" id="cssExportMenu2" visible="false"><a href='#' onclick="javascript:load('http://jetnet14/help/listcompanysubscriptioninstalls.asp?SearchCompId=<%=Trim(Request("compid"))%>&rdIncludeAllInstalls=ON&chkLinkCustomerProgram=ON&chkIncludeSubscriptionNotes=ON&chkIncludeContractAmount=ON','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">Installs</a></li>
                                                </ul>
                                            </div>
                                        </div>
                                        <asp:Literal ID="company_help_button_label" runat="server"><img src="images/help-circle.svg" alt="Help" /></asp:Literal>
                                        <a href="#" onclick="javascript:window.close();" class="float_right">
                                            <img src="images/x.svg" alt="Help" /></a>
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
                        <asp:Label runat="server" ID="NextCompSwap" CssClass="float_right" Visible="false">
                       <input id="nextComp" type="button" value="Next Company > " class="gray_button" />
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <div class="clearfix">
            </div>

            <div class="grid">
                <asp:Button runat="server" ID="refreshPage" Text="Refresh Page" CssClass="display_none" />
                <asp:Label ID="history_information_label" runat="server" CssClass="grid-item" Visible="false"></asp:Label>
                <!--Block 1 Main Information-->
                <asp:Panel runat="server" ID="informationContainer" CssClass="grid-item">
                    <div class="Box specialHeadingTable">
                        <asp:Label ID="information_label" runat="server" Text=""></asp:Label>
                        <asp:Label ID="company_address" runat="server" CssClass="display_none"></asp:Label>
                        <asp:Label ID="company_name" runat="server" CssClass="display_none"></asp:Label>
                        <asp:Label ID="rollup_link" runat="server" Visible="false" CssClass="rollupLink"></asp:Label>
                        <asp:Label ID="clear_model" runat="server" Visible="false" CssClass="rollupLink"></asp:Label>
                        <asp:Label ID="faa_data_link" runat="server" Visible="false" CssClass="rollupLink"></asp:Label>
                    </div>
                </asp:Panel>
                <!--Block 2 Services Block-->
                <asp:Panel runat="server" ID="servicesContainer" Visible="false" CssClass="grid-item">
                    <div class="Box">
                        <div class="subHeader">
                            Service Summary
                        </div>
                        <div id="servicesButtons" style="text-align: right; padding-right: 8px;">
                            <asp:LinkButton ID="inactiveServices" runat="server" Text="Show Inactive" CssClass="float_right padding" PostBackUrl="~/DisplayCompanyDetail.aspx?task=inactive" />
                            <asp:LinkButton ID="activeServices" runat="server" Text="Show Active" CssClass="float_right padding" PostBackUrl="~/DisplayCompanyDetail.aspx?task=active" Visible="false" />
                        </div>
                        <br />
                        <asp:Label runat="server" ID="services_label"></asp:Label>

                        <asp:Panel runat="server" ID="Trials_Container">
                            <div class="subHeader">
                                Trial Summary
                            </div>
                            <asp:Label runat="server" ID="trial_label"></asp:Label>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="subscriptionSummaryContainer">
                            <div class="subHeader">
                                Subscription Summary
                            </div>
                            <asp:Label runat="server" ID="subscription_label"></asp:Label>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="RelatedCompanyServicesContainer">
                            <hr />
                            <div class="subHeader">
                                RELATED COMPANY SERVICES
                            </div>
                            <asp:Label runat="server" ID="related_company_services_label"></asp:Label>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="services_used_panel" Visible="false">
                            <hr />
                            <div class="subHeader">
                                Services Used<asp:Label ID="add_services" runat="server" CssClass="float_right smallLink upperCase display_inline_block" Style="width: 65%"></asp:Label>
                            </div>
                            <br />
                            <asp:Label runat="server" ID="servicesUsed_Label"></asp:Label>
                        </asp:Panel>
                    </div>
                </asp:Panel>
                <!--Block 3 Marketing Summary-->
                <asp:Label ID="marketing_label" runat="server" Text="" CssClass="grid-item" Visible="false"></asp:Label>
                <!--Block 4 History-->
                <asp:UpdatePanel ID="history_update_panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="historyContainer" CssClass="grid-item" Visible="false">
                            <div class="Box">
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
                                    <hr />
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
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--Block 5 Events-->
                <asp:UpdatePanel ID="events_update_panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="eventContainer" CssClass="grid-item" Visible="false">
                            <div class="Box">
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
                                    runat="server" ID="closeEvents" CssClass="float_right" OnClick="ViewCompanyEvents"
                                    Visible="false">Close Events</asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--Block 6 Folders-->
                <asp:UpdatePanel ID="folders_update_panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="foldersContainer" CssClass="grid-item">
                            <div class="Box">
                                <div class="subHeader">
                                    Folders
                <asp:LinkButton runat="server" ID="closeFolders" CssClass="float_right padding" OnClick="ViewCompanyFolders"
                    Visible="false">Close Folders</asp:LinkButton>
                                </div>
                                <asp:Label ID="folders_label" runat="server" Text="" CssClass="small_panel_height"></asp:Label>
                                <asp:Label ID="crm_folders_label" runat="server" Text="" CssClass="small_panel_height"></asp:Label>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--Block 7-->
                <asp:UpdatePanel ID="map_update_panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="mapContainer" Visible="false" CssClass="grid-item">
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
                <!--Block 8 Intel – Dealer Performance -->
                <asp:UpdatePanel ID="dealer_performance_update_panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="dealerPerformancePanel" Visible="false" CssClass="grid-item">
                            <div class="Box">
                                <!--Sales roles since block-->
                                <div class="subHeader" runat="server" id="sales_roles_since_header">SALES ROLES SINCE 2019</div>
                                <div class="tab_container_div2 overflow_hidden">
                                    <div id="sales_roles_since_chart_div_all">
                                    </div>
                                </div>
                                <hr />
                                <!--Dealer sales since table block-->
                                <asp:Label runat="server" ID="dealer_sales_since_label"></asp:Label>
                                <hr />
                                <!--Dealer Sales per year chart block-->
                                <div class="subHeader">Dealer Sales Per Year</div>
                                <div class="tab_container_div2 overflow_hidden">
                                    <div id="chart_div_sales_per_year_all" class="resizeChart">
                                    </div>
                                </div>
                                <hr />
                                <!--Models Represented Dealer Performance Table block-->
                                <asp:Label runat="server" ID="dealer_performance_model"></asp:Label>
                            </div>
                            <br clear="all" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--Block 9	Intel – Aircraft Ownership -->
                <asp:UpdatePanel ID="aircraft_ownership_update_panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="aircraftOwnershipPanel" Visible="false" CssClass="grid-item">
                            <div class="Box">
                                <div class="subHeader">Sales Per Year</div>
                                <div class="tab_container_div2 overflow_hidden">
                                    <div id="ownership_sales_per_year_all" class="resizeChart">
                                    </div>
                                </div>
                                <hr />
                                <asp:Label runat="server" ID="aircraft_ownership_history_label"></asp:Label>

                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--Block 10 Intel – Aircraft Operations – this includes 2 blocks (1) airport utilization and (2) model utilization-->
                <asp:UpdatePanel ID="aircraft_operations_update_panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="AircraftOperationsPanel" Visible="false" CssClass="grid-item">
                            <div class="Box">
                                <div class="subHeader">Model Utilization</div>
                                <asp:Label runat="server" ID="aircraft_operations_model_utilization_label"></asp:Label><hr />
                                <asp:Label runat="server" ID="aircraft_operations_airport_utilization_label"></asp:Label><hr />
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--Block 11 Intel – Manufacturing -->
                <asp:UpdatePanel ID="aircraft_manufacturing_update_panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="AircraftManufacturerPanel" Visible="false" CssClass="grid-item">
                            <div class="Box">
                                <div class="subHeader">Manufacturer Summary</div>
                                <asp:Label runat="server" ID="aircraft_manufacturer_summary_label"></asp:Label><hr />
                                <div class="subHeader">Models Manufactured</div>
                                <asp:Label runat="server" ID="aircraft_models_manufactured_label"></asp:Label><hr />
                                <div class="subHeader">IN PRODUCTION AIRCRAFT BY MFR YEAR</div>
                                <br />
                                <div class="tab_container_div2 overflow_hidden">
                                    <div id="manufacturer_production_aircraft_year_all" class="resizeChart">
                                    </div>
                                </div>
                                <hr />
                                <div class="subHeader">In Operation Aircraft By MFR Year</div>
                                <br />
                                <div class="tab_container_div2 overflow_hidden">
                                    <div id="manufacturer_operation_aircraft_year_all" class="resizeChart">
                                    </div>
                                </div>

                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--Block 12 Intel – Financial -->
                <asp:UpdatePanel ID="aircraft_financial_update_panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="AircraftFinancialPanel" Visible="false" CssClass="grid-item">
                            <div class="Box">
                                <div class="subHeader">Aircraft Models Financed (Last 6 Months)</div>
                                <asp:Label runat="server" ID="aircraft_models_financed_label"></asp:Label><hr />
                                <asp:Label runat="server" ID="aircraft_financial_documents_label"></asp:Label><hr />
                                <div class="subHeader">In Operation Aircraft By MFR Year</div>
                                <br />
                                <div class="tab_container_div2 overflow_hidden">
                                    <div id="aircraft_documents_month_all" class="resizeChart">
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--Block 13 Intel – Leasing -->
                <asp:UpdatePanel ID="aircraft_leased_update_panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="AircraftLeasedPanel" Visible="false" CssClass="grid-item">
                            <div class="Box">
                                <asp:Label runat="server" ID="aircraft_leased_models_leased_label"></asp:Label><hr />
                                <asp:Label runat="server" ID="aircraft_leased_summary_label"></asp:Label><hr />
                                <asp:Label runat="server" ID="aircraft_lessor_summary"></asp:Label>
                                <div class="tab_container_div2 overflow_hidden" runat="server" id="leasesPerMonthGraphContainer">
                                    <hr />
                                    <div class="subHeader">Leases Per Month</div>
                                    <div id="aircraft_leases_per_month_all" class="resizeChart">
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="aircraft_model_panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional" Visible="false">
                    <ContentTemplate>
                        <div class="grid-item">
                            <asp:Label runat="server" ID="modelHeader"></asp:Label><asp:Label runat="server"
                                ID="aircraft_model_label"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--Block 14 Prospects/Action items (notes on evo version should be combined)-->
                <asp:UpdatePanel
                    ID="notes_update_panel" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" CssClass="grid-item" ID="notesMainContainer" Visible="false">
                            <div class="Box">
                                <asp:Panel runat="server" ID="prospectsContainer" CssClass="display_none">
                                    <table class="formatTable blue" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <div class="subHeader">
                                                    Prospects / Opportunities<asp:Label ID="new_prospects_add" runat="server" CssClass="float_right smallLink"></asp:Label>
                                                    <asp:DropDownList runat="server" ID="prospects_drops" AutoPostBack="true">
                                                        <asp:ListItem Value="O">Open</asp:ListItem>
                                                        <asp:ListItem Value="All">Open/Inactive/Closed</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="prospects_label" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <hr />
                                </asp:Panel>
                                <asp:Panel runat="server" ID="actionPanel_Admin" Visible="false">
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
                                </asp:Panel>
                                <asp:Panel runat="server" ID="actionPanel" Visible="false">
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
                                    <hr />
                                </asp:Panel>
                                <asp:Panel runat="server" ID="notesPanel" Visible="false">
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
                                </asp:Panel>
                            </div>
                        </asp:Panel>

                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--Block 15 Customer Activities-->
                <asp:UpdatePanel ID="customerActivitiesUpdate" runat="server" ChildrenAsTriggers="true"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <a name="customerActivities"></a>
                        <asp:Panel runat="server" ID="customer_activities_panel" Visible="false" CssClass="grid-item">
                            <div class="Box">
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
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--Block 16 Business Types, Certifications, Accrediations-->
                <asp:Panel runat="server" ID="BusinessTypeCertificationContainer" CssClass="grid-item">
                    <div class="Box">
                        <asp:Label ID="business_label" runat="server" Text=""></asp:Label>
                        <asp:Label ID="certifications_label" runat="server" Text=""></asp:Label>
                    </div>
                </asp:Panel>
                <!--Block 17 Contacts/Users-->
                <asp:Label ID="contacts_label" runat="server" Text="" CssClass="grid-item"></asp:Label>
                <!--Block 18 Company Relationships-->
                <asp:UpdatePanel ID="relationships_udpate_panel" runat="server" ChildrenAsTriggers="True"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="RelationshipsContainer" CssClass="grid-item" Visible="false">
                            <div class="Box">
                                <asp:Label runat="server" ID="relationshipHeader" CssClass="rollupLink subHeader display_none"></asp:Label><asp:Label
                                    ID="relationships_label" runat="server" Text=""></asp:Label><div class="overflow_hidden mapPanel">
                                        <div id="chart_div_tab1_all" style="border-top: 0">
                                        </div>
                                    </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--Block 19 Aircraft-->
                <asp:UpdatePanel ID="aircraft_update_panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="aircraftPanel" CssClass="grid-item">
                            <div class="Box overflow_hidden">
                                <asp:Label runat="server" ID="AircraftTextHeader" CssClass="subHeader">AIRCRAFT</asp:Label><asp:DataGrid
                                    runat="server" ID="aircraftDataGrid" AutoGenerateColumns="false" Width="100%" PageSize="50"
                                    AllowPaging="true" Visible="true" CellSpacing="0" CellPadding="0" CssClass="formatTable blue small aircraftTable"
                                    GridLines="None" PagerStyle-Mode="NextPrev" PagerStyle-NextPageText="Next > " PagerStyle-PrevPageText="< Previous">
                                    <HeaderStyle CssClass="header_row" />
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="<b class='title'>Source</b>" ItemStyle-VerticalAlign="Top"
                                            HeaderStyle-VerticalAlign="Top" Visible="false">
                                            <ItemTemplate>
                                                <%#crmWebClient.clsGeneral.clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "source"))%>
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
                                                <asp:Label runat="server" ID="type" CssClass='<%#IIf(InStr(DataBinder.Eval(Container.DataItem, "act_name"), "Exclusive Broker") > 0, "light_purple_background padding_text", "" & IIf(InStr(DataBinder.Eval(Container.DataItem, "act_name"), "Lessee") > 0, "light_orange_background padding_text", "") & "") %>'><%#showFractionalPercent(DataBinder.Eval(Container.DataItem, "act_name").ToString, DataBinder.Eval(Container.DataItem, "acref_owner_percentage").ToString)%></asp:Label>
                                                -
                        <%#crmWebClient.ContactFunctions.DisplayContactNameTitle(DataBinder.Eval(Container.DataItem, "contact_first_name").ToString, DataBinder.Eval(Container.DataItem, "contact_last_name").ToString, DataBinder.Eval(Container.DataItem, "contact_title").ToString, DataBinder.Eval(Container.DataItem, "contact_id"), DataBinder.Eval(Container.DataItem, "comp_id"), False) %>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="<b class='title'>Type</b><br /><b class='title'>Contact</b>"
                                            HeaderStyle-VerticalAlign="Top" ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="mobile_display_on_cell"
                                            HeaderStyle-CssClass="mobile_display_on_cell">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="typeMob" CssClass='<%#IIf(InStr(DataBinder.Eval(Container.DataItem, "act_name"), "Exclusive Broker") > 0, "light_purple_background padding_text", "" & IIf(InStr(DataBinder.Eval(Container.DataItem, "act_name"), "Lessee") > 0, "light_orange_background padding_text", "") & "") %>'><%#DataBinder.Eval(Container.DataItem, "act_name").ToString%></asp:Label><br />
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
                                                    <%#DataBinder.Eval(InStr(DataBinder.Eval(Container.DataItem, "act_name"), "Exclusive Broker") > 0, "light_purple_background padding_text", "" & IIf(InStr(DataBinder.Eval(Container.DataItem, "act_name"), "Lessee") > 0, "light_orange_background padding_text", "") & "") %>
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
                <!--Block 20 Research Notes-->
                <asp:UpdatePanel ID="researchPanelUpdate" runat="server" ChildrenAsTriggers="true"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="researchNotesPanel" Visible="false" CssClass="grid-item">
                            <div class="Box">
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
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--Block 21 Wanteds-->
                <asp:Label ID="wanteds_label" runat="server" Text="" CssClass="grid-item"></asp:Label>
                <!--Block 22 Data Provider Summary-->
                <asp:Panel runat="server" ID="dataProviderContainer" CssClass="grid-item" Visible="false">
                    <div class="Box">
                        <div class="subHeader">
                            Data Provider Summary
                        </div>
                        <br />
                        <asp:Label runat="server" ID="submitted_label"></asp:Label>
                    </div>
                </asp:Panel>

                <asp:UpdatePanel ID="share_update_panel" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="shareContainer" Visible="false" CssClass="grid-item">
                            <div class="Box">
                                <asp:Label ID="share_label" runat="server"></asp:Label>
                                <asp:LinkButton runat="server" ID="closeShare" CssClass="float_right padding" OnClick="ViewCompanyShare"
                                    Visible="false">Close Relationships</asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>


                <asp:Panel runat="server" ID="yachtContainer" Visible="false" CssClass="grid-item">
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
                                        <asp:Label runat="server" ID="type" CssClass='<%#IIf(InStr(DataBinder.Eval(Container.DataItem, "yct_name"), "Central Agent") > 0, "light_purple_background padding_text", "" & IIf(InStr(DataBinder.Eval(Container.DataItem, "yct_name"), "Lessee") > 0, "light_orange_background padding_text", "") & "") %>'><%#DataBinder.Eval(Container.DataItem, "yct_name").ToString%></asp:Label>
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
                        <div class="grid-item">
                            <div class="Box">
                                <asp:Label runat="server" ID="relationshipHeaderText" CssClass="subHeader"></asp:Label>
                                <asp:Label runat="server" ID="Company_Relationship_Label"></asp:Label>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>


                <asp:Label ID="about_label" runat="server" Text="" CssClass="grid-item"></asp:Label>

                <asp:UpdatePanel ID="news_tab_update_panel" runat="server" ChildrenAsTriggers="True"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="newsContainer" CssClass="grid-item" Visible="false">
                            <asp:Label runat="server" ID="newsHeader"></asp:Label><asp:Label ID="news_label"
                                runat="server" Text=""></asp:Label>
                            <asp:CheckBox ID="all_news" runat="server" Text="Show All News" AutoPostBack="true" />

                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>


                <asp:UpdatePanel ID="summary_update" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="summary_label" runat="server" Text="" CssClass="grid-item"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>

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
           // loadMasonry()
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






        //function loadMasonry() {
        //    var grid = document.querySelector('.grid');
        //    var msnry = new Masonry(grid, {
        //        itemSelector: '.grid-item',
        //        columnWidth: '.grid-item',
        //        gutter: 10,
        //        horizontalOrder: true,
        //        percentPosition: true
        //    });


        //    //grid.addEventListener('click', function (event) {
        //    //    // don't proceed if item was not clicked on
        //    //    if (!matchesSelector(event.target, '.grid-item')) {
        //    //        return;
        //    //    }
        //    //    // change size of item via class
        //    //    event.target.classList.toggle('grid-item--gigante');

        //    //    // trigger layout
        //    //    msnry.layout();
        //    //});


        //}

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
