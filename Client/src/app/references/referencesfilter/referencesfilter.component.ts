import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { ReferenceManagerService } from '../../services/reference-manager.service';
import { ToastrService } from 'ngx-toastr';
import { HttpService } from '../../services/http-service';
import { UsState } from '../../models/us-state';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { MatAutocomplete } from '@angular/material';
import { FormControl } from '@angular/forms';

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
  date: string;
  public entitySearchName: string;
  adjustorModel: any = {};
  submitted = false;
  public flag = 'File Name';
  adjustorFilteredStates: Observable<UsState[]>;
  attorneyFilteredStates: Observable<UsState[]>;

  constructor(public rs: ReferenceManagerService,
    private http: HttpService,
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
    const filterValue = (value || '').toLowerCase(); // handle undefined that comes from the edit
    return this.rs.states.filter(option => option.stateName.toLowerCase().indexOf(filterValue) === 0);
  }

  ngOnInit() {
    this.adjustorFilteredStates = this.adjustorStateControl.valueChanges.pipe(
      startWith(''),
      map(val => this._filter(val))
    );
    this.attorneyFilteredStates = this.attorneyStateControl.valueChanges.pipe(
      startWith(''),
      map(val => this._filter(val))
    );
  }

  onAdjustorStateSelection(stateId: number) {
    const selected = this.rs.states.find(st => st.stateId === stateId);
    this.rs.adjustorForm.patchValue({state: selected.stateName, stateId: selected.stateId});
  }

  onAttorneyStateSelection(stateId: number) {
    const selected = this.rs.states.find(option => option.stateId === stateId);
    this.rs.attorneyForm.patchValue({state: selected.stateName, stateId: selected.stateId});
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
    $('#at_phoneNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      {
      }
      this.rs.attorneyForm.controls.phoneNumber.setValue(val);
    });
    $('#at_faxNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      {
      }
      this.rs.attorneyForm.controls.faxNumber.setValue(val);
    });
  }

  search() {
    this.rs.setSearchText(this.entitySearchName);
    this.rs.getReferencesList();
  }

  filter($event) { }

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
  }

  addAdjustor() {
    if (this.rs.editStatus() === true) {
      if (this.rs.typeSelected === this.rs.types[0]) {
        if (!this.rs.adjustorForm.valid) {
          this.toast.warning('Invalid field value(s). Please correct to proceed.');
        } else {
          this.rs.loading = true;
          this.http.updateAdjustor(this.rs.adjustorForm.value).subscribe(data => {
            this.toast.success('Record successfully updated.');
            this.rs.loading = false;
            this.cancel();
            this.rs.getReferencesList();
            $('#modalAddAdjustor').modal('hide');
          }, error => {
            this.rs.loading = false;
            this.toast.error(error);
          });
        }
      } else if (this.rs.typeSelected === this.rs.types[1]) {
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
    } else {
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
      } else if (this.rs.typeSelected === this.rs.types[1]) {
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
  }

  openModal(isModalEdit: boolean) {
    this.rs.openModal(isModalEdit);
  }

  changeSelection(event: { value: string; }): void {
    this.rs.typeSelected = event.value;
    this.rs.sortColumn = (event.value.toLowerCase() === 'payor' ? 'group' : event.value.toLowerCase()) + 'Name';
    this.rs.getReferencesList();
  }
}
