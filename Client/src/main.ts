import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from 'app/app.module';
import { environment } from 'environments/environment.prod';
import { LicenseManager } from 'ag-grid-enterprise';

LicenseManager.setLicenseKey('Bridgeport_Claims_BridgeportClaims_1Devs29_July_2020__MTU5NTk3NzIwMDAwMA==5d7412fedbbf766b48312e611123d50d');
// bootstrap your angular application. ie: platformBrowser().bootstrapModuleFactory(..)
if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule);
