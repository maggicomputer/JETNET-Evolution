<?xml version="1.0"?>

<!--
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/tempFiles/OperatingCostsHTMLstandard.xslt $
'$$Author: Mike $
'$$Date: 6/19/19 8:55a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: OperatingCostsHTMLstandard.xslt $
'
' ********************************************************************************
-->
<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:user="urn:my-scripts"
	xmlns:o="urn:schemas-microsoft-com:office:office"
	xmlns:x="urn:schemas-microsoft-com:office:excel">

  <xsl:template match="/">

    <html xmlns:o="urn:schemas-microsoft-com:office:office"
      xmlns:x="urn:schemas-microsoft-com:office:excel"
      xmlns="http://www.w3.org/1999/xhtml">

      <head>
        <meta http-equiv="Content-Language" content="en-us" />
        <meta http-equiv="Content-Type" content="text/html; charset=windows-1252" />
        <meta name="apple-mobile-web-app-capable" content="yes" />
        <meta name="format-detection" content="telephone=no" />
        <link type="text/css" rel="stylesheet" href="OperatingCostsHTML.css" />
        <title>JETNET - Export of Current Operating Costs List (US Standard)</title>
      </head>

      <body link="blue" vlink="purple" style="margin-top: 10px; margin-left: 10px;">
        <table border="0" cellpadding="0" cellspacing="0" width="367" style='border-collapse:collapse;table-layout:fixed;width:275pt'>

          <col width='207' style='mso-width-source:userset;mso-width-alt:8630;width:207pt' />

          <xsl:for-each select="NewDataSet/opCosts">
            <col width='171' style='mso-width-source:userset;mso-width-alt:4790;width:171pt' />
          </xsl:for-each>

          <tr height='17' valign='bottom' style='height:12.75pt'>
            <td height='17' width='171' style='height:12.75pt;width:151pt'></td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td width='171' style='width:171pt'>&#160;</td>
            </xsl:for-each>
          </tr>
          <tr height='17' valign='bottom' style='height:12.75pt'>
            <td height='17' class='xl77' bgcolor="#C0C0C0" align="center" valign="middle"
            style='height:12.75pt'>
              <font face="Arial">
                <b>DIRECT COSTS PER HOUR (US Standard)</b>
              </font>
            </td>
            <td class="xl77" bgcolor="#C0C0C0">
              <xsl:attribute name="colspan" >
                <xsl:value-of select="count(NewDataSet/opCosts)"/>
              </xsl:attribute>
              <font face="Arial">
                <b>
                  <xsl:value-of select="NewDataSet/opCosts//exchangeRateDate"/>
                </b>
              </font>
            </td>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl79' style='height:12.75pt;border-top:none'>
              <font face="Arial">&#160;</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl76" style='border-left:none' nowrap='nowrap'>
                <font face="Arial" color="#0000FF">
                  <b>
                    <xsl:value-of select="modelName"/>
                  </b>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl73' style='height:12.75pt'>
              <font face="Arial">
                <u>Fuel</u>
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="fuelTotalCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Fuel
                Cost Per Gallon
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="fuelGalCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Additive
                Cost Per Gallon
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="fuelAddCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Burn
                Rate (Gallons Per Hour)
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="fuelBurnRate"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl73' style='height:12.75pt'>
              <font face="Arial">
                <u>Maintenance</u>
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="maintTotalCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Labor
                Cost Per Hour
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="maintLaborCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Parts
                Cost Per Hour
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="maintPartsCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Labor
                Cost Per Man Hour
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="maintLaborCostManHour"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Parts
                Cost Per Man Hour
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="maintPartsCostManHour"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                Engine
                Overhaul
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="maintEngineCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                Thrust
                Reverse Overhaul
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="maintThrustCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl73' style='height:12.75pt'>
              <font face="Arial">
                <u>
                  Miscellaneous
                  Flight Expenses
                </u>
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="miscFlightTotalCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Landing-Parking
                Fee
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="miscLandParkCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Crew
                Expenses
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="miscCrewCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">&#160;&#160;Supplies-Catering</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="miscSupplyCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl74' style='height:12.75pt'>
              <font face="Arial">
                <b>
                  Total
                  Direct Costs
                </b>
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="totalDirCostHour"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">&#160;</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl66" style='border-top:none;border-left:none'>
                <font face="Arial">&#160;</font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                Block
                Speed Statute Miles Per Hour
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl70" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="avgBlockSpeed"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl75' style='height:12.75pt'>
              <font face="Arial">
                Total Cost
                Per Statute Mile
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="totalCostPerMile"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl77' bgcolor="#C0C0C0" align="center" valign="middle"
            style='height:12.75pt'>
              <font face="Arial">
                <b>ANNUAL FIXED COSTS (US Standard)</b>
              </font>
            </td>
            <td class="xl77" bgcolor="#C0C0C0">
              <xsl:attribute name="colspan" >
                <xsl:value-of select="count(NewDataSet/opCosts)"/>
              </xsl:attribute>
              <font face="Arial">
                <b>
                  <xsl:value-of select="NewDataSet/opCosts//exchangeRateDate"/>
                </b>
              </font>
            </td>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl79' style='height:12.75pt;border-top:none'>
              <font face="Arial">&#160;</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl76" style='border-left:none'>
                <font face="Arial" color="#0000FF">
                  <b>
                    <xsl:value-of select="modelName"/>
                  </b>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl73' style='height:12.75pt'>
              <font face="Arial">
                <u>
                  Crew
                  Salaries
                </u>
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl83" align="right" style='border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="crewTotalCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Capt.
                Salary
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="captSalaryCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Co-pilot
                Salary
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="coPilotSalaryCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">&#160;&#160;Benefits</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="benefitsCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                Hangar
                Cost
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="hangarCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl73' style='height:12.75pt'>
              <font face="Arial">
                <u>Insurance</u>
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="insuranceTotalCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">&#160;&#160;Hull</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="insuranceHullCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Legal
                Liability
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="insuranceLiabilityCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl73' style='height:12.75pt'>
              <font face="Arial">
                <u>
                  Misc.
                  Overhead
                </u>
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="miscTotalCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">&#160;&#160;Training</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="miscTrainCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">&#160;&#160;Modernization</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="miscModernCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Nav.
                Equipment
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="miscNavCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">Depreciation</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="depreciationCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl80' style='height:12.75pt'>
              <font face="Arial">
                <b>
                  Total
                  Fixed Costs
                </b>
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="totalFixedCosts"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl78' bgcolor="#C0C0C0" align="center" valign="middle"
            style='height:12.75pt;border-top:none'>
              <font face="Arial">
                <b>
                  ANNUAL BUDGET
                  (US Standard)
                </b>
              </font>
            </td>
            <td class="xl77" bgcolor="#C0C0C0">
              <xsl:attribute name="colspan" >
                <xsl:value-of select="count(NewDataSet/opCosts)"/>
              </xsl:attribute>
              <font face="Arial">
                <b>
                  <xsl:value-of select="NewDataSet/opCosts//exchangeRateDate"/>
                </b>
              </font>
            </td>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl79' style='height:12.75pt;border-top:none'>
              <font face="Arial">&#160;</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl65" style='border-left:none'>
                <font face="Arial" color="#0000FF">
                  <b>
                    <xsl:value-of select="modelName"/>
                  </b>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                Number of
                Seats
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl69" align="right" style='border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="numberOfSeats"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">Miles</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl67" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="annualMiles"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">Hours</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl68" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="annualHrs"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">&#160;</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl66" style='border-top:none;border-left:none'>
                <font face="Arial">&#160;</font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl74' style='height:12.75pt'>
              <font face="Arial">
                <b>
                  Total
                  Direct Costs
                </b>
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="totalDirCostYR"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl74' style='height:12.75pt'>
              <font face="Arial">
                <b>
                  Total
                  Fixed Costs
                </b>
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="totalFixedCosts"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
            <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl74' style='height:12.75pt'>
              <font face="Arial">
                <b>
                  Total
                  Variable Costs
                </b>
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl81" align="right" style='border-top:none;border-left:none'>
                <font face="Arial">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="variableTotalCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">&#160;</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl66" style='border-top:none;border-left:none'>
                <font face="Arial">&#160;</font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl73' style='height:12.75pt'>
              <font face="Arial">
                <u>
                  Total
                  Cost (Fixed &amp; Direct)
                </u>
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="totalFixedDirect"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">&#160;&#160;Cost/Hour</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="costPerHourFixDir"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Cost/Statute
                Mile
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="costPerMileFixDir"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Cost/Seat
                Mile
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="costPerSeatFixDir"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">&#160;</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl66" style='border-top:none;border-left:none'>
                <font face="Arial">&#160;</font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl73' style='height:12.75pt'>
              <font face="Arial">
                <u>
                  Total
                  Cost (No Depreciation)
                </u>
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="noDepTotalCost"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">&#160;&#160;Cost/Hour</font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="costPerHourNoDep"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl72' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Cost/Statute
                Mile
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="costPerMileNoDep"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="17" valign="bottom" style='height:12.75pt'>
            <td height='17' class='xl75' style='height:12.75pt'>
              <font face="Arial">
                &#160;&#160;Cost/Seat
                Mile
              </font>
            </td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td class="xl82" align="right" style='border-top:none;border-left:none'>
                <font face="Arial" color="#FF0000">
                  <xsl:value-of select="currencySymbol"/><xsl:value-of select="costPerSeatNoDep"/>
                </font>
              </td>
            </xsl:for-each>
          </tr>
          <tr height="0" style='display:none'>
            <td width='236' style='width:177pt'></td>
            <xsl:for-each select="NewDataSet/opCosts">
              <td width='131' style='width:98pt'></td>
            </xsl:for-each>
          </tr>
        </table>

      </body>
    </html>
  </xsl:template>

</xsl:stylesheet>
