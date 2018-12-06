import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClientModule } from '@angular/common/http';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { MatInputModule,MatSelectModule, MatFormFieldModule, MatListModule, MatMenuModule, MatToolbarModule, MatIconModule, MatProgressSpinnerModule, MatButtonModule, MatChipsModule, MatDividerModule, MatGridListModule, MatCheckboxModule, MatCardModule, MatDialogModule, MatRadioModule } from '@angular/material';

@NgModule({
  imports: [
    CommonModule,
    NgxDatatableModule,
    FormsModule, ReactiveFormsModule,
    NgbModule.forRoot(),
    HttpClientModule,
    MatRadioModule,MatInputModule,MatSelectModule, MatFormFieldModule, MatListModule, MatMenuModule, MatToolbarModule, MatIconModule, MatProgressSpinnerModule, MatButtonModule, MatChipsModule, MatDividerModule, MatGridListModule, MatCheckboxModule, MatCardModule, MatDialogModule
  ],
  exports: [
    NgbModule,
    FormsModule, ReactiveFormsModule,
    NgxDatatableModule,
    HttpClientModule,
    MatRadioModule,MatInputModule,MatSelectModule, MatFormFieldModule, MatListModule, MatMenuModule, MatToolbarModule, MatIconModule, MatProgressSpinnerModule, MatButtonModule, MatChipsModule, MatDividerModule, MatGridListModule, MatCheckboxModule, MatCardModule, MatDialogModule
  ],
  declarations: [
  ],
  schemas: [NO_ERRORS_SCHEMA],
  entryComponents: [

  ],
})
export class SharedModule { }