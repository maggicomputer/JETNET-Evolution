<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Submenu_Edit_Template.ascx.vb"
    Inherits="crmWebClient.Submenu_Edit_Template" %>
<asp:Panel ID="add_folder_panl" runat="server" Visible="true" BackColor="White" CssClass="edit_panel">

    <h4 align="right">
        Add a Subfolder</h4>
    <p align="left" class="nonflyout_info_box">
        Please type in your subfolder name and choose a main folder for it to appear under.</p>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="dynamic"
        ControlToValidate="folder_name" ErrorMessage="*Folder Name must be 1-50 characters."
        ValidationExpression="^[\s\S]{0,50}$" EnableClientScript="true" ForeColor="Red"
        Font-Bold="true" />
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
        ErrorMessage="*Folder Name is Required." ControlToValidate="add_folder_cbo" Font-Bold="true"></asp:RequiredFieldValidator>
    <table width="365" cellspacing="0" cellpadding="3">
        <tr>
            <td align="left" valign="top">
                <asp:TextBox ID="folder_name" runat="server" MaxLength="100"></asp:TextBox>
            </td>
            <td align="left" valign="top">
                <asp:DropDownList ID="add_folder_cbo" runat="server" CssClass="float_left">
                </asp:DropDownList>
                <br clear="all" />
            </td>
            <td align="left" valign="top">
                <asp:ImageButton ID="add_sub" runat="server" ImageUrl="~/images/add_new.jpg" CausesValidation="true" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel runat="server" ID="add_active_folder" Visible="false" BackColor="White"
    CssClass="edit_panel"><asp:Label runat="server" ID="add_active_shared" Visible="false" Font-Bold="true" ForeColor="Red"><p>The current folder is shared, but was not created by your login. Therefore editing of the folder is disabled.</p></asp:Label>
    <asp:Table runat="server" ID="add_folder_table" Width="98%" CellPadding="3" CellSpacing="0"
        CssClass="data_aircraft_grid float_left">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="right" VerticalAlign="top" CssClass="header_row">
                <h4 align="right">
                    Add
                    <asp:Label runat="server" ID="folder_type_label">Aircraft</asp:Label>
                    Active Folder</h4>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <p class="nonflyout_info_box remove_margin">
                    <asp:Label runat="server" ID="add_folder_text">The purpose of this form is to create
                        a Folder for the [<asp:Label runat="server" ID="foldertypeStringLabel">Aircraft</asp:Label>]
                        search that you have just completed. Please name and describe the folder you are
                        creating for easy reference and then click 'Add Folder' to save the folder for future
                        use.</asp:Label></p>
                <asp:Label runat="server" ID="active_folder_attention" Font-Bold="true" ForeColor="Red"
                    Visible="false"></asp:Label>
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="Folder_Edit"
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
                <asp:TextBox ID="cfolder_method" runat="server" Width="100%" CssClass="display_none">
                </asp:TextBox>
                <asp:TextBox ID="cfolder_id" runat="server" Width="100%" CssClass="display_none">
                </asp:TextBox>
                <asp:TextBox ID="cfolder_sort1" runat="server" Width="100%" CssClass="display_none">
                </asp:TextBox>
                <asp:TextBox ID="cfolder_sort2" runat="server" Width="100%" CssClass="display_none">
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
            <asp:TableCell HorizontalAlign="left" VerticalAlign="top" ColumnSpan="2">
                <asp:CheckBox ID="cfolder_hide" runat="server" TextAlign="Left" Text="Hide from Default Display?:" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="right" VerticalAlign="top" ColumnSpan="2" CssClass="alt_row">
                <asp:LinkButton ID="folder_submit_button" runat="server" CausesValidation="true"
                    ValidationGroup="Folder_Edit" CssClass="float_right gray_button white_text">Add Folder</asp:LinkButton>
                <asp:LinkButton ID="cancel_add_folder_button" runat="server" CssClass="float_right gray_button white_text"
                    Visible="false">Cancel Add Folder</asp:LinkButton>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
<asp:Panel ID="add_list_to_folder" runat="server" Visible="false" BackColor="White"
    CssClass="edit_panel">
    <h4 align="right">
        Add To Folder</h4>
    <p align="left" class="nonflyout_info_box">
        Please choose the correct folder for your selected items to appear under.</p>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic"
        ErrorMessage="A Folder is Required" ControlToValidate="add_list_folder_cbo" Font-Bold="true"></asp:RequiredFieldValidator>
    <table width="375" cellspacing="0" cellpadding="3">
        <tr>
            <td align="left" valign="top">
                <asp:DropDownList ID="add_list_folder_cbo" runat="server" CssClass="float_left">
                </asp:DropDownList>
                <br clear="all" />
            </td>
            <td align="left" valign="top">
                <asp:ImageButton ID="add_to_folder_btn" runat="server" ImageUrl="~/images/add_to_folder.jpg"
                    AlternateText="Add to Folder" CausesValidation="true" />
            </td>
        </tr>
    </table>
    <br />
    <br />
</asp:Panel>
<asp:Panel ID="edit_folder" runat="server" Visible="true" BackColor="White" CssClass="edit_panel">
    <asp:Label ID="label_header" runat="server"><h4 align="right">
        Edit Subfolders</h4></asp:Label>
    <p align="left" class="nonflyout_info_box">
        Please use this form to add, edit, and sort folders for display within the Marketplace Manager.
        Note that you may only edit and sort your own folders. Only administrators can sort
        shared folders in the display below and resulting order will be applied globally
        to shared folders for all users.</p>
    <p align="left" class="nonflyout_info_box">
        To sort folders simply drag the folder icon above or below the position you desire.
        Note that shared folders may not be dragged into the personal block and personal
        folders may not be dragged into the shared block</p>
    <asp:Label runat="server" ID="feedback" Font-Bold="true" ForeColor="Red" />
    </p>
    <div class="reorderListDemoShare">
        <p align="right">
            <b>
                <asp:LinkButton ID="add_new" runat="server">New Menu Item</asp:LinkButton></b><br />
            <br />
        </p>
        <h2 class="specialH2">
            Shared Folders</h2>
        <cc1:ReorderList ID="ReorderList2" runat="server" PostBackOnReorder="true" CallbackCssStyle="callbackStyle"
            LayoutType="Table" DragHandleAlignment="Left" DataKeyField="cfolder_id" SortOrderField="cfolder_sort2"
            ShowInsertItem="false" OnItemCommand="Save_Row_Shared" ItemInsertLocation="Beginning"
            Width="590px" EnableViewState="true" OnCancelCommand="Cancel_Shared" OnEditCommand="Edit_Shared"
            OnDeleteCommand="Delete_Shared">
            <ItemTemplate>
                <div class="itemArea">
                    <table width="50" cellpadding="0" cellspacing="0" class="float_right">
                        <tr>
                            <td align="left" valign="top">
                                <asp:LinkButton ID="edit" Visible='<%#IIf(Eval("cfolder_cliuser_id") = Session.Item("localUser").crmLocalUserID or Session.Item("localUser").crmUserType = 2, "true", "false")%>'
                                    runat="server" CommandName="Edit" CausesValidation="false"><img src="images/edit_icon.png" alt="Edit" border="0" title="Edit"/></asp:LinkButton>
                                &nbsp;&nbsp;<asp:LinkButton ID="delete" OnClientClick='<%#Display_Popup(DataBinder.Eval(Container.DataItem, "tcount"))%>'
                                    Visible='<%#IIf(Eval("cfolder_cliuser_id") = Session.Item("localUser").crmLocalUserID  or Session.Item("localUser").crmUserType = 2, "true", "false")%>'
                                    runat="server" CommandName="Delete" CausesValidation="false"><img src="images/delete_icon.png" alt="Delete" border="0" title="Delete" /></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="Label1" runat="server" Text='<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("cfolder_name"))) %>' />
                    <asp:TextBox ID="id" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_id") %>'
                        Style="display: none;"></asp:TextBox>
                </div>
            </ItemTemplate>
            <EditItemTemplate>
                <div class="itemArea">
                    Name:<asp:TextBox ID="new_name" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_name") %>'></asp:TextBox><br />
                    Hide from Menu?:
                    <asp:CheckBox ID="new_hide" runat="server" Checked='<%#IIf(Eval("cfolder_hide_flag") = "Y", "true", "false")%>' /><br />
                    Share with Other Users?
                    <asp:CheckBox ID="new_share" runat="server" Checked='<%#IIf(Eval("cfolder_share") = "Y", "true", "false")%>' /><br />
                    <asp:LinkButton ID="Cancel" runat="server" CommandName="Cancel" Text=" Cancel " CausesValidation="false"></asp:LinkButton>&nbsp;
                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Save" Text=" Save "
                        CausesValidation="false"></asp:LinkButton>
                    <asp:TextBox ID="id" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_id") %>'
                        Style="display: none;"></asp:TextBox>
                </div>
            </EditItemTemplate>
            <ReorderTemplate>
                <asp:Panel ID="Panel2" runat="server" CssClass="reorderCue" />
            </ReorderTemplate>
            <DragHandleTemplate>
                <asp:Label runat="server" Text='<%# ToggleNewFolderIcon("Y",DataBinder.Eval(Container.DataItem, "cfolder_method"), DataBinder.Eval(Container.DataItem, "cfolder_hide_flag"), DataBinder.Eval(Container.DataItem, "cfolder_share"))%>' />
            </DragHandleTemplate>
            <InsertItemTemplate>
                <div style="text-align: left; border-bottom: thin solid transparent;">
                    <asp:Panel ID="panel1" runat="server" DefaultButton="Button1">
                        Name:
                        <asp:TextBox ID="new_name" runat="server" Text='<%# Bind("cfolder_name") %>' /><br />
                        Hide from Menu?:
                        <asp:CheckBox ID="new_hide" runat="server" Checked='<%#IIf(Eval("cfolder_hide_flag") = "Y", "true", "false")%>' /><br />
                        Share with Other Users?
                        <asp:CheckBox ID="new_share" runat="server" Checked='<%#IIf(Eval("cfolder_share") = "Y", "true", "false")%>' /><br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter a Name"
                            ControlToValidate="new_name" /><br />
                        <asp:LinkButton ID="Button1" runat="server" CommandName="Insert" Text="Add"></asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Cancel" Text="Cancel"
                            CausesValidation="false"></asp:LinkButton>
                    </asp:Panel>
                </div>
            </InsertItemTemplate>
        </cc1:ReorderList>
    </div>
    <br />
    <div class="reorderListDemo">
        <h3 class="specialH3">
            Personal Folders</h3>
        <cc1:ReorderList ID="ReorderList1" runat="server" PostBackOnReorder="true" CallbackCssStyle="callbackStyle"
            LayoutType="Table" DragHandleAlignment="Left" DataKeyField="cfolder_id" SortOrderField="cfolder_sort2"
            ShowInsertItem="false" OnItemCommand="Save_Row_Shared" ItemInsertLocation="Beginning"
            Width="590px" EnableViewState="true" OnCancelCommand="Cancel_Shared" OnEditCommand="Edit_Shared"
            OnDeleteCommand="Delete_Shared">
            <ItemTemplate>
                <div class="itemArea">
                    <table width="50" cellpadding="0" cellspacing="0" class="float_right">
                        <tr>
                            <td align="left" valign="top">
                                <asp:LinkButton ID="edit" Visible='<%#IIf(Eval("cfolder_cliuser_id") = Session.Item("localUser").crmLocalUserID, "true", "false")%>'
                                    runat="server" CommandName="Edit" CausesValidation="false"><img src="images/edit_icon.png" alt="Edit" title="Edit" border="0"/></asp:LinkButton>
                                &nbsp;&nbsp;<asp:LinkButton ID="delete" OnClientClick='<%#Display_Popup(DataBinder.Eval(Container.DataItem, "tcount"))%>'
                                    Visible='<%#IIf(Eval("cfolder_cliuser_id") = Session.Item("localUser").crmLocalUserID, "true", "false")%>'
                                    runat="server" CommandName="Delete" CausesValidation="false"><img src="images/delete_icon.png" alt="Delete" title="Delete" border="0" /></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="Label1" runat="server" Text='<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("cfolder_name"))) %>' />
                    <asp:TextBox ID="id" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_id") %>'
                        Style="display: none;"></asp:TextBox>
                </div>
            </ItemTemplate>
            <EditItemTemplate>
                <div class="itemArea">
                    Name:<asp:TextBox ID="new_name" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_name") %>'></asp:TextBox><br />
                    Hide from Menu?:
                    <asp:CheckBox ID="new_hide" runat="server" Checked='<%#IIf(Eval("cfolder_hide_flag") = "Y", "true", "false")%>' /><br />
                    Share with Other Users?
                    <asp:CheckBox ID="new_share" runat="server" Checked='<%#IIf(Eval("cfolder_share") = "Y", "true", "false")%>' /><br />
                    <asp:LinkButton ID="Cancel" runat="server" CommandName="Cancel" Text=" Cancel " CausesValidation="false"></asp:LinkButton>&nbsp;
                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Save" Text=" Save "
                        CausesValidation="false"></asp:LinkButton>
                    <asp:TextBox ID="id" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_id") %>'
                        Style="display: none;"></asp:TextBox>
                </div>
            </EditItemTemplate>
            <ReorderTemplate>
                <asp:Panel ID="Panel2" runat="server" CssClass="reorderCue" />
            </ReorderTemplate>
            <DragHandleTemplate>
                <asp:Label ID="Label2" runat="server" Text='<%# ToggleNewFolderIcon("N",DataBinder.Eval(Container.DataItem, "cfolder_method"), DataBinder.Eval(Container.DataItem, "cfolder_hide_flag"), DataBinder.Eval(Container.DataItem, "cfolder_share"))%>' />
            </DragHandleTemplate>
            <InsertItemTemplate>
                <div style="text-align: left; border-bottom: thin solid transparent;">
                    <asp:Panel ID="panel1" runat="server" DefaultButton="Button1">
                        Name:
                        <asp:TextBox ID="new_name" runat="server" Text='<%# Bind("cfolder_name") %>' /><br />
                        Hide from Menu?:
                        <asp:CheckBox ID="new_hide" runat="server" Checked='<%#IIf(Eval("cfolder_hide_flag") = "Y", "true", "false")%>' /><br />
                        Share with Other Users?
                        <asp:CheckBox ID="new_share" runat="server" Checked='<%#IIf(Eval("cfolder_share") = "Y", "true", "false")%>' /><br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter a Name"
                            ControlToValidate="new_name" /><br />
                        <asp:LinkButton ID="Button1" runat="server" CommandName="Insert" Text="Add"></asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Cancel" Text="Cancel"
                            CausesValidation="false"></asp:LinkButton>
                    </asp:Panel>
                </div>
            </InsertItemTemplate>
        </cc1:ReorderList>
    </div>
    <p align="center">
        <img src="images/done_with_changes.jpg" alt="Done With Changes" onclick="if(confirm('Are you finished with your changes and would like to exit?'))javascript:close_fold_window();" /></p>
    <asp:DataGrid runat="server" ID="datagrid_details" CellPadding="3" horizontal-align="left"
        OnUpdateCommand="MyDataGrid_Update" EnableViewState="true" ShowFooter="false"
        BackColor="White" Font-Size="8pt" Width="100%" OnDeleteCommand="MyDataGrid_Delete"
        AllowPaging="false" PageSize="25" CssClass="grid" Visible="true" OnEditCommand="MyDataGrid_Edit"
        OnCancelCommand="MyDataGrid_Cancel" BorderStyle="None" AllowSorting="True" AutoGenerateColumns="false"
        BorderColor="Gray">
        <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
            Font-Underline="True" ForeColor="White" Mode="NumericPages" NextPageText="Next"
            PrevPageText="Previous" />
        <AlternatingItemStyle CssClass="alt_row" />
        <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="Gray" Font-Size="8pt" />
        <HeaderStyle BackColor="#A8C1DD" Font-Bold="True" Font-Size="8pt" Font-Underline="True"
            ForeColor="Black" Wrap="False" HorizontalAlign="Left" VerticalAlign="Middle">
        </HeaderStyle>
        <Columns>
            <asp:EditCommandColumn EditText="Edit" UpdateText="Save" />
            <asp:TemplateColumn HeaderText="Name" ItemStyle-HorizontalAlign="left">
                <ItemTemplate>
                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "cfolder_name")), (DataBinder.Eval(Container.DataItem, "cfolder_name")), "")%>
                    <asp:TextBox runat="server" ID="name_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_name") %>'
                        Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="id_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_id") %>'
                        Style="display: none;" />
                    <asp:TextBox runat="server" ID="type_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_cftype_id") %>'
                        Style="display: none;" />
                    <asp:TextBox runat="server" ID="user_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_cliuser_id") %>'
                        Style="display: none;" />
                    <img src="images/spacer.gif" width="110" height="1" alt="" />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="name" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_name") %>'
                        Visible="true" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="dynamic"
                        ControlToValidate="name" ErrorMessage="Folder Name must be 1-50 characters."
                        ValidationExpression="^[\s\S]{0,50}$" EnableClientScript="true" />
                    <asp:TextBox runat="server" ID="name_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_name") %>'
                        Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="id_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_id") %>'
                        Style="display: none;" />
                    <asp:TextBox runat="server" ID="type_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_cftype_id") %>'
                        Style="display: none;" />
                    <asp:TextBox runat="server" ID="user_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_cliuser_id") %>'
                        Style="display: none;" />
                </EditItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Main Folder" ItemStyle-HorizontalAlign="left">
                <ItemTemplate>
                    <%#WhatFolder(DataBinder.Eval(Container.DataItem, "cfolder_cftype_id"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Share with Other Users" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <%#IIf(Eval("cfolder_share") = "Y", "<span class='green'>&#10004;</span>", "<span class='red'>-</span>")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBox ID="cfolder_share" runat="server" Checked='<%#IIf(Eval("cfolder_share") = "Y", "true", "false")%>' />
                </EditItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Sort" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "cfolder_sort2")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:LinkButton ID="move_up" CommandName="Up" Text="Up" runat="server" /></ItemTemplate>
                    &nbsp;
                    <asp:LinkButton ID="move_down" CommandName="Down" Text="Down" runat="server" /></ItemTemplate>
                    <img src="images/spacer.gif" width="40" alt="" />
                    <asp:TextBox runat="server" ID="cfolder_sort2" Text='<%# DataBinder.Eval(Container.DataItem, "cfolder_sort2") %>'
                        Style="display: none;" />
                </EditItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" CommandName="Delete" Text="Delete" runat="server"
                        OnClientClick="if(!confirm('Are you sure you wish to delete this Folder?'))return false;" /></ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

    <script type="text/javascript" language="javascript">
        function close_fold_window(){
         window.opener.location.reload(true);
         window.close();
       }
    </script>

</asp:Panel>
