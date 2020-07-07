<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="listing.aspx.vb" Inherits="crmWebClient._listing"
  MasterPageFile="~/main_site.Master" %>

<%@ MasterType VirtualPath="~/main_site.Master" %>
<%@ Import Namespace="crmWebClient.clsGeneral" %>
<%@ Register TagPrefix="obout" Namespace="OboutInc.Flyout2" Assembly="obout_Flyout2_NET" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:DataGrid runat="server" ID="Results" CellPadding="3" OnItemCommand="dispDetails"
    Width="100%" AllowPaging="true" PageSize="25" AllowSorting="True" Font-Names="verdana"
    AutoGenerateColumns="false" PagerStyle-Mode="NumericPages" GridLines="None" CssClass="mGrid"
    PagerStyle-CssClass="pgr" AlternatingItemStyle-CssClass="alt" ItemStyle-CssClass="item_row"
    ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="th">
    <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" ForeColor="White" />
    <AlternatingItemStyle CssClass="alt_row" />
    <ItemStyle BorderStyle="None" VerticalAlign="Top" />
    <HeaderStyle Wrap="False" HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
    <Columns>
      <asp:TemplateColumn HeaderText="Company">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <%#IIf(DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") <> 0, "<a href='details.aspx?source=CLIENT&type=1&comp_ID=" & DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") & "'", "<a href='details.aspx?source=JETNET&type=1&comp_ID=" & DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id") & "'")%>
          <%#Master.what_comp(DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id"), DataBinder.Eval(Container.DataItem, "lnote_client_comp_id"), 1)%>
          </a><%#Master.what_comp(DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id"), DataBinder.Eval(Container.DataItem, "lnote_client_comp_id"), 2)%><asp:ImageButton
            ID="Button1" ImageUrl="~/images/magnify.png" runat="server" Style="text-align: center;"
            Visible='<%# IIF(DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") = 0 and DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id") = 0, "false", "true")%>' />
          <obout:Flyout ID="Flyout1" runat="server" AttachTo="Button1" Position="TOP_RIGHT"
            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true" Visible='<%# IIF(DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") = 0 and DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id") = 0, "false", "true")%>'>
            <%#clsGeneral.MouseOverTextStart() %>
            <%#Master.createANoteAddressPopOut(Eval("lnote_jetnet_comp_id"), Eval("lnote_client_comp_id"))%>
            <%#clsGeneral.MouseOverTextEnd()%>
          </obout:Flyout>
          <%#IIf(DataBinder.Eval(Container.DataItem, "lnote_client_contact_id") > 0 or DataBinder.Eval(Container.DataItem, "lnote_jetnet_contact_id") > 0, "<strong class=""strongContactInformation"">Contact Information:</strong>", "") %>
          <%#master.what_contact(DataBinder.Eval(Container.DataItem, "lnote_jetnet_contact_id"), DataBinder.Eval(Container.DataItem, "lnote_client_contact_id"))%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Status">
        <ItemTemplate>
          <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="10px" />
          <%#IIf(DataBinder.Eval(Container.DataItem, "lnote_opportunity_status") = "O", "Open", "Closed")%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Value">
        <ItemTemplate>
          <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="10px" />
          <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "lnote_cash_value")), "$" & FormatNumber(DataBinder.Eval(Container.DataItem, "lnote_cash_value"), 0), "")%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="%">
        <ItemTemplate>
          <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="10px" />
          <%#DataBinder.Eval(Container.DataItem, "lnote_capture_percentage")%>%
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Opportunity Description">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <%#clsGeneral.DisplayDocumentsDescription(DataBinder.Eval(Container.DataItem, "lnote_note"), DataBinder.Eval(Container.DataItem, "lnote_id"))%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Action Date">
        <ItemTemplate>
          <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="10px" />
          <a href="#" onclick="javascript:window.open('edit_note.aspx?action=edit&type= <%#IIf((DataBinder.Eval(Container.DataItem, "lnote_status") = "O"), "opportunity", "email")%>&id=<%#(DataBinder.Eval(Container.DataItem, "lnote_id"))%>','','scrollbars=no,menubar=no,height=600,width=880,resizable=yes,toolbar=no,location=no,status=no');"
            href="#">
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "lnote_schedule_start_date")), DateAdd("h", Session("timezone_offset"), FormatDateTime(DataBinder.Eval(Container.DataItem, "lnote_schedule_start_date"))), "")%></a>
          <br />
          By:
          <%#Master.what_user((DataBinder.Eval(Container.DataItem, "lnote_user_login")))%><br />
          <%#IIf(DataBinder.Eval(Container.DataItem, "lnote_status") = "O", "Assigned To: " & Master.what_user(DataBinder.Eval(Container.DataItem, "lnote_user_id")), "")%>
          <br />
          <img src="images/spacer.gif" width="100" alt="" height="1" />
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:BoundColumn DataField="lnote_jetnet_comp_id" Visible="false" />
      <asp:BoundColumn DataField="lnote_client_comp_id" Visible="false" />
      <asp:BoundColumn DataField="lnote_jetnet_ac_id" Visible="false" />
      <asp:BoundColumn DataField="lnote_client_ac_id" Visible="false" />
      <asp:TemplateColumn HeaderText="Category">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <%#Master.what_opportunity_cat(DataBinder.Eval(Container.DataItem, "lnote_notecat_key"), False)%>
        </ItemTemplate>
      </asp:TemplateColumn>
    </Columns>
  </asp:DataGrid>
</asp:Content>
