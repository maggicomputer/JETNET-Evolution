<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Notes.ascx.vb" Inherits="crmWebClient.Notes" %>

<script type="text/javascript">
    function checkDate(sender, args) {
        if (sender._selectedDate > new Date()) {
            alert("You cannot select a day later than today.");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
        }
    }
    function checkDate_Future(sender, args) {
        if (sender._selectedDate < new Date()) {
            alert("You must select a future date.");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
        }
    }
</script>

<asp:Label ID="resize_function" runat="server">

    <script type="text/javascript">
        function FitPic() {
            window.resizeTo(950, 690);
            self.focus();
        };
    </script> </asp:Label>
<asp:Label ID="mobile_style" runat="server" Text="" Visible="false"> <link href="common/style.css" rel="stylesheet" type="text/css" /></asp:Label>
<div class="row remove_margin">
    <div class="six columns remove_margin">
        <asp:Panel runat="server" ID="edit_table">
            <asp:Panel runat="server" ID="aircraft_model_prospect_swap" Visible="false" CssClass="Box">
                <table width="100%" cellpadding="3" cellspacing="0">
                    <tr>
                        <td align="left" valign="top">Attach Prospect by:
                        </td>
                        <td align="left" valign="top">
                            <asp:RadioButtonList runat="server" ID="attach_prospect_by" RepeatColumns="3" RepeatDirection="Horizontal"
                                CssClass="float_right" AutoPostBack="true">
                                <asp:ListItem Value="AIRCRAFT" Text="Aircraft"></asp:ListItem>
                                <asp:ListItem Value="MODEL" Text="Model"></asp:ListItem>
                                <asp:ListItem Value="NEITHER" Text="Neither"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <br clear="all" />
            </asp:Panel>
            <asp:Panel runat="server" ID="model_information_panel" Visible="false" CssClass="Box">
                <div class="subHeader">
                    Aircraft Model Information:
                </div>
                <br />
                <table width="100%" cellpadding="3" cellspacing="0">
                    <tr>
                        <td align="left" valign="top" colspan="3">
                            <asp:DropDownList ID="model_name" runat="server" AutoPostBack="true" Width="350">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:Label ID="model_text" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="aircraft_information_panel">
                <div class="Box">
                    <div class="subHeader">
                        Aircraft Information:
                    </div>
                    <br />
                    <table width="100%" cellpadding="3" cellspacing="0">
                        <tr>
                            <td align="left" valign="top">
                                <asp:CheckBox ID="aircraft_related" runat="server" Text="Aircraft Related to Company"
                                    Checked="true" AutoPostBack="true" />
                                <asp:CheckBox ID="ProspectAircraft" runat="server" Text="Aircraft Prospects" Checked="false"
                                    Visible="true" AutoPostBack="true" />
                                <cc1:MutuallyExclusiveCheckBoxExtender ID="mecbe1" runat="server" TargetControlID="aircraft_related"
                                    Key="YesNo" Enabled="true" />
                                <cc1:MutuallyExclusiveCheckBoxExtender ID="mecbe2" runat="server" TargetControlID="ProspectAircraft"
                                    Key="YesNo" Enabled="true" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" colspan="3">
                                <asp:LinkButton ID="AC_Search_Vis" runat="server" Visible="false">Click for AC Search</asp:LinkButton>
                                <asp:Panel runat="server" ID="ac_search" Visible="false" CssClass="notes_pnl padding">
                                    <div class="subHeader">
                                        Search Parameters
                                    </div>
                                    <br />
                                    <table width="100%" align="center" cellpadding="3" cellspacing="0" border="0">
                                        <tr>
                                            <td align="left" valign="top" width="150">
                                                <asp:Label ID="ac_search_text" runat="server">Ser #/Reg #/Make/Model:</asp:Label>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox runat="server" ID="serial" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top"></td>
                                            <td align="right" valign="top">
                                                <asp:LinkButton ID="ac_search_buttonLB" runat="server" CssClass="button float_right mobile_float_right"
                                                    Text="Search" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <asp:DropDownList ID="aircraft_name" runat="server" AutoPostBack="true" Width="350">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" InitialValue="0||0" Enabled="false" ID="prospect_ac_required"
                                    ControlToValidate="aircraft_name" ValidationGroup="Notes_Edit" ErrorMessage="*Aircraft is Required for Prospect"
                                    Text="" Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <asp:Label ID="aircraft_info" runat="server" Text=""></asp:Label>
                                <asp:Label runat="server" Visible="false" ID="ac_id"></asp:Label>
                                <asp:Label runat="server" Visible="false" ID="acval_id"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Panel runat="server" ID="aircraftForSaleBlock" Visible="false" CssClass="Box">
                    <div class="subHeader">
                        Aircraft For Sale Information
                    </div>
                    <br />
                    <table width="100%" cellspacing="0" cellpadding="3">
                        <tr>
                            <td align="left" valign="top" colspan="2">For Sale?
                <asp:RadioButtonList ID="ac_sale" runat="server" RepeatDirection="Horizontal" alt="For Sale"
                    CssClass="float_right" AutoPostBack="true">
                    <asp:ListItem id="sale_yes" runat="server" Value="Y" Text="Yes" alt="For Sale - Yes" />
                    <asp:ListItem id="sale_no" runat="server" Value="N" Text="No" Selected="True" alt="For Sale - No" />
                </asp:RadioButtonList>
                            </td>
                            <td align="left" valign="top" width="40">Status?
                            </td>
                            <td align="left" valign="top" width="120">
                                <asp:DropDownList ID="ac_status_for_sale" runat="server" alt="Status" Width="120"
                                    AutoPostBack="true">
                                    <asp:ListItem Value="" Selected="True">Please Choose One</asp:ListItem>
                                    <asp:ListItem Value="Deal">Deal</asp:ListItem>
                                    <asp:ListItem Value="For Sale">For Sale</asp:ListItem>
                                    <asp:ListItem Value="For Sale/Best Deal">For Sale/Best Deal</asp:ListItem>
                                    <asp:ListItem Value="For Sale/Lease">For Sale/Lease</asp:ListItem>
                                    <asp:ListItem Value="For Sale/Off Market">For Sale/Off Market</asp:ListItem>
                                    <asp:ListItem Value="For Sale/Possible">For Sale/Possible</asp:ListItem>
                                    <asp:ListItem Value="For Sale/Trade">For Sale/Trade</asp:ListItem>
                                    <asp:ListItem Value="For Sale/Share">For Sale/Share</asp:ListItem>
                                    <asp:ListItem Value="Other">Other</asp:ListItem>
                                    <asp:ListItem Value="Sale Pending">Sale Pending</asp:ListItem>
                                    <asp:ListItem Value="Unconfirmed">Unconfirmed</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ac_status_not_for_sale" runat="server" alt="Status" Width="120"
                                    AutoPostBack="true">
                                    <asp:ListItem Value="" Selected="True">Please Choose One</asp:ListItem>
                                    <asp:ListItem Value="Not For Sale">Not For Sale</asp:ListItem>
                                    <asp:ListItem Value="Withdrawn from Use">Withdrawn from Use</asp:ListItem>
                                    <asp:ListItem Value="Withdrawn from Use – Display">Withdrawn from Use – Display</asp:ListItem>
                                    <asp:ListItem Value="Withdrawn from Use – Stored">Withdrawn from Use – Stored</asp:ListItem>
                                    <asp:ListItem Value="Withdrawn from Use – Tech School">Withdrawn from Use – Tech School</asp:ListItem>
                                    <asp:ListItem Value="Written Off">Written Off</asp:ListItem>
                                    <asp:ListItem Value="Written Accident">Written Accident</asp:ListItem>
                                    <asp:ListItem Value="Written Damage">Written Damage</asp:ListItem>
                                    <asp:ListItem Value="Written Display">Written Display</asp:ListItem>
                                    <asp:ListItem Value="Written - Fire">Written - Fire</asp:ListItem>
                                    <asp:ListItem Value="Written - War Casualty">Written - War Casualty</asp:ListItem>
                                    <asp:ListItem Value="Stolen">Stolen</asp:ListItem>
                                    <asp:ListItem Value="Other">Other</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="ac_status_hold" runat="server" Width="40" Style="display: none;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" width="107">
                                <span class="help_cursor" title="Describe attributes of this aircraft directly impacting the price such as inspections, maintenance, damage, low/high hours, etc.">Value/Price Desc.<img src='../images/magnify_small.png' alt="Describe attributes of this aircraft directly impacting the price such as inspections, maintenance, damage, low/high hours, etc." /></span>
                            </td>
                            <td align="left" valign="top" colspan="3">
                                <asp:TextBox runat="server" ID="cliaircraft_value_description_text" TextMode="MultiLine"
                                    alt="Value/Price Description" Rows="3" Width="100%" AutoPostBack="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="4">
                                <asp:Panel ID="date_listed_panel" runat="server">
                                    <table cellpadding="3" cellspacing="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" valign="top" width="32%">Date Listed
                                            </td>
                                            <td align="left" valign="top">
                                                <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="date_listed"
                                                    Format="d" PopupButtonID="cal_image" />
                                                <asp:TextBox ID="date_listed" runat="server" Width="70" MaxLength="15" alt="Date Listed"
                                                    AutoPostBack="true"></asp:TextBox>
                                                <asp:Image runat="server" ID="Image2" ImageUrl="~/images/final.jpg" />
                                            </td>
                                            <td align="left" valign="top" colspan="2">
                                                <asp:Label ID="DOMWord" runat="server" Text="Days on Market:" Visible="false" CssClass="tiny"></asp:Label>
                                                <asp:Label ID="DOMlisted" runat="server" Text="" CssClass="tiny"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">Asking Wordage
                        <br />
                                                <em class="tiny">Select Price if known.</em>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:DropDownList ID="asking_wordage" runat="server" Width="105" alt="Asking Wordage"
                                                    AutoPostBack="true">
                                                    <asp:ListItem Value="">Please Select One</asp:ListItem>
                                                    <asp:ListItem Value="Make Offer">Make Offer</asp:ListItem>
                                                    <asp:ListItem Value="Price">Price</asp:ListItem>
                                                    <asp:ListItem Value="Auction">Auction</asp:ListItem>
                                                    <asp:ListItem Value="Sale/Trade">Sale/Trade</asp:ListItem>
                                                    <asp:ListItem Value="Sale/Lease">Sale/Lease</asp:ListItem>
                                                    <asp:ListItem Value="Sale/Share">Sale/Share</asp:ListItem>
                                                    <asp:ListItem Value="Sealed Bid">Sealed Bid</asp:ListItem>
                                                    <asp:ListItem Value="Lease">Lease</asp:ListItem>
                                                    <asp:ListItem Value="Lease Only">Lease Only</asp:ListItem>
                                                    <asp:ListItem Value="Trade">Trade</asp:ListItem>
                                                    <asp:ListItem Value="Other">Other</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:Label ID="ask_lbl" runat="server">Asking Price</asp:Label>
                                            </td>
                                            <td align="left" valign="top" width="100">
                                                <asp:TextBox ID="asking_price" runat="server" MaxLength="20" Width="100" alt="Asking Price"
                                                    AutoPostBack="true">0.00</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                                <asp:Label ID="est_label" runat="server">Take Price</asp:Label>
                                            </td>
                                            <td align="left" valign="top" colspan="3">
                                                <asp:TextBox ID="est_price" runat="server" MaxLength="20" Width="100px" alt="Take Price"
                                                    Height="16px" AutoPostBack="true">0.00</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                                <asp:Label ID="broker_lbl" runat="server">Broker Price<br /><em class="tiny">(Estimated Value)</em></asp:Label>
                                            </td>
                                            <td align="left" valign="top" colspan="3">
                                                <asp:TextBox ID="broker_price" runat="server" MaxLength="20" Width="100px" AutoPostBack="true"
                                                    alt="Broker Price" Height="16px">0.00</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top"></td>
                                            <td align="left" valign="top" colspan="3">
                                                <asp:TextBox ID="delivery" runat="server" MaxLength="45" Width="120" Style="display: none;"
                                                    alt="AC Delivery"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>

        <!----Company----->
        <div class="Box">
            <div class="subHeader">
                Company Information:
            </div>
            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td align="left" valign="top">
                        <asp:CheckBox ID="company_related" runat="server" Text="Company Related to Aircraft"
                            Checked="true" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="left" valign="top">
                        <asp:LinkButton ID="company_search_vis" runat="server" Visible="false">Click for Company Search</asp:LinkButton>
                        <asp:Panel runat="server" ID="company_search" Visible="false" CssClass="notes_pnl padding">
                            <div class="subHeader">
                                Search Parameters
                            </div>
                            <br />
                            <table width="100%" align="center" cellpadding="3" cellspacing="0" border="0">
                                <tr>
                                    <td align="left" valign="top">Company Name:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox runat="server" ID="Name" Width="164" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">First/Last Name:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox runat="server" ID="first_name" Width="78" /><asp:TextBox runat="server"
                                            ID="last_name" Width="79" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">Email Address:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox runat="server" ID="email_address" Width="164" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">Phone Number:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox runat="server" ID="phone_number" Width="164" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top"></td>
                                    <td align="right" valign="top">
                                        <asp:LinkButton ID="company_search_buttonLB" runat="server" CssClass="button float_right mobile_float_right"
                                            Text="Search" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <asp:DropDownList ID="company_name" runat="server" Width="350" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="prospect_company_required" Enabled="false" runat="server"
                            ControlToValidate="company_name" ErrorMessage="*Company is Required for Prospect."
                            ValidationGroup="Notes_Edit" Text="" Display="None" InitialValue=""></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="prospect_company_required_2" Enabled="false" runat="server"
                            ControlToValidate="company_name" ErrorMessage="*Company is Required for Prospect."
                            ValidationGroup="Notes_Edit" Text="" Display="None" InitialValue="|"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <asp:Label ID="company_info" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <!---Contact--->
        <div class="Box">
            <div class="subHeader">
                Contact Information:
            </div>
            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td align="left" valign="top">
                        <asp:CheckBox ID="contact_related" Visible="false" runat="server" Text="Contacts Related to Company"
                            Checked="true" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="left" valign="top">
                        <asp:LinkButton ID="contact_search_vis" runat="server" Visible="false">Click for Contact Search</asp:LinkButton>
                        <asp:Panel runat="server" ID="contact_search" Visible="false" CssClass="notes_pnl padding">
                            <div class="subHeader">
                                Search Parameters
                            </div>
                            <br />
                            <table width="100%" align="center" cellpadding="3" cellspacing="0" border="0">
                                <tr>
                                    <td align="left" valign="top">First Name:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox runat="server" ID="first" Width="110" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">Last Name:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox runat="server" ID="last" Width="110" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top"></td>
                                    <td align="right" valign="top">
                                        <asp:LinkButton ID="contact_search_buttonLB" runat="server" CssClass="button float_right mobile_float_right"
                                            Text="Search" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <asp:Label ID="contact_info" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <asp:DropDownList ID="contact_name" runat="server" AutoPostBack="true" Width="350">
                            <asp:ListItem Value="">PLEASE SELECT A COMPANY</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <!---ACTIVITIES--->
        <div class="Box">
            <div class="subHeader">
                Customer Activities:
            </div>

            <asp:UpdatePanel ID="customerActivitiesUpdate" runat="server" ChildrenAsTriggers="true"
                UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="customer_activities_panel" Visible="false" CssClass="Box">
                        <div style='max-height: 470px; overflow: auto;'>
                            <div id="activitiesButtons" style="text-align: right; padding-right: 8px;">
                                <asp:LinkButton ID="showAllActivities" runat="server" Text="Show All" CssClass="float_right padding" PostBackUrl="~/DisplayCompanyDetail.aspx?task=showAll#customerActivities" />
                                <asp:LinkButton ID="showTop50Activities" runat="server" Text="Show Last 50" CssClass="float_right padding" PostBackUrl="~/DisplayCompanyDetail.aspx?task=topFifty#customerActivities" Visible="false" />
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
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
        <!---Display Portion Boxes-->
    </div>
<div class="six columns remove_margin">
        <div class="Box">
            <div class="subHeader">
                Note Information:
            </div>
            <br />
            <asp:TextBox ID="jetnet_ac" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="action" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="client_ac" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="jetnet_comp" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="client_comp" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="jetnet_contact" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="client_contact" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="jetnet_mod" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="client_mod" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:Label ID="attention" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Notes_Edit"
                DisplayMode="BulletList" EnableClientScript="true" HeaderText="There are problems with the following fields:" />
            <asp:Panel runat="server" ID="action_view" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="0" class="formatTable blue">
                    <tr>
                        <td align="left" valign="top">Date:
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="dated" runat="server" Width="70"></asp:TextBox>
                            <asp:Image runat="server" ID="cal_image" ImageUrl="images/final.jpg" />
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="dated"
                                Format="d" PopupButtonID="cal_image" />
                            &nbsp;&nbsp;Time:&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">Priority:
                        </td>
                        <td align="left" valign="top">
                            <asp:DropDownList ID="priority" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">Status:
                        </td>
                        <td align="left" valign="top">
                            <asp:RadioButtonList ID="statused" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Text="Active" Value="P" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Completed" Value="C"></asp:ListItem>
                                <asp:ListItem Text="Dismissed" Value="D"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div>
                <asp:Table ID="Table1" runat="server" CssClass="formatTable blue">
                    <asp:TableRow runat="server" ID="start_table_row" Visible="false">
                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="200" runat="server" ID="start_table_cell">
       Start Date</asp:TableCell>
                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">
                            <asp:TextBox ID="start_date" runat="server" Width="70"></asp:TextBox>
                            <asp:Image runat="server" ID="Image4" ImageUrl="~/images/final.jpg" />
                            <cc1:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="start_date"
                                Format="d" PopupButtonID="cal_image" />
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow runat="server" ID="prospectOppRow3" Visible="false">
                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="200" runat="server" ID="TargetDateText">
                            <asp:Label runat="server" Text="Target Closing Date" ID="target_label"></asp:Label></asp:TableCell>
                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">
                            <asp:TextBox ID="targetdate" runat="server" Width="70"></asp:TextBox>
                            <asp:Image runat="server" ID="Image3" ImageUrl="~/images/final.jpg" />
                            <cc1:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="targetdate"
                                Format="d" PopupButtonID="cal_image" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ID="notesCell" ColumnSpan="2" HorizontalAlign="Left" VerticalAlign="Top">
                            <asp:Label runat="server" ID="market_value_description_text" Visible="false">Market Value Analysis Description:<br /></asp:Label>
                            <asp:TextBox ID="notes_edit" runat="server" TextMode="MultiLine" Width="440" Height="180" TabIndex="0"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Enabled="true" runat="server"
                                ControlToValidate="notes_edit" ErrorMessage="*Note Text is Required" ValidationGroup="Notes_Edit"
                                Text="" Display="None"></asp:RequiredFieldValidator>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="estimated_value_tr" runat="server" Visible="false">
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" colspan='2'>
                            <font size='-2'><i>Enter a description of any features or factors impacting the estimated
                value of this aircraft.</i></font>
                            <h3 align="right">Aircraft Estimated Value Information</h3>
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr height='27' valign="top">
                                    <td align="left" valign="top" width="100">
                                        <asp:Label ID="estval_type_estimate_label" runat="server" Text="Type of Estimate"></asp:Label>
                                    </td>
                                    <td align='left' width="120" valign="top">
                                        <asp:DropDownList ID="estval_type_of" runat="server" AutoPostBack="true">
                                            <asp:ListItem Value=""></asp:ListItem>
                                            <asp:ListItem Value="F">Full Appraisal</asp:ListItem>
                                            <asp:ListItem Value="D">Desktop Appraisal</asp:ListItem>
                                            <asp:ListItem Value="V">VREF</asp:ListItem>
                                            <asp:ListItem Value="B">Blue Book</asp:ListItem>
                                            <asp:ListItem Value="H">HeliValue$</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr height='27'>
                                    <td align="left" valign="top" width="100">
                                        <asp:Label ID="estval_asking_price_label" runat="server">Asking Price</asp:Label>
                                    </td>
                                    <td align="left" valign="top" width="120">
                                        <asp:TextBox ID="estval_asking_price" runat="server" Style="text-align: right" MaxLength="20"
                                            Width="100" alt="Asking Price">0.00</asp:TextBox><asp:CompareValidator ID="CompareValidator3"
                                                ErrorMessage="Incorrect Format for Asking Price." Display="dynamic" Text="*" ControlToValidate="estval_asking_price"
                                                Operator="DataTypeCheck" Type="Currency" runat="server" ValidationGroup="Notes_Edit"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr height='27'>
                                    <td align="left" valign="top" width="100">
                                        <asp:Label ID="estval_take_price_label" runat="server">Take Price</asp:Label>
                                    </td>
                                    <td align="left" valign="top" colspan="3" width="120">
                                        <asp:TextBox ID="estval_take_price" runat="server" Style="text-align: right" MaxLength="20"
                                            Width="100px" alt="Take Price" Height="16px">0.00</asp:TextBox><asp:CompareValidator
                                                ID="CompareValidator4" ErrorMessage="Incorrect Format for Take Price." Display="dynamic"
                                                Text="*" ControlToValidate="estval_take_price" Operator="DataTypeCheck" Type="Currency"
                                                runat="server" ValidationGroup="Notes_Edit"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr height='27'>
                                    <td align="left" valign="top" width="100">
                                        <asp:Label ID="estval_est_value_label" runat="server" Text="Estimated Value"></asp:Label>
                                    </td>
                                    <td align="left" valign="top" colspan="3" width="120">
                                        <asp:TextBox ID="estval_estimated_value" Style="text-align: right" runat="server"
                                            MaxLength="20" Width="100px" alt="Estimated Value" Height="16px">0.00</asp:TextBox><asp:CompareValidator
                                                ID="CompareValidator5" ErrorMessage="Incorrect Format for Estimated Value." Display="dynamic"
                                                Text="*" ControlToValidate="estval_estimated_value" Operator="DataTypeCheck" Type="Currency"
                                                runat="server" ValidationGroup="Notes_Edit"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr height='27'>
                                    <td align="left" valign="top" width="100">
                                        <asp:Label ID="estval_aftt_label" runat="server" Text="AFTT"></asp:Label>
                                    </td>
                                    <td align="left" valign="top" colspan="3" width="120">
                                        <asp:TextBox ID="estval_aftt" Style="text-align: right" runat="server" MaxLength="20"
                                            Width="100px" alt="AFTT" Height="16px"></asp:TextBox><asp:CompareValidator ID="CompareValidator6"
                                                ErrorMessage="Incorrect Format for AFTT." Display="dynamic" Text="*" ControlToValidate="estval_aftt"
                                                Operator="DataTypeCheck" Type="double" runat="server" ValidationGroup="Notes_Edit"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr height='27'>
                                    <td align="left" valign="top" width="100">
                                        <asp:Label ID="estval_total_landing_label" runat="server" Text="Total Landings"></asp:Label>
                                    </td>
                                    <td align="left" valign="top" colspan="3" width="120">
                                        <asp:TextBox ID="estval_total_landings" Style="text-align: right" runat="server"
                                            MaxLength="20" Width="100px" alt="Total Landings" Height="16px"></asp:TextBox><asp:CompareValidator
                                                ID="CompareValidator7" ErrorMessage="Incorrect Format for Total Landings." Display="dynamic"
                                                Text="*" ControlToValidate="estval_total_landings" Operator="DataTypeCheck" Type="Double"
                                                runat="server" ValidationGroup="Notes_Edit"></asp:CompareValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="100">
                            <asp:Label runat="server" ID="enteredByText">Entered By:</asp:Label>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            <asp:DropDownList ID="pertaining_to" runat="server" Width="120">
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" runat="server" ID="category_cell">
            Category: 
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="right" VerticalAlign="Top" runat="server" ID="category_cell2">
                            <asp:DropDownList ID="notes_cat" runat="server" Width="120">
                            </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server" ID="prospectOppRow" Visible="false">
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="75">
                            Title: </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            <asp:TextBox ID="notes_title" runat="server" Width="320"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server" ID="prospectOppRow2" Visible="false">
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="75">
                            Cash Value: </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            <asp:TextBox ID="opp_cash" runat="server" Width="70" Text="0"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Capture %: &nbsp;
              <asp:DropDownList ID="capt_per" runat="server" Width="55">
                  <asp:ListItem Value="0">0%</asp:ListItem>
                  <asp:ListItem Value="10">10%</asp:ListItem>
                  <asp:ListItem Value="20">20%</asp:ListItem>
                  <asp:ListItem Value="30">30%</asp:ListItem>
                  <asp:ListItem Value="40">40%</asp:ListItem>
                  <asp:ListItem Value="50">50%</asp:ListItem>
                  <asp:ListItem Value="60">60%</asp:ListItem>
                  <asp:ListItem Value="70">70%</asp:ListItem>
                  <asp:ListItem Value="80">80%</asp:ListItem>
                  <asp:ListItem Value="90">90%</asp:ListItem>
                  <asp:ListItem Value="100">100%</asp:ListItem>
              </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="prospect_row" runat="server" Visible="false">
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="100">
                            <asp:Label runat="server" ID="type_original_label" Text="Type:"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            <asp:DropDownList ID="notes_opp" runat="server" Width="120">
                            </asp:DropDownList>
                            <asp:TextBox ID="cat_name" runat="server" Width="250" Visible="false"></asp:TextBox>&nbsp;
              <asp:LinkButton ID="visible_all" runat="server" Font-Size="Smaller" Font-Italic="false"
                  CausesValidation="false">Add Type</asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                      ID="cat_insert" runat="server" Visible="false" Font-Size="Smaller" Font-Italic="false"
                      CausesValidation="true" ValidationGroup="categoryName">Insert</asp:LinkButton>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="cat_name"
                                ErrorMessage="<br />*Do not leave category name blank." Display="dynamic" ValidationGroup="categoryName"></asp:RequiredFieldValidator>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="prospect_status_row" runat="server" Visible="false">
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="prospect_status_text">
                            Status:
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            <asp:RadioButtonList ID="opp_status" CssClass="oppRadio" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Text="Open" Value="A" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Closed" Value="I"></asp:ListItem>
                            </asp:RadioButtonList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="prospect_priority_row" runat="server" Visible="false">
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="TableCell1">
                            <asp:Label ID="action_label" runat="server" Text="Action Taken/Priority:"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            <asp:DropDownList ID="priorityID" CssClass="oppRadio" runat="server">
                            </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="prospect_source_row" runat="server" Visible="false">
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="TableCell2">
                            <asp:Label ID="source_label" runat="server" Text="Source:"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            <asp:DropDownList ID="source_dropdown" CssClass="oppRadio" runat="server">
                            </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                      <asp:TableRow ID="referral_row" runat="server" Visible="false">
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="TableCell3">
                            <asp:Label ID="referral_label" runat="server" Text="Referrer:"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            <asp:DropDownList ID="referral_drop" CssClass="oppRadio" runat="server">
                            </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <p align="right">
                    <asp:Panel runat="server" ID="notesdate" CssClass="float_left" Width="400">
                        <table width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:RadioButtonList ID="curprev" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="P" Text="Previous Date"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="date_time_label" Text="Date/Time:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Label>
                                    <asp:Label runat="server" ID="current" Visible="false"></asp:Label>
                                    <asp:TextBox ID="note_date" runat="server" Width="100" Style="margin-left: 2px;"></asp:TextBox><asp:Image
                                        runat="server" ID="note_date_image" ImageUrl="../images/final.jpg" />
                                    <asp:DropDownList ID="time" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Enabled="false" runat="server"
                                        ControlToValidate="note_date" ErrorMessage="Date is Required" ValidationGroup="Notes_Edit"
                                        Text="" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="note_date"
                                        ErrorMessage="Enter a valid date" Operator="DataTypeCheck" Type="Date" ValidationGroup="Notes_Edit"
                                        Text="" Display="None" />
                                </td>
                            </tr>
                        </table>
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="note_date"
                            PopupButtonID="note_date_image" OnClientDateSelectionChanged="checkDate" Format="MM/dd/yyyy" />
                        <br />
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="You cannot select a day later than today"
                            OnServerValidate="checkDate" ControlToValidate="note_date" ValidationGroup="Notes_Edit"
                            Text="" Display="None"></asp:CustomValidator>
                    </asp:Panel>
                </p>
               
                <asp:Panel runat="server" ID="authorization_panel" Visible="false">
                    <h3 align="right">USER AUTHORIZATION</h3>
                    <asp:CheckBox ID="authorize_check" runat="server" />&nbsp; I understand that by
          checking this box that the data reported regarding this aircraft estimate will be
          sent to JETNET for use and display within JETNET's products as an "Estimated Probable
          Transaction Price". JETNET WILL NOT display the source data reported as part of
          this submittal process unless required to do so by court order or otherwise by law.
          <a href='#' onclick="javascript:window.open('http://www.jetnet.com/help/documents/661.pdf ','_blank','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');">Learn More</a>.
                </asp:Panel>
                <br clear="all" />
                <a href="javascript: window.opener.location.href = window.opener.location.href; self.close();"
                    class="button float_left">Close</a>&nbsp;&nbsp;
        <asp:LinkButton ID="removeNoteLB" OnClientClick="return confirm('Are you sure you want to Remove this Note?');"
            runat="server" CssClass="button float_left mobile_float_left" Visible="False" CausesValidation="False">Remove</asp:LinkButton>
                <asp:LinkButton ID="add_noteLB" runat="server" CssClass="button float_right mobile_float_right"
                    Text="Save" CausesValidation="true" ValidationGroup="Notes_Edit" OnClick="add_note_Click" />
                <asp:Button ID="MPM_Prospect_edit" runat="server" Text="Save" Visible="false" OnClick="Add_Note_MPM" />
                <hr style="margin: 0px;" />
                <!--Auto Note-->
                <asp:Label runat="server" ID="next_previous_note_link"></asp:Label>
                <asp:Panel runat="server" ID="add_note_automatically" Visible="false" CssClass="follow_up_action_item">
                    <asp:CheckBox runat="server" ID="add_note_automatically_checkbox" Text="Add this as a Note?" />
                </asp:Panel>
                <!--Additional Prospect Area-->
                <asp:Panel runat="server" ID="add_prospect_automatically" Visible="false" CssClass="follow_up_prospect">
                    <asp:CheckBox runat="server" ID="add_prospect_automatically_checkbox" Text="Add this company as a Prospect for this Aircraft?" />
                </asp:Panel>
                <!--Upload area-->
                <asp:Panel ID="upload_area" runat="server" Visible="false">
                    <asp:Label ID="existing_docs" runat="server"></asp:Label><br clear="all" />
                    <p id="upload-area">
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                    </p>
                </asp:Panel> 
                <asp:Panel runat="server" CssClass="follow_up_action_item" ID="action_item_lbl">
                    <asp:CheckBox ID="follow_up" runat="server" AutoPostBack="true" Text="Record a Follow-Up Action Item" />
                    <asp:Panel runat="server" ID="action_item_vis" Visible="false">
                        <asp:Table runat="server" Width="100%">
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                                    <asp:Label runat="server">Date/Time:
                                        <asp:TextBox ID="action_item_date" runat="server" Width="100" Style="margin-left: 2px;"></asp:TextBox><asp:Image
                                            runat="server" ID="Image1" ImageUrl="../images/final.jpg" />
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                    <asp:DropDownList ID="action_item_time" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Enabled="true" runat="server"
                                        ControlToValidate="action_item_date" ErrorMessage="Follow-Up Action Item Date is Required"
                                        ValidationGroup="Notes_Edit" Text="" Display="None"></asp:RequiredFieldValidator>

                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="action_item_date"
                                        ErrorMessage="Enter a valid Follow-Up Action Item Date" Operator="DataTypeCheck"
                                        Type="Date" ValidationGroup="Notes_Edit" Text="" Display="None" />
                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="action_item_date"
                                        PopupButtonID="note_date_image" OnClientDateSelectionChanged="checkDate_Future"
                                        Format="MM/dd/yyyy" />
                                    <br />
                                    <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="You must select a future date for Follow-Up Action Item"
                                        OnServerValidate="checkDate_Future" ControlToValidate="action_item_date" ValidationGroup="Notes_Edit"
                                        Text="" Display="None" Enabled="true"></asp:CustomValidator></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="top" ColumnSpan="2">
                                    <asp:TextBox ID="action_item_subject" runat="server" TextMode="MultiLine" Width="440"
                                        Height="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Enabled="false" runat="server"
                                        ControlToValidate="action_item_subject" ErrorMessage="Action Item Text is Required"
                                        ValidationGroup="Notes_Edit" Text="" Display="None"></asp:RequiredFieldValidator>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="email_action">
                                <asp:TableCell ColumnSpan="2" HorizontalAlign="Left" VerticalAlign="Top">
                                    <asp:CheckBox ID="email_pertaining" runat="server" Checked="false" Text="Email Action Item to Assigned Staff"
                                        AutoPostBack="true" />
                                </asp:TableCell>

                            </asp:TableRow>
                            <asp:TableRow ID="cc_row" runat="server" Visible="false">
                                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            CC:
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                    <asp:TextBox ID="action_cc" runat="server" Width="120" CausesValidation="true"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel runat="server" Visible="false" ID="next_action_panel">
                    <br /><br /><br />
                <asp:Label runat="server" ID="next_action_date_label" Visible="false"></asp:Label>
                    </asp:Panel>
            </div>
            <asp:Panel runat="server" ID="valuation_panel" Visible="false">
                <asp:Label ID="current_market_label" runat="server"></asp:Label>
                <asp:Label ID="current_sold_label" runat="server"></asp:Label>
                <asp:Label ID="field_label" runat="server"></asp:Label>
            </asp:Panel>
        </div>
    </div>
</div>

<script type="text/javascript">
    FitPic();
</script>

</asp:Panel> 