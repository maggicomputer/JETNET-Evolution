<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Aircraft_Edit_Engine_Tab.ascx.vb"
  Inherits="crmWebClient.Aircraft_Edit_Engine_Tab" %>
<asp:Panel ID="aircraft_edit" runat="server">
  <asp:Label runat="server" ID="title_change" CssClass="valueSpec viewValueExport Simplistic aircraftSpec"> <h2 align="right">
        Engine Edit</h2></asp:Label>
  <div class="valueSpec Simplistic viewValueExport aircraftSpec">
    <div class="Box">
    <div class="subHeader padding_left">Engine Edit</div><br />
      <asp:Table ID="Table1" runat="server" CellSpacing="3" CellPadding="7" GridLines="none"
        CssClass="formatTable blue">
        <asp:TableRow runat="server" CssClass="gray">
          <asp:TableCell runat="server" ColumnSpan="2">
            Model:<asp:TextBox ID="new_engine" Style="display: none;" runat="server"></asp:TextBox>
            <asp:TextBox ID="engine_model" runat="server"></asp:TextBox>
          </asp:TableCell>
          <asp:TableCell ID="TableCell2" runat="server">
           Maintenance Program:
          </asp:TableCell>
          <asp:TableCell ID="TableCell9" runat="server">
            <asp:DropDownList ID="engine_maintenance_program" runat="server" Width="150">
            </asp:DropDownList>
          </asp:TableCell>
          <asp:TableCell ID="TableCell4" runat="server">
         Management Program:
          </asp:TableCell>
          <asp:TableCell ID="TableCell10" runat="server">
            <asp:DropDownList ID="engine_management_program" runat="server" Width="150">
            </asp:DropDownList>
          </asp:TableCell>
          <asp:TableCell ID="TableCell5" runat="server">
            On Condition TBO:
          </asp:TableCell>
          <asp:TableCell ID="TableCell11" runat="server" ColumnSpan="3">
            <asp:RadioButtonList ID="on_condition_tbo_rd" runat="server" RepeatDirection="Horizontal">
              <asp:ListItem id="on_condition_tbo_yes" runat="server" Value="Y" Text="Yes" />
              <asp:ListItem id="on_condition_tboe_no" runat="server" Value="N" Text="No" Selected="True" />
            </asp:RadioButtonList>
          </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow2" runat="server" CssClass="gray">
          <asp:TableCell ID="TableCell29" runat="server" ColumnSpan="2">
 
          </asp:TableCell>
          <asp:TableCell ID="TableCell18" runat="server">
           Noise Rating
          </asp:TableCell>
          <asp:TableCell ID="TableCell19" runat="server">
            <asp:TextBox runat="server" ID="noise_rating" Width="150" MaxLength="50"></asp:TextBox>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Please Enter a Number"
              Display="dynamic" Operator="DataTypeCheck" Type="Integer" ControlToValidate="noise_rating"
              SetFocusOnError="true"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell ID="TableCell20" runat="server">
            Model Configuration
          </asp:TableCell>
          <asp:TableCell ID="TableCell21" runat="server">
            <asp:TextBox runat="server" ID="model_config" Width="150" MaxLength="4"></asp:TextBox>
          </asp:TableCell>
          <asp:TableCell ID="TableCell22" runat="server">
Overhaul Done By Name:
          </asp:TableCell>
          <asp:TableCell ID="TableCell23" runat="server" ColumnSpan="3">
            <asp:TextBox runat="server" ID="overhaul_done_by_name" Width="150" MaxLength="50"></asp:TextBox>
          </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow3" runat="server" CssClass="gray">
          <asp:TableCell ID="TableCell30" runat="server" ColumnSpan="2">

          </asp:TableCell>
          <asp:TableCell ID="TableCell27" runat="server">
Overhaul Done By Month/Name
          </asp:TableCell>
          <asp:TableCell ID="TableCell28" runat="server">
            <asp:TextBox runat="server" ID="overhaul_done_month_year" Width="150" MaxLength="6"></asp:TextBox>
          </asp:TableCell>
          <asp:TableCell ID="TableCell8" runat="server">
           HOT Inspection Done By Name
          </asp:TableCell>
          <asp:TableCell ID="TableCell24" runat="server">
            <asp:TextBox runat="server" ID="hot_inspection_done_by_name" Width="150" MaxLength="50"></asp:TextBox>
          </asp:TableCell>
          <asp:TableCell ID="TableCell25" runat="server">
            HOT Inspection Done Month/Year
          </asp:TableCell>
          <asp:TableCell ID="TableCell26" runat="server" ColumnSpan="3">
            <asp:TextBox runat="server" ID="hot_inspection_done_month_year" Width="150" MaxLength="6"></asp:TextBox>
          </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow1" runat="server" CssClass="dark_blue">
          <asp:TableCell ID="TableCell1" runat="server" ColumnSpan="2">
        &nbsp;
          </asp:TableCell>
          <asp:TableCell ID="TableCell3" runat="server" VerticalAlign="Top">
            <strong>Serial #:</strong>
          </asp:TableCell>
          <asp:TableCell ID="TableCell6" runat="server" VerticalAlign="Top">
            <strong>TTSNEW Hrs</strong><br /><span class="tiny">(Total Time Since New)</span>
          </asp:TableCell>
          <asp:TableCell ID="TableCell7" runat="server" VerticalAlign="Top">
            <strong>SOH/SCOR Hrs </strong><br /><span class="tiny">(Since Overhaul)</span>
          </asp:TableCell>
          <asp:TableCell ID="TableCell12" runat="server" VerticalAlign="Top">
            <strong>SHI/SMPI Hrs </strong><br /><span class="tiny">(Since Hot Inspection)</span>
          </asp:TableCell>
          <asp:TableCell ID="TableCell13" runat="server" VerticalAlign="Top">
            <strong>TBO/TBCI Hrs</strong> <br /><span class="tiny">(Time Between Overhaul)</span>
          </asp:TableCell>
          <asp:TableCell ID="TableCell14" runat="server" VerticalAlign="Top">
            <strong>Total Cycles<br /> Since New</strong>
          </asp:TableCell>
          <asp:TableCell ID="TableCell15" runat="server" VerticalAlign="Top">
          <strong>Total Cycles<br /> Since Overhaul</strong>
          </asp:TableCell>
          <asp:TableCell ID="TableCell16" runat="server" VerticalAlign="Top">
          <strong>Total Cycles Since Hot</strong>
          </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" CssClass="alt_row">
          <asp:TableCell runat="server" ColumnSpan="2">
        Engine 1:
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_1_ser" runat="server" Width="50" MaxLength="14"></asp:TextBox>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox runat="server" ID="engine_1_ttsnew" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Please Enter a Number"
              Display="dynamic" Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_1_ttsnew"
              SetFocusOnError="true"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_1_soh" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_1_soh" SetFocusOnError="true"
              Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_1_shi" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator4" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_1_shi" SetFocusOnError="true"
              Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_1_tbo" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_1_tbo" SetFocusOnError="true"
              Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_1_tot_snew_cycle" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator6" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_1_tot_snew_cycle"
              SetFocusOnError="true" Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_1_tot_overhaul_cycles" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator7" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_1_tot_overhaul_cycles"
              SetFocusOnError="true" Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_1_tot_cycle_shot" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator8" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_1_tot_cycle_shot"
              SetFocusOnError="true" Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server">
          <asp:TableCell runat="server" ColumnSpan="2">
        Engine 2:
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_2_ser" runat="server" Width="50" MaxLength="14"></asp:TextBox>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_2_ttsnew" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator10" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_2_ttsnew" SetFocusOnError="true"
              Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_2_soh" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator9" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_2_soh" SetFocusOnError="true"
              Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_2_shi" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator11" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_2_shi" SetFocusOnError="true"
              Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_2_tbo" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator12" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_2_tbo" SetFocusOnError="true"
              Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_2_tot_snew_cycle" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator13" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_2_tot_snew_cycle"
              SetFocusOnError="true" Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_2_tot_overhaul_cycles" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator14" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_2_tot_overhaul_cycles"
              SetFocusOnError="true" Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_2_tot_cycle_shot" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator15" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_2_tot_cycle_shot"
              SetFocusOnError="true" Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" CssClass="alt_row">
          <asp:TableCell ID="TableCell17" runat="server" ColumnSpan="2">
        Engine 3:
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_3_ser" runat="server" Width="50" MaxLength="14"></asp:TextBox>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_3_ttsnew" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator17" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_3_ttsnew" SetFocusOnError="true"
              Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_3_soh" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator16" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_3_soh" SetFocusOnError="true"
              Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_3_shi" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator18" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_3_shi" SetFocusOnError="true"
              Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_3_tbo" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator19" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_3_tbo" SetFocusOnError="true"
              Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_3_tot_snew_cycle" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator20" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_3_tot_snew_cycle"
              SetFocusOnError="true" Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_3_tot_overhaul_cycles" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator21" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_3_tot_overhaul_cycles"
              SetFocusOnError="true" Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_3_tot_cycle_shot" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator22" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_3_tot_cycle_shot"
              SetFocusOnError="true" Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server">
          <asp:TableCell runat="server" ColumnSpan="2">
        Engine 4:
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_4_ser" runat="server" Width="50" MaxLength="14"></asp:TextBox>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_4_ttsnew" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator23" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_4_ttsnew" SetFocusOnError="true"
              Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_4_soh" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator24" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_4_soh" SetFocusOnError="true"
              Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_4_shi" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator25" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_4_shi" SetFocusOnError="true"
              Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_4_tbo" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator26" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_4_tbo" SetFocusOnError="true"
              Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_4_tot_snew_cycle" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator27" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_4_tot_snew_cycle"
              SetFocusOnError="true" Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_4_tot_overhaul_cycles" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator28" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_4_tot_overhaul_cycles"
              SetFocusOnError="true" Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
          <asp:TableCell runat="server">
            <asp:TextBox ID="engine_4_tot_cycle_shot" runat="server" Width="50"></asp:TextBox><br />
            <asp:CompareValidator ID="CompareValidator29" runat="server" ErrorMessage="Please Enter a Number"
              Operator="DataTypeCheck" Type="Integer" ControlToValidate="engine_4_tot_cycle_shot"
              SetFocusOnError="true" Display="dynamic"></asp:CompareValidator>
          </asp:TableCell>
        </asp:TableRow>
      </asp:Table>
    </div>
  </div>
  <asp:Panel ID="buttons" runat="server" BackColor="White">
    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Right">
      <asp:Label ID="update_text" runat="server" Font-Italic="True"></asp:Label>
    </asp:Panel>
    <table width="100%" cellpadding="4" cellspacing="0">
      <tr>
        <td align="left" valign="top">
        <a href="javascript: window.opener.location.href = window.opener.location.href; self.close();" class="button">Close</a>
        </td>
        <td align="right" valign="top">
            <asp:Button runat="server" id="updateButton" CausesValidation="true" Text="Save" class="button" />
        </td>
        
      </tr>
    </table>
  </asp:Panel>
</asp:Panel>
