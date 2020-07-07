<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="View.aspx.vb" Inherits="crmWebClient.View"
    MasterPageFile="~/main_site.Master" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/main_site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" cellpadding="3" cellspacing="0">
        <tr>
            <td align="left" valign="top" width="300" height="223">
                <cc1:TabContainer ID="TabContainer2" runat="server" Height="223px" Width="300" CssClass="blue"
                    Visible="true">
                    <cc1:TabPanel ID="TabPanel14" runat="server">
                        <HeaderTemplate>
                            <asp:Label ID="make_model_name_label" Text="" runat="server"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table align='center' valign='middle' height='210'>
                                <tr valign='middle' height='210''>
                                    <td align='center' height='210'>
                                        <asp:Image ID="aircraft_image" runat="server" ImageUrl="images/spacer.gif" Width="265"
                                            BorderColor="Black" BorderWidth="1" BorderStyle="Solid" BackColor="#1f6c9a" />
                                        <asp:Label ID="label_behind_pic" Text="" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel16" runat="server">
                        <HeaderTemplate>
                            ?
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="main_pnl">
                                <asp:CheckBox ID="default_models" runat="server" Text="Default Models Only" Font-Size="XX-Small"
                                    Checked="true" AutoPostBack="true" Visible="true" />
                                <asp:ListBox ID="model_cbo" runat="server" SelectionMode="single" Rows="8" Visible="true"
                                    Width="290"></asp:ListBox>
                                <asp:Label runat="server" ID="model_evo_swap" Visible="false">
                                    <asp:CheckBoxList ID="model_type" runat="server" RepeatLayout="Table" Enabled="true"
                                        AutoPostBack="true" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="Helicopter" Text="Helicopter" Selected="True" />
                                        <asp:ListItem Value="Business" Text="Business" Selected="True" />
                                        <asp:ListItem Value="Commercial" Text="Commercial" Selected="True" />
                                    </asp:CheckBoxList>
                                    <table width="100%" cellpadding="3" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                Type:<br />
                                                <asp:ListBox ID="type" runat="server" Width="105px" Rows="7" AutoPostBack="true"
                                                    Font-Size="10px" SelectionMode="Multiple">
                                                    <asp:ListItem>All</asp:ListItem>
                                                </asp:ListBox>
                                            </td>
                                            <td align="left" valign="top">
                                                Make:<br />
                                                <asp:ListBox ID="make" runat="server" Width="170px" Rows="7" AutoPostBack="true"
                                                    Font-Size="10px" SelectionMode="Multiple">
                                                    <asp:ListItem>All</asp:ListItem>
                                                </asp:ListBox>
                                            </td>
                                            <td align="left" valign="top">
                                                Model:<br />
                                                <asp:ListBox ID="model" runat="server" Width="100px" Rows="7" AutoPostBack="false"
                                                    Font-Size="10px" SelectionMode="Multiple">
                                                    <asp:ListItem>All</asp:ListItem>
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Label>
                                <asp:Button ID="new_model" Text="Select New Model" runat="server" />
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
            <td align="left" valign="top">
                <cc1:TabContainer ID="tabs_container" runat="server" Height="223px" Width="515px"
                    CssClass="blue" Visible="true">
                    <cc1:TabPanel ID="market_status_tab" runat="server">
                        <HeaderTemplate>
                            MARKET STATUS
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="market_status_tab_label" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="fleet_tab" runat="server">
                        <HeaderTemplate>
                            FLEET
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="fleet_tab_label" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="specs_tab" runat="server">
                        <HeaderTemplate>
                            SPECS
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="specs_tab_label" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="operating_costs_tab" runat="server">
                        <HeaderTemplate>
                            OPERATING COSTS
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="operating_costs_tab_label_direct" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel9" runat="server">
                        <HeaderTemplate>
                            DESCRIPTION
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="description_tab_label_direct" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel17" runat="server" Visible="false">
                        <HeaderTemplate>
                            REPORTS
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="reports_label" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" colspan="2">
                <cc1:TabContainer ID="TabContainer1" runat="server" Height="286px" Width="822px"
                    CssClass="blue" Visible="true" AutoPostBack="true">
                    <cc1:TabPanel ID="TabPanel1" runat="server">
                        <HeaderTemplate>
                            TRENDS
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="market_trends_label" Text="" runat="server"></asp:Label>
                            <asp:Chart ID="AVG_PRICE_MONTH" Visible="false" runat="server" ImageStorageMode="UseImageLocation"
                                ImageType="Jpeg">
                                <Series>
                                    <asp:Series Name="Series1">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1">
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                            <asp:Chart ID="FOR_SALE" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
                                Visible="False">
                                <Series>
                                    <asp:Series Name="Series1">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1">
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                            <asp:Chart ID="PER_MONTH" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
                                Visible="False">
                                <Series>
                                    <asp:Series Name="Series1">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1">
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                            <asp:Chart ID="AVG_DAYS_ON" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
                                Visible="False">
                                <Series>
                                    <asp:Series>
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1">
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="for_sale_tab" runat="server">
                        <HeaderTemplate>
                            FOR SALE
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="for_sale_label" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel3" runat="server">
                        <HeaderTemplate>
                            RECENT SALES
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="retail_sales_label" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel4" runat="server">
                        <HeaderTemplate>
                            ACTIVITY
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="market_activity_label" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel5" runat="server">
                        <HeaderTemplate>
                            NEWS
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="news_label" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel7" runat="server">
                        <HeaderTemplate>
                            WANTEDS
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="wanteds_label" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel8" runat="server">
                        <HeaderTemplate>
                            DOCUMENTS
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="documents_label" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel10" runat="server">
                        <HeaderTemplate>
                            OPERATORS
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="operators_label" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel11" runat="server">
                        <HeaderTemplate>
                            CHARTER
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="charter_label" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel15" runat="server">
                        <HeaderTemplate>
                            LEASE
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="lease_label" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel12" runat="server">
                        <HeaderTemplate>
                            FLIGHTS
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="flights_label" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="spi_tab" runat="server">
                        <HeaderTemplate>
                            SPI
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="spi_label" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="range_tab" runat="server" Visible="false">
                        <HeaderTemplate>
                            RANGE
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width='100%' cellpadding='3' valign='top'>
                                <tr valign='top'>
                                    <td width='40%' valign='top'>
                                        Range from Airport (IATA/ICAO):
                                        <input type="text" value='SYR' id="Location" name='Location' size='4' runat="server" />
                                        <asp:ListBox ID="aport_list_drop_down" name="aport_list_drop_down" Height="75" Width="300"
                                            runat="server"></asp:ListBox>
                                        <br />
                                        <input type="button" id='submit_location' name='submit_location' value='Display Map'
                                            runat="server" />
                                        <br />
                                        <asp:Label ID="range_label" Text="" runat="server"></asp:Label>
                                    </td>
                                    <td width='60%'>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
    </table>
    <asp:Chart ID="OP_COUNTRY_CHART" runat="server" ImageStorageMode="UseImageLocation"
        ImageType="Jpeg" Visible="False">
        <Series>
            <asp:Series>
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <asp:Chart ID="SPI_QUARTER" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
        Visible="False">
        <Series>
            <asp:Series>
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <asp:Chart ID="AVG_SOLD_PER_MONTH" runat="server" ImageStorageMode="UseImageLocation"
        ImageType="Jpeg" Visible="False">
        <Series>
            <asp:Series>
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
</asp:Content>
