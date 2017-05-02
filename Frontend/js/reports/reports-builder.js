//TODO: REFACTOR (IN CUSTOMER CONFIDENCE)
function toggle_visibility(eid, tid) {
    var link = document.getElementById(eid);
    var table = document.getElementById(tid);
    if (table.style.display == 'block') {
        table.style.display = 'none';
        link.innerText = 'Show comments for this graph';
    }
    else {
        table.style.display = 'block';
        link.innerText = 'Hide comments for this graph'
    }
}

var ReportsBuilder = function () {
    return {
        buildQueryString: function () {
            let surveyID = '';
            if (typeof (Storage) !== "undefined") {
                surveyID = localStorage.getItem("halo_current_surveyid");
            }else{
                window.location.href = Config.getLoginPageUrl();
            }
            let dateRange = Utils.getApiDateRange($('#from').val(), $('#to').val());

            let startDate = dateRange.StartDate;
            let endDate = dateRange.EndDate;
            let allWithOrWithoutFilters = $('input[name=radiobtn-responses]:checked').val();

            let location1 = $("#location1 option:selected").val();
            let location2 = $("#location2 option:selected").val();
            let location3 = $("#location3 option:selected").val();
            let location4 = $("#location4 option:selected").val();
            let sla = $("#sla option:selected").val();
            let slabreach = $("#slaBreach option:selected").val();
            let slacontract = $("#slaContract option:selected").val();
            let slaservice = $("#slaService option:selected").val();
            let slaservicetype = $("#slaServiceType option:selected").val();
            let cause = $("#cause option:selected").val();
            let remedy = $("#remedy option:selected").val();
            let problem = $("#problem option:selected").val();
            let failure = $("#failure option:selected").val();
            
            let filterQuestionResponse1 = $("#filterQuestion1 option:selected").val();
            let filterQuestionPromptID1 = $("#filterQuestion1-promptid").text();
            let filterQuestionResponse2 = $("#filterQuestion2 option:selected").val();
            let filterQuestionPromptID2 = $("#filterQuestion2-promptid").text();
            const hasFilterQuestion = ((filterQuestionResponse1 && filterQuestionResponse1.length >0)
                                        || (filterQuestionResponse2 && filterQuestionResponse2.length >0))
                                        ? "True" : '';
            
             let queryString= 'SurveyID=' + surveyID;
             queryString += startDate ? '&StartDate=' + startDate : '';
             queryString += endDate ? '&EndDate=' + endDate : '';
             queryString += allWithOrWithoutFilters ? '&AllWithOrWithoutFilters=' + allWithOrWithoutFilters : '';
             queryString += location1 ? '&LocationLevel1=' + location1 : '';
             queryString += location2 ? '&LocationLevel2=' + location2 : '';
             queryString += location3 ? '&LocationLevel3=' + location3 : '';
             queryString += location4 ? '&LocationLevel4=' + location4 : '';
             queryString += sla ? '&SLA=' + sla : '';
             queryString += slabreach ? '&SLABreach=' + slabreach : '';
             queryString += slacontract ? '&SLAContract=' + slacontract : '';
             queryString += slaservice ? '&SLAService=' + slaservice : '';
             queryString += slaservicetype ? '&SLAServiceType=' + slaservicetype : '';
             queryString += cause ? '&Cause=' + cause : '';
             queryString += remedy ? '&Remedy=' + remedy : '';
             queryString += problem ? '&Problem=' + problem : '';
             queryString += failure ? '&Failure=' + failure : '';
             queryString += filterQuestionResponse1 ? '&FilterQuestionResponse1=' + filterQuestionResponse1 : '';
             queryString += filterQuestionPromptID1 ? '&FilterQuestionPromptID1=' + filterQuestionPromptID1 : '';
             queryString += filterQuestionResponse2 ? '&FilterQuestionResponse2=' + filterQuestionResponse2 : '';
             queryString += filterQuestionPromptID2 ? '&FilterQuestionPromptID2=' + filterQuestionPromptID2 : '';
             queryString += hasFilterQuestion ? '&HasFilterQuestion=' + hasFilterQuestion : '';
             //TODO: ADD OTHER PARAMETERS
            return queryString;
        },
        getDateRange : function (){            
            let fromDate = new Date($('#from').val());
            let toDate = new Date($('#to').val());
            let fromDateArray = $('#from').val().split('/');
            let toDateArray = $('#to').val().split('/');
            let fromDateString = fromDateArray[2] + fromDateArray[1] + fromDateArray[0];
            let endDateString = toDateArray[2]  + toDateArray[1] + toDateArray[0];
            let dateRange = '/' + fromDateString + '/' + endDateString;

            return dateRange;
        }
    };

}();

jQuery(document).ready(function () {
    
    $('#btn-build-report').click(function () {
        if (typeof (Storage) !== "undefined") {           
            $('#report-body').css('display', 'block');            
            
            let surveyID = localStorage.getItem("halo_current_surveyid"); 
            let passcode = Utils.getPasscode();
            let queryString = ReportsBuilder.buildQueryString();
            let dateRange = ReportsBuilder.getDateRange();

            ReportsHeader.init(passcode);
            ReportsResponses.init(queryString);			
            ReportsConfidenceScore.init(queryString);			
            ReportsConfidenceTrend.init(surveyID);
            ReportsCustomerExperience.init(queryString);
            ReportsRecognition.init(queryString);
            ReportsFlags.init(queryString);
            ReportsDirectComments.init(surveyID, dateRange);
            ReportsPeriodComments.init(queryString);

        } else {
            //Refactor show modal;
            alert('Your browser does not support local storage. \r\n Please contact administrator.')
        }
    });

});