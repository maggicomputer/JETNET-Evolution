<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/ABITheme.Master"
  CodeBehind="abiRegIndex.aspx.vb" Inherits="crmWebClient.abiRegIndex" %>

<%@ MasterType VirtualPath="~/EvoStyles/ABITheme.Master" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="main_content">
  <div id="aside_right" class="span2" runat="server">
    <aside role="complementary">
  </aside>
  </div>
  <div id="component" class="span7" runat="server">
    <main role="main">
     <section class="page-blog page-blog__">
		    <header class="page_header">
		      <h1><span runat="server" id="ac_header">Aircraft Registry</span></h1>	
		    </header>
		  <asp:Literal runat="server" ID="acIncludedText" Visible="false">
		      <div class="items-row row-0 row-fluid"><h4 class="pull-left LeftBuffer">Click on the model below to see a list of aircraft registration numbers for that model.</h4>
		      </div>
        <hr />
     </asp:Literal>
		  <asp:Literal runat="server" ID="acModelIncludedText" Visible="false">
		      <div class="items-row row-0 row-fluid"><h4 class="pull-left LeftBuffer">Click on the Serial / Registration Number below to learn more about these aircraft.</h4>
		      </div>
        <hr />
     </asp:Literal>
   
        <div class="items-row cols-2 row-0 row-fluid">
         <asp:Literal runat="server" id="acListLiteral"></asp:Literal>
        </div>
        <div class="items-row row-0 row-fluid">
         <asp:Literal runat="server" id="acDetailedList"></asp:Literal>
        </div>
        <div class="items-row cols-5 row-0 row-fluid">
        <asp:Literal runat="server" id="serialRegNumberlist"></asp:Literal>
        </div>
        <asp:Label runat="server" ID="acDetailInfoIncludedText" Visible="false">
            <div class="items-row row-0 row-fluid"><h4 class="pull-left LeftBuffer">Welcome to JETNET,  the premier business aircraft information research service. Do you need to find critical aircraft information including actual Owners or Operators? Frustrated by other websites that show Owners and Operators at an LLC with a P.O. BOX in Delaware?<br /><br />
            Get real-time CONFIRMED information for this aircraft. Names, addresses, phone numbers, email addresses, detailed specifications and historical information.<br /><br />
            End your search with JETNET: aircraft contact <asp:Label ID="mailtoHref" runat="server" Text=""></asp:Label></h4>
            </div>
          <hr />
        </asp:Label>

		    <asp:Literal runat="server" id="moreModelInformationLiteral"></asp:Literal>
        <div runat="server" visible="false" id="viewAllDiv"><br clear="all" />
        <strong><a href="abiRegIndex.aspx">View All</a></strong>
      </div>
	   </section>   
	   
   </main>
  </div>
</asp:Content>
