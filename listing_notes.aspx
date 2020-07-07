<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="listing.aspx.vb" Inherits="crmWebClient._listing"
  MasterPageFile="~/main_site.Master" %>

<%@ Register TagPrefix="obout" Namespace="OboutInc.Flyout2" Assembly="obout_Flyout2_NET" %>
<%@ MasterType VirtualPath="~/main_site.Master" %>
<%@ Import Namespace="crmWebClient.clsGeneral" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:DataGrid runat="server" ID="Results" CellPadding="3" OnItemCommand="dispDetails"
    Width="100%" AllowPaging="true" PageSize="25" AllowSorting="True" AutoGenerateColumns="false"
    PagerStyle-Mode="NumericPages" GridLines="None" CssClass="mGrid" PagerStyle-CssClass="pgr"
    AlternatingItemStyle-CssClass="alt" ItemStyle-CssClass="item_row" ItemStyle-VerticalAlign="Top"
    HeaderStyle-CssClass="th">
    <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" ForeColor="White" />
    <AlternatingItemStyle CssClass="alt_row" />
    <ItemStyle BorderStyle="None" VerticalAlign="Top" />
    <HeaderStyle Wrap="False" HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
    <Columns>
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <%#IIf((DataBinder.Eval(Container.DataItem, "lnote_status") = "A"), "<img src='images/document.png' alt='Note' />", IIf(DataBinder.Eval(Container.DataItem, "lnote_status") = "B", IIF(DataBinder.Eval(Container.DataItem, "lnote_opportunity_status") = "A", "<img src='images/gold_prospect_icon.png' alt='Prospect' />", "<img src='images/disabled_gold_prospect_icon.png' alt='Inactive Prospect' />"), "<img src='images/mail_compose.png' alt='Email' />"))%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Date">
        <ItemTemplate>
          <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="10px" />
          <a href="#" onclick="javascript:window.open('edit_note.aspx?action=edit&type= <%#IIf((DataBinder.Eval(Container.DataItem, "lnote_status") = "A"), "note", IIf(DataBinder.Eval(Container.DataItem, "lnote_status") = "B", "prospect", "email"))%>&id=<%#(DataBinder.Eval(Container.DataItem, "lnote_id"))%>','','scrollbars=no,menubar=no,height=600,width=880,resizable=yes,toolbar=no,location=no,status=no');"
            href="#">
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "lnote_entry_date")), DateAdd("h", Session("timezone_offset"), FormatDateTime(DataBinder.Eval(Container.DataItem, "lnote_entry_date"))), "")%></a>
          <br />
          By:
          <%#Master.what_user((DataBinder.Eval(Container.DataItem, "lnote_user_login")))%><br />
          <%#IIf(DataBinder.Eval(Container.DataItem, "lnote_status") = "A", "For: " & Master.what_user(DataBinder.Eval(Container.DataItem, "lnote_user_id")), "")%>
          <br />
          <img src="images/spacer.gif" width="160" alt="" height="1" />
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Note Text">
        <ItemTemplate>
          <itemstyle width="200px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <%#clsGeneral.Display_Listing_Note_Email_Text(DataBinder.Eval(Container.DataItem, "lnote_note"), DataBinder.Eval(Container.DataItem, "lnote_status"))%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Category">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <%#master.what_cat(DataBinder.Eval(Container.DataItem, "lnote_notecat_key"))%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Aircraft">
        <ItemTemplate>
          <headerstyle width="20px" />
          <%#Master.what_ac(DataBinder.Eval(Container.DataItem, "lnote_jetnet_ac_id"), DataBinder.Eval(Container.DataItem, "lnote_client_ac_id"), 2)%>
          <asp:ImageButton ID="acbutton" ImageUrl="~/images/magnify.png" runat="server" OnClientClick="return false;"
            Style="text-align: center;" Visible='<%# IIF(DataBinder.Eval(Container.DataItem, "lnote_client_ac_id") = 0 and DataBinder.Eval(Container.DataItem, "lnote_jetnet_ac_id") = 0, "false", "true")%>' />
          <obout:Flyout ID="Flyout3" runat="server" AttachTo="acbutton" Position="TOP_RIGHT"
            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true" Visible='<%# IIF(DataBinder.Eval(Container.DataItem, "lnote_client_ac_id") = 0 and DataBinder.Eval(Container.DataItem, "lnote_jetnet_ac_id") = 0, "false", "true")%>'>
            <%#clsGeneral.MouseOverTextStart() %>
            <%#Master.createaNoteACPopOut(Eval("lnote_jetnet_ac_id"), Eval("lnote_client_ac_id"))%>
            </td>
            <td align="left" valign="top" class="rounded_right">
              <%#clsGeneral.MouseOverTextEnd()%>
          </obout:Flyout>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Company">
        <ItemTemplate>
          <%#IIf(DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") <> 0, "<a href='details.aspx?source=CLIENT&type=1&comp_ID=" & DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") & "'", "<a href='details.aspx?source=JETNET&type=1&comp_ID=" & DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id") & "'")%>
          <%#Master.what_comp(DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id"), DataBinder.Eval(Container.DataItem, "lnote_client_comp_id"), 1)%>
          </a><%#Master.what_comp(DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id"), DataBinder.Eval(Container.DataItem, "lnote_client_comp_id"), 2)%><asp:ImageButton
            ID="Button12" ImageUrl="~/images/magnify.png" runat="server" Style="text-align: center;"
            Visible='<%# IIF(DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") = 0 and DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id") = 0, "false", "true")%>' />
          <obout:Flyout ID="Flyout12" runat="server" AttachTo="Button12" Position="TOP_RIGHT"
            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true" Visible='<%# IIF(DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") = 0 and DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id") = 0, "false", "true")%>'>
            <%#clsGeneral.MouseOverTextStart() %>
            <%#Master.createANoteAddressPopOut(Eval("lnote_jetnet_comp_id"), Eval("lnote_client_comp_id"))%>
            <%#clsGeneral.MouseOverTextEnd()%>
          </obout:Flyout>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Aircraft" Visible="false">
        <ItemTemplate>
          <itemstyle width="150px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <span class='<%#IIF(DataBinder.Eval(Container.DataItem, "lnote_opportunity_status") = "I", "note_disabled","")%>'>
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_id")), "<a href=""details.aspx?ac_ID=" & DataBinder.Eval(Container.DataItem, "ac_id").ToString & "&source=" & DataBinder.Eval(Container.DataItem, "ac_source").ToString & "&type=3"">", "")%>
              <%#DataBinder.Eval(Container.DataItem, "amod_make_name") & " " & DataBinder.Eval(Container.DataItem, "amod_model_name")%> <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_id")), "</a>","") %>
            <a href="details.aspx?ac_ID=<%#DataBinder.Eval(Container.DataItem, "ac_id") %>&source=<%#DataBinder.Eval(Container.DataItem, "ac_source") %>&type=3">
              <%#IIF(not isdbnull(DataBinder.Eval(Container.DataItem, "ac_ser_nbr")), "<br />Ser #: " & DataBinder.Eval(Container.DataItem, "ac_ser_nbr").tostring , "") %></a>
            <%#IIF(not isdbnull(DataBinder.Eval(Container.DataItem, "ac_reg_nbr")), "<br />Reg #: " & DataBinder.Eval(Container.DataItem, "ac_reg_nbr"), "") %>
            <%#IIf(DataBinder.Eval(Container.DataItem, "amod_make_name").ToString = "" And DataBinder.Eval(Container.DataItem, "amod_model_name").ToString = "", "<span class='lighter_gray_text'>NO AIRCRAFT SPECIFIED</span>", "")%>
          </span>
          <asp:ImageButton ID="acbutton1" ImageUrl="~/images/magnify.png" runat="server" OnClientClick="return false;"
            Style="text-align: center;" Visible='<%# IIF(DataBinder.Eval(Container.DataItem, "lnote_client_ac_id") = 0 and DataBinder.Eval(Container.DataItem, "lnote_jetnet_ac_id") = 0, "false", "true")%>' />
          <obout:Flyout ID="Flyout33" runat="server" AttachTo="acbutton1" Position="TOP_RIGHT"
            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true" Visible='<%# IIF(DataBinder.Eval(Container.DataItem, "lnote_client_ac_id") = 0 and DataBinder.Eval(Container.DataItem, "lnote_jetnet_ac_id") = 0, "false", "true")%>'>
            <%#clsGeneral.MouseOverTextStart() %>
            <%#Master.PrepoluatedAircraftPopout(Eval("ac_id"), Eval("amod_make_name"), Eval("amod_model_name"), Eval("ac_year_mfr"), Eval("ac_reg_nbr"), Eval("ac_ser_nbr"), Eval("ac_date_purchased"), Eval("ac_forsale_flag"), Eval("ac_status"), Eval("ac_delivery"), Eval("ac_asking_wordage"), Eval("ac_asking_price"), Eval("ac_est_price"), Eval("ac_date_listed"), Eval("ac_exclusive_flag"), Eval("ac_lease_flag"))%>
            </td>
            <td align="left" valign="top" class="rounded_right">
              <%#clsGeneral.MouseOverTextEnd()%>
          </obout:Flyout>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Prospect" Visible="false">
        <ItemTemplate>
          <itemstyle width="150px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <span class='<%#IIF(DataBinder.Eval(Container.DataItem, "lnote_opportunity_status") = "I", "note_disabled","")%>'>
            <a href="details.aspx?comp_ID=<%#DataBinder.Eval(Container.DataItem, "comp_id").tostring %>&source=<%#DataBinder.Eval(Container.DataItem, "comp_source").tostring %>&type=1">
              <%#DataBinder.Eval(Container.DataItem, "comp_name") %></a>
            <%#IIF(not isdbnull(DataBinder.Eval(Container.DataItem, "comp_address1")), "<br />" & DataBinder.Eval(Container.DataItem, "comp_address1") & " ", "") %>
            <br />
            <%#iif(not isdbnull(DataBinder.Eval(Container.DataItem, "comp_city")), DataBinder.Eval(Container.DataItem, "comp_city") & ", ", "") %>
            <%#iif(not isdbnull(DataBinder.Eval(Container.DataItem, "comp_state")), DataBinder.Eval(Container.DataItem, "comp_state") & " ","") %>
            <%#iif(not isdbnull(DataBinder.Eval(Container.DataItem, "comp_country")), DataBinder.Eval(Container.DataItem, "comp_country") & " ", "") %>
            <%#DataBinder.Eval(Container.DataItem, "comp_zip_code").tostring %>
          </span>
          <asp:ImageButton ID="Button1" ImageUrl="~/images/magnify.png" runat="server" Style="text-align: center;"
            Visible='<%# IIF(DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") = 0 and DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id") = 0, "false", "true")%>' />
          <obout:Flyout ID="Flyout1" runat="server" AttachTo="Button1" Position="TOP_RIGHT"
            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true" Visible='<%# IIF(DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") = 0 and DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id") = 0, "false", "true")%>'>
            <%#clsGeneral.MouseOverTextStart() %>
            <%#Master.PrepopulatedAddressPopOut(Eval("comp_id"), Eval("comp_source"), Eval("comp_name"), Eval("comp_address1"), Eval("comp_address2"), Eval("comp_city"), Eval("comp_state"), Eval("comp_zip_code"), Eval("comp_country"), Eval("comp_description"), Eval("comp_email_address"), Eval("comp_phone_office"), Eval("comp_phone_fax"))%>
            <%#clsGeneral.MouseOverTextEnd()%>
          </obout:Flyout>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Contact">
        <ItemTemplate>
          <itemstyle width="150px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <%#master.what_contact(DataBinder.Eval(Container.DataItem, "lnote_jetnet_contact_id"), DataBinder.Eval(Container.DataItem, "lnote_client_contact_id"))%><asp:ImageButton
            ID="ImageButton2" ImageUrl="~/images/magnify.png" runat="server" Style="text-align: center;"
            Visible='<%# IIF(DataBinder.Eval(Container.DataItem, "lnote_client_contact_id") = 0 and DataBinder.Eval(Container.DataItem, "lnote_jetnet_contact_id") = 0, "false", "true")%>' />
          <obout:Flyout ID="Flyoutcontact" runat="server" AttachTo="ImageButton2" Position="TOP_RIGHT"
            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true" Visible='<%# IIF(DataBinder.Eval(Container.DataItem, "lnote_client_contact_id") = 0 and DataBinder.Eval(Container.DataItem, "lnote_jetnet_contact_id") = 0, "false", "true")%>'>
            <%#clsGeneral.MouseOverTextStart() %>
            <%#Master.createANOTEContactPopOut(Eval("lnote_jetnet_contact_id"), Eval("lnote_client_contact_id"), Eval("lnote_jetnet_comp_id"), Eval("lnote_client_comp_id"))%>
            <%#clsGeneral.MouseOverTextEnd()%>
          </obout:Flyout>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Details" Visible="false">
        <ItemTemplate>
          <span class='<%#IIF(DataBinder.Eval(Container.DataItem, "lnote_opportunity_status") = "I", "note_disabled","")%>'>
            <%#clsGeneral.Display_Listing_Note_Email_Text(DataBinder.Eval(Container.DataItem, "lnote_note"), DataBinder.Eval(Container.DataItem, "lnote_status"))%>
            <br />
            [<a href="#" onclick="javascript:window.open('edit_note.aspx?action=edit&type= <%#IIf((DataBinder.Eval(Container.DataItem, "lnote_status") = "A"), "note", IIf(DataBinder.Eval(Container.DataItem, "lnote_status") = "B", "prospect", "email"))%>&id=<%#(DataBinder.Eval(Container.DataItem, "lnote_id"))%>','','scrollbars=no,menubar=no,height=600,width=880,resizable=yes,toolbar=no,location=no,status=no');"
              href="#">
              <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "lnote_entry_date")), DateAdd("h", Session("timezone_offset"), FormatDateTime(DataBinder.Eval(Container.DataItem, "lnote_entry_date"))), "")%></a>
            By:
            <%#Master.what_user((DataBinder.Eval(Container.DataItem, "lnote_user_login")))%>]</span><br />
          <em>
            <%#Master.what_opportunity_cat(DataBinder.Eval(Container.DataItem, "lnote_notecat_key"), True)%></em>
        </ItemTemplate>
      </asp:TemplateColumn>
    </Columns>
  </asp:DataGrid>
</asp:Content>
