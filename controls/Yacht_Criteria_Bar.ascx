<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Yacht_Criteria_Bar.ascx.vb"
    Inherits="crmWebClient.Yacht_Criteria_Bar" %>
<table width="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td align="left" valign="top" class="dark_header" width="100%">
            <asp:Table runat="server" Width="100%" CellPadding="0" CellSpacing="0" CssClass="padding_table">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="20" ID="help_text">
                        <img src="../images/info_white.png" class="float_left" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="80" ID="search_expand_text">
                        <asp:Panel ID="Control_Panel" runat="server" Width="100%">
                            Search
                            <asp:Image ID="ControlImage" runat="server" ImageUrl="../Images/expand.jpg" />
                        </asp:Panel>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" ID="results_text">
                        <asp:Label ID="criteria_results" runat="server" Text="Label">798 Results</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="sort_by_text">
                        Sort By: 
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="70" ID="sort_by_dropdown">
                        <div class="action_dropdown_container">
                            <asp:BulletedList ID="sort_dropdown" runat="server" CssClass="ul_top sort_dropdown_width">
                                <asp:ListItem>Model/Ser#</asp:ListItem>
                            </asp:BulletedList>
                            <asp:BulletedList ID="sort_submenu_dropdown" runat="server" CssClass="ul_bottom sort_dropdown"
                                OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                                <asp:ListItem>Model/Ser#</asp:ListItem>
                                <asp:ListItem>List Date</asp:ListItem>
                                <asp:ListItem>AFTT</asp:ListItem>
                                <asp:ListItem>Status</asp:ListItem>
                            </asp:BulletedList>
                        </div>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="65" ID="per_page_text">
                        Per Page:&nbsp;
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="per_page_dropdown_">
                        <div class="action_dropdown_container">
                            <asp:BulletedList ID="per_page_dropdown" runat="server" CssClass="ul_top per_page_width">
                                <asp:ListItem Value="10">10</asp:ListItem>
                            </asp:BulletedList>
                            <asp:BulletedList ID="per_page_submenu_dropdown" runat="server" CssClass="ul_bottom per_page_dropdown"
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
                            </asp:BulletedList>
                        </div>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="65" ID="go_to_text">
                        Go To:&nbsp;
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="go_to_dropdown_">
                        <div class="action_dropdown_container">
                            <asp:BulletedList ID="go_to_dropdown" runat="server" CssClass="ul_top per_page_width">
                                <asp:ListItem>1</asp:ListItem>
                            </asp:BulletedList>
                            <asp:BulletedList ID="go_to_submenu_dropdown" runat="server" CssClass="ul_bottom per_page_dropdown"
                                OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                            </asp:BulletedList>
                        </div>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="45" ID="view_dropdown_">
                        <div class="action_dropdown_container">
                            <asp:BulletedList ID="view_dropdown" runat="server" CssClass="ul_top thumnail_view_bullet">
                                <asp:ListItem></asp:ListItem>
                            </asp:BulletedList>
                            <asp:BulletedList ID="view_submenu_dropdown" runat="server" CssClass="ul_bottom thumbnail"
                                OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                                <asp:ListItem>Listing</asp:ListItem>
                                <asp:ListItem>Gallery</asp:ListItem>
                            </asp:BulletedList>
                        </div>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="75" ID="action_dropdown">
                        <div class="action_dropdown_container">
                            <asp:BulletedList ID="actions_dropdown" runat="server" CssClass="ul_top">
                                <asp:ListItem>Actions</asp:ListItem>
                            </asp:BulletedList>
                            <asp:BulletedList ID="actions_submenu_dropdown" runat="server" CssClass="ul_bottom"
                                DisplayMode="HyperLink" Target="_blank" OnClick="submenu_dropdown_Click">
                                <asp:ListItem>Save As</asp:ListItem>
                                <asp:ListItem Value="../evo_exporter.aspx">Export/Report</asp:ListItem>
                                <asp:ListItem>Map Aircraft</asp:ListItem>
                                <asp:ListItem>Summary</asp:ListItem>
                            </asp:BulletedList>
                        </div>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="180" ID="results_text_">
                        <asp:Label ID="paging" runat="server" CssClass="criteria_text criteria_spacer">
                            <asp:ImageButton ID="previous_all" ImageUrl="../images/previous_all.png" runat="server"
                                Visible="false" />&nbsp;<asp:ImageButton ID="previous" ImageUrl="../images/previous_listing.png"
                                    Visible="false" runat="server" />&nbsp;<asp:Label ID="record_count" runat="server">Showing 25 - 50</asp:Label>&nbsp;<asp:ImageButton
                                        ID="next_" ImageUrl="../images/next_listing.png" runat="server" />&nbsp;<asp:ImageButton
                                            ID="next_all" ImageUrl="~/images/next_all.png" runat="server" /></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </td>
    </tr>
</table>
<cc1:CollapsiblePanelExtender ID="PanelCollapseEx" runat="server" TargetControlID="Collapse_Panel"
    Collapsed="true" ExpandControlID="Control_Panel" ImageControlID="ControlImage"
    ExpandedImage="../Images/root.jpg" CollapsedImage="../Images/expand.jpg" CollapseControlID="Control_Panel"
    Enabled="True">
</cc1:CollapsiblePanelExtender>
<asp:Panel ID="Collapse_Panel" runat="server" Height="0px" Width="100%" CssClass="collapse">
    <asp:Table Width="100%" CellPadding="3" CellSpacing="0" runat="server">
        <asp:TableRow>
            <asp:TableCell Width="50%" HorizontalAlign="Left" VerticalAlign="Top">
                <asp:UpdatePanel runat="server" ID="search_update_panel">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="model_search_box" CssClass="model_search_box">
                            <br />
                            <asp:Table ID="Table1" Width="100%" CellPadding="3" CellSpacing="0" runat="server">
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="33%">
                                        Brand:
                                        <br />
                                        <asp:ListBox ID="brand" runat="server" Width="100%" Rows="13" AutoPostBack="true"
                                            Font-Size="10px" SelectionMode="Multiple">
                                            <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                        </asp:ListBox>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="33%">
                                        Model:
                                        <br />
                                        <asp:ListBox ID="model" runat="server" Width="100%" Rows="13" AutoPostBack="false"
                                            Font-Size="10px" SelectionMode="Multiple">
                                            <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                        </asp:ListBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:TableCell>
            <asp:TableCell Width="50%" HorizontalAlign="Left" VerticalAlign="Top">
                <asp:Table runat="server" Width="100%" CellPadding="3">
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="50">
                          Hull MFR ID #
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="50">
                           From/To:
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="210px">
                            <asp:TextBox ID="serial_number_from" runat="server" Width="90px">
                            </asp:TextBox>/
                            <asp:TextBox ID="serial_number_to" runat="server" Width="90px">
                            </asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" RowSpan="2" CssClass="light_border">
                            <asp:CheckBox ID="do_not_search_alt_ser_num" runat="server" Text="Don't Search Alt. Ser#"
                                Font-Size="9px" /><br />
                            <asp:CheckBox ID="do_not_search_prev_reg" runat="server" Text="Don't Search Prev. Reg#"
                                Font-Size="9px" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                           Radio Call Sign 
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="30px">
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="90px">
                            <asp:TextBox ID="registration_number" runat="server" Width="90px">
                            </asp:TextBox>&nbsp;
                            <asp:CheckBox ID="registration_number_exact_match" runat="server" Text="Exact Match"
                                Font-Size="9px" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                           Year:
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="30px"></asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            <asp:DropDownList ID="yacht_year" runat="server">
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="90px">
                          
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            Category:
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="30px"></asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            <asp:DropDownList ID="yacht_category" runat="server" Width="80%">
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="90px">
                          
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Table ID="Table2" Width="100%" CellPadding="3" CellSpacing="0" runat="server"
                    CssClass="market_search_box">
                    <asp:TableRow>
                        <asp:TableCell ID="aerodex_toggle" HorizontalAlign="left" VerticalAlign="top" Width="43%"
                            RowSpan="2" CssClass="padding_market">
                            <span class="extra">Market Status:</span>
                            <asp:ListBox ID="market" runat="server" Width="100%" Rows="9" Font-Size="10px" SelectionMode="Single">
                                <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                <asp:ListItem Value="For Sale">For Sale</asp:ListItem>
                                <asp:ListItem Value="For Sale/Lease">For Sale/Lease</asp:ListItem>
                                <asp:ListItem Value="For Sale/Trade">For Sale/Trade</asp:ListItem>
                                <asp:ListItem Value="For Sale on Exclusive">For Sale on Exclusive</asp:ListItem>
                                <asp:ListItem Value="For Sale Not on Exclusive">For Sale Not on Exclusive</asp:ListItem>
                                <asp:ListItem Value="Not For Sale">Not For Sale</asp:ListItem>
                                <asp:ListItem Value="Lease">Lease</asp:ListItem>
                            </asp:ListBox>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="23%" CssClass="padding_market">
                            <span class="extra">Lifecycle:</span>
                            <asp:ListBox ID="lifecycle" runat="server" Width="100%" Rows="4" Font-Size="10px"
                                SelectionMode="Multiple">
                                <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                <asp:ListItem Value="1">In Production</asp:ListItem>
                                <asp:ListItem Value="2">New-With MFR</asp:ListItem>
                                <asp:ListItem Value="3">In Operation</asp:ListItem>
                                <asp:ListItem Value="4">Retired</asp:ListItem>
                                <asp:ListItem Value="4">In Storage</asp:ListItem>
                            </asp:ListBox>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="33%" CssClass="padding_market">
                            <span class="extra">Ownership:</span>
                            <asp:ListBox ID="ownership" runat="server" Width="100%" Rows="4" Font-Size="10px"
                                SelectionMode="Multiple">
                                <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                <asp:ListItem Value="W">Wholly Owned</asp:ListItem>
                                <asp:ListItem Value="S">Shared</asp:ListItem>
                                <asp:ListItem Value="F">Fractional</asp:ListItem>
                            </asp:ListBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="33%">
                            Previously Owned:
                            <asp:DropDownList ID="previously_owned" runat="server" Width="100%" Font-Size="10px">
                                <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                <asp:ListItem Value="Y">Yes</asp:ListItem>
                                <asp:ListItem Value="N">No</asp:ListItem>
                            </asp:DropDownList>
                            Lease Status:
                            <asp:DropDownList ID="lease_status" runat="server" Width="100%" Font-Size="10px">
                                <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                <asp:ListItem Value="Y">Leased</asp:ListItem>
                                <asp:ListItem Value="N">Not Leased</asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="right" VerticalAlign="top" Width="33%">
                            <asp:Button ID="search" runat="server" Text="Search Yachts" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>

<script type="text/javascript">
function ShowBar(type, visibility) { 
var vis = "visible";
var dropdown = null;

    if (visibility == false) { 
        vis = "hidden";
    }
    
//    if (type == "action") { 
//        dropdown = document.getElementById("<%= actions_submenu_dropdown.ClientID %>")
//    }else if (type == "sort") { 
//        dropdown = document.getElementById("<%= sort_submenu_dropdown.ClientID %>")
//    } else if (type == "view") { 
//        dropdown = document.getElementById("<%= view_submenu_dropdown.ClientID %>")
//    } else if (type == "page") { 
//        dropdown = document.getElementById("<%= per_page_submenu_dropdown.ClientID %>")
//    } else if (type == "go") { 

//    }  

    dropdown = document.getElementById(type)
   // alert(dropdown);
    if (dropdown != null) {
        dropdown.style.visibility = vis;
    }

}

</script>

