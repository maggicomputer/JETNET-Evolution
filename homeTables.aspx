<%@ Page ValidateRequest="false" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/EmptyHomebaseTheme.Master" CodeBehind="homeTables.aspx.vb" Inherits="crmWebClient.homeTables" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyHomebaseTheme.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.24/themes/smoothness/jquery-ui.css" />
    <link href="common/aircraft_model.css" type="text/css" rel="stylesheet" />
    <link href="EvoStyles/stylesheets/tableThemes.css" type="text/css" rel="stylesheet" />



    <script type="text/javascript">

        function openSmallWindowJS(address, windowname) {
            var rightNow = new Date();
            windowname += rightNow.getTime();
            var Place = window.open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1150,resizable=yes,toolbar=no,location=no,status=no");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="companyContainer">
        <div class="valueSpec viewValueExport Simplistic aircraftSpec">
            <asp:Panel runat="server" ID="contentClass">
                <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width='100%' runat="server"
                    class="DetailsBrowseTable">
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="right" VerticalAlign="middle">
              <div class="backgroundShade">
                <a href="#" onclick="javascript:window.close();return false;" class="gray_button float_left"><strong>Close</strong></a>
              </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
            <asp:Panel runat="server" ID="listing_panel">
                <div class="row">
                    <div class="seven columns remove_margin main">
                        <asp:Label ID="information_label" runat="server" Text=""></asp:Label>
                        <asp:Label ID="company_address" runat="server" CssClass="display_none"></asp:Label>
                        <asp:Label ID="company_name" runat="server" CssClass="display_none"></asp:Label>
                        <asp:Label ID="about_label" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="five columns main">
                    </div>
                </div>
                <div class="row" runat="server" id="companyMarketingNoteEdit" visible="false">
                    <div class="twelve columns main" style="margin-left: 1% !important; width: 98%;">
                        <div class="Box">
                            <div class="subHeader emphasisColor" style="padding-left: 5px;">Marketing Summary</div>
                            <asp:Label runat="server" ID="editMarketingSummaryNoteLabel" ForeColor="Red" Font-Bold="true" Visible="false"></asp:Label>
                            <table class="formatTable blue" width="100%">
                                <tbody>
                                    <tr>
                                        <td width="60" valign="top"><strong>Note: </strong></td>
                                        <td>
                                            <asp:TextBox runat="server" ID="marketingNote" Width="100%" Rows="25" TextMode="MultiLine"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button runat="server" ID="editMarketingSummaryNote" Text="Update" CssClass="float_right" OnClientClick="htmlMarketingEncode();" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="row" runat="server" id="journalInformationRow" visible="false">
                    <asp:Panel runat="server" ID="journalEdit" Visible="false" CssClass="Box">
                        <div class="subHeader padding_left emphasisColor">Details</div>
                        <asp:Label runat="server" ID="attentionJournal" ForeColor="Red" Font-Bold="true"></asp:Label>
                        <asp:ValidationSummary runat="server" ID="valSummary" ValidationGroup="JournalForm" ForeColor="Red" Font-Bold="true" DisplayMode="BulletList" CssClass="circle" />
                        <table class="formatTable blue" width="100%">
                            <tbody>
                                <tr>
                                    <td width="80"><strong>Date: </strong></td>
                                    <td width="140">
                                        <asp:CompareValidator ID="CompareValidator1" Display="Dynamic" ForeColor="Red" ControlToValidate="journ_date" Text="*" Operator="DataTypeCheck" Type="Date" runat="server" ErrorMessage="Date must be in date format."></asp:CompareValidator>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="journ_date" ForeColor="Red"
                                            Display="Dynamic" ErrorMessage="Date Required" ValidationGroup="JournalForm"
                                            Text="*" ToolTip="*Date Required" Font-Bold="True"></asp:RequiredFieldValidator>
                                        <asp:TextBox runat="server" ID="journ_date" Width="70" CssClass="float_left"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="journ_date"
                                            Format="d" PopupButtonID="cal_image" />
                                        <asp:Image runat="server" ID="cal_image" ImageUrl="~/images/final.jpg" CssClass="float_left" /></td>
                                    <td width="50"><strong>User: </strong></td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="journ_user"></asp:DropDownList></td>
                                </tr>
                                <tr runat="server" id="journContactRow" visible="false">
                                    <td><strong>Contact: </strong></td>
                                    <td colspan="4">
                                        <asp:DropDownList runat="server" ID="journ_contact">
                                            <asp:ListItem Value="0">NONE SELECTED</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr runat="server" id="journAircraftRow" visible="false">
                                    <td><strong>Aircraft: </strong></td>
                                    <td colspan="4">
                                        <asp:DropDownList runat="server" ID="journ_ac"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td><strong>Note Type: </strong></td>
                                    <td>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="journ_note_type"
                                            Display="Dynamic" ErrorMessage="Note Type Required" ValidationGroup="JournalForm" CssClass="float_right"
                                            Text="*" ToolTip="*Note Type Required" Font-Bold="True" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:DropDownList runat="server" ID="journ_note_type" Width="93%"></asp:DropDownList></td>

                                    <td><strong>Subject: </strong></td>
                                    <td>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="journ_subject"
                                            Display="Dynamic" ErrorMessage="Subject Required" ValidationGroup="JournalForm"
                                            Text="*" ToolTip="*Subject Required" Font-Bold="True" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:TextBox runat="server" ID="journ_subject" Width="100%"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="4"><strong>Description:</strong><br>
                                        <asp:TextBox runat="server" ID="journ_description" TextMode="MultiLine" Rows="15" Width="100%" CssClass="textAreaLimit" MaxLength="4000"></asp:TextBox>
                                        <p>Characters Remaining: <span runat="server" id="textRemaining" class="textAreaDisplay red_text">4000</span>.</p>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Button runat="server" ID="removeMarketingNote" Text="Remove Note" CssClass="float_left" CausesValidation="false" Visible="false" OnClientClick="htmlEncode();" />
                                        <asp:Button runat="server" ID="saveMarketingNote" Text="Save Note" CssClass="float_right" ValidationGroup="JournalForm" OnClientClick="htmlEncode();" /><asp:Button runat="server" ID="addMarketingNote" Text="Add Note" CssClass="float_right" Visible="false" ValidationGroup="JournalForm" OnClientClick="htmlEncode();" /></td>
                                </tr>
                            </tbody>
                        </table>

                    </asp:Panel>
                    <asp:Label runat="server" ID="journalDisplayText" Text=""></asp:Label>
                </div>
                <div class="row" runat="server" id="companyListingGrid">
                    <div class="twelve columns main" style="margin-left: 1% !important; width: 98%;">
                        <div class="Box">
                            <asp:Label runat="server" ID="listing_label" Text=""></asp:Label>
                        </div>
                    </div>
                </div>

            </asp:Panel>
            <div class="row">
                <asp:Panel ID="execution_panel" CssClass="twelve columns main" runat="server" Visible="false" Style="margin-left: 1% !important; width: 98%;">
                    <div class="Box">
                        <div class="subHeader">Customer Execution</div>
                        <asp:Table runat="server" CssClass="formatTable blue" Width="100%">
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="Left" Width="60px">
                                    SubID:&nbsp;
                                    <asp:DropDownList runat="server" ID="exec_sub_drop"></asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <asp:Label runat="server" ID="exec_label" Text=""></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="2">
                                    <asp:Table runat="server" CssClass="formatTable blue" Width="100%">
                                        <asp:TableRow>
                                            <asp:TableCell HorizontalAlign="Left" Width="70px">
                                      Exc Date: 
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left">
                                                <asp:Label runat="server" ID="exec_exc_date" Text=""></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="left" Width="30px">
                                      Seq#:
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" Width="60px">
                                                <asp:Label runat="server" ID="exec_seq" Text=""></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" Width="50px">
                                    Action:
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" Width="100px">
                                                <asp:DropDownList ID="exec_action_drop" runat="server"></asp:DropDownList>
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" Width="50px">
                                      Entered:
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left">
                                                <asp:Label runat="server" ID="exec_entered_date" Text=""></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                ID:
                                                <asp:Label runat="server" ID="exec_id" Text=""></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="Left" ColumnSpan="2">
                                    <asp:Table runat="server" CssClass="formatTable blue" Width="100%">
                                        <asp:TableRow>
                                            <asp:TableCell HorizontalAlign="Left">
                                                <asp:CheckBox ID="exec_new_customer" Text="New Customer" runat="server" Font-Size="X-Small" />
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left">
                                                <asp:CheckBox ID="exec_trial" Text="Trial" runat="server" Font-Size="X-Small" />
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left">
                                                <asp:CheckBox ID="exec_new_contract" Text="New Contract" runat="server" Font-Size="X-Small" />
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left">
                                                <asp:CheckBox ID="exec_re_connected" Text="Re-Connected" runat="server" Font-Size="X-Small" />
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left">
                                                <asp:CheckBox ID="exec_addl_location" Text="Add'l Location" runat="server" Font-Size="X-Small" />
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left">
                                                <asp:CheckBox ID="exec_upgrade" Text="Upgrade" runat="server" Font-Size="X-Small" />
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left">
                                                <asp:CheckBox ID="exec_downgrade" Text="Downgrade" runat="server" Font-Size="X-Small" />
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left">
                                                <asp:CheckBox ID="exec_interrupted" Text="Interrupted" runat="server" Font-Size="X-Small" />
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left">
                                                <asp:CheckBox ID="exec_cancellation" Text="Cancellation" runat="server" Font-Size="X-Small" />
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left">
                                                <asp:CheckBox ID="exec_addl_license" Text="Addl' License" runat="server" Font-Size="X-Small" />
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="Left">
                                    <asp:Table runat="server" CssClass="formatTable blue" Width="100%">

                                        <asp:TableRow VerticalAlign="Top">
                                            <asp:TableCell HorizontalAlign="Left" Width="120px" VerticalAlign="Top">
                                            Notes:   
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                                <asp:TextBox ID="exec_notes" runat="server" Text="" Columns="80" Rows="10" TextMode="MultiLine"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="Left">
                                    <asp:Table runat="server" CssClass="formatTable blue" Width="100%">

                                        <asp:TableRow>
                                            <asp:TableCell HorizontalAlign="Left" Width="120px">
                                    Monthly List Fee: 
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                                <asp:TextBox ID="exec_list_fee" runat="server" Columns="9" Text=""></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell HorizontalAlign="Left" Width="120px">
                                    Monthly Billed Fee: 
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                                <asp:TextBox ID="exec_monthly_price" runat="server" Columns="9" Text=""></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell HorizontalAlign="Left" Width="120px">
                                    Monthly Net Change Fee: 
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                                <asp:TextBox ID="exec_monthly_net" runat="server" Columns="9" Text=""></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell HorizontalAlign="Left" Width="120px">
                                                Service Changed:
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                                <asp:TextBox ID="exec_service_changed" runat="server" Text="" Columns="12"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="Left" ColumnSpan="2">
                                    <asp:Button ID="exec_delete_button" runat="server" Text="Delete" CssClass="float_left" />
                                    <asp:Button ID="exec_add_button" runat="server" Text="Add" CssClass="float_right" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                </asp:Panel>
                <asp:Panel ID="edit_panel" CssClass="twelve columns main" runat="server" Visible="false" Style="margin-left: 1% !important; width: 98%;">
                    <div class="Box">
                        <div class="subHeader">Edit Service</div>
                        <asp:Label runat="server" ID="backButton" CssClass="float_right"></asp:Label>
                        <asp:Table runat="server" CssClass="formatTable blue" Width="100%">
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="Right" ColumnSpan="2">
                                    <asp:Button ID="cancel_button" runat="server" Text="Cancel" CssClass="float_right" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="Left" Width="120px">
                                    <asp:Label runat="server" ID="droplabel1" Text=""></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="Left">
                                    <asp:DropDownList ID="Dynamic_Dropdown1" runat="server" Width="171px">
                                    </asp:DropDownList>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="Left">
                                    <asp:Label ID="TextLabel1" runat="server" Text=""></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="Left">
                                    <asp:TextBox ID="Textbox1" runat="server"></asp:TextBox><br />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="Left">
                                    <asp:Label ID="DateLabel1" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="Left">
                                    <asp:TextBox ID="Datebox1" runat="server"></asp:TextBox><br />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="Left">
                                    <asp:Label ID="DateLabel2" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="Left">
                                    <asp:TextBox ID="Datebox2" runat="server"></asp:TextBox><br />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="Left">
                                    <asp:Label ID="BottomLabel1" runat="server" Text=""></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="Left">
                                    <asp:TextBox ID="BottomText1" runat="server" TextMode="MultiLine" Rows="7" Width="100%"></asp:TextBox><br />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="Left" ColumnSpan="2">
                                    <asp:Button ID="delete_button" runat="server" Text="Delete" CssClass="float_left" />
                                    <asp:Button ID="submit_button" runat="server" Text="Add" CssClass="float_right" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                </asp:Panel>
                <asp:Label ID="results_label" runat="server" Text=""></asp:Label>
            </div>
            <br clear="all" />
        </div>
        <br clear="all" />
    </div>





    <asp:Label runat="server" ID="note_text" Text="" ForeColor="Red"></asp:Label>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

    <script type="text/javascript">


        function checkInputLimit(e) {
            var tval = $('.textAreaLimit').val(),
                tlength = tval.length,
                set = 4000,
                remain = parseInt(set - tlength);

            if (remain <= 0) {
                $('.textAreaLimit').val((tval).substring(0, set))
            }
            $('.textAreaDisplay').text( parseInt( set - $('.textAreaLimit').val().length));
        };

        $(".textAreaLimit").on('input selectionchange propertychange', function (e) {
            checkInputLimit(e);
        });

        //Marketing Note Submission.
        function htmlMarketingEncode() {
            var input = '';
            input = $('#<%= marketingNote.ClientID %>').val();
            $('#<%= marketingNote.ClientID %>').val($('<span>').text(input).html());
            return false;
        }
        //Action Item submission. Kept separate on purpose for further extension involving limiting characters.
        function htmlEncode() {
            var input = '';
            input = $('#<%= journ_description.clientID %>').val();
            $('#<%= journ_description.clientID %>').val($('<span>').text(input).html());
        }
        function NoteSubjectReplace() {
            if ($('#<%= journ_note_type.ClientID %>').val() != '') {
                $('#<%= journ_subject.ClientID %>').val($('#<%= journ_note_type.ClientID %> option:selected').text());
            }
            //If Trim(journ_note_type.Text) <> "" Then
            //    journ_subject.Text &= journ_note_type.SelectedItem.Text
            //Else
            //    journ_subject.Text = journ_note_type.SelectedItem.Text
            //End If

        }
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
