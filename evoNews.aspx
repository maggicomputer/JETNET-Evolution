<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="evoNews.aspx.vb" Inherits="crmWebClient.evoNews"
  MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <style type="text/css">
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
    .picture
    {
      overflow: auto;
      width: 310px;
      height: 212px;
      margin: 0px;
      padding-top: 5px;
      background-color: #1f6c9a;
      vertical-align: middle;
      text-align: center;
    }
    .papers a
    {
      background-image: url(../images/papers.jpg);
      background-repeat: no-repeat;
      padding-left: 26px;
      padding-top: 3px;
      line-height: 15px;
      display: block;
    }
    .cover
    {
      background-image: url(../images/star_cover.jpg);
      background-repeat: no-repeat;
      width: 250px;
      height: 350px;
      float: right;
      color: white;
    }
    .cover a
    {
      color: #ffffff;
      font-size: 14px;
    }
    .cover a:hover
    {
      color: #ff0000;
      font-size: 14px;
    }
    .cover .toptitle
    {
      color: #ffffff;
      font-size: 18px;
      font-weight: bold;
    }
    .cover .title
    {
      color: #ffffff;
      font-size: 14px;
      font-weight: bold;
    }
    .tiny
    {
      font-size: 10px;
      font-style: italic;
    }
    A.White:active
    {
      font-size: 8pt;
      color: white;
      font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
    }
    A.White:link
    {
      font-size: 8pt;
      color: white;
      font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
      text-decoration: underline;
    }
    A.White:visited
    {
      font-size: 8pt;
      color: white;
      font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
      text-decoration: underline;
    }
    A.White:hover
    {
      font-size: 8pt;
      color: Yellow;
      font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
    }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <p class="DetailsBrowseTable">
    <a href="#" class="light_gray_button float_right" onclick="javascript:window.close();">
      Close</a></p>
  <div class="clear">
  </div>
  <div class="NotesHeader">
  </div>
  <cc1:TabContainer runat="server" ID="tab_container_ID" Width="100%" BorderStyle="None"
    CssClass="dark-theme">
    <cc1:TabPanel ID="masterNews" runat="server" HeaderText="Master News List">
      <ContentTemplate>
        <div style="width: 100%; text-align: left;">
          <asp:Literal ID="newsContent" runat="server" Text="Please wait a moment for news ..."></asp:Literal>
        </div>
      </ContentTemplate>
    </cc1:TabPanel>
  </cc1:TabContainer>
</asp:Content>
