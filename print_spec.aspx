<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="print_spec.aspx.vb" Inherits="crmWebClient.print_spec" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <title>Marketplace Manager – Spec Sheet Report</title>
  <link href="common/redesign.css" rel="stylesheet" type="text/css" />
  <link rel="stylesheet" type="text/css" href="common/anylinkmenu.css" />

  <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.2.min.js"></script>

  <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.24/jquery-ui.min.js"></script>

  <script language="javascript" type="text/javascript" src="https://www.google.com/jsapi?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE"></script>

</head>
<body>

  <script type="text/javascript">
    google.load('visualization', '1', { packages: ['corechart'] });
  </script>

  <form id="form1" runat="server">
  <div>
    <asp:Panel ID="aircraft_edit" runat="server" BackColor="White" CssClass="edit_panel">
      <h4 align="right">
        <asp:Label ID="ac_mod_name_ser" runat="server" Text=""></asp:Label>&nbsp;Spec Sheet
        Generator</h4>
      <table border="0" cellpadding="3" cellspacing="0" width="100%">
        <tr>
          <td>
            <b>Sections to Include:</b>
          </td>
        </tr>
        <tr>
          <td align="left">
            <asp:CheckBox ID="CP" Checked="true" runat="server" Text=' Cover Page ' Visible="false" />
          </td>
        </tr>
        <tr>
          <td align="left">
            <asp:CheckBox ID="SP" Checked="true" runat="server" Text=' Specifications Page '
              Visible="false" />
          </td>
        </tr>
        <tr>
          <td align="left">
            <asp:CheckBox ID="MVA" Checked="true" runat="server" Text=' Market Value Analysis '
              Visible="false" />
              <asp:label id="mva_label" runat="server" text="Show Latest:"></asp:Label>
               <asp:DropDownList ID="mva_months" runat="server">
               <asp:ListItem Text="All" Value="120"></asp:ListItem>
                <asp:ListItem Text="2 Years" Value="24"></asp:ListItem>
                <asp:ListItem Text="1 Year" Value="12"></asp:ListItem>
              <asp:ListItem Text="6 Months" Value="6"></asp:ListItem> 
            </asp:DropDownList>
          </td>
        </tr>
        <tr>
          <td align="left">
            <asp:CheckBox ID="TP" Checked="true" runat="server" Text=' Company/Contacts ' Visible="true" />
            <asp:Label ID="break_point_label1" runat="server" Text="<br />&nbsp;&nbsp;" Visible="false"></asp:Label>
            <asp:CheckBox ID="CD" Checked="true" runat="server" Text=' Include Contact Details ' Visible="False" />
          </td>
        </tr>  
        <tr>
          <td align="left">
            <asp:CheckBox ID="NP" Checked="true" runat="server" Text=' Notes ' Visible="true" />
            <asp:Label ID="break_point_label" runat="server" Text="<br />&nbsp;&nbsp;" Visible="false"></asp:Label>
            <asp:CheckBox ID="sales_format" Checked="false" runat="server" Text=' Sales Inquiry Format '
              Visible="true" />
          </td>
        </tr>
        <tr>
          <td align="left">
            <asp:CheckBox ID="PR" Checked="true" runat="server" Text=' Prospects ' Visible="true" />
            <asp:DropDownList ID="prospect_type" runat="server">
              <asp:ListItem Text="My Aircraft" Value="AC"></asp:ListItem>
              <asp:ListItem Text="My Aircraft or Model (but not other Aircraft)" Value="ACMODEL"></asp:ListItem>
              <asp:ListItem Text="All Model Prospects" Value="MODEL"></asp:ListItem>
            </asp:DropDownList>
          </td>
        </tr>
        <tr>
          <td align="left">
            <asp:CheckBox ID="PP" Checked="true" runat="server" Text=' Picture Page ' Visible="false" />          
           <asp:Label id="pic_break" runat="server" visible="False" text="<br>&nbsp;&nbsp;&nbsp;"></asp:Label> 
            <asp:CheckBox ID="chkPP_Large" runat="server" Text=" Show Large Pictures" />
          </td>
        </tr>
        <tr>
          <td align="left">
            <asp:CheckBox ID="PH" Checked="true" runat="server" Text=' Price History ' Visible="true" />
          </td>
        </tr>
        <tr>
          <td align="left">
            <asp:CheckBox ID="BR" Checked="false" runat="server" Text=' Blind Report (No Serial/Reg# Information) ' />
          </td>
        </tr>
        <tr>
          <td align="left">
            <asp:CheckBox ID="MSV" Checked="true" runat="server" Text="Market Survey" Visible="false" />
          </td>
        </tr>
        <tr>
          <td align="left">
            <asp:CheckBox ID="CMC" Checked="true" runat="server" Text='Market Comparables' Visible="false" />
          </td>
        </tr>
        <tr>
          <td align="left">
            <asp:CheckBox ID="MSA" Checked="true" runat="server" Text="Market Status" Visible="false" />
          </td>
        </tr>
        <tr>
          <td align="left">
            <asp:CheckBox ID="MTR" Checked="true" runat="server" Text="Market Trends" Visible="false" />
          </td>
        </tr>
        <tr>
          <td align="left">
            <asp:CheckBox ID="RS" Checked="true" runat="server" Text="Sold Survey" Visible="false" />
          </td>
        </tr>
        <tr>
          <td align="left">
            <asp:CheckBox ID="SC" Checked="true" runat="server" Text='Sold Comparables' Visible="false" />
          </td>
        </tr>
        <tr>
          <td>
            <asp:CheckBox ID="logo_check" runat="server" Visible="False" Checked="True" Text=" Include My Company Logo in Header of Report "
              ToolTip="Include Logo" />
          </td>
        </tr>
        <tr>
          <td>
            <!--Just in case we ever want pdf-->
            <asp:RadioButtonList Visible="false" ID="WD" runat="server">
              <asp:ListItem Value="Word" Text="Word" Selected />
              <asp:ListItem Value="PDF" Text="PDF" />
            </asp:RadioButtonList>
          </td>
        </tr>
        <tr>
          <td align="right">
            <asp:Button ID="btnRunReport" runat="server" Text="Run Report" />
          </td>
        </tr>
      </table>
    </asp:Panel>
  </div>
  <asp:Chart ID="ANALYTICS_HISTORY" Visible="False" runat="server" ImageStorageMode="UseImageLocation"
    ImageType="Jpeg">
    <Series>
      <asp:Series Name="Series1" ChartArea="ChartArea1">
      </asp:Series>
    </Series>
    <ChartAreas>
      <asp:ChartArea Name="ChartArea1">
      </asp:ChartArea>
    </ChartAreas>
  </asp:Chart>
  <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
  </cc1:ToolkitScriptManager>
  <asp:UpdatePanel ID="bottom_tab_update_panel" runat="server" ChildrenAsTriggers="true">
    <ContentTemplate>
      <div id="chart_div_value_history" runat="server">
      </div>
      <div id="chart_div_survey" runat="server">
      </div>
    </ContentTemplate>
  </asp:UpdatePanel>
  <asp:Label runat="server" Visible="false" ID="dummy_label"></asp:Label>
  <asp:Label runat="server" Visible="false" ID="use_this_label"></asp:Label>
  <cc1:TabContainer ID="tabcontainer1" runat="server" Visible="false">
    <cc1:TabPanel runat="server">
      <ContentTemplate>
      </ContentTemplate>
    </cc1:TabPanel>
  </cc1:TabContainer>
  </form>
</body>
</html>
