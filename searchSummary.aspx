<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="searchSummary.aspx.vb"
  StylesheetTheme="Evo" Inherits="crmWebClient.searchSummary" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
  <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width='100%' runat="server"
    class="DetailsBrowseTable">
    <asp:TableRow>
      <asp:TableCell HorizontalAlign="right" VerticalAlign="middle">
              <div class="backgroundShade">
                <a href="#" onclick="javascript:window.close();" class="float_left"><img src="images/x.svg" alt="Close" /></a>
              </div>
      </asp:TableCell>
    </asp:TableRow>
  </asp:Table>
  <asp:Table runat="server" ID="table_container" Width="100%">
    <asp:TableRow runat="server">
      <asp:TableCell runat="server" VerticalAlign="Top" HorizontalAlign="Left" Width="25%" ID="leftTabContainerCell">
        <!--Folder Tab Display-->
        <cc1:TabContainer ID="folder_container" runat="server" Visible="true" CssClass="dark-theme"
          Width="100%">
          <cc1:TabPanel ID="folder_tab" runat="server" Visible="true" HeaderText="Summary Options">
            <ContentTemplate>
              <asp:TreeView ID="general_tree" runat="server" SkinID="project_tree_view" CssClass="aircraft_folder" Visible="false">
              </asp:TreeView>
              <asp:Label runat="server" ID="holder_for_new_tree_views">
              </asp:Label>
              <asp:Label runat="server" ID="holder_for_new_table">
              </asp:Label>
            </ContentTemplate>
          </cc1:TabPanel>
        </cc1:TabContainer>
        <!--End Folder Tab Display>-->
      </asp:TableCell>
      <asp:TableCell runat="server" VerticalAlign="Top" HorizontalAlign="Left">
        <!--Summary Results, Placeholder for now-->
        <cc1:TabContainer ID="summary_container" runat="server" Visible="true" CssClass="dark-theme"
          Width="100%">
          <cc1:TabPanel ID="summary_tab" runat="server" Visible="true" HeaderText="Summary Information">
            <ContentTemplate>
              <asp:Label runat="server" ID="summary_table_label"></asp:Label>
            </ContentTemplate>
          </cc1:TabPanel>
          <cc1:TabPanel ID="second_summary_tab" runat="server" Visible="false" HeaderText="Summary Information">
            <ContentTemplate>
              <asp:Label runat="server" ID="second_summary_table_label"></asp:Label>
            </ContentTemplate>
          </cc1:TabPanel>
        </cc1:TabContainer>
        <!--End Summary Results-->
      </asp:TableCell>
    </asp:TableRow>
  </asp:Table>

</asp:Content>
