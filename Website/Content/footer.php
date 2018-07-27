<?php
/**
 * The template for displaying the footer
 *
 * Contains footer content and the closing of the #main and #page div elements.
 *
 * @package WordPress
 * @subpackage Twenty_Twelve
 * @since Twenty Twelve 1.0
 */
wp_footer();
$options = get_option('cOptn');
?>
<footer>
	<div class="main-wrapper">
    	<p>&copy;2016. Brideport Pharmacy Services.<br />All Rights Reserved.</p>
        <ul>
        	<li><a href="#"><img src="<?php bloginfo('template_directory'); ?>/images/fb-icon.png" /></a></li>
            <li><a href="#"><img src="<?php bloginfo('template_directory'); ?>/images/twitter-icon.png" /></a></li>
            <li><a href="#"><img src="<?php bloginfo('template_directory'); ?>/images/google-icon.png" /></a></li>
            <li><a href="#"><img src="<?php bloginfo('template_directory'); ?>/images/youtube-icon.png" /></a></li>
        </ul>
        <div class="clear"></div>
    </div>
</footer>
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src="<?php bloginfo('template_directory'); ?>/js/jquery.cycle.all.js"></script>
<script src="<?php bloginfo('template_directory'); ?>/js/bx/jquery.bxslider.min.js"></script>
<script>
$('.do-crousel').bxSlider({
  minSlides: 3,
  maxSlides: 2,
  slideWidth: 570,
  slideMargin: 30,
  auto: true,
  infiniteLoop: true,
});
$(document).ready(function(e) {
  $('.s3').cycle({
    fx:'fade',
    delay:1000,
    pager:'#nav',
    vertical: true
  });
});
$(window).resize(function() {
$('.s3 li').css('height', $('body').height()+'px');
$('.slider-area').css('height', $('body').height()+'px');
$('.page-banner,.banner-area').css('height', $('body').height()+'px');

});
$('.s3 li').css('height', $('body').height()+'px');
$('.slider-area').css('height', $('body').height()+'px');
$('.page-banner,.banner-area').css('height', $('body').height()+'px');
</script>
</body>
</html>