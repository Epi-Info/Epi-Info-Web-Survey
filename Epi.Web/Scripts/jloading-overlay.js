/////////////////////////////////////////////////////
///Author: Andr� Leal & Pedro Sombreireiro
/////////////////////////////////////////////////////


(function ($) {
    var jLoadingOverlayId = 'jLoadingOverlay';
    var jLoadingOptions;
     $.fn.jLoadingOverlay = function (action, options) {
         jLoadingOptions = options;
         if (action == '') {

             var container = $('body');

             var wrapperClass = 'loading-wrapper';
             if (options) {
                 if (!options.fullscreen) {
                     wrapperClass = wrapperClass + ' inner-pane';
                     container = $(this);
                 }
             }


             container.append('<div id="' + jLoadingOverlayId + '" class="'+wrapperClass+'" style="display: none;"></div>');
             $('div#' + jLoadingOverlayId).append('<div class="loading-overlay"></div>');
             $('div#' + jLoadingOverlayId + ' div.loading-overlay').append('<div class="spinner"></div>');
             $('div#' + jLoadingOverlayId).append('<div class="overlay-bg"></div>');
             $('div#' + jLoadingOverlayId + ' div.spinner').append('<div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div>');
			 
			 if(options && options.backgroundColor){
				$('div#' + jLoadingOverlayId +' div.overlay-bg').css('background',options.backgroundColor);
			 }
			 
			 
			 
             $('div#' + jLoadingOverlayId).show();
         }
         else if('close'){
             $('div#' + jLoadingOverlayId).detach();
         }
     }
 })(jQuery);

