var AccessControl = function () {
    return {

        //main function to initiate the module
        init: function () {
            let passcode = Utils.getPasscode();
            if (passcode == ''){
                window.location.href = 'client-login.html';
            }
        }

    };

}();