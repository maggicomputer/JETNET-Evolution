<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/EmptyHomebaseTheme.Master" CodeBehind="homebaseSubscription.aspx.vb" Inherits="crmWebClient.homebaseSubscription" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyHomebaseTheme.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="Stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.12.1/themes/smoothness/jquery-ui.css" />
    <link href="common/aircraft_model.css" type="text/css" rel="stylesheet" />
    <link href="EvoStyles/stylesheets/tableThemes.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="/common/moment-with-locales.js"></script>

    <style type="text/css">
        .ui-state-default, .ui-widget-content .ui-state-default, .ui-widget-header .ui-state-default {
            border: 1px solid #d3d3d3;
            background: #078fd7 50% 50% repeat-x;
            font-weight: normal;
            color: #555555;
        }

        .container {
            max-width: 1150px;
        }

        .searchPanelContainerDiv .chosen-container {
            position: relative !important;
        }

        .bx-wrapper {
            margin-top: -8px !important;
        }

            .bx-wrapper .bx-controls-direction a {
                top: 73% !important;
            }

        .bx-wrapper {
            max-height: 250px !important;
        }
         .companyContainer .columns{margin-left:1% !important;}
        .companyContainer .mainHeading {font-size:20px;padding-left:1%;float:left;}
        @media (min-width: 550px) {
            .companyContainer .four.columns{width:31.666667%;}
            .companyContainer .twelve.columns{width:97%;}
        }
        .dropdownBoxContainer{clear:both;float:left;text-align:left;margin-left:1%;padding-top:5px;}
    </style>

    <script type="text/javascript">

        function openSmallWindowJS(address, windowname) {
            var rightNow = new Date();
            windowname += rightNow.getTime();
            var Place = window.open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1250,resizable=yes,toolbar=no,location=no,status=no");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div runat="server" id="toggle_vis" class="companyContainer">
        <div class="row valueSpec viewValueExport Simplistic aircraftSpec">

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

            <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width="100%" runat="server"
                class="DetailsBrowseTable">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="right" VerticalAlign="middle">
              <div class="backgroundShade">
                <a href="#" onclick="javascript:window.close();" class="gray_button float_left"><strong>Close</strong></a>
              </div>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <div class="row valueSpec viewValueExport Simplistic aircraftSpec">


                <div class="row">
                    <div class="six columns">
                        <!-- company info table -->
                        <div class="Box">
                            <asp:Label ID="company_name" runat="server"></asp:Label>
                            <asp:Label ID="company_information_label" runat="server"></asp:Label>
                            <asp:Label ID="company_address" runat="server" CssClass="display_none"></asp:Label>
                        </div>
                    </div>
                    <div class="six columns">
                        <!-- contact info table  -->
                        <div class="Box" id="ContactVisibilityBox" runat="server" visible="false">
                            <div class="subHeader padding_left">
                                <asp:Label runat="server" ID="contactNameText"></asp:Label>
                            </div>
                            <asp:Label ID="contact_information_label" runat="server"></asp:Label>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>


                <h2 class="mainHeading">Primary Service: <strong>
                    <asp:Literal ID="sub_serv_english_desc" runat="server" Text="SUBSCRIPTION"></asp:Literal></strong>
                </h2>

                <asp:UpdatePanel ID="subscription_panel" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional" Visible="true">
                    <ContentTemplate>

                        <div id="saveLicenseButton" style="text-align: right; padding-right: 16px; padding-bottom: 6px;">
                            <asp:Button ID="saveLicense" runat="server" Text="Save Subscription" CssClass="button-darker" OnClientClick="javascript:ShowLoadingMessage('DivLoadingMessage', 'Saving Subscription', 'Saving ... Please Wait ...');return true;" PostBackUrl="~/homebaseSubscription.aspx?task=save" />
                        </div>

                        <div class="row">
                            <div class="four columns removeLeftMargin">
                                <div class="Box">
                                    <div class="subHeader">
                                        <asp:Label ID="Label1" runat="server" Text="GENERAL INFORMATION"></asp:Label>
                                    </div>
                                    <asp:Table ID="Table_gen_1" runat="server" Width="100%" CssClass="formatTable blue">
                                        <asp:TableRow ID="TableRow_op_1_1" runat="server">
                                            <asp:TableCell ID="TableCell_op_1_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label_op_1" runat="server" Text="Subscription ID"></asp:Label>&nbsp;:
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell_op_1_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="sub_id" runat="server" Width="115px" Height="20px" placeholder="0" Enabled="false" BackColor="LightGray" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow_op_1_2" runat="server">
                                            <asp:TableCell ID="TableCell_op_1_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label_op_2" runat="server" Text="Total Licenses"></asp:Label>&nbsp;:
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell_op_1_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="sub_nbr_of_installs" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow_op_1_3" runat="server">
                                            <asp:TableCell ID="TableCell_op_1_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label_op_3" runat="server" Text="Contract Amount"></asp:Label>&nbsp;:
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell_op_1_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="sub_contract_amount" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                                <div class="Box">
                                    <div class="subHeader">
                                        <asp:Label ID="Label2" runat="server" Text="STATUS / DATES"></asp:Label>
                                    </div>

                                    <asp:Table ID="Table_gen_2" runat="server" Width="100%" CssClass="formatTable blue">
                                        <asp:TableRow ID="TableRow6" runat="server">
                                            <asp:TableCell ID="TableCell13" VerticalAlign="Middle" HorizontalAlign="Center" runat="server">
                                                <asp:Label ID="sub_active" runat="server" Text="Active Subscription" ForeColor="Green"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow14" runat="server">
                                            <asp:TableCell ID="TableCell14" VerticalAlign="Middle" HorizontalAlign="Center" runat="server">
                                                <asp:Label ID="Label6" runat="server" Text="Leave End Date blank for reoccurring subscriptions" Font-Size="X-Small"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow_perf_1_1" runat="server">
                                            <asp:TableCell ID="TableCell_perf_1_6" VerticalAlign="Middle" HorizontalAlign="Center" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="sub_start_date" runat="server" Width="75px" Height="20px" placeholder="" Style="text-align: right" Visible="true"></asp:TextBox>&nbsp;/&nbsp;
                            <asp:TextBox CssClass="homebaseTextBoxFont" ID="sub_end_date" runat="server" Width="75px" Height="20px" placeholder="" Style="text-align: right" Visible="true"></asp:TextBox>&nbsp;<em>start / end</em>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                            </div>
                            <div class="four columns removeLeftMargin">
                                <div class="Box">
                                    <div class="subHeader">
                                        <asp:Label ID="Label3" runat="server" Text="DATA ACCESS"></asp:Label>
                                    </div>
                                    <asp:Table ID="Table_data_1" runat="server" Width="100%" CssClass="formatTable blue">
                                        <asp:TableRow ID="TableRow3" runat="server">
                                            <asp:TableCell ID="TableCell16" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label11" runat="server" Text="Service"></asp:Label>&nbsp;:
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell17" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:ListBox ID="sub_serv_code" runat="server" Rows="1" Height="20" Width="115px" Font-Size="Small">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                </asp:ListBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="model_TableRow_7" runat="server">
                                            <asp:TableCell ID="model_TableCell_13" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label_7" runat="server" Text="Frequency"></asp:Label>&nbsp;:
                                            </asp:TableCell>
                                            <asp:TableCell ID="model_TableCell_14" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:ListBox ID="sub_frequency" runat="server" Rows="1" Height="20" Width="115px" Font-Size="Small">
                                                    <asp:ListItem Text="Live" Value="live"></asp:ListItem>
                                                    <asp:ListItem Text="Weekly" Value="weekly"></asp:ListItem>
                                                    <asp:ListItem Text="Monthly" Value="monthly"></asp:ListItem>
                                                </asp:ListBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow1" runat="server">
                                            <asp:TableCell ID="TableCell1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label7" runat="server" Text="Tier"></asp:Label>&nbsp;:
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell2" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:ListBox ID="sub_busair_tier_level" runat="server" Rows="1" Height="20" Width="115px" Font-Size="Small">
                                                    <asp:ListItem Text="Tier 1" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Tier 2" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Tier 3" Value="3"></asp:ListItem>
                                                </asp:ListBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="model_TableRow_15" runat="server">
                                            <asp:TableCell ID="model_TableCell_29" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:CheckBox ID="sub_business_aircraft_flag" runat="server" Text=" Business Aircraft" TextAlign="Right" />
                                            </asp:TableCell>
                                            <asp:TableCell ID="Table_product_code_Cell2" runat="server" HorizontalAlign="Left" VerticalAlign="Middle">
                                                <asp:CheckBox ID="sub_helicopters_flag" runat="server" Text=" Helicopters" TextAlign="Right" />
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow4" runat="server">
                                            <asp:TableCell ID="TableCell3" runat="server" HorizontalAlign="Left" VerticalAlign="Middle">
                                                <asp:CheckBox ID="sub_commerical_flag" runat="server" Text=" Commercial" TextAlign="Right" />
                                            </asp:TableCell>
                                            <asp:TableCell ID="Table_product_code_Cell3" runat="server" HorizontalAlign="Left" VerticalAlign="Middle">
                                                <asp:CheckBox ID="sub_yacht_flag" runat="server" Text=" Yachts" TextAlign="Right" />
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="Table_product_code_Row2" runat="server">
                                            <asp:TableCell ID="Table_product_code_Cell4" runat="server" HorizontalAlign="Left" VerticalAlign="Middle" ColumnSpan="2">
                                                <asp:CheckBox ID="sub_aerodex_flag" runat="server" Text=" Aerodex" TextAlign="Right" />
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow7" runat="server">
                                            <asp:TableCell ID="Table_product_code_Cell6" runat="server" HorizontalAlign="Left" VerticalAlign="Middle" ColumnSpan="2">
                                                <asp:CheckBox ID="sub_history_flag" runat="server" Text=" History (API only)" TextAlign="Right" />
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow8" runat="server">
                                            <asp:TableCell ID="TableCell4" runat="server" HorizontalAlign="Left" VerticalAlign="Middle">
                                                <asp:CheckBox ID="sub_sale_price_flag" runat="server" Text=" Values" TextAlign="Right" />&nbsp;&nbsp;<asp:Label ID="Label8" runat="server" Text="Licenses"></asp:Label>&nbsp;:
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell15" runat="server" HorizontalAlign="Left" VerticalAlign="Middle">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="sub_nbr_of_spi_installs" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" />
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                            </div>
                            <div class="four columns removeLeftMargin">
                                <div class="Box">
                                    <div class="subHeader">
                                        <asp:Label ID="Label4" runat="server" Text="PREFERENCES / LIMITS"></asp:Label>
                                    </div>
                                    <asp:Table ID="Table_pref_1" runat="server" Width="100%" CssClass="formatTable blue">
                                        <asp:TableRow ID="TableRow2" runat="server">
                                            <asp:TableCell ID="TableCell5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label9" runat="server" Text="Export&nbsp;Limit"></asp:Label>&nbsp;:
                                            </asp:TableCell><asp:TableCell ID="TableCell6" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="sub_max_allowed_custom_export" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow9" runat="server">
                                            <asp:TableCell ID="TableCell7" runat="server" HorizontalAlign="Left" VerticalAlign="Middle" ColumnSpan="2">
                                                <asp:CheckBox ID="sub_share_by_comp_id_flag" runat="server" Text="" TextAlign="Left" />&nbsp;
                            <asp:Label ID="Label18" runat="server" Text="Share data with others in my company"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow10" runat="server">
                                            <asp:TableCell ID="TableCell8" runat="server" HorizontalAlign="Left" VerticalAlign="Middle" ColumnSpan="2">
                                                <asp:CheckBox ID="sub_share_by_parent_sub_id_flag" runat="server" Text="" TextAlign="Left" />&nbsp;
                            <asp:Label ID="Label16" runat="server" Text="Share data with same parent company"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow11" runat="server">
                                            <asp:TableCell ID="TableCell9" runat="server" HorizontalAlign="Left" VerticalAlign="Middle" ColumnSpan="2">
                                                <asp:CheckBox ID="sub_abi_flag" runat="server" Text="" TextAlign="Left" />&nbsp;
                            <asp:Label ID="Label15" runat="server" Text="Show My Aircraft on JETNET Global"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow12" runat="server">
                                            <asp:TableCell ID="TableCell10" runat="server" HorizontalAlign="Left" VerticalAlign="Middle" ColumnSpan="2">
                                                <asp:CheckBox ID="sub_marketing_flag" runat="server" Text="" TextAlign="Left" />&nbsp;
                            <asp:Label ID="Label14" runat="server" Text="Marketing / Demo Account Only"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow19" runat="server">
                                            <asp:TableCell ID="TableCell22" runat="server" HorizontalAlign="Left" VerticalAlign="Middle" ColumnSpan="2">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label20" runat="server" Text="Expires in "></asp:Label>
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="sub_nbr_days_expire" runat="server" Width="45px" Height="20px" placeholder="0" Style="text-align: right" />
                                                <asp:Label ID="Label5" runat="server" Text=" days"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow13" runat="server">
                                            <asp:TableCell ID="TableCell11" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label10" runat="server" Text="Notes"></asp:Label>&nbsp;:
                                            </asp:TableCell><asp:TableCell ID="TableCell12" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:ListBox ID="sub_notes" runat="server" Rows="1" Height="20" Width="165px" Font-Size="Small">
                                                    <asp:ListItem Text="Off" Value="sub_notes_off"></asp:ListItem>
                                                    <asp:ListItem Text="Cloud Notes" Value="sub_cloud_notes_flag"></asp:ListItem>
                                                    <asp:ListItem Text="Cloud Notes Plus" Value="sub_server_side_notes_flag"></asp:ListItem>
                                                </asp:ListBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="twelve columns removeLeftMargin">
                                <div class="Box">
                                    <div class="subHeader">
                                        <asp:Label ID="Label21" runat="server" Text="LICENSES"></asp:Label>
                                    </div>
                                    <asp:TextBox runat="server" ID="selected_rows_licences" CssClass="display_none"></asp:TextBox>
                                    <div runat="server" id="div_results_table_licences" class="sixteen columns removeLeftMargin">
                                        <div style="text-align: center; width: 100%;" runat="server" id="searchResultsDiv_licences">
                                            <asp:Label ID="searchResultsTable_licences" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="twelve columns">
                                <div class="Box" runat="server" visible="false">
                                    <div class="subHeader">
                                        <asp:Label ID="Label23" runat="server" Text="CUSTOMER ACTIVITIES"></asp:Label>
                                    </div>
                                    <asp:Table ID="Table1" runat="server" Width="100%" CssClass="formatTable blue">
                                        <asp:TableRow ID="TableRow27" runat="server">
                                            <asp:TableCell ID="TableCell33" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <div style='max-height: 670px; overflow: auto;'>
                                                    <asp:Label ID="customerActivities_Label" runat="server" Text=""></asp:Label>
                                                </div>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="six columns">
                                <div class="Box">
                                    <div class="subHeader">
                                        <asp:Label ID="Label26" runat="server" Text="CONTRACT LIST"></asp:Label>
                                    </div>
                                    <asp:Table ID="Table4" runat="server" Width="100%" CssClass="formatTable blue">
                                        <asp:TableRow ID="TableRow31" runat="server">
                                            <asp:TableCell ID="TableCell37" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <div style='max-height: 670px; overflow: auto;'>
                                                    <asp:Label ID="contractList_Label" runat="server" Text=""></asp:Label>
                                                </div>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                            </div>
                            <div class="six columns">
                                <div class="Box">
                                    <div class="subHeader">
                                        <asp:Label ID="Label25" runat="server" Text="CONTRACT EXECUTION"></asp:Label>
                                    </div>

                                    <div style='max-height: 670px; overflow: auto;'>
                                        <asp:Label ID="contractExecution_Label" runat="server" Text=""></asp:Label>
                                    </div>

                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="div_clear"></div>
                <asp:UpdatePanel ID="subscription_login_install" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional" Visible="false">
                    <ContentTemplate>
                        <span class="dropdownBoxContainer">
                        <asp:Label ID="Label56" runat="server" Text="INSTALL SEQUENCE"></asp:Label><br />
                        <asp:ListBox ID="subins_platform_name" runat="server" Rows="1" Height="20" Font-Size="Small">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                        </asp:ListBox></span><div id="saveLoginButton" style="text-align: right; padding-right: 16px; padding-bottom: 6px;">
                            <asp:Button ID="saveLoginInstall" runat="server" Text="Save Login/Install" CssClass="button-darker" OnClientClick="javascript:ShowLoadingMessage('DivLoadingMessage', 'Saving LoginLogin/Install', 'Saving ... Please Wait ...');return true;" PostBackUrl="~/homebaseSubscription.aspx?task=saveLogin" />
                        </div><div class="div_clear"></div>
                        <div class="row">
                            <div class="four columns">

                                <div class="Box">
                                    <div class="subHeader">
                                        <asp:Label ID="Label12" runat="server" Text="LICENSE SETTINGS"></asp:Label>
                                    </div>
                                    <asp:Table ID="Table6" runat="server" Width="100%" CssClass="formatTable blue">
                                        <asp:TableRow ID="TableRow22" runat="server">
                                            <asp:TableCell ID="TableCell26" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label13" runat="server" Text="Status"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell27" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="sublogin_active_flag" runat="server" Width="115px" Height="20px" placeholder="" Enabled="false" BackColor="LightGray" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow23" runat="server">
                                            <asp:TableCell ID="TableCell28" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label17" runat="server" Text="Login/Seq#"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell29" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="sublogin_login_subins_seq_no" runat="server" Width="115px" Height="20px" placeholder="0/0" Enabled="false" BackColor="LightGray" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow32" runat="server">
                                            <asp:TableCell ID="TableCell38" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label19" runat="server" Text="Password"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell39" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="sublogin_password" runat="server" Width="115px" Height="20px" placeholder="" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow47" runat="server">
                                            <asp:TableCell ID="TableCell60" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label27" runat="server" Text="Amount"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell61" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="sublogin_contract_amount" runat="server" Width="115px" Height="20px" placeholder="$" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow48" runat="server">
                                            <asp:TableCell ID="TableCell62" VerticalAlign="Middle" HorizontalAlign="Left" runat="server" ColumnSpan="2">
                                                <asp:Label ID="Label29" runat="server" Text="Administrator"></asp:Label>&nbsp;
                                <asp:CheckBox ID="subins_admin_flag" runat="server" Text="" TextAlign="Left" />
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                                <div class="Box">
                                    <div class="subHeader">
                                        <asp:Label ID="Label22" runat="server" Text="STATUS / DATES"></asp:Label>
                                    </div>
                                    <asp:Table ID="Table7" runat="server" Width="100%" CssClass="formatTable blue">
                                        <asp:TableRow ID="TableRow34" runat="server">
                                            <asp:TableCell ID="TableCell41" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label24" runat="server" Text="Install Date"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell64" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_install_date" runat="server" Width="75px" Height="20px" placeholder="" Style="text-align: right" Visible="true"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow49" runat="server">
                                            <asp:TableCell ID="TableCell65" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label28" runat="server" Text="Last Access"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell66" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_access_date" runat="server" Width="75px" Height="20px" placeholder="" Style="text-align: right" Visible="true"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow50" runat="server">
                                            <asp:TableCell ID="TableCell67" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label48" runat="server" Text="Entry Date"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell68" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="sublogin_entry_date" runat="server" Width="75px" Height="20px" placeholder="" Style="text-align: right" Visible="true"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow51" runat="server">
                                            <asp:TableCell ID="TableCell69" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label49" runat="server" Text="Environment"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell70" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_platform_os" runat="server" Width="115px" Height="20px" placeholder="" Enabled="false" BackColor="LightGray" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow69" runat="server">
                                            <asp:TableCell ID="TableCell97" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label62" runat="server" Text="Default Business Type"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell98" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_business_type_code" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow70" runat="server">
                                            <asp:TableCell ID="TableCell99" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label63" runat="server" Text="Last Session GUID"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell100" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_session_guid" runat="server" Width="205px" Height="20px" placeholder="" Enabled="false" BackColor="LightGray" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                            </div>
                            <div class="four columns">
                                <div class="Box">
                                    <div class="subHeader">
                                        <asp:Label ID="Label30" runat="server" Text="FEATURES"></asp:Label>
                                    </div>
                                    <asp:Table ID="Table8" runat="server" Width="100%" CssClass="formatTable blue">
                                        <asp:TableRow ID="TableRow36" runat="server">
                                            <asp:TableCell ID="TableCell43" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:CheckBox ID="sublogin_demo_flag" runat="server" Text="" TextAlign="Left" />&nbsp;
                                <asp:Label ID="Label31" runat="server" Text="Demo"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow71" runat="server">
                                            <asp:TableCell ID="TableCell101" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:CheckBox ID="sub_marketing_flag2" runat="server" Text="" TextAlign="Left" Enabled="false" />&nbsp;
                                <asp:Label ID="Label64" runat="server" Text="Marketing"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow39" runat="server">
                                            <asp:TableCell ID="TableCell44" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:CheckBox ID="sublogin_values_flag" runat="server" Text="" TextAlign="Left" />&nbsp;
                                <asp:Label ID="Label32" runat="server" Text="Values"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow37" runat="server">
                                            <asp:TableCell ID="TableCell45" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:CheckBox ID="sublogin_mpm_flag" runat="server" Text="" TextAlign="Left" />&nbsp;
                                <asp:Label ID="Label33" runat="server" Text="MPM"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow40" runat="server">
                                            <asp:TableCell ID="TableCell46" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:CheckBox ID="sublogin_allow_export_flag" runat="server" Text="" TextAlign="Left" />&nbsp;
                                <asp:Label ID="Label34" runat="server" Text="Export"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow72" runat="server">
                                            <asp:TableCell ID="TableCell102" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:CheckBox ID="sublogin_allow_event_request_flag" runat="server" Text="" TextAlign="Left" />&nbsp;
                                <asp:Label ID="Label65" runat="server" Text="Events"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow38" runat="server">
                                            <asp:TableCell ID="TableCell47" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:CheckBox ID="sublogin_allow_local_notes_flag" runat="server" Text="" TextAlign="Left" />&nbsp;
                                <asp:Label ID="Label35" runat="server" Text="Local Notes"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow52" runat="server">
                                            <asp:TableCell ID="TableCell71" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:CheckBox ID="sublogin_allow_text_message_flag" runat="server" Text="" TextAlign="Left" />&nbsp;
                                <asp:Label ID="Label50" runat="server" Text="Text Messages"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow54" runat="server">
                                            <asp:TableCell ID="TableCell72" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:CheckBox ID="subins_evo_mobile_flag" runat="server" Text="" TextAlign="Left" />&nbsp;
                                <asp:Label ID="Label51" runat="server" Text="Mobile"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                                <div class="Box">
                                    <div class="subHeader">
                                        <asp:Label ID="Label55" runat="server" Text="EMAIL REQUESTS"></asp:Label>
                                    </div>
                                    <asp:Table ID="Table9" runat="server" Width="100%" CssClass="formatTable blue">
                                        <asp:TableRow ID="TableRow53" runat="server">
                                            <asp:TableCell ID="TableCell48" VerticalAlign="Middle" HorizontalAlign="Left" runat="server" ColumnSpan="2">
                                                <asp:CheckBox ID="sublogin_allow_email_request_flag" runat="server" Text="" TextAlign="Left" />&nbsp;
                                <asp:Label ID="Label36" runat="server" Text="Email Requests"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow65" runat="server">
                                            <asp:TableCell ID="TableCell90" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label57" runat="server" Text="HTML / TEXT"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell91" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:RadioButton ID="email_format_html" runat="server" Text="HTML" /><asp:RadioButton
                                                    ID="email_format_text" runat="server" Text="TEXT" />
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow66" runat="server">
                                            <asp:TableCell ID="TableCell92" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label58" runat="server" Text="Reply Email"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell93" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_email_replyaddress" runat="server" Width="185px" Height="20px" placeholder="" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow67" runat="server">
                                            <asp:TableCell ID="TableCell94" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label59" runat="server" Text="Reply Name"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell95" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_email_replyname" runat="server" Width="185px" Height="20px" placeholder="" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                            </div>
                            <div class="four columns">
                                <div class="Box">
                                    <div class="subHeader">
                                        <asp:Label ID="Label39" runat="server" Text="USER SETTINGS"></asp:Label>
                                    </div>
                                    <asp:Table ID="Table10" runat="server" Width="100%" CssClass="formatTable blue">
                                        <asp:TableRow ID="TableRow42" runat="server">
                                            <asp:TableCell ID="TableCell49" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label37" runat="server" Text="Default View"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell50" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_evoview_id" runat="server" Width="175px" Height="20px" placeholder="" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow43" runat="server">
                                            <asp:TableCell ID="TableCell52" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label38" runat="server" Text="Records/Page"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell53" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_nbr_rec_per_page" runat="server" Width="115px" Height="20px" placeholder="" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow44" runat="server">
                                            <asp:TableCell ID="TableCell54" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label40" runat="server" Text="Analysis Months"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell55" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_default_analysis_months" runat="server" Width="115px" Height="20px" placeholder="" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow55" runat="server">
                                            <asp:TableCell ID="TableCell56" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label41" runat="server" Text="Background"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell57" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_background_image_id" runat="server" Width="115px" Height="20px" placeholder="" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow56" runat="server">
                                            <asp:TableCell ID="TableCell73" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label42" runat="server" Text="Default View Model"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell74" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_default_amod_id" runat="server" Width="115px" Height="20px" placeholder="" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow62" runat="server">
                                            <asp:TableCell ID="TableCell63" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label54" runat="server" Text="Default Airports"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell85" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:Label CssClass="homebaseTextBoxFont" ID="subins_default_airports" runat="server" Style="text-align: left" BorderColor="LightGray" BorderStyle="Solid" BorderWidth="1" Width="115px">
                                                </asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow64" runat="server">
                                            <asp:TableCell ID="TableCell86" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label60" runat="server" Text="Default Models"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell88" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:Label CssClass="homebaseTextBoxFont" ID="subins_default_models" runat="server" Style="text-align: left" BorderColor="LightGray" BorderStyle="Solid" BorderWidth="1" Width="115px">
                                                </asp:Label>
                                            </asp:TableCell>

                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                                <div class="Box">
                                    <div class="subHeader">
                                        <asp:Label ID="Label46" runat="server" Text="TEXT SERVICES"></asp:Label>
                                    </div>
                                    <asp:Table ID="Table11" runat="server" CssClass="formatTable blue" Width="100%">
                                        <asp:TableRow ID="TableRow73" runat="server">
                                            <asp:TableCell ID="TableCell103" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label66" runat="server" Text="Status"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell104" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_smstxt_active_flag" runat="server" Width="115px" Height="20px" placeholder="" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow57" runat="server">
                                            <asp:TableCell ID="TableCell75" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label43" runat="server" Text="Cell Number"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell76" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_cell_number" runat="server" Width="115px" Height="20px" placeholder="" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow68" runat="server">
                                            <asp:TableCell ID="TableCell89" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label61" runat="server" Text="Service"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell96" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_cell_service" runat="server" Width="115px" Height="20px" placeholder="" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow59" runat="server">
                                            <asp:TableCell ID="TableCell79" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label45" runat="server" Text="Models"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell80" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:Label CssClass="homebaseTextBoxFont" ID="subins_smstxt_models" runat="server" Style="text-align: left" BorderColor="LightGray" BorderStyle="Solid" BorderWidth="1" Width="115px">
                                                </asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow60" runat="server">
                                            <asp:TableCell ID="TableCell81" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label52" runat="server" Text="Events"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell82" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:Label CssClass="homebaseTextBoxFont" ID="subins_sms_events" runat="server" Style="text-align: left" BorderColor="LightGray" BorderStyle="Solid" BorderWidth="1" Width="115px">
                                                </asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow61" runat="server">
                                            <asp:TableCell ID="TableCell83" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                                <asp:Label ID="Label53" runat="server" Text="Active Date"></asp:Label>
                                            </asp:TableCell><asp:TableCell ID="TableCell84" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="subins_mobile_active_date" runat="server" Width="75px" Height="20px" placeholder="" Enabled="false" BackColor="LightGray" Style="text-align: right">
                                                </asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
    <div id="DivLoadingMessage" style="display: none;">
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

    <script type="text/javascript">

        function ActiveTabChanged(sender, args) {

            var nextTab = sender.get_activeTab().get_id();

            if (nextTab.indexOf("tab1") > 0) {
                //alert("finder preferences");
                //swapChosenDropdowns();
            }

        }

        function ShowLoadingMessage(DivTag, Title, Message) {
            $("#" + DivTag).html(Message);
            $("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 75, resizable: false });
        }

        function CloseLoadingMessage(DivTag) {
            $("#" + DivTag).dialog("close");
        }

        function resizeImages() {

            var img = $(".pictureResize"); // Get my img elem
            var pic_real_width, pic_real_height;
            img.on('load', function () {

                var testStr = '#container-' + $(this).attr('id')
                var containerToChange = $(testStr);
                //alert(testStr);
                pic_real_width = this.width;   // Note: $(this).width() will not
                pic_real_height = this.height; // work for in memory images.

                //alert(pic_real_width + ' ' + pic_real_height);
                if (pic_real_width > pic_real_height) {

                    containerToChange.addClass("circular--landscape");
                    $(this).removeClass("pictureResize");
                } else if (pic_real_width < pic_real_height) {
                    containerToChange.addClass("circular--portrait");
                    $(this).removeClass("pictureResize");


                } else {
                    $(this).removeClass("pictureResize");
                    $(this).addClass("circular--square");

                }
            }).each(function () {
                if (this.complete) $(this).load();
            });
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
                    case "licences_DataTable":
                        {
                            selectedRows = "<%= selected_rows_licences.ClientID %>";
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
                            //selectAllRows(dataRows.data(), selectedRows, tableName);

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
                        { targets: [0], className: 'display_none' },
                    ],
                    order: [[3, 'asc']],
                    dom: 'Bfitrp',
                    buttons: [
                        { extend: 'csv', exportOptions: { columns: ':visible' } },
                        { extend: 'excel', exportOptions: { columns: ':visible' } },
                        { extend: 'pdf', orientation: 'landscape', pageSize: 'A2', exportOptions: { columns: ':visible' } },
                        { extend: 'colvis', text: 'Columns', collectionLayout: 'fixed two-column', postfixButtons: ['colvisRestore'] },

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

            //$("RefreshTableValue").addClass('display_none');

            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            $($.fn.dataTable.tables(true)).DataTable().scroller.measure();

        };

    </script>

</asp:Content>
