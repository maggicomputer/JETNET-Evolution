<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="yachtCatBrandModel.ascx.vb" Inherits="crmWebClient.yachtCatBrandModel" %>

<% 
  ' ********************************************************************************
  ' Copyright 2004-11. JETNET,LLC. All rights reserved.
  '
  '$$Archive: /commonWebProject/controls/yachtCatBrandModel.ascx $
  '$$Author: Mike $
  '$$Date: 6/19/19 8:47a $
  '$$Modtime: 6/18/19 6:12p $
  '$$Revision: 2 $
  '$$Workfile: yachtCatBrandModel.ascx $
  '
  ' ********************************************************************************
%>

<style type="text/css">

  A.underline
  {
    font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
    text-decoration: underline;
    cursor: pointer;
  }
  
</style>

<script type="text/javascript" language="JavaScript" src="common/rebuildClientArray.js"></script>

<script type="text/javascript" language="JavaScript" src="common/categoryBrandModel.js"></script>

<script type="text/javascript" language="JavaScript">
         	  
  var localMasterYachtArray = null;
  var localYachtLableArray = null;

  var sPassedServerStringJS = "";

  var yachtCategoryCboName = "<%= controlYachtCategoryName.trim%>ID";
  var yachtBrandCboName = "<%= controlYachtBrandName.trim%>ID";
  var yachtModelCboName = "<%= controlYachtModelName.trim%>ID";
       
  sPassedServerStringJS = "<%= server.htmlEncode(yachtBrandModelString) %>";

  if (sPassedServerStringJS != "" && localMasterYachtArray == null) {
    localMasterYachtArray = createClientArrayFromServerStringJS(sPassedServerStringJS);
    sPassedServerStringJS = "";
  }

  sPassedServerStringJS = "<%=server.htmlEncode(yachtMotorCategoryString)%>";

  if (sPassedServerStringJS != "" && localYachtLableArray == null) {
    localYachtLableArray = createClientArrayFromServerStringJS(sPassedServerStringJS);
    sPassedServerStringJS = "";
  }    
        	 
  function openSmallWindowJS(address, windowname) {
    var rightNow = new Date();
    windowname += rightNow.getTime();
    var Place = open(address, windowname, "scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no");

    return true;
  }
 
</script>

<input type="hidden" name="sessYachtCategory" value="<%= controlYachtCategory.Trim%>" id="sessYachtCategoryID" />
<input type="hidden" name="sessYachtBrand" value="<%= controlYachtBrand.Trim%>" id="sessYachtBrandID" />
<input type="hidden" name="sessYachtModel" value="<%= controlYachtModel.Trim%>" id="sessYachtModelID" />

<asp:Table ID="YachtCategoryBrandModelTable" runat="server" Width="100%" CellPadding="2" CellSpacing="0">
  <asp:TableRow ID="tableRowSpacer">
    <asp:TableCell ID="tableCellSpacer" HorizontalAlign="Left" Wrap="false" VerticalAlign="Middle" style="padding: 2px;">
    &nbsp;
    </asp:TableCell>
  </asp:TableRow>
  <asp:TableRow>
    <asp:TableCell ID="tableCellCategory" HorizontalAlign="left" VerticalAlign="bottom">
  Category&nbsp;:<br />
  <select <%= sHTMLSelectText.tolower%>size="<%= sHTMLSelectSize.trim%>" name="<%= controlYachtCategoryName.trim%>" id="<%= controlYachtCategoryName.trim%>ID" onchange='JavaScript:refreshYachtCategoryBrandModel("onChange","category");' title="Category" style="width:200px;">
  <option selected="selected" value="All">All</option></select>
    </asp:TableCell>
    <asp:TableCell ID="tableCellBrand" HorizontalAlign="left" VerticalAlign="bottom">
  Brand&nbsp;:<br />
  <select <%= sHTMLSelectText.tolower%>size="<%= sHTMLSelectSize.trim%>" name="<%= controlYachtBrandName.trim%>" id="<%= controlYachtBrandName.trim%>ID" onchange='JavaScript:refreshYachtCategoryBrandModel("onChange","brand");' title="Brand" style="width:200px;">
  <option value="All">All</option></select>
    </asp:TableCell>
    <asp:TableCell ID="tableCellModel" HorizontalAlign="left" VerticalAlign="bottom">
  Model&nbsp;:<br />
  <select <%= sHTMLSelectText.tolower%>size="<%= sHTMLSelectSize.trim%>" name="<%= controlYachtModelName.trim%>" id="<%= controlYachtModelName.trim%>ID" onchange='JavaScript:refreshYachtCategoryBrandModel("onChange","model");' title="Model" style="width:155px;">
  <option value="All">All</option></select>
    </asp:TableCell>
  </asp:TableRow>
</asp:Table>
