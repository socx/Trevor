var ReportsResponses = function () {
    return {
        init : function (queryString){
            if (queryString) {
                $.ajax({
                    beforeSend: function () {
                        Utils.blockUI({ target: '#number-of-responses-div', message: 'Loading Number of Responses. <br/>Please wait...', boxed : true });
                    },
                    complete: function () {
                        Utils.unblockUI('#number-of-responses-div');
                    },
                    url: Config.getApiRootUrl() + '/reports/responsedata?' + queryString,
                    success: function (json) {
                        if(json)
                            ReportsResponses.updateResponsesSection(json);                    
                    },
                    failure: function (errMsg) {
                        alert(errMsg); //TODO; Use Modal
                    }
                });
            }
        },
        updateResponsesSection : function (responseData) {
            $('#current-period-respondents').text(responseData.Current.Respondents);
            $('#current-period').text('(' + responseData.Current.Period + ')');
            $('#previous-period-respondents').text(responseData.Previous.Respondents);
            $('#previous-period').text('(' + responseData.Previous.Period + ')');        
        }
    };

}();