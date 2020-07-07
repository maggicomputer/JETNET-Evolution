<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/CustomerAdminTheme.Master"
  CodeBehind="adminBackground.aspx.vb" Inherits="crmWebClient.adminBackground" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/EvoStyles/CustomerAdminTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script language="javascript" type="text/javascript">
    var bDontClose = false;

    function ActiveTabChanged(sender, args) { }

    function openSmallWindowJS(address, windowname) {

      var rightNow = new Date();
      windowname += rightNow.getTime();
      var Place = open(address, windowname, "menubar,scrollbars=1,resizable,width=900,height=600");

      return true;
    }

    var lblFileUploadID = "backgroundDisplayLabel";
    var bIsEdit = <%= bShowBackgroundDetails.toString.toLower %>;

  </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <script type="text/javascript">    

    if (bIsEdit) {
      $(document).ready(function() {
        $("#" + lblFileUploadID).hide();

        var backgroundDateLbl = $("#backgroundDateID");
        backgroundDateLbl.html("");

        $("<div/>", {
          html: "Background Update Date:"
        }).appendTo(backgroundDateLbl);
      
      });
    }

  </script>

  <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="">
    <ProgressTemplate>
      <div id="divLoading" runat="server" style="text-align: center; font-weight: bold;
        background-color: #eeeeee; filter: alpha(opacity=90); opacity: 0.9; width: 395px;
        height: 295px; text-align: center; padding: 75px; position: absolute; border: 1px solid #003957;
        z-index: 10; margin-left: 225px;">
        <span>Please wait ... </span>
        <br />
        <br />
        <img src="/images/loading.gif" alt="Loading..." /><br />
      </div>
    </ProgressTemplate>
  </asp:UpdateProgress>
  <div style="text-align: left; padding-top: 8px;">
    <asp:UpdatePanel ID="admin_background_panel" runat="server" ChildrenAsTriggers="True"
      UpdateMode="Conditional">
      <ContentTemplate>
        <asp:Table ID="menuTable" CellPadding="4" CellSpacing="0" Width="100%" CssClass="buttonsTable"
          runat="server">
          <asp:TableRow>
            <asp:TableCell ID="TableCell0" runat="server" HorizontalAlign="left" VerticalAlign="middle"
              Style="padding-right: 4px;">
              Status :
              <asp:DropDownList ID="backgroundByStatus" runat="server" AutoPostBack="true">
                <asp:ListItem Text="ALL" Value=""></asp:ListItem>
                <asp:ListItem Text="Active" Value="true" Selected="True"></asp:ListItem>
              </asp:DropDownList>
              Product :
              <asp:DropDownList ID="backgroundByProduct" runat="server" AutoPostBack="true">
                <asp:ListItem Text="ALL" Value="" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Aerodex" Value="A"></asp:ListItem>
                <asp:ListItem Text="Business" Value="B"></asp:ListItem>
                <asp:ListItem Text="Commercial" Value="C"></asp:ListItem>
                <asp:ListItem Text="Helicopters" Value="H"></asp:ListItem>
                <asp:ListItem Text="Featured" Value="F"></asp:ListItem>
                <asp:ListItem Text="Yachts" Value="Y"></asp:ListItem>
              </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell ID="TableCell01" runat="server" HorizontalAlign="right" VerticalAlign="middle"
              Style="padding-right: 4px;">
              <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/adminBackground.aspx?task=add"><strong>Add New Background</strong></asp:LinkButton>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell ID="TableCell_background_list" runat="server" HorizontalAlign="left"
              VerticalAlign="middle" Style="padding-right: 4px;" ColumnSpan="2"><div class="Box">
              <asp:Label ID="addNewBackgroundLbl" runat="server" Text=""
                ForeColor="Maroon" Font-Bold="True" Visible="false"></asp:Label>
              <asp:Label ID="backgroundDisplayLbl" runat="server" Text="Label"></asp:Label></div>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell ID="TableCell_add_background_table" runat="server" HorizontalAlign="left"
              VerticalAlign="middle" Style="padding-right: 4px;" ColumnSpan="2">
              <div style="text-align: right;">
                <asp:LinkButton ID="updateBackgroundBtn" runat="server" PostBackUrl=""><strong>Update Background</strong></asp:LinkButton>
                &nbsp;&nbsp;<asp:LinkButton ID="insertBackgroundBtn" runat="server" PostBackUrl=""><strong>Submit New Background</strong></asp:LinkButton>
              </div>
              <br />
              <table width="100%" border="0" cellpadding="2" cellspacing="0">
                <tr>
                  <td align="center" valign="middle" rowspan="9" colspan="6">
                    <asp:Image ID="backgroundImage" runat="server" ImageUrl="" Width="420" Height="340"
                      AlternateText="" ToolTip=""  BorderColor="Black" BorderStyle="Solid" BorderWidth="1"/>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="middle" colspan="6">
                    <asp:Label ID="backgroundTableTitle" runat="server" Text="Label"></asp:Label><hr />
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="middle">
                    Background Title:
                  </td>
                  <td align="left" valign="middle" colspan="5">
                    <asp:TextBox ID="background_title" runat="server" TextMode="MultiLine" Rows="2" Columns="80"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="middle">
                    Background Link :
                  </td>
                  <td align="left" valign="middle" colspan="5">
                    <asp:TextBox ID="background_link" runat="server" Width="400" Visible="true" Text=""
                      Enabled="false"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="middle">
                    <div id="backgroundDisplayLabel" style="display: inline;">
                      Background File :
                    </div>
                  </td>
                  <td align="left" valign="middle" colspan="5">
                    <asp:FileUpload ID="background_fileLink" runat="server" Width="400" Visible="true" />
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="middle">
                    Background Status:
                  </td>
                  <td align="left" valign="middle" colspan="5">
                    <asp:CheckBox ID="background_statusChk" runat="server" Checked="true" />
                  </td>
                </tr>
                <tr>
                  <td align="right" valign="middle">
                    Business:
                  </td>
                  <td align="left" valign="middle">
                    <asp:CheckBox ID="background_busChk" runat="server" />
                  </td>
                  <td align="right" valign="middle">
                    Commercial:
                  </td>
                  <td align="left" valign="middle">
                    <asp:CheckBox ID="background_commChk" runat="server" />
                  </td>
                  <td align="right" valign="middle">
                    Helicopters:
                  </td>
                  <td align="left" valign="middle">
                    <asp:CheckBox ID="background_heliChk" runat="server" />
                  </td>
                </tr>
                <tr>
                  <td align="right" valign="middle">
                    Aerodex:
                  </td>
                  <td align="left" valign="middle">
                    <asp:CheckBox ID="background_aeroChk" runat="server" />
                  </td>
                  <td align="right" valign="middle">
                    Yachts:
                  </td>
                  <td align="left" valign="middle">
                    <asp:CheckBox ID="background_yachtsChk" runat="server" />
                  </td>
                  <td align="right" valign="middle">
                    Featured:
                  </td>
                  <td align="left" valign="middle">
                    <asp:CheckBox ID="background_featurChk" runat="server" />
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="middle">
                    <div id="backgroundDateID" style="display: inline;">
                      Background Entry Date:</div>
                  </td>
                  <td align="left" valign="middle" colspan="5">
                    <%= Now().ToShortDateString.Trim%>
                  </td>
                </tr>
              </table>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</asp:Content>
