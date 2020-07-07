<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SendSalesTransaction.aspx.vb"
  Inherits="crmWebClient.SendSalesTransaction" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script language="javascript" type="text/javascript">
</script>

  <style>
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="valueSpec viewValueExport Simplistic aircraftSpec plain">
    <asp:Panel runat="server" ID="sales_pre_submittal_form" Visible="false" CssClass="SalesTransaction">
      <div class="Box">
        <div class="row">
          <h1>
            <span class="medium_text padding">Share My Transaction Data with JETNET</span></h1>
          <div class="row SalesTransactionAircraftInfo">
            <div class="nine columns">
              <asp:Label runat="server" ID="journal_information"></asp:Label>
            </div>
            <asp:TextBox runat="server" ID="SalesTransactionDate" CssClass="display_none"></asp:TextBox>
          </div>
          <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="*Both asking and sale price may not be blank.<br /><br />"
            ValidationGroup="price" Display="dynamic" ControlToValidate="asking_price" OnServerValidate="HaveOnePrice"
            Enabled="true" ValidateEmptyText="true" CssClass="padding display_block"></asp:CustomValidator>
          <asp:ValidationSummary runat="server" ID="valSummary" ValidationGroup="price" ShowMessageBox="true"
            ShowSummary="false" />
          <div class="row">
            <table class="formatTable blue">
              <tr>
                <td align="left" valign="top">Asking Price:
                </td>
                <td align="left" valign="top" width="200">
                  <asp:CompareValidator ID="CompareValidator1" ValidationGroup="price" runat="server"
                    ControlToValidate="asking_price" Operator="DataTypeCheck" Type="Currency" ErrorMessage="Please enter a numeric Asking Price*"
                    Display="None" Enabled="true"></asp:CompareValidator>
                  <asp:RangeValidator runat="server" ValidationGroup="price" Display="None" Enabled="false"
                    Type="Currency" ControlToValidate="asking_price" ID="askingPriceRange"></asp:RangeValidator>
                  <asp:TextBox ID="asking_price" runat="server" Width="100%"></asp:TextBox>
                </td>
                <td align="left" valign="top">Sale Price:
                </td>
                <td align="left" valign="top" width="200">
                  <asp:CompareValidator ValidationGroup="price" runat="server" ControlToValidate="sale_price"
                    Operator="DataTypeCheck" Type="currency" ErrorMessage="Please enter a numeric Sale Price*"
                    Display="None" Enabled="true"></asp:CompareValidator>
                  <asp:RangeValidator runat="server" ValidationGroup="price" Display="None" Enabled="false"
                    Type="Currency" ControlToValidate="sale_price" ID="salesPriceRange"></asp:RangeValidator>
                  <asp:TextBox ID="sale_price" runat="server" Width="100%"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  <label class="help_cursor" title="Please note that the Airframe Total Time (AFTT) must be blank, or numeric only.">
                    Airframe Total Time (AFTT):</label>
                </td>
                <td align="left" valign="top">
                  <asp:CompareValidator ID="CompareValidator2" ValidationGroup="price" runat="server"
                    ControlToValidate="aftt" Operator="DataTypeCheck" Type="Currency" ErrorMessage="Please enter a numeric Airframe Total Time (AFTT)*"
                    Display="None" Enabled="true"></asp:CompareValidator>
                  <asp:TextBox ID="aftt" runat="server" Width="100%"></asp:TextBox>
                </td>
                <td align="left" valign="top">
                  <label class="help_cursor" title="Please note that the Total Landings must be blank, or numeric only.">
                    Total Landings:</label>
                </td>
                <td align="left" valign="top">
                  <asp:CompareValidator ID="CompareValidator3" ValidationGroup="price" runat="server"
                    ControlToValidate="total_landings" Operator="DataTypeCheck" Type="Currency" ErrorMessage="Please enter a numeric Total Landings*"
                    Display="None" Enabled="true"></asp:CompareValidator>
                  <asp:TextBox ID="total_landings" runat="server" Width="100%"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  <label class="help_cursor underline" title="Enter a description of key factors influencing the price of this aircraft (if any).">
                    Value/Price Description:</label>
                </td>
                <td align="left" valign="top" colspan="3">
                  <asp:TextBox ID="value_price_description" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top" colspan="4">
                  <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="User must agree to USER AUTHORIZATION statement above before submitting transaction data."
                    ValidationGroup="price" Display="None" ControlToValidate="AgreeToTermsValidationControl"
                    OnServerValidate="checkCheckbox"></asp:CustomValidator>
                  <asp:TextBox runat="server" ID="AgreeToTermsValidationControl" Visible="false" ValidateEmptyText="true"></asp:TextBox><strong
                    class="sales">USER AUTHORIZATION:</strong>
                  <asp:CheckBox runat="server" AutoPostBack="true" ID="agreeToTerms" Text="I understand that by checking this box that the data reported regarding this transaction will be sent to JETNET for use and display within JETNET's products including display of the sale price for this specific serial numbered aircraft if provided. " />JETNET
              WILL NOT display the source of any asking/sale price data reported as part of this
              submittal process unless required to do so by court order or otherwise by law. <a href="javascript:void(0);" onclick="javascript:load('/help/documents/661.pdf','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"
                target="_blank">Learn More</a> .
                </td>
              </tr>
              <tr>
                <td align="left" valign="top" colspan="4">
                  <asp:Button runat="server" Enabled="false" CssClass="disabledSalesButton" Text="Submit Transaction Data to JETNET"
                    CausesValidation="true" ValidationGroup="price" ID="SubmitTransactionData" ToolTip="Please agree to Terms before Submittal" />
                </td>
              </tr>
            </table>
          </div>
          <div class="clear">
          </div>
        </div>
      </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="post_submittal_form" Visible="false">
      <h1>
        <span class="medium_text">THANK YOU</span></h1>
      <p>
        Your transaction data has been submitted.
      </p>
    </asp:Panel>
    <asp:Panel runat="server" ID="error_submittal_form" Visible="false">
      <h1>
        <span class="medium_text">ERROR</span></h1>
      <p>
        An error has occurred with your submission. Please contact Jetnet for more information.
      </p>
    </asp:Panel>
  </div>
</asp:Content>
