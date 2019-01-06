import {AfterViewInit, Component, OnInit} from '@angular/core';
import {ReferenceManagerService} from '../../services/reference-manager.service';
import {ToastsManager} from 'ng2-toastr';
import {HttpService} from '../../services/http-service';

declare var $: any;

@Component({
  selector: 'app-referencesfilter',
  templateUrl: './referencesfilter.component.html',
  styleUrls: ['./referencesfilter.component.css']
})
export class ReferencesfilterComponent implements OnInit, AfterViewInit {
  date: string;
  adjustorName: string;
  adjustorModel: any = {};
  selectedStateId: string;
  submitted = false;
  public flag = 'File Name';

  constructor(public rs: ReferenceManagerService,
              private http: HttpService,
              private toast: ToastsManager) {
    this.http.getStates({}).subscribe(data => {
      this.rs.states = data;
    }, error => {
    });
  }

  ngOnInit() {

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
            this.rs.adjustorForm.reset();
            this.rs.editFlag = false;
            this.rs.getReferencesList();
            $('#modalAddAdjustor').modal('hide');
          }, error => {
            this.rs.loading = false;
            this.toast.error(error)
          });
        }
      } else if (this.rs.typeSelected === this.rs.types[1]) {
        this.rs.attorneyForm.controls.stateId.setValue(this.rs.selectedState.stateId);
        this.rs.attorneyForm.controls.stateName.setValue(this.rs.selectedState.stateName);
        if (!this.rs.attorneyForm.valid) {
          this.toast.warning('Invalid field value(s). Please correct to proceed.');
        } else {
          this.rs.loading = true;
          this.http.updateAttorney(this.rs.attorneyForm.value).subscribe(data => {
            this.toast.success('Record successfully updated.');
            this.rs.editFlag = false;
            this.rs.loading = false;
            this.rs.adjustorForm.reset();
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
            this.rs.adjustorForm.reset();
            this.rs.getReferencesList();
            $('#modalAddAdjustor').modal('hide');
          }, error => {
          });
        }
      } else if (this.rs.typeSelected === this.rs.types[1]) {
        if (!this.rs.attorneyForm.valid) {
          this.toast.warning('Invalid field value(s). Please correct to proceed.');
        } else {
          if (this.selectedStateId) {
            this.rs.attorneyForm.controls.stateId.setValue(this.selectedStateId);
            this.rs.loading = true;
            this.http.insertAttorney(this.rs.attorneyForm.value).subscribe(data => {
              this.toast.success('Added successfully.');
              this.rs.attorneyForm.reset();
              this.rs.loading = false;
              this.rs.getReferencesList();
              $('#modalAddAdjustor').modal('hide');
            }, error => {
            });
          } else {
            this.toast.error('Please select state.')
          }
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
  }
}

export interface Type {
  value: string;
  viewValue: string;
}
