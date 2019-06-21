import { DatePipe, DecimalPipe, HashLocationStrategy, LocationStrategy } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BootstrapModalModule } from 'ng2-bootstrap-modal';
import { FileUploadModule } from 'ng2-file-upload';
import { ToastrModule } from 'ngx-toastr';
import { NgxWebstorageModule } from 'ngx-webstorage';
import { AppComponent } from './app.component';
import { RoutingModule } from './app.routing';
import { AcquireEpisodeComponent } from './components/acquire-episode/acquire-episode.component';
import { AdjustorModalComponent } from './components/adjustor-modal/adjustor-modal.component';
import { AttorneyModalComponent } from './components/attorney-modal/attorney-modal.component';
import { CarrierModalComponent } from './components/carrier-modal/carrier-modal.component';
import { ClaimEpisodeComponent } from './components/claim-episode/claim-episode.component';
import { ClaimImagesComponent } from './components/claim-images/claim-images.component';
import { ClaimNoteComponent } from './components/claim-note/claim-note.component';
import { ClaimOutstandingComponent } from './components/claim-outstanding/claim-outstanding.component';
import { ClaimPaymentComponent } from './components/claim-payment/claim-payment.component';
import { ClaimPrescriptionsComponent } from './components/claim-prescriptions/claim-prescriptions.component';
import { ClaimResultComponent } from './components/claim-result/claim-result.component';
import { ClaimScriptNoteComponent } from './components/claim-script-note/claim-script-note.component';
import { ClaimSearchComponent } from './components/claim-search/claim-search.component';
import { DecisionTreeModalComponent } from './components/decesiontree-modal/decesiontree-modal.component';
import { EpisodeFilterComponent } from './components/episode-filter/episode-filter.component';
import { EpisodeResultsComponent } from './components/episode-results/episode-results.component';
import { FirewallFilterComponent } from './components/firewall-filter/firewall-filter.component';
import { FirewallGridComponent } from './components/firewall-grid/firewall-grid.component';
import { OutstandingFilterComponent } from './components/outstanding-filter/outstanding-filter.component';
import { OutstandingResultComponent } from './components/outstanding-result/outstanding-result.component';
import { AppLayoutComponent } from './layouts/app-layout.component';
import { FooterComponent } from './layouts/footer/footer.component';
/* import { SignalRModule ,SignalRConfiguration } from 'ng2-signalr'; */
// Layouts
import { HeaderComponent } from './layouts/header/header.component';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { SidebarComponent } from './layouts/sidebar/sidebar.component';
import { MaterialComponentsModule } from './material/material-components.module';
import { ChangePasswordComponent } from './pages/change-password/change-password.component';
import { ClaimsComponent } from './pages/claim/claim.component';
import { ConfirmEmailComponent } from './pages/confirm-email/confirm-email.component';
import { EpisodePageComponent } from './pages/episode-page/episode-page.component';
import { Error404Component } from './pages/error404/error404.component';
import { FileUploadComponent } from './pages/file-upload/file-upload.component';
import { FirewallSettingsComponent } from './pages/firewall-settings/firewall-settings.component';
import { IndexFileComponent } from './pages/index-file/index-file.component';
// end of layouts
import { LoginComponent } from './pages/login/login.component';
import { MainComponent } from './pages/main/main.component';
import { PasswordResetComponent } from './pages/password-reset/password-reset.component';
import { PayorsComponent } from './pages/payors/payors.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { RegisterComponent } from './pages/register/register.component';
import { TestComponent } from './pages/test/test.component';
import { UnindexedImageComponent } from './pages/unindex-image/unindex-image.component';
import { UnindexedImageFileListComponent } from './pages/unindexed-image-file-list/unindexed-image-file-list.component';
import { UnindexedImageFileComponent } from './pages/unindexed-image-file/unindexed-image-file.component';
import { UnpaidScriptComponent } from './pages/unpaid-script/unpaid-script.component';
import { EpisodeService } from './services/episode.service';
import { PaymentService } from './services/payment-service';
import { ReferenceManagerService } from './services/reference-manager.service';
// services
import { AccountReceivableService, AuthGuard, AuthInterceptor, ClaimManager, CollectionBonusService,
  DecisionTreeService, DiaryService, DocumentManagerService, EventsService, FirewallService, HttpService,
  PaymentScriptService, ProfileManager, ReportLoaderService, ShortPayService, SignalRService,
  SkippedPaymentService, UnpaidScriptService, WINDOW_PROVIDERS, QueryBuilderService, AddressEditService, InvoicesService } from './services/services.barrel';
import { StringService } from './services/string.service';
// import { SnotifyModule, SnotifyService, ToastDefaults } from 'ng-snotify';
import { SharedModule } from './shared';
import { AgPhoneNumberMaskComponent } from './components/ag-phone-number-mask/ag-phone-number-mask.component';
import { PopoverModule } from 'ngx-bootstrap/popover';

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
    CarrierModalComponent, AdjustorModalComponent, AttorneyModalComponent, DecisionTreeModalComponent, AgPhoneNumberMaskComponent
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
    BootstrapModalModule.forRoot({container: document.body}),
    MatSlideToggleModule,
    PopoverModule.forRoot()
  ],
  providers: [
    WINDOW_PROVIDERS,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    // { provide: 'SnotifyToastConfig', useValue: ToastDefaults},SnotifyService,
    DecimalPipe, DatePipe, CollectionBonusService, ReferenceManagerService,
    HttpService, ProfileManager, EventsService, AuthGuard, ClaimManager, PaymentService, DocumentManagerService,
    EpisodeService, FirewallService, DecisionTreeService,
    PaymentScriptService, DiaryService, ShortPayService, SkippedPaymentService, UnpaidScriptService,
    AccountReceivableService, ReportLoaderService, SignalRService, StringService, QueryBuilderService, AddressEditService, InvoicesService,
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy
    }
  ],
  entryComponents: [
    UnindexedImageFileComponent,
    CarrierModalComponent, AdjustorModalComponent, AttorneyModalComponent, DecisionTreeModalComponent, AppComponent, AgPhoneNumberMaskComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

}
