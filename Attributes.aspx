<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master"
  CodeBehind="Attributes.aspx.vb" Inherits="crmWebClient.Attributes" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <style type="text/css">
    .block-title {
      font-family: Arial;
      font-size: 16px;
      padding: 0 10px;
      height: 33px;
      line-height: 33px;
      position: relative;
      display: block;
    }

    .attentionText p {
      font-size: 1.2em;
      font-weight: bold !important;
      margin: 15px;
      text-align: center;
    }

    .block {
      margin: 5px 0;
      border: 1px solid #f1e8e2;
      background: #fff;
    }

      .block.block-title .isFolder {
        background-image: url('/images/folder-2x.png') !important;
        background-repeat: no-repeat;
        background-position: 5px 50%;
        padding-left: 30px;
      }

      .block.block-title .notFolder span {
        background-image: url('/images/file-2x.png');
        background-repeat: no-repeat;
        opacity: 0.5;
        background-position: 5px 50%;
        width: 50px;
        padding-left: 30px;
      }

    textarea {
      font-size: 13.3px;
      line-height: 1.3em;
    }

    .checkboxPadding {
      padding-right: 0em;
    }

    .valueSpec.Simplistic .subHeader {
      font-size: 14px !important;
    }

    table.dataTable td {
      font-size: 12px !important;
    }

    .mainBackground {
      border: 1px solid #b8b8b8;
    }

    .italic {
      font-style: italic;
    }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div id="outerDiv" class="valueSpec viewValueExport Simplistic aircraftSpec gray_background" runat="server">
    <table border="0" style="padding: 4px; border-spacing: 6px; text-align: left; width: 100%;" class="formatTable blue">
      <tr>
        <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">
          <asp:Label ID="close_window_only" runat="server" CssClass="float_right criteria_text"></asp:Label>

          <asp:TextBox runat="server" ID="selectedAttribute" CssClass="display_none"></asp:TextBox>
          <asp:TextBox runat="server" ID="selectedAsset" CssClass="display_none"></asp:TextBox>
          <cc1:TabContainer ID="attributes_tab" runat="server" CssClass="dark-theme" Visible="true" AutoPostBack="false" OnClientActiveTabChanged="TabSwapFunction">
            <cc1:TabPanel ID="attributes_panel" runat="server">
              <HeaderTemplate>
                Attributes            
              </HeaderTemplate>
              <ContentTemplate>
                <asp:UpdatePanel ID="Attribute_UpdatePanel"
                  runat="server" UpdateMode="Conditional">
                  <ContentTemplate>
                    <asp:DropDownList runat="server" ID="viewStateShow" CssClass="float_right padding margin_4" AutoPostBack="true" onchange="ChangeTheMouseCursorOnItemParentDocument('cursor_wait standalone_page');">
                      <asp:ListItem Value="">View in Tree</asp:ListItem>
                      <asp:ListItem Value="alpha" Selected="true">View Alphabetically</asp:ListItem>
                      <asp:ListItem Value="glossary">View Glossary</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList runat="server" ID="viewStatus" CssClass="float_right padding margin_4" AutoPostBack="true" onchange="ChangeTheMouseCursorOnItemParentDocument('cursor_wait standalone_page');">
                      <asp:ListItem Value="Y">View Active Attributes</asp:ListItem>
                      <asp:ListItem Value="N">View Inactive Attributes</asp:ListItem>
                      <asp:ListItem Value="" Selected="true">View All Attributes</asp:ListItem>
                      <asp:ListItem Value="AS">Asset Insight Attributes</asp:ListItem>
                    </asp:DropDownList>
                    <div class="div_clear"></div>
                    <asp:Label runat="server" ID="add_attention" ForeColor="red" font-weight="bold" Font-Size="large" CssClass="attentionText"></asp:Label>
                    <asp:Label runat="server" ID="mainMenuAdd">
                    </asp:Label>
                    <br />
                    <asp:Button ID="add_new_attribute" runat="server" Text="Add New Attribute" />

                  </ContentTemplate>
                </asp:UpdatePanel>

              </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="edit_tab" runat="server" CssClass="dark-theme" Visible="true">
              <HeaderTemplate>
                Edit Attribute       
              </HeaderTemplate>
              <ContentTemplate>
                <asp:UpdatePanel ID="editTabUpdate" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                  <ContentTemplate>
                    <asp:UpdatePanel runat="server" ID="EditUpdate" ChildrenAsTriggers="true" UpdateMode="Conditional">
                      <ContentTemplate>
                        <asp:Label runat="server" ID="attention_label" ForeColor="red" font-weight="bold" Visible="false" CssClass="attentionText"><p>Your attribute has been updated.</p></asp:Label>
                        <asp:Button runat="server" ID="editUpdateButton" CssClass="display_none" />
                        <asp:Panel ID="edit_panel" runat="server">
                          <asp:Table ID="temp_table" runat="server" Width="100%" CellPadding="3">
                            <asp:TableRow>
                              <asp:TableCell Width="150">
                      Area:
                              </asp:TableCell>
                              <asp:TableCell Width="300">
                                <asp:DropDownList ID="area_drop" runat="server" Width="100%" AutoPostBack="true"></asp:DropDownList>
                              </asp:TableCell>
                              <asp:TableCell Width="70" ColumnSpan="2">
                      Block:
                              </asp:TableCell>
                              <asp:TableCell ColumnSpan="2">
                                <asp:DropDownList ID="block_drop" runat="server" Width="100%"></asp:DropDownList>
                              </asp:TableCell>
                              <asp:TableCell Width="130">
                      Last Action Date:
                              </asp:TableCell>
                              <asp:TableCell Width="150px">
                                <asp:Label runat="server" ID="last_action_date"></asp:Label>
                              </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                              <asp:TableCell>
                      Name:
                              </asp:TableCell>
                              <asp:TableCell>
                                <asp:TextBox ID="name_text" runat="server" Width="100%"></asp:TextBox>
                              </asp:TableCell>
                              <asp:TableCell ColumnSpan="2">
                                <asp:Label ID="known_as_label" runat="server" Text="Known As:" Visible="true"></asp:Label>
                              </asp:TableCell>
                              <asp:TableCell ColumnSpan="2">
                                <asp:DropDownList ID="synonym_id" runat="server" Width="100%" onchange="ToggleRows();"></asp:DropDownList>
                              </asp:TableCell>
                              <asp:TableCell>
                      Refresh Date:
                              </asp:TableCell>
                              <asp:TableCell>
                                <asp:Label runat="server" ID="last_refresh_date"></asp:Label>
                              </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="disable1" runat="server">
                              <asp:TableCell Width="100">
                      <span class="text_underline help_cursor" title="Unique 5 character code assigned to this attribute used for displaying in columns where space is an issue for customers.">Code</span>:
                              </asp:TableCell>
                              <asp:TableCell ColumnSpan="5">
                                <asp:TextBox ID="code_text" runat="server" Width="45px" onchange='this.value = this.value.toUpperCase();' MaxLength="5" class="upperCase"></asp:TextBox>
                                <span class="float_right checkboxPadding">
                                  <asp:CheckBox ID="business_check" runat="server" Text="Business" />
                                  <asp:CheckBox ID="commercial_check" runat="server" Text="Commercial" />
                                  <asp:CheckBox ID="heli_check" runat="server" Text="Heicopter" />
                                  <asp:CheckBox ID="aerodex_check" runat="server" Text="Aerodex" />
                                  <asp:CheckBox ID="model_dependent" runat="server" Text="Model Dependent?"></asp:CheckBox><asp:CheckBox ID="acatt_glossary" runat="server" Text="Glossary"></asp:CheckBox></span>
                              </asp:TableCell>
                              <asp:TableCell>Status:</asp:TableCell><asp:TableCell>
                                <asp:DropDownList runat="server" ID="acatt_status" Width="100%">
                                  <asp:ListItem Selected="true" Value="Y">Active</asp:ListItem>
                                  <asp:ListItem Value="N">Inactive</asp:ListItem>
                                </asp:DropDownList></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow1" runat="server">
                              <asp:TableCell VerticalAlign="Top">
                      URL
                              </asp:TableCell>
                              <asp:TableCell ColumnSpan="11">
                                <asp:TextBox ID="def_url" runat="server" Text="" Rows="1" Width="100%"></asp:TextBox>
                              </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="disable2" runat="server">
                              <asp:TableCell VerticalAlign="Top">
                      Description
                              </asp:TableCell>
                              <asp:TableCell ColumnSpan="11">
                                <asp:TextBox ID="description" runat="server" Width="100%" Rows="10" TextMode="MultiLine"></asp:TextBox>
                              </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow runat="server">
                              <asp:TableCell VerticalAlign="Top"></asp:TableCell>
                              <asp:TableCell>
                                <br />
                                <asp:Label runat="server" ID="acatt_count" Font-Size="large"></asp:Label>
                              </asp:TableCell>
                              <asp:TableCell ColumnSpan="2" Width="14%">
                                Low:
                                <asp:TextBox runat="server" ID="acatt_low" Width="50px"></asp:TextBox>
                              </asp:TableCell><asp:TableCell align="center" Width="14%">
                                Avg:
                                <asp:TextBox runat="server" ID="acatt_average" Width="50px"></asp:TextBox>
                              </asp:TableCell><asp:TableCell Width="14%">
                                High:
                                <asp:TextBox runat="server" ID="acatt_high" Width="50px"></asp:TextBox>
                              </asp:TableCell>
                              <asp:TableCell Wrap="false">
                                &nbsp;&nbsp;ID:&nbsp;
                                <asp:Label runat="server" ID="Temp_ID_New" Text=""></asp:Label>
                              </asp:TableCell>
                            </asp:TableRow>

                          </asp:Table>

                          <asp:Button ID="submit_button" runat="server" Text="Save" />

                          <asp:Button ID="cancel_button" runat="server" Text="Cancel" CssClass="float_right criteria_text" />

                        </asp:Panel>
                      </ContentTemplate>
                    </asp:UpdatePanel>

                    <cc1:TabContainer ID="bottom_tab_container" runat="server" CssClass="dark-theme" OnClientActiveTabChanged="subTabSwapFunction">
                      <cc1:TabPanel ID="tab_1" runat="server" HeaderText="How to Find/Rules">
                        <ContentTemplate>
                          <asp:Label ID="rules_label" runat="server" Visible="false"></asp:Label>
                          <asp:Button ID="add_rule" runat="server" Text="Add Rule" Visible="false" />
                          <table border="0" style="padding: 4px; border-spacing: 6px; text-align: left; width: 100%;" class="formatTable blue">
                            <tr>
                              <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">Auto Generate?</td>
                              <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">
                                <asp:CheckBox runat="server" ID="autoGenerateRule" /></td>
                            </tr>
                            <tr>
                              <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">How to Find this Attribute?</td>
                              <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">
                                <asp:TextBox runat="server" ID="howToFindRule" Width="100%"></asp:TextBox></td>
                            </tr>
                            <tr>
                              <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">Query</td>
                              <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">
                                <asp:TextBox runat="server" ID="queryRule" Width="100%" Rows="12" TextMode="MultiLine"></asp:TextBox></td>
                            </tr>
                          </table>
                        </ContentTemplate>
                      </cc1:TabPanel>
                      <cc1:TabPanel ID="tab_2" runat="server" HeaderText="Subsets">
                        <ContentTemplate>
                          <asp:Label runat="server" ID="subsets_label" Text="If an aircraft has any items on this tab, it would also indicate the aircraft has the attribute above."></asp:Label>
                          <asp:Label runat="server" ID="related_attributes"></asp:Label>
                        </ContentTemplate>
                      </cc1:TabPanel>
                      <cc1:TabPanel ID="tab_3" runat="server" HeaderText="Models Associated">
                        <ContentTemplate>
                          <asp:UpdatePanel runat="server" ID="modelUpdate" ChildrenAsTriggers="true" UpdateMode="Conditional">
                            <ContentTemplate>
                              <asp:Label runat="server" ID="model_relationships_label"></asp:Label>

                              <asp:Button runat="server" ID="addModelRelationship" Text="Add Model Relationship" />
                              <asp:Panel runat="server" ID="ModelRelationshipAddPanel" Visible="false">
                                <table border="0" style="padding: 4px; border-spacing: 6px; text-align: left; width: 100%;" class="formatTable blue">
                                  <tr>
                                    <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">Make/Model</td>
                                    <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">
                                      <asp:DropDownList runat="server" ID="models_makeModel">
                                        <asp:ListItem></asp:ListItem>
                                      </asp:DropDownList></td>
                                    <td  style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">Est. Value Impact</td>
                                    <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">
                                      <asp:TextBox runat="server" ID="model_EstValueImpact"></asp:TextBox></td>
                                    <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">Ser # Range:</td>
                                    <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">
                                      <asp:TextBox runat="server" ID="models_SerNoStart"></asp:TextBox>/<asp:TextBox runat="server" ID="models_SerNoEnd"></asp:TextBox></td>
                                  </tr>
                                </table>

                              </asp:Panel>
                            </ContentTemplate>
                          </asp:UpdatePanel>
                        </ContentTemplate>
                      </cc1:TabPanel>
                      <cc1:TabPanel ID="tab_6" runat="server" HeaderText="eValue Mapping">
                        <ContentTemplate>
                          <asp:Label runat="server" ID="evalue_label" Text="The items below are a list of assets/modifications from Asset Insight that are mapped to the attribute above for eValue calculation purposes."></asp:Label>
                          <asp:Label runat="server" ID="asset_attributes"></asp:Label>
                        </ContentTemplate>
                      </cc1:TabPanel>
                      <cc1:TabPanel ID="tab_7" runat="server" HeaderText="Components">
                        <ContentTemplate>
                          <asp:Label runat="server" Text="The following items are automatically included on an aircraft as part of the system/attribute described on this page"></asp:Label>
                          <asp:Label runat="server" ID="components_label" Text=""></asp:Label>
                        </ContentTemplate>

                      </cc1:TabPanel>
                      <cc1:TabPanel ID="tab_8" runat="server" HeaderText="Synonyms">
                        <ContentTemplate>
                          <asp:Label runat="server" ID="synonyms_label"></asp:Label>
                        </ContentTemplate>
                      </cc1:TabPanel>
                      <cc1:TabPanel ID="tab_5" runat="server" HeaderText="Summary by Model">
                        <ContentTemplate>
                          <asp:Label runat="server" ID="models_with_attributes_label"></asp:Label>
                        </ContentTemplate>
                      </cc1:TabPanel>
                      <cc1:TabPanel ID="tab_4" runat="server" HeaderText="Summary by Aircraft">
                        <ContentTemplate>
                          <asp:UpdatePanel runat="server" ID="acRelated" ChildrenAsTriggers="true" UpdateMode="Conditional">
                            <ContentTemplate>
                              <asp:TextBox runat="server" ID="acRelatedRan" CssClass="display_none"></asp:TextBox>
                              <asp:Label runat="server" ID="ac_related_label"></asp:Label>
                              <asp:Button runat="server" ID="runACButton" CssClass="display_none" />
                            </ContentTemplate>
                          </asp:UpdatePanel>

                        </ContentTemplate>
                      </cc1:TabPanel>

                    </cc1:TabContainer>
                  </ContentTemplate>
                </asp:UpdatePanel>
              </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="asset_edit_panel" HeaderText="Edit Asset" runat="server">
              <ContentTemplate>
                <asp:UpdatePanel ID="asset_update_panel" runat="server">
                  <ContentTemplate>
                    <asp:Button runat="server" ID="editAssetUpdateButton" CssClass="display_none" />
                    <table border="0" style="padding: 4px; border-spacing: 6px; text-align: left; width: 100%;" class="formatTable blue">
                      <tr>
                        <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">Asset:</td>
                        <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">
                          <asp:Label runat="server" ID="asset_Name" Font-Bold="true" Font-Size="Medium"></asp:Label></td>
                        <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">Linked Attribute:</td>
                        <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">
                          <asp:DropDownList runat="server" ID="linkedAttribute"></asp:DropDownList></td>
                      </tr>
                      <tr>
                        <td style="vertical-align: top; text-align: right; padding-left: 8px; padding-top: 8px;" colspan="4">
                          <asp:Button runat="server" ID="saveAssetAttribute" Text="Save" /></td>
                      </tr>
                    </table>
                  </ContentTemplate>
                </asp:UpdatePanel>
              </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="edit_rules_panel" Visible="false" HeaderText="Edit Rules" runat="server">
              <ContentTemplate>
               <asp:UpdatePanel ID="rules_update_panel" runat="server">
                  <ContentTemplate>
                    <asp:Panel ID="Panel1" runat="server">
                      <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                          <asp:TableCell Width="100">
                      Operator:
                          </asp:TableCell>
                          <asp:TableCell>
                            <asp:DropDownList ID="rule_operator" runat="server"></asp:DropDownList>
                          </asp:TableCell>
                          <asp:TableCell Width="100">
                      Area:
                          </asp:TableCell>
                          <asp:TableCell>
                            <asp:DropDownList ID="rule_area" runat="server"></asp:DropDownList>
                          </asp:TableCell>
                          <asp:TableCell>
                      Block:
                          </asp:TableCell>
                          <asp:TableCell>
                            <asp:DropDownList ID="rule_block" runat="server"></asp:DropDownList>
                          </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                          <asp:TableCell Width="100">
                      Action:
                          </asp:TableCell>
                          <asp:TableCell>
                            <asp:DropDownList ID="rule_action_drop" runat="server"></asp:DropDownList>
                          </asp:TableCell>
                          <asp:TableCell Width="100">
                      Phrases:
                          </asp:TableCell>
                          <asp:TableCell ColumnSpan="3">
                            <asp:TextBox ID="rule_phrases_textbox" runat="server" Columns="100"></asp:TextBox>
                          </asp:TableCell>

                        </asp:TableRow>

                      </asp:Table>
                    </asp:Panel>
                  </ContentTemplate>
                </asp:UpdatePanel>


              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
        </td>
      </tr>
    </table>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

  <script type="text/javascript">
    //Smaller tabs on edit page at bottom
    function subTabSwapFunction(sender, args) {
      if (sender.get_activeTabIndex() == 2) {
        $('#modelRelationships').DataTable();
      } else if (sender.get_activeTabIndex() == 1) {

        $('#relatedAttributes').DataTable();
      } else if (sender.get_activeTabIndex() == 7) {
        if ($('#<%= acRelatedRan.ClientID %>').val() == '') {
          ChangeTheMouseCursorOnItemParentDocument('cursor_wait standalone_page')
          $('#<%= runACButton.ClientID %>').click();
        }
      } else if (sender.get_activeTabIndex() == 6) {

        $('#assetAttributeData').DataTable();
      }

    }
    function TabSwapFunction(sender, args) {
      if (sender.get_activeTabIndex() == 0) {
        $('#<%= selectedAttribute.ClientID %>').val('0');
        $find('<%= edit_tab.ClientID %>')._hide();

        $find('<%= asset_edit_panel.ClientID %>')._hide();
        $('#<%= acRelatedRan.ClientID %>').val('');

      } else if (sender.get_activeTabIndex() == 1) {


      }
    }
    window.onload = function () {
      $find('<%= edit_tab.ClientID %>')._hide();
      $find('<%= asset_edit_panel.ClientID %>')._hide();
    }


    function ToggleRows() {
      if ($('#<%= synonym_id.ClientID %>').val() == '0') {
        $("#<%= disable1.ClientID %>").find("input,button,textarea,select").removeAttr("disabled");
        $("#<%= disable2.ClientID %>").find("input,button,textarea,select").removeAttr("disabled");

        $("#<%= disable1.ClientID %>").attr("class", "");
        $("#<%= disable2.ClientID %>").attr("class", "");

      } else {

        $("#<%= disable1.ClientID %>").find("input,button,textarea,select").attr("disabled", "disabled");
        $("#<%= disable2.ClientID %>").find("input,button,textarea,select").attr("disabled", "disabled");

        $("#<%= disable1.ClientID %>").attr("class", "display_disable");
        $("#<%= disable2.ClientID %>").attr("class", "display_disable");

      }
    }


  </script>

</asp:Content>
