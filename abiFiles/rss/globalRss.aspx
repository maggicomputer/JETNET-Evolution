<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/ABITheme.Master"
  CodeBehind="globalRss.aspx.vb" Inherits="crmWebClient.globalRss" %>
<%@ MasterType VirtualPath="~/EvoStyles/ABITheme.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main_content" runat="server">
  <div id="component" class="span9">
    <main role="main">
     <section class="page-blog page-blog__">
		    <header class="page_header">
		      <h3><span runat="server" id="news_header">RSS Feeds</span> </h3>	
		    </header>
		    <div class="items-row row-0 row-fluid">
		    <span class="span9">
		       <p>
   JETNET Global offers a variety of feeds for use by the aviation industry. Aircraft for sale listings are powered by <a href="http://www.jetnetglobal.com/">JETNET Global</a>. Aircraft dealers interested in advertising their aircraft for sale should contact us about dealer listings.
  </p>
  <p>
    To receive JETNET Global feeds in your RSS application:</p>
  <ul>
    <li>&bull;&nbsp;Click on any link below </li>
    <li>&bull;&nbsp;(You can also right-click or option-click for a shortcut.) </li>
    <li>&bull;&nbsp;Copy the URL in the address bar of your browser (or from your clipboard) and paste
      it into your RSS reader. </li>
  </ul>
  <hr />
  <a href="abiNewsRss.aspx">&raquo;&nbsp;JETNET News</a><br /><br />
  <a href="aviationrss.aspx">&raquo;&nbsp;Latest Aircraft for Sale</a><br />
  <a href="rss-jets.aspx">&raquo;&nbsp;Latest Jets for Sale</a><br />
  <a href="rss-helicopter.aspx">&raquo;&nbsp;Latest Helicopters for Sale</a><br />
  <a href="rss-piston.aspx">&raquo;&nbsp;Latest Pistons for Sale</a><br />
  <a href="rss-turbine.aspx">&raquo;&nbsp;Latest Turbines for Sale</a><br />
</span>
</div>
    </section>
    </main>
  </div>
</asp:Content>
