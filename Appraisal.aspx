<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Appraisal.aspx.vb"  
Inherits="crmWebClient.Appraisal" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
  
 


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<script type="text/javascript">
  //function ChangeCalendarView(sender,args){
  //  sender._switchMode("years", true);
  //}

  function checkDateSimple(sender, args) {

    var txtDateID = document.getElementById("<%= date_of_appraisal.text %>");
    var selectedDate = formatDateTime(cDate(txtDateID.value), vbShortDate);

    var rightNow = formatDateTime(new Date(), vbShortDate);


    var isNote = false;

    //alert("rightNow : " + rightNow);
    //alert("selectedDate : " + selectedDate);

    if (isNote) {
      //alert("note : isNote : " + isNote);
      //alert("check : " + (cDate(selectedDate, vbShortDate) > cDate(rightNow, vbShortDate)) );
      if (cDate(selectedDate, vbShortDate) > cDate(rightNow, vbShortDate)) {
        alert("You cannot select a day later than today!");
        sender._selectedDate = new Date();
        // set the date back to the current date
        sender._textbox.set_Value(sender._selectedDate.format(sender._format))
      }
    }
    else {
      //alert("action : isNote : " + isNote);
      //alert("check : " + (cDate(selectedDate, vbShortDate) < cDate(rightNow, vbShortDate)) );
      if (cDate(selectedDate, vbShortDate) < cDate(rightNow, vbShortDate)) {
        alert("You cannot select a day earlier than today!");
        sender._selectedDate = new Date();
        // set the date back to the current date
        sender._textbox.set_Value(sender._selectedDate.format(sender._format))
      }
    }
  }
 
</script>

<div class="sixteen columns">
  <div class="row remove_margin">
    <div class="six columns remove_margin">
     <asp:Label ID="ac_id" runat="server" Text="" CssClass="display_none"></asp:Label>
         <asp:Label ID="ModelID" runat="server" Text="" CssClass="display_none"></asp:Label>
      <asp:Label ID="YachtModelID" runat="server" Text="" CssClass="display_none"></asp:Label>
    <cc1:TabContainer ID="container_tab" runat="server" CssClass="dark-theme" Visible="false"
        Width="100%" AutoPostBack="false">
        <cc1:TabPanel ID="features_tab" runat="server" HeaderText="Features">
          <ContentTemplate>
            <table width="100%" cellpadding="3" cellspacing="0" class="alt_row border">
              <tr>
                <td align="left" valign="top">
                  <asp:Label ID="aircraft_information" runat="server" Text=""></asp:Label>
                </td>
              </tr> 
            </table>
          </ContentTemplate>
        </cc1:TabPanel>
      </cc1:TabContainer>
      <cc1:TabContainer ID="yacht_container_tab" runat="server" CssClass="dark-theme" Visible="false"
        Width="100%" AutoPostBack="false">
        <cc1:TabPanel ID="yacht_features_tab" runat="server" HeaderText="Features">
          <ContentTemplate>
            <table width="100%" cellpadding="3" cellspacing="0" class="alt_row border">
              <tr>
                <td align="left" valign="top">
                  <asp:Label ID="yacht_information" runat="server" Text=""></asp:Label>
                </td>
              </tr>
            </table>
          </ContentTemplate>
        </cc1:TabPanel>
      </cc1:TabContainer>
      <cc1:TabContainer ID="company_container_tab" runat="server" CssClass="dark-theme"
        Visible="false" Width="100%" AutoPostBack="false">
        <cc1:TabPanel ID="company_features_tab" runat="server" HeaderText="Features">
          <ContentTemplate>
            <table width="100%" cellpadding="3" cellspacing="0" class="alt_row border">
              <tr>
                <td align="left" valign="top">
                  <asp:Label ID="company_information" runat="server" Text=""></asp:Label>
                </td>
              </tr>
            </table>
          </ContentTemplate>
        </cc1:TabPanel>
      </cc1:TabContainer>
      <br>
       <font color='red'>Note that All appraisal data entered on this page will be shared with JETNET Subscribers.  The data entered on this form is NOT considered proprietary information.</font>   
    </div>
    <div class="ten columns">
      <cc1:TabContainer ID="note_information" runat="server" CssClass="dark-theme" Visible="true"
        Width="100%" ActiveTabIndex="0" AutoPostBack="false">
        <cc1:TabPanel ID="main_note_tab" runat="server" HeaderText="APPRAISAL INFORMATION">
          <HeaderTemplate>
            APPRAISAL INFORMATION
          </HeaderTemplate>
          <ContentTemplate> 
            <table class="noteStyle" width="100%" cellpadding="3" cellspacing="0">
              <tr>
                <td align="left" valign="middle" style="margin-left: 10px;margin-top: 10px" width='22%'>
                    Type of Appraisal:
                    </td><td align='left' colspan='4'> 
                      <asp:DropDownList ID="type_of" runat="server" Visible="true">
                      <asp:ListItem Value="DESK">Desktop Appraisal</asp:ListItem>
                      <asp:ListItem Value="FULL">Full Appraisal</asp:ListItem> 
                      </asp:DropDownList>
   &nbsp;&nbsp;             
Date:&nbsp;
<asp:TextBox ID="date_of_appraisal" runat="server" Rows="1" Width="70"></asp:TextBox>
                                  <asp:ImageButton runat="server" ID="cal_image" ImageUrl="../images/final.jpg" AlternateText="Click here to display calendar" />
                                  &nbsp;&nbsp;
                                   <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="date_of_appraisal"
                              PopupButtonID="cal_image" Format="d" OnClientDateSelectionChanged="checkDateSimple">
                            </cc1:CalendarExtender>
  </td></tr><tr><Td>
AFTT:         </td><td>
<asp:TextBox ID="aftt" runat="server"  Rows="1" Width="85"></asp:TextBox> 
</td><td rowspan='5'>
           Appraisal Notes:<br>
           <asp:TextBox ID="notes_text" TextMode="MultiLine" runat="server" height="100"  width="250"></asp:TextBox>            
 </td></tr><tr><Td>
 Cycles: </td><td>
<asp:TextBox ID="cycles" runat="server" Rows="1" Width="85"></asp:TextBox>
  </td></tr><tr><Td>
Asking $:   </td><td>
<asp:TextBox ID="asking_price" runat="server" Rows="1" Width="85"></asp:TextBox>             
 </td></tr><tr><Td>Take $:      </td><td>
<asp:TextBox ID="take_price" runat="server" Rows="1" Width="85"></asp:TextBox>               
  </td></tr><tr><Td><b>Estimated Value:</b> </td><td>
<asp:TextBox ID="est_value" runat="server" Rows="1" Width="85"></asp:TextBox>
<br>
</td></tr><tr>
<td>
<asp:Label visible='false' id="action_text" forecolor='red' runat="server"></asp:label>
</td>
<td align='center'>
                        <asp:LinkButton ID="remove_app" runat="server" CssClass="gray_button float_left"
                          Visible="False" CausesValidation="False">Remove</asp:LinkButton>
                          </td><td>
           <asp:LinkButton ID="save_button" runat="server" CssClass="gray_button float_right"
                        Visible="False" >Save</asp:LinkButton>

                      <asp:LinkButton ID="update_app" runat="server" CssClass="gray_button float_left"
                          Visible="False" CausesValidation="False">Update</asp:LinkButton>        
                          
          </td></tr></table>
         </ContentTemplate>
        </cc1:TabPanel>
      </cc1:TabContainer>
    </div>
     </div>
  </div> 

</asp:Content>
