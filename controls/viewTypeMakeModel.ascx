<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="viewTypeMakeModel.ascx.vb" Inherits="crmWebClient.viewTypeMakeModelCtrl" %>
<% 
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/controls/viewTypeMakeModel.ascx $
'$$Author: Mike $
'$$Date: 6/23/20 3:44p $
'$$Modtime: 6/23/20 1:46p $
'$$Revision: 9 $
'$$Workfile: viewTypeMakeModel.ascx $
'
' ********************************************************************************
%>
<style type="text/css">
  A.underline {
    font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
    text-decoration: underline;
    cursor: pointer;
  }
</style>

<script type="text/javascript">

  var localMasterAirframeArray = null;
  var localFilterAirframeArray = null;
  var localAircraftTypeLableArray = null;
  var localDefaultAirframeArray = null;

  var localMfrNamesArray = null;
  var localAircraftSizeArray = null;

  var s_rememberLastFilterJS = "";
  var b_isFilteredJS = <%= httpcontext.current.session.item("hasModelFilter").tostring.tolower %>;

  var sPassedServerStringJS = "";

  var typeCboName = "<%= controlAcTypeName.trim%>ID";
  var MakeCboName = "<%= controlAcMakeName.trim%>ID";
  var ModelCboName = "<%= controlAcModelName.trim%>ID";

  var mfrNamesCboName = "ddlMfrNameID";
  var sizeCboName = "ddlSizeCatID";

  //  var lastFilter = "";

  //  if (document.getElementById("lastModelFilterID") != null) {
  //    lastFilter = document.getElementById("lastModelFilterID").value
  //  }

  //  alert("lstFilter: " + lastFilter + " b_isFilteredJS: " + b_isFilteredJS)

  chkDefaultFilterID = "chkDefaultFilterID";

  sPassedServerStringJS = "<%=Server.HtmlEncode(makeModelString) %>";

  if (sPassedServerStringJS != "" && localMasterAirframeArray == null) {
    localMasterAirframeArray = createClientArrayFromServerStringJS(sPassedServerStringJS);
    sPassedServerStringJS = "";
  }

  sPassedServerStringJS = "<%=server.htmlEncode(typeLableString)%>";

  if (sPassedServerStringJS != "" && localAircraftTypeLableArray == null) {
    localAircraftTypeLableArray = createClientArrayFromServerStringJS(sPassedServerStringJS);
    sPassedServerStringJS = "";
  }

  sPassedServerStringJS = "<%=server.htmlEncode(defaultMakeModelString)%>";

  if (sPassedServerStringJS != "" && localDefaultAirframeArray == null) {
    localDefaultAirframeArray = createClientArrayFromServerStringJS(sPassedServerStringJS);
    sPassedServerStringJS = "";
  }

  sPassedServerStringJS = "<%=Server.HtmlEncode(mfrNamesString)%>";

  if (sPassedServerStringJS != "" && localMfrNamesArray == null) {
    localMfrNamesArray = createClientArrayFromServerStringJS(sPassedServerStringJS);
    sPassedServerStringJS = "";
  }

  sPassedServerStringJS = "<%=Server.HtmlEncode(sizeString)%>";

  if (sPassedServerStringJS != "" && localAircraftSizeArray == null) {
    localAircraftSizeArray = createClientArrayFromServerStringJS(sPassedServerStringJS);
    sPassedServerStringJS = "";
  }

  function selectDefaultMakeModel() {
    refreshTypeMakeModelByCheckBox("", "", <%= isHeliOnlyProduct.ToString.ToLower%>,<%= productCodeCount.ToString%>);
    return true;
  }

  function openSmallWindowJS(address, windowname) {
    var rightNow = new Date();
    windowname += rightNow.getTime();
    var Place = open(address, windowname, "scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no");

    return true;
  }

</script>

<input type="hidden" name="hasModelFilter" value="" id="hasModelFilterID" />
<input type="hidden" name="hasHelicopterFilter" value="<%=HttpContext.Current.Session.Item("hasHelicopterFilter").ToString.ToLower%>" id="hasHelicopterFilterID" />
<input type="hidden" name="hasBusinessFilter" value="<%=HttpContext.Current.Session.Item("hasBusinessFilter").ToString.ToLower%>" id="hasBusinessFilterID" />
<input type="hidden" name="hasCommercialFilter" value="<%=HttpContext.Current.Session.Item("hasCommercialFilter").ToString.ToLower%>" id="hasCommercialFilterID" />
<input type="hidden" name="hasRegionalFilter" value="<%=HttpContext.Current.Session.Item("hasRegionalFilter").ToString.ToLower%>" id="hasRegionalFilterID" />
<input type="hidden" name="lastModelFilter" value="<%=HttpContext.Current.Session.Item("lastModelFilter").ToString.ToUpper%>" id="lastModelFilterID" />

<input type="hidden" name="sessAircraftType" value="<%=controlAcType.Trim%>" id="sessAircraftTypeID" />
<input type="hidden" name="sessAircraftMake" value="<%=controlAcMake.Trim%>" id="sessAircraftMakeID" />
<input type="hidden" name="sessAircraftModel" value="<%=controlAcModel.Trim%>" id="sessAircraftModelID" />
<input type="hidden" name="sessAircraftMfrNames" value="<%=controlAcMfrNames.Trim%>" id="sessAircraftMfrNamesID" />
<input type="hidden" name="sessAircraftSize" value="<%=controlAcSize.Trim%>" id="sessAircraftSizeID" />

<asp:Table ID="typeMakeModelTable" runat="server" Width="100%" CellPadding="3" CellSpacing="0">
  <asp:TableRow ID="tableRowWeightClassProductFilter">
    <asp:TableCell ID="tableCellWeightClass" HorizontalAlign="Left" Wrap="false" VerticalAlign="Middle"
      Style="padding: 2px;">
      <a class="underline" onclick="javascript:load('MasterLists.aspx?helplist=weightclass','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"
        title="Click to see Weight Class Descriptions">Weight&nbsp;Class</a>&nbsp;:&nbsp;
      <asp:DropDownList ID="ddlWeightClass" runat="server" onclientclick='' ToolTip="Select Weight Class">
        <asp:ListItem Value="All">All</asp:ListItem>
        <asp:ListItem Value="V">Very Light Jet</asp:ListItem>
        <asp:ListItem Value="L">Light</asp:ListItem>
        <asp:ListItem Value="M">Medium</asp:ListItem>
        <asp:ListItem Value="H">Heavy</asp:ListItem>
      </asp:DropDownList>
    </asp:TableCell>
    <asp:TableCell ID="tableCellFilter">
      <asp:Literal ID="productCodeFilter" runat="server" Text=""></asp:Literal>
    </asp:TableCell>
  </asp:TableRow>
  <asp:TableRow>
    <asp:TableCell ID="tableCellType" HorizontalAlign="left" VerticalAlign="bottom" Width="33%">
  Type&nbsp;:<br />
  <select <%= sHTMLSelectText.tolower%>size="<%= sHTMLSelectSize.trim%>" name="<%= controlAcTypeName.trim%>" id="<%= controlAcTypeName.trim%>ID" onchange='JavaScript:refreshTypeMakeModelByCheckBox("onChange","type",<%= isHeliOnlyProduct.tostring.tolower%>,<%= productCodeCount.tostring%>);' title="Type" style="width: 98%;">
  <option selected="selected" value="All">All</option></select>
    </asp:TableCell>
    <asp:TableCell ID="tableCellMake" HorizontalAlign="left" VerticalAlign="bottom" Width="33%">
  Make&nbsp;:<br />
  <select <%= sHTMLSelectText.tolower%>size="<%= sHTMLSelectSize.trim%>" name="<%= controlAcMakeName.trim%>" id="<%= controlAcMakeName.trim%>ID" onchange='JavaScript:refreshTypeMakeModelByCheckBox("onChange","make",<%= isHeliOnlyProduct.tostring.tolower%>,<%= productCodeCount.tostring%>);' title="Make" style="width: 98%;">
  <option value="All">All</option></select>
    </asp:TableCell>
    <asp:TableCell ID="tableCellModel" HorizontalAlign="left" VerticalAlign="bottom" Width="33%">
  Model&nbsp;:<br />
  <select <%= sHTMLSelectText.tolower%>size="<%= sHTMLSelectSize.trim%>" name="<%= controlAcModelName.trim%>" id="<%= controlAcModelName.trim%>ID" onchange='JavaScript:refreshTypeMakeModelByCheckBox("onChange","model",<%= isHeliOnlyProduct.tostring.tolower%>,<%= productCodeCount.tostring%>);' title="Model" style="width: 98%;">
  <option value="All">All</option></select>
    </asp:TableCell>
  </asp:TableRow>
  <asp:TableRow ID="tableRowExtraSelections">
    <asp:TableCell ID="tableCellExtraSelections" ColumnSpan="3" HorizontalAlign="Left" VerticalAlign="Top" Wrap="false">
      <table border="0" style="padding: 2px; border-spacing: 6px; text-align: left; width: 100%;">
        <tr>
          <td style="vertical-align: text-bottom; text-align: left; padding-top: 8px; width: 50%;">
            <div runat="server" id="ddlMfrNameDiv">
              Manufacturer&nbsp;:<br />
              <select multiple="multiple" size="4" name="ddlMfrName" id="ddlMfrNameID" onchange='JavaScript:refreshTypeMakeModelByCheckBox("onChange","",<%= isHeliOnlyProduct.tostring.tolower%>,<%= productCodeCount.tostring%>);' title="Manufacturer Name" style="width: 98%;">
                <option value="All">All</option>
              </select>
            </div>
          </td>
          <td style="vertical-align: text-bottom; text-align: left; padding-top: 8px;">
            <div runat="server" id="ddlSizeCatDiv">
              Airframe&nbsp;Category&nbsp;:<br />
              <select multiple="multiple" size="4" name="ddlSizeCat" id="ddlSizeCatID" onchange='JavaScript:refreshTypeMakeModelByCheckBox("onChange","",<%= isHeliOnlyProduct.tostring.tolower%>,<%= productCodeCount.tostring%>);' title="Airframe Category" style="width: 98%;">
                <option value="All">All</option>
              </select>
            </div>
          </td>
        </tr>
      </table>
    </asp:TableCell>
  </asp:TableRow>
  <asp:TableRow ID="tableRowDefaultModelsCheck">
    <asp:TableCell ID="tableCellDefaultmodelsCheck" ColumnSpan="3" HorizontalAlign="Right" VerticalAlign="Middle" Wrap="false">
      <input style="display: inline;" type="checkbox" id="chkDefaultFilterID" onclick='selectDefaultMakeModel();'
        title="Check to set selections to show default models" />&nbsp;<asp:Label ID="DefaultFilterLabel"
          runat="server" Text="Default&nbsp;Aircraft&nbsp;"></asp:Label>
    </asp:TableCell>
  </asp:TableRow>
</asp:Table>
