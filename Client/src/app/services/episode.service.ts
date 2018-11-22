import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { EventsService } from './events-service';
import { ToastsManager } from 'ng2-toastr';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import * as Immutable from 'immutable';
import { SortColumnInfo } from "../directives/table-sort.directive";
import { EpisodesFilterPipe } from "../components/episode-results/episode-filter.pipe"
import { Episode } from '../interfaces/episode';
import { EpisodeNoteType } from '../models/episode-note-type';
import { ArraySortPipe } from '../pipes/sort.pipe';
import swal from "sweetalert2";
import { Payor } from "../models/payor";
import { Subject } from 'rxjs/Subject';
declare var $: any;

@Injectable()
export class EpisodeService {

  loading: Boolean = false;
  episodes: Immutable.OrderedMap<Number, Episode> = Immutable.OrderedMap<Number, Episode>();
  data: any = {};
  owner: String;
  goToPage: any = '';
  episodeNote: String;
  totalRowCount: number;
  episodetoAssign:Episode;
  episodeNoteTypes: Array<EpisodeNoteType> = [];
  owners: Array<{ownerId: any, owner: string}> = [];
  episodeForm: FormGroup;
  pharmacyName = '';
  payors: Array<Payor> = [];
  pageSize = 50;
  payorListReady = new Subject<any>();

  constructor(private http: HttpService, private formBuilder: FormBuilder,
    private epf: EpisodesFilterPipe, private sortPipe: ArraySortPipe,
    private events: EventsService, private toast: ToastsManager) {
    this.data = {
      startDate: null,
      endDate: null,
      episodeCategoryId: null,
      episodeTypeId: null,
      OwnerID: null,
      resolved: false,
      archived: false,
      sortColumn: 'episodeId',
      sortDirection: 'DESC',
      pageNumber: 1,
      pageSize: 30
    };

    this.episodeForm = this.formBuilder.group({
      claimId: [null],
      rxNumber: [null],
      pharmacyNabp: [null],
      episodeText: [null, Validators.compose([Validators.minLength(5), Validators.required])],
      episodeTypeId: ['1']
    });
    this.http.getEpisodesNoteTypes()
      .subscribe((result: Array<any>) => {
        this.episodeNoteTypes = result;
      }, () => {
        this.loading = false;
      });
    this.http.getEpisodesOwners()
      .subscribe((result: Array<any>) => {
        this.owners = result;
      }, err => {
        this.loading = false;
        const error = err.error;
      });
  }
  get EpisodeNoteTypes(): Array<any> {
    return this.episodeNoteTypes;
  }
  saveEpisode() {
    const pharmacyNabp = $('#ePayorsSelection').val() || null;
    this.episodeForm.controls['pharmacyNabp'].setValue(pharmacyNabp);
    if (this.episodeForm.controls['pharmacyNabp'].value == null && this.pharmacyName) {
      this.toast.warning('Incorrect Pharmacy name, Correct it to a valid value, or delete the value and leave it blank');
    }else if (this.episodeForm.valid) {
      swal({ title: '', html: 'Saving Episode... <br/> <img src=\'assets/1.gif\'>', showConfirmButton: false }).catch(swal.noop);
      // this.episodeForm.value.episodeId = this.episodeForm.value.episodeId ? Number(this.episodeForm.value.episodeId) : null;
      this.episodeForm.value.episodeTypeId = this.episodeForm.value.episodeTypeId ? Number(this.episodeForm.value.episodeTypeId) : null;
      const form = this.episodeForm.value;
      this.http.saveEpisode(form).single().subscribe(res => {
        this.episodeForm.reset();
        this.closeModal();
        this.toast.success(res.message);
        this.search();
      }, () => {
        this.events.broadcast('edit-episode', { episodeId: this.episodeForm.value.episodeId, type: this.episodeForm.value.episodeTypeId, episodeNote: this.episodeForm.value.episodeText });
      });
    } else {
      if (this.episodeForm.controls['episodeText'].errors && this.episodeForm.controls['episodeText'].errors.required) {
        this.toast.warning('Episode Note is required');
      } else if (this.episodeForm.controls['episodeText'].errors && this.episodeForm.controls['episodeText'].errors.minlength) {
        this.toast.warning('Episode Note must be at least 5 characters');
      } else if (this.episodeForm.controls['pharmacyNabp'].errors && this.episodeForm.controls['pharmacyNabp'].errors.required) {
        this.toast.warning('Pharmacy Name is required');
      }else {

      }
    }

  }

  refresh() {

  }
  get episodeTypes(): Array<EpisodeNoteType>{
    return this.episodeNoteTypes;
  }
  get totalPages() {
    return this.totalRowCount ? Math.ceil(this.totalRowCount / this.data.pageSize) : null;
  }
  get episodeList(): Array<Episode> {
    return this.episodes.toArray();
  }
  get episodeFilterSize(): Array<any> {
    return this.epf.transform(this.episodes.toArray() as any, this.episodeNote, this.owner);
  }
  onSortColumn(info: SortColumnInfo) {
    this.data.isDefaultSort = false;
    this.data.sortColumn = info.column;
    this.data.sortDirection = info.dir;
    this.search();
  }
  closeModal() {
    swal.clickCancel();
  }
  search(next: Boolean = false, prev: Boolean = false, page: number = undefined) {
    if (!this.data) {
      this.toast.warning('Please populate at least one search field.');
    } else {
      this.loading = true;
      const data = JSON.parse(JSON.stringify(this.data)); // copy data instead of memory referencing

      if (next) {
        data.pageNumber++;
      }
      if (prev && data.pageNumber > 1) {
        data.pageNumber--;
      }
      if (page) {
        data.pageNumber = page;
      }
      this.http.episodeList(data)
        .subscribe((result: any) => {
          this.loading = false;
          this.totalRowCount = result.totalRowCount;
          if (this.data.sortColumn === 'fileUrl'){
            result.episodeResults = this.sortPipe.transform(result.episodeResults, this.data.sortColumn, this.data.sortDirection);
          }
          this.episodes = Immutable.OrderedMap<Number, Episode>();
          result.episodeResults.forEach((episode: Episode) => {
            try {
              this.episodes = this.episodes.set(episode['episodeId'], episode);
            } catch (e) { }
          });
          if (next && result.episodeResults && result.episodeResults.length > 0) {
            this.data.pageNumber++;
          }
          if (prev) {
            this.data.pageNumber--;
          }
          if (page && result.episodeResults && result.episodeResults.length > 0) {
            this.data.pageNumber = page;
          }
          if (!prev && !next && ! page){
            this.data.pageNumber = this.totalRowCount === 0  ? null : 1;
            this.goToPage = this.totalRowCount === 0  ? null : this.data.pageNumber;
          }
          setTimeout(() => {
            // this.events.broadcast('payment-amountRemaining',result)
          }, 200);
        }, err => {
          this.loading = false;
          try {
            const error = err.error;
          } catch (e) { }
        }, () => {
          this.events.broadcast('episode-list-updated');
        });
    }
  }
  get pages(): Array<any> {
    return new Array(this.data.pageNumber);
  }
  get pageStart() {
    return this.totalRowCount === 0 ? 0 : (this.totalRowCount > 0 && this.episodeList.length > 1 ? ((this.data.pageNumber - 1) * this.data.pageSize) + 1 : null);
  }
  get pageEnd() {
    return this.episodeList.length > 1 ? (this.data.pageSize > this.episodeList.length ? ((this.data.pageNumber - 1) * this.data.pageSize) + this.episodeList.length : (this.data.pageNumber) * this.data.pageSize) : null;
  }

  get end(): Boolean {
    return this.pageStart && this.data.pageSize > this.episodeList.length;
  }

}
