


function NotifyByEmail(emailAddress,redirectUrl) {
    /*post email address and redirect url asynchronously to Post controller */
    //debugger;
    var user = { 'emailAddress': emailAddress,
                 'redirectUrl':redirectUrl
    };
    $.post(
            '/Post/Notify',
            user,
            function (data) {
                if (data === true) {
                    alert('An email has been sent with survey link.');
                }
                else {

                    alert('Failed to send email to the participant');

                }
            },
            'json'
        );

        }

/*generating Url*/
        function GetRedirectionUrl() {

            var responseId = $('#_responseId').val();
            var statusId = $('#_statusId').val();
            var currentUrl = window.location.href;
            var currentUrlArray = [];
            currentUrlArray = currentUrl.split("/");
            var responseUrl = currentUrlArray[0] + "//" + currentUrlArray[2] + "/Home/" + responseId + '/' + statusId;
            return responseUrl;
        }

function ValidateEmail($email) {
/*Email validation*/
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    if ($email.length == 0) {
        return false;
    }
    if (!emailReg.test($email)) {
        return false;
    } else {
        return true;
    }
}