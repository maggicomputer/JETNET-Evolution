<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebSource.aspx.vb" Inherits="crmWebClient.WebSource" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <!-- Basic Page Needs
  ================================================== -->
  <meta charset="utf-8" />
  <title></title>
  <meta name="description" content="" />
  <meta name="author" content="" />
  <!-- Mobile Specific Metas
  ================================================== -->
  <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
  <!-- CSS
  ================================================== -->
  <!--Created Stylesheet-->
  <link href="/EvoStyles/stylesheets/additional_styles.css" rel="stylesheet" type="text/css" />
  <!-- Header Alternate Styles-->
  <link href="/EvoStyles/stylesheets/header_styles.css" rel="stylesheet" type="text/css" />
  <!--Grid/Layout Styles-->
  <link href="/EvoStyles/stylesheets/layout/base_html_elements.css" rel="stylesheet"
    type="text/css" />
  <link href="/EvoStyles/stylesheets/layout/skeleton_grid.css" rel="stylesheet" type="text/css" />
  <!--Jquery Datatables-->
  <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/v/dt/jszip-3.1.3/pdfmake-0.1.27/dt-1.10.15/af-2.2.0/b-1.3.1/b-colvis-1.3.1/b-html5-1.3.1/b-print-1.3.1/cr-1.3.3/fc-3.2.2/fh-3.1.2/kt-2.2.1/r-2.1.1/rg-1.0.0/rr-1.2.0/sc-1.4.2/se-1.2.2/datatables.min.css" />
  <link rel="stylesheet" type="text/css" href="/common/chosen.css" />
  <link rel="stylesheet" href="/common/classic.css" type="text/css" />

  <!--Javascript ================================================== -->
  <link rel="stylesheet" href="/EvoStyles/stylesheets/ui.daterangepicker.css" type="text/css" />
  <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <style type="text/css">
    table.dataTable td

    {
      font-size: 10px !important;
      white-space: nowrap;
    }
    #MyJqueryTable, #RetailInnerTable, #ValueEstimateInnerTable, .dataTables_scrollHead
    {
      width: 100% !important;
    }
    thead
    {
      background-color: #eee;
    }
    .openButtonVis
    {
      display: none !important;
    }
    .specialTableContainer .dataTable thead tr
    {
      background-color: #eee;
    }
    .specialTableContainer .dataTable thead
    {
      font-weight: bold;
    }
    .specialTableContainer .dataTables_scrollBody thead td
    {
      padding-bottom: 0px !important;
      padding-top: 0px !important;
    }
    .specialTableContainer .dataTables_scrollBody .formatTable.blue thead
    {
      margin-bottom: 10px;
    }
    .specialTableContainer
    {
      width: 100% !important;
    }
    }</style>

  <script type="text/javascript">
    function reload() {
      //      $('#tableCopy').remove();
      //      $('#retailSalesCopy').remove();
      //      
      //      if ($.fn.DataTable.isDataTable('#MyJqueryTable')) {
      //        $('#forSaleInnerTable').empty();
      //      };

      //     // if ($.fn.DataTable.isDataTable('#RetailsTable')) {
      //        $('#RetailInnerTable').empty();alert('test');
      //      //};

    }
    function init() {
      //      scriptToRun = scriptToRun.replace("scrollX: 960,", "");
      //      scriptToRun = scriptToRun.replace("scrollY: 430,", "");
      //      scriptToRun = scriptToRun.replace("scrollCollapse: true,", "");
      //      scriptToRun = scriptToRun.replace("fixedHeader: true,", "fixedHeader: false, responsive: true,");
      //      scriptToRun = scriptToRun.replace("fixedColumns: {leftColumns:6} ,", "fixedColumns: false,");
      //      scriptToRun = scriptToRun.replace("fixedColumns: {leftColumns:5} ,", "fixedColumns: false,");


      //      if ((typeof CreateRetailsDatatable == 'function') || (typeof CreateValueEstimateDatatable == 'function') || (typeof CreateTheDatatable == 'function')) {
      //        //alert('already exists');
      //      }else{
      //        var script = document.createElement("script");
      //        script.type = "text/javascript";
      //        script.text = scriptToRun;          // use this for inline script
      //        document.body.appendChild(script);
      //      }


      //      innerTextParent = dataFromParent.innerHTML;
      ////      innerTextParent = innerTextParent.replace("960px", "100%");

      //      document.body.innerHTML += innerTextParent; //dataFromParent.innerHTML;
      // alert('test');
      var textTitle = $("span[id$='breadcrumbs']", opener.document).text();
      textTitle = textTitle.replace("Market Value Analysis: ", "");
      textTitle = textTitle.replace("Model Market Summary", "");
      textTitle += ' ' + $("#<%= TypeOfView.clientID %>").val();
      //      if (typeof CreateRetailsDatatable == 'function') {
      //        setTimeout(function() {
      //          //          CreateRetailsDatatable();
      //          var info = $('.dataTable').DataTable().page.info();
      //          $('.dataTables_info').html(info.recordsDisplay + ' entries');
      //        }, 800);
      //        textTitle += ' Sold Survey';
      //      }

      //      if (typeof CreateValueEstimateDatatable == 'function') {
      //        setTimeout(function() {
      //          //          CreateValueEstimateDatatable();
      //          var info = $('.dataTable').DataTable().page.info();
      //          $('.dataTables_info').html(info.recordsDisplay + ' entries');
      //        }, 800);
      //        textTitle += ' Value Estimates';
      //      }

      //      if (typeof CreateTheDatatable == 'function') {
      //        textTitle += ' Market Survey';
      //        setTimeout(function() {
      //          //          CreateTheDatatable();
      //          var info = $('.dataTable').DataTable().page.info();
      //          $('.dataTables_info').html(info.recordsDisplay + ' entries');
      //        }, 800);
      //      }

      $("#titleGoesHere").text(textTitle);
    }

  </script>

</head>
<body>
  <form id="form1" runat="server">
  <div class="tabContainerBottomBox">
    <a runat="server" id="refreshClose" href="#" class="float_right display_block padding_table">
      Close Window</a><br />
    <h1 align="center" id="titleGoesHere">
    </h1>
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true"
      EnablePageMethods="true" AsyncPostBackTimeout="8000">
    </cc1:ToolkitScriptManager>

    <script type="text/javascript" src="https://cdn.datatables.net/v/dt/jszip-3.1.3/pdfmake-0.1.27/dt-1.10.15/af-2.2.0/b-1.3.1/b-colvis-1.3.1/b-html5-1.3.1/b-print-1.3.1/cr-1.3.3/fc-3.2.2/fh-3.1.2/kt-2.2.1/r-2.1.1/rg-1.0.0/rr-1.2.0/sc-1.4.2/se-1.2.2/datatables.min.js"></script>

    <asp:Label runat="server" ID="tableBase" Visible="false"></asp:Label>
    <asp:TextBox runat="server" ID="fullSaleCurrentIDs" CssClass="display_none"></asp:TextBox>
    <asp:Button runat="server" ID="FullSaleRefresh" CssClass="display_none" />
    <asp:TextBox runat="server" ID="SoldSurveyCurrentID" CssClass="display_none"></asp:TextBox>
    <asp:TextBox runat="server" ID="valueEstimateCurrentID" CssClass="display_none"></asp:TextBox>
    <asp:Button runat="server" Text="Refresh Graph" ID="RefreshCurrentValueGraph" CssClass="display_none" />
    <asp:TextBox runat="server" ID="typeOfView" CssClass="display_none"></asp:TextBox>
    <asp:Panel ID="pnl_generic_report" runat="server" Visible="false">
      <div id="DivLoadingMessage" style="display: none;">
      </div>
      <div runat="server" id="div_generic_report_table" cssclass="display_none">
        <div style="text-align: center; width: 100%;" runat="server" id="genericReportResults">
          <asp:Label ID="genericReportTable" runat="server" Text=""></asp:Label>
        </div>
      </div>

      <script type="text/javascript">

      ShowLoadingMessage("DivLoadingMessage", "Loading Report", "Loading Report ... Please Wait ...");

      function ShowLoadingMessage(DivTag, Title, Message) {
        $("#" + DivTag).html(Message);
        $("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 75, resizable: false });
      }

      function CloseLoadingMessage(DivTag) {
        $("#" + DivTag).dialog("close");
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

        data.each(function(value, index) {
          if (IDsToUse.length == 0) {
            IDsToUse = value[1];
          } else {
            IDsToUse += ', ' + value[1];
          }
          count += 1;
        });

        //$("#" + selectedRows).val(IDsToUse);

      }

      function CreateGenericTable(divName, tableName, jQueryTablename) {

        var selectedRows = '';

        try {
          if ($.fn.DataTable.isDataTable("#" + jQueryTablename)) {
            $("#" + divName).empty();
          };

        }
        catch (err) {

        }

        if ($("#" + tableName).length) {

          //if ((tableName == "companyDataTable") || (tableName == "airportDataTable")) {
          //  selectedRows = "";

          //} else {
          //  selectedRows = "";

          //}

          //jQuery("#" + tableName).css('display', 'block');

          var clone = jQuery("#" + tableName).clone(true);

          jQuery("#" + tableName).css('display', 'none');
          clone[0].setAttribute('id', jQueryTablename);
          clone.appendTo("#" + divName);

          var table = $("#" + jQueryTablename).DataTable({
            destroy: true,
            language: { "search": "Filter:" },
            fixedHeader: true,
            "initComplete": function(settings, json) {
              setTimeout(function() {
                $("#" + jQueryTablename).DataTable().columns.adjust();
                $("#" + jQueryTablename).DataTable().scroller.measure();

                var dataRows = $("#" + jQueryTablename).DataTable().rows();
                selectAllRows(dataRows.data(), selectedRows, tableName);

              }, 1200)
            },
            scrollCollapse: true,
            stateSave: true,
            paging: false,
            columnDefs: [
                        { targets: [1], className: 'display_none' },
                        { orderable: false, className: 'select-checkbox', width: '10px', targets: [0] }
                      ],
            select: { style: 'multi', selector: 'td:first-child' },
            order: [[2, 'asc']],
            dom: 'Bftrp',
            buttons: [
                    { extend: 'csv', exportOptions: { columns: ':visible'} },
                    { extend: 'excel', exportOptions: { columns: ':visible'} },
                    { extend: 'pdf', orientation: 'landscape', pageSize: 'A2', exportOptions: { columns: ':visible'} },
                    { extend: 'colvis', text: 'Columns', collectionLayout: 'fixed two-column', postfixButtons: ['colvisRestore'] },

                    { text: 'Remove Selected Rows', className: 'RemoveRowsValue',
                      action: function(e, dt, node, config) {

                        dt.rows({ selected: true }).remove().draw(false);
                        selectAllRows(dt.rows({ selected: false }).data(), selectedRows, tableName);

                      }
                    },

                   { text: 'Keep Selected Rows', className: 'KeepTableRow',
                     action: function(e, dt, node, config) {

                       dt.draw();
                       selectAllRows(dt.rows({ selected: true }).data(), selectedRows, tableName);
                       dt.rows({ selected: false }).remove().draw(false);
                       dt.rows('.selected').deselect();

                     }
                   },

                    { text: 'Reload Table', className: 'RefreshTableValue',
                      action: function(e, dt, node, config) {

                        //$("#" + selectedRows).val('');
                        ChangeTheMouseCursorOnItemParentDocument('cursor_wait');

                      }
                    }
                   ]
          });
        }

        $(".RefreshTableValue").addClass('display_none');
        //$(".KeepTableRow").addClass('display_none');

        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
        $($.fn.dataTable.tables(true)).DataTable().scroller.measure();

      };

      </script>

    </asp:Panel>
    <div class="specialTableContainer" id="toggleFlightsOn" visible="false" runat="server">
      <div style="height: 680px; overflow-x: hidden;" class="resizeDiv">
        <table id="flightData" class="refreshable">
        </table>
      </div>
    </div>
  </div>
  </form>

  <script type="text/javascript">

    $(document).ready(function() {
      init();
    });

  </script>

</body>
</html>
