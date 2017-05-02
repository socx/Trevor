var Utils = function () {

    return {
        getApiDateRange: function (fromDate, toDate){
            var fromDateArray = fromDate.split('/');
            var startDate = fromDateArray[2] + '-' + fromDateArray[1] + '-' + fromDateArray[0];
            
            var toDateArray = toDate.split('/');
            var endDate = toDateArray[2] + '-' + toDateArray[1] + '-' + toDateArray[0];
            return {StartDate: startDate, EndDate : endDate};
        },
        getPasscode: function () {
            let passcode = '';
            if (typeof(Storage) !== "undefined") {
                var haloauthdata = localStorage.getItem("halo_auth_data");
                if(haloauthdata){
                    let dataArray = countryArray = haloauthdata.split(';');
                    passcode = dataArray[0];
                    let sid = dataArray[1];
                    let expiry = dataArray[2];

                    $.ajax({
                        url: Config.getApiRootUrl() + '/reports/login' + '/' + passcode,
                        beforeSend: function () {
                            Utils.blockUI({
                                target: '.content',
                                message: 'Loading...'
                            });
                        },
                        complete: function () {
                            Utils.unblockUI('.content');
                        },
                        success: function (json) {
                            //authenticate via api
                            if(json.Authenticated)
                            {
                                let authData = json.Passcode + ';' + json.SurveyID + ';' + json.ExpiryDate;
                                localStorage.setItem("halo_auth_data", authData);
                            } else {
                                passcode = '';
                            }
                        }
                    });
                }
            }
            return passcode;
        },
        blockUI : function (options) {             
            options = $.extend(true, {}, options);
            let html = '';
            if (options.animate) {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '">' + '<div class="block-spinner-bar"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div>' + '</div>';
            } else if (options.iconOnly) {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><img src="' + App.getGlobalImgPath() + 'loading-spinner-grey.gif" align=""></div>';
            } else if (options.textOnly) {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><span>&nbsp;&nbsp;' + (options.message ? options.message : 'LOADING...') + '</span></div>';
            } else {
                html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><img src="' + App.getGlobalImgPath() + 'loading-spinner-grey.gif" align=""><span>&nbsp;&nbsp;' + (options.message ? options.message : 'LOADING...') + '</span></div>';
            }

            if (options.target) { // element blocking
                let el = $(options.target);
                if (el.height() <= ($(window).height())) {
                    options.cenrerY = true;
                }
                el.block({
                    message: html,
                    baseZ: options.zIndex ? options.zIndex : 1000,
                    centerY: options.cenrerY !== undefined ? options.cenrerY : false,
                    css: {
                        top: '10%',
                        border: '0',
                        padding: '0',
                        backgroundColor: 'none'
                    },
                    overlayCSS: {
                        backgroundColor: options.overlayColor ? options.overlayColor : '#555',
                        opacity: options.boxed ? 0.05 : 0.1,
                        cursor: 'wait'
                    }
                });
            } else { // page blocking
                $.blockUI({
                    message: html,
                    baseZ: options.zIndex ? options.zIndex : 1000,
                    css: {
                        border: '0',
                        padding: '0',
                        backgroundColor: 'none'
                    },
                    overlayCSS: {
                        backgroundColor: options.overlayColor ? options.overlayColor : '#555',
                        opacity: options.boxed ? 0.05 : 0.1,
                        cursor: 'wait'
                    }
                });
            }
        },
        unblockUI: function(target) {
            if (target) {
                $(target).unblock({
                    onUnblock: function () {
                        $(target).css('position', '');
                        $(target).css('zoom', '');
                    }
                });
            } else {
                $.unblockUI();
            }
        },
        getQueryStringParameterValue :  function(key, url) {
            if (!url) url = window.location.href;
            url = url.toLowerCase(); // This is just to avoid case sensitivity  
            key = key.replace(/[\[\]]/g, "\\$&").toLowerCase();// This is just to avoid case sensitiveness for query parameter name
            let regex = new RegExp("[?&]" + key + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }
    };

}();

var noTrendChart = false;

jQuery(document).ready(function () {
    $('.btn-log-out').click(function (e) {
        e.preventDefault();
        if (typeof (Storage) !== "undefined") {
            localStorage.removeItem("halo_auth_data");
            window.location.href = 'client-login.html';
        }
    });
});