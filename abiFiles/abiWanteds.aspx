<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/ABITheme.Master" CodeBehind="abiWanteds.aspx.vb" Inherits="crmWebClient.abiWanteds" %>

<%@ MasterType VirtualPath="~/EvoStyles/ABITheme.Master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="main_content">


<div id="aside_right" class="span2" runat="server">
    <aside role="complementary">
         <div class="moduletable">
            <header>
              <h3 class="moduleTitle" runat="server" id="moduleTitleHeader">
                <span>Types</span> 
              </h3>
                <ul class="categories-module" runat="server" id="typesOfWantedList">
                  <li><a href="?type=Executive">Executive</a></li>
                  <li><a href="?type=Jets">Jets</a></li>
                  <li><a href="?type=Turboprops">Turboprops</a></li>
                  <li><a href="?type=Pistons">Pistons</a></li>
                  <li><a href="?type=Helicopters">Helicopters</a></li>
                </ul>
                <asp:Literal runat="server" id="companyInformation"></asp:Literal>
            </header>
          </div>
  </aside>
  </div>
  <!-- End Right sidebar -->
  
  <div id="component" class="span7" runat="server">
    <main role="main">
     <section class="page-blog page-blog__">
		    <header class="page_header">
		      <h1><span runat="server" id="wanted_header">Aviation Wanted</span> </h1>	
		    </header>
           <div class="items-row row-0 row-fluid">
		          <asp:Literal runat="server" id="wantedListLiteral"></asp:Literal>
	          </div>
      </section> 
       <div runat="server" visible="false" id="viewAllDiv"><br clear="all" />
        <strong><a href="/aviationwanteds/">View All</a></strong>
      </div>  
    </main>
  </div>
  <!-- End Content-top Row -->
  
</asp:Content>
