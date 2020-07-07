<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="market_search.ascx.vb"
    Inherits="crmWebClient.market_search" %>
<asp:Panel ID="market" runat="server" CssClass="search_pnl" Visible="true" DefaultButton="search"
    Height="190px" Width="98%">
    <asp:Label ID="market_search_attention" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
    <asp:Table ID="Table1" runat="server" Height="190px" Width="100%">
        <asp:TableRow ID="TableRow1">
            <asp:TableCell HorizontalAlign="left" VerticalAlign="Top" Width="15%">
                <asp:Label ID="Label5" runat="server" Text="Activity from:"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="35%"><asp:panel ID="evo_swap" runat="server" Visible="true">
                <asp:TextBox ID="ad_start_date" runat="server" Visible="true" Width="30%"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="ad_start_date"
                    Format="d" PopupButtonID="cal_image4" />
                <asp:Image runat="server" ID="cal_image4" ImageUrl="~/images/final.jpg" Visible="true" />&nbsp;To&nbsp;
                <asp:TextBox ID="ad_end_date" runat="server" Visible="true" Width="30%"></asp:TextBox> 
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="ad_end_date"
                    Format="d" PopupButtonID="cal_image3" />
                <asp:Image runat="server" ID="cal_image3" ImageUrl="~/images/final.jpg" Visible="true" />
                <asp:CompareValidator ID="CompareValidator2" runat="server" Display="Dynamic" ControlToValidate="ad_end_date"
                    ErrorMessage="<br />* Enter a valid end date" Operator="DataTypeCheck" Type="Date" />
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ad_start_date"
                    ErrorMessage="<br />* Enter a valid start date" Operator="DataTypeCheck" Type="Date"
                    Display="Dynamic" /><br /></asp:panel>
                <asp:DropDownList ID="market_time" runat="server" Visible="false"> 
                    <asp:ListItem Value="1">1 Day</asp:ListItem>
                    <asp:ListItem Value="7">7 Days</asp:ListItem>
                    <asp:ListItem Value="31">One Month</asp:ListItem>
                    <asp:ListItem Value="93">Three Months</asp:ListItem>
                    <asp:ListItem Value="186">Six Months</asp:ListItem>
                    <asp:ListItem Value="279">Nine Months</asp:ListItem>
                    <asp:ListItem Value="365">Twelve Months</asp:ListItem>
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="10%">
                <asp:Label ID="Label3" runat="server" Text="Search In"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="30%">
                <asp:DropDownList ID="search_for_cbo" runat="server" Width="100%" Enabled="false">
                </asp:DropDownList>
                <asp:DropDownList ID="search_where" Visible="false" runat="server" Width="40%" AutoPostBack="true">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="right" VerticalAlign="Top" Width="10%">
                <asp:ImageButton ID="search" runat="server" ImageUrl="../images/search.png" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="aircraft_search">
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" RowSpan="3"
                ColumnSpan="2">
                <asp:CheckBox ID="default_models" runat="server" Text="Default Models Only" Font-Size="XX-Small"
                    Visible="false" Checked="true" AutoPostBack="true" />
                <asp:ListBox ID="model_cbo" runat="server" Width="100%" SelectionMode="Multiple" Rows="8"
                    Visible="false"></asp:ListBox>
                <asp:Label runat="server" ID="model_evo_swap">
                    <asp:CheckBoxList ID="model_type" runat="server" RepeatLayout="Table" Enabled="true"
                        AutoPostBack="true" RepeatDirection="Horizontal">
                        <asp:ListItem Value="Helicopter" Text="Helicopter" Selected="True" />
                        <asp:ListItem Value="Business" Text="Business" Selected="True" />
                        <asp:ListItem Value="Commercial" Text="Commercial" Selected="True" />
                    </asp:CheckBoxList>
                    <table width="100%" cellpadding="3" cellspacing="0">
                        <tr>
                            <td align="left" valign="top" width="33%">
                                Type:<br />
                                <asp:ListBox ID="type" runat="server" Width="100%" Rows="7" AutoPostBack="true"
                                    Font-Size="10px" SelectionMode="Multiple">
                                    <asp:ListItem>All</asp:ListItem>
                                </asp:ListBox>
                            </td>
                            <td align="left" valign="top" width="33%">
                                Make:<br />
                                <asp:ListBox ID="make" runat="server" Width="100%" Rows="7" AutoPostBack="true"
                                    Font-Size="10px" SelectionMode="Multiple">
                                    <asp:ListItem>All</asp:ListItem>
                                </asp:ListBox>
                            </td>
                            <td align="left" valign="top" width="33%">
                                Model:<br />
                                <asp:ListBox ID="model" runat="server" Width="100%" Rows="7" AutoPostBack="false"
                                    Font-Size="10px" SelectionMode="Multiple">
                                    <asp:ListItem>All</asp:ListItem>
                                </asp:ListBox>
                            </td>
                        </tr>
                    </table>
                </asp:Label>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                Category: 
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                <asp:ListBox ID="categories" runat="server" Width="100%" SelectionMode="Multiple"
                    Rows="3" AutoPostBack="true"></asp:ListBox>
                <br />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Height="15">
                Type: 
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                <asp:ListBox ID="types" runat="server" Width="100%" SelectionMode="Multiple" Rows="4">
                </asp:ListBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
