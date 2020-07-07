<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/EmptyHomebaseTheme.Master" CodeBehind="homebaseEditAircraftModel.aspx.vb"
  Inherits="crmWebClient.homebaseEditAircraftModel" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyHomebaseTheme.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <link rel="Stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.24/themes/smoothness/jquery-ui.css" />
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
  </style>

    <script type="text/javascript">

    function openSmallWindowJS(address, windowname) {
      var rightNow = new Date();
      windowname += rightNow.getTime();
      var Place = window.open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1150,resizable=yes,toolbar=no,location=no,status=no");
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
  <asp:Panel runat="server" ID="contentClass" CssClass="valueViewPDFExport remove_padding">
    <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width='100%' runat="server"
      class="DetailsBrowseTable">
      <asp:TableRow>
        <asp:TableCell HorizontalAlign="right" VerticalAlign="middle">
              <div class="backgroundShade">
                <a href="#" onclick="javascript:window.close();" class="gray_button noBefore float_left"><strong>Close</strong></a>
              </div>
        </asp:TableCell>
      </asp:TableRow>
    </asp:Table>
    <div id="searchPanelContainerDiv" runat="server" width="1050">
      <table border="0" cellpadding="2" cellspacing="0" width="100%">
        <tr>
          <td height="30%" width="100%" align="left" valign="top">
            <div class="row">
              <div style="text-align: left; padding-top: 4px; padding-bottom: 4px;">
                <asp:Label ID="Label18" runat="server" Text="Model : "></asp:Label>
                <asp:DropDownList runat="server" Width="35%" ID="modelList" CssClass="chosen-select" AutoPostBack="true" data-placeholder="Please Pick a Model">
                </asp:DropDownList>
                <div class="mobile_display_on_cell mobileChosenSpacer">
                </div>
              </div>
            </div>
            <h2>
              <asp:ListBox ID="ListBox2" runat="server" Rows="1" Height="20">
                <asp:ListItem Text="rep1"></asp:ListItem>
                <asp:ListItem Text="rep2"></asp:ListItem>
              </asp:ListBox>
              &nbsp; <strong>
                <asp:Label ID="make_model_Label" runat="server" Text="MAKE/MODEL"></asp:Label>
              </strong>&nbsp;&nbsp;<asp:Label ID="model_mfr_Label" runat="server" Text="MANUFACTURER"></asp:Label>
            </h2>
            <div id="modelIntelButton" style="text-align: right; padding-right: 8px;">
                   <asp:Button ID="model_assett_insight_features" runat="server" Text="Model Features/Attributes PDF" CssClass="button-darker" UseSubmitBehavior="false" Visible="false"  />&nbsp;&nbsp;
                <asp:Button ID="model_attributes" runat="server" Text="Model Attributes" CssClass="button-darker" UseSubmitBehavior="false" />&nbsp;&nbsp;
              <asp:Button ID="modelIntel" runat="server" Text="Model Intelligence" CssClass="button-darker" UseSubmitBehavior="false" />&nbsp;&nbsp;
              <asp:Button ID="saveModel0" runat="server" Text="Save Model" CssClass="button-darker button_width" OnClientClick="javascript:ShowLoadingMessage('DivLoadingMessage', 'Saving Model', 'Saving ... Please Wait ...');return true;" PostBackUrl="~/homebaseEditAircraftModel.aspx?task=save" />
            </div>
            <table id="Table_model_outer" border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
              <tr>
                <td align="center" valign="top" style="text-align: center; padding-left: 0px;" width="25%">
                  <h2>
                    <asp:Table ID="Table_model_inner" runat="server" Width="100%" CellPadding="2" CellSpacing="0">
                      <asp:TableRow ID="model_TableRow_1" runat="server">
                        <asp:TableCell ID="model_TableCell_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                          <asp:Label ID="Label_1" runat="server" Text="Make"></asp:Label>&nbsp;:
                        </asp:TableCell>
                        <asp:TableCell ID="model_TableCell_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                          <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_make_name" runat="server" Width="155px" Height="20px" placeholder=""></asp:TextBox>
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow ID="model_TableRow_2" runat="server">
                        <asp:TableCell ID="model_TableCell_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                          <asp:Label ID="Label_2" runat="server" Text="Model"></asp:Label>&nbsp;:
                        </asp:TableCell>
                        <asp:TableCell ID="model_TableCell_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                          <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_model_name" runat="server" Width="155px" Height="20px" placeholder=""></asp:TextBox>
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow ID="model_TableRow_3" runat="server">
                        <asp:TableCell ID="model_TableCell_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                          <asp:Label ID="Label_3" runat="server" Text="Manufacturer"></asp:Label>&nbsp;:
                        </asp:TableCell>
                        <asp:TableCell ID="model_TableCell_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                          <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_manufacturer" runat="server" Width="155px" Height="20px" placeholder=""></asp:TextBox>
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow ID="model_TableRow_3a" runat="server">
                        <asp:TableCell ID="model_TableCell_5a" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                          <asp:Label ID="Label_3a" runat="server" Text="Mfr CompID"></asp:Label>&nbsp;:
                        </asp:TableCell>
                        <asp:TableCell ID="model_TableCell_6a" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                          <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_manufacturer_comp_id" runat="server" Width="155px" Height="20px" placeholder="" Enabled="False" BackColor="LightGray"></asp:TextBox>
                        </asp:TableCell>
                      </asp:TableRow>
                    </asp:Table>
                  </h2>
                </td>
                <td align="center" valign="top" style="text-align: center; padding-left: 0px;" width="25%">
                  <h2>
                    <asp:Table ID="Table_model_inner1" runat="server" Width="100%" CellPadding="2" CellSpacing="0">
                      <asp:TableRow ID="model_TableRow_4" runat="server">
                        <asp:TableCell ID="model_TableCell_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                          <asp:Label ID="Label_4" runat="server" Text="Mk Abbrev"></asp:Label>&nbsp;:
                        </asp:TableCell>
                        <asp:TableCell ID="model_TableCell_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                          <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_make_abbrev" runat="server" Width="145px" Height="20px" placeholder=""></asp:TextBox>
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow ID="model_TableRow_5" runat="server">
                        <asp:TableCell ID="model_TableCell_9" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                          <asp:Label ID="Label_5" runat="server" Text="Mod Abbrev"></asp:Label>&nbsp;:
                        </asp:TableCell>
                        <asp:TableCell ID="model_TableCell_10" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                          <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_model_abbrev" runat="server" Width="145px" Height="20px" placeholder=""></asp:TextBox>
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow ID="model_TableRow_6" runat="server">
                        <asp:TableCell ID="model_TableCell_11" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                          <asp:Label ID="Label_6" runat="server" Text="Common" ToolTip="Mfr Common Name"></asp:Label>&nbsp;:
                        </asp:TableCell>
                        <asp:TableCell ID="model_TableCell_12" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                          <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_manufacturer_common_name" runat="server" Width="145px" Height="20px" placeholder=""></asp:TextBox>
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow ID="model_TableRow_6a" runat="server">
                        <asp:TableCell ID="model_TableCell_11a" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                          <asp:Label ID="Label_6a" runat="server" Text="Model ID"></asp:Label>&nbsp;:
                        </asp:TableCell>
                        <asp:TableCell ID="model_TableCell_12a" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                          <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_id" runat="server" Width="145px" Height="20px" placeholder="" Enabled="False" BackColor="LightGray"></asp:TextBox>
                        </asp:TableCell>
                      </asp:TableRow>
                    </asp:Table>
                  </h2>
                </td>
                <td align="center" valign="top" style="text-align: center; padding-left: 0px;" width="25%">
                  <h2>
                    <asp:Table ID="Table_model_inner2" runat="server" Width="100%" CellPadding="2" CellSpacing="0">
                      <asp:TableRow ID="model_TableRow_7" runat="server">
                        <asp:TableCell ID="model_TableCell_13" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                          <asp:Label ID="Label_7" runat="server" Text="Airframe"></asp:Label>&nbsp;:
                        </asp:TableCell>
                        <asp:TableCell ID="model_TableCell_14" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                          <asp:ListBox ID="amod_airframe_type_code" runat="server" Rows="1" Height="20" Width="155px" Font-Size="Small">
                            <asp:ListItem Text="Fixed Wing" Value="F"></asp:ListItem>
                            <asp:ListItem Text="Rotary" Value="R"></asp:ListItem>
                          </asp:ListBox>
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow ID="model_TableRow_8" runat="server">
                        <asp:TableCell ID="model_TableCell_15" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                          <asp:Label ID="Label_8" runat="server" Text="FAA ID" ToolTip="FAA Model ID"></asp:Label>&nbsp;:
                        </asp:TableCell>
                        <asp:TableCell ID="model_TableCell_16" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                          <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_faa_model_id" runat="server" Width="155px" Height="20px" placeholder=""></asp:TextBox>
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow ID="model_TableRow_9" runat="server">
                        <asp:TableCell ID="model_TableCell_17" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                          <asp:Label ID="Label_9" runat="server" Text="Size"></asp:Label>&nbsp;:
                        </asp:TableCell>
                        <asp:TableCell ID="model_TableCell_18" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                          <asp:ListBox ID="amod_jniq_size" runat="server" Rows="1" Font-Size="Small">
                            <asp:ListItem Text="Airline Business Jet" Value="ABJ"></asp:ListItem>
                            <asp:ListItem Text="Airliner Jet Converted" Value="ALJ"></asp:ListItem>
                            <asp:ListItem Text="Airliner Turbo-Prop Converted" Value="ALTP"></asp:ListItem>
                            <asp:ListItem Text="Large Jet" Value="LGJ"></asp:ListItem>
                            <asp:ListItem Text="Large Long-Range Jet" Value="LGLR"></asp:ListItem>
                            <asp:ListItem Text="Large Ultra Long-Range Jet" Value="LGULR"></asp:ListItem>
                            <asp:ListItem Text="Light Jet" Value="LJ"></asp:ListItem>
                            <asp:ListItem Text="Multi-Engine Piston" Value="MEP"></asp:ListItem>
                            <asp:ListItem Text="Multi-Engine Turbo-Prop" Value="METP"></asp:ListItem>
                            <asp:ListItem Text="Mid-Size Jet" Value="MJ"></asp:ListItem>
                            <asp:ListItem Text="Personal Jet" Value="PJ"></asp:ListItem>
                            <asp:ListItem Text="Single-Engine Piston" Value="SEP"></asp:ListItem>
                            <asp:ListItem Text="Single-Engine Turbo-Prop" Value="SETP"></asp:ListItem>
                            <asp:ListItem Text="Super Light Jet" Value="SLJ"></asp:ListItem>
                            <asp:ListItem Text="Super Mid-Size Jet" Value="SMJ"></asp:ListItem>
                            <asp:ListItem Text="Very Light Jet" Value="VLJ"></asp:ListItem>
                          </asp:ListBox>
                        </asp:TableCell>
                      </asp:TableRow>
                    </asp:Table>
                  </h2>
                </td>
                <td align="center" valign="top" style="text-align: center; padding-left: 0px;">
                  <h2>
                    <asp:Table ID="Table_model_inner3" runat="server" Width="100%" CellPadding="2" CellSpacing="0">
                      <asp:TableRow ID="model_TableRow_10" runat="server">
                        <asp:TableCell ID="model_TableCell_19" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                          <asp:Label ID="Label_10" runat="server" Text="Type"></asp:Label>&nbsp;:
                        </asp:TableCell>
                        <asp:TableCell ID="model_TableCell_20" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                          <asp:ListBox ID="amod_type_code" runat="server" Rows="1" Height="20" Width="155px" Font-Size="Small">
                            <asp:ListItem Text="Jet Airliner" Value="E"></asp:ListItem>
                            <asp:ListItem Text="Business Jet" Value="J"></asp:ListItem>
                            <asp:ListItem Text="Turbo Prop" Value="TP"></asp:ListItem>
                            <asp:ListItem Text="Turbine" Value="T"></asp:ListItem>
                            <asp:ListItem Text="Piston" Value="P"></asp:ListItem>
                          </asp:ListBox>
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow ID="model_TableRow_11" runat="server">
                        <asp:TableCell ID="model_TableCell_21" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                          <asp:Label ID="Label_11" runat="server" Text="Class"></asp:Label>&nbsp;:
                        </asp:TableCell>
                        <asp:TableCell ID="model_TableCell_22" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                          <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_class_code" runat="server" Width="155px" Height="20px" placeholder=""></asp:TextBox>
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow ID="model_TableRow_12" runat="server">
                        <asp:TableCell ID="model_TableCell_23" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                          <asp:Label ID="Label_12" runat="server" Text="Weight" ToolTip="Weight Class"></asp:Label>&nbsp;:
                        </asp:TableCell>
                        <asp:TableCell ID="model_TableCell_24" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                          <asp:ListBox ID="amod_weight_class" runat="server" Rows="1" Height="20" Width="155px" Font-Size="Small">
                            <asp:ListItem Text="Very Light Jet" Value="V"></asp:ListItem>
                            <asp:ListItem Text="Heavy" Value="H"></asp:ListItem>
                            <asp:ListItem Text="Light" Value="L"></asp:ListItem>
                            <asp:ListItem Text="Medium" Value="M"></asp:ListItem>
                          </asp:ListBox>
                        </asp:TableCell>
                      </asp:TableRow>
                    </asp:Table>
                  </h2>
                </td>
              </tr>
              <tr>
                <td colspan="4">
                  <table>
                    <tr>
                      <td align="center" valign="top" style="text-align: center; padding-left: 0px;" width="35%">
                        <h2>
                          <strong>
                            <asp:Label ID="Label_13" runat="server" Text="Years Built"></asp:Label>
                          </strong>
                          <asp:Table ID="Table_model_inner4" runat="server" Width="100%" CellPadding="2" CellSpacing="0">
                            <asp:TableRow ID="model_TableRow_13" runat="server">
                              <asp:TableCell ID="model_TableCell_25" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                <em>start</em>&nbsp;:&nbsp;
                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_start_year" runat="server" Width="85px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                              </asp:TableCell>
                              <asp:TableCell ID="model_TableCell_26" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                <em>end</em>&nbsp;:&nbsp;
                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_end_year" runat="server" Width="85px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                              </asp:TableCell>
                            </asp:TableRow>
                          </asp:Table>
                          <strong>
                            <asp:Label ID="Label_14" runat="server" Text="Price Range"></asp:Label>
                          </strong>
                          <asp:Table ID="Table_model_inner5" runat="server" Width="100%" CellPadding="2" CellSpacing="0">
                            <asp:TableRow ID="model_TableRow_14" runat="server">
                              <asp:TableCell ID="model_TableCell_27" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                <em>low</em>&nbsp;:&nbsp;$
                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_start_price" runat="server" Width="105px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                              </asp:TableCell>
                              <asp:TableCell ID="model_TableCell_28" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                <em>high</em>&nbsp;:&nbsp;$
                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_end_price" runat="server" Width="105px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                              </asp:TableCell>
                            </asp:TableRow>
                          </asp:Table>
                          <strong>
                            <asp:Label ID="Label17" runat="server" Text="Description"></asp:Label>
                          </strong>
                          <asp:Table ID="Table_model_inner5a" runat="server" Width="100%" CellPadding="2" CellSpacing="0">
                            <asp:TableRow ID="model_TableRow_14a" runat="server">
                              <asp:TableCell ID="model_TableCell_28a" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_description" runat="server" Rows="5" Width="98%" placeholder="" TextMode="MultiLine"></asp:TextBox>
                              </asp:TableCell>
                            </asp:TableRow>
                          </asp:Table>
                        </h2>
                      </td>
                      <td align="center" valign="top" style="text-align: center; padding-left: 0px;" width="35%">
                        <h2>
                          <strong>
                            <asp:Label ID="Label_15" runat="server" Text="Product Code"></asp:Label>
                          </strong>
                          <asp:Table ID="Table_model_inner6" runat="server" Width="100%" CellPadding="2" CellSpacing="0">
                            <asp:TableRow ID="model_TableRow_15" runat="server">
                              <asp:TableCell ID="model_TableCell_29" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                <asp:Table ID="Table_product_code" runat="server">
                                  <asp:TableRow ID="Table_product_code_Row1" runat="server" VerticalAlign="Middle">
                                    <asp:TableCell ID="Table_product_code_Cell1" runat="server">
                                      <asp:CheckBox ID="amod_product_business_flag" runat="server" Text="Business" />
                                    </asp:TableCell>
                                    <asp:TableCell ID="Table_product_code_Cell2" runat="server" VerticalAlign="Middle">
                                      <asp:CheckBox ID="amod_product_commercial_flag" runat="server" Text="Commercial" />
                                    </asp:TableCell>
                                    <asp:TableCell ID="Table_product_code_Cell3" runat="server" VerticalAlign="Middle">
                                      <asp:CheckBox ID="amod_product_airbp_flag" runat="server" Text="Air BP" />
                                    </asp:TableCell>
                                  </asp:TableRow>
                                  <asp:TableRow ID="Table_product_code_Row2" runat="server">
                                    <asp:TableCell ID="Table_product_code_Cell4" runat="server" VerticalAlign="Middle">
                                      <asp:CheckBox ID="amod_product_helicopter_flag" runat="server" Text="Helicopter" />
                                    </asp:TableCell>
                                    <asp:TableCell ID="Table_product_code_Cell5" runat="server" VerticalAlign="Middle">
                                      <asp:CheckBox ID="amod_product_abi_flag" runat="server" Text="ABI" />
                                    </asp:TableCell>
                                    <asp:TableCell ID="Table_product_code_Cell6" runat="server" VerticalAlign="Middle">
                                      <asp:CheckBox ID="amod_product_regional_flag" runat="server" Text="Regional" />
                                    </asp:TableCell>
                                  </asp:TableRow>
                                </asp:Table>
                              </asp:TableCell>
                            </asp:TableRow>
                          </asp:Table>
                          <strong>
                            <asp:Label ID="Label_16" runat="server" Text="Body Config"></asp:Label>
                          </strong>
                          <asp:Table ID="Table_model_inner7" runat="server" Width="100%" CellPadding="2" CellSpacing="0">
                            <asp:TableRow ID="model_TableRow_16" runat="server">
                              <asp:TableCell ID="model_TableCell_30" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                <asp:ListBox ID="amod_body_config" runat="server" Rows="1" Height="20" Font-Size="Small">
                                  <asp:ListItem Text="Combi" Value="C"></asp:ListItem>
                                  <asp:ListItem Text="Freighter" Value="F"></asp:ListItem>
                                  <asp:ListItem Text="Narrowbody" Value="NB"></asp:ListItem>
                                  <asp:ListItem Text="Passenger Freight Convertible" Value="PF"></asp:ListItem>
                                  <asp:ListItem Text="Regional" Value="R"></asp:ListItem>
                                  <asp:ListItem Text="Single Engine" Value="SE"></asp:ListItem>
                                  <asp:ListItem Text="Turboprop Commercial" Value="TC"></asp:ListItem>
                                  <asp:ListItem Text="Twin Engine" Value="TE"></asp:ListItem>
                                  <asp:ListItem Text="Unknown" Value="U"></asp:ListItem>
                                  <asp:ListItem Text="Widebody" Value="WB"></asp:ListItem>
                                </asp:ListBox>
                              </asp:TableCell>
                            </asp:TableRow>
                          </asp:Table>
                        </h2>
                      </td>
                      <td align="center" valign="top" style="text-align: center; padding-left: 0px;" width="30%">
                        <h2>
                          <strong>
                            <asp:Label ID="Label_17" runat="server" Text="Serial Number"></asp:Label>
                          </strong>
                          <asp:Table ID="Table_model_inner8" runat="server" Width="50%" CellPadding="2" CellSpacing="0">
                            <asp:TableRow ID="model_TableRow_16a" runat="server">
                              <asp:TableCell ID="model_TableCell_31" VerticalAlign="Middle" HorizontalAlign="Right" runat="server" ColumnSpan="2">
                                <asp:CheckBox ID="amod_serno_hyphen_flag" runat="server" Text="Hyphen ?" />
                              </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="model_TableRow_17" runat="server">
                              <asp:TableCell ID="model_TableCell_33" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                <asp:Label ID="Label_18" runat="server" Text="Prefix"></asp:Label>&nbsp;:
                              </asp:TableCell>
                              <asp:TableCell ID="model_TableCell_34" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_ser_no_prefix" runat="server" Width="155px" Height="20px" placeholder=""></asp:TextBox>
                              </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="model_TableRow_18" runat="server">
                              <asp:TableCell ID="model_TableCell_35" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                <asp:Label ID="Label_19" runat="server" Text="Start"></asp:Label>&nbsp;:
                              </asp:TableCell>
                              <asp:TableCell ID="model_TableCell_36" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_ser_no_start" runat="server" Width="155px" Height="20px" placeholder=""></asp:TextBox>
                              </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="model_TableRow_19" runat="server">
                              <asp:TableCell ID="model_TableCell_37" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                <asp:Label ID="Label_20" runat="server" Text="End"></asp:Label>&nbsp;:
                              </asp:TableCell>
                              <asp:TableCell ID="model_TableCell_38" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_ser_no_end" runat="server" Width="155px" Height="20px" placeholder=""></asp:TextBox>
                              </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="model_TableRow_20" runat="server">
                              <asp:TableCell ID="model_TableCell_39" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                <asp:Label ID="Label_21" runat="server" Text="Suffix"></asp:Label>&nbsp;:
                              </asp:TableCell>
                              <asp:TableCell ID="TableCell1" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_ser_no_suffix" runat="server" Width="155px" Height="20px" placeholder=""></asp:TextBox>
                              </asp:TableCell>
                            </asp:TableRow>
                          </asp:Table>
                        </h2>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td width="100%">
            <div style='max-height: 670px; overflow: auto;'>
              <cc1:TabContainer runat="server" ID="tabContainer1" Width="100%" ActiveTabIndex="0" OnClientActiveTabChanged="ActiveTabChanged"
                BorderStyle="None" Style="margin-left: auto; margin-right: auto; text-align: left;" CssClass="dark-theme">
                <cc1:TabPanel ID="tab1" runat="server" HeaderText="Performance Specs">
                  <HeaderTemplate>
                    Performance Specs
                  </HeaderTemplate>
                  <ContentTemplate>
                    <div style="text-align: right; padding-right: 8px;">
                      <asp:Button ID="saveModel1" runat="server" Text="Save Model" CssClass="button-darker button_width" OnClientClick="javascript:ShowLoadingMessage('DivLoadingMessage', 'Saving Model', 'Saving ... Please Wait ...');return true;" PostBackUrl="~/homebaseEditAircraftModel.aspx?task=save" />
                    </div>
                    <div style="text-align: left; padding-left: 8px;">
                      <font color="maroon">*<em>Fixed Wing only</em></font>&nbsp;&nbsp;&nbsp;<font color="blue">^<em>Rotary only</em></font>
                    </div>
                    <table id="Table_perf_outer" border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
                      <tr>
                        <td align="center" valign="top" style="text-align: center; padding-left: 0px;" width="50%">
                          <h2>
                            <strong>
                              <asp:Label ID="Label_perf_title_1" runat="server" Text="FUSELAGE DIMENSIONS"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_perf_1" runat="server" Width="90%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_perf_1_1" runat="server">
                                <asp:TableCell ID="TableCell_perf_1_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_1" runat="server" Text="Length"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_1_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_fuselage_length" runat="server" Width="155px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_1_2" runat="server">
                                <asp:TableCell ID="TableCell_perf_1_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_2" runat="server" Text="Height"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_1_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_fuselage_height" runat="server" Width="155px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_1_3" runat="server">
                                <asp:TableCell ID="TableCell_perf_1_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_3" runat="server" Text="Wing Span / Width"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_1_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_fuselage_wingspan" runat="server" Width="155px" Height="20px" placeholder="0" Style="text-align: right" Visible="true"></asp:TextBox>
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_fuselage_width" runat="server" Width="155px" Height="20px" placeholder="0" Style="text-align: right" Visible="false"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <strong>
                              <asp:Label ID="Label_perf_title_2" runat="server" Text="TYPICAL CONFIGURATION"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_perf_2" runat="server" Width="90%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_perf_2_1" runat="server">
                                <asp:TableCell ID="TableCell_perf_2_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label1_perf_4" runat="server" Text="Crew"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_2_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_number_of_crew" runat="server" Width="155px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_2_2" runat="server">
                                <asp:TableCell ID="TableCell_perf_2_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label1_perf_5" runat="server" Text="Passengers"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_2_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_number_of_passengers" runat="server" Width="155px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_2_3" runat="server">
                                <asp:TableCell ID="TableCell_perf_2_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <font color="maroon">*</font>
                                  <asp:Label ID="Label1_perf_6" runat="server" Text="Pressurization"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_2_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_pressure" runat="server" Width="155px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>psi</em>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <strong>
                              <asp:Label ID="Label_perf_title_3" runat="server" Text="WEIGHT"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_perf_3" runat="server" Width="90%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_perf_3_1" runat="server">
                                <asp:TableCell ID="TableCell_perf_3_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_7" runat="server" Text="Max. Ramp"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_3_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_max_ramp_weight" runat="server" Width="155px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_3_2" runat="server">
                                <asp:TableCell ID="TableCell_perf_3_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_8" runat="server" Text="Max. Takeoff"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_3_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_max_takeoff_weight" runat="server" Width="155px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_3_3" runat="server">
                                <asp:TableCell ID="TableCell_perf_3_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <font color="maroon">*</font>
                                  <asp:Label ID="Label_perf_9" runat="server" Text="Zero Fuel"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_3_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_zero_fuel_weight" runat="server" Width="155px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_3_4" runat="server">
                                <asp:TableCell ID="TableCell_perf_3_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_10" runat="server" Text="EOW" ToolTip="Empty Operating Weight"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_3_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_weight_eow" runat="server" Width="155px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_3_5" runat="server">
                                <asp:TableCell ID="TableCell_perf_3_9" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_11" runat="server" Text="Basic Operating"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_3_10" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_basic_op_weight" runat="server" Width="155px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_3_6" runat="server">
                                <asp:TableCell ID="TableCell_perf_3_11" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_12" runat="server" Text="Max. Landing"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_3_12" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_max_landing_weight" runat="server" Width="155px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <strong>
                              <asp:Label ID="Label_perf_title_4" runat="server" Text="IFR Certification"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_perf_4" runat="server" Width="90%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_perf_4_1" runat="server">
                                <asp:TableCell ID="TableCell_perf_4_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_13" runat="server" Text="(IFR)"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_4_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_ifr_certification" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <strong>
                              <asp:Label ID="Label_perf_title_5" runat="server" Text="CLIMB"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_perf_5" runat="server" Width="90%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_perf_5_1" runat="server">
                                <asp:TableCell ID="TableCell_perf_5_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_14" runat="server" Text="Normal"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_5_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_climb_normal_feet" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_5_2" runat="server">
                                <asp:TableCell ID="TableCell_perf_5_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_15" runat="server" Text="Engine Out"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_5_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_climb_engout_feet" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_5_3" runat="server">
                                <asp:TableCell ID="TableCell_perf_5_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_16" runat="server" Text="Ceiling"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_5_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_ceiling_feet" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_5_4" runat="server">
                                <asp:TableCell ID="TableCell_perf_5_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <font color="blue">^</font>
                                  <asp:Label ID="Label1" runat="server" Text="HOGE" ToolTip="Out of Ground Effect"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_5_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_climb_hoge" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_5_5" runat="server">
                                <asp:TableCell ID="TableCell_perf_5_9" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <font color="blue">^</font>
                                  <asp:Label ID="Label2" runat="server" Text="HIGE" ToolTip="In Ground Effect"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_5_10" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_climb_hige" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <strong>
                              <asp:Label ID="Label_perf_title_6" runat="server" Text="RANGE"></asp:Label>
                            </strong>
                            <asp:Table ID="TableTable_perf_6" runat="server" Width="90%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_perf_6_1" runat="server">
                                <asp:TableCell ID="TableCell_perf_6_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_17" runat="server" Text="Range"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_6_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_max_range_miles" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_6_2" runat="server">
                                <asp:TableCell ID="TableCell_perf_6_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_18" runat="server" Text="Tanks Full"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_6_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_range_tanks_full" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_6_3" runat="server">
                                <asp:TableCell ID="TableCell_perf_6_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_19" runat="server" Text="Seats Full"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_6_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_range_seats_full" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_6_4" runat="server">
                                <asp:TableCell ID="TableCell_perf_6_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_20" runat="server" Text="Range (4 PAX)"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_6_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_range_4_passenger" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_6_5" runat="server">
                                <asp:TableCell ID="TableCell_perf_6_9" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_21" runat="server" Text="Range (8 PAX)"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_6_10" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_range_8_passenger" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <strong>
                              <asp:Label ID="Label_perf_title_7" runat="server" Text="PROPELLERS"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_perf_7" runat="server" Width="90%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_perf_7_1" runat="server">
                                <asp:TableCell ID="TableCell_perf_7_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label4" runat="server" Text="Number of"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_7_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_number_of_props" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_7_2" runat="server">
                                <asp:TableCell ID="TableCell_perf_7_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label5" runat="server" Text="Model"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_7_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_prop_model_name" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_7_3" runat="server">
                                <asp:TableCell ID="TableCell_perf_7_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label6" runat="server" Text="Mfr. Name"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_7_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_prop_mfr_name" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_7_4" runat="server">
                                <asp:TableCell ID="TableCell_perf_7_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label9" runat="server" Text="COM TBO Hrs"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_7_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_prop_com_tbo_hrs" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <strong>
                              <asp:Label ID="Label_perf_title_8" runat="server" Text="CONFIG NOTE"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_perf_8" runat="server" Width="90%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_perf_8_1" runat="server">
                                <asp:TableCell ID="TableCell_perf_8_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server" ColumnSpan="2">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_other_config_note" runat="server" Rows="5" Columns="90" TextMode="MultiLine" placeholder=""></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                          </h2>
                        </td>
                        <td align="center" valign="top" style="text-align: center; padding-left: 0px;">
                          <h2>
                            <strong>
                              <asp:Label ID="Label_perf_right_title_1" runat="server" Text="CABIN DIMENSIONS"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_perf_right_1" runat="server" Width="70%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_perf_right_1_1" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_1_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_1" runat="server" Text="Length"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_1_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_cabinsize_length_feet" runat="server" Width="85px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>ft</em>
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_cabinsize_length_inches" runat="server" Width="85px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>in</em>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_1_2" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_1_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_2" runat="server" Text="Height"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_1_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_cabinsize_height_feet" runat="server" Width="85px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>ft</em>
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_cabinsize_height_inches" runat="server" Width="85px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>in</em>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_1_3" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_1_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_3" runat="server" Text="Width"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_1_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_cabinsize_width_feet" runat="server" Width="85px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>ft</em>
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_cabinsize_width_inches" runat="server" Width="85px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>in</em>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_1_4" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_1_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_4" runat="server" Text="Cabin Volume"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_1_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_cabin_volume" runat="server" Width="155px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_1_5" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_1_9" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_5" runat="server" Text="Baggage Volume"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_1_10" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_baggage_volume" runat="server" Width="155px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <strong>
                              <asp:Label ID="Label_perf_right_title_2" runat="server" Text="FUEL CAPACITY"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_perf_right_2" runat="server" Width="90%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_perf_right_2_1" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_2_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_6" runat="server" Text="Standard"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_2_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_fuel_cap_std_weight" runat="server" Width="125px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>lbs</em>
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_fuel_cap_std_gal" runat="server" Width="125px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>gal</em>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_2_2" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_2_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_7" runat="server" Text="Optional"></asp:Label>
                                  &nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_2_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_fuel_cap_opt_weight" runat="server" Width="125px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>lbs</em>
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_fuel_cap_opt_gal" runat="server" Width="125px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>gal</em>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <strong>
                              <asp:Label ID="Label_perf_right_title_3" runat="server" Text="SPEED"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_perf_right_3" runat="server" Width="90%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_perf_right_3_1" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_3_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <font color="maroon">*</font>
                                  <asp:Label ID="Label_perf_right_8" runat="server" Text="Vs Clean"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_3_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_stall_vs" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_3_2" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_3_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <font color="maroon">*</font>
                                  <asp:Label ID="Label_perf_right_9" runat="server" Text="Vso Landing"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_3_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_stall_vso" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_3_3" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_3_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_10" runat="server" Text="Normal Cruise TAS"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_3_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_cruis_speed" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_3_4" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_3_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_11" runat="server" Text="Vmo (Max Op) IAS"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_3_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_max_speed" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRowf_right_3_8" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_3_15" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <font color="blue">^</font>
                                  <asp:Label ID="Label3" runat="server" Text="Vne"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_3_16" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_vne_maxop_speed" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_3_5" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_3_9" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_12" runat="server" Text="V1 Takeoff"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_3_10" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_v1_takeoff_speed" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_3_6" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_3_11" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_13" runat="server" Text="VFE Max Flap Ext"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_3_12" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_vfe_max_flap_extended_speed" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_3_7" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_3_13" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_14" runat="server" Text="VLE Max Land Gear Ext"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_3_14" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_vle_max_landing_gear_ext_speed" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <strong>
                              <asp:Label ID="Label_perf_right_title_4" runat="server" Text="LANDING PERFORMANCE"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_perf_right_4" runat="server" Width="90%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_perf_right_4_1" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_4_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <font color="maroon">*</font>
                                  <asp:Label ID="Label_perf_right_15" runat="server" Text="FAA Field Length"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_4_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_field_length" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <strong>
                              <asp:Label ID="Label_perf_right_title_5" runat="server" Text="TAKEOFF PERFORMANCE"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_perf_right_5" runat="server" Width="90%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_perf_right_5_1" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_5_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_16" runat="server" Text="SL ISA BFL"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_5_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_takeoff_ali" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_5_2" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_5_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_17" runat="server" Text="5000' +20C BFL"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_5_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_takeoff_500" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <strong>
                              <asp:Label ID="Label_perf_right_title_6" runat="server" Text="ENGINES"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_perf_right_6" runat="server" Width="90%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_perf_right_6_1" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_6_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_18" runat="server" Text="Number of"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_6_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_number_of_engines" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_6_2" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_6_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_19" runat="server" Text="Model(s)"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_6_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:ListBox ID="ListBox_engines" runat="server" Width="155px">
                                    <asp:ListItem Text="eng1"></asp:ListItem>
                                    <asp:ListItem Text="eng2"></asp:ListItem>
                                  </asp:ListBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_6_2a" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_6_2a2" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Button ID="addModelEngine" runat="server" Text="Add Engine" CssClass="button-darker button_width" OnClientClick="javascript:ShowLoadingMessage('DivLoadingMessage', 'Adding Engine', 'Adding ... Please Wait ...');return true;" />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_6_2a3" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="engine_model_name" runat="server" Width="195px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_6_3" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_6_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_20" runat="server" Text="Thrust (per engine)"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_6_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_engine_thrust_lbs" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_6_4" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_6_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_21" runat="server" Text="Shaft (per engine)"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_6_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_engine_shaft" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_6_5" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_6_9" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_perf_right_22" runat="server" Text="Common TBO Hours"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_6_10" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_engine_com_tbo_hrs" runat="server" Width="155px" Height="20px" placeholder="" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <strong>
                              <font color="blue">^</font>
                              <asp:Label ID="Label_perf_right_title_7" runat="server" Text="ROTORS"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_perf_right_7" runat="server" Width="90%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_perf_right_7_1" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_7_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label11" runat="server" Text="Main Rotor Blades #1"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_7_2" VerticalAlign="Bottom" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_main_rotor_1_blade_count" runat="server" Width="75px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>num</em>&nbsp;&nbsp;
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_main_rotor_1_blade_diameter" runat="server" Width="75px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>dia</em>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_7_2" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_7_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label12" runat="server" Text="Main Rotor Blades #2"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_7_4" VerticalAlign="Bottom" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_main_rotor_2_blade_count" runat="server" Width="75px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>num</em>&nbsp;&nbsp;
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_main_rotor_2_blade_diameter" runat="server" Width="75px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>dia</em>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_7_3" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_7_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label13" runat="server" Text="Tail Rotor Blades"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_7_6" VerticalAlign="Bottom" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tail_rotor_blade_count" runat="server" Width="75px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>num</em>&nbsp;&nbsp;
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tail_rotor_blade_diameter" runat="server" Width="75px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox><em>dia</em>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_perf_right_7_4" runat="server">
                                <asp:TableCell ID="TableCell_perf_right_7_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label10" runat="server" Text="Anti Torque System"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_perf_right_7_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_rotor_anti_torque_system" runat="server" Width="175px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                          </h2>
                        </td>
                      </tr>
                    </table>
                  </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="tab2" runat="server" HeaderText="Operational Costs">
                  <HeaderTemplate>
                    Operational Costs
                  </HeaderTemplate>
                  <ContentTemplate>
                    <div style="text-align: right; padding-right: 8px;">
                      <asp:Button ID="saveModel2" runat="server" Text="Save Model" CssClass="button-darker button_width" OnClientClick="javascript:ShowLoadingMessage('DivLoadingMessage', 'Saving Model', 'Saving ... Please Wait ...');return true;" PostBackUrl="~/homebaseEditAircraftModel.aspx?task=save" />
                    </div>
                    <table id="Table_op_outer" border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
                      <tr>
                        <td align="center" valign="top" style="text-align: center; padding-left: 0px;" width="33%">
                          <h2>
                            <strong>
                              <asp:Label ID="Label7" runat="server" Text="DIRECT COSTS/HOUR"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_op_1" runat="server" Width="95%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_op_1_0" runat="server">
                                <asp:TableCell ID="TableCell_op_1_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <strong>
                                    <asp:Label ID="Label_op_0" runat="server" Text="FUEL"></asp:Label>&nbsp;: </strong>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_1_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_fuel_tot_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_1_1" runat="server">
                                <asp:TableCell ID="TableCell_op_1_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_1" runat="server" Text="Cost/Gallon"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_1_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_fuel_gal_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_1_2" runat="server">
                                <asp:TableCell ID="TableCell_op_1_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_2" runat="server" Text="Additive/Gallon"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_1_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_fuel_add_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_1_3" runat="server">
                                <asp:TableCell ID="TableCell_op_1_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_3" runat="server" Text="Burn/Hour"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_1_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_fuel_burn_rate" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="Table_op_2" runat="server" Width="95%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_op_2_0" runat="server">
                                <asp:TableCell ID="TableCell_op_2_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <strong>
                                    <asp:Label ID="Label_op_4" runat="server" Text="MAINTENANCE"></asp:Label>&nbsp;: </strong>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_2_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_maint_tot_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_2_1" runat="server">
                                <asp:TableCell ID="TableCell_op_2_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_5" runat="server" Text="Labor/Hour"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_2_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_maint_lab_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_2_2" runat="server">
                                <asp:TableCell ID="TableCell_op_2_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_6" runat="server" Text="Man Hour X"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_2_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_maint_labor_cost_man_hours_multiplier" runat="server" Width="115px" Height="20px" placeholder="" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_2_3" runat="server">
                                <asp:TableCell ID="TableCell_op_2_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_7" runat="server" Text="Parts/Hour"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_2_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_maint_parts_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_2_4" runat="server">
                                <asp:TableCell ID="TableCell_op_2_9" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_8" runat="server" Text="Man Hour X"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_2_10" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_maint_parts_cost_man_hours_multiplier" runat="server" Width="115px" Height="20px" placeholder="" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_2_5" runat="server">
                                <asp:TableCell ID="TableCell_op_2_11" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_op_9" runat="server" Text="Engine Overhaul"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_2_12" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_engine_ovh_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_2_6" runat="server">
                                <asp:TableCell ID="TableCell_op_2_13" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_op_10" runat="server" Text="Thrust Reverse OVH"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_2_14" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_thrust_rev_ovh_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="Table_op_3" runat="server" Width="95%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_op_3_0" runat="server">
                                <asp:TableCell ID="TableCell_op_3_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <strong>
                                    <asp:Label ID="Label_op_11" runat="server" Text="MISC. FLIGHT EXP."></asp:Label>&nbsp;: </strong>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_3_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_misc_flight_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_3_1" runat="server">
                                <asp:TableCell ID="TableCell_op_3_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_12" runat="server" Text="Land/Park Fee"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_3_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_land_park_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_3_2" runat="server">
                                <asp:TableCell ID="TableCell_op_3_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_13" runat="server" Text="Crew Expenses"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_3_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_crew_exp_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_3_3" runat="server">
                                <asp:TableCell ID="TableCell_op_3_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_14" runat="server" Text="Supplies/Catering"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_3_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_supplies_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_3_4" runat="server">
                                <asp:TableCell ID="TableCell_op_3_9" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <strong>
                                    <asp:Label ID="Label_op_15" runat="server" Text="TOTAL DIRECT COSTS"></asp:Label>&nbsp;: </strong>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_3_10" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tot_hour_direct_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_3_5" runat="server">
                                <asp:TableCell ID="TableCell_op_3_11" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_op_17" runat="server" Text="Block Speed" ToolTip="Statute MPH"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_3_12" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_avg_block_speed" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_3_6" runat="server">
                                <asp:TableCell ID="TableCell_op_3_13" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_op_18" runat="server" Text="Cost/Statute Mile"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_3_14" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tot_stat_mile_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                          </h2>
                        </td>
                        <td align="center" valign="top" style="text-align: center; padding-left: 0px;" width="33%">
                          <h2>
                            <strong>
                              <asp:Label ID="Label8" runat="server" Text="ANNUAL FIXED COSTS"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_op_4" runat="server" Width="95%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_op_4_0" runat="server">
                                <asp:TableCell ID="TableCell_op_4_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <strong>
                                    <asp:Label ID="Label_op_19" runat="server" Text="CREW SALARIES"></asp:Label>&nbsp;: </strong>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_4_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tot_crew_salary_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_4_1" runat="server">
                                <asp:TableCell ID="TableCell_op_4_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_20" runat="server" Text="Capt. Salary"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_4_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_capt_salary_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_4_2" runat="server">
                                <asp:TableCell ID="TableCell_op_4_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_21" runat="server" Text="Co-Pilot Salary"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_4_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_cpilot_salary_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_4_3" runat="server">
                                <asp:TableCell ID="TableCell_op_4_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_22" runat="server" Text="Benefits"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_4_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_crew_benefit_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_4_4" runat="server">
                                <asp:TableCell ID="TableCell_op_4_9" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_op_23" runat="server" Text="Hangar Cost"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_4_10" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_hangar_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="Table_op_5" runat="server" Width="95%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_op_5_0" runat="server">
                                <asp:TableCell ID="TableCell_op_5_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <strong>
                                    <asp:Label ID="Label_op_24" runat="server" Text="INSURANCE"></asp:Label>&nbsp;: </strong>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_5_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_insurance_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_5_1" runat="server">
                                <asp:TableCell ID="TableCell_op_5_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_25" runat="server" Text="Hull"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_5_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_hull_insurance_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_5_2" runat="server">
                                <asp:TableCell ID="TableCell_op_5_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_26" runat="server" Text="Libility"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_5_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_liability_insurance_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="Table_op_6" runat="server" Width="95%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_op_6_0" runat="server">
                                <asp:TableCell ID="TableCell_op_6_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <strong>
                                    <asp:Label ID="Label_op_27" runat="server" Text="MISC. OVERHEAD"></asp:Label>&nbsp;: </strong>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_6_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tot_misc_ovh_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_6_1" runat="server">
                                <asp:TableCell ID="TableCell_op_6_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_28" runat="server" Text="Training"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_6_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_misc_train_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_6_2" runat="server">
                                <asp:TableCell ID="TableCell_op_6_5" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_29" runat="server" Text="Modernization"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_6_6" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_misc_modern_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_6_3" runat="server">
                                <asp:TableCell ID="TableCell_op_6_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_30" runat="server" Text="Nav. Equip."></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_6_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_misc_naveq_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_6_4" runat="server">
                                <asp:TableCell ID="TableCell_op_6_9" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_31" runat="server" Text="Depreciation"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_6_10" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_deprec_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_6_5" runat="server">
                                <asp:TableCell ID="TableCell_op_6_11" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <strong>
                                    <asp:Label ID="Label_op_33" runat="server" Text="TOTAL FIXED COSTS"></asp:Label>&nbsp;: </strong>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_6_12" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tot_fixed_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_6_6" runat="server">
                                <asp:TableCell ID="TableCell_op_6_13" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <strong>
                                    <asp:Label ID="Label_op_34" runat="server" Text="TOTAL VARIABLE COSTS"></asp:Label>&nbsp;: </strong>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_6_14" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_variable_costs" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                          </h2>
                        </td>
                        <td align="center" valign="top" style="text-align: center; padding-left: 0px;">
                          <h2>
                            <strong>
                              <asp:Label ID="Label14" runat="server" Text="ANNUAL BUDGET"></asp:Label>
                            </strong>
                            <asp:Table ID="Table_op_7" runat="server" Width="95%" CellPadding="2" CellSpacing="0">
                              <asp:TableRow ID="TableRow_op_7_0" runat="server">
                                <asp:TableCell ID="TableCell_op_7_1" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_op_35" runat="server" Text="Passengers"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_7_2" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_number_of_seats" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_7_1" runat="server">
                                <asp:TableCell ID="TableCell_op_7_3" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <asp:Label ID="Label_op_36" runat="server" Text="Miles"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_7_4" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_annual_miles" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_7_2" runat="server">
                                <asp:TableCell ID="TableCell_op_7_5" VerticalAlign="Top" HorizontalAlign="Left" runat="server" Height="45px">
                                  <asp:Label ID="Label_op_37" runat="server" Text="Hours"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_7_6" VerticalAlign="Top" HorizontalAlign="Right" runat="server" Height="45px">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_annual_hours" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right">
                                  </asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_7_3" runat="server">
                                <asp:TableCell ID="TableCell_op_7_7" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <strong>
                                    <asp:Label ID="Label_op_38" runat="server" Text="TOTAL DIRECT COSTS"></asp:Label>&nbsp;: </strong>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_7_8" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tot_direct_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_7_4" runat="server">
                                <asp:TableCell ID="TableCell_op_7_9" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <strong>
                                    <asp:Label ID="Label_op_39" runat="server" Text="TOTAL FIXED COSTS"></asp:Label>&nbsp;: </strong>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_7_10" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tot_fixed_cost2" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_7_5" runat="server">
                                <asp:TableCell ID="TableCell_op_7_11" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <strong>
                                    <asp:Label ID="Label_op_40" runat="server">TOTAL COST<br/>(fixed and direct)</asp:Label>&nbsp;: </strong>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_7_12" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tot_df_annual_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_7_6" runat="server">
                                <asp:TableCell ID="TableCell_op_7_13" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_41" runat="server" Text="Cost/Hour"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_7_14" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tot_df_hour_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_7_7" runat="server">
                                <asp:TableCell ID="TableCell_op_7_15" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_42" runat="server" Text="Cost/Statute Mile"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_7_16" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tot_df_statmile_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_7_8" runat="server">
                                <asp:TableCell ID="TableCell_op_7_17" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_43" runat="server" Text="Cost/Seat Mile"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_7_18" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tot_df_seat_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_7_9" runat="server">
                                <asp:TableCell ID="TableCell_op_7_19" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  <strong>
                                    <asp:Label ID="Label_op_44" runat="server">TOTAL COST<br/>(no depreciation)</asp:Label>&nbsp;: </strong>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_7_20" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tot_nd_annual_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_7_10" runat="server">
                                <asp:TableCell ID="TableCell_op_7_21" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_45" runat="server" Text="Cost/Hour"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_7_22" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tot_nd_hour_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_7_11" runat="server">
                                <asp:TableCell ID="TableCell_op_7_23" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_46" runat="server" Text="Cost/Statute Mile"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_7_24" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tot_nd_statmile_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                              <asp:TableRow ID="TableRow_op_7_12" runat="server">
                                <asp:TableCell ID="TableCell_op_7_25" VerticalAlign="Middle" HorizontalAlign="Left" runat="server">
                                  &nbsp;&nbsp;<asp:Label ID="Label_op_47" runat="server" Text="Cost/Seat Mile"></asp:Label>&nbsp;:
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell_op_7_26" VerticalAlign="Middle" HorizontalAlign="Right" runat="server">
                                  <asp:TextBox CssClass="homebaseTextBoxFont" ID="amod_tot_nd_seat_cost" runat="server" Width="115px" Height="20px" placeholder="0" Style="text-align: right" Enabled="false" BackColor="LightGray"></asp:TextBox>
                                </asp:TableCell>
                              </asp:TableRow>
                            </asp:Table>
                          </h2>
                        </td>
                      </tr>
                    </table>
                  </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="tab3" runat="server" HeaderText="Attributes">
                  <HeaderTemplate>
                    Attributes
                  </HeaderTemplate>
                  <ContentTemplate>
                    <div style="text-align: right; padding-right: 8px;">
                      <asp:Button ID="editAttributesBtn" runat="server" Text="Edit Attributes" CssClass="button-darker button_width" UseSubmitBehavior="false" Visible="false" />
                    </div>
                    <table id="Table1" border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
                      <tr>
                        <td align="center" valign="top" style="text-align: center; padding-left: 0px;">
                          <asp:Label ID="attributesLabel" runat="server" Text=""></asp:Label>
                        </td>
                      </tr>
                    </table>
                  </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="tab4" runat="server" HeaderText="Features">
                  <HeaderTemplate>
                    Features
                  </HeaderTemplate>
                  <ContentTemplate>

                    <table id="Table3" border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
                      <tr>
                        <td align="center" valign="top" style="text-align: center; padding-left: 0px;">
                          <asp:Label ID="featuresLabel" runat="server" Text=""></asp:Label>
                        </td>
                      </tr>
                    </table>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
            </div>
          </td>
        </tr>
      </table>
    </div>
    <div id="DivLoadingMessage" style="display: none;">
    </div>
  </asp:Panel>

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

  </script>

</asp:Content>
