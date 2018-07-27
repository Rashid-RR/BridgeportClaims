<?php
/**
 * The Template for displaying all single posts
 *
 * @package WordPress
 * @subpackage Twenty_Twelve
 * @since Twenty Twelve 1.0
 */

get_header(); ?>
<?php $index_query = new WP_Query(array( 'post_type' => 'page', 'p' => '9' , 'posts_per_page' => '1')); ?>
<?php while ($index_query->have_posts()) : $index_query->the_post(); ?>
<div class="inner-banner">
	<?php the_post_thumbnail(); ?>
    <div class="title-area"><h3>What we do</h3></div>
</div>
<?php endwhile; wp_reset_query(); ?>

<div class="main-wrapper">
	<div class="inner-content">
    	<?php if ( have_posts() ) while ( have_posts() ) : the_post(); ?>
        <?php the_content(); ?>
        <?php $id = get_the_ID(); ?>
        <?php endwhile; // end of the loop. ?>
        <div class="clear"></div>
	</div>
</div>

<div class="main-wrapper">
	<div class="what-we-do-area">
    	<h2>Other Services</h2>
    	<ul>
		<?php $index_query = new WP_Query(array( 'post_type' => 'home_boxes', 'posts_per_page' => 3,'order'=>'DES', 'post__not_in' => array($id) )); ?>
        <?php while ($index_query->have_posts()) : $index_query->the_post(); ?>
        <li>
            <?php the_post_thumbnail('full'); ?>
            <h3><?php the_title(); ?></h3>
            <?php the_excerpt(); ?>
            <a href="<?php the_permalink(); ?>">Service Detail</a>
        </li>
        <?php endwhile; wp_reset_postdata(); ?>
        </ul>
		<div class="clear"></div>
	</div>
</div>

<?php get_footer(); ?>