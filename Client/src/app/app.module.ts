import {BrowserModule, DomSanitizer} from "@angular/platform-browser";
import {NgModule, Pipe, PipeTransform} from "@angular/core";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpModule} from "@angular/http";
import {HashLocationStrategy, LocationStrategy} from "@angular/common";
import {AppComponent} from "./app.component";
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
import {Error404Component} from "./pages/error404/error404.component";
import {RoutingModule} from "./app.routing";
import {RouterModule, ActivatedRouteSnapshot,RouterStateSnapshot,PreloadAllModules} from "@angular/router";


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
    AppLayoutComponent,
    Error404Component, 
    HeaderComponent, 
    LoginComponent,
    LoginComponent,
    MainComponent,
    PasswordResetComponent, 
    RegisterComponent,
    SafeStylePipe, SafeUrlPipe, ClaimsComponent,
    SidebarComponent, PrivateComponent, PayorsComponent, ClaimSearchComponent, ClaimResultComponent, ClaimPaymentComponent, ClaimImagesComponent, ClaimPrescriptionsComponent, ClaimNoteComponent, ClaimEpisodeComponent, ClaimScriptNoteComponent, UsersComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule, 
    RoutingModule
  ],
  providers: [  
    HttpService,ProfileManager,EventsService,AuthGuard, ClaimManager,
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy
    }],
  bootstrap: [AppComponent]
})
export class AppModule {}
