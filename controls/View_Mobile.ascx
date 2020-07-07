<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="View_Mobile.ascx.vb"
  Inherits="crmWebClient.View_Mobile" %>

<script type="text/javascript" src="https://cdn.rawgit.com/Mikhus/canvas-gauges/gh-pages/download/2.1.4/all/gauge.min.js"></script>

<script type="text/javascript">
  google.load('visualization', '1', { packages: ['corechart'] });
</script>
<style>
.tiny {
	font-size: 10px !important;
	line-height:10px !important;
	margin:0px !important;
	font-style: italic;
}</style>
<script type="text/javascript" src="https://www.google.com/jsapi?autoload={'modules':[{'name':'visualization','version':'1.0','packages':['corechart']},{'name':'visualization','version':'1.0','packages':['controls']}]}"></script>

<div id="containerBox" class="container ViewClass valueSpec aircraftListing Simplistic aircraftSpec" visible="false" runat="server">
  <div id="searchBox" runat="server" class="sixteen columns">
    <asp:Panel runat="server" ID="MobileSearchVisible" HorizontalAlign="center">
      <asp:DropDownList runat="server" AutoPostBack="true" ID="makeModelDynamic" CssClass="chosen-select"
        Width="100%">
        <asp:ListItem>Please pick a Model</asp:ListItem>
      </asp:DropDownList>
    </asp:Panel>
  </div>
  <div class="clearfix">
  </div>
  <div id="description_box" runat="server" class="sixteen columns">
    <asp:Image runat="server" ID="mainImage" />
    <!--Description-->
    <asp:Literal ID="description_text" runat="server"></asp:Literal>
  </div>
  <div id="market_box" runat="server" class="sixteen columns marketStatus">
    <!--Market Status-->
    <asp:Literal ID="market_text" runat="server"></asp:Literal>
  </div>
  <div id="fleet_box" runat="server" class="sixteen columns marketStatus">
    <!--Fleet-->
    <asp:Literal ID="fleet_text" runat="server"></asp:Literal>
  </div>
  <div id="performance_box" runat="server" class="sixteen columns  PerformanceListingTable Performance OpHeight">
    <!--Performance Specs-->
    <asp:Literal ID="performance_text" runat="server"></asp:Literal>
  </div>
  <div id="operating_box" runat="server" class="sixteen columns  PerformanceListingTable OpHeight">
    <!--Operating Costs-->
    <asp:Literal ID="operating_text" runat="server"></asp:Literal>
  </div>
  <div id="trends_box" runat="server" class="sixteen columns TrendsTabClass">
    <div>
      <!--Trends-->
      <asp:Chart ID="FOR_SALE" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
        Visible="False">
        <Series>
          <asp:Series Name="Series1" ChartArea="ChartArea1">
          </asp:Series>
        </Series>
        <ChartAreas>
          <asp:ChartArea Name="ChartArea1">
          </asp:ChartArea>
        </ChartAreas>
      </asp:Chart>
      <asp:Chart ID="AVG_PRICE_MONTH" Visible="False" runat="server" ImageStorageMode="UseImageLocation"
        ImageType="Jpeg">
        <Series>
          <asp:Series Name="Series1" ChartArea="ChartArea1">
          </asp:Series>
        </Series>
        <ChartAreas>
          <asp:ChartArea Name="ChartArea1">
          </asp:ChartArea>
        </ChartAreas>
      </asp:Chart>
      <asp:Chart ID="AVG_DAYS_ON" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
        Visible="False">
        <Series>
          <asp:Series ChartArea="ChartArea1" Name="Series1">
          </asp:Series>
        </Series>
        <ChartAreas>
          <asp:ChartArea Name="ChartArea1">
          </asp:ChartArea>
        </ChartAreas>
      </asp:Chart>
      <asp:Chart ID="PER_MONTH" runat="server" ImageStorageMode="UseImageLocation" ImageType="Jpeg"
        Visible="False">
        <Series>
          <asp:Series Name="Series1" ChartArea="ChartArea1">
          </asp:Series>
        </Series>
        <ChartAreas>
          <asp:ChartArea Name="ChartArea1">
          </asp:ChartArea>
        </ChartAreas>
      </asp:Chart>
      <asp:Literal ID="trends_text" runat="server"></asp:Literal>
    </div>
  </div>
  <div id="forSale_box" runat="server" class="sixteen columns ViewForSale">
    <!--For Sale-->
    <asp:Literal ID="forsale_text" runat="server"></asp:Literal>
    <asp:DataList ID="AircraftSearchDataList" runat="server" RepeatColumns="1" RepeatDirection="Horizontal"
      AutoGenerateColumns="False" GridLines="None" BorderColor="#eeeeee" AllowPaging="false"
      CssClass="mGrid">
      <ItemStyle VerticalAlign="Top" Width="100%" />
      <ItemTemplate>
        <div class="boxed_item_padding">
          <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_picture_id")), "<img src='" & IIf(HttpContext.Current.Session.Item("jetnetWebSiteType") <> crmWebClient.eWebSiteTypes.LOCAL, HttpContext.Current.Session.Item("jetnetFullHostName").ToString & HttpContext.Current.Session.Item("AircraftPicturesFolderVirtualPath") & "/", "https://www.testjetnetevolution.com/pictures/aircraft/") & DataBinder.Eval(Container.DataItem, "ac_id") & "-0-" & DataBinder.Eval(Container.DataItem, "ac_picture_id") & ".jpg' alt='AC Picture' width='220' class='border float_left cursor fullWidthMobile' onclick=""javascript:SubmitTransactionDocumentForm('" & DataBinder.Eval(Container.DataItem, "amod_make_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "amod_model_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString & "'," & DataBinder.Eval(Container.DataItem, "ac_id").ToString & ",0,'');""/>", IIf(DataBinder.Eval(Container.DataItem, "amod_airframe_type_code ").ToString = "F", "<img src='images/jet_no_image.jpg' width='220' class='border float_left fullWidthMobile toggleSmallScreen' />", "<img src='images/helo_no_image.jpg' width='220' class='border float_left fullWidthMobile toggleSmallScreen' />"))%>
          <div class="float_right dataListSeperator">
            <h1 class="dataListH1">
              <%#DataBinder.Eval(Container.DataItem, "ac_mfr_year")%>
              <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%>
              <%#crmWebClient.DisplayFunctions.WriteModelLink(DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "amod_model_name"), True)%>
              S/N
              <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString, "", "")%></h1>
            <span class="li"><span class="label">Year Mfr/Dlv:</span>
              <%#DataBinder.Eval(Container.DataItem, "ac_mfr_year")%>/<%#DataBinder.Eval(Container.DataItem, "ac_year")%></span>
            <span class="li"><span class="label">Reg #:</span>
              <%#DataBinder.Eval(Container.DataItem, "ac_reg_no")%></span>
            <%#crmWebClient.clsGeneral.clsGeneral.DisplayStatusListingDateEvoACListing(DataBinder.Eval(Container.DataItem, "ac_forsale_flag"), DataBinder.Eval(Container.DataItem, "ac_status"), DataBinder.Eval(Container.DataItem, "ac_delivery"), DataBinder.Eval(Container.DataItem, "ac_asking_price"), DataBinder.Eval(Container.DataItem, "ac_list_date"), DataBinder.Eval(Container.DataItem, "ac_asking"), False, Now())%>
            <asp:Label ID="company_information" runat="server" Text='<%#(crmWebClient.CompanyFunctions.FindEvolutionACCompanies(aclsData_Temp, DataBinder.Eval(Container.DataItem, "ac_id")))%>'></asp:Label>
          </div>
          <div class="float_left clear_left gray_background_color margin-top tiny_text">
            <asp:Label ID="lbl_aftt_estaftt" runat="server" Text='<%#crmWebClient.DisplayFunctions.showEstAFTT(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs")),DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs"),""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_est_airframe_hrs")),DataBinder.Eval(Container.DataItem, "ac_est_airframe_hrs"),""),IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_year")),DataBinder.Eval(Container.DataItem, "ac_year"),""),IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_times_as_of_date")),DataBinder.Eval(Container.DataItem, "ac_times_as_of_date"),""), true, false)%>'></asp:Label>
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_tot_hrs")) Or Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_tot_hrs")) Or Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_tot_hrs")) Or Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_tot_hrs")), "<span class='li_no_bullet' style='padding:0px !important;'><span class=""label"">Eng TT</span>: " & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_tot_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_1_tot_hrs") & "]", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_tot_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_2_tot_hrs") & "]", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_tot_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_3_tot_hrs") & "]", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_tot_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_4_tot_hrs") & "]", "") & "</span>", "")%>
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_soh_hrs")) Or Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_soh_hrs")) Or Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_soh_hrs")) Or Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_soh_hrs")), "<span class='li_no_bullet' style='padding:0px !important;'><span class=""label"">Eng SMOH</span>: " & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_soh_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_1_soh_hrs") & "]", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_soh_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_2_soh_hrs") & "]", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_soh_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_3_soh_hrs") & "]", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_soh_hrs")), "[" & DataBinder.Eval(Container.DataItem, "ac_engine_4_soh_hrs") & "]", "") & "</span>", "")%>
          </div>
        </div>
        <br class="div_clear" />
        <div class="seperator">
        </div>
      </ItemTemplate>
    </asp:DataList>
  </div>
  <div id="sold_box" runat="server" class="sixteen columns ViewForSale">
    <!--Recent Sales-->
    <asp:Literal ID="retailText" runat="server"></asp:Literal>
    <asp:DataList ID="TransactionSearchDataList" runat="server" RepeatColumns="1" RepeatDirection="Horizontal"
      AutoGenerateColumns="False" GridLines="None" BorderColor="#eeeeee" AllowPaging="false"
      CssClass="mGrid">
      <ItemStyle VerticalAlign="Top" CssClass="paddingRight" />
      <ItemTemplate>
        <div class="dataListSeperator no_bottom_border">
          <h1 class="dataListH1">
            <%#DataBinder.Eval(Container.DataItem, "ac_year")%>
            <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%>
            <%#crmWebClient.DisplayFunctions.WriteModelLink(DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "amod_model_name"), True)%>
            S/N
            <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString, "", "")%>
          </h1>
          <div class="dataListSeperatorHistory no_bottom_border">
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "journ_date")), "<span class=""li""><span class=""label"">Transaction Date:</span> " & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "journ_date")), crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, DataBinder.Eval(Container.DataItem, "journ_id"), True, crmWebClient.clsGeneral.clsGeneral.FormatDateShorthand(DataBinder.Eval(Container.DataItem, "journ_date")), "", ""), "") & "</span>", "")%>
            <span class="li">
              <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, DataBinder.Eval(Container.DataItem, "journ_id"), True, IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "jcat_subcategory_name")), DataBinder.Eval(Container.DataItem, "jcat_subcategory_name") & " - ", "") & DataBinder.Eval(Container.DataItem, "journ_subject").ToString, "", "")%><%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "journ_customer_note")), IIf(Not String.IsNullOrEmpty(Trim(DataBinder.Eval(Container.DataItem, "journ_customer_note").ToString)), "&nbsp;&nbsp;(<a " & crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, DataBinder.Eval(Container.DataItem, "journ_id"), False, "", "help_cursor", "") & " title='" & DataBinder.Eval(Container.DataItem, "journ_customer_note") & "' alt='" & DataBinder.Eval(Container.DataItem, "journ_customer_note") & "'  class='help_cursor error_text no_text_underline'>Note</a>)", ""), "")%></span>
          </div>
          <span class="li"><span class="label">Year Mfr/Dlv:</span>
            <%#DataBinder.Eval(Container.DataItem, "ac_mfr_year")%>/<%#DataBinder.Eval(Container.DataItem, "ac_year")%></span>
          <span class="li"><span class="label">Reg #:</span>
            <%#DataBinder.Eval(Container.DataItem, "ac_reg_no")%></span>
          <%#crmWebClient.clsGeneral.clsGeneral.DisplayStatusListingDateEvoACListing(DataBinder.Eval(Container.DataItem, "ac_forsale_flag"), DataBinder.Eval(Container.DataItem, "ac_status").ToString, DataBinder.Eval(Container.DataItem, "ac_delivery"), DataBinder.Eval(Container.DataItem, "ac_asking_price"), DataBinder.Eval(Container.DataItem, "ac_list_date"), DataBinder.Eval(Container.DataItem, "ac_asking"), True, DataBinder.Eval(Container.DataItem, "journ_date"))%>
          <br class="div_clear" />
          <div class="seperator">
          </div>
        </div>
      </ItemTemplate>
    </asp:DataList>
  </div>
  <div id="star_box" runat="server" class="sixteen columns StarReports">
    <!--Star-->
    <asp:Literal ID="starHeader" runat="server"></asp:Literal>
    <asp:Panel runat="server" ID="starReportHolder">
      <asp:Literal ID="starText" runat="server"></asp:Literal>
    </asp:Panel>
  </div>
</div>
