declare var jQuery:any 

export function warn(title?:string){
     jQuery.notify({
        // options
        icon:"glyphicon glyphicon-warning-sign",
        message: title !==undefined ? title : 'Please correct the errors highlighted in red' 
      },{
        // settings
        type: 'warning',
        animate: {
          enter: 'animated swing',
          exit: 'animated fadeOutUp'
        },
        offset: 50,
        delay: 5000,
	    z_index: 1031,
        allow_dismiss: true,
          placement: {
            from: 'bottom',
            align: 'right'
          }
      }); 
  }
  export function success(message){
    jQuery.notify({
          // options
          icon:"glyphicon glyphicon-warning-sign",
          message: message 
        },{
          // settings
          type: 'success',
          animate: {
            enter: 'animated fadeInDown',
            exit: 'animated fadeOutUp'
          },
          offset: 50,
          delay: 5000,
          allow_dismiss: true,
            placement: {
              from: 'bottom',
              align: 'right'
            }
        }); 
  }