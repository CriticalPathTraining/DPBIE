var myApp;
(function (myApp) {
    var appSettings = /** @class */ (function () {
        function appSettings() {
        }
        appSettings.clientId = "";
        appSettings.appWorkspaceId = "";
        appSettings.tenant = "YOUR_DOMAIN.onMicrosoft.com";
        return appSettings;
    }());
    myApp.appSettings = appSettings;
})(myApp || (myApp = {}));
//# sourceMappingURL=appSettings.js.map