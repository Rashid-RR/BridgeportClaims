import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DecisionTreeComponent } from './decision-tree/decision-tree.component';
import { DesignTreeComponent } from './design-tree/design-tree.component';
import { TreeListComponent } from './tree-list/tree-list.component';
import { TreeListFilterComponent } from './tree-list-filter/tree-list-filter.component';
import { TreeListGridComponent } from './tree-list-grid/tree-list-grid.component';
import { DecisionTreeRoutes } from './decision-tree.routing';
import { SharedModule } from '../shared';
import { RouterModule } from '@angular/router';


@NgModule({
  imports: [
    CommonModule, SharedModule,
    RouterModule.forChild(DecisionTreeRoutes),
  ],
  declarations: [DecisionTreeComponent, TreeListGridComponent, TreeListFilterComponent, TreeListComponent, DesignTreeComponent]
})
export class DecisionTreeModule { }
