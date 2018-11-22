import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AutoCompleteComponent } from './auto-complete.component';
import { AutoCompleteDirective } from './auto-complete.directive';

@NgModule({
  imports: [CommonModule, FormsModule],
  declarations: [AutoCompleteComponent, AutoCompleteDirective],
  exports:  [AutoCompleteComponent, AutoCompleteDirective],
  entryComponents: [AutoCompleteComponent]
})
export class AutoCompleteModule {
  static forRoot() {
    return {
      ngModule: AutoCompleteModule,
      providers: []// [{provide: AutoComplete, useValue: window['AutoComplete']}]
    };
  }
 }
