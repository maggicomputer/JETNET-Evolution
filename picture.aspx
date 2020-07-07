<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="picture.aspx.vb" Inherits="crmWebClient.picture"
    MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" language='javascript'>
        var arrTemp = self.location.href.split("?");
        var picUrl = (arrTemp.length > 0) ? arrTemp[1] : "";
        var NS = (navigator.appName == "Netscape") ? true : false;

        function FitPic() {
            // iWidth = (NS)?window.innerWidth:document.body.clientWidth; 
            //iHeight = (NS)?window.innerHeight:document.body.clientHeight; 
            //           iWidth = document.images[0].width - iWidth + 100; 
            //           iHeight = document.images[0].height - iHeight + 100; 
            //if (iHeight < 500 && iWidth < 500) {
            //window.resizeTo(iWidth, iHeight);  
            //} else {
            window.resizeTo(1064, 1000);
            //}
            self.focus();
        };

        window.onload = function () {
            FitPic();
            LoadBackButtonIfExists();
        };

        function LoadBackButtonIfExists() {
            if ($("#back-top").length) {
                // hide #back-top first
                jQuery("#back-top").hide();

                // fade in #back-top
                jQuery(window).scroll(function () {
                    if (jQuery(this).scrollTop() > 100) {
                        jQuery('#back-top').fadeIn();
                    } else {
                        jQuery('#back-top').fadeOut();
                    }
                });


                // scroll body to 0px on click
                jQuery('#back-top a').click(function () {
                    jQuery('body,html').animate({
                        scrollTop: 0
                    }, 400);
                    return false;
                });
            }
        }
    </script>

    <link rel="stylesheet" href="Gallery/css/basic.css" type="text/css" />
    <link rel="stylesheet" href="Gallery/css/galleriffic-3.css" type="text/css" />
    <!-- We only want the thunbnails to display when javascript is disabled -->

    <script type="text/javascript">
        document.write('<style>.noscript { display: none; }</style>');
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label runat="server" ID="controls_buttons" Visible="false" CssClass="DetailsBrowseTable">
    <span class="backgroundShade"><a href="#" onclick="javascript:window.close();" class="float_right"><img src="/images/x.svg" alt="close" /></a>
        <div class="dropdownSettings-sub"><a href="javascript:void(0);"><img src="/images/menu.svg" alt="Menu" /></a>
         <div class="dropdown-content-sub">
            <div class="row">
                <div class="twelve columns">
                   <ul>
                       <li><a href="#" onclick="javascript:view('slide');">View Slideshow</a></li>
                       <li><a href="#" onclick="javascript:view('printer');">View Printer Friendly Version</a> </li>
                       <li><a href="#" onclick="javascript:view('all');">View All</a></li>
                   </ul>
                    </div>
                </div>
             </div>
            </div>
          </span>
    </asp:Label>
    <asp:Label ID="picture" runat="server" Text="" Style="display: none;"></asp:Label>
    <asp:Label ID="picture_plain" runat="server" Text=""></asp:Label>
    <asp:Label ID="javascript_text" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

    <script type="text/javascript">
        function view(ty) {
            switch (ty) {
                case "all":
                    $("#<%= picture.clientID %>").css({ "display": "none" });
                    $("#<%= picture_plain.clientID %>").css({ "display": "block" });
                    $(".bg_image").css({ "display": "block" });
                    $(".caption_text_large").css({ "width": "640px" });
                    $("#container img").css({ "width": "650px" });
                    $("div#page").removeClass("removingBorderPicturePage");
                    $(".standalone_page .white_background_color .contentContainer").css({ "border": "1px solid #b0b0b0" });
                    break;
                case "slide":
                    $("#<%= picture.clientID %>").css({ "display": "block" });
                    $("#<%= picture_plain.clientID %>").css({ "display": "none" });
                    $(".bg_image").css({ "display": "block" });
                    $(".caption_text_large").css({ "width": "640px" });
                    $("#container img").css({ "width": "100%" });
                    $("div#page").removeClass("removingBorderPicturePage");
                    $(".standalone_page .white_background_color .contentContainer").css({ "border": "1px solid #b0b0b0" });
                    break;

                case "printer":
                    $("#<%= picture.clientID %>").css({ "display": "none" });
                    $("#<%= picture_plain.clientID %>").css({ "display": "block" });
                    $(".bg_image").css({ "display": "none" });
                    $("#body").css({ "background": "white !important" });
                    $(".caption_text_large").css({ "width": "100%" });
                    $("#container img").css({ "width": "100%" });
                    $("div#page").addClass("removingBorderPicturePage");
                    $(".standalone_page .white_background_color .contentContainer").css({ "border": "0px solid #fff" });
                    break;
            }
        }
    </script>

    <script type="text/javascript" src="Gallery/js/jquery.history.js"></script>

    <script type="text/javascript" src="Gallery/js/jquery.galleriffic.js"></script>

    <script type="text/javascript" src="Gallery/js/jquery.opacityrollover.js"></script>

    <div id="back-top">
        <a href="#"><span></span></a>
    </div>
</asp:Content>
