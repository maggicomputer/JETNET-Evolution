<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Calendar.ascx.vb" Inherits="crmWebClient.Calendar" %>
<asp:Calendar ID="Calendar" runat="server" DayNameFormat="FirstLetter" Width="100%" Enabled="true" BackColor="White">
    <NextPrevStyle ForeColor="White" /> 
    <DayHeaderStyle BackColor="#cccccc" />
    <TitleStyle BackColor="#184d7b" BorderStyle="None" Font-Bold="True" ForeColor="White" />
</asp:Calendar>