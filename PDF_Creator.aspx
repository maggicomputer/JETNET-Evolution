<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="PDF_Creator.aspx.vb" Inherits="crmWebClient.PDF_Creator" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

    <style>
        .valueViewPDFExport {
            height: 800px;
            overflow: hidden;
        }
    </style>
    <script type="text/javascript" src="https://cdn.rawgit.com/Mikhus/canvas-gauges/gh-pages/download/2.1.4/all/gauge.min.js"></script>

    <script type="text/javascript">

        // have to use the first item from the radio button list to get the "name" of the radio button group
        var optFormatCtrlID = "ContentPlaceHolder1_main_tab_container_select_report_panel_reportType_0";

        function viewSampleReportJS(report_id) {

            var sFormat = "blank";
            var sSample = "Report";

            var aerodexFlag = <%= bAerodexFlag.ToString.Tolower %>;

            //alert('viewSampleReportJS');

            try {
                if ((typeof (document.getElementById(optFormatCtrlID).name) != "undefined") && (document.getElementById(optFormatCtrlID) != null)) {
                    var optFormatCtrlName = document.getElementById(optFormatCtrlID).name;
                }

            }
            catch (err) {

                alert('viewSampleReportJS ' + err);

            }

            var optFormat = document.getElementsByName(optFormatCtrlName);
            sSample += report_id;

            for (i = 0; i < optFormat.length; i++) {
                if (optFormat[i].checked) {
                    sFormat = optFormat[i].value;
                }
            }

            if (sFormat.toLowerCase() == "blank") {
                return false;
            }

            if (sFormat.toLowerCase() == "excel") {
                sSample += "excel.xls";
            }
            else if (sFormat.toLowerCase() == "html") {
                sSample += "html.html";
            }
            else if (sFormat.toLowerCase() == "text (comma delimited)") {
                sSample += "comma.txt";
            }
            else if (sFormat.toLowerCase() == "text (tab delimited)") {
                sSample += "tab.txt";
            }

            //alert('viewSampleReportJS sample[' + sSample + ']');

            if (aerodexFlag == true) {
                window.open("ReportExamples/Aerodex/" + sSample, "", "scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no");
            }
            else {
                window.open("ReportExamples/" + sSample, "", "scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no");
            }
            return true;
        }

        function openReportWindow(reportWindowPath, sReportID) {

            //alert(" show report : " + bShowReport + " report path : " + reportWindowPath + " report number : " + sReportID);

            var rightNow = new Date();
            var reportWindowName = "Report" + sReportID + "Window";
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
    <script type="text/javascript">
        google.charts.load('45.2', { 'packages': ['corechart', 'table'] });
    </script>

    <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="" class="loadingScreenBox">
        <ProgressTemplate>
            <span></span>
            <div class="loader">Loading...</div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div style="text-align: left;">
        <asp:UpdatePanel ID="Report_update" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Table ID="buttonsTable" CellPadding="3" CellSpacing="0" Width="100%" CssClass="DetailsBrowseTable" runat="server">
                    <asp:TableRow>
                        <asp:TableCell ID="TableCell_help" runat="server" HorizontalAlign="right" VerticalAlign="middle">
                            <span class="backgroundShade">
                                <asp:LinkButton ID="close_button" runat="server" OnClientClick="javascript:window.close();" CssClass="float_right"><img src="/images/x.svg" alt="Close" /></asp:LinkButton>

                                <a href="#" onclick="javascript:load('help.aspx','','');"
                                    class="float_right" title="Show Reports Help">
                                    <img src="/images/help-circle.svg" alt="Help" /></a>
                                <asp:LinkButton ID="btnRunReport" runat="server" CssClass="noBefore gray_button float_right" ToolTip="Run selected report with options chosen"
                                    Visible="false"><strong>Run Report</strong></asp:LinkButton>&nbsp;&nbsp;
                <asp:LinkButton ID="btnSampleID" runat="server" CssClass="noBefore gray_button float_right" ToolTip="Show sample report in selected report format"
                    Visible="false"><strong>Sample Report</strong></asp:LinkButton>
                            </span>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div class="NotesHeader" style="margin-bottom: 3px;">
                </div>
                <p class="nonflyout_info_box remove_margin" align="left">
                    <asp:Label runat="server" ID="help_text_label"></asp:Label>
                </p>
                <br />
                <cc1:TabContainer ID="main_tab_container" runat="server" Visible="true" CssClass="dark-theme" AutoPostBack="true" Width="100%"
                    Style="margin-left: auto; margin-right: auto;">
                    <cc1:TabPanel ID="select_report_panel" runat="server" Visible="true" HeaderText="Select Report">
                        <HeaderTemplate>
                            Select Report
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table class="data_aircraft_grid" border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td class="seperator_row" valign='top' align="left" style="text-align: left; vertical-align: top;">
                                        <b>Type of Report:</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="padding_table">
                                        <table width='100%' cellpadding="2" cellspacing="0" border="0">
                                            <tr>
                                                <td valign="top" width='40%' align="left">
                                                    <asp:Panel ID="Report_Panel" Visible="False" runat="server">
                                                        <asp:ListBox ID="reportsListID" runat="server" Height="150px" Width="300px" AutoPostBack="True"></asp:ListBox>
                                                    </asp:Panel>
                                                    <asp:Panel ID="report_pages" runat="server" Visible="False">
                                                        <br />
                                                        <b>PAGES TO INCLUDE IN REPORT</b>
                                                        <br />
                                                        <asp:Table ID="pages_table" runat="server">
                                                            <asp:TableRow ID="tr_cover" runat="server">
                                                                <asp:TableCell ID="TableCell24" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                    <asp:CheckBox ID="check_cover" Checked="True" runat="server" Text=" Cover Page " ToolTip="Spec Page" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="tr_airframe" runat="server">
                                                                <asp:TableCell ID="TableCell25" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                    <asp:CheckBox ID="check_airframe" Checked="True" runat="server" Text=" Airframe Page " ToolTip="Airframe Page" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="tr_enginesAPU" runat="server">
                                                                <asp:TableCell ID="TableCell34" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                    <asp:CheckBox ID="check_engineAPU" Checked="True" runat="server" Text=" Engines/APU Page " ToolTip="Engines/APU Page" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="tr_avionics" runat="server">
                                                                <asp:TableCell ID="TableCell26" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                    <asp:CheckBox ID="check_avionics" Checked="True" runat="server" Text=" Avionics Page " ToolTip="Avionics Page" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="tr_int_ext" runat="server">
                                                                <asp:TableCell ID="TableCell27" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                    <asp:CheckBox ID="check_int_ext" Checked="True" runat="server" Text=" Interior/Exterior Page " ToolTip="Interior/Exterior Page" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="tr_maint" runat="server">
                                                                <asp:TableCell ID="TableCell28" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                    <asp:CheckBox ID="check_maint" Checked="True" runat="server" Text=" Maintenance Page " ToolTip="Maintenance Page" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="spec_page" runat="server">
                                                                <asp:TableCell ID="TableCell29" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                    <asp:CheckBox ID="chkSP_53" Checked="True" runat="server" Text=" Specifications Page " ToolTip="Spec Page" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="notes_page" runat="server">
                                                                <asp:TableCell ID="TableCell30" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                    <asp:CheckBox ID="chkIncludeNotes_53" runat="server" Text=" Notes Page" ToolTip="Local Notes" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="comp_contacts" runat="server">
                                                                <asp:TableCell ID="TableCell31" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                    <asp:CheckBox ID="chkTP_53" Checked="True" runat="server" Text=" Company / Contacts Page " ToolTip="Company and contacts Page" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="history_page" runat="server">
                                                                <asp:TableCell ID="TableCell32" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                    <asp:CheckBox ID="chkIncludeHistory_53" runat="server" Text=" History Page" ToolTip="Historical Transactions" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="picture_pages" runat="server">
                                                                <asp:TableCell ID="TableCell33" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                    <asp:CheckBox ID="chkPP" Checked="True" runat="server" Text=" Picture Page(s)" ToolTip="Pictures Page" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="flight_activity_pages" runat="server">
                                                                <asp:TableCell ID="TableCell35" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                    <asp:CheckBox ID="chkFA" Checked="True" runat="server" Text=" Flight Activity" ToolTip="Flight Activity" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </asp:Panel>
                                                </td>
                                                <td valign="top" align="left">
                                                    <asp:UpdatePanel ID="check_update" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="standard_report_options" runat="server">
                                                                <asp:Label ID="report_description_label" runat="server" Style="font-size: 14px;"></asp:Label>
                                                                <asp:Table ID="reportCheckOptions" CellPadding="3" CellSpacing="0" Width="100%" runat="server">
                                                                    <asp:TableRow ID="TableRow0" runat="server">
                                                                        <asp:TableCell ID="TableCell0" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkShowReportHeader" Checked="True" runat="server" Text=" Include Report Header " ToolTip="Include Header" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow13" runat="server">
                                                                        <asp:TableCell ID="TableCell13" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="logo_check" runat="server" Visible="False" Checked="True" Text=" Include My Company Logo in Header of Report "
                                                                                ToolTip="Include Logo" />
                                                                            <br />
                                                                            <asp:Label ID="logo_image" runat="server" ToolTip="Company Logo"></asp:Label>
                                                                            <br />
                                                                            <asp:CheckBox ID="name_alt_check" runat="server" Text="Include Company Alt Name" Visible="false" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow23" runat="server" Visible="False">
                                                                        <asp:TableCell ID="TableCell23" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="check_prepared_for" runat="server" Text=" Include Prepared For Line " ToolTip="Include Prepared For Line" />
                                                                            <br />
                                                                            &nbsp;&nbsp;&nbsp;<asp:TextBox ID="prepared_for" runat="server" ToolTip="Prepared For:" Width="195px"></asp:TextBox>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow21" runat="server" Visible="False">
                                                                        <asp:TableCell ID="TableCell21" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkIncludeKeyFeatures" Checked="True" runat="server" Text=" Include Key Features" ToolTip="Include Key Features" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow8" runat="server">
                                                                        <asp:TableCell ID="TableCell8" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkBlindReport" runat="server" AutoPostBack="true" Text=" Blind Report (No Serial# / Reg# Information) "
                                                                                ToolTip="Blind Report" /><br />
                                                                            <asp:CheckBox ID="check_blind_company" runat="server" Text="Blind Report (No Company Header Data)" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow1" runat="server">
                                                                        <asp:TableCell ID="TableCell1" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkShowAsking" runat="server" Text=" Include Asking Price " ToolTip="Include Asking" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow2" runat="server">
                                                                        <asp:TableCell ID="TableCell2" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            Add to Asking Amount $&nbsp;&nbsp;
                                      <asp:TextBox ID="addToAsking" runat="server" ToolTip="Amount to add to asking price"></asp:TextBox>

                                                                            <script language="javascript" type="text/javascript">
                                                                                EnableAddAskingTextBox();
                                                                            </script>

                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow12" runat="server">
                                                                        <asp:TableCell ID="TableCell12" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkEB" runat="server" Text=" Exclude Broker " ToolTip="Exclude Broker" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow16" runat="server">
                                                                        <asp:TableCell ID="TableCell16" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkIncludeBaseLocation" Checked="True" runat="server" Text=" Include Aircraft Base Location" ToolTip="Include Aircraft Base Location" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow3" runat="server">
                                                                        <asp:TableCell ID="TableCell3" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkShowConfidential" runat="server" Text=" Include Confidential Notes " ToolTip="Include Confidential" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow4" runat="server">
                                                                        <asp:TableCell ID="TableCell4" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkIncludeContacts" runat="server" Text=" Include Contacts " ToolTip="Include Contacts" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow5" runat="server">
                                                                        <asp:TableCell ID="TableCell5" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkIncludeAllContacts" runat="server" Text=" Include All Contacts " ToolTip="Include All Contacts" />
                                                                            <% If report_id <> 19 Then%>

                                                                            <script language="javascript" type="text/javascript">
                                                                                EnableShowAllContacts();
                                                                            </script>

                                                                            <%End If%>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow6" runat="server">
                                                                        <asp:TableCell ID="TableCell6" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkIncludeBaseAC" runat="server" Text=" Include Base Aircraft " ToolTip="Include Base Aircraft" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow7" runat="server">
                                                                        <asp:TableCell ID="TableCell7" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkIncludeCompAC" runat="server" Text=" Include Company Aircraft " ToolTip="Include Company Aircraft" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow19" runat="server" Visible="False">
                                                                        <asp:TableCell ID="TableCell19" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkincludeCompYacht" runat="server" Text=" Include Company Yacht " ToolTip="Include Company Yacht" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow11" runat="server">
                                                                        <asp:TableCell ID="TableCell11" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="Chk_ALLPIC" Checked="True" runat="server" Text=" Show All Pictures in Pages " ToolTip="Show All Pictures in Pages" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow18" runat="server">
                                                                        <asp:TableCell ID="TableCell18" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chk_related_comp" runat="server" Text="Include Related Companies"></asp:CheckBox>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow20" runat="server">
                                                                        <asp:TableCell ID="TableCell20" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chk_comp_wanteds" runat="server" Text="Include Company Wanteds"></asp:CheckBox>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow22" runat="server" Visible="False">
                                                                        <asp:TableCell ID="TableCell22" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkIncludesale" Checked="True" runat="server" Text=" Include Status/Asking Price" ToolTip="Include Status/Asking Price" />
                                                                            <asp:CheckBox ID="chkCompanyLocations" runat="server" Visible="false" Text="Show All Company Locations" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow17" runat="server">
                                                                        <asp:TableCell ID="TableCell17" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkPP_Large" runat="server" Text=" Show Each Picture On Its Own Page" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow10" runat="server">
                                                                        <asp:TableCell ID="TableCell10" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkTP_std" Checked="True" runat="server" Text=" Company / Contacts Page " ToolTip="Company and contacts Page" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow9" runat="server">
                                                                        <asp:TableCell ID="TableCell9" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkSP_std" Checked="True" runat="server" Text=" Specifications Page " ToolTip="Spec Page" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow15" runat="server">
                                                                        <asp:TableCell ID="TableCell15" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkIncludeHistory_std" runat="server" Text=" History Page" ToolTip="Historical Transactions" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow14" runat="server">
                                                                        <asp:TableCell ID="TableCell14" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chkIncludeNotes_std" runat="server" Text=" Notes Page" ToolTip="Local Notes" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="TableRow24" runat="server">
                                                                        <asp:TableCell ID="TableCell36" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;" runat="server">
                                                                            <asp:CheckBox ID="chk_include_evalues" runat="server" Text=" Include eValues" ToolTip="Include eValues" Visible="false" />
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="operator_tablerow" runat="server">
                                                                        <asp:TableCell ID="operator_tablecell" runat="server" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;">
                                                                            Aircraft Operations
                                      <ul>
                                          <asp:CheckBox ID="op_locations" runat="server" Text="Operating Locations" Visible="false" /><br />
                                          <asp:CheckBox ID="op_certs" runat="server" Text="Operating Certifications" /><br />
                                          <asp:CheckBox ID="op_models" runat="server" Text="Models Operated" /><br />
                                          <asp:CheckBox ID="op_util_chart" runat="server" Text="Utilization Summary Chart" /><br />
                                          <asp:CheckBox ID="op_airport" runat="server" Text="Airport Utilization" />
                                      </ul>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="owner_tablerow" runat="server">
                                                                        <asp:TableCell ID="owner_tablecell" runat="server" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;">
                                                                            Aircraft Ownership
                                      <ul>
                                          <asp:CheckBox ID="own_locations" runat="server" Text="Ownership Locations" Visible="false" /><br />
                                          <asp:CheckBox ID="own_history" runat="server" Text="Ownership History" /><br />
                                          <asp:CheckBox ID="own_purchase_chart" runat="server" Text="Purcahse History Chart" />
                                      </ul>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="leasing_tablerow" runat="server">
                                                                        <asp:TableCell ID="leasing_tablecell" runat="server" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;">
                                                                            Aircraft Leasing
                                      <ul>
                                          <asp:CheckBox ID="lease_locations" runat="server" Text="Lessor Locations" Visible="false" /><br />
                                          <asp:CheckBox ID="lease_models" runat="server" Text="Models Leased" /><br />
                                          <asp:CheckBox ID="lease_per_month_chart" runat="server" Text="Leases Per Month Chart" /><br />
                                          <asp:CheckBox ID="lease_summary" runat="server" Text="Summary of Leases" />
                                      </ul>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="finance_tablerow" runat="server">
                                                                        <asp:TableCell ID="finance_tablecell" runat="server" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;">
                                                                            Financial Documents
                                      <ul>
                                          <asp:CheckBox ID="fin_models" runat="server" Text="Aircraft Models Financed" /><br />
                                          <asp:CheckBox ID="fin_documents_chart" runat="server" Text="Documents by Month Chart" /><br />
                                          <asp:CheckBox ID="fin_related" runat="server" Text="Related Financing Companies" /><br />
                                          <asp:CheckBox ID="fin_types" runat="server" Text="Types of Financial Documents" />
                                      </ul>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="dealer_tablerow" runat="server">
                                                                        <asp:TableCell ID="dealer_tablecell" runat="server" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;">
                                                                            Dealer Performance
                                      <ul>
                                          <asp:CheckBox ID="dealer_locations" runat="server" Text="Company Locations" Visible="false" /><br />
                                          <asp:CheckBox ID="dealer_models" runat="server" Text="Models Represented" /><br />
                                          <asp:CheckBox ID="dealer_sales" runat="server" Text="Dealer Sales Per Year" /><br />
                                          <asp:CheckBox ID="dealer_roles" runat="server" Text="Dealer Sales Roles" /><br />
                                          <asp:CheckBox ID="dealer_sales_by_model" runat="server" Text="Dealer Sales by Model" />
                                      </ul>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow ID="manu_tablerow" runat="server">
                                                                        <asp:TableCell ID="manu_tablecell" runat="server" HorizontalAlign="Left" VerticalAlign="Top" Style="padding-right: 4px;">
                                                                            Aircraft Manufactured
                                      <ul>
                                          <asp:CheckBox ID="manu_summary" runat="server" Text="Manufacturer Summary" /><br />
                                          <asp:CheckBox ID="manu_models" runat="server" Text="Models Manufactured" /><br />
                                          <asp:CheckBox ID="manu_in_operation" runat="server" Text="In Operation Aircraft by Year Chart" /><br />
                                          <asp:CheckBox ID="manu_in_production" runat="server" Text="In Production Aircraft by Year Chart" />
                                      </ul>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                </asp:Table>
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Panel ID="features_panel" Visible="False" runat="server">
                                                        <asp:CheckBox runat="server" ID="customFeatureList" Text="Display Custom Feature List Below" />
                                                        <b>Features/Highlights</b><br />
                                                        <asp:TextBox ID="features_text" runat="server" TextMode="MultiLine" Height="340px" Width="280px"></asp:TextBox>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="seperator_row" align="left" valign="top" style="text-align: left; vertical-align: top;">
                                        <asp:Panel ID="submit_report_panel" runat="server">
                                            <asp:CheckBox runat="server" ID="chkSubmitReport" Text="Submit a request for Jetnet to run this report" ToolTip="Jetnet will process the report and email you a link to download the results" />
                                            &nbsp;&nbsp;Name&nbsp;:&nbsp;<asp:TextBox ID="replyUsername" runat="server" ToolTip="Reply to User Name" Width="195px"></asp:TextBox>
                                            &nbsp;&nbsp;Email&nbsp;:&nbsp;<asp:TextBox ID="replyEmail" runat="server" ToolTip="Send report to User Email address" Width="245px"></asp:TextBox>

                                            <script language="javascript" type="text/javascript">
                                                toggleRunButtonName();
                                            </script>

                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table class="data_aircraft_grid" border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td class="header_row" align="left" valign="top">
                                        <b>Export / Report Formats :</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="seperator_row" align="left" valign="top">
                                        <asp:RadioButtonList ID="reportType" RepeatLayout="Flow" runat="server" RepeatDirection="Horizontal" Visible="False">
                                        </asp:RadioButtonList>
                                        <br />
                                        <asp:Label ID="HelpText2" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:Image ID="img1" runat="server" Visible="False" />
                            <cc1:TabPanel ID="invisible_panel" runat="server" Visible="False">
                            </cc1:TabPanel>
                            <cc1:TabContainer ID="invisible_containor" runat="server" Visible="False" CssClass="">
                            </cc1:TabContainer>
                            <asp:Label ID="invisible_label" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="invisible_label2" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="invisible_label3" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="invisible_label4" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="invisible_no_use_label" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="invisible_label_parent_image" runat="server" Visible="False"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:Chart ID="AVG_PRICE_MONTH" Visible="false" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg">
        <Series>
            <asp:Series Name="Series1">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <asp:Button ID="save_checks" runat="server" Text="Remember Report Selections" Visible="false" />
    <asp:TextBox ID="start_date" runat="server" Text="1/1/2017" Visible="false"></asp:TextBox>
    <asp:TextBox ID="end_date" runat="server" Text="1/1/2018" Visible="false"></asp:TextBox>
    <div id="graphContainer" style="visibility: hidden;">
        <div id="chart_div_tab13_all" style="border-top: 0;">
        </div>
        <asp:TextBox runat="server" ID="valueGraphText13"></asp:TextBox>
        <div id='png13' runat="server" clientidmode="Static">
        </div>
        <div id="chart_div_tabVal1_all" style="border-top: 0;">
        </div>
        <asp:TextBox runat="server" ID="valueGraphTextVal1"></asp:TextBox>
        <div id='pngVal1' runat="server" clientidmode="Static">
        </div>
        <div id="chart_div_tabVal2_all" style="border-top: 0;">
        </div>
        <asp:TextBox runat="server" ID="valueGraphTextVal2"></asp:TextBox>
        <div id='pngVal2' runat="server" clientidmode="Static">
        </div>
    </div>
    <asp:UpdatePanel ID="valuesUpdatePanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label runat="server" ID="values_label" CssClass="valueSpec viewValueExport Simplistic aircraftSpec"></asp:Label>
            <asp:Chart ID="valuation_chart" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg" Visible="False">
                <Series>
                    <asp:Series>
                    </asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
