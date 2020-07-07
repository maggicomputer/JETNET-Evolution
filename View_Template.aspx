<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="View_Template.aspx.vb" Inherits="crmWebClient.View_Template" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<%@ Register Src="controls/View_Master.ascx" TagName="View_Master" TagPrefix="uc1" %>
<%@ Register Src="controls/valueControl.ascx" TagName="ValueView" TagPrefix="val" %>
<%@ Register Src="controls/View_Mobile.ascx" TagName="ValueMobile" TagPrefix="mob" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <style type="text/css">
        TH {
            font-weight: bolder;
            font-size: 8.5pt;
            color: black;
            font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
            background-color: #F2F2F2;
            text-align: center;
            vertical-align: middle;
            padding: 2px;
        }

        TD.White {
            font-size: 8pt;
            color: white;
            font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
            font-weight: bold;
        }

        DIV.textLocalNote {
            font-size: 8pt;
            font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
            border: 1px solid #CCD6DB;
            padding: 2px;
            text-decoration: underline;
            float: left;
            width: 300px;
            cursor: pointer;
        }

        DIV.TimeZone {
            font-size: 8pt;
            font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
        }

        .forSaleCellBorder {
            border-width: 0px 0px 1px 1px;
            border-style: solid;
            border-color: #CCD6DB;
        }

        .forSaleCellBorderNoNotes {
            border-width: 0px 1px 1px 1px;
            border-style: solid;
            border-color: #CCD6DB;
        }

        .featuresCellBorder {
            border-width: 1px 1px 1px 1px;
            border-style: solid;
            border-color: #CCD6DB;
        }

        .featuresCellNoBorder {
            border-width: 0px 0px 0px 0px;
            border-style: none;
        }

        .module {
            border: 1px solid #CCD6DB;
            background-color: #ffffff;
        }

        .border {
            border: 1px solid #CCD6DB;
            background-color: #dddddd;
        }

        .header {
            background-image: url(../images/views_header.jpg);
            background-repeat: repeat-x;
            border-bottom: 1px solid #CCD6DB;
            color: #ffffff;
        }

        .tabheader {
            border-width: 1px 1px 0px 0px;
            border-style: solid;
            border-color: #CCD6DB;
            background-color: #EEEEEE;
            text-align: left;
        }

        .border_bottom {
            border-width: 0px 0px 1px 0px;
            border-style: solid;
            border-color: #CCD6DB;
        }

        .border_bottom_right {
            border-width: 0px 1px 1px 0px;
            border-style: solid;
            border-color: #CCD6DB;
        }

        .leftside {
            border-width: 0px 0px 0px 1px;
            border-style: solid;
            border-color: #CCD6DB;
            text-align: left;
        }

        .rightside {
            border-width: 0px 1px 1px 0px;
            border-style: solid;
            border-color: #CCD6DB;
        }

        .leftside_right {
            border-width: 0px 0px 0px 1px;
            border-style: solid;
            border-color: #CCD6DB;
            text-align: right;
        }

        .seperator {
            border-width: 0px 0px 1px 0px;
            border-style: solid;
            border-color: #CCD6DB;
        }

        .picture {
            overflow: auto;
            width: 310px;
            height: 212px;
            margin: 0px;
            padding-top: 5px;
            background-color: #1f6c9a;
            vertical-align: middle;
            text-align: center;
        }

        .picture_charter {
            overflow: auto;
            width: 260px;
            height: 162px;
            margin: 0px;
            padding-top: 5px;
            background-color: #1f6c9a;
            vertical-align: middle;
            text-align: center;
        }

        .papers a {
            background-image: url(../images/papers.jpg);
            background-repeat: no-repeat;
            padding-left: 26px;
            padding-top: 3px;
            line-height: 15px;
            display: block;
        }

        .cover {
            background-image: url(../images/star_cover.jpg);
            background-repeat: no-repeat;
            width: 250px;
            height: 350px;
            float: right;
            color: white;
        }

            .cover a {
                color: #ffffff;
                font-size: 14px;
            }

                .cover a:hover {
                    color: #ff0000;
                    font-size: 14px;
                }

            .cover .toptitle {
                color: #ffffff;
                font-size: 18px;
                font-weight: bold;
            }

            .cover .title {
                color: #ffffff;
                font-size: 14px;
                font-weight: bold;
            }

        .tiny {
            font-size: 10px;
            font-style: italic;
        }

        A.White:active {
            font-size: 8pt;
            color: white;
            font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
        }

        A.White:link {
            font-size: 8pt;
            color: white;
            font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
            text-decoration: underline;
        }

        A.White:visited {
            font-size: 8pt;
            color: white;
            font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
            text-decoration: underline;
        }

        A.White:hover {
            font-size: 8pt;
            color: Yellow;
            font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
        }
    </style>

    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE"></script>

    <link href="common/aircraft_model.css" type="text/css" rel="stylesheet" />



    <script type="text/javascript">
        function ActiveTabChanged(sender, args) { alert("tab:" + sender.name + " activeTab:" + sender.activeTabIndex) }

        function openStarWindowJS(address, windowname) {
            var rightNow = new Date();
            windowname += rightNow.getTime();
            var Place = window.open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1050,resizable=yes,toolbar=no,location=no,status=no");
            return true;
        }

        function openNotesWindowJS(reportWindowPath, sReportFrom) {

            //alert(" show report : " + bShowReport + " report path : " + reportWindowPath + " report number : " + sReportID);

            var rightNow = new Date();
            var reportWindowName = "NotesReport" + sReportFrom + "Window";
            reportWindowName += rightNow.getTime();

            var reportWindowOptions = "scrollbars=yes,menubar=yes,height=800,width=1050,resizable=yes,toolbar=no,location=no,status=no";

            if (reportWindowPath != "") {
                var Place = window.open(reportWindowPath, reportWindowName, reportWindowOptions);
            }

            return true;
        }

        function openSmallWindowJS(address, windowname) {
            var rightNow = new Date();
            windowname += rightNow.getTime();
            var Place = window.open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1150,resizable=yes,toolbar=no,location=no,status=no");
            return true;
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart', 'table'] });
    </script>
    <uc1:View_Master ID="View_Master1" runat="server" />
    <val:ValueView ID="Value_View1" runat="server" Visible="false" />
    <mob:ValueMobile ID="MobileView1" runat="server" Visible="false" />
</asp:Content>
