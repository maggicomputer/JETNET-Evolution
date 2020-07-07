<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/ABITheme.Master" CodeBehind="abiAircraftDetails.aspx.vb" Inherits="crmWebClient.abiAircraftDetails" %>

<%@ MasterType VirtualPath="~/EvoStyles/ABITheme.Master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="main_content">
  <div id="component" class="span12" runat="server">
    <main role="main">
     <section class="page-blog page-blog__">
		    <header class="page_header">
		      <h1><span runat="server" id="ac_header">Aircraft</span> </h1>	
		      <asp:Literal runat="server" id="acInformation"></asp:Literal>
		       <asp:Literal runat="server" id="companyInformation"></asp:Literal>
		    </header>
		
   </main>
  </div>
  
  <script>
 

  </script>
  
  
      <style type="text/css">
  .bx-wrapper .bx-pager {
    bottom: -295px;
    text-align:left !important;
  }
  .bx-pager img{width:100px;height:100px;}
  
  .bx-wrapper .bx-pager a {
    border:3px solid #000000;
    display: block;
    margin: 0 5px;
    padding: 0px;
  }
  
  .bx-wrapper .bx-pager a:hover,
  .bx-wrapper .bx-pager a.active 
  {
    border:3px solid #ff0000;

  }
  
  .bx-wrapper {

  }
</style>


</asp:Content>
