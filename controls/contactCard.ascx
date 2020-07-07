<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="contactCard.ascx.vb"
  Inherits="crmWebClient._contactCard" EnableViewState="true" %>
<cc1:TabContainer ID="tab_info_container" runat="server" Width="100%" CssClass="dark-theme"
  Visible="true" Height="190px" AutoPostBack="false">
  <cc1:TabPanel ID="aircraft_contact_tab" runat="server" HeaderText="CONTACTS">
    <HeaderTemplate>
      CONTACTS
    </HeaderTemplate>
    <ContentTemplate>
      <asp:Panel runat="server" CssClass="card_overflow_grid" ID="Panel1">
        <table width="100%" cellspacing="0" cellpadding="0" align="center" border="0">
          <tr>
            <td>
              <asp:Panel ID="aircraft_contact" runat="server" Visible="False">
                <table width="100%">
                  <tr>
                    <td align="left" valign="top">
                      <asp:Label ID="aircraft_contact_details" runat="server" CssClass="comp_contact_info"></asp:Label>
                    </td>
                  </tr>
                </table>
              </asp:Panel>
            </td>
          </tr>
        </table>
      </asp:Panel>
      <asp:Label ID="aircraft_contact_add" runat="server">
                    <a href="#" onclick="javascript:load('edit.aspx?action=reference','','scrollbars=yes,menubar=no,height=500,width=970,resizable=yes,toolbar=no,location=no,status=no');">
                        <img src="images/add_new.jpg" alt="Edit" border="0"></a></asp:Label>
      <asp:Panel ID="contact_add" runat="server" HorizontalAlign="Center">
        <asp:Label ID="synch_date_cont" runat="server" CssClass="float_left" Width="140px"></asp:Label>
        <% If Session("ListingSource") = "CLIENT" Then%>
        <a href="#" onclick="javascript:load('edit.aspx?action=new&type=contact','','scrollbars=yes,menubar=no,height=500,width=1000,resizable=yes,toolbar=no,location=no,status=no');">
          <img src="images/add_new.jpg" alt="Edit" border="0" /></a>
        <% End If%></asp:Panel>
    </ContentTemplate>
  </cc1:TabPanel>
  <cc1:TabPanel ID="company_profile_tabs" runat="server" HeaderText="PROFILE">
    <ContentTemplate>
      <asp:Panel ID="Panel3" runat="server" CssClass="card_overflow_grid">
        <asp:Label ID="company_profile" runat="server" Text="contact_details" Visible="true"></asp:Label>
        <asp:Label ID="company_categories" runat="server" Text="contact_details" Visible="true"></asp:Label>
      </asp:Panel>
    </ContentTemplate>
  </cc1:TabPanel>
  <cc1:TabPanel ID="company_contact_tab" runat="server" HeaderText="CONTACTS">
    <ContentTemplate>
      <asp:Panel runat="server" CssClass="card_overflow_grid" ID="card_overflow" Visible="true">
        <asp:Label ID="contact_no_results" runat="server" Text="" />
        <asp:DataGrid runat="server" ID="contacts_gv" CellPadding="3" OnItemCommand="dispDetails"
          Width="100%" Visible="false" AllowPaging="false" PageSize="60" AllowCustomPaging="True"
          AutoGenerateColumns="false" GridLines="None" CssClass="mGrid" PagerStyle-CssClass="pgr"
          AlternatingItemStyle-CssClass="alt" Font-Size="11px" ItemStyle-CssClass="item_row"
          ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="th">
          <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" />
          <ItemStyle BorderStyle="None" VerticalAlign="Top" HorizontalAlign="Left" />
          <HeaderStyle Wrap="False" HorizontalAlign="left" VerticalAlign="Middle" Height="20px">
          </HeaderStyle>
          <Columns>
            <asp:BoundColumn DataField="contact_id" Visible="false" />
            <asp:TemplateColumn HeaderText="Full Name">
              <ItemTemplate>
                <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                <headerstyle width="180px" />
                <a href="details.aspx?contact_ID=<%#DataBinder.Eval(Container.DataItem, "contact_id")%>&comp_ID=<%#DataBinder.Eval(Container.DataItem, "contact_comp_id")%>&type=1&source=<%#DataBinder.Eval(Container.DataItem, "contact_type")%>">
                  <%#DataBinder.Eval(Container.DataItem, "contact_sirname")%>&nbsp;
                  <%#DataBinder.Eval(Container.DataItem, "contact_first_name")%>&nbsp;
                  <%#DataBinder.Eval(Container.DataItem, "contact_middle_initial")%>&nbsp;
                  <%#DataBinder.Eval(Container.DataItem, "contact_last_name")%>&nbsp;
                  <%#DataBinder.Eval(Container.DataItem, "contact_suffix")%>&nbsp; </a>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Title">
              <ItemTemplate>
                <itemstyle horizontalalign="center" verticalalign="top" />
                <%#DataBinder.Eval(Container.DataItem, "contact_title")%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="">
              <ItemTemplate>
                <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                <headerstyle width="180px" />
                <asp:LinkButton runat="server" ID="contact_remove" CommandName="remove" ForeColor="Red"
                  Visible='<%# IIF(ucase(DataBinder.Eval(Container.DataItem, "contact_type") = "JETNET"),"false" ,"true")%>'
                  OnClientClick="return confirm('Are you sure you want to Remove this Contact?');"> 
                                  Remove
                </asp:LinkButton>
              </ItemTemplate>
            </asp:TemplateColumn>
          </Columns>
        </asp:DataGrid>
        <table width="100%" cellspacing="0" cellpadding="0" align="center" border="0">
          <tr>
            <td align="center" valign="top">
              <asp:LinkButton ID="company_header" runat="server" CssClass="right_head" Font-Underline="True"></asp:LinkButton>&nbsp;&nbsp;
            </td>
            <td align="right" valign="top">
              <asp:Label runat="server" Text="" ID="air_comp_head" CssClass="comp_header"></asp:Label>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top" colspan="2">
              <asp:Panel ID="aircraft_right_panel" runat="server" Visible="false">
                <table width="100%" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="60%" align="left">
                      <asp:Label ID="contact_details" runat="server" Text="contact_details" CssClass="comp_contact_info"></asp:Label>
                    </td>
                    <td width="40%" align="left">
                      <asp:Label ID="contact_phone_details" runat="server" Text="contact_details" CssClass="comp_contact_info"></asp:Label>
                    </td>
                  </tr>
                </table>
              </asp:Panel>
            </td>
          </tr>
        </table>
      </asp:Panel>
      <asp:Panel ID="comp_contact_add" runat="server" HorizontalAlign="Center">
        <% If Session("ListingSource") = "CLIENT" Then%>
        <a href="#" onclick="javascript:load('edit.aspx?action=new&type=contact','','scrollbars=yes,menubar=no,height=500,width=1030,resizable=yes,toolbar=no,location=no,status=no');">
          <img src="images/add_new.jpg" alt="Edit" border="0" /></a>
        <% End If%></asp:Panel>
        <asp:Panel ID="compJetnetAddToClient" Visible="false" runat="server" CssClass="float_right">
               <a href="#" onclick="javascript:load('edit.aspx?action=new&type=contact&createClient=true&contact_ID=<% response.write (Session("ContactID")) %>&comp_ID=<% response.write (Session("OtherID")) %>&source=<%= session.item("listingSource") %>','','scrollbars=yes,menubar=no,height=500,width=1030,resizable=yes,toolbar=no,location=no,status=no');">
          <img src="images/create_client.jpg" alt="Edit" border="0" /></a>
        </asp:Panel>
      <asp:Label runat="server" ID="view_all_contacts" Visible="false" CssClass="float_left"><a href="details.aspx?comp_ID=<%= session.item("listingID") %>&type=1&source=<%= session.item("listingSource") %>&sc=true"><img src="images/view_all.jpg" alt="View All Contacts" border="0"/></a></asp:Label>
      <asp:Panel ID="contact_edit" Visible="false" runat="server" HorizontalAlign="Center">
        <asp:Label ID="synch_date_cont2" runat="server" CssClass="float_left" Width="140px"></asp:Label>
        <% If Session("IsJob") = True Then%>
        <a href="#" onclick="javascript:load('http://www.jetadvisors.com/development/admin/seeker_submittal.asp?id=<% response.write (Session("ListingID")) %>&crm=true','scrollbars=no,menubar=no,height=500,width=1030,resizable=yes,toolbar=no,location=no,status=no');">
          <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/edit_card.jpg" /></a>
        <% Else%>
        <table cellpadding="0" cellspacing="0" align="right">
          <tr>
            <td align="left" valign="top">
              &nbsp;
            </td>
            <td align="right" valign="top">
              <a href="#" onclick="javascript:load('edit.aspx?type=contact&contact_ID=<% response.write (Session("ContactID")) %>&comp_ID=<% response.write (Session("ListingID")) %>&source=<% response.write (Session("ListingSource")) %>','','scrollbars=yes,menubar=no,height=500,width=1030,resizable=yes,toolbar=no,location=no,status=no');">
                <asp:Panel runat="server" ID="contact_edit_btn" Width="63" Style="padding: 0px; margin: 0px;">
                  <img border="0" alt="" src="images/edit_card.jpg" /></asp:Panel>
              </a>
            </td>
            <td align="left" valign="top">
              &nbsp;
            </td>
            <td align="left" valign="top">
              <asp:LinkButton ID="folder_contact" runat="server" CommandName="folder" Visible="false"><img src="images/add_to_folder.jpg" alt="Add to Folder" border="0"/></asp:LinkButton>
            </td>
          </tr>
        </table>
        <% End If%></asp:Panel>
    </ContentTemplate>
  </cc1:TabPanel>
  <cc1:TabPanel ID="ac_picture_tab" Visible="true" runat="server" HeaderText="PICTURES">
    <ContentTemplate>
      <asp:Panel runat="server" CssClass="card_overflow_grid">
        <asp:Label ID="picture_label" runat="server" Visible="true" Font-Bold="true"></asp:Label>
      </asp:Panel>
    </ContentTemplate>
  </cc1:TabPanel>
  <cc1:TabPanel ID="folders_tab" Visible="true" runat="server" HeaderText="FOLDERS">
    <ContentTemplate>
      <asp:Panel ID="Panel2" runat="server" CssClass="card_overflow_grid">
        <asp:UpdatePanel ID="foldersUpdatePanel">
          <ContentTemplate>
            <asp:Label ID="folders_saved_message" runat="server" Visible="false" ForeColor="Red"
              Font-Bold="true"><p align="center">Your folders have been saved.</p></asp:Label>
            <asp:LinkButton ID="save_folder_top" runat="server" Font-Bold="true" Visible="false">Save Folders</asp:LinkButton>
            <asp:Label ID="folders" runat="server" Text="" Visible="false" Style="text-align: left;">folders</asp:Label>
            <asp:LinkButton ID="save_folder_bottom" runat="server" Font-Bold="true" Visible="false">Save Folders</asp:LinkButton>
          </ContentTemplate>
        </asp:UpdatePanel>
      </asp:Panel>
    </ContentTemplate>
  </cc1:TabPanel>
  <cc1:TabPanel ID="aircraft_flight_tab" runat="server" HeaderText="FLIGHTS">
    <ContentTemplate>
      <div class="card_overflow_grid_long">
        <asp:Label ID="flights_warning_text" runat="server" Text="" Font-Bold="true" ForeColor="Red"
          Font-Size="8px"></asp:Label>
        <asp:DataGrid runat="server" ID="flight_dg" Width="390px" CellPadding="0" AllowPaging="false"
          GridLines="None" Font-Size="8px" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingItemStyle-CssClass="alt"
          AutoGenerateColumns="false" ItemStyle-CssClass="item_row" ItemStyle-VerticalAlign="Top"
          HeaderStyle-CssClass="th">
          <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" ForeColor="White" />
          <AlternatingItemStyle CssClass="alt_row" />
          <ItemStyle BorderStyle="None" VerticalAlign="Top" />
          <HeaderStyle Wrap="false" HorizontalAlign="left" VerticalAlign="top"></HeaderStyle>
          <Columns>
            <asp:TemplateColumn HeaderText="Date">
              <ItemTemplate>
                <itemstyle horizontalalign="center" verticalalign="top" />
                <headerstyle width="40px" />
                <%#(DataBinder.Eval(Container.DataItem, "aractivity_date_depart"))%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Origin">
              <ItemTemplate>
                <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                <headerstyle width="10px" />
                <%#(DataBinder.Eval(Container.DataItem, "origin"))%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Destination">
              <ItemTemplate>
                <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                <%#(DataBinder.Eval(Container.DataItem, "destination"))%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Distance<br />(nm)">
              <ItemTemplate>
                <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                <%#(DataBinder.Eval(Container.DataItem, "aractivity_distance"))%>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Flight Time<br />(min)">
              <ItemTemplate>
                <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
                <%#(DataBinder.Eval(Container.DataItem, "aractivity_flight_time"))%>
              </ItemTemplate>
            </asp:TemplateColumn>
          </Columns>
        </asp:DataGrid>
        <asp:Label runat="server" Text="" ID="flight_summary_label" Font-Size="9px"></asp:Label>
        <asp:TextBox runat="server" ID="flight_tab_time" Style="display: none;" />
    
    </ContentTemplate>
  </cc1:TabPanel>
</cc1:TabContainer>
