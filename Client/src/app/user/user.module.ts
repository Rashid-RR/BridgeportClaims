import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UserRoutes } from './user.routing';
import { UsersComponent } from './list/users.component';
import { SharedModule } from '../shared';

@NgModule({
  imports: [
    SharedModule,
    CommonModule,
    RouterModule.forChild(UserRoutes),
  ],
  declarations: [
    UsersComponent
  ]
})
export class UserModule { }
