<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/MobileTheme.master"
  CodeBehind="Airport_Listing.aspx.vb" Inherits="crmWebClient.Airport_Listing" %>

<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="content">
  <div class="AirportListing fixPosition PerformanceListingTable">
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
      <tr>
        <td align="left" valign="top" class="dark_header" width="100%">
          <asp:Table ID="Table3" runat="server" Width="100%" CellPadding="0" CellSpacing="0"
            CssClass="padding_table">
            <asp:TableRow>
              <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="90" ID="expand_text">
                <a href="javascript:void(0);" id="controlLink" runat="server" class="newSearchLink">
                </a>
              </asp:TableCell>
              <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" ID="results_text" CssClass="mobile_padding">
                <asp:UpdatePanel runat="server" ID="criteriaUpdatePanel" UpdateMode="Conditional">
                  <ContentTemplate>
                    <asp:Label ID="criteria_results" runat="server" Text=""></asp:Label>
                  </ContentTemplate>
                </asp:UpdatePanel>
              </asp:TableCell>
            </asp:TableRow>
          </asp:Table>
        </td>
      </tr>
    </table>
    <asp:Panel ID="Collapse_Panel" runat="server" Width="100%">
      <asp:Table ID="Table4" Width="100%" CellPadding="2" CellSpacing="0" runat="server"
        CssClass="mobileWhiteBackground">
        <asp:TableRow>
          <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="90px">Airport Name:</asp:TableCell>
          <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
            <asp:TextBox runat="server" ID="airportSearchTxt" CssClass="float_left" Width="95%"></asp:TextBox>
            <asp:TextBox runat="server" ID="airportSearchID" CssClass="display_none"></asp:TextBox>
          </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
          <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">City:</asp:TableCell>
          <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
            <asp:TextBox runat="server" ID="airportCitytxt" CssClass="float_left" Width="95%"></asp:TextBox>
          </asp:TableCell>
        </asp:TableRow>
         <asp:TableRow>
          <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">IATA/ICAO:</asp:TableCell>
          <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
            <asp:TextBox runat="server" ID="airportIATACodetxt"  Width="95%"></asp:TextBox>
          </asp:TableCell>
        </asp:TableRow> 
        <asp:TableRow>
          <asp:TableCell HorizontalAlign="right" VerticalAlign="Top" ColumnSpan="2">
            <asp:UpdatePanel runat="server" ID="SearchUpdate" UpdateMode="Conditional">
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="airportSearchButton" />
              </Triggers>
              <ContentTemplate>
              </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Button runat="server" ID="airportSearchButton" Text="Search" CssClass="button-darker" Width="50%" />
          </asp:TableCell>
        </asp:TableRow>
      </asp:Table>
    </asp:Panel>
  </div>
  <div class="DataGridShadowContainer AirportListing">
    <asp:UpdatePanel runat="server" ID="listingUpdatePanel" UpdateMode="Conditional"
      ChildrenAsTriggers="false">
      <ContentTemplate>
        <asp:DataList ID="ResultsSearchDataList" runat="server" RepeatColumns="1" RepeatDirection="Horizontal"
          AutoGenerateColumns="False" GridLines="horizontal" BorderColor="#eeeeee" AllowPaging="true"
          Visible="true" CssClass="mGrid mobileTopPaddingAttention">
          <ItemStyle VerticalAlign="Top" Width="50%" />
          <ItemTemplate>
            <div class="boxed_item_padding">
              <div id="_<%#DataBinder.Eval(Container.DataItem, "aport_id").ToString%>" class="swipeToMove float_right CompanydataListSeperator mobileWidth">
                <h1>
                  <a id="<%#DataBinder.Eval(Container.DataItem, "aport_id").ToString%>" class="cursor"
                    href="javascript:void(0);">
                    <%#DataBinder.Eval(Container.DataItem, "aport_name").ToString%></a></h1>
                <%#DisplayIataIcao(DataBinder.Eval(Container.DataItem, "aport_iata_code").ToString, DataBinder.Eval(Container.DataItem, "aport_icao_code").ToString)%>&ndash;
                <%#IIf(DataBinder.Eval(Container.DataItem, "aport_city").ToString <> "" Or DataBinder.Eval(Container.DataItem, "aport_state").ToString <> "" Or DataBinder.Eval(Container.DataItem, "aport_country").ToString <> "", IIf(DataBinder.Eval(Container.DataItem, "aport_city").ToString <> "", DataBinder.Eval(Container.DataItem, "aport_city").ToString & ", ", "") & DataBinder.Eval(Container.DataItem, "aport_state").ToString & " " & Replace(DataBinder.Eval(Container.DataItem, "aport_country").ToString, "United States", "US"), "")%>
              </div>
            </div>
          </ItemTemplate>
        </asp:DataList>
        <asp:Label ID="ap_attention" runat="server" Text="" CssClass="red_text emphasis_text text_align_center small_to_medium_text mobileTopPaddingAttention"></asp:Label>
      </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="mobileUpdate" UpdateMode="Conditional" ChildrenAsTriggers="false">
      <ContentTemplate>
        <asp:Literal runat="server" ID="airportName"></asp:Literal>
        <asp:Label ID="ac_attention" runat="server" Text="" CssClass="red_text emphasis_text text_align_center small_to_medium_text mobileTopPaddingAttention"></asp:Label>
        <asp:DataList ID="mobileDataList" runat="server" RepeatColumns="1" RepeatDirection="Horizontal"
          AutoGenerateColumns="False" GridLines="horizontal" BorderColor="#eeeeee" AllowPaging="false"
          CssClass="mGrid">
          <ItemStyle VerticalAlign="Top" />
          <ItemTemplate>
            <div class="boxed_item_padding swipeToMoveBack">
              <h1 class="dataListH1 float_left div_clear">
                <%#crmWebClient.DisplayFunctions.TrimName(DataBinder.Eval(Container.DataItem, "ac_mfr_year"), DataBinder.Eval(Container.DataItem, "amod_make_name"), DataBinder.Eval(Container.DataItem, "amod_model_name"), DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "ac_ser_no_full"), DataBinder.Eval(Container.DataItem, "ac_id"))%></h1>
              <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_picture_id")), "<img src='" & IIf(HttpContext.Current.Session.Item("jetnetWebSiteType") <> crmWebClient.eWebSiteTypes.LOCAL, HttpContext.Current.Session.Item("jetnetFullHostName").ToString & HttpContext.Current.Session.Item("AircraftPicturesFolderVirtualPath") & "/", "https://www.testjetnetevolution.com/pictures/aircraft/") & DataBinder.Eval(Container.DataItem, "ac_id") & "-0-" & DataBinder.Eval(Container.DataItem, "ac_picture_id") & ".jpg' alt='AC Picture' width='220' class='border float_left cursor' onclick=""javascript:SubmitTransactionDocumentForm('" & DataBinder.Eval(Container.DataItem, "amod_make_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "amod_model_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString & "'," & DataBinder.Eval(Container.DataItem, "ac_id").ToString & ",0,'');""/>", IIf(DataBinder.Eval(Container.DataItem, "amod_airframe_type_code ").ToString = "F", "<img src='images/jet_no_image.jpg' width='220' class='border float_left' />", "<img src='images/helo_no_image.jpg' width='220' class='border float_left' />"))%>
              <div class="float_right halfScreen">
                <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_reg_no")), "<span class=""float_left"">" & DataBinder.Eval(Container.DataItem, "ac_reg_no") & "</span>", "")%>
                <%#showEstAFTT(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs")), DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs"), ""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_est_airframe_hrs")), DataBinder.Eval(Container.DataItem, "ac_est_airframe_hrs"), ""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_year")), DataBinder.Eval(Container.DataItem, "ac_year"), ""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_times_as_of_date")), DataBinder.Eval(Container.DataItem, "ac_times_as_of_date"), ""), True, False)%>
                <%#crmWebClient.clsGeneral.clsGeneral.MobileDisplayStatus(DataBinder.Eval(Container.DataItem, "ac_forsale_flag"), DataBinder.Eval(Container.DataItem, "ac_status"), DataBinder.Eval(Container.DataItem, "ac_delivery"), DataBinder.Eval(Container.DataItem, "ac_asking_price"), DataBinder.Eval(Container.DataItem, "ac_list_date"), DataBinder.Eval(Container.DataItem, "ac_asking"), False, Now())%>
                <asp:Label ID="company_information" runat="server" Text=''></asp:Label>
                <%#crmWebClient.DisplayFunctions.DisplayBaseInfo(DataBinder.Eval(Container.DataItem, "ac_aport_country"), DataBinder.Eval(Container.DataItem, "ac_aport_state"))%>
                <asp:Label ID="Label1" runat="server" Text='<%#(crmWebClient.DisplayFunctions.DisplayMobileCompanies(DataBinder.Eval(Container.DataItem, "ac_id")))%>'></asp:Label>
              </div>
            </div>
          </ItemTemplate>
        </asp:DataList>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
  <div id="divTabLoading" runat="server" class="loadingScreenAC display_none" align="center">
    <p>
      Please Wait While Search is Loading.</p>
    <img src="Images/loading.gif" alt="Loading..." />
  </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="below_form">
</asp:Content>
