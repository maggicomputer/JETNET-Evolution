<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="listing.aspx.vb" Inherits="crmWebClient._listing"
    MasterPageFile="~/main_site.Master" EnableViewState="true" %>

<%@ MasterType VirtualPath="~/main_site.Master" %>
<%@ Import Namespace="crmWebClient.clsGeneral" %>
<%@ Register TagPrefix="obout" Namespace="OboutInc.Flyout2" Assembly="obout_Flyout2_NET" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlTooltip" runat="server">
    </asp:Panel>
    &nbsp;<asp:DataGrid runat="server" ID="Results" CellPadding="3" GridLines="None"
        OnItemCommand="dispDetails" Width="100%" AllowPaging="true" PageSize="25" AllowSorting="True"
        AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingItemStyle-CssClass="alt"
        ItemStyle-CssClass="item_row" ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="th"
        PagerStyle-NextPageText="Next" PagerStyle-PrevPageText="Previous" PagerStyle-Mode="NumericPages">
        <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" ForeColor="White" />
        <AlternatingItemStyle />
        <ItemStyle BorderStyle="None" VerticalAlign="Top" />
        <HeaderStyle Wrap="False" HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="">
                <ItemTemplate>
                    <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="10px" />
                    <input onclick="javascript:append_cookie('<%#(DataBinder.Eval(Container.DataItem, "comp_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "source"))%>','companies_marked');"
                        type="checkbox" id='<%#(DataBinder.Eval(Container.DataItem, "comp_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "source"))%>'
                        value='<%#(DataBinder.Eval(Container.DataItem, "comp_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "source"))%>'
                        style="<%#master.IsInCookie(DataBinder.Eval(Container.DataItem, "comp_id") & "#" & ucase(DataBinder.Eval(Container.DataItem, "source")))%>" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "source"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="comp_id" Visible="false" />
            <asp:BoundColumn DataField="source" Visible="false" />
            <asp:TemplateColumn HeaderText="Company Name">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#clsGeneral.Company_Popout(Eval("comp_email_address"), Eval("comp_web_address"), Eval("comp_id"), Eval("source"), Nothing, Master)%>
                    <a href="details.aspx?comp_ID=<%#DataBinder.Eval(Container.DataItem, "comp_id")%>&source=<%#DataBinder.Eval(Container.DataItem, "source")%>&type=1">
                        <%#DataBinder.Eval(Container.DataItem, "comp_name")%></a>
                    <asp:ImageButton ID="Button1" ImageUrl="~/images/magnify.png" runat="server" Style="text-align: center;"
                        OnClientClick="return false;" Visible='<%#clsGeneral.showPopout%>' />
                    <obout:Flyout ID="Flyout1" runat="server" AttachTo="Button1" Position="TOP_RIGHT"
                        Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true" Visible='<%#clsGeneral.showPopout%>'>
                        <%#clsGeneral.MouseOverTextStart() %>
                        <%#clsGeneral.Company_Popout_Text%>
                        <%#clsGeneral.MouseOverTextEnd() %>
                    </obout:Flyout>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="comp_name" Visible="false" />
            <asp:TemplateColumn HeaderText="Address">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#DataBinder.Eval(Container.DataItem, "comp_address1")%>
                    <%#DataBinder.Eval(Container.DataItem, "comp_address2")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="City/State">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#DataBinder.Eval(Container.DataItem, "comp_city")%>
                    <%#DataBinder.Eval(Container.DataItem, "comp_state")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Country">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#DataBinder.Eval(Container.DataItem, "comp_country")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Address" Visible="false">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#clsGeneral.Company_Listing_Address_Display(DataBinder.Eval(Container.DataItem, "comp_address1"), DataBinder.Eval(Container.DataItem, "comp_city"), DataBinder.Eval(Container.DataItem, "comp_state"), DataBinder.Eval(Container.DataItem, "comp_country"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn Visible="false">
                <ItemTemplate>
                    <itemstyle horizontalalign="center" verticalalign="top" />
                    <headerstyle horizontalalign="center" />
                    <%#DataBinder.Eval(Container.DataItem, "comp_category1")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn Visible="false">
                <ItemTemplate>
                    <itemstyle horizontalalign="center" verticalalign="top" />
                    <headerstyle horizontalalign="center" />
                    <%#DataBinder.Eval(Container.DataItem, "comp_category2")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn Visible="false">
                <ItemTemplate>
                    <itemstyle horizontalalign="center" verticalalign="top" />
                    <headerstyle horizontalalign="center" />
                    <%#DataBinder.Eval(Container.DataItem, "comp_category3")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn Visible="false">
                <ItemTemplate>
                    <itemstyle horizontalalign="center" verticalalign="top" />
                    <headerstyle horizontalalign="center" />
                    <%#DataBinder.Eval(Container.DataItem, "comp_category4")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn Visible="false">
                <ItemTemplate>
                    <itemstyle horizontalalign="center" verticalalign="top" />
                    <headerstyle horizontalalign="center" />
                    <%#DataBinder.Eval(Container.DataItem, "comp_category5")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="">
                <ItemTemplate>
                    <itemstyle horizontalalign="center" verticalalign="top" />
                    <headerstyle horizontalalign="center" />
                    <div align="center">
                        <%#Master.ViewNoteAttachedACComp(Eval("comp_id"), Eval("source"), 1, "A", 0, "JETNET")%>
                        <asp:LinkButton runat="server" ID="viewnote" OnClientClick="return false;">
                        <%#IIf(Master.noteactext <> "", "<img src='images/document.png' alt='Notes Attached to Aircraft' border='0' />", "")%>
                        </asp:LinkButton>
                        <obout:Flyout ID="note_fly" runat="server" AttachTo="viewnote" Position="TOP_LEFT"
                            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true" Visible='<%#IIf(Master.noteactext <> "", "true", "false")%>'>
                            <%#clsGeneral.MouseOverTextStart() %>
                            <%#Master.noteactext%>
                            <%#clsGeneral.MouseOverTextEnd()%>
                        </obout:Flyout>
                        <%#IIf(Session.Item("localUser").crmEvo = False, "<a href=""#"" style=""font-size: 9px;"" onclick=""javascript:load('edit_note.aspx?comp_ID=" & Eval("comp_id") & "&source=" & Eval("source") & "&type=note&action=new','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');"">[+]</a>", "")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="">
                <ItemTemplate>
                    <itemstyle horizontalalign="center" verticalalign="top" />
                    <headerstyle horizontalalign="center" />
                    <div align="center">
                        <%#Master.ViewNoteAttachedACComp(Eval("comp_id"), Eval("source"), 1, "P", 0, "JETNET")%>
                        <asp:LinkButton runat="server" ID="viewact" OnClientClick="return false;">
                        <%#IIf(Master.noteactext <> "", "<img src='images/red_pin.png' alt='Action Attached to Aircraft' border='0' />", "")%>
                        </asp:LinkButton>
                        <obout:Flyout ID="note_flyac" runat="server" AttachTo="viewact" Position="TOP_LEFT"
                            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true" Visible='<%#IIf(Master.noteactext <> "", "true", "false")%>'>
                            <%#clsGeneral.MouseOverTextStart()%>
                            <%#Master.noteactext%>
                            <%#clsGeneral.MouseOverTextEnd()%>
                        </obout:Flyout>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</asp:Content>
