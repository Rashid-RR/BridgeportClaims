import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';

@Component({
    selector: 'app-state-renderer',
    template: `
            <span>
                {{value}}
            </span>
    `
})
export class StateCellRendererComponent implements ICellRendererAngularComp {
  private params: any;
  public value: string;
  refresh(): boolean {
    return false;
  }

  agInit(params: any): void {
    this.params = params;
    this.value = this.params.value;
  }
}
