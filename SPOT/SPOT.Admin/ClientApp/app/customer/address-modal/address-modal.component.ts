import { Component, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Inject } from '@angular/core';
import { FormArray, FormBuilder, FormControl, Validators, FormGroup } from "@angular/forms"

@Component({
  selector: 'customer-address-modal',
  templateUrl: './address-modal.component.html',
  styleUrls: ['./address-modal.component.scss']
})
export class CustomerAddressModalComponent implements OnInit {

  addressForm: FormGroup;
  title: string;
  addressUse: any = [
    "Default",
    "Delivery",
    "Billing",
    "Mailing",
    "Other"
  ];
  states = [ { "name": "Alabama", "code": "AL" }, { "name": "Alaska", "code": "AK" }, { "name": "American Samoa", "code": "AS" }, { "name": "Arizona", "code": "AZ" }, { "name": "Arkansas", "code": "AR" }, { "name": "California", "code": "CA" }, { "name": "Colorado", "code": "CO" }, { "name": "Connecticut", "code": "CT" }, { "name": "Delaware", "code": "DE" }, { "name": "District Of Columbia", "code": "DC" }, { "name": "Federated States Of Micronesia", "code": "FM" }, { "name": "Florida", "code": "FL" }, { "name": "Georgia", "code": "GA" }, { "name": "Guam", "code": "GU" }, { "name": "Hawaii", "code": "HI" }, { "name": "Idaho", "code": "ID" }, { "name": "Illinois", "code": "IL" }, { "name": "Indiana", "code": "IN" }, { "name": "Iowa", "code": "IA" }, { "name": "Kansas", "code": "KS" }, { "name": "Kentucky", "code": "KY" }, { "name": "Louisiana", "code": "LA" }, { "name": "Maine", "code": "ME" }, { "name": "Marshall Islands", "code": "MH" }, { "name": "Maryland", "code": "MD" }, { "name": "Massachusetts", "code": "MA" }, { "name": "Michigan", "code": "MI" }, { "name": "Minnesota", "code": "MN" }, { "name": "Mississippi", "code": "MS" }, { "name": "Missouri", "code": "MO" }, { "name": "Montana", "code": "MT" }, { "name": "Nebraska", "code": "NE" }, { "name": "Nevada", "code": "NV" }, { "name": "New Hampshire", "code": "NH" }, { "name": "New Jersey", "code": "NJ" }, { "name": "New Mexico", "code": "NM" }, { "name": "New York", "code": "NY" }, { "name": "North Carolina", "code": "NC" }, { "name": "North Dakota", "code": "ND" }, { "name": "Northern Mariana Islands", "code": "MP" }, { "name": "Ohio", "code": "OH" }, { "name": "Oklahoma", "code": "OK" }, { "name": "Oregon", "code": "OR" }, { "name": "Palau", "code": "PW" }, { "name": "Pennsylvania", "code": "PA" }, { "name": "Puerto Rico", "code": "PR" }, { "name": "Rhode Island", "code": "RI" }, { "name": "South Carolina", "code": "SC" }, { "name": "South Dakota", "code": "SD" }, { "name": "Tennessee", "code": "TN" }, { "name": "Texas", "code": "TX" }, { "name": "Utah", "code": "UT" }, { "name": "Vermont", "code": "VT" }, { "name": "Virgin Islands", "code": "VI" }, { "name": "Virginia", "code": "VA" }, { "name": "Washington", "code": "WA" }, { "name": "West Virginia", "code": "WV" }, { "name": "Wisconsin", "code": "WI" }, { "name": "Wyoming", "code": "WY" } ];

  constructor(public dialogRef: MatDialogRef<CustomerAddressModalComponent>, @Inject(MAT_DIALOG_DATA) public data: any, private formBuilder: FormBuilder) {
    let address = data.address || {};
    this.title = data.title;
    const controls = this.addressUse.map(c => new FormControl(false));
    if (address.for && address.for.length > 0) {
      address.for.forEach(f => { 
        controls[this.addressUse.indexOf(f)].setValue(true);
      })
    }
    this.addressForm = this.formBuilder.group({
      for: new FormArray(controls),
      line1: [address.line1 || null, Validators.compose([Validators.required])],
      line2: [address.line2 || null],
      city: [address.city || null, Validators.compose([Validators.required])],
      state: [address.state || null, Validators.compose([Validators.required])],
      zip: [address.zip || null, Validators.compose([Validators.required])]
    });
  }

  ngOnInit() {

  }
  save() {
    let addressForm: any = {};
    Object.assign(addressForm, this.addressForm.value);
    addressForm.for = {};
    addressForm.for = Object.keys(this.addressUse).filter(k => this.addressForm.value.for[k])
    addressForm.for = addressForm.for.map(o=>{
      return this.addressUse[o];
    })
    this.data.address = addressForm;
    this.dialogRef.close(addressForm);
  }
  cancel() {
    this.dialogRef.close();
  }

}
