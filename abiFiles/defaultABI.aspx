<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="defaultABI.aspx.vb" Inherits="crmWebClient.defaultABI" MasterPageFile="~/EvoStyles/ABITheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/ABITheme.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="main_content">
  <div id="component" class="span9">
    <!--Start Component-->
    <main role="main"><!--Start Main-->
                  <div id="content-top-row" class="row"><!-- Start Content-top Row -->
                    <div id="content-top"> <!--Start Content-top-->
                      <div class="moduletable big  span6 aircraftForSaleBox pull-left"><!--Start ModuleTable-->
                       
                       <h1 class="moduleTitle ">
						 		           <span>Aircraft For Sale</span> 
						           </h1>
						           <div class="pull-right">
						           <div class="content-row">
						           <asp:DropDownList ID="searchMakeModel" runat="server">
						            <asp:ListItem value= "0">All Makes/Models</asp:ListItem>
						           </asp:DropDownList>
						           </div>
						           <div class="content-row">
						           Year Range:  <asp:DropDownList ID="year_start" runat="server" Width="67px">
						           </asp:DropDownList> to  <asp:DropDownList ID="year_end" runat="server" Width="67px">
						           </asp:DropDownList>
						           </div>
						           <div class="content-row">
						            <asp:DropDownList ID="searchDealers" runat="server">
						            <asp:ListItem value= "0">All ABI Dealers</asp:ListItem>
						           </asp:DropDownList>
						           </div>
						           </div>
						           
						           <div class="pull-left">
						           <ul>
						              <li><a href="abiForsale.aspx?type=Executive&AirframeType=F&AirType=E">Executive Airliners for Sale</a></li>
						              <li><a href="abiForsale.aspx?type=Jets&AirframeType=F&AirType=J">Jets for Sale</a></li>
						              <li><a href="abiForsale.aspx?type=Turboprops&AirframeType=F&AirType=T">Turboprops for Sale</a></li>
						              <li><a href="abiForsale.aspx?type=Pistons&AirframeType=F&AirType=P">Piston Aircraft for Sale</a></li>
						              <li><a href="abiForsale.aspx?type=Helicopters&AirframeType=R">Helicopters for Sale</a></li>           
						           </ul>
						           <asp:Button id="findAircraft" runat="server" Text="Find Aircraft" CssClass="pull-right" />
						           </div>

</div><!---End Moduletable-->

                      <!--Start Column moduletable popular (right of slideshow). Featured Aircraft-->
                      <div class="moduletable popular pull-right span3">
		                       <header>
		 				                       <h3 class="moduleTitle ">
						 		                       <span>Featured Aircraft</span> 
						                       </h3>
		                       </header>
                      		 
		                       <div class="mod-newsflash-adv news mod-newsflash-adv__ popular cols-1"><!--Start popular pull-right-->
		                           <!--AC-->
		                           <asp:Repeater ID="featuredAircraftRepeater" runat="server">
                                   <ItemTemplate>
		 			                          <div class="row-fluid"><!--Start Row--> 
							                             <article class="span4 item item_num1 item__module"><!--Start Aircraft-->
							                               <figure class="item_img img-intro img-intro__left">
                                              <a href="<%# crmWebClient.abi_functions.AircraftDetailsURL(Container.DataItem("ac_id"), Container.DataItem("ac_year") , Container.DataItem("amod_make_name") , Container.DataItem("amod_model_name"), Container.DataItem("ac_reg_no"))%>/">
                                                <img class="featuredACimage" src="<%# IIf(HttpContext.Current.Session.Item("jetnetWebSiteType") <> crmWebClient.eWebSiteTypes.LOCAL, HttpContext.Current.Session.Item("jetnetFullHostName").ToString & HttpContext.Current.Session.Item("AircraftPicturesFolderVirtualPath") & "/", "https://www.testjetnetevolution.com/pictures/aircraft/") %><%#Container.DataItem("ac_id")%>-0-<%#Container.DataItem("acpic_id")%>.jpg" class="lazy" alt=""/>
                                              </a>
                                             </figure>
                                            <div class="item_content"><!--Item Content-->
                  		                          <figcaption><a href="<%# crmWebClient.abi_functions.AircraftDetailsURL(Container.DataItem("ac_id"), Container.DataItem("ac_year") , Container.DataItem("amod_make_name") , Container.DataItem("amod_model_name"), Container.DataItem("ac_reg_no"))%>/"><%#Container.DataItem("amod_make_name")%> <%#Container.DataItem("amod_model_name")%></a></figcaption>
                          											<div class="clear"></div>
                  		                          <h4 class="item_title item_title__ popular pull-left"><!--Start Item Title-->
                  				                          <a href="<%#crmwebclient.abi_functions.AircraftDealerURL(Container.DataItem("comp_id"),Container.DataItem("comp_name"))%>">By <%#Container.DataItem("comp_name")%></a>
                  		                          </h4><!--End Item Title-->
                          											<div class="clearfix"></div>
                  	                            <div class="item_introtext"><%#Container.DataItem("ac_year")%> - <%#Container.DataItem("ac_country_of_registration")%></div>
                  	                          </div><!--End Item Content-->
									                          <div class="clearfix"></div><!--Clearing After Article-->  
		 						                          </article><!--End Aircraft-->
							                          </div><!--End Row-->
							                     </ItemTemplate>
							                  </asp:Repeater>
			                          <!--End AC-->
	                              <div class="mod-newsflash-adv_custom-link"><!--Start See All Link-->
                                  <a class="btn btn-info" href="abiForsale.aspx">See all Aircraft for Sale</a>  
        	                      </div><!--End See All Link-->
			                      </div><!--End popular pull-right-->
                         </div><!--End Column moduletable popular (right of slideshow). Featured Aircraft-->
                      
                      <!--Start Latest Two Articles (with Pictures)-->
                      <div id="latest_articles" class="moduletable small span6 pull-left">
                        <div class="mod-newsflash-adv type mod-newsflash-adv__ small cols-2">
                          <div class="row-fluid">
                           
		                      <asp:Repeater ID="latest_articles_holder_repeater" runat="server">
                              <ItemTemplate>
                         
                                   <article class="span3 item">
                                      <div class="item_content">
                                        <figure class="item_img img-intro">
                                          <a href="<%#Container.DataItem("abinewslnk_web_address")%>" target="_blank">
                                            <img class="lazy" src="<%#Container.DataItem("picture")%>" onerror="if (this.src != '/images/background/54.jpg') {this.src='/images/background/54.jpg'};" />
                                            <figcaption><%#Container.DataItem("abinewssrc_name")%></figcaption>
                                          </a>
                                        </figure>
                                        <div class="itemInner">
                                           
                                         <h4 class="item_title item_title__ small latestAviationNewsTitle">
                                            <a class="custom_hover" href="<%#Container.DataItem("abinewslnk_web_address")%>" target="_blank">
                                              <span>
                                                <span class="white"><%#Container.DataItem("abinewslnk_title")%></span>
                                                <strong></strong>
                                              </span>
                                            </a>
                                          </h4>
                                           
                                          <div class="item_info">
                                            <dl class="item_info_dl">
                                              <dt class="article-info-term"></dt>
                                              <dd>
                                                <time datetime="<%#Container.DataItem("dateWithoutTime")%>" class="item_published"><%#Container.DataItem("dateWithoutTime")%></time>
                                              </dd>
                                            </dl>
                                          </div>
                                           
                                         </div>
                                      </div>
                                    </article>
                                   
                               </ItemTemplate>
                          </asp:Repeater>
                          </div>
                        </div>
                      </div>
                      <!--End latest two articles-->

                      <!--Start Latest News-->
                      <div class="moduletable span6 pull-left"><!--Start Moduletable-->
	                      <header>
		                      <h3 class="moduleTitle">Latest Aviation News</h3>
	                      </header>
                      	
	                      <div class="mod-newsflash-adv news mod-newsflash-adv__ cols-1"><!---Start mod-newsflash-adv-->
                            <asp:Repeater ID="latest_aviation_news_repeater" runat="server">
                              <ItemTemplate>
                                <div class="row-fluid">
                                <article class="span6 item item_num0 item__module">

		                            <div class="item_content">
		                           <figcaption><%#Container.DataItem("abinewssrc_name")%></figcaption>
                              	
		                            <h4 class="item_title item_title__"><!--News Title-->
  		                            <a href="<%#Container.DataItem("abinewslnk_web_address")%>" target="new"><%#Container.DataItem("abinewslnk_title")%></a>
  	                            </h4><!--End News Title-->
                            		
  	                            <div class="item_introtext"><!--News Text-->
  		                            <%#Container.DataItem("abinewslnk_description")%>
  		                            [More at: <a href="<%#Container.DataItem("abinewslnk_web_address")%>" target="new"><%#Container.DataItem("abinewssrc_name")%></a>]
  	                            </div><!--End News Text-->
                            	
  	                            <!---Article Time Stamp-->
  	                            <div class="item_info">
  		                            <dl class="item_info_dl">
  			                            <dt class="article-info-term"></dt>
  				                            <dd>
  						                            <time datetime="<%#Container.DataItem("abinewslnk_date")%>" class="item_published"><%#Format(Container.DataItem("abinewslnk_date"), "MM/dd/yyyy")%></time>
  			                              </dd>
  		                            </dl>
  	                            </div>
  	                            <!--End Article Time Stamp-->
	                            </div><!--End Item Content-->
                            <div class="clearfix"></div><!--Clear before article end-->
                            </article><!--End Article-->

                            </div><!--End Row-->
	                              <div class="clearfix"></div><!--Clear before article end-->
                               </ItemTemplate>
                             </asp:Repeater>

                        </div>
                        <div class="clearfix"></div><!--Clear after row-->
                              
                            <div class="mod-newsflash-adv_custom-link"><!--See all link-->
                              <a class="btn btn-info" href="/aviationnews/">View all News</a>  
  	                        </div>
                      </div><!---end Moduletable-->
                                            <div class="clear_right"></div>
                      <div class="moduletable pull-right medium   span3">
                        <div class="mod-newsflash-adv type mod-newsflash-adv__pull-right medium  cols-1">
                        <asp:Repeater runat="server" ID="topJetnetArticle">
                          <ItemTemplate>
                            <div class="row-fluid">
                              <article class="span4 item item__module">
                                <div class="item_content">
                                
                                  <figure class="item_img img-intro img-intro__none">
                                    <a href="<%#iif(instr(Container.DataItem("evonot_doc_link"),"http://") > 0, Container.DataItem("evonot_doc_link"),"http://" & Container.DataItem("evonot_doc_link"))%>" target="_blank">
                                      <img src="<%#Container.DataItem("picture")%>" class="lazy" alt="" />
                                      <figcaption>JETNET</figcaption>
                                    </a>
                                  </figure>
                                  <div class="itemInner">
                                       
                                      <h4 class="JetnetHeaderSpecialNews">
                                        <a class="custom_hover" href="<%#iif(instr(Container.DataItem("evonot_doc_link"),"http://") > 0, Container.DataItem("evonot_doc_link"),"http://" & Container.DataItem("evonot_doc_link"))%>" target="_blank">
                                          <span>
                                            <span class="white"><%#Container.DataItem("evonot_title")%></span>
                                          </span>
                                       </a>
                                      </h4>

                                      <div class="item_info">
                                         <dl class="item_info_dl white">
                                            <dt class="article-info-term"></dt>
                                            <dd>
                                              <time datetime="<%#Container.DataItem("dateWithoutTime")%>" class="item_published"><%#Container.DataItem("dateWithoutTime")%></time>
                                            </dd>
                                         </dl>
                                      </div>
                               
                              </div>
                                </div>
                                <div class="clearfix"></div> 
                              </article>
                            </div>
                            <div class="clearfix"></div>
                          </ItemTemplate>
                        </asp:Repeater>
                        </div>
                      </div>
                      <div class="clear_right"></div>
                      <div class="moduletable span3 pull-right"><!--Start ModuleTable (Events)-->
		                       <header><!--Start Header-->
		 		                      <h3 class="moduleTitle ">
						                      <span>JETNET News</span> 
				                      </h3>
		                       </header><!--End Header-->
                      				
		                      <div class="mod-newsflash-adv news mod-newsflash-adv__ cols-1"><!--Start mod-newsflash-adv-->
		                        <asp:Repeater ID="jetnetNewsRepeater" runat="server">
                              <ItemTemplate>
    	                          <div class="row-fluid"><!--Start Event Row-->
   			                      <article class="span4 item item_num0 item__module"><!--Start Event-->
					                      <div class="item_content"><!--Start Item Content-->

        		                        <h4 class="item_title item_title__"><!--Start Event Title-->
        				                      <a href="<%#iif(instr(Container.DataItem("evonot_doc_link"),"http://") > 0, Container.DataItem("evonot_doc_link"),"http://" & Container.DataItem("evonot_doc_link"))%>" target="_blank"><%#Container.DataItem("evonot_title")%></a>
        			                      </h4><!--End Event Title-->
                      							
							                      <div class="item_introtext"><%#Container.DataItem("evonot_description")%></div><!--End Event Intro Text-->

            	                      <div class="item_info"><!--Start Item Date-->
            		                      <dl class="item_info_dl">
            			                      <dt class="article-info-term"></dt>
            				                      <dd>
            					                      <time datetime="<%#Container.DataItem("evonot_release_date")%>" class="item_published"><%#Format(Container.DataItem("evonot_release_date"), "MM/dd/yyyy")%></time>
            			                        </dd>			
            		                      </dl>
            	                      </div><!--End Item Date-->
					                      </div><!--End Item Content-->
					                      <div class="clearfix"></div>  
			                      </article><!--End Event-->
                        </div><!--End Event Row-->
                              </ItemTemplate>
                            </asp:Repeater>
                            <div class="clearfix"></div><!--Clear after Events-->
                         
                        
	                      </div><!--End mod-newsflash-adv-->
                      </div><!--End ModuleTable (Events)-->
                      <!--Events Table-->
                      <div class="moduletable span6 pull-left"><!--Start Moduletable-->
	                      <header>
		                      <h3 class="moduleTitle">Upcoming Aviation Events</h3>
	                      </header>
                      	
	                      <div class="mod-newsflash-adv news mod-newsflash-adv__ cols-1"><!--Start mod-newsflash-adv--> 
                            <asp:Repeater ID="eventRepeater" runat="server">
                              <ItemTemplate>
                                <div class="row-fluid">
                                <article class="span6 item item_num0 item__module">
                                      <figure class="item_img img-intro img-intro__left">
                                          <a href="#">
                                             <img src="<%= HttpContext.Current.Session.Item("jetnetFullHostName").ToString %>abiFiles/images/blank.gif" class="lazy" data-src="<%= iif(HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL, "https://www.jetnetevolution.com/pictures",HttpContext.Current.Session.Item("jetnetFullHostName").ToString & HttpContext.Current.Session.Item("ABIPhotosFolderVirtualPath"))%>/events/<%#Container.DataItem("abievent_id")%>.jpg" alt=""/>
                                          </a>
                                       </figure>
		                            <div class="item_content">

                              	 <figcaption><%#iif(not isdbnull(Container.DataItem("abievent_start_date")),  Format(Container.DataItem("abievent_start_date"), "MM/dd/yyyy"), "")%> <%#iif(not isdbnull(Container.DataItem("abievent_end_date")), " - " & Format(Container.DataItem("abievent_end_date"), "MM/dd/yyyy"), "")%> | <%#Container.DataItem("abievent_location")%></figcaption>

		                            <h4 class="item_title item_title__"><!--News Title-->
  		                             <a href="<%#iif(instr(Container.DataItem("abievent_web_address"),"http://") > 0, Container.DataItem("abievent_web_address"),"http://" & Container.DataItem("abievent_web_address"))%>" target="_blank"><%#Container.DataItem("abievent_title")%></a>
  	                            </h4><!--End News Title-->
                            		
  	                            <div class="item_introtext"><!--News Text-->
  		                          	                    <%#IIf(Not IsDBNull(Container.DataItem("abievent_description")), Container.DataItem("abievent_description"), "")%> [More at: <a href="<%#iif(instr(Container.DataItem("abievent_web_address"),"http://") > 0, Container.DataItem("abievent_web_address"),"http://" & Container.DataItem("abievent_web_address"))%>" target="_blank"><%#Container.DataItem("abievent_web_address")%></a>]  	                            </div><!--End News Text-->
                            	
  	                            <!---Article Time Stamp-->
  	                            <div class="item_info">
  		                            <dl class="item_info_dl">
  			                            <dt class="article-info-term"></dt>
  				                            <dd>
  						                            <time datetime="<%#Container.DataItem("abievent_start_date")%>" class="item_published"><%#iif(not isdbnull(Container.DataItem("abievent_start_date")),  Format(Container.DataItem("abievent_start_date"), "MM/dd/yyyy"), "")%> <%#iif(not isdbnull(Container.DataItem("abievent_end_date")), " - " & Format(Container.DataItem("abievent_end_date"), "MM/dd/yyyy"), "")%></time>
  			                              </dd>
  		                            </dl>
  	                            </div>
  	                            <!--End Article Time Stamp-->
	                            </div><!--End Item Content-->
                            <div class="clearfix"></div><!--Clear before article end-->
                            </article><!--End Article-->

                            </div><!--End Row-->
	                              <div class="clearfix"></div><!--Clear before article end-->
                               </ItemTemplate>
                             </asp:Repeater>

                        </div>
                        <div class="clearfix"></div><!--Clear after row-->
                          <div class="mod-newsflash-adv_custom-link"><!--Start See All Link-->
                                  <a class="btn btn-info" href="/aviationevents/">See all Events</a>  
        	                      </div><!--End See All Link-->
                      </div><!---end Moduletable-->
                      <!--End Events Table-->


                    </div> <!--End Content-top-->
                </div><!--End Content-top-->
    </main>
  </div>
  <!-- End Content-top Row -->
</asp:Content>
