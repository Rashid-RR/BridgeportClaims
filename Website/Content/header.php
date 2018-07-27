<?php
/**
 * The Header template for our theme
 *
 * Displays all of the <head> section and everything up till <div id="main">
 *
 * @package WordPress
 * @subpackage Twenty_Twelve
 * @since Twenty Twelve 1.0
 */
?><!DOCTYPE html>
<!--[if IE 7]>
<html class="ie ie7" <?php language_attributes(); ?>>
<![endif]-->
<!--[if IE 8]>
<html class="ie ie8" <?php language_attributes(); ?>>
<![endif]-->
<!--[if !(IE 7) | !(IE 8)  ]><!-->
<html <?php language_attributes(); ?>>
<!--<![endif]-->
<head>
<meta charset="<?php bloginfo( 'charset' ); ?>" />
<meta name="viewport" content="width=device-width" />
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<title><?php wp_title( '|', true, 'right' ); ?></title>
<link rel="profile" href="http://gmpg.org/xfn/11" />
<link rel="pingback" href="<?php bloginfo( 'pingback_url' ); ?>" />
<link href="https://fonts.googleapis.com/css?family=Lato:100,100i,300,300i,400,400i,700,700i,900,900i" rel="stylesheet">
<link href="<?php bloginfo('template_directory'); ?>/images/fav-icon.png" rel="shortcut icon" type="image/x-icon" />
<?php
	/* We add some JavaScript to pages with the comment form
	 * to support sites with threaded comments (when in use).
	 */
	if ( is_singular() && get_option( 'thread_comments' ) )
		wp_enqueue_script( 'comment-reply' );
		global $options;global $logo;global $copyrite;
		$options = get_option('cOptn');
		$logo = $options['logo'];
		$copyright = $options['copyright'];
		$twitter_link = $options['twitter_link'];
		$facebook_link = $options['facebook_link'];
		$googleplus_link = $options['googleplus_link'];
		$rss_link = $options['rss_link'];
		$email_link = $options['email_link'];
		$phone_num = $options['phone_num'];
		$fax_num = $options['fax_num'];
		$email = $options['email'];
		$flicker_link = $options['flicker_link'];
		$header_caption = $options['header_caption'];
		$linkedin = $options['linkedin'];
		$youtube = $options['youtube'];
		$size = 334;
		$options['logo'] = wp_get_attachment_image($logo, array($size, $size), false);
		$att_img = wp_get_attachment_image($logo, array($size, $size), false); 
		$logoSrc = wp_get_attachment_url($logo);
		$att_src_thumb = wp_get_attachment_image_src($logo, array($size, $size), false);
?>
<?php wp_head(); ?>


</head>

<body <?php body_class(); ?>>
<?php //echo $options['twitter']; ?>

<header>
    <div class="main-wrapper">
        <div class="logo">
			<?php 
                if (!((get_post_meta($post->ID, 'wpcf-page-logo', TRUE))=='')) {
                $logo = get_post_meta($post->ID,'wpcf-page-logo',TRUE);
            ?>
                <a href="<?php echo site_url(); ?>" ><img src="<?php echo $logo; ?>" /></a>
            <?php
                } else { 
            ?>
                <a href="<?php echo site_url(); ?>" ><?php echo $options['logo']  ?></a>
            <?php } ?>
        </div>
        <div class="nav-area">
            <?php wp_nav_menu( array( 'theme_location' => 'primary', 'menu' => 'Main Menu' ) ); ?>
        </div>
        <a href="appointment" class="appointment-btn"><img src="<?php bloginfo('template_directory'); ?>/images/appointment-btn.png" /></a>
        <div class="clear"></div>
    </div>
</header>