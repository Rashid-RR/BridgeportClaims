<?php
/**
 * The sidebar containing the front page widget areas
 *
 * If no active widgets are in either sidebar, hide them completely.
 *
 * @package WordPress
 * @subpackage Twenty_Twelve
 * @since Twenty Twelve 1.0
 */

/*
 * The front page widget area is triggered if any of the areas
 * have widgets. So let's check that first.
 *
 * If none of the sidebars have widgets, then let's bail early.
 */
if ( ! is_active_sidebar( 'sidebar-2' ) && ! is_active_sidebar( 'sidebar-3' ) )
	return;

// If we get this far, we have widgets. Let do this.
?>
<div id="secondary" class="widget-area" role="complementary">
	<?php if ( is_active_sidebar( 'sidebar-2' ) ) : ?>
	<div class="first front-widgets">
		<?php dynamic_sidebar( 'sidebar-2' ); ?>
        <h3>Recent News</h3>
        
        <ul class="news">
            <?php $index_query = new WP_Query(array( 'post_type' => 'latest_news', 'posts_per_page' => 3,'order'=>'DES' )); ?>
    		<?php while ($index_query->have_posts()) : $index_query->the_post(); ?>
            <li>
				<a href="<?php echo get_permalink(); ?>"><?php the_post_thumbnail( array(60,60) ); ?></a>
                <h4><?php the_title();?></h4>
                <span class="info"><?php echo get_the_date(); ?></span>
                
                <p>
                	<?php 
				
					   $content = get_the_content();
					   $trimmed_content = wp_trim_words( $content, 10, '' );
					   echo $trimmed_content;
					?>
                </p>
			</li>
			<?php endwhile; wp_reset_postdata(); ?>
        </ul>
	</div><!-- .first -->
	<?php endif; ?>

	<?php if ( is_active_sidebar( 'sidebar-3' ) ) : ?>
	<div class="second front-widgets">
		<?php dynamic_sidebar( 'sidebar-3' ); ?>
	</div><!-- .second -->
	<?php endif; ?>
</div><!-- #secondary -->