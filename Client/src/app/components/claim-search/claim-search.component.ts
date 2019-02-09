import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { HttpService } from '../../services/http-service';
import { ClaimManager } from '../../services/claim-manager';
import { EventsService } from '../../services/events-service';
declare var $: any;

@Component({
  selector: 'app-claim-search',
  templateUrl: './claim-search.component.html',
  styleUrls: ['./claim-search.component.css']
})
export class ClaimSearchComponent implements OnInit {

  form: FormGroup;
  submitted = false;
  showDropDown = new Subject<any>();
  searchText = '';
  dropdownVisible = false;
  constructor(public claimManager: ClaimManager, private formBuilder: FormBuilder, private http: HttpService, private router: Router, private events: EventsService,
    private toast: ToastrService) {
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

    this.events.on('clear-claims', () => {
      this.form.reset();
    });
    this.events.on('refresh-claims', () => {
      this.refresh();
    });
    // setTimeout(function() {
    //   this.claimManager.search('0309094012');
    // }, 2000);
  }

  textChange(controlName: string) {
    if (this.form.get(controlName).value === 'undefined' || this.form.get(controlName).value === '') {
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
  search() {
    this.claimManager.search(this.form.value);
  }
  refresh() {
    const form = this.form.value;
    if (this.claimManager.selectedClaim) {
      form.claimId = this.claimManager.selectedClaim.claimId;
    }
    this.claimManager.search(form, false);
  }
  clear() {
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
