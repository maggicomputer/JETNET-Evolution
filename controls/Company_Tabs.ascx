<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Company_Tabs.ascx.vb"
  Inherits="crmWebClient.Company_Tabs" %>
<%@ Import Namespace="crmWebClient.clsGeneral" %>
<asp:UpdatePanel ID="bottom_tab_update_panel" runat="server" ChildrenAsTriggers="true">
  <ContentTemplate>
    <cc1:TabContainer ID="tabs_container" runat="server" Height="343px" Width="100%"
      CssClass="dark-theme pad_top" Visible="true" AutoPostBack="true" OnClientActiveTabChanged="ActiveTabChanged">
      <cc1:TabPanel ID="aircraft_tab" runat="server" HeaderText="AIRCRAFT" Visible="true">
        <HeaderTemplate>
          AIRCRAFT
        </HeaderTemplate>
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label ID="aircraft_warning_text" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
            <asp:Label runat="server" ID="aircraft_label"></asp:Label>
            <table width="100%" cellspacing="0">
              <tr>
                <td align="left" valign="top">
                  <asp:Label runat="server" ID="aircraft"></asp:Label>
                </td>
              </tr>
            </table>
          </div>
          <asp:TextBox runat="server" ID="aircraft_tab_time" Style="display: none;" />
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="transaction_tab" runat="server" HeaderText="TRANSACTION" Visible="true">
        <HeaderTemplate>
          TRANSACTION
        </HeaderTemplate>
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label ID="trans_warning_text" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
            <table width="100%" cellpadding="0" cellspacing="0">
              <tr>
                <td align="left" valign="top">
                  <table cellpadding="0" class="float_right">
                    <tr>
                      <td align="left" valign="top">
                        <asp:Panel ID="Panel2" runat="server" HorizontalAlign="Left">
                          <asp:Label runat="server" ID="trans_label"></asp:Label></asp:Panel>
                      </td>
                    </tr>
                  </table>
<br clear="all" />
                        <asp:Label runat="server" ID="trans_label_table_text" CssClass="display_block"></asp:Label>

                  <asp:DataGrid runat="server" ID="transaction_gv" CellPadding="7" horizontal-align="left"
                    Font-Size="8pt" Width="400px" AllowPaging="True" PageSize="25" BorderStyle="None"
                    AllowSorting="True" AutoGenerateColumns="false" GridLines="None" CssClass="mGrid"
                    PagerStyle-CssClass="pgr" AlternatingItemStyle-CssClass="alt" ItemStyle-CssClass="item_row"
                    ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="th">
                    <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" Font-Bold="True" Font-Underline="True"
                      ForeColor="White" Mode="NumericPages" NextPageText="Next" PrevPageText="Previous" />
                    <AlternatingItemStyle CssClass="alt_row" />
                    <ItemStyle BorderStyle="None" VerticalAlign="Top" />
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                    <Columns>
                      <asp:TemplateColumn HeaderText="Date">
                        <ItemTemplate>
                          <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "trans_date")), FormatDateTime(DataBinder.Eval(Container.DataItem, "trans_date")), "")%></ItemTemplate>
                      </asp:TemplateColumn>
                      <asp:TemplateColumn HeaderText="Aircraft">
                        <ItemTemplate>
                          <%#DataBinder.Eval(Container.DataItem, "amod_make_name") & " " & DataBinder.Eval(Container.DataItem, "amod_model_name") & " " & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_ser_no_full")), "<br />Ser # <a href='details.aspx?ac_ID=" & DataBinder.Eval(Container.DataItem, "ac_id") & "&type=3&source=JETNET'>" & DataBinder.Eval(Container.DataItem, "ac_ser_no_full") & "</a>", "")%>
                          <img src="images/spacer.gif" width="100" alt="" height="1" />
                        </ItemTemplate>
                      </asp:TemplateColumn>
                      <asp:TemplateColumn HeaderText="Description">
                        <ItemTemplate>
                          <%If Session.Item("crmUserLogon") <> False Then%>
                          <a href="#" onclick="javascript:load('DisplayAircraftDetail.aspx?acid=<%#DataBinder.Eval(Container.DataItem, "ac_id")%>&jid=<%#DataBinder.Eval(Container.DataItem, "trans_id")%>','','scrollbars=yes,menubar=no,height=900,width=1180,resizable=yes,toolbar=no,location=no,status=no');return false;">
                            <%#DataBinder.Eval(Container.DataItem, "tcat_name")%></a> (<em><%#DataBinder.Eval(Container.DataItem, "trans_subject")%></em>)
                          <% End If%>
                        </ItemTemplate>
                      </asp:TemplateColumn>
                      <asp:TemplateColumn HeaderText="Document">
                        <ItemTemplate>
                          <%#clsGeneral.Show_Document_AC_Listing(DataBinder.Eval(Container.DataItem, "tdoc_pdf_exist_flag"))%><%#DataBinder.Eval(Container.DataItem, "tdoc_doc_type")%></ItemTemplate>
                      </asp:TemplateColumn>
                    </Columns>
                  </asp:DataGrid>
                </td>
              </tr>
            </table>
            <asp:TextBox runat="server" ID="trans_tab_time" Style="display: none;" />
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="relationship_tab" runat="server" HeaderText="RELATIONSHIPS" Visible="true">
        <HeaderTemplate>
          RELATIONSHIPS
        </HeaderTemplate>
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label ID="relationship_warning" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
            <asp:Label ID="rel_warning_text" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
            <table width="100%" cellpadding="0" cellspacing="0">
              <tr>
                <td align="left" valign="top">
                  <asp:Label runat="server" ID="relationship_text"></asp:Label>
                </td>
                <td align="left" valign="top">
                  <asp:Label runat="server" ID="relationship_phone"></asp:Label>
                </td>
              </tr>
            </table>
            <asp:TextBox runat="server" ID="rel_tab_time" Style="display: none;" />
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="notes_tab" runat="server" HeaderText="NOTES" Visible="true">
        <ContentTemplate>
          <div class="tab_container_div" align="right">
            <table width="100%" cellspacing="0" cellpadding="2">
              <tr>
                <td align="center" valign="top">
                  <asp:Label runat="server" Text="" ID="notes_list" Width="100%"></asp:Label>
                  <asp:Label runat="server" Text="" ID="email_list" Width="100%"></asp:Label>
                </td>
              </tr>
            </table>
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="action_tab" runat="server" HeaderText="ACTION" Visible="true">
        <ContentTemplate>
          <div class="tab_container_div">
            <table width="100%" cellspacing="0" cellpadding="2">
              <tr>
                <td align="center" valign="top">
                  <asp:Panel ID="action_pnl" Width="100%" runat="server" HorizontalAlign="Right">
                    <asp:Label runat="server" ID="action_label"></asp:Label>
                  </asp:Panel>
                </td>
              </tr>
            </table>
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="opp_tab" runat="server" HeaderText="OPPORTUNITIES" Visible="true">
        <ContentTemplate>
          <div class="tab_container_div" align="right">
            <table width="100%" cellspacing="0" cellpadding="2">
              <tr>
                <td align="center" valign="top">
                  <asp:Label runat="server" Text="" ID="opp_list" Width="100%"></asp:Label>
                </td>
              </tr>
            </table>
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="wanted_tab" runat="server" HeaderText="WANTED(S)" Visible="true">
        <HeaderTemplate>
          WANTEDS
        </HeaderTemplate>
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label ID="wanted_warning_text" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
            <asp:Label ID="add_wanted" runat="server" Visible="false"><p align="left">&nbsp;<a href="#" onclick="javascript:load('edit_note.aspx?type=wanted&action=new','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');">Add Wanted</a>&nbsp;</p></asp:Label>
            <asp:DataGrid runat="server" ID="wanted_dg" CellPadding="17" horizontal-align="left"
              GridLines="None" BackColor="white" Width="100%" AllowPaging="True" PageSize="25"
              CssClass="tab_container_grid" BorderStyle="None" AllowSorting="True" AutoGenerateColumns="false">
              <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
                Font-Underline="True" ForeColor="White" Mode="NumericPages" NextPageText="Next"
                PrevPageText="Previous" />
              <AlternatingItemStyle CssClass="alt_row" />
              <ItemStyle BorderStyle="None" VerticalAlign="Top" />
              <HeaderStyle CssClass="aircraft_list" Font-Underline="false" ForeColor="white" Wrap="False"
                HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
              <Columns>
                <asp:TemplateColumn HeaderText="">
                  <ItemTemplate>
                    <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="20px" />
                    <%#clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "source"))%>
                  </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Date Listed">
                  <ItemTemplate>
                    <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="180px" />
                    <%#clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "amwant_listed_date")) %>
                  </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Make/Model">
                  <ItemTemplate>
                    <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="180px" />
                    <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%>&nbsp;<%#DataBinder.Eval(Container.DataItem, "amod_model_name")%>
                  </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Notes">
                  <ItemTemplate>
                    <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="180px" />
                    <%#IIf(DataBinder.Eval(Container.DataItem, "source") = "JETNET", DataBinder.Eval(Container.DataItem, "amwant_notes"), "<a href='#' onclick=""javascript:window.open('edit_note.aspx?action=edit&type=wanted&id=" & Eval("lnote_id") & "','','scrollbars=no,menubar=no,height=600,width=880,resizable=yes,toolbar=no,location=no,status=no');"">" & DataBinder.Eval(Container.DataItem, "amwant_notes") & "</a>")%>
                  </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Year Range">
                  <ItemTemplate>
                    <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="180px" />
                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "amwant_start_year")), DataBinder.Eval(Container.DataItem, "amwant_start_year") & " -", "")%>
                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "amwant_end_year")), DataBinder.Eval(Container.DataItem, "amwant_end_year"), "")%>
                  </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Max Price">
                  <ItemTemplate>
                    <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="180px" />
                    <%#DataBinder.Eval(Container.DataItem, "amwant_max_price")%>
                  </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Max AFTT">
                  <ItemTemplate>
                    <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="180px" />
                    <%#DataBinder.Eval(Container.DataItem, "amwant_max_aftt")%>
                  </ItemTemplate>
                </asp:TemplateColumn>
              </Columns>
            </asp:DataGrid>
            <asp:Label ID="wanted_label" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
            <asp:TextBox runat="server" ID="wanted_tab_time" Style="display: none;" />
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="certification_tab" runat="server" HeaderText="CERTIFICATIONS" Visible="true">
        <HeaderTemplate>
          CERTIFICATIONS
        </HeaderTemplate>
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label ID="cert_warning_text" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
            <asp:Label ID="cert_text" runat="server" Text=""></asp:Label>
            <asp:TextBox runat="server" ID="cert_tab_time" Style="display: none;" />
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="opportunities_tab" runat="server" HeaderText="DOCUMENTS" Visible="true">
        <ContentTemplate>
          <div class="tab_container_div">
            <table width="100%" cellspacing="0" cellpadding="2">
              <tr>
                <td align="center" valign="top">
                  <asp:Panel ID="Panel3" Width="100%" runat="server" HorizontalAlign="Right">
                    <asp:Label runat="server" ID="document_label"></asp:Label>
                  </asp:Panel>
                </td>
              </tr>
            </table>
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="prospect_tab" runat="server" HeaderText="PROSPECTS" Visible="true">
        <ContentTemplate>
          <div class="tab_container_div">
            <table width="100%" cellspacing="0" cellpadding="2">
              <tr>
                <td align="center" valign="top">
                  <asp:Panel ID="Panel4" Width="100%" runat="server" HorizontalAlign="Right">
                    <asp:Label runat="server" ID="prospect_list"></asp:Label>
                  </asp:Panel>
                </td>
              </tr>
            </table>
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
      <cc1:TabPanel ID="job_tab" runat="server" HeaderText="JOB QUALIFICATIONS" Visible="true">
        <ContentTemplate>
          <div class="tab_container_div">
            <asp:Label ID="job_warning_text" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
            <a href="#" onclick="javascript:load('http://www.jetadvisors.com/development/admin/seeker_submittal.asp?id=<% response.write (Session("ListingID")) %>&crm=true','scrollbars=no,menubar=no,height=500,width=1000,resizable=yes,toolbar=no,location=no,status=no');"
              class="float_right"><b>Edit/Approve Job Seeker</b>&nbsp;&nbsp;&nbsp;</a>
            <asp:Panel ID="Panel1" runat="server" Width="660px">
              <asp:Label runat="server" Text="" ID="resume_label"></asp:Label></asp:Panel>
            <asp:DataGrid runat="server" ID="Datagrid1" GridLines="Horizontal" CellPadding="3"
              HeaderStyle-BackColor="#204763" BackColor="White" Font-Size="8pt" Width="366px"
              AllowPaging="false" PageSize="60" CssClass="grid" BorderStyle="None" AllowSorting="True"
              Font-Names="tahoma" AllowCustomPaging="True" AutoGenerateColumns="true">
              <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
                Font-Underline="True" ForeColor="White" />
              <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="#C6C6C6" HorizontalAlign="Left" />
              <HeaderStyle BackColor="#67A0D9" Font-Bold="True" Font-Size="9" Font-Underline="True"
                ForeColor="White" Wrap="False" HorizontalAlign="left" VerticalAlign="Middle" Height="20px">
              </HeaderStyle>
            </asp:DataGrid>
            <asp:TextBox runat="server" ID="job_tab_time" Style="display: none;" />
          </div>
        </ContentTemplate>
      </cc1:TabPanel>
    </cc1:TabContainer>
  </ContentTemplate>
</asp:UpdatePanel>
   <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="bottom_tab_update_panel"
        runat="server" DisplayAfter="500" Visible="true">
        <ProgressTemplate>
          <div id="divTabLoading" runat="server" class="loadingScreenBox" align="center">
            <img src="Images/loading.gif" alt="Loading..." />
          </div>
        </ProgressTemplate>
      </asp:UpdateProgress>
<script type="text/javascript">
  function PanelClick(sender, e) {
  }

  function ActiveTabChanged(sender, args) {
    //  see if the table elements for the grids exist yet
    //var isTab10Loaded = $get('transaction_gv');
    var tab = sender.get_activeTab();
    var idTEXT = tab.get_id();
    //ctl00_ContentPlaceHolder1_Company_Tabs1_tabs_container_notes_tab

    var mySplitResult = idTEXT.split("tabs_container_");
    //alert(mySplitResult[1]);

    // createCookie('ppkcookie', idTEXT, 7);


  }

  function createCookie(name, value, days) {
    if (days) {
      var date = new Date();
      date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
      var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
  }
  function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
      var c = ca[i];
      while (c.charAt(0) == ' ') c = c.substring(1, c.length);
      if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
  }


</script>

