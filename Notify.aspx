<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Notify.aspx.vb" Inherits="crmWebClient.Notify"
    MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
</script>
    <style>
        html {
            background-color: white;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="notifyFormBackground">
        <asp:Panel runat="server" ID="pre_submittal_form">
            <h1>
                <span class="medium_text" runat="server" id="tellChangesText">TELL JETNET ABOUT CHANGES TO THIS AIRCRAFT</span></h1>
            <p>
                Our goal is to always to have the most up to date and verified data in the industry.
      Use this form below to identify any changes you feel are relevant to this <span runat="server" id="tellTypeText">aircraft</span>.
      <br />
                <br />
                Note that changes submitted from this window are only accepted in a text format.
            </p>
            <asp:Label runat="server" ID="attention" ForeColor="Red" CssClass="float_left"><br /><br /></asp:Label>
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="*Text must be no longer than 2000 characters."
                OnServerValidate="validateLength" CssClass="float_left padding_bottom" ControlToValidate="responseText"
                Text="" Display="static" Enabled="true"></asp:CustomValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*Text is required"
                ControlToValidate="responseText" runat="server" Display="static" CssClass="float_right padding_bottom"
                Enabled="true"></asp:RequiredFieldValidator>
            <div class="text_align_center">
                <asp:TextBox runat="server" ID="responseText" Rows="10" Width="98%" TextMode="MultiLine"></asp:TextBox>
            </div>
            <div class="clear">
            </div>
            <br />
            <span class="float_left">*All changes will be evaluated and confirmed by JETNET.<br />
                <br />
            </span>
            <asp:Button ID="submitNotify" CssClass="gray_button float_left clear_left" Text="Submit"
                runat="server" CausesValidation="true" />
        </asp:Panel>
        <asp:Panel runat="server" ID="post_submittal_form" Visible="false">
            <h1>
                <span class="medium_text">THANK YOU</span></h1>
            <p>
                Your request has been submitted.
            </p>
        </asp:Panel>
        <br clear="all" />
    </div>
</asp:Content>
