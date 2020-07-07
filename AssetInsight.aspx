<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master"
  CodeBehind="AssetInsight.aspx.vb" Inherits="crmWebClient.AssetInsight" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script language="javascript" type="text/javascript" src="https://www.google.com/jsapi?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE"></script>
<script type="text/javascript">
  google.load('visualization', '1', { packages: ['corechart'] });
</script>

  <style>
    .removeLeftMargin
    { 
      margin-left: 0px !important;
    }
    .aircraftContainer .sixteen.columns .six.columns .Box.gauge
    {
      height: 200px;
      margin-left: 0px;
      margin-right: 0px;
    }
    .gauge .subHeader
    {
      padding-bottom: 15px !important;
    }
    .aircraftContainer .formatTable.large, .aircraftContainer .formatTable.large td span
    {
      font-size: 14px !important;
    }
    .logo_text_title{text-transform:none !important;}
    .aircraftContainer .formatTable.large td span.smallMaintenanceLabel
    {
      font-size: 10px !important;
      display: block;
      padding-top: 5px;
      margin-bottom: -9px;
    }
    .aircraftContainer .gaugeBars .six.columns{width:49%;} 
    .aircraftContainer .gaugeBars .columns{margin-left:2%;}
    .green_text {color: #2ce427 !important; padding:0px 0px 0px 0px;display:inline;}
    .excellent_text{color:#62cc14 !important;}
    .verygood_text{color:#cad310 !important;}
    .good_text{color:#e9c43b !important;}
    .average_text{color:#e88d23 !important;}
  </style>


  <script type="text/javascript" src="https://cdn.rawgit.com/Mikhus/canvas-gauges/gh-pages/download/2.1.4/all/gauge.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="DetailsBrowseTable"><span class="backgroundShade"><a href="#" class="gray_button float_right noBefore" onclick="javascript:window.close();"><img src="/images/x.svg" alt="Close" /></a></span><div class="clear"></div></div>
  <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="">
    <ProgressTemplate>
      <div id="divLoading" runat="server" style="text-align: center; font-weight: bold;
        background-color: #eeeeee; filter: alpha(opacity=90); opacity: 0.9; width: 395px;
        height: 295px; text-align: center; padding: 75px; position: absolute; border: 1px solid #003957;
        z-index: 10; margin-left: 225px;">
        <span>Please wait ... </span>
        <br />
        <br />
        <img src="/images/loading.gif" alt="Loading..." /><br />
      </div>
    </ProgressTemplate>
  </asp:UpdateProgress>
  <div class="aircraftContainer">
    <div class="valueSpec viewValueExport Simplistic aircraftSpec">
      <div class="row">
        <div class="sixteen columns">
          <asp:UpdatePanel ID="asset_insight_outer_panel" runat="server" ChildrenAsTriggers="True"
            UpdateMode="Conditional">
            <ContentTemplate>
              <strong>
                <asp:Label ID="AIAClbl" runat="server" Text=""></asp:Label></strong>
              <asp:Table ID="menuTable" CellPadding="4" CellSpacing="0" Width="100%" runat="server">
                <asp:TableRow>
                  <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="left" VerticalAlign="top"
                    Width="40%">
                    <asp:Label ID="ac_block" runat="server" Text=""></asp:Label>
                    <asp:Label ID="ac_status" runat="server"></asp:Label>
                    <asp:Label ID="evalue_label" runat="server" Text=""></asp:Label>
                    <asp:UpdatePanel runat="server" ID="graphUpdateResidual">
                      <ContentTemplate>
                        <asp:Label ID="residual_label" runat="server" Visible="false">
                        </asp:Label>
                      </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Label ID="afttGauge" runat="server" CssClass="display_none">
                   <div class="Box removeLeftMargin" style="height:180px;">
                    <table cellpadding="0" cellspacing="0" class="formatTable blue large" width="100%">
                     <tr class="noBorder"><td align="left" valign="top"><span class="subHeader">AFTT vs FLEET</span></td></tr>
                       <tr><td colspan="2" align="center"><canvas id="afttCount"></canvas></td></tr>
                       </table>
                      </div>
                    </asp:Label>
                    <asp:Label ID="AirframeEnginesLbl" runat="server" Text=""></asp:Label>
                    <asp:Label ID="maint_coverage" runat="server" Text=""></asp:Label>
                       <asp:Label ID="sales_comparables_label" runat="server" Text="" CssClass="display_none"></asp:Label>
                    <asp:Label ID="recent_sales" runat="server" Text="" CssClass="display_none"></asp:Label>
                    <asp:Label ID="factors" runat="server" Text="" CssClass="display_none"></asp:Label>
                  </asp:TableCell>
                  <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="left" VerticalAlign="top"
                    Width="60%">
                    <div class="row remove_margin">
                      <div class="twelve columns">
                        <div class="row remove_margin">
                          <div class="columns twelve">
                            <div class="Box">
                              <table class="formatTable large blue" cellpadding="0" cellspacing="0" width="100%">
                                <tr class="noBorder">
                                  <td align="left" valign="top">
                                    <span class="subHeader">Evalue Overview</span>
                                  </td>
                                </tr>
                                <tr>
                                  <td align="left" valign="top">
                                    <br />
                                    <asp:Label runat="server" ID="aircraftYearCompareText"></asp:Label>
                                    <asp:Label runat="server" ID="aircraftAFTTCompareText"></asp:Label>
                                    <asp:Label runat="server" ID="aircraftSalesText"></asp:Label>
                                    <asp:Label runat="server" ID="aircraftDeliveryText"></asp:Label>
                                    <asp:Label runat="server" ID="aircraftAirframeMaintenanceText"></asp:Label>
                                    <asp:Label runat="server" ID="aircraftEngineMaintenanceText"></asp:Label>
                                    <asp:Label runat="server" ID="aircraftATCText"></asp:Label>
                                    <asp:Label runat="server" ID="aircraftAFTCText"></asp:Label>
                                    <asp:Label runat="server" ID="aircraftQualityText"></asp:Label>
                                  </td>
                                </tr>
                              </table>
                            </div>
                          </div>
                        </div>
                        <asp:Label ID="evalueGauges" runat="server" CssClass="display_none">
                        <span class="gaugeBars">
                          <div class="six columns removeLeftMargin">
                            <div class="Box gauge">
                              <table cellpadding="0" cellspacing="0" class="formatTable blue large" width="100%">
                                <tr class="noBorder">
                                  <td align="left" valign="top">
                                    <span class="subHeader">TECHNICAL CONDITION RELATIVE TO MAINTENANCE</span>
                                  </td>
                                </tr>
                                <tr>
                                  <td colspan="2" align="center">
                                    <canvas id="atcCount"></canvas>
                                  </td>
                                </tr>
                              </table>
                            </div>
                          </div>
                          <div class="six columns ">
                            <div class="Box gauge">
                              <table cellpadding="0" cellspacing="0" class="formatTable blue large" width="100%">
                                <tr class="noBorder">
                                  <td align="left" valign="top">
                                    <span class="subHeader">TECHNICAL CONDITION RELATIVE TO COST</span>
                                  </td>
                                </tr>
                                <tr>
                                  <td colspan="2" align="center">
                                    <canvas id="aftcCount"></canvas>
                                  </td>
                                </tr>
                              </table>
                            </div>
                          </div>
                          <div class="six columns removeLeftMargin">
                            <div class="Box gauge">
                              <table cellpadding="0" cellspacing="0" class="formatTable blue large" width="100%">
                                <tr class="noBorder">
                                  <td align="left" valign="top">
                                    <span class="subHeader">OVERALL QUALITY RATING</span>
                                  </td>
                                </tr>
                                <tr>
                                  <td colspan="2" align="center">
                                    <canvas id="qualityCount"></canvas>
                                  </td>
                                </tr>
                              </table>
                            </div>
                          </div>
                          <div class="six columns">
                            <div class="Box removeLeftMargin">
                              <table cellpadding="0" cellspacing="0" class="formatTable blue large" width="100%">
                                <tr class="noBorder">
                                  <td align="left" valign="top">
                                    <span class="subHeader">MAINTENANCE EXPOSURE</span>
                                  </td>
                                </tr>
                                <tr>
                                  <td align="left" valign="top">
                                    <asp:Label runat="server" ID="maintenance_exposure_label"></asp:Label><div class="clearfix">
                                    </div>
                                    <span class="smallMaintenanceLabel">Accrued cost of future scheduled maintenance.</span>
                                  </td>
                                </tr>
                              </table>
                            </div>
                            <div class="Box removeLeftMargin">
                              <table cellpadding="0" cellspacing="0" class="formatTable blue large" width="100%">
                                <tr class="noBorder">
                                  <td align="left" valign="top">
                                    <span class="subHeader">MODEL COMPARABLES</span>
                                  </td>
                                </tr>
                                <tr>
                                  <td align="left" valign="top">
                                    <asp:Label runat="server" ID="model_comparables_label"></asp:Label>
                                  </td>
                                </tr>
                              </table>
                            </div>
                            <div class="Box removeLeftMargin">
                              <table cellpadding="0" cellspacing="0" class="formatTable blue large" width="100%">
                                <tr class="noBorder">
                                  <td align="left" valign="top">
                                    <span class="subHeader">Analysis ID</span>
                                  </td>
                                </tr>
                                <tr>
                                  <td align="left" valign="top">
                                    <asp:Label runat="server" ID="analysis_id_label"></asp:Label>
                                  </td>
                                </tr>
                              </table>
                            </div>
                          </div>
                          <div class="twelve columns removeLeftMargin">
                          <div class="Box">
                          <div class="subHeader">ASSET INSIGHT GRADING</div>
                          <img src="images/legendValueChart.jpg" style="padding:8px;width:583px;margin-top:11px;" />
                          </div>
                          </div>
                          </span>
                        </asp:Label>
                      </div>
                    </div>
                    <div class="row remove_margin gaugeBars">
                      <div class="six columns removeLeftMargin" runat="server" id="inspectionClass">
                        <asp:Label ID="InspectionsLbl" runat="server" Text=""></asp:Label>
                      </div>
                      <div class="six columns" runat="server" id="modBox">
                        <asp:Label ID="ModificationsLbl" runat="server" Text=""></asp:Label>
                      </div>
                    </div>
                  </asp:TableCell>
                </asp:TableRow>
              </asp:Table>
              <asp:CheckBox ID="passCheckboxForAsking" Checked="true" runat="server" CssClass="display_none" />
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">
</asp:Content>
