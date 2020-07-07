<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ChatHistory.aspx.vb" Inherits="crmWebClient.ChatHistory" EnableViewState="false" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
  <title>JETNET CHAT</title>

  <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.2.min.js"></script>

  <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.24/jquery-ui.min.js"></script>

  <link rel="Stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.24/themes/smoothness/jquery-ui.css" />
  <link href="../EvoStyles/stylesheets/additional_styles.css" rel="stylesheet" type="text/css" />

  <script type="text/javascript">

    var talkerID = "<%= TalkerID.trim %>";
    var talkerID1 = "<%= TalkerID1.trim %>";
   
    function pageLoad(sender, e) {
    
      // getHistoricalMessageList
      ShowMessageBox("DivMessage", "Chat History", "Getting chat history ... Please wait ...");
      fnGetHistoricalMessages(talkerID, talkerID1)
    
    }

  </script>

  <style type="text/css">
    body
    {
      font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
      font-size: 12px;
      padding-top: 10px;
    }
    ._tlkFriend
    {
      width: 98%;
      float: left;
      padding: 3px;
      color: blue;
      border-bottom: 1px solid gray;
      white-space: normal;
      overflow: auto;
    }
    ._tlkMe
    {
      width: 98%;
      float: left;
      padding: 3px;
      color: black;
      border-bottom: 1px solid gray;
      white-space: normal;
      overflow: auto;
    }
    ._noHistory
    {
      width: 98%;
      float: left;
      padding: 3px;
      color: Maroon;
      border-bottom: 1px solid gray;
      white-space: normal;
      overflow: auto;
    }    
    ._centerStatus
    {
      text-align: center;
      padding: 3px;
    }
    .tabs
    {
      margin-top: 0.5em;
    }
    #messageHistory-tab
    {
      padding: 0px;
      background: none;
      border-width: 0px;
    }
    #messageHistory-tab .ui-tabs-nav
    {
      padding-left: 0px;
      background: transparent;
      border-width: 0px 0px 1px 0px;
      -moz-border-radius: 0px;
      -webkit-border-radius: 0px;
      border-radius: 0px;
    }
    #messageHistory-tab .ui-tabs-panel
    {
      border-width: 0px 1px 1px 1px;
      padding: .5em;
    }
  </style>
</head>
<body>
  <form id="form1" runat="server">
  
  <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableViewState="false">
    
    <Scripts>
      <asp:ScriptReference Path="~/common/jsDate.js" />
      <asp:ScriptReference Path="~/chat/Scripts/chatMessage.js" />
      <asp:ScriptReference Path="~/chat/Scripts/chatWithUser.js" />
    </Scripts>
    
    <Services>
      <asp:ServiceReference Path="~/chat/Services/chatServices.svc" />
    </Services>
    
  </cc1:ToolkitScriptManager>
  <div style="width: 99%; border: 1px solid; text-align: center; vertical-align: middle;
    padding: 2px;">
    <table width="100%" cellpadding="3" cellspacing="0">
      <tr>
        <td style="width: 100%;" width="100%" align="left" valign="middle">
          <div id="messageHistory-tab" class="tabs">
            <ul>
              <li><a href="#messageHistory-tab-1">Message History</a></li>
            </ul>
            <div id="messageHistory-tab-1">
              <div id="txtMessageHistoryList" style="border: 1px solid; width: 100%; height: 520px;
                overflow: auto; text-align: left;">
              </div>
            </div>
          </div>
        </td>
      </tr>
      <tr>
        <td align="left" valign="middle" style="padding-left: 6px;">
          <asp:Button ID="btnDeleteHistory" runat="server" OnClientClick="fnDeleteHistory(talkerID);return false;"
            Text="Delete" Style="width: 60px; height: 25px;" Visible="true" />
          <asp:Label ID="lblDeleteHistory" runat="server" Visible="true" Text=" all Historical Messages"></asp:Label>
        </td>
      </tr>
    </table>
    
        <script type="text/javascript">
          try {
            $("#messageHistory-tab").tabs();
          }

          catch (err) {
            //document.getElementById("txtMessage").innerHTML = err.message;
          }
      
    </script>
    
  </div>
  
  <div id="DivMessage" style="display: none;">
  </div>

  </form>
</body>
</html>
