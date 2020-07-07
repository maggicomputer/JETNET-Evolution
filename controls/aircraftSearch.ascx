<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="aircraftSearch.ascx.vb"
  Inherits="crmWebClient.aircraftSearch" %>
<%@ Register TagPrefix="obout" Namespace="OboutInc.Flyout2" Assembly="obout_Flyout2_NET" %>
<cc1:CollapsiblePanelExtender ID="PanelCollapseEx" runat="server" TargetControlID="search_pnl"
  Collapsed="false" ExpandControlID="Control_Panel" ImageControlID="ControlImage"
  CollapsedText="New Search" ExpandedText="Hide Search" ExpandedImage="~/images/crm_SearchHeader_blank.png"
  CollapsedImage="~/images/crm_SearchHeader.png" CollapseControlID="Control_Panel"
  Enabled="True">
</cc1:CollapsiblePanelExtender>
<asp:Table runat="server" ID="containerTable" CssClass="dark_header crmSearchBarBackground">
  <asp:TableRow>
    <asp:TableCell runat="server">
      <asp:Panel ID="Control_Panel" runat="server">
        <asp:Image ID="ControlImage" runat="server" ImageUrl="~/images/search_expand.jpg" />
      </asp:Panel>
    </asp:TableCell>
    <asp:TableCell runat="server">
      <asp:Label ID="NewSearch" runat="server" Text="" CssClass="float_right" Visible="false"> <a href='/listing_air.aspx?clear=true' class="newSearchLink"><img src="/images/crm_SearchHeaderNew.png" alt="New Search" border="0" /></a></asp:Label>
    </asp:TableCell>
  </asp:TableRow>
</asp:Table>
<asp:Panel ID="search_pnl" runat="server" CssClass="search_pnl" Visible="true" Height="190px"
  Width="98%" DefaultButton="search_button">
  <asp:Table ID="search_pnl_table" runat="server" CellPadding="0" Height="190px" Width="100%">
    <asp:TableRow ID="regular_search">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="top" ColumnSpan="3" RowSpan="7"
        Width="50%">
        <asp:Label ID="model_lbl" runat="server" Text="Model" Visible="false"></asp:Label>
        <asp:CheckBox ID="default_models" runat="server" Text="Default Models Only" Font-Size="XX-Small"
          Checked="true" AutoPostBack="true" Visible="false" />
        <asp:ListBox ID="model_cbo" runat="server" SelectionMode="Multiple" Rows="10" Visible="false"
          Width="100%"></asp:ListBox>
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
                <asp:ListBox ID="type" runat="server" Width="100%" Rows="7" AutoPostBack="true" Font-Size="10px"
                  SelectionMode="Multiple">
                  <asp:ListItem>All</asp:ListItem>
                </asp:ListBox>
              </td>
              <td align="left" valign="top" width="33%">
                Make:<br />
                <asp:ListBox ID="make" runat="server" Width="100%" Rows="7" AutoPostBack="true" Font-Size="10px"
                  SelectionMode="Multiple">
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
    </asp:TableRow>
    <asp:TableRow ID="aircraft_search">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="10%" ID="market_status_cell">
        <asp:Label ID="market_status_lbl" runat="server" Text="Market Status"></asp:Label></asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="23%" ID="market_status_dropdown_cell">
        <asp:DropDownList ID="market_status_cbo" runat="server" Width="100%">
        </asp:DropDownList>
      </asp:TableCell>
      <asp:TableCell ID="types_of_owners_cell" HorizontalAlign="right" VerticalAlign="Top"
        ColumnSpan="2" Width="14%">
        <asp:DropDownList ID="types_of_owners" runat="server" Width="100%">
          <asp:ListItem Text="All Companies" Value=""></asp:ListItem>
          <asp:ListItem Text="All Owners" Value="all" Selected="True"></asp:ListItem>
          <asp:ListItem Text="Whole Owners" Value="whole"></asp:ListItem>
          <asp:ListItem Text="Operators" Value="operators"></asp:ListItem>
          <asp:ListItem Text="Fractional Owners" Value="fractional"></asp:ListItem>
          <asp:ListItem Text="Shared Owners" Value="shared"></asp:ListItem>
        </asp:DropDownList>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:Label runat="server" Text="Sort By"></asp:Label></asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:DropDownList ID="sort_by_cbo" runat="server" Width="60%">
        </asp:DropDownList>
        <asp:DropDownList ID="sort_method_cbo" runat="server" Width="35%">
          <asp:ListItem Selected="True">Asc</asp:ListItem>
          <asp:ListItem>Desc</asp:ListItem>
        </asp:DropDownList>
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="exclusive">
        <asp:Label ID="exclusive_label" runat="server" Text="">Lease?:</asp:Label></asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="60">
        <asp:DropDownList ID="on_exclusive" runat="server" Width="100%">
          <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
          <asp:ListItem Text="No" Value="N"></asp:ListItem>
          <asp:ListItem Text="N/A" Value="" Selected="True"></asp:ListItem>
        </asp:DropDownList>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell Visible="true" HorizontalAlign="Left" VerticalAlign="Top">Lifecycle:</asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:DropDownList ID="ac_lifecycle_dropdown" runat="server" Width="100%">
          <asp:ListItem Selected="True" Value="">All</asp:ListItem>
          <asp:ListItem Value="1">In Production</asp:ListItem>
          <asp:ListItem Value="2">New-With MFR</asp:ListItem>
          <asp:ListItem Value="3">In Operation</asp:ListItem>
          <asp:ListItem Value="4">Retired</asp:ListItem>
          <asp:ListItem Value="5">In Storage</asp:ListItem>
        </asp:DropDownList>
      </asp:TableCell>
      <asp:TableCell Visible="true" HorizontalAlign="Left" VerticalAlign="Top">Ownership:</asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:DropDownList ID="ac_ownership_type" runat="server">
          <asp:ListItem Selected="True" Value="">All</asp:ListItem>
          <asp:ListItem Value="W">Wholly Owned</asp:ListItem>
          <asp:ListItem Value="S">Shared</asp:ListItem>
          <asp:ListItem Value="F">Fractional</asp:ListItem>
        </asp:DropDownList>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell Visible="true" ID="exclusive_cell_label" HorizontalAlign="Left" VerticalAlign="Top">Exclusive?:</asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="4">
        <asp:DropDownList ID="on_lease" runat="server" Width="20%">
          <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
          <asp:ListItem Text="No" Value="N"></asp:ListItem>
          <asp:ListItem Text="N/A" Value="" Selected="True"></asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="year_from_label" runat="server">&nbsp;&nbsp; Year from:</asp:Label>
        <asp:DropDownList ID="year_start" runat="server" Width="15%">
        </asp:DropDownList>
        &nbsp;To:&nbsp;
        <asp:DropDownList ID="year_end" runat="server" Width="15%">
        </asp:DropDownList>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell HorizontalAlign="left" VerticalAlign="Top" ColumnSpan="3">
        Show AFTT/Engine Times?:
        <asp:CheckBox ID="aftt" runat="server" onclick="javascript:createCookie('aftt',this.checked, 356);" />
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="right" VerticalAlign="bottom" RowSpan="2">
        <table width="100%" cellpadding="3" cellspacing="0">
          <tr>
            <td align="right" valign="top">
              <asp:ImageButton ID="search_button" runat="server" ImageUrl="../images/search.png" /><br />
              <asp:LinkButton ID="adv_search" runat="server" Font-Size="smaller" Font-Underline="False"
                Font-Italic="True">Advanced?</asp:LinkButton>
            </td>
          </tr>
        </table>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell HorizontalAlign="left" VerticalAlign="Top"><span>Show:</span></asp:TableCell>
      <asp:TableCell HorizontalAlign="left" VerticalAlign="Top" ColumnSpan="2">
        <asp:DropDownList ID="aircraftNotes" runat="server" Width="165" onchange="toggleNotesDateToggle(this)">
          <asp:ListItem Text="Aircraft without Notes" Value="2"></asp:ListItem>
          <asp:ListItem Text="Aircraft with Notes" Value="1"></asp:ListItem>
          <asp:ListItem Text="Aircraft with or without Notes" Value="0" Selected="True"></asp:ListItem>
        </asp:DropDownList>
        <span id="placerHold" runat="server">
          <img src="images/spacer.gif" width="55" height="17" /></span><span id="aircraftNotesDateToggle"
            class="display_none" runat="server">
            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="notesDate"
              ErrorMessage="&nbsp;*" Operator="DataTypeCheck" Type="Date" Text="&nbsp;*" Display="static"
              ToolTip="*Valid Date Needed" Font-Bold="true" Font-Size="14px" />Since:&nbsp;<asp:TextBox
                runat="server" ID="notesDate" Width="55"></asp:TextBox>
            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="notesDate"
              Format="d" PopupButtonID="cal_image" />
            <asp:Image runat="server" ID="cal_image" ImageUrl="~/images/final.jpg" Visible="true" /></span>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:DropDownList ID="search_field" runat="server" Width="100%" Enabled="true" Visible="true">
          <asp:ListItem Text="Ser#/Reg#" Value="1"></asp:ListItem>
          <asp:ListItem Text="Ser#" Value="2"></asp:ListItem>
          <asp:ListItem Text="Reg#" Value="3"></asp:ListItem>
          <asp:ListItem Text="Aircraft ID" Value="4"></asp:ListItem>
        </asp:DropDownList>
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:DropDownList ID="search_where" runat="server" Width="100%">
        </asp:DropDownList>
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:TextBox ID="search_for_txt" runat="server" Width="94%" TabIndex="1"></asp:TextBox>
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="subset_label">Data Subset:</asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3">
        <asp:DropDownList ID="subset" runat="server" Width="70px">
          <asp:ListItem Text="JETNET" Value="J"></asp:ListItem>
          <asp:ListItem Text="CLIENT" Value="C"></asp:ListItem>
          <asp:ListItem Text="BOTH" Value="JC" Selected="True"></asp:ListItem>
        </asp:DropDownList>
        <asp:CheckBox ID="MergeList" runat="server" Text="Exclude Jetnet Records where a client record exists"  />
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="base" Visible="false">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="7"><br /><strong><u>Aircraft Base Location:</u></strong></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="base1" Visible="false">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Airport Name:</asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
        <asp:TextBox ID="airport_name" runat="server" Width="80%"></asp:TextBox></asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">IATA/ICAO:</asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:TextBox ID="iata_code" runat="server" Width="40%"></asp:TextBox>/<asp:TextBox
          ID="icao_code" runat="server" Width="40%"></asp:TextBox></asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
        City:
        <asp:TextBox ID="city" runat="server" Width="82%"></asp:TextBox></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server" Visible="false" ID="basecountry">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Country:</asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
        <asp:ListBox runat="server" ID="country" Width="80%" Rows="5" AutoPostBack="True"
          SelectionMode="Multiple"></asp:ListBox>
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="state_text" Visible="false">State:</asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3">
        <asp:ListBox ID="state" runat="server" SelectionMode="Multiple" Rows="5" Width="100%"
          Visible="false">
          <asp:ListItem Text="SELECT ONE" Value=""></asp:ListItem>
        </asp:ListBox>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="base2" Visible="false">
    </asp:TableRow>
    <asp:TableRow ID="advanced_search_categories" Visible="false">
      <asp:TableCell ColumnSpan="7" HorizontalAlign="Left" VerticalAlign="Top">
        <strong><u>Aircraft Custom Data:</u></strong>&nbsp;&nbsp;<asp:ImageButton ID="infoButton1"
          runat="server" Height="15" ImageUrl="~/images/info.png" Visible="true" />
        <obout:Flyout ID="Flyout1" runat="server" AttachTo="infoButton1" Position="TOP_RIGHT"
          Visible="true" Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true">
          <p class="info_box">
            Use _ (underscore) character to search for all records with anything in field and
            use % as a wildcard character in searches.<br />
            <br />
          </p>
        </obout:Flyout>
        <table width="100%" cellpadding="3" cellspacing="0">
          <tr>
            <td align="left" valign="top" width="150">
              <asp:Label runat="server" ID="custom_pref_name1"></asp:Label>
            </td>
            <td align="left" valign="top">
              <asp:TextBox runat="server" ID="custom_pref_text1" Width="98%"></asp:TextBox>
            </td>
            <td align="left" valign="top" width="150">
              <asp:Label runat="server" ID="custom_pref_name2"></asp:Label>
            </td>
            <td align="left" valign="top">
              <asp:TextBox runat="server" ID="custom_pref_text2" Width="98%"></asp:TextBox>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:Label runat="server" ID="custom_pref_name3"></asp:Label>
            </td>
            <td align="left" valign="top">
              <asp:TextBox runat="server" ID="custom_pref_text3" Width="98%"></asp:TextBox>
            </td>
            <td align="left" valign="top">
              <asp:Label runat="server" ID="custom_pref_name4"></asp:Label>
            </td>
            <td align="left" valign="top">
              <asp:TextBox runat="server" ID="custom_pref_text4" Width="98%"></asp:TextBox>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:Label runat="server" ID="custom_pref_name5"></asp:Label>
            </td>
            <td align="left" valign="top">
              <asp:TextBox runat="server" ID="custom_pref_text5" Width="98%"></asp:TextBox>
            </td>
            <td align="left" valign="top">
              <asp:Label runat="server" ID="custom_pref_name6"></asp:Label>
            </td>
            <td align="left" valign="top">
              <asp:TextBox runat="server" ID="custom_pref_text6" Width="98%"></asp:TextBox>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:Label runat="server" ID="custom_pref_name7"></asp:Label>
            </td>
            <td align="left" valign="top">
              <asp:TextBox runat="server" ID="custom_pref_text7" Width="98%"></asp:TextBox>
            </td>
            <td align="left" valign="top">
              <asp:Label runat="server" ID="custom_pref_name8"></asp:Label>
            </td>
            <td align="left" valign="top">
              <asp:TextBox runat="server" ID="custom_pref_text8" Width="98%"></asp:TextBox>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:Label runat="server" ID="custom_pref_name9"></asp:Label>
            </td>
            <td align="left" valign="top">
              <asp:TextBox runat="server" ID="custom_pref_text9" Width="98%"></asp:TextBox>
            </td>
            <td align="left" valign="top">
              <asp:Label runat="server" ID="custom_pref_name10"></asp:Label>
            </td>
            <td align="left" valign="top">
              <asp:TextBox runat="server" ID="custom_pref_text10" Width="98%"></asp:TextBox>
            </td>
          </tr>
        </table>
      </asp:TableCell>
    </asp:TableRow>
  </asp:Table>
</asp:Panel>
<asp:Label ID="ac_search_attention" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>