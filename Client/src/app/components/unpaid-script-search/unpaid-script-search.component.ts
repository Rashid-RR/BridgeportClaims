import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DatePipe } from '@angular/common';
// Services
import { UnpaidScriptService } from '../../services/unpaid-script.service';
declare var $:any;

@Component({
  selector: 'app-unpaid-script-search',
  templateUrl: './unpaid-script-search.component.html',
  styleUrls: ['./unpaid-script-search.component.css']
})
export class UnpaidScriptSearchComponent implements OnInit, AfterViewInit {

  startDate: string;
  endDate: string;
  constructor(
    private us: UnpaidScriptService,
    private toast: ToastsManager,
    private dp: DatePipe,
    private fb: FormBuilder
  ) {
      
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    // Date picker
    $('#startDate').datepicker({
      autoclose: true
    });
    $('#endDate').datepicker({
      autoclose: true
    });
    $('#datemask').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' });
    $('[data-mask]').inputmask();
  }

  search() {
    let startDate = this.dp.transform($('#startDate').val(), "dd/M/yyyy");
    let endDate = this.dp.transform($('#endDate').val(), "dd/M/yyyy");
 
    //if(this.startDate && this.endDate){
      this.us.data.startDate = startDate || null
      this.us.data.endDate = endDate || null
      
      this.us.search();
   /*  }else{
        this.toast.warning("Ensure you select both start date and end date");
    } */
  }
  clearDates(){
    $('#startDate').val('');
    $('#endDate').val(''); 
  }
}
