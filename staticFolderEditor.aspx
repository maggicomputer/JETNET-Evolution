<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="staticFolderEditor.aspx.vb"
  Inherits="crmWebClient.staticFolderEditor" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script type="text/javascript">

    var bFromPreferences = <%= bRefreshPreferences.ToString.Tolower %>;
    var bFromHome = <%= bFromHome.ToString.Tolower %>;

    function refreshPreferences() {
      if ((typeof (window.opener) != "undefined") && (window.opener != null)) {
        try {
          window.opener.refreshPreferences();
        }
        catch (err) {

        }
      }
    }

    function refreshHome() {
      if ((typeof (window.opener) != "undefined") && (window.opener != null)) {
        try {
          window.opener.refreshHome();
        }
        catch (err) {

        }
      }
    }

    function Fit() {
      window.resizeTo(1094, 680);
      self.focus();
    };

    window.onload = function () {
      Fit();
    };

    function closeAndRefreshParent() {

      //alert(" refresh Preferences? [" + bFromPreferences + "]  refresh Home? [" + bFromHome + "]");

      if (bFromPreferences) {
        refreshPreferences();
      }

      if (bFromHome) {
        refreshHome();
      }

      self.close();

    }

    function openSmallWindowJS(address, windowname) {
      var rightNow = new Date();
      windowname += rightNow.getTime();
      var Place = window.open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1050,resizable=yes,toolbar=no,location=no,status=no");
      return true;
    }

    function ActiveTabChanged(sender, args) {
      RedrawDatatablesOnSys();
    }

  </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div id="div_static_folder_manager">
    <asp:UpdatePanel ID="pnl_static_folder_editor" runat="server" ChildrenAsTriggers="True"
      UpdateMode="Conditional">
      <ContentTemplate>
        <div id="outerDiv" class="valueSpec viewValueExport Simplistic aircraftSpec gray_background" runat="server">
          <table border="0" style="padding: 4px; border-spacing: 6px; text-align: left; width: 100%;">
            <tr>
              <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">

                <asp:ImageButton ID="close_button_new" runat="server" OnClientClick="javascript:closeAndRefreshParent();" CssClass="float_right criteria_text" AlternateText="Close" ImageUrl="~/images/x.svg" ImageAlign="Middle"></asp:ImageButton>

                <asp:Literal ID="debug_output" runat="server"></asp:Literal>
                <cc1:TabContainer ID="tabContainer" runat="server" Visible="true" OnClientActiveTabChanged="ActiveTabChanged" AutoPostBack="false">
                  <cc1:TabPanel ID="tabPanel1" runat="server">
                    <HeaderTemplate>
                      <asp:Label ID="lbl_static_folder_name" Text="STATIC FOLDER NAME" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    </HeaderTemplate>
                    <ContentTemplate>
                      <div id="div_static_folder_editor_results">
                        <asp:Panel ID="pnl_static_folder_editor_results" runat="server" Visible="true" HorizontalAlign="Left" Width="100%">
                          <div id="div_save_folder_edits" style="text-align: right">
                            <asp:TextBox runat="server" ID="selected_folder_rows" CssClass="display_none"></asp:TextBox>
                            <asp:LinkButton ID="remove_folder" runat="server" PostBackUrl="~/FolderMaintenance.aspx?t=17"><strong>Remove Empty Folder</strong></asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton ID="reset_page" runat="server" PostBackUrl="~/staticFolderEditor.aspx"><strong>Clear Selections</strong></asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton ID="save_folder_edits" runat="server" PostBackUrl="~/staticFolderEditor.aspx?task=saveEdits"><strong>Save Changes</strong></asp:LinkButton>
                          </div>
                          <div runat="server" id="div_static_folder_editor_results_table">
                            <div style="text-align: left; width: 100%;" runat="server" id="folderResults">
                              <asp:Label runat="server" ID="folderTable"></asp:Label>
                            </div>
                          </div>
                        </asp:Panel>
                      </div>
                    </ContentTemplate>
                  </cc1:TabPanel>
                  <cc1:TabPanel ID="tabPanel2" runat="server">
                    <HeaderTemplate>
                      <asp:Label ID="lbl_quick_search" Text="SEARCH" runat="server" Font-Size="Medium"></asp:Label>
                    </HeaderTemplate>
                    <ContentTemplate>
                      <asp:Panel runat="server" ID="quickSearchPanel" DefaultButton="quick_search_button">
                        <div id="div_quick_search">
                          <table border="0" style="padding: 4px; border-spacing: 6px; text-align: left; width: 100%;">
                            <tr>
                              <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">
                                <asp:TextBox ID="quick_search_input" runat="server" TextMode="SingleLine" Rows="1"
                                  Font-Size="Medium" Style="padding-top: 0px; padding-bottom: 2px;" Columns="50"></asp:TextBox>&nbsp;&nbsp;
                              </td>
                              <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">
                                <asp:RadioButtonList ID="continent_or_region" runat="server" OnSelectedIndexChanged="fill_location_Box" AutoPostBack="true">
                                  <asp:ListItem Text="Continent" Value="C" Selected="True">Continent</asp:ListItem>
                                  <asp:ListItem Text="Region" Value="R">Region</asp:ListItem>
                                </asp:RadioButtonList>
                              </td>
                              <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">
                                <asp:DropDownList ID="quick_search_location_box" runat="server"></asp:DropDownList>
                              </td>
                              <td style="vertical-align: top; text-align: left; width: 68px;">&nbsp;</td>
                              <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">
                                <asp:LinkButton ID="quick_search_button" runat="server" PostBackUrl="~/staticFolderEditor.aspx?task=qSearch"><strong>Search</strong></asp:LinkButton>
                              </td>
                            </tr>
                          </table>
                          <asp:CheckBox CssClass="display_block" runat="server" ID="searchCodes" Visible="false" Text="Search Airport Codes" />
                          <asp:CheckBox CssClass="display_block" runat="server" ID="search_reg_nos" Visible="false" Text="Search Registration Numbers" />
                        </div>
                      </asp:Panel>
                      <div id="div_quick_search_results">
                        <asp:Panel ID="pnl_quick_search_results" runat="server" Visible="true" HorizontalAlign="Left"
                          Width="100%">
                          <div id="div_save_quick_search_results" style="text-align: right">
                            <asp:TextBox runat="server" ID="selected_quick_search_rows" CssClass="display_none"></asp:TextBox>
                            <asp:TextBox Visible="false" runat="server" ID="folderName" TextMode="SingleLine" Rows="1" Style="padding-top: 0px; padding-bottom: 2px;" placeholder="folder name"></asp:TextBox>&nbsp;&nbsp;
                            <asp:LinkButton ID="save_quick_search_results_button" runat="server" Visible="false" PostBackUrl="~/staticFolderEditor.aspx?task=saveSearch" ToolTip="Save Search Results"><strong>Save Results</strong></asp:LinkButton>
                          </div>
                          <div runat="server" id="div_quick_search_results_table">
                            <div style="text-align: left; width: 100%;" runat="server" id="searchResults">
                              <asp:Label runat="server" ID="searchTable"></asp:Label>
                            </div>
                          </div>
                        </asp:Panel>
                      </div>
                    </ContentTemplate>
                  </cc1:TabPanel>
                </cc1:TabContainer>
              </td>
            </tr>
          </table>
        </div>
      </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">

      $('#<%= remove_folder.ClientID.ToString %>').hide();

      function confirmEmptyFolder() {

        var r = confirm(" You are not allowed to save EMPTY folders. \n\n Click 'OK' to remove folder or 'Cancel' to continue and add items.");
        return r;

      }

      function showRemoveButton() {

        var selectedItems = document.getElementById("<%= selected_folder_rows.ClientID.ToString %>").value;

        if (selectedItems == '') {

          if (confirmEmptyFolder()) {
            alert("Click 'Remove' button to remove this folder.");
            $('#<%= remove_folder.ClientID.ToString %>').show();
            $('#<%= reset_page.ClientID.ToString %>').hide();
            $('#<%= save_folder_edits.ClientID.ToString %>').hide();
          }

        }
      }

      //Automatically submit on enter press
      $(function () {
        $('textarea').on('keyup', function (e) {
          if (e.keyCode == 13) {
            $("#<%= quick_search_button.ClientID %>").click();
          }
        });
      });

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

        if (tableName == "companyDataTable") {

          var displayTotal = $("#companyLabel");
          displayTotal.html("");

          $("<div/>", {
            html: "<strong>" + count + " Companies</strong>"
          }).appendTo(displayTotal);

        }

        if (tableName == "airportDataTable") {

          var displayTotal = $("#airportLabel");
          displayTotal.html("");

          $("<div/>", {
            html: "<strong>" + count + " Airports</strong>"
          }).appendTo(displayTotal);

        }

        if (tableName == "qsearchDataTable") {

          var displayTotal = $("#qsearchLabel");
          displayTotal.html("");

          $("<div/>", {
            html: "<strong>" + count + " Companies</strong>"
          }).appendTo(displayTotal);


        }

        if (tableName == "airportQsearchDataTable") {

          var displayTotal = $("#airportQsearchLabel");
          displayTotal.html("");

          $("<div/>", {
            html: "<strong>" + count + " Airport(s)</strong>"
          }).appendTo(displayTotal);

        }

      }


      function CreateTheDatatable(divName, tableName, jQueryTablename) {

        var selectedRows = '';

        try {
          if ($.fn.DataTable.isDataTable("#" + jQueryTablename)) {
            $("#" + divName).empty();
          };
        }
        catch (err) {

        }

        if ($("#" + tableName).length) {

          if ((tableName == "companyDataTable") || (tableName == "airportDataTable")) {
            selectedRows = "<%= selected_folder_rows.ClientID %>";

          } else {
            selectedRows = "<%= selected_quick_search_rows.ClientID %>";

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
              { extend: 'csv', exportOptions: { columns: ':visible' } },
              { extend: 'excel', exportOptions: { columns: ':visible' } },
              { extend: 'pdf', orientation: 'landscape', pageSize: 'A2', exportOptions: { columns: ':visible' } },
              { extend: 'colvis', text: 'Columns', collectionLayout: 'fixed two-column', postfixButtons: ['colvisRestore'] },

              {
                text: 'Remove Selected Rows', className: 'RemoveRowsValue',
                action: function (e, dt, node, config) {

                  dt.rows({ selected: true }).remove().draw(false);
                  selectAllRows(dt.rows({ selected: false }).data(), selectedRows, tableName);

                  //alert($("#" + selectedRows).val());
                  if ($("#" + selectedRows).val() == '') {
                    showRemoveButton();
                  }

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

                  $("#" + selectedRows).val('');
                  ChangeTheMouseCursorOnItemParentDocument('cursor_wait');

                }
              }
            ]
          });
        }

        $(".RefreshTableValue").addClass('display_none');
        //if (tableName == "qsearchDataTable") {
        //  $(".KeepTableRow").addClass('display_none');
        //}

        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
        $($.fn.dataTable.tables(true)).DataTable().scroller.measure();

      };

      function CreateTheDatatable_Clean(divName, tableName, jQueryTablename) {

        var selectedRows = '';

        try {
          if ($.fn.DataTable.isDataTable("#" + jQueryTablename)) {
            $("#" + divName).empty();
          };
        }
        catch (err) {

        }

        if ($("#" + tableName).length) {

          if ((tableName == "companyDataTable") || (tableName == "airportDataTable")) {
            selectedRows = "<%= selected_folder_rows.ClientID %>";

          } else {
            selectedRows = "<%= selected_quick_search_rows.ClientID %>";

          }
          //alert(selectedRows);
          //jQuery("#" + tableName).css('display', 'block');

          var clone = jQuery("#" + tableName).clone(true);

          jQuery("#" + tableName).css('display', 'none');
          clone[0].setAttribute('id', jQueryTablename);
          clone.appendTo("#" + divName);

          var table = $("#" + jQueryTablename).DataTable({
            destroy: true,
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
            stateSave: true,
            paging: false,
            columnDefs: [
              { targets: [1], className: 'display_none' },
              { orderable: false, className: 'select-checkbox', width: '10px', targets: [0] }
            ],
            select: { style: 'multi', selector: 'td:first-child' },
            order: [[2, 'asc']],
            dom: 'Btrp',  // Deleted F for the search panel visibility - MSW 
            buttons: [
              // { extend: 'csv', exportOptions: { columns: ':visible'} },
              //  { extend: 'excel', exportOptions: { columns: ':visible'} },
              //  { extend: 'pdf', orientation: 'landscape', pageSize: 'A2', exportOptions: { columns: ':visible'} },
              //  { extend: 'colvis', text: 'Columns', collectionLayout: 'fixed two-column', postfixButtons: ['colvisRestore'] },

              {
                text: 'Select All Rows', className: 'SelectAllRows',
                action: function (e, dt, node, config) {
                  dt.draw();
                  dt.rows().select();
                  selectAllRows(dt.rows({ selected: true }).data(), selectedRows, tableName);

                }
              },

              //   { text: 'Remove Selected Rows', className: 'RemoveRowsValue',
              //      action: function(e, dt, node, config) {

              //        dt.rows({ selected: true }).remove().draw(false);
              //        selectAllRows(dt.rows({ selected: false }).data(), selectedRows, tableName);

              //       }
              //     },

              //      { text: 'Keep Selected Rows', className: 'KeepTableRow',
              //         action: function(e, dt, node, config) {

              //          dt.draw();
              //          selectAllRows(dt.rows({ selected: true }).data(), selectedRows, tableName);
              //          dt.rows({ selected: false }).remove().draw(false);
              //          dt.rows('.selected').deselect();

              //       }
              //      },

              {
                text: 'Reload Table', className: 'RefreshTableValue',
                action: function (e, dt, node, config) {

                  $("#" + selectedRows).val('');
                  ChangeTheMouseCursorOnItemParentDocument('cursor_wait');

                }
              }
            ]
          });
        }

        $(".RefreshTableValue").addClass('display_none');
        //if (tableName == "qsearchDataTable") {
        //  $(".KeepTableRow").addClass('display_none');
        //}

        $("#" + jQueryTablename).on('select.dt deselect.dt', function (e, dt, type, indexes) {
          var rows = dt.rows({ selected: true }).indexes();
          var data = dt.cells(rows, 1).data();
          var IDsToUse = '';
          data.each(function (value, index) {
            if (IDsToUse.length == 0) {
              IDsToUse = value;
            } else {
              IDsToUse += ', ' + value;
            }
          });
          //alert(IDsToUse);
          $("#" + selectedRows).val(IDsToUse);
        });

        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
        $($.fn.dataTable.tables(true)).DataTable().scroller.measure();

      }

    </script>

  </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">
</asp:Content>
