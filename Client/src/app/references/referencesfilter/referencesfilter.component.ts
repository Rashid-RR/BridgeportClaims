import {AfterViewInit, Component, NgZone, OnInit} from '@angular/core';
import {ReferenceManagerService} from '../../services/reference-manager.service';
import {ToastsManager} from 'ng2-toastr';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
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
  states: any = [];
  selectedState: any = {};
  selectedStateId: string;
  submitted = false;
  public flag = 'File Name';
  form: FormGroup;
  attornyform: FormGroup;

  constructor(public rs: ReferenceManagerService,
              private formBuilder: FormBuilder,
              private http: HttpService,
              private toast: ToastsManager) {
    this.form = this.formBuilder.group({
      adjustorId: [null, Validators.compose([])],
      adjustorName: [null, Validators.compose([Validators.required])],
      faxNumber: [null, Validators.compose([])],
      phoneNumber: [null],
      emailAddress: [null, Validators.compose([])],
      extension: [null, Validators.compose([])]
    });
    this.attornyform = this.formBuilder.group({
      attorneyId: [null, Validators.compose([])],
      attorneyName: [null, Validators.compose([Validators.required])],
      address2: [null, Validators.compose([])],
      address1: [null, Validators.compose([])],
      city: [null, Validators.compose([])],
      postalCode: [null, Validators.compose([])],
      stateName: [null, Validators.compose([])],
      stateId: [null, Validators.compose([])],
      extension: [null, Validators.compose([])],
      faxNumber: [null, Validators.compose([])],
      phoneNumber: [null],


    });
    this.http.getstates({}).subscribe(data => {
      this.states = data;
      this.selectedState = this.states[0]
    }, error1 => {
      //console.log(error1)
    })
  }

  ngOnInit() {

  }

  ngAfterViewInit() {
    $('#phoneNumber').inputmask().on('change', (ev) => {

      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.form.controls.phoneNumber.setValue(val);
    });

    $('#at_phoneNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      {
        debugger
      }

      this.attornyform.controls.phoneNumber.setValue(val);
    });
  }

  search() {

    this.rs.setSearchText(this.adjustorName);
    this.rs.getadjustorslist();
  }

  filter($event) {
  }

  clearFilters() {
    this.adjustorName = '';
    this.rs.setSearchText(null);
    this.rs.getadjustorslist();
  }

  addadjustor() {

    if (this.rs.editStatus() === true) {
      if (this.rs.typeSelected === this.rs.types[0]) {
        if (!this.form.dirty) {
          this.toast.warning('you have not made any changes yet');

        } else if (!this.form.valid) {
          this.toast.warning('Invalid field value(s). Please correct to proceed.');


        } else {
          this.rs.loading = true;

          this.http.updateadjustor(this.form.value).subscribe(data => {
            this.toast.success('Record successfully updated.');

            this.rs.loading = false;

            this.form.reset();
            this.rs.editFlag = false;
            this.rs.getadjustorslist();
            $('#modalAddAdjustor').modal('hide');

          }, error1 => {
            this.rs.loading = false;
            this.toast.error(error1)
          })

        }
      } else if (this.rs.typeSelected === this.rs.types[1]) {
        this.attornyform.controls.stateId.setValue(this.selectedState.stateId);
        this.attornyform.controls.stateName.setValue(this.selectedState.stateName);

        if (this.rs.editAdjustor.stateName === this.selectedState.stateName && !this.attornyform.dirty) {
          this.toast.error('you have not made any changes yet')
        } else if (!this.attornyform.valid) {
          this.toast.warning('Invalid field value(s). Please correct to proceed.');


        } else {
          this.rs.loading = true;

          this.http.updateattorney(this.attornyform.value).subscribe(data => {

            this.toast.success('Record successfully updated.');
            this.rs.editFlag = false;
            this.rs.loading = false;


            this.form.reset();
            this.rs.getadjustorslist();
            $('#modalAddAdjustor').modal('hide');

          }, error1 => {
            this.toast.error(error1)
          })

        }
      }


    } else {
      if (this.rs.typeSelected === this.rs.types[0]) {
        if (!this.form.dirty) {
          this.toast.warning('you have not made any changes yet');

        } else if (!this.form.valid) {
          this.toast.warning('Invalid field value(s). Please correct to proceed.');


        } else {
          this.rs.loading = true;

          this.http.insertadjustor(this.form.value).subscribe(data => {

            this.toast.success('Added successfully.');

            this.rs.loading = false;

            this.form.reset();
            this.rs.getadjustorslist();

            $('#modalAddAdjustor').modal('hide');

          }, error1 => {
            //console.log(error1)
          })
        }
      } else if (this.rs.typeSelected === this.rs.types[1]) {
        if (!this.attornyform.dirty) {
          this.toast.warning('you have not made any changes yet');

        } else if (!this.attornyform.valid) {
          this.toast.warning('Invalid field value(s). Please correct to proceed.');


        } else {
          if (this.selectedStateId) {
            this.attornyform.controls.stateId.setValue(this.selectedStateId);

            this.rs.loading = true;

            this.http.insertattorney(this.attornyform.value).subscribe(data => {

              this.toast.success('Added successfully.');

              this.form.reset();
              this.rs.loading = false;

              this.rs.getadjustorslist();

              $('#modalAddAdjustor').modal('hide');

            }, error1 => {
              //console.log(error1)
            })
          } else {
            this.toast.error('Please select state!')
          }

        }
      }
    }


  }

  openModel() {

    $('#modalAddAdjustor').modal('show');
    $('#phoneNumber').inputmask().on('change', (ev) => {

      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.form.controls.phoneNumber.setValue(val);
    });

    $('#at_phoneNumber').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.attornyform.controls.phoneNumber.setValue(val);
    });

    if (this.rs.editFlag === true) {
      if (this.rs.typeSelected === this.rs.types[0]) {
        this.form.controls.adjustorId.setValue(this.rs.editAdjustor.adjustorId);
        this.form.controls.adjustorName.setValue(this.rs.editAdjustor.adjustorName);
        this.form.controls.phoneNumber.setValue(this.rs.editAdjustor.phoneNumber);
        this.form.controls.emailAddress.setValue(this.rs.editAdjustor.emailAddress);
        this.form.controls.extension.setValue(this.rs.editAdjustor.extension);
        this.form.controls.faxNumber.setValue(this.rs.editAdjustor.faxNumber);

      } else if (this.rs.typeSelected === this.rs.types[1]) {

        this.attornyform.controls.attorneyId.setValue(this.rs.editAdjustor.attorneyId);
        this.attornyform.controls.attorneyName.setValue(this.rs.editAdjustor.attorneyName);
        this.attornyform.controls.phoneNumber.setValue(this.rs.editAdjustor.phoneNumber);
        this.attornyform.controls.extension.setValue(this.rs.editAdjustor.extension);
        this.attornyform.controls.faxNumber.setValue(this.rs.editAdjustor.faxNumber);
        this.attornyform.controls.city.setValue(this.rs.editAdjustor.city);
        this.attornyform.controls.address1.setValue(this.rs.editAdjustor.address1);
        this.attornyform.controls.address2.setValue(this.rs.editAdjustor.address2);
        this.attornyform.controls.stateName.setValue(this.rs.editAdjustor.stateName);
        this.attornyform.controls.postalCode.setValue(this.rs.editAdjustor.postalCode);
        const index = this.states.findIndex(state => state.stateName = this.rs.editAdjustor.stateName);
        this.selectedState = this.states[index];

      }

    }


  }

  changeSelection(event) {
    this.rs.typeSelected = event.value;
    this.rs.sortColumn = event.value.toLowerCase() + 'Name';
    this.rs.getadjustorslist();
  }

  changestate(event) {
    //console.log(event);
    this.selectedStateId = event.value.stateId;
    this.attornyform.controls.stateName.setValue(event.value.stateName);

  }
}

export interface Type {
  value: string;
  viewValue: string;
}
