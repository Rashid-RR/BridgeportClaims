import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
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

export interface AddressEditState {
  stateId: number;
  stateName: string;
}

@Injectable()
export class AddressEditService {
  public filterText: string;
  rows: AddressEdit[] = [];

  constructor(private http: HttpService) {}

  getPatientAddressEdit(): Observable<AddressEdit> {
    return this.http.getPatientAddressEdit();
  }

  getStates(): AddressEditState[] {
    let states: AddressEditState[];
    this.http.states({}).subscribe(res => {
      states = res;
    }, () => {});
    return states;
  }
}
