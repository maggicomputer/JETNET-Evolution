// ********************************************************************************
// Copyright 2004-11. JETNET,LLC. All rights reserved.
//
//$$Archive: /CRM Evolution/chat/Scripts/chatWithUser.js $
//$$Author: Mike $
//$$Date: 5/08/15 9:25a $
//$$Modtime: 5/08/15 9:18a $
//$$Revision: 14 $
//$$Workfile: chatWithUser.js $
//
// ********************************************************************************

// chat with variables
var toAlias = "";
var toAliasID = "";
var toAliasName = "";
var userRoomID = "";

var textAlias = ""; 
var textAliasID = "";

function fnStartNewChat(chatWithAlias, chatWithAliasID, chatWithFriendlyName) {

    toAlias = chatWithAlias;
    toAliasID = chatWithAliasID;
    toAliasName = chatWithFriendlyName;
    
    ShowChatConnect("DivChatMessage", "Community Chat", "connecting with " + toAliasName + " ... Please wait ...");

    crmwebclient.chatservices.IsUserInCurrentChat(toAliasID, fnInCurrentChatSuccessCallBack);
}

function fnInCurrentChatSuccessCallBack(args) {

  if (args != null) {

    // check and see if we are not "chatting" with this user
    if (args === false) {
      crmwebclient.chatservices.AddChatUserAlias(toAliasID, fnAddChatUserAliasSuccessCallBack);
    } else {
      CloseChatConnect("DivChatMessage");
      alert("Already in a \"CHAT\" with " + toAliasName);
    }
    
  }
  
}

function fnAddChatUserAliasSuccessCallBack(args) {
  crmwebclient.chatservices.newChatRoom("JETNET Chat", "", 2, false, fnAddChatRoomSuccessCallBack, fnAddChatRoomErrorCallBack);
}

function fnAddChatRoomSuccessCallBack(args) {
  if (args != null) {
    //alert("RoomID : " + args);
    userRoomID = args;
    fnSetChatNotification(userRoomID, toAlias, toAliasID);
  }
  else {
    // remove this "alias" from the array
    crmwebclient.chatservices.DeleteChatUserAlias(toAliasID);
    alert("Error : Problem Adding Chat room");
  }

}

function fnAddChatRoomErrorCallBack(args) {
  // remove this "alias" from the array
  crmwebclient.chatservices.DeleteChatUserAlias(toAliasID);
  alert("Error : Problem Adding Chat room");
}

function fnSetChatNotification(roomID, toAlias, toAliasID) {
  // set notification
  crmwebclient.chatservices.NotifyChatUser(toAlias, toAliasID, roomID, fnSetChatNotificationOnSuccessCallBack);
}

function fnSetChatNotificationOnSuccessCallBack(args) {

  if (args !== null) {

    if (args != userRoomID) {

      crmwebclient.chatservices.DeleteChatRoom(userRoomID); // remove the "new" chat room user is joining previous chat

      userRoomID = args;
      fnJoinChatRoomFrom(userRoomID);

    } else {

      fnJoinChatRoomFrom(userRoomID);

    }
        
  }

}

function fnJoinChatRoomFrom(roomID) {
  crmwebclient.chatservices.JoinChatRoom(roomID, fnJoinChatRoomFromSuccessCallBack, fnJoinChatRoomFromErrorCallBack);
}

function fnJoinChatRoomFromSuccessCallBack(args) {

  var address = "";
  var rightNow = new Date();
  var windowname = "chatWindow_" + rightNow.getTime();

  if (args !== null) {

    CloseChatConnect("DivChatMessage");
    
    address = "/chat/chatbox.aspx?rid=" + args.RoomID;

    //alert("Address : " + address);

    var place = window.open(address, windowname, "dependent=yes,toolbar=no,scrollbars=no,resizable=no,status=no,menubar=no,location=no,width=780,height=585");

    place.focus();

  }
  else {
    // remove this "alias" from the array
    crmwebclient.chatservices.DeleteChatUserAlias(toAliasID);
    alert("Error : Problem Joining Chat room (from)");
  }

}

function fnJoinChatRoomFromErrorCallBack(args) {
  crmwebclient.chatservices.DeleteChatUserAlias(toAliasID);
  CloseChatConnect("DivChatMessage");
}

function fnGetChatResponse(txtAlias, txtAliasID) {
  textAlias = txtAlias;
  textAliasID = txtAliasID;
  crmwebclient.chatservices.GetBackNotifications(textAlias, textAliasID, fnGetChatResponseOnSuccessCallBack, fnGetChatResponseErrorCallBack);
}

function fnGetChatResponseOnSuccessCallBack(args) {

  if ((typeof (args) != "undefined") && (args.NotifyID != 0)) {
    
    //alert("back notification : " + args.NotifyID);
    
    fnShowChatResponse(args);

  } else { // if we dont have any responses check again in 4 sec
    
    //alert("back notification : false");

    setTimeout(function() { fnGetChatResponse(textAlias, textAliasID); }, 4000);
  
  }
}

function fnGetChatResponseErrorCallBack(args) {
  //alert("back notification : error");
  // if we dont have any responses check again in 4 sec
  setTimeout(function() { fnGetChatResponse(textAlias, textAliasID); }, 4000);
}

function fnShowChatResponse(args) {

  var toUserName = "";
  var toUserCompany = "";
  var toUserAlias = "";

  var roomID = "";
  var notifyStatus = "";

  toUserName = args.ToUserName;
  toUserCompany = args.ToUserCompanyName;
  
  toUserAlias = args.ToAlias;

  roomID = args.RoomID
  notifyStatus = args.NotifyStatus;

  //alert("fnShowChatResponse : communityTextAlias[" + communityTextAlias + "] toUserAlias[" + toUserAlias + "]");
  
  // if the "textAlias" = sessionAlias then show back notifacition
  if (textAlias == toUserAlias) {
    if (notifyStatus == "U") {
      $("#textNotifyBackMessage").html(toUserName + " from " + toUserCompany + " is not available. Please try again later.");
      $("#DivBackNotifyMessage").dialog({ modal: false, show: 'slide', title: 'JETNET Chat Response', width: 275, height: 140, resizable: false, closeOnEscape: true, close: function(event) { } });
      
      $("#DivBackNotifyMessage").unbind();
      $("#DivBackNotifyMessage").on('dialogclose', function(event) {
        crmwebclient.chatservices.DeleteUserNotification(roomID, false);
      });
      
    } else {
      if (notifyStatus == "Y") {
        crmwebclient.chatservices.DeleteUserNotification(roomID, true);
      }
    }

  }

}

function fnAddCommunityUser(txtAlias, txtAliasID) {
  ShowChatConnect("DivChatMessage", "Adding Community User", "Please wait ...");
  crmwebclient.chatservices.AddUserCommunityList(txtAlias, txtAliasID, fnAddCommunityUserOnSuccessCallBack);
}

function fnAddCommunityUserOnSuccessCallBack(args) {
  CloseChatConnect("DivChatMessage");
  if (args != null) {
    if (args === true) {
      alert("User Added to JETNET Online Community List");
      bIsAdd = false
      var button = $("#btnAddToList");
      if ((typeof (button) != "undefined") && (button != null)) {
        $("#btnAddToList").hide();
        $("#btnRemoveFromList").show();
      }
    }
  }
}

function fnRemoveCommunityUser(txtAlias, txtAliasID) {
  ShowChatConnect("DivChatMessage", "Removing Community User", "Please wait ...");
  crmwebclient.chatservices.DeleteCommunityListUser(txtAlias, txtAliasID, fnRemoveCommunityUserOnSuccessCallBack);
}

function fnRemoveCommunityUserOnSuccessCallBack(args) {
  CloseChatConnect("DivChatMessage");
  if (args != null) {
    if (args === true) {
      alert("User Removed from JETNET Online Community List");
      bIsAdd = true
      var button = $("#btnAddToList");
      if ((typeof (button) != "undefined") && (button != null)) {
        $("#btnAddToList").show();
        $("#btnRemoveFromList").hide();
      }

    }
  }
}

function fnLeaveChatRoom(roomID, aliasID) {
 
  toAliasID = aliasID;
  
  ShowChatConnect("DivChatMessage", "Ending Chat", "Please wait ...");
  crmwebclient.chatservices.LeaveChatRoom(roomID, fnLeaveChatRoomOnSuccessCallBack, fnLeaveChatRoomOnErrorCallBack);
}

function fnLeaveChatRoomOnSuccessCallBack() {

  // remove this "alias" from the array
  crmwebclient.chatservices.DeleteChatUserAlias(toAliasID, fnDeleteChatUserAliasOnSuccessCallBack);
}

function fnLeaveChatRoomOnErrorCallBack() {
  
  alert("Error : Problem Leaving Chat room");
  // remove this "alias" from the array
  crmwebclient.chatservices.DeleteChatUserAlias(toAliasID, fnDeleteChatUserAliasOnSuccessCallBack);
}

function fnDeleteChatUserAliasOnSuccessCallBack() {
  CloseChatConnect("DivChatMessage");
}


function ShowChatConnect(DivTag, Title, Message) {
  $("#" + DivTag).html(Message);
  $("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 55, resizable: false });
}

function CloseChatConnect(DivTag) {
  $("#" + DivTag).dialog("close");
}