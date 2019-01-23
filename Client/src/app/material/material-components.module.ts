import { MatButtonModule, MatFormFieldModule, MatSelectModule, MatInputModule, MAT_AUTOCOMPLETE_DEFAULT_OPTIONS } from '@angular/material';
import { NgModule } from '@angular/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';

@NgModule({
  imports: [MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatAutocompleteModule
  ],
  exports: [MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatAutocompleteModule],
  providers: [{provide: MAT_AUTOCOMPLETE_DEFAULT_OPTIONS, useValue: { autoActiveFirstOption: false }}]
})
export class MaterialComponentsModule {
}
