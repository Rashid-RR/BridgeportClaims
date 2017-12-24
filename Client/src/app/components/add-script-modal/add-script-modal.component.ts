import { Component, OnInit,AfterViewInit } from '@angular/core';
import { Toast, ToastsManager } from 'ng2-toastr/ng2-toastr';
import {PaymentScriptService} from "../../services/payment-script-service"
import {EventsService} from "../../services/events-service"
import { Subject } from 'rxjs/Subject';
declare var $:any;

@Component({
  selector: 'app-add-script-modal',
  templateUrl: './add-script-modal.component.html',
  styleUrls: ['./add-script-modal.component.css']
})
export class AddScriptModalComponent implements OnInit,AfterViewInit {


  dropdownVisible:boolean=false; 
  searchText: string = '';
  showDropDown = new Subject<any>();
  constructor(private payment:PaymentScriptService,private events:EventsService,
    private toast: ToastsManager) { 

  }

  ngOnInit() {    

  }
  ngAfterViewInit() {    
    $('#datepicker').datepicker({
      autoclose: true
    });
  }
  checkMatch($event) {
    this.payment.exactMatch = $event.target.checked;
    this.showDropDown.next($event.target.checked);
  }

  claimSelected($event) {
    if (this.searchText && $event.claimId) {
      this.payment.form.patchValue({
        claimId: $event.claimId,
        claimNumber: $event.claimNumber,
        firstName: $event.firstName,
        groupNumber: $event.groupNumber,
        lastName: $event.lastName
      });
      this.toast.info($event.lastName + " " + $event.firstName + " " + $event.claimNumber + " has been linked", 'Claim Linked', { enableHTML: true, positionClass: 'toast-top-center' })
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
  lastInput($event) {
    this.searchText = $event.target.value;
  }

}
