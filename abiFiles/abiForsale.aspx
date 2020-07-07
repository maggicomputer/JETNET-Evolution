<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/ABITheme.Master"
  CodeBehind="abiForsale.aspx.vb" Inherits="crmWebClient.abiForsale" %>

<%@ MasterType VirtualPath="~/EvoStyles/ABITheme.Master" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="main_content">
  <div id="aside_right" class="span2" runat="server">
    <aside role="complementary">
         <div class="moduletable ">
            <header>
              <h3 class="moduleTitle " runat="server" id="leftHeaderText">
                <span >Dealers</span> 
              </h3>
            </header>
            
            <ul id="leftCategory" class="categories-module" runat="server">
              <asp:Repeater runat="server" ID="dealersRepeater">
                <ItemTemplate> 
                  <li><a href="<%#crmwebclient.abi_functions.AircraftDealerURL(Container.DataItem("comp_id"),Container.DataItem("comp_name"))%>"><b><%#Container.DataItem("comp_name")%></b></a><br /><%#iif(not isdbnull(Container.DataItem("comp_city")), Container.DataItem("comp_city") & ", ","")%> <%#Container.DataItem("comp_state")%></li>
                </ItemTemplate>
              </asp:Repeater> 
            </ul>
            <asp:Literal runat="server" id="companyInformation"></asp:Literal>
          </div>

  </aside>
  </div>
  <div id="component" class="span7" runat="server">
    <main role="main">
     <section class="page-blog page-blog__">
		    <header class="page_header">
		      <h1><span runat="server" id="ac_header">Aircraft For Sale</span> </h1>	
		    </header>
		<asp:Literal runat="server" ID="acIncludedText" Visible="false">
		      <div class="items-row row-0 row-fluid"><h4 class="pull-left LeftBuffer">Want your Aircraft included here?</h4><figcaption class="pull-right"><a href="/abiFiles/abiContact.aspx?inquiry=true" class="pull-right">Post your listing today ></a></figcaption></div>
<hr /></asp:Literal>
		    <div class="items-row cols-2 row-0 row-fluid">
		       <asp:Literal runat="server" id="acListLiteral"></asp:Literal>
	     </div>
	      <div class="items-row row-0 row-fluid">
		       <asp:Literal runat="server" id="acDetailedList"></asp:Literal>
	     </div>
	     <asp:Literal runat="server" id="companyWantedLiteral"></asp:Literal>
		    <asp:Literal runat="server" id="moreModelInformationLiteral"></asp:Literal>
 <div runat="server" visible="false" id="viewAllDiv"><br clear="all" />
        <strong><a href="/abiFiles/abiForSale.aspx">View All</a></strong>
      </div>
        <asp:Literal runat="server" id="newsByMake"></asp:Literal>
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
		 </div>  
	   </section>   
	   
   </main>
  </div>
</asp:Content>
