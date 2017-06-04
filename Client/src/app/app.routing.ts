import {NgModule} from "@angular/core";
import {RouterModule, Routes,PreloadAllModules} from "@angular/router";

import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpModule} from "@angular/http";
import {HashLocationStrategy, LocationStrategy} from "@angular/common";
import {AppComponent} from "./app.component";
//Layouts 
import {HeaderComponent} from "./layouts/header/header.component";
import {AppLayoutComponent} from "./layouts/app-layout.component";
import {SidebarComponent} from "./layouts/sidebar/sidebar.component";
//end of layouts
import {LoginComponent} from "./pages/login/login.component";
import {PrivateComponent} from "./pages/private/private.component";
import {RegisterComponent} from "./pages/register/register.component";
import {MainComponent} from "./pages/main/main.component";
import {PasswordResetComponent} from "./pages/password-reset/password-reset.component";
import {Error404Component} from "./pages/error404/error404.component";
import { PayorsComponent } from './pages/payors/payors.component'
 import {AuthGuard} from "./services/services.barrel";


export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: '',
    component: AppLayoutComponent,
    children: [
      {
        path: 'home',
        component: MainComponent
      },
      {
        path: 'login',
        component: LoginComponent
      }, {
        path: 'recover-lost-password',
        component: PasswordResetComponent
      }, {
        path: 'register',
        component: RegisterComponent      
      },         
      {
        path: 'main',
        canActivate:[AuthGuard],
        children: [
        {
          path: 'private',
          component: PrivateComponent
        },
        {
          path: 'payors',
          component: PayorsComponent
        }
      ]
    }
    ]
  },
  {path: '404', component: Error404Component},
  {path: '**', redirectTo: '/404'}
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class RoutingModule {
}
