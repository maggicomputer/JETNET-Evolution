<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="adminDeveloper.aspx.vb"
  Inherits="crmWebClient.adminDeveloper" MasterPageFile="~/EvoStyles/CustomerAdminTheme.Master" %>

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
    <asp:UpdatePanel ID="admin_developer_panel" runat="server" ChildrenAsTriggers="True"
      UpdateMode="Conditional">
      <ContentTemplate>
        <asp:Table ID="menuTable" CellPadding="4" CellSpacing="0" Width="100%" CssClass="buttonsTable"
          runat="server">
          <asp:TableRow>
            <asp:TableCell ID="TableCell_project_table" runat="server" HorizontalAlign="left"
              VerticalAlign="middle" Style="padding-right: 4px;">
              <div style="text-align: right;">
                <asp:LinkButton ID="addTaskBtn" runat="server" PostBackUrl="~/adminDeveloper.aspx?task=add"><strong>Add New Task</strong></asp:LinkButton>
              </div>
              <br />
              <br />
              <asp:Label ID="addNewTaskLbl" runat="server" Text="* New Task Added *" ForeColor="Maroon"
                Font-Bold="True" Visible="false"></asp:Label>
              <asp:Label ID="taskingByPriorityLbl" runat="server" Text="Label"></asp:Label><br />
              <br />
              <asp:Label ID="taskingByStaffLbl" runat="server" Text="Label"></asp:Label><br />
              <br />
              <asp:Label ID="taskingSummaryLbl" runat="server" Text="Label"></asp:Label>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell ID="TableCell_details_table" runat="server" HorizontalAlign="left"
              VerticalAlign="middle" Style="padding-right: 4px;">
              <asp:Label ID="taskingDetailsLbl" runat="server" Text="Label"></asp:Label>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell ID="TableCell_add_task_table" runat="server" HorizontalAlign="left"
              VerticalAlign="middle" Style="padding-right: 4px;">
              <div style="text-align: right;">
                <asp:LinkButton ID="insertTaskBtn" runat="server" PostBackUrl="~/adminDeveloper.aspx?task=submit"><strong>Submit New Task</strong></asp:LinkButton>
              </div>
              <br />
              <table width="100%" border="0" cellpadding="2" cellspacing="0">
                <tr>
                  <td align="left" valign="middle" colspan="2">
                    NEW TASK DETAILS<hr />
                  </td>
                </tr>
                <tr>
                  <td valign="top">
                    Task Entry Staff:
                  </td>
                  <td valign="top">
                    Jackie Ciccone<asp:HiddenField ID="staff_entry_name" runat="server" Value="Jackie Ciccone" />
                  </td>
                </tr>
                <tr>
                  <td width="25%" valign="top">
                    Task Project Title:
                  </td>
                  <td width="72%" valign="top">
                    <asp:DropDownList ID="projectKeyDDl" runat="server">
                      <asp:ListItem Text="Project Title" Value=""></asp:ListItem>
                    </asp:DropDownList>
                  </td>
                </tr>
                <tr>
                  <td width="25%" valign="top">
                    Task Assigned To:
                  </td>
                  <td width="72%" valign="top">
                    Rick Wanner<asp:HiddenField ID="staff_name" runat="server" Value="Rick Wanner" />
                  </td>
                </tr>
                <tr>
                  <td valign="top">
                    Task Title:
                  </td>
                  <td valign="top">
                    <asp:TextBox ID="task_title" runat="server" Width="250"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td valign="top">
                    Task Description:
                  </td>
                  <td valign="top">
                    <asp:TextBox ID="task_description" runat="server" TextMode="MultiLine" Rows="8" Columns="80"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td valign="top">
                    Task Status:
                  </td>
                  <td valign="top">
                    New Unassigned<asp:HiddenField ID="task_status" runat="server" Value="N" />
                  </td>
                </tr>
                <tr>
                  <td valign="top">
                    Task Priority:
                  </td>
                  <td valign="top">
                    <asp:DropDownList ID="projectPriorityDDL" runat="server">
                      <asp:ListItem Text="Priority" Value=""></asp:ListItem>
                    </asp:DropDownList>
                  </td>
                </tr>
                <tr>
                  <td valign="top">
                    Task Entry Date:
                  </td>
                  <td valign="top">
                    <%= Now().ToShortDateString.Trim%>
                  </td>
                </tr>
                <tr>
                  <td valign="top">
                    Task Entered For:
                  </td>
                  <td valign="top">
                    <asp:TextBox ID="task_follow_up" runat="server" TextMode="MultiLine" Rows="8" Columns="80"></asp:TextBox>
                  </td>
                </tr>
              </table>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
      </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
