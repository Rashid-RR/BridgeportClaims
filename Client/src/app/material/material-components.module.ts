import { MatButtonModule, MatFormFieldModule, MatSelectModule, MatInputModule, MAT_AUTOCOMPLETE_SCROLL_STRATEGY } from '@angular/material';
import { MAT_AUTOCOMPLETE_DEFAULT_OPTIONS } from '@angular/material/autocomplete';

import { NgModule } from '@angular/core';

@NgModule({
  imports: [MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule],
  providers: [
    { provide: MAT_AUTOCOMPLETE_DEFAULT_OPTIONS, useValue: { autoActiveFirstOption: false } }
  ],
  exports: [MatButtonModule,
    MatFormFieldModule,

    MatSelectModule,
    MatInputModule],
})
export class MaterialComponentsModule {
}
