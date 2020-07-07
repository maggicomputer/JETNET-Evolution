<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ChatBox.aspx.vb" Inherits="crmWebClient._ChatBox"
  EnableViewState="false" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
  <title>JETNET Community Chat</title>

  <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.2.min.js"></script>

  <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.24/jquery-ui.min.js"></script>

  <link rel="Stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.24/themes/smoothness/jquery-ui.css" />

  <script type="text/javascript" src="../common/jquery.sidr.min.js"></script>

  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css" />
  
  <link href="../EvoStyles/stylesheets/additional_styles.css" rel="stylesheet" type="text/css" />
  <link href="../EvoStyles/stylesheets/layout/skeleton_grid.css" rel="stylesheet" type="text/css" />
  <link href="../EvoStyles/stylesheets/chatStyles.css" rel="stylesheet" type="text/css" />

  <script type="text/javascript">

    var fullHostname = "<%= fullHostname.trim %>";
 
    var textAlias = "<%= txtAlias.trim %>";
    var textAliasID = <%= txtAliasID.ToString %>;
 
    var communityTextAlias = "<%= communityAlias.trim %>";
    var communityTextAliasID = <%= communityAliasID.ToString %>;
    var communityTextName = "<%= chatWithFriendlyName.trim %>";

    var localRoomID = "<%= localRoomID.trim %>";

    var bIsAdd = <%= bIsAdd.toString.toLower %>;

    function pageLoad(sender, e) {

      // Get the current message list
      fnUpdateLocalMessage();

      // Get the current user list in this chat room  
      fnUpdateRoomTalkerList();

      // check for response that user accepted or declined to chat
      fnGetChatResponse(communityTextAlias, communityTextAliasID);

    }
  
    $(window).bind('unload', function(){
      runOnUnload();
    });
       
    window.onbeforeunload = runOnBeforeUnload;

    function runOnUnload() {
      //alert('unload event');
                  
      if ((typeof(window.opener) != "undefined") && (window.opener != null)) {
               
        try { // call the fnLeaveChatRoom on the "parent" window
          window.opener.fnLeaveChatRoom(localRoomID, communityTextAliasID);
        }
        catch (err) { // if that fails then open the "cleanUp window"
        
          openCleanupWindow();

        }
        
      }
      else {
        
        openCleanupWindow();
        
      }
      
    }
     
    function runOnBeforeUnload() {
      //alert('beforeunload event');
     
      $(window).unbind('unload');
            
      if ((typeof(window.opener) != "undefined") && (window.opener != null)) {
               
        try { // call the fnLeaveChatRoom on the "parent" window
          window.opener.fnLeaveChatRoom(localRoomID, communityTextAliasID);
        }
        catch (err) { // if that fails then open the "cleanUp window"
        
          openCleanupWindow();

        }
        
      }
      else {
        
        openCleanupWindow();
        
      }
      
    } 
 
//    
//    $(window).bind('focus', function(){
//      PageTitleNotification.Off();
//    });
//   
  </script>

</head>
<body>
  <form id="form1" runat="server" defaultbutton="btnSendMessage" defaultfocus="txtMessage">
  <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableViewState="false">
    <Scripts>
      <asp:ScriptReference Path="~/common/jsDate.js" />
      <asp:ScriptReference Path="~/chat/Scripts/chatUtility.js" />
      <asp:ScriptReference Path="~/chat/Scripts/chatMessage.js" />
      <asp:ScriptReference Path="~/chat/Scripts/chatWithUser.js" />
    </Scripts>
    <Services>
      <asp:ServiceReference Path="~/chat/Services/chatServices.svc" />
    </Services>
  </cc1:ToolkitScriptManager>

  <script type="text/javascript">

//    try {

//      var xPos, yPos;
//      var prm = Sys.WebForms.PageRequestManager.getInstance();

//      function BeginRequestHandler(sender, args) {
//        if (($get('txtMessageList')) != null) {
//          xPos = $get('txtMessageList').scrollLeft;
//          yPos = $get('txtMessageList').scrollTop;
//        }
//      }

//      function EndRequestHandler(sender, args) {
//        if (($get('txtMessageList')) != null) {
//          $get('txtMessageList').scrollLeft = xPos;
//          $get('txtMessageList').scrollTop = yPos;
//        }
//      }

//      prm.add_beginRequest(BeginRequestHandler);
//      prm.add_endRequest(EndRequestHandler);

//    }

//    catch (err) {
//      //document.getElementById("txtMessage").innerHTML = err.message;
//    }

  </script>

  <div class="container">
  
    <div class="sixteen columns">
    
      <div class="gradient-background">
        <a id="simple-menu" href="#sidr" class="gradient-icon"></a>
      </div>
      <div id="sidr">
        <!-- Your content -->
        <ul>
          <li><a href="#" id="btnAddToList" runat="server" onclick="fnAddCommunityUser(communityTextAlias,communityTextAliasID);return false;">
            Add to my Community</a></li>
          <li><a href="#" id="btnRemoveFromList" runat="server" onclick="fnRemoveCommunityUser(communityTextAlias,communityTextAliasID);return false;">
            Remove from my Community</a></li>
          <li><a href="#" id="btnMessageHistory" runat="server" onclick="fnShowHistory();return false;">
            View History</a></li>
        </ul>
      </div>
      <div style="margin-top:.5em; background-image: url('chatHeader.png'); height:100px; background-repeat:no-repeat; width:100%; background-size: 100% 100%;">
      </div>
      <div class="row">
        <div id="lstUserList" class="statusBar  threeColumnPercentage">
          <div class="float_left">
            <img src="../images/user_male_gray.png" alt="<%= chatWithFriendlyName.trim %> not connected" title="<%= chatWithFriendlyName.trim %> not connected"
              border="0" height="44" width="44" /></div>
          <div class="float_left">
            <span class="personName" id="chatPersonName"><%= chatWithFriendlyName.trim %></span><div class="clear_left"></div>
            <span class="personStatus">Online | Not Connected</span>
          </div>
        </div>
        <div class="float_right threeColumnPercentage mobileFloatLeft">
          <asp:Label runat="server" ID="followUpUserInformation" CssClass="otherUserInfo" class="float_left"></asp:Label>
          <asp:Label runat="server" ID="contact_picture" class="float_right"></asp:Label>
        </div>
      </div>
      <div class="row">
        <div class="sixteen columns">
          <div id="message-tab" class="tabs">
            <ul>
              <li><a href="#message-tab-1">Message Window</a></li>
              <li><a href="#company-tab-1">
                <asp:Label ID="company_name" runat="server"></asp:Label></a></li>
            </ul>
            <div id="message-tab-1">
              <div id="txtMessageList" style="font: 14px Arial; padding: 0 10px; overflow: auto; height: 200px;">
              </div>
            </div>
            <div id="company-tab-1">
              <asp:Label ID="company_information_label" runat="server"></asp:Label>
              <asp:Label ID="company_address" runat="server" CssClass="display_none"></asp:Label>
            </div>
          </div>
        </div>
      </div>
      <br />
      <div class="row">
        <div class="sixteen columns">
          <asp:TextBox ID="txtMessage" CssClass="messageBox" runat="server" ToolTip="Enter Message"
            placeholder="Enter Message"></asp:TextBox>
          <asp:Button ID="btnSendMessage" runat="server" OnClientClick="fnSendMessage($get('txtMessage'));return false;"
            Text="Send" class="float_right sendButton" ToolTip="Send message" />
        </div>
      </div>

      <script type="text/javascript">
        try {
          $("#message-tab").tabs();
          $("#simple-menu").sidr();
        }

        catch (err) {
          //document.getElementById("txtMessage").innerHTML = err.message;
        }

        $(document).ready(function(){

          if (bIsAdd) {
            $("#btnAddToList").show();
            $("#btnRemoveFromList").hide();
          }
          else {
            $("#btnAddToList").hide();
            $("#btnRemoveFromList").show();
          }
          
        });
      
      </script>      
    
    </div>
  
  </div>
  
  <div id="DivBackNotifyMessage" style="display: none;">
    <table width="100%" cellpadding="3" cellspacing="0">
      <tr>
        <td colspan="2" style="text-align: center; height: 42px;">
          <div id="textNotifyBackMessage">
          </div>
        </td>
      </tr>
      <tr>
        <td style="text-align: center; vertical-align: bottom;">
          <asp:Button ID="btnBackNotification" runat="server" OnClientClick='$("#DivBackNotifyMessage").dialog("close");return false;'
            Text="Ok" />
        </td>
      </tr>
    </table>
  </div>

  </form>
</body>
</html>
