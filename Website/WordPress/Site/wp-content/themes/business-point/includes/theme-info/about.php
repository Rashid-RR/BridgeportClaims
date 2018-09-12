<?php
/**
 * About configuration
 *
 * @package Business_Point
 */

$config = array(
	'menu_name' => esc_html__( 'About Business Point', 'business-point' ),
	'page_name' => esc_html__( 'About Business Point', 'business-point' ),

	/* translators: theme version */
	'welcome_title' => sprintf( esc_html__( 'Welcome to %s - ', 'business-point' ), 'Business Point' ),

	/* translators: 1: theme name */
	'welcome_content' => sprintf( esc_html__( 'We hope this page will help you to setup %1$s with few clicks. We believe you will find it easy to use and perfect for your website development.', 'business-point' ), 'Business Point' ),

	// Quick links.
	'quick_links' => array(
		'theme_url' => array(
			'text' => esc_html__( 'Theme Details','business-point' ),
			'url'  => 'https://promenadethemes.com/downloads/business-point/',
			),
		'demo_url' => array(
			'text' => esc_html__( 'View Demo','business-point' ),
			'url'  => 'https://promenadethemes.com/demo/business-point/',
			),
		'documentation_url' => array(
			'text'   => esc_html__( 'View Documentation','business-point' ),
			'url'    => 'https://promenadethemes.com/documentation/business-point/',
			'button' => 'primary',
			),
		'rate_url' => array(
			'text' => esc_html__( 'Rate This Theme','business-point' ),
			'url'  => 'https://wordpress.org/support/theme/business-point/reviews/',
			),
		),

	// Tabs.
	'tabs' => array(
		'getting_started'     => esc_html__( 'Getting Started', 'business-point' ),
		'recommended_actions' => esc_html__( 'Recommended Actions', 'business-point' ),
		'support'             => esc_html__( 'Support', 'business-point' ),
		'upgrade_to_pro'      => esc_html__( 'Upgrade to Pro', 'business-point' ),
		'free_pro'            => esc_html__( 'FREE VS. PRO', 'business-point' ),
	),

	// Getting started.
	'getting_started' => array(
		array(
			'title'               => esc_html__( 'Theme Documentation', 'business-point' ),
			'text'                => esc_html__( 'Find step by step instructions with video documentation to setup theme easily.', 'business-point' ),
			'button_label'        => esc_html__( 'View documentation', 'business-point' ),
			'button_link'         => 'https://promenadethemes.com/documentation/business-point/',
			'is_button'           => false,
			'recommended_actions' => false,
			'is_new_tab'          => true,
		),
		array(
			'title'               => esc_html__( 'Recommended Actions', 'business-point' ),
			'text'                => esc_html__( 'We recommend few steps to take so that you can get complete site like shown in demo.', 'business-point' ),
			'button_label'        => esc_html__( 'Check recommended actions', 'business-point' ),
			'button_link'         => esc_url( admin_url( 'themes.php?page=business-point-about&tab=recommended_actions' ) ),
			'is_button'           => false,
			'recommended_actions' => false,
			'is_new_tab'          => false,
		),
		array(
			'title'               => esc_html__( 'Customize Everything', 'business-point' ),
			'text'                => esc_html__( 'Start customizing every aspect of the website with customizer.', 'business-point' ),
			'button_label'        => esc_html__( 'Go to Customizer', 'business-point' ),
			'button_link'         => esc_url( wp_customize_url() ),
			'is_button'           => true,
			'recommended_actions' => false,
			'is_new_tab'          => false,
		),
	),

	// Recommended actions.
	'recommended_actions' => array(
		'content' => array(
			
			'front-page' => array(
				'title'       => esc_html__( 'Setting Static Front Page','business-point' ),
				'description' => esc_html__( 'Create a new page to display on front page ( Ex: Home ) and assign "Home" template. Select A static page then Front page and Posts page to display front page specific sections. Note: Static page will be set automatically when you import demo content.', 'business-point' ),
				'id'          => 'front-page',
				'check'       => ( 'page' === get_option( 'show_on_front' ) ) ? true : false,
				'help'        => '<a href="' . esc_url( wp_customize_url() ) . '?autofocus[section]=static_front_page" class="button button-secondary">' . esc_html__( 'Static Front Page', 'business-point' ) . '</a>',
			),

			'one-click-demo-import' => array(
				'title'       => esc_html__( 'One Click Demo Import', 'business-point' ),
				'description' => esc_html__( 'Please install the One Click Demo Import plugin to import the demo content. After activation go to Appearance >> Import Demo Data and import it.', 'business-point' ),
				'check'       => class_exists( 'OCDI_Plugin' ),
				'plugin_slug' => 'one-click-demo-import',
				'id'          => 'one-click-demo-import',
			),
		),
	),

	// Support.
	'support_content' => array(
		'first' => array(
			'title'        => esc_html__( 'Contact Support', 'business-point' ),
			'icon'         => 'dashicons dashicons-sos',
			'text'         => esc_html__( 'If you have any problem, feel free to create ticket on our dedicated Support forum.', 'business-point' ),
			'button_label' => esc_html__( 'Contact Support', 'business-point' ),
			'button_link'  => esc_url( 'https://promenadethemes.com/support/item/business-point/' ),
			'is_button'    => true,
			'is_new_tab'   => true,
		),
		'second' => array(
			'title'        => esc_html__( 'Theme Documentation', 'business-point' ),
			'icon'         => 'dashicons dashicons-book-alt',
			'text'         => esc_html__( 'Kindly check our theme documentation for detailed information and video instructions.', 'business-point' ),
			'button_label' => esc_html__( 'View Documentation', 'business-point' ),
			'button_link'  => 'https://promenadethemes.com/documentation/business-point/',
			'is_button'    => false,
			'is_new_tab'   => true,
		),
		'third' => array(
			'title'        => esc_html__( 'Pro Version', 'business-point' ),
			'icon'         => 'dashicons dashicons-products',
			'icon'         => 'dashicons dashicons-star-filled',
			'text'         => esc_html__( 'Upgrade to pro version for additional features and options.', 'business-point' ),
			'button_label' => esc_html__( 'View Pro Version', 'business-point' ),
			'button_link'  => 'https://promenadethemes.com/downloads/business-point-plus/',
			'is_button'    => true,
			'is_new_tab'   => true,
		),
		'fourth' => array(
			'title'        => esc_html__( 'Youtube Video Tutorials', 'business-point' ),
			'icon'         => 'dashicons dashicons-video-alt3',
			'text'         => esc_html__( 'Please check our youtube channel for video instructions of Business Point setup and customization.', 'business-point' ),
			'button_label' => esc_html__( 'Video Tutorials', 'business-point' ),
			'button_link'  => 'https://www.youtube.com/watch?v=MM_O9iGfq2I&list=PL-Ic437QwxQ_0GyK0RzYSTwjplnQNkQre',
			'is_button'    => false,
			'is_new_tab'   => true,
		),
		'fifth' => array(
			'title'        => esc_html__( 'Customization Request', 'business-point' ),
			'icon'         => 'dashicons dashicons-admin-tools',
			'text'         => esc_html__( 'We have dedicated team members for theme customization. Feel free to contact us any time if you need any customization service.', 'business-point' ),
			'button_label' => esc_html__( 'Customization Request', 'business-point' ),
			'button_link'  => 'https://promenadethemes.com/contact-us/',
			'is_button'    => false,
			'is_new_tab'   => true,
		),
		'sixth' => array(
			'title'        => esc_html__( 'Child Theme', 'business-point' ),
			'icon'         => 'dashicons dashicons-admin-customizer',
			'text'         => esc_html__( 'If you want to customize theme file, you should use child theme rather than modifying theme file itself.', 'business-point' ),
			'button_label' => esc_html__( 'About child theme', 'business-point' ),
			'button_link'  => 'https://developer.wordpress.org/themes/advanced-topics/child-themes/',
			'is_button'    => false,
			'is_new_tab'   => true,
		),
	),

	// Upgrade.
	'upgrade_to_pro' 	=> array(
		'description'   => esc_html__( 'Upgrade to pro version for more exciting features and additional theme options.', 'business-point' ),
		'button_label' 	=> esc_html__( 'Upgrade to Pro', 'business-point' ),
		'button_link'  	=> 'https://promenadethemes.com/downloads/business-point-plus/',
		'is_new_tab'   	=> true,
	),

    // Free vs pro array.
    'free_pro' => array(
	    array(
		    'title'		=> esc_html__( 'Custom Widgets', 'business-point' ),
		    'desc' 		=> esc_html__( 'Widgets added by theme to enhance features', 'business-point' ),
		    'free' 		=> esc_html__('10','business-point'),
		    'pro'  		=> esc_html__('13','business-point'),
	    ),
	    
	    array(
		    'title'     => esc_html__( 'Google Fonts', 'business-point' ),
		    'desc' 		=> esc_html__( 'Google fonts options for changing the overall site fonts', 'business-point' ),
		    'free'  	=> 'no',
		    'pro'   	=> esc_html__('100+','business-point'),
	    ),
	    array(
		    'title'     => esc_html__( 'Color Options', 'business-point' ),
		    'desc'      => esc_html__( 'Options to change primary color of the site', 'business-point' ),
		    'free'      => esc_html__('no','business-point'),
		    'pro'       => esc_html__('yes','business-point'),
	    ),
	    array(
		    'title'     => esc_html__( 'WooCommerce Ready', 'business-point' ),
		    'desc'      => esc_html__( 'Theme supports/works with WooCommerce plugin', 'business-point' ),
		    'free'      => esc_html__('no','business-point'),
		    'pro'       => esc_html__('yes','business-point'),
	    ),
        array(
    	    'title'     => esc_html__( 'Latest Product Carousel', 'business-point' ),
    	    'desc'      => esc_html__( 'Widget to display latest products in carousel mode', 'business-point' ),
    	    'free'      => esc_html__('no','business-point'),
    	    'pro'       => esc_html__('yes','business-point'),
        ),

        array(
    	    'title'     => esc_html__( 'Featured Product Carousel', 'business-point' ),
    	    'desc'      => esc_html__( 'Widget to display featured products in carousel mode', 'business-point' ),
    	    'free'      => esc_html__('no','business-point'),
    	    'pro'       => esc_html__('yes','business-point'),
        ),

        array(
    	    'title'     => esc_html__( 'Fact Counter', 'business-point' ),
    	    'desc'      => esc_html__( 'Widget to display facts count that goes up when viewport is visible', 'business-point' ),
    	    'free'      => esc_html__('no','business-point'),
    	    'pro'       => esc_html__('yes','business-point'),
        ),
        array(
    	    'title'     => esc_html__( 'Hide Footer Credit', 'business-point' ),
    	    'desc'      => esc_html__( 'Option to enable/disable Powerby(Designer) credit in footer', 'business-point' ),
    	    'free'      => esc_html__('no','business-point'),
    	    'pro'       => esc_html__('yes','business-point'),
        ),
        array(
    	    'title'     => esc_html__( 'Override Footer Credit', 'business-point' ),
    	    'desc'      => esc_html__( 'Option to Override existing Powerby credit of footer', 'business-point' ),
    	    'free'      => esc_html__('no','business-point'),
    	    'pro'       => esc_html__('yes','business-point'),
        ),
	    array(
		    'title'     => esc_html__( 'SEO', 'business-point' ),
		    'desc' 		=> esc_html__( 'Developed with high skilled SEO tools.', 'business-point' ),
		    'free'  	=> 'yes',
		    'pro'   	=> 'yes',
	    ),
	    array(
		    'title'     => esc_html__( 'Support Forum', 'business-point' ),
		    'desc'      => esc_html__( 'Highly experienced and dedicated support team for your help plus online chat.', 'business-point' ),
		    'free'      => esc_html__('yes', 'business-point'),
		    'pro'       => esc_html__('High Priority', 'business-point'),
	    )

    ),

);
Business_Point_About::init( apply_filters( 'business_point_about_filter', $config ) );
