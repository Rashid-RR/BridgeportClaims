<?php
/**
 * Options.
 *
 * @package Business_Point
 */

class Business_Point_Info extends WP_Customize_Control {
    public $type = 'info';
    public $label = '';
    public function render_content() {
    ?>
        <h2><?php echo esc_html( $this->label ); ?></h2>
    <?php
    }
}

$default = business_point_get_default_theme_options();

//Logo Options Setting Starts
$wp_customize->add_setting('site_identity', array(
	'default' 			=> $default['site_identity'],
	'sanitize_callback' => 'business_point_sanitize_select'
	));

$wp_customize->add_control('site_identity', array(
	'type' 		=> 'radio',
	'label' 	=> esc_html__('Logo Options', 'business-point'),
	'section' 	=> 'title_tagline',
	'choices' 	=> array(
		'logo-only' 	=> esc_html__('Logo Only', 'business-point'),
		'title-text' 	=> esc_html__('Title + Tagline', 'business-point'),
		'logo-desc' 	=> esc_html__('Logo + Tagline', 'business-point')
		)
));

// Add Theme Options Panel.
$wp_customize->add_panel( 'theme_option_panel',
	array(
		'title'      => esc_html__( 'Theme Options', 'business-point' ),
		'priority'   => 100,
		'capability' => 'edit_theme_options',
	)
);

// Header Section.
$wp_customize->add_section( 'section_header',
	array(
		'title'      => esc_html__( 'Header Options', 'business-point' ),
		'priority'   => 100,
		'capability' => 'edit_theme_options',
		'panel'      => 'theme_option_panel',
	)
);

// Setting show_top_header.
$wp_customize->add_setting( 'show_top_header',
	array(
		'default'           => $default['show_top_header'],
		'sanitize_callback' => 'business_point_sanitize_checkbox',
	)
);
$wp_customize->add_control( 'show_top_header',
	array(
		'label'    			=> esc_html__( 'Show Top Header', 'business-point' ),
		'section'  			=> 'section_header',
		'type'     			=> 'checkbox',
		'priority' 			=> 100,
	)
);

// Setting Address.
$wp_customize->add_setting( 'top_address',
	array(
		'sanitize_callback' => 'sanitize_text_field',
	)
);
$wp_customize->add_control( 'top_address',
	array(
		'label'    			=> esc_html__( 'Address/Location', 'business-point' ),
		'section'  			=> 'section_header',
		'type'     			=> 'text',
		'priority' 			=> 100,
		'active_callback' 	=> 'business_point_is_top_header_active',
	)
);

// Setting Phone.
$wp_customize->add_setting( 'top_phone',
	array(
		'sanitize_callback' => 'sanitize_text_field',
	)
);
$wp_customize->add_control( 'top_phone',
	array(
		'label'    			=> esc_html__( 'Phone Number', 'business-point' ),
		'section'  			=> 'section_header',
		'type'     			=> 'text',
		'priority' 			=> 100,
		'active_callback' 	=> 'business_point_is_top_header_active',
	)
);

// Setting Email.
$wp_customize->add_setting( 'top_email',
	array(
		'sanitize_callback' => 'sanitize_text_field',
	)
);
$wp_customize->add_control( 'top_email',
	array(
		'label'    			=> esc_html__( 'Email', 'business-point' ),
		'section'  			=> 'section_header',
		'type'     			=> 'text',
		'priority' 			=> 100,
		'active_callback' 	=> 'business_point_is_top_header_active',
	)
);

// Setting top right header.
$wp_customize->add_setting( 'right_section',
	array(
		'default'           => $default['right_section'],
		'sanitize_callback' => 'business_point_sanitize_select',
	)
);
$wp_customize->add_control( 'right_section',
	array(
		'label'    			=> esc_html__( 'Top Header Right Section', 'business-point' ),
		'section'  			=> 'section_header',
		'type'     			=> 'radio',
		'priority' 			=> 100,
		'choices'  			=> array(
								'top-social' => esc_html__( 'Social Links', 'business-point' ),
								'top-menu'  => esc_html__( 'Menu', 'business-point' ),
							),
		'active_callback' 	=> 'business_point_is_top_header_active',
	)
);

// Layout Section.
$wp_customize->add_section( 'section_layout',
	array(
		'title'      => esc_html__( 'Layout Options', 'business-point' ),
		'priority'   => 100,
		'capability' => 'edit_theme_options',
		'panel'      => 'theme_option_panel',
	)
);

// Setting global_layout.
$wp_customize->add_setting( 'global_layout',
	array(
		'default'           => $default['global_layout'],
		'capability'        => 'edit_theme_options',
		'sanitize_callback' => 'business_point_sanitize_select',
	)
);
$wp_customize->add_control( 'global_layout',
	array(
		'label'    => esc_html__( 'Global Layout', 'business-point' ),
		'section'  => 'section_layout',
		'type'     => 'radio',
		'priority' => 100,
		'choices'  => array(
				'left-sidebar'  => esc_html__( 'Left Sidebar', 'business-point' ),
				'right-sidebar' => esc_html__( 'Right Sidebar', 'business-point' ),
				'no-sidebar'    => esc_html__( 'No Sidebar', 'business-point' ),
			),
	)
);

// Setting excerpt_length.
$wp_customize->add_setting( 'excerpt_length',
	array(
		'default'           => $default['excerpt_length'],
		'capability'        => 'edit_theme_options',
		'sanitize_callback' => 'business_point_sanitize_positive_integer',
	)
);
$wp_customize->add_control( 'excerpt_length',
	array(
		'label'       => esc_html__( 'Excerpt Length', 'business-point' ),
		'description' => esc_html__( 'in words', 'business-point' ),
		'section'     => 'section_layout',
		'type'        => 'number',
		'priority'    => 100,
		'input_attrs' => array( 'min' => 1, 'max' => 200, 'style' => 'width: 55px;' ),
	)
);

// Setting category_meta.
$wp_customize->add_setting( 
	'category_meta',
	array(
		'default'           => $default['category_meta'],
		'sanitize_callback' => 'business_point_sanitize_checkbox',
	)
);
$wp_customize->add_control( 
	'category_meta',
	array(
		'label'    		=> esc_html__( 'Hide Category', 'business-point' ),
		'section'  		=> 'section_layout',
		'type'     		=> 'checkbox',
		'priority'      => 100,
	)
);

// Setting author_meta.
$wp_customize->add_setting( 
	'author_meta',
	array(
		'default'           => $default['author_meta'],
		'sanitize_callback' => 'business_point_sanitize_checkbox',
	)
);
$wp_customize->add_control( 
	'author_meta',
	array(
		'label'    		=> esc_html__( 'Hide Author', 'business-point' ),
		'section'  		=> 'section_layout',
		'type'     		=> 'checkbox',
		'priority'      => 100,
	)
);

// Setting date_meta.
$wp_customize->add_setting( 
	'date_meta',
	array(
		'default'           => $default['date_meta'],
		'sanitize_callback' => 'business_point_sanitize_checkbox',
	)
);
$wp_customize->add_control( 
	'date_meta',
	array(
		'label'    		=> esc_html__( 'Hide Posted Date', 'business-point' ),
		'section'  		=> 'section_layout',
		'type'     		=> 'checkbox',
		'priority'      => 100,
	)
);

// Setting comment_meta.
$wp_customize->add_setting( 
	'comment_meta',
	array(
		'default'           => $default['comment_meta'],
		'sanitize_callback' => 'business_point_sanitize_checkbox',
	)
);
$wp_customize->add_control( 
	'comment_meta',
	array(
		'label'    		=> esc_html__( 'Hide Comment Count', 'business-point' ),
		'section'  		=> 'section_layout',
		'type'     		=> 'checkbox',
		'priority'      => 100,
	)
);

// Footer Section.
$wp_customize->add_section( 'section_footer',
	array(
		'title'      => esc_html__( 'Footer Options', 'business-point' ),
		'priority'   => 100,
		'capability' => 'edit_theme_options',
		'panel'      => 'theme_option_panel',
	)
);

// Setting copyright_text.
$wp_customize->add_setting( 'copyright_text',
	array(
		'default'           => $default['copyright_text'],
		'capability'        => 'edit_theme_options',
		'sanitize_callback' => 'sanitize_text_field',
	)
);
$wp_customize->add_control( 'copyright_text',
	array(
		'label'    => esc_html__( 'Copyright Text', 'business-point' ),
		'section'  => 'section_footer',
		'type'     => 'text',
		'priority' => 100,
	)
);

// Breadcrumb Section.
$wp_customize->add_section( 'section_breadcrumb',
	array(
		'title'      => esc_html__( 'Breadcrumb Options', 'business-point' ),
		'priority'   => 100,
		'capability' => 'edit_theme_options',
		'panel'      => 'theme_option_panel',
	)
);

// Setting breadcrumb_type.
$wp_customize->add_setting( 'breadcrumb_type',
	array(
		'default'           => $default['breadcrumb_type'],
		'capability'        => 'edit_theme_options',
		'sanitize_callback' => 'business_point_sanitize_select',
	)
);
$wp_customize->add_control( 'breadcrumb_type',
	array(
		'label'       => esc_html__( 'Breadcrumb Type', 'business-point' ),
		'section'     => 'section_breadcrumb',
		'type'        => 'radio',
		'priority'    => 100,
		'choices'     => array(
			'disable' => esc_html__( 'Disable', 'business-point' ),
			'simple'  => esc_html__( 'Simple', 'business-point' ),
		),
	)
);

// Add Slider Options Panel.
$wp_customize->add_panel( 'slider_option_panel',
	array(
		'title'      => esc_html__( 'Featured Slider Options', 'business-point' ),
		'priority'   => 100,
	)
);

// Slider Section.
$wp_customize->add_section( 'section_slider',
	array(
		'title'      => esc_html__( 'Slider On/Off', 'business-point' ),
		'panel'      => 'slider_option_panel',
	)
);

// Setting slider_status.
$wp_customize->add_setting( 'slider_status',
	array(
		'default'           => $default['slider_status'],
		'sanitize_callback' => 'business_point_sanitize_checkbox',
	)
);
$wp_customize->add_control( 'slider_status',
	array(
		'label'    			=> esc_html__( 'Enable Slider', 'business-point' ),
		'section'  			=> 'section_slider',
		'type'     			=> 'checkbox',
		'priority' 			=> 100,
	)
);

$slider_number = 5;

for ( $i = 1; $i <= $slider_number; $i++ ) {

	$wp_customize->add_setting( "slider_page_$i",
		array(
			'sanitize_callback' => 'business_point_sanitize_dropdown_pages',
		)
	);
	$wp_customize->add_control( "slider_page_$i",
		array(
			'label'           => esc_html__( 'Slide ', 'business-point' ) . ' - ' . $i,
			'section'         => 'section_slider',
			'type'            => 'dropdown-pages',
			'active_callback' => 'business_point_is_featured_slider_active',
			'priority' 		  => 100,
		)
	); 

}

// Slider Options Section.
$wp_customize->add_section( 'section_slider_options',
	array(
		'title'      => esc_html__( 'Slider Effects Setting', 'business-point' ),
		'panel'      => 'slider_option_panel',
	)
);

// Setting slider_transition_effect.
$wp_customize->add_setting( 'slider_transition_effect',
	array(
		'default'           => $default['slider_transition_effect'],
		'sanitize_callback' => 'business_point_sanitize_select',
	)
);
$wp_customize->add_control( 'slider_transition_effect',
	array(
		'label'           => esc_html__( 'Transition Effect', 'business-point' ),
		'section'         => 'section_slider_options',
		'type'            => 'select',
		'choices'         => array(
			'fade'       => esc_html__( 'fade', 'business-point' ),
			'fadeout'    => esc_html__( 'fadeout', 'business-point' ),
			'none'       => esc_html__( 'none', 'business-point' ),
			'scrollHorz' => esc_html__( 'scrollHorz', 'business-point' ),
		),
	)
);

// Setting slider_transition_delay.
$wp_customize->add_setting( 'slider_transition_delay',
	array(
		'default'           => $default['slider_transition_delay'],
		'sanitize_callback' => 'business_point_sanitize_positive_integer',
	)
);
$wp_customize->add_control( 'slider_transition_delay',
	array(
		'label'           => esc_html__( 'Transition Delay', 'business-point' ),
		'description'     => esc_html__( 'in seconds', 'business-point' ),
		'section'         => 'section_slider_options',
		'type'            => 'number',
		'input_attrs'     => array( 'min' => 1, 'max' => 5, 'step' => 1, 'style' => 'width: 60px;' ),
	)
);

// Setting slider_transition_duration.
$wp_customize->add_setting( 'slider_transition_duration',
	array(
		'default'           => $default['slider_transition_duration'],
		'sanitize_callback' => 'business_point_sanitize_positive_integer',
	)
);
$wp_customize->add_control( 'slider_transition_duration',
	array(
		'label'           => esc_html__( 'Transition Duration', 'business-point' ),
		'description'     => esc_html__( 'in seconds', 'business-point' ),
		'section'         => 'section_slider_options',
		'type'            => 'number',
		'input_attrs'     => array( 'min' => 1, 'max' => 10, 'step' => 1, 'style' => 'width: 60px;' ),
	)
);

// Slider Element Section.
$wp_customize->add_section( 'section_slider_elements',
	array(
		'title'      => esc_html__( 'Slider Elements On/Off', 'business-point' ),
		'panel'      => 'slider_option_panel',
	)
);

// Setting slider_caption_status.
$wp_customize->add_setting( 'slider_caption_status',
	array(
		'default'           => $default['slider_caption_status'],
		'sanitize_callback' => 'business_point_sanitize_checkbox',
	)
);
$wp_customize->add_control( 'slider_caption_status',
	array(
		'label'           => esc_html__( 'Show Caption/Description', 'business-point' ),
		'section'         => 'section_slider_elements',
		'type'            => 'checkbox',
	)
);

// Setting slider_arrow_status.
$wp_customize->add_setting( 'slider_arrow_status',
	array(
		'default'           => $default['slider_arrow_status'],
		'sanitize_callback' => 'business_point_sanitize_checkbox',
	)
);
$wp_customize->add_control( 'slider_arrow_status',
	array(
		'label'           => esc_html__( 'Show Arrow', 'business-point' ),
		'section'         => 'section_slider_elements',
		'type'            => 'checkbox',
	)
);

// Setting slider_pager_status.
$wp_customize->add_setting( 'slider_pager_status',
	array(
		'default'           => $default['slider_pager_status'],
		'sanitize_callback' => 'business_point_sanitize_checkbox',
	)
);
$wp_customize->add_control( 'slider_pager_status',
	array(
		'label'           => esc_html__( 'Show Pager', 'business-point' ),
		'section'         => 'section_slider_elements',
		'type'            => 'checkbox',
	)
);

// Setting slider_autoplay_status.
$wp_customize->add_setting( 'slider_autoplay_status',
	array(
		'default'           => $default['slider_autoplay_status'],
		'sanitize_callback' => 'business_point_sanitize_checkbox',
	)
);
$wp_customize->add_control( 'slider_autoplay_status',
	array(
		'label'           => esc_html__( 'Enable Autoplay', 'business-point' ),
		'section'         => 'section_slider_elements',
		'type'            => 'checkbox',
	)
);

// Setting slider_overlay_status.
$wp_customize->add_setting( 'slider_overlay_status',
	array(
		'default'           => $default['slider_overlay_status'],
		'sanitize_callback' => 'business_point_sanitize_checkbox',
	)
);
$wp_customize->add_control( 'slider_overlay_status',
	array(
		'label'           => esc_html__( 'Enable Overlay', 'business-point' ),
		'section'         => 'section_slider_elements',
		'type'            => 'checkbox',
	)
);

// Setting slider excerpt_length.
$wp_customize->add_setting( 'slider_excerpt_length',
	array(
		'default'           => $default['slider_excerpt_length'],
		'sanitize_callback' => 'business_point_sanitize_positive_integer',
	)
);
$wp_customize->add_control( 'slider_excerpt_length',
	array(
		'label'       => esc_html__( 'Slide Caption/Description Length', 'business-point' ),
		'section'     => 'section_slider_elements',
		'type'        => 'number',
		'input_attrs' => array( 'min' => 1, 'max' => 50, 'style' => 'width: 55px;' ),
		'priority' 	  => 100,
	)
);

// Setting slider_readmore_status.
$wp_customize->add_setting( 'slider_readmore_status',
	array(
		'default'           => $default['slider_readmore_status'],
		'sanitize_callback' => 'business_point_sanitize_checkbox',
	)
);
$wp_customize->add_control( 'slider_readmore_status',
	array(
		'label'           => esc_html__( 'Enable Readmore Button', 'business-point' ),
		'section'         => 'section_slider_elements',
		'type'            => 'checkbox',
		'priority' 		  => 100,
	)
);

// Setting slider readmore text.
$wp_customize->add_setting( 'slider_readmore_text',
	array(
		'default'           => $default['slider_readmore_text'],
		'sanitize_callback' => 'sanitize_text_field',
	)
);
$wp_customize->add_control( 'slider_readmore_text',
	array(
		'label'    => esc_html__( 'Read More Text', 'business-point' ),
		'section'  => 'section_slider_elements',
		'type'     => 'text',
		'priority' => 100,
		'active_callback' 	=> 'business_point_is_slider_readmore_text_active',
	)
);