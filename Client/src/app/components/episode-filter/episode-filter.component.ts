import { Component, ViewChild, OnInit, AfterViewInit, Inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { SwalComponent, SwalPartialTargets } from '@sweetalert2/ngx-sweetalert2';
import { LocalStorageService } from 'ngx-webstorage';
import { ToastrService } from 'ngx-toastr';
import * as Immutable from 'immutable';
// Services
import { HttpService } from '../../services/http-service';
import { EpisodeService } from '../../services/episode.service';
import { Episode } from '../../interfaces/episode';
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
  open = true;
  closed: Boolean = false;
  submitted = false;
  domain: string;
  over: boolean[];
  resolved: Boolean = false;
  @ViewChild('episodeSwal') private episodeSwal: SwalComponent;
  constructor(
    private toast: ToastrService,
    private localSt: LocalStorageService,
    public ds: EpisodeService,
    private dp: DatePipe,
    public readonly swalTargets: SwalPartialTargets,
    private http: HttpService
  ) {
    this.over = new Array(1);
    this.over.fill(false);
  }
  showDecisionTreeWindow() {
    const win = window.open('#/main/decision-tree/list/episode', '_blank');
    this.http.documentWindow = this.http.documentWindow.set((new Date()).getTime(), win);
  }

  ngOnInit() {
    this.ds.loading = true;
    this.http.getEpisodesNoteTypes()
      .subscribe((result: Array<any>) => {
        this.ds.episodeNoteTypes = result;
      }, err => {
        this.ds.loading = false;
        const error = err.error;
      });
      this.localSt.observe('treeExperienceEpisode')
      .subscribe((res) => {
        if (res.value.episode) {
          const episode: Episode = res.value.episode;
          if (!episode.episodeId) {
            episode.episodeId = episode['id'];
          }
          if (!episode.episodeNoteCount) {
            episode.episodeNoteCount = episode['noteCount'];
          }
          episode.justAdded = true;
          const current = this.ds.episodes.toArray();
          current.unshift(episode);
          this.ds.episodes = Immutable.OrderedMap<Number, Episode>();
          current.forEach((ep: Episode) => {
            try {
              this.ds.episodes = this.ds.episodes.set(ep['episodeId'], ep);
            } catch (e) { }
          });
        }
        this.toast.success(res.value.message);
        this.http.closeTreeWindows();
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
    const startDate = this.dp.transform($('#startDate').val(), 'MM/dd/yyyy');
    const endDate = this.dp.transform($('#endDate').val(), 'MM/dd/yyyy');
    this.ds.data.resolved = this.resolved;
    this.ds.data.startDate = startDate || null;
    this.ds.data.endDate = endDate || null;
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
      sortColumn: 'Created',
      sortDirection: 'DESC',
      pageNumber: 1,
      pageSize: 30
    };
    this.reset();
    this.resolved = false;
    this.ds.search();
  }
  reset() {
    this.http.closeTreeWindows();
    $('#startDate').val('');
    $('#endDate').val('');
    this.ownerId = null;
    this.ds.owner = null;
    this.ds.episodeNote = null;
    this.episodeCategoryId = null;
    this.episodeTypeId = null;
  }

  episode(id: number = undefined, TypeId: string = '1', note: string = null) {

    this.ds.episodeForm.reset();
    this.ds.episodeForm.patchValue({
      claimId: null,
      episodeText: note,
      pharmacyNabp: null,
      episodeTypeId: TypeId
    });
    this.episodeSwal.show().then((r) => {

    });
  }


}
