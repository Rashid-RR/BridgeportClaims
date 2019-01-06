import {Injectable} from '@angular/core';
import {HttpService} from './http-service';
import {adjustorItem} from '../references/dataitems/adjustors';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

declare var $: any;

class UsState {
  stateId: number;
  stateName: string;
}

@Injectable()
export class ReferenceManagerService {
  adjustorForm: FormGroup;
  attorneyForm: FormGroup;
  states: UsState[];
  selectedState: any = {};
  public editFlag = false;
  public loading = false;
  public adjustors: Array<adjustorItem>;
  public totalAdjustors = 0;
  public totalRows = 0;
  public pageSize: number;
  public types = ['Adjustor', 'Attorney'];
  public typeSelected: string;
  public editAdjustor: any;
  private searchText: string;
  public sortColumn: string;
  public currentPage = 1;
  public currentStartedPage: number;
  private sortType = 'ASC';
  checkDisplay = 'list';
  private data = {
    'searchText': null,
    'sort': '',
    'sortDirection': 'ASC',
    'page': this.currentPage,
    'pageSize': 30
  };

  constructor(private http: HttpService, private formBuilder: FormBuilder) {
    this.typeSelected = this.types[0];
    this.totalAdjustors = 0;
    this.pageSize = 30;
    this.data.sort = this.typeSelected + 'Name';
    this.searchText = null;
    this.sortColumn = 'AdjustorName';
    this.sortType = 'ASC';
    this.data.sortDirection = this.sortType;
    this.data.searchText = this.searchText;
    this.data.pageSize = this.pageSize;
    this.data.sort = this.sortColumn;
    this.fetchAdjustors(this.data);
    this.selectedState = 'null';
    this.adjustorForm = this.formBuilder.group({
      adjustorId: [null],
      adjustorName: [null, Validators.compose([Validators.required])],
      faxNumber: [null],
      phoneNumber: [null],
      emailAddress: [null],
      extension: [null]
    });
    this.attorneyForm = this.formBuilder.group({
      attorneyId: [null],
      attorneyName: [null, Validators.compose([Validators.required])],
      address2: [null],
      address1: [null],
      city: [null],
      postalCode: [null],
      state: [null],
      stateName: [null],
      stateId: [null],
      extension: [null],
      faxNumber: [null],
      phoneNumber: [null]
    });
  }

  editStatus() {
    return this.editFlag;
  }

  getReferencesList() {
    this.data.searchText = this.searchText;
    this.data.pageSize = this.pageSize;
    this.data.page = this.currentPage;
    this.data.sortDirection = this.sortType;
    this.data.sort = this.sortColumn;
    if (this.typeSelected === 'Adjustor') {
      this.fetchAdjustors(this.data);
    } else if (this.typeSelected === 'Attorney') {
      this.fetchAttorneys(this.data);
    }
  }

  onSortColumn($event) {
    this.sortColumn = $event.column;
    this.sortType = $event.dir;
    this.getReferencesList();
  }

  setSearchText(key: any) {
    this.searchText = key;
  }

  setSortType(option: any) {
    if (this.sortType !== option) {
      this.sortType = option;
      this.getReferencesList();
    }
  }

  fetchAdjustors(data: any) {
    this.loading = true;
    this.http.getAdjustorName(data)
      .subscribe((result: any) => {
          this.adjustors = result.results;
          this.totalAdjustors = result.totalRows;
          this.loading = false;
        }, error => {
          this.loading = false;
        }
      );
  }


  fetchAttorneys(data: any) {
    this.loading = true;
    this.http.getAttorneyName(data)
      .subscribe((result: any) => {
          this.adjustors = result.results;
          this.totalAdjustors = result.totalRows;
          this.loading = false;
        }, error => {
          this.loading = false;
        }
      );
  }

  getAdjustors() {
    return this.adjustors;
  }

  getTotalAdjustors() {
    return this.totalAdjustors;
  }

  getTotalRows() {
    this.totalRows = this.totalAdjustors / this.pageSize;
    return Math.floor(this.totalRows);
  }

  getCurrentStartPage() {
    this.currentStartedPage = ((this.currentPage - 1) * this.pageSize) + 1;
    return Math.floor(this.currentStartedPage);
  }

  openModal(isModalEdit: boolean) {
    this.editFlag = isModalEdit;
    $('#modalAddAdjustor').modal('show');
    $('#phoneNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.adjustorForm.controls.phoneNumber.setValue(val);
    });
    $('#at_phoneNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.attorneyForm.controls.phoneNumber.setValue(val);
    });
    if (this.editFlag === true) {
      if (this.typeSelected === this.types[0]) {
        this.adjustorForm.controls.adjustorId.setValue(this.editAdjustor.adjustorId);
        this.adjustorForm.controls.adjustorName.setValue(this.editAdjustor.adjustorName);
        this.adjustorForm.controls.phoneNumber.setValue(this.editAdjustor.phoneNumber);
        this.adjustorForm.controls.emailAddress.setValue(this.editAdjustor.emailAddress);
        this.adjustorForm.controls.extension.setValue(this.editAdjustor.extension);
        this.adjustorForm.controls.faxNumber.setValue(this.editAdjustor.faxNumber);
      } else if (this.typeSelected === this.types[1]) {
        this.attorneyForm.controls.attorneyId.setValue(this.editAdjustor.attorneyId);
        this.attorneyForm.controls.attorneyName.setValue(this.editAdjustor.attorneyName);
        this.attorneyForm.controls.phoneNumber.setValue(this.editAdjustor.phoneNumber);
        this.attorneyForm.controls.extension.setValue(this.editAdjustor.extension);
        this.attorneyForm.controls.faxNumber.setValue(this.editAdjustor.faxNumber);
        this.attorneyForm.controls.city.setValue(this.editAdjustor.city);
        this.attorneyForm.controls.address1.setValue(this.editAdjustor.address1);
        this.attorneyForm.controls.address2.setValue(this.editAdjustor.address2);
        this.attorneyForm.controls.stateName.setValue(this.editAdjustor.stateName);
        this.attorneyForm.controls.postalCode.setValue(this.editAdjustor.postalCode);
        const state = this.states.filter(st => st.stateName === this.editAdjustor.stateName)[0];
        console.log(state.stateId);
        this.attorneyForm.get('state').setValue(state.stateId);
        // this.selectedState = this.states[36];
      }
    }
  }
}
