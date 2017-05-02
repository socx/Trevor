
var CommentInput = function () {    

    return { 
        initCommentInput : function(data) {
            let today = moment().format('DD-MM-YYYY');
            $('#comment-date').val(today);
            $('.alert').addClass('display-hide');
        }
    };
}();


jQuery(document).ready(function () { 
    
    //check if authenticated
    AccessControl.init();

    CommentInput.initCommentInput();
    $('#btn-clear-comment').click(function () {
        $('#comment-input').val('');
    });

    $('#btn-submit-comment').click(function (e) {
        e.preventDefault();
        let comment = $('#comment-input').val();
        let surveyID = '';
        if (typeof (Storage) !== "undefined") {
            surveyID = localStorage.getItem("halo_current_surveyid");
        }else{
            window.location.href = Config.getApiRootUrl();
        }
        let request = {SurveyID : surveyID, Comment : comment};
        
        if(comment === "")
        {
            $('.alert').text("Please enter comment")
            $('.alert').addClass('alert-danger');
            $('.alert').removeClass('display-hide');
        }
        else
        {  
            $.ajax({
                beforeSend: function () {
                    Utils.blockUI({
                        target: '#comment-input-div',
                        message: 'Processing. <br/>Please wait...',
                        boxed : true
                    });
                },
                complete: function () {
                    Utils.unblockUI('#comment-input-div');
                },
                url: Config.getApiRootUrl() + '/reports/directcomment',
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(request),
                success: function (json) {
                    $('.alert').text("Comment submitted successfully")
                    $('.alert').addClass('alert-success');
                    $('.alert').removeClass('display-hide');
                },
                error: function(err) {
                    $('.alert').text("Error processing your request. Please try again.")
                    $('.alert').addClass('alert-danger');
                    $('.alert').removeClass('display-hide');
                }
            });
        }
        
    });
      
});