<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Documents.ascx.vb"
  Inherits="crmWebClient.Documents" %>

<script type="text/javascript">
  function FitPic() {
    window.resizeTo(920, 616);
    self.focus();
  }; 
</script>

<div class="row remove_margin">
  <div class="six columns remove_margin">
    <div class="Box">
      <div class="subHeader">
        Aircraft Model Information</div><br />
      <table width="100%" cellpadding="3" cellspacing="0">
        <tr class="noBorder">
          <td align="left" valign="top">
            <asp:CheckBox ID="aircraft_related" runat="server" Text="Uncheck for Aircraft Search"
              Checked="true" AutoPostBack="true" />
          </td>
        </tr>
        <tr class="noBorder">
          <td align="left" valign="top" colspan="3">
            <asp:LinkButton ID="AC_Search_Vis" runat="server" Visible="false" CausesValidation="false">Click for Aircraft Search</asp:LinkButton>
            <asp:Panel runat="server" ID="ac_search" Visible="false"  cssclass="notes_pnl padding"><div class="subHeader">Search Parameters</div><br />
              <table width="100%" align="center" cellpadding="3" cellspacing="0" border="0">
                <tr>
                  <td align="left" valign="top">
                    <asp:Label ID="ac_search_text" runat="server" Width="130">Ser #/Reg #/Make/Model:</asp:Label>
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
    <div class="Box">
      <!----Company----->
     <div class="subHeader">
        Company Information:</div><br />
  <table width="100%" cellpadding="3" cellspacing="0">
        <tr class="noBorder">
          <td align="left" valign="top">
            <asp:CheckBox ID="company_related" runat="server" Text="Company Related to Aircraft"
              Checked="true" AutoPostBack="true" />
          </td>
        </tr>
        <tr class="noBorder">
          <td colspan="3" align="left" valign="top">
            <asp:LinkButton ID="company_search_vis" runat="server" Visible="false" CausesValidation="false">Click for Company Search</asp:LinkButton>
            <asp:Panel runat="server" ID="company_search" Visible="false" CssClass="notes_pnl padding">
              <table width="95%" align="center" cellpadding="3" cellspacing="0"  border="0">
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
                    <asp:TextBox runat="server" ID="first_name" Width="78" /><asp:TextBox runat="server"
                      ID="last_name" Width="79" />
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
        <tr class="noBorder">
          <td align="left" valign="top">
            <asp:DropDownList ID="company_name" runat="server" Width="350" AutoPostBack="true">
            </asp:DropDownList>
          </td>
        </tr>
        <tr class="noBorder">
          <td align="left" valign="top">
            <asp:Label ID="company_info" runat="server" Text=""></asp:Label>
          </td>
        </tr>
      </table>
    </div>
    <div class="Box">
      <!---Contact--->
     <div class="subHeader">
        Contact Information:</div><br />
  <table width="100%" cellpadding="3" cellspacing="0" class="formatTable blue">
        <tr class="noBorder">
          <td align="left" valign="top">
            <asp:CheckBox ID="contact_related" Visible="false" runat="server" Text="Contacts Related to Company"
              Checked="true" AutoPostBack="true" />
          </td>
        </tr>
        <tr class="noBorder">
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
                     <asp:linkbutton ID="contact_search_buttonLB" runat="server"  CssClass="button float_right mobile_float_right" Text="Search"/>
                  </td>
                </tr>
              </table>
            </asp:Panel>
          </td>
        </tr>
        <tr class="noBorder">
          <td align="left" valign="top">
            <asp:Label ID="contact_info" runat="server" Text=""></asp:Label>
          </td>
        </tr>
        <tr class="noBorder">
          <td align="left" valign="top">
            <asp:DropDownList ID="contact_name" runat="server" AutoPostBack="true" Width="350">
              <asp:ListItem Value="">PLEASE SELECT A COMPANY</asp:ListItem>
            </asp:DropDownList>
          </td>
        </tr>
      </table>
    </div>
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
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Doc_Edit"
      DisplayMode="BulletList" EnableClientScript="true" HeaderText="There are problems with the following fields:" />
   <div class="Box">
        <asp:Label ID="attention" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
        <asp:Panel runat="server" ID="action_view" Visible="false">
  <table width="100%" cellpadding="3" cellspacing="0" class="formatTable blue">
            <tr>
              <td align="left" valign="top">
                Date:
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
              <td align="left" valign="top">
                Priority:
              </td>
              <td align="left" valign="top">
                <asp:DropDownList ID="priority" runat="server">
                </asp:DropDownList>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                Status:
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
          <asp:Table ID="Table1" runat="server">
            <asp:TableRow>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Visible="false">
                                Opportunity For:
              </asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Visible="false">
                <asp:DropDownList ID="pertaining_to" runat="server" Width="120">
                </asp:DropDownList>
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="75">
                            Title: </asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="notes_title" runat="server" Width="350"></asp:TextBox>
                <asp:TextBox ID="notes_old_document_title" runat="server" Width="350" Style="display: none;"></asp:TextBox>
              </asp:TableCell></asp:TableRow>
            <asp:TableRow Visible="true" ID="remote_storage">
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
              </asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:CheckBox ID="stored_remotely" runat="server" Text="Document Stored Remotely?"
                  AutoPostBack="true"></asp:CheckBox>
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Visible="false" ID="web_url">
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                     URL:
              </asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="remote_url" runat="server" Width="350"></asp:TextBox>
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">

                                    Category:</asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:DropDownList ID="notes_cat" runat="server" Width="120">
                </asp:DropDownList>
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                 Description:
              </asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="notes_edit" runat="server" TextMode="MultiLine" Width="350" Height="140">
                </asp:TextBox>
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
                        Date/Time:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="current"
                          Visible="false"></asp:Label>
                        <asp:TextBox ID="note_date" runat="server" Width="100" Style="margin-left: 2px;"></asp:TextBox><asp:Image
                          runat="server" ID="note_date_image" ImageUrl="../images/final.jpg" />
                        <asp:DropDownList ID="time" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Enabled="false" runat="server"
                          ControlToValidate="note_date" ErrorMessage="Date is Required" ValidationGroup="Doc_Edit"
                          Text="" Display="None"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="note_date"
                          ErrorMessage="Enter a valid date" Operator="DataTypeCheck" Type="Date" ValidationGroup="Doc_Edit"
                          Text="" Display="None" />
                      </td>
                    </tr>
                  </table>
                  <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="note_date"
                    PopupButtonID="note_date_image" OnClientDateSelectionChanged="checkDate" Format="MM/dd/yyyy" />
                  <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="You cannot select a day later than today"
                    OnServerValidate="checkDate" ControlToValidate="note_date" ValidationGroup="Doc_Edit"
                    Text="" Display="None"></asp:CustomValidator>
                </asp:Panel>
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" ID="file_upload_area">
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"></asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                <asp:Label ID="existing_docs" runat="server"></asp:Label>
                <asp:CheckBox ID="file_upload_new" runat="server" Text="Upload new file?   " Visible="false"
                  AutoPostBack="true" />
                <asp:FileUpload ID="FileUpload1" runat="server" Width="360" size="42" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="FileUpload1"
                  runat="server" ErrorMessage="Please upload a file" ValidationGroup="Doc_Edit" Text=""
                  Display="None">
                </asp:RequiredFieldValidator>
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"></asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <br />
                <asp:Label runat="server" ID="mobile_close">
                               <a href="javascript: window.opener.location.href = window.opener.location.href; self.close();"
                        class="button float_left">Close</a>
                </asp:Label>&nbsp;&nbsp;  <asp:LinkButton ID="removeNoteLB" OnClientClick="return confirm('Are you sure you want to Remove this Note?');"  runat="server" CssClass="button float_left mobile_float_left"
                        Visible="False" CausesValidation="False">Remove</asp:LinkButton><asp:linkbutton ID="add_noteLB" runat="server"  CssClass="button float_right mobile_float_right" Text="Save"
                  CausesValidation="true" ValidationGroup="Doc_Edit" OnClick="add_note_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
              </asp:TableCell>
            </asp:TableRow>
          </asp:Table>
          </p>
        </div>
    </div>
  </div>
</div>

<script type="text/javascript">
  FitPic();
</script>

</asp:Panel> 