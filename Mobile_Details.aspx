<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Mobile_Details.aspx.vb"
    Inherits="crmWebClient.Mobile_Details" MasterPageFile="~/Mobile.Master" %>

<%@ Register Src="controls/Company_Edit_Template.ascx" TagName="Company_Edit_Template"
    TagPrefix="uc6" %>
<%@ Import Namespace="crmWebClient.clsGeneral" %>
<%@ MasterType VirtualPath="~/Mobile.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <asp:Panel ID="content" runat="server" Visible="false"  Width="343">
        <asp:Table runat="server"  Width="343" CellPadding="0" CellSpacing="0">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="switched" CssClass="jetnet_block"
                    Width="140">
                    <asp:Label runat="server" ID="information" CssClass="jetnet"></asp:Label>
                    <asp:Label runat="server" ID="phone"></asp:Label>
                    <img src="images/spacer.gif" width="140" alt=""  border="0" height="1" />
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" CssClass="block" Width="150">
                    <asp:Label ID="contact_information" runat="server">
                    </asp:Label>
                       <img src="images/spacer.gif" width="155" alt="" border="0" height="1" />
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="TableCell1" Width="10">
                                &nbsp;
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3">
                    <br />
                    <asp:Panel runat="server" CssClass="menu"  Width="321" Height="17" Font-Size="Smaller">
                        <table  cellpadding="0" cellspacing="1">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:LinkButton ID="all_switch" runat="server" Visible="true" Font-Bold="true" EnableViewState="true">All</asp:LinkButton>
                                </td>
                                <td align="left" valign="top">
                                    <asp:LinkButton ID="aircraft_switch" runat="server" Visible="true" Font-Bold="true">Aircraft</asp:LinkButton>
                                </td>
                                <td align="left" valign="top">
                                    <asp:LinkButton ID="notes_switch" runat="server" Visible="true" Font-Bold="true">Notes</asp:LinkButton>
                                </td>
                                <td align="left" valign="top">
                                    <asp:LinkButton ID="actions_switch" runat="server" Visible="true" Font-Bold="true">Actions</asp:LinkButton>
                                </td>
                                <td align="left" valign="top">
                                    <asp:LinkButton ID="documents_switch" runat="server" Visible="true" Font-Bold="true">Docs</asp:LinkButton>
                                </td>
                                <td align="left" valign="top">
                                    <asp:LinkButton ID="folders_switch" runat="server" Visible="true" Font-Bold="true">Folders</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="aircraft_visibility" runat="server" CssClass="notes" Width="331">
                    <h2> AIRCRAFT</h2>
                        <asp:Label runat="server" ID="aircraft_information" CssClass="aircraft">
                                <table  cellpadding="0" cellspacing="0">
                                    <tr class="head">
                                        <td align="left" valign="top">Make</td>
                                        <td align="left" valign="top">Model</td>
                                        <td align="left" valign="top">Year</td>
                                        <td align="left" valign="top">Serial #</td>
                                        <td align="left" valign="top">Reg #</td>
                                        <td align="left" valign="top">Status</td>
                                    </tr>
                                    <tr>
                                        <td colspan="6"><p align="center" class="attention">No aircraft at this time</p></td>
                                    </tr>
                                </table>
                        </asp:Label>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true">
                    </asp:GridView>
                    <asp:Panel ID="notes_visibility" runat="server" Width="331">
                        <h2>
                            NOTES [<a href="edit_note.aspx?action=new&type=note&cat_key=0">+</a>]</h2>
                        <asp:Label runat="server" ID="notes_display" CssClass="aircraft">
                 
                        </asp:Label>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
               <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="true">
                    </asp:GridView>
                    <asp:Panel ID="opp_visibility" runat="server" Width="331">
                        <h2>
                            OPPORTUNITIES [<a href="edit_note.aspx?action=new&type=opportunity&cat_key=0">+</a>]</h2>
                        <asp:Label runat="server" ID="opp_display" CssClass="aircraft">
                 
                        </asp:Label>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:Panel ID="actions_visibility" runat="server" Width="331">
                        <h2>
                            ACTION ITEMS [<a href="edit_note.aspx?action=new&type=action&cat_key=0">+</a>]</h2>
                        <asp:Label runat="server" ID="actions_display" CssClass="aircraft">
                 
                        </asp:Label>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:Panel ID="documents_visibility" runat="server" Width="331">
                        <h2>
                            DOCUMENTS [<a href="edit_note.aspx?action=new&type=documents&cat_key=0">+</a>]</h2>
                        <asp:Label runat="server" ID="documents_display" CssClass="aircraft">
                 
                        </asp:Label>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:Panel ID="folders_visibility" runat="server" Width="331">
                        <h2>
                            FOLDERS</h2>
                        <asp:Label runat="server" ID="folders_display" CssClass="aircraft">

                        </asp:Label>
                        <asp:panel runat="server" ID="folder_button_container" HorizontalAlign="Right"><asp:LinkButton ID="save_folder" runat="server" Font-Bold="true">Save Folders</asp:LinkButton></asp:panel>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:Panel ID="apu_visibility" runat="server" Width="331">
                        <h2>
                            APU</h2>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="jetnet_apu_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="client_apu_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:Panel ID="avionics_visibility" runat="server" Width="331">
                        <h2>
                            AVIONICS</h2>
                        <table width="320" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="jetnet_avionics_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="client_avionics_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
                        <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:Panel ID="transaction_visibility" runat="server" Width="331">
                        <h2>TRANSACTIONS</h2>
                        <table width="331" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="jetnet_transactions" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:Panel ID="cockpit_visibility" runat="server" Width="331">
                        <h2>
                            COCKPIT</h2>
                        <table width="308" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="jetnet_cockpit_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="client_cockpit_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:Panel ID="usage_visibility" runat="server"  Width="331">
                        <h2>
                            USAGE</h2>
                        <table width="320" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="jetnet_usage_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="client_usage_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:Panel ID="engine_visibility" runat="server"  Width="331">
                        <h2>
                            ENGINE</h2>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="jetnet_engine_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="client_engine_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:Panel ID="events_visibility" runat="server"  Width="331">
                        <h2>EVENTS (last 5)</h2>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="jetnet_event_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:Panel ID="equipment_visibility" runat="server"  Width="331">
                        <h2>
                            EQUIPMENT</h2>
                        <table width="331" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="jetnet_equipment_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="client_equipment_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:Panel ID="features_visibility" runat="server" Width="331">
                        <h2>
                            FEATURES</h2>
                        <table width="331" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="jetnet_features_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="client_features_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:Panel ID="int_visibility" runat="server"  Width="331">
                        <h2>
                            INT/EXT</h2>
                        <table width="320" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="jetnet_interior_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                    <asp:Label runat="server" ID="jetnet_exterior_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="client_interior_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                    <asp:Label runat="server" ID="client_exterior_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3" CssClass="notes">
                    <asp:Panel ID="maintenance_visibility" runat="server"  Width="331">
                        <h2>
                            MAINTENANCE</h2>
                        <table width="320" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="jetnet_maintenance_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Label runat="server" ID="client_maintenance_display" CssClass="aircraft">
                 
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
          
        </asp:Table>
    </asp:Panel>
    <asp:Panel runat="server" ID="edit_company" Visible="false">
        <uc6:Company_Edit_Template ID="Company_Edit_Template1" runat="server" Visible="false" />
    </asp:Panel>
</asp:Content>
