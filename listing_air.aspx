<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="listing.aspx.vb" Inherits="crmWebClient._listing"
  MasterPageFile="~/main_site.Master" EnableViewState="true" %>

<%@ Import Namespace="crmWebClient.clsGeneral" %>
<%@ MasterType VirtualPath="~/main_site.Master" %>
<%@ Register TagPrefix="obout" Namespace="OboutInc.Flyout2" Assembly="obout_Flyout2_NET" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:DataGrid runat="server" ID="Results" CellPadding="3" OnItemCommand="dispDetails"
    Width="100%" AllowPaging="true" PageSize="25" EnableViewState="true" AllowSorting="True"
    AutoGenerateColumns="false" PagerStyle-Mode="NumericPages" GridLines="None" CssClass="mGrid"
    PagerStyle-CssClass="pgr" AlternatingItemStyle-CssClass="alt" ItemStyle-CssClass="item_row"
    ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="th">
    <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" ForeColor="White" />
    <AlternatingItemStyle CssClass="alt_row smaller" />
    <ItemStyle BorderStyle="None" VerticalAlign="Top" CssClass="smaller" />
    <HeaderStyle Wrap="False" HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
    <Columns>
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="10px" />
          <input onclick="javascript:append_cookie('<%#(DataBinder.Eval(Container.DataItem, "other_ac_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "other_source"))%>','aircraft_marked');"
            type="checkbox" id='<%#(DataBinder.Eval(Container.DataItem, "other_ac_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "other_source"))%>'
            value='<%#(DataBinder.Eval(Container.DataItem, "other_ac_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "other_source"))%>'
            style="<%#master.IsInCookie(DataBinder.Eval(Container.DataItem, "other_ac_id") & "#" & DataBinder.Eval(Container.DataItem, "other_source"))%>" />
          <br />
          <input onclick="javascript:append_cookie('<%#(DataBinder.Eval(Container.DataItem, "ac_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "source"))%>','aircraft_marked');"
            type="checkbox" id='<%#(DataBinder.Eval(Container.DataItem, "ac_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "source"))%>'
            value='<%#(DataBinder.Eval(Container.DataItem, "ac_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "source"))%>'
            style="<%#master.IsInCookie(DataBinder.Eval(Container.DataItem, "ac_id") & "#" & DataBinder.Eval(Container.DataItem, "source"))%>" />
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <%#clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "other_source"))%><br />
          <%#clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "source"))%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:BoundColumn DataField="other_source" Visible="false" />
      <asp:BoundColumn DataField="source" Visible="false" />
      <asp:BoundColumn DataField="ac_id" Visible="false" />
      <asp:BoundColumn DataField="other_ac_id" Visible="false" />
      <asp:TemplateColumn HeaderText="Year<br />AFTT">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <table width="100%" cellpadding="0" cellspacing="0" class="none">
            <tr>
              <td align="left" valign="top" width="1">
                <img src="images/spacer.gif" alt="" height="75" width="1" />
              </td>
              <td align="left" valign="top">
                <%#clsGeneral.difference_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_year_mfr"), DataBinder.Eval(Container.DataItem, "other_source"), DataBinder.Eval(Container.DataItem, "ac_year_mfr"), DataBinder.Eval(Container.DataItem, "source"), "", "")%>
              </td>
            </tr>
          </table>
          <%#clsGeneral.difference_ac_listing(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_ac_airframe_tot_hrs")), Master.DisplayAFTT("<span class='help_cursor' title='Airframe Total Time'>" & DataBinder.Eval(Container.DataItem, "other_ac_airframe_tot_hrs") & "</span>", False, DataBinder.Eval(Container.DataItem, "other_ac_airframe_tot_hrs")), ""), DataBinder.Eval(Container.DataItem, "other_source"), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs")), Master.DisplayAFTT("<span class='help_cursor' title='Airframe Total Time'>" & DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs") & "</span>", False, DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs")), ""), DataBinder.Eval(Container.DataItem, "source"), "", "")%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Model<br />Engine TT">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <table width="100%" cellpadding="0" cellspacing="0" class="none smaller">
            <tr>
              <td align="left" valign="top">
                <img src="images/spacer.gif" alt="" height="75" width="1" />
              </td>
              <td align="left" valign="top">
                <%#clsGeneral.isitnull(DataBinder.Eval(Container.DataItem, "amod_make_name"))%>&nbsp;<%#clsGeneral.isitnull(DataBinder.Eval(Container.DataItem, "amod_model_name"))%>
              </td>
            </tr>
          </table>
          <%# clsGeneral.showEngineLabel("ENG1: ", DataBinder.Eval(Container.DataItem, "acep_engine_1_tsoh_hours"),DataBinder.Eval(Container.DataItem, "other_acep_engine_1_tsoh_hours"),DataBinder.Eval(Container.DataItem, "acep_engine_1_ttsn_hours"), DataBinder.Eval(Container.DataItem, "other_acep_engine_1_ttsn_hours"),nothing, nothing)%><%#Replace(clsGeneral.difference_ac_listing(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_1_ttsn_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG1 TTSNEW Hrs (Total Time Since New)'>" & DataBinder.Eval(Container.DataItem, "other_acep_engine_1_ttsn_hours") & "</span>", False, DataBinder.Eval(Container.DataItem, "other_acep_engine_1_ttsn_hours")), ""), DataBinder.Eval(Container.DataItem, "other_source"), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_1_ttsn_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG1 TTSNEW Hrs (Total Time Since New)'>" & DataBinder.Eval(Container.DataItem, "acep_engine_1_ttsn_hours") & "</span>", False, DataBinder.Eval(Container.DataItem, "acep_engine_1_ttsn_hours")), ""), DataBinder.Eval(Container.DataItem, "source"), "", ""), "<br />", "/")%>
          <%# clsGeneral.showEngineLabel("ENG2: ", DataBinder.Eval(Container.DataItem, "acep_engine_2_tsoh_hours"),DataBinder.Eval(Container.DataItem, "other_acep_engine_2_tsoh_hours"),DataBinder.Eval(Container.DataItem, "acep_engine_2_ttsn_hours"), DataBinder.Eval(Container.DataItem, "other_acep_engine_2_ttsn_hours"),nothing, nothing)%><%#Replace(clsGeneral.difference_ac_listing(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_2_ttsn_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG2 TTSNEW Hrs (Total Time Since New)'>" & DataBinder.Eval(Container.DataItem, "other_acep_engine_2_ttsn_hours") & "</span>", False, DataBinder.Eval(Container.DataItem, "other_acep_engine_2_ttsn_hours")), ""), DataBinder.Eval(Container.DataItem, "other_source"), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_2_ttsn_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG2 TTSNEW Hrs (Total Time Since New)'>" & DataBinder.Eval(Container.DataItem, "acep_engine_2_ttsn_hours") & "</span>", False, DataBinder.Eval(Container.DataItem, "acep_engine_2_ttsn_hours")), ""), DataBinder.Eval(Container.DataItem, "source"), "", ""), "<br />", "/")%>
          <%# clsGeneral.showEngineLabel("ENG3: ", DataBinder.Eval(Container.DataItem, "acep_engine_3_tsoh_hours"),DataBinder.Eval(Container.DataItem, "other_acep_engine_3_tsoh_hours"),DataBinder.Eval(Container.DataItem, "acep_engine_3_ttsn_hours"), DataBinder.Eval(Container.DataItem, "other_acep_engine_3_ttsn_hours"),nothing, nothing)%><%#Replace(clsGeneral.difference_ac_listing(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_3_ttsn_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG3 TTSNEW Hrs (Total Time Since New)'>" & DataBinder.Eval(Container.DataItem, "other_acep_engine_3_ttsn_hours") & "</span>", False, DataBinder.Eval(Container.DataItem, "other_acep_engine_3_ttsn_hours")), ""), DataBinder.Eval(Container.DataItem, "other_source"), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_3_ttsn_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG3 TTSNEW Hrs (Total Time Since New)'>" & DataBinder.Eval(Container.DataItem, "acep_engine_3_ttsn_hours") & "</span>", False, DataBinder.Eval(Container.DataItem, "acep_engine_3_ttsn_hours")), ""), DataBinder.Eval(Container.DataItem, "source"), "", ""), "<br />", "/")%>
          <%# clsGeneral.showEngineLabel("ENG4: ", DataBinder.Eval(Container.DataItem, "acep_engine_4_tsoh_hours"),DataBinder.Eval(Container.DataItem, "other_acep_engine_4_tsoh_hours"),DataBinder.Eval(Container.DataItem, "acep_engine_4_ttsn_hours"), DataBinder.Eval(Container.DataItem, "other_acep_engine_4_ttsn_hours"),nothing, nothing)%><%#Replace(clsGeneral.difference_ac_listing(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_4_ttsn_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG4 TTSNEW Hrs (Total Time Since New)'>" & DataBinder.Eval(Container.DataItem, "other_acep_engine_4_ttsn_hours") & "</span>", False, DataBinder.Eval(Container.DataItem, "other_acep_engine_4_ttsn_hours")), ""), DataBinder.Eval(Container.DataItem, "other_source"), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_4_ttsn_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG4 TTSNEW Hrs (Total Time Since New)'>" & DataBinder.Eval(Container.DataItem, "acep_engine_4_ttsn_hours") & "</span>", False, DataBinder.Eval(Container.DataItem, "acep_engine_4_ttsn_hours")), ""), DataBinder.Eval(Container.DataItem, "source"), "", ""), "<br />", "/")%>
          </span>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Ser #<br />SMOH">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <table width="100%" cellpadding="0" cellspacing="0" class="none">
            <tr>
              <td align="left" valign="top">
                <img src="images/spacer.gif" alt="" height="75" width="1" />
              </td>
              <td align="left" valign="top">
                <%#clsGeneral.isitnull(DataBinder.Eval(Container.DataItem, "other_ac_ser_nbr"))%><br />
                <%#clsGeneral.isitnull(DataBinder.Eval(Container.DataItem, "ac_ser_nbr"))%>
              </td>
            </tr>
          </table>
                   <div style="margin-top: 0px;padding-left:9px;">
            <%#Replace(clsGeneral.difference_ac_listing(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_1_tsoh_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG1 SOH/SCOR Hrs (Since Overhaul)'>" & DataBinder.Eval(Container.DataItem, "other_acep_engine_1_tsoh_hours"), False, DataBinder.Eval(Container.DataItem, "other_acep_engine_1_tsoh_hours")), ""), DataBinder.Eval(Container.DataItem, "other_source"), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_1_tsoh_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG1 SOH/SCOR Hrs (Since Overhaul)'>" & DataBinder.Eval(Container.DataItem, "acep_engine_1_tsoh_hours") & "", False, DataBinder.Eval(Container.DataItem, "acep_engine_1_tsoh_hours")), ""), DataBinder.Eval(Container.DataItem, "source"), "", ""), "<br />", "/")%>
            <%#Replace(clsGeneral.difference_ac_listing(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_2_tsoh_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG2 SOH/SCOR Hrs (Since Overhaul)'>" & DataBinder.Eval(Container.DataItem, "other_acep_engine_2_tsoh_hours"), False, DataBinder.Eval(Container.DataItem, "other_acep_engine_2_tsoh_hours")), ""), DataBinder.Eval(Container.DataItem, "other_source"), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_2_tsoh_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG2 SOH/SCOR Hrs (Since Overhaul)'>" & DataBinder.Eval(Container.DataItem, "acep_engine_2_tsoh_hours") & "", False, DataBinder.Eval(Container.DataItem, "acep_engine_2_tsoh_hours")), ""), DataBinder.Eval(Container.DataItem, "source"), "", ""), "<br />", "/")%>
            <%#Replace(clsGeneral.difference_ac_listing(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_3_tsoh_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG3 SOH/SCOR Hrs (Since Overhaul)'>" & DataBinder.Eval(Container.DataItem, "other_acep_engine_3_tsoh_hours"), False, DataBinder.Eval(Container.DataItem, "other_acep_engine_3_tsoh_hours")), ""), DataBinder.Eval(Container.DataItem, "other_source"), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_3_tsoh_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG3 SOH/SCOR Hrs (Since Overhaul)'>" & DataBinder.Eval(Container.DataItem, "acep_engine_3_tsoh_hours") & "", False, DataBinder.Eval(Container.DataItem, "acep_engine_3_tsoh_hours")), ""), DataBinder.Eval(Container.DataItem, "source"), "", ""), "<br />", "/")%>
            <%#Replace(clsGeneral.difference_ac_listing(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_4_tsoh_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG4 SOH/SCOR Hrs (Since Overhaul)'>" & DataBinder.Eval(Container.DataItem, "other_acep_engine_4_tsoh_hours"), False, DataBinder.Eval(Container.DataItem, "other_acep_engine_4_tsoh_hours")), ""), DataBinder.Eval(Container.DataItem, "other_source"), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_4_tsoh_hours")), Master.DisplayAFTT("<span class='help_cursor' title='ENG4 SOH/SCOR Hrs (Since Overhaul)'>" & DataBinder.Eval(Container.DataItem, "acep_engine_4_tsoh_hours") & "", False, DataBinder.Eval(Container.DataItem, "acep_engine_4_tsoh_hours")), ""), DataBinder.Eval(Container.DataItem, "source"), "", ""), "<br />", "/")%>
          </span>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Reg #<br />Updated<br />SHI">
        <ItemTemplate>
          <table width="100%" cellpadding="0" cellspacing="0" class="none smaller">
            <tr>
              <td align="left" valign="top" width="1" rowspan="2">
                <img src="images/spacer.gif" alt="" height="75" width="1" />
              </td>
              <td align="right" valign="top">
                <%#clsGeneral.difference_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_reg_nbr"), DataBinder.Eval(Container.DataItem, "other_source"), DataBinder.Eval(Container.DataItem, "ac_reg_nbr"), DataBinder.Eval(Container.DataItem, "source"), "", "")%>
              </td>
            </tr> 
            <tr>
              <td align="right" valign="top">
                <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_source")), "<span class='jetnet_row'>", IIf(DataBinder.Eval(Container.DataItem, "source") = "CLIENT", "<span class='client_row'>", "<span class='jetnet_row'>"))%>
                <%#clsGeneral.FormatDateShorthand(clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "other_ac_upd_date")))%></span>
                <br />
                <span class="client_row">
                  <%#clsGeneral.FormatDateShorthand(clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "ac_upd_date")))%></span>
            </tr>
          </table>
            <%#Replace(clsGeneral.difference_ac_listing(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_ac_engine_1_shi_hrs")), Master.DisplayAFTT("<span class='help_cursor' title='ENG1 SHI/SMPI Hrs (Since Hot Inspection)'>" & DataBinder.Eval(Container.DataItem, "other_ac_engine_1_shi_hrs") & "", False, DataBinder.Eval(Container.DataItem, "other_ac_engine_1_shi_hrs")), ""), DataBinder.Eval(Container.DataItem, "other_source"), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_shi_hrs")), Master.DisplayAFTT("<span class='help_cursor' title='ENG1 SHI/SMPI Hrs (Since Hot Inspection)'>" & DataBinder.Eval(Container.DataItem, "ac_engine_1_shi_hrs") & "", False, DataBinder.Eval(Container.DataItem, "ac_engine_1_shi_hrs")), ""), DataBinder.Eval(Container.DataItem, "source"), "", ""), "<br />", "/")%>
            <%#Replace(clsGeneral.difference_ac_listing(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_ac_engine_2_shi_hrs")), Master.DisplayAFTT("<span class='help_cursor' title='ENG2 SHI/SMPI Hrs (Since Hot Inspection)'>" & DataBinder.Eval(Container.DataItem, "other_ac_engine_2_shi_hrs") & "", False, DataBinder.Eval(Container.DataItem, "other_ac_engine_2_shi_hrs")), ""), DataBinder.Eval(Container.DataItem, "other_source"), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_shi_hrs")), Master.DisplayAFTT("<span class='help_cursor' title='ENG2 SHI/SMPI Hrs (Since Hot Inspection)'>" & DataBinder.Eval(Container.DataItem, "ac_engine_2_shi_hrs") & "", False, DataBinder.Eval(Container.DataItem, "ac_engine_2_shi_hrs")), ""), DataBinder.Eval(Container.DataItem, "source"), "", ""), "<br />", "/")%>
            <%#Replace(clsGeneral.difference_ac_listing(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_ac_engine_3_shi_hrs")), Master.DisplayAFTT("<span class='help_cursor' title='ENG3 SHI/SMPI Hrs (Since Hot Inspection)'>" & DataBinder.Eval(Container.DataItem, "other_ac_engine_3_shi_hrs") & "", False, DataBinder.Eval(Container.DataItem, "other_ac_engine_3_shi_hrs")), ""), DataBinder.Eval(Container.DataItem, "other_source"), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_shi_hrs")), Master.DisplayAFTT("<span class='help_cursor' title='ENG3 SHI/SMPI Hrs (Since Hot Inspection)'>" & DataBinder.Eval(Container.DataItem, "ac_engine_3_shi_hrs") & "", False, DataBinder.Eval(Container.DataItem, "ac_engine_3_shi_hrs")), ""), DataBinder.Eval(Container.DataItem, "source"), "", ""), "<br />", "/")%>
            <%#Replace(clsGeneral.difference_ac_listing(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_ac_engine_4_shi_hrs")), Master.DisplayAFTT("<span class='help_cursor' title='ENG4 SHI/SMPI Hrs (Since Hot Inspection)'>" & DataBinder.Eval(Container.DataItem, "other_ac_engine_4_shi_hrs") & "", False, DataBinder.Eval(Container.DataItem, "other_ac_engine_4_shi_hrs")), ""), DataBinder.Eval(Container.DataItem, "other_source"), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_shi_hrs")), Master.DisplayAFTT("<span class='help_cursor' title='ENG4 SHI/SMPI Hrs (Since Hot Inspection)'>" & DataBinder.Eval(Container.DataItem, "ac_engine_4_shi_hrs") & "", False, DataBinder.Eval(Container.DataItem, "ac_engine_4_shi_hrs")), ""), DataBinder.Eval(Container.DataItem, "source"), "", ""), "<br />", "/")%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Company">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <asp:Panel ID="company_hold" runat="server">
          </asp:Panel>
          <%Master.Build_Aircraft_Company_Listings()%>
          <%#Master.createExclusiveBroker(Eval("ac_ser_nbr"), 0, Eval("source"), Eval("ac_id"), DataBinder.Eval(Container.DataItem, "ac_exclusive_flag"), DataBinder.Eval(Container.DataItem, "other_ac_exclusive_flag"))%>
          <img src="images/spacer.gif" width="150" height="1" alt="" />
          <headerstyle width="20px" />
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Listed" ItemStyle-VerticalAlign="Top">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle horizontalalign="center" />
          <%#clsGeneral.difference_ac_listing(clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "other_ac_date_listed")), DataBinder.Eval(Container.DataItem, "other_source"), clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "ac_date_listed")), DataBinder.Eval(Container.DataItem, "source"), "", "")%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Asking $" ItemStyle-VerticalAlign="Top">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle horizontalalign="center" />
          <%#clsGeneral.difference_ac_listing_AskingPrice(DataBinder.Eval(Container.DataItem, "other_ac_forsale_flag"), DataBinder.Eval(Container.DataItem, "ac_forsale_flag"), clsGeneral.ConvertIntoThousands(DataBinder.Eval(Container.DataItem, "other_ac_asking_price")), DataBinder.Eval(Container.DataItem, "other_source"), clsGeneral.ConvertIntoThousands(DataBinder.Eval(Container.DataItem, "ac_asking_price")), DataBinder.Eval(Container.DataItem, "source"), DataBinder.Eval(Container.DataItem, "other_ac_asking_wordage"), DataBinder.Eval(Container.DataItem, "ac_asking_wordage"), "", "")%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Take $" ItemStyle-VerticalAlign="Top">
        <ItemTemplate>
          <%#clsGeneral.ConvertIntoThousands(DataBinder.Eval(Container.DataItem, "ac_est_price"))%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Status" ItemStyle-Width="90px">
        <ItemTemplate>
          <%#clsGeneral.price_difference_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_status"), DataBinder.Eval(Container.DataItem, "other_source"), DataBinder.Eval(Container.DataItem, "ac_status"), DataBinder.Eval(Container.DataItem, "source"), DataBinder.Eval(Container.DataItem, "other_ac_forsale_flag"), DataBinder.Eval(Container.DataItem, "ac_forsale_flag"), "", "")%>
          <span class="client_row">
            <%#clsGeneral.DisplayTextShorthand(DataBinder.Eval(Container.DataItem, "value_price"))%></span>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <itemstyle horizontalalign="center" verticalalign="top" />
          <headerstyle horizontalalign="center" />
          <div align="center">
            <%#clsGeneral.colormelease_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_lease_flag"), DataBinder.Eval(Container.DataItem, "ac_lease_flag"), False)%>
            <asp:Label runat="server" ID="popup_ex">
                        <%#clsGeneral.colormeex_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_exclusive_flag"), DataBinder.Eval(Container.DataItem, "ac_exclusive_flag"), False)%></asp:Label>
          </div>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <itemstyle horizontalalign="center" verticalalign="top" />
          <headerstyle horizontalalign="center" />
          <% If Session.Item("crmUserLogon") = True Then%>
          <% If Session.Item("localUser").crmEvo <> True Then%>
          <% Master.noteactext = ""%>
          <%#Master.ViewNoteAttachedACComp(IIf(Not IsDBNull(Eval("other_ac_id")), Eval("other_ac_id"), Eval("ac_id")), IIf(Not IsDBNull(Eval("other_source")), Eval("other_source"), Eval("source")), 2, "A", 0, Eval("lastnote"))%>
          <div align="center">
            <asp:LinkButton runat="server" ID="viewnote" OnClientClick="return false;">
                       <%#IIf(Master.noteactext <> "", "<img src='images/document.png' alt='Notes Attached to Aircraft' border='0' />", "")%>
            </asp:LinkButton></div>
          <obout:Flyout ID="note_fly" runat="server" AttachTo="viewnote" Position="TOP_LEFT"
            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true">
            <%#clsGeneral.MouseOverTextStart() %>
            <%#Master.noteactext%>
            <%#clsGeneral.MouseOverTextEnd()%>
          </obout:Flyout>
          <obout:Flyout ID="Flyout1" runat="server" AttachTo="popup_ex" Position="TOP_LEFT"
            Align="LEFT" FadingEffect="true" Visible='<%#Master.broker %>'>
          </obout:Flyout>
          <a href="#" style="font-size: 9px;" onclick="javascript:load('edit_note.aspx?ac_ID=<%# IIf(Not IsDBNull(Eval("other_ac_id")), Eval("other_ac_id"), Eval("ac_id")) %>&source=<%# IIf(Not IsDBNull(Eval("other_source")), Eval("other_source"), Eval("source")) %>&type=note&action=new','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');">
            [+]</a>
          <% End If%>
          <% End If%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <itemstyle horizontalalign="center" verticalalign="top" />
          <headerstyle horizontalalign="center" />
          <div align="center">
            <%#Master.ViewPriorityEventsClient(Eval("lastevent"), Eval("jetnet_ac_id"))%>
            <asp:LinkButton runat="server" ID="viewevent" OnClientClick="return false;">
                       <%#IIf(Master.eventactext <> "", "<img src='images/light.png' alt='Recent Events Attached to Aircraft' border='0' />", "")%>
            </asp:LinkButton></div>
          <obout:Flyout ID="event_fly" runat="server" AttachTo="viewevent" Position="TOP_LEFT"
            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true">
            <%#clsGeneral.MouseOverTextStart() %>
            <%#Master.eventactext%>
            <%#clsGeneral.MouseOverTextEnd()%>
          </obout:Flyout>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <itemstyle horizontalalign="center" verticalalign="top" />
          <headerstyle horizontalalign="center" />
          <% Master.noteactext = ""%>
          <%#Master.ViewNoteAttachedACComp(Eval("ac_id"), Eval("source"), 2, "P", 0, Eval("lastnote"))%>
          <div align="center">
            <asp:LinkButton runat="server" ID="viewaction" OnClientClick="return false;">
                       <%#IIf(Master.noteactext <> "", "<img src='images/red_pin.png' alt='Actions Attached to Aircraft' border='0' />", "")%>
            </asp:LinkButton></div>
          <obout:Flyout ID="action_fly" runat="server" AttachTo="viewaction" Position="TOP_LEFT"
            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true">
            <%#clsGeneral.MouseOverTextStart() %>
            <%#Master.noteactext%>
            <%#clsGeneral.MouseOverTextEnd()%>
          </obout:Flyout>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:BoundColumn DataField="comp_name" Visible="false" />
      <asp:BoundColumn DataField="comp_address1" Visible="false" />
      <asp:BoundColumn DataField="comp_address2" Visible="false" />
      <asp:BoundColumn DataField="comp_city" Visible="false" />
      <asp:BoundColumn DataField="comp_state" Visible="false" />
      <asp:BoundColumn DataField="comp_country" Visible="false" />
      <asp:BoundColumn DataField="comp_zip_code" Visible="false" />
      <asp:BoundColumn DataField="comp_email_address" Visible="false" />
      <asp:BoundColumn DataField="comp_web_address" Visible="false" />
      <asp:BoundColumn DataField="contact_first_name" Visible="false" />
      <asp:BoundColumn DataField="contact_last_name" Visible="false" />
      <asp:BoundColumn DataField="contact_middle_initial" Visible="false" />
      <asp:BoundColumn DataField="contact_title" Visible="false" />
      <asp:BoundColumn DataField="contact_preferred_name" Visible="false" />
      <asp:BoundColumn DataField="contact_notes" Visible="false" />
      <asp:BoundColumn DataField="contact_email_address" Visible="false" />
      <asp:BoundColumn DataField="comp_source" Visible="false" />
      <asp:BoundColumn DataField="act_name" Visible="false" />
      <asp:BoundColumn DataField="comp_id" Visible="false" />
      <asp:BoundColumn DataField="contact_id" Visible="false" />
      <asp:BoundColumn DataField="acref_owner_percentage" Visible="false" />
    </Columns>
  </asp:DataGrid>
  <asp:DataGrid runat="server" ID="Results___" CellPadding="3" HeaderStyle-BackColor="#204763"
    OnItemCommand="dispDetails" BackColor="White" Font-Name="tahoma" Font-Size="8pt"
    Width="825px" AllowPaging="true" PageSize="25" CssClass="grid" EnableViewState="true"
    AllowSorting="True" Font-Names="verdana" AutoGenerateColumns="false" BorderColor="#BCC9D6"
    Visible="false" PagerStyle-Mode="NumericPages" BorderStyle="Solid" BorderWidth="1px">
    <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
      Font-Underline="True" ForeColor="White" />
    <AlternatingItemStyle CssClass="alt_row" />
    <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="#eeeeee" />
    <HeaderStyle BackColor="#67A0D9" Font-Bold="True" Font-Size="10" CssClass="smaller"
      Font-Underline="True" ForeColor="White" Wrap="False" HorizontalAlign="left" VerticalAlign="top">
    </HeaderStyle>
    <Columns>
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="10px" />
          <input onclick="javascript:append_cookie('<%#(DataBinder.Eval(Container.DataItem, "other_ac_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "other_source"))%>','aircraft_marked');"
            type="checkbox" id='<%#(DataBinder.Eval(Container.DataItem, "other_ac_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "other_source"))%>'
            value='<%#(DataBinder.Eval(Container.DataItem, "other_ac_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "other_source"))%>'
            style="<%#master.IsInCookie(DataBinder.Eval(Container.DataItem, "other_ac_id") & "#" & DataBinder.Eval(Container.DataItem, "other_source"))%>" />
          <br />
          <input onclick="javascript:append_cookie('<%#(DataBinder.Eval(Container.DataItem, "ac_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "source"))%>','aircraft_marked');"
            type="checkbox" id='<%#(DataBinder.Eval(Container.DataItem, "ac_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "source"))%>'
            value='<%#(DataBinder.Eval(Container.DataItem, "ac_id"))%>#<%#(DataBinder.Eval(Container.DataItem, "source"))%>'
            style="<%#master.IsInCookie(DataBinder.Eval(Container.DataItem, "ac_id") & "#" & DataBinder.Eval(Container.DataItem, "source"))%>" />
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <%#clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "other_source"))%><br />
          <%#clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "source"))%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:BoundColumn DataField="comp_id" Visible="false" />
      <asp:BoundColumn DataField="source" Visible="false" />
      <asp:BoundColumn DataField="contact_id" Visible="false" />
      <asp:BoundColumn DataField="ac_id" Visible="false" />
      <asp:TemplateColumn HeaderText="Year<br />AFTT">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <table width="100%" cellpadding="0" cellspacing="0" class="none">
            <tr>
              <td align="left" valign="top" width="1">
                <img src="images/spacer.gif" alt="" height="40" width="1" />
              </td>
              <td align="left" valign="top">
                <%#clsGeneral.difference_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_year_mfr"), DataBinder.Eval(Container.DataItem, "other_source"), DataBinder.Eval(Container.DataItem, "ac_year_mfr"), DataBinder.Eval(Container.DataItem, "source"), "", "")%>
              </td>
            </tr>
          </table>
          <span class="jetnet_row">
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_ac_airframe_tot_hrs")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_ac_airframe_tot_hrs") & "]", False, DataBinder.Eval(Container.DataItem, "other_ac_airframe_tot_hrs")), "")%>
          </span>
          <br />
          <span class="client_row">
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs") & "]", False, DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs")), "")%></span>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Model<br />Engine TT">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <table width="100%" cellpadding="0" cellspacing="0" class="none">
            <tr>
              <td align="left" valign="top">
                <img src="images/spacer.gif" alt="" height="40" width="1" />
              </td>
              <td align="left" valign="top">
                <%#clsGeneral.isitnull(DataBinder.Eval(Container.DataItem, "amod_make_name"))%>&nbsp;<%#clsGeneral.isitnull(DataBinder.Eval(Container.DataItem, "amod_model_name"))%>
              </td>
            </tr>
          </table>
          <span class="jetnet_row">
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_1_ttsn_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_1_ttsn_hours") & "]", False, DataBinder.Eval(Container.DataItem, "other_acep_engine_1_ttsn_hours")), "")%>
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_2_ttsn_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_2_ttsn_hours") & "]", False, DataBinder.Eval(Container.DataItem, "other_acep_engine_2_ttsn_hours")), "")%>
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_3_ttsn_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_3_ttsn_hours") & "]", False, DataBinder.Eval(Container.DataItem, "other_acep_engine_3_ttsn_hours")), "")%>
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_4_ttsn_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_4_ttsn_hours") & "]", False, DataBinder.Eval(Container.DataItem, "other_acep_engine_4_ttsn_hours")), "")%>
          </span>
          <br />
          <span class="client_row">
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_1_ttsn_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_1_ttsn_hours") & "]", False, DataBinder.Eval(Container.DataItem, "acep_engine_1_ttsn_hours")), "")%>
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_2_ttsn_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_2_ttsn_hours") & "]", False, DataBinder.Eval(Container.DataItem, "acep_engine_2_ttsn_hours")), "")%>
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_3_ttsn_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_3_ttsn_hours") & "]", False, DataBinder.Eval(Container.DataItem, "acep_engine_3_ttsn_hours")), "")%>
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_4_ttsn_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_4_ttsn_hours") & "]", False, DataBinder.Eval(Container.DataItem, "acep_engine_4_ttsn_hours")), "")%>
          </span>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Ser #<br />SMOH">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <table width="100%" cellpadding="0" cellspacing="0" class="none">
            <tr>
              <td align="left" valign="top">
                <img src="images/spacer.gif" alt="" height="40" width="1" />
              </td>
              <td align="left" valign="top">
                <%#clsGeneral.isitnull(DataBinder.Eval(Container.DataItem, "other_ac_ser_nbr"))%><br />
                <%#clsGeneral.isitnull(DataBinder.Eval(Container.DataItem, "ac_ser_nbr"))%>
              </td>
            </tr>
          </table>
          <span class="jetnet_row">
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_1_tsoh_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_1_tsoh_hours") & "]", False, DataBinder.Eval(Container.DataItem, "other_acep_engine_1_tsoh_hours")), "")%>
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_2_tsoh_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_2_tsoh_hours") & "]", False, DataBinder.Eval(Container.DataItem, "other_acep_engine_2_tsoh_hours")), "")%>
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_3_tsoh_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_3_tsoh_hours") & "]", False, DataBinder.Eval(Container.DataItem, "other_acep_engine_3_tsoh_hours")), "")%>
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "other_acep_engine_4_tsoh_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "other_acep_engine_4_tsoh_hours") & "]", False, DataBinder.Eval(Container.DataItem, "other_acep_engine_4_tsoh_hours")), "")%>
          </span>
          <br />
          <span class="client_row">
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_1_tsoh_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_1_tsoh_hours") & "]", False, DataBinder.Eval(Container.DataItem, "acep_engine_1_tsoh_hours")), "")%>
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_2_tsoh_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_2_tsoh_hours") & "]", False, DataBinder.Eval(Container.DataItem, "acep_engine_2_tsoh_hours")), "")%>
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_3_tsoh_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_3_tsoh_hours") & "]", False, DataBinder.Eval(Container.DataItem, "acep_engine_3_tsoh_hours")), "")%>
            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "acep_engine_4_tsoh_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "acep_engine_4_tsoh_hours") & "]", False, DataBinder.Eval(Container.DataItem, "acep_engine_4_tsoh_hours")), "")%>
          </span>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Reg #">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <%#clsGeneral.difference_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_reg_nbr"), DataBinder.Eval(Container.DataItem, "other_source"), DataBinder.Eval(Container.DataItem, "ac_reg_nbr"), DataBinder.Eval(Container.DataItem, "source"), "", "")%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Company">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <asp:Panel ID="company_hold" runat="server">
          </asp:Panel>
          <%#Master.createExclusiveBroker(Eval("ac_ser_nbr"), 0, Eval("source"), Eval("ac_id"), DataBinder.Eval(Container.DataItem, "ac_exclusive_flag"), DataBinder.Eval(Container.DataItem, "other_ac_exclusive_flag"))%>
          <headerstyle width="20px" />
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Listed">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle horizontalalign="center" />
          <%#clsGeneral.difference_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_date_listed"), DataBinder.Eval(Container.DataItem, "other_source"), DataBinder.Eval(Container.DataItem, "ac_date_listed"), DataBinder.Eval(Container.DataItem, "source"), "", "")%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Asking $">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle horizontalalign="center" />
          <%#clsGeneral.difference_ac_listing(clsGeneral.ConvertIntoThousands(DataBinder.Eval(Container.DataItem, "other_ac_asking_price")), DataBinder.Eval(Container.DataItem, "other_source"), clsGeneral.ConvertIntoThousands(DataBinder.Eval(Container.DataItem, "ac_asking_price")), DataBinder.Eval(Container.DataItem, "source"), "", "")%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Take $">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle horizontalalign="center" />
          <br />
          <%#clsGeneral.ConvertIntoThousands(DataBinder.Eval(Container.DataItem, "ac_est_price"))%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Status">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle horizontalalign="center" />
          <%#clsGeneral.price_difference_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_status"), DataBinder.Eval(Container.DataItem, "other_source"), DataBinder.Eval(Container.DataItem, "ac_status"), DataBinder.Eval(Container.DataItem, "source"), DataBinder.Eval(Container.DataItem, "other_ac_forsale_flag"), DataBinder.Eval(Container.DataItem, "ac_forsale_flag"), "", "")%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <itemstyle horizontalalign="center" verticalalign="top" />
          <headerstyle horizontalalign="center" />
          <div align="center">
            <%#clsGeneral.colormelease_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_lease_flag"), DataBinder.Eval(Container.DataItem, "ac_lease_flag"), False)%>
            <asp:Label runat="server" ID="popup_ex">
                        <%#clsGeneral.colormeex_ac_listing(DataBinder.Eval(Container.DataItem, "other_ac_exclusive_flag"), DataBinder.Eval(Container.DataItem, "ac_exclusive_flag"), False)%></asp:Label>
          </div>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <itemstyle horizontalalign="center" verticalalign="top" />
          <headerstyle horizontalalign="center" />
          <% If Session.Item("crmUserLogon") = True Then%>
          <% If Session.Item("localUser").crmEvo <> True Then%>
          <% Master.noteactext = ""%>
          <%#Master.ViewNoteAttachedACComp(IIf(Not IsDBNull(Eval("other_ac_id")), Eval("other_ac_id"), Eval("ac_id")), IIf(Not IsDBNull(Eval("other_source")), Eval("other_source"), Eval("source")), 2, "A", 0, Eval("lastnote"))%>
          <div align="center">
            <asp:LinkButton runat="server" ID="viewnote" OnClientClick="return false;">
                       <%#IIf(Master.noteactext <> "", "<img src='images/document.png' alt='Notes Attached to Aircraft' border='0' />", "")%>
            </asp:LinkButton></div>
          <obout:Flyout ID="note_fly" runat="server" AttachTo="viewnote" Position="TOP_LEFT"
            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true">
            <%#clsGeneral.MouseOverTextStart() %>
            <%#Master.noteactext%>
            <%#clsGeneral.MouseOverTextEnd()%>
          </obout:Flyout>
          <obout:Flyout ID="Flyout1" runat="server" AttachTo="popup_ex" Position="TOP_LEFT"
            Align="LEFT" FadingEffect="true" Visible='<%#Master.broker %>'>
          </obout:Flyout>
          <a href="#" style="font-size: 9px;" onclick="javascript:load('edit_note.aspx?ac_ID=<%# IIf(Not IsDBNull(Eval("other_ac_id")), Eval("other_ac_id"), Eval("ac_id")) %>&source=<%# IIf(Not IsDBNull(Eval("other_source")), Eval("other_source"), Eval("source")) %>&type=note&action=new','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');">
            [+]</a>
          <% End If%>
          <% End If%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:BoundColumn DataField="act_name" Visible="false" />
      <asp:BoundColumn DataField="acref_owner_percentage" Visible="false" />
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <itemstyle horizontalalign="center" verticalalign="top" />
          <headerstyle horizontalalign="center" />
          <div align="center">
            <%#Master.ViewPriorityEventsClient(Eval("lastevent"), Eval("jetnet_ac_id"))%>
            <asp:LinkButton runat="server" ID="viewevent" OnClientClick="return false;">
                       <%#IIf(Master.eventactext <> "", "<img src='images/light.png' alt='Recent Events Attached to Aircraft' border='0' />", "")%>
            </asp:LinkButton></div>
          <obout:Flyout ID="event_fly" runat="server" AttachTo="viewevent" Position="TOP_LEFT"
            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true">
            <%#clsGeneral.MouseOverTextStart() %>
            <%#Master.eventactext%>
            <%#clsGeneral.MouseOverTextEnd()%>
          </obout:Flyout>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:BoundColumn DataField="comp_name" Visible="false" />
      <asp:BoundColumn DataField="comp_address1" Visible="false" />
      <asp:BoundColumn DataField="comp_address2" Visible="false" />
      <asp:BoundColumn DataField="comp_city" Visible="false" />
      <asp:BoundColumn DataField="comp_state" Visible="false" />
      <asp:BoundColumn DataField="comp_country" Visible="false" />
      <asp:BoundColumn DataField="comp_zip_code" Visible="false" />
      <asp:BoundColumn DataField="comp_email_address" Visible="false" />
      <asp:BoundColumn DataField="comp_web_address" Visible="false" />
      <asp:BoundColumn DataField="contact_first_name" Visible="false" />
      <asp:BoundColumn DataField="contact_last_name" Visible="false" />
      <asp:BoundColumn DataField="contact_middle_initial" Visible="false" />
      <asp:BoundColumn DataField="contact_title" Visible="false" />
      <asp:BoundColumn DataField="contact_preferred_name" Visible="false" />
      <asp:BoundColumn DataField="contact_notes" Visible="false" />
      <asp:BoundColumn DataField="contact_email_address" Visible="false" />
      <asp:BoundColumn DataField="comp_source" Visible="false" />
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <itemstyle horizontalalign="center" verticalalign="top" />
          <headerstyle horizontalalign="center" />
          <% Master.noteactext = ""%>
          <%#Master.ViewNoteAttachedACComp(Eval("ac_id"), Eval("source"), 2, "P", 0, Eval("lastnote"))%>
          <div align="center">
            <asp:LinkButton runat="server" ID="viewaction" OnClientClick="return false;">
                       <%#IIf(Master.noteactext <> "", "<img src='images/red_pin.png' alt='Actions Attached to Aircraft' border='0' />", "")%>
            </asp:LinkButton></div>
          <obout:Flyout ID="action_fly" runat="server" AttachTo="viewaction" Position="TOP_LEFT"
            Align="TOP" FlyingEffect="TOP_RIGHT" FadingEffect="true">
            <%#clsGeneral.MouseOverTextStart() %>
            <%#Master.noteactext%>
            <%#clsGeneral.MouseOverTextEnd()%>
          </obout:Flyout>
        </ItemTemplate>
      </asp:TemplateColumn>
    </Columns>
  </asp:DataGrid>
</asp:Content>
