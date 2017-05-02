var ReportsDirectComments = function () {
    return {
        init : function (surveyID, dateRange){
            if (surveyID && dateRange) {
                $.ajax({
                    beforeSend: function () {
                        Utils.blockUI({
                            target: '#direct-comments-div',
                            message: 'Loading Direct. <br/>Please wait...', 
                            boxed : true
                        });
                    },
                    complete: function () {
                        Utils.unblockUI('#direct-comments-div');
                    },
                    url: Config.getApiRootUrl() + '/reports/directcommentsdata/' + surveyID + dateRange,
                    success: function (json) {                        
                        if(json.length  > 0) {
                            $('#direct-comments-div').removeClass('display-hide');
                            ReportsDirectComments.initDirectCommentsSections(json);
                        }else{
                            $('#direct-comments-div').addClass('display-hide');
                        }
                    }
                });
            }
        },
        initDirectCommentsSections : function(data) {
            let html = '<ul id="direct-comments">';
            $.each(data, function (key, comment) {
                html += '<li>' + comment.replace(/(\r\n|\n|\r)/gm," ") + ' </li>';
            });
            html += '</ul>';
            $('#direct-comments-main-div').html(html);
            $('#direct-comments-div').removeClass('display-hide');
        }
    };

}();