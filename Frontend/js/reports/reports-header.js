var ReportsHeader = function () {
    return {
        init : function (passcode){
            if (passcode) {
                $.ajax({
                    beforeSend: function () {
                        Utils.blockUI({ target: '#report-header', message: 'Loading Number of Responses. <br/>Please wait...', boxed : true });
                    },
                    complete: function () {
                        Utils.unblockUI('#report-header');
                    },
                    url: Config.getApiRootUrl() + '/reports/surveylist/' + passcode ,
                    success: function (json)  {
                        if((json) && json.length > 0)
                            ReportsHeader.drawHeader(json[0]);                    
                    },
                    failure: function (errMsg) {
                        alert(errMsg); //TODO; Use Modal
                    }
                });
            }
        },
        drawHeader : function (headerInfo) {            
            $('#report-header').html(headerInfo.SurveyName + ' Report');
            $('#survey-logo').attr("src", Config.getHaloImgPath() + "/Logos/" + headerInfo.LogoFile);      
        },
    };

}();