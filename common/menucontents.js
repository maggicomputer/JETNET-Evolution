/* Client-side access to querystring name=value pairs
Version 1.3
28 May 2008
	
License (Simplified BSD):
http://adamv.com/dev/javascript/qslicense.txt
*/
function Querystring(qs) { // optionally pass a querystring to parse
	this.params = {};

	if (qs == null) qs = location.search.substring(1, location.search.length);
	if (qs.length == 0) return;

	// Turn <plus> back to <space>
	// See: http://www.w3.org/TR/REC-html40/interact/forms.html#h-17.13.4.1
	qs = qs.replace(/\+/g, ' ');
	var args = qs.split('&'); // parse out name/value pairs separated via &

	// split out each name=value pair
	for (var i = 0; i < args.length; i++) {
		var pair = args[i].split('=');
		var name = decodeURIComponent(pair[0]);

		var value = (pair.length == 2)
			? decodeURIComponent(pair[1])
			: name;

		this.params[name] = value;
	}
}

Querystring.prototype.get = function(key, default_) {
	var value = this.params[key];
	return (value != null) ? value : default_;
}

Querystring.prototype.contains = function(key) {
	var value = this.params[key];
	return (value != null);
}



//anylinkmenu.init("menu_anchors_class") //Pass in the CSS class of anchor links (that contain a sub menu)
anylinkmenu.init("menuanchorclass");

       function search_me() {
        view = "";
        market = "";
        statused = "";
        model = "";
        searched = "";
        first = "";
        last = "";
        comp = "";
        status = "";
        user = "";
        ordered = "";
        doc = "";
        
        if (document.getElementById("search_for_cbo").value == "1") {
        	pagename = "listing.aspx";
        	comp = document.getElementById("comp_name_txt")
        	searched = document.getElementById("search_for_txt").value;
        	if (comp == null || comp == "") {
        		comp = "";
        	} else {
        		comp = document.getElementById("comp_name_txt").value
        	}
        	status = document.getElementById("status_cbo")
        	if (status == null || status == "" ) {
        		status = "";
        	} else {
        		status = document.getElementById("status_cbo").value
        	}
        
        } else if (document.getElementById("search_for_cbo").value == "2") {
        pagename = "listing_contact.aspx";
        first = document.getElementById("first_name").value;
        last = document.getElementById("last_name").value;
        comp = document.getElementById("comp_name_txt")
        if (comp == null || status == "") {
        	comp = "";
        } else {
        comp = document.getElementById("comp_name_txt").value
       }

       status = document.getElementById("status_cbo")
       if (status == null || status == "") {
       	status = "";
       } else {
       status = document.getElementById("status_cbo").value
       }

       ordered = document.getElementById("ordered_by")
       if (ordered == null || ordered == "") {
       	ordered = "";
       } else {
       ordered = document.getElementById("ordered_by").value
       }

        } else if (document.getElementById("search_for_cbo").value == "3") {
        pagename = "listing_air.aspx";
        searched = document.getElementById("search_for_txt").value;
            statused = document.getElementById("market_status_cbo").value;
            model = document.getElementById("model_cbo").value;
            searched = document.getElementById("search_for_txt").value;
        } else if (document.getElementById("search_for_cbo").value == "4") {
        pagename = "listing_action.aspx";
        searched = document.getElementById("search_for_txt").value;
            //model = document.getElementById("model_cbo").value;
        view = document.getElementById("view_cbo").value;
            user = document.getElementById("display_cbo").value; 
        } else if (document.getElementById("search_for_cbo").value == "5") {
        pagename = "listing_jobs.aspx";
        searched = document.getElementById("search_for_txt").value;
        } else if (document.getElementById("search_for_cbo").value == "6") {
        pagename = "listing_notes.aspx";

        doc = document.getElementById("document_status").value;
        if (doc == null || doc == "") {
        	doc = "";
        } else {
        doc = document.getElementById("document_status").value
        }
        
         user = document.getElementById("display_cbo").value; 
        searched = document.getElementById("search_for_txt").value;
       } else if (document.getElementById("search_for_cbo").value == "7") {
       	pagename = "listing_opportunities.aspx";
       	model = document.getElementById("model_cbo").value;
       	user = document.getElementById("display_cbo").value;
       	searched = document.getElementById("search_for_txt").value;
       }

       url = pagename + "?";

       if (doc != "") {
       	url = url + "doc=" +doc;
       }
       
        if (status != "") {
        	url = url + "status=" + status;
        }
        
        if (comp != "") {
        	url = url + "&comp=" + comp;
        }

        if (last != "") {
        	url = url + "&last=" + last;
        }
        if (ordered != "") {
        	url = url + "&ordered=" + ordered;
        }

        if (first != "") {
        	url = url + "&first=" + first;
        }
        if (user != "") {
        	url = url + "&user=" + user;
        }

        if (searched != "") {
        	url = url + "&search=" + searched;
        }

        if (view != "") {
        	url = url + "&view=" + view;
        }


        if (statused != "") {
        	url = url + "&status=" + statused;
        }


        if (model != "") {
        	url = url + "&model=" + model;
        }
        
        url = url + "&parent=" + document.getElementById("search_for_cbo").value + "&sort=" + document.getElementById("search_where").value
        document.location = url;
    }


window.name = "main";
