<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Wanted_Listing.aspx.vb" Inherits="crmWebClient.Wanted_Listing"
    MasterPageFile="~/EvoStyles/EvoTheme.Master" StylesheetTheme="Evo" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/EvoStyles/EvoTheme.Master" %>
<%@ Register Src="controls/viewTypeMakeModel.ascx" TagName="viewTMMDropDowns" TagPrefix="evo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="DataGridShadowContainer">
        <div class="valueSpec Simplistic aircraftSpec">
            <asp:Panel ID="Wanted_Criteria" runat="server" Visible="true">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td align="left" valign="top" class="dark_header" width="100%">
                            <asp:Table ID="Table13" runat="server" Width="100%" CellPadding="0" CellSpacing="0" CssClass="padding_table">
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="20" ID="wanted_help_text" CssClass="evoHelp">
                        <a href="#">Help</a> </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="90" ID="wanted_search_expand_text">
                                        <asp:Panel ID="wanted_Control_Panel" runat="server" Width="100%">
                                            <asp:Image ID="wanted_ControlImage" runat="server" ImageUrl="../Images/search_expand.jpg" />
                                        </asp:Panel>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" ID="wanted_results_text">
                                        <asp:Label ID="wanted_criteria_results" runat="server" Text="Label"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="wanted_sort_by_text">
                        Sort By: 
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="70" ID="wanted_sort_by_dropdown">
                                        <div class="action_dropdown_container">
                                            <asp:BulletedList ID="wanted_sort_dropdown" runat="server" CssClass="ul_top sort_dropdown_width">
                                                <asp:ListItem>Make/Model</asp:ListItem>
                                            </asp:BulletedList>
                                            <asp:BulletedList ID="wanted_sort_submenu_dropdown" runat="server" CssClass="ul_bottom sort_dropdown" DisplayMode="LinkButton"
                                                OnClick="submenu_dropdown_Click">
                                                <asp:ListItem>Make/Model</asp:ListItem>
                                                <asp:ListItem>Interested Party</asp:ListItem>
                                                <asp:ListItem>Date Listed</asp:ListItem>
                                            </asp:BulletedList>
                                        </div>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="65" ID="wanted_per_page_text">
                        Per Page:&nbsp;
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="wanted_per_page_dropdown_">
                                        <div class="action_dropdown_container">
                                            <asp:BulletedList ID="wanted_per_page_dropdown" runat="server" CssClass="ul_top per_page_width">
                                                <asp:ListItem Value="10">10</asp:ListItem>
                                            </asp:BulletedList>
                                            <asp:BulletedList ID="wanted_per_page_submenu_dropdown" runat="server" CssClass="ul_bottom per_page_dropdown" OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
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
                                    <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="65" ID="wanted_go_to_text">
                        Go To:&nbsp;
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="wanted_go_to_dropdown_">
                                        <div class="action_dropdown_container">
                                            <asp:BulletedList ID="wanted_go_to_dropdown" runat="server" CssClass="ul_top per_page_width">
                                                <asp:ListItem>1</asp:ListItem>
                                            </asp:BulletedList>
                                            <asp:BulletedList ID="wanted_go_to_submenu_dropdown" runat="server" CssClass="ul_bottom per_page_dropdown" DisplayMode="LinkButton">
                                            </asp:BulletedList>
                                        </div>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="75" ID="wanted_action_dropdown">
                                        <div class="action_dropdown_container">
                                            <asp:BulletedList ID="wanted_actions_dropdown" runat="server" CssClass="ul_top">
                                                <asp:ListItem>Actions</asp:ListItem>
                                            </asp:BulletedList>
                                            <asp:BulletedList ID="wanted_actions_submenu_dropdown" runat="server" CssClass="ul_bottom ac_action_dropdown" DisplayMode="HyperLink">
                                            </asp:BulletedList>
                                        </div>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="70" ID="TableCell1">
                                        <div class="action_dropdown_container">
                                            <asp:BulletedList ID="wanted_folders_dropdown" runat="server" CssClass="ul_top sort_dropdown_width">
                                                <asp:ListItem>Folders</asp:ListItem>
                                            </asp:BulletedList>
                                            <asp:BulletedList ID="wanted_folders_submenu_dropdown" runat="server" CssClass="ul_bottom folder_dropdown" DisplayMode="HyperLink">
                                            </asp:BulletedList>
                                        </div>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="180" ID="wanted_results_text_">
                                        <asp:Label ID="wanted_paging" runat="server" CssClass="criteria_text criteria_spacer">
                                            <asp:ImageButton ID="wanted_previous_all" ImageUrl="../images/previous_all.png" runat="server" Visible="false" CommandName="previous_all" />&nbsp;<asp:ImageButton
                                                ID="wanted_previous" CommandName="previous" ImageUrl="../images/previous_listing.png" Visible="false" runat="server" />&nbsp;<asp:Label
                                                    ID="wanted_record_count" runat="server">Showing 25 - 50</asp:Label>&nbsp;<asp:ImageButton ID="wanted_next_" ImageUrl="../images/next_listing.png"
                                                        CommandName="next" runat="server" />&nbsp;<asp:ImageButton ID="wanted_next_all" ImageUrl="~/images/next_all.png" runat="server"
                                                            CommandName="next_all" /></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </td>
                    </tr>
                </table>
                <cc1:CollapsiblePanelExtender ID="wanted_PanelCollapseEx" runat="server" TargetControlID="wanted_Collapse_Panel" Collapsed="true"
                    ExpandControlID="wanted_Control_Panel" ImageControlID="wanted_ControlImage" CollapsedText="New Search" ExpandedText="Hide Search"
                    ExpandedImage="../Images/spacer.gif" CollapsedImage="../Images/search_expand.jpg" CollapseControlID="wanted_Control_Panel"
                    Enabled="True">
                </cc1:CollapsiblePanelExtender>
                <asp:Panel ID="wanted_Collapse_Panel" runat="server" Height="0px" Width="100%" CssClass="collapse">
                    <asp:Label runat="server" ID="close_current_folder" Font-Bold="true" ForeColor="Red" Visible="false"><br /><br /><p align="center" class="medium_text">You must Close Current Folder before starting a New Search.</p><br /><br /></asp:Label>
                    <asp:Table ID="Table4" Width="100%" CellPadding="3" CellSpacing="0" runat="server">
                        <asp:TableRow>
                            <asp:TableCell Width="33%" HorizontalAlign="Left" VerticalAlign="Top">
                                <asp:Panel runat="server" ID="wanted_model_search_box" CssClass="model_search_box">
                                    <asp:Panel ID="wanted_make_model_panel" runat="server">
                                        <evo:viewTMMDropDowns ID="ViewTMMDropDowns" runat="server" />

                                        <script language="javascript" type="text/javascript">
                    refreshTypeMakeModelByCheckBox("", "", <%= isHeliOnlyProduct.tostring.tolower%>,<%= productCodeCount.tostring%>);
                                        </script>

                                    </asp:Panel>
                                </asp:Panel>
                            </asp:TableCell>
                            <asp:TableCell Width="66%" HorizontalAlign="Left" VerticalAlign="Top">
                                <asp:Table ID="Table16" Width="100%" CellPadding="3" CellSpacing="0" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="90">
                               Date Listed:
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="30">
                                From: </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="60">
                                            <asp:TextBox ID="wanted_from" runat="server" Width="100%"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="20">
                                To:</asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="60">
                                            <asp:TextBox ID="wanted_to" runat="server" Width="100%"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="right" VerticalAlign="top" RowSpan="3">
                                            <asp:Button ID="wanted_search" runat="server" Text="Search" CssClass="button-darker button_width" CausesValidation="true"
                                                ValidationGroup="Numeric" /><br />
                                            <asp:Button ID="reset" runat="server" Text="Clear Selections" CssClass="font-weight-normal button_width" CausesValidation="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top" ColumnSpan="2"></asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top" ColumnSpan="3">
                                            <asp:CompareValidator runat="server" ID="ValidateWantedFrom" ControlToValidate="wanted_from" Operator="DataTypeCheck" Type="Date"
                                                ValidationGroup="Numeric" SetFocusOnError="true" Font-Bold="true" Text="*Incorrect Format (From Date)<br />" Display="Dynamic"
                                                Enabled="true"></asp:CompareValidator>
                                            <asp:CompareValidator runat="server" ID="ValidateWantedTo" ControlToValidate="wanted_to" Font-Bold="true" Operator="DataTypeCheck"
                                                Type="Date" ValidationGroup="Numeric" SetFocusOnError="true" Text="*Incorrect Format (To Date)" Display="Dynamic" Enabled="true"></asp:CompareValidator>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                               Interested Party:
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                             
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="150" ColumnSpan="3">
                                            <asp:TextBox ID="wanted_interested" runat="server" Width="150"></asp:TextBox>
                                            <asp:TextBox ID="wanted_folder_name" runat="server" CssClass="display_none"></asp:TextBox>
                                            <asp:TextBox ID="amwant_id" runat="server" CssClass="display_none"></asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                               Placed By:
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                             
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:DropDownList ID="wanted_placed_by" runat="server">
                                                <asp:ListItem Text="All">All</asp:ListItem>
                                                <asp:ListItem Text="Dealer">Dealer</asp:ListItem>
                                                <asp:ListItem Text="End User">End User</asp:ListItem>
                                            </asp:DropDownList>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </asp:Panel>
            <asp:Label runat="server" ID="FolderInformation" Visible="false" CssClass="FolderNameBar help_cursor"></asp:Label>
            <asp:Label ID="wanted_attention" runat="server" Text="" CssClass="red_text emphasis_text text_align_center small_to_medium_text"></asp:Label>
            <asp:DataGrid runat="server" ID="Results" CssClass="formatTable blue" AutoGenerateColumns="false" Width="100%" AllowCustomPaging="false" AllowPaging="true"
                Visible="false">
                <Columns>
                    <asp:TemplateColumn HeaderText="MAKE" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "amod_make_name").ToString & " "%> <span class="text_underline"><%#crmWebClient.DisplayFunctions.WriteModelLink(DataBinder.Eval(Container.DataItem, "amwant_amod_id"), DataBinder.Eval(Container.DataItem, "amod_model_name").ToString, True)%></span>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="DATE LISTED" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                            <a href="/WantedDetails.aspx?id=<%#DataBinder.Eval(Container.DataItem, "amwant_id").ToString%>" class="text_underline" target="_blank">
                                <%#crmWebClient.clsGeneral.clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "amwant_listed_date").ToString)%></a>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="INTERESTED PARTY" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                            <%#crmWebClient.DisplayFunctions.WriteDetailsLink(0, DataBinder.Eval(Container.DataItem, "comp_id"), 0, 0, True, DataBinder.Eval(Container.DataItem, "comp_name").ToString, "", "")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="YEAR RANGE" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "amwant_start_year").ToString%>-<%#DataBinder.Eval(Container.DataItem, "amwant_end_year").ToString%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="MAX PRICE" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                            <%#crmWebClient.clsGeneral.clsGeneral.ConvertIntoThousands(DataBinder.Eval(Container.DataItem, "amwant_max_price").ToString)%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="MAX AFTT" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "amwant_max_aftt").ToString%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="DAMAGE" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
        </div>
    </div>
</asp:Content>
