<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="AirportLocationMap.aspx.vb"
  Inherits="crmWebClient.AirportLocationMap" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script language="javascript" type="text/javascript">
  </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="">
    <ProgressTemplate>
      <div id="divLoading" runat="server" style="text-align: center; font-weight: bold;
        background-color: #eeeeee; filter: alpha(opacity=90); opacity: 0.9; width: 395px;
        height: 295px; text-align: center; padding: 75px; position: absolute; border: 1px solid #003957;
        z-index: 10; margin-left: 225px;">
        <span>Please wait ... </span>
        <br />
        <br />
        <img src="/images/loading.gif" alt="Loading..." /><br />
      </div>
    </ProgressTemplate>
  </asp:UpdateProgress>
  <div style="text-align: left;">
    <asp:UpdatePanel ID="airport_location_map" runat="server" ChildrenAsTriggers="True"
      UpdateMode="Conditional">
      <ContentTemplate>
        <asp:Table ID="buttonsTable" CellPadding="3" CellSpacing="0" Width="90%" CssClass="DetailsBrowseTable"
          runat="server">
          <asp:TableRow>
            <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="right" VerticalAlign="middle">
         
            </asp:TableCell>
            <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="right" VerticalAlign="middle"
              Width="23%">
              <div class="backgroundShade">
                <asp:LinkButton ID="close_button" runat="server" onClientClick="javascript:window.close();"
                  CssClass="float_right"><img src="/images/x.svg" alt="Close" /></asp:LinkButton>
                <a href="help.aspx" target="_blank" class="float_right" title="Show Market Transactions Help"><img src="/images/help-circle.svg" alt="Help" /></a>
              </div>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
        <br />
        <asp:Literal ID="debug_output" runat="server"></asp:Literal>
        <div id="map_canvas" style="width: 98%; height: 556px; text-align: center; margin-left: 10px;
          margin-bottom: 20px;">
        </div>

        <script type="text/javascript" language="javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE&sensor=false&libraries=geometry"></script>

        <script type="text/javascript" language="javascript">

          function initialize(latitude, longitude, title) {

            var mapOptions = {
              zoom: 14,
              center: new google.maps.LatLng(latitude, longitude),
              mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            var mapDiv = document.getElementById("map_canvas");
            var map = new google.maps.Map(mapDiv, mapOptions);

            var marker = new google.maps.Marker({
              position: new google.maps.LatLng(latitude, longitude),
              map: map,
              title: title
            });

          }
        </script>

      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</asp:Content>
