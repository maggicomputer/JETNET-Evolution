<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="details.aspx.vb" Inherits="crmWebClient._details"
    MasterPageFile="~/main_site.Master" EnableViewState="true" EnableEventValidation="true" %>

<%@ MasterType VirtualPath="~/main_site.Master" %>
<%@ Register Src="controls/companyCard.ascx" TagName="companyCard" TagPrefix="uc1" %>
<%@ Register Src="controls/contactCard.ascx" TagName="contactCard" TagPrefix="uc2" %>
<%@ Register Src="controls/aircraftCard.ascx" TagName="aircraftCard" TagPrefix="uc3" %>
<%@ Register Src="controls/Company_Tabs.ascx" TagName="Company_Tabs" TagPrefix="uc4" %>
<%@ Register Src="controls/Aircraft_Tabs.ascx" TagName="Aircraft_Tabs" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:Literal ID="white_background_style" runat="server">
    <style type="text/css">
        .white_background
        {
            background-image: url(   '../images/spacer.gif' );
            background-repeat: repeat;
        }
        .container
        {
            -webkit-box-shadow: 0px 0px 0px 0px rgba(4, 4, 4, .3);
            box-shadow: 0px 0px 0px 0px rgba(4, 4, 4, .3);
        }
        .sub_menu {-webkit-box-shadow:  2px 2px 1px 1px rgba(4, 4, 4, .3);box-shadow:  2px 2px 1px 1px rgba(4, 4, 4, .3);}
    </style>
     <style type="text/css">
    [
    unselectable=on]
    {
      -webkit-user-select: none; /* Chrome all / Safari all */
      -moz-user-select: none; /* Firefox all */
      -ms-user-select: none; /* IE 10+ */
      user-select: none; /* Likely future */
    }
  </style>
    </asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" cellspacing="0" cellpadding="0" border="0" align="right">
        <tr>
            <td align="left" valign="top" width="49%">
                <asp:Panel runat="server" ID="css_card_left" CssClass="client_card_left">
                    <uc1:companyCard ID="companyCard" runat="server" />
                    <uc3:aircraftCard ID="aircraftCard" runat="server" Visible="false" />
                </asp:Panel>

            </td> <td align="left" valign="top" width="1%"></td>
            <td align="right" valign="top" width="49%">
                <asp:Panel runat="server" ID="css_card_right" CssClass="client_card_right">
                    <table width="100%" cellspacing="0" cellpadding="0" align="center" border="0">
                        <tr>
                            <td align="right" valign="top">
                                <uc2:contactCard ID="contactCard" runat="server" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="3" valign="top" align="left">
                <uc4:Company_Tabs ID="Company_Tabs1" runat="server" />
                <uc5:Aircraft_Tabs ID="Aircraft_Tabs1" runat="server" />
            </td>
        </tr>
    </table>

    <script type="text/javascript">
	var children=Array();
	function new_child(x) {
	    children[children.length] = window.open("" + x + "", "", "scrollbars=yes,menubar=no,height=700,width=860,resizable=yes,toolbar=no,location=no,status=no");
	} 
 
    function test(x, y) {
        //if (confirm("Would you like to create a client copy of the aircraft first?\n\nPlease press OK to create the Aircraft\n\nPlease press CANCEL to just enter a note.")) {
          //  javascript:window.open("" + y + "","","scrollbars=no,menubar=no,height=450,width=860,resizable=yes,toolbar=no,location=no,status=no");
        //}
       // else {
            javascript: window.open("" + x + "", "", "scrollbars=no,menubar=no,height=450,width=860,resizable=yes,toolbar=no,location=no,status=no");
       // }
    }
    
function CreateValuationRecord(editPage) {
        if (confirm("Opening a Value Analysis for this aircraft requires the creating of a 'client aircraft' record first.  Would you like to proceed to create a client aircraft record?")) {
            javascript:window.open("" + editPage + "","vaulation","scrollbars=yes,menubar=no,height=1000,width=1100,resizable=yes,toolbar=no,location=no,status=no");
        }
}
 function create_comp(x, y) {
        if (confirm("Adding a note to a Jetnet Company forces Client Company Creation. Would you still like to add a note?")) {
            javascript:window.open("" + y + "","","scrollbars=no,menubar=no,height=450,width=860,resizable=yes,toolbar=no,location=no,status=no");
        }
    }
    function warning(y) {
        alert("Please switch to Client View to edit or add a Note.");
        //javascript: window.open("" + y + "", "", "scrollbars=no,menubar=no,height=450,width=860,resizable=yes,toolbar=no,location=no,status=no");
    }
      function create_comp_wanted(x, y) {
        if (confirm("Adding a wanted to a Jetnet Company forces Client Company Creation. Would you still like to add a Wanted?")) {
            javascript:window.open("" + y + "","","scrollbars=no,menubar=no,height=450,width=860,resizable=yes,toolbar=no,location=no,status=no");
        }
    }
    </script>

</asp:Content>
