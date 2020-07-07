<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="adminHelp.aspx.vb" Inherits="crmWebClient.adminHelp"
  MasterPageFile="~/EvoStyles/CustomerAdminTheme.Master" ValidateRequest="false" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ MasterType VirtualPath="~/EvoStyles/CustomerAdminTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script type="text/javascript">
    var bDontClose = false;

    function ActiveTabChanged(sender, args) { }

    function openSmallWindowJS(address, windowname) {

      var rightNow = new Date();
      windowname += rightNow.getTime();
      var Place = open(address, windowname, "menubar,scrollbars=1,resizable,width=900,height=600");

      return true;
    }

    var lblModelListID = "makeModelDisplayLabel";
    var dllModelListID = "makeModelDisplayDDL";

    var lblViewID = "viewDisplayLabel";
    var tbxViewID = "viewDisplayTextBox";

    var lblTabID = "tabDisplayLabel";
    var tbxTabID = "tabDisplayTextBox";

    var lblFileUploadID = "helpFileDisplayLabel";

    var bIsEdit = <%= bEditHelpItem.toString.toLower %>;
    var bHasModel = <%= bHasHelpModelItem.toString.toLower %>;
    var bHasHint = <%= bHasHelpHintItem.ToString.ToLower %>;

  </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <script type="text/javascript">

    if (bIsEdit) {
      $(document).ready(function () { $("#" + lblFileUploadID).hide(); });

      if (bHasModel || bHasHint) {
        $(document).ready(function () { ListEnableItem(); });
      }

    }

  </script>

  <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="">
    <ProgressTemplate>
      <div id="divLoading" runat="server" style="text-align: center; font-weight: bold; background-color: #eeeeee; filter: alpha(opacity=90); opacity: 0.9; width: 395px; height: 295px; text-align: center; padding: 75px; position: absolute; border: 1px solid #003957; z-index: 10; margin-left: 225px;">
        <span>Please wait ... </span>
        <br />
        <br />
        <img src="/images/loading.gif" alt="Loading..." /><br />
      </div>
    </ProgressTemplate>
  </asp:UpdateProgress>
  <div style="text-align: left; padding-top: 8px;">
    <asp:UpdatePanel ID="admin_help_panel" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
      <ContentTemplate>
        <strong>Evolution Help Center</strong>
        <asp:Table ID="menuTable" CellPadding="2" CellSpacing="0" Width="100%" CssClass="buttonsTable" runat="server">
          <asp:TableRow>
            <asp:TableCell ID="TableCell0" runat="server" HorizontalAlign="left" VerticalAlign="top" Width="30%">
              <br /><asp:Label ID="adminHelpListLbl" runat="server"></asp:Label>
            </asp:TableCell>
            <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="left" VerticalAlign="top">
              <asp:Panel runat="server" ID="adminDetailHelpListPnl" HorizontalAlign="Left" VerticalAlign="middle">
                <asp:Label ID="adminDetailHelpListLbl" runat="server" Text="Please select item on left to view a detailed list." ForeColor="Black"></asp:Label>
              </asp:Panel>
              <asp:Panel runat="server" ID="adminDetailHelpItemPnl" HorizontalAlign="Left">
                <div style="text-align: right;">
                  <asp:LinkButton ID="submitItemBtn" runat="server" PostBackUrl=""><strong>Submit Help Item</strong></asp:LinkButton>&nbsp;&nbsp;
                  <asp:LinkButton ID="deleteItemBtn" runat="server" OnClientClick="return confirm('Are you sure you want to Remove this Help Item?');">Delete Help Item</asp:LinkButton>
                </div>
                <br />
                <table width="100%" border="0" cellpadding="3" cellspacing="0">
                  <tr>
                    <td align="left" valign="middle" colspan="4"><strong><asp:label ID="helpDetailsLabel" runat="server" Text="HELP ITEM DETAILS"></asp:label></strong><hr />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="middle">Item Status :
                    </td>
                    <td align="left" valign="middle">
                      <asp:CheckBox ID="helpItemStatusChk" runat="server" Text="Active" />
                    </td>
                    <td align="left" valign="middle">Admin Only? :
                    </td>
                    <td align="left" valign="middle">
                      <asp:CheckBox ID="helpItemAdminOnly" runat="server" Text="Admin Only" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="middle">Sub ID :
                    </td>
                    <td align="left" valign="middle">
                      <asp:TextBox ID="helpItemSubID" runat="server" Width="150"></asp:TextBox>
                    </td>
                    <td align="left" valign="middle">Company ID :
                    </td>
                    <td align="left" valign="middle">
                      <asp:TextBox ID="helpItemCompanyID" runat="server" Width="150"></asp:TextBox>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="middle">Release Date :
                    </td>
                    <td align="left" valign="middle" colspan="3">
                      <asp:TextBox ID="helpItemReleaseDate" runat="server" Width="150"></asp:TextBox>
                    </td>
                  </tr>

                  <tr>
                    <td align="left" valign="middle">
                      <div id="viewDisplayLabel" style="display: none;">
                        View[#] :
                      </div>
                    </td>
                    <td align="left" valign="middle" colspan="3">
                      <div id="viewDisplayTextBox" style="display: none;">
                        <asp:TextBox ID="helpItemViewNumber" runat="server" Width="150" ToolTip="Only enter if the hint is designed to display on a specific view."></asp:TextBox>
                      </div>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="middle">
                      <div id="tabDisplayLabel" style="display: none;">
                      Tab Name :
                      </div>
                    </td>
                    <td align="left" valign="middle" colspan="3">
                      <div id="tabDisplayTextBox" style="display: none;">
                      <asp:TextBox ID="helpItemTabName" runat="server" Width="150"></asp:TextBox>
                      </div>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" width="20%" valign="middle">Release Type :
                    </td>
                    <td align="left" width="80%" valign="middle" colspan="3">
                      <asp:DropDownList ID="helpItemTypeDDl" runat="server">
                        <asp:ListItem Text="Release Type" Value=""></asp:ListItem>
                      </asp:DropDownList>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="middle">Release Title :
                    </td>
                    <td align="left" valign="middle" colspan="3">
                      <asp:TextBox ID="helpItemReleaseTitle" runat="server" Width="390"></asp:TextBox>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="middle">
                      <div id="makeModelDisplayLabel" style="display: none;">
                        Make / Model :
                      </div>
                    </td>
                    <td align="left" valign="middle" colspan="3">
                      <div id="makeModelDisplayDDL" style="display: none;">
                        <asp:DropDownList ID="MakeModelDDL" runat="server" Enabled="true">
                          <asp:ListItem Text="Make Model name" Value=""></asp:ListItem>
                        </asp:DropDownList>
                      </div>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      <asp:Label ID="rel_label" runat="server" Text="Release Description:"></asp:Label>
                      <asp:Label ID="image_url_label" runat="server" Text="Image URL:" Visible="false"></asp:Label>
                    </td>
                    <td align="left" valign="middle" colspan="3">
                      <CKEditor:CKEditorControl ID="helpItemReleaseDescription" BasePath="~/ckeditor/"
                        runat="server" Rows="8" Columns="80">
                      </CKEditor:CKEditorControl>
                      <asp:Panel runat="server" ID="help_panel" Visible="false">
                        <table width="100%" border="0" cellpadding="2" cellspacing="0">
                          <tr>
                            <td>
                              <asp:TextBox ID="helpItemReleaseDescription_R" runat="server" Rows="1" Columns="80"
                                Visible="false"></asp:TextBox>
                            </td>
                          </tr>
                          <tr>
                            <td>IMG Height:&nbsp;<asp:TextBox ID="pop_height" runat="server"></asp:TextBox>
                            </td>
                          </tr>
                          <tr>
                            <td>IMG Width :&nbsp;<asp:TextBox ID="pop_width" runat="server"></asp:TextBox>
                            </td>
                          </tr>
                        </table>
                      </asp:Panel>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="middle">Video Link :
                    </td>
                    <td align="left" valign="middle" colspan="3">
                      <asp:Panel runat="server" ID="video_panel">
                        <asp:TextBox ID="helpItemVideoLink" runat="server" TextMode="MultiLine" Rows="8"
                          Columns="80"></asp:TextBox>
                      </asp:Panel>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="middle">JETNET Products :
                    </td>
                    <td align="left" valign="middle" colspan="3">
                      <table width="100%" border="0" cellpadding="2" cellspacing="0">
                        <tr>
                          <td>
                            <asp:CheckBox ID="helpItemBusChk" runat="server" Text="Business" />
                          </td>
                          <td>
                            <asp:CheckBox ID="helpItemHeliChk" runat="server" Text="Helicopter" />
                          </td>
                          <td>
                            <asp:CheckBox ID="helpItemComChk" runat="server" Text="Commercial" />
                          </td>
                          <td>
                            <asp:CheckBox ID="helpItemYchtChk" runat="server" Text="Yacht" Visible="False" />
                          </td>
                        </tr>
                        <tr>
                          <td>
                            <asp:CheckBox ID="helpItemNewEvoChk" runat="server" Text="New Evolution" />
                          </td>
                          <td>
                            <asp:CheckBox ID="helpItemNewEvoOnlyChk" runat="server" Text="New Evolution(only)" />
                          </td>
                          <td>
                            <asp:CheckBox ID="helpItemOldEvoChk" runat="server" Text="Old Evolution" />
                          </td>
                          <td>
                            <asp:CheckBox ID="helpItemCRMChk" runat="server" Text="CRM" />
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="middle">Help Document Link :
                    </td>
                    <td align="left" valign="middle" colspan="3">
                      <asp:TextBox ID="helpItemDocumentLink" runat="server" Width="490" Visible="true"></asp:TextBox>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="middle">
                      <div id="helpFileDisplayLabel" style="display: inline;">
                        Help Document File :
                      </div>
                    </td>
                    <td align="left" valign="middle" colspan="3">
                      <asp:FileUpload ID="helpItemDocumentFileLink" runat="server" Width="490" Visible="true" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="middle">Help Item Topic :
                    </td>
                    <td align="left" valign="middle" colspan="3">
                      <asp:CheckBoxList ID="helpItemTopicCBL" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                        RepeatLayout="Table">
                        <asp:ListItem Text="Help Topic" Value=""></asp:ListItem>
                      </asp:CheckBoxList>
                    </td>
                  </tr>
                </table>
              </asp:Panel>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</asp:Content>
