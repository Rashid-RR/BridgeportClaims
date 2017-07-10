import {BrowserModule, DomSanitizer} from "@angular/platform-browser";
import {NgModule, Pipe, PipeTransform} from "@angular/core";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpModule} from "@angular/http";
import {HashLocationStrategy, LocationStrategy,DatePipe} from "@angular/common";
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {ToastModule} from 'ng2-toastr/ng2-toastr';
import {AppComponent} from "./app.component";
import { BootstrapModalModule } from 'ng2-bootstrap-modal';
import { ConfirmComponent } from './components/confirm.component';
import { FileUploadModule } from 'ng2-file-upload';

//Layouts 
import {HeaderComponent} from "./layouts/header/header.component";
import {AppLayoutComponent} from "./layouts/app-layout.component";
import {SidebarComponent} from "./layouts/sidebar/sidebar.component";
//end of layouts
import {PrivateComponent} from "./pages/private/private.component";
import {LoginComponent} from "./pages/login/login.component";
import {RegisterComponent} from "./pages/register/register.component";
import {MainComponent} from "./pages/main/main.component";
import {PasswordResetComponent} from "./pages/password-reset/password-reset.component";
import {ChangePasswordComponent} from "./pages/change-password/change-password.component";
import {Error404Component} from "./pages/error404/error404.component";
import {RoutingModule} from "./app.routing";
import {RouterModule, ActivatedRouteSnapshot,RouterStateSnapshot,PreloadAllModules} from "@angular/router";
import {ProfileComponent} from "./pages/profile/profile.component";

 //services
import {HttpService,AuthGuard,ProfileManager,EventsService,ClaimManager} from "./services/services.barrel";
import { PayorsComponent } from './pages/payors/payors.component'
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
import { FileUploadComponent } from './pages/file-upload/file-upload.component';


@Pipe({name: 'safeStyle'})
export class SafeStylePipe implements PipeTransform {
  constructor(private sanitized: DomSanitizer) {}

  transform(value:any) {
    return this.sanitized.bypassSecurityTrustStyle(value);
  }
}
@Pipe({name: 'safeURL'})
export class SafeUrlPipe implements PipeTransform {
  constructor(private sanitized: DomSanitizer) {}
  transform(value:any) {
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
    SafeStylePipe, SafeUrlPipe, ClaimsComponent,ProfileComponent,
    SidebarComponent, PrivateComponent, PayorsComponent, ClaimSearchComponent, ClaimResultComponent, ClaimPaymentComponent, ClaimImagesComponent, ClaimPrescriptionsComponent, ClaimNoteComponent, ClaimEpisodeComponent, ClaimScriptNoteComponent, UsersComponent,ChangePasswordComponent, ConfirmEmailComponent,FilterUserPipe, FileUploadComponent
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
    FileUploadModule
  ],
  providers: [  
    DatePipe,HttpService,ProfileManager,EventsService,AuthGuard, ClaimManager,
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy
    }],
  entryComponents: [
        ConfirmComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
