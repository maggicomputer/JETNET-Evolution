<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="JobsSearch.ascx.vb"
    Inherits="crmWebClient.JobsSearch" %>

<asp:Panel ID="search_pnl" runat="server" BackColor="#D4FAE9" 
                    CssClass="search_pnl" Height="60px" Width="98%">
                    <asp:Table ID="search_pnl_table" runat="server" Height="58px" Width="100%">
                    <asp:TableRow ID="regular_search">
                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="100"><asp:Label ID="search_for_lbl" runat="server" Text="Search For"></asp:Label></asp:TableCell>
                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"><asp:TextBox ID="search_for_txt" runat="server" Width="180"></asp:TextBox></asp:TableCell>
                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"><asp:DropDownList ID="search_where" runat="server" Width="120"></asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="80"><asp:Label ID="search_in" runat="server" Text="Search In"></asp:Label></asp:TableCell>
                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"><asp:DropDownList ID="search_for_cbo" runat="server" Width="160"  enabled="false"></asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                               <asp:ImageButton ID="search_button" runat="server" ImageUrl="../images/search.png" /><br />
                               <asp:LinkButton visible="false" ID="adv_search" runat="server" Font-Size="XX-Small" Font-Underline="False" Font-Italic="True" >Advanced Search?</asp:LinkButton>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="activity_view" Visible="false">
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Status:</asp:TableCell>
                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"><asp:DropDownList ID="status_cbo" runat="server" Width="100"></asp:DropDownList>
                          </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
