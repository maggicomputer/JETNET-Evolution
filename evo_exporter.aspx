<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="evo_exporter.aspx.vb"
  Inherits="crmWebClient.evo_exporter" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


  <script type="text/javascript">
    function checkTextAreaMaxLength(textBox, e, length) {

      var mLen = textBox["MaxLength"];
      if (null == mLen)
        mLen = length;

      var maxLength = parseInt(mLen);
      if (!checkSpecialKeys(e)) {
        if (textBox.value.length > maxLength - 1) {
          if (window.event)//IE
          {
            e.returnValue = false;
            return false;
          }
          else//Firefox
            e.preventDefault();
        }
      }
    }

    function checkSpecialKeys(e) {
      if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 35 && e.keyCode != 36 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
        return false;
      else
        return true;
    }
  </script>
  <style>
    .aircraftListing .formatTable, select {
      font-size: 13px;
    }

    .viewBoxMargin {
      margin-top: -15px;
      margin-bottom: -10px;
    }

    .aircraftListing.valueSpec.Simplistic, .aircraftListing.valueSpec.Simplistic .formatTable {
      border-spacing: 0px;
    }

    .aircraftListing .columns.four {
      padding-left: 2%;
      border-left: 3px solid #ddd;
    }
  </style>
  <div class="valueSpec Simplistic aircraftSpec aircraftListing viewBoxMargin" style="margin-top: -15px">
    <div class="gray_background_color" style="padding: 15px; height: 100%;">
      <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" style="width:99.5% !important;" runat="server"
        class="DetailsBrowseTable">
        <asp:TableRow>
          <asp:TableCell HorizontalAlign="center" VerticalAlign="middle">
          <div class="backgroundShade">
    <a href="#" class="float_right" onclick="javascript:window.close();"><img src="/images/x.svg" alt="Close" /></a></div>
          </asp:TableCell>
        </asp:TableRow>
      </asp:Table>
      <asp:Label runat="server" ID="container">
        <asp:Panel runat="server" ID="company_new" Visible="false" HorizontalAlign="left">
          <div class="Box">
            <asp:Label ID="label_text" runat="server">
            </asp:Label>
            <asp:Label ID="attention" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
            <asp:Label ID="warning1" runat="server"></asp:Label>
          </div>
          <br clear="all" />
          <cc1:TabContainer ID="tabs_container" Style="min-height: 400px;" runat="server" Width="100%"
            CssClass="dark-theme" Visible="true" AutoPostBack="true" ActiveTabIndex="1">
            <cc1:TabPanel ID="template_tab" runat="server" HeaderText="&nbsp;&nbsp;My Export Templates&nbsp;&nbsp;">
              <ContentTemplate>
                <table cellpadding="3" cellspacing="0" align="left" width="100%">
                  <tr>
                    <td align="left" valign="top">
                      <div class="columns eight">
                        <cc1:TabContainer ID="TabContainer1" Height="261px" CssClass="sub-theme" runat="server"
                          Width="100%" Visible="false" AutoPostBack="false" ActiveTabIndex="0">
                          <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Common Templates" Visible="false">
                            <HeaderTemplate>
                              Common Templates
                            </HeaderTemplate>
                            <ContentTemplate>
                              <asp:ListBox ID="common_export_list_box" runat="server" Width="400" Height="260"
                                SelectionMode="multiple" AutoPostBack="true"></asp:ListBox>
                            </ContentTemplate>
                          </cc1:TabPanel>
                          <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="My Export Templates" Visible="false">
                            <HeaderTemplate>
                              My Export Templates
                            </HeaderTemplate>
                            <ContentTemplate>
                            </ContentTemplate>
                          </cc1:TabPanel>
                        </cc1:TabContainer>
                        <asp:ListBox ID="my_export_list_box" runat="server" Width="100%" Height="220" SelectionMode="multiple"
                          AutoPostBack="true"></asp:ListBox>
                        <asp:Panel ID="templates_panel" runat="server" Visible="false">
                          <strong>VIEW:</strong>
                          <asp:DropDownList ID="export_types" runat="server" AutoPostBack="true">
                          </asp:DropDownList>
                          <asp:Label ID="help_link" runat="server" Visible="false"></asp:Label><asp:Label ID="selected_name"
                            Visible="false" Text="" runat="server"></asp:Label>
                          <asp:Label ID="help_button_label" runat="server" Visible="false"><input type='button' onclick="javascript: load('/help/documents/549.pdf', '', 'scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');" value="Help"></asp:Label>
                          <table width="100%" cellpadding="3" cellspacing="0" class="formatTable blue">
                            <tr class="header_row">
                              <td valign='top' align='left' width='16'>&nbsp;
                              </td>
                              <td valign='top' align='left' width='400'>
                                <strong>TEMPLATE NAME&nbsp;&nbsp;</strong>
                              </td>
                              <td valign='top' align='left' width='147'>
                                <strong>CREATOR&nbsp;&nbsp;</strong>
                              </td>
                              <td valign='top' align='left' width='12'>&nbsp;
                              </td>
                              <td valign='top' align='left' width='12'>&nbsp;
                              </td>
                            </tr>
                          </table>
                          <div style='height: 220px; width: 100%; overflow: auto;'>
                            <asp:Label ID="export_list" Visible="false" runat="server"></asp:Label>
                          </div>
                          <table width="100%" cellpadding="3" cellspacing="0" class="formatTable blue">
                            <tr>
                              <td>
                                <asp:Label ID="bottom_label_text" runat="server" Text="" CssClass="red_text"></asp:Label>&nbsp;
                              </td>
                            </tr>
                          </table>
                        </asp:Panel>
                      </div>
                      <div class="columns four">
                        <table width="100%" cellpadding="3" cellspacing="0" class="formatTable blue">
                          <tr class="noBorder">
                            <td>&nbsp;
                            </td>
                            <td></td>
                          </tr>
                          <tr>
                            <td class="noBorder" align="left" valign="top" colspan="2">
                              <asp:Panel runat="server" ID="export_instructions">
                                <span class="red_text">Please pick an export from the left hand side.</span>
                              </asp:Panel>
                              <asp:Panel runat="server" ID="export_info" Visible="false">
                                <strong>TITLE:</strong>
                                <asp:Label ID="my_export_title" runat="server" Text="">Special Export Format Aircraft Locations.</asp:Label>
                                <asp:Label ID="export_id_hold" runat="server" Text="" Visible="false"></asp:Label>
                              </asp:Panel>
                            </td>
                          </tr>
                          <tr>
                            <td align="left" valign="top" width="130">
                              <strong>EXPORT FIELDS:</strong></td>
                            <td align="left" valign="top">
                              <asp:ListBox ID="export_field_list_box" runat="server" Width="100%" Height="220" SelectionMode="multiple">
                                <asp:ListItem Text="">Please Select an Export</asp:ListItem>
                              </asp:ListBox></td>
                          </tr>
                          <tr>
                            <td align="left" valign="top">

                              <strong>DESCRIPTION:</strong></td>
                            <td align="left" valign="top">
                              <asp:Label ID="my_export_description" runat="server" Text="">Another export designed to showing where aircraft are physically
                                                            located.</asp:Label></td>
                          </tr>
                        </table>
                      </div>
                      <div>
                        <div style="padding-top: 7px;">
                          <table width="99%" align="center" cellpadding="3" cellspacing="0">
                            <tr>
                              <td align="left" valign="top" width="330">
                                <asp:Button ID="create_new_export_btn" runat="server" Text="Create New Export" />
                              </td>
                              <td align="right" valign="top" width="330">
                                <asp:Label ID="no_permission" Visible="false" runat="server" Text="No Permission to Modify this Template.<br/>The creator [] has permission to edit.<br>"
                                  Font-Bold="true" ForeColor="Red"></asp:Label>
                                <asp:Button ID="modify_export" runat="server" Text="Modify Export >" Visible="false" />
                                <asp:Button ID="run_export" runat="server" Text="Run Export >" />
                                <asp:Button ID="run_csv_export" runat="server" Text="Run CSV Export >" Width="120px" CssClass="float_right" />
                              </td>
                            </tr>
                          </table>
                        </div>
                      </div>
                    </td>
                  </tr>
                </table>
              </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="customize_tab" runat="server" HeaderText="&nbsp;&nbsp;Customize Export&nbsp;&nbsp;">
              <ContentTemplate>
                <table width="99%" cellpadding="3" cellspacing="0" border="0">
                  <tr>
                    <td align="left" valign="top">
                      <table width="100%" cellpadding="0" cellspacing="0" border="0" class="formatTable blue">
                        <tr>
                          <td align="left" valign="top" width="73">
                            <strong>TYPE:</strong>
                          </td>
                          <td align="left" valign="top" width="280">
                            <asp:DropDownList ID="export_type" runat="server" Width="180px" AutoPostBack="True">
                              <asp:ListItem Value="individual" Selected="True">Individual Fields</asp:ListItem>
                              <asp:ListItem Value="summary">Summary Level (Totals)</asp:ListItem>
                            </asp:DropDownList>
                          </td>
                          <td align="left" valign="top" width="73">
                            <strong>FORMAT:</strong>
                          </td>
                          <td align="left" valign="top">
                            <asp:DropDownList ID="format_options" runat="server" Width="180px">
                              <asp:ListItem Value="EXCEL">Export to Excel</asp:ListItem>
                            </asp:DropDownList>
                            <!--
                                                    <asp:ListItem Value="CSV">Comma Separated (CSV) File</asp:ListItem>
                                                    <asp:ListItem Value="TEXT">Text (Fixed Length)</asp:ListItem>
                                                    -->
                          </td>
                              <td align="left" valign="top">
                                  <asp:RadioButtonList ID="operating_radio" runat="server" Visible="false" >
                                      <asp:ListItem Text="Nautical Miles" Value="0">Nautical Miles</asp:ListItem>
                                      <asp:ListItem Text="Statute Miles" Value="1" >Statute Miles</asp:ListItem>
                                  </asp:RadioButtonList>
                                  </td>
                          <asp:Panel ID="shared_panel" runat="server" Visible="false">
                            <td align="right" valign="top" width="99">Shared?:
                            </td>
                            <td align="left" valign="top">
                              <asp:CheckBox ID="shared_check" runat="server"></asp:CheckBox>
                            </td>
                          </asp:Panel>
                        </tr>
                      </table>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      <asp:Panel ID="export_form_class" runat="server" Width="100%">
                        <table width="100%" cellspacing="0" cellpadding="0" class="formatTable blue">
                          <tr>
                            <td align="left" valign="top">
                              <asp:Panel ID="title_panel" runat="server" Visible="False">
                                <table width="100%" cellpadding="3" cellspacing="0">
                                  <tr>
                                    <td align="left" valign="top" width="88">
                                      <strong>TITLE:</strong>
                                    </td>
                                    <td align="left" valign="top" width="91%">
                                      <asp:TextBox ID="subject" runat="server" Width="100%"></asp:TextBox>
                                    </td>
                                  </tr>
                                </table>
                              </asp:Panel>
                            </td>
                            <td>&nbsp;</td>
                          </tr>
                          <tr>
                            <td align="left" valign="top" width="75%">
                              <asp:Panel ID="save_export_form" runat="server" Visible="False" Width="100%">
                                <table width="100%" cellpadding="3" cellspacing="0">
                                  <tr>
                                    <td align="left" valign="top" width="88">
                                      <strong>DESCRIPTION:</strong>
                                    </td>
                                    <td align="left" valign="top" width="91%">
                                      <textarea id="description" runat="server" style="width: 100%; float: right;" rows="19"> 
                                                                        </textarea>
                                    </td>
                                  </tr>
                                </table>
                              </asp:Panel>
                              <asp:Panel ID="available_fields_swap" runat="server" Width="100%">
                                <table width="100%" cellpadding="3" cellspacing="0">
                                  <tr class="noBorder">
                                    <td align="left" valign="top">
                                      <strong>AVAILABLE DATA TYPES:</strong>
                                    </td>
                                    <td align="left" valign="top">&nbsp;
                                    </td>
                                    <!--
                                                                <td align="left" valign="top">
                                                                    <b>Sub Selections</b>
                                                                </td> 
                                                                <td align="left" valign="top">
                                                                    &nbsp;
                                                                </td>
                                                                -->
                                    <td align="left" valign="top">
                                      <strong>AVAILABLE FIELDS</strong>
                                    </td>
                                    <td align="left" valign="top">&nbsp;
                                    </td>
                                  </tr>
                                  <tr>
                                    <!--
                                                                <td align="left" valign="top">
                                                                    <asp:ListBox ID="available_data_types" runat="server" Width="137px" Height="250px"
                                                                        SelectionMode="Multiple" AutoPostBack="True"></asp:ListBox>
                                                                </td>
                                                                <td align="left" width="5">
                                                                    &nbsp;
                                                                </td>
                                                                -->
                                    <td align="left" valign="top" width="50%">
                                      <asp:ListBox ID="sub_selections" runat="server" Width="100%" Height="200px" SelectionMode="Multiple"
                                        AutoPostBack="True"></asp:ListBox>
                                    </td>
                                    <td align="left" width="5">&nbsp;
                                    </td>
                                    <td align="left" valign="top" width="50%">
                                      <asp:ListBox ID="choice_to_export" runat="server" Width="100%" Height="200px" SelectionMode="Multiple"></asp:ListBox>
                                      <br />
                                      <asp:Button ID="Button1" Text="<<" OnClick="RemoveAllBtn_Click" runat="server" Width="30px" />
                                      <asp:Button ID="Button2" Text="<" OnClick="RemoveBtn_Click" runat="server" Width="26px" />
                                      <asp:Button ID="Button3" Text=">" OnClick="AddBtn_Click" runat="server" Width="29px" />
                                      <asp:Button ID="Button4" Text=">>" OnClick="AddAllBtn_Click" runat="server" Width="33px" />
                                    </td>
                                    <td align="left" width="5">&nbsp;
                                    </td>
                                  </tr>
                                </table>
                              </asp:Panel>
                            </td>
                            <td align="left" valign="top">
                              <table width="100%" cellpadding="3" cellspacing="0">
                                <tr class="noBorder">
                                  <td align="left" valign="top" colspan="2">
                                    <strong>FIELDS TO EXPORT:</strong>
                                  </td>
                                </tr>
                                <tr>
                                  <td align="left" valign="top" width='78%'>
                                    <asp:ListBox ID="info_to_export" runat="server" Width="99%" Height="200px" SelectionMode="Multiple"></asp:ListBox>
                                    <br />
                                    <asp:Button ID="clearselectedfields" Width="100%" Text="Clear All Fields" runat="server" /><br />
                                    <asp:Button ID="edit_selected_fields" Width="100%" Visible="False" Text="Edit Selected Fields"
                                      runat="server" /><br />
                                  </td>
                                  <td align="left" valign="top" width="22%">
                                    <asp:Button ID="move_up" Text="&uarr;" OnClick="ButtonMoveUp_Click" runat="server" /><br />
                                    <asp:Button ID="move_down" Text="&darr;" OnClick="ButtonMoveDown_Click" runat="server" />
                                  </td>
                                </tr>
                              </table>
                              <br />
                              <table width="100%">
                                <tr>
                                  <td align="right" valign="bottom">
                                    <table width="100%" cellpadding="2" cellspacing="0">
                                      <tr>
                                        <td align="right" valign="bottom">
                                          <asp:Panel ID="save_export_buttons" runat="server" Visible="False">
                                            <div style="margin-top: 7px;">
                                              <asp:TextBox ID="export_id" runat="server" Style="display: none;"></asp:TextBox>
                                              <asp:Button runat="server" ID="save_export_template" Text="Save" />&nbsp;
                                          <asp:Button runat="server" ID="save_run_export_template" Text="Save/Run" Visible="false" />
                                            </div>
                                          </asp:Panel>
                                        </td>
                                        <td align="left" valign="top">
                                          <asp:Panel ID="save_as_export_buttons" runat="server" Visible="False">
                                            <div style="margin-top: 7px;">
                                              <table width='90%'>
                                                <tr>
                                                  <td>
                                                    <asp:Button runat="server" ID="save_as_export_btn" Text="Save" />
                                                    <asp:Label ID="delete_warning" runat="server" Text="<table><tr><td nowrap='nowrap'>Are You Sure You Want to Delete?</td></tr></table>"
                                                      Visible="false" ForeColor="Red"></asp:Label>
                                                  </td>
                                                  <td>
                                                    <asp:Label ID="edit_warning" runat="server" Text="<table><tr><td nowrap='nowrap'>You Are Editing Someone Else's Export</td></tr></table>"
                                                      Visible="false" ForeColor="Red"></asp:Label>
                                                  </td>
                                                  <td>&nbsp;&nbsp;&nbsp;
                                                  </td>
                                                  <td>
                                                    <asp:Button runat="server" ID="delete_custom_button" Text="Delete" OnClientClick="if(!confirm('Are you sure you want to remove this export?'))return false;" />
                                                  </td>
                                                </tr>
                                              </table>
                                            </div>
                                          </asp:Panel>
                                        </td>
                                      </tr>
                                    </table>
                                  </td>
                                  <td align="right" width="100%" valign="bottom">
                                    <table width="100%" align="right" cellspacing="0" cellpadding="2" border="0">
                                      <tr>
                                        <td align="right" valign="bottom">
                                          <asp:Label ID="export_label" runat="server" Visible="False">
                                            <asp:Button ID="export_now_btn" runat="server" Text="Run Export >" Width="100px" CssClass="float_right" />
                                            <asp:Button runat="server" ID="create_new_export" Text="Save Template" Width="100px" CssClass="float_right" />
                                            <asp:Button ID="export_now_csv" runat="server" Text="Run CSV Export >" Width="120px" CssClass="float_right" />
                                          </asp:Label>
                                          <img src="images/spacer.gif" alt="" width="100%" height="1" />
                                        </td>
                                      </tr>
                                    </table>
                                  </td>
                                </tr>
                              </table>
                            </td>
                          </tr>
                        </table>
                      </asp:Panel>
                    </td>
                  </tr>
                </table>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <br clear="all" />
        </asp:Panel>
        <asp:DataGrid runat="server" ID="gridview1" CellPadding="9" HeaderStyle-BackColor="#204763"
          Visible="true" BackColor="White" font-name="tahoma" Font-Size="8pt" Width="825px"
          CssClass="grid" BorderStyle="None" Font-Names="verdana" AutoGenerateColumns="true">
          <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
            Font-Underline="True" ForeColor="White" />
          <AlternatingItemStyle CssClass="alt_row" />
          <ItemStyle BorderStyle="None" VerticalAlign="Top" HorizontalAlign="left" />
          <HeaderStyle BackColor="#67A0D9" Font-Bold="True" Font-Size="10" Font-Underline="True"
            ForeColor="White" Wrap="False" HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
        </asp:DataGrid>
        <%--</form>--%>
        <asp:Label Visible="false" ID="order_by" name="order_ny" runat="server"></asp:Label>
        <%--</body> </html> --%></asp:Label>
    </div>
  </div>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="below_form">

  <script type="text/javascript">
    window.onload = function () {
      window.resizeTo(1070, 800);
      self.focus();
    };

    if (<%= bRefreshPreferences.ToString.Tolower %>) {
      if ((typeof (window.opener) != "undefined") && (window.opener != null)) {
        window.opener.refreshPreferences();
        self.close();
      }
    }

  </script>

</asp:Content>
