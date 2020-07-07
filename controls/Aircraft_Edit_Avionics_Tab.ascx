<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Aircraft_Edit_Avionics_Tab.ascx.vb"
  Inherits="crmWebClient.Aircraft_Edit_Avionics_Tab" %>
<asp:Panel ID="aircraft_edit" runat="server">
  <asp:Label runat="server" ID="title_change" CssClass="valueSpec viewValueExport Simplistic aircraftSpec"> <h2 align="right">
        Avionics Edit</h2></asp:Label>
  <asp:Label runat="server" ID="updated" Font-Bold="True" ForeColor="Red"></asp:Label>
  <div class="valueSpec Simplistic viewValueExport aircraftSpec">
    <table width="100%" cellpadding="13" cellspacing="0">
      <tr class="noBorder">
        <td colspan="4" align="right">
          <asp:Label runat="server" ID="subheaderText" CssClass="subHeader padding">Avionics Edit</asp:Label>

          <div class="Box">
            <asp:DataGrid runat="server" ID="datagrid_avionics" CellPadding="3" Width="100%"
              horizontal-align="left" EnableViewState="true" ShowFooter="false" BackColor="White"
              AllowPaging="false" PageSize="25" OnCancelCommand="MyDataGrid_Cancel" GridLines="Horizontal"
              OnUpdateCommand="MyDataGrid_Update" OnDeleteCommand="MyDataGrid_Delete" OnEditCommand="MyDataGrid_Edit"
              CssClass="formatTable blue" BorderStyle="None" AllowSorting="True" AutoGenerateColumns="false">
              <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" Font-Bold="True" Font-Underline="True"
                ForeColor="White" Mode="NumericPages" NextPageText="Next" PrevPageText="Previous" />
              <ItemStyle VerticalAlign="Top" />
              <HeaderStyle Font-Bold="True" Wrap="False" HorizontalAlign="Left" VerticalAlign="Middle"
                BorderColor="White"></HeaderStyle>
              <Columns>
                <asp:EditCommandColumn EditText="<img src='/images/edit_icon.png' width='14px'/>" UpdateText="<img src='/images/save_disk_icon.png' width='19px'  />" />
                <asp:TemplateColumn HeaderText="NAME">
                  <ItemTemplate>
                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "cliav_name")), (DataBinder.Eval(Container.DataItem, "cliav_name")), "")%>
                    <asp:TextBox runat="server" ID="name_delete" Text='<%# DataBinder.Eval(Container.DataItem, "cliav_name") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="id_delete" Text='<%# DataBinder.Eval(Container.DataItem, "cliav_cliac_id") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="description_delete" Text='<%# DataBinder.Eval(Container.DataItem, "cliav_description") %>'
                      Visible="true" Style="display: none;" />
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:DropDownList runat="server" ID="name_type">
                    </asp:DropDownList>
                    <asp:TextBox runat="server" ID="name_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cliav_name") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="id_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cliav_cliac_id") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="name_delete" Text='<%# DataBinder.Eval(Container.DataItem, "cliav_name") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="id_delete" Text='<%# DataBinder.Eval(Container.DataItem, "cliav_cliac_id") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="description_delete" Text='<%# DataBinder.Eval(Container.DataItem, "cliav_description") %>'
                      Visible="true" Style="display: none;" />
                  </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="DESCRIPTION">
                  <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "cliav_description")%></ItemTemplate>
                  <EditItemTemplate>
                    <asp:TextBox runat="server" ID="description" TextMode="MultiLine" Rows="6" Text='<%# DataBinder.Eval(Container.DataItem, "cliav_description") %>'
                      MaxLength="75" />
                    <asp:TextBox runat="server" ID="description_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cliav_description") %>'
                      Visible="true" Style="display: none;" />
                  </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                  <ItemTemplate>
                    <asp:LinkButton CommandName="Delete" runat="server" OnClientClick="return confirm('Do you really want to remove this record?');"><i class="fa fa-trash-o"></i></asp:LinkButton></ItemTemplate>
                </asp:TemplateColumn>
              </Columns>
            </asp:DataGrid></div>
        </td>
      </tr>
      <tr>
        <td colspan="4" align="right">
          <asp:LinkButton ID="add_new" CommandName="Add" Text="ADD DETAIL" runat="server"   Font-Bold="true" Font-Underline="false" />
          <asp:Panel runat="server" CssClass="gray" Visible="false" ID="new_row">
            <table width="400" cellpadding="3" cellspacing="0">
              <tr>
                <td align="left" valign="top" width="47">
                  &nbsp;
                </td>
                <td align="left" valign="top" width="140">
                  <asp:DropDownList runat="server" ID="cliav_name">
                  </asp:DropDownList>
                </td>
                <td align="left" valign="top">
                  <asp:TextBox ID="cliav_description" Width="140px" TextMode="MultiLine" Rows="6" runat="server" />
                </td>
                <td align="left" valign="top">
                  <asp:LinkButton ID="insert" CommandName="insert" Text="Insert" runat="server" />
                </td>
              </tr>
            </table>
          </asp:Panel>
        </td>
      </tr>
    </table>
  </div>
</asp:Panel>
<a onclick="javascript: window.opener.location.href = window.opener.location.href; self.close();"
  class="button">Close</a>

<script type="text/javascript" language="javascript">
        function close_fold_window(){
         window.opener.location.reload(true);
         window.close();
       }
</script>

