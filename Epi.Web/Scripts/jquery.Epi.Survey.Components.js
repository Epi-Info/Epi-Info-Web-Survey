
function PopulateDiv(id, name) {
  
}

function FilterRecords(ctrl , OrgId ) {
    var src = $(ctrl).val();
      //alert(src);
    //$("#imagePreview").html(src ? "<img src='" + src + "'>" : "");
    if (src == 1) {
        $(".metro-collect").hide();
        $(".metro-design-Shared").hide();
        $(".metro-design").show();
        
    }

    if (src == 2) {
        $(".metro-design").hide();
        $(".metro-design-Shared").hide();
        $(".metro-collect").show();
         
    }
    if (src == 3) {
        $(".metro-design").show();
        $(".metro-collect").show();
        $(".metro-design-Shared").show();
    }
    if(src==4){
       
        $(".metro-design-Shared").show();
        $(".metro-design").hide();
        $(".metro-collect").hide();
    }


    $(".NotSelectedOrg").hide();
}

function FilterRecordsByOrg(src) {
    //alert("src" + src);
    $("button[class*='metro-tile']").addClass('NotSelectedOrg');
    $(".metro-tile2").hide();
    $(".Org-" + src.toString()).show();
    $(".Org-" + src.toString()).removeClass('NotSelectedOrg');
    $("#right").hide();
    $("button[class*='metro-tile']").removeClass('metro-set');
    $("button:visible").triggerHandler('click');
}

function FilterRecordsByOrgMobile(ctrl) {
   
    //alert(src);
    $("button[class*='metro-tile']").addClass('NotSelectedOrg');
    $(".metro-tile2").hide();
    $(".Org-" + ctrl.toString()).show();
    $(".Org-" + ctrl.toString()).removeClass('NotSelectedOrg');
    $("#right").hide();
    $("button[class*='metro-tile']").removeClass('metro-set');
}



function NotifyByEmail(emailAddress, redirectUrl, surveyName, postUrl,passCode,EmailSubject) {
    /*post email address and redirect url asynchronously to Post controller */

    var user = { 'emailAddress': emailAddress,
        'redirectUrl': redirectUrl,
        'surveyName': escape(surveyName),
        'passCode':passCode,
        'EmailSubject':escape(EmailSubject),
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

        function AjaxCallToActionMethod(url, method, sucessFunc) {
            $.ajax({
                url: url,
                type: method,
                contentType: 'application/json; charset=utf-8',
                //data: JSON.stringify(model)
                sucess: successFunc
            });
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

                    //alert('Unable to sign out');
                    window.location.href = homePageUrl; //rerirecting to home page

                }
            },
            'json'
        );
}
function UpdateResponse(UpdateUrl,pName, pValue,responseId) {
    //post to the UpdateResponse action method and signout after that redirect to home page
   
    var user = {'NameList': pName,
        'Value': pValue,
        'responseId':responseId,
        __RequestVerificationToken: $("input[name=__RequestVerificationToken]").val()
    };

    $.post(
            UpdateUrl,
            user,
            function (data) {
                if (data === true) {
                 
                }
                else {

                    alert('Unable to Update');

                }
            },
            'json'
        );
}



function SaveAndUpdate(UpdateUrl,pName, pValue,responseId) {
     
   
    var user = {'Name': pName,
        'Value': pValue,
        'responseId':responseId,
        __RequestVerificationToken: $("input[name=__RequestVerificationToken]").val()
    };

    $.post(
            UpdateUrl,
            user,
            function (data) {
                if (data === true) {
                 //populate IsSaved and StatusId hidden fileds
          
            document.getElementById("HiddenStatusId").value =1;
            document.getElementById("HiddenIsSaved").value = true;
                }
                else {

                    alert('Unable to Update');

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