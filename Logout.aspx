<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Logout.aspx.vb" Inherits="crmWebClient.Logout" %>

<!DOCTYPE html>
<!--[if lt IE 7 ]><html class="ie ie6" lang="en"><![endif]-->
<!--[if IE 7 ]><html class="ie ie7" lang="en"> <![endif]-->
<!--[if IE 8 ]><html class="ie ie8" lang="en"> <![endif]-->
<!--[if (gte IE 9)|!(IE)]><!-->
<html lang="en">
<!--<![endif]-->
<head id="head_tag" runat="server">

  <title>Welcome to Marketplace Manager - Web</title>

  <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
  <!-- CSS
      ================================================== -->
  <!--Created Stylesheet-->
  <link href="EvoStyles/stylesheets/additional_styles.css" rel="stylesheet" type="text/css" />
  <!--Grid/Layout Styles-->
  <link href="EvoStyles/stylesheets/layout/base_html_elements.css" rel="stylesheet" type="text/css" />
  <!-- Header Alternate Styles-->
  <link href="EvoStyles/stylesheets/header_styles.css" rel="stylesheet" type="text/css" />

  <!-- Favicons ================================================== -->
  <link rel="shortcut icon" href="/images/favicon.ico" />
  <link rel="apple-touch-icon" href="/EvoStyles/images/apple-touch-icon.png?v=2" />
  <link rel="apple-touch-icon" sizes="72x72" href="/EvoStyles/images/apple-touch-icon-72x72.png?v=2" />
  <link rel="apple-touch-icon" sizes="114x114" href="/EvoStyles/images/apple-touch-icon-114x114.png?v=2" />

  <!--both-->
  <link href="EvoStyles/stylesheets/layout/skeleton_grid.css" rel="stylesheet" type="text/css" />

  <asp:Literal runat="server" ID="mobile_styles" Visible="false">
   <link href="EvoStyles/stylesheets/additional_mobile_styles.css" rel="stylesheet" type="text/css" />
  </asp:Literal>

</head>
<body>
  <div class="FixedHeaderBar" runat="server" id="fixedBar" visible="false">
  </div>
  <asp:Image ImageUrl="~/images/background/59.jpg" ID="background_image" CssClass="bg_image"
    runat="server" />
  <form id="form1" runat="server">
    <div class="container">
      <div class="sixteen columns headerHeight" runat="server" id="header_div">
        <div class="one-third column mobileAlignCenter">
          <asp:Image ID="logo" ImageUrl="images/logo.png" runat="server" CssClass="evolution_logo home" />
        </div>
        <div class="six columns">
          <asp:Label ID="CRM_Logo_Text" runat="server"></asp:Label>
        </div>
      </div>
      <div id="belowWelcomeContainer" runat="server" class="headerHeightPadding home">
      </div>
      <div class="sixteen columns">
        <div class="two-thirds column login_white_page">
          <asp:Panel runat="server" ID="regular_page_information">
            <asp:PlaceHolder runat="server" ID="PlaceHolder1" />
            <div class="login_page_blue_bar content_padding">
              <asp:Label runat="server" ID="logon_add_info_lbl" Font-Bold="true" ForeColor="#efefef"></asp:Label>
            </div>
            <div class="content_padding">
              <asp:Label ID="welcome_to_text" runat="server" CssClass="login_h1" Text="You have been logged out of JETNET Online"></asp:Label>
              <p>
                <asp:Label ID="welcome_paragraph" runat="server">Click <a href="default.aspx">here</a> to Log back on again! </asp:Label>
              </p>
            </div>
          </asp:Panel>
          <asp:Panel runat="server" Visible="false">
            <asp:TextBox runat="server" ID="decode_text"></asp:TextBox>
            <asp:Button runat="server" ID="decode" Text="decode" />
            <asp:Label runat="server" ID="decode_label"></asp:Label>
          </asp:Panel>
          <div class="login_page_blue_bar_bottom">
            &nbsp;
          </div>
        </div>
      </div>
    </div>
    </div>
  </form>
</body>
</html>
