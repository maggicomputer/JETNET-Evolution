<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="performance_specs.aspx.vb"
    Inherits="crmWebClient.performance_specs" MasterPageFile="~/main_site.Master" %>

<%@ MasterType VirtualPath="~/main_site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="main_pnl" CssClass="search_pnl" runat="server" BackColor="#D4FAE9"
        Width="98%">
        <table width="400" cellpadding="3" cellspacing="0">
            <tr>
                <td align="left" valign="top">
                    <asp:Label runat="server" ID="model_crm_swap">
                        <asp:Label ID="model_lbl" runat="server" Text="Model" Visible="false"></asp:Label>
                        <asp:CheckBox ID="default_models" runat="server" Text="Default Models Only" Font-Size="XX-Small"
                            Checked="true" AutoPostBack="true" Visible="true" /><br />
                        <asp:ListBox ID="model_cbo" runat="server" SelectionMode="Multiple" Rows="9" Visible="true"
                            Width="399"></asp:ListBox>
                    </asp:Label>
                    <asp:Label runat="server" ID="model_evo_swap">
                        <asp:CheckBoxList ID="model_type" runat="server" RepeatLayout="Table" Enabled="true"
                            AutoPostBack="true" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Helicopter" Text="Helicopter" Selected="True" />
                            <asp:ListItem Value="Business" Text="Business" Selected="True" />
                            <asp:ListItem Value="Commercial" Text="Commercial" Selected="True" />
                        </asp:CheckBoxList>
                        <table width="400" cellpadding="3" cellspacing="0">
                            <tr>
                                <td align="left" valign="top">
                                    Type:<br />
                                    <asp:ListBox ID="type" runat="server" Width="105px" Rows="7" AutoPostBack="true"
                                        Font-Size="10px" SelectionMode="Multiple">
                                        <asp:ListItem>All</asp:ListItem>
                                    </asp:ListBox>
                                </td>
                                <td align="left" valign="top">
                                    Make:<br />
                                    <asp:ListBox ID="make" runat="server" Width="170px" Rows="7" AutoPostBack="true"
                                        Font-Size="10px" SelectionMode="Multiple">
                                        <asp:ListItem>All</asp:ListItem>
                                    </asp:ListBox>
                                </td>
                                <td align="left" valign="top">
                                    Model:<br />
                                    <asp:ListBox ID="model" runat="server" Width="100px" Rows="7" AutoPostBack="false"
                                        Font-Size="10px" SelectionMode="Multiple">
                                        <asp:ListItem>All</asp:ListItem>
                                    </asp:ListBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </asp:Label>
                    <td>
                        <asp:Button ID="model_search" name="model_search" runat="server" Text="Search" />
                    </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="performance_specs_label" name="performance_specs_label" runat="server"></asp:Label>
</asp:Content>
