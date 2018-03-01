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
import { AuthGuard, SignalRService } from './services/services.barrel';
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
import { UnindexedImageFileComponent } from './pages/unindexed-image-file/unindexed-image-file.component';
import { IndexFileComponent } from './pages/index-file/index-file.component';
import { UnindexedImageFileListComponent } from './pages/unindexed-image-file-list/unindexed-image-file-list.component';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { EpisodePageComponent } from 'app/pages/episode-page/episode-page.component';
import { FirewallSettingsComponent } from './pages/firewall-settings/firewall-settings.component';
import { TestComponent } from './pages/test/test.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'main/private',
    pathMatch: 'full'
  },
  {
    path: '404',
    component: Error404Component
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
      },
      {
        path: 'test',
        component: TestComponent
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
        component: MainLayoutComponent,
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        children: [
          {
            path: 'private',
            component: DashboardLinksComponent
          },
          {
            path: 'firewall',
            component: FirewallSettingsComponent
          },
          {
            path: 'profile',
            component: ProfileComponent
          },
          {
            path: 'reports',
            component: ReportComponent,
            children: [
              {
                path: '',
                redirectTo: 'list',
                pathMatch: 'full',
              },
              {
                path: 'list',
                component: ReportListComponent
              },
              {
                path: 'revenue',
                component: ReportSampleComponent
              },
              {
                path: 'account-receivable',
                component: ReportAccountReceivableComponent
              },
              {
                path: 'sample',
                component: ReportSampleComponent
              }
            ]
          },
          {
            path: 'payors',
            component: PayorsComponent
          },
          {
            path: 'indexed-image/:id',
            component: UnindexedImageFileComponent,
          },
          {
            path: 'unindexed-images',
            component: UnindexedImageComponent,
          },
          {
            path: 'unindexed-images/:date',
            component: UnindexedImageComponent,
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
          },
          {
            path: 'episodes',
            component: EpisodePageComponent
          }
        ]
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
