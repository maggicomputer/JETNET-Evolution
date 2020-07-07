<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="listing.aspx.vb" Inherits="crmWebClient._listing"
  MasterPageFile="~/main_site.Master" EnableViewState="true" %> 
  
<%@ Import Namespace="crmWebClient.clsGeneral" %>
<%@ MasterType VirtualPath="~/main_site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
  <asp:DataGrid runat="server" ID="Results" CellPadding="3" OnItemCommand="dispDetails"
    OnItemDataBound="Transaction_Bind" Width="100%" AllowPaging="true" PageSize="25"
    AllowSorting="True" AutoGenerateColumns="false" PagerStyle-Mode="NumericPages"
    GridLines="None" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingItemStyle-CssClass="alt"
    ItemStyle-CssClass="item_row" ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="th">
    <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" ForeColor="White" />
    <AlternatingItemStyle CssClass="alt_row" />
    <ItemStyle BorderStyle="None" VerticalAlign="Top" />
    <HeaderStyle Wrap="False" HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
    <Columns>
      <asp:BoundColumn DataField="comp_id" Visible="false" />
      <asp:BoundColumn DataField="contact_id" Visible="false" />
      <asp:BoundColumn DataField="contact_type" Visible="false" />
      <asp:BoundColumn DataField="source" Visible="false" />
      <asp:BoundColumn DataField="trans_id" Visible="false" />
      <asp:BoundColumn DataField="trans_ac_id" Visible="false" />
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <itemstyle width="10px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="10px" />
          <input type="checkbox" />
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <%#clsGeneral.WhatAmI(DataBinder.Eval(Container.DataItem, "source"))%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="">
        <ItemTemplate>
          <img src="images/edit_icon.png" alt="Edit Transaction" title="Edit this Transaction"
            class="help_cursor" onclick="javascript:load('edit.aspx?action=edit&amp;type=transaction<%# iif(DataBinder.Eval(Container.DataItem, "trans_id") = DataBinder.Eval(Container.DataItem, "jetnet_trans_id"), "","&amp;cli_trans=" & DataBinder.Eval(Container.DataItem, "trans_id")) %>&trans=<%#DataBinder.Eval(Container.DataItem, "jetnet_trans_id")%>&acID=<%#iif(DataBinder.Eval(Container.DataItem, "trans_ac_id") = 0, DataBinder.Eval(Container.DataItem, "jetnet_trans_ac_id") & "&source=JETNET",DataBinder.Eval(Container.DataItem, "trans_ac_id") & "&source=" & DataBinder.Eval(Container.DataItem, "source"))%>','','scrollbars=yes,menubar=no,height=880,width=1000,resizable=yes,toolbar=no,location=no,status=no');" /></ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Date">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <%#clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "trans_date"))%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Aircraft Info">
        <ItemTemplate>
          <itemstyle horizontalalign="center" verticalalign="top" width="150px" />
          <headerstyle />
          <a href="details.aspx?ac_ID=<%#iif(DataBinder.Eval(Container.DataItem, "trans_ac_id") = 0, DataBinder.Eval(Container.DataItem, "jetnet_trans_ac_id") & "&source=JETNET",DataBinder.Eval(Container.DataItem, "trans_ac_id") & "&source=" & DataBinder.Eval(Container.DataItem, "source"))%>&type=3">
            <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%>&nbsp;<%#DataBinder.Eval(Container.DataItem, "amod_model_name")%>
            Ser#:<%#DataBinder.Eval(Container.DataItem, "trans_ser_nbr")%><br />
            Reg#:<%#DataBinder.Eval(Container.DataItem, "trans_reg_nbr")%>
          </a>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Year Mfr<br />AFTT" Visible="true">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
              <td align="left" valign="top" width="1">
                <img src="images/spacer.gif" alt="" height="40" width="1" />
              </td>
              <td align="left" valign="top">
                <%#DataBinder.Eval(Container.DataItem, "trans_year_mfr")%>
              </td>
            </tr>
          </table>
          <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "trans_airframe_total_hours")), Master.DisplayAFTT("[" & DataBinder.Eval(Container.DataItem, "trans_airframe_total_hours") & "]", True, DataBinder.Eval(Container.DataItem, "trans_airframe_total_hours")), "")%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Listed" Visible="true">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <%#clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "trans_date_listed"))%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Asking ($k)">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <%#clsGeneral.no_zero_sold(DataBinder.Eval(Container.DataItem, "trans_asking_price"),  IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "trans_asking_wordage")), DataBinder.Eval(Container.DataItem, "trans_asking_wordage"), "") , True, DataBinder.Eval(Container.DataItem, "source"), "Asking",  IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "trans_asking_wordage")), DataBinder.Eval(Container.DataItem, "trans_asking_wordage"), "") )%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Take ($k)">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <%#clsGeneral.no_zero_sold(DataBinder.Eval(Container.DataItem, "clitrans_est_price"), "", True, DataBinder.Eval(Container.DataItem, "source"), "Take",   IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "trans_asking_wordage")), DataBinder.Eval(Container.DataItem, "trans_asking_wordage"), "") )%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Sold ($k)">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <%#clsGeneral.no_zero_sold(DataBinder.Eval(Container.DataItem, "clitrans_sold_price"), DataBinder.Eval(Container.DataItem, "clitrans_sold_price_type"), True, DataBinder.Eval(Container.DataItem, "source"), "Sold",  IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "trans_asking_wordage")), DataBinder.Eval(Container.DataItem, "trans_asking_wordage"), "")   )%>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Relationship">
        <ItemTemplate>
          <itemstyle width="20px" horizontalalign="center" verticalalign="top" />
          <headerstyle width="20px" />
          <asp:Panel ID="company_hold" runat="server">
          </asp:Panel>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:BoundColumn DataField="tcomp_name" Visible="false" />
      <asp:BoundColumn DataField="tcomp_address1" Visible="false" />
      <asp:BoundColumn DataField="tcomp_address2" Visible="false" />
      <asp:BoundColumn DataField="tcomp_city" Visible="false" />
      <asp:BoundColumn DataField="tcomp_state" Visible="false" />
      <asp:BoundColumn DataField="tcomp_country" Visible="false" />
      <asp:BoundColumn DataField="tcomp_zip_code" Visible="false" />
      <asp:BoundColumn DataField="tcomp_email_address" Visible="false" />
      <asp:BoundColumn DataField="tcomp_web_address" Visible="false" />
      <asp:BoundColumn DataField="tcontact_first_name" Visible="false" />
      <asp:BoundColumn DataField="tcontact_last_name" Visible="false" />
      <asp:BoundColumn DataField="tcontact_middle_initial" Visible="false" />
      <asp:BoundColumn DataField="tcontact_title" Visible="false" />
      <asp:BoundColumn DataField="tcontact_preferred_name" Visible="false" />
      <asp:BoundColumn DataField="tcontact_notes" Visible="false" />
      <asp:BoundColumn DataField="tcontact_email_address" Visible="false" />
      <asp:BoundColumn DataField="contact_type_id" Visible="false" />
    </Columns>
  </asp:DataGrid>
</asp:Content>
