<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FolderMaintenance.aspx.vb"
  Inherits="crmWebClient.FolderMaintenance" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


  <script type="text/javascript" language='javascript'>
  
    if (<%= bRefreshPreferences.ToString.Tolower %>) {
      if ((typeof (window.opener) != "undefined") && (window.opener != null)) {
        window.opener.refreshPreferences();
        self.close();
      }
    }

    if (<%= bRefreshHome.ToString.Tolower %>) {
      if ((typeof (window.opener) != "undefined") && (window.opener != null)) {
        window.opener.refreshHome();
        self.close();
      }
    }
    
    function Fit() {
      window.resizeTo(1094, 680);
      self.focus();
    };

    window.onload = function() {
      Fit();
      FigureParentPage();
    };

    function FigureParentPage() {
      var pathname = window.opener.location.href;
      pathname = pathname.replace(/^.*\/\/[^\/]+/, '')
      var updateBox = document.getElementById("<%= parent_path.clientID %>")
      updateBox.value = pathname
    }
    
    function openSmallWindowJS(address, windowname) {
      var rightNow = new Date();
      windowname += rightNow.getTime();
      var Place = window.open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1150,resizable=yes,toolbar=no,location=no,status=no");
      return true;
    }
    
  </script>

  <style type="text/css">
    /*DragHandle Classes*//*Reorder List*/.dragHandle, .dragHandleHide, .dragHandleShare, .dragHandleShareHide, .dragHandleShareStatic, .dragHandleShareHideStatic, .dragHandleStatic, .dragHandleHideStatic
    {
      width: 32px;
      height: 16px;
      background-repeat: no-repeat;
      background-position: left top;
      cursor: move;
    }
    .dragHandle
    {
      background-image: url(                           '../images/refresh_regular_folder.png' );
    }
    .dragHandleHide
    {
      background-image: url(                           '../images/refresh_regular_folder_hidden.png' );
    }
    .dragHandleStatic
    {
      background-image: url(                           '../images/static_regular_folder.png' );
    }
    .dragHandleHideStatic
    {
      background-image: url(                           '../images/static_regular_folder_hidden.png' );
    }
    .dragHandleShare
    {
      background-image: url(                           '../images/refresh_shared_folder.png' );
    }
    .dragHandleShareHide
    {
      background-image: url(                           '../images/refresh_shared_folder_hidden.png' );
    }
    .dragHandleShareStatic
    {
      background-image: url(                           '../images/static_shared_folder.png' );
    }
    .dragHandleShareHideStatic
    {
      background-image: url(                           '../images/static_shared_folder_hidden.png' );
    }
    .callbackStyle table
    {
      background-color: #5377A9;
      color: Black;
    }
    .reorderListDemoShare li
    {
      list-style: none;
      margin: 2px;
      background-color: #ecfbea;
      padding: 3px;
      color: #000000;
      display: block;
    }
    .reorderListDemoShare li a
    {
      /*   color: #000 !important;*/
      font-weight: bold;
    }
    .reorderListDemoShare ul
    {
      margin: 0px;
      padding: 0px;
    }
    .specialH3
    {
      display: block;
      border-bottom: 1px solid #dddddd;
      font-size: 12px;
      font-weight: bold;
      margin: 5px;
    }
    .specialH4
    {
      display: block;
      border-bottom: 1px solid #dddddd;
      font-size: 12px;
      font-weight: bold;
      margin: 5px;
      color: #53854e;
    }
    .reorderListDemo li
    {
      list-style: none;
      margin: 2px;
      background-color: #daeaff;
      padding: 3px;
      color: #000000;
      display: block;
    }
    .reorderListDemo li a
    {
      /*  color: #000 !important;*/
      font-weight: bold;
    }
    .reorderListDemo ul
    {
      margin: 0px;
      padding: 0px;
    }
    .reorderCue
    {
      border: dashed thin black;
      width: 100%;
      padding: 3px;
      height: 25px;
    }
    .itemArea
    {
      margin-left: 15px;
      font-family: Arial, Verdana, sans-serif;
      font-size: 1em;
      text-align: left;
      display: block;
      width: 100%;
    }
    .valueSpec.aircraftSpec.Simplistic.plain, .valueSpec.Simplistic.plain, .valueSpec.plain
    {
      min-height: 700px;
      background-image: none;
      background-color: #eee !important;
      margin-top: -25px;
      height: none;
    }
    .specialRow
    {
      width: 98%;
      margin-left: auto;
      margin-right: auto;
      padding-top: 9px;
    }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="valueSpec viewValueExport Simplistic aircraftSpec plain">
    <table width="100%" cellspacing="0" cellpadding="0" class="DetailsBrowseTable">
      <tr>
        <td align="left" valign="top">
          <div class="backgroundShade">
            <asp:LinkButton ID="Add_Folder_Mode" runat="server" CssClass="float_left gray_button noBefore"><strong>Add New Folder</strong></asp:LinkButton>
            <asp:LinkButton ID="CloseButton" runat="server" CssClass="gray_button float_right noBefore"
              OnClick="RefreshPage"><img src="/images/x.svg" alt="Close" /></asp:LinkButton></div>
        </td>
      </tr>
    </table>
    <asp:Label runat="server" ID="attention" CssClass="red_text"><p align='center'></p></asp:Label>
    <asp:Panel runat="server" ID="AddFolder" CssClass="row specialRow">
      <div class="Box seven columns">
        <asp:Table runat="server" ID="add_folder_table" Width="100%" CellPadding="3" CellSpacing="0"
          CssClass="formatTable blue float_left">
          <asp:TableRow CssClass="noBorder">
            <asp:TableCell HorizontalAlign="right" VerticalAlign="top">
                        <div class="subHeader">Folder Information</div>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow CssClass="noBorder">
            <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
              <asp:Panel runat="server" ID="add_text_panel" CssClass="nonflyout_info_box remove_margin">
                <p>
                  <asp:Label runat="server" ID="add_folder_text">The purpose of this form is to create
                    a Folder for the [<asp:Label runat="server" ID="foldertypeStringLabel" Text="Aircraft"></asp:Label>]
                    search that you have just completed. Please name and describe the folder you are
                    creating for easy reference and then click 'Add Folder' to save the folder for future
                    use.</asp:Label></p>
              </asp:Panel>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
              <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Folder_Edit"
                CssClass="circle alt_row padding" DisplayMode="BulletList" EnableClientScript="true"
                HeaderText="There are problems with the following fields:" />
              <table width="100%" cellpadding="2" cellspacing="0">
                <tr>
                  <td align="left" valign="top" width="110">
                    Folder Name:
                  </td>
                  <td align="left" valign="top">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Folder_Edit"
                      ErrorMessage="A Folder Name is Required" ControlToValidate="cfolder_name" Text=""
                      Display="None">
                    </asp:RequiredFieldValidator>
                    <asp:TextBox ID="cfolder_name" runat="server" Width="100%" MaxLength="250">
                    </asp:TextBox>
                  </td>
                </tr>
              </table>
              <asp:TextBox ID="cfolder_type_of_folder" runat="server" Width="100%" CssClass="display_none">
              </asp:TextBox>
              <asp:TextBox ID="TextBox1" runat="server" Width="100%" CssClass="display_none">
              </asp:TextBox>
              <asp:TextBox ID="cfolder_method" runat="server" Width="100%" CssClass="display_none">
              </asp:TextBox>
              <asp:TextBox ID="cfolder_id" runat="server" Width="100%" CssClass="display_none">
              </asp:TextBox>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
              <table width="100%" cellpadding="2" cellspacing="0">
                <tr>
                  <td align="left" valign="top" width="110">
                    Folder Description:
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="cfolder_description" runat="server" TextMode="MultiLine" Rows="5"
                      Width="100%">
                    </asp:TextBox>
                  </td>
                </tr>
              </table>
              <asp:TextBox ID="cfolder_data" runat="server" TextMode="MultiLine" Rows="5" Width="100%"
                CssClass="display_none">
              </asp:TextBox>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
              <asp:CheckBox ID="cfolder_share" Enabled="true" runat="server" TextAlign="Left" Text="Share with Others on your Subscription?:" />
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
              <asp:CheckBox ID="cfolder_hide" runat="server" TextAlign="Left" Text="Hide from Default Display?:" />
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell HorizontalAlign="left" VerticalAlign="top" ColumnSpan="2">
              <asp:CheckBox ID="cfolder_default" runat="server" TextAlign="Left" Text="Set as Default?:"
                Enabled="false" />
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow runat="server" ID="operatorAnalysisRow" CssClass="display_none">
            <asp:TableCell HorizontalAlign="left" VerticalAlign="top" ColumnSpan="2">
              <asp:CheckBox ID="cfolder_operator_flag" runat="server" TextAlign="Left" Text="Include in Flight Activity View?:"
                Enabled="false" />
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell HorizontalAlign="right" VerticalAlign="top" ColumnSpan="2">
              <asp:CheckBox runat="server" CssClass="display_none" ID="launchFlight" Checked="false" Text="launch flight View"
                runat="server" />
              <asp:Button ID="folder_submit_button" runat="server" CausesValidation="true" ValidationGroup="Folder_Edit"
                CssClass="float_right" Text="Add Folder" />
              <input type="button" id="folder_submit_button_flight" runat="server" visible="false"
                class="float_right" value="Save and Launch Flight Activity View" />
              <asp:Button ID="cancel_add_folder_button" runat="server" CssClass="float_right"
                Visible="false" Text="Cancel Add Folder" />
                
              &nbsp;&nbsp;<asp:button ID="folder_delete_button" runat="server" PostBackUrl=""
                OnClientClick="if(!confirm('Are you sure you want to remove this folder?'))return false;"
                CausesValidation="true" Visible="false" CssClass="float_right" text="Delete Folder" />
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
        <br clear="all" />
      </div>
      <div class="five columns Box" runat="server" id="eventsBox" visible="false">
        <asp:Table ID="EventTable" runat="server" Width="100%" CellPadding="3" CellSpacing="0"
          CssClass="formatTable blue float_right" Visible="false">
          <asp:TableRow CssClass="noBorder">
            <asp:TableCell HorizontalAlign="right" VerticalAlign="top" ColumnSpan="2">
                    <div class="subHeader">Event Emailer Settings</div>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow CssClass="noBorder">
            <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
              <p class="nonflyout_info_box dataListBlue">
                JETNET provides the ability for users to have the results of Event based folders
                automatically sent via email based on the timeframe of the Event search. To have
                the results of this Folder sent via email just click on the checkbox below.<br />
                <br />
              </p>
              <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <asp:CheckBox ID="cfolder_jetnet_run_flag" runat="server" ToolTip="Click to Send Email Alerts."
                      Text="Send Folder Results Via Automated Email?" />
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top" width="70">
                    Name:
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="cfolder_jetnet_run_reply_username" MaxLength="100" runat="server"
                      Width="55%" ToolTip="Name that Alert is Addressed to." />
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    Email:
                  </td>
                  <td align="left" valign="top">
                    <asp:RegularExpressionValidator ControlToValidate="cfolder_jetnet_run_reply_email"
                      ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"
                      ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please Enter a valid Email Address<br />"
                      Text="" ValidationGroup="Folder_Edit" Display="None"></asp:RegularExpressionValidator>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Email is Required"
                      OnServerValidate="checkEmail" ControlToValidate="cfolder_jetnet_run_reply_email"
                      ValidationGroup="Folder_Edit" ValidateEmptyText="true" Text="" Display="None"></asp:CustomValidator>
                    <asp:TextBox ID="cfolder_jetnet_run_reply_email" MaxLength="150" runat="server" Width="55%"
                      ToolTip="Email Address Used to Send Alert." />
                    <asp:TextBox ID="emptyBox" runat="server" Width="55%" Text="0" CssClass="display_none" />
                  </td>
                </tr>
              </table>
              <asp:Label ID="folder_query_list" runat="server" Text="">
              </asp:Label>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
        <br clear="all" />
      </div>
    </asp:Panel>
    <asp:Panel ID="EditFolder" runat="server">
      <div class="Box">
        <p align="left" class="nonflyout_info_box remove_margin">
          Please use this form to add, edit, and sort folders for display within Evolution.
          Note that you may only edit and sort your own Personal folders, Shared Folders can
          only be edited by your subscription administrator. Only administrators can sort
          shared folders in the display below and resulting order will be applied globally
          to shared folders for all users.<br />
          <br />
          To sort folders simply drag the folder icon above or below the position you desire.
          Administrators should note that shared folders may not be dragged into the personal
          block and personal folders may not be dragged into the shared block.
          <asp:Label ID="bottom_label_text" runat="server" Text=""></asp:Label>
        </p>
        <asp:Label runat="server" ID="feedback" Font-Bold="true" ForeColor="Red" /><br />
        <b>View:</b>&nbsp;<asp:DropDownList ID="export_types" runat="server" AutoPostBack="true">
        </asp:DropDownList>
        <br />
        <asp:Panel runat="server" ID="shared_folders_panel">
          <div class="reorderListDemoShare">
            <div class="data_aircraft_grid">
              <h2 class="header_row remove_margin">
                <b>Shared Folders</b></h2>
            </div>
            <cc1:ReorderList ID="Shared_Reorder_List" runat="server" PostBackOnReorder="true"
              CallbackCssStyle="callbackStyle" LayoutType="Table" DragHandleAlignment="Left"
              DataKeyField="cfolder_id" SortOrderField="cfolder_sort2" ShowInsertItem="false"
              OnItemCommand="Save_Row" ItemInsertLocation="Beginning" Width="590px" EnableViewState="true"
              OnItemDataBound="Item_Bound">
              <itemtemplate>
            <div class="itemArea">
              <table width="50" cellpadding="0" cellspacing="0" class="float_right">
                <tr>
                  <td align="left" valign="top">
                    <asp:LinkButton ID="edit" Visible='true' runat="server" CommandName="Edit" CausesValidation="false"><img src="images/edit_icon.png" alt="Edit" border="0" title="Edit"/></asp:LinkButton>
                    &nbsp;&nbsp;<asp:LinkButton ID="delete" Visible='true' runat="server" CommandName="Delete"
                      OnClientClick="if(!confirm('Are you sure you want to remove this folder?'))return false;"
                      CausesValidation="false"><img src="images/delete_icon.png" alt="Delete" border="0" title="Delete" /></asp:LinkButton>
                  </td>
                </tr>
              </table>
              <table cellpadding="0" cellspacing="0" width='100%'>
                <tr>
                  <td width='65%'>
                    <asp:Label ID="Label1" runat="server" Text='<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("cfolder_name")))%>' />
                    &nbsp;
                  </td>
                  <td width='35%'>
                    <asp:Label ID="Label2" runat="server" Text='<%#IIf(not isdbnull(DataBinder.Eval(Container.DataItem, "contact_first_name")), DataBinder.Eval(Container.DataItem, "contact_first_name"), "") & " " & IIf(not isdbnull(DataBinder.Eval(Container.DataItem, "contact_last_name")), DataBinder.Eval(Container.DataItem, "contact_last_name"), "")%>' />
                    &nbsp;
                  </td>
                </tr>
              </table>
              <asp:TextBox ID="id" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_id") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_name" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_name") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_cftype_id" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cftype_id") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_share" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_share") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_hide_flag" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_hide_flag") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_sort" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_sort") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_operator_flag" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_operator_flag") %>'
                Style="display: none;"></asp:TextBox>
            </div>
          </itemtemplate>
              <edititemtemplate>
            <div class="itemArea">
              <asp:TextBox ID="id" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_id") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_name" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_name") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_cftype_id" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cftype_id") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_share" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_share") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_hide_flag" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_hide_flag") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_operator_flag" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_operator_flag") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_sort" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_sort") %>'
                Style="display: none;"></asp:TextBox>
              <table width="96%" cellpadding="3" cellspacing="3" >
                <tr>
                  <td align="left" valign="top" width="110">
                    Folder Name:
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="new_name" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_name") %>'></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    Folder Description:
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="new_description" runat="server" TextMode="MultiLine" Rows="5" Width="100%"
                      Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_description") %>'></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <asp:CheckBox ID="new_share" Enabled='<%#IIf(Session.Item("localUser").crmUserType = 2, "true", "false")%>'
                      runat="server" TextAlign="Left" Checked='<%#IIf(Eval("cfolder_share") = "Y", "true", "false")%>'
                      Text="Share with Others on your Subscription?:" />
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <asp:CheckBox ID="new_hide" runat="server" TextAlign="Left" Checked='<%#IIf(Eval("cfolder_hide_flag") = "Y", "true", "false")%>'
                      Text="Hide from Default Display?:" />
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <asp:CheckBox ID="new_operator" runat="server" cssClass='<%#IIf(Eval("cftype_id") = "1", "", "display_none")%>' TextAlign="Left" Checked='<%#IIf(Eval("cfolder_operator_flag") = "Y", "true", "false")%>'
                      Text="Include in Flight Activity View?:" />
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <asp:LinkButton ID="Cancel" runat="server" CommandName="Cancel" Text=" Cancel " CssClass="float_right gray_button white_text"
                      CausesValidation="false" ForeColor="White"></asp:LinkButton>&nbsp;
                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Save" Text=" Save "
                      CssClass="float_right gray_button white_text" ForeColor="White" CausesValidation="false"></asp:LinkButton>
                  </td>
                </tr>
              </table>
            </div>
          </edititemtemplate>
              <reordertemplate>
            <asp:Panel ID="Panel2" runat="server" CssClass="reorderCue" />
          </reordertemplate>
              <draghandletemplate>
            <asp:Label ID="Label1" runat="server" Text='<%# FolderClassDisplay("Y", Eval("cfolder_hide_flag").tostring, Eval("cfolder_method").tostring)%>' />
          </draghandletemplate>
            </cc1:ReorderList>
          </div>
        </asp:Panel>
        <br />
        <asp:Panel runat="server" ID="personal_folders_panel" Visible="true">
          <div class="reorderListDemo">
            <div class="data_aircraft_grid">
              <h2 class="header_row remove_margin">
                <b>Personal Folders.</b></h2>
            </div>
            <cc1:ReorderList ID="NonShared_Reorder_List" runat="server" PostBackOnReorder="true"
              CallbackCssStyle="callbackStyle" LayoutType="Table" DragHandleAlignment="Left"
              DataKeyField="cfolder_id" SortOrderField="cfolder_sort2" ShowInsertItem="false"
              OnItemCommand="Save_Row" ItemInsertLocation="Beginning" Width="590px" EnableViewState="true">
              <itemtemplate>
            <div class="itemArea">
              <table width="50" cellpadding="0" cellspacing="0" class="float_right">
                <tr>
                  <td align="left" valign="top">
                    <asp:LinkButton ID="edit" runat="server" CommandName="Edit" CausesValidation="false"><img src="images/edit_icon.png" alt="Edit" title="Edit" border="0"/></asp:LinkButton>
                    &nbsp;&nbsp;<asp:LinkButton ID="delete" runat="server" CommandName="Delete" OnClientClick="if(!confirm('Are you sure you want to remove this folder?'))return false;"
                      CausesValidation="false"><img src="images/delete_icon.png" alt="Delete" title="Delete" border="0" /></asp:LinkButton>
                  </td>
                </tr>
              </table>
              <table cellpadding="0" cellspacing="0" width='100%'>
                <tr>
                  <td width='65%' align='left'>
                    <asp:Label ID="Label1" runat="server" Text='<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("cfolder_name")))%>' />
                    &nbsp;
                  </td>
                  <td width='35%' align='left'>
                    <asp:Label ID="Label2" runat="server" Text='<%#IIf(not isdbnull(DataBinder.Eval(Container.DataItem, "contact_first_name")), DataBinder.Eval(Container.DataItem, "contact_first_name"), "") & " " & IIf(not isdbnull(DataBinder.Eval(Container.DataItem, "contact_last_name")), DataBinder.Eval(Container.DataItem, "contact_last_name"), "")%>' />
                    &nbsp;
                  </td>
                </tr>
              </table>
              <asp:TextBox ID="id" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_id") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_name" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_name") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_cftype_id" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cftype_id") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_share" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_share") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_hide_flag" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_hide_flag") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_operator_flag" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_operator_flag") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_sort1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_sort") %>'
                Style="display: none;"></asp:TextBox>
            </div>
          </itemtemplate>
              <edititemtemplate>
            <div class="itemArea">
              <asp:TextBox ID="id" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_id") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_name" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_name") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_cftype_id" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cftype_id") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_share" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_share") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_hide_flag" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_hide_flag") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_operator_flag" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_operator_flag") %>'
                Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="cfolder_sort1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_sort") %>'
                Style="display: none;"></asp:TextBox>
              <table width="96%" cellpadding="3" cellspacing="3">
                <tr>
                  <td align="left" valign="top" width="110">
                    Folder Name:
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="new_name" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_name") %>'></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    Folder Description:
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="new_description" runat="server" TextMode="MultiLine" Rows="5" Width="100%"
                      Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_description") %>'></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <asp:CheckBox ID="new_share" Enabled='<%#IIf(Session.Item("localUser").crmUserType = 2 Or Eval("cfolder_login") = Session.Item("localUser").crmUserLogin, "true", "false")%>'
                      runat="server" TextAlign="Left" Checked='<%#IIf(Eval("cfolder_share") = "Y", "true", "false")%>'
                      Text="Share with Others on your Subscription?:" />
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <asp:CheckBox ID="new_hide" runat="server" TextAlign="Left" Checked='<%#IIf(Eval("cfolder_hide_flag") = "Y", "true", "false")%>'
                      Text="Hide from Default Display?:" />
                  </td>
                </tr>
                 <tr>
                  <td align="left" valign="top" colspan="2">
                    <asp:CheckBox ID="new_operator" runat="server" TextAlign="Left"  cssClass='<%#IIf(Eval("cftype_id") = "1", "", "display_none")%>' Checked='<%#IIf(Eval("cfolder_operator_flag") = "Y", "true", "false")%>'
                      Text="Include in Flight Activity View?:" />
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <asp:LinkButton ID="Cancel" runat="server" CommandName="Cancel" Text=" Cancel " CausesValidation="false"
                      CssClass="float_right gray_button white_text"></asp:LinkButton>&nbsp;&nbsp;
                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Save" Text=" Save "
                      CssClass="float_right gray_button white_text" CausesValidation="false"></asp:LinkButton>
                  </td>
                </tr>
              </table>
            </div>
          </edititemtemplate>
              <reordertemplate>
            <asp:Panel ID="Panel2" runat="server" CssClass="reorderCue" />
          </reordertemplate>
              <draghandletemplate>
            <asp:Label ID="Label2" runat="server" Text='<%# FolderClassDisplay("N", Eval("cfolder_hide_flag").tostring, Eval("cfolder_method").tostring)%>' />
          </draghandletemplate>
            </cc1:ReorderList>
          </div>
        </asp:Panel>
      </div>
    </asp:Panel>
    <asp:TextBox runat="server" ID="parent_path" Style="display: none;"></asp:TextBox>
  </div>
</asp:Content>
<asp:Content runat="server" ID="bottom" ContentPlaceHolderID="below_form">

  <script type="text/javascript">
    $('#<%= folder_submit_button_flight.ClientID %>').click(function() {
      $('#<%= launchFlight.ClientID %>').prop("checked", true);
      $('#<%= folder_submit_button.ClientID %>').click();
    });
    

    $('#<%= cfolder_operator_flag.ClientID %>').change(function() {
      if (this.checked) {
        $('#<%= folder_submit_button_flight.ClientID %>').removeClass("display_none");
      } else {
      $('#<%= folder_submit_button_flight.ClientID %>').addClass("display_none");
      }
    });

    function setFlightActivityView(portfolioID, portfolioName) {
      my_form = document.createElement('FORM');
      my_form.method = 'POST';

      //if (window.opener != null) {
      //  window.opener.name = "MyParent";
      //  my_form.target = "MyParent";
      //}
      my_form.target = "_blank";
      my_form.name = 'mappingForm';
      my_form.action = 'view_template.aspx?ViewID=28&ViewName=Flight Activity (Operator/Airport)';

      my_tb = document.createElement('INPUT');
      my_tb.type = 'HIDDEN';
      my_tb.name = "acfolder";
      my_tb.value = portfolioID;
      my_form.appendChild(my_tb);

      document.body.appendChild(my_form);

      my_tb = document.createElement('INPUT');
      my_tb.type = 'HIDDEN';
      my_tb.name = "acfoldername";
      my_tb.value = portfolioName;
      my_form.appendChild(my_tb);

      document.body.appendChild(my_form);
      my_form.submit();
    }

    function setOperatorAnalysisView(portfolioID, portfolioName) {
      my_form = document.createElement('FORM');
      my_form.method = 'POST';

      if (window.opener != null) {
        window.opener.name = "MyParent";
        my_form.target = "MyParent";
      }

      my_form.name = 'mappingForm';
      my_form.action = 'view_template.aspx?ViewID=28&ViewName=Flight Activity (Operator/Airport)';

      my_tb = document.createElement('INPUT');
      my_tb.type = 'HIDDEN';
      my_tb.name = "opfolder";
      my_tb.value = portfolioID;
      my_form.appendChild(my_tb);

      document.body.appendChild(my_form);

      my_tb = document.createElement('INPUT');
      my_tb.type = 'HIDDEN';
      my_tb.name = "opfoldername";
      my_tb.value = portfolioName;
      my_form.appendChild(my_tb);

      document.body.appendChild(my_form);
      my_form.submit();
    }
  </script>
  </script>
  
  

</asp:Content>
