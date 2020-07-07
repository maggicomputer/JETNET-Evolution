// ********************************************************************************
// Copyright 2004-11. JETNET,LLC. All rights reserved.
//
//$$Archive: /CRM Evolution/chat/Scripts/chatNotify.js $
//$$Author: Mike $
//$$Date: 11/05/15 4:35p $
//$$Modtime: 11/05/15 1:40p $
//$$Revision: 66 $
//$$Workfile: chatNotify.js $
//
// ********************************************************************************

// chat from variables
var bAcceptFlag = false;
var notifyRoomID = "";
var fromAliasName = "";
var fromAliasID = "";

function fnLogOffUserSession() {
  crmwebclient.chatservices.LogSessionOff();
}

function fnUpdateUserSession(txtAlias) {
  crmwebclient.chatservices.UpdateChatSession(fnUpdateUserSessionOnSuccessCallBack);
}

function fnUpdateUserSessionOnSuccessCallBack() {
  setTimeout(function() { fnUpdateUserSession(); }, 10000);
}

function fnGetChatNotification() {
  crmwebclient.chatservices.HasChatNotifications(fnGetNotificationOnSuccessCallBack, fnGetNotificationErrorCallBack);
}

function fnGetNotificationOnSuccessCallBack(args) {

  if ((typeof (args) != "undefined") && (args.NotifyID != 0)) {
    
    fnShowChatNotification(args);
    
  } else { // if we dont have any notifications check again in 5 sec
  
    setTimeout(function() { fnGetChatNotification(); }, 5000);
  
  }

}

function fnGetNotificationErrorCallBack(args) {
  // check for addtional notifications
  setTimeout(function() { fnGetChatNotification(); }, 5000);
}

function fnShowChatNotification(args) {

  var fromAlias = "";
  var roomID = "";
  var notifyStatus = "";
  var notifyID = "";
  var fromUserCompany = "";

  fromUserCompany = args.FromUserCompanyName;
  fromAlias = args.FromAlias;

  roomID = args.RoomID;
  notifyID = args.NotifyID;
  notifyStatus = args.NotifyStatus;

  fromAliasName = args.FromUserName;
  fromAliasID = args.FromUserUID;

  //alert("fromAliasID : " + fromAliasID + " fromAliasName : " + fromAliasName);
  
  // if the "textAlias" != sessionAlias then show chat notifacition
  if (textAlias != fromAlias) {
    if (notifyStatus == "N") {

      fnDingSound();

      $("#textNotifyMessage").html(fromAliasName + " from " + fromUserCompany + " would like to chat with you?");
      $("#DivNotifyMessage").dialog({ modal: false, show: 'slide', title: 'JETNET Chat Request', width: 275, height: 140, resizable: false, closeOnEscape: true, close: function(event) { } });

      $("#DivNotifyMessage").unbind();
      $("#DivNotifyMessage").on('dialogclose', function(event) {

        notifyRoomID = roomID;
        crmwebclient.chatservices.UpdateNotificationStatus(notifyID, bAcceptFlag, roomID, fnUpdateNotificationStatusSuccessCallBack, fnUpdateNotificationStatusErrorCallBack);

      });

    }

  }

}

function fnUpdateNotificationStatusErrorCallBack(args) {
  alert("ERROR : UpdateNotification : " + args);
  
  // check for addtional notifications
  setTimeout(function() { fnGetChatNotification(); }, 5000);

}

function fnUpdateNotificationStatusSuccessCallBack(args) {
  //alert("SUCCESS UpdateNotification : " + args);

  if (bAcceptFlag) {
    
    ShowChatConnect("DivChatMessage", "Community Chat", "connecting with " + fromAliasName + " ... Please wait ...");

    // check and see if we are not "chatting" with this user
    crmwebclient.chatservices.IsUserInCurrentChat(fromAliasID, fnInCurrentChatWithSuccessCallBack);

  } else { // if user declined chat check for other notifications
    fnGetChatNotification();
  }

}

function fnInCurrentChatWithSuccessCallBack(args) {

  if (args != null) {

    if (args === false) {
      crmwebclient.chatservices.AddChatUserAlias(fromAliasID, fnAddChatWithUserAliasSuccessCallBack);
    } else {
      alert("Already in a \"CHAT\" with " + fromAliasName);

      // check for addtional notifications
      setTimeout(function() { fnGetChatNotification(); }, 5000);
    }

  }

}

function fnAddChatWithUserAliasSuccessCallBack(args) {
  // before we "join the chat room" verify "other talker" is still there
  fnIsRoomTalkerAvailable(notifyRoomID);
}

function fnIsRoomTalkerAvailable(roomID) {
  notifyRoomID = roomID;
  crmwebclient.chatservices.IsRoomTalkerAvailable(roomID, fnIsRoomTalkerAvailableSuccessCallBack, fnIsRoomTalkerAvailableErrorCallBack);
}

function fnIsRoomTalkerAvailableErrorCallBack(args) {

  alert("ERROR : available? " + args);

  // check for addtional notifications
  setTimeout(function() { fnGetChatNotification(); }, 5000);

}

function fnIsRoomTalkerAvailableSuccessCallBack(args) {
  
  //alert("SUCCESS : available? " + args);
  
  var bAvailable = false;

  if (args != null) {
    bAvailable = args;
  }

  fnJoinChatRoomTo(notifyRoomID, bAvailable);
  
}

function fnJoinChatRoomTo(roomID, bAvailable) {

  // if bAvailable = true JoinChatRoom else display "unavailable" message

  if (bAvailable) {
    crmwebclient.chatservices.JoinChatRoom(roomID, fnJoinChatRoomToSuccessCallBack, fnJoinChatRoomToErrorCallBack);
  }
  else {
    $("#textNotAvailbleMessage").html("User is no longer available. Please try again later.");
    $("#DivNotAvailbleMessage").dialog({ modal: false, show: 'slide', title: 'JETNET Chat Response', width: 275, height: 140, resizable: false, closeOnEscape: true, close: function(event) { } });
    
    $("#DivNotAvailbleMessage").unbind();
    $("#DivNotAvailbleMessage").on('dialogclose', function(event) {
 
      crmwebclient.chatservices.DeleteUserNotification(roomID, false);
      
      // remove this "alias" from the array
      crmwebclient.chatservices.DeleteChatUserAlias(fromAliasID);

      // start checking for notifications again
      fnGetChatNotification();

    });

  }
  
}

function fnJoinChatRoomToSuccessCallBack(args) {

  var address = "";
  var rightNow = new Date();
  var windowname = "chatWindow_" + rightNow.getTime();
  
  if (args != null) {

    CloseChatConnect("DivChatMessage");

    address = "/chat/chatbox.aspx?rid=" + args.RoomID;

    //alert("Address : " + address);

    var place = window.open(address, windowname, "dependent=yes,toolbar=no,scrollbars=no,resizable=no,status=no,menubar=no,location=no,width=780,height=585");

    place.focus();

  }
  else {
    // remove this "alias" from the array
    crmwebclient.chatservices.DeleteChatUserAlias(fromAliasID);
    alert("Error : Problem Joining Chat room (to)");
  }

  // start checking for notifications again
  fnGetChatNotification();
  
}

function fnJoinChatRoomToErrorCallBack(args) {
  CloseChatConnect("DivChatMessage");

  // start checking for notifications again
  setTimeout(function() { fnGetChatNotification(); }, 5000);

}

function fnEnableChatSession() {
  //alert("enable chat session");
  if (!bChatEnabled) {
    crmwebclient.chatservices.ChangeChatSession(sessGUID, textAlias, true, bChangeSub, fnEnableChatSessionOnSuccessCallBack);
  }
}

function fnEnableChatSessionOnSuccessCallBack(args) {
  if (args != null) {
    if (args == true) {
      alert("You have \"ENABLED\" chat for this subscription");
      window.location.reload(true);
    } else {
      alert("there was an \"ERROR\" starting chat for this subscription");
    }
  }
}

function fnDisableChatSession() {
  //alert("disable chat session");
  if (bChatEnabled) {
    crmwebclient.chatservices.ChangeChatSession(sessGUID, textAlias, false, bChangeSub, fnDisableChatSessionOnSuccessCallBack);
  }
}

function fnDisableChatSessionOnSuccessCallBack(args) {
  if (args != null) {
    if (args == true) {
      alert("You have \"DISABLED\" chat for this subscription");
      window.location.reload(true);
    } else {
      alert("there was an \"ERROR\" stopping chat for this subscription");
    }
  }
}
