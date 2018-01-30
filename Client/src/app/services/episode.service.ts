import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { EventsService } from './events-service';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { UUID } from 'angular2-uuid';
import * as Immutable from 'immutable';
import { SortColumnInfo } from "../directives/table-sort.directive";
import { EpisodesFilterPipe } from "../components/episode-results/episode-filter.pipe"
import { Episode } from 'app/interfaces/episode';
import { EpisodeNoteType } from 'app/models/episode-note-type';

@Injectable()
export class EpisodeService {

  loading: Boolean = false;
  episodes: Immutable.OrderedMap<Number, Episode> = Immutable.OrderedMap<Number, Episode>();
  data: any = {};
  owner: String;
  episodeNote: String;
  totalRowCount: number;
  episodeNoteTypes: Array<EpisodeNoteType> = []
  owners: Array<{ownerId:any,owner:string}> = []
  constructor(private http: HttpService, private formBuilder: FormBuilder,
    private epf: EpisodesFilterPipe,
    private events: EventsService, private toast: ToastsManager) {
    this.data = {
      isDefaultSort: true,
      /* startDate: null,
      endDate: null, */
      OwnerID:null,
      resolved: false,
      sortColumn: "Created",
      sortDirection: "DESC",
      pageNumber: 1,
      pageSize: 30
    };
    this.http.getEpisodesNoteTypes().map(res => { return res.json() })
      .subscribe((result: Array<any>) => {
        this.episodeNoteTypes = result;
      }, err => {
        this.loading = false;
        let error = err.json();
      });
    this.http.getEpisodesOwners().map(res => { return res.json() })
      .subscribe((result: Array<any>) => {
        this.owners = result;
      }, err => {
        this.loading = false; 
        let error = err.json();
      });
  }

  refresh() {

  }
  get episodeTypes():Array<EpisodeNoteType>{
    return this.episodeNoteTypes
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

  search(next: Boolean = false, prev: Boolean = false, page: number = undefined) {
    if (!this.data) {
      this.toast.warning('Please populate at least one search field.');
    } else {
      this.loading = true;
      let data = JSON.parse(JSON.stringify(this.data)); //copy data instead of memory referencing

      if (next) {
        data.pageNumber++;
      }
      if (prev && data.pageNumber > 1) {
        data.pageNumber--;
      }
      if (page) {
        data.pageNumber = page;
      }
      this.http.episodeList(data).map(res => { return res.json(); })
        .subscribe((result: any) => {
          this.loading = false;
          this.totalRowCount = result.totalRowCount;
          this.episodes = Immutable.OrderedMap<Number, Episode>();
          result.episodeResults.forEach((episode: Episode) => {
            try {
              this.episodes = this.episodes.set(episode['episodeId'], episode);
            } catch (e) { }
          });
          if (next && result.episodeResults && result.episodeResults.length>0) {
            this.data.pageNumber++;
          }
          if (prev) {
            this.data.pageNumber--;
          }
          if (page && result.episodeResults && result.episodeResults.length>0) {
            this.data.pageNumber = page;
          }
          setTimeout(() => {
            //this.events.broadcast('payment-amountRemaining',result)
          }, 200);
        }, err => {
          this.loading = false;
          try {
            const error = err.json();
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
    return this.episodeList.length > 1 ? ((this.data.pageNumber - 1) * this.data.pageSize) + 1 : null;
  }
  get pageEnd() {
    return this.episodeList.length > 1 ? (this.data.pageSize > this.episodeList.length ? ((this.data.pageNumber - 1) * this.data.pageSize) + this.episodeList.length : (this.data.pageNumber) * this.data.pageSize) : null;
  }

  get end(): Boolean {
    return this.pageStart && this.data.pageSize > this.episodeList.length;
  }

}
