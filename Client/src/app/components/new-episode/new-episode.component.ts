import { Subject } from 'rxjs/Subject';
import { Component, EventEmitter, Input, ViewChild, OnInit, NgZone, AfterViewInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
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
  form: FormGroup;
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
  get payorAutoComplete(): string {
    return this.http.baseUrl + "/reports/pharmacy-name/?pharmacyName=:keyword";
  }
  payorSelected($event) {
    if (this.payorId && $event.payorId) {
      this.form.patchValue({ payorId: $event.payorId }); 
    }
  }
  pharmacySelected($event) {
    if (this.searchText && $event.pharmacyName) {
      this.claimManager.episodeForm.patchValue({
        pharmacyName: $event.pharmacyName
      });
      console.info(this.claimManager.episodeForm.value);
      setTimeout(() => {
        this.searchText = undefined;
        this.dropdownVisible = false
      }, 100);
    }
  }
  ngOnInit() {
  }

}
