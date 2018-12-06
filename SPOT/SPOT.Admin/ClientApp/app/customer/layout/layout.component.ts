import { Component, OnInit } from '@angular/core';
import { CustomerService } from "../../shared"

@Component({
  selector: 'customer-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class CustomerLayoutComponent implements OnInit {

  constructor(public customerService: CustomerService) { }

  ngOnInit() {

  }
  get customerName() {
    return (this.customerService.contactForm.controls.firstName.value || '') + ' ' + (this.customerService.contactForm.controls.lastName.value || '');
  }

}
