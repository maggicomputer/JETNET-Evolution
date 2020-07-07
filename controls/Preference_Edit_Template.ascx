<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Preference_Edit_Template.ascx.vb"
    Inherits="crmWebClient.Preference_Edit_Template" %>

<asp:Panel CssClass="edit_panel" runat="server" ID="market_pref" Width="100%" Visible="false"
    BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" BackColor="#F9F9FF">
    <h4 align="right">
        Market Preferences Edit</h4>
    <p align="left" class="info_box">
        Use this form to identify the aircraft models that your company will use as defaults
        throughout the CRM as your primary aircraft market. Note that these defaults will
        be used for all users of your CRM.</p>
    <table width="100%" cellpadding="3" cellspacing="0">
        <tr>
            <td align="left" valign="top">
                Default to display activity for:
                <asp:DropDownList ID="market_time" runat="server">
                    <asp:ListItem Value="7">7 Days</asp:ListItem>
                    <asp:ListItem Value="31">One Month</asp:ListItem>
                    <asp:ListItem Value="93">Three Months</asp:ListItem>
                    <asp:ListItem Value="186">Six Months</asp:ListItem>
                    <asp:ListItem Value="279">Nine Months</asp:ListItem>
                    <asp:ListItem Value="365">Twelve Months</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" colspan="2">
                <table width="100%">
                    <tr>
                        <td align="left" valign="top">
                            Available Models<br />
                            <asp:ListBox ID="market_pref_models" runat="server" Rows="10" Width="250px" SelectionMode="multiple">
                            </asp:ListBox>
                        </td>
                        <td align="left" valign="top">
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td align="left" valign="top">
                            Selected Model Preferences<br />
                            <asp:ListBox ID="selected_models" runat="server" Rows="10" Width="250px" SelectionMode="multiple">
                            </asp:ListBox>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="Button8" Text="<<" OnClick="RemoveAllBtn_Click" runat="server" Width="30px"
                    CommandArgument="market" />
                <asp:Button ID="Button9" Text="<" OnClick="RemoveBtn_Click" runat="server" Width="26px"
                    CommandArgument="market" />
                <asp:Button ID="Button10" Text=">" OnClick="AddBtn_Click" runat="server" Width="29px"
                    CommandArgument="market" />
                <asp:Button ID="Button11" Text=">>" OnClick="AddAllBtn_Click" runat="server" Width="33px"
                    CommandArgument="market" />
            </td>
        </tr>
        <tr>
            <td align="right" valign="top" colspan="2">
                <asp:ImageButton ID="market_pref_btn" CausesValidation="true" Text="Update" runat="server"
                    ImageUrl="~/images/update.gif" AlternateText="Update" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel CssClass="edit_panel" runat="server" ID="ac_fields" Width="800" Visible="false"
    BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" BackColor="#F9F9FF">
    <h4 align="right">
        Aircraft Preferences Edit</h4>
    <asp:Label ID="aircraft_atten" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
    Field Preview
    <br />
    <asp:Label runat="server" ID="table_columns">
         <table width="100%" cellpadding="3" cellspacing="0" class="engine">
        <tr class="gray">
            <td align="left" valign="top">
                Year
            </td>
            <td align="left" valign="top">
                Model
            </td>
            <td align="left" valign="top">
                Serial #
            </td>
            <td align="left" valign="top">
                Registration #
            </td>
            <td align="left" valign="top">
                Owner
            </td>
            <td align="left" valign="top">
                Listed
            </td>
            <td align="left" valign="top">
                Asking
            </td>
            <td align="left" valign="top">
                Take
            </td>
            <td align="left" valign="top">
                Status
            </td>
        </tr>
    </table>
    </asp:Label>
    <br />
    <asp:Button ID="Button7" Text="Update" OnClick="UpdateFields" runat="server" />
    <table width="100%">
        <tr>
            <td align="left" valign="top">
                <asp:ListBox ID="all_fields" runat="server" Rows="15" Width="250"></asp:ListBox>
            </td>
            <td align="left" valign="top" width="250">
                <asp:ListBox ID="client_fields" runat="server" Rows="15" SelectionMode="Multiple"
                    Width="250"></asp:ListBox>
            </td>
            <td align="left" valign="top">
                <asp:Button ID="Button5" Text="&uarr;" OnClick="ButtonMoveUp_Click" runat="server" /><br />
                <asp:Button ID="Button6" Text="&darr;" OnClick="ButtonMoveDown_Click" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <asp:Button ID="Button1" Text="<<" OnClick="RemoveAllBtn_Click" runat="server" Width="30px" />
                <asp:Button ID="Button2" Text="<" OnClick="RemoveBtn_Click" runat="server" Width="26px" />
                <asp:Button ID="Button3" Text=">" OnClick="AddBtn_Click" runat="server" Width="29px" />
                <asp:Button ID="Button4" Text=">>" OnClick="AddAllBtn_Click" runat="server" Width="33px" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel CssClass="edit_panel" runat="server" ID="preferences_panel" Width="100%"
    Visible="true" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" BackColor="#F9F9FF">
    <h4 align="right">
        Personal Preferences Edit</h4>
    <asp:Label ID="personal_atten" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
    <table width="350" cellpadding="4" cellspacing="0">
        <tr>
            <td align="left" valign="top">
                Name:
            </td>
            <td align="left" valign="top">
                <asp:Label ID="cliuser_first_name" runat="server"><%#Eval("cliuser_first_name") %></asp:Label>
                <asp:Label ID="cliuser_last_name" runat="server"><%#Eval("cliuser_last_name")%></asp:Label>
                <asp:Label ID="cliuser_end_date" runat="server" visible="false"><%#Eval("cliuser_end_date")%></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                Login:
            </td>
            <td align="left" valign="top">
                <asp:Label ID="cliuser_login" runat="server"><%#Eval("cliuser_login")%></asp:Label>
                <asp:Label ID="cliuser_password" runat="server" Style="display: none;"><%#Eval("cliuser_password")%></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                Email Address:
            </td>
            <td align="left" valign="top">
                <asp:Label ID="cliuser_email_address" runat="server"><%#Eval("cliuser_email_address")%></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                Timezone:
            </td>
            <td align="left" valign="top">
                <asp:DropDownList ID="cliuser_time_zone" runat="server">
                </asp:DropDownList>
                <asp:Label ID="cliuser_id" runat="server" Style="display: none;"><%#Eval("cliuser_id")%></asp:Label><asp:Label
                    ID="cliuser_user_id" runat="server" Style="display: none;"><%#Eval("cliuser_user_id")%></asp:Label></asp:Label><asp:Label
                        ID="cliuser_admin_flag" runat="server" Style="display: none;"><%#Eval("cliuser_admin_flag")%></asp:Label>
            </td>
        </tr>
    </table>
    <p align="right">
        <asp:ImageButton ID="update_preferences" CausesValidation="true" CommandArgument=""
            CommandName="Update" Text="Update" runat="server" ImageUrl="~/images/update.gif"
            AlternateText="Update" /></p>
</asp:Panel>
<asp:Panel CssClass="edit_panel" runat="server" ID="client_preferences" Visible="true"
    BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" BackColor="#F9F9FF"
    Width="100%">
    <h4 align="right">
        Company Preferences Edit</h4>
    <asp:Label ID="company_atten" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
    <table width="450" cellpadding="4" cellspacing="0">
        <tr>
            <td align="left" valign="top">
                &nbsp;
            </td>
            <td align="left" valign="top">
                <strong>Category Name</strong>
            </td>
            <td align="left" valign="top">
                <strong>Use?</strong>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                Category #1:
            </td>
            <td align="left" valign="top">
                <asp:TextBox ID="pref_id" runat="server" Width="310" Style="display: none;" />
                <asp:TextBox ID="pref_1" runat="server" value='<%#Eval("clipref_category1_name")%>'
                    Width="310" MaxLength="60" />
            </td>
            <td align="left" valign="top">
                <asp:CheckBox ID="pref_1_use" runat="server" Checked='<%#IIf(Eval("clipref_category1_use") = "Y", "true", "false")%>' />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                Category #2:
            </td>
            <td align="left" valign="top">
                <asp:TextBox ID="pref_2" runat="server" value='<%#Eval("clipref_category2_name")%>'
                    Width="310" MaxLength="60" />
            </td>
            <td align="left" valign="top">
                <asp:CheckBox ID="pref_2_use" runat="server" Checked='<%#IIf(Eval("clipref_category2_use") = "Y", "true", "false")%>' />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                Category #3:
            </td>
            <td align="left" valign="top">
                <asp:TextBox ID="pref_3" runat="server" value='<%#Eval("clipref_category3_name")%>'
                    Width="310" MaxLength="60" />
            </td>
            <td align="left" valign="top">
                <asp:CheckBox ID="pref_3_use" runat="server" Checked='<%#IIf(Eval("clipref_category3_use") = "Y", "true", "false")%>' />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                Category #4:
            </td>
            <td align="left" valign="top">
                <asp:TextBox ID="pref_4" runat="server" value='<%#Eval("clipref_category4_name")%>'
                    Width="310" MaxLength="60" />
            </td>
            <td align="left" valign="top">
                <asp:CheckBox ID="pref_4_use" runat="server" Checked='<%#IIf(Eval("clipref_category4_use") = "Y", "true", "false")%>' />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                Category #5:
            </td>
            <td align="left" valign="top">
                <asp:TextBox ID="pref_5" runat="server" value='<%#Eval("clipref_category5_name")%>'
                    Width="310" MaxLength="60" />
            </td>
            <td align="left" valign="top">
                <asp:CheckBox ID="pref_5_use" runat="server" Checked='<%#IIf(Eval("clipref_category5_use") = "Y", "true", "false")%>' />
            </td>
        </tr>
    </table>
    <p align="right">
        <asp:ImageButton ID="update_client_pref" CausesValidation="true" Text="Update" runat="server"
            ImageUrl="~/images/update.gif" AlternateText="Update" /></p>
</asp:Panel>
