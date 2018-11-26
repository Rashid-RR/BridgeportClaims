
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared';
import { DashboardLinksComponent } from './dashboard-links/dashboard-links.component';
import { ClientViewComponent } from './client-view/client-view.component';
import { DashboardRoutes } from './dashboard.routing';
import { NotificationComponent } from './notification/notification.component';

@NgModule({
  imports: [
      SharedModule,
      CommonModule,
      RouterModule.forChild(DashboardRoutes),
  ],
  declarations: [DashboardLinksComponent, ClientViewComponent,NotificationComponent]
})
export class DashboardModule { }
