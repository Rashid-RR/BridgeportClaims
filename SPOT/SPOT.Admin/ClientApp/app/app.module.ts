import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { AppLayoutComponent } from './app-layout/app-layout.component';
import { SharedModule } from './shared/shared.module';
import {CustomerService} from "./shared"
@NgModule({
  declarations: [
    AppComponent,
    AppLayoutComponent    
  ],
  imports: [
    BrowserModule,BrowserAnimationsModule,
    AppRoutingModule,SharedModule
  ],
  providers: [CustomerService],
  bootstrap: [AppComponent]
})
export class AppModule { }
