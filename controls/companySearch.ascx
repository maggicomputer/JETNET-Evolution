<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="companySearch.ascx.vb"
  Inherits="crmWebClient.companySearch" %>
<%@ Register TagPrefix="obout" Namespace="OboutInc.Flyout2" Assembly="obout_Flyout2_NET" %>

<asp:Panel ID="search_pnl" runat="server" CssClass="search_pnl" Height="155px" Width="98%" DefaultButton="search_button">
  <asp:Label ID="company_search_attention" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
  <asp:Table ID="search_pnl_table" runat="server" Height="155px" Width="100%">
    <asp:TableRow ID="regular_search">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="15%">
        <asp:Label ID="search_for_lbl" runat="server" Text="Search For"></asp:Label></asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="25%">
        <asp:TextBox ID="search_for_txt" runat="server" Width="98%" TabIndex="0"></asp:TextBox></asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="20%">
        <asp:DropDownList ID="search_where" runat="server" Width="98%">
          <asp:ListItem Selected="true" Value="2">Begins With</asp:ListItem>
          <asp:ListItem Value="1">Anywhere</asp:ListItem>
        </asp:DropDownList>
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="15%">
        <asp:Label ID="search_in" runat="server" Text="Search In"></asp:Label></asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="20%">
        <asp:DropDownList ID="search_for_cbo" runat="server" Width="98%" Enabled="false">
        </asp:DropDownList>
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="5%">
        <asp:ImageButton ID="search_button" runat="server" ImageUrl="../images/search.png"
          OnClick="search_Click" /><br />
        <asp:LinkButton ID="adv_search" runat="server" Font-Size="Smaller" Font-Underline="False"
          Font-Italic="True" Visible="false">Advanced Search?</asp:LinkButton>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TableRow1">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:Label ID="Company_Phone" runat="server" Text="Phone #"></asp:Label></asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:TextBox ID="company_phone_number" runat="server" Width="98%"></asp:TextBox></asp:TableCell>
      <asp:TableCell HorizontalAlign="right" VerticalAlign="Top" ColumnSpan="3">
        <asp:CheckBox ID="MergeList" runat="server" Text="Exclude Jetnet Records where a client record exists" Visible="false" />
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="activity_view" Visible="true">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="stat_lbl">Status:</asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
        <asp:DropDownList ID="status_cbo" runat="server" Width="55%">
        </asp:DropDownList>
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="sub_lbl">Data Subset:</asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:DropDownList ID="subset" runat="server" Width="100%" AutoPostBack="true">
          <asp:ListItem Text="JETNET" Value="J"></asp:ListItem>
          <asp:ListItem Text="CLIENT" Value="C"></asp:ListItem>
          <asp:ListItem Text="BOTH" Value="JC" Selected="True"></asp:ListItem>
        </asp:DropDownList>
        <asp:CheckBox ID="show_all" runat="server" Text="Show all?" Visible="false" />
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="location_search" Visible="true">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Country:</asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:DropDownList ID="country" runat="server" Width="100%" AutoPostBack="True">
        </asp:DropDownList>
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" RowSpan="2">
        <asp:ListBox ID="state" runat="server" SelectionMode="Multiple" Rows="5" Visible="false">
          <asp:ListItem Text="SELECT ONE" Value=""></asp:ListItem>
        </asp:ListBox>
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" RowSpan="2">Types:</asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" RowSpan="2">
        <asp:DropDownList ID="types_of_owners" runat="server" Width="100%">
          <asp:ListItem Text="All Companies" Value="" Selected="True"></asp:ListItem>
          <asp:ListItem Text="All Owners" Value="all"></asp:ListItem>
          <asp:ListItem Text="Whole Owners" Value="whole"></asp:ListItem>
          <asp:ListItem Text="Operators" Value="operators"></asp:ListItem>
          <asp:ListItem Text="Fractional Owners" Value="fractional"></asp:ListItem>
          <asp:ListItem Text="Shared Owners" Value="shared"></asp:ListItem>
        </asp:DropDownList>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:Label runat="server" ID="city_label" Visible="false">City:</asp:Label></asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:TextBox ID="city_textbox" runat="server" Width="100%" Visible="false"></asp:TextBox>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="fields" Visible="true">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Category Search:</asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:DropDownList ID="special_field_cbo" runat="server" Width="100%" AutoPostBack="true">
        </asp:DropDownList>
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:TextBox ID="special_field_txt" runat="server" Width="100%" Visible="false"></asp:TextBox>&nbsp;<asp:ImageButton
          ID="Button1" Height="15" runat="server" ImageUrl="~/images/info.png" Visible="false" /><obout:Flyout
            ID="Flyout1" runat="server" AttachTo="Button1" Position="TOP_RIGHT" Visible="false"
            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true">
            <p class="info_box">
              Use _ (underscore) character to search for all records with anything in field and
              use % as a wildcard character in searches.<br />
              <br />
            </p>
          </obout:Flyout>
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="4">
        <asp:CheckBox ID="special_field_view" runat="server" Visible="false" Text="Display Special Field?" />
      </asp:TableCell>
    </asp:TableRow>
  </asp:Table>
</asp:Panel>
