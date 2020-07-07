/// <reference name="MicrosoftAjax.js"/>

Type.registerNamespace("crmWebClient");

crmWebClient.ClientControl1 = function(element) {
    crmWebClient.ClientControl1.initializeBase(this, [element]);
}

crmWebClient.ClientControl1.prototype = {
    initialize: function() {
        crmWebClient.ClientControl1.callBaseMethod(this, 'initialize');
        
        // Add custom initialization here
    },
    dispose: function() {        
        //Add custom dispose actions here
        crmWebClient.ClientControl1.callBaseMethod(this, 'dispose');
    }
}
crmWebClient.ClientControl1.registerClass('crmWebClient.ClientControl1', Sys.UI.Control);

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
