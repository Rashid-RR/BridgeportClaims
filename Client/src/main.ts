import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from 'app/app.module';
import { environment } from 'environments/environment.prod';
import { LicenseManager } from 'ag-grid-enterprise';

LicenseManager.setLicenseKey('Evaluation_License-_Not_For_Production_Valid_Until_4_August_2019__MTU2NDg3MzIwMDAwMA==0c1afcd1a9474154203be6f2b7946940');
// bootstrap your angular application. ie: platformBrowser().bootstrapModuleFactory(..)
if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule);
