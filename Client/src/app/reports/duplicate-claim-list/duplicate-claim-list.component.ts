import { Component, ViewChild, OnInit } from '@angular/core';
import { ReportLoaderService, ComparisonClaim, DuplicateClaim } from '../../services/services.barrel';
import { Toast, ToastrService } from 'ngx-toastr';
import { SwalComponent } from '@toverux/ngx-sweetalert2';
import swal from 'sweetalert2';
import { HttpService } from '../../services/http-service';
import { DatePipe } from '@angular/common';
import { ConfirmComponent } from '../../components/confirm.component';
import { DialogService } from 'ng2-bootstrap-modal';

declare var jQuery: any;
@Component({
  selector: 'app-duplicate-claim-list',
  templateUrl: './duplicate-claim-list.component.html',
  styleUrls: ['./duplicate-claim-list.component.css']
})
export class DuplicateClaimListComponent implements OnInit {
  goToPage: any = '';
  activeToast: number;
  selectMultiple: Boolean = false;
  lastSelectedIndex: number;
  mergedClaim: any = {} as any;
  @ViewChild('claimActionSwal') private claimSwal: SwalComponent;
  comparisonClaims: ComparisonClaim = {} as ComparisonClaim;
  constructor(private dialogService: DialogService, private dp: DatePipe, private http: HttpService, public reportloader: ReportLoaderService, private toast: ToastrService) { }

  ngOnInit() {
    this.reportloader.current = 'Duplicate Claims Report';
    this.reportloader.currentURL = 'duplicate-claims';
    // this.reportloader.loading = false;
  }
  archive(claim) {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Remove Claim',
      message: 'Are you sure you\'d like to remove this claim?'
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.reportloader.loading = true;
          this.http.archiveDuplicateClaim(claim.claimId).subscribe(res => {
            this.toast.success(res.message);
            this.reportloader.loading = false;
            this.reportloader.fetchDuplicateClaims();
            this.closeModal();
          }, error => {
            this.toast.error(error.message);
            this.reportloader.loading = false;
          });
        }
      });
  }
  merge(value: any, $event, index: string) {
    if (value === this.mergedClaim[index] && !$event.checked) {
      this.mergedClaim[index] = undefined;
    } else {
      this.mergedClaim[index] = $event.checked ? value : this.mergedClaim[index];
    }
  }
  closeModal() {
    this.deselectAll();
    try { swal.clickCancel(); } catch (e) { }
  }
  checked(field: string, side: string) {
    return this.mergedClaim.hasOwnProperty(field) && this.mergedClaim[field] === this.comparisonClaims[`${side}${field}`];
  }
  save() {
    const duplicate = this.reportloader.selectedClaims.find(c => c.claimId !== this.mergedClaim.ClaimId);
    const form = JSON.parse(JSON.stringify(this.mergedClaim));
    const InjuryDate = this.dp.transform(form.InjuryDate, 'MM/dd/yyyy');
    const DateOfBirth = this.dp.transform(form.DateOfBirth, 'MM/dd/yyyy');
    form.DuplicateClaimId = duplicate.claimId;
    form.ClaimFlex2Id = form.ClaimFlex2Value === this.comparisonClaims.leftClaimFlex2Value ? this.comparisonClaims.leftClaimFlex2Value : (form.ClaimFlex2Value ? this.comparisonClaims.rightClaimFlex2Id : undefined);
    form.AdjustorId = form.AdjustorName === this.comparisonClaims.leftAdjustorName ? this.comparisonClaims.leftAdjustorId : (form.AdjustorName ? this.comparisonClaims.rightAdjustorId : undefined);
    form.PatientId = form.PatientName === this.comparisonClaims.leftPatientName ? this.comparisonClaims.leftPatientId : (form.PatientName ? this.comparisonClaims.rightPatientId : undefined);
    form.PayorId = form.Carrier === this.comparisonClaims.leftCarrier ? this.comparisonClaims.leftPayorId : (form.Carrier ? this.comparisonClaims.rightPayorId : undefined);
    // form.PersonCode = this.reportloader.selectedClaims[0].personCode;
    if (!this.comparisonClaims.leftClaimFlex2Id && !this.comparisonClaims.rightClaimFlex2Id) {
      form['ClaimFlex2Id'] = null;
    } else  if (form.ClaimFlex2Id === undefined) {
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
      this.reportloader.loading = true;
      this.http.getMergeClaims(form)
        .subscribe(r => {
          this.activeToast = this.toast.success('Claim successfully merged').toastId;
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
      if (this.reportloader.selectedClaims.length === 2 && $event.checked) {
        this.showModal();
        return;
      }
      claim.selected = $event.checked;
      if (this.selectMultiple) {
        for (let i = this.lastSelectedIndex; i < index; i++) {
          try {
            const c = jQuery('#row' + i).attr('claim');
            const claim = JSON.parse(c);
            const data = this.reportloader.duplicates.find(c => c.claimId === claim.claimId);
            data.selected = true;
          } catch (e) { }
        }
      }
      this.lastSelectedIndex = index;
      if (this.reportloader.selectedClaims.length === 1) {
        this.activeToast = this.toast.info('You have selected a Claim to compare. Please select another', '',
         { timeOut: 15000, closeButton: true }).toastId;
      } else if (this.reportloader.selectedClaims.length === 2) {
        this.activeToast = null;
        jQuery('.toast.toast-info').hide();
        this.showModal();
      }
    } else {
      this.toast.warning('You you have already chosen this claim for comparison.').onHidden.subscribe((toast: Toast) => {
      });
      $event.checked = true;
    }
  }

  deselectAll() {
    this.reportloader.duplicates.forEach(c => {
      c.selected = false;
    });
    this.mergedClaim = {};
  }



  showModal() {
    this.reportloader.loading = true;
    this.http.getComparisonClaims({ leftClaimId: this.reportloader.selectedClaims[0].claimId, rightClaimId: this.reportloader.selectedClaims[1].claimId })
      .subscribe(r => {
        this.reportloader.loading = false;
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
    const page = Number.parseInt(this.goToPage);
    if (!this.goToPage || isNaN(page)) {
      /* if(this.activeToast && this.activeToast.timeoutId){
        this.activeToast.message =  'Invalid page number entered'
        }else{
          this.toast.warning('Invalid page number entered').onHidden.subscribe((toast: Toast) => {
              this.activeToast = toast;
          })
      }*/
    } else if (page > 0 && ((this.reportloader.totalPages && page <= this.reportloader.totalPages) || this.reportloader.totalPages == null)) {
      this.reportloader.fetchDuplicateClaims(false, false, page);
    } else {
      let toast = this.toast.toasts.find(t=>t.toastId ==this.activeToast)
      if (toast) {
        toast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.reportloader.totalPages;
      } else {
        this.activeToast = this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.reportloader.totalPages).toastId;
      }
    }
  }

  keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;
    const inputChar = String.fromCharCode(event.charCode);
    const input = Number(this.goToPage + '' + inputChar);
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
