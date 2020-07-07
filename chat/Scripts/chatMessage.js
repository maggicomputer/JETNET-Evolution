// ********************************************************************************
// Copyright 2004-11. JETNET,LLC. All rights reserved.
//
//$$Archive: /commonWebProject/chat/Scripts/chatMessage.js $
//$$Author: Mike $
//$$Date: 6/19/19 8:44a $
//$$Modtime: 6/18/19 6:12p $
//$$Revision: 2 $
//$$Workfile: chatMessage.js $
//
// ********************************************************************************

var messageCount = 0;

// Send message
function fnSendMessage(textbox) {

  if (textbox.value != "") {
    
    crmwebclient.chatservices.SendMessage(textbox.value, fnSendMessageSuccessCallBack);
    textbox.value = "";
  }
}

function fnSendMessageSuccessCallBack(args) {
  if (args) {
    fnUpdateLocalMessage();
  }
  else {
    alert(args);
  }
}

// Update message list
function fnUpdateLocalMessage() {
  crmwebclient.chatservices.RecieveMessage(fnUpdateMessageSuccessCallBack, fnUpdateMessageFailedCallBack);  
}

function fnUpdateMessageFailedCallBack(args) {
  setTimeout(function() { fnUpdateLocalMessage(); }, 1000);
}

function fnUpdateMessageSuccessCallBack(args) {
  
  var msgList = $("#txtMessageList");
  msgList.html("");

  if (args !== null) {

    $(args).each(function(i) {

      $("<div/>", {
        "class": (args[i].IsFriend ? "_tlkFriend" : "_tlkMe"),
        text: Format(cDate(args[i].SendTime), "TTT") + ": " + args[i].MessageData
      }).appendTo(msgList);
      
    });

    if (args.length > messageCount) {
      messageCount = args.length

      fnNotifySound();
      //PageTitleNotification.On("New Message!");
      
      msgList.scrollTop(msgList[0].scrollHeight - msgList.height());

    }
   

  }
    
  setTimeout(function() { fnUpdateLocalMessage(); }, 2000);
    
}

// Update talker list in the current chat room
function fnUpdateRoomTalkerList() {
  crmwebclient.chatservices.GetRoomTalkerList(fnUpdateRoomTalkerListSuccessCallBack, fnUpdateRoomTalkerListErrorCallBack);
}

function fnUpdateRoomTalkerListSuccessCallBack(args) {
  var statusLst = $("#lstUserList");
  statusLst.html("");

  var friendConnected = false;

  if (args !== null) {

    $(args).each(function(i) {

      if (args[i].IsFriend == true) {
        friendConnected = true;
        return false;
      }

    });

    if (friendConnected) {
      $("<div/>", {
        "class": "_centerStatus",
        html: "<div class=\"float_left\"><img src=\"../images/users.png\" border=\"0\" height=\"44\" width=\"44\" id=\"userIcon\" alt=\"" + communityTextName + "\" title=\"" + communityTextName + "\"/></div><div class=\"float_left\"><span class=\"personName\" id=\"chatPersonName\">" + communityTextName + "</span><div class=\"clear_left\"></div><span class=\"personStatus\">Online | Joined Conversation</span></div>"
      }).appendTo(statusLst);
    }
    else {
      $("<div/>", {
        "class": "_centerStatus",
        html: "<div class=\"float_left\"><img src=\"../images/user_male_gray.png\" border=\"0\" height=\"44\" width=\"44\" id=\"userIcon\" alt=\"" + communityTextName + "\" title=\"" + communityTextName + "\"/></div><div class=\"float_left\"><span class=\"personName\" id=\"chatPersonName\">" + communityTextName + "</span><div class=\"clear_left\"><span class=\"personStatus\">Online | Not Connected</span></div>"
      }).appendTo(statusLst);
    }
 
  }

  setTimeout(function() { fnUpdateRoomTalkerList(); }, 2000);
}

function fnUpdateRoomTalkerListErrorCallBack(args) {
}

function fnShowHistory() {
  var address = "";
  var rightNow = new Date();
  var windowname = "chatHistory_" + rightNow.getTime();

  address = "/chat/chathistory.aspx?rid=" + localRoomID;

  var place = open(address, windowname, "dependent=no,toolbar=no,scrollbars=no,resizable=no,status=no,menubar=no,location=no,width=660,height=650");

}

function fnGetHistoricalMessages(talkerID, TalkerID1) {
  crmwebclient.chatservices.GetHistoricalMessages(talkerID, TalkerID1, fnGetHistoricalMessagesSuccessCallBack, fnGetHistoricalMessagesFailedCallBack);
}

function fnGetHistoricalMessagesFailedCallBack(args) {
}

function fnGetHistoricalMessagesSuccessCallBack(args) {

  var msgList = $("#txtMessageHistoryList");
  msgList.html("");
  
  CloseMessageBox("DivMessage");

  if (args !== null) {
    if (args.length > 0) {
      $(args).each(function(i) {

        $("<div/>", {
          "class": (args[i].IsFriend ? "_tlkFriend" : "_tlkMe"),
          text: (args[i].IsFriend ? args[i].talkerUserName : "me") + " : " + fnFormatMessageDate(args[i].messageDate) + " - " + args[i].messageBody
        }).appendTo(msgList);

      });

    }
    else {
      $("<div/>", {
        "class": "_noHistory",
        text: "No historical messages to display at this time"
      }).appendTo(msgList);
    }
  }
}

function fnDeleteHistory(txtAlias) {

  alert("not implimented at this time");

}

function ShowMessageBox(DivTag, Title, Message) {
  $("#" + DivTag).html(Message);
  $("#" + DivTag).dialog({ modal: true, title: Title });
}

function CloseMessageBox(DivTag) {
  $("#" + DivTag).dialog("close");
}

function fnFormatMessageDate(txtMessageDate) {

  var msgDate = Format(cDate(txtMessageDate), "m/d/yy")
  var msgTime = Format(cDate(txtMessageDate), "TTT")

  return msgDate + " " + msgTime;
  
}
