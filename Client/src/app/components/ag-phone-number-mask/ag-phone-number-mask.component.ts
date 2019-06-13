import {AfterViewInit, Component, ViewChild, ViewContainerRef} from "@angular/core";

import {ICellEditorAngularComp} from "ag-grid-angular";

@Component({
    selector: 'numeric-cell',
    template: `<input #input (keyup)="onKeyDown($event)" [(ngModel)]="value" style="width: 100%">`
})
export class AgPhoneNumberMaskComponent implements ICellEditorAngularComp, AfterViewInit {
    private params: any;
    public value: any;
    private cancelBeforeStart: boolean = false;

    @ViewChild('input', {read: ViewContainerRef}) public input;


    agInit(params: any): void {
        this.params = params;
        this.value = this.params.value;

        // only start edit if key pressed is a number, not a letter
        this.cancelBeforeStart = params.charPress && ('1234567890'.indexOf(params.charPress) < 0);
    }

    getValue(): any {
        return this.value;
    }

    isCancelBeforeStart(): boolean {
        return this.cancelBeforeStart;
    }

    onKeyDown(event): void {
        const x = (this.value).replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
        this.value = !x[2] ? x[1] : '(' + x[1] + ') ' + x[2] + (x[3] ? '-' + x[3] : '');
    }

    // dont use afterGuiAttached for post gui events - hook into ngAfterViewInit instead for this
    ngAfterViewInit() {
        window.setTimeout(() => {
            this.input.element.nativeElement.focus();
        })
    }
}
