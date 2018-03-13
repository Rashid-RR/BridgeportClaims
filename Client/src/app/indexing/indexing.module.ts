import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AutoCompleteModule } from '../auto-complete';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { IndexingRoutes } from "./indexing.routing"
import { UnindexedImageComponent } from './unindex-image/unindex-image.component';
import { UnindexedInvoiceComponent } from './unindexed-invoice/unindexed-invoice.component';

import { UnindexedImageListComponent } from './unindex-image-list/unindex-image-list.component';
import { UnindexedImageFilterComponent } from './unindex-image-filter/unindex-image-filter.component';
import { UnindexedImageFileComponent } from './unindexed-image-file/unindexed-image-file.component';
import { UnindexedImageFileListComponent } from './unindexed-image-file-list/unindexed-image-file-list.component';

import { UnindexedInvoiceListComponent } from './unindexed-invoice-list/unindexed-invoice-list.component';
import { UnindexedInvoiceFilterComponent } from './unindexed-invoice-filter/unindexed-invoice-filter.component';
import { UnindexedInvoiceFileListComponent } from './unindexed-invoice-file-list/unindexed-invoice-file-list.component';

import { IndexFileComponent } from './index-file/index-file.component';
import { ShContextMenuModule } from 'ng2-right-click-menu';
import { ImagesInvoiceComponent } from './images-invoice/images-invoice.component';

@NgModule({
  imports: [
    ShContextMenuModule,
    CommonModule,AutoCompleteModule,
    FormsModule, ReactiveFormsModule,
    RouterModule.forChild(IndexingRoutes),
  ],
  declarations: [
    UnindexedImageFileComponent,UnindexedImageFileListComponent,IndexFileComponent,UnindexedImageComponent,UnindexedImageFilterComponent,UnindexedImageListComponent, ImagesInvoiceComponent,
    UnindexedInvoiceFileListComponent,UnindexedInvoiceFilterComponent,UnindexedInvoiceListComponent,UnindexedInvoiceFileListComponent,UnindexedInvoiceComponent
  ]
})
export class IndexingModule { }
