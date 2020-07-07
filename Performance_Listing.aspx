<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Performance_Listing.aspx.vb"
    Inherits="crmWebClient.Performance_Listing" MasterPageFile="~/EvoStyles/EvoTheme.Master"
    EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/EvoStyles/EvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        //These functions are there for the popout link to the view. This means it's opening up the model market summary.
        function SetViewName() {
            return "Model Market Summary";
        }
        function SetViewID() {
            return 1;
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="DataGridShadowContainer PerformanceListingTable Performance">
        <asp:Panel runat="server" Visible="true" ID="Performance_Criteria">
            <cc1:CollapsiblePanelExtender ID="PerformancePanelEx" runat="server" TargetControlID="Performance_Collapse_Panel"
                Collapsed="true" ExpandControlID="Performance_Control_Panel" ImageControlID="Performance_Image"
                CollapsedText="New Search" ExpandedText="Hide Search" ExpandedImage="../Images/search_collapse.jpg"
                CollapsedImage="../Images/search_expand.jpg" CollapseControlID="Performance_Control_Panel"
                Enabled="True">
            </cc1:CollapsiblePanelExtender>
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="left" valign="top" class="dark_header" width="100%">
                        <asp:Table ID="Table5" runat="server" Width="100%" CellPadding="0" CellSpacing="0"
                            CssClass="padding_table">
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="20" ID="performance_help_text"
                                    CssClass="evoHelp displayNoneMobile">
                        <a href="#">Help</a>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="90" ID="performance_search_expand_text"
                                    CssClass="displayNoneMobile">
                                    <asp:Panel ID="Performance_Control_Panel" runat="server" Width="100%">
                                        <asp:Image ID="Performance_Image" runat="server" ImageUrl="../Images/search_expand.jpg" />
                                    </asp:Panel>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" ID="TableCell15">
                                    <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                                    <asp:Panel runat="server" ID="MobileSearchVisible" Visible="false" CssClass="padding_bottom">
                                        <asp:DropDownList runat="server" AutoPostBack="true" ID="makeModelDynamic" CssClass="chosen-select"
                                            Width="100%">
                                            <asp:ListItem Value="">Please pick a Model</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:Panel>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="TableCell16"
                                    CssClass="displayNoneMobile">
                      
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="70" ID="TableCell17"
                                    CssClass="displayNoneMobile">
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="65" ID="TableCell18"
                                    CssClass="displayNoneMobile">
                    
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="TableCell19"
                                    CssClass="displayNoneMobile">
                          
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="65" ID="TableCell20"
                                    CssClass="displayNoneMobile">
                       
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="TableCell21"
                                    CssClass="displayNoneMobile">
                         
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="45" ID="TableCell22"
                                    CssClass="displayNoneMobile">
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="75" ID="TableCell23"
                                    CssClass="displayNoneMobile">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="performance_actions_dropdown" runat="server" CssClass="ul_top">
                                            <asp:ListItem>Actions</asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="performance_actions_submenu_dropdown" runat="server" CssClass="ul_bottom ac_action_dropdown"
                                            DisplayMode="HyperLink">
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="70" ID="TableCell1"
                                    CssClass="displayNoneMobile">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="performance_folders_dropdown" runat="server" CssClass="ul_top sort_dropdown_width">
                                            <asp:ListItem>Folders</asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="performance_folders_submenu_dropdown" runat="server" CssClass="ul_bottom folder_dropdown"
                                            DisplayMode="HyperLink">
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Performance_Collapse_Panel" runat="server" Height="0px" Width="100%"
                CssClass="collapse">
                <asp:Label runat="server" ID="close_current_folder" Font-Bold="true" ForeColor="Red"
                    Visible="false"><br /><br /><p align="center" class="medium_text">You must Close Current Folder before starting a New Search.</p><br /><br /></asp:Label>
                <asp:Table ID="perfSpecsTable" Width="100%" CellPadding="3" CellSpacing="0" runat="server">
                    <asp:TableRow>
                        <asp:TableCell Width="33%" HorizontalAlign="Left" VerticalAlign="Top" CssClass="model_search_box collapseSearchTable">
                            <asp:Panel runat="server">
                                <asp:Panel runat="server" ID="perfspecs_make_model_panel">
                                    <asp:Table ID="Table7" Width="100%" CellPadding="3" CellSpacing="0" runat="server">
                                        <asp:TableRow>
                                            <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                                                <asp:Panel ID="opcosts_make_model_panel" runat="server">
                                                    <evo:viewTMMDropDowns ID="ViewTMMDropDowns" runat="server" />

                                                    <script language="javascript" type="text/javascript">
                                                                refreshTypeMakeModelByCheckBox("", "", <%= isHeliOnlyProduct.tostring.tolower%>,<%= productCodeCount.tostring%>);
                                                    </script>

                                                </asp:Panel>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </asp:Panel>
                            </asp:Panel>
                        </asp:TableCell>
                        <asp:TableCell Width="67%" HorizontalAlign="Left" VerticalAlign="bottom" ID="tableCellToggle"
                            CssClass="collapseSearchTable mobileWhiteBackground margin_1 displayNoneMobile">
                            <div>
                                <table cellpadding="2" cellspacing="0">
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <div id="Div2" runat="server">
                                                <strong>FUSELAGE DIMENSIONS (ft)</strong>
                                            </div>
                                        </td>
                                        <td align="left" valign="top" width="25%">Value
                                        </td>
                                        <td align="left" valign="top">Format
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" width="25%" class='indent_cell'>Length:
                                        </td>
                                        <td align="left" valign="top" width="25%">
                                            <asp:DropDownList ID="fuselage_length_ddl" runat="server" Width="100%" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'fuselage_length_txt', 'input');">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>Equals</asp:ListItem>
                                                <asp:ListItem>Less Than</asp:ListItem>
                                                <asp:ListItem>Greater Than</asp:ListItem>
                                                <asp:ListItem>Between</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" valign="top" width="25%">
                                            <asp:TextBox ID="fuselage_length_txt" runat="server" Width="100%"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="ValidateFuselageLength" runat="server" ControlToValidate="fuselage_length_txt"
                                                Font-Bold="true" ValidationGroup="Numeric" SetFocusOnError="true" ValidationExpression="^[\d,:\s\n]+$"
                                                Text="*Incorrect Format" Display="Dynamic" Enabled="true"></asp:RegularExpressionValidator>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="Label7" runat="server" Text="" BackColor="#E0E0E0" CssClass="display_block padding border_format_label">nnnn</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class='indent_cell'>Height:
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="fuselage_height_ddl" runat="server" Width="100%" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'fuselage_height_txt', 'input');">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>Equals</asp:ListItem>
                                                <asp:ListItem>Less Than</asp:ListItem>
                                                <asp:ListItem>Greater Than</asp:ListItem>
                                                <asp:ListItem>Between</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="fuselage_height_txt" runat="server" Width="100%"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="ValidateFuselageHeight" runat="server" ControlToValidate="fuselage_height_txt"
                                                Font-Bold="true" ValidationGroup="Numeric" SetFocusOnError="true" ValidationExpression="^[\d,:\s\n]+$"
                                                Text="*Incorrect Format" Display="Dynamic" Enabled="true"></asp:RegularExpressionValidator>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="Label8" runat="server" Text="" BackColor="#E0E0E0" CssClass="display_block padding border_format_label">nnnn</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class='indent_cell'>
                                            <span id="wing_span">Wing Span/Width:</span>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="fuselage_wing_ddl" runat="server" Width="100%" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'fuselage_wing_txt', 'input');">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>Equals</asp:ListItem>
                                                <asp:ListItem>Less Than</asp:ListItem>
                                                <asp:ListItem>Greater Than</asp:ListItem>
                                                <asp:ListItem>Between</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="fuselage_wing_txt" runat="server" Width="100%"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="ValidateFuselageWing" runat="server" ControlToValidate="fuselage_wing_txt"
                                                Font-Bold="true" ValidationGroup="Numeric" SetFocusOnError="true" ValidationExpression="^[\d,:\s\n]+$"
                                                Text="*Incorrect Format" Display="Dynamic" Enabled="true"></asp:RegularExpressionValidator>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="Label9" runat="server" Text="" BackColor="#E0E0E0" CssClass="display_block padding border_format_label">nnnn</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <strong>CONFIGURATION</strong>
                                        </td>
                                        <td align="left" valign="top">Value
                                        </td>
                                        <td align="left" valign="top">Format
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class='indent_cell'>Crew:
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="crew_ddl" runat="server" Width="100%" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'crew_txt', 'input');">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>Equals</asp:ListItem>
                                                <asp:ListItem>Less Than</asp:ListItem>
                                                <asp:ListItem>Greater Than</asp:ListItem>
                                                <asp:ListItem>Between</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="crew_txt" runat="server" Width="100%"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="ValidateCrew" runat="server" ControlToValidate="crew_txt"
                                                Font-Bold="true" ValidationGroup="Numeric" SetFocusOnError="true" ValidationExpression="^[\d,:\s\n]+$"
                                                Text="*Incorrect Format" Display="Dynamic" Enabled="true"></asp:RegularExpressionValidator>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="Label10" runat="server" Text="" BackColor="#E0E0E0" CssClass="display_block padding border_format_label">nnnn</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class='indent_cell'>Passengers:
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="passengers_ddl" runat="server" Width="100%" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'passengers_txt', 'input');">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>Equals</asp:ListItem>
                                                <asp:ListItem>Less Than</asp:ListItem>
                                                <asp:ListItem>Greater Than</asp:ListItem>
                                                <asp:ListItem>Between</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="passengers_txt" runat="server" Width="100%"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="ValidatePassengers" runat="server" ControlToValidate="passengers_txt"
                                                Font-Bold="true" ValidationGroup="Numeric" SetFocusOnError="true" ValidationExpression="^[\d,:\s\n]+$"
                                                Text="*Incorrect Format" Display="Dynamic" Enabled="true"></asp:RegularExpressionValidator>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="Label11" runat="server" Text="" BackColor="#E0E0E0" CssClass="display_block padding border_format_label">nnnn</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <div id="Div3" runat="server">
                                                <strong>WEIGHT (lbs)</strong>
                                            </div>
                                        </td>
                                        <td align="left" valign="top">Value
                                        </td>
                                        <td align="left" valign="top">Format
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class='indent_cell'>Max Takeoff:
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="max_takeoff_ddl" runat="server" Width="100%" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'max_takeoff_txt', 'input');">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>Equals</asp:ListItem>
                                                <asp:ListItem>Less Than</asp:ListItem>
                                                <asp:ListItem>Greater Than</asp:ListItem>
                                                <asp:ListItem>Between</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="max_takeoff_txt" runat="server" Width="100%"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="ValidateMaxTakeoff" runat="server" ControlToValidate="max_takeoff_txt"
                                                Font-Bold="true" ValidationGroup="Numeric" SetFocusOnError="true" ValidationExpression="^[\d,:\s\n]+$"
                                                Text="*Incorrect Format" Display="Dynamic" Enabled="true"></asp:RegularExpressionValidator>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="Label12" runat="server" Text="" BackColor="#E0E0E0" CssClass="display_block padding border_format_label">nnnn</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <div id="Div4" runat="server">
                                                <strong>CAPACITY (gal)</strong>
                                            </div>
                                        </td>
                                        <td align="left" valign="top">Value
                                        </td>
                                        <td align="left" valign="top">Format
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class='indent_cell'>Fuel Capacity:
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="fuel_capacity_ddl" runat="server" Width="100%" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'fuel_capacity_txt', 'input');">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>Equals</asp:ListItem>
                                                <asp:ListItem>Less Than</asp:ListItem>
                                                <asp:ListItem>Greater Than</asp:ListItem>
                                                <asp:ListItem>Between</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="fuel_capacity_txt" runat="server" Width="100%"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="fuel_capacity_txt"
                                                Font-Bold="true" ValidationGroup="Numeric" SetFocusOnError="true" ValidationExpression="^[\d,:\s\n]+$"
                                                Text="*Incorrect Format" Display="Dynamic" Enabled="true"></asp:RegularExpressionValidator>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="Label1" runat="server" Text="" BackColor="#E0E0E0" CssClass="display_block padding border_format_label">nnnn</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <div id="Div5" runat="server">
                                                <strong>SPEED (kts)</strong>
                                            </div>
                                        </td>
                                        <td align="left" valign="top">Value
                                        </td>
                                        <td align="left" valign="top">Format
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class='indent_cell'>Normal Cruise TAS:
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="cruise_speed_ddl" runat="server" Width="100%" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'cruise_speed_txt', 'input');">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>Equals</asp:ListItem>
                                                <asp:ListItem>Less Than</asp:ListItem>
                                                <asp:ListItem>Greater Than</asp:ListItem>
                                                <asp:ListItem>Between</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="cruise_speed_txt" runat="server" Width="100%"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="ValidateCruiseSpeed" runat="server" ControlToValidate="cruise_speed_txt"
                                                Font-Bold="true" ValidationGroup="Numeric" SetFocusOnError="true" ValidationExpression="^[\d,:\s\n]+$"
                                                Text="*Incorrect Format" Display="Dynamic" Enabled="true"></asp:RegularExpressionValidator>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="Label13" runat="server" Text="" BackColor="#E0E0E0" CssClass="display_block padding border_format_label">nnnn</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <strong>DISPLAY UNITS</strong>
                                        </td>
                                        <td align="left" valign="top" colspan="3">
                                            <asp:CheckBox ID="us_standard" runat="server" Text="US Standard" Checked="true" />
                                            <asp:CheckBox ID="metric_standard" runat="server" Text="Metric Standard" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <strong>DISPLAY MILES</strong>
                                        </td>
                                        <td align="left" valign="top" colspan="3">
                                            <asp:CheckBox ID="nautical_miles" runat="server" Text="Nautical Miles" Checked="true" />
                                            <asp:CheckBox ID="statute_miles" runat="server" Text="Statute Miles" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <div id="Div0" runat="server">
                                                <strong>TAKEOFF PERFORMANCE (ft)</strong>
                                            </div>
                                        </td>
                                        <td align="left" valign="top" width="15%">Value
                                        </td>
                                        <td align="left" valign="top" width="25%">Format
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class='indent_cell'>SL ISA BFL:
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="takeoff_sl_ddl" runat="server" Width="100%" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'takeoff_sl_txt', 'input');">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>Equals</asp:ListItem>
                                                <asp:ListItem>Less Than</asp:ListItem>
                                                <asp:ListItem>Greater Than</asp:ListItem>
                                                <asp:ListItem>Between</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" valign="top" width="25%">
                                            <asp:TextBox ID="takeoff_sl_txt" runat="server" Width="100%"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="Validate_SLI" runat="server" ControlToValidate="takeoff_sl_txt"
                                                Font-Bold="true" ValidationGroup="Numeric" SetFocusOnError="true" ValidationExpression="^[\d,:\s\n]+$"
                                                Text="*Incorrect Format" Display="Dynamic" Enabled="true"></asp:RegularExpressionValidator>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="Label5" runat="server" Text="" BackColor="#E0E0E0" CssClass="display_block padding border_format_label">nnnn</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <div id="Div1" runat="server">
                                                <strong>RANGE (nm)</strong>
                                            </div>
                                        </td>
                                        <td align="left" valign="top">Value
                                        </td>
                                        <td align="left" valign="top">Format
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class='indent_cell'>
                                            <span id="range">Max Range (NBAA IFR/Tanks Full):</span>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="maxrange_ddl" runat="server" Width="100%" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'maxrange_txt', 'input');">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>Equals</asp:ListItem>
                                                <asp:ListItem>Less Than</asp:ListItem>
                                                <asp:ListItem>Greater Than</asp:ListItem>
                                                <asp:ListItem>Between</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="performance_specs_folder_name" runat="server" CssClass="display_none"></asp:TextBox>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="maxrange_txt" runat="server" Width="100%"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="ValidateMaxRange" runat="server" ControlToValidate="maxrange_txt"
                                                Font-Bold="true" ValidationGroup="Numeric" SetFocusOnError="true" ValidationExpression="^[\d,:\s\n]+$"
                                                Text="*Incorrect Format" Display="Dynamic" Enabled="true"></asp:RegularExpressionValidator>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="Label6" runat="server" Text="" BackColor="#E0E0E0" CssClass="display_block padding border_format_label">nnnn</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" valign="top" colspan="4">
                                            <asp:Button ID="performance_search" runat="server" Text="Search" CssClass="button-darker button_width"
                                                CausesValidation="true" ValidationGroup="Numeric" /><br />
                                            <asp:Button ID="reset" runat="server" Text="Clear Selections" CssClass="font-weight-normal button_width"
                                                CausesValidation="false" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
        </asp:Panel>
        <asp:Label runat="server" ID="FolderInformation" Visible="false" CssClass="FolderNameBar help_cursor"></asp:Label>
        <asp:Label ID="performance_attention" runat="server" Text="" CssClass="red_text emphasis_text text_align_center small_to_medium_text"></asp:Label>
        <asp:Panel ID="container_performance_listing" runat="server">
            <asp:Label ID="performance_listing_text" runat="server" Text=""></asp:Label>
        </asp:Panel>
    </div>

    <script type="text/javascript">

        $('#chkHelicopterFilterID').change(function () {
            UpdateLabels();
        });
        $('#chkCommercialFilterID').change(function () {
            UpdateLabels();
        });
        $('#chkBusinessFilterID').change(function () {
            UpdateLabels();
        });
        function UpdateLabels() {
            if ($('#chkHelicopterFilterID:checked').val() == "true") {
                if ($('#chkBusinessFilterID:checked').val() == "true" || $('#chkCommercialFilterID:checked').val() == "true") {
                    $("#wing_span").text("Wing Span/Width:");
                    $("#range").text("Max Range (NBAA IFR/Tanks Full):");
                } else {
                    $("#wing_span").text("Width:");
                    $("#range").text("Max Range (Tanks Full):");
                }
            } else {
                if ($('#chkBusinessFilterID:checked').val() == "true" || $('#chkCommercialFilterID:checked').val() == "true") {
                    $("#wing_span").text("Wing Span:");
                    $("#range").text("Max Range (NBAA IFR):");
                }
            }
        }
    </script>

</asp:Content>
