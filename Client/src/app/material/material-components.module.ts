import {MatButtonModule, MatFormFieldModule, MatSelectModule, MatInputModule } from '@angular/material';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import {NgModule} from '@angular/core';
@NgModule({
  imports: [MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatAutocompleteModule],
  exports: [MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatAutocompleteModule],
  //providers: [{provide: MAT_AUTOCOMPLETE_DEFAULT_OPTIONS, useValue: { autoActiveFirstOption: false }}]
})
export class MaterialComponentsModule {
}
