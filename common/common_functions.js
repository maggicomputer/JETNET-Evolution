function load(x, z, y) {
    var browserZoomLevel = Math.round(window.devicePixelRatio * 100) / 100;

    if (browserZoomLevel > 1) {
        var widthArr = y.split("width=");
        if (widthArr.length > 0) {
            var widthVar = widthArr[1].split(",");
            if (widthVar.length >= 0) {
                var newWidth = Number(widthVar[0])
                newWidth = newWidth * browserZoomLevel;
                y = y.replace(widthVar[0], newWidth)
            }
        }
    }
    if (y != '') {
        y = y + ", menubar=1";
    }
    var w = window.open(x, "_blank", y);
}

function resetForm($form) {
    alert("test");
    $form.find('input:text, input:password, input:file, select, textarea').val('');
    $form.find('input:radio, input:checkbox')
        .removeAttr('checked').removeAttr('selected');
}


function ShowBar(type, visibility) {
    var vis = "block";
    var dropdown = null;

    if (visibility == false) {
        vis = "none";
    }

    dropdown = $('#' + type)
    // alert(dropdown);
    if (dropdown != null) {
        if (visibility == false) {
            dropdown.hide();
        } else {
            dropdown.show();
        }
    }

}
function ParseViewFolders(Val, viewID, dataIn, blankTarget) {
    ChangeTheMouseCursorOnItemParentDocument('cursor_wait');

    var seperated = dataIn.split("!~!");
    var tempHold;
    var valPair;

    my_form = document.createElement('FORM');
    my_form.name = 'myForm';

    if (blankTarget == 'true') {
        my_form.target = "_blank";
    } else if (blankTarget == '') {
        my_form.target = "ValueMaster";
    }
    my_form.method = 'POST';


    my_form.action = '/view_template.aspx?ViewID=' + viewID;

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'project_id';
    my_tb.value = Val;
    my_form.appendChild(my_tb);


    for (var i = 0, length = seperated.length; i < length; i++) {
        tempHold = seperated[i].replace("!", ""); //replace these characters for now
        tempHold = tempHold.replace("~", ""); ///replace these characters for now 
        valPair = tempHold.split("="); //split by equals sign
        //Setting up Form
        my_tb = document.createElement('INPUT');
        my_tb.type = 'HIDDEN';
        my_tb.name = valPair[0];
        my_tb.value = valPair[1];
        my_form.appendChild(my_tb);

    }

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'project_search';
    my_tb.value = "Y";
    my_form.appendChild(my_tb);

    document.body.appendChild(my_form);
    my_form.submit();
}

function ParseSpecsOperatingMarketForm(Val, Performance, Operating, Market, dataIn) {
    ChangeTheMouseCursorOnItemParentDocument('cursor_wait');

    var seperated = dataIn.split("!~!");
    var tempHold;
    var valPair;

    my_form = document.createElement('FORM');
    my_form.name = 'myForm';

    if (window.opener != null) {
        window.opener.name = "MyParent";
        my_form.target = "MyParent";
    }
    my_form.method = 'POST';

    if (Performance == true) {
        my_form.action = 'Performance_Listing.aspx';
    } else if (Operating == true) {
        my_form.action = 'Operating_Listing.aspx';
    } else {
        my_form.action = 'MarketSummary.aspx';
    }

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'project_id';
    my_tb.value = Val;
    my_form.appendChild(my_tb);


    for (var i = 0, length = seperated.length; i < length; i++) {
        tempHold = seperated[i].replace("!", ""); //replace these characters for now
        tempHold = tempHold.replace("~", ""); ///replace these characters for now 
        valPair = tempHold.split("="); //split by equals sign
        //Setting up Form
        my_tb = document.createElement('INPUT');
        my_tb.type = 'HIDDEN';
        my_tb.name = valPair[0];
        my_tb.value = valPair[1];
        my_form.appendChild(my_tb);

    }

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'project_search';
    my_tb.value = "Y";
    my_form.appendChild(my_tb);

    document.body.appendChild(my_form);
    my_form.submit();
}
function ParseYachtSpecialFolders(Val, yachtHistory, yachtEvent, dataIn) {
    var seperated = dataIn.split("!~!");
    var tempHold;
    var valPair;

    my_form = document.createElement('FORM');
    my_form.name = 'myForm';

    if (window.opener != null) {
        window.opener.name = "MyParent";
        my_form.target = "MyParent";
    }
    my_form.method = 'POST';

    my_form.action = 'YachtListing.aspx';
    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'project_id';
    my_tb.value = Val;
    my_form.appendChild(my_tb);

    if (yachtHistory == true) {
        my_form.action = 'YachtListing.aspx?h=1';
    }
    if (yachtEvent == true) {
        my_form.action = 'YachtListing.aspx?e=1';
    }
    for (var i = 0, length = seperated.length; i < length; i++) {
        tempHold = seperated[i].replace("!", ""); //replace these characters for now
        tempHold = tempHold.replace("~", ""); ///replace these characters for now 
        valPair = tempHold.split("="); //split by equals sign
        //Setting up Form
        my_tb = document.createElement('INPUT');
        my_tb.type = 'HIDDEN';
        my_tb.name = valPair[0];
        my_tb.value = valPair[1];
        my_form.appendChild(my_tb);

    }

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'project_search';
    my_tb.value = "Y";
    my_form.appendChild(my_tb);

    document.body.appendChild(my_form);
    my_form.submit();
}


function ParseForm(Val, history, event, company, wanted, yacht, dataIn) {
    ChangeTheMouseCursorOnItemParentDocument('cursor_wait');

    var seperated = dataIn.split("!~!");
    var tempHold;
    var valPair;

    my_form = document.createElement('FORM');
    my_form.name = 'myForm';

    if (window.opener != null) {
        window.opener.name = "MyParent";
        my_form.target = "MyParent";
    }
    my_form.method = 'POST';

    if (company == true) {
        my_form.action = 'Company_Listing.aspx';
    } else if (wanted == true) {
        my_form.action = 'Wanted_Listing.aspx';
    } else if (yacht == true) {
        my_form.action = 'YachtListing.aspx';
    } else {
        my_form.action = 'Aircraft_Listing.aspx';
    }
    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'project_id';
    my_tb.value = Val;
    my_form.appendChild(my_tb);

    if (history == true) {
        my_form.action = 'Aircraft_Listing.aspx?h=1';
    }
    if (event == true) {
        my_form.action = 'Aircraft_Listing.aspx?e=1';
    }
    for (var i = 0, length = seperated.length; i < length; i++) {
        tempHold = seperated[i].replace("!", ""); //replace these characters for now
        tempHold = tempHold.replace("~", ""); ///replace these characters for now 
        valPair = tempHold.split("="); //split by equals sign
        //Setting up Form
        my_tb = document.createElement('INPUT');
        my_tb.type = 'HIDDEN';
        my_tb.name = valPair[0];
        my_tb.value = valPair[1];
        my_form.appendChild(my_tb);

    }

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'project_search';
    my_tb.value = "Y";
    my_form.appendChild(my_tb);

    document.body.appendChild(my_form);
    my_form.submit();
}

function ParseCLIENTForm(Val, history, event, company, wanted, yacht, dataIn) {
    ChangeTheMouseCursorOnItemParentDocument('cursor_wait');

    // var seperated = dataIn.split("!~!");
    var tempHold;
    var valPair;

    my_form = document.createElement('FORM');
    my_form.name = 'myForm';

    if (window.opener != null) {
        window.opener.name = "MyParent";
        my_form.target = "MyParent";
    }
    my_form.method = 'POST';

    if (company == true) {
        my_form.action = 'Company_Listing.aspx';
    } else if (wanted == true) {
        my_form.action = 'Wanted_Listing.aspx';
    } else if (yacht == true) {
        my_form.action = 'YachtListing.aspx';
    } else {
        my_form.action = 'Aircraft_Listing.aspx';
    }
    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'project_id';
    my_tb.value = Val;
    my_form.appendChild(my_tb);

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'cfolder_source';
    my_tb.value = "CLIENT";
    my_form.appendChild(my_tb);

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'cfolder_data';
    my_tb.value = dataIn;
    my_form.appendChild(my_tb);

    if (history == true) {
        my_form.action = 'Aircraft_Listing.aspx?h=1';
    }
    if (event == true) {
        my_form.action = 'Aircraft_Listing.aspx?e=1';
    }
    var seperated = dataIn.split("!~!");

    for (var i = 0, length = seperated.length; i < length; i++) {
        tempHold = seperated[i].replace("!", ""); //replace these characters for now
        tempHold = tempHold.replace("~", ""); ///replace these characters for now 
        valPair = tempHold.split("="); //split by equals sign
        //Setting up Form
        my_tb = document.createElement('INPUT');
        my_tb.type = 'HIDDEN';
        my_tb.name = valPair[0];
        my_tb.value = valPair[1];
        my_form.appendChild(my_tb);
        // alert(valPair[0] + ' ' + valPair[1]);

    }
    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'project_search';
    my_tb.value = "Y";
    my_form.appendChild(my_tb);

    document.body.appendChild(my_form);
    my_form.submit();
}

function SubmitTransactionDocumentForm(make, model, ser, acID, journalID, documentSeqNo) {
    my_form = document.createElement('FORM');
    my_form.name = 'myForm';
    my_form.method = 'POST';
    my_form.action = 'picture.aspx';
    my_form.target = "_blank"

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'model';
    my_tb.value = model;
    my_form.appendChild(my_tb);

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'make';
    my_tb.value = make;
    my_form.appendChild(my_tb);

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'serial';
    my_tb.value = ser;
    my_form.appendChild(my_tb);

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'acID';
    my_tb.value = acID;
    my_form.appendChild(my_tb);

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'journalID';
    my_tb.value = journalID;
    my_form.appendChild(my_tb);

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'document';
    my_tb.value = documentSeqNo;
    my_form.appendChild(my_tb);


    document.body.appendChild(my_form);
    my_form.submit();
}

function createCookie(name, value, days) {
    //        alert(name + ":" + value);
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    } else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

function SaveRemoveDefault(reportID, folder_type, remove, save) {
    my_form = document.createElement('FORM');
    my_form.method = 'POST';
    my_form.target = "_blank"

    my_form.action = 'FolderMaintenance.aspx';
    my_form.name = 'folderForm';

    //Appending this is a default folder edit.
    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = "DEFAULT_FOLDER_EDIT";
    my_tb.value = "true"; //.innerHTML;
    my_form.appendChild(my_tb);


    //Appending the type of folder
    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = "TYPE_OF_FOLDER";
    my_tb.value = folder_type//.innerHTML;
    my_form.appendChild(my_tb);


    //Appending remove
    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = "REMOVE";
    my_tb.value = remove//.innerHTML;
    my_form.appendChild(my_tb);


    //Appending save
    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = "SAVE";
    my_tb.value = save//.innerHTML;
    my_form.appendChild(my_tb);

    //this parameter means that this is an update instead of insert.
    if (reportID != 0) {
        my_tb = document.createElement('INPUT');
        my_tb.type = 'HIDDEN';
        my_tb.name = "REPORT_ID";
        my_tb.value = reportID;
        my_form.appendChild(my_tb);
    }

    document.body.appendChild(my_form);
    my_form.submit();

}
function SubMenuDrop(x, reportID, folder_type) {
    //var folder_type;
    //folder_type = document.getElementById("<%= page_type.clientID %>");

    my_form = document.createElement('FORM');
    my_form.method = 'POST';
    my_form.target = "_blank"
    // alert(folder_type);

    switch (x) {
        case 4:
            //Map Form
            my_form.name = 'mappingForm';
            my_form.action = 'MapItems.aspx';
            document.body.appendChild(my_form);
            my_form.submit();
            break;
        case 2:
            //Summary popup
            if (folder_type == 'COMPANY') {
                window.location = 'SearchSummary.aspx?sub_type=C'; //redirects to homepage
            } else {
                my_form.name = 'exportForm';
                my_form.action = 'evo_exporter.aspx';
                my_tb = document.createElement('INPUT');
                my_tb.type = 'HIDDEN';
                my_tb.name = 'type';
                my_tb.value = "summary";
                my_form.appendChild(my_tb);

                //Appending the type of folder, either Aircraft or History.
                my_tb = document.createElement('INPUT');
                my_tb.type = 'HIDDEN';
                my_tb.name = "export_type";
                my_tb.value = folder_type//.innerHTML;
                my_form.appendChild(my_tb);
                document.body.appendChild(my_form);
                my_form.submit();
            }
            break;
        case 5:
            load("PDF_Creator.aspx?export_type=" + folder_type, "", "scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no");
            break;
        case 6:
            //alert("market selection : " + folder_type);
            load("STAR_ToFromReport.aspx?starReport=" + reportID + "&marketSelection=" + folder_type, "", "scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no");
            break;
        case 7:
            load("WebSource.aspx?viewID=4&viewType=dynamic&PageTitle=Financial Documents&display=table", "", "scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no");
            break;
        case 3:
            //folders maintenance popup  

            my_form.action = 'FolderMaintenance.aspx';
            my_form.name = 'folderForm';

            //Appending the type of folder, either Aircraft or History.
            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = "TYPE_OF_FOLDER";
            my_tb.value = folder_type//.innerHTML;
            my_form.appendChild(my_tb);

            //this parameter means that this is an update instead of insert.
            if (reportID != 0) {
                my_tb = document.createElement('INPUT');
                my_tb.type = 'HIDDEN';
                my_tb.name = "REPORT_ID";
                my_tb.value = reportID;
                my_form.appendChild(my_tb);
            }


            var str = '';
            var elem = document.getElementById('aspnetForm').elements;
            for (var i = 0; i < elem.length; i++) {
                if (elem[i].type != 'hidden' && elem[i].type != 'submit') {
                    if (elem[i].value != '') {
                        var re = new RegExp("ctl[A-Za-z0-9]*_ContentPlaceHolder[A-Za-z0-9]_", "g");
                        var re2 = new RegExp("Criteria_Bar[A-Za-z0-9]*_", "g");
                        var re8 = new RegExp("ContentPlaceHolder1_", "g");
                        var rep = elem[i].id;
                        var temp = rep.replace(re, "");
                        temp = temp.replace(re8, "")

                        temp = temp.replace(re2, "");
                        my_tb = document.createElement('INPUT');
                        my_tb.type = 'HIDDEN';
                        my_tb.name = temp;

                        //If it has a checked value that's not undefined, go ahead and 
                        //Pass that, if not, pass the value

                        if (elem[i].type == 'checkbox') {
                            my_tb.value = elem[i].checked;
                            //alert(temp + " : " + elem[i].value);
                        } else if (elem[i].type == 'select-multiple') {
                            //var opt = document.getElementById('' + elem[i].id + '').options
                            //alert(elem[i].id);
                            var SelBranchVal = "";
                            var x = 0;
                            for (x = 0; x < elem[i].length; x++) {
                                if (elem[i][x].selected) {
                                    //Add seperator just not for 1st entry.
                                    if (SelBranchVal != "") {
                                        SelBranchVal = SelBranchVal + "##"
                                    }
                                    SelBranchVal = SelBranchVal + elem[i][x].value;
                                }
                            }
                            //alert(SelBranchVal);
                            my_tb.value = SelBranchVal; //elem[i].value;
                        } else if (elem[i].type == 'radio') {
                            my_tb.value = elem[i].checked;
                        } else {
                            my_tb.value = elem[i].value;
                            //alert(temp + " : " + elem[i].checked);
                        }

                        my_form.appendChild(my_tb);
                    }
                }
            }
            document.body.appendChild(my_form);
            my_form.submit();
            break;
        default:
            //Evo Exporter popup
            my_form.name = 'exportForm';
            my_form.action = 'evo_exporter.aspx';
            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = 'type';
            my_tb.value = "";
            my_form.appendChild(my_tb);

            //Appending the type of folder, either Aircraft or History.
            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = "export_type";
            my_tb.value = folder_type//.innerHTML;
            my_form.appendChild(my_tb);
            document.body.appendChild(my_form);
            my_form.submit();
    }


}


//VALIDATES AD CHECKS FOR A CORRECT DATE FORMAT AND VALID DATE
/**--------------------------
//* Validate Date Field script- By JavaScriptKit.com
//* For this script and 100s more, visit http://www.javascriptkit.com
//* This notice must stay intact for usage
---------------------------**/

function checkdate(input) {
    var validformat = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/
    //var validformat=/^\d{2}\/\d{2}\/\d{4}$/ //Basic check for format validity
    var returnval = false
    if (!validformat.test(input))
        returnval = false
    else { //Detailed check for valid date ranges
        var monthfield = input.split("/")[0]
        var dayfield = input.split("/")[1]
        var yearfield = input.split("/")[2]
        var dayobj = new Date(yearfield, monthfield - 1, dayfield)

        if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield))
            returnval = false
        else
            returnval = true
    }

    //if (returnval==false) 
    return returnval
}

//This takes a textbox and validates to see if the date is valid
//based on a bunch of different searching
//styles
function validateDate(sender, args) {
    var InputFromBox = args.Value;
    var replaceColon = new RegExp(":", "g");
    var replaceNewLine = new RegExp("\n", "g");
    var InputToValidate = InputFromBox.replace(replaceColon, ",");
    InputToValidate = InputToValidate.replace(replaceNewLine, ",");

    var mySplitDates = InputToValidate.split(",");

    for (i = 0; i < mySplitDates.length; i++) {
        if (mySplitDates[i] != '') {
            if ((checkdate(mySplitDates[i])) == false) {
                args.IsValid = false;
                return;
            }
        }
    }

    args.IsValid = true;
    return;

}

//Clear associated operator dropdowns with associated textbox if (only if) the value is blank of the operator.
///Text area parameter tells what type of input you're trying to clear.
function ClearAssociatedBox(selectValue, SelectBoxID, inputType) {
    var clearBox = $(inputType + "[name*='$" + SelectBoxID + "']");
    if (selectValue == '') {
        clearBox.val('');
    }
}


//This function goes and filters the maintenance program dropdowns on the aircraft search page,
//However it was set up to accept any array, any value and update the dropdown based on the array filtering.
//So it could be used in a variety of cases. 
function FilterDropDownBasedOnValue(valueToFilter, arrayToSearch, DropDownIDToReplace) {
    var select = document.getElementById(DropDownIDToReplace);
    //Clear dropdown
    for (var selIndex = select.length - 1; selIndex >= 0; selIndex--) {
        // Delete the option in the first select box.
        select[selIndex] = null;
    }

    var Emptopt = document.createElement("option");

    // Add an Empty Option object to Drop Down/List Box
    select.options.add(Emptopt);

    // Assign text and value to Option object
    Emptopt.text = "";
    Emptopt.value = "";


    for (i = 0; i < arrayToSearch.length; i++) {
        if (valueToFilter == "'" + arrayToSearch[i][0] + "'") {

            var opt = document.createElement("option");

            // Add an Option object to Drop Down/List Box
            select.options.add(opt);

            // Assign text and value to Option object
            opt.text = arrayToSearch[i][1];
            opt.value = "'" + arrayToSearch[i][1] + "'";
        }
    }
}

function PerformYachtSearch(requestName, requestVar, optionalRequestName, optionalRequestVar) {
    ChangeTheMouseCursorOnItemParentDocument('cursor_wait');
    //    alert(requestName); 
    //     alert(requestVar);
    my_form = document.createElement('FORM');
    my_form.name = 'myForm';
    my_form.method = 'POST';
    my_form.action = 'YachtListing.aspx';
    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';

    my_tb.name = '' + requestName + '';
    my_tb.value = '' + requestVar + '';

    my_form.appendChild(my_tb);

    //optional ability to add a second parameter if needed.
    if (optionalRequestName != '') {
        my_tb = document.createElement('INPUT');
        my_tb.type = 'HIDDEN';

        my_tb.name = '' + optionalRequestName + '';
        my_tb.value = '' + optionalRequestVar + '';

        my_form.appendChild(my_tb);
    }


    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';

    my_tb.name = 'complete_search';
    my_tb.value = 'Y';

    my_form.appendChild(my_tb);

    document.body.appendChild(my_form);
    my_form.submit();
}

function FillStateHiddenValue(typeValue) {
    //This is set up to find the company State Box ID, if the parameter type is passed as 1, this is searched for, as well as the base.
    //If 2 is passed, it's just this.
    switch (typeValue) {

        case 1:
            var CompanyStateBox = $('#cboCompanyStateID');
            var CompanyRadioButton = $('#radContinentRegionID1:checked');
            AutoSelectAllState(CompanyStateBox, CompanyRadioButton, 'cboCompanyStateID');

            //We need to auto select all the countries if they have all selected.
            var CompanyCountryBox = $('#cboCompanyCountryID');
            AutoSelectAllState(CompanyCountryBox, CompanyRadioButton, 'cboCompanyCountryID');

            var BaseStateBox = $('#cboBaseStateID');
            var BaseRadioButton = $('#radBaseContinentRegionID1:checked');
            AutoSelectAllState(BaseStateBox, BaseRadioButton, 'cboBaseStateID');
            var BaseCountryBox = $('#cboBaseCountryID');

            AutoSelectAllState(BaseCountryBox, BaseRadioButton, 'cboBaseCountryID');

            break;

        case 2:
            var CompanyStateBox = $('#cboCompanyStateID');
            var CompanyRadioButton = $('#radContinentRegionID1:checked');
            AutoSelectAllState(CompanyStateBox, CompanyRadioButton, 'cboCompanyStateID');
            var CompanyCountryBox = $('#cboCompanyCountryID');
            AutoSelectAllState(CompanyCountryBox, CompanyRadioButton, 'cboCompanyCountryID');

    }
}

function AutoSelectAllState(SelectStateBox, RadioButtonValue, applicableSelector) {//
    if (RadioButtonValue.val() == 'Region') {
        if (SelectStateBox.val() == 'All') {//
            htmlReference = document.getElementById(applicableSelector);
            //$(applicableSelector).find("option").attr("selected", false);

            //Alright, in this case - and this case ONLY - we have to loop through the company state box and basically 
            //save the states.
            //		       SelectStateBox.children("option").each(function() {//
            //		            var $this = $(this);
            //		            if ($this.text() != 'All') { //
            //		            
            for (var i = 0; i < htmlReference.options.length; i++) {
                if (htmlReference.options[i].innerHTML != 'All') {
                    htmlReference.options[i].selected = true;
                } else {
                    htmlReference.options[i].selected = false;
                }
            }
            //		            alert($this.text());
            //		                $this.attr('selected','selected');
            //		            }//
            // });//
        } //
    } //
} //


function ChangeTheMouseCursorOnItemParentDocument(cursorType) {
    document.body.className = cursorType;
}

function SetWaitCursor() {
    document.body.className = 'cursor_wait';
}


function callQuickHeaderSearch(dataIn, samePage) {

    my_form = document.createElement('FORM');
    my_form.name = 'myForm';

    my_form.target = "_blank";
    //if (samePage == '') {
    //    my_form.target = "formresult";
    //    my_form.setAttribute("target", "formresult");
    //}
    my_form.method = 'POST';



    my_form.action = '/fullTextSearch.aspx'

    my_tb = document.createElement('INPUT');
    my_tb.type = 'HIDDEN';
    my_tb.name = 'q';
    my_tb.value = dataIn;
    my_form.appendChild(my_tb);

    document.body.appendChild(my_form);

    // creating the 'formresult' window with custom features prior to submitting the form
    //if (samePage == '') {
    //    window.open('', 'formresult', 'scrollbars=no,menubar=no,height=600,width=1100,resizable=yes,toolbar=no,status=no');
    //}
    my_form.submit();
}


/* Modernizr 2.6.2 (Custom Build) | MIT & BSD
* Build: http://modernizr.com/download/#-touch-shiv-cssclasses-teststyles-prefixes-load
*/
; window.Modernizr = function (a, b, c) { function w(a) { j.cssText = a } function x(a, b) { return w(m.join(a + ";") + (b || "")) } function y(a, b) { return typeof a === b } function z(a, b) { return !! ~("" + a).indexOf(b) } function A(a, b, d) { for (var e in a) { var f = b[a[e]]; if (f !== c) return d === !1 ? a[e] : y(f, "function") ? f.bind(d || b) : f } return !1 } var d = "2.6.2", e = {}, f = !0, g = b.documentElement, h = "modernizr", i = b.createElement(h), j = i.style, k, l = {}.toString, m = " -webkit- -moz- -o- -ms- ".split(" "), n = {}, o = {}, p = {}, q = [], r = q.slice, s, t = function (a, c, d, e) { var f, i, j, k, l = b.createElement("div"), m = b.body, n = m || b.createElement("body"); if (parseInt(d, 10)) while (d--) j = b.createElement("div"), j.id = e ? e[d] : h + (d + 1), l.appendChild(j); return f = ["&#173;", '<style id="s', h, '">', a, "</style>"].join(""), l.id = h, (m ? l : n).innerHTML += f, n.appendChild(l), m || (n.style.background = "", n.style.overflow = "hidden", k = g.style.overflow, g.style.overflow = "hidden", g.appendChild(n)), i = c(l, a), m ? l.parentNode.removeChild(l) : (n.parentNode.removeChild(n), g.style.overflow = k), !!i }, u = {}.hasOwnProperty, v; !y(u, "undefined") && !y(u.call, "undefined") ? v = function (a, b) { return u.call(a, b) } : v = function (a, b) { return b in a && y(a.constructor.prototype[b], "undefined") }, Function.prototype.bind || (Function.prototype.bind = function (b) { var c = this; if (typeof c != "function") throw new TypeError; var d = r.call(arguments, 1), e = function () { if (this instanceof e) { var a = function () { }; a.prototype = c.prototype; var f = new a, g = c.apply(f, d.concat(r.call(arguments))); return Object(g) === g ? g : f } return c.apply(b, d.concat(r.call(arguments))) }; return e }), n.touch = function () { var c; return "ontouchstart" in a || a.DocumentTouch && b instanceof DocumentTouch ? c = !0 : t(["@media (", m.join("touch-enabled),("), h, ")", "{#modernizr{top:9px;position:absolute}}"].join(""), function (a) { c = a.offsetTop === 9 }), c }; for (var B in n) v(n, B) && (s = B.toLowerCase(), e[s] = n[B](), q.push((e[s] ? "" : "no-") + s)); return e.addTest = function (a, b) { if (typeof a == "object") for (var d in a) v(a, d) && e.addTest(d, a[d]); else { a = a.toLowerCase(); if (e[a] !== c) return e; b = typeof b == "function" ? b() : b, typeof f != "undefined" && f && (g.className += " " + (b ? "" : "no-") + a), e[a] = b } return e }, w(""), i = k = null, function (a, b) { function k(a, b) { var c = a.createElement("p"), d = a.getElementsByTagName("head")[0] || a.documentElement; return c.innerHTML = "x<style>" + b + "</style>", d.insertBefore(c.lastChild, d.firstChild) } function l() { var a = r.elements; return typeof a == "string" ? a.split(" ") : a } function m(a) { var b = i[a[g]]; return b || (b = {}, h++ , a[g] = h, i[h] = b), b } function n(a, c, f) { c || (c = b); if (j) return c.createElement(a); f || (f = m(c)); var g; return f.cache[a] ? g = f.cache[a].cloneNode() : e.test(a) ? g = (f.cache[a] = f.createElem(a)).cloneNode() : g = f.createElem(a), g.canHaveChildren && !d.test(a) ? f.frag.appendChild(g) : g } function o(a, c) { a || (a = b); if (j) return a.createDocumentFragment(); c = c || m(a); var d = c.frag.cloneNode(), e = 0, f = l(), g = f.length; for (; e < g; e++) d.createElement(f[e]); return d } function p(a, b) { b.cache || (b.cache = {}, b.createElem = a.createElement, b.createFrag = a.createDocumentFragment, b.frag = b.createFrag()), a.createElement = function (c) { return r.shivMethods ? n(c, a, b) : b.createElem(c) }, a.createDocumentFragment = Function("h,f", "return function(){var n=f.cloneNode(),c=n.createElement;h.shivMethods&&(" + l().join().replace(/\w+/g, function (a) { return b.createElem(a), b.frag.createElement(a), 'c("' + a + '")' }) + ");return n}")(r, b.frag) } function q(a) { a || (a = b); var c = m(a); return r.shivCSS && !f && !c.hasCSS && (c.hasCSS = !!k(a, "article,aside,figcaption,figure,footer,header,hgroup,nav,section{display:block}mark{background:#FF0;color:#000}")), j || p(a, c), a } var c = a.html5 || {}, d = /^<|^(?:button|map|select|textarea|object|iframe|option|optgroup)$/i, e = /^(?:a|b|code|div|fieldset|h1|h2|h3|h4|h5|h6|i|label|li|ol|p|q|span|strong|style|table|tbody|td|th|tr|ul)$/i, f, g = "_html5shiv", h = 0, i = {}, j; (function () { try { var a = b.createElement("a"); a.innerHTML = "<xyz></xyz>", f = "hidden" in a, j = a.childNodes.length == 1 || function () { b.createElement("a"); var a = b.createDocumentFragment(); return typeof a.cloneNode == "undefined" || typeof a.createDocumentFragment == "undefined" || typeof a.createElement == "undefined" }() } catch (c) { f = !0, j = !0 } })(); var r = { elements: c.elements || "abbr article aside audio bdi canvas data datalist details figcaption figure footer header hgroup mark meter nav output progress section summary time video", shivCSS: c.shivCSS !== !1, supportsUnknownElements: j, shivMethods: c.shivMethods !== !1, type: "default", shivDocument: q, createElement: n, createDocumentFragment: o }; a.html5 = r, q(b) }(this, b), e._version = d, e._prefixes = m, e.testStyles = t, g.className = g.className.replace(/(^|\s)no-js(\s|$)/, "$1$2") + (f ? " js " + q.join(" ") : ""), e }(this, this.document), function (a, b, c) { function d(a) { return "[object Function]" == o.call(a) } function e(a) { return "string" == typeof a } function f() { } function g(a) { return !a || "loaded" == a || "complete" == a || "uninitialized" == a } function h() { var a = p.shift(); q = 1, a ? a.t ? m(function () { ("c" == a.t ? B.injectCss : B.injectJs)(a.s, 0, a.a, a.x, a.e, 1) }, 0) : (a(), h()) : q = 0 } function i(a, c, d, e, f, i, j) { function k(b) { if (!o && g(l.readyState) && (u.r = o = 1, !q && h(), l.onload = l.onreadystatechange = null, b)) { "img" != a && m(function () { t.removeChild(l) }, 50); for (var d in y[c]) y[c].hasOwnProperty(d) && y[c][d].onload() } } var j = j || B.errorTimeout, l = b.createElement(a), o = 0, r = 0, u = { t: d, s: c, e: f, a: i, x: j }; 1 === y[c] && (r = 1, y[c] = []), "object" == a ? l.data = c : (l.src = c, l.type = a), l.width = l.height = "0", l.onerror = l.onload = l.onreadystatechange = function () { k.call(this, r) }, p.splice(e, 0, u), "img" != a && (r || 2 === y[c] ? (t.insertBefore(l, s ? null : n), m(k, j)) : y[c].push(l)) } function j(a, b, c, d, f) { return q = 0, b = b || "j", e(a) ? i("c" == b ? v : u, a, b, this.i++, c, d, f) : (p.splice(this.i++, 0, a), 1 == p.length && h()), this } function k() { var a = B; return a.loader = { load: j, i: 0 }, a } var l = b.documentElement, m = a.setTimeout, n = b.getElementsByTagName("script")[0], o = {}.toString, p = [], q = 0, r = "MozAppearance" in l.style, s = r && !!b.createRange().compareNode, t = s ? l : n.parentNode, l = a.opera && "[object Opera]" == o.call(a.opera), l = !!b.attachEvent && !l, u = r ? "object" : l ? "script" : "img", v = l ? "script" : u, w = Array.isArray || function (a) { return "[object Array]" == o.call(a) }, x = [], y = {}, z = { timeout: function (a, b) { return b.length && (a.timeout = b[0]), a } }, A, B; B = function (a) { function b(a) { var a = a.split("!"), b = x.length, c = a.pop(), d = a.length, c = { url: c, origUrl: c, prefixes: a }, e, f, g; for (f = 0; f < d; f++) g = a[f].split("="), (e = z[g.shift()]) && (c = e(c, g)); for (f = 0; f < b; f++) c = x[f](c); return c } function g(a, e, f, g, h) { var i = b(a), j = i.autoCallback; i.url.split(".").pop().split("?").shift(), i.bypass || (e && (e = d(e) ? e : e[a] || e[g] || e[a.split("/").pop().split("?")[0]]), i.instead ? i.instead(a, e, f, g, h) : (y[i.url] ? i.noexec = !0 : y[i.url] = 1, f.load(i.url, i.forceCSS || !i.forceJS && "css" == i.url.split(".").pop().split("?").shift() ? "c" : c, i.noexec, i.attrs, i.timeout), (d(e) || d(j)) && f.load(function () { k(), e && e(i.origUrl, h, g), j && j(i.origUrl, h, g), y[i.url] = 2 }))) } function h(a, b) { function c(a, c) { if (a) { if (e(a)) c || (j = function () { var a = [].slice.call(arguments); k.apply(this, a), l() }), g(a, j, b, 0, h); else if (Object(a) === a) for (n in m = function () { var b = 0, c; for (c in a) a.hasOwnProperty(c) && b++; return b }(), a) a.hasOwnProperty(n) && (!c && ! --m && (d(j) ? j = function () { var a = [].slice.call(arguments); k.apply(this, a), l() } : j[n] = function (a) { return function () { var b = [].slice.call(arguments); a && a.apply(this, b), l() } }(k[n])), g(a[n], j, b, n, h)) } else !c && l() } var h = !!a.test, i = a.load || a.both, j = a.callback || f, k = j, l = a.complete || f, m, n; c(h ? a.yep : a.nope, !!i), i && c(i) } var i, j, l = this.yepnope.loader; if (e(a)) g(a, 0, l, 0); else if (w(a)) for (i = 0; i < a.length; i++) j = a[i], e(j) ? g(j, 0, l, 0) : w(j) ? B(j) : Object(j) === j && h(j, l); else Object(a) === a && h(a, l) }, B.addPrefix = function (a, b) { z[a] = b }, B.addFilter = function (a) { x.push(a) }, B.errorTimeout = 1e4, null == b.readyState && b.addEventListener && (b.readyState = "loading", b.addEventListener("DOMContentLoaded", A = function () { b.removeEventListener("DOMContentLoaded", A, 0), b.readyState = "complete" }, 0)), a.yepnope = k(), a.yepnope.executeStack = h, a.yepnope.injectJs = function (a, c, d, e, i, j) { var k = b.createElement("script"), l, o, e = e || B.errorTimeout; k.src = a; for (o in d) k.setAttribute(o, d[o]); c = j ? h : c || f, k.onreadystatechange = k.onload = function () { !l && g(k.readyState) && (l = 1, c(), k.onload = k.onreadystatechange = null) }, m(function () { l || (l = 1, c(1)) }, e), i ? k.onload() : n.parentNode.insertBefore(k, n) }, a.yepnope.injectCss = function (a, c, d, e, g, i) { var e = b.createElement("link"), j, c = i ? h : c || f; e.href = a, e.rel = "stylesheet", e.type = "text/css"; for (j in d) e.setAttribute(j, d[j]); g || (n.parentNode.insertBefore(e, n), m(c, 0)) } }(this, document), Modernizr.load = function () { yepnope.apply(window, [].slice.call(arguments, 0)) };