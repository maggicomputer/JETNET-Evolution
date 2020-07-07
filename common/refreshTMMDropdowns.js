// ********************************************************************************
// Copyright 2004-11. JETNET,LLC. All rights reserved.
//
// $$Archive: /commonWebProject/common/refreshTMMDropdowns.js $
// $$Author: Mike $
// $$Date: 5/28/20 5:34p $
// $$Modtime: 5/28/20 5:11p $
// $$Revision: 13 $
// $$Workfile: refreshTMMDropdowns.js $
//
// ********************************************************************************

var nSelectedFilterCount = 0;
var marketSearchButton = "";

function setProductFilter(isFiltered, filteredBy) {

  var usage = "";
  var airframe = "";
  var rememberLastIndex = -1;
  var nCounter = 0;
  var filterArray = null;
  var tmpUsageArry = null;

  if (isFiltered) {

    // make temp array as big as masterArray
    try {

      // generate local array for client
      localFilterAirframeArray = new Array(localMasterAirframeArray.length);
      //alert("localFilterAirframeArray.length - " + localFilterAirframeArray.length);

      for (x = 0; x < localMasterAirframeArray.length; x++) {
        // generate an array for each dimension
        localFilterAirframeArray[x] = new Array(clientAIRFRAMEARRAY_DIM + 1);
        //alert("localFilterAirframeArray[" + x + "].length - " + localFilterAirframeArray[x].length);
      }

    }
    catch (err) {
      alert("err - " + err.description);
    }

    // determine how to filter based on what user has checked	  
    filterArray = filteredBy.split(",");

    for (var nfilterloop = 0; nfilterloop < filterArray.length; nfilterloop++) {

      switch (filterArray[nfilterloop]) {
        case "H":
          {
            // find all rotary airframe models
            if (airframe == "") {
              airframe = "R";
            }
            else {
              if (airframe.indexOf("R") == -1) {
                airframe = airframe + ",R";
              }
            }

            if (usage == "") {
              usage = "H";
            }
            else {
              if (usage.indexOf("H") == -1) {
                usage = usage + ",H";
              }
            }
          }
          break;

        case "B":
          {
            // find all fixed airframe models
            if (airframe == "") {
              airframe = "F";
            }
            else {
              if (airframe.indexOf("F") == -1) {
                airframe = airframe + ",F";
              }
            }

            if (usage == "") {
              usage = "B";
            }
            else {
              if (usage.indexOf("B") == -1) {
                usage = usage + ",B";
              }
            }
          }
          break;

        case "C":
          {
            // find all fixed airframe models
            if (airframe == "") {
              airframe = "F";
            }
            else {
              if (airframe.indexOf("F") == -1) {
                airframe = airframe + ",F";
              }
            }

            if (usage == "") {
              usage = "C";
            }
            else {
              if (usage.indexOf("C") == -1) {
                usage = usage + ",C";
              }
            }
          }
          break;

        case "R":
          {

            // find all fixed airframe models
            if (airframe == "") {
              airframe = "F";
            }
            else {
              if (airframe.indexOf("F") == -1) {
                airframe = airframe + ",F";
              }
            }

            if (usage == "") {
              usage = "R";
            }
            else {
              if (usage.indexOf("R") == -1) {
                usage = usage + ",R";
              }
            }

          }
          break;
      }

    } // nfilterloop

    // for H,B,C filter we are just moving from master array to filter array

    for (var afilterloop = 0; afilterloop < localMasterAirframeArray.length; afilterloop++) {
      if (airframe.indexOf(localMasterAirframeArray[afilterloop][LOCAIR_FRAME]) > -1) {

        for (var nAirframeLoop = 0; nAirframeLoop < filterArray.length; nAirframeLoop++) {

          tmpUsageArry = localMasterAirframeArray[afilterloop][LOCAIR_USAGE].split(",");

          for (var nUsageLoop = 0; nUsageLoop < tmpUsageArry.length; nUsageLoop++) {
            if (usage.indexOf(tmpUsageArry[nUsageLoop]) > -1) {

              // make sure we don't load the same index twice
              if (Number(localMasterAirframeArray[afilterloop][LOCAIR_INDEX]) != rememberLastIndex) {

                localFilterAirframeArray[nCounter][LOCAIR_INDEX] = localMasterAirframeArray[afilterloop][LOCAIR_INDEX];
                localFilterAirframeArray[nCounter][LOCAIR_TYPE] = localMasterAirframeArray[afilterloop][LOCAIR_TYPE];
                localFilterAirframeArray[nCounter][LOCAIR_MAKE] = localMasterAirframeArray[afilterloop][LOCAIR_MAKE];
                localFilterAirframeArray[nCounter][LOCAIR_MAKE_ABR] = localMasterAirframeArray[afilterloop][LOCAIR_MAKE_ABR];
                localFilterAirframeArray[nCounter][LOCAIR_MODEL] = localMasterAirframeArray[afilterloop][LOCAIR_MODEL];
                localFilterAirframeArray[nCounter][LOCAIR_MODEL_ID] = localMasterAirframeArray[afilterloop][LOCAIR_MODEL_ID];
                localFilterAirframeArray[nCounter][LOCAIR_USAGE] = localMasterAirframeArray[afilterloop][LOCAIR_USAGE];
                localFilterAirframeArray[nCounter][LOCAIR_FRAME] = localMasterAirframeArray[afilterloop][LOCAIR_FRAME];
                localFilterAirframeArray[nCounter][LOCAIR_MFRNAME] = localMasterAirframeArray[afilterloop][LOCAIR_MFRNAME];
                localFilterAirframeArray[nCounter][LOCAIR_SIZE] = localMasterAirframeArray[afilterloop][LOCAIR_SIZE];

                nCounter++;
                rememberLastIndex = Number(localMasterAirframeArray[afilterloop][LOCAIR_INDEX]);

              }

            } // (usage.indexOf(tmpUsageArry[nUsageLoop]) >= 0 )

          } // nUsageLoop
        } // nAirframeLoop
      } // (airframe.indexOf(localMasterAirframeArray[afilterloop][LOCAIR_FRAME]) >= 0 )
    } // nfilterloop
  }
  else {
    localFilterAirframeArray = null;  // take the filter off
  } // isFiltered
  return true;
}

function refreshTypeMakeModelByListBox(fromEvent, updateWhat, isHeliOnlyFlag, productCodeCount, viewProductCode) {

  var typeCbo = document.getElementById(typeCboName); // variables from the script on the page
  var makeCbo = document.getElementById(MakeCboName);  // variables from the script on the page
  var modelCbo = document.getElementById(ModelCboName); // variables from the script on the page

  var sessionType = document.getElementById("sessAircraftTypeID");   // grabs hidden value from page
  var sessionMake = document.getElementById("sessAircraftMakeID");   // grabs hidden value from page
  var sessionModel = document.getElementById("sessAircraftModelID"); // grabs hidden value from page

  var nSelectedFilter = "";

  // use dropdown for filtering
  if ((updateWhat == "filter") && (viewProductCode != null)) {

    document.getElementById("hasModelFilterID").value = "true";
    b_isFilteredJS = true;

    if (viewProductCode != null) {
      if (viewProductCode.value != "") {
        nSelectedFilter = viewProductCode.value;
      }
    }

    // if we have a filter apply it
    if ((nSelectedFilter != "") && (nSelectedFilter != "0")) {

      s_rememberLastFilterJS = "";

      if (nSelectedFilter == "3") { // helicopter
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

      if (nSelectedFilter == "1") { // business
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

      if (nSelectedFilter == "2") { // Commercial
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

      if (nSelectedFilter == "4") { // regional
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

      document.getElementById("sessAircraftTypeID").value = ""
      document.getElementById("sessAircraftMakeID").value = ""
      document.getElementById("sessAircraftModelID").value = ""

      sessionType = document.getElementById("sessAircraftTypeID");
      sessionMake = document.getElementById("sessAircraftMakeID");
      sessionModel = document.getElementById("sessAircraftModelID");

      document.getElementById("lastModelFilterID").value = s_rememberLastFilterJS;
      setProductFilter(b_isFilteredJS, s_rememberLastFilterJS);

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
      setProductFilter(b_isFilteredJS, "");

    }

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
      setProductFilter(b_isFilteredJS, "");

    } // ((fromEvent == "onClick") && (b_isFilteredJS) && (s_rememberLastFilterJS == "")) 

    if ((updateWhat == "") && (b_isFilteredJS)) {

      // find out which filter is set (we are comming in from a new page)
      if (viewProductCode != null) {
        if (viewProductCode.value != "") {
          nSelectedFilter = viewProductCode.value;
        }
      }

      // if we have a filter apply it
      if (nSelectedFilter != "") {

        s_rememberLastFilterJS = "";

        if (nSelectedFilter == "3") { // helicopter
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

        if (nSelectedFilter == "1") { // business
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

        if (nSelectedFilter == "2") { // Commercial
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

        if (nSelectedFilter == "4") { // regional
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

        document.getElementById("lastModelFilterID").value = s_rememberLastFilterJS;
        setProductFilter(b_isFilteredJS, s_rememberLastFilterJS);

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
        setProductFilter(b_isFilteredJS, "");

      } // nSelectedFilterCount > 0
    } // updateWhat = "" && b_isFilteredJS
  }

  switch (fromEvent) {
    case "onChange":
      {
        // need to reset both the dropdown selections and the session variables
        // this causes false selections when the user resets the dropdowns to "ALL"
        //

        if (typeCbo.options[0].selected == true) {

          if (makeCbo.options[0].selected == true) {
            makeCbo.options[0].selected = true;
          }

          if (modelCbo.options[0].selected == true) {
            modelCbo.options[0].selected = true;
          }

          sessionType.value = "";
        }

        if ((makeCbo.options[0].selected == true) && (typeCbo.options[0].selected == true)) {
          if (makeCbo.options[0].selected == true) {
            makeCbo.options[0].selected = true;
          }
          sessionMake.value = "";
        }

        if ((modelCbo.options[0].selected == true) && (makeCbo.options[0].selected == true) && (typeCbo.options[0].selected == true)) {
          if (modelCbo.options[0].selected == true) {
            modelCbo.options[0].selected = true;
          }
          sessionModel.value = "";
        }


        switch (updateWhat) {

          case "make":
            {
              modelCbo = fillAircraftModel(modelCbo, typeCbo, makeCbo, modelCbo, b_isFilteredJS, isHeliOnlyFlag, productCodeCount, sessionModel.value);
              break;
            }
          case "type":
            {
              makeCbo = fillAircraftMake(makeCbo, typeCbo, makeCbo, b_isFilteredJS, isHeliOnlyFlag, productCodeCount, sessionMake.value);
              modelCbo = fillAircraftModel(modelCbo, typeCbo, makeCbo, modelCbo, b_isFilteredJS, isHeliOnlyFlag, productCodeCount, sessionModel.value);
              break;
            }
        }
        break;
      }
    case "onClick":
      {

        switch (updateWhat) {
          case "type":
            {
              //makeCbo = fillAircraftMake(makeCbo, typeCbo, makeCbo, b_isFilteredJS, isHeliOnlyFlag, productCodeCount, sessionMake.value);
              //modelCbo = fillAircraftModel(modelCbo, typeCbo, makeCbo, modelCbo, b_isFilteredJS, isHeliOnlyFlag, productCodeCount, sessionModel.value);
              break;
            }

          case "filter":
            {
              typeCbo = fillAircraftType(typeCbo, typeCbo, b_isFilteredJS, productCodeCount, sessionType.value);
              makeCbo = fillAircraftMake(makeCbo, typeCbo, makeCbo, b_isFilteredJS, isHeliOnlyFlag, productCodeCount, sessionMake.value);
              modelCbo = fillAircraftModel(modelCbo, typeCbo, makeCbo, modelCbo, b_isFilteredJS, isHeliOnlyFlag, productCodeCount, sessionModel.value);
              break;
            }
        }
        break;
      }

    default:
      {

        if (updateWhat == "") {

          typeCbo = fillAircraftType(typeCbo, typeCbo, b_isFilteredJS, productCodeCount, sessionType.value);
          makeCbo = fillAircraftMake(makeCbo, typeCbo, makeCbo, b_isFilteredJS, isHeliOnlyFlag, productCodeCount, sessionMake.value);
          modelCbo = fillAircraftModel(modelCbo, typeCbo, makeCbo, modelCbo, b_isFilteredJS, isHeliOnlyFlag, productCodeCount, sessionModel.value);

        } // 'updateWhat = ""
        break;
      }
  }
  return true;
}

function refreshTypeMakeModelByCheckBox(fromEvent, updateWhat, isHeliOnlyFlag, productCodeCount) {

  var typeCbo = document.getElementById(typeCboName);
  var makeCbo = document.getElementById(MakeCboName);
  var modelCbo = document.getElementById(ModelCboName);

  var mfrNamesCbo = document.getElementById(mfrNamesCboName);
  var acSizeCbo = document.getElementById(sizeCboName);

  var sessionType = document.getElementById("sessAircraftTypeID");
  var sessionMake = document.getElementById("sessAircraftMakeID");
  var sessionModel = document.getElementById("sessAircraftModelID");

  var sessionMfrNames = document.getElementById("sessAircraftMfrNamesID");
  var sessionAcSize = document.getElementById("sessAircraftSizeID");

  var nfilterloop = 0;
  var filterArray = null;

  //alert("fromEvent: " + fromEvent + " updateWhat: " + updateWhat + "\n\n heliOnlyFlag: " + isHeliOnlyFlag + " productCodeCount: " + productCodeCount + "\n\n isFilter: " + b_isFilteredJS);

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
      setProductFilter(b_isFilteredJS, s_rememberLastFilterJS);

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
      setProductFilter(b_isFilteredJS, "");

    }

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
      setProductFilter(b_isFilteredJS, "");

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
        setProductFilter(b_isFilteredJS, s_rememberLastFilterJS);

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
        setProductFilter(b_isFilteredJS, "");

      } // nSelectedFilterCount > 0

    } // updateWhat = "" && b_isFilteredJS
  }

  switch (fromEvent) {
    case "onChange":
      {
        // need to reset both the dropdown selections and the session variables
        // this causes false selections when the user resets the dropdowns to "ALL"

        if (typeCbo.options[0].selected == true) {

          if (makeCbo.options[0].selected == true) {
            makeCbo.options[0].selected = true;
          }

          if (modelCbo.options[0].selected == true) {
            modelCbo.options[0].selected = true;
          }

          sessionType.value = "";
        }

        if ((makeCbo.options[0].selected == true) && (typeCbo.options[0].selected == true)) {
          if (makeCbo.options[0].selected == true) {
            makeCbo.options[0].selected = true;
          }
          sessionMake.value = "";
        }

        if ((modelCbo.options[0].selected == true) && (makeCbo.options[0].selected == true) && (typeCbo.options[0].selected == true)) {
          if (modelCbo.options[0].selected == true) {
            modelCbo.options[0].selected = true;
          }
          sessionModel.value = "";
        }

        if (mfrNamesCbo != null) {
          if (mfrNamesCbo.options[0].selected == true) {
            sessionMfrNames.value = "";
          }
        }

        if (acSizeCbo != null) {
          if (acSizeCbo.options[0].selected == true) {
            sessionAcSize.value = "";
          }
        }

        switch (updateWhat) {

          case "make":
            {
              modelCbo = fillAircraftModel(modelCbo, typeCbo, makeCbo, modelCbo, b_isFilteredJS, isHeliOnlyFlag, sessionModel.value);

              if ((mfrNamesCbo != null) && (acSizeCbo != null)) {
                mfrNamesCbo = fillMfrName(mfrNamesCbo, mfrNamesCbo, sessionMfrNames.value, s_rememberLastFilterJS, b_isFilteredJS, typeCbo, makeCbo);
                acSizeCbo = fillAcSize(acSizeCbo, acSizeCbo, sessionAcSize.value, s_rememberLastFilterJS, b_isFilteredJS, typeCbo, makeCbo);
              }

              break;
            }
          case "type":
            {
              makeCbo = fillAircraftMake(makeCbo, typeCbo, makeCbo, b_isFilteredJS, isHeliOnlyFlag, sessionMake.value);
              modelCbo = fillAircraftModel(modelCbo, typeCbo, makeCbo, modelCbo, b_isFilteredJS, isHeliOnlyFlag, sessionModel.value);

              if ((mfrNamesCbo != null) && (acSizeCbo != null)) {
                mfrNamesCbo = fillMfrName(mfrNamesCbo, mfrNamesCbo, sessionMfrNames.value, s_rememberLastFilterJS, b_isFilteredJS, typeCbo, makeCbo);
                acSizeCbo = fillAcSize(acSizeCbo, acSizeCbo, sessionAcSize.value, s_rememberLastFilterJS, b_isFilteredJS, typeCbo, makeCbo);
              }

              break;
            }
          default:
            {
              // update both mfrName and acSize listboxes based on filter (H,B,C,R)
              if ((mfrNamesCbo != null) && (acSizeCbo != null)) {
                mfrNamesCbo = fillMfrName(mfrNamesCbo, mfrNamesCbo, sessionMfrNames.value, s_rememberLastFilterJS, b_isFilteredJS, typeCbo, makeCbo);
                acSizeCbo = fillAcSize(acSizeCbo, acSizeCbo, sessionAcSize.value, s_rememberLastFilterJS, b_isFilteredJS, typeCbo, makeCbo);
              }
              break;
            }
        }
        break;
      }
    case "onClick":
      {

        switch (updateWhat) {
          case "type":
            {
              //makeCbo = fillAircraftMake(makeCbo, typeCbo, makeCbo, b_isFilteredJS, isHeliOnlyFlag, sessionMake.value);
              //modelCbo = fillAircraftModel(modelCbo, typeCbo, makeCbo, modelCbo, b_isFilteredJS, isHeliOnlyFlag, sessionModel.value);

              break;
            }

          case "filter":
            {
              typeCbo = fillAircraftType(typeCbo, typeCbo, b_isFilteredJS, productCodeCount, sessionType.value);
              makeCbo = fillAircraftMake(makeCbo, typeCbo, makeCbo, b_isFilteredJS, isHeliOnlyFlag, sessionMake.value);
              modelCbo = fillAircraftModel(modelCbo, typeCbo, makeCbo, modelCbo, b_isFilteredJS, isHeliOnlyFlag, sessionModel.value);

              // update both mfrName and acSize listboxes based on filter (H,B,C,R)
              if ((mfrNamesCbo != null) && (acSizeCbo != null)) {
                mfrNamesCbo = fillMfrName(mfrNamesCbo, mfrNamesCbo, sessionMfrNames.value, s_rememberLastFilterJS, b_isFilteredJS, typeCbo, makeCbo);
                acSizeCbo = fillAcSize(acSizeCbo, acSizeCbo, sessionAcSize.value, s_rememberLastFilterJS, b_isFilteredJS, typeCbo, makeCbo);
              }

              break;
            }
        }
        break;
      }

    default:
      {

        if (document.getElementById(chkDefaultFilterID) != null) {

          if (document.getElementById(chkDefaultFilterID).checked == true) {

            //  set any selected values to all[0], then set session(type,make,model) to "default aircraft" values
            modelCbo.options[0].selected = true;
            makeCbo.options[0].selected = true;
            typeCbo.options[0].selected = true;
            sessionType.value = "";
            sessionMake.value = "";
            sessionModel.value = "";

            var tmpMasterModelID = "";
            var tmpDefaultModelID = "";

            var rememberLastType = "";
            var rememberLastMake = "";

            if (((localMasterAirframeArray != null) && (localMasterAirframeArray.length > 0)) && ((localDefaultAirframeArray != null) && (localDefaultAirframeArray.length > 0))) {

              for (var xMasterLoop = 0; xMasterLoop < localMasterAirframeArray.length; xMasterLoop++) {

                tmpMasterModelID = localMasterAirframeArray[xMasterLoop][LOCAIR_MODEL_ID]

                for (var xDefaultLoop = 0; xDefaultLoop < localDefaultAirframeArray.length; xDefaultLoop++) {

                  tmpDefaultModelID = localDefaultAirframeArray[xDefaultLoop][LOCAIR_MODEL_ID]

                  if (Number(tmpDefaultModelID) == Number(tmpMasterModelID)) { // if model id on default array matches this model from the master

                    if (rememberLastType != localMasterAirframeArray[xMasterLoop][LOCAIR_TYPE]) {

                      // only add the type if its different than last type
                      if (sessionType.value == "") {
                        sessionType.value = localMasterAirframeArray[xMasterLoop][LOCAIR_INDEX];
                      }
                      else {
                        if (sessionType.value.indexOf(localMasterAirframeArray[xMasterLoop][LOCAIR_INDEX]) == -1) {
                          sessionType.value = sessionType.value + "," + localMasterAirframeArray[xMasterLoop][LOCAIR_INDEX];
                        }
                      }

                      rememberLastType = localMasterAirframeArray[xMasterLoop][LOCAIR_TYPE];

                    }

                    if (rememberLastMake != localMasterAirframeArray[xMasterLoop][LOCAIR_MAKE]) {

                      // only add the make if its different than last make
                      if (sessionMake.value == "") {
                        sessionMake.value = localMasterAirframeArray[xMasterLoop][LOCAIR_INDEX];
                      }
                      else {
                        if (sessionMake.value.indexOf(localMasterAirframeArray[xMasterLoop][LOCAIR_INDEX]) == -1) {
                          sessionMake.value = sessionMake.value + "," + localMasterAirframeArray[xMasterLoop][LOCAIR_INDEX];
                        }
                      }

                      rememberLastMake = localMasterAirframeArray[xMasterLoop][LOCAIR_MAKE];
                    }

                    if (sessionModel.value == "") {
                      sessionModel.value = localMasterAirframeArray[xMasterLoop][LOCAIR_INDEX];
                    }
                    else {
                      if (sessionModel.value.indexOf(localMasterAirframeArray[xMasterLoop][LOCAIR_INDEX]) == -1) {
                        sessionModel.value = sessionModel.value + "," + localMasterAirframeArray[xMasterLoop][LOCAIR_INDEX];
                      }
                    }

                  }

                } // for (var xDefaultLoop = 0; xDefaultLoop < localMasterAirframeArray.length; xDefaultLoop++)

              } // for (var xMasterLoop = 0; xMasterLoop < localMasterAirframeArray.length; xMasterLoop++)

            } // if (((localMasterAirframeArray != null) && (localMasterAirframeArray.length > 0)) && ((localDefaultAirframeArray != null) && (localDefaultAirframeArray.length > 0)))

          } // if (document.getElementById(chkDefaultFilterID).checked == true)

        } // if (document.getElementById(chkDefaultFilterID) != null)

        if (updateWhat == "") {

          typeCbo = fillAircraftType(typeCbo, typeCbo, b_isFilteredJS, productCodeCount, sessionType.value);
          makeCbo = fillAircraftMake(makeCbo, typeCbo, makeCbo, b_isFilteredJS, isHeliOnlyFlag, sessionMake.value);
          modelCbo = fillAircraftModel(modelCbo, typeCbo, makeCbo, modelCbo, b_isFilteredJS, isHeliOnlyFlag, sessionModel.value);

          // update both mfrName and acSize listboxes based on filter (H,B,C,R)
          if ((mfrNamesCbo != null) && (acSizeCbo != null)) {
            mfrNamesCbo = fillMfrName(mfrNamesCbo, mfrNamesCbo, sessionMfrNames.value, s_rememberLastFilterJS, b_isFilteredJS, typeCbo, makeCbo);
            acSizeCbo = fillAcSize(acSizeCbo, acSizeCbo, sessionAcSize.value, s_rememberLastFilterJS, b_isFilteredJS, typeCbo, makeCbo);
          }

        } // 'updateWhat = ""
        break;
      }

  }


  // check for market summary page 

  $(document).ready(function () {
    try {

      if (marketSearchButton != '') {
        if ((typeof (document.getElementById(marketSearchButton)) != "undefined") && (document.getElementById(marketSearchButton) != null)) {
          // alert("on market summary page nSelectedFilterCount[" + nSelectedFilterCount + "]");
          if (nSelectedFilterCount == 1) {
            document.getElementById(marketSearchButton).style.visibility = "visible";
            document.getElementById("ProdWarningID").style.visibility = "hidden";
          } else {
            document.getElementById(marketSearchButton).style.visibility = "hidden";
            document.getElementById("ProdWarningID").style.visibility = "visible";
          }
        }
      }
    }
    catch (err) {
    }

  });


  return true;
}