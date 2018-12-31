import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DecisionTreeComponent } from './decision-tree/decision-tree.component';
import { DesignTreeComponent } from './design-tree/design-tree.component';
import { DecisionTreeRoutes } from './decision-tree.routing';
import { SharedModule } from '../shared';
import { RouterModule } from '@angular/router';


@NgModule({
  imports: [
    CommonModule,SharedModule,
    RouterModule.forChild(DecisionTreeRoutes),
  ],
  declarations: [DecisionTreeComponent, DesignTreeComponent]
})
export class DecisionTreeModule { }
