


function NotifyByEmail(emailAddress, redirectUrl, surveyName, postUrl,passCode) {
    /*post email address and redirect url asynchronously to Post controller */

    var user = { 'emailAddress': emailAddress,
        'redirectUrl': redirectUrl,
        'surveyName': surveyName,
        'passCode':passCode,
        __RequestVerificationToken: $("input[name=__RequestVerificationToken]").val()
    };

    $.post(
            postUrl,
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

function SignOutAndRedirect(signoutUrl,homePageUrl) {
    //post to the login/SignOut action method and signout after that redirect to home page
   
    var user = {
        __RequestVerificationToken: $("input[name=__RequestVerificationToken]").val()
    };

    $.post(
            signoutUrl,
            user,
            function (data) {
                if (data === true) {
                    window.location.href = homePageUrl; //rerirecting to home page
                }
                else {

                    alert('Unable to sign out');

                }
            },
            'json'
        );
}

/*generating Url*/
function GetRedirectionUrl() {
    //debugger;
    // return to survey url: 'http://hostname/survey/responseid'
    var currentUrl = window.location.href;
    currentUrl = processUrl(currentUrl, 'RedirectionUrl', "");
    return currentUrl;
}

function processUrl(currentUrl, processType, pageNumber) {
    //debugger;
    var currentUrlArray = [];
    currentUrlArray = currentUrl.split("/");
    var intRegex = /^\d+$/;

    switch (processType) {
        case 'RedirectionUrl':

            if (intRegex.test(currentUrlArray[currentUrlArray.length - 1])) { //if page number  attached to url remove the number
                currentUrlArray.splice(currentUrlArray.length - 1, 1);
                currentUrl = currentUrlArray.join("/");
            }

            break;
        case 'PreviousUrl':

            var pageNumberP;
            pageNumberP = parseInt(pageNumber) - 1;
            if (!intRegex.test(currentUrlArray[currentUrlArray.length - 1])) { //if page number not attached to url
                currentUrl = currentUrl + "/" + pageNumberP.toString();
            }
            else { //if page number attached to url
                currentUrlArray[currentUrlArray.length - 1] = pageNumberP;
                currentUrl = currentUrlArray.join("/");
            }
            break;
        case 'ContinueUrl':
            var pageNumberC;
            pageNumberC = parseInt(pageNumber) + 1;
            if (!intRegex.test(currentUrlArray[currentUrlArray.length - 1])) { //if page number not attached to url
                currentUrl = currentUrl + "/" + pageNumberC.toString();
            }
            else { //if page number attached to url
                currentUrlArray[currentUrlArray.length - 1] = pageNumberC;
                currentUrl = currentUrlArray.join("/");
            }
            break;

        default:
            //code to be executed if n is different from case 1 and 2
    }
    return currentUrl;
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