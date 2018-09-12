<?php
/**
 * Load files.
 *
 * @package Business_Point
 */

// Load core functions.
require_once trailingslashit( get_template_directory() ) . '/includes/core.php';

// Load helper functions.
require_once trailingslashit( get_template_directory() ) . '/includes/helpers.php';

// Custom template tags for this theme.
require_once trailingslashit( get_template_directory() ) . '/includes/template-tags.php';

// Custom functions that act independently of the theme templates.
require_once trailingslashit( get_template_directory() ) . '/includes/extras.php';

// Customizer additions.
require_once trailingslashit( get_template_directory() ) . '/includes/customizer.php';

// Load widgets.
require_once trailingslashit( get_template_directory() ) . '/includes/widgets/widgets.php';

// Load hooks.
require_once trailingslashit( get_template_directory() ) . '/includes/hooks.php';

//TGM Plugin activation.
require_once trailingslashit( get_template_directory() ) . '/includes/tgm/class-tgm-plugin-activation.php';

if ( is_admin() ) {
	// Load about.
	require_once trailingslashit( get_template_directory() ) . 'includes/theme-info/class-about.php';
	require_once trailingslashit( get_template_directory() ) . 'includes/theme-info/about.php';

	// Load demo.
	require_once trailingslashit( get_template_directory() ) . 'includes/demo/class-demo.php';
	require_once trailingslashit( get_template_directory() ) . 'includes/demo/demo.php';
}