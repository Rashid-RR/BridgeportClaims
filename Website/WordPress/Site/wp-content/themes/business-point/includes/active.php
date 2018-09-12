<?php
/**
 * Functions for active_callback.
 *
 * @package Business_Point
 */

if ( ! function_exists( 'business_point_is_featured_slider_active' ) ) :

	/**
	 * Check if featured slider is active.
	 *
	 * @since 1.0.0
	 *
	 * @param WP_Customize_Control $control WP_Customize_Control instance.
	 *
	 * @return bool Whether the control is active to the current preview.
	 */
	function business_point_is_featured_slider_active( $control ) {

		if ( true == $control->manager->get_setting( 'slider_status' )->value() ) {
			return true;
		} else {
			return false;
		}

	}

endif;


if ( ! function_exists( 'business_point_is_top_header_active' ) ) :

	/**
	 * Check if featured slider is active.
	 *
	 * @since 1.0.0
	 *
	 * @param WP_Customize_Control $control WP_Customize_Control instance.
	 *
	 * @return bool Whether the control is active to the current preview.
	 */
	function business_point_is_top_header_active( $control ) {

		if ( true == $control->manager->get_setting( 'show_top_header' )->value() ) {
			return true;
		} else {
			return false;
		}

	}

endif;

if ( ! function_exists( 'business_point_is_slider_readmore_text_active' ) ) :

	/**
	 * Check if featured slider is active.
	 *
	 * @since 1.0.0
	 *
	 * @param WP_Customize_Control $control WP_Customize_Control instance.
	 *
	 * @return bool Whether the control is active to the current preview.
	 */
	function business_point_is_slider_readmore_text_active( $control ) {

		if ( true == $control->manager->get_setting( 'slider_readmore_status' )->value() ) {
			return true;
		} else {
			return false;
		}

	}

endif;