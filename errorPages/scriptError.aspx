<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="scriptError.aspx.vb" Inherits="crmWebClient.scriptError" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Welcome to Jetnet CRM - Web Script Error</title>
    <link href="../common/redesign.css" rel="stylesheet" type="text/css" />
    <meta name="robots" content="noindex, nofollow" />
    <link rel="stylesheet" media="all and (min-device-width: 481px) and (max-device-width: 1024px) and (orientation:portrait)"
        href="../common/ipad-portrait.css" />
    <link rel="stylesheet" media="all and (min-device-width: 481px) and (max-device-width: 1024px) and (orientation:landscape)"
        href="../common/ipad-landscape.css" />
    <link rel="stylesheet" media="all and (min-device-width: 1025px)" href="../common/regular.css" />
</head>
<body>
    <img src="../images/background/10.jpg" alt="" class="bg_image" />
    <form id="form1" runat="server">
    <br />
    <br />
    <br />
    <br />
    <table id="Table1" runat="server" width="850" cellspacing="0" cellpadding="0" class="login_white_page body_width"
        align="center">
        <tr>
            <td align="left" valign="top">
                <table border="0" cellpadding="4" cellspacing="0" width="100%">
                    <tr>
                        <td align="left" valign="top" class="login_page_blue_bar">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="top">
                            <h1 class="login_h1">
                                JETNET CRM WEB APPLICATION HAS EXPERIENCED A SCRIPT ERROR</h1>
                            <p style="text-align: center;">
                                Customer Relationship Management (CRM) designed specifically to meet the needs of
                                the Aviation Industry. The JETNET CRM is for Authorized JETNET Users Only.<br />
                                <strong>For more information contact : Paul Cardarelli at 1-800-553-8638 Ext. 254.</strong></p>
                            <br />
                            <asp:LinkButton ID="LinkButton1" runat="server" Font-Size="Larger" PostBackUrl="~/Default.aspx">Return to Login Page</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" class="login_page_blue_bar_bottom">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
