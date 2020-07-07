<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="WelcomeUser.ascx.vb"
    Inherits="crmWebClient.WelcomeUser" %>
<asp:Literal runat="server" ID="crmEvoModeScriptsToggle" Visible="false">
  <link rel="stylesheet" type="text/css" href="common/anylinkmenu.css" />
    <script type="text/javascript" src="../common/anylinkmenu.js">
      /***********************************************
      * AnyLink JS Drop Down Menu v2.0- © Dynamic Drive DHTML code library (www.dynamicdrive.com)
      * This notice MUST stay intact for legal use
      * Visit Project Page at http://www.dynamicdrive.com/dynamicindex1/dropmenuindex.htm for full source code
      ***********************************************/
  </script>
  <script>
      anylinkmenu.init("menuanchorclass");
      var anylinkmenu_CRMEVO = { divclass: 'anylinkmenu anylinkmenulower', inlinestyle: '', linktarget: '' }
      anylinkmenu_CRMEVO.items = [
          ["New Company", "javascript:load('/edit.aspx?action=new&type=company&Listing=1&from=homePage','','scrollbars=yes,menubar=no,height=900,width=940,resizable=yes,toolbar=no,location=no,status=no');"],
          ["Quick Contact Entry", "javascript:load('edit.aspx?action=quick&from=homePage','','scrollbars=yes,menubar=no,height=700,width=960,resizable=yes,toolbar=no,location=no,status=no');"]
      ]
  </script>
</asp:Literal>
<!--[if IE 7]>
<style type="text/css">
.welcome_user_class_ie{font-size:15px !important;font-weight:normal;}
</style>
<![endif]-->
<style>
    .searchBox {
        float: right;
        margin-top: -2px;
    }

    .headerHeight .searchBox {
        margin-top: 15px;
        margin-bottom: -28px;
        margin-right: 5px;
        position: relative;
        z-index: 10000
    }

    .margin_larger {
        margin-top: -17px;
    }

    .searchBox .searchIcon {
        line-height: 13px; /*margin: 2px 7px 2px 8px;*/
        font-size: 11px;
        font-weight: normal;
        font-family: 'FontAwesome';
        padding: 6px;
        position: absolute;
        z-index: 1000;
        right: 4px;
        margin-top: 3px;
        width: 24px;
    }

    .headerHeight .searchBox .searchIcon {
        margin-right: 1px;
    }

    .searchBox input[type="text"] {
        padding: 5px !important;
        margin-top: 2px !important;
        font-family: 'Noto Sans JP', sans-serif;
    }

    @media (max-width: 1400px) {
        .headerHeight .searchBox {
            margin-bottom: 0px !important;
        }
    }

    .dropdownSettings {
        position: relative;
    }



    .headerHeight .welcome_text .dropdown-content li a:hover {
        text-decoration: underline !important;
    }

    .welcome_text .dropdown-content li a:before {
        padding-left: 0px;
        padding-right: 0px;
        content: ''
    }

    .linkLock {
        position: relative;
        top: 5px;
        padding-right: 4px;
        font-size: 18px;
        width: 18px;
    }
</style>
<div class="sixteen columns headerHeight" id="welcomeContainer" runat="server">
    <div class="one-third column mobileAlignCenter">
        <a href="/home.aspx" runat="server" id="homePageLink">
            <asp:Image ID="logo" ImageUrl="images/logo.png" runat="server" CssClass="evolution_logo"
                border="0" /></a>
    </div>
    <div class="three columns" runat="server" id="logoTextClass">
        <asp:Label ID="CRM_Logo_Text" runat="server"></asp:Label>
    </div>
    <div class="seven columns float_right main_repeat remove_margin" runat="server" id="toggleColumnsWidth">
        <div id="welcome_message" class="welcome_text" runat="server">
            <span runat="server" id="displayMobileMenuClass">
                <span id="toggleStandaloneButtons" runat="server">
                    <div class="dropdownSettings">
                        <asp:LinkButton ID="myPreferences_link" runat="server" OnClientClick="javascript:load('myCRM.aspx','','scrollbars=yes,menubar=no,height=800,width=800,resizable=yes,toolbar=no,location=no,status=no');return false;"
                            class="myCRM_login noBefore"><img src="images/settings.svg" alt="Settings" /></asp:LinkButton>
                        <div class="dropdown-content dropdown-content" style="right: 0px;">
                            <div class="row">
                                <div class="four columns">
                                    <strong>SETTINGS</strong>

                                    <ul>
                                        <li><a href="#" onclick="javascript:window.open('/Preferences.aspx');return false;" class="displayNoneMobile">Account</a></li>
                                        <li><a href="#" onclick="javascript:window.open('/Preferences.aspx?selected=display');return false;" class="displayNoneMobile">Display</a></li>
                                        <li><a href="#" onclick="javascript:window.open('/Preferences.aspx?selected=services');return false;"  class="displayNoneMobile">Services</a></li>
                                        <li><a href="#" onclick="javascript:window.open('/Preferences.aspx?selected=notes');return false;" class="displayNoneMobile">Notes</a></li>
                                        <li><a href="#" onclick="javascript:window.open('/Preferences.aspx?selected=dashboard');return false;" class="displayNoneMobile yachtHide">Homepage</a></li>
                                    </ul>
                                </div>
                                <div class="four columns yachtHide">
                                    <strong>LISTS</strong>
                                    <ul>
                                        <li><a href="#" onclick="javascript:window.open('/Preferences.aspx?selected=folders');return false;" class="displayNoneMobile">My Folders</a></li>
                                        <li><a href="#" onclick="javascript:window.open('/Preferences.aspx?selected=templates');return false;" class="displayNoneMobile">My Templates</a></li>
                                        <li><a href="#" onclick="javascript:window.open('/Preferences.aspx?selected=models');return false;" class="displayNoneMobile">My Models</a></li>
                                        <li><a href="#" onclick="javascript:window.open('/Preferences.aspx?selected=airports');return false;" class="displayNoneMobile">My Airports</a></li>
                                    </ul>
                                </div>
                                <div class="four columns">
                                    <strong>ACTIONS</strong>
                                    <ul>
                                        <li><a href="#" onclick="javascript:window.open('/Preferences.aspx?selected=support');return false;" class="displayNoneMobile" >Support/Feedback</a></li>
                                        <li>
                                            <asp:LinkButton runat="server" ID="logoutButton"><img src="images/lock.svg" alt="Lock" width="18" class="padding_right" />Sign out</asp:LinkButton></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:LinkButton ID="auto_evolution_button" runat="server" CssClass="float_right noBefore yacht_button yachtsitePadding"
                        Visible="false" OnClientClick="document.body.style.cursor='wait';changeCursor(this);"
                        ToolTip="Login to Evolution">Login to Evolution</asp:LinkButton>
                    <asp:UpdatePanel runat="server" ID="yachtSideOpenerUpdatePanel" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:LinkButton ID="yachtSideOpener" runat="server" CssClass="float_right noBefore yacht_button yachtsitePadding"
                                Visible="false" ToolTip="Login to Evolution"><img src="/images/plane_icon.png" border="0" alt="Login to Evolution" title="Login to Evolution"/></asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Label ID="login_to_evolution" runat="server"><a href="https://www.jetnetevolution.com " target="new" class="myCRM_login" title="Login to Evolution"><strong>Login to Evolution</strong></a></asp:Label>
                    <asp:LinkButton ID="yacht_button" runat="server" CssClass="float_right yacht_button cursor" ToolTip="YachtSpot" Visible="false" OnClientClick="document.body.style.cursor='wait';changeCursor(this);"><img src="images/yacht_icon.png" border="0" alt="Login to Yacht Spot" title="Login to Yacht Spot" class="yachtIcon" /></asp:LinkButton>
                    <asp:UpdatePanel runat="server" ID="evoSideOpenerUpdatePanel" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:LinkButton ID="evoSideOpener" runat="server" CssClass="float_right yacht_button cursor"
                                Visible="false" title="Login to Yacht Spot" ToolTip="YachtSpot"><img src="images/yacht_icon.png" border="0" alt="Login to Yacht Spot" title="Login to Yacht Spot"  class="yachtIcon"/></asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                </span>
                <asp:LinkButton ID="close_button" runat="server" CssClass="noBefore closeButtonTopRight" Visible="false"
                    OnClientClick="javascript:window.close();return false;"><img src="images/x.svg" alt="Close" /></asp:LinkButton>
                <asp:Label
                    runat="server" ID="extraButtons" CssClass="display_none"></asp:Label>
                <a ID="helpEvo" runat="server" class="helpEvoButton help_cursor" title="Help" href="/help.aspx?t=2" target="_blank"><img src="images/help-circle.svg" alt="Help" /></a>

                <div class="dropdownSettings-sub">
                    <asp:LinkButton runat="server" ID="crmEvoEditMenu" Visible="false" class="myCRM_login"><img src="images/edit.svg" alt="Help" /></asp:LinkButton>
                    <div class="dropdown-content-sub" style="right: 0px;">
                        <div class="row">
                            <div class="twelve columns">
                                <ul>
                                    <li><a href="#" class="displayNoneMobile" onclick="javascript:window.open('/edit.aspx?action=new&type=company&Listing=1&from=homePage');">New Company</a></li>
                                    <li><a href="#" class="displayNoneMobile" onclick="javascript:window.open('/edit.aspx?action=quick&from=homePage');">Quick Contact Entry</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>


                <a Visible="false" runat="server" ID="EvoAlertMenu" title="Alerts" class="float_right noBefore emphasisAlert" href="/userScheduledJobs.aspx" target="_blank"><img src="images/bell.svg" alt="Alerts" /></a>
            </span><a href="/home.aspx" class="fa fa-home float_right" id="homeButton" runat="server"
                visible="false"></a><span id="searchPopup" class="searchPopupButton" runat="server"
                    visible="false">&#xf002;</span>
            <asp:UpdatePanel runat="server" ID="searchCriteriaUpdate" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Label runat="server" ID="searchCriteriaToggle" CssClass="searchCriteria slideoutToolTip"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:Label runat="server" ID="searchPanelSlideOut" Visible="false" CssClass="searchPanelPopout"><strong>Search</strong></asp:Label>
            <asp:Label ID="welcome_user" runat="server" CssClass="welcome_user_class_ie"></asp:Label>
            <div class="div_clear">
            </div>
        </div>
        <asp:Label ID="break_toggle_evo" runat="server"><br clear="all" /></asp:Label>
        <asp:Panel ID="searchBoxVisible" CssClass="searchBox" runat="server" Visible="false"
            DefaultButton="searchIcon">
            <asp:TextBox ID="searchBoxText" runat="server" placeholder="Search" CssClass="tooltip"></asp:TextBox><asp:Button
                runat="server" ID="searchIcon" CssClass="searchIcon" Text="" OnClientClick="RunSearch();return false;" />
            <img src="images/search.svg" class="searchIcon" onclick="RunSearch();return false;"/>
        </asp:Panel>
        <div id='message' style="display: none;">
            <span class="message_span">
                <p>
                    <asp:Label ID="evo_message_text" runat="server"></asp:Label>
                </p>
            </span>
        </div>
    </div>
</div>
<asp:TextBox runat="server" ID="isMobileVersion" CssClass="display_none"></asp:TextBox>
<div id="belowWelcomeContainer" runat="server" class="headerHeightPadding">
</div>
<asp:Panel runat="server" Visible="false" ID="modalPopupsEvo">
    <div id="yachtSidedialog" title="" style="display: none;">
        <p>
            JETNET LLC offers a companion service for the Aviation community named <b>JETNET Evolution</b>.
        </p>
        <p>
            To view additional information regarding aviation related services, email <a href="mailto:customerservice@jetnet.com"
                class="blue_text">customerservice@jetnet.com</a> or JETNET at 1-800-553-8638 for
      information regarding a JETNET Evolution subscription.
        </p>
        <p class="tiny_text float_left padding_top">
            Copyright &copy; 2000-<%=Year(Now())%>. JETNET LLC All rights reserved.
        </p>
        <img src="/images/JN_EvolutionMarketplace_Logo2.png" width="160" class="float_right" />
    </div>
    <div id="evoSidedialog" style="display: none;">
        <p>
            JETNET LLC offers a companion service for the Yacht community named <b>YachtSpot</b>.
        </p>
        <p>
            To view additional information regarding yacht related services, email <a href="mailto:customerservice@jetnet.com"
                class="blue_text">customerservice@jetnet.com</a> or JETNET at 1-800-553-8638 for
      information regarding a JETNET Yacht Spot subscription.
        </p>
        <p class="tiny_text float_left padding_top">
            Copyright &copy; 2000-<%=Year(Now())%>. JETNET LLC All rights reserved.
        </p>
        <img src="/images/JN_EvolutionMarketplace_Logo2.png" width="160" class="float_right" />
    </div>
    <asp:Literal runat="server" ID="includeJqueryTheme"></asp:Literal>
</asp:Panel>

<script>
    function changeCursor(link) {
        link.className += " cursor_wait";
    }


    function RunSearch() {
        if ($("#<%=searchBoxText.clientID %>").val() != '') {
            callQuickHeaderSearch($("#<%=searchBoxText.clientID %>").val(), $("#<%=isMobileVersion.clientID %>").val());
        }
    }


</script>

