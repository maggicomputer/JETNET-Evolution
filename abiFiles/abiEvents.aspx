<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/ABITheme.Master"
  CodeBehind="abiEvents.aspx.vb" Inherits="crmWebClient.abiEvents" %>

<%@ MasterType VirtualPath="~/EvoStyles/ABITheme.Master" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="main_content">
  <div id="component" class="span9">
    <main role="main">
     <section class="page-blog page-blog__">
		    <header class="page_header">
		      <h1><span runat="server" id="news_header">Upcoming Aviation Events</span> </h1>	
		    </header>
		    <div class="items-row row-0 row-fluid">
		          <asp:Repeater runat="server" ID="eventRepeater">
		            <ItemTemplate>

			            <article class="item column-1">
				            <!-- Intro image -->    
				            <span class="span3">
                    <figure class="item_img img-intro">
		                    <a href="#">
		                    
		                    
			                    <img width="250" src="<%= HttpContext.Current.Session.Item("jetnetFullHostName").ToString %>abiFiles/images/blank.gif" class="lazy" data-src="<%= iif(HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL, "https://www.jetnetevolution.com/pictures",HttpContext.Current.Session.Item("jetnetFullHostName").ToString & HttpContext.Current.Session.Item("ABIPhotosFolderVirtualPath"))%>/events/<%#Container.DataItem("abievent_id")%>.jpg" alt=""/></span></a></figure></span>
			                    <span class="span9"><figcaption><%#iif(not isdbnull(Container.DataItem("abievent_start_date")),  Format(Container.DataItem("abievent_start_date"), "MM/dd/yyyy"), "")%> <%#iif(not isdbnull(Container.DataItem("abievent_end_date")), " - " & Format(Container.DataItem("abievent_end_date"), "MM/dd/yyyy"), "")%> | <%#Container.DataItem("abievent_location")%></figcaption>
                    <!--  title/author -->
                    <header class="item_header">
	                    <h4 class="item_title">		 
	                      <%#IIf(not IsDBNull(Container.DataItem("abievent_web_address")), iif(instr(Container.DataItem("abievent_web_address"),"http://") > 0, "<a href='" & Container.DataItem("abievent_web_address") & "' target='_blank'>", iif(len(Container.DataItem("abievent_web_address")) > 0, "<a href='http://" & Container.DataItem("abievent_web_address") & "' target='_blank'>", ""))  , "test")%>            
	                       <span><%#Container.DataItem("abievent_title")%></span> 
	                      </a>
	                    </h4>
	                  </header>
                    <!-- Introtext -->
                    <div class="item_introtext">
	                    <%#IIf(Not IsDBNull(Container.DataItem("abievent_description")), Container.DataItem("abievent_description"), "")%> [More at: <a href="<%#iif(instr(Container.DataItem("abievent_web_address"),"http://") > 0, Container.DataItem("abievent_web_address"),"http://" & Container.DataItem("abievent_web_address"))%>" target="_blank"><%#Container.DataItem("abievent_web_address")%></a>]
                    </div>

                    <!-- info TOP -->
                    <div class="item_info">
	                    <dl class="item_info_dl">
		                    <dt class="article-info-term"></dt>
				                    <dd>
			                        <time datetime="<%#Container.DataItem("abievent_start_date")%>" class="item_published"><%#iif(not isdbnull(Container.DataItem("abievent_start_date")),  Format(Container.DataItem("abievent_start_date"), "MM/dd/yyyy"), "")%> <%#iif(not isdbnull(Container.DataItem("abievent_end_date")), " - " & Format(Container.DataItem("abievent_end_date"), "MM/dd/yyyy"), "")%></time>
		                        </dd>
			                </dl>
			                 </div>
			               </span>

			             </article><!-- end item -->

					      </ItemTemplate>
		          </asp:Repeater>
	      </div><!-- end row -->

	   </section>   
   </main>
  </div>
</asp:Content>
