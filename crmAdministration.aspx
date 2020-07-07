<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="crmAdministration.aspx.vb"
  Inherits="crmWebClient.crmAdministration" MasterPageFile="~/main_site.Master" %>

<%@ MasterType VirtualPath="~/main_site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:Table runat="server" Width="100%">
    <asp:TableRow runat="server">
      <asp:TableCell runat="server">
        <asp:Label runat="server" ID="attention" ForeColor="Red" Font-Bold="true" Visible="false"
          CssClass="window_view"><p align="center">Serial #'s have been fixed.</p></asp:Label>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TableRow1" runat="server">
      <asp:TableCell ID="TableCell1" runat="server">
        <table width="100%" cellspacing="0" cellpadding="5">
          <tr>
            <td align="left" valign="top" class="style1">
              <h4 class="specialh4">
                Aircraft Administration</h4>
            </td>
            <td align="left" valign="top" class="style1">
              <h4 class="specialh4">
                Transaction Administration</h4>
            </td>
            <td align="left" valign="top" class="style1">
              <!--<h4 class="specialh4">
                                Notes/Folders Administration</h4>-->
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:Button ID="Fix_Serial_Sorts" Text="Fix Serial Number Sort" runat="server" Width="300"
                Visible="false" />
            </td>
            <td align="left" valign="top">
              <asp:Button ID="orphaned_contact" Text="Cleanup Orphaned Contact/Phone/Reference"
                runat="server" Width="300" Visible="false" />
            </td>
            <td align="left" valign="top">
              <asp:Button ID="orphaned_notes_folders" Text="Cleanup Orphaned Notes/Folders" runat="server"
                Visible="false" Width="300" />
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:Button ID="orphaned_aircraft" Text="Cleanup Orphaned Aircraft Records" runat="server"
                Visible="false" Width="300" />
            </td>
            <td align="left" valign="top">
            </td>
            <td align="left" valign="top">
              <asp:Button ID="fix_notes_models" Text="Add MODEL ID to existing NOTES" runat="server"
                Visible="false" Width="300" />
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:Button ID="client_aircraft_bad_matches" Text="List Client Aircraft Bad Matches"
                Visible="false" runat="server" Width="300" />
            </td>
            <td align="left" valign="top">
            </td>
            <td align="left" valign="top">
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:Button ID="potential_orphaned_client_records" Text="Identify Potential Orphaned Client Aircraft Records"
                Visible="false" runat="server" Width="300" />
            </td>
            <td align="left" valign="top">
            </td>
            <td align="left" valign="top">
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:Button ID="consolidate_AC" Text="Consolidate AC Fixes" runat="server" Width="300" />
              <br />
              <asp:Button ID="load_client_maint" Text="Load Client Aircraft Maintenance" runat="server"   width="300" />
            </td>
            <td align="left" valign="top">
              <asp:Button ID="fixTransactionRecords" Text="Transaction Fix" runat="server" Width="300" />
              <asp:Button ID="fixClientAircraftTransactionRecords" Text="Client Aircraft Transaction Fix"
                runat="server" Width="300" />
              <asp:Button ID="Synch_Feature_Codes" Text="Synchronize Client Feature Codes" runat="server"
                Width="300" />
              <asp:Button ID="fixTransactionCategories" Text="Set Transaction Categories" runat="server"
                Width="300" />
            </td>
            <td align="left" valign="top">
            </td>
          </tr>
        </table>
      </asp:TableCell></asp:TableRow>
  </asp:Table>
</asp:Content>
