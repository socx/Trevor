var ReportsFlags = function () {
    return {
        init : function (queryString){
            if (queryString) {
                $.ajax({
                    beforeSend: function () {
                        Utils.blockUI({
                            target: '#red-flag-div',
                            message: 'Loading Flag Responses Data. <br/>Please wait...', 
                            boxed : true
                        });
                    },
                    complete: function () {
                        Utils.unblockUI('#red-flag-div');
                    },
                    url: Config.getApiRootUrl() + '/reports/flagdata' + '?' + queryString,
                    success: function (json) {
                        ReportsFlags.initFlagSections(json);
                    },
                    failure: function (errMsg) {
                        alert(errMsg); //TODO; Use Modal
                    }
                });
            }
        },  
        initFlagSections: function (flagData) {
            $('#amber-flag').attr("src", Config.getAppRootUrl() + "/images/amber_flag_32.jpg");
            $('#red-flag').attr("src", Config.getAppRootUrl() + "/images/red_flag_32.jpg");
            $('.flag-current-period').text('(' + flagData.Current.Period + ')');
            $('#current-redflag').text(flagData.Current.RedFlags);
            $('#current-amberflag').text(flagData.Current.AmberFlags);
            $('#red-flag-span').text(flagData.Current.RedFlags);
            $('#amber-flag-span').text(flagData.Current.AmberFlags);
            $('.flag-previous-period').text('(' + flagData.Previous.Period + ')');
            $('#previous-redflag').text(flagData.Previous.RedFlags);
            $('#previous-amberflag').text(flagData.Previous.AmberFlags);

            // Hide or show section 
            if((flagData.Current.RedFlags < 1 ) && (flagData.Previous.RedFlags < 1 ))
                $('#red-flag-div').addClass('display-hide');
            if((flagData.Current.AmberFlags < 1 ) && (flagData.Previous.AmberFlags < 1))
                $('#amber-flag-div').addClass('display-hide');
            
        }
    };

}();