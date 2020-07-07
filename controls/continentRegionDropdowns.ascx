<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="continentRegionDropdowns.ascx.vb" Inherits="crmWebClient.continentRegionDropdownsCtrl" %>

<% 
  ' ********************************************************************************
  ' Copyright 2004-11. JETNET,LLC. All rights reserved.
  '
  '$$Archive: /commonWebProject/controls/continentRegionDropdowns.ascx $
  '$$Author: Mike $
  '$$Date: 9/13/19 12:56p $
  '$$Modtime: 9/13/19 12:54p $
  '$$Revision: 4 $
  '$$Workfile: continentRegionDropdowns.ascx $
  '
  ' ********************************************************************************
%>

<script type="text/javascript" src="common/rebuildClientArray.js"></script>

<script type="text/javascript" src="common/RegionDropdowns.js"></script>

<script type="text/javascript" src="common/refreshCCSTDropdowns.js"></script>

<script type="text/javascript">
         	  
  var localAryContinent;
  var localAryRegion;
  var localTimeZoneAry;
   
  var sPassedServerStringJS = "";

  var bIsFirstControl = <%= bFirstControl.tostring.tolower %>;
  
  if (bIsFirstControl) {
    localAryContinent = null;
    localAryRegion = null;
    localTimeZoneAry = null;
    
    sPassedServerStringJS = "<%= HttpContext.Current.Server.HtmlEncode(countryString) %>";
    
    if (sPassedServerStringJS != "" && localAryContinent == null) {
      localAryContinent = createClientArrayFromServerStringJS(sPassedServerStringJS);
      sPassedServerStringJS = "";
    }
          	
    sPassedServerStringJS = "<%= HttpContext.Current.Server.HtmlEncode(regionString)%>";
    
    if (sPassedServerStringJS != "" && localAryRegion == null) {
      localAryRegion = createClientArrayFromServerStringJS(sPassedServerStringJS);
      sPassedServerStringJS = "";
    }    

    sPassedServerStringJS = "<%= HttpContext.Current.Server.HtmlEncode(timeZoneString)%>";
    
    if (sPassedServerStringJS != "" && localTimeZoneAry == null) {
      localTimeZoneAry = createClientArrayFromServerStringJS(sPassedServerStringJS);
      sPassedServerStringJS = "";
    }
  
  }

  var s_rememberLastCompanyStateJS = "";
  var s_rememberLastViewStateJS = "";
              
  var whichOneCompany = "<%= HttpContext.Current.Session.Item("companyRegionOrContinent").ToString.ToLower %>";
  var whichOneBase = "<%= HttpContext.Current.Session.Item("baseRegionOrContinent").ToString.ToLower %>";
  var whichOneView = "<%= HttpContext.Current.Session.Item("viewRegionOrContinent").ToString.ToLower %>";
        
  var companyRegion = "<%= HttpContext.Current.Session.Item("companyRegion").ToString %>";
  var baseRegion = "<%= HttpContext.Current.Session.Item("baseRegion").ToString %>";
  var viewRegion = "<%= HttpContext.Current.Session.Item("viewRegion").ToString %>";

  var companyCountry = "<%= HttpContext.Current.Session.Item("companyCountry").ToString %>";
  var baseCountry = "<%= HttpContext.Current.Session.Item("baseCountry").ToString %>";
  var viewCountry = "<%= HttpContext.Current.Session.Item("viewCountry").ToString %>";

  var companyState = "<%= HttpContext.Current.Session.Item("companyState").ToString %>";
  var baseState = "<%= HttpContext.Current.Session.Item("baseState").ToString %>";
  var viewState = "<%= HttpContext.Current.Session.Item("viewState").ToString %>";

  var companyTimeZone = "<%= HttpContext.Current.Session.Item("companyTimeZone").ToString %>";
  var viewTimeZone = "<%= HttpContext.Current.Session.Item("viewTimeZone").ToString %>";

  var sControlType = "<%= sControlType.tolower %>";

  if (sControlType == "view") {
    bIsBaseView = false;
    bIsViewView = true;
    bShowInactiveCountriesView = <%= bShowInactiveCountries.ToString.tolower %>;
  }

  if (sControlType == "base") {
    bIsBaseBase = true;
    bIsViewBase = false;
    bShowInactiveCountriesBase = <%= bShowInactiveCountries.ToString.tolower %>;
 }

  if (sControlType == "company") {
    bIsBaseCompany = false;
    bIsViewCompany = false;
    bShowInactiveCountriesCompany = <%= bShowInactiveCountries.ToString.tolower %>;
  }

  sControlType = '';

</script>

<input type="hidden" name="hasCompanyTimeZones" value="<%= session.item("hasCompanyTimeZones").tostring.tolower%>" id="hasCompanyTimeZonesID" />
<input type="hidden" name="hasViewTimeZones" value="<%= session.item("hasViewTimeZones").tostring.tolower%>" id="hasViewTimeZonesID" />

<asp:Table ID="ContinentRegionCountryState" runat="server" Width="100%" CellPadding="2" CellSpacing="0">
  <asp:TableRow>
    <asp:TableCell ID="tableCellContinentRegion" HorizontalAlign="Left" VerticalAlign="Middle">
       <asp:Literal ID="continentRegionDropdownsID" runat="server" Text=""></asp:Literal>   
    </asp:TableCell>
  </asp:TableRow>
  
</asp:Table>
