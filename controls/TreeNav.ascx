<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TreeNav.ascx.vb" Inherits="crmWebClient.TreeNav" %>
<div>
  <asp:TreeView ID="left_nav_tv" runat="server" Font-Names="helvetica" Width="100%"
    CssClass="display_none" ImageSet="BulletedList3" NodeIndent="0" ExpandDepth="2"
    ShowExpandCollapse="False" NodeWrap="true">
    <ParentNodeStyle Font-Bold="false" ForeColor="Black" />
    <HoverNodeStyle Font-Underline="True" ForeColor="#204763" />
    <SelectedNodeStyle Font-Underline="True" ForeColor="Black" HorizontalPadding="4px"
      VerticalPadding="0px" />
    <RootNodeStyle ChildNodesPadding="0px" Font-Bold="false" ForeColor="black" />
    <NodeStyle Font-Names="Verdana" Font-Size="8px" ForeColor="Black" HorizontalPadding="2px"
      NodeSpacing="0px" VerticalPadding="2px" />
  </asp:TreeView>
  <asp:BulletedList runat="server" ID="orderedTreeList" DisplayMode="LinkButton">
  </asp:BulletedList>
  <ul style="margin-top:-15px;">
    <li class="expanded"><a href="#" onclick="javascript:load('view_template.aspx?ViewID=1&noMaster=false','','scrollbars=yes,menubar=no,height=700,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;">
      Model Market Summary</a></li><li class="expanded"><a href="performance_specs.aspx">Performance Specs</a></li><li class="expanded">
        <a href="op_costs.aspx">Operating Costs</a></li></ul>
  <asp:CheckBox ID="show_hidden" runat="server" Text="Show Hidden Folders?" AutoPostBack="true"
    Font-Size="Smaller" />
</div>
