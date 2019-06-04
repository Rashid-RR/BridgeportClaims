import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from 'app/app.module';
import { environment } from 'environments/environment.prod';
import { LicenseManager } from 'ag-grid-enterprise';

LicenseManager.setLicenseKey('');
// bootstrap your angular application. ie: platformBrowser().bootstrapModuleFactory(..)
if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule);
