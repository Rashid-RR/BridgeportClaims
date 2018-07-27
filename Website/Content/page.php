<?php
/**
 * The template for displaying all pages
 *
 * This is the template that displays all pages by default.
 * Please note that this is the WordPress construct of pages
 * and that other 'pages' on your WordPress site will use a
 * different template.
 *
 * @package WordPress
 * @subpackage Twenty_Twelve
 * @since Twenty Twelve 1.0
 */

get_header(); ?>
<?php while ( have_posts() ) : the_post(); ?>
<div class="banner-area">
	<div class="page-banner" style="background:url(
<?php if ( has_post_thumbnail() ) {?>
<?php 
	$thumb = wp_get_attachment_image_src( get_post_thumbnail_id($post->ID), 'full' );
	$url = $thumb['0'];
	echo $url;
} 
?>
) no-repeat top center;
    background-repeat: no-repeat;
    background-size: cover;"></div>
    <div class="main-wrapper gap-wrapper">
        <div class="inner-content">
        	<h2><?php the_title(); ?></h2>
            <?php the_content(); ?>
            <?php if(is_page('our-team')): ?>
            <ul class="team-list">
			<?php $index_query = new WP_Query(array( 'post_type' => 'team', 'posts_per_page' => 4,'order'=>'ASC' )); ?>
			<?php while ($index_query->have_posts()) : $index_query->the_post(); ?>
        	<li>
				<div class="image-area"><?php the_post_thumbnail(); ?></div>
                <h3><?php the_title(); ?></h3>
        	</li>
            <?php endwhile; wp_reset_postdata(); ?>
            </ul>
            <?php endif; ?>
            <?php if(is_page('why-bridgeport')): ?>
            <ul class="bridge-list">
			<?php $index_query = new WP_Query(array( 'post_type' => 'why_bridge', 'posts_per_page' => 3,'order'=>'ASC' )); ?>
			<?php while ($index_query->have_posts()) : $index_query->the_post(); ?>
        	<li>
            	<h3><?php the_title(); ?></h3>
				<div class="image-area"><?php the_post_thumbnail(); ?></div>
                <?php the_content(); ?>
        	</li>
            <?php endwhile; wp_reset_postdata(); ?>
            </ul>
            <?php endif; ?>
            <div class="clear"></div>
        </div>
	</div>
    <?php endwhile; // end of the loop. ?>
    <?php get_footer(); ?>
</div>