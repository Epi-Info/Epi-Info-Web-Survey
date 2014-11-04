
/*to make sure the form does not submit on enter. To make sure that the form does not submit only for textbox, numerictextbox, 
datepicker, check box we are passing the control id to the following jquery plugin so we can choose which control we want to prevent
from submitting. e.g we certainly dont want to prevent submit or save to perform submit when they are on focus
*/

(function ($) {

    $.fn.BlockEnter = function (id) {
        
        return this.each(function () {

            var $this = $(this);

            $('#' + id).focus(function (event) {
                
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });

            $('#' + id).keydown(function (event) {
                
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });

        });

    };
})(jQuery);