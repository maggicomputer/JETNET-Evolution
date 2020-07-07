<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master"
  CodeBehind="ShowNoteDetails.aspx.vb" Inherits="crmWebClient.ShowNoteDetails" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <p class="DetailsBrowseTable">
    <span class="backgroundShade"><a href="#" class="gray_button float_right noBefore"
      onclick="javascript:window.close();"><strong>Close</strong></a></span><div class="clear">
      </div>
  </p>
  <div class="NotesHeader">
  </div>
  <div class="aircraftContainer">
    <div class="valueSpec viewValueExport Simplistic blue gray_background">
      <div class="sixteen columns">
        <div class="row remove_margin">
          <div class="twelve columns" style="margin-left: auto; margin-right: auto; width: 96%;
            float: none;">
            <div class="Box">
              <div class="padding">
                <asp:Label ID="aircraft_information" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="company_information" runat="server" Visible="false"></asp:Label>
                <br />
                <br clear="all" />
                <br />
                <div class="four columns remove_margin">
                  <div runat="server" id="crmToggleOn">
                    Category:
                    <asp:DropDownList runat="server" ID="noteCategory">
                    </asp:DropDownList>
                  </div>
                  &nbsp;
                </div>
                <div class="four columns">
                  Staff:
                  <asp:DropDownList runat="server" ID="noteStaff">
                  </asp:DropDownList>
                </div>
                <div class="four columns">
                  <input type="reset" id="resetButton" value="Clear Selections" class="float_right" />
                  <asp:Button runat="server" ID="searchButton" Text="Search" CssClass="float_right" /></div>
              </div>
            </div>
          </div>
          <div class="twelve columns remove_margin">
            <div class="Box noteListing">
              <div style="max-height: 680px; overflow-x: hidden;" class="resizeDiv">
                <asp:Literal runat="server" ID="notesDataLiteral"></asp:Literal>
              </div>
              <asp:DataGrid runat="server" AutoGenerateColumns="true" ID="testCol">
              </asp:DataGrid>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">
</asp:Content>
