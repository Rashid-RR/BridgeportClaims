import { NgModule } from '@angular/core';
import { RouterModule, Routes, PreloadAllModules } from '@angular/router';
import { AppLayoutComponent } from './layouts/app-layout.component';
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
import { AuthGuard } from './services/services.barrel';
import { ProfileComponent } from './pages/profile/profile.component';
import { ConfirmEmailComponent } from './pages/confirm-email/confirm-email.component';
import { FileUploadComponent } from './pages/file-upload/file-upload.component';
import { UnpaidScriptComponent } from './pages/unpaid-script/unpaid-script.component';
import { UnindexedImageComponent } from './pages/unindex-image/unindex-image.component';
import { UnindexedImageFileComponent } from './pages/unindexed-image-file/unindexed-image-file.component';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { EpisodePageComponent } from './pages/episode-page/episode-page.component';
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
          { path: 'private', component: DashboardLinksComponent },
          { path: 'firewall', component: FirewallSettingsComponent },
          { path: 'profile', component: ProfileComponent },
          { path: 'reports', loadChildren: './reports/reports.module#ReportsModule' },
          { path: 'payors', component: PayorsComponent },
          { path: 'indexed-image/:id', component: UnindexedImageFileComponent, },
          { path: 'indexing', loadChildren: './indexing/indexing.module#IndexingModule' },
          { path: 'unindexed-images', component: UnindexedImageComponent, },
          { path: 'unindexed-images/:date', component: UnindexedImageComponent, },
          { path: 'unpaid-scripts', component: UnpaidScriptComponent },
          { path: 'users', component: UsersComponent },
          { path: 'claims', component: ClaimsComponent },
          { path: 'payments', loadChildren: './payment/payment.module#PaymentModule' },
          { path: 'fileupload', component: FileUploadComponent },
          { path: 'diary', loadChildren: './diaries/diaries.module#DiariesModule' },
          { path: 'episodes', component: EpisodePageComponent }
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
