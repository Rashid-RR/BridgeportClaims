<?php
/**
 * Business Point functions and definitions.
 *
 * @link https://developer.wordpress.org/themes/basics/theme-functions/
 *
 * @package Business_Point
 */

if ( ! function_exists( 'business_point_setup' ) ) :
/**
 * Sets up theme defaults and registers support for various WordPress features.
 *
 * Note that this function is hooked into the after_setup_theme hook, which
 * runs before the init hook. The init hook is too late for some features, such
 * as indicating support for post thumbnails.
 */
function business_point_setup() {
	/*
	 * Make theme available for translation.
	 * Translations can be filed in the /languages/ directory.
	 * If you're building a theme based on Business Way, use a find and replace
	 * to change 'business-point' to the name of your theme in all the template files.
	 */
	load_theme_textdomain( 'business-point' );

	// Add default posts and comments RSS feed links to head.
	add_theme_support( 'automatic-feed-links' );

	/*
	 * Let WordPress manage the document title.
	 * By adding theme support, we declare that this theme does not use a
	 * hard-coded <title> tag in the document head, and expect WordPress to
	 * provide it for us.
	 */
	add_theme_support( 'title-tag' );

	/*
	 * Enable support for custom logo.
	 */
	add_theme_support( 'custom-logo' );

	/*
	 * Enable support for Post Thumbnails on posts and pages.
	 *
	 * @link https://developer.wordpress.org/themes/functionality/featured-images-post-thumbnails/
	 */
	add_theme_support( 'post-thumbnails' );
	add_image_size('business-point-long', 370, 500, true);
	add_image_size('business-point-small', 370, 235, true);

	// Register navigation menu locations.
	register_nav_menus( array(
		'top' 		=> esc_html__( 'Top Header', 'business-point' ),
		'primary' 	=> esc_html__( 'Primary Header', 'business-point' ),
		'social'  	=> esc_html__( 'Social Links', 'business-point' ),
	) );

	/*
	 * Switch default core markup for search form, comment form, and comments
	 * to output valid HTML5.
	 */
	add_theme_support( 'html5', array(
		'search-form',
		'comment-form',
		'comment-list',
		'gallery',
		'caption',
	) );

	// Set up the WordPress core custom background feature.
	add_theme_support( 'custom-background', apply_filters( 'business_point_custom_background_args', array(
		'default-color' => 'ffffff',
		'default-image' => '',
	) ) );
}
endif;
add_action( 'after_setup_theme', 'business_point_setup' );

/**
 * Set the content width in pixels, based on the theme's design and stylesheet.
 *
 * Priority 0 to make it available to lower priority callbacks.
 *
 * @global int $content_width
 */
function business_point_content_width() {
	$GLOBALS['content_width'] = apply_filters( 'business_point_content_width', 810 );
}
add_action( 'after_setup_theme', 'business_point_content_width', 0 );

/**
 * Register widget area.
 *
 * @link https://developer.wordpress.org/themes/functionality/sidebars/#registering-a-sidebar
 */
function business_point_widgets_init() {
	register_sidebar( array(
		'name'          => esc_html__( 'Sidebar', 'business-point' ),
		'id'            => 'sidebar-1',
		'description'   => esc_html__( 'Add widgets here to appear in Sidebar.', 'business-point' ),
		'before_widget' => '<section id="%1$s" class="widget %2$s">',
		'after_widget'  => '</section>',
		'before_title'  => '<h2 class="widget-title">',
		'after_title'   => '</h2>',
	) );
	register_sidebar( array(
		'name'          => esc_html__( 'Home Page Widget Area', 'business-point' ),
		'id'            => 'home-page-widget-area',
		'description'   => esc_html__( 'Add widgets here to appear in Home Page Widget Area.', 'business-point' ),
		'before_widget' => '<section id="%1$s" class="widget %2$s"><div class="container">',
		'after_widget'  => '</div></section>',
		'before_title'  => '<h2 class="widget-title">',
		'after_title'   => '</h2>',
	) );
	register_sidebar( array(
		'name'          => sprintf( esc_html__( 'Footer %d', 'business-point' ), 1 ),
		'id'            => 'footer-1',
		'before_widget' => '<section id="%1$s" class="widget %2$s">',
		'after_widget'  => '</section>',
		'before_title'  => '<h2 class="widget-title">',
		'after_title'   => '</h2>',
	) );
	register_sidebar( array(
		'name'          => sprintf( esc_html__( 'Footer %d', 'business-point' ), 2 ),
		'id'            => 'footer-2',
		'before_widget' => '<section id="%1$s" class="widget %2$s">',
		'after_widget'  => '</section>',
		'before_title'  => '<h2 class="widget-title">',
		'after_title'   => '</h2>',
	) );
	register_sidebar( array(
		'name'          => sprintf( esc_html__( 'Footer %d', 'business-point' ), 3 ),
		'id'            => 'footer-3',
		'before_widget' => '<section id="%1$s" class="widget %2$s">',
		'after_widget'  => '</section>',
		'before_title'  => '<h2 class="widget-title">',
		'after_title'   => '</h2>',
	) );
	register_sidebar( array(
		'name'          => sprintf( esc_html__( 'Footer %d', 'business-point' ), 4 ),
		'id'            => 'footer-4',
		'before_widget' => '<section id="%1$s" class="widget %2$s">',
		'after_widget'  => '</section>',
		'before_title'  => '<h2 class="widget-title">',
		'after_title'   => '</h2>',
	) );
}
add_action( 'widgets_init', 'business_point_widgets_init' );

/**
* Enqueue scripts and styles.
*/
function business_point_scripts() {

	wp_enqueue_style( 'business-point-fonts', business_point_fonts_url(), array(), null );

	wp_enqueue_style( 'jquery-meanmenu', get_template_directory_uri() . '/assets/third-party/meanmenu/meanmenu.css' );

	wp_enqueue_style( 'jquery-slick', get_template_directory_uri() . '/assets/third-party/slick/slick.css', '', '1.6.0' );

	wp_enqueue_style( 'font-awesome', get_template_directory_uri() . '/assets/third-party/font-awesome/css/font-awesome.min.css', '', '4.7.0' );

	wp_enqueue_style( 'business-point-style', get_stylesheet_uri() );

	wp_enqueue_script( 'business-point-navigation', get_template_directory_uri() . '/assets/js/navigation.js', array(), '20151215', true );

	wp_enqueue_script( 'business-point-skip-link-focus-fix', get_template_directory_uri() . '/assets/js/skip-link-focus-fix.js', array(), '20151215', true );

	wp_enqueue_script( 'jquery-cycle2', get_template_directory_uri() . '/assets/third-party/cycle2/js/jquery.cycle2.min.js', array( 'jquery' ), '2.1.6', true );

	wp_enqueue_script( 'jquery-meanmenu', get_template_directory_uri() . '/assets/third-party/meanmenu/jquery.meanmenu.js', array('jquery'), '2.0.2', true );

	wp_enqueue_script( 'jquery-slick', get_template_directory_uri() . '/assets/third-party/slick/slick.js', array('jquery'), '1.6.0', true );

	wp_enqueue_script( 'business-point-custom', get_template_directory_uri() . '/assets/js/custom.js', array( 'jquery' ), '1.0.3', true );

	if ( is_singular() && comments_open() && get_option( 'thread_comments' ) ) {
		wp_enqueue_script( 'comment-reply' );
	}
}
add_action( 'wp_enqueue_scripts', 'business_point_scripts' );

/**
* Enqueue scripts and styles for admin >> widget page only.
*/
function business_point_admin_scripts( $hook ) {

	if( 'widgets.php' === $hook ){

		wp_enqueue_style( 'business-point-admin', get_template_directory_uri() . '/includes/widgets/css/admin.css', array(), '1.0.3' );

		wp_enqueue_media();

		wp_enqueue_script( 'business-point-admin', get_template_directory_uri() . '/includes/widgets/js/admin.js', array( 'jquery' ), '1.0.3' );

	}

}

add_action( 'admin_enqueue_scripts', 'business_point_admin_scripts' );

// Load main file.
require_once trailingslashit( get_template_directory() ) . '/includes/main.php';