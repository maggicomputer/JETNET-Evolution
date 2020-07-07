<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="sessionKeepAlive.aspx.vb"
  Inherits="crmWebClient.sessionKeepAlive" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>

  <script type="text/javascript">
    function TimeOutRedirect() {
      myString = document.location.toString();
      myString = myString.toUpperCase();

      //    indexOf("Com")
      document.write(myString.indexOf("DEFAULT.ASPX?INACTIVE=TRUE"));
      //        try { 
      //            if (document.location = ) 
      //                self.parent.location=document.location; 
      //             } 
      //        catch (Exception) {}  
    }
  </script>

</head>
<body>
  <form id="form1" runat="server">
  <div>
  </div>
  </form>
</body>
</html>
