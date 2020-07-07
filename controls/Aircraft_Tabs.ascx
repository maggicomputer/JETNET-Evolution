<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Aircraft_Tabs.ascx.vb"
  Inherits="crmWebClient.Aircraft_Tabs" %>

<script language="javascript" type="text/javascript" src="https://www.google.com/jsapi?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE"></script>

<script type="text/javascript">
  google.load('visualization', '1', { packages: ['corechart'] });

</script>

<asp:UpdateProgress runat="server" AssociatedUpdatePanelID="bottom_tab_update_panel" DisplayAfter="1000">
  <ProgressTemplate>
    <div id="divLoading" runat="server" style="width: 100%;
        height: 100%; padding: 0; position: absolute; z-index: 10; margin-left: 0px;cursor:progress;">
        <br />
        <br />
      </div>
  </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="bottom_tab_update_panel" runat="server" ChildrenAsTriggers="true">
  <ContentTemplate>
    <cc1:TabContainer ID="tabs_container" runat="server" Width="100%" Height="343px"
      CssClass="dark-theme pad_top" Visible="true" AutoPostBack="true" ActiveTabIndex="14">
      <cc1:TabPanel ID="features_tab" runat="server" HeaderText="FEATURES">
        <ContentTemplate>
          <div class="tab_container_div">
            <table width="100%" cellpadding="2" cellspacing="0">
              <tr>
                <td align="left" valign="top">
                  <div class="row">
                    <div class="six_half columns">
                      <asp:Label runat="server" ID="features_label"></asp:Label></div>
                    <div class="six_half columns">
                      <asp:Label runat="server" ID="features_label_client"></asp:Label></div>
                    <div class="six_half columns float_right">
                      <asp:Label runat="server" ID="features_label_notes" EnableViewState="true"></asp:Label></div>
                  </div>
                </td>
              </tr>
            </table>
            <asp:TextBox runat="server" ID="feature_tab_time" Style="display: none;" />
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="engine_tab" runat="server" HeaderText="ENG.">
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label runat="server" ID="engine_warning_text" ForeColor="Red" Font-Bold="true"></asp:Label>
            <table width="100%" cellpadding="2" cellspacing="0">
              <tr>
                <td align="left" valign="top">
                  <asp:Label runat="server" Text="" ID="engine_label"></asp:Label>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  <asp:Label runat="server" Text="" ID="engine_label_client"></asp:Label>
                </td>
              </tr>
            </table>
            <table width="100%" cellspacing="0">
              <tr>
                <td align="right" valign="top">
                  <table width="100%">
                    <tr>
                      <td align="left" valign="top" width="50%">
                      </td>
                      <td align="left" valign="top" width="50%">
                        <asp:Label runat="server" ID="engine_label_notes"></asp:Label>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
            <asp:TextBox runat="server" ID="engine_tab_time" Style="display: none;" />
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="transaction_tab" runat="server" HeaderText="TRANSACTION">
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label runat="server" ID="trans_warning_text" ForeColor="Red" Font-Bold="true"></asp:Label>
            <table width="99%" cellpadding="0" cellspacing="0">
              <tr>
                <td align="left" valign="top">
                  <asp:Label runat="server" Text="" ID="trans_label"></asp:Label>
                  <div class="row">
                    <div class="six_half columns float_right">
                      <asp:Label runat="server" ID="trans_label_notes"></asp:Label></div>
                  </div>
                </td>
              </tr>
            </table>
            <asp:TextBox runat="server" ID="trans_tab_time" Style="display: none;" />
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="avionics_tab" runat="server" HeaderText="AVIONICS">
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label runat="server" ID="avionics_warning_text" ForeColor="Red" Font-Bold="true"></asp:Label>
            <table width="100%" cellpadding="2" cellspacing="0">
              <tr>
                <td align="left" valign="top">
                  <div class="row">
                    <div class="six_half columns">
                      <asp:Label runat="server" Text="" ID="avionics_label"></asp:Label></div>
                    <div class="six_half columns">
                      <asp:Label runat="server" Text="" ID="avionics_label_client"></asp:Label></div>
                    <div class="six_half columns float_right">
                      <asp:Label runat="server" ID="avionics_label_notes"></asp:Label>
                    </div>
                </td>
              </tr>
            </table>
            <asp:TextBox runat="server" ID="avionics_tab_time" Style="display: none;" />
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="usage_tab" runat="server" HeaderText="USE">
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label runat="server" ID="usuage_warning_text" ForeColor="Red" Font-Bold="true"></asp:Label>
            <table width="100%" cellpadding="2" cellspacing="0">
              <tr>
                <td align="left" valign="top">
                  <div class="row">
                    <div class="seven columns">
                      <asp:Label runat="server" Text="" ID="usage_label"></asp:Label></div>
                    <div class="six columns">
                      <asp:Label runat="server" Text="" ID="usage_label_client"></asp:Label></div>
                    <div class="six columns float_right">
                      <asp:Label runat="server" ID="usage_label_notes"></asp:Label></div>
                  </div>
                </td>
              </tr>
            </table>
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="maint_tab" runat="server" HeaderText="MAINT">
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label runat="server" ID="maint_warning_text" ForeColor="Red" Font-Bold="true"></asp:Label>
            <table width="100%" cellpadding="2" cellspacing="0">
              <tr>
                <td align="left" valign="top">
                  <div class="row">
                    <div class="six_half columns">
                      <asp:Label runat="server" Text="" ID="maitenance_label"></asp:Label>
                      <asp:Label ID="ac_maint_left" runat="server"></asp:Label>
                    </div>
                    <div class="six_half columns">
                      <asp:Label runat="server" Text="" ID="maitenance_label_client"></asp:Label> 
                      <asp:Label ID="ac_maint_right" runat="server"></asp:Label> 
                    </div>
                    <div class="six_half columns float_right">
                      <asp:Label runat="server" ID="maitenance_label_notes"></asp:Label> 
                    </div>
                  </div>
                </td>
              </tr>
            </table>
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="equipment_tab" runat="server" HeaderText="EQUIP">
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label runat="server" ID="equipment_warning_text" ForeColor="Red" Font-Bold="true"></asp:Label>
            <table width="100%" cellpadding="2" cellspacing="0">
              <tr>
                <td align="left" valign="top">
                  <div class="row">
                    <div class="six_half columns">
                      <asp:Label runat="server" Text="" ID="equipment_label"></asp:Label>
                    </div>
                    <div class="six_half columns">
                      <asp:Label runat="server" Text="" ID="equipment_label_client"></asp:Label>
                    </div>
                    <div class="six_half columns float_right">
                      <asp:Label runat="server" ID="equipment_label_notes"></asp:Label>
                    </div>
                  </div>
                </td>
              </tr>
            </table>
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="int_tab" runat="server" HeaderText="INT/EXT">
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label runat="server" ID="int_warning_text" ForeColor="Red" Font-Bold="true"></asp:Label>
            <table width="100%" cellpadding="2" cellspacing="0">
              <tr>
                <td align="left" valign="top">
                  <div class="row">
                    <div class="six_half columns">
                      <asp:Label runat="server" Text="" ID="interior_label"></asp:Label>
                      <asp:Label runat="server" Text="" ID="exterior_label"></asp:Label>
                    </div>
                    <div class="six_half columns">
                      <asp:Label runat="server" Text="" ID="interior_label_client"></asp:Label>
                      <asp:Label runat="server" Text="" ID="exterior_label_client"></asp:Label>
                    </div>
                    <div class="six_half columns float_right">
                      <asp:Label runat="server" ID="interior_label_notes"></asp:Label>
                      <asp:Label runat="server" ID="exterior_label_notes"></asp:Label>
                    </div>
                </td>
              </tr>
            </table>
            <asp:TextBox runat="server" ID="other_tab_time" Style="display: none;" />
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="cockpit_tab" runat="server" HeaderText="COCKPIT">
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label runat="server" ID="cockpit_warning_text" ForeColor="Red" Font-Bold="true"></asp:Label>
            <table width="100%" cellpadding="2" cellspacing="0">
              <tr>
                <td align="left" valign="top">
                  <div class="row">
                    <div class="six_half columns">
                      <asp:Label runat="server" Text="" ID="cockpit_label"></asp:Label>
                    </div>
                    <div class="six_half columns">
                      <asp:Label runat="server" Text="" ID="cockpit_label_client"></asp:Label>
                    </div>
                    <div class="six_half columns float_right">
                      <asp:Label runat="server" ID="cockpit_label_notes"></asp:Label>
                    </div>
                  </div>
                </td>
              </tr>
            </table>
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="event_tab" runat="server" HeaderText="EVENTS">
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label runat="server" ID="event_warning_text" ForeColor="Red" Font-Bold="true"></asp:Label>
            <asp:Label runat="server" ID="event_status" ForeColor="Red" Font-Bold="true"></asp:Label>
            <table width="100%" cellspacing="0" cellpadding="2">
              <tr>
                <td align="left" valign="top">
                  <div class="row">
                    <div class="six_half columns">
                      <asp:Label runat="server" Text="" ID="event_label"></asp:Label>
                    </div>
                    <div class="six_half columns">
                      <asp:Label runat="server" ID="event_label_notes"></asp:Label>
                    </div>
                  </div>
                </td>
              </tr>
            </table>
            <asp:TextBox runat="server" ID="events_tab_time" Style="display: none;" />
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="apu_tab" runat="server" HeaderText="APU">
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label runat="server" ID="apu_warning_text" ForeColor="Red" Font-Bold="true"></asp:Label>
            <table width="100%" cellspacing="0" cellpadding="2">
              <tr>
                <td align="left" valign="top">
                  <div class="row">
                    <div class="six_half columns">
                      <asp:Label runat="server" Text="" ID="apu_label"></asp:Label>
                    </div>
                    <div class="six_half columns">
                      <asp:Label runat="server" Text="" ID="apu_label_client"></asp:Label>
                    </div>
                    <div class="six_half columns float_right">
                      <asp:Label runat="server" ID="apu_label_notes"></asp:Label></div>
                  </div>
                </td>
              </tr>
            </table>
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="notes_tab" runat="server" HeaderText="NOTES">
        <ContentTemplate>
          <div class="tab_container_div">
            <table width="100%" cellspacing="0" cellpadding="2">
              <tr>
                <td align="right" valign="top">
                  <asp:Panel ID="Panel1" Width="100%" runat="server" HorizontalAlign="right">
                    <asp:Label runat="server" Text="" ID="notes_list"></asp:Label></asp:Panel>
                </td>
              </tr>
            </table>
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="action_tab" runat="server" HeaderText="ACTION">
        <ContentTemplate>
          <div class="tab_container_div">
            <table width="100%" cellspacing="0" cellpadding="2">
              <tr>
                <td align="right" valign="top">
                  <asp:Panel ID="action_pnl" Width="100%" runat="server" HorizontalAlign="right">
                    <asp:Label runat="server" Text="" ID="action_label"></asp:Label></asp:Panel>
                </td>
              </tr>
            </table>
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="opportunities_tab" runat="server" HeaderText="DOCS" Visible="false">
        <ContentTemplate>
          <div class="tab_container_div">
            <table width="100%" cellspacing="0" cellpadding="2">
              <tr>
                <td align="left" valign="top">
                  <asp:Panel ID="Panel3" Width="100%" runat="server" HorizontalAlign="right">
                    <asp:Label runat="server" Text="" ID="document_label"></asp:Label></asp:Panel>
                </td>
              </tr>
            </table>
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="props_tab" runat="server" HeaderText="PROPS" Visible="false">
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label runat="server" ID="props_warning_text" ForeColor="Red" Font-Bold="true"></asp:Label>
            <table width="100%" cellpadding="2" cellspacing="0">
              <tr>
                <td align="left" valign="top" width="50%">
                  <asp:Label runat="server" Text="" ID="props_label"></asp:Label>
                </td>
                <td>
                  &nbsp;
                </td>
                <td align="left" valign="top" width="50%">
                  <asp:Label runat="server" Text="" ID="props_label_client"></asp:Label>
                </td>
              </tr>
              <tr>
                <td colspan="2">
                  &nbsp;
                </td>
                <td align="right" valign="top">
                  <asp:Label runat="server" ID="props_label_notes"></asp:Label>
                </td>
              </tr>
            </table>
            <asp:TextBox runat="server" ID="props_tab_time" Style="display: none;" />
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="prospects_tab" runat="server" HeaderText="PROSPECTS">
        <ContentTemplate>
          <div class="tab_container_div">
            <table width="100%" cellspacing="0" cellpadding="2">
              <tr>
                <td align="left" valign="top">
                  <asp:UpdatePanel runat="server" ID="prospectUpdatePanel">
                    <ContentTemplate>
                      <asp:Label runat="server" ID="prospectQuery"></asp:Label>
                      <p>
                        <asp:DropDownList runat="server" ID="changeProspectDropdown" AutoPostBack="true">
                          <asp:ListItem Selected="True" Value="1">Display Only Prospects for My Aircraft</asp:ListItem>
                        </asp:DropDownList>
                      </p>
                      <asp:Panel ID="Panel2" Width="100%" runat="server" HorizontalAlign="right">
                        <asp:Label runat="server" Text="" ID="prospect_label"></asp:Label></asp:Panel>
                    </ContentTemplate>
                  </asp:UpdatePanel>
                </td>
              </tr>
            </table>
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="value_tab" runat="server" HeaderText="VALUE">
        <ContentTemplate>
          <div class="tab_container_div">
            <table width="100%" cellpadding="2" cellspacing="0">
              <tr>
                <td align="left" valign="top">
                  <div class="row">
                    <div class="six_half columns">
                      <asp:Label runat="server" ID="aircraftValueMessage" ForeColor="Red" Font-Bold="true"
                        CssClass="display_none"><p>You currently have multiple open market value analysis records for this aircraft. Please select from the list below.</p></asp:Label>
                      <asp:Label runat="server" ID="aircraft_value_list_label" />
                      <asp:TextBox runat="server" ID="aircraft_value_time" Style="display: none;" />
                    </div>
                    <div class="six_half columns">
                      <div id="chart_div_value_history">
                      </div>
                      <asp:Label runat="server" ID="aircraft_value_history_label"></asp:Label>
                      <asp:Chart ID="valuation_chart" runat="server" ImageStorageMode="UseImageLocation"
                        ImageType="Jpeg" Visible="False">
                        <Series>
                          <asp:Series>
                          </asp:Series>
                        </Series>
                        <ChartAreas>
                          <asp:ChartArea Name="ChartArea1">
                          </asp:ChartArea>
                        </ChartAreas>
                      </asp:Chart>
                    </div>
                    <div class="six_half columns float_right">
                      <asp:Label runat="server" ID="value_label_notes" EnableViewState="true"></asp:Label></div>
                  </div>
                </td>
              </tr>
            </table>
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
    </cc1:TabContainer>
  </ContentTemplate>
</asp:UpdatePanel>
