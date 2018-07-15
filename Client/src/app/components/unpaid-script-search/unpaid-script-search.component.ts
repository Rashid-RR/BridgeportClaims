import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, } from '@angular/forms';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { HttpService } from '../../services/services.barrel';
import { DatePipe } from '@angular/common';
import { Payor } from "../../models/payor"
import { Subject } from 'rxjs/Subject';
// Services
import { UnpaidScriptService } from '../../services/unpaid-script.service';
declare var $: any;

@Component({
  selector: 'app-unpaid-script-search',
  templateUrl: './unpaid-script-search.component.html',
  styleUrls: ['./unpaid-script-search.component.css']
})
export class UnpaidScriptSearchComponent implements OnInit, AfterViewInit {

  startDate: string;
  endDate: string;
  submitted: boolean = false;
  payors: Array<Payor> = [];
  loading: boolean = false;
  pageNumber: number;
  pageSize: number = 50;
  payorListReady = new Subject<any>();
  constructor(
    private http: HttpService,
    public us: UnpaidScriptService,
    public dp: DatePipe
  ) {

  }

  ngOnInit() {
    this.us.payorListReady.subscribe(() => {
      $("#payorsSelection").select2();
    })
  }
  filter($event) {
    this.us.data.isArchived = $event.target.checked;
  }
  

  ngAfterViewInit() {
    if(this.us.payors && this.us.payors.length>0){
      $("#payorsSelection").select2();
    }
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
    this.us.data.payorIds=($('#payorsSelection').val()||undefined);
    let startDate = this.dp.transform($('#startDate').val(), "MM/dd/yyyy");
    let endDate = this.dp.transform($('#endDate').val(), "MM/dd/yyyy");
    this.us.data.startDate = startDate || null
    this.us.data.endDate = endDate || null
    this.us.search();
  }
  clearDates() {
    $('#startDate').val('');
    $('#payorsSelection').val(null).trigger('change');
    $('#endDate').val('');
    $('#archivedCheck').prop('checked',false);
  }
}
