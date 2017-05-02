var ReportsPeriodComments = function () {
    return {
        init : function (queryString){
            if (queryString) {
                $.ajax({
                    beforeSend: function () {
                        Utils.blockUI({
                            target: '#period-comments-div',
                            message: 'Loading Direct. <br/>Please wait...'
                        });
                    },
                    complete: function () {
                        Utils.unblockUI('#period-comments-div');
                    },
                    url: Config.getApiRootUrl() + '/reports/periodcommentsdata' + '?' + queryString,
                    success: function (json) {      
                        if(json.length  > 0) {
                            $('#period-comments-div').removeClass('display-hide');
                            ReportsPeriodComments.initPeriodCommentsSections(json);
                        }else{
                            $('#period-comments-div').addClass('display-hide');
                        }
                    }
                });
            }
        },
        initPeriodCommentsSections: function(data) {
            $('#period-comments-div').empty();
            $('#period-comments-div').append('<p class="section-header">Comments for this period:</p>'); 
            $('#period-comments-div').append('<p>This section lists all of the comments submitted in this reporting period as well as highlighting any comments linked to either a red and/or amber flag response. </p>');
            let html = 
                '<table style="width: 100%; border: 1px solid #cccccc; border-collapse: collapse;" >' +
                    '<tr>' +
                        '<th width="80%" style="text-align: left; border: 1px solid #cccccc">Comment</th>' +
                        '<th width="10%" style="text-align: center; border: 1px solid #cccccc" nowrap>Red Flag</th>' +
                        '<th width="10%" style="text-align: center; border: 1px solid #cccccc" nowrap>Amber Flag</th></tr>';
            $.each(data, function (key, entry) {
                html += '<tr>'
                html += '<td style="border: 1px solid #cccccc">' + entry.Comment.replace(/(\r\n|\n|\r)/gm," ") + ' </td>';
                html += '<td style="text-align: center; border: 1px solid #cccccc">';
                html += entry.RedFlag ? "<img src='" + Config.getAppRootUrl() + "/images/flag_red.png' />"  : '&nbsp;';
                html +=  '</td>';
                html += '<td style="text-align: center; border: 1px solid #cccccc">';
                html += entry.AmberFlag ? "<img src='" + Config.getAppRootUrl() + "/images/flag_yellow.png' />"  : '&nbsp;';
                html +=  '</td>';
                html += '</tr>'
            });
            html += '</table>';            
			
            $('#period-comments-div').append(html);
            $('#period-comments-div').append('<div style="padding:10px"></div>');                         
            $('#period-comments-div').addClass('display-hide');
        }
    };

}();