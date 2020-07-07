<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SubNav.ascx.vb" Inherits="crmWebClient.SubNav" %>
<asp:Panel ID="sub_nav" runat="server" CssClass="float_right">
  <table width="100%" cellpadding="0" cellspacing="0">
    <tr>
      <td align="left" valign="middle">
        <asp:Label runat="server" ID="back_visible">
      <% If Session.Item("FromTypeOfListing") = 1 Then%>
       <a href="listing.aspx?redo_search=true" class="back_to_listing">&#9668; Back
        <% ElseIf Session.Item("FromTypeOfListing") = 2 Then%>
        <a href="listing_contact.aspx?redo_search=true" class="back_to_listing">&#9668; Back
        <% ElseIf Session.Item("FromTypeOfListing") = 3 Then%>
        <a href="listing_air.aspx?redo_search=true" class="back_to_listing">&#9668; Back
        <% ElseIf Session.Item("FromTypeOfListing") = 8 Then%>
        <a href="listing_transaction.aspx?redo_search=true" class="back_to_listing">&#9668; Back
        <% Else%>
          <a href="javascript:history.go(-1)" class="back_to_listing">&#9668; Back
        <% End If%></a></asp:Label>
      </td>
      <td align="left" valign="middle">
        <asp:Label runat="server" ID="toggle_evo_js">
    <script type="text/javascript" language="javascript">
      function confirm_box() {
        if (document.getElementById("ctl00_SubNav1_add_folder_cbo").value != "0") {
          return confirm('Are you sure you want to Add this item to this Folder?');
        } else {
          return confirm('Are you sure you want to remove this item from its Folder?');
        }
      }


      window.onload = function() {
        var sPath = window.location.pathname;
        var sPage = sPath.substring(sPath.lastIndexOf('/') + 1);

        if (sPage == "listing_air.aspx") {
          var menuTable = document.getElementById("<%=selected_item_menu.ClientID%>");  //specify your menu id instead of Menu1
          var menuLinks = menuTable.getElementsByTagName("a");
          setOnClickForNextLevelMenuItems(menuTable.nextSibling, "<%=selected_item_menu.ClientID%>", "aircraft_marked");
        } else if (sPage == "listing.aspx") {
          var menuTable = document.getElementById("<%=my_companies.ClientID%>");  //specify your menu id instead of Menu1
          var menuLinks = menuTable.getElementsByTagName("a");
          setOnClickForNextLevelMenuItems(menuTable.nextSibling, "<%=my_companies.ClientID%>", "companies_marked");
        } else if (sPage == "listing_contact.aspx") {
          var menuTable = document.getElementById("<%=my_contacts.ClientID%>");  //specify your menu id instead of Menu1
          var menuLinks = menuTable.getElementsByTagName("a");
          setOnClickForNextLevelMenuItems(menuTable.nextSibling, "<%=my_contacts.ClientID%>", "contacts_marked");

        }


        function setOnClickForNextLevelMenuItems(currentMenuItemsContainer, contain, cookie_name) {
          var id = currentMenuItemsContainer.id;
          var len = id.length;
          if (id != null && typeof (id) != "undefined" && id.substring(0, parseInt(len) - 7) == contain && id.substring(parseInt(len) - 5, parseInt(len)) == "Items") {
            var subMenuLinks = currentMenuItemsContainer.getElementsByTagName("a");
            for (i = 0; i < subMenuLinks.length; i++) {
              switch (i) {
                //case 2: //remove selections from my AC  
                //subMenuLinks[i].onclick = function(){remove_all();}  
                //break;  
                case 2: //Clear Selections
                  subMenuLinks[i].onclick = function() { clear_all(cookie_name); return false; }
                  break;
                //case 3:  //Save to Folder  
                ///   subMenuLinks[i].onclick = function(){load('edit.aspx?action=folder&type=add_list','scrollbars=yes,menubar=no,height=150,width=400,resizable=yes,toolbar=no,location=no,status=no');return false;}  
                //   break;  
                //  case 4:  
                //   subMenuLinks[i].onclick = function(){var where_to= confirm("Do you really want to remove these selections?");if (where_to== true){return true;} else {return false;}}  
                //     break;  
              }
            }
            setOnClickForNextLevelMenuItems(currentMenuItemsContainer.nextSibling);
          }
        }
      }
    </script>
        </asp:Label>
      </td>
      <asp:Label runat="server" ID="operations_text"></asp:Label><td align="left" valign="middle">
        <asp:Menu ID="selected_item_menu" runat="server" Orientation="Vertical" MaximumDynamicDisplayLevels="1"
          DynamicHorizontalOffset="110" Font-Bold="false" StaticHoverStyle-CssClass="static_hover" SkipLinkText=""
          DynamicVerticalOffset="20" Visible="false" Font-Size="11px">
          <LevelMenuItemStyles>
            <asp:MenuItemStyle CssClass="sub" />
            <asp:MenuItemStyle CssClass="mini" BackColor="#eeeeee" />
          </LevelMenuItemStyles>
          <Items>
            <asp:MenuItem Text="My Aircraft" PopOutImageUrl="~/images/spacer.gif" ImageUrl="~/images/spacer.gif">
              <asp:MenuItem Text="Select All Aircraft" Value="1"></asp:MenuItem>
              <asp:MenuItem Text="Save Selections to My Aircraft" Value="2"></asp:MenuItem>
              <asp:MenuItem Text="Clear Selections" Value="4"></asp:MenuItem>
            </asp:MenuItem>
          </Items>
        </asp:Menu>
      </td>
      <td align="left" valign="middle">
      
        <asp:Menu ID="my_companies" runat="server" Orientation="Vertical" MaximumDynamicDisplayLevels="1"
          DynamicHorizontalOffset="110" Font-Bold="false" StaticHoverStyle-CssClass="static_hover"
          DynamicVerticalOffset="20" Visible="false" ImageUrl="~/images/spacer.gif" CssClass="float_left" SkipLinkText=""
          Font-Size="11px">
          <LevelMenuItemStyles>
            <asp:MenuItemStyle CssClass="sub" />
            <asp:MenuItemStyle CssClass="mini" BackColor="#eeeeee" />
          </LevelMenuItemStyles>
          <Items>
            <asp:MenuItem Text="My Companies" PopOutImageUrl="~/images/spacer.gif" ImageUrl="~/images/spacer.gif">
              <asp:MenuItem Text="Select All Companies" Value="1"></asp:MenuItem>
              <asp:MenuItem Text="Save Selections to My Companies" Value="2"></asp:MenuItem>
              <asp:MenuItem Text="Clear Selections" Value="4"></asp:MenuItem>
            </asp:MenuItem>
          </Items>
        </asp:Menu>
      </td>
      <td align="left" valign="middle">
        <asp:Menu ID="my_contacts" runat="server" Orientation="Vertical" MaximumDynamicDisplayLevels="1"
          DynamicHorizontalOffset="110" Font-Bold="false" StaticHoverStyle-CssClass="static_hover" SkipLinkText=""
          DynamicVerticalOffset="20" Visible="false" CssClass="float_left" Font-Size="11px">
          <LevelMenuItemStyles>
            <asp:MenuItemStyle CssClass="sub" />
            <asp:MenuItemStyle CssClass="mini" BackColor="#eeeeee" />
          </LevelMenuItemStyles>
          <Items>
            <asp:MenuItem Text="My Contacts" PopOutImageUrl="~/images/spacer.gif" ImageUrl="~/images/spacer.gif">
              <asp:MenuItem Text="Select All Contacts" Value="1"></asp:MenuItem>
              <asp:MenuItem Text="Save Selections to My Contacts" Value="2"></asp:MenuItem>
              <asp:MenuItem Text="Clear Selections" Value="4"></asp:MenuItem>
            </asp:MenuItem>
          </Items>
        </asp:Menu>
      </td>
      <td align="left" valign="middle">
        <asp:Label runat="server" ID="new_search">
        <% If Session.Item("FromTypeOfListing") = 1 Then%>
        <a href="listing.aspx" class="new_search">Search
        <% ElseIf Session.Item("FromTypeOfListing") = 2 Then%>
        <a href="listing_contact.aspx" class="new_search">Search
        <% ElseIf Session.Item("FromTypeOfListing") = 3 Then%>
        <a href="listing_air.aspx" class="new_search">Search
        <% ElseIf Session.Item("FromTypeOfListing") = 8 Then%>
        <a href="listing_transaction.aspx" class="new_search">Search
        <% End If%></a></asp:Label>
      </td>
      <td align="left" valign="middle">
        <asp:DropDownList ID="add_folder_cbo" runat="server" CssClass="float_left" Visible="false"
          Style="margin-top: 5px; margin-left: 4px;">
        </asp:DropDownList>
      </td>
      <td align="left" valign="middle">
        <asp:ImageButton ID="add_to_folder" runat="server" ImageUrl="~/images/add.png" OnClientClick="return confirm_box();"
          Visible="false" />
      </td>
      <td align="left" valign="middle">
        <asp:Label ID="switch_link_begin" runat="server"></asp:Label>
        <asp:Label runat="server" ID="switch_view_text">
          <asp:Image ID="switch" runat="server" Visible="false" BorderStyle="None" /></asp:Label>
        <asp:Label ID="switch_link_end" runat="server"></a></asp:Label>
      </td>
      <td align="left" valign="middle">
        <asp:Label runat="server" CssClass="float_right">
          <asp:Label ID="show_jetnet_lbl" runat="server" CssClass="oper_header" Visible="false"
            Font-Size="8px">
            <asp:CheckBox runat="server" AutoPostBack="true" ID="show_jetnet_client" ForeColor="White" />Details
            Side by Side?</asp:Label></asp:Label>
        <asp:Label runat="server" ID="gold_prospect_icon_label" Visible="true"></asp:Label>
        <asp:Label runat="server" ID="valuation_label" Visible="true" CssClass="float_right"></asp:Label>
      </td>
    </tr>
  </table>
</asp:Panel>

<script type="text/javascript">
  function crmSaveFolder(strTypeName, actionType, folderID) {
    var str = '';
    window.open("", "myNewWin", "width=700,height=400,toolbar=0,scrollbars=1");
    my_form = document.createElement('FORM');
    my_form.method = 'POST';
    my_form.target = "myNewWin"
    my_form.action = "edit.aspx?action=" + actionType + "&type=add_active"

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'FOLDER_ID';
    my_tb.value = folderID;
    my_form.appendChild(my_tb);

    var elem = document.getElementById('aspnetForm').elements;
    for (var i = 0; i < elem.length; i++) {
      if (elem[i].type != 'hidden' && elem[i].type != 'submit') {
        if (elem[i].value != '') {
          if (elem[i].id.indexOf(strTypeName) != -1) {
            var re = new RegExp("ctl[A-Za-z0-9]*_", "g");
            var re2 = new RegExp(strTypeName + "[A-Za-z0-9]*_", "g");
            var rep = elem[i].id;
            var temp = rep.replace(re, "");

            temp = temp.replace(re2, "");
            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = temp;

            //If it has a checked value that's not undefined, go ahead and 
            //Pass that, if not, pass the value
            if (elem[i].type == 'checkbox') {
              my_tb.value = elem[i].checked;
            } else if (elem[i].type == 'select-multiple') {
              var SelBranchVal = "";
              var x = 0;
              for (x = 0; x < elem[i].length; x++) {
                if (elem[i][x].selected) {
                  //Add seperator just not for 1st entry.
                  if (SelBranchVal != "") {
                    SelBranchVal = SelBranchVal + "##"
                  }
                  SelBranchVal = SelBranchVal + elem[i][x].value;
                }
              }
              my_tb.value = SelBranchVal; //elem[i].value;
            } else if (elem[i].type == 'radio') {
            } else {
              my_tb.value = elem[i].value;
            }

            my_form.appendChild(my_tb);
          }
        }
      }
    }
    document.body.appendChild(my_form);
    my_form.submit();
  }

</script>

