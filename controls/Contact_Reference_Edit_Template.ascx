<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Contact_Reference_Edit_Template.ascx.vb"
  Inherits="crmWebClient.Contact_Reference_Edit_Template" %>
<!--Client Company Add an AC Reference-->
<asp:Panel runat="server" ID="view_panel" Visible="false" CssClass="valueSpec viewValueExport Simplistic aircraftSpec plain">
  <asp:Table runat="server" ID="view_panel_table" Width="700px" CssClass="formatTable blue">
    <asp:TableRow Visible="false" ID="sep1" BackColor="#94B4CB">
      <asp:TableCell ColumnSpan="2" ForeColor="#486071" HorizontalAlign="left" Font-Bold="true"
        Font-Size="Large"><span style="display:block;padding:5px">Add a Relationship</span></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TableRow1" cellpadding="4" cellspacing="0" runat="server" BorderColor="#CCCCCC"
      BorderStyle="Solid" BorderWidth="1px" BackColor="#F9F9FF">
      <asp:TableCell runat="server" ID="Company_View" VerticalAlign="Top" Width="350px">
        <div class="subHeader">
          Company Information:</div>
        <asp:Label runat="server" ID="company_view_text"></asp:Label><br />
      </asp:TableCell>
      <asp:TableCell VerticalAlign="Top" Width="280px">
        <div class="subHeader">
          Aircraft Information:</div>
        <table width="100%" cellpadding="3" cellspacing="0">
          <tr>
            <td align="left" valign="top" colspan="3">
              <asp:Panel runat="server" ID="ac_search">
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
                      Ser #/Reg #<br />
                      Make/Model:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox runat="server" ID="ac_sear" Width="130" />
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
        </table>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TableRow2" runat="server" Visible="false">
      <asp:TableCell runat="server" ID="TableCell1" VerticalAlign="Top" ColumnSpan="2"
        BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" BackColor="#F9F9FF">
        <div class="subHeader">
          Customize this Reference</div>
        <table width="500" cellpadding="4" cellspacing="0">
          <tr>
            <td align="left" valign="top">
              <asp:ListBox runat="server" SelectionMode="Single" ID="aircraft_name" Width="420px"
                AutoPostBack="true"></asp:ListBox>
            </td>
            <td align="left" valign="top" width="250">
              <br />
              <asp:Panel runat="server" ID="company_ac_add" Visible="false">
                <table width="100%" cellpadding="5" cellspacing="0">
                  <tr>
                    <td align="left" valign="top">
                      Relationship:
                    </td>
                    <td align="left" valign="top">
                      <asp:DropDownList ID="relationship" runat="server" AutoPostBack="true" Width="250">
                      </asp:DropDownList>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Priority:
                    </td>
                    <td align="left" valign="top">
                      <asp:DropDownList ID="ac_priority" runat="server" AutoPostBack="true" Width="250">
                      </asp:DropDownList>
                    </td>
                  </tr>
                </table>
              </asp:Panel>
              <img src="images/spacer.gif" alt="" width="280" height="1" />
            </td>
          </tr>
        </table>
        <p align="center">
          <asp:Label ID="ref_update" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label><br />
          <br />
          <asp:ImageButton ID="add_ref" runat="server" ImageUrl="~/images/add_new.jpg" Visible="false"
            OnClientClick="return confirm('Are you sure you want to Add this Reference?');" /></p>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow Visible="false" ID="sep" BackColor="#94B4CB">
      <asp:TableCell ColumnSpan="2" ForeColor="#486071" HorizontalAlign="left" Font-Bold="true"
        Font-Size="Large"><span style="display:block;padding:5px">Company Editing</asp:TableCell>
    </asp:TableRow>
  </asp:Table>
</asp:Panel>
<!--Add Contact Ref-->
<asp:Panel runat="server" ID="contact_ref_add" Visible="false" CssClass="valueSpec viewValueExport Simplistic aircraftSpec plain">
  <asp:Label ID="aircraft_edit_text" runat="server"><h2 class="mainHeading" align="right"><strong>Aircraft Name</strong> Edit</h2></asp:Label>
  <asp:Label ID="contact_ref_add_errormsg" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
  <asp:Table runat="server" ID="Table1" Width="800px">
    <asp:TableRow ID="TableRow3" cellpadding="4" cellspacing="0" runat="server" Width="800px"
      BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" BackColor="#F9F9FF">
      <asp:TableCell runat="server" ID="TableCell2" VerticalAlign="Top" Width="250px">
        <div class="Box">
          <div class="subHeader">
            Aircraft Information:</div>
          <asp:Label runat="server" ID="ac_info_display" CssClass="formatTable blue" Width="220px"></asp:Label><br />
          <img src="images/spacer.gif" alt="" width="250" height="1" /><br />
          <br />
        </div>
      </asp:TableCell>
      <asp:TableCell Width="20">&nbsp;</asp:TableCell>
      <asp:TableCell VerticalAlign="Top" Width="280px">
        <div class="Box">
          <div class="subHeader">
            Contact Information:</div>
          <p align="left" class="info_box">
            <asp:Label runat="server" ID="ac_ref_instructions">
                    Please search for a contact or a company first.</asp:Label></p>
          <asp:TextBox runat="server" ID="contact_ref_id" Style="display: none;" />
          <asp:TextBox runat="server" ID="jetnet_comp_id" Style="display: none;" />
          <asp:TextBox runat="server" ID="client_comp_id" Style="display: none;" />
          <img src="images/spacer.gif" width="700" height="1" />
          <asp:Table Width="100%" CellPadding="3" CellSpacing="0" runat="server" CssClass="formatTable blue">
            <asp:TableRow runat="server" CssClass="noBorder">
              <asp:TableCell align="left" valign="top" Width="33%" runat="server">
                <asp:Button ID="existing_company_button" runat="server" Text="Choose from Existing Company" />
              </asp:TableCell>
              <asp:TableCell runat="server" align="center" valign="top" Width="37%">
                <asp:Button ID="search_company_button" runat="server" Text="Search for a Contact or Company" />
              </asp:TableCell>
              <asp:TableCell ID="TableCell5" runat="server" align="right" valign="top" Width="25%">
                <input type="button" value="Contact Quick Entry" runat="server" id="contactQuickEntry" />
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow4" runat="server" CssClass="noBorder">
              <asp:TableCell ID="TableCell4" runat="server" ColumnSpan="3">
                <asp:Panel runat="server" ID="existing_company_panel" Visible="false">
                  <asp:RadioButtonList runat="server" ID="existing_subset" RepeatDirection="Horizontal"
                    AutoPostBack="true">
                    <asp:ListItem Value="jetnet_input">View Jetnet Data</asp:ListItem>
                    <asp:ListItem Value="client_input">View Client Data</asp:ListItem>
                  </asp:RadioButtonList>
                </asp:Panel>
                <asp:Panel runat="server" ID="second_existing_company_panel" Visible="false">
                  <table width="100%" cellpadding="3" cellspacing="0">
                    <tr>
                      <td align="left" valign="top" colspan="3">  <div class="subHeader">Search Parameters</div>
                        <table width="50%" align="left" cellpadding="3" cellspacing="0" class="Box"
                          border="0">
                          <tr>
                            <td align="left" valign="top" width="100">
                              <asp:Label ID="Label1" runat="server" Text="First Name" Visible="true">First Name</asp:Label>
                            </td>
                            <td align="left" valign="top">
                              <asp:TextBox runat="server" ID="existing_first" Width="100" Visible="true" />
                            </td>
                          </tr>
                          <tr>
                            <td align="left" valign="top">
                              <asp:Label ID="Label2" runat="server" Text="Last Name" Visible="true">Last Name:</asp:Label>
                            </td>
                            <td align="left" valign="top">
                              <asp:TextBox runat="server" ID="existing_last" Width="100" Visible="true" />
                            </td>
                          </tr>
                          <tr>
                            <td align="left" valign="top">
                            </td>
                            <td align="right" valign="top">
                              <asp:ImageButton ID="contact_search_ref_2" runat="server" ImageUrl="~/images/search_button.jpg" />
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </asp:Panel>
              </asp:TableCell></asp:TableRow>
            <asp:TableRow runat="server" CssClass="noBorder">
              <asp:TableCell runat="server" ColumnSpan="2">
                <asp:Panel runat="server" ID="search_company_panel" Visible="false">
                  <table width="100%" cellpadding="3" cellspacing="0">
                    <tr>
                      <td align="left" valign="top" colspan="3">
                        <div class="subHeader">
                          Search Parameters</div>
                        <table width="100%" align="center" cellpadding="3" cellspacing="0" class="Box"
                          border="0">
                          <tr>
                            <td align="left" valign="top" colspan="2">
                              <asp:RadioButtonList runat="server" ID="input" RepeatDirection="Horizontal">
                                <asp:ListItem Value="jetnet_input">Search Jetnet Data</asp:ListItem>
                                <asp:ListItem Value="client_input" Selected="True">Search Client Data</asp:ListItem>
                              </asp:RadioButtonList>
                            </td>
                          </tr>
                          <tr>
                            <td align="left" valign="top">
                              Company Name:
                            </td>
                            <td align="left" valign="top">
                              <asp:TextBox runat="server" ID="company" Width="180" />
                            </td>
                          </tr>
                          <tr class="noBorder">
                            <td align="left" valign="top">
                              <asp:Label ID="first_name_vis" runat="server" Text="First Name" Visible="false">First Name</asp:Label>
                            </td>
                            <td align="left" valign="top">
                              <asp:TextBox runat="server" ID="first" Width="100" Visible="false" />
                            </td>
                          </tr>
                          <tr class="noBorder">
                            <td align="left" valign="top">
                              <asp:Label ID="last_name_vis" runat="server" Text="Last Name" Visible="false">Last Name:</asp:Label>
                            </td>
                            <td align="left" valign="top">
                              <asp:TextBox runat="server" ID="last" Width="100" Visible="false" />
                            </td>
                          </tr>
                          <tr class="noBorder">
                            <td align="left" valign="top">
                            </td>
                            <td align="right" valign="top">
                              <asp:ImageButton ID="searching_cont_ref" runat="server" ImageUrl="~/images/search_button.jpg" />
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </asp:Panel>
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" ID="customize_reference" Visible="false" CssClass="noBorder">
              <asp:TableCell runat="server" ID="TableCell3" VerticalAlign="Top" ColumnSpan="2">
                <div class="subHeader">
                  Customize this Reference</h3>
                  <table width="100%">
                    <tr>
                      <td align="left" valign="top">
                        <table width="250" cellpadding="0" cellspacing="0">
                          <tr>
                            <td align="left" valign="top">
                              <asp:Panel ID="comp_name_vis" Visible="false" runat="server">
                                <asp:ListBox runat="server" SelectionMode="Single" ID="comp_name" Width="420px" AutoPostBack="true">
                                </asp:ListBox>
                              </asp:Panel>
                              <br />
                              <asp:Panel ID="contact_name_vis" Visible="false" runat="server">
                                <asp:ListBox runat="server" SelectionMode="Single" ID="contact_name" Width="420px"
                                  AutoPostBack="true"></asp:ListBox>
                              </asp:Panel>
                              <br />
                              <asp:Label ID="contact_info" runat="server" Text="" Visible="true"></asp:Label>
                            </td>
                          </tr>
                        </table>
                      </td>
                      <td align="left" valign="top" width="250">
                        <img src="images/spacer.gif" alt="" width="250" height="1" /><br />
                        <asp:Panel ID="customize_relationship" Visible="false" runat="server">
                          <table width="250" cellpadding="4" cellspacing="0" align="right">
                            <tr>
                              <td align="left" valign="top" width="100">
                                Relationship:
                              </td>
                              <td align="left" valign="top">
                                <asp:DropDownList ID="relationship_con" runat="server" Width="150" AutoPostBack="true">
                                </asp:DropDownList>
                              </td>
                              <td align="left" valign="top" rowspan="2">
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top">
                                Priority:
                              </td>
                              <td align="left" valign="top">
                                <asp:DropDownList ID="priority" runat="server" Width="150">
                                </asp:DropDownList>
                              </td>
                            </tr>
                          </table>
                        </asp:Panel>
                      </td>
                    </tr>
                  </table>
                  <p align="center">
                    <asp:Label ID="ref_two_add" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label><br />
                    <br />
                    <asp:ImageButton ID="add_cont_ref" Visible="false" runat="server" ImageUrl="~/images/add_new.jpg"
                      OnClientClick="return confirm('Are you sure you want to add this relationship to
    the aircraft?');" /></p>
              </asp:TableCell>
            </asp:TableRow>
          </asp:Table>
        </div>
      </asp:TableCell></asp:TableRow>
  </asp:Table>
</asp:Panel>
<!--End Add Contact Ref-->
