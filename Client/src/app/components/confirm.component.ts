import { Component, ElementRef, Renderer2, AfterViewInit } from '@angular/core';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';
import { Subject } from 'rxjs/Subject';
export interface ConfirmModel {
  title: string;
  buttonText?: string;
  customStyle?: string;
  message: string;
  listener?: Subject<any>;
  prescription?: any;
  buttonDisabled?: boolean;
}
@Component({
  selector: 'confirm',
  template: `<div class="modal-dialog">
                <div class="modal-content">
                   <div class="modal-header">
                     <button type="button" class="close" (click)="close()" >&times;</button>
                     <h4 class="modal-title">{{title || 'Confirm'}}</h4>
                   </div>
                   <div class="modal-body">
                     <p [innerHTML]="msg || 'Are you sure?'"></p>
                   </div>
                   <div class="modal-footer">
                     <button type="button" class="btn btn-primary" (click)="confirm()" [disabled]="buttonDisabled">{{buttonText ||'OK'}}</button>
                     <button type="button" class="btn btn-default" (click)="close()" >Cancel</button>
                   </div>
                 </div>
              </div>`
})
export class ConfirmComponent extends DialogComponent<ConfirmModel, boolean> implements ConfirmModel, AfterViewInit {
  title: string;
  customStyle: string;
  message: string;
  buttonText: string;
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
      this.listener.subscribe(r => {
        this.prescriptions = r.prescriptions;
        this.buttonDisabled = r.buttonDisabled;
      });
    } else {
      this.renderer.setStyle(this.elRef.nativeElement.parentElement, 'height', 'auto');
    }
  }
  get msg() {
    return this.message + '<ul>' +
      (this.prescriptions.filter(p => p.prescriptionId !== this.prescription.prescriptionId).map(p => p.labelName).reduce((accumulator, currValue) => {
        return [...accumulator, ...currValue];
      }, [])).join('<li>');
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
