<?xml version="1.0"?>

<!--
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/tempFiles/OperatingCostsEXCELstandard.xslt $
'$$Author: Mike $
'$$Date: 6/19/19 8:55a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: OperatingCostsEXCELstandard.xslt $
'
' ********************************************************************************
-->
<xsl:stylesheet version="1.0"
  xmlns="urn:schemas-microsoft-com:office:spreadsheet"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:user="urn:my-scripts"
	xmlns:o="urn:schemas-microsoft-com:office:office"
	xmlns:x="urn:schemas-microsoft-com:office:excel"
	xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">

  <xsl:template match="/">

    <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
     xmlns:o="urn:schemas-microsoft-com:office:office"
     xmlns:x="urn:schemas-microsoft-com:office:excel"
     xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"
     xmlns:html="http://www.w3.org/TR/REC-html40">

      <DocumentProperties xmlns="urn:schemas-microsoft-com:office:office">
        <Author>Mike</Author>
        <LastAuthor>Mike</LastAuthor>
        <Created>2005-04-13T18:27:04Z</Created>
        <LastSaved>2010-10-04T20:04:24Z</LastSaved>
        <Company>Jetnet</Company>
      </DocumentProperties>

      <OfficeDocumentSettings xmlns="urn:schemas-microsoft-com:office:office">
        <DoNotRelyOnCSS/>
        <DoNotOrganizeInFolder/>
      </OfficeDocumentSettings>

      <ExcelWorkbook xmlns="urn:schemas-microsoft-com:office:excel">
        <HideWorkbookTabs/>
        <WindowHeight>11640</WindowHeight>
        <WindowWidth>11985</WindowWidth>
        <WindowTopX>900</WindowTopX>
        <WindowTopY>180</WindowTopY>
        <DisplayDrawingObjects>HideAll</DisplayDrawingObjects>
        <DoNotSaveLinkValues/>
        <ProtectStructure>False</ProtectStructure>
        <ProtectWindows>False</ProtectWindows>
      </ExcelWorkbook>

      <Styles>
        <Style ss:ID="Default" ss:Name="Normal">
          <Alignment ss:Vertical="Bottom"/>
          <Borders/>
          <Font ss:FontName="Arial"/>
          <Interior/>
          <NumberFormat/>
          <Protection/>
        </Style>
        <Style ss:ID="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9" ss:Color="#0000FF"
           ss:Bold="1"/>
          <NumberFormat ss:Format="@"/>
        </Style>
        <Style ss:ID="s63">
          <Alignment ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9"/>
          <NumberFormat ss:Format="@"/>
        </Style>
        <Style ss:ID="s64">
          <Alignment ss:Horizontal="Right" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9"/>
          <NumberFormat ss:Format="#,##0"/>
        </Style>
        <Style ss:ID="s65">
          <Alignment ss:Horizontal="Right" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9" ss:Color="#FF0000"/>
          <NumberFormat ss:Format="#,##0"/>
        </Style>
        <Style ss:ID="s66">
          <Alignment ss:Vertical="Bottom"/>
        </Style>
        <Style ss:ID="s67">
          <Alignment ss:Horizontal="Right" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9"/>
          <NumberFormat ss:Format="#,##0"/>
        </Style>
        <Style ss:ID="s68">
          <Alignment ss:Horizontal="Right" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9"/>
          <NumberFormat ss:Format="#,##0"/>
        </Style>
        <Style ss:ID="s69">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
          <Borders/>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9" ss:Bold="1"/>
          <Interior ss:Color="#C0C0C0" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s70">
          <Alignment ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9"/>
        </Style>
        <Style ss:ID="s71">
          <Alignment ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9" ss:Underline="Single"/>
        </Style>
        <Style ss:ID="s72">
          <Alignment ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9" ss:Bold="1"/>
        </Style>
        <Style ss:ID="s73">
          <Alignment ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9"/>
        </Style>
        <Style ss:ID="s74">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9" ss:Color="#0000FF"
           ss:Bold="1"/>
          <NumberFormat ss:Format="@"/>
        </Style>
        <Style ss:ID="s75">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9" ss:Bold="1"/>
          <Interior ss:Color="#C0C0C0" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s76">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9" ss:Bold="1"/>
          <Interior ss:Color="#C0C0C0" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s77">
          <Alignment ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9"/>
        </Style>
        <Style ss:ID="s78">
          <Alignment ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9" ss:Bold="1"/>
        </Style>
        <Style ss:ID="s79">
          <Alignment ss:Horizontal="Right" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9"/>
          <NumberFormat ss:Format="Standard"/>
        </Style>
        <Style ss:ID="s80">
          <Alignment ss:Horizontal="Right" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9" ss:Color="#FF0000"/>
          <NumberFormat ss:Format="Standard"/>
        </Style>
        <Style ss:ID="s81">
          <Alignment ss:Horizontal="Right" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="9" ss:Color="#FF0000"/>
          <NumberFormat ss:Format="#,##0"/>
        </Style>
      </Styles>

      <Worksheet ss:Name="CostOfOps">

        <Table ss:ExpandedRowCount="57" x:FullColumns="1" x:FullRows="1" ss:StyleID="s66">

          <xsl:attribute name="ss:ExpandedColumnCount" >
            <xsl:value-of select="count(NewDataSet/opCosts)+1"/>
          </xsl:attribute>

          <Column ss:StyleID="s66" ss:AutoFitWidth="0" ss:Width="186.75"/>

          <xsl:for-each select="NewDataSet/opCosts">
            <Column ss:StyleID="s66" ss:AutoFitWidth="0" ss:Width="160.25"/>
          </xsl:for-each>

          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s66"/>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s66"/>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s75">
              <Data ss:Type="String">DIRECT COSTS PER HOUR (US Standard)</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s75">
                <Data ss:Type="String">
                  <xsl:value-of select="exchangeRateDate"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s77"/>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s74">
                <Data ss:Type="String">
                  <xsl:value-of select="modelName"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s71">
              <Data ss:Type="String">Fuel</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s80" ss:Formula="=ROUND((R[1]C+R[2]C)*R[3]C,2)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Fuel Cost Per Gallon</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s79">
                <Data ss:Type="Number">
                  <xsl:value-of select="fuelGalCost"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Additive Cost Per Gallon</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s79">
                <Data ss:Type="Number">
                  <xsl:value-of select="fuelAddCost"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Burn Rate (Gallons Per Hour)</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s79">
                <Data ss:Type="Number">
                  <xsl:value-of select="fuelBurnRate"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s71">
              <Data ss:Type="String">Maintenance</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s80" ss:Formula="=ROUND((R[1]C+R[2]C),2)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Labor Cost Per Hour</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s79">
                <Data ss:Type="Number">
                  <xsl:value-of select="maintLaborCost"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Parts Cost Per Hour</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s79">
                <Data ss:Type="Number">
                  <xsl:value-of select="maintPartsCost"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
            <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Labor Cost Per Man Hour</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s79">
                <Data ss:Type="Number">
                  <xsl:value-of select="maintLaborCostManHour"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Parts Cost Per Man Hour</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s79">
                <Data ss:Type="Number">
                  <xsl:value-of select="maintPartsCostManHour"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">Engine Overhaul</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s79">
                <Data ss:Type="Number">
                  <xsl:value-of select="maintEngineCost"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">Thrust Reverse Overhaul</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s79">
                <Data ss:Type="Number">
                  <xsl:value-of select="maintThrustCost"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s71">
              <Data ss:Type="String">Miscellaneous Flight Expenses</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s80" ss:Formula="=ROUND((R[1]C+R[2]C+R[3]C),2)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Landing-Parking Fee</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s79">
                <Data ss:Type="Number">
                  <xsl:value-of select="miscLandParkCost"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Crew Expenses</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s79">
                <Data ss:Type="Number">
                  <xsl:value-of select="miscCrewCost"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Supplies-Catering</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s79">
                <Data ss:Type="Number">
                  <xsl:value-of select="miscSupplyCost"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s72">
              <Data ss:Type="String">Total Direct Costs</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s80" ss:Formula="=ROUND((R[-15]C+R[-11]C+R[-6]C+R[-5]C+R[-4]C),2)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70"/>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s63"/>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">Block Speed Statute Miles Per Hour</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s68">
                <Data ss:Type="Number">
                  <xsl:value-of select="avgBlockSpeed"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s73">
              <Data ss:Type="String">Total Cost Per Statute Mile</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s80" ss:Formula="=ROUND(IF(R[-3]C=0,0,R[-3]C/R[-1]C),2)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s75">
              <Data ss:Type="String">ANNUAL FIXED COSTS (US Standard)</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s75">
                <Data ss:Type="String">
                  <xsl:value-of select="exchangeRateDate"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s77"/>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s74">
                <Data ss:Type="String">
                  <xsl:value-of select="modelName"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s71">
              <Data ss:Type="String">Crew Salaries</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s81" ss:Formula="=ROUND((R[1]C+R[2]C+R[3]C),0)">
                <Data ss:Type="Number">0</Data>
              </Cell>

            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Capt. Salary</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s64">
                <Data ss:Type="Number">
                  <xsl:value-of select="captSalaryCostRaw"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Co-pilot Salary</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s64">
                <Data ss:Type="Number">
                  <xsl:value-of select="coPilotSalaryCostRaw"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Benefits</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s64">
                <Data ss:Type="Number">
                  <xsl:value-of select="benefitsCostRaw"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">Hangar Cost</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s64">
                <Data ss:Type="Number">
                  <xsl:value-of select="hangarCost"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s71">
              <Data ss:Type="String">Insurance</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s65" ss:Formula="=ROUND((R[1]C+R[2]C),0)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Hull</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s64">
                <Data ss:Type="Number">
                  <xsl:value-of select="insuranceHullCostRaw"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Legal Liability</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s64">
                <Data ss:Type="Number">
                  <xsl:value-of select="insuranceLiabilityCostRaw"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s71">
              <Data ss:Type="String">Misc. Overhead</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s65" ss:Formula="=ROUND((R[1]C+R[2]C+R[3]C),0)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Training</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s64">
                <Data ss:Type="Number">
                  <xsl:value-of select="miscTrainCostRaw"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Modernization</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s64">
                <Data ss:Type="Number">
                  <xsl:value-of select="miscModernCostRaw"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Nav. Equipment</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s64">
                <Data ss:Type="Number">
                  <xsl:value-of select="miscNavCostRaw"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">Depreciation</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s64">
                <Data ss:Type="Number">
                  <xsl:value-of select="depreciationCostRaw"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s78">
              <Data ss:Type="String">Total Fixed Costs</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s65" ss:Formula="=ROUND((R[-13]C+R[-9]C+R[-8]C+R[-5]C+R[-1]C),0)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s76">
              <Data ss:Type="String">ANNUAL BUDGET (US Standard)</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s75">
                <Data ss:Type="String">
                  <xsl:value-of select="exchangeRateDate"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s77"/>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s62">
                <Data ss:Type="String">
                  <xsl:value-of select="modelName"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">Number of Seats</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s67">
                <Data ss:Type="Number">
                  <xsl:value-of select="numberOfSeats"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">Miles</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s64">
                <Data ss:Type="Number">
                  <xsl:value-of select="annualMiles"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">Hours</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s65" ss:Formula="=ROUND(IF(R[-1]C=0,0,R[-1]C/R[-22]C),0)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70"/>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s63"/>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s72">
              <Data ss:Type="String">Total Direct Costs</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s65" ss:Formula="=TRUNC((R[-26]C*R[-2]C),0)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s72">
              <Data ss:Type="String">Total Fixed Costs</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s65" ss:Formula="=R[-8]C">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s72">
              <Data ss:Type="String">Total Variable Costs</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s65">
                <Data ss:Type="Number">
                  <xsl:value-of select="variableTotalCost"/>
                </Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70"/>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s63"/>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s71">
              <Data ss:Type="String">Total Cost (Fixed &amp; Direct)</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s65" ss:Formula="=TRUNC((R[-4]C+R[-3]C),2)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Cost/Hour</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s65" ss:Formula="=ROUND(IF(R[-1]C=0,0,R[-1]C/R[-7]C),2)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Cost/Statute Mile</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s80" ss:Formula="=ROUND(IF(R[-2]C=0,0,R[-2]C/R[-9]C),2)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Cost/Seat Mile</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s80" ss:Formula="=ROUND(IF(R[-1]C=0,0,R[-1]C/R[-11]C),2)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70"/>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s63"/>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s71">
              <Data ss:Type="String">Total Cost (No Depreciation)</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s65" ss:Formula="=ROUND((R[-5]C-R[-17]C),2)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Cost/Hour</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s65" ss:Formula="=ROUND(IF(R[-1]C=0,0,R[-1]C/R[-12]C),2)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s70">
              <Data ss:Type="String">&#160;&#160;Cost/Statute Mile</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s80" ss:Formula="=ROUND(IF(R[-2]C=0,0,R[-2]C/R[-14]C),2)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>
          <Row ss:StyleID="Default">
            <Cell ss:StyleID="s73">
              <Data ss:Type="String">&#160;&#160;Cost/Seat Mile</Data>
            </Cell>
            <xsl:for-each select="NewDataSet/opCosts">
              <Cell ss:StyleID="s80" ss:Formula="=ROUND(IF(R[-1]C=0,0,R[-1]C/R[-16]C),2)">
                <Data ss:Type="Number">0</Data>
              </Cell>
            </xsl:for-each>
          </Row>

        </Table>

        <WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
          <Print>
            <ValidPrinterInfo/>
            <HorizontalResolution>600</HorizontalResolution>
            <VerticalResolution>600</VerticalResolution>
          </Print>
          <Selected/>
          <DoNotDisplayGridlines/>
          <DoNotDisplayOutline/>
          <ProtectObjects>False</ProtectObjects>
          <ProtectScenarios>False</ProtectScenarios>
        </WorksheetOptions>

      </Worksheet>

    </Workbook>

  </xsl:template>

</xsl:stylesheet>
