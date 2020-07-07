<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Mobile_Listing.aspx.vb"
    Inherits="crmWebClient.Mobile_Listing" MasterPageFile="~/Mobile.Master" %>

<%@ Register Src="controls/TreeNav.ascx" TagName="TreeNav" TagPrefix="uc7" %>
<%@ Import Namespace="crmWebClient.clsGeneral" %>
<%@ MasterType VirtualPath="~/Mobile.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <asp:Label ID="general_search_error" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
    <asp:Panel runat="server" ID="company_folders" Visible="false">
        <uc7:TreeNav ID="TreeNav" runat="server" />
    </asp:Panel>
    <asp:Panel runat="server" ID="company_search" Visible="false">
        <div class="search_bar">
            <asp:Table ID="search_pnl_table" runat="server">
                <asp:TableRow ID="regular_search">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        Search:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:TextBox ID="company_search_for" runat="server" Width="120"></asp:TextBox></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="company_search_where" runat="server" Width="110">
                            <asp:ListItem Value="2">Begins With</asp:ListItem>
                            <asp:ListItem Value="1">Anywhere</asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow11">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        Phone #:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:TextBox ID="company_phone_number" runat="server" Width="120"></asp:TextBox></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                       
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="activity_view" Visible="false">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Status:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="company_status_cbo" runat="server" Width="120">
                            <asp:ListItem Value="B">All</asp:ListItem>
                            <asp:ListItem Value="Y">Active</asp:ListItem>
                            <asp:ListItem Value="N">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" RowSpan="3">
                        <asp:DropDownList ID="company_subset" runat="server" Width="120" AutoPostBack="true">
                            <asp:ListItem Text="JETNET" Value="J"></asp:ListItem>
                            <asp:ListItem Text="CLIENT" Value="C"></asp:ListItem>
                            <asp:ListItem Text="BOTH" Value="JC" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <asp:CheckBox ID="show_all" runat="server" Text="Show all?" Visible="false" />
                        <asp:ListBox ID="state" runat="server" SelectionMode="Multiple" Rows="5" Width="110"
                            Visible="false">
                            <asp:ListItem Text="SELECT ONE" Value=""></asp:ListItem>
                        </asp:ListBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="location_search" Visible="false">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Country:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="country" runat="server" Width="120" AutoPostBack="True">
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="types_search" Visible="false">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Types:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="types_of_owners" runat="server" Width="120">
                            <asp:ListItem Text="All Companies" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="All Owners" Value="all"></asp:ListItem>
                            <asp:ListItem Text="Whole Owners" Value="whole"></asp:ListItem>
                            <asp:ListItem Text="Operators" Value="operators"></asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="fields" Visible="false">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Category Search:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="special_field_cbo" runat="server" Width="120" AutoPostBack="true">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:TextBox ID="special_field_txt" runat="server" Width="110" Visible="false"></asp:TextBox>&nbsp;<asp:ImageButton
                            ID="Button1" Height="15" runat="server" ImageUrl="~/images/info.png" Visible="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="bottom" ColumnSpan="3">
                        <asp:ImageButton ID="company_search_button" runat="server" ImageUrl="../images/search.png"
                            CssClass="float_right" /><br />
                        <asp:LinkButton ID="company_adv_search" runat="server" Font-Size="Smaller" Font-Underline="False"
                            Font-Italic="True">Advanced Search?</asp:LinkButton>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
    </asp:Panel>
    <asp:Panel runat="server" ID="transaction_search" Visible="false">
        <div class="search_bar">
            <asp:Table ID="Table5" runat="server">
                <asp:TableRow ID="TableRow10">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="120">
                        <asp:Label ID="search_for_lbl" runat="server" Text="Search For"></asp:Label></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="transaction_search_where" runat="server" Width="90" CssClass="float_right">
                            <asp:ListItem Value="1">Begins With</asp:ListItem>
                            <asp:ListItem Value="2">Anywhere</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="transaction_search_for_txt" runat="server" Width="100"></asp:TextBox>&nbsp;&nbsp;
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="trans_date_search">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="80">
                        <asp:Label ID="trans_type" runat="server" Text="Type"></asp:Label></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="transaction_trans_type_cbo" runat="server" Width="200">
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="Top">
                        <asp:Label ID="end_date" runat="server" Text="Start/End"></asp:Label></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:TextBox ID="transaction_start_date_txt" runat="server" Width="90"></asp:TextBox>&nbsp;&nbsp;
                        <asp:TextBox ID="transaction_end_date_txt" runat="server" Width="90"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="trans_search">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="80">
                        <asp:Label ID="model_lbl" runat="server" Text="Model"></asp:Label><br />
                        <asp:CheckBox ID="trans_default" runat="server" Text="Default Models Only" Font-Size="XX-Small"
                            Checked="true" AutoPostBack="true" /></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:ListBox ID="transaction_model" runat="server" Width="200" SelectionMode="Multiple"
                            Rows="5"></asp:ListBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="80">
                        <asp:Label ID="dataset" runat="server" Text="Data Set"></asp:Label></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="transaction_subset" runat="server" Width="100">
                            <asp:ListItem Text="JETNET" Value="J"></asp:ListItem>
                            <asp:ListItem Text="CLIENT" Value="C"></asp:ListItem>
                            <asp:ListItem Text="BOTH" Value="JC" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:ImageButton ID="search_transactions" runat="server" ImageUrl="../images/search.png"
                            CssClass="float_right" /><br />
                        <br />
                        Internal Transactions?
                        <asp:DropDownList ID="internal_trans" runat="server" Width="70">
                            <asp:ListItem Text="BOTH" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="YES" Value="Y"></asp:ListItem>
                            <asp:ListItem Text="NO" Value="N"></asp:ListItem>
                        </asp:DropDownList>
                        <img src="images/spacer.gif" alt="" width="91" height="1" /><br />
                        Awaiting Documentation?
                        <asp:CheckBox ID="awaiting" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                Year Range from:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="transaction_year_start" runat="server" Width="50">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp; To:
                        <asp:DropDownList ID="transaction_year_end" runat="server" Width="50">
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="80">
                        <asp:Label ID="Label2" runat="server" Text="Relationships"></asp:Label></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:ListBox ID="relationships" runat="server" Width="200" SelectionMode="Multiple"
                            Rows="4"></asp:ListBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="market_search" Visible="false">
        <div class="search_bar">
            <asp:Table ID="Table4" runat="server">
                <asp:TableRow ID="TableRow7">
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="Top">
                        <asp:Label ID="Label5" runat="server" Text="Activity for Last"></asp:Label></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="market_time" runat="server">
                            <asp:ListItem Value="7">7 Days</asp:ListItem>
                            <asp:ListItem Value="31">One Month</asp:ListItem>
                            <asp:ListItem Value="93">Three Months</asp:ListItem>
                            <asp:ListItem Value="186">Six Months</asp:ListItem>
                            <asp:ListItem Value="279">Nine Months</asp:ListItem>
                            <asp:ListItem Value="365">Twelve Months</asp:ListItem>
                        </asp:DropDownList>
                        <asp:ImageButton ID="search_market" runat="server" ImageUrl="../images/search.png"
                            CssClass="float_right" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow9">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:Label ID="Label6" runat="server" Text="Model"></asp:Label><br />
                        <asp:CheckBox ID="CheckBox1" runat="server" Text="Default Models Only" Font-Size="XX-Small"
                            Checked="true" AutoPostBack="true" /></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:ListBox ID="market_model" runat="server" Width="200" SelectionMode="Multiple">
                        </asp:ListBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Category:
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:ListBox ID="categories" runat="server" Width="200" SelectionMode="Multiple"
                            Rows="3" AutoPostBack="true"></asp:ListBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Height="15">
                Type: 
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                        <asp:ListBox ID="market_types" runat="server" Width="200" SelectionMode="Multiple"
                            Rows="4"></asp:ListBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="contact_search" Visible="false">
        <div class="search_bar">
            <asp:Table ID="Table1" runat="server">
                <asp:TableRow ID="TableRow1">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        First Name:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:TextBox ID="contact_first_name" runat="server" Width="100"></asp:TextBox></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="contact_search_where" runat="server" Width="115">
                            <asp:ListItem Value="1">Begins With</asp:ListItem>
                            <asp:ListItem Value="2">Anywhere</asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        Last Name:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:TextBox ID="contact_last_name" runat="server" Width="100"></asp:TextBox></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="contact_subset" runat="server" Width="115">
                            <asp:ListItem Text="JETNET" Value="J"></asp:ListItem>
                            <asp:ListItem Text="CLIENT" Value="C"></asp:ListItem>
                            <asp:ListItem Text="BOTH" Value="B" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow6" Visible="true">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="comp_name" Visible="true">Company</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Visible="true" ID="comp_name_second">
                        <asp:TextBox ID="comp_name_txt" runat="server" Width="100"></asp:TextBox></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="contact_status_cbo" runat="server" Width="115">
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow8" Visible="true">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="order_by_text">Order By</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="contact_ordered_by" runat="server" Width="105">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3">
                        <asp:ImageButton ID="contact_search_button" runat="server" ImageUrl="../images/search.png"
                            CssClass="float_right" /><br />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
    </asp:Panel>
    <asp:Panel runat="server" ID="aircraft_search" Visible="false">
        <div class="search_bar">
            <asp:Table ID="Table2" runat="server">
                <asp:TableRow ID="TableRow2">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        Search: </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:TextBox ID="aircraft_search_for" runat="server" Width="100"></asp:TextBox></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="aircraft_search_where" runat="server" Width="100">
                            <asp:ListItem Value="1">Begins With</asp:ListItem>
                            <asp:ListItem Value="2">Anywhere</asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow4">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                        <asp:ListBox ID="model_cbo" runat="server" Width="190" SelectionMode="Multiple">
                        </asp:ListBox>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:CheckBox ID="default_models" runat="server" Text="Default Models Only" Font-Size="XX-Small"
                            Checked="true" AutoPostBack="true" CssClass="float_right" /><br />
                        <br />
                        <br />
                        <asp:ImageButton ID="aircraft_search_button" runat="server" ImageUrl="../images/search.png" /><br />
                        <asp:LinkButton ID="aircraft_adv_search" runat="server" Font-Size="smaller" Font-Underline="False"
                            Font-Italic="True">Advanced Search?</asp:LinkButton>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow5">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="80">
                        <asp:Label ID="market_status_lbl" runat="server" Text="Market Status"></asp:Label></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                        <asp:DropDownList ID="market_status_cbo" runat="server" Width="100">
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">&nbsp;&nbsp;View
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                        <asp:DropDownList ID="ac_types_of_owners" runat="server" Width="100">
                            <asp:ListItem Text="All Companies" Value=""></asp:ListItem>
                            <asp:ListItem Text="All Owners" Value="all" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Whole Owners" Value="whole"></asp:ListItem>
                            <asp:ListItem Text="Operators" Value="operators"></asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="80">
                        <asp:Label ID="Label1" runat="server" Text="Sort By"></asp:Label></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                        <asp:DropDownList ID="ac_sort" runat="server" Width="100">
                        </asp:DropDownList>
                        <asp:DropDownList ID="sort_method_cbo" runat="server" Width="60">
                            <asp:ListItem Selected="True">Asc</asp:ListItem>
                            <asp:ListItem>Desc</asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">&nbsp;&nbsp;Data Subset:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                        <asp:DropDownList ID="ac_subset" runat="server" Width="100">
                            <asp:ListItem Text="JETNET" Value="J"></asp:ListItem>
                            <asp:ListItem Text="CLIENT" Value="C"></asp:ListItem>
                            <asp:ListItem Text="BOTH" Value="JC" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">&nbsp;&nbsp;On Lease</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                        <asp:DropDownList ID="on_exclusive" runat="server" Width="100">
                            <asp:ListItem Text="Yes" Value="Y">
                            </asp:ListItem>
                            <asp:ListItem Text="No" Value="N">
                            </asp:ListItem>
                            <asp:ListItem Text="N/A" Value="" Selected="True">
                            </asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                &nbsp;&nbsp;On Exclusive?:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                        <asp:DropDownList ID="on_lease" runat="server" Width="100">
                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                            <asp:ListItem Text="N/A" Value="" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                &nbsp;&nbsp;Year Range<br />&nbsp;&nbsp;from:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                        <asp:DropDownList ID="year_start" runat="server" Width="100">
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>&nbsp;&nbsp;To:  </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                        <asp:DropDownList ID="year_end" runat="server" Width="100">
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="3">
                        &nbsp;&nbsp;Show AFTT/Engine Times?:
                        <asp:CheckBox ID="aftt" runat="server" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="base" Visible="false">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3"><br />&nbsp;&nbsp;<strong><u>Aircraft Base Location:</u></strong></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="base1" Visible="false">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">&nbsp;&nbsp;Airport Name:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                        <asp:TextBox ID="airport_name" runat="server" Width="100"></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="base5" Visible="false">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">&nbsp;&nbsp;IATA/ICAO:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                        <asp:TextBox ID="iata_code" runat="server" Width="50"></asp:TextBox>/<asp:TextBox
                            ID="icao_code" runat="server" Width="50"></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="base2" Visible="false">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">&nbsp;&nbsp;City:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                        <asp:TextBox ID="city" runat="server" Width="150"></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="base3" Visible="false">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">&nbsp;&nbsp;Country:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                        <asp:DropDownList ID="ac_country" runat="server" Width="160" AutoPostBack="True">
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="base4" Visible="false">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="state_text" Visible="false">&nbsp;&nbsp;State:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:ListBox ID="ac_state" runat="server" SelectionMode="Multiple" Rows="5" Width="160px"
                            Visible="false">
                            <asp:ListItem Text="SELECT ONE" Value=""></asp:ListItem>
                        </asp:ListBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="notes_search" Visible="false">
        <div class="search_bar">
            <asp:Table ID="Table3" runat="server">
                <asp:TableRow ID="TableRow3">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="80">Search For</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:TextBox ID="notes_search_txt" runat="server" Width="110"></asp:TextBox></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="notes_search_where" runat="server" Width="110">
                            <asp:ListItem Value="1">Begins With</asp:ListItem>
                            <asp:ListItem Value="2">Anywhere</asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="action_sort">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                    Start/End</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:TextBox ID="notes_start_date" runat="server" Visible="true" Width="110"></asp:TextBox></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:TextBox ID="notes_end_date" runat="server" Visible="true" Width="105"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="wanted_hide3">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        Sort/Order By
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:DropDownList ID="display_cbo" runat="server" Width="118">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="Top">
                        <asp:DropDownList ID="order_bo" runat="server" Width="110">
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow Visible="true" ID="models_row">
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                        <asp:Label ID="Label3" runat="server" Text="Model"></asp:Label><br />
                        <asp:CheckBox ID="CheckBox2" runat="server" Text="Default Models Only" Font-Size="XX-Small"
                            Checked="true" AutoPostBack="true" /></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                        <asp:ListBox ID="notes_model" runat="server" Width="230" SelectionMode="Multiple">
                        </asp:ListBox>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="Top">
                       

                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="wanted_hide">
        Category: 
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="wanted_hide2">
                        <asp:DropDownList ID="notes_cat" runat="server" Width="118">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="right" VerticalAlign="Top">
                        <asp:ImageButton ID="notes_search_button" runat="server" ImageUrl="../images/search.png"
                            CssClass="float_right" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </asp:Panel>
    <asp:Label ID="search_results_error" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
    <asp:Panel runat="server" ID="company_results">
        <asp:DataGrid runat="server" ID="company_list" CellPadding="3" HeaderStyle-BackColor="#204763"
            BackColor="White" Font-Name="tahoma" Font-Size="8pt" Width="100%" AllowPaging="false"
            BorderStyle="None" CssClass="grid" AllowSorting="True" Font-Names="verdana" AutoGenerateColumns="False"
            BorderColor="#BCC9D6" PagerStyle-NextPageText=" Next " EnableViewState="true"
            PagerStyle-PrevPageText=" Previous " PagerStyle-Mode="NextPrev">
            <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" Position="Bottom" BackColor="#0c437c"
                Font-Bold="True" Font-Underline="false" ForeColor="White" />
            <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="#C6C6C6" />
            <AlternatingItemStyle CssClass="alt_row" />
            <FooterStyle BackColor="#0c437c" Font-Bold="true" Font-Underline="false" />
            <HeaderStyle BackColor="#0c437c" Font-Bold="True" Font-Size="8" Font-Underline="false"
                ForeColor="White" Wrap="False" HorizontalAlign="left" VerticalAlign="Middle">
            </HeaderStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="">
                    <ItemTemplate>
                        <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="10px" />
                        <div align="center">
                            <%#clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "source"))%></div>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Company Name">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <a href="Mobile_Details.aspx?type=1&comp_id=<%#DataBinder.Eval(Container.DataItem, "comp_id")%>&source=<%#DataBinder.Eval(Container.DataItem, "source")%>">
                            <%#DataBinder.Eval(Container.DataItem, "comp_name")%></a><br />
                        <%#clsGeneral.Company_Listing_Address_Display(DataBinder.Eval(Container.DataItem, "comp_address1"), DataBinder.Eval(Container.DataItem, "comp_city"), DataBinder.Eval(Container.DataItem, "comp_state"), DataBinder.Eval(Container.DataItem, "comp_country"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </asp:Panel>
    <asp:Panel ID="contact_results" runat="server">
        <asp:DataGrid runat="server" ID="contact_list" CellPadding="3" HeaderStyle-BackColor="#204763"
            BackColor="White" font-name="tahoma" Font-Size="8pt" Width="100%" AllowPaging="false"
            CssClass="grid" BorderStyle="None" AllowSorting="false" Font-Names="verdana"
            AutoGenerateColumns="false" BorderColor="#BCC9D6" PagerStyle-Mode="NumericPages">
            <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
                Font-Underline="True" ForeColor="White" />
            <AlternatingItemStyle CssClass="alt_row" />
            <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="#C6C6C6" />
            <HeaderStyle BackColor="#67A0D9" Font-Bold="True" Font-Size="10" Font-Underline="True"
                ForeColor="White" Wrap="False" HorizontalAlign="left" VerticalAlign="Middle">
            </HeaderStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="">
                    <ItemTemplate>
                        <%#clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "contact_type"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Name">
                    <ItemTemplate>
                        <a href="Mobile_Details.aspx?contact_ID=<%#DataBinder.Eval(Container.DataItem, "contact_id")%>&comp_ID=<%#DataBinder.Eval(Container.DataItem, "contact_comp_id")%>&type=1&source=<%#DataBinder.Eval(Container.DataItem, "contact_type")%>">
                            <%#DataBinder.Eval(Container.DataItem, "contact_sirname")%>&nbsp;
                            <%#DataBinder.Eval(Container.DataItem, "contact_first_name")%>&nbsp;
                            <%#DataBinder.Eval(Container.DataItem, "contact_last_name")%><br />
                        </a>
                        <%#DataBinder.Eval(Container.DataItem, "contact_title")%><br />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Location">
                    <ItemTemplate>
                        <b><a href="Mobile_Details.aspx?contact_ID=comp_ID=<%#DataBinder.Eval(Container.DataItem, "contact_comp_id")%>&type=1&source=<%#DataBinder.Eval(Container.DataItem, "contact_type")%>">
                            <%#DataBinder.Eval(Container.DataItem, "comp_name")%></a></b><br />
                        <%#DataBinder.Eval(Container.DataItem, "comp_address1")%><br />
                        <%#DataBinder.Eval(Container.DataItem, "comp_city")%>,
                        <%#DataBinder.Eval(Container.DataItem, "comp_state")%><br />
                        <%#DataBinder.Eval(Container.DataItem, "comp_country")%><br />
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </asp:Panel>
    <asp:Panel ID="ac_results" runat="server">
        <asp:DataGrid runat="server" ID="ac_list" CellPadding="3" HeaderStyle-BackColor="#204763"
            BackColor="White" Font-Name="tahoma" Font-Size="8pt" Width="100%" AllowPaging="false"
            CssClass="grid" EnableViewState="true" AllowSorting="false" Font-Names="verdana"
            OnItemDataBound="Aircraft_Item_Databound" AutoGenerateColumns="false" BorderColor="#BCC9D6"
            PagerStyle-Mode="NumericPages" BorderStyle="Solid" BorderWidth="1px">
            <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
                Font-Underline="True" ForeColor="White" />
            <AlternatingItemStyle CssClass="alt_row" />
            <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="#eeeeee" />
            <HeaderStyle BackColor="#67A0D9" Font-Bold="True" Font-Size="10" Font-Underline="True"
                ForeColor="White" Wrap="False" HorizontalAlign="left" VerticalAlign="top"></HeaderStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="">
                    <ItemTemplate>
                        <itemstyle horizontalalign="center" verticalalign="top" />
                        <%#clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "other_source"))%><br />
                        <%#clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "source"))%>
                        <%#clsGeneral.colormelease_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_lease_flag"), DataBinder.Eval(Container.DataItem, "ac_lease_flag"), False)%>
                        <asp:Label runat="server" ID="popup_ex">
                        <%#clsGeneral.colormeex_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_exclusive_flag"), DataBinder.Eval(Container.DataItem, "ac_exclusive_flag"), False)%></asp:Label>
                        </div>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="source" Visible="false" />
                <asp:BoundColumn DataField="other_source" Visible="false" />
                <asp:BoundColumn DataField="ac_id" Visible="false" />
                <asp:BoundColumn DataField="other_ac_id" Visible="false" />
                <asp:TemplateColumn HeaderText="Model<br />Year<br />Status<br />AFTT<br />Engine TT">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <div class="aircraft_seperator_alt" align="right">
                            <%#clsGeneral.isitnull(DataBinder.Eval(Container.DataItem, "amod_make_name"))%>&nbsp;<%#clsGeneral.isitnull(DataBinder.Eval(Container.DataItem, "amod_model_name"))%>
                        </div>
                        <%#clsGeneral.difference_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_year_mfr"), DataBinder.Eval(Container.DataItem, "other_source"), DataBinder.Eval(Container.DataItem, "ac_year_mfr"), DataBinder.Eval(Container.DataItem, "source"), "aircraft_seperator", "Yr")%>
                        <%#clsGeneral.price_difference_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_status"), DataBinder.Eval(Container.DataItem, "other_source"), DataBinder.Eval(Container.DataItem, "ac_status"), DataBinder.Eval(Container.DataItem, "source"), DataBinder.Eval(Container.DataItem, "other_ac_forsale_flag"), DataBinder.Eval(Container.DataItem, "ac_forsale_flag"), "aircraft_seperator_alt", "")%>
                        <span class="jetnet_row">
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_ac_airframe_tot_hrs")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_ac_airframe_tot_hrs") & "]"), "")%>
                        </span>
                        <br />
                        <span class="client_row">
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs") & "]"), "")%></span>
                        </span> <span class="jetnet_row">
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_1_ttsn_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_1_ttsn_hours") & "]"), "")%>
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_2_ttsn_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_2_ttsn_hours") & "]"), "")%>
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_3_ttsn_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_3_ttsn_hours") & "]"), "")%>
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_4_ttsn_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_4_ttsn_hours") & "]"), "")%>
                        </span>
                        <br />
                        <span class="client_row">
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_1_ttsn_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_1_ttsn_hours") & "]"), "")%>
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_2_ttsn_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_2_ttsn_hours") & "]"), "")%>
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_3_ttsn_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_3_ttsn_hours") & "]"), "")%>
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_4_ttsn_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_4_ttsn_hours") & "]"), "")%>
                        </span>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Ser #<br />Reg #<br />Listed<br />Asking<br />Take $<br />SMOH">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <div class="aircraft_seperator_alt" align="right">
                            <h3>
                                S#</h3>
                            <%#clsGeneral.isitnull(DataBinder.Eval(Container.DataItem, "other_ac_ser_nbr"))%><br />
                            <%#clsGeneral.isitnull(DataBinder.Eval(Container.DataItem, "ac_ser_nbr"))%>
                        </div>
                        <%#clsGeneral.difference_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_reg_nbr"), DataBinder.Eval(Container.DataItem, "other_source"), DataBinder.Eval(Container.DataItem, "ac_reg_nbr"), DataBinder.Eval(Container.DataItem, "source"), "aircraft_seperator", "R#")%>
                        <%#clsGeneral.difference_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_date_listed"), DataBinder.Eval(Container.DataItem, "other_source"), DataBinder.Eval(Container.DataItem, "ac_date_listed"), DataBinder.Eval(Container.DataItem, "source"), "aircraft_seperator_alt", "Listed")%>
                        <%#clsGeneral.difference_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_asking_price"), DataBinder.Eval(Container.DataItem, "other_source"), DataBinder.Eval(Container.DataItem, "ac_asking_price"), DataBinder.Eval(Container.DataItem, "source"), "aircraft_seperator", "Asking")%>
                        <%#DataBinder.Eval(Container.DataItem, "ac_est_price")%>
                        <span class="jetnet_row">
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_1_tsoh_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_1_tsoh_hours") & "]"), "")%>
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_2_tsoh_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_2_tsoh_hours") & "]"), "")%>
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_3_tsoh_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_3_tsoh_hours") & "]"), "")%>
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_4_tsoh_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_4_tsoh_hours") & "]"), "")%>
                        </span>
                        <br />
                        <span class="client_row">
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_1_tsoh_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_1_tsoh_hours") & "]"), "")%>
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_2_tsoh_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_2_tsoh_hours") & "]"), "")%>
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_3_tsoh_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_3_tsoh_hours") & "]"), "")%>
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_4_tsoh_hours")), DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_4_tsoh_hours") & "]"), "")%>
                        </span>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Company">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <asp:Panel ID="company_hold" runat="server">
                        </asp:Panel>
                        <headerstyle width="20px" />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="" Visible="false">
                    <ItemTemplate>
                        <itemstyle horizontalalign="center" verticalalign="top" />
                        <headerstyle horizontalalign="center" />
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </asp:Panel>
    <asp:Panel ID="notes_results" runat="server">
        <asp:DataGrid runat="server" ID="notes_list" CellPadding="3" HeaderStyle-BackColor="#204763"
            BackColor="White" Font-Name="tahoma" Font-Size="8pt" CssClass="grid" BorderStyle="None"
            AllowSorting="True" Width="100%" Font-Names="verdana" AutoGenerateColumns="false"
            BorderColor="#BCC9D6" PagerStyle-Mode="NumericPages">
            <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
                Font-Underline="True" ForeColor="White" />
            <AlternatingItemStyle CssClass="alt_row" />
            <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="#C6C6C6" />
            <HeaderStyle BackColor="#67A0D9" Font-Bold="True" Font-Size="10" Font-Underline="True"
                ForeColor="White" Wrap="False" HorizontalAlign="left" VerticalAlign="Middle">
            </HeaderStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="Date">
                    <ItemTemplate>
                        <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="10px" />
                        <a href="edit_note.aspx?action=edit&type=note&id=<%#(DataBinder.Eval(Container.DataItem, "lnote_id"))%>"
                            class="smallest">
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "lnote_entry_date")), DateAdd("h", Session("timezone_offset"), FormatDateTime(DataBinder.Eval(Container.DataItem, "lnote_entry_date"))), "")%></a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Note Text">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <%#clsGeneral.Display_Listing_Note_Email_Text(DataBinder.Eval(Container.DataItem, "lnote_note"), DataBinder.Eval(Container.DataItem, "lnote_status"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Category">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <%#clsGeneral.what_cat(DataBinder.Eval(Container.DataItem, "lnote_notecat_key"), Master, Nothing)%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <asp:DataGrid runat="server" ID="action_list" CellPadding="3" HeaderStyle-BackColor="#204763"
            BackColor="White" Font-Name="tahoma" Font-Size="8pt" Width="100%" AllowPaging="false"
            Visible="true" PageSize="25" CssClass="grid" BorderStyle="None" AllowSorting="false"
            Font-Names="verdana" AutoGenerateColumns="false" BorderColor="#BCC9D6" PagerStyle-NextPageText="Next"
            EnableViewState="true" PagerStyle-PrevPageText="Previous" PagerStyle-Mode="NumericPages">
            <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
                Font-Underline="True" ForeColor="White" />
            <AlternatingItemStyle CssClass="alt_row" />
            <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="#C6C6C6" />
            <HeaderStyle BackColor="#67A0D9" Font-Bold="True" Font-Size="10" Font-Underline="True"
                ForeColor="White" Wrap="False" HorizontalAlign="left" VerticalAlign="Middle">
            </HeaderStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="Date">
                    <ItemTemplate>
                        <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="180px" />
                        <a href="edit_note.aspx?action=edit&type=action&id=<%#(DataBinder.Eval(Container.DataItem, "lnote_id"))%>"
                            class="smallest">
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "lnote_schedule_start_date")), DateAdd("h", Session("timezone_offset"), FormatDateTime(DataBinder.Eval(Container.DataItem, "lnote_schedule_start_date"))), "")%></a>
                        <br />
                        By:
                        <%#clsGeneral.what_user((DataBinder.Eval(Container.DataItem, "lnote_user_login")), Master, Nothing)%><br />
                        For:
                        <%#clsGeneral.what_user((DataBinder.Eval(Container.DataItem, "lnote_user_id")), Master, Nothing)%>
                        <br />
                        <img src="images/spacer.gif" width="160" alt="" height="1" />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Note Text">
                    <ItemTemplate>
                        <headerstyle width="10px" />
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <%#IIF((len(DataBinder.Eval(Container.DataItem, "lnote_note")) > 100), left(DataBinder.Eval(Container.DataItem, "lnote_note"),255) & "...", DataBinder.Eval(Container.DataItem, "lnote_note"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Company" Visible="false">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Priority">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <div align="center">
                            <%#clsgeneral.what_flag(DataBinder.Eval(Container.DataItem, "clipri_name"))%></div>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Category">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <%#clsGeneral.what_cat(DataBinder.Eval(Container.DataItem, "lnote_notecat_key"), Master, Nothing)%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <asp:DataGrid runat="server" ID="documents_list" CellPadding="3" HeaderStyle-BackColor="#204763"
            BackColor="White" Font-Name="tahoma" Font-Size="8pt" Width="100%" AllowPaging="false"
            PageSize="25" CssClass="grid" BorderStyle="None" AllowSorting="True" Font-Names="verdana"
            AutoGenerateColumns="false" BorderColor="#BCC9D6" PagerStyle-Mode="NumericPages">
            <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
                Font-Underline="True" ForeColor="White" />
            <AlternatingItemStyle CssClass="alt_row" />
            <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="#C6C6C6" />
            <HeaderStyle BackColor="#67A0D9" Font-Bold="True" Font-Size="10" Font-Underline="True"
                ForeColor="White" Wrap="False" HorizontalAlign="left" VerticalAlign="Middle">
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
                        <a href="edit_note.aspx?action=edit&type=documents&id=<%#(DataBinder.Eval(Container.DataItem, "lnote_id"))%>"
                            class="smallest">
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "lnote_entry_date")), DateAdd("h", Session("timezone_offset"), FormatDateTime(DataBinder.Eval(Container.DataItem, "lnote_entry_date"))), "")%></a>
                        <br />
                        By:
                        <%#clsGeneral.what_user((DataBinder.Eval(Container.DataItem, "lnote_user_login")), Master, Nothing)%><br />
                        For:
                        <%#clsGeneral.what_user((DataBinder.Eval(Container.DataItem, "lnote_user_id")), Master, Nothing)%><br />
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
                <asp:TemplateColumn HeaderText="Category">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <%#clsGeneral.what_cat(DataBinder.Eval(Container.DataItem, "lnote_notecat_key"), Master, Nothing)%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </asp:Panel>
    <asp:Panel ID="market_results" runat="server">
        <asp:DataGrid runat="server" ID="market_list" CellPadding="5" HeaderStyle-BackColor="#204763"
            BackColor="White" Font-Name="tahoma" Font-Size="8pt" Width="100%" CssClass="grid"
            BorderStyle="None" AllowSorting="True" Font-Names="verdana" AutoGenerateColumns="False"
            BorderColor="#BCC9D6" PagerStyle-NextPageText="Next" PagerStyle-PrevPageText="Previous"
            PagerStyle-Mode="NumericPages">
            <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
                Font-Underline="True" ForeColor="White" />
            <AlternatingItemStyle CssClass="alt_row" />
            <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="#C6C6C6" />
            <HeaderStyle BackColor="#67A0D9" Font-Bold="True" Font-Size="10" Font-Underline="True"
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
                        <a href="mobile_details.aspx?ac_ID=<%#DataBinder.Eval(Container.DataItem, "ac_id")%>&type=3&source=JETNET">
                            <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%>&nbsp;
                            <%#DataBinder.Eval(Container.DataItem, "amod_model_name")%><br />
                        </a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Ser #">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <a href="mobile_details.aspx?ac_ID=<%#DataBinder.Eval(Container.DataItem, "ac_id")%>&type=3&source=JETNET">
                            <%#DataBinder.Eval(Container.DataItem, "ac_ser_nbr")%></a>
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
                        <a href="mobile_details.aspx?ac_ID=<%#DataBinder.Eval(Container.DataItem, "client_id")%>&type=3&source=CLIENT">
                            <%#IIf(DataBinder.Eval(Container.DataItem, "client_id") <> 0, "<img src='images/client_aircraft.png' alt='Client Aircraft Associated with this Record' border='0'/>", "")%></a>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </asp:Panel>
    <asp:Panel ID="wanted_results" runat="server">
      <asp:DataGrid runat="server" ID="wanted_list" CellPadding="3" HeaderStyle-BackColor="#204763"
            BackColor="White" Font-Name="tahoma" Font-Size="8pt" Width="100%" AllowPaging="false"
            Visible="true" PageSize="25" CssClass="grid" BorderStyle="None" AllowSorting="false"
            Font-Names="verdana" AutoGenerateColumns="false" BorderColor="#BCC9D6" PagerStyle-NextPageText="Next"
            EnableViewState="true" PagerStyle-PrevPageText="Previous" PagerStyle-Mode="NumericPages">
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
    
    </asp:Panel>
    <asp:DataGrid runat="server" ID="opportunity_list" CellPadding="3" HeaderStyle-BackColor="#204763"
        BackColor="White" Font-Name="tahoma" Font-Size="8pt" Width="825px" AllowPaging="true"
        PageSize="25" CssClass="grid" BorderStyle="None" AllowSorting="True" Font-Names="verdana"
        AutoGenerateColumns="false" BorderColor="#BCC9D6" PagerStyle-Mode="NumericPages">
        <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
            Font-Underline="True" ForeColor="White" />
        <AlternatingItemStyle CssClass="alt_row" />
        <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="#C6C6C6" />
        <HeaderStyle BackColor="#67A0D9" Font-Bold="True" Font-Size="10" Font-Underline="True"
            ForeColor="White" Wrap="False" HorizontalAlign="left" VerticalAlign="Middle">
        </HeaderStyle>
        <Columns>
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
                    <%#clsGeneral.Format_Currency(Container.DataItem("lnote_cash_value"))%>
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
                    <a href="#" onclick="javascript:window.open('edit_note.aspx?action=edit&type= <%#IIf((DataBinder.Eval(Container.DataItem, "lnote_status") = "O"), "opportunity", "email")%>&id=<%#(DataBinder.Eval(Container.DataItem, "lnote_id"))%>','','scrollbars=no,menubar=no,height=600,width=880,resizable=yes,toolbar=no,location=no,status=no');">
                        <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "lnote_schedule_start_date")), DateAdd("h", Session("timezone_offset"), FormatDateTime(DataBinder.Eval(Container.DataItem, "lnote_schedule_start_date"))), "")%></a>
                    <br />
                    By:
                    <%#clsGeneral.what_user((DataBinder.Eval(Container.DataItem, "lnote_user_login")), Master, Nothing)%><br />
                    <%#IIf(DataBinder.Eval(Container.DataItem, "lnote_status") = "O", "Assigned To: " & clsGeneral.what_user(DataBinder.Eval(Container.DataItem, "lnote_user_id"), Master, Nothing), "")%>
                    <br />
                    <img src="images/spacer.gif" width="160" alt="" height="1" />
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
                    <%#clsGeneral.what_opportunity_cat(DataBinder.Eval(Container.DataItem, "lnote_notecat_key"), Master, Nothing)%>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <asp:Panel ID="transactions_results" runat="server">
        <asp:DataGrid runat="server" ID="trans_list" CellPadding="3" HeaderStyle-BackColor="#204763"
            BackColor="White" Font-Name="tahoma" Font-Size="8pt" Width="100%" CssClass="grid"
            BorderStyle="None" AllowSorting="True" Font-Names="verdana" AutoGenerateColumns="false"
            BorderColor="#BCC9D6" PagerStyle-Mode="NumericPages">
            <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
                Font-Underline="True" ForeColor="White" />
            <AlternatingItemStyle CssClass="alt_row" />
            <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="#C6C6C6" />
            <HeaderStyle BackColor="#67A0D9" Font-Bold="True" Font-Size="10" Font-Underline="True"
                ForeColor="White" Wrap="False" HorizontalAlign="left" VerticalAlign="Middle">
            </HeaderStyle>
            <Columns>
                <asp:BoundColumn DataField="comp_id" Visible="false" />
                <asp:BoundColumn DataField="contact_id" Visible="false" />
                <asp:BoundColumn DataField="contact_type" Visible="false" />
                <asp:BoundColumn DataField="source" Visible="false" />
                <asp:BoundColumn DataField="trans_id" Visible="false" />
                <asp:BoundColumn DataField="trans_ac_id" Visible="false" />
                <asp:TemplateColumn HeaderText="" Visible="false">
                    <ItemTemplate>
                        <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="10px" />
                        <input type="checkbox" />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <%#clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "source"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Date">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <%#clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "trans_date"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Aircraft Info">
                    <ItemTemplate>
                        <itemstyle horizontalalign="center" verticalalign="top" width="150px" />
                        <headerstyle />
                        <a href="mobile_details.aspx?ac_ID=<%#DataBinder.Eval(Container.DataItem, "trans_ac_id")%>&type=3&source=<%#DataBinder.Eval(Container.DataItem, "source")%>">
                            <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%>&nbsp;<%#DataBinder.Eval(Container.DataItem, "amod_model_name")%>
                            Ser#:<%#DataBinder.Eval(Container.DataItem, "trans_ser_nbr")%><br />
                            Reg#:<%#DataBinder.Eval(Container.DataItem, "trans_reg_nbr")%>
                        </a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Year Mfr" Visible="false">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <%#DataBinder.Eval(Container.DataItem, "trans_year_mfr")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Listed" Visible="false">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <%#clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "trans_date_listed"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Asking $" Visible="false">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <%#clsGeneral.no_zero(DataBinder.Eval(Container.DataItem, "trans_asking_price"), DataBinder.Eval(Container.DataItem, "trans_asking_wordage"), True)%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Take $" Visible="false">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <%#clsGeneral.no_zero(DataBinder.Eval(Container.DataItem, "clitrans_est_price"), "", True)%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Sold $" Visible="false">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <%#clsGeneral.no_zero(DataBinder.Eval(Container.DataItem, "clitrans_sold_price"), DataBinder.Eval(Container.DataItem, "clitrans_sold_price_type"), True)%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Relationship">
                    <ItemTemplate>
                        <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                        <headerstyle width="20px" />
                        <asp:Panel ID="company_hold" runat="server">
                        </asp:Panel>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="tcomp_name" Visible="false" />
                <asp:BoundColumn DataField="tcomp_address1" Visible="false" />
                <asp:BoundColumn DataField="tcomp_address2" Visible="false" />
                <asp:BoundColumn DataField="tcomp_city" Visible="false" />
                <asp:BoundColumn DataField="tcomp_state" Visible="false" />
                <asp:BoundColumn DataField="tcomp_country" Visible="false" />
                <asp:BoundColumn DataField="tcomp_zip_code" Visible="false" />
                <asp:BoundColumn DataField="tcomp_email_address" Visible="false" />
                <asp:BoundColumn DataField="tcomp_web_address" Visible="false" />
                <asp:BoundColumn DataField="tcontact_first_name" Visible="false" />
                <asp:BoundColumn DataField="tcontact_last_name" Visible="false" />
                <asp:BoundColumn DataField="tcontact_middle_initial" Visible="false" />
                <asp:BoundColumn DataField="tcontact_title" Visible="false" />
                <asp:BoundColumn DataField="tcontact_preferred_name" Visible="false" />
                <asp:BoundColumn DataField="tcontact_notes" Visible="false" />
                <asp:BoundColumn DataField="tcontact_email_address" Visible="false" />
                <asp:BoundColumn DataField="contact_type_id" Visible="false" />
            </Columns>
        </asp:DataGrid>
    </asp:Panel>
</asp:Content>
