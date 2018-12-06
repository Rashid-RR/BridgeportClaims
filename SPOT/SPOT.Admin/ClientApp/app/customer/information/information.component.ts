import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from "@angular/forms"

@Component({
  selector: 'customer-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.scss']
})
export class CustomerInformationComponent implements OnInit {
  contactForm: FormGroup;

  constructor(private formBuilder: FormBuilder) {
      //hardcoded form values are only for testing, will remove
    this.contactForm = this.formBuilder.group({
      firstName: ['Can', Validators.compose([Validators.required])],
      lastName: ['Yelloc', Validators.compose([Validators.required])],
      type: ['Individual', Validators.compose([Validators.required])],
      email: ['email@example.com', Validators.compose([Validators.email])],
      vipUser: [false],
      phoneType: ['Mobile', Validators.compose([Validators.required])],
      phoneNumber: ['(123) 456-7890', Validators.compose([Validators.required])],
      password: ['00000', Validators.compose([Validators.required])],
      address: [
        [{ for: 'Billing, Mailing', line1: '', line2: '', city: 'San Francisco', state: 'CA', zip: '123-456' }],        
      ] 
    });
  }

  ngOnInit() {
  }
  get customerName() {
    return (this.contactForm.controls.firstName.value||'')+' '+(this.contactForm.controls.lastName.value||'');
  }

}
