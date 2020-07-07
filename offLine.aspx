<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="offLine.aspx.vb" Inherits="crmWebClient.offLine" %>

<!DOCTYPE html>
<!--[if lt IE 7 ]><html class="ie ie6" lang="en"><![endif]-->
<!--[if IE 7 ]><html class="ie ie7" lang="en"> <![endif]-->
<!--[if IE 8 ]><html class="ie ie8" lang="en"> <![endif]-->
<!--[if (gte IE 9)|!(IE)]><!-->
<html lang="en">
<!--<![endif]-->
<head id="head_tag" runat="server">
  <title>Welcome to Marketplace Manager - Web</title>
  <asp:Label runat="server" ID="Evo_Styles">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <!-- CSS ================================================== -->
    <!--Created Stylesheet-->
    <link href="EvoStyles/stylesheets/additional_styles.css" rel="stylesheet" type="text/css" />
    <!--Grid/Layout Styles-->
    <link href="EvoStyles/stylesheets/layout/base_html_elements.css" rel="stylesheet" type="text/css" />
    <!--[if lt IE 9]>
		    <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script> <![endif]-->
    <!-- Favicons ================================================== -->
    <link rel="shortcut icon" href="images/favicon.ico" />
    <link rel="apple-touch-icon" href="/EvoStyles/images/apple-touch-icon.png" />
    <link rel="apple-touch-icon" sizes="72x72" href="/EvoStyles/images/apple-touch-icon-72x72.png" />
    <link rel="apple-touch-icon" sizes="114x114" href="/EvoStyles/images/apple-touch-icon-114x114.png" />
  </asp:Label>
  <!--both-->
  <link href="EvoStyles/stylesheets/layout/skeleton_grid.css" rel="stylesheet" type="text/css" />
</head>
<body>
  <asp:Image ImageUrl="~/images/background/10.jpg" ID="background_image" CssClass="bg_image"
    runat="server" />
  <form id="form1" runat="server">
    <div class="container">
      <div class="sixteen columns">
        <div class="one-third column">
          <asp:Image ID="logo" ImageUrl="images/JN_EvolutionMarketplace_Logo2.png" runat="server"
            Width="250" CssClass="evolution_logo" />
        </div>
        <div class="six columns">
        </div>
      </div>
      <div class="sixteen columns">
        <div class="two-thirds column login_white_page">
          <asp:Panel runat="server" ID="regular_page_information">
            <asp:PlaceHolder runat="server" ID="PlaceHolder1" />
            <div class="login_page_blue_bar content_padding">
              <asp:Label runat="server" ID="logon_add_info_lbl" Font-Bold="true" ForeColor="#efefef"></asp:Label>
            </div>
            <div class="content_padding">
              <asp:Label ID="welcome_to_text" runat="server" CssClass="login_h1" Text="JETNET Online is temporarily down for maintenance!"></asp:Label>
              <p>
                <asp:Label ID="welcome_paragraph" runat="server"></asp:Label>
              </p>
            </div>
          </asp:Panel>

          <div class="login_page_blue_bar_bottom">&nbsp;</div>
        </div>
      </div>
    </div>
  </form>
</body>
</html>
