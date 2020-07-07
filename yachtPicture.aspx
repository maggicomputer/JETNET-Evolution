<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="yachtPicture.aspx.vb" Inherits="crmWebClient.yachtPicture"MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" language='javascript'> 
       var arrTemp=self.location.href.split("?"); 
       var picUrl = (arrTemp.length>0)?arrTemp[1]:""; 
       var NS = (navigator.appName=="Netscape")?true:false; 

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
         
         window.onload = function() {
          FitPic();
        };
    </script>

    <link rel="stylesheet" href="Gallery/css/basic.css" type="text/css" />
    <link rel="stylesheet" href="Gallery/css/galleriffic-3.css" type="text/css" />

    <script type="text/javascript" src="Gallery/js/jquery-1.3.2.js"></script>

    <script type="text/javascript" src="Gallery/js/jquery.history.js"></script>

    <script type="text/javascript" src="Gallery/js/jquery.galleriffic.js"></script>

    <script type="text/javascript" src="Gallery/js/jquery.opacityrollover.js"></script>

    <!-- We only want the thunbnails to display when javascript is disabled -->

    <script type="text/javascript">
			document.write('<style>.noscript { display: none; }</style>');
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label runat="server" ID="controls_buttons" Visible="false">
        <a href="#" onclick="javascript:view('slide');" class="gray_button">View Slideshow</a> <a href="#"
            onclick="javascript:view('printer');" class="gray_button">View Printer Friendly Version</a> <a href="#"
                onclick="javascript:view('all');" class="gray_button">View All</a> <a href="#"
                onclick="javascript:window.close();" class="gray_button float_right">Close</a><div class="NotesHeader"></div></asp:Label>
    <asp:Label ID="picture" runat="server" Text="" Style="display: none;"></asp:Label>
    <asp:Label ID="picture_plain" runat="server" Text=""></asp:Label>

    <asp:Label ID="javascript_text" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

    <script type="text/javascript">
    function view(ty) {
        switch (ty) {
            case "all":
                 $("#<%= picture.clientID %>").css({"display":"none"});
                 $("#<%= picture_plain.clientID %>").css({"display":"block"});
                 $("#bg_image").css({"display":"block"});
            break;
            case "slide":
                 $("#<%= picture.clientID %>").css({"display":"block"});
                 $("#<%= picture_plain.clientID %>").css({"display":"none"});
                 $("#bg_image").css({"display":"block"});
            break;
            
            case "printer":
                 $("#<%= picture.clientID %>").css({"display":"none"});
                 $("#<%= picture_plain.clientID %>").css({"display":"block"});
                 $("#bg_image").css({"display":"none"});
                 $('div#page').css('background-color', 'white');
                 $('#body').css('background', 'white !important');
            break;
        }
    }
    </script>

</asp:Content>