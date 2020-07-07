<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="NotesSearch.ascx.vb"
  Inherits="crmWebClient.NotesSearch" %>
<asp:Panel ID="search_pnl" runat="server" CssClass="search_pnl" Height="175px" Width="98%" DefaultButton="search_button">
  <asp:Table ID="search_pnl_table" runat="server" Height="170px" Width="100%" cellpadding="0" CellSpacing="0">
    <asp:TableRow ID="regular_search">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="10%">
        <asp:Label ID="search_for_lbl" runat="server" Text="Search For"></asp:Label></asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="25%">
        <asp:TextBox ID="search_for_txt" runat="server" Width="98%"></asp:TextBox></asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="15%">
        <asp:DropDownList ID="search_where" runat="server" Width="100%">
        </asp:DropDownList>
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="15%">
        <asp:Label ID="search_in" runat="server" Text="Search In"></asp:Label></asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="25">
        <asp:DropDownList ID="search_for_cbo" runat="server" Width="100%" Enabled="false">
        </asp:DropDownList>
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="10%">
        <asp:ImageButton ID="search_button" runat="server" ImageUrl="../images/search.png" />
        <asp:LinkButton ID="adv_search" runat="server" Font-Size="XX-Small" Font-Underline="False"
          Font-Italic="True" Visible="false">Advanced Search?</asp:LinkButton>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="prospect_search_by" Visible="false">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="middle" ColumnSpan="5">
        <span class="float_left">Search By:</span>
        <asp:DropDownList runat="server" ID="prospect_search_by_dropdown" CssClass="float_left"
          AutoPostBack="true">
          <asp:ListItem Value="1">Prospects Assigned to Aircraft</asp:ListItem>
          <asp:ListItem Value="2">Prospects Assigned to Model</asp:ListItem>
          <asp:ListItem Value="3" Selected="True">All Prospects</asp:ListItem>
        </asp:DropDownList>
        <asp:CheckBox runat="server" AutoPostBack="true" ID="showInactiveProspect" Text="Include Inactive Prospects" />
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="prospect_neither_holder" Visible="false">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" RowSpan="5">
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="prospect_ac_sort" Visible="false">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" RowSpan="5"
        BackColor="#dbe7fa" BorderColor="#b2c0d6" BorderStyle="Solid" BorderWidth="1px">
        <asp:ListBox ID="ac_prospect_list" runat="server" Width="105%" Rows="7" Font-Size="10px"
          SelectionMode="Multiple">
          <asp:ListItem>All</asp:ListItem>
        </asp:ListBox>
      </asp:TableCell>
      <asp:TableCell>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="action_sort">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" RowSpan="6"
        BackColor="#dbe7fa" BorderColor="#b2c0d6" BorderStyle="Solid" BorderWidth="1px"
        ID="model_swap_cell">
        <asp:CheckBox ID="default_models" runat="server" Text="Default Models Only&nbsp;&nbsp;&nbsp;"
          Font-Size="XX-Small" Visible="false" Checked="true" AutoPostBack="true" />
        <asp:CheckBox ID="search_by_models" runat="server" Text="Search by Models" Font-Size="XX-Small"
          Visible="true" Checked="false" AutoPostBack="true" />
        <asp:ListBox ID="model_cbo" runat="server" Width="100%" SelectionMode="Multiple"
          Rows="6" Visible="false" CssClass="margin-bottom"></asp:ListBox>
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
                <asp:ListBox ID="type" runat="server" Width="105%" Rows="6" AutoPostBack="true" Font-Size="10px"
                  SelectionMode="Multiple">
                  <asp:ListItem>All</asp:ListItem>
                </asp:ListBox>
              </td>
              <td align="left" valign="top" width="33%">
                Make:<br />
                <asp:ListBox ID="make" runat="server" Width="170%" Rows="6" AutoPostBack="true" Font-Size="10px"
                  SelectionMode="Multiple">
                  <asp:ListItem>All</asp:ListItem>
                </asp:ListBox>
              </td>
              <td align="left" valign="top" width="33%">
                Model:<br />
                <asp:ListBox ID="model" runat="server" Width="100%" Rows="6" AutoPostBack="false"
                  Font-Size="10px" SelectionMode="Multiple">
                  <asp:ListItem>All</asp:ListItem>
                </asp:ListBox>
              </td>
            </tr>
          </table>
        </asp:Label>
        <asp:Table runat="server" ID="ac_details_table" Width="100%">
          <asp:TableRow ID="TableRow3">
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
              <asp:DropDownList ID="ac_search_field" runat="server" Width="100%" Enabled="true"
                Visible="true">
                <asp:ListItem Text="Ser#/Reg#" Value="1"></asp:ListItem>
                <asp:ListItem Text="Ser#" Value="2"></asp:ListItem>
                <asp:ListItem Text="Reg#" Value="3"></asp:ListItem>
                <asp:ListItem Text="Aircraft ID" Value="4"></asp:ListItem>
              </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
              <asp:DropDownList ID="ac_search_field_operator" runat="server" Width="100%">
                <asp:ListItem Value="1">Begins With</asp:ListItem>
                <asp:ListItem Value="2">Anywhere</asp:ListItem>
                <asp:ListItem Value="3">Equals</asp:ListItem>
              </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
              <asp:TextBox ID="ac_search_field_text" runat="server" Width="100%"></asp:TextBox>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="action_sort_col_one">
        <asp:Label ID="view_lbl" runat="server" Text=""></asp:Label><asp:Label ID="Label1"
          runat="server" Text="Start/End"></asp:Label></asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="action_sort_col_two"
        ColumnSpan="2">
        <asp:DropDownList ID="document_status" runat="server" Width="100%" Visible="false">
        </asp:DropDownList>
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
    <asp:TableRow ID="prospect_category_row" Visible="false" runat="server">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
      Category:
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:DropDownList runat="server" ID="prospect_category" Width="100%">
          <asp:ListItem Selected="True" Value="0">Please Select One</asp:ListItem>
        </asp:DropDownList>
      </asp:TableCell>
    </asp:TableRow>
       <asp:TableRow Visible="false" ID="folderTypeRow">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        View notes for all</asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="4">
        <asp:DropDownList runat="server" ID="FolderType" Width="100px" AutoPostBack="true">
          <asp:ListItem Value="">Please Select</asp:ListItem>
          <asp:ListItem Value="2">Contacts</asp:ListItem>
          <asp:ListItem Value="1">Companies</asp:ListItem>
          <asp:ListItem Value="3">Aircraft</asp:ListItem>
        </asp:DropDownList>
        in the
        <asp:DropDownList runat="server" ID="listOfFolders" Width="100px" Enabled="false"
          CssClass="display_disable">
          <asp:ListItem>N/A</asp:ListItem>
        </asp:DropDownList>
        folder
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TableRow1">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Staff
      <asp:CheckBox ID="include_inactives" runat="server" Text="Include Inactive" AutoPostBack="true"  />
      </asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:DropDownList ID="display_cbo" runat="server" Width="100%">
        </asp:DropDownList>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TableRow2">
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="80">
        <asp:Label ID="order_lbl" runat="server" Text="Order By"></asp:Label></asp:TableCell>
      <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
        <asp:DropDownList ID="order_bo" runat="server" Width="100%">
        </asp:DropDownList>
      </asp:TableCell>
    </asp:TableRow>
  </asp:Table>
</asp:Panel>
