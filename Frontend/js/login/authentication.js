var Authentication = function () {

    let handleLogin = function () {
        $('.login-form').submit(function (e) {
            e.preventDefault();
        }).validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            rules: {
                username: {
                    required: true
                },
                password: {
                    required: true
                },
                remember: {
                    required: false
                }
            },

            messages: {
                username: {
                    required: "Username is required."
                },
                password: {
                    required: "Passcode is required."
                }
            },

            invalidHandler: function (event, validator) { //display error alert on form submit   
                $('.alert-danger', $('.login-form')).show();
            },

            highlight: function (element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function (label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function (error, element) {
                error.insertAfter(element.closest('.input-icon'));
            },

            submitHandler: function (form) {
                $.ajax({
                    url: Config.getApiRootUrl() + '/authentication/login' + '/' + $('#password').val(),
                    beforeSend: function () {
                        Utils.blockUI({
                            target: '.content',
                            message: 'Loading...',
                            boxed: true
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
                            window.location.href = 'report_listing.html';//?id=' + json.SurveyID;
                        } else {
                            $('#validation-msg').text("Invalid Passcode");
                            $('.alert-danger', $('.login-form')).show();
                        }
                    }
                });
            }
        });

        $('.login-form input').keypress(function (e) {
            if (e.which == 13) {
                if ($('.login-form').validate().form()) {
                     $('.login-form').submit();
                }
                return false;
            }
        });
    }

    return {
        //main function to initiate the module
        init: function () {

            handleLogin();
            
            $.backstretch("../assets/pages/media/bg/2.jpg");
        }
    };

}();

jQuery(document).ready(function () {    
    Authentication.init();
});