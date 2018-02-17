import { Subject } from 'rxjs/Subject';
import { Component, Input, OnInit, NgZone } from '@angular/core';
import { HttpService } from "../../services/http-service"
import { EventsService } from "../../services/events-service"
import { EpisodeService } from "../../services/episode.service";
import { Toast, ToastsManager } from 'ng2-toastr/ng2-toastr';
import { Router } from "@angular/router";
import { DatePipe, DecimalPipe } from '@angular/common';
import { SwalComponent, SwalPartialTargets } from '@toverux/ngx-sweetalert2';
import { DialogService } from 'ng2-bootstrap-modal';

import { ConfirmComponent } from '../../components/confirm.component';
import { UserProfile } from '../../models/profile';
import { UUID } from 'angular2-uuid';

declare var $:any;

@Component({
  selector: 'app-acquire-episode',
  templateUrl: './acquire-episode.component.html',
  styleUrls: ['./acquire-episode.component.css']
})
export class AcquireEpisodeComponent implements OnInit {

  users:{id:UUID,firstName:string,lastName:string}[] = [];
  user:UUID;
  constructor(
    private router: Router,
    private dialogService: DialogService,
    public episodeService: EpisodeService,
    private http: HttpService,
    private dp: DatePipe,
    private events: EventsService,
    private toast: ToastsManager
  ) {

  }

  ngOnInit() {
    this.http.userToAssignEpisode().single().map(res=>res.json()).subscribe(users=>{
      this.users  = users;
    })
  }
  acquire() {    
    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: 'Acquire Episode',
      message: 'Are you sure you want to acquire this episode?'
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.episodeService.loading = true;
          this.http.acquireEpisode(this.episodeService.episodetoAssign.episodeId).map(r => { return r.json(); }).single().subscribe(res => {
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
  assign(){
    if(this.user){
      let u = this.users.find(us=>us.id==this.user);
      const disposable = this.dialogService.addDialog(ConfirmComponent, {
        title: 'Assign Episode',
        message: 'Are you sure you want to assign this episode to '+u.firstName+' '+u.lastName+'?'
      })
        .subscribe((isConfirmed) => {
          if (isConfirmed) {
            this.episodeService.loading = true;
            this.http.assignEpisode(this.episodeService.episodetoAssign.episodeId,u.id).map(r => { return r.json(); }).single().subscribe(res => {
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
    }else{
      this.toast.warning("You need to select a user to assign the Episode!");
    }
  }

}
