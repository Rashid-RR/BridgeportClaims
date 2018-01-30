import { Subject } from 'rxjs/Subject';
import { Component, Input, OnInit, NgZone } from '@angular/core';
import { HttpService } from "../../services/http-service"
import { EventsService } from "../../services/events-service"
import { ClaimManager } from "../../services/claim-manager";
import { Toast, ToastsManager } from 'ng2-toastr/ng2-toastr';
import { Router } from "@angular/router";
import { DatePipe, DecimalPipe } from '@angular/common';
import { SwalComponent, SwalPartialTargets } from '@toverux/ngx-sweetalert2';

declare var $: any

@Component({
  selector: 'app-new-episode',
  templateUrl: './new-episode.component.html',
  styleUrls: ['./new-episode.component.css']
})
export class NewEpisodeComponent implements OnInit {

  pharmacyName: string = '';
  payorId: any = '';
  submitted: boolean = false;
  searchText: string = '';


  dropdownVisible: boolean = false; 
  loading: boolean = false
  showDropDown = new Subject<any>();
  constructor(
    private router: Router,
    public claimManager: ClaimManager,
    private http: HttpService,
    private dp: DatePipe,
    private events: EventsService,
    private toast: ToastsManager
  ) {

  }

  get autoCompletePharmacyName(): string {
    return this.http.baseUrl + "/reports/pharmacy-name/?pharmacyName=:keyword";
  } 
  pharmacySelected($event) {
    if (this.searchText && $event.pharmacyName) {
      this.claimManager.episodeForm.patchValue({
        pharmacyName: $event.pharmacyName
      });
      setTimeout(() => {
        this.searchText = undefined;
        this.dropdownVisible = false
      }, 100);
    }
  }
  ngOnInit() {
  }

}
