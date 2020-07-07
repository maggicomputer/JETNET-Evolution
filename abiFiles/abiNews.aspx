<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/ABITheme.Master" CodeBehind="abiNews.aspx.vb" Inherits="crmWebClient.abiNews" %>

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
              <asp:Repeater runat="server" ID="newsCategories">
                <ItemTemplate>
                  <li><a href="?topic=<%#Container.DataItem("abinews_id")%>"><%#Container.DataItem("abinews_topic")%> </a></li>
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
		      <h1><span runat="server" id="news_header">Latest Aviation News</span> </h1>	
		    </header>
		    <div class="items-row cols-1 row-0 row-fluid">
		    <asp:Repeater runat="server" ID="newsRepeater">
		      <ItemTemplate>
		        	<div class="span12">
			      <article class="item column-1">
				      <!-- Intro image -->
              <figcaption><%#Container.DataItem("abinewssrc_name")%></figcaption>
              <!--  title/author -->
              <header class="item_header">
	              <h4 class="item_title">		
	                <a href="<%#Container.DataItem("abinewslnk_web_address")%>" target="new"> 
	                  <span><%#Container.DataItem("abinewslnk_title")%></span> 
	                </a>
	              </h4>
	            </header>
              <!-- Introtext -->
              <div class="item_introtext">
	              <%#IIf(Not IsDBNull(Container.DataItem("abinewslnk_description")), Left(Container.DataItem("abinewslnk_description"), 255) & "...", "")%>[More at: <a href="<%#Container.DataItem("abinewslnk_web_address")%>" target="new"><%#Container.DataItem("abinewssrc_name")%></a>]
              </div>

              <!-- info TOP -->
              <div class="item_info">
	              <dl class="item_info_dl">
		              <dt class="article-info-term"></dt>
				              <dd>
			                  <time datetime="2014-05-02 19:47" class="item_published">
				                 <%#IIf(Not IsDBNull(Container.DataItem("abinewslnk_date")), Format(Container.DataItem("abinewslnk_date"), "MM/dd/yyyy"), "")%>
				                </time>
		                  </dd>
			          </dl>
              </div>

			       </article><!-- end item -->
					 </div>
					</ItemTemplate>
		    </asp:Repeater>
	</div><!-- end row -->

</section>   
                                  </main>
  </div>
  <!-- End Content-top Row -->
</asp:Content>

