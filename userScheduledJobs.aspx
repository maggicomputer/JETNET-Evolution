<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" CodeBehind="userScheduledJobs.aspx.vb" Inherits="crmWebClient.userScheduledJobs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">



  <script type="text/javascript" src="https://cdn.rawgit.com/Mikhus/canvas-gauges/gh-pages/download/2.1.4/all/gauge.min.js"></script>

  <script type="text/javascript" src="https://www.google.com/jsapi?autoload={'modules':[{'name':'visualization','version':'1.0','packages':['corechart']},{'name':'visualization','version':'1.0','packages':['controls']}]}"></script>


  <script>

    google.load('visualization', '1', { packages: ['corechart'] });
  </script>
  <style type="text/css">
    .viewValueExport.Simplistic .formatTable.blue td b {
      font-weight: bold;
    }

    .switchToggle {
      margin-left: 0px;
      width: 45px;
      height: 20px;
    }

    input:checked + .sliderToggle:before {
      left: 1px;
    }

    .sliderToggle:before {
      height: 14px;
      width: 15px;
      bottom: 3px;
    }

    .fa-trash-o::before {
      font-size: 20px;
      float: right;
    }

    .red_text {
      text-align: left;
      padding-top: 15px;
      display: block;
    }

    .hiddenPopupDiv {
      z-index: 1000;
      position: absolute;
      display: block;
      padding: 10px;
      background-color: #7eb6d4;
    }

      .hiddenPopupDiv .Box {
        margin-top: 0px !important;
      }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width='100%' runat="server"
    class="DetailsBrowseTable">
    <asp:TableRow>
      <asp:TableCell HorizontalAlign="right" VerticalAlign="middle">
              <div class="backgroundShade">
                <a href="#" onclick="javascript:window.close();" class="gray_button float_left noBefore"><img src="images/x.svg" alt="Help" /></a>
              </div>
      </asp:TableCell>
    </asp:TableRow>
  </asp:Table>
  <div class="valueSpec viewValueExport Simplistic aircraftSpec">
    <asp:Panel ID="contentClass" runat="server" Width="100%" HorizontalAlign="Center"
      CssClass="gray_background" style="margin-top:-15px;padding-top:15px;">
      <div class="row">
        <div class="seven columns remove_margin main">
          <asp:Label ID="information_label" runat="server" Text=""></asp:Label>
          <asp:Label ID="company_address" runat="server" CssClass="display_none"></asp:Label>
          <asp:Label ID="company_name" runat="server" CssClass="display_none"></asp:Label>
          <asp:Label ID="about_label" runat="server" Text=""></asp:Label>
        </div>
        <div class="five columns main">
          <div class="Box">
            <div class="row remove_margin">
              <div class="subHeader padding_left">
                <asp:Label runat="server" ID="contactNameText"></asp:Label>
              </div>
              <br />
              <div class="columns eight remove_margin">
                <asp:Label ID="contact_information_label" runat="server"></asp:Label>

              </div>
            </div>
          </div>
          <div class="Box">
            <div class="subHeader">NEED HELP WITH EVENT ALERTS?</div>
            <ul class="remove_padding">
              <li><a href="#" onclick="javascript:load('/help/documents/983.pdf','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');">Learn more about scheduling event alerts.</a></li>
              <li><a href="#" onclick="javascript:load('/Preferences.aspx?activetab=5','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');">Setup text-message alerts.</a></li>
            </ul>
          </div>
        </div>
      </div>
      <div class="row">
        <asp:Button runat="server" ID="refreshList" Text="Refresh List" CssClass="display_none" />
        <div class="twelve columns main" style="margin-left: 1% !important; width: 98%;">
          <div class="Box">
            <div class="subHeader emphasisColor">SCHEDULED EVENT ALERTS/JOBS<span><asp:CheckBox runat="server" CssClass="float_right" ID="show_unscheduled" Text="Show Unscheduled Events" AutoPostBack="true" /></span></div>
            <asp:Label runat="server" ID="attentionWarning" CssClass="red_text" Visible="false" Font-Bold="true"></asp:Label>
            <asp:Label runat="server" ID="attentionSchedule" CssClass="red_text" Visible="false" Font-Bold="true"><p>You currently have no scheduled alerts.  To learn more about how to schedule event alerts click <a href="#" onclick="javascript:load('/help/documents/983.pdf','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');">here</a>.</p></asp:Label>
            <asp:DataGrid runat="server" ID="jobsTableGrid" AutoGenerateColumns="false" CssClass="formatTable blue small" Width="100%" BorderWidth="0" GridLines="None" OnDeleteCommand="MyDataGrid_Delete">
              <Columns>
                <asp:TemplateColumn HeaderText="<b>NAME</b>">
                  <ItemTemplate><%#DisplayNameLink(DataBinder.Eval(Container.DataItem, "WATCHNAME"), DataBinder.Eval(Container.DataItem, "ID"), DataBinder.Eval(Container.DataItem, "SOURCE"), DataBinder.Eval(Container.DataItem, "DATA"))%></ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<b>SCHEDULE</b>">
                  <ItemTemplate>
                    <%#FigureOutTimeDisplay(DataBinder.Eval(Container.DataItem, "DAYS"), DataBinder.Eval(Container.DataItem, "HOURS"), DataBinder.Eval(Container.DataItem, "MINUTES"), DataBinder.Eval(Container.DataItem, "ID"), DataBinder.Eval(Container.DataItem, "SOURCE"))%><br />
                  </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<b>LAST RUN</b>">
                  <ItemTemplate><%#DataBinder.Eval(Container.DataItem, "LASTRUN")%></ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<b>NEXT RUN</b>">
                  <ItemTemplate><%#crmWebClient.clsGeneral.clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "NEXTRUN"), True)%></ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<b>SEND TO</b>">
                  <ItemTemplate><%#DataBinder.Eval(Container.DataItem, "SENDTO")%></ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="<b>ON/OFF</b>" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="45px">
                  <ItemTemplate>
                    <%# DisplayCheckbox(DataBinder.Eval(Container.DataItem, "SOURCE"), DataBinder.Eval(Container.DataItem, "ID"), DataBinder.Eval(Container.DataItem, "cfolder_jetnet_run_flag")) %>
                    <asp:LinkButton CommandName="Delete" Visible='<%#IIf(DataBinder.Eval(Container.DataItem, "SOURCE") = "PROJECT", "true", "false") %>' ToolTip="Delete this legacy/project alert permanently." runat="server" OnClientClick="return confirm('This is a legacy system event alert.  Turning this alert off will remove it from the system permanently. Click Yes to continue or Cancel.');"><i class="fa fa-trash-o"></i></asp:LinkButton>
                    <asp:TextBox runat="server" ID="id_delete" Text='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Visible="true" CssClass="display_none" />
                  </ItemTemplate>
                </asp:TemplateColumn>
              </Columns>
            </asp:DataGrid>
          </div>
        </div>
      </div>
      <div id="DivLoadingMessage">
      </div>

    </asp:Panel>
    <br clear="all" />
  </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">
  <script type="text/javascript">

    var startWindow;

    function ShowLoadingMessage(DivTag, Title, Message) {
      $("#" + DivTag).html(Message);
      $("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 75, resizable: false });
    }

    function CloseLoadingMessage(DivTag) {
      $("#" + DivTag).dialog("close");
    }
  </script>
</asp:Content>
