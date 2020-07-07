<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ActionItems.ascx.vb"
    Inherits="crmWebClient.ActionItems" %>
<style>
html{
   min-width:900px; 
   width: auto !important;  
   width:900px;           
   overflow:auto;
}
</style>
<script type="text/javascript">
    function checkDatePrevJava(sender, args) {
    var now = new Date();
    var dateString = now.getMonth() + "/" + now.getDate() + "/" + now.getFullYear() 
    
    
    var now2 = sender._selectedDate;
    var dateString2 = now2.getMonth() + "/" + now2.getDate() + "/" + now2.getFullYear() 
   // alert(dateString);
   // alert(dateString2);
    
        if (dateString2 < dateString) {
            alert("You cannot select a day earlier than today for a planned Action Item.");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
        }
    }
</script>

<asp:Label ID="resize_function" runat="server">
<script type="text/javascript">
     function FitPic() { 
       window.resizeTo(930,616); 
       self.focus(); 
     };  
</script>
</asp:Label>
<asp:Panel runat="server" ID="edit_table">
  <div class="row remove_margin">
          <div class="six columns remove_margin">
     <div class="Box"> <div class="subHeader">
                    Aircraft Information:</div>
                <table width="100%" cellpadding="3" cellspacing="0">
                    <tr>
                        <td align="left" valign="top">
                            <asp:CheckBox ID="aircraft_related" runat="server" Text="Aircraft Related to Company"
                                Checked="true" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" colspan="3">
                            <asp:LinkButton ID="AC_Search_Vis" runat="server" Visible="false">Click for AC Search</asp:LinkButton>
                            <asp:Panel runat="server" ID="ac_search" Visible="false"  cssclass="notes_pnl padding"><div class="subHeader">Search Parameters</div><br />
                                <table width="100%" align="center" cellpadding="3" cellspacing="0"
                                    border="0">
                                    <tr>
                                        <td align="left" valign="top" width="130">
                                            <asp:Label ID="ac_search_text" runat="server">Ser #/Reg #/Make/Model:</asp:Label>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="serial" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                        </td>
                                        <td align="right" valign="top">
                                            <asp:linkbutton ID="ac_search_buttonLB" runat="server"  CssClass="button float_right mobile_float_right" Text="Search"/>
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
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:Label ID="aircraft_info" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                </div>
                <!----Company----->
           <div class="Box"> <div class="subHeader">
                    Company Information:</div>
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
                            <asp:Panel runat="server" ID="company_search" Visible="false"  cssclass="notes_pnl padding"><div class="subHeader">Search Parameters</div><br />
                                <table width="100%" align="center" cellpadding="3" cellspacing="0" 
                                    border="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            Company Name:
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="Name" Width="164" />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td align="left" valign="top">
                                            First/Last Name:
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="first_name" Width="78" /><asp:TextBox runat="server" ID="last_name" Width="79" />
                                        </td>
                                    </tr>
                                      <tr>
                                        <td align="left" valign="top">
                                           Email Address:
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="email_address" Width="164" />
                                        </td>
                                    </tr>
                                      <tr>
                                        <td align="left" valign="top">
                                           Phone Number:
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="phone_number" Width="164" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                        </td>
                                        <td align="right" valign="top">
                                          <asp:linkbutton ID="company_search_buttonLB" runat="server"  CssClass="button float_right mobile_float_right" Text="Search"/>
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
                    <div class="Box"> <div class="subHeader">
                    Contact Information:</div>
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
                            <asp:Panel runat="server" ID="contact_search" Visible="false"  cssclass="notes_pnl padding"><div class="subHeader">Search Parameters</div><br />
                                <table width="100%" align="center" cellpadding="3" cellspacing="0" 
                                    border="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            First Name:
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="first" Width="110" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            Last Name:
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="last" Width="110" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                        </td>
                                        <td align="right" valign="top">
                                           <asp:linkbutton ID="contact_search_buttonLB" runat="server"  CssClass="button float_right mobile_float_right" Text="Search"/>
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
                <!---Display Portion Boxes-->
  </div>
          <div class="six columns remove_margin">
                <asp:TextBox ID="jetnet_ac" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="action" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="client_ac" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="jetnet_comp" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="client_comp" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="jetnet_contact" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="client_contact" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="jetnet_mod" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="client_mod" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Action_Edit"
                    DisplayMode="BulletList" EnableClientScript="true" HeaderText="There are problems with the following fields:" />
                 <div class="Box"> <div class="subHeader">Record Information</div><br />
                        <asp:Panel runat="server" ID="action_view" Visible="true">
                            <table width="100%" cellpadding="1" cellspacing="0" class="formatTable blue">
                                <tr>
                                    <td align="left" valign="top">
                                        Date:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:Label runat="server" ID="current" Visible="false"></asp:Label>
                                        <asp:TextBox ID="dated" runat="server" Width="70" CausesValidation="true"></asp:TextBox>
                                        <asp:Image runat="server" ID="cal_image" ImageUrl="~/images/final.jpg" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="dated"
                                            Format="d" PopupButtonID="cal_image" />
                                        &nbsp;&nbsp;Time:&nbsp;&nbsp;
                                        <asp:DropDownList ID="time" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dated"
                                            ErrorMessage="Date is Required" ValidationGroup="Action_Edit" Text="" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="You cannot select a day earlier than today"
                                            OnServerValidate="checkDatePrev" ControlToValidate="dated" ValidationGroup="Action_Edit"
                                            Text="" Display="None"></asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="dated"
                                            ErrorMessage="Enter a valid date" Operator="DataTypeCheck" Type="Date" ValidationGroup="Action_Edit"
                                            Text="" Display="None" />
                                    </td>
                                </tr>
                                <asp:Panel runat="server" ID="priority_action" Visible="true">
                                    <tr>
                                        <td align="left" valign="top">
                                            Priority:
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="priority" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <tr>
                                    <td align="left" valign="top">
                                        Status:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:Label ID="action_to_note_warning" runat="server" Text="" ForeColor="Red"></asp:Label><br />
                                        <asp:RadioButtonList ID="statused" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                            AutoPostBack="true">
                                            <asp:ListItem Text="Active" Value="P" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Completed" Value="C"></asp:ListItem>
                                            <asp:ListItem Text="Dismissed" Value="D"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <div>
                            <asp:Table runat="server" CssClass="formatTable blue">
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="2" HorizontalAlign="Left" VerticalAlign="Top">
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Enabled="false" runat="server"
                                                    ControlToValidate="notes_edit" ErrorMessage="Text is Required" ValidationGroup="Action_Edit"
                                                    Text="" Display="None"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="notes_edit" runat="server" TextMode="MultiLine" Width="440" Height="200"></asp:TextBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow ID="email_action">
                                    <asp:TableCell ColumnSpan="2" HorizontalAlign="Left" VerticalAlign="Top">
                                        <asp:CheckBox ID="email_pertaining" runat="server" Checked="false" Text="Email Action Item to Assigned Staff"
                                            AutoPostBack="true" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow ID="cc_row" Visible="false">
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            CC:
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                        <asp:TextBox ID="action_cc" runat="server" Width="120" CausesValidation="true"></asp:TextBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            Note Entered For:
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                        <asp:DropDownList ID="pertaining_to" runat="server" Width="120">
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow ID="notescatpanel">
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            Category:
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                        <asp:DropDownList ID="notes_cat" runat="server" Width="120">
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow ID="TableRow1">
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                                        <asp:Panel CssClass="float_right">
                                            <a href="javascript: window.opener.location.href = window.opener.location.href; self.close();"
                        class="button float_left">Close</a>&nbsp;&nbsp;<asp:LinkButton ID="add_noteLB" runat="server"  CssClass="button float_right mobile_float_right"
                                                CausesValidation="true" ValidationGroup="Action_Edit" Text="Save" />&nbsp;&nbsp;
                                                      <asp:LinkButton ID="removeNoteLB" OnClientClick="return confirm('Are you sure you want to Remove this Note?');"  runat="server" CssClass="button float_left mobile_float_left"
                        Visible="False" CausesValidation="False">Remove</asp:LinkButton>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                        </asp:Panel>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <div align="right" style="padding: 3px;">
                                <hr />
                                <!--Upload area-->
                                <asp:Panel ID="upload_area" runat="server" Visible="false">
                                    <asp:Label ID="existing_docs" runat="server"></asp:Label><br clear="all" />
                                    <p id="upload-area">
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                    </p>
                                        <asp:Button ID="btnSubmit" runat="server" autopostback="false" Text="Upload Now" />
                                </asp:Panel>
                              </div>
                        </div>
                    </div>

</div></div>

    <script type="text/javascript">
        FitPic();
    </script>

</asp:Panel>
