<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="enterWantedInfo.aspx.vb"
  Inherits="crmWebClient.enterWantedInfo" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script type="text/javascript" language="javascript">

    function makeActiveJS(PassedValue) {

      switch (PassedValue.toLowerCase()) {
        case "yearrange":
          {
            document.getElementById("optYearRangeID").checked = true;
            document.getElementById("txtYearNoteID").value = "";
          }
          break;
        case "yearnote":
          {
            document.getElementById("optYearRangeID1").checked = true;
            document.getElementById("txtYearRange1ID").value = "";
            document.getElementById("txtYearRange2ID").value = "";
          }
          break;

        case "maxprice":
          {
            document.getElementById("optMaxPriceID").checked = true;
            document.getElementById("txtPriceNoteID").value = "";
          }
          break;

        case "pricenote":
          {
            document.getElementById("optMaxPriceID1").checked = true;
            document.getElementById("txtMaxPriceID").value = "";
          }
          break;
      }
      return true;
    }

    function submitWantedSuccess() {
      alert("Thank You ... The information you have submitted will be reviewed by a Jetnet Researcher and posted to Evolution as soon as possible.");
    }

    function submitWantedFailure() {
      alert("We are sorry ... there was an error submitting your wanted info to JETNET ... Please look at your selections and try again.");
    }
   
  </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <a href="#" class="light_gray_button float_right" onclick="javascript:window.close();">
    Close</a><asp:Button ID="save_button" runat="server" Text="Save" CssClass="gray_button float_right" /><div
      class="clear">
    </div>
  <div class="NotesHeader">
  </div>
  <cc1:TabContainer runat="server" ID="tab_container_ID" Width="100%" BorderStyle="None"
    CssClass="dark-theme">
    <cc1:TabPanel ID="enterWanted" runat="server" HeaderText="Enter Wanted">
      <ContentTemplate>
        <div style="width: 100%; text-align: left;">
          <table border='0' cellspacing="0" cellpadding="2" width="50%">
            <tr>
              <td align="center" colspan="2" style="border-color: Maroon; border-style: solid;
                border-width: 2px; padding: 6px;">
                <font color='DarkRed'><b>Please Note:</b></font> Information entered here must be
                approved by a JETNET Researcher before being listed in the database. You will NOT
                see your new entry until the approval process is complete.
              </td>
            </tr>
            <tr>
              <td colspan="2" height='5'>
                <hr />
              </td>
            </tr>
            <tr>
              <td align="left">
                Make/Model:
              </td>
              <td>
                <asp:DropDownList ID="wantedModelList" runat="server" ToolTip="Select Wanted Aircraft Make/Model"
                  CausesValidation="True">
                </asp:DropDownList>
              </td>
            </tr>
            <tr>
              <td colspan="2">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Make/Model"
                  SetFocusOnError="true" ControlToValidate="wantedModelList" InitialValue=""></asp:RequiredFieldValidator>
              </td>
            </tr>
            <tr>
              <td colspan="2">
                Year
              </td>
            </tr>
            <tr>
              <td align="left">
                <input type="radio" name="optYearRange" id="optYearRangeID" value="Range" checked="checked" />Range:
              </td>
              <td>
                <input type="text" name="txtYearRange1" id="txtYearRange1ID" size="6" onkeypress='Javascript:makeActiveJS("yearrange");' />&nbsp;
                <input type="text" name="txtYearRange2" id="txtYearRange2ID" size="6" onkeypress='Javascript:makeActiveJS("yearrange");' />
              </td>
            </tr>
            <tr>
              <td align="left">
                <input type="radio" name="optYearRange" id="optYearRangeID1" value="" />Note:
              </td>
              <td>
                <input type="text" name="txtYearNote" id="txtYearNoteID" size="16" onkeypress='Javascript:makeActiveJS("yearnote");' />
              </td>
            </tr>
            <tr>
              <td colspan="2">
                Price
              </td>
            </tr>
            <tr>
              <td align="left">
                <input type="radio" name="optMaxPrice" id="optMaxPriceID" value="Price" checked="checked" />Max:
              </td>
              <td>
                <input type="text" name="txtMaxPrice" id="txtMaxPriceID" size="16" onkeypress='Javascript:makeActiveJS("maxprice");' />
              </td>
            </tr>
            <tr>
              <td align="left">
                <input type="radio" name="optMaxPrice" id="optMaxPriceID1" value="" />Note:
              </td>
              <td>
                <input type="text" name="txtPriceNote" id="txtPriceNoteID" size="16" onkeypress='Javascript:makeActiveJS("pricenote");' />
              </td>
            </tr>
            <tr>
              <td align="left">
                Max AFTT:
              </td>
              <td>
                <input type="text" name="txtMaxAFTT" size="16" />
              </td>
            </tr>
            <tr>
              <td align="left">
                Has Damage ?
              </td>
              <td>
                <asp:DropDownList ID="wantedDamageList" runat="server" ToolTip="Select to include damage">
                  <asp:ListItem Value="U">Unknown</asp:ListItem>
                  <asp:ListItem Value="Y">Yes</asp:ListItem>
                  <asp:ListItem Value="N">No</asp:ListItem>
                </asp:DropDownList>
              </td>
            </tr>
            <tr>
              <td align="left">
                Notes:
              </td>
              <td>
                <textarea wrap="virtual" name="txtNotes" rows="5" cols="45"></textarea>
              </td>
            </tr>
            <%

              If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag Then
	
                Response.Write("<tr><td colspan='2' align='center'><br />Note:&nbsp;&nbsp;You are not allowed to add a wanted since this is just a temporary account.")
                Response.Write("</td></tr>")

              End If

            %>
          </table>
        </div>
      </ContentTemplate>
    </cc1:TabPanel>
  </cc1:TabContainer>
</asp:Content>
