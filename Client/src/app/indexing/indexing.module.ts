import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
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
import { SharedModule } from '../shared';
import { UnindexedCheckComponent } from './unindexed-check/unindexed-check.component';
import { UnindexedCheckListComponent } from './unindexed-check-list/unindexed-check-list.component';
import { UnindexedCheckFilterComponent } from './unindexed-check-filter/unindexed-check-filter.component';
import { UnindexedInvalidCheckListComponent } from './unindexed-invalid-check-list/unindexed-invalid-check-list.component';
import { UnindexedInvalidCheckFilterComponent } from './unindexed-invalid-check-filter/unindexed-invalid-check-filter.component';

@NgModule({
  imports: [
    SharedModule,
    CommonModule,
    RouterModule.forChild(IndexingRoutes),
  ],
  declarations: [
    UnindexedImageFileComponent,UnindexedImageFileListComponent,IndexFileComponent,UnindexedImageComponent,UnindexedImageFilterComponent,UnindexedImageListComponent, ImagesInvoiceComponent,UnindexedInvalidCheckListComponent,UnindexedInvalidCheckFilterComponent,
    UnindexedInvoiceFileListComponent,UnindexedInvoiceFilterComponent,UnindexedInvoiceListComponent,UnindexedInvoiceFileListComponent,UnindexedInvoiceComponent, UnindexedCheckComponent, UnindexedCheckListComponent, UnindexedCheckFilterComponent
  ]
})
export class IndexingModule { }
