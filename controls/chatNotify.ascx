<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="chatNotify.ascx.vb" Inherits="crmWebClient.chatNotify" %>

  <script type="text/javascript" src="/chat/scripts/chatUtility.js"></script>
  <script type="text/javascript" src="/chat/scripts/chatNotify.js"></script>
  <script type="text/javascript" src="/chat/scripts/chatWithUser.js"></script>

  <link rel="Stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.24/themes/smoothness/jquery-ui.css" />

  <style type="text/css">

    A.underline
    {
      font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
      text-decoration: underline;
      cursor: pointer;
    }
    
  </style>

  <script type="text/javascript">

    var bChatEnabled = <%= bEnableChat.tostring.tolower %>;
    var fullHostname = "<%= fullHostname.trim %>";
 
    var textAlias = "<%= txtAlias.trim %>";
    var textAliasID = <%= txtAliasID.ToString %>;
    var sessGUID = "<%= userSessionGUID.Trim %>";
    var bChangeSub = <%= bChatChangeSub.ToString.Tolower %>;
    var popUpsBlocked = false;
      
    function pageLoad(sender, e) {

      //alert("pageload chat notify"); 
       
      popUpsBlocked = arePopupWindowsBlocked();
  
      if (bChatEnabled && !popUpsBlocked) {

        // add user to chat session table if user doesnt have session allready
        fnUpdateUserSession();
       
        // check for notifications that user wants to chat
        fnGetChatNotification();

        // get community list
        fnGetCommunityListUsers();

        // attach chat search filter
        fnAttachChatSearchFilter();

        $(document).ready(function(){ 
   
          //alert("pageload chat notify document(ready) bChatEnabled :" + bChatEnabled");
 
          var divListLbl = $("#divCommunityListLbl");
          divListLbl.fadeIn();
         
          var divList = $("#divCommunityList");
          divList.fadeIn();
        
          var htmlText = "<em>To turn off your chat service and not display your online status to other users click " +
                     "<a id=\"_enableChatLinkID\" class=\"underline pointer\" style=\"color:Blue;\" onclick='fnDisableChatSession();'>HERE</a>.</em>";
 
          var div = $("#divEnableChat");
          div.html(htmlText);
          div.fadeIn();
          
        });

      } else {
      
        $(document).ready(function(){ 
   
          //alert("pageload chat notify document(ready) bChatEnabled :" + bChatEnabled");
          
          var htmlText = "Evolution provides the ability to Chat with members of the JETNET community. " +
                     "In order for other subscribers to be aware of your online status you must first turn " +
                     "on your Evolution chat service. To turn on your chat service and display your online status to other users click " +
                     "<a id=\"_enableChatLinkID\" class=\"underline pointer\" style=\"color:Blue;\" onclick='fnEnableChatSession();'>HERE</a>.";
          
          if (popUpsBlocked) {
            htmlText = "Evolution provides the ability to Chat with members of the JETNET community. " +
                     "In order for the JETNET Community Chat Service to work properly you must first <strong>\"ALLOW\" pop-up windows " +
                     "on your browser.";

          }
          
          var div = $("#divEnableChat");
          div.html(htmlText);
          div.fadeIn();
          
        });
      
      }
          
    }
  
  </script>

  <div id="DivNotifyMessage" style="display: none;">
    <table width="100%" cellpadding="3" cellspacing="0">
      <tr>
        <td colspan="2" style="text-align: center; height: 42px;">
          <div id="textNotifyMessage"></div>
        </td>
      </tr>
      <tr>
        <td style="text-align: center; vertical-align:bottom;">
          <asp:Button ID="btnAcceptChat" runat="server" OnClientClick='bAcceptFlag = true;$("#DivNotifyMessage").dialog("close");return false;' Text="Yes" />
        </td>
        <td style="text-align:center; vertical-align:bottom;">
          <asp:Button ID="btnDeclineChat" runat="server" OnClientClick='bAcceptFlag = false;$("#DivNotifyMessage").dialog("close");return false;' Text="Not Now" />
        </td>
      </tr>
    </table>
  </div>
  <div id="DivNotAvailbleMessage" style="display: none;">
    <table width="100%" cellpadding="3" cellspacing="0">
      <tr>
        <td colspan="2" style="text-align: center; height: 42px;">
          <div id="textNotAvailbleMessage">
          </div>
        </td>
      </tr>
      <tr>
        <td style="text-align: center; vertical-align: bottom;">
          <asp:Button ID="btnNotAvailble" runat="server" OnClientClick='$("#DivNotAvailbleMessage").dialog("close");return false;'
            Text="Ok" />
        </td>
      </tr>
    </table>
  </div>
  <div id="DivChatMessage" style="display: none;">
  </div>