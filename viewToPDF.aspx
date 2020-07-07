<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="viewToPDF.aspx.vb" Inherits="crmWebClient.viewtopdf_aspx"
    MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE"></script>


    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/0.4.1/html2canvas.min.js"></script>

    <%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting"
        TagPrefix="asp" %>
    <style>
        .loadingTextStyle {
            display: none;
            position: fixed;
            z-index: 10007;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            text-align: center;
            font-size: 16px;
            font-weight: bold;
            vertical-align: middle;
            background-color: rgba(255,255,255,.8);
        }

        .valueViewPDFExport {
            height: 800px;
            overflow: hidden;
        }
        /* When the body has the loading class, we turn
   the scrollbar off with overflow:hidden */ body.loading {
            overflow: hidden;
        }
            /* Anytime the body has the loading class, our
   modal element will be visible */ body.loading .modal, body.loading .loadingTextStyle {
                display: block;
            }

                body.loading .loadingTextStyle .display_block {
                    margin: 9px;
                }

                body.loading .loadingTextStyle div {
                    position: absolute;
                    top: 20%;
                    left: 40%;
                }

        .columns.four.checkboxDiv {
            margin-left: 0px;
            padding: 0px;
            background-color: #fff;
        }

        .chosen-container.chosen-container-single {
            width: 202px !important;
            font-size: 11px;
        }

        .chosen-container-single .chosen-single {
            padding: 0 0 0 3px;
        }

        .airportSelector {
            font-size: 10px;
            padding: 4px !important;
            width: 190px !important;
        }
    </style>

    <script runat="Server">
        Sub Check_Clicked(ByVal sender As Object, ByVal e As EventArgs)
            If Session.Item("show_cost_values").ToString.ToLower = "yes" Then
                Session.Item("show_cost_values") = "no"
            Else
                Session.Item("show_cost_values") = "yes"
            End If
        End Sub
    </script>




    <script type="text/javascript" language="javascript">

        var pop_up_model_range = "";
        var pop_up_latitude = 0;
        var pop_up_longitude = 0;
        var pop_up_airport = "";
        var tab_range_map = null;

        // just sets a default map centered in us
        function initialize_range_map() {
            var mapOptions = {
                zoom: 4,
                center: new google.maps.LatLng(39.2323, -95.8887),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };


            //alert("show default range map");
            var map_RangeDiv = document.getElementById("view_range_tab_map_canvas");
            var map = new google.maps.Map(map_RangeDiv, mapOptions);
            if ((map != null) && (typeof (map) != "undefined")) {
                tab_range_map = map;
            }
        }

        //Building the tab Map
        function build_range_tab_map(airport_location, modelRange) {
            var latitude = 0;
            var longitude = 0;
            var answer = $("#<%= destination_id.ClientID %>").val();
            $('#<%= btnRunReport.ClientID %>').addClass("display_none");
            if (answer !== '') {
                var res = answer.split("|");
                latitude = res[0];
                longitude = res[1];
                $("#<%= attentionAirport.ClientID %>").addClass("display_none");
            } else {
                $("#<%= attentionAirport.ClientID %>").removeClass("display_none");
            }
            //alert(latitude + ' - ' + longitude); 
            if (Number(latitude) == 0 && Number(longitude) == 0 && Number(modelRange) == 0) { //not initalizing map, do not ignore this
                initialize_range_map();
                return false;
            }


            //      pop_up_airport = airport_location;
            //      pop_up_latitude = latitude;
            //      pop_up_longitude = longitude;
            //      pop_up_model_range = modelRange;

            //show_map.disabled = false;

            //Setting up the new options for the map.
            zoomLevel = 2;

            if (modelRange > 4023360) {
                zoomLevel = 1;
                //alert('here ' + modelRange);
            }
            var model_information1 = new Array(1);
            var model_information2 = new Array(1);
            if (document.getElementById("<%= first_model.ClientID %>").value != "") {
                //getting second model (well first dropdown) info
                model_information1 = document.getElementById("<%= first_model.ClientID %>").value.split("|");
                if (model_information1[1] > 4023360) {
                    zoomLevel = 1;
                }
            } //1st model

            if (document.getElementById("<%= second_model.ClientID %>").value != "") { //checking second dropdown.
                //getting third model (second dropdown) information
                model_information2 = document.getElementById("<%= second_model.ClientID %>").value.split("|");
                if (model_information2[1] > 4023360) {
                    zoomLevel = 1;
                }
            } //2nd model

            if (zoomLevel == 1) {
                document.getElementById("view_range_tab_map_canvas").style.height = "430px";
            } else {
                document.getElementById("view_range_tab_map_canvas").style.height = "690px";
            }
            //alert('zoom level' + zoomLevel);
            var mapOptions = {
                zoom: zoomLevel,
                center: new google.maps.LatLng(latitude, longitude),
                disableDefaultUI: true,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            //$("#view_range_tab_map_canvas").empty();

            var map_RangeDiv = document.getElementById("view_range_tab_map_canvas");
            var map = new google.maps.Map(map_RangeDiv, mapOptions);

            //finding the map.    
            if ((map != null) && (typeof (map) != "undefined")) {

                tab_range_map = map;

                // 


                //checking for other selections
                if (document.getElementById("<%= first_model.ClientID %>").value != "") {
          //getting second model (well first dropdown) info
          //var model_information1 = document.getElementById("<%= first_model.ClientID %>").value.split("|");
                    Draw_Circle("00CD00", model_information1[1], latitude, longitude);  //drawing second circle.
                } //1st model

                if (document.getElementById("<%= second_model.ClientID %>").value != "") { //checking second dropdown.
          //getting third model (second dropdown) information
          //var model_information2 = document.getElementById("<%= second_model.ClientID %>").value.split("|");
                    Draw_Circle("0276FD", model_information2[1], latitude, longitude);  //drawing third circle.
                } //2nd model

                //drawing first established circle.
                Draw_Circle("ff0000", modelRange, latitude, longitude);

                // have to add marker AFTER circles are drawn or google map script bombs out
                add_range_tab_marker(airport_location, latitude, longitude);
            }



            google.maps.event.addListenerOnce(map, 'tilesloaded', function () {
                //  google.maps.event.trigger(map, 'resize');
                setTimeToAppendCanvas();
            });

            //
        }

        function resetView() {
            document.aspnetForm.reset();
            $('#<%= second_model.ClientID %>').val('').trigger('chosen:updated');
            $('#<%= first_model.ClientID %>').val('').trigger('chosen:updated');
            $("#<%= MMS_RM.ClientID %>").attr('checked', false).triggerHandler('click');
        }


        //function to draw a circle.
        function Draw_Circle(color, radius_range, latitude, longitude) {

            radiusRange = Number(radius_range);

            //checking for valid map object.
            if ((tab_range_map != null) && (typeof (tab_range_map) != "undefined")) {

                point_map = new google.maps.LatLng(latitude, longitude); //set point to use for circle.

                var populationOptions = { //setting up circle options.
                    strokeColor: "#" + color, //color
                    strokeOpacity: 0.8,       //line opacity
                    strokeWeight: 2,
                    fillOpacity: 0.0,         //filled circle?
                    map: tab_range_map,             //map variable?
                    center: point_map, //center of circle, meaning the airport it's based around.
                    radius: radiusRange //radius of circle
                };

                new google.maps.Circle(populationOptions); //creating google maps circle.

            }
        }

        function add_range_tab_listener(marker, title, map) { //adding listener on click event. Basically adds a popup window with predetermined text on click event of marker.

            var contentString = '<div id="content"><div id="siteNotice"></div>' +
                '<h1 id="firstHeading" class="firstHeading">' + title + '</h1>' +
                '<div id="bodyContent"></div></div>';

            var infowindow = new google.maps.InfoWindow({ content: contentString });

            //Then go ahead and add the listener marker to the map.
            google.maps.event.addListener(marker, 'click', function () {
                infowindow.open(map, marker);
            });
        }

        function add_range_tab_marker(location_title, latitude, longitude) { //adding a new marker to the map ... Basically adds a popup window with predetermined text on click event of marker.
            //creating the marker for the map based on latitude, longitude

            //alert("add marker to range tab map");

            //finding the map.
            if ((tab_range_map != null) && (typeof (tab_range_map) != "undefined")) {

                var icon = {
                    url: '../images/evoPlane.png'
                };
                var marker = new google.maps.Marker({
                    position: new google.maps.LatLng(latitude, longitude),
                    map: tab_range_map,
                    icon: icon,
                    title: location_title
                });

                google.maps.event.clearListeners(marker, 'onclick');

                add_range_tab_listener(marker, location_title, tab_range_map);

            }
        }

        function setTimeToAppendCanvas() {
            $('#<%= mapText.ClientID %>').val('');
            setTimeout(function () {
                html2canvas(document.querySelector("#view_range_tab_map_canvas"), {
                    logging: false,
                    useCORS: true,
                    onrendered: function (canvas) {
                        $('#<%= mapText.ClientID %>').val(canvas.toDataURL("image/png"));
                        if ($.browser.safari) {// Fix for Chrome
                            $(".gm-style>div:first>div").css({
                                left: 0,
                                top: 0,
                                "transform": transform
                            });
                        }
                    }
                });
                $('#<%= btnRunReport.ClientID %>').removeClass("display_none");
            }, 1000);
        }


    </script>

    <script type="text/javascript" src="https://cdn.rawgit.com/Mikhus/canvas-gauges/gh-pages/download/2.1.4/all/gauge.min.js"></script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        google.charts.load('45.2', { 'packages': ['corechart', 'table'] });
    </script>

    <div id="divLoading" class="loadingScreenBox">
        <span></span>
        <div class="loader">Loading...</div>
    </div>

    <p class="DetailsBrowseTable">
        <span class="backgroundShade"><a href="#" class="float_right" onclick="javascript:window.close();">
            <img src="images/x.svg" alt="Close" /></a></span><div class="clear"></div>
    </p>

    <asp:TextBox runat="server" ID="mapText" CssClass="display_none" />
    <div style="z-index: -2000; position: absolute;">
        <div id="view_range_tab_map_canvas" style="width: 940px;">
        </div>
    </div>
    <asp:Panel runat="server" ID="contentClass" CssClass="valueViewPDFExport">
        <div id="outerDivViewToPDFID" runat="server" class="center_outer_div" width="1000">
            <table id="mainTableID" border="0" cellpadding="0" cellspacing="0" align="center"
                width='100%'>
                <tr>
                    <td align="center" style="text-align: center; padding-left: 0px;">
                        <h2>
                            <strong>
                                <asp:Label ID="form_title" runat="server" Text=""></asp:Label></strong>&nbsp;
              <asp:Label ID="view_type_label" runat="server" Text=""></asp:Label></h2>
                        <asp:Label runat="server" ID="attentionLabel" ForeColor="Red" Font-Bold="true"></asp:Label>
                        <div class="row checkboxMenu">
                            <div class="columns three checkboxDiv">
                                <strong class="display_none">MODEL/FLEET INFO</strong>
                                <asp:Panel runat="server" ID="MR_Panel1" Visible="false">
                                    <asp:CheckBox ID="MR_DESC_SPECS" Checked="true" runat="server" Text='Description & Specifications'
                                        Visible="true" CssClass="listCheck" />
                                    <asp:CheckBox ID="MR_OPERATING_COSTS" Checked="true" runat="server" Text='Operating Costs'
                                        Visible="true" CssClass="listCheck" />
                                    <asp:CheckBox ID="MMS_BPS" Checked="true" runat="server" AutoPostBack="True" Text='Performance Specs'
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="MMS_BOC" Checked="true" runat="server" AutoPostBack="True" Text='Operating Costs'
                                        OnCheckedChanged="Check_Clicked" CssClass="listCheck" />
                                </asp:Panel>
                                <asp:CheckBox ID="MMS_FMS" Checked="true" runat="server" Text='Fleet/Market Summary'
                                    CssClass="listCheck" />
                                <asp:CheckBox ID="MMS_UP" Checked="true" runat="server" Text='Upgrade To and From'
                                    CssClass="listCheck" />
                                <asp:CheckBox ID="MR_Fleet" Checked="true" runat="server" Text='Fleet Composition/Age'
                                    Visible="false" CssClass="listCheck" />
                                <asp:CheckBox ID="MR_LOCATIONS" Checked="true" runat="server" Text='Fleet Locations'
                                    Visible="false" CssClass="listCheck" />
                                <asp:CheckBox ID="MR_UTILIZATION" Checked="true" runat="server" Text='Flight Activity'
                                    Visible="false" CssClass="listCheck" />
                                <asp:CheckBox ID="C_MPAS" Checked="true" runat="server" Text='Model Pictures and Specs'
                                    CssClass="listCheck" />
                                <asp:CheckBox ID="S_SM" Checked="true" runat="server" Text='Similar Models' CssClass="listCheck" />
                                <asp:CheckBox ID="MCOMP_FMS" Checked="true" runat="server" Text='Fleet Summary/Market Status'
                                    CssClass="listCheck" />
                                <asp:CheckBox ID="MMS_STAT" Checked="true" runat="server" Text='For Sale Statistics by Year of Mfr'
                                    Visible="false" CssClass="listCheck" />
                                <asp:CheckBox ID="MMS_OP_COSTS_2" Checked="true" runat="server" Text='Operating Costs'
                                    Visible="false" CssClass="listCheck" />
                                <asp:CheckBox ID="L_TM" Checked="true" runat="server" AutoPostBack="True" Text='Top Models'
                                    CssClass="listCheck" />
                                <asp:CheckBox ID="C_MOD" Checked="true" runat="server" Text='Model List' CssClass="listCheck" />
                                <asp:CheckBox ID="O_MOD" Checked="true" runat="server" Text='Model List' CssClass="listCheck" />
                                <asp:Label ID="indent_label" runat="server" CssClass="display_none" Visible="false"
                                    Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Label><asp:CheckBox ID="MMS_CHARTS"
                                        Checked="TRUE" runat="server" Text='Include Market Trends' CssClass="listCheck" />
                                <asp:CheckBox ID="L_TL" Checked="true" runat="server" Text='Top Lessors' CssClass="listCheck" />
                                <asp:CheckBox ID="C_OP" Checked="true" runat="server" Text='Top Operators List' CssClass="listCheck" />
                                <asp:CheckBox ID="O_OS" Checked="true" runat="server" Text='Operator Summary' CssClass="listCheck" />
                                <asp:CheckBox ID="O_ENG" Checked="true" runat="server" Text='Engine Information'
                                    CssClass="listCheck" />
                                <asp:CheckBox ID="MC_FSBM" Checked="true" runat="server" Text='For Sale By Month'
                                    CssClass="listCheck" />
                                <asp:CheckBox ID="F_DSPM" Checked="true" runat="server" Text='Display Sold Per Month'
                                    CssClass="listCheck" />
                                <asp:CheckBox ID="MC_AVG_DOM" Checked="true" runat="server" Text='Average Days on Market'
                                    CssClass="listCheck" />
                                <asp:CheckBox ID="CRM_Wanteds" Checked="true" Visible="false" runat="server" Text="Wanteds"
                                    CssClass="listCheck" />
                                <asp:CheckBox ID="MMS_AA" Checked="true" runat="server" Text='Average Age Of Aircraft'
                                    CssClass="listCheck" />
                                <asp:Panel Visible="false" runat="server" ID="rangeTab">
                                    <asp:CheckBox ID="MMS_RM" Checked="false" runat="server" Text='Model Range Map' CssClass="listCheck" />
                                    <div id="rangeOptions">
                                        <table width="100%" cellpadding="2" cellspacing="0">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:DropDownList ID="first_model" runat="server" Width="200px" CssClass="chosen-select specialChosen"
                                                        data-placeholder="Pick Comparison Model (optional)">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:DropDownList ID="second_model" runat="server" Width="200px" CssClass="chosen-select specialChosen"
                                                        data-placeholder="Pick Comparison Model (optional)">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:TextBox runat="server" ID="destination" placeholder="Pick an Airport (required*)" Enabled="true" CssClass="airportSelector" />
                                                    <asp:TextBox runat="server" ID="destination_id" CssClass="display_none" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="columns three checkboxDiv">
                                <asp:Panel ID="current_market_panel" runat="server" Visible="true">
                                    <strong class="display_none">CURRENT MARKET</strong>
                                    <asp:CheckBox ID="MR_CMS" Checked="true" runat="server" Text='Market Indices/Trends'
                                        Visible="false" CssClass="listCheck" />
                                    <asp:CheckBox ID="MR_CMACFORSALE" Checked="true" runat="server" Text='Aircraft for Sale'
                                        Visible="false" CssClass="listCheck" />
                                    <asp:CheckBox ID="MR_MARKET_CHARACTERISTICS" Checked="true" runat="server" Text='Market Characteristics'
                                        Visible="false" CssClass="listCheck" />
                                    <asp:CheckBox ID="MR_DOM" Checked="true" runat="server" Text='Average Days on Market'
                                        Visible="false" CssClass="listCheck" />
                                    <asp:CheckBox ID="MR_RECENT_MARKET" Checked="true" runat="server" Text='Recent Market Activity'
                                        Visible="false" CssClass="listCheck" />
                                    <asp:CheckBox ID="MR_FSS" Checked="true" runat="server" Text='Market By Age of Fleet'
                                        Visible="false" CssClass="listCheck" />
                                    <asp:CheckBox ID="L_AL" Checked="true" runat="server" Text='Active Leases' CssClass="listCheck" />
                                    <asp:CheckBox ID="C_AC" Checked="true" runat="server" Text='Aircraft List' CssClass="listCheck" />
                                    <asp:CheckBox ID="MMS_BAFS" Checked="true" runat="server" Text='Aircraft For Sale'
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="O_AC" Checked="true" runat="server" Text='Aircraft List' CssClass="listCheck" />
                                    <asp:CheckBox ID="MC_AVG_PRICE" Checked="true" runat="server" Text='Avg Price By Month'
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="PAD_DET" Checked="true" runat="server" Text='Aircraft Details'
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="F_DM" Checked="true" runat="server" Text='Display Market' CssClass="listCheck" />
                                    <asp:CheckBox ID="MMS_BRMA" Checked="true" runat="server" Text='Events' CssClass="listCheck" />
                                    <asp:CheckBox ID="F_DFS" Checked="true" runat="server" Text='Display For Sale' CssClass="listCheck" />
                                    <asp:CheckBox ID="F_NVU" Checked="true" runat="server" Text='New Vs. Used Sales Chart'
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="S_3D" runat="server" Text="3D Charts" Checked="false" CssClass="listCheck" />
                                </asp:Panel>
                            </div>
                            <div class="columns three checkboxDiv">
                                <asp:Panel ID="sales_panel" runat="server" Visible="true">
                                    <strong class="display_none">SALES</strong>
                                    <asp:CheckBox ID="MR_SITRENS" Checked="true" runat="server" Text='Sales Insight/Trends'
                                        Visible="false" CssClass="listCheck" />
                                    <asp:CheckBox ID="MR_RRS" Checked="true" runat="server" Text='Recent Retail Sales'
                                        Visible="false" CssClass="listCheck" />
                                    <asp:Label runat="server" ID="indent_label2" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                        Visible="false"></asp:Label><asp:CheckBox ID="MMS_Sale_Prices" Checked="true" runat="server"
                                            Text='Include Sale Prices' Visible="false" CssClass="listCheck" />
                                    <asp:CheckBox ID="MR_UPGRADE" Checked="true" runat="server" Text='Owner Habits' Visible="false"
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="MR_STBM" Checked="true" runat="server" Text='Sale Summary by Month'
                                        Visible="false" CssClass="listCheck" />
                                    <asp:CheckBox ID="MR_STBMFR" Checked="true" runat="server" Text='Sales Summary By Age'
                                        Visible="false" CssClass="listCheck" />
                                    <asp:CheckBox ID="MR_STBAFTT" Checked="true" runat="server" Text='Sale Summary by AFTT'
                                        Visible="false" CssClass="listCheck" />
                                    <asp:CheckBox ID="MR_STBWC" Checked="true" runat="server" Text='Comparative Model Sales'
                                        Visible="false" CssClass="listCheck" />
                                    <asp:CheckBox ID="L_MRT" Checked="true" runat="server" Text='50 Most Recent Lease Transactions'
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="MMS_BRRS" Checked="true" runat="server" Text='Recent Sales' CssClass="listCheck" />
                                    <asp:CheckBox ID="MR_SALE_PRICES" Checked="true" Visible="false" runat="server" Text="Include Sale Prices"
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="S_FQ" Checked="true" runat="server" Text='First Quarter Data' CssClass="listCheck" />
                                    <asp:CheckBox ID="Lease_View_PDF" Checked="true" runat="server" Text='Lease View PDF'
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="C_AF" Checked="true" runat="server" Text='Aircraft By Aiframe'
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="Operator_View_PDF" Checked="true" runat="server" Text='Operator View PDF'
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="C_LOC" Checked="true" runat="server" Text='Country Locations List'
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="C_LOCITY" Checked="true" runat="server" Text='City Locations List'
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="O_COUNTRY" Checked="true" runat="server" Text='Country Summary'
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="F_DTD" Checked="true" runat="server" Text='Display Transaction Documents'
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="MC_SOLD_PM" Checked="true" runat="server" Text='Sold Per Month'
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="L_LE" Checked="true" runat="server" Text='Leasses Expired' CssClass="listCheck" />
                                    <asp:CheckBox ID="L_LDTE" Checked="true" runat="server" Text='Leasses Due To Expire'
                                        CssClass="listCheck" />
                                    <asp:CheckBox ID="F_LTG" Checked="true" runat="server" Text='Lease Trends Graph'
                                        CssClass="listCheck" />
                                </asp:Panel>
                            </div>
                            <div class="columns three checkboxDiv">
                                <strong class="display_none">Flight Activity</strong>
                                <asp:CheckBox ID="utilizationSummary" Checked="true" runat="server" Text='Flight Activity Summary'
                                    Visible="false" CssClass="listCheck" />
                                <asp:CheckBox ID="utilizationRoutes" Checked="true" runat="server" Text='Top Routes'
                                    Visible="false" CssClass="listCheck" />
                                <asp:CheckBox ID="utilizationModels" Checked="true" runat="server" Text='Top Models'
                                    Visible="false" CssClass="listCheck" />
                                <asp:CheckBox ID="utilizationOperators" Checked="true" runat="server" Text='Top Operators'
                                    Visible="false" CssClass="listCheck" />
                                <asp:CheckBox ID="utilizationAC" Checked="true" runat="server" Text='Top Aircraft'
                                    Visible="false" CssClass="listCheck" />
                                <asp:CheckBox ID="refuel_check" runat="server" Text="Refuel/Tech Stop" Visible="false" CssClass="listCheck" />
                                <asp:CheckBox ID="utilizationInternational" runat="server" Text="International Flights Only" Visible="false" CssClass="listCheck" AutoPostBack="true" />
                                <asp:CheckBox ID="utilizationGraphs" runat="server" Text='Show Flight Activity Graphs'
                                    Visible="false" CssClass="listCheck" AutoPostBack="true" onclick="if (this.checked == true) {SetLoadingText('Flight Activity Graphs Being Created, Please Wait');$('body').addClass('loading');};" />
                                <asp:CheckBox ID="utilization_state_check" CssClass="listCheck" Visible="false" runat="server" Text="Show States Instead of Continents" AutoPostBack="true" onclick="SetLoadingText('Flight Activity Graphs Being Created, Please Wait');$('body').addClass('loading');" />
                            </div>
                            <div class="columns three checkboxDiv">
                                <asp:Panel ID="valuation_panel" runat="server" Visible="false">
                                    <strong class="display_none">VALUATION</strong>
                                    <asp:CheckBox ID="current_market_check" Checked="true" runat="server" Text="Current Market Valuation" CssClass="listCheck" />
                                    <asp:CheckBox ID="mfr_year_check" Checked="true" runat="server" Text="Values By Year DLV" CssClass="listCheck" />
                                    <asp:CheckBox ID="by_month_check" Checked="true" runat="server" Text="Values By Month" CssClass="listCheck" />
                                    <asp:CheckBox ID="residual_check" Checked="true" runat="server" Text="Model Residuals" CssClass="listCheck" />
                                    <asp:CheckBox ID="aftt_check" Checked="true" runat="server" Text="Values by Model AFTT" CssClass="listCheck" />
                                </asp:Panel>
                            </div>
                            <div class="columns threehalf checkboxDiv">
                                <strong class="valueViewOnly">FORMAT &amp; CUSTOMIZATION</strong>
                                <!-- Cover Page Listing Section -->
                                <asp:CheckBox ID="CP" runat="server" Checked="true" Text="Cover Page with Model Description"
                                    CssClass="listCheck" />
                                <asp:Panel ID="prepared_panel" runat="server" Visible="false" CssClass="listCheck">
                                    <asp:CheckBox ID="check_prepared_for" runat="server" Text="Prepared For Line: " ToolTip="Include Logo" />
                                    &nbsp;&nbsp;<asp:TextBox ID="prepared_for" runat="server" ToolTip="Prepared For:"
                                        Width="195px"></asp:TextBox>
                                </asp:Panel>
                                <asp:CheckBox ID="chk_header" runat="server" Checked="True" Text="Include Company Address Block in Header"
                                    ToolTip="Include Company Address Block in Header" Visible="False" CssClass="listCheck" />
                                <br />
                                <asp:CheckBox ID="chk_alt_name" runat="server" Text="Include Alt Name In Header" Visible="False" CssClass="listCheck" />
                                <br />
                                <hr class="remove-bottom" />
                                <asp:CheckBox ID="logo_check" runat="server" Checked="True" Text="Include Logo In Header"
                                    ToolTip="Include Logo In Header" Visible="False" CssClass="listCheck" /><hr class="remove-bottom" />
                                <br />
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td valign="top" align="left">
                                            <asp:Label ID="S_COL_LABEL" runat="server" Text="Color of Headers "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="S_COL" runat="server">
                                                <asp:ListItem Value="Blue">Blue</asp:ListItem>
                                                <asp:ListItem Value="Navy">Navy</asp:ListItem>
                                                <asp:ListItem Value="Light Gray">Light Gray</asp:ListItem>
                                                <asp:ListItem Value="Gray">Gray</asp:ListItem>
                                                <asp:ListItem Value="Dark Gray">Dark Gray</asp:ListItem>
                                                <asp:ListItem Value="Black">Black</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="market_color" runat="server" AutoPostBack="true" Visible="false" onchange="javascript:SetWaitCursor();SetLoadingText('Generating Graphs with new color selections..');$('body').addClass('loading');">
                                                <asp:ListItem Value="Blue">Blue</asp:ListItem>
                                                <asp:ListItem Value="brown">Brown</asp:ListItem>
                                                <asp:ListItem Value="Gray">Gray</asp:ListItem>
                                                <asp:ListItem Value="green">Green</asp:ListItem>
                                                <asp:ListItem Value="light_blue" Selected="true">Light Blue</asp:ListItem>
                                                <asp:ListItem Value="orange">Orange</asp:ListItem>
                                                <asp:ListItem Value="red">Red</asp:ListItem>
                                                <asp:ListItem Value="yellow">Yellow</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left">
                                            <asp:Label runat="server" ID="month_timeframe_label" Text="Timeframe: " Visible="false"></asp:Label>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:DropDownList ID="month_timeframe" runat="server" Visible="false" AutoPostBack="true">
                                                <asp:ListItem Value="3">3 Months</asp:ListItem>
                                                <asp:ListItem Value="6">6 Months</asp:ListItem>
                                                <asp:ListItem Value="9">9 Months</asp:ListItem>
                                                <asp:ListItem Value="12">12 Months</asp:ListItem>
                                                <asp:ListItem Value="18">18 Months</asp:ListItem>
                                                <asp:ListItem Value="24">24 Months</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left">
                                            <asp:Label ID="No_Background" Visible="true" runat="server">White Background</asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="no_back_check" Visible="true" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left">
                                            <asp:Label ID="fuel_burn_label" Visible="true" runat="server">Show Fuel Burn in Liters</asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="fuel_burn_liters" Visible="true" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="bottom">Document Type
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:RadioButtonList ID="WD" OnSelectedIndexChanged="Check_Clicked" AutoPostBack="True"
                                                RepeatDirection="Horizontal" runat="server">
                                                <asp:ListItem Value="Word" Text="Word (.doc)" />
                                                <asp:ListItem Value="PDF" Text="PDF" Selected="True" />
                                            </asp:RadioButtonList>
                                            <asp:Label ID="HelpText2" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <hr class="remove-bottom" />
                                <br />
                                <asp:Panel runat="server" Visible="false" ID="ac_checkbox_panel">
                                    <table align="left">
                                        <tr>
                                            <td align="left">
                                                <asp:CheckBox ID="acpic_cover" runat="server" Text="My Aircraft Picture on Cover" />
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:CheckBox ID="achigh_check" runat="server" Text="Highlight My Aircraft in Report" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <!-- <div class="columns three checkboxDiv">
          <strong class="display_none">EMPTY CONTAINER CAN HOUSE ANYTHING. DISAPPEARS ON LOAD
            IF EMPTY</strong></div>
        </div>-->
                            <table align="center" class="data_aircraft_grid_cell" border='0' cellpadding='3'
                                cellspacing='0' width='100%'>
                                <tr>
                                    <td align="center" valign="top">
                                        <%If MMS_BOC.Visible Then
                                                If MMS_BOC.Checked Then%><tr>
                                                    <td style="text-align: left;">&nbsp;Currency&nbsp;&nbsp;
                            <select name='thecurrency' id='defaultCurrencyID' size=''>
                                <% 
                                    If IsNothing(Request.Item("print_details")) Then
                                        Response.Write(FillCurrencyList2())
                                    End If
                                %>
                            </select>
                                                    </td>
                                                </tr>
                                        <%
                                                End If
                                            End If
                                        %>
                                    </td>
                                </tr>
                                <!-- This is the Recent Market Activity Checkbox Section -->
                                <%If MMS_BOC.Visible Or MMS_BPS.Visible Then
                                        If MMS_BOC.Checked Or MMS_BPS.Checked Then%><!-- This is the Standard or Metric Radio Section --><tr>
                                            <td align="center" class="alt_row" valign="top">
                                                <b>&nbsp;Performance Specs &amp; Operating Costs
                          <br />
                                                    &nbsp;Units For Display</b>
                                            </td>
                                        </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="useStandardOrMetric" runat="server" name="useStandardOrMetric">
                                            <asp:ListItem Text="US Standard" Value="standard" />
                                            <asp:ListItem Text="Metric" Value="metric" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <!-- This is the Standard or Metric Radio Section -->
                                <%End If
                                    End If%>
                                <tr>
                                    <td valign="top" align="left" class="checkboxMenu"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                    </td>
                </tr>
                <tr>
                    <td id='tdInner_viewToPdf_SelectedReport_RunReport' align="right">
                        <asp:Label runat="server" ID="mpm_attention" ForeColor="Red" Visible="false" Text="Marketplace Manager Users: Note that both Aircraft for Sale and Recent Sales Pages will include any client edited data."></asp:Label>
                        <asp:Label runat="server" ID="attentionAirport" ForeColor="Red" CssClass="display_none"
                            Font-Bold="true">You must select an origin airport for generating a range map.*<br /><br /></asp:Label>
                        <div class="float_right">
                            <asp:Label runat="server" Visible="false" ID="extra_map_warning_label" ForeColor="Red" Text="You Have The Range Map Selected with No Values. Please Either De-Select Range Map or Enter Appropriate Information."></asp:Label>
                            <asp:Button ID="resetButton" runat="server" Text="Reset Selections" CssClass="display_none"
                                OnClientClick="resetView();return false;" />
                            <asp:Button ID="btnRunReport" runat="server" Text="Run Report" OnClick="runReport" OnClientClick="SetWaitCursor();SetLoadingText('Generating Report..');$('body').addClass('loading standalone_page');" />
                            <asp:Button ID="select_all_types_button" runat="server" Text="Select Types" />
                        </div>
                        <div class="float_right">
                            <asp:UpdatePanel runat="server" ID="updateSelections">
                                <ContentTemplate>
                                    <asp:Button ID="btnSave_Selections" runat="server" Text="Save Selections" CssClass="display_inline"
                                        OnClientClick="javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait standalone_page');" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
            <!--  OnCheckedChanged="Check_Clicked"  -->
            <asp:Chart ID="AVG_SOLD_PER_MONTH" Visible="false" runat="server" ImageStorageMode="UseImageLocation"
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
            <br />
            <br />
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
            <br />
            <br />
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
            <br />
            <br />
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
            <br />
            <br />
            <asp:Chart ID="OP_COUNTRY_CHART" runat="server" ImageStorageMode="UseImageLocation"
                ImageType="Jpeg" Visible="False">
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
            <br />
            <br />
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
            <br />
            <br />
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
            <br />
            <br />
            <asp:Chart ID="M_TREND" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
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
            <br />
            <br />
            <asp:Chart ID="M_TREND2" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
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
            <br />
            <br />
            <asp:Chart ID="SALES_TRENDS" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
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
            <asp:Chart ID="LEASES_SOLD_PER_MONTH" runat="server" ImageStorageMode="UseImageLocation"
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
            <asp:Chart ID="DOCS_BY_MONTH_GRAPH" runat="server" ImageStorageMode="UseImageLocation"
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
            <asp:Chart ID="VALUES_CHART" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
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
            <asp:Chart ID="VALUES_CHART2" runat="server" ImageStorageMode="UseImageLocation"
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
            <asp:Chart ID="VALUES_CHART3" runat="server" ImageStorageMode="UseImageLocation"
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
            <asp:Chart ID="IN_OPERATION_CHART" runat="server" ImageStorageMode="UseImageLocation"
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
            </td> </tr> </table>
        </div>
        <asp:Panel runat="server" Visible="false" ID="searchPanelToggle" CssClass="valuesSearchPanel">
            <asp:UpdatePanel runat="server" ID="modelUpdatePanel" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox runat="server" ID="acIDText" CssClass="display_none"></asp:TextBox>
                    <asp:TextBox runat="server" ID="variantThere" CssClass="display_none"></asp:TextBox>
                    <div class="row">
                        <div class="two columns removeLeftMargin displayNoneMobile">
                            <label>
                                Aircraft Model:</label>
                        </div>
                        <div class="five columns removeLeftMargin">
                            <asp:DropDownList runat="server" Width="102%" ID="modelList" CssClass="chosen-select"
                                AutoPostBack="true" data-placeholder="Please Pick a Model">
                            </asp:DropDownList>
                            <div class="mobile_display_on_cell mobileChosenSpacer">
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="row div_clear toggleSmallScreen">
                <div class="one columns removeLeftMargin">
                    <label>
                        Year(s):</label>
                </div>
                <div class="one columns">
                    <asp:TextBox runat="server" ID="year_start" CssClass="amount float_right"></asp:TextBox>
                </div>
                <div class="four columns removeLeftMargin">
                    <div id="slider-range">
                    </div>
                </div>
                <div class="one columns">
                    <asp:TextBox runat="server" ID="year_end" CssClass="amount float_left"></asp:TextBox>
                </div>
            </div>
            <div class="row toggleSmallScreen">
                <div class="one columns removeLeftMargin">
                    <label>
                        AFTT:</label>
                </div>
                <div class="one columns">
                    <asp:TextBox runat="server" ID="aftt_start" CssClass="amount float_right"></asp:TextBox>
                </div>
                <div class="four columns removeLeftMargin">
                    <div id="aftt-range">
                    </div>
                </div>
                <div class="one columns">
                    <asp:TextBox runat="server" ID="aftt_end" CssClass="amount float_left"></asp:TextBox>
                </div>
            </div>
            <div class="row toggleSmallScreen">
                <div class="two columns removeLeftMargin">
                    <label>
                        Registration:</label>
                </div>
                <div class="two columns removeLeftMargin">
                    <asp:DropDownList ID="aircraft_registration" runat="server" CssClass="chosen-select"
                        Width="100%">
                        <asp:ListItem Text="US (Domestic)" Value="N"></asp:ListItem>
                        <asp:ListItem Text="International" Value="I"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="Worldwide" Value="Worldwide"></asp:ListItem>
                    </asp:DropDownList>
                    <div class="mobile_display_on_cell mobileChosenSpacer">
                    </div>
                </div>
                <div class="two columns">
                    <label>
                        &nbsp;</label>
                </div>
                <div class="two columns removeLeftMargin">
                </div>
                <div class="threehalf columns removeLeftMargin">
                    <asp:UpdatePanel runat="server" ID="loadWhatUpdate" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="loadWhatAC" runat="server" CssClass="chosen-select" Width="100%"
                                AutoPostBack="true" onchange="SetLoadingText('Loading Current Aircraft');$('body').addClass('loading');">
                                <asp:ListItem Text="For Sale Market" Value="Y" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="All In Operation Aircraft" Value="All"></asp:ListItem>
                            </asp:DropDownList>
                            <div class="mobile_display_on_cell mobileChosenSpacer">
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="row removeMarginBottom">
                <div class="sevenhalf columns removeLeftMargin">
                    <asp:Label runat="server" ID="variantModelText" CssClass="display_none" ForeColor="Red">
      *Variant Models Loaded, Including: </asp:Label>
                </div>
                <div class="clearfix">
                </div>
                <asp:Label runat="server" ID="FolderInformation" Visible="false" CssClass="FolderNameBar help_cursor"></asp:Label>
                <asp:Label runat="server" ID="StaticFolderNewSearchLabel"></asp:Label>
            </div>
        </asp:Panel>
        <asp:TextBox runat="server" ID="acRangeText" CssClass="display_none"></asp:TextBox>
        <asp:Label runat="server" ID="ac_market" Text="" Visible="false"></asp:Label>
        <asp:Label runat="server" ID="start_date" Visible="false"></asp:Label>
        <asp:Label runat="server" ID="end_date" Visible="false"></asp:Label>
        <asp:Label runat="server" ID="VariantList" Text="" Visible="false"></asp:Label>
        <asp:Label runat="server" ID="modelAirframeTypeCode" CssClass="display_none"></asp:Label>
        <asp:Label runat="server" ID="ModelTypeCode" CssClass="display_none"></asp:Label>
        <asp:Label runat="server" ID="ModelWeightClass" CssClass="display_none"></asp:Label>
        <asp:Label runat="server" ID="invisible_label_parent_image"></asp:Label>
        <asp:Panel ID="large_graph_panel" runat="server" Visible="true" Width='100%'>
            <asp:UpdatePanel ID="graph_update_panel" Visible="false" runat="server">
                <ContentTemplate>
                    <cc1:TabContainer ID="tabcontainer_graph" runat="server" Width="100%" BorderStyle="None"
                        Style="margin-left: auto; margin-right: auto;" CssClass="dark-theme">
                        <cc1:TabPanel ID="graph_panel" runat="server">
                            <ContentTemplate>
                                <br />
                                <br />
                                <div id="large_graph_div">
                                </div>
                                <br />
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>

        <div id="graphContainer" style="visibility: hidden;">
            <asp:Panel runat="server" ID="featuresGaugePanel">
                <canvas id="features1"></canvas>
                <asp:TextBox runat="server" ID="features1text"></asp:TextBox>
                <div id='features1image' runat="server" clientidmode="Static">
                </div>
                <canvas id="features2"></canvas>
                <asp:TextBox runat="server" ID="features2text"></asp:TextBox>
                <div id='features2image' runat="server" clientidmode="Static">
                </div>
                <canvas id="features3"></canvas>
                <asp:TextBox runat="server" ID="features3text"></asp:TextBox>
                <div id='features3image' runat="server" clientidmode="Static">
                </div>
                <canvas id="features4"></canvas>
                <asp:TextBox runat="server" ID="features4text"></asp:TextBox>
                <div id='features4image' runat="server" clientidmode="Static">
                </div>
                <canvas id="features5"></canvas>
                <asp:TextBox runat="server" ID="features5text"></asp:TextBox>
                <div id='features5image' runat="server" clientidmode="Static">
                </div>
                <canvas id="features6"></canvas>
                <asp:TextBox runat="server" ID="features6text"></asp:TextBox>
                <div id='features6image' runat="server" clientidmode="Static">
                </div>
                <canvas id="program1"></canvas>
                <asp:TextBox runat="server" ID="program1text"></asp:TextBox>
                <div id='program1image' runat="server" clientidmode="Static">
                </div>
                <canvas id="program2"></canvas>
                <asp:TextBox runat="server" ID="program2text"></asp:TextBox>
                <div id='program2image' runat="server" clientidmode="Static">
                </div>
                <canvas id="program3"></canvas>
                <asp:TextBox runat="server" ID="program3text"></asp:TextBox>
                <div id='program3image' runat="server" clientidmode="Static">
                </div>
            </asp:Panel>
            <div id="visualization20" style="height: 295px;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText20"></asp:TextBox>
            <div id='png20' runat="server" clientidmode="Static">
            </div>
            <div id='visualization1' style="height: 295px;">
            </div>
            <asp:TextBox runat="server" ID="visualizationGraphText"></asp:TextBox>
            <div id='visualizationPNG1' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab1_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText"></asp:TextBox>
            <div id='png2' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab4_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText4"></asp:TextBox>
            <div id='png4' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab5_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText5"></asp:TextBox>
            <div id='png5' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab6_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText6"></asp:TextBox>
            <div id='png6' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab7_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText7"></asp:TextBox>
            <div id='png7' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab8_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText8"></asp:TextBox>
            <div id='png8' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab9_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText9"></asp:TextBox>
            <div id='png9' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab10_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText10"></asp:TextBox>
            <div id='png10' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab11_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText11"></asp:TextBox>
            <div id='png11' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab12_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText12"></asp:TextBox>
            <div id='png12' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab13_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText13"></asp:TextBox>
            <div id='png13' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab14_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText14"></asp:TextBox>
            <div id='png14' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab15_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText15"></asp:TextBox>
            <div id='png15' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab16_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText16"></asp:TextBox>
            <div id='png16' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab17_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText17"></asp:TextBox>
            <div id='png17' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab18_all" style="border-top: 0">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText18"></asp:TextBox>
            <div id='png18' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab19_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText19"></asp:TextBox>
            <div id='png19' runat="server" clientidmode="Static">
            </div>
            <canvas id="scripted-gauge"></canvas>
            <asp:TextBox runat="server" ID="valueGraphTextGuage1"></asp:TextBox>
            <div id='pngGuage1' runat="server" clientidmode="Static">
            </div>
            <canvas id="scripted-gaugeUS"></canvas>
            <asp:TextBox runat="server" ID="valueGraphTextGuageUS"></asp:TextBox>
            <div id='pngGuageUS' runat="server" clientidmode="Static">
            </div>
            <canvas id="scripted-gaugeForeign"></canvas>
            <asp:TextBox runat="server" ID="valueGraphTextGuageForeign"></asp:TextBox>
            <div id='pngGuageForeign' runat="server" clientidmode="Static">
            </div>
            <canvas id="scripted-gauge2"></canvas>
            <asp:TextBox runat="server" ID="valueGraphTextGuage2"></asp:TextBox>
            <div id='pngGuage2' runat="server" clientidmode="Static">
            </div>
            <canvas id="scripted-gauge3"></canvas>
            <asp:TextBox runat="server" ID="valueGraphTextGauge3"></asp:TextBox>
            <div id='pngGauge3' runat="server" clientidmode="Static">
            </div>
            <canvas id="scripted-gauge4"></canvas>
            <asp:TextBox runat="server" ID="valueGraphTextGauge4"></asp:TextBox>
            <div id='pngGauge4' runat="server" clientidmode="Static">
            </div>
            <canvas id="scripted-gauge5"></canvas>
            <asp:TextBox runat="server" ID="valueGraphTextGauge5"></asp:TextBox>
            <div id='pngGauge5' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab20_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="TextBox1"></asp:TextBox>
            <div id='Div1' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab21_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText21"></asp:TextBox>
            <div id='png21' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab22_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="ValueGraphText22"></asp:TextBox>
            <div id='png22' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab23_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="ValueGraphText23"></asp:TextBox>
            <div id='png23' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab24_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText24"></asp:TextBox>
            <div id='png24' runat="server" clientidmode="Static">
            </div>
            <canvas id="scripted-gauge6"></canvas>
            <asp:TextBox runat="server" ID="valueGraphTextGauge6"></asp:TextBox>
            <div id='pngGauge6' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab25_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText25"></asp:TextBox>
            <div id='png25' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab26_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText26"></asp:TextBox>
            <div id='png26' runat="server" clientidmode="Static">
            </div>

            <div id="chart_div_tab32_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText32"></asp:TextBox>
            <div id='png32' runat="server" clientidmode="Static">
            </div>

            <div id="chart_div_tab33_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valueGraphText33"></asp:TextBox>
            <div id='png33' runat="server" clientidmode="Static">
            </div>

            <div id="chart_div_tab_Valuation1_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valuuationGraph1"></asp:TextBox>
            <div id='pngValuation1' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab_Valuation2_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valuuationGraph2"></asp:TextBox>
            <div id='pngValuation2' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab_Valuation3_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valuuationGraph3"></asp:TextBox>
            <div id='pngValuation3' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab_Valuation4_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valuuationGraph4"></asp:TextBox>
            <div id='pngValuation4' runat="server" clientidmode="Static">
            </div>
            <div id="chart_div_tab_Valuation5_all" style="border-top: 0;">
            </div>
            <asp:TextBox runat="server" ID="valuuationGraph5"></asp:TextBox>
            <div id='pngValuation5' runat="server" clientidmode="Static">
            </div>
            <asp:Label runat="server" ID="invisible_count_years" Visible="false" Text=""></asp:Label>
        </div>
    </asp:Panel>

</asp:Content>
<asp:Content runat="server" ID="below" ContentPlaceHolderID="below_form">

    <script>
        function SetLoadingText(textToSet) {
            $("#divLoading").css("display", "block");
        //  $('#<%'=loadingText.ClientID %>').text(textToSet);
        }

        function swapChosenDropdowns() {
            $(".chosen-select").chosen("destroy");
            $(".chosen-select").chosen({ no_results_text: "No results found.", disable_search_threshold: 10, search_contains: true });
        }

        $(document).ready(function () {
            $(".checkboxDiv").each(function (index) {
                var self = $(this);
                if ($(this).find(".listCheck").length > 0) {
                    $(this).find("strong").removeClass("display_none");
                } else {
                    $(this).addClass("display_none")
                };
            })
            setUpAutoComplete();
            $("#divLoading").css("display", "none");
        });

        function setUpAutoComplete() {
            $("#<%= destination.clientID %>").prop("disabled", false);
            $("#<%= destination.clientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "GET",
                        url: "JSONresponse.aspx/AirportIata?term=" + request.term,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: {},
                        success: function (data) {
                            var json = data.d;
                            var obj = JSON.parse(json);
                            response(obj);
                        }
                    });
                },
                minLength: 3,
                select: function (event, ui) {
                    $("#<%= destination.clientID %>").val(ui.item.label);
                    $("#<%= destination_id.clientID %>").val(ui.item.value);
                    build_range_tab_map($('#<%=destination_id.ClientID  %>').val(), $('#<%= acRangeText.clientID %>').val());
                    return false;
                }
            });
        }
    </script>

</asp:Content>
