<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="OpportunitiesSearch.ascx.vb"
    Inherits="crmWebClient.OpportunitesSearch" %>

<asp:Panel ID="search_pnl" runat="server" CssClass="search_pnl"
    Height="80px" Width="98%">
    <asp:Table ID="search_pnl_table" runat="server" Height="80px" Width="100%">
        <asp:TableRow ID="regular_search">
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="10%">
                <asp:Label ID="search_for_lbl" runat="server" Text="Search For"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="25%">
                <asp:TextBox ID="search_for_txt" runat="server" Width="97%"></asp:TextBox></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="15%">
                <asp:DropDownList ID="search_where" runat="server" Width="100%">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="20%">
                <asp:Label ID="search_in" runat="server" Text="Search In"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="25%">
                <asp:DropDownList ID="search_for_cbo" runat="server" Width="100%" Enabled="false">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="15%">
                <asp:ImageButton ID="search_button" runat="server" ImageUrl="../images/search.png" />
                <asp:LinkButton ID="adv_search" runat="server" Font-Size="XX-Small" Font-Underline="False"
                    Font-Italic="True" Visible="false">Advanced Search?</asp:LinkButton>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="aircraft_search">
               <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        Category: 
        </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:DropDownList ID="notes_cat" runat="server" Width="100%">
                <asp:ListItem Value="0" Text="All"></asp:ListItem>
                </asp:DropDownList>
            </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
    
        </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="TableCell1">
                <asp:Label ID="Label1" runat="server" Text=""></asp:Label><asp:Label ID="Label2"
                    runat="server" Text="Date Range"></asp:Label></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="TableCell2" ColumnSpan="2">
                <asp:TextBox ID="ad_start_date" runat="server" Visible="true" Width="30%"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="ad_start_date"
                    Format="d" PopupButtonID="cal_image4" />
                <asp:Image runat="server" ID="cal_image4" ImageUrl="~/images/final.jpg" Visible="true" />&nbsp;
                 <asp:TextBox ID="ad_end_date" runat="server" Visible="true" Width="30%"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="ad_end_date"
                    Format="d" PopupButtonID="cal_image3" />
                <asp:Image runat="server" ID="cal_image3" ImageUrl="~/images/final.jpg" Visible="true" />
                <asp:CompareValidator ID="CompareValidator2" runat="server" Display="Dynamic" ControlToValidate="ad_end_date"
                    ErrorMessage="<br />* Enter a valid end date" Operator="DataTypeCheck" Type="Date" />
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ad_start_date"
                    ErrorMessage="<br />* Enter a valid start date" Operator="DataTypeCheck" Type="Date"
                    Display="Dynamic" />
            </asp:TableCell>
        </asp:TableRow>
         <asp:TableRow ID="TableRow1">
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:DropDownList ID="display_cbo" runat="server" Width="100%">
                </asp:DropDownList>
            </asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
    
        </asp:TableCell>
          <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
    Status
        </asp:TableCell>
             <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:DropDownList ID="opportunity_status" runat="server" Width="100%">
                <asp:ListItem Value="O">Open</asp:ListItem>
                 <asp:ListItem Value="C">Closed</asp:ListItem>
                  <asp:ListItem Value="">All</asp:ListItem>
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>