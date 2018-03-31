import { browser, element, by, protractor } from 'protractor';
import { vars } from "./test-vars"
export class ClaimsPage {
  navigateToLogin() {
    return browser.get('/#/login');
  }
  navigateToClaims() {
    return browser.get('/#/main/claims');

  }
  loadClaims() {
    this.navigateToClaims();
    let firstnameEl = element(by.name('firstName')),
      searchEl = element(by.id('claimSearchBtn'));
    firstnameEl.sendKeys(vars.firstName);
    var until = protractor.ExpectedConditions;
    searchEl.click().then(r => {
      let elem = element(by.id('claimLoader'));
      browser.wait(until.presenceOf(elem), 30000, 'Element taking too long to appear in the DOM');
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
