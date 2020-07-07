// ********************************************************************************
// Copyright 2004-11. JETNET,LLC. All rights reserved.
//
// $$Archive: /commonWebProject/common/refreshCCSTDropdowns.js $
// $$Author: Mike $
// $$Date: 9/19/19 4:02p $
// $$Modtime: 9/19/19 4:00p $
// $$Revision: 5 $
// $$Workfile: refreshCCSTDropdowns.js $
//
// ********************************************************************************

function refreshCompanyFilterJS(fromEvent, updateWhat, productCodeCount) {

  var nSelectedFilterCount = 0;
  var nfilterloop = 0;            
  var filterArray = null;

  if ((updateWhat == "filter") && ((document.getElementById("chkHelicopterFilterID") != null) ||
                                   (document.getElementById("chkBusinessFilterID") != null) ||
                                   (document.getElementById("chkCommercialFilterID") != null) ||
                                   (document.getElementById("chkRegionalFilterID") != null))) {

    document.getElementById("hasModelFilterID").value = "true";
    b_isFilteredJS = true;
    nSelectedFilterCount = 0;

    if (document.getElementById("chkHelicopterFilterID") != null) {
      if (document.getElementById("chkHelicopterFilterID").checked == true) {
        nSelectedFilterCount = nSelectedFilterCount + 1;
      }
    }

    if (document.getElementById("chkBusinessFilterID") != null) {
      if (document.getElementById("chkBusinessFilterID").checked == true) {
        nSelectedFilterCount = nSelectedFilterCount + 1;
      }
    }


    if (document.getElementById("chkCommercialFilterID") != null) {
      if (document.getElementById("chkCommercialFilterID").checked == true) {
        nSelectedFilterCount = nSelectedFilterCount + 1;
      }
    }

    if (document.getElementById("chkRegionalFilterID") != null) {
      if (document.getElementById("chkRegionalFilterID").checked == true) {
        nSelectedFilterCount = nSelectedFilterCount + 1;
      }
    }
    
    if (nSelectedFilterCount == 0) {
      filterArray = s_rememberLastFilterJS.split(",");

      for (nfilterloop = 0; nfilterloop < filterArray.length; nfilterloop++) {

        switch (filterArray[nfilterloop]) {
          case "H":
            {
              if (document.getElementById("chkHelicopterFilterID") != null) {
                document.getElementById("chkHelicopterFilterID").checked = true;
                nSelectedFilterCount = nSelectedFilterCount + 1;
              }
              break;
            }
          case "B":
            {
              if (document.getElementById("chkBusinessFilterID") != null) {
                document.getElementById("chkBusinessFilterID").checked = true;
                nSelectedFilterCount = nSelectedFilterCount + 1;
              }
              break;
            }
          case "C":
            {
              if (document.getElementById("chkCommercialFilterID") != null) {
                document.getElementById("chkCommercialFilterID").checked = true;
                nSelectedFilterCount = nSelectedFilterCount + 1;
              }
              break;
            }
          case "R":
            {
              if (document.getElementById("chkRegionalFilterID") != null) {
                document.getElementById("chkRegionalFilterID").checked = true;
                nSelectedFilterCount = nSelectedFilterCount + 1;
              }
              break;
            }
        }
      } // nfilterloop
    } // nSelectedFilterCount = 0

    // if our filter count is less than the count of product codes then filter
    if (nSelectedFilterCount < productCodeCount) {

      s_rememberLastFilterJS = "";

      if (document.getElementById("chkHelicopterFilterID") != null) {
        if (document.getElementById("chkHelicopterFilterID").checked == true) {
          document.getElementById("hasHelicopterFilterID").value = "true";
          if (s_rememberLastFilterJS == "") {
            s_rememberLastFilterJS = "H";
          }
          else {
            s_rememberLastFilterJS = s_rememberLastFilterJS + ",H";
          }
        }
        else {
          document.getElementById("hasHelicopterFilterID").value = "false";
        }
      }

      if (document.getElementById("chkBusinessFilterID") != null) {
        if (document.getElementById("chkBusinessFilterID").checked == true) {
          document.getElementById("hasBusinessFilterID").value = "true";
          if (s_rememberLastFilterJS == "") {
            s_rememberLastFilterJS = "B";
          }
          else {
            s_rememberLastFilterJS = s_rememberLastFilterJS + ",B";
          }
        }
        else {
          document.getElementById("hasBusinessFilterID").value = "false";
        }
      }

      if (document.getElementById("chkCommercialFilterID") != null) {
        if (document.getElementById("chkCommercialFilterID").checked == true) {
          document.getElementById("hasCommercialFilterID").value = "true";
          if (s_rememberLastFilterJS == "") {
            s_rememberLastFilterJS = "C";
          }
          else {
            s_rememberLastFilterJS = s_rememberLastFilterJS + ",C";
          }
        }
        else {
          document.getElementById("hasCommercialFilterID").value = "false";
        }
      }

      if (document.getElementById("chkRegionalFilterID") != null) {
        if (document.getElementById("chkRegionalFilterID").checked == true) {
          document.getElementById("hasRegionalFilterID").value = "true";
          if (s_rememberLastFilterJS == "") {
            s_rememberLastFilterJS = "R";
          }
          else {
            s_rememberLastFilterJS = s_rememberLastFilterJS + ",R";
          }
        }
        else {
          document.getElementById("hasRegionalFilterID").value = "false";
        }
      }
      
      document.getElementById("lastModelFilterID").value = s_rememberLastFilterJS;

    }
    else {

      document.getElementById("hasModelFilterID").value = "false";
      document.getElementById("hasHelicopterFilterID").value = "false";
      document.getElementById("hasBusinessFilterID").value = "false";
      document.getElementById("hasCommercialFilterID").value = "false";
      document.getElementById("hasRegionalFilterID").value = "false";

      b_isFilteredJS = false;
      s_rememberLastFilterJS = "";
      document.getElementById("lastModelFilterID").value = s_rememberLastFilterJS;

    } // nSelectedFilterCount = 0 

  }

  else {

    if ((fromEvent == "onClick") && (b_isFilteredJS) && (s_rememberLastFilterJS == "")) {

      document.getElementById("hasModelFilterID").value = "false";
      document.getElementById("hasHelicopterFilterID").value = "false";
      document.getElementById("hasBusinessFilterID").value = "false";
      document.getElementById("hasCommercialFilterID").value = "false";
      document.getElementById("hasRegionalFilterID").value = "false";

      b_isFilteredJS = false;
      s_rememberLastFilterJS = "";
      document.getElementById("lastModelFilterID").value = s_rememberLastFilterJS;

    } // ((fromEvent == "onClick") && (b_isFilteredJS) && (s_rememberLastFilterJS == "")) 

    if ((updateWhat == "") && (b_isFilteredJS)) {

      // find out which filter is set (we are comming in from a new page)
      nSelectedFilterCount = 0;

      if (document.getElementById("chkHelicopterFilterID") != null) {
        if (document.getElementById("chkHelicopterFilterID").checked == true) {
          nSelectedFilterCount = nSelectedFilterCount + 1;
        }
      }

      if (document.getElementById("chkBusinessFilterID") != null) {
        if (document.getElementById("chkBusinessFilterID").checked == true) {
          nSelectedFilterCount = nSelectedFilterCount + 1;
        }
      }

      if (document.getElementById("chkCommercialFilterID") != null) {
        if (document.getElementById("chkCommercialFilterID").checked == true) {
          nSelectedFilterCount = nSelectedFilterCount + 1;
        }
      }

      if (document.getElementById("chkRegionalFilterID") != null) {
        if (document.getElementById("chkRegionalFilterID").checked == true) {
          nSelectedFilterCount = nSelectedFilterCount + 1;
        }
      }
      
      if (nSelectedFilterCount == 0) {
        filterArray = s_rememberLastFilterJS.split(",");

        for (nfilterloop = 0; nfilterloop < filterArray.length; nfilterloop++) {

          switch (filterArray[nfilterloop]) {
            case "H":
              {
                if (document.getElementById("chkHelicopterFilterID") != null) {
                  document.getElementById("chkHelicopterFilterID").checked = true;
                  nSelectedFilterCount = nSelectedFilterCount + 1;
                }
                break;
              }
            case "B":
              {
                if (document.getElementById("chkBusinessFilterID") != null) {
                  document.getElementById("chkBusinessFilterID").checked = true;
                  nSelectedFilterCount = nSelectedFilterCount + 1;
                }
                break;
              }
            case "C":
              {
                if (document.getElementById("chkCommercialFilterID") != null) {
                  document.getElementById("chkCommercialFilterID").checked = true;
                  nSelectedFilterCount = nSelectedFilterCount + 1;
                }
                break;
              }
            case "R":
              {
                if (document.getElementById("chkRegionalFilterID") != null) {
                  document.getElementById("chkRegionalFilterID").checked = true;
                  nSelectedFilterCount = nSelectedFilterCount + 1;
                }
                break;
              }
          }
        } // nfilterloop
      } // nSelectedFilterCount = 0

      // if our filter count is less than the count of product codes then filter
      if (nSelectedFilterCount < productCodeCount) {

        s_rememberLastFilterJS = "";

        if (document.getElementById("chkHelicopterFilterID") != null) {
          if (document.getElementById("chkHelicopterFilterID").checked == true) {
            document.getElementById("hasHelicopterFilterID").value = "true";
            if (s_rememberLastFilterJS == "") {
              s_rememberLastFilterJS = "H";
            }
            else {
              s_rememberLastFilterJS = s_rememberLastFilterJS + ",H";
            }
          }
          else {
            document.getElementById("hasHelicopterFilterID").value = "false";
          }
        }

        if (document.getElementById("chkBusinessFilterID") != null) {
          if (document.getElementById("chkBusinessFilterID").checked == true) {
            document.getElementById("hasBusinessFilterID").value = "true";
            if (s_rememberLastFilterJS == "") {
              s_rememberLastFilterJS = "B";
            }
            else {
              s_rememberLastFilterJS = s_rememberLastFilterJS + ",B";
            }
          }
          else {
            document.getElementById("hasBusinessFilterID").value = "false";
          }
        }

        if (document.getElementById("chkCommercialFilterID") != null) {
          if (document.getElementById("chkCommercialFilterID").checked == true) {
            document.getElementById("hasCommercialFilterID").value = "true";
            if (s_rememberLastFilterJS == "") {
              s_rememberLastFilterJS = "C";
            }
            else {
              s_rememberLastFilterJS = s_rememberLastFilterJS + ",C";
            }
          }
          else {
            document.getElementById("hasCommercialFilterID").value = "false";
          }
        }

        if (document.getElementById("chkRegionalFilterID") != null) {
          if (document.getElementById("chkRegionalFilterID").checked == true) {
            document.getElementById("hasRegionalFilterID").value = "true";
            if (s_rememberLastFilterJS == "") {
              s_rememberLastFilterJS = "R";
            }
            else {
              s_rememberLastFilterJS = s_rememberLastFilterJS + ",R";
            }
          }
          else {
            document.getElementById("hasRegionalFilterID").value = "false";
          }
        }
        
        document.getElementById("lastModelFilterID").value = s_rememberLastFilterJS;
      
      }
      else {

        document.getElementById("hasModelFilterID").value = "false";
        document.getElementById("hasHelicopterFilterID").value = "false";
        document.getElementById("hasBusinessFilterID").value = "false";
        document.getElementById("hasCommercialFilterID").value = "false";
        document.getElementById("hasRegionalFilterID").value = "false";

        b_isFilteredJS = false;
        s_rememberLastFilterJS = "";
        document.getElementById("lastModelFilterID").value = s_rememberLastFilterJS;

      } // nSelectedFilterCount > 0
    } // updateWhat = "" && b_isFilteredJS
  }
  return true; 
}

function refreshRegionsJS(fromEvent, updateWhat, isBase, isView, sessCompRegion, sessBaseRegion, sessDocRegion, sessCompCountry, sessBaseCountry, sessDocCountry, sessCompState, sessBaseState, sessDocState, sessCompTz, sessDocTz) {

  var regionCbo = null;
  var countryCbo = null;
  var stateCbo = null;
  var tzCbo = null;
  var tzDiv = null;
  var hvHasTimeZones = null;
  var WhichOne = '';

  //alert("(rr) isbase[ " + isBase + " ] isView[ " + isView + " ]");
 
  if (isBase && (!isView)) {
    if (document.getElementById("radBaseContinentRegionID").checked == true) { 
      WhichOne = "continent";
    }
    else {	
      WhichOne = "region";
    }

    //alert("(rr)WhichOne[ " + WhichOne + " ]");

    regionCbo = document.getElementById("cboBaseRegionID");
    countryCbo = document.getElementById("cboBaseCountryID");
    stateCbo = document.getElementById("cboBaseStateID");
  
  }
  else {
    if ((!isBase) && (!isView)) {

      //alert("(rr)radContinentRegionID[ " + document.getElementById("radContinentRegionID").checked + " ]");
      //alert("(rr)radContinentRegionID1[ " + document.getElementById("radContinentRegionID1").checked + " ]");

      if (document.getElementById("radContinentRegionID").checked == true) { 
        WhichOne = "continent"; 
      }
      else {	
        WhichOne = "region"; 
      }

      //alert("(rr)WhichOne[ " + WhichOne + " ]");
      
      regionCbo = document.getElementById("cboCompanyRegionID");
      countryCbo = document.getElementById("cboCompanyCountryID");
      stateCbo = document.getElementById("cboCompanyStateID");
      tzCbo = document.getElementById("cboCompanyTimeZoneID");
      tzDiv = document.getElementById("cboCompanyTimeZoneLabelID");
      hvHasTimeZones = document.getElementById("hasCompanyTimeZonesID");    
    }
    else {
      if (document.getElementById("radViewContinentRegionID").checked == true) { 
        WhichOne = "continent";
      }
      else {	
        WhichOne = "region";
      }

      //alert("(rr)WhichOne[ " + WhichOne + " ]");
      	
      regionCbo = document.getElementById("cboViewRegionID");
      countryCbo = document.getElementById("cboViewCountryID");
      stateCbo = document.getElementById("cboViewStateID");
      tzCbo = document.getElementById("cboViewTimeZoneID");
      tzDiv = document.getElementById("cboViewTimeZoneLabelID");
      hvHasTimeZones = document.getElementById("hasViewTimeZonesID");  
    }
  }
 
  switch (fromEvent) {
    case "onChange":
      {

        switch (updateWhat) {

          case "timeZone":
            {
              if (!isBase && hvHasTimeZones.value == "true") {
                tzDiv.style.visibility = "visible";
                tzCbo.style.visibility = "visible";
                tzCbo = fillTimezone(tzCbo, WhichOne, tzCbo, "", isBase, isView, sessCompTz, sessDocTz);
              }
              else {
                if (!isBase) {
                  tzDiv.style.visibility = "hidden";
                  tzCbo.style.visibility = "hidden";
                  tzCbo.options.length = 0;
                  tzCbo.options[0] = new Option("");
                  tzCbo.options[0].innerHTML = "All";
                  tzCbo.options[0].value = "All";
                  tzCbo.options[0].selected = true;
                  tzCbo.options[0].selectedindex = 0;
                } //Not isBase
              }

              break;
            }
          case "state":
            {
              if (!isBase && hvHasTimeZones.value == "true") {
                tzDiv.style.visibility = "visible";
                tzCbo.style.visibility = "visible";
                tzCbo = fillTimezone(tzCbo, WhichOne, tzCbo, "", isBase, isView, sessCompTz, sessDocTz);
              }
              else {
                if (!isBase) {
                  tzDiv.style.visibility = "hidden";
                  tzCbo.style.visibility = "hidden";
                  tzCbo.options.length = 0;
                  tzCbo.options[0] = new Option("");
                  tzCbo.options[0].innerHTML = "All";
                  tzCbo.options[0].value = "All";
                  tzCbo.options[0].selected = true;
                  tzCbo.options[0].selectedindex = 0;
                } //Not isBase
              }
              break;
            }
          case "country":
            {
              stateCbo = fillState(stateCbo, regionCbo, countryCbo, WhichOne, stateCbo, isBase, isView, sessCompState, sessBaseState, sessDocState);

              if (!isBase && hvHasTimeZones.value == "true") {
                tzDiv.style.visibility = "visible";
                tzCbo.style.visibility = "visible";
                tzCbo = fillTimezone(tzCbo, WhichOne, tzCbo, "", isBase, isView, sessCompTz, sessDocTz);
              }
              else {
                if (!isBase) {
                  tzDiv.style.visibility = "hidden";
                  tzCbo.style.visibility = "hidden";
                  tzCbo.options.length = 0;
                  tzCbo.options[0] = new Option("");
                  tzCbo.options[0].innerHTML = "All";
                  tzCbo.options[0].value = "All";
                  tzCbo.options[0].selected = true;
                  tzCbo.options[0].selectedindex = 0;
                } //Not isBase
              }
              break;
            }
          case "region":
            {
              countryCbo = fillCountry(countryCbo, regionCbo, WhichOne, countryCbo, isBase, isView, sessCompCountry, sessBaseCountry, sessDocCountry);
              stateCbo = fillState(stateCbo, regionCbo, countryCbo, WhichOne, stateCbo, isBase, isView, sessCompState, sessBaseState, sessDocState);

              if (!isBase && hvHasTimeZones.value == "true") {
                tzDiv.style.visibility = "visible";
                tzCbo.style.visibility = "visible";
                tzCbo = fillTimezone(tzCbo, WhichOne, tzCbo, "", isBase, isView, sessCompTz, sessDocTz);
              }
              else {
                if (!isBase) {
                  tzDiv.style.visibility = "hidden";
                  tzCbo.style.visibility = "hidden";
                  tzCbo.options.length = 0;
                  tzCbo.options[0] = new Option("");
                  tzCbo.options[0].innerHTML = "All";
                  tzCbo.options[0].value = "All";
                  tzCbo.options[0].selected = true;
                  tzCbo.options[0].selectedindex = 0;
                } //Not isBase
              }
              break;
            }
        }
        break;
      }

    default:
      {

        if (updateWhat == "") {

          regionCbo = fillRegionContinent(regionCbo, WhichOne, regionCbo, isBase, isView, sessCompRegion, sessBaseRegion, sessDocRegion);
          countryCbo = fillCountry(countryCbo, regionCbo, WhichOne, countryCbo, isBase, isView, sessCompCountry, sessBaseCountry, sessDocCountry);
          stateCbo = fillState(stateCbo, regionCbo, countryCbo, WhichOne, stateCbo, isBase, isView, sessCompState, sessBaseState, sessDocState);

          if (!isBase && hvHasTimeZones.value == "true") {
            tzDiv.style.visibility = "visible";
            tzCbo.style.visibility = "visible";
            tzCbo = fillTimezone(tzCbo, WhichOne, tzCbo, "", isBase, isView, sessCompTz, sessDocTz);
          }
          else {
            if (!isBase) {
              tzDiv.style.visibility = "hidden";
              tzCbo.style.visibility = "hidden";
              tzCbo.options.length = 0;
              tzCbo.options[0] = new Option(""); 
              tzCbo.options[0].innerHTML = "All";
              tzCbo.options[0].value = "All";
              tzCbo.options[0].selected = true;
              tzCbo.options[0].selectedindex = 0;
            } //Not isBase
          }

        } // 'updateWhat = ""
        break;
      }
  }
  
}

function checkRadioButtons(isBase, isView, sessCompRegion, sessBaseRegion, sessDocRegion, sessCompCountry, sessBaseCountry, sessDocCountry, sessCompState, sessBaseState, sessDocState, sessCompTz, sessDocTz) {

  //alert("crb woBase[ " + whichOneBase + " ] woCompany[ " + whichOneCompany + "] woView[ " + whichOneView + " ]");

  var dbugString = 'crb[input] companyRegion[ ' + sessCompRegion + ' ] baseRegion[ ' + sessBaseRegion + ' ] viewRegion[ ' + sessDocRegion + ' ]\n\n'
    + 'companyCountry[ ' + sessCompCountry + ' ] baseCountry[ ' + sessBaseCountry + ' ] viewCountry[ ' + sessDocCountry + ' ]\n\n'
    + 'companyState[ ' + sessCompState + ' ] baseState[ ' + sessBaseState + ' ] viewState[ ' + sessDocState + ' ]\n\n'
    + 'companyTimeZone[ ' + sessCompTz + ' ] viewTimeZone[ ' + sessDocTz + ' ]';
  //alert(dbugString);

  if (isBase && !isView) {
    if (whichOneBase.toLowerCase() == "continent") {
      document.getElementById("radBaseContinentRegionID").checked = true;
      document.getElementById("radBaseContinentRegionID1").checked = false;
    }
    else {
      document.getElementById("radBaseContinentRegionID").checked = false;
      document.getElementById("radBaseContinentRegionID1").checked = true;
    }
    refreshRegionsJS("onClick", "", isBase, isView, sessCompRegion, sessBaseRegion, sessDocRegion, sessCompCountry, sessBaseCountry, sessDocCountry, sessCompState, sessBaseState, sessDocState, sessCompTz, sessDocTz);
  }
  else {
    if (!isBase && !isView) {
      if (whichOneCompany.toLowerCase() == "continent") {
        document.getElementById("radContinentRegionID").checked = true;
        document.getElementById("radContinentRegionID1").checked = false;
      }
      else {
        document.getElementById("radContinentRegionID").checked = false;
        document.getElementById("radContinentRegionID1").checked = true;
      }
      refreshRegionsJS("onClick", "", isBase, isView, sessCompRegion, sessBaseRegion, sessDocRegion, sessCompCountry, sessBaseCountry, sessDocCountry, sessCompState, sessBaseState, sessDocState, sessCompTz, sessDocTz);
    }
    else {
      if (whichOneView.toLowerCase() == "continent") {
        document.getElementById("radViewContinentRegionID").checked = true;
        document.getElementById("radViewContinentRegionID1").checked = false;
      }
      else {
        document.getElementById("radViewContinentRegionID").checked = false;
        document.getElementById("radViewContinentRegionID1").checked = true;
      }
      refreshRegionsJS("onClick", "", isBase, isView, sessCompRegion, sessBaseRegion, sessDocRegion, sessCompCountry, sessBaseCountry, sessDocCountry, sessCompState, sessBaseState, sessDocState, sessCompTz, sessDocTz);
    }
  }

}
