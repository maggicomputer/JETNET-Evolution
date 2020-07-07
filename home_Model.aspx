<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master"
    CodeBehind="home_Model.aspx.vb" Inherits="crmWebClient.home_Model" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .defaultModel, .defaultModel a {
            color: #0c95a4 !important;
        }

        .large {
            font-size: 16px;
            margin: 15px;
        }

        #sortable1, #sortable2 {
            border: 1px solid #eee;
            width: 95%;
            min-height: 20px;
            list-style-type: none;
            margin: 15px;
            padding: 5px 0 0 0;
            float: left;
            margin-right: 10px;
            max-height: 500px;
            overflow: auto;
        }

            #sortable1 li, #sortable2 li {
                margin: 0 5px 0px 5px;
                padding: 5px;
                font-size: 1.2em;
                border-bottom: 1px solid #eee;
            }

        h3 {
            font-size: 2em;
            margin-left: 15px;
        }

        h1 {
            font-size: 2.5em;
            color: #078fd7 !important;
            font-style: normal !important;
            margin-left: 15px;
            margin-bottom: -10px;
            font-weight: bold;
        }

        .ui-state-disabled {
            color: Black;
            text-transform: uppercase;
            font-weight: bold;
        }

            .ui-state-disabled:hover {
                text-decoration: none !important;
                cursor: default !important;
                color: Black !important;
            }

        .area {
            font-size: 1.5em !important;
            color: #078fd7 !important;
        }

            .area:hover {
                color: #078fd7 !important;
            }

        .indent {
            margin-left: 15px;
        }

        #sortable1 .ui-state-default:hover, #sortable2 .ui-state-default:hover {
            text-decoration: underline;
            color: #2c93ac;
            cursor: move;
        }

            #sortable1 .ui-state-default:hover, #sortable2 .ui-state-default:hover a:hover {
                color: #2c93ac;
            }

        #searchValueCheck {
            background-image: url('/images/magnify.png');
            background-position: -4px 7px;
            background-repeat: no-repeat;
            width: 87%;
            font-size: 16px;
            padding: 8px 8px 8px 25px;
            border: 1px solid #7fabc2;
        }

        .formatTable.blue.editArea {
            margin-left: auto;
            margin-right: auto;
        }

            .formatTable.blue.editArea td {
                padding: 9px;
            }

            .formatTable.blue.editArea tr {
                border-bottom: 1px solid #eee;
            }

                .formatTable.blue.editArea tr.noBorder {
                    border-bottom: 0px solid #eee;
                }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width='100%' runat="server"
        class="DetailsBrowseTable">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="right" VerticalAlign="middle">
              <div class="backgroundShade">
                <a href="#" onclick="javascript:window.close();" class="gray_button noBefore float_left"><strong>Close</strong></a>
              </div>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <h1 class="emphasis_text" runat="server" id="informationText"></h1>
    <p class="large" runat="server" id="defaultText">Standard Equipment are shown in <span class="defaultModel">teal</span> text. Click to edit attributes in list. Drag attributes within list to re-sort.</p>
    <asp:Panel runat="server" ID="panelUpdateAtt" Visible="false">
        <div class="Box">
            <asp:Label runat="server" CssClass="float_right large" ID="ReturnToList"></asp:Label>
            <asp:Label runat="server" ID="attention" CssClass="large" ForeColor="Red" Font-Bold="true"></asp:Label>
            <hr />
            <asp:ValidationSummary runat="server" ValidationGroup="editField" DisplayMode="BulletList" Font-Bold="true" ForeColor="Red" />
            <table width="90%" cellpadding="3" cellspacing="0" class="formatTable blue large editArea">
                <tr>
                    <td align="left" valign="top" colspan="2">
                        <asp:CheckBox runat="server" ID="standardEquipUpdate" Text="Check this box if this attribute/feature is considered as standard equipment for this aircraft model." TextAlign="Right" /></td>
                </tr>
                <tr>
                    <td align="right" valign="top">Average Dollar Value of this Equipment/Attribute on this model:</td>
                    <td align="left" valign="top">
                        <asp:TextBox runat="server" ID="valueAttUpdate"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" valign="top">S/N Range (if any):</td>
                    <td align="left" valign="top">[<asp:TextBox runat="server" ID="valueAttmod_stdeq_start_ser_no"></asp:TextBox><asp:CompareValidator ErrorMessage="S/N Range Start must be numeric." Operator="DataTypeCheck" runat="server" Type="Integer"  Controltovalidate="valueAttmod_stdeq_start_ser_no" Display="Static" ToolTip="Please use numbers only." Text="*" Font-Size="X-Large" CssClass="help_cursor" ForeColor="Red" Font-Bold="true" ValidationGroup="editField"></asp:CompareValidator> to [<asp:TextBox runat="server" ID="valueAttmod_stdeq_end_ser_no"></asp:TextBox><asp:CompareValidator Operator="DataTypeCheck" runat="server"  Type="Integer" Controltovalidate="valueAttmod_stdeq_end_ser_no" Display="Static" ToolTip="Please use numbers only." CssClass="help_cursor" Text="*" ForeColor="Red" Font-Bold="true" ValidationGroup="editField" Font-Size="X-Large"  ErrorMessage="S/N Range End must be numeric."></asp:CompareValidator>]</td>
                </tr>
                <tr class="noBorder">
                    <td align="left" valign="top" colspan="2">Notes:<br />
                        <asp:TextBox runat="server" ID="valueAttmod_notes" TextMode="MultiLine" Width="100%" Rows="12"></asp:TextBox></td>
                </tr>
                <tr class="noBorder">
                    <td align="right" valign="top" colspan="3">
                        <asp:Button runat="server" ID="submitAttribute" Text="Update" ValidationGroup="editField" CausesValidation="true" /></td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:DataGrid runat="server" ID="test" AutoGenerateColumns="true">
    </asp:DataGrid>
    <asp:Literal runat="server" ID="sort2"></asp:Literal>
    <asp:Literal runat="server" ID="sort1"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

    <script>
        $(function () {
            $("#sortable1, #sortable2").sortable({
                connectWith: ".connectedSortable",
                items: "li:not(.ui-state-disabled)"
            }).disableSelection();

            $("#sortable2").bind("sortupdate", function (event, ui) {
                var data = $(this).sortable('serialize');
                $.ajax({
                    data: '{sendData: ' + ui.item.attr('model') + '}',
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    url: 'JSONresponse.aspx/ModelAttributes?ModelID=' + ui.item.attr('model') + '&' + data
                });
            });
        });


    </script>
    <script>

        function searchTheSortable() {
            var input, filter, ulList, liList, i, txtValue;
            input = document.getElementById("searchValueCheck");
            filter = input.value.toUpperCase();
            ulList = document.getElementById("sortable1");
            liList = ulList.getElementsByTagName("li");
            for (i = 0; i < liList.length; i++) {
                txtValue = liList[i].innerHTML || liList[i].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    liList[i].style.display = "";
                } else {
                    liList[i].style.display = "none";
                }
            }
        }
    </script>
</asp:Content>
