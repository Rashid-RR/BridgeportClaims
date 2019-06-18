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
import {MatCardModule} from '@angular/material/card';
import {MatIconModule} from '@angular/material/icon';

@NgModule({
  imports: [MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatDividerModule,
    MatDialogModule,
    MatTooltipModule,
    MatCardModule,
    MatAutocompleteModule,
    MatIconModule
  ],
  exports: [MatButtonModule,
    MatFormFieldModule,
    MatDividerModule,
    MatSelectModule,
    MatInputModule,
    MatCardModule,
    MatIconModule,
    MatDialogModule,
    MatTooltipModule,
    MatAutocompleteModule],
  providers: [{ provide: MAT_AUTOCOMPLETE_DEFAULT_OPTIONS, useValue: { autoActiveFirstOption: false } }]
})
export class MaterialComponentsModule {
}
