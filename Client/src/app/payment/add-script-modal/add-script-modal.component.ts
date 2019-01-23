import { Component, OnInit, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { Toast, ToastrService } from 'ngx-toastr';
import { PaymentScriptService } from '../../services/payment-script-service';
import { EventsService } from '../../services/events-service';
import { Subject } from 'rxjs';
declare var $: any;

@Component({
  selector: 'app-add-script-modal',
  templateUrl: './add-script-modal.component.html',
  styleUrls: ['./add-script-modal.component.css']
})
export class AddScriptModalComponent implements OnInit, AfterViewInit {

  @ViewChild('lastname') lastname: ElementRef;
  submitted = false;
  dropdownVisible = false;
  searchText = '';
  showDropDown = new Subject<any>();

  constructor(public payment: PaymentScriptService, private events: EventsService,
    private toast: ToastrService) {

  }

  ngOnInit() {

  }
  ngAfterViewInit() {
    $('#datepicker').datepicker({
      autoclose: true
    });
    this.lastname.nativeElement.focus();
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
      this.toast.info($event.lastName + ' ' + $event.firstName + ' ' + $event.claimNumber +
        ' has been loaded. Wait for a few seconds to load details...',
        'Claim Loaded', { timeOut: 3000, enableHtml: true, positionClass: 'toast-top-center' });
      this.payment.search();
      setTimeout(() => {
        this.searchText = undefined;
        this.dropdownVisible = false;
      }, 100);
    }
  }
  lastInput($event) {
    this.searchText = $event.target.value;
  }

}
