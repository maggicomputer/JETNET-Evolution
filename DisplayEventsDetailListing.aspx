<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="DisplayEventsDetailListing.aspx.vb"
  Inherits="crmWebClient.DisplayEventsDetailListing" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script language="javascript" type="text/javascript">
    
    function setAbsPage(locationHref) {
      if (Number(document.getElementById("txtGotoPageID").value) > 0) {
        document.location.href = locationHref + document.getElementById("txtGotoPageID").value;
      }
      return true;
    }

    function openSmallWindowJS(address, windowname) {
 
      var rightNow = new Date();
      windowname += rightNow.getTime();
      var Place = open(address, windowname, "menubar,scrollbars=1,resizable,width=1150,height=600");

      return true;
    }
 
  </script>

  <style type="text/css">
    A.underline
    {
      font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
      text-decoration: underline;
      cursor: pointer;
    }
    .module
    {
      border: 1px solid #CCD6DB;
      background-color: #ffffff;
    }
    .border
    {
      border: 1px solid #CCD6DB;
      background-color: #dddddd;
    }
    .header
    {
      background-image: url(../images/views_header.jpg);
      background-repeat: repeat-x;
      border-bottom: 1px solid #CCD6DB;
      color: #ffffff;
    }
    .tabheader
    {
      border-width: 1px 1px 0px 0px;
      border-style: solid;
      border-color: #CCD6DB;
      background-color: #EEEEEE;
      text-align: left;
    }
    .border_bottom
    {
      border-width: 0px 0px 1px 0px;
      border-style: solid;
      border-color: #CCD6DB;
    }
    .border_bottom_right
    {
      border-width: 0px 1px 1px 0px;
      border-style: solid;
      border-color: #CCD6DB;
    }
    .leftside
    {
      border-width: 0px 0px 0px 1px;
      border-style: solid;
      border-color: #CCD6DB;
      text-align: left;
    }
    .rightside
    {
      border-width: 0px 1px 1px 0px;
      border-style: solid;
      border-color: #CCD6DB;
    }
    .leftside_right
    {
      border-width: 0px 0px 0px 1px;
      border-style: solid;
      border-color: #CCD6DB;
      text-align: right;
    }
    .seperator
    {
      border-width: 0px 0px 1px 0px;
      border-style: solid;
      border-color: #CCD6DB;
    }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <table width="100%" cellspacing="0" cellpadding="0" class="DetailsBrowseTable">
    <tr>
      <td align="left" valign="top">
        <div class="backgroundShade">
          <a href="#" onclick="javascript:load('help.aspx','','');"
            class="float_left"><img src="/images/help-circle.svg" alt="Help" /></a> <a href="#"
              class="float_right" onclick="javascript:window.close();"><img src="/images/x.svg" alt="Close" /></a><div
                class="clear">
              </div>
        </div>
      </td>
    </tr>
  </table>

  <table width="100%" cellpadding="0" cellspacing="0">
    <tr>
      <td align="center" valign="top" width="100%">
        <asp:Literal ID="detailEventsList" runat="server" Visible="true"></asp:Literal>
      </td>
    </tr>
  </table>
</asp:Content>
