/* 
 ********************************************************************************
 Copyright 2004-11. JETNET,LLC. All rights reserved.

 $$Archive: /commonWebProject/common/marketPickDateScript.js $
 $$Author: Mike $
 $$Date: 6/19/19 8:45a $
 $$Modtime: 6/18/19 6:12p $
 $$Revision: 2 $
 $$Workfile: marketPickDateScript.js $

 ********************************************************************************
*/

function get_quarter_for_monthJS(inMonth) {

  switch (inMonth) {
    case 0:
    case 1:
    case 2:
      {
        return "Q1";
        break;
      }
    case 3:
    case 4:
    case 5:
      {
        return "Q2";
        break;
      }
    case 6:
    case 7:
    case 8:
      {
        return "Q3";
        break;
      }
    case 9:
    case 10:
    case 11:
      {
        return "Q4";
        break;
      }
  }

}

function get_firstMonth_for_quarterJS(inQuarter) {

  switch (inQuarter) {
    case "Q1":
      {
        return 0;
        break;
      }
    case "Q2":
      {
        return 3;
        break;
      }
    case "Q3":
      {
        return 6;
        break;
      }
    case "Q4":
      {
        return 9;
        break;
      }
  }

}

function return_default_summary_rangeJS(sTimeScale) {

  switch (sTimeScale) {
    case "Years":
      {
        return 5;
        break;
      }
    case "Months":
      {
        return 6;
        break;
      }
    case "Days":
      {
        return 15;
        break;
      }
    case "Quarters":
      {
        return 4;
        break;
      }
    default:
      {
        return 6;
        break;
      }
  }

}

function return_total_rangeJS(sTimeScale) {

  switch (sTimeScale) {
    case "Years":
      {
        return 10;
        break;
      }
    case "Months":
      {
        return 12;
        break;
      }
    case "Days":
      {
        return 31;
        break;
      }
    case "Quarters":
      {
        return 12;
        break;
      }
    default:
      {
        return 12;
        break;
      }
  }

}

function fillStartDateJS(in_who_triggered, isOnlyHeli, isOnlyBusiness, isOnlyCommercial) {

  var cboTimeScale = document.getElementById(timeScaleCboName);
  var cboStartDate = document.getElementById(startDateCboName);
  var cboRangeSpan = document.getElementById(displayRangeCboName);  

  var sessTimeScale = document.getElementById("sessTimeScaleID");   // grabs hidden value from page
  var sessScaleSets = document.getElementById("sessDisplayRangeID"); // grabs hidden value from page
  var sessMktStartDate = document.getElementById("sessStartDateID");   // grabs hidden value from page
  
  var MAXARRAYDIM = 2;
  var displayITEM = 0;
  var displayITEMNAME = 1;

  var nScaleSet = Number(sessScaleSets.value);

  var displayCbo = null;
  var displayArray = null;

  var dtMonthSeed = new Date();
  dtMonthSeed.setFullYear(1989, 9, 1); 
  var dtYearSeed = new Date();
  dtYearSeed.setFullYear(1990, 0, 1); 

  var dtHeliMonthSeed = new Date();
  dtHeliMonthSeed.setFullYear(2006, 0, 1);
  var dtHeliYearSeed = new Date();
  dtHeliYearSeed.setFullYear(2006, 0, 1); 

  var dtEndDate = new Date();
  var dtStartDate = new Date();
  
  var sSelStartDate = "";
  var selectedTimeScale = "";

  var dtSelected = new Date();
  
  var nCounter = 0;

  for (var nloop = 0; nloop < cboTimeScale.length; nloop++) {
    if (cboTimeScale.options[nloop].selected == true) {
      sSelTimeScale = cboTimeScale.options[nloop].value;
      break;
    } // (cboTimeScale.options[nloop].selected == true)
  } // (nloop = 0; nloop < cboTimeScale.length; nloop++)

  for (nloop = 0; nloop < cboStartDate.length; nloop++) {
    if (cboStartDate.options[nloop].selected == true) {
      sSelStartDate = cboStartDate.options[nloop].value;
      break;
    } // (cboStartDate.options[nloop].selected == true)
  } // (nloop = 0; nloop < cboStartDate.length; nloop++)

  if (nScaleSet == 0) {
    nScaleSet = return_default_summary_rangeJS(sSelTimeScale)
  }
  else {
    if ((nScaleSet != return_default_summary_rangeJS(sSelTimeScale)) && (in_who_triggered == "scale")) {
      nScaleSet = return_default_summary_rangeJS(sSelTimeScale)
    }
  }

  if (sSelStartDate != "") {
    if (in_who_triggered != "scale") {
      dtSelected = new Date(cDate(sSelStartDate));
    }
    else {
      // Default selection date always be (current date - nScaleSet)
      switch (sSelTimeScale) {
        case "Years":
          {
            dtSelected.setFullYear(dtStartDate.getFullYear() - nScaleSet);
            break;
          }
        case "Months":
          {
            dtSelected.setMonth(dtStartDate.getMonth() - nScaleSet);
            break;
          }
        case "Days":
          {
            dtSelected.setDate(dtStartDate.getDate() - nScaleSet);
            break;
          }
        case "Quarters":
          {
            dtSelected.setMonth(get_firstMonth_for_quarterJS(get_quarter_for_monthJS(dtStartDate.getMonth())) - (nScaleSet * 3));
            break;
          }
      } 
    }
  }
  else {
    if ((sessMktStartDate.value != "") && (in_who_triggered != "scale")) {
      dtSelected = new Date(cDate(sessMktStartDate.value));
    }
    else {
      // Default selection date always be (current date - nScaleSet)
      switch (sSelTimeScale) {
        case "Years":
          {
            dtSelected.setFullYear(dtStartDate.getFullYear() - nScaleSet);
            break;
          }
        case "Months":
          {
            dtSelected.setMonth(dtStartDate.getMonth() - nScaleSet);
            break;
          }
        case "Days":
          {
            dtSelected.setDate(dtStartDate.getDate() - nScaleSet);
            break;
          }
        case "Quarters":
          {
            dtSelected.setMonth(get_firstMonth_for_quarterJS(get_quarter_for_monthJS(dtStartDate.getMonth())) - (nScaleSet * 3));
            break;
          }
      } 
    }
  }
        
  displayCboJS = cboStartDate;

  displayCboJS.options.length = 0;
  displayCboJS.options[0] = new Option("");
  displayCboJS.options[0].innerHTML = "";

  switch (sSelTimeScale) {
    case "Years":
      {
        // 2011,2010,2009,2008,2007,2006,2005,2004,2003,2002,2001
        if (isOnlyHeli) {
          dtStartDate = dtHeliYearSeed;
        }
        else {
          dtStartDate = dtYearSeed;
        }

        var yearDiff = dateDiff("yyyy", dtStartDate, dtEndDate);

        displayArray = new Array(yearDiff);
        for (var x = 0; x < displayArray.length; x++) {
          // generate an array for each dimension
          displayArray[x] = new Array(MAXARRAYDIM);
          //alert("displayArray[" + x + "].length - " + displayArray[x].length);
        }

        // find total years up to the year before the current year
        for (var y = dtStartDate.getFullYear(); y < dtEndDate.getFullYear(); y++) {
          displayArray[nCounter][displayITEM] = formatDateTime(cDate("1/1/" + y), vbShortDate);
          displayArray[nCounter][displayITEMNAME] = y;
          nCounter++;
        }
        
        // reverse the string for display
        // 2001,2002,2003,2004,2005,2006,2007,2008,2009,2010,2011

        nCounter = 0;
        for (y = (displayArray.length - 1); y > -1; y--) {

          displayCboJS.options[nCounter] = new Option(displayArray[y][displayITEM]);
          displayCboJS.options[nCounter].value = displayArray[y][displayITEM];
          displayCboJS.options[nCounter].innerHTML = displayArray[y][displayITEMNAME];

          if (dtSelected == "") {
            if (nCounter == 0) {
              displayCboJS.options[0].selected = true;
              displayCboJS.options[0].selectedindex = 0;
            }
          }
          else {

            var tmpDateSelect = new Date(dtSelected.getFullYear(), 0, 1);
            var tmpDateArray = new Date(cDate(displayArray[y][displayITEM]));

            if (tmpDateArray.toString() == tmpDateSelect.toString()) {
              displayCboJS.options[nCounter].selected = true;
              displayCboJS.options[nCounter].selectedindex = nCounter;
            }
          }

          nCounter++;
        }
          
        break;
      }
    case "Months":
      {

        // 3/2012,2/2012,1/2012,12/2011,11/2011,10/2011,9/2011....
        if (isOnlyHeli) {
          dtStartDate = dtHeliMonthSeed;
        }
        else {
          dtStartDate = dtMonthSeed;
        }

        var nTmpMonth = dtStartDate;

        var monthDiff = dateDiff("m", dtStartDate, dtEndDate);

        displayArray = new Array(monthDiff);
        for (x = 0; x < displayArray.length; x++) {
          // generate an array for each dimension
          displayArray[x] = new Array(MAXARRAYDIM);
          //alert("displayArray[" + x + "].length - " + displayArray[x].length);
        }

        // find total months up to the month before the current month
        for (y = 0; y < monthDiff; y++) {
          displayArray[nCounter][displayITEM] = formatDateTime(cDate(nTmpMonth), vbShortDate);
          displayArray[nCounter][displayITEMNAME] = (nTmpMonth.getMonth() + 1) + "/" + nTmpMonth.getFullYear();

          nTmpMonth = DateAdd("m", 1, nTmpMonth)

          nCounter++;
        }

        // reverse the string for display
        // 9/2011,10/2011,11/2011,12/2011,1/2012,2/2012,3/2012....

        nCounter = 0;
        for (y = (displayArray.length - 1); y > -1; y--) {

          displayCboJS.options[nCounter] = new Option(displayArray[y][displayITEM]);
          displayCboJS.options[nCounter].value = displayArray[y][displayITEM];
          displayCboJS.options[nCounter].innerHTML = displayArray[y][displayITEMNAME];

          if (dtSelected == "") {
            if (nCounter == 0) {
              displayCboJS.options[0].selected = true;
              displayCboJS.options[0].selectedindex = 0;
            }
          }
          else {

            tmpDateSelect = new Date(dtSelected.getFullYear(), dtSelected.getMonth(), 1);
            tmpDateArray = new Date(cDate(displayArray[y][displayITEM]));

            if (tmpDateArray.toString() == tmpDateSelect.toString()) {
              displayCboJS.options[nCounter].selected = true;
              displayCboJS.options[nCounter].selectedindex = nCounter;
            }
          }

          nCounter++;
        }

        break;
      }
    case "Days":
      {

        // 3/13/2012,3/12/2012,3/11/2012,3/10/2012,3/09/2012,3/08/2012,3/07/2012 ...
        if (isOnlyHeli) {
          dtStartDate = dtHeliMonthSeed;
        }
        else {
          dtStartDate = dtMonthSeed;
        }

        var nTmpDay = dtStartDate;

        var dayDiff = dateDiff("d", dtStartDate, dtEndDate);

        displayArray = new Array(dayDiff);
        for (x = 0; x < displayArray.length; x++) {
          // generate an array for each dimension
          displayArray[x] = new Array(MAXARRAYDIM);
          //alert("displayArray[" + x + "].length - " + displayArray[x].length);
        }

        // find total days up to the day before the current day
        for (y = 0; y < dayDiff; y++) {
          displayArray[nCounter][displayITEM] = formatDateTime(cDate(nTmpDay), vbShortDate);
          displayArray[nCounter][displayITEMNAME] = nTmpDay.toString();

          nTmpDay = dateAdd("d", 1, nTmpDay);
          
          nCounter++;
        }

        // reverse the string for display
        // 3/07/2012,3/08/2012,3/09/2012,3/10/2012,3/11/2012,3/12/2012,3/13/2012,...

        nCounter = 0;
        for (y = (displayArray.length - 1); y > -1; y--) {

          displayCboJS.options[nCounter] = new Option(displayArray[y][displayITEM]);
          displayCboJS.options[nCounter].value = displayArray[y][displayITEM];
          displayCboJS.options[nCounter].innerHTML = displayArray[y][displayITEMNAME];

          if (dtSelected == "") {
            if (nCounter == 0) {
              displayCboJS.options[0].selected = true;
              displayCboJS.options[0].selectedindex = 0;
            }
          }
          else {
            tmpDateSelect = new Date(dtSelected.getFullYear(), dtSelected.getMonth(), dtSelected.getDate());
            tmpDateArray = new Date(cDate(displayArray[y][displayITEM]));
            if (tmpDateArray.toString() == tmpDateSelect.toString()) {
              displayCboJS.options[nCounter].selected = true;
              displayCboJS.options[nCounter].selectedindex = nCounter;
            }
          }

          nCounter++;
        }

        break;
      }
    case "Quarters":
      {

        // 1Q 12, 4Q 11, 3Q 11, 2Q 11, 1Q 11, 4Q 10
        if (isOnlyHeli) {
          dtStartDate = dtHeliYearSeed;
        }
        else {
          dtStartDate = dtYearSeed;
        }

        var dtTmpDate = new Date(dtStartDate.getFullYear(), dtStartDate.getMonth(), 1);

        var quarterDiff = dateDiff("q", dtStartDate, dtEndDate);

        displayArray = new Array(quarterDiff);
        for (x = 0; x < displayArray.length; x++) {
          // generate an array for each dimension
          displayArray[x] = new Array(MAXARRAYDIM);
          //alert("displayArray[" + x + "].length - " + displayArray[x].length);
        }

        // find total quarters up to the quarters before the current quarters
        for (y = 0; y < displayArray.length; y++) {

          displayArray[nCounter][displayITEM] = formatDateTime(cDate(dtTmpDate), vbShortDate);
          displayArray[nCounter][displayITEMNAME] = get_quarter_for_monthJS(dtTmpDate.getMonth()) + "/" + dtTmpDate.getFullYear();

          dtTmpDate = dateAdd("q", 1, dtTmpDate);

          nCounter++;

        }

        // reverse the string for display
        // 4Q 10, 1Q 11, 2Q 11, 3Q 11, 4Q 11, 1Q 12

        nCounter = 0;
        for (y = (displayArray.length - 1); y > -1; y--) {

          displayCboJS.options[nCounter] = new Option(displayArray[y][displayITEM]);
          displayCboJS.options[nCounter].value = displayArray[y][displayITEM];
          displayCboJS.options[nCounter].innerHTML = displayArray[y][displayITEMNAME];

          if (dtSelected == "") {
            if (nCounter == 0) {
              displayCboJS.options[0].selected = true;
              displayCboJS.options[0].selectedindex = 0;
            }
          }
          else {

            tmpDateSelect = new Date(dtSelected.getFullYear(), dtSelected.getMonth(), 1);
            tmpDateArray = new Date(cDate(displayArray[y][displayITEM]));

            if (tmpDateArray.toString() == tmpDateSelect.toString()) {
              displayCboJS.options[nCounter].selected = true;
              displayCboJS.options[nCounter].selectedindex = nCounter;
            }
          }

          nCounter++;
        }

        break;
      }
  }

  if (displayCboJS.options.length > 1) {

    cboRangeSpan.options.length = 0;      
    nCounter = 0;

    for (x = 0; x < return_total_rangeJS(sSelTimeScale); x++) {
      cboRangeSpan.options[nCounter] = new Option((x + 1));
      cboRangeSpan.options[nCounter].value = (x + 1);

      if (nCounter == 0) {
        cboRangeSpan.options[nCounter].innerHTML = (x + 1) + " " + sSelTimeScale.replace("s","");
      }
      else {
        cboRangeSpan.options[nCounter].innerHTML = (x + 1) + " " + sSelTimeScale;
      }

      if ((nCounter + 1) == nScaleSet) {
        cboRangeSpan.options[nCounter].selected = true;
        cboRangeSpan.options[nCounter].selectedindex = nCounter;
      }
      
      nCounter++;
      
    }
  }

  displayArray = null;

  return true;

}