<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm1.aspx.vb" EnableViewState="true"
    Inherits="crmWebClient.WebForm1" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <evo:insertForm runat="server" ID="InsertFormActionsNotes" Visible="true" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">
</asp:Content>
