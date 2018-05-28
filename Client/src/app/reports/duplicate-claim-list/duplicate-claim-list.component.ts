import { Component, ViewChild, ElementRef, OnInit } from '@angular/core';
import { ReportLoaderService, ComparisonClaim, DuplicateClaim } from "../../services/services.barrel";
import { Toast, ToastsManager } from 'ng2-toastr/ng2-toastr';
import { Claim } from '../../models/claim';
import { SwalComponent } from '@toverux/ngx-sweetalert2';
import swal from "sweetalert2";
import { HttpService } from '../../services/http-service';

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
  mergedClaim: Claim = {} as Claim;
  @ViewChild('claimActionSwal') private claimSwal: SwalComponent;
  comparisonClaims: ComparisonClaim = {} as ComparisonClaim
  constructor(private http: HttpService, public reportloader: ReportLoaderService, private toast: ToastsManager) { }

  ngOnInit() {
    this.reportloader.current = 'Duplicate Claims Report';
    this.reportloader.currentURL = 'duplicate-claims';
    this.reportloader.loading = false;
  }
  merge(value: any,$event, index: string) {
    if(value==this.mergedClaim[index] && !$event.checked){
      this.mergedClaim[index]=undefined;
    }else{
      this.mergedClaim[index]= $event.checked ? value : this.mergedClaim[index];
    }
  }
  save(){
    console.log(this.mergedClaim);
    this.toast.info(JSON.stringify(this.mergedClaim),'Awaiting API to save').then((toast: Toast) => {
      this.activeToast = toast;
    })
  }
  select(claim: DuplicateClaim, $event, index: number) {    
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
    if (this.reportloader.selectedClaims.length == 2) {
      this.showModal();
    }
  }



  showModal() {
    this.reportloader.loading = true;
    this.http.getComparisonClaims({ leftClaimId: this.reportloader.selectedClaims[0].claimId, rightClaimId: this.reportloader.selectedClaims[1].claimId })
      .single().map(r => r.json()).subscribe(r => {
        this.reportloader.loading = false;
        this.comparisonClaims = Array.isArray(r) ? r[0] : r;
        this.claimSwal.show().then((r) => {

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
