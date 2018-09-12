<?php
/**
 * Demo configuration
 *
 * @package Business_Point
 */

$config = array(
	'static_page'    => 'home',
	'posts_page'     => 'blog',
	'menu_locations' => array(
		'top'  	  => 'top-menu',
		'primary' => 'main-menu',
		'social'  => 'social-menu',
	),
	'ocdi'           => array(
		array(
			'import_file_name'             => esc_html__( 'Theme Demo Content', 'business-point' ),
			'local_import_file'            => trailingslashit( get_template_directory() ) . 'includes/demo/demo-content/content.xml',
			'local_import_widget_file'     => trailingslashit( get_template_directory() ) . 'includes/demo/demo-content/widget.wie',
			'local_import_customizer_file' => trailingslashit( get_template_directory() ) . 'includes/demo/demo-content/customizer.dat',
		),
	),
);

Business_Point_Demo::init( apply_filters( 'Business_Point_Demo_filter', $config ) );
