import { Routes } from '@angular/router';
import { DecisionTreeComponent } from './decision-tree/decision-tree.component';
import { DesignTreeComponent } from './design-tree/design-tree.component';

export const DecisionTreeRoutes: Routes = [
    {
        path: '', component: DecisionTreeComponent,
        children: [
            { path: '', redirectTo: 'construct',pathMatch: 'full' },
            { path: 'construct', component: DesignTreeComponent }

        ]
    }
];