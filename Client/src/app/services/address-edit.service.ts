import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UsState } from '../models/us-state';
import { HttpService } from './http-service';

export interface AddressEdit {
  patientId: number;
  lastName: string;
  firstName: string;
  address1: string;
  address2: string;
  city: string;
  postalCode: string;
  stateId?: number;
  stateCode: string;
  phoneNumber: string;
  emailAddress: string;
}

@Injectable()
export class AddressEditService {
  public filterText: string;
  rows: AddressEdit[] = [];
  states: string[];

  constructor(private http: HttpService) {}

  getPatientAddressEdit(): Observable<AddressEdit> {
    return this.http.getPatientAddressEdit();
  }
}
