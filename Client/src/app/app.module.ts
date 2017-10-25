import {BrowserModule, DomSanitizer} from '@angular/platform-browser';
import {NgModule, Pipe, PipeTransform} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HttpModule} from '@angular/http';
import {HashLocationStrategy, LocationStrategy,DatePipe} from '@angular/common';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {ToastModule} from 'ng2-toastr/ng2-toastr';
import {AppComponent} from './app.component';
import { BootstrapModalModule } from 'ng2-bootstrap-modal';
import { ConfirmComponent } from './components/confirm.component';
import { FileUploadModule } from 'ng2-file-upload';
import {Ng2Webstorage} from 'ng2-webstorage';
import {WindowsInjetor,WindowBackdrop,BootstrapWindowContainer} from "./components/ng-window";
// Layouts 
import {HeaderComponent} from './layouts/header/header.component';
import {AppLayoutComponent} from './layouts/app-layout.component';
import {SidebarComponent} from './layouts/sidebar/sidebar.component';
// end of layouts
import {DashboardLinksComponent} from './pages/dashboard-links/dashboard-links.component';
import {LoginComponent} from './pages/login/login.component';
import {RegisterComponent} from './pages/register/register.component';
import {MainComponent} from './pages/main/main.component';
import {PasswordResetComponent} from './pages/password-reset/password-reset.component';
import {ChangePasswordComponent} from './pages/change-password/change-password.component';
import {Error404Component} from './pages/error404/error404.component';
import {RoutingModule} from './app.routing';
import {RouterModule, ActivatedRouteSnapshot, RouterStateSnapshot, PreloadAllModules} from '@angular/router';
import {ProfileComponent} from './pages/profile/profile.component';
import { DiaryInputComponent } from './components/diary-input/diary-input.component';
import { DiaryResultsComponent } from './components/diary-results/diary-results.component';
import { DiaryComponent } from './pages/diary/diary.component';
import { UnpaidScriptComponent } from './pages/unpaid-script/unpaid-script.component';
 // services
import { DiaryService, HttpService, AuthGuard, ProfileManager, EventsService, ClaimManager, 
  PaymentScriptService,UnpaidScriptService} from './services/services.barrel';
import { PayorsComponent } from './pages/payors/payors.component';
import { ClaimsComponent } from './pages/claim/claim.component';
import { ClaimSearchComponent } from './components/claim-search/claim-search.component';
import { ClaimResultComponent } from './components/claim-result/claim-result.component';
import { ClaimPaymentComponent } from './components/claim-payment/claim-payment.component';
import { ClaimImagesComponent } from './components/claim-images/claim-images.component';
import { ClaimPrescriptionsComponent } from './components/claim-prescriptions/claim-prescriptions.component';
import { ClaimNoteComponent } from './components/claim-note/claim-note.component';
import { ClaimEpisodeComponent } from './components/claim-episode/claim-episode.component';
import { ClaimScriptNoteComponent } from './components/claim-script-note/claim-script-note.component';
import { UsersComponent } from './pages/users/users.component';
import { DisplayRolesPipe } from './pipes/display-roles.pipe';
import { ConfirmEmailComponent } from './pages/confirm-email/confirm-email.component';
import { FilterUserPipe } from './pages/users/filter-user.pipe';
import { DiariesFilterPipe } from './components/diary-results/diary-filter.pipe';
import { DecimalPipe } from '@angular/common';
import { PaymentInvoiceComponent, PaymentInputComponent, PaymentResultComponent, PaymentClaimResultComponent,
  PaymentDetailedResultComponent,DiaryScriptNoteWindowComponent,UnpaidScriptResultsComponent,UnpaidScriptSearchComponent} from './components/components-barrel';
import { FileUploadComponent } from './pages/file-upload/file-upload.component';
import { FooterComponent } from './layouts/footer/footer.component';
import { PaymentComponent } from './pages/payment/payment.component';
import { PaymentService} from './services/payment-service';
import { ColumnSortDirective } from './directives/column-sort.directive';
import { TableSortDirective } from './directives/table-sort.directive';
 
@Pipe({name: 'safeStyle'})
export class SafeStylePipe implements PipeTransform {
  constructor(private sanitized: DomSanitizer) {}

  transform(value: any) {
    return this.sanitized.bypassSecurityTrustStyle(value);
  }
}
@Pipe({name: 'safeURL'})
export class SafeUrlPipe implements PipeTransform {
  constructor(private sanitized: DomSanitizer) {}
  transform(value: any) {
    return this.sanitized.bypassSecurityTrustUrl(value);
  }
}
@NgModule({
  declarations: [
    AppComponent,
    WindowBackdrop,
    BootstrapWindowContainer,
    ConfirmComponent,
    AppLayoutComponent,
    Error404Component,
    HeaderComponent,
    LoginComponent,
    LoginComponent,
    MainComponent,
    PasswordResetComponent,
    RegisterComponent,
    DisplayRolesPipe, SafeStylePipe, SafeUrlPipe, ClaimsComponent, ProfileComponent,
    SidebarComponent, DashboardLinksComponent, PayorsComponent, ClaimSearchComponent, ClaimResultComponent, ClaimPaymentComponent,
     ClaimImagesComponent, ClaimPrescriptionsComponent, ClaimNoteComponent, ClaimEpisodeComponent, ClaimScriptNoteComponent,
     UsersComponent, ChangePasswordComponent, ConfirmEmailComponent, FilterUserPipe,DiariesFilterPipe, FileUploadComponent, FooterComponent,
     PaymentComponent, PaymentInvoiceComponent, PaymentInputComponent, PaymentResultComponent, PaymentClaimResultComponent,
     PaymentDetailedResultComponent,
    ColumnSortDirective, TableSortDirective,
    DiaryComponent, DiaryInputComponent, DiaryResultsComponent, DiaryScriptNoteWindowComponent,
    UnpaidScriptComponent,UnpaidScriptResultsComponent,UnpaidScriptSearchComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ToastModule.forRoot(),
    BootstrapModalModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    RoutingModule,
    FileUploadModule,
    Ng2Webstorage
  ],
  providers: [
    DecimalPipe, DatePipe, HttpService, ProfileManager, EventsService, AuthGuard, ClaimManager, PaymentService,
    PaymentScriptService, DiaryService,WindowsInjetor,UnpaidScriptService,
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy
    }],
  entryComponents: [
      ConfirmComponent,BootstrapWindowContainer,WindowBackdrop,DiaryScriptNoteWindowComponent,AppComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
