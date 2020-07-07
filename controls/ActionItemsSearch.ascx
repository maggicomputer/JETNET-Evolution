<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ActionItemsSearch.ascx.vb" Inherits="crmWebClient.ActionItemsSearch" %>
<asp:Panel ID="search_pnl" runat="server" CssClass="search_pnl"
    Height="110px" Width="98%">


    <asp:Table ID="search_pnl_table" runat="server" Height="110px" Width="100%">
        <asp:TableRow ID="regular_search">
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="10%">
                <asp:Label ID="search_for_lbl" runat="server" Text="Search For"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="25%">
                <asp:TextBox ID="search_for_txt" runat="server" Width="96%"></asp:TextBox></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="15%">
                <asp:DropDownList ID="search_where" runat="server" Width="100%">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="10%">
                <asp:Label ID="search_in" runat="server" Text="Search In"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="30%">
                <asp:DropDownList ID="search_for_cbo" runat="server" Width="100%"  enabled="false">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="10%">
                <asp:ImageButton ID="search_button" runat="server" ImageUrl="../images/search.png" />
                <asp:LinkButton ID="adv_search" runat="server" Font-Size="XX-Small" Font-Underline="False"
                    Font-Italic="True" Visible="false">Advanced Search?</asp:LinkButton>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="action_sort">
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="action_sort_col_one">
                <asp:Label ID="view_lbl" runat="server" Text="View By"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="action_sort_col_two">
              <asp:DropDownList ID="view_cbo" runat="server" Width="100%"></asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:DropDownList ID="display_cbo" runat="server" Width="100%">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:Label ID="order_lbl" runat="server" Text="Order By"></asp:Label>
                <asp:Label ID="start_date_lbl" runat="server" Text="Start Date" Visible="false"></asp:Label>
                </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:DropDownList ID="order_bo" runat="server" Width="50%">
                </asp:DropDownList>
                <asp:TextBox ID="start_date" runat="server" Visible="false" Width="30%"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="start_date" Format="d" PopupButtonID="cal_image2" />
                <asp:Image runat="server" ID="cal_image2" ImageUrl="~/images/final.jpg" visible="false"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="advanced_search">
        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3">&nbsp;</asp:TableCell>
        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
           <asp:Label runat="server" Text="Start/End"></asp:Label>
        </asp:TableCell>
        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
               <asp:TextBox ID="ad_start_date" runat="server" Visible="true" Width="30%"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="ad_start_date" Format="d" PopupButtonID="cal_image4" />
                <asp:Image runat="server" ID="cal_image4" ImageUrl="~/images/final.jpg" visible="true"/>&nbsp;
                <asp:TextBox ID="ad_end_date" runat="server" Visible="true" Width="30%"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="ad_end_date" Format="d" PopupButtonID="cal_image3" />
                <asp:Image runat="server" ID="cal_image3" ImageUrl="~/images/final.jpg" visible="true"/>
                 <asp:CompareValidator ID="CompareValidator2" runat="server" Display="Dynamic" ControlToValidate="ad_end_date"
                    ErrorMessage="<br />* Enter a valid end date" Operator="DataTypeCheck" Type="Date" />
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ad_start_date"
                    ErrorMessage="<br />* Enter a valid start date" Operator="DataTypeCheck" Type="Date"
                    Display="Dynamic" /><br />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
