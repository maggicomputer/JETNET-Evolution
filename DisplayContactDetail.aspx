<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DisplayContactDetail.aspx.vb"
    Inherits="crmWebClient.DisplayContactDetail" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        var chatWithUser = "<%= strUserEmailAddress.Trim %>";
        var chatWithUserID = <%= nUserEmailAddressChatID.ToString %>;
        var chatWithUserFriendlyName = "<%= sImageTitle.trim %>";

        var btnAddToListID = "<%= btnAddToList.ClientID %>";
        var btnRemoveFromListID = "<%= btnRemoveFromList.ClientID %>";
        var lblCommunityListID = "<%= lblCommunityList.ClientID %>";

        var bChatEnabled = <%= bEnableChat.tostring.tolower %>;
        var bDontShowList = <%= bDontShowList.tostring.tolower %>;
        var bIsAdd = <%= bIsAdd.toString.toLower %>;

        function pageLoad(sender, e) {

            if (bChatEnabled) {
                if (!bDontShowList) {
                    if (bIsAdd) {
                        $("#" + btnAddToListID).show();
                        $("#" + btnRemoveFromListID).hide();
                        $("#" + lblCommunityListID).html("user to my JETNET Online Community");
                    }
                    else {
                        $("#" + btnAddToListID).hide();
                        $("#" + btnRemoveFromListID).show();
                        $("#" + lblCommunityListID).html("user from my JETNET Online Community");
                    }

                }
                else {
                    $("#" + btnAddToListID).hide();
                    $("#" + btnRemoveFromListID).hide();
                    $("#" + lblCommunityListID).hide();
                }

            }
            else {
                $("#" + btnAddToListID).hide();
                $("#" + btnRemoveFromListID).hide();
                $("#" + lblCommunityListID).hide();
            }

        }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server" ID="history_background" CssClass="">
    </asp:Panel>
    <div runat="server" id="toggle_vis" class="row contactContainer companyContainer">
        <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="" DisplayAfter="500" class="loadingScreenBox">
            <ProgressTemplate>
                <span></span>
                <div class="loader">Loading...</div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div class="sixteen columns Main_Aircraft_Display_Table">
            <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width='96%' runat="server"
                CssClass="DetailsBrowseTable">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                        <asp:Label runat="server" ID="PreviousConSwap" Visible="false">
                                    <input id="previousCon" type="button" value=" < Previous Contact "  />
                        </asp:Label>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="center" VerticalAlign="top">
                        <span class="backgroundShade">
                            <asp:UpdatePanel ID="control_update_panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="dropdownSettings-sub">
                                        <a href="javascript:void(0);">
                                            <img src="images/menu.svg" alt="Menu" /></a>
                                        <div class="dropdown-content-sub" style="right: 40px;">
                                            <div class="row">
                                                <div class="twelve columns">
                                                    <ul>
                                                        <li>
                                                            <asp:LinkButton ID="view_folders" runat="server" Visible="true"
                                                                OnClick="ViewContactFolders" CssClass="float_left">Folders</asp:LinkButton>
                                                        </li>
                                                        <li id="view_notes" runat="server" visible="false">
                                                            <asp:LinkButton CssClass="float_left" ID="view_notes_link" runat="server" Visible="false"
                                                                OnClick="ViewCompanyNotes">Notes/Actions</asp:LinkButton></li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="dropdownSettings-sub" runat="server" id="AddMenuItem" visible="false">
                                <a href="javascript:void(0);">
                                    <img src="images/edit.svg" alt="Edit" /></a>
                                <div class="dropdown-content-sub" style="right: 50px; text-align: left;">
                                    <div class="row">
                                        <div class="twelve columns">
                                            <ul>

                                                <li runat="server" id="edit_company_link" visible="false">Edit Company</li>
                                                <asp:Literal runat="server" ID="viewOther" Visible="false"></asp:Literal>
                                                <li runat="server" id="new_company_link" visible="false"><a href="#" onclick="javascript:window.open('/edit.aspx?action=new&amp;type=company&amp;Listing=1&amp;from=companyDetails');">New Company</a></li>

                                                <li runat="server" id="Add_Note_Top" visible="false"></li>
                                                <li runat="server" id="Add_Action_Top" visible="false"></li>
                                                <li runat="server" id="Add_Prospect_Top" visible="false"></li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="dropdownSettings-sub" id="demo_trial_block" runat="server" visible="false">
                                <a href="#">
                                    <img src="images/edit.svg" alt="Edit" /></a>
                                <div class="dropdown-content-sub" style="text-align: left;">
                                    <div class="row">
                                        <div class="twelve columns">
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="create_demo_login" runat="server" OnClick="Create_Demo_Trials">Add Demo/Trial License</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="connect_trails" runat="server" Visible="False" OnClick="View_Connect_Trials">Connect Trial</asp:LinkButton>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="dropdownSettings-sub" id="cssExportMenu" runat="server">
                                <a href="#">
                                    <img src="images/download.svg" alt="Help" /></a>
                                <div class="dropdown-content-sub">
                                    <a href="#"><strong>EXPORT/REPORTS</strong></a>
                                    <ul>
                                        <li>
                                            <asp:LinkButton ID="export_company" runat="server">Company to Outlook</asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="export_contact" runat="server">Contact to Outlook</asp:LinkButton></li>


                                    </ul>
                                </div>
                            </div>

                            <a href="#" class=" float_right" onclick="javascript:window.close();">
                                <img src="images/x.svg" alt="Help" /></a>

                        </span>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="3" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="NotesHeader"
                        BackColor="#4d4d4d" ForeColor="White">
                        <asp:Label ID="browseTableTitle" runat="server" Text=""></asp:Label>
                        <asp:Label runat="server" ID="browse_label" Visible="false">Record
              <asp:Label ID="currentRecLabel" runat="server" Text="1"></asp:Label>
                            of
              <asp:Label ID="totalRecLabel" runat="server" Text="1"></asp:Label>
                            found</asp:Label>
                        <asp:Label runat="server" ID="NextCompSwap" CssClass="float_right" Visible="false">
                                    <input id="nextCon" type="button" value="Next Contact > " class="gray_button" />
                        </asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <div class="row valueSpec viewValueExport Simplistic aircraftSpec">
                <div class="grid">
                    <asp:Button runat="server" ID="refreshPage" Text="Refresh Page" CssClass="display_none" />
                    <asp:Panel runat="server" ID="informationContainer" CssClass="grid-item">
                        <asp:Label ID="company_information_label" runat="server"></asp:Label>
                        <asp:Label ID="company_address" runat="server" CssClass="display_none"></asp:Label>
                    </asp:Panel>
                    <div class="grid-item">
                        <div class="Box specialHeadingTable">
                            <div class="row remove_margin">
                                <div class="subHeader padding_left emphasisColor">
                                    <asp:Label runat="server" ID="contactNameText"></asp:Label>
                                </div>
                                <br />
                                <div class="columns eight remove_margin">
                                    <asp:Label ID="contact_information_label" runat="server"></asp:Label>
                                    <asp:Image ID="contact_chat_img" Width="60px" runat="server" ImageUrl="/images/user_male.png"
                                        Visible="False" Style="cursor: pointer;" OnClientClick="fnStartNewChat(chatWithUser,chatWithUserID,chatWithUserFriendlyName);return false;" />
                                    <asp:Image ID="contact_chat_img_offline" Width="60px" runat="server" ImageUrl="/images/user_male_gray.png"
                                        Visible="False" Style="cursor: pointer;" />
                                    <asp:Image ID="contact_chat_img_self" Width="60px" runat="server" ImageUrl="/images/user_male.png"
                                        Visible="False" />
                                    <asp:Label ID="contact_chat_label" runat="server" CssClass="padding display_block"
                                        Visible="False"></asp:Label>
                                </div>
                                <div class="columns four remove_margin">
                                    <asp:Label ID="contact_picture" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row remove_margin">
                                <div class="columns sixteen remove_margin">
                                    <asp:Button ID="btnAddToList" runat="server" OnClientClick="fnAddCommunityUser(chatWithUser,chatWithUserID);return false;"
                                        Text="Add" Style="width: 45px; height: 25px;" Visible="true" />
                                    <asp:Button ID="btnRemoveFromList" runat="server" OnClientClick="fnRemoveCommunityUser(chatWithUser,chatWithUserID);return false;"
                                        Text="Remove" Style="width: 70px; height: 25px;" Visible="true" />
                                    <asp:Label ID="lblCommunityList" runat="server" Visible="true" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="notes_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="notesPanel" Visible="false" CssClass="grid-item">
                                <div class="Box">
                                    <table class="formatTable blue" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <div class="subHeader">
                                                    Notes
                          <asp:Label ID="notes_add_new" runat="server" CssClass="float_right smallLink upperCase display_inline_block"
                              Style="width: 65%"></asp:Label>
                                                </div>
                                                <asp:Label ID="notes_label" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>

                            <asp:Panel runat="server" ID="actionPanel" Visible="false" CssClass="grid-item">
                                <div class="Box">
                                    <table class="formatTable blue" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <div class="subHeader">
                                                    ACTION ITEMS
                          <asp:Label ID="action_add_new" runat="server" CssClass="float_right smallLink upperCase"></asp:Label>
                                                </div>
                                                <asp:Label ID="action_label" runat="server" Text=""></asp:Label><asp:LinkButton runat="server"
                                                    ID="closeNotes" CssClass="float_right padding" OnClick="ViewCompanyNotes" Visible="false">Close Notes/Actions</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="Trials_Container" Visible="false" CssClass="grid-item">
                                <div class="Box">
                                    <div class="subHeader">
                                        Trial Summary
                                    </div>
                                    <div id="trialsButtons" style="text-align: right; padding-right: 8px;">
                                        <asp:LinkButton ID="trials_link_button_all" runat="server" Text="Show Inactive" CssClass="float_right padding" />
                                        <asp:LinkButton ID="trails_link_button_active" runat="server" Text="Show Active" CssClass="float_right padding" Visible="false" />
                                    </div>
                                    <br /> 
                                    <asp:Label runat="server" ID="trial_label"></asp:Label>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>



                    <asp:UpdatePanel ID="folders_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="foldersContainer" CssClass="grid-item" Visible="false">
                                <div class="Box">
                                    <div class="subHeader">
                                        Folders<asp:LinkButton runat="server" ID="closeFolders" CssClass="float_right padding"
                                            OnClick="ViewContactFolders" Visible="false">Close Folders</asp:LinkButton>
                                    </div>
                                    <asp:Label ID="folders_label" runat="server" CssClass="small_panel_height"></asp:Label>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Panel runat="server" ID="historyContainer" Visible="false" CssClass="grid-item">
                        <div class="subHeader" id="historyHeaderText" runat="server">
                        </div>
                        <asp:Label ID="history_information_label" runat="server"></asp:Label>
                    </asp:Panel>

                    <asp:Label ID="contact_information_other_listing_label" runat="server" CssClass="grid-item"
                        Visible="False"></asp:Label>
                    <asp:UpdatePanel ID="ProspectUpdate" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="prospectsContainer" CssClass="display_none">
                                <div class="Box">
                                    <table class="formatTable blue" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <div class="subHeader">
                                                    PROSPECTS/OPPORTUNITIES<asp:Label ID="new_prospects_add" runat="server" CssClass="float_right smallLink"></asp:Label>
                                                </div>
                                                <asp:Label ID="prospects_label" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:Panel runat="server" ID="edit_trial_panel" Visible="false" CssClass="grid-item">
                        <div class="Box">
                            <asp:Label runat="server" ID="edit_trial_label" Text=""></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:UpdatePanel ID="user_accounts_update" runat="server" ChildrenAsTriggers="true"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="user_account_panel" Visible="false" CssClass="grid-item">
                                <div class="Box">
                                    <div class="subHeader">
                                        User Accounts
                                    </div>
                                    <asp:Label runat="server" ID="user_account_label"></asp:Label>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="customerActivitiesUpdate" runat="server" ChildrenAsTriggers="true"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="customer_activities_panel" Visible="false" CssClass="grid-item">
                                <div class="Box">
                                    <div class="subHeader">
                                        Customer Activities
                                    </div>
                                    <div style='max-height: 470px; overflow: auto;'>
                                        <div id="activitiesButtons" style="text-align: right; padding-right: 8px;">
                                            <asp:LinkButton ID="showAllActivities" runat="server" Text="Show All" CssClass="float_right padding" />
                                            <asp:LinkButton ID="showTop50Activities" runat="server" Text="Show Last 50" CssClass="float_right padding"  Visible="false" />
                                        </div>
                                        <br />
                                        <asp:DropDownList runat="server" ID="customerActivitiesFilter" AutoPostBack="true">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                            <asp:ListItem Value="DOCUMENT">Contracts/Documents</asp:ListItem>
                                            <asp:ListItem Value="ACTIVITY">Technical Support</asp:ListItem>
                                            <asp:ListItem Value="MARKETING">Marketing Activities</asp:ListItem>
                                            <asp:ListItem Value="EXECUTION">Executions</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="customerActivities_Label"></asp:Label>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Panel runat="server" ID="aircraftPanel" CssClass="grid-item">
                        <div class="Box">
                            <asp:Label runat="server" ID="AircraftTextHeader" CssClass="subHeader">RELATED AIRCRAFT LISTINGS</asp:Label>
                            <asp:DataGrid ID="ac_results" runat="server" AutoGenerateColumns="False" Width="100%"
                                PageSize="50" CellSpacing="3" CellPadding="3" CssClass="formatTable blue small aircraftTable"
                                GridLines="None">
                                <HeaderStyle CssClass="header_row" VerticalAlign="bottom" />
                                <AlternatingItemStyle CssClass="alt_row" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Make/Model" ItemStyle-Width="120" ItemStyle-VerticalAlign="Top">
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "amod_make_name").ToString%>
                                            <%#DataBinder.Eval(Container.DataItem, "amod_model_name").ToString%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Ser #/Reg" ItemStyle-Wrap="false" ItemStyle-VerticalAlign="Top">
                                        <ItemTemplate>
                                            <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString, "", "")%>
                      /
                      <%#DataBinder.Eval(Container.DataItem, "ac_reg_no").ToString%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Status" ItemStyle-VerticalAlign="Top">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="forsale" CssClass="company_aircraft_list">  <%#crmWebClient.clsGeneral.clsGeneral.DisplayStatusListingDateEvoACListing(DataBinder.Eval(Container.DataItem, "ac_forsale_flag"), DataBinder.Eval(Container.DataItem, "ac_status").ToString, DataBinder.Eval(Container.DataItem, "ac_delivery"), DataBinder.Eval(Container.DataItem, "ac_asking_price"), DataBinder.Eval(Container.DataItem, "ac_date_listed"), DataBinder.Eval(Container.DataItem, "ac_asking_wordage"), True, Now())%></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Relationship - Company" ItemStyle-VerticalAlign="Top">
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "actype_name").ToString%>
                      -
                      <%#DataBinder.Eval(Container.DataItem, "comp_name").ToString%><%#display_comp_city(DataBinder.Eval(Container.DataItem, "comp_city").ToString, DataBinder.Eval(Container.DataItem, "comp_state").ToString)%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Relationship" ItemStyle-VerticalAlign="Top" Visible="false">
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "actype_name").ToString%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                            <asp:DataGrid runat="server" ID="aircraftDataGrid_YachtSpot" AutoGenerateColumns="false"
                                Width="100%" PageSize="50" Visible="true" CellSpacing="3" CellPadding="3" CssClass="formatTable blue"
                                GridLines="None">
                                <HeaderStyle CssClass="header_row" />
                                <AlternatingItemStyle CssClass="alt_row" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Aircraft" ItemStyle-VerticalAlign="Top">
                                        <ItemTemplate>
                                            <a href='#' style='font-weight: 100;' onclick="javascript:window.open('DisplayCompanyDetail.aspx?jetnet_note=Y','','scrollbars=yes,menubar=no,height=150,width=800,resizable=yes,toolbar=no,location=no,status=no');">
                                                <%#DataBinder.Eval(Container.DataItem, "amod_make_name").ToString%>
                                                <%#DataBinder.Eval(Container.DataItem, "amod_model_name").ToString%>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="acTransContainer" CssClass="grid-item" Visible="false">
                        <div class="Box">
                            <div class="subHeader">
                                AIRCRAFT RELATED TRANSACTIONS
                            </div>
                            <asp:DataGrid runat="server" ID="ac_trans_grid" AutoGenerateColumns="False" Width="100%"
                                CellSpacing="3" CellPadding="3" CssClass="formatTable blue small" GridLines="None"
                                ItemStyle-VerticalAlign="Top">
                                <HeaderStyle CssClass="header_row" />
                                <AlternatingItemStyle CssClass="alt_row" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Date">
                                        <ItemTemplate>
                                            <%#crmWebClient.clsGeneral.clsGeneral.TwoPlaceYear(DataBinder.Eval(Container.DataItem, "journ_date"))%>
                    -
                    <%#DataBinder.Eval(Container.DataItem, "amod_make_name").ToString%>
                                            <%#DataBinder.Eval(Container.DataItem, "amod_model_name").ToString%>
                    Ser#:&nbsp;<a href='DisplayAircraftDetail.aspx?acid=<%#DataBinder.Eval(Container.DataItem, "ac_id").ToString%>&jid=<%#DataBinder.Eval(Container.DataItem, "journ_id").ToString%>'
                        target='_blank'><%#DataBinder.Eval(Container.DataItem, "ac_ser_no").ToString.Trim%></a>-
                    <%#IIf(Not String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "jcat_subcategory_name")), DataBinder.Eval(Container.DataItem, "jcat_subcategory_name").ToString & " -", "")%>
                                            <%#DataBinder.Eval(Container.DataItem, "journ_subject").ToString%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Description">
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "actype_name").ToString%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                            <asp:Label ID="ac_trans_label" runat="server"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="yachtContainer" CssClass="grid-item" Visible="false" runat="server">
                        <div class="Box">
                            <div class="subHeader" runat="server" id="yachtHeader">
                                YACHTS
                            </div>
                            <asp:DataGrid runat="server" ID="YachtDataGrid" AutoGenerateColumns="False" Width="100%"
                                CellSpacing="3" CellPadding="3" CssClass="formatTable blue" GridLines="None">
                                <HeaderStyle CssClass="header_row" />
                                <AlternatingItemStyle CssClass="alt_row" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="<b class='title'>Brand/Model</b>">
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "ym_brand_name").ToString%>/<%#DataBinder.Eval(Container.DataItem, "ym_model_name").ToString%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="<b class='title'>Name</b>">
                                        <ItemTemplate>
                                            <a href="#" onclick="javascript:load('DisplayYachtDetail.aspx?yid=<%#DataBinder.Eval(Container.DataItem, "yt_id").ToString%>','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">
                                                <%#DataBinder.Eval(Container.DataItem, "yt_yacht_name").ToString%></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="<b class='title'>Year</b>">
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "yt_year_mfr").ToString%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="<b class='title'>Hull#</b>">
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "yt_hull_mfr_nbr").ToString%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="<b class='title'>Relationship - Company</b>">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="type" CssClass='<%#IIF(instr(DataBinder.Eval(Container.DataItem, "yct_name"),"Central Agent") > 0,"light_purple_background padding_text", "" & IIF(instr(DataBinder.Eval(Container.DataItem, "yct_name"),"Lessee") > 0,"light_orange_background padding_text", "") & "") %>'><%#DataBinder.Eval(Container.DataItem, "yct_name").ToString%> - <%#DataBinder.Eval(Container.DataItem, "comp_name").ToString%><%#display_comp_city(DataBinder.Eval(Container.DataItem, "comp_city").ToString,DataBinder.Eval(Container.DataItem, "comp_state").ToString)%></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                            <asp:Label ID="yacht_label" runat="server"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="yachtTransContainer" CssClass="grid-item" Visible="false" runat="server">
                        <div class="Box">
                            <div class="subHeader" runat="server" id="yachtTransText">
                                YACHTS
                            </div>
                            <asp:DataGrid runat="server" ID="yacht_trans_grid" AutoGenerateColumns="False" Width="100%"
                                CellSpacing="3" CellPadding="3" CssClass="formatTable blue" GridLines="None">
                                <HeaderStyle CssClass="header_row" />
                                <AlternatingItemStyle CssClass="alt_row" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="<b class='title'>Transaction Description</b>">
                                        <ItemTemplate>
                                            <%#FormatDateTime(DataBinder.Eval(Container.DataItem, "journ_date").ToString, DateFormat.ShortDate)%>
                    - <a href='DisplayYachtDetail.aspx?yid=<%#DataBinder.Eval(Container.DataItem, "yt_id").ToString%>&jid=<%#DataBinder.Eval(Container.DataItem, "journ_id").ToString%>'
                        target='_blank'>
                        <%#DataBinder.Eval(Container.DataItem, "ym_brand_name")%>&nbsp;<%#DataBinder.Eval(Container.DataItem, "ym_model_name")%>&nbsp;
                      <%#DataBinder.Eval(Container.DataItem, "yt_yacht_name").ToString%></a> -
                    <%#DataBinder.Eval(Container.DataItem, "jcat_subcategory_name").ToString%>
                    -
                    <%#DataBinder.Eval(Container.DataItem, "journ_subject").ToString%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="<b class='title'>Relationships</b>">
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "yct_name").ToString%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                            <asp:Label ID="yacht_trans_label" runat="server"></asp:Label>
                        </div>
                    </asp:Panel>
                </div>

            </div>
            <asp:UpdatePanel ID="create_demo_update" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="create_subscription_panel" Visible="false" CssClass="grid-item">
                        <div class="Box">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div class="subHeader">Create Demo Login</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Service Level:</td>
                                    <td>
                                        <asp:DropDownList ID="available_services" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Login Name:</td>
                                    <td>
                                        <asp:TextBox ID="txtLoginName" runat="server" Text=""></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Password:</td>
                                    <td>
                                        <asp:TextBox ID="txt_sub_password" runat="server" Text=""></asp:TextBox>

                                    </td>
                                    <!--  <td>
                                <asp:Button ID="Generate_Password" runat="server" Text="Generate Random Password" />
                                 OnClick="Generate_Random_Password" 
                            </td>-->
                                </tr>
                                <tr>
                                    <td>Values?:</td>
                                    <td>
                                        <asp:CheckBox ID="Values_Checkbox" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td>Platform Name:</td>
                                    <td>
                                        <asp:TextBox ID="txt_Platform_Name" runat="server"></asp:TextBox></td>
                                </tr>
                                <!--
            <tr>
                <td>Reply Name:</td><td>
                    <asp:TextBox ID="txtReplyName" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Reply Email:</td><td>
                    <asp:TextBox ID="txtReplyEMail" runat="server"></asp:TextBox></td>
            </tr>
            -->
                                <tr>
                                    <td>
                                        <asp:Button ID="save_update_demo" runat="server" Text="Create Demo Account" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="create_results_label" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">
    <script>

        $(document).ready(function () {
            loadMasonry()
        });


        function fnRefreshPage() {
            $('#<%= refreshPage.ClientID%>').click();
        }

    </script>
</asp:Content>
