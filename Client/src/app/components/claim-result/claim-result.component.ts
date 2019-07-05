import {Component, ViewChild, AfterViewInit, OnInit, Input} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ClaimManager} from '../../services/claim-manager';
import {EventsService} from '../../services/events-service';
import {Claim} from '../../models/claim';
import {HttpService, ComparisonClaim} from '../../services/services.barrel';
import {ProfileManager} from '../../services/profile-manager';
import {DatePipe} from '@angular/common';
import {SwalComponent} from '@sweetalert2/ngx-sweetalert2';
import {ToastrService} from 'ngx-toastr';
import swal from 'sweetalert2';
import {MatDialog} from '@angular/material';
import {CarrierModalComponent} from '../carrier-modal/carrier-modal.component';
import {AdjustorModalComponent} from '../adjustor-modal/adjustor-modal.component';
import {AttorneyModalComponent} from '../attorney-modal/attorney-modal.component';

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
  attorneyId = '';
  attorney: any;
  payor: any;
  adjustor: any;
  lastForm: any;
  activeToast: number;
  selectMultiple: Boolean = false;
  lastSelectedIndex: number;
  mergedClaim: any = {} as any;
  @ViewChild('claimActionSwal') private claimSwal: SwalComponent;
  comparisonClaims: ComparisonClaim = {} as ComparisonClaim;

  constructor(
    public claimManager: ClaimManager,
    private formBuilder: FormBuilder,
    private profileManager: ProfileManager,
    private toast: ToastrService,
    private dp: DatePipe,
    public dialog: MatDialog,
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
      attorneyId: [undefined],
      dateOfInjury: [undefined],
      address1: [undefined], // NULL
      address2: [undefined], // NULL
      city: [undefined], // NULL
      stateId: [undefined], // NULL
      postalCode: [undefined] // NULL
    });
  }

  get auth() {
    const user = localStorage.getItem('user');
    try {
      const us = JSON.parse(user);
      return `Bearer ${us.access_token}`;
    } catch (error) {
    }
    return null;
  }

  get adminOnly(): Boolean {
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array) &&
      this.profileManager.profile.roles.indexOf('Admin') > -1);
  }

  get payorAutoComplete(): string {
    return this.http.baseUrl + '/payors/search/?searchText=:keyword';
  }

  merge(value: any, $event, index: string) {
    if (value === this.mergedClaim[index] && !$event.checked) {
      this.mergedClaim[index] = undefined;
    } else {
      this.mergedClaim[index] = $event.checked ? value : this.mergedClaim[index];
    }
  }

  closeModal() {
    this.claimManager.deselectAll();
    this.mergedClaim = {};
    try {
      swal.clickCancel();
    } catch (e) {
    }
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
    // form.PersonCode = this.claimManager.selectedClaims[0].personCode;
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
      delete form['PayorId'];
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
        .subscribe(r => {
          this.activeToast = this.toast.success('Claim successfully merged').toastId;
          this.closeModal();
          this.events.broadcast('refresh-claims', []);
        }, () => {
          this.claimManager.loading = false;
        });
    } else {
      this.toast.warning('Please choose a value for every field for these claims in order to save the merge...');
    }
  }

  showModal() {
    this.claimManager.loading = true;
    this.http.getComparisonClaims({leftClaimId: this.claimManager.selectedClaims[0].claimId, rightClaimId: this.claimManager.selectedClaims[1].claimId})
      .subscribe(r => {
        this.claimManager.loading = false;
        this.comparisonClaims = Array.isArray(r) ? r[0] : r;
        Object.keys(this.comparisonClaims).forEach(k => {
          if (k.indexOf('left') > -1) {
            const right = k.replace('left', 'right');
            const id = k.replace('left', '');
            // id = id.substr(0,1).toLowerCase()+id.substring(1);
            if (this.comparisonClaims[k] === this.comparisonClaims[right]) {
              this.mergedClaim[id] = this.comparisonClaims[k];
            }
          }
        });
        this.claimSwal.show().then((r) => {
          // this.mergedClaim={};
        });
      }, err => {
        this.claimManager.loading = false;
      });
  }

  select(claim: Claim, $event, index: number) {
    if ($event.checked) {
      if (this.claimManager.selectedClaims.length === 2 && $event.checked) {
        this.showModal();
        return;
      }
      claim.selected = $event.checked;
      if (this.selectMultiple) {
        for (let i = this.lastSelectedIndex; i < index; i++) {
          try {
            const c = $('#row' + i).attr('claim');
            const claimParsed = JSON.parse(c);
            const data = this.claimManager.selectedClaims.find(cl => cl.claimId === claimParsed.claimId);
            data.selected = true;
          } catch (e) {
          }
        }
      }
      this.lastSelectedIndex = index;
      this.claimManager.comparisonClaims = this.claimManager.comparisonClaims.set(claim.claimId, claim);

      if (this.claimManager.selectedClaims.length === 1) {
        this.activeToast = this.toast.info('You have selected a Claim to compare. Please select another', '',
          {timeOut: 15000, closeButton: true}).toastId;
      } else if (this.claimManager.selectedClaims.length === 2) {
        this.activeToast = null;
        $('.toast.toast-info').hide();
        this.showModal();
      }
    } else {
      this.toast.warning('You you have already chosen this claim for comparison.');
      $event.checked = true;
    }
  }

  get adjustorAutoComplete(): string {
    return this.http.baseUrl + '/adjustors/search/?searchText=:keyword';
  }

  payorSelected($event) {
    if (this.payorId && $event.payorId) {
      this.form.patchValue({payorId: $event.payorId});
      this.payor = $event;
    }
  }

  adjustorSelected($event) {
    if (this.adjustorId && $event.adjustorId) {
      this.form.patchValue({adjustorId: $event.adjustorId});
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
    this.attorneyId = '';
    this.attorney = undefined;
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
      attorneyId: this.claimManager.selectedClaim.attorneyId,
      dateOfInjury: injuryDate,
      claimFlex2Id: this.claimManager.selectedClaim.claimFlex2Id,
      address1: this.claimManager.selectedClaim.address1, // NULL
      address2: this.claimManager.selectedClaim.address2, // NULL
      city: this.claimManager.selectedClaim.city, // NULL
      stateId: this.claimManager.selectedClaim.stateId,
      postalCode: this.claimManager.selectedClaim.postalCode
    });
    this.lastForm = {
      claimId: this.claimManager.selectedClaim.claimId,
      dateOfBirth: dateOfBirth, // NULL
      genderId: this.claimManager.selectedClaim.genderId,
      payorId: this.claimManager.selectedClaim.payorId,
      adjustorId: this.claimManager.selectedClaim.adjustorId,
      adjustor: this.claimManager.selectedClaim.adjustor,
      attorneyId: this.claimManager.selectedClaim.attorneyId,
      attorney: this.claimManager.selectedClaim.attorney,
      dateOfInjury: injuryDate,
      claimFlex2Id: this.claimManager.selectedClaim.claimFlex2Id,
      address1: this.claimManager.selectedClaim.address1, // NULL
      address2: this.claimManager.selectedClaim.address2, // NULL
      city: this.claimManager.selectedClaim.city, // NULL
      stateId: this.claimManager.selectedClaim.stateId,
      postalCode: this.claimManager.selectedClaim.postalCode
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
    const editAttorneySelection = $('#editAttorneySelection').select2({
      ajax: {
        headers: {'Authorization': this.auth},
        url: function (params) {
          return '/api/attorney/attorney-names/?attorneyName=' + (params.term || '');
        },
        type: 'POST',
        processResults:  (data) => {
          const content = data.filter(d => d.attorneyId !== (this.claimManager.selectedClaim.attorneyId || 'null'));
          content.forEach(d => {
            d.id = d.attorneyId,
            d.text = d.attorneyName;
          });
          content.unshift({
            id: 'null',
            text: '-- No Attorney --'
          });
          if (this.claimManager.selectedClaim.attorneyId) {
            content.unshift({
              id: this.claimManager.selectedClaim.attorneyId,
              text: this.claimManager.selectedClaim.attorney
            });
          }
          return {
            results: (content || [])
          };
        }
      }
    }).on('change', (e) => {
      const data = $('#editAttorneySelection option:selected').val();
      this.attorneyId = $('#editAttorneySelection option:selected').text();
      const val = data === 'null' ? null : data;
      this.form.controls['attorneyId'].setValue(val);
    });
    const attorneyOption = new Option(this.claimManager.selectedClaim.attorney as string, this.claimManager.selectedClaim.attorneyId as any, true, true);
    if (this.claimManager.selectedClaim.attorneyId) {
      editAttorneySelection.append(attorneyOption).trigger('change');
    }

    const editAdjustorSelection = $('#editAdjustorSelection').select2({
      ajax: {
        headers: {'Authorization': this.auth},
        url: function (params) {
          return '/api/adjustors/adjustor-names/?adjustorName=' + (params.term || '');
        },
        type: 'POST',
        processResults:  (data) => {
          const content = data.filter(d => d.adjustorId !== (this.claimManager.selectedClaim.adjustorId || 'null'));
          content.forEach(d => {
            d.id = d.adjustorId,
            d.text = d.adjustorName;
          });
          content.unshift({
            id: 'null',
            text: '-- No Adjustor --'
          });
          if (this.claimManager.selectedClaim.adjustorId) {
            content.unshift({
              id: this.claimManager.selectedClaim.adjustorId,
              text: this.claimManager.selectedClaim.adjustor
            });
          }
          return {
            results: (content || [])
          };
        }
      }
    }).on('change', (e) => {
      const data = $('#editAdjustorSelection option:selected').val();
      this.adjustorId = $('#editAdjustorSelection option:selected').text();
      const val = data === 'null' ? null : data;
      this.form.controls['adjustorId'].setValue(val);
    });
    const adjustorOption = new Option(this.claimManager.selectedClaim.adjustor as string, this.claimManager.selectedClaim.adjustorId as any, true, true);
    if (this.claimManager.selectedClaim.adjustorId) {
      editAdjustorSelection.append(adjustorOption).trigger('change');
    }
    const eCarrierSelection = $('#eCarrierSelection').select2({
      ajax: {
        headers: {'Authorization': this.auth},
        url: (params) => {
          return '/api/payors/search/?searchText=' + (params.term || '');
        },
        type: 'POST',
        processResults: (data) => {
          const content = data.filter(d => d.adjustorId !== (this.claimManager.selectedClaim.adjustorId || 'null'));
          content.forEach(d => {
            d.id = d.payorId,
              d.text = d.groupName;
          });

          content.unshift({
            id: 'null',
            text: '-- No Carrier --'
          });
          if (this.claimManager.selectedClaim.payorId) {
            content.unshift({
              id: this.claimManager.selectedClaim.payorId,
              text: this.claimManager.selectedClaim.carrier
            });
          }
          return {
            results: (content || [])
          };
        }
      }
    }).on('change', () => {
      const data = $('#eCarrierSelection option:selected').val();
      this.payorId = $('#eCarrierSelection option:selected').text();
      const val = data === 'null' ? null : data;
      this.form.controls['payorId'].setValue(val);
    });
    if (this.claimManager.selectedClaim.carrier) {
    const payorOption = new Option(this.claimManager.selectedClaim.carrier as string, this.claimManager.selectedClaim.payorId as any, true, true);
    if (this.claimManager.selectedClaim.payorId) {
      eCarrierSelection.append(payorOption).trigger('change');
    }
    }
  }

  view(claimID: number) {
    this.claimManager.getClaimsDataById(claimID);
    this.events.broadcast('minimize', []);
    // this.minimize();
  }

  formatDate(input: String) {
    if (!input) {
      return null;
    }
    if (input.indexOf('-') > -1) {
      const date = input.split('T');
      const d = date[0].split('-');
      return d[1] + '/' + d[2] + '/' + d[0];
    } else {
      return input;
    }
  }

  save() {
    let key: any;
    const dob = $('#dateOfBirth').val() + '';
    const doi = $('#dateOfInjury').val() + '';
    this.form.value.dateOfBirth = dob;
    this.form.value.dateOfInjury = doi;
    for (key in this.form.value) {
      if (this.form.value.hasOwnProperty(key)) {
        this.form.value[key] = this.form.value[key] === '' || (this.form.value[key] &&
          String(this.form.value[key]).trim() === '') ? null : this.form.value[key];
      }
    }
    if (this.form.value.claimId === this.lastForm.claimId && this.form.value.dateOfBirth == this.lastForm.dateOfBirth &&
      this.form.value.genderId == this.lastForm.genderId && this.form.value.payorId == this.lastForm.payorId &&
      this.form.value.adjustorId == this.lastForm.adjustorId && this.form.value.dateOfInjury == this.lastForm.dateOfInjury &&
      this.form.value.claimFlex2Id == this.lastForm.claimFlex2Id && this.form.value.address1 == this.lastForm.address1 &&
      this.form.value.address2 == this.lastForm.address2 && this.form.value.city == this.lastForm.city &&
      this.form.value.postalCode == this.lastForm.postalCode && this.form.value.postalCode == this.lastForm.postalCode &&
      this.form.value.stateId == this.lastForm.stateId && this.form.value.attorneyId == this.lastForm.attorneyId) {
      this.toast.warning('No changes were made.', 'Not saved');
    } else if (!this.form.controls['payorId'].value) { // Check for the required payorId
      this.toast.warning('Please link a Carrier');
    } else if (!this.form.controls['genderId'].value) { // Check for the required payorId
      this.toast.warning('Please select a Gender');
    } else {
      const form: any = {};
      form.claimId = this.form.value.claimId;
      form.claimFlex2Id = this.form.value.claimFlex2Id !== this.lastForm.claimFlex2Id ?
        (this.form.value.claimFlex2Id === null ? null : Number(this.form.value.claimFlex2Id)) : undefined;
      form.payorId = this.form.value.payorId !== this.lastForm.payorId ? (this.form.value.payorId == null ? null : Number(this.form.value.payorId)) : undefined;
      form.genderId = this.form.value.genderId !== this.lastForm.genderId ? Number(this.form.value.genderId) : undefined;
      form.stateId = this.form.value.stateId !== this.lastForm.stateId ?
        (this.form.value.stateId === null ? null : Number(this.form.value.stateId)) : undefined;
      form.dateOfBirth = dob !== this.lastForm.dateOfBirth ? this.form.value.dateOfBirth : undefined, // NULL
        form.dateOfInjury = this.lastForm.dateOfInjury !== this.form.value.dateOfInjury ? this.form.value.dateOfInjury : undefined, // NULL
        form.address1 = this.form.value.address1 !== this.lastForm.address1 ? this.form.value.address1 : undefined;
      form.address2 = this.form.value.address2 !== this.lastForm.address2 ? this.form.value.address2 : undefined;
      form.postalCode = this.form.value.postalCode !== this.lastForm.postalCode ? this.form.value.postalCode : undefined;
      form.adjustorId = this.form.value.adjustorId !== this.lastForm.adjustorId ?
        (this.form.value.adjustorId === null ? null : Number(this.form.value.adjustorId)) : undefined;
      form.attorneyId = this.form.value.attorneyId !== this.lastForm.attorneyId ?
        (this.form.value.attorneyId === null ? null : Number(this.form.value.attorneyId)) : undefined;
      form.city = this.form.value.city !== this.lastForm.city ? this.form.value.city : undefined;
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
        if (form.attorneyId || form.attorneyId === null) {
          this.claimManager.selectedClaim.attorneyId = form.attorneyId;
          this.claimManager.selectedClaim.attorney = form.attorneyId === null ? null : this.attorneyId;
        }
        if (form.adjustorId || form.adjustorId === null) {
          this.claimManager.selectedClaim.adjustorId = form.adjustorId;
          this.claimManager.selectedClaim.adjustor = form.adjustorId === null ? null : this.adjustorId;
        }
        if (form.dateOfInjury) {
          this.claimManager.selectedClaim.injuryDate = form.dateOfInjury;
        }
        // is the user nulls out the date of injury, remove it from the screen after saving.
        if (this.form.value.dateOfInjury == null) {
          this.claimManager.selectedClaim.injuryDate = null;
        }
        // non-nullable field so don't have to worry abouot removing it from the screen if the user removes it.
        if (form.dateOfBirth) {
          this.claimManager.selectedClaim.dateOfBirth = form.dateOfBirth;
        }
        if (form.city) {
          this.claimManager.selectedClaim.city = form.city;
        }
        // is the user nulls out the city, remove it from the screen after saving.
        if (this.form.value.city == null) {
          this.claimManager.selectedClaim.city = null;
        }
        if (form.address1) {
          this.claimManager.selectedClaim.address1 = form.address1;
        }
        if (this.form.value.address1 == null) {
          this.claimManager.selectedClaim.address1 = null;
        }
        if (form.address2) {
          this.claimManager.selectedClaim.address2 = form.address2;
        }
        if (this.form.value.address2 == null) {
          this.claimManager.selectedClaim.address2 = null;
        }
        if (form.postalCode) {
          this.claimManager.selectedClaim.postalCode = form.postalCode;
        }
        if (this.form.value.postalCode == null) {
          this.claimManager.selectedClaim.postalCode = null;
        }
        if (form.claimFlex2Id || form.claimFlex2Id === null) {
          const newFlex2 = this.claimManager.selectedClaim.getFlex2.find(g => g.claimFlex2Id + '' === form.claimFlex2Id + '');
          this.claimManager.selectedClaim.flex2 = newFlex2 ? newFlex2.flex2 : null;
          this.claimManager.selectedClaim.claimFlex2Id = form.claimFlex2Id;
        }
        if (form.genderId) {
          const newGender = this.claimManager.selectedClaim.genders.find(g => g.genderId + '' === form.genderId + '');
          this.claimManager.selectedClaim.gender = newGender ? newGender.genderName : null;
          this.claimManager.selectedClaim.genderId = form.genderId;
        }
        if (form.stateId || form.stateId === null) {
          const newState = this.claimManager.selectedClaim.states.find(g => g.stateId + '' === form.stateId + '');
          this.claimManager.selectedClaim.stateAbbreviation = newState ? newState.stateName : null;
          this.claimManager.selectedClaim.stateId = form.stateId;
        }
      }, () => {
        this.claimManager.loading = false;
      });
    }
  }

  openPayorDialog(): void {
    this.claimManager.loading = true;
    this.http.getPayorById(this.claimManager.claimsData[0].payorId).subscribe(data => {
      this.claimManager.payorData = data;
      this.claimManager.loading = false;
      this.dialog.open(CarrierModalComponent, {
        width: '900px',
      });
    }, error => {
      this.claimManager.loading = false;
    });
  }

  openAdjustorDialog(): void {
    this.claimManager.loading = true;
    this.http.getAdjustorById(this.claimManager.claimsData[0].adjustorId).subscribe(data => {
      this.claimManager.adjustorData = data;
      this.claimManager.loading = false;
      this.dialog.open(AdjustorModalComponent, {
        width: '900px',
      });
    }, error => {
      this.claimManager.loading = false;
    });
  }

  openAttorneyDialog(): void {
    this.claimManager.loading = true;
    this.http.getAttorneyById(this.claimManager.claimsData[0].attorneyId).subscribe(data => {
      this.claimManager.attorneyData = data;
      this.claimManager.loading = false;
      this.dialog.open(AttorneyModalComponent, {
        width: '900px',
      });
    }, error => {
      this.claimManager.loading = false;
    });
  }
}


