<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="company_spec.aspx.vb"
    Inherits="crmWebClient.company_spec" %>

<%@ Import Namespace="crmWebClient.clsGeneral" %>
<html>
<head>
    <title>Specs</title>
    <link href="common/redesign.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        *
        {
            font-size: 13px;
        }
        strong
        {
            font-size: 15px;
        }
    </style>
</head>
<body>
    <form id="Form1" runat="server">
    <asp:Table ID="company_spec" CellPadding="3" runat="server" Width="900px" BorderColor="Black"
        BackColor="White" BorderWidth="1" Font-Size="Small">
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" BackColor="#436891" BorderColor="#2f4e6f" BorderStyle="Solid"
                BorderWidth="1px">
                <asp:Label ID="company_description" runat="server" ForeColor="#fffbe8" Font-Size="Large">Company Information</asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow1" runat="server">
            <asp:TableCell ID="Company_Information" runat="server" VerticalAlign="top" ColumnSpan="2"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" BackColor="#436891" BorderColor="#2f4e6f" BorderStyle="Solid"
                BorderWidth="1px">
                <asp:Label ID="contact_description" runat="server" ForeColor="#fffbe8" Font-Size="Large">Contact Information</asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow2" runat="server">
            <asp:TableCell ID="Contact_Information" runat="server" VerticalAlign="top" ColumnSpan="2"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" BackColor="#436891" BorderColor="#2f4e6f" BorderStyle="Solid"
                BorderWidth="1px">
                <asp:Label ID="aircraft_description" runat="server" ForeColor="#fffbe8" Font-Size="Large">Aircraft Listing</asp:Label></asp:TableCell></asp:TableRow>
        <asp:TableRow ID="TableRow3" runat="server">
            <asp:TableCell ID="Aircraft_Information" runat="server" ColumnSpan="2">
        
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" BackColor="#436891" BorderColor="#2f4e6f" BorderStyle="Solid"
                BorderWidth="1px">
                <asp:Label ID="wanted_description" runat="server" ForeColor="#fffbe8" Font-Size="Large">Wanted Listing</asp:Label></asp:TableCell></asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ID="wanted_information" runat="server" ColumnSpan="2">
                <asp:DataGrid runat="server" ID="wanted_dg" CellPadding="7" horizontal-align="left"
                    BackColor="White" Font-Size="8pt" Width="100%" AllowPaging="True" PageSize="25"
                    CssClass="grid" BorderStyle="None" AllowSorting="True" AutoGenerateColumns="false"
                    BorderColor="DarkGray">
                    <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
                        Font-Underline="True" ForeColor="White" Mode="NumericPages" NextPageText="Next"
                        PrevPageText="Previous" />
                    <AlternatingItemStyle CssClass="alt_row" />
                    <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="Gray" Font-Size="8pt" />
                    <HeaderStyle BackColor="#67A0D9" Font-Bold="True" Font-Size="11pt" Font-Underline="false"
                        ForeColor="white" Wrap="False" HorizontalAlign="Left" VerticalAlign="Middle">
                    </HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn HeaderText="">
                            <ItemTemplate>
                                <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                                <headerstyle width="20px" />
                                <%#clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "source"))%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Date Listed">
                            <ItemTemplate>
                                <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                                <headerstyle width="180px" />
                                <%#clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "amwant_listed_date"))%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Make/Model">
                            <ItemTemplate>
                                <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                                <headerstyle width="180px" />
                                <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%>&nbsp;<%#DataBinder.Eval(Container.DataItem, "amod_model_name")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Interested Party">
                            <ItemTemplate>
                                <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                                <headerstyle width="10px" />
                                <a href="details.aspx?comp_ID=<%#DataBinder.Eval(Container.DataItem, "comp_id")%>&source=JETNET&type=1&wanted=true">
                                    <%#DataBinder.Eval(Container.DataItem, "comp_name")%></a>
                                <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "contact_id")), "<br /><em><a href='details.aspx?comp_ID=" & DataBinder.Eval(Container.DataItem, "comp_id") & "&contact_ID=" & DataBinder.Eval(Container.DataItem, "contact_id") & "&source=JETNET&type=1&wanted=true'>" & DataBinder.Eval(Container.DataItem, "contact_first_name") & " " & DataBinder.Eval(Container.DataItem, "contact_last_name") & "</a></em>", "")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Notes">
                            <ItemTemplate>
                                <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                                <headerstyle width="180px" />
                                <%#IIf(DataBinder.Eval(Container.DataItem, "source") = "JETNET", DataBinder.Eval(Container.DataItem, "amwant_notes"), "<a href='#' onclick=""javascript:window.open('edit_note.aspx?action=edit&type=wanted&id= " & Eval("lnote_id") & "','','scrollbars=no,menubar=no,height=600,width=880,resizable=yes,toolbar=no,location=no,status=no');"">" & DataBinder.Eval(Container.DataItem, "amwant_notes") & "</a>")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Year Range">
                            <ItemTemplate>
                                <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                                <headerstyle width="180px" />
                                <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "amwant_start_year")), DataBinder.Eval(Container.DataItem, "amwant_start_year") & " -", "")%>
                                <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "amwant_end_year")), DataBinder.Eval(Container.DataItem, "amwant_end_year"), "")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Max Price">
                            <ItemTemplate>
                                <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                                <headerstyle width="180px" />
                                <%#DataBinder.Eval(Container.DataItem, "amwant_max_price")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Max AFTT">
                            <ItemTemplate>
                                <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                                <headerstyle width="180px" />
                                <%#DataBinder.Eval(Container.DataItem, "amwant_max_aftt")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <asp:Label ID="wanted_label" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow4" runat="server">
            <asp:TableCell ID="Notes_Information" runat="server"></asp:TableCell>
            <asp:TableCell ID="TableCell1" runat="server"></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    </form>
</body>
</html>
