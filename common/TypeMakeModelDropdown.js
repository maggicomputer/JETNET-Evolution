// ********************************************************************************
// Copyright 2004-11. JETNET,LLC. All rights reserved.
//
// $$Archive: /commonWebProject/common/TypeMakeModelDropdown.js $
// $$Author: Mike $
// $$Date: 5/21/20 10:16p $
// $$Modtime: 5/21/20 10:12p $
// $$Revision: 11 $
// $$Workfile: TypeMakeModelDropdown.js $
//
// ********************************************************************************

function getIndexForItem(inItemIndex, isFiltered) {

  var localArray = null;

  if (isFiltered) {
    if ((localFilterAirframeArray != null) && (localFilterAirframeArray.length > 0)) {
      localArray = localFilterAirframeArray;
    }
    for (var i = 0; i < localArray.length; i++) {
      if ((typeof (localArray[i][LOCAIR_INDEX]) != "undefined") && (localArray[i][LOCAIR_INDEX] != null)) {
        if (Number(localArray[i][LOCAIR_INDEX]) == Number(inItemIndex)) {
          break;
        }
      }
    }
    if (i == localArray.length) // we looped thru and didn't find matching index
      return -1;
    else
      return i;
  }
  else {
    return inItemIndex;
  }

}

function getAircraftAirframeLabelClient(productCodeCount, inAirframeType, inAircraftMakeType, bSingleAirframeSelected) {
  var sAirframeLabel, nProductCount;

  sAirframeLabel = "";
  nProductCount = productCodeCount;

  if (bSingleAirframeSelected) {
    nProductCount = 1;
  }

  for (i = 0; i < localAircraftTypeLableArray.length; i++) {

    if (localAircraftTypeLableArray[i][AIRMAKETYPE_AFT] == inAirframeType) {
      // airframe type matched
      if (localAircraftTypeLableArray[i][AIRMAKETYPE_AFMT] == inAircraftMakeType) {
        // airframe make type matched

        if (nProductCount == 1) {
          // this is a single product depending on the clients product return the propper label
          if (localAircraftTypeLableArray[i][AIRMAKETYPE_CODE].indexOf(localAircraftTypeLableArray[i][AIRMAKETYPE_AFT]) == -1) {
            sAirframeLabel = localAircraftTypeLableArray[i][AIRMAKETYPE_NAME];
            break;
          }
        }
        else {
          // this is a multi product depending on the clients product(s) return the propper label
          if (localAircraftTypeLableArray[i][AIRMAKETYPE_CODE].indexOf(localAircraftTypeLableArray[i][AIRMAKETYPE_AFT]) != -1) {
            sAirframeLabel = localAircraftTypeLableArray[i][AIRMAKETYPE_NAME];
            break;
          }
        } // nProductCount = 1
      }
    }
  }

  return sAirframeLabel;

}

function isModelInArray(n_inModelArray, n_inItemMake, isFiltered) {

  var localArray = null;
  var bFound = false;

  var modelIndex = 0;
  var tmpIndex = getIndexForItem(n_inItemMake, isFiltered);

  if (!isFiltered) {
    if ((localMasterAirframeArray != null) && (localMasterAirframeArray.length > 0)) {
      localArray = localMasterAirframeArray;
    }
  }
  else {
    if ((localFilterAirframeArray != null) && (localFilterAirframeArray.length > 0)) {
      localArray = localFilterAirframeArray;
    }
  }

  if ((n_inModelArray != null) && (n_inModelArray != "")) {
    if (n_inModelArray[0].toUpperCase() == "ALL") {
      bFound = true;
    }
    else {
      for (var xloop = 0; xloop < n_inModelArray.length; xloop++) {
        modelIndex = getIndexForItem(n_inModelArray[xloop], isFiltered);
        if ((localArray[modelIndex][LOCAIR_MAKE] == localArray[tmpIndex][LOCAIR_MAKE]) &&
          (localArray[modelIndex][LOCAIR_TYPE] == localArray[tmpIndex][LOCAIR_TYPE]) &&
          (localArray[modelIndex][LOCAIR_FRAME] == localArray[tmpIndex][LOCAIR_FRAME])) {
          bFound = true;
          break;
        }
      }
    }
  }

  localArray = null;
  return bFound;

}

function fillAircraftType(inCboType, inSelected, isFiltered, productCodeCount, sessionType) {

  var bfoundSelection = false;
  var sRememberType = "";
  var nCurrentOption = 0;

  var bSingleAirframeSelected = false;

  var displayCboJS = null;
  var optionArray = null;
  var localArray = null;

  var sSelectionStr = "";

  if (!isFiltered) {
    if ((localMasterAirframeArray != null) && (localMasterAirframeArray.length > 0)) {
      localArray = localMasterAirframeArray;
    }
  }
  else {
    if ((localFilterAirframeArray != null) && (localFilterAirframeArray.length > 0)) {
      localArray = localFilterAirframeArray;
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
      if (sSelectionStr.toUpperCase() != "ALL") {
        sessionType = sSelectionStr;
      }
      else {
        if ((sessionType != null) && (sessionType != "")) {
          bSelectedItem = true;
          var remove = /, /gi;
          sSelectionStr = sessionType.replace(remove, ",");
        }
      }

      optionArray = sSelectionStr.split(",");
    }
    else {
      if ((sessionType != null) && (sessionType != "")) {
        bSelectedItem = true;
        var remove = /, /gi;
        sSelectionStr = sessionType.replace(remove, ",");
        optionArray = sSelectionStr.split(",");
      }
    }
       
  } // ((typeof(inSelected.name) != "undefined") && (inSelected != ""))

  // we are just filtering the output
  if (isFiltered) {
    // if we have more than 2 product codes and we are filtered then 
    if (productCodeCount < 2) {
      bSingleAirframeSelected = true
    }
    else {
      bSingleAirframeSelected = false
    }
  }
  else {
    bSingleAirframeSelected = false
  }

  if (sSelectionStr.toUpperCase() != "ALL") {

    var rememberAirframeType = "";
    var rememberAircraftType = "";
    var sTypeStr = "";
    var sAirFrameStr = "";

    if ((optionArray != null) && (optionArray != "")) {

      // get the types for selected index	   
      for (var x = 0; x < optionArray.length; x++) {
        if (!isNaN(optionArray[x])) {
          if ((localMasterAirframeArray[optionArray[x]][LOCAIR_TYPE] != rememberAircraftType) || (localMasterAirframeArray[optionArray[x]][LOCAIR_FRAME] != rememberAirframeType)) {
            if (sTypeStr == "") {
              sTypeStr = localMasterAirframeArray[optionArray[x]][LOCAIR_TYPE];
              sAirFrameStr = localMasterAirframeArray[optionArray[x]][LOCAIR_FRAME];
            }
            else {
              sTypeStr = sTypeStr + "," + localMasterAirframeArray[optionArray[x]][LOCAIR_TYPE];
              sAirFrameStr = sAirFrameStr + "," + localMasterAirframeArray[optionArray[x]][LOCAIR_FRAME];
            }
          }
          rememberAircraftType = localMasterAirframeArray[optionArray[x]][LOCAIR_TYPE];
          rememberAirframeType = localMasterAirframeArray[optionArray[x]][LOCAIR_FRAME];
        }
      }
    }

    var tmpTypeArray = sTypeStr.split(",");
    var tmpFrameArray = sAirFrameStr.split(",");
    var tmpTypeString = "";

    // get the local type because it might not be in our filtered list
    for (var z = 0; z < sTypeStr.length; z++) {
      for (var y = 0; y < localArray.length; y++) {
        if ((typeof (localArray[y][LOCAIR_TYPE]) != "undefined") && (localArray[y][LOCAIR_TYPE] != null)) {
          if ((localArray[y][LOCAIR_TYPE] == tmpTypeArray[z]) && (localArray[y][LOCAIR_FRAME] == tmpFrameArray[z])) {
            if (tmpTypeString == "") {
              tmpTypeString = localArray[y][LOCAIR_INDEX];
            }
            else {
              tmpTypeString = tmpTypeString + "," + localArray[y][LOCAIR_INDEX];
            }
            break;
          }
        }
      }
    }
    optionArray = tmpTypeString.split(",");
  }

  displayCboJS = inCboType;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  for (var iloop = 0; iloop < localArray.length; iloop++) {
    if ((typeof (localArray[iloop][LOCAIR_TYPE]) != "undefined") && (localArray[iloop][LOCAIR_TYPE] != null)) {
      if (localArray[iloop][LOCAIR_TYPE] != sRememberType) {
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
          displayCboJS.options[nCurrentOption] = new Option(localArray[iloop][LOCAIR_INDEX]);
          displayCboJS.options[nCurrentOption].value = localArray[iloop][LOCAIR_INDEX];
          displayCboJS.options[nCurrentOption].innerHTML = getAircraftAirframeLabelClient(productCodeCount, localArray[iloop][LOCAIR_FRAME], localArray[iloop][LOCAIR_TYPE], bSingleAirframeSelected);

          if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
            if (optionArray.length > 0) {
              if (inClientArrayJS(optionArray, localArray[iloop][LOCAIR_INDEX])) {
                displayCboJS.options[nCurrentOption].selected = true;
                displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                bfoundSelection = true;
              }
            }
          }
        }
        else {

          displayCboJS.options[nCurrentOption] = new Option(localArray[iloop][LOCAIR_INDEX]);
          displayCboJS.options[nCurrentOption].value = localArray[iloop][LOCAIR_INDEX];
          displayCboJS.options[nCurrentOption].innerHTML = getAircraftAirframeLabelClient(productCodeCount, localArray[iloop][LOCAIR_FRAME], localArray[iloop][LOCAIR_TYPE], bSingleAirframeSelected);

          if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
            if (optionArray.length > 0) {
              if (inClientArrayJS(optionArray, localArray[iloop][LOCAIR_INDEX])) {
                displayCboJS.options[nCurrentOption].selected = true;
                displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                bfoundSelection = true;
              }
            }
          }
        } // nCurrentOption == 0

        nCurrentOption = nCurrentOption + 1;
        sRememberType = localArray[iloop][LOCAIR_TYPE];

      } // ((localArray[iloop][LOCAIR_INDEX] != -1) && (localArray[iloop][LOCAIR_TYPE] != sRememberType))
    } // (localArray[iloop][LOCAIR_INDEX] != -1)

  } // (iloop = 0; iloop < localArray.length; iloop++)


  if ((!bfoundSelection) && (displayCboJS.options.length > 1)) {
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
  }

  localArray = null;
  optionArray = null;

  return displayCboJS;

}

function fillAircraftMake(inMakeCbo, inSelAircraftType, inSelected, isFiltered, isHeliOnlyFlag, sessionMake) {

  var bfoundSelection = false;
  var sTempMakeID = 0;
  var sTempMakeIndex = 0;
  var sAirframeMakeType = "";
  var rememberAirframeType = "";

  var sRememberMake = "";
  var sCboType = "";

  var bAllType = false;
  var nCurrentOption = 0;
  var bSelectedItem = false;
  var bSingleTypeSelected = false;
  var bSingleAirframeSelected = false;

  var selectedType = "";
  var selectedAirframe = "";
  var selectedTypeAirframe = "";
  var selectedModelAirframe = "";

  var displayCboJS = null;
  var displayArray = null; // array of items to display in dropdown
  var optionArray = null; // list of selected items
  var localArray = null;
  var airframeArray = null;

  var sSelectionStr = "";

  if (!isFiltered) {
    if ((localMasterAirframeArray != null) && (localMasterAirframeArray.length > 0)) {
      localArray = localMasterAirframeArray;
    }
  }
  else {
    if ((localFilterAirframeArray != null) && (localFilterAirframeArray.length > 0)) {
      localArray = localFilterAirframeArray;
    }
  }

  // get the list of selected types
  if ((typeof (inSelAircraftType.name) != "undefined") && (inSelAircraftType != null)) {
    for (var nloop = 0; nloop < inSelAircraftType.length; nloop++) {
      if ((inSelAircraftType.options[nloop].selected == true) || (bAllType == true)) {
        if (nloop == 0) {
          bAllType = true;
        }
        else {
          if (sCboType == "") {
            sCboType = inSelAircraftType.options[nloop].value;
            selectedTypeAirframe = inSelAircraftType.options[nloop].value;
          }
          else {
            sCboType = sCboType + "," + inSelAircraftType.options[nloop].value;
            selectedTypeAirframe = selectedTypeAirframe + "," + inSelAircraftType.options[nloop].value;
          }
        } // (nloop == 0)
      } // ((inSelAircraftType.options[nloop].selected == true) || (bAllType == true))
    } // (nloop = 0; nloop < inSelAircraftType.length; nloop++)
  }
  else {
    bAllType = true;
  }

  // get currently selected items
  if ((typeof (inSelected.name) != "undefined") && (inSelected != null)) {
    for (var mloop = 0; mloop < inSelected.length; mloop++) {
      if (inSelected.options[mloop].selected == true) {
        if (mloop == 0) {
          sSelectionStr = "All";
        }
        else {
          bSelectedItem = true;
          if (sSelectionStr == "") {
            sSelectionStr = inSelected.options[mloop].value;
          }
          else {
            sSelectionStr = sSelectionStr + "," + inSelected.options[mloop].value;
          }
        } // (nloop == 0)
      } // (optionList.options[mloop].selected == true)
    } // (mloop = 0; mloop < optionList.length; mloop++)

    if (sSelectionStr != "") {
      if ((!bAllType) && bSelectedItem) {
        if (sSelectionStr.toUpperCase() != "ALL") {
          sessionMake = sSelectionStr;
        }
        optionArray = sSelectionStr.split(",");
        if (optionArray.length > 0) {
          for (var x = 0; x < optionArray.length; x++) {
            if (selectedType == "") {
              selectedType = optionArray[x];
              selectedModelAirframe = optionArray[x];
            }
            else {
              selectedType = selectedType + "," + optionArray[x];
              selectedModelAirframe = selectedModelAirframe + "," + optionArray[x];
            }
          } // (xloop = 0; xloop < optionArray.length; xloop++)
        }
      }
      else {
        optionArray = ("All").split(",");
      } // (!bAllType && bSelectedItem)    
    } // (sSelectionStr != "")

    if ((sessionMake != null) && (sessionMake != "")) {
      bSelectedItem = true;
      var remove = /, /gi;
      sSelectionStr = sessionMake.replace(remove, ",");
      optionArray = sSelectionStr.split(",");

      if (optionArray.length > 0) {
        for (var x = 0; x < optionArray.length; x++) {
          if (selectedType == "") {
            selectedType = optionArray[x];
            selectedModelAirframe = optionArray[x];
          }
          else {
            selectedType = selectedType + "," + optionArray[x];
            selectedModelAirframe = selectedModelAirframe + "," + optionArray[x];
          }
        } // (var x = 0; x < optionArray.length; x++)
      }
    } // (sessionMake != "") 

  } // ((typeof(inSelected.name) != "undefined") && (inSelected != ""))

  if ((sCboType != "") && (!bSelectedItem)) {
    displayArray = sCboType.split(",");
    selectedAirframe = selectedTypeAirframe;
  }
  else {
    if ((!bAllType) && (bSelectedItem)) {

      // need to check if this type is different from the selected types
      // if its different then use selected types to display model
      var tmpSelectedType = sCboType.split(",");
      var inModelType = selectedType.split(",");
      var bMatchedTypes = false;
      var makeIndex = 0;
      var typeIndex = 0;

      for (var x = 0; x < inModelType.length; x++) {
        for (var y = 0; y < tmpSelectedType.length; y++) {
          // get index for item
          if (!isNaN(inModelType[x]) && !isNaN(tmpSelectedType[y])) {
            makeIndex = getIndexForItem(inModelType[x], isFiltered);
            typeIndex = getIndexForItem(tmpSelectedType[y], isFiltered);
            if ((makeIndex != -1) && (typeIndex != -1)) {
              if (localArray[makeIndex][LOCAIR_TYPE] == localArray[typeIndex][LOCAIR_TYPE]) {
                bMatchedTypes = true;
              }
            }
          }
        }
      }

      if (!bMatchedTypes) {
        displayArray = sCboType.split(",");
        selectedAirframe = selectedTypeAirframe;
        optionArray = ("All").split(",");
        sSelectionStr = "";
      }
      else {
        displayArray = sCboType.split(",");
        selectedAirframe = selectedTypeAirframe;
      }
    }
    else {
      if (bAllType && ((sessionMake != null) && (sessionMake != ""))) {
        if ((selectedType != null) && (selectedType != "")) {
          displayArray = selectedType.split(",");
          selectedAirframe = selectedModelAirframe;
        }
        else {
          displayArray = sCboType.split(",");
          selectedAirframe = selectedTypeAirframe;
        }
      }
      else {
        displayArray = sCboType.split(",");
        selectedAirframe = selectedTypeAirframe;
        optionArray = ("All").split(",");
        sSelectionStr = "";
      }
    }
  }

  if ((sSelectionStr.toUpperCase() != "ALL") && (sSelectionStr.toUpperCase() != "") && (!bAllType)) {

    var rememberAmodID = "";
    var sMakeStr = "";
    var sTypeStr = "";

    if ((optionArray != null) && (optionArray != "")) {

      // get the makes for selected index	(and selected product filter)
      for (var x = 0; x < optionArray.length; x++) {
        if (!isNaN(optionArray[x])) {
          if (localMasterAirframeArray[optionArray[x]][LOCAIR_MODEL_ID] != rememberAmodID) {
            if (sMakeStr == "") {
              sMakeStr = localMasterAirframeArray[optionArray[x]][LOCAIR_MAKE];
              sTypeStr = localMasterAirframeArray[optionArray[x]][LOCAIR_TYPE];
            }
            else {
              sMakeStr = sMakeStr + "," + localMasterAirframeArray[optionArray[x]][LOCAIR_MAKE];
              sTypeStr = sTypeStr + "," + localMasterAirframeArray[optionArray[x]][LOCAIR_TYPE];
            }
          }
          rememberAmodID = localMasterAirframeArray[optionArray[x]][LOCAIR_MODEL_ID];
        }
      }

    }

    var tmpMakeArray = sMakeStr.split(",");
    var tmpTypeArray = sTypeStr.split(",");
       
    var tmpMakeString = "";
    var tmpDisplayString = "";
    var tmpTypeString = "";

    // get the local indexes because it might not be in our filtered list
    for (var x = 0; x < tmpMakeArray.length; x++) {
      for (var y = 0; y < localArray.length; y++) {
        if ((typeof (localArray[y][LOCAIR_TYPE]) != "undefined") && (localArray[y][LOCAIR_TYPE] != null)) {
          if ((localArray[y][LOCAIR_MAKE] == tmpMakeArray[x]) && (localArray[y][LOCAIR_TYPE] == tmpTypeArray[x])) {
            if (tmpMakeString == "") {
              tmpMakeString = localArray[y][LOCAIR_INDEX];
              tmpTypeString = localArray[y][LOCAIR_TYPE];
            }
            else {
              tmpMakeString = tmpMakeString + "," + localArray[y][LOCAIR_INDEX];
              if (tmpTypeString.indexOf(localArray[y][LOCAIR_TYPE]) == -1) {
                tmpTypeString = tmpTypeString + "," + localArray[y][LOCAIR_TYPE];
              }
            }
            break;
          }
        }
      }
    }

    // if we find matches then try to re-match
    if ((tmpTypeString != null) && (tmpTypeString != "")) {
      var tmpLocalTypeArray = tmpTypeString.split(",");

      // get the first local indes because it might not be in our filtered list
      for (var x = 0; x < tmpLocalTypeArray.length; x++) {
        for (var y = 0; y < localArray.length; y++) {
          if ((typeof (localArray[y][LOCAIR_TYPE]) != "undefined") && (localArray[y][LOCAIR_TYPE] != null)) {
            if (localArray[y][LOCAIR_TYPE] == tmpLocalTypeArray[x]) {
              if (tmpDisplayString == "") {
                tmpDisplayString = localArray[y][LOCAIR_INDEX];
              }
              else {
                tmpDisplayString = tmpDisplayString + "," + localArray[y][LOCAIR_INDEX];
              }
              break;
            }
          }
        }
      }

      optionArray = tmpMakeString.split(",");

      if (displayArray.length == 0) {
        displayArray = tmpDisplayString.split(",");
      }

      if (selectedAirframe != "") {
        selectedAirframe = tmpMakeString;
      }

    }
    else {
      displayArray = sCboType.split(",");
      optionArray = ("All").split(",");
      selectedAirframe = selectedTypeAirframe;
    }
  }

  var bCheckFilter = false;

  // check to see if selections are available if filter has been applied
  if (isFiltered) {
    if ((displayArray != null) && (displayArray != "")) {
      for (var dloop = 0; dloop < displayArray.length; dloop++) {
        if (getIndexForItem(displayArray[dloop], isFiltered) != -1) {
          bCheckFilter = true;
        }
      }
    }

    if (!bCheckFilter) {
      displayArray = sCboType.split(",");
      optionArray = ("All").split(",");
      selectedAirframe = selectedTypeAirframe;
    }

  }

  displayCboJS = inMakeCbo;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  if ((displayArray.length == 1) && (!bAllType)) {
    bSingleTypeSelected = true;
  }

  if (displayArray.length > 0) {

    if (selectedAirframe != "") {

      bSingleAirframeSelected = true;
      airframeArray = selectedAirframe.split(",");

      if ((airframeArray != null) && (airframeArray != "")) {

        for (var xloop = 0; xloop < airframeArray.length; xloop++) {
          if (!isNaN(airframeArray[xloop])) {
            if ((localArray[getIndexForItem(airframeArray[xloop], isFiltered)][LOCAIR_FRAME] != rememberAirframeType) && (rememberAirframeType != "")) {
              bSingleAirframeSelected = false;
              break;
            }
            rememberAirframeType = localArray[getIndexForItem(airframeArray[xloop], isFiltered)][LOCAIR_FRAME];
          }
        }
      }
    }
    else {
      airframeArray = ("0").split(",");
      bSingleAirframeSelected = true;
    }

    for (var xloop = 0; xloop < displayArray.length; xloop++) {
      if (displayArray[xloop].toUpperCase() != "ALL") {
        sTempMakeID = displayArray[xloop];

        sTempMakeIndex = getIndexForItem(sTempMakeID, isFiltered);

        sAirframeMakeType = localArray[Number(sTempMakeIndex)][LOCAIR_TYPE];
        sAirframeType = localArray[Number(sTempMakeIndex)][LOCAIR_FRAME];
      }

      if (bAllType && ((sessionMake != null) && (sessionMake != ""))) {
        sTempMakeIndex = "0"
      }

      for (var zloop = Number(sTempMakeIndex); zloop < localArray.length; zloop++) {
        if ((typeof (localArray[zloop][LOCAIR_INDEX]) != "undefined") && (localArray[zloop][LOCAIR_INDEX] != null)) {
          if ((localArray[zloop][LOCAIR_TYPE] == sAirframeMakeType) && (localArray[zloop][LOCAIR_FRAME] == sAirframeType)) {
            if (((typeof (localArray[zloop][LOCAIR_MAKE]) != "undefined") && (localArray[zloop][LOCAIR_MAKE] != null)) && (localArray[zloop][LOCAIR_MAKE] != sRememberMake)) {
              if (nCurrentOption == 0) {

                displayCboJS.options[0].innerHTML = "All";
                displayCboJS.options[0].value = "All";

                if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() == "ALL")) {
                  displayCboJS.options[0].selected = true;
                  displayCboJS.options[0].selectedindex = 0;
                }
                else {
                  if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                    if (sSelectionStr.substring(0, 3) == displayCboJS.options[nCurrentOption].value) {
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
                displayCboJS.options[nCurrentOption] = new Option("");
                displayCboJS.options[nCurrentOption].value = localArray[zloop][LOCAIR_INDEX];

                if (!bSingleTypeSelected) {
                  if (!isHeliOnlyFlag) {
                    if (!bSingleAirframeSelected) {
                      displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_FRAME] + "][" + localArray[zloop][LOCAIR_TYPE] + "] - " + localArray[zloop][LOCAIR_MAKE];
                    }
                    else {
                      displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_TYPE] + "] - " + localArray[zloop][LOCAIR_MAKE];
                    }
                  }
                  else {
                    displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_TYPE] + "] - " + localArray[zloop][LOCAIR_MAKE];
                  }
                }
                else {
                  displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCAIR_MAKE];
                }

                if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                  if (optionArray.length > 0) {
                    if (inClientArrayJS(optionArray, localArray[zloop][LOCAIR_INDEX])) {
                      displayCboJS.options[nCurrentOption].selected = true;
                      displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                      bfoundSelection = true;
                    }
                  }
                }
              }
              else {

                displayCboJS.options[nCurrentOption] = new Option("");
                displayCboJS.options[nCurrentOption].value = localArray[zloop][LOCAIR_INDEX];

                if (!bSingleTypeSelected) {
                  if (!isHeliOnlyFlag) {
                    if (!bSingleAirframeSelected) {
                      displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_FRAME] + "][" + localArray[zloop][LOCAIR_TYPE] + "] - " + localArray[zloop][LOCAIR_MAKE];
                    }
                    else {
                      displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_TYPE] + "] - " + localArray[zloop][LOCAIR_MAKE];
                    }
                  }
                  else {
                    displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_TYPE] + "] - " + localArray[zloop][LOCAIR_MAKE];
                  }
                }
                else {
                  displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCAIR_MAKE];
                }

                if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                  if (optionArray.length > 0) {
                    if (inClientArrayJS(optionArray, localArray[zloop][LOCAIR_INDEX])) {
                      displayCboJS.options[nCurrentOption].selected = true;
                      displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                      bfoundSelection = true;
                    }
                  }
                }
              } // (nCurrentOption == 0)

              nCurrentOption = nCurrentOption + 1;
              sRememberMake = localArray[zloop][LOCAIR_MAKE];

            } // ((localArray[zloop][LOCAIR_MAKE] != "") && (localArray[zloop][LOCAIR_MAKE] != sRememberMake))
          }  // ((localArray[zloop][LOCAIR_TYPE] == sAirframeMakeType) && (localArray[zloop][LOCAIR_FRAME] == sAirframeType))
        } // ((localArray[zloop][LOCAIR_TYPE] != null) && (localArray[zloop][LOCAIR_TYPE] != ""))
      } //	zloop
    } //	xloop
    if (nCurrentOption == 0) {
      displayCboJS.options[0].innerHTML = "All";
      displayCboJS.options[0].value = "All";
      displayCboJS.options[0].selected = true;
      displayCboJS.options[0].selectedindex = 0;
    }
  }
  else {
    displayCboJS.options[0].innerHTML = "All";
    displayCboJS.options[0].value = "All";
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
  }  // (displayArray.length > 0)

  if (displayCboJS.options.length > 1) {
    displayCboJS = sortListBoxJS(displayCboJS);
  }

  if ((!bfoundSelection) && (displayCboJS.options.length > 1)) {
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
  }
  else {
    if ((displayCboJS.options.length > 1) && (bfoundSelection)) {
      displayCboJS.options[0].selected = false;
    }
  }

  aircraftTypeList = null;
  aircraftMakeArray = null;

  displayArray = null;
  optionArray = null;
  localArray = null;
  airframeArray = null;

  return displayCboJS;

}

function fillAircraftModel(inModelCbo, inSelAircraftType, inSelAircraftMake, inSelected, isFiltered, isHeliOnlyFlag, sessionModel) {

  var bfoundSelection = false;
  var bSelectedItem = false;
  var sTempModelID = 0;
  var sTempModelIndex = 0;
  var sAirframeMakeType = "";
  var sAirframeType = "";
  var sAirframeMake = "";
  var rememberAirframeType = "";
  var rememberMake = "";
  var nRememberModel = 0;
  var rememberMakeType = "";
  var sCboType = "";
  var sCboMake = "";

  var bAllType = false;
  var bAllMake = false;
  var selectedMakeType = "";
  var selectedAirframe = "";
  var selectedTypeAirframe = "";
  var selectedMakeAirframe = "";

  var nCurrentOption = 0;

  var bSingleMakeSelected = false;
  var bSingleTypeSelected = false;
  var bSingleAirframeSelected = false;
  var bSingleMakeAirframeSelected = false;
  var bSingleMakeTypeSelected = false;

  var makeArray = null;

  var displayCboJS = null;
  var displayArray = null;
  var optionArray = null;
  var localArray = null;
  var airframeArray = null;

  var sSelectionStr = "";

  if (!isFiltered) {
    if ((localMasterAirframeArray != null) && (localMasterAirframeArray.length > 0)) {
      localArray = localMasterAirframeArray;
    }
  }
  else {
    if ((localFilterAirframeArray != null) && (localFilterAirframeArray.length > 0)) {
      localArray = localFilterAirframeArray;
    }
  }

  // get the list of selected types
  if ((typeof (inSelAircraftType.name) != "undefined") && (inSelAircraftType != null)) {
    for (var nloop = 0; nloop < inSelAircraftType.length; nloop++) {
      if ((inSelAircraftType.options[nloop].selected == true) || (bAllType == true)) {
        if (nloop == 0) {
          bAllType = true;
        }
        else {
          if (sCboType == "") {
            sCboType = inSelAircraftType.options[nloop].value;
            selectedTypeAirframe = inSelAircraftType.options[nloop].value;
          }
          else {
            sCboType = sCboType + "," + inSelAircraftType.options[nloop].value;
            selectedTypeAirframe = selectedTypeAirframe + "," + inSelAircraftType.options[nloop].value;
          }
        } // (nloop == 0)
      } // ((inSelAircraftType.options[nloop].selected == true) || (bAllType == true))
    } // (nloop = 0; nloop < inSelAircraftType.length; nloop++)
  }
  else {
    bAllType = true;
  }

  // get the list of selected makes
  if ((typeof (inSelAircraftMake.name) != "undefined") && (inSelAircraftMake != null)) {
    for (var nloop = 0; nloop < inSelAircraftMake.length; nloop++) {
      if ((inSelAircraftMake.options[nloop].selected == true) || (bAllMake == true)) {
        if (nloop == 0) {
          bAllMake = true;
        }
        else {
          if (sCboMake == "") {
            sCboMake = inSelAircraftMake.options[nloop].value;
            selectedMakeType = inSelAircraftMake.options[nloop].value;
            selectedMakeAirframe = inSelAircraftMake.options[nloop].value;
          }
          else {
            sCboMake = sCboMake + "," + inSelAircraftMake.options[nloop].value;
            selectedMakeType = selectedMakeType + "," + inSelAircraftMake.options[nloop].value;
            selectedMakeAirframe = selectedMakeAirframe + "," + inSelAircraftMake.options[nloop].value;
          }

          if ((bAllType) && (!bAllMake)) {
            sCboType = "";
          }
        } // (nloop == 0)
      } // ((inSelAircraftType.options[nloop].selected == true) || (bAllType == true))
    } // (nloop = 0; nloop < inSelAircraftType.length; nloop++)
  }

  if (sCboMake != "") {
    makeArray = sCboMake.split(",");
  }
  else {
    bAllMake = true;
    makeArray = ("All").split(",");
  } // (sCboMake != "")

  if ((makeArray == null) && (!bAllMake)) {
    bSingleMakeSelected = true;
  }
  else {
    if (((makeArray != null)) && (makeArray.length > 0) && (!bAllMake)) {

      bSingleMakeSelected = true;

      for (var xloop = 0; xloop < makeArray.length; xloop++) {
        if (!isNaN(makeArray[xloop])) {
          if ((localArray[getIndexForItem(makeArray[xloop], isFiltered)][LOCAIR_MAKE] != rememberMake) && (rememberMake != "")) {
            bSingleMakeSelected = false;
            break;
          }
          rememberMake = localArray[getIndexForItem(makeArray[xloop], isFiltered)][LOCAIR_MAKE];
        }
      }
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
      if (sSelectionStr.toUpperCase() != "ALL") {
        sessionModel = sSelectionStr;
      }
      else {
        if ((sessionModel != null) && (sessionModel != "")) {
          bSelectedItem = true;
          var remove = /, /gi;
          sSelectionStr = sessionModel.replace(remove, ",");
        }
      }

      optionArray = sSelectionStr.split(",");
    }
    else {
      if ((sessionModel != null) && (sessionModel != "")) {
        bSelectedItem = true;
        var remove = /, /gi;
        sSelectionStr = sessionModel.replace(remove, ",");
        optionArray = sSelectionStr.split(",");
      }
    }
  }

  displayCboJS = inModelCbo;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  if ((sCboType != "") && (!bSelectedItem) && (sCboMake == "")) {
    displayArray = sCboType.split(",");
    selectedAirframe = selectedTypeAirframe;
  }
  else {
    if ((bSelectedItem) && (!bAllType) && (sCboMake != "")) {
      displayArray = selectedMakeType.split(",");
      selectedAirframe = selectedMakeAirframe;
    }
    else {
      if ((sCboMake != "") && (!bAllMake)) {
        displayArray = selectedMakeType.split(",");
        selectedAirframe = selectedMakeAirframe;
      }
      else {
        displayArray = ("All").split(",");
        bAllType = true;
      }
    }
  }

  if ((displayArray.length == 1) && (!bAllType)) {
    bSingleTypeSelected = true;
  }

  if (bAllType && bAllMake) {
    displayCboJS.options[0].innerHTML = "All";
    displayCboJS.options[0].value = "All";
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
    return displayCboJS;
  }
  else {
    if ((!bAllType) && bAllMake) {
      displayCboJS.options[0].innerHTML = "All";
      displayCboJS.options[0].value = "All";
      displayCboJS.options[0].selected = true;
      displayCboJS.options[0].selectedindex = 0;
      return displayCboJS;
    }
  }

  if (displayArray.length > 0) {
    if (selectedAirframe != "") {

      bSingleAirframeSelected = true;
      airframeArray = selectedAirframe.split(",");

      if ((airframeArray != null) && (airframeArray != "")) {
        for (var xloop = 0; xloop < airframeArray.length; xloop++) {
          if (!isNaN(airframeArray[xloop])) {
            if ((localArray[getIndexForItem(airframeArray[xloop], isFiltered)][LOCAIR_FRAME] != rememberAirframeType) && (rememberAirframeType != "")) {
              bSingleAirframeSelected = false;
              break;
            }
            rememberAirframeType = localArray[getIndexForItem(airframeArray[xloop], isFiltered)][LOCAIR_FRAME];
          }
        }
      }
    }
    else {
      airframeArray = ("F").split(",");
      bSingleAirframeSelected = true;
    }

    if ((makeArray.length > 0) && (makeArray[0].toUpperCase() != "ALL")) {

      bSingleMakeAirframeSelected = true;

      if ((makeArray != null) && (makeArray != "")) {
        for (var xloop = 0; xloop < makeArray.length; xloop++) {
          if (!isNaN(makeArray[xloop])) {
            if ((localArray[getIndexForItem(makeArray[xloop], isFiltered)][LOCAIR_FRAME] != rememberAirframeType) && (rememberAirframeType != "")) {
              bSingleMakeAirframeSelected = false;
              break;
            }
            rememberAirframeType = localArray[getIndexForItem(makeArray[xloop], isFiltered)][LOCAIR_FRAME];
          }
        }
      }
    }

    if ((makeArray.length > 0) && (makeArray[0].toUpperCase() != "ALL")) {

      bSingleMakeTypeSelected = true;

      if ((makeArray != null) && (makeArray != "")) {
        for (var xloop = 0; xloop < makeArray.length; xloop++) {
          if (!isNaN(makeArray[xloop])) {
            if ((localArray[getIndexForItem(makeArray[xloop], isFiltered)][LOCAIR_TYPE] != rememberMakeType) && (rememberMakeType != "")) {
              bSingleMakeTypeSelected = false;
              break;
            }
            rememberMakeType = localArray[getIndexForItem(makeArray[xloop], isFiltered)][LOCAIR_TYPE];
          }
        }
      }
    }

    for (var xloop = 0; xloop < displayArray.length; xloop++) {  // loop through the display array to get the makes to find the models to display
      if (displayArray[xloop].toUpperCase() != "ALL") {
        sTempModelID = displayArray[xloop];

        sTempModelIndex = getIndexForItem(sTempModelID, isFiltered);

        sAirframeMakeType = localArray[Number(sTempModelIndex)][LOCAIR_TYPE];
        sAirframeType = localArray[Number(sTempModelIndex)][LOCAIR_FRAME];
        sAirframeMake = localArray[Number(sTempModelIndex)][LOCAIR_MAKE];
      }

      for (var zloop = Number(sTempModelIndex); zloop < localArray.length; zloop++) {
        if ((typeof (localArray[zloop][LOCAIR_INDEX]) != "undefined") && (localArray[zloop][LOCAIR_INDEX] != null)) {
          if ((localArray[zloop][LOCAIR_TYPE] == sAirframeMakeType) && (localArray[zloop][LOCAIR_FRAME] == sAirframeType) && (localArray[zloop][LOCAIR_MAKE] == sAirframeMake)) {
            if ((Number(localArray[zloop][LOCAIR_MODEL_ID]) != 0) && (Number(localArray[zloop][LOCAIR_MODEL_ID]) != Number(nRememberModel))) {
              if (isModelInArray(makeArray, localArray[zloop][LOCAIR_INDEX], isFiltered)) {

                if (nCurrentOption == 0) {
                  displayCboJS.options[0].innerHTML = "All";
                  displayCboJS.options[0].value = "All";

                  if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() == "ALL")) {
                    displayCboJS.options[0].selected = true;
                    displayCboJS.options[0].selectedindex = 0;
                  }
                  else {
                    if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                      if (sSelectionStr.substring(0, 3) == displayCboJS.options[nCurrentOption].value) {
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
                  displayCboJS.options[nCurrentOption] = new Option("");
                  displayCboJS.options[nCurrentOption].value = localArray[zloop][LOCAIR_INDEX];

                  if (!bSingleMakeSelected) {
                    if (!isHeliOnlyFlag) {
                      if (!bSingleAirframeSelected) {
                        if (!bSingleMakeAirframeSelected) {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_FRAME] + "][" + localArray[zloop][LOCAIR_TYPE] + "][" + localArray[zloop][LOCAIR_MAKE_ABR] + "] - " + localArray[zloop][LOCAIR_MODEL];
                        }
                        else {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_TYPE] + "][" + localArray[zloop][LOCAIR_MAKE_ABR] + "] - " + localArray[zloop][LOCAIR_MODEL];
                        }
                      }
                      else {
                        if (!bSingleTypeSelected) {
                          if (!bSingleMakeTypeSelected) {
                            displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_TYPE] + "][" + localArray[zloop][LOCAIR_MAKE_ABR] + "] - " + localArray[zloop][LOCAIR_MODEL];
                          }
                          else {
                            displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_MAKE_ABR] + "] - " + localArray[zloop][LOCAIR_MODEL];
                          }
                        }
                        else {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_MAKE_ABR] + "] - " + localArray[zloop][LOCAIR_MODEL];
                        }
                      }
                    }
                    else {
                      if (!bSingleMakeTypeSelected) {
                        displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_MAKE_ABR] + "][" + localArray[zloop][LOCAIR_MAKE_ABR] + "] - " + localArray[zloop][LOCAIR_MODEL];
                      }
                      else {
                        displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_MAKE_ABR] + "] - " + localArray[zloop][LOCAIR_MODEL];
                      }
                    }
                  }
                  else {
                    if (!bSingleTypeSelected) {
                      if (!bSingleMakeTypeSelected) {
                        if ((!bSingleAirframeSelected) || (!bSingleMakeAirframeSelected)) {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_FRAME] + "][" + localArray[zloop][LOCAIR_TYPE] + "] - " + localArray[zloop][LOCAIR_MODEL];
                        }
                        else {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_TYPE] + "] - " + localArray[zloop][LOCAIR_MODEL];
                        }
                      }
                      else {
                        displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCAIR_MODEL];
                      }
                    }
                    else {
                      displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCAIR_MODEL];
                    }
                  }

                  if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                    if (optionArray.length > 0) {
                      if (inClientArrayJS(optionArray, localArray[zloop][LOCAIR_INDEX])) {
                        displayCboJS.options[nCurrentOption].selected = true;
                        displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                        bfoundSelection = true;
                      }
                    }
                  }
                }
                else {

                  displayCboJS.options[nCurrentOption] = new Option("");
                  displayCboJS.options[nCurrentOption].value = localArray[zloop][LOCAIR_INDEX];

                  if (!bSingleMakeSelected) {
                    if (!isHeliOnlyFlag) {
                      if (!bSingleAirframeSelected) {
                        if (!bSingleMakeAirframeSelected) {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_FRAME] + "][" + localArray[zloop][LOCAIR_TYPE] + "][" + localArray[zloop][LOCAIR_MAKE_ABR] + "] - " + localArray[zloop][LOCAIR_MODEL];
                        }
                        else {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_TYPE] + "][" + localArray[zloop][LOCAIR_MAKE_ABR] + "] - " + localArray[zloop][LOCAIR_MODEL];
                        }
                      }
                      else {
                        if (!bSingleTypeSelected) {
                          if (!bSingleMakeTypeSelected) {
                            displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_TYPE] + "][" + localArray[zloop][LOCAIR_MAKE_ABR] + "] - " + localArray[zloop][LOCAIR_MODEL];
                          }
                          else {
                            displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_MAKE_ABR] + "] - " + localArray[zloop][LOCAIR_MODEL];
                          }
                        }
                        else {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_MAKE_ABR] + "] - " + localArray[zloop][LOCAIR_MODEL];
                        }
                      }
                    }
                    else {
                      if (!bSingleMakeTypeSelected) {
                        displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_MAKE_ABR] + "][" + localArray[zloop][LOCAIR_MAKE_ABR] + "] - " + localArray[zloop][LOCAIR_MODEL];
                      }
                      else {
                        displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_MAKE_ABR] + "] - " + localArray[zloop][LOCAIR_MODEL];
                      }
                    }
                  }
                  else {
                    if (!bSingleTypeSelected) {
                      if (!bSingleMakeTypeSelected) {
                        if ((!bSingleAirframeSelected) || (!bSingleMakeAirframeSelected)) {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_FRAME] + "][" + localArray[zloop][LOCAIR_TYPE] + "] - " + localArray[zloop][LOCAIR_MODEL];
                        }
                        else {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCAIR_TYPE] + "] - " + localArray[zloop][LOCAIR_MODEL];
                        }
                      }
                      else {
                        displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCAIR_MODEL];
                      }
                    }
                    else {
                      displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCAIR_MODEL];
                    }
                  }

                  if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                    if (optionArray.length > 0) {
                      if (inClientArrayJS(optionArray, localArray[zloop][LOCAIR_INDEX])) {
                        displayCboJS.options[nCurrentOption].selected = true;
                        displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                        bfoundSelection = true;
                      }
                    }
                  }
                } // (nCurrentOption == 0)

                nCurrentOption = nCurrentOption + 1;
                nRememberModel = localArray[zloop][LOCAIR_MODEL_ID];

              } // (isModelInArray(makeArray, localArray[zloop][LOCAIR_INDEX], isFiltered))
            } // ((Number(localArray[zloop][LOCAIR_MODEL_ID]) != 0) && (Number(localArray[zloop][LOCAIR_MODEL_ID]) != Number(nRememberModel)))
          }
          else {
            break;
          } // ((localArray[zloop][LOCAIR_TYPE] == sAirframeMakeType) && (localArray[zloop][LOCAIR_FRAME] == sAirframeType) && (localArray[zloop][LOCAIR_MAKE] == sAirframeMake))
        } // ((typeof (localArray[zloop][LOCAIR_INDEX]) != "undefined") && (localArray[zloop][LOCAIR_INDEX] != null))
      } // zloop	
    } // xloop
  }
  else {
    displayCboJS.options[0].innerHTML = "All";
    displayCboJS.options[0].value = "All";
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
  } // (displayArray.length > 0)

  if (displayCboJS.options.length > 1) {
    displayCboJS = sortListBoxJS(displayCboJS);
  }
  else {
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
  }

  if ((!bfoundSelection) && (displayCboJS.options.length > 1)) {
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
  }
  else {
    if ((displayCboJS.options.length > 1) && (bfoundSelection)) {
      displayCboJS.options[0].selected = false;
    }
  }

  makeArray = null;

  aircraftTypeList = null;
  aircraftModelArray = null;

  displayArray = null;
  optionArray = null;
  optionList = null;
  localArray = null;
  airframeArray = null;

  return displayCboJS;

}

function fillMfrName(inCboMfrName, inSelected, sessionMfrName, inFilterString, isFiltered, selectedType, selectedMake) {

  var bfoundSelection = false;
  var sRememberMfrName = "";
  var nCurrentOption = 0;

  var displayCboJS = null;
  var optionArray = null;
  var localArray = null;
  var localAirFrameArray = null;

  var sFilter = "";
  var filterArray = null;

  var sSelectionStr = "";

  var bIsHelicopter = false;
  var bIsBusiness = false;
  var bIscommercial = false;
  var bIsMakeMfr = false;
  var bIsTypeMfr = false;

  var sMake = "";
  var makeArray = null;

  var sType = "";
  var tmpTypeArray = null;
  var tmpMfrString = "";
  var typeArray = null;

  if ((localMfrNamesArray != null) && (localMfrNamesArray.length > 0)) {
    localArray = localMfrNamesArray;
  }

  if (!isFiltered) {
    if ((localMasterAirframeArray != null) && (localMasterAirframeArray.length > 0)) {
      localAirFrameArray = localMasterAirframeArray;
    }
  }
  else {
    if ((localFilterAirframeArray != null) && (localFilterAirframeArray.length > 0)) {
      localAirFrameArray = localFilterAirframeArray;
    }
  }

  if ((inFilterString != null) && (inFilterString != "")) {

    var remove = /, /gi;
    sFilter = inFilterString.replace(remove, ",");
    filterArray = sFilter.split(",");

    //alert("inFilterString [" + sFilter + "][mfrname]");

  }

  if ((selectedMake != null) && (selectedMake != "")) {

    for (var nloop = 0; nloop < selectedMake.length; nloop++) {
      if (selectedMake.options[nloop].selected == true) {
        if (nloop > 0) {
          if (sMake == "") {
            sMake = localAirFrameArray[getIndexForItem(selectedMake.options[nloop].value, isFiltered)][LOCAIR_MFRNAME].toUpperCase();
          }
          else {
            sMake = sMake + "," + localAirFrameArray[getIndexForItem(selectedMake.options[nloop].value, isFiltered)][LOCAIR_MFRNAME].toUpperCase();
          }
        } // (nloop > 0)
      } // (optionList.options[nloop].selected == true)
    } // (nloop = 0; nloop < optionList.length; nloop++) 

    if (sMake != "") {
      makeArray = sMake.split(",");
    }

    //alert("selectedMake [" + sMake + "][mfrname]");

  }

  if ((selectedType != null) && (selectedType != "")) {

    for (var nloop = 0; nloop < selectedType.length; nloop++) {
      if (selectedType.options[nloop].selected == true) {
        if (nloop > 0) {
          if (sType == "") {
            sType = localAirFrameArray[getIndexForItem(selectedType.options[nloop].value, isFiltered)][LOCAIR_FRAME] + "|" + localAirFrameArray[getIndexForItem(selectedType.options[nloop].value, isFiltered)][LOCAIR_TYPE];
          }
          else {
            sType = sType + "," + localAirFrameArray[getIndexForItem(selectedType.options[nloop].value, isFiltered)][LOCAIR_FRAME] + "|" + localAirFrameArray[getIndexForItem(selectedType.options[nloop].value, isFiltered)][LOCAIR_TYPE];
          }
        } // (nloop > 0)
      } // (optionList.options[nloop].selected == true)
    } // (nloop = 0; nloop < optionList.length; nloop++)

    if (sType != "") {
      tmpTypeArray = sType.split(",");

      // now find all the "mfr names" for these types ...

      for (var mloop = 0; mloop < tmpTypeArray.length; mloop++) {

        var tmpArray = tmpTypeArray[mloop].split("|");

        var sAirframe = tmpArray[0];
        var sAirtype = tmpArray[1];

        for (var xloop = 0; xloop < localAirFrameArray.length; xloop++) {

          if ((sAirframe == localAirFrameArray[xloop][LOCAIR_FRAME]) && (sAirtype == localAirFrameArray[xloop][LOCAIR_TYPE])) {

            if (localAirFrameArray[xloop][LOCAIR_MFRNAME].toUpperCase() != sRememberMfrName) {

              if (tmpMfrString == "") {
                tmpMfrString = localAirFrameArray[xloop][LOCAIR_MFRNAME].toUpperCase();
              }
              else {
                tmpMfrString = tmpMfrString + "," + localAirFrameArray[xloop][LOCAIR_MFRNAME].toUpperCase();
              }

              sRememberMfrName = localAirFrameArray[xloop][LOCAIR_MFRNAME].toUpperCase();

            }

          }

        }

      }

      if (tmpMfrString != "") {
        typeArray = tmpMfrString.split(",");
      }
    }

    //alert("selectedType [" + sType + "] tmpMfrString [" + tmpMfrString + "][mfrname]");

  }

  // get currently selected items		
  if ((typeof (inSelected.name) != "undefined") && (inSelected != null)) {

    for (var nloop = 0; nloop < inSelected.length; nloop++) {
      if (inSelected.options[nloop].selected == true) {
        if (nloop == 0) {
          sSelectionStr = "All";
        }
        else {

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
      if (sSelectionStr.toUpperCase() != "ALL") {
        sessionMfrName = sSelectionStr;
      }
      else {
        if ((sessionMfrName != null) && (sessionMfrName != "")) {
          var remove = /##/gi;
          sSelectionStr = sessionMfrName.replace(remove, ",");

          remove = /, /gi;
          sSelectionStr = sSelectionStr.replace(remove, ",");
        }
      }

      optionArray = sSelectionStr.split(",");
    }
    else {
      if ((sessionMfrName != null) && (sessionMfrName != "")) {
        var remove = /##/gi;
        sSelectionStr = sessionMfrName.replace(remove, ",");

        remove = /, /gi;
        sSelectionStr = sSelectionStr.replace(remove, ",");

        optionArray = sSelectionStr.split(",");
      }
    }

  } // ((typeof(inSelected.name) != "undefined") && (inSelected != ""))

  //alert("sessionMfrName [" + sessionMfrName + "] filter[" + isFiltered + "][mfrname]");

  displayCboJS = inCboMfrName;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  sRememberMfrName = "";

  for (var iloop = 0; iloop < localArray.length; iloop++) {

    if ((typeof (localArray[iloop][LOCMFR_NAME]) != "undefined") && (localArray[iloop][LOCMFR_NAME] != null)) {

      if (localArray[iloop][LOCMFR_NAME].toUpperCase() != sRememberMfrName) {

        //alert("sRememberMfrName [" + sRememberMfrName + "] filterArray [" + filterArray + "] makeArray [" + makeArray + "] typeArray [" + typeArray + "][mfrname]");

        if (makeArray != null) {
          if (inClientArrayJS(makeArray, localArray[iloop][LOCMFR_NAME].toUpperCase())) {
            bIsMakeMfr = true;
          }
        }
        else {

          if (typeArray != null) {
            if (inClientArrayJS(typeArray, localArray[iloop][LOCMFR_NAME].toUpperCase())) {
              bIsTypeMfr = true;
            }
          }
          else {

            if (filterArray != null) {

              if (inClientArrayJS(filterArray, localArray[iloop][LOCMFR_HEL])) {
                bIsHelicopter = true;
              }

              if (inClientArrayJS(filterArray, localArray[iloop][LOCMFR_BUS])) {
                bIsBusiness = true;
              }

              if (inClientArrayJS(filterArray, localArray[iloop][LOCMFR_COM])) {
                bIscommercial = true;
              }

            }
            else {

              bIsHelicopter = true;
              bIsBusiness = true;
              bIscommercial = true;

            } // if (filterArray != null)

          } // if (typeArray != null)

        } // if (makeArray != null)

        //alert("bIsHelicopter [" + bIsHelicopter + "] bIsBusiness [" + bIsBusiness + "] bIscommercial [" + bIscommercial + "] bIsMakeMfr [" + bIsMakeMfr + "] bIsTypeMfr [" + bIsTypeMfr + "][mfrname]");

        if (bIsHelicopter || bIsBusiness || bIscommercial || bIsMakeMfr || bIsTypeMfr) {

          //alert("ADD mfrName");

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
            displayCboJS.options[nCurrentOption] = new Option("");
            displayCboJS.options[nCurrentOption].value = localArray[iloop][LOCMFR_NAME];
            displayCboJS.options[nCurrentOption].innerHTML = localArray[iloop][LOCMFR_NAME].toUpperCase();

            if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
              if (optionArray.length > 0) {
                if (inClientArrayJS(optionArray, localArray[iloop][LOCMFR_NAME])) {
                  displayCboJS.options[nCurrentOption].selected = true;
                  displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                  bfoundSelection = true;
                }
              }
            }

          }
          else {

            displayCboJS.options[nCurrentOption] = new Option("");
            displayCboJS.options[nCurrentOption].value = localArray[iloop][LOCMFR_NAME];
            displayCboJS.options[nCurrentOption].innerHTML = localArray[iloop][LOCMFR_NAME].toUpperCase();

            if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
              if (optionArray.length > 0) {
                if (inClientArrayJS(optionArray, localArray[iloop][LOCMFR_NAME])) {
                  displayCboJS.options[nCurrentOption].selected = true;
                  displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                  bfoundSelection = true;
                }
              }
            }
          } // nCurrentOption == 0

          nCurrentOption = nCurrentOption + 1;

        }

        sRememberMfrName = localArray[iloop][LOCMFR_NAME].toUpperCase();

      } // (localArray[iloop][LOCMFR_NAME] != sRememberMfrName)

      bIsHelicopter = false;
      bIsBusiness = false;
      bIscommercial = false;
      bIsMakeMfr = false;
      bIsTypeMfr = false;

    } // ((typeof (localArray[iloop][LOCMFR_NAME]) != "undefined") && (localArray[iloop][LOCMFR_NAME] != null))

  } // (iloop = 0; iloop < localArray.length; iloop++)


  if ((!bfoundSelection) && (displayCboJS.options.length > 1)) {
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
  }

  localArray = null;
  optionArray = null;

  return displayCboJS;

}

function fillAcSize(inCboAcSize, inSelected, sessionAcSize, inFilterString, isFiltered, selectedType, selectedMake) {

  var bfoundSelection = false;
  var sRememberSize = "";
  var nCurrentOption = 0;

  var displayCboJS = null;
  var optionArray = null;
  var localArray = null;
  var localAirFrameArray = null;

  var sFilter = "";
  var filterArray = null;

  var sSelectionStr = "";

  var bIsHelicopter = false;
  var bIsBusiness = false;
  var bIscommercial = false;
  var bIsMakeAcSize = false;
  var bIsTypeAcSize = false;

  var sMake = "";
  var tmpMakeArray = null;
  var makeArray = null;

  var sType = "";
  var tmpTypeArray = null;
  var typeArray = null;

  if ((localAircraftSizeArray != null) && (localAircraftSizeArray.length > 0)) {
    localArray = localAircraftSizeArray;
  }

  if (!isFiltered) {
    if ((localMasterAirframeArray != null) && (localMasterAirframeArray.length > 0)) {
      localAirFrameArray = localMasterAirframeArray;
    }
  }
  else {
    if ((localFilterAirframeArray != null) && (localFilterAirframeArray.length > 0)) {
      localAirFrameArray = localFilterAirframeArray;
    }
  }

  if ((inFilterString != null) && (inFilterString != "")) {

    var remove = /, /gi;
    sFilter = inFilterString.replace(remove, ",");
    filterArray = sFilter.split(",");

    //alert("inFilterString [" + sFilter + "][acsize]");

  }

  if ((selectedMake != null) && (selectedMake != "")) {

    for (var nloop = 0; nloop < selectedMake.length; nloop++) {
      if (selectedMake.options[nloop].selected == true) {
        if (nloop > 0) {
          if (sMake == "") {
            sMake = localAirFrameArray[getIndexForItem(selectedMake.options[nloop].value, isFiltered)][LOCAIR_MAKE].toUpperCase() + "|" + localAirFrameArray[getIndexForItem(selectedMake.options[nloop].value, isFiltered)][LOCAIR_FRAME] + "|" + localAirFrameArray[getIndexForItem(selectedMake.options[nloop].value, isFiltered)][LOCAIR_TYPE];
          }
          else {
            sMake = sMake + "," + localAirFrameArray[getIndexForItem(selectedMake.options[nloop].value, isFiltered)][LOCAIR_MAKE].toUpperCase() + "|" + localAirFrameArray[getIndexForItem(selectedMake.options[nloop].value, isFiltered)][LOCAIR_FRAME] + "|" + localAirFrameArray[getIndexForItem(selectedMake.options[nloop].value, isFiltered)][LOCAIR_TYPE];
          }
        } // (nloop > 0)
      } // (selectedMake.options[nloop].selected == true)
    } // (nloop = 0; nloop < selectedMake.length; nloop++) 

    if (sMake != "") {
      tmpMakeArray = sMake.split(",");

      // now find all the "ac sizes" for these makes ...
      var sMake = "";
      var sAirframe = "";
      var sAirtype = "";
      var tmpAcSizeString = "";
      sRememberSize = "";

      for (var mloop = 0; mloop < tmpMakeArray.length; mloop++) {

        var tmpArray = tmpMakeArray[mloop].split("|");

        sMake = tmpArray[0];
        sAirframe = tmpArray[1];
        sAirtype = tmpArray[2];

        for (var xloop = 0; xloop < localAirFrameArray.length; xloop++) {

          if ((sMake == localAirFrameArray[xloop][LOCAIR_MAKE]) && (sAirframe == localAirFrameArray[xloop][LOCAIR_FRAME]) && (sAirtype == localAirFrameArray[xloop][LOCAIR_TYPE])) {

            if (localAirFrameArray[xloop][LOCAIR_SIZE].toUpperCase() != sRememberSize) {

              if (tmpAcSizeString == "") {
                tmpAcSizeString = localAirFrameArray[xloop][LOCAIR_SIZE].toUpperCase();
              }
              else {
                //if (tmpAcSizeString.indexOf(localAirFrameArray[xloop][LOCAIR_SIZE].toUpperCase()) == -1) {
                tmpAcSizeString = tmpAcSizeString + "," + localAirFrameArray[xloop][LOCAIR_SIZE].toUpperCase();
                //}
              }

              sRememberSize = localAirFrameArray[xloop][LOCAIR_SIZE].toUpperCase();

            }

          }

        }

      }

      if (tmpAcSizeString != "") {
        makeArray = tmpAcSizeString.split(",");
      }

    }

    //alert("selectedMake [" + sMake + "] tmpSizeString [" + tmpAcSizeString + "][acsize]");

  }

  if ((selectedType != null) && (selectedType != "")) {

    for (var nloop = 0; nloop < selectedType.length; nloop++) {
      if (selectedType.options[nloop].selected == true) {
        if (nloop > 0) {
          if (sType == "") {
            sType = localAirFrameArray[getIndexForItem(selectedType.options[nloop].value, isFiltered)][LOCAIR_FRAME] + "|" + localAirFrameArray[getIndexForItem(selectedType.options[nloop].value, isFiltered)][LOCAIR_TYPE];
          }
          else {
            sType = sType + "," + localAirFrameArray[getIndexForItem(selectedType.options[nloop].value, isFiltered)][LOCAIR_FRAME] + "|" + localAirFrameArray[getIndexForItem(selectedType.options[nloop].value, isFiltered)][LOCAIR_TYPE];
          }
        } // (nloop > 0)
      } // (selectedType.options[nloop].selected == true)
    } // (nloop = 0; nloop < selectedType.length; nloop++)

    if (sType != "") {
      tmpTypeArray = sType.split(",");

      // now find all the "ac sizes" for these types ...
      var sAirframe = "";
      var sAirtype = "";
      var tmpAcSizeString = "";
      sRememberSize = "";

      for (var mloop = 0; mloop < tmpTypeArray.length; mloop++) {

        var tmpArray = tmpTypeArray[mloop].split("|");

        sAirframe = tmpArray[0];
        sAirtype = tmpArray[1];

        for (var xloop = 0; xloop < localAirFrameArray.length; xloop++) {

          if ((sAirframe == localAirFrameArray[xloop][LOCAIR_FRAME]) && (sAirtype == localAirFrameArray[xloop][LOCAIR_TYPE])) {

            if (localAirFrameArray[xloop][LOCAIR_SIZE].toUpperCase() != sRememberSize) {

              if (tmpAcSizeString == "") {
                tmpAcSizeString = localAirFrameArray[xloop][LOCAIR_SIZE].toUpperCase();
              }
              else {
                //if (tmpAcSizeString.indexOf(localAirFrameArray[xloop][LOCAIR_SIZE].toUpperCase()) == -1) {
                tmpAcSizeString = tmpAcSizeString + "," + localAirFrameArray[xloop][LOCAIR_SIZE].toUpperCase();
                //}
              }

              sRememberSize = localAirFrameArray[xloop][LOCAIR_SIZE].toUpperCase();

            }

          }

        }

      }

      if (tmpAcSizeString != "") {
        typeArray = tmpAcSizeString.split(",");
      }
    }

    //alert("selectedType [" + sType + "] tmpSizeString [" + tmpAcSizeString + "][acsize]");

  }

  // get currently selected items		
  if ((typeof (inSelected.name) != "undefined") && (inSelected != null)) {

    for (var nloop = 0; nloop < inSelected.length; nloop++) {
      if (inSelected.options[nloop].selected == true) {
        if (nloop == 0) {
          sSelectionStr = "All";
        }
        else {

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
      if (sSelectionStr.toUpperCase() != "ALL") {
        sessionAcSize = sSelectionStr;
      }
      else {
        if ((sessionAcSize != null) && (sessionAcSize != "")) {

          var remove = /##/gi;
          sSelectionStr = sessionAcSize.replace(remove, ",");

          remove = /, /gi;
          sSelectionStr = sSelectionStr.replace(remove, ",");
        }
      }

      optionArray = sSelectionStr.split(",");
    }
    else {
      if ((sessionAcSize != null) && (sessionAcSize != "")) {

        var remove = /##/gi;
        sSelectionStr = sessionAcSize.replace(remove, ",");

        remove = /, /gi;
        sSelectionStr = sSelectionStr.replace(remove, ",");

        optionArray = sSelectionStr.split(",");
      }
    }

  } // ((typeof(inSelected.name) != "undefined") && (inSelected != ""))

  //alert("sessionAcSize [" + sessionAcSize + "] filter[" + isFiltered + "][acsize]");

  displayCboJS = inCboAcSize;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  sRememberSize = "";

  for (var iloop = 0; iloop < localArray.length; iloop++) {

    if ((typeof (localArray[iloop][LOCACSIZE_CODE]) != "undefined") && (localArray[iloop][LOCACSIZE_CODE] != null)) {
      if (localArray[iloop][LOCACSIZE_CODE] != sRememberSize) {

        //alert("sRememberSize [" + sRememberSize + "] filterArray [" + filterArray + "] makeArray [" + makeArray + "] typeArray [" + typeArray + "][acsize]");

        if (makeArray != null) {
          if (inClientArrayJS(makeArray, localArray[iloop][LOCACSIZE_CODE].toUpperCase())) {
            bIsMakeAcSize = true;
          }
        }
        else {

          if (typeArray != null) {
            if (inClientArrayJS(typeArray, localArray[iloop][LOCACSIZE_CODE].toUpperCase())) {
              bIsTypeAcSize = true;
            }
          }
          else {

            if (filterArray != null) {

              if (inClientArrayJS(filterArray, localArray[iloop][LOCACSIZE_HEL])) {
                bIsHelicopter = true;
              }

              if (inClientArrayJS(filterArray, localArray[iloop][LOCACSIZE_BUS])) {
                bIsBusiness = true;
              }

              if (inClientArrayJS(filterArray, localArray[iloop][LOCACSIZE_COM])) {
                bIscommercial = true;
              }

            }
            else {

              bIsHelicopter = true;
              bIsBusiness = true;
              bIscommercial = true;

            } // if (filterArray != null)

          } // if (typeArray != null)

        } // if (makeArray != null)

        //alert("bIsHelicopter [" + bIsHelicopter + "] bIsBusiness [" + bIsBusiness + "] bIscommercial [" + bIscommercial + "] bIsMakeAcSize [" + bIsMakeAcSize + "] bIsTypeAcSize [" + bIsTypeAcSize + "][acsize]");

        if (bIsHelicopter || bIsBusiness || bIscommercial || bIsMakeAcSize || bIsTypeAcSize) {

          //alert("ADD acSize");

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
            displayCboJS.options[nCurrentOption] = new Option("");
            displayCboJS.options[nCurrentOption].value = localArray[iloop][LOCACSIZE_CODE];
            displayCboJS.options[nCurrentOption].innerHTML = localArray[iloop][LOCACSIZE_NAME];

            if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
              if (optionArray.length > 0) {
                if (inClientArrayJS(optionArray, localArray[iloop][LOCACSIZE_CODE])) {
                  displayCboJS.options[nCurrentOption].selected = true;
                  displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                  bfoundSelection = true;
                }
              }
            }
          }
          else {

            displayCboJS.options[nCurrentOption] = new Option("");
            displayCboJS.options[nCurrentOption].value = localArray[iloop][LOCACSIZE_CODE];
            displayCboJS.options[nCurrentOption].innerHTML = localArray[iloop][LOCACSIZE_NAME];

            if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
              if (optionArray.length > 0) {
                if (inClientArrayJS(optionArray, localArray[iloop][LOCACSIZE_CODE])) {
                  displayCboJS.options[nCurrentOption].selected = true;
                  displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                  bfoundSelection = true;
                }
              }
            }
          } // nCurrentOption == 0

          nCurrentOption = nCurrentOption + 1;
        }

        sRememberSize = localArray[iloop][LOCACSIZE_CODE];

      } // (localArray[iloop][LOCACSIZE_NAME] != sRememberSize)

      bIsHelicopter = false;
      bIsBusiness = false;
      bIscommercial = false;
      bIsMakeAcSize = false;
      bIsTypeAcSize = false;

    } // ((typeof (localArray[iloop][LOCACSIZE_CODE]) != "undefined") && (localArray[iloop][LOCACSIZE_CODE] != null))

  } // (iloop = 0; iloop < localArray.length; iloop++)


  if ((!bfoundSelection) && (displayCboJS.options.length > 1)) {
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
  }

  localArray = null;
  optionArray = null;

  return displayCboJS;

}

