import { Component, OnInit } from '@angular/core';
import { CustomerService } from "../../shared"
import { MatDialog } from "@angular/material";
import {CustomerAddressModalComponent} from "../address-modal/address-modal.component"

@Component({
  selector: 'customer-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.scss']
})
export class CustomerInformationComponent implements OnInit {


  constructor(public customerService: CustomerService, public dialog: MatDialog) {
    //hardcoded form values are only for testing, will remove

  }

  ngOnInit() {
  }
  get customerName() {
    return (this.customerService.contactForm.controls.firstName.value || '') + ' ' + (this.customerService.contactForm.controls.lastName.value || '');
  }
  addAddress() {
    const dialogRef = this.dialog.open(CustomerAddressModalComponent,{maxWidth:500,data:{title:'Add New Address'}});

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result){
        let address = this.customerService.contactForm.controls.address.value;
        address.push(result);
        this.customerService.contactForm.controls.address.setValue(address);
      }
    });
  }
  editAddress(i,address) {
    const dialogRef = this.dialog.open(CustomerAddressModalComponent,{maxWidth:500,data:{title:'Update Address',address:address}});
    dialogRef.afterClosed().subscribe(result => {
      if(result){
        let address = this.customerService.contactForm.controls.address.value;
        address.splice(i,1,result); 
        this.customerService.contactForm.controls.address.setValue(address);
      }
    });
  }

}
