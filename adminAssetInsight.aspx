<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/CustomerAdminTheme.Master" CodeBehind="adminAssetInsight.aspx.vb"
  Inherits="crmWebClient.adminAssetInsight" %>

<%@ MasterType VirtualPath="~/EvoStyles/CustomerAdminTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <link rel="Stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.24/themes/smoothness/jquery-ui.css" />

  <style type="text/css">
    .dataTables_scrollHead
    {
      width: 100% !important;
    }
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="">
    <ProgressTemplate>
      <div id="divLoading" runat="server" style="text-align: center; font-weight: bold; background-color: #eeeeee; filter: alpha(opacity=90);
        opacity: 0.9; width: 395px; height: 295px; text-align: center; padding: 75px; position: absolute; border: 1px solid #003957;
        z-index: 10; margin-left: 225px;">
        <span>Please wait ... </span>
        <br />
        <br />
        <img src="/images/loading.gif" alt="Loading..." /><br />
      </div>
    </ProgressTemplate>
  </asp:UpdateProgress>
  <div style="text-align: left; padding-top: 8px;">
    <asp:UpdatePanel ID="admin_evalue_panel" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
      <ContentTemplate>
        <strong>Evolution eValue Dashboard</strong>
        <asp:Table ID="menuTable" CellPadding="4" CellSpacing="0" Width="100%" CssClass="buttonsTable" runat="server">
          <asp:TableRow>
            <asp:TableCell ID="TableCell0" runat="server" HorizontalAlign="left" VerticalAlign="middle" Style="padding-right: 4px;">
              <table border="0" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                  <td width="50%" align="left" valign="top">
                    <asp:Label ID="evalue_summary_table" runat="server" Visible="true">eValue Summary</asp:Label>
                  </td>
                  <td align="left" valign="top">
                    <asp:Label ID="evalue_processing_table" runat="server" Visible="true">eValue Processing</asp:Label>
                  </td>
                </tr>
              </table>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell ID="TableCell01" runat="server" HorizontalAlign="right" VerticalAlign="middle" Style="padding-right: 4px;">
              <cc1:TabContainer ID="evalue_tabContainer" runat="server" CssClass="dark-theme" Width="100%" Style="margin-left: auto; margin-right: auto;"
                ActiveTabIndex="0" OnClientActiveTabChanged="ActiveTabChanged" AutoPostBack="true" Visible="False">
                <cc1:TabPanel ID="evalue_tabPanel0" runat="server">
                  <HeaderTemplate>
                    <asp:Label ID="evalue_tabPanel0_Label1" runat="server" Text="Results"></asp:Label>
                  </HeaderTemplate>
                  <ContentTemplate>
                    <asp:TextBox runat="server" ID="selected_aircraft_rows_tabPanel0" CssClass="display_none"></asp:TextBox>
                    <div runat="server" id="div_aircraft_results_table_tabPanel0" class="sixteen columns removeLeftMargin">
                      <div style="text-align: center; width: 100%;" runat="server" id="acSearchResultsDiv_tabPanel0">
                        <asp:Label ID="acSearchResultsTable_tabPanel0" runat="server"></asp:Label>
                      </div>
                    </div>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
        
         <asp:Label runat="server" ID="probation_label" Text="<a href='adminAssetInsight.aspx?run_probation=Y&run_world=Y&amod_id=0&run_all=Y'>Run Probation Routine</a>"></asp:Label>
        
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
  <div id="DivLoadingMessage" style="display: none;">
  </div>

  <script type="text/javascript">

    var startWindow;

    function ShowLoadingMessage(DivTag, Title, Message) {
      $("#" + DivTag).html(Message);
      $("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 75, resizable: false });
    }

    function CloseLoadingMessage(DivTag) {
      $("#" + DivTag).dialog("close");
    }

    function ActiveTabChanged(sender, args) {

      //alert("switch tab");

      //ShowLoadingMessage('DivLoadingMessage', 'Loading Aircraft', 'Switching Tabs ... Please Wait ...');

      var nextTab = sender.get_activeTab().get_id();

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

      data.each(function(value, index) {
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

        data.each(function(value, index) {

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
          "initComplete": function(settings, json) {
            setTimeout(function() {
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
          columnDefs: [
                        { targets: [1], className: 'display_none' },
                        { orderable: false, className: 'select-checkbox', width: '10px', targets: [0] }
                      ],
          select: { style: 'multi', selector: 'td:first-child' },
          order: [[2, 'asc']],
          dom: 'Bfitrp',
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

</asp:Content>
