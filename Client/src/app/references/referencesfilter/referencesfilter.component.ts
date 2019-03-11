import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { ReferenceManagerService } from '../../services/reference-manager.service';
import { ToastrService } from 'ngx-toastr';
import { HttpService } from '../../services/http-service';
import { UsState } from '../../models/us-state';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { MatAutocomplete, MatAutocompleteSelectedEvent } from '@angular/material';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

declare var $: any;

@Component({
  selector: 'app-referencesfilter',
  templateUrl: './referencesfilter.component.html',
  styleUrls: ['./referencesfilter.component.css']
})
export class ReferencesfilterComponent implements OnInit, AfterViewInit {
  @ViewChild(MatAutocomplete) matAutocomplete: MatAutocomplete;
  adjustorStateControl = new FormControl();
  attorneyStateControl = new FormControl();
  public payorStateControl = new FormControl();
  date: string;
  public entitySearchName: string;
  adjustorModel: any = {};
  submitted = false;
  public flag = 'File Name';
  adjustorFilteredStates: Observable<UsState[]>;
  attorneyFilteredStates: Observable<UsState[]>;
  payorFilteredStates: Observable<UsState[]>;

  constructor(public rs: ReferenceManagerService,
    private http: HttpService,
    private route: ActivatedRoute,
    private toast: ToastrService) {
    this.http.getStates({}).subscribe(data => {
      this.rs.states = data;
    }, error => {
    });
  }

  private _filter(value: string): UsState[] {
    if (!isNaN(Number(value))) {
      return this.rs.states;
    }
    const filterValue = (value || '').toUpperCase(); // handle undefined that comes from the edit
    return this.rs.states.filter(option => option.stateName.toUpperCase().indexOf(filterValue) === 0);
  }

  ngOnInit() {
    this.route.queryParams.subscribe(queryOptions => {
      if (queryOptions['payorId']) {
        this.rs.payorId = Number(queryOptions['payorId']);
        this.rs.typeSelected = this.rs.types[2];
        this.rs.sortColumn = ('Payor'.toLowerCase() === 'payor' ? 'group' : 'Payor'.toLowerCase()) + 'Name';
        this.rs.getReferencesList();
      } else if (queryOptions['adjustorId']) {
        this.rs.adjustorId = Number(queryOptions['adjustorId']);
        this.rs.typeSelected = this.rs.types[0];
        this.rs.sortColumn = 'adjustorName';
        this.rs.getReferencesList();
      } else if (queryOptions['attorneyId']) {
        this.rs.attorneyId = Number(queryOptions['attorneyId']);
        this.rs.typeSelected = this.rs.types[1];
        this.rs.sortColumn = 'attorneyName';
        this.rs.getReferencesList();
      }
    });
    this.adjustorFilteredStates = this.adjustorStateControl.valueChanges.pipe(
      startWith(''),
      map(val => this._filter(val))
    );
    this.attorneyFilteredStates = this.attorneyStateControl.valueChanges.pipe(
      startWith(''),
      map(val => this._filter(val))
    );
    this.payorFilteredStates = this.payorStateControl.valueChanges.pipe(
      startWith(''),
      map(val => this._filter(val))
    );
  }

  public valueMapper = (key) => {
    const selection = this.rs.states.find(x => x.stateName === key);
    if (selection) {
      return selection.stateName;
    }
  }

  // This doesn't seem to work.
  onSelectionChanged(event: MatAutocompleteSelectedEvent) {
    const state = event.option.value;
    if (state) {
      this.rs.payorForm.get('billToStateName').setValue(state.toUpperCase());
    }
  }

  onAdjustorStateSelection(stateId: number) {
    const selected = this.rs.states.find(st => st.stateId === stateId);
    if (selected) {
      this.rs.adjustorForm.patchValue({ state: selected.stateName.toUpperCase(), stateId: selected.stateId });
    }
  }

  onAttorneyStateSelection(stateId: number) {
    const selected = this.rs.states.find(option => option.stateId === stateId);
    if (selected) {
      this.rs.attorneyForm.patchValue({ state: selected.stateName.toUpperCase(), stateId: selected.stateId });
    }
  }

  onPayorStateSelection(stateId: number) {
    const selected = this.rs.states.find(option => option.stateId === stateId);
    if (selected) {
      this.rs.payorForm.patchValue({ billToStateName: selected.stateName.toUpperCase(), billToStateId: selected.stateId });
    }
  }

  ngAfterViewInit() {
    $('#adjustorPhoneNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.rs.adjustorForm.controls.phoneNumber.setValue(val);
    });
    $('#adjustorFaxNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.rs.adjustorForm.controls.faxNumber.setValue(val);
    });
    $('#attorneyPhoneNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      {
      }
      this.rs.attorneyForm.controls.phoneNumber.setValue(val);
    });
    $('#attorneyFaxNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      {
      }
      this.rs.attorneyForm.controls.faxNumber.setValue(val);
    });
    $('#payorPhoneNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      {
      }
      this.rs.payorForm.controls.phoneNumber.setValue(val);
    });
    $('#payorAlternatePhoneNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      {
      }
      this.rs.payorForm.controls.alternatePhoneNumber.setValue(val);
    });
    $('#payorFaxNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      {
      }
      this.rs.payorForm.controls.faxNumber.setValue(val);
    });
  }

  search() {
    this.rs.setSearchText(this.entitySearchName);
    this.rs.getReferencesList();
  }

  filter($event: any) {
  }

  clearFilters() {
    this.entitySearchName = '';
    this.rs.setSearchText(null);
    this.rs.getReferencesList();
  }

  cancel() {
    this.rs.adjustorForm.reset();
    this.rs.attorneyForm.reset();
    this.rs.payorForm.reset();
    this.rs.editFlag = false;
    this.adjustorStateControl.reset();
    this.attorneyStateControl.reset();
    this.payorStateControl.reset();
  }

  private get checkEditMode(): boolean {
    return this.rs.editStatus() === true ? true : false;
  }

  openAdjustorModal(): void {
    if (!this.checkEditMode) {
      this.addAdjustor();
      return;
    }
    if (this.rs.typeSelected === this.rs.types[0]) {
      if (!this.rs.adjustorForm.valid) {
        this.toast.warning('Invalid field value(s). Please correct to proceed.');
      } else {
        this.rs.loading = true;
        this.http.updateAdjustor(this.rs.adjustorForm.value).subscribe(data => {
          this.toast.success('Adjustor successfully updated.');
          this.rs.loading = false;
          this.cancel();
          this.rs.getReferencesList();
          $('#modalAddAdjustor').modal('hide');
        }, error => {
          this.rs.loading = false;
          this.toast.error(error);
        });
      }
    }
  }

  openAttorneyModal(): void {
    if (!this.checkEditMode) {
      this.addAttorney();
      return;
    }
    if (this.rs.typeSelected === this.rs.types[1]) {
      if (!this.rs.attorneyForm.valid) {
        this.toast.warning('Invalid field value(s). Please correct to proceed.');
      } else {
        this.rs.loading = true;
        this.http.updateAttorney(this.rs.attorneyForm.value).subscribe(data => {
          this.toast.success('Record successfully updated.');
          this.cancel();
          this.rs.loading = false;
          this.rs.getReferencesList();
          $('#modalAddAdjustor').modal('hide');
        }, error => {
          this.toast.error(error);
        });
      }
    }
  }

  openPayorModal(): void {
    if (!this.checkEditMode) {
      this.addPayor();
      return;
    }
    if (this.rs.typeSelected === this.rs.types[2]) {
      if (!this.rs.payorForm.valid) {
        this.toast.warning('Invalid field value(s). Please correct to proceed.');
      } else {
        this.rs.loading = true;
        this.http.updatePayor(this.rs.payorForm.value).subscribe(data => {
          this.toast.success('Record successfully updated.');
          this.cancel();
          this.rs.loading = false;
          this.rs.getReferencesList();
          $('#modalAddAdjustor').modal('hide');
        }, (error: string) => {
          this.toast.error(error);
        });
      }
    }
  }

  private addAdjustor(): void {
    if (this.rs.typeSelected === this.rs.types[0]) {
      if (!this.rs.adjustorForm.valid) {
        this.toast.warning('Invalid field value(s). Please correct to proceed.');
      } else {
        this.rs.loading = true;
        this.http.insertAdjustor(this.rs.adjustorForm.value).subscribe(data => {
          this.toast.success('Added successfully.');
          this.rs.loading = false;
          this.cancel();
          this.rs.getReferencesList();
          $('#modalAddAdjustor').modal('hide');
        }, error => {
        });
      }
    }
  }

  private addAttorney(): void {
    if (this.rs.typeSelected === this.rs.types[1]) {
      if (!this.rs.attorneyForm.valid) {
        this.toast.warning('Invalid field value(s). Please correct to proceed.');
      } else {
        this.rs.loading = true;
        this.http.insertAttorney(this.rs.attorneyForm.value).subscribe(data => {
          this.toast.success('Added successfully.');
          this.cancel();
          this.rs.loading = false;
          this.rs.getReferencesList();
          $('#modalAddAdjustor').modal('hide');
        }, error => {
        });
      }
    }
  }

  private addPayor(): void {
    if (this.rs.typeSelected === this.rs.types[2]) {
      if (!this.rs.payorForm.valid) {
        this.toast.warning('Invalid field value(s). Please correct to proceed.');
      } else {
        this.rs.loading = true;
        this.http.insertPayor(this.rs.payorForm.value).subscribe(data => {
          this.toast.success('Added successfully.');
          this.cancel();
          this.rs.loading = false;
          this.rs.getReferencesList();
          $('#modalAddAdjustor').modal('hide');
        }, error => {
        });
      }
    }
  }

  openModal(isModalEdit: boolean) {
    this.rs.openModal(isModalEdit);
  }

  changeSelection(event: { value: string; }): void {
    this.rs.typeSelected = event.value;
    this.rs.sortColumn = (event.value.toLowerCase() === 'payor' ? 'group' : event.value.toLowerCase()) + 'Name';
    this.rs.getReferencesList();
  }

  displayBillToStateName(usState: UsState) {
    return usState ? usState.stateName : '';
  }
}
