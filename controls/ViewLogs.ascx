<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ViewLogs.ascx.vb" Inherits="crmWebClient.ViewLogs" %>
<asp:DataGrid runat="server" ID="event_log" CellPadding="3" Width="100%" AutoGenerateColumns="false"
    CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingItemStyle-CssClass="alt"  GridLines="None"
    ItemStyle-CssClass="item_row" ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="th">
    <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" ForeColor="White" />
    <AlternatingItemStyle />
    <ItemStyle BorderStyle="None" VerticalAlign="Top" />
    <Columns>
        <asp:BoundColumn DataField="clievent_id" HeaderText="ID" />
        <asp:TemplateColumn HeaderText="Description">
            <ItemTemplate>
                <%#(DataBinder.Eval(Container.DataItem, "clievent_desc"))%>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Time">
            <ItemTemplate>
                <%#(DataBinder.Eval(Container.DataItem, "clievent_time"))%>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="User">
            <ItemTemplate>
                <%#(DataBinder.Eval(Container.DataItem, "clievent_login"))%>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>