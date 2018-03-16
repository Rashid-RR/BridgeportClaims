import { Routes } from '@angular/router';
import { ImagesInvoiceComponent } from './images-invoice/images-invoice.component';
import { UnindexedImageFileComponent } from './unindexed-image-file/unindexed-image-file.component';

export const IndexingRoutes: Routes = [   
    {path: '', component: ImagesInvoiceComponent},
    {path: ':date', component: ImagesInvoiceComponent},
    {
        path: 'indexed-image/:id',
        component: UnindexedImageFileComponent,
      },
];