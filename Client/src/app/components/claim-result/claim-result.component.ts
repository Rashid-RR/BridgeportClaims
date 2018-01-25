import { Component, AfterViewInit, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ClaimManager } from '../../services/claim-manager';
import { EventsService } from '../../services/events-service';
import { AfterContentInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { HttpService } from 'app/services/services.barrel';
import { DatePipe } from '@angular/common';
import { ToastsManager } from 'ng2-toastr/src/toast-manager';

declare var $: any;

@Component({
  selector: 'app-claim-result',
  templateUrl: './claim-result.component.html',
  styleUrls: ['./claim-result.component.css']
})
export class ClaimResultComponent implements OnInit, AfterViewInit {

  @Input() expand: Function;
  @Input() minimize: Function;
  editing = false;
  form: FormGroup;
  payorId = '';
  adjustorId = '';
  payor: any;
  adjustor: any;
  lastForm: any;
  constructor(
    public claimManager: ClaimManager,
    private formBuilder: FormBuilder,
    private router: Router,
    private toast: ToastsManager,
    private dp: DatePipe,
    private http: HttpService,
    private events: EventsService
  ) {
    this.form = this.formBuilder.group({
      claimId: [''],
      dateOfBirth: [undefined], // NULL
      genderId: [undefined],
      claimFlex2Id: [undefined],
      payorId: [undefined],
      adjustorId: [undefined],
      adjustorPhone: [undefined],
      dateOfInjury: [undefined],
      adjustorFax: [undefined], // NULL
      address1: [undefined], // NULL
      address2: [undefined], // NULL
      city: [undefined], // NULL
      stateId: [undefined], // NULL
    });
  }
  get payorAutoComplete(): string {
    return this.http.baseUrl + '/payors/search/?searchText=:keyword';
  }
  get adjustorAutoComplete(): string {
    return this.http.baseUrl + '/adjustors/search/?searchText=:keyword';
  }
  payorSelected($event) {
    if (this.payorId && $event.payorId) {
      this.form.patchValue({ payorId: $event.payorId });
      this.payor = $event;
    }
  }
  adjustorSelected($event) {
    if (this.adjustorId && $event.adjustorId) {
      this.form.patchValue({ adjustorId: $event.adjustorId });
      this.adjustor = $event;
    }
  }

  ngOnInit() {
    const claimsLength = this.claimManager.claimsData;
    this.editing = false;
    this.claimManager.onClaimIdChanged.subscribe(res => {
      this.editing = false;
      this.payorId = '';
      this.adjustorId = '';
      this.payor = undefined;
      this.adjustor = undefined;
    });
  }
  edit() {
    this.editing = true;
    this.payorId = '';
    this.adjustorId = '';
    this.payor = undefined;
    this.adjustor = undefined;
    let dateOfBirth = this.formatDate(this.claimManager.selectedClaim.dateOfBirth as any);
    let injuryDate = this.formatDate(this.claimManager.selectedClaim.injuryDate as any);
    console.log(this.claimManager.selectedClaim.stateId, this.claimManager.selectedClaim.stateAbbreviation)
    this.form.patchValue({
      claimId: this.claimManager.selectedClaim.claimId,
      dateOfBirth: dateOfBirth, // NULL
      genderId: this.claimManager.selectedClaim.genderId,
      payorId: this.claimManager.selectedClaim.payorId,
      adjustorId: this.claimManager.selectedClaim.adjustorId,
      adjustorPhone: this.claimManager.selectedClaim.adjustorPhoneNumber,
      dateOfInjury: injuryDate,
      claimFlex2Id: this.claimManager.selectedClaim.claimFlex2Id,
      adjustorFax: this.claimManager.selectedClaim.adjustorFaxNumber, // NULL
      address1: this.claimManager.selectedClaim.address1, // NULL
      address2: this.claimManager.selectedClaim.address2, // NULL
      city: this.claimManager.selectedClaim.city, // NULL
      stateId: this.claimManager.selectedClaim.stateId,
    });
    this.lastForm = {
      claimId: this.claimManager.selectedClaim.claimId,
      dateOfBirth: dateOfBirth, // NULL
      genderId: this.claimManager.selectedClaim.genderId,
      payorId: this.claimManager.selectedClaim.payorId,
      adjustorId: this.claimManager.selectedClaim.adjustorId,
      adjustorPhone: this.claimManager.selectedClaim.adjustorPhoneNumber,
      dateOfInjury: injuryDate,
      claimFlex2Id: this.claimManager.selectedClaim.claimFlex2Id,
      adjustorFax: this.claimManager.selectedClaim.adjustorFaxNumber, // NULL
      address1: this.claimManager.selectedClaim.address1, // NULL
      address2: this.claimManager.selectedClaim.address2, // NULL
      city: this.claimManager.selectedClaim.city, // NULL
      stateId: this.claimManager.selectedClaim.stateId,
    };

    setTimeout(() => {
      this.payorId = this.claimManager.selectedClaim.carrier as string;
      this.adjustorId = this.claimManager.selectedClaim.adjustor as string;
      $('#dateOfBirth').datepicker({
        autoclose: true
      });
      $('#dateOfInjury').datepicker({
        autoclose: true
      });
      $('[data-mask]').inputmask();
    }, 1000);
  }
  ngAfterViewInit() {

  }

  view(claimID: Number) {
    this.claimManager.getClaimsDataById(claimID);
    this.events.broadcast('minimize', []);
    // this.minimize();
  }

  formatDate(input: String) {
    if (!input) return null;
    if (input.indexOf("-") > -1) {
      let date = input.split("T");
      let d = date[0].split("-");
      return d[1] + "/" + d[2] + "/" + d[0];
    } else {
      return input;
    }
  }
  save() {
    let key: any;
    this.form.value.adjustorPhone = $('#adjustorPhone').val() || '';
    this.form.value.adjustorFax = $('#adjustorFax').val() || '';
    const adjustorPhone = this.form.value.adjustorPhone.replace(/[\D]/g, ''),
      adjustorFax = this.form.value.adjustorFax.replace(/[\D]/g, '');
    this.form.value.adjustorPhone = adjustorPhone
    this.form.value.adjustorFax = adjustorFax;
    let dob = $('#dateOfBirth').val() + "";
    let doi = $('#dateOfInjury').val() + "";
    this.form.value.dateOfBirth = dob
    this.form.value.dateOfInjury = doi
    for (key in this.form.value) {
      if (this.form.value.hasOwnProperty(key)) {
        this.form.value[key] = this.form.value[key] == "" || (this.form.value[key] && String(this.form.value[key]).trim() === "") ? null : this.form.value[key];
      }
    }
    if (this.form.value.claimId == this.lastForm.claimId && this.form.value.dateOfBirth == this.lastForm.dateOfBirth && this.form.value.genderId == this.lastForm.genderId && this.form.value.payorId == this.lastForm.payorId && this.form.value.adjustorId == this.lastForm.adjustorId && this.form.value.adjustorPhone == this.lastForm.adjustorPhone && this.form.value.dateOfInjury == this.lastForm.dateOfInjury && this.form.value.claimFlex2Id == this.lastForm.claimFlex2Id && this.form.value.adjustorFax == this.lastForm.adjustorFax && this.form.value.address1 == this.lastForm.address1 && this.form.value.address2 == this.lastForm.address2 && this.form.value.city == this.lastForm.city && this.form.value.stateId == this.lastForm.stateId) {
      this.toast.warning('No changes were made.', 'Not saved');
    } else if (!this.form.controls['payorId'].value) { //Check for the required payorId
      this.toast.warning('Please link a Carrier');
    } else if (!this.form.controls['genderId'].value) { // Check for the required payorId
      this.toast.warning('Please select a Gender');
    } else {
      let form: any = {};
      form.claimId = this.form.value.claimId;
      //check nullable values 
      form.claimFlex2Id = this.form.value.claimFlex2Id != this.lastForm.claimFlex2Id ? Number(this.form.value.claimFlex2Id) : undefined;
      form.payorId = this.form.value.payorId != this.lastForm.payorId ? Number(this.form.value.payorId) : undefined;
      form.genderId = this.form.value.genderId != this.lastForm.genderId ? Number(this.form.value.genderId) : undefined;
      form.stateId = this.form.value.stateId != this.lastForm.stateId ? Number(this.form.value.stateId) : undefined;
      form.dateOfBirth = dob != this.lastForm.dateOfBirth ? this.form.value.dateOfBirth : undefined, // NULL  
        form.dateOfInjury = this.lastForm.dateOfInjury != this.form.value.dateOfInjury ? this.form.value.dateOfInjury : undefined, // NULL  
        form.address1 = this.form.value.address1 != this.lastForm.address1 ? this.form.value.address1 : undefined;
      form.address2 = this.form.value.address2 != this.lastForm.address2 ? this.form.value.address2 : undefined;
      form.adjustorId = this.adjustor && this.adjustor.adjustorName != this.lastForm.adjustor ? Number(this.form.value.adjustorId) : undefined;
      form.adjustorPhone = this.form.value.adjustorPhone != this.lastForm.adjustorPhone ? this.form.value.adjustorPhone : undefined;
      form.adjustorFax = this.form.value.adjustorFax != this.lastForm.adjustorFax ? this.form.value.adjustorFax : undefined;
      form.city = this.form.value.city != this.lastForm.city ? this.form.value.city : undefined;
      // console.log(form);
      this.claimManager.loading = true;
      this.http.editClaim(form).map(r => r.json()).subscribe(res => {
        this.claimManager.loading = false;
        this.editing = false;
        this.toast.success(res.message);
        this.claimManager.selectedClaim.adjustor = this.adjustorId;
        if (form.payorId) {
          this.claimManager.selectedClaim.carrier = this.payorId;
          this.claimManager.selectedClaim.payorId = form.payorId;
        }
        if (form.adjustorId) {
          this.claimManager.selectedClaim.adjustorId = form.adjustorId;
          this.claimManager.selectedClaim.adjustor = this.adjustorId;
        }
        if (form.adjustorPhone) {
          this.claimManager.selectedClaim.adjustorPhoneNumber = form.adjustorPhone;
        }
        if (form.adjustorFax) {
          this.claimManager.selectedClaim.adjustorFaxNumber = form.adjustorFax;
        }
        if (form.dateOfInjury) {
          this.claimManager.selectedClaim.injuryDate = form.dateOfInjury;
        }
        if (form.dateOfBirth) {
          this.claimManager.selectedClaim.dateOfBirth = form.dateOfBirth;
        }
        if (form.city) {
          this.claimManager.selectedClaim.city = form.city;
        }
        if (form.address1) {
          this.claimManager.selectedClaim.address1 = form.address1;
        }
        if (form.address2) {
          this.claimManager.selectedClaim.address2 = form.address2;
        }
        if (form.claimFlex2Id) {
          const newFlex2 = this.claimManager.selectedClaim.getFlex2.find(g => g.claimFlex2Id + '' == form.claimFlex2Id + '');
          this.claimManager.selectedClaim.flex2 = newFlex2.flex2;
          this.claimManager.selectedClaim.claimFlex2Id = form.claimFlex2Id;
        }
        if (form.genderId) {
          const newGender = this.claimManager.selectedClaim.genders.find(g => g.genderId + '' == form.genderId + '');
          this.claimManager.selectedClaim.gender = newGender.genderName;
          this.claimManager.selectedClaim.genderId = form.genderId;
        }
        if (form.stateId) {
          let newState = this.claimManager.selectedClaim.states.find(g => g.stateId + "" == form.stateId + "");
          this.claimManager.selectedClaim.stateAbbreviation = newState.stateName;
          this.claimManager.selectedClaim.stateId = form.stateId;
        }
      }, error => {
        this.claimManager.loading = false;
      });
    }
  }
}
