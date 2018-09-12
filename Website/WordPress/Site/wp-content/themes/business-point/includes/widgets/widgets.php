<?php
/**
 * Custom widgets.
 *
 * @package Business_Point
 */

if ( ! function_exists( 'business_point_load_widgets' ) ) :

	/**
	 * Load widgets.
	 *
	 * @since 1.0.0
	 */
	function business_point_load_widgets() {

		// Social.
		register_widget( 'Business_Point_Social_Widget' );

		// Latest news.
		register_widget( 'Business_Point_Latest_News_Widget' );

		// CTA widget.
		register_widget( 'Business_Point_CTA_Widget' );

		// Services widget.
		register_widget( 'Business_Point_Services_Widget' );

		// Newsletter widget.
		register_widget( 'Business_Point_Newsletter_Widget' );

		// Advanced recent posts widget.
		register_widget( 'Business_Point_Recent_Posts_Widget' );

	}

endif;

add_action( 'widgets_init', 'business_point_load_widgets' );

if ( ! class_exists( 'Business_Point_Social_Widget' ) ) :

	/**
	 * Social widget class.
	 *
	 * @since 1.0.0
	 */
	class Business_Point_Social_Widget extends WP_Widget {

		/**
		 * Constructor.
		 *
		 * @since 1.0.0
		 */
		function __construct() {
			$opts = array(
				'classname'   => 'business_point_widget_social',
				'description' => esc_html__( 'Social Icons Widget', 'business-point' ),
			);
			parent::__construct( 'business-point-social', esc_html__( 'BP: Social', 'business-point' ), $opts );
		}

		/**
		 * Echo the widget content.
		 *
		 * @since 1.0.0
		 *
		 * @param array $args     Display arguments including before_title, after_title,
		 *                        before_widget, and after_widget.
		 * @param array $instance The settings for the particular instance of the widget.
		 */
		function widget( $args, $instance ) {

			$title = apply_filters( 'widget_title', empty( $instance['title'] ) ? '' : $instance['title'], $instance, $this->id_base );

			echo $args['before_widget'];

			if ( ! empty( $title ) ) {
				echo $args['before_title'] . esc_html( $title ). $args['after_title'];
			}

			if ( has_nav_menu( 'social' ) ) {
				wp_nav_menu( array(
					'theme_location' => 'social',
					'link_before'    => '<span class="screen-reader-text">',
					'link_after'     => '</span>',
				) );
			}

			echo $args['after_widget'];

		}

		/**
		 * Update widget instance.
		 *
		 * @since 1.0.0
		 *
		 * @param array $new_instance New settings for this instance as input by the user via
		 *                            {@see WP_Widget::form()}.
		 * @param array $old_instance Old settings for this instance.
		 * @return array Settings to save or bool false to cancel saving.
		 */
		function update( $new_instance, $old_instance ) {
			$instance = $old_instance;

			$instance['title'] = sanitize_text_field( $new_instance['title'] );

			return $instance;
		}

		/**
		 * Output the settings update form.
		 *
		 * @since 1.0.0
		 *
		 * @param array $instance Current settings.
		 * @return void
		 */
		function form( $instance ) {

			$instance = wp_parse_args( (array) $instance, array(
				'title' => '',
			) );
			?>
			<p>
				<label for="<?php echo esc_attr( $this->get_field_id( 'title' ) ); ?>"><?php esc_html_e( 'Title:', 'business-point' ); ?></label>
				<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'title' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'title' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['title'] ); ?>" />
			</p>

			<?php if ( ! has_nav_menu( 'social' ) ) : ?>
	        <p>
				<?php esc_html_e( 'Social menu is not set. Please create menu and assign it to Social Theme Location.', 'business-point' ); ?>
	        </p>
	        <?php endif; ?>
			<?php
		}
	}

endif;


if ( ! class_exists( 'Business_Point_Latest_News_Widget' ) ) :

	/**
	 * Latest News widget class.
	 *
	 * @since 1.0.0
	 */
	class Business_Point_Latest_News_Widget extends WP_Widget {

	    function __construct() {
	    	$opts = array(
				'classname'   => 'business_point_widget_latest_news',
				'description' => esc_html__( 'Latest News Widget', 'business-point' ),
    		);

			parent::__construct( 'business-point-latest-news', esc_html__( 'BP: Latest News', 'business-point' ), $opts );
	    }


	    function widget( $args, $instance ) {

			$title             	= apply_filters( 'widget_title', empty( $instance['title'] ) ? '' : $instance['title'], $instance, $this->id_base );

			$section_icon 	   	= !empty( $instance['section_icon'] ) ? $instance['section_icon'] : '';

			$sub_title 	 		= !empty( $instance['sub_title'] ) ? $instance['sub_title'] : '';

			$post_category     	= ! empty( $instance['post_category'] ) ? $instance['post_category'] : 0;

			$exclude_categories = !empty( $instance[ 'exclude_categories' ] ) ? esc_attr( $instance[ 'exclude_categories' ] ) : '';

			$disable_date   	= ! empty( $instance['disable_date'] ) ? $instance['disable_date'] : 0;

	        echo $args['before_widget']; ?>

	        <div class="latest-news-widget bp-latest-news">

        		<div class="section-title">

			        <?php 

			        if ( $title ) {
			        	echo $args['before_title'] . esc_html( $title ) . $args['after_title'];
			        }

			        if ( $section_icon ) { ?>

			        	<div class="seperator">
			        	    <span><i class="fa <?php echo esc_html( $section_icon ); ?>"></i></span>
			        	</div>
			        	<?php
			        	
			        }

			        if ( $sub_title ) { ?>

			        	<p><?php echo esc_html( $sub_title ); ?></p>

			        	<?php 
			        	
			        } ?>

		        </div>

		        <?php

		        $query_args = array(
					        	'posts_per_page' 		=> 5,
					        	'no_found_rows'  		=> true,
					        	'post__not_in'          => get_option( 'sticky_posts' ),
					        	'ignore_sticky_posts'   => true,
				        	);

		        if ( absint( $post_category ) > 0 ) {

		        	$query_args['cat'] = absint( $post_category );
		        	
		        }

		        if ( !empty( $exclude_categories ) ) {

		        	$exclude_ids = explode(',', $exclude_categories);

		        	$query_args['category__not_in'] = $exclude_ids;

		        }

		        $all_posts = new WP_Query( $query_args );

		        if ( $all_posts->have_posts() ) :?>

			        <div class="inner-wrapper">

						<?php 

						$post_count = $all_posts->post_count;

						$p_count = 1;

						while ( $all_posts->have_posts() ) :

                            $all_posts->the_post(); ?>

                        	<?php 
                        	if( 1 == $p_count ){ ?>
                        		<div class="large-item">
                        	<?php } ?>

                        	<?php 
                        	if( 2 == $p_count ){ ?>
                        		<div class="small-items-wrap">
                        	<?php } ?>

				                <div class="latest-news-item">
					                <div class="latest-news-wrapper">

						                <?php if ( has_post_thumbnail() ) :  ?>
						                  <div class="latest-news-thumb">
						                    <a href="<?php the_permalink(); ?>">
												<?php

												if( 1 == $p_count ){

													$thumb_size = 'business-point-long';

												}else{

													$thumb_size = 'business-point-small';
												}
						                        
						                        the_post_thumbnail( $thumb_size );
												?>
						                    </a>
						                  </div><!-- .latest-news-thumb -->
						                <?php endif; ?>
						                <div class="latest-news-text-wrap">
											<h3 class="latest-news-title">
												<a href="<?php the_permalink(); ?>"><?php the_title(); ?></a>
											</h3><!-- .latest-news-title -->
											<?php if( 1 != $disable_date ){ ?>
												<span class="latest-news-date"><?php echo esc_html( get_the_date() ); ?></span>
											<?php } ?>

						                </div><!-- .latest-news-text-wrap -->

					                </div>
				                </div>

				                <?php 
                        		if( ( $post_count == $p_count ) && ( 1 < $p_count ) ){ ?>
                        		</div>
                        		<?php } ?>

				                 <?php if( 1 == $p_count ){ ?></div><?php } ?>

			                <?php 

							$p_count++;

						endwhile; 

                        wp_reset_postdata(); ?>

                    </div>

		        <?php endif; ?>

	        </div><!-- .latest-news-widget -->

	        <?php
	        echo $args['after_widget'];

	    }

	    function update( $new_instance, $old_instance ) {
	        $instance = $old_instance;
			$instance['title']          	= sanitize_text_field( $new_instance['title'] );
			$instance['section_icon'] 		= sanitize_text_field( $new_instance['section_icon'] );
			$instance['sub_title'] 		    = sanitize_text_field( $new_instance['sub_title'] );
			$instance['post_category']  	= absint( $new_instance['post_category'] );
			$instance['exclude_categories'] = sanitize_text_field( $new_instance['exclude_categories'] );
			$instance['disable_date']    	= (bool) $new_instance['disable_date'] ? 1 : 0;

	        return $instance;
	    }

	    function form( $instance ) {

	        $instance = wp_parse_args( (array) $instance, array(
				'title'          		=> '',
				'section_icon' 			=> 'fa-folder-open-o',
				'sub_title' 			=> '',
				'post_category'  		=> '',
				'exclude_categories' 	=> '',
				'disable_date'   		=> 0,
	        ) );
	        ?>
	        <p>
	          <label for="<?php echo esc_attr( $this->get_field_id( 'title' ) ); ?>"><strong><?php esc_html_e( 'Title:', 'business-point' ); ?></strong></label>
	          <input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'title' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'title' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['title'] ); ?>" />
	        </p>
	        <p>
	        	<label for="<?php echo esc_attr( $this->get_field_id( 'section_icon' ) ); ?>"><strong><?php esc_html_e( 'Icon:', 'business-point' ); ?></strong></label>
	        	<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'section_icon' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'section_icon' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['section_icon'] ); ?>" />
	        </p>
	        <p>
	        	<label for="<?php echo esc_attr( $this->get_field_id( 'sub_title' ) ); ?>"><strong><?php esc_html_e( 'Sub Title:', 'business-point' ); ?></strong></label>
	        	<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'sub_title' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'sub_title' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['sub_title'] ); ?>" />
	        </p>
	        <p>
	          <label for="<?php echo  esc_attr( $this->get_field_id( 'post_category' ) ); ?>"><strong><?php esc_html_e( 'Select Category:', 'business-point' ); ?></strong></label>
				<?php
	            $cat_args = array(
	                'orderby'         => 'name',
	                'hide_empty'      => 0,
	                'class' 		  => 'widefat',
	                'taxonomy'        => 'category',
	                'name'            => $this->get_field_name( 'post_category' ),
	                'id'              => $this->get_field_id( 'post_category' ),
	                'selected'        => absint( $instance['post_category'] ),
	                'show_option_all' => esc_html__( 'All Categories','business-point' ),
	              );
	            wp_dropdown_categories( $cat_args );
				?>
	        </p>
            <p>
            	<label for="<?php echo esc_attr( $this->get_field_id( 'exclude_categories' ) ); ?>"><strong><?php esc_html_e( 'Exclude Categories:', 'business-point' ); ?></strong></label>
            	<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'exclude_categories' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'exclude_categories' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['exclude_categories'] ); ?>" />
    	        <small>
    	        	<?php esc_html_e('Enter category id seperated with comma. Posts from these categories will be excluded from latest post listing.', 'business-point'); ?>	
    	        </small>
            </p>
	        <p>
	            <input class="checkbox" type="checkbox" <?php checked( $instance['disable_date'] ); ?> id="<?php echo $this->get_field_id( 'disable_date' ); ?>" name="<?php echo $this->get_field_name( 'disable_date' ); ?>" />
	            <label for="<?php echo $this->get_field_id( 'disable_date' ); ?>"><?php esc_html_e( 'Hide Posted Date', 'business-point' ); ?></label>
	        </p>
	        <?php
	    }

	}

endif;

if ( ! class_exists( 'Business_Point_CTA_Widget' ) ) :

	/**
	 * CTA widget class.
	 *
	 * @since 1.0.0
	 */
	class Business_Point_CTA_Widget extends WP_Widget {

		/**
		 * Constructor.
		 *
		 * @since 1.0.0
		 */
		function __construct() {
			$opts = array(
				'classname'   => 'business_point_widget_call_to_action',
				'description' => esc_html__( 'Call To Action Widget', 'business-point' ),
			);
			parent::__construct( 'business-point-cta', esc_html__( 'BP: CTA', 'business-point' ), $opts );
		}

		/**
		 * Echo the widget content.
		 *
		 * @since 1.0.0
		 *
		 * @param array $args     Display arguments including before_title, after_title,
		 *                        before_widget, and after_widget.
		 * @param array $instance The settings for the particular instance of the widget.
		 */
		function widget( $args, $instance ) {
			$title       = apply_filters( 'widget_title', empty( $instance['title'] ) ? '' : $instance['title'], $instance, $this->id_base );
			$cta_page    = !empty( $instance['cta_page'] ) ? $instance['cta_page'] : ''; 
			$button_text = ! empty( $instance['button_text'] ) ? esc_html( $instance['button_text'] ) : '';
			$button_url  = ! empty( $instance['button_url'] ) ? esc_url( $instance['button_url'] ) : '';
			$secondary_button_text = ! empty( $instance['secondary_button_text'] ) ? esc_html( $instance['secondary_button_text'] ) : '';
			$secondary_button_url  = ! empty( $instance['secondary_button_url'] ) ? esc_url( $instance['secondary_button_url'] ) : '';
			$bg_pic  	 = ! empty( $instance['bg_pic'] ) ? esc_url( $instance['bg_pic'] ) : '';

			// Add background image.
			if ( ! empty( $bg_pic ) ) {
				$background_style = '';
				$background_style .= ' style="background-image:url(' . esc_url( $bg_pic ) . ');" ';
				$args['before_widget'] = implode( $background_style . ' ' . 'class="bg_enabled ', explode( 'class="', $args['before_widget'], 2 ) );
			}

			echo $args['before_widget']; ?>

			<div class="cta-widget">

				<?php

				if ( ! empty( $title ) ) {
					echo $args['before_title'] . esc_html( $title ) . $args['after_title'];
				}  

				if ( $cta_page ) { 

					$cta_args = array(
									'posts_per_page' => 1,
									'page_id'	     => absint( $cta_page ),
									'post_type'      => 'page',
									'post_status'  	 => 'publish',
								);

					$cta_query = new WP_Query( $cta_args );	

					if( $cta_query->have_posts()){

						while( $cta_query->have_posts()){

							$cta_query->the_post(); ?>

							<div class="call-to-action-content">
								<?php the_content(); ?>
							</div>

							<?php

						}

						wp_reset_postdata();

					} ?>
					
				<?php } ?>

				<div class="call-to-action-buttons">

					<?php if ( ! empty( $button_text ) ) : ?>
						<a href="<?php echo esc_url( $button_url ); ?>" class="button cta-button cta-button-primary"><?php echo esc_attr( $button_text ); ?></a>
					<?php endif; ?>

					<?php if ( ! empty( $secondary_button_text ) ) : ?>
						<a href="<?php echo esc_url( $secondary_button_url ); ?>" class="button cta-button cta-button-secondary"><?php echo esc_attr( $secondary_button_text ); ?></a>
					<?php endif; ?>

				</div><!-- .call-to-action-buttons -->

			</div><!-- .cta-widget -->

			<?php
			echo $args['after_widget'];

		}

		/**
		 * Update widget instance.
		 *
		 * @since 1.0.0
		 *
		 * @param array $new_instance New settings for this instance as input by the user via
		 *                            {@see WP_Widget::form()}.
		 * @param array $old_instance Old settings for this instance.
		 * @return array Settings to save or bool false to cancel saving.
		 */
		function update( $new_instance, $old_instance ) {

			$instance = $old_instance;

			$instance['title'] 			= sanitize_text_field( $new_instance['title'] );

			$instance['cta_page'] 	 	= absint( $new_instance['cta_page'] );

			$instance['button_text'] 	= sanitize_text_field( $new_instance['button_text'] );
			$instance['button_url']  	= esc_url_raw( $new_instance['button_url'] );

			$instance['secondary_button_text'] 	= sanitize_text_field( $new_instance['secondary_button_text'] );
			$instance['secondary_button_url']  	= esc_url_raw( $new_instance['secondary_button_url'] );

			$instance['bg_pic']  	 	= esc_url_raw( $new_instance['bg_pic'] );

			return $instance;
		}

		/**
		 * Output the settings update form.
		 *
		 * @since 1.0.0
		 *
		 * @param array $instance Current settings.
		 */
		function form( $instance ) {

			$instance = wp_parse_args( (array) $instance, array(
				'title'       			=> '',
				'cta_page'    			=> '',
				'button_text' 			=> esc_html__( 'Find More', 'business-point' ),
				'button_url'  			=> '',
				'secondary_button_text' => esc_html__( 'Buy Now', 'business-point' ),
				'secondary_button_url'  => '',
				'bg_pic'      			=> '',
			) );

			$bg_pic = '';

            if ( ! empty( $instance['bg_pic'] ) ) {

                $bg_pic = $instance['bg_pic'];

            }

            $wrap_style = '';

            if ( empty( $bg_pic ) ) {

                $wrap_style = ' style="display:none;" ';
            }

            $image_status = false;

            if ( ! empty( $bg_pic ) ) {
                $image_status = true;
            }

            $delete_button = 'display:none;';

            if ( true === $image_status ) {
                $delete_button = 'display:inline-block;';
            }
			?>
			<p>
				<label for="<?php echo esc_attr( $this->get_field_id( 'title' ) ); ?>"><strong><?php esc_html_e( 'Title:', 'business-point' ); ?></strong></label>
				<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'title' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'title' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['title'] ); ?>" />
			</p>
			<p>
				<label for="<?php echo $this->get_field_id( 'cta_page' ); ?>">
					<strong><?php esc_html_e( 'CTA Page:', 'business-point' ); ?></strong>
				</label>
				<?php
				wp_dropdown_pages( array(
					'id'               => $this->get_field_id( 'cta_page' ),
					'class'            => 'widefat',
					'name'             => $this->get_field_name( 'cta_page' ),
					'selected'         => $instance[ 'cta_page' ],
					'show_option_none' => esc_html__( '&mdash; Select &mdash;', 'business-point' ),
					)
				);
				?>
				<small>
		        	<?php esc_html_e('Content of this page will be used as content of CTA', 'business-point'); ?>	
		        </small>
			</p>
			<p>
				<label for="<?php echo esc_attr( $this->get_field_id( 'button_text' ) ); ?>"><strong><?php esc_html_e( 'Primary Button Text:', 'business-point' ); ?></strong></label>
				<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'button_text' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'button_text' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['button_text'] ); ?>" />
			</p>
			<p>
				<label for="<?php echo esc_attr( $this->get_field_id( 'button_url' ) ); ?>"><strong><?php esc_html_e( 'Primary Button URL:', 'business-point' ); ?></strong></label>
				<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'button_url' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'button_url' ) ); ?>" type="text" value="<?php echo esc_url( $instance['button_url'] ); ?>" />
			</p>

			<p>
				<label for="<?php echo esc_attr( $this->get_field_id( 'secondary_button_text' ) ); ?>"><strong><?php esc_html_e( 'Secondary Button Text:', 'business-point' ); ?></strong></label>
				<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'secondary_button_text' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'secondary_button_text' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['secondary_button_text'] ); ?>" />
			</p>
			<p>
				<label for="<?php echo esc_attr( $this->get_field_id( 'secondary_button_url' ) ); ?>"><strong><?php esc_html_e( 'Secondary Button URL:', 'business-point' ); ?></strong></label>
				<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'secondary_button_url' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'secondary_button_url' ) ); ?>" type="text" value="<?php echo esc_url( $instance['secondary_button_url'] ); ?>" />
			</p>

			<div class="cover-image">
                <label for="<?php echo esc_attr( $this->get_field_id( 'bg_pic' ) ); ?>">
                    <strong><?php esc_html_e( 'Background Image:', 'business-point' ); ?></strong>
                </label>
                <input type="text" class="img widefat" name="<?php echo esc_attr( $this->get_field_name( 'bg_pic' ) ); ?>" id="<?php echo esc_attr( $this->get_field_id( 'bg_pic' ) ); ?>" value="<?php echo esc_url( $instance['bg_pic'] ); ?>" />
                <div class="rtam-preview-wrap" <?php echo $wrap_style; ?>>
                    <img src="<?php echo esc_url( $bg_pic ); ?>" alt="<?php esc_attr_e( 'Preview', 'business-point' ); ?>" />
                </div><!-- .rtam-preview-wrap -->
                <input type="button" class="select-img button button-primary" value="<?php esc_html_e( 'Upload', 'business-point' ); ?>" data-uploader_title="<?php esc_html_e( 'Select Background Image', 'business-point' ); ?>" data-uploader_button_text="<?php esc_html_e( 'Choose Image', 'business-point' ); ?>" />
                <input type="button" value="<?php echo esc_attr_x( 'X', 'Remove Button', 'business-point' ); ?>" class="button button-secondary btn-image-remove" style="<?php echo esc_attr( $delete_button ); ?>" />
            </div>
		<?php
		} 
	
	}

endif;

if ( ! class_exists( 'Business_Point_Services_Widget' ) ) :

	/**
	 * Service widget class.
	 *
	 * @since 1.0.0
	 */
	class Business_Point_Services_Widget extends WP_Widget {

		function __construct() {
			$opts = array(
					'classname'   => 'business_point_widget_services',
					'description' => esc_html__( 'Display services.', 'business-point' ),
			);
			parent::__construct( 'business-point-services', esc_html__( 'BP: Services', 'business-point' ), $opts );
		}

		function widget( $args, $instance ) {

			$title 			= apply_filters( 'widget_title', empty( $instance['title'] ) ? '' : $instance['title'], $instance, $this->id_base );

			$section_icon 	= !empty( $instance['section_icon'] ) ? $instance['section_icon'] : '';

			$sub_title 	 	= !empty( $instance['sub_title'] ) ? $instance['sub_title'] : '';

			$excerpt_length	= !empty( $instance['excerpt_length'] ) ? $instance['excerpt_length'] : 20;

			$read_more_text	= !empty( $instance['read_more_text'] ) ? $instance['read_more_text'] : '';

			$services_ids 	= array();

			$item_number 	= 9;

			for ( $i = 1; $i <= $item_number; $i++ ) {
				if ( ! empty( $instance["item_id_$i"] ) && absint( $instance["item_id_$i"] ) > 0 ) {
					$id = absint( $instance["item_id_$i"] );
					$services_ids[ $id ]['id']   = $id;
					$services_ids[ $id ]['icon'] = $instance["item_icon_$i"];
				}
			}

			echo $args['before_widget']; ?>

			<div class="services-list bp-services">

				<div class="section-title">
					<?php

					if ( $title ) {
						echo $args['before_title'] . esc_html( $title ) . $args['after_title'];
					}

					if ( $section_icon ) { ?>

						<div class="seperator">
						    <span><i class="fa <?php echo esc_html( $section_icon ); ?>"></i></span>
						</div>
						<?php
						
					}

					if ( $sub_title ) { ?>

						<p><?php echo esc_html( $sub_title ); ?></p>

						<?php 
						
					} ?>

				</div>
				<?php

				if ( ! empty( $services_ids ) ) {
					$query_args = array(
						'posts_per_page' => count( $services_ids ),
						'post__in'       => wp_list_pluck( $services_ids, 'id' ),
						'orderby'        => 'post__in',
						'post_type'      => 'page',
						'no_found_rows'  => true,
					);
					$all_services = get_posts( $query_args ); ?>

					<?php if ( ! empty( $all_services ) ) : ?>
						<?php global $post; ?>
						
							<div class="inner-wrapper">

								<?php foreach ( $all_services as $post ) : ?>
									<?php setup_postdata( $post ); ?>
									<div class="services-item">
										<div class="service-icon">
											<i class="fa <?php echo esc_attr( $services_ids[ $post->ID ]['icon'] ); ?>"></i>
										</div>
										<h3 class="services-item-title"><?php the_title(); ?></h3>
										<?php 
										$content = business_point_get_the_excerpt( absint( $excerpt_length ), $post );
										
										echo $content ? wpautop( wp_kses_post( $content ) ) : '';

										if ( ! empty( $read_more_text ) ) {

											echo '<a href="' . esc_url( get_permalink() ) . '" class="btn-continue">' . esc_html( $read_more_text ) . '</a>';

										} ?>
										
									</div><!-- .services-item -->
								<?php endforeach; ?>

							</div><!-- .inner-wrapper -->

						<?php wp_reset_postdata(); ?>

					<?php endif;
				} ?>

			</div><!-- .services-list -->

			<?php

			echo $args['after_widget'];

		}

		function update( $new_instance, $old_instance ) {
			$instance = $old_instance;

			$instance['title'] 			= sanitize_text_field( $new_instance['title'] );

			$instance['section_icon'] 	= sanitize_text_field( $new_instance['section_icon'] );

			$instance['sub_title'] 		= sanitize_text_field( $new_instance['sub_title'] );

			$instance['excerpt_length'] = absint( $new_instance['excerpt_length'] );

			$instance['read_more_text'] = sanitize_text_field( $new_instance['read_more_text'] );

			$item_number = 9;

			for ( $i = 1; $i <= $item_number; $i++ ) {
				$instance["item_id_$i"]   = absint( $new_instance["item_id_$i"] );
				$instance["item_icon_$i"] = sanitize_text_field( $new_instance["item_icon_$i"] );
			}

			return $instance;
		}

		function form( $instance ) {

			// Defaults.
			$defaults = array(
							'title' 			=> '',
							'section_icon' 		=> 'fa-laptop',
							'sub_title' 		=> '',
							'excerpt_length'	=> 20,
							'read_more_text'	=> esc_html__( 'Read More', 'business-point' ),
						);

			$item_number = 9;

			for ( $i = 1; $i <= $item_number; $i++ ) {
				$defaults["item_id_$i"]   = '';
				$defaults["item_icon_$i"] = 'fa-cog';
			}

			$instance = wp_parse_args( (array) $instance, $defaults );
			?>
			<p>
				<label for="<?php echo esc_attr( $this->get_field_id( 'title' ) ); ?>"><strong><?php esc_html_e( 'Title:', 'business-point' ); ?></strong></label>
				<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'title' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'title' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['title'] ); ?>" />
			</p>
			
			<p>
				<label for="<?php echo esc_attr( $this->get_field_id( 'section_icon' ) ); ?>"><strong><?php esc_html_e( 'Icon:', 'business-point' ); ?></strong></label>
				<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'section_icon' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'section_icon' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['section_icon'] ); ?>" />
			</p>

			<p>
				<label for="<?php echo esc_attr( $this->get_field_id( 'sub_title' ) ); ?>"><strong><?php esc_html_e( 'Sub Title:', 'business-point' ); ?></strong></label>
				<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'sub_title' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'sub_title' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['sub_title'] ); ?>" />
			</p>

			<p>
				<label for="<?php echo esc_attr( $this->get_field_name('excerpt_length') ); ?>">
					<?php esc_html_e('Excerpt Length:', 'business-point'); ?>
				</label>
				<input class="widefat" id="<?php echo esc_attr( $this->get_field_id('excerpt_length') ); ?>" name="<?php echo esc_attr( $this->get_field_name('excerpt_length') ); ?>" type="number" value="<?php echo absint( $instance['excerpt_length'] ); ?>" />
			</p>

			<p>
				<label for="<?php echo esc_attr( $this->get_field_id( 'read_more_text' ) ); ?>"><strong><?php esc_html_e( 'Read More Text:', 'business-point' ); ?></strong></label>
				<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'read_more_text' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'read_more_text' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['read_more_text'] ); ?>" />
				<small>
					<?php esc_html_e('Leave this field empty if you want to hide read more button in services section', 'business-point'); ?>	
				</small>
			</p>
			
			<?php
				for ( $i = 1; $i <= $item_number; $i++ ) {
					?>
					<hr>
					<p>
						<label for="<?php echo $this->get_field_id( "item_id_$i" ); ?>"><strong><?php esc_html_e( 'Page:', 'business-point' ); ?>&nbsp;<?php echo $i; ?></strong></label>
						<?php
						wp_dropdown_pages( array(
							'id'               => $this->get_field_id( "item_id_$i" ),
							'class'            => 'widefat',
							'name'             => $this->get_field_name( "item_id_$i" ),
							'selected'         => $instance["item_id_$i"],
							'show_option_none' => esc_html__( '&mdash; Select &mdash;', 'business-point' ),
							)
						);
						?>
					</p>
					<p>
						<label for="<?php echo esc_attr( $this->get_field_id( "item_icon_$i" ) ); ?>"><strong><?php esc_html_e( 'Icon:', 'business-point' ); ?>&nbsp;<?php echo $i; ?></strong></label>
						<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( "item_icon_$i" ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( "item_icon_$i" ) ); ?>" type="text" value="<?php echo esc_attr( $instance["item_icon_$i"] ); ?>" />
					</p>
					<?php 
				}
		}
	}

endif;

if ( ! class_exists( 'Business_Point_Newsletter_Widget' ) ) :

	/**
	 * Contact widget class.
	 *
	 * @since 1.0.0
	 */
	class Business_Point_Newsletter_Widget extends WP_Widget {

		function __construct() {
			$opts = array(
					'classname'   => 'business_point_widget_newsletter',
					'description' => esc_html__( 'Newsletter with mailchimp form', 'business-point' ),
			);
			parent::__construct( 'business-point-newsletter', esc_html__( 'BP: Newsletter', 'business-point' ), $opts );
		}

		function widget( $args, $instance ) {


			$title 					= apply_filters( 'widget_title', empty( $instance['title'] ) ? '' : $instance['title'], $instance, $this->id_base );

			$section_icon 	   		= !empty( $instance['section_icon'] ) ? $instance['section_icon'] : '';

			$sub_title 	 			= !empty( $instance['sub_title'] ) ? $instance['sub_title'] : '';

			$newsletter_shortcode 	= !empty( $instance['newsletter_shortcode'] ) ? $instance['newsletter_shortcode'] : ''; 

			$overlay_type 	 		= !empty( $instance['overlay_type'] ) ? $instance['overlay_type'] : '';

			$bg_pic  	 			= ! empty( $instance['bg_pic'] ) ? esc_url( $instance['bg_pic'] ) : '';

			// Add background image.
			if ( ! empty( $bg_pic ) ) {
				$background_style = '';
				$background_style .= ' style="background-image:url(' . esc_url( $bg_pic ) . ');" ';
				$args['before_widget'] = implode( $background_style . ' ' . 'class="overlay-'.esc_html( $overlay_type ).' bg_enabled ', explode( 'class="', $args['before_widget'], 2 ) );
			}
		
			echo $args['before_widget']; ?>

			<div class="section-newsletter">

				<div class="section-title">

					<?php 

					if ( $title ) {
						echo $args['before_title'] . $title . $args['after_title'];
					}

					if ( $section_icon ) { ?>

						<div class="seperator">
						    <span><i class="fa <?php echo esc_html( $section_icon ); ?>"></i></span>
						</div>
						<?php
						
					}

					if ( $sub_title ) { ?>

						<p><?php echo esc_html( $sub_title ); ?></p>

						<?php 
						
					} ?>

				</div>
				
				<div class="newsletter-wrapper">

					<?php if ( $newsletter_shortcode ) { ?>

						<div class="newsletter-form">

							<?php echo do_shortcode( wp_kses_post( $newsletter_shortcode ) ); ?>

						</div>

					<?php } ?>

				</div><!-- .newsletter-wrapper -->

			</div><!-- .newsletter -->

			<?php 

			echo $args['after_widget'];

		}

		function update( $new_instance, $old_instance ) {
			$instance = $old_instance;

			$instance['title'] 					= sanitize_text_field( $new_instance['title'] );

			$instance['section_icon'] 			= sanitize_text_field( $new_instance['section_icon'] );

			$instance['sub_title'] 		    	= sanitize_text_field( $new_instance['sub_title'] );

			$instance['newsletter_shortcode'] 	= sanitize_text_field( $new_instance['newsletter_shortcode'] );

			$instance['overlay_type'] 			= sanitize_text_field( $new_instance['overlay_type'] );

			$instance['bg_pic']  	 			= esc_url_raw( $new_instance['bg_pic'] );


			return $instance;
		}

		function form( $instance ) {

			// Defaults.
			$defaults = array(
				'title' 				=> '',
				'section_icon' 			=> 'fa-envelope-o',
				'sub_title' 			=> '',
				'newsletter_shortcode' 	=> '',
				'overlay_type'   		=> 'light',
				'bg_pic'      			=> '',
			);

			$instance = wp_parse_args( (array) $instance, $defaults );


			$newsletter_shortcode = !empty( $instance['newsletter_shortcode'] ) ? $instance['newsletter_shortcode'] : '';

			$bg_pic = '';

			if ( ! empty( $instance['bg_pic'] ) ) {

			    $bg_pic = $instance['bg_pic'];

			}

			$wrap_style = '';

			if ( empty( $bg_pic ) ) {

			    $wrap_style = ' style="display:none;" ';
			}

			$image_status = false;

			if ( ! empty( $bg_pic ) ) {
			    $image_status = true;
			}

			$delete_button = 'display:none;';

			if ( true === $image_status ) {
			    $delete_button = 'display:inline-block;';
			}

			?>
			
			<p>
				<label for="<?php echo esc_attr( $this->get_field_id( 'title' ) ); ?>"><strong><?php esc_html_e( 'Title:', 'business-point' ); ?></strong></label>
				<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'title' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'title' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['title'] ); ?>" />
			</p>

			<p>
				<label for="<?php echo esc_attr( $this->get_field_id( 'section_icon' ) ); ?>"><strong><?php esc_html_e( 'Icon:', 'business-point' ); ?></strong></label>
				<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'section_icon' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'section_icon' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['section_icon'] ); ?>" />
			</p>
			<p>
				<label for="<?php echo esc_attr( $this->get_field_id( 'sub_title' ) ); ?>"><strong><?php esc_html_e( 'Sub Title:', 'business-point' ); ?></strong></label>
				<input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'sub_title' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'sub_title' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['sub_title'] ); ?>" />
			</p>

			<p>
				<label for="<?php echo esc_attr( $this->get_field_name('newsletter_shortcode') ); ?>">
					<?php esc_html_e('Newsletter Shortcode:', 'business-point'); ?>
				</label>
				<input class="widefat" id="<?php echo esc_attr( $this->get_field_id('newsletter_shortcode') ); ?>" name="<?php echo esc_attr( $this->get_field_name('newsletter_shortcode') ); ?>" type="text" value="<?php echo esc_attr( $newsletter_shortcode ); ?>" />	
				<small>
		        	<?php esc_html_e('Shortcode of mailchimp or other mailing applications can be used.', 'business-point'); ?>	
		        </small>	
			</p>

	        <p>
	          <label for="<?php echo esc_attr( $this->get_field_id( 'overlay_type' ) ); ?>"><strong><?php _e( 'Overlay Type:', 'business-point' ); ?></strong></label>
				<?php
	            $this->dropdown_overlay_type( array(
					'id'       => $this->get_field_id( 'overlay_type' ),
					'name'     => $this->get_field_name( 'overlay_type' ),
					'selected' => esc_attr( $instance['overlay_type'] ),
					)
	            );
				?>
	        </p>

			<div class="cover-image">
                <label for="<?php echo esc_attr( $this->get_field_id( 'bg_pic' ) ); ?>">
                    <strong><?php esc_html_e( 'Background Image:', 'business-point' ); ?></strong>
                </label>
                <input type="text" class="img widefat" name="<?php echo esc_attr( $this->get_field_name( 'bg_pic' ) ); ?>" id="<?php echo esc_attr( $this->get_field_id( 'bg_pic' ) ); ?>" value="<?php echo esc_url( $instance['bg_pic'] ); ?>" />
                <div class="rtam-preview-wrap" <?php echo $wrap_style; ?>>
                    <img src="<?php echo esc_url( $bg_pic ); ?>" alt="<?php esc_attr_e( 'Preview', 'business-point' ); ?>" />
                </div><!-- .rtam-preview-wrap -->
                <input type="button" class="select-img button button-primary" value="<?php esc_html_e( 'Upload', 'business-point' ); ?>" data-uploader_title="<?php esc_html_e( 'Select Background Image', 'business-point' ); ?>" data-uploader_button_text="<?php esc_html_e( 'Choose Image', 'business-point' ); ?>" />
                <input type="button" value="<?php echo esc_attr_x( 'X', 'Remove Button', 'business-point' ); ?>" class="button button-secondary btn-image-remove" style="<?php echo esc_attr( $delete_button ); ?>" />
            </div>
					
		<?php
				
		}

	    function dropdown_overlay_type( $args ) {
			$defaults = array(
		        'id'       => '',
		        'class'    => 'widefat',
		        'name'     => '',
		        'selected' => 'light',
			);

			$r = wp_parse_args( $args, $defaults );
			$output = '';

			$choices = array(
				'light' 		=> esc_html__( 'Light', 'business-point' ),
				'dark' 			=> esc_html__( 'Dark', 'business-point' ),
				'none' 			=> esc_html__( 'None', 'business-point' ),
			);

			if ( ! empty( $choices ) ) {

				$output = "<select name='" . esc_attr( $r['name'] ) . "' id='" . esc_attr( $r['id'] ) . "' class='" . esc_attr( $r['class'] ) . "'>\n";
				foreach ( $choices as $key => $choice ) {
					$output .= '<option value="' . esc_attr( $key ) . '" ';
					$output .= selected( $r['selected'], $key, false );
					$output .= '>' . esc_html( $choice ) . '</option>\n';
				}
				$output .= "</select>\n";
			}

			echo $output;
	    }
	}

endif;

if ( ! class_exists( 'Business_Point_Recent_Posts_Widget' ) ) :

	/**
	 * Recent Posts widget class.
	 *
	 * @since 1.0.0
	 */
	class Business_Point_Recent_Posts_Widget extends WP_Widget {

	    function __construct() {
	    	$opts = array(
				'classname'   => 'business_point_widget_advanced_recent_posts',
				'description' => esc_html__( 'Widget to display recent posts with thumbnail. Receommneded to use in sidebar or footer widgets area.', 'business-point' ),
    		);

			parent::__construct( 'business-point-advanced-recent-posts', esc_html__( 'BP: Advanced Recent Posts', 'business-point' ), $opts );
	    }


	    function widget( $args, $instance ) {

			$title       = apply_filters( 'widget_title', empty( $instance['title'] ) ? '' : $instance['title'], $instance, $this->id_base );

			$post_number = ! empty( $instance['post_number'] ) ? $instance['post_number'] : 5;

	        echo $args['before_widget']; ?>

	        <div class="bp-advanced-recent-posts">

        		<?php 

        		if ( $title ) {
        			echo $args['before_title'] . esc_html( $title ) . $args['after_title'];
        		} ?>

        		<div class="recent-posts-side">

        		    <?php

        		    $recent_args = array(
        		                        'posts_per_page'        => absint( $post_number ),
        		                        'no_found_rows'         => true,
        		                        'post__not_in'          => get_option( 'sticky_posts' ),
        		                        'ignore_sticky_posts'   => true,
        		                        'post_status'           => 'publish', 
        		                    );

        		    $recent_posts = new WP_Query( $recent_args );

        		    if ( $recent_posts->have_posts() ) :


        		        while ( $recent_posts->have_posts() ) :

        		            $recent_posts->the_post(); ?>

        		            <div class="news-item">
        		                <div class="news-thumb">
        		                    <a href="<?php the_permalink(); ?>"><?php the_post_thumbnail('thumbnail'); ?></a>   
        		                </div><!-- .news-thumb --> 

        		                <div class="news-text-wrap">
        		                    <h2><a href="<?php the_permalink(); ?>"><?php the_title(); ?></a></h2>
        		                     <span class="posted-date"><?php echo esc_html( get_the_date() ); ?></span>
        		                </div><!-- .news-text-wrap -->
        		            </div><!-- .news-item -->

        		            <?php

        		        endwhile; 

        		        wp_reset_postdata(); ?>

        		    <?php endif; ?>

        		</div>

	        </div><!-- .bp-advanced-recent-posts -->

	        <?php
	        echo $args['after_widget'];

	    }

	    function update( $new_instance, $old_instance ) {
	        $instance = $old_instance;
			$instance['title']           = sanitize_text_field( $new_instance['title'] );
			$instance['post_number']     = absint( $new_instance['post_number'] );

	        return $instance;
	    }

	    function form( $instance ) {

	        $instance = wp_parse_args( (array) $instance, array(
				'title'         => esc_html__( 'Recent Posts', 'business-point' ),
				'post_number'   => 5,
	        ) );
	        ?>
	        <p>
	          <label for="<?php echo esc_attr( $this->get_field_id( 'title' ) ); ?>"><strong><?php esc_html_e( 'Title:', 'business-point' ); ?></strong></label>
	          <input class="widefat" id="<?php echo esc_attr( $this->get_field_id( 'title' ) ); ?>" name="<?php echo esc_attr( $this->get_field_name( 'title' ) ); ?>" type="text" value="<?php echo esc_attr( $instance['title'] ); ?>" />
	        </p>
	        <p>
	            <label for="<?php echo esc_attr( $this->get_field_name('post_number') ); ?>">
	                <?php esc_html_e('Number of Posts:', 'business-point'); ?>
	            </label>
	            <input class="widefat" id="<?php echo esc_attr( $this->get_field_id('post_number') ); ?>" name="<?php echo esc_attr( $this->get_field_name('post_number') ); ?>" type="number" value="<?php echo absint( $instance['post_number'] ); ?>" />
	        </p>
	        <?php
	    }

	}

endif;