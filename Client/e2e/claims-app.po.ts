import { browser, element, by, protractor } from 'protractor';
import { vars } from "./test-vars"
export class ClaimsPage { 
  constructor() { 
  }
  navigateToLogin() {
    return browser.get('/#/login');
  }
  navigateToClaims() {
    return browser.get('/#/main/claims');
  }
  getFirstClaim() {
    let elem = element.all(by.className('view-btn')).first();
    var until = protractor.ExpectedConditions;
    return browser.wait(until.visibilityOf(elem), 30000, 'GetClaimsData API took too long to load')
  }
  loadClaim() {
    let elem = element.all(by.className('view-btn')).first();
    return elem.click() 
  }
  checkClaimLoaded() {
    return browser.driver.wait(() => {
      var until = protractor.ExpectedConditions;
      let elem = element(by.id('claimLoader'));
      return browser.wait(until.presenceOf(elem), 30000, 'Claim took too long to appear in the DOM');
    });
  }
  loadClaims() {
    this.navigateToClaims();
    let firstnameEl = element(by.name('firstName')),
      searchEl = element(by.id('claimSearchBtn'));
    firstnameEl.sendKeys(vars.firstName);
    var until = protractor.ExpectedConditions;
    searchEl.click().then(r => {
      this.getFirstClaim();
    });
  }
  login() {
    let emailEl = element(by.name('email')),
      passwordEl = element(by.name('password')),
      loginEl = element(by.buttonText('Sign in'));
    emailEl.sendKeys(vars.testEmail);
    passwordEl.sendKeys(vars.testPWD);
    loginEl.click().then(r => {
      return browser.driver.wait(function () {
        return browser.driver.getCurrentUrl().then(function (url) {
          return /#\/main\/private/.test(url);
        });
      }, 50000);
    })

  }
}
