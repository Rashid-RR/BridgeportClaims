import { BrowserModule } from '@angular/platform-browser';
import { NgModule, } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { DatePipe, DecimalPipe, HashLocationStrategy, LocationStrategy } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { FileUploadModule } from 'ng2-file-upload';
import { NgxWebstorageModule } from 'ngx-webstorage';
import { BootstrapModalModule } from 'ng2-bootstrap-modal';
import { ToastrModule } from 'ngx-toastr';
// import { SnotifyModule, SnotifyService, ToastDefaults } from 'ng-snotify';
import { SharedModule } from './shared';
/* import { SignalRModule ,SignalRConfiguration } from 'ng2-signalr'; */
// Layouts
import { HeaderComponent } from './layouts/header/header.component';
import { AppLayoutComponent } from './layouts/app-layout.component';
import { SidebarComponent } from './layouts/sidebar/sidebar.component';
// end of layouts
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { MainComponent } from './pages/main/main.component';
import { PasswordResetComponent } from './pages/password-reset/password-reset.component';
import { ChangePasswordComponent } from './pages/change-password/change-password.component';
import { Error404Component } from './pages/error404/error404.component';
import { RoutingModule } from './app.routing';
import { ProfileComponent } from './pages/profile/profile.component';
import { UnpaidScriptComponent } from './pages/unpaid-script/unpaid-script.component';
import { MaterialComponentsModule } from './material/material-components.module';
// services
import {
  AccountReceivableService,
  AuthGuard, DecisionTreeService,
  AuthInterceptor,
  ClaimManager,
  CollectionBonusService,
  DiaryService,
  DocumentManagerService,
  EventsService,
  FirewallService,
  HttpService,
  PaymentScriptService,
  ProfileManager,
  ReportLoaderService,
  ShortPayService,
  SignalRService,
  SkippedPaymentService,
  UnpaidScriptService, WINDOW_PROVIDERS
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
import { ConfirmEmailComponent } from './pages/confirm-email/confirm-email.component';
import { FileUploadComponent } from './pages/file-upload/file-upload.component';
import { FooterComponent } from './layouts/footer/footer.component';
import { PaymentService } from './services/payment-service';
import { UnindexedImageComponent } from './pages/unindex-image/unindex-image.component';
import { UnindexedImageFileComponent } from './pages/unindexed-image-file/unindexed-image-file.component';
import { IndexFileComponent } from './pages/index-file/index-file.component';
import { UnindexedImageFileListComponent } from './pages/unindexed-image-file-list/unindexed-image-file-list.component';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { EpisodePageComponent } from './pages/episode-page/episode-page.component';
import { EpisodeResultsComponent } from './components/episode-results/episode-results.component';
import { EpisodeFilterComponent } from './components/episode-filter/episode-filter.component';
import { EpisodeService } from './services/episode.service';
import { FirewallSettingsComponent } from './pages/firewall-settings/firewall-settings.component';
import { FirewallFilterComponent } from './components/firewall-filter/firewall-filter.component';
import { FirewallGridComponent } from './components/firewall-grid/firewall-grid.component';
import { AcquireEpisodeComponent } from './components/acquire-episode/acquire-episode.component';
import { TestComponent } from './pages/test/test.component';
import { OutstandingFilterComponent } from './components/outstanding-filter/outstanding-filter.component';
import { OutstandingResultComponent } from './components/outstanding-result/outstanding-result.component';
import { ClaimOutstandingComponent } from './components/claim-outstanding/claim-outstanding.component';
import { StringService } from './services/string.service';
import { ReferenceManagerService } from './services/reference-manager.service';
import { CarrierModalComponent } from './components/carrier-modal/carrier-modal.component';
import { AdjustorModalComponent } from './components/adjustor-modal/adjustor-modal.component';
import { AttorneyModalComponent } from './components/attorney-modal/attorney-modal.component';
import { FormsModule } from '@angular/forms';
import { DecisionTreeModalComponent } from './components/decesiontree-modal/decesiontree-modal.component';
import { MatSlideToggleModule } from '@angular/material';


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
    RegisterComponent,
    ClaimsComponent, ProfileComponent,
    SidebarComponent, PayorsComponent, ClaimSearchComponent, ClaimResultComponent, ClaimPaymentComponent,
    ClaimImagesComponent, ClaimPrescriptionsComponent, ClaimNoteComponent, ClaimEpisodeComponent, ClaimScriptNoteComponent,
    ChangePasswordComponent, ConfirmEmailComponent, FileUploadComponent, FooterComponent,
    UnpaidScriptComponent, UnindexedImageComponent,
    UnindexedImageFileComponent, IndexFileComponent, UnindexedImageFileListComponent, MainLayoutComponent, EpisodePageComponent,
    EpisodeResultsComponent, EpisodeFilterComponent, FirewallSettingsComponent, FirewallFilterComponent,
    FirewallGridComponent, AcquireEpisodeComponent, TestComponent, OutstandingFilterComponent, OutstandingResultComponent,
    ClaimOutstandingComponent,
    CarrierModalComponent, AdjustorModalComponent, AttorneyModalComponent, DecisionTreeModalComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    SharedModule,
    FormsModule,
    HttpClientModule,
    RoutingModule,
    FileUploadModule,
    MaterialComponentsModule,
    NgxWebstorageModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-center',
      timeOut: 15000
    }),
    BootstrapModalModule.forRoot({container:document.body}),
    MatSlideToggleModule
  ],
  providers: [
    WINDOW_PROVIDERS,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    // { provide: 'SnotifyToastConfig', useValue: ToastDefaults},SnotifyService,
    DecimalPipe, DatePipe, CollectionBonusService, ReferenceManagerService,
    HttpService, ProfileManager, EventsService, AuthGuard, ClaimManager, PaymentService, DocumentManagerService,
    EpisodeService, FirewallService, DecisionTreeService,
    PaymentScriptService, DiaryService, ShortPayService, SkippedPaymentService, UnpaidScriptService,
    AccountReceivableService, ReportLoaderService, SignalRService, StringService,
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy
    }
  ],
  entryComponents: [
    UnindexedImageFileComponent,
    CarrierModalComponent, AdjustorModalComponent, AttorneyModalComponent,DecisionTreeModalComponent, AppComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

}
