import { NgModule } from '@angular/core';
import { RouterModule, Routes, PreloadAllModules } from '@angular/router';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { AppComponent } from './app.component';
// Layouts
import { HeaderComponent } from './layouts/header/header.component';
import { AppLayoutComponent } from './layouts/app-layout.component';
import { SidebarComponent } from './layouts/sidebar/sidebar.component';
// end of layouts
import { LoginComponent } from './pages/login/login.component';
import { DashboardLinksComponent } from './pages/dashboard-links/dashboard-links.component';
import { RegisterComponent } from './pages/register/register.component';
import { MainComponent } from './pages/main/main.component';
import { PasswordResetComponent } from './pages/password-reset/password-reset.component';
import { ChangePasswordComponent } from './pages/change-password/change-password.component';
import { Error404Component } from './pages/error404/error404.component';
import { PayorsComponent } from './pages/payors/payors.component';
import { UsersComponent } from './pages/users/users.component';
import { ClaimsComponent } from './pages/claim/claim.component';
import { AuthGuard,SignalRService } from './services/services.barrel';
import { ProfileComponent } from './pages/profile/profile.component';
import { ConfirmEmailComponent } from './pages/confirm-email/confirm-email.component';
import { FileUploadComponent } from './pages/file-upload/file-upload.component';
import { PaymentComponent } from './pages/payment/payment.component';
import { DiaryComponent } from './pages/diary/diary.component';
import { UnpaidScriptComponent } from './pages/unpaid-script/unpaid-script.component';
import { ReportComponent } from './pages/report/report.component';
import { ReportListComponent } from './pages/report-list/report-list.component';
import { ReportSampleComponent } from './pages/report-sample/report-sample.component';
import { ReportAccountReceivableComponent } from './pages/report-account-receivable/report-account-receivable.component';
import { UnindexedImageComponent } from './pages/unindex-image/unindex-image.component';
import { SignalrDemoComponent } from './pages/signalr-demo/signalr-demo.component';
import { UnindexedImageFileComponent } from './pages/unindexed-image-file/unindexed-image-file.component';
import { IndexFileComponent } from './pages/index-file/index-file.component';
import { UnindexedImageFileListComponent } from './pages/unindexed-image-file-list/unindexed-image-file-list.component';
import { SignalrDocmentDemoComponent } from './pages/signalr-docment-demo/signalr-docment-demo.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'main/private',
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
      }
      , {
        path: 'recover-lost-password',
        component: PasswordResetComponent
      }, {
        path: 'resetpassword',
        component: ChangePasswordComponent
      }
      , {
        path: 'register',
        component: RegisterComponent
      },
      {
        path: 'main',
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        children: [
          {
            path: 'private',
            component: DashboardLinksComponent
          },
          {
            path: 'profile',
            component: ProfileComponent
          },
          {
            path: 'reports',
            component: ReportComponent,
            children:[
              {
                  path: '',
                  redirectTo: 'list',
                  pathMatch: 'full',
              },
              {
                path: 'list',
                component:ReportListComponent
              },
              {
                path: 'revenue',
                component:ReportSampleComponent
              },
              {
                path: 'account-receivable',
                component:ReportAccountReceivableComponent
              },
              {
                path: 'sample',
                component:ReportSampleComponent
              }
            ] 
          },
          {
            path: 'payors',
            component: PayorsComponent
          },
          {
            path: 'signalr-demo',
            component: SignalrDemoComponent,
            /* resolve: {
              connected: SignalRService
            } */
          },{
            path: 'signalr-doc-demo',
            component: SignalrDocmentDemoComponent,
            /* resolve: {
              connected: SignalRService
            } */
          },
          {
            path: 'unindexed-images',
            component:UnindexedImageComponent,            
          },
          {
            path: 'unpaid-scripts',
            component: UnpaidScriptComponent
          },
          {
            path: 'users',
            component: UsersComponent
          },
          {
            path: 'claims',
            component: ClaimsComponent
          },
          {
            path: 'payments',
            component: PaymentComponent
          },
          {
            path: 'fileupload',
            component: FileUploadComponent
          },
          {
            path: 'diary',
            component: DiaryComponent
          }
        ]
      },
      {
        path: '404',
        component: Error404Component
      }
    ]
  },
  { path: 'confirm-email', component: ConfirmEmailComponent },
  { component: Error404Component, path: '**' }
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class RoutingModule {
}
