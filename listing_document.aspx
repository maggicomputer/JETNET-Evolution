<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="listing.aspx.vb" Inherits="crmWebClient._listing"
    MasterPageFile="~/main_site.Master" %>


<%@ Import Namespace="crmWebClient.clsGeneral" %>
<%@ Register TagPrefix="obout" Namespace="OboutInc.Flyout2" Assembly="obout_Flyout2_NET" %>
<%@ MasterType VirtualPath="~/main_site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
    <asp:DataGrid runat="server" ID="Results" CellPadding="3" 
        OnItemCommand="dispDetails" 
        Width="100%" AllowPaging="true" PageSize="25" 
        AllowSorting="True" AutoGenerateColumns="false" 
        PagerStyle-Mode="NumericPages"  GridLines="None"
        CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingItemStyle-CssClass="alt"
        ItemStyle-CssClass="item_row" ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="th">
         <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" ForeColor="White" />
        <AlternatingItemStyle CssClass="alt_row" />
        <ItemStyle BorderStyle="None" VerticalAlign="Top" />
        <HeaderStyle Wrap="False" HorizontalAlign="left" VerticalAlign="Middle">
        </HeaderStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#clsGeneral.DisplayDocuments(DataBinder.Eval(Container.DataItem, "lnote_document_name"), DataBinder.Eval(Container.DataItem, "lnote_document_flag"), False, DataBinder.Eval(Container.DataItem, "lnote_id"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Date">
                <ItemTemplate>
                    <itemstyle horizontalalign="center" verticalalign="top" /> 
                    <headerstyle />
                    <a href="#" onclick="javascript:window.open('edit_note.aspx?action=edit&type=documents&id=<%#(DataBinder.Eval(Container.DataItem, "lnote_id"))%>','','scrollbars=no,menubar=no,height=600,width=880,resizable=yes,toolbar=no,location=no,status=no');">
                        <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "lnote_entry_date")), DateAdd("h", Session("timezone_offset"), FormatDateTime(DataBinder.Eval(Container.DataItem, "lnote_entry_date"))), "")%></a>
                    <br />
                    By:
                    <%#clsGeneral.what_user((DataBinder.Eval(Container.DataItem, "lnote_user_login")), Nothing, Master)%><br />
                    For:
                    <%#clsGeneral.what_user((DataBinder.Eval(Container.DataItem, "lnote_user_id")), Nothing, Master)%><br />
                    <img src='images/spacer.gif' alt='' border='0' width='160' height='1' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Doc Title/Desc">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#clsGeneral.DisplayDocumentsDescription(DataBinder.Eval(Container.DataItem, "lnote_note"), DataBinder.Eval(Container.DataItem, "lnote_id"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="lnote_jetnet_comp_id" Visible="false" />
            <asp:BoundColumn DataField="lnote_client_comp_id" Visible="false" />
            <asp:TemplateColumn HeaderText="Category">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#Master.what_cat(DataBinder.Eval(Container.DataItem, "lnote_notecat_key"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Aircraft">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <%#Master.what_ac(DataBinder.Eval(Container.DataItem, "lnote_jetnet_ac_id"), DataBinder.Eval(Container.DataItem, "lnote_client_ac_id"), 2)%>
                  
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Company">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#IIf(DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") <> 0, "<a href='details.aspx?source=CLIENT&type=1&comp_ID=" & DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") & "'", "<a href='details.aspx?source=JETNET&type=1&comp_ID=" & DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id") & "'")%>
                    <%#Master.what_comp(DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id"), DataBinder.Eval(Container.DataItem, "lnote_client_comp_id"), 1)%>
                    </a><%#Master.what_comp(DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id"), DataBinder.Eval(Container.DataItem, "lnote_client_comp_id"), 2)%>
                
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Contact">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#master.what_contact(DataBinder.Eval(Container.DataItem, "lnote_jetnet_contact_id"), DataBinder.Eval(Container.DataItem, "lnote_client_contact_id"))%>
                   
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</asp:Content>