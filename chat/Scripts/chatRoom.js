// ********************************************************************************
// Copyright 2004-11. JETNET,LLC. All rights reserved.
//
//$$Archive: /commonWebProject/chat/Scripts/chatRoom.js $
//$$Author: Mike $
//$$Date: 6/19/19 8:44a $
//$$Modtime: 6/18/19 6:12p $
//$$Revision: 2 $
//$$Workfile: chatRoom.js $
//
// ********************************************************************************

var selectedRoomID = null;
var selectedAlias = null;

// Show a popup form which is used to create a chat room
function fnShowChatRoomForm() {
    // jQuery.dialog reference:
    // http://jqueryui.com/demos/dialog/
    $("#divCreateChatRoomForm").dialog({ modal: true, show: 'slide', title: 'Create a chat room', width: 500 });
}

// Show a popup message box
function ShowMessageBox(DivTag, Title, Message) {
  $("#" + DivTag).html(Message);
  $("#" + DivTag).dialog({ modal: true, title: Title });
}

// Close the popup message box
function CloseMessageBox(DivTag) {
  $("#" + DivTag).dialog("close");
}


// add user session when loading page
function fuAddUserSession() {
  ShowMessageBox("DivMessage", "Add user session...", "Adding user session...");
  crmwebclient.chatservices.UpdateChatSession($("#txtAlias").val(), fuAddUserSessionOnSuccessCallBack, ajaxErrorCallBack);
}

function fuAddUserSessionOnSuccessCallBack() {
  CloseMessageBox("DivMessage");
}

// delete all sessions
function fuDeleteAllSessions() {
  //crmwebclient.chatservices.DeleteAllSessions(fuDeleteAllSessionsOnSuccessCallBack, ajaxErrorCallBack);
}

function fuDeleteAllSessionsOnSuccessCallBack() {
}

function fuSetChatNotification() {

  ShowMessageBox("DivNotifyUserOfChat", "Notify User...", "Notify Chat User...");

  crmwebclient.chatservices.NotifyChatUser($("#txtWithAlias").val(), $("#txtInRoom").val(), fuSetChatNotificationOnSuccessCallBack, ajaxErrorCallBack);

}

function fuSetChatNotificationOnSuccessCallBack(args) {

  CloseMessageBox("DivNotifyUserOfChat");
  fnJoinChatRoom($("#txtInRoom").val());

}

function fuGetChatNotification() {

  crmwebclient.chatservices.HasChatNotifications(fuGetChatNotificationOnSuccessCallBack, fuGetChatNotificationErrorCallBack);

}

function fuGetChatNotificationOnSuccessCallBack(args) {

  var table = $("#tblChatNotify");
  table.html("");

  var TR = document.createElement("TR");
  var TH = document.createElement("TD");
  $(TR).appendTo(table);
  $(TH).appendTo(TR).html("Alias");
  TH = document.createElement("TH");
  $(TH).appendTo(TR).html("Chat With");

  $(args).each(function (i) {
    var tr = document.createElement("TR");
    var td = document.createElement("TD");
    $(tr).appendTo(table);
    $(td).appendTo(tr).html(this.FromAlias);
    td = document.createElement("TD");
    $(td).appendTo(tr).html("<input type='button' value='Chat' onclick=\"fnJoinChatRoom('" + args[i].RoomID + "');\" />");
  });

  setTimeout(function () { fuGetChatNotification(); }, 2000);

}

function fuGetChatNotificationErrorCallBack(args) {
  setTimeout(function () { fuGetChatNotification(); }, 5000);
}

// Create a chat room
function fuCreateChatRoom() {

    $("#divCreateChatRoomForm").dialog('close');
    ShowMessageBox("DivMessage", "Creating chat room...", "Creating chat room...");

    crmwebclient.chatservices.CreateChatRoom(
                $("#txtAlias").val(),
                $("#txtRoomName").val(),
                $("#txtPassword").val(),
                $("#ddlMaxUser").val(),
                ($("#chkNeedPassword").val() === "on"),
                fuCreateChatRoomOnSuccessCallBack,
                ajaxErrorCallBack
                );

    selectedAlias = $("#txtAlias").val();

}
function fuCreateChatRoomOnSuccessCallBack(args) {
  CloseMessageBox("DivMessage");
  fuGetRoomList();
}

// Get chat room list
function fuGetRoomList() {
  ShowMessageBox("DivMessage","Getting chat room list...", "Getting chat room list...");
  crmwebclient.chatservices.GetChatRoomList(fuGetRoomListOnSuccessCallBack, ajaxErrorCallBack);
}

function fuGetRoomListOnSuccessCallBack(args) {
    var table = $("#tblRoomList");
    table.html("");

    var TR = document.createElement("TR");
    var TH = document.createElement("TD");
    $(TR).appendTo(table);
    $(TH).appendTo(TR).html("RoomID");
    TH = document.createElement("TH");
    $(TH).appendTo(TR).html("RoomName");
    TH = document.createElement("TH");
    $(TH).appendTo(TR).html("MaxUser");
    TH = document.createElement("TH");
    $(TH).appendTo(TR).html("CurrentUser");
    TH = document.createElement("TH");
    $(TH).appendTo(TR).html("Join");

    $(args).each(function (i) {
        var tr = document.createElement("TR");
        var td = document.createElement("TD");
        $(tr).appendTo(table);
        $(td).appendTo(tr).html(this.RoomID);
        td = document.createElement("TD");
        $(td).appendTo(tr).html(this.RoomName);
        td = document.createElement("TD");
        $(td).appendTo(tr).html(this.MaxUser);
        td = document.createElement("TD");
        $(td).appendTo(tr).html("<span id='_cu_" + this.RoomID + "'>" + this.CurrentUser + "</span>");
        td = document.createElement("TD");
        $(td).appendTo(tr).html("<input type='button' value='Join' onclick=\"fnJoinChatRoom('" + args[i].RoomID + "');\" />");
    });

    CloseMessageBox("DivMessage");
}

// Join one chat room
function fnJoinChatRoom(roomid) {
  ShowMessageBox("DivMessage","Joining Chat Room", "Starting Chat");
  crmwebclient.chatservices.JoinChatRoom(roomid, $("#txtAlias").val(), fnJoinChatRoomOnSuccessCallBack);
}

function fnJoinChatRoomOnSuccessCallBack(args) {
  CloseMessageBox("DivMessage");
  if (args !== null) {
    var chatbox = new WebChat.ChatBox();
    chatbox.open(args.RoomID, args.RoomName);
  }
  else {
    ShowMessageBox("Error", "Argument error");
  }
}

function ajaxErrorCallBack(args) {
  CloseMessageBox("DivMessage");
}
