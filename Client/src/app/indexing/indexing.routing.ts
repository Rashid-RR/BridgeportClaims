import { Routes } from '@angular/router';
import { ImagesInvoiceComponent } from './images-invoice/images-invoice.component';

export const IndexingRoutes: Routes = [   
    {path: '', component: ImagesInvoiceComponent},
    {path: ':date', component: ImagesInvoiceComponent}
];