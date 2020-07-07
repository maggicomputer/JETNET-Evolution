<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Email.ascx.vb" Inherits="crmWebClient.Email" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc2" %>
<asp:Label ID="mobile_style" runat="server" Text="" Visible="false"> <link href="common/style.css" rel="stylesheet" type="text/css" /></asp:Label>
<asp:Label ID="resize_function" runat="server"> 
<script type="text/javascript">
     function FitPic() { 
       window.resizeTo(920,1006); 
       self.focus(); 
     }; 
</script>
</asp:Label>

<table width="100%" cellpadding="5" cellspacing="0">
    <tr>
        <td align="left" valign="top">
            <tr>
                <td align="left" valign="top" bgcolor="#f5fafd" width="320">
                    <h3 align="right">
                        Aircraft Information:</h3>
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
                                <asp:Panel runat="server" ID="ac_search" Visible="false">
                                    <table width="90%" align="center" cellpadding="3" cellspacing="0" class="notes_pnl"
                                        border="0">
                                        <tr>
                                            <td align="left" valign="top">
                                            </td>
                                            <td align="right" valign="top">
                                                <b>Search Parameters</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                                <asp:Label ID="ac_search_text" runat="server">Ser #/Reg #<Br />Make/Model:</asp:Label>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox runat="server" ID="serial" Width="130" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                            </td>
                                            <td align="right" valign="top">
                                                <asp:ImageButton ID="ac_search_button" runat="server" ImageUrl="~/images/search_button.jpg" />
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
                    <!----Company----->
                    <h3 align="right">
                        Company Information:</h3>
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
                                <asp:Panel runat="server" ID="company_search" Visible="false">
                                    <table width="95%" align="center" cellpadding="3" cellspacing="0" class="notes_pnl"
                                        border="0">
                                        <tr>
                                            <td align="left" valign="top">
                                            </td>
                                            <td align="right" valign="top">
                                                <b>Search Parameters</b>
                                            </td>
                                        </tr>
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
                                                <asp:ImageButton ID="company_search_button" runat="server" ImageUrl="~/images/search_button.jpg" />
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
                    <!---Contact--->
                    <h3 align="right">
                        Contact Information:</h3>
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
                                <asp:Panel runat="server" ID="contact_search" Visible="false">
                                    <table width="90%" align="center" cellpadding="3" cellspacing="0" class="notes_pnl"
                                        border="0">
                                        <tr>
                                            <td align="left" valign="top">
                                            </td>
                                            <td align="right" valign="top">
                                                <b>Search Parameters</b>
                                            </td>
                                        </tr>
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
                                                <asp:ImageButton ID="contact_search_button" runat="server" ImageUrl="~/images/search_button.jpg" />
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
                    <!---Display Portion Boxes-->
                    <br />
                    <br />
                </td>
                <asp:Label ID="regular_view" runat="server" Visible="true">
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td align="left" valign="top" width="400">
                </asp:Label>
                <asp:Label ID="mobile_view" runat="server" Visible="false">
        </td>
    </tr>
    <tr>
        <td>
            </asp:Label>
            <asp:TextBox ID="jetnet_ac" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="action" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="client_ac" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="jetnet_comp" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="client_comp" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="jetnet_contact" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="client_contact" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="jetnet_mod" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:TextBox ID="client_mod" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Email_Edit"
                DisplayMode="BulletList" EnableClientScript="true" HeaderText="There are problems with the following fields:" />
            <ul class="notes_list_edit">
                <li>
                    <asp:Label ID="return_error" ForeColor="Red" Font-Bold="true" runat="server"></asp:Label>
                    <table width="100%" cellpadding="1" cellspacing="0">
                        <tr>
                            <td align="left" valign="top">
                                To:
                            </td>
                            <td align="left" valign="top">
                                <asp:Label ID="email_to" runat="server" CssClass="display_disable"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                From:
                            </td>
                            <td align="left" valign="top">
                                <asp:Label ID="email_from" runat="server" CssClass="display_disable"></asp:Label>
                                <asp:CheckBox ID="email_from_bcc" runat="server" Text="BCC Sender?" Checked="true" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                CC:
                            </td>
                            <td align="left" valign="top">
                                <asp:RegularExpressionValidator ControlToValidate="email_cc" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please Enter a valid Email Address<br />"
                                    ValidationGroup="Email_Edit" Text="" Display="None"></asp:RegularExpressionValidator>
                                <asp:TextBox ID="email_cc" runat="server" Width="250"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                Subject:
                            </td>
                            <td align="left" valign="top">
                                <asp:TextBox ID="email_subject" runat="server" Width="250"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                Category:
                            </td>
                            <td align="left" valign="top">
                                <asp:DropDownList ID="notes_cat" runat="server" Width="255">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow runat="server" ID="file_upload_area" Visible="false">
                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Attachment:</asp:TableCell>
                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                <asp:FileUpload ID="FileUpload1" runat="server" Width="360" size="39" /><br />
                                <asp:CheckBox ID="store_document" runat="server" Enabled="true" Text="Store Document with Email Record?" />
                                <p class="info_box">
                                    Please keep file sizes below 5MB to ensure a speedy delivery.<br />
                                    <br />
                                </p>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server" ID="existing" Visible="false">
                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Attachment:</asp:TableCell>
                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                <asp:Label ID="existing_docs" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2" HorizontalAlign="Left" VerticalAlign="Top">
                                Email Body:<br />
                                <cc2:Editor ID="body" runat="server" Height="280px" Width="435" AutoFocus="true" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2" HorizontalAlign="Left" VerticalAlign="Top">
                                Note:<br />
                                <asp:TextBox ID="notes_edit" runat="server" TextMode="MultiLine" Width="430" Height="200"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow1">
                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                                <asp:Panel CssClass="float_right" runat="server">
                                    <asp:Label runat="server" ID="mobile_close">
                                <a href="javascript: self.close ()">
                                    <img src="images/cancel.gif" alt="Cancel" border="0" /></a>
                                    </asp:Label>&nbsp;&nbsp;<asp:ImageButton ID="add_note" runat="server" ImageUrl="~/images/add_new.jpg"
                                        CausesValidation="true" ValidationGroup="Email_Edit" />&nbsp;&nbsp;<asp:ImageButton
                                            ID="remove_note" runat="server" ImageUrl="~/images/remove.gif" Visible="false"
                                            CausesValidation="false" OnClientClick="return confirm('Are you sure you want to Remove this Email?');" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                </asp:Panel>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </li>
            </ul>
        </td>
    </tr>
</table>

<script type="text/javascript">
        FitPic();
</script>

