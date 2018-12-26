import {MatButtonModule, MatFormFieldModule, MatSelectModule, MatInputModule} from '@angular/material';
import {NgModule} from '@angular/core';

@NgModule({
  imports: [MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule],



  exports: [MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule],
})
export class MaterialComponentsModule {
}
