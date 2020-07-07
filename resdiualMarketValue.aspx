<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master"
    CodeBehind="resdiualMarketValue.aspx.vb" Inherits="crmWebClient.resdiualMarketValue" %>

<%@ Register Src="~/controls/viewTypeMakeModel.ascx" TagName="viewTMMDropDowns_ViewSpecific"
    TagPrefix="evo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .dataTables_scrollHead {
            width: 100% !important;
        }

        .dataTables_wrapper .dataTables_length {
            float: right;
        }

        .dataTables_scrollBody thead td {
            padding-bottom: 0px !important;
            padding-top: 0px !important;
        }

        .dataTables_scrollBody .formatTable.blue thead {
            margin-bottom: 10px;
        }

        .dataTable thead {
            font-weight: bold;
        }

        .valueSpec.Simplistic .formatTable.dataTable th {
            padding: 10px 18px;
            background-color: #eee;
            font-size: 12px !important;
            text-transform: none;
            vertical-align: middle;
        }

        .setUpLeftMargin {
            margin-left: 13px !important;
        }

        .setUpLeftMargin_Width {
            margin-left: 13px !important;
            width: 97% !important;
        }

        #atAGlanceCriteriaDivID {
            margin-bottom: 4px;
        }
    </style>

    <script type="text/javascript" src="https://cdn.rawgit.com/Mikhus/canvas-gauges/gh-pages/download/2.1.4/all/gauge.min.js"></script>
    <script>

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        google.charts.load('current', { 'packages': ['corechart', 'table'] });
    </script>
    <asp:Panel ID="contentClass" runat="server" Width="100%" HorizontalAlign="Center"
        CssClass="valueViewPDFExport remove_padding">

        <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width='96%' runat="server"
            class="DetailsBrowseTable">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="center" VerticalAlign="middle">
                    <div class="backgroundShade">
                         <a class="underline help_cursor" onclick='javascript:openSmallWindowJS("help.aspx?t=2&s=1","HelpWindow");'>
                      <img src="/images/help-circle.svg" class="float_left" border="0" alt="Show View Help"
                        title="Show View Help" />
                    </a>
                        <a href="#" onclick="javascript:window.close();">
                            <img src="images/x.svg" alt="Close" /></a>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>

        <div id="searchPanelContainerDiv" runat="server" class="center_outer_div" width="1050">
            <asp:Panel ID="portfolio_view_search" runat="server" HorizontalAlign="Left" Width="100%">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td align="left" valign="top" class="dark_header">
                            <table width="100%" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td align="left" valign="middle" width="12%" class="evoHelp">

                                        <asp:Panel ID="Control_Panel1" runat="server">
                                            <asp:Image ID="ControlImage1" runat="server" ImageUrl="../images/search_expand.jpg" />
                                        </asp:Panel>
                                    </td>
                                    <td align="left" valign="bottom" style="padding-bottom: 10px;" width="460">
                                        <asp:Label ID="breadcrumbs1" runat="server" CssClass="float_left criteria_text"></asp:Label>
                                    </td>
                                    <td align="left" valign="bottom" style="padding-bottom: 10px;" width="310"></td>
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
                <div id="atAGlanceCriteriaDivID">
                    <asp:Panel ID="Collapse_Panel1" runat="server" Height="0px" Width="100%" CssClass="collapse">
                        <asp:Table ID="Table1" runat="server" Width="100%" CellPadding="3" CellSpacing="0"
                            BorderWidth="1" BorderColor="#8b8b8b" BorderStyle="Solid">
                            <asp:TableRow>
                                <asp:TableCell ID="TableCell0" HorizontalAlign="Left" VerticalAlign="top" Width="30%">
                                    <asp:Panel ID="opcosts_make_model_panel" runat="server">
                                        <evo:viewTMMDropDowns_ViewSpecific ID="ViewTMMDropDowns" runat="server" />

                                        <script language="javascript" type="text/javascript">
                            refreshTypeMakeModelByCheckBox("", "", <%= isHeliOnlyProduct.tostring.tolower%>,<%= productCodeCount.tostring%>);
                                        </script>
                                    </asp:Panel>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell1" HorizontalAlign="Left" VerticalAlign="top">
                                </asp:TableCell>
                                <asp:TableCell ID="split_year_cell" HorizontalAlign="Left" VerticalAlign="top">
                                    <asp:CheckBox ID="split_by_year" runat="server" Text="Display by Dlv Year" Style="margin-top: 5px; display: block;" /><div class="clearfix">
                                    </div>
                                    <br />
                                    <asp:ListBox ID="year_start" runat="server" SelectionMode="Multiple" Width="60" Height='200'
                                        Visible="false"></asp:ListBox>
                                    <asp:ListBox ID="year_end" runat="server" CssClass="display_none" SelectionMode="Multiple"
                                        Width="60" Height='80' Style="margin-top: 8px;"></asp:ListBox>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell10" HorizontalAlign="right" VerticalAlign="bottom">
                                    <asp:Button runat="server" ID="atGlanceGo" Text="Search" ToolTip='Click to Apply Critera'
                                        OnClientClick="javascript:clearBox();ShowLoadingMessage('DivLoadingMessage', 'Loading Aircraft', 'Searching ... Please Wait ...');return true;" />
                                    <asp:Button runat="server" ID="atGlanceClear" Text="Clear Selections" ToolTip="Click to Clear Critera"
                                        UseSubmitBehavior="false" OnClientClick="javascript:ChangeTheMouseCursorOnItemParentDocument('standalone_page cursor_wait');" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:Panel>
                </div>
            </asp:Panel>
            <asp:Panel ID="resdiualMarketValue_view_results" runat="server" HorizontalAlign="Left"
                Width="100%">
                <div id="resdiualMarketValue_view_results_div" runat="server">
                    <asp:Panel ID="resdiualMarketValue_view_top_panel" runat="server" HorizontalAlign="Left"
                        Width="100%">
                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                            <tr>
                                <td align="left" valign="top" width="30%"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </asp:Panel>
        </div>

        <div class="aircraftContainer">
            <div class="sixteen columns">
                <div class="valueSpec portfolioManager Simplistic aircraftSpec remove_padding">
                    <h2 runat="server" class="mainHeading" id="mainHeader" visible="false">Residual Market Forecast</h2>
                    <br />
                    <asp:UpdatePanel runat="server" ID="tab1Update" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="tab1Panel" Visible="false">
                                <div class="row">
                                    <div class="columns eight setUpLeftMargin" runat="server" id="chartCSSResize">
                                        <div class="Box">
                                            <asp:Label runat="server" ID="residualValueChart" Visible="false"></asp:Label>
                                            <br />
                                            <br />
                                            <br />
                                            <asp:LinkButton ID="ViewACResidualByMFR" runat="server" CssClass="float_left subMenuText">Enlarge Graph</asp:LinkButton>
                                        </div>
                                    </div>
                                    <asp:Label runat="server" ID="residualGaugeChart" Visible="false"><div class="columns four setUpLeftMargin">
          <div class="Box removeLeftMargin" style="height:180px;">
                    <table cellpadding="0" cellspacing="0" class="formatTable blue large" width="100%">
                     <tr class="noBorder"><td align="left" valign="top"><span class="subHeader">Residual Average</span></td></tr>
                       <tr><td colspan="2" align="center"><canvas id="avgCount"></canvas></td></tr>
                       </table>
                      </div>
           </div>
                                    </asp:Label><div class="columns four setUpLeftMargin" id="chart_divResContainer">
                                        <div class="Box">
                                            <span class="subHeader">% Residual Comparison</span><div id="chart_divRes"></div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Button runat="server" ID="tabPanel1GraphButton" Text="Refresh Tab 1 Graphs" CssClass="display_none" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
                    <asp:TextBox runat="server" ID="rowIDs" CssClass="display_none"></asp:TextBox>
                    <div style="height: 680px; overflow-x: hidden; margin-top: -10px" class="resizeDiv">
                        <cc1:TabContainer ID="main_tab_container" runat="server" CssClass="dark-theme setUpLeftMargin"
                            Width="97%" Style="margin-left: auto; margin-right: auto; text-align: left;">
                            <cc1:TabPanel ID="tabPanel1" runat="server" Visible="true" HeaderText="RESIDUAL FORECAST DETAILS">





                                <HeaderTemplate>
                                    RESIDUAL FORECAST DETAILS
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="tabPanel1Update"
                                        runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="table_label" runat="server"></asp:Label>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>























                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div id="DivLoadingMessage" class="loadingScreenBox" style="display: none;">
        <span></span>
        <div class="loader">Loading...</div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

    <script type="text/javascript">

        var startWindow;

        function ShowLoadingMessage(DivTag, Title, Message) {
            $("#" + DivTag).css("display", "block");
        }

        function CloseLoadingMessage(DivTag) {
            $("#" + DivTag).css("display", "none");
        }
        function clearBox() { $('#<%= rowIDs.ClientID %>').val(''); }

        function ActiveTabChanged(sender, args) {

            //alert("switch tab");

            var nextTab = sender.get_activeTab().get_id();

            //      if (nextTab.indexOf("finder_preferences") > 0) {
            //        swapChosenDropdowns();
            //      }

        }

        $('#<%= split_by_year.ClientID %>').click(function () {
            if (this.checked == false) {
                $('#<%= year_end.ClientID %>').addClass("display_none");
            } else {
                $('#<%= year_end.ClientID %>').removeClass("display_none");
            };
        });


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

            var idCol = data[1] || '';
            var yearCol = data[3] || '';
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

                valUpdate = $('#<%= rowIDs.ClientID %>').val();
              if (valUpdate !== '') {
                  valUpdate += ','
              }
              if ($('#<%= split_by_year.ClientID %>').is(":checked")) {
                  valUpdate += idCol + '--' + yearCol.trim();
              } else {
                  valUpdate += idCol;
              }

              $('#<%= rowIDs.ClientID %>').val(valUpdate);

                        return false;
                    }

                } else { return true; }
            });

//    $(window).resize(function() {
//      setTimeout(function() {
//      //var mw = $(".container.MaxWidthRemove").width() - 20;
//        //$(".cwContainer").width(cw);
//       // hideShowGraphs(mw);
//       // alert(mw);
//      }, 700);
//    });

//    function hideShowGraphs(amountAvailable) {

//      if (Number(amountAvailable) >= 770) {
//        alert('made it');
//        //we have room for the graph.
//        //show graph
//        //chartCSSResize.Attributes.Remove("class")
//        //chartCSSResize.Attributes.Add("class", "columns eight setUpLeftMargin")
//        $('#chart_divRes Container').removeClass();
//        $('#chart_divResContainer').addClass("columns four setUpLeftMargin");
//        $('#<%= chartCSSResize.clientID %>').removeClass();
//        $('#<%= chartCSSResize.clientID %>').addClass("columns eight setUpLeftMargin");
//      } else {
//      $('#chart_divRes Container').removeClass();
//      $('#chart_divResContainer').addClass("columns four setUpLeftMargin display_none");
//      $('#<%= chartCSSResize.clientID %>').removeClass();
//      $('#<%= chartCSSResize.clientID %>').addClass("twelve column setUpLeftMargin_Width");
//        //chartCSSResize.Attributes.Remove("class")
//        //chartCSSResize.Attributes.Add("class", "twelve setUpLeftMargin_Width")
//        
//      }

//    }
    </script>

</asp:Content>
