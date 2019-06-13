import { Component, OnInit } from '@angular/core';
import { AddressEditService } from '../../services/services.barrel';

@Component({
  selector: 'app-address-edit-search-filter',
  templateUrl: './address-edit-search-filter.component.html',
  styleUrls: ['./address-edit-search-filter.component.css']
})
export class AddressEditSearchFilterComponent implements OnInit {

  constructor(public addressEditSvc: AddressEditService) {}

  ngOnInit(): void {}

  clearFilter(): void {
    this.addressEditSvc.filterText = '';
  }

  refreshList(): void {
    this.addressEditSvc.refreshList$.next(true);
  }
}
