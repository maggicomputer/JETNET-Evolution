<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/ABITheme.Master"
  CodeBehind="abiContact.aspx.vb" Inherits="crmWebClient.abiContact" %>

<%@ MasterType VirtualPath="~/EvoStyles/ABITheme.Master" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="main_content">
  <div id="component" class="span9">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="contact"
      ShowMessageBox="true" ShowSummary="false"></asp:ValidationSummary>
    <main role="main">
     <section class="page-blog page-blog__">
		    <header class="page_header">
		      <h1><span runat="server" id="news_header">CONTACT US</span> </h1>	
		    </header>
	     <asp:Label runat="server" ID="attention" ForeColor="Red" Font-Bold="true" Visible="false"><p>Thank you for contacting us. Your Message has been sent.</p></asp:Label>
		    <asp:Label runat="server" ID="dealerInformation"></asp:Label>
		    <asp:Label runat="server" ID="modelInformation" CssClass="display_none"></asp:Label>
		    <asp:TextBox runat="server" ID="companyID" CssClass="display_none" />
		    <asp:panel runat="server" ID="textBody">
		    <div class="items-row row-0 row-fluid">
		      <span class="span2">&nbsp;&nbsp;&nbsp;Interest:</span>
		      <span class="span10"><asp:DropDownList runat="server" ID="interest" Width="100%"><asp:ListItem>General Information</asp:ListItem><asp:ListItem>Advertising</asp:ListItem><asp:ListItem>Event Listing</asp:ListItem><asp:ListItem>Dealer Aircraft Listing</asp:ListItem><asp:ListItem>Web Services</asp:ListItem></asp:DropDownList></span>
		    </div>
		    <div class="items-row row-0 row-fluid">
		      <span class="span2">&nbsp;&nbsp;&nbsp;Company Name:</span>
		      <span class="span10"><asp:TextBox runat="server" ID="company_name" Width="100%"></asp:TextBox></span> 
		    </div>
		      <div class="items-row row-0 row-fluid">
		      <span class="span2"><asp:RequiredFieldValidator ErrorMessage="*First Name is Required" ID="required_first_name" runat="server" Text="*" ValidationGroup="contact" Display="Static" ControlToValidate="first_name"></asp:RequiredFieldValidator>&nbsp;First Name:</span>
		      <span class="span4"><asp:TextBox runat="server" ID="first_name" Width="100%"></asp:TextBox></span>
		      <span class="span2"><asp:RequiredFieldValidator ErrorMessage="*Last Name is Required" ID="required_last_name" runat="server" Text="*" ValidationGroup="contact" Display="Static" ControlToValidate="last_name"></asp:RequiredFieldValidator>&nbsp;Last Name:</span>
		      <span class="span4"><asp:TextBox runat="server" ID="last_name" Width="100%"></asp:TextBox></span>
		    </div>
		      <div class="items-row row-0 row-fluid">
		      <span class="span2">&nbsp;&nbsp;&nbsp;Address:</span>
		      <span class="span10"><asp:TextBox runat="server" ID="address" Width="100%"></asp:TextBox></span>
		    </div>
		    	<div class="items-row row-0 row-fluid">
		      <span class="span2"></span>
		      <span class="span10"><asp:TextBox runat="server" ID="address_cont" Width="100%"></asp:TextBox></span>
		    </div>
		     <div class="items-row row-0 row-fluid">
		      <span class="span2">&nbsp;&nbsp;&nbsp;City:</span>
		      <span class="span4"><asp:TextBox runat="server" ID="city" Width="100%"></asp:TextBox></span>
		      <span class="span2">&nbsp;&nbsp;State:</span>
		      <span class="span4"><asp:TextBox runat="server" ID="state" Width="100%"></asp:TextBox></span>
		      
		     </div>
		     <div class="itemSpan1 row row-0 row-fluid">
		      <span class="span2">&nbsp;&nbsp;Zip:</span>
		      <span class="span4"><asp:TextBox runat="server" ID="zip" Width="100%"></asp:TextBox></span>
		      <span class="span2">&nbsp;&nbsp;Country:</span>
		      <span class="span4"><asp:DropDownList runat="server" ID="country" Width="100%"><asp:ListItem Value="">Please Select One</asp:ListItem></asp:DropDownList></span>
		    </div>
		     <div class="items-row row-0 row-fluid">

		      <span class="span2">&nbsp;&nbsp;&nbsp;Phone:</span>
		      <span class="span4"><asp:TextBox runat="server" ID="phone" Width="100%"></asp:TextBox></span>
		      <span class="span2"><asp:RequiredFieldValidator ErrorMessage="*Email Address is Required" ID="required_email" runat="server" Text="*" ValidationGroup="contact" Display="Static" ControlToValidate="email"></asp:RequiredFieldValidator>&nbsp;Email:</span>
		      <span class="span4"><asp:TextBox runat="server" ID="email" Width="100%"></asp:TextBox></span>
		    </div>
		     <div class="items-row row-0 row-fluid">

		      <span class="span2">&nbsp;&nbsp;&nbsp;Message:</span>
		      <span class="span10"><asp:TextBox runat="server" ID="message" Width="100%" TextMode="MultiLine" Rows="5" Columns="6" Height="100"></asp:TextBox><br /><asp:Label runat="server" ForeColor="Red" Font-Bold="true" Visible="false" ID="RecaptchaFail"  CssClass="float_left">*&nbsp;&nbsp;</asp:Label><div class="g-recaptcha float_left" data-sitekey="6LdLCgUTAAAAAEdbTcxodnGple_dHPYF7JTibxm3" ></div><br clear="all" /><br /><asp:Button ID="submitForm" CausesValidation="true" ValidationGroup="contact" runat="server" Text="Submit" CssClass="float_right"></asp:Button></span>
		    </div>
		    </asp:panel>
	   </section>   
   </main>
  </div>
</asp:Content>
