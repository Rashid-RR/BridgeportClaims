import {
  MatButtonModule,
  MatTooltipModule,
  MatFormFieldModule,
  MatSelectModule,
  MatInputModule,
  MAT_AUTOCOMPLETE_DEFAULT_OPTIONS,
  MatDialogModule, MatDividerModule
} from '@angular/material';
import { NgModule } from '@angular/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';

@NgModule({
  imports: [MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatDividerModule,
    MatDialogModule,
    MatTooltipModule,
    MatAutocompleteModule
  ],
  exports: [MatButtonModule,
    MatFormFieldModule,
    MatDividerModule,
    MatSelectModule,
    MatInputModule,
    MatDialogModule,
    MatTooltipModule,
    MatAutocompleteModule],
  providers: [{ provide: MAT_AUTOCOMPLETE_DEFAULT_OPTIONS, useValue: { autoActiveFirstOption: false } }]
})
export class MaterialComponentsModule {
}
