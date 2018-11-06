var Dashboard = /** @class */ (function () {
    function Dashboard() {
    }
    return Dashboard;
}());
var Report = /** @class */ (function () {
    function Report() {
    }
    return Report;
}());
var Dataset = /** @class */ (function () {
    function Dataset() {
    }
    return Dataset;
}());
var ViewModel = /** @class */ (function () {
    function ViewModel() {
    }
    return ViewModel;
}());
$(function () {
    var dashboardsList = $("#dashboards-list");
    var reportsList = $("#reports-list");
    var datasetsList = $("#datasets-list");
    var viewModel = window['viewModel'];
    viewModel.dashboards.forEach(function (dashboard) {
        var li = $("<li>");
        li.append($("<i>").addClass("fa fa-bar-chart"));
        li.append($("<a>", {
            "href": "javascript:void(0);"
        }).text(dashboard.displayName).click(function () { embedDashboard(dashboard); }));
        dashboardsList.append(li);
    });
    viewModel.reports.forEach(function (report) {
        var li = $("<li>");
        li.append($("<i>").addClass("fa fa-bar-chart"));
        li.append($("<a>", {
            "href": "javascript:void(0);"
        }).text(report.name).click(function () { embedReport(report); }));
        reportsList.append(li);
    });
    viewModel.datasets.forEach(function (dataset) {
        var li = $("<li>");
        li.append($("<i>").addClass("fa fa-bar-chart"));
        li.append($("<a>", {
            "href": "javascript:void(0);"
        }).text(dataset.name).click(function () { embedNewReport(dataset); }));
        datasetsList.append(li);
    });
});
var embedReport = function (report) {
    var viewModel = window['viewModel'];
    var token = viewModel.token;
    var models = window['powerbi-client'].models;
    var config = {
        type: 'report',
        id: report.id,
        embedUrl: report.embedUrl,
        accessToken: token,
        tokenType: models.TokenType.Aad,
        permissions: models.Permissions.All,
        viewMode: models.ViewMode.View,
        displayOptions: 1,
        settings: {
            filterPaneEnabled: false,
            navContentPaneEnabled: true
            //,background: models.BackgroundType.Transparent
        }
    };
    // Get a reference to the embedded report HTML element
    var reportContainer = document.getElementById('embed-container');
    var powerbi = window['powerbi'];
    // Embed the report and display it within the div container.
    powerbi.reset(reportContainer);
    var embeddedReport = powerbi.embed(reportContainer, config);
    window.location.hash = "report:" + report.id;
};
var embedDashboard = function (dashboard) {
    var viewModel = window['viewModel'];
    var token = viewModel.token;
    var models = window['powerbi-client'].models;
    var config = {
        type: 'dashboard',
        id: dashboard.id,
        embedUrl: dashboard.embedUrl,
        accessToken: token,
        tokenType: models.TokenType.Aad,
        pageView: "fitToWidth" // options: "actualSize", "fitToWidth", "oneColumn"
    };
    // Get a reference to the embedded report HTML element
    var reportContainer = document.getElementById('embed-container');
    var powerbi = window['powerbi'];
    // Embed the report and display it within the div container.
    powerbi.reset(reportContainer);
    var embeddedReport = powerbi.embed(reportContainer, config);
    window.location.hash = "dashboard:" + dashboard.id;
};
var embedNewReport = function (dataset) {
    var viewModel = window['viewModel'];
    var token = viewModel.token;
    var models = window['powerbi-client'].models;
    var config = {
        datasetId: dataset.id,
        embedUrl: "https://app.powerbi.com/reportEmbed",
        accessToken: token,
        tokenType: models.TokenType.Aad
    };
    // Get a reference to the embedded report HTML element
    var reportContainer = document.getElementById('embed-container');
    var powerbi = window['powerbi'];
    // Embed the report and display it within the div container.
    powerbi.reset(reportContainer);
    var embeddedReport = powerbi.createReport(reportContainer, config);
    window.location.hash = "dataset:" + dataset.id;
};
window.onhashchange = function () {
    // this.console.log("Location: " + location.hash);
};
//# sourceMappingURL=app.js.map