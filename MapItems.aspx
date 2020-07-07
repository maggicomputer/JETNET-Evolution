<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MapItems.aspx.vb" Inherits="crmWebClient.MapItems"
    MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE&sensor=false"></script>

    <script type="text/javascript">
        function FitPic() {
            window.resizeTo(1250, 750);
            self.focus();
        };

        window.onload = function () {
            FitPic();
        };
    </script>
    <style type="text/css">
        .Box {
            margin-bottom: 5px !important;
            margin-top: 2px !important
        }

        .gray_background {
            padding-left: 3px;
            padding-right: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="valueSpec Simplistic aircraftSpec">
        <div class="gray_background">

            <p class="DetailsBrowseTable">
                <span class="backgroundShade"><a href="#" onclick="javascript:window.close();" class="float_right"><img src="/images/x.svg" alt="Close" /></a></span>
            </p>
            <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                    <td align="left" valign="top" width="74%">
                        <div class="Box">
                            <div id="map_canvas" style="width: 100%; height: 550px">
                            </div>
                        </div>
                    </td>
                    <td align="left" valign="top">
                        <div class="Box"><div class="subHeader">AIRCRAFT INFORMATION</div>
                                <p class="nonflyout_info_box padding remove_margin" visible="false" runat="server" id="aircraftWarningBox">
                                    <span   runat="server" id="warningAircraftNotShow" visible="false">Note that that not all aircraft from your list may have enough information to display
    on the map. Only the aircraft listed below are displayed.<br /></span>
                                    <em class="tiny_text red_text" runat="server" id="warning" visible="false"><br />Also note that lists larger than 500 aircraft will
      only show the first 500.</em>
                                </p><br clear="all" />
                            
                            <table width="100%" cellpadding="3" cellspacing="0" class="formatTable blue">
                                <asp:Literal runat="server" ID="aircraft_list"></asp:Literal>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:Label ID="locations_generated" runat="server"></asp:Label>
        </div>
    </div>
    <script type="text/javascript">
        var map = null;;
        var myOptions;
        var locations;
        var marker, i, blue_image, red_image, shadow;
        shadow = new google.maps.MarkerImage('https://labs.google.com/ridefinder/images/mm_20_shadow.png', new google.maps.Size(22, 20), new google.maps.Point(0, 0), new google.maps.Point(6, 20));
        red_image = new google.maps.MarkerImage('https://storage.googleapis.com/support-kms-prod/SNP_2752125_en_v0', new google.maps.Size(12, 20), new google.maps.Point(0, 0), new google.maps.Point(6, 20));
        blue_image = new google.maps.MarkerImage('https://storage.googleapis.com/support-kms-prod/SNP_2752068_en_v0', new google.maps.Size(12, 20), new google.maps.Point(0, 0), new google.maps.Point(6, 20));

        //For each location in location
        function BuildPoints() {
            myOptions = {
                zoom: 2,
                center: new google.maps.LatLng(36.085142, -115.151181),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            }

            map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
            for (i = 0; i < locations.length; i++) {
                marker = new google.maps.Marker({ //create a marker for the map.
                    position: new google.maps.LatLng(locations[i][1], locations[i][2]),
                    icon: red_image,
                    shadow: shadow,
                    map: map
                });
                AddHover(marker, locations[i][0], i, map, locations[i][3]);
            }
        }
    </script>

</asp:Content>
