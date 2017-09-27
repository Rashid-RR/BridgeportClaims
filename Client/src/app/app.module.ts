import { BrowserModule, DomSanitizer } from '@angular/platform-browser';
import { NgModule, Pipe, PipeTransform } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HashLocationStrategy, LocationStrategy, DatePipe, DecimalPipe } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, ActivatedRouteSnapshot, RouterStateSnapshot, PreloadAllModules } from '@angular/router';

// Third Party Modules
import { ToastModule } from 'ng2-toastr/ng2-toastr';
import { BootstrapModalModule } from 'ng2-bootstrap-modal';
import { FileUploadModule } from 'ng2-file-upload';
import { Ng2Webstorage } from 'ng2-webstorage';

// Routing
import { RoutingModule } from './app.routing';

// Services
import { HttpService, AuthGuard, ProfileManager, EventsService, ClaimManager } from './services/services.barrel';
import { PaymentService } from './services/payment-service';
import { DiaryService } from './services/diary.service';

// Directives
import { ColumnSortDirective } from './directives/column-sort.directive';
import { TableSortDirective } from './directives/table-sort.directive';

// Pipes
import { DisplayRolesPipe } from './pipes/display-roles.pipe';

// Components
import { AppComponent } from './app.component';
import { ConfirmComponent } from './components/confirm.component';
import { ClaimSearchComponent } from './components/claim-search/claim-search.component';
import { ClaimResultComponent } from './components/claim-result/claim-result.component';
import { ClaimPaymentComponent } from './components/claim-payment/claim-payment.component';
import { ClaimImagesComponent } from './components/claim-images/claim-images.component';
import { ClaimPrescriptionsComponent } from './components/claim-prescriptions/claim-prescriptions.component';
import { ClaimNoteComponent } from './components/claim-note/claim-note.component';
import { ClaimEpisodeComponent } from './components/claim-episode/claim-episode.component';
import { ClaimScriptNoteComponent } from './components/claim-script-note/claim-script-note.component';
import { PaymentInvoiceComponent, PaymentInputComponent, PaymentResultComponent, PaymentClaimResultComponent, PaymentDetailedResultComponent, SelectedPaymentDetailedResultComponent } from './components/components-barrel';
import { DiaryInputComponent } from './components/diary-input/diary-input.component';
import { DiaryResultsComponent } from './components/diary-results/diary-results.component';
// Components - Layouts 
import { HeaderComponent } from './layouts/header/header.component';
import { AppLayoutComponent } from './layouts/app-layout.component';
import { SidebarComponent } from './layouts/sidebar/sidebar.component';
import { FooterComponent } from './layouts/footer/footer.component';
// Component - Pages
import { PrivateComponent } from './pages/private/private.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { MainComponent } from './pages/main/main.component';
import { PasswordResetComponent } from './pages/password-reset/password-reset.component';
import { ChangePasswordComponent } from './pages/change-password/change-password.component';
import { Error404Component } from './pages/error404/error404.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { PayorsComponent } from './pages/payors/payors.component'
import { ClaimsComponent } from './pages/claim/claim.component';
import { UsersComponent } from './pages/users/users.component';
import { ConfirmEmailComponent } from './pages/confirm-email/confirm-email.component';
import { FilterUserPipe } from './pages/users/filter-user.pipe';
import { FileUploadComponent } from './pages/file-upload/file-upload.component';
import { PaymentComponent } from './pages/payment/payment.component';
import { DiaryComponent } from './pages/diary/diary.component';

@Pipe({ name: 'safeStyle' })
export class SafeStylePipe implements PipeTransform {
  constructor(private sanitized: DomSanitizer) { }

  transform(value: any) {
    return this.sanitized.bypassSecurityTrustStyle(value);
  }
}

@Pipe({ name: 'safeURL' })
export class SafeUrlPipe implements PipeTransform {
  constructor(private sanitized: DomSanitizer) { }
  transform(value: any) {
    return this.sanitized.bypassSecurityTrustUrl(value);
  }
}

@NgModule({
  declarations: [
    AppComponent,
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
    SidebarComponent, PrivateComponent, PayorsComponent, ClaimSearchComponent, ClaimResultComponent, ClaimPaymentComponent, ClaimImagesComponent, ClaimPrescriptionsComponent, ClaimNoteComponent, ClaimEpisodeComponent, ClaimScriptNoteComponent, UsersComponent, ChangePasswordComponent, ConfirmEmailComponent, FilterUserPipe, FileUploadComponent, FooterComponent, PaymentComponent, PaymentInvoiceComponent, PaymentInputComponent, PaymentResultComponent, PaymentClaimResultComponent, PaymentDetailedResultComponent,
    SelectedPaymentDetailedResultComponent,
    ColumnSortDirective, TableSortDirective, DiaryComponent, DiaryInputComponent, DiaryResultsComponent
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
    DecimalPipe,
    DatePipe,
    HttpService,
    ProfileManager,
    EventsService,
    AuthGuard,
    ClaimManager,
    PaymentService,
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy
    },
    DiaryService
  ],

  entryComponents: [
    ConfirmComponent
  ],

  bootstrap: [AppComponent]
})
export class AppModule { }
