<?php


add_action( 'admin_init', 'theme_options_init' );
add_action( 'admin_menu', 'theme_options_add_page' );

/**
 * Init plugin options to white list our options
 */
function theme_options_init(){
	register_setting( 'sample_options', 'cOptn', 'theme_options_validate' );
}

/**
 * Load up the menu page
 */
function theme_options_add_page() {
	add_theme_page( __( 'Theme Options', 'sampletheme' ), __( 'Theme Options', 'sampletheme' ), 'edit_theme_options', 'theme_options', 'theme_options_do_page' );
}

/**
 * Create arrays for our select and radio options
 */
$select_options = array(
	'0' => array(
		'value' =>	'0',
		'label' => __( 'Zero', 'sampletheme' )
	),
	'1' => array(
		'value' =>	'1',
		'label' => __( 'One', 'sampletheme' )
	),
	'2' => array(
		'value' => '2',
		'label' => __( 'Two', 'sampletheme' )
	),
	'3' => array(
		'value' => '3',
		'label' => __( 'Three', 'sampletheme' )
	),
	'4' => array(
		'value' => '4',
		'label' => __( 'Four', 'sampletheme' )
	),
	'5' => array(
		'value' => '3',
		'label' => __( 'Five', 'sampletheme' )
	)
);

$radio_options = array(
	'yes' => array(
		'value' => 'yes',
		'label' => __( 'Yes', 'sampletheme' )
	),
	'no' => array(
		'value' => 'no',
		'label' => __( 'No', 'sampletheme' )
	),
	'maybe' => array(
		'value' => 'maybe',
		'label' => __( 'Maybe', 'sampletheme' )
	)
);

/**
 * Create the options page
 */
function theme_options_do_page() {
	global $select_options, $radio_options;

	if ( ! isset( $_REQUEST['settings-updated'] ) )
		$_REQUEST['settings-updated'] = false;

	?>
	<div class="wrap">
		<?php screen_icon(); echo "<h2>" . get_current_theme() . __( ' Options', 'sampletheme' ) . "</h2>"; ?>

		<?php if ( false !== $_REQUEST['settings-updated'] ) : ?>
		<div class="updated fade"><p><strong><?php _e( 'Options saved', 'sampletheme' ); ?></strong></p></div>
		<?php endif; ?>
		<?php // echo $_SERVER['REQUEST_URI']; ?>
		<form id="file-form" enctype="multipart/form-data" action=" <?php bloginfo('siteurl'); ?>/wp-admin/themes.php?page=theme_options" method="POST">
            <table class="form-table">
    
                    <tr valign="top"><th scope="row"><?php _e( 'Logo Path', 'sampletheme' ); ?></th>
                        <td>
                            <p id="async-upload-wrap">
                            <label for="async-upload">Upload</label>
                            <input type="file" id="async-upload" name="async-upload"> <input type="submit" value="Upload" name="html-upload">
                            </p>
                        
                            <p>
                            <input type="hidden" name="post_id" id="post_id" value="1199" />
                            <?php wp_nonce_field('client-file-upload'); ?>
                            <input type="hidden" name="redirect_to" value="<?php echo $_SERVER['REQUEST_URI']; ?>" />
                            </p>
                        
                            <p>
                            <input type="submit" value="Save all changes" name="save" style="display: none;">
                            </p>
                        </td>
        </tr></table>
        </form>
    
    <?php 
	if ( isset( $_POST['html-upload'] ) && !empty( $_FILES ) ) {
	require_once(ABSPATH . 'wp-admin/includes/admin.php');
	$id = media_handle_upload('async-upload', 1199); //post id of Client Files page
	unset($_FILES);
	if ( is_wp_error($id) ) {
		$errors['upload_error'] = $id;
		$id = false;
	}

	if ($errors) {
		echo "<p>There was an error uploading your file.</p>";
	} else {
		echo "<p>Your file has been uploaded.</p>";
		//echo $id;
		//echo $_FILES['image']['name'][0];
		echo wp_get_attachment_image($options['logo'],'100');
	}
	
}
echo wp_get_attachment_image($options['logo'],'100');
	?>
    
        
        
        <form method="post" action="options.php" >
			<?php settings_fields( 'sample_options' ); ?>
			<?php $options = get_option( 'cOptn' ); ?>

			<table class="form-table">

				<tr valign="top"><th scope="row"><?php _e( 'Preview', 'sampletheme' ); ?></th>
					<td>
                    	<?php 
							$options = get_option('cOptn');
							$oldId = $options['logo'];
							if( $id == NULL){
								$id = $oldId;	
							}
							echo wp_get_attachment_image($id,'100');
						 ?>
                        
						<input id="cOptn[logo]" class="regular-text" type="text" name="cOptn[logo]" value="<?php esc_attr_e( $id ); ?>" style=" width:1px"  />
						<label class="description" for="cOptn[logo]"><?php _e( 'Uploaded media Path', 'sampletheme' ); ?></label>
                        
					</td>
				</tr>
				
				<?php
				/**
				 * A sample textarea option
				 */
				?>
				<tr valign="top"><th scope="row"><?php _e( 'Copyright', 'sampletheme' ); ?></th>
					<td>
						<textarea id="cOptn[copyright]" class="large-text" cols="50" rows="10" name="cOptn[copyright]"><?php echo esc_textarea( $options['copyright'] ); ?></textarea>
						<label class="description" for="cOptn[copyright]"><?php _e( 'Footer Copyright Text', 'sampletheme' ); ?></label>
					</td>
				</tr>
                
                <tr valign="top"><th scope="row"><?php _e( 'twitter', 'sampletheme' ); ?></th>
					<td>
						<input id="cOptn[twitter]" class="large-text" type="text" name="cOptn[twitter]" value="<?php echo esc_textarea( $options['twitter'] ); ?>">
                        <label class="description" for="cOptn[twitter]"><?php _e( 'Please Enter Twitter profile link', 'sampletheme' ); ?></label>
					</td>
				</tr>
                
                <tr valign="top"><th scope="row"><?php _e( 'tagline', 'sampletheme' ); ?></th>
					<td>
						<input id="cOptn[tagline]" class="large-text" type="text" name="cOptn[tagline]" value="<?php echo esc_textarea( $options['tagline'] ); ?>">
                        <label class="description" for="cOptn[tagline]"><?php _e( 'Please Enter tagline', 'sampletheme' ); ?></label>
					</td>
				</tr>
                
                <tr valign="top"><th scope="row"><?php _e( 'googleplus', 'sampletheme' ); ?></th>
					<td>
						<input id="cOptn[linkedin]" class="large-text" type="text" name="cOptn[googleplus]" value="<?php echo esc_textarea( $options['googleplus'] ); ?>">
                        <label class="description" for="cOptn[googleplus]"><?php _e( 'Please Enter Google Plus link', 'sampletheme' ); ?></label>
					</td>
				</tr>
                            
                
                
              
                
                <tr valign="top"><th scope="row"><?php _e( 'phone', 'sampletheme' ); ?></th>
					<td>
						<input id="cOptn[phone]" class="large-text" type="text" name="cOptn[phone]" value="<?php echo esc_textarea( $options['phone'] ); ?>">
                        <label class="description" for="cOptn[phone]"><?php _e( 'Please Enter Phone Number', 'sampletheme' ); ?></label>
					</td>
				</tr>
                
                <tr valign="top"><th scope="row"><?php _e( 'fax', 'sampletheme' ); ?></th>
					<td>
						<input id="cOptn[fax]" class="large-text" type="text" name="cOptn[fax]" value="<?php echo esc_textarea( $options['fax'] ); ?>">
                        <label class="description" for="cOptn[fax]"><?php _e( 'Please Enter Fax', 'sampletheme' ); ?></label>
					</td>
				</tr>
                
                <tr valign="top"><th scope="row"><?php _e( 'email', 'sampletheme' ); ?></th>
					<td>
						<input id="cOptn[email]" class="large-text" type="text" name="cOptn[email]" value="<?php echo esc_textarea( $options['email'] ); ?>">
                        <label class="description" for="cOptn[email]"><?php _e( 'Please Enter Your Email Address', 'sampletheme' ); ?></label>
					</td>
				</tr>
                
                 <tr valign="top"><th scope="row"><?php _e( 'address', 'sampletheme' ); ?></th>
					<td>
						<input id="cOptn[address]" class="large-text" type="text" name="cOptn[address]" value="<?php echo esc_textarea( $options['address'] ); ?>">
                        <label class="description" for="cOptn[address]"><?php _e( 'Please Enter Your Address', 'sampletheme' ); ?></label>
					</td>
				</tr>
                        <tr valign="top"><th scope="row"><?php _e( 'facebook', 'sampletheme' ); ?></th>
					<td>
						<input id="cOptn[facebook]" class="large-text" type="text" name="cOptn[facebook]" value="<?php echo esc_textarea( $options['facebook'] ); ?>">
                        <label class="description" for="cOptn[facebook]"><?php _e( 'Please Enter Your Facebook', 'sampletheme' ); ?></label>
					</td>
				</tr>
                
                <tr valign="top"><th scope="row"><?php _e( 'pinterest', 'sampletheme' ); ?></th>
					<td>
						<input id="cOptn[pinterest]" class="large-text" type="text" name="cOptn[pinterest]" value="<?php echo esc_textarea( $options['pinterest'] ); ?>">
                        <label class="description" for="cOptn[pinterest]"><?php _e( 'Please Enter Your Pinterest', 'sampletheme' ); ?></label>
					</td>
				</tr>
            
                 <tr valign="top"><th scope="row"><?php _e( 'linkedin', 'sampletheme' ); ?></th>
					<td>
						<input id="cOptn[linkedin]" class="large-text" type="text" name="cOptn[linkedin]" value="<?php echo esc_textarea( $options['linkedin'] ); ?>">
                        <label class="description" for="cOptn[linkedin]"><?php _e( 'Please Enter Your linkedin', 'sampletheme' ); ?></label>
					</td>
				</tr>
                
                <tr valign="top"><th scope="row"><?php _e( 'instagram', 'sampletheme' ); ?></th>
					<td>
						<input id="cOptn[instagram]" class="large-text" type="text" name="cOptn[instagram]" value="<?php echo esc_textarea( $options['instagram'] ); ?>">
                        <label class="description" for="cOptn[instagram]"><?php _e( 'Please Enter Your instagram', 'sampletheme' ); ?></label>
					</td>
				</tr>
			</table>

			<p class="submit">
				<input type="submit" class="button-primary" value="<?php _e( 'Save Options', 'sampletheme' ); ?>" />
			</p>
		</form>
	</div>
	<?php
}

/**
 * Sanitize and validate input. Accepts an array, return a sanitized array.
 */
function theme_options_validate( $input ) {
	global $select_options, $radio_options;

	// Our checkbox value is either 0 or 1
	if ( ! isset( $input['option1'] ) )
		$input['option1'] = null;
	$input['option1'] = ( $input['option1'] == 1 ? 1 : 0 );

	// Say our text option must be safe text with no HTML tags
	$input['sometext'] = wp_filter_nohtml_kses( $input['sometext'] );

	// Our select option must actually be in our array of select options
	if ( ! array_key_exists( $input['selectinput'], $select_options ) )
		$input['selectinput'] = null;

	// Our radio option must actually be in our array of radio options
	if ( ! isset( $input['radioinput'] ) )
		$input['radioinput'] = null;
	if ( ! array_key_exists( $input['radioinput'], $radio_options ) )
		$input['radioinput'] = null;

	// Say our textarea option must be safe text with the allowed tags for posts
	$input['sometextarea'] = wp_filter_post_kses( $input['sometextarea'] );

	return $input;
}

// adapted from http://planetozh.com/blog/2009/05/handling-plugins-options-in-wordpress-28-with-register_setting/