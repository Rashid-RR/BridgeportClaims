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
 import {HttpService,AuthGuard,ProfileManager,EventsService} from "./services/services.barrel";
import { PayorsComponent } from './pages/payors/payors.component'
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
    SafeStylePipe, SafeUrlPipe, 
    SidebarComponent, PrivateComponent, PayorsComponent, 
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule, 
    RoutingModule, 
  ],
  providers: [  
    HttpService,ProfileManager,EventsService,AuthGuard,     
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy
    }],
  bootstrap: [AppComponent]
})
export class AppModule {}
