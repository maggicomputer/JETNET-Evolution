<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Aircraft_Edit_Features_Tab.ascx.vb"
  Inherits="crmWebClient.Aircraft_Edit_Features_Tab" %>
<asp:Panel ID="aircraft_edit" runat="server">
  <asp:Label runat="server" ID="title_change" CssClass="valueSpec viewValueExport Simplistic aircraftSpec"> <h2 align="right">
        Features Edit</h2></asp:Label>
  <div class="valueSpec Simplistic viewValueExport aircraftSpec">
    <div class="Box">
      <table width="100%" cellpadding="3" cellspacing="0">
        <tr class="noBorder">
          <td align="right">
            <asp:Label runat="server" ID="subheaderText" CssClass="subHeader padding_left">Features Edit</asp:Label><br />
          </td>
        </tr>
        <tr> 
          <td align="left" valign="top">
            <asp:Label ID="message" ForeColor="Red" Font-Bold="true" runat="server" Text=""></asp:Label>
            <asp:DataGrid runat="server" ID="datagrid_features" CellPadding="3" horizontal-align="left"
              OnUpdateCommand="MyDataGrid_Update" EnableViewState="true" ShowFooter="false" BackColor="White" BorderStyle="None"
              Width="100%" OnDeleteCommand="MyDataGrid_Delete" AllowPaging="false"
              PageSize="25" CssClass="formatTable blue" OnEditCommand="MyDataGrid_Edit" OnCancelCommand="MyDataGrid_Cancel"
              AllowSorting="True" AutoGenerateColumns="false" GridLines="Horizontal">
              <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" Font-Bold="True"
                Font-Underline="True" ForeColor="White" Mode="NumericPages" NextPageText="Next"
                PrevPageText="Previous" />
              <ItemStyle VerticalAlign="Top"  />
              <HeaderStyle Font-Bold="True" BorderColor="White" Wrap="False" HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
              <Columns>
                <asp:EditCommandColumn  EditText="<img src='/images/edit_icon.png' width='14px'/>" UpdateText="<img src='/images/save_disk_icon.png' width='19px' />" />
                <asp:TemplateColumn HeaderText="NAME">
                  <ItemTemplate>
                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "clikfeat_name")), (DataBinder.Eval(Container.DataItem, "clikfeat_name")), "")%>
                    <asp:TextBox runat="server" ID="name_delete" Text='<%# DataBinder.Eval(Container.DataItem, "cliafeat_type") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="id_delete" Text='<%# DataBinder.Eval(Container.DataItem, "cliafeat_cliac_id") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="description_delete" Text='<%# DataBinder.Eval(Container.DataItem, "cliafeat_flag") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox ID="seq_delete" Style="display: none;" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cliafeat_seq_nbr") %>'></asp:TextBox>
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:DropDownList runat="server" ID="name_type" Width="250">
                    </asp:DropDownList>
                    <asp:TextBox runat="server" ID="id_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cliafeat_cliac_id") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="name_delete" Text='<%# DataBinder.Eval(Container.DataItem, "cliafeat_type") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="id_delete" Text='<%# DataBinder.Eval(Container.DataItem, "cliafeat_cliac_id") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="description_delete" Text='<%# DataBinder.Eval(Container.DataItem, "cliafeat_flag") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox ID="seq_delete" Style="display: none;" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "cliafeat_seq_nbr") %>'></asp:TextBox>
                  </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="STATUS">
                  <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "clikff_name")%></ItemTemplate>
                  <EditItemTemplate>
                    <asp:DropDownList runat="server" ID="description">
                    </asp:DropDownList>
                  </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="SEQ #">
                  <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "cliafeat_seq_nbr")%></ItemTemplate>
                  <EditItemTemplate>
                    <asp:TextBox ID="seq" runat="server" Width="30" Text='<%# DataBinder.Eval(Container.DataItem, "cliafeat_seq_nbr") %>'></asp:TextBox>
                  </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                  <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" CommandName="Delete" runat="server"  OnClientClick="return confirm('Do you really want to remove this record?');"><i class="fa fa-trash-o"></i></asp:LinkButton></ItemTemplate>
                </asp:TemplateColumn>
              </Columns>
            </asp:DataGrid>
          </td>
        </tr>
        <tr>
          <td align="right"><br /><br />
            <asp:LinkButton ID="add_new" CommandName="Add" Text="ADD DETAIL" runat="server"  Font-Bold="true" Font-Underline="false"/>
            <asp:Panel runat="server" CssClass="gray" Visible="false" ID="new_row">
              <table width="400" cellpadding="3" cellspacing="0">
                <tr>
                  <td align="left" valign="top" width="300">
                    <asp:DropDownList runat="server" ID="clikfeat_name" Width="300">
                    </asp:DropDownList>
                  </td>
                  <td align="left" valign="top">
                    <asp:DropDownList runat="server" ID="status" Width="80">
                    </asp:DropDownList>
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="seq_no" runat="server" Width="30" Text="0"></asp:TextBox>
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
  </div>
</asp:Panel>
<asp:Panel ID="buttons" runat="server" BackColor="White">
  <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Right">
    <asp:Label ID="update_text" runat="server" Font-Italic="True"></asp:Label>
  </asp:Panel>
  <table width="100%" cellpadding="4" cellspacing="0">
    <tr>
      <td align="left" valign="top">
        <a href="javascript: window.opener.location.href = window.opener.location.href; self.close();" class="button">Close</a>
      </td>
      <td align="right" valign="top">
      </td>
    </tr>
  </table>
</asp:Panel>
