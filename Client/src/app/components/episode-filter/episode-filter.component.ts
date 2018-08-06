import { Component, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ToastsManager } from 'ng2-toastr';
import { DatePipe } from '@angular/common';
import swal from "sweetalert2";
import { SwalComponent, SwalPartialTargets } from '@toverux/ngx-sweetalert2';
import { PrescriptionNoteType } from "../../models/prescription-note-type";

// Services
import { HttpService } from "../../services/http-service"
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
  ownerId: string = null;
  episodeCategoryId: string = null;
  episodeTypeId: string = null;
  open: boolean = true;
  closed: Boolean = false;
  submitted: boolean = false;
  resolved: Boolean = false;
  @ViewChild('episodeSwal') private episodeSwal: SwalComponent;
  constructor(
    public ds: EpisodeService,
    private dp: DatePipe,
    public readonly swalTargets: SwalPartialTargets,
    private http: HttpService,
    private toast: ToastsManager,
    private fb: FormBuilder
  ) {

  }

  ngOnInit() {
    this.ds.loading = true;
    this.http.getEpisodesNoteTypes()
      .subscribe((result: Array<any>) => {
        this.ds.episodeNoteTypes = result;
      }, err => {
        this.ds.loading = false;
        let error = err.error
        console.log(error);
      });
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
  filter($event) {
    this.ds.data.archived = $event.target.checked;
  }
  search() {
    let startDate = this.dp.transform($('#startDate').val(), "MM/dd/yyyy");
    let endDate = this.dp.transform($('#endDate').val(), "MM/dd/yyyy");
    this.ds.data.resolved = this.resolved;
    this.ds.data.startDate = startDate || null
    this.ds.data.endDate = endDate || null
    this.ds.data.OwnerID = this.ownerId ? this.ownerId : null;
    this.ds.data.episodeCategoryId = this.episodeCategoryId ? Number(this.episodeCategoryId) : null;
    this.ds.data.episodeTypeId = this.episodeTypeId ? Number(this.episodeTypeId) : null;
    this.ds.search();
  }

  refresh() {
    this.ds.data = {
      startDate: null,
      endDate: null,
      episodeCategoryId: null,
      episodeTypeId: null,
      OwnerID: null,
      resolved: false,
      sortColumn: "Created",
      sortDirection: "DESC",
      pageNumber: 1,
      pageSize: 30
    };
    this.reset();
    this.resolved = false;
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

  episode(id: number = undefined, TypeId: string = "1", note: string = null) {

    this.ds.episodeForm.reset();
    this.ds.episodeForm.patchValue({
      claimId: null,
      episodeText: note,
      pharmacyNabp: null,
      episodeTypeId: TypeId
    });
    this.episodeSwal.show().then((r) => {

    })
  }


}
