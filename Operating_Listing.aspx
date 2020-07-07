<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Operating_Listing.aspx.vb"
    Inherits="crmWebClient.Operating_Listing" MasterPageFile="~/EvoStyles/EvoTheme.Master"
    StylesheetTheme="Evo" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/EvoStyles/EvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="DataGridShadowContainer  PerformanceListingTable OpHeight">
        <asp:Panel runat="server" Visible="true" ID="Operating_Criteria">
            <cc1:CollapsiblePanelExtender ID="OperatingPanelEx" runat="server" TargetControlID="Operating_Collapse_Panel"
                Collapsed="true" ExpandControlID="Operating_Control_Panel" ImageControlID="Operating_Image"
                CollapsedText="New Search" ExpandedText="Hide Search" ExpandedImage="../Images/search_collapse.jpg"
                CollapsedImage="../Images/search_expand.jpg" CollapseControlID="Operating_Control_Panel"
                Enabled="True">
            </cc1:CollapsiblePanelExtender>
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="left" valign="top" class="dark_header" width="100%">
                        <asp:Table ID="Table17" runat="server" Width="100%" CellPadding="0" CellSpacing="0"
                            CssClass="padding_table">
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="20" ID="Operating_help_text"
                                    CssClass="evoHelp displayNoneMobile">
                        <a href="#">Help</a></asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="90" ID="Operating_search_expand_text"
                                    CssClass="displayNoneMobile">
                                    <asp:Panel ID="Operating_Control_Panel" runat="server" Width="100%">
                                        <asp:Image ID="Operating_Image" runat="server" ImageUrl="../Images/search_expand.jpg" />
                                    </asp:Panel>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="top" ID="TableCell1">
                                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                    <asp:Panel runat="server" ID="MobileSearchVisible" Visible="false" CssClass="padding_bottom">
                                        <asp:DropDownList runat="server" AutoPostBack="true" ID="makeModelDynamic" CssClass="chosen-select float_left margin"
                                            Width="100%">
                                            <asp:ListItem Value="">Please pick a Model</asp:ListItem>
                                        </asp:DropDownList><div class="clearfix margin-bottom"></div>
                                        <asp:DropDownList runat="server" AutoPostBack="true" ID="mobileCurrency" CssClass="chosen-select marginDropdown"
                                            Width="100%">
                                            <asp:ListItem Value="">Currency</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:Panel>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="30" ID="TableCell2"
                                    CssClass="displayNoneMobile">
                     
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="70" ID="TableCell3"
                                    CssClass="displayNoneMobile">
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="65" ID="TableCell4"
                                    CssClass="displayNoneMobile">
                    
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="TableCell5"
                                    CssClass="displayNoneMobile">
                          
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="65" ID="TableCell6"
                                    CssClass="displayNoneMobile">
                       
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="TableCell7" CssClass="displayNoneMobile">
                         
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="45" ID="TableCell8"
                                    CssClass="displayNoneMobile">
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="75" ID="TableCell9"
                                    CssClass="displayNoneMobile">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="Operating_actions_dropdown" runat="server" CssClass="ul_top">
                                            <asp:ListItem>Actions</asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="Operating_actions_submenu_dropdown" runat="server" CssClass="ul_bottom ac_action_dropdown"
                                            DisplayMode="HyperLink">
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="70" ID="TableCell10"
                                    CssClass="displayNoneMobile">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="operating_folders_dropdown" runat="server" CssClass="ul_top sort_dropdown_width">
                                            <asp:ListItem>Folders</asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="operating_folders_submenu_dropdown" runat="server" CssClass="ul_bottom folder_dropdown"
                                            DisplayMode="HyperLink">
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Operating_Collapse_Panel" runat="server" Height="0px" Width="100%"
                CssClass="collapse">
                <asp:Label runat="server" ID="close_current_folder" Font-Bold="true" ForeColor="Red"
                    Visible="false"><br /><br /><p align="center" class="medium_text">You must Close Current Folder before starting a New Search.</p><br /><br /></asp:Label>
                <asp:Table ID="opCostsTableSearch" Width="100%" CellPadding="3" CellSpacing="0" runat="server" CssClass="displayNoneMobile">
                    <asp:TableRow>
                        <asp:TableCell Width="33%" HorizontalAlign="Left" VerticalAlign="Top" CssClass="model_search_box collapseSearchTable">
                            <asp:Panel runat="server" ID="op_model_panel">
                                <asp:Table ID="Table19" Width="100%" CellPadding="3" CellSpacing="0" runat="server">
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
                        </asp:TableCell>
                        <asp:TableCell Width="67%" HorizontalAlign="Left" VerticalAlign="top" ID="tableCellToggle"
                            CssClass="collapseSearchTable mobileWhiteBackground">

                            <table cellpadding="2" cellspacing="0" class="margin_1">
                                <tr>
                                    <td align="left" valign="top" colspan="2">
                                        <strong>DIRECT COSTS PER HOUR</strong>
                                    </td>
                                    <td align="left" valign="top">Value
                                    </td>
                                    <td align="left" valign="top">Format
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <div id="Div0" runat="server">
                                            <strong>FUEL BURN (Gallons/Hour)</strong>
                                        </div>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:DropDownList ID="fuel_burn_operator_ddl" runat="server" Width="100" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'fuel_burn_txt', 'input');">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>Equals</asp:ListItem>
                                            <asp:ListItem>Less Than</asp:ListItem>
                                            <asp:ListItem>Greater Than</asp:ListItem>
                                            <asp:ListItem>Between</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="fuel_burn_txt" runat="server" Width="100%"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:Label ID="Label3" runat="server" Text="" BackColor="#E0E0E0" CssClass="display_block padding border_format_label">nnnn</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" width="25%">Total Direct Costs:
                                    </td>
                                    <td align="left" valign="top" width="20%">
                                        <asp:DropDownList ID="total_direct_ddl" runat="server" Width="100%" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'total_direct_txt', 'input');">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>Equals</asp:ListItem>
                                            <asp:ListItem>Less Than</asp:ListItem>
                                            <asp:ListItem>Greater Than</asp:ListItem>
                                            <asp:ListItem>Between</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="operating_costs_folder_name" runat="server" CssClass="display_none"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" width="25%">
                                        <asp:TextBox ID="total_direct_txt" runat="server" Width="100%"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" width="25%">
                                        <asp:Label ID="Label14" runat="server" Text="" BackColor="#E0E0E0" CssClass="display_block padding border_format_label">nnnn</asp:Label>
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
                                    <td align="left" valign="top" width="25%">
                                        <strong>CURRENCY</strong>
                                    </td>
                                    <td align="left" valign="top" width="20%">
                                        <asp:DropDownList ID="currencyList" runat="server" Width="100%">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right" valign="top" colspan="2">
                                        <asp:Button ID="operating_search" runat="server" Text="Search" CssClass="button-darker button_width" /><br />
                                        <asp:Button ID="reset" runat="server" Text="Clear Selections" CssClass="font-weight-normal button_width" />
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
        </asp:Panel>
        <asp:Label runat="server" ID="FolderInformation" Visible="false" CssClass="FolderNameBar help_cursor"></asp:Label>
        <asp:Label ID="operating_attention" runat="server" Text="" CssClass="red_text emphasis_text text_align_center small_to_medium_text"></asp:Label>
        <asp:Panel ID="container_operating_costs" runat="server">
            <asp:Label ID="operating_listing_text" runat="server" Text=""></asp:Label>
        </asp:Panel>
    </div>
</asp:Content>
