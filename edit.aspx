<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="edit.aspx.vb" Inherits="crmWebClient.edit"
    EnableViewState="true" %>

<%@ Register Src="controls/Submenu_Edit_Template.ascx" TagName="Submenu_Edit_Template"
    TagPrefix="uc1" %>
<%@ Register Src="controls/Preference_Edit_Template.ascx" TagName="Preference_Edit_Template"
    TagPrefix="uc2" %>
<%@ Register Src="controls/User_Edit_Template.ascx" TagName="User_Edit_Template"
    TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="controls/Contact_Reference_Edit_Template.ascx" TagName="Contact_Reference_Edit_Template"
    TagPrefix="uc4" %>
<%@ Register Src="controls/Aircraft_Edit_Template.ascx" TagName="Aircraft_Edit_Template"
    TagPrefix="uc5" %>
<%@ Register Src="controls/Company_Edit_Template.ascx" TagName="Company_Edit_Template"
    TagPrefix="uc6" %>
<%@ Register Src="controls/Contact_Edit_Template.ascx" TagName="Contact_Edit_Template"
    TagPrefix="uc7" %>
<%@ Register Src="controls/Aircraft_Edit_Engine_Tab.ascx" TagName="Aircraft_Edit_Engine_Tab"
    TagPrefix="uc8" %>
<%@ Register Src="controls/Aircraft_Edit_Avionics_Tab.ascx" TagName="Aircraft_Edit_Avionics_Tab"
    TagPrefix="uc9" %>
<%@ Register Src="controls/Aircraft_Edit_Propeller_Tab.ascx" TagName="Aircraft_Edit_Propeller_Tab"
    TagPrefix="uc10" %>
<%@ Register Src="controls/Aircraft_Edit_Details_Tabs.ascx" TagName="Aircraft_Edit_Details_Tabs"
    TagPrefix="uc11" %>
<%@ Register Src="controls/Aircraft_Edit_Transactions_Tab.ascx" TagName="Aircraft_Edit_Transactions_Tab"
    TagPrefix="uc12" %>
<%@ Register Src="controls/Aircraft_Edit_Features_Tab.ascx" TagName="Aircraft_Edit_Features_Tab"
    TagPrefix="uc13" %>
<%@ Register Src="controls/ViewLogs.ascx" TagName="ViewLogs" TagPrefix="uc14" %>
<%@ Register Src="controls/ContactQuickEntry.ascx" TagName="ContactQuickEntry" TagPrefix="uc15" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <asp:Literal runat="server" ID="titleh">Web Based CRM</asp:Literal></title>
    <link href="common/redesign.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="common/anylinkmenu.css" />
    <!--Grid/Layout Styles-->
    <link href="EvoStyles/stylesheets/layout/base_html_elements.css" rel="stylesheet"
        type="text/css" />
    <!--Created Stylesheet-->
    <link href="/EvoStyles/stylesheets/additional_styles.css" rel="stylesheet" type="text/css" />
    <link href="/EvoStyles/stylesheets/layout/skeleton_grid.css" rel="stylesheet" type="text/css" />

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-migrate/3.1.0/jquery-migrate.min.js"></script>
    <link href="EvoStyles/stylesheets/tableThemes.css" type="text/css" rel="stylesheet" />

</head>
<body class="gray_background">
    <div class="editContainer row container">
        <div class="columns sixteen">
            <form id="form1" runat="server">
                <!-- The following loads a blank page that refreshes 60 seconds before the session timeout to keep session from expiring -->
                <iframe id="ifrmBlank" frameborder="0" width="0" height="0" runat="server" src="sessionKeepAlive.aspx"></iframe>
                <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </cc1:ToolkitScriptManager>
                <uc14:ViewLogs ID="ViewLogs1" runat="server" Visible="false" />
                <uc1:Submenu_Edit_Template ID="Submenu_Edit_Template1" runat="server" Visible="false" />
                <uc2:Preference_Edit_Template ID="Preference_Edit_Template1" runat="server" Visible="false" />
                <uc3:User_Edit_Template ID="User_Edit_Template1" runat="server" Visible="false" />
                <uc4:Contact_Reference_Edit_Template ID="Contact_Reference_Edit_Template1" runat="server"
                    Visible="false" />
                <uc15:ContactQuickEntry ID="ContactQuickEntry1" runat="server" Visible="false" />
                <uc5:Aircraft_Edit_Template ID="Aircraft_Edit_Template1" runat="server" Visible="false" />
                <uc6:Company_Edit_Template ID="Company_Edit_Template1" runat="server" Visible="false" />
                <uc7:Contact_Edit_Template ID="Contact_Edit_Template1" runat="server" Visible="false" />
                <uc8:Aircraft_Edit_Engine_Tab ID="Aircraft_Edit_Engine_Tab1" runat="server" Visible="false" />
                <uc9:Aircraft_Edit_Avionics_Tab ID="Aircraft_Edit_Avionics_Tab1" runat="server" Visible="false" />
                <uc10:Aircraft_Edit_Propeller_Tab ID="Aircraft_Edit_Propeller_Tab1" runat="server"
                    Visible="false" />
                <uc11:Aircraft_Edit_Details_Tabs ID="Aircraft_Edit_Details_Tabs1" runat="server"
                    Visible="false" />
                <uc12:Aircraft_Edit_Transactions_Tab ID="Aircraft_Edit_Transactions_Tab1" runat="server"
                    Visible="false" />
                <uc13:Aircraft_Edit_Features_Tab ID="Aircraft_Edit_Features_Tab1" runat="server"
                    Visible="false" />
                <br />
                <input type="text" id="_ispostback" value="<%=Page.IsPostBack.ToString()%>" class="display_none" />

                <script type="text/javascript">
                    window.onload = function () {
                        if (document.getElementById('_ispostback').value == 'False') {
                            ResizeWindowInfo();
                        }
                    }

                    function ResizeWindowInfo() {
                        var myWidth = 0, myHeight = 0;
                        var innerW = 0, innerH = 0;
                        var resizeWidth = 0, resizeHeight = 0;
                        if (typeof (window.outerWidth) == 'number') {
                            //Non-IE
                            myWidth = window.outerWidth;
                            myHeight = window.outerHeight;
                            innerW = window.innerWidth;
                            innerH = window.innerHeight;

                            if ((myWidth == 0) && (myHeight == 0)) {
                                myWidth = 1000;
                                myHeight = 990;
                            }
                        } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                            //IE 6+ in 'standards compliant mode'
                            myWidth = document.documentElement.clientWidth;
                            myHeight = document.documentElement.clientHeight;
                        } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                            //IE 4 compatible
                            myWidth = document.body.clientWidth;
                            myHeight = document.body.clientHeight;
                        }

                        if ((innerH > 0) && (innerW > 0)) {
                            resizeWidth = (myWidth - innerW) + 60;

                            if (myHeight < 600) {
                                resizeHeight = (myHeight - innerH) + 60
                            }
                            window.resizeBy(resizeWidth, resizeHeight)
                        }

                    }
                </script>

            </form>
        </div>
    </div>
</body>
</html>
