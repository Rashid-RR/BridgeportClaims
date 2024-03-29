import { Component, ElementRef, Renderer2, AfterViewInit } from '@angular/core';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';
import { Subject } from 'rxjs';
declare var $: any;
export interface ConfirmModel {
  title: string;
  buttonText?: string;
  cancelText?: string;
  customStyle?: string;
  message: string;
  listener?: Subject<any>;
  prescription?: any;
  buttonDisabled?: boolean;
}
@Component({
  selector: 'confirm',
  // tslint:disable-next-line: max-line-length
  template: `<div class="modal-dialog confirmDialog">
  <div class="modal-content" style="padding: 0;">
     <div class="modal-header">
      <div class="modal-title">{{title || 'Confirm'}}</div>
      <img src="assets/images/cross_btn.png" (click)="close()" class="closeConfirmModal close-btn" alt="X" >
     </div>
     <div class="modal-body">
       <p [innerHTML]="msg || 'Are you sure?'"></p>
     </div>
     <div class="modal-footer">
     <button type="button" class="confirm mr-2" mat-raised-button (click)="confirm()"
      [disabled]="buttonDisabled" cdkFocusInitial>{{buttonText ||'OK'}}</button>
     <button type="button" (click)="close()" mat-raised-button class="cancle">{{cancelText ||'Cancel'}}</button>
     </div>
   </div>
</div>`,
styles : [ `.close-btn:{margin: 1.5px 5px !important;}`]
})
export class ConfirmComponent extends DialogComponent<ConfirmModel, boolean> implements ConfirmModel, AfterViewInit {
  title: string;
  customStyle: string;
  message: string;
  buttonText: string;
  cancelText: string;
  listener: Subject<any>;
  buttonDisabled = false;
  prescription: any = {};
  prescriptions: any[] = [];
  constructor(private renderer: Renderer2, private elRef: ElementRef, dialogService: DialogService) {
    super(dialogService);
  }
  ngAfterViewInit() {
    if (this.listener) {
      this.buttonDisabled = true;
      this.renderer.setStyle(this.elRef.nativeElement.parentElement, 'height', 'fit-content');
      $('.modal').css({'background': 'rgba(0,0,0,0)'});
      this.listener.subscribe(r => {
        this.prescriptions = r.prescriptions;
        this.buttonDisabled = r.buttonDisabled;
      });
    } else {
      this.renderer.setStyle(this.elRef.nativeElement.parentElement, 'height', 'auto');
    }
  }
  get msg() {
    return this.message + '<ul>' + (this.prescription && this.prescriptionsIds.length > 0 ? '<li>' : '') +
      this.prescriptionsIds.join('<li>');
  }
  get prescriptionsIds() {
    return (this.prescriptions.filter(p => p.prescriptionId !== this.prescription.prescriptionId).map(p => p.labelName).reduce((accumulator, currValue) => {
      return [...accumulator, ...currValue];
    }, []));
  }
  get style() {
    return this.customStyle;
  }
  confirm() {
    // we set dialog result as true on click on confirm button,
    // then we can get dialog result from caller code
    this.result = true;
    this.close();
  }
}
