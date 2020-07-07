<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="listing.aspx.vb" Inherits="crmWebClient._listing"
    MasterPageFile="~/main_site.Master" %>

<%@ Register TagPrefix="obout" Namespace="OboutInc.Flyout2" Assembly="obout_Flyout2_NET" %>
<%@ MasterType VirtualPath="~/main_site.Master" %>
<%@ Import Namespace="crmWebClient.clsGeneral" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DataGrid runat="server" ID="Results" CellPadding="3" OnItemCommand="dispDetails"
        Width="100%" AllowPaging="true" PageSize="25" BorderStyle="None"
        AllowSorting="True" AutoGenerateColumns="false" PagerStyle-Mode="NumericPages"  GridLines="None"
        CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingItemStyle-CssClass="alt"
        ItemStyle-CssClass="item_row" ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="th">
        <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" Font-Bold="false" Font-Underline="false"
            ForeColor="White" />
        <AlternatingItemStyle CssClass="alt_row" />
        <ItemStyle BorderStyle="None" VerticalAlign="Top" />
        <HeaderStyle Wrap="False" HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="">
                <ItemTemplate>
                    <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="10px" />
                    <input onclick="javascript:append_cookie('<%#(DataBinder.Eval(Container.DataItem, "contact_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "contact_type"))%>','contacts_marked');"
                        type="checkbox" id='<%#(DataBinder.Eval(Container.DataItem, "contact_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "contact_type"))%>'
                        value='<%#(DataBinder.Eval(Container.DataItem, "contact_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "contact_type"))%>'
                        style="<%#master.IsInCookie(DataBinder.Eval(Container.DataItem, "contact_id") & "#" & ucase(DataBinder.Eval(Container.DataItem, "contact_type")))%>" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "contact_type"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="contact_comp_id" Visible="false" />
            <asp:BoundColumn DataField="contact_type" Visible="false" />
            <asp:BoundColumn DataField="contact_id" Visible="false" />
            <asp:TemplateColumn HeaderText="Name">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <a href="details.aspx?contact_ID=<%#DataBinder.Eval(Container.DataItem, "contact_id")%>&comp_ID=<%#DataBinder.Eval(Container.DataItem, "contact_comp_id")%>&type=1&source=<%#DataBinder.Eval(Container.DataItem, "contact_type")%>">
                        <%#DataBinder.Eval(Container.DataItem, "contact_sirname")%>&nbsp;
                        <%#DataBinder.Eval(Container.DataItem, "contact_first_name")%>&nbsp;
                        <%#DataBinder.Eval(Container.DataItem, "contact_last_name")%>
                    </a>
                    <%#Master.createAContactPopOutPhone(Eval("contact_id"), Eval("contact_type"), Eval("contact_comp_id"))%><asp:ImageButton
                        ID="ImageButton2" ImageUrl="~/images/magnify.png" runat="server" Style="text-align: center;"
                        Visible='<%# IIF(master.ContactPhone = "", "false", "true")%>' />
                    <obout:Flyout ID="Flyoutcontact" runat="server" AttachTo="ImageButton2" Position="TOP_RIGHT"
                        Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true" Visible='<%#  IIF(master.ContactPhone = "", "false", "true")%>'>
                        <%#clsGeneral.MouseOverTextStart() %>
                        <%#Master.ContactPhone%>
                        <%#clsGeneral.MouseOverTextEnd()%>
                    </obout:Flyout>
                    </a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Title">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <a href="details.aspx?contact_ID=<%#DataBinder.Eval(Container.DataItem, "contact_id")%>&comp_ID=<%#DataBinder.Eval(Container.DataItem, "contact_comp_id")%>&type=1&source=<%#DataBinder.Eval(Container.DataItem, "contact_type")%>">
                        <%#DataBinder.Eval(Container.DataItem, "contact_title")%>
                    </a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Company Name">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <a href="details.aspx?comp_ID=<%#DataBinder.Eval(Container.DataItem, "contact_comp_id")%>&type=1&source=<%#DataBinder.Eval(Container.DataItem, "contact_type")%>">
                        <%#DataBinder.Eval(Container.DataItem, "comp_name")%>
                    </a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Location">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#DataBinder.Eval(Container.DataItem, "comp_address1")%>
                    <%#DataBinder.Eval(Container.DataItem, "comp_city")%>,
                    <%#DataBinder.Eval(Container.DataItem, "comp_state")%>
                    <%#DataBinder.Eval(Container.DataItem, "comp_country")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="">
                <ItemTemplate>
                    <itemstyle horizontalalign="center" verticalalign="top" />
                    <headerstyle horizontalalign="center" />
                    <div align="center">
                        <%#Master.ViewNoteAttachedACComp(Eval("contact_comp_id"), Eval("contact_type"), 1, "A", Eval("contact_id"),"JETNET")%>
                        <asp:LinkButton runat="server" ID="viewnote" OnClientClick="return false;">
                         <%#IIf(Master.noteactext <> "", "<img src='images/document.png' alt='Notes Attached to Contact' border='0' />", "")%></asp:LinkButton>
                        <obout:Flyout ID="note_fly" runat="server" AttachTo="viewnote" Position="TOP_LEFT"
                            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true">
                            <%#clsGeneral.MouseOverTextStart() %>
                            <%#Master.noteactext%>
                            <%#clsGeneral.MouseOverTextEnd()%>
                        </obout:Flyout>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="">
                <ItemTemplate>
                    <itemstyle horizontalalign="center" verticalalign="top" />
                    <headerstyle horizontalalign="center" />
                    <div align="center">
                        <%#Master.ViewNoteAttachedACComp(Eval("contact_comp_id"), Eval("contact_type"), 1, "P", Eval("contact_id"), "JETNET")%>
                        <asp:LinkButton runat="server" ID="viewact" OnClientClick="return false;">
                        <%#IIf(Master.noteactext <> "", "<img src='images/red_pin.png' alt='Action Attached to Aircraft' border='0' />", "")%>                        
                        </asp:LinkButton>
                        <obout:Flyout ID="note_flyac" runat="server" AttachTo="viewact" Position="TOP_LEFT"
                            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true">
                            <%#clsGeneral.MouseOverTextStart() %>
                            <%#Master.noteactext%>
                            <%#clsGeneral.MouseOverTextEnd()%>
                        </obout:Flyout>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</asp:Content>
