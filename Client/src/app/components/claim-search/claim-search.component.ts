import {Component, OnInit } from '@angular/core';
import {FormBuilder,FormControl, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import { Subject } from 'rxjs/Subject';
import { Toast, ToastsManager } from 'ng2-toastr/ng2-toastr';
import {HttpService} from "../../services/http-service";
import {ClaimManager} from "../../services/claim-manager";
import {EventsService} from "../../services/events-service";
declare var $:any;

@Component({
  selector: 'app-claim-search',
  templateUrl: './claim-search.component.html',
  styleUrls: ['./claim-search.component.css']
})
export class ClaimSearchComponent implements OnInit {

  form: FormGroup;
  submitted: boolean = false;
  showDropDown = new Subject<any>();
  searchText: string = '';
  dropdownVisible:boolean=false;
  constructor(public claimManager:ClaimManager,private formBuilder: FormBuilder, private http: HttpService, private router: Router, private events: EventsService,
    private toast: ToastsManager) {
    this.form = this.formBuilder.group({
      claimNumber: [null],
      firstName: [null],
      lastName: [null],
      groupNumber: [null],
      rxNumber: [null],
      invoiceNumber: [null]
    });
  }
  ngOnInit() {

    this.events.on("clear-claims", (status: boolean) => {
      this.form.reset();
    });
  }

 textChange(controlName:string){
   if(this.form.get(controlName).value ==='undefined' || this.form.get(controlName).value ===''){
     this.form.get(controlName).setValue(null);
   }
 }

 checkMatch($event) {
  this.claimManager.exactMatch = $event.target.checked;
  this.showDropDown.next($event.target.checked);
}
lastInput($event) {
  this.searchText = $event.target.value;
}
  search(){
    this.claimManager.search(this.form.value);
  }
  refresh(){
    var form = this.form.value;
    if(this.claimManager.selectedClaim){
      form.claimId = this.claimManager.selectedClaim.claimId
    }
    this.claimManager.search(form,false);
  }
  claimSelected($event) {
    if (this.searchText && $event.claimId) {
      this.form.patchValue({
        claimId: $event.claimId,
        claimNumber: $event.claimNumber,
        firstName: $event.firstName,
        groupNumber: $event.groupNumber,
        lastName: $event.lastName
      });
      this.toast.info($event.lastName + " " + $event.firstName + " " + $event.claimNumber + " has been loaded", 'Claim Loaded', { enableHTML: true, positionClass: 'toast-top-center' })
        .then((toast: Toast) => {
          const toasts: Array<HTMLElement> = $('.toast-message');
          for (let i = 0; i < toasts.length; i++) {
            const msg = toasts[i];
            if (msg.innerHTML === toast.message) {
              msg.parentNode.parentElement.style.left = 'calc(50vw - 200px)';
              msg.parentNode.parentElement.style.position = 'fixed';
            }
          }
        })
      setTimeout(() => {
        this.searchText = undefined;
        this.dropdownVisible=false
      }, 100);
    }
  }
  clear(){
    this.claimManager.selected = undefined;
    this.claimManager.clearClaimsData();
    this.form.patchValue({
      claimNumber: null,
      firstName: null,
      lastName: null,
      rxNumber: null,
      invoiceNumber: null
    });
  }

}
