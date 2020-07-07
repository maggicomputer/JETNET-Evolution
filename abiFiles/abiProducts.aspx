<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/ABITheme.Master" CodeBehind="abiProducts.aspx.vb" Inherits="crmWebClient.abiProducts" %>

<%@ MasterType VirtualPath="~/EvoStyles/ABITheme.Master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="main_content">
<div id="aside-right" class="span2">
    <aside role="complementary">
         <div class="moduletable ">
            <header>
              <h3 class="moduleTitle ">
                <span>Category</span> 
              </h3>
            </header>
            
            <ul class="categories-module">
              <asp:Repeater runat="server" ID="productsCategories">
                <ItemTemplate>
                  <li><a href="?topic=<%#Server.UrlEncode(Container.DataItem("abiserv_subgroup"))%>"><%#Container.DataItem("abiserv_subgroup")%> </a></li>
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
		      <span runat="server" id="product_header"><h1>Products</h1></span>	
		    </header>
		    <p>For additional information regarding the products and services below or a customized aviation mail/email list please contact JETNET LLC for more information at 1-800.553.8638 US TOLL FREE or 41-0-43-243-7056 for International.</p>
		    <div class="items-row row-0 row-fluid">
		         <asp:Literal runat="server" ID="productText"></asp:Literal>
	      </div><!-- end row -->
	     </section>   
    </main>
  </div>
  <!-- End Content-top Row -->  
</asp:Content>
