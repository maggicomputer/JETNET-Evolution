<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="largeGraphDisplay.aspx.vb"
    Inherits="crmWebClient.largeGraphDisplay" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart', 'table'] });
    </script>
    <div class="aircraftContainer">
        <div class="valueSpec viewValueExport Simplistic aircraftSpec">
            <div class="row">
                <div class="sixteen columns">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="" DisplayAfter="500" class="loadingScreenBox">
                        <ProgressTemplate>
                            <span></span>
                            <div class="loader">Loading...</div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                    <asp:UpdatePanel ID="outer_update_panel" runat="server" ChildrenAsTriggers="True"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="HeaderText"></asp:Label>
                            <asp:Literal runat="server" ID="aircraft_information"></asp:Literal>
                            <asp:CheckBox ID="check_avg_asking" runat="server" Visible="false" AutoPostBack="true" Checked="true" Text="Show Asking Prices" />
                            <asp:CheckBox ID="check_avg_sale" runat="server" Visible="false" AutoPostBack="true" Checked="true" Text="Show Sale Prices" />
                            <asp:CheckBox ID="check_eValues" runat="server" Visible="false" AutoPostBack="true" Checked="true" Text="Show eValues" />
                            <asp:Label runat="server" ID="drop_label" Text="Order By" Visible="false"></asp:Label>
                            <asp:DropDownList ID="drop_order_by" runat="server" Visible="false" AutoPostBack="true">
                                <asp:ListItem Text="Serial Number" Value="Serial Number"></asp:ListItem>
                                <asp:ListItem Text="Year DLV" Value="Year"></asp:ListItem>
                            </asp:DropDownList>
                            <div class="boxed_item_padding">
                                <asp:Label runat="server" ID="graph_label" Text=""></asp:Label>
                            </div>
                            <asp:Table ID="buttonsTable" CellPadding="3" CellSpacing="0" Width="100%" class="DetailsBrowseTable"
                                runat="server">
                                <asp:TableRow>
                                    <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="right" VerticalAlign="middle">
                                        <div class="backgroundShade">
                                            <asp:LinkButton ID="create_pdf" Visible="false" Text="" runat="server" class="gray_button float_left noBefore"><strong>Create PDF</strong></asp:LinkButton>
                                            <a href="#" onclick="javascript:load('help.aspx','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"
                                                class="float_left" title="Show Help">
                                                <img src="/images/help-circle.svg" alt="Help" />
                                            </a>
                                            <asp:LinkButton ID="close_button" runat="server" OnClientClick="javascript:window.close();"
                                                CssClass="float_left"><img src="/images/x.svg" alt="Close" /></asp:LinkButton>
                                        </div>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <div class="NotesHeader" style="margin-bottom: 3px;">
                            </div>
                            <br />
                            <asp:Literal ID="debug_output" runat="server"></asp:Literal>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
