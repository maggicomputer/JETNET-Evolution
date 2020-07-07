<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="listing.aspx.vb" Inherits="crmWebClient._listing"
    MasterPageFile="~/main_site.Master" EnableViewState="true" %>

<%@ Register TagPrefix="obout" Namespace="OboutInc.Flyout2" Assembly="obout_Flyout2_NET" %>
<%@ MasterType VirtualPath="~/main_site.Master" %>
<%@ Import Namespace="crmWebClient.clsGeneral" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DataGrid runat="server" ID="Results" CellPadding="3" OnItemCommand="dispDetails"
        Width="100%" AllowPaging="true" Visible="true" PageSize="25" AllowSorting="false"
        AutoGenerateColumns="false" PagerStyle-NextPageText="Next" EnableViewState="true"
        PagerStyle-PrevPageText="Previous" PagerStyle-Mode="NumericPages" GridLines="None"
        CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingItemStyle-CssClass="alt"
        ItemStyle-CssClass="item_row" ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="th">
        <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" ForeColor="White" />
        <AlternatingItemStyle CssClass="alt_row" />
        <ItemStyle BorderStyle="None" VerticalAlign="Top" />
        <HeaderStyle Wrap="False" HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="Date">
                <ItemTemplate>
                    <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="180px" />
                    <a href="#" onclick="javascript:window.open('edit_note.aspx?action=edit&type=action&id=<%#(DataBinder.Eval(Container.DataItem, "lnote_id"))%>','','scrollbars=no,menubar=no,height=600,width=880,resizable=yes,toolbar=no,location=no,status=no');">
                        <%#Master.GenerateActionItemStartDate(DataBinder.Eval(Container.DataItem, "lnote_schedule_start_date"))%></a>
                    <br />
                    By:
                    <%#Master.what_user((DataBinder.Eval(Container.DataItem, "lnote_user_login")))%><br />
                    For:
                    <%#Master.what_user((DataBinder.Eval(Container.DataItem, "lnote_user_id")))%>
                    <%#DataBinder.Eval(Container.DataItem, "lnote_status")%>
                    <br />
                    <img src="images/spacer.gif" width="100" alt="" height="1" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Note Text">
                <ItemTemplate>
                    <headerstyle width="10px" />
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <%#IIF((len(DataBinder.Eval(Container.DataItem, "lnote_note")) > 100), left(DataBinder.Eval(Container.DataItem, "lnote_note"),255) & "...", DataBinder.Eval(Container.DataItem, "lnote_note"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="lnote_jetnet_comp_id" Visible="false" />
            <asp:BoundColumn DataField="lnote_client_comp_id" Visible="false" />
            <asp:BoundColumn DataField="lnote_jetnet_ac_id" Visible="false" />
            <asp:BoundColumn DataField="lnote_client_ac_id" Visible="false" />
            <asp:BoundColumn DataField="lnote_id" Visible="false" />
            <asp:TemplateColumn HeaderText="Category">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#Master.what_cat(DataBinder.Eval(Container.DataItem, "lnote_notecat_key"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Aircraft">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
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
            <asp:TemplateColumn HeaderText="Aircraft" Visible="false">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#Master.what_ac(DataBinder.Eval(Container.DataItem, "lnote_jetnet_ac_id"), DataBinder.Eval(Container.DataItem, "lnote_client_ac_id"), 2)%>
                    <%#Master.what_ac(DataBinder.Eval(Container.DataItem, "lnote_jetnet_ac_id"), DataBinder.Eval(Container.DataItem, "lnote_client_ac_id"), 1)%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Company">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#IIf(DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") <> 0, "<a href='details.aspx?source=CLIENT&type=1&comp_ID=" & DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") & "'", "<a href='details.aspx?source=JETNET&type=1&comp_ID=" & DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id") & "'")%>
                    <%#Master.what_comp(DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id"), DataBinder.Eval(Container.DataItem, "lnote_client_comp_id"), 1)%>
                    </a><%#Master.what_comp(DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id"), DataBinder.Eval(Container.DataItem, "lnote_client_comp_id"), 2)%><asp:ImageButton
                        ID="Button1" ImageUrl="~/images/magnify.png" runat="server" Style="text-align: center;"
                        Visible='<%# IIF(DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") = 0 and DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id") = 0, "false", "true")%>' />
                    <obout:Flyout ID="Flyout1" runat="server" AttachTo="Button1" Position="TOP_RIGHT"
                        Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true" Visible='<%# IIF(DataBinder.Eval(Container.DataItem, "lnote_client_comp_id") = 0 and DataBinder.Eval(Container.DataItem, "lnote_jetnet_comp_id") = 0, "false", "true")%>'>
                        <%#clsGeneral.MouseOverTextStart() %>
                        <%#Master.createANoteAddressPopOut(Eval("lnote_jetnet_comp_id"), Eval("lnote_client_comp_id"))%>
                        <%#clsGeneral.MouseOverTextEnd()%>
                    </obout:Flyout>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Company" Visible="false">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Contact">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#Master.what_contact(DataBinder.Eval(Container.DataItem, "lnote_jetnet_contact_id"), DataBinder.Eval(Container.DataItem, "lnote_client_contact_id"))%><asp:ImageButton
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
            <asp:TemplateColumn HeaderText="Priority">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <div align="center">
                        <%#clsgeneral.what_flag(DataBinder.Eval(Container.DataItem, "clipri_name"))%></div>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Change Status">
                <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <div align="center">
                        <asp:RadioButtonList ID="change_status" runat="server" EnableViewState="true">
                            <asp:ListItem Text="Completed?" Value="C" />
                            <asp:ListItem Text="Dismissed?" Value="D" />
                        </asp:RadioButtonList>
                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="complete_action" CommandArgument="dispDetails">Change</asp:LinkButton>
                    </div>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

    <script type="text/javascript">
  //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  //' Method name: DayPilotCalendar1_EventClick
  //' Purpose: to redirct the user to the edit_note.aspx
  //' Parameters: lnote_id
  //' Return: 
  //'       none
  //' Change Log
  //'           3/24/2010    - Created By: Tom Jones
  //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  function DayPilotCalendar1_EventClick(lnote_id) 
    {
      {
        // redirect the user to the page appending the lnote_id of the note they clicked on
        //alert('The lnote_id is:' + lnote_id);
        //  window.open("edit_note.aspx?id=" + lnote_id + "&action=edit&type=action", "", "scrollbars=no,menubar=no,height=600,width=900,resizable=yes,toolbar=no,location=no,status=no");
      }
    }
    </script>

    <script type="text/javascript">
  //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  //' Method name: DayPilotCalendar1_NoEventClick
  //' Purpose: to redirct the user to the edit_note.aspx
  //' Parameters: date_time
  //' Return: 
  //'       none
  //' Change Log
  //'           3/25/2010    - Created By: Tom Jones
  //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  function DayPilotCalendar1_NoEventClick(date_time) {
    {
      // redirect the user to the page appending the lnote_id of the note they clicked on
      //alert('The lnote_id is:' + lnote_id);
     // window.open("edit_note.aspx?type=action&action=new&time=" + date_time);
    }
  }
    </script>

</asp:Content>
