



jQuery(document).ready(function() {
  /* The code here is executed on page load */
SetUpSlider();
});



function SetUpSlider() {
   /* Replacing all the paragraphs */
  jQuery('.slideoutToolTip p').replaceWith(function() {
  //jQuery('.slideoutToolTip p').empty();
    /* The style, class and title attributes of the p are copied to the slideout: */

    return '\
		<div class="slideOutTip ' + jQuery(this).attr('class') + '" style="' + jQuery(this).attr('style') + '">\
			\
			<div class="tipVisible">\
				<div class="tipIcon"><div class="plusIcon"></div></div>\
				<p class="tipTitle">' + jQuery(this).attr('title') + '</p>\
			</div>\
			\
			<div class="slideOutContent">\
				<p>' + jQuery(this).html() + '</p>\
			</div>\
		</div>';
  });

  jQuery('.slideOutTip').each(function() {

    /*
    Implicitly defining the width of the slideouts according to the width of its title,
    because IE fails to calculate it on its own.
    */

    jQuery(this).width(22);
  });

  /* Listening for the click event: */

  jQuery('.tipVisible').bind('click', function() {
    var tip = jQuery(this).parent();

    /* If a open/close animation is in progress, exit the function */
    if (tip.is(':animated'))
      return false;

    if (tip.find('.slideOutContent').css('display') == 'none') {
      tip.trigger('slideOut');
    }
    else tip.trigger('slideIn');

  });

  jQuery('.slideOutTip').bind('slideOut', function() {

    var tip = jQuery(this);
    var slideOut = tip.find('.slideOutContent');

    /* Closing all currently open slideouts: */
    jQuery('.slideOutTip.isOpened').trigger('slideIn');

    /* Executed only the first time the slideout is clicked: */
    if (!tip.data('dataIsSet')) {
      tip.data('origWidth', tip.width())
				.data('origHeight', tip.height())
				.data('dataIsSet', true);

      if (tip.hasClass('openTop')) {
        /*
        If this slideout opens to the top, instead of the bottom,
        calculate the distance to the bottom and fix the slideout to it.
        */

        tip.css({
          bottom: tip.parent().height() - (tip.position().top + tip.outerHeight()),
          top: 'auto'
        });

        /* Fixing the title to the bottom of the slideout, so it is not slid to the top on open: */
        tip.find('.tipVisible').css({ position: 'absolute', bottom: 3 });

        /* Moving the content above the title, so it can slide open to the top: */
        tip.find('.slideOutContent').remove().prependTo(tip);
      }

      if (tip.hasClass('openLeft')) {
        /*
        If this slideout opens to the left, instead of right, fix it to the
        right so the left edge can expand without moving the entire div:
        */
        tip.css({
          right: Math.abs(tip.parent().outerWidth() - (tip.position().left + tip.outerWidth())),
          left: 'auto'
        });

        tip.find('.tipVisible').css({ position: 'absolute', right: 3 });
      }
    }

    /* Resize the slideout to fit the content, which is then faded into view: */

    tip.addClass('isOpened').animate({
      width: Math.max(slideOut.outerWidth() -47, tip.data('origWidth')),
      height: slideOut.outerHeight() + tip.data('origHeight')
    }, function() {
      slideOut.fadeIn('fast');
    });

  }).bind('slideIn', function() {
    var tip = jQuery(this);

    /* Hide the content and restore the original size of the slideout: */

    tip.find('.slideOutContent').fadeOut('fast', function() {
      tip.animate({
        width: tip.data('origWidth'),
        height: tip.data('origHeight')
      }, function() {
        tip.removeClass('isOpened');
      });
    });

  });
}



