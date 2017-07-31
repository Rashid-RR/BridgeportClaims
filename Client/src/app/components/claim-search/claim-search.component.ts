import { Component, OnInit } from '@angular/core';
import {FormBuilder,FormControl, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {HttpService} from "../../services/http-service";
import {ClaimManager} from "../../services/claim-manager";
import {EventsService} from "../../services/events-service";
@Component({
  selector: 'app-claim-search',
  templateUrl: './claim-search.component.html',
  styleUrls: ['./claim-search.component.css']
})
export class ClaimSearchComponent implements OnInit {

  form: FormGroup;
  submitted: boolean = false;
  constructor(public claimManager:ClaimManager,private formBuilder: FormBuilder, private http: HttpService, private router: Router, private events: EventsService) {
    this.form = this.formBuilder.group({
      claimNumber: [null],
      firstName: [null],
      lastName: [null],
      rxNumber: [null],
      invoiceNumber: [null]
    });
  }
  ngOnInit() {

  }

 textChange(controlName:string){
   if(this.form.get(controlName).value ==='undefined' || this.form.get(controlName).value ===''){
     this.form.get(controlName).setValue(null);
   }
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
