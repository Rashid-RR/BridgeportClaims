import * as Immutable from 'immutable';
import { Subject } from 'rxjs/Subject';
import { Claim } from '../models/claim';
import { Prescription } from '../models/prescription';
import { ClaimNote } from '../models/claim-note';
import { PrescriptionNoteType } from '../models/prescription-note-type';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EpisodeNoteType } from '../models/episode-note-type';
import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { AuthGuard } from './auth.guard';
import { EventsService } from './events-service';
import { Router } from '@angular/router';
import { ToastsManager, Toast } from 'ng2-toastr';
import { DialogService } from 'ng2-bootstrap-modal';
import { ConfirmComponent } from '../components/confirm.component';
import { Episode } from '../interfaces/episode';
import { PhonePipe } from '../pipes/phone-pipe';
import swal from 'sweetalert2';
import { ProfileManager } from './services.barrel';

declare var $: any;

@Injectable()
export class ClaimManager {
  onClaimIdChanged = new Subject<Number>();
  dialogListener: Subject<any> = new Subject<any>();
  private claims: Immutable.OrderedMap<Number, Claim> = Immutable.OrderedMap<Number, Claim>();
  public comparisonClaims: Immutable.OrderedMap<Number, Claim> = Immutable.OrderedMap<Number, Claim>();
  private history: Array<Claim> = [];
  selected: Number;
  loading: Boolean = false;
  loadingHistory: Boolean = false;
  loadingEpisodes: Boolean = false;
  loadingPayment: Boolean = false;
  loadingPrescription: Boolean = false;
  loadingOutstanding: Boolean = false;
  loadingImage: Boolean = false;
  private notetypes: Array<any> = [];
  private prescriptionNotetypes: Array<PrescriptionNoteType> = [];
  private episodeNoteTypes: Array<EpisodeNoteType> = [];

  searchText = '';
  pharmacyName = '';
  exactMatch = false;
  // Expanded Table Properties
  isClaimsExpanded: boolean;
  isNotesExpanded: boolean;
  isEpisodesExpanded: boolean;
  isPrescriptionsExpanded: boolean;
  isScriptNotesExpanded: boolean;
  isPaymentsExpanded: boolean;
  isOutstandingExpanded: boolean;
  isImagesExpanded: boolean;
  activeToast: Toast;
  episodeForm: FormGroup;
  get selectedClaims() {
    return this.comparisonClaims.toArray();
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
  get isClient(): Boolean {
    return (this.profileManager.profile && this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
      && this.profileManager.profile.roles.indexOf('Client') > -1);
  }
  constructor(private pp: PhonePipe, private auth: AuthGuard, private http: HttpService, private events: EventsService,
    private router: Router, private toast: ToastsManager, private formBuilder: FormBuilder, private profileManager: ProfileManager,
    private dialogService: DialogService) {
      this.episodeForm = this.formBuilder.group({
        // episodeId: [undefined], // only send on episode edit
        claimId: [null, Validators.required],
        rxNumber: [null],
        pharmacyNabp: [null],
        episodeText: [null, Validators.compose([Validators.minLength(5), Validators.required])],
        episodeTypeId: ['1']
      });
      this.events.on('loadHistory', () => {
        if (!this.isClient) {this.getHistory(); }
      });
      if (!this.isClient) {
      this.getHistory();
      }
      this.events.on('clear-claims', (status: boolean) => {
        this.selected = undefined;
        this.claims = Immutable.OrderedMap<Number, Claim>();
      });
  }

  closeModal() {
    swal.clickCancel();
  }
  saveEpisode() {
    const pharmacyNabp = $('#ePayorsSelection').val() || null;
    this.episodeForm.controls['pharmacyNabp'].setValue(pharmacyNabp);
    if (this.episodeForm.controls['pharmacyNabp'].value == null && this.pharmacyName) {
      this.toast.warning('Incorrect Pharmacy name, Correct it to a valid value, or delete the value and leave it blank');
    } else if (this.episodeForm.valid) {
      swal({ title: '', html: 'Saving Episode... <br/> <img src=\'assets/1.gif\'>', showConfirmButton: false }).catch(swal.noop);
      // this.episodeForm.value.episodeId = this.episodeForm.value.episodeId ? Number(this.episodeForm.value.episodeId) : null;
      this.episodeForm.value.episodeTypeId = this.episodeForm.value.episodeTypeId ? Number(this.episodeForm.value.episodeTypeId) : null;
      const form = this.episodeForm.value;
      this.http.saveEpisode(this.episodeForm.value).single().subscribe(res => {
        const claim = this.claims.get(form.claimId);
        claim.episodes.splice(0, 0, res.episode as Episode);
        this.episodeForm.reset();
        this.closeModal();
        this.toast.success(res.message);
      }, err => {
        this.events.broadcast('edit-episode', { episodeId: this.episodeForm.value.episodeId, type: this.episodeForm.value.episodeTypeId, episodeNote: this.episodeForm.value.episodeText });
      });
    } else {
      if (this.episodeForm.controls['episodeText'].errors && this.episodeForm.controls['episodeText'].errors.required) {
        this.toast.warning('Episode Note is required');
      } else if (this.episodeForm.controls['episodeText'].errors && this.episodeForm.controls['episodeText'].errors.minlength) {
        this.toast.warning('Episode Note must be at least 5 characters');
      } else if (this.episodeForm.controls['pharmacyNabp'].errors && this.episodeForm.controls['pharmacyNabp'].errors.required) {
        this.toast.warning('Pharmacy Name is required');
      } else {

      }
    }

  }
  getHistory() {
    this.auth.isLoggedIn.single().subscribe(res => {
      if (res === true) {
        this.loadingHistory = true;
        this.http.getHistory().single().subscribe(res => {
          this.history = res as Array<Claim>;
          this.loadingHistory = false;
        }, error => {
          this.loadingHistory = false;
        });
      }
    }, error => {
      this.loadingHistory = false;
    });
  }

  showDetails(prescription: Prescription) {
    this.toast.info('Prescriber: ' + prescription.prescriber +
      '<br> Prescriber NPI: ' + prescription.prescriberNpi +
      '<br> Prescriber Phone: ' + (this.pp.transform(prescription.prescriberPhone, [])) +
      '<br> Pharmacy Name: ' + prescription.pharmacyName +
      '<br> NDC: ' + prescription.prescriptionNdc +
      '<br> Quantity: ' + prescription.quantity +
      '<br> Day Supply: ' + prescription.daySupply +
      '<br> Generic: ' + prescription.generic +
      '<br> AWP: ' + prescription.awp +
      '<br> Payable Amount: ' + prescription.payableAmount,
      null,
      { toastLife: 1210000, showCloseButton: true, enableHTML: true, positionClass: 'toast-top-center' }).then((toast: Toast) => {
        const toasts: Array<HTMLElement> = $('.toast-message');
        for (let i = 0; i < toasts.length; i++) {
          const msg = toasts[i];
          if (msg.innerHTML === toast.message) {
            msg.parentNode.parentElement.style.left = 'calc(50vw - 200px)';
            msg.parentNode.parentElement.style.position = 'fixed';
            msg.parentNode.parentElement.style.width = 'auto';
          }
        }
        this.activeToast = toast;
      });
  }

  get claimHistory(): Array<Claim> {
    return this.history;
  }

  get totalInvoiced() {
    let total = 0;
    if (this.selectedClaim) {
      const selected = this.selectedClaim.prescriptions.filter(c => c.selected === true);
      for (let i = 0; i < selected.length; i++) {
        total += selected[i].invoiceAmount;
      }
    }
    return total;
  }
  get totalOutstanding() {
    let total = 0;
    if (this.selectedClaim) {
      const selected = this.selectedClaim.prescriptions.filter(c => c.selected === true);
      for (let i = 0; i < selected.length; i++) {
        total += selected[i].outstanding;
      }
    }
    return total;
  }
  addHistory(id: Number) {
    this.http.addHistory(id).single().subscribe((res: any) => {
      this.getHistory();
    }, err => {

    });
  }
  search(data, addHistory = true) {
    // let result:any = {"claimId":26957,"name":"Lakisha Krause","claimNumber":"08264","dateOfBirth":"1983-11-08T00:00:00.0000000","gender":"Not Specified","carrier":"AAA - DE","adjustor":"Karrie83","adjustorPhoneNumber":"399843-7211","adjustorFaxNumber":null,"eligibilityTermDate":"2104-12-20T21:28:32.0000000","flex2":"PIP","address1":"82 South Oak St.","address2":"285 West White Clarendon Way","city":"Oklahoma","stateAbbreviation":null,"postalCode":"61993","patientPhoneNumber":"327135-2042","dateEntered":"1984-02-04T17:22:41.0000000","claimFlex2s":[{"claimFlex2Id":2,"flex2":"DR APT"},{"claimFlex2Id":8,"flex2":"PA"},{"claimFlex2Id":1,"flex2":"PIP"},{"claimFlex2Id":12,"flex2":"TFS"},{"claimFlex2Id":11,"flex2":"UFO"},{"claimFlex2Id":9,"flex2":"UI"},{"claimFlex2Id":10,"flex2":"UIO"},{"claimFlex2Id":3,"flex2":"VALUE1"},{"claimFlex2Id":4,"flex2":"VALUE2"},{"claimFlex2Id":5,"flex2":"VALUE3"},{"claimFlex2Id":6,"flex2":"VALUE4"},{"claimFlex2Id":7,"flex2":"VALUE5"}],"claimNotes":[{"noteType":"OTHER","noteText":"Id Tam Id in eudis esset et Id venit. Et apparens Multum in volcans pars travissimantor pladior delerium."}],"episodes":[],"prescriptionStatuses":[{"prescriptionStatusId":1,"statusName":"VALUE1"},{"prescriptionStatusId":10,"statusName":"VALUE10"},{"prescriptionStatusId":2,"statusName":"VALUE2"},{"prescriptionStatusId":3,"statusName":"VALUE3"},{"prescriptionStatusId":4,"statusName":"VALUE4"},{"prescriptionStatusId":5,"statusName":"VALUE5"},{"prescriptionStatusId":6,"statusName":"VALUE6"},{"prescriptionStatusId":7,"statusName":"VALUE7"},{"prescriptionStatusId":8,"statusName":"VALUE8"},{"prescriptionStatusId":9,"statusName":"VALUE9"}],"prescriptions":[{"prescriptionId":15851,"rxDate":"2017-12-03T00:20:04.0000000","rxNumber":"47289","labelName":"Dwayne634","billTo":"Amelia","invoiceNumber":"39731","invoiceAmount":890.4770,"amountPaid":null,"outstanding":null,"invoiceDate":"1990-03-10T00:00:00.0000000","status":"VALUE7","noteCount":0,"isReversed":true},{"prescriptionId":5229,"rxDate":"2017-09-23T21:07:41.0000000","rxNumber":"27981","labelName":"Olga94","billTo":"Amelia","invoiceNumber":"45021","invoiceAmount":927.5000,"amountPaid":494.1700,"outstanding":433.3300,"invoiceDate":"1955-02-14T00:00:00.0000000","status":null,"noteCount":3,"isReversed":false}],"prescriptionNotes":[{"claimId":26957,"prescriptionNoteId":1170,"rxDate":"2017-09-23T21:07:41.0000000","rxNumber":"27981","type":"Denial","enteredBy":"Atiq Masood","note":"fecit. in vobis funem. glavans vobis funem. quartu travissimantor gravis e et bono cognitio, rarendum trepicandor fecit, parte","noteUpdatedOn":"1967-01-31T09:22:51.0000000",hasDiaryEntry:true},{"claimId":26957,"prescriptionNoteId":3416,"rxDate":"2017-09-23T21:07:41.0000000","rxNumber":"27981","type":"Med Records","enteredBy":"Jordan Gurney","note":"new diary entry","noteUpdatedOn":"2017-10-11T23:22:22.0000000",hasDiaryEntry:true},{"claimId":26957,"prescriptionNoteId":3417,"rxDate":"2017-09-23T21:07:41.0000000","rxNumber":"27981","type":"Non Formulary","enteredBy":"Josephat Ogwayi","note":"Test this","noteUpdatedOn":"2017-10-16T18:40:20.0000000",hasDiaryEntry:false}],"acctPayables":[{"date":"1998-01-09T00:00:00.0000000","checkNumber":"83027","rxNumber":"27981","rxDate":"2017-09-23T00:00:00.0000000","checkAmount":494.1700}],"payments":[{"prescriptionPaymentId":5256,"prescriptionId":5229,"postedDate":"1998-01-09T00:00:00.0000000","checkNumber":"83027","checkAmt":494.1700,"rxNumber":"27981","rxDate":"2017-09-23T21:07:41.2513254","invoiceNumber":null,"isReversed":false}]}; //for ofline testing
    this.loading = true;
    this.http.getClaimsData(data)
      .subscribe((result: any) => {
        this.loading = false;
        this.selected = undefined;

        if (result.length < 1) {
          this.toast.info('No records were found from your search.');
        }
        if (Object.prototype.toString.call(result) === '[object Array]') {
          const res: Array<Claim> = result as Array<Claim>;
          this.claims = Immutable.OrderedMap<Number, Claim>();
          result.forEach((claim) => {
            const c = new Claim(claim.claimId, claim.claimNumber, claim.dateOfBirth, claim.injuryDate || claim.dateOfInjury, claim.gender,
              claim.carrier, claim.adjustor, claim.adjustorPhoneNumber, claim.dateEntered, claim.adjustorFaxNumber
              , claim.name, claim.firstName, claim.lastName, claim.flex2, claim.eligibilityTermDate, claim.address1, claim.address2, claim.city, claim.stateAbbreviation, claim.postalCode, claim.genders, claim.adjustorExtension);
            c.genders = claim.genders;
            c.states = claim.states;
            c.adjustorId = claim.adjustorId;
            c.isVip = claim.isVip;
            c.isMaxBalance = claim.isMaxBalance;
            c.payorId = claim.payorId;
            c.genderId = claim.patientGenderId;
            c.stateId = claim.stateId;
            c.claimFlex2Id = claim.claimFlex2Id;
            if (data.outstanding) {
              c.outstanding = result.outstanding.results;
              c.totalOutstandingAmount = result.outstanding.totalOutstandingAmount;
              c.numberOutstanding = result.outstanding.totalRows;
              c.loadingOutstanding = false;
            }
            this.claims = this.claims.set(claim.claimId, c);
          });
        } else {
          this.claims = Immutable.OrderedMap<Number, Claim>();
          const c = new Claim(result.claimId, result.claimNumber, result.date, result.injuryDate || result.dateOfInjury, result.gender,
            result.carrier, result.adjustor, result.adjustorPhoneNumber, result.dateEntered, result.adjustorFaxNumber
            , result.name, result.firstName, result.lastName, result.flex2, result.eligibilityTermDate, result.address1, result.address2, result.city, result.stateAbbreviation, result.postalCode, result.genders, result.adjustorExtension);
          c.dateOfBirth = result.dateOfBirth;
          c.adjustor = result.adjustor;
          c.adjustorPhoneNumber = result.adjustorPhoneNumber;
          c.adjustorFaxNumber = result.adjustorFaxNumber;
          c.eligibilityTermDate = result.eligibilityTermDate;
          c.dateEntered = result.dateEntered;
          c.injuryDate = result.injuryDate || result.dateOfInjury;
          c.gender = result.gender;
          c.genders = result.genders;
          c.states = result.states;
          c.isVip = result.isVip;
          c.isMaxBalance = result.isMaxBalance;
          c.adjustorId = result.adjustorId;
          c.payorId = result.payorId;
          c.genderId = result.patientGenderId;
          c.stateId = result.stateId;
          c.claimFlex2Id = result.claimFlex2Id;
          if (result.outstanding) {
            c.outstanding = result.outstanding.results;
            c.totalOutstandingAmount = result.outstanding.totalOutstandingAmount;
            c.numberOutstanding = result.outstanding.totalRows;
            c.loadingOutstanding = false;
          }
          this.claims = this.claims.set(result.claimId, c);
          const claim = this.claims.get(result.claimId);
          claim.setPrescription(result.prescriptions as Array<Prescription>);
          claim.setPayment(result.payments);
          claim.setEpisodes(result.episodes);
          claim.setClaimNotes(result.claimNotes && result.claimNotes[0] ? new ClaimNote(result.claimNotes[0].noteText, result.claimNotes[0].noteType) : null);
          claim.setPrescriptionNotes(result.prescriptionNotes);
          claim.setFlex2(result.claimFlex2s);
          claim.setDocumentTypes(result.documentTypes);
          if (result.images) {
            claim.setImages(result.images);
          }
          this.selected = result.claimId;
          claim.setPrescriptionStatuses(result.prescriptionStatuses);
          if (addHistory && result.claimId) {
            this.addHistory(result.claimId);
          }
          if (result.episodeTypes) {
            this.episodeNoteTypes = result.episodeTypes;
          }
          this.onClaimIdChanged.next(this.selected);
        }
      }, err => {
        this.loading = false;
        try {
          const error = err.error;

        } catch (e) { }
      }, () => {
        this.events.broadcast('claim-updated');
        this.http.getNotetypes()
          .subscribe((result: Array<any>) => {

            this.notetypes = result;
          }, err => {
            this.loading = false;
            // let error = err.error;
          });
        this.http.getPrescriptionNotetypes()
          .subscribe((result: Array<any>) => {

            this.prescriptionNotetypes = result;
          }, err => {
            this.loading = false;

            // let error = err.error;
          });

        if (!this.episodeNoteTypes) {
          this.http.getEpisodesNoteTypes()
            .subscribe((result: Array<any>) => {

              this.episodeNoteTypes = result;
            }, err => {
              this.loading = false;

              const error = err.error;
            });
        }
      });
  }

  get dataSize() {
    return this.claims.size;
  }
  get claimsData(): Claim[] {
    return this.claims.asImmutable().toArray();
  }
  deselectAll() {
    this.claims.forEach(c => {
      c.selected = false;
      this.claims.set(c.claimId, c);
    });
    this.comparisonClaims = Immutable.OrderedMap<Number, Claim>(); ;
  }
  claimsDataLength(): number {
    const claimsLength = this.claims.asImmutable().toArray().length;
    return claimsLength;
  }
  clearClaimsData() {
    this.claims = Immutable.OrderedMap<Number, Claim>();
  }
  get NoteTypes(): Array<any> {
    return this.notetypes;
  }
  get PrescriptionNoteTypes(): Array<any> {
    return this.prescriptionNotetypes;
  }

  get EpisodeNoteTypes(): Array<any> {
    return this.episodeNoteTypes;
  }
  getClaimsDataById(id: Number, addHistory = true) {
    this.selected = id;
    const claim: Claim = this.claims.get(id) as Claim;
    if (id !== undefined) {
      this.loading = true;
      this.history.unshift(claim);
      this.http.getClaimsData({ claimId: id })
        .subscribe(result => {
          this.loading = false;
          claim.dateOfBirth = result.dateOfBirth;
          claim.adjustor = result.adjustor;
          claim.adjustorExtension = result.adjustorExtension;
          claim.adjustorPhoneNumber = result.adjustorPhoneNumber;
          claim.adjustorFaxNumber = result.adjustorFaxNumber;
          claim.eligibilityTermDate = result.eligibilityTermDate;
          claim.dateEntered = result.dateEntered;
          claim.gender = result.gender;
          claim.address1 = result.address1;
          claim.address2 = result.address2;
          claim.city = result.city;
          claim.genders = result.genders;
          claim.states = result.states;
          claim.stateAbbreviation = result.stateAbbreviation;
          claim.postalCode = result.postalCode;
          claim.adjustorId = result.adjustorId;
          claim.payorId = result.payorId;
          claim.genderId = result.patientGenderId;
          claim.stateId = result.stateId;
          claim.isVip = result.isVip;
          claim.isMaxBalance = result.isMaxBalance;
          claim.claimFlex2Id = result.claimFlex2Id;
          claim.setPrescription(result.prescriptions as Array<Prescription>);
          claim.setPayment(result.payments);
          claim.setEpisodes(result.episodes);
          claim.setClaimNotes(result.claimNotes && result.claimNotes[0] ? new ClaimNote(result.claimNotes[0].noteText, result.claimNotes[0].noteType) : null);
          claim.setPrescriptionNotes(result.prescriptionNotes);
          claim.setFlex2(result.claimFlex2s);
          claim.setPrescriptionStatuses(result.prescriptionStatuses);
          claim.setDocumentTypes(result.documentTypes);
          if (result.images) {
            claim.setImages(result.images);
          }
          if (result.episodeTypes) {
            this.episodeNoteTypes = result.episodeTypes;
          }
          if (result.outstanding) {
            claim.outstanding = result.outstanding.results;
            claim.totalOutstandingAmount = result.outstanding.totalOutstandingAmount;
            claim.numberOutstanding = result.outstanding.totalRows;
            claim.loadingOutstanding = false;
          }
          this.onClaimIdChanged.next(this.selected);
        }, err => {
          this.loading = false;
          const error = err.error;
        }, () => {
          this.events.broadcast('claim-updated');
          if (addHistory && id) {
            this.addHistory(id);
          }
        });
    }
  }
  get selectedClaim(): Claim {
    return this.claims.get(this.selected);
  }

  changeFlex2(claim: Claim, flex: any) {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Save Flex 2 for claim #' + claim.claimNumber,
      message: 'Would you like to change the Flex2 value to ' + (flex.flex2) + '?'
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.loading = true;
          this.http.saveFlex2({ claimId: claim.claimId, claimFlex2Id: flex.claimFlex2Id }).subscribe(result => {
            if (result.message) {
              this.toast.success(result.message);
              claim.flex2 = flex.flex2;
            }
            this.loading = false;
          }, err => {
            if (err.message) {
              this.toast.success(err.message);
            }
            this.loading = false;
          });
        }
      });
  }
  changePrescriptionStatus(prescription: Prescription, status: any) {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Change status for Prescription #' + prescription.prescriptionId,
      message: 'Would you like to change the status to ' + (status.statusName) + '?'
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.loading = true;
          this.http.updatePrescriptionStatus({ prescriptionId: prescription.prescriptionId, prescriptionStatusId: status.prescriptionStatusId }).subscribe(result => {
            if (result.message) {
              this.toast.success(result.message);
              prescription.status = status.statusName;
            }
            this.loading = false;
          }, err => {
            if (err.message) {
              this.toast.success(err.message);
            }
            this.loading = false;
          });
        }
      });
  }


}
