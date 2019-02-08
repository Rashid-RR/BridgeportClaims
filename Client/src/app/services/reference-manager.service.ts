import {Injectable} from '@angular/core';
import {HttpService} from './http-service';
import {AdjustorItem} from '../references/dataitems/adjustor-item.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UsState } from '../models/us-state';
import { PayorItem } from '../references/dataitems/payor-item.model';
import { AttorneyItem } from '../references/dataitems/attorney-item.model';

declare var $: any;

@Injectable()
export class ReferenceManagerService {
  adjustorForm: FormGroup;
  attorneyForm: FormGroup;
  payorForm: FormGroup;
  states: UsState[];
  public editFlag = false;
  public loading = false;
  public adjustors: Array<AdjustorItem>;
  public payors: Array<PayorItem>;
  public attorneys: Array<AttorneyItem>;
  public totalEntityRows = 0;
  public totalRows = 0;
  public pageSize: number;
  public types = ['Adjustor', 'Attorney', 'Payor'];
  public typeSelected: string;
  public editedEntity: any;
  private searchText: string;
  public sortColumn: string;
  public currentPage = 1;
  public currentStartedPage: number;
  private sortType = 'ASC';
  checkDisplay = 'list';
  private abstractSearchParams = {
    'searchText': null,
    'sort': '',
    'sortDirection': 'ASC',
    'page': this.currentPage,
    'pageSize': 30
  };

  constructor(private http: HttpService, private formBuilder: FormBuilder) {
    this.typeSelected = this.types[0];
    this.totalEntityRows = 0;
    this.pageSize = 30;
    this.abstractSearchParams.sort = this.typeSelected + 'Name';
    this.searchText = null;
    this.sortColumn = 'AdjustorName';
    this.sortType = 'ASC';
    this.abstractSearchParams.sortDirection = this.sortType;
    this.abstractSearchParams.searchText = this.searchText;
    this.abstractSearchParams.pageSize = this.pageSize;
    this.abstractSearchParams.sort = this.sortColumn;
    this.fetchAdjustors(this.abstractSearchParams);
    this.adjustorForm = this.formBuilder.group({
      adjustorId: [null],
      adjustorName: [null, Validators.compose([Validators.required])],
      address1: [null],
      address2: [null],
      city: [null],
      state: [null],
      stateId: [null],
      postalCode: [null],
      phoneNumber: [null],
      faxNumber: [null],
      emailAddress: [null, Validators.compose([Validators.email])],
      extension: [null]
    });
    this.attorneyForm = this.formBuilder.group({
      attorneyId: [null],
      attorneyName: [null, Validators.compose([Validators.required])],
      address1: [null],
      address2: [null],
      city: [null],
      state: [null], // placeholder for drop-down.
      stateId: [null],
      postalCode: [null],
      phoneNumber: [null],
      faxNumber: [null]
    });
    this.payorForm = this.formBuilder.group({
      payorId: [null],
      groupName: [null, Validators.compose([Validators.required])],
      billToName: [null, Validators.compose([Validators.required])],
      billToAddress1: [null],
      billToAddress2: [null],
      billToCity: [null],
      billToStateName: [null],
      billToStateId: [null],
      billToPostalCode: [null],
      phoneNumber: [null],
      alternatePhoneNumber: [null],
      faxNumber: [null],
      notes: [null],
      contact: [null],
      letterName: [null, Validators.compose([Validators.required])]
    });
  }

  editStatus() {
    return this.editFlag;
  }

  getReferencesList() {
    this.abstractSearchParams.searchText = this.searchText;
    this.abstractSearchParams.pageSize = this.pageSize;
    this.abstractSearchParams.page = this.currentPage;
    this.abstractSearchParams.sortDirection = this.sortType;
    this.abstractSearchParams.sort = this.sortColumn;
    if (this.typeSelected === 'Adjustor') {
      this.fetchAdjustors(this.abstractSearchParams);
    } else if (this.typeSelected === 'Attorney') {
      this.fetchAttorneys(this.abstractSearchParams);
    } else if (this.typeSelected === 'Payor') {
      this.fetchPayors(this.abstractSearchParams);
    }
  }

  onSortColumn($event: { column: string; dir: string; }) {
    this.sortColumn = $event.column;
    this.sortType = $event.dir;
    this.getReferencesList();
  }

  setSearchText(key: any): void {
    this.searchText = key;
  }

  setSortType(option: any): void {
    if (this.sortType !== option) {
      this.sortType = option;
      this.getReferencesList();
    }
  }

  fetchAdjustors(data: any): void {
    this.loading = true;
    this.http.getReferencesAdjustorsList(data)
      .subscribe((result: any) => {
          this.adjustors = result.results;
          this.totalEntityRows = result.totalRows;
          this.loading = false;
        }, error => {
          this.loading = false;
        }
      );
  }

  fetchPayors(data: any): void {
    this.loading = true;
    this.http.getReferencesPayorsList(data)
      .subscribe((result: any) => {
        this.payors = result.results;
        this.totalEntityRows = result.totalRowCount;
        this.loading = false;
      }, error => {
        this.loading = false;
      });
  }

  fetchAttorneys(data: any): void {
    this.loading = true;
    this.http.getReferencesAttorneysList(data)
      .subscribe((result: any) => {
          this.attorneys = result.results;
          this.totalEntityRows = result.totalRows;
          this.loading = false;
        }, error => {
          this.loading = false;
        }
      );
  }

  getPayors(): PayorItem[] {
    return this.payors;
  }

  getAdjustors(): AdjustorItem[] {
    return this.adjustors;
  }

  getAttorneys(): AttorneyItem[] {
    return this.attorneys;
  }

  get totalEntities(): number {
    return this.totalEntityRows;
  }

  getTotalRows(): number {
    this.totalRows = this.totalEntityRows / this.pageSize;
    return Math.floor(this.totalRows);
  }

  getLastPage(): number {
    this.totalRows = this.totalEntityRows / this.pageSize;
    return Math.ceil(this.totalRows);
  }

  getCurrentStartPage() {
    this.currentStartedPage = ((this.currentPage - 1) * this.pageSize) + 1;
    return Math.floor(this.currentStartedPage);
  }

  openModal(isModalEdit: boolean): void {
    this.editFlag = isModalEdit;
    $('#modalAddAdjustor').modal('show');
    $('#adjustorPhoneNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.adjustorForm.controls.phoneNumber.setValue(val);
    });
    $('#adjustorFaxNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.adjustorForm.controls.faxNumber.setValue(val);
    });
    $('#attorneyPhoneNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.attorneyForm.controls.phoneNumber.setValue(val);
    });
    $('#attorneyFaxNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.attorneyForm.controls.faxNumber.setValue(val);
    });
    $('#payorPhoneNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.payorForm.controls.faxNumber.setValue(val);
    });
    $('#payorAlternatePhoneNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.payorForm.controls.alternatePhoneNumber.setValue(val);
    });
    $('#payorFaxNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.payorForm.controls.faxNumber.setValue(val);
    });
    if (this.editFlag === true) {
      // Adjustors
      if (this.typeSelected === this.types[0]) {
        this.adjustorForm.controls.adjustorId.setValue(this.editedEntity.adjustorId);
        this.adjustorForm.controls.adjustorName.setValue(this.editedEntity.adjustorName);
        this.adjustorForm.controls.address1.setValue(this.editedEntity.address1);
        this.adjustorForm.controls.address2.setValue(this.editedEntity.address2);
        this.adjustorForm.controls.city.setValue(this.editedEntity.city);
        const adjustorState = this.states.find(st => st.stateName === this.editedEntity.stateName);
        if (adjustorState) {
          this.adjustorForm.controls.state.setValue(adjustorState.stateName);
          this.adjustorForm.controls.stateId.setValue(adjustorState.stateId);
        }
        this.adjustorForm.controls.postalCode.setValue(this.editedEntity.postalCode);
        this.adjustorForm.controls.phoneNumber.setValue(this.editedEntity.phoneNumber);
        this.adjustorForm.controls.faxNumber.setValue(this.editedEntity.faxNumber);
        this.adjustorForm.controls.emailAddress.setValue(this.editedEntity.emailAddress);
        this.adjustorForm.controls.extension.setValue(this.editedEntity.extension);
        // Attorneys
      } else if (this.typeSelected === this.types[1]) {
        this.attorneyForm.controls.attorneyId.setValue(this.editedEntity.attorneyId);
        this.attorneyForm.controls.attorneyName.setValue(this.editedEntity.attorneyName);
        this.attorneyForm.controls.address1.setValue(this.editedEntity.address1);
        this.attorneyForm.controls.address2.setValue(this.editedEntity.address2);
        this.attorneyForm.controls.city.setValue(this.editedEntity.city);
        const attorneyState = this.states.find(st => st.stateName === this.editedEntity.stateName);
        if (attorneyState) {
          this.attorneyForm.controls.state.setValue(attorneyState.stateName);
          this.attorneyForm.controls.stateId.setValue(attorneyState.stateId);
        }
        this.attorneyForm.controls.postalCode.setValue(this.editedEntity.postalCode);
        this.attorneyForm.controls.phoneNumber.setValue(this.editedEntity.phoneNumber);
        this.attorneyForm.controls.faxNumber.setValue(this.editedEntity.faxNumber);
        // Payors
      } else if (this.typeSelected === this.types[2]) {
        this.payorForm.controls.payorId.setValue(this.editedEntity.payorId);
        this.payorForm.controls.groupName.setValue(this.editedEntity.groupName);
        this.payorForm.controls.billToName.setValue(this.editedEntity.billToName);
        this.payorForm.controls.billToAddress1.setValue(this.editedEntity.billToAddress1);
        this.payorForm.controls.billToAddress2.setValue(this.editedEntity.billToAddress2);
        this.payorForm.controls.billToCity.setValue(this.editedEntity.billToCity);
        this.payorForm.controls.billToStateName.setValue(this.editedEntity.billToStateName);
        const payorState = this.states.find(st => st.stateName === this.editedEntity.billToStateName);
        if (payorState) {
          this.payorForm.controls.billToStateName.setValue(payorState.stateName);
          this.payorForm.controls.billToStateId.setValue(payorState.stateId);
        }
        this.payorForm.controls.billToPostalCode.setValue(this.editedEntity.billToPostalCode);
        this.payorForm.controls.phoneNumber.setValue(this.editedEntity.phoneNumber);
        this.payorForm.controls.alternatePhoneNumber.setValue(this.editedEntity.alternatePhoneNumber);
        this.payorForm.controls.faxNumber.setValue(this.editedEntity.faxNumber);
        this.payorForm.controls.notes.setValue(this.editedEntity.notes);
        this.payorForm.controls.contact.setValue(this.editedEntity.contact);
        this.payorForm.controls.letterName.setValue(this.editedEntity.letterName);
      }
    }
  }
}
