<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserVerification.aspx.vb" Inherits="crmWebClient.UserVerification"
    MasterPageFile="~/EvoStyles/EvoTheme.Master" StylesheetTheme="Evo" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/main_site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://www.google.com/recaptcha/api.js" async defer></script>
    <style type="text/css">
        .Box {
            width: 95%;
            margin-left: auto;
            margin-right: auto;
            margin-bottom: -16px !important;
        }

        .mainHeading {
            margin-top: -5px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="valueSpec viewValueExport aircraftListing Simplistic aircraftSpec">
        <div class="row">
            <div class="twelve columns">
                <h2 class="mainHeading padded_left"><strong>Unusual Traffic</strong> Detected</h2>
                <div class="Box">
                    <br />
                    <br />
                    <asp:Label runat="server" ID="attention_text" ForeColor="Red" Font-Bold="true"></asp:Label>
                    <p>Our system has detected unusual traffic from your computer network.  Please use the form below to verify that your activity is not that from robot software.</p>
                    <asp:Label runat="server" ForeColor="Red" Font-Bold="true" Visible="false" ID="RecaptchaFail"
                        CssClass="float_left">*&nbsp;&nbsp;</asp:Label>
                    <div class="g-recaptcha" data-sitekey="6LfsWdUUAAAAAKbAlI17VMIra51a2ellf4BMUU-v"></div>
                    <br />
                   <asp:Button runat="server" ID="submitButton" OnClientClick="SubmitForm();" Text="Submit"/>
                </div>
            </div>
        </div>
    </div>
<script>
    function SubmitForm() {
        if (grecaptcha.getResponse()) {
            //alert('ClientSide Okay');
            return true;
        }
        return false;
    }

</script>
</asp:Content>
