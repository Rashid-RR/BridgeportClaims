import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { ReferenceManagerService } from '../../services/reference-manager.service';
import { ToastrService } from 'ngx-toastr';
import { HttpService } from '../../services/http-service';
import { UsState } from '../../models/us-state';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { MatAutocomplete } from '@angular/material';

declare var $: any;

@Component({
  selector: 'app-referencesfilter',
  templateUrl: './referencesfilter.component.html',
  styleUrls: ['./referencesfilter.component.css']
})
export class ReferencesfilterComponent implements OnInit, AfterViewInit {
  @ViewChild(MatAutocomplete) matAutocomplete: MatAutocomplete;
  date: string;
  adjustorName: string;
  adjustorModel: any = {};
  selectedStateId: string;
  submitted = false;
  public flag = 'File Name';
  filteredStates: Observable<UsState[]>;

  constructor(public rs: ReferenceManagerService,
    private http: HttpService,
    private toast: ToastrService) {
    this.http.getStates({}).subscribe(data => {
      this.rs.states = data;
    }, error => {
    });
  }

  private _filter(value: string): UsState[] {
    if (!this.rs.states || !isNaN(Number(value))) {
      return [];
    }
    const filterValue = (value||'').toLowerCase();    //handle undefined that comes from the edit 
    return this.rs.states.filter(option => option.stateName.toLowerCase().indexOf(filterValue) === 0);
  }

  ngOnInit() {
    this.filteredStates = this.rs.stateControl.valueChanges.pipe(
      startWith(''),
      map(val => this._filter(val))
    );
  }
  onChangeState(event) {
    console.log(event.option)
    let value = event.option.value;
    let selected = this.rs.states.find(option => option.stateName.toLowerCase()===value.toLowerCase());
    this.rs.attorneyForm.patchValue({stateName:value,stateId:selected.stateId}); //patch the state name and stateId to the attorney form
    console.log(this.rs.attorneyForm.value);
  }
  onStateSelection(stateId) {
    let selected = this.rs.states.find(option => option.stateId===stateId);
    this.rs.attorneyForm.patchValue({stateName:selected.stateName,stateId:stateId}); //patch the state name and stateId to the attorney form
    console.log(this.rs.attorneyForm.value);
  }

  ngAfterViewInit() {
    $('#phoneNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.rs.adjustorForm.controls.phoneNumber.setValue(val);
    });
    $('#faxNumber').inputmask().on('change', (ev) => {
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

    this.rs.setSearchText(this.adjustorName);
    this.rs.getReferencesList();
  }

  filter($event) {
  }

  clearFilters() {
    this.adjustorName = '';
    this.rs.setSearchText(null);
    this.rs.getReferencesList();
  }

  cancel() {
    this.rs.adjustorForm.reset();
    this.rs.attorneyForm.reset();
    this.rs.selectedState = 'null';
    this.rs.editFlag = false;
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
            this.toast.error(error)
          });
        }
      } else if (this.rs.typeSelected === this.rs.types[1]) {
        if (this.selectedStateId) {
          this.rs.attorneyForm.get('stateId').setValue(this.selectedStateId);
        }
        if (this.rs.selectedState && this.rs.selectedState.stateName) {
          this.rs.attorneyForm.controls.stateName.setValue(this.rs.selectedState.stateName);
        }
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
          const stateId = this.rs.states.filter(x => x.stateName === this.rs.attorneyForm.get('state').value)[0].stateId;
          if (stateId) {
            this.rs.attorneyForm.controls.stateId.setValue(stateId);
          }
          if (this.selectedStateId) {
            this.rs.attorneyForm.controls.stateId.setValue(this.selectedStateId);
          }
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

  changeSelection(event) {
    this.rs.typeSelected = event.value;
    this.rs.sortColumn = event.value.toLowerCase() + 'Name';
    this.rs.getReferencesList();
  }

  changeState(event) {
    this.selectedStateId = event.value;
    this.rs.attorneyForm.controls.stateName.setValue(event.value.stateName);
    if (this.selectedStateId) {
      this.rs.attorneyForm.get('stateId').setValue(this.selectedStateId);
    }
  }

  chooseFirstOption(): void {
    this.matAutocomplete.options.first.select();
    this.matAutocomplete.options.first.setActiveStyles();
  }

  public bindState(): any {
    return (val) => this.display(val);
  }

  private display(state): string {
    // access component "this" here
    return state ? state.stateName : state;
  }
}
