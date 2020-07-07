<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="listing.aspx.vb" Inherits="crmWebClient._listing"
    MasterPageFile="~/main_site.Master" %>

<%@ Register TagPrefix="obout" Namespace="OboutInc.Flyout2" Assembly="obout_Flyout2_NET" %>
<%@ MasterType VirtualPath="~/main_site.Master" %>
<%@ Import Namespace="crmWebClient.clsGeneral" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DataGrid runat="server" ID="Results" CellPadding="3" HeaderStyle-BackColor="#204763"
        OnItemCommand="dispDetails" BackColor="White" font-name="tahoma" Font-Size="8pt"
        Width="100%" AllowPaging="true" PageSize="25" CssClass="grid" BorderStyle="None"
        AllowSorting="True" Font-Names="verdana" AutoGenerateColumns="false" BorderColor="#BCC9D6"
        PagerStyle-Mode="NumericPages">
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
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "source"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Listed Date">
                <ItemTemplate>
                    <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="10px" />
                    <%#clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "amwant_listed_date"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Make/Model">
                <ItemTemplate>
                    <itemstyle horizontalalign="center" verticalalign="top" />
                    <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%>
                    <%#DataBinder.Eval(Container.DataItem, "amod_model_name")%>
                    <img src="images/spacer.gif" alt="" width="150" height="1" />
                </ItemTemplate>
            </asp:TemplateColumn>
               <asp:TemplateColumn HeaderText="Interested Party">
                <ItemTemplate>
                    <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="10px" />
                            <a href="details.aspx?comp_ID=<%#DataBinder.Eval(Container.DataItem, "comp_id")%>&source=<%#DataBinder.Eval(Container.DataItem, "source")%>&type=1&wanted=true"><%#DataBinder.Eval(Container.DataItem, "comp_name")%></a>
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "contact_id")), "<br /><em><a href='details.aspx?comp_ID=" & DataBinder.Eval(Container.DataItem, "comp_id") & "&contact_ID=" & DataBinder.Eval(Container.DataItem, "contact_id") & "&source=" & DataBinder.Eval(Container.DataItem, "source") & "&type=1&wanted=true'>" & DataBinder.Eval(Container.DataItem, "contact_first_name") & " " & DataBinder.Eval(Container.DataItem, "contact_last_name") & "</a></em>", "")%>
                </ItemTemplate>
            </asp:TemplateColumn>
             <asp:TemplateColumn HeaderText="Notes">
                <ItemTemplate>
                    <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="10px" />
                    <%#IIf(DataBinder.Eval(Container.DataItem, "source") = "JETNET", DataBinder.Eval(Container.DataItem, "amwant_notes"), "<a href='#' onclick=""javascript:window.open('edit_note.aspx?action=edit&type=wanted&id=" & Eval("lnote_id") & "','','scrollbars=no,menubar=no,height=600,width=880,resizable=yes,toolbar=no,location=no,status=no');"">" & DataBinder.Eval(Container.DataItem, "amwant_notes") & "</a>")%>
                </ItemTemplate>
            </asp:TemplateColumn>
             <asp:TemplateColumn HeaderText="Year Range">
                <ItemTemplate>
                    <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="10px" />
                    <%#DataBinder.Eval(Container.DataItem, "amwant_start_year")%>
                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "amwant_end_year")), "- " & DataBinder.Eval(Container.DataItem, "amwant_end_year"), "")%>
                </ItemTemplate>
            </asp:TemplateColumn>
             <asp:TemplateColumn HeaderText="Max Price">
                <ItemTemplate>
                    <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="10px" />
                    <%#clsGeneral.no_zero(DataBinder.Eval(Container.DataItem, "amwant_max_price"), "", True)%>
                </ItemTemplate>
            </asp:TemplateColumn>
              <asp:TemplateColumn HeaderText="Max AFTT">
                <ItemTemplate>
                    <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="10px" />
                    <%#DataBinder.Eval(Container.DataItem, "amwant_max_aftt")%>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</asp:Content>
