<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/CustomerAdminTheme.Master" CodeBehind="adminActions.aspx.vb" Inherits="crmWebClient.adminActions" %>

<%@ MasterType VirtualPath="~/EvoStyles/CustomerAdminTheme.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <link rel="Stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.12.1/themes/smoothness/jquery-ui.css" />

  <style type="text/css">
    .dataTables_scrollHead {
      width: 100% !important;
    }

    table.dataTable td {
      font-size: 12px !important;
    }
  </style>
  <script type="text/javascript">

    var parentWindow;

    function openSmallWindowJS(address, windowname) {
      var rightNow = new Date();
      windowname += rightNow.getTime();
      var Place = open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1050,resizable=yes,toolbar=no,location=no,status=no");
    }

    if ((typeof (window.opener) != "undefined") && (window.opener != null)) {
      try { // call the fnRefreshPage on the parent window
        parentWindow = window.opener;
        //alert('show opener' + typeof (parentWindow));
      }
      catch (err) { // if that fails then
        //alert('no opener');
      }
    }
    else {
      //alert('no opener');
    }


  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="">
    <ProgressTemplate>
      <div id="divLoading" runat="server" style="text-align: center; font-weight: bold; background-color: #eeeeee; filter: alpha(opacity=90); opacity: 0.9; width: 395px; height: 295px; text-align: center; padding: 75px; position: absolute; border: 1px solid #003957; z-index: 10; margin-left: 225px;">
        <span>Please wait ... </span>
        <br />
        <br />
        <img src="/images/loading.gif" alt="Loading..." /><br />
      </div>
    </ProgressTemplate>
  </asp:UpdateProgress>
  <div style="text-align: left; padding-top: 8px;">

    <asp:Table ID="menuTable" CellPadding="4" CellSpacing="0" Width="100%" CssClass="buttonsTable" runat="server">
      <asp:TableRow>
        <asp:TableCell ID="TableCell0" runat="server" HorizontalAlign="left" VerticalAlign="middle" Style="padding-right: 4px;">

          <table border="0" style="padding: 4px; border-spacing: 6px; text-align: left; width: 100%;">
            <tr>
              <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px; height: 30%; width: 20%;">
                <asp:Table ID="Table_critera_1" runat="server" CellPadding="2" CellSpacing="2" BackColor="White">
                  <asp:TableRow ID="TableRow2" runat="server">
                    <asp:TableCell ID="TableCell2_1" VerticalAlign="Middle" HorizontalAlign="left" runat="server">
                      <asp:Label ID="Label2" runat="server" Text="Users"></asp:Label>&nbsp;:<br />
                      <asp:ListBox ID="action_users" runat="server" Rows="5" Width="115px" Font-Size="Small">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                      </asp:ListBox>
                    </asp:TableCell>
                  </asp:TableRow>
                </asp:Table>
              </td>
              <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px; height: 30%;">
                <asp:Table ID="Table_critera_2" runat="server" CellPadding="2" CellSpacing="2" BackColor="White" Visible="true">
                  <asp:TableRow ID="TableRow2_1" runat="server">
                    <asp:TableCell ID="TableCell3_1" VerticalAlign="Middle" HorizontalAlign="left" runat="server">
                      <asp:Label ID="Label4" runat="server" Text="Start"></asp:Label>&nbsp;:<br />
                      <asp:TextBox CssClass="homebaseTextBoxFont" ID="action_start_date" runat="server" Width="115px" Height="20px" placeholder="" Style="text-align: right" Visible="true"></asp:TextBox>&nbsp;
                      <br />
                      <asp:Label ID="Label5" runat="server" Text="End"></asp:Label>&nbsp;:<br />
                      <asp:TextBox CssClass="homebaseTextBoxFont" ID="action_end_date" runat="server" Width="115px" Height="20px" placeholder="" Style="text-align: right" Visible="true"></asp:TextBox>&nbsp;
                    </asp:TableCell>
                  </asp:TableRow>
                </asp:Table>
              </td>
            </tr>
          </table>
          <div style="text-align: right; padding-right: 16px; padding-bottom: 6px;">
            <asp:LinkButton ID="searchBtn" runat="server" CssClass="button-darker" OnClientClick="javascript:ShowLoadingMessage('DivLoadingMessage', 'Getting Results', 'Searching ... Please Wait ...');return true;" PostBackUrl="~/adminActions.aspx?task=results" Text="Search" />
          </div>
        </asp:TableCell>
      </asp:TableRow>
      <asp:TableRow>
        <asp:TableCell ID="TableCell01" runat="server" HorizontalAlign="right" VerticalAlign="middle" Style="padding-right: 4px;">
          <asp:UpdatePanel ID="admin_actions_panel" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
            <ContentTemplate>
              <cc1:TabContainer ID="actions_tabContainer" runat="server" CssClass="dark-theme" Width="98%" Style="margin-left: auto; margin-right: auto;"
                ActiveTabIndex="0" AutoPostBack="true" Visible="true">
                <cc1:TabPanel ID="actions_tabPanel0" runat="server" Visible="false">
                  <HeaderTemplate>
                    <asp:Label ID="actions_tabPanel0_Label1" runat="server" Text="Results"></asp:Label>
                  </HeaderTemplate>
                  <ContentTemplate>

                    <asp:TextBox runat="server" ID="selected_rows_tabPanel0" CssClass="display_none"></asp:TextBox>
                    <div runat="server" id="div_results_table_tabPanel0" class="sixteen columns removeLeftMargin">
                      <div style="text-align: center; width: 100%;" runat="server" id="actSearchResultsDiv_tabPanel0">
                        <asp:Label ID="actSearchResultsTable_tabPanel0" runat="server"></asp:Label>
                      </div>
                    </div>

                  </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="actions_tabPanel1" runat="server" Visible="false">
                  <HeaderTemplate>
                    <asp:Label ID="Label1" runat="server" Text="Add New Action"></asp:Label>
                  </HeaderTemplate>
                  <ContentTemplate>
                    <div style="text-align: right; padding-top: 10px; padding-right: 16px; padding-bottom: 6px;">
                      <asp:LinkButton ID="saveBtn" runat="server" CssClass="button-darker" OnClientClick="javascript:ShowLoadingMessage('DivLoadingMessage', 'Saving Action', 'Saving ... Please Wait ...');return true;" PostBackUrl="~/adminActions.aspx" Text="Save" Tooltip="Save current Action Item"/>&nbsp;&nbsp;
                      <asp:LinkButton ID="deleteBtn" runat="server" CssClass="button-darker" OnClientClick="javascript:return confirm('Are you sure you want to Remove this Action Item?');" PostBackUrl="~/adminActions.aspx" Text="Delete" Tooltip="Delete current Action Item"/>
                    </div>
                    <div runat="server" id="div1" class="sixteen columns removeLeftMargin">
                      <div style="text-align: center; width: 100%;" runat="server" id="Div2">

                        <table border="0" style="padding: 4px; border-collapse: separate; border-spacing: 6px; text-align: left; width: 98%;">
                          <tr>
                            <td style="vertical-align: top; text-align: left; padding: 4px; width: 15%;">
                              <asp:Label ID="Label3" runat="server" Text="User"></asp:Label>&nbsp;:<br />
                              <asp:ListBox ID="ListBox1" runat="server" Rows="1" Width="115px" Font-Size="Small">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                              </asp:ListBox>
                            </td>

                            <td style="vertical-align: top; text-align: left; padding: 3px;" colspan="1">
                              <asp:Label ID="Label6" runat="server" Text="Action Date / Time"></asp:Label>&nbsp;:<br />
                              <asp:TextBox CssClass="homebaseTextBoxFont" ID="entry_date" runat="server" Width="108px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>&nbsp;
                              &nbsp;&nbsp;
                              <asp:ListBox ID="entry_time" runat="server" Rows="1" Width="90px" Font-Size="Small">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                <asp:ListItem Text="12:00 AM" Value="0:00:00 AM"></asp:ListItem>
                                <asp:ListItem Text="1:00 AM" Value="1:00:00 AM"></asp:ListItem>
                                <asp:ListItem Text="2:00 AM" Value="2:00:00 AM"></asp:ListItem>
                                <asp:ListItem Text="3:00 AM" Value="3:00:00 AM"></asp:ListItem>
                                <asp:ListItem Text="4:00 AM" Value="4:00:00 AM"></asp:ListItem>
                                <asp:ListItem Text="5:00 AM" Value="5:00:00 AM"></asp:ListItem>
                                <asp:ListItem Text="6:00 AM" Value="6:00:00 AM"></asp:ListItem>
                                <asp:ListItem Text="7:00 AM" Value="7:00:00 AM"></asp:ListItem>
                                <asp:ListItem Text="8:00 AM" Value="8:00:00 AM"></asp:ListItem>
                                <asp:ListItem Text="9:00 AM" Value="9:00:00 AM"></asp:ListItem>
                                <asp:ListItem Text="10:00 AM" Value="10:00:00 AM"></asp:ListItem>
                                <asp:ListItem Text="11:00 AM" Value="11:00:00 AM"></asp:ListItem>
                                <asp:ListItem Text="12:00 PM" Value="12:00:00 PM"></asp:ListItem>
                                <asp:ListItem Text="1:00 PM" Value="13:00:00 PM"></asp:ListItem>
                                <asp:ListItem Text="2:00 PM" Value="14:00:00 PM"></asp:ListItem>
                                <asp:ListItem Text="3:00 PM" Value="15:00:00 PM"></asp:ListItem>
                                <asp:ListItem Text="4:00 PM" Value="16:00:00 PM"></asp:ListItem>
                                <asp:ListItem Text="5:00 PM" Value="17:00:00 PM"></asp:ListItem>
                                <asp:ListItem Text="6:00 PM" Value="18:00:00 PM"></asp:ListItem>
                                <asp:ListItem Text="7:00 PM" Value="19:00:00 PM"></asp:ListItem>
                                <asp:ListItem Text="8:00 PM" Value="20:00:00 PM"></asp:ListItem>
                                <asp:ListItem Text="9:00 PM" Value="21:00:00 PM"></asp:ListItem>
                                <asp:ListItem Text="10:00 PM" Value="22:00:00 PM"></asp:ListItem>
                                <asp:ListItem Text="11:00 PM" Value="23:00:00 PM"></asp:ListItem>
                              </asp:ListBox> 
                            </td>
                              <td>Item Type: 
                                  <asp:DropDownList ID="Type_DropDown" runat="server"> 
                                      <asp:ListItem Value="0" Text="Action Item"></asp:ListItem>
                                      <asp:ListItem Value="1" Text="Research Action"></asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                          </tr>
                          <tr>
                            <td style="vertical-align: top; text-align: left; padding: 4px; width: 15%;">
                              <asp:Label ID="Label7" runat="server" Text="Company"></asp:Label>&nbsp;:<br />
                              <asp:TextBox CssClass="homebaseTextBoxFont" ID="TextBox6" runat="server" Width="98%" Rows="8" placeholder="" TextMode="MultiLine" Style="padding-top: 6px; text-align: left" Enabled="false" BackColor="LightGray"></asp:TextBox>
                            </td>

                            <td style="vertical-align: top; text-align: left; padding: 4px; width: 20%;">
                              <asp:Label ID="Label10" runat="server" Text="Contact"></asp:Label>&nbsp;:<br />
                              <asp:ListBox ID="ListBox3" runat="server" Rows="1" Width="115px" Font-Size="Small">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                              </asp:ListBox>
                            </td>

                            <td style="vertical-align: top; text-align: left; padding: 4px;">
                              <asp:Label ID="Label8" runat="server" Text="Details"></asp:Label>&nbsp;:<br />
                              <asp:TextBox CssClass="homebaseTextBoxFont textAreaLimit" MaxLength="4000" ID="TextBox4" runat="server" Columns="115" Rows="15" TextMode="MultiLine" placeholder="" Style="padding-top: 6px; padding-left: 6px; text-align: left" Visible="true"></asp:TextBox>
                             <p>Characters Remaining: <span runat="server" id="textRemaining" class="textAreaDisplay red_text"></span>.</p>
                            </td>
                          </tr>
                        </table>

                      </div>
                    </div>
                    <div style="text-align: right; padding-top: 10px; padding-right: 16px; padding-bottom: 6px;">
                      <asp:LinkButton ID="confirmBtn" runat="server" CssClass="button-darker" OnClientClick="javascript:return confirm('Are you sure you want to complete this action item and save as a marketing note?');" PostBackUrl="~/adminActions.aspx" Text="Complete" Tooltip="Complete Action Item and save as marketing note"/>
                    </div>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
            </ContentTemplate>
          </asp:UpdatePanel>
        </asp:TableCell>
      </asp:TableRow>
    </asp:Table>
  </div>
  <div id="DivLoadingMessage" style="display: none;">
  </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

  <script type="text/javascript">

    function checkInputLimit(e) {
        var tval = $('.textAreaLimit').val(),
            tlength = tval.length,
            set = <%= JOURN_DESCRIPTION_LEN %>,
            remain = parseInt(set - tlength);

        if (remain <= 0) {
            $('.textAreaLimit').val((tval).substring(0, set))
        }
        $('.textAreaDisplay').text( parseInt( set - $('.textAreaLimit').val().length));
    };

    $(".textAreaLimit").on('input selectionchange propertychange', function (e) {
        checkInputLimit(e);
    });

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
          case "tabPanel0_DataTable":
            {
              selectedRows = "<%= selected_rows_tabPanel0.ClientID %>";
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
          autoWidth: false,
          pageLength: 100,
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

      $(".RefreshTableValue").addClass('display_none');
      //$(".KeepTableRow").addClass('display_none');

      $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
      $($.fn.dataTable.tables(true)).DataTable().scroller.measure();

    };



  </script>

</asp:Content>
