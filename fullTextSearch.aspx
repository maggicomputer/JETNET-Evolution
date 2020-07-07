<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="fullTextSearch.aspx.vb"
    Inherits="crmWebClient.fullTextSearch" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .fullTextTableHeader {
            /* Permalink - use to edit and share this gradient: http://colorzilla.com/gradient-editor/#f2f5f6+0,e3eaed+37,c8d7dc+100;Grey+3D+%234 */
            background: rgb(242,245,246); /* Old browsers */
            background: -moz-linear-gradient(top, rgba(249, 249, 249,1) 0%, rgba(245, 245, 245,1) 37%, rgba(214, 223, 227,1) 100%); /* FF3.6-15 */
            background: -webkit-linear-gradient(top, rgba(249, 249, 249,1) 0%,rgba(245, 245, 245,1) 37%,rgba(214, 223, 227,1) 100%); /* Chrome10-25,Safari5.1-6 */
            linear-gradient(to bottom, rgb(249, 249, 249) 0%,rgb(245, 245, 245) 37%,rgb(214, 223, 227) 100%) filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#f2f5f6', endColorstr='#c8d7dc',GradientType=0 ); /* IE6-9 */
            padding: 5px;
            text-align: center;
            vertical-align: middle;
            border: 1px solid rgb(156, 174, 180);
            margin-top: 5px;
        }

        .standalone_page br.clear {
            display: none;
        }

        .fullTextSearch {
            text-align: left;
        }

            .fullTextSearch .row {
                margin-bottom: 0px;
            }

            .fullTextSearch .eight.columns {
                width: 50%;
                margin: 0px !important;
            }

                .fullTextSearch .eight.columns span {
                    padding: 0px 5px 0px 5px;
                    display: block;
                }

        .even {
            background-color: #FCFCFC !important;
        }

        .fullTextTableHeader strong {
            color: #266798;
            text-transform: uppercase;
        }

        .fullTextColumns {
            width: 50%
        }

        table.dataTable thead th, table.dataTable thead td {
            padding: 5px 0px 5px 0px;
            background-color: #f2f2f2;
        }

        .searchBox {
            float: right;
            margin-top: -56px;
            z-index: 100000;
            position: absolute;
            right: 22px !important;
            margin-right: 0px;
        }

            .searchBox .searchIcon {
                width: 32px !important;
                height: 20px;
                line-height: 6px;
                margin: 10px 10px 2px 8px;
                font-size: 11px;
                font-weight: normal;
                font-family: 'FontAwesome';
                padding: 6px;
            }

            .searchBox input[type="text"] {
                padding: 6px !important;
                margin-top: 2px !important;
                margin-right: -10px;
                height: 20px;
                border-radius:5px;
            }
    </style>

    <script language="javascript" type="text/javascript">

        function openSmallWindowJS(address, windowname) {
            var rightNow = new Date();
            windowname += rightNow.getTime();
            var Place = window.open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1050,resizable=yes,toolbar=no,location=no,status=no");
            return true;
        }

    </script>
        <style type="text/css">
        .Box{margin-bottom:5px !important;margin-top:2px !important}
        .gray_background{padding-left:3px;padding-right:3px;margin-top:-5px;padding-top:5px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="" DisplayAfter="500" class="loadingScreenBox">
            <ProgressTemplate>
                <span></span>
                <div class="loader">Loading...</div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    <div class="valueSpec Simplistic aircraftSpec">
        <div class="gray_background">
            <div class="fullTextSearch">

                <asp:UpdatePanel ID="full_text_search_update" runat="server" ChildrenAsTriggers="True"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Table ID="buttonsTable" CellPadding="3" CellSpacing="0" Width="100%" class="DetailsBrowseTable"
                            runat="server">
                            <asp:TableRow>
                                <asp:TableCell ID="TableCell00" runat="server" HorizontalAlign="right" VerticalAlign="middle">
                                    <div class="backgroundShade">
                                        <a href="#" class="help_cursor" onclick="javascript:load('/help/documents/707.pdf','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"
                                            title="Full Text Search Help"><img src="images/help-circle.svg" alt="Help" />
                                        </a>
                                        <asp:LinkButton ID="close_button" runat="server" OnClientClick="javascript:window.close();"><img src="images/x.svg" alt="Close" /></asp:LinkButton>
                                    </div>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        <asp:Literal ID="debug_output" runat="server"></asp:Literal>
                        <div class="row">
                            <div class="columns twelve searchBoxFullSearch">
                                <div id="search_input_div" class="searchBox searchBoxMobile">
                                    <asp:TextBox ID="full_text_search_input" runat="server" TextMode="SingleLine" Rows="1"
                                        Font-Size="Medium" Style="padding-top: 0px; padding-bottom: 2px;"></asp:TextBox>&nbsp;&nbsp;
                                    <asp:ImageButton ID="full_text_search_button" runat="server" CssClass="searchIcon" ImageUrl="/images/search.svg" />
                                </div>
                            </div>
                        </div>
                        <div id="search_panel_div" class="row">
                            <asp:Panel ID="full_text_search_panel" runat="server" Visible="true" HorizontalAlign="Left"
                                Width="100%">
                                <asp:Label runat="server" ID="full_text_search_no_results" Visible="false"></asp:Label>
                                <div class="row" runat="server" id="full_text_search_results_row">
                                    <div class="columns eight float_left displayBlockMobile" runat="server" id="modelColumn">
                                        <asp:Label runat="server" ID="modelTable"></asp:Label>
                                    </div>
                                    <div class="columns eight float_right displayBlockMobile" runat="server" id="aircraftColumn">
                                        <asp:Label runat="server" ID="aircraftTable"></asp:Label>
                                    </div>
                                    <div class="columns eight float_left displayBlockMobile" runat="server" id="companyColumn">
                                        <asp:Label runat="server" ID="companyTable"></asp:Label>
                                    </div>
                                    <div class="columns eight float_right displayBlockMobile" runat="server" id="contactColumn">
                                        <asp:Label runat="server" ID="contactTable"></asp:Label>
                                    </div>
                                    <div class="columns eight float_right displayBlockMobile" runat="server" id="yachtColumn">
                                        <asp:Label runat="server" ID="yachtTable"></asp:Label>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>


                    </ContentTemplate>
                </asp:UpdatePanel>

                <script type="text/javascript">

                    //Automatically submit on enter press
                    $(function () {
                        $('textarea').on('keyup', function (e) {
                            if (e.keyCode == 13) {
                                $("#<%= full_text_search_button.clientID %>").click();
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
                                    }, 1200)
                                },
                                scrollCollapse: true,
                                stateSave: true,
                                paging: false,
                                columnDefs: [{ targets: [0] }],
                                select: { style: 'multi', selector: 'td:first-child' },
                                order: [[0, 'asc']],
                                dom: 'Btrp',
                                buttons: [
                                    //  { extend: 'excel', exportOptions: { columns: ':visible'} }
                                ]
                            });
                        }

                        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
                        $($.fn.dataTable.tables(true)).DataTable().scroller.measure();
                    };

                </script>
            </div>
        </div>
    </div>
</asp:Content>
