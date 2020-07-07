// ********************************************************************************
// Copyright 2004-11. JETNET,LLC. All rights reserved.
//
// $$Archive: /commonWebProject/common/eventSearchScript.js $
// $$Author: Mike $
// $$Date: 6/19/19 8:45a $
// $$Modtime: 6/18/19 6:12p $
// $$Revision: 2 $
// $$Workfile: eventSearchScript.js $
//
// ********************************************************************************
  
function fillEventCategoryJS(inEventTypeCbo, inSelected, sessCatType) {

  var bSelectedItem = false;
  var bfoundSelection = false;

  var nCurrentOption = 0;

  var displayCboJS = null;
  var optionArray = null;
  var localArray = null;

  var justACArray = null;

  var sSelectionStr = "";
  var sRememberType = "";

  var excludeCode = "";
  var excludeCode1 = "";

  if ((localAryEventCategory != null) && (localAryEventCategory != "")) {
    localArray = localAryEventCategory;
  }

  justACArray = sessCatType.split("!");
  if (justACArray.length > 1) {

    excludeCode = justACArray[0];
    excludeCode1 = justACArray[1];
    sessCatType = justACArray[2];
    
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

      if (bSelectedItem && justACArray.length == 1) {
        optionArray = sSelectionStr.split(",");
      }
      else {
        if (inSelected.length > 1 && justACArray.length == 1) {
          optionArray = ("All").split(",");
        }
        else {
          if ((sessCatType != null) && (sessCatType != "")) {
            var remove = /, /gi;
            sSelectionStr = sessCatType.replace(remove, ",");
            optionArray = sSelectionStr.split(",");
          }
        } // (optionList.length > 1)
      } // (bSelectedItem)
    } // (sSelectionStr != "") 

  } // ((typeof(inSelected.name) != "undefined") && (inSelected != ""))

  displayCboJS = inEventTypeCbo;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  for (var x = 0; x < localArray.length; x++) {
    if ((localArray[x][PRIOREVCAT_CATEGORY] != "") &&
        (localArray[x][PRIOREVCAT_CATEGORY].toUpperCase() != sRememberType.toUpperCase()) &&
        ((localArray[x][PRIOREVCAT_CATEGORY_CODE].toUpperCase() != excludeCode.toUpperCase()) ||
        (localArray[x][PRIOREVCAT_CATEGORY_CODE].toUpperCase() != excludeCode1.toUpperCase()))) {

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
        displayCboJS.options[nCurrentOption] = new Option(localArray[x][PRIOREVCAT_CATEGORY]);
        displayCboJS.options[nCurrentOption].value = localArray[x][PRIOREVCAT_CATEGORY];
        displayCboJS.options[nCurrentOption].innerHTML = localArray[x][PRIOREVCAT_CATEGORY];

        if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
          if (optionArray.length > 0) {
            if (inClientArrayJS(optionArray, localArray[x][PRIOREVCAT_CATEGORY])) {
              displayCboJS.options[nCurrentOption].selected = true;
              displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
              bfoundSelection = true;
            }
          }
        }
      }
      else {

        displayCboJS.options[nCurrentOption] = new Option(localArray[x][PRIOREVCAT_CATEGORY]);
        displayCboJS.options[nCurrentOption].value = localArray[x][PRIOREVCAT_CATEGORY];
        displayCboJS.options[nCurrentOption].innerHTML = localArray[x][PRIOREVCAT_CATEGORY];

        if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
          if (optionArray.length > 0) {
            if (inClientArrayJS(optionArray, localArray[x][PRIOREVCAT_CATEGORY])) {
              displayCboJS.options[nCurrentOption].selected = true;
              displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
              bfoundSelection = true;
            }
          }
        }
      } // nCurrentOption == 0

      nCurrentOption = nCurrentOption + 1;
      sRememberType = localArray[x][PRIOREVCAT_CATEGORY];

    } // ((localArray[x][PRIOREVCAT_CATEGORY] != "") && (localArray[x][PRIOREVCAT_CATEGORY].toUpperCase() != sRememberType.toUpperCase()))
  } // (var x = 0; x < localArray.length; x++)

  if ((!bfoundSelection) && (displayCboJS.options.length > 1)) {
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
  }

  localArray = null;
  optionArray = null;

  return displayCboJS;
}
  	
function fillEventCategoryCodeJS(inEventCodeCbo, inSelEventCategory, inSelected, sessCatCode) {

  var bSelectedItem = false;
  var bfoundSelection = false;

  var nCurrentOption = 0;
  var nEvent = 0;

  var displayCboJS = null;
  var displayArray = null;
  var optionArray = null;
  var localArray = null;

  var justACArray = null;

  var bAllType = false;

  var sEventTypeCbo = "";
  var sRememberCatCode = "";

  var sSelectionStr = "";
  var sTempEventCategory = "";

  if ((localAryEventCategory != null) && (localAryEventCategory != "")) {
    localArray = localAryEventCategory;
  }
      
  justACArray = sessCatCode.split("!");

  if (justACArray.length > 1) {

    sessCatCode = justACArray[1];

  }
  
  // get the list of selected Content/Regions
  if ((typeof (inSelEventCategory.name) != "undefined") && (inSelEventCategory != null)) {
    for (var nloop = 0; nloop < inSelEventCategory.length; nloop++) {
      if ((inSelEventCategory.options[nloop].selected == true) || (bAllType == true)) {
        if (nloop == 0) {
          bAllType = true;
        }
        else {
          if (sEventTypeCbo == "") {
            sEventTypeCbo = inSelEventCategory.options[nloop].value;
          }
          else {
            sEventTypeCbo = sEventTypeCbo + "," + inSelEventCategory.options[nloop].value;
          }
        } // (nloop == 0)
      } // ((inSelEventCategory.options[nloop].selected == true) || (bAllType == true))
    } // (nloop = 0; nloop < inSelEventCategory.length; nloop++)
  }
  else {
    bAllType = true;
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

      if (bSelectedItem && justACArray.length == 1) {
        optionArray = sSelectionStr.split(",");
      }
      else {
        if (inSelected.length > 1 && justACArray.length == 1) {
          optionArray = ("All").split(",");
        }
        else {
          if ((sessCatCode != null) && (sessCatCode != "") && (sessCatCode.indexOf("AAAA") == -1)) {

            var remove = /, /gi;
            sSelectionStr = sessCatCode.replace(remove, ",");
            optionArray = sSelectionStr.split(",");

          } 
          else {

            optionArray = ("All").split(",");
            
          }
        } // (optionList.length > 1)
      } // (bSelectedItem)
    } // (sSelectionStr != "") 

  } // ((typeof(inSelected.name) != "undefined") && (inSelected != ""))

  displayCboJS = inEventCodeCbo;

  displayCboJS.options.length = 0;
  displayCboJS.options[nCurrentOption] = new Option("");
  displayCboJS.options[nCurrentOption].innerHTML = "";

  if ((sEventTypeCbo != "") && !bAllType) {
    displayArray = sEventTypeCbo.split(",");
  } 
  else {
	  displayArray = ("All").split(",");
	  bAllType = true;
  } // ((sEventTypeCbo != "") && !bAllType)

  if (bAllType) {
    displayCboJS.options[0].innerHTML = "All";
    displayCboJS.options[0].value = "All";
    displayCboJS.options[0].selected = true;
    displayCboJS.options[0].selectedindex = 0;
    return displayCboJS;
  }

  for (var x = 0; x < displayArray.length; x++) {

    if (displayArray[x].toUpperCase() != "ALL") {
      sTempEventCategory = displayArray[x];
    }

    for (var z = 0; z < localArray.length; z++) {

      if (localArray[z][PRIOREVCAT_CATEGORY].toUpperCase() == sTempEventCategory.toUpperCase()) {

        if (justACArray.length > 1 && (sessCatCode.indexOf("AAAA") == -1)) {

          if (localArray[z][PRIOREVCAT_CATEGORY_CODE].toUpperCase() == sessCatCode.toUpperCase()) {

            displayCboJS.options[0].value = localArray[z][PRIOREVCAT_CATEGORY_CODE];
            displayCboJS.options[0].innerHTML = localArray[z][PRIOREVCAT_CATEGORY_NAME];

            if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
              if (optionArray.length > 0) {
                if (inClientArrayJS(optionArray, localArray[z][PRIOREVCAT_CATEGORY_CODE])) {
                  displayCboJS.options[0].selected = true;
                  displayCboJS.options[0].selectedindex = nCurrentOption;
                  bfoundSelection = true;
                }
              }
            }

            break;
          }

        }
        else {

          if ((localArray[z][PRIOREVCAT_CATEGORY_CODE] != "") &&
            (localArray[z][PRIOREVCAT_CATEGORY_CODE].toUpperCase() != sRememberCatCode.toUpperCase())) {

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
              displayCboJS.options[nCurrentOption] = new Option(localArray[z][PRIOREVCAT_CATEGORY_CODE]);
              displayCboJS.options[nCurrentOption].value = localArray[z][PRIOREVCAT_CATEGORY_CODE];
              displayCboJS.options[nCurrentOption].innerHTML = localArray[z][PRIOREVCAT_CATEGORY_NAME];

              if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                if (optionArray.length > 0) {
                  if (inClientArrayJS(optionArray, localArray[z][PRIOREVCAT_CATEGORY_CODE])) {
                    displayCboJS.options[nCurrentOption].selected = true;
                    displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                    bfoundSelection = true;
                  }
                }
              }
            }
            else {

              displayCboJS.options[nCurrentOption] = new Option(localArray[z][PRIOREVCAT_CATEGORY_CODE]);
              displayCboJS.options[nCurrentOption].value = localArray[z][PRIOREVCAT_CATEGORY_CODE];
              displayCboJS.options[nCurrentOption].innerHTML = localArray[z][PRIOREVCAT_CATEGORY_NAME];

              if ((sSelectionStr != "") && (sSelectionStr.toUpperCase() != "ALL")) {
                if (optionArray.length > 0) {
                  if (inClientArrayJS(optionArray, localArray[z][PRIOREVCAT_CATEGORY_CODE])) {
                    displayCboJS.options[nCurrentOption].selected = true;
                    displayCboJS.options[nCurrentOption].selectedindex = nCurrentOption;
                    bfoundSelection = true;
                  }
                }
              }
            } // nCurrentOption == 0

            nCurrentOption = nCurrentOption + 1;
            sRememberCatCode = localArray[z][PRIOREVCAT_CATEGORY_CODE];

          } // ((localArray[z][PRIOREVCAT_CATEGORY_CODE] != "") && (localArray[z][PRIOREVCAT_CATEGORY_CODE].toUpperCase() != sRememberCatCode.toUpperCase()))
        } // (justACArray.length > 1) 
      } // (localArray[z][PRIOREVCAT_CATEGORY].toUpperCase() == sTempEventCategory.toUpperCase())  
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

  displayArray = null;
  optionArray = null;
  localArray = null;
  
  return displayCboJS;

}

function refreshEventCombosJS(fromEvent, updateWhat, sessCatType, sessCatCode) {

  var typeCbo = null;
  var codeCbo = null;

  typeCbo = document.getElementById(eventCatTypeCboName);
  codeCbo = document.getElementById(eventCatTypeCodeCboName);

  //alert("fromEvent: " + fromEvent + " updateWhat: " + updateWhat + "\n\n sessCatType: " + sessCatType + " sessCatCode: " + sessCatCode);
  
  switch (fromEvent) {
    case "onchange":
      {
        switch (updateWhat) {
          case "code":
            {
              //codeCbo = fillEventCategoryCodeJS(codeCbo, typeCbo, codeCbo, sessCatCode);
              break;
            }
          case "cat":
            {
              codeCbo = fillEventCategoryCodeJS(codeCbo, typeCbo, codeCbo, sessCatCode);
              break;
            }
        }
        break;
      }
    case "onclick":
      {
        switch (updateWhat) {   //
          case "air":
            {
              document.getElementById("radEventsValueID").value = "AIRCRAFT";
              
              var msgList = $("#eventsMsgID");
              msgList.html("");
              $("<div/>", {
                "class": "red_text",
                text: ""
              }).appendTo(msgList);
             
              typeCbo = fillEventCategoryJS(typeCbo, typeCbo, "CFNC!NEWWA!Aircraft Information");
              codeCbo = fillEventCategoryCodeJS(codeCbo, typeCbo, codeCbo, "!AAAA");
              break;
            }
          case "wnt":
            {
              document.getElementById("radEventsValueID").value = "WANTED";

              var msgList = $("#eventsMsgID");
              msgList.html("");
              $("<div/>", {
                "class": "red_text",
                text: "When running a Wanted Event Search, Individual Aircraft search fields do not apply."
              }).appendTo(msgList);

              typeCbo = fillEventCategoryJS(typeCbo, typeCbo, "!!Company/Contact");
              codeCbo = fillEventCategoryCodeJS(codeCbo, typeCbo, codeCbo, "!NEWWA");
              break;
            }
          case "cmp":
            {
              document.getElementById("radEventsValueID").value = "COMPANY";

              var msgList = $("#eventsMsgID");
              msgList.html("");
              $("<div/>", {
                "class": "red_text",
                text: "When running a Company Event Search, Model and Aircraft search fields do not apply."
              }).appendTo(msgList);

              typeCbo = fillEventCategoryJS(typeCbo, typeCbo, "!!Company/Contact");
              codeCbo = fillEventCategoryCodeJS(codeCbo, typeCbo, codeCbo, "!CFNC");
              break;
            }
        }
        break;
      }

    default:
      {

        if ((updateWhat == "") && ((sessCatType != "") || (sessCatCode != ""))) {

          typeCbo = fillEventCategoryJS(typeCbo, typeCbo, sessCatType);
          codeCbo = fillEventCategoryCodeJS(codeCbo, typeCbo, codeCbo, sessCatCode);

          switch (document.getElementById("radEventsValueID").value.toUpperCase()) {
           case "AIRCRAFT":
            {
              document.getElementById("radEventsID").checked = true
              
              var msgList = $("#eventsMsgID");
              msgList.html("");
              $("<div/>", {
                "class": "red_text",
                text: ""
              }).appendTo(msgList);

              break;
            }
          case "WANTED":
            {
              document.getElementById("radEventsID1").checked = true

              var msgList = $("#eventsMsgID");
              msgList.html("");
              $("<div/>", {
                "class": "red_text",
                text: "When running a Wanted Event Search, Individual Aircraft search fields do not apply."
              }).appendTo(msgList);

              break;
            }
          case "COMPANY":
            {
              document.getElementById("radEventsID2").checked = true
              
              var msgList = $("#eventsMsgID");
              msgList.html("");
              $("<div/>", {
                "class": "red_text",
                text: "When running a Company Event Search, Model and Aircraft search fields do not apply."
              }).appendTo(msgList);

              break;
            }
          }
          
        }
        else {
          if ((updateWhat == "") && (sessCatType == "") && (sessCatCode == "")) {
            
            document.getElementById("radEventsID").checked = true

            typeCbo = fillEventCategoryJS(typeCbo, typeCbo, sessCatType);
            codeCbo = fillEventCategoryCodeJS(codeCbo, typeCbo, codeCbo, sessCatCode);
          }
        } // 'updateWhat = ""

        break;
      }
  }

  //MultiListEnsureItemVisible();
  
  $(document).ready(function(){
    SwapPageDependingOnEventType(document.getElementById("radEventsValueID").value);
  });
  
}
