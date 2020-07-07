<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="subscriptionInfo.aspx.vb"
    Inherits="crmWebClient.subscriptionInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Subscription Information:</title>
    <link href="common/redesign.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="user_spec" CellPadding="3" runat="server" Width="850px" BorderColor="Black"
            BackColor="White" BorderWidth="1" Font-Size="Small">
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2" BackColor="#436891" BorderColor="#2f4e6f" BorderStyle="Solid"
                    BorderWidth="1px">
                    <asp:Label ID="company_description" runat="server" ForeColor="#fffbe8" Font-Size="Large">Subscription Information</asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow1" runat="server">
                <asp:TableCell ID="subscription_Information" runat="server" VerticalAlign="top" ColumnSpan="2"
                    Font-Size="Small"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="change_subscription" Visible="false">
                <asp:TableCell ColumnSpan="2" BackColor="#436891" BorderColor="#2f4e6f" BorderStyle="Solid"
                    BorderWidth="1px">
                    <asp:Label ID="Label1" runat="server" ForeColor="#fffbe8" Font-Size="Large">Change Subscription</asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="change_subscription2" Visible="false">
                <asp:TableCell ColumnSpan="2">
                    <table width="100%">
                        <tr>
                            <td align="left" valign="top">
                                Helicopter:
                            </td>
                            <td align="left" valign="top">
                                <asp:DropDownList ID="helicopter_session" runat="server">
                                    <asp:ListItem Value="true">TRUE</asp:ListItem>
                                    <asp:ListItem Value="false">FALSE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="left" valign="top">
                                Business:
                            </td>
                            <td align="left" valign="top">
                                <asp:DropDownList ID="business_session" runat="server">
                                    <asp:ListItem Value="true">TRUE</asp:ListItem>
                                    <asp:ListItem Value="false">FALSE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="left" valign="top">
                                Commercial:
                            </td>
                            <td align="left" valign="top">
                                <asp:DropDownList ID="commercial_session" runat="server">
                                    <asp:ListItem Value="true">TRUE</asp:ListItem>
                                    <asp:ListItem Value="false">FALSE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                Turboprops:
                            </td>
                            <td align="left" valign="top">
                                <asp:DropDownList ID="turboprops_session" runat="server">
                                    <asp:ListItem Value="true">TRUE</asp:ListItem>
                                    <asp:ListItem Value="false">FALSE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="left" valign="top">
                                Executive:
                            </td>
                            <td align="left" valign="top">
                                <asp:DropDownList ID="executive_session" runat="server">
                                    <asp:ListItem Value="true">TRUE</asp:ListItem>
                                    <asp:ListItem Value="false">FALSE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="left" valign="top">
                                Jets:
                            </td>
                            <td align="left" valign="top">
                                <asp:DropDownList ID="jets_session" runat="server">
                                    <asp:ListItem Value="true">TRUE</asp:ListItem>
                                    <asp:ListItem Value="false">FALSE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                Aerodex:
                            </td>
                            <td align="left" valign="top">
                                <asp:DropDownList ID="aerodex_session" runat="server">
                                    <asp:ListItem Value="true">TRUE</asp:ListItem>
                                    <asp:ListItem Value="false">FALSE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="left" valign="top">
                                Star Reports:
                            </td>
                            <td align="left" valign="top">
                                <asp:DropDownList ID="star_reports_session" runat="server">
                                    <asp:ListItem Value="true">TRUE</asp:ListItem>
                                    <asp:ListItem Value="false">FALSE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="left" valign="top">
                                Sales Price Index:
                            </td>
                            <td align="left" valign="top">
                                <asp:DropDownList ID="sales_price_session" runat="server">
                                    <asp:ListItem Value="true">TRUE</asp:ListItem>
                                    <asp:ListItem Value="false">FALSE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                Frequency:
                            </td>
                            <td align="left" valign="top">
                                <asp:DropDownList ID="frequency_session" runat="server">
                                    <asp:ListItem Value="Live">Live</asp:ListItem>
                                    <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                    <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="left" valign="top">
                                Server Side Notes:
                            </td>
                            <td align="left" valign="top">
                                <asp:DropDownList ID="server_side_notes_session" runat="server">
                                    <asp:ListItem Value="true">TRUE</asp:ListItem>
                                    <asp:ListItem Value="false">FALSE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="left" valign="top">
                                Evo Display
                            </td>
                            <td align="left" valign="top">
                                <asp:DropDownList ID="evo_session" runat="server">
                                    <asp:ListItem Value="true">TRUE</asp:ListItem>
                                    <asp:ListItem Value="false">FALSE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                            </td>
                            <td align="left" valign="top">
                            </td>
                            <td align="left" valign="top">
                            </td>
                            <td align="left" valign="top">
                            </td>
                            <td align="left" valign="top">
                            </td>
                            <td align="left" valign="top">
                                <asp:Button ID="Submit" runat="server" Text="Swap Values" />
                            </td>
                        </tr>
                    </table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
