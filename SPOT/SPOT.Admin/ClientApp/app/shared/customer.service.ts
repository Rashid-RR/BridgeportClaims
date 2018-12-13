import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from "@angular/forms"

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  contactForm: FormGroup;
  constructor(private formBuilder: FormBuilder) { 
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
        [{ for: ['Billing','Mailing'], line1: '123 South 300', line2: '', city: 'East Drapper', state: 'UT', zip: '84020' }],        
      ] 
    });
  }
}
