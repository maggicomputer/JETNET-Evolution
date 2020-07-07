// ********************************************************************************
// Copyright 2004-11. JETNET,LLC. All rights reserved.
//
// $$Archive: /commonWebProject/common/rebuildClientArray.js $
// $$Author: Mike $
// $$Date: 5/21/20 10:16p $
// $$Modtime: 5/21/20 9:42p $
// $$Revision: 6 $
// $$Workfile: rebuildClientArray.js $
//
// ********************************************************************************

// javascript doesn't have a true "constant"
var SELECT_PLACEHOLDER = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
var NOTAPPLICABLE_PLACEHOLDER = "- NA -";

var cSvrDataSeperator = "|";
var cSvrRecordSeperator = "!~!";
var cSvrStringSeperator = "!$!";
var cSvrStringCRLF = "^^";
var cSvrSORTSeperator = "&&";
var cSvrNotesSeperator = "!@!";
var cSvrElpsis = " ...";

var cHTMLEncodeAMP = "&amp;";
var cHTMLEncodeGT = "&gt;";
var cHTMLEncodeLT = "&lt;";

var cHTMLnbsp = "&nbsp;";

var cHTMLDecodeAMP = "&";
var cHTMLDecodeGT = ">";
var cHTMLDecodeLT = "<";

var CRLF = String.fromCharCode(13) + String.fromCharCode(10);
var QUOTE = String.fromCharCode(34);


var AIRMAKETYPE_INDEX = 0;
var AIRMAKETYPE_AFT = 1;
var AIRMAKETYPE_AFMT = 2;
var AIRMAKETYPE_CODE = 3;
var AIRMAKETYPE_NAME = 4;

var clientAIRFRAMEARRAY_DIM = 9;

var LOCAIR_INDEX = 0;
var LOCAIR_TYPE = 1;
var LOCAIR_MAKE = 2;
var LOCAIR_MAKE_ABR = 3;
var LOCAIR_MODEL = 4;
var LOCAIR_MODEL_ID = 5;
var LOCAIR_USAGE = 6;
var LOCAIR_FRAME = 7;
var LOCAIR_MFRNAME = 8;
var LOCAIR_SIZE = 9;

var YACHT_LABEL_INDEX = 0;
var YACHT_LABEL_MOTOR = 1;
var YACHT_LABEL_CATEGORY = 2;
var YACHT_LABEL_CODE = 3;
var YACHT_LABEL_NAME = 4;

var LOCYACHT_INDEX = 0;
var LOCYACHT_CATEGORY = 1;
var LOCYACHT_BRAND = 2;
var LOCYACHT_BRAND_ABR = 3;
var LOCYACHT_MODEL = 4;
var LOCYACHT_MODEL_ID = 5;
var LOCYACHT_MOTOR = 6;

var LOCRGN_CONTINENT = 0;
var LOCRGN_COUNTRY = 1;
var LOCRGN_STATE_CODE = 2;
var LOCRGN_STATE_NAME = 3;
var LOCRGN_STATE_TZ = 4;
var LOCRGN_COUNTRY_ACTIVE = 5;

var LOCTZ_INDEX = 0;
var LOCTZ_NAME = 1;
var LOCTZ_SHORT = 2;

var PRIOREVCAT_CATEGORY = 0;
var PRIOREVCAT_CATEGORY_NAME = 1;
var PRIOREVCAT_CATEGORY_CODE = 2;

var FEATURE_CODE = 0;
var FEATURE_CODE_NAME = 1;

var ADTDATATYPE = 0;
var ADTDATANAME = 1;

var MAINT_PROVIDER_ID = 0;
var MAINT_PROVIDER_NAME = 1;
var MAINT_PROGRAM_NAME = 2;

var LOCMFR_INDEX = 0;
var LOCMFR_NAME = 1;
var LOCMFR_HEL = 2;
var LOCMFR_BUS = 3;
var LOCMFR_COM = 4;

var LOCACSIZE_INDEX = 0;
var LOCACSIZE_CODE = 1;
var LOCACSIZE_NAME = 2;
var LOCACSIZE_HEL = 3;
var LOCACSIZE_BUS = 4;
var LOCACSIZE_COM = 5;

// Client side support function for rebuilding array from server string
function createClientArrayFromServerStringJS(s_inDataString) {

  var aClientArray = null;

  var tStrArray = new Array();
  var tRecArray = new Array();
  var tDataArray = new Array();

  // first we break apart the server string to get length and dimension info

  //alert("s_inDataString " + s_inDataString);

  if (s_inDataString != null && s_inDataString != "") {
    tStrArray = s_inDataString.split(cSvrStringSeperator)

    if (tStrArray.length == 3) {
      // should only have 3 data items: len = tStrArray[0], dim = tStrArray[1], data = tStrArray[2]

      // ok create the client array
      if (Number(tStrArray[1]) > 0) {

        try {

          // generate master array for client
          aClientArray = new Array(Number(tStrArray[0]) + 1);
          //alert("aClientArray.length - " + aClientArray.length);

          for (var x = 0; x < Number(tStrArray[0]) + 1; x++) {
            // generate an array for each dimension
            aClientArray[x] = new Array(Number(tStrArray[1]) + 1);
            //alert("aClientArray[" + x + "].length - " + aClientArray[x].length);
          }

        }
        catch (err) {
          alert("err - " + err.description);
        }

        // ok split out records
        tRecArray = tStrArray[2].split(cSvrRecordSeperator)

        for (var x = 0; x < tRecArray.length; x++) {

          //alert("tRecArray[" + x + "] - " + tRecArray[x]);

          tDataArray = tRecArray[x].split(cSvrDataSeperator);

          if (tDataArray.length > 0) {

            for (var y = 0; y < tDataArray.length; y++) {
              var remove = /&amp;/gi;
              tDataArray[y] = tDataArray[y].replace(remove, cHTMLDecodeAMP);
              aClientArray[x][y] = tDataArray[y];
              //alert("aClientArray[" + x + "][" + y + "] - " + aClientArray[x][y]);
            }

          }
          else {
            var remove = /&amp;/gi;
            tDataArray[0] = tDataArray[0].replace(remove, cHTMLDecodeAMP);
            aClientArray[x][0] = tDataArray[0];
            //alert("aClientArray[" + x + "][0] - " + aClientArray[x][0]);
          }  // tDataArray.length > 0

        }

      }
      else {

        try {
          aClientArray = new Array(Number(tStrArray[1]) + 1);
        }
        catch (err) {
          alert("err - " + err.description);
        }

        tDataArray = tStrArray[2].split(cSvrDataSeperator)
        alert("tDataArray.length - " + tDataArray.length);

        if (tDataArray.length > 0) {

          for (var y = 0; y < tDataArray.length; y++) {
            var remove = /&amp;/gi;
            tDataArray[y] = tDataArray[y].replace(remove, cHTMLDecodeAMP);
            aClientArray[y] = tDataArray[y];
            alert("aClientArray[" + y + "] - " + aClientArray[y]);
          }

        }
        else {
          var remove = /&amp;/gi;
          tDataArray[0] = tDataArray[0].replace(remove, cHTMLDecodeAMP);
          aClientArray[0] = tDataArray[0];
          alert("aClientArray[0] - " + aClientArray[0]);
        }  // ubound(tDataArray) > 0

      }  // ubound(tRecArray) > 0

    } // ubound(tStrArray) = 2     

  } // s_inDataString <> ""

  //alert("aClientArray.length - " + aClientArray.length);

  if (aClientArray != null) {
    if (tStrArray[0] >= 0) {
      return aClientArray;
    }
  }

  tDataArray = null;
  tRecArray = null;
  tStrArray = null;

}

function inClientArrayJS(theArray, strFind) {

  var bResults = false;

  var lUBound = 0;
  var theArrayValue = "";
  var lCnt1 = 0;

  if ((theArray != null) && (theArray != "")) {

    lUBound = theArray.length;

    if (lUBound > 1) {

      while ((lCnt1 < lUBound) && (bResults != true)) {

        theArrayValue = theArray[lCnt1];

        if ((!isNaN(theArrayValue)) && (!isNaN(strFind))) {
          if (Number(theArrayValue) == Number(strFind)) {
            bResults = true;
            break;
          }
        }
        else {
          if (theArrayValue.toUpperCase() == strFind.toUpperCase()) {
            bResults = true;
            break;
          }
        }

        lCnt1 = lCnt1 + 1;

      } //while ((bResults != True) || (lCnt1 <= lUBound))

    } // (lUBound > 0)

    if (lUBound == 1) { // there is one item in the array see if this matches strFind

      theArrayValue = theArray[0];

      if ((!isNaN(theArrayValue)) && (!isNaN(strFind))) {
        if (Number(theArrayValue) == Number(strFind)) {
          bResults = true;
        }
      }
      else {
        if (theArrayValue.toUpperCase() == strFind.toUpperCase()) {
          bResults = true;
        }
      }
    }
    else {
      if (lUBound < 0) { // there are no items in the array return false
        bResults = false;
      }

    } // (lUBound = 0)
  } // (isArray(theArray) && (theArray != null))

  return bResults;

} // End Function inArray

function quickSortJS(arr) {

  if (arr.length == 0)
    return [];

  var left = new Array();
  var right = new Array();
  var pivot = arr[0];

  for (var i = 1; i < arr.length; i++) {
    if (arr[i] < pivot) {
      left.push(arr[i]);
    } else {
      right.push(arr[i]);
    }
  }

  return quickSortJS(left).concat(pivot, quickSortJS(right));
}  //QuickSort

function sortListBoxJS(inListBoxJS) {

  var tmpSortString = "";
  var tmpSelectedItems = "";
  var tmpRememberItem = "";
  var theListJS = null;
  var remove = /,/gi;

  if (inListBoxJS.options.length > 0) {

    theListJS = inListBoxJS;

    for (var a = 0; a < theListJS.options.length; a++) {
      if (tmpSortString == "") {
        tmpSortString = theListJS.options[a].innerHTML.replace(remove, "^") + "&&" + theListJS.options[a].value.replace(remove, "^");
      }
      else {
        tmpSortString = tmpSortString + "," + theListJS.options[a].innerHTML.replace(remove, "^") + "&&" + theListJS.options[a].value.replace(remove, "^");
      }

      if (theListJS.options[a].selected == true) {
        if (tmpSelectedItems == "") {
          tmpSelectedItems = theListJS.options[a].value.replace(remove, "^");
        }
        else {
          tmpSelectedItems = tmpSelectedItems + "," + theListJS.options[a].value.replace(remove, "^");
        }
      }
    }

    try {
      var arrSortList = tmpSortString.split(",");
      var arrSortSelected = tmpSelectedItems.split(",");
    }
    catch (err) {
      alert("err - " + err.description);
    }

    if ((arrSortList != null) && (arrSortList != "")) {

      var optcnt = 0;

      arrSortList = quickSortJS(arrSortList, 0, arrSortList.length);

      theListJS.options.length = 0;
      theListJS.options[0] = new Option("");

      for (var b = 0; b < arrSortList.length; b++) {
        if (arrSortList[b].substring(0, (arrSortList[b].indexOf("&&"))) != "") {
          if (arrSortList[b].substring(0, (arrSortList[b].indexOf("&&"))).toUpperCase() != "ALL") {
            if (tmpRememberItem != arrSortList[b].substring(0, (arrSortList[b].indexOf("&&")))) {

              if (optcnt == 0) {
                theListJS.options[0].innerHTML = "All";
                theListJS.options[0].value = "All";

                if ((tmpSelectedItems != "") && (tmpSelectedItems.toUpperCase() == "ALL")) {
                  theListJS.options[0].selected = true;
                }
                else {
                  theListJS.options[optcnt].selected = false;
                }
                optcnt++;
              } // t = 0

              theListJS.options[optcnt] = new Option(arrSortList[b].substring(0, (arrSortList[b].indexOf("&&"))));

              var tmpValue = arrSortList[b].substring((arrSortList[b].indexOf("&&") + 2), arrSortList[b].length);
              var tmpInnerHTML = arrSortList[b].substring(0, (arrSortList[b].indexOf("&&")))
              var remove2 = /^/gi;

              if (tmpValue.indexOf("^") > -1) {
                tmpValue = tmpValue.replace(remove2, ",");
              }

              if (tmpInnerHTML.indexOf("^") > -1) {
                tmpInnerHTML = tmpInnerHTML.replace(remove2, ",");
              }

              theListJS.options[optcnt].value = tmpValue;
              theListJS.options[optcnt].innerHTML = tmpInnerHTML;

              if (arrSortSelected.length > 0) {
                if (inClientArrayJS(arrSortSelected, theListJS.options[optcnt].value.replace(remove, "^"))) {
                  theListJS.options[optcnt].selected = true;
                }
              }

              tmpRememberItem = arrSortList[b].substring(0, (arrSortList[b].indexOf("&&")));
              optcnt++;

            } // (tmpRememberItem != arrSortList[b].substring(0, (arrSortList[b].indexOf("&&"))))
          } // (arrSortList[b].substring(0, (arrSortList[b].indexOf("&&"))).toUpperCase() != "ALL")
        } // (arrSortList[b].substring(0, (arrSortList[b].indexOf("&&"))) != "")
      } // (var b = 0; b < arrSortList.length; b++)
    } //((arrSortList != null) && (arrSortList != ""))

  } // (inListBox.options.length > 0)

  return theListJS;

}
