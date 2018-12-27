import {AfterViewInit, Component, NgZone, OnInit} from '@angular/core';
import {ReferenceManagerService} from '../../services/reference-manager.service';
import {ToastsManager} from 'ng2-toastr';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

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
  submitted = false;
  public flag = 'File Name';
  form: FormGroup;

  constructor(public rs: ReferenceManagerService,
              private formBuilder: FormBuilder,
              private toast: ToastsManager) {
    this.form = this.formBuilder.group({
      adjustorName: [null, Validators.compose([Validators.required])],
      faxNumber: [null, Validators.compose([Validators.required])],
      phoneNumber: [null],
      emailAddress: [null, Validators.compose([Validators.required])],
      extension: [null, Validators.compose([Validators.required])]
    });


  }


  ngOnInit() {

  }

  ngAfterViewInit() {
    $('#phoneNumber').inputmask().on('change', (ev) => {

      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.form.controls.phoneNumber.setValue(val);
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

    if (this.form.valid) {

      console.log(this.form.value);

      $('#modalAddAdjustor').modal('hide');
    } else {
      this.toast.warning('Invalid field value(s). Please correct to proceed.');
    }

  }

  changeSelection(event) {
    this.rs.typeSelected = event.value;
    this.rs.sortColumn = event.value.toLowerCase() + 'Name';
    this.rs.getadjustorslist();
  }
}

export interface Type {
  value: string;
  viewValue: string;
}
