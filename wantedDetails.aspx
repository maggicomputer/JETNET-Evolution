<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wantedDetails.aspx.vb"
    Inherits="crmWebClient.wantedDetails" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="sixteen columns">
        <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width='96%' runat="server"
            class="DetailsBrowseTable">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="center" VerticalAlign="middle">
                    <div class="backgroundShade">
                        <div class="dropdownSettings-sub">
                            <asp:LinkButton ID="LinkButton1" runat="server"><img src="images/menu.svg" alt="Menu" /></asp:LinkButton>
                            <div class="dropdown-content-sub" style="right: 40px;">
                                <div class="row">
                                    <div class="twelve columns">
                                        <ul>
                                            <li>
                                                <asp:LinkButton ID="view_folders" runat="server" Visible="true" OnClick="ViewWantedFolders" CssClass="float_left"><strong>Folders</strong></asp:LinkButton></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <a href="#" onclick="javascript:window.close();">
                            <img src="images/x.svg" alt="Close" /></a>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <div class="companyContainer">
            <asp:Panel ID="contentClass" runat="server" Width="100%" HorizontalAlign="Center"
                CssClass="valueSpec viewValueExport Simplistic aircraftSpec">
                <div class="grid">
                    <div class="sixteen columns">
                        <asp:Label ID="top_section" runat="server" Visible="false"></asp:Label>
                    </div>

                    <div class="grid-item specialHeadingTable">
                        <div class="Box">
                        <div class="subHeader padding_left emphasisColor">
                            <asp:Label ID="make_model" runat="server" Text=""></asp:Label>
                        </div>
                        <asp:Label ID="content_wanted" runat="server"></asp:Label></div>
                    </div>

                    <asp:UpdatePanel ID="folders_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="folders_tab" Visible="false">
                                <div class="grid-item specialHeadingTable">
                                    <div class="Box">
                                        <div class="subHeader padding_left emphasisColor">
                                            FOLDERS
                                        </div>
                                        <asp:Label ID="folders_label" runat="server" Text="" CssClass="small_panel_height"></asp:Label>

                                        <asp:LinkButton runat="server" ID="closeFolders" CssClass="float_right padding" OnClick="ViewWantedFolders"
                                            Visible="false">Close Folders</asp:LinkButton><div class="clear"></div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <div class="grid-item specialHeadingTable">
                        <asp:Label ID="company_name_label" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="company_label" runat="server" Text=""></asp:Label>
                        <asp:Label ID="about_label" runat="server" Text=""></asp:Label>
                        <asp:Label ID="company_address_label" runat="server" Text="" Visible="false"></asp:Label>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            loadMasonry()
        });

    </script>
</asp:Content>
