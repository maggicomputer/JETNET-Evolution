<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="Short_Spec_Sheet.aspx.vb"
  Inherits="crmWebClient.short_spec_sheet_aspx" %>

<!doctype html public "-//w3c//dtd html 4.0 transitional//en">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="casHead" runat="server">
  <title>Condensed Aircraft Spec Sheet</title>
  <meta http-equiv="Content-Language" content="en-us" />
  <meta http-equiv="Content-Type" content="text/html; charset=windows-1252" />
  <meta name="apple-mobile-web-app-capable" content="yes" />
  <meta name="format-detection" content="telephone=yes" />
  <link href="commonNet/my_evo_style.css" type="text/css" rel="stylesheet" />
  <link href="css/ipad-landscape.css" rel="stylesheet" media="all and (min-device-width: 481px) and (max-device-width: 1024px) and (orientation:portrait)" />
  <link href="css/ipad-landscape.css" rel="stylesheet" media="all and (min-device-width: 481px) and (max-device-width: 1024px) and (orientation:landscape)" />
  <link href="css/regular.css" rel="stylesheet" media="all and (min-device-width: 1024px)" />
</head>    

  <script runat="Server">
    Sub Check_Clicked(ByVal sender As Object, ByVal e As EventArgs)
      If Me.WD.SelectedValue = "Word" Then
         Me.HelpText2.Text = "<font color='red'>When the Word document is opened in the browser, select File, Save As, and select Word Document as the type.</font>"
      Else
         Me.HelpText2.Text = ""
      End If
    End Sub
  </script>

<body id="casBodyID" runat="server" class='bg_image_ie' style='background-image: url(images/background/11.jpg); margin-top: 5px;'>

  <form id="casIDForm" runat="server">
  <div id="outerDivCasIDForm" runat="server" class="center_outer_div" width="1000">
    <table class="centerTable" id="mainTableID" width='80%' border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td align="center" style="text-align:center; padding-left: 0px;">
          <table class="centerTableShadow" border="0" cellpadding="0" cellspacing="0" width="50%">
            <!-- This is the Title Section -->
           <tr>
            <td align="center" style="text-align:center; padding-left: 0px;">
              <asp:Label ID="makemodelname" runat="server" Text="Label"></asp:Label>
            </td>
          </tr>
           <tr>
              <td style="text-align:center; background-color: #EEEEEE;">
                <b>Report Sections to Include:</b>
              </td>
            </tr>
            <!-- This is the Title Section -->
            <!-- This is the Cover Page Checkbox Section -->
            <tr>
              <td width="100%" style="text-align: center;">
                <asp:CheckBox ID="BR" Checked="FALSE" runat="server" Text=' Blind Report (No Serial/Reg# Information) ' />
              </td>
            </tr>
            <!-- This is the Cover Page Checkbox Section -->
            <tr>
              <td style="text-align: center;">
                <table class="centerTable" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td valign='top'>
                      <br />
                      Type of Document:<br />
                      <asp:RadioButtonList ID="WD"  OnSelectedIndexChanged="Check_Clicked" AutoPostBack="True" runat="server">
                        <asp:ListItem Value="Word" Text="Word" />
                        <asp:ListItem Value="PDF" Text="PDF" Selected="True" />
                      </asp:RadioButtonList>
                       <asp:Label ID="HelpText2" runat="server" Text=""></asp:Label>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr id='trInner_viewToPdf_SelectedReport_RunReport'>
              <td id='tdInner_viewToPdf_SelectedReport_RunReport' align="center">
                <asp:Button ID="btnRunReport" runat="server" Text="Run Report" Style="height: 26px"
                  Height="26px" />
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </div>
  </form>
</body>
</html>
