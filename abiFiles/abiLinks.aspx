<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/ABITheme.Master" CodeBehind="abiLinks.aspx.vb" Inherits="crmWebClient.abiLinks" %>

<%@ MasterType VirtualPath="~/EvoStyles/ABITheme.Master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="main_content">

 <div id="aside-right" class="span2">
    <aside role="complementary">
         <div class="moduletable ">
            <header>
              <h3 class="moduleTitle ">
                <span>Topics</span> 
              </h3>
            </header>
            
            <ul class="categories-module">
              <asp:Repeater runat="server" ID="linksTopics">
                <ItemTemplate>
                  <li><a href="?topic=<%#Server.UrlEncode(Container.DataItem("cbus_name"))%>"><%#Container.DataItem("cbus_name")%> </a></li>
                </ItemTemplate>
              </asp:Repeater>
              
            </ul>
          </div>

  </aside>
  </div>
  <!-- End Right sidebar -->
  <div id="component" class="span7">
    <main role="main">
     <section class="page-blog page-blog__">
		    <header class="page_header">
		      <h1><span runat="server" id="links_header">Aviation Links</span> </h1>	
		    </header>
           <div class="items-row cols-2 row-0 row-fluid">
		          <asp:Literal runat="server" id="linkListLiteral"></asp:Literal>
	          </div>
      </section>   
      <div runat="server" visible="false" id="viewAllDiv"><br clear="all" />
        <strong><a href="/aviationlinks/">View All</a></strong>
      </div>
    </main>
  </div>
  <!-- End Content-top Row -->
</asp:Content>
