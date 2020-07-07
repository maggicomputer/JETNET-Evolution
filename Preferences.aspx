<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="Preferences.aspx.vb" Inherits="crmWebClient.Preferences"
    MaintainScrollPositionOnPostback="true" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="Stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.12.1/themes/smoothness/jquery-ui.css" />

    <style type="text/css">
        .charactersLeft {
            padding-left: 16px;
            color: Red;
        }

        .defaultModel, .defaultModel a {
            color: #0c95a4 !important;
        }

        .large {
            font-size: 16px;
            margin: 15px;
        }

        #sortable1, #sortable2 {
            border: 1px solid #eee;
            width: 95%;
            min-height: 20px;
            list-style-type: none;
            margin: 15px;
            padding: 5px 0 0 0;
            float: left;
            margin-right: 10px;
            max-height: 500px;
            overflow: auto;
        }

            #sortable1 li, #sortable2 li {
                margin: 0 5px 0px 5px;
                padding: 5px;
                font-size: 1.2em;
                border-bottom: 1px solid #eee;
                text-decoration: underline;
            }



        .ui-state-disabled {
            color: Black;
            text-transform: uppercase;
            font-weight: bold;
        }

            .ui-state-disabled:hover {
                text-decoration: none !important;
                cursor: default !important;
                color: Black !important;
            }

        #connectedSortable .area {
            font-size: 1.5em !important;
            color: #078fd7 !important;
        }

            #connectedSortable .area:hover {
                color: #078fd7 !important;
            }

        #connectedSortable .indent {
            margin-left: 15px;
        }

        #sortable1 .ui-state-default:hover, #sortable2 .ui-state-default:hover {
            text-decoration: underline;
            color: #2c93ac;
            cursor: move;
        }

        .moduleHeaderText {
            font-size: 2em;
            margin-left: 15px;
        }

        #sortable1 .ui-state-default:hover, #sortable2 .ui-state-default:hover a:hover {
            color: #2c93ac;
        }
    </style>
    <script type="text/javascript">
        var bDontClose = false;

        var bClickedSave = <%= bRefreshSession.ToString.Tolower %>;

        var bChangeSubFlag = false;
        var currentTab = "<%= currentActiveTab.ToString.Tolower %>";
        var tabContainer = "<%= tab_container_ID.ClientID.ToString %>";
        var bShowDialog = <%= bShowChatChangeDialog.ToString.Tolower %>;

        var sessGUID = "<%= sChatChangeGUID.Trim %>";
        var txtAlias = "<%= sChatChangeUserAlias.Trim %>";
        var bEnable = <%= bChatChangeEnable.ToString.Tolower %>;
        var bChangeSub = <%= bChatChangeSub.ToString.Tolower %>;

        var bSiteChatEnabled = <%= bIsSiteChatEnabled.ToString.Tolower %>;

        var aerodexFlag = <%= Session.Item("localPreferences").AerodexFlag.ToString.Tolower %>;
        var sessionGuid = "<%= Session.Item("localPreferences").sessionGuid.ToString %>";

        function openSmallWindowJS(address, windowname) {
            var rightNow = new Date();
            windowname += rightNow.getTime();
            var Place = open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1050,resizable=yes,toolbar=no,location=no,status=no");
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" name="previousTab" value="<%= currentActiveTab.ToString.Tolower %>"
        id="previousTabID" />
    <asp:Label ID="debug_label" runat="server" Text="" BackColor="#eeeeee"></asp:Label>
    <div id="outerDivPreferencesIDForm" runat="server" class="myEvolution" width="80%">
        <asp:Table ID="buttonsTable" CellPadding="3" CellSpacing="0" Width="100%" CssClass="DetailsBrowseTable"
            runat="server">
            <asp:TableRow>
                <asp:TableCell runat="server" HorizontalAlign="right" VerticalAlign="middle" Style="padding-right: 4px;"
                    Width="23%">
                    <div class="backgroundShade">
                        <a href="help.aspx" target="_blank" class="float_left">
                            <img src="/images/help-circle.svg" alt="Help" /></a> <span class="float_right">
                                <a href="#" onclick="javascript:window.close();" class="float_right seperator">
                                    <img src="/images/x.svg" alt="Close" /></a> </span>
                        <asp:LinkButton ID="save_button1" runat="server" CssClass="gray_button noBefore float_left"
                            Visible="true" OnClientClick="javascript:ShowPreferencesMessage('DivPreferencesMessage','Saving Preferences','Saving Preferences ... Please Wait ...');return true;"><img src="/images/save.svg" /></asp:LinkButton>
                        <a href="javascript:void(0);" id="sortSaveTop" class="gray_button noBefore float_left" style="display: none !important;">
                            <img src="/images/save.svg" /></a>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <cc1:TabContainer runat="server" ID="tab_container_ID" Width="100%" ActiveTabIndex="0"
            OnClientActiveTabChanged="ActiveTabChanged" BorderStyle="None" Style="margin-left: auto; margin-right: auto;"
            CssClass="dark-theme">
            <cc1:TabPanel ID="my_account" runat="server" HeaderText="Account">
                <HeaderTemplate>
                    Account
                </HeaderTemplate>
                <ContentTemplate>
                    <table width="100%" cellspacing="0" cellpadding="3">
                        <tr>
                            <td align="left" valign="top">
                                <h1>Subscription&nbsp;Summary</h1>
                            </td>
                            <td align="left" valign="top" width="78%">
                                <div class="seperator_line">
                                    &nbsp;
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table id="actinfo_subscriber_company_table" width="100%" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td colspan="4" class="subheading">
                                            <asp:Label runat="server" ID="actinfo_subscriber_company_id">SUBSCRIBER COMPANY: ID[ <%=Session.Item("localPreferences").UserCompanyID.ToString.Trim%> ]</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" rowspan="7" width="60">
                                            <img src="images/info.jpg" alt="Information" width="60" />
                                        </td>
                                        <td align="left" valign="middle">
                                            <asp:Label runat="server" ID="actinfo_company_name"></asp:Label>
                                        </td>
                                        <td align="left" valign="middle" colspan="2">
                                            <asp:Label runat="server" ID="actinfo_office"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="middle">
                                            <asp:Label runat="server" ID="actinfo_address1"></asp:Label>
                                            <asp:Label runat="server" ID="actinfo_address2"></asp:Label>
                                        </td>
                                        <td align="left" valign="middle" colspan="2">
                                            <asp:Label runat="server" ID="actinfo_fax"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="middle">
                                            <asp:Label runat="server" ID="actinfo_city"></asp:Label>
                                            <asp:Label runat="server" ID="actinfo_state"></asp:Label>
                                            <asp:Label runat="server" ID="actinfo_zipcode"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="actinfo_toll"></asp:Label>
                                            <br />
                                        </td>
                                        <td style="text-align: right;">
                                            <input type="button" id="actinfo_subscriber_support" value="Customer Support" onclick="changeTab(<%= nSupportTabIndex.ToString %>);" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="middle" colspan="3">
                                            <asp:Label runat="server" ID="actinfo_country"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="middle" colspan="3">
                                            <asp:Label runat="server" ID="actinfo_email"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="middle" colspan="3">
                                            <asp:Label runat="server" ID="actinfo_website"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="middle" colspan="3">
                                            <asp:Label runat="server" ID="actinfo_business_type"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table id="actinfo_subscriber_contact_ID_table" width="100%" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td class="subheading" colspan="3">
                                            <asp:Label runat="server" ID="actinfo_subscriber_contact_ID">SUBSCRIBER CONTACT: ID[ <%=Session.Item("localPreferences").UserContactID.ToString.Trim%> ]</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" valign="top">
                                            <table width="100%" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td align="left" valign="top" width="40%">
                                                        <!--contact information display-->
                                                        <table width="100%" cellpadding="3" cellspacing="0">
                                                            <tr>
                                                                <td align="left" valign="top" rowspan="3" width="60">
                                                                    <asp:Image ID="actinfo_contact_image" runat="server" ImageUrl="images/contact.jpg"
                                                                        Width="60px" />
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:Label runat="server" ID="actinfo_contact_name"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" valign="middle">
                                                                    <asp:Label runat="server" ID="actinfo_contact_title"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" valign="middle">
                                                                    <asp:Label runat="server" ID="actinfo_contact_email"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <!---picture display-->
                                                    <td align="left" valign="top" width="60%">
                                                        <asp:Panel runat="server" ID="Panel1" CssClass="border_left">
                                                            <p class="tiny_text emphasis_text">
                                                                JETNET Provides the ability to display your picture next to your information on
                                the company and contact pages. Use the 'Upload Image' button below to load your
                                image for display.
                                                            </p>
                                                            <asp:Label runat="server" ID="actinfo_contact_image_large"></asp:Label><br />
                                                            <br />
                                                            <asp:Button runat="server" Text="Remove Image" ID="actinfo_contact_edit_image_button_remove"
                                                                CssClass="specialButton float_right" OnClientClick="javascript:ShowPreferencesMessage('DivPreferencesMessage','Remove Image','Removing Image ... Please Wait ...');return true;"
                                                                Visible="False" />
                                                            <asp:Button runat="server" Text="Change Image" ID="actinfo_contact_edit_image_button"
                                                                CssClass="specialButton float_right" OnClientClick="javascript:ShowPreferencesMessage('DivPreferencesMessage','Change Image','Initializing ... Please Wait ...');return true;"
                                                                Visible="False" />
                                                            <asp:Label runat="server" ID="actinfo_contact_image_attention" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                            <asp:Panel runat="server" ID="actinfo_contact_edit_image_panel" Visible="False">
                                                                <div class="border_bottom">
                                                                </div>
                                                                <asp:FileUpload ID="actinfo_contact_file_upload" runat="server" />
                                                                <asp:Button runat="server" Text="Upload" ID="actinfo_image_upload_button" CssClass="specialButton"
                                                                    OnClientClick="javascript:ShowPreferencesMessage('DivPreferencesMessage','Upload Image','Uploading Image ... Please Wait ...');return true;" />
                                                                <div class="border_bottom">
                                                                </div>
                                                            </asp:Panel>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table id="actinfo_subscription_information_table" width="100%" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td class="subheading" colspan="4">SUBSCRIPTION INFORMATION
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" rowspan="5" width="60">
                                            <img src="images/person.jpg" alt="Information" width="60" />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="subscription_username"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label runat="server" ID="subscription_email"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="subscription_subscription_id"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label runat="server" ID="subscription_platform"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="subscription_marketing_account"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label runat="server" ID="subscription_demo_account"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="subscription_max_export" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="subscription_privilege" runat="server"></asp:Label>
                                        </td>
                                        <tr>
                                            <td>
                                                <asp:Label ID="subscription_share" runat="server"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="subscription_show_on_global" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    <tr>
                                        <td colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <!--Start HomeBase User Data Block-->
                                    <tr runat="server" id="homebaseUserInformationPanel" visible="False">
                                        <td align="left" valign="top" colspan="4" runat="server">
                                            <table id="actinfo_homebase_information_table" width="100%" cellpadding="3" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top" rowspan="5" width="60">
                                                        <img src="images/homebase_watermark.jpg" alt="Information" width="60" />
                                                    </td>
                                                    <td width="54%">
                                                        <asp:Label runat="server" ID="homebaseUser_UserID"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label runat="server" ID="homebaseUser_AccountID"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="homebaseUser_userType"></asp:Label>
                                                    </td>
                                                    <td colspan="2"></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <hr />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <!--End HomeBase User Data Block-->
                                    <tr>
                                        <td align="left" rowspan="6" valign="top" width="60">
                                            <img alt="Tiers" src="images/tiers.jpg" width="60" />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="subscription_tier"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="subscription_aerodex" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="subscription_business" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="subscription_helicopter" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="subscription_commercial" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="subscription_yacht" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="subscription_spi_view" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="subscription_star" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="subscription_service_code" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" rowspan="3" valign="top" width="60">
                                            <img alt="Display" src="images/display_watermark.jpg" width="60" />
                                        </td>
                                        <td>
                                            <asp:Label ID="subscription_default_model" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="subscription_default_business_segment" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align: right;">
                                            <input id="subscription_display_modify" onclick="changeTab(1);" type="button" value="Modify" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="subscription_default_background" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="subscription_default_analysis_months" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" rowspan="7" valign="top" width="60">
                                            <img alt="Services" src="images/services_watermark.jpg" width="60" />
                                        </td>
                                        <td></td>
                                        <td>
                                            <asp:Label ID="subscription_default_project" runat="server"></asp:Label>
                                        </td>
                                        <td rowspan="3" style="text-align: right;">
                                            <!-- will need to change index if models or both are missing -->
                                            <input id="subscription_services_modify" onclick="changeTab(<%= nServicesTabIndex.ToString %>);"
                                                type="button" value="Modify" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="subscription_mobile_number" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="subscription_default_email_format" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="subscription_default_reply_email" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="subscription_default_reply_name" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="subscription_SMS_txt_msg" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="subscription_SMS_provider" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="subscription_SMS_events" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="subscription_SMS_models" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="subscription_chat_enabled" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" rowspan="3" valign="top" width="60">
                                            <img alt="Notes" src="images/notes_watermark.jpg" width="60" />
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="subscription_server_notes" runat="server"></asp:Label>
                                        </td>
                                        <td rowspan="2" style="text-align: right;">
                                            <!-- have to remove modify button when Notes is not present or change index if model tab is off -->
                                            <% If bCanUseLocalNotes Then%>
                                            <input id="subscription_notes_modify" onclick="changeTab(<%= nNotesTabIndex%>);"
                                                type="button" value="Modify" />
                                            <% End If%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table width="100%" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>Auto&nbsp;Login</h1>
                                        </td>
                                        <td align="left" valign="top" width="87%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table id="actinfo_auto_login_table" width="100%" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td width="60px">&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="actinfo_auto_login_checkbox" runat="server" Text="<strong>Auto Login Enabled</strong> - <em>Uncheck to disable</em>" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table width="100%" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>Change&nbsp;Password</h1>
                                        </td>
                                        <td align="left" valign="top" width="82%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <% If Not (bDemoUser Or bMarketingUser) Then%>
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table id="actinfo_change_password_table" width="100%" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td align="left" valign="top" rowspan="4" width="60">
                                            <img src="images/tools.jpg" alt="Change Password" width="60" />
                                        </td>
                                        <td width="30%">Old Password:
                                        </td>
                                        <td width="70%">
                                            <asp:TextBox ID="oldPasswordID" runat="server" TextMode="Password" Text="test"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>New Password:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="newPasswordID" runat="server" TextMode="Password" Text="test"></asp:TextBox>
                                            &nbsp;&nbsp;<asp:Image ID="actinfo_password_mouseover_img" Height="15px" runat="server"
                                                ImageUrl="/images/info.png" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Confirm Password:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="confirmPasswordID" runat="server" TextMode="Password" Text="test"
                                                TabIndex="1"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <% End If%>
                    </table>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="my_display" runat="server" HeaderText="Display">
                <HeaderTemplate>
                    Display
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Table ID="display_defaults_content_table" runat="server" Width="100%" CellPadding="3"
                        CellSpacing="0">
                        <asp:TableRow>
                            <asp:TableCell ID="display_defaults_label_cell" HorizontalAlign="left" VerticalAlign="top"
                                runat="server">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>
                                                <asp:Literal ID="display_defaults_label" runat="server"></asp:Literal>
                                            </h1>
                                        </td>
                                        <td align="left" valign="top" width="68%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <img src="images/world.jpg" alt="World" width="60" />
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:Table ID="display_default_model_view_bkground_table" runat="server" Width="100%"
                                                            CellPadding="3" CellSpacing="0">
                                                            <asp:TableRow>
                                                                <asp:TableCell ID="display_defaults_model_view_bkground_text_cell" runat="server"
                                                                    HorizontalAlign="left" VerticalAlign="middle" ColumnSpan="2">
                                  <p style="text-align: left; padding: 5px 5px 15px 5px;">
                                    Set a Background and/or a Model as a default model for views when you first login. To select a Background and/or a
                                    Model as a default just select your desired Background and/or a Model, and then click the 'Save' button above to remember your selections.
                                    </p>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell ID="display_default_view_cell" runat="server" HorizontalAlign="left"
                                                                    VerticalAlign="middle" Wrap="false">
                                                                    <b>Default&nbsp;View&nbsp;:&nbsp;</b><br />
                                                                    <asp:Label runat="server" ID="display_default_viewID"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="display_default_view_cell_1" runat="server" HorizontalAlign="left"
                                                                    VerticalAlign="middle" Wrap="false">
                                                                    <asp:CheckBox ID="display_reset_default_viewID" runat="server" Text="Reset Default Home View" />
                                                                    &nbsp;&nbsp;<asp:Image ID="mydisplay_default_view_img" Height="15px" runat="server"
                                                                        ImageUrl="/images/info.png" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell ID="display_default_model_cell" runat="server" HorizontalAlign="left"
                                                                    VerticalAlign="middle" Wrap="false">
                                                                    <b>Default&nbsp;Model&nbsp;:&nbsp;</b><br />
                                                                    <asp:Label runat="server" ID="display_default_modelID"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="display_default_model_cell_1" runat="server" HorizontalAlign="left"
                                                                    VerticalAlign="middle">
                                                                    <asp:DropDownList ID="choose_default_modelID" runat="server">
                                                                    </asp:DropDownList>
                                                                    <br />
                                                                    <em>Choose default model for all views</em>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell ID="display_default_bkground_cell" runat="server" HorizontalAlign="left"
                                                                    VerticalAlign="middle" Wrap="false">
                                                                    <b>Default&nbsp;Background&nbsp;:&nbsp;</b><br />
                                                                    <asp:Label runat="server" ID="display_default_backgroundID"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="display_default_bkground_cell_1" runat="server" HorizontalAlign="left"
                                                                    VerticalAlign="middle">
                                                                    <asp:DropDownList ID="choose_default_backgroundID" runat="server">
                                                                        <asp:ListItem Value="0">Random</asp:ListItem>
                                                                        <asp:ListItem Value="17">White</asp:ListItem>
                                                                        <asp:ListItem Value="18">Light Gray</asp:ListItem>
                                                                        <asp:ListItem Value="19">Light Brown</asp:ListItem>
                                                                        <asp:ListItem Value="20">Light Blue</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <br />
                                                                    <em>Choose default Background</em>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ID="display_defaults_records_per_page_cell" HorizontalAlign="left"
                                VerticalAlign="top" runat="server">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>Records Per Page</h1>
                                        </td>
                                        <td align="left" valign="top" width="82%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top" width="60">
                                                        <img src="images/number.jpg" alt="Number of Records" width="60" />
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <p>
                                                            To provide you with the maximum search and display speed over the web, search results
                              will return sets of information to you based on your needs. The number of records
                              per page identifies the number of records that will be returned in each data set
                              without requiring you to click &quot;Next Page&quot;.
                                                        </p>
                                                        Number of Records Per Page:&nbsp;
                            <asp:DropDownList ID="display_records_per_page_ddl" runat="server">
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>40</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>60</asp:ListItem>
                                <asp:ListItem>70</asp:ListItem>
                                <asp:ListItem>80</asp:ListItem>
                                <asp:ListItem>90</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                                <asp:ListItem>200</asp:ListItem>
                                <asp:ListItem>300</asp:ListItem>
                                <asp:ListItem>400</asp:ListItem>
                                <asp:ListItem>500</asp:ListItem>
                            </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ID="display_defaults_ac_relationship_cell" HorizontalAlign="left"
                                VerticalAlign="top" runat="server">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>Relationship to Aircraft</h1>
                                        </td>
                                        <td align="left" valign="top" width="78%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top" width="60">
                                                        <img src="images/relationship.jpg" alt="Relationships" width="60" />
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <p>
                                                            Please select the check box below if you would like to enable your currently selected
                              &quot;Relationship to Aircraft&quot; selection to be saved as <b>YOUR default &quot;Relationship
                                to Aircraft&quot;</b> for aircraft searches.
                              <asp:Image ID="mydisplay_relationship_img" Height="15px" runat="server" ImageUrl="/images/info.png" />
                                                        </p>
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td align="left" colspan="2" valign="top">
                                                                    <b>Enable Default Feature</b>&nbsp;<asp:CheckBox ID="mydisplay_enabled_default_feature"
                                                                        runat="server" /><br />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" valign="top">Your Current default relationship(s) are:
                                  <asp:Label ID="mydisplay_default_relationships_value" runat="server" ForeColor="Red"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:RadioButton ID="mydisplay_default_relationships" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" valign="top">You currently have selected these relationship(s):
                                  <asp:Label ID="mydisplay_selected_relationships_value" runat="server" ForeColor="Blue"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:RadioButton ID="mydisplay_selected_relationships" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ID="display_default_business_segment" HorizontalAlign="left" VerticalAlign="top"
                                runat="server">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>User Perspective</h1>
                                        </td>
                                        <td align="left" valign="top" width="82%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top" width="60">
                                                        <img src="images/tools.jpg" alt="User Perspective" width="70" />
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <p>
                                                            Evolution allows users to customize some elements of their interface based on their
                              user perspective. For example: If a subscriber sets their User Perspective to “Dealer/Broker”
                              then they will have different Home page tabs than if they had selected a User Perspective
                              of “Fixed Based Operator”. Select the User Perspective below that best matches your
                              needs. Click <a href="/help/documents/596.pdf" target="new">HERE</a> to learn more
                                                        </p>
                                                        User Perspective:&nbsp;
                            <asp:DropDownList ID="display_business_segment_ddl" runat="server">
                                <asp:ListItem Value="DB">Dealer/Broker</asp:ListItem>
                                <asp:ListItem Value="FB">Fixed Base Operator</asp:ListItem>
                                <asp:ListItem Value="UI">Unidentified</asp:ListItem>
                            </asp:DropDownList>
                                                        &nbsp; Default Analysis Timeframe (Months):&nbsp;
                            <asp:DropDownList ID="default_analysis_months_ddl" runat="server">
                                <asp:ListItem Value="6">6 months</asp:ListItem>
                                <asp:ListItem Value="12">12 months</asp:ListItem>
                                <asp:ListItem Value="18">18 months</asp:ListItem>
                            </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ID="display_blank_fields_on_aircraft" HorizontalAlign="left" VerticalAlign="top"
                                runat="server">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>Show Blank Fields for Aircraft</h1>
                                        </td>
                                        <td align="left" valign="top" width="72%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top" width="60">
                                                        <img src="images/tools.jpg" alt="Show Blank Fields for Aircraft" width="70" />
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <p>
                                                            Evolution allows users to display aircraft details in two different formats<br />
                                                            (1) Display Aircraft in Condensed Format showing all critical fields and those with
                              information filled in<br />
                                                            (2) Display Aircraft in Expanded Format showing all fields even if blank providing
                              more of a template for filling in specifications.
                                                        </p>
                                                        Format:&nbsp;
                            <asp:DropDownList ID="display_no_blank_fields_on_aircraft_ddl" runat="server">
                                <asp:ListItem Value="CF">Display Aircraft in Condensed Format (Do Not Display Blank Fields)</asp:ListItem>
                                <asp:ListItem Value="EF">Display Aircraft in Expanded Format (Display Blank Fields)</asp:ListItem>
                            </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ID="display_values" HorizontalAlign="left"
                                VerticalAlign="top" runat="server">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>Values</h1>
                                        </td>
                                        <td align="left" valign="top" width="78%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top" width="60">
                                                        <img src="images/tools.jpg" alt="Values" width="60" />
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <p>
                                                            Please select the check box below if you would like to enable display of eValues in Market area pages/reports.
                              <asp:Image ID="mydisplay_evalues_img" Height="15px" runat="server" ImageUrl="/images/info.png" />
                                                        </p>
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td align="left" colspan="2" valign="top">
                                                                    <b>Enable display of eValues</b>&nbsp;<asp:CheckBox ID="display_valuesChk"
                                                                        runat="server" /><br />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ID="TableCell7" HorizontalAlign="left"
                                VerticalAlign="top" runat="server">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>Options</h1>
                                        </td>
                                        <td align="left" valign="top" width="78%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top" width="60">
                                                        <img src="images/tools.jpg" alt="Values" width="60" />
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <p>
                                                            Turn on/off aircraft page option to tell JETNET about changes.
                              <asp:Image ID="Image1" Height="15px" runat="server" ImageUrl="/images/info.png" />
                                                        </p>
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td align="left" colspan="2" valign="top">
                                                                    <b>Disable option to tell JETNET about changes.</b>&nbsp;<asp:CheckBox ID="disableTellAbout"
                                                                        runat="server" /><br />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="my_models" runat="server" HeaderText="Models">
                <HeaderTemplate>
                    Models
                </HeaderTemplate>
                <ContentTemplate>
                    <table width="100%" cellpadding="3" cellspacing="0">
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>Default Market Model(s)</h1>
                                        </td>
                                        <td align="left" valign="top" width="75%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="2" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top" rowspan="3" width="60">
                                                        <img src="images/autologin.jpg" alt="Default Models" width="60" />
                                                    </td>
                                                    <td align="center" valign="top" colspan="2">
                                                        <p class="nonflyout_info_box">
                                                            <asp:Label ID="models_info_lbl" runat="server"></asp:Label>
                                                        </p>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" style="width: 50%;">
                                                        <asp:ListBox ID="models_model_lb" runat="server" Rows="16" SelectionMode="Multiple"></asp:ListBox>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:ListBox ID="models_models_picked_lb" runat="server" Rows="16" SelectionMode="Multiple"></asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <br />
                                                        <asp:Button ID="models_move_all_left" runat="server" Text=" << " />&nbsp;
                            <asp:Button ID="models_move_left" runat="server" Text=" < " />&nbsp;
                            <asp:Button ID="models_move_right" runat="server" Text=" > " />&nbsp;
                            <asp:Button ID="models_move_all_right" runat="server" Text=" >> " />
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="my_airports" runat="server" HeaderText="Airports">
                <HeaderTemplate>
                    Airports
                </HeaderTemplate>
                <ContentTemplate>
                    <table width="100%" cellpadding="3" cellspacing="0">
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>Default Airport(s)</h1>
                                        </td>
                                        <td align="left" valign="top" width="75%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <asp:Table ID="Table3" runat="server" Width="100%" CellPadding="3" CellSpacing="0">
                                                <asp:TableRow>
                                                    <asp:TableCell ID="TableCell5" HorizontalAlign="left" VerticalAlign="top" runat="server">
                                                        <asp:Label runat="server" ID="default_airport_label" Visible="true" Width="90%"></asp:Label>
                                                        <asp:DropDownList ID="default_airport_ddl" runat="server" AutoPostBack="true" Visible="false">
                                                            <asp:ListItem></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell6" HorizontalAlign="left" VerticalAlign="top" runat="server">
                                                        <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="openSmallWindowJS('FolderMaintenance.aspx?t=17&newStaticFolder=true&default=true','FolderMaintenanceEditor');"
                                                            PostBackUrl="" Visible="false"><strong>Add Default Airport Folder</strong></asp:LinkButton>
                                                        &nbsp;&nbsp;
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="" PostBackUrl="" Visible="false"><strong>Edit Default Airport Folder</strong></asp:LinkButton>
                                                        &nbsp;&nbsp;
                            <asp:LinkButton ID="LinkButton3" runat="server" PostBackUrl="" Visible="false"><strong>Save This Folder As Default Airport Folder</strong></asp:LinkButton>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell ColumnSpan="2">
                                                        <div runat="server" id="div_airport_results_table">
                                                            <div style="text-align: left; width: 100%;" runat="server" id="airportResults">
                                                                <asp:Label runat="server" ID="airportTable"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="my_region" runat="server" HeaderText="Region">
                <HeaderTemplate>
                    Regions
                </HeaderTemplate>
                <ContentTemplate>
                    <table width="100%" cellpadding="3" cellspacing="0">
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>Default Market Region(s)</h1>
                                        </td>
                                        <td align="left" valign="top" width="75%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top" width="60" rowspan="3">
                                                        <img src="images/autologin.jpg" alt="Default Regions" width="60" />
                                                    </td>
                                                    <td align="left" valign="top" colspan="2">
                                                        <p class="nonflyout_info_box">
                                                            <asp:Label ID="regions_info_lbl" runat="server"></asp:Label>
                                                        </p>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" style="width: 50%;">
                                                        <asp:ListBox ID="regions_region_lb" runat="server" Rows="8" SelectionMode="Multiple"></asp:ListBox>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:ListBox ID="regions_region_picked_lb" runat="server" Rows="8" SelectionMode="Multiple"></asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <asp:Button ID="regions_move_all_prev_btn" runat="server" Text=" << " />&nbsp;
                            <asp:Button ID="regions_move_one_prev_btn" runat="server" Text=" < " />&nbsp;
                            <asp:Button ID="regions_move_one_for_btn" runat="server" Text=" > " />&nbsp;
                            <asp:Button ID="regions_move_all_for_btn" runat="server" Text=" >> " />
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="my_services" runat="server" HeaderText="Services">
                <HeaderTemplate>
                    Services
                </HeaderTemplate>
                <ContentTemplate>
                    <table width="100%" cellpadding="3" cellspacing="0">
                        <tr>
                            <td align="left" valign="top">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>Mobile</h1>
                                        </td>
                                        <td align="left" valign="top" width="93%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <!-- -->
                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="left" valign="top" rowspan="2" width="60">
                                                        <img src="images/mobile_left.jpg" alt="Mobile" width="60" />
                                                    </td>
                                                    <td align="left" valign="middle" rowspan="2" colspan="2">
                                                        <p style="text-align: left; padding-right: 8px; padding-top: 10px; color: Maroon;">
                                                            To use the Evolution mobile website, use the <strong><a href="http://www.jetnetevomobile.com"
                                                                title="http://www.jetnetevomobile.com" target="_blank">http://www.jetnetevomobile.com</a></strong> address
                              on your mobile device.
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>JETNET Global Aircraft Listing</h1>
                                        </td>
                                        <td align="left" valign="top" width="93%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <!-- -->
                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="left" valign="top" rowspan="2" width="60">
                                                        <img src="images/airplane_light_blue.png" alt="Global Listing" width="60" />
                                                    </td>
                                                    <td align="left" valign="middle">
                                                        <p>
                                                            <asp:Label ID="global_info_lbl" runat="server"></asp:Label>
                                                        </p>
                                                        <asp:CheckBox ID="myservices_enable_global_list_ck" runat="server" />
                                                        &nbsp;&nbsp;<asp:Image ID="myservices_global_img" Height="15px" runat="server" ImageUrl="/images/info.png" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%If bIsSiteChatEnabled Then%>
                        <tr>
                            <td align="left" valign="top">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>Chat</h1>
                                        </td>
                                        <td align="left" valign="top" width="93%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top" rowspan="2" width="60">
                                                        <img src="images/person.jpg" alt="Chat" width="60" />
                                                    </td>
                                                    <td align="left" valign="middle">
                                                        <p>
                                                            <asp:Label ID="chat_info_lbl" runat="server" Width="80%"></asp:Label>
                                                        </p>
                                                        <asp:CheckBox ID="myservices_enable_chat_ck" runat="server" />
                                                        &nbsp;&nbsp;<asp:Image ID="myservices_chat_img" Height="15px" runat="server" ImageUrl="/images/info.png" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%End If%>
                        <%If bCanSaveDefaultEmail Then%>
                        <tr>
                            <td align="left" valign="top">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>Email Requests</h1>
                                        </td>
                                        <td align="left" valign="top" width="86%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="3" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="middle" rowspan="2" width="60">
                                                        <img src="images/email.jpg" alt="Email" width="60" />
                                                    </td>
                                                    <td align="left" valign="middle" width="90">Email Name:
                                                    </td>
                                                    <td align="left" valign="middle">
                                                        <asp:TextBox ID="myservices_email_name_txt" runat="server" Width="150px"></asp:TextBox>
                                                    </td>
                                                    <td align="left" valign="middle" rowspan="2 " style="text-align: center; padding-top: 10px;">E-Mail Format:<br />
                                                        <asp:RadioButton ID="myservices_email_format_html" runat="server" Text="HTML" /><asp:RadioButton
                                                            ID="myservices_email_format_text" runat="server" Text="TEXT" />
                                                    </td>
                                                    <td align="right" valign="middle" rowspan="2" width="20%" style="text-align: right; padding-right: 8px; padding-top: 10px;">To learn more about our Email Request Service click <strong><a class="underline pointer"
                                                        onclick="javascript:openSmallWindowJS('/help/documents/636.pdf','HelpWindow');">Here</a></strong>.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="middle">Email Address:
                                                    </td>
                                                    <td align="left" valign="middle">
                                                        <asp:TextBox ID="myservice_email_address_txt" runat="server" Width="150px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%End If%>
                        <%If bCanHaveSMS And Not CBool(Session.Item("localPreferences").AerodexFlag.ToString) Then%>
                        <tr>
                            <td align="left" valign="top">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>SMS Text Messaging</h1>
                                        </td>
                                        <td align="left" valign="top" width="81%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table width="100%" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td align="left" valign="top" rowspan="2" width="60">
                                            <img src="images/text.jpg" alt="SMS Messaging" width="60" />
                                        </td>
                                        <td align="left" valign="top">
                                            <p>
                                                Enter your Cell Phone Number, Models and Provider to setup this service.
                                            </p>
                                            <p align="center">
                                                <em>SMS Text Messaging Service is
                          <asp:Label runat="server" ID="myservices_SMS_service_status" ForeColor="Red"></asp:Label>
                                                    for this subscription.</em>
                                            </p>
                                            <asp:CheckBox ID="myservices_enable_SMS_ck" runat="server" Text="Enable SMS Service " />
                                            &nbsp;&nbsp;<asp:Image ID="myservices_SMS_img" Height="15px" runat="server" ImageUrl="/images/info.png" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="3" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top">Cell Phone Number:
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:TextBox ID="myservices_sms_phone_number" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:Image
                                                            ID="myservices_sms_phone_img" Height="15px" runat="server" ImageUrl="/images/info.png" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top">SMS Service Providers:
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:DropDownList ID="myservices_SMS_providers" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" width="60">
                                            <img src="images/to_monitor.jpg" alt="To Monitor" width="60" />
                                        </td>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="3" cellspacing="0" class="my_services_row_top">
                                                <tr>
                                                    <td align="left" valign="top" width="80%">Event(s):
                                                    </td>
                                                    <td align="right" valign="top">
                                                        <asp:ListBox ID="myservices_events_to_monitor" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" width="80%">Model(s):
                                                    </td>
                                                    <td align="right" valign="top">
                                                        <asp:ListBox ID="myservices_models_to_monitor" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="middle" width="60"></td>
                                        <td align="left" valign="top" colspan="2">
                                            <p>
                                                Terms of Service: Subscription SMS alert service. Text
                        messaging and data rates may apply depending on your wireless service plan. United
                        States clients can also text the word HELP for assistance or STOP to
                        <% If bEnableTelTags Then%>cancel to <a href='tel:12069228444'>12069228444</a> (<a
                            href='tel:12065189785'>12065189785</a>
                                                <% Else%>
                        cancel to 12069228444 (12065189785
                        <% End If%>
                        for International).
                                            </p>
                                            <p>
                                                By entering your cell phone number you certify that
                                            </p>
                                            <ul class="circle">
                                                <li>you are the account holder or</li>
                                                <li>have account holder's permission to do so.</li>
                                            </ul>
                                            <p class="important_text">
                                                <b>Content Provided:</b> by JETNET LLC. Long Code is managed by JETNET LLC. For
                        support assistance contact <a href="mailto:customerservice@jetnet.com">customerservice@jetnet.com</a>
                                                <% If bEnableTelTags Then%>
                        or call <a href='tel:800-553-8638'>800-553-8638</a> (<a href='tel:315-797-4420'>315-797-4420</a>)
                        <% Else%>
                        or call 800-553-8638 (315-797-4420)
                        <% End If%>
                        or go to <a href="http://www.jetnet.com/contact.shtml" title="Contact Us" target="_new">Contact Us</a> See SMS Service Providers pull down for list of supported carriers.
                                            </p>
                                            <asp:CheckBox ID="myservices_terms_and_conditions_ck" runat="server" Text="I accept the Terms and Conditions" /><p>
                                                <i><a href="help/smstextmessageservicetermsofservice.html" title="Click To View Terms and Conditions"
                                                    target="_new">Terms and Conditions</a><br />
                                                    <a href="help/smstextmessageserviceprivacypolicy.html" title="Click To View Privacy Policy"
                                                        target="_new">Privacy Policy</a></i>
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <% End If%>
                    </table>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="my_notes" runat="server" HeaderText="Notes">
                <HeaderTemplate>
                    Notes
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel runat="server" ID="cloud_notes">
                        <table width="100%" cellpadding="3" cellspacing="0">
                            <tr>
                                <td align="left" valign="top" colspan="2">
                                    <table width="100%" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <h1>Cloud Notes</h1>
                                            </td>
                                            <td align="left" valign="top" width="85%">
                                                <div class="seperator_line">
                                                    &nbsp;
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" colspan="2">
                                                <table width="100%" cellpadding="3" cellspacing="0">
                                                    <tr>
                                                        <td align="left" valign="top" rowspan="2" width="60">
                                                            <img src="images/cloud_watermark.jpg" alt="Notes" width="60" />
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <table width="100%" cellpadding="3" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" valign="top" width="230">
                                                                        <asp:RadioButtonList runat="server" ID="notes_notes_options" CssClass="float_left">
                                                                            <asp:ListItem Value="" Selected="True">No Notes Active for this Subscription</asp:ListItem>
                                                                            <asp:ListItem Value="Standard">Standard Cloud Notes</asp:ListItem>
                                                                            <asp:ListItem Value="Plus">Cloud Notes Plus</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <a href="#" onclick="javascript:load('help.aspx?id=349','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');">
                                                                            <img src="images/info.png" alt="More Information on these Options" class="float_left"
                                                                                width="15" border="0" /></a>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <br />
                                                            <asp:Label runat="server" ID="notes_admin_text" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:CheckBox ID="myservernotes_ac_notes_enabled_ck" runat="server" Text="Server Aircraft Notes Enabled"
                                                    Enabled="False" Visible="False" /><br />
                                                <asp:CheckBox ID="myservernotes_ac_notes_listing_ck" runat="server" Text="Enable Notes Indicator on Listings"
                                                    Enabled="False" />
                                                &nbsp;&nbsp;
                        <asp:Image ID="myservernotes_ac_notes_listing_img" Height="15px" runat="server" ImageUrl="/images/info.png" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="my_support" runat="server" HeaderText="Support">
                <HeaderTemplate>
                    Support
                </HeaderTemplate>
                <ContentTemplate>
                    <table width="100%" cellpadding="3" cellspacing="0" bgcolor="white">
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table width="100%" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <h1>Additional Support Information</h1>
                                        </td>
                                        <td align="left" valign="top" width="71%">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table width="100%" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="actinfo_subscriber_information_id">SUBSCRIBER CONTACT: ID[ <%=Session.Item("localUser").crmUserContactID.ToString.Trim%> ]</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" width="60">
                                            <img src="images/support.jpg" alt="Support" width="60" />
                                        </td>
                                        <td colspan="3">If you have a specific question or issue to report to JETNET please enter a description
                      of your issue in the box below and click on the 'Submit Customer Support Issue'
                      button. Staff from our customer support center will then research your issue and
                      respond as quickly as possible.<br />
                                            <br />
                                            <asp:TextBox ID="support_email_textbox" CssClass="float_right" runat="server" TextMode="MultiLine" Width="98%"
                                                Rows="10"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top"></td>
                                        <td valign="top" colspan="3"><span id="spnCharLeft" class="float_left charactersLeft">455 characters left.</span>
                                            <p align="right">
                                                <asp:Label runat="server" ID="alert_text" Visible="false" ForeColor="Red"></asp:Label>
                                                <asp:Button ID="support_email_button" runat="server" Text="Submit Customer Support Issue" />
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="subheading" colspan="4">CONTACT JETNET</td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" width="60">
                                            <img src="images/support.jpg" alt="Support" width="60" />
                                        </td>
                                        <td colspan="3">
                                            <table cellpadding="3" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left" valign="top">101 First Street, 2<sup>nd</sup> Floor
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <% If bEnableTelTags Then%>
                            Phone: <a href='tel:315-797-4420'>(315)-797-4420</a>
                                                        <% Else%>
                            Phone: (315)-797-4420
                            <% End If%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top">Utica,&nbsp;NY&nbsp;13501-1222
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <% If bEnableTelTags Then%>
                            Toll Free: <a href='tel:800-553-8638'>(800)-553-8638</a>
                                                        <% Else%>
                            Toll Free: (800)-553-8638
                            <% End If%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top">United States
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <% If bEnableTelTags Then%>
                            Fax: <a href='tel:315-797-4798'>(315)-797-4798</a>
                                                        <% Else%>
                            Fax: (315)-797-4798
                            <% End If%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left" valign="top">
                                                        <a href="mailto:customerservice@jetnet.com">customerservice@jetnet.com</a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left" valign="top">
                                                        <a href='http://www.jetnet.com' target='_new'>www.jetnet.com</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="subheading" colspan="4">CONTACT YOUR JETNET REPRESENTATIVE</td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" width="60">
                                            <img src="images/support.jpg" alt="Support" width="60" />
                                        </td>
                                        <td colspan="3">
                                            <table width="100%" cellpadding="3" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <asp:Label runat="server" ID="support_rep_name"></asp:Label><br />
                                                        <br />
                                                        <asp:Label runat="server" ID="support_rep_number"></asp:Label><br />
                                                        <asp:Label runat="server" ID="support_rep_email"></asp:Label>
                                                    </td>
                                                    <td align="right" valign="top">
                                                        <asp:Image ID="support_rep_image" runat="server" Width='150px' />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="my_users" runat="server" HeaderText="Users" Visible="false">
                <HeaderTemplate>
                    Users
                </HeaderTemplate>
                <ContentTemplate>
                    <table width="100%" cellspacing="0" cellpadding="3">
                        <tr>
                            <td align="left" valign="top">
                                <h1>Users&nbsp;Summary</h1>
                            </td>
                            <td align="left" valign="top" width="84%">
                                <div class="seperator_line">
                                    &nbsp;
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table width="100%" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td align="left" valign="top" width="60">
                                            <img src="images/tools.jpg" alt="User List" width="60" />
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="my_users_email_response" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <asp:Label ID="my_users_list" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <h1>User&nbsp;Counts</h1>
                            </td>
                            <td align="left" valign="top">
                                <div class="seperator_line">
                                    &nbsp;
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table width="100%" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td align="left" valign="top" rowspan="2" width="60">
                                            <img src="images/tools.jpg" alt="User Count" width="60" />
                                        </td>
                                        <td align="left" valign="bottom">
                                            <table width="100%" cellpadding="3" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <b>Total User Licenses:</b>
                                                        <asp:Label runat="server" ID="myusers_total_user_license"></asp:Label>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <b>Total Users Assigned:</b>
                                                        <asp:Label runat="server" ID="myusers_total_user_assigned"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="my_folders" runat="server" HeaderText="Folders" Visible="true">
                <HeaderTemplate>
                    Folders
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Table ID="Table1" runat="server" Width="100%" CellPadding="3" CellSpacing="0">
                        <asp:TableRow>
                            <asp:TableCell ID="TableCell2" HorizontalAlign="left" VerticalAlign="top" runat="server">
                                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true">
                                    <asp:ListItem>All</asp:ListItem>
                                </asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell1" HorizontalAlign="left" VerticalAlign="top" runat="server"
                                Width="90%">
                                <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="true">
                                    <asp:ListItem Value="mf" Text="My Folders"></asp:ListItem>
                                    <asp:ListItem Value="usf" Text="User Folders for this Subscription"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;&nbsp;<asp:LinkButton ID="edit_folder_button" runat="server" PostBackUrl="~/Preferences.aspx?task=editFolder"
                                    Visible="false"><strong>Edit Folder</strong></asp:LinkButton>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">
                                <div runat="server" id="div_folder_results_table">
                                    <div style="text-align: left; width: 100%;" runat="server" id="folderResults">
                                        <asp:Label runat="server" ID="folderTable"></asp:Label>
                                    </div>
                                </div>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="my_templates" runat="server" HeaderText="Templates" Visible="true">
                <HeaderTemplate>
                    Templates
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Table ID="Table2" runat="server" Width="100%" CellPadding="3" CellSpacing="0">
                        <asp:TableRow>
                            <asp:TableCell ID="TableCell3" HorizontalAlign="left" VerticalAlign="top" runat="server">
                                <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="true">
                                    <asp:ListItem>All</asp:ListItem>
                                </asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell4" HorizontalAlign="left" VerticalAlign="top" runat="server"
                                Width="90%">
                                <asp:DropDownList ID="DropDownList4" runat="server" AutoPostBack="true">
                                    <asp:ListItem Value="mt" Text="My Templates"></asp:ListItem>
                                    <asp:ListItem Value="ust" Text="User Templates for this Subscription">User Templates for this Subscription</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;&nbsp;<asp:LinkButton ID="edit_template_button" runat="server" PostBackUrl="~/Preferences.aspx?task=editTemplate"
                                    Visible="false"><strong>Edit Template</strong></asp:LinkButton>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">
                                <div runat="server" id="div_template_results_table">
                                    <div style="text-align: left; width: 100%;" runat="server" id="templateResults">
                                        <asp:Label runat="server" ID="templateTable"></asp:Label>
                                    </div>
                                </div>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="my_company" runat="server">
                <HeaderTemplate>
                    Company Preferences
                </HeaderTemplate>
                <ContentTemplate>
                    <table width="100%" cellpadding="3" cellspacing="0">
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <p class="nonflyout_info_box">
                                    The following preferences will be applied to all users of this system and should
                  only be modified by a system administrator.
                                </p>
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top" width="165">
                                            <h1>Company Categories</h1>
                                        </td>
                                        <td align="left" valign="top">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top" width="60">
                                                        <img src="images/category_watermark.jpg" alt="Categories" />
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:Label ID="preference_attention" runat="server" class="attention"></asp:Label>
                                                        <asp:Panel ID="preference_toggle" runat="server">
                                                            <p style="text-align: left; padding-right: 8px; padding-top: 10px;">
                                                                Enter a Name/Label for each custom Company Catagory that you desire and check the
                                box to the right of the name if you wish to have it applied in the system.<br />
                                                                To Clear Catagory "Clear Name and Clear Checkbox"<br />
                                                                <span style="color: Maroon;">Note: Click "SAVE" to save catagory values.</span>
                                                            </p>
                                                            <table width="450" cellpadding="4" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" valign="top">&nbsp;
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <strong>Category Name</strong>
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <strong>Use?</strong>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Category #1:
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="pref_1" runat="server" Width="310" MaxLength="60" Enabled="true" />
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:CheckBox ID="pref_1_use" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Category #2:
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="pref_2" runat="server" Width="310" MaxLength="60" Enabled="true" />
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:CheckBox ID="pref_2_use" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Category #3:
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="pref_3" runat="server" Width="310" MaxLength="60" Enabled="true" />
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:CheckBox ID="pref_3_use" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Category #4:
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="pref_4" runat="server" Width="310" MaxLength="60" Enabled="true" />
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:CheckBox ID="pref_4_use" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Category #5:
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="pref_5" runat="server" Width="310" MaxLength="60" Enabled="true" />
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:CheckBox ID="pref_5_use" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" width="165">
                                            <h1>Aircraft Custom Fields</h1>
                                        </td>
                                        <td align="left" valign="top">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top" width="60">
                                                        <img src="images/category_watermark.jpg" alt="Custom Fields" />
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:Label ID="Label1" runat="server" class="attention"></asp:Label>
                                                        <asp:Panel ID="aircraft_preference_toggle" runat="server">
                                                            <p style="text-align: left; padding-right: 8px; padding-top: 10px;">
                                                                Enter a Name/Label for each custom aircraft data field that you desire and check
                                the box to the right of the name if you wish to have it applied in the system.<br />
                                                                At any point where a given field is no longer used simply uncheck the box to the
                                right of the name.
                                <br />
                                                                <span style="color: Maroon;">Note: Do not reuse fields for a different purpose in
                                  the future since data stored in each given field would still have previous values.</span>
                                                            </p>
                                                            <table width="650" cellpadding="4" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" valign="top">&nbsp;
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <strong>Custom Field Name</strong>
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <strong>Use?</strong>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top" width="150">Custom Field #1:
                                                                    </td>
                                                                    <td align="left" valign="top" width="160">
                                                                        <asp:TextBox ID="ac_category_1" runat="server" Width="250" MaxLength="60" Enabled="false" />
                                                                    </td>
                                                                    <td align="left" valign="top" nowrap="nowrap" width="320">
                                                                        <asp:CheckBox ID="ac_category_1_use" runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="edit_ac_1" AlternateText="Edit" ImageUrl="~/images/edit_icon.png"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="deleteq_ac_1" AlternateText="Delete" ImageUrl="~/images/red_x.gif"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="updateq_ac_1" AlternateText="Update" ImageUrl="~/images/update.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="cancel_ac_1" AlternateText="Cancel" ImageUrl="~/images/cancel.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:Label runat="server" ID="deleteq_label1" Text="Delete?" Visible="false"></asp:Label>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="yes_delete1" runat="server" Text="Yes" Visible="false"></asp:LinkButton>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="no_delete1" runat="server" Text="No" Visible="false"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Custom Field #2:
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="ac_category_2" runat="server" Width="250" MaxLength="60" Enabled="false" />
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:CheckBox ID="ac_category_2_use" runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="edit_ac_2" AlternateText="Edit" ImageUrl="~/images/edit_icon.png"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="deleteq_ac_2" AlternateText="Delete" ImageUrl="~/images/red_x.gif"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="updateq_ac_2" AlternateText="Update" ImageUrl="~/images/update.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="cancel_ac_2" AlternateText="Cancel" ImageUrl="~/images/cancel.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:Label runat="server" ID="deleteq_label2" Text="Delete?" Visible="false"></asp:Label>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="yes_delete2" runat="server" Text="Yes" Visible="false"></asp:LinkButton>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="no_delete2" runat="server" Text="No" Visible="false"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Custom Field #3:
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="ac_category_3" runat="server" Width="250" MaxLength="60" Enabled="false" />
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:CheckBox ID="ac_category_3_use" runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="edit_ac_3" AlternateText="Edit" ImageUrl="~/images/edit_icon.png"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="deleteq_ac_3" AlternateText="Delete" ImageUrl="~/images/red_x.gif"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="updateq_ac_3" AlternateText="Update" ImageUrl="~/images/update.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="cancel_ac_3" AlternateText="Cancel" ImageUrl="~/images/cancel.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:Label runat="server" ID="deleteq_label3" Text="Delete?" Visible="false"></asp:Label>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="yes_delete3" runat="server" Text="Yes" Visible="false"></asp:LinkButton>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="no_delete3" runat="server" Text="No" Visible="false"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Custom Field #4:
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="ac_category_4" runat="server" Width="250" MaxLength="60" Enabled="false" />
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:CheckBox ID="ac_category_4_use" runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="edit_ac_4" AlternateText="Edit" ImageUrl="~/images/edit_icon.png"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="deleteq_ac_4" AlternateText="Delete" ImageUrl="~/images/red_x.gif"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="updateq_ac_4" AlternateText="Update" ImageUrl="~/images/update.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="cancel_ac_4" AlternateText="Cancel" ImageUrl="~/images/cancel.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:Label runat="server" ID="deleteq_label4" Text="Delete?" Visible="false"></asp:Label>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="yes_delete4" runat="server" Text="Yes" Visible="false"></asp:LinkButton>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="no_delete4" runat="server" Text="No" Visible="false"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Custom Field #5:
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="ac_category_5" runat="server" Width="250" MaxLength="60" Enabled="false" />
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:CheckBox ID="ac_category_5_use" runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="edit_ac_5" AlternateText="Edit" ImageUrl="~/images/edit_icon.png"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="deleteq_ac_5" AlternateText="Delete" ImageUrl="~/images/red_x.gif"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="updateq_ac_5" AlternateText="Update" ImageUrl="~/images/update.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="cancel_ac_5" AlternateText="Cancel" ImageUrl="~/images/cancel.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:Label runat="server" ID="deleteq_label5" Text="Delete?" Visible="false"></asp:Label>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="yes_delete5" runat="server" Text="Yes" Visible="false"></asp:LinkButton>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="no_delete5" runat="server" Text="No" Visible="false"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Custom Field #6:
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="ac_category_6" runat="server" Width="250" MaxLength="60" Enabled="false" />
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:CheckBox ID="ac_category_6_use" runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="edit_ac_6" AlternateText="Edit" ImageUrl="~/images/edit_icon.png"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="deleteq_ac_6" AlternateText="Delete" ImageUrl="~/images/red_x.gif"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="updateq_ac_6" AlternateText="Update" ImageUrl="~/images/update.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="cancel_ac_6" AlternateText="Cancel" ImageUrl="~/images/cancel.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:Label runat="server" ID="deleteq_label6" Text="Delete?" Visible="false"></asp:Label>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="yes_delete6" runat="server" Text="Yes" Visible="false"></asp:LinkButton>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="no_delete6" runat="server" Text="No" Visible="false"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Custom Field #7:
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="ac_category_7" runat="server" Width="250" MaxLength="60" Enabled="false" />
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:CheckBox ID="ac_category_7_use" runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="edit_ac_7" AlternateText="Edit" ImageUrl="~/images/edit_icon.png"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="deleteq_ac_7" AlternateText="Delete" ImageUrl="~/images/red_x.gif"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="updateq_ac_7" AlternateText="Update" ImageUrl="~/images/update.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="cancel_ac_7" AlternateText="Cancel" ImageUrl="~/images/cancel.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:Label runat="server" ID="deleteq_label7" Text="Delete?" Visible="false"></asp:Label>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="yes_delete7" runat="server" Text="Yes" Visible="false"></asp:LinkButton>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="no_delete7" runat="server" Text="No" Visible="false"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Custom Field #8:
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="ac_category_8" runat="server" Width="250" MaxLength="60" Enabled="false" />
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:CheckBox ID="ac_category_8_use" runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="edit_ac_8" AlternateText="Edit" ImageUrl="~/images/edit_icon.png"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="deleteq_ac_8" AlternateText="Delete" ImageUrl="~/images/red_x.gif"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="updateq_ac_8" AlternateText="Update" ImageUrl="~/images/update.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="cancel_ac_8" AlternateText="Cancel" ImageUrl="~/images/cancel.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:Label runat="server" ID="deleteq_label8" Text="Delete?" Visible="false"></asp:Label>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="yes_delete8" runat="server" Text="Yes" Visible="false"></asp:LinkButton>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="no_delete8" runat="server" Text="No" Visible="false"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Custom Field #9:
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="ac_category_9" runat="server" Width="250" MaxLength="60" Enabled="false" />
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:CheckBox ID="ac_category_9_use" runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="edit_ac_9" AlternateText="Edit" ImageUrl="~/images/edit_icon.png"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="deleteq_ac_9" AlternateText="Delete" ImageUrl="~/images/red_x.gif"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="updateq_ac_9" AlternateText="Update" ImageUrl="~/images/update.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="cancel_ac_9" AlternateText="Cancel" ImageUrl="~/images/cancel.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:Label runat="server" ID="deleteq_label9" Text="Delete?" Visible="false"></asp:Label>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="yes_delete9" runat="server" Text="Yes" Visible="false"></asp:LinkButton>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="no_delete9" runat="server" Text="No" Visible="false"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Custom Field #10:
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="ac_category_10" runat="server" Width="250" MaxLength="60" Enabled="false" />
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:CheckBox ID="ac_category_10_use" runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="edit_ac_10" AlternateText="Edit" ImageUrl="~/images/edit_icon.png"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="deleteq_ac_10" AlternateText="Delete" ImageUrl="~/images/red_x.gif"
                                        runat="server" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="updateq_ac_10" AlternateText="Update" ImageUrl="~/images/update.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:ImageButton ID="cancel_ac_10" AlternateText="Cancel" ImageUrl="~/images/cancel.gif"
                                        runat="server" Visible="false" />
                                                                        &nbsp;&nbsp;
                                    <asp:Label runat="server" ID="deleteq_label10" Text="Delete?" Visible="false"></asp:Label>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="yes_delete10" runat="server" Text="Yes" Visible="false"></asp:LinkButton>
                                                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="no_delete10" runat="server" Text="No" Visible="false"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" width="165">
                                            <h1>Company Settings</h1>
                                        </td>
                                        <td align="left" valign="top">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top" width="60">
                                                        <img src="images/number.jpg" alt="Categories" />
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:Panel ID="maximum_export" runat="server">
                                                            <asp:CompareValidator ID="maximum_compare" runat="server" ControlToValidate="maximum_records_export"
                                                                Operator="DataTypeCheck" Type="Double" ErrorMessage="* Maximum Records Must be Numeric"></asp:CompareValidator>
                                                            <table width="450" cellpadding="4" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" valign="top" colspan="2">Maximum # of Client Records in Single Export:&nbsp;
                                    <asp:TextBox ID="maximum_records_export" runat="server" Width="50" MaxLength="10"
                                        Text="0" />
                                                                        <p class="nonflyout_info_box">
                                                                            "0" indicates unlimited export of client records
                                                                        </p>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="my_featurecodes" runat="server">
                <HeaderTemplate>
                    Features
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Label ID="main_attention" runat="server" class="attention"></asp:Label>
                    <table width="100%" cellpadding="3" cellspacing="0">
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top" width="220">
                                            <h1>FEATURE CODE MAINTENANCE</h1>
                                        </td>
                                        <td align="left" valign="top">
                                            <div class="seperator_line">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="2">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top" width="60" rowspan="3">
                                                        <img src="images/autologin.jpg" alt="Default Regions" />
                                                    </td>
                                                    <td align="left" valign="top" colspan="2">Add Feature Codes for Use within your Program.
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="4" align="left">
                                        <p align="right" style="padding-right: 3px">
                                            <asp:LinkButton ID="add_new" CommandName="Add" Text="Add New Feature" runat="server" />
                                        </p>
                                        <asp:Panel runat="server" CssClass="gray" Visible="false" ID="new_row">
                                            <table width="100%" cellpadding="3" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="top" width="320">
                                                        <b>Feature Name</b>
                                                    </td>
                                                    <td align="left" valign="top" width="90">
                                                        <b>Feature Code</b>
                                                    </td>
                                                    <td align="left" valign="top"></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <asp:TextBox ID="clickfeat_name" Width="320px" runat="server" MaxLength="60" />
                                                    </td>
                                                    <td align="left" valign="top" width="90">
                                                        <asp:TextBox ID="clikfeat_type" Width="90px" runat="server" MaxLength="3" />
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:LinkButton ID="insert" CommandName="insert" Text="Insert" runat="server" />
                                                        <asp:LinkButton ID="cancel" CommandName="cancel" Text="Cancel" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:DataGrid runat="server" ID="datagrid_feature_code" CellPadding="3" horizontal-align="left"
                                            EnableViewState="true" ShowFooter="false" BackColor="White" Font-Size="8pt" Width="100%"
                                            OnCancelCommand="MyDataGrid_Cancel" OnEditCommand="MyDataGrid_Edit" OnDeleteCommand="MyDataGrid_Delete"
                                            AllowPaging="false" PageSize="25" Visible="true" BorderStyle="None" AllowSorting="True"
                                            AutoGenerateColumns="false" BorderColor="Gray">
                                            <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
                                                Font-Underline="True" ForeColor="White" Mode="NumericPages" NextPageText="Next"
                                                PrevPageText="Previous" />
                                            <AlternatingItemStyle BackColor="#eeeeee" />
                                            <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="Gray" Font-Size="8pt" />
                                            <HeaderStyle BackColor="#A8C1DD" Font-Bold="True" Font-Size="10pt" ForeColor="Black"
                                                Wrap="False" HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Feature Name" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "clikfeat_name")), (DataBinder.Eval(Container.DataItem, "clikfeat_name")), "")%>
                                                        <asp:TextBox runat="server" ID="id_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "clikfeat_name") %>'
                                                            Visible="true" Width="430px" MaxLength="60" Style="display: none;" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox runat="server" ID="id" Text='<%# DataBinder.Eval(Container.DataItem, "clikfeat_name") %>'
                                                            Visible="true" Width="330px" MaxLength="60" />
                                                        <asp:TextBox runat="server" ID="id_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "clikfeat_name") %>'
                                                            Visible="true" Width="330px" MaxLength="60" Style="display: none;" />
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Feature Code" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "clikfeat_type")), (DataBinder.Eval(Container.DataItem, "clikfeat_type")), "")%>
                                                        <asp:TextBox runat="server" ID="type_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "clikfeat_type") %>'
                                                            Visible="true" Width="40px" MaxLength="3" Style="display: none;" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox runat="server" ID="type" Text='<%# DataBinder.Eval(Container.DataItem, "clikfeat_type") %>'
                                                            Visible="true" Width="40px" MaxLength="3" />
                                                        <asp:TextBox runat="server" ID="type_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "clikfeat_type") %>'
                                                            Visible="true" Width="40px" MaxLength="3" Style="display: none;" />
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="feature_code_delete" CommandName="Delete" Text="Delete" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="my_modules" HeaderText="HOMEPAGE" Visible="false" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="contentBox">
                            <p>This tab is used to identify the list of tiles to appear on your homepage.  The tiles available to display will be shown on the left while the tiles currently displayed on your homepage will be shown on the right.</p>
                            <ul>
                                <li><strong>Adding Tiles to Homepage</strong>To add tiles on your homepage, drag items from the left to the right-hand side and click save icon.</li>
                                <li><strong>Removing Tiles from Homepage</strong>To remove tiles on your homepage, drag items from the right to the left-hand side and click save icon.</li>
                                <li><strong>Ordering Tiles on Homepage</strong>To re-order the tiles on your homepage, simply drag the tiles in the right-hand list of blocks to the position desired and click save icon.</li>
                            </ul>
                        </div>
                    </div>
                    <div class="row">
                        <asp:Literal runat="server" ID="sort1"></asp:Literal>
                        <asp:Literal runat="server" ID="sort2"></asp:Literal>
                    </div>
                </ContentTemplate>
            </cc1:TabPanel>
        </cc1:TabContainer>
        <div class="NotesHeader">
        </div>
        <asp:Table ID="buttonsTable1" CellPadding="3" CellSpacing="0" Width="100%" CssClass="buttonsTable"
            runat="server">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Style="padding-right: 4px;">
                    <input type="button" id="sortableSave" value="Save" class="display_none" />
                    <asp:Button ID="save_button2" runat="server" Text="Save" OnClientClick="javascript:ShowPreferencesMessage('DivPreferencesMessage','Saving Preferences','Saving Preferences ... Please Wait ...');return true;"
                        CssClass="gray_button float_right" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    <div id="DivChatSubscriptionError" style="display: none;">
        <table width="100%" cellpadding="3" cellspacing="0">
            <tr>
                <td colspan="2" style="text-align: center; height: 42px;">
                    <div id="textChatSubscriptionError">
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; vertical-align: bottom;">
                    <asp:Button ID="btnChangeChatSub" runat="server" OnClientClick='bChangeSubFlag = true;$("#DivChatSubscriptionError").dialog("close");return false;'
                        Text="Ok" />
                </td>
                <td style="text-align: center; vertical-align: bottom;">
                    <asp:Button ID="btnCancelChangeChatSub" runat="server" OnClientClick='bChangeSubFlag = false;$("#DivChatSubscriptionError").dialog("close");return false;'
                        Text="Cancel" />
                </td>
            </tr>
        </table>
    </div>
    <div id="DivPreferencesMessage" style="display: none;">
    </div>

    <script type="text/javascript">

        $('#<%= disableTellAbout.ClientID %>').click(function () {
            if (this.checked == false) {
                createCookie('tellAboutChanges', 'false', 365);
            } else {
                createCookie('tellAboutChanges', 'true', 365);
            };
        });
        $('#<%= support_email_textbox.ClientID %>').keyup(function () {
            var maxLimit = 455;
            var lengthCount = this.value.length;
            if (lengthCount > maxLimit) {
                this.value = this.value.substring(0, maxLimit);
                var charactersLeft = 0;//We don't want to ever have this display less than 0, so we're just going to make it 0 if you have to substring the excess away;                   
            }
            else {
                var charactersLeft = maxLimit - lengthCount;
            }
            $('#spnCharLeft').text(charactersLeft + ' Characters left');
        });

        function validatePassword() {
            var txttext = document.getElementById("<%= newPasswordID.ClientID.ToString %>").value;
            var regex = /^(?=(.*[A-Z]){1,})(?=(.*[a-z]){1,})(?=(.*[\d]){1,})(?=(.*[\W]){1,})(?!.*\s).{8,15}$/;

            if (eval(regex.test(txttext)) == false && txttext != '') {
                alert('Your new password should be a minimum of 8 characters in length and must contain at least "one number, one LOWER case and one UPPER case, and one SPECIAL character ( !@#$%^&*()_+=- )" ...');
                document.getElementById("<%=  newPasswordID.ClientID.ToString%>").style.color = "red";
            }
      //document.getElementById("<%=  newPasswordID.ClientID.ToString%>").style.color = "white";
        }

        $(document).ready(function () {
            try {
                ShowChangeDefailtAirportButton(<%= HttpContext.Current.Session.Item("currentDefaultAirportFolderID").ToString %>);
            }
            catch (err) {

            }
        });

        function refreshPreferences() {
      <%= PostBackStrDirty.ToString %>;
        }

        function ActiveTabChanged(sender, args) {

            //   alert("currentTab " + currentTab + "\n tabSelected " + sender.get_activeTab().get_id());

            var nextTab = sender.get_activeTab().get_id();
            if (nextTab == "<%= my_modules.ClientID %>") {

                $("#sortableSave").removeClass("display_none");
                $("#<%=save_button2.ClientID%>").addClass("display_none");
                $("#<%=save_button1.ClientID%>").attr("style", "display: none !important;");
                $("#sortSaveTop").removeAttr("style");

            } else {
                $("#sortableSave").addClass("display_none");
                $("#<%=save_button2.ClientID%>").removeClass("display_none");
                $("#<%=save_button1.ClientID%>").removeAttr("style");
                $("#sortSaveTop").attr("style", "display: none !important;");
            }
            if (currentTab != "my_account" && currentTab != "my_support" && currentTab != "my_users" && currentTab != "my_folders" && currentTab != "my_templates" && currentTab != "my_company" && currentTab != "my_featurecodes" && !bClickedSave) {

                var r = confirm("Do you want to save your changes?");

                if (r == true) {
          <%= PostBackStrSave.ToString %>;
                } else {
                    if (nextTab.indexOf("my_display") > 0 || nextTab.indexOf("my_models") > 0 || nextTab.indexOf("my_airports") > 0 || nextTab.indexOf("my_users") > 0 || nextTab.indexOf("my_folders") > 0 || nextTab.indexOf("my_templates") > 0 || nextTab.indexOf("my_company") > 0 || nextTab.indexOf("my_featurecodes") > 0) {
            <%= PostBackStrDirty.ToString %>;
                    }
                }

            } else {

                if (nextTab.indexOf("my_display") > 0 || nextTab.indexOf("my_models") > 0 || nextTab.indexOf("my_airports") > 0 || nextTab.indexOf("my_users") > 0 || nextTab.indexOf("my_folders") > 0 || nextTab.indexOf("my_templates") > 0 || nextTab.indexOf("my_company") > 0 || nextTab.indexOf("my_featurecodes") > 0) {
          <%= PostBackStrDirty.ToString %>;
                }
            }

            //RedrawDatatablesOnSys();

        }


        function showChatChangeDialog() {

            $("#textChatSubscriptionError").html("<p align=\"left\">We have detected that you are currently have CHAT enabled on another subscription.</p><p align=\"left\">Would you like to \"Enable\" CHAT on this subscription? (Cancel to leave your CHAT Subscription alone)</p>");
            $("#DivChatSubscriptionError").dialog({ modal: false, show: 'slide', title: 'JETNET Chat Subscription Change', width: 425, height: 190, resizable: false, closeOnEscape: true, close: function (event) { } });

            $("#DivChatSubscriptionError").unbind();
            $("#DivChatSubscriptionError").on('dialogclose', function (event) {

                if (bChangeSubFlag) {
                    crmwebclient.chatservices.ChangeChatSession(sessGUID, txtAlias, bEnable, bChangeSub, fnChangeUserSessionOnSuccessCallBack);
                } else {
                    document.getElementById("<%= myservices_enable_chat_ck.ClientID.ToString %>").checked = false;
                }

            });

        }

        function fnChangeUserSessionOnSuccessCallBack(args) {
            alert("Chat subscription changed to use CURRENT subscription");
        }

        $(document).ready(function () {


            if (bSiteChatEnabled) {

                if (bShowDialog) {
                    //alert("show chat dialog");
                    showChatChangeDialog();
                }

                var popUpsBlocked = false;
                popUpsBlocked = arePopupWindowsBlocked();

                if (popUpsBlocked) {
                    document.getElementById("<%= myservices_enable_chat_ck.ClientID.ToString %>").disabled = true;
                }
            }

            ClosePreferencesMessage("DivPreferencesMessage");

        });

        function ShowPreferencesMessage(DivTag, Title, Message) {
            $("#" + DivTag).html(Message);
            $("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 75, resizable: false });
        }

        function ClosePreferencesMessage(DivTag) {
            $("#" + DivTag).dialog("close");
        }

        function alertEmailSent() {
            alert("Customer Support Your Email has been sent ... You should hear back from a Customer Support Representitive, within 24 hrs of submittal");
        }

        function alertSMSNotConfirmed() {
            alert("To enable SMS Text Messaging Please Check 'Terms and Conditions' checkbox before Saving");
            bDontClose = true;
        }

        function alertSMSPhoneNotUnique() {
            alert("SMS Text Messaging error. This phone number is used on another subscription! Please Correct before Saving");
            bDontClose = true;
        }

        function pwdOldDontMatch() {
            alert("Your old password doesn't match current password, Please Correct and try again.");
            bDontClose = true;
        }

        function pwdConfirmDontMatch() {
            alert("Your new password doesn't match confirm password, Please Correct and try again.");
            bDontClose = true;
        }

        function pwdChangeSuccess() {
            alert("Your password has been changed successfully please use at next logon.");
        }

        function pwdChangeError() {
            alert("There was an error with changing your password. Your password has NOT been changed! Please try again");
            bDontClose = true;
        }


        if (!<%= bRefreshSession.ToString.Tolower %>) {
            //setdefaultCompType();

            if (!aerodexFlag) {

                if (<%= bCanHaveSMS.ToString.ToLower %>) {
                    enableSMS();
                }

            }

        }
        else {

            if (!aerodexFlag) {

                if (<%= bCanHaveSMS.ToString.ToLower %>) {
                    enableSMS();
                }

            }

            //alert("bDontClose : " + bDontClose)

            if ((typeof (window.opener) != "undefined") && (window.opener != null)) {
                if (!bDontClose) {
                    //window.close();
                }
            }
            else {
                if (!bDontClose) {
                    //window.close();
                }
            }
        }


        function RedrawDatatablesOnSys() {
            setTimeout(reRenderThem, 1800);
        }

        function reRenderThem() {
            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            $($.fn.dataTable.tables(true)).DataTable().scroller.measure();
            $($.fn.dataTable.tables(true)).DataTable().responsive.recalc()
        }

        function selectAllRows(data, selectedRows, tableName) {

            var IDsToUse = '';
            var count = 0;

            data.each(function (value, index) {
                if (IDsToUse.length == 0) {
                    IDsToUse = value[1];
                } else {
                    IDsToUse += ', ' + value[1];
                }
                count += 1;
            });

            $("#" + selectedRows).val(IDsToUse);

            //      if (tableName == "folderDataTable") {

            //        var displayTotal = $("#folderLabel");
            //        displayTotal.html("");

            //        $("<div/>", {
            //          html: "<strong>" + count + " Folder(s)</strong>"
            //        }).appendTo(displayTotal);

            //      } else {

            //        var displayTotal = $("#templateLabel");
            //        displayTotal.html("");

            //        $("<div/>", {
            //          html: "<strong>" + count + " Template(s)</strong>"
            //        }).appendTo(displayTotal);

            //      }

        }

        function CreateTheDatatable(divName, tableName, jQueryTablename) {

            var selectedRows = '';

            try {
                if ($.fn.DataTable.isDataTable("#" + jQueryTablename)) {
                    $("#" + divName).empty();
                };
            }
            catch (err) {

            }

            if ($("#" + tableName).length) {

                //jQuery("#" + tableName).css('display', 'block');

                var clone = jQuery("#" + tableName).clone(true);

                jQuery("#" + tableName).css('display', 'none');
                clone[0].setAttribute('id', jQueryTablename);
                clone.appendTo("#" + divName);

                var table = $("#" + jQueryTablename).DataTable({
                    destroy: true,
                    fixedHeader: true,
                    "initComplete": function (settings, json) {
                        setTimeout(function () {
                            $("#" + jQueryTablename).DataTable().columns.adjust();
                            $("#" + jQueryTablename).DataTable().scroller.measure();

                            //var dataRows = $("#" + jQueryTablename).DataTable().rows();
                            //selectAllRows(dataRows.data(), selectedRows, tableName);

                        }, 1200)
                    },
                    scrollCollapse: true,
                    stateSave: true,
                    paging: false,
                    order: [[0, 'asc']],
                    dom: 't'

                    //          scrollCollapse: true,
                    //          stateSave: true,
                    //          paging: false,
                    //          columnDefs: [
                    //                      { targets: [1], className: 'display_none' },
                    //                      { orderable: false, className: 'select-checkbox', width: '10px', targets: [0] }
                    //                    ],
                    //          select: { style: 'single', selector: 'td:first-child' },
                    //          order: [[2, 'asc']],
                    //          dom: 'Bftrp',
                    ////          buttons: [
                    //                  { extend: 'colvis', text: 'Columns', collectionLayout: 'fixed two-column', postfixButtons: ['colvisRestore'] },

                    //                    { text: 'Mark for Edit', className: 'EditRowValue',
                    //                      action: function(e, dt, node, config) {

                    //                        dt.draw();
                    //                        selectAllRows(dt.rows({ selected: true }).data(), selectedRows, tableName);

                    //                      }
                    //                    },

                    //                    { text: 'Delete Selected Row', className: 'DeleteTableRow',
                    //                      action: function(e, dt, node, config) {

                    //                        dt.rows({ selected: true }).remove().draw(false);
                    //                        selectAllRows(dt.rows({ selected: true }).data(), selectedRows, tableName);

                    //                      }
                    //                    },

                    //                  { text: 'Reload Table', className: 'RefreshTableValue',
                    //                    action: function(e, dt, node, config) {

                    //                      $("#" + selectedRows).val('');
                    //                      ChangeTheMouseCursorOnItemParentDocument('cursor_wait');

                    //                    }
                    //                  }
                    //                 ]
                });
            }

            //$(".RefreshTableValue").addClass('display_none');

            //if (tableName == "qsearchDataTable") {
            //  $(".EditRowValue").addClass('display_none');
            //  $(".KeepTableRow").addClass('display_none');
            //}

            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            $($.fn.dataTable.tables(true)).DataTable().scroller.measure();
        };

    </script>

</asp:Content>
