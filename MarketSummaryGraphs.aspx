<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="MarketSummaryGraphs.aspx.vb"
  Inherits="crmWebClient.MarketSummaryGraphs" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script language="javascript" type="text/javascript">
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
  <div style="text-align: left;">
    <asp:UpdatePanel ID="graphs_update" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
      <ContentTemplate>
        <asp:Table ID="buttonsTable" CellPadding="3" CellSpacing="0" Width="100%" class="DetailsBrowseTable"
          runat="server">
          <asp:TableRow>
            <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="right" VerticalAlign="middle">
              <div class="backgroundShade">
                <a href="#" onclick="javascript:load('help.aspx','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"
                  class="gray_button float_left noBefore" title="Show Market Summary Graphs Help"><strong>Help</strong>
                </a>
                <asp:LinkButton ID="close_button" runat="server" OnClientClick="javascript:window.close();"
                  CssClass="gray_button float_left"><strong>Close</strong></asp:LinkButton>
              </div>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
        <div class="NotesHeader" style="margin-bottom: 3px;">
        </div>
        <br />
        <asp:Literal ID="debug_output" runat="server"></asp:Literal>
        <asp:Panel ID="market_summary_graph_panel" runat="server" Visible="false" HorizontalAlign="Center">
          <asp:Chart ID="market_summary_graph_chart" Visible="False" runat="server" ImageStorageMode="UseImageLocation"
            ImageType="Jpeg">
            <series>
              <asp:Series Name="Series1" ChartArea="ChartArea1">
              </asp:Series>
            </series>
            <chartareas>
              <asp:ChartArea Name="ChartArea1">
              </asp:ChartArea>
            </chartareas>
          </asp:Chart>
        </asp:Panel>
        <br />
        <asp:Literal ID="graph_image" runat="server"></asp:Literal>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</asp:Content>
