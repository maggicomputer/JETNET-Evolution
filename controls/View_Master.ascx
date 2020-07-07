<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="View_Master.ascx.vb"
    Inherits="crmWebClient.View_Master" %>
<%@ Register Src="viewTypeMakeModel.ascx" TagName="viewTMMDropDowns_ViewSpecific"
    TagPrefix="evo" %>
<%@ Register Src="continentRegionDropdowns.ascx" TagName="viewCCSTDropDowns_ViewSpecific"
    TagPrefix="evo" %>
<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting"
    TagPrefix="asp" %>

<script type="text/javascript" src="https://cdn.rawgit.com/Mikhus/canvas-gauges/gh-pages/download/2.1.4/all/gauge.min.js"></script>

<script type="text/javascript">


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

<style type="text/css">
    /*  #mostCommonOriginsDestinations_wrapper .dataTables_scrollHead
  {
    width: fit-content !important;
  }*/ .ui-menu {
        z-index: 2 !important;
    }

    [ unselectable=on ] {
        -webkit-user-select: none; /* Chrome all / Safari all */
        -moz-user-select: none; /* Firefox all */
        -ms-user-select: none; /* IE 10+ */
        user-select: none; /* Likely future */
    }
</style>
<div class="clearfix"></div>
<p class="DetailsBrowseTable">
    <span class="backgroundShade">

        <asp:Label runat="server" ID="evo_view_help" Visible="true">
            <a class="underline" onclick="javascript:load('help.aspx?t=2','','');" >
                <img src="/images/help-circle.svg" class="float_left" border="0" alt="Show View Help" title="Show View Help"  />
            </a>
        </asp:Label>
        <asp:ImageButton runat="server" ID="crm_view_help" Visible="false" OnClientClick='javascript:load("http://www.jetnetcrm.com/support/ProspectManagementHelp.pdf","",""); return false;'
            ImageUrl="/images/help-circle.svg" CssClass="float_left help_cursor" ToolTip="Show View Help"
            AlternateText="Show View Help" />
        <a href="#" class="float_right" onclick="javascript:window.close();">
            <img src="/images/x.svg" alt="Close" /></a></span><div class="clear"></div>
</p>
<table class="centerTable" id="mainTableID" width='100%' border="0" cellpadding="0"
    cellspacing="0">
    <tr>
        <td align="center" style="text-align: center; padding-left: 0px;">
            <div id="DivLoadingMessage" class="loadingScreenBox" style="display: none;">
                <span>Loading </span>
                <div class="loader">Loading...</div>
            </div>
            <div id="divTabLoading2" class="loadingScreenBox" style="display: none;">
               <span>Loading </span>
                <div class="loader">Loading...</div>
            </div>
            <div id="divLoading" runat="server" class="loadingScreenBox" style="display: none;">
                <span>Loading </span>
                <div class="loader">Loading...</div>
            </div>

            <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="bottom_tab_update_panel"
                runat="server" DisplayAfter="500" class="loadingScreenBox">
                <ProgressTemplate>
                    <span>Loading </span>
                    <div class="loader">Loading...</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdateProgress ID="UpdateProgress5" AssociatedUpdatePanelID="top_tab_update_panel"
                runat="server" DisplayAfter="500" class="loadingScreenBox">
                <ProgressTemplate>
                    <span>Loading </span>
                    <div class="loader">Loading...</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="top_tab_update_panel"
                runat="server" DisplayAfter="500" class="loadingScreenBox">
                <ProgressTemplate>
                    <span>Loading </span>
                    <div class="loader">Loading...</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:Panel runat="server" ID="login_warning_panel" CssClass="loadingScreenPage" Visible="false">
                <asp:Label runat="server" ID="login_warning_text"></asp:Label>
            </asp:Panel>
            <asp:Panel ID="loaded_visibility" runat="server" CssClass="display_none" Width="100%"
                HorizontalAlign="Center" ChildrenAsTriggers="True">
                <asp:Label runat="server" ID="attention" ForeColor="Red" Font-Bold="true"></asp:Label>
                <table cellpadding="0" cellspacing="0" align="center" width="100%">
                    <tr>
                        <td valign="top" align="center" class="mobileAlignLeft">
                            <asp:Panel ID="parent_toggle" runat="server">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="mobileWidth">
                                    <tr>
                                        <td align="left" valign="top" class="dark_header">
                                            <table width="100%" cellpadding="3" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="middle" width="20%" runat="server" id="newSearchContainer">

                                                        <asp:Panel ID="Control_Panel" runat="server" Width="100%">
                                                            <asp:Image ID="ControlImage" runat="server" ImageUrl="../images/search_expand.jpg" />
                                                        </asp:Panel>
                                                    </td>
                                                    <td align="left" valign="bottom" style="padding-bottom: 10px;" width="80%" nowrap="nowrap">
                                                        <asp:Panel runat="server" ID="MobileSearchVisible" Visible="false">
                                                            <asp:DropDownList runat="server" AutoPostBack="true" ID="makeModelDynamic" CssClass="chosen-select"
                                                                Width="100%">
                                                                <asp:ListItem>Please pick a Model</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </asp:Panel>
                                                        <asp:Label ID="breadcrumbs" runat="server" CssClass="float_left criteria_text"></asp:Label>
                                                    </td>
                                                    <td align="right" valign="bottom" style="padding-bottom: 10px;" width="10%" nowrap="nowrap"
                                                        runat="server" id="dropdownCell">
                                                        <div class="action_dropdown_container">
                                                            <asp:BulletedList ID="actions_dropdown" runat="server" CssClass="ul_top" Visible="false">
                                                                <asp:ListItem>Actions</asp:ListItem>
                                                            </asp:BulletedList>
                                                            <asp:BulletedList ID="actions_submenu_dropdown" runat="server" CssClass="ul_bottom ac_action_dropdown"
                                                                DisplayMode="HyperLink" OnClick="submenu_dropdown_Click">
                                                                <asp:ListItem Value="javascript:alert('client side reaction - server side is submenu_dropdown_Click');">Test Item 1</asp:ListItem>
                                                            </asp:BulletedList>
                                                        </div>
                                                    </td>
                                                    <td align="left" valign="bottom" style="padding-bottom: 10px;" nowrap="nowrap" runat="server"
                                                        id="buttonsCell">
                                                        <table cellpadding='0' cellspacing='0' border='0'>
                                                            <tr>
                                                                <td align="left" valign="bottom" class="display_none" runat="server" id="exportReportCell">
                                                                    <div class="action_dropdown_container" style="margin-right: 15px;">
                                                                        <asp:BulletedList ID="ParentExportReport" runat="server" CssClass="ul_top">
                                                                            <asp:ListItem>Export/Report</asp:ListItem>
                                                                        </asp:BulletedList>
                                                                        <asp:BulletedList ID="ChildExportReport" runat="server" CssClass="ul_bottom" DisplayMode="hyperlink">
                                                                            <asp:ListItem Value="javascript:CreateExport('39','','single_page_spec');">Single Spec</asp:ListItem>
                                                                            <asp:ListItem Value="javascript:CreateExport('40','','Short_Spec_Sheet');">Condensed Spec</asp:ListItem>
                                                                            <asp:ListItem Value="javascript:CreateExport('53','','Spec_Sheet');">Full Spec</asp:ListItem>
                                                                        </asp:BulletedList>
                                                                    </div>

                                                                    <script type="text/javascript">

                                                                        function CreateExport(reportID, folder_type, page) {

                                                                            my_form = document.createElement('FORM');
                                                                            my_form.method = 'POST';
                                                                            my_form.target = "_blank"
                                                                            my_form.action = 'viewtopdf.aspx';
                                                                            my_form.name = 'folderForm';

                                                                            my_tb = document.createElement('INPUT');
                                                                            my_tb.type = 'HIDDEN';
                                                                            my_tb.name = "r_id";
                                                                            my_tb.value = reportID
                                                                            my_form.appendChild(my_tb);

                                                                            my_tb = document.createElement('INPUT');
                                                                            my_tb.type = 'HIDDEN';
                                                                            my_tb.name = "page";
                                                                            my_tb.value = page
                                                                            my_form.appendChild(my_tb);




                                                                            var clientIDs = "";
                                                                            var jetnetIDs = "";
                                                                            var BreakableIDArray = $('#<%= fullSaleCurrentIDs.clientID %>').val().split(", ");
                                                                            if (BreakableIDArray) {
                                                                                var arrayLength = BreakableIDArray.length;
                                                                                for (var i = 0; i < arrayLength; i++) {
                                                                                    if (BreakableIDArray[i].indexOf("|CLIENT") > 0) {
                                                                                        if (clientIDs !== "") {
                                                                                            clientIDs += ","
                                                                                        }
                                                                                        clientIDs += BreakableIDArray[i].replace("|CLIENT", "").trim()
                                                                                    } else {
                                                                                        if (jetnetIDs !== "") {
                                                                                            jetnetIDs += ","
                                                                                        }
                                                                                        jetnetIDs += BreakableIDArray[i].replace("|JETNET", "").trim()
                                                                                    }
                                                                                }
                                                                            }

                                                                            //Client
                                                                            my_tb = document.createElement('INPUT');
                                                                            my_tb.type = 'HIDDEN';
                                                                            my_tb.name = "clientIDs";
                                                                            my_tb.value = clientIDs;
                                                                            my_form.appendChild(my_tb);

                                                                            //Jetnet
                                                                            my_tb = document.createElement('INPUT');
                                                                            my_tb.type = 'HIDDEN';
                                                                            my_tb.name = "jetnetIDs";
                                                                            my_tb.value = jetnetIDs;
                                                                            my_form.appendChild(my_tb);

                                                                            document.body.appendChild(my_form);
                                                                            my_form.submit();
                                                                        }
                                                                    </script>

                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="standard_pdf_button" class="pdf_button underline cursor float_right criteria_text"
                                                                        runat="server" Visible="false">&nbsp;&nbsp;Standard</asp:LinkButton>
                                                                    <asp:Label ID="crmProspectViewNewProspect" runat="server" CssClass="float_right criteria_text"
                                                                        Visible="false">+<a href="javascript:void(0);" onclick="javascript:load('/edit_note.aspx?action=new&ViewID=18&type=prospect&cat_key=0&refreshing=prospect&comp_ID=0&ac_ID=0','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');">ADD NEW PROSPECT</a></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="buttons" runat="server" CssClass="float_right criteria_text"></asp:Label>
                                                                    <asp:Label ID="crmProspectViewExportLink" runat="server" CssClass="float_right criteria_text"
                                                                        Visible="false"><a href="javascript:void(0);" onclick="$('#<%= crmProspectSearchExport.ClientID %>').prop('checked', true);$('#<%= crmProspectSearchButton.clientID%>').click();$('#loadingScreenViewSearchText').text('Exporting all prospects to Excel. This may take a few minutes. Please wait...');">Export Prospects</a></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Label ID="close_window_only" runat="server" CssClass="float_right criteria_text"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <asp:UpdateProgress ID="UpdateProgress4" AssociatedUpdatePanelID="airportsSearchUpdate"
                                    runat="server" DisplayAfter="500" class="loadingScreenBox">
                                    <ProgressTemplate>
                                        <span>Loading </span>
                                        <div class="loader">Loading...</div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <cc1:CollapsiblePanelExtender ID="PanelCollapseEx" runat="server" TargetControlID="Collapse_Panel"
                                    Collapsed="true" ExpandControlID="Control_Panel" ImageControlID="ControlImage"
                                    BehaviorID="searchCollapseEx" ExpandedImage="../images/search_collapse.jpg" CollapsedImage="../images/search_expand.jpg"
                                    CollapseControlID="Control_Panel" Enabled="True" CollapsedText="New Search" ExpandedText="Hide Search">
                                </cc1:CollapsiblePanelExtender>
                                <div id="viewCriteriaDivID">
                                    <asp:Panel ID="Collapse_Panel" runat="server" Height="0px" Width="100%" CssClass="collapse">
                                        <asp:Table ID="ViewCriteriaBlock" runat="server" Width="100%" CellPadding="3" CellSpacing="0">
                                            <asp:TableRow>
                                                <asp:TableCell ID="cellTypeMakeModel" HorizontalAlign="left" VerticalAlign="top"
                                                    Width="50%" Visible="true" ColumnSpan="2">
                                                    <asp:Panel ID="opcosts_make_model_panel" runat="server">
                                                        <evo:viewTMMDropDowns_ViewSpecific ID="ViewTMMDropDowns" runat="server" />

                                                        <script language="javascript" type="text/javascript">
                              refreshTypeMakeModelByCheckBox("", "", <%= isHeliOnlyProduct.tostring.tolower%>,<%= productCodeCount.tostring%>);
                                                        </script>
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" ID="toggleFlightSummaryPanel" Visible="false">
                                                        <asp:CheckBox runat="server" ID="toggleFlightSummary" Visible="false" Checked="true"
                                                            Text="Display Flight Summary" />
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" ID="searchUtilizationBasedOn" Visible="false">
                                                        <span class="float_left">Based On:
                              <asp:DropDownList runat="server" ID="searchBasedOnDropdown">
                                  <asp:ListItem Selected="True" Value="D">Arrivals</asp:ListItem>
                                  <asp:ListItem Value="O">Departures</asp:ListItem>
                                  <asp:ListItem Value="X">Arrivals/Departures</asp:ListItem>
                              </asp:DropDownList>
                                                        </span><span class="float_left">&nbsp;&nbsp;&nbsp;&nbsp; </span><span class="float_left">Distance:
                              <asp:DropDownList ID="distance_compare" runat="server">
                                  <asp:ListItem Value=""> </asp:ListItem>
                                  <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                  <asp:ListItem Value="Less Than">Less Than</asp:ListItem>
                                  <asp:ListItem Value="Greater Than">Greater Than</asp:ListItem>
                                  <asp:ListItem Value="Between">Between</asp:ListItem>
                              </asp:DropDownList>
                                                            <asp:TextBox ID="distance_value" runat="server" Width="90" ToolTip="Distance Value"
                                                                Rows="1" Height="12px" ValidationGroup="String" TextAlign="right"></asp:TextBox>
                                                        </span>
                                                        <br />
                                                        <span class="float_left">&nbsp;&nbsp;&nbsp;&nbsp; </span><span class="float_left">Lifecycle:
                                  <asp:DropDownList ID="in_operation_drop" runat="server">
                                      <asp:ListItem Text="All">All</asp:ListItem>
                                      <asp:ListItem Text="In-Operation">In-Operation</asp:ListItem>
                                      <asp:ListItem Text="Out of Operation">Out of Operation</asp:ListItem>
                                  </asp:DropDownList>
                                                        </span>
                                                    </asp:Panel>
                                                </asp:TableCell>
                                                <asp:TableCell ID="cellTimeSpan" HorizontalAlign="left" VerticalAlign="middle" Visible="true">
                                                    <span class="float_left">
                                                        <asp:Label runat="server" ID="timespanText">Time&nbsp;Span&nbsp;for&nbsp;View&nbsp;:</asp:Label><br />
                                                        <asp:DropDownList ID="selectViewTimeSpan" runat="server" OnClientClick='' ToolTip="Select View Time Span">
                                                            <asp:ListItem Value="3">3 Months</asp:ListItem>
                                                            <asp:ListItem Value="6">6 Months</asp:ListItem>
                                                            <asp:ListItem Value="9">9 Months</asp:ListItem>
                                                            <asp:ListItem Value="12">1 Year</asp:ListItem>
                                                            <asp:ListItem Value="24">2 Years</asp:ListItem>
                                                            <asp:ListItem Value="36">3 Years</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
                                                    <asp:Panel runat="server" ID="valuesCheckboxHolderToggle" CssClass="float_right"
                                                        Visible="false">
                                                        <asp:CheckBox runat="server" ID="valuesCheckbox" />
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" ID="searchDateRange" Visible="false">
                                                        <div class="float_left" style="padding-top: 7px; margin-bottom: -15px;">
                                                            Start Date:
                              <asp:TextBox runat="server" ID="airportViewStartDate" Width="68px" />
                                                            &nbsp;End Date:
                              <asp:TextBox runat="server" Width="68px" ID="airportViewEndDate" />
                                                        </div>
                                                        <div class="float_right" style="width: 175px">
                                                            <asp:UpdatePanel runat="server" ID="airportsSearchUpdate">
                                                                <ContentTemplate>
                                                                    <asp:Button runat="server" CssClass="float_right" ID="goSearchAirports" Text="Search"
                                                                        ValidationGroup="airportSearch" CausesValidation="true" Style="padding: 6px 7px;" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <span class="float_right">
                                                                <asp:Button runat="server" CssClass="float_right" ID="clearAirportCriteria" Text="Clear Selections"
                                                                    OnClientClick="setUpAirportPanel();" CausesValidation="false" Style="padding: 6px 7px;" />
                                                            </span>
                                                        </div>
                                                        <span class="float_left" style="clear: both;">
                                                            <asp:CompareValidator ID="dateValidator" ValidationGroup="airportSearch" runat="server"
                                                                Type="Date" Operator="DataTypeCheck" ControlToValidate="airportViewStartDate" ErrorMessage="Please use format mm/dd/yyyy for Start.*"
                                                                Font-Italic="true" CssClass="float_right  display_inline_block padding_left" Font-Bold="true"></asp:CompareValidator>
                                                            <asp:CompareValidator ID="CompareValidator1" ValidationGroup="airportSearch" runat="server"
                                                                Type="Date" Operator="DataTypeCheck" ControlToValidate="airportViewEndDate" ErrorMessage="Please use format mm/dd/yyyy for End.*"
                                                                Font-Italic="true" CssClass="float_right display_inline_block" Font-Bold="true"></asp:CompareValidator>
                                                        </span>
                                                    </asp:Panel>
                                                    <asp:Panel ID="searchSliders" Visible="false" runat="server" CssClass="searchPanelContainerDiv"
                                                        Style="padding-top: 25px; clear: both">
                                                        <div class="row div_clear toggleSmallScreen">
                                                            <div class="one columns removeLeftMargin">
                                                                <label>
                                                                    Year(s):</label>
                                                            </div>
                                                            <div class="two columns">
                                                                <asp:TextBox runat="server" ID="year_start" CssClass="padding_right amount float_right"></asp:TextBox><asp:TextBox
                                                                    runat="server" ID="hidden_year_start" CssClass="display_none" Text="0"></asp:TextBox><asp:TextBox
                                                                        runat="server" ID="hidden_year_end" CssClass="display_none" Text="0"></asp:TextBox>
                                                            </div>
                                                            <div class="four columns removeLeftMargin">
                                                                <div id="slider-range">
                                                                </div>
                                                            </div>
                                                            <div class="two columns">
                                                                <asp:TextBox runat="server" ID="year_end" CssClass="amount float_left text_align_left_important"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="row toggleSmallScreen">
                                                            <div class="one columns removeLeftMargin">
                                                                <label>
                                                                    AFTT:</label>
                                                            </div>
                                                            <div class="two columns">
                                                                <asp:TextBox runat="server" ID="hidden_aftt_start" CssClass="display_none" Text="0"></asp:TextBox><asp:TextBox
                                                                    runat="server" Text="0" ID="hidden_aftt_end" CssClass="display_none"></asp:TextBox>
                                                                <asp:TextBox runat="server" ID="aftt_start" CssClass="padding_right amount float_right"></asp:TextBox>
                                                            </div>
                                                            <div class="four columns removeLeftMargin">
                                                                <div id="aftt-range">
                                                                </div>
                                                            </div>
                                                            <div class="two columns">
                                                                <asp:TextBox runat="server" ID="aftt_end" CssClass="amount float_left text_align_left_important"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <span class="tiny red_text" id="evoSliderText" runat="server">Sliders only applied
                              to Market Status, Trends and For Sale/Sold Survey charts and tables.*</span>
                                                    </asp:Panel>
                                                </asp:TableCell>
                                                <asp:TableCell ID="prospectCRMSearchCell" Visible="false">
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left" valign="middle" width="60">
                                                                <asp:Label runat="server" ID="search_for" Text="Search For:" Visible="true"></asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="200">
                                                                <asp:TextBox runat="server" ID="crmProspectSearchText" Width="97%"></asp:TextBox>
                                                                <asp:CheckBox runat="server" ID="crmProspectSearchExport" CssClass="display_none" />
                                                                <asp:TextBox runat="server" ID="crmProspectAircraftID" Width="97%" Text="0" CssClass="display_none"></asp:TextBox>
                                                            </td>
                                                            <td align="left" valign="middle" width="170">
                                                                <asp:DropDownList runat="server" ID="crmProspectSearchTextWhere">
                                                                    <asp:ListItem Selected="True" Value="2">Begins With</asp:ListItem>
                                                                    <asp:ListItem Value="1">Anywhere</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left" valign="middle" width="60">
                                                                <asp:Label runat="server" ID="search_by_label" Text="Search By:" Visible="true"></asp:Label>
                                                            </td>
                                                            <td align="left" valign="middle" width="140">
                                                                <asp:DropDownList runat="server" ID="crmProspectSearchTypeOfProspect">
                                                                    <asp:ListItem Value="1">Prospects Assigned to Aircraft</asp:ListItem>
                                                                    <asp:ListItem Value="2">Prospects Assigned to Model</asp:ListItem>
                                                                    <asp:ListItem Value="3" Selected="True">All Prospects</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left" valign="middle">
                                                                <asp:CheckBox runat="server" ID="crmProspectSearchIncludeActive" Text="Open Prospects"
                                                                    Checked="true" /><asp:CheckBox runat="server" ID="crmProspectSearchIncludeClosed"
                                                                        Text="Closed Deal" />
                                                                <asp:CheckBox runat="server" ID="crmProspectSearchIncludeInactive" Text="Inactive Prospects" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="middle">
                                                                <asp:Label runat="server" ID="category_label" Text="Category:"></asp:Label>
                                                            </td>
                                                            <td align="left" valign="middle">
                                                                <asp:DropDownList runat="server" ID="crmProspectSearchCategory" Width="97%">
                                                                    <asp:ListItem Selected="True" Value="0">Please Select One</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:DropDownList runat="server" ID="crmProspectSearchOrder" Width="97%">
                                                                    <asp:ListItem Value="Make">Make/Model/Ser #/Company</asp:ListItem>
                                                                    <asp:ListItem Selected="True" Value="Company">Company/Make/Model/Ser #</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>Staff:
                                                            </td>
                                                            <td align="left" valign="middle">
                                                                <asp:DropDownList runat="server" ID="crmProspectSearchUserDropdown" Width="100%">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left" valign="middle">
                                                                <asp:CheckBox runat="server" ID="crmProspectSearchUserInactive" Text="Include Inactive"
                                                                    CssClass="display_none" />&nbsp;&nbsp; Target/Closing Date(s):
                                <asp:TextBox runat="server" ID="crmProspectSearchStartDate" Width="68px" />
                                                                &nbsp;/
                                <asp:TextBox runat="server" Width="68px" ID="crmProspectSearchEndDate" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="middle" colspan="5">
                                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td align="left" valign="top" width="84px">
                                                                            <asp:Label runat="server" ID="action_label" Text="Action Taken/Priority:"></asp:Label>
                                                                        </td>
                                                                        <td align="left" valign="top">
                                                                            <asp:ListBox runat="server" ID="crmProspectActionTakenList" Width="96%" SelectionMode="Multiple"></asp:ListBox>
                                                                        </td>
                                                                        <td align="left" valign="top" width="60px">
                                                                            <asp:Label runat="server" ID="source_label" Text="Source:"></asp:Label>
                                                                        </td>
                                                                        <td align="left" valign="top">
                                                                            <asp:ListBox runat="server" ID="source_listbox" Width="98%" SelectionMode="Multiple"></asp:ListBox>
                                                                        </td>
                                                                        <td align="left" valign="top" width="104px">
                                                                            <asp:Label runat="server" ID="referrer_label" Text="Referrer:"></asp:Label>
                                                                        </td>
                                                                        <td align="left" valign="top">
                                                                            <asp:DropDownList runat="server" ID="referrer_drop" Width="100%">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="right" colspan="5">
                                                                <asp:Button runat="server" ID="crmProspectSearchButton" Text="Search" ToolTip="Click to Apply Critera"
                                                                    UseSubmitBehavior="false" />
                                                                <asp:Button runat="server" ID="crmProspectViewAcSearchButton" Text="Search" ToolTip="Click to Apply Critera"
                                                                    UseSubmitBehavior="false" CssClass="display_none" />
                                                                <asp:Button runat="server" ID="crmProspectSearchClearButton" Text="Clear Selections"
                                                                    UseSubmitBehavior="false" ToolTip="Click to Clear Critera" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow runat="server">
                                                <asp:TableCell runat="server" ColumnSpan="2" VerticalAlign="Top" ID="airportFBOViewSearchCell"
                                                    Visible="false">
                                                    <table width="100%" cellpadding="5" cellspacing="0" class="data_aircraft_grid">
                                                        <tr class="header_row">
                                                            <td colspan="5" valign="top" align="left"></td>
                                                        </tr>
                                                    </table>
                                                    <asp:UpdateProgress ID="UpdateProgress3" AssociatedUpdatePanelID="airportFBOView_UpdatePanel"
                                                        runat="server" DisplayAfter="500">
                                                        <ProgressTemplate>
                                                            <div id="Div1" runat="server" class="loadingScreenViewSearch">
                                                                <span>Please wait while the Search Results are loading......... </span>
                                                                <br />
                                                                <br />
                                                                <img src="Images/loading.gif" alt="Loading..." /><br />
                                                            </div>
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                    <asp:UpdatePanel runat="server" ID="airportFBOView_UpdatePanel" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="Panel1" runat="server">
                                                                <table width="100%" cellpadding="0" cellspacing="0" class="data_aircraft_grid override_borders">
                                                                    <tr class="header_row">
                                                                        <td runat="server" id="airportsHeaderWidth" width="225px" valign="top" align="left"
                                                                            style="padding-left: 7px;">
                                                                            <b>AIRPORTS</b>
                                                                            <asp:DropDownList runat="server" ID="selectAirportsType" onchange="setUpAirportPanel();"
                                                                                Width="144px" Visible="false" Style="margin-left: 10px; margin-top: 5px; margin-bottom: 5px;">
                                                                                <asp:ListItem Value="1">All Airports</asp:ListItem>
                                                                                <asp:ListItem Value="3">Select New Airport</asp:ListItem>
                                                                                <asp:ListItem Value="5">Airport Code(s)</asp:ListItem>
                                                                                <asp:ListItem Value="4">Select Airport Folder</asp:ListItem>
                                                                                <asp:ListItem Value="0">Create Airport Folder</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:CheckBox ID="avis_Check" runat="server" class="display_none" Checked="false"
                                                                                Visible="true" />
                                                                        </td>
                                                                        <td align="left" valign="middle" width="237px">
                                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                                <tr id="airportIataToggle" runat="server" class="display_none">
                                                                                    <td align="left" valign="top" width="400px">
                                                                                        <asp:TextBox runat="server" ID="airportIATABoxSearch" Width="228px" placeholder="Please list Airport Codes"
                                                                                            TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                                                    </td>
                                                                                    <td align="left" valign="top"></td>
                                                                                    <td align="left" valign="top"></td>
                                                                                </tr>
                                                                                <tr id="airportSearchToggleOnOff" runat="server" class="display_none">
                                                                                    <td align="left" valign="top" width="400px">
                                                                                        <asp:TextBox runat="server" ID="fboViewSearch_textbox" Width="400px" placeholder="Please search by Airport IATA, ICAO, City or Name"></asp:TextBox>
                                                                                    </td>
                                                                                    <td align="left" valign="top" id="fboViewSearch_ButtonOnOff" runat="server">
                                                                                        <asp:Button ID="fboViewSearch_Button" Text="Show Airport" runat="server" Style="margin-top: -2px;"
                                                                                            OnClick="fboViewSearch_Button_Click" />
                                                                                    </td>
                                                                                    <td align="left" valign="top">
                                                                                        <asp:TextBox runat="server" ID="fboViewSearch_ICAOCODE" Visible="false"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="display_none" id="airportFolderToggleOnOff" runat="server">
                                                                                    <td align="left" valign="top" width="160px">
                                                                                        <asp:DropDownList runat="server" ID="searchAirportFolder" Width="166px" onchange="keepAirportSame(this);">
                                                                                            <asp:ListItem Value="">Please choose one:</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td align="left" valign="top" style="padding-left: 4px;">
                                                                                        <asp:CheckBox runat="server" ID="excludeAircraftFolders" Text="Exclude" onchange="updateExcludeAirport(this);" />
                                                                                        <asp:CheckBox runat="server" ID="defaultFolderSelectedCheckBox" CssClass="display_none" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                                <script type="text/javascript">
                                                                    function keepOperatorsSame() {
                                                                        sameVal = $('#<%= airportOperatorFolder.clientID %>').val();
                                                                        if (sameVal != '') {
                                                                            $('#<%= operator_drop2.clientID %>').val(sameVal);
                                                                            $('#<%= utilizationViewDropDown.clientID %>').val(sameVal);
                                                                        } else { $('#<%= operator_drop2.clientID %>').val('U'); $('#<%= utilizationViewDropDown.clientID %>').val('U'); }
                                                                        $('#<%= utilizationExcludeCheck.clientID %>').prop('checked', $('#<%= airportOperatorExclude.clientID %>').is(":checked"))
                                                                        $('#<%= exclude_check.clientID %>').prop('checked', $('#<%= airportOperatorExclude.clientID %>').is(":checked"))
                                                                    }

                                                                    function keepAirportSame(drop) {
                                                                        sameVal = $(drop).val();
                                                                        if (sameVal != '') {
                                                                            $('#<%= searchAirportFolder.clientID %>').val('');
                                                                            $('#<%= airportDrop.clientID %>').val("");
                                                                            $('#<%= selectAirportsType.clientID %>').val("4");
                                                                            $('#<%=airportFolderToggleOnOff.clientID %>').removeClass("display_none");
                                                                            $('#<%= airportSearchToggleOnOff.clientID %>').addClass("display_none");
                                                                            $('#<%= airportDrop.clientID %>').val(sameVal);
                                                                            $('#<%= searchAirportFolder.clientID %>').val(sameVal);
                                                                        } else {
                                                                            $('#<%= airportDrop.clientID %>').val(""); $('#<%= searchAirportFolder.clientID %>').val(""); $('#<%=airportFolderToggleOnOff.clientID %>').addClass("display_none");
                                                                            $('#<%= airportSearchToggleOnOff.clientID %>').addClass("display_none"); $('#<%= selectAirportsType.clientID %>').val("1");
                                                                        }
                                                                    }
                                                                    function updateExclude(checked) {
                                                                        var answer = $(checked).find('input').is(":checked");
                                                                        $('#<%= exclude_check.clientID %>').prop('checked', answer);
                                                                        $('#<%= airportOperatorExclude.clientID %>').prop('checked', answer);
                                                                        $('#<%= utilizationExcludeCheck.clientID %>').prop('checked', answer);
                                                                    }

                                                                    function updateExcludeAirport(checked) {
                                                                        var answer = $(checked).find('input').is(":checked");
                                                                        $('#<%= airportExcludeCheck.clientID %>').prop('checked', answer);
                                                                        $('#<%= excludeAircraftFolders.clientID %>').prop('checked', answer);
                                                                    }
                                                                    function setUpAirportPanel() {

                                                                        var compareVal;
                                                                        compareVal = $('#<%= selectAirportsType.clientID %>').val();

                                                                        if (compareVal == 3) {
                                                                            $('#<%= airportSearchToggleOnOff.clientID %>').removeClass("display_none");
                                                                            $('#<%= searchAirportFolder.clientID %>').val('');
                                                                            $('#<%= airportDrop.clientID %>').val("");
                                                                            $('#<%=airportFolderToggleOnOff.clientID %>').addClass("display_none");
                                                                            $('#<%= avis_Check.clientID %>').prop('checked', 1);
                                                                            $('#<%=airportIataToggle.clientID %>').addClass("display_none");
                                                                        } else if (compareVal == 4) {
                                                                            $('#<%= searchAirportFolder.clientID %>').val('');
                                                                            $('#<%= airportDrop.clientID %>').val("");
                                                                            $('#<%=airportFolderToggleOnOff.clientID %>').removeClass("display_none");
                                                                            $('#<%= airportSearchToggleOnOff.clientID %>').addClass("display_none");
                                                                            $('#<%=airportIataToggle.clientID %>').addClass("display_none");
                                                                        } else if (compareVal == 0) {
                                                                            window.open("/FolderMaintenance.aspx?t=17&newStaticFolder=true", "", "width=1250,height=390,resizable,");
                                                                            $('#<%=airportFolderToggleOnOff.clientID %>').addClass("display_none");
                                                                            $('#<%= airportSearchToggleOnOff.clientID %>').addClass("display_none");
                                                                            $('#<%=airportIataToggle.clientID %>').addClass("display_none");
                                                                        } else if (compareVal == 1) {
                                                                            $("#<%= selectAirportID.clientID %>").val('0');   // added in MSW - 4/23/18
                                                                            $('#<%= fboViewSearch_textbox.clientID %>').val('');  // added in MSW - 4/23/18
                                                                            $('#<%= airportDrop.clientID %>').val("");
                                                                            $('#<%= searchAirportFolder.clientID %>').val('');
                                                                            $('#<%=airportFolderToggleOnOff.clientID %>').addClass("display_none");
                                                                            $('#<%= airportSearchToggleOnOff.clientID %>').addClass("display_none");
                                                                            $('#<%=airportIataToggle.clientID %>').addClass("display_none");
                                                                        } else if (compareVal == 5) {
                                                                            $("#<%= selectAirportID.clientID %>").val('0');   // added in MSW - 4/23/18
                                                                            $('#<%= fboViewSearch_textbox.clientID %>').val('');  // added in MSW - 4/23/18
                                                                            $('#<%= airportDrop.clientID %>').val("");
                                                                            $('#<%= searchAirportFolder.clientID %>').val('');
                                                                            $('#<%=airportFolderToggleOnOff.clientID %>').addClass("display_none");
                                                                            $('#<%= airportSearchToggleOnOff.clientID %>').addClass("display_none");
                                                                            $('#<%=airportIataToggle.clientID %>').removeClass("display_none");
                                                                        } else {
                                                                            $('#<%= airportDrop.clientID %>').val("");
                                                                            $('#<%= searchAirportFolder.clientID %>').val('');
                                                                            $('#<%=airportFolderToggleOnOff.clientID %>').addClass("display_none");
                                                                            $('#<%= airportSearchToggleOnOff.clientID %>').addClass("display_none");
                                                                            $('#<%=airportIataToggle.clientID %>').addClass("display_none");
                                                                        }

                                                                    }
                                                                    function setUpOperatorPanel() {
                                                                        var compareVal;
                                                                        $("#<%= viewOperatorRoll.clientID  %>").prop("checked", false);
                                                                        compareVal = $('#<%= airportOperatorType.clientID %>').val();
                                                                        $('#<%= airportOperatorFolder.clientID %>').val('');
                                                                        $('#<%= operator_drop2.clientID %>').val('U');
                                                                        $('#<%= utilizationViewDropDown.clientID %>').val('U');
                                                                        if (compareVal == 4) {
                                                                            $('#<%= airportOperatorFolderToggleOnOff.clientID %>').removeClass("display_none");
                                                                        } else if (compareVal == 0) {
                                                                            document.location.href = "/Company_Listing.aspx";
                                                                            //  window.open("/FolderMaintenance.aspx?t=1&newStaticFolder=true&opChecked=true", "", "width=1250,height=390,resizable,");
                                                                            $('#<%= airportOperatorFolderToggleOnOff.clientID %>').addClass("display_none");
                                                                        } else if (compareVal == 1) {
                                                                            $("#<%= viewOperatorHiddenCompanyID.clientID %>").val('0')
                                                                            $('#<%= airportOperatorFolderToggleOnOff.clientID %>').addClass("display_none");
                                                                        } else {
                                                                            $('#<%= airportOperatorFolderToggleOnOff.clientID %>').addClass("display_none");
                                                                        }

                                                                    }
                                  //                                  $(document).ready(function() {
                                  //                                    $("#filter").keyup(function() {

                                  //                                      // Retrieve the input field text and reset the count to zero
                                  //                                      var filter = $(this).val(), count = 0;
                                  //                                      var css = '';
                                  //                                      if (filter !== '') {
                                  //                                        if (filter.length >= 3) {
                                  //                                          // Loop through the comment list
                                  //                                          $(".commentlist li").each(function() {

                                  //                                            iataAnswer = $(this).find("iata");
                                  //                                            icaoAnswer = $(this).find("icao");
                                  //                                            cityAnswer = $(this).find("city");
                                  //                                            nameAnswer = $(this).find("strong");
                                  //                                            nameLink = $(this).find("a");

                                  //                                            if (iataAnswer.text().search(new RegExp(filter, "i")) < 0) {
                                  //                                              DoesIATAMatch = false;
                                  //                                              iataAnswer.removeClass("error_text");
                                  //                                            } else {
                                  //                                              DoesIATAMatch = true;
                                  //                                              iataAnswer.addClass("error_text");
                                  //                                            }
                                  //                                            if (icaoAnswer.text().search(new RegExp(filter, "i")) < 0) {
                                  //                                              DoesICAOMatch = false;
                                  //                                              icaoAnswer.removeClass("error_text");
                                  //                                            } else {
                                  //                                              DoesICAOMatch = true;
                                  //                                              icaoAnswer.addClass("error_text");
                                  //                                            }

                                  //                                            if (cityAnswer.text().search(new RegExp(filter, "i")) < 0) {
                                  //                                              DoesCityMatch = false;
                                  //                                              cityAnswer.removeClass("error_text");
                                  //                                            } else {
                                  //                                              DoesCityMatch = true;
                                  //                                              cityAnswer.addClass("error_text");
                                  //                                            }

                                  //                                            if (nameAnswer.text().search(new RegExp(filter, "i")) < 0) {
                                  //                                              DoesNameMatch = false;
                                  //                                              nameAnswer.removeClass("error_text");
                                  //                                              nameLink.removeClass("error_text");
                                  //                                            } else {
                                  //                                              DoesNameMatch = true;
                                  //                                              nameAnswer.addClass("error_text");
                                  //                                              nameLink.addClass("error_text");
                                  //                                            }

                                  //                                            // If the list item does not contain the text phrase fade it out
                                  //                                            if ((DoesIATAMatch == false) && (DoesICAOMatch == false) && (DoesCityMatch == false) && (DoesNameMatch == false)) {
                                  //                                              $(this).fadeOut();

                                  //                                              // Show the list item if the phrase matches and increase the count by 1
                                  //                                            } else {
                                  //                                              $(this).show();
                                  //                                              if (css !== '') {
                                  //                                                $(this).addClass("alt_row");
                                  //                                                css = '';
                                  //                                              } else {
                                  //                                                css = 'alt_row';
                                  //                                                $(this).removeClass("alt_row");
                                  //                                              }
                                  //                                              count++;
                                  //                                            }
                                  //                                          });
                                  //                                          $("#filter-count").text(count + " Results");
                                  //                                          $("#airportFBOHeader").show();
                                  //                                        }
                                  //                                      }
                                  //                                    });

                                  //                                  });
                                  //                                
                                                                </script>
                                                                <script type="text/javascript">

                                                                    function setUpAircraftPanel() {
                                                                        var compareVal;
                                                                        compareVal = $('#<%= airportAircraftType.clientID %>').val();
                                                                        $('#<%= airportAircraftFolder.clientID %>').val('');
                                                                        if (compareVal == 4) {
                                                                            $('#<%= airportAircraftFolderToggleOnOff.clientID %>').removeClass("display_none");
                                                                            $('#<%= airportAircraftRegNumberToggleOnOff.clientID %>').addClass("display_none");
                                                                        } else if (compareVal == 5) {
                                                                            $('#<%= airportAircraftFolderToggleOnOff.clientID %>').addClass("display_none");
                                                                            $('#<%= airportAircraftRegNumberToggleOnOff.clientID %>').removeClass("display_none");
                                                                        } else if (compareVal == 0) {
                                                                            document.location.href = "/Aircraft_Listing.aspx?flight=true";
                                                                            $('#<%= airportAircraftFolderToggleOnOff.clientID %>').addClass("display_none");
                                                                            $('#<%= airportAircraftRegNumberToggleOnOff.clientID %>').addClass("display_none");
                                                                        } else {
                                                                            $('#<%= airportAircraftFolderToggleOnOff.clientID %>').addClass("display_none");
                                                                            $('#<%= airportAircraftRegNumberToggleOnOff.clientID %>').addClass("display_none");
                                                                        }

                                                                    }

                                                                </script>
                                                                <div id="airportFBOHeader" style="display: none;">
                                                                    Airport Search Results: Click on the airport name to select. <span id="filter-count"
                                                                        class="float_right emphasis_text"></span>
                                                                </div>
                                                                <asp:TextBox runat="server" ID="selectAirportID" Text="0" CssClass="display_none"></asp:TextBox><asp:TextBox
                                                                    runat="server" CssClass="display_none" ID="selectOperatorID" Text="0"></asp:TextBox>
                                                                <asp:Label runat="server" ID="fboViewSearchResultsJS"></asp:Label><asp:Panel runat="server"
                                                                    ID="fboViewSearch_Results">
                                                                    <asp:Label runat="server" ID="fboViewSearch_ResultsLabel"></asp:Label>
                                                                </asp:Panel>
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow runat="server" Visible="false" ID="aircraftFolderRow">
                                                <asp:TableCell ID="aircraftFolderCell">
                                                    <table width="100%" cellpadding="5" cellspacing="0" class="data_aircraft_grid override_borders">
                                                        <tr class="header_row">
                                                            <td colspan="5" valign="top" align="left" style="width: 210px;">
                                                                <b>AIRCRAFT</b>&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList runat="server" ID="airportAircraftType" onchange="setUpAircraftPanel();"
                                    Width="144px">
                                    <asp:ListItem Value="1">All Aircraft</asp:ListItem>
                                    <asp:ListItem Value="4">Select Aircraft Folder</asp:ListItem>
                                    <asp:ListItem Value="5">Reg Number(s)</asp:ListItem>
                                    <asp:ListItem Value="0">Create Aircraft Folder</asp:ListItem>
                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left" class="display_none" valign="top" id="airportAircraftFolderToggleOnOff"
                                                                runat="server">
                                                                <asp:DropDownList runat="server" ID="airportAircraftFolder" Width="166px" onchange="">
                                                                    <asp:ListItem Value="">Please choose one:</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:CheckBox runat="server" ID="airportAircraftExclude" Text="Exclude" onchange="" />
                                                            </td>
                                                            <td align="left" class="display_none" valign="top" id="airportAircraftRegNumberToggleOnOff"
                                                                runat="server">
                                                                <asp:TextBox runat="server" ID="aircraftRegNumberView" TextMode="MultiLine" Rows="2"
                                                                    Width="228px" placeholder="Please list Reg Number(s)"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell ID="airportOperatorPanel" Visible="false" runat="server" VerticalAlign="Top">
                                                    <asp:Panel ID="Panel6" runat="server">
                                                        <asp:CheckBox runat="server" ID="exportUtilization" Checked="false" CssClass="display_none"
                                                            Text="Export Flights" />
                                                        <table width="100%" cellpadding="5" cellspacing="0" class="data_aircraft_grid override_borders">
                                                            <tr class="header_row">
                                                                <td colspan="5" valign="top" align="left">
                                                                    <b>OPERATORS</b>
                                                                    <asp:DropDownList runat="server" ID="airportOperatorType" onchange="setUpOperatorPanel();"
                                                                        Width="144px">
                                                                        <asp:ListItem Value="1">All Operators</asp:ListItem>
                                                                        <asp:ListItem Value="4">Select Operator Folder</asp:ListItem>
                                                                        <asp:ListItem Value="0">Create Operator Folder</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:TextBox runat="server" ID="viewOperatorHiddenCompanyID" CssClass="display_none"></asp:TextBox>
                                                                    <asp:CheckBox runat="server" ID="viewOperatorRoll" CssClass="display_none" />
                                                                </td>
                                                                <td align="left" class="display_none" valign="top" id="airportOperatorFolderToggleOnOff"
                                                                    runat="server">
                                                                    <asp:DropDownList runat="server" ID="airportOperatorFolder" Width="166px" onchange="keepOperatorsSame();">
                                                                        <asp:ListItem Value="">Please choose one:</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:CheckBox runat="server" ID="airportOperatorExclude" Text="Exclude" onchange="updateExclude(this);keepOperatorsSame();" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </asp:TableCell><asp:TableCell HorizontalAlign="right" VerticalAlign="top" ColumnSpan="3" ID="cellGoApply">
                                                    <asp:Button runat="server" ID="viewApplyID" Text="Search" ToolTip="Click to Apply Critera"
                                                        UseSubmitBehavior="false" OnClientClick="SetLoading('DivLoadingMessage')" />
                                                    <asp:Button runat="server" ID="ViewClearID" Text="Clear Selections" UseSubmitBehavior="false"
                                                        ToolTip="Click to Clear Critera" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:Panel>
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="center" style="width: 100%;">
                            <asp:Label runat="server" ID="crmProspectSearchLabel"></asp:Label><asp:Panel ID="tabOuterPanel" runat="server" HorizontalAlign="Left" Width="100%">
                                <table cellpadding="4" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="left" valign="top" width="20%" runat="server" id="resizeTopLeftTab" class="modelImageBox mobileModelTable">
                                            <asp:UpdatePanel ID="UpdatePanel_top_left" runat="server" ChildrenAsTriggers="True"
                                                UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <cc1:TabContainer ID="TabContainer2" runat="server" CssClass="dark-theme" Visible="true">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <cc1:TabPanel ID="TabPanel14" runat="server">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="make_model_name_label" Text="PLACER TEXT" runat="server"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ContentTemplate>
                                                                <asp:Panel ID="topLeftCSSContainer" runat="server" CssClass="tab_container_div_small">
                                                                    <table align='center' cellpadding="0" cellspacing="0" width="100%" valign='top'>
                                                                        <tr valign='top'>
                                                                            <td align='center' valign="top">
                                                                                <asp:Image ID="aircraft_image" runat="server" ImageUrl="~/images/spacer.gif" BorderColor="#404040"
                                                                                    BorderWidth="1px" BackColor="#1F6C9A" Width="285px" Style="max-height: 225px;" />


                                                                                <asp:Label ID="div_start_label" runat="server" Visible="False"></asp:Label><asp:Label
                                                                                    ID="estimated_value_label" runat="server" Visible="False"></asp:Label><asp:Label
                                                                                        ID="estimated_value_label2" runat="server" Visible="False"></asp:Label><asp:Label
                                                                                            ID="label_behind_pic" runat="server"></asp:Label><asp:Label ID="permenant_amod_id"
                                                                                                runat="server" Visible="False"></asp:Label><asp:Label ID="aircraft_picture_slideshow"
                                                                                                    runat="server"></asp:Label><asp:Label ID="div_end_label" runat="server" Visible="False"></asp:Label><input
                                                                                                        type="button" runat="server" id="update_compare2" visible="False" /><div id="airport_map_div" runat="server">
                                                                                                            <div id="airport_map_label" style="width: 100%; height: 240px;">
                                                                                                            </div>
                                                                                                        </div>


                                                                                <asp:Image ID="Image1" ImageAlign="Top" runat="server" ImageUrl="~/images/spacer.gif"
                                                                                    BorderWidth="0px" Width="285px" Height="1px" />


                                                                                <br />
                                                                                <asp:Label runat="server" ID="clear_airport_id" Visible="False"></asp:Label></div></td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </cc1:TabContainer><asp:Image runat="server" ID="imageSpacer" ImageUrl="/images/spacer.gif" Width="300"
                                                        Visible="false" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td id="middle_view_column" runat="server" width='300' visible="false" align="left"
                                            valign="top">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <cc1:TabContainer ID="TabContainer3" runat="server" CssClass="dark-theme" Visible="true">
                                                        <cc1:TabPanel runat="server" ID="middle_col_tabpanel">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="middle_column_company_name" Text="PLACER TEXT" runat="server"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ContentTemplate>
                                                                <asp:Label runat="server" ID="company_details_middle_column"></asp:Label>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                    </cc1:TabContainer>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="left" valign="top" class="tabContainerRightBox mobileModelTable">
                                            <asp:LinkButton ID="crm_export_button" CssClass="float_right" runat="server" Visible="false">Export All</asp:LinkButton><asp:LinkButton ID="crm_export_email_button" CssClass="float_right padding_right"
                                                runat="server" Visible="false">Export Email Addresses</asp:LinkButton><asp:UpdatePanel ID="top_tab_update_panel" runat="server" ChildrenAsTriggers="True"
                                                    UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel runat="server" ID="utilizationBoxes" Visible="false" CssClass="valueSpec aircraftListing Simplistic aircraftSpec gray_background viewBoxes">
                                                            <div class="row">
                                                                <asp:Label runat="server" ID="utilizationBoxHeader"></asp:Label>
                                                            </div>
                                                        </asp:Panel>
                                                        <asp:Panel ID="crm_info" Visible="false" runat="server" Font-Size="10px" CssClass="valueSpec aircraftListing aircraftContainer Simplistic aircraftSpec gray_background viewBoxes"
                                                            Width="99%">

                                                            <h1 runat="server" id="crmProspectHeader" class="mainHeading padded_left"></h1>
                                                            <asp:Panel runat="server" ID="crmProspectPictureContainer" CssClass="margin-top margin-bottom Box float_left" Width="20.5%">
                                                                <asp:Label runat="server" ID="crmProspect_Picture"></asp:Label>
                                                            </asp:Panel>
                                                            <asp:Panel runat="server" ID="crmProspectWhiteBoxContainer" CssClass="margin-top margin-bottom Box float_left" Width="50.5%">
                                                                <asp:Label runat="server" ID="crm_prospect_label_text"></asp:Label><asp:Label runat="server" ID="crm_prospect_label_text_count" Font-Bold="true"></asp:Label><asp:LinkButton ID="crm_prospect_link_view_all" runat="server" CssClass="tiny_text emphasis_text">(view all)</asp:LinkButton><br />
                                                                <br clear="all" />
                                                                <asp:Panel runat="server" ID="crm_fractional_owners_search">
                                                                    Display Expiring Fractional Owners by
                                                                    <asp:DropDownList ID="crm_fractional_owners_tab_model" runat="server">
                                                                    </asp:DropDownList>
                                                                    expiring in the next
                                                                    <asp:DropDownList ID="crm_fractional_owners_tab_timeframe" runat="server">
                                                                        <asp:ListItem Value="1">1 Year</asp:ListItem>
                                                                        <asp:ListItem Value="2">2 Year</asp:ListItem>
                                                                        <asp:ListItem Value="3">3 Year</asp:ListItem>
                                                                        <asp:ListItem Value="4">4 Year</asp:ListItem>
                                                                        <asp:ListItem Value="5">5 Year</asp:ListItem>
                                                                        <asp:ListItem Value="6">6 Year</asp:ListItem>
                                                                        <asp:ListItem Value="7">7 Year</asp:ListItem>
                                                                        <asp:ListItem Value="8">8 Year</asp:ListItem>
                                                                        <asp:ListItem Value="9">9 Year</asp:ListItem>
                                                                        <asp:ListItem Value="10">10 Year</asp:ListItem>
                                                                    </asp:DropDownList><!--ordered by--><asp:DropDownList ID="crm_fractional_owners_tab_sort" runat="server" CssClass="display_none">
                                                                        <asp:ListItem Value="company">Company Name</asp:ListItem>
                                                                        <asp:ListItem Value="expiration">Fractional Expires Date</asp:ListItem>
                                                                    </asp:DropDownList><asp:DropDownList ID="crm_fractional_owners_tab_do_not_include" runat="server">
                                                                        <asp:ListItem>Do Not Include</asp:ListItem>
                                                                        <asp:ListItem>Include</asp:ListItem>
                                                                    </asp:DropDownList>fractional agreements already expired.
                                                                    <asp:Button ID="Button6" runat="server" Text="Search" Height="25" />
                                                                </asp:Panel>
                                                                <asp:Panel ID="crm_expiring_leases_search" runat="server" Visible="false">
                                                                    Display Expiring Leases by
                                                                    <asp:DropDownList ID="crm_expiring_leases_model" runat="server">
                                                                    </asp:DropDownList>
                                                                    expiring in the next
                                                                    <asp:DropDownList ID="crm_expiring_leases_timeframe" runat="server">
                                                                        <asp:ListItem Value="1">1 Year</asp:ListItem>
                                                                        <asp:ListItem Value="2">2 Year</asp:ListItem>
                                                                        <asp:ListItem Value="3">3 Year</asp:ListItem>
                                                                        <asp:ListItem Value="4">4 Year</asp:ListItem>
                                                                        <asp:ListItem Value="5">5 Year</asp:ListItem>
                                                                        <asp:ListItem Value="6">6 Year</asp:ListItem>
                                                                        <asp:ListItem Value="7">7 Year</asp:ListItem>
                                                                        <asp:ListItem Value="8">8 Year</asp:ListItem>
                                                                        <asp:ListItem Value="9">9 Year</asp:ListItem>
                                                                        <asp:ListItem Value="10">10 Year</asp:ListItem>
                                                                    </asp:DropDownList><!--ordered by--><asp:DropDownList ID="crm_expiring_leases_sort" runat="server" CssClass="display_none">
                                                                        <asp:ListItem Value="company">Company Name</asp:ListItem>
                                                                        <asp:ListItem Value="expiration">Lease Expiration</asp:ListItem>
                                                                    </asp:DropDownList><asp:DropDownList ID="crm_expiring_leases_do_not_include" runat="server">
                                                                        <asp:ListItem>Do Not Include</asp:ListItem>
                                                                        <asp:ListItem>Include</asp:ListItem>
                                                                    </asp:DropDownList>leases already expired.
                                                                    <asp:Button ID="Button5" runat="server" Text="Search" Height="25" />
                                                                </asp:Panel>
                                                                <asp:Panel ID="crm_previous_owners_tab_search" runat="server" Visible="false">
                                                                    Display Previous Owners By
                                                                    <asp:DropDownList ID="crm_previous_owners_tab_model" runat="server">
                                                                    </asp:DropDownList>
                                                                    sold in the last
                                                                    <asp:DropDownList ID="crm_previous_owners_tab_sold_within" runat="server">
                                                                        <asp:ListItem Value="1">1 Year</asp:ListItem>
                                                                        <asp:ListItem Value="2">2 Year</asp:ListItem>
                                                                        <asp:ListItem Value="3">3 Year</asp:ListItem>
                                                                        <asp:ListItem Value="4">4 Year</asp:ListItem>
                                                                        <asp:ListItem Value="5">5 Year</asp:ListItem>
                                                                        <asp:ListItem Value="6">6 Year</asp:ListItem>
                                                                        <asp:ListItem Value="7">7 Year</asp:ListItem>
                                                                        <asp:ListItem Value="8">8 Year</asp:ListItem>
                                                                        <asp:ListItem Value="9">9 Year</asp:ListItem>
                                                                        <asp:ListItem Value="10">10 Year</asp:ListItem>
                                                                    </asp:DropDownList><!--ordered by--><asp:DropDownList ID="crm_previous_owners_tab_sort" runat="server" CssClass="display_none">
                                                                        <asp:ListItem Value="company">Company Name</asp:ListItem>
                                                                        <asp:ListItem Value="date">Date Sold</asp:ListItem>
                                                                    </asp:DropDownList><asp:Button ID="Button4" runat="server" Text="Search" Height="25" />
                                                                </asp:Panel>
                                                                <asp:Panel runat="server" ID="crm_time_to_buy_tab_search" Visible="false">
                                                                    <asp:Label ID="Label1" runat="server" Text="Display"></asp:Label><asp:DropDownList
                                                                        ID="crm_time_to_buy_owners_of" runat="server">
                                                                        <asp:ListItem Value="ALL" Selected="true">All Owners</asp:ListItem>
                                                                        <asp:ListItem Value="PREVIOUS">Owners of Pre Owned Aircraft</asp:ListItem>
                                                                        <asp:ListItem Value="NEW">Owners of New Aircraft</asp:ListItem>
                                                                    </asp:DropDownList><asp:Label ID="Label2" runat="server" Text="of"></asp:Label><asp:DropDownList ID="crm_time_to_buy_model_type"
                                                                        runat="server">
                                                                    </asp:DropDownList>
                                                                    <asp:Label ID="Label3" runat="server" Text="who have aircraft"></asp:Label><asp:DropDownList
                                                                        ID="crm_time_to_buy_aircraft_status" runat="server">
                                                                        <asp:ListItem Value="NOT FOR SALE">Not For Sale</asp:ListItem>
                                                                        <asp:ListItem Value="FOR SALE">For Sale</asp:ListItem>
                                                                        <asp:ListItem Value="BOTH" Selected="True">With Any Status</asp:ListItem>
                                                                    </asp:DropDownList><asp:Label ID="Label4" runat="server" Text="and have reached over"></asp:Label><asp:DropDownList
                                                                        ID="crm_time_to_buy_lifecycle" runat="server">
                                                                        <asp:ListItem Value="150">150</asp:ListItem>
                                                                        <asp:ListItem Value="125">125</asp:ListItem>
                                                                        <asp:ListItem Value="100">100</asp:ListItem>
                                                                        <asp:ListItem Value="90" Selected="True">90</asp:ListItem>
                                                                        <asp:ListItem Value="85">85</asp:ListItem>
                                                                        <asp:ListItem Value="80">80</asp:ListItem>
                                                                        <asp:ListItem Value="75">75</asp:ListItem>
                                                                    </asp:DropDownList>%
                                                                    <asp:Label ID="Label5" runat="server" Text="of their typical length of ownership for their aircraft"></asp:Label><asp:DropDownList
                                                                        ID="crm_time_to_buy_sort" runat="server" CssClass="display_none">
                                                                        <asp:ListItem Value="COMPANY">Company Name</asp:ListItem>
                                                                        <asp:ListItem Value="OWNERSHIP" Selected="True">Length of Ownership</asp:ListItem>
                                                                    </asp:DropDownList><asp:Button ID="Button2" runat="server" Visible="true" Text="Search" Height="25" />
                                                                </asp:Panel>
                                                                <asp:Panel ID="crm_wanted_tab_search" runat="server" Visible="false">
                                                                    Display Wanteds by
                                                                    <asp:DropDownList ID="crm_wanted_tab_models" runat="server">
                                                                    </asp:DropDownList>
                                                                    listed in the last
                                                                    <asp:DropDownList ID="crm_wanted_tab_timeframe" runat="server">
                                                                        <asp:ListItem Value="1">1 Year</asp:ListItem>
                                                                        <asp:ListItem Value="2">2 Year</asp:ListItem>
                                                                        <asp:ListItem Value="3">3 Year</asp:ListItem>
                                                                        <asp:ListItem Value="4">4 Year</asp:ListItem>
                                                                        <asp:ListItem Value="5">5 Year</asp:ListItem>
                                                                        <asp:ListItem Value="6">6 Year</asp:ListItem>
                                                                        <asp:ListItem Value="7">7 Year</asp:ListItem>
                                                                        <asp:ListItem Value="8">8 Year</asp:ListItem>
                                                                        <asp:ListItem Value="9">9 Year</asp:ListItem>
                                                                        <asp:ListItem Value="10">10 Year</asp:ListItem>
                                                                    </asp:DropDownList><!--ordered by--><asp:DropDownList ID="crm_wanted_tab_sort" runat="server" CssClass="display_none">
                                                                        <asp:ListItem Value="company">Company Name</asp:ListItem>
                                                                        <asp:ListItem Value="date">List Date</asp:ListItem>
                                                                    </asp:DropDownList><asp:DropDownList ID="crm_wanted_tab_do_not_include" runat="server">
                                                                        <asp:ListItem>Include</asp:ListItem>
                                                                        <asp:ListItem>Do Not Include</asp:ListItem>
                                                                    </asp:DropDownList>Broker Wanteds.
                                                                    <asp:Button ID="Button7" runat="server" Text="Search" Height="25" />
                                                                </asp:Panel>
                                                                <asp:Panel ID="crm_recent_sales_search" runat="server" Visible="false">
                                                                    Display Recent Sales by
                                                                    <asp:DropDownList ID="crm_recent_sales_models" runat="server">
                                                                    </asp:DropDownList>
                                                                    sold in the last
                                                                    <asp:DropDownList ID="crm_recent_sales_timeframe" runat="server">
                                                                        <asp:ListItem Value="1">1 Year</asp:ListItem>
                                                                        <asp:ListItem Value="2">2 Year</asp:ListItem>
                                                                        <asp:ListItem Value="3">3 Year</asp:ListItem>
                                                                        <asp:ListItem Value="4">4 Year</asp:ListItem>
                                                                        <asp:ListItem Value="5">5 Year</asp:ListItem>
                                                                        <asp:ListItem Value="6">6 Year</asp:ListItem>
                                                                        <asp:ListItem Value="7">7 Year</asp:ListItem>
                                                                        <asp:ListItem Value="8">8 Year</asp:ListItem>
                                                                        <asp:ListItem Value="9">9 Year</asp:ListItem>
                                                                        <asp:ListItem Value="10">10 Year</asp:ListItem>
                                                                    </asp:DropDownList><!--ordered by--><asp:DropDownList ID="crm_recent_sales_sort" runat="server" CssClass="display_none">
                                                                        <asp:ListItem Value="company">Company Name</asp:ListItem>
                                                                        <asp:ListItem Value="sold">Date Sold</asp:ListItem>
                                                                    </asp:DropDownList><asp:Button ID="Button8" runat="server" Text="Search" Height="25" />
                                                                </asp:Panel>
                                                                <asp:Panel ID="crm_prospector_search" runat="server" Visible="false">
                                                                    Display Prospects for
                                                                    <asp:DropDownList ID="crm_prospector_tab_models" runat="server">
                                                                    </asp:DropDownList>
                                                                    <asp:CheckBox ID="crm_prospector_tab_checkbox" runat="server" Text="Include Inactive" />
                                                                    <asp:Button ID="Button9" runat="server" Text="Search" Height="25" />
                                                                </asp:Panel>
                                                                <asp:Panel ID="crm_notes_search" Visible="false" runat="server">
                                                                    Display
                                                                    <asp:DropDownList ID="crm_notes_tab_filter" runat="server">
                                                                        <asp:ListItem Value="All">All Notes</asp:ListItem>
                                                                        <asp:ListItem Value="Companies">Notes for Companies</asp:ListItem>
                                                                    </asp:DropDownList>for this Aircraft ordered by
                                                                    <asp:DropDownList ID="crm_notes_tab_sort" runat="server">
                                                                        <asp:ListItem Value="Date">Date Descending</asp:ListItem>
                                                                        <asp:ListItem Value="Company">Company Name</asp:ListItem>
                                                                    </asp:DropDownList><asp:Button ID="Button3" runat="server" Text="Search" Height="25" />
                                                                </asp:Panel>



                                                            </asp:Panel>
                                                            <asp:Panel ID="prospects_panel" runat="server" Visible="false" CssClass="margin-top margin-bottom Box float_left" Width="22.5%">
                                                                <asp:Label ID="upgrade_text" runat="server" Text=""></asp:Label><asp:Label runat="server" ID="upgrade_models_text2" Text=""></asp:Label>
                                                            </asp:Panel>
                                                            <br />
                                                            <br clear="all" />
                                                        </asp:Panel>
                                                        <asp:Button runat="server" ID="invisibleAirportFBOButtonToCausePostback" CssClass="display_none" />
                                                        <cc1:TabContainer ID="tabs_container" runat="server" Width="100%" ActiveTabIndex="0"
                                                            BorderStyle="None" Style="margin-left: auto; margin-right: auto;" CssClass="dark-theme">
                                                            <cc1:TabPanel ID="market_values_top_tab" runat="server" Visible="false">
                                                                <ContentTemplate>
                                                                    <div class="tab_container_div_small" runat="server" id="marketValuesTopTabContainer">
                                                                        <div id="valChart">
                                                                        </div>
                                                                        <asp:Label runat="server" ID="currentMarketEValuesGraph" Width="960" Style="margin-top: -6px; margin-left: -5px; text-align: center;"></asp:Label>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="model_market_status_tab" runat="server">
                                                                <ContentTemplate>
                                                                    <div class="tab_container_div_small">
                                                                        <table cellpadding='0' cellspacing='0'>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <asp:Label ID="overview_label" runat="server" Text="<b>FAA Flight Arrivals By </b>"
                                                                                        Visible="False"></asp:Label><asp:DropDownList ID="airport_overview_type" runat="server"
                                                                                            Visible="False" AutoPostBack="True">
                                                                                            <asp:ListItem Value="Month">Month</asp:ListItem>
                                                                                            <asp:ListItem Value="Type">Aircraft Type</asp:ListItem>
                                                                                            <asp:ListItem Value="BWeight">Business Jets by Weight Class</asp:ListItem>
                                                                                            <asp:ListItem Value="TWeight">TurboProps by Weight Class</asp:ListItem>
                                                                                            <asp:ListItem Value="HWeight">Helicopters by Weight Class</asp:ListItem>
                                                                                        </asp:DropDownList><asp:Label runat="server" ID="right_side_overview" Visible="False"></asp:Label><asp:Label
                                                                                            ID="last_overview_type" runat="server" Visible="False"></asp:Label></td>
                                                                            </tr>
                                                                            <tr valign='top'>
                                                                                <td valign='top'>
                                                                                    <asp:Literal ID="view_market_status_label" runat="server"></asp:Literal></td>
                                                                                <td valign='top' class="mobile_display_off_cell">
                                                                                    <div id="chart_div_tab1" style="border-top: 0">
                                                                                    </div>
                                                                                    <div id='png2' runat="server" clientidmode="Static" visible="false">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:Literal ID="model_market_summary_status_text" runat="server"></asp:Literal>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="model_fleet_tab" runat="server">
                                                                <ContentTemplate>
                                                                    <div class="tab_container_div_small">
                                                                        <asp:Literal ID="view_fleet_label" Text="" runat="server"></asp:Literal>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="model_performance_tab" runat="server">
                                                                <ContentTemplate>
                                                                    <div class="tab_container_div_small">
                                                                        <asp:Literal ID="view_performance_label" Text="" runat="server"></asp:Literal>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="model_operating_costs_tab" runat="server">
                                                                <ContentTemplate>
                                                                    <div class="tab_container_div_small">
                                                                        <asp:CheckBox ID="ac_based" runat="server" Visible="false" Text="Only Show Aircraft Not Based at This Airport"
                                                                            AutoPostBack="true" Checked="false" />
                                                                        <asp:Literal ID="view_operating_costs_label" Text="" runat="server"></asp:Literal>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="model_description_tab" runat="server">
                                                                <ContentTemplate>
                                                                    <div class="tab_container_div_small">
                                                                        <asp:CheckBox ID="controlled" runat="server" Visible="false" Text="Controlled Airports"
                                                                            AutoPostBack="true" Checked="true" />
                                                                        <asp:Literal ID="view_description_label" Text="" runat="server"></asp:Literal>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="model_flight_tab" runat="server">
                                                                <ContentTemplate>
                                                                    <div class="tab_container_div_small">
                                                                        <asp:Panel ID="reg_search_panel" runat="server" Visible="false">
                                                                            Reg#:<asp:TextBox ID="reg_search" runat="server" Width="150"></asp:TextBox>&nbsp;&nbsp;<asp:CheckBox
                                                                                ID="exact_match_reg" runat="server" Text="Exact Match" />
                                                                            &nbsp;&nbsp;<asp:CheckBox ID="dont_search_prev_reg" runat="server" Checked="true"
                                                                                Text="Don't Search Prev Reg#" />
                                                                            &nbsp;&nbsp;<asp:Button ID="search_reg" runat="server" Text="Search" />
                                                                        </asp:Panel>
                                                                        <asp:Literal ID="view_flights_label" Text="" runat="server"></asp:Literal>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="model_reports_tab" runat="server" Visible="false">
                                                                <ContentTemplate>
                                                                    <div class="tab_container_div_small">
                                                                        <asp:Label ID="myairportsLabelRan" Text="" runat="server" CssClass="display_none"></asp:Label><asp:Label
                                                                            ID="my_airports_label" Text="" runat="server" Visible="false"></asp:Label><asp:DropDownList
                                                                                ID="months_choice" runat="server" Visible="false" AutoPostBack="true">
                                                                                <asp:ListItem Value="365">Last 365 Days</asp:ListItem>
                                                                                <asp:ListItem Value="YTD">Current Year to Date</asp:ListItem>
                                                                            </asp:DropDownList><asp:Label ID="view_reports_label" Text="" runat="server"></asp:Label><div id="chart_div_survey"
                                                                                style="border-top: 0" visible="false">
                                                                            </div>

                                                                        <asp:Label ID="count_label" runat="server" Text="" Font-Bold="true"></asp:Label><asp:LinkButton
                                                                            ID="crm_view_view_all" runat="server" CssClass="tiny_text emphasis_text">(view all)</asp:LinkButton>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel Visible="false" ID="settings_tab" runat="server" HeaderText="My Analysis">
                                                                <ContentTemplate>
                                                                    <table width='100%' cellpadding="1" cellspacing="0" border="0" class="data_view_grid">
                                                                        <tr class="header_row">
                                                                            <td>Historical Sales Data Use </td>
                                                                            <td>General Information </td>
                                                                        </tr>
                                                                        <tr valign="top">
                                                                            <td width='66%' align="left" valign="top">
                                                                                <div style="height: 230px; overflow: auto;">
                                                                                    <table width='99%' cellpadding="1" cellspacing="0">
                                                                                        <tr valign="top">
                                                                                            <td>
                                                                                                <asp:Label ID="checking_text" runat="server"></asp:Label><asp:Label runat="server"
                                                                                                    ID="settings_text_label" Text="" Visible="false"></asp:Label><b>Show Estimates:</b><asp:CheckBox
                                                                                                        ID="estimated_value" runat="server" Checked="true" />
                                                                                                <ul>
                                                                                                    <li>Use Sales within
                                                                                                        <asp:DropDownList ID="years_of" runat="server">
                                                                                                            <asp:ListItem Value="0">All</asp:ListItem>
                                                                                                            <asp:ListItem Value="1">1</asp:ListItem>
                                                                                                            <asp:ListItem Value="2">2</asp:ListItem>
                                                                                                            <asp:ListItem Value="3">3</asp:ListItem>
                                                                                                            <asp:ListItem Value="4">4</asp:ListItem>
                                                                                                            <asp:ListItem Value="5">5</asp:ListItem>
                                                                                                        </asp:DropDownList>Year(s) Manufactured of My Aircraft</li>
                                                                                                    <li>Use Sales within
                                                                                                        <asp:DropDownList ID="sales_within" runat="server">
                                                                                                            <asp:ListItem Value="0">All</asp:ListItem>
                                                                                                            <asp:ListItem Value="200">200</asp:ListItem>
                                                                                                            <asp:ListItem Value="400">400</asp:ListItem>
                                                                                                            <asp:ListItem Value="600">600</asp:ListItem>
                                                                                                            <asp:ListItem Value="800">800</asp:ListItem>
                                                                                                            <asp:ListItem Value="1000">1,000</asp:ListItem>
                                                                                                        </asp:DropDownList>Hours AFTT of My Aircraft</li>
                                                                                                    <li>Used Sales of Used Aircraft Only
                                                                                                        <asp:CheckBox runat="server" ID="used_of_used" /></li>
                                                                                                </ul>
                                                                                                <b>Include JETNET Sale Price Summary Data:</b><asp:CheckBox ID="use_jetnet_data"
                                                                                                    runat="server" /><div class="nonflyout_info_box">
                                                                                                        JETNET gathers sale prices on transactions but only uses this data for statistical
                                                  calculations and therefore no individual sale prices are shown for JETNET transactions.
                                                  Where JETNET sale price data is used in calculations, client transaction data overrides
                                                  JETNET sale price data.
                                                                                                    </div>
                                                                                                <br clear="all" />
                                                                                                <asp:Button runat="server" ID="update_check1" Text="Save Settings" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <asp:Label Visible="false" ID="year_of_current" runat="server" Text="0"></asp:Label><asp:Label
                                                                                        Visible="false" ID="aftt_of_current" runat="server" Text="0"></asp:Label>
                                                                                </div>
                                                                            </td>
                                                                            <td width='27%' align="left" valign="top">
                                                                                <table width='99%' cellpadding="3" cellspacing="0">
                                                                                    <tr>
                                                                                        <td valign="top" align="left" class="companyLabel">
                                                                                            <asp:Label ID="settings_right_label" runat="server"></asp:Label><center>
                                              <asp:Label ID="edit_label_click" runat="server" Visible="false" CssClass="float_right"></asp:Label></center>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="variantTabPanel" Visible="false" HeaderText="Variants" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:Panel runat="server" ID="modelVariants" Visible="false" CssClass="float_left"
                                                                        Width="100%">
                                                                        <asp:ListBox runat="server" CssClass="float_left" Style="min-width: 295px" Width="100%"
                                                                            Rows="10" AutoPostBack="false" ID="VariantList" data-placeholder="Please Pick Model(s)"
                                                                            SelectionMode="Multiple"></asp:ListBox>
                                                                        <asp:Button runat="server" ID="includeVariants" Text="Include Variants" OnClientClick="ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" />
                                                                        <br clear="all" />
                                                                        <div>
                                                                            <span class="tiny red_text float_left padding_top">Variants only applied to Market
                                      Status, News, Wanteds Events, Estimates, Trends and Market/Sold Survey charts and
                                      tables.*</span>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                        </cc1:TabContainer>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="middle" colspan="3" width="100%" class="tabContainerBottomBox">
                                            <asp:UpdatePanel ID="bottom_tab_update_panel" runat="server" ChildrenAsTriggers="True"
                                                UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <cc1:TabContainer ID="TabContainer1" runat="server" Width="100%" ActiveTabIndex="0"
                                                        AutoPostBack="true" BorderStyle="None" Style="margin-left: auto; margin-right: auto;"
                                                        CssClass="dark-theme">
                                                        <cc1:TabPanel ID="trends_tab" runat="server">
                                                            <ContentTemplate>
                                                                <div class="tab_container_div">
                                                                    <div id="chart_div_value_history" runat="server">
                                                                    </div>
                                                                    <asp:Panel runat="server" ID="utilizationOperatorAirportParent" CssClass="columns three">
                                                                        <asp:Panel runat="server" ID="utilizationViewAirport" Visible="False" Style="margin-bottom: 13px;">
                                                                            <div class="Box">
                                                                                <asp:Label runat="server" ID="utilizationViewAirportLabel"></asp:Label><div id="utilizationAirportMap" runat="server" style="width: 100%; height: 250px;"
                                                                                    visible="False">
                                                                                </div>
                                                                            </div>
                                                                        </asp:Panel>
                                                                    </asp:Panel>
                                                                    <asp:CheckBox ID="market_check" runat="server" AutoPostBack="True" Checked="True"
                                                                        Text="Include Market Snapshot Data" Visible="False" />
                                                                    <asp:DropDownList ID="airportDrop" runat="server" Visible="False" AutoPostBack="True"
                                                                        onchange="keepAirportSame(this);">
                                                                    </asp:DropDownList>
                                                                    <asp:CheckBox ID="exclude_check" runat="server" Text="Exclude&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                                                        Visible="False" AutoPostBack="True" />
                                                                    <asp:CheckBox ID="airportExcludeCheck" runat="server" Text="Exclude&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                                                        Visible="False" AutoPostBack="True" onchange="updateExcludeAirport(this);" />
                                                                    <asp:Label runat="server" ID="operator_list_label" Visible="False"></asp:Label><span
                                                                        class="trendsTabImageHolder"><asp:Literal ID="view_trends_label" runat="server"></asp:Literal></span><asp:Literal
                                                                            ID="aircraftViewText" runat="server" Visible="False"></asp:Literal><table cellpadding="1"
                                                                                cellspacing='0'>
                                                                                <tr>
                                                                                    <td align='center'>
                                                                                        <div id="chart_div_tab16_all" style="border-top: 0;">
                                                                                        </div>
                                                                                    </td>
                                                                                    <td align='center'>
                                                                                        <div id="chart_div_tab15_all" style="border-top: 0;">
                                                                                        </div>
                                                                                    </td>
                                                                                    <td align='center'>
                                                                                        <div id="chart_div_tab18_all" style="border-top: 0">
                                                                                        </div>
                                                                                    </td>
                                                                                    <td align='center'>
                                                                                        <div id="chart_div_tab100_all" style="border-top: 0">
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                    <asp:Panel Visible="False" ID="graph_extras" runat="server">
                                                                        <asp:TextBox runat="server" ID="valueGraphText16"></asp:TextBox><div id='png16' runat="server"></div>
                                                                        <asp:TextBox runat="server" ID="valueGraphText15"></asp:TextBox><div id='png15' runat="server"></div>
                                                                        <asp:TextBox runat="server" ID="valueGraphText18"></asp:TextBox><div id='png18' runat="server"></div>
                                                                        <asp:TextBox runat="server" ID="valueGraphText100"></asp:TextBox><div id='png100'
                                                                            runat="server">
                                                                        </div>
                                                                    </asp:Panel>
                                                                    <div class="specialTableContainer">
                                                                        <table id="startTableOper" class="refreshable display_none operatorData">
                                                                            <tfoot>
                                                                                <tr>
                                                                                    <th colspan="5" runat="server" id="acStartTableColspan" style="text-align: right"></th>
                                                                                    <th></th>
                                                                                    <th></th>
                                                                                    <th></th>
                                                                                    <th colspan="3" visible="False" id="acStartTableToggleOff" runat="server"></th>
                                                                                    <th></th>
                                                                                </tr>
                                                                            </tfoot>
                                                                        </table>
                                                                    </div>
                                                                    <asp:DataGrid runat="server" ID="AircraftSearchDataGrid" AutoGenerateColumns="False"
                                                                        Width="100%" Visible="False" CssClass="mGrid" GridLines="None">
                                                                        <Columns>
                                                                            <asp:TemplateColumn>
                                                                                <ItemTemplate>
                                                                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_picture_id")), "<img src='images/camera.png' alt='AC Picture' onclick=""javascript:SubmitTransactionDocumentForm('" & DataBinder.Eval(Container.DataItem, "amod_make_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "amod_model_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString & "'," & DataBinder.Eval(Container.DataItem, "ac_id").ToString & ",0,'');"" class=""cursor""/>", "")%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="MAKE<br />MODEL">
                                                                                <ItemTemplate>
                                                                                    <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%><br />
                                                                                    <%#crmWebClient.DisplayFunctions.WriteModelLink(DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "amod_model_name"), True)%>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Font-Bold="True" />
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="YEAR MFG<br />YEAR DLV">
                                                                                <ItemTemplate>
                                                                                    <%#DataBinder.Eval(Container.DataItem, "ac_mfr_year")%><br />
                                                                                    <%#DataBinder.Eval(Container.DataItem, "ac_year")%>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Font-Bold="True" />
                                                                                <ItemStyle Width="60px" />
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="SERIAL<br />NUMBER">
                                                                                <ItemTemplate>
                                                                                    <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full"), "", "")%>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Font-Bold="True" />
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="REG<br />NUMBER">
                                                                                <ItemTemplate>
                                                                                    <%#DataBinder.Eval(Container.DataItem, "ac_reg_no")%>
                                                                                    <br />
                                                                                    <%#IIf(Trim(Session.Item("useFAAFlightData")) <> "" And Trim(Session.Item("useFAAFlightData")) <> "ARGUS", "<a href='#' onclick=""javascript:load('FAAFlightData.aspx?acid=" & DataBinder.Eval(Container.DataItem, "ac_id").ToString & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;""  title='Flight Data' ><img src='images/ac_active.png' alt='ARG/US TRAQPak Activity Data (Last 90 Days)' border='0' /></a>", "")%>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Font-Bold="True" />
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="STATUS<br />PRICE" Visible="False">
                                                                                <ItemTemplate>
                                                                                    <%#IIf(DataBinder.Eval(Container.DataItem, "ac_forsale_flag").ToString = "Y", "<span class='green_background'>" & DataBinder.Eval(Container.DataItem, "ac_status") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_asking_price")), IIf(DataBinder.Eval(Container.DataItem, "ac_asking").ToString = "Price", "<br /><span class=""emphasis_text"">" & crmWebClient.clsGeneral.clsGeneral.no_zero(DataBinder.Eval(Container.DataItem, "ac_asking_price"), "", True) & "</span>", ""), "<br /><span class=""emphasis_text"">" & DataBinder.Eval(Container.DataItem, "ac_asking").ToString & "</span>") & "</span>", "<span>" & DataBinder.Eval(Container.DataItem, "ac_status") & "</span>")%>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Font-Bold="True" />
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="DELIVERY<br />LISTED" Visible="False">
                                                                                <ItemTemplate>
                                                                                    <%#DataBinder.Eval(Container.DataItem, "ac_delivery")%>
                                                                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_list_date")), "<br />" & crmWebClient.clsGeneral.clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "ac_list_date")), "")%>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Font-Bold="True" />
                                                                                <ItemStyle Width="70px" />
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="COMPANY">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="company_information" runat="server" Text='<%#(crmWebClient.CompanyFunctions.FindEvolutionACCompanies(aclsData_Temp_aspx, DataBinder.Eval(Container.DataItem, "ac_id")))%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Font-Bold="True" />
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="AFTT<br />ENGINE TT<br />SMOH">
                                                                                <HeaderStyle Font-Bold="True" />
                                                                                <ItemStyle Width="120px" />
                                                                                <ItemTemplate>
                                                                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs") & "]", "")%><br />
                                                                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_tot_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_1_tot_hrs") & "]", "")%>
                                                                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_tot_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_2_tot_hrs") & "]", "")%>
                                                                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_tot_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_3_tot_hrs") & "]", "")%>
                                                                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_tot_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_4_tot_hrs") & "]", "")%><br />
                                                                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_soh_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_1_soh_hrs") & "]", "")%>
                                                                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_soh_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_2_soh_hrs") & "]", "")%>
                                                                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_soh_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_3_soh_hrs") & "]", "")%>
                                                                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_soh_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_4_soh_hrs") & "]", "")%><br />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>
                                                                                <ItemTemplate>
                                                                                    <%#crmWebClient.clsGeneral.clsGeneral.Show_Evo_Event_Ac_Listing(DataBinder.Eval(Container.DataItem, "ac_last_event"), DataBinder.Eval(Container.DataItem, "ac_last_aerodex_event"))%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn Visible="False">
                                                                                <ItemTemplate>
                                                                                    <%#IIf(Session.Item("localSubscription").crmServerSideNotes_Flag = True Or Session.Item("localSubscription").crmCloudNotes_Flag = True, crmWebClient.DisplayFunctions.BuildNote(DataBinder.Eval(Container.DataItem, "ac_id"), aclsData_Temp_aspx, "AC"), "")%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                        </Columns>
                                                                        <AlternatingItemStyle CssClass="alt" />
                                                                        <HeaderStyle CssClass="th2" />
                                                                        <ItemStyle CssClass="item_row" VerticalAlign="Top" />
                                                                        <PagerStyle CssClass="pgr" />
                                                                    </asp:DataGrid>
                                                                    <asp:Label ID="end_div" runat="server" Visible="False" Text="</div>"></asp:Label><asp:Chart
                                                                        ID="AVG_PRICE_MONTH" Visible="False" runat="server" ImageStorageMode="UseImageLocation"
                                                                        ImageType="Jpeg" RightToLeft="No">
                                                                        <Series>
                                                                            <asp:Series Name="Series1" ChartArea="ChartArea1">
                                                                            </asp:Series>
                                                                        </Series>
                                                                        <ChartAreas>
                                                                            <asp:ChartArea Name="ChartArea1">
                                                                            </asp:ChartArea>
                                                                        </ChartAreas>
                                                                    </asp:Chart>
                                                                    <asp:Chart ID="FOR_SALE" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
                                                                        Visible="False" RightToLeft="No">
                                                                        <Series>
                                                                            <asp:Series Name="Series1" ChartArea="ChartArea1">
                                                                            </asp:Series>
                                                                        </Series>
                                                                        <ChartAreas>
                                                                            <asp:ChartArea Name="ChartArea1">
                                                                            </asp:ChartArea>
                                                                        </ChartAreas>
                                                                    </asp:Chart>
                                                                    <asp:Chart ID="PER_MONTH" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
                                                                        Visible="False" RightToLeft="No">
                                                                        <Series>
                                                                            <asp:Series Name="Series1" ChartArea="ChartArea1">
                                                                            </asp:Series>
                                                                        </Series>
                                                                        <ChartAreas>
                                                                            <asp:ChartArea Name="ChartArea1">
                                                                            </asp:ChartArea>
                                                                        </ChartAreas>
                                                                    </asp:Chart>
                                                                    <asp:Chart ID="AVG_DAYS_ON" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
                                                                        Visible="False" RightToLeft="No">
                                                                        <Series>
                                                                            <asp:Series ChartArea="ChartArea1" Name="Series1">
                                                                            </asp:Series>
                                                                        </Series>
                                                                        <ChartAreas>
                                                                            <asp:ChartArea Name="ChartArea1">
                                                                            </asp:ChartArea>
                                                                        </ChartAreas>
                                                                    </asp:Chart>
                                                                </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="for_sale_tab" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Button runat="server" ID="FullSaleRefresh" CssClass="display_none" />
                                                                <asp:CheckBox ID="show_sale" runat="server" Visible="false" Text="Display Last Sale Prices"
                                                                    AutoPostBack="true" />
                                                                <asp:Panel runat="server" ID="utilizationViewToggle" Visible="false" CssClass="float_left">
                                                                    <asp:DropDownList runat="server" ID="utilizationViewDropDown" AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                    <asp:CheckBox runat="server" ID="utilizationExcludeCheck" Text="Exclude" onchange="updateExclude(this);keepOperatorsSame();"
                                                                        AutoPostBack="true" />
                                                                    <asp:Label runat="server" ID="utilizationListLabel" CssClass="padding_left"></asp:Label>
                                                                </asp:Panel>
                                                                <asp:UpdatePanel ID="model_summary_update_panel" Visible="true" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:DropDownList ID="operator_drop2" runat="server" Visible="False" AutoPostBack="True">
                                                                        </asp:DropDownList>
                                                                        <asp:Literal runat="server" ID="oper2AirportViewTextToggle" Visible="false"></asp:Literal><asp:TextBox runat="server" ID="tabReorder" CssClass="display_none"></asp:TextBox><asp:TextBox runat="server" ID="fullSaleCurrentIDs" CssClass="display_none"></asp:TextBox><div id="divComparableTab" runat="server" class="display_none">
                                                                            <span>Please wait while the process is running... </span>
                                                                            <br />
                                                                            <br />
                                                                            <img src="Images/loading.gif" alt="Loading..." />
                                                                        </div>
                                                                        <div id="displayDisable" runat="server">
                                                                            <asp:Label ID="projects_text" runat="server" Visible="false" Text="Format: "></asp:Label><asp:DropDownList
                                                                                ID="ac_projects_ddl" runat="server" Visible="false" AutoPostBack="true">
                                                                            </asp:DropDownList>
                                                                            <asp:Label runat="server" ID="edit_export_link" Text="" Visible="false"></asp:Label><a href='' title="Create a snapshot of the aircraft for sale and market values as they are today for future market trend analysis."><asp:Button ID="run_comparable_insert" Text="Save Market Snapshot" Visible="false"
                                                                                OnClientClick="return confirm('Are you sure you want to save this market snapshot?');"
                                                                                runat="server" />
                                                                            </a>
                                                                            <asp:Label runat="server" ID="create_new_format" Visible="false" Text=""></asp:Label><asp:Label ID="informationIconForsale" runat="server" CssClass="float_right"></asp:Label><asp:Label ID="compare_view_current_label" runat="server"></asp:Label><asp:DropDownList ID="fbo_months_drop" runat="server" Visible="false" AutoPostBack="true">
                                                                                <asp:ListItem Text="3 Months" Value="3" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="6 Months" Value="6"></asp:ListItem>
                                                                            </asp:DropDownList><asp:Label ID="compare_view_current_label2" runat="server" Visible="false"></asp:Label><asp:Panel runat="server" ID="date_labels" Visible="false">
                                                                                Start:
                                                                                <asp:TextBox runat="server" ID="start_date" CssClass="datepicker" Width="70px" Style="margin-top: -2px;"></asp:TextBox>End:
                                                                                <asp:TextBox runat="server" ID="end_date" CssClass="datepicker" Width="70px" Style="margin-top: -12px;"></asp:TextBox><div
                                                                                    style="width: 225px; margin-left: 106px; display: none;">
                                                                                    <div id="date_slider">
                                                                                    </div>
                                                                                </div>
                                                                                <asp:CheckBox ID="check_busj" runat="server" Text="Business Jets" />
                                                                                <asp:CheckBox ID="check_bust" runat="server" Text="Business TurboProps" />
                                                                                <asp:CheckBox ID="check_heli" runat="server" Text="Helicopters" />
                                                                                <asp:CheckBox ID="check_comm" runat="server" Text="Commercial" />
                                                                                <asp:Button ID="submit_date_change" Text="Search" runat="server" OnClientClick="re_run_airport_view()" />
                                                                            </asp:Panel>
                                                                            <asp:Panel runat="server" ID="utilizationViewOperator" Visible="False">
                                                                                <div class="Box">
                                                                                    <asp:Label runat="server" ID="utilizationViewOperatorLabel"></asp:Label>
                                                                                </div>
                                                                            </asp:Panel>
                                                                            <div class="tab_container_div forceOverflowMobile">
                                                                                <asp:Literal ID="view_for_sale_label" Text="" runat="server"></asp:Literal><div class="specialTableContainer">
                                                                                    <table id="startTableOper3" class="refreshable display_none operatorData">
                                                                                        <tfoot>
                                                                                            <tr>
                                                                                                <th runat="server" id="Th2" style="text-align: right"></th>
                                                                                                <th></th>
                                                                                                <th></th>
                                                                                                <th></th>
                                                                                                <th></th>
                                                                                                <th></th>
                                                                                                <th></th>
                                                                                                <th></th>
                                                                                                <th></th>
                                                                                                <th visible="False" id="Th3" runat="server"></th>
                                                                                                <th></th>
                                                                                                <th></th>
                                                                                                <th></th>
                                                                                                <th></th>
                                                                                                <th></th>
                                                                                                <th></th>
                                                                                                <th></th>
                                                                                            </tr>
                                                                                        </tfoot>
                                                                                    </table>
                                                                                    <table id="mostCommonOrigins" class="refreshable display_none" width="960">
                                                                                        <tfoot>
                                                                                            <tr>
                                                                                                <th colspan="4" runat="server" id="originTableColspan" style="text-align: right"></th>
                                                                                                <th></th>
                                                                                                <th></th>
                                                                                                <th></th>
                                                                                            </tr>
                                                                                        </tfoot>
                                                                                    </table>
                                                                                </div>
                                                                                <asp:Label ID="add_comparable_field" runat="server" Visible="false"></asp:Label>
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="retail_tab" runat="server">
                                                            <ContentTemplate>
                                                                <asp:UpdatePanel runat="server" ID="update_sold_panel" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox runat="server" ID="EvoRetailSalesIDsCurrent" CssClass="display_none"></asp:TextBox><asp:Label ID="projects_text2" runat="server" Visible="false" Text="Display Format: "></asp:Label><asp:DropDownList ID="ac_projects_ddl2" runat="server" Visible="false" AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="ac_projects_dd_sales" runat="server" Visible="false" AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="compare_view_sold_label2" runat="server" Visible="false"></asp:Label><asp:DropDownList ID="company_range" runat="server" Visible="false" AutoPostBack="true">
                                                                            <asp:ListItem Value="25">25</asp:ListItem>
                                                                            <asp:ListItem Value="50">50</asp:ListItem>
                                                                        </asp:DropDownList><asp:Label ID="compare_view_sold_label" runat="server" Visible="false"></asp:Label><div class="tab_container_div forceOverflowMobile">
                                                                            <asp:CheckBox ID="check_include_internals2" runat="server" Text="Include Internal Sales"
                                                                                Visible="false" AutoPostBack="true" OnCheckedChanged="run_recent_sales" />
                                                                            <asp:CheckBox ID="check_retail_sales2" runat="server" Text="" Visible="false" AutoPostBack="true"
                                                                                Checked="true" OnCheckedChanged="run_recent_sales" />
                                                                            <asp:Label runat="server" ID="check_retail_sales2_label" Visible="false"></asp:Label><asp:CheckBox ID="preownedSales_Only" AutoPostBack="true" runat="server" Visible="false" onclick="javascript:SetWaitCursor();"
                                                                                Text="Pre-Owned Sales Only" />
                                                                            <div class="specialTableContainer" runat="server" visible="false" id="originDataContainer">
                                                                                <table width='100%' cellpadding="0" cellspacing="0">
                                                                                    <tr valign='top'>
                                                                                        <td width='550'>
                                                                                            <div style="height: 530px; overflow-x: hidden;" class="resizeDiv2">
                                                                                                <table id="mostCommonOriginsDestinations" class="refreshable display_none">
                                                                                                </table>

                                                                                                <table id="mostCommonOrigins2" class="refreshable display_none" width="960">
                                                                                                    <tfoot>
                                                                                                        <tr>
                                                                                                            <th colspan="4" runat="server" id="Th4" style="text-align: right"></th>
                                                                                                            <th></th>
                                                                                                            <th></th>
                                                                                                            <th></th>
                                                                                                        </tr>
                                                                                                    </tfoot>
                                                                                                </table>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <asp:Label ID="view_retail_sales_label1" Text="" runat="server"></asp:Label><asp:Label ID="add_comparable_field2" runat="server" Visible="false"></asp:Label>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="event_tab" runat="server">
                                                            <ContentTemplate>
                                                                <div class="tab_container_div">
                                                                    <asp:Panel runat="server" ID="crm_time_panel2" CssClass="display_none light_seafoam_green_header_color">
                                                                        <table width="100%" cellpadding="3" cellspacing="0">
                                                                            <tr>
                                                                                <td align="right" valign="middle" width="50">
                                                                                    <asp:Label ID="Label12" runat="server" Font-Size="9px">Range:</asp:Label></td>
                                                                                <td align="left" valign="top">
                                                                                    <asp:RadioButtonList ID="crm_event_time2" Visible="true" RepeatColumns="10" CellPadding="3"
                                                                                        runat="server" RepeatLayout="Table" AutoPostBack="true" Font-Size="9px">
                                                                                        <asp:ListItem Value="1">One Day</asp:ListItem>
                                                                                        <asp:ListItem Selected="True" Value="7">One Week</asp:ListItem>
                                                                                        <asp:ListItem Value="30">One Month</asp:ListItem>
                                                                                        <asp:ListItem Value="90">Three Months</asp:ListItem>
                                                                                    </asp:RadioButtonList></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" valign="middle">
                                                                                    <asp:Label ID="Label13" runat="server" Font-Size="9px">Category:</asp:Label></td>
                                                                                <td align="left" valign="top">
                                                                                    <asp:RadioButtonList ID="crm_event_category2" Visible="true" RepeatColumns="12" CellPadding="3"
                                                                                        runat="server" RepeatLayout="flow" AutoPostBack="true" Font-Size="9px">
                                                                                        <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                                                                    </asp:RadioButtonList></td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <asp:CheckBox ID="check_owner_ac" Text="Display Aircraft for Owners" runat="server"
                                                                        Visible="false" AutoPostBack="true" />
                                                                    <div class="specialTableContainer" runat="server" visible="false" id="flightDataContainer">
                                                                        <table width='100%' cellpadding="0" cellspacing="0">
                                                                            <tr valign='top'>
                                                                                <td width='550'>
                                                                                    <div class="specialTableContainer">
                                                                                        <asp:Literal runat="server" ID="flightDataLabel"></asp:Literal><table id="flightData" class="refreshable">
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <asp:Literal ID="view_events_label" Text="" runat="server"></asp:Literal>
                                                                </div>
                                                                <asp:Panel ID="CRM_MODEL_MARKET_SPI_GRAPH_PANEL" runat="server" Visible="False">
                                                                    <br />
                                                                    <table cellpadding="0" cellspacing="0" border='0' valign='top' width="100%">
                                                                        <tr valign='top'>
                                                                            <td>
                                                                                <table cellpadding='0' cellspacing='0' border='0'>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <b>Avg Asking vs Selling Price By Year Mfr($k)</b>
                                                                                            <br />
                                                                                            <div id="CRM2chart_div_survey_ask_vs_sell_all">
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td>
                                                                                <div style='width: 2px; height: 100px; background-color: #D8D8D8;'>
                                                                                    &nbsp;
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <table cellpadding='0' cellspacing='0' border='0'>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <b>Avg Selling Price By Year Mfr($k)</b>
                                                                                            <br />
                                                                                            <div id="CRM2chart_div_survey_sell_all">
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <asp:Label ID="CRM_spiMiddle" Text="" runat="server"></asp:Label><table cellpadding="0"
                                                                        cellspacing="0" border='0' valign='top' width="100%">
                                                                        <tr valign='top'>
                                                                            <td>
                                                                                <table cellpadding='0' cellspacing='0' border='0'>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="Label7" runat="server" Text="<b>Avg Asking vs Selling Price ($k) - (For Asking with Sold)</b>"></asp:Label><br />
                                                                                            <div id="CRM2chart_div_survey">
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <b>Avg Asking Price ($k) - (All Asking Prices)</b>
                                                                                            <br />
                                                                                            <div id="CRM2chart_div_sold_avg_asking_all">
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <b>Avg. Sold Price % of Asking Price - (For Asking with Sold)</b>
                                                                                            <br />
                                                                                            <div id="CRM2chart_div_percent_asking_all">
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <div id="CRM2chart_div_dom_all">
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <div style='width: 2px; height: 700px; background-color: #D8D8D8;'>
                                                                                    &nbsp;
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <table cellpadding='0' cellspacing='0' border='0'>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <b>Avg Asking vs Selling Price ($k) - (All Asking/Sold Prices)</b>
                                                                                            <br />
                                                                                            <div id="CRM2chart_div_survey_2_all">
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <b>Avg Sold Price ($k) - (All Sold Prices)</b>
                                                                                            <br />
                                                                                            <div id="CRM2chart_div_sold_avg_sold_all">
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align='center'>
                                                                                            <b>Variance of Sold Price from Asking Price - (For Asking with Sold)</b>
                                                                                            <br />
                                                                                            <div id="CRM2chart_div_variance_all">
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <div id="CRM2chart_div_aftt_all">
                                                                                </div>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="news_tab" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="owner_drop" runat="server" Visible="false" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="hidden_market_value_source_label" Visible="false"></asp:Label><asp:Label
                                                                    ID="hidden_market_survey1" runat="server" Text="" Visible="false"></asp:Label><asp:Label
                                                                        ID="hidden_market_survey2" runat="server" Text="" Visible="false"></asp:Label><asp:Label
                                                                            ID="hidden_market_survey3" runat="server" Text="" Visible="false"></asp:Label><asp:Label
                                                                                ID="hidden_market_survey4" runat="server" Text="" Visible="false"></asp:Label><asp:Label
                                                                                    ID="hidden_market_survey5" runat="server" Text="" Visible="false"></asp:Label><asp:Label
                                                                                        ID="hidden_market_survey6" runat="server" Text="" Visible="false"></asp:Label><asp:DropDownList
                                                                                            ID="operator_drop" runat="server" Visible="false" AutoPostBack="true">
                                                                                        </asp:DropDownList>
                                                                <asp:CheckBox ID="check_operator_ac" Text="Display Aircraft for Operators" runat="server"
                                                                    Visible="false" AutoPostBack="true" />
                                                                <div class="specialTableContainer">
                                                                    <table id="acData" class="refreshable display_none">
                                                                        <tfoot>
                                                                            <tr>
                                                                                <th colspan="4" style="text-align: right"></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                                <th colspan="10"></th>
                                                                            </tr>
                                                                        </tfoot>
                                                                    </table>
                                                                </div>
                                                                <div class="tab_container_div">
                                                                    <asp:Panel ID="value_panel_all" runat="server" Visible="false">
                                                                        <div style='height: 370px; width: 970px; overflow: auto;'>
                                                                            <table cellpadding="2" cellspacing="0" width='100%'>
                                                                                <tr valign='top'>
                                                                                    <td>
                                                                                        <div id="value_panel1_all">
                                                                                        </div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style='width: 2px; height: 210px; background-color: #D8D8D8;'>
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div id="value_panel2_all">
                                                                                        </div>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan='3' align='center'>
                                                                                        <asp:Label runat="server" ID="view_snapshot_label"></asp:Label></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div id="value_panel3_all">
                                                                                        </div>
                                                                                        <br />
                                                                                        <div id="value_panel5_all">
                                                                                        </div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div id="value_panel4_all">
                                                                                        </div>
                                                                                        <br />
                                                                                        <div id="value_panel6_all">
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </asp:Panel>
                                                                    <asp:Label runat="server" ID="view_news_label1"></asp:Label>
                                                                </div>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="wanted_tab" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Button runat="server" ID="SoldSurveyIdButton" CssClass="display_none" />
                                                                <asp:UpdatePanel runat="server" ID="update_sold_panel2" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <div class="tab_container_div">
                                                                            <asp:TextBox runat="server" ID="SoldSurveyCurrentID" CssClass="display_none"></asp:TextBox><asp:Label ID="project_text4" runat="server" Visible="false" Text="Display Format: "></asp:Label><asp:DropDownList ID="ac_projects_ddl4" runat="server" Visible="false" AutoPostBack="true">
                                                                            </asp:DropDownList>
                                                                            <asp:CheckBox ID="check_include_internals" runat="server" Text="Include Internal Sales"
                                                                                Visible="false" AutoPostBack="true" OnCheckedChanged="run_recent_sales" />
                                                                            <asp:CheckBox ID="check_retail_sales" runat="server" Text="" Visible="false" AutoPostBack="true"
                                                                                Checked="true" OnCheckedChanged="run_recent_sales" />
                                                                            <asp:Label runat="server" Visible="false" ID="check_retail_sales_label">Retail Transactions</asp:Label><asp:DropDownList ID="year_month_settings" runat="server" CssClass="float_right"
                                                                                AutoPostBack="true" onchange="ChangeTheMouseCursorOnItemParentDocument('cursor_wait');"
                                                                                Visible="false">
                                                                                <asp:ListItem Value='6'>6 Months</asp:ListItem>
                                                                                <asp:ListItem Value='12'>1 Year</asp:ListItem>
                                                                                <asp:ListItem Value='18' Selected="True">18 Months</asp:ListItem>
                                                                                <asp:ListItem Value='24'>2 Years</asp:ListItem>
                                                                                <asp:ListItem Value='36'>3 Years</asp:ListItem>
                                                                                <asp:ListItem Value='48'>4 Years</asp:ListItem>
                                                                                <asp:ListItem Value='60'>5 Years</asp:ListItem>
                                                                            </asp:DropDownList><asp:Label runat="server" ID="months_text" Text="Last:" Visible="false" CssClass="float_right margin_right"></asp:Label><asp:Label ID="view_wanteds_label1" runat="server"></asp:Label><asp:Chart ID="ANALYTICS_HISTORY" Visible="False" runat="server" ImageStorageMode="UseImageLocation"
                                                                                ImageType="Jpeg">
                                                                                <Series>
                                                                                    <asp:Series Name="Series1" ChartArea="ChartArea1">
                                                                                    </asp:Series>
                                                                                </Series>
                                                                                <ChartAreas>
                                                                                    <asp:ChartArea Name="ChartArea1">
                                                                                    </asp:ChartArea>
                                                                                </ChartAreas>
                                                                            </asp:Chart>
                                                                            <asp:Chart ID="ANALYTICS_CURRENT_MARKET" Visible="False" runat="server" ImageStorageMode="UseImageLocation"
                                                                                ImageType="Jpeg">
                                                                                <Series>
                                                                                    <asp:Series Name="Series1" ChartArea="ChartArea1">
                                                                                    </asp:Series>
                                                                                </Series>
                                                                                <ChartAreas>
                                                                                    <asp:ChartArea Name="ChartArea1">
                                                                                    </asp:ChartArea>
                                                                                </ChartAreas>
                                                                            </asp:Chart>
                                                                            <asp:Chart ID="ANALYTICS_SOLD_COMPARABLES" Visible="False" runat="server" ImageStorageMode="UseImageLocation"
                                                                                ImageType="Jpeg">
                                                                                <Series>
                                                                                    <asp:Series Name="Series1" ChartArea="ChartArea1">
                                                                                    </asp:Series>
                                                                                </Series>
                                                                                <ChartAreas>
                                                                                    <asp:ChartArea Name="ChartArea1">
                                                                                    </asp:ChartArea>
                                                                                </ChartAreas>
                                                                            </asp:Chart>
                                                                            <asp:Chart ID="ANALYTICS_RECENT_SALES" Visible="False" runat="server" ImageStorageMode="UseImageLocation"
                                                                                ImageType="Jpeg">
                                                                                <Series>
                                                                                    <asp:Series Name="Series1" ChartArea="ChartArea1">
                                                                                    </asp:Series>
                                                                                </Series>
                                                                                <ChartAreas>
                                                                                    <asp:ChartArea Name="ChartArea1">
                                                                                    </asp:ChartArea>
                                                                                </ChartAreas>
                                                                            </asp:Chart>
                                                                            <asp:Chart ID="ANALYTICS_MARKET_STATUS" Visible="False" runat="server" ImageStorageMode="UseImageLocation"
                                                                                ImageType="Jpeg">
                                                                                <Series>
                                                                                    <asp:Series Name="Series1" ChartArea="ChartArea1">
                                                                                    </asp:Series>
                                                                                </Series>
                                                                                <ChartAreas>
                                                                                    <asp:ChartArea Name="ChartArea1">
                                                                                    </asp:ChartArea>
                                                                                </ChartAreas>
                                                                            </asp:Chart>
                                                                            <asp:Chart ID="ANALYTICS_MARKET_SURVEY" Visible="False" runat="server" ImageStorageMode="UseImageLocation"
                                                                                ImageType="Jpeg">
                                                                                <Series>
                                                                                    <asp:Series Name="Series1" ChartArea="ChartArea1">
                                                                                    </asp:Series>
                                                                                </Series>
                                                                                <ChartAreas>
                                                                                    <asp:ChartArea Name="ChartArea1">
                                                                                    </asp:ChartArea>
                                                                                </ChartAreas>
                                                                            </asp:Chart>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                <div class="specialTableContainer">
                                                                    <table id="startTableOper2" class="refreshable display_none operatorData">
                                                                        <tfoot>
                                                                            <tr>
                                                                                <th runat="server" id="Th1" style="text-align: right"></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                                <th colspan="2"></th>
                                                                            </tr>
                                                                        </tfoot>
                                                                    </table>
                                                                </div>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="documents_tab" runat="server">
                                                            <ContentTemplate>
                                                                <asp:UpdatePanel runat="server" ID="update_sold_panel3" UpdateMode="Conditional"
                                                                    Visible="true">
                                                                    <ContentTemplate>
                                                                        <div class="tab_container_div">
                                                                            <asp:Label ID="projects_text3" runat="server" Visible="false" Text="Display Format: "></asp:Label><asp:DropDownList
                                                                                ID="ac_projects_dd3" runat="server" Visible="false" AutoPostBack="true">
                                                                            </asp:DropDownList>
                                                                            <asp:Label ID="view_documents_label1" Text="" runat="server"></asp:Label><asp:Label
                                                                                ID="prospects_label" runat="server" Visible="false"></asp:Label>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="operators_tab" runat="server">
                                                            <ContentTemplate>
                                                                <div class="tab_container_div">
                                                                    <asp:Literal ID="view_operators_label" Text="" runat="server"></asp:Literal><asp:Panel
                                                                        ID="google_sold_trends_graphs3" runat="server" Visible="false">
                                                                        <br />
                                                                        <table cellpadding="0" cellspacing="0" border='0' width="100%">
                                                                            <tr valign='top'>
                                                                                <td>
                                                                                    <table cellpadding='0' cellspacing='0' border='0'>
                                                                                        <tr>
                                                                                            <td align="center">
                                                                                                <b>Avg Asking vs Selling Price By Year Mfr($k)</b>
                                                                                                <br />
                                                                                                <div id="22chart_div_survey_ask_vs_sell_all">
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td>
                                                                                    <div style='width: 2px; height: 100px; background-color: #D8D8D8;'>
                                                                                        &nbsp;
                                                                                    </div>
                                                                                </td>
                                                                                <td>
                                                                                    <table cellpadding='0' cellspacing='0' border='0'>
                                                                                        <tr>
                                                                                            <td align="center">
                                                                                                <b>Avg Selling Price By Year Mfr($k)</b>
                                                                                                <br />
                                                                                                <div id="22chart_div_survey_sell_all">
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <asp:Label runat="server" ID="spi_middle_label"></asp:Label><asp:Panel ID="google_sold_trends_graphs"
                                                                        runat="server" Visible="false">
                                                                        <div style='height: 370px; width: 970px; overflow: auto;'>
                                                                            <table cellpadding="2" cellspacing="0">
                                                                                <tr valign='top'>
                                                                                    <td>
                                                                                        <table cellpadding='0' cellspacing='0' border='0'>
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <asp:Label ID="ask_v_selling_label2" runat="server" Visible="true" Text="<b>Avg Asking vs Selling Price ($k) - (For Asking with Sold)</b>"></asp:Label></td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div id="orig_chart_div_survey_all">
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <b>Avg Asking Price ($k) - (All Asking Prices)</b>
                                                                                                    <br />
                                                                                                    <div id="chart_div_sold_avg_asking_all">
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <b>Avg. Sold Price % of Asking Price - (For Asking with Sold)</b>
                                                                                                    <br />
                                                                                                    <div id="chart_div_percent_asking_all">
                                                                                                    </div>
                                                                                                    <br />
                                                                                                    <div id="chart_div_dom_all">
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style='width: 2px; height: 500px; background-color: #D8D8D8;'>
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <table cellpadding='0' cellspacing='0' border='0'>
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <b>Avg Sold Price ($k) - (All Sold Prices)</b>
                                                                                                    <br />
                                                                                                    <div id="chart_div_sold_avg_sold_all">
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <b>Variance of Sold Price from Asking Price - (For Asking with Sold)</b>
                                                                                                    <br />
                                                                                                    <div id="chart_div_variance_all">
                                                                                                    </div>
                                                                                                    <br />
                                                                                                    <div id="chart_div_aftt_all">
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </asp:Panel>
                                                                    <asp:Label ID="hidden_spi_graph_text" runat="server" Text="" Visible="false"></asp:Label><asp:Label
                                                                        ID="hidden_spi_graph_text2" runat="server" Text="" Visible="false"></asp:Label><asp:Label
                                                                            ID="hidden_spi_graph_text3" runat="server" Text="" Visible="false"></asp:Label><asp:Label
                                                                                ID="hidden_spi_graph_text4" runat="server" Text="" Visible="false"></asp:Label><asp:Label
                                                                                    ID="hidden_spi_graph_text5" runat="server" Text="" Visible="false"></asp:Label><asp:Label
                                                                                        ID="hidden_spi_graph_text6" runat="server" Text="" Visible="false"></asp:Label><asp:Label
                                                                                            ID="hidden_spi_graph_text7" runat="server" Text="" Visible="false"></asp:Label><asp:Label
                                                                                                ID="hidden_spi_graph_text8" runat="server" Text="" Visible="false"></asp:Label>
                                                                </div>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="charter_tab" runat="server">
                                                            <ContentTemplate>
                                                                <div class="tab_container_div">
                                                                    <asp:Panel ID="all_google_charts" runat="server" Visible="false">
                                                                        <div style='height: 370px; width: 970px; overflow: auto;'>
                                                                            <table cellpadding="2" cellspacing="0">
                                                                                <tr valign='top'>
                                                                                    <td>
                                                                                        <div id="chart_div_value_history_all">
                                                                                        </div>
                                                                                        <asp:Label runat="server" ID="view_history_chart"></asp:Label></td>
                                                                                    <td>
                                                                                        <div style='width: 2px; height: 500px; background-color: #D8D8D8;'>
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </td>
                                                                                    <td valign='top' align='left'>
                                                                                        <div id="chart_div_current_all">
                                                                                        </div>
                                                                                        <asp:Label runat="server" ID="current_market_chart"></asp:Label></td>
                                                                                    <td>
                                                                                        <div style='width: 2px; height: 500px; background-color: #D8D8D8;'>
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </td>
                                                                                    <td valign='top' align='left'>
                                                                                        <div id="chart_div_sold_all">
                                                                                        </div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style='width: 2px; height: 500px; background-color: #D8D8D8;'>
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </td>
                                                                                    <td valign='top' align='left'>
                                                                                        <div id="chart_div_status_all">
                                                                                        </div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style='width: 2px; height: 500px; background-color: #D8D8D8;'>
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </td>
                                                                                    <td valign='top' align='left'>
                                                                                        <div id="chart_div_survey_all">
                                                                                        </div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style='width: 2px; height: 500px; background-color: #D8D8D8;'>
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </td>
                                                                                    <td valign='top' align='left'>
                                                                                        <div id="chart_div_recent_all">
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </asp:Panel>
                                                                    <asp:Label ID="view_charter_label1" Text="" runat="server"></asp:Label><asp:Chart
                                                                        ID="charter_chart" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
                                                                        Visible="false">
                                                                        <Series>
                                                                            <asp:Series>
                                                                            </asp:Series>
                                                                        </Series>
                                                                        <ChartAreas>
                                                                            <asp:ChartArea Name="ChartArea1">
                                                                            </asp:ChartArea>
                                                                        </ChartAreas>
                                                                    </asp:Chart>
                                                                </div>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="lease_tab" runat="server">
                                                            <ContentTemplate>
                                                                <div class="tab_container_div">
                                                                    <asp:Literal ID="view_lease_label" Text="" runat="server"></asp:Literal><asp:ListBox
                                                                        ID="add_compare_field_list" runat="server" Visible="false" Height="150" Width="200"></asp:ListBox>
                                                                    <asp:Button runat="server" ID="change_comparable" Visible="false" Text="Comparable" />
                                                                    <asp:Button runat="server" ID="cancel_comparable" Visible="false" Text="Cancel" />
                                                                    <asp:Chart ID="LEASES_SOLD_PER_MONTH" runat="server" ImageStorageMode="UseImageLocation"
                                                                        ImageType="Jpeg" Visible="False">
                                                                        <Series>
                                                                            <asp:Series>
                                                                            </asp:Series>
                                                                        </Series>
                                                                        <ChartAreas>
                                                                            <asp:ChartArea Name="ChartArea1">
                                                                            </asp:ChartArea>
                                                                        </ChartAreas>
                                                                    </asp:Chart>
                                                                </div>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="spi_tab" runat="server">
                                                            <ContentTemplate>
                                                                <div class="tab_container_div">
                                                                    <asp:TextBox runat="server" ID="valueEstimateCurrentID" CssClass="display_none"></asp:TextBox><asp:Button
                                                                        runat="server" Text="Refresh Graph" ID="RefreshCurrentValueGraph" CssClass="display_none" />
                                                                    <asp:Panel runat="server" ID="crm_time_panel1" CssClass="display_none light_seafoam_green_header_color" Visible="false">
                                                                        <table width="100%" cellpadding="3" cellspacing="0">
                                                                            <tr>
                                                                                <td align="right" valign="middle" width="50">
                                                                                    <asp:Label ID="Label8" runat="server" Font-Size="9px">Range:</asp:Label></td>
                                                                                <td align="left" valign="top">
                                                                                    <asp:RadioButtonList ID="crm_event_time1" Visible="true" RepeatColumns="10" CellPadding="3"
                                                                                        runat="server" RepeatLayout="Table" AutoPostBack="true" Font-Size="9px">
                                                                                        <asp:ListItem Value="1">One Day</asp:ListItem>
                                                                                        <asp:ListItem Selected="True" Value="7">One Week</asp:ListItem>
                                                                                        <asp:ListItem Value="30">One Month</asp:ListItem>
                                                                                        <asp:ListItem Value="90">Three Months</asp:ListItem>
                                                                                    </asp:RadioButtonList></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" valign="middle">
                                                                                    <asp:Label ID="Label9" runat="server" Font-Size="9px">Category:</asp:Label></td>
                                                                                <td align="left" valign="top">
                                                                                    <asp:RadioButtonList ID="crm_event_category1" Visible="true" RepeatColumns="12" CellPadding="3"
                                                                                        runat="server" RepeatLayout="flow" AutoPostBack="true" Font-Size="9px">
                                                                                        <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                                                                    </asp:RadioButtonList></td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <table cellpadding="1" cellspacing='0'>
                                                                        <tr>
                                                                            <td>
                                                                                <div id="2chart_div_sold_avg_asking_all"></div>
                                                                            </td>
                                                                            <td>
                                                                                <div id="2chart_div_percent_asking_all"></div>
                                                                            </td>
                                                                            <td>
                                                                                <div id="2chart_div_sold_avg_sold_all"></div>
                                                                            </td>
                                                                            <td>
                                                                                <div id="2chart_div_variance_all"></div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <div id="2chart_div_survey_ask_vs_sell_all"></div>
                                                                            </td>
                                                                            <td>
                                                                                <div id="2chart_div_survey_2_all"></div>
                                                                            </td>
                                                                            <td>
                                                                                <div id="2chart_div_survey_sell_all"></div>
                                                                            </td>
                                                                            <td></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align='center'>
                                                                                <div id="chart_div_tab16_all" style="border-top: 0;">
                                                                                </div>
                                                                            </td>
                                                                            <td align='center'>
                                                                                <div id="chart_div_tab15_all" style="border-top: 0;">
                                                                                </div>
                                                                            </td>
                                                                            <td align='center'>
                                                                                <div id="chart_div_tab18_all" style="border-top: 0">
                                                                                </div>
                                                                            </td>
                                                                            <td align='center'>
                                                                                <div id="chart_div_tab100_all" style="border-top: 0">
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>

                                                                    <asp:Literal ID="view_spi_label" Text="" runat="server"></asp:Literal><asp:Chart ID="SPI_QUARTER" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
                                                                        Visible="False">
                                                                        <Series>
                                                                            <asp:Series>
                                                                            </asp:Series>
                                                                        </Series>
                                                                        <ChartAreas>
                                                                            <asp:ChartArea Name="ChartArea1">
                                                                            </asp:ChartArea>
                                                                        </ChartAreas>
                                                                    </asp:Chart>




                                                                </div>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="location_tab" runat="server">
                                                            <ContentTemplate>
                                                                <div style="height: 370px; width: 100%; overflow: auto;">
                                                                    <p>
                                                                        <table id="locationViewTopTable" width="100%" cellpadding="4" cellspacing="0">
                                                                            <tr>
                                                                                <td align="center" valign="top" width="70%" style="width: 70%;">
                                                                                    <div id="view_location_tab_map_canvas" style="width: 100%; height: 426px;">
                                                                                    </div>
                                                                                </td>
                                                                                <td align="left" valign="top" width="30%" style="width: 30%;">
                                                                                    <asp:Label ID="view_location_tab_label_display_by" runat="server" Text="&nbsp;&nbsp;Sort Aircraft By : "
                                                                                        Visible="false"></asp:Label><asp:DropDownList runat="server" ID="view_location_tab_display_by"
                                                                                            AutoPostBack="True" Visible="false">
                                                                                        </asp:DropDownList>
                                                                                    <br />
                                                                                    <asp:Literal ID="view_location_tab_label" Text="" runat="server"></asp:Literal></td>
                                                                            </tr>
                                                                        </table>
                                                                    </p>
                                                                </div>
                                                                <script type="text/javascript" language="javascript">

                                                                    var tab_location_map = null;

                                                                    function initialize_tab_map() {
                                                                        var mapOptions = {
                                                                            zoom: 2,
                                                                            center: new google.maps.LatLng(0, 0),
                                                                            mapTypeId: google.maps.MapTypeId.HYBRID
                                                                        };

                                                                        //alert("show default location tab map");

                                                                        var tab_mapDiv = document.getElementById("view_location_tab_map_canvas");
                                                                        var map = new google.maps.Map(tab_mapDiv, mapOptions);

                                                                        if ((map != null) && (typeof (map) != "undefined")) {

                                                                            tab_location_map = map;

                                                                        }
                                                                    }

                                                                    function center_location_tab_map(latitude, longitude, zoom_level) {

                                                                        if (Number(latitude) == 0 && Number(longitude) == 0 && Number(zoom_level) == 0) { //not initalizing map, do not ignore this
                                                                            initialize_tab_map();
                                                                            return false;
                                                                        }

                                                                        //Setting up the new options for the map.
                                                                        var mapOptions = {
                                                                            zoom: zoom_level,
                                                                            center: new google.maps.LatLng(latitude, longitude),
                                                                            mapTypeId: google.maps.MapTypeId.HYBRID
                                                                        };

                                                                        //alert("show location tab map[lat:" + latitude + "][lng:" + longitude + "][zl:" + zoom_level + "]");
                                                                        var tab_mapDiv = document.getElementById("view_location_tab_map_canvas");
                                                                        var map = new google.maps.Map(tab_mapDiv, mapOptions);

                                                                        //finding the map.    
                                                                        if ((map != null) && (typeof (map) != "undefined")) {

                                                                            tab_location_map = map;
                                                                            google.maps.event.clearListeners(window, 'resize');

                                                                        }
                                                                    }

                                                                    function add_location_tab_listener(marker, title, counter, link, map) { //adding listener on click event. Basically adds a popup window with predetermined text on click event of marker.

                                                                        var contentString = "";

                                                                        if (Number(counter) > 0) {

                                                                            contentString = '<div id="content"><div id="siteNotice"></div>' +
                                                                                '<h1 id="firstHeading" class="firstHeading">' + title + '</h1>' +
                                                                                '<div id="bodyContent"><p><b>Number of aircraft at this location is ' + counter + '</b></p>' +
                                                                                '<p><a href="' + link + '" title="' + link + '">Click to view aircraft at this location</a></p></div></div>';
                                                                        }
                                                                        else {
                                                                            if (Number(counter) == -1) {
                                                                                contentString = '<div id="content"><div id="siteNotice"></div>' +
                                                                                    '<h1 id="firstHeading" class="firstHeading">' + title + '</h1>' +
                                                                                    '<div id="bodyContent"><p><a href="' + link + '" title="' + link + '">Click to view aircraft at this location</a></p></div></div>';
                                                                            }
                                                                            else {
                                                                                contentString = '<div id="content"><div id="siteNotice"></div>' +
                                                                                    '<h1 id="firstHeading" class="firstHeading">' + title + '</h1>' +
                                                                                    '<div id="bodyContent"></div></div>';

                                                                            }
                                                                        }

                                                                        var infowindow = new google.maps.InfoWindow({ content: contentString });

                                                                        //Then go ahead and add the listener marker to the map.
                                                                        google.maps.event.addListener(marker, 'click', function () {
                                                                            infowindow.open(map, marker);
                                                                        });
                                                                    }

                                                                    function add_location_tab_marker(location_title, latitude, longitude, counter, link) { //adding a new marker to the map ... Basically adds a popup window with predetermined text on click event of marker.

                                                                        //alert("add marker to location tab map");

                                                                        //finding the map.
                                                                        if ((tab_location_map != null) && (typeof (tab_location_map) != "undefined")) {

                                                                            var icon = {
                                                                                url: '../images/evoPlane.png'
                                                                            };

                                                                            //creating the marker for the map based on latitude, longitude
                                                                            var marker = new google.maps.Marker({
                                                                                position: new google.maps.LatLng(latitude, longitude),
                                                                                map: tab_location_map,
                                                                                icon: icon,
                                                                                title: location_title
                                                                            });

                                                                            google.maps.event.clearListeners(marker, 'onclick');
                                                                            add_location_tab_listener(marker, location_title, counter, link, tab_location_map);

                                                                        }
                                                                    }

                                                                </script>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="star_tab" runat="server">
                                                            <ContentTemplate>
                                                                <div class="tab_container_div">
                                                                    <a href="#" id="mobilePopLink" runat="server" onclick="javascript:return PopUpPanel();"
                                                                        visible="false">Open New Window</a>
                                                                    <asp:Panel runat="server" ID="starReportPanel">
                                                                        <asp:Literal ID="view_star_reports_label" Text="" runat="server"></asp:Literal>
                                                                    </asp:Panel>
                                                                </div>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="range_tab" runat="server">
                                                            <ContentTemplate>
                                                                <cc1:ModalPopupExtender ID="MPE" runat="server" TargetControlID="google_map_hidden_button"
                                                                    PopupControlID="google_map" BackgroundCssClass="modalBackground" DropShadow="true"
                                                                    CancelControlID="CancelButton" />
                                                                <asp:Button ID="google_map_hidden_button" runat="server" Text="Button" Style="display: none;" />
                                                                <asp:Panel ID="Panel2" Width="800" runat="server">
                                                                    <asp:Panel ID="google_map" Width="800" runat="server" CssClass="modalPopup" Style="display: none">
                                                                        <table width="100%" cellpadding="4" cellspacing="0">
                                                                            <tr>
                                                                                <td align="left" valign="top" width="80%">
                                                                                    <div id="div_large_mapID" style="width: 100%; height: 590px">
                                                                                    </div>
                                                                                </td>
                                                                                <td align="center" valign="top" width="20%">
                                                                                    <asp:UpdatePanel runat="server" ID="large_map_update" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:Label ID="airport_information_label_large" runat="server" Text=""></asp:Label>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                    <asp:Button ID="CancelButton" runat="server" Text="Collapse Range Tab" />
                                                                                    <br />
                                                                                    <br />
                                                                                    <table align="left">
                                                                                        <tr>
                                                                                            <td align='left'>
                                                                                                <i><b>Assumptions:</b> Range values presented for turbine powered, fixed wing, business
                                                  aircraft are predominantly expressed under the requirements for NBAA IFR Reserves
                                                  and 0 wind.</i> </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <br clear="all" />
                                                                    </asp:Panel>
                                                                </asp:Panel>
                                                                <div class="tab_container_div">
                                                                    <table width="100%" cellpadding="2" cellspacing="0">
                                                                        <tr>
                                                                            <td align="left" valign="top">
                                                                                <table width="100%" cellpadding="2" cellspacing="0" class="search_box">
                                                                                    <tr>
                                                                                        <td align="left" valign="middle" style="padding-left: 5px;">
                                                                                            <strong>Airport&nbsp;(IATA/ICAO)&nbsp;Code</strong>&nbsp;:&nbsp;
                                                                                            <asp:TextBox ID="range_search_text" runat="server"></asp:TextBox></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" valign="middle">
                                                                                            <div id="ExpandableTitleBar">
                                                                                                <asp:Panel ID="TitlePanelTM" runat="server" Width="100%">
                                                                                                    <table width="100%" cellpadding="2" cellspacing="0">
                                                                                                        <tr>
                                                                                                            <td align="left" valign="middle">
                                                                                                                <strong>Compare&nbsp;Models&nbsp;</strong><asp:Image ID="ImageToControlTM" runat="server"
                                                                                                                    ImageUrl="/Images/expand.jpg" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </asp:Panel>
                                                                                            </div>
                                                                                            <cc1:CollapsiblePanelExtender ID="PanelCollapse" runat="server" TargetControlID="compare_models"
                                                                                                ExpandControlID="TitlePanelTM" Collapsed="true" ExpandedText="Hide Models" CollapsedText="Select Models"
                                                                                                ImageControlID="ImageToControlTM" ExpandedImage="/Images/root.jpg" CollapsedImage="/Images/expand.jpg"
                                                                                                SuppressPostBack="False" CollapseControlID="TitlePanelTM">
                                                                                            </cc1:CollapsiblePanelExtender>
                                                                                            <div id="ExpandableContent">
                                                                                                <asp:Panel ID="compare_models" runat="server">
                                                                                                    <table width="100%" cellpadding="2" cellspacing="0">
                                                                                                        <tr>
                                                                                                            <td align="left" valign="top" width="60">
                                                                                                                <asp:Label ID="first_model_text" runat="server" Text="Model #1:" ForeColor="#00CD00"
                                                                                                                    Font-Bold="true"></asp:Label></td>
                                                                                                            <td align="left" valign="top">
                                                                                                                <asp:DropDownList ID="first_model" runat="server" Width="100%">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left" valign="top">
                                                                                                                <asp:Label ID="second_model_text" runat="server" Text="Model #2:" ForeColor="#0276FD"
                                                                                                                    Font-Bold="true"></asp:Label></td>
                                                                                                            <td align="left" valign="top">
                                                                                                                <asp:DropDownList ID="second_model" runat="server" Width="100%">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </asp:Panel>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center" valign="middle">
                                                                                            <asp:Button ID="search_code" runat="server" Text="Display Range Map" OnClick="views_range_search_IATA_ICAO_AIRPORT" />
                                                                                            &nbsp;&nbsp;
                                                                                            <input type="button" id="btn_show_mapID" value="Expand Range Tab" onclick="showPopup();" /><br />
                                                                                            <br />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <asp:Label ID="airport_information" runat="server" Text=""></asp:Label><p align="center"
                                                                                    class="important_text">
                                                                                    <b>*</b>In Order to View a Range, Please type in an <b>IATA Code</b>, <b>ICAO Code</b> or <b>Airport Name</b>, choose comparison model(s) and click &quot;Display Range
                                          Map&quot;.
                                                                                    <br />
                                                                                    <br />
                                                                                    <i><b>Assumptions:</b> Range values presented for turbine powered, fixed wing, business
                                            aircraft are predominantly expressed under the requirements for NBAA IFR Reserves
                                            and 0 wind.</i>
                                                                                    <br />
                                                                                    <br />
                                                                                    <i><b>Capturing Range Map Images:</b> To capture the range map images for use in other
                                            documents use the following approach: - PC (hold down Ctrl and press PrntScr button)
                                            - Mac (command – shift - #4) </i>
                                                                                </p>
                                                                            </td>
                                                                            <td align="left" valign="top" width="570">
                                                                                <div id="view_range_tab_map_canvas" style="width: 570px; height: 396px;">
                                                                                </div>
                                                                                <br />
                                                                                <div id="view_range_tab_map_canvas2" style="width: 570px; height: 396px;">
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <br />
                                                                    <asp:Literal ID="view_range_label" Text="" runat="server"></asp:Literal>
                                                                </div>
                                                                <script type="text/javascript" language="javascript">

                                                                    var pop_up_model_range = "";
                                                                    var pop_up_latitude = 0;
                                                                    var pop_up_longitude = 0;
                                                                    var pop_up_airport = "";
                                                                    var tab_range_map = null;

                                                                    // just sets a default map centered in us
                                                                    function initialize_range_map() {
                                                                        var mapOptions = {
                                                                            zoom: 4,
                                                                            center: new google.maps.LatLng(39.2323, -95.8887),
                                                                            mapTypeId: google.maps.MapTypeId.ROADMAP
                                                                        };

                                                                        //alert("show default range map");
                                                                        var map_RangeDiv = document.getElementById("view_range_tab_map_canvas");
                                                                        var map = new google.maps.Map(map_RangeDiv, mapOptions);
                                                                        if ((map != null) && (typeof (map) != "undefined")) {
                                                                            tab_range_map = map;
                                                                        }
                                                                    }

                                                                    //Building the tab Map 
                                                                    function build_range_tab_map(airport_location, latitude, longitude, modelRange) {

                                                                        if (Number(latitude) == 0 && Number(longitude) == 0 && Number(modelRange) == 0) { //not initalizing map, do not ignore this
                                                                            initialize_range_map();
                                                                            return false;
                                                                        }

                                                                        //alert("show new range tab map[lat:" + latitude + "][lng:" + longitude + "][ap:" + airport_location + "][mr:" + modelRange + "]");

                                                                        pop_up_airport = airport_location;
                                                                        pop_up_latitude = latitude;
                                                                        pop_up_longitude = longitude;
                                                                        pop_up_model_range = modelRange;

                                                                        var show_map = document.getElementById("btn_show_mapID");
                                                                        show_map.disabled = false;

                                                                        //Setting up the new options for the map.
                                                                        var mapOptions = {
                                                                            zoom: 2,
                                                                            center: new google.maps.LatLng(latitude, longitude),
                                                                            mapTypeId: google.maps.MapTypeId.ROADMAP
                                                                        };

                                                                        var map_RangeDiv = document.getElementById("view_range_tab_map_canvas");
                                                                        var map = new google.maps.Map(map_RangeDiv, mapOptions);

                                                                        //finding the map.    
                                                                        if ((map != null) && (typeof (map) != "undefined")) {

                                                                            tab_range_map = map;

                                                                            google.maps.event.addListenerOnce(map, 'idle', function () {
                                                                                google.maps.event.trigger(map, 'resize');
                                                                            });

                                                                            //drawing first established circle.
                                                                            Draw_Circle("ff0000", modelRange, latitude, longitude);

                                                                            //checking for other selections
                                                                            if (document.getElementById("<%= first_model.clientID %>").value != "") {

                                                                                //getting second model (well first dropdown) info
                                                                                var model_information1 = document.getElementById("<%= first_model.clientID %>").value.split("|");

                                                                                Draw_Circle("00CD00", model_information1[1], latitude, longitude);  //drawing second circle.

                                                                            } //1st model

                                                                            if (document.getElementById("<%= second_model.clientID %>").value != "") { //checking second dropdown.

                                                                                //getting third model (second dropdown) information
                                                                                var model_information2 = document.getElementById("<%= second_model.clientID %>").value.split("|");

                                                                                Draw_Circle("0276FD", model_information2[1], latitude, longitude);  //drawing third circle.

                                                                            } //2nd model

                                                                            // have to add marker AFTER circles are drawn or google map script bombs out
                                                                            add_range_tab_marker(airport_location, latitude, longitude);

                                                                        }



                                                                        var map_RangeDiv = document.getElementById("view_range_tab_map_canvas2");
                                                                        // map_RangeDiv.innerHTML = "<img src='https://maps.googleapis.com/maps/api/staticmap?center=" + latitude + "," + longitude + "&zoom=3&size=640x400&style=element:labels|visibility:off&style=element:geometry.stroke|visibility:off&style=feature:landscape|element:geometry|saturation:-100&style=feature:water|saturation:-100|invert_lightness:true&key=AIzaSyA4rAT0fdTZLNkJ5o0uaAwZ89vVPQpr_Kc'>";
                                                                        map_RangeDiv.innerHTML = "<img src='" + encodeURI("https://maps.googleapis.com/maps/api/staticmap?center=" + latitude + "," + longitude + "&zoom=2&size=640x400&markers=color:red|label:A|" + latitude + "," + longitude + "&style=feature:landscape|element:geometry|true&key=AIzaSyA4rAT0fdTZLNkJ5o0uaAwZ89vVPQpr_Kc") + "'>";
                                                                    }

                                                                    //Showing the big map Popup
                                                                    function showPopup() {
                                                                        $find("<%= MPE.clientID %>").show();

                                                                        if (Number(pop_up_latitude) == 0 && Number(pop_up_longitude) == 0 && Number(model_range) == 0) { //not initalizing map, do not ignore this
                                                                            initialize_range_map();
                                                                            return false;
                                                                        }

                                                                        //alert("show default popup range map");

                                                                        //Setting up the new options for the big map.
                                                                        var mapOptions = {
                                                                            zoom: 2,
                                                                            center: new google.maps.LatLng(pop_up_latitude, pop_up_longitude),
                                                                            mapTypeId: google.maps.MapTypeId.ROADMAP
                                                                        };

                                                                        var mapPopUpDiv = document.getElementById("div_large_mapID");
                                                                        var map = new google.maps.Map(mapPopUpDiv, mapOptions);

                                                                        //finding the map.    
                                                                        if ((map != null) && (typeof (map) != "undefined")) {

                                                                            tab_range_map = map;

                                                                            google.maps.event.addListenerOnce(map, 'idle', function () {
                                                                                google.maps.event.trigger(map, 'resize');
                                                                            });

                                                                            //drawing first established circle.
                                                                            Draw_Circle("ff0000", pop_up_model_range, pop_up_latitude, pop_up_longitude);

                                                                            //checking for other selections
                                                                            if (document.getElementById("<%= first_model.clientID %>").value != "") {
                                                                                //getting second model (well first dropdown) info
                                                                                var model_information1 = document.getElementById("<%= first_model.clientID %>").value.split("|");
                                                                                Draw_Circle("00CD00", model_information1[1], pop_up_latitude, pop_up_longitude);  //drawing second circle.
                                                                            } //1st model

                                                                            if (document.getElementById("<%= second_model.clientID %>").value != "") { //checking second dropdown.
                                                                                //getting third model (second dropdown) information
                                                                                var model_information2 = document.getElementById("<%= second_model.clientID %>").value.split("|");
                                                                                Draw_Circle("0276FD", model_information2[1], pop_up_latitude, pop_up_longitude);  //drawing third circle.
                                                                            } //2nd model

                                                                            // have to add marker AFTER circles are drawn or google map script bombs out
                                                                            add_range_tab_marker(pop_up_airport, pop_up_latitude, pop_up_longitude);

                                                                        }
                                                                    }

                                                                    //function to draw a circle.
                                                                    function Draw_Circle(color, radius_range, latitude, longitude) {

                                                                        radiusRange = Number(radius_range);

                                                                        //checking for valid map object.
                                                                        if ((tab_range_map != null) && (typeof (tab_range_map) != "undefined")) {

                                                                            point_map = new google.maps.LatLng(latitude, longitude); //set point to use for circle.

                                                                            var populationOptions = { //setting up circle options.
                                                                                strokeColor: "#" + color, //color
                                                                                strokeOpacity: 0.8,       //line opacity
                                                                                strokeWeight: 2,
                                                                                fillOpacity: 0.0,         //filled circle?
                                                                                map: tab_range_map,             //map variable?
                                                                                center: point_map, //center of circle, meaning the airport it's based around.
                                                                                radius: radiusRange //radius of circle
                                                                            };

                                                                            new google.maps.Circle(populationOptions); //creating google maps circle.

                                                                        }
                                                                    }

                                                                    function add_range_tab_listener(marker, title, map) { //adding listener on click event. Basically adds a popup window with predetermined text on click event of marker.

                                                                        var contentString = '<div id="content"><div id="siteNotice"></div>' +
                                                                            '<h1 id="firstHeading" class="firstHeading">' + title + '</h1>' +
                                                                            '<div id="bodyContent"></div></div>';

                                                                        var infowindow = new google.maps.InfoWindow({ content: contentString });

                                                                        //Then go ahead and add the listener marker to the map.
                                                                        google.maps.event.addListener(marker, 'click', function () {
                                                                            infowindow.open(map, marker);
                                                                        });
                                                                    }

                                                                    function add_range_tab_marker(location_title, latitude, longitude) { //adding a new marker to the map ... Basically adds a popup window with predetermined text on click event of marker.
                                                                        //creating the marker for the map based on latitude, longitude

                                                                        //alert("add marker to range tab map");

                                                                        //finding the map.
                                                                        if ((tab_range_map != null) && (typeof (tab_range_map) != "undefined")) {

                                                                            var icon = {
                                                                                url: '../images/evoPlane.png'
                                                                            };
                                                                            var marker = new google.maps.Marker({
                                                                                position: new google.maps.LatLng(latitude, longitude),
                                                                                map: tab_range_map,
                                                                                icon: icon,
                                                                                title: location_title
                                                                            });

                                                                            google.maps.event.clearListeners(marker, 'onclick');

                                                                            add_range_tab_listener(marker, location_title, tab_range_map);

                                                                        }
                                                                    }

                                                                </script>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="fractional_tab" runat="server">
                                                            <ContentTemplate>
                                                                <div class="tab_container_div">
                                                                    <asp:Literal ID="view_fractional_label" Text="" runat="server"></asp:Literal><asp:Chart
                                                                        ID="SOLD_FROM_PROVIDER" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
                                                                        Visible="False">
                                                                        <Series>
                                                                            <asp:Series>
                                                                            </asp:Series>
                                                                        </Series>
                                                                        <ChartAreas>
                                                                            <asp:ChartArea Name="ChartArea1">
                                                                            </asp:ChartArea>
                                                                        </ChartAreas>
                                                                    </asp:Chart>
                                                                    <asp:Chart ID="SOLD_TO_PROVIDER" runat="server" ImageStorageMode="UseImageLocation"
                                                                        ImageType="Jpeg" Visible="False">
                                                                        <Series>
                                                                            <asp:Series>
                                                                            </asp:Series>
                                                                        </Series>
                                                                        <ChartAreas>
                                                                            <asp:ChartArea Name="ChartArea1">
                                                                            </asp:ChartArea>
                                                                        </ChartAreas>
                                                                    </asp:Chart>
                                                                </div>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="utilization_tab" runat="server" HeaderText="Utilization">
                                                            <ContentTemplate>
                                                                <asp:Panel runat="server" ID="flightActivityContainer" Visible="false">
                                                                <table width="100%" cellpadding="0" cellspacing="0">

                                                                    <tr>
                                                                        <td valign="top" align="left" width="35%">
                                                                            <asp:Label ID="flightActivityTable" runat="server"></asp:Label>
                                                                        </td>
                                                                        <td width="5%">&nbsp;</td>
                                                                        <td valign="top" align="left" width="60%">
                                                                            <asp:UpdatePanel ID="utilizationGraphUpdate" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" >
                                                                                <ContentTemplate><asp:panel runat="server" id="utilizationGraphControls" Visible="false">
                                                                                    Calendar Years:
                                                                                    <asp:DropDownList runat="server" ID="FlightUtilizationGraphDropdown" AutoPostBack="true">
                                                                                        <asp:ListItem Value="2" Selected="true">2 Years</asp:ListItem>
                                                                                        <asp:ListItem Value="3">3 Years</asp:ListItem>
                                                                                        <asp:ListItem Value="4">4 Years</asp:ListItem>
                                                                                        <asp:ListItem Value="5">5 Years</asp:ListItem>
                                                                                    </asp:DropDownList><br /></asp:panel>
                                                                                    <asp:Label runat="server" ID="utilization_graph"></asp:Label>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                </asp:Panel>
                                                                <asp:Label runat="server" ID="utilization_label"></asp:Label>

                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="estimates_tab" runat="server" HeaderText="Estimates" Visible="false">
                                                            <ContentTemplate>
                                                                <asp:Literal runat="server" ID="estimatesMfrYearGraph"></asp:Literal><asp:Literal runat="server" ID="estimatesMonthGraph"></asp:Literal><asp:Literal runat="server" ID="estimatesAFTTGraph"></asp:Literal><asp:Literal runat="server" ID="estimatesResidualGraph"></asp:Literal><asp:Label runat="server" ID="value_estimates_label"></asp:Label>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="utilization_summaries_tab" runat="server" Visible="false" HeaderText="Refuel/Tech Stops">
                                                            <ContentTemplate>
                                                                <asp:UpdatePanel ID="reports_update" runat="server">
                                                                    <ContentTemplate>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                <div class="tab_container_div">
                                                                    Report:
                                                                    <asp:DropDownList runat="server" ID="utilization_summary_type" AutoPostBack="true">
                                                                        <asp:ListItem Selected="true" Value="refuel">Refuel/Tech Stops</asp:ListItem>
                                                                    </asp:DropDownList><asp:Label runat="server" ID="techstop_label" runat="server" Visible="false" Text="Based on any leg that uses more than "></asp:Label><asp:DropDownList ID="percentage_drop" runat="server" Visible="false" AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                    <asp:Label runat="server" ID="techstop_label2" runat="server" Visible="false" Text="% of NBAA max range, stops and is on the ground for "></asp:Label><asp:DropDownList ID="minutes_drop" runat="server" Visible="false" AutoPostBack="true">
                                                                        <asp:ListItem Text="30" Value="30">30</asp:ListItem>
                                                                        <asp:ListItem Text="60" Value="60">60</asp:ListItem>
                                                                        <asp:ListItem Text="90" Value="90" Selected="True">90</asp:ListItem>
                                                                        <asp:ListItem Text="120" Value="120">120</asp:ListItem>
                                                                        <asp:ListItem Text="240" Value="240">240</asp:ListItem>
                                                                    </asp:DropDownList><asp:Label runat="server" ID="techstop_label3" runat="server" Visible="false" Text=" minutes or less, and then takes another flight."></asp:Label><div class="clearfix">
                                                                    </div>
                                                                    <br />
                                                                    <asp:Label runat="server" ID="utilization_summaries_label"></asp:Label><div style="width: 960px">
                                                                        <div class="specialTableContainer">
                                                                            <table id="refuelSummary" class="refreshable display_none operatorData">
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="clearfix">
                                                                </div>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="utilization_Aircraft_Model" runat="server" HeaderText="Aircraft Model"
                                                            Visible="false">
                                                            <ContentTemplate>
                                                                <div class="specialTableContainer">
                                                                    <table id="modelData" class="refreshable display_none">
                                                                        <tfoot>
                                                                            <tr>
                                                                                <th></th>
                                                                                <th colspan="1" style="text-align: right"></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                                <th></th>
                                                                            </tr>
                                                                        </tfoot>
                                                                    </table>
                                                                </div>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="Nearby_Airports_18_Panel" runat="server" HeaderText="Nearby Airports"
                                                            Visible="false">
                                                            <ContentTemplate>
                                                                Change Range Miles from Currently Selected Airport to
                                                                <asp:DropDownList ID="distance_drop" runat="server" AutoPostBack="true">
                                                                    <asp:ListItem Text="25" Value="25">25</asp:ListItem>
                                                                    <asp:ListItem Text="50" Value="50">50</asp:ListItem>
                                                                    <asp:ListItem Text="75" Value="75">75</asp:ListItem>
                                                                    <asp:ListItem Text="100" Value="100">100</asp:ListItem>
                                                                    <asp:ListItem Text="150" Value="150" Selected="True">150</asp:ListItem>
                                                                    <asp:ListItem Text="200" Value="200">200</asp:ListItem>
                                                                </asp:DropDownList><br />
                                                                <asp:Label runat="server" ID="nearby_airports_label" Width="100%"></asp:Label>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="panel_19" runat="server" HeaderText="Company Directory" Visible="false">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ac_projects_ddl2_19" runat="server" Visible="false" AutoPostBack="false"
                                                                    onchange='searchThisForm();'>
                                                                </asp:DropDownList>
                                                                <asp:Label ID="compare_view_sold_label2_19" runat="server" Visible="false"></asp:Label><asp:DropDownList
                                                                    ID="company_range_19" runat="server" Visible="false" AutoPostBack="false" onchange='searchThisForm();'>
                                                                    <asp:ListItem Value="25">25</asp:ListItem>
                                                                    <asp:ListItem Value="50">50</asp:ListItem>
                                                                </asp:DropDownList><asp:Label ID="compare_view_sold_label_19" runat="server" Visible="false"></asp:Label><asp:Label runat="server" ID="panel_19_label" Width="100%"></asp:Label>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="panel_20" runat="server" HeaderText="Owners" Visible="false">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="owner_drop_19" runat="server" Visible="false" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                                <asp:CheckBox ID="check_owner_ac_19" Text="Display Aircraft for Owners" runat="server"
                                                                    Visible="false" AutoPostBack="true" />
                                                                <asp:Label runat="server" ID="panel_20_label" Width="100%"></asp:Label>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="reports_tab_panel" runat="server" Visible="false" HeaderText="Reports">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="reports_drop" AutoPostBack="true">
                                                                    <asp:ListItem Selected="true" Value="manufacturer">Manufacturer Flight Summary</asp:ListItem>
                                                                </asp:DropDownList><asp:Label ID="reports_label" runat="server" Text=""></asp:Label>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="flight_summary_utilization_tab" runat="server" Visible="false"
                                                            HeaderText="Summary">
                                                            <ContentTemplate>
                                                                <asp:Panel runat="server" ID="utilizationViewMainBox" Visible="False" CssClass="columns nine"
                                                                    Style="overflow: hidden">
                                                                    <div class="Box">
                                                                        <div class="row">
                                                                            <asp:Label runat="server" ID="utilizationViewMainBoxLabel" CssClass="columns five float_right">SUMMARY</asp:Label><asp:Panel runat="server" ID="utilizationDropDownContainer">
                                                                                <asp:Label ID="UtilizationViewLeftMainBoxDropdownText" runat="server" Text=""></asp:Label><asp:DropDownList
                                                                                    ID="utilizationViewMainBoxDropdown" runat="server" AutoPostBack="True" Width="155px">
                                                                                    <asp:ListItem Value="Month">Month</asp:ListItem>
                                                                                    <asp:ListItem Value="Type">Aircraft Type</asp:ListItem>
                                                                                    <asp:ListItem Value="BWeight">Business Jets by Weight Class</asp:ListItem>
                                                                                    <asp:ListItem Value="TWeight">TurboProps by Weight Class</asp:ListItem>
                                                                                    <asp:ListItem Value="HWeight">Helicopters by Weight Class</asp:ListItem>
                                                                                    <asp:ListItem Value="Hours">Hours By Month</asp:ListItem>
                                                                                </asp:DropDownList><asp:Label runat="server" ID="UtilizationViewRightMainBoxDropdownText"></asp:Label>
                                                                            </asp:Panel>
                                                                            <style>
                                                                                #utilizationViewGraphall div:first-child {
                                                                                    margin-left: 0px;
                                                                                }
                                                                            </style>
                                                                            <div id="utilizationViewGraphall" style="overflow: hidden;">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                    </cc1:TabContainer>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="loaded_visibility1" runat="server" CssClass="display_none" Width="100%"
                HorizontalAlign="Center" ChildrenAsTriggers="True">
                <table cellpadding="0" cellspacing="0" align="center" width="100%">
                    <tr>
                        <td valign="top" align="center" class="hideMaxHeight">
                            <asp:Panel ID="parent_toggle2" runat="server">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td align="left" valign="top" class="dark_header">
                                            <table width="100%" cellpadding="3" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="middle" width="12%">
                                                        <asp:Panel ID="Control_Panel1" runat="server">
                                                            <asp:Image ID="ControlImage1" runat="server" ImageUrl="../images/search_expand.jpg" />
                                                        </asp:Panel>
                                                    </td>
                                                    <td align="left" valign="bottom" style="padding-bottom: 10px;" width='460'>
                                                        <asp:Label ID="breadcrumbs1" runat="server" CssClass="float_left criteria_text"></asp:Label></td>
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
                                                    <td align="left" valign="bottom" style="padding-bottom: 10px;" width="310">
                                                        <asp:Label ID="buttons1" runat="server" CssClass="float_right criteria_text"></asp:Label></td>
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
                                        <asp:Table ID="Table1" runat="server" Width="100%" CellPadding="3" CellSpacing="0"
                                            BorderWidth="1" BorderColor="Black" BorderStyle="Solid">
                                            <asp:TableRow>
                                                <asp:TableCell ID="cellTypeMakeModelView" HorizontalAlign="left" VerticalAlign="top"
                                                    Width="50%" ColumnSpan="2">
                                                    <asp:Panel ID="Panel3" runat="server">
                                                        <evo:viewTMMDropDowns_ViewSpecific ID="ViewTMMDropDowns1" runat="server" />

                                                        <script language="javascript" type="text/javascript">
                              refreshTypeMakeModelByCheckBox("", "", <%= isHeliOnlyProduct.tostring.tolower%>,<%= productCodeCount.tostring%>);
                                                        </script>
                                                    </asp:Panel>
                                                </asp:TableCell><asp:TableCell ID="cellTimeSpanView" HorizontalAlign="left" VerticalAlign="middle">
                                                    Time&nbsp;Span&nbsp;for&nbsp;View&nbsp;:<br />
                                                    <asp:DropDownList ID="selectMarketTimeSpan" runat="server" OnClientClick='' ToolTip="Select View Time Span">
                                                        <asp:ListItem Value="3">3 Months</asp:ListItem>
                                                        <asp:ListItem Value="6">6 Months</asp:ListItem>
                                                        <asp:ListItem Value="9">9 Months</asp:ListItem>
                                                        <asp:ListItem Value="12">1 Year</asp:ListItem>
                                                        <asp:ListItem Value="24">2 Years</asp:ListItem>
                                                        <asp:ListItem Value="36">3 Years</asp:ListItem>
                                                        <asp:ListItem Value="48">4 Years</asp:ListItem>
                                                        <asp:ListItem Value="60">5 Years</asp:ListItem>
                                                    </asp:DropDownList>
                                                </asp:TableCell><asp:TableCell ID="celldeliveryLeasesView" HorizontalAlign="left" VerticalAlign="middle" Visible="false">
                                                    <asp:CheckBox ID="deliveryViewIncludeLeases" Text="Include Leases" runat="server"
                                                        AutoPostBack="true" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell ID="cellSPIWeightClass" HorizontalAlign="left" VerticalAlign="middle"
                                                    Visible="false">
                                                    Weight&nbsp;Class&nbsp;:<br />
                                                    <asp:DropDownList ID="salesPriceViewWeightClassID" runat="server" ToolTip="Select Weight Class">
                                                        <asp:ListItem Value="0">All</asp:ListItem>
                                                        <asp:ListItem Value="H">Heavy</asp:ListItem>
                                                        <asp:ListItem Value="M">Medium</asp:ListItem>
                                                        <asp:ListItem Value="H,M">Heavy and Medium</asp:ListItem>
                                                        <asp:ListItem Value="L">Light</asp:ListItem>
                                                        <asp:ListItem Value="V">Very Light Jet</asp:ListItem>
                                                        <asp:ListItem Value="L,V">Light and Very Light Jet</asp:ListItem>
                                                    </asp:DropDownList>
                                                </asp:TableCell><asp:TableCell ID="cellLocationViewType" HorizontalAlign="left" VerticalAlign="middle"
                                                    Visible="false">
                                                    Location&nbsp;Sort&nbsp;By&nbsp;:<br />
                                                    <asp:DropDownList ID="locationViewTypeID" runat="server" ToolTip="Select Aircraft Location">
                                                        <asp:ListItem Value="0">Aircraft Base</asp:ListItem>
                                                        <asp:ListItem Value="1">Owners</asp:ListItem>
                                                        <asp:ListItem Value="2">Operators</asp:ListItem>
                                                    </asp:DropDownList>
                                                </asp:TableCell><asp:TableCell ID="cellExportToExcel" HorizontalAlign="left" VerticalAlign="middle"
                                                    Visible="false">
                                                    <asp:Button runat="server" ID='btnToExcelID' Text='Export To Excel' OnClientClick=''
                                                        ToolTip='Click to export Document Records to Excel' />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell ColumnSpan="4" ID="cellFractionalControls" HorizontalAlign="left"
                                                    VerticalAlign="middle" Visible="false">
                                                    <asp:Table ID="fractionalControlsTable" CellPadding="2" CellSpacing="0" runat="server">
                                                        <asp:TableRow>
                                                            <asp:TableCell ID="cellFractionalProgram" HorizontalAlign="left" VerticalAlign="top">
                                                                Fractional&nbsp;Programs&nbsp;:<br />
                                                                <asp:ListBox ID="lbFractionalProgram" runat="server" Rows="5" SelectionMode="Single"
                                                                    ToolTip="Available Fractional Programs">
                                                                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                                                                </asp:ListBox>
                                                            </asp:TableCell>
                                                            <asp:TableCell ID="cellFractionalModels" HorizontalAlign="left" VerticalAlign="top">
                                                                Fractional&nbsp;Models&nbsp;:<br />
                                                                <asp:ListBox ID="lbFractionalModel" runat="server" Rows="5" SelectionMode="Single"
                                                                    ToolTip="Models in a Fractional Program">
                                                                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                                                                </asp:ListBox>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell ColumnSpan="2" ID="cellSPYearSold" HorizontalAlign="left" VerticalAlign="middle"
                                                    Visible="false">
                                                    <asp:Table ID="spi_year_sold" Width="100%" CellPadding="3" CellSpacing="0" runat="server">
                                                        <asp:TableRow>
                                                            <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="33%">
                                                                Year&nbsp;Sold&nbsp;:<br />
                                                                <asp:ListBox ID="YearSld1ID" runat="server" Width="100%" Rows="2" AutoPostBack="true"
                                                                    Font-Size="10px" SelectionMode="Single" ToolTip="From Year Sold">
                                                                    <asp:ListItem Selected="True">All</asp:ListItem>
                                                                </asp:ListBox>
                                                            </asp:TableCell>
                                                            <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="33%">
                                                                To&nbsp;Year&nbsp;Sold&nbsp;:<br />
                                                                <asp:ListBox ID="YearSld2ID" runat="server" Width="100%" Rows="2" AutoPostBack="true"
                                                                    Font-Size="10px" SelectionMode="Single" ToolTip="To Year Sold">
                                                                    <asp:ListItem Selected="True">All</asp:ListItem>
                                                                </asp:ListBox>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </asp:TableCell><asp:TableCell ColumnSpan="2" ID="cellSPYearQtrSold" HorizontalAlign="left" VerticalAlign="middle"
                                                    Visible="false">
                                                    <asp:Table ID="spi_year_quarter_sold" Width="100%" CellPadding="3" CellSpacing="0"
                                                        runat="server">
                                                        <asp:TableRow>
                                                            <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="33%">
                                                                Year&nbsp;Sold&nbsp;:<br />
                                                                <asp:ListBox ID="YearQtrSld1ID" runat="server" Width="100%" Rows="2" AutoPostBack="true"
                                                                    Font-Size="10px" SelectionMode="Single" ToolTip="From Quarter Sold">
                                                                    <asp:ListItem Selected="True">All</asp:ListItem>
                                                                </asp:ListBox>
                                                            </asp:TableCell>
                                                            <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="33%">
                                                                To&nbsp;Year&nbsp;Sold&nbsp;:<br />
                                                                <asp:ListBox ID="YearQtr1ID" runat="server" Width="100%" Rows="2" AutoPostBack="true"
                                                                    Font-Size="10px" SelectionMode="Single" ToolTip="To Quarter Sold">
                                                                    <asp:ListItem Selected="True">All</asp:ListItem>
                                                                </asp:ListBox>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell ID="cellFinancialInstitutions" HorizontalAlign="left" VerticalAlign="top">
                                                    Financial&nbsp;Institutions&nbsp;:<br />
                                                    <asp:ListBox ID="lbFinancialInstitutions" runat="server" Rows="5" SelectionMode="Single"
                                                        ToolTip="Available Financial Institutions">
                                                        <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                                    </asp:ListBox>
                                                </asp:TableCell><asp:TableCell ID="cellFinancialStartDate" HorizontalAlign="left" VerticalAlign="top">
                                                    Start&nbsp;Date&nbsp;:<br />
                                                    <asp:TextBox ID="viewFinancialStartDate" runat="server" Columns="20" Text="" AutoPostBack="false"></asp:TextBox>
                                                </asp:TableCell><asp:TableCell ID="cellFinancialEndDate" HorizontalAlign="left" VerticalAlign="top">
                                                    End&nbsp;Date&nbsp;:<br />
                                                    <asp:TextBox ID="viewFinancialEndDate" runat="server" Columns="20" Text="" AutoPostBack="false"></asp:TextBox>
                                                </asp:TableCell><asp:TableCell ID="cellInternalTransactions" HorizontalAlign="left" VerticalAlign="top">
                                                    <asp:CheckBox ID="chk_internal_transactions" runat="server" Text="Include Internal Transactions"
                                                        AutoPostBack="false" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell ID="cellRegionDropdowns" HorizontalAlign="left" VerticalAlign="top"
                                                    ColumnSpan="4">
                                                    Borrower&nbsp;Company&nbsp;Location&nbsp;Selections&nbsp;:<br />
                                                    <asp:Panel ID="Panel4" runat="server">
                                                        <evo:viewCCSTDropDowns_ViewSpecific ID="viewCCSTDropDowns" runat="server" />
                                                        <!-- country/region dropdowns -->

                                                        <script language="javascript" type="text/javascript">
                                                            checkRadioButtons(bIsBaseView, bIsViewView, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);
                                                        </script>
                                                    </asp:Panel>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell ID="cellBaseDropdowns" HorizontalAlign="left" VerticalAlign="top"
                                                    ColumnSpan="2">
                                                    Aircraft&nbsp;Location&nbsp;Selections&nbsp;:<br />
                                                    <asp:Panel ID="Panel5" runat="server">
                                                        <evo:viewCCSTDropDowns_ViewSpecific ID="viewCCSTDropDowns1" runat="server" />
                                                        <!-- base dropdowns -->

                                                        <script language="javascript" type="text/javascript">
                                                            checkRadioButtons(bIsBaseBase, bIsViewBase, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);
                                                        </script>
                                                    </asp:Panel>
                                                </asp:TableCell><asp:TableCell ID="cellTransType" HorizontalAlign="left" VerticalAlign="top">
                                                    Transaction&nbsp;Type&nbsp;:<br />
                                                    <asp:DropDownList ID="viewFinancialTxsddl" runat="server" ToolTip="Select Transaction Type">
                                                        <asp:ListItem Value=""></asp:ListItem>
                                                        <asp:ListItem Value="WS">Full Sales</asp:ListItem>
                                                        <asp:ListItem Value="SS">Share Sales</asp:ListItem>
                                                        <asp:ListItem Value="LA, LO, LT">Leases</asp:ListItem>
                                                    </asp:DropDownList>
                                                </asp:TableCell><asp:TableCell ID="cellDocType" HorizontalAlign="left" VerticalAlign="top">
                                                    Transaction&nbsp;Document&nbsp;Type&nbsp;:<br />
                                                    <asp:ListBox ID="lbFinancialDocType" runat="server" Rows="5" SelectionMode="Multiple"
                                                        ToolTip="Select Document Type">
                                                        <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                                    </asp:ListBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell ID="cellModelIDA" HorizontalAlign="left" VerticalAlign="top" Width="33%">
                                                    <asp:DropDownList ID="compare_amod_id_a" runat="server" ToolTip="Select First Aircraft Model">
                                                    </asp:DropDownList>
                                                </asp:TableCell><asp:TableCell ID="cellModelIDB" HorizontalAlign="left" VerticalAlign="top" Width="33%">
                                                    <asp:DropDownList ID="compare_amod_id_b" runat="server" ToolTip="Select Second Aircraft Model">
                                                    </asp:DropDownList>
                                                </asp:TableCell><asp:TableCell ID="cellModelIDC" HorizontalAlign="left" VerticalAlign="top" Width="33%">
                                                    <asp:DropDownList ID="compare_amod_id_c" runat="server" ToolTip="Select Third Aircraft Model">
                                                    </asp:DropDownList>
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
                                                                AC Search:<br />
                                                                <asp:Table runat="server" ID="notesSearch_ac_details_table" Width="100%" CellPadding="1"
                                                                    CellSpacing="0">
                                                                    <asp:TableRow ID="TableRow3">
                                                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="middle" Width="25%">
                                                                            Search Field:&nbsp;
                                      <asp:DropDownList ID="notesSearch_ac_search_field" runat="server" Enabled="true"
                                          Visible="true">
                                          <asp:ListItem Text="" Value="0"></asp:ListItem>
                                          <asp:ListItem Text="Ser#/Reg#" Value="1"></asp:ListItem>
                                          <asp:ListItem Text="Ser#" Value="2"></asp:ListItem>
                                          <asp:ListItem Text="Reg#" Value="4"></asp:ListItem>
                                          <asp:ListItem Text="Aircraft ID" Value="8"></asp:ListItem>
                                      </asp:DropDownList>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="middle" Width="25%">
                                                                            Select How:&nbsp;
                                      <asp:DropDownList ID="notesSearch_ac_search_field_operator" runat="server">
                                          <asp:ListItem Value="1">Begins With</asp:ListItem>
                                          <asp:ListItem Value="2">Anywhere</asp:ListItem>
                                          <asp:ListItem Value="4">Equals</asp:ListItem>
                                      </asp:DropDownList>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="middle">
                                                                            Search For:&nbsp;
                                      <asp:TextBox ID="notesSearch_ac_search_field_text" runat="server" Width="70%"></asp:TextBox>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                </asp:Table>
                                                            </asp:TableCell>
                                                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="middle" ID="action_sort_col_two"
                                                                Wrap="false">
                                                                <asp:Label ID="Label6" runat="server" Text="Start/End:"></asp:Label>&nbsp;
                                <asp:TextBox ID="notesSearch_date" runat="server" Visible="true" Width="140px"></asp:TextBox>&nbsp;
                                <img src="../images/final.jpg" id="cal_image4" alt="&ldquo;mm/dd/yyyy&rdquo;, for Between Use &ldquo;mm/dd/yyyy:mm/dd/yyyy&rdquo;"
                                    title="&ldquo;mm/dd/yyyy&rdquo;, for Between Use &ldquo;mm/dd/yyyy:mm/dd/yyyy&rdquo;" />&nbsp
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell ID="cellUserPortfolio" HorizontalAlign="Left" VerticalAlign="top"
                                                    ColumnSpan="4">
                                                    <asp:Label ID="user_portfolio_lbl" runat="server" Text="Saved Folder List"></asp:Label>&nbsp;
                          <asp:DropDownList ID="user_portfolio_list" runat="server">
                          </asp:DropDownList>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="right" VerticalAlign="top" ColumnSpan="4">
                                                    <asp:Button runat="server" ID="atGlanceGo" Text="Search" ToolTip='Click to Apply Critera'
                                                        UseSubmitBehavior="false" />
                                                    <asp:Button runat="server" ID="atGlanceClear" Text="Clear Selections" ToolTip="Click to Clear Critera"
                                                        UseSubmitBehavior="false" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:Panel>
                                </div>
                                <br />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr class="maxHeight">
                        <td valign="top" align="center" style="width: 100%;">
                            <asp:Panel ID="viewOuterPanel" runat="server" Visible="true" HorizontalAlign="Left"
                                Width="100%">
                                <div id="location_view" runat="server" visible="False" class="valueSpec aircraftListing Simplistic aircraftSpec">
                                    <table width='100%' cellspacing='0' cellpadding='0'>
                                        <tr valign='top'>
                                            <td align="center" valign="top" width='70%' valign='top' style="width: 70%;">
                                                <div id="view_location_map_canvas" style="width: 100%; height: 426px;">
                                                </div>
                                            </td>
                                            <td align="left" valign="top" width='30%' style="width: 30%;" class="noHeight">
                                                <asp:Label ID="view_location_label_display_by" runat="server" Text="&nbsp;Sort&nbsp;By&nbsp;:&nbsp;"
                                                    Visible="False"></asp:Label><asp:DropDownList runat="server" ID="view_location_display_by" ToolTip="Location by AC BASE or OWNER or OPERATOR"
                                                        AutoPostBack="True" Visible="False">
                                                    </asp:DropDownList>
                                                <asp:DropDownList ID="view_location_clear_list" runat="server" AutoPostBack="True"
                                                    ToolTip="Clear Location" Visible="False">
                                                </asp:DropDownList>
                                                <br />
                                                <asp:Literal ID="view_location_label" runat="server"></asp:Literal></td>
                                        </tr>
                                    </table>
                                    <script type="text/javascript" language="javascript">

                                        var view_mapDiv = document.getElementById("view_location_map_canvas");
                                        var view_map = null;

                                        function initialize_view_map() {
                                            var mapOptions = {
                                                zoom: 2,
                                                center: new google.maps.LatLng(0, 0),
                                                mapTypeId: google.maps.MapTypeId.HYBRID
                                            };

                                            //alert("show default location view map");

                                            var map = new google.maps.Map(view_mapDiv, mapOptions);

                                            if ((map != null) && (typeof (map) != "undefined")) {
                                                view_map = map;
                                            }

                                        }

                                        function center_location_view_map(latitude, longitude, zoom_level) {

                                            if (Number(latitude) == 0 && Number(longitude) == 0 && Number(zoom_level) == 0) { //not initalizing map, do not ignore this
                                                initialize_view_map();
                                                return false;
                                            }

                                            //Setting up the new options for the map.
                                            var mapOptions = {
                                                zoom: zoom_level,
                                                center: new google.maps.LatLng(latitude, longitude),
                                                mapTypeId: google.maps.MapTypeId.HYBRID
                                            };

                                            //alert("show location map[lat:" + latitude + "][lng:" + longitude + "][zl:" + zoom_level + "]");

                                            var map = new google.maps.Map(view_mapDiv, mapOptions);

                                            //finding the map.    
                                            if ((map != null) && (typeof (map) != "undefined")) {

                                                view_map = map;
                                                google.maps.event.addListenerOnce(map, 'idle', function () {
                                                    google.maps.event.trigger(map, 'resize');
                                                });
                                            }
                                        }

                                        function add_location_view_listener(marker, title, counter, link, map) { //adding listener on click event. Basically adds a popup window with predetermined text on click event of marker.

                                            var contentString = "";

                                            if (Number(counter) > 0) {

                                                contentString = '<div id="content"><div id="siteNotice"></div>' +
                                                    '<h1 id="firstHeading" class="firstHeading">' + title + '</h1>' +
                                                    '<div id="bodyContent"><p><b>Number of aircraft at this location is ' + counter + '</b></p>' +
                                                    '<p><a href="' + link + '" title="' + link + '">Click to view aircraft at this location</a></p></div></div>';
                                            }
                                            else {
                                                if (Number(counter) == -1) {
                                                    contentString = '<div id="content"><div id="siteNotice"></div>' +
                                                        '<h1 id="firstHeading" class="firstHeading">' + title + '</h1>' +
                                                        '<div id="bodyContent"><p><a href="' + link + '" title="' + link + '">Click to view aircraft at this location</a></p></div></div>';
                                                }
                                                else {
                                                    contentString = '<div id="content"><div id="siteNotice"></div>' +
                                                        '<h1 id="firstHeading" class="firstHeading">' + title + '</h1>' +
                                                        '<div id="bodyContent"></div></div>';

                                                }
                                            }

                                            var infowindow = new google.maps.InfoWindow({ content: contentString });

                                            //Then go ahead and add the listener marker to the map.
                                            google.maps.event.addListener(marker, 'click', function () {
                                                infowindow.open(map, marker);
                                            });
                                        }


                                        function add_location_view_marker(location_title, latitude, longitude, counter, link, map) { //adding a new marker to the map ... Basically adds a popup window with predetermined text on click event of marker.
                                            //alert("add marker too location view map");

                                            //finding the map.    
                                            if ((map != null) && (typeof (map) != "undefined")) {

                                                var icon = {
                                                    url: '../images/evoPlane.png'
                                                };

                                                //creating the marker for the map based on latitude, longitude
                                                var marker = new google.maps.Marker({
                                                    position: new google.maps.LatLng(latitude, longitude),
                                                    map: map,
                                                    icon: icon,
                                                    title: location_title
                                                });

                                                google.maps.event.clearListeners(marker, 'onclick');
                                                add_location_view_listener(marker, location_title, counter, link, map);

                                            }
                                        }

                                    </script>
                                </div>
                                <cc1:TabContainer ID="viewTabContainer" runat="server" CssClass="dark-theme" Width="100%">
                                    <cc1:TabPanel ID="viewTabPanel" runat="server">
                                        <HeaderTemplate>
                                            <asp:Label ID="viewHeaderLabel" runat="server"></asp:Label>
                                        </HeaderTemplate>
                                        <ContentTemplate>


                                            <asp:Chart ID="DOCS_BY_MONTH_GRAPH" runat="server" ImageStorageMode="UseImageLocation"
                                                ImageType="Jpeg" Visible="False">
                                                <Series>
                                                    <asp:Series Name="Series1" ChartArea="ChartArea1"></asp:Series>
                                                </Series>
                                                <ChartAreas>
                                                    <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                                                </ChartAreas>
                                            </asp:Chart>


                                            <table cellpadding='0' cellspacing='0' border='0' width="100%">
                                                <tr valign='top'>
                                                    <td valign='top'>
                                                        <asp:Label ID="viewContentLabel" runat="server"></asp:Label><asp:Panel ID="google_sold_trends_graphs1" runat="server" Visible="False">
                                                            <br />
                                                            <table cellpadding="0" cellspacing="0" border='0' valign='top' width="100%">
                                                                <tr valign='top'>
                                                                    <td>
                                                                        <table cellpadding='0' cellspacing='0' border='0'>
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <b>Avg Asking vs Selling Price By Year Mfr($k)</b>
                                                                                    <br />
                                                                                    <div id="2chart_div_survey_ask_vs_sell_all">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <div style='width: 2px; height: 100px; background-color: #D8D8D8;'>
                                                                            &nbsp;
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <table cellpadding='0' cellspacing='0' border='0'>
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <b>Avg Selling Price By Year Mfr($k)</b>
                                                                                    <br />
                                                                                    <div id="2chart_div_survey_sell_all">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>


                                                        <asp:Label runat="server" ID="spi_middle" Visible="False"></asp:Label><asp:Panel ID="google_sold_trends_graphs2" runat="server" Visible="false">
                                                            <br />
                                                            <table cellpadding="0" cellspacing="0" border='0' valign='top' width="100%">
                                                                <tr valign='top'>
                                                                    <td>
                                                                        <table cellpadding='0' cellspacing='0' border='0'>
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <asp:Label ID="ask_v_selling_label" runat="server" Text="<b>Avg Asking vs Selling Price ($k) - (For Asking with Sold)</b>"></asp:Label><br />
                                                                                    <div id="2chart_div_survey">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <b>Avg Asking Price ($k) - (All Asking Prices)</b>
                                                                                    <br />
                                                                                    <div id="2chart_div_sold_avg_asking_all">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <b>Avg. Sold Price % of Asking Price - (For Asking with Sold)</b>
                                                                                    <br />
                                                                                    <div id="2chart_div_percent_asking_all">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <div id="2chart_div_dom_all">
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div style='width: 2px; height: 700px; background-color: #D8D8D8;'>
                                                                            &nbsp;
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <table cellpadding='0' cellspacing='0' border='0'>
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <b>Avg Asking vs Selling Price ($k) - (All Asking/Sold Prices)</b>
                                                                                    <br />
                                                                                    <div id="2chart_div_survey_2_all">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <b>Avg Sold Price ($k) - (All Sold Prices)</b>
                                                                                    <br />
                                                                                    <div id="2chart_div_sold_avg_sold_all">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align='center'>
                                                                                    <b>Variance of Sold Price from Asking Price - (For Asking with Sold)</b>
                                                                                    <br />
                                                                                    <div id="2chart_div_variance_all">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <div id="2chart_div_aftt_all">
                                                                        </div>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>


                                                        <asp:Label runat="server" ID="spi_bottom" Visible="False"></asp:Label></td>
                                                    <td valign='top' runat="server" id="ToggleOffForSPI" visible="False">
                                                        <asp:Panel ID="right_side_graph_panel" Visible="False" runat="server">
                                                            <table cellpadding='0' cellspacing='0' border='0'>
                                                                <tr>
                                                                    <td width='100%' align='center'>
                                                                        <b>DEALER SALES PER YEAR (#SALES)</b> </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div id="chart_div_top_all" style="border-top: 0">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width='100%' align='center'>
                                                                        <b>DEALER SALES ROLES SINCE <%=(Year(Now) - 1)%></b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div id="chart_div_bottom_all" style="border-top: 0">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>


                                                        <asp:Panel ID="right_side_bottom_panel" Visible="False" runat="server">
                                                            <table cellpadding='0' cellspacing='0' border='0'>
                                                                <tr>
                                                                    <td width='100%' align='center'>
                                                                        <b>DEALER SALES BY MODEL SINCE
                                                                            <asp:Label ID="sales_sum_year" runat="server"></asp:Label></b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="chart_label_right_bottom" runat="server"></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div id="documents_view" runat="server" visible="False">
                                                <cc1:TabContainer ID="docTabContainer" runat="server" CssClass="dark-theme" Width="100%"
                                                    AutoPostBack="True">
                                                    <cc1:TabPanel ID="docTabPanel1" runat="server">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="docHeaderLabel1" runat="server"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <asp:Label ID="docContentLabel1" runat="server"></asp:Label>
                                                        </ContentTemplate>
                                                    </cc1:TabPanel>
                                                    <cc1:TabPanel ID="docTabPanel2" runat="server">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="docHeaderLabel2" runat="server"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <asp:Label ID="docContentLabel2" runat="server"></asp:Label>
                                                        </ContentTemplate>
                                                    </cc1:TabPanel>
                                                    <cc1:TabPanel ID="docTabPanel3" runat="server">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="docHeaderLabel3" runat="server"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <asp:Label ID="docContentLabel3" runat="server"></asp:Label>
                                                        </ContentTemplate>
                                                    </cc1:TabPanel>
                                                    <cc1:TabPanel ID="docTabPanel4" runat="server">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="docHeaderLabel4" runat="server"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <asp:Label ID="docContentLabel4" runat="server"></asp:Label>
                                                        </ContentTemplate>
                                                    </cc1:TabPanel>
                                                </cc1:TabContainer>
                                            </div>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                </cc1:TabContainer>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="viewOuterPanel2" runat="server" Visible="false" HorizontalAlign="Left"
                                Width="100%">
                                <div id="portfolio_view" runat="server">
                                    <cc1:TabContainer ID="portfolio_tabContainer" runat="server" CssClass="dark-theme"
                                        Width="100%" AutoPostBack="false">
                                        <cc1:TabPanel ID="portfolio_tabPanel1" runat="server">
                                            <HeaderTemplate>
                                                <asp:Label ID="portfolio_tabPanel1_Label1" runat="server" Text="Market Valuation"></asp:Label>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:CheckBox ID="portfolio_tabPanel1_CheckBox1"
                                                    runat="server" Text="Include Internal Sales"
                                                    AutoPostBack="True"
                                                    OnCheckedChanged="run_recent_sales" Visible="False" />


                                                <asp:CheckBox ID="portfolio_tabPanel1_CheckBox2"
                                                    runat="server" AutoPostBack="True"
                                                    Checked="True" OnCheckedChanged="run_recent_sales"
                                                    Visible="False" />


                                                <asp:Label runat="server" ID="portfolio_tabPanel1_Label3"></asp:Label><asp:Label ID="portfolio_tabPanel1_Label2" runat="server"></asp:Label>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="portfolio_tabPanel2" runat="server">
                                            <HeaderTemplate>
                                                <asp:Label ID="portfolio_tabPanel2_Label1" runat="server" Text="Maintenance"></asp:Label>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:Label ID="portfolio_tabPanel2_Label2" runat="server"></asp:Label>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="portfolio_tabPanel3" runat="server">
                                            <HeaderTemplate>
                                                <asp:Label ID="portfolio_tabPanel3_Label1" runat="server" Text="Related Features"></asp:Label>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:Label ID="portfolio_tabPanel3_Label2" runat="server"></asp:Label>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="portfolio_tabPanel4" runat="server">
                                            <HeaderTemplate>
                                                <asp:Label ID="portfolio_tabPanel4_Label1" runat="server" Text="Usage"></asp:Label>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:Label ID="portfolio_tabPanel4_Label2" runat="server"></asp:Label>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="portfolio_tabPanel5" runat="server">
                                            <HeaderTemplate>
                                                <asp:Label ID="portfolio_tabPanel5_Label1" runat="server" Text="Operations"></asp:Label>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:Label ID="portfolio_tabPanel5_Label2" runat="server"></asp:Label>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                    </cc1:TabContainer>
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
<asp:DropDownList ID="acKeepRemove" runat="server" CssClass="float_right display_none"
    Width="100%">
    <asp:ListItem Value="keep">keep</asp:ListItem>
    <asp:ListItem Selected="True" Value="remove">remove</asp:ListItem>
</asp:DropDownList><table width='100%' align='left'>
    <tr>
        <td align='right'>
            <asp:Label ID="buttons2" runat="server" CssClass="float_right criteria_text"></asp:Label></td>
    </tr>
    <tr>
        <td width='100%' align='left'>
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


                                    <asp:CheckBox ID="show_estimated" runat="server"
                                        Text="Show Estimated Value" />


                                    <asp:CheckBox ID="show_my" runat="server" Text="Show MY AC" />


                                    <asp:Button ID="change_large_graph" runat="server"
                                        OnClientClick="change_large_graph_clicked()" Text="Update Graph" />




                                </ContentTemplate>
                            </cc1:TabPanel>

                        </cc1:TabContainer>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:TextBox runat="server" ID="view_name_text" Style="display: none;" />
            <asp:TextBox runat="server" ID="view_id_text" Style="display: none;" />
            <asp:TextBox runat="server" ID="last_selected_zoom" Text="" Style="display: none;" />
        </td>
    </tr>
</table>
<asp:Label runat="server" ID="loadingTextContainer" CssClass="loadingTextStyle" Visible="false">
    <div>
        <img src="/images/loading.gif" alt="" />
        <asp:Label ID="loadingText" runat="server" CssClass="display_block" Text="false"></asp:Label>
    </div>
</asp:Label><cc1:TabContainer ID="tabs_bottom" CssClass="dark-theme" Width="100%" runat="server"
    ActiveTabIndex="1">
</cc1:TabContainer>
<asp:TextBox runat="server" ID="hiddenAftt_start" CssClass="display_none"></asp:TextBox><asp:TextBox runat="server" ID="hiddenAftt_end" CssClass="display_none"></asp:TextBox><asp:TextBox runat="server" ID="hiddenYear_start" CssClass="display_none"></asp:TextBox><asp:TextBox runat="server" ID="hiddenYear_end" CssClass="display_none"></asp:TextBox><script type="text/javascript">


                                                                                                                                                                                                                                                                                                                                                                function SubMenuDropValue2(reportID, folder_type) {

                                                                                                                                                                                                                                                                                                                                                                    my_form = document.createElement('FORM');
                                                                                                                                                                                                                                                                                                                                                                    my_form.method = 'GET';
                                                                                                                                                                                                                                                                                                                                                                    my_form.target = "_blank"
                                                                                                                                                                                                                                                                                                                                                                    // alert(folder_type);

                                                                                                                                                                                                                                                                                                                                                                    //folders maintenance popup  

                                                                                                                                                                                                                                                                                                                                                                    my_form.action = 'viewtopdf.aspx';
                                                                                                                                                                                                                                                                                                                                                                    my_form.name = 'folderForm';

                                                                                                                                                                                                                                                                                                                                                                    //Appending the type of folder, either Aircraft or History. 
                                                                                                                                                                                                                                                                                                                                                                    my_tb = document.createElement('INPUT');
                                                                                                                                                                                                                                                                                                                                                                    my_tb.type = 'HIDDEN';
                                                                                                                                                                                                                                                                                                                                                                    my_tb.name = "viewID";
                                                                                                                                                                                                                                                                                                                                                                    my_tb.value = "998"
                                                                                                                                                                                                                                                                                                                                                                    my_form.appendChild(my_tb);

                                                                                                                                                                                                                                                                                                                                                                    var str = '';
                                                                                                                                                                                                                                                                                                                                                                    var elem = document.getElementById('aspnetForm').elements;
                                                                                                                                                                                                                                                                                                                                                                    for (var i = 0; i < elem.length; i++) {
                                                                                                                                                                                                                                                                                                                                                                        if (elem[i].type != 'hidden' && elem[i].type != 'submit') {
                                                                                                                                                                                                                                                                                                                                                                            //if (elem[i].value != '') {
                                                                                                                                                                                                                                                                                                                                                                            if ((elem[i].id.indexOf("tabs_top_right_tabs_top_right_1") == -1) && (elem[i].id.indexOf("tabs_bottom_tabs_bottom_") == -1)) {
                                                                                                                                                                                                                                                                                                                                                                                var re = new RegExp("ctl[A-Za-z0-9]*_ContentPlaceHolder[A-Za-z0-9]_", "g");
                                                                                                                                                                                                                                                                                                                                                                                var re2 = new RegExp("View_Master1_", "g");
                                                                                                                                                                                                                                                                                                                                                                                var re3 = new RegExp("TabContainer1_", "g");
                                                                                                                                                                                                                                                                                                                                                                                var re4 = new RegExp("Value_View1_", "g");
                                                                                                                                                                                                                                                                                                                                                                                var re5 = new RegExp("ContentPlaceHolder1_", "g");

                                                                                                                                                                                                                                                                                                                                                                                var rep = elem[i].id;
                                                                                                                                                                                                                                                                                                                                                                                var temp = rep.replace(re, "");


                                                                                                                                                                                                                                                                                                                                                                                temp = temp.replace(re2, "");
                                                                                                                                                                                                                                                                                                                                                                                temp = temp.replace(re3, "");
                                                                                                                                                                                                                                                                                                                                                                                temp = temp.replace(re4, "");
                                                                                                                                                                                                                                                                                                                                                                                temp = temp.replace(re5, "");
                                                                                                                                                                                                                                                                                                                                                                                my_tb = document.createElement('INPUT');
                                                                                                                                                                                                                                                                                                                                                                                my_tb.type = 'HIDDEN';
                                                                                                                                                                                                                                                                                                                                                                                my_tb.name = temp;

                                                                                                                                                                                                                                                                                                                                                                                //If it has a checked value that's not undefined, go ahead and 
                                                                                                                                                                                                                                                                                                                                                                                //Pass that, if not, pass the value

                                                                                                                                                                                                                                                                                                                                                                                if (elem[i].type == 'checkbox') {
                                                                                                                                                                                                                                                                                                                                                                                    my_tb.value = elem[i].checked;
                                                                                                                                                                                                                                                                                                                                                                                    //alert(temp + " : " + elem[i].value);
                                                                                                                                                                                                                                                                                                                                                                                } else if (elem[i].type == 'select-multiple') {
                                                                                                                                                                                                                                                                                                                                                                                    //var opt = document.getElementById('' + elem[i].id + '').options
                                                                                                                                                                                                                                                                                                                                                                                    //alert(elem[i].id);
                                                                                                                                                                                                                                                                                                                                                                                    var SelBranchVal = "";
                                                                                                                                                                                                                                                                                                                                                                                    var x = 0;
                                                                                                                                                                                                                                                                                                                                                                                    for (x = 0; x < elem[i].length; x++) {
                                                                                                                                                                                                                                                                                                                                                                                        if (elem[i][x].selected) {
                                                                                                                                                                                                                                                                                                                                                                                            //Add seperator just not for 1st entry.
                                                                                                                                                                                                                                                                                                                                                                                            if (SelBranchVal != "") {
                                                                                                                                                                                                                                                                                                                                                                                                SelBranchVal = SelBranchVal + "##"
                                                                                                                                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                                                                                                                                            SelBranchVal = SelBranchVal + elem[i][x].value;
                                                                                                                                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                                                                                                                                    //alert(SelBranchVal);
                                                                                                                                                                                                                                                                                                                                                                                    my_tb.value = SelBranchVal; //elem[i].value;
                                                                                                                                                                                                                                                                                                                                                                                } else if (elem[i].type == 'radio') {
                                                                                                                                                                                                                                                                                                                                                                                    my_tb.value = elem[i].checked;
                                                                                                                                                                                                                                                                                                                                                                                } else {
                                                                                                                                                                                                                                                                                                                                                                                    my_tb.value = elem[i].value;
                                                                                                                                                                                                                                                                                                                                                                                }

                                                                                                                                                                                                                                                                                                                                                                                my_form.appendChild(my_tb);
                                                                                                                                                                                                                                                                                                                                                                                // }
                                                                                                                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                                                                                                                    }

                                                                                                                                                                                                                                                                                                                                                                    //Let's do a test to see if sliders moved:

                                                                                                                                                                                                                                                                                                                                                                    //year_start=2001&year_end=2014&aftt_start=0&aftt_end=10439

                                                                                                                                                                                                                                                                                                                                                                    my_tb = document.createElement('INPUT');
                                                                                                                                                                                                                                                                                                                                                                    my_tb.name = "afttFilter";

                                                                                                                                                                                                                                                                                                                                                                    if ($("#<%= aftt_start.ClientID %>").val() !== $("#<%= hiddenAftt_start.clientID %>").val() || $("#<%= aftt_end.clientID %>").val() !== $("#<%= hiddenAftt_end.clientID %>").val()) {
                                                                                                                                                                                                                                                                                                                                                                        my_tb.value = "true";
                                                                                                                                                                                                                                                                                                                                                                    } else {
                                                                                                                                                                                                                                                                                                                                                                        my_tb.value = "false";

                                                                                                                                                                                                                                                                                                                                                                    }

                                                                                                                                                                                                                                                                                                                                                                    my_form.appendChild(my_tb);

                                                                                                                                                                                                                                                                                                                                                                    my_tb = document.createElement('INPUT');
                                                                                                                                                                                                                                                                                                                                                                    my_tb.name = "yearFilter";

                                                                                                                                                                                                                                                                                                                                                                    if (($("#<%= year_start.clientID %>").val() !== $("#<%= hiddenYear_start.clientID %>").val() || $("#<%= year_end.clientID %>").val() !== $("#<%= hiddenYear_end.clientID %>").val())) {
                                                                                                                                                                                                                                                                                                                                                                        my_tb.value = "true";
                                                                                                                                                                                                                                                                                                                                                                    } else {
                                                                                                                                                                                                                                                                                                                                                                        my_tb.value = "false";
                                                                                                                                                                                                                                                                                                                                                                    }

                                                                                                                                                                                                                                                                                                                                                                    my_form.appendChild(my_tb);

                                                                                                                                                                                                                                                                                                                                                                    document.body.appendChild(my_form);
                                                                                                                                                                                                                                                                                                                                                                    my_form.submit();


                                                                                                                                                                                                                                                                                                                                                                }

                                                                                                                                                                                                                                                                                                                                                                function SubMenuDropValue3(reportID, folder_type) {

                                                                                                                                                                                                                                                                                                                                                                    my_form = document.createElement('FORM');
                                                                                                                                                                                                                                                                                                                                                                    my_form.method = 'GET';
                                                                                                                                                                                                                                                                                                                                                                    my_form.target = "_blank"
                                                                                                                                                                                                                                                                                                                                                                    // alert(folder_type);

                                                                                                                                                                                                                                                                                                                                                                    //folders maintenance popup  

                                                                                                                                                                                                                                                                                                                                                                    my_form.action = 'viewtopdf.aspx';
                                                                                                                                                                                                                                                                                                                                                                    my_form.name = 'folderForm';

                                                                                                                                                                                                                                                                                                                                                                    //Appending the type of folder, either Aircraft or History. 
                                                                                                                                                                                                                                                                                                                                                                    my_tb = document.createElement('INPUT');
                                                                                                                                                                                                                                                                                                                                                                    my_tb.type = 'HIDDEN';
                                                                                                                                                                                                                                                                                                                                                                    my_tb.name = "viewID";
                                                                                                                                                                                                                                                                                                                                                                    my_tb.value = "997"
                                                                                                                                                                                                                                                                                                                                                                    my_form.appendChild(my_tb);

                                                                                                                                                                                                                                                                                                                                                                    var str = '';
                                                                                                                                                                                                                                                                                                                                                                    var elem = document.getElementById('aspnetForm').elements;
                                                                                                                                                                                                                                                                                                                                                                    for (var i = 0; i < elem.length; i++) {
                                                                                                                                                                                                                                                                                                                                                                        if (elem[i].type != 'hidden' && elem[i].type != 'submit') {
                                                                                                                                                                                                                                                                                                                                                                            //if (elem[i].value != '') {
                                                                                                                                                                                                                                                                                                                                                                            if ((elem[i].id.indexOf("tabs_top_right_tabs_top_right_1") == -1) && (elem[i].id.indexOf("tabs_bottom_tabs_bottom_") == -1)) {
                                                                                                                                                                                                                                                                                                                                                                                var re = new RegExp("ctl[A-Za-z0-9]*_ContentPlaceHolder[A-Za-z0-9]_", "g");
                                                                                                                                                                                                                                                                                                                                                                                var re2 = new RegExp("TabContainer1_", "g");
                                                                                                                                                                                                                                                                                                                                                                                var re3 = new RegExp("View_Master1_", "g");
                                                                                                                                                                                                                                                                                                                                                                                var re4 = new RegExp("Value_View1_", "g");
                                                                                                                                                                                                                                                                                                                                                                                var re5 = new RegExp("ContentPlaceHolder1_", "g");

                                                                                                                                                                                                                                                                                                                                                                                var rep = elem[i].id;
                                                                                                                                                                                                                                                                                                                                                                                var temp = rep.replace(re, "");


                                                                                                                                                                                                                                                                                                                                                                                temp = temp.replace(re2, "");
                                                                                                                                                                                                                                                                                                                                                                                temp = temp.replace(re3, "");
                                                                                                                                                                                                                                                                                                                                                                                temp = temp.replace(re4, "");
                                                                                                                                                                                                                                                                                                                                                                                temp = temp.replace(re5, "")
                                                                                                                                                                                                                                                                                                                                                                                my_tb = document.createElement('INPUT');
                                                                                                                                                                                                                                                                                                                                                                                my_tb.type = 'HIDDEN';
                                                                                                                                                                                                                                                                                                                                                                                my_tb.name = temp;

                                                                                                                                                                                                                                                                                                                                                                                //If it has a checked value that's not undefined, go ahead and 
                                                                                                                                                                                                                                                                                                                                                                                //Pass that, if not, pass the value

                                                                                                                                                                                                                                                                                                                                                                                //alert(rep + ' ' + elem[i].value);
                                                                                                                                                                                                                                                                                                                                                                                if (elem[i].type == 'checkbox') {
                                                                                                                                                                                                                                                                                                                                                                                    my_tb.value = elem[i].checked;
                                                                                                                                                                                                                                                                                                                                                                                    //alert(temp + " : " + elem[i].value);
                                                                                                                                                                                                                                                                                                                                                                                } else if (elem[i].type == 'select-multiple') {
                                                                                                                                                                                                                                                                                                                                                                                    //var opt = document.getElementById('' + elem[i].id + '').options
                                                                                                                                                                                                                                                                                                                                                                                    //alert(elem[i].id);
                                                                                                                                                                                                                                                                                                                                                                                    if (elem[i].id == 'cboAircraftViewModelID') {
                                                                                                                                                                                                                                                                                                                                                                                        my_tb.name = 'model_list';
                                                                                                                                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                                                                                                                                    var SelBranchVal = "";
                                                                                                                                                                                                                                                                                                                                                                                    var x = 0;
                                                                                                                                                                                                                                                                                                                                                                                    for (x = 0; x < elem[i].length; x++) {
                                                                                                                                                                                                                                                                                                                                                                                        if (elem[i][x].selected) {
                                                                                                                                                                                                                                                                                                                                                                                            //Add seperator just not for 1st entry.
                                                                                                                                                                                                                                                                                                                                                                                            if (SelBranchVal != "") {
                                                                                                                                                                                                                                                                                                                                                                                                SelBranchVal = SelBranchVal + "##"
                                                                                                                                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                                                                                                                                            SelBranchVal = SelBranchVal + elem[i][x].value;
                                                                                                                                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                                                                                                                                                                    //alert(SelBranchVal);
                                                                                                                                                                                                                                                                                                                                                                                    my_tb.value = SelBranchVal; //elem[i].value;
                                                                                                                                                                                                                                                                                                                                                                                } else if (elem[i].type == 'radio') {
                                                                                                                                                                                                                                                                                                                                                                                    my_tb.value = elem[i].checked;
                                                                                                                                                                                                                                                                                                                                                                                } else {
                                                                                                                                                                                                                                                                                                                                                                                    my_tb.value = elem[i].value;
                                                                                                                                                                                                                                                                                                                                                                                }

                                                                                                                                                                                                                                                                                                                                                                                my_form.appendChild(my_tb);
                                                                                                                                                                                                                                                                                                                                                                                // }
                                                                                                                                                                                                                                                                                                                                                                            }
                                                                                                                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                                                                                                                    }

                                                                                                                                                                                                                                                                                                                                                                    //Let's do a test to see if sliders moved:

                                                                                                                                                                                                                                                                                                                                                                    //year_start=2001&year_end=2014&aftt_start=0&aftt_end=10439

                                                                                                                                                                                                                                                                                                                                                                    my_tb = document.createElement('INPUT');
                                                                                                                                                                                                                                                                                                                                                                    my_tb.name = "afttFilter";

                                                                                                                                                                                                                                                                                                                                                                    if ($("#<%= aftt_start.clientID %>").val() !== $("#<%= hiddenAftt_start.clientID %>").val() || $("#<%= aftt_end.clientID %>").val() !== $("#<%= hiddenAftt_end.clientID %>").val()) {
                                                                                                                                                                                                                                                                                                                                                                        my_tb.value = "true";
                                                                                                                                                                                                                                                                                                                                                                    } else {
                                                                                                                                                                                                                                                                                                                                                                        my_tb.value = "false";

                                                                                                                                                                                                                                                                                                                                                                    }

                                                                                                                                                                                                                                                                                                                                                                    my_form.appendChild(my_tb);

                                                                                                                                                                                                                                                                                                                                                                    my_tb = document.createElement('INPUT');
                                                                                                                                                                                                                                                                                                                                                                    my_tb.name = "yearFilter";

                                                                                                                                                                                                                                                                                                                                                                    if (($("#<%= year_start.clientID %>").val() !== $("#<%= hiddenYear_start.clientID %>").val() || $("#<%= year_end.clientID %>").val() !== $("#<%= hiddenYear_end.clientID %>").val())) {
                                                                                                                                                                                                                                                                                                                                                                        my_tb.value = "true";
                                                                                                                                                                                                                                                                                                                                                                    } else {
                                                                                                                                                                                                                                                                                                                                                                        my_tb.value = "false";
                                                                                                                                                                                                                                                                                                                                                                    }

                                                                                                                                                                                                                                                                                                                                                                    my_form.appendChild(my_tb);

                                                                                                                                                                                                                                                                                                                                                                    document.body.appendChild(my_form);
                                                                                                                                                                                                                                                                                                                                                                    my_form.submit();


                                                                                                                                                                                                                                                                                                                                                                }


                                                                                                                                                                                                                                                                                                                                                                function setUpAutoComplete() {
                                                                                                                                                                                                                                                                                                                                                                    $("#<%= fboViewSearch_textbox.clientID %>").autocomplete({
                                                                                                                                                                                                                                                                                                                                                                        source: function (request, response) {
                                                                                                                                                                                                                                                                                                                                                                            $.ajax({
                                                                                                                                                                                                                                                                                                                                                                                type: "GET",
                                                                                                                                                                                                                                                                                                                                                                                url: "JSONresponse.aspx/Airport?term=" + request.term,
                                                                                                                                                                                                                                                                                                                                                                                contentType: "application/json; charset=utf-8",
                                                                                                                                                                                                                                                                                                                                                                                dataType: "json",
                                                                                                                                                                                                                                                                                                                                                                                data: {},
                                                                                                                                                                                                                                                                                                                                                                                success: function (data) {
                                                                                                                                                                                                                                                                                                                                                                                    var json = data.d;
                                                                                                                                                                                                                                                                                                                                                                                    var obj = JSON.parse(json);
                                                                                                                                                                                                                                                                                                                                                                                    response(obj);
                                                                                                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                                                                                            });
                                                                                                                                                                                                                                                                                                                                                                        },
                                                                                                                                                                                                                                                                                                                                                                        appendTo: ".container",
                                                                                                                                                                                                                                                                                                                                                                        minLength: 3,
                                                                                                                                                                                                                                                                                                                                                                        select: function (event, ui) {
                                                                                                                                                                                                                                                                                                                                                                            $("#<%= fboViewSearch_textbox.clientID %>").val(ui.item.label);
                                                                                                                                                                                                                                                                                                                                                                            $("#<%= selectAirportID.clientID %>").val(ui.item.value);
                                                                                                                                                                                                                                                                                                                                                                            return false;
                                                                                                                                                                                                                                                                                                                                                                        }
                                                                                                                                                                                                                                                                                                                                                                    });

                                                                                                                                                                                                                                                                                                                                                                }

</script><asp:Literal runat="server" ID="fboMapScript" Visible="false">
<script type="text/javascript" language="javascript">
    var viewMapFBO = null;
    function initialize_view_mapWithCenter(lat, longi, mapName) {

        var view_mapDiv = document.getElementById(mapName);
        var mapOptions = {
            zoom: 13,
            center: new google.maps.LatLng(lat, longi),
            mapTypeId: google.maps.MapTypeId.SATELLITE,
            disableDefaultUI: true
        };

        viewMapFBO = new google.maps.Map(view_mapDiv, mapOptions);
        if ((viewMapFBO != null) && (typeof (viewMapFBO) != "undefined")) {
            view_map = viewMapFBO;
        }

    }


    function center_location_view_map(latitude, longitude, zoom_level) {

        if (Number(latitude) == 0 && Number(longitude) == 0 && Number(zoom_level) == 0) { //not initalizing map, do not ignore this
            initialize_view_map();
            return false;
        }

        //Setting up the new options for the map.
        var mapOptions = {
            zoom: zoom_level,
            center: new google.maps.LatLng(latitude, longitude),
            mapTypeId: google.maps.MapTypeId.HYBRID
        };

        //alert("show location map[lat:" + latitude + "][lng:" + longitude + "][zl:" + zoom_level + "]");

        var map = new google.maps.Map(view_mapDiv, mapOptions);

        //finding the map.    
        if ((map != null) && (typeof (map) != "undefined")) {

            view_map = map;
            google.maps.event.addListenerOnce(map, 'idle', function () {
                google.maps.event.trigger(map, 'resize');
            });
        }
    }

    function add_location_view_listener(marker, title, counter, link, map) { //adding listener on click event. Basically adds a popup window with predetermined text on click event of marker.

        var contentString = "";

        if (Number(counter) > 0) {

            contentString = '<div id="content"><div id="siteNotice"></div>' +
                '<h1 id="firstHeading" class="firstHeading">' + title + '</h1>' +
                '<div id="bodyContent"><p><b>Number of aircraft at this location is ' + counter + '</b></p>' +
                '<p><a href="' + link + '" title="' + link + '">Click to view aircraft at this location</a></p></div></div>';
        }
        else {
            if (Number(counter) == -1) {
                contentString = '<div id="content"><div id="siteNotice"></div>' +
                    '<h1 id="firstHeading" class="firstHeading">' + title + '</h1>' +
                    '<div id="bodyContent"><p><a href="' + link + '" title="' + link + '">Click to view aircraft at this location</a></p></div></div>';
            }
            else {
                contentString = '<div id="content"><div id="siteNotice"></div>' +
                    '<h1 id="firstHeading" class="firstHeading">' + title + '</h1>' +
                    '<div id="bodyContent"></div></div>';

            }
        }

        var infowindow = new google.maps.InfoWindow({ content: contentString });

        //Then go ahead and add the listener marker to the map.
        google.maps.event.addListener(marker, 'click', function () {
            infowindow.open(map, marker);
        });
    }


    function add_location_view_marker(location_title, latitude, longitude, counter, link, map) { //adding a new marker to the map ... Basically adds a popup window with predetermined text on click event of marker.

        //finding the map.    
        if ((map != null) && (typeof (map) != "undefined")) {

            var icon = {
                url: '../images/evoPlane.png'
            };

            //creating the marker for the map based on latitude, longitude
            var marker = new google.maps.Marker({
                position: new google.maps.LatLng(latitude, longitude),
                map: map,
                icon: icon,
                center: new google.maps.LatLng(latitude, longitude),
                title: location_title
            });

            google.maps.event.clearListeners(marker, 'onclick');
            add_location_view_listener(marker, location_title, counter, link, map);

        }
    }

                                          </script> </asp:Literal><script type="text/javascript" language="javascript">

                                                                      function setUpLinks(selectorName) {
                                                                          $("" + selectorName + "").each(function (i, obj) {
                                                                              if (typeof $(this).attr('href') !== typeof undefined && $(this).attr('href') !== false) {
                                                                                  if ($(this).attr('href').toLowerCase().indexOf('view_template.aspx') >= 0) {
                                                                                      $(this).attr("onClick", "breakApartLinkAndSearch($(this).attr('href'));return false;");
                                                                                  }
                                                                              }
                                                                          })
                                                                      }


                                                                      function breakApartLinkAndSearch(link) {
                                                                          var test = link.split("aport_id="); var shown = test[1].split("&");

                                                                          if (shown[0] > 0) {
                                                                              $("#<%= selectAirportsType.clientID %>").val("3");
                                                                              $("#<%= selectAirportID.clientID %>").val(shown[0]);
                                                                              $("#<%=  airportFolderToggleOnOff.clientID %>").addClass("display_none");
                                                                              $("#<%=airportSearchToggleOnOff.clientID %>").addClass("display_none");
                                                                              $("#<%= searchAirportFolder.clientID %>").val('');
                                                                              $("#<%=  airportIataToggle.clientID %>").addClass("display_none");
                                                                              $("#<%= airportIATABoxSearch.clientID %>").val('');
                                                                          } else {
                                                                              $("#<%= selectAirportID.clientID %>").val('0');
                                                                          }

                                                                          var test2 = shown[1].split("comp_id=");
                                                                          if (test2[1] > 0) {
                                                                              $('#<%= airportOperatorType.clientID %>').val("0");
                                                                              $("#<%= viewOperatorHiddenCompanyID.clientID %>").val(test2[1]);
                                                                              $("#<%=airportOperatorFolderToggleOnOff.clientID %>").addClass("display_none");
                                                                              $("#<%= airportOperatorFolder.clientID %>").val('');
                                                                          } else { $("#<%= viewOperatorHiddenCompanyID.clientID %>").val('0'); }

                                                                          searchThisForm();
                                                                      }

                                                                      function searchThisForm() {
                                                                          $("#<%= goSearchAirports.clientID %>").click();

                                                                          $('html, body').animate({
                                                                              scrollTop: $(".loadingScreenViewSearch").offset().top
                                                                          }, 1000);
                                                                      }


                                          </script><script>

  //  $("#<%= preownedSales_Only.clientID %>").change(function() {
  ////    $("#<%= used_of_used.clientID %>").checked = this.checked;
  //    alert('test');
  //  });

  //  $("#<%= preownedSales_Only.clientID %>").click(function() {
                                                       //    if ($(this).is(':checked')) alert("checked");
                                                       //  });
                                                       function checkPreOwned(CheckVal) {
                                                           SetWaitCursor();
                                                           $("#<%= used_of_used.clientID %>").checked = CheckVal;
                                                       }


                                                       function SetLoading(DivTag) {
                                                           //$("#" + DivTag).show();
                                                           $("#" + DivTag).css("display", "block");
                                                           //$("#" + DivTag).html(Message);
                                                           //$("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 75, resizable: false });
                                                       }

                                                       function CloseLoadingMessage(DivTag) {
                                                           //$("#" + DivTag).fadeOut(1000);
                                                           $("#" + DivTag).css("display", "none");
                                                       }
                                          </script>