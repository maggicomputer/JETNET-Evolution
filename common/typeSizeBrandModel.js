
// ********************************************************************************
// Copyright 2004-11. JETNET,LLC. All rights reserved.
//
// $$Archive: /commonWebProject/common/typeSizeBrandModel.js $
// $$Author: Mike $
// $$Date: 6/19/19 8:45a $
// $$Modtime: 6/18/19 6:12p $
// $$Revision: 2 $
// $$Workfile: typeSizeBrandModel.js $
//
// ********************************************************************************

function isModelInArray(n_inModelArray, n_inItemMake) {

  var localArray = null;
  var bFound = false;

  var modelIndex = 0;

  if ((localMasterYachtArray != null) && (localMasterYachtArray.length > 0)) {
    localArray = localMasterYachtArray;
  }

  if ((n_inModelArray != null) && (n_inModelArray != "")) {
    if (n_inModelArray[0].toUpperCase() == "ALL") {
      bFound = true;
    }
    else {
      for (var xloop = 0; xloop < n_inModelArray.length; xloop++) {
        modelIndex = n_inModelArray[xloop];
        if (localArray[modelIndex][LOCYACHT_BRAND] == localArray[n_inItemMake][LOCYACHT_BRAND]) {
          bFound = true;
          break;
        }
      }
    }
  }

  localArray = null;
  return bFound;

}

function getYachtLabelClient(inMotorType, inYachtCategory, bSingleMotorTypeSelected) {
  var sYachtLabel;

  sYachtLabel = "";

  for (i = 0; i < localYachtLableArray.length; i++) {

    if (localYachtLableArray[i][YACHT_LABEL_MOTOR] == inMotorType) {
      // motor type matched

      if (inYachtCategory != "") {

        if (localYachtLableArray[i][YACHT_LABEL_CATEGORY] == inYachtCategory) {
          // category matched

          if (bSingleMotorTypeSelected) {
            if (localYachtLableArray[i][YACHT_LABEL_CODE].indexOf(localYachtLableArray[i][YACHT_LABEL_CATEGORY]) == 0) {
              sYachtLabel = localYachtLableArray[i][YACHT_LABEL_NAME];
              break;
            }
          }
          else {

            if (localYachtLableArray[i][YACHT_LABEL_CODE].indexOf(localYachtLableArray[i][YACHT_LABEL_MOTOR]) == 0) {

              if (localYachtLableArray[i][YACHT_LABEL_CODE].length == 2) {
                if (localYachtLableArray[i][YACHT_LABEL_CODE].indexOf(localYachtLableArray[i][YACHT_LABEL_CATEGORY]) >= 0) {
                  sYachtLabel = localYachtLableArray[i][YACHT_LABEL_NAME];
                  break;
                }
              }
            }

          }

        } // if

      }
      else {

        switch (localYachtLableArray[i][YACHT_LABEL_MOTOR].toUpperCase()) {
          case "S":
            {
              sYachtLabel = "Sailing";
              break;
            }

          case "M":
            {
              sYachtLabel = "Motor";
              break;
            }
        }

        break;
      }

    } // if 

  } // for

  return sYachtLabel;

} // GetYachtLabelClient

function fillYachtType(inCboType, inSelected, sessionType) {

  var bSelectedItem = false;
  var bfoundSelection = false;
  var sRememberMotor = "";
  var nCurrentOption = 0;

  var displayCboJS = null;
  var optionArray = null;
  var localArray = null;

  var sSelectionStr = "";

  if ((localMasterYachtArray != null) && (localMasterYachtArray.length > 0)) {
    localArray = localMasterYachtArray;
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

  displayCboJS = inCboType;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  for (var iloop = 0; iloop < localArray.length; iloop++) {

    if ((typeof (localArray[iloop][LOCYACHT_MOTOR]) != "undefined") && (localArray[iloop][LOCYACHT_MOTOR] != null)) {
      if (localArray[iloop][LOCYACHT_MOTOR] != sRememberMotor) {
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
          displayCboJS.options[nCurrentOption] = new Option(localArray[iloop][LOCYACHT_INDEX]);
          displayCboJS.options[nCurrentOption].value = localArray[iloop][LOCYACHT_INDEX];
          displayCboJS.options[nCurrentOption].innerHTML = getYachtLabelClient(localArray[iloop][LOCYACHT_MOTOR], "", true);

          if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
            if (optionArray.length > 0) {
              if (inClientArrayJS(optionArray, localArray[iloop][LOCYACHT_INDEX])) {
                displayCboJS.options[nCurrentOption].selected = true;
                displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                bfoundSelection = true;
              }
            }
          }
        }
        else {

          displayCboJS.options[nCurrentOption] = new Option(localArray[iloop][LOCYACHT_INDEX]);
          displayCboJS.options[nCurrentOption].value = localArray[iloop][LOCYACHT_INDEX];
          displayCboJS.options[nCurrentOption].innerHTML = getYachtLabelClient(localArray[iloop][LOCYACHT_MOTOR], "", true);

          if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
            if (optionArray.length > 0) {
              if (inClientArrayJS(optionArray, localArray[iloop][LOCYACHT_INDEX])) {
                displayCboJS.options[nCurrentOption].selected = true;
                displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                bfoundSelection = true;
              }
            }
          }
        } // nCurrentOption == 0

        nCurrentOption = nCurrentOption + 1;
        sRememberMotor = localArray[iloop][LOCYACHT_MOTOR];

      } // (localArray[iloop][LOCYACHT_MOTOR] != sRememberMotor)
    } // ((typeof (localArray[iloop][LOCYACHT_MOTOR]) != "undefined") && (localArray[iloop][LOCYACHT_MOTOR] != null))

  } // (iloop = 0; iloop < localArray.length; iloop++)


  if ((!bfoundSelection) && (displayCboJS.options.length > 1)) {
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
  }

  return displayCboJS;

  localArray = null;
  optionArray = null;

}

function fillYachtSize(inSizeCbo, inSelMotor, inSelected, sessionSize) {

  var bfoundSelection = false;

  var sTempSizeIndex = 0;
  var sSize = "";
  var sMotor = "";

  var sRememberSize = "";
  var bSingleMotorTypeSelected = false;

  var sCboMotor = "";

  var bAllMotor = false;
  var nCurrentOption = 0;
  var bSelectedItem = false;

  var selectedsize = "";

  var displayCboJS = null;
  var displayArray = null; // array of items to display in dropdown
  var optionArray = null; // list of selected items
  var localArray = null;
  var motorArray = null;

  var sSelectionStr = "";

  if ((localMasterYachtArray != null) && (localMasterYachtArray.length > 0)) {
    localArray = localMasterYachtArray;
  }

  // get the list of selected types
  if ((typeof (inSelMotor.name) != "undefined") && (inSelMotor != null)) {
    for (var nloop = 0; nloop < inSelMotor.length; nloop++) {
      if ((inSelMotor.options[nloop].selected == true) || (bAllMotor == true)) {
        if (nloop == 0) {
          bAllMotor = true;
        }
        else {
          if (sCboMotor == "") {
            sCboMotor = inSelMotor.options[nloop].value;
          }
          else {
            sCboMotor = sCboMotor + "," + inSelMotor.options[nloop].value;
          }
        } // (nloop == 0)
      } // ((inSelMotor.options[nloop].selected == true) || (bAllMotor == true))
    } // (nloop = 0; nloop < inSelMotor.length; nloop++)
  }
  else {
    bAllMotor = true;
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
      if ((!bAllMotor) && bSelectedItem) {
        if (sSelectionStr.toUpperCase() != "ALL") {
          sessionSize = sSelectionStr;
        }
        optionArray = sSelectionStr.split(",");
      }
      else {
        optionArray = ("All").split(",");
      } // (!bAllCategory && bSelectedItem)    
    } // (sSelectionStr != "")

    if ((sessionSize != null) && (sessionSize != "")) {
      bSelectedItem = true;
      var remove = /, /gi;
      sSelectionStr = sessionSize.replace(remove, ",");
      optionArray = sSelectionStr.split(",");

    } // (sessionSize != "") 

  } // ((typeof(inSelected.name) != "undefined") && (inSelected != ""))

  if ((sCboMotor != "") && (!bSelectedItem)) {
    displayArray = sCboMotor.split(",");
  }
  else {
    if (bSelectedItem) {
      displayArray = sCboMotor.split(",");
    }
    else {
      displayArray = sCboMotor.split(",");
      optionArray = ("All").split(",");
      sSelectionStr = "";
    }
  }

  displayCboJS = inSizeCbo;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  if ((displayArray.length == 1) && (!bAllMotor)) {
    bSingleMotorTypeSelected = true;
  }
  
  if (displayArray.length > 0) {

    for (var xloop = 0; xloop < displayArray.length; xloop++) {
      if (displayArray[xloop].toUpperCase() != "ALL") {

        sTempSizeIndex = displayArray[xloop];
        
        sSize = localArray[Number(sTempSizeIndex)][LOCYACHT_CATEGORY];
        sMotor = localArray[Number(sTempSizeIndex)][LOCYACHT_MOTOR];

      }

      if (bAllMotor && ((sessionSize != null) && (sessionSize != ""))) {
        sTempSizeIndex = "0"
      }

      for (var zloop = Number(sTempSizeIndex); zloop < localArray.length; zloop++) {
        if ((typeof (localArray[zloop][LOCYACHT_INDEX]) != "undefined") && (localArray[zloop][LOCYACHT_INDEX] != null)) {
          
          if (localArray[zloop][LOCYACHT_MOTOR] == sMotor) {

            if ((typeof (localArray[zloop][LOCYACHT_CATEGORY]) != "undefined") && (localArray[zloop][LOCYACHT_CATEGORY] != null) && (!inClientArrayJS(sRememberSize.split(","), localArray[zloop][LOCYACHT_CATEGORY]))) {
              
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
                displayCboJS.options[nCurrentOption] = new Option(localArray[zloop][LOCYACHT_INDEX]);
                displayCboJS.options[nCurrentOption].value = localArray[zloop][LOCYACHT_INDEX];

                displayCboJS.options[nCurrentOption].innerHTML = getYachtLabelClient(localArray[zloop][LOCYACHT_MOTOR], localArray[zloop][LOCYACHT_CATEGORY], true);

                if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                  if (optionArray.length > 0) {
                    if (inClientArrayJS(optionArray, localArray[zloop][LOCYACHT_INDEX])) {
                      displayCboJS.options[nCurrentOption].selected = true;
                      displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                      bfoundSelection = true;
                    }
                  }
                }
              }
              else {

                displayCboJS.options[nCurrentOption] = new Option(localArray[zloop][LOCYACHT_INDEX]);
                displayCboJS.options[nCurrentOption].value = localArray[zloop][LOCYACHT_INDEX];

                displayCboJS.options[nCurrentOption].innerHTML = getYachtLabelClient(localArray[zloop][LOCYACHT_MOTOR], localArray[zloop][LOCYACHT_CATEGORY], true);

                if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                  if (optionArray.length > 0) {
                    if (inClientArrayJS(optionArray, localArray[zloop][LOCYACHT_INDEX])) {
                      displayCboJS.options[nCurrentOption].selected = true;
                      displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                      bfoundSelection = true;
                    }
                  }
                }
              } // (nCurrentOption == 0)

              nCurrentOption = nCurrentOption + 1;


              if (sRememberSize == "") {
                sRememberSize = localArray[zloop][LOCYACHT_CATEGORY];
              }
              else {
                sRememberSize = sRememberSize + "," + localArray[zloop][LOCYACHT_CATEGORY];
              }
           
            } // ((localArray[zloop][LOCYACHT_CATEGORY] != "") && (localArray[zloop][LOCYACHT_CATEGORY] != sRememberSize))
          }  // (localArray[zloop][LOCYACHT_MOTOR] == sMotor)
        } // ((localArray[zloop][LOCYACHT_CATEGORY] != null) && (localArray[zloop][LOCYACHT_CATEGORY] != ""))
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

  //if (displayCboJS.options.length > 1) {
  //  displayCboJS = sortListBoxJS(displayCboJS);
  //}

  if ((!bfoundSelection) && (displayCboJS.options.length > 1)) {
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
  }
  else {
    if ((displayCboJS.options.length > 1) && (bfoundSelection)) {
      displayCboJS.options[0].selected = false;
    }
  }

  displayArray = null;
  optionArray = null;
  localArray = null;
  motorArray = null;

  return displayCboJS;

}

function fillYachtBrand(inBrandCbo, inSelMotor, inSelSize, inSelected, sessionBrand) {

  var bfoundSelection = false;

  var sTempBrandIndex = 0;
  var sSizeBrand = "";

  var sRememberBrand = "";
  
  var sCboSize = "";
  var bAllSize = false;
  
  var sCboMotor = "";
  var bAllMotor = false;

  var nCurrentOption = 0;
  var bSelectedItem = false;

  var displayCboJS = null;
  var displayArray = null; // array of items to display in dropdown
  var optionArray = null; // list of selected items
  var localArray = null;
  var motorArray = null;

  var sSelectionStr = "";

  if ((localMasterYachtArray != null) && (localMasterYachtArray.length > 0)) {
    localArray = localMasterYachtArray;
  }

  // get the list of selected types
  if ((typeof (inSelMotor.name) != "undefined") && (inSelMotor != null)) {
    for (var nloop = 0; nloop < inSelMotor.length; nloop++) {
      if ((inSelMotor.options[nloop].selected == true) || (bAllMotor == true)) {
        if (nloop == 0) {
          bAllMotor = true;
        }
        else {
          if (sCboMotor == "") {
            sCboMotor = localArray[Number(inSelMotor.options[nloop].value)][LOCYACHT_MOTOR];
          }
          else {
            sCboMotor = sCboMotor + "," + localArray[Number(inSelMotor.options[nloop].value)][LOCYACHT_MOTOR];
          }
        } // (nloop == 0)
      } // ((inSelMotor.options[nloop].selected == true) || (bAllMotor == true))
    } // (nloop = 0; nloop < inSelMotor.length; nloop++)
  }
  else {
    bAllMotor = true;
  }

  // get the list of selected sizes
  if ((typeof (inSelSize.name) != "undefined") && (inSelSize != null)) {
    for (var nloop = 0; nloop < inSelSize.length; nloop++) {
      if ((inSelSize.options[nloop].selected == true) || (bAllSize == true)) {
        if (nloop == 0) {
          bAllSize = true;
        }
        else {
          if (sCboSize == "") {
            sCboSize = inSelSize.options[nloop].value;
          }
          else {
            sCboSize = sCboSize + "," + inSelSize.options[nloop].value;
          }
        } // (nloop == 0)
      } // ((inSelSize.options[nloop].selected == true) || (bAllSize == true))
    } // (nloop = 0; nloop < inSelSize.length; nloop++)
  }
  else {
    bAllSize = true;
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
      if ((!bAllMotor) && (!bAllSize) && bSelectedItem) {
        if (sSelectionStr.toUpperCase() != "ALL") {
          sessionBrand = sSelectionStr;              // set sessionbrand = to selection string
        }
        optionArray = sSelectionStr.split(",");
      }
      else {
        optionArray = ("All").split(",");
      } // ((!bAllMotor) && (!bAllSize) && bSelectedItem)    
    } // (sSelectionStr != "")

    if ((sessionBrand != null) && (sessionBrand != "")) {
      bSelectedItem = true;
      var remove = /, /gi;
      sSelectionStr = sessionBrand.replace(remove, ",");
      optionArray = sSelectionStr.split(",");

    } // if ((sessionBrand != null) && (sessionBrand != ""))

  } // ((typeof(inSelected.name) != "undefined") && (inSelected != ""))


  if ((sCboSize != "") && (!bSelectedItem)) {
    displayArray = sCboSize.split(",");
  }
  else {
    if (bSelectedItem) {
      displayArray = sCboSize.split(",");
    }
    else {
      displayArray = sCboSize.split(",");
      optionArray = ("All").split(",");
      sSelectionStr = "";
    }
  }

  displayCboJS = inBrandCbo;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  if (displayArray.length > 0) {

    for (var xloop = 0; xloop < displayArray.length; xloop++) {
      if (displayArray[xloop].toUpperCase() != "ALL") {

        sTempBrandIndex = displayArray[xloop];
        sSizeBrand = localArray[Number(sTempBrandIndex)][LOCYACHT_CATEGORY];
               
      }

      if (bAllMotor && bAllSize && ((sessionBrand != null) && (sessionBrand != ""))) {
        sTempBrandIndex = "0"
      }

      for (var zloop = Number(sTempBrandIndex); zloop < localArray.length; zloop++) {
        if ((typeof (localArray[zloop][LOCYACHT_INDEX]) != "undefined") && (localArray[zloop][LOCYACHT_INDEX] != null)) {
          
          if (localArray[zloop][LOCYACHT_CATEGORY] == sSizeBrand ){ 
            
            if (((typeof (localArray[zloop][LOCYACHT_BRAND]) != "undefined") && (localArray[zloop][LOCYACHT_BRAND] != null)) && (!inClientArrayJS(sRememberBrand.split(","), localArray[zloop][LOCYACHT_BRAND]))) {
              
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
                } // ((sSelectionStr != "") && (sSelectionStr.toUpperCase() == "ALL"))


                if (!bAllMotor && (inClientArrayJS(sCboMotor.split(","), localArray[zloop][LOCYACHT_MOTOR]))) { // only show brands that match the motor type - pick up the first option
                  nCurrentOption = nCurrentOption + 1;
                  displayCboJS.options[nCurrentOption] = new Option(localArray[zloop][LOCYACHT_INDEX]);
                  displayCboJS.options[nCurrentOption].value = localArray[zloop][LOCYACHT_INDEX];

                  displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCYACHT_BRAND];

                  if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                    if (optionArray.length > 0) {
                      if (inClientArrayJS(optionArray, localArray[zloop][LOCYACHT_INDEX])) {
                        displayCboJS.options[nCurrentOption].selected = true;
                        displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                        bfoundSelection = true;
                      }
                    }
                  }
              

                }
                else { // show the brand - pick up the first option

                  if (bAllMotor) {

                    nCurrentOption = nCurrentOption + 1;
                    displayCboJS.options[nCurrentOption] = new Option(localArray[zloop][LOCYACHT_INDEX]);
                    displayCboJS.options[nCurrentOption].value = localArray[zloop][LOCYACHT_INDEX];

                    displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCYACHT_BRAND];

                    if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                      if (optionArray.length > 0) {
                        if (inClientArrayJS(optionArray, localArray[zloop][LOCYACHT_INDEX])) {
                          displayCboJS.options[nCurrentOption].selected = true;
                          displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                          bfoundSelection = true;
                        }
                      }
                    }
                    
                  }
                  
                } // (!bAllMotor && (inClientArrayJS(sCboMotor.split(","), localArray[zloop][LOCYACHT_MOTOR])) )
                                                
              }
              else {

                if (!bAllMotor && (inClientArrayJS(sCboMotor.split(","), localArray[zloop][LOCYACHT_MOTOR]))) { // only show brands that match the motor type - pick up the next option

                  displayCboJS.options[nCurrentOption] = new Option(localArray[zloop][LOCYACHT_INDEX]);
                  displayCboJS.options[nCurrentOption].value = localArray[zloop][LOCYACHT_INDEX];

                  displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCYACHT_BRAND];

                  if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                    if (optionArray.length > 0) {
                      if (inClientArrayJS(optionArray, localArray[zloop][LOCYACHT_INDEX])) {
                        displayCboJS.options[nCurrentOption].selected = true;
                        displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                        bfoundSelection = true;
                      }
                    }
                  }


                }
                else { // show the brand - pick up the next option

                  if (bAllMotor) {
                  
                    displayCboJS.options[nCurrentOption] = new Option(localArray[zloop][LOCYACHT_INDEX]);
                    displayCboJS.options[nCurrentOption].value = localArray[zloop][LOCYACHT_INDEX];

                    displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCYACHT_BRAND];

                    if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                      if (optionArray.length > 0) {
                        if (inClientArrayJS(optionArray, localArray[zloop][LOCYACHT_INDEX])) {
                          displayCboJS.options[nCurrentOption].selected = true;
                          displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                          bfoundSelection = true;
                        }
                      }
                    }
                    
                  }

                } // (!bAllMotor && (inClientArrayJS(sCboMotor.split(","), localArray[zloop][LOCYACHT_MOTOR])) )
                
              } // (nCurrentOption == 0)

              nCurrentOption = nCurrentOption + 1;

              if (sRememberBrand == "") {
                sRememberBrand = localArray[zloop][LOCYACHT_BRAND];
              }
              else {
                sRememberBrand = sRememberBrand + "," + localArray[zloop][LOCYACHT_BRAND];
              }

            } // ((localArray[zloop][LOCYACHT_BRAND] != "") && (localArray[zloop][LOCYACHT_BRAND] != sRememberBrand))
          }  // ((localArray[zloop][LOCYACHT_CATEGORY] == sCategoryBrand) && (localArray[zloop][LOCYACHT_MOTOR] == sMotorBrand))
        } // ((localArray[zloop][LOCYACHT_CATEGORY] != null) && (localArray[zloop][LOCYACHT_CATEGORY] != ""))
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

  displayArray = null;
  optionArray = null;
  localArray = null;
  motorArray = null;

  return displayCboJS;

}

function fillYachtModel(inModelCbo, inSelMotor, inSelSize, inSelBrand, inSelected, sessionModel) {

  var bfoundSelection = false;
  var bSelectedItem = false;

  var sTempModelID = 0;
  var sTempModelIndex = 0;

  var sYachtSize = "";
  var sYachtMotor = "";
  var sYachtBrand = "";

  var rememberMotorType = "";
  var rememberSize = "";
  var rememberBrand = "";
  var sRememberModel = "";

  var sCboModel = "";
  var sCboSize = "";
  var sCboBrand = "";
  var sCboMotor = "";

  var bAllMotor = false;
  var bAllSize = false;
  var bAllBrand = false;

  var selectedBrandSize = "";
  var selectedSizeMotor = "";
  var selectedBrandMotor = "";

  var nCurrentOption = 0;

  var bSingleBrandSelected = true;
  var bSingleSizeSelected = true;
  var bSingleMotorSelected = true;
  var bSingleBrandMotorSelected = true;
  var bSingleBrandSizeSelected = true;

  var brandArray = null;

  var displayCboJS = null;
  var displayArray = null;
  var optionArray = null;
  var localArray = null;
  var motorArray = null;

  var sSelectionStr = "";

  if ((localMasterYachtArray != null) && (localMasterYachtArray.length > 0)) {
    localArray = localMasterYachtArray;
  }

  // get the list of selected types
  if ((typeof (inSelMotor.name) != "undefined") && (inSelMotor != null)) {
    for (var nloop = 0; nloop < inSelMotor.length; nloop++) {
      if ((inSelMotor.options[nloop].selected == true) || (bAllMotor == true)) {
        if (nloop == 0) {
          bAllMotor = true;
        }
        else {
          if (sCboMotor == "") {
            sCboMotor = inSelMotor.options[nloop].value;
          }
          else {
            sCboMotor = sCboMotor + "," + inSelMotor.options[nloop].value;
          }
        } // (nloop == 0)
      } // ((inSelMotor.options[nloop].selected == true) || (bAllMotor == true))
    } // (nloop = 0; nloop < inSelMotor.length; nloop++)
  }
  else {
    bAllMotor = true;
  }

  // get the list of selected sizes
  if ((typeof (inSelSize.name) != "undefined") && (inSelSize != null)) {
    for (var nloop = 0; nloop < inSelSize.length; nloop++) {
      if ((inSelSize.options[nloop].selected == true) || (bAllSize == true)) {
        if (nloop == 0) {
          bAllSize = true;
        }
        else {
          if (sCboSize == "") {
            sCboSize = inSelSize.options[nloop].value;
            selectedSizeMotor = inSelSize.options[nloop].value;
          }
          else {
            sCboSize = sCboSize + "," + inSelSize.options[nloop].value;
            selectedSizeMotor = selectedSizeMotor + "," + inSelSize.options[nloop].value;
          }
        } // (nloop == 0)
      } // ((inSelSize.options[nloop].selected == true) || (bAllSize == true))
    } // (nloop = 0; nloop < inSelSize.length; nloop++)
  }
  else {
    bAllCategory = true;
  }

  // get the list of selected brands
  if ((typeof (inSelBrand.name) != "undefined") && (inSelBrand != null)) {
    for (var nloop = 0; nloop < inSelBrand.length; nloop++) {
      if ((inSelBrand.options[nloop].selected == true) || (bAllBrand == true)) {
        if (nloop == 0) {
          bAllBrand = true;
        }
        else {
          if (sCboBrand == "") {
            sCboBrand = inSelBrand.options[nloop].value;
            selectedBrandSize = inSelBrand.options[nloop].value;
            selectedBrandMotor = inSelBrand.options[nloop].value;
          }
          else {
            sCboBrand = sCboBrand + "," + inSelBrand.options[nloop].value;
            selectedBrandSize = selectedBrandSize + "," + inSelBrand.options[nloop].value;
            selectedBrandMotor = selectedBrandMotor + "," + inSelBrand.options[nloop].value;
          }

          if ((bAllSize) && (!bAllBrand)) {
            sCboSize = "";
          }
        } // (nloop == 0)
      } // ((inSelBrand.options[nloop].selected == true) || (bAllBrand == true))
    } // (nloop = 0; nloop < inSelBrand.length; nloop++)
  }

  if (sCboBrand != "") {
    brandArray = sCboBrand.split(",");
  }
  else {
    bAllBrand = true;
    brandArray = ("All").split(",");
  } // (sCboBrand != "")

//  if ((brandArray == null) && (!bAllBrand)) {
//    bSingleBrandSelected = true;
//  }
//  else {
//    if (((brandArray != null)) && (brandArray.length > 0) && (!bAllBrand)) {

//      bSingleBrandSelected = true;

//      for (var xloop = 0; xloop < brandArray.length; xloop++) {
//        if (!isNaN(brandArray[xloop])) {
//          if ((localArray[brandArray[xloop]][LOCYACHT_BRAND] != rememberBrand) && (rememberBrand != "")) {
//            bSingleBrandSelected = false;
//            break;
//          }
//          rememberBrand = localArray[brandArray[xloop]][LOCYACHT_BRAND];
//        }
//      }
//    }
//  }

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

  if ((sCboSize != "") && (!bSelectedItem) && (sCboBrand == "")) {
    displayArray = sCboSize.split(",");
    sCboMotor = selectedSizeMotor;
  }
  else {
    if ((bSelectedItem) && (!bAllSize) && (sCboBrand != "")) {
      displayArray = selectedBrandSize.split(",");
      sCboMotor = selectedBrandMotor;
    }
    else {
      if ((sCboBrand != "") && (!bAllBrand)) {
        displayArray = selectedBrandSize.split(",");
        sCboMotor = selectedBrandMotor;
      }
      else {
        displayArray = ("All").split(",");
        bAllSize = true;
      }
    }
  }

//  if ((displayArray.length == 1) && (!bAllSize)) {
//    bSingleSizeSelected = true;
//  }

  if (bAllSize && bAllBrand) {
    displayCboJS.options[0].innerHTML = "All";
    displayCboJS.options[0].value = "All";
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
    return displayCboJS;
  }
  else {
    if ((!bAllSize) && bAllBrand) {
      displayCboJS.options[0].innerHTML = "All";
      displayCboJS.options[0].value = "All";
      displayCboJS.options[0].selected = true;
      displayCboJS.options[0].selectedindex = 0;
      return displayCboJS;
    }
  }

  if (displayArray.length > 0) {
  
//    if (sCboMotor != "") {

//      bSingleMotorSelected = true;
//      motorArray = sCboMotor.split(",");

//      if ((motorArray != null) && (motorArray != "")) {
//        for (var xloop = 0; xloop < motorArray.length; xloop++) {
//          if (!isNaN(motorArray[xloop])) {
//            if ((localArray[motorArray[xloop]][LOCYACHT_MOTOR] != rememberMotorType) && (rememberMotorType != "")) {
//              bSingleMotorSelected = false;
//              break;
//            }
//            rememberMotorType = localArray[motorArray[xloop]][LOCYACHT_MOTOR];
//          }
//        }
//      }
//    }
//    else {
//      motorArray = ("M").split(",");
//      bSingleMotorSelected = true;
//    }

//    if ((brandArray.length > 0) && (brandArray[0].toUpperCase() != "ALL")) {

//      bSingleBrandMotorSelected = true;
//      rememberMotorType = "";

//      if ((brandArray != null) && (brandArray != "")) {
//        for (var xloop = 0; xloop < brandArray.length; xloop++) {
//          if (!isNaN(brandArray[xloop])) {
//            if ((localArray[brandArray[xloop]][LOCYACHT_MOTOR] != rememberMotorType) && (rememberMotorType != "")) {
//              bSingleBrandMotorSelected = false;
//              break;
//            }
//            rememberMotorType = localArray[brandArray[xloop]][LOCYACHT_MOTOR];
//          }
//        }
//      }
//    }

//    if ((brandArray.length > 0) && (brandArray[0].toUpperCase() != "ALL")) {

//      bSingleBrandSizeSelected = true;

//      if ((brandArray != null) && (brandArray != "")) {
//        for (var xloop = 0; xloop < brandArray.length; xloop++) {
//          if (!isNaN(brandArray[xloop])) {
//            if ((localArray[brandArray[xloop]][LOCYACHT_CATEGORY] != rememberSize) && (rememberSize != "")) {
//              bSingleBrandSizeSelected = false;
//              break;
//            }
//            rememberSize = localArray[brandArray[xloop]][LOCYACHT_CATEGORY];
//          }
//        }
//      }
//    }

    for (var xloop = 0; xloop < displayArray.length; xloop++) {  // loop through the display array to get the makes to find the models to display
      if (displayArray[xloop].toUpperCase() != "ALL") {

        sTempModelID = displayArray[xloop];

        sYachtBrand = localArray[Number(sTempModelID)][LOCYACHT_BRAND];

      }

      for (var zloop = Number(sTempModelID); zloop < localArray.length; zloop++) {
        if ((typeof (localArray[zloop][LOCYACHT_INDEX]) != "undefined") && (localArray[zloop][LOCYACHT_INDEX] != null)) {
          if (localArray[zloop][LOCYACHT_BRAND] == sYachtBrand) {
            if ((Number(localArray[zloop][LOCYACHT_MODEL_ID]) != 0) && (!inClientArrayJS(sRememberModel.split(","), localArray[zloop][LOCYACHT_MODEL]))) {

              if (isModelInArray(brandArray, localArray[zloop][LOCYACHT_INDEX])) {

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
                  displayCboJS.options[nCurrentOption] = new Option(localArray[zloop][LOCYACHT_INDEX]);
                  displayCboJS.options[nCurrentOption].value = localArray[zloop][LOCYACHT_INDEX];

                  if (!bSingleBrandSelected) {
                    if (!bSingleMotorSelected) {
                      if (!bSingleBrandMotorSelected) {
                        displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_MOTOR] + "][" + localArray[zloop][LOCYACHT_CATEGORY] + "][" + localArray[zloop][LOCYACHT_BRAND_ABR] + "] - " + localArray[zloop][LOCYACHT_MODEL];
                      }
                      else {
                        displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_CATEGORY] + "][" + localArray[zloop][LOCYACHT_BRAND_ABR] + "] - " + localArray[zloop][LOCYACHT_MODEL];
                      }
                    }
                    else {
                      if (!bSingleSizeSelected) {
                        if (!bSingleBrandSizeSelected) {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_CATEGORY] + "][" + localArray[zloop][LOCYACHT_BRAND_ABR] + "] - " + localArray[zloop][LOCYACHT_MODEL];
                        }
                        else {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_BRAND_ABR] + "] - " + localArray[zloop][LOCYACHT_MODEL];
                        }
                      }
                      else {
                        displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_BRAND_ABR] + "] - " + localArray[zloop][LOCYACHT_MODEL];
                      }
                    }
                  }
                  else {
                    if (!bSingleSizeSelected) {
                      if (!bSingleBrandSizeSelected) {
                        if ((!bSingleMotorSelected) || (!bSingleBrandMotorSelected)) {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_MOTOR] + "][" + localArray[zloop][LOCYACHT_CATEGORY] + "] - " + localArray[zloop][LOCYACHT_MODEL];
                        }
                        else {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_CATEGORY] + "] - " + localArray[zloop][LOCYACHT_MODEL];
                        }
                      }
                      else {
                        displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCYACHT_MODEL];
                      }
                    }
                    else {
                      displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCYACHT_MODEL];
                    }
                  }

                  if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                    if (optionArray.length > 0) {
                      if (inClientArrayJS(optionArray, localArray[zloop][LOCYACHT_INDEX])) {
                        displayCboJS.options[nCurrentOption].selected = true;
                        displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                        bfoundSelection = true;
                      }
                    }
                  }
                }
                else {

                  displayCboJS.options[nCurrentOption] = new Option(localArray[zloop][LOCYACHT_INDEX]);
                  displayCboJS.options[nCurrentOption].value = localArray[zloop][LOCYACHT_INDEX];

                  if (!bSingleBrandSelected) {
                    if (!bSingleMotorSelected) {
                      if (!bSingleBrandMotorSelected) {
                        displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_MOTOR] + "][" + localArray[zloop][LOCYACHT_CATEGORY] + "][" + localArray[zloop][LOCYACHT_BRAND_ABR] + "] - " + localArray[zloop][LOCYACHT_MODEL];
                      }
                      else {
                        displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_CATEGORY] + "][" + localArray[zloop][LOCYACHT_BRAND_ABR] + "] - " + localArray[zloop][LOCYACHT_MODEL];
                      }
                    }
                    else {
                      if (!bSingleSizeSelected) {
                        if (!bSingleBrandSizeSelected) {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_CATEGORY] + "][" + localArray[zloop][LOCYACHT_BRAND_ABR] + "] - " + localArray[zloop][LOCYACHT_MODEL];
                        }
                        else {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_BRAND_ABR] + "] - " + localArray[zloop][LOCYACHT_MODEL];
                        }
                      }
                      else {
                        displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_BRAND_ABR] + "] - " + localArray[zloop][LOCYACHT_MODEL];
                      }
                    }
                  }
                  else {
                    if (!bSingleSizeSelected) {
                      if (!bSingleBrandSizeSelected) {
                        if ((!bSingleMotorSelected) || (!bSingleBrandMotorSelected)) {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_MOTOR] + "][" + localArray[zloop][LOCYACHT_CATEGORY] + "] - " + localArray[zloop][LOCYACHT_MODEL];
                        }
                        else {
                          displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_CATEGORY] + "] - " + localArray[zloop][LOCYACHT_MODEL];
                        }
                      }
                      else {
                        displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCYACHT_MODEL];
                      }
                    }
                    else {
                      displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCYACHT_MODEL];
                    }
                  }

                  if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                    if (optionArray.length > 0) {
                      if (inClientArrayJS(optionArray, localArray[zloop][LOCYACHT_INDEX])) {
                        displayCboJS.options[nCurrentOption].selected = true;
                        displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                        bfoundSelection = true;
                      }
                    }
                  }
                } // (nCurrentOption == 0)

                nCurrentOption = nCurrentOption + 1;

                if (sRememberModel == "") {
                  sRememberModel = localArray[zloop][LOCYACHT_MODEL];
                }
                else {
                  sRememberModel = sRememberModel + "," + localArray[zloop][LOCYACHT_MODEL];
                }

              } // (isModelInArray(brandArray, localArray[zloop][LOCYACHT_INDEX], isFiltered))
            } // ((Number(localArray[zloop][LOCYACHT_MODEL_ID]) != 0) && (Number(localArray[zloop][LOCYACHT_MODEL_ID]) != Number(nRememberModel)))
          } // (localArray[zloop][LOCYACHT_BRAND] == sYachtBrand)
        } // ((typeof (localArray[zloop][LOCYACHT_INDEX]) != "undefined") && (localArray[zloop][LOCYACHT_INDEX] != null))
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

  brandArray = null;

  displayArray = null;
  optionArray = null;
  optionList = null;
  localArray = null;
  motorArray = null;

  return displayCboJS;

}

function refreshYachtTypeSizeBrandModel(fromEvent, updateWhat) {

  var typeCbo = document.getElementById(yachtTypeCboName); // variables from the script on the page
  var sizeCbo = document.getElementById(yachtSizeCboName); // variables from the script on the page
  var brandCbo = document.getElementById(yachtBrandCboName);  // variables from the script on the page
  var modelCbo = document.getElementById(yachtModelCboName); // variables from the script on the page

  var sessionType = document.getElementById("sessYachtTypeID");   // grabs hidden value from page
  var sessionSize = document.getElementById("sessYachtSizeID");   // grabs hidden value from page
  var sessionBrand = document.getElementById("sessYachtBrandID");   // grabs hidden value from page
  var sessionModel = document.getElementById("sessYachtModelID"); // grabs hidden value from page

  switch (fromEvent) {
    case "onChange":
      {
        // need to reset both the dropdown selections and the session variables
        // this causes false selections when the user resets the dropdowns to "ALL"
        //

        if (typeCbo.options[0].selected == true) {

          if (sizeCbo.options[0].selected == true) {
            sizeCbo.options[0].selected = true;
          }

          if (brandCbo.options[0].selected == true) {
            brandCbo.options[0].selected = true;
          }

          if (modelCbo.options[0].selected == true) {
            modelCbo.options[0].selected = true;
          }

          sessionType.value = "";
        }

        if ((typeCbo.options[0].selected == true) && (sizeCbo.options[0].selected == true)) {
          if (sizeCbo.options[0].selected == true) {
            sizeCbo.options[0].selected = true;
          }
          sessionSize.value = "";
        }

        if ((brandCbo.options[0].selected == true) && (typeCbo.options[0].selected == true) && (sizeCbo.options[0].selected == true)) {
          if (brandCbo.options[0].selected == true) {
            brandCbo.options[0].selected = true;
          }
          sessionBrand.value = "";
        }

        if ((modelCbo.options[0].selected == true) && (brandCbo.options[0].selected == true) && (typeCbo.options[0].selected == true) && (sizeCbo.options[0].selected == true)) {
          if (modelCbo.options[0].selected == true) {
            modelCbo.options[0].selected = true;
          }
          sessionModel.value = "";
        }

        switch (updateWhat) {

          case "brand":
            {
              modelCbo = fillYachtModel(modelCbo, typeCbo, sizeCbo, brandCbo, modelCbo, sessionModel.value);
              break;
            }
          case "size":
            {
              brandCbo = fillYachtBrand(brandCbo, typeCbo, sizeCbo, brandCbo, sessionBrand.value);
              modelCbo = fillYachtModel(modelCbo, typeCbo, sizeCbo, brandCbo, modelCbo, sessionModel.value);
              break;
            }
          case "type":
            {
              sizeCbo = fillYachtSize(sizeCbo, typeCbo, sizeCbo, sessionSize.value);
              brandCbo = fillYachtBrand(brandCbo, typeCbo, sizeCbo, brandCbo, sessionBrand.value);
              modelCbo = fillYachtModel(modelCbo, typeCbo, sizeCbo, brandCbo, modelCbo, sessionModel.value);
              break;
            }
        }
        break;
      }
    case "onClick":
      {

        switch (updateWhat) {
          case "size":
            {
              brandCbo = fillYachtBrand(brandCbo, typeCbo, sizeCbo, brandCbo, sessionBrand.value);
              modelCbo = fillYachtModel(modelCbo, typeCbo, sizeCbo, brandCbo, modelCbo, sessionModel.value);
              break;
            }
          case "type":
            {
              sizeCbo = fillYachtSize(sizeCbo, typeCbo, sizeCbo, sessionSize.value);
              brandCbo = fillYachtBrand(brandCbo, typeCbo, sizeCbo, brandCbo, sessionBrand.value);
              modelCbo = fillYachtModel(modelCbo, typeCbo, sizeCbo, brandCbo, modelCbo, sessionModel.value);
              break;
            }

          case "filter":
            {
              typeCbo = fillYachtType(typeCbo, typeCbo, sessionType.value);
              sizeCbo = fillYachtSize(sizeCbo, typeCbo, sizeCbo, sessionSize.value);
              brandCbo = fillYachtBrand(brandCbo, typeCbo, sizeCbo, brandCbo, sessionBrand.value);
              modelCbo = fillYachtModel(modelCbo, typeCbo, sizeCbo, brandCbo, modelCbo, sessionModel.value);
              break;
            }
        }
        break;
      }

    default:
      {

        if (updateWhat == "") {

          typeCbo = fillYachtType(typeCbo, typeCbo, sessionType.value);
          sizeCbo = fillYachtSize(sizeCbo, typeCbo, sizeCbo, sessionSize.value);
          brandCbo = fillYachtBrand(brandCbo, typeCbo, sizeCbo, brandCbo, sessionBrand.value);
          modelCbo = fillYachtModel(modelCbo, typeCbo, sizeCbo, brandCbo, modelCbo, sessionModel.value);

        } // 'updateWhat = ""
        break;
      }
  }
  return true;
}
