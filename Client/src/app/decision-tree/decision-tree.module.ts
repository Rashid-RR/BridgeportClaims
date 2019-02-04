import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DecisionTreeComponent } from './decision-tree/decision-tree.component';
import { DesignTreeComponent } from './design-tree/design-tree.component';
import { DecisionTreeRoutes } from './decision-tree.routing';
import { SharedModule } from '../shared';
import { RouterModule } from '@angular/router';
import { MaterialComponentsModule } from '../material/material-components.module';
 import {TreeAuthGuard} from "./tree-route.guard"
 
@NgModule({
  imports: [
    CommonModule, SharedModule,MaterialComponentsModule,
    RouterModule.forChild(DecisionTreeRoutes),
  ],
  providers:[TreeAuthGuard],
  declarations: [DecisionTreeComponent, DesignTreeComponent]
})
export class DecisionTreeModule { }
