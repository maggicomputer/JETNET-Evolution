<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="crmWebClient._Default_aspx"
  EnableViewState="false" %>

<!DOCTYPE html>
<!--[if lt IE 7 ]><html class="ie ie6" lang="en"><![endif]-->
<!--[if IE 7 ]><html class="ie ie7" lang="en"> <![endif]-->
<!--[if IE 8 ]><html class="ie ie8" lang="en"> <![endif]-->
<!--[if (gte IE 9)|!(IE)]><!-->
<html lang="en">
<!--<![endif]--> 
<head id="head_tag" runat="server">
  <title>Welcome to Marketplace Manager - Web</title>
  <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
  <link href="EvoStyles/stylesheets/layout/skeleton_grid.css" rel="stylesheet" type="text/css" />
  <!--Grid/Layout Styles-->
  <link href="EvoStyles/stylesheets/layout/base_html_elements.css" rel="stylesheet" type="text/css" />

  <asp:Literal runat="server" ID="hotjarScriptTestEvo" Visible="false">
    <!-- Hotjar Tracking Code for https://www.testjetnetevolution.com/ -->
    <script type="text/javascript">
      (function (h, o, t, j, a, r) {
        h.hj = h.hj || function () { (h.hj.q = h.hj.q || []).push(arguments) };
        h._hjSettings = { hjid: 1433190, hjsv: 6 };
        a = o.getElementsByTagName('head')[0];
        r = o.createElement('script'); r.async = 1;
        r.src = t + h._hjSettings.hjid + j + h._hjSettings.hjsv;
        a.appendChild(r);
      })(window, document, 'https://static.hotjar.com/c/hotjar-', '.js?sv=');
    </script>
  </asp:Literal>

  <asp:Literal runat="server" ID="hotjarScriptLiveEvo" Visible="false">
    <!-- Hotjar Tracking Code for https://www.jetnetevolution.com/ -->
    <script type="text/javascript">
      (function (h, o, t, j, a, r) {
        h.hj = h.hj || function () { (h.hj.q = h.hj.q || []).push(arguments) };
        h._hjSettings = { hjid: 1419295, hjsv: 6 };
        a = o.getElementsByTagName('head')[0];
        r = o.createElement('script'); r.async = 1;
        r.src = t + h._hjSettings.hjid + j + h._hjSettings.hjsv;
        a.appendChild(r);
      })(window, document, 'https://static.hotjar.com/c/hotjar-', '.js?sv=');
    </script>      
  </asp:Literal>

  <asp:Label runat="server" ID="CRM_Styles" Visible="false">
    <link href="common/redesign.css" rel="stylesheet" type="text/css" />
    <meta name="robots" content="noindex, nofollow" />
    <meta http-equiv="refresh" content="600" />
    <asp:Literal runat="server" ID="mobile_resize" Visible="false">
      <meta name="viewport" content="width=320">
    </asp:Literal>
    <link rel="stylesheet" media="all and (min-width: 768px) and (orientation:portrait)" href="common/ipad-portrait.css" />
    <link rel="stylesheet" media="all and (min-width: 768px) and (orientation:landscape)" href="common/ipad-landscape.css" />
    <link rel="stylesheet" media="all and (min-width: 992px)" href="common/regular.css" />
  </asp:Label>

  <asp:Label runat="server" ID="Evo_Styles" Visible="false">
    <link rel="shortcut icon" href="/images/favicon.ico" />
    <link rel="apple-touch-icon" href="/EvoStyles/images/apple-touch-icon.png?v=2" />
    <link rel="apple-touch-icon" sizes="72x72" href="/EvoStyles/images/apple-touch-icon-72x72.png?v=2" />
    <link rel="apple-touch-icon" sizes="114x114" href="/EvoStyles/images/apple-touch-icon-114x114.png?v=2" />
  </asp:Label>

</head>
<body class="loginPage">
  <script type="text/javascript">

    var bIsTest = <%= bIsTestSite.ToString.Tolower %>;
    var bIsMobile = false;

    var d, dom, nu = '', brow = '', ie, ie4, ie5, ie5x, ie6, ie7, ie8, ie9, ie10, ie11;
    var ns4, moz, moz_rv_sub, release_date = '', moz_brow, moz_brow_nu = '', moz_brow_nu_sub = '', rv_full = '';
    var mac, win, old, lin, ie5mac, ie5xwin, konq, saf, op, op4, op5, op6, op7;
    var ipad, ipod, iphone, droid, edge, chrome;

    function findBrowserTypeJS() {

      d = document;
      n = navigator;
      nav = n.appVersion;
      nan = n.appName;
      nua = n.userAgent;

      mac = false;
      win = false;
      lin = false;

      op = false;
      konq = false;
      saf = false;
      moz = false;
      chrome = false;
      msie = false;

      ipad = false;
      ipod = false;
      iphone = false;
      droid = false;

      ie = false;
      edge = false;

      old = (nav.substring(0, 1) < 4);
      mac = (nav.indexOf('Mac') != -1);
      win = (((nav.indexOf('Win') != -1) || (nav.indexOf('NT') != -1)) && !mac) ? true : false;
      lin = (nua.indexOf('Linux') != -1);

      ipad = (nua.toLowerCase().indexOf('ipad') != -1);
      ipod = (nua.toLowerCase().indexOf('ipod') != -1);
      iphone = (nua.toLowerCase().indexOf('iphone') != -1);
      droid = (nua.toLowerCase().indexOf('android') != -1);

      // begin primary dom/ns4 test
      // this is the most important test on the page
      if (!document.layers) {
        dom = (d.getElementById) ? d.getElementById : false;
      }
      else {
        dom = false;
        ns4 = true; // only netscape 4 supports document layers
      }
      // end main dom/ns4 test

      edge = (nua.indexOf('Edge') != -1);
      chrome = (nua.indexOf("Chrome") != -1 && !edge);
      op = (nua.indexOf('Opera') != -1);
      saf = ((!chrome && !edge) && nua.indexOf('Safari') != -1);
      konq = (!saf && (nua.indexOf('Konqueror') != -1)) ? true : false;
      moz = ((!saf && !konq && !chrome && !edge) && (nua.indexOf('Gecko') != -1)) ? true : false;
      ie = (nua.indexOf('MSIE') != -1 || nua.indexOf('rv:11.0') != -1);

      if (op) {
        str_pos = nua.indexOf('Opera');
        nu = nua.substr((str_pos + 6), 4);
        brow = 'Opera';
      }
      else if (saf) {
        str_pos = nua.indexOf('Safari');
        nu = nua.substr((str_pos + 7), 5);
        brow = 'Safari';
      }
      else if (konq) {
        str_pos = nua.indexOf('Konqueror');
        nu = nua.substr((str_pos + 10), 3);
        brow = 'Konqueror';
      }
      // this part is complicated a bit, don't mess with it unless you understand regular expressions
      // note, for most comparisons that are practical, compare the 3 digit rv nubmer, that is the output
      // placed into 'nu'.
      else if (moz) {
        // regular expression pattern that will be used to extract main version/rv numbers
        pattern = /[(); \n]/;
        // moz type array, add to this if you need to
        moz_types = new Array('Firebird', 'Phoenix', 'Firefox', 'Iceweasel', 'Galeon', 'K-Meleon', 'Camino', 'Epiphany', 'Netscape6', 'Netscape', 'MultiZilla', 'Gecko Debian', 'rv');
        rv_pos = nua.indexOf('rv'); // find 'rv' position in nua string
        rv_full = nua.substr(rv_pos + 3, 6); // cut out maximum size it can be, eg: 1.8a2, 1.0.0 etc
        // search for occurance of any of characters in pattern, if found get position of that character
        rv_slice = (rv_full.search(pattern) != -1) ? rv_full.search(pattern) : '';
        //check to make sure there was a result, if not do  nothing
        // otherwise slice out the part that you want if there is a slice position
        (rv_slice) ? rv_full = rv_full.substr(0, rv_slice) : '';
        // this is the working id number, 3 digits, you'd use this for 
        // number comparison, like if nu >= 1.3 do something
        nu = rv_full.substr(0, 3);
        for (i = 0; i < moz_types.length; i++) {
          if (nua.indexOf(moz_types[i]) != -1) {
            moz_brow = moz_types[i];
            break;
          }
        }
        if (moz_brow)// if it was found in the array
        {
          str_pos = nua.indexOf(moz_brow); // extract string position
          moz_brow_nu = nua.substr((str_pos + moz_brow.length + 1), 3); // slice out working number, 3 digit
          // if you got it, use it, else use nu
          moz_brow_nu = (isNaN(moz_brow_nu)) ? moz_brow_nu = nu : moz_brow_nu;
          moz_brow_nu_sub = nua.substr((str_pos + moz_brow.length + 1), 8);
          // this makes sure that it's only the id number
          sub_nu_slice = (moz_brow_nu_sub.search(pattern) != -1) ? moz_brow_nu_sub.search(pattern) : '';
          //check to make sure there was a result, if not do  nothing
          (sub_nu_slice) ? moz_brow_nu_sub = moz_brow_nu_sub.substr(0, sub_nu_slice) : '';
        }
        if (moz_brow == 'Netscape6') {
          moz_brow = 'Netscape';
        }
        else if (moz_brow == 'rv' || moz_brow == '')// default value if no other gecko name fit
        {
          moz_brow = 'Mozilla';
        }
        if (!moz_brow_nu)// use rv number if nothing else is available
        {
          moz_brow_nu = nu;
          moz_brow_nu_sub = nu;
        }
        if (n.productSub) {
          release_date = n.productSub;
        }
      }
      else if (ie) {
        str_pos = nua.indexOf('MSIE');
        nu = nua.substr((str_pos + 5), 3);
        str_pos = nua.indexOf('rv');
        nu = nua.substr((str_pos + 3), 3);
        brow = 'Microsoft Internet Explorer';
      }
      // default to navigator app name
      else {
        brow = nan;
      }

      op5 = (op && (nu.substring(0, 1) == 5));
      op6 = (op && (nu.substring(0, 1) == 6));
      op7 = (op && (nu.substring(0, 1) == 7));
      op8 = (op && (nu.substring(0, 1) == 8));
      op9 = (op && (nu.substring(0, 1) == 9));

      ie4 = (ie && !dom);
      ie5 = (ie && (nu.substring(0, 1) == 5));
      ie6 = (ie && (nu.substring(0, 1) == 6));
      ie7 = (ie && (nu.substring(0, 1) == 7));
      ie8 = (ie && (nu.substring(0, 1) == 8));
      ie9 = (ie && (nu.substring(0, 1) == 9));
      ie10 = (ie && (nu.substring(0, 1) == 10));
      ie11 = (ie && (nu.substring(0, 1) == 11));

      // default to get number from navigator app version.
      if (!nu) {
        nu = nav.substring(0, 1);
      }

      /*ie5x tests only for functionavlity. dom or ie5x would be default settings. 
      Opera will register true in this test if set to identify as IE 5*/

      ie5x = (d.all && dom);
      ie5mac = (mac && ie5);
      ie5xwin = (win && ie5x);

      try {

        //alert("[n.appVersion] " + nav + "\n[n.userAgent] " + nua + "\n[n.appName] " + nan);

        //alert("[brow] " + brow + "\n[nu] " + nu);       

        //alert("[os = old " + old + "] [os = win " + win + "] [os = mac " + mac + "] [os = lin " + lin + "]\n[os = ipad/ipod " + ipad + "/" + ipod + "] [os = iphone " + iphone + "] [os = android " + droid + "]");

        //alert("[br = op " + op + "] [br = saf " + saf + "] [br = konq " + konq + "]\n[br = moz " + moz + "] [br = chrome " + chrome + "] [br = ie " + ie + "] [br = edge " + edge + "]");

      } catch (err) { }

      return true;
    }

    function updateServerWithBrowserTypeJS() {

      var osString = "other  ";
      var brString = "unknown  ";

      var tmpString = "";

      // os type  
      if (win) {
        osString = "win    ";
      }

      if (mac) {
        osString = "mac    ";
      }

      if (lin) {
        osString = "linux  ";
      }

      if (ipad) {
        osString = "ipad   ";
      }

      if (ipod) {
        osString = "ipod   ";
      }

      if (iphone) {
        osString = "iphone ";
      }

      if (droid) {
        osString = "droid  ";
      }

      // browser type
      if (op) {
        brString = "opera    ";
      }

      if (konq) {
        brString = "konqueror";
      }

      if (saf) {
        brString = "safari   ";
      }

      if (moz) {
        brString = "firefox  ";
      }

      if (chrome) {
        brString = "chrome   ";
      }

      if (ie) {
        brString = "msie     ";
      }

      if (edge) {
        brString = "edge     ";
      }

      tmpString = osString + brString;

      if (tmpString == "") {
        tmpString = "other  unknown  "
      }

      if (ipod || iphone || droid) {
        bIsMobile = true;
      }

      document.getElementById("whatBrowserID").value = tmpString;

      //alert("os / browser : " + document.getElementById("whatBrowserID").value);

      return true;

    }

    function redirectToMobileSite() {

      var r = confirm(" We have detected that you are using a mobile device. Would you like to switch to the mobile version? ");

      if (r) {

        if (bIsTest) {
          window.location = "http://www.testjetnetmobile.com";
        } else {
          window.location = "https://www.jetnetevomobile.com";
        }

      }

      return true;

    }

  </script>

  <div class="FixedHeaderBar" runat="server" id="fixedBar" visible="false">
  </div>
  <asp:Label ID="debugTextLbl" runat="server" Visible="false"></asp:Label>
  <asp:Image ID="background_image" CssClass="bg_image" runat="server" />
  <form id="form1" runat="server" submitdisabledcontrols="true">
    <input type="hidden" name="whatBrowser" value="" id="whatBrowserID" />
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true" AsyncPostBackTimeout="8000">
    </cc1:ToolkitScriptManager>
    <div class="one-third column mobileAlignCenter loginHeader">
      <asp:Image ID="logo" ImageUrl="images/logo.png" runat="server" CssClass="evolution_logo home" />
    </div>
    <div class="container">
      <div class="sixteen columns headerHeight" runat="server" id="header_div">
        <div class="six columns">
          <asp:Label ID="CRM_Logo_Text" runat="server"></asp:Label>
        </div>
      </div>
      <div id="belowWelcomeContainer" runat="server" class="headerHeightPadding home">
      </div>
      <div class="sixteen columns">
        <asp:Label runat="server" ID="folder_load" CssClass="display_none">
          <div id="Div2" runat="server" class="loadingScreenComparable">
            <span>Please wait while your Default Folder is Loading... </span>
            <br />
            <br />
            <img src="Images/loading.gif" alt="Loading..." /><br />
          </div>
        </asp:Label>
        <div class="two-thirds column login_white_page toggleSmallScreen">
          <asp:Panel runat="server" ID="regular_page_information">
            <div class="login_page_blue_bar">
              <asp:Label runat="server" ID="logon_add_info_lbl" Font-Bold="true" ForeColor="#efefef"></asp:Label>
            </div>
            <div class="content_padding">
              <asp:Label ID="welcome_to_text" runat="server" CssClass="login_h1"></asp:Label>
              <asp:Label ID="welcome_paragraph" runat="server"></asp:Label>
            </div>
          </asp:Panel>
          <div class="login_page_blue_bar_bottom">
            &nbsp;
          </div>
        </div>
        <div class="four columns login_white_page fullWidthMobile loginForm" style="width: 25%;">
          <div class="login_page_blue_bar">
            &nbsp;
          </div>
          <div class="content_padding">
            <asp:Label ID="lbl_inactive" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
            <asp:Panel runat="server" ID="mobile_page_information" Visible="false">
              <h4>WELCOME TO JETNET CRM</h4>
            </asp:Panel>
            <crm:loginCrmUser ID="logonUser" runat="server" Visible="false" />
            <crm:validateCrmUser ID="validateUser" runat="server" Visible="false" />
          </div>
          <div class="login_page_blue_bar_bottom">
            &nbsp;
          </div>
        </div>
        <div id="evoNotificationdialog" style="display: none;">
          <asp:Label runat="server" ID="notificationText"></asp:Label>
          <p class="tiny_text float_left padding_top copyR display_none">
            Copyright &copy; 2000-<%=Year(Now())%>. JETNET LLC All rights reserved.
          </p>
          <img src="/images/JN_EvolutionMarketplace_Logo2.png" width="160" class="float_right padding display_none" />
        </div>
        <asp:Literal runat="server" ID="includeJqueryTheme"></asp:Literal>
        <cc1:ModalPopupExtender ID="MPE" runat="server" TargetControlID="error_logged_on_users"
          PopupControlID="max_user_warning" BackgroundCssClass="modalBackground" DropShadow="true"
          CancelControlID="CancelButton" RepositionMode="None" />
        <asp:Panel ID="max_user_warning" runat="server" Style="display: none">
          <p style="text-align:left;">
            We have detected that you are currently logged in via another session.
          </p>
          <p style="font-style: italic; text-align:left;">
            <span style="color: #ff0000;">*</span>Please note: This may have happened because
          you failed to logout the last time you completed your session.
          </p>
          <p>
            Would you like to terminate your previous session and login? (Cancel to leave your
          previous session active)
          </p>
          <div style="padding-top: 4px; padding-right: 4px; text-align:center;">
            <asp:Button ID="OkButton" runat="server" Text="OK" BackColor="LightBlue" OnClientClick="hidePreviousBox();" />
            <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
          </div>
        </asp:Panel>
        <asp:Button ID="error_logged_on_users" runat="server" Text="Button" Style="display: none;" />
        <cc1:ModalPopupExtender ID="MPE1" runat="server" TargetControlID="acceptEula" PopupControlID="eulaAgreement"
          BackgroundCssClass="modalBackground" DropShadow="true" RepositionMode="None" />
        <asp:Panel ID="eulaAgreement" runat="server" HorizontalAlign="Center" Style="display: none">
          <div class="eulaAgreementPopup">
            <p style="font-size: 16px; font-weight: bold; padding: 2px; color: black; text-align: center;">
              JETNET End User License Agreement
            </p>
            <asp:Label ID="eulaText" runat="server" EnableViewState="False" Text="eula"
              Font-Size="8" BackColor="WhiteSmoke" Style="padding: 4px; width: 98%; overflow: auto; text-align: left; border: 2px solid black;"></asp:Label>
            <div style="padding-top: 4px; padding-right: 4px; text-align:center;">
              <asp:Button ID="btnAccept" runat="server" Text="Accept" BackColor="LightBlue" OnClientClick="hideEulaBox();" />
              <asp:Button ID="btnDecline" runat="server" Text="Decline" OnClientClick="hideEulaBox();" />
            </div>
          </div>
        </asp:Panel>
        <asp:Button ID="acceptEula" runat="server" Text="Button" Style="display: none;" />
      </div>
      <div class="sixteen columns calendar displayNoneMobile" runat="server" id="evoCalendar" visible="false">
        <div class="four columns">
          <h3>What's Happening</h3>
        </div>
        <div class="eight columns">
          <p>Mark your calendar for upcoming JETNET training courses and events!</p>
          <img src="images/calendar_picture.png" id="calendarPopup" alt="Recent Events at JETNET" width="250px" />
          <asp:Label ID="current_jetnet_events" runat="server" Width="96%" Style="display: none; position: relative; z-index: 10000;"></asp:Label>
        </div>
      </div>
    </div>
    <div class="sixteen columns footerCopyright">
      <p>
        Copyright &copy; 2000 -
        <%= Now().Year.ToString()%>
        JETNET LLC all rights reserved <a href="https://www.jetnet.com" target="new">www.JETNET.com</a>
      </p>
    </div>
  </form>

  <script type="text/javascript">

    function hidePreviousBox() {
      var obj = document.getElementById("<%= max_user_warning.clientID %>")
      if ((typeof (obj) != "undefined") && (obj != null)) {
        obj.style.display = 'none'
      }
    }

    function hideEulaBox() {
      var obj = document.getElementById("<%= eulaAgreement.clientID %>")
      if ((typeof (obj) != "undefined") && (obj != null)) {
        obj.style.display = 'none'
      }
    }

    var rememberMe = document.getElementById("<%= logonUser.RememberMe.clientID %>");

    function AlertUser() {
      if ((typeof (rememberMe) != "undefined") && (rememberMe != null)) {
        if (rememberMe.checked == 1) {
          alert("This option should not be used from a public computer where your personal subscription could be compromised");
        }
      }
    }

    try {

      findBrowserTypeJS();

    } catch (err) { }

    updateServerWithBrowserTypeJS();

    if (bIsMobile && (window.location.href.indexOf('mobile') == -1)) {
      redirectToMobileSite();
    }

  </script>

</body>
</html>
