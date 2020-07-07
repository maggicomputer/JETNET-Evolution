<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="listing.aspx.vb" Inherits="crmWebClient._listing"
    MasterPageFile="~/main_site.Master" %>
<%@ Import Namespace="crmWebClient.clsGeneral" %>
<%@ MasterType VirtualPath="~/main_site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DataGrid runat="server" ID="Results" CellPadding="5" HeaderStyle-BackColor="#204763"
        OnItemCommand="dispDetails" BackColor="White" Font-Name="tahoma" Font-Size="8pt"
        Width="100%" AllowPaging="true" PageSize="25" CssClass="grid" BorderStyle="None"
        AllowSorting="True" Font-Names="verdana" AutoGenerateColumns="False" BorderColor="#BCC9D6"
        PagerStyle-NextPageText="Next" PagerStyle-PrevPageText="Previous" PagerStyle-Mode="NumericPages">
         <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#246193" Font-Bold="false"
            Font-Underline="false" ForeColor="White" />
        <AlternatingItemStyle CssClass="alt_row" />
        <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="#C6C6C6" />
        <HeaderStyle BackColor="#246193" Font-Bold="false" Font-Size="10" Font-Underline="false"
            ForeColor="White" Wrap="False" HorizontalAlign="left" VerticalAlign="Middle">
        </HeaderStyle>
        <Columns>
            <asp:BoundColumn DataField="ac_id" Visible="false" />
             <asp:BoundColumn DataField="client_id" Visible="false" />
            <asp:TemplateColumn HeaderText="Date">
                <ItemTemplate>
                    <itemstyle horizontalalign="center" verticalalign="top" />
                    <%#FormatDateTime(DataBinder.Eval(Container.DataItem, "apev_entry_date"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Aircraft">
                <ItemTemplate>
                    <itemstyle horizontalalign="center" verticalalign="top" />
                   <a href="details.aspx?ac_ID=<%#DataBinder.Eval(Container.DataItem, "ac_id")%>&type=3&source=JETNET">
                    <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%>&nbsp;
                    <%#DataBinder.Eval(Container.DataItem, "amod_model_name")%><br />
                    </a>
                </ItemTemplate>
            </asp:TemplateColumn>
               <asp:TemplateColumn HeaderText="Ser #">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                     <a href="details.aspx?ac_ID=<%#DataBinder.Eval(Container.DataItem, "ac_id")%>&type=3&source=JETNET"><%#DataBinder.Eval(Container.DataItem, "ac_ser_nbr")%></a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Subject">
                <ItemTemplate>
                    <itemstyle horizontalalign="center" verticalalign="top" />
                    <b><i>
                        <%#DataBinder.Eval(Container.DataItem, "apev_subject")%></i></b> -
                    <%#DataBinder.Eval(Container.DataItem, "apev_description")%>
                </ItemTemplate>
            </asp:TemplateColumn>
             <asp:TemplateColumn HeaderText="">
                <ItemTemplate>
                    <itemstyle horizontalalign="center" verticalalign="top" />
                       <%#Master.Market_Client_AC_Return(DataBinder.Eval(Container.DataItem, "ac_id"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</asp:Content>
