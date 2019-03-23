import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { EventsService } from './events-service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as Immutable from 'immutable';
import { SortColumnInfo } from '../directives/table-sort.directive';
import { EpisodesFilterPipe } from '../components/episode-results/episode-filter.pipe';
import { Episode } from '../interfaces/episode';
import { EpisodeNoteType } from '../models/episode-note-type';
import { ArraySortPipe } from '../pipes/sort.pipe';
import swal from 'sweetalert2';
import { Payor } from '../models/payor';
import { Subject } from 'rxjs';
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
  episodetoAssign: Episode;
  episodeNoteTypes: Array<EpisodeNoteType> = [];
  owners: Array<{ownerId: any, owner: string}> = [];
  episodeForm: FormGroup;
  pharmacyName = '';
  payors: Array<Payor> = [];
  pageSize = 50;
  payorListReady = new Subject<any>();

  constructor(private http: HttpService, private formBuilder: FormBuilder,
    private epf: EpisodesFilterPipe, private sortPipe: ArraySortPipe,
    private events: EventsService, private toast: ToastrService) {
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
      episodeTypeId: ['1', Validators.compose([Validators.required])]
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
  saveEpisode(): void {
    const pharmacyNabp = $('#ePayorsSelection').val() || null;
    this.episodeForm.controls['pharmacyNabp'].setValue(pharmacyNabp);
    if (this.episodeForm.controls['pharmacyNabp'].value == null && this.pharmacyName) {
      this.toast.warning('Incorrect Pharmacy name, Correct it to a valid value, or delete the value and leave it blank');
    } else if (this.episodeForm.valid) {
      swal({ title: '', html: 'Saving Episode... <br/> <img src=\'assets/1.gif\'>', showConfirmButton: false }).catch(() => {});
      // this.episodeForm.value.episodeId = this.episodeForm.value.episodeId ? Number(this.episodeForm.value.episodeId) : null;
      this.episodeForm.value.episodeTypeId = this.episodeForm.value.episodeTypeId ? Number(this.episodeForm.value.episodeTypeId) : null;
      const form = this.episodeForm.value;
      this.http.saveEpisode(form).subscribe(res => {
        this.episodeForm.reset();
        this.closeModal();
        this.toast.success(res.message);
        this.search();
      }, () => {
        this.events.broadcast('edit-episode', { episodeId: this.episodeForm.value.episodeId,
          type: this.episodeForm.value.episodeTypeId, episodeNote: this.episodeForm.value.episodeText });
      });
    } else {
      if (this.episodeForm.controls['episodeTypeId'].errors && this.episodeForm.controls['episodeTypeId'].errors.required) {
        this.toast.warning('Episode type is required.');
      } else if (this.episodeForm.controls['episodeText'].errors && this.episodeForm.controls['episodeText'].errors.required) {
        this.toast.warning('Episode Note is required');
      } else if (this.episodeForm.controls['episodeText'].errors && this.episodeForm.controls['episodeText'].errors.minlength) {
        this.toast.warning('Episode Note must be at least 5 characters');
      } else if (this.episodeForm.controls['pharmacyNabp'].errors && this.episodeForm.controls['pharmacyNabp'].errors.required) {
        this.toast.warning('Pharmacy Name is required');
      } else {
      }
    }
  }

  refresh() {

  }
  get episodeTypes(): Array<EpisodeNoteType> {
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
    // tslint:disable-next-line:max-line-length
    setTimeout(function() { console.log('length: ' , $('.modal.in').length); if ( $('.modal.in').length > 0 ) { $('.modal.in').modal('hide'); } else { console.log('-'); } }, 100);
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
          if (this.data.sortColumn === 'fileUrl') {
            result.episodeResults = this.sortPipe.transform(result.episodeResults, this.data.sortColumn, this.data.sortDirection);
          }
          this.episodes = Immutable.OrderedMap<Number, Episode>();
          result.episodeResults = [{ "episodeId": 14562, "created": "2019-03-17T00:00:00.0000000", "owner": "Josephat Ogwayi", "type": "LEGAL", "pharmacy": "CAREMONT PHARMACY", "rxNumber": "234324324", "resolved": false, "noteCount": 1, "hasTree": true }, { "episodeId": 14563, "created": "2019-03-17T00:00:00.0000000", "owner": "Josephat Ogwayi", "type": "DENIAL", "pharmacy": "CENTERLINE CITY PHARMACY LLC", "rxNumber": "343432", "resolved": false, "noteCount": 1, "hasTree": true }, { "episodeId": 14564, "created": "2019-03-17T00:00:00.0000000", "owner": "Josephat Ogwayi", "type": "MAX BALANCE", "pharmacy": "KENMORE RX CENTER", "rxNumber": "3213122", "resolved": false, "noteCount": 1, "hasTree": true }, { "episodeId": 14565, "created": "2019-03-17T00:00:00.0000000", "owner": "Josephat Ogwayi", "type": "MAX BALANCE", "pharmacy": "K & K PHARMACY", "rxNumber": "4324321", "resolved": false, "noteCount": 1, "hasTree": true }, { "episodeId": 14566, "created": "2019-03-17T00:00:00.0000000", "owner": "Josephat Ogwayi", "type": "REFILL TOO SOON", "pharmacy": "MAX-WELL PHARMACY SERVICES", "rxNumber": "3423434243", "resolved": false, "noteCount": 1, "hasTree": true }, { "episodeId": 587, "created": null, "owner": "Jake Eaton", "type": "NONE", "pharmacy": null, "rxNumber": "000000304009", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 588, "created": null, "owner": null, "type": "NONE", "pharmacy": null, "rxNumber": "000000301589", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 589, "created": null, "owner": null, "type": "NONE", "pharmacy": null, "rxNumber": null, "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 590, "created": null, "owner": null, "type": "NONE", "pharmacy": null, "rxNumber": null, "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 591, "created": null, "owner": null, "type": "NONE", "pharmacy": null, "rxNumber": null, "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 592, "created": null, "owner": null, "type": "NONE", "pharmacy": null, "rxNumber": null, "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 593, "created": null, "owner": "Adam Condie", "type": "NONE", "pharmacy": null, "rxNumber": "000000301585", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 594, "created": null, "owner": "Adam Condie", "type": "NONE", "pharmacy": null, "rxNumber": "000000301585", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 595, "created": null, "owner": "Jake Eaton", "type": "NONE", "pharmacy": null, "rxNumber": "292255", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 596, "created": null, "owner": "Jake Eaton", "type": "NONE", "pharmacy": null, "rxNumber": "292255", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 597, "created": null, "owner": "Jake Eaton", "type": "NONE", "pharmacy": null, "rxNumber": "292255", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 598, "created": null, "owner": "Ashlee Gifford", "type": "NONE", "pharmacy": null, "rxNumber": "000000301284", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 599, "created": null, "owner": "Jake Eaton", "type": "NONE", "pharmacy": null, "rxNumber": "292255", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 600, "created": null, "owner": "Jake Eaton", "type": "NONE", "pharmacy": null, "rxNumber": "292255", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 601, "created": null, "owner": "Ashlee Gifford", "type": "NONE", "pharmacy": null, "rxNumber": "000000291421", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 602, "created": null, "owner": "Jake Eaton", "type": "NONE", "pharmacy": null, "rxNumber": "000000296700", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 603, "created": null, "owner": "Jake Eaton", "type": "NONE", "pharmacy": null, "rxNumber": "292255", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 604, "created": null, "owner": "Jake Eaton", "type": "NONE", "pharmacy": null, "rxNumber": "292255", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 605, "created": null, "owner": "Jake Eaton", "type": "NONE", "pharmacy": null, "rxNumber": "000000296700", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 606, "created": null, "owner": "Jake Eaton", "type": "NONE", "pharmacy": null, "rxNumber": "292255", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 607, "created": null, "owner": "Ashlee Gifford", "type": "NONE", "pharmacy": null, "rxNumber": "000000299959", "resolved": true, "noteCount": 1, "hasTree": false }, { "episodeId": 608, "created": null, "owner": "Jake Eaton", "type": "NONE", "pharmacy": null, "rxNumber": "292255", "resolved": true, "noteCount": 1, "hasTree": false }];
          this.totalRowCount = result.episodeResults.length;
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
          if (!prev && !next && ! page) {
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
