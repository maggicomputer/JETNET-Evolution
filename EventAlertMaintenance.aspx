<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EventAlertMaintenance.aspx.vb" Inherits="crmWebClient.EventAlertMaintenance" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="EvoStyles/stylesheets/tableThemes.css" type="text/css" rel="stylesheet" />
    <style>
        .subHeader span {
            display: inline-block;
            border-bottom: 1px solid #eee;
        }

        .aircraftSpec {
            margin-top: -5px;
            margin-bottom: -10px !important;
            padding-top: 10px;
            padding-bottom: 10px;
            font-size: 14px !important;
        }

        .radioButtonList {
            font-size: 14px !important;
        }

            .radioButtonList.formatTable.blue tr {
                border-bottom: 0px;
            }

        .formatTable.blue {
            font-size: 14px;
        }

        .columns.Box {
            margin-bottom: 10px !important;
            margin-right: 2% !important;
            margin-left: 0% !important
        }

        .reoccurrenceBox {
            border-right: 1px solid #c3c3c3;
        }

        td {
            vertical-align: top;
        }

        .rightSideBox {
            font-size: 14px;
            padding-top: 20px;
            padding-left: 20px;
            border-top: 1px solid #c3c3c3
        }

        .spacingRow {
            width: 100%;
        }

            .spacingRow .grid-item {
                margin-right: 1.5% !important;
                margin-left: 0px !important;
            }
    </style>
    <script>
        function toggleRadio() {
            $('#<%= live_toggle.ClientID %>').hide();
            $('#<%= daily_toggle.ClientID %>').hide();
            $('#<%= weekly_toggle.ClientID %>').hide();
            switch ($('#<%= reoccurence_radio.ClientID %> input:checked').val()) {
                case 'Daily':
                    $('#<%= daily_toggle.ClientID %>').show();
                    break;
                case 'Weekly':
                    $('#<%= weekly_toggle.ClientID %>').show();
                    break;
                default:
                    $('#<%= live_toggle.ClientID %>').show();
                    break;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="valueSpec viewValueExport Simplistic aircraftSpec plain">
          <asp:Table ID="buttonsTable" CellPadding="3" CellSpacing="0" Width="100%" CssClass="DetailsBrowseTable"
            runat="server">
            <asp:TableRow>
                <asp:TableCell runat="server" HorizontalAlign="right" VerticalAlign="middle" Style="padding-right: 4px;"
                    Width="23%">
                    <div class="backgroundShade">
                        <span class="float_right"><a href="#" onclick="javascript:window.close();" class="float_right seperator"><img src="/images/x.svg" alt="Close" /></a></span>
                        <asp:LinkButton ID="save_button" runat="server" CssClass="float_left" Visible="true"><img src="/images/save.svg" /></asp:LinkButton>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>

        <div class="row remove_margin spacingRow">
            <div class="four columns grid-item" style="margin-left:1% !important">
                <div class="Box">
                    <div class="subHeader emphasisColor">EVENT ALERT IDENTIFICATION:<span></span></div>
                    <p>Please name and describe the event alert that you are scheduling to run.</p>
                    <table class="formatTable blue" width="100%">
                        <tr>
                            <td width="90">Name:
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Folder_Edit"
                                    ErrorMessage="A Folder Name is Required" ControlToValidate="cfolder_name" Text="*"
                                    Display="None">
                                </asp:RequiredFieldValidator>
                                <asp:TextBox ID="cfolder_name" runat="server" Width="100%" MaxLength="250">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Description:</td>
                            <td>
                                <asp:TextBox ID="cfolder_description" runat="server" TextMode="MultiLine" Rows="5"
                                    Width="100%">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Selections:</td>
                            <td>
                                <asp:Literal runat="server" ID="searchFilterText"></asp:Literal></td>
                        </tr>
                    </table>
                    <asp:TextBox ID="cfolder_data" runat="server" TextMode="MultiLine" Rows="5" Width="100%"
                        CssClass="display_none" />
                    <asp:TextBox ID="cfolder_type_of_folder" runat="server" Width="100%" CssClass="display_none">
                    </asp:TextBox>
                    <asp:TextBox ID="cfolder_method" runat="server" Width="100%" CssClass="display_none">
                    </asp:TextBox>
                    <asp:TextBox ID="cfolder_id" runat="server" Width="100%" CssClass="display_none">
                    </asp:TextBox>
                </div>
            </div>
            <div class="eight columns grid-item">
                <div class="row">
                    <div class="Box seven columns">
                        <div class="subHeader emphasisColor">Event Schedule:<span></span></div>
                        <p>Use the area below to schedule the automatic delivery (email) of this event report based on the reoccurring timeframe identified.</p>
                        <table width="100%">
                            <tr>
                                <td width="85px" class="reoccurrenceBox">
                                    <div>
                                        <asp:RadioButtonList runat="server" ID="reoccurence_radio" AutoPostBack="true" RepeatDirection="vertical" Width="80px" CssClass="float_left radioButtonList formatTable blue" CellPadding="5" Font-Size="small">
                                            <asp:ListItem Value="LIVE">Live</asp:ListItem>
                                            <asp:ListItem Value="DAILY">Daily</asp:ListItem>
                                            <asp:ListItem Value="WEEKLY">Weekly</asp:ListItem>
                                            <asp:ListItem Value="MONTHLY">Monthly</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </td>
                                <td class="rightSideBox">

                                    <asp:Panel runat="server" ID="live_toggle" Visible="false">
                                        Next run:
                        <asp:Label runat="server" ID="live_toggle_next_run"></asp:Label><br />
                                        <br />
                                        <p class="emphasis_text">Live alerts for your selections will be sent approximately every 15 minutes as changes occur by JETNET research.</p>
                                    </asp:Panel>

                                    <asp:Panel runat="server" ID="daily_toggle" Visible="false">
                                        Hour to run:
                            <asp:DropDownList runat="server" ID="daily_time_hour" AutoPostBack="true">
                                <asp:ListItem Value="0">12 AM</asp:ListItem>
                                <asp:ListItem Value="1">1 AM</asp:ListItem>
                                <asp:ListItem Value="2">2 AM</asp:ListItem>
                                <asp:ListItem Value="3">3 AM</asp:ListItem>
                                <asp:ListItem Value="4">4 AM</asp:ListItem>
                                <asp:ListItem Value="5">5 AM</asp:ListItem>
                                <asp:ListItem Value="6">6 AM</asp:ListItem>
                                <asp:ListItem Value="7">7 AM</asp:ListItem>
                                <asp:ListItem Value="8">8 AM</asp:ListItem>
                                <asp:ListItem Value="9">9 AM</asp:ListItem>
                                <asp:ListItem Value="10">10 AM</asp:ListItem>
                                <asp:ListItem Value="11">11 AM</asp:ListItem>
                                <asp:ListItem Value="12">12 PM</asp:ListItem>
                                <asp:ListItem Value="13">1 PM</asp:ListItem>
                                <asp:ListItem Value="14">2 PM</asp:ListItem>
                                <asp:ListItem Value="15">3 PM</asp:ListItem>
                                <asp:ListItem Value="16">4 PM</asp:ListItem>
                                <asp:ListItem Value="17">5 PM</asp:ListItem>
                                <asp:ListItem Value="18">6 PM</asp:ListItem>
                                <asp:ListItem Value="19">7 PM</asp:ListItem>
                                <asp:ListItem Value="20">8 PM</asp:ListItem>
                                <asp:ListItem Value="21">9 PM</asp:ListItem>
                                <asp:ListItem Value="22">10 PM</asp:ListItem>
                                <asp:ListItem Value="23">11 PM</asp:ListItem>
                            </asp:DropDownList><br />
                                        <br />
                                        Next run:
                       
                            <asp:Label runat="server" ID="daily_next_run"></asp:Label>
                                    </asp:Panel>

                                    <asp:Panel runat="server" ID="weekly_toggle" Visible="false">
                                        <div class="float_left">
                                            Hour to Run:
                                <asp:DropDownList runat="server" ID="weekly_time_hour" AutoPostBack="true">
                                    <asp:ListItem Value="0">12 AM</asp:ListItem>
                                    <asp:ListItem Value="1">1 AM</asp:ListItem>
                                    <asp:ListItem Value="2">2 AM</asp:ListItem>
                                    <asp:ListItem Value="3">3 AM</asp:ListItem>
                                    <asp:ListItem Value="4">4 AM</asp:ListItem>
                                    <asp:ListItem Value="5">5 AM</asp:ListItem>
                                    <asp:ListItem Value="6">6 AM</asp:ListItem>
                                    <asp:ListItem Value="7">7 AM</asp:ListItem>
                                    <asp:ListItem Value="8">8 AM</asp:ListItem>
                                    <asp:ListItem Value="9">9 AM</asp:ListItem>
                                    <asp:ListItem Value="10">10 AM</asp:ListItem>
                                    <asp:ListItem Value="11">11 AM</asp:ListItem>
                                    <asp:ListItem Value="12">12 PM</asp:ListItem>
                                    <asp:ListItem Value="13">1 PM</asp:ListItem>
                                    <asp:ListItem Value="14">2 PM</asp:ListItem>
                                    <asp:ListItem Value="15">3 PM</asp:ListItem>
                                    <asp:ListItem Value="16">4 PM</asp:ListItem>
                                    <asp:ListItem Value="17">5 PM</asp:ListItem>
                                    <asp:ListItem Value="18">6 PM</asp:ListItem>
                                    <asp:ListItem Value="19">7 PM</asp:ListItem>
                                    <asp:ListItem Value="20">8 PM</asp:ListItem>
                                    <asp:ListItem Value="21">9 PM</asp:ListItem>
                                    <asp:ListItem Value="22">10 PM</asp:ListItem>
                                    <asp:ListItem Value="23">11 PM</asp:ListItem>
                                </asp:DropDownList>
                                        </div>
                                        <asp:RadioButtonList runat="server" ID="weekly_time_day" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="3" CssClass="float_right formatTable blue">
                                            <asp:ListItem Value="0">Sunday</asp:ListItem>
                                            <asp:ListItem Value="1">Monday</asp:ListItem>
                                            <asp:ListItem Value="2">Tuesday</asp:ListItem>
                                            <asp:ListItem Value="3">Wednesday</asp:ListItem>
                                            <asp:ListItem Value="4">Thursday</asp:ListItem>
                                            <asp:ListItem Value="5">Friday</asp:ListItem>
                                            <asp:ListItem Value="6">Saturday</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <div class="float_left clear_left">
                                            <br />
                                            Next run:
                        <asp:Label runat="server" ID="weekly_next_run"></asp:Label>
                                        </div>

                                    </asp:Panel>
                                    <asp:Panel ID="monthly_toggle" Visible="false" runat="server">
                                        <table cellpadding="5" cellspacing="2">
                                            <tr>
                                                <td width="190">
                                                    <asp:RadioButton runat="server" ID="monthly_every_month" Text="Every Month" GroupName="monthly" Checked="true" /></td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="monthly_day_of_month">
                                                        <asp:ListItem Value="1">1st</asp:ListItem>
                                                        <asp:ListItem Value="2">2nd</asp:ListItem>
                                                        <asp:ListItem Value="3">3rd</asp:ListItem>
                                                        <asp:ListItem Value="4">4th</asp:ListItem>
                                                        <asp:ListItem Value="5">5th</asp:ListItem>
                                                        <asp:ListItem Value="6">6th</asp:ListItem>
                                                        <asp:ListItem Value="7">7th</asp:ListItem>
                                                        <asp:ListItem Value="8">8th</asp:ListItem>
                                                        <asp:ListItem Value="9">9th</asp:ListItem>
                                                        <asp:ListItem Value="10">10th</asp:ListItem>
                                                        <asp:ListItem Value="11">11th</asp:ListItem>
                                                        <asp:ListItem Value="12">12th</asp:ListItem>
                                                        <asp:ListItem Value="13">13th</asp:ListItem>
                                                        <asp:ListItem Value="14">14th</asp:ListItem>
                                                        <asp:ListItem Value="15">15th</asp:ListItem>
                                                        <asp:ListItem Value="16">16th</asp:ListItem>
                                                        <asp:ListItem Value="17">17th</asp:ListItem>
                                                        <asp:ListItem Value="18">18th</asp:ListItem>
                                                        <asp:ListItem Value="19">19th</asp:ListItem>
                                                        <asp:ListItem Value="20">20th</asp:ListItem>
                                                        <asp:ListItem Value="21">21th</asp:ListItem>
                                                        <asp:ListItem Value="22">22st</asp:ListItem>
                                                        <asp:ListItem Value="23">23rd</asp:ListItem>
                                                        <asp:ListItem Value="24">24th</asp:ListItem>
                                                        <asp:ListItem Value="25">25th</asp:ListItem>
                                                        <asp:ListItem Value="26">26th</asp:ListItem>
                                                        <asp:ListItem Value="27">27th</asp:ListItem>
                                                        <asp:ListItem Value="28">28th</asp:ListItem>
                                                    </asp:DropDownList>
                                                    Day of Month
                                                </td>
                                                <td>Hour to Run:
                                                  <asp:DropDownList runat="server" ID="monthly_time_hour" AutoPostBack="true">
                                                      <asp:ListItem Value="0">12 AM</asp:ListItem>
                                                      <asp:ListItem Value="1">1 AM</asp:ListItem>
                                                      <asp:ListItem Value="2">2 AM</asp:ListItem>
                                                      <asp:ListItem Value="3">3 AM</asp:ListItem>
                                                      <asp:ListItem Value="4">4 AM</asp:ListItem>
                                                      <asp:ListItem Value="5">5 AM</asp:ListItem>
                                                      <asp:ListItem Value="6">6 AM</asp:ListItem>
                                                      <asp:ListItem Value="7">7 AM</asp:ListItem>
                                                      <asp:ListItem Value="8">8 AM</asp:ListItem>
                                                      <asp:ListItem Value="9">9 AM</asp:ListItem>
                                                      <asp:ListItem Value="10">10 AM</asp:ListItem>
                                                      <asp:ListItem Value="11">11 AM</asp:ListItem>
                                                      <asp:ListItem Value="12">12 PM</asp:ListItem>
                                                      <asp:ListItem Value="13">1 PM</asp:ListItem>
                                                      <asp:ListItem Value="14">2 PM</asp:ListItem>
                                                      <asp:ListItem Value="15">3 PM</asp:ListItem>
                                                      <asp:ListItem Value="16">4 PM</asp:ListItem>
                                                      <asp:ListItem Value="17">5 PM</asp:ListItem>
                                                      <asp:ListItem Value="18">6 PM</asp:ListItem>
                                                      <asp:ListItem Value="19">7 PM</asp:ListItem>
                                                      <asp:ListItem Value="20">8 PM</asp:ListItem>
                                                      <asp:ListItem Value="21">9 PM</asp:ListItem>
                                                      <asp:ListItem Value="22">10 PM</asp:ListItem>
                                                      <asp:ListItem Value="23">11 PM</asp:ListItem>
                                                  </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td colspan="2">Next run:
                        <asp:Label runat="server" ID="monthly_next_run"></asp:Label></td>
                                            </tr>
                                        </table>



                                        <!--Once the user selects Monthly, then the user will identify if they want to run every month or every x months and  
                                        select the day of the month to run and the time of the day to run. -->



                                    </asp:Panel>

                                </td>
                            </tr>
                        </table>
                        <div class="div_clear"></div>
                    </div>
                    <div class="Box five columns">
                        <div class="subHeader emphasisColor">Delivery</div>
                        <p>Please add the name and email below of the receipent.</p>
                        <table class="formatTable blue" width="100%">
                            <tr>
                                <td width="70">Name:
                                </td>
                                <td>
                                    <asp:TextBox ID="cfolder_jetnet_run_reply_username" MaxLength="100" runat="server"
                                        Width="100%" ToolTip="Name that Alert is Addressed to." />
                                </td>
                            </tr>
                            <tr>
                                <td>Email:
                                </td>
                                <td>
                                    <asp:RegularExpressionValidator ControlToValidate="cfolder_jetnet_run_reply_email"
                                        ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"
                                        ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please Enter a valid Email Address<br />"
                                        Text="" ValidationGroup="Folder_Edit" Display="None"></asp:RegularExpressionValidator>
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Email is Required"
                                        OnServerValidate="checkEmail" ControlToValidate="cfolder_jetnet_run_reply_email"
                                        ValidationGroup="Folder_Edit" ValidateEmptyText="true" Text="" Display="None"></asp:CustomValidator>
                                    <asp:TextBox ID="cfolder_jetnet_run_reply_email" MaxLength="150" runat="server" Width="100%"
                                        ToolTip="Email Address Used to Send Alert." />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button runat="server" ID="saveAndSchedule" Text="Save & Schedule Alert" CssClass="float_right" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>


    </div>
</asp:Content>
