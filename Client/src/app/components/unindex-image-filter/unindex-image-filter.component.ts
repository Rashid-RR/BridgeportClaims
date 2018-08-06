import { Component, NgZone, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastsManager } from 'ng2-toastr';
import { DatePipe } from '@angular/common';
// Services
import { DocumentManagerService } from "../../services/document-manager.service";
declare var $: any;

@Component({
  selector: 'app-unindex-image-filter',
  templateUrl: './unindex-image-filter.component.html',
  styleUrls: ['./unindex-image-filter.component.css']
})
export class UnindexedImageFilterComponent implements OnInit, AfterViewInit {

  date: string;
  fileName: string;
  submitted: boolean = false;
  constructor(
    public ds: DocumentManagerService,
    private dp: DatePipe,
    private zone: NgZone,
    private route: ActivatedRoute,
    private toast: ToastsManager,
    private fb: FormBuilder) { }

  ngOnInit() {

  }
  ngAfterViewInit() {
    // Date picker
    $('#date').datepicker({
      autoclose: true
    });
    this.route.params.subscribe(params => {
      if (params['date']) {
        this.date = params['date'].replace(/\-/g, "/");
        this.ds.data.date = this.date;
        this.zone.run(() => {
          this.ds.search();
        })
      }
    });
  }

  search() {
    let date = this.dp.transform($('#date').val(), "MM/dd/yyyy");
    //if(this.startDate && this.endDate){
    this.ds.data.date = date || null
    this.ds.data.fileName = this.fileName || null
    this.ds.search();
    /*  }else{
         this.toast.warning("Ensure you select both start date and end date");
     } */
  }
  filter($event) {
    this.ds.data.archived = $event.target.checked;

  }


  clearDates() {
    $('#date').val('');
    this.fileName = '';
  }


}
