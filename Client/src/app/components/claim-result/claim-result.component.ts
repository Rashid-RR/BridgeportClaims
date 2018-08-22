import { Component, ViewChild, AfterViewInit, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ClaimManager } from '../../services/claim-manager';
import { EventsService } from '../../services/events-service';
import { Claim } from '../../models/claim';
import { HttpService, ComparisonClaim } from '../../services/services.barrel';
import { ProfileManager } from "../../services/profile-manager";
import { DatePipe } from '@angular/common';
import { SwalComponent } from '@toverux/ngx-sweetalert2';
import { Toast, ToastsManager } from 'ng2-toastr';
import swal from 'sweetalert2';

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
  activeToast: Toast;
  selectMultiple: Boolean = false;
  lastSelectedIndex: number;
  mergedClaim: any = {} as any;
  @ViewChild('claimActionSwal') private claimSwal: SwalComponent;
  comparisonClaims: ComparisonClaim = {} as ComparisonClaim

  constructor(
    public claimManager: ClaimManager,
    private formBuilder: FormBuilder,
    private profileManager: ProfileManager,
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
      adjustorExtension: [undefined, Validators.maxLength(10)],
      dateOfInjury: [undefined],
      adjustorFax: [undefined], // NULL
      address1: [undefined], // NULL
      address2: [undefined], // NULL
      city: [undefined], // NULL
      stateId: [undefined], // NULL
    });
  }
  get auth() {
    let user = localStorage.getItem('user');
    try {
      let us = JSON.parse(user);
      return `Bearer ${us.access_token}`;
    } catch (error) {

    }
    return null;
  }

  get adminOnly(): Boolean {
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array) &&
      this.profileManager.profile.roles.indexOf('Admin') > -1)
  }
  get payorAutoComplete(): string {
    return this.http.baseUrl + '/payors/search/?searchText=:keyword';
  }
  merge(value: any, $event, index: string) {
    if (value == this.mergedClaim[index] && !$event.checked) {
      this.mergedClaim[index] = undefined;
    } else {
      this.mergedClaim[index] = $event.checked ? value : this.mergedClaim[index];
    }
  }
  closeModal() {
    this.claimManager.deselectAll();
    this.mergedClaim = {};
    try { swal.clickCancel(); } catch (e) { }
  }
  checked(field: string, side: string) {
    return this.mergedClaim.hasOwnProperty(field) && this.mergedClaim[field] === this.comparisonClaims[`${side}${field}`];
  }
  saveMerge() {
    const duplicate = this.claimManager.selectedClaims.find(c => c.claimId !== this.mergedClaim.ClaimId);
    const form = JSON.parse(JSON.stringify(this.mergedClaim));
    const InjuryDate = this.dp.transform(form.InjuryDate, 'MM/dd/yyyy');
    const DateOfBirth = this.dp.transform(form.DateOfBirth, 'MM/dd/yyyy');
    form.DuplicateClaimId = duplicate.claimId;
    form.ClaimFlex2Id = form.ClaimFlex2Value === this.comparisonClaims.leftClaimFlex2Value ? this.comparisonClaims.leftClaimFlex2Value : (form.ClaimFlex2Value !== undefined ? this.comparisonClaims.rightClaimFlex2Id : undefined);
    form.AdjustorId = form.AdjustorName === this.comparisonClaims.leftAdjustorName ? this.comparisonClaims.leftAdjustorId : (form.AdjustorName !== undefined ? this.comparisonClaims.rightAdjustorId : undefined);
    form.PatientId = form.PatientName === this.comparisonClaims.leftPatientName ? this.comparisonClaims.leftPatientId : (form.PatientName ? this.comparisonClaims.rightPatientId : undefined);
    form.PayorId = form.Carrier === this.comparisonClaims.leftCarrier ? this.comparisonClaims.leftPayorId : (form.Carrier ? this.comparisonClaims.rightPayorId : undefined);
    //form.PersonCode = this.claimManager.selectedClaims[0].personCode;
    if (!this.comparisonClaims.leftClaimFlex2Id && !this.comparisonClaims.rightClaimFlex2Id) {
      form['ClaimFlex2Id'] = null;
    } else if (form.ClaimFlex2Id === undefined) {
      delete form['ClaimFlex2Id'];
    }
    if (!this.comparisonClaims.leftAdjustorId && !this.comparisonClaims.rightAdjustorId) {
      form['AdjustorId'] = null;
    } else if (form.AdjustorId === undefined) {
      delete form['AdjustorId'];
    }
    if (!this.comparisonClaims.leftPatientId && !this.comparisonClaims.rightPatientId) {
      form['PatientId'] = null;
    } else if (form.PatientId === undefined) {
      delete form['PatientId'];
    }
    if (!this.comparisonClaims.leftPayorId && !this.comparisonClaims.rightPayorId) {
      form['PayorId'] = null;
    } else if (form.PayorId === undefined) {
      delete form['PayorId']
    }
    delete form.PatientName;
    delete form.AdjustorName;
    delete form.Carrier;
    delete form.ClaimFlex2Value;
    if (InjuryDate) {
      form.InjuryDate = InjuryDate;
    }
    if (DateOfBirth) {
      form.DateOfBirth = DateOfBirth;
    }
    if (form.DuplicateClaimId && form.hasOwnProperty('ClaimFlex2Id') && form.hasOwnProperty('ClaimId') &&
      form.hasOwnProperty('ClaimNumber') && form.hasOwnProperty('DateOfBirth') && form.hasOwnProperty('InjuryDate')
      && form.hasOwnProperty('AdjustorId') && form.hasOwnProperty('PatientId') && form.hasOwnProperty('PayorId')) {
      this.claimManager.loading = true;
      this.http.getMergeClaims(form)
        .single().subscribe(r => {
          this.toast.success('Claim successfully merged').then((toast: Toast) => {
            this.activeToast = toast;
          });
          this.closeModal();
          this.events.broadcast('refresh-claims', []);
        }, err => {
          this.claimManager.loading = false;
        });
    } else {
      this.toast.warning('Please choose a value for every field for these claims in order to save the merge...');
    }
  }
  showModal() {
    this.claimManager.loading = true;
    this.http.getComparisonClaims({ leftClaimId: this.claimManager.selectedClaims[0].claimId, rightClaimId: this.claimManager.selectedClaims[1].claimId })
      .single().subscribe(r => {
        this.claimManager.loading = false;
        this.comparisonClaims = Array.isArray(r) ? r[0] : r;
        Object.keys(this.comparisonClaims).forEach(k => {
          if (k.indexOf('left') > -1) {
            let right = k.replace('left', 'right');
            let id = k.replace('left', '');
            //id = id.substr(0,1).toLowerCase()+id.substring(1);
            if (this.comparisonClaims[k] == this.comparisonClaims[right]) {
              this.mergedClaim[id] = this.comparisonClaims[k];
            }
          }
        })
        this.claimSwal.show().then((r) => {
          //this.mergedClaim={};
        })
      }, err => {
        this.claimManager.loading = false;
      });
  }

  select(claim: Claim, $event, index: number) {
    if ($event.checked) {
      if (this.claimManager.selectedClaims.length == 2 && $event.checked) {
        this.showModal();
        return;
      }
      claim.selected = $event.checked;
      if (this.selectMultiple) {
        for (let i = this.lastSelectedIndex; i < index; i++) {
          try {
            let c = $('#row' + i).attr('claim');
            let claim = JSON.parse(c);
            let data = this.claimManager.selectedClaims.find(c => c.claimId == claim.claimId);
            data.selected = true;
          } catch (e) { }
        }
      }
      this.lastSelectedIndex = index;
      this.claimManager.comparisonClaims = this.claimManager.comparisonClaims.set(claim.claimId, claim);

      if (this.claimManager.selectedClaims.length == 1) {
        this.toast.info('You have selected a Claim to compare. Please select another', '',
          { toastLife: 15000, showCloseButton: true }).then((toast: Toast) => {
            this.activeToast = toast;
          })
      } else if (this.claimManager.selectedClaims.length == 2) {
        this.activeToast.timeoutId = null;
        $('.toast.toast-info').hide();
        this.showModal();
      }
    } else {
      this.toast.warning('You you have already chosen this claim for comparison.').then((toast: Toast) => {
      });
      $event.checked = true;
    }
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
    this.cancel();
    this.claimManager.onClaimIdChanged.subscribe(res => {
      this.cancel();
      this.payorId = '';
      this.adjustorId = '';
      this.payor = undefined;
      this.adjustor = undefined;
    });
  }
  cancel() {
    this.editing = false;
  }
  edit() {
    this.editing = true;
    this.payorId = '';
    this.adjustorId = '';
    this.payor = undefined;
    this.adjustor = undefined;
    const dateOfBirth = this.formatDate(this.claimManager.selectedClaim.dateOfBirth as any);
    const injuryDate = this.formatDate(this.claimManager.selectedClaim.injuryDate as any);
    this.form.patchValue({
      claimId: this.claimManager.selectedClaim.claimId,
      dateOfBirth: dateOfBirth, // NULL
      genderId: this.claimManager.selectedClaim.genderId,
      payorId: this.claimManager.selectedClaim.payorId,
      adjustorId: this.claimManager.selectedClaim.adjustorId,
      adjustorPhone: this.claimManager.selectedClaim.adjustorPhoneNumber,
      adjustorExtension: this.claimManager.selectedClaim.adjustorExtension,
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
      adjustor: this.claimManager.selectedClaim.adjustor,
      adjustorExtension: this.claimManager.selectedClaim.adjustorExtension,
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
      this.enableSelect2();
    }, 1000);
  }
  ngAfterViewInit() {

  }

  enableSelect2() {
    $('#eadjustorSelection').select2({
      initSelection: (element, callback) => {
        callback({ id: this.claimManager.selectedClaim.adjustorId, text: this.claimManager.selectedClaim.adjustor });
      },
      ajax: {
        headers: { 'Authorization': this.auth },
        url: function (params) {
          return '/api/adjustors/adjustor-names/?adjustorName=' + (params.term || '');
        },
        type: 'POST',
        processResults: function (data) {

          data.forEach(d => {
            d.id = d.adjustorId,
              d.text = d.adjustorName
          });
          data.unshift({
            id: 'null',
            text: '-- No Adjustor --'
          });
          return {
            results: (data || [])
          };
        }
      }
    }).on('change', () => {
      const data = $('#eadjustorSelection option:selected').val();
      this.adjustorId = $('#eadjustorSelection option:selected').text();
      data == 'null' ? null : data;
      this.form.controls['adjustorId'].setValue(data);
    });
    $('#eCarrierSelection').select2({
      initSelection: (element, callback) => {
        callback({ id: this.claimManager.selectedClaim.payorId, text: this.claimManager.selectedClaim.carrier });
      },
      ajax: {
        headers: { 'Authorization': this.auth },
        url: (params) => {
          return '/api/payors/search/?searchText=' + (params.term || this.claimManager.selectedClaim.carrier);
        },
        type: 'POST',
        processResults: function (data) {
          data.forEach(d => {
            d.id = d.payorId,
              d.text = d.groupName
          });
          return {
            results: (data || [])
          };
        }
      }
    }).on('change', () => {
      let data = $("#eCarrierSelection option:selected").val();
      this.payorId = $("#eCarrierSelection option:selected").text();
      this.form.controls['payorId'].setValue(data);
    })
  }

  view(claimID: Number) {
    this.claimManager.getClaimsDataById(claimID);
    this.events.broadcast('minimize', []);
    // this.minimize();
  }

  formatDate(input: String) {
    if (!input) {
      return null;
    }
    if (input.indexOf('-') > -1) {
      const date = input.split("T");
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
    const adjustorPhone = this.form.value.adjustorPhone.replace(/[\D]/g, '')
    const adjustorFax = this.form.value.adjustorFax.replace(/[\D]/g, '');
    this.form.value.adjustorPhone = adjustorPhone;
    this.form.value.adjustorFax = adjustorFax;
    const dob = $('#dateOfBirth').val() + '';
    const doi = $('#dateOfInjury').val() + '';
    this.form.value.dateOfBirth = dob;
    this.form.value.dateOfInjury = doi;
    for (key in this.form.value) {
      if (this.form.value.hasOwnProperty(key)) {
        this.form.value[key] = this.form.value[key] == '' || (this.form.value[key] &&
          String(this.form.value[key]).trim() === '') ? null : this.form.value[key];
      }
    }
    if (this.form.value.claimId == this.lastForm.claimId && this.form.value.dateOfBirth == this.lastForm.dateOfBirth &&
      this.form.value.genderId == this.lastForm.genderId && this.form.value.payorId == this.lastForm.payorId &&
      this.form.value.adjustorId == this.lastForm.adjustorId && this.form.value.adjustorExtension == this.lastForm.adjustorExtension
      && this.form.value.adjustorPhone == this.lastForm.adjustorPhone && this.form.value.dateOfInjury == this.lastForm.dateOfInjury &&
      this.form.value.claimFlex2Id == this.lastForm.claimFlex2Id && this.form.value.adjustorFax == this.lastForm.adjustorFax &&
      this.form.value.address1 == this.lastForm.address1 && this.form.value.address2 == this.lastForm.address2 &&
      this.form.value.city == this.lastForm.city && this.form.value.stateId == this.lastForm.stateId) {
      this.toast.warning('No changes were made.', 'Not saved');
    } else if (!this.form.controls['payorId'].value) { //Check for the required payorId
      this.toast.warning('Please link a Carrier');
    } else if (!this.form.controls['genderId'].value) { // Check for the required payorId
      this.toast.warning('Please select a Gender');
    } else if (this.form.controls['adjustorExtension'].errors &&
      this.form.controls['adjustorExtension'].errors.maxlength) {// Check for the required payorId
      this.toast.warning('Adjustor Extension cannot have more than 10 characters');
    } else {
      const form: any = {};
      form.claimId = this.form.value.claimId;
      // check nullable values.
      form.claimFlex2Id = this.form.value.claimFlex2Id != this.lastForm.claimFlex2Id ? (this.form.value.claimFlex2Id === null ? null : Number(this.form.value.claimFlex2Id)) : undefined;
      form.payorId = this.form.value.payorId != this.lastForm.payorId ? (this.form.value.payorId == null ? null : Number(this.form.value.payorId)) : undefined;
      form.genderId = this.form.value.genderId != this.lastForm.genderId ? Number(this.form.value.genderId) : undefined;
      form.stateId = this.form.value.stateId != this.lastForm.stateId ? (this.form.value.stateId === null ? null : Number(this.form.value.stateId)) : undefined;
      form.dateOfBirth = dob != this.lastForm.dateOfBirth ? this.form.value.dateOfBirth : undefined, // NULL  
        form.dateOfInjury = this.lastForm.dateOfInjury != this.form.value.dateOfInjury ? this.form.value.dateOfInjury : undefined, // NULL  
        form.address1 = this.form.value.address1 != this.lastForm.address1 ? this.form.value.address1 : undefined;
      form.address2 = this.form.value.address2 != this.lastForm.address2 ? this.form.value.address2 : undefined;
      form.adjustorId = this.form.value.adjustorId != this.lastForm.adjustorId ? (this.form.value.adjustorId === null ? null : Number(this.form.value.adjustorId)) : undefined;
      form.adjustorExtension = this.form.value.adjustorExtension != this.lastForm.adjustorExtension ? this.form.value.adjustorExtension : undefined;
      form.adjustorPhone = this.form.value.adjustorPhone != this.lastForm.adjustorPhone ? this.form.value.adjustorPhone : undefined;
      form.adjustorFax = this.form.value.adjustorFax != this.lastForm.adjustorFax ? this.form.value.adjustorFax : undefined;
      form.city = this.form.value.city != this.lastForm.city ? this.form.value.city : undefined;
      this.claimManager.loading = true;
      this.http.editClaim(form).subscribe(res => {
        this.claimManager.loading = false;
        this.cancel();
        this.toast.success(res.message);
        this.claimManager.selectedClaim.adjustor = this.adjustorId;
        if (form.payorId) {
          this.claimManager.selectedClaim.carrier = this.payorId;
          this.claimManager.selectedClaim.payorId = form.payorId;
        }
        if (form.adjustorId || form.adjustorId === null) {
          this.claimManager.selectedClaim.adjustorId = form.adjustorId;
          this.claimManager.selectedClaim.adjustor = form.adjustorId === null ? null : this.adjustorId;
        }
        if (form.adjustorExtension) {
          this.claimManager.selectedClaim.adjustorExtension = form.adjustorExtension;
        }
        if (form.adjustorFax) {
          this.claimManager.selectedClaim.adjustorFaxNumber = form.adjustorFax;
        }
        if (form.adjustorPhone) {
          this.claimManager.selectedClaim.adjustorPhoneNumber = form.adjustorPhone;
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
        if (form.claimFlex2Id || form.claimFlex2Id === null) {
          const newFlex2 = this.claimManager.selectedClaim.getFlex2.find(g => g.claimFlex2Id + '' == form.claimFlex2Id + '');
          this.claimManager.selectedClaim.flex2 = newFlex2 ? newFlex2.flex2 : null;
          this.claimManager.selectedClaim.claimFlex2Id = form.claimFlex2Id;
        }
        if (form.genderId) {
          const newGender = this.claimManager.selectedClaim.genders.find(g => g.genderId + '' == form.genderId + '');
          this.claimManager.selectedClaim.gender = newGender ? newGender.genderName : null;
          this.claimManager.selectedClaim.genderId = form.genderId;
        }
        if (form.stateId || form.stateId === null) {
          const newState = this.claimManager.selectedClaim.states.find(g => g.stateId + '' == form.stateId + '');
          this.claimManager.selectedClaim.stateAbbreviation = newState ? newState.stateName : null;
          this.claimManager.selectedClaim.stateId = form.stateId;
        }
      }, () => {
        this.claimManager.loading = false;
      });
    }
  }
}
