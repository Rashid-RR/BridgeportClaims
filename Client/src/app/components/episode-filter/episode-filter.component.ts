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
  episodeTypeId: string=null;
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
    let startDate = this.dp.transform($('#startDate').val(), "dd/MM/yyyy");
    let endDate = this.dp.transform($('#endDate').val(), "dd/MM/yyyy");
    this.ds.data.resolved = this.resolved;
    this.ds.data.startDate = startDate || null
    this.ds.data.endDate = endDate || null
    this.ds.data.OwnerID = this.ownerId ? this.ownerId : null;
    this.ds.data.episodeCategoryId = this.episodeCategoryId ? Number(this.episodeCategoryId) : null;
    this.ds.data.episodeTypeId = this.episodeTypeId ? Number(this.episodeTypeId) : null;
    this.ds.search();
  }

  refresh() {
    this.ds.data= this.data = {
      startDate: null,
      endDate: null,
      episodeCategoryId:null,
      episodeTypeId:null,
      OwnerID:null,
      resolved: false,
      sortColumn: "Created",
      sortDirection: "DESC",
      pageNumber: 1,
      pageSize: 30
    };
    this.reset();
    this.resolved=false;
    this.ds.search();
  }
  reset() {
    $('#startDate').val('');
    $('#endDate').val('');
    this.ownerId = null;
    this.ds.owner = null;
    this.ds.episodeNote = null;
    this.episodeCategoryId = null;
    this.episodeTypeId = null;
  }

}
