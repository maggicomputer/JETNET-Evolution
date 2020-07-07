<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master"
    CodeBehind="userPortfolio.aspx.vb" Inherits="crmWebClient.userPortfolio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">



    <style type="text/css">
        .dataTables_scrollHead {
            width: 100% !important;
        }


        .gaugePanel .Box {
            height: 150px !important;
            overflow: hidden;
        }

        .gaugePanel .three.columns {
            width: 23% !important;
            margin: 0px 10px 0px 7px !important;
        }

        .Box .subHeader {
            font-size: 15px !important;
        }

        .gaugePanel .three.columns .Box .subHeader {
            height: 35px !important;
        }

        .gaugePanel .three.columns .Box canvas {
            margin-left: -13px;
        }

        .green_text {
            color: #509c23 !important;
        }

        .alignTable {
            margin-left: 0px;
            width: 100%;
        }

            .alignTable td {
                text-align: center;
            }

        .spacingRow {
            width: 100%;
            margin-left: 1.25%;
        }

        .grid-item {
            display: inline-block;
            width: auto;
        }

        .valueSpec.Simplistic .Box.marginTop {
            margin-top: 0px !important;
        }

        .spacingRow .grid-item {
            margin-right: 1.25% !important;
            margin-left: 0px;
        }
    </style>
    <style type="text/css">
        /* ---- grid ---- */
        .grid {
            margin-left: auto !important;
            margin-right: auto !important;
            width: 100%;
        }

            /* clearfix */
            .grid:after {
                content: '';
                display: block;
                clear: both;
            }

        /* ---- grid-item ---- */
        .grid-item {
            margin-right: 1em;
            margin-bottom: 10px !important;
            float: left;
        }

            .grid-item .Box, .grid-item.Box {
                -webkit-box-shadow: 1px 1px 2px 1px #C9C9C9;
                box-shadow: 1px 1px 2px 1px #C9C9C9;
            }

                .grid-item .Box .Box, .grid-item.Box .Box {
                    -webkit-box-shadow: 0px 0px 0px 0px #C9C9C9;
                    box-shadow: 0px 0px 0px 0px #C9C9C9;
                }

        .Box .Box {
            border: 0px !important;
        }

        .valueSpec.Simplistic .Box.grid-item .subHeader {
            /*  color: #078fd7 !important*/
        }

        .marginLeft2 {
            margin-left: 2%;
        }

        .marginLeftHalf {
            margin-left: .5%;
        }

        .featuresDescriptionLine {
            border-top: 1px solid #d0d7da;
            padding-top: 7px;
        }
    </style>
    <script type="text/javascript" src="https://cdn.rawgit.com/Mikhus/canvas-gauges/gh-pages/download/2.1.4/all/gauge.min.js"></script>


    <script>
        function setFlightActivityView(portfolioID, portfolioName) {
            my_form = document.createElement('FORM');
            window.open('', 'result' + portfolioID, 'width=1150,height=900');
            my_form.method = 'POST';
            my_form.target = 'result' + portfolioID

            my_form.name = 'mappingForm';
            my_form.action = 'view_template.aspx?noMaster=false&ViewID=28&ViewName=Flight Activity (Operator/Airport)';

            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = "acfolder";
            my_tb.value = portfolioID;
            my_form.appendChild(my_tb);

            document.body.appendChild(my_form);

            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = "acfoldername";
            my_tb.value = portfolioName;
            my_form.appendChild(my_tb);

            document.body.appendChild(my_form);
            my_form.submit();
        }
        function CreateSearchTableArray0(divName, tableName, jQueryTablename, enableExport) {
            var countItem = 'Records.';
            //var countItem = 'Aircraft.';
            //if (tableName == 'tab6_DataTable') {
            //    countItem = 'Aircraft Owners.';
            //} else if (tableName == 'tab5_DataTable') {
            //    countItem = 'Aircraft with Operators.';
            //} else if (tableName == 'tab7_DataTable') {
            //    countItem = 'Models.';
            //} else if (tableName == 'tab8_DataTable') {
            //    countItem = 'Transactions.';
            //} else if (tableName == 'tab8_DataTable_folder') {
            //    countItem = 'Transactions.';
            //} else if (tableName == 'tab9_DataTable') {
            //    countItem = 'Records.';
            //} else if (tableName == 'tab9_DataTable_Summary') {
            //    countItem = 'Records.';
            //} else if (tableName == 'tab0_DataTable_Summary') {
            //    countItem = 'Records.';
            //}



            var columnSetArray = [];
            var selectedRows = '';
            //var enableExport = true;
            var dynamicDataSet;
            try {
                if ($.fn.DataTable.isDataTable("#" + jQueryTablename)) {
                    $("#" + divName).empty();
                };

            }
            catch (err) {

            }




            if ($("#" + tableName).length) {

                var showNotes = $('#<%= Show_Notes.ClientID%>').is(":checked");
                var cssClassShow = "";
                if (showNotes == false) {
                    cssClassShow = "display_none"
                }
                switch (tableName) {
                    case "tab0_DataTable":
                        {
                            dynamicDataSet = tab0DataSet;
                            // alert(tab0DataSet);
                            selectedRows = "<%= selected_aircraft_rows_tabPanel0.ClientID %>";

                            columnSetArray = [
                                { title: "SEL", width: "20px", data: "check" },
                                { title: "id", data: "id" },
                                { title: "NOTES", data: "note", className: cssClassShow },
                                { title: "MAKE", data: "make" },
                                { title: "MODEL", data: "model" },
                                { title: "SER #", data: "ser.0", _: "ser.1" },
                                { title: "REG #", data: "reg" },
                                { title: "MFR YEAR", data: "mfryear" },
                                { title: "DLV YEAR", data: "dlvyear" },
                                { title: "LIFECYCLE", data: "lifecycle" },
                                { title: "OWNERSHIP", data: "ownership" },
                                { title: "OWNER", data: "owner" },
                                { title: "OPERATOR", data: "operator" },
                                { title: "BASEAPORT", data: "baseaport" },
                                { title: "BASECOUNTRY", data: "basecountry" },
                                { title: "ESTAIRFRAMEHRS", data: "estairframehrs", className: "text_align_right" }
                            ];
                        }
                        break;
                    case "tab0_DataTable_Summary":
                        {
                            dynamicDataSet = tab0DataSet;
                            // alert(tab0DataSet);
                            selectedRows = "<%= selected_aircraft_rows_tabPanel0.ClientID %>";
                            columnSetArray = [
                                { title: "SEL", width: "20px", data: "check" },
                                { title: "id", data: "id" },
                                { title: "SUMMARIZED", data: { _: "Summarized.0", sort: "Summarized.1" } },
                                { title: "TOTAL", data: "Total", className: "text_align_right" }
                            ];
                        }
                        break;
                    case "tab1_DataTable":
                        {
             // enableExport = <%'= iif(Session.Item("localUser").crmDemoUserFlag, "false","true") %>
                            dynamicDataSet = tab1DataSet;
                            selectedRows = "<%= selected_aircraft_rows_tabPanel1.clientID %>";
                            columnSetArray = [
                                { title: "SEL", width: "20px", data: "check" },
                                { title: "id", data: "id" },
                                { title: "NOTES", data: "note", className: cssClassShow },
                                { title: "MAKE", data: "make" },
                                { title: "MODEL", data: "model" },
                                { title: "SER #", data: "ser.0", _: "ser.1" },
                                { title: "REG #", data: "reg" },
                                { title: "MFR YEAR", data: "mfryear" },
                                { title: "DLV YEAR", data: "dlvyear" },
                                { title: "STATUS", data: "status" },
                                { title: "ASKING", data: "asking.0", _: "asking.1", className: "text_align_right" },
              <%= iif (isDisplayEvalues, "{ title: ""EVALUE"", data: ""eval"", className: ""evalue_blue text_align_right"" },{ title: ""AVG MODEL YEAR"", data: ""avgmod"", className: ""evalue_blue text_align_right"" },","")  %>

                                { title: "DATE LISTED", data: "listdate.0", _: "listdate.1" },
                                { title: "AFTT", data: "tothrs", className: "text_align_right" },
                                { title: "ENGINE TT", data: "tothrs1", className: "text_align_right" },
                                { title: "PAX", data: "PAX", className: "text_align_right" },
                                { title: "INTERIOR YEAR", data: "intyear" },
                                { title: "EXTERIOR YEAR", data: "extyear" },
                                { title: "BASED", data: "based" }
                            ];
                        }
                        break;
                    case "tab2_DataTable":
                        {
                            dynamicDataSet = tab2DataSet;
                            selectedRows = "<%= selected_aircraft_rows_tabPanel2.clientID %>";
                            columnSetArray = [
                                { title: "SEL", width: "20px", data: "check" },
                                { title: "id", data: "id" },
                                { title: "NOTES", data: "note", className: cssClassShow },
                                { title: "MAKE", data: "make" },
                                { title: "MODEL", data: "model" },
                                { title: "SER #", data: "ser.0", _: "ser.1" },
                                { title: "REG #", data: "reg" },
                                { title: "ESTAIRFRAMEHOURS", data: "estairframe", className: "text_align_right" },
                                { title: "AIRFRAMEMAINTPROGRAM", data: "airmaint" },
                                { title: "AIRFRAMETRACKPROGRAM", data: "airtrack" },
                                { title: "MAINTAINED", data: "maintained" },
                                { title: "ENGINEMODELNAME", data: "enginemodel" },
                                { title: "ENGINEMAINTPROGRAM", data: "enginemaint" },
                                { title: "ENG1HRS", data: "eng1", className: "text_align_right" },
                                { title: "ENG2HRS", data: "eng2", className: "text_align_right" },
                                { title: "ENG1SOHHRS", data: "eng1so", className: "text_align_right" },
                                { title: "ENG2SOHHRS", data: "eng2so", className: "text_align_right" },
                                { title: "APUMODELNAME", data: "apumodel" },
                                { title: "APUPROGRAMNAME", data: "apuprogram" },
                                { title: "INTERIORDATE", data: "interiordate" },
                                { title: "INTERIORDONEBY", data: "interiordone" },
                                { title: "EXTERIORDATE", data: "exteriordate" },
                                { title: "LASTREPORTEDHRS", data: "lastreportedhrs", className: "text_align_right" },
                                { title: "LASTREPORTEDCYCLES", data: "lastreportedcycles", className: "text_align_right" },
                                { title: "LASTREPORTEDDATE", data: "lastreporteddate" },
                                { title: "DAMAGE", data: "damage" }
                            ]
                        }
                        break;
                    case "tab2_DataTable_Summary":
                        {
                            dynamicDataSet = tab2DataSet;
                            selectedRows = "<%= selected_aircraft_rows_tabPanel2.ClientID %>";
                            columnSetArray = [
                                { title: "SEL", width: "20px", data: "check" },
                                { title: "id", data: "id" },
                                { title: "SUMMARIZED", data: { _: "Summarized.0", sort: "Summarized.1" } },
                                { title: "TOTAL", data: "Total", className: "text_align_right" }
                            ]
                        }
                        break;
                    case "tab3_DataTable":
                        {
                            selectedRows = "<%= selected_aircraft_rows_tabPanel3.clientID %>";
                            dynamicDataSet = tab3DataSet;

                            columnSetArray = [
                                { title: "SEL", width: "20px", data: "check" },
                                { title: "id", data: "id" },
                                { title: "NOTES", data: "note", className: cssClassShow },
                                { title: "MAKE", data: "make" },
                                { title: "MODEL", data: "model" },
                                { title: "SER #", data: "ser.0", _: "ser.1" },
                                { title: "REG #", data: "reg" },
                                { title: "MFR YEAR", data: "mfryear" },
                                { title: "DLV YEAR", data: "dlvyear" },
                                { title: "LIFECYCLE", data: "lifecycle" },
                                { title: "OWNERSHIP", data: "ownership" },
                                { title: "OWNER", data: "owner" },
                                { title: "OPERATOR", data: "operator" },
                                { title: "BASEAPORT", data: "baseaport" },
                                { title: "BASECOUNTRY", data: "basecountry" },
                                { title: "ESTAIRFRAMEHRS", data: "estairframehrs", className: "text_align_right" }
                            ];
                        }
                        break;
                    case "tab3_DataTable_Summary":
                        {
                            dynamicDataSet = tab3DataSet;
                            selectedRows = "<%= selected_aircraft_rows_tabPanel3.ClientID %>";
                            columnSetArray = [
                                { title: "SEL", width: "20px", data: "check" },
                                { title: "id", data: "id" },
                                { title: "SUMMARIZED", data: { _: "Summarized", sort: "Summarized" } },
                                { title: "TOTAL", data: "Total", className: "text_align_right" }
                            ]
                        }
                        break;
                    case "tab4_DataTable":
                        {
                            selectedRows = "<%= selected_aircraft_rows_tabPanel4.clientID %>";
                            dynamicDataSet = tab4DataSet;
                            columnSetArray = [
                                { title: "SEL", width: "20px", data: "check" },
                                { title: "id", data: "id" },
                                { title: "NOTES", data: "note", className: cssClassShow },
                                { title: "MAKE", data: "make" },
                                { title: "MODEL", data: "model" },
                                { title: "SER #", data: "ser.0", _: "ser.1" },
                                { title: "REG #", data: "reg" },
                                { title: "BASE", data: "base" },
                                { title: "NBR FLIGHTS", data: "flts12months", className: "text_align_right" },
                                { title: "FLIGHTS/MO", data: "fltspermonths", className: "text_align_right" },
                                { title: "TOTAL FLIGHT HOURS", data: "totalflighttimehrs", className: "text_align_right" },
                                { title: "EST FUEL BURN (GAL)", data: "totalfuelburn", className: "text_align_right" }
                            ]
                        }
                        break;
                    case "tab5_DataTable":
                        {
                            selectedRows = "<%= selected_aircraft_rows_tabPanel5.clientID %>";
                            dynamicDataSet = tab5DataSet;
                            columnSetArray = [
                                { title: "SEL", width: "20px", data: "check" },
                                { title: "id", data: "id" },
                                { title: "NOTES", data: "note", className: cssClassShow },
                                { title: "MAKE", data: "make" },
                                { title: "MODEL", data: "model" },
                                { title: "SER #", data: "ser.0", _: "ser.1" },
                                { title: "REG #", data: "reg" },
                                { title: "MFR YEAR", data: "mfryear" },
                                { title: "DLV YEAR", data: "dlvyear" },
                                { title: "OPERATOR", data: "operator" },
                                { title: "OPCITY", data: "opcity" },
                                { title: "OPSTATE", data: "opstate" },
                                { title: "OPCOUNTRY", data: "opcountry" },
                                { title: "OPWEBADDRESS", data: "opwebaddress" },
                                { title: "OPEMAIL", data: "opemail" },
                                { title: "OPOFFICEPHONE", data: "opofficephone" },
                                { title: "CONTACTNAME", data: "contactname" },
                                { title: "CONTACTTITLE", data: "contacttitle" },
                                { title: "CONTACTEMAIL", data: "contactemail" },
                                { title: "CONTACTOFFICEPHONE", data: "contactofficephone" },
                                { title: "CONTACTMOBILEPHONE", data: "contactmobilephone" }
                            ]
                        }
                        break;
                    case "tab6_DataTable":
                        {
                            selectedRows = "<%= selected_aircraft_rows_tabPanel6.clientID %>";
                            dynamicDataSet = tab6DataSet;
                            columnSetArray = [
                                { title: "SEL", width: "20px", data: "check" },
                                { title: "id", data: "id" },
                                { title: "NOTES", data: "note", className: cssClassShow },
                                { title: "MAKE", data: "make" },
                                { title: "MODEL", data: "model" },
                                { title: "SER #", data: "ser.0", _: "ser.1" },
                                { title: "REG #", data: "reg" },
                                { title: "MFR YEAR", data: "mfryear" },
                                { title: "DLV YEAR", data: "dlvyear" },
                                { title: "OWNERSHIP", data: "ownership" },
                                { title: "PERCENTOWNED", data: "percentowned", className: "text_align_right" },
                                { title: "OWNER", data: "owner" },

                                { title: "OWNERCITY", data: "ownercity" },
                                { title: "OWNERSTATE", data: "ownerstate" },
                                { title: "OWNERCOUNTRY", data: "ownercountry" },

                                { title: "OWNERWEBADDRESS", data: "ownerwebaddress" },
                                { title: "OWNEREMAIL", data: "owneremail" },
                                { title: "OWNEROFFICEPHONE", data: "ownerofficephone" },

                                { title: "CONTACTNAME", data: "contactname" },
                                { title: "CONTACTTITLE", data: "contacttitle" },
                                { title: "CONTACTEMAIL", data: "contactemail" },
                                { title: "CONTACTOFFICEPHONE", data: "contactofficephone" },
                                { title: "CONTACTMOBILEPHONE", data: "contactmobilephone" }
                            ]
                        }
                        break;
                    case "tab7_DataTable":
                        {
                            selectedRows = "<%= selected_aircraft_rows_tabPanel7.clientID %>";
                            dynamicDataSet = tab7DataSet;
                            columnSetArray = [
                                { title: "SEL", width: "20px", data: "check" },
                                { title: "id", data: "id" },
                                { title: "MAKE", data: "make" },
                                { title: "MODEL", data: "model" },
                                { title: "NUMAIRCRAFT", data: "numaircraft", className: "text_align_right" }
                            ]
                        }
                        break;
                    case "tab8_DataTable":
                        {
             //enableExport = <%'= iif(Session.Item("localUser").crmDemoUserFlag, "false","true") %>
                            selectedRows = "<%= selected_aircraft_rows_tabPanel8.clientID %>";
                            dynamicDataSet = tab8DataSet;
                            columnSetArray = [
                                { title: "SEL", width: "20px", data: "check" },
                                { title: "id", data: "id" },
                                { title: "NOTES", data: "note", className: cssClassShow },
                                { title: "MAKE", data: "make" },
                                { title: "MODEL", data: "model" },
                                { title: "SER #", data: "ser.0", _: "ser.1" },
                                { title: "REG #", data: "reg" },
                                { title: "MFR YEAR", data: "mfryear" },
                                { title: "DLV YEAR", data: "dlvyear" },
                                { title: "TRANS DATE", data: "TRANS_DATE" },
                                { title: "DESCRIPTION", data: "DESCRIPTION" },
                                { title: "LIST DATE", data: "LIST_DATE" },
                                { title: "ASKING PRICE", data: "ASKING_PRICE", className: "text_align_right" },
                                { title: "SALE PRICE", data: "SALE_PRICE", className: "text_align_right" },
                                { title: "RELATIONSHIP", data: "RELATIONSHIP" },
                                { title: "SELLER", data: "seller" },
                                { title: "PURCHASER", data: "purchaser" },
                                //  { title: "LIFECYCLE", data: "lifecycle" },
                                //    { title: "OWNERSHIP", data: "ownership" },
                                //   { title: "BASEAPORT", data: "baseaport" },
                                //   { title: "BASECOUNTRY", data: "basecountry" },
                                //   { title: "ESTAIRFRAMEHRS", data: "estairframehrs" }
                            ]
                        }
                        break;
                    case "tab8_DataTable_folder":
                        {
              //enableExport = <%'= iif(Session.Item("localUser").crmDemoUserFlag, "false","true") %>;
                            selectedRows = "<%= selected_aircraft_rows_tabPanel8.clientID %>";
                            dynamicDataSet = tab8DataSet;
                            columnSetArray = [
                                { title: "SEL", width: "20px", data: "check" },
                                { title: "id", data: "id" },
                                { title: "NOTES", data: "note", className: cssClassShow },
                                { title: "MAKE", data: "make" },
                                { title: "MODEL", data: "model" },
                                { title: "SER #", data: "ser.0", _: "ser.1" },
                                { title: "REG #", data: "reg" },
                                { title: "MFR YEAR", data: "mfryear" },
                                { title: "DLV YEAR", data: "dlvyear" },
                                { title: "TRANS DATE", data: "TRANS_DATE" },
                                { title: "DESCRIPTION", data: "DESCRIPTION" },
                                { title: "LIST DATE", data: "LIST_DATE" },
                                { title: "ASKING PRICE", data: "ASKING_PRICE", className: "text_align_right" },
                                { title: "SALE PRICE", data: "SALE_PRICE", className: "text_align_right" },
                                // { title: "RELATIONSHIP", data: "RELATIONSHIP" }, 
                                { title: "SELLER", data: "seller" },
                                { title: "PURCHASER", data: "purchaser" },
                                //  { title: "LIFECYCLE", data: "lifecycle" },
                                //    { title: "OWNERSHIP", data: "ownership" },
                                //   { title: "BASEAPORT", data: "baseaport" },
                                //   { title: "BASECOUNTRY", data: "basecountry" },
                                //   { title: "ESTAIRFRAMEHRS", data: "estairframehrs" }
                            ]
                        }
                        break;
                    case "tab9_DataTable":
                        {
             //enableExport = <%'= iif(Session.Item("localUser").crmDemoUserFlag, "false","true") %>
                            selectedRows = "<%= selected_aircraft_rows_tabPanel9.clientID %>";
                            dynamicDataSet = tab9DataSet;
                            columnSetArray = [
                                { title: "SEL", width: "20px", data: "check" },
                                { title: "id", data: "id" },
                                { title: "NOTES", data: "note", className: cssClassShow },
                                { title: "MAKE", data: "make" },
                                { title: "MODEL", data: "model" },
                                { title: "SER #", data: "ser.0", _: "ser.1" },
                                { title: "REG #", data: "reg" },
                                { title: "MFR YEAR", data: "mfryear" },
                                { title: "DLV YEAR", data: "dlvyear" },
                                { title: "BASE IATA CODE", data: "BASE_IATA" },
                                { title: "BASE ICAO CODE", data: "BASE_ICAO" },
                                { title: "FAA ID", data: "FAA_ID" },
                                { title: "APORT NAME", data: "APORT_NAME" },
                                { title: "APORT CITY", data: "APORT_CITY" },
                                { title: "APORT STATE", data: "APORT_STATE" },
                                { title: "APORT COUNTRY", data: "APORT_COUNTRY" },
                                { title: "APORT CONTINENT", data: "APORT_CONTINENT" },
                                { title: "REG COUNTRY", data: "REG_COUNTRY" },
                            ]
                        }
                        break;
                    case "tab9_DataTable_Summary":
                        {
             //enableExport = <%'= iif(Session.Item("localUser").crmDemoUserFlag, "false","true") %>
                            selectedRows = "<%= selected_aircraft_rows_tabPanel9.ClientID %>";
                            dynamicDataSet = tab9DataSet;
                            columnSetArray = [
                                { title: "SEL", width: "20px", data: "check" },
                                { title: "id", data: "id" },
                                { title: "Location", data: { _: "Location.0", sort: "Location.1" } },
                                { title: "Total", data: "Total", className: "text_align_right" },
                            ]
                        }
                        break;
                }

                $("#<%= filter_draw.ClientID %>").val("");
                var clone = jQuery("#" + tableName).clone(true);

                jQuery("#" + tableName).css('display', 'none');
                clone[0].setAttribute('id', jQueryTablename);
                clone.appendTo("#" + divName);


                var table = $("#" + jQueryTablename).DataTable({
                    data: dynamicDataSet,
                    destroy: true,
                    language: { "search": "Filter:" },
                    fixedHeader: true,
                    "initComplete": function (settings, json) {
                        setTimeout(function () {
                            $("#" + jQueryTablename).DataTable().columns.adjust();
                            $("#" + jQueryTablename).DataTable().scroller.measure();

                            // var dataRows = $("#" + jQueryTablename).DataTable().rows();
                            // selectAllRows(dataRows.data(), selectedRows, tableName);

                        }, 1200)
                    },
                    scrollCollapse: true,
                    scroller: true,
                    deferRender: true,
                    stateSave: true,
                    paging: true,
                    processing: true,
                    autoWidth: true,
                    scrollY: 390,
                    scrollX: 960,
                    pageLength: 100,
                    columns: columnSetArray,
                    infoCallback: function (settings, start, end, max, total, pre) {
                        return total + ' ' + countItem;  //Aircraft.';
                    },
                    columnDefs: [
                        { targets: [1], className: 'display_none' },
                        { orderable: false, className: 'select-checkbox', width: '10px', targets: [0] }
                    ],
                    select: { style: 'multi', selector: 'td:first-child' },
                    order: [[2, 'asc']],
                    dom: 'Bfitrp',
                    buttons: [
                        { extend: 'csv', enabled: enableExport, exportOptions: { columns: ':visible' } },
                        { extend: 'excel', enabled: enableExport, exportOptions: { columns: ':visible' } },
                        { extend: 'pdf', enabled: enableExport, orientation: 'landscape', pageSize: 'A2', exportOptions: { columns: ':visible' } },
                        { extend: 'colvis', enabled: enableExport, text: 'Columns', collectionLayout: 'fixed two-column', postfixButtons: ['colvisRestore'] },

                        {
                            text: 'Remove Selected Rows', className: 'RemoveRowsValue',
                            action: function (e, dt, node, config) {

                                //                        dt.rows({ selected: true }).remove().draw(false);
                                //                        selectAllRows(dt.rows({ selected: false }).data(), selectedRows, tableName);

                                $("#<%= filter_draw.ClientID %>").val('filter');
                                $("#<%= acKeepRemove.ClientID %>").val('remove');
                                dt.rows('.selected').nodes().to$().addClass('remove');
                                dt.rows({ selected: true }).deselect(); dt.draw();


                            }
                        },

                        {
                            text: 'Keep Selected Rows', className: 'KeepTableRow',
                            action: function (e, dt, node, config) {

                                //                       dt.draw();
                                //                       selectAllRows(dt.rows({ selected: true }).data(), selectedRows, tableName);
                                //                       dt.rows({ selected: false }).remove().draw(false);
                                //                       dt.rows('.selected').deselect();
                                $("#<%= filter_draw.ClientID %>").val('filter');
                                $("#<%= acKeepRemove.ClientID %>").val('keep');
                                dt.rows('.selected').nodes().to$().addClass('keep');
                                dt.rows({ selected: true }).deselect();
                                dt.draw();

                            }
                        },

                        {
                            text: 'Reload Table', className: 'RefreshTableValue',
                            action: function (e, dt, node, config) {

                                //$("#" + selectedRows).val('');
                                //ChangeTheMouseCursorOnItemParentDocument('cursor_wait');
                                $("#<%= filter_draw.ClientID %>").val('filter');
                                $("#<%= acKeepRemove.ClientID %>").val('remove');
                                dt.rows().nodes().to$().removeClass('gone');
                                dt.rows('.selected').deselect(); dt.draw();

                            }
                        }
                    ]
                });
            }

            //$(".RefreshTableValue").addClass('display_none');

            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            $($.fn.dataTable.tables(true)).DataTable().scroller.measure();

        };


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart', 'table'] });
    </script>
    <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width='98%' runat="server"
        class="DetailsBrowseTable">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                                <div class="backgroundShade">
                                    <span class="float_right">
                                         <a class="underline" href="/help/documents/856.pdf" target="_blank">
                                            <img src="/images/help-circle.svg" class="float_left" border="0" alt="Show View Help"
                                                title="Show View Help" style="padding-bottom: 2px;" />
                                        </a>
                                            <a href="#" onclick="javascript:window.close();" class="gray_button float_right seperator noBefore"><img src="images/x.svg" alt="Close" /></a>
                                        </span>
                                    </div>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:DropDownList ID="acKeepRemove" runat="server" CssClass="float_right display_none"
        Width="100%">
        <asp:ListItem Value="keep">keep</asp:ListItem>
        <asp:ListItem Selected="True" Value="remove">remove</asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList ID="filter_draw" runat="server" CssClass="float_right display_none"
        Width="100%">
        <asp:ListItem Value="filter">filter</asp:ListItem>
        <asp:ListItem Selected="True" Value="">no filter</asp:ListItem>
    </asp:DropDownList>
    <asp:Panel ID="contentClass" runat="server" Width="100%" HorizontalAlign="Center"
        CssClass="valueViewPDFExport remove_padding" style="margin-top:15px !important">
        <div id="searchPanelContainerDiv" runat="server" class="center_outer_div" width="1050">
            <asp:Panel ID="portfolio_view_search" runat="server" HorizontalAlign="Left" Width="100%">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td align="left" valign="bottom" class="dark_header">
                            <table width="100%" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td align="left" valign="bottom" width="12%">
                                        <asp:Panel ID="Control_Panel1" runat="server">
                                            <asp:Image ID="ControlImage1" runat="server" ImageUrl="../images/search_expand.jpg" />

                                        </asp:Panel>
                                    </td>
                                    <td align="left" valign="bottom" style="padding-bottom: 10px;" width="460">
                                        <asp:Label ID="breadcrumbs1" runat="server" CssClass="float_left criteria_text"></asp:Label>
                                    </td>
                                    <td align="left" valign="bottom" style="padding-bottom: 10px;" width="310">
                                        <asp:Label ID="buttons1" runat="server" CssClass="float_right criteria_text"></asp:Label><asp:CheckBox ID="Show_Notes" runat="server" Text="Show Notes In Listings" CssClass="float_right" onclick="runShowNotes();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <cc1:CollapsiblePanelExtender ID="PanelCollapseEx1" runat="server" TargetControlID="Collapse_Panel1"
                    Collapsed="true" ExpandControlID="Control_Panel1" ImageControlID="ControlImage1"
                    ExpandedImage="../images/search_collapse.jpg" CollapsedImage="../images/search_expand.jpg"
                    CollapseControlID="Control_Panel1" Enabled="True" CollapsedText="New Search" ExpandedText="Hide Search">
                </cc1:CollapsiblePanelExtender>
                <div id="atAGlanceCriteriaDivID" class="valueSpec portfolioManager Simplistic aircraftSpec remove_padding">
                    <asp:Panel ID="Collapse_Panel1" runat="server" Height="0px" Width="100%" CssClass="collapse">
                        <asp:Table ID="Table1" runat="server" Width="100%" CellPadding="3" CellSpacing="0"
                            CssClass="removeBorderSpacing" BorderWidth="0">
                            <asp:TableRow>
                                <asp:TableCell ID="cellUserPortfolio" HorizontalAlign="Left" VerticalAlign="top"
                                    ColumnSpan="4">
                                    <div class="Box">
                                        <span class="float_right"><span class="float_right">Hide Shared Folders?</span><input type="checkbox" id="hideSharedCheck" runat="server" class="float_right" onclick="var sharedFlag = ''; if (this.checked) { sharedFlag = '?shared=true' }; window.location.href = '/userPortfolio.aspx' + sharedFlag;" /></span><br clear="all" />
                                        <asp:Label ID="user_portfolio_lbl" runat="server" Text="Portfolio" CssClass="rowAdjustedHeight"></asp:Label>&nbsp;
                    <asp:TextBox runat="server" ID="companyIDText" CssClass="display_none"></asp:TextBox>
                                        <asp:DropDownList ID="user_portfolio_list" runat="server" CssClass="display_none">
                                        </asp:DropDownList>
                                        <br clear="all" />
                                        <asp:UpdatePanel ID="searchUpdate" runat="server">
                                            <ContentTemplate>
                                                <asp:Button runat="server" ID="atGlanceGo" Text="Search" ToolTip='Click to Apply Critera'
                                                    CssClass="float_right display_none" OnClientClick="javascript:ShowLoadingMessage('DivLoadingMessage', 'Loading Aircraft', 'Searching ... Please Wait ...');return true;" />
                                                <asp:Button runat="server" ID="atGlanceClear" Text="Clear Selections" ToolTip="Click to Clear Critera"
                                                    CssClass="float_right" UseSubmitBehavior="false" Visible="false" />
                                                <asp:Label ID="company_portfolio_links" runat="server" Text="" Visible="false"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <br clear="all" />
                                        <table class=" formatTable blue large">
                                            <tr>
                                                <td align="left">
                                                    <p class="large" id="startingText" runat="server">
                                                        Click on the desired Portfolio link above to select.
                                                    </p>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:Panel>
                </div>
            </asp:Panel>
            <asp:Panel ID="portfolio_view_results" runat="server" HorizontalAlign="Left" Width="100%"
                class="valueSpec portfolioManager Simplistic aircraftSpec remove_padding">
                <asp:UpdatePanel ID="headerUpdate" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h2 runat="server" visible="false" id="pageTitle" class="mainHeading"></h2>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label runat="server" ID="attention" ForeColor="Red" Font-Bold="true"></asp:Label>
                <div id="portfolio_view_results_div" runat="server">
                    <asp:Panel ID="portfolio_view_top_panel" runat="server" HorizontalAlign="Left" Width="100%" CssClass="grid">
                        <asp:UpdatePanel ID="tab_0_graph_update_panel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="portfolio_tab_0_graphs" CssClass="spacingRow">
                                    <div class="grid-item five columns">
                                        <asp:Label ID="ac_composition_table" runat="server" Visible="false"></asp:Label>
                                    </div>
                                    <div class="grid-item seven columns">
                                        <asp:Label ID="ac_mfryear_bar_chart" CssClass="Box display_block" runat="server" Visible="false">
                                            <asp:Label runat="server" ID="fleetTitle" CssClass="subHeader">FLEET BY MFR YEAR</asp:Label>
                                            <div id="visualization1" style="text-align: center; width: 100%; height: 232px;"></div>
                                        </asp:Label>
                                    </div>

                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="tab_1_graph_update_panel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="portfolio_tab_1_graphs" runat="server" HorizontalAlign="Left" Width="100%"
                                    Visible="False">
                                    <div class="spacingRow">
                                        <div class="grid-item five columns">
                                            <asp:Label ID="value_composition_box" runat="server"></asp:Label>
                                        </div>
                                        <div class="grid-item four columns">
                                            <asp:Label ID="value_summary_box" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="tab_2_graph_update_panel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="portfolio_tab_2_graphs" Visible="false">
                                    <div runat="server" id="div_graphs_table_tabPanel2" class="sixteen columns">
                                        <div class="row spacingRow">

                                            <div class="grid-item five columns" runat="server" visible="false" id="maintenance_composition_container">
                                                <asp:Label ID="maintenance_composition_table" runat="server"></asp:Label>
                                            </div>
                                            <div class="four columns" runat="server" id="maintenance_graph_container">
                                                <asp:Label ID="maint_graph_1" runat="server">
                                                    <div class="Box">
                                                        <span class="subHeader" runat="server" id="afttTitle">AFTT SUMMARY</span><table class="alignTable">
                                                            <tr>
                                                                <td valign="top" align="left">
                                                                    <div id="visualization2" style="text-align: center; width: 100%; height: 245px;"></div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>

                                                </asp:Label>
                                            </div>

                                            <div class="four columns marginLeft2" runat="server" id="maintenance_program_summary_container">
                                                <asp:Label ID="maint_graph_3" runat="server">
                             <div class="Box"><span class="subHeader">MAINTENANCE PROGRAM SUMMARY</span><table class="alignTable">
                            <tr><td valign="top" align="left"><div id="visualization3" style="text-align:center; width:100%; height:245px;"></div></td></tr></table></div>
                     
                                                </asp:Label>
                                            </div>
                                            <div class="four columns marginLeft2" runat="server" id="engine_maintenance_program_container">
                                                <asp:Label ID="maint_graph_2" runat="server">
                         <div class="Box"><span class="subHeader">ENGINE MAINTENANCE PROGRAM</span><table  class="alignTable">
     <tr><td valign="top" align="left"><div id="visualization4" style="text-align:center; width:100%; height:245px;"></div></td></tr></table></div>

                          
                                                </asp:Label>
                                            </div>
                                        </div>

                                        <br />
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="tab_3_graph_update_panel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="portfolio_tab_3_graphs" Visible="false">
                                    <div class="spacingRow">
                                        <div class="grid-item five columns" runat="server" visible="false" id="features_fleet_composition_panel">
                                            <asp:Label ID="features_fleet_composition_label" runat="server"></asp:Label>
                                        </div>
                                        <div class="seven columns grid-item remove_margin" runat="server" id="featuresChartPanel">
                                            <div class="Box">
                                                <div class="subHeader" runat="server" id="featuresGraphSubHeader">Summary Level Features</div>
                                                <div style="height: 244px; overflow: hidden">
                                                    <div id="visualization23" style="height: 304px; margin-top: -57px;">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="seven columns grid-item remove_margin Box" runat="server" id="featuresGaugeSelectedPanel" visible="false">
                                            <div class="row remove_margin">
                                                <div class="twelve columns">
                                                    <asp:Label runat="server" ID="featuresGaugeSelectedLabel" class="subHeader emphasisColor"></asp:Label>

                                                </div>
                                            </div>
                                            <div class="row remove_margin">
                                                <div class="six columns" style="height: 180px;">
                                                    <table class="alignTable">
                                                        <tr>
                                                            <td>
                                                                <canvas id="featuresGaugeSelected"></canvas>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div class="six columns">
                                                    <asp:Label runat="server" ID="featuresGaugeSelectedCompositionLabel"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="row remove_margin featuresDescriptionLine" runat="server" id="featureGaugeSelectedDescriptionPanel" visible="false" style="max-height: 56px; overflow: auto;">
                                                <div class="twelve columns">
                                                    <asp:Literal runat="server" ID="featureGaugeSelectedDescription"></asp:Literal>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:Panel runat="server" ID="featuresGaugePanel" CssClass="gaugePanel" Visible="false">
                                        <div class="row">
                                            <div class="three columns" runat="server" id="Box1" visible="false">
                                                <div class="Box">
                                                    <asp:Label runat="server" ID="featuresText1" class="subHeader"></asp:Label>
                                                    <table class="alignTable">
                                                        <tr>
                                                            <td>
                                                                <canvas id="features1"></canvas>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="three columns" runat="server" id="Box2" visible="false">
                                                <div class="Box">
                                                    <asp:Label runat="server" ID="featuresText2" class="subHeader"></asp:Label>
                                                    <table class="alignTable">
                                                        <tr>
                                                            <td>
                                                                <canvas id="features2"></canvas>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="three columns" runat="server" id="Box3" visible="false">
                                                <div class="Box">
                                                    <asp:Label runat="server" ID="featuresText3" class="subHeader"></asp:Label>
                                                    <table class="alignTable">
                                                        <tr>
                                                            <td>
                                                                <canvas id="features3"></canvas>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="three columns" runat="server" id="Box4" visible="false">
                                                <div class="Box">
                                                    <asp:Label runat="server" ID="featuresText4" class="subHeader"></asp:Label>
                                                    <table class="alignTable">
                                                        <tr>
                                                            <td>
                                                                <canvas id="features4"></canvas>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="three columns" runat="server" id="Box5" visible="false">
                                                <div class="Box">
                                                    <asp:Label runat="server" ID="featuresText5" class="subHeader"></asp:Label>
                                                    <table class="alignTable">
                                                        <tr>
                                                            <td>
                                                                <canvas id="features5"></canvas>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="three columns" runat="server" id="Box6" visible="false">
                                                <div class="Box">
                                                    <asp:Label runat="server" ID="featuresText6" class="subHeader"></asp:Label>
                                                    <table class="alignTable">
                                                        <tr>
                                                            <td>
                                                                <canvas id="features6"></canvas>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="three columns" runat="server" id="Box7" visible="false">
                                                <div class="Box">
                                                    <asp:Label runat="server" ID="featuresText7" class="subHeader"></asp:Label>
                                                    <table class="alignTable">
                                                        <tr>
                                                            <td>
                                                                <canvas id="features7"></canvas>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="three columns" runat="server" id="Box8" visible="false">
                                                <div class="Box">
                                                    <asp:Label runat="server" ID="featuresText8" class="subHeader"></asp:Label>
                                                    <table class="alignTable">
                                                        <tr>
                                                            <td>
                                                                <canvas id="features8"></canvas>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="tab_4_graph_update_panel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="portfolio_tab_4_graphs" Visible="false">
                                    <div class="spacingRow">
                                        <div class="seven columns grid-item">
                                            <div class="Box">
                                                <div id="utilizationViewGraphall">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="five columns  grid-item">
                                            <div class="Box">
                                                <span class="subHeader">FLIGHT SUMMARY <strong>LAST 12 MONTHS</strong></span>
                                                <asp:Label ID="flight_activity_summary" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="tab_5_graph_update_panel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="portfolio_tab_5_graphs" Visible="false">
                                    <div class="spacingRow">
                                        <div class="grid-item five columns">
                                            <asp:Label ID="operator_fleet_comp" runat="server" Visible="false"></asp:Label>
                                        </div>
                                        <div class="four columns grid-item">
                                            <div class="Box">
                                                <span class="subHeader">Primary Business Type</span>
                                                <div id="operator_business_chart" style="width: 100%; height: 200px;">
                                                </div>
                                            </div>
                                        </div>
                                        <div class=" three columns grid-item" style="width: 25% !important;">
                                            <div class="Box">
                                                <span class="subHeader">Operators by Continent</span>
                                                <div id="operator_continent_chart" style="width: 100%; height: 200px;">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="tab_6_graph_update_panel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="portfolio_tab_6_graphs" Visible="false">
                                    <div class="spacingRow">
                                        <div class="grid-item five columns">
                                            <asp:Label ID="owners_fleet_composition" runat="server" Visible="false"></asp:Label>
                                        </div>
                                        <div class="four columns grid-item">
                                            <div class="Box">
                                                <span class="subHeader">Types of Ownership</span>
                                                <div id="ownership_pie_chart" style="width: 100%; height: 200px;">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="three columns grid-item" style="width: 25% !important;">
                                            <div class="Box">
                                                <span class="subHeader">Owners by Continent</span>
                                                <div id="owner_continent_chart" style="width: 100%; height: 200px;">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="tab_7_graph_update_panel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="portfolio_tab_7_graphs" Visible="false">
                                    <div class="row spacingRow">
                                        <div class="twelve columns grid-item" style="width: 97% !important;">
                                            <div class="Box">
                                                <span class="subHeader">Top Models</span>
                                                <div id="top_models_graphs">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="tab_8_graph_update_panel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="portfolio_tab_8_graphs" Visible="false">
                                    <div class="spacingRow">
                                        <div class="grid-item seven columns">
                                            <asp:Label runat="server" ID="transaction_roles_panel" Visible="true">
                                                <div class="Box">
                                                    <span class="subHeader">
                                                        <asp:Label runat="server" ID="left_graph_label" Text="Transaction Roles"></asp:Label></span>
                                                    <div id="chart_div_port_tab2_all" style="border-top: 0; width: 100%;">
                                                    </div>
                                                </div>
                                            </asp:Label>
                                        </div>
                                        <div class="grid-item five columns">
                                            <div class="Box">
                                                <span class="subHeader">Transaction Summary</span>
                                                <div id="chart_div_port_tab3_all" style="border-top: 0; width: 100%;">
                                                </div>
                                            </div>

                                            <div id="history_graphs">
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="tab_9_graph_update_panel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="portfolio_tab_9_graphs" Visible="false">
                                    <div class="spacingRow">
                                        <div class="grid-item five columns">
                                            <asp:Label ID="location_composition" runat="server"></asp:Label>
                                        </div>
                                        <asp:Label runat="server" ID="location_panel" Visible="true" CssClass="grid-item seven columns remove_margin">
                                            <div class="Box">
                                                <span class="subHeader">
                                                    <asp:Label runat="server" ID="left_graph_label99" Text="Country Summary"></asp:Label></span>
                                                <div id="visualization13" style="width: 100%; border-top: 0; height: 300px">
                                                </div>
                                            </div>
                                        </asp:Label>

                                        <div id="location_graphs">
                                        </div>

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <br />
                    <cc1:TabContainer ID="portfolio_tabContainer" runat="server" CssClass="dark-theme"
                        Width="100%" Style="margin-left: auto; margin-right: auto;" ActiveTabIndex="0"
                        OnClientActiveTabChanged="ActiveTabChanged">
                        <cc1:TabPanel ID="portfolio_tabPanel0" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="portfolio_tabPanel0_Label1" runat="server" Text="Fleet"></asp:Label>

                            </HeaderTemplate>

                            <ContentTemplate>
                                <asp:UpdatePanel ID="tab_0_update_panel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        Summarize By:<asp:DropDownList runat="server" ID="fleet_dropdown" AutoPostBack="true" onchange="ChangeTheMouseCursorOnItemParentDocument('standalone_page cursor_wait');"></asp:DropDownList>

                                        <asp:TextBox runat="server" ID="ranTab0" Text="false" CssClass="display_none"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="selected_aircraft_rows_tabPanel0" CssClass="display_none"></asp:TextBox>
                                        <div runat="server" id="div_aircraft_results_table_tabPanel0" class="sixteen columns removeLeftMargin">
                                            <div style="text-align: center; width: 100%;" runat="server" id="acSearchResultsDiv_tabPanel0">
                                                <asp:Label ID="acSearchResultsTable_tabPanel0" runat="server" Visible="false"></asp:Label>
                                                <table id="tab0_DataTable" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                                <table id="tab0_DataTable_Summary" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                                <div id="tab0_InnerTable" align="left" valign="middle" style="max-height: 510px; overflow: auto;"></div>
                                            </div>
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </ContentTemplate>

                        </cc1:TabPanel>
                        <cc1:TabPanel ID="portfolio_tabPanel1" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="portfolio_tabPanel1_Label1" runat="server" Text="For Sale"></asp:Label>
                            </HeaderTemplate>

                            <ContentTemplate>
                                <asp:UpdatePanel ID="tab_1_update_panel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>

                                        <asp:TextBox runat="server" ID="ranTab1" Text="false" CssClass="display_none"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="selected_aircraft_rows_tabPanel1" CssClass="display_none"></asp:TextBox>
                                        <div runat="server" id="div_aircraft_results_table_tabPanel1" class="sixteen columns removeLeftMargin">
                                            <div style="text-align: center; width: 100%;" runat="server" id="acSearchResultsDiv_tabPanel1">
                                                <asp:Label ID="acSearchResultsTable_tabPanel1" runat="server" Visible="false"></asp:Label>
                                                <table id="tab1_DataTable" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                                <div id="tab1_InnerTable" align="left" valign="middle" style="max-height: 510px; overflow: auto;"></div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </ContentTemplate>

                        </cc1:TabPanel>
                        <cc1:TabPanel ID="portfolio_tabPanel2" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="portfolio_tabPanel2_Label1" runat="server" Text="Maintenance"></asp:Label>
                            </HeaderTemplate>


                            <ContentTemplate>
                                <asp:UpdatePanel ID="tab_2_update_panel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        Summarize By:<asp:DropDownList runat="server" ID="equip_dropdown" AutoPostBack="true" onchange="ChangeTheMouseCursorOnItemParentDocument('standalone_page cursor_wait');"></asp:DropDownList>

                                        <asp:TextBox runat="server" ID="ranTab2" Text="false" CssClass="display_none"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="selected_aircraft_rows_tabPanel2" CssClass="display_none"></asp:TextBox>
                                        <div runat="server" id="div_aircraft_results_table_tabPanel2" class="sixteen columns removeLeftMargin">
                                            <div style="text-align: center; width: 100%;" runat="server" id="acSearchResultsDiv_tabPanel2">
                                                <asp:Label ID="acSearchResultsTable_tabPanel2" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="acSearchResultsTable_tabPanel22" runat="server" Visible="false"></asp:Label>
                                                <table id="tab2_DataTable" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                                <table id="tab2_DataTable_Summary" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                                <div id="tab2_InnerTable" align="left" valign="middle" style="max-height: 510px; overflow: auto;"></div>
                                            </div>
                                        </div>



                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </ContentTemplate>

                        </cc1:TabPanel>
                        <cc1:TabPanel ID="portfolio_tabPanel3" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="portfolio_tabPanel3_Label1" runat="server" Text="Features"></asp:Label>

                            </HeaderTemplate>

                            <ContentTemplate>
                                <asp:UpdatePanel ID="tab_3_update_panel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        Summarize By:
                                        <asp:DropDownList runat="server" ID="features_dropdown" AutoPostBack="true" onchange="clearRan3();ChangeTheMouseCursorOnItemParentDocument('standalone_page cursor_wait');">
                                            <asp:ListItem Value="0">All</asp:ListItem>
                                            <asp:ListItem Value="-1">Feature Profile</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Button runat="server" ID="features_dropdownButton" CssClass="display_none" OnClientClick="clearRan3();ChangeTheMouseCursorOnItemParentDocument('standalone_page cursor_wait');" Text="Submit Dropdown" />
                                        <p class="tiny_text display_inline">Select a feature from the drop down to see all aircraft with the feature.</p>
                                        <asp:TextBox runat="server" ID="ranTab3" Text="false" CssClass="display_none"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="selected_aircraft_rows_tabPanel3" CssClass="display_none"></asp:TextBox>
                                        <div runat="server" id="div_aircraft_results_table_tabPanel3" class="sixteen columns removeLeftMargin">
                                            <div style="text-align: center; width: 100%;" runat="server" id="acSearchResultsDiv_tabPanel3">
                                                <asp:Label ID="acSearchResultsTable_tabPanel3" runat="server" Visible="false"></asp:Label>
                                                <table id="tab3_DataTable" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                                <table id="tab3_DataTable_Summary" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                                <div id="tab3_InnerTable" align="left" valign="middle" style="max-height: 510px; overflow: auto;"></div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </ContentTemplate>

                        </cc1:TabPanel>
                        <cc1:TabPanel ID="portfolio_tabPanel4" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="portfolio_tabPanel4_Label1" runat="server" Text="Flight Activity"></asp:Label>

                            </HeaderTemplate>

                            <ContentTemplate>
                                <asp:UpdatePanel ID="tab_4_update_panel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>

                                        <asp:TextBox runat="server" ID="ranTab4" Text="false" CssClass="display_none"></asp:TextBox>

                                        <asp:TextBox runat="server" ID="selected_aircraft_rows_tabPanel4" CssClass="display_none"></asp:TextBox>
                                        <div runat="server" id="div_aircraft_results_table_tabPanel4" class="sixteen columns removeLeftMargin">
                                            <div style="text-align: center; width: 100%;" runat="server" id="acSearchResultsDiv_tabPanel4">
                                                <asp:Label ID="acSearchResultsTable_tabPanel4" runat="server" Visible="false"></asp:Label>
                                                <table id="tab4_DataTable" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                                <div id="tab4_InnerTable" align="left" valign="middle" style="max-height: 510px; overflow: auto;"></div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </ContentTemplate>

                        </cc1:TabPanel>
                        <cc1:TabPanel ID="portfolio_tabPanel5" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="portfolio_tabPanel5_Label1" runat="server" Text="Operators"></asp:Label>

                            </HeaderTemplate>

                            <ContentTemplate>
                                <asp:UpdatePanel ID="tab_5_update_panel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>

                                        <asp:TextBox runat="server" ID="ranTab5" Text="false" CssClass="display_none"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="selected_aircraft_rows_tabPanel5" CssClass="display_none"></asp:TextBox>
                                        <div runat="server" id="div_aircraft_results_table_tabPanel5" class="sixteen columns removeLeftMargin">
                                            <div style="text-align: center; width: 100%;" runat="server" id="acSearchResultsDiv_tabPanel5">
                                                <asp:Label ID="acSearchResultsTable_tabPanel5" runat="server" Visible="false"></asp:Label>
                                                <table id="tab5_DataTable" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                                <div id="tab5_InnerTable" align="left" valign="middle" style="max-height: 510px; overflow: auto;"></div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </ContentTemplate>


                        </cc1:TabPanel>
                        <cc1:TabPanel ID="portfolio_tabPanel6" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="portfolio_tabPanel6_Label1" runat="server" Text="Owners"></asp:Label>

                            </HeaderTemplate>

                            <ContentTemplate>
                                <asp:UpdatePanel ID="tab_6_update_panel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>

                                        <asp:TextBox runat="server" ID="ranTab6" Text="false" CssClass="display_none"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="selected_aircraft_rows_tabPanel6" CssClass="display_none"></asp:TextBox>
                                        <div runat="server" id="div_aircraft_results_table_tabPanel6" class="sixteen columns removeLeftMargin">
                                            <div style="text-align: center; width: 100%;" runat="server" id="acSearchResultsDiv_tabPanel6">
                                                <asp:Label ID="acSearchResultsTable_tabPanel6" runat="server" Visible="false"></asp:Label>
                                                <table id="tab6_DataTable" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                                <div id="tab6_InnerTable" align="left" valign="middle" style="max-height: 510px; overflow: auto;"></div>
                                            </div>
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </ContentTemplate>

                        </cc1:TabPanel>
                        <cc1:TabPanel ID="portfolio_tabPanel7" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="portfolio_tabPanel7_Label1" runat="server" Text="Models"></asp:Label>
                            </HeaderTemplate>

                            <ContentTemplate>
                                <asp:UpdatePanel ID="tab_7_update_panel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>

                                        <asp:TextBox runat="server" ID="ranTab7" Text="false" CssClass="display_none"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="selected_aircraft_rows_tabPanel7" CssClass="display_none"></asp:TextBox>
                                        <div runat="server" id="div_aircraft_results_table_tabPanel7" class="sixteen columns removeLeftMargin">
                                            <div style="text-align: center; width: 100%;" runat="server" id="acSearchResultsDiv_tabPanel7">
                                                <asp:Label ID="acSearchResultsTable_tabPanel7" runat="server" Visible="false"></asp:Label>
                                                <table id="tab7_DataTable" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                                <div id="tab7_InnerTable" align="left" valign="middle" style="max-height: 510px; overflow: auto;"></div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>


                            </ContentTemplate>

                        </cc1:TabPanel>
                        <cc1:TabPanel ID="portfolio_tabPanel8" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="portfolio_tabPanel8_Label1" runat="server" Text="Recent Sales"></asp:Label>
                            </HeaderTemplate>

                            <ContentTemplate>
                                <asp:UpdatePanel ID="tab_8_update_panel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>

                                        <asp:TextBox runat="server" ID="ranTab8" Text="false" CssClass="display_none"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="selected_aircraft_rows_tabPanel8" CssClass="display_none"></asp:TextBox>
                                        <div runat="server" id="div_aircraft_results_table_tabPanel8" class="sixteen columns removeLeftMargin">
                                            <div style="text-align: center; width: 100%;" runat="server" id="acSearchResultsDiv_tabPanel8">
                                                <asp:Label ID="acSearchResultsTable_tabPanel8" runat="server" Visible="false"></asp:Label>
                                                <table id="tab8_DataTable" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                                <table id="tab8_DataTable_folder" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                                <div id="tab8_InnerTable" align="left" valign="middle" style="max-height: 510px; overflow: auto;"></div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </ContentTemplate>

                        </cc1:TabPanel>
                        <cc1:TabPanel ID="portfolio_tabPanel9" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="portfolio_tabPanel9_Label1" runat="server" Text="Location"></asp:Label>
                            </HeaderTemplate>

                            <ContentTemplate>
                                <asp:UpdatePanel ID="tab_9_update_panel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TextBox runat="server" ID="ranTab9" Text="false" CssClass="display_none"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="selected_aircraft_rows_tabPanel9" CssClass="display_none"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="selected_aircraft_rows_tabPanel92" CssClass="display_none"></asp:TextBox>
                                        Summarize By:<asp:DropDownList runat="server" ID="location_drop" AutoPostBack="true" onchange="ChangeTheMouseCursorOnItemParentDocument('standalone_page cursor_wait');"></asp:DropDownList>
                                        <asp:Label runat="server" ID="tab9_label" Text=""></asp:Label><a href="javascript:void(0);" class="float_right" onclick="javascript:load('MapItems.aspx?id=true','','scrollbars=yes,menubar=no,width=1250,height=600,resizable=yes,toolbar=no,location=no,status=no');">Map these aircraft</a>
                                        <asp:Label ID="acSearchResultsTable_tabPanel9" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="acSearchResultsTable_tabPanel92" runat="server" Visible="false"></asp:Label>
                                        <table id="tab9_DataTable" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                        <table id="tab9_DataTable_Summary" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                        <div id="tab9_InnerTable" align="left" valign="middle" style="max-height: 510px; overflow: auto;"></div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </div>
            </asp:Panel>
        </div>
        <div id="DivLoadingMessage" class="loadingScreenBox" style="display: none;">
            <span></span>
            <div class="loader">Loading...</div>
        </div>

    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

    <script type="text/javascript">


        var startWindow;

        function ShowLoadingMessage(DivTag, Title, Message) {
            //$("#" + DivTag).show();
            $("#" + DivTag).css("display", "block");
            //$("#" + DivTag).html(Message);
            //$("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 75, resizable: false });
        }

        function CloseLoadingMessage(DivTag) {
            //$("#" + DivTag).fadeOut(1000);
            $("#" + DivTag).css("display", "none");
        }
        function clearAllTextboxes() {
            $('#<%= companyIDText.ClientID %>').val('');
            $('#<%= ranTab0.ClientID %>').val('false');
            $('#<%= ranTab1.ClientID %>').val('false');
            $('#<%= ranTab2.ClientID %>').val('false');
            $('#<%= ranTab3.ClientID %>').val('false');
            $('#<%= ranTab4.ClientID %>').val('false');
            $('#<%= ranTab5.ClientID %>').val('false');
            $('#<%= ranTab6.ClientID %>').val('false');
            $('#<%= ranTab7.ClientID %>').val('false');
            $('#<%= ranTab8.ClientID %>').val('false');
            $('#<%= ranTab9.ClientID %>').val('false');
            $('#<%= Show_Notes.ClientID  %>').prop('disabled', false);
            $('#<%= features_dropdown.ClientID  %>').empty()
        }
        function hideAllTabs() {
            $('#<%= portfolio_tab_0_graphs.ClientID %>').hide();
            $('#<%= portfolio_tab_1_graphs.ClientID %>').hide();
            $('#<%= portfolio_tab_2_graphs.ClientID %>').hide();
            $('#<%= portfolio_tab_3_graphs.ClientID %>').hide();
            $('#<%= portfolio_tab_4_graphs.ClientID %>').hide();
            $('#<%= portfolio_tab_5_graphs.ClientID %>').hide();
            $('#<%= portfolio_tab_6_graphs.ClientID %>').hide();
            $('#<%= portfolio_tab_7_graphs.ClientID %>').hide();
            $('#<%= portfolio_tab_8_graphs.ClientID %>').hide();
            $('#<%= portfolio_tab_9_graphs.ClientID %>').hide();
        }

        function runShowNotes() {

            if ($("#<%=user_portfolio_list.clientID %>").val() != '0') {
                clearAllTextboxes();
                ShowLoadingMessage('DivLoadingMessage', 'Loading Notes', 'Loading Notes ... Please Wait ...');
                $('#<%= atGlanceGo.clientID %>').click();
            }

        }
        function ActiveTabChanged(sender, args) {
            var PostBackTab = true;
            //alert("switch tab");



            var nextTab = sender.get_activeTab().get_id();

            if (nextTab == '<%= portfolio_tabPanel0.ClientID %>') {
                if ($('#<%= ranTab0.clientID %>').val() == 'true') {
                    PostBackTab = false;
                    hideAllTabs();
                    $('#<%= portfolio_tab_0_graphs.ClientID %>').show();

                }
            } else if (nextTab == '<%= portfolio_tabPanel1.clientID %>') {
                if ($('#<%= ranTab1.clientID %>').val() == 'true') {
                    PostBackTab = false;
                    hideAllTabs();
                    $('#<%= portfolio_tab_1_graphs.ClientID %>').show();
                }
            } else if (nextTab == '<%= portfolio_tabPanel2.clientID %>') {
                if ($('#<%= ranTab2.clientID %>').val() == 'true') {
                    PostBackTab = false;
                    hideAllTabs();
                    $('#<%= portfolio_tab_2_graphs.ClientID %>').show();
                }
            } else if (nextTab == '<%= portfolio_tabPanel3.clientID %>') {
                if ($('#<%= ranTab3.clientID %>').val() == 'true') {
                    PostBackTab = false;
                    hideAllTabs();
                    $('#<%= portfolio_tab_3_graphs.ClientID %>').show();
                }
            } else if (nextTab == '<%= portfolio_tabPanel4.clientID %>') {
                if ($('#<%= ranTab4.clientID %>').val() == 'true') {
                    PostBackTab = false;
                    hideAllTabs();
                    $('#<%= portfolio_tab_4_graphs.ClientID %>').show();
                }
            } else if (nextTab == '<%= portfolio_tabPanel5.clientID %>') {
                if ($('#<%= ranTab5.clientID %>').val() == 'true') {
                    PostBackTab = false;
                    hideAllTabs();
                    $('#<%= portfolio_tab_5_graphs.ClientID %>').show();
                }
            } else if (nextTab == '<%= portfolio_tabPanel5.clientID %>') {
                if ($('#<%= ranTab5.clientID %>').val() == 'true') {
                    PostBackTab = false;
                    $('#<%= portfolio_tab_6_graphs.ClientID %>').show();
                }
            } else if (nextTab == '<%= portfolio_tabPanel6.clientID %>') {
                if ($('#<%= ranTab6.clientID %>').val() == 'true') {
                    PostBackTab = false;
                    hideAllTabs();
                    $('#<%= portfolio_tab_6_graphs.ClientID %>').show();
                }
            } else if (nextTab == '<%= portfolio_tabPanel7.clientID %>') {
                if ($('#<%= ranTab7.clientID %>').val() == 'true') {
                    PostBackTab = false;
                    hideAllTabs();
                    $('#<%= portfolio_tab_7_graphs.ClientID %>').show();
                }
            }
            else if (nextTab == '<%= portfolio_tabPanel8.clientID %>') {
                if ($('#<%= ranTab8.clientID %>').val() == 'true') {
                    PostBackTab = false;
                    hideAllTabs();
                    $('#<%= portfolio_tab_8_graphs.ClientID %>').show();
                }
            }
            else if (nextTab == '<%= portfolio_tabPanel9.ClientID %>') {
                if ($('#<%= ranTab9.ClientID %>').val() == 'true') {
                    PostBackTab = false;
                    hideAllTabs();
                    $('#<%= portfolio_tab_9_graphs.ClientID %>').show();
                }
            }


            if (PostBackTab == true) {
                ShowLoadingMessage('DivLoadingMessage', 'Loading Aircraft', 'Switching Tabs ... Please Wait ...');
                $('#<%= atGlanceGo.clientID %>').click();
            }



            //      if (nextTab.indexOf("finder_preferences") > 0) {
            //        swapChosenDropdowns();
            //      }

        }

        function RedrawDatatablesOnSys() {
            setTimeout(reRenderThem, 1800);
        }

        function reRenderThem() {
            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            $($.fn.dataTable.tables(true)).DataTable().scroller.measure();
            $($.fn.dataTable.tables(true)).DataTable().responsive.recalc()
        }

        function selectAllRows(data, selectedRows, tableName) {

            var IDsToUse = '';
            var count = 0;

            data.each(function (value, index) {
                if (IDsToUse.length == 0) {
                    IDsToUse = value[1];
                } else {
                    IDsToUse += ', ' + value[1];
                }
                count += 1;
            });

            $("#" + selectedRows).val(IDsToUse);

        }

        function setRowSelected(data, selectedRows, tableName) {

            if (selectedRows != '') {

                //alert("sel:" + selectedRows);

                var rowSelected = null;

                rowSelected = selectedRows.split(", ");

                data.each(function (value, index) {

                    for (var i = 0; i < rowSelected.length; i++) {

                        if (value[1] == rowSelected[i]) {

                            var row = $("#" + tableName).DataTable().row(rowSelected[i]).node();

                            //alert("idx:" + index + "row:" + row);

                            $(row).addClass("selected");

                        }

                    }

                });
            }
        }

        function CreateSearchTable(divName, tableName, jQueryTablename) {
            var countItem = 'Aircraft.';
            if (tableName == 'tab6_DataTable') {
                countItem = 'Aircraft Owners.';
            } else if (tableName == 'tab5_DataTable') {
                countItem = 'Aircraft with Operators.';
            } else if (tableName == 'tab7_DataTable') {
                countItem = 'Models.';
            }



            var selectedRows = '';

            try {

                if ($.fn.DataTable.isDataTable("#" + jQueryTablename)) {
                    $("#" + divName).empty();
                };

            }
            catch (err) {

            }

            if ($("#" + tableName).length) {


                switch (tableName) {
                    case "tab0_DataTable":
                        {
                            selectedRows = "<%= selected_aircraft_rows_tabPanel0.clientID %>";
                        }
                        break;
                    case "tab0_DataTable_Summary":
                        {
                            selectedRows = "<%= selected_aircraft_rows_tabPanel0.clientID %>";
                        }
                        break;
                    case "tab1_DataTable":
                        {
                            selectedRows = "<%= selected_aircraft_rows_tabPanel1.clientID %>";
                        }
                        break;
                    case "tab2_DataTable":
                        {
                            selectedRows = "<%= selected_aircraft_rows_tabPanel2.clientID %>";
                        }
                        break;
                    case "tab2_DataTable_Summary":
                        {
                            selectedRows = "<%= selected_aircraft_rows_tabPanel2.clientID %>";
                        }
                        break;
                    case "tab3_DataTable":
                        {
                            selectedRows = "<%= selected_aircraft_rows_tabPanel3.clientID %>";
                        }
                        break;
                    case "tab4_DataTable":
                        {
                            selectedRows = "<%= selected_aircraft_rows_tabPanel4.clientID %>";
                        }
                        break;
                    case "tab5_DataTable":
                        {
                            selectedRows = "<%= selected_aircraft_rows_tabPanel5.clientID %>";
                        }
                        break;
                    case "tab6_DataTable":
                        {
                            selectedRows = "<%= selected_aircraft_rows_tabPanel6.clientID %>";
                        }
                        break;
                }

                //jQuery("#" + tableName).css('display', 'block');

                var clone = jQuery("#" + tableName).clone(true);

                jQuery("#" + tableName).css('display', 'none');
                clone[0].setAttribute('id', jQueryTablename);
                clone.appendTo("#" + divName);

                var table = $("#" + jQueryTablename).DataTable({
                    destroy: true,
                    language: { "search": "Filter:" },
                    fixedHeader: true,
                    "initComplete": function (settings, json) {
                        setTimeout(function () {
                            $("#" + jQueryTablename).DataTable().columns.adjust();
                            $("#" + jQueryTablename).DataTable().scroller.measure();

                            var dataRows = $("#" + jQueryTablename).DataTable().rows();
                            selectAllRows(dataRows.data(), selectedRows, tableName);

                        }, 1200)
                    },
                    scrollCollapse: true,
                    scroller: true,
                    deferRender: true,
                    stateSave: true,
                    paging: true,
                    processing: true,
                    autoWidth: true,
                    scrollY: 390,
                    scrollX: 960,
                    pageLength: 100,

                    infoCallback: function (settings, start, end, max, total, pre) {
                        return total + ' ' + countItem;  //Aircraft.';
                    },
                    columnDefs: [
                        { targets: [1], className: 'display_none' },
                        { orderable: false, className: 'select-checkbox', width: '10px', targets: [0] }
                    ],
                    select: { style: 'multi', selector: 'td:first-child' },
                    order: [[2, 'asc']],
                    dom: 'Bfitrp',
                    buttons: [
                        { extend: 'csv', exportOptions: { columns: ':visible' } },
                        { extend: 'excel', exportOptions: { columns: ':visible' } },
                        { extend: 'pdf', orientation: 'landscape', pageSize: 'A2', exportOptions: { columns: ':visible' } },
                        { extend: 'colvis', text: 'Columns', collectionLayout: 'fixed two-column', postfixButtons: ['colvisRestore'] },

                        {
                            text: 'Remove Selected Rows', className: 'RemoveRowsValue',
                            action: function (e, dt, node, config) {

                                dt.rows({ selected: true }).remove().draw(false);
                                selectAllRows(dt.rows({ selected: false }).data(), selectedRows, tableName);

                            }
                        },

                        {
                            text: 'Keep Selected Rows', className: 'KeepTableRow',
                            action: function (e, dt, node, config) {

                                dt.draw();
                                selectAllRows(dt.rows({ selected: true }).data(), selectedRows, tableName);
                                dt.rows({ selected: false }).remove().draw(false);
                                dt.rows('.selected').deselect();

                            }
                        },

                        {
                            text: 'Reload Table', className: 'RefreshTableValue',
                            action: function (e, dt, node, config) {

                                //$("#" + selectedRows).val('');
                                ChangeTheMouseCursorOnItemParentDocument('cursor_wait');

                            }
                        }
                    ]
                });
            }

            //$(".RefreshTableValue").addClass('display_none');
            //$(".KeepTableRow").addClass('display_none');

            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            $($.fn.dataTable.tables(true)).DataTable().scroller.measure();



        };

        function clearRan3() {
            $('#<%= ranTab3.ClientID %>').val('false');
        }

        function SelectDropDownItem(itemValue) {
            $("#<%=user_portfolio_list.clientID %>").val(itemValue);
            $find("<%=PanelCollapseEx1.clientID %>")._doClose();
            $("#<%=atGlanceGo.clientID %>").click();

        }

        $.fn.dataTable.ext.search.push(
            function (settings, data, dataIndex) {
                var checkFilter = true;
                var row = $.fn.dataTable.Api(settings).row(dataIndex).nodes();
                var FilterRows = false;
                if ($("#<%= filter_draw.ClientID %>").val() == '') {
                    FilterRows = false;
                } else if ($("#<%= filter_draw.ClientID %>").val() == 'filter') {
                    FilterRows = true;
                }

                if (FilterRows == true) {
                    var KeepRemove = $('#<%= acKeepRemove.clientID %>').val();
                    checkFilter = ($(row).hasClass('gone') ? false : true);

                    switch (KeepRemove) {
                        case "remove":
                            if ($(row).hasClass('remove')) {
                                $(row).removeClass('remove');
                                $(row).removeClass('keep');
                                $(row).addClass('gone');
                                checkFilter = false;
                            }
                            break;
                        default:
                            if ($(row).hasClass('keep')) {
                                $(row).removeClass('remove');
                                $(row).removeClass('keep');
                                $(row).removeClass('gone');
                                checkFilter = true;
                            } else {
                                $(row).removeClass('remove');
                                $(row).removeClass('keep');
                                $(row).addClass('gone');
                                checkFilter = false;
                            };
                    }



                    if (checkFilter) {
                        return true;
                    } else {
                        return false;
                    }

                } else { return true; }
            });

    </script>

</asp:Content>
