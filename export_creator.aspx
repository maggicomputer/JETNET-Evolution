<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="export_creator.aspx.vb"
  Inherits="crmWebClient.export_creator" %>

<%@ Import Namespace="crmwebclient.clsgeneral" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <title>Edit Template</title>
  <link href="common/redesign.css" rel="stylesheet" type="text/css" />
  <link rel="stylesheet" type="text/css" href="common/anylinkmenu.css" />
  <style>
    input, textarea
    {
      font-family: arial;
      font-size: 12px;
    }
  </style>
</head>
<body bgcolor="#ffffff">
  <form id="form1" runat="server">
  <div style="background-image: url('images/file_background.jpg'); background-repeat: repeat-x;
    margin: 0px; padding: 0px;">
    <asp:Menu ID="file_menu" runat="server" Orientation="Horizontal" Font-Size="11px"
      Font-Bold="false" DynamicEnableDefaultPopOutImage="False" EnableViewState="False"
      Visible="false" StaticEnableDefaultPopOutImage="False">
      <LevelMenuItemStyles>
        <asp:MenuItemStyle CssClass="sub" />
        <asp:MenuItemStyle CssClass="export_menu" BackColor="#0d4d7b" />
      </LevelMenuItemStyles>
      <StaticHoverStyle CssClass="static_hover"></StaticHoverStyle>
      <Items>
        <asp:MenuItem ImageUrl="images/file_save.jpg"></asp:MenuItem>
      </Items>
    </asp:Menu>
  </div>
  <asp:Label ID="no_export_error" runat="server" Text="" Font-Bold="true" ForeColor="Red"
    Visible="false"></asp:Label>
  <asp:Panel runat="server" ID="company_new" Visible="false" CssClass="search_pnl_export"
    HorizontalAlign="left" Height="530">
    <asp:Label ID="export_title" runat="server" Text="Company List Export" CssClass="export_header"></asp:Label><br />
    <asp:Label ID="attention" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
    <table cellpadding="3" cellspacing="0" align="center">
      <tr>
        <td align="left" valign="top">
          <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
              <td align="left" valign="top" width="250">
                <asp:CheckBoxList runat="server" ID="type_of_info" AutoPostBack="true">
                </asp:CheckBoxList>
              </td>
              <td align="left" valign="top">
                <asp:CheckBox ID="phone" runat="server" AutoPostBack="true" Text="Company Phone Numbers"
                  OnCheckedChanged="add_phone" Visible="false" /><br />
                <asp:CheckBox ID="contact_phone" runat="server" AutoPostBack="true" Text="Contact Phone Numbers"
                  OnCheckedChanged="add_phone" Visible="false" />
              </td>
            </tr>
          </table>
        </td>
        <td align="left" valign="top" rowspan="2">
          <table width="100%">
            <tr>
              <td align="left" valign="top" width="90%">
                <asp:CheckBox ID="custom_export" runat="server" Text="Enable Custom Export?<br /><br />"
                  Visible="false" AutoPostBack="true" /><asp:Label ID="export_label" runat="server"
                    Visible="false">
                    <asp:ImageButton ID="export_now" runat="server" AlternateText="Export Now" ImageUrl="~/images/export_now.jpg" /><br />
                    <br />
                  </asp:Label><asp:Label ID="load_export_label" runat="server" Visible="false">
                    <asp:ImageButton ID="load_export" runat="server" Visible="true" AlternateText="Load Export Template"
                      ImageUrl="~/images/load_export_template.jpg" /><br />
                    <br />
                  </asp:Label><asp:ImageButton ID="create_export_template" runat="server" AlternateText="Create Export Template"
                    ImageUrl="~/images/create_export_template.jpg" />
              </td>
              <td align="left" valign="top">
                <asp:ImageButton ID="cancel_now" runat="server" AlternateText="Cancel Export" ImageUrl="~/images/cancel_export.jpg"
                  OnClientClick="javascript:window.close();" />
              </td>
            </tr>
          </table>
          <br />
          <table><tr><td>
          <asp:CheckBox ID="merge_lists" runat="server" Checked="true" Visible="false" Text="Exclude Jetnet Records where a client record exists" />  
          </td></tr></table>
          <p align="left" class="info_box">
            Please select from the list of available fields using the arrows below the list.
            Once you have your desired fields in the "Fields Desired for Export" list then click
            on "Create Export" to generate the desired export.</p>
          <asp:Label runat="server" ID="export_info_box"><p align="left" class="info_box">If you have previously saved an export template, click on 'Load Export Templates' to select and open a previous template.</p></asp:Label>
          <img src="images/spacer.gif" width="400" height="1" alt="" />
          <asp:Panel runat="server" ID="open_project" Visible="false" HorizontalAlign="left">
            <table width="100%" style="background-color: #e5e5e5; border: 1px solid #67a0d9;">
              <tr>
                <td align="left" valign="top" colspan="2">
                  <h3>
                    Template Name (Model if Applicable)</h3>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  <asp:DropDownList ID="open_project_ddl" runat="server" Width="340">
                  </asp:DropDownList>
                  Open Export Template
                </td>
                <td align="left" valign="top">
                  <asp:Button ID="Open_Project_Btn" runat="server" Text="Open" />
                </td>
              </tr>
            </table>
          </asp:Panel>
          <asp:Panel runat="server" ID="file_open_dialog" Visible="false">
            <table width="100%" cellpadding="2" cellspacing="0" style="background-color: #e5e5e5;
              border: 1px solid #67a0d9;">
              <tr>
                <td align="left" valign="top" colspan="4">
                  <h3>
                    <asp:Label runat="server" ID="selected_export_template">Selected Export Template:</asp:Label></h3>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Name:
                </td>
                <td align="left" valign="top">
                  <asp:TextBox ID="file_name" runat="server" Width="320" />
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Description:
                </td>
                <td align="left" valign="top" colspan="3">
                  <asp:TextBox ID="file_description" TextMode="multiline" runat="server" Width="320"
                    Rows="7" />
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Shared:
                </td>
                <td align="left" valign="top">
                  <asp:CheckBox ID="file_shared" runat="server" value="Y" /><em class="tiny">Check this
                    box if you would like to share this export template with others on your team</em>
                  <asp:TextBox ID="file_id" runat="server" Width="40" Text="0" Style="display: none" />
                </td>
              </tr>
              <tr><td>
              Market Default?
              </td>
              <td>
             <asp:CheckBox ID="check_market_default" runat="server" Visible="true" AutoPostBack="true" /> <em class="tiny">Check this box if you want to make this your default standard template?</em> </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Model:
                </td>
                <td align="left" valign="top">
                   <asp:DropDownList ID="model_list" runat="server" ></asp:DropDownList>
                   <asp:DropDownList ID="model_list_source"  runat="server" Visible="false" ></asp:DropDownList>
                   <asp:Label ID="default_label" runat="server" Visible="false" Text="Is Default Model Export?"></asp:Label><asp:CheckBox ID="default_model_export" runat="server" Visible="false" Checked="false"  />
                </td>
              </tr>
              <tr><td colspan="5">
              <asp:Label ID="warning_label" runat="server" Visible="false" Text="" ForeColor="Red"></asp:Label>
              </td></tr>
              <tr>
                <td align="left" valign="top" colspan="2">
                  <p align="left" class="float_left">
                    <asp:ImageButton ID="file_save" runat="server" AlternateText="Save Template" ImageUrl="~/images/save_template.jpg" />
                  <asp:ImageButton ID="file_save_as" runat="server" AlternateText="Save as New Template"
                      ImageUrl="~/images/save_as_new_template.jpg" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  </p>
                  <p align="left" class="float_right">
                    <asp:ImageButton ID="cancel_file_save" runat="server" AlternateText="Cancel" ImageUrl="~/images/cancel.gif" />
                    <asp:ImageButton ID="file_delete" OnClientClick="if(!confirm('Are you sure you wish to delete this Template?'))return false;"
                      Visible="false" runat="server" AlternateText="Delete Template" ImageUrl="~/images/remove.gif" />
                  </p>
                </td>
              </tr>
            </table>
          </asp:Panel>
        </td>
      </tr>
      <tr>
        <td align="left" valign="top">
          <table cellpadding="3" cellspacing="0">
            <tr>
              <td align="left" valign="top">
                Available Field List<br /> 
                  <asp:DropDownList ID="export_type_drop" runat="server" Width="150" AutoPostBack="true">
                  </asp:DropDownList>
                <asp:ListBox ID="choice_to_export" runat="server"  Height="250" SelectionMode="multiple" Font-Size="X-Small">
                  <asp:ListItem Text="Please make a Selection"></asp:ListItem>
                </asp:ListBox>
                <br />
                <asp:Button ID="Button1" Text="<<" OnClick="RemoveAllBtn_Click" runat="server" Width="30px" />
                <asp:Button ID="Button2" Text="<" OnClick="RemoveBtn_Click" runat="server" Width="26px" />
                <asp:Button ID="Button3" Text=">" OnClick="AddBtn_Click" runat="server" Width="29px" />
                <asp:Button ID="Button4" Text=">>" OnClick="AddAllBtn_Click" runat="server" Width="33px" />
              </td>
              <td align="left" width="50">
                &nbsp;&nbsp;&nbsp;
              </td>
              <td align="left" valign="top">
                Fields to Export<br />
                <asp:ListBox ID="info_to_export" runat="server" Width="275" Height="250" SelectionMode="multiple" Font-Size="X-Small">
                </asp:ListBox>
              </td>
              <td align="left" valign="top" width="50">
                <br />
                <asp:Button ID="Button5" Text="&uarr;" OnClick="ButtonMoveUp_Click" runat="server" /><br />
                <asp:Button ID="Button6" Text="&darr;" OnClick="ButtonMoveDown_Click" runat="server" />
              </td>
            </tr>
          </table>
        </td>
        <td align="left" valign="top">
        </td>
      </tr>
    </table>
    <asp:DataGrid runat="server" ID="gridview1" CellPadding="9" HeaderStyle-BackColor="#204763"
      Visible="true" BackColor="White" Font-Name="tahoma" Font-Size="8pt" Width="825px"
      CssClass="grid" BorderStyle="None" Font-Names="verdana" AutoGenerateColumns="true">
      <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
        Font-Underline="True" ForeColor="White" />
      <AlternatingItemStyle CssClass="alt_row" />
      <ItemStyle BorderStyle="None" VerticalAlign="Top" HorizontalAlign="left" />
      <HeaderStyle BackColor="#67A0D9" Font-Bold="True" Font-Size="10" Font-Underline="True"
        ForeColor="White" Wrap="False" HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
    </asp:DataGrid>
  </asp:Panel>
  
  <asp:TextBox runat="server" ID="ac_prospect"></asp:TextBox>
  </form>

  <script language="javascript" type="text/javascript">
    acbox_id = querySt("acp")
    acbox = window.opener.document.getElementById(acbox_id);
    if (acbox != null) { 
      valuepair = new Array(); for (var i = 0; i < acbox.options.length; i++) if (acbox.options[i].selected) valuepair.push(acbox.options[i].value);
      //alert(valuepair);
      document.getElementById("<%=ac_prospect.clientID %>").value = valuepair;
    }
selectbox_id = querySt("m")
selectbox = window.opener.document.getElementById(selectbox_id);
if (selectbox != null) {
selected = new Array(); for (var i = 0; i < selectbox.options.length; i++) if (selectbox.options[i].selected) selected.push(selectbox.options[i].value);
createCookie("model_export",selected,365);

} 

function querySt(ji) {
hu = window.location.search.substring(1);
gy = hu.split("&");
for (i=0;i<gy.length;i++) {
    ft = gy[i].split("=");
        if (ft[0] == ji) {
            return ft[1];
        }
    }
}
  function createCookie(name, value, days) {
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000)); 
                var expires = "; expires=" + date.toGMTString();
            }   else var expires = "";
                document.cookie = name + "=" + value + expires + "; path=/";
            }
            
             function readCookie(name) {
            var nameEQ = name + "=";
            var ca = document.cookie.split(';');
                for (var i = 0; i < ca.length; i++) {
                    var c = ca[i];
                        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
                            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
                         }
                    return "";
                }
  </script>

</body>
</html>
