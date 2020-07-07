<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Maintenance.aspx.vb" Inherits="crmWebClient.Maintenance"
  MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <style type="text/css">
    .table
    {
      color: #333;
      font-family: Helvetica, Arial, sans-serif;
      width: 640px;
      border-collapse: collapse;
      border-spacing: 0;
    }
    .table td, .table th
    {
      border-top: 1px solid #000;
      border-right: 1px solid #000;
      border-bottom: 0px solid transparent;
      border-left: 0px solid transparent;
      transition: all 0.3s; /* Simple transition for hover effect */
      text-align: left;
      padding: 5px 5px 10px 5px;
    }
    .table th
    {
      background: #666; /* Darken header a bit */
      font-weight: bold;
      color: #FFF;
      padding: 5px 5px 10px 5px;
    }
    .table td
    {
      background: #FAFAFA;
    }
    /* Cells in even rows (2,4,6...) are one color */.table tr:nth-child(even) td
    {
      background: #F1F1F1;
    }
    /* Cells in odd rows (1,3,5...) are another (excludes header cells)  */.table tr:nth-child(odd) td
    {
      background: #FEFEFE;
    }
    .element
    {
      text-decoration: none;
      padding: 5px;
    }
    .element:before
    {
      content: "\f040";
      font-family: FontAwesome;
      color: #000;
      font-size: 15px;
      text-decoration: none;
    }
    .save:before
    {
      content: '';
    }
    .save
    {
      background-color: #43ac6a !important;
      border-color: #3c9a5f !important;
      font-size: 13px !important;
      color: #ffffff !important;
      width: 30px;
    }
    .save:hover, .save:focus, .save.focus, .save:active
    {
      color: #ffffff !important;
      background-color: #358753 !important;
      border-color: #2b6e44 !important;
    }
    .add:before
    {
      content: "";
    }
    .add
    {
      background-color: #0179ff !important;
      border-color: #022574 !important;
      font-size: 13px !important;
      color: #ffffff !important;
      width: 30px;
    }
    .add:hover, .add:focus, .add.focus, .add:active
    {
      color: #ffffff !important;
      background-color: #003abc !important;
      border-color: #022574 !important;
    }
     .remove{background-color:#C52B00 !important;border-color:#C52B00 !important;}
    .remove:before
    {
      color: #ffffff;
      content: "\f014";
    }
    .cancel:before
    {
      color: #DA0000;
      content: "\f00d";
    }
    .element
    {
      display: inline-block;
      margin-bottom: 0;
      font-weight: normal;
      text-align: center;
      vertical-align: middle;
      -ms-touch-action: manipulation;
      touch-action: manipulation;
      cursor: pointer;
      background-image: none;
      border: 1px solid transparent;
      white-space: nowrap;
      padding: 3px 5px;
      font-size: 15px;
      line-height: 1.4;
      border-radius: 0;
      -webkit-user-select: none;
      -moz-user-select: none;
      -ms-user-select: none;
      user-select: none;
      color: #333333;
      background-color: #e7e7e7;
      border-color: #cccccc;
    }
    .element:hover, .element:focus, .element.focus, .element:active, .selected
    {
      color: #333333;
      background-color: #cecece;
      border-color: #adadad;
    }
    .element:active, .selected
    {
      outline: 0;
      background-image: none;
      -webkit-box-shadow: inset 0 3px 5px rgba(0,0,0,0.125);
      box-shadow: inset 0 3px 5px rgba(0,0,0,0.125);
    }
    .tinyFormat
    {
      display: inline-block;
      padding: 3px;
      font-size: 7px;
      color: rgb(125, 121, 121);
      font-weight: bold;
      text-transform: lowercase;
    }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
<asp:Panel runat="server"  ID="avionics_panel" Visible="false" > 
<table cellpadding="3" cellspacing="0"> 
<tr valign='top'><td align='left'>ID:</td><td align='left'><asp:Label ID="avitem_id" runat="server" Text="" ></asp:Label></td></tr>
<tr valign='top'><td align='left'>Name:</td><td align='left'><asp:TextBox ID="avitem_name" runat="server" Text="" Columns="40" ></asp:TextBox></td></tr>
<tr valign='top'><td align='left'>Mfr Name:</td><td align='left'><asp:DropDownList ID="avitem_mfr_name" runat="server" ></asp:DropDownList></td></tr>
<tr valign='top'><td align='left'>Item Name:</td><td align='left'><asp:TextBox ID="avitem_item_name" runat="server" Text="" Columns="40"></asp:TextBox></td></tr>
<tr valign='top'><td align='left'>Description:</td><td align='left'><asp:TextBox ID="avitem_Description" runat="server" Text="" TextMode="MultiLine" Columns="40"  Rows="15"></asp:TextBox></td></tr>
<tr valign='top'><td align='left'>Web Address:</td><td align='left'><asp:TextBox ID="avitem_web_address" runat="server" Text=""  Columns="40"></asp:TextBox></td></tr>
<tr valign='top'><td align='left'>Research Description:</td><td align='left'><asp:TextBox ID="avitem_research_description" runat="server" Text="" TextMode="MultiLine" Columns="40"  Rows="15"></asp:TextBox></td></tr>
<tr valign='top'><td align='left'>Upgrade Cost:</td><td align='left'><asp:TextBox ID="avitem_upgrade_cost" runat="server" Text=""  Columns="3"></asp:TextBox></td></tr>
<tr valign='top'><td align='left'>Upgrade Down Time:</td><td align='left'><asp:TextBox ID="avitem_upgrade_downtime" runat="server" Text="" Columns="40" ></asp:TextBox></td></tr>
</table>


</asp:Panel>
<asp:Panel runat="server" ID="maint_panel" Visible="true" >
  <p class="DetailsBrowseTable">
    <span class="backgroundShade">
      <asp:LinkButton runat="server" ID="TryToAddRow" CssClass="gray_button noBefore"><strong>Add New Maintenance/Inspection Item</strong></asp:LinkButton><a
        href="#" class="gray_button float_right" onclick="javascript:window.close();" runat="server" id="closeButton"><strong>Close</strong></a></span>
  </p>
  <h1 runat="server" id="AircraftInfo" class="padding_table">
  </h1>
  <table width="98%" align='center'><tr><td width="22%">
  <asp:Label runat="server" ID="attention" ForeColor="Red" Font-Bold="true"></asp:Label> 
  <asp:Label runat="server" ID="view_all_maint"></asp:Label>
  </td><td align="right" width="76%">
  <asp:Label runat="server" ID="model_specs"></asp:Label>
  </td></tr></table>
  <asp:GridView ID="maintenanceInfo" AutoGenerateColumns="false" runat="server" BackColor="White"
    Width="99%" CellPadding="3" ShowFooter="false" HorizontalAlign="Center" CssClass="table">
    <RowStyle ForeColor="#000066" VerticalAlign="Top" />
    <FooterStyle BackColor="White" ForeColor="#000066" VerticalAlign="Top" />
    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
    <Columns>
      <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="top"
        ItemStyle-Width="100px">
        <ItemTemplate>
          <asp:LinkButton runat="server" CommandName="Edit" CssClass="element"></asp:LinkButton>
        </ItemTemplate>
        <EditItemTemplate>
          <asp:LinkButton runat="server" CommandName="Remove" CssClass="element remove"  OnClientClick="return confirm('Do you really want to remove this record?');"></asp:LinkButton>
          <asp:LinkButton runat="server" CommandName="Cancel" CssClass="element edit selected"></asp:LinkButton>
          <asp:LinkButton runat="server" CommandName="Update" CssClass="element save">Save</asp:LinkButton>
        </EditItemTemplate>
        <FooterTemplate>
          <asp:LinkButton runat="server" CommandName="CancelAdd" CssClass="element cancel"></asp:LinkButton>
          <asp:LinkButton runat="server" CommandName="Insert" CssClass="element save">Save</asp:LinkButton> 
        </FooterTemplate>
      </asp:TemplateField>
      <asp:TemplateField HeaderText="Maintenance/Inspections" ItemStyle-Wrap="false" ItemStyle-Width="250px">
        <ItemTemplate>
          <%#DataBinder.Eval(Container.DataItem, "acmaint_name")%>
        </ItemTemplate>
        <EditItemTemplate>
          <asp:Label runat="server" ID="acmaint_id" Text='<%#DataBinder.Eval(Container.DataItem, "acmaint_id")%>'
            CssClass="display_none"></asp:Label>
          <asp:TextBox runat="server" ID="acmaint_name_textbox" Visible="true" CssClass="display_none"
            Text='<%#DataBinder.Eval(Container.DataItem, "acmaint_name") + "|" + DataBinder.Eval(Container.DataItem, "mitem_duration").tostring%>'></asp:TextBox>
          <asp:TextBox runat="server" ID="acmaint_date_type" Visible="true" Text='<%#DataBinder.Eval(Container.DataItem, "acmaint_date_type")%>'
            CssClass="display_none"></asp:TextBox>
          <asp:DropDownList runat="server" ID="acmaint_name" TabIndex="1" Width="160px">
          </asp:DropDownList>
          <asp:DropDownList runat="server" ID="acmaint_by_date" Width="85px" TabIndex="2">
          <asp:ListItem Value="M">By Month</asp:ListItem>
            <asp:ListItem Value="D">By Date</asp:ListItem> 
            <asp:ListItem Value="Y">By Year</asp:ListItem>
          </asp:DropDownList>
          
        </EditItemTemplate>
        <FooterTemplate>
          <asp:Label runat="server" ID="acmaint_id" CssClass="display_none">0</asp:Label>
          <asp:TextBox runat="server" ID="acmaint_name_textbox" Visible="true" CssClass="display_none"
            Text='<%#DataBinder.Eval(Container.DataItem, "acmaint_name")%>'></asp:TextBox>
          <asp:TextBox runat="server" ID="acmaint_date_type" Visible="true" CssClass="display_none"
            Text='M'></asp:TextBox>
          <asp:DropDownList runat="server" ID="acmaint_name" TabIndex="1" Width="160px">
          </asp:DropDownList>
          <asp:DropDownList runat="server" ID="acmaint_by_date" TabIndex="2" Width="85px">
           <asp:ListItem Value="M">By Month</asp:ListItem>
            <asp:ListItem Value="D">By Date</asp:ListItem> 
            <asp:ListItem Value="Y">By Year</asp:ListItem>
          </asp:DropDownList>
        </FooterTemplate>
      </asp:TemplateField>
      <asp:TemplateField ItemStyle-Width="130px" ItemStyle-Wrap="false">
        <HeaderTemplate>
          <b class="text_underline help_cursor" title="Complied With or Completed" alt="Complied with or Completed">
            C/W</b> Date
        </HeaderTemplate>
        <ItemTemplate>
          <%#FormattingMaintenanceDate(DataBinder.Eval(Container.DataItem, "acmaint_complied_date"), DataBinder.Eval(Container.DataItem, "acmaint_date_type"))%>
        </ItemTemplate>
        <EditItemTemplate>
          <asp:TextBox runat="server" ID="acmaint_complied_date" CssClass="float_left" Text='<%# FormattingMaintenanceDate(DataBinder.Eval(Container.DataItem, "acmaint_complied_date"), DataBinder.Eval(Container.DataItem, "acmaint_date_type")) %>'
            Width="65px" TabIndex="3" />
          <asp:Label runat="server" ID="acmaint_complied_date_format" CssClass="float_right tinyFormat"><%#UCase(ReturnCalendarFormat(DataBinder.Eval(Container.DataItem, "acmaint_date_type")))%></asp:Label>
        </EditItemTemplate>
        <FooterTemplate>
          <asp:TextBox runat="server" ID="acmaint_complied_date" Width="65px" TabIndex="3"
            CssClass="float_left" />
          <asp:Label runat="server" ID="acmaint_complied_date_format" CssClass="float_right tinyFormat"><%#UCase(ReturnCalendarFormat("D"))%></asp:Label>
        </FooterTemplate>
      </asp:TemplateField>
      <asp:TemplateField HeaderText="C/W Hrs" ItemStyle-Width="50px">
        <HeaderTemplate>
          <b class="text_underline help_cursor" title="Complied With or Completed" alt="Complied with or Completed">
            C/W</b> Hrs
        </HeaderTemplate>
        <ItemTemplate>
          <%#IgnoreZero(DataBinder.Eval(Container.DataItem, "acmaint_complied_hrs"))%>
        </ItemTemplate>
        <EditItemTemplate>
          <asp:TextBox runat="server" ID="acmaint_complied_hrs" Text='<%# IgnoreZero(DataBinder.Eval(Container.DataItem, "acmaint_complied_hrs")) %>'
            Width="50px" TabIndex="5" />
        </EditItemTemplate>
        <FooterTemplate>
          <asp:TextBox runat="server" ID="acmaint_complied_hrs" Width="50px" TabIndex="5" />
        </FooterTemplate>
      </asp:TemplateField>
      <asp:TemplateField HeaderText="Due Date" ItemStyle-Width="130px" ItemStyle-Wrap="false">
        <ItemTemplate>
          <%#FormattingMaintenanceDate(DataBinder.Eval(Container.DataItem, "acmaint_due_date"), DataBinder.Eval(Container.DataItem, "acmaint_date_type"))%>
        </ItemTemplate>
        <EditItemTemplate>
          <asp:TextBox runat="server" ID="acmaint_due_date" CssClass="float_left" Text='<%# FormattingMaintenanceDate(DataBinder.Eval(Container.DataItem, "acmaint_due_date"), DataBinder.Eval(Container.DataItem, "acmaint_date_type")) %>'
            Width="65px" TabIndex="6" />
          <asp:Label runat="server" ID="acmaint_due_date_format" CssClass="float_right tinyFormat"><%#UCase(ReturnCalendarFormat(DataBinder.Eval(Container.DataItem, "acmaint_date_type")))%></asp:Label>
        </EditItemTemplate>
        <FooterTemplate>
          <asp:TextBox runat="server" ID="acmaint_due_date" Width="65px" TabIndex="6" CssClass="float_left" />
          <asp:Label runat="server" ID="acmaint_due_date_format" CssClass="float_right tinyFormat"><%#UCase(ReturnCalendarFormat("D"))%></asp:Label>
        </FooterTemplate>
      </asp:TemplateField>
      <asp:TemplateField HeaderText="Due Hrs" ItemStyle-Width="50px">
        <ItemTemplate>
          <%#IgnoreZero(DataBinder.Eval(Container.DataItem, "acmaint_due_hrs"))%>
        </ItemTemplate>
        <EditItemTemplate>
          <asp:TextBox runat="server" ID="acmaint_due_hrs" Text='<%# IgnoreZero(DataBinder.Eval(Container.DataItem, "acmaint_due_hrs")) %>'
            Width="50px" TabIndex="8" />
        </EditItemTemplate>
        <FooterTemplate>
          <asp:TextBox runat="server" ID="acmaint_due_hrs" Width="50px" TabIndex="8" />
        </FooterTemplate>
      </asp:TemplateField>
      <asp:TemplateField HeaderText="Notes">
        <ItemTemplate>
          <%#DataBinder.Eval(Container.DataItem, "acmaint_notes")%>
        </ItemTemplate>
        <EditItemTemplate>
          <asp:TextBox runat="server" TextMode="MultiLine" Width="100%" Rows="3" ID="acmaint_notes"
            Text='<%# DataBinder.Eval(Container.DataItem, "acmaint_notes") %>' TabIndex="9" />
        </EditItemTemplate>
        <FooterTemplate>
          <asp:TextBox runat="server" TextMode="MultiLine" Width="250" Rows="3" ID="acmaint_notes"
            TabIndex="9" />
        </FooterTemplate>
      </asp:TemplateField>
    </Columns>
  </asp:GridView>
  <asp:Button ID="auto_add_multiple" runat="server" Text="Auto Add" Visible="false" /> 
  <asp:Label Visible="false" runat="server" ID="invis_maint_row" Text=""></asp:Label>
  <div class="div_clear">
  </div>
  <asp:CheckBox ID="check_auto" runat="server" Text="Automatically Update Dates"  AutoPostBack="true" />
  <asp:LinkButton runat="server" ID="TryToAddRowBelow" CssClass="padding display_block">Add New Maintenance/Inspection Item</asp:LinkButton>
  <asp:Literal runat="server" ID="maintenanceTableLiteral"></asp:Literal>
  <table align='right'><tr><td>
  <asp:Label runat="server" ID="Model_Items_List_Label" Visible="false" ></asp:Label>
  <asp:Label runat="server" ID="spacer_label" Text="<br /><br /><br/>"></asp:Label>
  <asp:Label runat="server" ID="delete_label" Visible="false" Text="Are You Sure You Want To Delete All of the Maintenance Items?"></asp:Label>
  <asp:Button runat="server" ID="delete_yes" Visible="false" Text="Yes" />
  <asp:Button runat="server" ID="delete_no" Visible="false" Text="No" />
  <asp:Button id="delete_all_maint_click" runat="server"  Text="Delete All Items?" />
  </td></tr></table>
  </asp:Panel>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="below_form">
  <!-- Latest compiled and minified CSS -->

  <script src="/common/moment-with-locales.js"></script>

  <script>
    function UpdateFormat(FormatToPick, Label1ToUpdate, Label2ToUpdate) {
      var TextToUse;
      switch (FormatToPick) {
        case "Y":
          TextToUse = "YYYY"
          break;
        case "M":
          TextToUse = "MM/YYYY"
          break;
        default:
          TextToUse = "MM/DD/YYYY"
      }

      Label1ToUpdate.text(TextToUse);
      Label2ToUpdate.text(TextToUse);

    }
    function FigureOutNewDate(TypeOfDate, DateKnown, DateFiguredOutTextBox, DurationTime, FormatToUse) {
      var AcceptableFormat;
      var BooleanValid;
      switch (FormatToUse) {
        case "Y":
          AcceptableFormat = "YYYY"
          break;
        case "M":
          AcceptableFormat = "MM/YYYY"
          break;
        default:
          AcceptableFormat = "MM/DD/YYYY"
      }

      BooleanValid = (moment(DateKnown, AcceptableFormat, true).isValid());

      if (BooleanValid) {
        if (DateKnown.length > 0) {
          var DurationTimeArray = DurationTime.split("|");
          var NewDateToUse
          if (DurationTimeArray[1] > 0) {
            if (TypeOfDate == 1) {
              NewDateToUse = moment(DateKnown, AcceptableFormat).add(DurationTimeArray[1], 'M');
            } else {
              NewDateToUse = moment(DateKnown, AcceptableFormat).subtract(DurationTimeArray[1], 'M');
            }

            DateFiguredOutTextBox.val(NewDateToUse.format(AcceptableFormat));
          }
        }
      } else { alert('Invalid date format. Please use: ' + AcceptableFormat); }
    }
  </script>

</asp:Content>
