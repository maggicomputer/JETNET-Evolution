<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Aircraft_Edit_Details_Tabs.ascx.vb"
  Inherits="crmWebClient.Aircraft_Edit_Details_Tabs" %>
<asp:Panel ID="aircraft_edit" runat="server">
  <asp:Label runat="server" ID="title_change" CssClass="valueSpec viewValueExport Simplistic aircraftSpec"> <h2 align="right">
        Avionics Edit</h2></asp:Label>
  <asp:Label runat="server" ID="updated" Font-Bold="True" ForeColor="Red"></asp:Label>
  <div class="valueSpec Simplistic viewValueExport aircraftSpec">
    <asp:Panel runat="server" ID="topPage">
      <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
          <td colspan="4" align="right">
            <asp:Panel runat="server" Visible="false" ID="interior">
              <div class="Box">
                <table width="100%" cellpadding="3" cellspacing="0" class="formatTable blue">
                  <tr class="noBorder">
                    <td colspan="4" align="right">
                      <asp:Label runat="server" ID="subheaderText" CssClass="subHeader">Interior Edit</asp:Label><br />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      AC Interior Rating:
                    </td>
                    <td align="left" valign="top" width="100">
                      <asp:TextBox ID="ac_interior_rating" Width="100px" runat="server" /><br />
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Enter a Value"
                        ControlToValidate="ac_interior_rating" Display="Dynamic" SetFocusOnError="True"></asp:RequiredFieldValidator>
                      <asp:CompareValidator ID="CompareValidator4" runat="server" ErrorMessage="Please enter a Number"
                        ControlToValidate="ac_interior_rating" Display="Dynamic" Operator="DataTypeCheck"
                        SetFocusOnError="True" Type="Integer"></asp:CompareValidator>
                    </td>
                    <td align="left" valign="top">
                      Done By:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="ac_interior_doneby_name" Width="100px" runat="server" MaxLength="50" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      MM/YYYY:
                    </td>
                    <td align="left" valign="top" width="100">
                      <asp:TextBox ID="ac_interior_month" Width="30px" runat="server" MaxLength="2" />/
                      <asp:TextBox ID="ac_interior_year" Width="59px" runat="server" MaxLength="4" />
                    </td>
                    <td align="left" valign="top">
                      Passengers:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="ac_passenger_count" Width="100px" runat="server" />
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ac_passenger_count"
                        Display="Dynamic" ErrorMessage="Please enter a Value" SetFocusOnError="True"></asp:RequiredFieldValidator>
                      <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="ac_passenger_count"
                        Display="Dynamic" ErrorMessage="Please enter a Number" Operator="DataTypeCheck"
                        SetFocusOnError="True" Type="Integer"></asp:CompareValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Configuration
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="ac_interior_config_name" Width="100px" runat="server" MaxLength="15" />
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="3">
                      <a href="javascript: window.opener.location.href = window.opener.location.href; self.close();"
                        class="button float_left">Close</a>
                    </td>
                    <td align="right" valign="top">
                      <asp:Button runat="server" ID="interiorSave" Text="Save" class="button" />
                    </td>
                  </tr>
                </table>
              </div>
            </asp:Panel>
          </td>
        </tr>
        <tr>
          <td colspan="4" align="right">
            <asp:Panel runat="server" Visible="false" ID="exterior">
              <div class="Box">
                <table width="100%" class="formatTable blue" cellpadding="3" cellspacing="0">
                  <tr class="noBorder">
                    <td colspan="4" align="right">
                      <asp:Label runat="server" ID="Label1" CssClass="subHeader">Exterior Edit</asp:Label><br />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Ac Exterior Rating:
                    </td>
                    <td align="left" valign="top" width="100">
                      <asp:TextBox ID="ac_exterior_rating" Width="100px" runat="server" />
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ac_exterior_rating"
                        Display="Dynamic" ErrorMessage="Please Enter a Value" SetFocusOnError="True"></asp:RequiredFieldValidator>
                      <asp:CompareValidator ID="CompareValidator7" runat="server" ControlToValidate="ac_exterior_rating"
                        Display="Dynamic" ErrorMessage="Please enter a Number" Operator="DataTypeCheck"
                        SetFocusOnError="True" Type="Integer"></asp:CompareValidator>
                    </td>
                    <td align="left" valign="top">
                      Done By:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="ac_exterior_doneby_name" Width="100px" runat="server" MaxLength="50" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      MM/YYYY:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="ac_exterior_month" Width="30px" runat="server" MaxLength="2" />/
                      <asp:TextBox ID="ac_exterior_year" Width="59px" runat="server" MaxLength="4" />
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="3">
                      <a href="javascript: window.opener.location.href = window.opener.location.href; self.close();"
                        class="button float_left">Close</a>
                    </td>
                    <td align="right" valign="top">
                      <asp:Button runat="server" ID="exteriorSave" Text="Save" class="button" />
                    </td>
                  </tr>
                </table>
              </div>
            </asp:Panel>
          </td>
        </tr>
        <tr>
          <td colspan="4" align="right">
            <asp:Panel runat="server" Visible="false" ID="additional_details_edit">
              <div class="Box">
                <table width="100%" class="formatTable blue" cellpadding="0" cellspacing="0">
                  <tr class="noBorder">
                    <td colspan="4" align="right">
                      <asp:Label runat="server" ID="Label5" CssClass="subHeader">Additional Equipment Edit</asp:Label>
                    </td>
                  </tr>
                </table>
              </div>
            </asp:Panel>
          </td>
        </tr>
        <tr>
          <td colspan="4" align="right">
            <asp:Panel runat="server" Visible="false" ID="main">
              <div class="Box">
                <table width="100%" class="formatTable blue" cellpadding="3" cellspacing="0">
                  <tr class="noBorder">
                    <td colspan="4" align="right">
                      <asp:Label runat="server" ID="Label2" CssClass="subHeader">Maintenance Edit</asp:Label><br />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      EOH By Name:
                    </td>
                    <td align="left" valign="top" width="140">
                      <asp:TextBox ID="ac_maint_eoh_by_name" Width="290px" runat="server" MaxLength="50" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      HOTS By Name:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="ac_maint_hots_by_name" Width="290px" runat="server" MaxLength="50" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      Airframe Maintenance Program:
                    </td>
                    <td align="left" valign="top" width="140">
                      <asp:DropDownList ID="airframe_maintenance_program" runat="server" Width="294px">
                      </asp:DropDownList>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      Airframe Tracking Program:
                    </td>
                    <td align="left" valign="top" width="140">
                      <asp:DropDownList ID="airframe_tracking_program" runat="server" Width="294px">
                      </asp:DropDownList>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      AC Maintained:
                    </td>
                    <td align="left" valign="top" width="140">
                      <asp:TextBox ID="ac_maintained" Width="290px" runat="server" MaxLength="50" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      AC Damage History Notes:
                    </td>
                    <td align="left" valign="top" width="140">
                      <asp:TextBox ID="damage_history" Width="290px" runat="server" MaxLength="250" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      <a href="javascript: window.opener.location.href = window.opener.location.href; self.close();"
                        class="button float_left">Close</a>
                    </td>
                    <td align="right" valign="top">
                      <asp:Button runat="server" ID="maintenanceSave" Text="Save" class="button" />
                    </td>
                  </tr>
                </table>
              </div>
            </asp:Panel>
          </td>
        </tr>
        <tr>
          <td colspan="4" align="right">
            <asp:Panel runat="server" Visible="false" ID="usage">
              <div class="Box">
                <table width="100%" cellpadding="3" cellspacing="0" class="formatTable blue">
                  <tr class="noBorder">
                    <td colspan="4" align="right">
                      <asp:Label runat="server" ID="Label3" CssClass="subHeader">Usage Edit</asp:Label><br />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      Times/Values Current As Of:
                    </td>
                    <td align="left" valign="top" width="140">
                      <asp:TextBox ID="ac_date_engine_times_as_of" Width="120px" runat="server" />
                      <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Please Enter a Date"
                        Enabled="true" ControlToValidate="ac_date_engine_times_as_of" Operator="DataTypeCheck"
                        Width="120px" SetFocusOnError="True" Type="Date" Display="Dynamic" Height="19px"></asp:CompareValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      Air Frame Total Time (AFTT):
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="ac_airframe_total_hours" Width="120px" runat="server" /><br />
                      <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ac_airframe_total_hours"
                        ErrorMessage="Please Enter a number" Operator="DataTypeCheck" Type="Integer" Display="Dynamic"></asp:CompareValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      Landings/Cycles:
                    </td>
                    <td align="left" valign="top" width="140">
                      <asp:TextBox ID="ac_airframe_total_landings" Width="120px" runat="server" />
                      <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="ac_airframe_total_landings"
                        ErrorMessage="Please Enter a number" Operator="DataTypeCheck" Type="Integer" Display="Dynamic"></asp:CompareValidator>
                      <br />
                    </td>
                  </tr>
                  <tr>
                    <td align="right" valign="top" colspan="2">
                      <a href="javascript: window.opener.location.href = window.opener.location.href; self.close();"
                        class="button float_left">Close</a>
                    </td>
                    <td align="right" valign="top">
                      <asp:Button runat="server" ID="usageSave" Text="Save" class="button" />
                    </td>
                  </tr>
                </table>
              </div>
            </asp:Panel>
          </td>
        </tr>
        <tr>
          <td colspan="4" align="right">
            <asp:Panel runat="server" Visible="false" ID="APU">
              <div class="Box">
                <table width="100%" class="formatTable blue" cellpadding="3" cellspacing="0">
                  <tr class="noBorder">
                    <td colspan="4" align="right">
                      <asp:Label runat="server" ID="Label4" CssClass="subHeader">APU Edit</asp:Label><br />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      APU Model Name:
                    </td>
                    <td align="left" valign="top" width="140">
                      <asp:TextBox ID="ac_apu_model_name" Width="120px" runat="server" MaxLength="40" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      APU Serial #:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="ac_apu_ser_nbr" Width="120px" runat="server" MaxLength="20" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      APU Maintenance Plan:
                    </td>
                    <td align="left" valign="top" width="140">
                      <asp:DropDownList ID="ac_apu_main_dropdown" runat="server">
                      </asp:DropDownList>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      APU Total Time (Hours) Since New:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="ac_apu_ttsn_hours" Width="120px" runat="server" /><br />
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ac_apu_ttsn_hours"
                        Display="Dynamic" ErrorMessage="Please enter a Value"></asp:RequiredFieldValidator>
                      <asp:CompareValidator ID="CompareValidator8" runat="server" ControlToValidate="ac_apu_ttsn_hours"
                        Display="Dynamic" ErrorMessage="Please Enter a number" Operator="DataTypeCheck"
                        Type="Integer"></asp:CompareValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      Since Overhaul (SOH) Hours:
                    </td>
                    <td align="left" valign="top" width="140">
                      <asp:TextBox ID="ac_apu_tsoh_hours" Width="120px" runat="server" />
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ac_apu_tsoh_hours"
                        Display="Dynamic" ErrorMessage="Please enter a Value"></asp:RequiredFieldValidator>
                      <asp:CompareValidator ID="CompareValidator9" runat="server" ControlToValidate="ac_apu_tsoh_hours"
                        Display="Dynamic" ErrorMessage="Please Enter a number" Operator="DataTypeCheck"
                        Type="Integer"></asp:CompareValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" colspan="2">
                      APU Since Hot Inspection (SHI) Hours:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox ID="ac_apu_tshi_hours" Width="120px" runat="server" />
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ac_apu_tshi_hours"
                        Display="Dynamic" ErrorMessage="Please enter a Value"></asp:RequiredFieldValidator>
                      <asp:CompareValidator ID="CompareValidator10" runat="server" ControlToValidate="ac_apu_tshi_hours"
                        Display="Dynamic" ErrorMessage="Please Enter a number" Operator="DataTypeCheck"
                        Type="Integer"></asp:CompareValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="right" valign="top" colspan="2">
                      <a href="javascript: window.opener.location.href = window.opener.location.href; self.close();"
                        class="button float_left">Close</a>
                    </td>
                    <td align="right" valign="top">
                      <asp:Button runat="server" ID="apuSave" Text="Save" class="button" />
                    </td>
                  </tr>
                </table>
              </div>
            </asp:Panel>
          </td>
        </tr>
      </table>
    </asp:Panel>
    <div class="Box">
      <table width="100%" cellpadding="3" cellspacing="0" class="formatTable blue">
        <tr>
          <td align="left" valign="top" colspan="4">
            <div class="subHeader" runat="server" visible="false" id="equipHeader">
              Equipment Edit
              <br />
              <br />
            </div>
            <asp:DataGrid runat="server" ID="datagrid_details" CellPadding="3" horizontal-align="left"
              EnableViewState="true" ShowFooter="false" BackColor="White" Width="100%" AllowPaging="false"
              PageSize="25" OnCancelCommand="MyDataGrid_Cancel" OnUpdateCommand="MyDataGrid_Update"
              OnDeleteCommand="MyDataGrid_Delete" OnEditCommand="MyDataGrid_Edit" CssClass="formatTable blue"
              Visible="true" AllowSorting="True" GridLines="None" AutoGenerateColumns="false">
              <ItemStyle BorderStyle="None" VerticalAlign="Top" />
              <HeaderStyle Font-Bold="True" Font-Size="11pt" Wrap="False" HorizontalAlign="Left"
                VerticalAlign="Middle" BorderColor="White"></HeaderStyle>
              <Columns>
                <asp:EditCommandColumn EditText="<img src='/images/edit_icon.png' width='14px'/>"
                  UpdateText="<img src='/images/save_disk_icon.png' width='19px'  />" />
                <asp:TemplateColumn HeaderText="NAME" ItemStyle-HorizontalAlign="left">
                  <ItemTemplate>
                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "cliadet_data_name")), (DataBinder.Eval(Container.DataItem, "cliadet_data_name")), "")%>
                    <asp:TextBox runat="server" ID="id" Text='<%# DataBinder.Eval(Container.DataItem, "cliadet_id") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="ac_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cliadet_cliac_id") %>'
                      Style="display: none;" />
                    <img src="images/spacer.gif" alt="" width="100" height="1" />
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:DropDownList runat="server" ID="name_type">
                    </asp:DropDownList>
                    <asp:TextBox runat="server" ID="id" Text='<%# DataBinder.Eval(Container.DataItem, "cliadet_id") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="name_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cliadet_data_name") %>'
                      Style="display: none;" />
                    <asp:TextBox runat="server" ID="ac_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cliadet_cliac_id") %>'
                      Style="display: none;" />
                    <asp:TextBox runat="server" ID="type_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cliadet_data_type") %>'
                      Style="display: none;" />
                  </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="DESCRIPTION">
                  <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "cliadet_data_description")%></ItemTemplate>
                  <EditItemTemplate>
                    <asp:TextBox runat="server" TextMode="MultiLine" Width="200px" Rows="6" ID="description"
                      Text='<%# DataBinder.Eval(Container.DataItem, "cliadet_data_description") %>' />
                  </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                  <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" CommandName="Delete" runat="server" OnClientClick="return confirm('Do you really want to remove this record?');"><i class="fa fa-trash-o"></i></asp:LinkButton></ItemTemplate>
                </asp:TemplateColumn>
              </Columns>
            </asp:DataGrid><br />
            <br />
            <div class="subHeader" runat="server" visible="false" id="addHeader">
              Addl Cockpit Equipment
              <br />
              <br />
            </div>
            <asp:DataGrid runat="server" ID="additional" CellPadding="3" horizontal-align="left"
              EnableViewState="true" ShowFooter="false" BackColor="White" Width="100%" AllowPaging="false"
              PageSize="25" OnCancelCommand="MyDataGrid_CancelAdd" OnUpdateCommand="MyDataGrid_Update"
              OnDeleteCommand="MyDataGrid_Delete" OnEditCommand="MyDataGrid_EditAdd" CssClass="formatTable blue"
              Visible="true" AllowSorting="True" GridLines="None" AutoGenerateColumns="false">
              <ItemStyle BorderStyle="None" VerticalAlign="Top" />
              <HeaderStyle Font-Bold="True" Font-Size="11pt" Wrap="False" HorizontalAlign="Left"
                VerticalAlign="Middle" BorderColor="White"></HeaderStyle>
              <Columns>
                <asp:EditCommandColumn EditText="<img src='/images/edit_icon.png' width='14px'/>"
                  UpdateText="<img src='/images/save_disk_icon.png' width='19px'  />" />
                <asp:TemplateColumn HeaderText="NAME" ItemStyle-HorizontalAlign="left">
                  <ItemTemplate>
                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "cliadet_data_name")), (DataBinder.Eval(Container.DataItem, "cliadet_data_name")), "")%>
                    <asp:TextBox runat="server" ID="id" Text='<%# DataBinder.Eval(Container.DataItem, "cliadet_id") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="ac_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cliadet_cliac_id") %>'
                      Style="display: none;" />
                    <img src="images/spacer.gif" alt="" width="100" height="1" />
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:DropDownList runat="server" ID="name_type">
                    </asp:DropDownList>
                    <asp:TextBox runat="server" ID="id" Text='<%# DataBinder.Eval(Container.DataItem, "cliadet_id") %>'
                      Visible="true" Style="display: none;" />
                    <asp:TextBox runat="server" ID="name_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cliadet_data_name") %>'
                      Style="display: none;" />
                    <asp:TextBox runat="server" ID="ac_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cliadet_cliac_id") %>'
                      Style="display: none;" />
                    <asp:TextBox runat="server" ID="type_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "cliadet_data_type") %>'
                      Style="display: none;" />
                  </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="DESCRIPTION">
                  <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "cliadet_data_description")%></ItemTemplate>
                  <EditItemTemplate>
                    <asp:TextBox runat="server" TextMode="MultiLine" Width="200px" Rows="6" ID="description"
                      Text='<%# DataBinder.Eval(Container.DataItem, "cliadet_data_description") %>' />
                  </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                  <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" CommandName="Delete" runat="server" OnClientClick="return confirm('Do you really want to remove this record?');"><i class="fa fa-trash-o"></i></asp:LinkButton></ItemTemplate>
                </asp:TemplateColumn>
              </Columns>
            </asp:DataGrid>
          </td>
        </tr>
        <tr>
          <td colspan="4" align="right">
            <asp:LinkButton ID="add_new" CommandName="Add" Text="ADD DETAIL" runat="server" Font-Bold="true" />
            <asp:Panel runat="server" CssClass="formatTable blue" Visible="false" ID="new_row">
              <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                  <td align="left" valign="top" width="10">
                    <asp:Panel runat="server" ID="typeDropdownVisible">
                      <asp:DropDownList runat="server" ID="typeDropdownPick">
                      </asp:DropDownList>
                    </asp:Panel>
                  </td>
                  <td align="left" valign="top" width="100">
                    <asp:DropDownList runat="server" ID="cliadet_data_name" Width="100">
                    </asp:DropDownList>
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="cliadet_data_description" Width="240px" TextMode="MultiLine" Rows="6"
                      runat="server" MaxLength="65535" />
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
    <asp:Panel ID="buttons" runat="server" BackColor="White" Visible="false">
      <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Right">
        <asp:Label ID="update_text" runat="server" Font-Italic="True"></asp:Label>
      </asp:Panel>
      <table width="100%" cellpadding="4" cellspacing="0">
        <tr>
          <td align="left" valign="top">
            <a href="javascript: window.opener.location.href = window.opener.location.href; self.close();"
              class="button  float_left">Close</a>
          </td>
          <td align="right" valign="top">
            <asp:Button runat="server" ID="updateButton" CausesValidation="true" Text="Save" />
          </td>
        </tr>
      </table>
    </asp:Panel>
  </div>
</asp:Panel>
<asp:Panel runat="server" ID="done_with_Changes" Visible="false">
  <a onclick="javascript: window.opener.location.href = window.opener.location.href; self.close();"
    class="button float_left">Close</a>
  <% If Session.Item("crmUserLogon") = True Then%>

  <script type="text/javascript" language="javascript">

    function close_fold_window() {
      window.opener.location.href = 'details.aspx?type=3&source=<%= Session.Item("ListingSource") %>&ac_ID=<%= Session.Item("ListingID") %>';
      window.close();
    }

  </script>

  <% End If%>
</asp:Panel>
