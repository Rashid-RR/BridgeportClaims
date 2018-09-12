<?php
/**
 * Helper functions.
 *
 * @package Business_Point
 */

if ( ! function_exists( 'business_point_get_slider_details' ) ) :

	/**
	 * Slider details.
	 *
	 * @since 1.0.0
	 *
	 * @return array Slider details.
	 */
	function business_point_get_slider_details() {

		$output = array();

		$slider_number = 5;

		$slider_excerpt_length  = business_point_get_option('slider_excerpt_length');

		$page_ids = array();

		for ( $i = 1; $i <= $slider_number ; $i++ ) {
			$page_id = business_point_get_option( "slider_page_$i" );
			if ( absint( $page_id ) > 0 ) {
				$page_ids[] = absint( $page_id );
			}
		}

		if ( empty( $page_ids ) ) {
			return $output;
		}

		$query_args = array(
			'posts_per_page' => count( $page_ids ),
			'orderby'        => 'post__in',
			'post_type'      => 'page',
			'post__in'       => $page_ids,
			'meta_query'     => array(
				array( 'key' => '_thumbnail_id' ),
			),
		);

		$posts = get_posts( $query_args );

		if ( ! empty( $posts ) ) {
			$p_count = 1;
			foreach ( $posts as $post ) {
				if ( has_post_thumbnail( $post->ID ) ) {
					$image_array = wp_get_attachment_image_src( get_post_thumbnail_id( $post->ID ), 'full' );
					$output[ $p_count ]['image_url'] = $image_array[0];
					$output[ $p_count ]['title']     = $post->post_title;
					$output[ $p_count ]['url']       = get_permalink( $post->ID );
					$output[ $p_count ]['excerpt']   = business_point_get_the_excerpt( absint($slider_excerpt_length), $post );
					
					$p_count++;
				}
			}
		}

		return $output;
	}

endif;

if ( ! function_exists( 'business_point_get_the_excerpt' ) ) :

	/**
	 * Returns post excerpt.
	 *
	 * @since 1.0.0
	 *
	 * @param int     $length      Excerpt length in words.
	 * @param WP_Post $post_object The post object.
	 * @return string Post excerpt.
	 */
	function business_point_get_the_excerpt( $length = 0, $post_object = null ) {
		global $post;

		if ( is_null( $post_object ) ) {
			$post_object = $post;
		}

		$length = absint( $length );
		if ( 0 === $length ) {
			return;
		}



		$source_content = $post_object->post_content;

		if ( ! empty( $post_object->post_excerpt ) ) {
			$source_content = $post_object->post_excerpt;
		}

		$source_content = strip_shortcodes( $source_content );
		$trimmed_content = wp_trim_words( $source_content, $length, '...' );
		return $trimmed_content;
	}

endif;

if ( ! function_exists( 'business_point_fonts_url' ) ) :

	/**
	 * Register Google fonts.
	 *
	 * @since 1.0.0
	 *
	 * @return string Google fonts URL for the theme.
	 */
	function business_point_fonts_url() {
		$fonts_url = '';
		$fonts     = array();
		$subsets   = 'latin,latin-ext';

		/* translators: If there are characters in your language that are not supported by Open Sans, translate this to 'off'. Do not translate into your own language. */
		if ( 'off' !== _x( 'on', 'Open Sans font: on or off', 'business-point' ) ) {
			$fonts[] = 'Open Sans:400,700,900,400italic,700italic,900italic';
		}
		/* translators: If there are characters in your language that are not supported by Open Sans, translate this to 'off'. Do not translate into your own language. */
		if ( 'off' !== _x( 'on', 'Raleway font: on or off', 'business-point' ) ) {
			$fonts[] = 'Raleway:400,500,700,900,400italic,700italic,900italic';
		}
		if ( $fonts ) {
			$fonts_url = add_query_arg( array(
				'family' => urlencode( implode( '|', $fonts ) ),
				'subset' => urlencode( $subsets ),
			), 'https://fonts.googleapis.com/css' );
		}

		return $fonts_url;
	}

endif;