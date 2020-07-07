<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="adminMPM.aspx.vb" Inherits="crmWebClient.adminMPM"
  MasterPageFile="~/EvoStyles/CustomerAdminTheme.Master" %>

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
  
  </script>
  <link rel="Stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.24/themes/smoothness/jquery-ui.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

    <asp:UpdatePanel ID="admin_mph_panel" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
      <ContentTemplate> 
        <asp:Table ID="menuTable" CellPadding="4" CellSpacing="0" Width="100%" CssClass="buttonsTable"
          runat="server">
          <asp:TableRow ID="TableRow0" runat="server">
            <asp:TableCell ID="TableCell0" runat="server" HorizontalAlign="left" VerticalAlign="middle"
              Style="padding-right: 4px;">
              Type of Select :
              <asp:DropDownList ID="mpmDisplayType" runat="server" AutoPostBack="true">
                <asp:ListItem Text="Error List" Value="ERROR" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Connections" Value="CONN"></asp:ListItem>
                <asp:ListItem Text="Client Data" Value="DATA"></asp:ListItem>
                <asp:ListItem Text="Current Users" Value="USERS"></asp:ListItem>
              </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell ID="TableCell01" runat="server" HorizontalAlign="right" VerticalAlign="top" Style="padding-right: 4px;">
              <asp:LinkButton ID="showAllUsers" runat="server" PostBackUrl="~/adminMPM.aspx?show_all=true"><strong>Show All Clients</strong></asp:LinkButton>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow ID="TableRow1" runat="server">
            <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="left" VerticalAlign="top"
              Style="padding-right: 4px;" ColumnSpan="2">
              <div style="text-align: left;">
                <asp:Label runat="server" ID="user_info1"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="show_type" runat="server" AutoPostBack="true">
                <asp:ListItem Value="Active">Show Only Active Users</asp:ListItem>
                <asp:ListItem Value="All">Show All Users</asp:ListItem>
                </asp:DropDownList>
                <asp:Label runat="server" ID="refresh" Text=""></asp:Label>
              </div>
              <div style="text-align: right;"> 
                <asp:LinkButton ID="addNewClientUser" runat="server" PostBackUrl=""><strong>Add New User</strong></asp:LinkButton>
              </div>
              <br />
              <asp:Label ID="mpm_data_list_display" runat="server" Text=""></asp:Label> 
              <asp:Label ID="Bottom_label" runat="server" Text="" Visible="false"></asp:Label>
             </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow ID="TableRow2" runat="server">
            <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="left" VerticalAlign="middle"
              Style="padding-right: 4px;" ColumnSpan="2">
              <div style="text-align: right;">
                <asp:LinkButton ID="submitNewClientUser" runat="server" PostBackUrl=""><strong>Submit New user</strong></asp:LinkButton>
              </div>
              <br />
              <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                  <td colspan="2" height="18">
                    <asp:Label ID="domainName" runat="server" Text=""></asp:Label>
                  </td>
                </tr>
                <tr>
                  <td width="20%">
                    First Name:
                  </td>
                  <td>
                    <asp:TextBox runat="server" ID="firstName" Columns="20" row="1"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td>
                    Last Name:
                  </td>
                  <td>
                    <asp:TextBox runat="server" ID="lastName" Columns="20" row="1"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td>
                    Login (password):
                  </td>
                  <td>
                    <asp:TextBox runat="server" ID="loginString" Columns="20" row="1"></asp:TextBox>&nbsp;&nbsp;<asp:Image ID="actinfo_password_mouseover_img" Height="15px" runat="server"
                        ImageUrl="/images/info.png" />
                  </td>
                </tr>
                <tr>
                  <td>
                    Email:
                  </td>
                  <td>
                    <asp:TextBox runat="server" ID="emailAddress" Columns="20" row="1"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td>
                    Admin:
                  </td>
                  <td>
                    <asp:CheckBox runat="server" ID="isAdmin" />
                  </td>
                </tr>
              </table>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
      </ContentTemplate>
    </asp:UpdatePanel>

    <div id="DivMPMAdminMessage" style="display: none;">
  </div>

  <script type="text/javascript">
 
    function validatePassword() {
      var txttext = document.getElementById("<%= loginString.ClientID.ToString %>").value;
      var regex = /^(?=.*[0-9]+.*)(?=.*[a-zA-Z]+.*)[0-9a-zA-Z]{8,15}$/;

      if (eval(regex.test(txttext)) == false && txttext != '') {
        ShowMPMAdminMessage('DivMPMAdminMessage', 'Password Error', 'Your new password should be a minimum of 8 characters in length and must contain at least ""one number"" ...');
        document.getElementById("<%=  loginString.ClientID.ToString%>").focus();
      }
    }

    function ShowMPMAdminMessage(DivTag, Title, Message) {
      $("#" + DivTag).html(Message);
      $("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 75, resizable: false });
    }

    function CloseMPMAdminMessage(DivTag) {
      $("#" + DivTag).dialog("close");
    }

  </script>
  
</asp:Content>
