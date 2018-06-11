import { Component, ViewChild, ElementRef, OnInit } from '@angular/core';
import { ReportLoaderService, ComparisonClaim, DuplicateClaim } from "../../services/services.barrel";
import { Toast, ToastsManager } from 'ng2-toastr/ng2-toastr';
import { Claim } from '../../models/claim';
import { SwalComponent } from '@toverux/ngx-sweetalert2';
import swal from "sweetalert2";
import { HttpService } from '../../services/http-service';
import { DatePipe, DecimalPipe } from '@angular/common';

declare var jQuery: any;
@Component({
  selector: 'app-duplicate-claim-list',
  templateUrl: './duplicate-claim-list.component.html',
  styleUrls: ['./duplicate-claim-list.component.css']
})
export class DuplicateClaimListComponent implements OnInit {
  goToPage: any = '';
  activeToast: Toast;
  selectMultiple: Boolean = false;
  lastSelectedIndex: number;
  mergedClaim: any = {} as any;
  @ViewChild('claimActionSwal') private claimSwal: SwalComponent;
  comparisonClaims: ComparisonClaim = {} as ComparisonClaim
  constructor(private dp: DatePipe, private http: HttpService, public reportloader: ReportLoaderService, private toast: ToastsManager) { }

  ngOnInit() {
    this.reportloader.current = 'Duplicate Claims Report';
    this.reportloader.currentURL = 'duplicate-claims';
    //this.reportloader.loading = false;
  }
  merge(value: any, $event, index: string) {
    if (value == this.mergedClaim[index] && !$event.checked) {
      this.mergedClaim[index] = undefined;
    } else {
      this.mergedClaim[index] = $event.checked ? value : this.mergedClaim[index];
    }
  }
  closeModal() {
    this.deselectAll();
    try { swal.clickCancel(); } catch (e) { }
  }
  checked(field:string,side:string){
    return this.mergedClaim.hasOwnProperty(field) && this.mergedClaim[field]===this.comparisonClaims[`${side}${field}`];
  }
  save() {
    let duplicate = this.reportloader.selectedClaims.find(c => c.claimId != this.mergedClaim.ClaimId)
    let form = JSON.parse(JSON.stringify(this.mergedClaim));
    let InjuryDate = this.dp.transform(form.InjuryDate, "MM/dd/yyyy");
    let DateOfBirth = this.dp.transform(form.DateOfBirth, "MM/dd/yyyy");
    form.DuplicateClaimId = duplicate.claimId;
    form.ClaimFlex2Id = form.ClaimFlex2Value === this.comparisonClaims.leftClaimFlex2Value ? this.comparisonClaims.leftClaimFlex2Value : (form.ClaimFlex2Value ? this.comparisonClaims.rightClaimFlex2Id : undefined);
    form.AdjustorId = form.AdjustorName === this.comparisonClaims.leftAdjustorName ? this.comparisonClaims.leftAdjustorId : (form.AdjustorName ? this.comparisonClaims.rightAdjustorId : undefined);
    form.PatientId = form.PatientName === this.comparisonClaims.leftPatientName ? this.comparisonClaims.leftPatientId : (form.PatientName ? this.comparisonClaims.rightPatientId : undefined);
    form.PayorId = form.Carrier === this.comparisonClaims.leftCarrier ? this.comparisonClaims.leftPayorId : (form.Carrier ? this.comparisonClaims.rightPayorId : undefined);
    //form.PersonCode = this.reportloader.selectedClaims[0].personCode;
    if(!this.comparisonClaims.leftClaimFlex2Id && !this.comparisonClaims.rightClaimFlex2Id){
      form['ClaimFlex2Id'] =null;
    }else  if(form.ClaimFlex2Id===undefined){
      delete form['ClaimFlex2Id']
    }
    if(!this.comparisonClaims.leftAdjustorId && !this.comparisonClaims.rightAdjustorId){
      form['AdjustorId'] =null;
    }else if(form.AdjustorId===undefined){
      delete form['AdjustorId']
    }
    if(!this.comparisonClaims.leftPatientId && !this.comparisonClaims.rightPatientId){
      form['PatientId'] =null;
    }else if(form.PatientId===undefined){
      delete form['PatientId']
    }
    if(!this.comparisonClaims.leftPayorId && !this.comparisonClaims.rightPayorId){
      form['PayorId'] =null;
    }else if(form.PayorId===undefined){
      delete form['PayorId']
    }
    delete form.PatientName;
    delete form.AdjustorName;
    delete form.Carrier;
    delete form.ClaimFlex2Value;
    if(InjuryDate) form.InjuryDate =InjuryDate
    if(DateOfBirth) form.DateOfBirth =DateOfBirth;
    if (form.DuplicateClaimId && form.hasOwnProperty("ClaimFlex2Id") && form.hasOwnProperty("ClaimId") && form.hasOwnProperty("ClaimNumber") && form.hasOwnProperty("DateOfBirth") && form.hasOwnProperty("InjuryDate")
      && form.hasOwnProperty("AdjustorId") && form.hasOwnProperty("PatientId") && form.hasOwnProperty("PayorId")) {
      this.reportloader.loading = true;
      this.http.getMergeClaims(form)
        .single().map(r => r.json()).subscribe(r => {
          this.toast.success('Claim successfully merged').then((toast: Toast) => {
            this.activeToast = toast;
          })
          this.closeModal();
          this.reportloader.fetchDuplicateClaims();
          
        }, err => {
          this.reportloader.loading = false;
        });
    } else {
      this.toast.warning('Please choose a value for every field for these claims in order to save the merge...');
    }
  }
  select(claim: DuplicateClaim, $event, index: number) {
    if ($event.checked) {
      if (this.reportloader.selectedClaims.length == 2 && $event.checked) {
        this.showModal();
        return;
      }
      claim.selected = $event.checked;
      if (this.selectMultiple) {
        for (var i = this.lastSelectedIndex; i < index; i++) {
          try {
            let c = jQuery('#row' + i).attr('claim');
            let claim = JSON.parse(c);
            let data = this.reportloader.duplicates.find(c => c.claimId == claim.claimId);
            data.selected = true;
          } catch (e) { }
        }
      }
      this.lastSelectedIndex = index;
      if (this.reportloader.selectedClaims.length == 1) {
        this.toast.info('You have selected a Claim to compare. Please select another', '', { toastLife: 15000, showCloseButton: true }).then((toast: Toast) => {
          this.activeToast = toast;
        })
      } else if (this.reportloader.selectedClaims.length == 2) {
        this.activeToast.timeoutId = null;
        jQuery(".toast.toast-info").hide();
        this.showModal();
      }
    } else {
      this.toast.warning('You you have already chosen this claim for comparison.').then((toast: Toast) => {
      })
      $event.checked = true;
    }
  }

  deselectAll() {
    this.reportloader.duplicates.forEach(c => {
      c.selected = false;
    });
    this.mergedClaim={};
  }



  showModal() {
    this.reportloader.loading = true;
    this.http.getComparisonClaims({ leftClaimId: this.reportloader.selectedClaims[0].claimId, rightClaimId: this.reportloader.selectedClaims[1].claimId })
      .single().map(r => r.json()).subscribe(r => {
        this.reportloader.loading = false;
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
        this.reportloader.loading = false;
      });
  }


  next() {
    this.reportloader.fetchDuplicateClaims(true);
  }
  prev() {
    this.reportloader.fetchDuplicateClaims(false, true);
  }

  goto() {
    let page = Number.parseInt(this.goToPage);
    if (!this.goToPage || isNaN(page)) {
      /* if(this.activeToast && this.activeToast.timeoutId){
        this.activeToast.message =  'Invalid page number entered'
        }else{
          this.toast.warning('Invalid page number entered').then((toast: Toast) => {
              this.activeToast = toast;
          })
      }*/
    } else if (page > 0 && ((this.reportloader.totalPages && page <= this.reportloader.totalPages) || this.reportloader.totalPages == null)) {
      this.reportloader.fetchDuplicateClaims(false, false, page);
    } else {
      if (this.activeToast && this.activeToast.timeoutId) {
        this.activeToast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.reportloader.totalPages
      } else {
        this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.reportloader.totalPages).then((toast: Toast) => {
          this.activeToast = toast;
        })
      }
    }
  }

  keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;
    let inputChar = String.fromCharCode(event.charCode);
    let input = Number(this.goToPage + "" + inputChar);
    if (!pattern.test(inputChar)) {
      event.preventDefault();
    } else if (!this.isNumeric(input)) {
      event.preventDefault();
    } else if (input < 1) {
      event.preventDefault();
    }
  }
  isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
  }

}
