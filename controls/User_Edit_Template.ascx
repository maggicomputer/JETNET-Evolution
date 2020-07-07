<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="User_Edit_Template.ascx.vb"
  Inherits="crmWebClient.User_Edit_Template" %>
<%@ Import Namespace="crmWebClient.clsGeneral" %>
<asp:Panel ID="support_email" CssClass="edit_panel" runat="server" Visible="false"
  BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" BackColor="#F9F9FF"
  Width="101%">
  <h4 align="right">
    JETNET CRM Customer Support Center</h4>
  <p align="left" class="info_box">
    Welcome to the JETNET CRM Customer Support Center. Below is a form for submitting
    questions and issues to the CRM support team. For issues requiring immediate response
    call 315-542-6132.</p>
  <table width="547" cellpadding="3" cellspacing="0">
    <tr>
      <td align="left" valign="top">
        Name:
      </td>
      <td align="left" valign="top">
        <asp:Label ID="email_name" runat="server" Text=""></asp:Label>
        <asp:TextBox ID="email_client" runat="server" Style="display: none;"></asp:TextBox>
      </td>
    </tr>
    <tr>
      <td align="left" valign="top">
        Email:
      </td>
      <td align="left" valign="top">
        <asp:TextBox ID="email_email" runat="server" Width="200"></asp:TextBox><asp:RequiredFieldValidator
          ID="RequiredFieldValidator4" runat="server" Display="Dynamic" ControlToValidate="email_email"
          ErrorMessage="Email is Required*"></asp:RequiredFieldValidator>
      </td>
    </tr>
    <tr>
      <td align="left" valign="top">
        Phone:
      </td>
      <td align="left" valign="top">
        <asp:TextBox ID="email_phone" runat="server" Width="200"></asp:TextBox>
      </td>
    </tr>
    <tr>
      <td align="left" valign="top">
        Description:
      </td>
      <td align="left" valign="top">
        <asp:TextBox ID="email_description" runat="server" TextMode="MultiLine" Rows="7"
          Columns="55"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1"
            runat="server" Display="Dynamic" ControlToValidate="email_description" ErrorMessage="Description is Required*"></asp:RequiredFieldValidator>
      </td>
    </tr>
    <tr>
      <td align="left" valign="top">
      </td>
      <td align="right" valign="top">
        <asp:ImageButton ID="submit_email" runat="server" ImageUrl="~/images/submit_email.jpg"
          CausesValidation="true" />&nbsp;
      </td>
    </tr>
  </table>
</asp:Panel>
<asp:Panel CssClass="edit_panel" ID="user_edit" runat="server" Visible="true" BorderColor="#CCCCCC"
  BorderStyle="Solid" BorderWidth="1px" BackColor="#F9F9FF">
  <h4 align="right">
    User Data Edit</h4>
  <asp:Label ID="error_max_users" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
  <table width="100%" cellspacing="0" cellpadding="0">
    <tr>
      <td align="left" valign="top">
        <asp:DataGrid runat="server" ID="Datagrid1" GridLines="Horizontal" CellPadding="3"
          Width="366px" AllowPaging="false" PageSize="60" AllowSorting="True" AllowCustomPaging="True"
          AutoGenerateColumns="false" OnItemCommand="dispDetails" CssClass="mGrid" PagerStyle-CssClass="pgr"
          AlternatingItemStyle-CssClass="alt" ItemStyle-CssClass="item_row" ItemStyle-VerticalAlign="Top"
          HeaderStyle-CssClass="th">
          <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" ForeColor="White" />
          <AlternatingItemStyle />
          <ItemStyle BorderStyle="None" VerticalAlign="Top" />
          <HeaderStyle Wrap="False" HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
          <Columns>
            <asp:BoundColumn DataField="cliuser_id" Visible="false" />
            <asp:TemplateColumn HeaderText="User">
              <ItemTemplate>
                <asp:LinkButton CommandName="details" runat="server" ID="Details" Width="130" CausesValidation="false"><%#(DataBinder.Eval(Container.DataItem, "cliuser_first_name"))%>&nbsp;<%#(DataBinder.Eval(Container.DataItem, "cliuser_last_name"))%></asp:LinkButton></ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Login">
              <ItemTemplate>
                <%#(DataBinder.Eval(Container.DataItem, "cliuser_login"))%></ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Timezone">
              <ItemTemplate>
                <%#whatTimeZone((DataBinder.Eval(Container.DataItem, "cliuser_timezone")))%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Admin">
              <ItemTemplate>
                <%#IIf(Eval("cliuser_admin_flag") & "" = "Y", "<span class='green'>&#10004</span>", "<span class='red'>&ndash;</span>")%></ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Demo">
              <ItemTemplate>
                <%#demo_expiration(Eval("cliuser_admin_flag"), Eval("cliuser_end_date"))%></ItemTemplate>
            </asp:TemplateColumn>
          </Columns>
        </asp:DataGrid>
      </td>
      <td align="left" valign="top">
        <asp:ImageButton ID="add_new_user" ImageUrl="~/images/add_new.jpg" AlternateText="Add New User"
          ToolTip="Add New User" runat="server" ImageAlign="right" CssClass="add_new_user"
          CausesValidation="false" />
        <asp:DetailsView ID="DetailsView1" runat="server" Width="430px" BackColor="White"
          BorderColor="#a9a9a9" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" GridLines="Horizontal"
          Font-Names="Arial" Font-Size="10pt" HorizontalAlign="Right" AutoGenerateRows="false"
          EnableModelValidation="True" Visible="true" autopostback="false">
          <Fields>
            <asp:BoundField DataField="cliuser_id" Visible="True" ReadOnly="True" HeaderText="Website Reference #"
              InsertVisible="False" />
            <asp:TemplateField HeaderText="" ShowHeader="False">
              <EditItemTemplate>
                <table width="100%" cellspacing="0" cellpadding="5">
                  <tr>
                    <td align="left" valign="top">
                      First Name:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="cliuser_first_name" runat="server" Text='<%# Eval("cliuser_first_name") %>'
                        Width="100" MaxLength="15"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator5"
                          runat="server" ErrorMessage="*" ControlToValidate="cliuser_first_name"></asp:RequiredFieldValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Last Name:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="cliuser_last_name" runat="server" Text='<%# Eval("cliuser_last_name") %>'
                        Width="150" MaxLength="25"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator6"
                          runat="server" ErrorMessage="*" ControlToValidate="cliuser_last_name"></asp:RequiredFieldValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Email Address:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="cliuser_email_address" runat="server" Text='<%# Eval("cliuser_email_address") %>'
                        Width="200" MaxLength="60"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator7"
                          runat="server" ErrorMessage="*" ControlToValidate="cliuser_email_address"></asp:RequiredFieldValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="center" valign="top" colspan="2">
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="dynamic"
                        ControlToValidate="cliuser_email_address" ErrorMessage="Invalid Format for Email Address"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Login:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="cliuser_login" runat="server" Text='<%# Eval("cliuser_login") %>'
                        Width="200" MaxLength="50"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator8"
                          runat="server" ErrorMessage="*" ControlToValidate="cliuser_login"></asp:RequiredFieldValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Password:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="cliuser_password" runat="server" value='<%# Eval("cliuser_password") %>'
                        Text='<%# Eval("cliuser_password") %>' Width="150" TextMode="Password" MaxLength="25"></asp:TextBox><asp:RequiredFieldValidator
                          ID="RequiredFieldValidator9" runat="server" ErrorMessage="*" ControlToValidate="cliuser_password"></asp:RequiredFieldValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="center" valign="top" colspan="2">
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="dynamic"
                        ControlToValidate="cliuser_password" ErrorMessage="Password must be 8-20 nonblank characters."
                        ValidationExpression="[^\s]{8,20}" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Confirm Password:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="cliuser_confirm" runat="server" value='<%# Eval("cliuser_password") %>'
                        Text='<%# Eval("cliuser_password") %>' Width="150" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator
                          ID="RequiredFieldValidator10" runat="server" ErrorMessage="*" ControlToValidate="cliuser_confirm"></asp:RequiredFieldValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="center" valign="top" colspan="2">
                      <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="cliuser_confirm"
                        ControlToCompare="cliuser_password" ErrorMessage="Passwords do not match." />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Timezone:
                    </td>
                    <td align="left" valign="top">
                      <asp:DropDownList ID="cliuser_time" runat="server">
                      </asp:DropDownList>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      User Type:
                    </td>
                    <td align="left" valign="top">
                      <asp:RadioButtonList ID="cliuser_admin_flag" runat="server" SelectedValue='<%#Eval("cliuser_admin_flag")%>'
                        OnSelectedIndexChanged="test" AutoPostBack="true">
                        <asp:ListItem Value="Y">Admin</asp:ListItem>
                        <asp:ListItem Value="D">Demo</asp:ListItem>
                        <asp:ListItem Value="R">Research/Entry Only</asp:ListItem>
                        <asp:ListItem Value="N">Standard</asp:ListItem>
                        <asp:ListItem Value="M">Restricted to My Notes Only</asp:ListItem>
                      </asp:RadioButtonList>
                      <asp:ImageButton ID="ResetDemo" runat="server" AlternateText="ResetDemoTime" ImageUrl="~/images/reset_demo.gif"
                        CommandArgument='' CommandName="Update" Visible="false" horizontalalign="right" />
                    </td>
                  </tr>
                  <asp:Panel runat="server" ID="demo_end_date" Visible='<%#iif(Eval("cliuser_admin_flag") &"" = "D","true","false")%>'>
                    <tr>
                      <td align="left" valign="top">
                        Demo End Date:
                      </td>
                      <td align="left" valign="top">
                        <asp:TextBox runat="server" ID="demo_time" Width="70" Text='<%# clsgeneral.datenull(eval("cliuser_end_date"))%>'
                          Visible='true' />
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="demo_time"
                          Format="d" PopupButtonID="cal_image2" />
                        <asp:Image runat="server" ID="cal_image2" ImageUrl="~/images/final.jpg" Visible="true" />
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="demo_time"
                          ErrorMessage="<br />* Enter a valid start date" Operator="DataTypeCheck" Type="Date"
                          Display="Dynamic" />
                      </td>
                    </tr>
                  </asp:Panel>
                </table>
              </EditItemTemplate>
              <InsertItemTemplate>
                <table width="100%" cellspacing="0" cellpadding="5">
                  <tr>
                    <td align="left" valign="top">
                      First Name:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="cliuser_first_name" runat="server" Text='' Width="100" MaxLength="15"></asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ControlToValidate="cliuser_first_name"></asp:RequiredFieldValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Last Name:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="cliuser_last_name" runat="server" Text='' Width="150" MaxLength="25"></asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ControlToValidate="cliuser_last_name"></asp:RequiredFieldValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Email Address:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="cliuser_email_address" runat="server" Text='' Width="200" MaxLength="60"></asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator7" runat="server" ErrorMessage="*" ControlToValidate="cliuser_email_address"></asp:RequiredFieldValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="center" valign="top" colspan="2">
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="dynamic"
                        ControlToValidate="cliuser_email_address" ErrorMessage="Invalid Format for Email Address"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Login:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="cliuser_login" runat="server" Width="200" MaxLength="50"></asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator8" runat="server" ErrorMessage="*" ControlToValidate="cliuser_login"></asp:RequiredFieldValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Password:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="cliuser_password" runat="server" Width="150" TextMode="Password"
                        MaxLength="25"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator9"
                          runat="server" ErrorMessage="*" ControlToValidate="cliuser_password"></asp:RequiredFieldValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="center" valign="top" colspan="2">
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="dynamic"
                        ControlToValidate="cliuser_password" ErrorMessage="Password must contain one of the following: @#$%^&*/."
                        ValidationExpression=".*[@#$%^&*/].*" />
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="dynamic"
                        ControlToValidate="cliuser_password" ErrorMessage="Password must be 4-20 nonblank characters."
                        ValidationExpression="[^\s]{4,20}" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Confirm Password:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="cliuser_confirm" runat="server" value='' Text='' Width="150" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator10" runat="server" ErrorMessage="*" ControlToValidate="cliuser_confirm"></asp:RequiredFieldValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="center" valign="top" colspan="2">
                      <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="cliuser_confirm"
                        ControlToCompare="cliuser_password" ErrorMessage="Passwords do not match." />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Timezone:
                    </td>
                    <td align="left" valign="top">
                      <asp:DropDownList ID="cliuser_time" runat="server">
                      </asp:DropDownList>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      User Type:
                    </td>
                    <td align="left" valign="top">
                      <asp:RadioButtonList ID="cliuser_admin_flag" runat="server" OnSelectedIndexChanged="test"
                        AutoPostBack="true">
                        <asp:ListItem Value="Y">Admin</asp:ListItem>
                        <asp:ListItem Value="D">Demo</asp:ListItem>
                        <asp:ListItem Value="R">Research/Entry Only</asp:ListItem>
                        <asp:ListItem Value="N">Standard</asp:ListItem>
                      </asp:RadioButtonList>
                      <asp:TextBox runat="server" ID="cliuserExistingID" style="display:none"></asp:TextBox>
                    </td>
                  </tr>
                  <asp:Panel runat="server" ID="demo_end_date" Visible="false">
                    <tr>
                      <td align="left" valign="top">
                        Demo End Date:
                      </td>
                      <td align="left" valign="top">
                        <asp:TextBox runat="server" ID="demo_time" Width="70" Visible='true' />
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="demo_time"
                          Format="d" PopupButtonID="cal_image2" />
                        <asp:Image runat="server" ID="cal_image2" ImageUrl="~/images/final.jpg" Visible="true" />
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="demo_time"
                          ErrorMessage="<br />* Enter a valid start date" Operator="DataTypeCheck" Type="Date"
                          Display="Dynamic" />
                      </td>
                    </tr>
                  </asp:Panel>
                  <asp:ImageButton ID="ResetDemo" runat="server" AlternateText="ResetDemoTime" ImageUrl="~/images/reset_demo.gif"
                    CommandArgument='' CommandName="Update" horizontalalign="right" Visible='false' />
                  <%# set_demo_flags() %>
                  </td> </tr>
                </table>
              </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="" ShowHeader="False">
              <EditItemTemplate>
                <asp:ImageButton ID="ImageCancel" runat="server" AlternateText="Cancel" ImageUrl="~/images/cancel.gif"
                  CommandArgument='' CommandName="Cancel" CausesValidation="false" />
                <asp:ImageButton ID="ImageRemove" runat="server" AlternateText="Remove" ImageUrl="~/images/remove.gif"
                  CausesValidation="false" CommandArgument='' CommandName="Delete" horizontalalign="right" />
                <asp:ImageButton ID="ImageUpdate" runat="server" AlternateText="Update" ImageUrl="~/images/update.gif"
                  CommandArgument='' CommandName="Update" horizontalalign="right" />
              </EditItemTemplate>
              <InsertItemTemplate>
                <asp:ImageButton ID="ImageCancel" runat="server" AlternateText="Cancel" ImageUrl="~/images/cancel.gif"
                  CommandArgument='' CommandName="Cancel" CausesValidation="false" />
                <img src='images/spacer.gif' width='130' height='25' alt="" />
                <asp:ImageButton ID="ImageInsert" runat="server" AlternateText="Insert" ImageUrl="~/images/add_new.jpg"
                  CommandArgument='' CommandName="Insert" />
                <img src='images/spacer.gif' width='130' height='25' alt="" />
              </InsertItemTemplate>
            </asp:TemplateField>
          </Fields>
        </asp:DetailsView>
      </td>
    </tr>
  </table>
  <cc1:ModalPopupExtender ID="MPE" runat="server" TargetControlID="error_AlreadyExists"
    PopupControlID="inactiveUserPopup" BackgroundCssClass="modalBackground" DropShadow="true"
    CancelControlID="CancelButton" RepositionMode="None" />
  <asp:Panel ID="inactiveUserPopup" runat="server" Style="display: none" BackColor="AntiqueWhite"
    ForeColor="Red" Font-Bold="true">
    <p style="padding: 15px;">
      This login exists for an inactive account. Would you like to update and activate
      this account?</p>
    <div align="center">
      <asp:Button ID="OkButton" runat="server" Text="OK" OnClientClick="ToggleBoxOff();"
        OnClick="Okay_Button_ModalPopup" />
      <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
    </div>
  </asp:Panel>

  <script language="javascript" type="text/javascript"> 

              function ToggleBoxOff() { 
                 var obj = document.getElementById('<%= inactiveUserPopup.clientID %>')
                 if ((typeof (obj) != "undefined") && (obj != null)) {
                  obj.style.display = 'none'
                 }
              }
  </script>

  <asp:Button ID="error_AlreadyExists" runat="server" Text="Button" Style="display: none;" />
</asp:Panel>
