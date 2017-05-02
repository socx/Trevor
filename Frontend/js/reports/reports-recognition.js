var ReportsRecognition = function () {
    return {
        init : function (queryString){
            if (queryString) {                
                $.ajax({
                    beforeSend: function () {
                        Utils.blockUI({
                            target: '#service-recognition-div',
                            message: 'Loading Staff Recognition Data. <br/>Please wait...', 
                            boxed : true
                        });
                    },
                    complete: function () {
                        Utils.unblockUI('#service-recognition-div');
                    },
                    url: Config.getApiRootUrl() + '/reports/recognitiondata' + '?' + queryString,
                    success: function (json) {
                        if(json && json.Recognitions)
                            ReportsRecognition.initRecognitionSection(json.Recognitions);
                    },
                    failure: function (errMsg) {
                        alert(errMsg); //TODO; Use Modal
                    }
                });
            }
        },
        initRecognitionSection: function (recognitions) {
            let total = 0;
            let html = '<ul id="recognitionList">';
            $.each(recognitions, function (key, value) {
                total += value.Count;
                html += '<li>' + value.Name + ' (' + value.Count + ') </li>';
                $.each(value.Comments, function (index, comment) {
                    html += comment + '<br/>';
                });
            });
            html += '</ul>';
                        
            $('#recognition-span').text(total);
            $('#service-recognition-comment-div').empty();
            $('#service-recognition-comment-div').append($(html));
            $('#gold-start').attr("src", Config.getAppRootUrl() + "/images/gold_star_32.jpg");
            
        }
    };

}();