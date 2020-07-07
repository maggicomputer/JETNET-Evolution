<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="marketTimeSpanDropdowns.ascx.vb" Inherits="crmWebClient.marketTimeSpanDropdownsCtrl" %>

<% 
  ' ********************************************************************************
  ' Copyright 2004-11. JETNET,LLC. All rights reserved.
  '
  '$$Archive: /commonWebProject/controls/marketTimeSpanDropdowns.ascx $
  '$$Author: Mike $
  '$$Date: 6/19/19 8:46a $
  '$$Modtime: 6/18/19 6:12p $
  '$$Revision: 2 $
  '$$Workfile: marketTimeSpanDropdowns.ascx $
  '
  ' ********************************************************************************
%>
<script type="text/javascript" language="JavaScript" src="common/jsDate.js"></script>

<script type="text/javascript" language="JavaScript" src="common/marketPickDateScript.js"></script>

<script type="text/javascript" language="JavaScript">

  var timeScaleCboName = "<%= timeScaleCboName.trim%>ID";
  var startDateCboName = "<%= startDateCboName.trim%>ID";
  var displayRangeCboName = "<%= displayRangeCboName.trim%>ID";

</script>

<input type="hidden" name="sessTimeScale" value="<%= timeScaleValue.Trim%>" id="sessTimeScaleID" />
<input type="hidden" name="sessStartDate" value="<%= startDateValue.Trim%>" id="sessStartDateID" />
<input type="hidden" name="sessDisplayRange" value="<%= displayRangeValue.Trim%>" id="sessDisplayRangeID" />

<asp:Table ID="ScaleStartRange" runat="server" Width="100%" CellPadding="2" CellSpacing="0">
  <asp:TableRow>
    <asp:TableCell ID="tableCelltimeScale" HorizontalAlign="left" VerticalAlign="top">Summary&nbsp;Type&nbsp;:<br />
      <select id="<%= timeScaleCboName.trim%>ID" name="<%= timeScaleCboName.trim%>" onchange='JavaScript:fillStartDateJS("scale", <%= session.item("localPreferences").isHeliOnlyProduct.tostring.tolower%>, <%= session.item("localPreferences").isBusinessOnlyProduct.tostring.tolower%>, <%= Session.Item("localPreferences").isCommercialOnlyProduct.tostring.tolower%>);' title="Time Span" style="width:100px;">
        
        <% If timeScaleValue.ToLower.Trim = "years" Then%>
          <option value="Years" selected="selected">Yearly</option>
        <% Else%>
          <option value="Years">Yearly</option>
        <% End If%>
        
        <% If timeScaleValue.ToLower.Trim = "quarters" Then%>
          <option value="Quarters" selected="selected">Quarterly</option>
        <% Else%>
          <option value="Quarters">Quarterly</option>
        <% End If%>

        <% If timeScaleValue.ToLower.Trim = "months" Then%>
          <option value="Months" selected="selected">Monthly</option>
        <% Else%>
          <option value="Months">Monthly</option>
        <% End If%>
        
        <!--<option value="Days">Daily</option>-->
      </select>
    </asp:TableCell>
    <asp:TableCell ID="tableCellstartDate" HorizontalAlign="left" VerticalAlign="top">Start&nbsp;Date:<br />
      <select id="<%= startDateCboName.trim%>ID" name="<%= startDateCboName.trim%>" title="Start Date" style="width:100px;"><option value=''></option></select>
    </asp:TableCell>
    <asp:TableCell ID="tableCelldisplayRange" HorizontalAlign="left" VerticalAlign="top">Display&nbsp;Range:<br />
      <select id="<%= displayRangeCboName.trim%>ID" name="<%= displayRangeCboName.trim%>" title="Display Range" style="width:100px;"><option value=''></option></select>  
    </asp:TableCell>
  </asp:TableRow>
</asp:Table>
