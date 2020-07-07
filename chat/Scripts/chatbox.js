
// ********************************************************************************
// Copyright 2004-11. JETNET,LLC. All rights reserved.
//
//$$Archive: /CRM Evolution/chat/Scripts/chatbox.js $
//$$Author: Mike $
//$$Date: 2/17/15 3:30a $
//$$Modtime: 2/17/15 3:30a $
//$$Revision: 11 $
//$$Workfile: chatbox.js $
//
// ********************************************************************************

Type.registerNamespace('WebChat');

WebChat.ChatBox = function () {
  WebChat.ChatBox.initializeBase(this);
  this._title = "";
  this._iframe = null;
  this._element = null;
  this._roomid = null;
  this._onDispose = null;
};

var lockBox = new Array();

WebChat.ChatBox.prototype = {
  get_title: function() {
    return this._title;
  },
  set_title: function(val) {
    this._title = val;
  },
  get_roomid: function() {
    return this._roomid;
  },
  set_roomid: function(val) {
    this._roomid = val;
  },
  open: function(RoomId, RoomName) {
    // if this room is NOT in the "room array" then "create chat dialog"

    if ($.inArray(RoomId, lockBox) === -1) {

      this._roomid = RoomId;
      this._title = RoomName;
      this._element = document.createElement("DIV");
      this._element.style.display = "none";

      this._iframe = document.createElement("IFRAME");
      this._iframe.src = "/chat/chatbox.aspx?rid=" + RoomId;
      this._iframe.style.width = "710px";
      this._iframe.frameBorder = 0;
      this._iframe.style.height = "620px";
      this._iframe.scrolling = "no";

      this._element.appendChild(this._iframe);
      document.body.appendChild(this._element);

      var obj = this;
      $(this._element).dialog({ modal: true, show: 'slide', title: this._title, width: 750, height: 670, resizable: false, beforeClose: function() { obj.quit(obj._roomid); } });

      lockBox.push(RoomId);

      this._onDispose = Function.createDelegate(this, this._dispose);

      Sys.Application.add_unload(this._onDispose);

      //$addHandler(window, "unload", function() { obj.quit(obj._roomid); });

      }
      else {
      // this room is still in the "room" array so user did not "close" previous "chat" dialog properly
      //  alert("Exception - You have not joined the chat");
    }
  },
  quit: function(roomid) {

    // leave chat room
    crmwebclient.chatservices.LeaveChatRoom(roomid, function() {

      lockBox = $.map(lockBox, function(n) { return n !== roomid ? n : null; });

      // delete notifications
      crmwebclient.chatservices.DeleteUserNotification(roomid, false);
    });

    $(this._iframe).attr("src", "about:blank");
    $(this._element).dialog('destroy').remove();
    
  },
  _dispose: function() {
    // Add custom dispose actions here
    // leave chat room
    crmwebclient.chatservices.LeaveChatRoom(this._roomid, function() {

      // delete notifications
      crmwebclient.chatservices.DeleteUserNotification(this._roomid, false);
    });

    Sys.Application.remove_unload(this._onDispose);
  }
}
WebChat.ChatBox.registerClass('WebChat.ChatBox');

if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();