<?php
/**
 * Template part for displaying results in search pages.
 *
 * @link https://codex.wordpress.org/Template_Hierarchy
 *
 * @package Business_Point
 */

?>

<article id="post-<?php the_ID(); ?>" <?php post_class(); ?>>

	<div class="content-wrap">
		<div class="content-wrap-inner">
			<header class="entry-header">
				<?php
				$cat_meta = business_point_get_option( 'category_meta' );

				if ( ( 'post' === get_post_type() ) && ( 1 !== absint( $cat_meta ) ) ) : ?>
					<div class="entry-meta">
						<?php business_point_posted_on(); ?>
					</div><!-- .entry-meta -->
					<?php
				endif; 

				the_title( '<h2 class="entry-title"><a href="' . esc_url( get_permalink() ) . '" rel="bookmark">', '</a></h2>' );
				?>
			</header><!-- .entry-header -->

			<div class="entry-summary">
				<?php the_excerpt(); ?>
				
				<div class="entry-footer">
					<?php business_point_entry_footer(); ?>
				</div>
			</div><!-- .entry-content -->
		</div>
	</div>

</article><!-- #post-## -->