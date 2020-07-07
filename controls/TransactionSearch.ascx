<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TransactionSearch.ascx.vb"
    Inherits="crmWebClient.TransactionSearch" %>
<asp:Panel ID="search_pnl" runat="server" CssClass="search_pnl"
    Visible="true" Height="215px" Width="98%" DefaultButton="search_button">
    <asp:Label ID="trans_search_attention" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
    <asp:Table ID="search_pnl_table" runat="server" Height="215px" Width="100%">
        <asp:TableRow ID="regular_search">
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="10%">
                <asp:Label ID="search_for_lbl" runat="server" Text="Search For"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="15%">
                <asp:TextBox ID="search_for_txt" runat="server" Width="98%"></asp:TextBox></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="20%">
                <asp:DropDownList ID="search_where" runat="server" Width="100%">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="10%">
                <asp:Label ID="search_in" runat="server" Text="Search In"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="30%">
                <asp:DropDownList ID="search_for_cbo" runat="server" Width="100%" Enabled="false">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="15%">
                <asp:ImageButton ID="search_button" runat="server" ImageUrl="../images/search.png" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="trans_date_search">
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="80">
                <asp:Label ID="trans_type" runat="server" Text="Type"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                <asp:DropDownList ID="trans_type_cbo" runat="server" Width="100%">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="left" VerticalAlign="Top">
                <asp:Label ID="end_date" runat="server" Text="Start/End"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                <asp:TextBox ID="start_date_txt" runat="server" Width="30%"></asp:TextBox>
                <asp:Image runat="server" ID="cal_image2" ImageUrl="~/images/final.jpg" />&nbsp;&nbsp;
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="start_date_txt"
                    Format="d" PopupButtonID="cal_image2" />
                &nbsp;
                <asp:TextBox ID="end_date_txt" runat="server" Width="30%"></asp:TextBox><asp:Image
                    runat="server" ID="cal_image3" ImageUrl="~/images/final.jpg" />
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="end_date_txt"
                    Format="d" PopupButtonID="cal_image3" />
                 <asp:CompareValidator ID="CompareValidator2" runat="server" Display="Dynamic" ControlToValidate="end_date_txt"
                    ErrorMessage="<br />* Enter a valid end date" Operator="DataTypeCheck" Type="Date" />
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="start_date_txt"
                    ErrorMessage="<br />* Enter a valid start date" Operator="DataTypeCheck" Type="Date"
                    Display="Dynamic" /><br />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="trans_search">
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" RowSpan="3"
                ColumnSpan="3">
                <asp:CheckBox ID="default_models" runat="server" Text="Default Models Only" Font-Size="XX-Small"
                    Checked="true" AutoPostBack="true" Visible="false" />
                <asp:ListBox ID="model_cbo" runat="server" SelectionMode="Multiple" Rows="8" Visible="false" Width="100%">
                </asp:ListBox>
                <asp:Label runat="server" ID="model_evo_swap">
                    <asp:CheckBoxList ID="model_type" runat="server" RepeatLayout="Table" Enabled="true"
                        AutoPostBack="true" RepeatDirection="Horizontal">
                        <asp:ListItem Value="Helicopter" Text="Helicopter" Selected="True" />
                        <asp:ListItem Value="Business" Text="Business" Selected="True" />
                        <asp:ListItem Value="Commercial" Text="Commercial" Selected="True" />
                    </asp:CheckBoxList>
                    <table width="100%" cellpadding="3" cellspacing="0">
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
                        </tr>
                    </table>
                </asp:Label>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:Label ID="dataset" runat="server" Text="Data Set"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                <asp:DropDownList ID="subset" runat="server" Width="25%">
                    <asp:ListItem Text="JETNET" Value="J"></asp:ListItem>
                    <asp:ListItem Text="CLIENT" Value="C"></asp:ListItem>
                    <asp:ListItem Text="BOTH" Value="JC" Selected="True"></asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp; Internal Transactions?
                 <asp:DropDownList ID="internal_trans" runat="server" Width="20%">
                    <asp:ListItem Text="BOTH" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="YES" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="NO" Value="N"></asp:ListItem>
                </asp:DropDownList><br />
                 Show AFTT/Engine Times?:
                <asp:CheckBox ID="aftt" runat="server" onclick="javascript:createCookie('aftt',this.checked, 356);" />
                <img src="images/spacer.gif" alt="" width="10" height="1" />
                Awaiting Documentation Only?
                <asp:CheckBox ID="awaiting" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                Range From:</asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3">
                <asp:DropDownList ID="year_start" runat="server" Width="30%">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp; To:
                <asp:DropDownList ID="year_end" runat="server" Width="30%">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:Label ID="Label1" runat="server" Text="Relationships"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                <asp:ListBox ID="relationships" runat="server" Width="100%" SelectionMode="Multiple"
                    Rows="4"></asp:ListBox>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
