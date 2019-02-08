import { Routes } from '@angular/router';
import { DecisionTreeComponent } from './decision-tree/decision-tree.component';
import { DesignTreeComponent } from './design-tree/design-tree.component';
import { TreeListComponent } from './tree-list/tree-list.component';
import { TreeAuthGuard } from './tree-route.guard';
 
export const DecisionTreeRoutes: Routes = [
    {
        path: '', component: DecisionTreeComponent,
        children: [
            { path: '', redirectTo: 'list',pathMatch: 'full' },
            { path: 'construct', component: DesignTreeComponent ,canActivate: [TreeAuthGuard]},
            { path: 'list', component: TreeListComponent,canActivate: [TreeAuthGuard]},
            { path: 'list/:claimId', component: TreeListComponent },
            { path: 'construct/:treeId', component: DesignTreeComponent,canActivate: [TreeAuthGuard] },
            { path: 'experience/:treeId/:claimId', component: DesignTreeComponent }

        ]
    }
];