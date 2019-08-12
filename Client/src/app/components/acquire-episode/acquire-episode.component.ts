import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UUID } from 'angular2-uuid';
import { DialogService } from 'ng2-bootstrap-modal';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { EpisodeService } from '../../services/episode.service';
import { HttpService } from '../../services/http-service';
import { ConfirmComponent } from '../confirm.component';

declare var $: any;

@Component({
  selector: 'app-acquire-episode',
  templateUrl: './acquire-episode.component.html',
  styleUrls: ['./acquire-episode.component.css']
})
export class AcquireEpisodeComponent implements OnInit {

  users: { id: UUID, firstName: string, lastName: string }[] = [];
  user: UUID;
  exactMatch = false;
  searchText = '';
  placeholder = 'Start typing to search claims...';
  dropdownVisible = false;
  showDropDown = new Subject<any>();
  form: FormGroup;
  constructor(
    private formBuilder: FormBuilder,
    private dialogService: DialogService,
    public episodeService: EpisodeService,
    private http: HttpService,
    private toast: ToastrService
  ) {
    this.form = this.formBuilder.group({
      claimNumber: [null],
      episodeId: [null, Validators.compose([Validators.required])],
      claimId: [null, Validators.compose([Validators.required])]
    });

  }

  checkMatch($event) {
    this.exactMatch = $event.target.checked;
    this.showDropDown.next($event.target.checked);
  }
  lastInput($event) {
    this.searchText = $event.target.value;
  }
  get autoCompleteClaim(): string {
    return this.http.baseUrl + '/document/claim-search/?exactMatch=' + this.exactMatch + '&searchText=:keyword';
  }
  claimSelected($event) {
    if (this.searchText && $event.claimId) {
      this.form.patchValue({
        episodeId: this.episodeService.episodetoAssign.episodeId,
        claimNumber: $event.claimNumber,
        claimId: $event.claimId
      });
      this.toast.info('Episode will be linked to ' + $event.lastName + ' ' + $event.firstName +
      ' ' + $event.claimNumber, 'Claim Link ready to save', { enableHtml: true, positionClass: 'toast-top-center' });

      setTimeout(() => {
        this.placeholder = $event.lastName + ' ' + $event.firstName + ' ~ ' + $event.claimNumber;
        this.searchText = undefined;
        this.dropdownVisible = false;
      }, 100);
    }
  }

  ngOnInit() {
    this.http.userToAssignEpisode().subscribe(users => {
      this.users = users;
    });
  }
  acquire() {
    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: 'Acquire Episode',
      message: 'Are you sure you want to acquire this episode?'
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.episodeService.loading = true;
          this.http.acquireEpisode(this.episodeService.episodetoAssign.episodeId).subscribe(res => {
            this.toast.success(res.message);
            this.episodeService.loading = false;
            this.episodeService.episodetoAssign.owner = res.owner;
            this.episodeService.closeModal();
          }, error => {
            this.toast.error(error.message);
            this.episodeService.loading = false;
          });
        }
      });
  }
  submitLink() {
    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: 'Associate Episode to Claim',
      message: 'Are you sure you want to associate this episode to Claim Number ' + this.form.value.claimNumber + '?'
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.episodeService.loading = true;
          this.http.associateEpisodeClaim(this.form.value).subscribe(res => {
            this.toast.success(res.message);
            this.episodeService.loading = false;
            this.episodeService.episodetoAssign.claimNumber = this.form.value.claimNumber;
            this.episodeService.episodetoAssign['claimId'] = this.form.value.claimId;
            this.episodeService.search();
            this.episodeService.closeModal();
          }, error => {
            this.toast.error(error.message);
            this.episodeService.loading = false;
          });
        }
      });
  }
  assign() {
    if (this.user) {
      const u = this.users.find(us => us.id === this.user);
      const disposable = this.dialogService.addDialog(ConfirmComponent, {
        title: 'Assign Episode',
        message: 'Are you sure you want to assign this episode to ' + u.firstName + ' ' + u.lastName + '?'
      })
        .subscribe((isConfirmed) => {
          if (isConfirmed) {
            this.episodeService.loading = true;
            this.http.assignEpisode(this.episodeService.episodetoAssign.episodeId, u.id).subscribe(res => {
              this.toast.success(res.message);
              this.episodeService.loading = false;
              this.episodeService.episodetoAssign.owner = res.owner;
              this.episodeService.closeModal();
            }, error => {
              this.toast.error(error.message);
              this.episodeService.loading = false;
            });
          }
        });
    } else {
      this.toast.warning('You need to select a user to assign the episode.');
    }
  }
  archive() {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Archive Episode',
      message: 'Are you sure you\'d like to archive this episode?'
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.episodeService.loading = true;
          this.http.archiveEpisode(this.episodeService.episodetoAssign.episodeId).subscribe(res => {
            this.toast.success(res.message);
            this.episodeService.loading = false;
            this.episodeService.search();
            this.episodeService.closeModal();
          }, error => {
            this.toast.error(error.message);
            this.episodeService.loading = false;
          });
        }
      });
  }
}
