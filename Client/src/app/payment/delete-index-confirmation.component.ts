import { Component } from '@angular/core';
import { ToastsManager } from 'ng2-toastr';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';
export interface ConfirmModel {
  title: string;
  message: string;
}

@Component({
  selector: 'app-delete-index-confirmation',
  template: `<div class="modal-dialog">
              <div class="modal-content">
                <div class="modal-header">
                  <button type="button" class="close" (click)="close()" >&times;</button>
                  <h3 class="modal-title" style=" font-weight: 700 !important; font-size:16px !important; ">{{title || 'Confirm'}}</h3>
                </div>
                <div class="modal-body">
                  <p>{{message || 'Are you sure?'}}</p>
                  <u><b>Please select an option</b></u>
                  <div class="row">
                    <div class="col-sm-12" style="display:flex;flex-direction:row;padding: 5px 15px;">
                      <div style="width:90px;">
                        <img src="assets/img/payment/icon1.png" class="img-responsive"/>
                      </div>
                      <div>
                          <input class="archivedCheck"  [value]="true" name="confirmcheck" type="radio" id="CancelarchivedCheck" (change)="filter($event)">
                          <label for="CancelarchivedCheck" class="archived"></label>
                          <label for="CancelarchivedCheck">Un-Index Check - Keep all Payments</label>
                      </div>
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-sm-12"  style="display:flex;flex-direction:row;padding: 5px 15px;">
                      <div style="width:90px;">
                        <img src="assets/img/payment/icon2.png" class="img-responsive"/>
                      </div>
                      <div>
                            <input class="archivedCheck" [value]="false" name="confirmcheck" type="radio" id="CancelarchivedCheck1" (change)="filter($event)">
                            <label for="CancelarchivedCheck1" class="archived"></label>
                            <label for="CancelarchivedCheck1">Un-Index Check & Delete all Payments</label>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="modal-footer">
                  <button type="button" class="btn btn-primary" (click)="confirm()">Submit</button>
                  <button type="button" class="btn btn-default" (click)="close()" >Cancel</button>
                </div>
              </div>
            </div>`,
  styles: [`label{font-weight:normal}`]
})
export class DeleteIndexConfirmationComponent extends DialogComponent<ConfirmModel, any> implements ConfirmModel {

  title: string;
  message: string;
  option: any;
  constructor(dialogService: DialogService, private toast: ToastsManager) {
    super(dialogService);
  }
  filter($event) {
    this.option = $event.target.value;
  }
  confirm() {
    if (this.option) {
      this.result = this.option;
      this.close();
    } else {
      this.toast.warning('Please select an option to complete the index deletion.');
    }
  }
}
