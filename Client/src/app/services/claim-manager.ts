import { UUID } from 'angular2-uuid';
import * as Immutable from 'immutable';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Claim } from '../models/claim';
import { Prescription } from '../models/prescription';
import { ClaimNote } from '../models/claim-note';
import { PrescriptionNoteType } from '../models/prescription-note-type';
import { EpisodeNoteType } from '../models/episode-note-type';
import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { AuthGuard } from './auth.guard';
import { EventsService } from './events-service';
import { Router } from '@angular/router';
import { ToastsManager,Toast } from 'ng2-toastr/ng2-toastr';
import { DialogService } from 'ng2-bootstrap-modal';
import { ConfirmComponent } from '../components/confirm.component';
declare var $: any;
@Injectable()
export class ClaimManager {
  onClaimIdChanged = new Subject<Number>();
  private claims: Immutable.OrderedMap<Number, Claim> = Immutable.OrderedMap<Number, Claim>();
  private history: Array<Claim> = [];
  selected: Number;
  loading: Boolean = false;
  loadingHistory: Boolean = false;
  loadingPayment: Boolean = false;
  loadingPrescription: Boolean = false;
  private notetypes: Array<any> = [];
  private prescriptionNotetypes: Array<PrescriptionNoteType> = [];
  private episodeNoteTypes: Array<EpisodeNoteType> = []

  // Expanded Table Properties
  isClaimsExpanded: boolean;
  isNotesExpanded: boolean;
  isEpisodesExpanded: boolean;
  isPrescriptionsExpanded: boolean;
  isScriptNotesExpanded: boolean;
  isPaymentsExpanded: boolean;
  isImagesExpanded: boolean;
  activeToast: Toast;

  constructor(private auth: AuthGuard, private http: HttpService, private events: EventsService,
    private router: Router, private toast: ToastsManager,
    private dialogService: DialogService) {
    this.getHistory();
    this.events.on('loadHistory', () => {
      this.getHistory();
    });
    this.events.on('clear-claims', (status: boolean) => {
      this.selected = undefined;
      this.claims = Immutable.OrderedMap<Number, Claim>();
    });
  }

  getHistory() {
    this.auth.isLoggedIn.single().subscribe(res => {
      if (res == true) {
        this.loadingHistory = true;
        this.http.getHistory().single().map(r => { return r.json() }).subscribe(res => {
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
    '<br> Pharmacy Name: ' + prescription.pharmacyName,
     null,
    {toastLife: 10988000, showCloseButton: true, enableHTML: true, positionClass : 'toast-top-center'}).then((toast: Toast) => {
      //$(".toast-top-right").addClass('toast-top-center')

      const toasts: Array<HTMLElement > = $('.toast-message');
      console.log(toasts, toast);
        for (let i = 0; i < toasts.length; i++) {
            const msg = toasts[i];
            if (msg.innerHTML === toast.message) {
              msg.parentNode.parentElement.style.left = 'calc(50vw - 200px)';
              msg.parentNode.parentElement.style.position = 'fixed';
            }
        }
        this.activeToast = toast;
    })
  }

  get claimHistory(): Array<Claim> {
    return this.history;
  }
  addHistory(id: Number) {
    this.http.addHistory(id).single().subscribe((res: any) => {
      this.getHistory();
    }, err => {

    })
  }
  search(data, addHistory = true) {
    // let result:any = {"claimId":26957,"name":"Lakisha Krause","claimNumber":"08264","dateOfBirth":"1983-11-08T00:00:00.0000000","gender":"Not Specified","carrier":"AAA - DE","adjustor":"Karrie83","adjustorPhoneNumber":"399843-7211","adjustorFaxNumber":null,"eligibilityTermDate":"2104-12-20T21:28:32.0000000","flex2":"PIP","address1":"82 South Oak St.","address2":"285 West White Clarendon Way","city":"Oklahoma","stateAbbreviation":null,"postalCode":"61993","patientPhoneNumber":"327135-2042","dateEntered":"1984-02-04T17:22:41.0000000","claimFlex2s":[{"claimFlex2Id":2,"flex2":"DR APT"},{"claimFlex2Id":8,"flex2":"PA"},{"claimFlex2Id":1,"flex2":"PIP"},{"claimFlex2Id":12,"flex2":"TFS"},{"claimFlex2Id":11,"flex2":"UFO"},{"claimFlex2Id":9,"flex2":"UI"},{"claimFlex2Id":10,"flex2":"UIO"},{"claimFlex2Id":3,"flex2":"VALUE1"},{"claimFlex2Id":4,"flex2":"VALUE2"},{"claimFlex2Id":5,"flex2":"VALUE3"},{"claimFlex2Id":6,"flex2":"VALUE4"},{"claimFlex2Id":7,"flex2":"VALUE5"}],"claimNotes":[{"noteType":"OTHER","noteText":"Id Tam Id in eudis esset et Id venit. Et apparens Multum in volcans pars travissimantor pladior delerium."}],"episodes":[],"prescriptionStatuses":[{"prescriptionStatusId":1,"statusName":"VALUE1"},{"prescriptionStatusId":10,"statusName":"VALUE10"},{"prescriptionStatusId":2,"statusName":"VALUE2"},{"prescriptionStatusId":3,"statusName":"VALUE3"},{"prescriptionStatusId":4,"statusName":"VALUE4"},{"prescriptionStatusId":5,"statusName":"VALUE5"},{"prescriptionStatusId":6,"statusName":"VALUE6"},{"prescriptionStatusId":7,"statusName":"VALUE7"},{"prescriptionStatusId":8,"statusName":"VALUE8"},{"prescriptionStatusId":9,"statusName":"VALUE9"}],"prescriptions":[{"prescriptionId":15851,"rxDate":"2017-12-03T00:20:04.0000000","rxNumber":"47289","labelName":"Dwayne634","billTo":"Amelia","invoiceNumber":"39731","invoiceAmount":890.4770,"amountPaid":null,"outstanding":null,"invoiceDate":"1990-03-10T00:00:00.0000000","status":"VALUE7","noteCount":0,"isReversed":true},{"prescriptionId":5229,"rxDate":"2017-09-23T21:07:41.0000000","rxNumber":"27981","labelName":"Olga94","billTo":"Amelia","invoiceNumber":"45021","invoiceAmount":927.5000,"amountPaid":494.1700,"outstanding":433.3300,"invoiceDate":"1955-02-14T00:00:00.0000000","status":null,"noteCount":3,"isReversed":false}],"prescriptionNotes":[{"claimId":26957,"prescriptionNoteId":1170,"rxDate":"2017-09-23T21:07:41.0000000","rxNumber":"27981","type":"Denial","enteredBy":"Atiq Masood","note":"fecit. in vobis funem. glavans vobis funem. quartu travissimantor gravis e et bono cognitio, rarendum trepicandor fecit, parte","noteUpdatedOn":"1967-01-31T09:22:51.0000000",hasDiaryEntry:true},{"claimId":26957,"prescriptionNoteId":3416,"rxDate":"2017-09-23T21:07:41.0000000","rxNumber":"27981","type":"Med Records","enteredBy":"Jordan Gurney","note":"new diary entry","noteUpdatedOn":"2017-10-11T23:22:22.0000000",hasDiaryEntry:true},{"claimId":26957,"prescriptionNoteId":3417,"rxDate":"2017-09-23T21:07:41.0000000","rxNumber":"27981","type":"Non Formulary","enteredBy":"Josephat Ogwayi","note":"Test this","noteUpdatedOn":"2017-10-16T18:40:20.0000000",hasDiaryEntry:false}],"acctPayables":[{"date":"1998-01-09T00:00:00.0000000","checkNumber":"83027","rxNumber":"27981","rxDate":"2017-09-23T00:00:00.0000000","checkAmount":494.1700}],"payments":[{"prescriptionPaymentId":5256,"prescriptionId":5229,"postedDate":"1998-01-09T00:00:00.0000000","checkNumber":"83027","checkAmt":494.1700,"rxNumber":"27981","rxDate":"2017-09-23T21:07:41.2513254","invoiceNumber":null,"isReversed":false}]}; //for ofline testing
    this.loading = true;
     this.http.getClaimsData(data).map(res => { return res.json() })
      .subscribe((result: any) => {
        this.loading = false;
        this.selected = undefined;

        if (result.length < 1) {
          this.toast.info('No records were found from your search.');
        }
        if (Object.prototype.toString.call(result) === '[object Array]') {
          let res: Array<Claim> = result as Array<Claim>;
          this.claims = Immutable.OrderedMap<Number, Claim>();
          result.forEach((claim) => {
            var c = new Claim(claim.claimId, claim.claimNumber, claim.dateOfBirth, claim.injuryDate, claim.gender,
              claim.carrier, claim.adjustor, claim.adjustorPhoneNumber, claim.dateEntered, claim.adjustorPhoneNumber
              , claim.name, claim.firstName, claim.lastName,result.flex2);
            this.claims = this.claims.set(claim.claimId, c);
          })
        } else/*   if(result.name) */
        {
          this.claims = Immutable.OrderedMap<Number, Claim>();
          var c = new Claim(result.claimId, result.claimNumber, result.date, result.injuryDate, result.gender,
            result.carrier, result.adjustor, result.adjustorPhoneNumber, result.dateEntered, result.adjustorPhoneNumber
            , result.name, result.firstName, result.lastName,result.flex2);
          c.dateOfBirth = result.dateOfBirth;
          c.adjustor = result.adjustor;
          c.adjustorPhoneNumber = result.adjustorPhoneNumber;
          c.eligibilityTermDate = result.eligibilityTermDate;
          c.dateEntered = result.dateEntered;
          c.gender = result.gender;
          this.claims = this.claims.set(result.claimId, c);
          const claim = this.claims.get(result.claimId);
          claim.setPrescription(result.prescriptions as Array<Prescription>);
          claim.setPayment(result.payments);
          claim.setEpisodes(result.episodes);
          claim.setClaimNotes(result.claimNotes && result.claimNotes[0] ? new ClaimNote(result.claimNotes[0].noteText, result.claimNotes[0].noteType) : null);
          claim.setPrescriptionNotes(result.prescriptionNotes);
          claim.setFlex2(result.claimFlex2s);
          this.selected = result.claimId;
          claim.setPrescriptionStatuses(result.prescriptionStatuses);
          if (addHistory && result.claimId) {
            this.addHistory(result.claimId);
          }
          this.onClaimIdChanged.next(this.selected);
       }
      }, err => {
        this.loading = false;
        try {
          let error = err.json();
          // console.log(error);
        } catch (e) { }
      }, () => {
        this.events.broadcast("claim-updated");
      this.http.getNotetypes().map(res => { return res.json() })
        .subscribe((result: Array<any>) => {
          // console.log("Claim Notes",result)
          this.notetypes = result;
        }, err => {
          this.loading = false;
          // let error = err.json();
        })
      this.http.getPrescriptionNotetypes().map(res => { return res.json() })
        .subscribe((result: Array<any>) => {
          // console.log("Prescription Notes", result)
          this.prescriptionNotetypes = result;
        }, err => {
          this.loading = false;
          // console.log(err);
          // let error = err.json();
        })
      this.http.getEpisodesNoteTypes().map(res => { return res.json() })
        .subscribe((result: Array<any>) => {
          // console.log("Episode Notes", result)
          this.episodeNoteTypes = result;
        }, err => {
          this.loading = false;
          console.log(err);
          let error = err.json();
        });
      })
  }

  get dataSize() {
    return this.claims.size;
  }
  get claimsData(): Claim[] {
    return this.claims.asImmutable().toArray();
  }
  claimsDataLength(): number {
    let claimsLength = this.claims.asImmutable().toArray().length;
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
    let claim: Claim = this.claims.get(id) as Claim;
    if (id !== undefined) {
      this.loading = true;
      this.history.unshift(claim);
      this.http.getClaimsData({ claimId: id }).map(res => { return res.json() })
        .subscribe(result => {
          this.loading = false;
          claim.dateOfBirth = result.dateOfBirth;
          claim.adjustor = result.adjustor;
          claim.adjustorPhoneNumber = result.adjustorPhoneNumber;
          claim.eligibilityTermDate = result.eligibilityTermDate;
          claim.dateEntered = result.dateEntered;
          claim.gender = result.gender;
          claim.setPrescription(result.prescriptions as Array<Prescription>);
          claim.setPayment(result.payments);
          claim.setEpisodes(result.episodes);
          claim.setClaimNotes(result.claimNotes && result.claimNotes[0] ? new ClaimNote(result.claimNotes[0].noteText, result.claimNotes[0].noteType) : null);
          claim.setPrescriptionNotes(result.prescriptionNotes);
          claim.setFlex2(result.claimFlex2s);
          claim.setPrescriptionStatuses(result.prescriptionStatuses);
        }, err => {
          this.loading = false;
          const error = err.json();
        }, () => {
          this.events.broadcast("claim-updated");
          if (addHistory && id) {
            this.addHistory(id);
          }
        })
    }
  }
  get selectedClaim(): Claim {
    return this.claims.get(this.selected)
  }

  changeFlex2(claim:Claim,flex:any){
      const disposable = this.dialogService.addDialog(ConfirmComponent, {
        title: "Save Flex 2 for claim #"+claim.claimNumber,
        message: "Would you like to change the Flex2 value to "+(flex.flex2)+"?"
      })
      .subscribe((isConfirmed) => {
          if (isConfirmed) {
            this.loading = true;
            this.http.saveFlex2({claimId:claim.claimId,claimFlex2Id:flex.claimFlex2Id}).map(r=>{return r.json()}).subscribe(result=>{
              if (result.message) {
                this.toast.success(result.message);
                claim.flex2 = flex.flex2;
              }
              this.loading = false;
            },err=>{
              if (err.message) {
                this.toast.success(err.message);
              }
              this.loading = false;
            });
          }
      });
  }
  changePrescriptionStatus(prescription:Prescription,status:any){
      const disposable = this.dialogService.addDialog(ConfirmComponent, {
        title: 'Change status for Prescription #'+prescription.prescriptionId,
        message: 'Would you like to change the status to '+(status.statusName)+'?'
      })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.loading = true;
          this.http.updatePrescriptionStatus({prescriptionId:prescription.prescriptionId,prescriptionStatusId:status.prescriptionStatusId}).map(r=>{return r.json();}).subscribe(result => {
            if (result.message) {
              this.toast.success(result.message);
              prescription.status = status.statusName;
            }
            this.loading = false;
          },err => {
            if (err.message) {
              this.toast.success(err.message);
            }
            this.loading = false;
          });
        }
      });
  }
}
