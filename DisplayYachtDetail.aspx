<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DisplayYachtDetail.aspx.vb"
  Inherits="crmWebClient.DisplayYachtDetail" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE&sensor=false"></script>

  <script language="javascript" type="text/javascript" src="https://www.google.com/jsapi?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE"></script>

  <script language="javascript" type="text/javascript">

    var bDontClose = true;

    function reloadDetailsPage() {
      window.location.href = window.location.href;
    }

  </script>

  <script type="text/javascript">
    google.load('visualization', '1', { packages: ['corechart'] });
  </script>

  <script type="text/javascript">
    var data;
    var data_bar;
    function drawVisualization() {

      // Create and draw the visualization.
      new google.visualization.LineChart(document.getElementById('visualization')).
            draw(data, { curveType: "function",
              width: 500, height: 300,
              pointSize: 3,
              vAxis: { maxValue: 20 }
            }
                );
    }

    function drawBarVisualization() {

      // Create and draw the visualization.
      new google.visualization.ColumnChart(document.getElementById('visualization_bar')).
          draw(data_bar,
               { width: 500, height: 300,
                 hAxis: { title: " " },
                 vAxis: { title: "Clicks", minValue: -1 }
               }
          );
    }


  </script>
    <style>
        #previousAC, #nextAC {font-size:11px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:TextBox runat="server" ID="parent_page_name" Text="" Style="display: none;" />
   <asp:TextBox runat="server" ID="parent_check_page_name" Text="YACHT_LISTING" Style="display: none;" />
  <asp:Panel runat="server" ID="history_background" CssClass="">
  </asp:Panel>
  <div id="divLoading" class="display_none">
    <div class="loadingScreenPage">
      <span>Please wait while the page is loading... </span>
      <br />
      <br />
      <img src="Images/loading.gif" alt="Loading..." /><br />
    </div>
    <br />
    <br />
  </div>
  <div runat="server" id="toggle_vis" class="aircraftContainer">
    <div class="twelve columns">
      <div class="row remove_margin">
        <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width='100%' runat="server"
          class="DetailsBrowseTable">
          <asp:TableRow>
            <asp:TableCell HorizontalAlign="left" VerticalAlign="middle">
              <div class="backgroundShade">
                <span class="float_left">
                  <asp:Label runat="server" ID="PreviousYachtSwap" Visible="false">
                                    <a id="previousAC" value=" < Previous Yacht " tooltip="Click to View the Previous Yacht" class="gray_button"> < Previous Yacht </a>
                  </asp:Label>
                </span>
                <asp:UpdatePanel ID="control_update_panel" runat="server" ChildrenAsTriggers="true">
                  <ContentTemplate>
                    <table width="100%" cellpadding="0" cellspacing="0">
                      <tr>
                        <td align="left" valign="top">
                          <asp:LinkButton ID="view_notes" runat="server" Visible="false" CssClass="blue_button float_left noBefore"
                            OnClick="ViewYachtNotes"><strong>Notes/Actions</strong></asp:LinkButton>
                          <asp:LinkButton ID="view_folders" runat="server" CssClass="gray_button float_left"
                            OnClick="ViewYachtFolders" Visible="true"><strong>Folders</strong></asp:LinkButton>
                          <asp:LinkButton ID="view_yacht_events" runat="server" CssClass="gray_button float_left"
                            OnClick="ViewYachtEvents"><strong>Events</strong></asp:LinkButton>
                          <asp:LinkButton ID="view_analytics" runat="server" CssClass="gray_button float_left"
                            OnClick="ViewYachtAnalytics" Visible="true"><strong>Analytics</strong></asp:LinkButton>
                          <asp:LinkButton ID="map_this_yacht" runat="server" CssClass="gray_button float_left"
                            Visible="false"><strong>Map Yacht</strong></asp:LinkButton>
                        </td>
                        <td align="left" valign="top">
                          <ul id="cssExportMenu" runat="server" class="cssMenu_subpage">
                            <li><a href="#" class="gray_button"><strong>JETNET Export/Report</strong></a>
                              <ul>
                                <li class="display_none">
                                  <asp:Label ID="single_spec_link" runat="server"></asp:Label></li>
                                <li class="display_none">
                                  <asp:Label ID="condensed_spec_link" runat="server"></asp:Label></li>
                                <li>
                                  <asp:Label ID="full_spec_link" runat="server"></asp:Label></li>
                              </ul>
                            </li>
                          </ul>
                        </td>
                      </tr>
                    </table>
                  </ContentTemplate>
                </asp:UpdatePanel>
                <span class="float_right">
                  <asp:Label runat="server" ID="NextYachtSwap" CssClass="float_right" Visible="false">

                                    <a id="nextAC" type="button" class="gray_button" tooltip="Click to View the Next Yacht">Next Yacht > </a>
                  </asp:Label><a href="#" class="gray_button float_right" onclick="javascript:window.close();"><strong>Close</strong></a></span>
              </div>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell ColumnSpan="3" HorizontalAlign="Center" VerticalAlign="Middle">
              <asp:Panel runat="server" CssClass="NotesHeader" BackColor="#4d4d4d" ForeColor="White"
                ID="recordsOf" Visible= "false">
                <asp:Label ID="browseTableTitle" runat="server" Text=""></asp:Label>
                <asp:Label runat="server" ID="browse_label">Record
                  <asp:Label ID="currentRecLabel" runat="server" Text="1"></asp:Label>
                  of
                  <asp:Label ID="totalRecLabel" runat="server" Text="1"></asp:Label>
                  found</asp:Label></asp:Panel>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
      </div>
      <div class="row">
        <div class='six columns Main_Aircraft_Display_Table remove_margin'>
          <cc1:TabContainer ID="history_information" runat="server" Visible="false" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="history_information_panel" runat="server" HeaderText="HISTORY"
              Visible="true">
              <ContentTemplate>
                <asp:Label ID="history_information_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="aircraft_stats" runat="server" Visible="true" CssClass="dark-theme">
            <cc1:TabPanel ID="stats_tab" runat="server" Visible="true" HeaderText="">
              <ContentTemplate>
                <table cellpadding='0' cellspacing='0' width='100%'>
                  <asp:Label ID="aircraft_information_label" runat="server" Text=""></asp:Label>
                  <asp:Label ID="shipyard_company_name" runat="server" Text=""></asp:Label>
                </table>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="model_tab_container" runat="server" Visible="false" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="model_tab" runat="server" HeaderText="MODEL" Visible="true">
              <ContentTemplate>
                <asp:Label ID="model_tab_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="status_tab_container" runat="server" Visible="true" AutoPostBack="false">
            <cc1:TabPanel ID="status_tab" runat="server" HeaderText="STATUS: " Visible="true">
              <ContentTemplate>
                <asp:Label ID="aircraft_status_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <asp:UpdatePanel ID="notes_update_panel" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
              <cc1:TabContainer ID="Notes" runat="server" Visible="true" CssClass="blue-theme"
                AutoPostBack="false">
                <cc1:TabPanel ID="notes_panel" runat="server" HeaderText="NOTES" Visible="true">
                  <ContentTemplate>
                    <asp:Label ID="notes_label" runat="server" Text=""></asp:Label>
                    <asp:Label ID="notes_add_new" runat="server"></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <cc1:TabContainer ID="Reminders" runat="server" Visible="true" CssClass="blue-theme"
                AutoPostBack="false">
                <cc1:TabPanel ID="action_panel" runat="server" HeaderText="ACTION ITEMS" Visible="true">
                  <ContentTemplate>
                    <asp:Label ID="action_label" runat="server" Text=""></asp:Label>
                    <asp:Label ID="action_add_new" runat="server"></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <asp:LinkButton runat="server" ID="closeNotes" CssClass="float_right padding" OnClick="ViewYachtNotes"
                Visible="false">Close Notes/Actions</asp:LinkButton>
            </ContentTemplate>
          </asp:UpdatePanel>
          <cc1:TabContainer ID="feature_tab_container" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="feat_tab" runat="server" HeaderText="DIMENSIONS AND SIZE" Visible="true">
              <ContentTemplate>
                <asp:Label ID="features_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="performace_container" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="performance_name_tab" runat="server" HeaderText="PERFORMANCE AND CAPABILITIES"
              Visible="true">
              <ContentTemplate>
                <asp:Label ID="performance_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="helipad_tab_container" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="helipad_tab" runat="server" HeaderText="HELIPAD" Visible="true">
              <ContentTemplate>
                <asp:Label ID="helipad_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="engine_tab_container" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="engine_tab" runat="server" HeaderText="POWER" Visible="true">
              <ContentTemplate>
                <asp:Label ID="engine_tab_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="maintenance_tab_container" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="maintentance_tab" runat="server" HeaderText="MAINTENANCE" Visible="true">
              <ContentTemplate>
                <asp:Label ID="maint_label" runat="server" Text="" Visible="true"></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="bridge_tab_container" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="bridge_tab" runat="server" HeaderText="BRIDGE" Visible="true">
              <ContentTemplate>
                <asp:Label ID="bridge_tab_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="systems_tab_container" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="systems_tab_panel" runat="server" HeaderText="SYSTEMS" Visible="true">
              <ContentTemplate>
                <asp:Label ID="systems_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="interior_tab_container" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="interior_tab" runat="server" HeaderText="INTERIOR" Visible="true">
              <ContentTemplate>
                <asp:Label ID="interior_tab_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="exterior_tab_container" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="exterior_tab_panel" runat="server" HeaderText="EXTERIOR" Visible="true">
              <ContentTemplate>
                <asp:Label ID="exterior_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="equipment_tab_container" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="equipment_tab" runat="server" HeaderText="EQUIPMENT" Visible="true">
              <ContentTemplate>
                <asp:Label ID="equip_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="amenities_tab_container" runat="server" Visible="false" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="amenities_tab_panel" runat="server" HeaderText="AMENITIES" Visible="true">
              <ContentTemplate>
                <asp:Label ID="amenities_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="usage_tab_container" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="usage_tab" runat="server" HeaderText="USAGE" Visible="true">
              <ContentTemplate>
                <asp:Label ID="aircraft_usage_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
        </div>
        <div class="six columns Main_Information">
          <asp:UpdatePanel ID="folders_update_panel" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
              <cc1:TabContainer ID="folders_container" runat="server" Visible="false" CssClass="dark-theme"
                AutoPostBack="false" Width="515px" Height="300px">
                <cc1:TabPanel ID="folders_tab" runat="server" HeaderText="FOLDERS" Visible="true"
                  CssClass="small_panel_height">
                  <ContentTemplate>
                    <asp:Label ID="folders_label" runat="server" Text="" CssClass="small_panel_height"></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <asp:LinkButton runat="server" ID="closeFolders" CssClass="float_right padding" OnClick="ViewYachtFolders"
                Visible="false">Close Folders</asp:LinkButton>
            </ContentTemplate>
          </asp:UpdatePanel>
          <asp:UpdatePanel ID="analytic_update_panel" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
              <cc1:TabContainer ID="analytic_container" runat="server" Visible="true" CssClass="display_none"
                AutoPostBack="false" Width="515px" Height="350px">
                <cc1:TabPanel ID="analytic_tab" runat="server" HeaderText="ANALYTICS" Visible="true">
                  <ContentTemplate>
                    <asp:Panel runat="server" Style="height: 350px; overflow-y: auto; overflow-x: hidden;">
                      <div class="data_aircraft_grid">
                        <div class="header_row medium_text text_align_center padding ">
                          <b>Clicks per Month (Last 12 Months)</b></div>
                      </div>
                      <div id="visualization" style="width: 500px; height: 300px;">
                      </div>
                      <div>
                        <asp:Label ID="analytic_label" runat="server" Text="" CssClass="panel_no_height"></asp:Label>
                      </div>
                    </asp:Panel>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <asp:LinkButton runat="server" ID="closeAnalytics" CssClass="float_right padding"
                OnClick="ViewYachtAnalytics" Visible="false">Close Analytics</asp:LinkButton>
            </ContentTemplate>
          </asp:UpdatePanel>
          <asp:UpdatePanel ID="events_update_panel" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
              <cc1:TabContainer ID="events_container" runat="server" Visible="false" CssClass="dark-theme"
                AutoPostBack="false" Width="515px" Height="300px">
                <cc1:TabPanel ID="events_tab" runat="server" HeaderText="EVENTS" Visible="true" CssClass="small_panel_height">
                  <ContentTemplate>
                    <asp:Label ID="events_label" runat="server" Text="" CssClass="small_panel_height"></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
              <div class="div_clear">
              </div>
              <asp:LinkButton runat="server" ID="closeEvents" CssClass="float_right padding" OnClick="ViewYachtEvents"
                Visible="false">Close Events</asp:LinkButton>
            </ContentTemplate>
          </asp:UpdatePanel>
          <asp:Label ID="aircraft_picture_slideshow" runat="server" Text="" Visible="false"></asp:Label>
          <asp:Label ID="all_pics" runat="server" Visible="false"></asp:Label>
          <cc1:TabContainer ID="description_container" runat="server" Visible="false" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="description_panel" runat="server" HeaderText="DESCRIPTION" Visible="true">
              <ContentTemplate>
                <asp:Label ID="description_label" CssClass="padding square_ul" runat="server"></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="company_tab_container" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="company_tab_panel" runat="server" HeaderText="COMPANY / CONTACTS / PEDIGREE"
              Visible="true">
              <ContentTemplate>
                <asp:Label ID="yacht_contacts_label" runat="server"></asp:Label></ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="previous_name_containter" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="previous_name_tab" runat="server" HeaderText="PREVIOUS NAMES" Visible="true">
              <ContentTemplate>
                <asp:Label ID="previous_name_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="compliance_cert" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="compliance_cert_tab" runat="server" HeaderText="COMPLIANCE AND CERTIFICATIONS"
              Visible="true">
              <ContentTemplate>
                <asp:Label ID="compliance_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="history_container" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="history_Tab" runat="server" HeaderText="HISTORY" Visible="true">
              <ContentTemplate>
                <asp:Label ID="history_label" runat="server"></asp:Label></ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <cc1:TabContainer ID="news_container" runat="server" Visible="true" CssClass="dark-theme"
            AutoPostBack="false">
            <cc1:TabPanel ID="news_tab" runat="server" HeaderText="NEWS" Visible="true">
              <ContentTemplate>
                <asp:Label ID="news_label" runat="server"></asp:Label></ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>
          <asp:Label ID="aircraft_details_bottom" runat="server" Text=""></asp:Label>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">
  <link href="common/contentslider.css" rel="stylesheet" type="text/css" />

  <script type="text/javascript" src="common/contentslider.js">
    /***********************************************
    * Featured Content Slider- (c) Dynamic Drive DHTML code library (www.dynamicdrive.com)
    * This notice MUST stay intact for legal use
    * Visit Dynamic Drive at http://www.dynamicdrive.com/ for this script and 100s more
    ***********************************************/
  </script>

  <script type="text/javascript" src="common/stepcarousel.js">
    /***********************************************
    * Step Carousel Viewer script- (c) Dynamic Drive DHTML code library (www.dynamicdrive.com)
    * Visit http://www.dynamicDrive.com for hundreds of DHTML scripts
    * This notice must stay intact for legal use
    ***********************************************/
  </script>

  <asp:Literal runat="server" ID="slideshow_script" Visible="false">
                    <script type="text/javascript">

                      featuredcontentslider.init({
                        id: "slider1",  //id of main slider DIV
                        contentsource: ["inline", ""],  //Valid values: ["inline", ""] or ["ajax", "path_to_file"]
                        toc: "#increment",  //Valid values: "#increment", "markup", ["label1", "label2", etc]
                        nextprev: ["", ""],  //labels for "prev" and "next" links. Set to "" to hide.
                        revealtype: "click", //Behavior of pagination links to reveal the slides: "click" or "mouseover"
                        enablefade: [true, 0.1],  //[true/false, fadedegree]
                        autorotate: [true, 10000],  //[true/false, pausetime]
                        onChange: function(previndex, curindex) {  //event handler fired whenever script changes slide
                          //previndex holds index of last slide viewed b4 current (1=1st slide, 2nd=2nd etc)
                          //curindex holds index of currently shown slide (1=1st slide, 2nd=2nd etc)
                        }
                      })

        </script> </asp:Literal>
  <asp:Literal ID="step_script" runat="server" Visible="false">
                    <script type="text/javascript">
                      stepcarousel.setup({
                        galleryid: 'mygallery', //id of carousel DIV
                        beltclass: 'belt', //class of inner "belt" DIV containing all the panel DIVs
                        panelclass: 'panel', //class of panel DIVs each holding content
                        autostep: { enable: true, moveby: 1, pause: 3000 },
                        panelbehavior: { speed: 500, wraparound: false, wrapbehavior: 'slide', persist: false },
                        defaultbuttons: { enable: true, moveby: 1, leftnav: ['images/previous.png', -2, 50], rightnav: ['images/next.png', -13, 50] },
                        statusvars: ['statusA', 'statusB', 'statusC'], //register 3 variables that contain current panel (start), current panel (last), and total panels
                        contenttype: ['inline'] //content setting ['inline'] or ['ajax', 'path_to_external_file']
                      })

        </script> </asp:Literal>
  
  <script language="javascript" type="text/javascript">
  
    function ToggleVis() {
      document.getElementById("<%= toggle_vis.clientID %>").className = "display_none";
      ToggleButtons();
      document.getElementById("divLoading").className = "display_block";
    }

  </script>

  <script type="text/javascript">

    window.onload = function() {
      var parent = '';
      if ((window.opener) && (window.opener.location)) {
        parent = String(window.opener.location);
        parent = parent.toUpperCase();
      }

      document.getElementById("<%= parent_page_name.clientID %>").value = parent;
      var n = parent.indexOf(document.getElementById("<%= parent_check_page_name.clientID %>").value);
      var hist = parent.indexOf("H=1");
      var even = parent.indexOf("E=1");


      var invis = false

      if (n == -1) {
        invis = true;
      }

      if (hist != -1) {
        invis = true;
      }

      if (even != -1) {
        invis = true;
      }


      if (invis == true) 
        var nextElement = document.getElementById("nextAC");
        if (nextElement != null) {
          document.getElementById("nextAC").style.display = 'none';
        }
        //check for previous
        var previousElement = document.getElementById("previousAC");
        if (previousElement != null) {
          document.getElementById("previousAC").style.display = 'none';
        }
        //check if browse element exists.
        var browseElement = document.getElementById("<%= browse_label.clientID %>");
        if (browseElement != null) {
          document.getElementById("<%= browse_label.clientID %>").style.display = 'none';
        }
      }


    }

    
  </script>
</asp:Content>
