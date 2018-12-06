import { Component, OnInit } from '@angular/core';
import {CustomerService} from "../../shared"

@Component({
  selector: 'customer-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.scss']
})
export class CustomerInformationComponent implements OnInit {
  

  constructor(public customerService:CustomerService) {
      //hardcoded form values are only for testing, will remove
    
  }

  ngOnInit() {
  }
  get customerName() {
    return (this.customerService.contactForm.controls.firstName.value||'')+' '+(this.customerService.contactForm.controls.lastName.value||'');
  }

}
