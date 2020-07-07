<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/EmptyHomebaseTheme.Master" CodeBehind="homeMenuEditor.aspx.vb" Inherits="crmWebClient.homeMenuEditor" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyHomebaseTheme.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.12.1/themes/smoothness/jquery-ui.css" />
    <link href="common/aircraft_model.css" type="text/css" rel="stylesheet" />


    <script type="text/javascript" src="/common/moment-with-locales.js"></script>


    <style type="text/css">
        .addBox {
            text-align: left;
            width: 95%;
            margin-left: auto;
            margin-right: auto
        }


        .block.block-title .isFolder {
            background-image: url('../images/folder-2x.png') !important;
            background-repeat: no-repeat;
            background-position: 5px 50%;
            padding-left: 30px;
        }

        .block.block-title .notFolder {
            background-image: url('../images/file-2x.png');
            background-repeat: no-repeat;
            background-position: 5px 50%;
            padding-left: 30px;
        }

        .blue.second {
            text-indent: 25px;
            display: block;
        }


        .bold {
            font-weight: bold;
        }

        .callbackStyle table {
            background-color: #5377A9;
            color: Black;
        }

        .reorderListDemo table {
            margin: 0px 0px 0px 0px;
        }

        .reorderListDemo {
            clear: both;
        }

            .reorderListDemo table td {
                margin: 0px 0px 0px 0px;
                border: 0px;
            }

            .reorderListDemo li {
                list-style: none;
                margin: 0px;
                background-color: #f8f8f8;
                padding: 0px;
                color: #000000;
                display: block;
                border-bottom: 1px solid #868686;
            }

            .reorderListDemo ul {
                margin: 10px 0px 0px 20px;
                padding: 0px;
            }

                .reorderListDemo ul li td .display_block {
                    cursor: move;
                }

                .reorderListDemo ul li {
                    font-family: Arial;
                    font-size: 12px;
                    color: #4c4743;
                    padding: 0 10px;
                    line-height: 33px;
                    position: relative; /* cursor: move;*/
                    display: block;
                    margin: 5px 0;
                    border: 1px solid #f1e8e2;
                    background: #fff;
                }

        .reorderCue {
            border: dashed thin black;
            width: 100%;
            padding: 3px;
            height: 25px;
        }

        .itemArea {
            margin-left: 15px;
            font-family: Arial, Verdana, sans-serif;
            font-size: 16px;
            text-align: left;
            display: block;
            width: 100%;
            margin-bottom: 1px;
        }

        .block {
            margin: 5px 0;
            border: 1px solid #f1e8e2;
            background: #fff;
        }

        .block-title {
            font-family: Arial;
            font-size: 16px;
            color: #4c4743;
            padding: 0 10px;
            height: 33px;
            line-height: 33px;
            position: relative;
            display: block;
        }

        .sortable {
            list-style: none;
            padding-left: 0;
            margin: 0px 0px 0px 0px;
        }

            .sortable ul li {
                margin: 0px 0px 0px 0px;
            }

            .sortable ul {
                margin-left: 25px;
            }

        .ui-sortable-helper {
            box-shadow: rgba(0,0,0,0.15) 0 3px 5px 0;
            height: 33px !important;
        }

        .sortable-placeholder {
            height: 35px;
            background: #e3dcd7;
            margin-bottom: 5px;
            margin-top: 5px;
        }

        .addBox {
            border: 1px solid #f1e8e2;
            background-color: #eee;
            margin-left: 20px;
            width: 98%;
        }

        #addBox.itemArea {
            /*margin-top: -35px;*/
        }

        .addBox a {
            color: #000;
        }

        .itemArea h3 {
            font-size: 1.8em;
            margin-left: -35px;
            padding-bottom: 15px;
            margin-top: 7px;
        }

        .addBox h3 {
            padding-top: 7px;
            margin-left: 0px;
        }

        .worldLink {
            color: #2073a9;
            padding-right: 5px;
        }

        .visibilityHidden {
            visibility: hidden;
        }

        .itemArea .columns.two.iconsBox {
            width: 164px !important;
            padding-right: 15px;
            float: right !important;
        }

        .starMenu {
            padding-right: 5px;
            color: #ffbd00;
        }

        input[type=checkbox] {
            margin-left: 5px !important;
        }

        @media only screen and (max-width:650px) {
            .itemArea .twelve.columns, .itemArea .columns.two, .itemArea .columns.three, .itemArea .columns.four, .itemArea .columns.five, .itemArea .columns.six, .itemArea .columns.sixteen, .itemArea .columns.fourteen {
                width: 98% !important;
            }

                .itemArea .columns.two.iconsBox .worldLink {
                    float: left;
                    padding-right: 10px;
                }

            #addBox {
                width: 96%;
            }
        }

        #addBox {
            background-color: #fff;
            padding: 15px;
            border: 1px solid #aeaeae;
        }

        .viewTopLink {
            float: right;
            display: block;
            margin: 5px;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <a href="/homeMenuEditor.aspx?level=Main&parent=" class="float_right viewTopLink">View Top Menu</a>

    <cc1:ReorderList ID="Reorder_ListR" runat="server" PostBackOnReorder="true" CallbackCssStyle="callbackStyle" OnItemReorder="Reorder_ListR_ItemReorder"
        LayoutType="Table" CssClass="reorderListDemo" DragHandleAlignment="Left" DataKeyField="menutree_id" 
        SortOrderField="menutree_order" ShowInsertItem="false" OnItemCommand="Save_Row" AllowReorder="true" 
        ItemInsertLocation="Beginning" EnableViewState="true" Width="100%">
        <ItemTemplate>
            <div class="itemArea row">
                <div class="columns two iconsBox">
                    <asp:LinkButton ID="edit" Visible='true' runat="server" CommandName="Edit" CssClass="float_right"
                        CausesValidation="false">Edit</asp:LinkButton>
                </div>
                <div class="columns ten remove_margin">
                    <asp:Label ID="new_menutree_item_name" runat="server" Text='<%# CreateALink(Eval("menutree_item_name"), Eval("menutree_page_name"), Eval("itemSubCount"), Eval("menutree_display_name"))%>' />
                </div>
                <asp:TextBox ID="id" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "menutree_id") %>'
                    Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="menutree_item_name" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "menutree_item_name") %>'
                    Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="menutree_page_name_current" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "menutree_page_name") %>'
                    Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="menutree_display_url" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "menutree_display_url") %>'
                    Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="menutree_order_original" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "menutree_order") %>'
                    Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="menutree_description" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "menutree_description") %>'
                    Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="menutree_status" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "menutree_status") %>'
                    Style="display: none;"></asp:TextBox>
                <asp:DropDownList runat="server" ID="menutree_page_name" Width="100%" Style="display: none;">
                </asp:DropDownList>
            </div>
        </ItemTemplate>
        <EditItemTemplate>
            <div class="itemArea row">
                <h3>Edit Menu Item:</h3>
                <div class="sixteen columns">
                    <div class="row">
                        <div class="two columns">
                            Page:
                        </div>
                        <div class="nine columns">
                            <asp:TextBox ID="id" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "menutree_id") %>'
                                Style="display: none;"></asp:TextBox>
                            <asp:DropDownList runat="server" ID="menutree_page_name" Width="100%">
                            </asp:DropDownList>
                            <asp:TextBox ID="menutree_page_name_current" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "menutree_page_name") %>'
                                Style="display: none;"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="two columns">
                            <span style="color: red; cursor: help; text-decoration: underline;" title="Unique Name Required">Item Name</span>:
                        </div>
                        <div class="four columns"><asp:Label runat="server" ID="old_menutree_item_name" CssClass="display_none" Text='<%# DataBinder.Eval(Container.DataItem, "menutree_item_name") %>'></asp:Label>
                            <asp:TextBox ID="menutree_item_name" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "menutree_item_name") %>'></asp:TextBox>
                        </div>
                        <div class="two columns">
                            Display Name:
                        </div>
                        <div class="three columns">
                            <asp:TextBox ID="menutree_display_name" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "menutree_display_name") %>'></asp:TextBox>
                        </div>
                        <div class="one columns">
                        </div>
                        <div class="four columns">
                        </div>
                    </div>
                    <div class="row">
                        <div class="two columns">
                            Display URL:
                        </div>
                        <div class="nine columns">
                            <asp:TextBox ID="menutree_display_url" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "menutree_display_url") %>'></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">

                        <div class="one columns">
                            Admin Flag:
                        </div>
                        <div class="two columns">
                            <asp:CheckBox runat="server" ID="menutree_admin_flag" />
                        </div>
                        <div class="one columns">
                            Order:
                        </div>
                        <div class="one columns">
                            <asp:Label ID="menutree_order" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "menutree_order") %>'></asp:Label>
                        </div>
                        <div class="two columns">
                            Status:
                        </div>
                        <div class="three columns">
                            <asp:DropDownList runat="server" ID="menutree_status" Width="94%">
                                <asp:ListItem Selected="True" Value="T"></asp:ListItem>
                                <asp:ListItem Value="C"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="two columns">
                            Description:
                        </div>
                        <div class="nine columns">
                            <asp:TextBox ID="menutree_description" runat="server" Width="100%" TextMode="MultiLine"
                                Rows="6" Text='<%# DataBinder.Eval(Container.DataItem, "menutree_description") %>'></asp:TextBox>
                        </div>
                    </div>
                    <div class="eleven remove_margin columns">
                        <asp:LinkButton ID="Cancel" runat="server" CommandName="Cancel" Text=" Cancel " ForeColor="Black"
                            CssClass="float_left" CausesValidation="false"></asp:LinkButton>&nbsp;
              <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Save" Text=" Save "
                  CssClass="float_right" CausesValidation="true" ForeColor="Black" ValidationGroup="saveForm"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </EditItemTemplate>
        <ReorderTemplate>
            <asp:Panel ID="Panel2" runat="server" CssClass="reorderCue" />
        </ReorderTemplate>
        <DragHandleTemplate>
            <asp:Label ID="Label2" runat="server" CssClass="display_block" Text='<%# FolderClassDisplay(Eval("itemSubCount").ToString)%>' />
        </DragHandleTemplate>
    </cc1:ReorderList>
    <div class="reorderListDemo addBox">
        <table class="float_right">
            <tr>
                <td>
                    <input type="button" value="Add" text="Add" id="addButton" title="Add"
                        onclick="$('#addBox').toggle(); $('#addButton').hide(); $('#cancelButton').show(); return false" />
                </td>
            </tr>
        </table>
        <div class="clear">
        </div>
        <div class="itemArea twelve columns" id="addBox" style="display: none;">
            <h3>Add Menu Item:</h3>
            <asp:ValidationSummary runat="server" ID="valSummaryNew" ValidationGroup="addNewForm"
                DisplayMode="BulletList" ShowMessageBox="true" ShowSummary="false" />
            <div class="sixteen columns">
                <div class="row">
                    <div class="two columns">
                        Page Name:
                    </div>
                    <div class="four columns">
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="new_menutree_page_name"
                            ErrorMessage="Page Name Required." ValidationGroup="addNewForm" Display="None"></asp:RequiredFieldValidator>
                        <asp:DropDownList runat="server" ID="new_menutree_page_name" Width="100%">
                        </asp:DropDownList>
                        <asp:TextBox runat="server" ID="menutree_page_name_hidden" CssClass="display_none"></asp:TextBox>
                    </div>
                    <div class="two columns">
                        Admin:
                    </div>
                    <div class="four columns">
                        <asp:CheckBox CssClass="float_left" runat="server" ID="new_menutree_admin_flag" />
                    </div>
                </div>
                <div class="row">
                    <div class="two columns">
                        <span style="color: red; cursor: help; text-decoration: underline;" title="Unique Name Required">Item Name</span>:
                    </div>
                    <div class="four columns">
                        <asp:CustomValidator ID="CustomValidator4" ControlToValidate="new_menutree_item_name"
                            ValidationGroup="addNewForm" Display="None" ErrorMessage="Please change your item name."
                            runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="new_menutree_item_name"
                            ErrorMessage="Item Name Required." ValidationGroup="addNewForm" Display="None"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="new_menutree_item_name" runat="server" Width="100%" Text=''></asp:TextBox>
                    </div>
                    <div class="two columns">
                        Display Name:
                    </div>
                    <div class="four columns">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="new_menutree_display_name"
                            ErrorMessage="Display Name Required." ValidationGroup="addNewForm" Display="None"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="new_menutree_display_name" runat="server" Width="100%" Text=''></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="two columns">
                        Display URL:
                    </div>
                    <div class="ten columns">
                        <asp:CustomValidator runat="server" ID="CustomValidator5" ControlToValidate="new_menutree_display_url" Display="Dynamic" ValidationGroup="addNewForm" ErrorMessage="<p>You have already saved a page with the same name. Please modify the page name to be unique.</p>"></asp:CustomValidator>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="new_menutree_display_url" ErrorMessage="URL Required."
                            ValidationGroup="addNewForm" Display="None"></asp:RequiredFieldValidator>
                        <asp:CustomValidator runat="server" ID="CustomValidator6" ControlToValidate="new_menutree_display_url" Display="Dynamic" ValidationGroup="addNewForm" ErrorMessage="<p>Please enter a properly formed URL.</p>"></asp:CustomValidator>
                        <asp:TextBox ID="new_menutree_display_url" runat="server" Width="100%" Text=''></asp:TextBox>
                    </div>
                </div>
                <div class="row">

                    <div class="two columns">
                        Order:
                    </div>
                    <div class="four columns">
                        <asp:Label runat="server" ID="new_menutree_order" Width="100%"></asp:Label>
                        <asp:TextBox runat="server" ID="menutree_order_hidden" CssClass="display_none"></asp:TextBox>
                    </div>
                    <div class="two columns">
                        Status:
                    </div>
                    <div class="four columns">
                        <asp:DropDownList runat="server" ID="new_menutree_status" Width="94%">
                            <asp:ListItem Selected="True" Value="T"></asp:ListItem>
                            <asp:ListItem Value="C"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="two columns">
                        Description:
                    </div>
                    <div class="twelve columns">
                        <asp:TextBox ID="new_menutree_description" runat="server" Width="100%" Text='' TextMode="MultiLine"
                            Rows="6"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="fourteen columns">
                        <input type="button" class="float_left" value="Cancel" id="cancelButton" style="display: none;" title="Cancel Add" onclick="$('#addBox').toggle(); $('#addButton').show(); $('#cancelButton').hide();" />
                        <asp:Button ID="saveNewMenu" runat="server" Text="Save" CssClass="float_right" CausesValidation="true" ValidationGroup="addNewForm" />
                    </div>
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
    <asp:TextBox runat="server" ID="parent_area_name" CssClass="display_none">Main</asp:TextBox>
    <asp:TextBox runat="server" ID="accessID" CssClass="display_none"></asp:TextBox>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

    <script type="text/javascript">

        function ActiveTabChanged(sender, args) {

            var nextTab = sender.get_activeTab().get_id();

            if (nextTab.indexOf("tab1") > 0) {
                //alert("finder preferences");
                //swapChosenDropdowns();
            }

        }

        function ShowLoadingMessage(DivTag, Title, Message) {
            $("#" + DivTag).html(Message);
            $("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 75, resizable: false });
        }

        function CloseLoadingMessage(DivTag) {
            $("#" + DivTag).dialog("close");
        }

    </script>

</asp:Content>
