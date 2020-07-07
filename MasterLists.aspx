<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="MasterLists.aspx.vb" Inherits="crmWebClient.MasterLists"
  MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <div id="outerDiv" class="valueSpec viewValueExport Simplistic aircraftSpec gray_background" runat="server">
    <table border="0" style="padding: 4px; border-spacing: 6px; text-align: left; width: 100%;">
      <tr>
        <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">
          <asp:Label ID="close_window_only" runat="server" CssClass="float_right criteria_text"></asp:Label>

          <cc1:TabContainer runat="server" ID="tab_container_ID" Width="100%" BorderStyle="None"
            CssClass="dark-theme">
            <cc1:TabPanel ID="masterList" runat="server" HeaderText="Master List">
              <ContentTemplate>
                <asp:Table ID="helpMasterListTable" runat="server" HorizontalAlign="Left">
                  <asp:TableRow ID="TableRow" runat="server">
                    <asp:TableCell ID="TableCell" runat="server">
                      <img src="images/ch_red.jpg" alt="Weight Class" title="Weight Class" />
                      <asp:HyperLink ID="HyperLink1" runat="server" Text="Weight Class" ToolTip="Weight Class"
                        Style="" Target="_self" NavigateUrl="?helplist=weightclass">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow1" runat="server">
                    <asp:TableCell ID="TableCell1" runat="server">
                      <img src="images/ch_red.jpg" alt="Weight Classes by Make/Model" title="Weight Classes by Make/Model" />
                      <asp:HyperLink ID="HyperLink2" runat="server" Text="Weight Classes by Make/Model"
                        ToolTip="Weight Classes by Make/Model" Style="" Target="_self" NavigateUrl="?helplist=weightclassmodel">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow2" runat="server">
                    <asp:TableCell ID="TableCell2" runat="server">
                      <img src="images/ch_red.jpg" alt="Lifecycle" title="Lifecycle" />
                      <asp:HyperLink ID="HyperLink3" runat="server" Text="Aircraft Lifecycle" ToolTip="Aircraft Lifecycle"
                        Style="" Target="_self" NavigateUrl="?helplist=lifecycle">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow3" runat="server">
                    <asp:TableCell ID="TableCell3" runat="server">
                      <img src="images/ch_red.jpg" alt="Serial Number" title="Serial Number" />
                      <asp:HyperLink ID="HyperLink4" runat="server" Text="Serial Number" ToolTip="Serial Number"
                        Style="" Target="_self" NavigateUrl="?helplist=serial">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow4" runat="server">
                    <asp:TableCell ID="TableCell4" runat="server">
                      <img src="images/ch_red.jpg" alt="Registration Number" title="Registration Number" />
                      <asp:HyperLink ID="HyperLink5" runat="server" Text="Registration Number" ToolTip="Registration Number"
                        Style="" Target="_self" NavigateUrl="?helplist=registration">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow5" runat="server">
                    <asp:TableCell ID="TableCell5" runat="server">
                      <img src="images/ch_red.jpg" alt="Feature Codes" title="Feature Codes" />
                      <asp:HyperLink ID="HyperLink6" runat="server" Text="Feature Codes" ToolTip="Feature Codes"
                        Style="" Target="_self" NavigateUrl="?helplist=feature">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow6" runat="server">
                    <asp:TableCell ID="TableCell6" runat="server">
                      <img src="images/ch_red.jpg" alt="Feature Codes by Make/Model" title="Feature Codes by Make/Model" />
                      <asp:HyperLink ID="HyperLink7" runat="server" Text="Feature Codes by Make/Model"
                        ToolTip="Feature Codes by Make/Model" Style="" Target="_self" NavigateUrl="?helplist=featuremodel">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow7" runat="server">
                    <asp:TableCell ID="TableCell7" runat="server">
                      <img src="images/ch_red.jpg" alt="Avionics" title="Avionics" />
                      <asp:HyperLink ID="HyperLink8" runat="server" Text="Avionics" ToolTip="Avionics"
                        Style="" Target="_self" NavigateUrl="?helplist=avionics">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow8" runat="server">
                    <asp:TableCell ID="TableCell8" runat="server">
                      <img src="images/ch_red.jpg" alt="Engine Models Prefixes by Make/Model" title="Engine Models Prefixes by Make/Model" />
                      <asp:HyperLink ID="HyperLink9" runat="server" Text="Engine Models Prefixes by Make/Model"
                        ToolTip="Engine Models Prefixes by Make/Model" Style="" Target="_self" NavigateUrl="?helplist=engineprefix">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow9" runat="server">
                    <asp:TableCell ID="TableCell9" runat="server">
                      <img src="images/ch_red.jpg" alt="Engine Maintenance Program (EMP)" title="Engine Maintenance Program (EMP)" />
                      <asp:HyperLink ID="HyperLink10" runat="server" Text="Engine Maintenance Program (EMP)"
                        ToolTip="EMP" Style="" Target="_self" NavigateUrl="?helplist=emp">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow10" runat="server">
                    <asp:TableCell ID="TableCell10" runat="server">
                      <img src="images/ch_red.jpg" alt="Engine Management Program (EMGP)" title="Engine Management Program (EMGP)" />
                      <asp:HyperLink ID="HyperLink11" runat="server" Text="Engine Management Program (EMGP)"
                        ToolTip="EMGP" Style="" Target="_self" NavigateUrl="?helplist=emgp">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow11" runat="server">
                    <asp:TableCell ID="TableCell11" runat="server">
                      <img src="images/ch_red.jpg" alt="Airframe Maintenance Program (AMP)" title="Airframe Maintenance Program (AMP)" />
                      <asp:HyperLink ID="HyperLink12" runat="server" Text="Airframe Maintenance Program (AMP)"
                        ToolTip="AMP" Style="" Target="_self" NavigateUrl="?helplist=amp">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow12" runat="server">
                    <asp:TableCell ID="TableCell12" runat="server">
                      <img src="images/ch_red.jpg" alt="Weight Class" title="Airframe Maintenance Tracking Program (AMTP)" />
                      <asp:HyperLink ID="HyperLink13" runat="server" Text="Airframe Maintenance Tracking Program (AMTP)"
                        ToolTip="AMTP" Style="" Target="_self" NavigateUrl="?helplist=amtp">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow13" runat="server">
                    <asp:TableCell ID="TableCell13" runat="server">
                      <img src="images/ch_red.jpg" alt="Transaction Codes" title="Transaction Codes" />
                      <asp:HyperLink ID="HyperLink14" runat="server" Text="Transaction Codes" ToolTip="Transaction Codes"
                        Style="" Target="_self" NavigateUrl="?helplist=transactioncodes">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow14" runat="server">
                    <asp:TableCell ID="TableCell14" runat="server">
                      <img src="images/ch_red.jpg" alt="Contact Types" title="Contact Types" />
                      <asp:HyperLink ID="HyperLink15" runat="server" Text="Contact Types" ToolTip="Contact Types"
                        Style="" Target="_self" NavigateUrl="?helplist=contacttypes">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow15" runat="server">
                    <asp:TableCell ID="TableCell15" runat="server">
                      <img src="images/ch_red.jpg" alt="Company Business Type" title="Company Business Type" />
                      <asp:HyperLink ID="HyperLink16" runat="server" Text="Company Business Type" ToolTip="Company Business Type"
                        Style="" Target="_self" NavigateUrl="?helplist=companybustype">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow16" runat="server">
                    <asp:TableCell ID="TableCell16" runat="server">
                      <img src="images/ch_red.jpg" alt="Aircraft Models by Subscription Level" title="Aircraft Models by Subscription Level" />
                      <asp:HyperLink ID="HyperLink18" runat="server" Text="Aircraft Models by Subscription Level"
                        ToolTip="Aircraft Models by Subscription Level" Style="" Target="_self" NavigateUrl="?helplist=aircraftmodelbustypes">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow17" runat="server">
                    <asp:TableCell ID="TableCell17" runat="server">
                      <img src="images/ch_red.jpg" alt="Business Aircraft Sizes" title="Business Aircraft Sizes" />
                      <asp:HyperLink ID="HyperLink17" runat="server" Text="Business Aircraft Sizes"
                        ToolTip="Business Aircraft Sizes" Style="" Target="_self" NavigateUrl="?helplist=bas">
                      </asp:HyperLink>
                    </asp:TableCell>
                  </asp:TableRow>
                </asp:Table>
              </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="displayList" runat="server" HeaderText="">
              <ContentTemplate>
                <div style="width: 60%; text-align: left;">
                  <asp:Literal ID="helpListContent" runat="server" Text="Please wait a moment for list ..."></asp:Literal>
                </div>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
        </td>
      </tr>
    </table>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">
</asp:Content>
