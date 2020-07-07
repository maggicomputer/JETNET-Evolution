<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="listing.aspx.vb" Inherits="crmWebClient._listing"
    MasterPageFile="~/main_site.Master" %>
<%@ Import Namespace="crmWebClient.clsGeneral" %>
<%@ MasterType VirtualPath="~/main_site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DataGrid runat="server" ID="Results" CellPadding="3" HeaderStyle-BackColor="#204763" OnItemCommand="dispDetails"
        BackColor="White" Font-Name="tahoma" Font-Size="8pt" Width="100%" AllowPaging="true"
        PageSize="24" CssClass="grid" BorderStyle="None" AllowSorting="True" Font-Names="verdana"
        AutoGenerateColumns="false" BorderColor="#BCC9D6" PagerStyle-NextPageText="Next"
        PagerStyle-PrevPageText="Previous" PagerStyle-Mode="NumericPages">
         <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#246193" Font-Bold="false"
            Font-Underline="false" ForeColor="White" />
        <AlternatingItemStyle CssClass="alt_row" />
        <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="#C6C6C6" />
        <HeaderStyle BackColor="#246193" Font-Bold="false" Font-Size="10" Font-Underline="false"
            ForeColor="White" Wrap="False" HorizontalAlign="left" VerticalAlign="Middle">
        </HeaderStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="">
                <ItemTemplate>
                    <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="10px" />
                    <input type='checkbox' />
                    <%#Master.evalme(DataBinder.Eval(Container.DataItem, "clicomp_id"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#Master.showme(clsGeneral.active(DataBinder.Eval(Container.DataItem, "jobseek_status")))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="clicomp_id" Visible="false" />
            <asp:BoundColumn DataField="clicontact_id" Visible="false" />
            <asp:TemplateColumn HeaderText="Name">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                          <a href="details.aspx?comp_ID=<%#DataBinder.Eval(Container.DataItem, "clicomp_id")%>&type=1&contact_ID=<%#DataBinder.Eval(Container.DataItem, "clicontact_id")%>source=CLIENT">
                               <%#Master.showme(DataBinder.Eval(Container.DataItem, "clicontact_first_name"))%>&nbsp;
                               <%#Master.showme(DataBinder.Eval(Container.DataItem, "clicontact_last_name"))%>
                          </a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Name" Visible="false">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                       
                               <%#Master.showme(DataBinder.Eval(Container.DataItem, "clicontact_first_name"))%>&nbsp;
                               <%#Master.showme(DataBinder.Eval(Container.DataItem, "clicontact_last_name"))%>
               
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Location">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#Master.showme(DataBinder.Eval(Container.DataItem, "clicomp_city") & ",")%>
                    <%#Master.showme(DataBinder.Eval(Container.DataItem, "clicomp_state"))%>
                    <%#Master.showme(DataBinder.Eval(Container.DataItem, "clicomp_country"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Type">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#Master.showme(IIf((Not IsDBNull(DataBinder.Eval(Container.DataItem, "jobseek_type"))), clsGeneral.pilot_mechanic(DataBinder.Eval(Container.DataItem, "jobseek_type")), ""))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Aircraft">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#DataBinder.Eval(Container.DataItem, "jobsind_model_name")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Experience">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#DataBinder.Eval(Container.DataItem, "jobsind_model_experience")%>
                    <%#DataBinder.Eval(Container.DataItem, "jobsind_model_experience_type")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Date Submitted">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#Master.showme(DataBinder.Eval(Container.DataItem, "jobseek_date_posted"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</asp:Content>
