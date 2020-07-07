<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ContactSearch.ascx.vb"
    Inherits="crmWebClient.ContactSearch" %>
<asp:Panel ID="search_pnl" runat="server" CssClass="search_pnl" Height="127px" Width="98%" DefaultButton="search_button">
    <asp:Label ID="contact_search_attention" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
    <asp:Table ID="search_pnl_table" runat="server" Height="97px" Width="100%">
        <asp:TableRow ID="regular_search">
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="15%">
                <asp:Label ID="search_for_lbl" runat="server" Text="First/Last Name"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="35%">
                <asp:TextBox ID="first_name" runat="server" Width="45%"></asp:TextBox>&nbsp;<asp:TextBox
                    ID="last_name" runat="server" Width="45%"></asp:TextBox></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="20%">
                <asp:DropDownList ID="search_where" runat="server" Width="100%">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="10%">
                <asp:Label ID="search_in" runat="server" Text="Search In"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="20%">
                <asp:DropDownList ID="search_for_cbo" runat="server" Width="100%" Enabled="false">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:ImageButton ID="search_button" runat="server" ImageUrl="../images/search.png"
                    OnClick="search_Click" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="activity_view" Visible="true">
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="comp_name" Visible="true">Company Name</asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Visible="true" ID="comp_name_second">
                <asp:TextBox ID="comp_name_txt" runat="server" Width="94%"></asp:TextBox></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:DropDownList ID="status_cbo" runat="server" Width="100%">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="order_by_text">Order By</asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:DropDownList ID="ordered_by" runat="server" Width="100%">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="second_advanced" Visible="true">
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Email Address:</asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="comp_email_address" runat="server" Width="94%"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Data Subset:</asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:DropDownList ID="subset" runat="server" Width="100%">
                    <asp:ListItem Text="JETNET" Value="J"></asp:ListItem>
                    <asp:ListItem Text="CLIENT" Value="C"></asp:ListItem>
                    <asp:ListItem Text="BOTH" Value="B" Selected="True"></asp:ListItem>
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
         <asp:TableRow ID="TableRow1" Visible="true">
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Phone Number:</asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="phone" runat="server" Width="94%"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
