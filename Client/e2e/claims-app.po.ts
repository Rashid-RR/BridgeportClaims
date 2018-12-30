import { browser, element, by, protractor } from 'protractor';
import { environment as dev } from 'environments/environment';
import { environment as local } from 'environments/environment.local';
import { environment as prod } from 'environments/environment.prod';
export class ClaimsPage {
  environment: any;
  constructor() {
    this.environment = browser.params && browser.params.env && browser.params.env == 'prod' ?
      prod : (browser.params && browser.params.env && browser.params.env == 'dev' ? dev : local);

  }
  navigateToLogin() {
    return browser.get('/#/login');
  }
  navigateToClaims() {
    return browser.get('/#/main/claims');
  }
  getFirstClaim() {
    const elem = element.all(by.className('view-btn')).first();
    const until = protractor.ExpectedConditions;
    return browser.wait(until.visibilityOf(elem), 30000, 'GetClaimsData API took too long to load');
  }
  loadClaim() {
    const elem = element.all(by.className('view-btn')).first();
    return elem.click();
  }
  checkClaimLoaded() {
    return browser.driver.wait(() => {
      const until = protractor.ExpectedConditions;
      const elem = element(by.id('claimLoader'));
      return browser.wait(until.presenceOf(elem), 30000, 'Claim took too long to appear in the DOM');
    });
  }
  loadClaims() {
    this.navigateToClaims();
    const firstnameEl = element(by.name('firstName')),
      searchEl = element(by.id('claimSearchBtn'));
    firstnameEl.sendKeys(this.environment.vars.firstName);
    const until = protractor.ExpectedConditions;
    searchEl.click().then(r => {
      this.getFirstClaim();
    });
  }
  login() {
    const emailEl = element(by.name('email')),
      passwordEl = element(by.name('password')),
      loginEl = element(by.buttonText('Sign in'));
    emailEl.sendKeys(this.environment.vars.testEmail);
    passwordEl.sendKeys(this.environment.vars.testPWD);
    loginEl.click().then(r => {
      return browser.driver.wait(function () {
        return browser.driver.getCurrentUrl().then(function (url) {
          return /#\/main\/private/.test(url);
        });
      }, 50000);
    });

  }
}
