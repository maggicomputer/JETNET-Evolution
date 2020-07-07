// ********************************************************************************
// Copyright 2004-11. JETNET,LLC. All rights reserved.
//
// $$Archive: /commonWebProject/common/RegionDropdowns.js $
// $$Author: Mike $
// $$Date: 6/19/19 8:45a $
// $$Modtime: 6/18/19 6:12p $
// $$Revision: 2 $
// $$Workfile: RegionDropdowns.js $
//
// ********************************************************************************

function selectTimeZoneForOption(inContentRegionArray, inStateName) {

  var stateArray = null;
  var sTimeZoneList = "";

  stateArray = inStateName.split(",");

  if ((inContentRegionArray != null) && (inContentRegionArray != "")) {
    for (var x = 0; x < inContentRegionArray.length; x++) {

      if ((stateArray.length == 1) && (stateArray[0].toUpperCase() != "ALL")) {
        if (inContentRegionArray[x][LOCRGN_STATE_NAME] == stateArray[0]) {
          sTimeZoneList = inContentRegionArray[x][LOCRGN_STATE_TZ];
          break;
        }
      }
      else {
        for (var y = 0; y < stateArray.length; y++) {
          if (inContentRegionArray[x][LOCRGN_STATE_NAME] == stateArray[y]) {
            if (sTimeZoneList == "") {
              sTimeZoneList = inContentRegionArray[x][LOCRGN_STATE_TZ];
            }
            else {
              sTimeZoneList = sTimeZoneList + "," + inContentRegionArray[x][LOCRGN_STATE_TZ];
            }
          }
        } // yLoop
      }
    } // xLoop
  }

  return sTimeZoneList;
}

function fillRegionContinent(inCboContinentRegion, inWhich, inSelected, isBase, isView, sessCompRegion, sessBaseRegion, sessDocRegion) {

  var bSelectedItem = false;
  var bfoundSelection = false;

  var nCurrentOption = 0;

  var displayCboJS = null;
  var optionArray = null;
  var contentRegionArray = null;

  var sSelectionStr = "";
  var sRememberRegion = "";

  if (inWhich.toLowerCase() == "continent") {
    if ((localAryContinent != null) && (localAryContinent.length > 0)) {
      contentRegionArray = localAryContinent;
    }
  }
  else {
    if ((localAryRegion != null) && (localAryRegion.length > 0)) {
      contentRegionArray = localAryRegion;
    }
  }

  // get currently selected items
  if ((typeof (inSelected.name) != "undefined") && (inSelected != null)) {

    for (var nloop = 0; nloop < inSelected.length; nloop++) {
      if (inSelected.options[nloop].selected == true) {
        if (nloop == 0) {
          sSelectionStr = "All";
        }
        else {
          bSelectedItem = true;
          if (sSelectionStr == "") {
            sSelectionStr = inSelected.options[nloop].value;
          }
          else {
            sSelectionStr = sSelectionStr + "," + inSelected.options[nloop].value;
          }
        } // (nloop == 0)
      } // (optionList.options[nloop].selected == true)
    } // (nloop = 0; nloop < optionList.length; nloop++)

    if (sSelectionStr != "") {

      if (bSelectedItem) {
        optionArray = sSelectionStr.split(",");
      }
      else {
        if (inSelected.length > 1) {
          optionArray = ("All").split(",");
        }
        else {
          if (isBase && !isView) {
            if ((sessBaseRegion != null) && (sessBaseRegion != "")) {
              var remove = /, /gi;
              sSelectionStr = sessBaseRegion.replace(remove, ",");
              optionArray = sSelectionStr.split(",");
            }
          }
          else {
            if (!isBase && !isView) {
              if ((sessCompRegion != null) && (sessCompRegion != "")) {
                var remove = /, /gi;
                sSelectionStr = sessCompRegion.replace(remove, ",");
                optionArray = sSelectionStr.split(",");
              }
            }
            else {
              if ((sessDocRegion != null) && (sessDocRegion != "")) {
                var remove = /, /gi;
                sSelectionStr = sessDocRegion.replace(remove, ",");
                optionArray = sSelectionStr.split(",");
              }
            }
          }
        } // (optionList.length > 1)
      } // (bSelectedItem)
    } // (sSelectionStr != "") 

  } // ((typeof(inSelected.name) != "undefined") && (inSelected != ""))

  displayCboJS = inCboContinentRegion;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  for (var x = 0; x < contentRegionArray.length; x++) {
    if ((contentRegionArray[x][LOCRGN_CONTINENT] != "") && (contentRegionArray[x][LOCRGN_CONTINENT] != sRememberRegion)) {

      if (nCurrentOption == 0) {

        displayCboJS.options[0].innerHTML = "All";
        displayCboJS.options[0].value = "All";

        if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() == "ALL")) {
          displayCboJS.options[0].selected = true;
          displayCboJS.options[0].selectedindex = 0;
          bfoundSelection = true;
        }
        else {
          if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
            if (sSelectionStr.substring(0, 3) == displayCboJS.options[nCurrentOption].value) {
              bfoundSelection = true;
              displayCboJS.options[0].selected = true;
              displayCboJS.options[0].selectedindex = 0;
            }
            else {
              displayCboJS.options[0].selected = false;
            }
          }
        }

        // pick up the first option
        nCurrentOption = nCurrentOption + 1;
        displayCboJS.options[nCurrentOption] = new Option(contentRegionArray[x][LOCRGN_CONTINENT]);
        displayCboJS.options[nCurrentOption].value = contentRegionArray[x][LOCRGN_CONTINENT];
        displayCboJS.options[nCurrentOption].innerHTML = contentRegionArray[x][LOCRGN_CONTINENT];

        if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
          if (optionArray.length > 0) {
            if (inClientArrayJS(optionArray, contentRegionArray[x][LOCRGN_CONTINENT])) {
              displayCboJS.options[nCurrentOption].selected = true;
              displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
              bfoundSelection = true;
            }
          }
        }
      }
      else {

        displayCboJS.options[nCurrentOption] = new Option(contentRegionArray[x][LOCAIR_INDEX]);
        displayCboJS.options[nCurrentOption].value = contentRegionArray[x][LOCAIR_INDEX];
        displayCboJS.options[nCurrentOption].innerHTML = contentRegionArray[x][LOCRGN_CONTINENT];

        if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
          if (optionArray.length > 0) {
            if (inClientArrayJS(optionArray, contentRegionArray[x][LOCRGN_CONTINENT])) {
              displayCboJS.options[nCurrentOption].selected = true;
              displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
              bfoundSelection = true;
            }
          }
        }
      } // nCurrentOption == 0

      nCurrentOption = nCurrentOption + 1;
      sRememberRegion = contentRegionArray[x][LOCRGN_CONTINENT];

    } // (contentRegionArray[x][LOCRGN_CONTINENT] != sRememberRegion)
  } // (var x = 0; x < contentRegionArray.length; x++)


  if ((!bfoundSelection) && (displayCboJS.options.length > 1)) {
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
  }

  contentRegionArray = null;
  optionArray = null;

  return displayCboJS;
}

function fillCountry(inCountryCbo, inContinentOrRegion, inWhich, inSelected, isBase, isView, sessCompCountry, sessBaseCountry, sessDocCountry) {
  var bSelectedItem = false;
  var bfoundSelection = false;

  var nCurrentOption = 0;
  var nContinent = 0;

  var displayCboJS = null;
  var optionArray = null;
  var contentRegionArray = null;

  var bAllRegion = false;

  var sCboContentRegion = "";

  var sSelectionStr = "";
  var sRememberCountry = "";
  var displayList = "";
  var sTempItem = "";

  var bDisplayInactive = false;
  var displayInactive = "";

  // when the bShowInactiveCountries flag is set it will show inactive countries

  if (isBase && !isView) {
    if (bShowInactiveCountriesBase) {
      bDisplayInactive = true;
    }

  }
  else {
    if (!isBase && !isView) {
      if (bShowInactiveCountriesCompany) {
        bDisplayInactive = true;
      }

    }
    else {
      if (bShowInactiveCountriesView) {
        bDisplayInactive = true;
      }

    }
  }
      
  if (inWhich.toLowerCase() == "continent") {
    if ((localAryContinent != null) && (localAryContinent.length > 0)) {
      contentRegionArray = localAryContinent;
    }
  }
  else {
    if ((localAryRegion != null) && (localAryRegion.length > 0)) {
      contentRegionArray = localAryRegion;
    }
  }

  // get the list of selected Content/Regions
  if ((typeof (inContinentOrRegion.name) != "undefined") && (inContinentOrRegion != null)) {
    for (var nloop = 0; nloop < inContinentOrRegion.length; nloop++) {
      if ((inContinentOrRegion.options[nloop].selected == true) || (bAllRegion == true)) {
        if (nloop == 0) {
          bAllRegion = true;
        }
        else {
          if (sCboContentRegion == "") {
            sCboContentRegion = inContinentOrRegion.options[nloop].value;
          }
          else {
            sCboContentRegion = sCboContentRegion + "," + inContinentOrRegion.options[nloop].value;
          }
        } // (nloop == 0)
      } // ((inContinentOrRegion.options[nloop].selected == true) || (bAllType == true))
    } // (nloop = 0; nloop < inContinentOrRegion.length; nloop++)
  }
  else {
    bAllRegion = true;
  }

  // get currently selected items
  if ((typeof (inSelected.name) != "undefined") && (inSelected != null)) {
    for (var iloop = 0; iloop < inSelected.length; iloop++) {
      if (inSelected.options[iloop].selected == true) {
        if (nloop == 0) {
          sSelectionStr = "All";
        }
        else {
          bSelectedItem = true;
          if (sSelectionStr == "") {
            sSelectionStr = inSelected.options[iloop].value;
          }
          else {
            sSelectionStr = sSelectionStr + "," + inSelected.options[iloop].value;
          }
        } // (nloop == 0)
      } // (optionList.options[nloop].selected == true)
    } // (nloop = 0; nloop < optionList.length; nloop++)

    if (sSelectionStr != "") {
      if ((!bAllRegion) && bSelectedItem) {
        optionArray = sSelectionStr.split(",");
      }
      else {
        optionArray = ("All").split(",");
      } // (!bAllType && bSelectedItem)
    }
    else {
      if (isBase && !isView) {
        if ((sessBaseCountry != null) && (sessBaseCountry != "")) {
          var remove = /, /gi;
          sSelectionStr = sessBaseCountry.replace(remove, ",");
          optionArray = sSelectionStr.split(",");
        }
      }
      else {
        if (!isBase && !isView) {
          if ((sessCompCountry != null) && (sessCompCountry != "")) {
            var remove = /, /gi;
            sSelectionStr = sessCompCountry.replace(remove, ",");
            optionArray = sSelectionStr.split(",");
          }
        }
        else {
          if ((sessDocCountry != null) && (sessDocCountry != "")) {
            var remove = /, /gi;
            sSelectionStr = sessDocCountry.replace(remove, ",");
            optionArray = sSelectionStr.split(",");
          }
        }
      }
    } // (sSelectionStr != "") 
  } // ((typeof(inSelected.name) != "undefined") && (inSelected != ""))

  if ((sSelectionStr == "") && !bSelectedItem) {
    optionArray = ("All").split(",");
    sSelectionStr = "All";
  } // (sSelectionStr == "")

  displayCboJS = inCountryCbo;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  displayList = sCboContentRegion.split(",");

  for (var x = 0; x < displayList.length; x++) {

    sTempItem = displayList[x];

    for (var y = 0; y < contentRegionArray.length; y++) {
      if (contentRegionArray[y][LOCRGN_CONTINENT].toLowerCase() == sTempItem.toLowerCase()) {
        nContinent = y;
        break;
      }
    } // y

    for (var z = nContinent; z < contentRegionArray.length; z++) {

      if (contentRegionArray[z][LOCRGN_CONTINENT].toLowerCase() == sTempItem.toLowerCase()) {

        if (bDisplayInactive) {
          displayInactive = "";
        }
        else {
          displayInactive = "N";
        }

        if ((contentRegionArray[z][LOCRGN_COUNTRY] != "") &&
		       (contentRegionArray[z][LOCRGN_COUNTRY_ACTIVE] != displayInactive) &&
		       (contentRegionArray[z][LOCRGN_COUNTRY] != sRememberCountry)) {

          if (nCurrentOption == 0) {

            displayCboJS.options[0].innerHTML = "All";
            displayCboJS.options[0].value = "All";

            if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() == "ALL")) {
              displayCboJS.options[0].selected = true;
              displayCboJS.options[0].selectedindex = 0;
              bfoundSelection = true;
            }
            else {
              if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                if (sSelectionStr.substring(0, 3) == displayCboJS.options[nCurrentOption].value) {
                  bfoundSelection = true;
                  displayCboJS.options[0].selected = true;
                  displayCboJS.options[0].selectedindex = 0;
                }
                else {
                  displayCboJS.options[0].selected = false;
                }
              }
            }

            // pick up the first option
            nCurrentOption = nCurrentOption + 1;
            displayCboJS.options[nCurrentOption] = new Option(contentRegionArray[z][LOCRGN_COUNTRY]);
            displayCboJS.options[nCurrentOption].value = contentRegionArray[z][LOCRGN_COUNTRY];
            displayCboJS.options[nCurrentOption].innerHTML = contentRegionArray[z][LOCRGN_COUNTRY];

            if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
              if (optionArray.length > 0) {
                if (inClientArrayJS(optionArray, contentRegionArray[z][LOCRGN_COUNTRY])) {
                  displayCboJS.options[nCurrentOption].selected = true;
                  displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                  bfoundSelection = true;
                }
              }
            }
          }
          else {

            displayCboJS.options[nCurrentOption] = new Option(contentRegionArray[z][LOCRGN_COUNTRY]);
            displayCboJS.options[nCurrentOption].value = contentRegionArray[z][LOCRGN_COUNTRY];
            displayCboJS.options[nCurrentOption].innerHTML = contentRegionArray[z][LOCRGN_COUNTRY];

            if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
              if (optionArray.length > 0) {
                if (inClientArrayJS(optionArray, contentRegionArray[z][LOCRGN_COUNTRY])) {
                  displayCboJS.options[nCurrentOption].selected = true;
                  displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                  bfoundSelection = true;
                }
              }
            }
          } // nCurrentOption == 0

          nCurrentOption = nCurrentOption + 1;
          sRememberCountry = contentRegionArray[z][LOCRGN_COUNTRY];

        }
      } // (contentRegionArray[y][LOCRGN_CONTINENT].toLowerCase() == sTempItem.toLowerCase())  
    } // z
  } // x

  if (displayCboJS.options.length > 1) {
    displayCboJS = sortListBoxJS(displayCboJS);
  }

  if ((!bfoundSelection) && (displayCboJS.options.length > 1)) {
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
  }
  else {
    if ((displayCboJS.options.length > 1) && (bfoundSelection) && (sSelectionStr.toUpperCase() != "ALL")) {
      displayCboJS.options[0].selected = false;
      displayCboJS.options[0].selectedindex = 0;
    }
  }

  optionArray = null;
  contentRegionArray = null;

  return displayCboJS;
}

function fillState(inStateCbo, inContinentOrRegion, inCountryCbo, inWhich, inSelected, isBase, isView, sessCompState, sessBaseState, sessDocState) {
  var bSelectedItem = false;
  var bfoundSelection = false;

  var nCurrentOption = 0;
  var nContinent = 0;

  var displayCboJS = null;
  var optionArray = null;
  var contentRegionArray = null;

  var bAllRegion = false;
  var bAllCoutry = false;

  var sCboContentRegion = "";
  var sCboCountry = "";

  var sSelectionStr = "";
  var sRememberState = "";
  var displayList = "";
  var sTempItem = "";

  if (inWhich.toLowerCase() == "continent") {
    if ((localAryContinent != null) && (localAryContinent.length > 0)) {
      contentRegionArray = localAryContinent;
    }
  }
  else {
    if ((localAryRegion != null) && (localAryRegion.length > 0)) {
      contentRegionArray = localAryRegion;
    }
  }

  // get the list of selected Content/Regions
  if ((typeof (inContinentOrRegion.name) != "undefined") && (inContinentOrRegion != null)) {
    for (var nloop = 0; nloop < inContinentOrRegion.length; nloop++) {
      if ((inContinentOrRegion.options[nloop].selected == true) || (bAllRegion == true)) {
        if (nloop == 0) {
          bAllRegion = true;
        }
        else {
          if (sCboContentRegion == "") {
            sCboContentRegion = inContinentOrRegion.options[nloop].value;
          }
          else {
            sCboContentRegion = sCboContentRegion + "," + inContinentOrRegion.options[nloop].value;
          }
        } // (nloop == 0)
      } // ((inContinentOrRegion.options[nloop].selected == true) || (bAllType == true))
    } // (nloop = 0; nloop < inContinentOrRegion.length; nloop++)
  }
  else {
    bAllRegion = true;
  }

  // get the list of selected Countries
  if ((typeof (inCountryCbo.name) != "undefined") && (inCountryCbo != null)) {
    for (var nloop = 0; nloop < inCountryCbo.length; nloop++) {
      if ((inCountryCbo.options[nloop].selected == true) || (bAllCoutry == true)) {
        if (nloop == 0) {
          bAllCoutry = true;
        }
        else {
          if (sCboCountry == "") {
            sCboCountry = inCountryCbo.options[nloop].value;
          }
          else {
            sCboCountry = sCboCountry + "," + inCountryCbo.options[nloop].value;
          }
        } // (nloop == 0)
      } // ((inCountryCbo.options[nloop].selected == true) || (bAllCoutry == true))
    } // (nloop = 0; nloop < inCountryCbo.length; nloop++)
  }
  else {
    bAllCoutry = true;
  }

  // get currently selected items
  if ((typeof (inSelected.name) != "undefined") && (inSelected != null)) {
    for (var nloop = 0; nloop < inSelected.length; nloop++) {
      if (inSelected.options[nloop].selected == true) {
        if (nloop == 0) {
          sSelectionStr = "All";
        }
        else {
          bSelectedItem = true;
          if (sSelectionStr == "") {
            sSelectionStr = inSelected.options[nloop].value; 
          }
          else {
            sSelectionStr = sSelectionStr + "," + inSelected.options[nloop].value;
          }
        } // (nloop == 0)
      } // (optionList.options[nloop].selected == true)
    } // (nloop = 0; nloop < optionList.length; nloop++)

    if (sSelectionStr != "") {
    
      // send the traslated state array to the
    
      if ((!bAllRegion || !bAllCoutry) && bSelectedItem) {
        optionArray = sSelectionStr.split(",");
      }
      else {
        optionArray = ("All").split(",");
      } // (!bAllType && bSelectedItem)
    }
    else {
      if (isBase && !isView) {
        if ((sessBaseState != null) && (sessBaseState != "")) {
          var remove = /, /gi;
          sSelectionStr = sessBaseState.replace(remove, ",");
          optionArray = sSelectionStr.split(",");
        }
      }
      else {
        if (!isBase && !isView) {
          if ((sessCompState != null) && (sessCompState != "")) {
            var remove = /, /gi;
            sSelectionStr = sessCompState.replace(remove, ",");
            optionArray = sSelectionStr.split(",");
          }
        }
        else {
          if ((sessDocState != null) && (sessDocState != "")) {
            var remove = /, /gi;
            sSelectionStr = sessDocState.replace(remove, ",");
            optionArray = sSelectionStr.split(",");
          }
        }
      }
    } // (sSelectionStr != "")
  } // ((typeof(inSelected.name) != "undefined") && (inSelected != ""))

  if ((sSelectionStr == "") && !bSelectedItem) {
    optionArray = ("All").split(",");
    sSelectionStr = "All";
  } // (sSelectionStr == "")

  displayCboJS = inStateCbo;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  displayList = sCboContentRegion.split(",");

  for (var x = 0; x < displayList.length; x++) {

    sTempItem = displayList[x];

    for (var y = 0; y < contentRegionArray.length; y++) {
      if (contentRegionArray[y][LOCRGN_CONTINENT].toLowerCase() == sTempItem.toLowerCase()) {
        nContinent = y;
        break;
      }
    } // y

    for (var z = nContinent; z < contentRegionArray.length; z++) {

      if (contentRegionArray[z][LOCRGN_CONTINENT].toLowerCase() == sTempItem.toLowerCase()) {

        if ((contentRegionArray[z][LOCRGN_STATE_NAME] != "") && (contentRegionArray[z][LOCRGN_STATE_NAME] != sRememberState)) {

          if (inClientArrayJS(sCboCountry.split(","), contentRegionArray[z][LOCRGN_COUNTRY])) {

            if (nCurrentOption == 0) {

              displayCboJS.options[0].innerHTML = "All";
              displayCboJS.options[0].value = "All";

              if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() == "ALL")) {
                displayCboJS.options[0].selected = true;
                displayCboJS.options[0].selectedindex = 0;
                bfoundSelection = true;
              }
              else {
                if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                  if (sSelectionStr.substring(0, 3) == displayCboJS.options[nCurrentOption].value) {
                    bfoundSelection = true;
                    displayCboJS.options[0].selected = true;
                    displayCboJS.options[0].selectedindex = 0;
                  }
                  else {
                    displayCboJS.options[0].selected = false;
                  }
                }
              }

              // pick up the first option
              nCurrentOption = nCurrentOption + 1;
              displayCboJS.options[nCurrentOption] = new Option(contentRegionArray[z][LOCRGN_STATE_NAME]);
              displayCboJS.options[nCurrentOption].value = contentRegionArray[z][LOCRGN_STATE_NAME];
              displayCboJS.options[nCurrentOption].innerHTML = contentRegionArray[z][LOCRGN_STATE_NAME];

              if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                if (optionArray.length > 0) {
                  if (inClientArrayJS(optionArray, contentRegionArray[z][LOCRGN_STATE_NAME])) {
                    displayCboJS.options[nCurrentOption].selected = true;
                    displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                    bfoundSelection = true;
                  }
                }
              }
            }
            else {

              displayCboJS.options[nCurrentOption] = new Option(contentRegionArray[z][LOCRGN_STATE_NAME]);
              displayCboJS.options[nCurrentOption].value = contentRegionArray[z][LOCRGN_STATE_NAME];
              displayCboJS.options[nCurrentOption].innerHTML = contentRegionArray[z][LOCRGN_STATE_NAME];

              if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                if (optionArray.length > 0) {
                  if (inClientArrayJS(optionArray, contentRegionArray[z][LOCRGN_STATE_NAME])) {
                    displayCboJS.options[nCurrentOption].selected = true;
                    displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                    bfoundSelection = true;
                  }
                }
              }
            } // nCurrentOption == 0

            nCurrentOption = nCurrentOption + 1;
            sRememberState = contentRegionArray[z][LOCRGN_STATE_NAME];

          } // (inClientArrayJS(sCboCountry.split(","), contentRegionArray[z][LOCRGN_COUNTRY]))
        } // ((contentRegionArray[z][LOCRGN_STATE_NAME] != "") && (contentRegionArray[z][LOCRGN_STATE_NAME] != sRememberState))      
      } // (contentRegionArray[y][LOCRGN_CONTINENT].toLowerCase() == sTempItem.toLowerCase())  
    } // z
  } // x

  if (displayCboJS.options.length > 1) {
    displayCboJS = sortListBoxJS(displayCboJS);
    
    if (!isBase && !isView) {
      document.getElementById("hasCompanyTimeZonesID").value = "true";
    }
    else {
      if (!isBase && isView) {
        document.getElementById("hasViewTimeZonesID").value = "true";
      }
    }
    
  }
  else {
    displayCboJS.options[nCurrentOption].innerHTML = "No States";
    displayCboJS.options[nCurrentOption].value = "";
    displayCboJS.options[nCurrentOption].selected = false;

    if (!isBase && !isView) {
      document.getElementById("hasCompanyTimeZonesID").value = "false";
    }
    else {
      if (!isBase && isView) {
        document.getElementById("hasViewTimeZonesID").value = "false";
      }
    }

    return displayCboJS;
  }

  if ((!bfoundSelection) && (displayCboJS.options.length > 1)) {
    if (displayCboJS.options[0].innerHTML == "No States") {
      displayCboJS.options[0].selected = false;
    }
    else {
      if (displayCboJS.options.length > 1) {
        displayCboJS.options[0].selected = true;
        displayCboJS.options[0].selectedindex = 0;
      }
    }
  }
  else {
    if ((displayCboJS.options.length > 1) && (bfoundSelection)) {
      if (displayCboJS.options[0].innerHTML == "No States") {
        displayCboJS.options[0].selected = false;
      }
      else {
        if ((displayCboJS.options.length > 1) && (bfoundSelection) && (sSelectionStr.toUpperCase() != "ALL")) {
          displayCboJS.options[0].selected = false;
        }
      }
    }
  }

  optionArray = null;
  contentRegionArray = null;

  return displayCboJS;

}

function fillTimezone(inCboTimeZone, inWhich, inSelected, inState, isBase, isView, sessCompTz, sessDocTz) {

  var bSelectedItem = false;
  var bfoundSelection = false;
  var bStateMatch = false;

  var bAllState = false;

  var nRememberTimeZoneID = 0;
  var nCurrentOption = 0;

  var displayCboJS = null;
  var optionArray = null;
  var stateArray = null;

  var contentRegionArray = null;

  var sCboState = "";
  var sSelectionStr = "";

  if (inWhich.toLowerCase() == "continent") {
    if ((localAryContinent != null) && (localAryContinent.length > 0)) {
      contentRegionArray = localAryContinent;
    }
  }
  else {
    if ((localAryRegion != null) && (localAryRegion.length > 0)) {
      contentRegionArray = localAryRegion;
    }
  }

  // get the list of selected States
  if ((typeof (inState.name) != "undefined") && (inState != null)) {
    for (var nloop = 0; nloop < inState.length; nloop++) {
      if ((inState.options[nloop].selected == true) || (bAllState == true)) {
        if (nloop == 0) {
          bAllState = true;
        }
        else {
          if (sCboState == "") {
            sCboState = inState.options[nloop].value;
          }
          else {
            sCboState = sCboState + "," + inState.options[nloop].value;
          }
        } // (nloop == 0)
      } // ((inState.options[nloop].selected == true) || (bAllState == true))
    } // (nloop = 0; nloop < inState.length; nloop++)
  }
  else {
    bAllState = true;
  }

  if (bAllState && ((typeof (inState.name) != "undefined") && (inState != null))) {
        
    if (!isBase && !isView) {
      s_rememberLastCompanyStateJS = "";
    }
    else {
      if (!isBase && isView) {
        s_rememberLastViewStateJS = "";
      }
    }

    sCboState = "All";
    for (var nloop = 0; nloop < inSelected.length; nloop++) {
      inSelected.options[nloop].selected = false;
    }
    inSelected.options[0].selected = true;
  }

  // get currently selected items
  if ((typeof (inSelected.name) != "undefined") && (inSelected != null)) {
    for (var nloop = 0; nloop < inSelected.length; nloop++) {
      if (inSelected.options[nloop].selected == true) {
        if (nloop == 0) {
          sSelectionStr = "All";
        }
        else {
          bSelectedItem = true;
          if (sSelectionStr == "") {
            sSelectionStr = inSelected.options[nloop].value;
          }
          else {
            sSelectionStr = sSelectionStr + "," + inSelected.options[nloop].value;
          }
        } // (nloop == 0)
      } // (optionList.options[nloop].selected == true)
    } // (nloop = 0; nloop < optionList.length; nloop++)

    if (sSelectionStr != "") {
      if (bSelectedItem) {
        if ((sCboState != "") && (!bAllState)) {
          var tmpSelectedStateArray = sCboState.split(",");
          var tmpLastSelectedStateArray = null;
                 
          if (!isBase && !isView) {
            tmpLastSelectedStateArray = s_rememberLastCompanyStateJS.split(",");
            s_rememberLastCompanyStateJS = sCboState;
          }
          else {
            if (!isBase && isView) {
              tmpLastSelectedStateArray = s_rememberLastViewStateJS.split(",");
              s_rememberLastViewStateJS = sCboState;
            }
          }

          // check if last state matches any selected states
          // if we dont have a match then use the selected states time zone
          for (var y = 0; y < tmpSelectedStateArray.length; y++) {
            if (inClientArrayJS(tmpLastSelectedStateArray, tmpSelectedStateArray[y])) {
              bStateMatch = true;
            }
          }

          if (!bStateMatch) {
            sSelectionStr = "";
            var tmpStateArray = sCboState.split(",");
            for (var y = 0; y < tmpStateArray.length; y++) {
              if (sSelectionStr == "") {
                sSelectionStr = selectTimeZoneForOption(contentRegionArray, tmpStateArray[y]);
              }
              else {
                if (sSelectionStr.indexOf(selectTimeZoneForOption(contentRegionArray, tmpStateArray[y])) == -1) {
                  sSelectionStr = sSelectionStr + "," + selectTimeZoneForOption(contentRegionArray, tmpStateArray[y]);
                }
              }
            }
          }
          else { // add the selected states time zone to selection if its not selected

            if (!isBase && !isView) {
              tmpLastSelectedStateArray = s_rememberLastCompanyStateJS.split(",");
            }
            else {
              if (!isBase && isView) {
                tmpLastSelectedStateArray = s_rememberLastViewStateJS.split(",");
              }
            }
            
            for (var y = 0; y < tmpLastSelectedStateArray.length; y++) {
              if (sSelectionStr == "") {
                sSelectionStr = selectTimeZoneForOption(contentRegionArray, tmpLastSelectedStateArray[y]);
              }
              else {
                if (sSelectionStr.indexOf(selectTimeZoneForOption(contentRegionArray, tmpLastSelectedStateArray[y])) == -1) {
                  sSelectionStr = sSelectionStr + "," + selectTimeZoneForOption(contentRegionArray, tmpLastSelectedStateArray[y]);
                }
              }
            }
          } // !bStateMatch

        } // ((sCboState != "") && (!bAllState))

        optionArray = sSelectionStr.split(",");

      }
      else { // we didnt have selected item use last selected states time zone if we have one
        if ((sCboState != "") && !bAllState) {
          
          if (!isBase && !isView) {
            s_rememberLastCompanyStateJS = sCboState;
          }
          else {
            if (!isBase && isView) {
              s_rememberLastViewStateJS = sCboState;
            }
          }
          
          // append selected state values if state time zone has not been added
          if (sSelectionStr.indexOf(selectTimeZoneForOption(contentRegionArray, sCboState)) == -1) {
            if (sSelectionStr.toUpperCase() != "ALL") {
              sSelectionStr = sSelectionStr + "," + selectTimeZoneForOption(contentRegionArray, sCboState);
            }
            else {
              sSelectionStr = selectTimeZoneForOption(contentRegionArray, sCboState);
            }
          }
          optionArray = sSelectionStr.split(",");
        } // sCboState <> ""
        else {
          if ((sSelectionStr.toUpperCase() == "ALL") || (sSelectionStr.toUpperCase() == "")) {
            optionArray = ("ALL").split(",");
          }
          else {
            if (!isBase && !isView) {
              if ((sessCompTz != null) && (sessCompTz != "")) {
                sSelectionStr = sessCompTz;
                if ((sCboState != "") && !bAllState) {
                  // if we have a selected state then add that states timezone to the list to select
                  sSelectionStr = sSelectionStr + "," + selectTimeZoneForOption(contentRegionArray, sCboState);
                } // ((sCboState != "") && !bAllState)                
                optionArray = sSelectionStr.split(",");
              }
            }
            else {
              if (!isBase && isView) {
                if ((sessDocTz != null) && (sessDocTz != "")) {
                  sSelectionStr = sessDocTz;
                  if ((sCboState != "") && !bAllState) {
                    // if we have a selected state then add that states timezone to the list to select
                    sSelectionStr = sSelectionStr + "," + selectTimeZoneForOption(contentRegionArray, sCboState);
                  } //((sCboState != "") && !bAllState)                
                  optionArray = sSelectionStr.split(",");
                }
              } // (!isBase && isView)
            } // (!isBase && !isView)
          } // (sSelectionStr.toUpperCase() == "ALL")
        } // ((sCboState != "") && !bAllState)
      } // (bSelectedItem)
    }
    else {

      if (sSelectionStr != "") {
        optionArray = sSelectionStr.split(",");
      }
      else {
        if (!isBase && !isView) {
          if ((sessCompTz != null) && (sessCompTz != "")) {
            var remove = /, /gi;
            sSelectionStr = sessCompTz.replace(remove, ",");
            optionArray = sSelectionStr.split(",");
          }
        }
        else {
          if (!isBase && isView) {
            if ((sessDocTz != null) && (sessDocTz != "")) {
              var remove = /, /gi;
              sSelectionStr = sessDocTz.replace(remove, ",");
              optionArray = sSelectionStr.split(",");
            }
          } // (!isBase && isView)
        } // (!isBase && !isView)
      }
    } // (sSelectionStr != "") 
  } // ((typeof(inSelected.name) != "undefined") && (inSelected != ""))

  if ((sSelectionStr == null) || (sSelectionStr == "")) {
    optionArray = ("ALL").split(",");
  }

  displayCboJS = inCboTimeZone;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  for (var x = 0; x < localTimeZoneAry.length; x++) {
    if (localTimeZoneAry[x][LOCTZ_INDEX] != nRememberTimeZoneID) {

      if (nCurrentOption == 0) {

        displayCboJS.options[0].innerHTML = "All";
        displayCboJS.options[0].value = "All";

        if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() == "ALL")) {
          displayCboJS.options[0].selected = true;
          displayCboJS.options[0].selectedindex = 0;
          bfoundSelection = true;
        }
        else {
          if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
            if (sSelectionStr.substring(0, 3) == displayCboJS.options[0].value) {
              bfoundSelection = true;
              displayCboJS.options[0].selected = true;
              displayCboJS.options[0].selectedindex = 0;
            }
            else {
              displayCboJS.options[0].selected = false;
            }
          }
        }

        // pick up the first option
        nCurrentOption = nCurrentOption + 1;
        displayCboJS.options[nCurrentOption] = new Option(localTimeZoneAry[x][LOCTZ_INDEX]);
        displayCboJS.options[nCurrentOption].value = localTimeZoneAry[x][LOCTZ_INDEX];
        displayCboJS.options[nCurrentOption].innerHTML = localTimeZoneAry[x][LOCTZ_NAME];

        if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
          if (optionArray.length > 0) {
            if (inClientArrayJS(optionArray, localTimeZoneAry[x][LOCTZ_INDEX])) {
              displayCboJS.options[nCurrentOption].selected = true;
              displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
              bfoundSelection = true;
            }
          }
        }
      }
      else {

        displayCboJS.options[nCurrentOption] = new Option(localTimeZoneAry[x][LOCTZ_INDEX]);
        displayCboJS.options[nCurrentOption].value = localTimeZoneAry[x][LOCTZ_INDEX];
        displayCboJS.options[nCurrentOption].innerHTML = localTimeZoneAry[x][LOCTZ_NAME];

        if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
          if (optionArray.length > 0) {
            if (inClientArrayJS(optionArray, localTimeZoneAry[x][LOCTZ_INDEX])) {
              displayCboJS.options[nCurrentOption].selected = true;
              displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
              bfoundSelection = true;
            }
          }
        }
      } // nCurrentOption == 0

      nCurrentOption = nCurrentOption + 1;
      nRememberTimeZoneID = localTimeZoneAry[x][LOCTZ_INDEX];

    } // (localTimeZoneAry[x][LOCTZ_INDEX] != nRememberTimeZoneID)
  } // (var x = 0; x < localTimeZoneAry.length; x++)


  if ((!bfoundSelection) && (displayCboJS.options.length > 1)) {
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
  }

  contentRegionArray = null;
  optionArray = null;

  return displayCboJS;

}