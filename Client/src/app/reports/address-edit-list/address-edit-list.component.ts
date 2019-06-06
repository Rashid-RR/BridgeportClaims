import { Component, OnInit } from '@angular/core';
import { AddressEditService } from '../../services/address-edit.service';

@Component({
  selector: 'app-address-edit-list',
  templateUrl: './address-edit-list.component.html'
})
export class AddressEditListComponent implements OnInit {

  constructor(private addressEditService: AddressEditService) {}

  ngOnInit(): void {}
}
