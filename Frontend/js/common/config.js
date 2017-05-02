var customerExperiencePageBroke = false;
var Config = function () {
    return {
        //main function to initiate the module
        getAppRootUrl: function () {
            return 'http://guardiance.thehaloworks.com:8080/halo_reports';
        },
        getAppRootUrl1: function () {
            return 'http://report.thehaloworks.com/frontend';
        },
        getApiRootUrl: function () {
            return 'http://report.thehaloworks.com/api';
        },
        getLoginPageUrl: function () {
            return 'http://report.thehaloworks.com/frontend/pages/client-login.html';
        },
        getHaloImgPath: function () {
            return 'http://guardiance.thehaloworks.com/Images';
        },
        getDirectInputUrl: function () {
            return 'https://guardiance.thehaloworks.com/client/report-list.aspx';
        },
        getCustomerExperiencePageBroke: function () {
            return customerExperiencePageBroke;
        }
    };

}();