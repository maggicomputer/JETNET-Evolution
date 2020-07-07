// ********************************************************************************
// Copyright 2004-11. JETNET,LLC. All rights reserved.
//
//$$Archive: /commonWebProject/chat/Scripts/chatUtility.js $
//$$Author: Mike $
//$$Date: 6/19/19 8:44a $
//$$Modtime: 6/18/19 6:12p $
//$$Revision: 2 $
//$$Workfile: chatUtility.js $
//
// ********************************************************************************

//    var PageTitleNotification = {
//        Vars:{
//            OriginalTitle: document.title,
//            Interval: null
//        },    
//        On: function(notification, intervalSpeed){
//            var _this = this;
//            _this.Vars.Interval = setInterval(function(){
//                 document.title = (_this.Vars.OriginalTitle == document.title)
//                                     ? notification
//                                     : _this.Vars.OriginalTitle;
//            }, (intervalSpeed) ? intervalSpeed : 1000);
//        },
//        Off: function(){
//            clearInterval(this.Vars.Interval);
//            document.title = this.Vars.OriginalTitle;   
//        }
//    }

function fnDingSound() {

  try { // check and see if this works

    var sound_file = fullHostname + "chat/ding.mp3"
    var html5_audio = new Audio(sound_file);

    if ((typeof (html5_audio) != "undefined") && (html5_audio != null)) {
      html5_audio.play();
    }

  }
  catch (err) { // if that fails then exit
    //alert("Error : Problem creating audio tag");
  }

}

function fnNotifySound() {

  try { // check and see if this works

    var sound_file = fullHostname + "chat/notify.mp3"
    var html5_audio = new Audio(sound_file);

    if ((typeof (html5_audio) != "undefined") && (html5_audio != null)) {
      html5_audio.play();
    }

  }
  catch (err) { // if that fails then exit
    //alert("Error : Problem creating audio tag");
  }

}

function openCleanupWindow() {
  var address = "ChatCleanup.aspx?rid=" + localRoomID + "&aid=" + communityTextAliasID;
  var rightNow = new Date();
  var windowname = "chatCleanup_" + rightNow.getTime();

  var place = window.open(address, windowname, "dependent=no,toolbar=no,scrollbars=no,resizable=no,status=no,menubar=no,location=no,width=10,height=10");
}

function arePopupWindowsBlocked() {

  var address = "/chat/ChatTestPopUp.aspx";
  var rightNow = new Date();
  var windowname = "chatTestPopUp_" + rightNow.getTime();

  var popUp = window.open(address, windowname, "dependent=no,toolbar=no,scrollbars=no,resizable=no,status=no,menubar=no,location=no,width=10,height=10");
  
  try {
    
    popUp.focus();
    popUp.close(); 
    
    return false;
  }
  catch (err) {
    alert("Your browser has blocked \"pop-up\" windows. You must allow popups to use JETNET Community Chat.");
    return true;
  }
  
  //checkPopupBlocked(popUp);

//  $(window).on('message', function(event) {
//    alert(event.originalEvent.data.loaded)
//  });
  //
      
}


function checkPopupBlocked(poppedWindow) {
  setTimeout(function() { doCheckPopupBlocked(poppedWindow); }, 3000);
}

function doCheckPopupBlocked(poppedWindow) {

  var result = false;

  try {
    if (typeof poppedWindow == 'undefined') {
      // Safari with popup blocker... leaves the popup window handle undefined
      result = true;
    }
    else if (poppedWindow && poppedWindow.closed) {
      // This happens if the user opens and closes the client window...
      // Confusing because the handle is still available, but it's in a "closed" state.
      // We're not saying that the window is not being blocked, we're just saying
      // that the window has been closed before the test could be run.
      result = false;
    }
    else if (poppedWindow && poppedWindow.outerWidth == 0) {
      // This is usually Chrome's doing. The outerWidth (and most other size/location info)
      // will be left at 0, EVEN THOUGH the contents of the popup will exist (including the
      // test function we check for next). The outerWidth starts as 0, so a sufficient delay
      // after attempting to pop is needed.
      result = true;
    }
    else if (poppedWindow && poppedWindow.test) {
      // This is the actual test. The client window should be fine.
      result = false;
    }
    else {
      // Else we'll assume the window is not OK
      result = true;
    }

  } catch (err) {
    //if (console) {
    //    console.warn("Could not access popup window", err);
    //}
  }

  if (result) {
    alert("Your browser has blocked \"pop-up\" windows. You must allow popups to use JETNET Community Chat.");
  }
}

function fnAttachChatSearchFilter() {

  try { // check and see if this works

    $("#userSearch").show();

    $("#filter").unbind();
    $("#filter").on('keyup', function() {

      var filter = $(this).val();
      var count = 0;
      var css = "";

      if (filter != null) {
        if (filter.length >= 3) {

          $(".commentlist li").each(function() {

            companyAnswer = $(this).find("company");
            emailAnswer = $(this).find("email");
            nameAnswer = $(this).find("strong");
            nameLink = $(this).find("a");

            if (companyAnswer.text().search(new RegExp(filter, "i")) < 0) {
              DoesCompanyMatch = false;
              companyAnswer.removeClass("red_text");
            } else {
              DoesCompanyMatch = true;
              companyAnswer.addClass("red_text");
            }

            if (emailAnswer.text().search(new RegExp(filter, "i")) < 0) {
              DoesEmailMatch = false;
              emailAnswer.removeClass("red_text");
            } else {
              DoesEmailMatch = true;
              emailAnswer.addClass("red_text");
            }

            if (nameAnswer.text().search(new RegExp(filter, "i")) < 0) {
              DoesNameMatch = false;
              nameAnswer.removeClass("red_text");
              nameLink.removeClass("red_text");
            } else {
              DoesNameMatch = true;
              nameAnswer.addClass("red_text");
              nameLink.addClass("red_text");
            }

            if ((DoesCompanyMatch == false) && (DoesEmailMatch == false) && (DoesNameMatch == false)) {
              $(this).fadeOut();
            } else {
              $(this).show();
              if (css != "") {
                $(this).addClass("alt_row");
                css = "";
              } else {
                css = "alt_row";
                $(this).removeClass("alt_row");
              }
              count++;
            }

          });

          $("#filter-count").text(count);
          $("#userHeader").show();

        }
      }

    });

  }
  catch (err) { // if that fails then exit
    //alert("Error : Problem Finding Community List");
  }

}

function fnGetCommunityListUsers() {
  crmwebclient.chatservices.GetCommunityList(fnGetCommunityListUsersOnSuccessCallBack, fnGetCommunityListUsersErrorCallBack);
}

function fnGetCommunityListUsersOnSuccessCallBack(args) {

  $(document).ready(function() {
    var table = $("#tblChatUsers");

    table.html("");

    //    var TR = document.createElement("TR");
    //    var TH = document.createElement("TH");
    //    $(TR).appendTo(table);

    //    $(TH).attr("colspan", "3");

    //    $(TH).appendTo(TR).html("");

    var bOnline = false;

    if (args.length != 0) {

      $(args).each(function(i) {

        bOnline = this.IsOnline;

        var tr = document.createElement("TR");
        $(tr).attr("id", "_chat_row_" + this.BuddyUID);
        var td = document.createElement("TD");
        $(tr).appendTo(table);

        if (bOnline) {
          $(td).appendTo(tr).html("<div id=\"_chat_user_" + this.BuddyUID + "\" style=\"text-align: left; vertical-align: middle;\"><a id=\"_chat_link_" + i + "\" class=\"chat_link underline pointer\" onclick='fnStartNewChat(\"" + this.BuddyAlias + "\"," + this.BuddyUID + ",\"" + this.BuddyName + "\");'><img src=\"../images/user_male.png\" alt=\"chat with " + this.BuddyName + "\" title=\"chat with " + this.BuddyName + "\" border=\"0\" height=\"34\" width=\"34\" /></a></div>");
        } else {
          $(td).appendTo(tr).html("<div id=\"_cu_" + this.BuddyUID + "\" style=\"text-align: left; vertical-align: middle;\"><img src=\"../images/user_male_gray.png\" alt=\"" + this.BuddyName + " is OFFLINE\" title=\"" + this.BuddyName + " is OFFLINE\" border=\"0\" height=\"34\" width=\"34\" /></div>");
        }

        td = document.createElement("TD");
        $(td).appendTo(tr).html("<div style=\"text-align: left; vertical-align: middle; font-size: 10px;\">" + this.BuddyName + "<br />" + this.BuddyComapnyName + "</div>");
        td = document.createElement("TD");
        $(td).appendTo(tr).html("<a class=\"underline pointer\" onclick='fnRemoveCommunityUserNotify(\"" + this.BuddyAlias + "\"," + this.BuddyUID + ");'><img src=\"../images/delete_icon.png\" alt=\" remove " + this.BuddyName + " from your JETNET Community list\" title=\" remove " + this.BuddyName + " from your JETNET Community list\" border=\"0\" height=\"14\" width=\"14\" /></a>");

      });

    } else {

      var tr = document.createElement("TR");
      var td = document.createElement("TD");
      $(tr).appendTo(table);
      $(td).appendTo(tr).html("<div style=\"text-align: left; vertical-align: middle;\"> no JETNET Community users <strong><em>Online</em></strong> </div>");
      td = document.createElement("TD");
      $(td).appendTo(tr).html("&nbsp;");
      td = document.createElement("TD");
      $(td).appendTo(tr).html("&nbsp;");

    }

  });


  setTimeout(function() { fnGetCommunityListUsers(); }, 3000);

}

function fnGetCommunityListUsersErrorCallBack(args) {
  setTimeout(function() { fnGetCommunityListUsers(); }, 3000);
}

function fnAddCommunityUserNotify(txtAlias, txtAliasID) {
  ShowChatConnect("DivChatMessage", "Adding Community User", "Please wait ...");
  crmwebclient.chatservices.AddUserCommunityList(txtAlias, txtAliasID, fnAddCommunityUserNotifyOnSuccessCallBack);
}

function fnAddCommunityUserNotifyOnSuccessCallBack(args) {
  CloseChatConnect("DivChatMessage");
  if (args != null) {
    if (args === true) {
      alert("User Added to JETNET Online Community List");
      $(".commentlist").fadeOut();
      $("#userHeader").fadeOut();
      $("#filter").value = ""
    }
  }
}

function fnRemoveCommunityUserNotify(txtAlias, txtAliasID) {
  ShowChatConnect("DivChatMessage", "Removing Community User", "Please wait ...");
  crmwebclient.chatservices.DeleteCommunityListUser(txtAlias, txtAliasID, fnRemoveCommunityUserNotifyOnSuccessCallBack);
}

function fnRemoveCommunityUserNotifyOnSuccessCallBack(args) {
  CloseChatConnect("DivChatMessage");
  if (args != null) {
    if (args === true) {
      alert("User Removed from JETNET Online Community List");
    }
  }
}