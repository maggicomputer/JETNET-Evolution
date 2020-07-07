<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Opportunities.ascx.vb"
  Inherits="crmWebClient.Opportunities" %>
<%@ Register TagPrefix="obout" Namespace="OboutInc.Flyout2" Assembly="obout_Flyout2_NET" %>

<script type="text/javascript">
  function FitPic() {
    window.resizeTo(930, 616);
    self.focus();
  }; 
</script>

<div class="row remove_margin">
  <div class="six columns remove_margin">
    <asp:Panel CssClass="Box" ID="ac_vis_display" runat="server" Visible="false">
      <div class="subHeader">
        Aircraft Model Information</div>
      <table width="320" cellpadding="3" cellspacing="0">
        <tr>
          <td align="left" valign="top">
            <asp:CheckBox ID="aircraft_related" runat="server" Text="Uncheck for Aircraft Search"
              Checked="true" AutoPostBack="true" />
          </td>
        </tr>
        <tr>
          <td align="left" valign="top" colspan="3">
            <asp:LinkButton ID="AC_Search_Vis" runat="server" Visible="false" CausesValidation="false">Click for Aircraft Search</asp:LinkButton>
            <asp:Panel runat="server" ID="ac_search" Visible="false">
              <table width="320" align="center" cellpadding="3" cellspacing="0" class="notes_pnl"
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
                    <asp:ImageButton ID="ac_search_button" runat="server" ImageUrl="~/images/search_button.jpg"
                      CausesValidation="false" />
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
      </table><br />
    </asp:Panel>
    <!----Company----->
    <div class="Box">
    <div class="subHeader">
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
          <asp:LinkButton ID="company_search_vis" runat="server" Visible="false" CausesValidation="false">Click for Company Search</asp:LinkButton>
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
                  <asp:ImageButton ID="company_search_button" runat="server" ImageUrl="~/images/search_button.jpg"
                    CausesValidation="false" />
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
    </table><br /></div>
    <!---Contact--->    <div class="Box">
    <div class="subHeader">
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
                  <asp:ImageButton ID="contact_search_button" runat="server" ImageUrl="images/search_button.jpg" />
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
    </table></div>
    <!---Display Portion Boxes-->
  </div>
  <div class="six columns">
    <asp:TextBox ID="jetnet_ac" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
    <asp:TextBox ID="action" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
    <asp:TextBox ID="client_ac" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
    <asp:TextBox ID="jetnet_comp" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
    <asp:TextBox ID="client_comp" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
    <asp:TextBox ID="jetnet_contact" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
    <asp:TextBox ID="client_contact" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
    <asp:TextBox ID="jetnet_mod" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
    <asp:TextBox ID="client_mod" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Opp_Edit"
      DisplayMode="BulletList" EnableClientScript="true" HeaderText="There are problems with the following fields:" />
    <asp:Panel runat="server" ID="oppPanel">
  
          <asp:Label ID="attention" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
          <asp:Panel runat="server" ID="action_view" Visible="true">
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="opp_cash"
              ErrorMessage="Cash value is required" ValidationGroup="Opp_Edit" Text="" Display="None"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dated"
              ErrorMessage="Date is Required" ValidationGroup="Opp_Edit" Text="" Display="None"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Date must be in mm/dd/yyyy format"
              Operator="DataTypeCheck" ControlToValidate="dated" Type="Date" ValidationGroup="Opp_Edit"
              Text="" Display="None"></asp:CompareValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="notes_title"
              ErrorMessage="Title is Required" ValidationGroup="Opp_Edit" Text="" Display="None"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Cash Value must be a valid Currency"
              Operator="DataTypeCheck" ControlToValidate="opp_cash" Type="Currency" ValidationGroup="Opp_Edit"
              Text="" Display="None"></asp:CompareValidator>
            <table width="100%" cellpadding="4" cellspacing="0" border="0" class="formatTable blue">
              <tr>
                <td align="left" valign="top">
                  Target Date:
                </td>
                <td align="left" valign="top">
                  <asp:TextBox ID="dated" runat="server" Width="70"></asp:TextBox>
                  <asp:Image runat="server" ID="cal_image" ImageUrl="~/images/final.jpg" />
                  <asp:ImageButton ID="Button1" Height="15" runat="server" ImageUrl="~/images/info.png"
                    Visible="true" /><obout:Flyout ID="Flyout1" runat="server" AttachTo="Button1" Position="TOP_LEFT"
                      Visible="true" Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true">
                      <p class="info_box">
                        Anticipated Capture Date<br />
                        <br />
                        <br />
                      </p>
                    </obout:Flyout>
                  <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="dated"
                    Format="d" PopupButtonID="cal_image" />
                </td>
              </tr>
              <tr>
                <td align="left" valign="top" width="73">
                  Status:
                </td>
                <td align="left" valign="top">
                  <asp:RadioButtonList ID="opp_status" runat="server" RepeatDirection="Horizontal"
                    RepeatLayout="Flow">
                    <asp:ListItem Text="Open" Value="O" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Closed" Value="C"></asp:ListItem>
                  </asp:RadioButtonList>
                </td>
              </tr>
            </table>
          </asp:Panel>
            <asp:Table ID="Table1" runat="server" cssclass="formatTable blue" Width="100%">
              <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Visible="true">
                                Assigned To:
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Visible="true">
                  <asp:DropDownList ID="pertaining_to" runat="server" Width="120">
                  </asp:DropDownList>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="75">
                            Title: </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                  <asp:TextBox ID="notes_title" runat="server" Width="320"></asp:TextBox>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="75">
                            Cash Value: </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                  <asp:TextBox ID="opp_cash" runat="server" Width="70"></asp:TextBox>
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
                  &nbsp;
                  <asp:ImageButton ID="ImageButton1" Height="15" runat="server" ImageUrl="~/images/info.png"
                    Visible="true" /><obout:Flyout ID="Flyout2" runat="server" AttachTo="ImageButton1"
                      Position="TOP_LEFT" Visible="true" Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true">
                      <p class="info_box">
                        % Chance of Capturing Opportunity<br />
                        <br />
                        <br />
                      </p>
                    </obout:Flyout>
                </asp:TableCell></asp:TableRow>
              <asp:TableRow ID="CategoryRow">
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">

                                    Category:</asp:TableCell>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                  <asp:DropDownList ID="notes_opp" runat="server" Width="120">
                  </asp:DropDownList>
                  <asp:TextBox ID="cat_name" runat="server" Width="250" Visible="false"></asp:TextBox>&nbsp;
                  <asp:LinkButton ID="visible_all" runat="server" Font-Size="Smaller" Font-Italic="false"
                    CausesValidation="false">Add Category</asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                      ID="cat_insert" runat="server" Visible="false" Font-Size="Smaller" Font-Italic="false"
                      CausesValidation="false">Insert</asp:LinkButton><asp:LinkButton ID="category_edit"
                        runat="server" Font-Size="Smaller" Font-Italic="false" CausesValidation="false">Edit Categories</asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                 Description:
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                  <asp:TextBox ID="notes_edit" runat="server" TextMode="MultiLine" Width="320" Height="140">
                  </asp:TextBox>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Enabled="true" runat="server"
                    ControlToValidate="notes_edit" ErrorMessage="Description Text is Required" ValidationGroup="Opp_Edit"
                    Text="" Display="None"></asp:RequiredFieldValidator>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                    <a href="javascript: window.opener.location.href = window.opener.location.href; self.close();"
                        class="button float_left">Close</a><asp:linkbutton ID="add_noteLB" runat="server"  CssClass="button float_right mobile_float_right" Text="Save"
                  CausesValidation="true" ValidationGroup="Opp_Edit" /><asp:LinkButton ID="removeNoteLB" OnClientClick="return confirm('Are you sure you want to Remove this Note?');"  runat="server" CssClass="button float_left mobile_float_left"
                        Visible="False" CausesValidation="False">Remove</asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
            </asp:Table>

    </asp:Panel>
    <asp:Table ID="categoryEditTable" runat="server" Width="100%" Visible="false"  cssclass="formatTable blue">
      <asp:TableRow>
        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">
          <h4>
            OPPORTUNITY CATEGORY EDITING</h4>
          <asp:Label ID="opp_updated" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
          <asp:DataGrid runat="server" ID="datagrid_details" CellPadding="3" horizontal-align="left"
            EnableViewState="true" ShowFooter="false" BackColor="White" Font-Size="8pt" AllowPaging="false"
            Width="530px" PageSize="25" CssClass="grid" Visible="true" BorderStyle="None" AllowSorting="True"
            AutoGenerateColumns="false" BorderColor="Gray" OnEditCommand="MyDataGrid_Edit"
            OnCancelCommand="MyDataGrid_Cancel" OnUpdateCommand="MyDataGrid_Update">
            <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
              Font-Underline="True" ForeColor="White" Mode="NumericPages" NextPageText="Next"
              PrevPageText="Previous" />
            <AlternatingItemStyle CssClass="alt_row" />
            <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="Gray" Font-Size="8pt" />
            <HeaderStyle BackColor="#A8C1DD" Font-Bold="True" Font-Size="8pt" Font-Underline="True"
              ForeColor="Black" Wrap="False" HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
            <Columns>
              <asp:EditCommandColumn EditText="Edit" UpdateText="Save" CancelText="Cancel" />
              <asp:TemplateColumn HeaderText="Name" ItemStyle-HorizontalAlign="left">
                <ItemTemplate>
                  <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "oppcat")), (DataBinder.Eval(Container.DataItem, "oppcat")), "")%>
                  <asp:TextBox runat="server" ID="id" Text='<%# DataBinder.Eval(Container.DataItem, "oppcat_key") %>'
                    Visible="true" Style="display: none;" />
                  <asp:TextBox runat="server" ID="name" Text='<%# DataBinder.Eval(Container.DataItem, "oppcat") %>'
                    Style="display: none;" />
                  <img src="images/spacer.gif" alt="" width="100" height="1" />
                </ItemTemplate>
                <EditItemTemplate>
                  <asp:TextBox runat="server" ID="id" Text='<%# DataBinder.Eval(Container.DataItem, "oppcat_key") %>'
                    Visible="true" Style="display: none;" />
                  <asp:TextBox runat="server" ID="name" Text='<%# DataBinder.Eval(Container.DataItem, "oppcat") %>' />
                </EditItemTemplate>
              </asp:TemplateColumn>
            </Columns>
          </asp:DataGrid>
          <p>
            <br />
            <br />
            <asp:LinkButton ID="ResumeOpportunities" runat="server" Font-Size="Medium" Font-Italic="false"
              CausesValidation="false" CssClass="right">Go Back to Opportunity</asp:LinkButton></p>
        </asp:TableCell>
      </asp:TableRow>
    </asp:Table>
  </div>
</div>

<script type="text/javascript">
  FitPic();
</script>

</asp:Panel> 