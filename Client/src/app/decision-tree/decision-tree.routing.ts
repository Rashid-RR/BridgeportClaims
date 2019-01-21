import { Routes } from '@angular/router';
import { DecisionTreeComponent } from './decision-tree/decision-tree.component';
import { DesignTreeComponent } from './design-tree/design-tree.component';
import { TreeListComponent } from './tree-list/tree-list.component';

export const DecisionTreeRoutes: Routes = [
    {
        path: '', component: DecisionTreeComponent,
        children: [
            { path: '', redirectTo: 'list',pathMatch: 'full' },
            { path: 'construct', component: DesignTreeComponent },
            { path: 'list', component: TreeListComponent },
            { path: 'construct/:treeId', component: DesignTreeComponent }

        ]
    }
];