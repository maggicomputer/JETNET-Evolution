
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/controls/valueControl.ascx.vb $
'$$Author: Amanda $
'$$Date: 6/17/20 12:45p $
'$$Modtime: 6/17/20 12:45p $
'$$Revision: 30 $
'$$Workfile: valueControl.ascx.vb $

'
' ********************************************************************************

Partial Public Class valueControl
    Inherits System.Web.UI.UserControl
    Dim masterPage As Object
    Dim ModelID As Long = 0
    Dim ModelName As String = ""
    Dim aircraftID As Long = 0
    Dim FeaturesList As String = ""
    Dim TableBuild As New StringBuilder 'Sets up the Table Build Javascript so it only needs to be called in code.
    Dim TransactionTableBuild As New StringBuilder 'Sets up the Transaction Table Build Javascript so it only needs to be called in code.
    Dim CurrentTableArray As New StringBuilder 'Current Table Dataset
    Dim EvaluesTableArray As New StringBuilder 'Evalues Table Dataset
    Dim TransactionTableArray As New StringBuilder 'Transaction Table Dataset
    Dim sliderYearString As New StringBuilder 'Sets up Year Slider javascript
    Dim sliderAFTTString As New StringBuilder 'Sets up AFTT slider JS
    Dim dropdownString As New StringBuilder 'Sets up Code for dropdown toggle.
    Dim sliderDateString As New StringBuilder 'Sets up Date slider
    Dim SpecialProjectScript As New StringBuilder
    Dim ResetRemoveAircraftString As New StringBuilder 'Function to remove AC
    Dim VariantString As New StringBuilder
    Dim jqueryClickEventsString As New StringBuilder
    Dim maxYear As Integer = Year(Now())
    Dim minYear As Integer = Year(DateAdd(DateInterval.Year, -20, Now()))
    Dim minAFTT As Long = 0
    Dim maxAFTT As Long = 10000
    Dim startBaseAFTT As Long = 0
    Dim endBaseAFTT As Long = 0
    Dim startBaseYear As Integer = 0
    Dim endBaseYear As Integer = 0
    Dim displayEValues As Boolean = False
    Dim minTransDate As Date = DateAdd(DateInterval.Year, -20, Now())
    Dim market_functions As New market_model_functions
    Dim localDataLayer As New viewsDataLayer
    Dim searchCriteria As New viewSelectionCriteriaClass
    Dim EvaluesScript As New StringBuilder
    Dim EvaluesTableStr As New StringBuilder
    Dim gaugeScr As New StringBuilder




    Public Shared ac_id_array(200) As String
    Public Shared ac_asking_array(200) As String
    Public Shared ac_sold_array(200) As String
    Public Shared ac_sold_aftt_array(200) As String
    Public Shared ac_dlv_year_array(200) As String
    Public Shared array_count As Integer = 0
    Public Shared has_client_data As Boolean = False
    Public Shared client_record_found As Boolean = False
    Public Shared ac_id_array_current(200) As String
    Public Shared ac_asking_array_current(200) As String
    Public Shared ac_asking_aftt_array(200) As String
    Public Shared ac_dlv_year_array_current(200) As String
    Public Shared array_count_current As Integer = 0
    Public Shared has_current_client_data As Boolean = False
    Public Shared bad_year_ac_id As String = ""










    Private Sub BuildButtonString(ByRef ButtonsString As StringBuilder, ByVal tableName As String)
        Dim exportOptions As String = ""
        If Session.Item("isMobile") = False Then
            exportOptions = "columns: [function ( idx, data, node ) {"
            exportOptions += "var isVisible = " & tableName & ".column( idx ).visible();"
            exportOptions += "var isNotForExport = $.inArray( idx, hideFromExport" & tableName & " ) !== -1;"
            exportOptions += "return isVisible && !isNotForExport ? true : false; "
            'ExportOptions += "}"
            exportOptions += "}, 'colvis']"


            'ButtonsString.Append("buttons: [ ")
            'CSV Button:
            ButtonsString.Append("{")
            ButtonsString.Append("extend:  'csv',")
            ButtonsString.Append("exportOptions: {")
            ButtonsString.Append(exportOptions)
            ButtonsString.Append("}")
            ButtonsString.Append("}, ")
            'Excel Button
            ButtonsString.Append("{extend: 'excel', ")
            ButtonsString.Append("exportOptions: {")
            ButtonsString.Append(exportOptions)
            ButtonsString.Append("}")
            ButtonsString.Append("},")
            'PDF Button
            ButtonsString.Append(" {extend: 'pdf', orientation: 'landscape', pageSize: 'A2', ")
            ButtonsString.Append("exportOptions: {")
            ButtonsString.Append(exportOptions)
            ButtonsString.Append("}")
            ButtonsString.Append("}, ")
            'Print Button
            'ButtonsString.Append(" {extend: 'print', ")
            'ButtonsString.Append("exportOptions: {")
            'ButtonsString.Append(exportOptions)
            'ButtonsString.Append("}")
            'ButtonsString.Append("}, ")
            'Column Visibility Button
            ButtonsString.Append("{")
            ButtonsString.Append("extend: 'colvis', text: 'Columns',")
            ButtonsString.Append("collectionLayout:  'fixed two-column',")
            ButtonsString.Append("postfixButtons: [ 'colvisRestore' ]")
            ButtonsString.Append("}")
        End If
    End Sub


    Private Function BuildVintageTable() As StringBuilder
        TableBuild = New StringBuilder
        Dim ButtonsString As New StringBuilder
        BuildButtonString(ButtonsString, "vintageTable")
        'Vintage Table
        TableBuild.Append("var hideFromExportvintageTable = [];")
        TableBuild.Append("var vintageTable = $('#vintageTable').DataTable({destroy:true, dom: 'Bilrtfp',")
        If Session.Item("isMobile") = True Then
            TableBuild.Append("responsive:true, ")
        End If
        TableBuild.Append(" pageLength: 100, ")
        TableBuild.Append("buttons: [ ")
        TableBuild.Append(ButtonsString)
        'Remove Selected Button:
        TableBuild.Append(", { text:'Refresh Values', ")
        TableBuild.Append(" action: function( e, dt, node, config) {SetLoadingText('Refreshing Values By Year/Vintage');$(""body"").addClass(""loading"");$('#" & valuesByYearVintageButton.ClientID & "').click();}")
        TableBuild.Append("}")
        TableBuild.Append("] ")
        TableBuild.Append("});")

        TableBuild.Append("$('#vintageTable').on( 'draw.dt', function () {")
        TableBuild.Append("console.log( 'Vintage Table Redraw occurred at: '+new Date().getTime() );")
        TableBuild.Append("} );")
        TableBuild.Append("$(""#vintageTable_wrapper .dt-buttons .dt-button:last-child"").addClass( ""display_none"");")
        Return TableBuild

    End Function

    Private Sub BuildWeightTable()
        TableBuild = New StringBuilder
        Dim ButtonsString As New StringBuilder
        BuildButtonString(ButtonsString, "weightTable")

        'Weight Table
        TableBuild.Append("var hideFromExportweightTable = [];")
        TableBuild.Append("var weightTable = $('#weightTable').DataTable({destroy:true, dom: 'Bilrtfp', pageLength: 100,")
        If Session.Item("isMobile") = True Then
            TableBuild.Append("responsive:true, ")
        End If

        TableBuild.Append("buttons: [ ")
        TableBuild.Append(ButtonsString)
        'Remove Selected Button:
        TableBuild.Append(", { text:'Refresh Values', ")
        TableBuild.Append(" action: function( e, dt, node, config) {SetLoadingText('Refreshing Values By Weight');$(""body"").addClass(""loading"");$('#" & valuesByWeightClassButton.ClientID & "').click();}")
        TableBuild.Append("}")
        TableBuild.Append("] ")
        TableBuild.Append("});")

        TableBuild.Append("$('#weightTable').on( 'draw.dt', function () {")
        TableBuild.Append("console.log( 'Weight Table Redraw occurred at: '+new Date().getTime() );")
        TableBuild.Append("} );")
        TableBuild.Append(";$(""#weightTable_wrapper .dt-buttons .dt-button:last-child"").addClass( ""display_none"");")
    End Sub

    Private Sub BuildQuarterTable()
        TableBuild = New StringBuilder
        Dim ButtonsString As New StringBuilder
        BuildButtonString(ButtonsString, "quarterTable")
        'Quarter Table:
        TableBuild.Append("var hideFromExportquarterTable = [];")
        TableBuild.Append("var quarterTable = $('#quarterTable').DataTable({destroy:true, dom: 'Bilrtfp', pageLength: 100,  ")
        If Session.Item("isMobile") = True Then
            TableBuild.Append("responsive:true, ")
        End If

        TableBuild.Append("buttons: [ ")
        TableBuild.Append(ButtonsString)
        TableBuild.Append(", { text:'Refresh Values', ")
        TableBuild.Append(" action: function( e, dt, node, config) {SetLoadingText('Refreshing Values By Quarter');$(""body"").addClass(""loading"");$('#" & valuesByQuarterButton.ClientID & "').click();}")
        TableBuild.Append("}")
        TableBuild.Append("] ")
        TableBuild.Append("});")

        TableBuild.Append("$('#quarterTable').on( 'draw.dt', function () {")
        TableBuild.Append("console.log( 'Quarter Table Redraw occurred at: '+new Date().getTime() );")
        TableBuild.Append("} );")
        TableBuild.Append("$(""#quarterTable_wrapper .dt-buttons .dt-button:last-child"").addClass( ""display_none"");")
    End Sub

    Private Sub BuildAFTTTable()
        TableBuild = New StringBuilder
        Dim ButtonsString As New StringBuilder
        BuildButtonString(ButtonsString, "afttTable")

        'Aftt Table
        TableBuild.Append("var hideFromExportafttTable = [];")
        TableBuild.Append("var afttTable = $('#afttTable').DataTable({destroy:true, dom: 'Bilrtfp', pageLength: 100,  ")
        If Session.Item("isMobile") = True Then
            TableBuild.Append("responsive:true, ")
        End If


        TableBuild.Append("buttons: [ ")
        TableBuild.Append(ButtonsString)
        'Remove Selected Button:
        TableBuild.Append(", { text:'Refresh Values', ")
        TableBuild.Append(" action: function( e, dt, node, config) {SetLoadingText('Refreshing Values By AFTT');$(""body"").addClass(""loading"");$('#" & valuesByAFTTButton.ClientID & "').click();}")
        TableBuild.Append("}")
        TableBuild.Append("] ")
        TableBuild.Append("});")

        TableBuild.Append("$('#afttTable').on( 'draw.dt', function () {")
        TableBuild.Append("console.log( 'AFTT Table Redraw occurred at: '+new Date().getTime() );")
        TableBuild.Append("} );")
        TableBuild.Append("$(""#afttTable_wrapper .dt-buttons .dt-button:last-child"").addClass( ""display_none"");")
    End Sub
    ''' <summary>
    ''' Writes javascript to turn HTML tables into jquery datatable.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TableBuildJavascript(ByVal onlySales As Boolean)
        Dim FinalFeatureArray As Array = BuildFeatureArray()
        Dim ExportOptions As String = ""
        Dim ButtonsString As New StringBuilder
        Dim ButtonsStringSelect As New StringBuilder
        TableBuild = New StringBuilder


        BuildButtonString(ButtonsString, IIf(onlySales = False, "table", "historicalTable"))

        'Remove Selected Button:
        If Session.Item("isMobile") = False Then
            ButtonsStringSelect.Append(",{ text:'Remove Selected Rows', action: function( e, dt, node, config) {$( ""#" & acKeepRemove.ClientID & """).val('remove'); ")
            ButtonsStringSelect.Append("dt.rows('.selected').nodes().to$().addClass('remove');  ")

            ButtonsStringSelect.Append("var data = dt.rows({ selected: true } ).data();")
            ButtonsStringSelect.Append("var IDsToUse ='';")
            ButtonsStringSelect.Append("data.each(function (value, index) {")

            ButtonsStringSelect.Append(" if (IDsToUse.length > 0) {")
            ButtonsStringSelect.Append(" IDsToUse += ', '; ")
            ButtonsStringSelect.Append(" }")
            ButtonsStringSelect.Append("IDsToUse += ' ' + value.id;")
            ButtonsStringSelect.Append(" });")
            ButtonsStringSelect.Append("$(""#FOLDERIDNAME"").val(IDsToUse);dt.rows({ selected: true} ).deselect();dt.draw();")


            ButtonsStringSelect.Append("}}")
            ButtonsStringSelect.Append(",{ text:'Keep Selected Rows',  className:'keep', action: function( e, dt, node, config) {$( ""#" & acKeepRemove.ClientID & """).val('keep');") 'dt.draw();dt.rows('.selected').deselect();$( ""#" & acKeepRemove.ClientID & """).val('remove');")
            ButtonsStringSelect.Append("dt.rows('.selected').nodes().to$().addClass('keep');  ")

            ButtonsStringSelect.Append("var data = dt.rows({ selected: false} ).data();")
            'className: 'selected ' + $('#" & acKeepRemove.ClientID & "').val()
            ButtonsStringSelect.Append("var IDsToUse ='';")
            ButtonsStringSelect.Append("data.each(function (value, index) {")

            ButtonsStringSelect.Append(" if (IDsToUse.length > 0) {")
            ButtonsStringSelect.Append(" IDsToUse += ', '; ")
            ButtonsStringSelect.Append(" }")
            ButtonsStringSelect.Append("IDsToUse += ' ' + value.id;")
            ButtonsStringSelect.Append(" });")
            ButtonsStringSelect.Append("$(""#FOLDERIDNAME"").val(IDsToUse);dt.rows({ selected: true} ).deselect();")
            ButtonsStringSelect.Append("dt.draw();")
            ButtonsStringSelect.Append("}}")
            ButtonsStringSelect.Append(",{ text:'Graph Rows', action: function( e, dt, node, config) {")
            'ButtonsStringSelect.Append("var data = dt.rows({search:'applied'}).column(19).data();")
            ButtonsStringSelect.Append("var IDsToUse ='';")
            'ButtonsStringSelect.Append("data.each(function (value, index) {")
            'ButtonsStringSelect.Append(" if (IDsToUse.length > 0) {")
            'ButtonsStringSelect.Append(" IDsToUse += ', '; ")
            'ButtonsStringSelect.Append(" }")
            'ButtonsStringSelect.Append("IDsToUse += ' ' + value;")
            'ButtonsStringSelect.Append("});")

            ButtonsStringSelect.Append("var data = dt. rows( { filter: 'applied' } ).data();")
            'ButtonsStringSelect.Append("filteredRows.forEach(function(row) {")

            ' ButtonsStringSelect.Append("});")
            ButtonsStringSelect.Append("data.each(function (value, index) {")
            ButtonsStringSelect.Append(" if (IDsToUse.length > 0) {")
            ButtonsStringSelect.Append(" IDsToUse += ', '; ")
            ButtonsStringSelect.Append(" }")
            ButtonsStringSelect.Append("IDsToUse += ' ' + value.id;")
            ButtonsStringSelect.Append(" });")
            'ButtonsStringSelect.Append("IDsToUse += ' ' + value(19);")
            'ButtonsStringSelect.Append("});")
            'ButtonsStringSelect.Append("for (var itemI in filteredRows) {")
            'ButtonsStringSelect.Append("console.log(data);")
            ButtonsStringSelect.Append("$(""#" & graphWhat.ClientID & """).val('WHATWEAREREFRESHING');")
            ButtonsStringSelect.Append("$(""#" & startIDs.ClientID & """).val(IDsToUse);")
            ButtonsStringSelect.Append("$(""#BUTTONCLICKSWAP"").click();")
            'ButtonsStringSelect.Append("}")
            ButtonsStringSelect.Append("}},")
            ButtonsStringSelect.Append("{ text:'Reload Table', action: function( e, dt, node, config) {$( ""#" & acKeepRemove.ClientID & """).val('remove');")
            ButtonsStringSelect.Append("dt.rows().nodes().to$().removeClass('gone');  ")
            ButtonsStringSelect.Append("dt.rows('.selected').deselect(); dt.draw();$('#FOLDERIDNAME').val('');}")
            ButtonsStringSelect.Append("}")

            'ButtonsString.Append("]")
        End If



        'Current Table JS
        TableBuild.Append(BuildVintageTable())

        If onlySales = False Then

            TableBuild.Append("var cw = $('.valueTabs').width() - 20;")
            TableBuild.Append("$("".cwContainer"").width(cw);")

            TableBuild.Append("$(window).resize(function() {")
            TableBuild.Append("var cw = $('.valueTabs').width() - 20;")
            TableBuild.Append("$("".cwContainer"").width(cw);")
            TableBuild.Append("});")

            'TableBuild.Append("var hideFromExporttable = [0,8];")
            TableBuild.Append("var hideFromExporttable = [0];")  ' removed 8 so that it can export 

            'Adding this check to destroy a table if one already exists:
            Dim tableName As String = "startTable"
            Dim tableDataSet As String = "currentDataSet"




            TableBuild.Append("var table = $('#" & tableName & "').DataTable({destroy:true,dom: 'Bilrtfp', paging: true, pageLength: 100, ")


            TableBuild.Append("data: " & tableDataSet & ", ")

            If Session.Item("isMobile") = True Then
                TableBuild.Append(" responsive: {")
                TableBuild.Append("details: { ")
                TableBuild.Append("type:  'column', ")
                TableBuild.Append("target: -1 ")
                TableBuild.Append("} ")
                TableBuild.Append("},")

                TableBuild.Append("fixedHeader: false, ")
                TableBuild.Append("scrollY: false,")
                TableBuild.Append("scrollX: false,")
                TableBuild.Append("autoWidth:false,")
            Else
                TableBuild.Append("scrollY: 430,")
                TableBuild.Append("deferRender: true, ")
                TableBuild.Append("scrollX: cw,")
                TableBuild.Append("scroller:true,")
                TableBuild.Append("scrollCollapse:true,")
            End If

            If Session.Item("isMobile") = True Then
                TableBuild.Append("responsive:true, ")
            End If


            'If currentACIDs.Text <> "" And onlySales = False Then
            '  TableBuild.Append("""initComplete"": function(settings, json) {")
            '  TableBuild.Append("setTimeout(function(){$( "".keep"").click();console.log('here');},500)")
            '  TableBuild.Append("},")
            'End If
            ' TableBuild.Append("fixedColumns: {leftColumns:2} ,")

            TableBuild.Append("""infoCallback"": function( settings, start, end, max, total, pre ) {")

            TableBuild.Append("return total + "" Aircraft for Sale."";")


            TableBuild.Append("},")

            TableBuild.Append("""fnCreatedRow"": function( nRow, aData, iDataIndex, e ) {")


            TableBuild.Append("var eID = $('#" & currentACIDs.ClientID & "').val();")


            TableBuild.Append("if(eID === undefined) {")
            TableBuild.Append("return;")
            TableBuild.Append("}")

            TableBuild.Append("var eIDArray = eID.split(',');")

            TableBuild.Append("if(eIDArray.length > 0) {")
            TableBuild.Append("$.each(eIDArray, function(index, value) {")

            TableBuild.Append("var id = aData.id;  ")
            TableBuild.Append("value = value.trim();  ")
            TableBuild.Append("if(parseInt(id) == parseInt(value)) {")
            TableBuild.Append("$(nRow).toggleClass('selected');")
            TableBuild.Append("};")
            TableBuild.Append("});")
            TableBuild.Append("};")
            TableBuild.Append("},")
            TableBuild.Append("processing: true, ")
            TableBuild.Append("columns: [ ")
            TableBuild.Append("{ title: """ & IIf(Session.Item("isMobile"), "", "SEL") & """, width: ""20px"", data: ""check"", responsivePriority: 1}, ")
            TableBuild.Append("{ title: ""Ser #"", width: ""60px"", responsivePriority: 2, data: {")
            TableBuild.Append("_:    ""ser.0"",")
            TableBuild.Append("sort: ""ser.1"",")
            TableBuild.Append("} }, ")
            TableBuild.Append("{ title: ""Reg #"", data: ""reg"", width: ""60px"", responsivePriority: 3 }, ")
            TableBuild.Append("{ title: ""Year MFR"", width: ""50px"",className: ""text_align_right"", data:""mfr"", responsivePriority: 4 }, ")
            TableBuild.Append("{ title: ""Year DLV"", width: ""50px"", className: ""text_align_right"",data:""year"",responsivePriority: 5 }, ")
            TableBuild.Append("{ title: ""EST AFTT"", width: ""50px"", className: ""text_align_right"", data:""aftt"" }, ")
            TableBuild.Append("{ title: ""ENGINE TT"", width: ""50px"", className: ""text_align_right"", data:""ett"" }, ")
            TableBuild.Append("{ title: ""Asking ($k)"", width: ""90px"", className: ""text_align_right"", responsivePriority: 6, data: {")
            TableBuild.Append("_:    ""ask.0"",")
            TableBuild.Append("sort: ""ask.1"",")
            TableBuild.Append("} }, ")
            If displayEValues Then
                TableBuild.Append("{ title: """ & Left(Constants.eValues_Refer_Name, Constants.eValues_Refer_Name.Length - 1) & """, width: ""100px"",className: ""text_align_right " & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """, data: {")
                TableBuild.Append("_:    ""evalue.0"",")
                TableBuild.Append("sort: ""evalue.1"",")
                TableBuild.Append("} }, ")
                TableBuild.Append("{ title: ""MODEL YEAR AVG " & Left(Constants.eValues_Refer_Name, Constants.eValues_Refer_Name.Length - 1) & """, width: ""100px"",className: ""text_align_right " & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """, data: {")
                TableBuild.Append("_:    ""evaluemodel.0"",")
                TableBuild.Append("sort: ""evaluemodel.1"",")
                TableBuild.Append("} }, ")
            Else
                TableBuild.Append("{ title: ""holder"", data:   ""check""},")
                TableBuild.Append("{ title: ""holder2"", data:   ""check""},")
            End If
            TableBuild.Append("{ title: ""Last Reported<br />Sold Price ($k)"",  width: ""100px"", className: ""text_align_right"",data:""sale"" }, ")
            TableBuild.Append("{ title: ""Sold Price<br />Date"", width: ""100px"",className: ""text_align_right"", data: {")
            TableBuild.Append("_:    ""saledate.0"",")
            TableBuild.Append("sort: ""saledate.1"",")
            TableBuild.Append("} }, ")
            TableBuild.Append("{ title: ""Date Listed"", width: ""100px"",className: ""text_align_right"", data: {")
            TableBuild.Append("_:    ""listdate.0"",")
            TableBuild.Append("sort: ""listdate.1"",")
            TableBuild.Append("} }, ")

            TableBuild.Append("{ title: ""PAX"", width: ""50px"", className: ""text_align_right"", data:""PAX"" }, ")
            TableBuild.Append("{ title: ""INT YEAR"", width: ""100px"",className: ""text_align_right"", data:""INT"" }, ")
            TableBuild.Append("{ title: ""EXT YEAR"", width: ""100px"",className: ""text_align_right"", data:""EXT"" }, ")
            TableBuild.Append("{ title: ""ENGINE PROGRAM"", width: ""50px"",className: ""text_align_right"", data:""EPROG"" }, ")
            TableBuild.Append("{ title: ""ENG1 SOH"", width: ""10px"", className: ""text_align_right"", data:""ENGSOH1"" }, ")
            TableBuild.Append("{ title: ""ENG2 SOH"", width: ""10px"", className: ""text_align_right"", data:""ENGSOH2"" }, ")
            TableBuild.Append("{ title: ""AIRFRAME PROGRAM"", width: ""50px"",className: ""text_align_right"", data:""APROG"" }, ")
            TableBuild.Append("{ title: ""MAINTAINED"", width: ""100px"",className: ""text_align_right"", data:""MAINTAINED"" }, ")
            TableBuild.Append("{ title: ""OWNER"", width: ""250px"",className: ""text_align_right"", data:""OWNER"" }, ")
            TableBuild.Append("{ title: ""For Sale"",width: ""10px"",  data:""forsale"" }, ")
            TableBuild.Append("{ title: ""ID"", width: ""10px"", data: ""id"" } ")

            For CountOfFeatures = 0 To UBound(FinalFeatureArray)
                TableBuild.Append(", { title: """ & Replace(FinalFeatureArray(CountOfFeatures), "'", "") & """, data:""" & Replace(FinalFeatureArray(CountOfFeatures), "'", "").ToString & """}")
            Next


            TableBuild.Append("],")

            TableBuild.Append("""columnDefs"": [ ")
            TableBuild.Append("{")
            TableBuild.Append("""visible"": false,")
            TableBuild.Append("""name"": 'idStr',")
            TableBuild.Append("""targets"": 21")
            TableBuild.Append("}, ")
            TableBuild.Append("{")
            TableBuild.Append("""visible"": false,")
            TableBuild.Append("""targets"": 20")
            TableBuild.Append("}, ")
            If displayEValues = False Then
                TableBuild.Append("{")
                TableBuild.Append("""visible"": false,")
                TableBuild.Append("""targets"": 8")
                TableBuild.Append("}, ")
                TableBuild.Append("{")
                TableBuild.Append("""visible"": false,")
                TableBuild.Append("""targets"": 9")
                TableBuild.Append("}, ")
            End If
            ' TableBuild.Append("{")
            'TableBuild.Append("orderable: false,")
            'TableBuild.Append("""targets"": 8")
            'TableBuild.Append("}, ")
            TableBuild.Append(" {")
            TableBuild.Append("orderable: false,")
            If Session.Item("isMobile") = False Then
                TableBuild.Append("className:  'select-checkbox',")
            End If
            TableBuild.Append(" width: '10px',")
            TableBuild.Append("targets:   0")
            TableBuild.Append(" }")
            TableBuild.Append(" ],")
            TableBuild.Append("rowId:  'idStr',")

            TableBuild.Append("select: {")
            TableBuild.Append("style:    'multi',")
            TableBuild.Append("selector: 'td:first-child'")
            TableBuild.Append("}, ")

            TableBuild.Append(" buttons: [ ")

            TableBuild.Append(ButtonsString)

            TableBuild.Append(Replace(Replace(Replace(Replace(ButtonsStringSelect.ToString, "IDToReplace", "11"), "BUTTONCLICKSWAP", createStartGraphs.ClientID), "WHATWEAREREFRESHING", "1"), "FOLDERIDNAME", currentACIDs.ClientID))

            TableBuild.Append("]")


            TableBuild.Append("});")



            TableBuild.Append("$('#" & tableName & "').on( 'draw.dt', function () {")
            TableBuild.Append("console.log( '" & tableName & " Table Redraw occurred at: '+new Date().getTime() );")
            'TableBuild.Append("$('#startTable').DataTable().fixedHeader.adjust();")
            TableBuild.Append("$('#" & tableName & "').DataTable().columns.adjust();")
            TableBuild.Append("$('#" & tableName & "').DataTable().fixedColumns().relayout();")
            TableBuild.Append("} );")



        End If

        'Historical Table JS

        TransactionTableBuild.Append("var cw =  $('.valueTabs').width() - 20;")

        TransactionTableBuild.Append("$("".cwContainer"").width(cw); ")

        TransactionTableBuild.Append("$(window).resize(function() {;")
        TransactionTableBuild.Append("var cw = $('.valueTabs').width() - 20;")
        TransactionTableBuild.Append("$("".cwContainer"").width(cw);")
        TransactionTableBuild.Append("});")
        TransactionTableBuild.Append("var hideFromExporthistoricalTable = [0,9];")
        TransactionTableBuild.Append("var historicalTable = $('#transactionTable').DataTable({destroy:true, dom: 'Bilrtfp', pageLength: 100, ")
        TransactionTableBuild.Append("data: transactionDataSet, ")
        If Session.Item("isMobile") = True Then
            TransactionTableBuild.Append("autoWidth: false,")
            TransactionTableBuild.Append("fixedHeader: false, ")
            TransactionTableBuild.Append("scrollY: false,")
            TransactionTableBuild.Append("scrollX: false,")
        Else
            TransactionTableBuild.Append("autoWidth: false,")
            TransactionTableBuild.Append("scrollY: 430,")
            TransactionTableBuild.Append("deferRender: true, ")
            TransactionTableBuild.Append("scrollX: cw,")
            TransactionTableBuild.Append("scroller:true,")
        End If



        '  TransactionTableBuild.Append("fixedColumns: {leftColumns:2} ,")
        'TransactionTableBuild.Append("fixedHeader: true, ")
        'TransactionTableBuild.Append("scrollCollapse: true,")
        If Session.Item("isMobile") = True Then
            TransactionTableBuild.Append("responsive:true, ")

        End If

        If salesACIDs.Text <> "" And onlySales = True Then
            TransactionTableBuild.Append("""initComplete"": function(settings, json) {")
            TransactionTableBuild.Append("setTimeout(function(){$('#transactionTable').DataTable().draw();},500)")
            TransactionTableBuild.Append("},")
        End If

        TransactionTableBuild.Append("""infoCallback"": function( settings, start, end, max, total, pre ) {")
        TransactionTableBuild.Append("return total + "" Aircraft Sales."";")
        TransactionTableBuild.Append("},")

        TransactionTableBuild.Append("""fnCreatedRow"": function( nRow, aData, iDataIndex, e ) {")
        TransactionTableBuild.Append("var eID = $('#" & salesACIDs.ClientID & "').val();")
        TransactionTableBuild.Append("if(eID === undefined) {")
        TransactionTableBuild.Append("return;")
        TransactionTableBuild.Append("}")

        TransactionTableBuild.Append("var eIDArray = eID.split(',');")

        TransactionTableBuild.Append("if(eIDArray.length > 0) {")
        TransactionTableBuild.Append("$.each(eIDArray, function(index, value) {")

        TransactionTableBuild.Append("var id = aData.id;  ")
        TransactionTableBuild.Append("value = value.trim();  ")
        TransactionTableBuild.Append("if(parseInt(id) == parseInt(value)) {")
        TransactionTableBuild.Append("$(nRow).toggleClass('selected');")
        TransactionTableBuild.Append("};")
        TransactionTableBuild.Append("});")
        TransactionTableBuild.Append("};")
        TransactionTableBuild.Append("},")
        TransactionTableBuild.Append("processing: true,")
        TransactionTableBuild.Append("columns: [ ")
        TransactionTableBuild.Append("{ title:  """ & IIf(Session.Item("isMobile"), "", "SEL") & """,")
        'TransactionTableBuild.Append(" render:function ( data, type, item, meta )" & vbNewLine)
        'TransactionTableBuild.Append("{" & vbNewLine)
        'TransactionTableBuild.Append("if (type == 'display') { " & vbNewLine)
        'TransactionTableBuild.Append("   $('#renderCounter').text(parseInt($('#renderCounter').text())+1);" & vbNewLine)
        'TransactionTableBuild.Append("  return 1;" & vbNewLine)
        'TransactionTableBuild.Append("} " & vbNewLine)
        'TransactionTableBuild.Append(" return 1;" & vbNewLine & "},")
        TransactionTableBuild.Append(" data: ""check"", responsivePriority: 1 }, ")
        TransactionTableBuild.Append("{ title: ""Ser #"", responsivePriority: 2, data: {")
        TransactionTableBuild.Append("_:    ""ser.0"",")
        TransactionTableBuild.Append("sort: ""ser.1"",")
        TransactionTableBuild.Append("} }, ")
        TransactionTableBuild.Append("{ title: ""Reg #"", responsivePriority: 3, data: ""reg"" }, ")
        TransactionTableBuild.Append("{ title: ""Year DLV"", data: ""dlv"", responsivePriority: 4  }, ")
        TransactionTableBuild.Append("{ title: ""Date"", data: ""jdate"", responsivePriority: 5  }, ")

        TransactionTableBuild.Append("{ title: ""EST AFTT"", className: ""text_align_right"", data: ""aftt""  }, ")
        TransactionTableBuild.Append("{ title: ""ENGINE TT"", className: ""text_align_right"", data:""ett"" }, ")
        TransactionTableBuild.Append("{ title: ""Asking ($k)"", className: ""text_align_right"", data: {")
        TransactionTableBuild.Append("_:    ""ask.0"",")
        TransactionTableBuild.Append("sort: ""ask.1"",")
        TransactionTableBuild.Append("} }, ")
        TransactionTableBuild.Append("{ title: ""Sold ($k)"", className: ""text_align_center"", data: ""sale"", responsivePriority: 6  }, ")


        TransactionTableBuild.Append("{ title: ""PAX"", className: ""text_align_right"", data:""PAX"" }, ")
        TransactionTableBuild.Append("{ title: ""INT YEAR"",className: ""text_align_right"", data:""INT"" }, ")
        TransactionTableBuild.Append("{ title: ""EXT YEAR"",className: ""text_align_right"", data:""EXT"" }, ")
        TransactionTableBuild.Append("{ title: ""ENGINE PROGRAM"", width: ""50px"",className: ""text_align_right"", data:""EPROG"" }, ")
        TransactionTableBuild.Append("{ title: ""ENG1 SOH"", width: ""10px"", className: ""text_align_right"", data:""ENGSOH1"" }, ")
        TransactionTableBuild.Append("{ title: ""ENG2 SOH"", width: ""10px"", className: ""text_align_right"", data:""ENGSOH2"" }, ")
        TransactionTableBuild.Append("{ title: ""AIRFRAME PROGRAM"", width: ""50px"",className: ""text_align_right"", data:""APROG"" }, ")
        TransactionTableBuild.Append("{ title: ""MAINTAINED"",className: ""text_align_right"", data:""MAINTAINED"" }, ")
        TransactionTableBuild.Append("{ title: ""For Sale"", data: ""forsale"" }, ")
        TransactionTableBuild.Append("{ title: ""NewACFlag"", data: ""new"" }, ")
        TransactionTableBuild.Append("{ title: ""ID"", data: ""id"" }, ")
        TransactionTableBuild.Append("{ title: ""Date Listed"", data: {")
        TransactionTableBuild.Append("_:    ""listdate.0"",")
        TransactionTableBuild.Append("sort: ""listdate.1"",")
        TransactionTableBuild.Append("} }, ")

        TransactionTableBuild.Append("{ title: ""Transaction Info"", data: ""info""  } ")
        For CountOfFeatures = 0 To UBound(FinalFeatureArray)
            TransactionTableBuild.Append(", { title: """ & Replace(FinalFeatureArray(CountOfFeatures), "'", "") & """, data:""" & Replace(FinalFeatureArray(CountOfFeatures), "'", "").ToString & """}")
        Next

        TransactionTableBuild.Append("],")
        TransactionTableBuild.Append("""columnDefs"": [ ")
        TransactionTableBuild.Append("{")
        TransactionTableBuild.Append("""visible"": false,")
        TransactionTableBuild.Append("""name"": 'idStr',")
        TransactionTableBuild.Append("""targets"": 17")
        TransactionTableBuild.Append("}, ")
        TransactionTableBuild.Append("{")
        TransactionTableBuild.Append("""visible"": false,")
        TransactionTableBuild.Append("""targets"": [15, 16]")
        TransactionTableBuild.Append("}, ")

        If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = False Then
            TransactionTableBuild.Append("{")
            TransactionTableBuild.Append("orderable: false,")
            TransactionTableBuild.Append("""targets"": 8")
            TransactionTableBuild.Append("}, ")
        End If
        TransactionTableBuild.Append(" {")
        TransactionTableBuild.Append("orderable: false,")
        If Session.Item("isMobile") = False Then
            TransactionTableBuild.Append("className:  'select-checkbox',")
        End If
        TransactionTableBuild.Append(" width: '10px',")

        TransactionTableBuild.Append("targets:   0")
        TransactionTableBuild.Append(" }")
        TransactionTableBuild.Append(" ],")
        TransactionTableBuild.Append("rowId:  'idStr',")
        TransactionTableBuild.Append("select: {")
        TransactionTableBuild.Append("style:    'multi',")
        TransactionTableBuild.Append("selector: 'td:first-child'")
        TransactionTableBuild.Append("},")
        TransactionTableBuild.Append("buttons: [ ")


        TransactionTableBuild.Append(Replace(Replace(ButtonsString.ToString, "table.", "historicalTable."), "hideFromExporttable", "hideFromExporthistoricalTable"))
        TransactionTableBuild.Append(Replace(Replace(Replace(Replace(Replace(Replace(ButtonsStringSelect.ToString, "table.", "historicalTable."), "hideFromExporttable", "hideFromExporthistoricalTable"), "BUTTONCLICKSWAP", createStartTransGraphs.ClientID), "WHATWEAREREFRESHING", "2"), "FOLDERIDNAME", salesACIDs.ClientID), "className:'keep',", "className:'keepTr',"))
        TransactionTableBuild.Append("]")
        TransactionTableBuild.Append("});")


        TransactionTableBuild.Append("$('#transactionTable').on( 'draw.dt', function () {")
        TransactionTableBuild.Append("console.log( 'Trans Redraw occurred at: '+new Date().getTime() );")

        TransactionTableBuild.Append("} );")


    End Sub
    Public Function generateGauge(ByVal chartName As String, ByVal gaugeMin As Double, ByVal gaugeVal As Double, ByVal gaugeMax As Double, ByVal gaugeTitle As String, ByVal functionName As String) As StringBuilder
        Dim htmlOut As New StringBuilder
        'Dim jsScr As New StringBuilder

        gaugeScr.Append(" function initGauge_" & functionName & "() {$('#" & chartName & "').empty(); ")

        gaugeScr.Append(" var gauge = new RadialGauge({ renderTo:  '" & chartName & "',")
        gaugeScr.Append(" width: 180, height: 200, units: false,")
        gaugeScr.Append(" fontTitleSize: ""34"",")
        gaugeScr.Append(" fontTitle:""Arial"",")
        gaugeScr.Append("colorTitle:  '#4f5050',")

        gaugeScr.Append(" title: """ & FormatNumber(gaugeVal, 0).ToString & "k"", ")
        gaugeScr.Append("  startAngle: 90, SweepAngle: 180, valueBox: false, ticksAngle: 180, exactTicks: true, ")
        gaugeScr.Append("  minValue: " & gaugeMin.ToString & ",  maxValue: " & gaugeMax.ToString & ",")
        gaugeScr.Append(" majorTicks: false, minorTicks: 0,strokeTicks: false,")
        gaugeScr.Append(" colorUnits: ""#000000"",")
        gaugeScr.Append(" fontUnitsSize: ""30"",")
        gaugeScr.Append("highlights: false,animation: false,")
        gaugeScr.Append("barWidth: 25,")
        gaugeScr.Append("barProgress: true,")
        gaugeScr.Append("colorBarProgress:  '#078fd7',")
        gaugeScr.Append("needle: false,")
        gaugeScr.Append("colorBar:  '#eee',")
        gaugeScr.Append("colorStrokeTicks: '#fff',")
        gaugeScr.Append("numbersMargin: -18,")
        gaugeScr.Append("  colorPlate: ""rgba(0,0,0,0)"",") 'Make background transparent.
        gaugeScr.Append("    borderShadowWidth: 0,")
        gaugeScr.Append("    borders: false,")
        gaugeScr.Append("    value: " & gaugeVal.ToString & ",")
        gaugeScr.Append("}).draw();")


        gaugeScr.Append(" };initGauge_" & functionName & "();")


        'System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "gaugeString" & functionName, jsScr.ToString, True)


        Return htmlOut
    End Function
    ''' <summary>
    ''' Writes javascript to turn HTML tables into jquery datatable.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EvalTableBuildJavascript(ByVal evaluesTable As Boolean)
        Dim FinalFeatureArray As Array = BuildFeatureArray()
        Dim ExportOptions As String = ""
        Dim ButtonsString As New StringBuilder
        Dim ButtonsStringSelect As New StringBuilder
        TableBuild = New StringBuilder


        BuildButtonString(ButtonsString, "tableEval")

        'Remove Selected Button:
        If Session.Item("isMobile") = False Then
            ButtonsStringSelect.Append(",{ text:'Remove Selected Rows', action: function( e, dt, node, config) {$( ""#" & acKeepRemove.ClientID & """).val('remove'); ")
            ButtonsStringSelect.Append("dt.rows({ selected: true} ).nodes().to$().addClass('remove');  ")

            ButtonsStringSelect.Append("var data = dt.rows({ selected: true } ).data();")
            ButtonsStringSelect.Append("var IDsToUse ='';")
            ButtonsStringSelect.Append("data.each(function (value, index) {")

            ButtonsStringSelect.Append(" if (IDsToUse.length > 0) {")
            ButtonsStringSelect.Append(" IDsToUse += ', '; ")
            ButtonsStringSelect.Append(" }")
            ButtonsStringSelect.Append("IDsToUse += ' ' + value.id;")
            ButtonsStringSelect.Append(" });")
            ButtonsStringSelect.Append("$(""#" & evalueIDs.ClientID & """).val(IDsToUse);dt.rows({ selected: true} ).deselect();dt.draw();")


            ButtonsStringSelect.Append("}}")
            ButtonsStringSelect.Append(",{ text:'Keep Selected Rows', action: function( e, dt, node, config) {$( ""#" & acKeepRemove.ClientID & """).val('keep'); ")
            ButtonsStringSelect.Append("dt.rows({ selected: true} ).nodes().to$().addClass('keep');  ")

            ButtonsStringSelect.Append("var data = dt.rows({ selected: true } ).data();")
            ButtonsStringSelect.Append("var IDsToUse ='';")
            ButtonsStringSelect.Append("data.each(function (value, index) {")

            ButtonsStringSelect.Append(" if (IDsToUse.length > 0) {")
            ButtonsStringSelect.Append(" IDsToUse += ', '; ")
            ButtonsStringSelect.Append(" }")
            ButtonsStringSelect.Append("IDsToUse += ' ' + value.id;")
            ButtonsStringSelect.Append(" });")
            ButtonsStringSelect.Append("$(""#" & evalueIDs.ClientID & """).val(IDsToUse);dt.rows({ selected: true} ).deselect();dt.draw();")


            ButtonsStringSelect.Append("}}")
            'ButtonsStringSelect.Append(",{ text:'Keep Selected Rows',  className:'keep', action: function( e, dt, node, config) {$( ""#" & acKeepRemove.ClientID & """).val('remove');") 'dt.draw();dt.rows('.selected').deselect();$( ""#" & acKeepRemove.ClientID & """).val('remove');")
            'ButtonsStringSelect.Append("dt.rows({ selected: false} ).nodes().to$().addClass('remove');  ")

            'ButtonsStringSelect.Append("var data = dt.rows({ selected: true} ).data();")
            ''className: 'selected ' + $('#" & acKeepRemove.ClientID & "').val()
            'ButtonsStringSelect.Append("var IDsToUse ='';")
            'ButtonsStringSelect.Append("data.each(function (value, index) {")

            'ButtonsStringSelect.Append(" if (IDsToUse.length > 0) {")
            'ButtonsStringSelect.Append(" IDsToUse += ', '; ")
            'ButtonsStringSelect.Append(" }")
            'ButtonsStringSelect.Append("IDsToUse += ' ' + value.id;")
            'ButtonsStringSelect.Append(" });")
            'ButtonsStringSelect.Append("$(""#" & evalueIDs.ClientID & """).val(IDsToUse);")
            'ButtonsStringSelect.Append("dt.draw();")
            'ButtonsStringSelect.Append("}}")
            ButtonsStringSelect.Append(",{ text:'Graph Rows', action: function( e, dt, node, config) {")
            'ButtonsStringSelect.Append("var data = dt.rows({search:'applied'}).column(19).data();")
            ButtonsStringSelect.Append("var IDsToUse ='';")
            'ButtonsStringSelect.Append("data.each(function (value, index) {")
            'ButtonsStringSelect.Append(" if (IDsToUse.length > 0) {")
            'ButtonsStringSelect.Append(" IDsToUse += ', '; ")
            'ButtonsStringSelect.Append(" }")
            'ButtonsStringSelect.Append("IDsToUse += ' ' + value;")
            'ButtonsStringSelect.Append("});")

            ButtonsStringSelect.Append("var data = dt. rows( { filter: 'applied' } ).data();")
            'ButtonsStringSelect.Append("filteredRows.forEach(function(row) {")

            ' ButtonsStringSelect.Append("});")
            ButtonsStringSelect.Append("data.each(function (value, index) {")
            ButtonsStringSelect.Append(" if (IDsToUse.length > 0) {")
            ButtonsStringSelect.Append(" IDsToUse += ', '; ")
            ButtonsStringSelect.Append(" }")
            ButtonsStringSelect.Append("IDsToUse += ' ' + value.id;")
            ButtonsStringSelect.Append(" });")
            'ButtonsStringSelect.Append("IDsToUse += ' ' + value(19);")
            'ButtonsStringSelect.Append("});")
            'ButtonsStringSelect.Append("for (var itemI in filteredRows) {")
            'ButtonsStringSelect.Append("console.log(data);")
            ButtonsStringSelect.Append("$(""#" & graphWhat.ClientID & """).val('WHATWEAREREFRESHING');")
            ButtonsStringSelect.Append("$(""#" & startIDs.ClientID & """).val(IDsToUse);")
            ButtonsStringSelect.Append("$(""#BUTTONCLICKSWAP"").click();")
            'ButtonsStringSelect.Append("}")
            ButtonsStringSelect.Append("}},")
            ButtonsStringSelect.Append("{ text:'Reload Table',className:'removeEvalue', action: function( e, dt, node, config) {$( ""#" & acKeepRemove.ClientID & """).val('remove');")
            ButtonsStringSelect.Append("dt.rows().nodes().to$().removeClass('gone');  ")
            ButtonsStringSelect.Append("dt.rows('.selected').deselect(); dt.draw();$('#FOLDERIDNAME').val('');}")
            ButtonsStringSelect.Append("}")

            'ButtonsString.Append("]")
        End If

        'Current Table JS

        'EvaluesTableStr.Append("var hideFromExport = [0,8];")
        'Adding this check to destroy a table if one already exists:
        Dim tableName As String = "startTable"
        Dim tableDataSet As String = "currentDataSet"


        tableName = "evaluesTable"
        tableDataSet = "evaluesDataSet"
        EvaluesTableStr.Append("var cw = $('.valueTabs').width() - 20;")
        EvaluesTableStr.Append("$("".cwContainer"").width(cw);")

        EvaluesTableStr.Append("$(window).resize(function() {")
        EvaluesTableStr.Append("var cw = $('.valueTabs').width() - 20;")
        EvaluesTableStr.Append("$("".cwContainer"").width(cw);")
        EvaluesTableStr.Append("});")
        EvaluesTableStr.Append("var hideFromExporttableEval = [0,8];")
        EvaluesTableStr.Append("var tableEval = $('#" & tableName & "').DataTable({destroy:true,dom: 'Bilrtfp', paging: true, pageLength: 100, ")

        EvaluesTableStr.Append("data: " & tableDataSet & ", ")

        EvaluesTableStr.Append("scrollY: 430,")
        EvaluesTableStr.Append("deferRender: true, ")
        EvaluesTableStr.Append("scrollX: cw,")
        EvaluesTableStr.Append("scroller:true,")
        EvaluesTableStr.Append("scrollCollapse:true,")

        If Session.Item("isMobile") = True Then
            EvaluesTableStr.Append("responsive:true, ")
        End If


        EvaluesTableStr.Append("""infoCallback"": function( settings, start, end, max, total, pre ) {")

        EvaluesTableStr.Append("return total + "" " & Constants.eValues_Refer_Name & "."";")


        EvaluesTableStr.Append("},")
        EvaluesTableStr.Append("""fnCreatedRow"": function( nRow, aData, iDataIndex, e ) {")


        EvaluesTableStr.Append("var eID = $('#" & evalueIDs.ClientID & "').val();")


        EvaluesTableStr.Append("if(eID === undefined) {")
        EvaluesTableStr.Append("return;")
        EvaluesTableStr.Append("}")

        EvaluesTableStr.Append("var eIDArray = eID.split(',');")

        EvaluesTableStr.Append("if(eIDArray.length > 0) {")
        EvaluesTableStr.Append("$.each(eIDArray, function(index, value) {")

        EvaluesTableStr.Append("var id = aData.id;  ")
        EvaluesTableStr.Append("value = value.trim();  ")
        EvaluesTableStr.Append("if(parseInt(id) == parseInt(value)) {")
        EvaluesTableStr.Append("$(nRow).toggleClass('selected');")
        EvaluesTableStr.Append("};")
        EvaluesTableStr.Append("});")
        EvaluesTableStr.Append("};")
        EvaluesTableStr.Append("},")

        EvaluesTableStr.Append("processing: true, ")
        EvaluesTableStr.Append("columns: [ ")
        EvaluesTableStr.Append("{ title: """ & IIf(Session.Item("isMobile"), "", "SEL") & """, width: ""20px"", data: ""check"", responsivePriority: 1}, ")
        EvaluesTableStr.Append("{ title: ""Ser #"", width: ""60px"", responsivePriority: 2, data: {")
        EvaluesTableStr.Append("_:    ""ser.0"",")
        EvaluesTableStr.Append("sort: ""ser.1"",")
        EvaluesTableStr.Append("} }, ")
        EvaluesTableStr.Append("{ title: ""Reg #"", data: ""reg"", width: ""60px"", responsivePriority: 3 }, ")
        EvaluesTableStr.Append("{ title: ""Year MFR"", width: ""50px"",className: ""text_align_right"", data:""mfr"", responsivePriority: 4 }, ")
        EvaluesTableStr.Append("{ title: ""Year DLV"", width: ""50px"", className: ""text_align_right"",data:""year"",responsivePriority: 5 }, ")
        EvaluesTableStr.Append("{ title: ""AFTT"", width: ""50px"", className: ""text_align_right"", data:""aftt"" }, ")
        EvaluesTableStr.Append("{ title: ""ENGINE TT"", width: ""50px"", className: ""text_align_right"", data:""ett"" }, ")
        EvaluesTableStr.Append("{ title: ""Asking ($k)"", width: ""90px"", className: ""text_align_right"", responsivePriority: 6, data: {")
        EvaluesTableStr.Append("_:    ""ask.0"",")
        EvaluesTableStr.Append("sort: ""ask.1"",")
        EvaluesTableStr.Append("} }, ")

        EvaluesTableStr.Append("{ title: """ & Left(Constants.eValues_Refer_Name, Constants.eValues_Refer_Name.Length - 1) & """, width: ""100px"",className: ""text_align_right " & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """, data: {")
        EvaluesTableStr.Append("_:    ""evalue.0"",")
        EvaluesTableStr.Append("sort: ""evalue.1"",")
        EvaluesTableStr.Append("} }, ")
        EvaluesTableStr.Append("{ title: ""MODEL YEAR AVG " & Left(Constants.eValues_Refer_Name, Constants.eValues_Refer_Name.Length - 1) & """, width: ""100px"",className: ""text_align_right " & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """, data: {")
        EvaluesTableStr.Append("_:    ""evaluemodel.0"",")
        EvaluesTableStr.Append("sort: ""evaluemodel.1"",")
        EvaluesTableStr.Append("} }, ")

        EvaluesTableStr.Append("{ title: ""Last Reported<br />Sold Price ($k)"",  width: ""100px"", className: ""text_align_right"",data:""sale"" }, ")
        EvaluesTableStr.Append("{ title: ""Sold Price<br />Date"", width: ""100px"",className: ""text_align_right"", data: {")
        EvaluesTableStr.Append("_:    ""saledate.0"",")
        EvaluesTableStr.Append("sort: ""saledate.1"",")
        EvaluesTableStr.Append("} }, ")
        EvaluesTableStr.Append("{ title: ""Date Listed"", width: ""100px"",className: ""text_align_right"", data: {")
        EvaluesTableStr.Append("_:    ""listdate.0"",")
        EvaluesTableStr.Append("sort: ""listdate.1"",")
        EvaluesTableStr.Append("} }, ")

        EvaluesTableStr.Append("{ title: ""PAX"", width: ""50px"", className: ""text_align_right"", data:""PAX"" }, ")
        EvaluesTableStr.Append("{ title: ""INT YEAR"", width: ""100px"",className: ""text_align_right"", data:""INT"" }, ")
        EvaluesTableStr.Append("{ title: ""EXT YEAR"", width: ""100px"",className: ""text_align_right"", data:""EXT"" }, ")
        EvaluesTableStr.Append("{ title: ""ENGINE PROGRAM"", width: ""50px"",className: ""text_align_right"", data:""EPROG"" }, ")
        EvaluesTableStr.Append("{ title: ""AIRFRAME PROGRAM"", width: ""50px"",className: ""text_align_right"", data:""APROG"" }, ")
        EvaluesTableStr.Append("{ title: ""MAINTAINED"", width: ""100px"",className: ""text_align_right"", data:""MAINTAINED"" }, ")
        EvaluesTableStr.Append("{ title: ""OWNER"", width: ""250px"",className: ""text_align_right"", data:""OWNER"" }, ")
        EvaluesTableStr.Append("{ title: ""For Sale"",width: ""10px"",  data:""forsale"" }, ")
        EvaluesTableStr.Append("{ title: ""ID"", width: ""10px"", data: ""id"" } ")

        For CountOfFeatures = 0 To UBound(FinalFeatureArray)
            EvaluesTableStr.Append(", { title: """ & Replace(FinalFeatureArray(CountOfFeatures), "'", "") & """, data:""" & Replace(FinalFeatureArray(CountOfFeatures), "'", "").ToString & """}")
        Next


        EvaluesTableStr.Append("],")

        EvaluesTableStr.Append("""columnDefs"": [ ")
        EvaluesTableStr.Append("{")
        EvaluesTableStr.Append("""visible"": false,")
        EvaluesTableStr.Append("""name"": 'idStr',")
        EvaluesTableStr.Append("""targets"": 21")
        EvaluesTableStr.Append("}, ")
        EvaluesTableStr.Append("{")
        EvaluesTableStr.Append("""visible"": false,")
        EvaluesTableStr.Append("""targets"": 20")
        EvaluesTableStr.Append("}, ")
        'EvaluesTableStr.Append("{")
        'EvaluesTableStr.Append("orderable: false,")
        'EvaluesTableStr.Append("""targets"": 8")
        'EvaluesTableStr.Append("}, ")
        EvaluesTableStr.Append(" {")
        EvaluesTableStr.Append("orderable: false,")
        If Session.Item("isMobile") = False Then
            EvaluesTableStr.Append("className:  'select-checkbox',")
        End If
        EvaluesTableStr.Append(" width: '10px',")
        EvaluesTableStr.Append("targets:   0")
        EvaluesTableStr.Append(" }")
        EvaluesTableStr.Append(" ],")
        EvaluesTableStr.Append("rowId:  'idStr',")

        EvaluesTableStr.Append("select: {")
        EvaluesTableStr.Append("style:    'multi',")
        EvaluesTableStr.Append("selector: 'td:first-child'")
        EvaluesTableStr.Append("}, ")

        EvaluesTableStr.Append(" buttons: [ ")

        EvaluesTableStr.Append(ButtonsString)

        EvaluesTableStr.Append(Replace(Replace(Replace(Replace(ButtonsStringSelect.ToString, "IDToReplace", "11"), "BUTTONCLICKSWAP", createStartGraphs.ClientID), "WHATWEAREREFRESHING", "1"), "FOLDERIDNAME", evalueIDs.ClientID))

        EvaluesTableStr.Append("]")


        EvaluesTableStr.Append("});")


        EvaluesTableStr.Append("$('#" & tableName & "').on( 'draw.dt', function () {")
        EvaluesTableStr.Append("console.log( '" & tableName & " Table Redraw occurred at: '+new Date().getTime() );")
        'EvaluesTableStr.append("$('#startTable').DataTable().fixedHeader.adjust();")
        EvaluesTableStr.Append("$('#" & tableName & "').DataTable().columns.adjust();")
        EvaluesTableStr.Append("$('#" & tableName & "').DataTable().fixedColumns().relayout();")
        EvaluesTableStr.Append("} );")



    End Sub
    ''' <summary>
    ''' Function to fill model dropdown.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillDropdownModels()
        'Filling up Model Dropdownlist if the count is 0
        If modelList.Items.Count = 0 Then
            Dim TempTable As New DataTable
            TempTable = masterPage.aclsData_Temp.GetAircraft_MakeModels("", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, "")

            Dim ModelTableView As New DataView
            Dim ModelTableFinal As New DataTable

            ModelTableView = TempTable.DefaultView
            ModelTableView.Sort = "atype_name, amod_make_name, amod_model_name"

            ModelTableFinal = ModelTableView.ToTable()


            modelList.Items.Insert(0, New ListItem("", ""))
            For Each r As DataRow In ModelTableFinal.Rows
                If Not IsDBNull(r("amod_model_name")) And Not IsDBNull(r("amod_make_name")) Then
                    If Not IsDBNull(r("amod_id")) Then
                        Dim NewItem As New ListItem(r("amod_make_name").ToString & " " & r("amod_model_name").ToString, r("amod_id"))
                        NewItem.Attributes("OptionGroup") = r("atype_name")
                        modelList.Items.Add(NewItem)
                    End If
                End If
            Next
            TempTable.Dispose()


            'Let's select a default if needed:
            If ModelID > 0 Then
                modelList.SelectedValue = ModelID
            Else
                If Session.Item("localPreferences").DefaultModel > 0 Then
                    modelList.SelectedValue = Session.Item("localPreferences").DefaultModel
                Else
                    If Session.Item("localPreferences").UserBusinessFlag = True Then
                        If Session.Item("localPreferences").Tierlevel = eTierLevelTypes.TURBOS Then
                            modelList.SelectedValue = 207 '- king air b200 
                        Else 'Jets or ALL
                            modelList.SelectedValue = 272   ' challenger 300 - business jet
                        End If
                    ElseIf Session.Item("localPreferences").UserCommercialFlag = True Then
                        modelList.SelectedValue = 698 ' boeng bbj -  commercial jet 
                    ElseIf Session.Item("localPreferences").UserHelicopterFlag = True Then
                        modelList.SelectedValue = 408 ' augusta westland aw139 - helicopter 
                    End If
                End If
            End If


            'modelList.SelectedValue = IIf(ModelID > 0, ModelID, IIf(Session.Item("localPreferences").DefaultModel > 0, Session.Item("localPreferences").DefaultModel, 272))
        End If
    End Sub
    ''' <summary>
    ''' Sets up chosen dropdowns.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BuildJqueryDropdownJavascript()
        dropdownString = New StringBuilder
        dropdownString.Append("function swapChosenDropdowns() {")
        dropdownString.Append("$("".chosen-select"").chosen(""destroy"");")
        dropdownString.Append("$("".chosen-select"").chosen({ no_results_text: ""No results found."", disable_search_threshold: 10 });")
        dropdownString.Append("}")
        If Not Page.ClientScript.IsClientScriptBlockRegistered("chosenDropdowns") Then
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "chosenDropdowns", dropdownString.ToString, True)
        End If
        dropdownString = New StringBuilder
        dropdownString.Append(";swapChosenDropdowns();")
    End Sub

    ''' <summary>
    ''' This grabs/resets the slider values.
    ''' </summary>
    ''' <param name="MaxYear"></param>
    ''' <param name="MaxAFTT"></param>
    ''' <param name="minYear"></param>
    ''' <param name="minAFTT"></param>
    ''' <param name="minTransDate"></param>
    ''' <param name="UpdateStringTotals"></param>
    ''' <remarks></remarks>
    Private Sub GetSliderValues(ByRef MaxYear As Integer, ByRef MaxAFTT As Integer, ByRef minYear As Integer, ByRef minAFTT As Integer, ByRef minTransDate As Date, ByRef UpdateStringTotals As String)
        Dim SliderValuesTable As New DataTable
        Dim BaseAircraftValuesTable As New DataTable
        Dim temp_max_aftt As Long = 0

        UpdateStringTotals = ""
        SliderValuesTable = GetAircraftSliderValues(modelList.SelectedValue, clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True))
        If Not IsNothing(SliderValuesTable) Then
            If SliderValuesTable.Rows.Count > 0 Then
                If Not IsDBNull(SliderValuesTable.Rows(0).Item("MAXAFTT")) Then
                    MaxAFTT = SliderValuesTable.Rows(0).Item("MAXAFTT")
                End If

                If Not IsDBNull(SliderValuesTable.Rows(0).Item("MAXAFTT_PROJ")) Then
                    temp_max_aftt = SliderValuesTable.Rows(0).Item("MAXAFTT_PROJ")
                End If

                If MaxAFTT > 0 Then
                    'if we have a top estimated value, and its higher, then use it instead 
                    If temp_max_aftt > MaxAFTT Then
                        MaxAFTT = temp_max_aftt
                    End If

                    UpdateStringTotals += "$( ""#" & aftt_end.ClientID & """).val('" & MaxAFTT & "');"
                    UpdateStringTotals += "$( ""#" & hiddenAftt_end.ClientID & """).val('" & MaxAFTT & "');"
                End If



                ' If Not IsDBNull(SliderValuesTable.Rows(0).Item("MINAFTT")) Then
                minAFTT = 0
                UpdateStringTotals += "$( ""#" & aftt_start.ClientID & """).val('0');"
                UpdateStringTotals += "$( ""#" & hiddenAftt_start.ClientID & """).val('0');"

                'End If
                If Not IsDBNull(SliderValuesTable.Rows(0).Item("MAXYEAR")) Then
                    MaxYear = SliderValuesTable.Rows(0).Item("MAXYEAR")
                    UpdateStringTotals += "$( ""#" & year_end.ClientID & """).val('" & SliderValuesTable.Rows(0).Item("MAXYEAR") & "');"
                    UpdateStringTotals += "$( ""#" & hiddenYear_end.ClientID & """).val('" & SliderValuesTable.Rows(0).Item("MAXYEAR") & "');"
                End If

                If Not IsDBNull(SliderValuesTable.Rows(0).Item("MINYEAR")) Then
                    minYear = SliderValuesTable.Rows(0).Item("MINYEAR")
                    UpdateStringTotals += "$( ""#" & year_start.ClientID & """).val('" & SliderValuesTable.Rows(0).Item("MINYEAR") & "');"
                    UpdateStringTotals += "$( ""#" & hiddenYear_start.ClientID & """).val('" & SliderValuesTable.Rows(0).Item("MINYEAR") & "');"
                End If


                If Not IsDBNull(SliderValuesTable.Rows(0).Item("MINYEAR")) Then
                    minTransDate = "01/01/" & SliderValuesTable.Rows(0).Item("MINYEAR")

                    'UpdateStringTotals += "$( ""#" & start_date.ClientID & """).val(" & startDateString & "');"
                End If
            End If
        End If

        evaluesTextAircraft.Text = ""

        If aircraftID > 0 Then
            Dim utilization_functions As New utilization_view_functions
            utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
            utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
            utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

            Dim AircraftTableInformation As New DataTable
            Dim CheckValuesInfo As New DataTable
            Dim HolderAircraftStats As New AjaxControlToolkit.TabPanel
            BaseAircraftValuesTable = GetAircraftBaseValues(aircraftID)


            If Not IsNothing(BaseAircraftValuesTable) Then
                If BaseAircraftValuesTable.Rows.Count > 0 Then
                    If Not IsDBNull(BaseAircraftValuesTable.Rows(0).Item("ac_year_dlv")) Then

                        If minYear < (BaseAircraftValuesTable.Rows(0).Item("ac_year_dlv") - 1) Then
                            startBaseYear = BaseAircraftValuesTable.Rows(0).Item("ac_year_dlv") - 1
                        Else
                            startBaseYear = minYear
                        End If

                        If MaxYear > (startBaseYear + 2) Then
                            endBaseYear = startBaseYear + 2
                        Else
                            endBaseYear = MaxYear
                        End If
                    End If
                    If Not IsDBNull(BaseAircraftValuesTable.Rows(0).Item("ac_est_airframe_hrs")) Then
                        If BaseAircraftValuesTable.Rows(0).Item("ac_est_airframe_hrs") > 0 Then

                            If BaseAircraftValuesTable.Rows(0).Item("ac_est_airframe_hrs") > 1000 Then
                                startBaseAFTT = BaseAircraftValuesTable.Rows(0).Item("ac_est_airframe_hrs") - 1000
                            Else
                                startBaseAFTT = 0
                            End If
                            If MaxAFTT > (startBaseAFTT + 2000) Then
                                endBaseAFTT = startBaseAFTT + 2000
                            Else
                                endBaseAFTT = MaxAFTT
                            End If
                        End If


                        CheckValuesInfo = CheckGetValuesVintageTab(modelList.SelectedValue, ac_market.SelectedValue, startBaseYear, endBaseYear, startBaseAFTT, endBaseAFTT, aircraft_registration.SelectedValue, "01/01/" & Year(DateAdd(DateInterval.Year, -1, Now())), Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()), clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True))
                        If Not IsNothing(CheckValuesInfo) Then
                            If CheckValuesInfo.Rows.Count > 0 Then
                                If Not IsDBNull(CheckValuesInfo.Rows(0).Item("tcount")) Then
                                    If CheckValuesInfo.Rows(0).Item("tcount") = 0 Then
                                        endBaseAFTT = MaxAFTT
                                        startBaseAFTT = minAFTT
                                    End If
                                End If
                            End If
                        End If
                    End If

                    ac_market.SelectedValue = "All" 'When entering the view by aircraft the filtering works great but I would like to change the default setting for the market status to “All” when we enter with an aircraft id so that we get more aircraft – it is a bit too restrictive right now.
                    viewAircraft.OnClientClick = "javascript:load('DisplayAircraftDetail.aspx?acid=" & aircraftID & "','','scrollbars=yes,menubar=no,height=900,width=1090,resizable=yes,toolbar=no,location=no,status=no');return false;"



                    tabs_bottom_7.Visible = True
                    tabs_top_left_2.HeaderText = "Details"

                    tabs_top_left_1_header.Text = "My Aircraft > " & tabs_top_left_1.HeaderText
                    Dim mfrYear As Integer = 0
                    Dim passCheckbox As New CheckBox
                    passCheckbox.Checked = True

                    AircraftTableInformation = CommonAircraftFunctions.BuildReusableTable(aircraftID, 0, "", "", masterPage.aclsData_Temp, False, 0, 0)
                    If AircraftTableInformation.Rows.Count > 0 Then

                        If Not IsDBNull(AircraftTableInformation.Rows(0).Item("ac_year")) Then
                            tabs_top_left_1_header.Text += AircraftTableInformation.Rows(0).Item("ac_year")
                        End If
                        If Not IsDBNull(AircraftTableInformation.Rows(0).Item("ac_ser_nbr")) Then
                            tabs_top_left_1_header.Text += " S/N: " & AircraftTableInformation.Rows(0).Item("ac_ser_nbr")
                        End If


                        If displayEValues Then
                            If IsNumeric(AircraftTableInformation.Rows(0).Item("ac_year")) Then
                                mfrYear = AircraftTableInformation.Rows(0).Item("ac_year")
                            End If
                            Dim mfrYearString As String = ""

                            evaluesTextAircraft.Visible = True
                            Dim Current As New DataTable
                            searchCriteria.ViewCriteriaAircraftID = aircraftID

                            Current = utilization_functions.get_current_month_assett_summary(searchCriteria, mfrYear)
                            If Not IsNothing(Current) Then
                                If Current.Rows.Count > 0 Then
                                    For Each r As DataRow In Current.Rows
                                        If Not IsDBNull(r("AVGVALUE")) Then
                                            mfrYearString = "(Avg/Year: " & clsGeneral.clsGeneral.ConvertIntoThousands(r("AVGVALUE")) & ")"
                                        End If
                                    Next
                                End If
                            End If

                            Current = New DataTable
                            Current = utilization_functions.get_current_month_assett_summary(searchCriteria)
                            If Not IsNothing(Current) Then
                                If Current.Rows.Count > 0 Then
                                    For Each r As DataRow In Current.Rows
                                        If Not IsDBNull(r("AVGVALUE")) Then
                                            evaluesTextAircraft.Text += "<div class=""four columns removeLeftMargin""><label><a href=""javascript:void(0)"" class=""text_underline " & Session.Item("localUser").crmUser_Evalues_CSS & """  onclick=""javascript:load('/help/documents/809.pdf','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">" & Left(Constants.eValues_Refer_Name, Constants.eValues_Refer_Name.Length - 1) & "</a>:</label></div> <div class=""eight columns removeLeftMargin""><span class=""" & Session.Item("localUser").crmUser_Evalues_CSS & """>" & clsGeneral.clsGeneral.ConvertIntoThousands(r("AVGVALUE")) & " " & mfrYearString & "</span></div>"
                                        End If
                                    Next
                                End If
                            End If
                        End If

                        If Not IsDBNull(AircraftTableInformation.Rows(0).Item("ac_forsale_flag")) Then
                            If AircraftTableInformation.Rows(0).Item("ac_forsale_flag") = "Y" Then 'The record you're looking at is for sale
                                If Not IsDBNull(AircraftTableInformation.Rows(0).Item("ac_asking_price")) Then
                                    evaluesTextAircraft.Text += "<div class=""four columns removeLeftMargin""><label class=""green_text"">Asking Price:</label></div> <div class=""eight columns removeLeftMargin""><span class=""green_text"">" & FormatCurrency((CDbl(AircraftTableInformation.Rows(0).Item("ac_asking_price").ToString) / 1000), 0) & "k" & "</span></div>"
                                End If
                            End If
                        End If

                        aircraft_information.Text = CommonAircraftFunctions.CreateHeaderLine(AircraftTableInformation.Rows(0).Item("amod_make_name"), AircraftTableInformation.Rows(0).Item("amod_model_name"), AircraftTableInformation.Rows(0).Item("ac_ser_nbr"), "")

                        aircraft_information.Text += CommonAircraftFunctions.Build_Identification_Block("blue", False, "", "100%", "100%", 0, AircraftTableInformation, "", 0, Me.aircraftID, masterPage.aclsData_Temp, New CheckBox, passCheckbox, HttpContext.Current.Session.Item("localPreferences").AerodexFlag, 0, False, False)
                        aircraft_information.Text = Replace(aircraft_information.Text, "IDENTIFICATION", "")
                        status_information.Text = CommonAircraftFunctions.Build_Status_Block(Me.aircraftID, 0, New DataTable, AircraftTableInformation, False, HttpContext.Current.Session.Item("localPreferences").AerodexFlag, 0, "100%", "100%", "blue", "", masterPage.AclsData_Temp, New TextBox, passCheckbox, New CheckBox, New CheckBox, "", "", New TextBox, False, False, False, "")

                    End If



                    If Not IsDBNull(BaseAircraftValuesTable.Rows(0).Item("ac_picture_id")) Then
                        picture_information.Text = "<img src='" & IIf(HttpContext.Current.Session.Item("jetnetWebSiteType") <> crmWebClient.eWebSiteTypes.LOCAL, HttpContext.Current.Session.Item("jetnetFullHostName").ToString & HttpContext.Current.Session.Item("AircraftPicturesFolderVirtualPath") & "/", "https://www.jetnettest.com/pictures/aircraft/") & BaseAircraftValuesTable.Rows(0).Item("ac_id") & "-0-" & BaseAircraftValuesTable.Rows(0).Item("ac_picture_id") & ".jpg' alt='Aircraft Picture' class=""float_right pictureInfo"" />"
                    End If


                    tabs_top_left_2.Visible = True

                    ' BuildAircraftValueHistory()


                    'This is all we need the aircraft ID for. So in order to make sure it clears up, we're going to clear the session here.
                    acIDText.Text = Session.Item("searchCriteria").SearchCriteriaViewAC
                    Session.Item("searchCriteria").SearchCriteriaViewAC = 0

                End If
            End If
        End If
    End Sub


    Private Sub BuildAircraftValueHistory()
        values_label.Text = ""
        If IsNumeric(acIDText.Text) Then
            If acIDText.Text > 0 Then
                aircraftID = acIDText.Text
                Dim utilization_functions As New utilization_view_functions
                Dim graphString As String = ""
                Dim GoogleMapArray As String = ""
                utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
                utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
                utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

                Call utilization_functions.FillAssettInsightGraphs("ASKSOLD", 0, values_label.Text, tabs_bottom_7_update_panel, 17, aircraftID, 0, 450, 0, True, True, True, "", "A", "", "", "", "", "", "", "", "", "", graphString, "", False, False)
                values_label.Text = Replace(values_label.Text, " border='1' cellpadding='3' cellspacing='0' class='engine'", " border='0' cellpadding='0' cellspacing='0' class='formatTable blue large valuesTable' width=""100%""")
                values_label.Text = "<div class=""Box""><table border = '0' width='100%' class='formatTable blue large valuesTable'><tr class=""noBorder""><td align=""left"" valign=""top""><div class='subHeader'>VALUATION/HISTORY</div></td></tr><tr><td align=""left"" valign=""top""><div id=""chart_div_value_history1_all""></div><br />" & values_label.Text & "</td></tr></table></div>"

                If Trim(graphString) <> "" Then

                    DisplayFunctions.load_google_chart(tabs_bottom_7, graphString, "", "Aircraft Value ($k)", "chart_div_value_history1_all", 900, 450, "POINTS", 17, GoogleMapArray, Me.Page, tabs_bottom_7_update_panel, False, False, True, False, False, False, False, False, False, False, 0, "bottom", "", True)
                    GoogleMapArray = Replace(GoogleMapArray, "'72%'", "'82%'")
                    DisplayFunctions.load_google_chart_all(GoogleMapArray, Me.Page, tabs_bottom_7_update_panel)
                End If
            End If
        End If
    End Sub

    'Private Sub BuildAircraftValueHistory()
    '  'Dim acHistoryTable As DataTable = GetAircraftValueHistoryJetnet(aircraftID)
    '  'If Not IsNothing(acHistoryTable) Then
    '  '  If acHistoryTable.Rows.Count > 0 Then
    '  '    Dim results As String = ""
    '  '    Dim journDate As String = ""
    '  '    Dim askingDisplay As String = ""
    '  '    Dim soldDisplay As String = ""
    '  '    results = "<table width=""100%"" cellpadding=""3"" cellspacing=""0"" border=""1"">"
    '  '    results += "<thead>"
    '  '    results += "<tr>"
    '  '    results += "<th align=""left"" valign=""top"">Date</th>"
    '  '    results += "<th align=""left"" valign=""top"">Description</th>"
    '  '    results += "<th align=""left"" valign=""top"">Asking ($k)</th>"
    '  '    results += "<th align=""left"" valign=""top"">Sold ($k)</th>"
    '  '    results += "</tr>"
    '  '    results += "</thead>"
    '  '    results += "<tbody>"
    '  '    'Query = "select ac_asking_price as asking_price, 0 as take_price, ac_asking, 0 as sold_price, "
    '  '    'Query += " journ_date as date_of, journ_subject as description, 'JETNET' as Data_Source from Aircraft with (NOLOCK) "
    '  '    For Each r As DataRow In acHistoryTable.Rows
    '  '      results += "<tr>"
    '  '      results += "<td align=""left"" valign=""top"">"
    '  '      If Not IsDBNull(r("date_of")) Then
    '  '        results += Format(r("date_of"), "MM/dd/yy")
    '  '        journDate = Format(r("date_of"), "MM/dd/yyyy")
    '  '      End If
    '  '      results += "</td>"
    '  '      results += "<td align=""left"" valign=""top"">"
    '  '      If Not IsDBNull(r("description")) Then
    '  '        results += r("description").ToString
    '  '      End If
    '  '      results += "</td>"
    '  '      results += "<td align=""right"" valign=""top"">"
    '  '      If Not IsDBNull(r("asking_price")) Then
    '  '        If Not IsDBNull(r("ac_asking")) Then
    '  '          If r("ac_asking").ToString.ToUpper = "PRICE" Then
    '  '            results += "$" & FormatNumber((r("asking_price") / 1000), 0)
    '  '            askingDisplay = FormatNumber((r("asking_price") / 1000), 0)
    '  '          Else
    '  '            results += " " & r("ac_asking").ToString
    '  '            askingDisplay = "null"
    '  '          End If
    '  '        End If
    '  '      End If
    '  '      results += "</td>"
    '  '      results += "<td align=""right"" valign=""top"">"

    '  '      If Not IsDBNull(r("sold_price")) Then
    '  '        If r("sold_price") > 0 Then
    '  '          results += DisplayFunctions.TextToImage("$" & FormatNumber((r("sold_price") / 1000), 0), 10, "Arial", "43px", "", "")
    '  '          'results += FormatNumber((r("sold_price") / 1000), 0)
    '  '          soldDisplay = FormatNumber((r("sold_price") / 1000), 0)
    '  '        Else
    '  '          soldDisplay = "null"
    '  '        End If
    '  '      Else
    '  '        soldDisplay = "null"
    '  '      End If
    '  '      results += "</td>"
    '  '      results += "</tr>"

    '  '      If Not String.IsNullOrEmpty(askingDisplay) And Not String.IsNullOrEmpty(askingDisplay) Then

    '  '        If graph1 <> "" Then
    '  '          graph1 += ", "
    '  '        End If
    '  '        graph1 += "['" & journDate & "', " & Replace(askingDisplay, ",", "") & ", " & Replace(soldDisplay, ",", "") & "]"
    '  '      End If



    '  '    Next
    '  '    results += "</tbody>"
    '  '    results += "</table>"
    '  '    valueHistoryText.Text = results
    '  '    BuildValueHistoryGraphs(graph1)

    '  '  Else
    '  '    valueHistoryText.Text = "<p>Currently no price history for this aircraft</p>"
    '  '    valueHistoryGraphTextToggle.Attributes.Add("class", "display_none")
    '  '  End If
    '  'End If
    'End Sub


    ''' <summary>
    ''' Sets up the AFTT sliders.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BuildSliderAFTTJavascript()
        sliderAFTTString = New StringBuilder
        sliderAFTTString.Append("$( ""#aftt-range"" ).slider({")
        sliderAFTTString.Append("range: true,")
        sliderAFTTString.Append("step: 1000,")
        sliderAFTTString.Append("min: " & minAFTT & ",")
        sliderAFTTString.Append("max: " & maxAFTT & ",")

        sliderAFTTString.Append("values: [ " & IIf(startBaseAFTT > 0, startBaseAFTT, minAFTT) & ", " & IIf(endBaseAFTT > 0, endBaseAFTT, maxAFTT) & " ],")

        sliderAFTTString.Append("slide: function( event, ui ) {")
        sliderAFTTString.Append("$('#" & vtgRan.ClientID & "').val(""false"");")
        sliderAFTTString.Append("$( ""#" & aftt_start.ClientID & """ ).val( ui.values[ 0 ]); $( ""#" & aftt_end.ClientID & """ ).val( ui.values[ 1 ]); ")
        sliderAFTTString.Append("$('.dataTable').DataTable().draw();")
        sliderAFTTString.Append(ToggleRefreshButtonsOn())
        sliderAFTTString.Append("}")
        sliderAFTTString.Append("});")
    End Sub

    Public Function ToggleRefreshButtonsOn() As String
        Dim returnString As String = ""
        returnString = "$(""#" & valueSummaryRefreshButton.ClientID & """).css( ""display"", ""inline"" );"
        returnString += "$(""#" & refreshGraphs.ClientID & """).css( ""display"", ""inline"" );"
        returnString += "$(""#vintageTable_wrapper .dt-buttons .dt-button:last-child"").removeClass( ""display_none"");"
        returnString += "$(""#afttTable_wrapper .dt-buttons .dt-button:last-child"").removeClass( ""display_none"");"
        returnString += "$(""#weightTable_wrapper .dt-buttons .dt-button:last-child"").removeClass( ""display_none"");"
        returnString += "$(""#quarterTable_wrapper .dt-buttons .dt-button:last-child"").removeClass( ""display_none"");"
        Return returnString
    End Function
    ''' <summary>
    ''' Sets up the Date Picker Javascript.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BuildSliderDateJavascript()
        sliderDateString = New StringBuilder
        'If Page.IsPostBack Then
        '   'Else
        'sliderDateString.Append("function BuildDateSlider() {$(""#date_slider"").dateRangeSlider({ arrows: false, valueLabels: ""hide"",")
        'sliderDateString.Append("step: {")
        'sliderDateString.Append("months: 1")
        'sliderDateString.Append("},")
        'sliderDateString.Append("defaultValues: {")
        'sliderDateString.Append("min: new Date(" & Year(minTransDate) & ", " & Month(minTransDate) - 1 & ", 1),")
        'sliderDateString.Append("max: new Date(" & Year(Now()) & ", " & Month(Now()) - 1 & ", " & Day(Now()) & ")")
        'sliderDateString.Append("},")
        'sliderDateString.Append("bounds: {")
        'sliderDateString.Append("min: new Date(" & Year(minTransDate) & ", " & Month(minTransDate) - 1 & ", 1),")
        'sliderDateString.Append("max: new Date(" & Year(Now()) & ", " & Month(Now()) - 1 & ", " & Day(Now()) & ")")
        'sliderDateString.Append("}")
        'sliderDateString.Append("});}")
        'System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "dateSliderInitialize", sliderDateString.ToString, True)
        'sliderDateString = New StringBuilder
        'sliderDateString.Append("$(""#date_slider"").dateRangeSlider(""bounds"", new Date(" & Year(minTransDate) & ", " & Month(minTransDate) - 1 & ", 1), new Date(" & Year(Now()) & ", " & Month(Now()) - 1 & ", " & Day(Now()) & "));")
        'sliderDateString.Append("$(""#date_slider"").dateRangeSlider(""min"", new Date(" & Year(minTransDate) & ", " & Month(minTransDate) - 1 & ", 1));")
        'sliderDateString.Append("$(""#date_slider"").dateRangeSlider(""values"", new Date(" & Year(minTransDate) & ", " & Month(minTransDate) - 1 & ", 1), new Date(" & Year(Now()) & ", " & Month(Now()) - 1 & ", " & Day(Now()) & "));")
        sliderDateString.Append("$(""#" & end_date.ClientID & """).datepicker({ minDate: new Date(" & Year(minTransDate) & ", " & Month(minTransDate) - 1 & ", 1), maxDate: new Date(" & Year(Now()) & ", " & Month(Now()) - 1 & ", " & Day(Now()) & ") });")
        sliderDateString.Append("$(""#" & start_date.ClientID & """).datepicker({ minDate: new Date(" & Year(minTransDate) & ", " & Month(minTransDate) - 1 & ", 1), maxDate: new Date(" & Year(Now()) & ", " & Month(Now()) - 1 & ", " & Day(Now()) & ") });")
    End Sub

    ''' <summary>
    ''' Sets up all the jquery click events.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BuildJQueryClickEventsJavascript()
        jqueryClickEventsString = New StringBuilder


        jqueryClickEventsString.Append("$( ""#" & removeAircraft.ClientID & """ ).click(function() {")
        jqueryClickEventsString.Append(" resetAircraftOnPage();$(this).hide();return false;")
        jqueryClickEventsString.Append("});")

        jqueryClickEventsString.Append("$(""[id*=" & aircraft_registration.ClientID & "]"").change(function() {")
        jqueryClickEventsString.Append("$('.dataTable').DataTable().draw();")
        jqueryClickEventsString.Append("$('#" & vtgRan.ClientID & "').val(""false"");")

        jqueryClickEventsString.Append(ToggleRefreshButtonsOn())
        jqueryClickEventsString.Append("});")


        jqueryClickEventsString.Append("$(""[id*=" & ac_market.ClientID & "]"").change(function() {")
        jqueryClickEventsString.Append("$('.dataTable').DataTable().draw();")
        jqueryClickEventsString.Append("$('#" & vtgRan.ClientID & "').val(""false"");")
        jqueryClickEventsString.Append(ToggleRefreshButtonsOn())
        jqueryClickEventsString.Append("});")

        jqueryClickEventsString.Append("$(""#" & newUsed.ClientID & """).change(function() {")
        jqueryClickEventsString.Append("$('.dataTable').DataTable().draw();")
        jqueryClickEventsString.Append("$('#" & vtgRan.ClientID & "').val(""false"");")
        jqueryClickEventsString.Append(ToggleRefreshButtonsOn())
        jqueryClickEventsString.Append("});")

        jqueryClickEventsString.Append("$(""#" & salePriceDropdown.ClientID & """).change(function() {")
        jqueryClickEventsString.Append("$('.dataTable').DataTable().draw();")
        jqueryClickEventsString.Append("$('#" & vtgRan.ClientID & "').val(""false"");")
        jqueryClickEventsString.Append(ToggleRefreshButtonsOn())
        jqueryClickEventsString.Append("});")

        jqueryClickEventsString.Append("$(""#" & start_date.ClientID & ",#" & end_date.ClientID & """).change(function() {")
        jqueryClickEventsString.Append("$('.dataTable').DataTable().draw();")
        jqueryClickEventsString.Append("$('#" & vtgRan.ClientID & "').val(""false"");")
        jqueryClickEventsString.Append("var minDateFormat = moment($(""#" & start_date.ClientID & """).val());")
        jqueryClickEventsString.Append("var maxDateFormat = moment($(""#" & end_date.ClientID & """).val());")
        jqueryClickEventsString.Append(ToggleRefreshButtonsOn())
        jqueryClickEventsString.Append("$(""#" & dateValueText.ClientID & """).val(minDateFormat.format(""MM/DD/YY"") + ' - ' + maxDateFormat.format(""MM/DD/YY""));")
        jqueryClickEventsString.Append("});")

        jqueryClickEventsString.Append("$('#" & year_start.ClientID & ", #" & year_end.ClientID & ",#" & aftt_start.ClientID & ", #" & aftt_end.ClientID & "').keyup( function() {")
        jqueryClickEventsString.Append("$('.dataTable').DataTable().draw();")
        jqueryClickEventsString.Append("} );")

        'jqueryClickEventsString.Append("$( ""#" & valueSummaryRefresh.ClientID & """ ).click(function() {")
        'jqueryClickEventsString.Append("$(""body"").addClass(""loading"");$('#" & valuesByYearVintageButton.ClientID & "').click();")
        'jqueryClickEventsString.Append("});")

        'jqueryClickEventsString.Append("$(""#date_slider"").bind(""valuesChanging"", function(e, data) {")
        'jqueryClickEventsString.Append("var minDate = data.values.min;")
        'jqueryClickEventsString.Append("var maxDate = data.values.max;")

        'jqueryClickEventsString.Append("var minDateFormat = moment(minDate);")
        'jqueryClickEventsString.Append("var maxDateFormat = moment(maxDate);")
        'jqueryClickEventsString.Append("$('#" & start_date.ClientID & "').val(minDateFormat.format(""MM/DD/YYYY""));")
        'jqueryClickEventsString.Append("$('#" & end_date.ClientID & "').val(maxDateFormat.format(""MM/DD/YYYY""));")
        'jqueryClickEventsString.Append("historicalTable.draw();")
        'jqueryClickEventsString.Append("});")
    End Sub

    ''' <summary>
    ''' Sets up the javascript for the year slider.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BuildSliderYearJavascript()
        sliderYearString = New StringBuilder

        sliderYearString.Append("$( ""#slider-range"" ).slider({")
        sliderYearString.Append("range: true,")
        sliderYearString.Append("min: " & minYear & ",")
        sliderYearString.Append("max: " & maxYear & ",")
        sliderYearString.Append("values: [ " & IIf(startBaseYear > 0, startBaseYear, minYear) & ", " & IIf(endBaseYear, endBaseYear, maxYear) & " ],")

        sliderYearString.Append("slide: function( event, ui ) {")
        sliderYearString.Append("$('#" & vtgRan.ClientID & "').val(""false"");")
        sliderYearString.Append("$( ""#" & year_start.ClientID & """ ).val( ui.values[ 0 ]); $( ""#" & year_end.ClientID & """ ).val( ui.values[ 1 ]); ")
        sliderYearString.Append("$('.dataTable').DataTable().draw();")
        sliderYearString.Append(ToggleRefreshButtonsOn())
        sliderYearString.Append("}")
        sliderYearString.Append("});")
    End Sub

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'tabs_top_left_1.HeaderText = "My Aircraft"
        tabs_top_left_1_header.Text = "My Aircraft"
        tabs_top_left_2.Visible = False
        If Session.Item("isMobile") = True Then
            tabs_bottom_1.HeaderText = "Aircraft<br />&nbsp;&nbsp;&nbsp;"
            tabs_bottom_2.HeaderText = "Sales<br />&nbsp;&nbsp;&nbsp;"
            tabs_bottom_3.HeaderText = "Values<br />Yr/Vtg"
            tabs_bottom_4.HeaderText = "Values<br />History"
            tabs_bottom_5.HeaderText = "Values<br />AFTT"
            'tabs_bottom_6.HeaderText = "Values<br />Wgt Cls"
            tabs_bottom_6.Visible = False
        End If

        If clsGeneral.clsGeneral.isEValuesAvailable() Then
            Dim ToggleCookie As HttpCookie = Request.Cookies("evalues")

            If Not IsNothing(ToggleCookie) Then
                If ToggleCookie.Value = "true" Then
                    displayEValues = True
                    tabs_bottom_8.Visible = True
                    tabs_bottom_9.Visible = True
                    slide2.Visible = True
                    slide3.Visible = True
                    slide4.Visible = True
                    slide5.Visible = True
                    slide6.Visible = True
                    tabs_bottom_10.Visible = True
                    tabs_bottom_10.HeaderText = Constants.eValues_Refer_Name
                    optionalEvaluesBox.Visible = True
                End If
            Else

                Dim aCookie As New HttpCookie("evalues")
                aCookie.Value = "true"
                aCookie.Expires = DateTime.Now.AddDays(365)
                HttpContext.Current.Response.Cookies.Add(aCookie)


            End If
        End If

        evaluesTextAircraft.Visible = False

        If displayEValues Then
            avgEvaluesRow.Visible = True
            evalues1.Visible = True
            evalues2.Visible = True
            evalues3.Visible = True
            evalues4.Visible = True
            evalues5.Visible = True

        End If

        closeGraphs.OnClientClick = "$find('" & tabs_top_right_4.ClientID & "')._hide();"
        closeGraphs.OnClientClick += "var tabTop = $find(""" & tabs_top_right.ClientID & """);"
        closeGraphs.OnClientClick += "tabTop.set_activeTabIndex(0);return false;"
    End Sub

    ''' <summary>
    ''' Page load. Sets up the query string variables and grabs the model/ac from session if one has previously been passed in the query string.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Visible Then

            If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = False Or HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then
                '  Response.Redirect("/home.aspx")
                Table2.Visible = False
                close_current_folder.Text = "<br /><br /><p>You do not have the proper credentials to view the Value View.</p>"
                close_current_folder.Visible = True
            Else
                If Me.Visible Then
                    market_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                    market_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                    market_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
                    market_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
                    market_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim
                    localDataLayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                    localDataLayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                    localDataLayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
                    localDataLayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
                    localDataLayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim
                End If

                Dim SearchButtonSlideID As String = ""
                'masterPage = DirectCast(Page.Master, EvoTheme) 'Find the master page to use the objects associated with it.

                If Session.Item("isMobile") Then
                    masterPage = DirectCast(Page.Master, MobileTheme)
                    masterPage.MenuBarVisibility(True)

                Else
                    masterPage = DirectCast(Page.Master, EmptyEvoTheme)
                End If

                masterPage.SetContainerClass("container MaxWidthRemove") 'set full width page

                masterPage.setPageTitle("Values View")
                If Session.Item("searchCriteria").SearchCriteriaViewModel > 0 Then
                    ModelID = Session.Item("searchCriteria").SearchCriteriaViewModel
                End If

                If Session.Item("searchCriteria").SearchCriteriaViewAC > 0 Then
                    aircraftID = Session.Item("searchCriteria").SearchCriteriaViewAC
                End If
                masterPage.UpdateHelpLink("/help/documents/685.pdf")


                If Not IsPostBack Then
                    If IsNumeric(Trim(Request("amod_id"))) Or IsNumeric(Trim(Request("acid"))) Then
                        If IsNumeric(Trim(Request("amod_id"))) Then
                            ModelID = Trim(Request("amod_id"))
                            Session.Item("searchCriteria").SearchCriteriaViewModel = ModelID

                        End If
                        If IsNumeric(Trim(Request("acid"))) Then
                            aircraftID = Trim(Request("acid"))
                            Session.Item("searchCriteria").SearchCriteriaViewAC = aircraftID
                        End If

                        Response.Redirect("/view_template.aspx?ViewID=" & Trim(Request("ViewID")) & "&ViewName=" & Trim(Request("ViewName")))
                    Else
                        'Fill Models
                        FillDropdownModels()
                        If Page.Request.Form("project_search") = "Y" Then
                            SetUpAndRunFolder()

                            RunPageLoad()
                        Else
                            Dim script As String = "$(document).ready(function () {SetLoadingText('Loading ' + $(""#" & modelList.ClientID & " option:selected"").text() + ' Sales');document.body.className += ' ' + 'loading'; $('#" & runFirstQuery.ClientID & "').click(); });"
                            If Not Page.ClientScript.IsStartupScriptRegistered("load") Then
                                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "load", script, True)
                            End If
                        End If
                    End If
                Else

                    If Session.Item("isMobile") = True Then
                        If tabs_bottom.ActiveTabIndex = 1 Then
                            Dim script As String = "$(document).ready(function () {SetLoadingText('Loading ' + $(""#" & modelList.ClientID & " option:selected"").text() + ' Sales');document.body.className += ' ' + 'loading';$('#" & loadingTextContainer.ClientID & "').css(""display"", ""none"");});"
                            If Not Page.ClientScript.IsStartupScriptRegistered("load") Then
                                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "load", script, True)
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    'Private Sub SetUpSlider()
    '  Dim updateTotalsJS As String = ""
    '  'If Not Page.ClientScript.IsStartupScriptRegistered("bxSlider") Then

    '  System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "bxSlider", "$(document).ready(function () {setUpSlider();};", True)


    '  'End If
    'End Sub
    Public Sub FillUpVariantListbox()
        Dim VariantModelList As String() = Split(Session.Item("searchCriteria").SearchCriteriaViewVariantString, ",")
        VariantList.Items.Clear()
        VariantList.Items.Add(New ListItem("NONE", ""))

        For VariantModelListCount = 0 To UBound(VariantModelList)
            VariantList.Items.Add(New ListItem(modelList.Items.FindByValue(Trim(VariantModelList(VariantModelListCount).ToString)).Text.ToString, VariantModelList(VariantModelListCount).ToString))
        Next

    End Sub
    Public Sub SetUpAndRunFolder()
        If Page.Request.Form("project_search") = "Y" Then
            Dim folderID As Long = 0
            Dim FoldersTableData As New DataTable
            Dim cfolderData As String = ""

            If IsNumeric(Trim(Request("modelList"))) Then
                modelList.SelectedValue = Trim(Request("modelList"))
                LoadUpModelAndVariant()

                FolderInformation.Text = ""
                FolderInformation.Visible = False
                folderID = Page.Request.Form("project_id")


                If folderID <> 0 Then
                    FoldersTableData = masterPage.aclsData_Temp.GetEvolutionFolderssBySubscription(folderID, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 0, Nothing, "")
                    If Not IsNothing(FoldersTableData) Then
                        If FoldersTableData.Rows.Count > 0 Then
                            cfolderData = FoldersTableData.Rows(0).Item("cfolder_data").ToString

                            If cfolderData <> "" Then
                                'Fills up the applicable folder Information pulled from the cfolder data field
                                DisplayFunctions.FillUpFolderInformation(Table2, close_current_folder, cfolderData, FolderInformation, FoldersTableData, False, False, False, False, False, Collapse_Panel, New BulletedList, tabs_top_left_1, StaticFolderNewSearchLabel, Nothing, "")
                                DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, tabs_top_left_3)
                                DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, tabs_top_right_2)

                                startBaseAFTT = aftt_start.Text
                                endBaseAFTT = aftt_end.Text
                                startBaseYear = year_start.Text
                                endBaseYear = year_end.Text

                                'If currentACIDs.Text <> "" Or salesACIDs.Text <> "" Then
                                '  acKeepRemove.SelectedValue = "keep"
                                'End If
                                FolderInformation.Text = Replace(Replace(FolderInformation.Text, "Close Current Folder", "Close Folder"), "Aircraft_Listing.aspx?restart=1", "view_template.aspx?ViewID=27")
                                'SpecialProjectScript.Append("$("".keep"").click();")

                                'If VariantList.SelectedIndex > 0 Then
                                If VariantList.Items.Count > 1 Then
                                    SpecialProjectScript.Append("var tabTopL = $find(""" & tabs_top_left.ClientID & """);")
                                    SpecialProjectScript.Append("tabTopL.set_activeTabIndex(0);")

                                    If VariantList.SelectedValue = "" Then
                                    Else
                                        SpecialProjectScript.Append("$('#" & variantModelText.ClientID & "').html(""" & DisplayFunctions.BuildSearchTextDisplay(VariantList, "*Variant Models Loaded, Including") & """);")
                                        SpecialProjectScript.Append("$('#" & variantModelText.ClientID & "').removeClass();")
                                    End If


                                    'End If
                                End If

                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Public Sub SetUpTopButton()
        Dim FoldersTable As New DataTable
        Dim FolderID As Long = 0
        FoldersTable = masterPage.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 16, Nothing, "")

        Dim ExtraButtonsString As String = ""
        ExtraButtonsString = "<span class=""float_left marketReportTopNav"" ><a href=""javascript:SubMenuDropValue2(0,'VALUE');""><i class=""fa fa-area-chart""></i> Market Report</a></span><ul class=""cssMenu_subpage"">"
        ExtraButtonsString += "<li><a href=""#""><strong>Actions</strong></a>"
        ExtraButtonsString += "<ul><li><a href=""javascript:SubMenuDropValue(0,'VALUE');"" class=""noBefore"">Save as - New Folder</a></li>"

        If clsGeneral.clsGeneral.isEValuesAvailable() Then
            If clsGeneral.clsGeneral.isShowingEvalues() Then

                ExtraButtonsString += "<li>" 'SetLoadingText('Turning " & Constants.eValues_Refer_Name & " On');$('body').addClass('loading');$find('" & tabs_bottom.ClientID & "').set_activeTabIndex(1);$('#" & runEvaluesSwap.ClientID & "').click();
                ExtraButtonsString += "<a href=""javascript:createCookie('evalues', 'false', 365);SetLoadingText('Turning " & Constants.eValues_Refer_Name & " Off');$('body').addClass('loading');window.location.href = window.location.href;"" class=""noBefore"">Toggle eValues Off</a></li>"
            Else
                ExtraButtonsString += "<li><a href=""javascript:createCookie('evalues', 'true', 365);SetLoadingText('Turning " & Constants.eValues_Refer_Name & " On');$('body').addClass('loading');window.location.href = window.location.href;"" class=""noBefore"">Toggle eValues On</a></li>"
            End If
        End If
        '  If HttpContext.Current.Session.Item("jetnetWebHostType") = eWebHostTypes.EVOLUTION And InStr(HttpContext.Current.Session.Item("jetnetFullHostName"), "www.jetnettest.com") > 0 Then

        'HttpContext.Current.Session.Item("jetnetServerNotesDatabase") = HttpContext.Current.Session.Item("jetnetClientDatabase")
        ' If localDataLayer.Get_Subscription_Team(Session.Item("localUser").crmSubSubID, "Market Insight") = True Then
        'ExtraButtonsString += "<li><a href=""javascript:SubMenuDropValue2(0,'VALUE');"" class=""noBefore"">Create Market Report</a></li>"
        '  End If

        '  ElseIf InStr(Session.Item("localUser").crmLocalUserEmailAddress, "jetnet.com") > 0 Or InStr(Session.Item("localUser").crmLocalUserEmailAddress, "mvintech.com") > 0 Then
        '  ExtraButtonsString += "<li><a href=""javascript:SubMenuDropValue2(0,'VALUE');"" class=""noBefore"">Create Market Report</a></li>"
        '   End If


        If Page.Request.Form("project_search") = "Y" Then
            If Page.Request.Form("project_id") > 0 Then
                FolderID = Page.Request.Form("project_id")
                ExtraButtonsString += "<li><a href=""javascript:SubMenuDropValue('" & FolderID & "','VALUE');"" class=""noBefore"">Save Current Folder</a></li>"
            End If
        End If

        ExtraButtonsString += "</ul></li>"
        ExtraButtonsString += "</ul>"

        If Not IsNothing(FoldersTable) Then
            If FoldersTable.Rows.Count > 0 Then
                ExtraButtonsString += "<ul class=""cssMenu_subpage"">"
                ExtraButtonsString += "<li><a href=""#""><strong>Folders</strong></a>"
                ExtraButtonsString += "<ul>"
                For Each r As DataRow In FoldersTable.Rows
                    If Not IsDBNull(r("cfolder_data")) Then
                        Dim FolderDataString As Array = Split(r("cfolder_data"), "THEREALSEARCHQUERY")

                        If FolderDataString(0) = "" Then
                            ExtraButtonsString += "<li><a href=""javascript:alert('This folder contains no information.');"" class=""noBefore"">" & r("cfolder_name").ToString & "</a></li>"
                        Else
                            ExtraButtonsString += "<li><a href=""javascript:ParseViewFolders('" & r("cfolder_id").ToString & "', 27,'" & Replace(FolderDataString(0), "'", "\'") & "','false');"" class=""noBefore"">" & r("cfolder_name").ToString & "</a></li>"
                        End If
                    End If

                Next
                ExtraButtonsString += "<li><a href=""FolderMaintenance.aspx?t=16"" target=""new"" class=""noBefore"">Edit Folders</a></li>"
                ExtraButtonsString += "</ul>"
                ExtraButtonsString += "</li>"
                ExtraButtonsString += "</ul>"
            End If
        End If

        If Session.Item("isMobile") = False Then
            buttons.Text = (ExtraButtonsString)
        End If
    End Sub

    Public Sub LoadUpModelAndVariant()
        Dim ModelFeaturesTable As New DataTable
        ModelID = modelList.SelectedValue
        searchCriteria.ViewCriteriaAmodID = ModelID
        searchCriteria.ViewID = 99
        modelImage.ImageUrl = Session.Item("jetnetFullHostName") + Session.Item("ModelPicturesFolderVirtualPath") + "/" + ModelID.ToString + ".jpg"
        Session.Item("searchCriteria").SearchCriteriaViewVariantString = ""
        VariantList.Items.Clear() 'You need to clear these out.
        VariantList.Items.Add(New ListItem("NONE", ""))

        ModelFeaturesTable = GetFeaturesListByModel(ModelID)
        If Not IsNothing(ModelFeaturesTable) Then
            If ModelFeaturesTable.Rows.Count > 0 Then
                FeaturesList = ModelFeaturesTable.Rows(0).Item("FEATURES")
                Session.Item("searchCriteria").SearchCriteriaViewFeatureString = FeaturesList
                If Not IsDBNull(ModelFeaturesTable.Rows(0).Item("ModelVariants")) Then
                    Session.Item("searchCriteria").SearchCriteriaViewVariantString = ModelFeaturesTable.Rows(0).Item("ModelVariants")
                    variantThere.Text = "true"
                    FillUpVariantListbox()
                End If
            End If
        End If
    End Sub
    ''' <summary>
    ''' Function that runs on page load after loading screen is toggled on and button is clicked.
    ''' </summary>
    ''' <remarks></remarks>
    ''' 


    Public Sub RunPageLoad()
        SetUpTopButton()


        'Dim ModelFeaturesTable As New DataTable
        'ModelID = modelList.SelectedValue
        If Page.Request.Form("project_search") = "Y" Then
        Else
            'Do not run this if project search is running. It runs earlier.
            LoadUpModelAndVariant()
        End If
        'searchCriteria.ViewCriteriaAmodID = ModelID
        'searchCriteria.ViewID = 99
        'modelImage.ImageUrl = Session.Item("jetnetFullHostName") + Session.Item("ModelPicturesFolderVirtualPath") + "/" + ModelID.ToString + ".jpg"
        'Session.Item("searchCriteria").SearchCriteriaViewVariantString = ""
        'VariantList.Items.Clear() 'You need to clear these out.
        'VariantList.Items.Add(New ListItem("NONE", ""))

        'ModelFeaturesTable = GetFeaturesListByModel(ModelID)
        'If Not IsNothing(ModelFeaturesTable) Then
        '  If ModelFeaturesTable.Rows.Count > 0 Then
        '    FeaturesList = ModelFeaturesTable.Rows(0).Item("FEATURES")
        '    Session.Item("searchCriteria").SearchCriteriaViewFeatureString = FeaturesList
        '    If Not IsDBNull(ModelFeaturesTable.Rows(0).Item("ModelVariants")) Then
        '      Session.Item("searchCriteria").SearchCriteriaViewVariantString = ModelFeaturesTable.Rows(0).Item("ModelVariants")
        '      variantThere.Text = "true"
        '      FillUpVariantListbox()
        '    End If
        '  End If
        'End If

        ac_market.SelectedValue = ac_market.SelectedValue
        CallScriptToShowHideVariant()
        'DisplayCurrentAircraftTable()
        DisplayTransactionAircraftTable()
        TableBuildJavascript(True) 'Running the table build javascript so we don't have to set it up anywhere else.


        market_functions.views_display_fleet_market_summary(searchCriteria, modelSummaryText.Text, "", "", 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", ac_id_array_current, ac_asking_array_current, array_count_current)

        modelList.Attributes.Add("onChange", "$(""body"").addClass(""loading"")")
        'Get max/min for inputs
        GetSliderValues(maxYear, maxAFTT, minYear, minAFTT, minTransDate, "")
        'Setting these textboxes as default.
        aftt_start.Text = IIf(startBaseAFTT > 0, startBaseAFTT, minAFTT)
        aftt_end.Text = IIf(endBaseAFTT > 0, endBaseAFTT, maxAFTT)
        year_start.Text = IIf(startBaseYear > 0, startBaseYear, minYear)
        year_end.Text = IIf(endBaseYear > 0, endBaseYear, maxYear)

        hiddenAftt_end.Text = aftt_end.Text
        hiddenAftt_start.Text = aftt_start.Text
        hiddenYear_end.Text = year_end.Text
        hiddenYear_start.Text = year_start.Text

        If Page.Request.Form("project_search") = "Y" Then
            dateValueText.Text = IIf(Month(start_date.Text) < 10, "0" & Month(start_date.Text), Month(start_date.Text)) & "/" & IIf(Day(start_date.Text) < 10, "0" & Day(start_date.Text), Day(start_date.Text)) & "/" & Right(Year(start_date.Text), 2) & " - " & IIf(Month(end_date.Text) < 10, "0" & Month(end_date.Text), Month(end_date.Text)) & "/" & IIf(Day(end_date.Text) < 10, "0" & Day(end_date.Text), Day(end_date.Text)) & "/" & Right(Year(end_date.Text), 2)
        Else
            If start_date.Text = "" Then
                Dim TempStartDate As Date = DateAdd(DateInterval.Year, -1, Now())
                Dim startDateString As String = IIf(Month(TempStartDate) < 10, "0" & Month(TempStartDate), Month(TempStartDate)) & "/" & IIf(Day(TempStartDate) < 10, "0" & Day(TempStartDate), Day(TempStartDate)) & "/" & Year(TempStartDate)
                start_date.Text = startDateString '"01/01/" & Year(DateAdd(DateInterval.Year, -1, Now()))
                end_date.Text = IIf(Month(Now()) < 10, "0" & Month(Now()), Month(Now())) & "/" & IIf(Day(Now()) < 10, "0" & Day(Now()), Day(Now())) & "/" & Year(Now())

                dateValueText.Text = startDateString & " - " & IIf(Month(Now()) < 10, "0" & Month(Now()), Month(Now())) & "/" & IIf(Day(Now()) < 10, "0" & Day(Now()), Day(Now())) & "/" & Right(Year(Now()), 2)
            End If
        End If


        RemoveAircraftFromPage()
        ac_market.SelectedValue = ac_market.SelectedValue
        BuildJqueryDropdownJavascript()
        BuildSliderYearJavascript() 'Runs Year Builder JS so it's all set to run when needed.
        BuildSliderAFTTJavascript()
        ac_market.SelectedValue = ac_market.SelectedValue
        BuildSliderDateJavascript()
        BuildJQueryClickEventsJavascript()
        RunValueVintageTab()
        BuildOnLoadJavascript()


        tabs_top_right.OnClientActiveTabChanged = "TabRightTopSwapFunction"
        tabs_top_left.OnClientActiveTabChanged = "TabLeftTopSwapFunction"
        tabs_bottom.OnClientActiveTabChanged = "TabBottomSwapFunction"




    End Sub

    Public Sub RemoveAircraftFromPage()
        ResetRemoveAircraftString.Append(" function resetAircraftOnPage() { ")
        ResetRemoveAircraftString.Append("$find('" & tabs_top_left.ClientID & "').get_tabs()[0]._header.innerHTML = 'My Aircraft';")

        ResetRemoveAircraftString.Append("$find('" & tabs_top_left_2.ClientID & "')._hide();")

        'ResetRemoveAircraftString.Append("var tabBottomPicked = $find('" & tabs_bottom.ClientID & "');")
        'ResetRemoveAircraftString.Append("var thisTab = tabBottomPicked._activeTabIndex;")

        'ResetRemoveAircraftString.Append("if (thisTab == 6) {")
        'ResetRemoveAircraftString.Append("tabBottomPicked.set_activeTab(0);")
        'ResetRemoveAircraftString.Append("};")

        ResetRemoveAircraftString.Append("$find('" & tabs_bottom_7.ClientID & "')._hide();")
        ResetRemoveAircraftString.Append(" };")
    End Sub
    ''' <summary>
    ''' Function that builds the onload javascript.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BuildOnLoadJavascript()
        If Not Page.ClientScript.IsClientScriptBlockRegistered("sliderYearString") Then
            Dim JavascriptOnLoad As String = ""
            JavascriptOnLoad = vbCrLf & "if (window.addEventListener) {"
            JavascriptOnLoad += vbCrLf & " window.addEventListener(""load"", "
            JavascriptOnLoad += vbCrLf & "function () {"

            '   
            'function goes here.
            JavascriptOnLoad += TransactionTableArray.ToString 'CurrentTableArray.ToString
            JavascriptOnLoad += TransactionTableBuild.ToString 'TableBuild.ToString
            JavascriptOnLoad += dropdownString.ToString
            JavascriptOnLoad += jqueryClickEventsString.ToString
            JavascriptOnLoad += sliderYearString.ToString
            JavascriptOnLoad += sliderAFTTString.ToString()
            JavascriptOnLoad += sliderDateString.ToString
            JavascriptOnLoad += ResetRemoveAircraftString.ToString()
            JavascriptOnLoad += VariantString.ToString()
            JavascriptOnLoad += SpecialProjectScript.ToString()
            JavascriptOnLoad += EvaluesScript.ToString()
            JavascriptOnLoad += gaugeScr.ToString()
            JavascriptOnLoad += ";"
            If displayEValues Then
                JavascriptOnLoad += "setUpSliderInitial();"
            End If

            'JavascriptOnLoad += ";$('.bxslider').bxSlider({"
            'JavascriptOnLoad += "auto: false,responsive: true,slideSelector: '.child',"
            'JavascriptOnLoad += "autoControls: false,"
            'JavascriptOnLoad += "stopAutoOnClick: true,"
            'JavascriptOnLoad += "pager: false"
            'JavascriptOnLoad += "})"

            JavascriptOnLoad += "setTimeout(function() {"
            JavascriptOnLoad += "var gw = $("".ContainerBoxSummary"").width() - 20;"
            JavascriptOnLoad += "hideShowGraphs(gw);"
            JavascriptOnLoad += "}, 900);"


            JavascriptOnLoad += vbCrLf & ";toggleVariant();$(""body"").removeClass(""loading"");}, false); "
            'JavascriptOnLoad += vbCrLf & "$find('" & tabs_top_right_4.ClientID & "')._hide();"


            JavascriptOnLoad += vbCrLf & "}" 'Else 
            JavascriptOnLoad += vbCrLf & "else {"

            JavascriptOnLoad += vbCrLf & " window.attachEvent(""load"","
            JavascriptOnLoad += vbCrLf & "function () {"
            'function goes here.

            JavascriptOnLoad += TransactionTableArray.ToString 'CurrentTableArray.ToString
            JavascriptOnLoad += TransactionTableBuild.ToString 'TableBuild.ToString
            JavascriptOnLoad += dropdownString.ToString
            JavascriptOnLoad += gaugeScr.ToString()
            JavascriptOnLoad += jqueryClickEventsString.ToString
            JavascriptOnLoad += sliderYearString.ToString
            JavascriptOnLoad += sliderAFTTString.ToString()
            JavascriptOnLoad += sliderDateString.ToString
            JavascriptOnLoad += ResetRemoveAircraftString.ToString()
            JavascriptOnLoad += VariantString.ToString()
            JavascriptOnLoad += SpecialProjectScript.ToString()
            JavascriptOnLoad += EvaluesScript.ToString()
            'JavascriptOnLoad += ";$('.bxslider').bxSlider({"
            'JavascriptOnLoad += "auto: false,responsive: true, slideSelector: '.child',"
            'JavascriptOnLoad += "autoControls: false,"
            'JavascriptOnLoad += "stopAutoOnClick: true,"
            'JavascriptOnLoad += "pager: false"
            'JavascriptOnLoad += "})"
            JavascriptOnLoad += ";"
            If displayEValues Then
                JavascriptOnLoad += "setUpSliderInitial();"
            End If
            JavascriptOnLoad += "setTimeout(function() {"
            JavascriptOnLoad += "var gw = $("".ContainerBoxSummary"").width() - 20;"
            JavascriptOnLoad += "hideShowGraphs(gw);"
            JavascriptOnLoad += "}, 900);"
            JavascriptOnLoad += vbCrLf & ";toggleVariant();$(""body"").removeClass(""loading"");});"
            'JavascriptOnLoad += vbCrLf & "$find('" & tabs_top_right_4.ClientID & "')._hide();"

            JavascriptOnLoad += vbCrLf & "}" 'End if

            'If Not Page.ClientScript.IsClientScriptBlockRegistered("load") Then
            '  System.Web.UI.ScriptManager.RegisterStartupScript(Me.modelUpdatePanel, Me.GetType, "refreshTable", "Sys.Application.add_load(function() {$('.dataTable').DataTable().draw();});", True)
            'End If
            If Not Page.ClientScript.IsClientScriptBlockRegistered("onLoadCode") Then
                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "onLoadCode", JavascriptOnLoad.ToString, True)
            End If
        End If
        Dim tabSwapString As String = ""
        If Not Page.ClientScript.IsClientScriptBlockRegistered("tabSwapJS") Then

            tabSwapString = "function TabRightTopSwapFunction(sender, args) { "
            tabSwapString += " if (sender.get_activeTabIndex() == 1) { "
            tabSwapString += " swapChosenDropdowns();"
            'tabSwapString += "BuildDateSlider();"
            tabSwapString += " }"
            tabSwapString += " }"
            tabSwapString += "function TabLeftTopSwapFunction(sender, args) { "
            tabSwapString += " if (sender.get_activeTabIndex() == 0) { "
            tabSwapString += " swapChosenDropdowns();"
            'tabSwapString += "BuildDateSlider();"
            tabSwapString += " }"
            tabSwapString += " }"
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "tabSwapJS", tabSwapString, True)
        End If


        If Not Page.ClientScript.IsClientScriptBlockRegistered("tabSwapJSBottom") Then
            tabSwapString = ""
            tabSwapString = "function TabBottomSwapFunction(sender, args) {"
            tabSwapString += "$($.fn.dataTable.tables(true)).DataTable().columns.adjust();"
            tabSwapString += "$($.fn.dataTable.tables(true)).DataTable().scroller.measure();"

            tabSwapString += "$find('" & tabs_top_right_4.ClientID & "')._hide();"
            tabSwapString += "var tabTop = $find(""" & tabs_top_right.ClientID & """);"
            tabSwapString += "tabTop.set_activeTabIndex(0);"

            tabSwapString += "$('#" & createStartGraphs.ClientID & "').removeClass();"
            tabSwapString += "$('#" & createStartGraphs.ClientID & "').addClass(""display_none"");"
            tabSwapString += "$('#" & createStartTransGraphs.ClientID & "').removeClass();"
            tabSwapString += "$('#" & createStartTransGraphs.ClientID & "').addClass(""display_none"");"
            tabSwapString += " if (sender.get_activeTabIndex() == 2) { "
            tabSwapString += "if ($('#" & vtgRan.ClientID & "').val() == 'false') {"
            'tabSwapString += "SetLoadingText('Loading ' + $(""#" & modelList.ClientID & " option:selected"").text() + ' Vintage Values');"
            tabSwapString += "$(""body"").addClass(""loading"");"
            tabSwapString += "$('#" & valuesByYearVintageButton.ClientID & "').click();" '
            tabSwapString += " } "
            tabSwapString += " } else if (sender.get_activeTabIndex() == 4) { "
            tabSwapString += "if ($('#" & afttRan.ClientID & "').val() == 'false') {"
            'tabSwapString += "SetLoadingText('Loading ' + $(""#" & modelList.ClientID & " option:selected"").text() + ' AFTT Values');"
            tabSwapString += "$(""body"").addClass(""loading"");"
            tabSwapString += "$('#" & FirstTimeValuesByAFTTButton.ClientID & "').click();" '
            tabSwapString += " } "
            tabSwapString += " } else if (sender.get_activeTabIndex() == 3) {"
            tabSwapString += "if ($('#" & QuarterRan.ClientID & "').val() == 'false') {"
            'tabSwapString += "SetLoadingText('Loading ' + $(""#" & modelList.ClientID & " option:selected"").text() + ' Quarter Values');"
            tabSwapString += "$(""body"").addClass(""loading"");"
            tabSwapString += "$('#" & FirstTimeValuesByQuarterButton.ClientID & "').click();" '
            tabSwapString += " } "
            tabSwapString += " } else if (sender.get_activeTabIndex() == 5) {"
            tabSwapString += "if ($('#" & WeightRan.ClientID & "').val() == 'false') {"
            'tabSwapString += "SetLoadingText('Loading ' + $(""#" & modelList.ClientID & " option:selected"").text() + ' Weight Class Values');"
            tabSwapString += "$(""body"").addClass(""loading"");"
            tabSwapString += "$('#" & FirstTimeValuesByWeightClassButton.ClientID & "').click();" '
            tabSwapString += " } "
            If displayEValues Then
                tabSwapString += " }  else if (sender.get_activeTabIndex() == 6) {"
                tabSwapString += "if ($('#" & valueValuationRan.ClientID & "').val() == 'false') {"
                'tabSwapString += "SetLoadingText('Loading ' + $(""#" & modelList.ClientID & " option:selected"").text() + ' Valuation');"
                tabSwapString += "$(""body"").addClass(""loading"");"
                tabSwapString += "$('#" & valuesValuationButton.ClientID & "').click();" '
                tabSwapString += " } "
                tabSwapString += " }  else if (sender.get_activeTabIndex() == 7) {"
                tabSwapString += "if ($('#" & valueResidualsRan.ClientID & "').val() == 'false') {"
                ' tabSwapString += "SetLoadingText('Loading ' + $(""#" & modelList.ClientID & " option:selected"").text() + ' Residuals');"
                tabSwapString += "$(""body"").addClass(""loading"");"
                tabSwapString += "$('#" & valuesResidualButton.ClientID & "').click();" '
                tabSwapString += " } "

                tabSwapString += " }  else if (sender.get_activeTabIndex() == 8) {"
                tabSwapString += "if ($('#" & valueEvaluesRan.ClientID & "').val() == 'false') {"
                ' tabSwapString += "SetLoadingText('Loading ' + $(""#" & modelList.ClientID & " option:selected"").text() + ' " & Constants.eValues_Refer_Name.ToString & "');"
                tabSwapString += "$(""body"").addClass(""loading"");"
                tabSwapString += "$('#" & valuesEvaluesButton.ClientID & "').click();" '
                tabSwapString += " } "
            End If

            tabSwapString += " }  else if (sender.get_activeTabIndex() == " & IIf(displayEValues, "9", "6") & ") {"
            tabSwapString += "if ($('#" & valueHistoryRan.ClientID & "').val() == 'false') {"
            'tabSwapString += "SetLoadingText('Loading ' + $(""#" & modelList.ClientID & " option:selected"").text() + ' Value History');"
            tabSwapString += "$(""body"").addClass(""loading"");"
            tabSwapString += "$('#" & valuesHistoryButton.ClientID & "').click();" '
            tabSwapString += " } "
            tabSwapString += " }  else if (sender.get_activeTabIndex() == 0) { "
            'check to see if this has already been ran before we postback, otherwise not needed.
            tabSwapString += "if ($('#" & currentRan.ClientID & "').val() == 'false') {"

            tabSwapString += "$(""body"").addClass(""loading"");"
            'Before this runs, we should try emptying the sales datatable to make sure that it doesn't refilter the info that's in there
            'before it gets refilled up.
            tabSwapString += "$('#startTable').empty();"
            tabSwapString += "$('#" & currentTabButton.ClientID & "').click();" '
            tabSwapString += " } "

            'tabSwapString += " }  else if (sender.get_activeTabIndex() == 1) { "
            ''check to see if this has already been ran before we postback, otherwise not needed.
            'tabSwapString += "if ($('#" & salesRan.ClientID & "').val() == 'false') {"

            'tabSwapString += "SetLoadingText('Loading 3 years of ' + $(""#" & modelList.ClientID & " option:selected"").text() + ' Sales');$(""body"").addClass(""loading"");"
            ''Before this runs, we should try emptying the sales datatable to make sure that it doesn't refilter the info that's in there
            ''before it gets refilled up.
            'tabSwapString += "$('#transactionTable').empty();"
            'tabSwapString += "$('#" & salesTabButton.ClientID & "').click();" '
            'tabSwapString += " } "

            tabSwapString += " } "


            tabSwapString += " }"

            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "tabSwapJSBottom", tabSwapString, True)
        End If

    End Sub

    'Public Function GetModelLoadingText() As String
    '  Dim returnString As String = ""
    '  'returnString = "var option_all = $(""#" & VariantList.ClientID & " option:selected"").map(function () {"
    '  'returnString += " if ($(this).text() == 'NONE') { return '' } {"
    '  'returnString += "return $(this).text();"
    '  'returnString += " } "
    '  'returnString += "}).get().join();"
    '  returnString += "var modelText = $(""#" & modelList.ClientID & " option:selected"").text() + ' ';"
    '  'returnString += " if (option_all == '') {} { "
    '  'returnString += " option_all = ' Variants: ' + option_all;"
    '  'returnString += " }"
    '  returnString += "modelText = modelText + $(""#" & variantModelText.ClientID & """).text();"
    '  Return returnString
    'End Function
    ''' <summary>
    ''' Runs the Current Tab.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RunCurrentTab()
        If currentRan.Text = "false" Then


            DisplayCurrentAircraftTable()
            currentRan.Text = "true"
            RecreateDropdownsValueYearGraphs(tabs_bottom_1_update_panel, False, True, True)
        End If
    End Sub
    ''' <summary>
    ''' Runs the Sales Tab.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RunSalesTab()
        If salesRan.Text = "false" Then


            DisplayTransactionAircraftTable()
            salesRan.Text = "true"

            RecreateDropdownsValueYearGraphs(tabs_bottom_2_update_panel, False, True, True)
        End If
    End Sub
    ''' <summary>
    ''' Runs the Value Vintage TabButton Click.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RunValueVintageTabClick()
        Dim utilization_functions As New utilization_view_functions
        utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

        If vtgRan.Text = "false" Then
            RunValueVintageTab()
            '  vtgRan.Text = "true"
            If Not Page.ClientScript.IsClientScriptBlockRegistered("refreshVintageTable") Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.tabs_top_right_1_update_panel, Me.GetType, "refreshVintageTable", "$(""#" & valueSummaryRefreshButton.ClientID & """).css( ""display"", ""none"" );" & BuildVintageTable().ToString & ";" & IIf(displayEValues, "setUpSlider();", ""), True)
            End If
            tabs_bottom_3_update_panel.Update()
            RecreateDropdownsValueYearGraphs(tabs_top_right_1_update_panel, True, True, True)
        End If

        Select Case tabs_bottom.ActiveTab.ID
            Case tabs_bottom_8.ID.ToString
                BuildValuationTab(False)
                tabs_bottom_8_update_panel.Update()
            Case tabs_bottom_9.ID.ToString
                BuildValuationTab(True)
                tabs_bottom_9_update_panel.Update()
            Case tabs_bottom_3.ID.ToString
                DisplayQuarterTable()
                tabs_bottom_3_update_panel.Update()
            Case tabs_bottom_4.ID.ToString
                BuildMFRGraph(valueYearVintageMFRGraph, tabs_bottom_4_update_panel, 6, utilization_functions, True, True, 500, False)
                valueYearVintageMFRGraph.Text += "</div>"
                tabs_bottom_4_update_panel.Update()
        End Select

        valueSliderGraphUpdate.Update()

    End Sub

    ''' <summary>
    ''' Runs the Value Vintage Tab.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RunValueVintageTab()
        DisplayValueVintageTable(False, False)
    End Sub

    ''' <summary>
    ''' Reinitializes Graphs/Dropdowns if needed
    ''' </summary>
    ''' <param name="updatePanelToAttachTo"></param>
    ''' <param name="graphsNeeded"></param>
    ''' <remarks></remarks>
    Public Sub RecreateDropdownsValueYearGraphs(ByVal updatePanelToAttachTo As UpdatePanel, ByVal graphsNeeded As Boolean, ByVal DropdownsNeeded As Boolean, ByVal LoadingNeeded As Boolean)
        Dim salesBuild As New StringBuilder
        salesBuild.Append(IIf(DropdownsNeeded, "swapChosenDropdowns();", "") & IIf(graphsNeeded, "DrawGraphs();", "") & IIf(LoadingNeeded, "$(""body"").removeClass(""loading"");", ""))

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelToAttachTo, Me.GetType(), "stopLoading", salesBuild.ToString, True)
    End Sub
    ''' <summary>
    ''' Function that displays the Value Vintage Table/Functions that go along with it.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayValueVintageTable(ByVal modelPostback As Boolean, ByVal swapAC As Boolean)
        Dim ValueVintageTable As New DataTable
        Dim MaxMinCurrentTable As New DataTable

        Dim averageSalesAnswer As Long = 0
        Dim averageAskingAnswer As Long = 0

        Dim AvgAskingPriceVsSellingPrice As String = ""
        Dim AvgSellingPriceByYear As String = ""



        If displayEValues Then


            Call utilization_view_functions.Get_Client_AC_Models(modelList.SelectedValue, Me.start_date.Text, Me.end_date.Text)

            If displayEValues Then
                FillUpTopRightSliderGraphs()
                'SetUpSlider()
            End If



            Dim EvaluesMinMax As New DataTable
            evalues_avg.Text = "______"
            evalues_low.Text = "______"
            evalues_high.Text = "______"
            evalues_count.Text = "______"


            EvaluesMinMax = GetEValuesMaxMinCurrent(clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True), modelList.SelectedValue, ac_market.SelectedValue, year_start.Text, year_end.Text, aftt_start.Text, aftt_end.Text, aircraft_registration.SelectedValue, start_date.Text, end_date.Text, "")
            EvaluesScript = New StringBuilder

            If Not IsNothing(EvaluesMinMax) Then
                If EvaluesMinMax.Rows.Count > 0 Then
                    Dim evalueLow As Double = 0
                    Dim evalueAvg As Double = 0
                    Dim evalueHigh As Double = 0
                    If Not IsDBNull(EvaluesMinMax.Rows(0).Item("AVGVALUE")) Then
                        If EvaluesMinMax.Rows(0).Item("AVGVALUE") > 0 Then
                            evalues_avg.Text = "$" & FormatNumber(EvaluesMinMax.Rows(0).Item("AVGVALUE") / 1000, 0)
                            evalueAvg = FormatNumber(EvaluesMinMax.Rows(0).Item("AVGVALUE") / 1000, 0)
                            EvaluesScript.Append("$find('" & tabs_bottom_8.ClientID & "')._show();")
                            EvaluesScript.Append("$find('" & tabs_bottom_10.ClientID & "')._show();")
                            EvaluesScript.Append("$find('" & tabs_bottom_9.ClientID & "')._show();$('#" & avgEvaluesRow.ClientID & "').show();")

                            EvaluesScript.Append("$('#" & evalues1.ClientID & "').show();")
                            EvaluesScript.Append("$('#" & evalues2.ClientID & "').show();")
                            EvaluesScript.Append("$('#" & evalues3.ClientID & "').show();")
                            EvaluesScript.Append("$('#" & evalues4.ClientID & "').show();")
                            EvaluesScript.Append("$('#" & evalues5.ClientID & "').show();")


                            EvaluesScript.Append("$('#" & slide2.ClientID & "').show();")
                            EvaluesScript.Append("$('#" & slide3.ClientID & "').show();")
                            EvaluesScript.Append("$('#" & slide4.ClientID & "').show();")
                            EvaluesScript.Append("$('#" & slide5.ClientID & "').show();")
                            EvaluesScript.Append("$('#" & slide6.ClientID & "').show();$('#" & sliderBX.ClientID & "').addClass(""bxslider"");")
                        End If
                    Else
                        EvaluesScript.Append("$find('" & tabs_bottom_8.ClientID & "')._hide();")
                        EvaluesScript.Append("$find('" & tabs_bottom_10.ClientID & "')._hide();")
                        EvaluesScript.Append("$find('" & tabs_bottom_9.ClientID & "')._hide();$('#" & avgEvaluesRow.ClientID & "').hide();")
                        EvaluesScript.Append("$('#" & evalues1.ClientID & "').hide();")
                        EvaluesScript.Append("$('#" & evalues2.ClientID & "').hide();")
                        EvaluesScript.Append("$('#" & evalues3.ClientID & "').hide();")
                        EvaluesScript.Append("$('#" & evalues4.ClientID & "').hide();")
                        EvaluesScript.Append("$('#" & evalues5.ClientID & "').hide();")
                        EvaluesScript.Append("$('#" & slide2.ClientID & "').hide();")
                        EvaluesScript.Append("$('#" & slide3.ClientID & "').hide();")
                        EvaluesScript.Append("$('#" & slide4.ClientID & "').hide();")
                        EvaluesScript.Append("$('#" & slide5.ClientID & "').hide();")
                        EvaluesScript.Append("$('#" & slide6.ClientID & "').hide();$('#" & sliderBX.ClientID & "').removeClass(""bxslider"");")

                    End If

                    If Not IsDBNull(EvaluesMinMax.Rows(0).Item("COUNTAC")) Then
                        If EvaluesMinMax.Rows(0).Item("COUNTAC") > 0 Then
                            evalues_count.Text = EvaluesMinMax.Rows(0).Item("COUNTAC").ToString
                        End If
                    End If
                    If Not IsDBNull(EvaluesMinMax.Rows(0).Item("HIGHVALUE")) Then
                        If EvaluesMinMax.Rows(0).Item("HIGHVALUE") > 0 Then
                            evalues_high.Text = "$" & FormatNumber(EvaluesMinMax.Rows(0).Item("HIGHVALUE") / 1000, 0)
                            evalueHigh = FormatNumber(EvaluesMinMax.Rows(0).Item("HIGHVALUE") / 1000, 0)
                        End If
                    End If
                    If Not IsDBNull(EvaluesMinMax.Rows(0).Item("LOWVALUE")) Then
                        If EvaluesMinMax.Rows(0).Item("LOWVALUE") > 0 Then
                            evalues_low.Text = "$" & FormatNumber(EvaluesMinMax.Rows(0).Item("LOWVALUE") / 1000, 0)
                            evalueLow = FormatNumber(EvaluesMinMax.Rows(0).Item("LOWVALUE") / 1000, 0)
                        End If
                    End If

                    generateGauge("evaluePriceGauge", evalueLow, evalueAvg, evalueHigh, "Evalue", "EV")
                End If
            End If
        End If


        ValueVintageTable = GetValuesVintageTab(modelList.SelectedValue, ac_market.SelectedValue, year_start.Text, year_end.Text, aftt_start.Text, aftt_end.Text, aircraft_registration.SelectedValue, start_date.Text, end_date.Text, clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True), "")
        MaxMinCurrentTable = GetValuesMaxMinCurrent(modelList.SelectedValue, ac_market.SelectedValue, year_start.Text, year_end.Text, aftt_start.Text, aftt_end.Text, aircraft_registration.SelectedValue, start_date.Text, end_date.Text)





        valueYearVintageLabel.Text = DisplayValueVintageTable(ValueVintageTable, averageAskingAnswer, averageSalesAnswer, AvgAskingPriceVsSellingPrice, AvgSellingPriceByYear)
        BuildChartsYearByVintage(AvgAskingPriceVsSellingPrice, AvgSellingPriceByYear, modelPostback, swapAC)

        If MaxMinCurrentTable.Rows.Count > 0 Then
            Dim minCurrentAsking As Long = Convert.ToInt64(IIf(Not IsDBNull(MaxMinCurrentTable.Compute("min(LOWASKING)", String.Empty)), MaxMinCurrentTable.Compute("min(LOWASKING)", String.Empty), 0))
            Dim maxCurrentAsking As Long = Convert.ToInt64(IIf(Not IsDBNull(MaxMinCurrentTable.Compute("max(HIGHASKING)", String.Empty)), MaxMinCurrentTable.Compute("max(HIGHASKING)", String.Empty), 0))
            Dim avgCurrentAskingTotal As Long = Convert.ToInt64(IIf(Not IsDBNull(MaxMinCurrentTable.Compute("sum(SUMASKING)", String.Empty)), MaxMinCurrentTable.Compute("sum(SUMASKING)", String.Empty), 0))
            Dim totalCountForSale As Long = Convert.ToInt64(IIf(Not IsDBNull(MaxMinCurrentTable.Compute("sum(COUNTFORSALE)", String.Empty)), MaxMinCurrentTable.Compute("sum(COUNTFORSALE)", String.Empty), 0))
            Dim avgCurrentAskingTotalCount As Long = Convert.ToInt64(IIf(Not IsDBNull(MaxMinCurrentTable.Compute("sum(COUNTASKING)", String.Empty)), MaxMinCurrentTable.Compute("sum(COUNTASKING)", String.Empty), 0))
            Dim avgCurrentAsking As Long = 0


            Call utilization_view_functions.Add_in_client_asking("ASKING", avgCurrentAskingTotal, avgCurrentAskingTotalCount, minCurrentAsking, maxCurrentAsking, 0, 0)

            If avgCurrentAskingTotal > 0 And avgCurrentAskingTotalCount > 0 Then
                avgCurrentAsking = avgCurrentAskingTotal / avgCurrentAskingTotalCount
            End If

            lowest_aircraft_on_market.Text = IIf(minCurrentAsking > 0, "$" & FormatNumber((minCurrentAsking / 1000), 0) & "", "______")
            highest_aircraft_on_market.Text = IIf(maxCurrentAsking > 0, "$" & FormatNumber((maxCurrentAsking / 1000), 0) & "", "______")
            average_aircraft_on_market.Text = IIf(avgCurrentAsking > 0, "$" & FormatNumber((avgCurrentAsking / 1000), 0) & "", "______")
            count_aircraft_on_market.Text = IIf(avgCurrentAskingTotalCount > 0, avgCurrentAskingTotalCount, "______")
        Else
            lowest_aircraft_on_market.Text = "______"
            highest_aircraft_on_market.Text = "______"
            average_aircraft_on_market.Text = "______"
            average_asking_sales.Text = "______"
            count_aircraft_on_market.Text = "______"
        End If


        If ValueVintageTable.Rows.Count > 0 Then

            Dim countAsking As Long = Convert.ToInt64(IIf(Not IsDBNull(ValueVintageTable.Compute("sum(COUNTASKING)", String.Empty)), ValueVintageTable.Compute("sum(COUNTASKING)", String.Empty), 0))
            Dim countSale As Long = Convert.ToInt64(IIf(Not IsDBNull(ValueVintageTable.Compute("sum(COUNTSALE)", String.Empty)), ValueVintageTable.Compute("sum(COUNTSALE)", String.Empty), 0))
            Dim SumAsking As Long = Convert.ToInt64(IIf(Not IsDBNull(ValueVintageTable.Compute("sum(SUMASKING)", String.Empty)), ValueVintageTable.Compute("sum(SUMASKING)", String.Empty), 0))
            Dim SumSale As Long = Convert.ToInt64(IIf(Not IsDBNull(ValueVintageTable.Compute("sum(SUMSALE)", String.Empty)), ValueVintageTable.Compute("sum(SUMSALE)", String.Empty), 0))

            Dim minAsking As Long = Convert.ToInt64(IIf(Not IsDBNull(ValueVintageTable.Compute("min(LOWASKING)", String.Empty)), ValueVintageTable.Compute("min(LOWASKING)", String.Empty), 0))
            Dim maxAsking As Long = Convert.ToInt64(IIf(Not IsDBNull(ValueVintageTable.Compute("max(HIGHASKING)", String.Empty)), ValueVintageTable.Compute("max(HIGHASKING)", String.Empty), 0))
            Dim minSale As Long = Convert.ToInt64(IIf(Not IsDBNull(ValueVintageTable.Compute("min(LOWSALE)", String.Empty)), ValueVintageTable.Compute("min(LOWSALE)", String.Empty), 0))
            Dim maxSale As Long = Convert.ToInt64(IIf(Not IsDBNull(ValueVintageTable.Compute("max(HIGHSALE)", String.Empty)), ValueVintageTable.Compute("max(HIGHSALE)", String.Empty), 0))


            Call Add_in_client_sale_prices_min_max(countAsking, countSale, minAsking, maxAsking, minSale, maxSale, SumAsking, SumSale)

            If SumAsking > 0 And countAsking > 0 Then
                averageAskingAnswer = SumAsking / countAsking
            End If

            If SumSale > 0 And countSale > 0 Then
                averageSalesAnswer = SumSale / countSale
            End If


            average_asking_sales.Text = IIf(averageSalesAnswer > 0, "$" & FormatNumber((averageAskingAnswer / 1000), 0) & "", "______")

            average_sale_aircraft_sales.Text = IIf(averageSalesAnswer > 0, "$" & FormatNumber((averageSalesAnswer / 1000), 0) & "", "______")

            lowest_asking_sales.Text = IIf(minAsking > 0, "$" & FormatNumber((minAsking / 1000), 0) & "", "______")
            highest_asking_sales.Text = IIf(maxAsking > 0, "$" & FormatNumber((maxAsking / 1000), 0) & "", "______")
            lowest_sale_aircraft_sales.Text = IIf(minSale > 0, "$" & FormatNumber((minSale / 1000), 0) & "", "______")
            highest_sale_aircraft_sales.Text = IIf(maxSale > 0, "$" & FormatNumber((maxSale / 1000), 0) & "", "______")

            count_asking_sales.Text = IIf(countAsking > 0, countAsking, "______")
            count_sale_aircraft_sales.Text = IIf(countSale > 0, countSale, "______")
            If displayEValues Then
                generateGauge("salePriceGauge", FormatNumber(minSale / 1000), FormatNumber(averageSalesAnswer / 1000), FormatNumber(maxSale / 1000), "SalePrice", "SP")
            End If
        ElseIf ValueVintageTable.Rows.Count = 0 Then
            'lowest_aircraft_on_market.Text = "______"
            'highest_aircraft_on_market.Text = "______"
            'average_aircraft_on_market.Text = "______"
            'average_asking_sales.Text = "______"
            'average_sale_aircraft_sales.Text = "______"
            'lowest_asking_sales.Text = "______"
            'highest_asking_sales.Text = "______"
            'lowest_sale_aircraft_sales.Text = "______"
            'highest_sale_aircraft_sales.Text = "______"
            'average_sale_aircraft_sales.Text = "______"
            'count_asking_sales.Text = "______"
            'count_sale_aircraft_sales.Text = "______"
            'count_aircraft_on_market.Text = "______"
            average_sale_aircraft_sales.Text = "______"
            lowest_asking_sales.Text = "______"
            highest_asking_sales.Text = "______"
            lowest_sale_aircraft_sales.Text = "______"
            highest_sale_aircraft_sales.Text = "______"
            average_sale_aircraft_sales.Text = "______"
            count_asking_sales.Text = "______"
            count_sale_aircraft_sales.Text = "______"
            'If Not String.IsNullOrEmpty(acIDText.Text) Then 'So we need to check for an aircraft ID 
            ' If IsNumeric(acIDText.Text) Then 'Then let's just double check and make sure it's numeric
            ' If Not modelPostback Then 'This function runs when the page loads + when the model gets swapped (when variants get loaded, when for sale/in operation dropdown is changed). 
            'We only need this specific catch to run when it's tied to an aircraft which means it shouldn't run when the model is postback because that clears your aircraft (coming in from details page).

            'sliderYearString.Replace("values: [ " & startBaseYear & ", " & endBaseYear & " ],", "values: [ " & minYear & ", " & maxYear & " ],")
            'sliderAFTTString.Replace("values: [ " & startBaseAFTT & ", " & endBaseAFTT & " ],", "values: [ " & minAFTT & ", " & maxAFTT & " ],")

            'aftt_start.Text = minAFTT
            'aftt_end.Text = maxAFTT
            'year_start.Text = minYear
            'year_end.Text = maxYear
            'DisplayValueVintageTable(False, False)
            'On the upside when this is running from the page load, all we really need to do is modify the slider aftt string/year string
            'End If
            'End If
            'End If

        End If



        tabs_top_right_1_update_panel.Update()
        tabs_bottom_3_update_panel.Update()
        ' SetUpSlider()
        ' valueSliderGraphUpdate.Update()
    End Sub


    Public Sub Add_in_client_sale_prices_min_max(ByRef countAsking As Long, ByRef countSale As Long, ByRef minAsking As Long, ByRef maxAsking As Long, ByRef minSale As Long, ByRef maxSale As Long, ByRef sumasking As Long, ByRef sumsale As Long)

        If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True And has_client_data = True Then
            If array_count > 0 Then
                For i = 0 To array_count - 1
                    If ac_asking_array(i) > 0 Then
                        countAsking = countAsking + 1

                        sumasking = sumasking + CLng(ac_asking_array(i))

                        If CLng(ac_asking_array(i)) < minAsking Then
                            minAsking = CLng(ac_asking_array(i))
                        End If

                        If CLng(ac_asking_array(i)) > maxAsking Then
                            maxAsking = CLng(ac_asking_array(i))
                        End If
                    End If

                    If ac_sold_array(i) > 0 Then
                        countSale = countSale + 1

                        sumsale = sumsale + CLng(ac_sold_array(i))

                        If CLng(ac_sold_array(i)) < minSale Then
                            minSale = CLng(ac_sold_array(i))
                        End If

                        If CLng(ac_sold_array(i)) > maxSale Then
                            maxSale = CLng(ac_sold_array(i))
                        End If
                    End If

                Next
            End If
        End If

    End Sub


    Public Sub RunMyHistoryTabClick(ByVal sender As Object, ByVal e As System.EventArgs)
        If valueHistoryRan.Text = "false" Then
            valueHistoryRan.Text = "true"
            BuildAircraftValueHistory()
            RecreateDropdownsValueYearGraphs(tabs_bottom_9_update_panel, False, True, True)
            tabs_bottom_9_update_panel.Update()
        End If
    End Sub
    Public Sub RunEvaluesTabClick(ByVal sender As Object, ByVal e As System.EventArgs)
        If valueEvaluesRan.Text = "false" Then
            valueEvaluesRan.Text = "true"
            BuildEvaluesTab()
            RecreateDropdownsValueYearGraphs(tabs_bottom_10_update_panel, False, True, True)
            tabs_bottom_10_update_panel.Update()
        End If
    End Sub
    Public Sub RunValuationTabClick(ByVal sender As Object, ByVal e As System.EventArgs)
        If valueValuationRan.Text = "false" Then
            valueValuationRan.Text = "true"
            BuildValuationTab(False)
            RecreateDropdownsValueYearGraphs(tabs_bottom_8_update_panel, False, True, True)
            tabs_bottom_8_update_panel.Update()
        End If
    End Sub
    Public Sub RunResidualTabClick(ByVal sender As Object, ByVal e As System.EventArgs)
        If valueResidualsRan.Text = "false" Then
            BuildValuationTab(True)
            valueResidualsRan.Text = "true"

            RecreateDropdownsValueYearGraphs(tabs_bottom_9_update_panel, False, True, True)
            tabs_bottom_9_update_panel.Update()
        End If
    End Sub
    Public Sub RunEvalSwap(ByVal sender As Object, ByVal e As System.EventArgs)
        RunPageLoad()


        RunValueVintageTab()
        If Not Page.ClientScript.IsClientScriptBlockRegistered("refreshVintageTable") Then
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.tabs_top_right_1_update_panel, Me.GetType, "refreshVintageTable", "$(""#" & valueSummaryRefreshButton.ClientID & """).css( ""display"", ""none"" );" & BuildVintageTable().ToString & ";" & IIf(displayEValues, "setUpSlider();", ""), True)
        End If
        tabs_bottom_3_update_panel.Update()
        RecreateDropdownsValueYearGraphs(tabs_top_right_1_update_panel, True, True, True)
        tabs_top_right_1_update_panel.Update()
    End Sub
    Private Sub BuildEvaluesTab()
        Dim evaluesTable As New DataTable

        Call utilization_view_functions.Get_Client_AC_Models(modelList.SelectedValue, Me.start_date.Text, Me.end_date.Text)

        FeaturesList = Session.Item("searchCriteria").SearchCriteriaViewFeatureString
        EvalTableBuildJavascript(True)
        evaluesTable = GetEValuesCurrentTable(clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True), modelList.SelectedValue, ac_market.SelectedValue, year_start.Text, year_end.Text, aftt_start.Text, aftt_end.Text, aircraft_registration.SelectedValue, start_date.Text, end_date.Text)


        WriteJSArrayCurrentTable(evaluesTable, True)
        If Not Page.ClientScript.IsClientScriptBlockRegistered("loadCurrent") Then
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(tabs_bottom_10_update_panel, Me.GetType(), "loadCurrent", CurrentTableArray.ToString & EvaluesTableStr.ToString & "", True)
        End If

    End Sub
    Private Sub BuildValuationTab(ByVal residualRun As Boolean)

        Dim utilization_functions As New utilization_view_functions
        utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

        If residualRun = True Then


            buildResidualGraph(estimatesResidualTabGraph, tabs_bottom_9_update_panel, 55, utilization_functions, True, 500, True, False)

            estimatesResidualTabGraph.Text = "<div class=""valueSpec aircraftListing Simplistic aircraftSpec gray_background viewBoxMargin"">" & estimatesResidualTabGraph.Text & "</div>"
        Else


            value_estimates_label.Text = ""

            buildAfttValueGraph(estimatesAFTTGraph, tabs_bottom_8_update_panel, 33, utilization_functions, False, 500, True, False, True)
            estimatesAFTTGraph.Text = "<br clear=""all"" /><br clear=""all"" />" & estimatesAFTTGraph.Text

            buildResidualGraph(estimatesResidualGraph, tabs_bottom_8_update_panel, 44, utilization_functions, False, 500, True, False)

            buildCurrentMarketGraph(currentMarketValueGraph, tabs_bottom_8_update_panel, 8, utilization_functions, True, 500, True, False)
            currentMarketValueGraph.Text = "<div class=""valueSpec aircraftListing Simplistic aircraftSpec gray_background viewBoxMargin"">" & currentMarketValueGraph.Text

            BuildMFRGraph(estimatesMfrYearGraph, tabs_bottom_8_update_panel, 1, utilization_functions, False, True, 500, False)

            buildMonthGraph(estimatesMonthGraph, tabs_bottom_8_update_panel, 2, utilization_functions, False, True, 500, False, True, False)

            estimatesResidualGraph.Text += "</div>"
        End If

        'If Not Page.ClientScript.IsClientScriptBlockRegistered("refreshValTable") Then
        '  System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.tabs_top_right_1_update_panel, Me.GetType, "refreshValTable", "setUpSlider();", True)
        'End If
    End Sub
    Sub buildCurrentMarketGraph(ByVal marketLiteral As Literal, ByVal updatePanelUpdate As UpdatePanel, ByVal GraphID As Long, ByRef Utilization_Functions As utilization_view_functions, ByVal displayEvaluesText As Boolean, ByVal divHeight As Integer, ByVal displayHeader As Boolean, ByVal miniGraph As Boolean)
        marketLiteral.Text = "<div class=""Box marginAutoDiv"">"
        If displayEvaluesText Then
            marketLiteral.Text += Constants.eValues_Descriptive_Text
        End If
        If displayHeader Then
            marketLiteral.Text += "<span class=""subHeader"">" & modelList.SelectedItem.Text & " CURRENT MARKET</span>"
        End If

        Utilization_Functions.FillAssettInsightGraphs("CURRENTMARKET", modelList.SelectedValue, marketLiteral.Text, updatePanelUpdate, GraphID, 0, 0, divHeight, 0, True, True, True, "", "N", "", "", "", year_start.Text, year_end.Text, ac_market.SelectedValue, aircraft_registration.SelectedValue, aftt_start.Text, aftt_end.Text, "", clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True), miniGraph, False)
        resizeScript(GraphID, miniGraph)
        marketLiteral.Text += "</div>"
    End Sub
    Sub buildResidualGraph(ByVal residualLiteral As Literal, ByVal updatePanelUpdate As UpdatePanel, ByVal GraphID As Long, ByRef Utilization_Functions As utilization_view_functions, ByVal displayEvaluesText As Boolean, ByVal divHeight As Integer, ByVal displayHeader As Boolean, ByVal miniGraph As Boolean)

        residualLiteral.Text = "<div class=""Box marginAutoDiv"">"
        If displayEvaluesText Then
            residualLiteral.Text += Constants.eValues_Descriptive_Text
        End If
        If displayHeader Then
            residualLiteral.Text += "<span class=""subHeader"">" & modelList.SelectedItem.Text & " RESIDUAL VALUES BY DLV YEAR</span>"
        End If

        Call Utilization_Functions.FillAssettInsightGraphs("RESIDUAL", modelList.SelectedValue, residualLiteral.Text, updatePanelUpdate, GraphID, 0, 0, divHeight, 0, True, True, True, "", "N", "", "", "", year_start.Text, year_end.Text, ac_market.SelectedValue, aircraft_registration.SelectedValue, aftt_start.Text, aftt_end.Text, "", clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True), miniGraph, False)
        residualLiteral.Text += "</div>"
        resizeScript(GraphID, miniGraph)
    End Sub
    Private Sub resizeScript(ByVal graphID As Long, ByVal miniGraph As Boolean)
        Dim scriptOut As New StringBuilder
        If miniGraph = True Then
            scriptOut.Append("$(window).resize(function() {" + vbCrLf)
            scriptOut.Append("if(this.resizeTO) clearTimeout(this.resizeTO);" + vbCrLf)
            scriptOut.Append("this.resizeTO = setTimeout(function() {" + vbCrLf)
            scriptOut.Append("$(this).trigger('resizeEnd');" + vbCrLf)
            scriptOut.Append("}, 500);" + vbCrLf)
            scriptOut.Append("});" + vbCrLf)

            '//redraw graph when window resize is completed  
            scriptOut.Append("$(window).on('resizeEnd', function() {")
            scriptOut.Append("$('#visualization" + graphID.ToString + "').empty();" + vbCrLf)
            scriptOut.Append("   drawVisualization" + graphID.ToString + "();" + vbCrLf)
            scriptOut.Append("});" + vbCrLf)

            System.Web.UI.ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "refreshGraph" & graphID.ToString, scriptOut.ToString, True)
        End If
    End Sub
    Sub buildAfttValueGraph(ByVal afttLiteral As Literal, ByVal updatePanelUpdate As UpdatePanel, ByVal graphID As Long, ByRef Utilization_Functions As utilization_view_functions, ByVal displayEvaluesText As Boolean, ByVal divHeight As Integer, ByVal displayHeader As Boolean, ByVal miniGraph As Boolean, ByVal LineBreak As Boolean)
        If displayEValues Then
            Dim scriptOut As New StringBuilder
            afttLiteral.Text = "<div class=""Box marginAutoDiv"">"
            If displayEvaluesText Then
                afttLiteral.Text += Constants.eValues_Descriptive_Text
            End If

            If displayHeader Then
                afttLiteral.Text += "<span class=""subHeader"">" & modelList.SelectedItem.Text & " VALUES BY AFTT</span>"
            End If
            Call Utilization_Functions.FillAssettInsightGraphs("AFTT", modelList.SelectedValue, afttLiteral.Text, updatePanelUpdate, graphID, 0, 0, divHeight, 0, True, True, True, "", "N", "", "", "", year_start.Text, year_end.Text, ac_market.SelectedValue, aircraft_registration.SelectedValue, aftt_start.Text, aftt_end.Text, "", clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True), miniGraph, False)

            resizeScript(graphID, miniGraph)


            afttLiteral.Text += "</div>"

            If LineBreak Then
                afttLiteral.Text += "<br clear=""all"" />"

            End If
        End If
    End Sub
    Sub buildMonthGraph(ByVal ValueLiteral As Literal, ByVal UpdatePanelUpdate As UpdatePanel, ByVal graphID As Long, ByRef Utilization_Functions As utilization_view_functions, ByVal displayEvaluesText As Boolean, ByVal displayHeader As Boolean, ByVal divHeight As Integer, ByVal miniGraph As Boolean, ByVal LineBreak As Boolean, ByVal aircraftIDUse As Boolean)
        ValueLiteral.Text = ""
        If displayEValues Then
            If LineBreak Then
                ValueLiteral.Text = "<br clear=""all"" />"
            End If
            ValueLiteral.Text += "<div class=""Box marginAutoDiv"">"
            If displayEvaluesText Then
                ValueLiteral.Text += Constants.eValues_Descriptive_Text
            End If

            If displayHeader Then
                ValueLiteral.Text += "<span class=""subHeader"">" & modelList.SelectedItem.Text & " VALUES BY MONTH</span>"
            End If

            Call Utilization_Functions.FillAssettInsightGraphs("ASKSOLD", modelList.SelectedValue, ValueLiteral.Text, UpdatePanelUpdate, graphID, 0, 0, divHeight, 0, True, True, True, "", "N", "", "", "", year_start.Text, year_end.Text, ac_market.SelectedValue, aircraft_registration.SelectedValue, aftt_start.Text, aftt_end.Text, "", clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True), miniGraph, False)
            ValueLiteral.Text += "</div>"
            resizeScript(graphID, miniGraph)
            If LineBreak Then
                ValueLiteral.Text = "<br clear=""all"" />"
            End If

        End If
    End Sub


    ''' <summary>
    ''' Function to display the AFTT table.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayAFTTTable()
        Dim ValueAFTTable As New DataTable
        Dim Graph1 As String = ""
        Dim Graph2 As String = ""
        ValueAFTTable = GetValuesTrendsByAFTT(modelList.SelectedValue, ac_market.SelectedValue, year_start.Text, year_end.Text, aftt_start.Text, aftt_end.Text, aircraft_registration.SelectedValue, start_date.Text, end_date.Text, clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True))

        valueTrendsByAFTTLabel.Text = DisplayValueAFTTTable(ValueAFTTable, Graph1, Graph2)
        BuildAFTTGraphs(Graph1, Graph2)

        BuildAFTTTable()

        If Not Page.ClientScript.IsClientScriptBlockRegistered("refreshAFTTTable") Then
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.tabs_bottom_5_update_panel, Me.GetType, "refreshAFTTTable", TableBuild.ToString, True)
        End If

        RecreateDropdownsValueYearGraphs(tabs_bottom_5_update_panel, False, True, True)

    End Sub

    Sub BuildValueHistoryGraphs(ByVal Graph1 As String)
        Dim GraphStr As String = ""
        Dim CallGraphString As String = ""

        If Not Page.ClientScript.IsClientScriptBlockRegistered("refreshAGraphs") Then
            GraphStr += " var dataVHGraph1 = new google.visualization.DataTable();"
            GraphStr += "dataVHGraph1.addColumn('string', 'Date');"
            GraphStr += "dataVHGraph1.addColumn('number', 'Asking ($k)');"
            GraphStr += "dataVHGraph1.addColumn('number', 'Sold ($k)');"
            GraphStr += "dataVHGraph1.addRows([" & Graph1 & "]);"

            GraphStr += "var VHoptions1 = {'title':'','width':" & IIf(Session.Item("isMobile"), "300", "600") & ",'height':250,legend: "
            GraphStr += "{ position: 'right', textStyle:{fontSize:'11'}},"
            GraphStr += "curveType:  'function',colors: ['blue', 'red', 'green','blue', 'red', 'green'], "
            GraphStr += " 'chartArea': {top:25}, hAxis: { textStyle:{fontSize:'9'}},"
            GraphStr += "vAxis: { title: ''} , series: {    0: { lineWidth: 2, pointSize: 3  }  }"
            GraphStr += " };"
            GraphStr += "var chartVH1 = new google.visualization.LineChart(document.getElementById('valueHistoryGraph1'));"
            GraphStr += "chartVH1.draw(dataVHGraph1, VHoptions1);"

            CallGraphString = "function DrawVHGraphs() {"
            CallGraphString += GraphStr
            CallGraphString += "} ;"
            CallGraphString += "DrawVHGraphs();"
            'valueHistoryGraph1
            ''First load needs to run it after google loads.
            System.Web.UI.ScriptManager.RegisterStartupScript(Me.tabs_bottom_7_update_panel, Me.GetType, "refreshVHGraphs", "" & CallGraphString & ";", True)

        End If

    End Sub

    ''' <summary>
    ''' Function that builds both graphs on the value by aftt tab.
    ''' </summary>
    ''' <param name="Graph1"></param>
    ''' <param name="Graph2"></param>
    ''' <remarks></remarks>
    Sub BuildAFTTGraphs(ByVal Graph1 As String, ByVal Graph2 As String)
        Dim GraphStr As String = ""
        Dim CallGraphString As String = ""
        Dim utilization_functions As New utilization_view_functions
        utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

        If Not Page.ClientScript.IsClientScriptBlockRegistered("refreshAGraphs") Then
            GraphStr += " var dataAFTTGraph1 = new google.visualization.DataTable();"
            GraphStr += "dataAFTTGraph1.addColumn('string', 'AC Year');"
            GraphStr += "dataAFTTGraph1.addColumn('number', 'Avg Asking Price ($k)');"
            GraphStr += "dataAFTTGraph1.addColumn('number', 'Avg Sale Price ($k)');"
            GraphStr += "dataAFTTGraph1.addRows([" & Graph1 & "]);"
            GraphStr += "var AFTToptions1 = {'title':'','width':" & IIf(Session.Item("isMobile"), "300", "490") & ",'height':250,legend: "
            GraphStr += "{ position: 'right', textStyle:{fontSize:'11'}},"
            GraphStr += "curveType:  'function',colors: ['blue', 'red', 'green','blue', 'red', 'green'], "
            GraphStr += " 'chartArea': {top:25}, hAxis: { textStyle:{fontSize:'9'}},"
            GraphStr += "vAxis: { title: ''} , series: {    0: { lineWidth: 2, pointSize: 3  }  }"
            GraphStr += " };"

            GraphStr += "var chartAFTT1 = new google.visualization.LineChart(document.getElementById('graphAFTT1Div'));"
            GraphStr += "chartAFTT1.draw(dataAFTTGraph1, AFTToptions1);"

            GraphStr += " var dataAFTTGraph2 = new google.visualization.DataTable();"
            GraphStr += "dataAFTTGraph2.addColumn('string', 'AC Year'); "
            GraphStr += "dataAFTTGraph2.addColumn('number', 'Avg Sale Price ($k)');  "
            GraphStr += "dataAFTTGraph2.addRows([" & Graph2 & "]);"

            GraphStr += " var AFTToptions2 = {'title':'','width':" & IIf(Session.Item("isMobile"), "300", "490") & ",'height':250,legend: { position: 'right', textStyle:{fontSize:'11'}},"
            GraphStr += "bar: {groupWidth: '75%'}, colors: ['green', 'blue', 'red', 'blue', 'red', 'green'], "
            GraphStr += " 'chartArea': {top:25}, hAxis: { textStyle:{fontSize:'9'}},vAxis: { title: ''} , "
            GraphStr += " series: {    0: { lineWidth: 0, pointSize: 3 } ,  1: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } , "
            GraphStr += " 2: { lineWidth: 0, pointSize: 3} ,  3: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } ,  "
            GraphStr += " 4: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } , "
            GraphStr += " 5: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false }  } };"
            GraphStr += "var chartAFTT2 = new google.visualization.ColumnChart(document.getElementById('graphAFTT2Div'));"
            GraphStr += " chartAFTT2.draw(dataAFTTGraph2, AFTToptions2);"

            CallGraphString = "function DrawAFTTGraphs() {"
            CallGraphString += GraphStr
            CallGraphString += "} ;"
            CallGraphString += "DrawAFTTGraphs();"

            ''First load needs to run it after google loads.
            System.Web.UI.ScriptManager.RegisterStartupScript(Me.tabs_bottom_5_update_panel, Me.GetType, "refreshAGraphs", "" & CallGraphString & ";", True)

        End If

        buildAfttValueGraph(valueAfttGraphAfttTab, tabs_bottom_5_update_panel, 9, utilization_functions, True, 500, True, False, True)

        valueAfttGraphAfttTab.Text = "<div class=""valueSpec aircraftListing Simplistic aircraftSpec gray_background viewBoxMargin"">" & valueAfttGraphAfttTab.Text & "</div>"

    End Sub

    ''' <summary>
    ''' Function to display the quarter table.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayQuarterTable()
        Dim utilization_functions As New utilization_view_functions
        utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim
        Dim ValueQuarterTable As New DataTable
        Dim Graph1 As String = ""
        Dim Graph5 As String = ""
        Dim Graph6 As String = ""
        Dim Graph3 As String = ""
        Dim Graph4 As String = ""
        Dim Graph2 As String = ""
        ValueQuarterTable = GetValuesTrendsByQuarter(modelList.SelectedValue, ac_market.SelectedValue, year_start.Text, year_end.Text, aftt_start.Text, aftt_end.Text, aircraft_registration.SelectedValue, start_date.Text, end_date.Text, False, False, clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True))

        valueTrendsByQuarterLabel.Text = DisplayValueQuarterTable(ValueQuarterTable, Graph1, Graph5, Graph6)

        ValueQuarterTable = New DataTable
        ValueQuarterTable = GetValuesTrendsByQuarter(modelList.SelectedValue, ac_market.SelectedValue, year_start.Text, year_end.Text, aftt_start.Text, aftt_end.Text, aircraft_registration.SelectedValue, start_date.Text, end_date.Text, True, True, clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True))

        DisplayValueQuarterOtherGraphs(ValueQuarterTable, Graph2, Graph3, Graph4)

        BuildValueGraphs(Graph1, Graph2, Graph3, Graph4, Graph5, Graph6)


        BuildQuarterTable()


        buildMonthGraph(valuesByQuarterMonthGraph, tabs_bottom_4_update_panel, 7, utilization_functions, True, True, 500, False, False, False)
        valuesByQuarterMonthGraph.Text = "<div class=""valueSpec aircraftListing Simplistic aircraftSpec gray_background viewBoxMargin"">" & valuesByQuarterMonthGraph.Text & "</div>"
        If Not Page.ClientScript.IsClientScriptBlockRegistered("refreshQuarterTable") Then
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.tabs_bottom_4_update_panel, Me.GetType, "refreshQuarterTable", TableBuild.ToString, True)
        End If


        RecreateDropdownsValueYearGraphs(tabs_bottom_4_update_panel, False, True, True)


    End Sub

    Private Sub BuildValueGraphs(ByRef Graph1 As String, ByRef Graph2 As String, ByRef Graph3 As String, ByRef Graph4 As String, ByRef Graph5 As String, ByRef Graph6 As String)
        Dim CallGraphString As String = ""
        If Not Page.ClientScript.IsClientScriptBlockRegistered("refreshQGraphs") Then
            Graph1 = "dataGraphQuarter1.addRows([" & Graph1 & "]);"
            Graph1 = "dataGraphQuarter1.addColumn('string', 'Quarter'); " & vbNewLine & " dataGraphQuarter1.addColumn('number', 'Asking');" & vbNewLine & " dataGraphQuarter1.addColumn('number', 'Sold'); " & vbNewLine & Graph1

            Graph1 += "var optionsQuarter1 = {'title':'','width':" & IIf(Session.Item("isMobile"), "300", "490") & ",'height':250, " & vbNewLine
            Graph1 += " legend: { position: 'right', textStyle:{fontSize:'11'}}, " & vbNewLine
            Graph1 += "curveType:  'function',colors: ['blue', 'green'], " & vbNewLine
            Graph1 += " 'chartArea': {top:5}, hAxis: { textStyle:{fontSize:'9'}},vAxis: { title: ''}, " & vbNewLine
            Graph1 += " series: {    0: { lineWidth: 2, pointSize: 3 } , " & vbNewLine
            Graph1 += " 1: { lineWidth: 2, pointSize: 3}}}; " & vbNewLine
            Graph1 += " var chartQuarter1 = new google.visualization.LineChart(document.getElementById('graphQuarter1Div'));" & vbNewLine
            Graph1 += " chartQuarter1.draw(dataGraphQuarter1, optionsQuarter1);" & vbNewLine

            Graph1 = " var dataGraphQuarter1 = new google.visualization.DataTable();" & vbNewLine & Graph1


            Graph2 = "dataGraphQuarter2.addRows([" & Graph2 & "]);"
            Graph2 = "dataGraphQuarter2.addColumn('string', 'Quarter'); " & vbNewLine & " dataGraphQuarter2.addColumn('number', 'Asking');" & vbNewLine & " dataGraphQuarter2.addColumn('number', 'Sold'); " & vbNewLine & Graph2
            Graph2 += "var optionsQuarter2 = {'title':'','width':" & IIf(Session.Item("isMobile"), "300", "490") & ",'height':250, " & vbNewLine
            Graph2 += " legend: { position: 'right', textStyle:{fontSize:'11'}}, " & vbNewLine
            Graph2 += "curveType:  'function',colors: ['blue', 'green'], " & vbNewLine
            Graph2 += " 'chartArea': {top:5}, hAxis: { textStyle:{fontSize:'9'}},vAxis: { title: ''}, " & vbNewLine
            Graph2 += " series: {    0: { lineWidth: 2, pointSize: 3 } , " & vbNewLine
            Graph2 += " 1: { lineWidth: 2, pointSize: 3}}}; " & vbNewLine
            Graph2 += " var chartQuarter2 = new google.visualization.LineChart(document.getElementById('graphQuarter2Div'));" & vbNewLine
            Graph2 += " chartQuarter2.draw(dataGraphQuarter2, optionsQuarter2);" & vbNewLine

            Graph2 = " var dataGraphQuarter2 = new google.visualization.DataTable();" & vbNewLine & Graph2

            Graph3 = " var dataGraphQuarter3 = new google.visualization.DataTable(); " &
            " dataGraphQuarter3.addColumn('string', 'Quarter'); " &
            " dataGraphQuarter3.addColumn('number', 'Avg Asking Price ($k)');" &
            " dataGraphQuarter3.addRows([" & Graph3 & "]);" &
            " var optionsQuarter3 = {'title':'','width':" & IIf(Session.Item("isMobile"), "300", "480") & ",'height':230,legend: { position: 'none' },bar: {groupWidth: '75%'}, colors: ['blue'], 'chartArea': {top:25}, hAxis: { textStyle:{fontSize:'9'}},vAxis: { title: ''} , series: {    0: { lineWidth: 0, pointSize: 3  }  } };"
            Graph3 += "var chartQuarter3 = new google.visualization.ColumnChart(document.getElementById('graphQuarter3Div'));"
            Graph3 += "chartQuarter3.draw(dataGraphQuarter3, optionsQuarter3);"


            Graph4 = " var dataGraphQuarter4 = new google.visualization.DataTable(); " &
            " dataGraphQuarter4.addColumn('string', 'Quarter'); " &
            " dataGraphQuarter4.addColumn('number', 'Avg Sold Price ($k)');" &
            " dataGraphQuarter4.addRows([" & Graph4 & "]);" &
            " var optionsQuarter4 = {'title':'','width':" & IIf(Session.Item("isMobile"), "300", "480") & ",'height':230,legend: { position: 'none' },bar: {groupWidth: '75%'}, colors: ['green'], 'chartArea': {top:25}, hAxis: { textStyle:{fontSize:'9'}},vAxis: { title: ''} , series: {    0: { lineWidth: 0, pointSize: 3  }  } };"
            Graph4 += "var chartQuarter4 = new google.visualization.ColumnChart(document.getElementById('graphQuarter4Div'));"
            Graph4 += "chartQuarter4.draw(dataGraphQuarter4, optionsQuarter4);"


            Graph5 = " var dataGraphQuarter5 = new google.visualization.DataTable(); " &
            " dataGraphQuarter5.addColumn('string', 'Quarter'); " &
            " dataGraphQuarter5.addColumn('number', 'Percent of Asking');  " &
            " dataGraphQuarter5.addRows([" & Graph5 & "]);" &
            " var optionsQuarter5 = {'title':'','width':" & IIf(Session.Item("isMobile"), "300", "480") & ",'height':230,legend: { position: 'none' },bar: {groupWidth: '75%'}, colors: ['red'], 'chartArea': {top:25}, hAxis: { textStyle:{fontSize:'9'}},vAxis: { title: ''} , series: {    0: { lineWidth: 0, pointSize: 3  }  } };"

            Graph5 += " var chartQuarter5 = new google.visualization.ColumnChart(document.getElementById('graphQuarter5Div'));"
            Graph5 += " chartQuarter5.draw(dataGraphQuarter5, optionsQuarter5);"


            Graph6 = "var dataGraphQuarter6 = new google.visualization.DataTable(); " &
            "dataGraphQuarter6.addColumn('string', 'Quarter'); " &
            "dataGraphQuarter6.addColumn('number', 'Variance on Asking');" &
            "dataGraphQuarter6.addRows([" & Graph6 & "]);" &
            "var optionsQuarter6 = {'title':'','width':" & IIf(Session.Item("isMobile"), "300", "480") & ",'height':230,legend: { position: 'none' },bar: {groupWidth: '75%'}, colors: ['red'], 'chartArea': {top:25}, hAxis: { textStyle:{fontSize:'9'}},vAxis: { title: ''} , series: {    0: { lineWidth: 0, pointSize: 3  }  } };"

            Graph6 += "var chartQuarter6 = new google.visualization.ColumnChart(document.getElementById('graphQuarter6Div'));"
            Graph6 += "chartQuarter6.draw(dataGraphQuarter6, optionsQuarter6);"



            CallGraphString = "function DrawQuarterGraphs() {"
            If Session.Item("isMobile") = False Then
                CallGraphString += Graph1
            End If
            CallGraphString += Graph2
            CallGraphString += Graph3
            CallGraphString += Graph4
            CallGraphString += Graph5
            CallGraphString += Graph6
            CallGraphString += "} ;DrawQuarterGraphs();"

            ''First load needs to run it after google loads.
            System.Web.UI.ScriptManager.RegisterStartupScript(Me.tabs_bottom_4_update_panel, Me.GetType, "refreshQGraphs", "" & CallGraphString & ";", True)
        End If
    End Sub
    ''' <summary>
    ''' Function to display the weight table.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayWeightTable()
        Dim ValueWeightTable As New DataTable
        Dim Graph1 As String = ""
        Dim Graph2 As String = ""
        ValueWeightTable = GetValuesTrendsByWeight(modelList.SelectedValue, ac_market.SelectedValue, year_start.Text, year_end.Text, aftt_start.Text, aftt_end.Text, aircraft_registration.SelectedValue, start_date.Text, end_date.Text, modelAirframeTypeCode.Text, ModelTypeCode.Text, ModelWeightClass.Text)

        valueTrendsByWeightLabel.Text = DisplayValueWeightTable(ValueWeightTable, Graph1, Graph2)

        BuildWeightGraphs(Graph1, Graph2)
        BuildWeightTable()
        If Not Page.ClientScript.IsClientScriptBlockRegistered("refreshWeightTable") Then
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.tabs_bottom_6_update_panel, Me.GetType, "refreshWeightTable", TableBuild.ToString, True)
        End If
        RecreateDropdownsValueYearGraphs(tabs_bottom_6_update_panel, False, True, True)
    End Sub

    ''' <summary>
    ''' Function that builds both graphs on the value by vintage/year tab.
    ''' </summary>
    ''' <param name="AvgAskingPriceVsSellingPrice"></param>
    ''' <param name="AvgSellingPriceByYear"></param>
    ''' <remarks></remarks>
    Sub BuildChartsYearByVintage(ByVal AvgAskingPriceVsSellingPrice As String, ByVal AvgSellingPriceByYear As String, ByVal modelPostback As Boolean, ByVal swapCurrent As Boolean)
        Dim GraphStr As String = ""
        Dim jsGraphStr As String = ""
        Dim utilization_functions As New utilization_view_functions
        utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim
        GraphStr = ""
        GraphStr += " var dataGraph1 = new google.visualization.DataTable();"
        GraphStr += "dataGraph1.addColumn('string', 'AC Year');"
        GraphStr += "dataGraph1.addColumn('number', 'Avg Asking Price ($k)');"
        GraphStr += "dataGraph1.addColumn('number', 'Avg Sale Price ($k)');"
        GraphStr += "dataGraph1.addRows([" & AvgAskingPriceVsSellingPrice & "]);"
        GraphStr += "var options1 = {'title':'','width':" & IIf(Session.Item("isMobile"), "300", "490") & ",'height':250,legend: "
        GraphStr += "{ position: 'right', textStyle:{fontSize:'11'}},"
        GraphStr += "curveType:  'function',colors: ['blue', 'red', 'green','blue', 'red', 'green'], "
        GraphStr += " 'chartArea': {top:25}, hAxis: { textStyle:{fontSize:'9'}},"
        GraphStr += "vAxis: { title: ''} , series: {    0: { lineWidth: 2, pointSize: 3  }  }"
        GraphStr += " };"

        GraphStr += "var chart1 = new google.visualization.LineChart(document.getElementById('graph1'));"
        GraphStr += "chart1.draw(dataGraph1, options1);"

        GraphStr += " var dataGraph2 = new google.visualization.DataTable();"
        GraphStr += "dataGraph2.addColumn('string', 'AC Year'); "
        GraphStr += "dataGraph2.addColumn('number', 'Avg Sale Price ($k)');  "
        GraphStr += "dataGraph2.addRows([" & AvgSellingPriceByYear & "]);"

        GraphStr += " var options2 = {'title':'','width':" & IIf(Session.Item("isMobile"), "300", "490") & ",'height':250,legend: { position: 'right', textStyle:{fontSize:'11'}},"
        GraphStr += "bar: {groupWidth: '75%'}, colors: ['green', 'blue', 'red', 'blue', 'red', 'green'], "
        GraphStr += " 'chartArea': {top:25}, hAxis: { textStyle:{fontSize:'9'}},vAxis: { title: ''} , "
        GraphStr += " series: {    0: { lineWidth: 0, pointSize: 3 } ,  1: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } , "
        GraphStr += " 2: { lineWidth: 0, pointSize: 3} ,  3: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } ,  "
        GraphStr += " 4: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } , "
        GraphStr += " 5: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false }  } };"
        GraphStr += "var chart2 = new google.visualization.ColumnChart(document.getElementById('graph2'));"
        GraphStr += " chart2.draw(dataGraph2, options2);"

        If modelPostback = True Or swapCurrent = True Then
        Else

            jsGraphStr += "function DrawGraphs() {" & GraphStr & " };"
            jsGraphStr += "google.charts.setOnLoadCallback(function() {"
            jsGraphStr += "DrawGraphs();"
            jsGraphStr += "});"

            GraphStr = jsGraphStr
        End If

        ' If Not Page.ClientScript.IsClientScriptBlockRegistered("loadMarketChar") Then
        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "loadMarketChar", " google.charts.load('current', { 'packages': ['corechart', 'table'] });", True)
        'End If

        ''First load needs to run it after google loads.
        System.Web.UI.ScriptManager.RegisterStartupScript(IIf(modelPostback, Me.modelUpdatePanel, IIf(swapCurrent, Me.loadWhatUpdate, Me.tabs_bottom_3_update_panel)), Me.GetType(), "DrawGraph", GraphStr, True)


        BuildMFRGraph(valueYearVintageMFRGraph, tabs_bottom_3_update_panel, 6, utilization_functions, True, True, 500, False)
        valueYearVintageMFRGraph.Text += "</div>"

        'If this runs in postback, we also need to draw graphs/rebuild the select boxes.
        'If Page.IsPostBack Then
        '  Dim PostBackStr As New StringBuilder
        '  PostBackStr.Append("$("".chosen-select"").chosen(""destroy"");")
        '  PostBackStr.Append("$("".chosen-select"").chosen({ no_results_text: ""No results found."", disable_search_threshold: 10 });")
        '  PostBackStr.Append("DrawGraphs();")
        '  System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.tabs_bottom_3_update_panel, Me.GetType(), "redrawGraph", PostBackStr.ToString, True)
        'End If
    End Sub


    Private Sub BuildMFRGraph(ByVal LiteralDisplay As Literal, ByVal updatePanelToTarget As UpdatePanel, ByVal graphID As Long, ByRef Utilization_Functions As utilization_view_functions, ByVal displayEvaluesText As Boolean, ByVal displayHeader As Boolean, ByVal divHeight As Integer, ByVal miniGraph As Boolean)

        If displayEValues Then
            LiteralDisplay.Text = "<div class=""valueSpec aircraftListing Simplistic aircraftSpec gray_background viewBoxMargin""><div class=""Box marginAutoDiv"">"
            If displayEvaluesText Then
                LiteralDisplay.Text += Constants.eValues_Descriptive_Text
            End If
            If displayHeader Then
                LiteralDisplay.Text += "<span class=""subHeader"">" & modelList.SelectedItem.Text & " VALUES BY DLV YEAR</span>"
            End If

            Call Utilization_Functions.FillAssettInsightGraphs("DLVYEAR", modelList.SelectedValue, LiteralDisplay.Text, updatePanelToTarget, graphID, 0, 0, divHeight, 0, True, True, True, "", "N", "", "", "", year_start.Text, year_end.Text, ac_market.SelectedValue, aircraft_registration.SelectedValue, aftt_start.Text, aftt_end.Text, "", clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True), miniGraph, False)


            LiteralDisplay.Text += "</div>"
            resizeScript(graphID, miniGraph)
        End If
    End Sub
    ''' <summary>
    ''' This function builds the aircraft tab's table.
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Private Sub DisplayCurrentAircraftTable()
        Dim CurrentAircraftTable As New DataTable


        FeaturesList = Session.Item("searchCriteria").SearchCriteriaViewFeatureString
        TableBuildJavascript(False)


        Call utilization_view_functions.Get_Client_AC_Models(modelList.SelectedValue, Me.start_date.Text, Me.end_date.Text)


        CurrentAircraftTable = GetAircraftStartingTable(modelList.SelectedValue, ac_market.SelectedValue, year_start.Text, year_end.Text, aftt_start.Text, aftt_end.Text, "", "", clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True), False)

        GrabCurrentModelInformation(CurrentAircraftTable)

        'currentAircraftText.Text = DisplayCurrentAircraftStartingTable(CurrentAircraftTable)
        WriteJSArrayCurrentTable(CurrentAircraftTable, False)
        If Not Page.ClientScript.IsClientScriptBlockRegistered("loadCurrent") Then
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(tabs_bottom_1_update_panel, Me.GetType(), "loadCurrent", CurrentTableArray.ToString & TableBuild.ToString & "", True)
        End If
    End Sub

    ''' <summary>
    ''' Right now this just pulls a couple of extra fields from the current aircraft list and updates some hidden labels.
    ''' If for some reason the current aircraft table starts pulling different models - we need to swap the datatable out and add a second lookup. Right now it is not needed.
    ''' </summary>
    ''' <param name="CurrentAircraftTable"></param>
    ''' <remarks></remarks>
    Private Sub GrabCurrentModelInformation(ByVal CurrentAircraftTable As DataTable)
        If Not IsNothing(CurrentAircraftTable) Then
            If CurrentAircraftTable.Rows.Count > 0 Then
                modelAirframeTypeCode.Text = CurrentAircraftTable.Rows(0).Item("amod_airframe_type_code")
                ModelTypeCode.Text = CurrentAircraftTable.Rows(0).Item("amod_type_code")
                ModelWeightClass.Text = CurrentAircraftTable.Rows(0).Item("amod_weight_class")
            End If
        End If
    End Sub
    ''' <summary>
    ''' This function builds the sales tab's table.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayTransactionAircraftTable()
        Dim CurrentTransactionTable As New DataTable
        'FeaturesList = Session.Item("searchCriteria").SearchCriteriaViewFeatureString
        ' TableBuildJavascript(True)


        CurrentTransactionTable = GetTransactionAircraftStartingTable(modelList.SelectedValue, "", clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True), has_client_data)
        GrabCurrentModelInformation(CurrentTransactionTable)

        WriteTransactionAircraftStartingTableJS(CurrentTransactionTable)
        'If Not Page.ClientScript.IsClientScriptBlockRegistered("loadSales") Then
        '  'If salesACIDs.Text <> "" Then
        '  '  TransactionTableArray.Append("setTimeout(function(){$( "".keepTr"").click();console.log('here');},500);")
        '  'End If
        '  System.Web.UI.ScriptManager.RegisterClientScriptBlock(tabs_bottom_2_update_panel, Me.GetType(), "loadSales", TransactionTableArray.ToString & TransactionTableBuild.ToString, True)
        'End If
    End Sub


    Public Function GetEValuesCurrentTable(ByVal VariantListString As String, ByVal amod_id As Long, ByVal forsaleFlag As String, ByVal yearOne As String, ByVal yearTwo As String, ByVal afttStart As String, ByVal afttEnd As String, ByVal regType As String, ByVal Startdate As String, ByVal EndDate As String) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim sQuery As New StringBuilder
        Try

            If amod_id <> 0 Then


                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()

                sQuery.Append(" select  ")

                sQuery.Append(" ac_id, ac_ser_no, ac_ser_no_full, ac_ser_no_sort, ac_reg_no, ac_year, ac_mfr_year, ac_forsale_flag, ")
                sQuery.Append(" ac_asking, ac_asking_price,")

                sQuery.Append(" ac_engine_1_soh_hrs, ac_engine_2_soh_hrs, ")


                If FeaturesList <> "" Then
                    sQuery.Append(FeaturesList & ",")
                End If

                If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
                    sQuery.Append(" (select top 1 ac_sale_price From Aircraft b with (NOLOCK)")
                    sQuery.Append(" inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id")
                    sQuery.Append(" where(a.ac_id = b.ac_id)")
                    sQuery.Append(" and ac_sale_price > 0  and ac_sale_price_display_flag = 'Y' and journ_subcat_code_part1='WS' AND journ_internal_trans_flag='N' ")
                    sQuery.Append(" and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')")
                    sQuery.Append(" order by journ_date desc) as  LASTSALEPRICE,")

                    If displayEValues Then
                        sQuery.Append(" afmv_value AS EVALUE, ")
                        sQuery.Append(" (select AVGMODYREVALUE from ReturnAssetInsightModelYeareValue(a.ac_id)) as AVGMODYREVALUE, ")
                    Else
                        sQuery.Append(" NULL as EVALUE,")
                        sQuery.Append(" NULL as AVGMODYREVALUE, ")
                    End If
                Else
                    sQuery.Append(" NULL as EVALUE,")
                    sQuery.Append(" NULL as AVGMODYREVALUE, ")
                    sQuery.Append(" NULL as LASTSALEPRICE, ")
                End If

                If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
                    sQuery.Append(" (select top 1 journ_date From Aircraft b with (NOLOCK)")
                    sQuery.Append(" inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id")
                    sQuery.Append(" where a.ac_id = b.ac_id and ac_sale_price > 0 and ac_sale_price_display_flag = 'Y' and journ_subcat_code_part1='WS'")
                    sQuery.Append(" AND journ_internal_trans_flag='N' ")
                    sQuery.Append(" and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')")
                    sQuery.Append(" order by journ_date desc) as  LASTSALEPRICEDATE,")
                Else
                    sQuery.Append(" NULL as LASTSALEPRICEDATE, ")
                End If
                sQuery.Append(" (select top 1 comp_name from Company with (NOLOCK)  inner join Aircraft_Reference with (NOLOCK) on cref_comp_id = comp_id and cref_journ_id = comp_journ_id where cref_ac_id = a.ac_id and a.ac_journ_id = cref_journ_id and cref_contact_type in ('00','08','17')) as ACOwner,")
                sQuery.Append(" case amp_program_name when 'Unknown' then '' when 'Confirmed to be on a maintenance program' then 'Confirmed' when 'Confirmed not on any maintenance program' then 'Confirmed Not' else amp_program_name end as APROG,")
                sQuery.Append(" case emp_program_name when 'Unknown' then '' when 'Confirmed to be on a maintenance program' then 'Confirmed' when 'Confirmed not on any maintenance program' then 'Confirmed Not' else emp_program_name end as EPROG, ")
                sQuery.Append(" ac_airframe_tot_hrs, ac_est_airframe_hrs, amod_airframe_type_code, amod_type_code, amod_weight_class, amod_model_name, amod_make_name, ")
                sQuery.Append(" ac_engine_1_tot_hrs, ")
                sQuery.Append(" ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs, ac_interior_moyear, ac_exterior_moyear, ac_list_date, ac_status, ")
                sQuery.Append(" ac_passenger_count, ac_journ_id,")
                sQuery.Append(" ac_previously_owned_flag, ac_lease_flag, ac_maintained, afmv_airframe_hrs ")

                sQuery.Append(" from Aircraft_FMV with (NOLOCK) ")
                sQuery.Append(" inner join Aircraft_Flat a with (NOLOCK) on afmv_ac_id = ac_id and ac_journ_id = 0 ")
                sQuery.Append(" LEFT outer join Aircraft_Features_Flat on a.ac_id = afeat_ac_id and a.ac_journ_id = afeat_journ_id")
                sQuery.Append(" where afmv_status='Y' and afmv_latest_flag='Y' and afmv_value > 0 and ac_journ_id = 0 ")


                If Not String.IsNullOrEmpty(VariantListString) Then
                    sQuery.Append(" and amod_id in (" & amod_id.ToString & "," & VariantListString & ")")
                Else
                    sQuery.Append(" and amod_id = @amodID")
                End If


                '-- YEAR RANGE
                sQuery.Append(" and ac_year between @yearOne and @yearTwo")

                If forsaleFlag = "Y" Then
                    sQuery.Append(" and ac_forsale_flag = 'Y' ")
                End If

                'reg Type
                If regType = "N" Then
                    sQuery.Append(" and ac_reg_no like 'N%' ")
                ElseIf regType = "I" Then
                    sQuery.Append(" and ac_reg_no not like 'N%' ")
                End If

                '-- AFTT
                sQuery.Append(" and afmv_airframe_hrs between @startAFTT and @endAFTT")



                sQuery.Append(utilization_view_functions.add_client_ac_string(False))


                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
                SqlCommand.Parameters.AddWithValue("amodID", amod_id)
                SqlCommand.Parameters.AddWithValue("yearOne", yearOne)
                SqlCommand.Parameters.AddWithValue("yearTwo", yearTwo)
                SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
                SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If




        Catch ex As Exception
            GetEValuesCurrentTable = Nothing
            'Me.class_error = "Error in GetEValuesCurrentTable(): As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try
        Return TempTable
    End Function




    ''' <summary>
    ''' This function grabs the aircraft table data.
    ''' </summary>
    ''' <param name="amod_id"></param>
    ''' <param name="forsaleFlag"></param>
    ''' <param name="yearOne"></param>
    ''' <param name="yearTwo"></param>
    ''' <param name="afttStart"></param>
    ''' <param name="afttEnd"></param>
    ''' <param name="regType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAircraftStartingTable(ByVal amod_id As Long, ByVal forsaleFlag As String, ByVal yearOne As String, ByVal yearTwo As String, ByVal afttStart As String, ByVal afttEnd As String, ByVal regType As String, ByVal idList As String, ByVal variantListString As String, ByVal loadAll As Boolean) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim query As String
        Try

            If amod_id <> 0 Then


                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()

                query = "SELECT ac_id,ac_ser_no, ac_ser_no_full, ac_ser_no_sort, ac_reg_no, ac_year, ac_mfr_year, ac_forsale_flag,"
                query += " ac_asking, ac_asking_price,"


                query += " ac_engine_1_soh_hrs, ac_engine_2_soh_hrs, "


                If FeaturesList <> "" Then
                    query += FeaturesList & ","
                End If

                If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
                    query += " (select top 1 ac_sale_price From Aircraft b with (NOLOCK)"
                    query += " inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id"
                    query += " where(a.ac_id = b.ac_id)"
                    query += " and ac_sale_price > 0  and ac_sale_price_display_flag = 'Y' and journ_subcat_code_part1='WS' AND journ_internal_trans_flag='N' "
                    query += " and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')"
                    query += " order by journ_date desc) as  LASTSALEPRICE,"

                    If displayEValues Then
                        query += " (select afmv_value from ReturnAssetInsighteValue(ac_id)) as EVALUE,"
                        query += " (select AVGMODYREVALUE from ReturnAssetInsightModelYeareValue(ac_id)) as AVGMODYREVALUE, "
                    Else
                        query += " NULL as EVALUE,"
                        query += " NULL as AVGMODYREVALUE, "
                    End If
                Else
                    query += " NULL as EVALUE,"
                    query += " NULL as AVGMODYREVALUE, "
                    query += " NULL as LASTSALEPRICE, "
                End If

                If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
                    query += " (select top 1 journ_date From Aircraft b with (NOLOCK)"
                    query += " inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id"
                    query += " where a.ac_id = b.ac_id and ac_sale_price > 0 and ac_sale_price_display_flag = 'Y' and journ_subcat_code_part1='WS'"
                    query += " AND journ_internal_trans_flag='N' "
                    query += " and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')"
                    query += " order by journ_date desc) as  LASTSALEPRICEDATE,"
                Else
                    query += " NULL as LASTSALEPRICEDATE, "
                End If
                query += " (select top 1 comp_name from Company with (NOLOCK)  inner join Aircraft_Reference with (NOLOCK) on cref_comp_id = comp_id and cref_journ_id = comp_journ_id where cref_ac_id = a.ac_id and a.ac_journ_id = cref_journ_id and cref_contact_type in ('00','08','17')) as ACOwner,"
                query += " case amp_program_name when 'Unknown' then '' when 'Confirmed to be on a maintenance program' then 'Confirmed' when 'Confirmed not on any maintenance program' then 'Confirmed Not' else amp_program_name end as APROG,"
                query += " case emp_program_name when 'Unknown' then '' when 'Confirmed to be on a maintenance program' then 'Confirmed' when 'Confirmed not on any maintenance program' then 'Confirmed Not' else emp_program_name end as EPROG, "
                query += " ac_airframe_tot_hrs , ac_est_airframe_hrs, amod_airframe_type_code, amod_type_code, amod_weight_class, amod_model_name, amod_make_name, "
                query += " ac_engine_1_tot_hrs, "
                query += " ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs, ac_interior_moyear, ac_exterior_moyear, ac_list_date, ac_status, "
                query += " ac_passenger_count, ac_journ_id,"
                query += " ac_previously_owned_flag, ac_lease_flag, ac_maintained"
                query += " from Aircraft_Flat a with (NOLOCK)"
                query += " LEFT outer join Aircraft_Features_Flat on a.ac_id = afeat_ac_id and a.ac_journ_id = afeat_journ_id"
                query += " WHERE ac_journ_id = 0 "

                If idList <> "" Then
                    query += " and ac_id in (" & idList & ") "
                End If

                query += clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True)

                If loadAll = False Then
                    query += " and (ac_forsale_flag ='Y' "

                    query += utilization_view_functions.add_client_ac_string(True)

                    query += " ) "
                End If

                query += " and ac_lifecycle_stage=3"

                If Not String.IsNullOrEmpty(variantListString) Then
                    query += " and amod_id in (" & amod_id & "," & variantListString & ")"
                Else
                    query += " and amod_id = @amodID"
                End If




                query += " ORDER BY ac_ser_no_sort"

                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)
                SqlCommand.Parameters.AddWithValue("amodID", amod_id)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If



            Return TempTable
        Catch ex As Exception
            GetAircraftStartingTable = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

    End Function
    Public Shared Function GetEValuesMaxMinCurrent(ByVal VariantListString As String, ByVal amod_id As Long, ByVal forsaleFlag As String, ByVal yearOne As String, ByVal yearTwo As String, ByVal afttStart As String, ByVal afttEnd As String, ByVal regType As String, ByVal Startdate As String, ByVal EndDate As String, ByVal acList As String) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim sQuery As New StringBuilder
        Try

            If amod_id <> 0 Or acList <> "" Then


                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()

                sQuery.Append(" select min(afmv_value) AS LOWVALUE, count(distinct ac_id) as COUNTAC, ")
                sQuery.Append(" AVG(afmv_value) AS AVGVALUE, MAX(afmv_value) AS HIGHVALUE, month(getdate()) as month1, year(getdate()) as year1 ")
                sQuery.Append(" from Aircraft_FMV with (NOLOCK) ")
                sQuery.Append(" inner join Aircraft_Flat with (NOLOCK) on afmv_ac_id = ac_id and ac_journ_id = 0 ")
                sQuery.Append(" where afmv_status='Y' and afmv_latest_flag='Y' and afmv_value > 0 ")

                If amod_id > 0 Then
                    If Not String.IsNullOrEmpty(VariantListString) Then
                        sQuery.Append(" and amod_id in (" & amod_id.ToString & "," & VariantListString & ")")
                    Else
                        sQuery.Append(" and amod_id = @amodID")
                    End If
                Else
                    sQuery.Append(" and ac_id IN (")
                    sQuery.Append(acList)
                    sQuery.Append(") ")
                End If

                '-- YEAR RANGE
                If Not String.IsNullOrEmpty(yearOne) And Not String.IsNullOrEmpty(yearTwo) Then
                    sQuery.Append(" and ac_year between @yearOne and @yearTwo")
                End If

                If forsaleFlag = "Y" Then
                    sQuery.Append(" and ac_forsale_flag = 'Y' ")
                End If

                'reg Type
                If regType = "N" Then
                    sQuery.Append(" and ac_reg_no like 'N%' ")
                ElseIf regType = "I" Then
                    sQuery.Append(" and ac_reg_no not like 'N%' ")
                End If

                '-- AFTT
                If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
                    sQuery.Append(" and afmv_airframe_hrs between @startAFTT and @endAFTT")
                End If

                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "ValueControl.ascx.vb GetEValuesMaxMinCurrent()", sQuery.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
                SqlCommand.Parameters.AddWithValue("amodID", amod_id)
                SqlCommand.Parameters.AddWithValue("yearOne", yearOne)
                SqlCommand.Parameters.AddWithValue("yearTwo", yearTwo)
                SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
                SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If




        Catch ex As Exception
            GetEValuesMaxMinCurrent = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try
        Return TempTable
    End Function


    ''' <summary>
    ''' This grabs the data for the top right tab (value summary.)
    ''' </summary>
    ''' <param name="amod_id"></param>
    ''' <param name="forsaleFlag"></param>
    ''' <param name="yearOne"></param>
    ''' <param name="yearTwo"></param>
    ''' <param name="afttStart"></param>
    ''' <param name="afttEnd"></param>
    ''' <param name="regType"></param>
    ''' <param name="Startdate"></param>
    ''' <param name="EndDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetValuesMaxMinCurrent(ByVal amod_id As Long, ByVal forsaleFlag As String, ByVal yearOne As String, ByVal yearTwo As String, ByVal afttStart As String, ByVal afttEnd As String, ByVal regType As String, ByVal Startdate As String, ByVal EndDate As String) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim query As String

        Try

            If amod_id <> 0 Then


                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()

                query = "SELECT count(distinct ac_id) as INOP,"
                query += " SUM(case when ac_forsale_flag='Y' then 1 else 0 end) as COUNTFORSALE,"
                query += " MIN(ac_asking_price) as LOWASKING, "
                query += " AVG(ac_asking_price) AS AVGASKING,"
                query += " SUM(case when ac_asking_price > 0 then 1 else 0 end) as COUNTASKING,"
                query += " SUM(ac_asking_price) as SUMASKING,"
                query += " MAX(ac_asking_price) as HIGHASKING"
                query += " From Aircraft_Flat with (NOLOCK) where ac_journ_id = 0 and ac_lifecycle_stage = 3 "
                query += " and amod_id = @amodID "

                query += clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True)

                '-- YEAR RANGE
                query += " and ac_year between @yearOne and @yearTwo"
                '-- WITH OR WITHOUT SALE PRICES

                'reg Type
                If regType = "N" Then
                    query += " and ac_reg_no like 'N%' "
                ElseIf regType = "I" Then
                    query += " and ac_reg_no not like 'N%' "
                End If

                '-- AFTT
                query += " and ac_airframe_tot_hrs between @startAFTT and @endAFTT"

                ' ADDED MSW - 10/24/19


                query += utilization_view_functions.add_client_ac_string(False)



                'query += " group by amod_id,ac_mfr_year"
                'query += " order by ac_mfr_year"



                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)
                SqlCommand.Parameters.AddWithValue("amodID", amod_id)
                SqlCommand.Parameters.AddWithValue("yearOne", yearOne)
                SqlCommand.Parameters.AddWithValue("yearTwo", yearTwo)
                SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
                SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If




        Catch ex As Exception
            GetValuesMaxMinCurrent = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try
        Return TempTable
    End Function


    ''' <summary>
    ''' This function grabs the values tab.
    ''' </summary>
    ''' <param name="amod_id"></param>
    ''' <param name="forsaleFlag"></param>
    ''' <param name="yearOne"></param>
    ''' <param name="yearTwo"></param>
    ''' <param name="afttStart"></param>
    ''' <param name="afttEnd"></param>
    ''' <param name="regType"></param>
    ''' <param name="Startdate"></param>
    ''' <param name="EndDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetValuesVintageTab(ByVal amod_id As Long, ByVal forsaleFlag As String, ByVal yearOne As String, ByVal yearTwo As String, ByVal afttStart As String, ByVal afttEnd As String, ByVal regType As String, ByVal Startdate As String, ByVal EndDate As String, ByVal variantListString As String, ByVal bad_year_ac_id As String) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim query As String
        Dim temp_query As String = ""
        Dim where_clause As String = ""

        Try

            If amod_id <> 0 Then

                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()

                query = "SELECT distinct ac_amod_id, ac_year AS DLVYEAR, "
                query += " (select count(distinct ac_id) from Aircraft_Flat with (NOLOCK)"
                query += " where ac_journ_id = 0 and ac_lifecycle_stage = 3 and Aircraft_Flat.amod_id = a.ac_amod_id and Aircraft_Flat.ac_year=a.ac_year) as INOP,"
                query += " count(distinct journ_id) as SALECOUNT,"
                query += " MIN(ac_asking_price) as LOWASKING, "
                query += " AVG(ac_asking_price) AS AVGASKING,"
                query += " MAX(ac_asking_price) as HIGHASKING,"
                query += " MIN(ac_sale_price) AS LOWSALE,"
                query += " AVG(ac_sale_price) AS AVGSALE,"
                query += " max(ac_sale_price) AS HIGHSALE,"
                query += " SUM(case when ac_asking_price > 0 then 1 else 0 end) as COUNTASKING, "
                query += " SUM(ac_asking_price) as SUMASKING, "
                query += " SUM(case when ac_sale_price > 0 then 1 else 0 end) as COUNTSALE, "
                query += " SUM(ac_sale_price) as SUMSALE"

                query += " From Aircraft a with (NOLOCK) "
                query += " inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id "


                query += " where ac_id > 0  "


                ' added MSW - will go in, even if no client 
                query += utilization_view_functions.setup_where_clause_client(bad_year_ac_id, variantListString, forsaleFlag, amod_id, regType, afttStart, "Vintage1")


                ' commented out, gonig to get all of the normal items the
                ' query += add_client_ac_string(False)

                query += " group by ac_amod_id,ac_year"
                query += " order by ac_year"


                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)
                SqlCommand.Parameters.AddWithValue("amodID", amod_id)
                SqlCommand.Parameters.AddWithValue("yearOne", yearOne)
                SqlCommand.Parameters.AddWithValue("yearTwo", yearTwo)
                SqlCommand.Parameters.AddWithValue("StartDate", Startdate)
                SqlCommand.Parameters.AddWithValue("EndDate", EndDate)
                SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
                SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If




        Catch ex As Exception
            GetValuesVintageTab = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try
        Return TempTable
    End Function

    '''' <summary>
    '''' This function grabs the values tab.
    '''' </summary>
    '''' <param name="amod_id"></param>
    '''' <param name="forsaleFlag"></param>
    '''' <param name="yearOne"></param>
    '''' <param name="yearTwo"></param>
    '''' <param name="afttStart"></param>
    '''' <param name="afttEnd"></param>
    '''' <param name="regType"></param>
    '''' <param name="Startdate"></param>
    '''' <param name="EndDate"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    Public Function CheckGetValuesVintageTab(ByVal amod_id As Long, ByVal forsaleFlag As String, ByVal yearOne As String, ByVal yearTwo As String, ByVal afttStart As String, ByVal afttEnd As String, ByVal regType As String, ByVal Startdate As String, ByVal EndDate As String, ByVal variantListString As String) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim query As String
        Try

            If amod_id <> 0 Then

                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()

                query = "SELECT distinct count(journ_id) as tcount"

                query += " From Aircraft a with (NOLOCK) "
                query += " inner join Journal with (NOLOCK) on journ_id = ac_journ_id And ac_id = journ_ac_id "
                query += " where journ_subcat_code_part1='WS' AND journ_internal_trans_flag='N' "
                query += " and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM') "

                If Not String.IsNullOrEmpty(variantListString) Then
                    query += " and ac_amod_id in (" & amod_id & "," & variantListString & ")"
                Else
                    query += " and ac_amod_id = @amodID"
                End If

                If forsaleFlag = "Y" Then
                    query += " and ac_forsale_flag = 'Y' "
                End If

                '-- ADD TRANSACTION DATE RANGE
                query += " and journ_date between @StartDate and @EndDate"
                '-- YEAR RANGE
                query += " and ac_year between @yearOne and @yearTwo"
                '-- WITH OR WITHOUT SALE PRICES

                'reg Type
                If regType = "N" Then
                    query += " and ac_reg_no like 'N%' "
                ElseIf regType = "I" Then
                    query += " and ac_reg_no not like 'N%' "
                End If

                '-- AFTT
                query += " and ac_airframe_tot_hrs between @startAFTT and @endAFTT"



                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)
                SqlCommand.Parameters.AddWithValue("amodID", amod_id)
                SqlCommand.Parameters.AddWithValue("yearOne", yearOne)
                SqlCommand.Parameters.AddWithValue("yearTwo", yearTwo)
                SqlCommand.Parameters.AddWithValue("StartDate", Startdate)
                SqlCommand.Parameters.AddWithValue("EndDate", EndDate)
                SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
                SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If




        Catch ex As Exception
            CheckGetValuesVintageTab = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try
        Return TempTable
    End Function
    ''' <summary>
    ''' This function grabs the values by AFTT tab. ///Stand in function for the real one.
    ''' </summary>
    ''' <param name="amod_id"></param>
    ''' <param name="forsaleFlag"></param>
    ''' <param name="yearOne"></param>
    ''' <param name="yearTwo"></param>
    ''' <param name="afttStart"></param>
    ''' <param name="afttEnd"></param>
    ''' <param name="regType"></param>
    ''' <param name="Startdate"></param>
    ''' <param name="EndDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetValuesTrendsByAFTT(ByVal amod_id As Long, ByVal forsaleFlag As String, ByVal yearOne As String, ByVal yearTwo As String, ByVal afttStart As String, ByVal afttEnd As String, ByVal regType As String, ByVal Startdate As String, ByVal EndDate As String, ByVal variantListString As String) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim query As String
        Try

            If amod_id <> 0 Then

                TempTable.Columns.Add("AFTT")
                TempTable.Columns.Add("AFTTSORT", System.Type.GetType("System.Int64"))

                TempTable.Columns.Add("SALECOUNT")
                TempTable.Columns.Add("LOWASKING")
                TempTable.Columns.Add("AVGASKING")
                TempTable.Columns.Add("HIGHASKING")
                TempTable.Columns.Add("LOWSALE")
                TempTable.Columns.Add("AVGSALE")
                TempTable.Columns.Add("HIGHSALE")
                TempTable.Columns.Add("COUNTASKING")
                TempTable.Columns.Add("SUMASKING")
                TempTable.Columns.Add("COUNTSALE")
                TempTable.Columns.Add("SUMSALE")



                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()

                query = "SELECT "
                Dim AFTTQuery As String = "case "

                Dim CeilingAFTT As Long = (Math.Ceiling(aftt_end.Text / 1000) * 1000)

                For x As Integer = 0 To CeilingAFTT Step 1000
                    If x = CeilingAFTT Then
                        AFTTQuery += " when ac_airframe_tot_hrs >= " & x & " and ac_airframe_tot_hrs <= " & x + 1000 & " then '" & x & " - " & (x + 1000) & "' "
                    Else
                        If x = 0 Then
                            AFTTQuery += " when ac_airframe_tot_hrs >= 1 and ac_airframe_tot_hrs < 1000 then '1 - 999' "
                        Else
                            AFTTQuery += " when ac_airframe_tot_hrs >= " & x & " and ac_airframe_tot_hrs < " & x + 1000 & " then '" & x & " - " & (x + 1000) - 1 & "' "
                        End If
                    End If
                Next

                AFTTQuery += " end "
                query += AFTTQuery & " as AFTT"
                query += " ,  "
                query += " count(distinct journ_id) as SALECOUNT,"
                query += " MIN(ac_asking_price) as LOWASKING, "
                query += " AVG(ac_asking_price) AS AVGASKING,"
                query += " MAX(ac_asking_price) as HIGHASKING,"
                query += " MIN(ac_sale_price) AS LOWSALE,"
                query += " AVG(ac_sale_price) AS AVGSALE,"
                query += " max(ac_sale_price) AS HIGHSALE,"
                query += " SUM(case when ac_asking_price > 0 then 1 else 0 end) as COUNTASKING, "
                query += " SUM(ac_asking_price) as SUMASKING, "
                query += " SUM(case when ac_sale_price > 0 then 1 else 0 end) as COUNTSALE, "
                query += " SUM(ac_sale_price) as SUMSALE"

                query += " From Aircraft a with (NOLOCK) "
                query += " inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id "
                query += " where journ_subcat_code_part1='WS' AND journ_internal_trans_flag='N' "
                query += " and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM') "


                If Not String.IsNullOrEmpty(variantListString) Then
                    query += " and ac_amod_id in (" & amod_id & "," & variantListString & ")"
                Else
                    query += " and ac_amod_id = @amodID"
                End If


                '-- ADD TRANSACTION DATE RANGE
                query += " and journ_date between @StartDate and @EndDate"
                '-- YEAR RANGE
                query += " and ac_year between @yearOne and @yearTwo"
                '-- WITH OR WITHOUT SALE PRICES

                'reg Type
                If regType = "N" Then
                    query += " and ac_reg_no like 'N%' "
                ElseIf regType = "I" Then
                    query += " and ac_reg_no not like 'N%' "
                End If

                '-- AFTT
                query += " and ac_airframe_tot_hrs between @startAFTT and @endAFTT"

                query += utilization_view_functions.add_client_ac_string(False)

                query += " group by  ac_amod_id, "
                query += "( " & AFTTQuery & " )"



                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)
                SqlCommand.Parameters.AddWithValue("amodID", amod_id)
                SqlCommand.Parameters.AddWithValue("yearOne", yearOne)
                SqlCommand.Parameters.AddWithValue("yearTwo", yearTwo)
                SqlCommand.Parameters.AddWithValue("StartDate", Startdate)
                SqlCommand.Parameters.AddWithValue("EndDate", EndDate)
                SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
                SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                While SqlReader.Read()

                    Dim newRow As DataRow = TempTable.NewRow()
                    newRow("AFTT") = SqlReader.Item("AFTT")

                    Dim SortAFTT As Long = 0
                    If Not IsDBNull(SqlReader.Item("AFTT")) Then
                        Dim SortStringAFTTArray As Array = Split(SqlReader.Item("AFTT"), "-")
                        If UBound(SortStringAFTTArray) = 1 Then
                            If IsNumeric(Trim(SortStringAFTTArray(0))) Then
                                SortAFTT = Trim(SortStringAFTTArray(0))
                            End If
                        End If
                    End If

                    newRow("AFTTSORT") = SortAFTT
                    newRow("SALECOUNT") = SqlReader.Item("SALECOUNT")
                    newRow("LOWASKING") = SqlReader.Item("LOWASKING")
                    newRow("AVGASKING") = SqlReader.Item("AVGASKING")
                    newRow("HIGHASKING") = SqlReader.Item("HIGHASKING")
                    newRow("LOWSALE") = SqlReader.Item("LOWSALE")
                    newRow("AVGSALE") = SqlReader.Item("AVGSALE")
                    newRow("HIGHSALE") = SqlReader.Item("HIGHSALE")
                    newRow("COUNTASKING") = SqlReader.Item("COUNTASKING")
                    newRow("SUMASKING") = SqlReader.Item("SUMASKING")
                    newRow("COUNTSALE") = SqlReader.Item("COUNTSALE")
                    newRow("SUMSALE") = SqlReader.Item("SUMSALE")

                    TempTable.Rows.Add(newRow)
                    TempTable.AcceptChanges()
                End While

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If

            Dim SortView As New DataView(TempTable)
            SortView.Sort = "AFTTSORT asc"
            TempTable = SortView.ToTable


        Catch ex As Exception
            GetValuesTrendsByAFTT = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try
        Return TempTable
    End Function

    ''' <summary>
    ''' ''' This function grabs the values by quarter tab.
    ''' </summary>
    ''' <param name="amod_id"></param>
    ''' <param name="forsaleFlag"></param>
    ''' <param name="yearOne"></param>
    ''' <param name="yearTwo"></param>
    ''' <param name="afttStart"></param>
    ''' <param name="afttEnd"></param>
    ''' <param name="regType"></param>
    ''' <param name="Startdate"></param>
    ''' <param name="EndDate"></param>
    ''' <param name="allSale"></param>
    ''' <param name="allAsking"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetValuesTrendsByQuarter(ByVal amod_id As Long, ByVal forsaleFlag As String, ByVal yearOne As String, ByVal yearTwo As String, ByVal afttStart As String, ByVal afttEnd As String, ByVal regType As String, ByVal Startdate As String, ByVal EndDate As String, ByVal allSale As Boolean, ByVal allAsking As Boolean, ByVal variantListString As String) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim query As String
        Try

            If amod_id <> 0 Then

                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()

                query = "SELECT DATEPART(year, journ_date) As YearSld, "
                query += " DATEPART(quarter, journ_date) As QuarterSld, "
                query += " AVG(CAST(ac_mfr_year AS INT)) As dAvgYearMfr, "
                query += " AVG(CAST(ac_year AS INT)) As dAvgYearDlv, "
                query += " AVG(ac_asking_price) As dAvgAsking, "
                query += " AVG(ac_hidden_asking_price) As dAvgAskingHidden, "
                query += " AVG(ac_sale_price) As dAvgSelling, "
                query += " ((AVG(ac_sale_price)/AVG(ac_asking_price)) * 100) As dPercent, "
                query += " ((1-(AVG(ac_sale_price)/AVG(ac_asking_price))) * 100) As dVariance, "
                query += " ((AVG(ac_sale_price)/AVG(ac_hidden_asking_price)) * 100) As dPercentHidden, "
                query += " ((1-(AVG(ac_sale_price)/AVG(ac_hidden_asking_price))) * 100) As dVarianceHidden, "
                query += " AVG(ac_airframe_tot_hrs) As dAvgAFTT, "
                query += " AVG(DateDiff(day,ac_list_date, journ_date)) As dAvgDOM "
                query += " FROM Aircraft WITH (NOLOCK) "
                query += " inner join Aircraft_Model with (NOLOCK) on ac_amod_id = amod_id"
                query += " inner join Journal with (NOLOCK) on journ_id = ac_journ_id and journ_ac_id = ac_id"
                query += " WHERE (ac_journ_id > 0) AND NOT (journ_subcat_code_part3 IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')) "
                query += " AND (journ_subcat_code_part1 = 'WS') AND (journ_internal_trans_flag = 'N') "

                'Add Model

                If Not String.IsNullOrEmpty(variantListString) Then
                    query += " and amod_id in (" & amod_id & "," & variantListString & ")"
                Else
                    query += " and amod_id = @amodID "
                End If


                If allSale = False Then
                    query += " AND (ac_sale_price IS NOT NULL) AND (ac_sale_price <> 0) "
                End If

                If allAsking = False Then
                    query += " AND (ac_asking_price IS NOT NULL) AND (ac_asking_price <> 0) "
                End If

                'reg Type
                If regType = "N" Then
                    query += " and ac_reg_no like 'N%' "
                ElseIf regType = "I" Then
                    query += " and ac_reg_no not like 'N%' "
                End If

                'Add flags:
                query += clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True)

                '-- AFTT
                query += " and ac_airframe_tot_hrs between @startAFTT and @endAFTT"

                ''-- ADD TRANSACTION DATE RANGE
                query += " and journ_date between @StartDate and @EndDate "

                If Month(Startdate) = 12 Or Month(Startdate) = 11 Or Month(Startdate) = 10 Then
                    Startdate = "01/01/" & (Year(Startdate) + 1)
                ElseIf Month(Startdate) = 7 Or Month(Startdate) = 8 Or Month(Startdate) = 9 Then
                    Startdate = "10/01/" & Year(Startdate)
                ElseIf Month(Startdate) = 4 Or Month(Startdate) = 5 Or Month(Startdate) = 6 Then
                    Startdate = "07/01/" & Year(Startdate)
                Else
                    Startdate = "04/01/" & Year(Startdate)
                End If

                If Month(EndDate) = 12 Or Month(EndDate) = 11 Or Month(EndDate) = 10 Then
                    EndDate = "01/01/" & (Year(EndDate) + 1)
                ElseIf Month(EndDate) = 7 Or Month(EndDate) = 8 Or Month(EndDate) = 9 Then
                    EndDate = "10/01/" & Year(EndDate)
                ElseIf Month(Startdate) = 4 Or Month(EndDate) = 5 Or Month(EndDate) = 6 Then
                    EndDate = "07/01/" & Year(EndDate)
                Else
                    EndDate = "04/01/" & Year(EndDate)
                End If

                '-- YEAR RANGE
                'query += " AND (DATEPART(year,journ_date) >= @yearSoldStart) AND (DATEPART(year,journ_date) <= @yearSoldEnd) "
                query += " and ac_year between @yearOne and @yearTwo"

                query += " GROUP BY DATEPART(year, journ_date), DATEPART(quarter, journ_date) "
                query += " ORDER BY DATEPART(year, journ_date) ASC, DATEPART(quarter, journ_date) ASC"

                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)
                SqlCommand.Parameters.AddWithValue("amodID", amod_id)
                SqlCommand.Parameters.AddWithValue("yearOne", yearOne)
                SqlCommand.Parameters.AddWithValue("yearTwo", yearTwo)
                SqlCommand.Parameters.AddWithValue("StartDate", Startdate)
                SqlCommand.Parameters.AddWithValue("EndDate", EndDate)
                'SqlCommand.Parameters.AddWithValue("yearSoldStart", Year(Startdate))
                'SqlCommand.Parameters.AddWithValue("yearSoldEnd", Year(EndDate))
                SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
                SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If




        Catch ex As Exception
            GetValuesTrendsByQuarter = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try
        Return TempTable
    End Function
    Public Function GetFeaturesListByModel(ByVal amod_id As Long) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim query As String
        Try

            If amod_id <> 0 Then

                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()

                query = " select amod_make_name, amod_model_name, amod_id,"

                query += " STUFF(("
                query += " SELECT ','+cast(amodg_model_id as varchar(10))"
                query += " FROM Aircraft_Model_Group AS AMG1"
                query += " WHERE (AMG1.amodg_group_type = 'VARIANT')"
                query += " AND (AMG1.amodg_group_name = (SELECT TOP 1 AMG2.amodg_group_name "
                query += " FROM Aircraft_Model_Group AS AMG2"
                query += " WHERE (AMG2.amodg_model_id = Aircraft_Model.amod_id ) "
                query += " AND (AMG2.amodg_group_type = 'VARIANT')"
                query += " )"
                query += " )"
                query += " and AMG1.amodg_model_id <> Aircraft_Model.amod_id "
                query += " FOR XML PATH('')"
                query += " ),1,1,'') as ModelVariants, "

                '-- GET KEY FEATURE LIST
                query += " STUFF(("
                query += " select ','+kfeat_aircraft_flat_name+' as '+ '''' + kfeat_code + '''' from Key_Feature with (NOLOCK)"
                query += " inner join Aircraft_Model_Key_Feature with (NOLOCK) on kfeat_code=amfeat_feature_code "
                query += " where amfeat_amod_id = Aircraft_Model.amod_id and amfeat_standard_equip <> 'Y' and kfeat_aircraft_flat_name <> ''"
                query += " FOR XML PATH('')"
                query += " ),1,1,'') as FEATURES"
                query += " from Aircraft_Model with(NOLOCK)"

                query += " WHERE (amod_id = @amodID)"


                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)
                SqlCommand.Parameters.AddWithValue("amodID", amod_id)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If




        Catch ex As Exception
            GetFeaturesListByModel = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try
        Return TempTable
    End Function
    ''' <summary>
    ''' This function grabs the values by weight tab. ///Stand in function for the real one.
    ''' </summary>
    ''' <param name="amod_id"></param>
    ''' <param name="forsaleFlag"></param>
    ''' <param name="yearOne"></param>
    ''' <param name="yearTwo"></param>
    ''' <param name="afttStart"></param>
    ''' <param name="afttEnd"></param>
    ''' <param name="regType"></param>
    ''' <param name="Startdate"></param>
    ''' <param name="EndDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetValuesTrendsByWeight(ByVal amod_id As Long, ByVal forsaleFlag As String, ByVal yearOne As String, ByVal yearTwo As String, ByVal afttStart As String, ByVal afttEnd As String, ByVal regType As String, ByVal Startdate As String, ByVal EndDate As String, ByVal AirframeTypeCode As String, ByVal AmodTypeCode As String, ByVal AmodWeightClass As String) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim query As String
        Try

            If amod_id <> 0 Then

                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()
                query = " SELECT amod_id As AModId, amod_make_name As Make, "
                query += " amod_make_abbrev As MakeAbbrev, "
                query += " amod_model_name As Model, "
                query += "AVG(CAST(ac_mfr_year AS INT)) As dAvgYearMfr,"
                query += " AVG(CAST(ac_year AS INT)) As dAvgYearDlv,"
                query += " AVG(ac_asking_price) As dAvgAsking, "
                query += "AVG(ac_hidden_asking_price) As dAvgAskingHidden,"
                query += " AVG(ac_sale_price) As dAvgSelling,"
                query += " ((AVG(ac_sale_price)/AVG(ac_asking_price)) * 100) As dPercent,"
                query += " ((1-(AVG(ac_sale_price)/AVG(ac_asking_price))) * 100) As dVariance, "
                query += "((AVG(ac_sale_price)/AVG(ac_hidden_asking_price)) * 100) As dPercentHidden, "
                query += "((1-(AVG(ac_sale_price)/AVG(ac_hidden_asking_price))) * 100) As dVarianceHidden, "
                query += "AVG(ac_airframe_tot_hrs) As dAvgAFTT, "
                query += " AVG(DateDiff(day,ac_list_date, journ_date)) As dAvgDOM "

                query += " FROM Aircraft WITH (NOLOCK) "
                query += " inner join Aircraft_Model with (NOLOCK) on ac_amod_id = amod_id"
                query += " inner join Journal with (NOLOCK) on journ_id = ac_journ_id and journ_ac_id = ac_id"

                query += " WHERE (ac_journ_id > 0) "
                query += " AND NOT (journ_subcat_code_part3 IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')) "
                query += "AND (journ_subcat_code_part1 = 'WS') AND "
                query += " (journ_internal_trans_flag = 'N') AND (amod_id <> @amodID) AND "
                query += " (amod_customer_flag = 'Y' AND amod_airframe_type_code=@AirframeTypeCode AND amod_type_code = @AmodTypeCode) AND (amod_weight_class = @AmodWeightClass) "
                query += " AND (ac_sale_price IS NOT NULL) AND (ac_sale_price <> 0) AND (ac_asking_price IS NOT NULL) AND (ac_asking_price <> 0) "
                'query += " AND (DATEPART(year,journ_date) >= 2016) AND (DATEPART(quarter,journ_date) = 1) "

                '-- ADD TRANSACTION DATE RANGE
                query += " and journ_date between @StartDate and @EndDate"
                '-- YEAR RANGE
                query += " and ac_year between @yearOne and @yearTwo"
                '-- WITH OR WITHOUT SALE PRICES

                'reg Type
                If regType = "N" Then
                    query += " and ac_reg_no like 'N%' "
                ElseIf regType = "I" Then
                    query += " and ac_reg_no not like 'N%' "
                End If

                '-- AFTT
                query += " and ac_airframe_tot_hrs between @startAFTT and @endAFTT"

                query += " GROUP BY amod_id, amod_make_name, amod_make_abbrev, amod_model_name ORDER BY amod_make_name, amod_model_name asc"



                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)
                SqlCommand.Parameters.AddWithValue("amodID", amod_id)
                SqlCommand.Parameters.AddWithValue("yearOne", yearOne)
                SqlCommand.Parameters.AddWithValue("yearTwo", yearTwo)
                SqlCommand.Parameters.AddWithValue("StartDate", Startdate)
                SqlCommand.Parameters.AddWithValue("EndDate", EndDate)
                SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
                SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
                SqlCommand.Parameters.AddWithValue("AirframeTypeCode", AirframeTypeCode)
                SqlCommand.Parameters.AddWithValue("AmodTypeCode", AmodTypeCode)
                SqlCommand.Parameters.AddWithValue("AmodWeightClass", AmodWeightClass)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If




        Catch ex As Exception
            GetValuesTrendsByWeight = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try
        Return TempTable
    End Function

    Private Sub BuildWeightGraphs(ByRef Graph1 As String, ByRef Graph2 As String)
        Dim CallGraphString As String = ""
        If Not Page.ClientScript.IsClientScriptBlockRegistered("refreshWGraphs") Then
            Graph1 = " var dataGraphWeight1 = new google.visualization.DataTable(); " &
            " dataGraphWeight1.addColumn('string', 'Make/Model(s)'); " &
            " dataGraphWeight1.addColumn('number', 'Percentage');" &
            " dataGraphWeight1.addRows([" & Graph1 & "]);" &
            " var optionsWeight1 = {'title':'','width':" & IIf(Session.Item("isMobile"), "300", "480") & ",'height':230,legend: { position: 'none' },bar: {groupWidth: '75%'}, colors: ['blue'], 'chartArea': {top:25}, hAxis: { textStyle:{fontSize:'9'}},vAxis: { title: ''} , series: {    0: { lineWidth: 0, pointSize: 3  }  } };"
            Graph1 += "var chartWeight1 = new google.visualization.ColumnChart(document.getElementById('graphWeight1Div'));"
            Graph1 += "chartWeight1.draw(dataGraphWeight1, optionsWeight1);"


            Graph2 = " var dataGraphWeight2 = new google.visualization.DataTable(); " &
            " dataGraphWeight2.addColumn('string', 'Make/Model(s)'); " &
            " dataGraphWeight2.addColumn('number', 'Variance');" &
            " dataGraphWeight2.addRows([" & Graph2 & "]);" &
            " var optionsWeight2 = {'title':'','width':" & IIf(Session.Item("isMobile"), "300", "480") & ",'height':230,legend: { position: 'none' },bar: {groupWidth: '75%'}, colors: ['blue'], 'chartArea': {top:25}, hAxis: { textStyle:{fontSize:'9'}},vAxis: { title: ''} , series: {    0: { lineWidth: 0, pointSize: 3  }  } };"
            Graph2 += "var chartWeight2 = new google.visualization.ColumnChart(document.getElementById('graphWeight2Div'));"
            Graph2 += "chartWeight2.draw(dataGraphWeight2, optionsWeight2);"




            CallGraphString = "function DrawWeightGraphs() {"
            CallGraphString += Graph1
            CallGraphString += Graph2
            CallGraphString += "} ;DrawWeightGraphs()"

            ''First load needs to run it after google loads.
            System.Web.UI.ScriptManager.RegisterStartupScript(Me.tabs_bottom_6_update_panel, Me.GetType, "refreshWGraphs", "" & CallGraphString & ";", True)
        End If
    End Sub

    ''' <summary>
    ''' Grabs the min/max slider values
    ''' </summary>
    ''' <param name="amod_id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAircraftSliderValues(ByVal amod_id As Long, ByVal variantIDs As String) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim query As String
        Try

            If amod_id <> 0 Then


                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()


                query = "select MIN(ac_year) as MINYEAR, MAX(ac_year) AS MAXYEAR, "
                query += " MAX(ac_est_airframe_hrs) AS MAXAFTT, MAX(afmv_airframe_hrs) as MAXAFTT_PROJ "

                query += " from Aircraft_Flat with (NOLOCK)"
                query += " left outer join Aircraft_FMV on afmv_ac_id = ac_id and afmv_latest_flag = 'Y' and afmv_status = 'Y' "

                If Not String.IsNullOrEmpty(variantIDs) Then
                    query += " where amod_id in (" & amod_id & "," & variantIDs & ") "
                Else
                    query += " where amod_id = @amodID "
                End If


                query += clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True)
                query += " and ac_journ_id = 0"



                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)
                Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)


                SqlCommand.Parameters.AddWithValue("amodID", amod_id)


                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing

            End If
            Return TempTable
        Catch ex As Exception
            GetAircraftSliderValues = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing



        End Try

    End Function

    ''' <summary>
    ''' Grabs the min/max slider values
    ''' </summary>
    ''' <param name="acid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAircraftValueHistoryJetnet(ByVal acID As Long) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim query As String
        Try

            If acID <> 0 Then


                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()

                query = "select ac_asking_price as asking_price, 0 as take_price, ac_asking, "
                query += " case when ac_sale_price_display_flag='Y' then ac_sale_price else 0 end as sold_price, "
                query += " journ_date as date_of, "
                query += " journ_subject as description, 'JETNET' as Data_Source "
                query += " from Aircraft with (NOLOCK) "
                query += " inner join journal with (NOLOCK) on ac_id = journ_ac_id and ac_journ_id = journ_id "
                query += " WHERE(journ_ac_id = @acID)"
                query += " and journ_subcategory_code like 'WS%' and journ_internal_trans_flag='N' "
                query += " order by journ_date asc"



                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)
                Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)


                SqlCommand.Parameters.AddWithValue("acID", acID)


                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing

            End If
            Return TempTable
        Catch ex As Exception
            GetAircraftValueHistoryJetnet = Nothing
            'Me.class_error = "Error in GetAircraftValueHistoryJetnet(ByVal acID As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing



        End Try

    End Function

    ''' <summary>
    ''' Gets Base Values for Aircraft if you come in with an ID.
    ''' </summary>
    ''' <param name="ac_id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAircraftBaseValues(ByVal ac_id As Long) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim query As String
        Try

            If ac_id <> 0 Then


                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()


                query = "select ac_est_airframe_hrs, ac_picture_id, "
                query += "ac_id, ac_upd_date, ac_foreign_currency_name, ac_reg_no_expiration_date, ac_journ_id, ac_reg_no_search, ac_delivery_date, ac_lifecycle_stage as ac_lifecycle, "
                query += "ac_ownership_type as ac_ownership, ac_use_code as ac_usage, ac_ser_no_full as ac_ser_nbr, amod_model_name, amod_make_name,"
                query += "ac_alt_ser_no as ac_alt_ser_nbr, ac_reg_no as ac_reg_nbr, ac_alt_ser_no_full, "
                query += "ac_prev_reg_no as ac_prev_reg_nbr, ac_country_of_registration, (select case when ac_previously_owned_flag='Y' then 'N' else 'Y' end) as ac_new_flag,ac_previously_owned_flag, "
                query += " ac_mfr_year as ac_year_mfr, ac_year as ac_year_dlv, ac_purchase_date as ac_date_purchased, "
                query += " ac_forsale_flag, ac_foreign_currency_price, ac_list_date as ac_date_listed,ac_status, ac_delivery, "
                query += " ac_exclusive_flag,  ac_asking as ac_asking_wordage, "
                query += " ac_asking_price, ac_lease_flag, "
                query += " ac_airframe_tot_hrs as ac_airframe_total_hours,  ac_airframe_tot_landings as ac_airframe_total_landings, "
                query += " ac_times_as_of_date as ac_date_engine_times_as_of, ac_aport_iata_code, ac_aport_icao_code, "
                query += " ac_aport_name, ac_aport_state,ac_aport_country, ac_aport_city, "
                query += " ac_aport_private, "
                query += " ac_confidential_notes,  ac_action_date, "
                query += " ac_maintained as ac_maintained, "
                query += " ac_aport_faaid_code "
                query += " from Aircraft_Flat with (NOLOCK)"
                query += " where ac_id = @acID"
                query += " and ac_journ_id = 0"



                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)
                Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)


                SqlCommand.Parameters.AddWithValue("acID", ac_id)


                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing

            End If
            Return TempTable
        Catch ex As Exception
            GetAircraftBaseValues = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing
        End Try

    End Function

    ''' <summary>
    ''' Grabs the sales starting table.
    ''' </summary>
    ''' <param name="amod_id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTransactionAircraftStartingTable(ByVal amod_id As Long, ByVal idList As String, ByVal variantListString As String, ByRef has_client_data As Boolean) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim query As String
        Try

            If amod_id <> 0 Then

                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()


                query = "SELECT amod_airframe_type_code, amod_type_code, amod_weight_class, ac_ser_no,ac_ser_no_full, ac_reg_no,journ_newac_flag, ac_mfr_year, ac_year, journ_date, journ_subject, amod_make_name, amod_model_name,"
                query += "	case when journ_newac_flag = 'Y' then 'NEWFIRSTOWNER' when (select top 1 j.journ_newac_flag from Journal j with (NOLOCK) where j.journ_ac_id = ac_id and j.journ_id < journal.journ_id and j.journ_newac_flag = 'Y' order by j.journ_id desc) = 'Y' then 'USED' when ac_previously_owned_flag = 'Y' then 'USED' else 'UNKNOWN' end as NEWUSED, "
                query += " ac_asking_price,  case when ac_asking IS NULL then '' else ac_asking end as ac_asking ,"

                query += " ac_engine_1_soh_hrs, ac_engine_2_soh_hrs, "

                If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
                    query += " case when ac_sale_price > 0 and ac_sale_price_display_flag = 'Y' then ac_sale_price else '' end as ac_sold_price, "
                Else
                    query += " '' as ac_sold_price, "
                End If
                If FeaturesList <> "" Then
                    query += FeaturesList & ","
                End If
                query += " case amp_program_name when 'Unknown' then '' when 'Confirmed to be on a maintenance program' then 'Confirmed' when 'Confirmed not on any maintenance program' then 'Confirmed Not' else amp_program_name end as APROG,"
                query += " case emp_program_name when 'Unknown' then '' when 'Confirmed to be on a maintenance program' then 'Confirmed' when 'Confirmed not on any maintenance program' then 'Confirmed Not' else emp_program_name end as EPROG, "
                query += " ac_engine_1_tot_hrs, ac_maintained, "
                query += " ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs, ac_interior_moyear, ac_exterior_moyear, "
                query += " ac_passenger_count, "
                query += " ac_sale_price_display_flag, ac_airframe_tot_hrs, ac_est_airframe_hrs, "
                query += " ac_ser_no_sort, ac_id, ac_forsale_flag,journ_id, journ_subcategory_code, "
                query += " ac_list_date, "
                query += " journ_customer_note "
                query += " From Aircraft_Flat with (NOLOCK)"
                query += " inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id"
                query += " LEFT outer join Aircraft_Features_Flat on ac_id = afeat_ac_id and ac_journ_id = afeat_journ_id"
                query += " where  journ_subcat_code_part1='WS'"
                query += " AND journ_date >= '01/01/" & Year(DateAdd(DateInterval.Year, IIf(Not Session.Item("isMobile"), -2, -1), Now())) & "' "
                query += " AND journ_internal_trans_flag='N' "
                query += " and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')"

                query += clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True)


                If Not String.IsNullOrEmpty(variantListString) Then
                    query += " and amod_id in (" & amod_id & "," & variantListString & ")"
                Else
                    query += " and amod_id = @amodID"
                End If



                If idList <> "" Then
                    query += " and journ_id in (" & idList & ") "
                End If

                query += " order by journ_date desc"

                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)
                SqlCommand.Parameters.AddWithValue("amodID", amod_id)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If



            Call utilization_view_functions.Get_Client_AC_Models(amod_id, Me.start_date.Text, Me.end_date.Text)


            Return TempTable
        Catch ex As Exception
            GetTransactionAircraftStartingTable = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing
        End Try

    End Function




    '''' <summary>
    '''' The function that writes the sales table out.
    '''' </summary>
    '''' <param name="dt"></param>
    '''' <remarks></remarks>
    Public Sub WriteTransactionAircraftStartingTableJS(ByVal dt As DataTable)
        Dim results As String = ""
        Dim JournalSubjectNote As String = ""
        Dim HtmlOut As New StringBuilder
        Dim i As Integer = 0
        Dim temp_asking As Integer = 0
        Dim temp_sold As Integer = 0
        'Column 1: Checkbox
        'Column 2: Serial #
        'Column 3: Reg #
        'Column 4: Year MFR
        'Column 5: Date
        'Column 6: Transaction Info
        'Column 7: AFTT
        'Column 8: Asking
        'Column 9: Sold
        'Column 10: Date Listed
        'Column 11: For Sale
        'Column 12: New AC Flag
        'Column 13: ID
        Dim FinalFeatureArray As Array = BuildFeatureArray()
        If Not IsNothing(dt) Then
            For Each r As DataRow In dt.Rows
                JournalSubjectNote = ""

                If Trim(HtmlOut.ToString.Trim) <> "" Then
                    HtmlOut.Append(",")
                End If
                HtmlOut.Append("{")

                HtmlOut.Append("""check"": """",")
                ' HtmlOut.append( """ser"": [""" & Replace(Replace(DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, True, r("ac_ser_no_full").ToString, "", ""), """", "\"""), "'", "\'") & """, """ & r("ac_ser_no_sort").ToString & """],"
                HtmlOut.Append("""ser"": [""" & Replace(Replace("<a " & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, False, "", "", "") & " title=""" & r("amod_make_name") & " " & r("amod_model_name") & " S/N #" & r("ac_ser_no_full") & """>" & r("ac_ser_no_full").ToString & "</a>", """", "\"""), "'", "\'") & """,   """ & r("ac_ser_no_sort").ToString & """],")
                HtmlOut.Append(vbNewLine)
                HtmlOut.Append("""reg"": """ & r("ac_reg_no").ToString & """,")
                HtmlOut.Append(vbNewLine)
                HtmlOut.Append("""dlv"": """ & r("ac_year").ToString & """,")
                HtmlOut.Append(vbNewLine)

                For CountOfFeatures = 0 To UBound(FinalFeatureArray)
                    HtmlOut.Append("""" & Replace(FinalFeatureArray(CountOfFeatures), "'", "") & """: """ & r(Replace(FinalFeatureArray(CountOfFeatures), "'", "")).ToString & """,")
                    HtmlOut.Append(vbNewLine)
                Next

                HtmlOut.Append("""jdate"": """)
                If Not IsDBNull(r("journ_date")) Then
                    HtmlOut.Append(Format(r("journ_date"), "MM/dd/yy"))
                End If
                HtmlOut.Append(""",")
                HtmlOut.Append(vbNewLine)
                'Set up the subject
                If Not IsDBNull(r("journ_subject")) Then
                    If Not String.IsNullOrEmpty(r("journ_subject")) Then
                        JournalSubjectNote = Server.HtmlEncode(Left(r.Item("journ_subject").ToString, 90).ToString)
                    End If
                End If

                ''The note
                If Not IsDBNull(r("journ_customer_note")) Then
                    If Not String.IsNullOrEmpty(r.Item("journ_customer_note")) Then
                        JournalSubjectNote += "&nbsp;&nbsp;(<span class='help_cursor error_text no_text_underline' title='" + Server.HtmlEncode(Trim(r.Item("journ_customer_note").ToString).Replace(vbCrLf, "")) + "'>Note</span>)"
                    End If
                End If
                HtmlOut.Append("""info"": """)
                HtmlOut.Append(Replace(Replace(DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, r("journ_id"), True, JournalSubjectNote, "", ""), """", "\"""), "'", "\'") & """,")
                HtmlOut.Append(vbNewLine)


                ' changed MSW 
                HtmlOut.Append("""aftt"": """ & r("ac_est_airframe_hrs").ToString & """,")
                '  HtmlOut.Append("""aftt"": """ & r("ac_airframe_tot_hrs").ToString & """,")
                HtmlOut.Append(vbNewLine)


                HtmlOut.Append("""ett"":""")

                If Not IsDBNull(r("ac_engine_1_tot_hrs")) Then
                    If CDbl(r.Item("ac_engine_1_tot_hrs").ToString) = 0 Then
                        HtmlOut.Append("[0]&nbsp;")
                    Else
                        HtmlOut.Append("[" + r.Item("ac_engine_1_tot_hrs").ToString + "]&nbsp;")
                    End If
                Else
                    HtmlOut.Append("[U]&nbsp;")
                End If

                If Not IsDBNull(r("ac_engine_2_tot_hrs")) Then
                    If CDbl(r.Item("ac_engine_2_tot_hrs").ToString) = 0 Then
                        HtmlOut.Append("[0]&nbsp;")
                    Else
                        HtmlOut.Append("[" + r.Item("ac_engine_2_tot_hrs").ToString + "]&nbsp;")
                    End If
                Else
                    HtmlOut.Append("[U]&nbsp;")
                End If

                If Not IsDBNull(r("ac_engine_3_tot_hrs")) Then
                    If CDbl(r.Item("ac_engine_3_tot_hrs").ToString) = 0 Then
                        HtmlOut.Append("[0]&nbsp;")
                    Else
                        HtmlOut.Append("[" + r.Item("ac_engine_3_tot_hrs").ToString + "]&nbsp;")
                    End If
                End If

                If Not IsDBNull(r("ac_engine_4_tot_hrs")) Then
                    If CDbl(r.Item("ac_engine_4_tot_hrs").ToString) = 0 Then
                        HtmlOut.Append("[0]&nbsp;")
                    Else
                        HtmlOut.Append("[" + r.Item("ac_engine_4_tot_hrs").ToString + "]&nbsp;")
                    End If
                End If

                HtmlOut.Append(""",")
                HtmlOut.Append(vbNewLine)



                client_record_found = False
                temp_asking = 0
                temp_sold = 0
                If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True And has_client_data = True Then
                    '------------------------------------- FOR MPM EVO USERS ---------------------------------------------------

                    If Trim(r("ac_id")) = 10288 Then
                        client_record_found = client_record_found
                    End If

                    For i = 0 To 200
                        If Trim(r("ac_id")) = ac_id_array(i) Then
                            client_record_found = True

                            temp_asking = ac_asking_array(i)
                            temp_sold = ac_sold_array(i)
                            i = 200
                        End If
                    Next
                End If

                If client_record_found = True Then
                    '------------------------------------- FOR MPM EVO USERS ---------------------------------------------------

                    HtmlOut.Append("""ask"":[")
                    If r("ac_forsale_flag").ToString = "Y" Or temp_asking > 0 Then

                        If temp_asking > 0 Then
                            HtmlOut.Append("""<span class=\'CLIENTCRMRow\'>$" & FormatNumber((temp_asking / 1000), 0) & "</span>"" , """ & temp_asking & """ ")
                        ElseIf Not IsDBNull(r("ac_asking")) Then
                            If r.Item("ac_asking").ToString.ToLower.Trim.Trim.Contains("price") Then
                                If Not IsDBNull(r("ac_asking_price")) Then
                                    If CDbl(r.Item("ac_asking_price").ToString) > 0 Then
                                        HtmlOut.Append("""<span class=\'CLIENTCRMRow\'>$" & FormatNumber((r("ac_asking_price") / 1000), 0) & "</span>"" , """ & r("ac_asking_price").ToString & """ ")
                                    Else
                                        HtmlOut.Append(""""",""""")
                                    End If
                                Else
                                    HtmlOut.Append(""""",""""")
                                End If
                            Else
                                HtmlOut.Append("""M/O"",""1""")
                                'HtmlOut.append( """" & Replace(Replace(localDataLayer.forsale_status(r("ac_asking").ToString.Trim), """", "\"""), "'", "\'") & """,""1"" "
                            End If
                        Else
                            HtmlOut.Append(""""",""""")
                        End If

                    Else
                        HtmlOut.Append("""<span class=\'CLIENTCRMRow\'>OFFMKT</span>"",""0""")
                    End If
                    HtmlOut.Append("],")
                    HtmlOut.Append(vbNewLine)


                    HtmlOut.Append("""sale"":""")
                    If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
                        If Not IsDBNull(r("ac_sold_price")) Or temp_sold > 0 Then

                            If temp_sold > 0 Then
                                HtmlOut.Append("<span class=\'CLIENTCRMRow\'>$" & FormatNumber((temp_sold / 1000), 0) & "</span>")
                            ElseIf r("ac_sold_price") > 0 Then
                                'We need to keep the space here down below before the sale price.
                                'The javascript filters on this row but it is not treating the image/span as actual content so it doesn't think any of the columns have a sale price.
                                'A way to get past this is by adding the space down below. It does not change the display but pads the data giving a way to tell whether
                                'a sales record has a sale price or not.
                                HtmlOut.Append(" <span unselectable=\'on\' alt=\'Reported Sale Price Displayed with Permission from Source\' title=\'Reported Sale Price Displayed with Permission from Source\'>")
                                HtmlOut.Append(Replace(Replace(DisplayFunctions.TextToImage("$" & FormatNumber((r("ac_sold_price") / 1000), 0), 10, "Arial", IIf(FormatNumber((r("ac_sold_price") / 1000), 0) < 10000, "35px", "43px"), ""), """", "\"""), "'", "\'"))
                                HtmlOut.Append("</span>")
                            Else
                                'HtmlOut.Append("<a href=\'\' onclick=\""javascript:load(\'/SendSalesTransaction.aspx?sendSales=true&ModelID=" & modelList.SelectedValue & "&jID=" & r("journ_id").ToString & "&acid=" & r("ac_id").ToString & "\',\'\',\'scrollbars=yes,menubar=no,height=438,width=800,resizable=yes,toolbar=no,location=no,status=no\');return false;\"" class=\'gray_text\'>ENTER</a>")
                            End If
                        End If
                    End If
                    HtmlOut.Append(""",")
                    HtmlOut.Append(vbNewLine)


                    '------------------------------------- FOR MPM EVO USERS ---------------------------------------------------
                Else
                    '------------------------------------- FOR NORMAL USERS - NON MPM ---------------------------------------------------
                    HtmlOut.Append("""ask"":[")
                    If r("ac_forsale_flag").ToString = "Y" Then
                        If Not IsDBNull(r("ac_asking")) Then
                            If r.Item("ac_asking").ToString.ToLower.Trim.Trim.Contains("price") Then
                                If Not IsDBNull(r("ac_asking_price")) Then
                                    If CDbl(r.Item("ac_asking_price").ToString) > 0 Then
                                        HtmlOut.Append("""$" & FormatNumber((r("ac_asking_price") / 1000), 0) & """ , """ & r("ac_asking_price").ToString & """ ")
                                    Else
                                        HtmlOut.Append(""""",""""")
                                    End If
                                Else
                                    HtmlOut.Append(""""",""""")
                                End If
                            Else
                                HtmlOut.Append("""M/O"",""1""")
                                'HtmlOut.append( """" & Replace(Replace(localDataLayer.forsale_status(r("ac_asking").ToString.Trim), """", "\"""), "'", "\'") & """,""1"" "
                            End If
                        Else
                            HtmlOut.Append(""""",""""")
                        End If
                    Else
                        HtmlOut.Append("""OFFMKT"",""0""")
                    End If
                    HtmlOut.Append("],")
                    HtmlOut.Append(vbNewLine)

                    HtmlOut.Append("""sale"":""")
                    If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
                        If Not IsDBNull(r("ac_sold_price")) Then
                            If r("ac_sold_price") > 0 Then
                                'We need to keep the space here down below before the sale price.
                                'The javascript filters on this row but it is not treating the image/span as actual content so it doesn't think any of the columns have a sale price.
                                'A way to get past this is by adding the space down below. It does not change the display but pads the data giving a way to tell whether
                                'a sales record has a sale price or not.
                                HtmlOut.Append(" <span unselectable=\'on\' alt=\'Reported Sale Price Displayed with Permission from Source\' title=\'Reported Sale Price Displayed with Permission from Source\'>")
                                HtmlOut.Append(Replace(Replace(DisplayFunctions.TextToImage("$" & FormatNumber((r("ac_sold_price") / 1000), 0), 10, "Arial", IIf(FormatNumber((r("ac_sold_price") / 1000), 0) < 10000, "35px", "43px"), ""), """", "\"""), "'", "\'"))
                                HtmlOut.Append("</span>")
                            Else
                                'HtmlOut.Append("<a href=\'\' onclick=\""javascript:load(\'/SendSalesTransaction.aspx?sendSales=true&ModelID=" & modelList.SelectedValue & "&jID=" & r("journ_id").ToString & "&acid=" & r("ac_id").ToString & "\',\'\',\'scrollbars=yes,menubar=no,height=438,width=800,resizable=yes,toolbar=no,location=no,status=no\');return false;\"" class=\'gray_text\'>ENTER</a>")
                            End If
                        End If
                    End If
                    HtmlOut.Append(""",")
                    HtmlOut.Append(vbNewLine)
                    '------------------------------------- FOR NORMAL USERS - NON MPM ---------------------------------------------------
                End If

                HtmlOut.Append("""listdate"":[")
                If Not IsDBNull(r("ac_list_date")) Then
                    HtmlOut.Append(" """ & Format(r("ac_list_date"), "MM/dd/yy") & " "",""" & Format(r("ac_list_date"), "yyyy/MM/dd") & " "" ")
                Else
                    HtmlOut.Append(""""",""""")
                End If
                HtmlOut.Append("],")
                HtmlOut.Append(vbNewLine)



                HtmlOut.Append("""PAX"":""")
                If Not IsDBNull(r("ac_passenger_count")) Then
                    If CDbl(r.Item("ac_passenger_count").ToString) = 0 Then
                        HtmlOut.Append("0&nbsp;")
                    Else
                        HtmlOut.Append(r.Item("ac_passenger_count").ToString + "&nbsp;")
                    End If
                Else
                    HtmlOut.Append("U&nbsp;")
                End If
                HtmlOut.Append(""",")

                HtmlOut.Append("""ENGSOH1"":""")
                If Not IsDBNull(r("ac_engine_1_soh_hrs")) Then
                    If CDbl(r.Item("ac_engine_1_soh_hrs").ToString) = 0 Then
                        HtmlOut.Append("0&nbsp;")
                    Else
                        HtmlOut.Append(r.Item("ac_engine_1_soh_hrs").ToString + "&nbsp;")
                    End If
                Else
                    HtmlOut.Append("0&nbsp;")
                End If
                HtmlOut.Append(""",")

                HtmlOut.Append("""ENGSOH2"":""")
                If Not IsDBNull(r("ac_engine_2_soh_hrs")) Then
                    If CDbl(r.Item("ac_engine_2_soh_hrs").ToString) = 0 Then
                        HtmlOut.Append("0&nbsp;")
                    Else
                        HtmlOut.Append(r.Item("ac_engine_2_soh_hrs").ToString + "&nbsp;")
                    End If
                Else
                    HtmlOut.Append("0&nbsp;")
                End If
                HtmlOut.Append(""",")

                HtmlOut.Append(vbNewLine)
                HtmlOut.Append("""INT"":""")
                If Not String.IsNullOrEmpty(r.Item("ac_interior_moyear").ToString) Then
                    HtmlOut.Append(Left(r.Item("ac_interior_moyear").ToString, 2).Trim)
                    If Not String.IsNullOrEmpty(Left(r.Item("ac_interior_moyear").ToString, 2).Trim) Then
                        HtmlOut.Append("/")
                    End If
                    HtmlOut.Append(Right(r.Item("ac_interior_moyear").ToString, 4).Trim + "&nbsp;")
                Else
                    HtmlOut.Append("&nbsp;")
                End If
                HtmlOut.Append(""",")
                HtmlOut.Append(vbNewLine)
                HtmlOut.Append("""EXT"":""")
                If Not String.IsNullOrEmpty(r.Item("ac_exterior_moyear").ToString) Then
                    HtmlOut.Append(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim)
                    If Not String.IsNullOrEmpty(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim) Then
                        HtmlOut.Append("/")
                    End If
                    HtmlOut.Append(Right(r.Item("ac_exterior_moyear").ToString, 4).Trim + "&nbsp;")
                Else
                    HtmlOut.Append("&nbsp;")
                End If
                HtmlOut.Append(""",")
                HtmlOut.Append(vbNewLine)


                HtmlOut.Append("""EPROG"":""")
                If Not IsDBNull(r("EPROG")) Then
                    If InStr(r("EPROG"), "Confirmed not") > 0 Then
                        HtmlOut.Append("Confirmed Not")
                    ElseIf InStr(r("EPROG"), "Confirmed") > 0 Then
                        HtmlOut.Append("Confirmed")
                    Else
                        HtmlOut.Append(r("EPROG").ToString)
                    End If
                End If
                HtmlOut.Append(""",")
                HtmlOut.Append(vbNewLine)
                HtmlOut.Append("""APROG"":""")
                If Not IsDBNull(r("APROG")) Then
                    If InStr(r("APROG"), "Confirmed not") > 0 Then
                        HtmlOut.Append("Confirmed Not")
                    ElseIf InStr(r("APROG"), "Confirmed") > 0 Then
                        HtmlOut.Append("Confirmed")
                    Else
                        HtmlOut.Append(r("APROG").ToString)
                    End If
                End If
                HtmlOut.Append(""",")
                HtmlOut.Append(vbNewLine)
                HtmlOut.Append("""MAINTAINED"":""")
                If Not IsDBNull(r("ac_maintained")) Then
                    HtmlOut.Append(r("ac_maintained").ToString)
                End If
                HtmlOut.Append(""",")
                HtmlOut.Append(vbNewLine)

                HtmlOut.Append("""forsale"":""" & r("ac_forsale_flag").ToString & """,")
                HtmlOut.Append(vbNewLine)
                HtmlOut.Append("""new"":""" & r("NEWUSED").ToString & """,")
                HtmlOut.Append(vbNewLine)
                HtmlOut.Append("""id"":""" & r("journ_id").ToString & """")
                HtmlOut.Append("}")
            Next
        End If
        results = " var transactionDataSet = [ " & HtmlOut.ToString & " ]; "
        TransactionTableArray = New StringBuilder
        TransactionTableArray.Append(results)


    End Sub
    '''' <summary>
    '''' The function that writes the sales table out.
    '''' </summary>
    '''' <param name="dt"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Shared Function DisplayTransactionAircraftStartingTable(ByVal dt As DataTable) As String
    '  Dim results As String = ""
    '  Dim JournalSubjectNote As String = ""

    '  results = "<table id=""transactionTable"" width=""100%"">"
    '  results += "<thead>"
    '  results += "<tr>"
    '  results += "<th></th>"
    '  results += "<th>Serial #</th>"
    '  results += "<th>REG #</th>"
    '  results += "<th>Year<br />MFR</th>"

    '  results += "<th>Date</th>"
    '  results += "<th>Transaction Info</th>"
    '  results += "<th>AFTT</th>"
    '  results += "<th>Asking</th>"
    '  results += "<th>Sold</th>"
    '  results += "<th>Date Listed</th>"
    '  results += "<th>For Sale</th>"
    '  results += "<th>NEWAC Flag</th>"
    '  results += "<th>ID</th>"
    '  results += "</tr>"
    '  results += "</thead>"
    '  results += "<tbody>"
    '  For Each r As DataRow In dt.Rows
    '    JournalSubjectNote = ""


    '    results += "<tr>"
    '    results += "<td></td>"
    '    results += "<td data-sort=""" & r("ac_ser_no_sort") & """>" & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, True, r("ac_ser_no_full").ToString, "", "") & "</td>"
    '    results += "<td>" & r("ac_reg_no").ToString & "</td>"
    '    results += "<td align=""center"">" & r("ac_mfr_year").ToString & "</td>"

    '    If Not IsDBNull(r("journ_date")) Then
    '      results += "<td align=""center"" data-sort='" & r("journ_date") & "'>"
    '      results += Format(r("journ_date"), "MM/dd/yy")
    '    Else
    '      results += "<td align=""center"" data-sort=''>"
    '    End If
    '    results += "</td>"

    '    'Set up the subject
    '    If Not IsDBNull(r("journ_subject")) Then
    '      If Not String.IsNullOrEmpty(r("journ_subject")) Then
    '        JournalSubjectNote = Left(r.Item("journ_subject").ToString, 90).ToString

    '      End If
    '    End If

    '    'The note
    '    If Not IsDBNull(r("journ_customer_note")) Then
    '      If Not String.IsNullOrEmpty(r.Item("journ_customer_note")) Then
    '        JournalSubjectNote += "&nbsp;&nbsp;(<span class=""help_cursor error_text no_text_underline"" title=""" + r.Item("journ_customer_note").ToString + """>Note</span>)"

    '      End If
    '    End If

    '    results += "<td>" & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, r("journ_id"), True, JournalSubjectNote, "", "") & "</td>"
    '    results += "<td align=""right"">" & r("ac_airframe_tot_hrs").ToString & "</td>"
    '    results += "<td data-sort=""" & r("ac_asking_price").ToString & """ align=""right"">"
    '    If Not IsDBNull(r("ac_asking_price")) Then
    '      If r("ac_asking_price") > 0 Then
    '        results += FormatNumber((r("ac_asking_price") / 1000), 0) & "k"
    '      End If
    '    End If
    '    results += "</td>"

    '    If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = False Then
    '      results += "<td align=""right"">"
    '    ElseIf HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
    '      results += "<td data-sort="""" align=""right"">"


    '      If Not IsDBNull(r("ac_sold_price")) Then
    '        If r("ac_sold_price") > 0 Then
    '          results += "<span unselectable='on' alt='Reported Sale Price Displayed with Permission from Source' title='Reported Sale Price Displayed with Permission from Source'>"
    '          results += DisplayFunctions.TextToImage(FormatNumber((r("ac_sold_price") / 1000), 0) & "k", 10, "Arial", IIf(FormatNumber((r("ac_sold_price") / 1000), 0) < 10000, "35px", "43px"), "")
    '          results += "</span>"
    '        End If
    '      End If

    '    End If

    '    results += "</td>"

    '    If Not IsDBNull(r("ac_list_date")) Then
    '      results += "<td align=""center"" data-sort='" & r("ac_list_date") & "'>"
    '      results += Format(r("ac_list_date"), "MM/dd/yy")
    '    Else
    '      results += "<td align=""center"" data-sort=''>"
    '    End If
    '    results += "</td>"
    '    results += "<td>" & r("ac_forsale_flag").ToString & "</td>"
    '    results += "<td>" & r("NEWUSED").ToString & "</td>"
    '    results += "<td>" & r("journ_id").ToString & "</td>"
    '    results += "</tr>"
    '  Next
    '  results += "</tbody>"

    '  results += "</table>"
    '  Return results
    'End Function
    '''' <summary>
    '''' Function that writes the aircraft table out.
    '''' </summary>
    '''' <param name="dt"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Shared Function DisplayCurrentAircraftStartingTable(ByVal dt As DataTable) As String
    '  Dim results As String = ""
    '  results = "<table id=""startTable"" width=""100%"">"
    '  results += "<thead>"
    '  results += "<tr>"
    '  results += "<th></th>"
    '  results += "<th>Serial #</th>"
    '  results += "<th>REG #</th>"
    '  results += "<th>Year<br />MFR</th>"
    '  results += "<th>Year<br />DLV</th>"
    '  results += "<th>AFTT</th>"
    '  results += "<th>Asking</th>"
    '  results += "<th>Last Sold Price</th>"
    '  results += "<th>Sold Price Date</th>"
    '  results += "<th>Date Listed</th>"
    '  results += "<th>For Sale</th>"
    '  results += "<th>ID</th>"
    '  results += "</tr>"
    '  results += "</thead>"
    '  results += "<tbody>"
    '  For Each r As DataRow In dt.Rows
    '    results += "<tr>"
    '    results += "<td></td>"
    '    results += "<td data-sort=""" & r("ac_ser_no_sort") & """>" & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, True, r("ac_ser_no_full").ToString, "", "") & "</td>"
    '    results += "<td>" & r("ac_reg_no").ToString & "</td>"
    '    results += "<td align=""center"">" & r("ac_mfr_year").ToString & "</td>"
    '    results += "<td align=""center"">" & r("ac_year").ToString & "</td>"
    '    results += "<td align=""right"">" & r("ac_est_airframe_hrs").ToString & "</td>"
    '    results += "<td data-sort=""" & r("ac_asking_price").ToString & """ align=""right"">"
    '    If Not IsDBNull(r("ac_asking_price")) Then
    '      If r("ac_asking_price") > 0 Then
    '        results += FormatNumber((r("ac_asking_price") / 1000), 0) & "k"
    '      End If
    '    End If
    '    results += "</td>"

    '    If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = False Then
    '      results += "<td align=""right"">"
    '    ElseIf HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
    '      results += "<td data-sort=""" & r("LASTSALEPRICE").ToString & """ align=""right"">"
    '      If Not IsDBNull(r("LASTSALEPRICE")) Then
    '        If r("LASTSALEPRICE") > 0 Then
    '          results += "<span unselectable='on' alt='Reported Sale Price Displayed with Permission from Source' title='Reported Sale Price Displayed with Permission from Source'>"
    '          results += DisplayFunctions.TextToImage(FormatNumber((r("LASTSALEPRICE") / 1000), 0) & "k", 10, "Arial", IIf(FormatNumber((r("LASTSALEPRICE") / 1000), 0) < 10000, "35px", "43px"), "")
    '          results += "</span>"
    '        End If
    '      End If
    '    End If

    '    results += "</td>"


    '    If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = False Then
    '      results += "<td align=""center"">"
    '    ElseIf HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
    '      If Not IsDBNull(r("LASTSALEPRICEDATE")) Then
    '        results += "<td data-sort=""" & Format(r("LASTSALEPRICEDATE"), "yyyy/MM/dd") & """ align=""center"">"
    '        results += Format(r("LASTSALEPRICEDATE"), "MM/dd/yy")
    '      Else
    '        results += "<td data-sort="""">"
    '      End If
    '    End If
    '    results += "</td>"

    '    If Not IsDBNull(r("ac_list_date")) Then
    '      results += "<td align=""center"" data-sort=""" & r("ac_list_date") & """>"
    '      results += Format(r("ac_list_date"), "MM/dd/yy")
    '    Else
    '      results += "<td data-sort="""">"
    '    End If
    '    results += "</td>"
    '    results += "<td>" & r("ac_forsale_flag").ToString & "</td>"
    '    results += "<td>" & r("ac_id").ToString & "</td>"
    '    results += "</tr>"
    '  Next
    '  results += "</tbody>"

    '  results += "</table>"
    '  Return results
    'End Function
    Public Function BuildFeatureArray() As Array
        Dim FeatureListArray As String() = Split(FeaturesList, ",")
        Dim featureNameString As String = ""
        Dim CountOfFeatures As Integer = 0

        For CountOfFeatures = 0 To UBound(FeatureListArray)
            Dim FeatureListNameArray As String() = Split(FeatureListArray(CountOfFeatures), " as ")
            If UBound(FeatureListNameArray) = 1 Then
                If featureNameString <> "" Then
                    featureNameString += ","
                End If
                featureNameString += FeatureListNameArray(1)
            End If
        Next

        Return Split(featureNameString, ",")
    End Function
    Public Sub WriteJSArrayCurrentTable(ByVal dt As DataTable, ByVal evaluesDisplay As Boolean)
        Dim results As String = ""
        'Column 1 - Checkbox:
        'Column 2 - Ser #:
        'Column 3 - Reg #
        'Column 4 - Year MFR
        'Column 5 - Year DLV
        'Column 6 - AFTT
        'Column 7 - Asking
        'Column 8 - Last Sold Price
        'Column 9 - evalue
        'Column 10 - evalue model avg.
        'Column 9 - Date Listed
        'Column 10 - For Sale 
        'Column 11 - ID
        Dim FinalFeatureArray As Array = BuildFeatureArray()
        Dim htmlout As New StringBuilder
        Dim temp_sale As String = ""
        Dim client_asking As Long = 0


        If Not IsNothing(dt) Then
            For Each r As DataRow In dt.Rows
                If Trim(htmlout.ToString.Trim) <> "" Then
                    htmlout.Append(",")
                End If
                htmlout.Append("{")
                htmlout.Append("""check"": """",") 'Checkbox row.
                htmlout.Append("""ser"": [""" & Replace(Replace("<a " & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, False, "", "", "") & " title=""" & r("amod_make_name") & " " & r("amod_model_name") & " S/N #" & r("ac_ser_no_full") & """>" & r("ac_ser_no_full").ToString & "</a>", """", "\"""), "'", "\'") & """,   """ & r("ac_ser_no_sort").ToString & """],")
                htmlout.Append(vbNewLine)
                htmlout.Append("""reg"": """ & r("ac_reg_no").ToString & """,")
                htmlout.Append(vbNewLine)



                For CountOfFeatures = 0 To UBound(FinalFeatureArray)
                    htmlout.Append("""" & Replace(FinalFeatureArray(CountOfFeatures), "'", "") & """: """ & r(Replace(FinalFeatureArray(CountOfFeatures), "'", "")).ToString & """,")
                    htmlout.Append(vbNewLine)
                Next


                htmlout.Append("""mfr"": """ & r("ac_mfr_year").ToString & """,")
                htmlout.Append(vbNewLine)
                htmlout.Append("""year"": """ & r("ac_year").ToString & """,")
                htmlout.Append(vbNewLine)

                If evaluesDisplay Then
                    If Not IsDBNull(r("afmv_airframe_hrs")) Then
                        htmlout.Append("""aftt"":""" & r("afmv_airframe_hrs").ToString & """,")   ' changed from ac_est_airframe_hrs to afmv_airframe_hrs  - MSW 
                    Else
                        htmlout.Append("""aftt"":""" & r("ac_est_airframe_hrs").ToString & """,")   ' changed from ac_est_airframe_hrs to afmv_airframe_hrs  - MSW 
                    End If
                Else
                    If Not IsDBNull(r("ac_est_airframe_hrs")) Then
                        htmlout.Append("""aftt"":""" & r("ac_est_airframe_hrs").ToString & """,")
                    Else
                        htmlout.Append("""aftt"":"""",")
                    End If
                End If


                htmlout.Append(vbNewLine)
                htmlout.Append("""ett"":""")

                If Not IsDBNull(r("ac_engine_1_tot_hrs")) Then
                    If CDbl(r.Item("ac_engine_1_tot_hrs").ToString) = 0 Then
                        htmlout.Append("[0]&nbsp;")
                    Else
                        htmlout.Append("[" + r.Item("ac_engine_1_tot_hrs").ToString + "]&nbsp;")
                    End If
                Else
                    htmlout.Append("[U]&nbsp;")
                End If

                If Not IsDBNull(r("ac_engine_2_tot_hrs")) Then
                    If CDbl(r.Item("ac_engine_2_tot_hrs").ToString) = 0 Then
                        htmlout.Append("[0]&nbsp;")
                    Else
                        htmlout.Append("[" + r.Item("ac_engine_2_tot_hrs").ToString + "]&nbsp;")
                    End If
                Else
                    htmlout.Append("[U]&nbsp;")
                End If

                If Not IsDBNull(r("ac_engine_3_tot_hrs")) Then
                    If CDbl(r.Item("ac_engine_3_tot_hrs").ToString) = 0 Then
                        htmlout.Append("[0]&nbsp;")
                    Else
                        htmlout.Append("[" + r.Item("ac_engine_3_tot_hrs").ToString + "]&nbsp;")
                    End If
                End If

                If Not IsDBNull(r("ac_engine_4_tot_hrs")) Then
                    If CDbl(r.Item("ac_engine_4_tot_hrs").ToString) = 0 Then
                        htmlout.Append("[0]&nbsp;")
                    Else
                        htmlout.Append("[" + r.Item("ac_engine_4_tot_hrs").ToString + "]&nbsp;")
                    End If
                End If

                htmlout.Append(""",")
                htmlout.Append(vbNewLine)

                htmlout.Append("""EPROG"":""")
                If Not IsDBNull(r("EPROG")) Then
                    If InStr(r("EPROG"), "Confirmed not") > 0 Then
                        htmlout.Append("Confirmed Not")
                    ElseIf InStr(r("EPROG"), "Confirmed") > 0 Then
                        htmlout.Append("Confirmed")
                    Else
                        htmlout.Append(r("EPROG").ToString)
                    End If
                End If
                htmlout.Append(""",")
                htmlout.Append(vbNewLine)
                htmlout.Append("""APROG"":""")
                If Not IsDBNull(r("APROG")) Then
                    If InStr(r("APROG"), "Confirmed not") > 0 Then
                        htmlout.Append("Confirmed Not")
                    ElseIf InStr(r("APROG"), "Confirmed") > 0 Then
                        htmlout.Append("Confirmed")
                    Else
                        htmlout.Append(r("APROG").ToString)
                    End If
                End If
                htmlout.Append(""",")
                htmlout.Append(vbNewLine)
                htmlout.Append("""MAINTAINED"":""")
                If Not IsDBNull(r("ac_maintained")) Then
                    htmlout.Append(r("ac_maintained").ToString)
                End If
                htmlout.Append(""",")
                htmlout.Append(vbNewLine)

                htmlout.Append("""OWNER"":""")
                If Not IsDBNull(r("ACOwner")) Then
                    htmlout.Append(r("ACOwner").ToString)
                End If
                htmlout.Append(""",")
                htmlout.Append(vbNewLine)



                client_asking = 0
                If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                    If array_count_current > 0 Then
                        For k = 0 To array_count_current - 1
                            If ac_asking_array_current(k) > 0 And ac_id_array_current(k) = r("ac_id") Then
                                client_asking = ac_asking_array_current(k)
                                k = array_count_current
                            End If
                        Next
                    End If
                End If



                If client_asking > 0 Then
                    htmlout.Append("""ask"":[")

                    If CDbl(client_asking) > 0 Then
                        htmlout.Append("""<span class=\'CLIENTCRMRow\'>$" & FormatNumber((client_asking / 1000), 0) & """ , """ & client_asking & "</span>"" ")
                    Else
                        htmlout.Append(""""",""""")
                    End If
                Else
                    htmlout.Append("""ask"":[")

                    If r("ac_forsale_flag").ToString = "Y" Then
                        If Not IsDBNull(r("ac_asking")) Then
                            If r.Item("ac_asking").ToString.ToLower.Trim.Trim.Contains("price") Then
                                If Not IsDBNull(r("ac_asking_price")) Then
                                    If CDbl(r.Item("ac_asking_price").ToString) > 0 Then
                                        htmlout.Append("""$" & FormatNumber((r("ac_asking_price") / 1000), 0) & """ , """ & r("ac_asking_price").ToString & """ ")
                                    Else
                                        htmlout.Append(""""",""""")
                                    End If
                                Else
                                    htmlout.Append(""""",""""")
                                End If
                            Else
                                htmlout.Append("""" & Replace(Replace(localDataLayer.forsale_status(r("ac_asking").ToString.Trim), """", "\"""), "'", "\'") & """,""1"" ")
                            End If
                        Else
                            htmlout.Append(""""",""""")
                        End If
                    Else
                        htmlout.Append("""OFFMKT"",""0""")
                    End If
                End If





                htmlout.Append("],")
                htmlout.Append(vbNewLine)
                htmlout.Append("""sale"":""")
                If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
                    If Not IsDBNull(r("LASTSALEPRICE")) Then
                        If r("LASTSALEPRICE") > 0 Then

                            temp_sale = Replace(Replace(DisplayFunctions.TextToImage("$" & FormatNumber((r("LASTSALEPRICE") / 1000), 0), 10, "Arial", IIf(FormatNumber((r("LASTSALEPRICE") / 1000), 0) < 10000, "35px", "43px"), ""), """", "\"""), "'", "\'")

                            If InStr(temp_sale, "img") > 0 Then
                                htmlout.Append("<span unselectable=\'on\' alt=\'Reported Sale Price Displayed with Permission from Source\' title=\'Reported Sale Price Displayed with Permission from Source\'>")
                                htmlout.Append(temp_sale)
                                htmlout.Append("</span>")
                            Else
                                htmlout.Append(temp_sale)
                            End If


                        End If
                    End If
                End If
                htmlout.Append(""",")
                htmlout.Append(vbNewLine)
                htmlout.Append("""saledate"":[")
                If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
                    If Not IsDBNull(r("LASTSALEPRICEDATE")) Then
                        htmlout.Append("""" & Format(r("LASTSALEPRICEDATE"), "MM/dd/yy") & """, """ & Format(r("LASTSALEPRICEDATE"), "yyyy/MM/dd") & """")
                    Else
                        htmlout.Append(""""",""""")
                    End If
                Else
                    htmlout.Append(""""",""""")
                End If
                htmlout.Append("],")
                htmlout.Append(vbNewLine)

                'Column 9 - evalue
                'Column 10 - evalue model avg.
                htmlout.Append("""evalue"":[")
                If displayEValues Then
                    If Not IsDBNull(r("EVALUE")) Then
                        htmlout.Append("""$" & FormatNumber((r("EVALUE") / 1000), 0) & """ , """ & r("EVALUE").ToString & """ ")
                    Else
                        htmlout.Append(""""",""""")
                    End If
                Else
                    htmlout.Append(""""",""""")
                End If
                htmlout.Append("],")
                htmlout.Append(vbNewLine)


                htmlout.Append("""evaluemodel"":[")
                If displayEValues Then
                    If Not IsDBNull(r("AVGMODYREVALUE")) Then
                        htmlout.Append("""$" & FormatNumber((r("AVGMODYREVALUE") / 1000), 0) & """ , """ & r("AVGMODYREVALUE").ToString & """ ")
                    Else
                        htmlout.Append(""""",""""")
                    End If
                Else
                    htmlout.Append(""""",""""")
                End If
                htmlout.Append("],")
                htmlout.Append(vbNewLine)

                htmlout.Append("""listdate"": [")
                If Not IsDBNull(r("ac_list_date")) Then
                    htmlout.Append("""" & Format(r("ac_list_date"), "MM/dd/yy") & """, """ & Format(r("ac_list_date"), "yyyy/MM/dd") & """")
                Else
                    htmlout.Append(""""",""""")
                End If
                htmlout.Append("],")
                htmlout.Append(vbNewLine)

                htmlout.Append("""PAX"":""")
                If Not IsDBNull(r("ac_passenger_count")) Then
                    If CDbl(r.Item("ac_passenger_count").ToString) = 0 Then
                        htmlout.Append("0&nbsp;")
                    Else
                        htmlout.Append(r.Item("ac_passenger_count").ToString + "&nbsp;")
                    End If
                Else
                    htmlout.Append("U&nbsp;")
                End If
                htmlout.Append(""",")

                htmlout.Append("""ENGSOH1"":""")
                If Not IsDBNull(r("ac_engine_1_soh_hrs")) Then
                    If CDbl(r.Item("ac_engine_1_soh_hrs").ToString) = 0 Then
                        htmlout.Append("0&nbsp;")
                    Else
                        htmlout.Append(r.Item("ac_engine_1_soh_hrs").ToString + "&nbsp;")
                    End If
                Else
                    htmlout.Append("0&nbsp;")
                End If
                htmlout.Append(""",")

                htmlout.Append("""ENGSOH2"":""")
                If Not IsDBNull(r("ac_engine_2_soh_hrs")) Then
                    If CDbl(r.Item("ac_engine_2_soh_hrs").ToString) = 0 Then
                        htmlout.Append("0&nbsp;")
                    Else
                        htmlout.Append(r.Item("ac_engine_2_soh_hrs").ToString + "&nbsp;")
                    End If
                Else
                    htmlout.Append("0&nbsp;")
                End If
                htmlout.Append(""",")

                htmlout.Append(vbNewLine)
                htmlout.Append("""INT"":""")
                If Not String.IsNullOrEmpty(r.Item("ac_interior_moyear").ToString) Then
                    htmlout.Append(Left(r.Item("ac_interior_moyear").ToString, 2).Trim)
                    If Not String.IsNullOrEmpty(Left(r.Item("ac_interior_moyear").ToString, 2).Trim) Then
                        htmlout.Append("/")
                    End If
                    htmlout.Append(Right(r.Item("ac_interior_moyear").ToString, 4).Trim + "&nbsp;")
                Else
                    htmlout.Append("&nbsp;")
                End If
                htmlout.Append(""",")
                htmlout.Append(vbNewLine)
                htmlout.Append("""EXT"":""")
                If Not String.IsNullOrEmpty(r.Item("ac_exterior_moyear").ToString) Then
                    htmlout.Append(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim)
                    If Not String.IsNullOrEmpty(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim) Then
                        htmlout.Append("/")
                    End If
                    htmlout.Append(Right(r.Item("ac_exterior_moyear").ToString, 4).Trim + "&nbsp;")
                Else
                    htmlout.Append("&nbsp;")
                End If
                htmlout.Append(""",")
                htmlout.Append(vbNewLine)
                htmlout.Append("""forsale"":""" & r("ac_forsale_flag").ToString & """,")
                htmlout.Append(vbNewLine)
                htmlout.Append("""id"":""" & r("ac_id").ToString & """")
                htmlout.Append("}")
            Next
        End If

        If evaluesDisplay Then
            results = " var evaluesDataSet = [ " & htmlout.ToString.Trim & " ]; "
        Else
            results = " var currentDataSet = [ " & htmlout.ToString.Trim & " ]; "
        End If

        CurrentTableArray = New StringBuilder
        CurrentTableArray.Append(results)
    End Sub


    ''' <summary>
    ''' Function that writes the vintage table out.
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="averageAsking"></param>
    ''' <param name="averageSale"></param>
    ''' <param name="AvgAskingPriceVsSellingPrice"></param>
    ''' <param name="AvgSellingPriceByYear"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DisplayValueVintageTable(ByRef dt As DataTable, ByRef averageAsking As Long, ByRef averageSale As Long, ByRef AvgAskingPriceVsSellingPrice As String, ByRef AvgSellingPriceByYear As String) As String
        Dim results As String = ""
        Dim sumAsking As Long = 0
        Dim sumSale As Long = 0
        Dim countAsking As Long = 0
        Dim CountSale As Long = 0
        Dim AircraftYear As String = ""
        Dim AvgAsking As String = ""
        Dim AvgSale As String = ""


        Dim SALECOUNT As Long = 0
        Dim LOWASKING As Long = 0
        Dim HIGHASKING As Long = 0
        Dim AvgAsking1 As Long = 0
        Dim LOWSALE As Long = 0
        Dim AVGSALE1 As Long = 0
        Dim HIGHSALE As Long = 0
        Dim htmlout As New StringBuilder

        Dim row_is_bad_year As Boolean = False
        Dim bad_years As String = ""

        Try

            ' this function, takes in the current SELECt, and RE-DOES IT WITH YEARS IF NECESARY 
            If utilization_view_functions.check_missing_client_years(dt, bad_years, bad_year_ac_id, "DLVYEAR") = True Then
                dt = GetValuesVintageTab(modelList.SelectedValue, ac_market.SelectedValue, year_start.Text, year_end.Text, aftt_start.Text, aftt_end.Text, aircraft_registration.SelectedValue, start_date.Text, end_date.Text, clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True), bad_year_ac_id)
            End If



            htmlout.Append("<table id=""vintageTable"" width=""100%"" border=""1"">")
            htmlout.Append("<thead>")
            htmlout.Append("<tr>")
            If HttpContext.Current.Session.Item("isMobile") Then
                htmlout.Append("<th></th>")
            End If
            htmlout.Append("<th>Year</th>")
            If HttpContext.Current.Session.Item("isMobile") = False Then
                htmlout.Append("<th>In Op Fleet</th>")
            End If
            htmlout.Append("<th>Total Retail Sales</th>")
            htmlout.Append("<th>Low Asking ($k)</th>")
            htmlout.Append("<th>Avg Asking ($k)</th>")
            htmlout.Append("<th>High Asking ($k)</th>")
            htmlout.Append("<th>Low Sale ($k)</th>")
            htmlout.Append("<th>Avg Sale ($k)</th>")
            htmlout.Append("<th>High Sale ($k)</th>")
            htmlout.Append("</tr>")
            htmlout.Append("</thead>")
            htmlout.Append("<tbody>")

            For Each r As DataRow In dt.Rows
                AircraftYear = ""
                AvgAsking = ""
                AvgSale = ""
                SALECOUNT = 0
                LOWASKING = 0
                HIGHASKING = 0
                AvgAsking1 = 0
                LOWSALE = 0
                AVGSALE1 = 0
                HIGHSALE = 0
                countAsking = 0
                CountSale = 0
                sumAsking = 0
                sumSale = 0

                row_is_bad_year = False



                htmlout.Append("<tr>")
                If HttpContext.Current.Session.Item("isMobile") Then
                    htmlout.Append("<td></td>")
                End If
                htmlout.Append("<td>")
                If Not IsDBNull(r("DLVYEAR")) Then
                    htmlout.Append(r("DLVYEAR").ToString)
                    AircraftYear = r("DLVYEAR").ToString
                End If
                htmlout.Append("</td>")
                If HttpContext.Current.Session.Item("isMobile") = False Then
                    htmlout.Append("<td align=""right"">")
                    If Not IsDBNull(r("INOP")) Then
                        htmlout.Append(r("INOP").ToString)
                    End If
                    htmlout.Append("</td>")
                End If


                ' if we have client records 
                If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True And has_client_data = True Then
                    If Trim(bad_years) <> "" Then
                        If InStr(Trim(bad_years), AircraftYear) > 0 Then
                            row_is_bad_year = True
                        End If
                    End If
                End If

                If row_is_bad_year = False Then
                    If Not IsDBNull(r("COUNTASKING")) Then
                        countAsking += r("COUNTASKING")
                    End If

                    If Not IsDBNull(r("COUNTSALE")) Then
                        CountSale += r("COUNTSALE")
                    End If

                    If Not IsDBNull(r("SUMASKING")) Then
                        sumAsking += r("SUMASKING")
                    End If

                    If Not IsDBNull(r("SUMSALE")) Then
                        sumSale += r("SUMSALE")
                    End If

                    If Not IsDBNull(r("LOWASKING")) Then
                        LOWASKING = r("LOWASKING")
                    End If

                    If Not IsDBNull(r("AVGASKING")) Then
                        AvgAsking1 = r("AVGASKING")
                    Else
                        AvgAsking = "null"
                    End If

                    If Not IsDBNull(r("HIGHASKING")) Then
                        HIGHASKING = r("HIGHASKING")
                    End If

                    If Not IsDBNull(r("LOWSALE")) Then
                        LOWSALE = r("LOWSALE")
                    End If

                    If Not IsDBNull(r("AVGSALE")) Then
                        AVGSALE1 = r("AVGSALE")
                    Else
                        AvgSale = "null"
                    End If

                    If Not IsDBNull(r("HIGHSALE")) Then
                        HIGHSALE = r("HIGHSALE")
                    End If
                End If


                ' SALECOUNT SHOULDNT CHANGE 
                If Not IsDBNull(r("SALECOUNT")) Then
                    SALECOUNT = r("SALECOUNT").ToString
                End If

                ' SALECOUNT SHOULDNT CHANGE 
                Call utilization_view_functions.Add_in_client_sale_prices("DLV", countAsking, sumAsking, CountSale, sumSale, AircraftYear, LOWASKING, HIGHASKING, AvgAsking1, LOWSALE, AVGSALE1, HIGHSALE, 0, 0)



                htmlout.Append("<td align=""right"">")
                If SALECOUNT > 0 Then
                    htmlout.Append(SALECOUNT)
                End If
                htmlout.Append("</td>")
                htmlout.Append("<td align=""right"">")
                If LOWASKING > 0 Then
                    htmlout.Append("$" & FormatNumber((LOWASKING / 1000), 0))
                End If
                htmlout.Append("</td>")


                If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True And has_client_data = True Then
                    If countAsking > 0 And sumAsking > 0 Then
                        AvgAsking1 = 0
                        AvgAsking1 = (sumAsking / countAsking)
                    Else
                        AvgAsking1 = 0
                    End If
                End If

                htmlout.Append("<td align=""right"">")
                If AvgAsking1 > 0 Then
                    htmlout.Append("$" & FormatNumber((AvgAsking1 / 1000), 0)) '& "k"
                    AvgAsking = FormatNumber((AvgAsking1 / 1000), 0).ToString
                Else
                    AvgAsking = "null"
                End If
                htmlout.Append("</td>")
                htmlout.Append("<td align=""right"">")
                If HIGHASKING > 0 Then
                    htmlout.Append("$" & FormatNumber((HIGHASKING / 1000), 0)) '& "k" 
                End If
                htmlout.Append("</td>")
                htmlout.Append("<td align=""right"">")
                If LOWSALE > 0 Then
                    htmlout.Append("$" & FormatNumber((LOWSALE / 1000), 0)) '& "k" 
                End If
                htmlout.Append("</td>")


                If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True And has_client_data = True Then
                    If CountSale > 0 And sumSale > 0 Then
                        AVGSALE1 = 0
                        AVGSALE1 = (sumSale / CountSale)
                    Else
                        AVGSALE1 = 0
                    End If
                End If


                htmlout.Append("<td align=""right"" class=""red_text"">")
                If AVGSALE1 > 0 Then
                    htmlout.Append("$" & FormatNumber((AVGSALE1 / 1000), 0)) '& "k"
                    AvgSale = FormatNumber((AVGSALE1 / 1000), 0).ToString
                Else
                    AvgSale = "null"
                End If
                htmlout.Append("</td>")

                htmlout.Append("<td align=""right"">")
                If HIGHSALE > 0 Then
                    htmlout.Append("$" & FormatNumber((HIGHSALE / 1000), 0)) '& "k" 
                End If

                If Not String.IsNullOrEmpty(AvgAsking) And Not String.IsNullOrEmpty(AvgSale) Then

                    If AvgAskingPriceVsSellingPrice <> "" Then
                        AvgAskingPriceVsSellingPrice += ", "
                    End If
                    AvgAskingPriceVsSellingPrice += "['" & AircraftYear & "', " & Replace(AvgAsking, ",", "") & ", " & Replace(AvgSale, ",", "") & "]"
                End If

                If Not String.IsNullOrEmpty(AvgSale) Then
                    If AvgSellingPriceByYear <> "" Then
                        AvgSellingPriceByYear += ", "
                    End If
                    AvgSellingPriceByYear += "['" & AircraftYear & "', " & Replace(AvgSale, ",", "") & "]"
                End If

                htmlout.Append("</td>")
                htmlout.Append("</tr>")
            Next
            htmlout.Append("</tbody>")

            htmlout.Append("</table>")

            If countAsking > 0 And sumAsking > 0 Then
                averageAsking = sumAsking / countAsking
            End If

            If CountSale > 0 And sumSale > 0 Then
                averageSale = sumSale / CountSale
            End If

        Catch ex As Exception
            Return ""
        End Try

        Return htmlout.ToString

    End Function

    ''' <summary>
    ''' Function that writes the aftt Trends table out.
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DisplayValueAFTTTable(ByVal dt As DataTable, ByRef Graph1 As String, ByRef Graph2 As String) As String
        Dim results As String = ""
        Dim sumAsking As Long = 0
        Dim sumSale As Long = 0
        Dim countAsking As Long = 0
        Dim CountSale As Long = 0
        Dim AircraftYear As String = ""
        Dim AvgAsking As String = ""
        Dim AvgSale As String = ""

        Dim SALECOUNT As Long = 0
        Dim LOWASKING As Long = 0
        Dim HIGHASKING As Long = 0
        Dim AvgAsking1 As Long = 0
        Dim LOWSALE As Long = 0
        Dim AVGSALE1 As Long = 0
        Dim HIGHSALE As Long = 0
        Dim low_aftt As Long = 0
        Dim high_aftt As Long = 0

        Try



            results = "<table id=""afttTable"" width=""100%"" border=""1"">"
            results += "<thead>"
            results += "<tr>"
            If HttpContext.Current.Session.Item("isMobile") Then
                results += "<th></th>"
            End If
            results += "<th>AFTT</th>"
            results += "<th># Sold</th>"
            results += "<th>Low Asking ($k)</th>"
            results += "<th>Avg Asking ($k)</th>"
            results += "<th>High Asking ($k)</th>"
            results += "<th>Low Sale ($k)</th>"
            results += "<th>Avg Sale ($k)</th>"
            results += "<th>High Sale ($k)</th>"
            results += "</tr>"
            results += "</thead>"
            results += "<tbody>"
            For Each r As DataRow In dt.Rows
                AircraftYear = ""
                AvgAsking = ""
                AvgSale = ""
                AircraftYear = ""
                AvgAsking = ""
                AvgSale = ""
                SALECOUNT = 0
                LOWASKING = 0
                HIGHASKING = 0
                AvgAsking1 = 0
                LOWSALE = 99999999
                AVGSALE1 = 0
                HIGHSALE = 0
                countAsking = 0
                CountSale = 0
                sumAsking = 0
                sumSale = 0

                If Not IsDBNull(r("COUNTASKING")) Then
                    countAsking += r("COUNTASKING")
                End If

                If Not IsDBNull(r("COUNTSALE")) Then
                    CountSale += r("COUNTSALE")
                End If

                If Not IsDBNull(r("SUMASKING")) Then
                    sumAsking += r("SUMASKING")
                End If

                If Not IsDBNull(r("SUMSALE")) Then
                    sumSale += r("SUMSALE")
                End If

                If Not IsDBNull(r("SALECOUNT")) Then
                    SALECOUNT = r("SALECOUNT").ToString
                End If

                If Not IsDBNull(r("LOWASKING")) Then
                    LOWASKING = r("LOWASKING")
                End If

                If Not IsDBNull(r("AVGASKING")) Then
                    AvgAsking1 = r("AVGASKING")
                Else
                    AvgAsking1 = 0
                End If

                If Not IsDBNull(r("HIGHASKING")) Then
                    HIGHASKING = r("HIGHASKING")
                End If

                If Not IsDBNull(r("LOWSALE")) Then
                    LOWSALE = r("LOWSALE")
                End If

                If Not IsDBNull(r("AVGSALE")) Then
                    AVGSALE1 = r("AVGSALE")
                End If

                If Not IsDBNull(r("HIGHSALE")) Then
                    HIGHSALE = r("HIGHSALE")
                End If

                results += "<tr>"
                If HttpContext.Current.Session.Item("isMobile") Then
                    results += "<td></td>"
                End If
                If Not IsDBNull(r("AFTT")) Then
                    results += "<td data-sort=""" & r("AFTTSORT") & """>"

                    results += r("AFTT").ToString
                    AircraftYear = r("AFTT").ToString

                    If InStr(AircraftYear, "-") > 0 Then
                        low_aftt = Left(Trim(AircraftYear), InStr(Trim(AircraftYear), "-") - 2)
                        high_aftt = Right(Trim(AircraftYear), Len(Trim(AircraftYear)) - InStr(Trim(AircraftYear), "-") - 1)
                    End If
                Else
                    results += "<td>"
                End If


                utilization_view_functions.Add_in_client_sale_prices("AFTT2", countAsking, sumAsking, CountSale, sumSale, 0, LOWASKING, HIGHASKING, AvgAsking1, LOWSALE, AVGSALE1, HIGHSALE, low_aftt, high_aftt)

                If CountSale > 0 And sumSale > 0 Then
                    AVGSALE1 = (sumSale / CountSale)
                End If

                If countAsking > 0 And sumAsking > 0 Then
                    AvgAsking1 = (sumAsking / countAsking)
                End If

                results += "</td>"
                results &= "<td align=""right"">"
                If SALECOUNT > 0 Then
                    results &= SALECOUNT
                End If
                results &= "</td>"
                results &= "<td align=""right"">"
                If LOWASKING > 0 Then
                    results &= "$" & FormatNumber((LOWASKING / 1000), 0)
                End If
                results += "</td>"
                results += "<td align=""right"">"
                If AvgAsking1 > 0 Then
                    results &= "$" & FormatNumber((AvgAsking1 / 1000), 0) '& "k"
                    AvgAsking = FormatNumber((AvgAsking1 / 1000), 0).ToString
                Else
                    AvgAsking = "null"
                End If
                results += "</td>"
                results += "<td align=""right"">"
                If HIGHASKING > 0 Then
                    results &= "$" & FormatNumber((HIGHASKING / 1000), 0) '& "k"
                End If
                results += "</td>"
                results += "<td align=""right"">"
                If LOWSALE > 0 Then
                    results &= "$" & FormatNumber((LOWSALE / 1000), 0) '& "k"
                End If
                results += "</td>"
                results += "<td align=""right"" class=""red_text"">"
                If AVGSALE1 > 0 Then
                    results &= "$" & FormatNumber((AVGSALE1 / 1000), 0) '& "k"
                    AvgSale = FormatNumber((AVGSALE1 / 1000), 0).ToString
                Else
                    AvgSale = "null"
                End If
                results += "</td>"
                results += "<td align=""right"">"
                If HIGHSALE > 0 Then
                    results += "$" & FormatNumber((HIGHSALE / 1000), 0) '& "k"
                End If




                If Graph1 <> "" Then
                    Graph1 += ", "
                End If
                Graph1 += "['" & AircraftYear & "', " & Replace(AvgAsking, ",", "") & ", " & Replace(AvgSale, ",", "") & "]"


                If Not String.IsNullOrEmpty(AvgSale) Then
                    If Graph2 <> "" Then
                        Graph2 += ", "
                    End If
                    Graph2 += "['" & AircraftYear & "', " & Replace(AvgSale, ",", "") & "]"
                End If

                results += "</td>"
                results += "</tr>"
            Next
            results += "</tbody>"

            results += "</table>"

        Catch ex As Exception

        End Try

        Return results
    End Function

    ''' <summary>
    ''' Function that writes the weight class Trends table out.
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DisplayValueWeightTable(ByVal dt As DataTable, ByRef Graph1 As String, ByRef Graph2 As String) As String
        Dim results As String = ""
        Dim AvgAsking As Long = 0
        Dim AvgAskingHidden As Long = 0

        Dim YearMFR As Long = 0
        Dim YearDLV As Long = 0
        Dim yearSold As String = ""
        Dim quarterSold As String = ""

        Dim selling As Long = 0
        Dim _percent As Long = 0
        Dim percentHidden As Long = 0

        Dim variance As Long = 0
        Dim varianceHidden As Long = 0
        Dim avgAftt As Long = 0
        Dim avgDOM As Long = 0
        Dim Make As String = ""
        Dim Model As String = ""
        results = "<table id=""weightTable"" width=""100%"" border=""1"">"
        results += "<thead>"
        results += "<tr>"
        If HttpContext.Current.Session.Item("isMobile") Then
            results += "<th></th>"
        End If
        results += "<th rowspan=""2"">MAKE<br />MODEL</th>"
        results += "<th colspan=""2"">AVG YEAR OF</th>"
        results += "<th colspan=""2"">AVG PRICE (k)</th>"
        results += "<th rowspan=""2"">PERCENT</th>"
        results += "<th rowspan=""2"">VARIANCE</th>"
        results += "<th colspan=""2"">AVERAGE</th>"
        results += "</tr>"
        results += "<tr>"
        If HttpContext.Current.Session.Item("isMobile") Then
            results += "<th></th>"
        End If
        results += "<th>MFR</th>"
        results += "<th>DELIVERY</th>"
        results += "<th>ASKING</th>"
        results += "<th>SELLING</th>"
        results += "<th>AFTT</th>"
        results += "<th>DAYS ON MARKET</th>"
        results += "</tr>"
        results += "</thead>"
        results += "<tbody>"
        For Each r As DataRow In dt.Rows
            AvgAsking = 0
            AvgAskingHidden = 0
            YearMFR = 0
            YearDLV = 0
            yearSold = ""
            quarterSold = ""

            selling = 0
            _percent = 0
            percentHidden = 0
            variance = 0
            varianceHidden = 0
            avgAftt = 0
            avgDOM = 0
            Make = ""
            Model = ""


            If Not IsDBNull(r("MakeAbbrev")) Then
                Make = r("MakeAbbrev")
            End If
            If Not IsDBNull(r("Model")) Then
                Model = r("Model")
            End If
            'Year MFR
            If Not IsDBNull(r("dAvgYearMfr")) Then
                YearMFR = r("dAvgYearMfr")
            Else
                YearMFR = 0
            End If
            'Year DLV
            If Not IsDBNull(r("dAvgYearDlv")) Then
                YearDLV = r("dAvgYearDlv")
            Else
                YearDLV = 0
            End If

            'Asking Price.
            If Not IsDBNull(r("dAvgAsking")) Then
                AvgAsking = r("dAvgAsking")
            Else
                AvgAsking = 0
            End If
            If Not IsDBNull(r("dAvgAskingHidden")) Then
                AvgAskingHidden = r("dAvgAskingHidden")
            Else
                AvgAskingHidden = 0
            End If


            If Not IsDBNull(r("dAvgSelling")) Then
                selling = r("dAvgSelling")
            Else
                selling = 0
            End If

            If Not IsDBNull(r("dPercent")) Then
                _percent = r("dPercent")
            Else
                _percent = 0
            End If

            If Not IsDBNull(r("dPercentHidden")) Then
                percentHidden = r("dPercentHidden")
            Else
                percentHidden = 0
            End If

            If Not IsDBNull(r("dVariance")) Then
                variance = r("dVariance")
            Else
                variance = 0
            End If

            If Not IsDBNull(r("dVarianceHidden")) Then
                varianceHidden = r("dVarianceHidden")
            Else
                varianceHidden = 0
            End If

            If AvgAsking = 0 And AvgAskingHidden > 0 Then
                AvgAsking = AvgAskingHidden
                _percent = percentHidden
                variance = varianceHidden
            End If

            If Not IsDBNull(r("dAvgAFTT")) Then
                avgAftt = r("dAvgAFTT")
            Else
                avgAftt = 0
            End If

            If Not IsDBNull(r("dAvgDOM")) Then
                avgDOM = r("dAvgDOM")
            Else
                avgDOM = 0
            End If


            results += "<tr>"
            If HttpContext.Current.Session.Item("isMobile") Then
                results += "<td></td>"
            End If
            results += "<td>"
            results += "(" & Make.ToString & ") "
            results += Model.ToString
            results += "</td>"
            results += "<td align=""right"">"
            If YearMFR > 0 Then
                results += YearMFR.ToString
            End If
            results += "</td>"
            results += "<td align=""right"">"
            If YearDLV > 0 Then
                results += YearDLV.ToString
            End If
            results += "</td>"
            results += "<td align=""right"">"
            If AvgAsking > 0 Then
                results += "$" & FormatNumber(AvgAsking / 1000, 0, True).ToString
            End If
            results += "</td>"
            results += "<td align=""right"" class=""red_text"">"
            If selling > 0 Then
                results += "$" & FormatNumber(selling / 1000, 0, True).ToString
            End If
            results += "</td>"
            results += "<td align=""right"">"
            If _percent > 0 Then
                results += FormatNumber(_percent, 1, True).ToString() & "%"
            End If
            results += "</td>"
            results += "<td align=""right"">"
            If AvgAsking > 0 Then
                results += FormatNumber(variance, 1, True).ToString() & "%</td>"
            ElseIf AvgAsking = selling Then
                results += "0.0%</td>"
            End If
            results += "</td>"
            results += "<td align=""right"">"
            If avgAftt > 0 Then
                results += FormatNumber(avgAftt, 0, True).ToString()
            End If
            results += "</td>"
            results += "<td align=""right"">"
            If avgDOM > 0 Then
                results += FormatNumber(avgDOM, 0, True).ToString
            End If
            results += "</td>"

            If _percent > 0 Then
                If Graph1 <> "" Then
                    Graph1 += ", "
                End If
                Graph1 += "['(" & Make & ") " & Model & "'," & FormatNumber(_percent, 1, True).ToString() & " ]"
            End If


            If Graph2 <> "" Then
                Graph2 += ", "
            End If
            If AvgAsking > 0 Then
                Graph2 += "['(" & Make & ") " & Model & "'," & FormatNumber(variance, 1, True).ToString() & " ]"
            ElseIf AvgAsking = selling Then
                Graph2 += "['(" & Make & ") " & Model & "',0 ]"
            End If

            results += "</tr>"
        Next
        results += "</tbody>"

        results += "</table>"


        Return results
    End Function

    ''' <summary>
    ''' Function that writes the quarter Trends table out.
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DisplayValueQuarterTable(ByVal dt As DataTable, ByRef Graph1 As String, ByRef Graph5 As String, ByRef Graph6 As String) As String
        Dim results As String = ""
        Dim AvgAsking As Double = 0
        Dim AvgAskingHidden As Double = 0

        Dim YearMFR As Long = 0
        Dim YearDLV As Long = 0
        Dim yearSold As String = ""
        Dim quarterSold As String = ""

        Dim selling As Double = 0
        Dim _percent As Double = 0
        Dim percentHidden As Double = 0

        Dim variance As Double = 0
        Dim varianceHidden As Double = 0
        Dim avgAftt As Long = 0
        Dim avgDOM As Long = 0
        results = "<table id=""quarterTable"" width=""100%"" border=""1"">"
        results += "<thead>"
        results += "<tr>"
        If HttpContext.Current.Session.Item("isMobile") Then
            results += "<th>&nbsp;&nbsp;</th>"
        End If
        results += "<th rowspan=""2"">Year<br />Quarter</th>"
        results += "<th colspan=""2"">AVG YEAR OF</th>"
        results += "<th colspan=""2"">AVG PRICE (k)</th>"
        results += "<th rowspan=""2"">%</th>"
        results += "<th rowspan=""2"">VARIANCE</th>"
        results += "<th colspan=""2"">AVG</th>"
        results += "</tr>"
        results += "<tr>"
        If HttpContext.Current.Session.Item("isMobile") Then
            results += "<th>&nbsp;&nbsp;</th>"
        End If
        results += "<th>MFR</th>"
        results += "<th>DLV</th>"
        results += "<th>ASKING</th>"
        results += "<th>SELLING</th>"
        results += "<th>AFTT</th>"
        results += "<th>DAYS ON MKT</th>"
        results += "</tr>"
        results += "</thead>"
        results += "<tbody>"
        For Each r As DataRow In dt.Rows
            AvgAsking = 0
            AvgAskingHidden = 0
            YearMFR = 0
            YearDLV = 0
            yearSold = ""
            quarterSold = ""

            selling = 0
            _percent = 0
            percentHidden = 0
            variance = 0
            varianceHidden = 0
            avgAftt = 0
            avgDOM = 0
            yearSold = ""
            quarterSold = ""


            'Year
            If Not String.IsNullOrEmpty(r("YearSld")) Then
                yearSold = r("YearSld")
            Else
                yearSold = Year(Now())
            End If

            'Quarter Sold
            If Not String.IsNullOrEmpty(r("QuarterSld")) Then
                quarterSold = r("QuarterSld")
            End If

            'Year MFR
            If Not IsDBNull(r("dAvgYearMfr")) Then
                YearMFR = r("dAvgYearMfr")
            Else
                YearMFR = 0
            End If
            'Year DLV
            If Not IsDBNull(r("dAvgYearDlv")) Then
                YearDLV = r("dAvgYearDlv")
            Else
                YearDLV = 0
            End If

            'Asking Price.
            If Not IsDBNull(r("dAvgAsking")) Then
                AvgAsking = r("dAvgAsking")
            Else
                AvgAsking = 0
            End If
            If Not IsDBNull(r("dAvgAskingHidden")) Then
                AvgAskingHidden = r("dAvgAskingHidden")
            Else
                AvgAskingHidden = 0
            End If


            If Not IsDBNull(r("dAvgSelling")) Then
                selling = r("dAvgSelling")
            Else
                selling = 0
            End If

            If Not IsDBNull(r("dPercent")) Then
                _percent = r("dPercent")
            Else
                _percent = 0
            End If

            If Not IsDBNull(r("dPercentHidden")) Then
                percentHidden = r("dPercentHidden")
            Else
                percentHidden = 0
            End If

            If Not IsDBNull(r("dVariance")) Then
                variance = r("dVariance")
            Else
                variance = 0
            End If

            If Not IsDBNull(r("dVarianceHidden")) Then
                varianceHidden = r("dVarianceHidden")
            Else
                varianceHidden = 0
            End If

            If AvgAsking = 0 And AvgAskingHidden > 0 Then
                AvgAsking = AvgAskingHidden
                _percent = percentHidden
                variance = varianceHidden
            End If

            If Not IsDBNull(r("dAvgAFTT")) Then
                avgAftt = r("dAvgAFTT")
            Else
                avgAftt = 0
            End If

            If Not IsDBNull(r("dAvgDOM")) Then
                avgDOM = r("dAvgDOM")
            Else
                avgDOM = 0
            End If


            If Not IsDBNull(r("dAvgSelling")) Then
                If r("dAvgSelling") > 0 Then
                    results += "<tr>"
                    If HttpContext.Current.Session.Item("isMobile") Then
                        results += "<td>&nbsp;&nbsp;</td>"
                    End If
                    results += "<td>"
                    results += yearSold.ToString
                    results += "-Q"
                    results += quarterSold.ToString
                    results += "</td>"
                    results += "<td align=""right"">"
                    If YearMFR > 0 Then
                        results += YearMFR.ToString
                    End If
                    results += "</td>"
                    results += "<td align=""right"">"
                    If YearDLV > 0 Then
                        results += YearDLV.ToString
                    End If
                    results += "</td>"
                    results += "<td align=""right"">"
                    If AvgAsking > 0 Then
                        results += "$" & FormatNumber(AvgAsking / 1000, 0, True).ToString
                    End If
                    results += "</td>"
                    results += "<td align=""right"" class=""red_text"">"
                    If selling > 0 Then
                        results += "$" & FormatNumber(selling / 1000, 0, True).ToString
                    End If
                    results += "</td>"
                    results += "<td align=""right"">"
                    If _percent > 0 Then
                        results += FormatNumber(_percent, 1, True).ToString() & "%"
                    End If
                    results += "</td>"
                    results += "<td align=""right"">"
                    If AvgAsking > 0 Then
                        results += FormatNumber(variance, 1, True).ToString() & "%</td>"
                    ElseIf AvgAsking = selling Then
                        results += "0.0%</td>"
                    End If
                    results += "</td>"
                    results += "<td align=""right"">"
                    If avgAftt > 0 Then
                        results += FormatNumber(avgAftt, 0, True).ToString()
                    End If
                    results += "</td>"
                    results += "<td align=""right"">"
                    If avgDOM > 0 Then
                        results += FormatNumber(avgDOM, 0, True).ToString
                    End If
                    results += "</td>"


                    results += "</tr>"
                    'This only shows up if there is a selling price.
                    If selling > 0 And AvgAsking > 0 Then
                        If Graph1 <> "" Then
                            Graph1 += ", "
                        End If
                        Graph1 += "['" + yearSold + " - Q" + quarterSold + "'," + IIf(AvgAsking > 0, Replace(FormatNumber(AvgAsking / 1000, 0, True).ToString, ",", ""), "null") + "," + IIf(selling > 0, Replace(FormatNumber(selling / 1000, 0, True).ToString, ",", ""), "null") & "]"
                    End If

                    If Graph5 <> "" Then
                        Graph5 += ", "
                    End If
                    Graph5 += "['" + yearSold + " - Q" + quarterSold + "'," + IIf(_percent > 0, FormatNumber(_percent, 1).ToString, "null") & "]"

                    If Graph6 <> "" Then
                        Graph6 += ", "
                    End If
                    Graph6 += "['" + yearSold + " - Q" + quarterSold + "'," + IIf(variance > 0, FormatNumber(variance, 1).ToString, "null") & "]"


                End If
            End If

        Next
        results += "</tbody>"

        results += "</table>"


        Return results
    End Function

    ''' <summary>
    ''' Function that writes the quarter Trends other graphs out.
    ''' </summary>
    Public Shared Sub DisplayValueQuarterOtherGraphs(ByVal dt As DataTable, ByRef Graph2 As String, ByRef Graph3 As String, ByRef Graph4 As String)
        Dim AvgAsking As Double = 0
        Dim AvgAskingHidden As Double = 0

        Dim YearMFR As Long = 0
        Dim YearDLV As Long = 0
        Dim yearSold As String = ""
        Dim quarterSold As String = ""

        Dim selling As Double = 0
        Dim _percent As Double = 0
        Dim percentHidden As Double = 0

        Dim variance As Double = 0
        Dim varianceHidden As Double = 0
        Dim avgAftt As Long = 0
        Dim avgDOM As Long = 0

        For Each r As DataRow In dt.Rows
            AvgAsking = 0
            AvgAskingHidden = 0
            YearMFR = 0
            YearDLV = 0
            yearSold = ""
            quarterSold = ""

            selling = 0
            _percent = 0
            percentHidden = 0
            variance = 0
            varianceHidden = 0
            avgAftt = 0
            avgDOM = 0
            yearSold = ""
            quarterSold = ""


            'Year
            If Not String.IsNullOrEmpty(r("YearSld")) Then
                yearSold = r("YearSld")
            Else
                yearSold = Year(Now())
            End If

            'Quarter Sold
            If Not String.IsNullOrEmpty(r("QuarterSld")) Then
                quarterSold = r("QuarterSld")
            End If

            'Year MFR
            If Not IsDBNull(r("dAvgYearMfr")) Then
                YearMFR = r("dAvgYearMfr")
            Else
                YearMFR = 0
            End If
            'Year DLV
            If Not IsDBNull(r("dAvgYearDlv")) Then
                YearDLV = r("dAvgYearDlv")
            Else
                YearDLV = 0
            End If

            'Asking Price.
            If Not IsDBNull(r("dAvgAsking")) Then
                AvgAsking = r("dAvgAsking")
            Else
                AvgAsking = 0
            End If
            If Not IsDBNull(r("dAvgAskingHidden")) Then
                AvgAskingHidden = r("dAvgAskingHidden")
            Else
                AvgAskingHidden = 0
            End If


            If Not IsDBNull(r("dAvgSelling")) Then
                selling = r("dAvgSelling")
            Else
                selling = 0
            End If

            If Not IsDBNull(r("dPercent")) Then
                _percent = r("dPercent")
            Else
                _percent = 0
            End If

            If Not IsDBNull(r("dPercentHidden")) Then
                percentHidden = r("dPercentHidden")
            Else
                percentHidden = 0
            End If

            If Not IsDBNull(r("dVariance")) Then
                variance = r("dVariance")
            Else
                variance = 0
            End If

            If Not IsDBNull(r("dVarianceHidden")) Then
                varianceHidden = r("dVarianceHidden")
            Else
                varianceHidden = 0
            End If

            If AvgAsking = 0 And AvgAskingHidden > 0 Then
                AvgAsking = AvgAskingHidden
                _percent = percentHidden
                variance = varianceHidden
            End If

            If Not IsDBNull(r("dAvgAFTT")) Then
                avgAftt = r("dAvgAFTT")
            Else
                avgAftt = 0
            End If

            If Not IsDBNull(r("dAvgDOM")) Then
                avgDOM = r("dAvgDOM")
            Else
                avgDOM = 0
            End If



            If Graph2 <> "" Then
                Graph2 += ", "
            End If
            Graph2 += "['" + yearSold + " - Q" + quarterSold + "'," & IIf(AvgAsking > 0, Replace(FormatNumber(AvgAsking / 1000, 0, True).ToString, ",", ""), "null") + "," + IIf(selling > 0, Replace(FormatNumber(selling / 1000, 0, True).ToString, ",", ""), "null") & "]"


            If Graph3 <> "" Then
                Graph3 += ", "
            End If
            Graph3 += "['" + yearSold + " - Q" + quarterSold + "'," & IIf(AvgAsking > 0, Replace(FormatNumber(AvgAsking / 1000, 0, True).ToString, ",", ""), "null") & "]"

            If Graph4 <> "" Then
                Graph4 += ", "
            End If
            Graph4 += "['" + yearSold + " - Q" + quarterSold + "'," & IIf(selling > 0, Replace(FormatNumber(selling / 1000, 0, True).ToString, ",", ""), "null") & "]"



        Next

    End Sub
    Private Sub modelList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles modelList.SelectedIndexChanged
        ModelSwap(True, False, False)
    End Sub

    '''' <summary>
    '''' Model change postback.
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    Private Sub ModelSwap(ByVal ModelsRefresh As Boolean, ByVal JustForVariants As Boolean, ByVal SwapCurrentAircraft As Boolean)
        Dim SliderValuesTable As New DataTable
        Dim ModelFeaturesTable As New DataTable
        Dim updateTotalsJs As String = ""
        valueTrendsByAFTTLabel.Text = ""
        valueTrendsByQuarterLabel.Text = ""
        valueTrendsByWeightLabel.Text = ""
        evaluesTextAircraft.Text = ""
        If ModelsRefresh = True Then
            Session.Item("searchCriteria").SearchCriteriaViewVariantString = ""
            VariantList.Items.Clear() 'You need to clear these out.
            VariantList.Items.Add(New ListItem("NONE", ""))
            variantThere.Text = "false"
            acIDText.Text = ""
            Session.Item("searchCriteria").SearchCriteriaViewAC = 0
        End If

        If JustForVariants = True Or SwapCurrentAircraft = True Then
            If IsNumeric(acIDText.Text) Then
                Session.Item("searchCriteria").SearchCriteriaViewAC = acIDText.Text
                aircraftID = acIDText.Text
            End If
        End If

        'Rerun on model change

        ModelFeaturesTable = GetFeaturesListByModel(modelList.SelectedValue)
        If Not IsNothing(ModelFeaturesTable) Then
            If ModelFeaturesTable.Rows.Count > 0 Then
                FeaturesList = ModelFeaturesTable.Rows(0).Item("FEATURES")
                Session.Item("searchCriteria").SearchCriteriaViewFeatureString = FeaturesList
                If ModelsRefresh = True Then
                    If Not IsDBNull(ModelFeaturesTable.Rows(0).Item("ModelVariants")) Then
                        Session.Item("searchCriteria").SearchCriteriaViewVariantString = ModelFeaturesTable.Rows(0).Item("ModelVariants")
                        variantThere.Text = "true"
                        FillUpVariantListbox()
                    End If
                End If
            End If
        End If


        valueValuationRan.Text = "false"
        valueResidualsRan.Text = "false"

        salesRan.Text = "false" 'reset this so the sales tab resets and runs on tab swap again.
        currentRan.Text = "false"
        QuarterRan.Text = "false"
        WeightRan.Text = "false"
        afttRan.Text = "false"
        vtgRan.Text = "false"
        valueEvaluesRan.Text = "false"
        aircraft_registration.SelectedValue = "Worldwide"
        ac_market.SelectedValue = "All"
        salesACIDs.Text = ""
        currentACIDs.Text = ""
        newUsed.SelectedValue = "U"
        salePriceDropdown.SelectedValue = ""
        'DisplayCurrentAircraftTable()
        DisplayTransactionAircraftTable()

        If ModelsRefresh = True Then
            startBaseAFTT = 0
            endBaseAFTT = 0
            startBaseYear = 0
            endBaseYear = 0
            ModelID = 0
            aircraftID = 0
        End If

        Session.Item("searchCriteria").SearchCriteriaViewModel = ModelID
        ModelID = modelList.SelectedValue
        searchCriteria.ViewCriteriaAmodID = ModelID

        'Get max/min for inputs
        If SwapCurrentAircraft = False Then
            GetSliderValues(maxYear, maxAFTT, minYear, minAFTT, minTransDate, updateTotalsJs)
        End If

        If tabs_top_left_2.Visible And ModelsRefresh = True Then
            updateTotalsJs += " resetAircraftOnPage();"
        End If
        If SwapCurrentAircraft = False Then
            aftt_start.Text = minAFTT
            aftt_end.Text = maxAFTT
            year_start.Text = minYear
            year_end.Text = maxYear

            hiddenAftt_end.Text = aftt_end.Text
            hiddenAftt_start.Text = aftt_start.Text
            hiddenYear_end.Text = year_end.Text
            hiddenYear_start.Text = year_start.Text
        End If

        modelImage.ImageUrl = Session.Item("jetnetFullHostName") + Session.Item("ModelPicturesFolderVirtualPath") + "/" + ModelID.ToString + ".jpg"
        market_functions.views_display_fleet_market_summary(searchCriteria, modelSummaryText.Text, "")

        CallScriptToShowHideVariant()
        TableBuildJavascript(False) 'Running the table build javascript so we don't have to set it up anywhere else.

        ''This only runs on model change.
        If JustForVariants = False Then 'And SwapCurrentAircraft = False Then
            BuildJqueryDropdownJavascript()
        End If

        If SwapCurrentAircraft = False Then
            BuildSliderYearJavascript() 'Runs Year Builder JS so it's all set to run when needed.

            BuildSliderAFTTJavascript()
        End If

        RemoveAircraftFromPage()
        BuildJQueryClickEventsJavascript()

        BuildSliderDateJavascript()



        DisplayValueVintageTable(ModelsRefresh, SwapCurrentAircraft)
        BuildPostBackJavascript(updateTotalsJs, JustForVariants, SwapCurrentAircraft)

        tabs_bottom_1_update_panel.Update() 'Current Table
        tabs_bottom_2_update_panel.Update() 'History Table
        tabs_bottom_3_update_panel.Update() 'Year by vintage
        tabs_bottom_4_update_panel.Update() 'quarter
        tabs_bottom_5_update_panel.Update() 'afft
        tabs_bottom_6_update_panel.Update() 'weight

        If displayEValues Then
            tabs_bottom_8_update_panel.Update()
            tabs_bottom_9_update_panel.Update()
            tabs_bottom_10_update_panel.Update() 'evalues
        End If


        tabs_top_right_3_update_panel.Update() 'Model Summary
        tabs_top_right_1_update_panel.Update() 'Value summary
        valueSliderGraphUpdate.Update()
    End Sub
    '''' <summary>
    '''' Function that builds the postback for the model change.
    '''' </summary>
    '''' <param name="updateTotalsJs"></param>
    '''' <remarks></remarks>
    Private Sub BuildPostBackJavascript(ByRef updateTotalsJs As String, ByVal ForVariants As Boolean, ByVal SwapCurrentAircraft As Boolean)
        If (displayEValues) Then
            updateTotalsJs += "setUpSlider();"
        End If
        updateTotalsJs += gaugeScr.ToString()
        updateTotalsJs += dropdownString.ToString
        updateTotalsJs += sliderAFTTString.ToString
        updateTotalsJs += sliderYearString.ToString
        updateTotalsJs += sliderDateString.ToString
        'updateTotalsJs += jqueryClickEventsString.ToString
        updateTotalsJs += ResetRemoveAircraftString.ToString()
        updateTotalsJs += VariantString.ToString()
        updateTotalsJs += EvaluesScript.ToString()
        'updateTotalsJs += ";setUpSlider();"

        updateTotalsJs += "setTimeout(function() {var lw = $("".ContainerBoxSummary"").width() - 20;"
        updateTotalsJs += "hideShowGraphs(lw);}, 2000);"

        If Not Page.ClientScript.IsClientScriptBlockRegistered("refreshTableModel") Then
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(IIf(ForVariants, Me.variantUpdatePanel, IIf(SwapCurrentAircraft, Me.loadWhatUpdate, Me.modelUpdatePanel)), Me.GetType, "refreshTable", "" & updateTotalsJs & ";$(""body"").removeClass(""loading"");", True)
        End If


        If Not Page.ClientScript.IsClientScriptBlockRegistered("resetVals") Then
            updateTotalsJs = "$('#" & aircraft_registration.ClientID & "').val('Worldwide');"
            updateTotalsJs += "$('#" & ac_market.ClientID & "').val('All');"

            updateTotalsJs += "$('#" & evaluesTextAircraft.ClientID & "').text('');"
            updateTotalsJs += "$('#" & newUsed.ClientID & "').val('U');"
            updateTotalsJs += "$('#" & salePriceDropdown.ClientID & "').val('');"
            updateTotalsJs += "$('#" & salesACIDs.ClientID & "').val('');"
            updateTotalsJs += "$('#" & currentACIDs.ClientID & "').val('');"
            updateTotalsJs += "$('#" & FolderInformation.ClientID & "').hide();"
            updateTotalsJs += "var tab = $find(""" & tabs_bottom.ClientID & """);"
            updateTotalsJs += "tab.set_activeTabIndex(1);DrawGraphs();"


            updateTotalsJs += "$find('" & tabs_top_right_4.ClientID & "')._hide();"
            updateTotalsJs += "var tabTop = $find(""" & tabs_top_right.ClientID & """);"
            updateTotalsJs += "tabTop.set_activeTabIndex(0);"
            updateTotalsJs += "toggleVariant();"
            updateTotalsJs += "$('#" & variantModelText.ClientID & "').html("""");"
            updateTotalsJs += "$('#" & variantModelText.ClientID & "').addClass('display_none');"
            updateTotalsJs += "$('#" & removeVariants.ClientID & "').addClass('display_none');"
            If ForVariants = True Or SwapCurrentAircraft = True Then
                'If VariantList.SelectedIndex > 0 Then
                If VariantList.Items.Count > 1 Then
                    updateTotalsJs += "var tabTopL = $find(""" & tabs_top_left.ClientID & """);"
                    updateTotalsJs += "tabTopL.set_activeTabIndex(0);"

                    If VariantList.SelectedValue = "" Then
                        If ForVariants = True Then
                            updateTotalsJs += "$('#" & variantModelText.ClientID & "').html(""" & DisplayFunctions.BuildSearchTextDisplay(VariantList, "*No Variant Models Loaded") & """);$('#" & removeVariants.ClientID & "').addClass('display_none');"
                        End If
                    Else
                        updateTotalsJs += "$('#" & variantModelText.ClientID & "').html(""" & DisplayFunctions.BuildSearchTextDisplay(VariantList, "*Variant Models Loaded, Including") & """);$('#" & removeVariants.ClientID & "').removeClass();"
                    End If

                    updateTotalsJs += "$('#" & variantModelText.ClientID & "').removeClass();"
                    'End If
                End If
            End If


            updateTotalsJs += TransactionTableArray.ToString 'CurrentTableArray.ToString
            updateTotalsJs += "setTimeout(function(){" & TransactionTableBuild.ToString & ";},1000);" 'TableBuild.ToString"
            updateTotalsJs += "$(window).resize(function() {"
            updateTotalsJs += "var cw = $('.searchPanelContainerDiv').width() - 20;"
            updateTotalsJs += "$("".cwContainer"").width(cw); "

            updateTotalsJs += "});"

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(IIf(ForVariants, Me.variantUpdatePanel, IIf(SwapCurrentAircraft, Me.loadWhatUpdate, Me.modelUpdatePanel)), Me.GetType(), "resetVals", updateTotalsJs, True)
        End If
    End Sub


    Private Sub FillUpTopRightSliderGraphs()
        Dim utilization_functions As New utilization_view_functions
        utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim



        buildCurrentMarketGraph(topTabCurrentMarketValuation, valueSliderGraphUpdate, 14, utilization_functions, False, 200, True, True)
        topTabCurrentMarketValuation.Text = "<div class=""valueSpec aircraftListing Simplistic aircraftSpec gray_background viewBoxMargin"">" & topTabCurrentMarketValuation.Text & "</div>"

        buildResidualGraph(topTabResidualValues, valueSliderGraphUpdate, 13, utilization_functions, False, 200, True, True)

        topTabResidualValues.Text = "<div class=""valueSpec aircraftListing Simplistic aircraftSpec gray_background viewBoxMargin"">" & topTabResidualValues.Text & "</div>"

        buildAfttValueGraph(topTabValuationByAFTT, valueSliderGraphUpdate, 10, utilization_functions, False, 200, True, True, False)

        topTabValuationByAFTT.Text = "<div class=""valueSpec aircraftListing Simplistic aircraftSpec gray_background viewBoxMargin"">" & topTabValuationByAFTT.Text & "</div>"

        BuildMFRGraph(topTabValuationByMFRYear, valueSliderGraphUpdate, 11, utilization_functions, False, True, 200, True)
        topTabValuationByMFRYear.Text += "</div>"
        buildMonthGraph(topTabValuationByMonth, valueSliderGraphUpdate, 12, utilization_functions, False, True, 200, True, False, False)
        topTabValuationByMonth.Text = "<div class=""valueSpec aircraftListing Simplistic aircraftSpec gray_background viewBoxMargin"">" & topTabValuationByMonth.Text & "</div>"
        valueSliderGraphUpdate.Update()
    End Sub
    Private Sub CallScriptToShowHideVariant()
        VariantString.Append("function toggleVariant() {")

        VariantString.Append(vbCrLf & "if ($('#" & variantThere.ClientID & "').val() == 'true') {")
        'If String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaViewVariantString) Then
        VariantString.Append(vbCrLf & "$find('" & tabs_top_left_3.ClientID & "')._show();")
        'Else
        VariantString.Append(vbCrLf & " } else {")
        VariantString.Append("$find('" & tabs_top_left_3.ClientID & "')._hide();")
        'End If
        VariantString.Append(vbCrLf & " }")

        VariantString.Append(vbCrLf & " }")
    End Sub


    Private Sub valuesByAFTTButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles valuesByAFTTButton.Click
        DisplayAFTTTable()
    End Sub

    Private Sub valuesByQuarterButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles valuesByQuarterButton.Click
        DisplayQuarterTable()
    End Sub

    Private Sub valuesByWeightClassButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles valuesByWeightClassButton.Click
        DisplayWeightTable()
    End Sub

    Private Sub FirstTimeValuesByWeightClassButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FirstTimeValuesByWeightClassButton.Click
        If WeightRan.Text = "false" Then
            WeightRan.Text = "true"
            DisplayWeightTable()
        End If
    End Sub

    Private Sub FirstTimeValuesByAFTTButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FirstTimeValuesByAFTTButton.Click
        If afttRan.Text = "false" Then
            afttRan.Text = "true"
            DisplayAFTTTable()
        End If
    End Sub

    Private Sub FirstTimeValuesByQuarterButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FirstTimeValuesByQuarterButton.Click
        If QuarterRan.Text = "false" Then
            QuarterRan.Text = "true"
            DisplayQuarterTable()
        End If
    End Sub

    Private Sub createStartGraphs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles createStartGraphs.Click
        RunCurrentGraph()
    End Sub


    Function BuildStartGraph(ByVal acCurrent As DataTable) As String
        Dim ReturnString As String = ""
        Dim GraphStr As String = ""
        If Not IsNothing(acCurrent) Then
            For Each r As DataRow In acCurrent.Rows
                Dim Asking As String = ""
                Dim SerNo As String = ""
                If Not IsDBNull(r("ac_ser_no")) Then
                    SerNo = r("ac_ser_no").ToString
                End If

                If Not IsDBNull(r("ac_asking_price")) Then
                    Asking = FormatNumber((r("ac_asking_price") / 1000), 0).ToString

                    If Not String.IsNullOrEmpty(Asking) Then

                        If ReturnString <> "" Then
                            ReturnString += ", "
                        End If
                        ReturnString += "['" & SerNo & "', " & Replace(Asking, ",", "") & "]"
                    End If
                End If

            Next

            GraphStr += "var dataStartGraph1 = new google.visualization.DataTable(); "
            GraphStr += " dataStartGraph1.addColumn('string', 'Serial#'); "
            GraphStr += " dataStartGraph1.addColumn('number', 'Asking($k)'); "
            GraphStr += " dataStartGraph1.addRows([" & ReturnString & "]);"
            GraphStr += "var startoptions1 = {'title':'','width':" & IIf(Session.Item("isMobile"), "300", "450") & ",'height':170,legend: 'none',curveType:  'function',colors: ['blue'], 'chartArea': {top:25}, hAxis: { textStyle:{fontSize:'7'}, slantedText:true, slantedTextAngle:70},vAxis: { title: 'Aircraft Value ($k)'} , series: {    0: {  pointSize: 3  } ,  1: {  pointSize: 3  } } };"
            GraphStr += "var startchart1 =  new google.visualization.ScatterChart(document.getElementById('startGraph'));"
            GraphStr += "startchart1.draw(dataStartGraph1, startoptions1);"

        End If
        Return GraphStr
    End Function

    Function BuildSalesGraph(ByVal acCurrent As DataTable) As String
        Dim ReturnString As String = ""
        Dim GraphStr As String = ""
        If Not IsNothing(acCurrent) Then
            For Each r As DataRow In acCurrent.Rows
                Dim Asking As String = ""
                Dim SerNo As String = ""
                Dim Sold As String = ""
                If Not IsDBNull(r("ac_ser_no")) Then
                    SerNo = r("ac_ser_no").ToString
                End If

                If ReturnString <> "" Then
                    ReturnString += ", "
                End If
                ReturnString += "['" & SerNo & " - " & Format(r("journ_date"), "MM/dd/yy") & "', "

                Asking = "null"
                If r("ac_forsale_flag").ToString = "Y" Then
                    If Not IsDBNull(r("ac_asking")) Then
                        If r.Item("ac_asking").ToString.ToLower.Trim.Trim.Contains("price") Then
                            If Not IsDBNull(r("ac_asking_price")) Then
                                Asking = FormatNumber((r("ac_asking_price") / 1000), 0).ToString
                            End If
                        End If
                    End If
                End If

                ReturnString += Replace(Asking, ",", "") & ", "

                Sold = "null"
                If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
                    If Not IsDBNull(r("ac_sold_price")) Then
                        If r("ac_sold_price") > 0 Then
                            Sold = FormatNumber((r("ac_sold_price") / 1000), 0).ToString
                        End If
                    End If
                End If

                ReturnString += Replace(Sold, ",", "") & " "
                ReturnString += "]"
            Next

            GraphStr += "var dataStartGraph2 = new google.visualization.DataTable(); "
            GraphStr += " dataStartGraph2.addColumn('string', 'Serial#'); "
            GraphStr += " dataStartGraph2.addColumn('number', 'Asking($k)'); "
            GraphStr += " dataStartGraph2.addColumn('number', 'Sold($k)'); "
            GraphStr += " dataStartGraph2.addRows([" & ReturnString & "]);"
            GraphStr += "var startoptions2 = {'title':'','width':" & IIf(Session.Item("isMobile"), "300", "450") & ",'height':170,legend: 'none',curveType:  'function',colors: ['blue', 'red', 'green','blue', 'red', 'green'], 'chartArea': {top:25}, hAxis: { textStyle:{fontSize:'7'}, slantedText:true, slantedTextAngle:70},vAxis: { title: 'Aircraft Value ($k)'} , series: {    0: {  pointSize: 3  } ,  1: {  pointSize: 3  } ,  1: {  pointSize: 2  } } };"
            GraphStr += "var startchart2 =  new google.visualization.ScatterChart(document.getElementById('startGraph'));"
            GraphStr += "startchart2.draw(dataStartGraph2, startoptions2);"

        End If
        Return GraphStr
    End Function

    Private Sub createStartTransGraphs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles createStartTransGraphs.Click
        RunTransGraph()

    End Sub

    Private Sub RunTransGraph()
        Dim SalesAircraftTable As New DataTable
        SalesAircraftTable = GetTransactionAircraftStartingTable(modelList.SelectedValue, startIDs.Text, clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True), has_client_data)
        Dim GraphStr As New StringBuilder
        'createStartTransGraphs.CssClass = "float_right padding"

        GraphStr.Append("swapChosenDropdowns();$find('" & tabs_top_right_4.ClientID & "')._show();")
        GraphStr.Append("var tab = $find(""" & tabs_top_right.ClientID & """);")
        GraphStr.Append("tab.set_activeTabIndex(3);")
        GraphStr.Append("$find('" & tabs_top_right.ClientID & "').get_tabs()[3]._header.innerHTML = 'Sales Value Graph';")

        GraphStr.Append(BuildSalesGraph(SalesAircraftTable))

        'If Not Page.ClientScript.IsClientScriptBlockRegistered("loadStartGraph") Then
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(tabs_top_right_4_update_panel, Me.GetType(), "loadStartTransGraph", GraphStr.ToString & ";$(""body"").removeClass(""loading""); ", True)
        'End If
    End Sub

    Private Sub RunCurrentGraph()
        Dim CurrentAircraftTable As New DataTable
        CurrentAircraftTable = GetAircraftStartingTable(modelList.SelectedValue, ac_market.SelectedValue, year_start.Text, year_end.Text, aftt_start.Text, aftt_end.Text, "", startIDs.Text, clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(VariantList, False, 0, True), IIf(loadWhatAC.SelectedValue = "All", True, False))
        Dim GraphStr As New StringBuilder
        'createStartGraphs.CssClass = "float_right padding"

        GraphStr.Append("$find('" & tabs_top_right_4.ClientID & "')._show();")
        GraphStr.Append("var tab = $find(""" & tabs_top_right.ClientID & """);")
        GraphStr.Append("tab.set_activeTabIndex(3);")
        GraphStr.Append("$find('" & tabs_top_right.ClientID & "').get_tabs()[3]._header.innerHTML = 'Aircraft Value Graph';")
        GraphStr.Append(BuildStartGraph(CurrentAircraftTable))

        'If Not Page.ClientScript.IsClientScriptBlockRegistered("loadStartGraph") Then
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(tabs_top_right_4_update_panel, Me.GetType(), "loadStartGraph", GraphStr.ToString & ";$(""body"").removeClass(""loading""); ", True)
        'End If

    End Sub
    Private Sub refreshGraphs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles refreshGraphs.Click
        If graphWhat.Text = "2" Then
            RunTransGraph()
        Else
            RunCurrentGraph()
        End If
    End Sub

    Private Sub removeVariants_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles removeVariants.Click
        VariantList.SelectedValue = ""
        runVariants_Click(runVariants, System.EventArgs.Empty)
    End Sub

    Private Sub runVariants_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles runVariants.Click
        'Dim script As String = "document.body.className += ' ' + 'loading';"

        'System.Web.UI.ScriptManager.RegisterClientScriptBlock(variantUpdatePanel, Me.GetType, "load", script, True)
        ModelSwap(False, True, False)
    End Sub

    Private Sub loadWhatAC_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles loadWhatAC.SelectedIndexChanged
        ModelSwap(False, False, True)
    End Sub


End Class


'Partial Public Class GoogleChartContainer
'  Dim _arrColumn("", "") As Array 'legend data type, data field
'  Dim _arrRow("", "") As Array 'data x, y
'  Dim _chartType As eGoogleChartType
'  Dim _chartTitle As String = ""
'  Dim _chartWidth As Integer = 100
'  Dim _chartHeight As Integer = 100
'  Dim _chartUnitOfMeasure As String = "percentage"

'  Public Property arrColumn() As Array
'    Get
'      Return _arrColumn
'    End Get
'    Set(ByVal value As Array)
'      _arrColumn = value
'    End Set
'  End Property
'  Public Property arrRow() As Array
'    Get
'      Return _arrRow
'    End Get
'    Set(ByVal value As Array)
'      _arrRow = value
'    End Set
'  End Property
'  Public Property chartType() As eGoogleChartType
'    Get
'      Return _chartType
'    End Get
'    Set(ByVal value As eGoogleChartType)
'      _chartType = value
'    End Set
'  End Property
'  Public Property chartTitle() As String
'    Get
'      Return _chartTitle
'    End Get
'    Set(ByVal value As String)
'      _chartTitle = value
'    End Set
'  End Property

'  Public Property chartWidth() As Integer
'    Get
'      Return _chartTitle
'    End Get
'    Set(ByVal value As Integer)
'      _chartTitle = value
'    End Set
'  End Property


'  Public Property chartHeight() As Integer
'    Get
'      Return _chartHeight
'    End Get
'    Set(ByVal value As Integer)
'      _chartHeight = value
'    End Set
'  End Property

'  Public Property chartUnitOfMeasure() As String
'    Get
'      Return _chartUnitOfMeasure
'    End Get
'    Set(ByVal value As String)
'      _chartUnitOfMeasure = value
'    End Set
'  End Property

'  Sub New()
'    _chartTitle = ""
'    _chartWidth = 100
'    _chartHeight = 100
'    _chartUnitOfMeasure = "percentage"
'    _chartType = eGoogleChartType.LINE


'  End Sub
'End Class
'<System.Serializable(), FlagsAttribute()> Public Enum eGoogleChartType As Integer

'  PIE = 0
'  LINE = 1
'  COLUMN = 2
'  BAR = 4

'End Enum