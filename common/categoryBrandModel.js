
// ********************************************************************************
// Copyright 2004-11. JETNET,LLC. All rights reserved.
//
// $$Archive: /commonWebProject/common/categoryBrandModel.js $
// $$Author: Mike $
// $$Date: 6/19/19 8:45a $
// $$Modtime: 6/18/19 6:12p $
// $$Revision: 2 $
// $$Workfile: categoryBrandModel.js $
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
        if ((localArray[modelIndex][LOCYACHT_BRAND] == localArray[n_inItemMake][LOCYACHT_BRAND]) &&
            (localArray[modelIndex][LOCYACHT_CATEGORY] == localArray[n_inItemMake][LOCYACHT_CATEGORY]) &&
            (localArray[modelIndex][LOCYACHT_MOTOR] == localArray[n_inItemMake][LOCYACHT_MOTOR])) {
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

function fillYachtCategory(inCboCategory, inSelected, sessionCategory) {

  var bSelectedItem = false;
  var bfoundSelection = false;
  var sRememberCategory = "";
  var nCurrentOption = 0;

  var bSingleMotorTypeSelected = false;

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
        sessionCategory = sSelectionStr;
      }
      else {
        if ((sessionCategory != null) && (sessionCategory != "")) {
          bSelectedItem = true;
          var remove = /, /gi;
          sSelectionStr = sessionCategory.replace(remove, ",");
        }
      }

      optionArray = sSelectionStr.split(",");
    }
    else {
      if ((sessionCategory != null) && (sessionCategory != "")) {
        bSelectedItem = true;
        var remove = /, /gi;
        sSelectionStr = sessionCategory.replace(remove, ",");
        optionArray = sSelectionStr.split(",");
      }
    }

    //	  if (sSelectionStr != "") {
    //	    
    //	    if (bSelectedItem) {
    //	      if (sSelectionStr.toUpperCase() != "ALL") {
    //	        sessionCategory = sSelectionStr;
    //	      }
    //	      optionArray = sSelectionStr.split(",");
    //		  }
    //		  else {
    //		    if (inSelected.length > 1) {
    //		      optionArray = ("All").split(",");
    //		    }
    //		    else {
    //		      if ((sessionCategory != null) && (sessionCategory != "")) {
    //		        var remove = /, /gi;
    //		        sSelectionStr = sessionCategory.replace(remove, ",");
    //		        optionArray = sSelectionStr.split(",");	
    //	        } // (sessionCategory != "") 
    //		    } // (optionList.length > 1)
    //		  } // (bSelectedItem)
    //    } // (sSelectionStr != "") 
    //        
  } // ((typeof(inSelected.name) != "undefined") && (inSelected != ""))

  if (sSelectionStr.toUpperCase() != "ALL") {

    var rememberMotorType = "";
    var rememberYachtCategory = "";
    var sCategoryStr = "";
    var sMotorStr = "";

    if ((optionArray != null) && (optionArray != "")) {

      // get the types for selected index	   
      for (var x = 0; x < optionArray.length; x++) {
        if (!isNaN(optionArray[x])) {
          if ((localArray[optionArray[x]][LOCYACHT_CATEGORY] != rememberYachtCategory) || (localArray[optionArray[x]][LOCYACHT_MOTOR] != rememberMotorType)) {
            if (sCategoryStr == "") {
              sCategoryStr = localArray[optionArray[x]][LOCYACHT_CATEGORY];
              sMotorStr = localArray[optionArray[x]][LOCYACHT_MOTOR];
            }
            else {
              sCategoryStr = sCategoryStr + "," + localArray[optionArray[x]][LOCYACHT_CATEGORY];
              sMotorStr = sMotorStr + "," + localArray[optionArray[x]][LOCYACHT_MOTOR];
            }
          }
          rememberYachtCategory = localArray[optionArray[x]][LOCYACHT_CATEGORY];
          rememberMotorType = localArray[optionArray[x]][LOCYACHT_MOTOR];
        }
      }
    }

    var tmpCategoryArray = sCategoryStr.split(",");
    var tmpMotorArray = sMotorStr.split(",");
    var tmpCategoryString = "";

    for (var z = 0; z < sCategoryStr.length; z++) {
      for (var y = 0; y < localArray.length; y++) {
        if ((typeof (localArray[y][LOCYACHT_CATEGORY]) != "undefined") && (localArray[y][LOCYACHT_CATEGORY] != null)) {
          if ((localArray[y][LOCYACHT_CATEGORY] == tmpCategoryArray[z]) && (localArray[y][LOCYACHT_MOTOR] == tmpMotorArray[z])) {
            if (tmpCategoryString == "") {
              tmpCategoryString = localArray[y][LOCAIR_INDEX];
            }
            else {
              tmpCategoryString = tmpCategoryString + "," + localArray[y][LOCAIR_INDEX];
            }
            break;
          }
        }
      }
    }

    optionArray = tmpCategoryString.split(",");

  }

  displayCboJS = inCboCategory;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  for (var iloop = 0; iloop < localArray.length; iloop++) {

    if ((typeof (localArray[iloop][LOCYACHT_CATEGORY]) != "undefined") && (localArray[iloop][LOCYACHT_CATEGORY] != null)) {
      if (localArray[iloop][LOCYACHT_CATEGORY] != sRememberCategory) {
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
          displayCboJS.options[nCurrentOption].innerHTML = getYachtLabelClient(localArray[iloop][LOCYACHT_MOTOR], localArray[iloop][LOCYACHT_CATEGORY], bSingleMotorTypeSelected);

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
          displayCboJS.options[nCurrentOption].innerHTML = getYachtLabelClient(localArray[iloop][LOCYACHT_MOTOR], localArray[iloop][LOCYACHT_CATEGORY], bSingleMotorTypeSelected);

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
        sRememberCategory = localArray[iloop][LOCYACHT_CATEGORY];

      } // ((localArray[iloop][LOCYACHT_INDEX] != -1) && (localArray[iloop][LOCYACHT_CATEGORY] != sRememberType))
    } // (localArray[iloop][LOCYACHT_INDEX] != -1)

  } // (iloop = 0; iloop < localArray.length; iloop++)


  if ((!bfoundSelection) && (displayCboJS.options.length > 1)) {
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
  }

  return displayCboJS;

  localArray = null;
  optionArray = null;

}

function fillYachtBrand(inBrandCbo, inSelCategory, inSelected, sessionBrand) {

  var bfoundSelection = false;

  var sTempBrandIndex = 0;
  var sCategoryBrand = "";
  var sMotorBrand = "";

  var rememberMotorType = "";

  var sRememberBrand = "";
  var sCboCategory = "";

  var bAllCategory = false;
  var nCurrentOption = 0;
  var bSelectedItem = false;
  var bSingleCategorySelected = false;
  var bSingleMotorSelected = false;

  var selectedCategory = "";
  var selectedMotor = "";
  var selectedCategoryMotor = "";

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
  if ((typeof (inSelCategory.name) != "undefined") && (inSelCategory != null)) {
    for (var nloop = 0; nloop < inSelCategory.length; nloop++) {
      if ((inSelCategory.options[nloop].selected == true) || (bAllCategory == true)) {
        if (nloop == 0) {
          bAllCategory = true;
        }
        else {
          if (sCboCategory == "") {
            sCboCategory = inSelCategory.options[nloop].value;
            selectedCategoryMotor = inSelCategory.options[nloop].value;
          }
          else {
            sCboCategory = sCboCategory + "," + inSelCategory.options[nloop].value;
            selectedCategoryMotor = selectedCategoryMotor + "," + inSelCategory.options[nloop].value;
          }
        } // (nloop == 0)
      } // ((inSelCategory.options[nloop].selected == true) || (bAllCategory == true))
    } // (nloop = 0; nloop < inSelCategory.length; nloop++)
  }
  else {
    bAllCategory = true;
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
      if ((!bAllCategory) && bSelectedItem) {
        if (sSelectionStr.toUpperCase() != "ALL") {
          sessionBrand = sSelectionStr;
        }
        optionArray = sSelectionStr.split(",");
        if (optionArray.length > 0) {
          for (var x = 0; x < optionArray.length; x++) {
            if (selectedCategory == "") {
              selectedCategory = optionArray[x];
              selectedModelMotor = optionArray[x];
            }
            else {
              selectedCategory = selectedCategory + "," + optionArray[x];
              selectedCategoryMotor = selectedCategoryMotor + "," + optionArray[x];
            }
          } // (xloop = 0; xloop < optionArray.length; xloop++)
        }
      }
      else {
        optionArray = ("All").split(",");
      } // (!bAllCategory && bSelectedItem)    
    } // (sSelectionStr != "")

    if ((sessionBrand != null) && (sessionBrand != "")) {
      bSelectedItem = true;
      var remove = /, /gi;
      sSelectionStr = sessionBrand.replace(remove, ",");
      optionArray = sSelectionStr.split(",");

      if (optionArray.length > 0) {
        for (var x = 0; x < optionArray.length; x++) {
          if (selectedCategory == "") {
            selectedCategory = optionArray[x];
            selectedModelMotor = optionArray[x];
          }
          else {
            selectedCategory = selectedCategory + "," + optionArray[x];
            selectedCategoryMotor = selectedCategoryMotor + "," + optionArray[x];
          }
        } // (var x = 0; x < optionArray.length; x++)
      }
    } // (sessionBrand != "") 

  } // ((typeof(inSelected.name) != "undefined") && (inSelected != ""))

  if ((sCboCategory != "") && (!bSelectedItem)) {
    displayArray = sCboCategory.split(",");
    selectedMotor = selectedCategoryMotor;
  }
  else {
    if ((!bAllCategory) && (bSelectedItem)) {

      // need to check if this type is different from the selected types
      // if its different then use selected types to display model
      var tmpSelectedCategory = sCboCategory.split(",");
      var inBrandCategory = selectedCategory.split(",");
      var bMatchedCategories = false;
      var brandIndex = 0;
      var categoryIndex = 0;

      for (var x = 0; x < inBrandCategory.length; x++) {
        for (var y = 0; y < tmpSelectedCategory.length; y++) {
          // get index for item
          if (!isNaN(inBrandCategory[x]) && !isNaN(tmpSelectedCategory[y])) {
            brandIndex = inBrandCategory[x];
            categoryIndex = tmpSelectedCategory[y];
            if ((brandIndex != -1) && (categoryIndex != -1)) {
              if (localArray[brandIndex][LOCYACHT_CATEGORY] == localArray[categoryIndex][LOCYACHT_CATEGORY]) {
                bMatchedCategories = true;
              }
            }
          }
        }
      }

      if (!bMatchedCategories) {
        displayArray = sCboCategory.split(",");
        selectedMotor = selectedCategoryMotor;
        optionArray = ("All").split(",");
        sSelectionStr = "";
      }
      else {
        displayArray = sCboCategory.split(",");
        selectedMotor = selectedCategoryMotor;
      }
    }
    else {
      displayArray = sCboCategory.split(",");
      selectedMotor = selectedCategoryMotor;
      optionArray = ("All").split(",");
      sSelectionStr = "";
    }
  }

  displayCboJS = inBrandCbo;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  if ((displayArray.length == 1) && (!bAllCategory)) {
    bSingleCategorySelected = true;
  }

  if (displayArray.length > 0) {

    if (selectedMotor != "") {

      bSingleMotorSelected = true;
      motorArray = selectedMotor.split(",");

      if ((motorArray != null) && (motorArray != "")) {

        for (var xloop = 0; xloop < motorArray.length; xloop++) {
          if (!isNaN(motorArray[xloop])) {
            if ((localArray[motorArray[xloop]][LOCYACHT_MOTOR] != rememberMotorType) && (rememberMotorType != "")) {
              bSingleMotorSelected = false;
              break;
            }
            rememberMotorType = localArray[motorArray[xloop]][LOCYACHT_MOTOR];
          }
        }
      }
    }
    else {
      motorArray = ("M").split(",");
      bSingleMotorSelected = true;
    }

    for (var xloop = 0; xloop < displayArray.length; xloop++) {
      if (displayArray[xloop].toUpperCase() != "ALL") {

        sTempBrandIndex = displayArray[xloop];
        sCategoryBrand = localArray[Number(sTempBrandIndex)][LOCYACHT_CATEGORY];
        sMotorBrand = localArray[Number(sTempBrandIndex)][LOCYACHT_MOTOR];

      }

      if (bAllCategory && ((sessionBrand != null) && (sessionBrand != ""))) {
        sTempBrandIndex = "0"
      }

      for (var zloop = Number(sTempBrandIndex); zloop < localArray.length; zloop++) {
        if ((typeof (localArray[zloop][LOCYACHT_INDEX]) != "undefined") && (localArray[zloop][LOCYACHT_INDEX] != null)) {
          if ((localArray[zloop][LOCYACHT_CATEGORY] == sCategoryBrand) && (localArray[zloop][LOCYACHT_MOTOR] == sMotorBrand)) {
            if (((typeof (localArray[zloop][LOCYACHT_BRAND]) != "undefined") && (localArray[zloop][LOCYACHT_BRAND] != null)) && (localArray[zloop][LOCYACHT_BRAND] != sRememberBrand)) {
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
                displayCboJS.options[nCurrentOption].value = localArray[zloop][LOCYACHT_INDEX];

                if (!bSingleCategorySelected) {
                  if (!bSingleMotorSelected) {
                    displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_MOTOR] + "][" + localArray[zloop][LOCYACHT_CATEGORY] + "] - " + localArray[zloop][LOCYACHT_BRAND];
                  }
                  else {
                    displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_CATEGORY] + "] - " + localArray[zloop][LOCYACHT_BRAND];
                  }
                }
                else {
                  displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCYACHT_BRAND];
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

                displayCboJS.options[nCurrentOption] = new Option("");
                displayCboJS.options[nCurrentOption].value = localArray[zloop][LOCYACHT_INDEX];

                if (!bSingleCategorySelected) {
                  if (!bSingleMotorSelected) {
                    displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_MOTOR] + "][" + localArray[zloop][LOCYACHT_CATEGORY] + "] - " + localArray[zloop][LOCYACHT_BRAND];
                  }
                  else {
                    displayCboJS.options[nCurrentOption].innerHTML = "[" + localArray[zloop][LOCYACHT_CATEGORY] + "] - " + localArray[zloop][LOCYACHT_BRAND];
                  }
                }
                else {
                  displayCboJS.options[nCurrentOption].innerHTML = localArray[zloop][LOCYACHT_BRAND];
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
              sRememberBrand = localArray[zloop][LOCYACHT_BRAND];

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

function fillYachtModel(inModelCbo, inSelCategory, inSelBrand, inSelected, sessionModel) {

  var bfoundSelection = false;
  var bSelectedItem = false;

  var sTempModelID = 0;
  var sTempModelIndex = 0;

  var sYachtCategory = "";
  var sYachtMotor = "";
  var sYachtBrand = "";

  var rememberMotorType = "";
  var rememberCategory = "";
  var rememberBrand = "";
  var nRememberModel = 0;

  var sCboModel = "";
  var sCboCategory = "";
  var sCboBrand = "";

  var bAllCategory = false;
  var bAllBrand = false;

  var selectedBrandCategory = "";
  var selectedMotor = "";
  var selectedCategoryMotor = "";
  var selectedBrandMotor = "";

  var nCurrentOption = 0;

  var bSingleBrandSelected = false;
  var bSingleCategorySelected = false;
  var bSingleMotorSelected = false;
  var bSingleBrandMotorSelected = false;
  var bSingleBrandCategorySelected = false;

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
  if ((typeof (inSelCategory.name) != "undefined") && (inSelCategory != null)) {
    for (var nloop = 0; nloop < inSelCategory.length; nloop++) {
      if ((inSelCategory.options[nloop].selected == true) || (bAllCategory == true)) {
        if (nloop == 0) {
          bAllCategory = true;
        }
        else {
          if (sCboCategory == "") {
            sCboCategory = inSelCategory.options[nloop].value;
            selectedCategoryMotor = inSelCategory.options[nloop].value;
          }
          else {
            sCboCategory = sCboCategory + "," + inSelCategory.options[nloop].value;
            selectedCategoryMotor = selectedCategoryMotor + "," + inSelCategory.options[nloop].value;
          }
        } // (nloop == 0)
      } // ((inSelCategory.options[nloop].selected == true) || (bAllCategory == true))
    } // (nloop = 0; nloop < inSelCategory.length; nloop++)
  }
  else {
    bAllCategory = true;
  }

  // get the list of selected makes
  if ((typeof (inSelBrand.name) != "undefined") && (inSelBrand != null)) {
    for (var nloop = 0; nloop < inSelBrand.length; nloop++) {
      if ((inSelBrand.options[nloop].selected == true) || (bAllBrand == true)) {
        if (nloop == 0) {
          bAllBrand = true;
        }
        else {
          if (sCboBrand == "") {
            sCboBrand = inSelBrand.options[nloop].value;
            selectedBrandCategory = inSelBrand.options[nloop].value;
            selectedBrandMotor = inSelBrand.options[nloop].value;
          }
          else {
            sCboBrand = sCboBrand + "," + inSelBrand.options[nloop].value;
            selectedBrandCategory = selectedBrandCategory + "," + inSelBrand.options[nloop].value;
            selectedBrandMotor = selectedBrandMotor + "," + inSelBrand.options[nloop].value;
          }

          if ((bAllCategory) && (!bAllBrand)) {
            sCboCategory = "";
          }
        } // (nloop == 0)
      } // ((inSelCategory.options[nloop].selected == true) || (bAllCategory == true))
    } // (nloop = 0; nloop < inSelCategory.length; nloop++)
  }

  if (sCboBrand != "") {
    brandArray = sCboBrand.split(",");
  }
  else {
    bAllBrand = true;
    brandArray = ("All").split(",");
  } // (sCboBrand != "")

  if ((brandArray == null) && (!bAllBrand)) {
    bSingleBrandSelected = true;
  }
  else {
    if (((brandArray != null)) && (brandArray.length > 0) && (!bAllBrand)) {

      bSingleBrandSelected = true;

      for (var xloop = 0; xloop < brandArray.length; xloop++) {
        if (!isNaN(brandArray[xloop])) {
          if ((localArray[brandArray[xloop]][LOCYACHT_BRAND] != rememberBrand) && (rememberBrand != "")) {
            bSingleBrandSelected = false;
            break;
          }
          rememberBrand = localArray[brandArray[xloop]][LOCYACHT_BRAND];
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

  if ((sCboCategory != "") && (!bSelectedItem) && (sCboBrand == "")) {
    displayArray = sCboCategory.split(",");
    selectedMotor = selectedCategoryMotor;
  }
  else {
    if ((bSelectedItem) && (!bAllCategory) && (sCboBrand != "")) {
      displayArray = selectedBrandCategory.split(",");
      selectedMotor = selectedBrandMotor;
    }
    else {
      if ((sCboBrand != "") && (!bAllBrand)) {
        displayArray = selectedBrandCategory.split(",");
        selectedMotor = selectedBrandMotor;
      }
      else {
        displayArray = ("All").split(",");
        bAllCategory = true;
      }
    }
  }

  if ((displayArray.length == 1) && (!bAllCategory)) {
    bSingleCategorySelected = true;
  }

  if (bAllCategory && bAllBrand) {
    displayCboJS.options[0].innerHTML = "All";
    displayCboJS.options[0].value = "All";
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
    return displayCboJS;
  }
  else {
    if ((!bAllCategory) && bAllBrand) {
      displayCboJS.options[0].innerHTML = "All";
      displayCboJS.options[0].value = "All";
      displayCboJS.options[0].selected = true;
      displayCboJS.options[0].selectedindex = 0;
      return displayCboJS;
    }
  }

  if (displayArray.length > 0) {
    if (selectedMotor != "") {

      bSingleMotorSelected = true;
      motorArray = selectedMotor.split(",");

      if ((motorArray != null) && (motorArray != "")) {
        for (var xloop = 0; xloop < motorArray.length; xloop++) {
          if (!isNaN(motorArray[xloop])) {
            if ((localArray[motorArray[xloop]][LOCYACHT_MOTOR] != rememberMotorType) && (rememberMotorType != "")) {
              bSingleMotorSelected = false;
              break;
            }
            rememberMotorType = localArray[motorArray[xloop]][LOCYACHT_MOTOR];
          }
        }
      }
    }
    else {
      motorArray = ("M").split(",");
      bSingleMotorSelected = true;
    }

    if ((brandArray.length > 0) && (brandArray[0].toUpperCase() != "ALL")) {

      bSingleBrandMotorSelected = true;
      rememberMotorType = "";

      if ((brandArray != null) && (brandArray != "")) {
        for (var xloop = 0; xloop < brandArray.length; xloop++) {
          if (!isNaN(brandArray[xloop])) {
            if ((localArray[brandArray[xloop]][LOCYACHT_MOTOR] != rememberMotorType) && (rememberMotorType != "")) {
              bSingleBrandMotorSelected = false;
              break;
            }
            rememberMotorType = localArray[brandArray[xloop]][LOCYACHT_MOTOR];
          }
        }
      }
    }

    if ((brandArray.length > 0) && (brandArray[0].toUpperCase() != "ALL")) {

      bSingleBrandCategorySelected = true;

      if ((brandArray != null) && (brandArray != "")) {
        for (var xloop = 0; xloop < brandArray.length; xloop++) {
          if (!isNaN(brandArray[xloop])) {
            if ((localArray[brandArray[xloop]][LOCYACHT_CATEGORY] != rememberCategory) && (rememberCategory != "")) {
              bSingleBrandCategorySelected = false;
              break;
            }
            rememberCategory = localArray[brandArray[xloop]][LOCYACHT_CATEGORY];
          }
        }
      }
    }

    for (var xloop = 0; xloop < displayArray.length; xloop++) {  // loop through the display array to get the makes to find the models to display
      if (displayArray[xloop].toUpperCase() != "ALL") {

        sTempModelID = displayArray[xloop];

        sYachtCategory = localArray[Number(sTempModelID)][LOCYACHT_CATEGORY];
        sYachtMotor = localArray[Number(sTempModelID)][LOCYACHT_MOTOR];
        sYachtBrand = localArray[Number(sTempModelID)][LOCYACHT_BRAND];

      }

      for (var zloop = Number(sTempModelID); zloop < localArray.length; zloop++) {
        if ((typeof (localArray[zloop][LOCYACHT_INDEX]) != "undefined") && (localArray[zloop][LOCYACHT_INDEX] != null)) {
          if ((localArray[zloop][LOCYACHT_CATEGORY] == sYachtCategory) && (localArray[zloop][LOCYACHT_MOTOR] == sYachtMotor) && (localArray[zloop][LOCYACHT_BRAND] == sYachtBrand)) {
            if ((Number(localArray[zloop][LOCYACHT_MODEL_ID]) != 0) && (Number(localArray[zloop][LOCYACHT_MODEL_ID]) != Number(nRememberModel))) {

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
                  displayCboJS.options[nCurrentOption] = new Option("");
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
                      if (!bSingleCategorySelected) {
                        if (!bSingleBrandCategorySelected) {
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
                    if (!bSingleCategorySelected) {
                      if (!bSingleBrandCategorySelected) {
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

                  displayCboJS.options[nCurrentOption] = new Option("");
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
                      if (!bSingleCategorySelected) {
                        if (!bSingleBrandCategorySelected) {
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
                    if (!bSingleCategorySelected) {
                      if (!bSingleBrandCategorySelected) {
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
                nRememberModel = localArray[zloop][LOCYACHT_MODEL_ID];

              } // (isModelInArray(brandArray, localArray[zloop][LOCYACHT_INDEX], isFiltered))
            } // ((Number(localArray[zloop][LOCYACHT_MODEL_ID]) != 0) && (Number(localArray[zloop][LOCYACHT_MODEL_ID]) != Number(nRememberModel)))
          }
          else {
            break;
          } // ((localArray[zloop][LOCYACHT_CATEGORY] == sYachtCategory) && (localArray[zloop][LOCYACHT_MOTOR] == sYachtMotor) && (localArray[zloop][LOCYACHT_BRAND] == sYachtBrand))
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

function refreshYachtCategoryBrandModel(fromEvent, updateWhat) {

  var categoryCbo = document.getElementById(yachtCategoryCboName); // variables from the script on the page
  var brandCbo = document.getElementById(yachtBrandCboName);  // variables from the script on the page
  var modelCbo = document.getElementById(yachtModelCboName); // variables from the script on the page

  var sessionCategory = document.getElementById("sessYachtCategoryID");   // grabs hidden value from page
  var sessionBrand = document.getElementById("sessYachtBrandID");   // grabs hidden value from page
  var sessionModel = document.getElementById("sessYachtModelID"); // grabs hidden value from page

  switch (fromEvent) {
    case "onChange":
      {
        // need to reset both the dropdown selections and the session variables
        // this causes false selections when the user resets the dropdowns to "ALL"
        //

        if (categoryCbo.options[0].selected == true) {

          if (brandCbo.options[0].selected == true) {
            brandCbo.options[0].selected = true;
          }

          if (modelCbo.options[0].selected == true) {
            modelCbo.options[0].selected = true;
          }

          sessionCategory.value = "";
        }

        if ((brandCbo.options[0].selected == true) && (categoryCbo.options[0].selected == true)) {
          if (brandCbo.options[0].selected == true) {
            brandCbo.options[0].selected = true;
          }
          sessionBrand.value = "";
        }

        if ((modelCbo.options[0].selected == true) && (brandCbo.options[0].selected == true) && (categoryCbo.options[0].selected == true)) {
          if (modelCbo.options[0].selected == true) {
            modelCbo.options[0].selected = true;
          }
          sessionModel.value = "";
        }

        switch (updateWhat) {

          case "brand":
            {
              modelCbo = fillYachtModel(modelCbo, categoryCbo, brandCbo, modelCbo, sessionModel.value);
              break;
            }
          case "category":
            {
              brandCbo = fillYachtBrand(brandCbo, categoryCbo, brandCbo, sessionBrand.value);
              modelCbo = fillYachtModel(modelCbo, categoryCbo, brandCbo, modelCbo, sessionModel.value);
              break;
            }
        }
        break;
      }
    case "onClick":
      {

        switch (updateWhat) {
          case "category":
            {
              brandCbo = fillYachtBrand(brandCbo, categoryCbo, brandCbo, sessionBrand.value);
              modelCbo = fillYachtModel(modelCbo, categoryCbo, brandCbo, modelCbo, sessionModel.value);
              break;
            }

          case "filter":
            {
              categoryCbo = fillYachtCategory(categoryCbo, categoryCbo, sessionCategory.value);
              brandCbo = fillYachtBrand(brandCbo, categoryCbo, brandCbo, sessionBrand.value);
              modelCbo = fillYachtModel(modelCbo, categoryCbo, brandCbo, modelCbo, sessionModel.value);
              break;
            }
        }
        break;
      }

    default:
      {

        if (updateWhat == "") {

          categoryCbo = fillYachtCategory(categoryCbo, categoryCbo, sessionCategory.value);
          brandCbo = fillYachtBrand(brandCbo, categoryCbo, brandCbo, sessionBrand.value);
          modelCbo = fillYachtModel(modelCbo, categoryCbo, brandCbo, modelCbo, sessionModel.value);

        } // 'updateWhat = ""
        break;
      }
  }
  return true;
}