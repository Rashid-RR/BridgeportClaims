import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DatePipe } from '@angular/common';
// Services
import { EpisodeService } from '../../services/episode.service';
declare var $: any;

@Component({
  selector: 'app-episode-filter',
  templateUrl: './episode-filter.component.html',
  styleUrls: ['./episode-filter.component.css']
})
export class EpisodeFilterComponent implements OnInit, AfterViewInit {
 
  startDate: String;
  endDate: String;
  ownerId: string=null;
  episodeCategoryId: string=null;
  open: boolean = true;
  closed: Boolean = false;
  submitted: boolean = false;
  resolved: Boolean = false;
  constructor(
    public ds: EpisodeService,
    private dp: DatePipe,
    private toast: ToastsManager,
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

  setClosed($event, value) {
    if ($event.target.checked) {
      this.resolved = value;
      this.search();
    }
  }
  search() {
    let startDate = this.dp.transform($('#startDate').val(), "MM/dd/yyyy");
    let endDate = this.dp.transform($('#endDate').val(), "MM/dd/yyyy");
    this.ds.data.resolved = this.resolved;
    this.ds.data.startDate = startDate || null
    this.ds.data.endDate = endDate || null
    this.ds.data.OwnerID = this.ownerId ? this.ownerId : null;
    this.ds.data.episodeCategoryId = Number(this.episodeCategoryId) ? this.episodeCategoryId : null;
    this.ds.search();
  }

  clearText() {
    this.ds.owner = null;
    this.ds.episodeNote = null;
  }
  clearDates() {
    $('#startDate').val('');
    $('#endDate').val('');
  }
  reset() {
    $('#startDate').val('');
    $('#endDate').val('');
    this.ds.owner = null;
    this.ds.episodeNote = null;
  }

}
