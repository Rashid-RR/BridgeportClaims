<?php
/**
 * The Template for displaying all single posts
 *
 * @package WordPress
 * @subpackage Twenty_Twelve
 * @since Twenty Twelve 1.0
 */

get_header(); ?>
<div class="main-wrapper">
	<div class="inner-content">
    	<?php if ( have_posts() ) while ( have_posts() ) : the_post(); ?>
        <h2><?php the_title(); ?></h2>
		<?php the_content(); ?>
        <?php endwhile; // end of the loop. ?>
        <div class="clear"></div>
	</div>
</div>
<?php get_footer(); ?>