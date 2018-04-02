import { BrowserModule } from '@angular/platform-browser';
import { NgModule, } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HashLocationStrategy, LocationStrategy, DecimalPipe, DatePipe } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';  
import { FileUploadModule } from 'ng2-file-upload';
import { Ng2Webstorage } from 'ng2-webstorage';  
import { SweetAlert2Module } from '@toverux/ngx-sweetalert2';
import { BootstrapModalModule } from 'ng2-bootstrap-modal';
import { ToastModule } from 'ng2-toastr/ng2-toastr';
import { SharedModule } from './shared';

/* import { SignalRModule ,SignalRConfiguration } from 'ng2-signalr'; */
 // Layouts
import { HeaderComponent } from './layouts/header/header.component';
import { AppLayoutComponent } from './layouts/app-layout.component';
import { SidebarComponent } from './layouts/sidebar/sidebar.component';
// end of layouts
import { DashboardLinksComponent } from './pages/dashboard-links/dashboard-links.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { MainComponent } from './pages/main/main.component';
import { PasswordResetComponent } from './pages/password-reset/password-reset.component';
import { ChangePasswordComponent } from './pages/change-password/change-password.component';
import { Error404Component } from './pages/error404/error404.component';
import { RoutingModule } from './app.routing';
import { RouterModule, ActivatedRouteSnapshot, RouterStateSnapshot, PreloadAllModules } from '@angular/router';
import { ProfileComponent } from './pages/profile/profile.component';
import { UnpaidScriptComponent } from './pages/unpaid-script/unpaid-script.component';
// services
import {
  SignalRService, DiaryService, HttpService, AuthGuard, ProfileManager, EventsService, ClaimManager,FirewallService,
  PaymentScriptService, UnpaidScriptService, AccountReceivableService, ReportLoaderService, DocumentManagerService
} from './services/services.barrel';
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
import { ConfirmEmailComponent } from './pages/confirm-email/confirm-email.component';
/*  import {  UnpaidScriptResultsComponent, UnpaidScriptSearchComponent,
  AccountReceivableSearchComponent, AccountReceivableResultComponent,EpisodeNoteModalComponent,
  UnindexedImageFilterComponent, UnindexedImageListComponent,ScriptNoteWindowComponent
} from './components/components-barrel'; */
import { FileUploadComponent } from './pages/file-upload/file-upload.component';
import { FooterComponent } from './layouts/footer/footer.component';
import { PaymentService } from './services/payment-service'; 
import { ReportComponent } from './pages/report/report.component';
import { ReportListComponent } from './pages/report-list/report-list.component';
import { ReportSampleComponent } from './pages/report-sample/report-sample.component';
import { ReportAccountReceivableComponent } from './pages/report-account-receivable/report-account-receivable.component';
import { UnindexedImageComponent } from './pages/unindex-image/unindex-image.component';
import { UnindexedImageFileComponent } from './pages/unindexed-image-file/unindexed-image-file.component';
import { IndexFileComponent } from './pages/index-file/index-file.component';
import { UnindexedImageFileListComponent } from './pages/unindexed-image-file-list/unindexed-image-file-list.component';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { EpisodePageComponent } from './pages/episode-page/episode-page.component';
import { EpisodeResultsComponent } from './components/episode-results/episode-results.component';
import { EpisodeFilterComponent } from './components/episode-filter/episode-filter.component';
import { EpisodeService } from 'app/services/episode.service';
import { NewEpisodeComponent } from './components/new-episode/new-episode.component';
import { FirewallSettingsComponent } from './pages/firewall-settings/firewall-settings.component';
import { FirewallFilterComponent } from './components/firewall-filter/firewall-filter.component';
import { FirewallGridComponent } from './components/firewall-grid/firewall-grid.component';
import { AcquireEpisodeComponent } from './components/acquire-episode/acquire-episode.component';
import { TestComponent } from './pages/test/test.component';
import { NotificationComponent } from './components/notification/notification.component';
 

@NgModule({
  declarations: [
    AppComponent,
    AppLayoutComponent,
    Error404Component,
    HeaderComponent,
    LoginComponent,
    LoginComponent,
    MainComponent,
    PasswordResetComponent,
    RegisterComponent, ReportComponent,
    /* PhonePipe, DisplayRolesPipe,ArraySortPipe, SafeStylePipe, SafeUrlPipe, */ ClaimsComponent, ProfileComponent,
    SidebarComponent, DashboardLinksComponent, PayorsComponent, ClaimSearchComponent, ClaimResultComponent, ClaimPaymentComponent,
    ClaimImagesComponent, ClaimPrescriptionsComponent, ClaimNoteComponent, ClaimEpisodeComponent, ClaimScriptNoteComponent,
    UsersComponent, ChangePasswordComponent, ConfirmEmailComponent,  FileUploadComponent, FooterComponent,  
    UnpaidScriptComponent,  ReportListComponent, ReportSampleComponent, ReportAccountReceivableComponent,  UnindexedImageComponent,
    UnindexedImageFileComponent, IndexFileComponent, UnindexedImageFileListComponent, MainLayoutComponent, EpisodePageComponent, EpisodeResultsComponent, EpisodeFilterComponent, NewEpisodeComponent, FirewallSettingsComponent, FirewallFilterComponent, FirewallGridComponent, AcquireEpisodeComponent, TestComponent, NotificationComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    SharedModule, 
    HttpModule,
    RoutingModule,
    FileUploadModule,
    Ng2Webstorage,
    SweetAlert2Module.forRoot(),
    ToastModule.forRoot(),
    BootstrapModalModule,
  ],
  providers: [
    DecimalPipe, DatePipe,
    HttpService, ProfileManager, EventsService, AuthGuard, ClaimManager, PaymentService,
    PaymentScriptService, DiaryService,  UnpaidScriptService, AccountReceivableService, ReportLoaderService, SignalRService,
    DocumentManagerService, EpisodeService,FirewallService,
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy
    }], 
  entryComponents: [
    UnindexedImageFileComponent,
    /* ConfirmComponent, BootstrapWindowContainer, WindowBackdrop, ScriptNoteWindowComponent,EpisodeNoteModalComponent,*/ AppComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

}
