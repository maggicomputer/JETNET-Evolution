<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="eventCategoryType.ascx.vb"
  Inherits="crmWebClient.eventCategoryType" %>
<% 
  ' ********************************************************************************
  ' Copyright 2004-11. JETNET,LLC. All rights reserved.
  '
  '$$Archive: /commonWebProject/controls/eventCategoryType.ascx $
  '$$Author: Mike $
  '$$Date: 6/19/19 8:46a $
  '$$Modtime: 6/18/19 6:12p $
  '$$Revision: 2 $
  '$$Workfile: eventCategoryType.ascx $
  '
  ' ********************************************************************************
%>

<script type="text/javascript" language="JavaScript" src="common/rebuildClientArray.js"></script>

<script type="text/javascript" language="JavaScript" src="common/eventSearchScript.js"></script>

<script type="text/javascript" language="JavaScript">

  var eventCatTypeCboName = "<%= eventCatTypeCboName.trim%>ID";
  var eventCatTypeCodeCboName = "<%= eventCatTypeCodeCboName.trim%>ID";

  var localAryEventCategory;
  
  var sPassedServerStringJS = "";

  sPassedServerStringJS = "<%= server.htmlEncode(eventCatString) %>";

  if (sPassedServerStringJS != "" && localAryEventCategory == null) {
    localAryEventCategory = createClientArrayFromServerStringJS(sPassedServerStringJS);
    sPassedServerStringJS = "";
  }

  var eventCatType = "<%= session.item("eventCatType").tostring.tolower %>";
  var eventCatCode = "<%= session.item("eventCatCode").tostring.tolower %>";

</script>

<input type="hidden" name="radEventsValue" value="<%= session.Item("eventType").tostring.toupper%>" id="radEventsValueID" />

<asp:Table ID="eventsCatagories" runat="server" Width="100%" CellPadding="2" CellSpacing="0">
  <asp:TableRow>
    <asp:TableCell ID="tableCell" HorizontalAlign="Left" VerticalAlign="Middle" ColumnSpan="2">
     
      <span class="displayNoneMobile"><input type="radio" name="radEvents" id="radEventsID" value="Aircraft" onclick='javascript:refreshEventCombosJS("onclick","air", eventCatType, eventCatCode);' />Aircraft Events&nbsp;</span>
      <span class="displayNoneMobile"><input type="radio" name="radEvents" id="radEventsID1" value="Wanted" onclick='javascript:refreshEventCombosJS("onclick","wnt", eventCatType, eventCatCode);' />Wanted Events&nbsp;</span>
      <span class="displayNoneMobile"><input type="radio" name="radEvents" id="radEventsID2" value="Company" onclick='javascript:refreshEventCombosJS("onclick","cmp", eventCatType, eventCatCode);' />Company Only Events</span>
    
    </asp:TableCell>
  </asp:TableRow>
  <asp:TableRow>
    <asp:TableCell ID="tableCell0" HorizontalAlign="Left" VerticalAlign="Middle">
      Category:
    </asp:TableCell>
    <asp:TableCell ID="tableCell1" HorizontalAlign="Left" VerticalAlign="Middle">
      Type:
    </asp:TableCell>
  </asp:TableRow>
  <asp:TableRow>
    <asp:TableCell ID="tableCell2" HorizontalAlign="Left" VerticalAlign="Middle">
      <select size="<%= sHTMLSelectSize.trim%>" <%= iif(session.item("isMobile") = false, "multiple=""multiple""", "") %> id="<%= eventCatTypeCboName.trim%>ID" name="<%= eventCatTypeCboName.trim%>" onchange='javascript:refreshEventCombosJS("onchange","cat", eventCatType, eventCatCode);' title="Categories" style="width:155px;">
       <option selected="selected" value="All">All</option>
      </select>
    </asp:TableCell>
    <asp:TableCell ID="tableCell3" HorizontalAlign="Left" VerticalAlign="Middle">
      <select size="<%= sHTMLSelectSize.trim%>" <%= iif(session.item("isMobile") = false, "multiple=""multiple""", "") %> id="<%= eventCatTypeCodeCboName.trim%>ID" name="<%= eventCatTypeCodeCboName.trim%>" onchange='javascript:refreshEventCombosJS("onchange","code", eventCatType, eventCatCode);' title="Types" style="width:195px;">
      <option selected="selected" value="All">All</option>
    </select>
    </asp:TableCell>
  </asp:TableRow>
  <asp:TableRow>
  <asp:TableCell ID="tableCell4" HorizontalAlign="Left" VerticalAlign="Middle" ColumnSpan="2">
      <div class="red_text" id="eventsMsgID">&nbsp;</div>
  </asp:TableCell>
  </asp:TableRow>
</asp:Table>
