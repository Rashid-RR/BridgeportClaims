import { Component, AfterViewInit, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { ClaimManager } from "../../services/claim-manager";
import { EventsService } from "../../services/events-service";
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
  editing: boolean = false;
  form: FormGroup;
  payorId: string = '';
  adjustorId: string = '';
  payor: any;
  adjustor: any;
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
      stateId: [undefined], //NULL
    })
  }
  get payorAutoComplete(): string {
    return this.http.baseUrl + "/payors/search/?searchText=:keyword";
  }
  get adjustorAutoComplete(): string {
    return this.http.baseUrl + "/adjustors/search/?searchText=:keyword";
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
    let claimsLength = this.claimManager.claimsData;
  }
  edit() {
    this.editing = true;
    this.payorId = '';
    this.adjustorId = '';
    this.payor = undefined;
    this.adjustor = undefined;
    let flex2 = this.claimManager.selectedClaim.getFlex2.find(g => g.flex2 == this.claimManager.selectedClaim.flex2);
    let gender = this.claimManager.selectedClaim.genders.find(g => g.genderName == this.claimManager.selectedClaim.gender);
    let state = this.claimManager.selectedClaim.states.find(g => g.stateName.indexOf(this.claimManager.selectedClaim.stateAbbreviation) == 0);
    let dateOfBirth = this.dp.transform(this.claimManager.selectedClaim.dateOfBirth, "MM/dd/yyyy");
    let injuryDate = this.dp.transform(this.claimManager.selectedClaim.injuryDate, "MM/dd/yyyy");
    this.form.patchValue({
      claimId: this.claimManager.selectedClaim.claimId,
      dateOfBirth: dateOfBirth, // NULL
      genderId: gender ? gender.genderId : null,
      payorId: null,
      adjustorId: null,
      adjustorPhone: this.claimManager.selectedClaim.adjustorPhoneNumber,
      dateOfInjury: injuryDate,
      claimFlex2Id: flex2 ? flex2.claimFlex2Id : undefined,
      adjustorFax: this.claimManager.selectedClaim.adjustorFaxNumber, // NULL
      address1: this.claimManager.selectedClaim.address1, // NULL
      address2: this.claimManager.selectedClaim.address2, // NULL
      city: this.claimManager.selectedClaim.city, // NULL
      stateId: state ? state.stateId : null,
    });
    setTimeout(() => {
      this.payorId = this.claimManager.selectedClaim.carrier as string;
      this.adjustorId = this.claimManager.selectedClaim.adjustor as string;
      $('#payorId').focus()
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
    //this.minimize();
  }

  save() {
    var key;
    let currentValues = this.claimManager.selectedClaim;
    for (key in this.form.value) {
      if (this.form.value.hasOwnProperty(key)) {
        this.form.value[key] = this.form.value[key] == "" || (this.form.value[key] && String(this.form.value[key]).trim() == "") ? null : this.form.value[key];
      }
    }
    if (!this.form.controls['payorId'].value) { //Check for the required payorId
      this.toast.warning('Please link a Carrier');
    } else if (!this.form.controls['genderId'].value) { //Check for the required payorId
      this.toast.warning('Please select a Gender');
    } else {
      let flex2 = this.claimManager.selectedClaim.getFlex2.find(g => g.flex2 == this.claimManager.selectedClaim.flex2);
      let gender = currentValues.genders.find(g => g.genderName == currentValues.gender);
      let state = currentValues.states.find(g => g.stateName.indexOf(currentValues.stateAbbreviation) == 0);
      let dateOfBirth = this.dp.transform(currentValues.dateOfBirth, "MM/dd/yyyy");
      let injuryDate = this.dp.transform(currentValues.injuryDate, "MM/dd/yyyy");
      let form: any = {};
      let dob = this.dp.transform($('#dateOfBirth').val(), "MM/dd/yyyy");
      let doi = this.dp.transform($('#dateOfInjury').val(), "MM/dd/yyyy");
      this.form.value.adjustorPhone = $('#adjustorPhone').val() || '';
      this.form.value.adjustorFax = $('#adjustorFax').val()  || '';
      let adjustorPhone = this.form.value.adjustorPhone.replace( /[\D]/g,''),
      adjustorFax = this.form.value.adjustorFax.replace( /[\D]/g,'');
      form.claimId = this.form.value.claimId;
      form.payorId = this.form.value.payorId;
      form.genderId = this.form.value.genderId;
      //check nullable values 
      form.claimFlex2Id = this.form.value.claimFlex2Id != flex2.claimFlex2Id || (this.form.value.claimFlex2Id && !flex2) ? this.form.value.claimFlex2Id : undefined;
      form.stateId = this.form.value.stateId != state.stateId || (this.form.value.stateId && !state) ? this.form.value.stateId : undefined;
      form.dateOfBirth = dob != dateOfBirth ? dob : undefined, // NULL  
      form.dateOfInjury = injuryDate != doi ? doi : undefined, // NULL  
      form.address1 = this.form.value.address1 != currentValues.address1 ? this.form.value.address1 : undefined;
      form.address2 = this.form.value.address2 != currentValues.address2 ? this.form.value.address2 : undefined;
      form.adjustorId = this.adjustor && this.adjustor.adjustorName != currentValues.adjustor ? this.form.value.adjustorId : undefined;
      form.adjustorPhone = adjustorPhone != currentValues.adjustorPhoneNumber ? adjustorPhone : undefined;
      form.adjustorFax = adjustorFax != currentValues.adjustorFaxNumber ? adjustorFax : undefined;
      form.city = this.form.value.city != currentValues.city ? this.form.value.city : undefined;
      this.claimManager.loading = true;
      console.log(currentValues.adjustorPhoneNumber,form)
      this.http.editClaim(form).map(r => r.json()).subscribe(res => {
        this.claimManager.loading = false;
        this.editing = false;
        this.toast.success(res.message);
        this.claimManager.selectedClaim.adjustor = this.adjustorId;
        if (form.payorId) {
          this.claimManager.selectedClaim.carrier = this.payorId;
        }
        if (form.adjustorPhone) {
          this.claimManager.selectedClaim.adjustorPhoneNumber = form.adjustorPhone;
        }
        if (form.adjustorFax) {
          this.claimManager.selectedClaim.adjustorFaxNumber = form.adjustorFax;
        }
        if (doi) {
          this.claimManager.selectedClaim.injuryDate = new Date(doi);
        }
        if (dateOfBirth) {
          this.claimManager.selectedClaim.dateOfBirth = new Date(dateOfBirth);
        }
        if (form.city) {
          this.claimManager.selectedClaim.city = form.city;
        }
        if (form.address1) {
          this.claimManager.selectedClaim.address1 = form.address1;
        }
        if (form.claimFlex2Id) {
          let newFlex2 = this.claimManager.selectedClaim.getFlex2.find(g => g.claimFlex2Id+"" == form.claimFlex2Id+"");      
          this.claimManager.selectedClaim.flex2 = newFlex2.flex2;
        }
      }, error => {
        this.claimManager.loading = false;
      })

    }

  }
}
