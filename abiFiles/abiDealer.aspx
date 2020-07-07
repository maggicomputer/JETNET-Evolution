<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="abiDealer.aspx.vb" Inherits="crmWebClient.abiDealer"
  MasterPageFile="~/EvoStyles/ABITheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/ABITheme.Master" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="main_content">
  <div id="aside_right" class="span2" runat="server">
    <aside role="complementary">
         <div class="moduletable ">
            <header>
              <h3 class="moduleTitle ">
                <span>Countries</span> 
              </h3>
            </header>
            
            <ul class="categories-module">
              <asp:Repeater runat="server" ID="dealerCountry">
                <ItemTemplate>
                  <li><a href="?country=<%#Server.UrlEncode(Container.DataItem("comp_country"))%>"><%#Container.DataItem("comp_country")%> </a></li>
                </ItemTemplate>
              </asp:Repeater>
              
            </ul>
          </div>

  </aside>
  </div>
  <div id="component" class="span7" runat="server">
    <main role="main">
     <section class="page-blog page-blog__">
		    <header class="page_header">
		      <h1><span runat="server" id="dealers_header">Aircraft Dealers</span> </h1>	
		    </header>
		     <div class="items-row row-0 row-fluid"><h4 class="pull-left LeftBuffer">Want to include your business listing here?</h4><figcaption class="pull-right"><a href="/abiFiles/abiContact.aspx?inquiry=true" class="pull-right">Post your information today ></a></figcaption></div>
<hr />
		    <div class="items-row row-0 row-fluid">
		          <asp:Repeater runat="server" ID="dealersRepeater">
		            <ItemTemplate>

			            <article class="item column-1">
				            <!-- Intro image -->
				            <span class="span3">
                    <figure class="item_img img-intro">
		                     <a href="<%#crmwebclient.abi_functions.AircraftDealerURL(Container.DataItem("comp_id"),Container.DataItem("comp_name"))%>">
			                    <img  src="<%= HttpContext.Current.Session.Item("jetnetFullHostName").ToString %>abiFiles/images/blank.gif" class="lazy" data-src="<%#setImagePath(Container.DataItem("comp_id"))%>" alt=""/></span></a></figure></span><span class="span6"><!--  title/author --><header class="item_header">
	                    <h4 class="item_title">		 
	                      <a href="<%#crmwebclient.abi_functions.AircraftDealerURL(Container.DataItem("comp_id"),Container.DataItem("comp_name"))%>">
	                       <span><%#Container.DataItem("comp_name")%></span> 
	                      </a>
	                    </h4>
	                  </header>
                    <!-- Introtext -->
                    <div class="item_introtext">
                   <%#crmWebClient.abi_functions.DisplayCompanyInformation(Eval("comp_id"), Eval("comp_address1"), Eval("comp_address2"), Eval("comp_city"), Eval("comp_state"), Eval("comp_zip_code"), Eval("comp_country"), Eval("comp_web_address"))%>

	                   
                    </div> 

                    <!-- info TOP -->
                    <div class="item_info">
	                   
			                 </div>
			               </span>
  <span class="span3"> <figcaption class="pull-right"><a  class="pull-right" href="<%#crmwebclient.abi_functions.AircraftDealerURL(Container.DataItem("comp_id"),Container.DataItem("comp_name"))%>">View Aircraft ></a></figcaption></span>
			             </article><!-- end item -->

					      </ItemTemplate>
		          </asp:Repeater>
	      </div><!-- end row -->

	   </section>   
   </main>
  </div>
</asp:Content>
