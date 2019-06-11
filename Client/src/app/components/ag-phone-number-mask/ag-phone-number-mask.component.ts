import {Component, OnDestroy} from '@angular/core';

import {ICellRendererAngularComp} from 'ag-grid-angular';

@Component({
    selector: 'app-ag-phone-number-mask',
    template: `{{valueMasked()}}`
})
export class AgPhoneNumberMaskComponent implements ICellRendererAngularComp, OnDestroy {
    private params: any;

    agInit(params: any): void {
        this.params = params;
    }

    public valueMasked(): number {
      if(this.params.value) {
        let x = this.params.value.replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
        this.params.value = !x[2] ? x[1] : '(' + x[1] + ') ' + x[2] + (x[3] ? '-' + x[3] : '');
        return this.params.value;
      } else {
        return null;
      }
    }

    ngOnDestroy() {
        // console.log(`Destroying AgPhoneNumberMaskComponent`);
    }

    refresh(): boolean {
        return false;
    }
}
